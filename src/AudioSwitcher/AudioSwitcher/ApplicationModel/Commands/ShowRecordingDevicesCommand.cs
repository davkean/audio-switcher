// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.ApplicationModel.Commands
{
    internal class ShowRecordingDevicesCommand : Command
    {
        public ShowRecordingDevicesCommand()
        {
            Text = Resources.ShowRecordingDevices;
            Image = Resources.Microphone.ToBitmap();
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
