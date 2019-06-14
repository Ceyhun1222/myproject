using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PDM;
using CDOTMA.Controls;
using CDOTMA.Geometries;
using CDOTMA.Drawings;
using CDOTMA.CoordinatSystems;

namespace CDOTMA
{
	public partial class MainForm : Form
	{
		int selectedIx;

		LegLayer lglay;
		LegPoint selectedPt;
		List<LegPoint> selectedPts;
		ViewerMode _mode;

		SelectLayersBox slb;

		DepartureRoutes departureRoutes;
		ArrivalRoutes arrivalRoutes;
		FrmEnroute enroute;

		Point CurrViewPt;
		NetTopologySuite.Geometries.Point CurrMapPt;

		public MainForm()
		{
			InitializeComponent();
			GlobalVars.InitCommand();

			this.tableLayoutPanel1.Controls.Remove(this.checkBox1);
			this.tableLayoutPanel1.Controls.Remove(this.checkBox2);
			this.tableLayoutPanel1.Controls.Remove(this.checkBox3);
			this.tableLayoutPanel1.Controls.Remove(this.checkBox4);
			this.tableLayoutPanel1.Controls.Remove(this.checkBox5);
			this.tableLayoutPanel1.Controls.Remove(this.checkBox6);
			this.tableLayoutPanel1.Controls.Remove(this.checkBox7);

			Converters.ConvertESRIvvsJTS.FromESRIToJTS.Initialize();

			slb = new SelectLayersBox();

			departureRoutes = new DepartureRoutes();
			arrivalRoutes = new ArrivalRoutes();
			selectedPts = new List<LegPoint>();
			enroute = new FrmEnroute();
			GlobalVars.mainForm = this;
		}

		void Process(double percents)
		{
			toolStripProgressBar1.Value = (int)percents;
		}

		private void checkBox1_Click(object sender, EventArgs e)
		{
			ILayer lyr = (ILayer)((CheckBox)sender).Tag;
			if (lyr == null)
				return;
			lyr.Visible = ((CheckBox)sender).Checked;
			viewControl1.DrawView();
		}

		#region viewControl

		private void viewControl1_MouseDown(object sender, MouseEventArgs e)
		{
			if (_mode != ViewerMode.Edit)
				return;

			CurrViewPt.X = e.X;
			CurrViewPt.Y = e.Y;
			CurrMapPt = viewControl1.Screen.ProjectToMap(e.X, e.Y);

			//=====================================================================================================;

			PointF pt = new PointF(e.X, e.Y);

			float dist = 25;
			selectedPt = null;
			selectedPts.Clear();
			selectedIx = -1;

			for (int i = 0; i < lglay.Count; i++)
			{
				LegPoint lpt = lglay[i];

				//if (lpt.legs.Count == 0)
				//    continue;

				PointF mpPt = viewControl1.Screen.ProjectToScr(lpt.pPtPrj.Coordinate);
				float dx = pt.X - mpPt.X;
				float dy = pt.Y - mpPt.Y;
				float dst = dx * dx + dy * dy;

				if (dst < dist)
				{
					dist = dst;
					selectedIx = i;
					selectedPt = lpt;
				}

				if (dst < 25)
					selectedPts.Add(lpt);
			}
		}

		private void viewControl1_MouseMove(object sender, MouseEventArgs e)
		{
			if (viewControl1.Screen.GetExtend().IsNull)
				return;

			CurrViewPt.X = e.X;
			CurrViewPt.Y = e.Y;
			CurrMapPt = viewControl1.Screen.ProjectToMap(e.X, e.Y);

			//=====================================================================================================;

			NetTopologySuite.Geometries.Point ptGeo = (NetTopologySuite.Geometries.Point)Functions.ToGeo(CurrMapPt);
			StatusLabelLat.Text = Functions.Degree2String(ptGeo.Y, Degree2StringMode.DMSLat);
			StatusLabelLong.Text = Functions.Degree2String(ptGeo.X, Degree2StringMode.DMSLon);

			if (selectedIx < 0)
				return;

			if (_mode != ViewerMode.Edit)
			{
				tsbNextExtend.Enabled = viewControl1.NextExtendIsPosible();
				tsbPrevExtend.Enabled = viewControl1.PrevExtendIsPosible();

				return;
			}

			if (e.Button != MouseButtons.Left)
				return;

			selectedPt.pPtPrj.X = CurrMapPt.X;
			selectedPt.pPtPrj.Y = CurrMapPt.Y;
			viewControl1.DrawView();
		}

