using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Telerik.Examples.WinControls.RibbonBar.ResizingDemo
{
    class MyFlowLayoutPanel : FlowLayoutPanel
    {
        public MyFlowLayoutPanel()
        { 
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
        }
    }
}
