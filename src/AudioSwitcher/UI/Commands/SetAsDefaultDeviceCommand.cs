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
    internal class SetAsDefaultDeviceCommand : Command<AudioDevice>
    {
        private readonly AudioDeviceManager _deviceManager;

        [ImportingConstructor]
        public SetAsDefaultDeviceCommand(AudioDeviceManager deviceManager)
        {
            _deviceManager = deviceManager;
        }

        public override void Run(AudioDevice argument)
        {
            _deviceManager.SetDefaultAudioDevice(argument);
        }

        public override void UpdateStatus(AudioDevice argument)
        {
            AudioDeviceViewModel viewModel = new AudioDeviceViewModel(_deviceManager, argument);

            Text = GetDisplayText(viewModel);
            Image = viewModel.Image;
            IsEnabled = viewModel.IsEnabled;
        }

        private string GetDisplayText(AudioDeviceViewModel viewModel)
        {
            StringBuilder text = new StringBuilder();
            text.AppendLine(viewModel.Description);         // Headphones (Black)
            text.AppendLine(viewModel.FriendlyName);        // High Definition Audio Device
            text.Append(viewModel.StateDisplayName);        // Ready

            return text.ToString();
        }
    }
}
