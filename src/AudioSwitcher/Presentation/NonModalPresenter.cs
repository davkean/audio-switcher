// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel;
using AudioSwitcher.ComponentModel;

namespace AudioSwitcher.Presentation
{
    internal abstract class NonModalPresenter : Presenter
    {
        protected NonModalPresenter()
        {
        }

        public abstract void ShowNonModal();
    }
}
