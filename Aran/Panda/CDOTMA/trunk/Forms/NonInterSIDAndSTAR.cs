using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetTopologySuite.Geometries;
using PDM;
using GeoAPI.Geometries;

namespace CDOTMA
{
	public partial class NonInterSIDAndSTAR : Form
	{
		int _selectedID;
		int _selectedNominalID;

		int _conflictID;
		int _conflictNominalID;

		List<ProcedureType> _procedures;
		ProcedureType _currProc;
		List<ProcedureType> _conflictProcedures;

		Drawings.FillElement _selectedElem;
		Drawings.LineElement _selectedNominalElem;

		Drawings.FillElement _conflictElem;
		Drawings.LineElement _conflictNominalElem;

		double minlev, maxlev;

		public NonInterSIDAndSTAR()
		{
			InitializeComponent();

			_selectedElem = new Drawings.FillElement();
			_selectedElem.Color = 16776960;
			//_selectedElem.Width = 2;
			_selectedElem.Style = 20;

			_selectedNominalElem = new Drawings.LineElement();
			_selectedNominalElem.Color = 16776960; //Functions.RGB(255, 255, 0);	//65535
			_selectedNominalElem.Width = 2;
			_selectedNominalElem.Style = 4;

			_conflictElem = new Drawings.FillElement();
			_conflictElem.Color = 255;
			_conflictElem.Style = 19;

			_conflictNominalElem = new Drawings.LineElement();
			_conflictNominalElem.Color = 255;
			_conflictNominalElem.Width = 2;
			_conflictNominalElem.Style = 4;

			listView1.Columns[2].Text = listView1.Columns[2].Text + " (FL)";
			listView1.Columns[3].Text = listView1.Columns[3].Text + " (FL)";

			label3.Text = GlobalVars.unitConverter.DistanceUnit;

			comboBox1.Items.Add(GlobalVars.unitConverter.DistanceToDisplayUnits(9260.0));
			comboBox1.Items.Add(GlobalVars.unitConverter.DistanceToDisplayUnits(12964.0));
		}

		public DialogResult ShowModal(IWin32Window owner)
		{
			return this.ShowDialog(owner);
		}

		public void ShowForm(IWin32Window owner)
		{
			Cursor.Current = Cursors.WaitCursor;

			_selectedID = 0;
			_selectedNominalID = 0;

			_conflictID = 0;
			_conflictNominalID = 0;

			_currProc = default(ProcedureType);
			_procedures = new List<ProcedureType>();
			_conflictProcedures = new List<ProcedureType>();

			checkBoxCheckedChanged(cbIAP, null);

			Cursor.Current = Cursors.Default;

			this.Show(owner);
		}


		private void NonInterSIDAndSTAR_FormClosed(object sender, FormClosedEventArgs e)
		{
			GlobalVars.mainForm.viewControl1.DeleteGeometry(_selectedID);
			GlobalVars.mainForm.viewControl1.DeleteGeometry(_selectedNominalID);

			GlobalVars.mainForm.viewControl1.DeleteGeometry(_conflictID);
			GlobalVars.mainForm.viewControl1.DeleteGeometry(_conflictNominalID);
		}

		private void checkBoxCheckedChanged(object sender, EventArgs e)
		{
			_procedures.Clear();

			if (cbIAP.Checked)
				foreach (ProcedureType pr in GlobalVars.ApproachProcedures)
					_procedures.Add(pr);

			if (cbSTAR.Checked)
				foreach (ProcedureType pr in GlobalVars.ArrivalProcedures)
					_procedures.Add(pr);

			if (cbSID.Checked)
				foreach (ProcedureType pr in GlobalVars.DepartureProcedures)
					_procedures.Add(pr);

			if (cbATS.Checked)
				foreach (ProcedureType pr in GlobalVars.Routs)
					_procedures.Add(pr);

			comboBox1_SelectedIndexChanged(comboBox1, null);
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox1.SelectedIndex < 0)
				comboBox1.SelectedIndex = 0;

			double buffVal = GlobalVars.unitConverter.DistanceToInternalUnits((double)comboBox1.SelectedItem);
			int n =_procedures.Count;
			ProcedureType selectedPr;
			selectedPr.pProcedure = null;
			TreeNode selected = null;

			if (treeView1.SelectedNode != null)
				selectedPr = (ProcedureType)(treeView1.SelectedNode.Tag);

			treeView1.BeginUpdate();
			treeView1.Nodes.Clear();

			for (int i = 0; i < n; i++ )
			{
				ProcedureType pr = _procedures[i];
				pr.RouteBuffer = null;

				Geometry geom = (Geometry)pr.NominalLine.Buffer(buffVal);
				if (geom.OgcGeometryType == GeoAPI.Geometries.OgcGeometryType.Polygon)
					pr.RouteBuffer = new MultiPolygon(new Polygon[] { (Polygon)geom });
				else
					pr.RouteBuffer = (MultiPolygon)geom;

				_procedures[i] = pr;

				TreeNode tNode = treeView1.Nodes.Add(pr.Name);
				tNode.Tag = pr;
				if (selectedPr.pProcedure == pr.pProcedure)
					selected = tNode;
			}

