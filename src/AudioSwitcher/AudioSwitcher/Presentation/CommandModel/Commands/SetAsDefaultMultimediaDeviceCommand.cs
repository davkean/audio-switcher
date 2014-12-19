// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using AudioSwitcher.Audio;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.Presentation.CommandModel.Commands
{
    internal class SetAsDefaultMultimediaDeviceCommand : SetAsDefaultDeviceCommand
    {
        public SetAsDefaultMultimediaDeviceCommand(AudioDeviceManager manager, AudioDevice device)
            : base(manager, device, AudioDeviceRole.Console)
        {
            Text = Resources.SetAsDefaultMultimediaDevice;
            Image = Resources.DefaultMultimediaDevice.ToBitmap();
        }
    }
}
