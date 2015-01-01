// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using System.Drawing;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.ApplicationModel.Commands
{
    [Command(CommandId.ToggleShowDisabledDevices)]
    internal class ToggleShowDisabledDevicesCommand : Command
    {
        [ImportingConstructor]
        public ToggleShowDisabledDevicesCommand()
        {
            Text = Resources.ShowDisabledDevices;
            Image = Resources.Disabled;
        }

        public override void UpdateStatus()
        {
            IsChecked = Settings.Default.ShowDisabledDevices;
        }

        public override void Run()
        {
            Settings.Default.ShowDisabledDevices = !Settings.Default.ShowDisabledDevices;
        }
    }
}
