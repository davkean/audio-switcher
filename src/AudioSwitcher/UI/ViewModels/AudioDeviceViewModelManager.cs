// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using AudioSwitcher.Audio;

namespace AudioSwitcher.UI.ViewModels
{
    [Export(typeof(AudioDeviceViewModelManager))]
    internal class AudioDeviceViewModelManager : IDisposable
    {
        private readonly AudioDeviceManager _deviceManager;
        private readonly AudioDeviceViewModelCollection _viewModels = new AudioDeviceViewModelCollection();

        [ImportingConstructor]
        public AudioDeviceViewModelManager(AudioDeviceManager deviceManager)
        {
            _deviceManager = deviceManager;
            
            RegisterHandlers();
            AddAll();
        }

        public event EventHandler<AudioDeviceViewModelEventArgs> ViewModelPropertyChanged;
        public event EventHandler<AudioDeviceViewModelEventArgs> ViewModelRemoved;
        public event EventHandler<AudioDeviceViewModelEventArgs> ViewModelAdded;

        public ReadOnlyCollection<AudioDeviceViewModel> ViewModels
        {
            get { return _viewModels; }
        }

        public void Dispose()
        {
            RegisterHandlers(false);
        }

        protected virtual void OnViewModelPropertyChanged(AudioDeviceViewModelEventArgs e)
        {
            var handler = ViewModelPropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnViewModelAdded(AudioDeviceViewModelEventArgs e)
        {
            var handler = ViewModelAdded;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnViewModelRemoved(AudioDeviceViewModelEventArgs e)
        {
            var handler = ViewModelRemoved;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void AddAll()
        {
            foreach (AudioDevice device in _deviceManager.GetAudioDevices(AudioDeviceKind.All, AudioDeviceState.All))
            {
                AddViewModel(device);
            }
        }

        private AudioDeviceViewModel AddViewModel(AudioDevice device)
        {
            AudioDeviceViewModel model = new AudioDeviceViewModel(device);
            model.UpdateStatus(_deviceManager);

            _viewModels.Add(model);

            return model;
        }

        private AudioDeviceViewModel FindViewModel(string id)
        {
            return _viewModels.Where(m => StringComparers.DeviceIds.Equals(m.Device.Id, id))
                              .SingleOrDefault();
        }

        private void OnDeviceStateChanged(object sender, AudioDeviceStateEventArgs e)
        {
            AudioDeviceViewModel viewModel = FindViewModel(e.Device.Id);
            if (viewModel != null)
            {
                viewModel.UpdateStatus(_deviceManager);

                OnViewModelPropertyChanged(new AudioDeviceViewModelEventArgs(viewModel));
            }
        }

        private void OnDevicePropertyChanged(object sender, AudioDeviceEventArgs e)
        {
            AudioDeviceViewModel viewModel = FindViewModel(e.Device.Id);
            if (viewModel != null)
            {
                viewModel.UpdateStatus(_deviceManager);

                OnViewModelPropertyChanged(new AudioDeviceViewModelEventArgs(viewModel));
            }
        }

        private void OnDeviceAdded(object sender, AudioDeviceEventArgs e)
        {
            AudioDeviceViewModel viewModel = AddViewModel(e.Device);

            OnViewModelAdded(new AudioDeviceViewModelEventArgs(viewModel));
        }

        private void OnDeviceRemoved(object sender, AudioDeviceRemovedEventArgs e)
        {
            AudioDeviceViewModel viewModel = FindViewModel(e.DeviceId);
            if (viewModel != null)
            {
                _viewModels.Remove(viewModel);
                OnViewModelRemoved(new AudioDeviceViewModelEventArgs(viewModel));
            }
        }

        private void OnDefaultDeviceChanged(object sender, DefaultAudioDeviceEventArgs e)
        {
            foreach (AudioDeviceViewModel viewModel in _viewModels)
            {
                viewModel.UpdateStatus(_deviceManager);
                OnViewModelPropertyChanged(new AudioDeviceViewModelEventArgs(viewModel));
            }
        }

        private void RegisterHandlers(bool register = true)
        {
            if (register)
            {
                _deviceManager.DefaultDeviceChanged += OnDefaultDeviceChanged;
                _deviceManager.DeviceAdded += OnDeviceAdded;
                _deviceManager.DevicePropertyChanged += OnDevicePropertyChanged;
                _deviceManager.DeviceRemoved += OnDeviceRemoved;
                _deviceManager.DeviceStateChanged += OnDeviceStateChanged;
                Settings.Default.PropertyChanged += OnSettingsChanged;  
            }
            else
            {
                _deviceManager.DefaultDeviceChanged -= OnDefaultDeviceChanged;
                _deviceManager.DeviceAdded -= OnDeviceAdded;
                _deviceManager.DevicePropertyChanged -= OnDevicePropertyChanged;
                _deviceManager.DeviceRemoved -= OnDeviceRemoved;
                _deviceManager.DeviceStateChanged -= OnDeviceStateChanged;
                Settings.Default.PropertyChanged -= OnSettingsChanged;
            }
        }

        private void OnSettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            foreach (AudioDeviceViewModel viewModel in _viewModels)
            {
                viewModel.UpdateStatus(_deviceManager);
                OnViewModelPropertyChanged(new AudioDeviceViewModelEventArgs(viewModel));
            }
        }

        private class AudioDeviceViewModelCollection : ReadOnlyCollection<AudioDeviceViewModel>
        {
            public AudioDeviceViewModelCollection()
                : base(new Collection<AudioDeviceViewModel>())
            {
            }

            public void Add(AudioDeviceViewModel item)
            {
                Items.Add(item);
            }

            public void Remove(AudioDeviceViewModel item)
            {
                Items.Remove(item);
            }
        }
    }
}
