// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System.ComponentModel.Composition;

using AudioSwitcher.Audio;
using AudioSwitcher.Presentation.CommandModel;
using AudioSwitcher.UI.ViewModels;

namespace AudioSwitcher.UI.Commands
{
    [Command(CommandId.NoRecordingDevices)]
    internal class NoRecordingDevicesCommand : NoDevicesCommandBase
    {
        [ImportingConstructor]
        public NoRecordingDevicesCommand(AudioDeviceViewModelManager viewModelManager)
            : base(Resources.NoRecordingDevices, viewModelManager, AudioDeviceKind.Recording)
        {
        }

        public override void Refresh()
        {
            if (Settings.Default.ShowRecordingDevices)
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
