// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.UI.Commands
{
    [Command(CommandId.ToggleShowUnpluggedDevices)]
    internal class ToggleShowUnpluggedDevicesCommand : Command
    {
        [ImportingConstructor]
        public ToggleShowUnpluggedDevicesCommand()
        {
            Text = Resources.ShowUnpluggedDevices;
            Image = Resources.Unplugged;
        }

        public override void UpdateStatus()
        {
            IsChecked = Settings.Default.ShowUnpluggedDevices;
        }

        public override void Run()
        {
            Settings.Default.ShowUnpluggedDevices = !Settings.Default.ShowUnpluggedDevices;
        }
    }
}
