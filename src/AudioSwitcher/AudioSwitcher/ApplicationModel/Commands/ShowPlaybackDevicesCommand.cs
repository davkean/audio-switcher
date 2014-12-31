// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.ApplicationModel.Commands
{
    internal class ShowPlaybackDevicesCommand : Command
    {
        public ShowPlaybackDevicesCommand()
        {
            Text = Resources.ShowPlaybackDevices;
            Image = Resources.Headphones.ToBitmap();
        }

        public override void UpdateStatus()
        {
            IsChecked = Settings.Default.ShowPlaybackDevices;
        }

        public override void Run()
        {
            Settings.Default.ShowPlaybackDevices = !Settings.Default.ShowPlaybackDevices;
        }
    }
}
