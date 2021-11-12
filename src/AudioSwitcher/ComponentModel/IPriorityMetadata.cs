// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System.ComponentModel;

namespace AudioSwitcher.ComponentModel
{
    public interface IPriorityMetadata
    {
        [DefaultValue(0)]
        int Priority
        {
            get;
        }
    }
}
