// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;

namespace AudioSwitcher.UI.ViewModels
{
    [Flags]
    internal enum AudioDeviceDefaultState
    {
        None = 0,
        Multimedia = 1,
        Communications = 2,
        All = Multimedia | Communications,
    }

    internal static class AudioDeviceDefaultStateExtensions
    {
        public static bool IsSet(this AudioDeviceDefaultState state, AudioDeviceDefaultState flag)
        {
            return (state & flag) == flag;
        }
    }
}
