// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

namespace AudioSwitcher.Presentation
{
    internal abstract class NonModalPresenter : Presenter
    {
        protected NonModalPresenter()
        {
        }

		public abstract void Show();
    }
}
