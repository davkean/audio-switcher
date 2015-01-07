// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using AudioSwitcher.ApplicationModel;
using AudioSwitcher.Audio;

namespace AudioSwitcher.Audio
{
    // Watches for device plug-in and automatically switches if the given setting is turned on
    [StartupService]
    internal class InputSwitcherService : IStartupService, IDisposable
    {
        private readonly AudioDeviceManager _manager;

        [ImportingConstructor]
        public InputSwitcherService(AudioDeviceManager manager)
        {
            _manager = manager;
        }

        public bool Startup()
        {
            _manager.DeviceStateChanged += OnDeviceStateChanged;
            return true;
        }

        private void OnDeviceStateChanged(object sender, AudioDeviceStateEventArgs e)
        {
            if (!Settings.Default.AutoSwitchToPluggedInDevice)
                return;

            // NOTE: The audio stack does a pretty good job of switching inputs when an audio device
            // is unplugged. For example, if you plug your headphones in and we then set that to the 
            // default device, then you unplug them, Windows will switch back to the last set default
            // audio device. This means that we don't need any smarts here to remember previous
            // devices.
            if (e.NewState == AudioDeviceState.Active)
            {
                _manager.SetDefaultAudioDevice(e.Device);
            }
        }

        public void Dispose()
        {
            _manager.DeviceStateChanged -= OnDeviceStateChanged;
        }
    }
}
