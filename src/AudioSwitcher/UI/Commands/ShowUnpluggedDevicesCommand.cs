﻿// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

using System.ComponentModel.Composition;

using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.UI.Commands
{
    [Command(CommandId.ShowUnpluggedDevices)]
    internal class ShowUnpluggedDevicesCommand : Command
    {
        [ImportingConstructor]
        public ShowUnpluggedDevicesCommand()
        {
            Text = Resources.ShowUnpluggedDevices;
        }

        public override void Refresh()
        {
            IsChecked = Settings.Default.ShowUnpluggedDevices;
        }

        public override void Run()
        {
            Settings.Default.ShowUnpluggedDevices = !Settings.Default.ShowUnpluggedDevices;
        }
    }
}
