using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Metadata.UI;

namespace UIMetadatEditor
{
    public partial class MainForm : Form
    {
        private MainController _controller;
        private int _currentRowIndex;
        private int _lastRowIndex;

        public MainForm ()
        {
            InitializeComponent ();

            _controller = new MainController ();
            _currentRowIndex = -1;
            _lastRowIndex = -1;
        }

        private void MainForm_Load (object sender, EventArgs e)
        {
            FillMainDataGrid ();

            ui_showPropertiesCheckBox.Checked = true;
        }

        private void MainForm_FormClosing (object sender, FormClosingEventArgs e)
        {
            if (_controller.IsChanged)
            {
                DialogResult dr = MessageBox.Show ("Save changes?", Text,
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                    e.Cancel = !Save ();
                else if (dr == DialogResult.Cancel)
                    e.Cancel = true;
            }
        }

        private void FillMainDataGrid ()
        {
            List<AimClassInfo> classInfoList = _controller.ClassInfoList;

            foreach (AimClassInfo classInfo in classInfoList)
            {
                UIClassInfo uiInfo = classInfo.UiInfo ();
                DataGridViewRow row = new DataGridViewRow ();

                for (int i = 0; i < ui_classInfoDGV.Columns.Count; i++)
                {
                    DataGridViewCell cell = (DataGridViewCell) ui_classInfoDGV.Columns [i].CellTemplate.Clone ();

                    switch (i)
                    {
                        case 0: cell.Value = classInfo.Name; break;
                        case 1: cell.Value = classInfo.AimObjectType; break;
                        case 2: cell.Value = uiInfo.Caption; break;
                        case 3:
                            {
                                DataGridViewComboBoxCell cbCell = (DataGridViewComboBoxCell) cell;
                                DependsFeatureFinder dependsFinder = new DependsFeatureFinder (classInfoList);
                                int index;
                                string [] features = dependsFinder.GetDependFeatureNames (classInfo, out index);
                                cbCell.Items.Add ("");
                                cbCell.Items.AddRange (features);

                                if (!string.IsNullOrEmpty (uiInfo.DependsFeature))
                                {
                                    cbCell.Value = uiInfo.DependsFeature;
                                }
                                else if (index != -1)
                                {
                                    cbCell.Value = features [index];
                                }

                                break;
                            }
                        case 4:
                            {
                                if (classInfo.Parent != null)
                                {
                                    cell.Value = classInfo.Parent.Name;
                                    cell.Tag = classInfo;
                                }
                            }
                            break;
                    }

                    row.Cells.Add (cell);
                }

                row.Tag = classInfo;
                ui_classInfoDGV.Rows.Add (row);
            }

            SetClassInfoCount ();
        }

        private void SetClassInfoCount ()
        {
            int n = 0;
            foreach (DataGridViewRow row in ui_classInfoDGV.Rows)
            {
                if (row.Visible)
                    n++;
            }
            ui_classInfoCountStatusLabel.Text = n.ToString ();
        }

        private void ui_showOnlyFeaturesCheckBox_CheckedChanged (object sender, EventArgs e)
        {
            bool b = ! ui_showOnlyFeaturesCheckBox.Checked;

            foreach (DataGridViewRow row in ui_classInfoDGV.Rows)
            {
                if ((row.Tag as AimClassInfo).AimObjectType != AimObjectType.Feature)
                {
                    row.Visible = b;
                }
            }

            SetClassInfoCount ();
        }

        private void ui_mainDGV_CellEndEdit (object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = ui_classInfoDGV.Rows [e.RowIndex];
            AimClassInfo classInfo = (AimClassInfo) row.Tag;

            if (e.ColumnIndex == ui_Column3.Index)
            {
                _controller.SetCaption (classInfo, (string) row.Cells [e.ColumnIndex].Value);
            }
            else if (e.ColumnIndex == ui_Column4.Index)
            {
                _controller.SetDependsFeature (classInfo, (string) row.Cells [e.ColumnIndex].Value);
            }
        }

        private void ui_saveButton_Click (object sender, EventArgs e)
        {
            Save ();
        }

        private bool Save ()
        {
            SaveFileDialog sfd = new SaveFileDialog ();
            sfd.FileName = "AimUIModel.xml";
            sfd.Filter = "XML Files (*.xml)|*.xml";

            if (sfd.ShowDialog () == DialogResult.OK)
            {
                _controller.Save (sfd.FileName);
                return true;
            }
            return false;
        }

        private void ui_showPropertiesCheckBox_CheckedChanged (object sender, EventArgs e)
        {
            ui_mainSplitContainer.Panel2Collapsed = !ui_showPropertiesCheckBox.Checked;
        }

        private void ui_classInfoDGV_CurrentCellChanged (object sender, EventArgs e)
        {
            if (ui_classInfoDGV.CurrentCell == null)
                return;

            int rowIndex = ui_classInfoDGV.CurrentCell.RowIndex;
            if (rowIndex != _currentRowIndex)
            {
                FillPropWindow ((AimClassInfo) ui_classInfoDGV.Rows [rowIndex].Tag);

                _lastRowIndex = _currentRowIndex;
                _currentRowIndex = rowIndex;
            }
        }

        private void FillPropWindow (AimClassInfo classInfo)
        {
            UIClassInfo uiClassInfo = classInfo.UiInfo ();

            ui_propInfoLabel.Text = classInfo.Name;
            ui_descFormatTextBox.Tag = null;
            ui_descFormatTextBox.Text = uiClassInfo.DescriptionFormat;
            ui_descFormatTextBox.Tag = uiClassInfo;

            ui_propInfoDGV.Rows.Clear ();

            foreach (AimPropInfo propInfo in classInfo.Properties)
            {
                UIPropInfo uiPropInfo = propInfo.UiPropInfo ();

                string propTypeName = AimMetadata.GetAimTypeName (propInfo.TypeIndex);
                if (propInfo.PropType.AimObjectType == AimObjectType.Field && propTypeName.StartsWith ("Sys"))
                    propTypeName = propTypeName.Substring (3);

                int index = ui_propInfoDGV.Rows.Add ();
                DataGridViewRow row = ui_propInfoDGV.Rows [index];
                row.Tag = propInfo;
                row.Cells [0].Value = propInfo.Name;
                row.Cells [1].Value = propTypeName;
                row.Cells [2].Value = uiPropInfo.Caption;
                row.Cells [3].Value = uiPropInfo.ShowGridView;
            }

            ui_propInfoCountStatusLabel.Text = ui_propInfoDGV.Rows.Count.ToString ();
        }

        private void ui_propInfoDGV_CellEndEdit (object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == ui_propInfoColumn4.Index)
            {
                DataGridViewRow row = ui_propInfoDGV.Rows [e.RowIndex];
                AimPropInfo propInfo = (AimPropInfo) row.Tag;

                _controller.SetPropInfoShowGridView (propInfo, (bool) row.Cells [e.ColumnIndex].Value);
            }
        }

