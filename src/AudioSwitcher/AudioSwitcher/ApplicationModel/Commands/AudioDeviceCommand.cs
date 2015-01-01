// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.Text;
using AudioSwitcher.ApplicationModel.ViewModels;
using AudioSwitcher.Audio;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.ApplicationModel.Commands
{
    // Displays an audio device in the context menu, clicking the device causes it to be marked as "default"
    internal class AudioDeviceCommand : Command
    {
        private readonly AudioDeviceManager _deviceManager;
        private readonly AudioDevice _device;
        private readonly AudioDeviceViewModel _viewModel;

        public AudioDeviceCommand(AudioDeviceManager deviceManager, AudioDevice device)
        {
            _deviceManager = deviceManager;
            _device = device;
            _viewModel = new AudioDeviceViewModel(deviceManager, device);
        }

        public override void Run()
        {
            _deviceManager.SetDefaultAudioDevice(_device);
        }

        public override void UpdateStatus()
        {
            Text = GetDisplayText();
            Image = _viewModel.Image;
            IsEnabled = _device.IsActive;
        }

        private string GetDisplayText()
        {
            StringBuilder text = new StringBuilder();
            text.AppendLine(_viewModel.Description);         // Headphones (Black)
            text.AppendLine(_viewModel.FriendlyName);        // High Definition Audio Device
            text.Append(_viewModel.StateDisplayName);        // Ready

            return text.ToString();
        }
    }
}
