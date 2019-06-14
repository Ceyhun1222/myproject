using System;
using System.Collections.Generic;
using System.Text;
using Telerik.WinControls.UI;
using System.Drawing;

namespace Telerik.Examples.WinControls.TreeView
{
    public class TreeExampleHeaderPanel: RadPanel
    {
        protected override void OnLoad(Size desiredSize)
        {
            base.OnLoad(desiredSize);

            //this.PanelElement.PanelFill.BackColor = Color.FromArgb(220, 238, 253);
            //this.PanelElement.PanelFill.BackColor2 = Color.FromArgb(170, 207, 253);
            //this.PanelElement.PanelFill.NumberOfColors = 2;
            //this.PanelElement.PanelFill.GradientStyle = Telerik.WinControls.GradientStyles.Linear;
            //this.PanelElement.PanelBorder.BoxStyle = Telerik.WinControls.BorderBoxStyle.FourBorders;
            //this.PanelElement.PanelBorder.TopColor = Color.FromArgb(61, 107, 192);
            //this.PanelElement.PanelBorder.LeftColor = Color.FromArgb(61, 107, 192);
            //this.PanelElement.PanelBorder.RightColor = Color.FromArgb(61, 107, 192);
            //this.PanelElement.PanelBorder.BottomColor = Color.FromArgb(187, 217, 253);
            //this.PanelElement.PanelBorder.TopShadowColor = Color.FromArgb(228,242,253);
            //this.PanelElement.PanelBorder.LeftShadowColor = Color.FromArgb(211,232,253);
            //this.PanelElement.PanelBorder.RightShadowColor = Color.FromArgb(211,232,253);
            //this.PanelElement.PanelBorder.LeftWidth = 1;
            //this.PanelElement.PanelBorder.TopWidth = 1;
            //this.PanelElement.PanelBorder.RightWidth = 1;
            //this.PanelElement.PanelBorder.BottomWidth = 1;
            this.PanelElement.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.PanelElement.Padding = new System.Windows.Forms.Padding(8, 4, 2, 2);
            //this.PanelElement.PanelBorder.BorderDrawMode = Telerik.WinControls.BorderDrawModes.VerticalOverHorizontal;
        }

        public override string ThemeClassName
        {
            get
            {
                return typeof(RadPanel).FullName;
            }
        }
    }
}
