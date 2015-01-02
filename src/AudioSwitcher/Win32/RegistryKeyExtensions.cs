// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.IO;
using System.Security;
using Microsoft.Win32;

namespace AudioSwitcher.Win32
{
    // Provides extension methods for registry keys
    internal static class RegistryKeyExtensions
    {
        public static bool TryOpenSubkey(this RegistryKey key, string name, bool writable, out RegistryKey result)
        {
            try
            {
                result = key.OpenSubKey(name, writable);
                return true;
            }
            catch (IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (SecurityException)
            {
            }

            result = null;
            return false;
        }

        public static bool TryGetValue<T>(this RegistryKey key, string name, out T value) where T : class
        {
            try
            {
                value = key.GetValue(name) as T;

                return value != null;
            }
            catch (IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (SecurityException)
            {
            }

            value = null;
            return false;
        }

        public static bool TryDeleteValue(this RegistryKey key, string name)
        {
            try
            {
                key.DeleteValue(name);
                return true;
            }
            catch (IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (SecurityException)
            {
            }

            return false;

        }

        public static bool TrySetValue(this RegistryKey key, string name, object value)
        {
            try
            {
                key.SetValue(name, value);
                return true;
            }
            catch (IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (SecurityException)
            {
            }

            return false;
        }
    }
}
