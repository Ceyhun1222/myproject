using System;
using System.Drawing;
using System.Windows.Forms;
using Telerik.QuickStart.WinControls;
using Telerik.WinControls.UI;

namespace Telerik.Examples.WinControls.CommandBar.TextPad
{
	public partial class Form1: ExamplesForm
	{
		public Form1()
		{
			InitializeComponent();
		}

        private void alignment_ToggleStateChanging(object sender, Telerik.WinControls.UI.StateChangingEventArgs args)
        {
            CommandBarToggleButton senderButton = (sender as CommandBarToggleButton);

            if (args.NewValue == Telerik.WinControls.Enumerations.ToggleState.Off)
            {
                return;
            }

            if (senderButton != radCommandBarToggleButtonItem4)
            {
                radCommandBarToggleButtonItem4.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off;
            }

            if (senderButton != radCommandBarToggleButtonItem5)
            {
                radCommandBarToggleButtonItem5.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off;
            }

            if (senderButton != radCommandBarToggleButtonItem6)
            {
                radCommandBarToggleButtonItem6.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off;
            }

            if (senderButton != radCommandBarToggleButtonItem7)
            {
                radCommandBarToggleButtonItem7.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off;
            }
        }

        protected override void WireEvents()
        {
            this.radCommandBarToggleButtonItem4.ToggleStateChanging += new Telerik.WinControls.UI.StateChangingEventHandler(this.alignment_ToggleStateChanging);
            this.radCommandBarToggleButtonItem5.ToggleStateChanging += new Telerik.WinControls.UI.StateChangingEventHandler(this.alignment_ToggleStateChanging);
            this.radCommandBarToggleButtonItem6.ToggleStateChanging += new Telerik.WinControls.UI.StateChangingEventHandler(this.alignment_ToggleStateChanging);
            this.radCommandBarToggleButtonItem7.ToggleStateChanging += new Telerik.WinControls.UI.StateChangingEventHandler(this.alignment_ToggleStateChanging);
        }
	}
}
