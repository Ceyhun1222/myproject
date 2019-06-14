using System;
using System.Windows.Forms;
using Holding.Models;

namespace Holding.Forms
{
    public partial class frmDrawTest : Form
    {
        private Bussines_Logic _blogic;
        private HoldingGeometry _holdingGeom;
        public frmDrawTest()
        {
            InitializeComponent();
        }

        public frmDrawTest(Bussines_Logic bLogic)
        {
            InitializeComponent();
            _blogic = bLogic;
            _holdingGeom = _blogic.HoldingGeom;
            
        }

        private void frmDrawTest_Load(object sender, EventArgs e)
        {
            holdingGeometryBindingSource.DataSource = _holdingGeom;
            holdingGeometryBindingSource.ResetBindings(true);
        }

        private void ckbShablon_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbShablon.Checked)
                _holdingGeom.DrawShablon();
            else
                GlobalParams.UI.SafeDeleteGraphic(_holdingGeom.ShablonHandle);
        }

        private void ckbToleranceArea_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbToleranceArea.Checked)
                _holdingGeom.DrawToleranceArea();
            else
                GlobalParams.UI.SafeDeleteGraphic(_holdingGeom.ToleranceAreaHandle);
        }

        private void ckbTurn180_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
                _holdingGeom.DrawTurn180();
            else
                GlobalParams.UI.SafeDeleteGraphic(_holdingGeom.Turn180Handle);
        }

        private void Buffers_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
                _holdingGeom.DrawBuffers();
            else
                _holdingGeom.ClearBuffers();
        }

        private void ckbSector_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
                _holdingGeom.DrawSectorEntries();
            else
                GlobalParams.UI.SafeDeleteGraphic(_holdingGeom.SectorProtectionHandle);
        }

        private void ckbTrack_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
                _holdingGeom.DrawHoldingTrack();
            else
                GlobalParams.UI.SafeDeleteGraphic(_holdingGeom.HoldingTrackHandle);
        }
    }
}
