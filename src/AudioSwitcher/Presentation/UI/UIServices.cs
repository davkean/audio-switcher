// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace AudioSwitcher.Presentation.UI
{
    internal static class UIServices
    {
        public static ToolStripDropDownDirection FlipDirectionOnXAxis(ToolStripDropDownDirection direction)
        {
            switch (direction)
            {
                case ToolStripDropDownDirection.Left:
                    return ToolStripDropDownDirection.Right;

                case ToolStripDropDownDirection.Right:
                    return ToolStripDropDownDirection.Left;

                case ToolStripDropDownDirection.AboveLeft:
                    return ToolStripDropDownDirection.AboveRight;

                    case ToolStripDropDownDirection.AboveRight:
                    return ToolStripDropDownDirection.AboveLeft;

                case ToolStripDropDownDirection.BelowLeft:
                    return ToolStripDropDownDirection.BelowRight;

                default:
                    Debug.Assert(direction == ToolStripDropDownDirection.BelowRight);
                    return ToolStripDropDownDirection.BelowLeft;
            }
        }

        public static bool BelongToSameScreen(Point point1, Point point2)
        {
            var screen1 = Screen.FromPoint(point1);
            var screen2 = Screen.FromPoint(point2);

            return screen1.Equals(screen2);
        }
    }
}
