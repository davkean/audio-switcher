// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.Collections.ObjectModel;
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

        public event EventHandler Changed;

        public ReadOnlyCollection<AudioDeviceViewModel> ViewModels
        {
            get { return _viewModels; }
        }

        public void Dispose()
        {
            RegisterHandlers(false);
        }

        protected virtual void OnChanged(EventArgs e)
        {
            var handler = Changed;
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

        private void AddViewModel(AudioDevice device)
        {
            AudioDeviceViewModel model = new AudioDeviceViewModel(device);
            model.UpdateStatus(_deviceManager);

            _viewModels.Add(model);
        }

        private AudioDeviceViewModel FindViewModel(string id)
        {
            return _viewModels.Where(m => m.Device.Id == id)
                              .SingleOrDefault();
        }

        private void OnDeviceStateChanged(object sender, AudioDeviceStateEventArgs e)
        {
            AudioDeviceViewModel viewModel = FindViewModel(e.Device.Id);

            Debug.Assert(viewModel != null);
            viewModel.UpdateStatus(_deviceManager);

            OnChanged(EventArgs.Empty);
        }

        private void OnDevicePropertyChanged(object sender, AudioDeviceEventArgs e)
        {
            AudioDeviceViewModel viewModel = FindViewModel(e.Device.Id);

            Debug.Assert(viewModel != null);
            viewModel.UpdateStatus(_deviceManager);

            OnChanged(EventArgs.Empty);
        }

        private void OnDeviceAdded(object sender, AudioDeviceEventArgs e)
        {
            AddViewModel(e.Device);

            OnChanged(EventArgs.Empty);
        }

        private void OnDeviceRemoved(object sender, AudioDeviceRemovedEventArgs e)
        {
            AudioDeviceViewModel viewModel = FindViewModel(e.DeviceId);

            if (viewModel != null)
            {
                _viewModels.Remove(viewModel);
            }

            OnChanged(EventArgs.Empty);
        }

        private void OnDefaultDeviceChanged(object sender, DefaultAudioDeviceEventArgs e)
        {
            foreach (AudioDeviceViewModel model in _viewModels)
            {
                model.UpdateStatus(_deviceManager);
            }

            OnChanged(EventArgs.Empty);
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
            }
            else
            {
                _deviceManager.DefaultDeviceChanged -= OnDefaultDeviceChanged;
                _deviceManager.DeviceAdded -= OnDeviceAdded;
                _deviceManager.DevicePropertyChanged -= OnDevicePropertyChanged;
                _deviceManager.DeviceRemoved -= OnDeviceRemoved;
                _deviceManager.DeviceStateChanged -= OnDeviceStateChanged;
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
