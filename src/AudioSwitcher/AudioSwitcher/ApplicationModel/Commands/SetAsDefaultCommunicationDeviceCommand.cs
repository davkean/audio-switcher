// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using AudioSwitcher.Audio;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.ApplicationModel.Commands
{
    internal class SetAsDefaultCommunicationDeviceCommand : SetAsDefaultDeviceCommand
    {
        public SetAsDefaultCommunicationDeviceCommand(AudioDeviceManager manager, AudioDevice device)
            : base(manager, device, AudioDeviceRole.Communications)
        {
            Text = Resources.SetAsDefaultComunicationDevice;
            Image = Resources.DefaultCommunicationsDevice;
        }
    }
}
