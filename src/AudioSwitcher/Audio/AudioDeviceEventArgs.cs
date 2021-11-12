// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

using System;

namespace AudioSwitcher.Audio
{
    internal class AudioDeviceEventArgs : EventArgs
    {
        private readonly AudioDevice _device;

        public AudioDeviceEventArgs(AudioDevice device)
        {
            _device = device;
        }

        public AudioDevice Device
        {
            get { return _device; }
        }
    }
}
