using System;
using System.Collections;
using System.Collections.Generic;
using NetSettings.Common.Classes;

namespace NetSettings.Common.Interfaces
{
    public interface IControl : IGuiElement
    {
        //    public void SetFont(IFont);
        //VisualItem Tag { get; set; }
        bool Visible { get; set; }//TODO: Can we remove the set?
        string Text { get; set; }//TODO: Can we remove the set?
        //IList<IControl> Controls { get; set; }
        //IList Controls { get; }//TODO: Rename to VisualControls
        //IList<IControl> VisualControl { get; set; }
        IList VisualControl { get; }
        IControl AddVisualControl(IControl control); //TODO: Can we remove this in favour of VisualControl?
        IList<IControl> LogicalControls { get; set; }//TODO: Can we remove the set?
        int Width { get; set; }//TODO: Can we remove the set?
        int Height { get; set; }//TODO: Can we remove the set?
        DockStyle Dock { get; set; }
        Color BackColor { get; set; }
        Color ForeColor { set; }//TODO: Do we need to add the get although it is not in use?
        Point Location { get; set; }//TODO: Can we remove the set?
        IFont Font { get; set; }//TODO: Can we remove the set?

        object Instance { get; }

        #region Events
        event EventHandler DoubleClick;
        event EventHandler MouseClick;
        event EventHandler TextChanged;
        event EventHandler Leave;
        event EventHandler MouseDoubleClick;
        event EventHandler KeyDown;
        event EventHandler Click;
        event EventHandler MouseEnter;
        event EventHandler MouseLeave;
        #endregion Events
    }
}