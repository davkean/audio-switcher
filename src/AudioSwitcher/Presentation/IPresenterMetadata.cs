// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

namespace AudioSwitcher.Presentation
{
    /// <summary>
    ///     Represents the export metadata of a <see cref="IPresenter"/>.
    /// </summary>
    public interface IPresenterMetadata
    {
        string Id
        {
            get;
        }

        bool IsToggle
        {
            get;
        }
    }
}
