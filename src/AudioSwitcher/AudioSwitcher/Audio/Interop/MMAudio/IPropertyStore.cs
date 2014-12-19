using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using AudioSwitcher.Audio;

namespace AudioSwitcher.Interop
{
    /// <summary>
    /// is defined in propsys.h
    /// </summary>
    [Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IPropertyStore
    {
        [PreserveSig]
        int GetCount(out int propCount);

        [PreserveSig]
        int GetAt(int property, out PropertyKey key);

        [PreserveSig]
        int GetValue(ref PropertyKey key, out PropVariant value);

        [PreserveSig]
        int SetValue(ref PropertyKey key, ref PropVariant value);

        [PreserveSig]
        int Commit();
    }
}
