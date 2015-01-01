// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.ApplicationModel.Commands
{
    [Command(CommandId.ToggleShowNotPresentDevices)]
    internal class ToggleShowNotPresentDevicesCommand : Command
    {
        [ImportingConstructor]
        public ToggleShowNotPresentDevicesCommand()
        {
            Text = Resources.ShowNotPresentDevices;
            Image = Resources.NotPresent;
        }

        public override void UpdateStatus()
        {
            IsChecked = Settings.Default.ShowNotPresentDevices;
        }

        public override void Run()
        {
            Settings.Default.ShowNotPresentDevices = !Settings.Default.ShowNotPresentDevices;
        }
    }
}
