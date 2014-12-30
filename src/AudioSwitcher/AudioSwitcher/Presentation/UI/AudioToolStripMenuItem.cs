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
        private Image _checkedImage;
        private bool _doNotCheckDropDownLocation;

        public AudioToolStripMenuItem()
        {
        }

        public Image CheckedImage
        {
            get { return _checkedImage; }
            set
            {
                if (value != _checkedImage)
                {
                    _checkedImage = value;
                    Invalidate();
                }
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
            return new AudioContextMenu();
        }
    }
}
