using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace CDOTMA.CoordinatSystems
{
	partial class ProjectionDlg : Form
	{
		ProjectedCoordinatSystem _pcs;

		public ProjectionDlg()
		{
			InitializeComponent();
		}

		static ProjectedCoordinatSystem ClonePCS(ProjectedCoordinatSystem scr)
		{
			ProjectedCoordinatSystem dst = new ProjectedCoordinatSystem(scr.Name, scr.projectionName, scr.baseCoordinatSystem,
				scr.unit, scr.CentralMeridian, scr.ScaleFactor, scr.FalseEasting, scr.FalseNorthing, scr.LatitudeOfOrigin);
			return dst;
		}

		ProjectedCoordinatSystem CreateResultPCS(ProjectedCoordinatSystem scr)
		{
			Unit unit = new Unit(cmbLUName.Text, double.Parse(txbMetPerUnit.Text));

			ProjectedCoordinatSystem dst = new ProjectedCoordinatSystem(txbGenName.Text, cmbPrName.Text, scr.baseCoordinatSystem,
				unit, scr.CentralMeridian, scr.ScaleFactor, scr.FalseEasting, scr.FalseNorthing, scr.LatitudeOfOrigin);
			return dst;
		}

		public static System.Windows.Forms.DialogResult ShowDialog(out CoordinatSystem result, CoordinatSystem oldCS = null, string Caption = null)
		{
			ProjectionDlg dlg = new ProjectionDlg();

			if (Caption != null)
				dlg.Text = Caption;

			if (!(oldCS is ProjectedCoordinatSystem))
				result = new ProjectedCoordinatSystem();
			else
				result = ClonePCS((ProjectedCoordinatSystem)oldCS);

			dlg._pcs = (ProjectedCoordinatSystem)result;

			dlg.txbGenName.Text = dlg._pcs.Name;
			dlg.cmbPrName.Text = dlg._pcs.projectionName;

			foreach (Parameter prm in dlg._pcs.Parameters)
			{
				DataGridViewRow dgvr = new DataGridViewRow();

				DataGridViewCell dgvc = new DataGridViewTextBoxCell();
				dgvc.Value = prm.Name;
				dgvr.Cells.Add(dgvc);

				dgvc = new DataGridViewTextBoxCell();
				dgvc.Value = prm.Value;					//.ToString();
				dgvr.Cells.Add(dgvc);

				dlg.dgvPrjParams.Rows.Add(dgvr);
			}

			dlg.cmbLUName.Text = dlg._pcs.unit.Name;
			dlg.txbMetPerUnit.Text = dlg._pcs.unit.Multiplier.ToString();

			string strGCS = string.Format("Name: {0}\nAngular Unit: {1} ({2})\nPrime Meridian: {3} ({4})\n",
				dlg._pcs.baseCoordinatSystem.Name,
				dlg._pcs.baseCoordinatSystem.unit.Name, dlg._pcs.baseCoordinatSystem.unit.Multiplier,
				dlg._pcs.baseCoordinatSystem.primem.Name, dlg._pcs.baseCoordinatSystem.primem.Value);

			double SemiminorAxis = (dlg._pcs.baseCoordinatSystem.datum.spheroid.SemiMajorAxis * (1.0 - 1.0 / dlg._pcs.baseCoordinatSystem.datum.spheroid.InversFlattening));

			strGCS = strGCS + string.Format("Datum: {0}\n  Spheroid: {1}\n    Semimajor Axis: {2}\n    Semiminor Axis: {3}\n    Inverse Flattening: {4}",
				dlg._pcs.baseCoordinatSystem.datum.Name, dlg._pcs.baseCoordinatSystem.datum.spheroid.Name, dlg._pcs.baseCoordinatSystem.datum.spheroid.SemiMajorAxis,
				SemiminorAxis, dlg._pcs.baseCoordinatSystem.datum.spheroid.InversFlattening);

			System.Windows.Forms.DialogResult dialogResult = dlg.ShowDialog();

			if (dialogResult == System.Windows.Forms.DialogResult.OK)
				result = dlg.CreateResultPCS(dlg._pcs);

			dlg = null;
			return dialogResult;
		}

		private void cmbPrName_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void cmbLUName_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cmbLUName.SelectedIndex == 0)
				txbMetPerUnit.Text = "1";
			else if (cmbLUName.SelectedIndex == 1)
				txbMetPerUnit.Text = "1000";
		}

		private void dgvPrjParams_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			double value;

			if (!double.TryParse(dgvPrjParams.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out value))
			{
				switch (e.RowIndex)
				{
					case 0:
						dgvPrjParams.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = _pcs.FalseEasting;
						break;
					case 1:
						dgvPrjParams.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = _pcs.FalseNorthing;
						break;
					case 2:
						dgvPrjParams.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = _pcs.CentralMeridian;
						break;
					case 3:
						dgvPrjParams.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = _pcs.ScaleFactor;
						break;
					case 4:
						dgvPrjParams.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = _pcs.LatitudeOfOrigin;
						break;
				}
			}
			else switch (e.RowIndex)
				{
					case 0:
						_pcs.FalseEasting = value;
						break;
					case 1:
						_pcs.FalseNorthing = value;
						break;
					case 2:
						_pcs.CentralMeridian = value;
						break;
					case 3:
						_pcs.ScaleFactor = value;
						break;
					case 4:
						_pcs.LatitudeOfOrigin = value;
						break;
				}
		}

		private void txbMetPerUnit_Validating(object sender, CancelEventArgs e)
		{
			Unit unit = new Unit(cmbLUName.Text, double.Parse(txbMetPerUnit.Text));
		}

	}
}
