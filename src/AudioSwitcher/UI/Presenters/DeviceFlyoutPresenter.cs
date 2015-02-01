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
        private readonly PresenterHost _presenterManager;

        [ImportingConstructor]
        public DeviceFlyoutPresenter(IApplication application, AudioDeviceViewModelManager viewModelManager, CommandManager commandManager, PresenterHost presenterManager)
            : base(application)
        {
            _viewModelManager = viewModelManager;
            _viewModelManager.Changed += OnViewModelsChanged;
            _commandManager = commandManager;
            _presenterManager = presenterManager;
        }

        protected override AudioContextMenuStrip CreateContextMenu()
        {
            return new DeviceFlyoutView();
        }

        private void OnViewModelsChanged(object sender, System.EventArgs e)
        {
            ContextMenu.RefreshCommands();
        }

        public override void Bind()
        {
            AddDeviceCommands(AudioDeviceKind.Playback, CommandId.NoPlaybackDevices);
            AddDeviceCommands(AudioDeviceKind.Recording, CommandId.NoRecordingDevices);

            ContextMenu.BindCommand(_commandManager, CommandId.NoDevices);
        }

        public override void Dispose()
        {
            base.Dispose();

            _viewModelManager.Changed -= OnViewModelsChanged;
        }

        private void AddDeviceCommands(AudioDeviceKind kind, string noDeviceCommandId)
        {
            ContextMenu.AddSeparatorIfNeeded();

            AudioDeviceViewModel[] devices = GetDevices(kind);
            foreach (AudioDeviceViewModel device in devices)
            {
                var item = ContextMenu.BindCommand(_commandManager, CommandId.SetAsDefaultDevice, device);
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
				AudioToolStripMenuItem item = (AudioToolStripMenuItem)sender;
				item.DropDown.BindCommand(_commandManager, CommandId.SetAsDefaultMultimediaDevice, item.GetArgument());
				item.DropDown.BindCommand(_commandManager, CommandId.SetAsDefaultCommunicationDevice, item.GetArgument());
				item.DropDownClosed += OnDropDownClosed;
				item.DropDown.Capture = true;
				item.ShowDropDown(e.Location, ToolStripDropDownDirection.BelowRight);
			}
		}

		private void OnDropDownClosed(object sender, EventArgs e)
		{
			ToolStripMenuItem item = (ToolStripMenuItem)sender;
			item.DropDownClosed -= OnDropDownClosed;
			item.DropDown.Items.Clear();
		}
    }
}
