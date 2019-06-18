using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.QuickStart.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls;

namespace Telerik.Examples.WinControls.GridView.Sorting.Sorting
{
    public partial class Form1 : ExamplesForm
    {
        private ListSortDirection sortOrder = ListSortDirection.Ascending;

        public Form1()
        {
            InitializeComponent();
        }

        private void BindGrid()
        {
            this.radGridView1.TableElement.BeginUpdate();

            this.SelectedControl = this.radGridView1;
            this.radGridView1.EnableHotTracking = true;

            // Populate and bind the datasource.
            this.customersTableAdapter.Fill(this.nwindRadGridView.Customers);
            this.radGridView1.DataSource = customersBindingSource;

            // Hide some columns, reduce clutter since columns are autogenerated.
            this.radGridView1.Columns["CustomerID"].IsVisible = false;
            this.radGridView1.Columns["Region"].IsVisible = false;
            this.radGridView1.Columns["Fax"].IsVisible = false;
            this.radGridView1.Columns["CompanyName"].IsVisible = false;
            this.radGridView1.Columns["Phone"].IsVisible = false;
            this.radGridView1.Columns["Address"].IsVisible = false;

            // Change header text to look normal.
            this.radGridView1.Columns["PostalCode"].HeaderText = "Postal Code";
            this.radGridView1.Columns["ContactName"].HeaderText = "Contact Name";
            this.radGridView1.Columns["ContactTitle"].HeaderText = "Contact Title";

            this.radGridView1.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            this.radGridView1.TableElement.EndUpdate(true);

            // Add grid sort expression.
            this.radGridView1.MasterTemplate.SortDescriptors.Add("ContactName", sortOrder);
        }

        private void InitializeSorting()
        {
            this.radRadioButtonContactName.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            this.radRadioButtonAsc.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            BindGrid();
            this.radLabelCol.ForeColor = Color.White;
            InitializeSorting();
        }

        private void radRadioButtonAsc_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (this.radRadioButtonAsc.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
            {
                this.sortOrder = ListSortDirection.Ascending;
            }
            if (this.radRadioButtonDesc.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
            {
                this.sortOrder = ListSortDirection.Descending;
            }

            radGridView1.MasterTemplate.SortDescriptors.Clear();

            if (this.radRadioButtonNone.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
            {
                return;
            }

            if (this.radRadioButtonContactName.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
            {
                this.radLabelCol.Text = "Contact Name";
                this.radGridView1.MasterTemplate.SortDescriptors.Add("ContactName", sortOrder);
            }

            if (this.radRadioButtonContactTitle.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
            {
                this.radLabelCol.Text = "Contact Title";
                this.radGridView1.MasterTemplate.SortDescriptors.Add("ContactTitle", sortOrder);
            }

            if (this.radRadioButtonCity.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
            {
                this.radLabelCol.Text = "City";
                this.radGridView1.MasterTemplate.SortDescriptors.Add("City", sortOrder);
            }

            if (this.radRadioButtonPostalCode.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
            {
                this.radLabelCol.Text = "Postal Code";
                this.radGridView1.MasterTemplate.SortDescriptors.Add("PostalCode", sortOrder);
            }

            if (this.radRadioButtonCountry.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
            {
                this.radLabelCol.Text = "Country";
                this.radGridView1.MasterTemplate.SortDescriptors.Add("Country", sortOrder);
            }

            if (this.radRadioButtonContactTitleCity.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
            {
                this.radLabelCol.Text = "Contact Title and City";
                this.radGridView1.MasterTemplate.SortDescriptors.Add("ContactTitle", sortOrder);
                this.radGridView1.MasterTemplate.SortDescriptors.Add("City", sortOrder);
            }

            if (this.radRadioButtonTitleCountryCity.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
            {
                this.radLabelCol.Text = "Contact Title, Country and City";
                this.radGridView1.MasterTemplate.SortDescriptors.Add("ContactTitle", sortOrder);
                this.radGridView1.MasterTemplate.SortDescriptors.Add("Country", sortOrder);
                this.radGridView1.MasterTemplate.SortDescriptors.Add("City", sortOrder);
            }
        }

        protected override void WireEvents()
        {
            this.radRadioButtonTitleCountryCity.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.radRadioButtonAsc_ToggleStateChanged);
            this.radRadioButtonContactTitleCity.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.radRadioButtonAsc_ToggleStateChanged);
            this.radRadioButtonCountry.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.radRadioButtonAsc_ToggleStateChanged);
            this.radRadioButtonCity.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.radRadioButtonAsc_ToggleStateChanged);
            this.radRadioButtonContactTitle.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.radRadioButtonAsc_ToggleStateChanged);
            this.radRadioButtonContactName.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.radRadioButtonAsc_ToggleStateChanged);
            this.radRadioButtonPostalCode.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.radRadioButtonAsc_ToggleStateChanged);
            this.radRadioButtonDesc.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.radRadioButtonAsc_ToggleStateChanged);
            this.radRadioButtonAsc.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.radRadioButtonAsc_ToggleStateChanged);

        }
    }
}