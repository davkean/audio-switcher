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

        [ImportingConstructor]
        public DeviceFlyoutPresenter(IApplication application, AudioDeviceViewModelManager viewModelManager, CommandManager commandManager)
            : base(application)
        {
            _viewModelManager = viewModelManager;
            _commandManager = commandManager;
        }

        protected override AudioContextMenuStrip CreateContextMenu()
        {
            return new DeviceFlyoutView();
        }

        public override void Bind()
        {
            RegisterHandlers();

            AddDeviceCommands(AudioDeviceKind.Playback, CommandId.NoPlaybackDevices);

            ContextMenu.BindSeparator(_commandManager, CommandId.DeviceSeparator);

            AddDeviceCommands(AudioDeviceKind.Recording, CommandId.NoRecordingDevices);

            ContextMenu.BindCommand(_commandManager, CommandId.NoDevices);
        }

        public override void Dispose()
        {
            base.Dispose();

            RegisterHandlers(add:false);
        }

        private void AddDeviceCommands(AudioDeviceKind kind, string noDeviceCommandId)
        {
            AudioDeviceViewModel[] devices = GetDevices(kind);
            foreach (AudioDeviceViewModel device in devices)
            {
                ToolStripMenuItem item = ContextMenu.BindCommand(_commandManager, CommandId.SetAsDefaultDevice, device);
                item.DropDownDirection = ToolStripDropDownDirection.AboveRight;
				item.MouseUp += OnItemMouseUp;
            }

            ContextMenu.BindCommand(_commandManager, noDeviceCommandId);
        }

        private AudioDeviceViewModel[] GetDevices(AudioDeviceKind kind)
        {
            return _viewModelManager.ViewModels.Where(v => v.Device.Kind == kind)
                                               .ToArray();
        }

		private void OnItemMouseUp(object sender, MouseEventArgs e)
		{
            // TODO: This should be WM_CONTEXTMENU
			if (e.Button == MouseButtons.Right)
			{
                var item = (AudioToolStripMenuItem)sender;
				item.DropDown.BindCommand(_commandManager, CommandId.SetAsDefaultMultimediaDevice, item.GetArgument());
				item.DropDown.BindCommand(_commandManager, CommandId.SetAsDefaultCommunicationDevice, item.GetArgument());
				item.DropDownClosed += OnDropDownClosed;
                item.DropDown.Capture = true;
				item.ShowDropDown(e.Location);
			}
		}

		private void OnDropDownClosed(object sender, EventArgs e)
		{
			var item = (ToolStripMenuItem)sender;
			item.DropDownClosed -= OnDropDownClosed;
			item.DropDown.Items.Clear();
		}

        private void RegisterHandlers(bool add = true)
        {
            if (add)
            {
                _viewModelManager.ViewModelPropertyChanged += OnViewModelsChanged;
                _viewModelManager.ViewModelAdded += OnViewModelsAdded;
                _viewModelManager.ViewModelRemoved += OnViewModelsRemoved;
            }
            else
            {
                _viewModelManager.ViewModelPropertyChanged -= OnViewModelsChanged;
                _viewModelManager.ViewModelAdded -= OnViewModelsAdded;
                _viewModelManager.ViewModelRemoved -= OnViewModelsRemoved;
            }
        }

        private void OnViewModelsChanged(object sender, AudioDeviceViewModelEventArgs e)
        {
            // We refresh all commmands, so that non-device commands 
            // (such as separaters, "no devices" text, etc are 
            // updated based on state changes.
            ContextMenu.RefreshCommands();
        }

        private void OnViewModelsRemoved(object sender, AudioDeviceViewModelEventArgs e)
        {
            ToolStripMenuItem item = FindMenuItem(e.ViewModel);
            if (item != null)
            {
                ContextMenu.Items.Remove(item);
            }
        }

        private void OnViewModelsAdded(object sender, AudioDeviceViewModelEventArgs e)
        {   
        }

        private ToolStripMenuItem FindMenuItem(AudioDeviceViewModel viewModel)
        {
            return ContextMenu.Items.OfType<ToolStripMenuItem>()
                                    .Where(m => m.GetArgument() == viewModel)
                                    .SingleOrDefault();
        }
    }
}
