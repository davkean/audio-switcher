// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Media;
using System.Windows.Forms;
using AudioSwitcher.ComponentModel;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.Presentation.UI
{
    // Responsible for sync'ing between a Command and a ToolStripMenuItem
    internal class ToolStripItemCommandBinding
    {
        private readonly ToolStripItem _item;
        private readonly ToolStripMenuItem _menuItem;
        private readonly ToolStripDropDown _dropDown;
        private readonly Lifetime<ICommand> _lifetime;
        private readonly ICommand _command;
        private readonly object _argument;

        public ToolStripItemCommandBinding(ToolStripDropDown dropDown, ToolStripItem item, Lifetime<ICommand> command, object argument)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            _dropDown = dropDown ?? throw new ArgumentNullException("dropDown");
            _item = item ?? throw new ArgumentNullException("item");
            _menuItem = item as ToolStripMenuItem;
            _command = command.Instance;
            _lifetime = command;
            _argument = argument;

            RegisterEvents();
            Refresh();
        }

        public object Argument
        {
            get { return _argument; }
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
            if (e.Item != _item)
                return;

            RegisterEvents(register: false);
            _lifetime.Dispose();
        }

        private void OnItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem != _item)
                return;
            
            if (_command.IsInvokable)
            {
                _command.Run(_argument);
            }
            else
            {
                SystemSounds.Beep.Play();
            }
        }

        public void Refresh()
        {
            _command.Refresh(_argument);
            SyncProperty(_command, CommandProperty.IsInvokable);
            SyncProperty(_command, CommandProperty.IsVisible);
            SyncProperty(_command, CommandProperty.IsEnabled);
            SyncProperty(_command, CommandProperty.IsChecked);
            SyncProperty(_command, CommandProperty.Text);
            SyncProperty(_command, CommandProperty.Image);
            SyncProperty(_command, CommandProperty.TooltipText);
        }

        private void OnContextMenuStripOpening(object sender, CancelEventArgs e)
        {
            Refresh();
        }

        private void OnCommandPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var command = (Command)sender;

            SyncProperty(command, e.PropertyName);
        }

        private void SyncProperty(ICommand command, string propertyName)
        {
            switch (propertyName)
            {
                case CommandProperty.IsVisible:
                    _item.Visible = command.IsVisible;
                    break;

                case CommandProperty.IsEnabled:
                    _item.Enabled = command.IsEnabled;
                    break;

                case CommandProperty.IsChecked:
                    if (_menuItem != null)
                    {
                        _menuItem.Checked = command.IsChecked;
                    }
                    break;

                case CommandProperty.Text:
                    _item.Text = command.Text;
                    break;

                case CommandProperty.TooltipText:
                    _item.ToolTipText = command.TooltipText;
                    break;

                case CommandProperty.IsInvokable:
                    if (_item is AudioToolStripMenuItem item)
                    {
                        item.AutoCloseOnClick = command.IsInvokable;
                    }
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
