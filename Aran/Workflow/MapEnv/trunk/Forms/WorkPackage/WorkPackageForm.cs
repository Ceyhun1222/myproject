using Aran.Aim.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapEnv
{
    public partial class WorkPackageForm : Form
    {
        private CawDbProvider _cawDbPro;
        public const string CurrentWorkPackageDataKey = "currentWorkPackage";

        public WorkPackageForm()
        {
            InitializeComponent();

            _cawDbPro = Globals.MainForm.DbProvider as CawDbProvider;
            Debug.Assert(_cawDbPro != null);
        }



        private void SelectWorkPackage_Click(object sender, EventArgs e)
        {
            var wpsForm = new WorkPackageSelectionForm();
            if (wpsForm.ShowDialog() != DialogResult.OK || wpsForm.SelectedWpOutline == null)
                return;

            ui_currWorkPackageTB.Text = wpsForm.SelectedWpOutline.Content.Name;

            _cawDbPro.IncludedWorkPackage = new WorkPackageName(
                wpsForm.SelectedWpOutline.Content.Id,
                wpsForm.SelectedWpOutline.Content.Name,
                wpsForm.SelectedWpOutline.Content.EffectiveDate);

            Globals.Environment.PutExtData(CurrentWorkPackageDataKey, _cawDbPro.IncludedWorkPackage);

            ui_clearWpButton.Enabled = true;
        }

        private void WorkPackageForm_Load(object sender, EventArgs e)
        {
            if (_cawDbPro.IncludedWorkPackage != null) {
                ui_currWorkPackageTB.Text = _cawDbPro.IncludedWorkPackage.Name;
                ui_clearWpButton.Enabled = true;
            }
        }

        private void ClearWorkPackage_Click(object sender, EventArgs e)
        {
            ui_currWorkPackageTB.Text = "";
            _cawDbPro.IncludedWorkPackage = null;
            ui_clearWpButton.Enabled = false;

            Globals.Environment.RemoveExtData(CurrentWorkPackageDataKey);
        }

        private void ShowFeatures_Click(object sender, EventArgs e)
        {
            if (_cawDbPro.IncludedWorkPackage == null)
                return;

            var workPackage = _cawDbPro.GetWorkPackage(_cawDbPro.IncludedWorkPackage.Id);
            //*** TODO....
        }

        private void SaveWorkPackage_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to SAVE current Work Package", "Work Package",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            _cawDbPro.CloseCurrentWorkPackage(true);
        }

        private void DiscardWorkPackage_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to DISCARD current Work Package", "Work Package",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            _cawDbPro.CloseCurrentWorkPackage(false);
        }
    }
}
