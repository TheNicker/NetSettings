using System;
using System.Collections;
using System.Collections.Generic;

namespace NetSettingsCore.Common
{
    public interface IControl : IGuiElement
    {
        //    public void SetFont(IFont);
        //VisualItem Tag { get; set; }
        bool Visible { get; set; }
        string Text { get; set; }
        //IList<IControl> Controls { get; set; }
        IList Controls { get; }//TODO: Rename to VisualControls
        IList<IControl> LogicalControls { get; }
        int Width { get; set; }
        int Height { get; set; }
        IColor BackColor { get; set; }
        IPoint Location { get; set; }
        IFont Font { get; set; }

        #region Events
        event EventHandler DoubleClick;
        event EventHandler MouseClick;
        event EventHandler TextChanged;
        event EventHandler Leave;
        event EventHandler SelectedIndexChanged;
        event EventHandler MouseDoubleClick;
        event EventHandler KeyDown;
        event EventHandler Click;
        event EventHandler MouseEnter;
        event EventHandler MouseLeave;
        #endregion Events
    }
}