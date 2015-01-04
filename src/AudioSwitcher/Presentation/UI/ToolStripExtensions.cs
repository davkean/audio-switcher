// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;
using AudioSwitcher.ComponentModel;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.Presentation.UI
{
    internal static partial class ToolStripExtensions
    {
        public static void RefreshCommands(this ToolStrip strip)
        {
            if (strip == null)
                throw new ArgumentNullException("strip");

            // If the strip itself is not visible, don't update the items
            if (!strip.Visible)
                return;

            foreach (ToolStripMenuItem item in strip.Items.OfType<ToolStripMenuItem>())
            {
                item.RefreshCommand(true);
            }
        }

        public static void RefreshCommand(this ToolStripMenuItem item, bool refreshChildren = false)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            
            MenuItemCommandBinding binding = (MenuItemCommandBinding)item.Tag;
            if (binding != null)
                binding.Refresh();

            if (refreshChildren)
            {
                RefreshCommands(item.DropDown);
            }
        }

        public static ToolStripMenuItem BindCommand(this ToolStripDropDown dropDown, CommandManager commandManager, string commandId)
        {
            return dropDown.BindCommand(commandManager, commandId, (object)null);
        }

        public static ToolStripMenuItem BindCommand(this ToolStripDropDown dropDown, CommandManager commandManager, string commandId, object argument)
        {
            Lifetime<ICommand> command = commandManager.FindCommand(commandId);
            if (command == null)
                throw new ArgumentException();

            return dropDown.BindCommand(command, argument);
        }

        private static ToolStripMenuItem BindCommand(this ToolStripDropDown dropDown, Lifetime<ICommand> command, object argument)
        {
            ToolStripMenuItem item = dropDown.Add(string.Empty);
            item.Tag = new MenuItemCommandBinding(dropDown, item, command, argument);

            return item;
        }

        public static ToolStripMenuItem Add(this ToolStripDropDown dropDown, string text)
        {
            return (ToolStripMenuItem)dropDown.Items.Add(text);
        }

        public static ToolStripMenuItem AddDisabled(this ToolStripDropDown dropDown, string text)
        {
            ToolStripMenuItem item = dropDown.Add(text);
            item.Enabled = false;

            return item;
        }

        public static void AddSeparator(this ToolStripDropDown dropDown)
        {
            dropDown.Items.Add("-");
        }

        public static void AddSeparatorIfNeeded(this ToolStripDropDown dropDown)
        {
            if (dropDown.Items.Count != 0)
                dropDown.AddSeparator();
        }
    }
}
