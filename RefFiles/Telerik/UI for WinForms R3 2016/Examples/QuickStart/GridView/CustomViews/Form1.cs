using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Telerik.QuickStart.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.Layouts;
using Telerik.WinControls;

namespace Telerik.Examples.WinControls.GridView.CustomViews
{
    public partial class Form1 : ExamplesForm
    {
        #region Fields

        private ColumnChooserGridViewElement columnChooserView;
        private GridViewDetailElement detailView;

        #endregion

        #region Constructor

        public Form1()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.InitializeGridControl();

            this.gridView.ThemeNameChanged += new Telerik.WinControls.ThemeNameChangedEventHandler(gridView_ThemeNameChanged);
        }

        private void gridView_ThemeNameChanged(object source, ThemeNameChangedEventArgs args)
        {
            this.detailView.DrawFill = true;
            this.detailView.BackColor = Color.FromArgb(240, 240, 240);
        }

        #endregion

        #region Methods

        private void InitializeGridControl()
        {
            this.columnChooserView = new ColumnChooserGridViewElement();
            this.gridView.GridViewElement.Panel.Children.Insert(0, columnChooserView);
            columnChooserView.SetValue(DockLayoutPanel.DockProperty, Telerik.WinControls.Layouts.Dock.Right);

            this.gridView.TableElement.SetValue(DockLayoutPanel.DockProperty, Telerik.WinControls.Layouts.Dock.Top);
            //this.gridView.TableElement.Margin = new Padding(10, 0, 10, 10);
            
            this.detailView = new GridViewDetailElement();
            this.gridView.GridViewElement.Panel.Children.Insert(2, this.detailView);
            this.detailView.SetValue(DockLayoutPanel.DockProperty, Telerik.WinControls.Layouts.Dock.Bottom);
            //this.detailView.Margin = new Padding(10, 0, 10, 2);

            this.employeesTableAdapter.Fill(this.northwindDataSet.Employees);

            this.gridView.Columns["LastName"].SortOrder = Telerik.WinControls.UI.RadSortOrder.Descending;
            this.gridView.Columns["City"].SortOrder = Telerik.WinControls.UI.RadSortOrder.Descending;

            foreach (GridViewColumn col in this.gridView.Columns)
            {
                if (ShouldBeVisibleColumn(col.Name))
                {
                    col.Width = 100;
                    continue;
                }

                col.IsVisible = false;

                if (col.Name == "Photo")
                {
                    this.gridView.Columns["Photo"].VisibleInColumnChooser = false;
                }
            }

            this.gridView.Rows[2].IsCurrent = true;
            this.gridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
        }

        private bool ShouldBeVisibleColumn(string column)
        {
            if (column == "FirstName" ||
                column == "LastName" ||
                column == "HireDate" ||
                column == "HomePhone" ||
                column == "PostalCode")
            {
                return true;
            }

            return false;
        }

        #endregion

        protected override void WireEvents()
        {
        }
    }
}
