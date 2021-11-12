// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.UI.Commands
{
    [Command(CommandId.NoDevices)]
    internal class NoDevicesCommand : DisabledCommand
    {
        public NoDevicesCommand()
            : base(Resources.NoDevices)
        {
        }

        public override void Refresh()
        {
            IsVisible = !Settings.Default.ShowRecordingDevices && !Settings.Default.ShowPlaybackDevices;
        }
    }
}
