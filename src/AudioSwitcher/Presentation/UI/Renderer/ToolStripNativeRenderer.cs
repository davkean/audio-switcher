// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
// Thanks for fixes:
//  * Marco Minerva, jachymko - http://www.codeplex.com/windowsformsaero
//  * Ben Ryves - http://www.benryves.com///
// ** Note for anyone considering using this: **//
// A better alternative to using this class is to use the MainMenu and ContextMenu
// controls instead of MenuStrip and ContextMenuStrip, as they provide true native
// rendering. If you require icons, try this:
// http://wyday.com/blog/2009/making-the-menus-in-your-net-app-look-professional/

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using AudioSwitcher.Presentation.UI.Interop;

namespace AudioSwitcher.Presentation.UI
{

    /// <summary>Renders a toolstrip using the UxTheme API via VisualStyleRenderer and a specific style.</summary>
    /// <remarks>Perhaps surprisingly, this does not need to be disposable.</remarks>
    internal class ToolStripNativeRenderer : ToolStripSystemRenderer
    {
        private const int RebarBackground = 6;
        private VisualStyleRenderer _renderer;

        public ToolStripNativeRenderer(ToolbarTheme theme)
        {
            Theme = theme;
        }

        public ToolbarTheme Theme
        {
            get;
            private set;
        }

        public bool IsSupported
        {
            get
            {
                if (!VisualStyleRenderer.IsSupported)
                    return false;

                // Needs a more robust check. It seems mono supports very different style sets.
                return VisualStyleRenderer.IsElementDefined(
                                VisualStyleElement.CreateElement("Menu",
                                                                 (int)MenuParts.BarBackground,
                                                                 (int)MenuBarStates.Active));
            }
        }

        private string RebarClass
        {
            get { return SubclassPrefix + "Rebar"; }
        }

        private string MenuClass
        {
            get { return SubclassPrefix + "Menu"; }

        }

        private string SubclassPrefix
        {
            get
            {

                switch (Theme)
                {

                    case ToolbarTheme.MediaToolbar: 
                        return "Media::";

                    case ToolbarTheme.CommunicationsToolbar: 
                        return "Communications::";

                    case ToolbarTheme.BrowserTabBar: 
                        return "BrowserTabBar::";

                    case ToolbarTheme.HelpBar: 
                        return "Help::";

                    default: return string.Empty;
                }
            }
        }

        public VisualStyleRenderer Renderer
        {
            get { return _renderer;}
        }

        protected bool EnsureRenderer()
        {
            if (!IsSupported)
                return false;

            if (_renderer == null)
                _renderer = new VisualStyleRenderer(VisualStyleElement.Button.PushButton.Normal);

            return true;
        }

        // Gives parented ToolStrips a transparent background.
        protected override void Initialize(ToolStrip toolStrip)
        {
            if (toolStrip.Parent is ToolStripPanel)
                toolStrip.BackColor = Color.Transparent;

            base.Initialize(toolStrip);
        }

        // Using just ToolStripManager.Renderer without setting the Renderer individually per ToolStrip means
        // that the ToolStrip is not passed to the Initialize method. ToolStripPanels, however, are. So we can 
        // simply initialize it here too, and this should guarantee that the ToolStrip is initialized at least 
        // once. Hopefully it isn't any more complicated than this.
        protected override void InitializePanel(ToolStripPanel toolStripPanel)
        {

            foreach (Control control in toolStripPanel.Controls)
                if (control is ToolStrip strip)
                    Initialize(strip);

            base.InitializePanel(toolStripPanel);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if (EnsureRenderer())
            {
                _renderer.SetParameters(MenuClass, (int)MenuParts.PopupBorders, 0);

                if (e.ToolStrip.IsDropDown)
                {
                    Region oldClip = e.Graphics.Clip;

                    // Tool strip borders are rendered *after* the content, for some reason.
                    // So we have to exclude the inside of the popup otherwise we'll draw over it.

                    Rectangle insideRect = e.ToolStrip.ClientRectangle;
                    insideRect.Inflate(-1, -1);
                    e.Graphics.ExcludeClip(insideRect);

                    _renderer.DrawBackground(e.Graphics, e.ToolStrip.ClientRectangle, e.AffectedBounds);

                    // Restore the old clip in case the Graphics is used again (does that ever happen?)
                    e.Graphics.Clip = oldClip;
                }

            }
            else
            {
                base.OnRenderToolStripBorder(e);
            }
        }

