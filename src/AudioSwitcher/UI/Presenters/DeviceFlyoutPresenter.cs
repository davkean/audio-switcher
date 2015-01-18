// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using AudioSwitcher.ApplicationModel;
using AudioSwitcher.Audio;
using AudioSwitcher.Presentation;
using AudioSwitcher.Presentation.CommandModel;
using AudioSwitcher.Presentation.UI;
using AudioSwitcher.Presentation.UI.Views;
using AudioSwitcher.UI.Commands;
using AudioSwitcher.UI.ViewModels;

namespace AudioSwitcher.UI.Presenters
{
    // Presents the device context flyout window when left-clicking on the notification icon
    [Presenter(PresenterId.DeviceFlyout, IsToggle=true)]
    internal class DeviceFlyoutPresenter : ContextMenuPresenter, IDisposable
    {
        private readonly AudioDeviceViewModelManager _viewModelManager;
        private readonly CommandManager _commandManager;
        private readonly PresenterManager _presenterManager;

        [ImportingConstructor]
        public DeviceFlyoutPresenter(IApplication application, AudioDeviceViewModelManager viewModelManager, CommandManager commandManager, PresenterManager presenterManager)
            : base(application)
        {
            _viewModelManager = viewModelManager;
            _viewModelManager.Changed += OnViewModelsChanged;
            _commandManager = commandManager;
            _presenterManager = presenterManager;
        }

        public override void Dispose()
        {
            base.Dispose();

            _viewModelManager.Changed -= OnViewModelsChanged;
        }

        protected override AudioContextMenuStrip CreateContextMenu()
        {
            return new DeviceFlyoutView();
        }

        private void OnViewModelsChanged(object sender, System.EventArgs e)
        {
            ContextMenu.RefreshCommands();
        }

        protected override void Bind()
        {
            AddDeviceCommands(AudioDeviceKind.Playback, CommandId.NoPlaybackDevices);
            AddDeviceCommands(AudioDeviceKind.Recording, CommandId.NoRecordingDevices);

            ContextMenu.BindCommand(_commandManager, CommandId.NoDevices);
        }

        private void AddDeviceCommands(AudioDeviceKind kind, string noDeviceCommandId)
        {
            ContextMenu.AddSeparatorIfNeeded();

            AudioDeviceViewModel[] devices = GetDevices(kind);
            foreach (AudioDeviceViewModel device in devices)
            {
                ContextMenu.BindCommand(_commandManager, CommandId.SetAsDefaultDevice, device);
            }

            ContextMenu.BindCommand(_commandManager, noDeviceCommandId);
        }

        private AudioDeviceViewModel[] GetDevices(AudioDeviceKind kind)
        {
            return _viewModelManager.ViewModels.Where(v => v.Device.Kind == kind)
                                               .ToArray();
        }
    }
}
