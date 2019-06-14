using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.QuickStart.WinControls;
using Telerik.WinControls.UI;

namespace Telerik.Examples.WinControls.DataEntry.DataLayout
{
    public partial class Form1 : ExamplesForm
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetupControls();
            ArrangePictureBox();
        }

        private void SetupControls()
        {
            this.radDataLayout1.ItemDefaultHeight =  26;
            this.radDataLayout1.ColumnCount = 2;
            this.radDataLayout1.FlowDirection = FlowDirection.TopDown;
            this.radDataLayout1.AutoSizeLabels = true;

            this.radDataLayout1.EditorInitializing += new Telerik.WinControls.UI.EditorInitializingEventHandler(radDataEntry1_EditorInitializing);
            this.radDataLayout1.BindingCreating += new Telerik.WinControls.UI.BindingCreatingEventHandler(radDataEntry1_BindingCreating);
            this.radDataLayout1.BindingCreated += new Telerik.WinControls.UI.BindingCreatedEventHandler(radDataEntry1_BindingCreated);

            this.productsTableAdapter.Fill(this.furnitureDataSet.Products);
            this.bindingSource1.AllowNew = true;

            this.radBindingNavigator1.BindingSource = this.bindingSource1;
            this.radBindingNavigator1.AutoHandleAddNew = false;
            this.radBindingNavigator1AddNewItem.Click += new EventHandler(radBindingNavigator1AddNewItem_Click);

            this.radDataLayout1.DataSource = this.bindingSource1;
        }

        private void ArrangePictureBox()
        {
            RadLayoutControl layoutControl = this.radDataLayout1.LayoutControl;

            layoutControl.AddItem((LayoutControlItemBase)layoutControl.Items[5], 
                (LayoutControlItemBase)layoutControl.Items[11], LayoutControlDropPosition.Top);

            layoutControl.ResizeItem((LayoutControlItemBase)layoutControl.Items[5], 22 - layoutControl.Items[5].Bounds.Height);
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
                pictureBox.Name = "PictureBoxPhoto";
                pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                e.Editor = pictureBox;
            }
        }

        private void radButtonCustomize_Click(object sender, EventArgs e)
        {
            this.radDataLayout1.LayoutControl.ShowCustomizeDialog();
        }

        private void radButtonSaveLayout_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.DefaultExt = ".xml";
                sfd.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.radDataLayout1.LayoutControl.SaveLayout(sfd.FileName);
                }
            }
        }

        private void radButtonLoadLayout_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.DefaultExt = ".xml";
                ofd.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.radDataLayout1.LayoutControl.LoadLayout(ofd.FileName);
                }
            }
        }
    }
}
