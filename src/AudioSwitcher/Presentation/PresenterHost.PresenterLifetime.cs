// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System.ComponentModel.Composition;
using AudioSwitcher.ComponentModel;

namespace AudioSwitcher.Presentation
{
    partial class PresenterHost
    {
        private class PresenterLifetime<T> : Lifetime<T>, ILifetime<T, IPresenterMetadata>
            where T : IPresenter
        {
            private readonly IPresenterMetadata _metadata;

            public PresenterLifetime(ExportLifetimeContext<IPresenter> presenter, IPresenterMetadata metadata)
                : base(() => (T)presenter.Value, () => presenter.Dispose())
            {
                _metadata = metadata;
            }

            public IPresenterMetadata Metadata
            {
                get { return _metadata; }
            }
        }
    }
}
