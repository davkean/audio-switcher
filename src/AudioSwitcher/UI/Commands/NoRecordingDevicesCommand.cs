// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.UI.Commands
{
    [Command(CommandId.NoRecordingDevices)]
    internal class NoRecordingDevicesCommand : DisabledCommand
    {
        public NoRecordingDevicesCommand()
            : base(Resources.NoRecordingDevices)
        {
        }
    }

    [Command(CommandId.NoPlaybackDevices)]
    internal class NoPlaybackDevicesCommand : DisabledCommand
    {
        public NoPlaybackDevicesCommand()
            : base(Resources.NoPlaybackDevices)
        {
        }
    }

    [Command(CommandId.NoDevices)]
    internal class NoDevicesCommand : DisabledCommand
    {
        public NoDevicesCommand()
            : base(Resources.NoDevices)
        {
        }
    }
}
