// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.UI.Commands
{
    [Command(CommandId.ToggleAutomaticallySwitchToPluggedInDevice)]
    internal class ToggleAutomaticallySwitchToPluggedInDeviceCommand : Command
    {
        public ToggleAutomaticallySwitchToPluggedInDeviceCommand()
        {
            Text = Resources.AutomaticallySwitchToPluggedInDevice;
        }

        public override void UpdateStatus()
        {
            IsChecked = Settings.Default.AutoSwitchToPluggedInDevice;
        }

        public override void Run()
        {
            Settings.Default.AutoSwitchToPluggedInDevice = !Settings.Default.AutoSwitchToPluggedInDevice;
        }
    }
}
