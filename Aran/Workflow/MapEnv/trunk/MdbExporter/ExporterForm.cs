using Aran.Aim;
using MapEnv;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Metadata.UI;

namespace Aran.Exporter.Gdb
{
    public partial class ExporterForm : Form
    {
        private List<ExportLayerInfo> _expLayerInfoList;
        private ExportLayerInfo _selectedExLI;


        public ExporterForm()
        {
            InitializeComponent();

            WorkspaceType = ExportWorkspaceType.PersonalGdb;
            _expLayerInfoList = new List<ExportLayerInfo>();
        }

        private ExportWorkspaceType WorkspaceType { get; set; }

        private void ExporterForm_Load(object sender, EventArgs e)
        {
            LoadExpInfoFromMetadata();
        }

        private void LoadExpInfoFromMetadata()
        {
            var classInfoList = AimMetadata.AimClassInfoList;

            foreach (var aimLayer in MapEnv.Globals.MainForm.AimLayers) {
                if (aimLayer.Layer.Visible &&
                    aimLayer.LayerType == MapEnv.Toc.AimLayerType.AimSimpleShapefile) {

                    if (aimLayer.Layer is AimFeatureLayer) {
                        var aimFL = aimLayer.Layer as AimFeatureLayer;

                        if (!aimFL.IsComplex) {

                            var expLI = new ExportLayerInfo();
                            expLI.Layer = aimFL;
                            expLI.IsChecked = true;
                            _expLayerInfoList.Add(expLI);
                        }
                    }
                }
            }

            foreach (var expLI in _expLayerInfoList) {
                var classInfo = AimMetadata.GetClassInfoByIndex((int)expLI.Layer.FeatureType);
                FillExpPropInfos(expLI.Properties, classInfo.Properties);
            }

            if (_expLayerInfoList.Count == 0) {
                MessageBox.Show("No Layers have been added.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            ui_kButton.Enabled = true;
            FillLayers();
        }

        private void FillExpPropInfos(List<ExportPropInfo> expProps, AimPropInfoList aimProps)
        {
            foreach (var propInfo in aimProps) {

                if (propInfo.TypeIndex == (int)ObjectType.MdMetadata)
                    continue;

                if (propInfo.TypeIndex == (int)ObjectType.Note ||
                    propInfo.TypeIndex == (int)DataType.TimeSlice ||
                    propInfo.TypeIndex == (int)ObjectType.ConditionCombination) {
                    continue;
                }

                var expPI = new ExportPropInfo();
                expPI.AimPropInfo = propInfo;
                expPI.IsChecked = propInfo.UiPropInfo().ShowGridView;
                expProps.Add(expPI);

                if (propInfo.TypeIndex == (int) DataType.FeatureRef ||
                    propInfo.PropType.SubClassType == AimSubClassType.ValClass ||
                    propInfo.PropType.SubClassType == AimSubClassType.AbstractFeatureRef) {
                    continue;
                }

                if (propInfo.PropType.Properties.Count > 0)
                    FillExpPropInfos(expPI.ChildList, propInfo.PropType.Properties);
            }
        }

        private void FillLayers()
        {
            ui_featuresCLB.Items.Clear();

            foreach (var expLI in _expLayerInfoList) {
                ui_featuresCLB.Items.Add(expLI, expLI.IsChecked);
            }

            ui_featuresCLB.SelectedIndex = 0;
        }

        private void FillProperties()
        {
            ui_propsTV.Nodes.Clear();

            if (_selectedExLI == null)
                return;

            foreach (var expPI in _selectedExLI.Properties)
                FillProperties(expPI, ui_propsTV.Nodes);
        }

        private void FillProperties(ExportPropInfo expPI, TreeNodeCollection nodeColl)
        {
            if (string.IsNullOrEmpty(expPI.AimPropInfo.AixmName))
                return;

            var tn = new TreeNode();
            tn.Checked = expPI.IsChecked;
            tn.Tag = expPI;

            if (expPI.AimPropInfo.IsList)
                tn.NodeFont = new Font(ui_propsTV.Font, FontStyle.Underline);

            var isComplex = (expPI.ChildList.Count > 0);

            if (isComplex) {
                tn.Text = string.Format("   {0}  ({1}{2})",
                    expPI.AimPropInfo.AixmName,
                    expPI.AimPropInfo.PropType.Name,
                    expPI.AimPropInfo.IsList ? " - List" : "");
                tn.NodeFont = new System.Drawing.Font(ui_propsTV.Font, FontStyle.Bold);
            }
            else {
                tn.Text = expPI.AimPropInfo.AixmName;
                tn.NodeFont = new System.Drawing.Font(ui_propsTV.Font, FontStyle.Regular);
            }

            nodeColl.Add(tn);

            

            foreach (var childExpPI in expPI.ChildList) {
                FillProperties(childExpPI, tn.Nodes);
            }

            if (isComplex)
                WindowsAPI.HideCheckBox(ui_propsTV, tn);
        }

        private void Features_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedExLI = ui_featuresCLB.SelectedItem as ExportLayerInfo;

            FillProperties();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            
        }

        private void PropsTreeView_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            var expPI = e.Node.Tag as ExportPropInfo;
            if (expPI.ChildList.Count > 0)
                e.Cancel = true;
        }

