// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;

namespace AudioSwitcher.Presentation.CommandModel
{
    /// <summary>
    ///     Represents the export metadata of a <see cref="Command"/>.
    /// </summary>
    public interface ICommandMetadata
    {
        string Id
        {
            get;
        }

        bool IsDynamic
        {
            get;
        }
    }
}
