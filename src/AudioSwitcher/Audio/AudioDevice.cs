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

        public string DeviceDescription
        {
            get
            {
                object value;
                if (Properties.TryGetValue(PropertyKeys.PKEY_Device_DeviceDesc, out value))
                    return (string)value;

                return string.Empty;
            }
        }

        public string FriendlyName
        {
            get
            {
                object value;
                if (Properties.TryGetValue(PropertyKeys.PKEY_Device_FriendlyName, out value))
                    return (string)value;

                return string.Empty;
            }
        }

        public string DeviceFriendlyName
        {
            get
            {
                object value;
                if (Properties.TryGetValue(PropertyKeys.PKEY_DeviceInterface_FriendlyName, out value))
                    return (string)value;

                return string.Empty;
            }
        }

        public string DeviceClassIconPath
        {
            get
            {
                object value;
                if (Properties.TryGetValue(PropertyKeys.PKEY_DeviceClass_IconPath, out value))
                    return (string)value;
             
                return string.Empty;
            }
        }

        public string Id
        {
            get
            {
                string result;
                Marshal.ThrowExceptionForHR(_underlyingDevice.GetId(out result));
                return result;
            }
        }

        public AudioDeviceKind Kind
        {
            get
            {
                AudioDeviceKind result;
                var ep = (IMMEndpoint)_underlyingDevice;
                ep.GetDataFlow(out result);
                return result;
            }
        }

        public AudioDeviceState State
        {
            get
            {
                AudioDeviceState result;
                Marshal.ThrowExceptionForHR(_underlyingDevice.GetState(out result));
                return result;
            }
        }

        public override string ToString()
        {
            return FriendlyName;
        }

        private PropertyStore OpenPropertyStore()
        {
            IPropertyStore underlyingPropertyStore;
            Marshal.ThrowExceptionForHR(_underlyingDevice.OpenPropertyStore(StorageAccessMode.Read, out underlyingPropertyStore));
            return new PropertyStore(underlyingPropertyStore);
        }
    }
}