        protected virtual Rectangle GetBackgroundRectangle(ToolStripItem item)
        {
            if (!item.IsOnDropDown)
                return new Rectangle(new Point(), item.Bounds.Size);

            // For a drop-down menu item, the background rectangles of the items should be touching vertically.
            // This ensures that's the case.
            Rectangle rect = item.Bounds;

            // The background rectangle should be inset two pixels horizontally (on both sides), but we have 
            // to take into account the border.
            rect.X = item.ContentRectangle.X + 1;
            rect.Width = item.ContentRectangle.Width - 1;

            // Make sure we're using all of the vertical space, so that the edges touch.
            rect.Y = 0;

            return rect;
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (EnsureRenderer())
            {
                int partID = e.Item.IsOnDropDown ? (int)MenuParts.PopupItem : (int)MenuParts.BarItem;

                _renderer.SetParameters(MenuClass, partID, GetItemState(e.Item));

                Rectangle bgRect = GetBackgroundRectangle(e.Item);
                _renderer.DrawBackground(e.Graphics, bgRect, bgRect);
            }
            else
            {
                base.OnRenderMenuItemBackground(e);
            }
        }

        protected override void OnRenderToolStripPanelBackground(ToolStripPanelRenderEventArgs e)
        {
            if (EnsureRenderer())
            {
                // Draw the background using Rebar & RP_BACKGROUND (or, if that is not available, fall back to
                // Rebar.Band.Normal)
                if (VisualStyleRenderer.IsElementDefined(VisualStyleElement.CreateElement(RebarClass, RebarBackground, 0)))
                {
                    _renderer.SetParameters(RebarClass, RebarBackground, 0);
                }
                else
                {
                    _renderer.SetParameters(RebarClass, 0, 0);
                }

                if (_renderer.IsBackgroundPartiallyTransparent())
                    _renderer.DrawParentBackground(e.Graphics, e.ToolStripPanel.ClientRectangle, e.ToolStripPanel);

                _renderer.DrawBackground(e.Graphics, e.ToolStripPanel.ClientRectangle);

                e.Handled = true;

            }
            else
            {
                base.OnRenderToolStripPanelBackground(e);
            }
        }

        // Render the background of an actual menu bar, dropdown menu or toolbar.
        protected override void OnRenderToolStripBackground(System.Windows.Forms.ToolStripRenderEventArgs e)
        {
            if (EnsureRenderer())
            {
                if (e.ToolStrip.IsDropDown)
                {
                    _renderer.SetParameters(MenuClass, (int)MenuParts.PopupBackground, 0);

                }
                else
                {
                    // It's a MenuStrip or a ToolStrip. If it's contained inside a larger panel, it should have a
                    // transparent background, showing the panel's background.
                    if (e.ToolStrip.Parent is ToolStripPanel)
                    {
                        // The background should be transparent, because the ToolStripPanel's background will be visible.
                        // (Of course, we assume the ToolStripPanel is drawn using the same theme, but it's not my fault
                        // if someone does that.)
                        return;
                    }
                    else
                    {
                        // A lone toolbar/menubar should act like it's inside a toolbox, I guess.
                        // Maybe I should use the MenuClass in the case of a MenuStrip, although that would break
                        // the other themes...
                        if (VisualStyleRenderer.IsElementDefined(VisualStyleElement.CreateElement(RebarClass, RebarBackground, 0)))
                            _renderer.SetParameters(RebarClass, RebarBackground, 0);
                        else
                            _renderer.SetParameters(RebarClass, 0, 0);
                    }
                }

                if (_renderer.IsBackgroundPartiallyTransparent())
                    _renderer.DrawParentBackground(e.Graphics, e.ToolStrip.ClientRectangle, e.ToolStrip);

                _renderer.DrawBackground(e.Graphics, e.ToolStrip.ClientRectangle, e.AffectedBounds);

            }
            else
            {
                base.OnRenderToolStripBackground(e);
            }
        }

        // The only purpose of this override is to change the arrow colour.
        // It's OK to just draw over the default arrow since we also pass down arrow drawing to the system renderer.
        protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (EnsureRenderer())
            {
                var sb = (ToolStripSplitButton)e.Item;

                base.OnRenderSplitButtonBackground(e);

                // It doesn't matter what colour of arrow we tell it to draw. OnRenderArrow will compute it from the item anyway.
                OnRenderArrow(new ToolStripArrowRenderEventArgs(e.Graphics, sb, sb.DropDownButtonBounds, Color.Red, ArrowDirection.Down));

            }
            else
            {
                base.OnRenderSplitButtonBackground(e);
            }
        }