		private void viewControl1_MouseUp(object sender, MouseEventArgs e)
		{
			selectedPt = null;
			selectedIx = -1;
		}

		private void viewControl1_ViewChanged(object sender, EventArgs e)
		{
			tsbNextExtend.Enabled = viewControl1.NextExtendIsPosible();
			tsbPrevExtend.Enabled = viewControl1.PrevExtendIsPosible();
		}

		private void viewControl1_ProjectionChanged(object sender, EventArgs e)
		{

		}

		#endregion

		#region toolStrip

		private void toolStrip1_ItemCheckedChanged(object sender, EventArgs e)
		{
			ToolStripButton CheckedItem = (ToolStripButton)sender;
			if (!CheckedItem.Checked)
			{
				//viewControl1.Mode = ViewerMode.None;	//(ViewerMode)command;
				//_mode = viewControl1.Mode;
				return;
			}

			int command;

			try
			{
				command = (int)CheckedItem.Tag;
			}
			catch
			{
				return;
			}

			foreach (ToolStripItem item in toolStrip1.Items)
				if (item != CheckedItem && item is ToolStripButton)
					((ToolStripButton)item).Checked = false;

			if ((ViewerMode)command >= ViewerMode.Super)
				return;

			viewControl1.Mode = (ViewerMode)command;
			_mode = viewControl1.Mode;

			switch (viewControl1.Mode)
			{
				case ViewerMode.None:
					tsbNONE.Checked = true;
					tsbHandFlat.Checked = false;
					tsbZoomIn.Checked = false;
					tsbZoomOut.Checked = false;
					tsbEdit.Checked = false;

					break;
				case ViewerMode.Pan:
					tsbNONE.Checked = false;
					tsbHandFlat.Checked = true;
					tsbZoomIn.Checked = false;
					tsbZoomOut.Checked = false;
					tsbEdit.Checked = false;

					break;
				case ViewerMode.ZoomIn:
					tsbNONE.Checked = false;
					tsbHandFlat.Checked = false;
					tsbZoomIn.Checked = true;
					tsbZoomOut.Checked = false;
					tsbEdit.Checked = false;

					break;
				case ViewerMode.ZoomOut:
					tsbNONE.Checked = false;
					tsbHandFlat.Checked = false;
					tsbZoomIn.Checked = false;
					tsbZoomOut.Checked = true;
					tsbEdit.Checked = false;

					break;
				case ViewerMode.Edit:
					tsbNONE.Checked = false;
					tsbHandFlat.Checked = false;
					tsbZoomIn.Checked = false;
					tsbZoomOut.Checked = false;
					tsbEdit.Checked = true;

					break;
			}
		}

		private void toolStrip1_ItemClicked(Object sender, ToolStripItemClickedEventArgs e)
		{
			int command;

			try
			{
				command = (int)e.ClickedItem.Tag;
			}
			catch
			{
				return;
			}

			switch (command)
			{
				case 20:
					viewControl1.PrevExtend();
					tsbNextExtend.Enabled = viewControl1.NextExtendIsPosible();
					tsbPrevExtend.Enabled = viewControl1.PrevExtendIsPosible();

					break;
				case 21:
					viewControl1.NextExtend();
					tsbNextExtend.Enabled = viewControl1.NextExtendIsPosible();
					tsbPrevExtend.Enabled = viewControl1.PrevExtendIsPosible();

					break;
			}
		}

		//=======================================================================
		int measuring = -1;
		bool msrDown;
		NetTopologySuite.Geometries.Point FromMPt;
		Point FromVPt;
		Point PrevVPt;
		Point CurrVPt;

		private void Measure_MouseDown(object sender, MouseEventArgs e)
		{
			FromVPt = CurrViewPt;
			FromMPt = CurrMapPt;
			msrDown = true;
		}

