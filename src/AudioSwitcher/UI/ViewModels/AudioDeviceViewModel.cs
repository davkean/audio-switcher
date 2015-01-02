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
        private readonly DeviceDefaultState _defaultState;

        public AudioDeviceViewModel(AudioDeviceManager deviceManager, AudioDevice device)
        {
            _device = device;
            _defaultState = CalculateDeviceDefaultState(deviceManager, device);
        }

        public bool IsEnabled
        {
            get { return _device.IsActive; }
        }

        public string Description
        {
            get { return _device.DeviceDescription; }
        }

        public string FriendlyName
        {
            get { return _device.DeviceFriendlyName; }
        }

        public string StateDisplayName
        {
            get 
            {
                // To mimic the Sound control panel, we display a device's 
                // default state first, and only then fall back to the actual
                // device's state if it's not a default device.

                if ((_defaultState & DeviceDefaultState.All) == DeviceDefaultState.All)
                {
                    return Resources.DeviceState_DefaultDevice;
                }

                if ((_defaultState & DeviceDefaultState.Multimedia) == DeviceDefaultState.Multimedia)
                {
                    return Resources.DeviceState_DefaultMultimediaDevice;
                }

                if ((_defaultState & DeviceDefaultState.Communications) == DeviceDefaultState.Communications)
                {
                    return Resources.DeviceState_DefaultCommunicationsDevice;
                }

                switch (_device.State)
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
        }
        public Image Image
        {
            get { return GetImage(); }
        }

        private Image GetImage()
        {
            if (String.IsNullOrEmpty(_device.DeviceClassIconPath))
                return null;

            Icon icon;
            if (!ShellIcon.TryExtractIconByIdOrIndex(_device.DeviceClassIconPath, new Size(48, 48), out icon))
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
            if ((_defaultState & DeviceDefaultState.Multimedia) == DeviceDefaultState.Multimedia)
            {   // Sound control panel shows the same icon between all and multimedia

                return Resources.DefaultMultimediaDevice;
            }
            
            if ((_defaultState & DeviceDefaultState.Communications) == DeviceDefaultState.Communications)
            {
                return Resources.DefaultCommunicationsDevice;
            }

            switch (_device.State)
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

        private static DeviceDefaultState CalculateDeviceDefaultState(AudioDeviceManager deviceManager, AudioDevice device)
        {
            DeviceDefaultState state = DeviceDefaultState.None;

            if (deviceManager.IsDefaultAudioDevice(device, AudioDeviceRole.Multimedia))
            {
                state |= DeviceDefaultState.Multimedia;
            }

            if (deviceManager.IsDefaultAudioDevice(device, AudioDeviceRole.Communications))
            {
                state |= DeviceDefaultState.Communications;
            }

            return state;
        }

        [Flags]
        private enum DeviceDefaultState
        {
            None = 0,
            Multimedia = 1,
            Communications = 2,
            All = Multimedia | Communications,
        }
    }
}
