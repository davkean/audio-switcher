// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.Drawing;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.ApplicationModel.Commands
{
    internal class ShowUnpluggedDevicesCommand : Command
    {
        public ShowUnpluggedDevicesCommand()
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
