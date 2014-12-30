// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.Drawing;
using System.Text;
using AudioSwitcher.Audio;
using AudioSwitcher.Presentation.Drawing;

namespace AudioSwitcher.Presentation.CommandModel.Commands
{
    // Displays an audio device in the context menu, clicking the device causes it to be marked as "default"
    internal class AudioDeviceCommand : Command
    {
        private readonly AudioDeviceManager _deviceManager;
        private readonly AudioDevice _device;

        public AudioDeviceCommand(AudioDeviceManager deviceManager, AudioDevice device)
        {
            _deviceManager = deviceManager;
            _device = device;
        }

        public override void Run()
        {
            _deviceManager.SetDefaultAudioDevice(_device);
        }

        public override void UpdateStatus()
        {
            Text = GetDisplayText();
            Image = GetImage();
            IsEnabled = _device.IsActive;

            UpdateCheckedStatus();
        }

        private void UpdateCheckedStatus()
        {
            if (_deviceManager.IsDefaultAudioDevice(_device, AudioDeviceRole.Multimedia))
            {
                IsChecked = true;
                CheckedImage = Resources.DefaultMultimediaDevice.ToBitmap();
                
            }
            else if (_deviceManager.IsDefaultAudioDevice(_device, AudioDeviceRole.Communications))
            {
                IsChecked = true;
                CheckedImage = Resources.DefaultCommunicationsDevice.ToBitmap();
            }
            else
            {
                IsChecked = false;
                CheckedImage = null;
            }
        }

        private string GetDisplayText()
        {
            StringBuilder text = new StringBuilder();
            text.AppendLine(_device.DeviceDescription);         // Headphones (Black)
            text.AppendLine(_device.DeviceFriendlyName);        // High Definition Audio Device
            text.Append(GetDisplayState());                     // Ready

            return text.ToString();
        }

        private string GetDisplayState()
        {
            switch (_device.State)
            {
                case AudioDeviceState.Active:
                    return Resources.DisplayName_Active;

                case AudioDeviceState.Disabled:
                    return Resources.DisplayName_Disabled;

                case AudioDeviceState.NotPresent:
                    return Resources.DisplayName_NotPresent;

                default:
                case AudioDeviceState.Unplugged:
                    return Resources.DisplayName_Unplugged;
            }
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
                return icon.ToBitmap();
            }
        }
    }
}
