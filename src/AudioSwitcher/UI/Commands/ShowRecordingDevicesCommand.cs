// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

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
