﻿// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

using System.ComponentModel.Composition;

using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.UI.Commands
{
    [Command(CommandId.ShowPlaybackDevices)]
    internal class ShowPlaybackDevicesCommand : Command
    {
        [ImportingConstructor]
        public ShowPlaybackDevicesCommand()
        {
            Text = Resources.ShowPlaybackDevices;
        }

        public override void Refresh()
        {
            IsChecked = Settings.Default.ShowPlaybackDevices;
        }

        public override void Run()
        {
            Settings.Default.ShowPlaybackDevices = !Settings.Default.ShowPlaybackDevices;
        }
    }
}
