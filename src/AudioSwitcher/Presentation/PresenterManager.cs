// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AudioSwitcher.ApplicationModel;
using AudioSwitcher.ComponentModel;

namespace AudioSwitcher.Presentation
{
    [Export(typeof(PresenterManager))]
    internal class PresenterManager : IDisposable
    {
        private readonly IApplication _application;
        private readonly ExportFactory<IPresenter, IPresenterMetadata>[] _presenters;
        private readonly List<IDisposable> _lifetimesToDispose = new List<IDisposable>();
        private PresenterLifetime<ContextMenuPresenter> _current;

        [ImportingConstructor]
        public PresenterManager(IApplication application, [ImportMany]ExportFactory<IPresenter, IPresenterMetadata>[] presenters)
        {
            _application = application;
            _application.Idle += OnApplicationIdle;
            _presenters = presenters;
        }

        public void ShowNonModal(string id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            PresenterLifetime<NonModalPresenter> presenter = CreatePresenter<NonModalPresenter>(id);
            presenter.Instance.Closed += (sender, e) => presenter.Dispose();
            presenter.Instance.ShowNonModal();
        }

        public void ShowContextMenu(string id, Point screenLocation)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            if (CloseOpenContextMenu(id))
                return;

            PresenterLifetime<ContextMenuPresenter> presenter = _current = CreatePresenter<ContextMenuPresenter>(id);
            presenter.Instance.Closed += (sender, e) =>
            {
                Debug.Assert(_current == presenter);
                _current = null;
                _lifetimesToDispose.Add(presenter);
            };

            presenter.Instance.Show(screenLocation);
        }

        private bool CloseOpenContextMenu(string id)
        {
            PresenterLifetime<ContextMenuPresenter> current = _current;
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

        private PresenterLifetime<T> CreatePresenter<T>(string id) where T : IPresenter
        {
            ExportFactory<IPresenter, IPresenterMetadata> factory = _presenters.Where(c => c.Metadata.Id == id)
                                                                               .SingleOrDefault();
            if (factory == null)
                throw new InvalidOperationException();

            ExportLifetimeContext<IPresenter> context = factory.CreateExport();

            return new PresenterLifetime<T>(context, factory.Metadata);
        }

        public void Dispose()
        {
            _application.Idle -= OnApplicationIdle;
        }

        private void OnApplicationIdle(object sender, EventArgs e)
        {
            // WORKAROUND: It's not possible to dispose of a context menu strip
            // in its Closed event because it tries to do some work after that
            // and throws ObjectDisposedException. To workaround this, we hold
            // off disposing them until the next idle event.
            foreach (IDisposable disposable in _lifetimesToDispose)
            {
                disposable.Dispose();
            }

            _lifetimesToDispose.Clear();
        }

        private class PresenterLifetime<T> : Lifetime<T>
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
