using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using AudioSwitcher.Audio;

namespace AudioSwitcher.Audio.Interop
{
    /// <summary>
    /// defined in MMDeviceAPI.h
    /// </summary>
    [Guid("1BE09788-6894-4089-8586-9A2A6C265AC5")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMEndpoint
    {
        [PreserveSig]
        int GetDataFlow(out AudioDeviceKind dataFlow);
    }
}