		private void Measure_MouseMove(object sender, MouseEventArgs e)
		{
			PrevVPt = CurrVPt;
			CurrVPt = CurrViewPt;

			if (msrDown)
			{
				NetTopologySuite.Geometries.Point CurrMPt = CurrMapPt;

				if (CurrVPt.X != FromVPt.X && CurrVPt.Y != FromVPt.Y)
				{
					double dX = CurrMPt.X - FromMPt.X;
					double dY = CurrMPt.Y - FromMPt.Y;

					if (measuring == 1)
					{
						double dist = Math.Sqrt(dX * dX + dY * dY);
						StatusLabelInfo.Text = dist.ToString("0.00") + " meters";
					}

					int oldRop;
					IntPtr oldpen;
					IntPtr hdc = XorDrawing.BeginDraw(viewControl1.Handle, out oldRop, out oldpen);

					XorDrawing.DrawXorLine(hdc, FromVPt.X, FromVPt.Y, PrevVPt.X, PrevVPt.Y);
					XorDrawing.DrawXorLine(hdc, FromVPt.X, FromVPt.Y, CurrVPt.X, CurrVPt.Y);

					XorDrawing.FinishDraw(viewControl1.Handle, hdc, oldRop, oldpen);
				}
			}
		}

		private void Measure_MouseUp(object sender, MouseEventArgs e)
		{
			msrDown = false;
			PrevVPt = CurrVPt;
			CurrVPt = CurrViewPt;

			//int oldRop;
			//IntPtr oldpen;

			//IntPtr hdc = XorDrawing.BeginDraw(viewControl1.Handle, out oldRop, out oldpen);
			//XorDrawing.DrawXorLine(hdc, FromVPt.X, FromVPt.Y, PrevVPt.X, PrevVPt.Y);
			//XorDrawing.FinishDraw(viewControl1.Handle, hdc, oldRop, oldpen);

			viewControl1.Refresh();
		}

		private void tsbMeasureDistance_CheckedChanged(object sender, EventArgs e)
		{
			if (tsbMeasureDistance.Checked)
			{
				foreach (ToolStripItem item in toolStrip1.Items)
					if (item != tsbMeasureDistance && item is ToolStripButton)
						((ToolStripButton)item).Checked = false;

				msrDown = false;
				measuring = 1;

				viewControl1.Mode = ViewerMode.None;
				viewControl1.MouseDown += Measure_MouseDown;
				viewControl1.MouseMove += Measure_MouseMove;
				viewControl1.MouseUp += Measure_MouseUp;

				viewControl1.Cursor = new Cursor(Properties.Resources.Measure.Handle);
			}
			else if (measuring == 1)
			{
				measuring -= 1;
				viewControl1.MouseDown -= Measure_MouseDown;
				viewControl1.MouseMove -= Measure_MouseMove;
				viewControl1.MouseUp -= Measure_MouseUp;
			}

		}

		//=======================================================================
		#endregion

		#region Menu

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
				return;

