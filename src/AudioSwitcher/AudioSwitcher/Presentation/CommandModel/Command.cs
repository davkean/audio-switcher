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
    // Represents a command that takes no arguments
    internal abstract class Command : ObservableObject, ICommand
    {
        private bool _isEnabled = true;
        private bool _isChecked;
        private bool _isBulleted;
        private string _text;
        private string _tooltipText;
        private Image _image;
        private Image _checkedImage;

        protected Command()
            : this((string)null)
        {
        }

        protected Command(string text)
        {
            Text = text;
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

        public bool IsBulleted
        {
            get { return _isBulleted; }
            set
            {
                if (value != _isBulleted)
                {
                    _isBulleted = value;
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
            get { return _text; }
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
            get { return _image; }
            set
            {
                if (value != _image)
                {
                    _image = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Image CheckedImage
        {
            get { return _checkedImage; }
            set
            {
                if (value != _checkedImage)
                {
                    _checkedImage = value;
                    RaisePropertyChanged();
                }
            }
        }

        public abstract void Run();

        public virtual void UpdateStatus()
        {
        }

        void ICommand.Run(object argument)
        {
            if (argument != null)
                throw new ArgumentException();

            Run();
        }

        void ICommand.UpdateStatus(object argument)
        {
            if (argument != null)
                throw new ArgumentException();

            UpdateStatus();
        }
    }
}
