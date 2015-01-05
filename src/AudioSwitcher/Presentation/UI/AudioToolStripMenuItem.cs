// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AudioSwitcher.Presentation.UI
{
    internal class AudioToolStripMenuItem : ToolStripMenuItem
    {
        private bool _doNotCheckDropDownLocation;

        public AudioToolStripMenuItem()
        {
            ImageScaling = ToolStripItemImageScaling.None;
        }

        protected override void OnParentChanged(ToolStrip oldParent, ToolStrip newParent)
        {
            if (oldParent != null)
                oldParent.LocationChanged -= OnParentLocationChanged;

            base.OnParentChanged(oldParent, newParent);

            if (newParent != null)
                newParent.LocationChanged += OnParentLocationChanged;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            // To prevent sub menus from being orphaned, 
            // hide them when our visibility changes
            if (!Visible)
            {
                HideDropDown();
            }
        }

        private void OnParentLocationChanged(object sender, EventArgs e)
        {
            // To prevent sub menus from being misaligned when additional menu items are added or made visible and the
            // toolstrip is repositioned, we fix up the position of the sub menu if our parent's location is changed. 
            // Make note, that if the menu item physically moves inside the strip, WinForms already fixes it up, so 
            // we don't need to handle that case.
            if (HasDropDownItems && DropDown.Visible)
            {
                DropDown.Location = DropDownLocation;
            }
        }

        protected override Point DropDownLocation
        {
            get
            {
                // BUG #3: The default location of a menu's drop down can appear on a different screen than the parent
                // of that menu. To prevent that, we prefer the reverse side of the current direction, if that results 
                // in the drop down appearing on the same screen.
                

                Point location = base.DropDownLocation;
                if (_doNotCheckDropDownLocation || Parent == null || UIServices.BelongToSameScreen(location, Parent.Location))
                    return location;

                // Unfortunately, ToolStripDownDownItem.GetDropDownBounds isn't public, and there doesn't appear
                // to be a better place to hook onto this, so we need to modify state on the MenuItem (and then 
                // reset it) to mimic this method.

                _doNotCheckDropDownLocation = true; // Prevent cycles
                ToolStripDropDownDirection currentDirection = DropDownDirection;
                DropDownDirection = UIServices.FlipDirectionOnXAxis(currentDirection);
                Point newLocation = base.DropDownLocation;

                if (UIServices.BelongToSameScreen(newLocation, Parent.Location))
                    location = newLocation; // Better match

                DropDownDirection = currentDirection;
                _doNotCheckDropDownLocation = false;
                return location;
            }
        }

        protected override ToolStripDropDown CreateDefaultDropDown()
        {
            return new AudioContextMenuStrip();
        }
    }
}
