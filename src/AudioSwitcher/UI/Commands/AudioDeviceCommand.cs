// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using System.Text;
using AudioSwitcher.Audio;
using AudioSwitcher.Presentation.CommandModel;
using AudioSwitcher.UI.ViewModels;

namespace AudioSwitcher.UI.Commands
{
    // Displays an audio device in the context menu, clicking the device causes it to be marked as "default"
    [Command(CommandId.SetAsDefaultDevice, IsReusable=false)]
    internal class AudioDeviceCommand : Command<AudioDeviceViewModel>
    {
        private readonly AudioDeviceManager _deviceManager;

        [ImportingConstructor]
        public AudioDeviceCommand(AudioDeviceManager deviceManager)
        {
            _deviceManager = deviceManager;
        }

        public override void Run(AudioDeviceViewModel argument)
        {
            if (argument.Device.IsActive)
                _deviceManager.SetDefaultAudioDevice(argument.Device);
        }

        public override void Refresh(AudioDeviceViewModel argument)
        {
            IsInvokable = argument.Device.IsActive;
            IsVisible = argument.IsVisible;
            if (IsVisible)
            {
                Text = GetDisplayText(argument);
                Image = argument.Image;
            }
            else
            {
                Text = null;
                Image = null;
            }
        }

        private string GetDisplayText(AudioDeviceViewModel viewModel)
        {
            StringBuilder text = new StringBuilder();
            text.AppendLine(viewModel.Description);         // Headphones (Black)
            text.AppendLine(viewModel.FriendlyName);        // High Definition Audio Device
            text.Append(viewModel.DeviceStateFriendlyName); // Ready

            return text.ToString();
        }
    }
}
