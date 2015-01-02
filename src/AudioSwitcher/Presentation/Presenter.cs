// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel;
using AudioSwitcher.ComponentModel;

namespace AudioSwitcher.Presentation
{
    internal abstract class Presenter : ObservableObject, IPresenter
    {
        protected Presenter()
        {
        }

        public event EventHandler Closed;

        protected virtual void OnClosed(EventArgs e)
        {
            var handler = Closed;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
