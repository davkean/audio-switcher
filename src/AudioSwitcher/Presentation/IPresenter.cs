// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel;

namespace AudioSwitcher.Presentation
{
    internal interface IPresenter : INotifyPropertyChanged
    {
        event EventHandler Closed;
    }
}
