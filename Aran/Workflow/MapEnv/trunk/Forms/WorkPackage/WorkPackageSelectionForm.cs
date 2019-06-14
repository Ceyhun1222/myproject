using Aran.Aim.CAWProvider.Configuration;
using Aran.Aim.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapEnv
{
    public partial class WorkPackageSelectionForm : Form
    {
        private CawDbProvider _cawDbPro;
        private List<WorkPackageOutline> _wpOutlineList;
        


        public WorkPackageSelectionForm()
        {
            InitializeComponent();

            _cawDbPro = Globals.MainForm.DbProvider as CawDbProvider;
            _wpOutlineList = new List<WorkPackageOutline>();
            ui_wpDgv.RowCount = _wpOutlineList.Count;

            Globals.SetDGV_DoubleBuffered(ui_wpDgv);
        }

        public static bool IsCawDbProvider
        {
            get { return (Globals.MainForm.DbProvider.ProviderType == DbProviderType.ComSoft); }
        }

        public WorkPackageOutline SelectedWpOutline { get; private set; }


        private void WorkPackageSelectionForm_Load(object sender, EventArgs e)
        {
            _wpOutlineList = _cawDbPro.GetWorkPackages(Aran.Aim.CAWProvider.WorksPackageStatusType.Open);
            ui_wpDgv.RowCount = _wpOutlineList.Count;
            ui_wpDgv.Refresh();
        }

        private void WpDgv_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex > _wpOutlineList.Count - 1)
                return;

            var wpOutline = _wpOutlineList[e.RowIndex];
            if (wpOutline.Content == null)
                return;

            var wpBasic = wpOutline.Content;

            switch (e.ColumnIndex) {
                case 0:
                    e.Value = wpBasic.Id;
                    break;
                case 1:
                    e.Value = wpBasic.Name;
                    break;
                case 2:
                    e.Value = GetStatusString(wpBasic.Status);
                    break;
                case 3:
                    e.Value = wpBasic.EffectiveDate.ToString("yyyy-MM-dd");
                    break;
                case 4:
                    e.Value = wpOutline.TimeSlices.Count;
                    break;
            }
        }

        private void WpDgv_CurrentCellChanged(object sender, EventArgs e)
        {
            ui_featuresDgv.RowCount = 0;
            SelectedWpOutline = null;

            if (ui_wpDgv.CurrentCell == null)
                return;

            SelectedWpOutline = _wpOutlineList[ui_wpDgv.CurrentCell.RowIndex];
            ui_featuresDgv.RowCount = SelectedWpOutline.TimeSlices.Count;

            ui_featuresDgv.Refresh();
        }

        private string GetStatusString(string value)
        {
            return value;
        }

        private void FeaturesDgv_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (SelectedWpOutline == null || e.RowIndex < 0 || e.RowIndex > SelectedWpOutline.TimeSlices.Count - 1)
                return;

            var tsr = SelectedWpOutline.TimeSlices[e.RowIndex].Content;

            switch (e.ColumnIndex) {
                case 0:
                    e.Value = tsr.Identifier;
                    break;
                case 1:
                    e.Value = tsr.FeatureType;
                    break;
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
