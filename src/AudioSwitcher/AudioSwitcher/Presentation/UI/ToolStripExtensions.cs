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
        public static ToolStripMenuItem AddCommand(this ToolStripDropDown dropDown, Command command)
        {
            AudioToolStripMenuItem item = new AudioToolStripMenuItem();
            item.Tag = command;
            item.ImageScaling = ToolStripItemImageScaling.None;
            dropDown.Items.Add(item);

            // Should be kept alive by the strip and command hookups.
            new CommandBinding(dropDown, item, command);

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