        private Color GetItemTextColor(ToolStripItem item)
        {
            int partId = item.IsOnDropDown ? (int)MenuParts.PopupItem : (int)MenuParts.BarItem;

            _renderer.SetParameters(MenuClass, partId, GetItemState(item));

            return _renderer.GetColor(ColorProperty.TextColor);
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            if (EnsureRenderer())
                e.TextColor = GetItemTextColor(e.Item);

            base.OnRenderItemText(e);
        }

        protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
        {
            Rectangle rectangle = e.ImageRectangle;
            if (e.Item.RightToLeft == RightToLeft.Yes)
            {
                rectangle.Offset(-1, 0);
            }
            else
            {
                rectangle.Offset(1, 0);
            }

            e = new ToolStripItemImageRenderEventArgs(e.Graphics, e.Item, e.Image, rectangle);

            base.OnRenderItemImage(e);
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            if (EnsureRenderer())
            {
                if (e.ToolStrip.IsDropDown)
                {
                    _renderer.SetParameters(MenuClass, (int)MenuParts.PopupGutter, 0);

                    // The AffectedBounds is usually too small, way too small to look right. Instead of using that,
                    // use the AffectedBounds but with the right width. Then narrow the rectangle to the correct edge
                    // based on whether or not it's RTL. (It doesn't need to be narrowed to an edge in LTR mode, but let's
                    // do that anyway.)
                    // Using the DisplayRectangle gets roughly the right size so that the separator is closer to the text.

                    Padding margins = GetThemeMargins(e.Graphics, MarginTypes.Sizing);
                    int extraWidth = (e.ToolStrip.Width - e.ToolStrip.DisplayRectangle.Width - margins.Left - margins.Right - 1) - e.AffectedBounds.Width;
                    Rectangle rect = e.AffectedBounds;
                    rect.Y += 2;
                    rect.Height -= 4;
                    int sepWidth = _renderer.GetPartSize(e.Graphics, ThemeSizeType.True).Width;

                    if (e.ToolStrip.RightToLeft == RightToLeft.Yes)
                    {
                        rect = new Rectangle(rect.X - extraWidth, rect.Y, sepWidth, rect.Height);
                        rect.X += sepWidth;
                    }
                    else
                    {
                        rect = new Rectangle(rect.Width + extraWidth - sepWidth, rect.Y, sepWidth, rect.Height);
                    }

                    _renderer.DrawBackground(e.Graphics, rect);
                }
            }
            else
            {
                base.OnRenderImageMargin(e);
            }
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            if (e.ToolStrip.IsDropDown && EnsureRenderer())
            {
                _renderer.SetParameters(MenuClass, (int)MenuParts.PopupSeparator, 0);

                var rect = new Rectangle(e.ToolStrip.DisplayRectangle.Left - 4, 0, e.ToolStrip.DisplayRectangle.Width + 4, e.Item.Height);

                _renderer.DrawBackground(e.Graphics, rect, rect);
            }
            else
            {
                base.OnRenderSeparator(e);
            }
        }

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            if (EnsureRenderer())
            {
                Rectangle backgroundRectangle = GetBackgroundRectangle(e.Item);
                //Rectangle bgRect = e.ImageRectangle;
                backgroundRectangle.Width = backgroundRectangle.Height;

                // Now, mirror its position if the menu item is RTL.
                if (e.Item.RightToLeft == RightToLeft.Yes)
                    backgroundRectangle = new Rectangle(e.ToolStrip.ClientSize.Width - backgroundRectangle.X - backgroundRectangle.Width, backgroundRectangle.Y, backgroundRectangle.Width, backgroundRectangle.Height);

                bool isRendereringImageCheck = IsRenderingImageCheck(e.Item);

                // WORKAROUND: On Windows 8.1 (not sure about anywhere else), the check background has the wrong 
                // border, see #29, instead we just use the same border as the hover state of a menu item
                //_renderer.SetParameters(MenuClass, (int)MenuParts.PopupCheckBackground, e.Item.Enabled ? (isRendereringImageCheck ? (int)MenuPopupCheckBackgroundStates.Bitmap : (int)MenuPopupCheckBackgroundStates.Normal) : (int)MenuPopupCheckBackgroundStates.Disabled);

                _renderer.SetParameters(MenuClass, (int)MenuParts.PopupItem, e.Item.Enabled ? (int)MenuPopupItemStates.Hover : (int)MenuPopupItemStates.DisabledHover);
                _renderer.DrawBackground(e.Graphics, backgroundRectangle);

                if (!isRendereringImageCheck)
                {
                    Rectangle checkRect = e.ImageRectangle;
                    checkRect.X = backgroundRectangle.X + backgroundRectangle.Width / 2 - checkRect.Width / 2;
                    checkRect.Y = backgroundRectangle.Y + backgroundRectangle.Height / 2 - checkRect.Height / 2;


                    if (e.Item is ToolStripMenuItem item && item.CheckState == CheckState.Indeterminate)
                    {
                        _renderer.SetParameters(MenuClass, (int)MenuParts.PopupCheck, e.Item.Enabled ? (int)MenuPopupCheckStates.BulletNormal : (int)MenuPopupCheckStates.BulletDisabled);
                    }
                    else
                    {
                        _renderer.SetParameters(MenuClass, (int)MenuParts.PopupCheck, e.Item.Enabled ? (int)MenuPopupCheckStates.CheckmarkNormal : (int)MenuPopupCheckStates.CheckmarkDisabled);
                    }

                    _renderer.DrawBackground(e.Graphics, checkRect);
                }
            }
            else
            {
                base.OnRenderItemCheck(e);
            }
        }

        private bool IsRenderingImageCheck(ToolStripItem item)
        {
            if (!(item is ToolStripMenuItem menuItem))
                return false;


            return (menuItem.Owner is ToolStripDropDownMenu menu && !menu.ShowCheckMargin && menuItem.Image != null);
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            // The default renderer will draw an arrow for us (the UXTheme API seems not to have one for all directions),
            // but it will get the colour wrong in many cases. The text colour is probably the best colour to use.
            if (EnsureRenderer())
                e.ArrowColor = GetItemTextColor(e.Item);

            base.OnRenderArrow(e);
        }

        protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (EnsureRenderer())
            {
                // BrowserTabBar::Rebar draws the chevron using the default background. Odd.
                string rebarClass = RebarClass;
                if (Theme == ToolbarTheme.BrowserTabBar)
                    rebarClass = "Rebar";

                int state = VisualStyleElement.Rebar.Chevron.Normal.State;

                if (e.Item.Pressed)
                    state = VisualStyleElement.Rebar.Chevron.Pressed.State;
                else if (e.Item.Selected)
                    state = VisualStyleElement.Rebar.Chevron.Hot.State;

                _renderer.SetParameters(rebarClass, VisualStyleElement.Rebar.Chevron.Normal.Part, state);
                _renderer.DrawBackground(e.Graphics, new Rectangle(Point.Empty, e.Item.Size));
            }
            else
            {
                base.OnRenderOverflowButtonBackground(e);
            }
        }

        private Padding GetThemeMargins(IDeviceContext dc, MarginTypes marginType)
        {
            
            try
            {
                IntPtr hDC = dc.GetHdc();

                if (DllImports.GetThemeMargins(_renderer.Handle, hDC, _renderer.Part, _renderer.State, (int)marginType, IntPtr.Zero, out MARGINS margins) == 0)
                    return new Padding(margins.cxLeftWidth, margins.cyTopHeight, margins.cxRightWidth, margins.cyBottomHeight);

                return new Padding(0);
            }
            finally
            {
                dc.ReleaseHdc();
            }
        }

        private static int GetItemState(ToolStripItem item)
        {
            bool hot = item.Selected;

            if (item.IsOnDropDown)
            {
                if (item.Enabled)
                    return hot ? (int)MenuPopupItemStates.Hover : (int)MenuPopupItemStates.Normal;

                return hot ? (int)MenuPopupItemStates.DisabledHover : (int)MenuPopupItemStates.Disabled;
            }
            else
            {
                if (item.Pressed)
                    return item.Enabled ? (int)MenuBarItemStates.Pushed : (int)MenuBarItemStates.DisabledPushed;

                if (item.Enabled)
                    return hot ? (int)MenuBarItemStates.Hover : (int)MenuBarItemStates.Normal;

                return hot ? (int)MenuBarItemStates.DisabledHover : (int)MenuBarItemStates.Disabled;
            }
        }
    }
}

