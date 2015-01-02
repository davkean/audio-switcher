// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using AudioSwitcher.Audio;

namespace AudioSwitcher.Audio
{
    internal class AudioDeviceRemovedEventArgs : EventArgs
    {
        private readonly string _deviceId;

        public AudioDeviceRemovedEventArgs(string deviceId)
        {
            _deviceId = deviceId;
        }
        
        public string DeviceId
        {
            get { return _deviceId; }
        }
    }
}
