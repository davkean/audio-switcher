// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;
using AudioSwitcher.ComponentModel;
using AudioSwitcher.Presentation.CommandModel;
using System.Drawing;

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

            foreach (ToolStripItem item in strip.Items)
            {
                item.RefreshCommand(true);
            }
        }

        public static void RefreshCommand(this ToolStripItem item, bool refreshChildren = false)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            
            ToolStripItemCommandBinding binding = (ToolStripItemCommandBinding)item.Tag;
            if (binding != null)
                binding.Refresh();

            if (refreshChildren)
            {
                ToolStripMenuItem menuItem = item as ToolStripMenuItem;
                if (menuItem != null)
                {
                    RefreshCommands(menuItem.DropDown);
                }
            }
        }

        public static ToolStripSeparator BindSeparator(this ToolStripDropDown dropDown, CommandManager commandManager, string commandId)
        {
            Lifetime<ICommand> command = commandManager.FindCommand(commandId);
            if (command == null)
                throw new ArgumentException();

            ToolStripSeparator item = dropDown.AddSeparator();
            item.Tag = new ToolStripItemCommandBinding(dropDown, item, command, (object)null);

            return item;
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
            item.Tag = new ToolStripItemCommandBinding(dropDown, item, command, argument);

            return item;
        }

        public static ToolStripMenuItem Add(this ToolStripDropDown dropDown, string text)
        {
            return (ToolStripMenuItem)dropDown.Items.Add(text);
        }

        public static ToolStripSeparator AddSeparator(this ToolStripDropDown dropDown)
        {
            return (ToolStripSeparator)dropDown.Items.Add("-");
        }

        public static object GetArgument(this ToolStripItem item)
        {
            ToolStripItemCommandBinding binding = (ToolStripItemCommandBinding)item.Tag;
            if (binding != null)
            {
                return binding.Argument;
            }

            return null;
        }

		public static Point ToParentPoint(this ToolStripItem item, Point point)
		{
            Point itemLocation = item.Bounds.Location;

			return new Point(point.X + itemLocation.X, point.Y + itemLocation.Y);
		}

		public static Point ToScreenPoint(this ToolStripItem item, Point point)
		{
			Point parentLocation = ToParentPoint(item, point);

			return item.GetCurrentParent().PointToScreen(parentLocation);
		}
    }
}