			if (slb.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
				return;

			this.Cursor = Cursors.WaitCursor;
			toolStripProgressBar1.Value = 0;
			toolStripProgressBar1.Visible = true;

			treeView1.Nodes.Clear();
			viewControl1.RemoveAllLayers();
			viewControl1.ClearAllDrawings();

			this.tableLayoutPanel1.Controls.Remove(this.checkBox1);
			this.tableLayoutPanel1.Controls.Remove(this.checkBox2);
			this.tableLayoutPanel1.Controls.Remove(this.checkBox3);
			this.tableLayoutPanel1.Controls.Remove(this.checkBox4);
			this.tableLayoutPanel1.Controls.Remove(this.checkBox5);
			this.tableLayoutPanel1.Controls.Remove(this.checkBox6);
			this.tableLayoutPanel1.Controls.Remove(this.checkBox7);
			int index = 0;

			saveFileDialog1.FileName = openFileDialog1.FileName;

			DBModule.SetDataFile(openFileDialog1.FileName);

			DBModule.FillLists(slb.datatypes, Process);

			Layer<NetTopologySuite.Geometries.Point> pointLayer;
			Layer<NetTopologySuite.Geometries.MultiLineString> polylineLayer;
			Layer<NetTopologySuite.Geometries.MultiPolygon> polygonLayer;

			PointElement ptElem;
			FillElement fillElem;

			//=====================================================================
			//tableLayoutPanel1.

			if (slb.LoadAirspaces)
			{
				TreeNode tNode = treeView1.Nodes.Add("Airspace");
				tNode.Nodes.Add("Volumes");

				polygonLayer = new Layer<NetTopologySuite.Geometries.MultiPolygon>("Airspace");
				polygonLayer.CS = GlobalVars.pSpRefGeo;
				polygonLayer.ViewCS = GlobalVars.pSpRefPrj;

				for (int i = 0; i < GlobalVars.AstList.Count; i++)
					for (int j = 0; j < GlobalVars.AstList[i].AsVT.Count; j++)
						polygonLayer.Add(GlobalVars.AstList[i].AsVT[j].geometryGeo);							//geometryPrj

				fillElem = new FillElement();
				fillElem.Color = Functions.RGB(0, 128, 255);
				fillElem.Style = 45;

				polygonLayer.Visible = checkBox2.Checked;
				polygonLayer.Element = fillElem;
				viewControl1.AddLayer((ILayer)polygonLayer);
				checkBox2.Tag = polygonLayer;
				this.tableLayoutPanel1.Controls.Add(this.checkBox2, 0, index++);
			}

			//=====================================================================

			if (slb.LoadAirportHeliports)
			{
				pointLayer = new Layer<NetTopologySuite.Geometries.Point>("Aeroport");
				pointLayer.CS = GlobalVars.pSpRefGeo;
				pointLayer.ViewCS = GlobalVars.pSpRefPrj;

				for (int i = 0; i < GlobalVars.ADHPList.Count; i++)
				{
					NetTopologySuite.Geometries.Point pt = GlobalVars.ADHPList[i].pPtGeo;							// pPtPrj
					pt.SetName(GlobalVars.ADHPList[i].ToString());
					pointLayer.Add(pt);
				}

				ptElem = new PointElement();
				ptElem.Size = 6;
				ptElem.Color = 255;

				pointLayer.Visible = checkBox1.Checked;
				pointLayer.Element = ptElem;

				viewControl1.AddLayer((ILayer)pointLayer);
				checkBox1.Tag = pointLayer;
				this.tableLayoutPanel1.Controls.Add(this.checkBox1, 0, index++);
			}

			//=====================================================================

			if (slb.LoadIAPs)
			{
				TreeNode tNode = treeView1.Nodes.Add("IAP");
				tNode = tNode.Nodes.Add("Transitions");
				tNode = tNode.Nodes.Add("Legs");
				tNode.Nodes.Add("StartPoint");
				tNode.Nodes.Add("EndPoint");

				polylineLayer = new Layer<NetTopologySuite.Geometries.MultiLineString>("ApproachLegs");
				polylineLayer.CS = GlobalVars.pSpRefGeo;
				polylineLayer.ViewCS = GlobalVars.pSpRefPrj;

				for (int i = 0; i < GlobalVars.ApproachProcedures.Count; i++)
					for (int j = 0; j < GlobalVars.ApproachProcedures[i].procTransitions.Count; j++)
						for (int k = 0; k < GlobalVars.ApproachProcedures[i].procTransitions[j].procLegs.Count; k++)
							polylineLayer.Add(GlobalVars.ApproachProcedures[i].procTransitions[j].procLegs[k].PathGeomGeo);	//PathGeomPrj

				fillElem = new FillElement();
				fillElem.Color = Functions.RGB(0, 0, 148);
				fillElem.Style = 0;

				polylineLayer.Visible = checkBox3.Checked;
				polylineLayer.Element = fillElem;
				viewControl1.AddLayer((ILayer)polylineLayer);
				checkBox3.Tag = polylineLayer;
				this.tableLayoutPanel1.Controls.Add(this.checkBox3, 0, index++);
			}

			//=====================================================================

			if (slb.LoadSTARs)
			{
				TreeNode tNode = treeView1.Nodes.Add("STAR");
				tNode = tNode.Nodes.Add("Transitions");
				tNode = tNode.Nodes.Add("Legs");
				tNode.Nodes.Add("StartPoint");
				tNode.Nodes.Add("EndPoint");

				polylineLayer = new Layer<NetTopologySuite.Geometries.MultiLineString>("ArrivalLegs");
				polylineLayer.CS = GlobalVars.pSpRefGeo;
				polylineLayer.ViewCS = GlobalVars.pSpRefPrj;

				for (int i = 0; i < GlobalVars.ArrivalProcedures.Count; i++)
					for (int j = 0; j < GlobalVars.ArrivalProcedures[i].procTransitions.Count; j++)
						for (int k = 0; k < GlobalVars.ArrivalProcedures[i].procTransitions[j].procLegs.Count; k++)
							polylineLayer.Add(GlobalVars.ArrivalProcedures[i].procTransitions[j].procLegs[k].PathGeomGeo);	//PathGeomPrj

				fillElem = new FillElement();
				fillElem.Color = Functions.RGB(0, 155, 0);
				fillElem.Style = 0;

				polylineLayer.Visible = checkBox4.Checked;
				polylineLayer.Element = fillElem;
				viewControl1.AddLayer((ILayer)polylineLayer);
				checkBox4.Tag = polylineLayer;
				this.tableLayoutPanel1.Controls.Add(this.checkBox4, 0, index++);
			}

			//=====================================================================

			if (slb.LoadSIDs)
			{
				TreeNode tNode = treeView1.Nodes.Add("SID");
				tNode = tNode.Nodes.Add("Transitions");
				tNode = tNode.Nodes.Add("Legs");
				tNode.Nodes.Add("StartPoint");
				tNode.Nodes.Add("EndPoint");

				polylineLayer = new Layer<NetTopologySuite.Geometries.MultiLineString>("DepartureLegs");
				polylineLayer.CS = GlobalVars.pSpRefGeo;
				polylineLayer.ViewCS = GlobalVars.pSpRefPrj;

				for (int i = 0; i < GlobalVars.DepartureProcedures.Count; i++)
					for (int j = 0; j < GlobalVars.DepartureProcedures[i].procTransitions.Count; j++)
						for (int k = 0; k < GlobalVars.DepartureProcedures[i].procTransitions[j].procLegs.Count; k++)
							polylineLayer.Add(GlobalVars.DepartureProcedures[i].procTransitions[j].procLegs[k].PathGeomGeo);	//PathGeomPrj

				fillElem = new FillElement();
				fillElem.Color = Functions.RGB(222, 188, 188);
				fillElem.Style = 0;

				polylineLayer.Visible = checkBox5.Checked;
				polylineLayer.Element = fillElem;
				viewControl1.AddLayer((ILayer)polylineLayer);
				checkBox5.Tag = polylineLayer;
				this.tableLayoutPanel1.Controls.Add(this.checkBox5, 0, index++);
			}

			//=====================================================================

			if (slb.LoadRouts)
			{
				TreeNode tNode = treeView1.Nodes.Add("Rout");
				tNode = tNode.Nodes.Add("");
				tNode = tNode.Nodes.Add("Legs");
				tNode.Nodes.Add("StartPoint");
				tNode.Nodes.Add("EndPoint");

				polylineLayer = new Layer<NetTopologySuite.Geometries.MultiLineString>("RoutLegs");
				polylineLayer.CS = GlobalVars.pSpRefGeo;
				polylineLayer.ViewCS = GlobalVars.pSpRefPrj;

				for (int i = 0; i < GlobalVars.Routs.Count; i++)
					for (int j = 0; j < GlobalVars.Routs[i].procTransitions.Count; j++)
						for (int k = 0; k < GlobalVars.Routs[i].procTransitions[j].procLegs.Count; k++)
							polylineLayer.Add(GlobalVars.Routs[i].procTransitions[j].procLegs[k].PathGeomGeo);	//PathGeomPrj

				fillElem = new FillElement();
				fillElem.Color = Functions.RGB(148, 148, 148);
				fillElem.Style = 0;

				polylineLayer.Visible = checkBox6.Checked;
				polylineLayer.Element = fillElem;
				viewControl1.AddLayer((ILayer)polylineLayer);
				checkBox6.Tag = polylineLayer;
				this.tableLayoutPanel1.Controls.Add(this.checkBox6, 0, index++);
			}

			//=====================================================================

			pointLayer = new Layer<NetTopologySuite.Geometries.Point>("WPT");
			pointLayer.CS = GlobalVars.pSpRefGeo;
			pointLayer.ViewCS = GlobalVars.pSpRefPrj;

			for (int i = 0; i < GlobalVars.WPTList.Count; i++)
			{
				NetTopologySuite.Geometries.Point pt = GlobalVars.WPTList[i].pPtGeo;		//pPtPrj
				pt.SetName(GlobalVars.WPTList[i].ToString());
				pointLayer.Add(pt);
			}

			//for (int i = 0; i < GlobalVars.LegPoints.Count; i++)
			//{
			//    NetTopologySuite.Geometries.Point pt = GlobalVars.LegPoints[i].pPtPrj;
			//    pt.SetName(GlobalVars.LegPoints[i].ToString());
			//    pointLayer.Add(pt);
			//}

			ptElem = new PointElement();
			ptElem.Size = 6;
			ptElem.Color = Functions.RGB(0, 128, 128);
			pointLayer.Visible = checkBox7.Checked;
			pointLayer.Element = ptElem;
			viewControl1.AddLayer((ILayer)pointLayer);
			checkBox7.Tag = pointLayer;
			this.tableLayoutPanel1.Controls.Add(this.checkBox7, 0, index++);

			//=====================================================================

			//LegLayer<TraceLeg> lglay = new LegLayer<TraceLeg>("Model");
			lglay = new LegLayer("Model");
			lglay.CS = GlobalVars.pSpRefGeo;
			lglay.ViewCS = GlobalVars.pSpRefPrj;

			fillElem = new FillElement();
			fillElem.Color = Functions.RGB(230, 230, 0);
			fillElem.Style = 0;

			lglay.Visible = true;
			lglay.Element = fillElem;

			for (int i = 0; i < GlobalVars.LegPoints.Count; i++)
				if (GlobalVars.LegPoints[i].legs.Count > 0)
					lglay.Add(GlobalVars.LegPoints[i]);

			viewControl1.AddLayer((ILayer)lglay);

			//=====================================================================

			viewControl1.ResetView();

			toolStripProgressBar1.Visible = false;
			this.Cursor = Cursors.Default;
		}

