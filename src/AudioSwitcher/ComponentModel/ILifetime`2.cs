// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

namespace AudioSwitcher.ComponentModel
{
    internal interface ILifetime<out T, TMetadata> : ILifetime<T>
    {
        TMetadata Metadata
        {
            get;
        }
    }
}
