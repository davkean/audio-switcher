// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using AudioSwitcher.ApplicationModel;
using AudioSwitcher.ComponentModel;

namespace AudioSwitcher.Presentation
{
    [Export(typeof(PresenterManager))]
    internal class PresenterManager
    {
        private readonly ExportFactory<IPresenter, IPresenterMetadata>[] _presenters;
        private ILifetime<ContextMenuPresenter, IPresenterMetadata> _current;

        [ImportingConstructor]
        public PresenterManager([ImportMany]ExportFactory<IPresenter, IPresenterMetadata>[] presenters)
        {
            _presenters = presenters;
        }

        public void ShowNonModal(string id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            ILifetime<NonModalPresenter> presenter = CreatePresenter<NonModalPresenter>(id);
            presenter.Instance.Closed += (sender, e) => presenter.Dispose();
            presenter.Instance.ShowNonModal();
        }

        public void ShowContextMenu(string id, Point screenLocation)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            if (CloseOpenContextMenu(id))
                return;

            ILifetime<ContextMenuPresenter, IPresenterMetadata> presenter = CreatePresenter<ContextMenuPresenter>(id);
            _current = presenter;
            presenter.Instance.Closed += (sender, e) =>
            {
                _current.Dispose();
                _current = null;
            };

            presenter.Instance.Show(screenLocation);
        }

        private bool CloseOpenContextMenu(string id)
        {
            ILifetime<ContextMenuPresenter, IPresenterMetadata> current = _current;
            if (current != null)
            {
                current.Instance.Close();

                // Current context menu is a toggle and is already
                // open, let's close it instead of opening another
                if (current.Metadata.Id == id && current.Metadata.IsToggle)
                    return true;
            }

            return false;
        }

        private ILifetime<T, IPresenterMetadata> CreatePresenter<T>(string id) where T : IPresenter
        {
            ExportFactory<IPresenter, IPresenterMetadata> factory = _presenters.Where(c => c.Metadata.Id == id)
                                                                               .SingleOrDefault();
            if (factory == null)
                throw new InvalidOperationException();

            ExportLifetimeContext<IPresenter> context = factory.CreateExport();

            return new PresenterLifetime<T>(context, factory.Metadata);
        }

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
