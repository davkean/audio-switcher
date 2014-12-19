// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.Linq;
using AudioSwitcher.Audio;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.Presentation.CommandModel.Commands
{
    internal abstract class SetAsDefaultDeviceCommand : Command
    {
        private readonly AudioDeviceManager _manager;
        private readonly AudioDevice _device;
        private readonly AudioDeviceRole _role;

        protected SetAsDefaultDeviceCommand(AudioDeviceManager manager, AudioDevice device, AudioDeviceRole role)
        {
            _manager = manager;
            _device = device;
            _role = role;
        }

        public AudioDevice Device
        {
            get { return _device; }
        }

        public override void UpdateStatus()
        {
            IsChecked = _manager.IsDefaultAudioDevice(_device, _role);
            IsEnabled = _device.IsActive && !IsChecked;
        }

        public override void Run()
        {
            _manager.SetDefaultAudioDevice(_device, _role);
        }
    }
}
