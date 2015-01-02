// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;

namespace AudioSwitcher.Presentation.UI
{
    // See http://msdn2.microsoft.com/en-us/library/bb773210.aspx - "Parts and States"
    // Only menu-related parts/states are needed here, VisualStyleRenderer handles most of the rest.
    internal enum MenuParts : int
    {
        ItemTMSchema = 1,
        DropDownTMSchema = 2,
        BarItemTMSchema = 3,
        BarDropDownTMSchema = 4,
        ChevronTMSchema = 5,
        SeparatorTMSchema = 6,
        BarBackground = 7,
        BarItem = 8,
        PopupBackground = 9,
        PopupBorders = 10,
        PopupCheck = 11,
        PopupCheckBackground = 12,
        PopupGutter = 13,
        PopupItem = 14,
        PopupSeparator = 15,
        PopupSubmenu = 16,
        SystemClose = 17,
        SystemMaximize = 18,
        SystemMinimize = 19,
        SystemRestore = 20
    }
}
