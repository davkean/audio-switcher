// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using AudioSwitcher.ComponentModel;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.Presentation.UI
{
    // Responsible for sync'ing between a Command and a ToolStripMenuItem
    internal class MenuItemCommandBinding
    {
        private static readonly Func<object> NoArgumentGetter = () => { return null; };

        private readonly ToolStripMenuItem _item;
        private readonly ToolStripDropDown _dropDown;
        private readonly Lifetime<ICommand> _lifetime;
        private readonly ICommand _command;
        private readonly Func<object> _argumentGetter;

        public MenuItemCommandBinding(ToolStripDropDown dropDown, ToolStripMenuItem item, Lifetime<ICommand> command)
            : this(dropDown, item, command, (Func<object>)null)
        {
        }

        public MenuItemCommandBinding(ToolStripDropDown dropDown, ToolStripMenuItem item, Lifetime<ICommand> command, Func<object> argumentGetter)
        {
            if (dropDown == null)
                throw new ArgumentNullException("dropDown");

            if (item == null)
                throw new ArgumentNullException("item");

            if (command == null)
                throw new ArgumentNullException("command");

            _dropDown = dropDown;
            _item = item;
            _command = command.Instance;
            _lifetime = command;
            _argumentGetter = argumentGetter ?? NoArgumentGetter;

            RegisterEvents();
            UpdateCommand();
        }

        private void RegisterEvents(bool register = true)
        {
            if (register)
            {
                _dropDown.Opening += OnContextMenuStripOpening;
                _dropDown.ItemRemoved += OnItemRemoved;
                _dropDown.ItemClicked += OnItemClicked;
                _command.PropertyChanged += OnCommandPropertyChanged;
            }
            else
            {
                _dropDown.Opening -= OnContextMenuStripOpening;
                _dropDown.ItemRemoved -= OnItemRemoved;
                _dropDown.ItemClicked -= OnItemClicked;
                _command.PropertyChanged -= OnCommandPropertyChanged;
            }
        }

        private void OnItemRemoved(object sender, ToolStripItemEventArgs e)
        {
			if (e.Item == _item)
				RegisterEvents(register: false);

            _lifetime.Dispose();
        }

        private void OnItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
			if (e.ClickedItem == _item)
                _command.Run(_argumentGetter());
        }

        private void UpdateCommand()
        {
            _command.UpdateStatus(_argumentGetter());
            SyncProperty(_command, CommandProperty.IsEnabled);
            SyncProperty(_command, CommandProperty.IsChecked);
            SyncProperty(_command, CommandProperty.Text);
            SyncProperty(_command, CommandProperty.Image);
            SyncProperty(_command, CommandProperty.TooltipText);
        }
        private void OnContextMenuStripOpening(object sender, CancelEventArgs e)
        {
            UpdateCommand();
        }

        private void OnCommandPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Command command = (Command)sender;

            SyncProperty(command, e.PropertyName);
        }

        private void SyncProperty(ICommand command, string propertyName)
        {
            switch (propertyName)
            {
                case CommandProperty.IsEnabled:
                    _item.Enabled = command.IsEnabled;
                    break;

                case CommandProperty.IsChecked:
                    _item.Checked = command.IsChecked;
                    break;

                case CommandProperty.Text:
                    _item.Text = command.Text;
                    break;

                case CommandProperty.TooltipText:
                    _item.ToolTipText = command.TooltipText;
                    break;

                case CommandProperty.Image:
				default:
                    Debug.Assert(propertyName == CommandProperty.Image);
                    _item.Image = command.Image;
                    break;
            }
        }
    }
}
