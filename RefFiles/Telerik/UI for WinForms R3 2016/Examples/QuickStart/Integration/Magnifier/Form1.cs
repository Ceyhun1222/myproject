using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Telerik.QuickStart.WinControls;
using Telerik.WinControls;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;

namespace Telerik.Examples.WinControls.Integration.Magnifier
{
	public partial class Form1 : ExamplesForm
	{
		public Form1()
		{
			InitializeComponent();

            FillMagnifierWithButtons();

            this.radPanel1.Text = "";
            radToggleButton1.ToggleState = ToggleState.On;
		}

        protected override string GetExampleDefaultTheme()
        {
            return "OfficeGlass";
        }

        private void FillMagnifierWithButtons()
        {
            for (int i = 0; i < 49; i++)
            {
                RadButtonElement button = new RadButtonElement(i.ToString());

                button.StretchHorizontally = false;
                button.StretchVertically = false;
                button.Alignment = ContentAlignment.MiddleCenter;
                button.TextAlignment = ContentAlignment.MiddleCenter;
                button.TextElement.Parent.Alignment = ContentAlignment.MiddleCenter;
                button.MinSize = new Size(33, 33);
                this.magnifier1.Items.Add(button);
            }
        }

		private void radToggleButton1_ToggleStateChanged(object sender, StateChangedEventArgs args)
		{
			Random rand = new Random();

			foreach (RadItem item in this.magnifier1.Items)
			{
				if (args.ToggleState == ToggleState.On)
				{
					item.Opacity = (double)(rand.Next(100) + 10) / 100d;
				}
				else
				{
					item.Opacity = 1;
				}
			}
		}

        private void radSpinEditor1_ValueChanged(object sender, EventArgs e)
        {
            this.magnifier1.ZoomFactor = (float)this.radSpinEditor1.Value;
        }

        protected override void WireEvents()
        {
            this.radToggleButton1.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.radToggleButton1_ToggleStateChanged);
            this.radSpinEditor1.ValueChanged += new System.EventHandler(this.radSpinEditor1_ValueChanged);
        }
	}
}
