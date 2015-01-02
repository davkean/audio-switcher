// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Windows.Forms;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.Presentation.UI
{
    internal static partial class ToolStripExtensions
    {
        public static ToolStripMenuItem AddCommand(this ToolStripDropDown dropDown, CommandManager commandManager, string commandId)
        {
            return AddCommand(dropDown, commandManager, commandId, (Func<object>)null);
        }

        public static ToolStripMenuItem AddCommand(this ToolStripDropDown dropDown, CommandManager commandManager, string commandId, Func<object> argumentGetter)
        {
            Lifetime<ICommand> command = commandManager.FindCommand(commandId);
            if (command == null)
                throw new ArgumentException();

            return AddCommand(dropDown, command, argumentGetter);
        }

        private static ToolStripMenuItem AddCommand(this ToolStripDropDown dropDown, Lifetime<ICommand> command)
        {
            return AddCommand(dropDown, command, (Func<object>)null);
        }

        private static ToolStripMenuItem AddCommand(this ToolStripDropDown dropDown, Lifetime<ICommand> command, Func<object> argumentGetter)
        {
            AudioToolStripMenuItem item = new AudioToolStripMenuItem();
            item.Tag = command;
            item.ImageScaling = ToolStripItemImageScaling.None;
            dropDown.Items.Add(item);

            // Should be kept alive by the strip and command hookups.
            new MenuItemCommandBinding(dropDown, item, command, argumentGetter);

            return item;
        }

        public static ToolStripDropDown AddNestedItem(this ToolStripDropDown dropDown, string text)
        {
            AudioToolStripMenuItem item = new AudioToolStripMenuItem();
            item.Text = text;
            dropDown.Items.Add(item);

            return item.DropDown;
        }

        public static ToolStripDropDown AddNestedCommand(this ToolStripDropDown dropDown, CommandManager commandManager, string commandId, Func<object> argumentGetter)
        {
            ToolStripMenuItem item = dropDown.AddCommand(commandManager, commandId, argumentGetter);

            return item.DropDown;
        }

        public static void AddSeparator(this ToolStripDropDown dropDown)
        {
            dropDown.Items.Add(new ToolStripSeparator());
        }

        public static void AddSeparatorIfNeeded(this ToolStripDropDown dropDown)
        {
            if (dropDown.Items.Count != 0)
                dropDown.AddSeparator();
        }
    }
}
