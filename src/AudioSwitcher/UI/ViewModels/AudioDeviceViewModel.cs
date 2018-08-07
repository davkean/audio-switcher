// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.Drawing;
using System.Windows.Forms;
using AudioSwitcher.Audio;
using AudioSwitcher.Presentation.Drawing;
using AudioSwitcher.Presentation.UI;

namespace AudioSwitcher.UI.ViewModels
{
    internal class AudioDeviceViewModel
    {
        private static readonly Size s_iconSize = DpiServices.Scale(new Size(48, 48));

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

        public bool IsVisible
        {
            get;
            private set;
        }

        public void UpdateStatus(AudioDeviceManager deviceManager)
        {
            DefaultState = CalculateDeviceDefaultState(deviceManager);
            Kind = _device.Kind;
            State = _device.State;
            IsVisible = CalculateIsVisible();

            // Only do work such as get text, icons, etc if we're visible
            if (IsVisible)
            {   
                Description = TryGetOrDefault(_device.TryGetDeviceDescription, Description);
                FriendlyName = TryGetOrDefault(_device.TryDeviceFriendlyName, FriendlyName);
                DeviceStateFriendlyName = GetDeviceStateFriendlyName();

                if (_device.TryGetDeviceClassIconPath(out string iconPath))
                {
                    Image = GetImage(iconPath);
                }
            }
        }

        private bool CalculateIsVisible()
        {
            bool isVisible = true;

            switch (Kind)
            {
                case AudioDeviceKind.Playback:
                    isVisible &= Settings.Default.ShowPlaybackDevices;
                    break;

                case AudioDeviceKind.Recording:
                    isVisible &= Settings.Default.ShowRecordingDevices;
                    break;
            }

            switch (State)
            {
                case AudioDeviceState.Active:
                    break;

                case AudioDeviceState.Disabled:
                    isVisible &= Settings.Default.ShowDisabledDevices;
                    break;

                case AudioDeviceState.Unplugged:
                    isVisible &= Settings.Default.ShowUnpluggedDevices;
                    break;

                default:
                case AudioDeviceState.NotPresent:
                    isVisible &= Settings.Default.ShowNotPresentDevices;
                    break;
            }

            return isVisible;
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

            return string.Empty;
        }

        private Image GetImage(string iconPath)
        {
            Image deviceImage = GetDeviceImage(iconPath);
            if (deviceImage == null)
                return null;

            Image overlayImage = GetOverlayImage();
            if (overlayImage == null)
                return deviceImage;

            using (deviceImage)
            using (overlayImage)
            {
                // Makes a copy
                return DrawingServices.CreateOverlayedImage(deviceImage, overlayImage, deviceImage.Size);
            }
        }

        private Image GetDeviceImage(string iconPath)
        {
            using (Icon icon = GetIconFromDeviceIconPath(iconPath))
            {
                if (icon == null)
                    return null;

                Image image = icon.ToBitmap();
                if (State == AudioDeviceState.Active)
                    return image;

                using (image)
                {
                    return ToolStripRenderer.CreateDisabledImage(image);
                }
            }
        }

        private Icon GetIconFromDeviceIconPath(string iconPath)
        {
            if (string.IsNullOrEmpty(iconPath))
                return null;

            if (string.IsNullOrEmpty(iconPath) || !ShellIcon.TryExtractIconByIdOrIndex(iconPath, s_iconSize, out Icon icon))
                return new Icon(Resources.FallbackDevice, s_iconSize);

            return icon;
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

        private static string TryGetOrDefault(TryDelegate getter, string defaultValue)
        {
            if (getter(out string result))
            {
                return result;
            }

            return defaultValue;
        }

        private delegate bool TryDelegate(out string result);
    }
}
