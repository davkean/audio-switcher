// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System.ComponentModel.Composition;

using AudioSwitcher.Audio;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.UI.Commands
{
    [Command(CommandId.SetAsDefaultCommunicationDevice)]
    internal class SetAsDefaultCommunicationDeviceCommand : SetAsDefaultDeviceCommandBase
    {
        [ImportingConstructor]
        public SetAsDefaultCommunicationDeviceCommand(AudioDeviceManager manager)
            : base(manager, AudioDeviceRole.Communications)
        {
            Text = Resources.SetAsDefaultComunicationDevice;
            Image = Resources.DefaultCommunicationsDevice;
        }
    }
}
