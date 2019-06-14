using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.QuickStart.WinControls;
using Telerik.WinControls.UI;
using System.Globalization;

namespace Telerik.Examples.WinControls.DataEntry.Customization
{
    public partial class Form1 : ExamplesForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.radDataEntry1.ShowValidationPanel = true;
            this.radDataEntry1.ItemDefaultSize = new Size(460, 25);
            this.radDataEntry1.ItemSpace = 10;

            this.radDataEntry1.EditorInitializing += new Telerik.WinControls.UI.EditorInitializingEventHandler(radDataEntry1_EditorInitializing);
            this.radDataEntry1.ItemInitialized += new Telerik.WinControls.UI.ItemInitializedEventHandler(radDataEntry1_ItemInitialized);
            this.radDataEntry1.BindingCreated += new BindingCreatedEventHandler(radDataEntry1_BindingCreated);


            this.radDataEntry1.DataSource = new Person(DateTime.Now, "Iva", "Ivanova", Person.OccupationPositions.SuppliesManager, "(555) 123 456", 1500);
        }

        void radDataEntry1_BindingCreated(object sender, BindingCreatedEventArgs e)
        {
            if (e.DataMember == "Salary") 
            {
                e.Binding.Parse += new ConvertEventHandler(Binding_Parse);       
            }
        }

        void radDataEntry1_ItemInitialized(object sender, Telerik.WinControls.UI.ItemInitializedEventArgs e)
        {
            if (e.Panel.Controls[1].Text == "First Name") 
            {
                e.Panel.Size = new Size(300, 25);
                e.Panel.Controls[1].Text = "Name";
            }
            else if (e.Panel.Controls[1].Text == "Last Name")
            {
                e.Panel.Size = new Size(160, 25);
                e.Panel.Controls[1].Visible = false;
                e.Panel.Location = new Point(310, 10);
            }
            else 
            {
                e.Panel.Location = new Point(e.Panel.Location.X, e.Panel.Location.Y - 35);
            }

            if (e.Panel.Controls[0] is RadDateTimePicker) 
            {
                e.Panel.Controls[0].ForeColor = Color.MediumVioletRed;
            }

            if (e.Panel.Controls[1].Text == "Note")
            {
                e.Panel.Size = new Size(e.Panel.Size.Width, 100);
                (e.Panel.Controls[0] as RadTextBox).Multiline = true;
            }

            e.Panel.Controls[1].Font = new System.Drawing.Font(e.Panel.Controls[1].Font.Name, 12.0F, FontStyle.Bold);
            e.Panel.Controls[1].ForeColor = Color.Red;
        }

        void radDataEntry1_EditorInitializing(object sender, Telerik.WinControls.UI.EditorInitializingEventArgs e)
        {
            if (e.Property.Name == "Salary") 
            {
                RadMaskedEditBox radMaskedEditBox = new RadMaskedEditBox();
                radMaskedEditBox.MaskType = MaskType.Numeric;
                radMaskedEditBox.MaskedEditBoxElement.StretchVertically = true;

                e.Editor = radMaskedEditBox;
            }

            if (e.Property.Name == "PassWord") 
            {
                (e.Editor as RadTextBox).PasswordChar = '*';
            }
        }

        void Binding_Parse(object sender, ConvertEventArgs e)
        {
            int salary = int.Parse(e.Value.ToString(), NumberStyles.Currency);
            e.Value = salary;
        }
    }
}
