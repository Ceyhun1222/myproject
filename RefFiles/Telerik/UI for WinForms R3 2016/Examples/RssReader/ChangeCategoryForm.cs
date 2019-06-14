using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RssReader
{
    public partial class ChangeCategoryForm : Telerik.WinControls.UI.RadForm
    {
        public ChangeCategoryForm()
        {
            InitializeComponent();
            radDropDownList1.DropDownStyle = RadDropDownStyle.DropDownList;
        }
        public RadCheckBox CheckBox
        {
            get
            {
                return this.radCheckBox1;
            }
        }

        public RadDropDownList RadDropDownList
        {
            get
            {
                return this.radDropDownList1;
            }
        }

        public RadTextBox NewCategoryNameTextBox
        {
            get
            {
                return this.radTextBox1;
            }
        }

        public RadTextBox NameTextBox
        {
            get
            {
                return this.radTextBox2;
            }
        }

        

        private void radCheckBox1_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (args.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
            {
                radTextBox1.Enabled = true;
                radDropDownList1.Enabled = false;
            }
            else
            {
                radTextBox1.Enabled = false;
                radDropDownList1.Enabled = true;
            }
        }

        public string ErrorText { get; set; }

        private void radButton1_Click(object sender, EventArgs e)
        {
            if (radCheckBox1.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On && string.IsNullOrEmpty(radTextBox1.Text))
            {
                errorProvider1.SetIconAlignment(radTextBox1, ErrorIconAlignment.MiddleLeft);
                errorProvider1.SetError(this.radTextBox1, ErrorText);
                return;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.NameTextBox.TextBoxElement.TextBoxItem.HostedControl.Focus();
        }

       
    }
}
