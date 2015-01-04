// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.Drawing;
using AudioSwitcher.Audio;
using AudioSwitcher.Presentation.Drawing;

namespace AudioSwitcher.UI.ViewModels
{
    internal class AudioDeviceViewModel
    {
        private readonly AudioDevice _device;

        public AudioDeviceViewModel(AudioDevice device)
        {
            _device = device;
        }

        public AudioDevice Device
        {
            get { return _device; }
        }

        public AudioDeviceDefaultState DefaultState
        {
            get;
            private set;
        }

        public AudioDeviceState State
        {
            get;
            private set;
        }

        public AudioDeviceKind Kind
        {
            get;
            private set;
        }

        public string Description
        {
            get;
            private set;
        }

        public string FriendlyName
        {
            get;
            private set;
        }

        public string DeviceStateFriendlyName
        {
            get;
            private set;
        }

        public Image Image
        {
            get;
            private set;
        }

        public void UpdateStatus(AudioDeviceManager deviceManager)
        {
            DefaultState = CalculateDeviceDefaultState(deviceManager);
            Kind = _device.Kind;
            State = _device.State;
            Description = _device.DeviceDescription;
            FriendlyName = _device.DeviceFriendlyName;
            DeviceStateFriendlyName = GetDeviceStateFriendlyName();
            Image = GetImage();
        }

        private string GetDeviceStateFriendlyName()
        {
            // To mimic the Sound control panel, we display a device's 
            // default state first, and only then fall back to the actual
            // device's state if it's not a default device.

            if (DefaultState.IsSet(AudioDeviceDefaultState.All))
            {
                return Resources.DeviceState_DefaultDevice;
            }

            if (DefaultState.IsSet(AudioDeviceDefaultState.Multimedia))
            {
                return Resources.DeviceState_DefaultMultimediaDevice;
            }

            if (DefaultState.IsSet(AudioDeviceDefaultState.Communications))
            {
                return Resources.DeviceState_DefaultCommunicationsDevice;
            }

            switch (State)
            {
                case AudioDeviceState.Active:
                    return Resources.DeviceState_Active;

                case AudioDeviceState.Disabled:
                    return Resources.DeviceState_Disabled;

                case AudioDeviceState.NotPresent:
                    return Resources.DeviceState_NotPresent;

                case AudioDeviceState.Unplugged:
                    return Resources.DeviceState_Unplugged;
            }

            return String.Empty;
        }

        private Image GetImage()
        {
            string iconPath = _device.DeviceClassIconPath;

            if (String.IsNullOrEmpty(iconPath))
                return null;

            Icon icon;
            if (!ShellIcon.TryExtractIconByIdOrIndex(iconPath, new Size(48, 48), out icon))
                return null;

            using (icon)
            {
                Image overlayImage = GetOverlayImage();

                if (overlayImage != null)
                {
                    return DrawingServices.Overlay(icon.ToBitmap(), overlayImage);
                }
                else
                {
                    return icon.ToBitmap();
                }
            }
        }

        private Image GetOverlayImage()
        {
            if (DefaultState.IsSet(AudioDeviceDefaultState.Multimedia))
            {   // Sound control panel shows the same icon between all and multimedia
                return Resources.DefaultMultimediaDevice;
            }

            if (DefaultState.IsSet(AudioDeviceDefaultState.Communications))
            {
                return Resources.DefaultCommunicationsDevice;
            }

            switch (State)
            {
                case AudioDeviceState.Disabled:
                    return Resources.Disabled;

                case AudioDeviceState.NotPresent:
                    return Resources.NotPresent;

                case AudioDeviceState.Unplugged:
                    return Resources.Unplugged;
            }

            return null;
        }

        private AudioDeviceDefaultState CalculateDeviceDefaultState(AudioDeviceManager deviceManager)
        {
            AudioDeviceDefaultState state = AudioDeviceDefaultState.None;

            if (deviceManager.IsDefaultAudioDevice(_device, AudioDeviceRole.Multimedia))
            {
                state |= AudioDeviceDefaultState.Multimedia;
            }

            if (deviceManager.IsDefaultAudioDevice(_device, AudioDeviceRole.Communications))
            {
                state |= AudioDeviceDefaultState.Communications;
            }

            return state;
        }
    }
}
