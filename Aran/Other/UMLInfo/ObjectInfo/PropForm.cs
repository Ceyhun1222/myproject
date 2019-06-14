using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UMLInfo
{
    public partial class PropForm : Form
    {
        private List<ObjectInfo> _objInfoList;
        private ObjectInfo _objInfo;
        private Form _owner;

        public PropForm (List<ObjectInfo> objInfoList, Form owner)
        {
            InitializeComponent ();

            _objInfoList = objInfoList;
            _owner = owner;
        }

        public void ShowForm (ObjectInfo objInfo)
        {
            Text = objInfo.Name + " Properties";
            _objInfo = objInfo;

            FillPropDGV ();

            Show (_owner);
        }

        public void FillPropDGV ()
        {
            propDGV.Columns.Clear ();
            propDGV.Rows.Clear ();

            DataGridViewColumn col = new DataGridViewTextBoxColumn ();
            col.HeaderText = "Id";
            col.Name = "Id";
            col.Visible = false;
            propDGV.Columns.Add (col);

            col = new DataGridViewTextBoxColumn ();
            col.HeaderText = "Name";
            col.Name = "Name";
            col.Width = 150;
            propDGV.Columns.Add (col);

            col = new DataGridViewTextBoxColumn ();
            col.HeaderText = "PropType";
            col.Name = "PropType";
            col.Width = 150;
            propDGV.Columns.Add (col);

            col = new DataGridViewCheckBoxColumn ();
            col.SortMode = DataGridViewColumnSortMode.Automatic;
            col.HeaderText = "Is Association";
            col.Name = "IsAssociation";
            col.Width = 60;
            propDGV.Columns.Add (col);

            col = new DataGridViewTextBoxColumn ();
            col.HeaderText = "Containment";
            col.Name = "Containment";
            col.Width = 50;
            propDGV.Columns.Add (col);

            col = new DataGridViewTextBoxColumn ();
            col.HeaderText = "Cardinality";
            col.Name = "Cardinality";
            col.Width = 50;
            propDGV.Columns.Add (col);

            col = new DataGridViewTextBoxColumn ();
            col.HeaderText = "Other Cardinality";
            col.Name = "OtherCardinality";
            col.Width = 50;
            propDGV.Columns.Add (col);

            col = new DataGridViewCheckBoxColumn ();
            col.SortMode = DataGridViewColumnSortMode.Automatic;
            col.HeaderText = "Nullable";
            col.Name = "Nullable";
            col.Width = 60;
            propDGV.Columns.Add (col);

            col = new DataGridViewCheckBoxColumn ();
            col.SortMode = DataGridViewColumnSortMode.Automatic;
            col.HeaderText = "Is List";
            col.Name = "IsList";
            col.Width = 60;
            propDGV.Columns.Add (col);

            col = new DataGridViewCheckBoxColumn ();
            col.SortMode = DataGridViewColumnSortMode.Automatic;
            col.HeaderText = "Has Restriction";
            col.Name = "HasRestriction";
            col.Width = 60;
            propDGV.Columns.Add (col);

            foreach (PropInfo item in _objInfo.Properties)
            {
                int index = propDGV.Rows.Add (
                    item.Id,
                    item.Name,
                    item.PropType,
                    item.IsAssociation,
                    item.Containment,
                    item.Cardinality,
                    item.OtherCardinality,
                    item.Nullable,
                    item.IsList,
                    (item.Restriction != null && item.Restriction.RestrictionValues.Count > 0));

                propDGV.Rows [index].Tag = item;
            }

            //propDGV.Sort (propDGV.Columns [1], ListSortDirection.Ascending);
        }

        private void closeButton_Click (object sender, EventArgs e)
        {
            Close ();
        }

        private void PropForm_FormClosing (object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide ();
            _owner.Visible = true;
            _owner.Activate ();
        }

        private void propDGV_CurrentCellChanged (object sender, EventArgs e)
        {
            if (propDGV.CurrentRow == null)
            {
                docTextBox.Text = "";
                return;
            }

            PropInfo pi = (propDGV.CurrentRow.Tag as PropInfo);
            if (pi == null)
            {
                docTextBox.Text = "";
                return;
            }
            docTextBox.Text = pi.Documentation;



            if (pi.Restriction == null)
            {
                restrictionTextBox.Text = "";
                return;
            }

            restrictionTextBox.Text = pi.Restriction.ToString ();
        }
    }
}