		private void closeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			viewControl1.RemoveAllLayers();
			viewControl1.ResetView();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void viewProjectionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CoordinatSystem newCoordinatSystem;

			if (ProjectionDlg.ShowDialog(out newCoordinatSystem, GlobalVars.pSpRefPrj) == System.Windows.Forms.DialogResult.OK)
			{
				GlobalVars.pSpRefPrj = newCoordinatSystem;

				viewControl1.SetViewProjection(newCoordinatSystem);

				DBModule.SaveViewPrjFile(newCoordinatSystem);
				DBModule.PackFileToData("view.prj", DBModule.FileName);
			}
		}

		private void unitSettingsMenuItem_Click(object sender, EventArgs e)
		{
			Settings newSettings;
			if (SettingsDlg.ShowDialog(out newSettings, GlobalVars.settings) == System.Windows.Forms.DialogResult.OK)
			{
				GlobalVars.settings = newSettings;
				DBModule.SaveSettings(newSettings);
				DBModule.PackFileToData("DisplaySettings.cfg", DBModule.FileName);
			}
		}

		private void intersectingTracksWithCommonPointToolStripMenuItem_Click(object sender, EventArgs e)
		{
			WithCommonPoint withCommonPoint = new WithCommonPoint();
			withCommonPoint.ShowForm(this);
		}

		private void intersectingTracksWithoutCommonPointToolStripMenuItem_Click(object sender, EventArgs e)
		{
			WithoutCommonPoint withoutCommonPoint = new WithoutCommonPoint();
			withoutCommonPoint.ShowForm(this);
		}

		private void nonintersectingSIDAndSTARToolStripMenuItem_Click(object sender, EventArgs e)
		{
			NonInterSIDAndSTAR nonInterSIDAndSTAR = new NonInterSIDAndSTAR();
			nonInterSIDAndSTAR.ShowForm(this);
		}

		private void departureRoutesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			departureRoutes.Show();
		}

		private void arrivalRoutesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			arrivalRoutes.Show();
		}

		private void enrouteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			enroute.Show();
		}

		private void lateralSeparationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LateralSeparationSettings frm = new LateralSeparationSettings();
			frm.ShowDialog(this);
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AboutForm about = new AboutForm();
			about.ShowDialog(this);
			about = null;
		}
		#endregion
	}
}
