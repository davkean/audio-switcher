// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AudioSwitcher.ComponentModel;

namespace AudioSwitcher.Presentation
{
    [Export(typeof(PresenterHost))]
    internal partial class PresenterHost
    {
        private readonly ExportFactory<IPresenter, IPresenterMetadata>[] _presenters;
        private readonly List<ILifetime<ContextMenuPresenter, IPresenterMetadata>> _openContextMenus = new List<ILifetime<ContextMenuPresenter, IPresenterMetadata>>();

        [ImportingConstructor]
        public PresenterHost([ImportMany]ExportFactory<IPresenter, IPresenterMetadata>[] presenters)
        {
            _presenters = presenters;
        }

        public void Show(string id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            ILifetime<NonModalPresenter> presenter = CreatePresenter<NonModalPresenter>(id);
            presenter.Instance.Closed += (sender, e) =>
            {
                presenter.Dispose();
            };

            presenter.Instance.Bind();
            presenter.Instance.Show();
        }

        public void ShowContextMenu(string id, Point screenLocation)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            if (CloseOpenContextMenu(id))
                return;

            ILifetime<ContextMenuPresenter, IPresenterMetadata> presenter = CreatePresenter<ContextMenuPresenter>(id);
            presenter.Instance.Closed += (sender, e) =>
            {
                _openContextMenus.Remove(presenter);
                presenter.Dispose();
            };

            _openContextMenus.Add(presenter);
            presenter.Instance.Bind();
            presenter.Instance.Show(screenLocation);
        }

        private bool CloseOpenContextMenu(string id)
        {
            foreach (var contextMenu in _openContextMenus.ToArray())
            {
                // Current context menu is a toggle and is already
                // open, let's close it instead of opening another
                if (contextMenu.Metadata.Id == id && contextMenu.Metadata.IsToggle)
                {
                    contextMenu.Instance.Close();
                    return true;
                }
            }

            return false;
        }

        private ILifetime<TPresenter, IPresenterMetadata> CreatePresenter<TPresenter>(string id) where TPresenter : IPresenter
        {
            ExportFactory<IPresenter, IPresenterMetadata> factory = FindPresenterFactory(id);

            var presenter = factory.CreateExport();

            return new PresenterLifetime<TPresenter>(presenter, factory.Metadata);
        }

        private ExportFactory<IPresenter, IPresenterMetadata> FindPresenterFactory(string id)
        {
            ExportFactory<IPresenter, IPresenterMetadata> factory = _presenters.Where(c => c.Metadata.Id == id)
                                                                               .SingleOrDefault();
            if (factory == null)
                throw new InvalidOperationException();

            return factory;
        }
    }
}
