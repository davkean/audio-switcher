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
            Command command = commandManager.FindCommand(commandId);
            if (command == null)
                throw new ArgumentException();

            return AddCommand(dropDown, command, argumentGetter);
        }

        public static ToolStripMenuItem AddCommand(this ToolStripDropDown dropDown, Command command)
        {
            return AddCommand(dropDown, command, (Func<object>)null);
        }

        public static ToolStripMenuItem AddCommand(this ToolStripDropDown dropDown, Command command, Func<object> argumentGetter)
        {
            AudioToolStripMenuItem item = new AudioToolStripMenuItem();
            item.Tag = command;
            item.ImageScaling = ToolStripItemImageScaling.None;
            dropDown.Items.Add(item);

            // Should be kept alive by the strip and command hookups.
            new CommandBinding(dropDown, item, command, argumentGetter);

            return item;
        }

        public static ToolStripDropDown AddNestedItem(this ToolStripDropDown dropDown, string text)
        {
            AudioToolStripMenuItem item = new AudioToolStripMenuItem();
            item.Text = text;
            dropDown.Items.Add(item);

            return item.DropDown;
        }

        public static ToolStripDropDown AddNestedCommand(this ToolStripDropDown dropDown, Command command)
        {
            ToolStripMenuItem item = dropDown.AddCommand(command);

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
