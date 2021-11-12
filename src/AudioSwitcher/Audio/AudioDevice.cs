// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

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
// modified for AudioSwitcher
using AudioSwitcher.Audio.Interop;
using System.Runtime.InteropServices;

namespace AudioSwitcher.Audio
{
    internal class AudioDevice
    {
        private readonly IMMDevice _underlyingDevice;
        private PropertyStore _propertyStore;

        internal AudioDevice(IMMDevice underlyingDevice)
        {
            _underlyingDevice = underlyingDevice;
        }

        public bool IsActive
        {
            get { return State == AudioDeviceState.Active; }
        }

        public PropertyStore Properties
        {
            get
            {
                if (_propertyStore == null)
                    _propertyStore = OpenPropertyStore();

                return _propertyStore;
            }
        }

        public bool TryGetDeviceDescription(out string result)
        {
            if (Properties.TryGetValue(PropertyKeys.PKEY_Device_DeviceDesc, out object value))
            {
                result = (string)value;
                return true;
            }

            result = string.Empty;
            return false;
        }

        public bool TryGetFriendlyName(out string result)
        {
            if (Properties.TryGetValue(PropertyKeys.PKEY_Device_FriendlyName, out object value))
            {
                result = (string)value;
                return true;
            }

            result = string.Empty;
            return false;
        }

        public bool TryDeviceFriendlyName(out string result)
        {
            if (Properties.TryGetValue(PropertyKeys.PKEY_DeviceInterface_FriendlyName, out object value))
            {
                result = (string)value;
                return true;
            }

            result = string.Empty;
            return false;
        }

        public bool TryGetDeviceClassIconPath(out string result)
        {
            if (Properties.TryGetValue(PropertyKeys.PKEY_DeviceClass_IconPath, out object value))
            {
                result = (string)value;
                return true;
            }

            result = string.Empty;
            return false;
        }

        public string Id
        {
            get
            {
                Marshal.ThrowExceptionForHR(_underlyingDevice.GetId(out string result));
                return result;
            }
        }

        public AudioDeviceKind Kind
        {
            get
            {
                var ep = (IMMEndpoint)_underlyingDevice;
                ep.GetDataFlow(out AudioDeviceKind result);
                return result;
            }
        }

        public AudioDeviceState State
        {
            get
            {
                Marshal.ThrowExceptionForHR(_underlyingDevice.GetState(out AudioDeviceState result));
                return result;
            }
        }

        public override string ToString()
        {
            TryGetFriendlyName(out string result);

            return result;
        }

        private PropertyStore OpenPropertyStore()
        {
            Marshal.ThrowExceptionForHR(_underlyingDevice.OpenPropertyStore(StorageAccessMode.Read, out IPropertyStore underlyingPropertyStore));
            return new PropertyStore(underlyingPropertyStore);
        }
    }
}