			treeView1.Sort();
			treeView1.EndUpdate();

			if (selected != null)
			{
				treeView1.SelectedNode = selected;
				treeView1_AfterSelect(treeView1, new TreeViewEventArgs(selected, TreeViewAction.Expand));
			}
			else if (treeView1.Nodes.Count > 0)
			{
				treeView1.SelectedNode = treeView1.Nodes[0];
				treeView1_AfterSelect(treeView1, new TreeViewEventArgs(treeView1.SelectedNode, TreeViewAction.Expand));
			}
			else
			{
				GlobalVars.mainForm.viewControl1.DeleteGeometry(_selectedID);
				GlobalVars.mainForm.viewControl1.DeleteGeometry(_selectedNominalID);
			}
		}

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Action == TreeViewAction.Unknown)
				return;

			GlobalVars.mainForm.viewControl1.DeleteGeometry(_selectedID);
			GlobalVars.mainForm.viewControl1.DeleteGeometry(_selectedNominalID);

			GlobalVars.mainForm.viewControl1.DeleteGeometry(_conflictID);
			GlobalVars.mainForm.viewControl1.DeleteGeometry(_conflictNominalID);

			//foreach (int id in _conflictID)
			//    GlobalVars.mainForm.viewControl1.DeleteGeometry(id);
			//foreach (int id in _conflictNominalID)
			//    GlobalVars.mainForm.viewControl1.DeleteGeometry(id);
			//_conflictID.Clear();
			//_conflictNominalID.Clear();

			TreeNode currNode = e.Node;
			_currProc = (ProcedureType)currNode.Tag;

			minlev = 10000000.0;
			maxlev = -100;

			foreach (Transition tr in _currProc.procTransitions)
			{
				foreach (Leg lg in tr.procLegs)
				{
					if (lg.PathGeomPrj != null)
					{
						if (minlev > lg.HFinish) minlev = lg.HFinish;
						if (minlev > lg.HStart) minlev = lg.HStart;

						if (maxlev < lg.HFinish) minlev = lg.HFinish;
						if (maxlev < lg.HStart) minlev = lg.HStart;
					}
				}
			}

			listView1.BeginUpdate();
			listView1.Items.Clear();
			_conflictProcedures.Clear();

			foreach (var proc in _procedures)
			{
				if (proc.pProcedure == _currProc.pProcedure)
					continue;

				if (!_currProc.RouteBuffer.Disjoint(proc.NominalLine))
				{
					_conflictProcedures.Add(proc);

					ListViewItem lvi = listView1.Items.Add(proc.Name);
					lvi.Tag = proc;

					if (proc.procType == PDM.PROC_TYPE_code.Multiple)
						lvi.SubItems.Add("Route");
					else
						lvi.SubItems.Add(proc.procType.ToString());
				}
			}

			listView1.EndUpdate();

			_selectedID = GlobalVars.mainForm.viewControl1.DrawGeometry(_currProc.RouteBuffer , _selectedElem, -1);
			_selectedNominalID = GlobalVars.mainForm.viewControl1.DrawGeometry(_currProc.NominalLine, _selectedNominalElem);
		}

		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listView1.SelectedIndices.Count == 0)
				return;

			GlobalVars.mainForm.viewControl1.DeleteGeometry(_conflictID);
			GlobalVars.mainForm.viewControl1.DeleteGeometry(_conflictNominalID);

			ListViewItem lvi0 = ((ListView)sender).SelectedItems[0];
			ProcedureType conflictProc = (ProcedureType)lvi0.Tag;

			_conflictID = GlobalVars.mainForm.viewControl1.DrawGeometry(conflictProc.RouteBuffer, _conflictElem, -1);
			_conflictNominalID = GlobalVars.mainForm.viewControl1.DrawGeometry(conflictProc.NominalLine, _conflictNominalElem);

			//double baseLowerH, baseUpperH;
			//double baseDir = 0.0, baseAzt;
			///*========================================================================*/
			//if (!double.TryParse(lvi0.SubItems[4].Text, out baseAzt))
			//    if (!double.TryParse(lvi0.SubItems[5].Text, out baseAzt))
			//        return;

			////if (currLeg.ptStartPrj != null && Functions.ReturnDistanceInMeters(_currPt.pPtPrj, currLeg.ptStartPrj) > 11.0)
			////    baseDir = Functions.ReturnAngleInDegrees(_currPt.pPtPrj, currLeg.ptStartPrj);
			////else if (currLeg.ptEndPrj != null && Functions.ReturnDistanceInMeters(_currPt.pPtPrj, currLeg.ptEndPrj) > 11.0)
			////    baseDir = Functions.ReturnAngleInDegrees(_currPt.pPtPrj, currLeg.ptEndPrj);
			///*========================================================================*/

			//if (currLeg.HStart > currLeg.HFinish)
			//{
			//    baseLowerH = currLeg.HFinish;
			//    baseUpperH = currLeg.HStart;
			//}
			//else
			//{
			//    baseLowerH = currLeg.HStart;
			//    baseUpperH = currLeg.HFinish;
			//}
		}


	}
}
