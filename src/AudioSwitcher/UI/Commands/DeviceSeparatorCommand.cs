// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.UI.Commands
{
    [Command(CommandId.DeviceSeparator)]
    internal class DeviceSeparatorCommand : Command
    {
        public DeviceSeparatorCommand()
        {
            Text = "-";
        }

        public override void Run()
        {   
        }

        public override void Refresh()
        {
            IsVisible = Settings.Default.ShowRecordingDevices && Settings.Default.ShowPlaybackDevices;
        }
    }
}
