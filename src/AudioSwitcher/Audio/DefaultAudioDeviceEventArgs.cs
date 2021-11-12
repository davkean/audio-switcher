// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------

namespace AudioSwitcher.Audio
{
    internal class DefaultAudioDeviceEventArgs : AudioDeviceEventArgs
    {
        private readonly AudioDeviceKind _kind;
        private readonly AudioDeviceRole _role;

        public DefaultAudioDeviceEventArgs(AudioDevice device, AudioDeviceKind kind, AudioDeviceRole role)
            : base(device)
        {
            _kind = kind;
            _role = role;
        }
        
        public AudioDeviceKind Kind
        {
            get { return _kind; }
        }

        public AudioDeviceRole Role
        {
            get { return _role; }
        }
    }
}
