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
        Image CheckedImage { get; }

        Image Image { get; }

        bool IsBulleted { get; }

        bool IsChecked { get; }

        bool IsEnabled { get; }

        string Text { get; }

        string TooltipText { get; }

        void Run(object argument);

        void UpdateStatus(object argument);
    }
}