        private void PropsTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            var expPI = e.Node.Tag as ExportPropInfo;
            expPI.IsChecked = e.Node.Checked;
        }

        private void SetIsChecked(List<ExportPropInfo> expPIList)
        {
            foreach (var propItem in expPIList) {
                if (propItem.ChildList.Count > 0) {
                    propItem.IsChecked = propItem.IsAnyChildChecked();
                    SetIsChecked(propItem.ChildList);
                }
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.OverwritePrompt = false;
            sfd.Filter = "ArcMap Document and Personal DB File (*.mxd & *.mdb)|*.mxd;*.mdb";

            sfd.FileOk += ExportToGDGSaveDialog_FileOk;

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            foreach (var item in _expLayerInfoList) {
                SetIsChecked(item.Properties);
            }

            var gdbExp = new Aran.Exporter.Gdb.GdbExporter2();
            gdbExp.WorkspaceType = WorkspaceType;
            gdbExp.Layers.AddRange(_expLayerInfoList);

            try {

                List<Exception> errors = null;
                int savedTableCount = 0;
                int savedFCCount = 0;

                errors = gdbExp.Export(sfd.FileName, ref savedTableCount, ref savedFCCount);

                MessageBox.Show(string.Format(
                    "Export to GDB finished.\n" +
                    "Saved FeatureClass count: {0}\n" +
                    "Saved AttributeTable count: {1}\n" +
                    "Ignored all row items count: {2}",
                    savedFCCount, savedTableCount, errors.Count), "Export to GDB");
            }
            catch (Exception ex) {
                MessageBox.Show("Error:\n" + ex.Message, "Export to GDB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
        }

        private void ExportToGDGSaveDialog_FileOk(object sender, CancelEventArgs e)
        {
            var sfd = sender as SaveFileDialog;
            var dir = System.IO.Path.GetDirectoryName(sfd.FileName);
            var fn = System.IO.Path.GetFileNameWithoutExtension(sfd.FileName);

            if (System.IO.File.Exists(sfd.FileName) ||
                System.IO.File.Exists(dir + "//" + fn + ".mdb")) {

                MessageBox.Show(string.Format(
                    "\"{0}.mxd\" or \"{0}.mdb\" already exists!\n" +
                    "Please, set the other name", fn), "Export to GDB",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                e.Cancel = true;
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FeaturesCLB_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var expLayInfo = ui_featuresCLB.Items[e.Index] as ExportLayerInfo;
            expLayInfo.IsChecked = (e.NewValue == CheckState.Checked);
        }

        private void ExpDbTypePersonal_CheckedChanged(object sender, EventArgs e)
        {
            WorkspaceType = ExportWorkspaceType.PersonalGdb;
        }

        private void ExpDbTypeFileGDB_CheckedChanged(object sender, EventArgs e)
        {
            WorkspaceType = ExportWorkspaceType.FileGdb;
        }

        private void CheckAll_CheckedChanged(object sender, EventArgs e)
        {
            ui_checkAll.Text = ui_checkAll.Checked ? "Uncheck All" : "Check All";
            SetIsChecked(ui_propsTV.Nodes, ui_checkAll.Checked);
        }

        private void SetIsChecked(TreeNodeCollection treeNodeColl, bool isChecked)
        {
            foreach (TreeNode tn in treeNodeColl) {
                tn.Checked = isChecked;
                SetIsChecked(tn.Nodes, isChecked);
            }
        }
    }


}
