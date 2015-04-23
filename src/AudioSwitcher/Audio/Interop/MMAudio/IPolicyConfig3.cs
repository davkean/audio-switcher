// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System.Runtime.InteropServices;
using AudioSwitcher.Audio;

namespace AudioSwitcher.Audio.Interop
{
    [ComImport]
    [Guid("8F9FB2AA-1C0B-4D54-B6BB-B2F2A10CE03C")]  // Windows 10+
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPolicyConfig3
    {
        void Reserved1();

        void Reserved2();

        void Reserved3();

        void Reserved4();

        void Reserved5();

        void Reserved6();

        void Reserved7();

        void Reserved8();

        void Reserved9();

        void Reserved10();

        [PreserveSig]
        int SetDefaultEndpoint(string deviceId, AudioDeviceRole role);

        void Reserved11();
    }
}