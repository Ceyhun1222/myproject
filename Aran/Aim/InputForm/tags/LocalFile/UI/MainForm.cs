using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.CAWProvider;
using Aran.Aim.Features;
using Aran.Aim.InputFormLib;
using Aran.Queries.Viewer;
using Aran.Queries.Common;

namespace Aran.Aim.InputForm
{
    public partial class MainForm : Form
    {
        private InputFormController _controller;
        private string _currentFeatureName;

        public MainForm ()
        {
            InitializeComponent ();
        }

        private void MainForm_Load (object sender, EventArgs e)
        {
            try
            {
                _controller = new InputFormController ();
                _controller.ClosedEventHandler = new FormClosingEventHandler (DbEntityControl_Closed);
                _controller.SavedEventHandler = new FeatureEventHandler (DbEntityControl_Saved);
                _controller.OpenedEventHandler = new FeatureEventHandler (DbEntityControl_FeatureOpened);

            }
            catch (Exception ex)
            {
                ui_MainSplitContainer.Enabled = false;
                MessageBox.Show (ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            #region Fill Interpretation List
            Array values = Enum.GetValues (typeof (InterpretationType));
            foreach (object item in values)
                ui_interpretationComboBox.Items.Add (item);
            #endregion

            ui_interpretationComboBox.SelectedIndex = 0;

            ui_FeatureTypesDGV.CurrentCellChanged += new EventHandler (ui_FeatureTypesDGV_CurrentCellChanged);
            
            string [] featureNames = _controller.GetFeaturesByDepends (null);
            FillFeatureTypesDGV (featureNames);
        }

        private void ui_interpretationComboBox_SelectedIndexChanged (object sender, EventArgs e)
        {
            LoadFeatures ();
        }

        private void ui_FeatureTypesDGV_CurrentCellChanged (object sender, EventArgs e)
        {
            if (ui_FeatureTypesDGV.CurrentCell == null)
                return;

            _currentFeatureName = ui_FeatureTypesDGV.CurrentCell.Value.ToString ();
            ui_statusLabel1.Text = "Features count: 0";

            LoadFeatures ();
        }

        private void ui_nextButton_Click (object sender, EventArgs e)
        {
            if (ui_FeaturesDGV.CurrentRow != null)
            {
                Feature selectedFeature = ui_FeaturesDGV.CurrentRow.Tag as Feature;
                AddNavigationControl (selectedFeature);
            }

            string [] featureNames = _controller.GetFeaturesByDepends (_currentFeatureName);
            FillFeatureTypesDGV (featureNames);
        }

        private void ui_showPropButton_Click (object sender, EventArgs e)
        {
            if (ui_FeaturesDGV.CurrentRow == null)
                return;

            ui_MainSplitContainer.Visible = false;

            Feature feature = (Feature) ui_FeaturesDGV.CurrentRow.Tag;
            Control propControl = _controller.GetPropertiesControl (feature);

            propControl.Location = ui_MainSplitContainer.Location;
            propControl.Size = ui_MainSplitContainer.Size;
            propControl.Anchor = ui_MainSplitContainer.Anchor;
            ui_MainSplitContainer.Parent.Controls.Add (propControl);

            ui_navFlowLayoutPanel.Enabled = false;
        }

        private void ui_FeaturesDGV_CurrentCellChanged (object sender, EventArgs e)
        {
            ui_editTSB.Enabled = (ui_FeaturesDGV.CurrentCell != null);
            ui_nextTSB.Enabled = (ui_FeaturesDGV.CurrentCell != null);
        }

        private void ui_newButton_Click (object sender, EventArgs e)
        {
            if (_currentFeatureName == null)
                return;

            FeatureType featureType;
            if (!Enum.TryParse<FeatureType> (_currentFeatureName, out featureType))
            {
                return;
            }

            Feature newFeature = _controller.CreateNewFeature (featureType, GetLastNavFeature ());

            int newRowIndex = ui_FeaturesDGV.Rows.Add ();
            DataGridViewRow newRow = ui_FeaturesDGV.Rows [newRowIndex];
            newRow.Tag = newFeature;
            ui_FeaturesDGV.CurrentCell = newRow.Cells [0];
            ui_showPropButton_Click (ui_editTSB, null);
        }

        private void ui_exitToolStripMenuItem_Click (object sender, EventArgs e)
        {
            Close ();
        }

        private void ui_aboutToolStripMenuItem_Click (object sender, EventArgs e)
        {
            //Feature [] featArr = _controller.GetFeatures (FeatureType.Airspace, 
            //    InterpretationType.BASELINE, DateTime.Now, null);
            //return;

            AboutForm aboutForm = new AboutForm ();
            aboutForm.ShowDialog (this);
        }

        private void ui_exportToolStripMenuItem_Click (object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog ();
            sfd.Filter = "XML Files (*.xml)|*.xml";
            if (sfd.ShowDialog () != DialogResult.OK)
            {
                return;
            }

            string result = _controller.ExportToXml (sfd.FileName);

            if (result == null)
            {
                MessageBox.Show ("Successfully exported.");
            }
            else
            {
                MessageBox.Show ("Exported failed.\nError: " + result, Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ui_FeaturesDGV_RowsAdded (object sender, DataGridViewRowsAddedEventArgs e)
        {
            FeaturesCountChanged ();
        }

        private void ui_FeaturesDGV_RowsRemoved (object sender, DataGridViewRowsRemovedEventArgs e)
        {
            FeaturesCountChanged ();
        }

        private void ui_quickFeatTypeSearchTB_Enter (object sender, EventArgs e)
        {
            ui_quickSearchTextBox.Text = "";
            ui_quickSearchTextBox.Font = new Font (ui_quickSearchTextBox.Font, FontStyle.Regular);
            ui_quickSearchTextBox.ForeColor = SystemColors.ControlText;
            ui_quickSearchTextBox.TextChanged += new EventHandler (ui_quickSearchTextBox_TextChanged);
        }

        private void ui_quickFeatTypeSearchTB_Leave (object sender, EventArgs e)
        {
            ui_quickSearchTextBox.TextChanged -= new EventHandler (ui_quickSearchTextBox_TextChanged);
            ui_quickSearchTextBox.Text = "Quick Search";
            ui_quickSearchTextBox.Font = new Font (ui_quickSearchTextBox.Font, FontStyle.Italic);
            ui_quickSearchTextBox.ForeColor = Color.Gray;
        }

        private void ui_quickSearchTextBox_TextChanged (object sender, EventArgs e)
        {
            if (ui_quickSearchTextBox.Text == "")
                return;

            int startIndex = -1; // ui_classInfoDGV.CurrentCell.RowIndex;

            for (int i = startIndex + 1; i < ui_FeatureTypesDGV.Rows.Count; i++)
            {
                DataGridViewRow row = ui_FeatureTypesDGV.Rows [i];
                if (row.Visible)
                {
                    if (row.Cells [0].Value.ToString ().StartsWith (ui_quickSearchTextBox.Text,
                        StringComparison.CurrentCultureIgnoreCase))
                    {
                        for (int j = 0; j < ui_FeatureTypesDGV.Rows.Count; j++)
                        {
                            DataGridViewRow row2 = ui_FeatureTypesDGV.Rows [j];

                            if (row2.Cells [0].Value.ToString ().
                                Equals (ui_quickSearchTextBox.Text, StringComparison.CurrentCultureIgnoreCase))
                            {
                                ui_FeatureTypesDGV.CurrentCell = row2.Cells [0];
                                row2.Selected = true;
                                return;
                            }
                        }

                        ui_FeatureTypesDGV.CurrentCell = row.Cells [0];
                        row.Selected = true;
                        return;
                    }
                }
            }
        }

        private void ui_navFlowLayoutPanel_ControlAdded (object sender, ControlEventArgs e)
        {
        }

        private void ui_navFlowLayoutPanel_ControlRemoved (object sender, ControlEventArgs e)
        {
        }

        private void FillFeatureTypesDGV (string [] featureNames, string selectedFeature = null)
        {
            ui_FeatureTypesDGV.CurrentCellChanged -= new EventHandler (ui_FeatureTypesDGV_CurrentCellChanged);
            ui_FeatureTypesDGV.SuspendLayout ();
            ui_FeatureTypesDGV.Rows.Clear ();
            bool b = true;

            foreach (string featName in featureNames)
            {
                int index = ui_FeatureTypesDGV.Rows.Add (featName);

                if (b && featName.Equals (selectedFeature))
                {
                    ui_FeatureTypesDGV.Rows [index].Selected = true;
                    b = false;
                }
            }

            ui_FeatureTypesDGV.Sort (ui_FeatureNameColumn, ListSortDirection.Ascending);
            ui_FeatureTypesDGV.CurrentCellChanged += new EventHandler (ui_FeatureTypesDGV_CurrentCellChanged);
            ui_FeatureTypesDGV.ResumeLayout (true);

            if (!b && ui_FeatureTypesDGV.SelectedRows.Count > 0)
                ui_FeatureTypesDGV.CurrentCell = ui_FeatureTypesDGV.SelectedRows [0].Cells [0];
            else if (ui_FeatureTypesDGV.Rows.Count > 0)
                ui_FeatureTypesDGV.CurrentCell = ui_FeatureTypesDGV.Rows [0].Cells [0];

            ui_FeatureTypesDGV_CurrentCellChanged (ui_FeatureTypesDGV, null);
        }

        private bool LoadFeatures ()
        {
            if (_currentFeatureName == null)
                return false;

            FeatureType featureType;
            if (!Enum.TryParse<FeatureType> (_currentFeatureName, out featureType))
            {
                ui_FeaturesDGV.Columns.Clear ();
                ui_statusLabel1.Text = "Abstract Feature has not realized yet...";
                return false;
            }

            Feature [] featureArr = _controller.GetFeatures (featureType,
                (InterpretationType) ui_interpretationComboBox.SelectedItem,
                ui_effectiveDateTimePicker.Value,
                GetLastNavFeature ());

            FillFeaturesDGV (featureArr, featureType);

            return (ui_FeaturesDGV.Rows.Count > 0);
        }

        private void FillFeaturesDGV (Feature [] featureArr, FeatureType featureType)
        {
            _controller.FillColumns (featureType, ui_FeaturesDGV);

            foreach (Feature feature in featureArr)
                _controller.SetRow (ui_FeaturesDGV, feature);
        }

        private void DbEntityControl_Closed (object sender, FormClosingEventArgs e)
        {
            FeatureControl featureControl = sender as FeatureControl;

            if (featureControl.HasChanged ())
            {
                DialogResult dlgRes = MessageBox.Show ("Do you want to save changes?", Text,
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (dlgRes == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                else if (dlgRes == DialogResult.Yes)
                {
                    Feature editingFeature = featureControl.GetEditingFeature ();
                    if (_controller.SaveFeature (editingFeature))
                        _controller.SetRow (ui_FeaturesDGV, editingFeature, ui_FeaturesDGV.CurrentRow.Index);
                }
                else if (featureControl.RootFeature.Id == -1)
                {
                    ui_FeaturesDGV.Rows.Remove (ui_FeaturesDGV.CurrentRow);
                }
            }
            else if (featureControl.RootFeature.Id == -1)
            {
                ui_FeaturesDGV.Rows.Remove (ui_FeaturesDGV.CurrentRow);
            }

            ui_MainSplitContainer.Visible = true;
            ui_navFlowLayoutPanel.Enabled = true;
        }

        private void DbEntityControl_Saved (object sender, FeatureEventArgs e)
        {
            if (_controller.SaveFeature (e.Feature))
            {
                for (int i = 0; i < ui_FeaturesDGV.Rows.Count; i++)
                {
                    Feature rowFeature = (Feature) ui_FeaturesDGV.Rows [i].Tag;
                    if (rowFeature.Identifier.Equals (e.Feature.Identifier))
                    {
                        _controller.SetRow (ui_FeaturesDGV, rowFeature, i);
                        break;
                    }
                }
            }
        }

        private void DbEntityControl_FeatureOpened (object sender, FeatureEventArgs e)
        {
            //if (e.Feature is SegmentLeg)
            //{
            //    SegmentLeg sg = e.Feature as SegmentLeg;
            //    if (sg.StartPoint != null)
            //    {
            //        sg.StartPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.OTHER_WPT;
            //        _controller.SaveFeature (sg);
            //    }
            //}
        }

        private void AddNavigationControl (Feature feature)
        {
            string s = _controller.GetFeatureDescription (feature);
            if (s != null)
                s = feature.FeatureType + " ( " + s + " )";
            else
                s = feature.FeatureType.ToString ();

            LinkLabel linkLabel = new LinkLabel ();
            linkLabel.Text = s;
            linkLabel.Tag = feature;

            linkLabel.Margin = new Padding (5);
            linkLabel.AutoSize = true;
            linkLabel.Click += new EventHandler (NavigatorLinkLabel_Click);

            ui_navFlowLayoutPanel.Controls.Add (linkLabel);
        }

        private void NavigatorLinkLabel_Click (object sender, EventArgs e)
        {
            Feature feature = ((Control) sender).Tag as Feature;

            for (int i = 0; i < ui_navFlowLayoutPanel.Controls.Count; i++)
            {
                if (feature == (Feature) ui_navFlowLayoutPanel.Controls [i].Tag)
                {
                    while (i < ui_navFlowLayoutPanel.Controls.Count)
                        ui_navFlowLayoutPanel.Controls.RemoveAt (i);
                    break;
                }
            }

            string dependsFeatureName = _controller.GetDependsFeature (feature);
            string [] featureNames = _controller.GetFeaturesByDepends (dependsFeatureName);
            FillFeatureTypesDGV (featureNames, feature.FeatureType.ToString ());

            foreach (DataGridViewRow row in ui_FeaturesDGV.Rows)
            {
                if (feature == (Feature) row.Tag)
                {
                    ui_FeaturesDGV.CurrentCell = row.Cells [0];
                    break;
                }
            }
        }

        private Feature GetLastNavFeature ()
        {
            if (ui_navFlowLayoutPanel.Controls.Count == 0)
                return null;
            return (Feature) ui_navFlowLayoutPanel.Controls [ui_navFlowLayoutPanel.Controls.Count - 1].Tag;
        }

        private void FeaturesCountChanged ()
        {
            ui_statusLabel1.Text = "Features count: " + ui_FeaturesDGV.Rows.Count;
        }
    }
}
