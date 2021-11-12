// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

namespace AudioSwitcher.Presentation.CommandModel
{
    /// <summary>
    ///     Represents the export metadata of a <see cref="ICommand"/>.
    /// </summary>
    public interface ICommandMetadata
    {
        string Id
        {
            get;
        }

        bool IsReusable
        {
            get;
        }
    }
}
