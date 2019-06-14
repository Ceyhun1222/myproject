using System;
using Telerik.Examples.WinControls.Editors.ComboBox;

namespace Telerik.Examples.WinControls.Buttons.RepeatButton
{
	/// <summary>
	/// Main class for the repeat button example
	/// </summary>
    public partial class Form1 : EditorExampleBaseForm
    {
        public Form1()
        {
            InitializeComponent();

            this.radProgressBar1.Text = "";
        }

		private void radRepeatButton3_ButtonClick(object sender, EventArgs e)
		{
            if (this.radProgressBar1.Value1 < this.radProgressBar1.Maximum)
			{
                this.radProgressBar1.Value1 += 1;
			}
			else
			{
                this.radProgressBar1.Value1 = this.radProgressBar1.Minimum;
			}
		}

        protected override void WireEvents()
        {
            this.radRepeatButton3.ButtonClick += new System.EventHandler(this.radRepeatButton3_ButtonClick);
        }
    }
}