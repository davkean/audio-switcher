// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace AudioSwitcher.Presentation.CommandModel
{
    // Represents a command that takes no arguments
    internal abstract class Command : INotifyPropertyChanged, ICommand
    {
        private bool _isEnabled = true;
        private bool _isChecked;
        private bool _isBulleted;
        private string _text;
        private string _tooltipText;
        private Image _image;
        private Image _checkedImage;

        protected Command()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsChecked
        {
            get { return _isChecked; }
            set 
            {
                if (value != _isChecked)
                {
                    _isChecked = value;
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                }
            }
        }

        public abstract void Run();

        public virtual void UpdateStatus()
        {
        }

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
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
