﻿// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

using System.ComponentModel.Composition;

using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.UI.Commands
{
    [Command(CommandId.ShowDisabledDevices)]
    internal class ShowDisabledDevicesCommand : Command
    {
        [ImportingConstructor]
        public ShowDisabledDevicesCommand()
        {
            Text = Resources.ShowDisabledDevices;
        }

        public override void Refresh()
        {
            IsChecked = Settings.Default.ShowDisabledDevices;
        }

        public override void Run()
        {
            Settings.Default.ShowDisabledDevices = !Settings.Default.ShowDisabledDevices;
        }
    }
}
