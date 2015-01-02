// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.UI.Commands
{
    [Command(CommandId.ToggleShowRecordingDevices)]
    internal class ToggleShowRecordingDevicesCommand : Command
    {
        [ImportingConstructor]
        public ToggleShowRecordingDevicesCommand()
        {
            Text = Resources.ShowRecordingDevices;
            Image = Resources.RecordingDevice;
        }

        public override void UpdateStatus()
        {
            IsChecked = Settings.Default.ShowRecordingDevices;
        }

        public override void Run()
        {
            Settings.Default.ShowRecordingDevices = !Settings.Default.ShowRecordingDevices;
        }
    }
}
