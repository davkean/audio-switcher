// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
// This source file was altered for use in AudioSwitcher.
/*
  LICENSE
  -------
  Copyright (C) 2007 Ray Molenkamp

  This source code is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this source code or the software it produces.

  Permission is granted to anyone to use this source code for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this source code must not be misrepresented; you must not
     claim that you wrote the original source code.  If you use this source code
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original source code.
  3. This notice may not be removed or altered from any source distribution.
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using AudioSwitcher.Audio.Interop;

namespace AudioSwitcher.Audio
{
    /// <summary>
    /// Property Store class, only supports reading properties at the moment.
    /// </summary>
    internal class PropertyStore
    {
        private readonly IPropertyStore _underlyingStore;

        /// <summary>
        /// Creates a new property store
        /// </summary>
        /// <param name="store">IPropertyStore COM interface</param>
        internal PropertyStore(IPropertyStore store)
        {
           _underlyingStore = store;
        }

        /// <summary>
        /// Property Count
        /// </summary>
        public int Count
        {
            get
            {
                int result;
                Marshal.ThrowExceptionForHR(_underlyingStore.GetCount(out result));
                return result;
            }
        }

        /// <summary>
        /// Gets property by index
        /// </summary>
        /// <param name="index">Property index</param>
        /// <returns>The property</returns>
        public PropertyStoreProperty this[int index]
        {
            get
            {
                PropVariant result;
                PropertyKey key = Get(index);
                Marshal.ThrowExceptionForHR(_underlyingStore.GetValue(ref key, out result));
                return new PropertyStoreProperty(key, result);
            }
        }

        /// <summary>
        /// Contains property guid
        /// </summary>
        /// <param name="key">Looks for a specific key</param>
        /// <returns>True if found</returns>
        public bool Contains(PropertyKey key)
        {
            for (int i = 0; i < Count; i++)
            {
                PropertyKey other = Get(i);
                if ((other.formatId == key.formatId) && (other.propertyId == key.propertyId))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Indexer by guid
        /// </summary>
        /// <param name="key">Property Key</param>
        /// <returns>Property or null if not found</returns>
        public PropertyStoreProperty this[PropertyKey key]
        {
            get
            {
                PropVariant result;
                for (int i = 0; i < Count; i++)
                {
                    PropertyKey other = Get(i);
                    if ((other.formatId == key.formatId) && (other.propertyId == key.propertyId))
                    {
                        Marshal.ThrowExceptionForHR(_underlyingStore.GetValue(ref other, out result));
                        return new PropertyStoreProperty(other, result);
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Gets property key at sepecified index
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Property key</returns>
        public PropertyKey Get(int index)
        {
            PropertyKey key;
            Marshal.ThrowExceptionForHR(_underlyingStore.GetAt(index, out key));
            return key;
        }

        public bool TryGetValue(PropertyKey key, out object value)
        {
            value = null;

            try
            {
                var property = this[key];
                if (property == null)
                    return false;
                
                value = property.Value;
                return true;

            }
            catch (COMException ex)
            {
                const int NoSuchHDevinst = unchecked((int)0xE000020B);

                if (ex.HResult == NoSuchHDevinst)
                {
                    return false;
                }

                throw;
            }
        }

        /// <summary>
        /// Gets property value at specified index
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Property value</returns>
        public PropVariant GetValue(int index)
        {
            PropVariant result;
            PropertyKey key = Get(index);
            Marshal.ThrowExceptionForHR(_underlyingStore.GetValue(ref key, out result));
            return result;
        }
    }
}

