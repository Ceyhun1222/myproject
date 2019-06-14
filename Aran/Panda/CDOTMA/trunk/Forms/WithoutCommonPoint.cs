using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetTopologySuite.Geometries;
using PDM;
using GeoAPI.Geometries;
using CDOTMA.Drawings;

namespace CDOTMA
{
	public partial class WithoutCommonPoint : Form
	{
		int _selectedNominalID;
		int _conflictPointID;
		int _conflictNominalID;
		int _conflictAreaID;

		List<ProcedureType> _procedures;
		ProcedureType _currProc;
		List<ProcedureType> _conflictProcedures;

		Drawings.LineElement _selectedNominalElem;
		Drawings.PointElement _conflictPointElem;
		Drawings.LineElement _conflictNominalElem;
		Drawings.FillElement _conflictAreaElem;

		double minlev, maxlev;

		public WithoutCommonPoint()
		{
			InitializeComponent();

			_selectedNominalElem = new Drawings.LineElement();
			_selectedNominalElem.Color = Functions.RGB(128, 128, 255);	//65535
			_selectedNominalElem.Width = 2;
			_selectedNominalElem.Style = LineElementStyle.Dash;

			_conflictPointElem = new Drawings.PointElement();
			_conflictPointElem.Color = 255;
			_conflictPointElem.Size = 14;
			_conflictPointElem.Style = PointElementStyle.X;

			_conflictNominalElem = new Drawings.LineElement();
			_conflictNominalElem.Color = Functions.RGB(128, 255, 128);	//255;
			_conflictNominalElem.Width = 2;
			_conflictNominalElem.Style = LineElementStyle.Dash;

			_conflictAreaElem = new Drawings.FillElement();
			_conflictAreaElem.Color = Functions.RGB(255, 128, 128);//8421631
			_conflictAreaElem.Style = FillElementStyle.SmallConfetti;

			listView1.Columns[2].Text = listView1.Columns[2].Text + " (FL)";
			listView1.Columns[3].Text = listView1.Columns[3].Text + " (FL)";

			label6.Text = GlobalVars.unitConverter.DistanceUnit;

			//label3.Text = GlobalVars.unitConverter.DistanceUnit;
			//comboBox1.Items.Add(GlobalVars.unitConverter.DistanceToDisplayUnits(9260.0));
			//comboBox1.Items.Add(GlobalVars.unitConverter.DistanceToDisplayUnits(12964.0));
		}

		private void WithoutCommonPoint_FormClosed(object sender, FormClosedEventArgs e)
		{
			GlobalVars.mainForm.viewControl1.DeleteGeometry(_selectedNominalID);
			GlobalVars.mainForm.viewControl1.DeleteGeometry(_conflictPointID);
			GlobalVars.mainForm.viewControl1.DeleteGeometry(_conflictNominalID);
		}

		public void ShowForm(IWin32Window owner)
		{
			Cursor.Current = Cursors.WaitCursor;

			_selectedNominalID = 0;
			_conflictPointID = 0;
			_conflictNominalID = 0;

			_currProc = default(ProcedureType);
			_procedures = new List<ProcedureType>();
			_conflictProcedures = new List<ProcedureType>();

			checkBoxCheckedChanged(cbIAP, null);

			Cursor.Current = Cursors.Default;
			this.Show(owner);
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
	
			//========================================================
			int n = _procedures.Count;
			ProcedureType selectedPr;
			selectedPr.pProcedure = null;
			TreeNode selected = null;

			if (treeView1.SelectedNode != null)
				selectedPr = (ProcedureType)(treeView1.SelectedNode.Tag);

			treeView1.BeginUpdate();
			treeView1.Nodes.Clear();

			for (int i = 0; i < n; i++)
			{
				ProcedureType pr = _procedures[i];
				//pr.RouteBuffer = null;

				//Geometry geom = (Geometry)pr.NominalLine;//.Buffer(buffVal);
				//if (geom.OgcGeometryType == GeoAPI.Geometries.OgcGeometryType.Polygon)
				//	pr.RouteBuffer = new MultiPolygon(new Polygon[] { (Polygon)geom });
				//else
				//	pr.RouteBuffer = (MultiPolygon)geom;

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
				//GlobalVars.mainForm.viewControl1.DeleteGeometry(_selectedID);
				GlobalVars.mainForm.viewControl1.DeleteGeometry(_selectedNominalID);
			}
		}

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Action == TreeViewAction.Unknown)
				return;

			GlobalVars.mainForm.viewControl1.DeleteGeometry(_selectedNominalID);
			GlobalVars.mainForm.viewControl1.DeleteGeometry(_conflictPointID);
			GlobalVars.mainForm.viewControl1.DeleteGeometry(_conflictNominalID);

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

