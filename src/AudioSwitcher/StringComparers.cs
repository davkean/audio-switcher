// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace AudioSwitcher
{
    internal class StringComparers
    {
        public static IEqualityComparer<string> DeviceIds
        {
            get { return StringComparer.OrdinalIgnoreCase; }
        }
    }
}
