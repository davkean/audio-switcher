// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using AudioSwitcher.Audio;
using AudioSwitcher.Presentation.CommandModel;
using AudioSwitcher.UI.ViewModels;

namespace AudioSwitcher.UI.Commands
{
    [Command(CommandId.NoPlaybackDevices)]
    internal class NoPlaybackDevicesCommand : NoDevicesCommandBase
    {
        [ImportingConstructor]
        public NoPlaybackDevicesCommand(AudioDeviceViewModelManager viewModelManager)
            : base(Resources.NoPlaybackDevices, viewModelManager, AudioDeviceKind.Playback)
        {
        }

        public override void Refresh()
        {
            if (Settings.Default.ShowPlaybackDevices)
            {
                base.Refresh();
            }
            else
            {
                IsVisible = false;
            }
        }
    }
}