        private void ui_quickSearchTextBox_Leave (object sender, EventArgs e)
        {
            ui_quickSearchTextBox.TextChanged -= new EventHandler (ui_quickSearchTextBox_TextChanged);
            ui_quickSearchTextBox.Text = "Quick Search";
            ui_quickSearchTextBox.Font = new Font (ui_quickSearchTextBox.Font, FontStyle.Italic);
            ui_quickSearchTextBox.ForeColor = Color.Gray;
        }

        private void ui_quickSearchTextBox_Enter (object sender, EventArgs e)
        {
            ui_quickSearchTextBox.Text = "";
            ui_quickSearchTextBox.Font = new Font (ui_quickSearchTextBox.Font, FontStyle.Regular);
            ui_quickSearchTextBox.ForeColor = SystemColors.ControlText;
            ui_quickSearchTextBox.TextChanged += new EventHandler (ui_quickSearchTextBox_TextChanged);
        }

        private void ui_quickSearchTextBox_TextChanged (object sender, EventArgs e)
        {
            if (ui_quickSearchTextBox.Text == "")
                return;

            int startIndex = -1; // ui_classInfoDGV.CurrentCell.RowIndex;

            for (int i = startIndex + 1; i < ui_classInfoDGV.Rows.Count; i++)
            {
                DataGridViewRow row = ui_classInfoDGV.Rows [i];
                if (row.Visible)
                {
                    if (row.Cells [0].Value.ToString ().StartsWith (ui_quickSearchTextBox.Text, StringComparison.CurrentCultureIgnoreCase))
                    {
                        for (int j = 0; j < ui_classInfoDGV.Rows.Count; j++)
                        {
                            DataGridViewRow row2 = ui_classInfoDGV.Rows [j];

                            if (row2.Cells [0].Value.ToString ().
                                Equals (ui_quickSearchTextBox.Text, StringComparison.CurrentCultureIgnoreCase))
                            {
                                ui_classInfoDGV.CurrentCell = row2.Cells [0];
                                row2.Selected = true;
                                return;
                            }
                        }

                        ui_classInfoDGV.CurrentCell = row.Cells [0];
                        row.Selected = true;
                        return;
                    }
                }
            }
        }

        private void ui_classInfoDGV_CellContentClick (object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
                return;

            if (ui_Column5.Index == e.ColumnIndex)
            {
                object classInfoName = ui_classInfoDGV.Rows [e.RowIndex].Cells [e.ColumnIndex].Value;

                if (classInfoName == null)
                    return;

                foreach (DataGridViewRow row in ui_classInfoDGV.Rows)
                {
                    if (classInfoName.Equals (row.Cells [0].Value))
                    {
                        row.Selected = true;
                        ui_classInfoDGV.CurrentCell = row.Cells [0];
                        break;
                    }
                }
            }
        }

        private void ui_backButton_Click (object sender, EventArgs e)
        {
            if (_lastRowIndex == -1)
                return;

            DataGridViewRow row = ui_classInfoDGV.Rows [_lastRowIndex];
            ui_classInfoDGV.CurrentCell = row.Cells [0];
            row.Selected = true;
            _lastRowIndex = -1;
        }

        private void testButton_Click (object sender, EventArgs e)
        {
            _controller.Test ();
        }

        private void ui_descFormatTextBox_TextChanged (object sender, EventArgs e)
        {
            UIClassInfo uiClassInfo = ui_descFormatTextBox.Tag as UIClassInfo;
            if (uiClassInfo == null)
                return;
            uiClassInfo.DescriptionFormat = ui_descFormatTextBox.Text;
        }
    }
}
