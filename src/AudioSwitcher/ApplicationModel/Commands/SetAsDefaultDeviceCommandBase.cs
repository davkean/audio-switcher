// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.Linq;
using AudioSwitcher.Audio;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.ApplicationModel.Commands
{
    internal abstract class SetAsDefaultDeviceCommandBase : Command<AudioDevice>
    {
        private readonly AudioDeviceManager _manager;
        private readonly AudioDeviceRole _role;

        protected SetAsDefaultDeviceCommandBase(AudioDeviceManager manager, AudioDeviceRole role)
        {
            _manager = manager;
            _role = role;
        }

        public override void UpdateStatus(AudioDevice argument)
        {
            IsChecked = _manager.IsDefaultAudioDevice(argument, _role);
            IsEnabled = argument.IsActive && !IsChecked;
        }

        public override void Run(AudioDevice argument)
        {
            _manager.SetDefaultAudioDevice(argument, _role);
        }
    }
}
