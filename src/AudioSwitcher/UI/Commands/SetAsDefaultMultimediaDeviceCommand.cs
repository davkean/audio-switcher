// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System.ComponentModel.Composition;

using AudioSwitcher.Audio;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.UI.Commands
{
    [Command(CommandId.SetAsDefaultMultimediaDevice)]
    internal class SetAsDefaultMultimediaDeviceCommand : SetAsDefaultDeviceCommandBase
    {
        [ImportingConstructor]
        public SetAsDefaultMultimediaDeviceCommand(AudioDeviceManager manager)
            : base(manager, AudioDeviceRole.Multimedia)
        {
            Text = Resources.SetAsDefaultMultimediaDevice;
            Image = Resources.DefaultMultimediaDevice;
        }
    }
}
