// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using AudioSwitcher.ComponentModel;

namespace AudioSwitcher.Presentation.CommandModel
{
    /// <summary>
    ///     Provides the base <see langword="abstract"/> class for commands that take no arguments.
    /// </summary>
    internal abstract class Command : ObservableObject, ICommand
    {
        private bool _isEnabled = true;
        private bool _isVisible = true;
        private bool _isInvokable = true;
        private bool _isChecked;
        private string _text;
        private string _tooltipText;
        private Image _image;

        protected Command()
            : this((string)null)
        {
        }

        protected Command(string text)
        {
            Text = text;
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (value != _isVisible)
                {
                    _isVisible = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set 
            {
                if (value != _isChecked)
                {
                    _isChecked = value;
                    RaisePropertyChanged();
                }
            }
        }


        public bool IsInvokable
        {
            get { return _isInvokable; }
            set
            {
                if (value != _isInvokable)
                {
                    _isInvokable = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set 
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Text
        {
            get { return IsVisible ? _text : null; }    // Prevent non-visible items from contributing to the size of context menu
            set 
            {
                if (value != _text)
                {
                    _text = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string TooltipText
        {
            get { return _tooltipText; }
            set
            {
                if (value != _tooltipText)
                {
                    _tooltipText = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Image Image
        {
            get { return IsVisible ? _image : null; }    // Prevent non-visible items from contributing to the size of context menu
            set
            {
                if (value != _image)
                {
                    _image = value;
                    RaisePropertyChanged();
                }
            }
        }

        public abstract void Run();

        public virtual void Refresh()
        {
        }

        void ICommand.Run(object argument)
        {
            if (argument != null)
                throw new ArgumentException();

            Run();
        }

        void ICommand.Refresh(object argument)
        {
            if (argument != null)
                throw new ArgumentException();

            Refresh();
        }
    }
}
