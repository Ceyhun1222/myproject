using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.QuickStart.WinControls;
using Telerik.WinControls.UI;

namespace Telerik.Examples.WinControls.GridView.Columns.ColumnChooser
{
    public partial class Form1 : ExamplesForm
    {
        public Form1()
        {
            InitializeComponent();

			this.SelectedControl = this.radGridView1;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.radGridView1.TableElement.BeginUpdate();

            this.BindGrid();
            this.radGridView1.Columns[0].IsVisible = false;

            this.radGridView1.EnableHotTracking = true;
            this.radGridView1.TableElement.EndUpdate(true);
            this.radGridView1.ShowColumnChooser();
        }

        private void BindGrid()
        {
            this.customersTableAdapter.Fill(this.nwindRadGridView.Customers);
        }

		void btnOpenCondFormatting_Click(object sender, System.EventArgs e)
		{
            radGridView1.ShowColumnChooser();
        }

        protected override void WireEvents()
        {
            this.radBtnLaunchColChooser.Click += new System.EventHandler(this.btnOpenCondFormatting_Click);
        }
    }
}