// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.UI.Commands
{
    [Command(CommandId.ToggleShowPlaybackDevices)]
    internal class ToggleShowPlaybackDevicesCommand : Command
    {
        [ImportingConstructor]
        public ToggleShowPlaybackDevicesCommand()
        {
            Text = Resources.ShowPlaybackDevices;
            Image = Resources.PlaybackDevice;
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
