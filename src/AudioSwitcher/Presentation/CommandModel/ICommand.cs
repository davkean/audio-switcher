// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Drawing;

namespace AudioSwitcher.Presentation.CommandModel
{
    internal interface ICommand : INotifyPropertyChanged
    {
        Image Image { get; }
        bool IsVisible { get; }

        bool IsChecked { get; }

        bool IsEnabled { get; }

        string Text { get; }

        string TooltipText { get; }

        void Run(object argument);

        void Refresh(object argument);
    }
}
