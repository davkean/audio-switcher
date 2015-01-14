// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;

namespace AudioSwitcher.ComponentModel
{
    internal interface ILifetime<out T> : IDisposable
    {
        T Instance
        {
            get;
        }
    }
}