			foreach (ProcedureType proc in _procedures)
			{
				if (proc.pProcedure == _currProc.pProcedure)
					continue;

				Geometry geom = (Geometry)_currProc.NominalLine.Intersection(proc.NominalLine);

				//Coordinate crd = geom.Coordinate;

				//Drawings.PointElement pe = new Drawings.PointElement();
				//pe.Color = 0;
				//pe.Size = 10;
				//GlobalVars.mainForm.viewControl1.DrawGeometry(new Point(crd), pe, -1);

				//pe = new Drawings.PointElement();
				//pe.Size = 10;
				//pe.Color = 255;
				//GlobalVars.mainForm.viewControl1.DrawGeometry((Geometry)mpt.Geometries[0], pe, -1);

				//pe = new Drawings.PointElement();
				//pe.Size = 10;
				//pe.Color = Functions.RGB(0, 0, 255);
				//GlobalVars.mainForm.viewControl1.DrawGeometry((Geometry)mpt.Geometries[1], pe, -1);

				//GlobalVars.mainForm.viewControl1.DeleteGeometry(-2);
				//GlobalVars.mainForm.viewControl1.DeleteGeometry(-3);
				//GlobalVars.mainForm.viewControl1.DeleteGeometry(-6);

				//while (true)
				//System.Windows.Forms.Application.DoEvents();

				double ln = geom.Length;
				if (ln > GlobalVars.distEps || geom.IsEmpty)
					continue;

				bool IsCommonPt = false;
				int n = 1;

				if (geom.OgcGeometryType == OgcGeometryType.MultiPoint)
				{
					MultiPoint mpt = (MultiPoint)geom;
					n = mpt.NumPoints;
				}

				for (int j = 0; j < n; j++)
				{
					Coordinate crd = geom.Coordinates[j];
					if (_currProc.NominalLine.NumPoints < proc.NominalLine.NumPoints)
					{
						for (int i = 0; i < _currProc.NominalLine.NumPoints; i++)
						{
							double dist = _currProc.NominalLine.Coordinates[i].Distance(crd);
							if (dist < 20.0)
							{
								if (dist < GlobalVars.distEps)
								{
									IsCommonPt = true;
									break;
								}

								for (int k = 0; k < proc.NominalLine.NumPoints; k++)
								{
									dist = proc.NominalLine.Coordinates[k].Distance(crd);
									if (dist < 20.0)
									{
										IsCommonPt = true;
										break;
									}
								}

								if (IsCommonPt)
									break;
							}
						}
					}
					else
					{
						for (int i = 0; i < proc.NominalLine.NumPoints; i++)
						{
							double dist = proc.NominalLine.Coordinates[i].Distance(crd);
							if (dist < 20.0)
							{
								if (dist < GlobalVars.distEps)
								{
									IsCommonPt = true;
									break;
								}

								for (int k = 0; k < _currProc.NominalLine.NumPoints; k++)
								{
									dist = _currProc.NominalLine.Coordinates[k].Distance(crd);
									if (dist < 20.0)
									{
										IsCommonPt = true;
										break;
									}
								}

								if (IsCommonPt)
									break;
							}
						}
					}

					if (IsCommonPt)
						continue;

					ProcedureType procedure = proc;

					Point ptIntersect = new Point(crd);
					procedure.Tag = ptIntersect;

					_conflictProcedures.Add(procedure);

					ListViewItem lvi = listView1.Items.Add(procedure.Name);

					lvi.Tag = procedure;

					if (procedure.procType == PDM.PROC_TYPE_code.Multiple)
						lvi.SubItems.Add("Route");
					else
						lvi.SubItems.Add(procedure.procType.ToString());
				}
			}

			listView1.EndUpdate();

			_selectedNominalID = GlobalVars.mainForm.viewControl1.DrawGeometry(_currProc.NominalLine, _selectedNominalElem);
			comboBox1_SelectedIndexChanged(comboBox1, null);
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox1.SelectedIndex < 0)
			{
				comboBox1.SelectedIndex = 0;
				return;
			}

			double cpDist;
			if (comboBox1.SelectedIndex == 0)
				cpDist = 15.0 * 1852.0;
			else
				cpDist = 23.0 * 1852.0;

			textBox1.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(cpDist, eRoundMode.NERAEST).ToString();

			if (listView1.Items.Count != 0)
			{
				if (listView1.SelectedIndices.Count == 0)
					listView1.SelectedIndices.Add(0);

				listView1_SelectedIndexChanged(listView1, new EventArgs());
			}
		}

		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listView1.SelectedIndices.Count == 0)
				return;

			GlobalVars.mainForm.viewControl1.DeleteGeometry(_conflictPointID);
			GlobalVars.mainForm.viewControl1.DeleteGeometry(_conflictNominalID);
			GlobalVars.mainForm.viewControl1.DeleteGeometry(_conflictAreaID);

			ListViewItem lvi0 = ((ListView)sender).SelectedItems[0];
			ProcedureType conflictProc = (ProcedureType)lvi0.Tag;

			_conflictNominalID = GlobalVars.mainForm.viewControl1.DrawGeometry(conflictProc.NominalLine, _conflictNominalElem);
			_conflictPointID = GlobalVars.mainForm.viewControl1.DrawGeometry((Point)conflictProc.Tag, _conflictPointElem);
		}



	}
}
