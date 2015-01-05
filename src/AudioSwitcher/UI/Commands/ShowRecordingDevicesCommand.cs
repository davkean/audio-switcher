// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.UI.Commands
{
    [Command(CommandId.ShowRecordingDevices)]
    internal class ShowRecordingDevicesCommand : Command
    {
        [ImportingConstructor]
        public ShowRecordingDevicesCommand()
        {
            Text = Resources.ShowRecordingDevices;
            Image = Resources.RecordingDevice;
        }

        public override void Refresh()
        {
            IsChecked = Settings.Default.ShowRecordingDevices;
        }

        public override void Run()
        {
            Settings.Default.ShowRecordingDevices = !Settings.Default.ShowRecordingDevices;
        }
    }
}
