using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.QuickStart.WinControls;
using Telerik.WinControls.UI;
using Telerik.Examples.WinControls.Editors.ComboBox;

namespace Telerik.Examples.WinControls.Buttons.CheckBoxes
{
    public partial class Form1 : EditorExampleBaseForm
    {
        public Form1()
        {
            InitializeComponent();       

            this.radCheckBox1.Font = new Font(new FontFamily("Arial"), 10.0f, GraphicsUnit.Point);
            this.radCheckBox2.Font = new Font(new FontFamily("Arial"), 12.0f, GraphicsUnit.Point);
            this.radCheckBox3.Font = new Font(new FontFamily("Arial"), 14.0f, GraphicsUnit.Point);
        }


        protected override void WireEvents()
        {
            this.radCheckBox3.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.radCheckBox1_ToggleStateChanged);
            this.radCheckBox2.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.radCheckBox1_ToggleStateChanged);
            this.radCheckBox1.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.radCheckBox1_ToggleStateChanged);
        }

		private void radCheckBox1_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
		{
			this.radTextBoxEvents.Text += string.Format("{0} toggled" + Environment.NewLine, (sender as RadCheckBox).Text);
            this.radTextBoxEvents.SelectionStart = this.radTextBoxEvents.Text.Length;
            this.radTextBoxEvents.ScrollToCaret();
		}

        
    }
}