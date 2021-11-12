// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;

using AudioSwitcher.ComponentModel;

namespace AudioSwitcher.Presentation
{
    internal abstract class Presenter : ObservableObject, IPresenter
    {
        protected Presenter()
        {
        }

		public abstract void Bind();
		
        public event EventHandler Closed;

        protected virtual void OnClosed(EventArgs e)
        {
            Closed?.Invoke(this, e);
        }
    }
}
