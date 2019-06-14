using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.QuickStart.WinControls;
using System.IO;
using Telerik.WinControls;

namespace Telerik.Examples.WinControls.DataEntry.FirstLook
{
    public partial class Form1 : ExamplesForm
    {
        public Form1()
        {
            InitializeComponent();
            SetupControls();
        }

        private void SetupControls()
        {
            this.SuspendLayout();

            this.radDataEntry1.ItemDefaultSize = new Size(300, 22);
            this.radDataEntry1.ColumnCount = 2;
            this.radDataEntry1.FlowDirection =  FlowDirection.TopDown;
            this.radDataEntry1.FitToParentWidth = true;
            this.radDataEntry1.ItemSpace = 8;

            this.radDataEntry1.ItemInitializing += new Telerik.WinControls.UI.ItemInitializingEventHandler(radDataEntry1_ItemInitializing);
            this.radDataEntry1.EditorInitializing += new Telerik.WinControls.UI.EditorInitializingEventHandler(radDataEntry1_EditorInitializing);
            this.radDataEntry1.BindingCreating += new Telerik.WinControls.UI.BindingCreatingEventHandler(radDataEntry1_BindingCreating);
            this.radDataEntry1.BindingCreated += new Telerik.WinControls.UI.BindingCreatedEventHandler(radDataEntry1_BindingCreated);

            this.productsTableAdapter.Fill(this.furnitureDataSet.Products);
            this.bindingSource1.AllowNew = true;

            this.radBindingNavigator1.BindingSource = this.bindingSource1;
            this.radBindingNavigator1.AutoHandleAddNew = false;
            this.radBindingNavigator1AddNewItem.Click += new EventHandler(radBindingNavigator1AddNewItem_Click);

            this.radDataEntry1.DataSource = this.bindingSource1;

            this.ResumeLayout();
        }

        void radBindingNavigator1AddNewItem_Click(object sender, EventArgs e)
        {
            Telerik.Examples.WinControls.DataSources.FurnitureDataSet.ProductsRow row = this.furnitureDataSet.Products.NewProductsRow();
            row.Price = 0;
            row.Photo = Telerik.WinControls.ImageHelper.GetBytesFromImage(Properties.Resources.insert5);
            row.Lining = "";
            row.Manufacturer = "";
            row.ProductName = "";
            row.Quantity = 0;
            row.SalesRepresentative = "";
            row.Front = "";
            row.Dimensions = "";
            row.Condition = false;
            
            this.furnitureDataSet.Products.Rows.Add(row);
           
            this.furnitureDataSet.AcceptChanges();

            productsTableAdapter.Update(this.furnitureDataSet.Products);

            this.bindingSource1.Position = this.bindingSource1.Count - 1;
        }

        void radDataEntry1_BindingCreated(object sender, Telerik.WinControls.UI.BindingCreatedEventArgs e)
        {
            if (e.DataMember == "Photo")
            {
                e.Binding.Format += new ConvertEventHandler(Binding_Format);
            }
        }

        void radDataEntry1_BindingCreating(object sender, Telerik.WinControls.UI.BindingCreatingEventArgs e)
        {
            if (e.DataMember == "Photo")
            {
                e.PropertyName = "Image";
            }
        }

        void Binding_Format(object sender, ConvertEventArgs e)
        {
            Image img = Telerik.WinControls.ImageHelper.GetImageFromBytes((byte[])e.Value);
            e.Value = img;
        }

        void radDataEntry1_EditorInitializing(object sender, Telerik.WinControls.UI.EditorInitializingEventArgs e)
        {
            if (e.Property.Name == "Photo")
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.Dock = DockStyle.Fill;
                pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                e.Editor = pictureBox;
            }
        }

        void radDataEntry1_ItemInitializing(object sender, Telerik.WinControls.UI.ItemInitializingEventArgs e)
        {
            if (e.Panel.Controls[1].Text == "Photo")
            {
                e.Panel.Location = new Point(8, 200);
                e.Panel.Size = new Size(500, 220);
            }
        }
    }
}
