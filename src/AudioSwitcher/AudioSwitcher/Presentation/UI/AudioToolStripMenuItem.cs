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

        protected override ToolStripDropDown CreateDefaultDropDown()
        {
            return new AudioContextMenu();
        }
    }
}
