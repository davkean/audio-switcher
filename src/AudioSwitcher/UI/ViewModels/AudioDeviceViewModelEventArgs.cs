// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;

namespace AudioSwitcher.UI.ViewModels
{
    internal class AudioDeviceViewModelEventArgs
    {
        private readonly AudioDeviceViewModel _viewModel;

        public AudioDeviceViewModelEventArgs(AudioDeviceViewModel viewModel)
        {
            _viewModel = viewModel ?? throw new ArgumentNullException("viewModel");
        }

        public AudioDeviceViewModel ViewModel
        {
            get { return _viewModel; }
        }
    }
}
