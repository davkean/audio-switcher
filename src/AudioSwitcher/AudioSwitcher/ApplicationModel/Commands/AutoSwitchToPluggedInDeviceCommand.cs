// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.ApplicationModel.Commands
{
    internal class AutoSwitchToPluggedInDeviceCommand : Command
    {
        public AutoSwitchToPluggedInDeviceCommand()
        {
            Text = Resources.AutoSwitchToPluggedInDevice;
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
