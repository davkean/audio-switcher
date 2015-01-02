// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Windows.Forms;
using AudioSwitcher.ComponentModel;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.Presentation.UI
{
    internal static partial class ToolStripExtensions
    {
        public static ToolStripMenuItem BindCommand(this ToolStripDropDown dropDown, CommandManager commandManager, string commandId)
        {
            return dropDown.BindCommand(commandManager, commandId, (Func<object>)null);
        }

        public static ToolStripMenuItem BindCommand(this ToolStripDropDown dropDown, CommandManager commandManager, string commandId, Func<object> argumentGetter)
        {
            Lifetime<ICommand> command = commandManager.FindCommand(commandId);
            if (command == null)
                throw new ArgumentException();

            return dropDown.BindCommand(command, argumentGetter);
        }

        private static ToolStripMenuItem BindCommand(this ToolStripDropDown dropDown, Lifetime<ICommand> command, Func<object> argumentGetter)
        {
            ToolStripMenuItem item = dropDown.Add(string.Empty);

            // Should be kept alive by the strip and command hookups.
            new MenuItemCommandBinding(dropDown, item, command, argumentGetter);

            return item;
        }

        public static ToolStripMenuItem Add(this ToolStripDropDown dropDown, string text)
        {
            return (ToolStripMenuItem)dropDown.Items.Add(text);
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
