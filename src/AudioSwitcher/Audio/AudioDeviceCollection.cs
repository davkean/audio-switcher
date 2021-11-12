using System.Collections;
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
// updated for AudioSwitcher
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AudioSwitcher.Audio.Interop;

namespace AudioSwitcher.Audio
{
    internal class AudioDeviceCollection : IEnumerable<AudioDevice>
    {
        private readonly IMMDeviceCollection _underlyingCollection;

        internal AudioDeviceCollection(IMMDeviceCollection parent)
        {
            _underlyingCollection = parent;
        }

        public int Count
        {
            get
            {
                Marshal.ThrowExceptionForHR(_underlyingCollection.GetCount(out int result));
                return result;
            }
        }

        public AudioDevice this[int index]
        {
            get
            {
                Marshal.ThrowExceptionForHR(_underlyingCollection.Item(index, out IMMDevice underlyingDevice));

                return new AudioDevice(underlyingDevice);
            }
        }

        public IEnumerator<AudioDevice> GetEnumerator()
        {
            int count = Count;
            for (int index = 0; index < count; index++)
            {
                yield return this[index];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
