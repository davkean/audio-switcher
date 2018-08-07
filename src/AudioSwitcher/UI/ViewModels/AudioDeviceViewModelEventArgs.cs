// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;

namespace AudioSwitcher.UI.ViewModels
{
    internal class AudioDeviceViewModelEventArgs
    {
        private AudioDeviceViewModel _viewModel;

        public AudioDeviceViewModelEventArgs(AudioDeviceViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException("viewModel");

            _viewModel = viewModel;
        }

        public AudioDeviceViewModel ViewModel
        {
            get { return _viewModel; }
        }
    }
}
