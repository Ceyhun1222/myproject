using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Aran.Aim.Data.Filters;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.AranEnvironment.Symbols;
using Aran.Converters;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.PANDA.Common;
using Aran.PANDA.RNAV.Enroute.VD.Properties;

namespace Aran.PANDA.RNAV.Enroute.VD
{
	public partial class MainForm : Form
	{
		const double EnRouteIAS = 585.0 * 0.277777777777777777777777;
		const double DVORMaxRange = 75.0 * 1852.0;
		private double[] EnrouteMOCValues = { 300.0, 450.0, 600.0 };

		private int CurrPage
		{
			set
			{
				tabControl1.SelectedIndex = value;

				PrevBtn.Enabled = value > 0;
				NextBtn.Enabled = value < tabControl1.TabPages.Count - 1;
				OkBtn.Enabled = value == tabControl1.TabPages.Count - 1;

				int n = Label1.Length;
				for (int i = 0; i < n; i++)
				{
					//Label1[i].Visible = tabControl1.TabPages[i].Visible;
					Label1[i].ForeColor = System.Drawing.Color.FromArgb(0XC0C0C0);
					Label1[i].Font = new System.Drawing.Font(Label1[i].Font, System.Drawing.FontStyle.Regular);
				}

				Label1[value].ForeColor = System.Drawing.Color.FromArgb(0XFF8000);
				Label1[value].Font = new System.Drawing.Font(Label1[value].Font, System.Drawing.FontStyle.Bold);

				Text = Resources.str00030 + "   [" + Label1[value].Text + "]";
				//this.HelpContextID = 4000 + 100 * (tabControl1.SelectedIndex + 1);
			}

			get { return tabControl1.SelectedIndex; }
		}

		#region Variable declarations

		//private int HelpContextID;
		//private bool bFormInitialised = false;
		private ReportsForm _reportForm;
		private FIXInfoForm _FIXInfoForm;
		Label[] Label1;
		#endregion

		#region Page I variables

		private Procedure _selectedProc;
		private List<Segment> _allRouteLegs;
		private List<Segment> _currProcLegs;

		private Segment _firstRouteLeg;
		private CodeDirection _segDir;

		private int _currSegElem;
		private double _currDir;
		//private double _TrueTrack;
		//private double _maxDistance;
		//private double _minAltitude;
		private double _maxAltitude;

		#endregion

		#region Page II variables

		private int _allSegElem;
		private int _startNavRangeElem;
		private int _endNavRangeElem;
		private int _currIndex;

		private double _moc;
		private double _TotalLen;
		private List<Segment> _legs;

		private Segment _currRouteLeg;

		private MultiPolygon _startCoverageRangePoly;
		private MultiPolygon _endCoverageRangePoly;

		#endregion

		#region Page III variables

		#endregion

		#region Form

		public MainForm()
		{
			InitializeComponent();

			label103.Text = GlobalVars.unitConverter.DistanceUnit;
			label110.Text = GlobalVars.unitConverter.HeightUnit;

			label204.Text = GlobalVars.unitConverter.DistanceUnit;
			label206.Text = GlobalVars.unitConverter.DistanceUnit;
			label208.Text = GlobalVars.unitConverter.HeightUnit;

			label220.Text = GlobalVars.unitConverter.DistanceUnit;
			label224.Text = GlobalVars.unitConverter.HeightUnit;

			//Common ==========================================================================

			Label01.Text = Resources.str01000;
			Label02.Text = Resources.str01001;
			Label03.Text = Resources.str01002;

			Label1 = new Label[] { Label01, Label02, Label03 };

			int i, n = Label1.Length;
			for (i = 0; i < n; i++)
				tabControl1.TabPages[i].Text = Label1[i].Text;

			PrevBtn.Text = Resources.str01003;
			NextBtn.Text = Resources.str01004;
			ReportBtn.Text = Resources.str01005;
			OkBtn.Text = Resources.str01006;
			CancelBtn.Text = Resources.str01007;

			//Fill Page I ==========================================================================
			label101.Text = Resources.str01101;
			label102.Text = Resources.str01102;
			label104.Text = Resources.str01103;
			label105.Text = Resources.str01104;
			label106.Text = Resources.str01106;
			label107.Text = Resources.str01107;
			label111.Text = Resources.str01111;
			label109.Text = Resources.str01108;

			//Fill Page II =========================================================================
			label201.Text = Resources.str01201;

			label202.Text = Resources.str01202;
			label203.Text = Resources.str01203;
			label205.Text = Resources.str01102;
			label207.Text = Resources.str01212;

			label217.Text = Resources.str01210;
			label218.Text = Resources.str01202;
			label219.Text = Resources.str01203;
			label221.Text = Resources.str01107;
			label223.Text = Resources.str01108;

			//Fill Page III =========================================================================
			dataGridViewTextBoxColumn1.HeaderText = Resources.str02010;
			dataGridViewTextBoxColumn2.HeaderText = Resources.str02011;
			dataGridViewTextBoxColumn3.HeaderText = Resources.str02012;
			dataGridViewTextBoxColumn4.HeaderText = Resources.str02013;
			dataGridViewTextBoxColumn5.HeaderText = Resources.str02014 + " (°)";
			dataGridViewTextBoxColumn6.HeaderText = Resources.str02015;
			dataGridViewTextBoxColumn7.HeaderText = Resources.str02016 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";
			dataGridViewTextBoxColumn8.HeaderText = Resources.str02017 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridViewTextBoxColumn9.HeaderText = Resources.str02023 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridViewTextBoxColumn10.HeaderText = Resources.str02024 + " (" + GlobalVars.unitConverter.HeightUnit + ")";

			//==========================================================================
			comboBox103.Items.AddRange(new object[] { CodeDirection.FORWARD, CodeDirection.BACKWARD }); //, CodeDirection.BOTH

			_allSegElem = -1;
			_currSegElem = -1;
			_startNavRangeElem = -1;
			_endNavRangeElem = -1;
			_allRouteLegs = new List<Segment>();

			// Init Page I ============================================================================

			this.CurrPage = 0;
			_reportForm = new ReportsForm();
			_reportForm.Init(ReportBtn);
			_FIXInfoForm = new FIXInfoForm();

			if (GlobalVars.gAranEnv.DbProvider is Aran.Aim.Data.DbProvider)
				FillComboBox101_Aim();
			else
				FillComboBox101();

			if (comboBox101.Items.Count < 1)
				throw new Exception("There must at least one En-Route procedure in Your database.");		//, null, MessageBoxButtons.OK, MessageBoxIcon.Asterisk

			PrepareLegList();

			comboBox101.SelectedIndex = 0;
			comboBox103.SelectedIndex = 0;

			// Init Page II ===========================================================================
			comboBox203.Items.Clear();
			n = EnrouteMOCValues.Length;
			for (i = 0; i < n; i++)
				comboBox203.Items.Add(GlobalVars.unitConverter.HeightToDisplayUnits(EnrouteMOCValues[i], eRoundMode.SPECIAL_NEAREST));
			comboBox203.SelectedIndex = 0;

			_legs = new List<Segment>();

			//==========================================================================
			int hang = tabControl1.Size.Height + 3 - tabControl1.TabPages[0].Size.Height - ((this.Width - this.ClientSize.Width) >> 1);
			int dd = hang - tabControl1.Top;

			//hang = tabControl1.ItemSize.Height + ((this.Width - this.ClientSize.Width) >> 1);
			//Frame02.Top = -hang;

			tabControl1.Top = -hang;
			ShowPanelBtn.Top = -hang;

			Frame01.Top -= dd;
			this.Height -= dd;

			//this.bFormInitialised = true;

			if (ShowPanelBtn.Checked)
				ShowPanelBtn.Checked = false;
			else
				ShowPanelBtn_CheckedChanged(ShowPanelBtn, null);
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_currSegElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_allSegElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_startNavRangeElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_endNavRangeElem);

			_currRouteLeg.DeleteGraphics();

			foreach (Segment sg in _legs)
				sg.DeleteGraphics();

			DBModule.CloseDB();
			_reportForm.Close();
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			// Get a handle to a copy of this form's system (window) menu
			IntPtr hSysMenu = Utility.GetSystemMenu(this.Handle, false);
			// Add a separator
			Utility.AppendMenu(hSysMenu, GlobalVars.MF_SEPARATOR, 0, string.Empty);
			// Add the About menu item
			Utility.AppendMenu(hSysMenu, GlobalVars.MF_STRING, GlobalVars.SYSMENU_ABOUT_ID, "&About…");
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			if ((m.Msg == GlobalVars.WM_SYSCOMMAND) && ((int)m.WParam == GlobalVars.SYSMENU_ABOUT_ID))
			{
				AboutForm about = new AboutForm();
				about.ShowDialog(this);
				about = null;
			}
		}

		private static int CompareRouteByName(Route r1, Route r2)
		{
			if (r1 == null)
			{
				if (r2 == null)
					return 0;

				return -1;
			}
			else if (r2 == null)
				return 1;

			if (r1.Name == null)
			{
				if (r2.Name == null)
					return 0;

				return -1;
			}
			else if (r2.Name == null)
				return 1;

			return r1.Name.CompareTo(r2.Name);
		}

		private void FillComboBox101_Aim()
		{
			List<Route> pProcedureList = DBModule.pObjectDir.GetRouteList(Guid.Empty);      //GlobalVars.CurrADHP.OrgID
			pProcedureList.Sort(CompareRouteByName);

			ComparisonOps cmpOps = new ComparisonOps(ComparisonOpType.EqualTo, "RouteFormed");
			OperationChoice opChoice = new OperationChoice(cmpOps);
			Aim.Data.Filters.Filter filters = new Aim.Data.Filters.Filter(opChoice);
			_allRouteLegs.Clear();

			foreach (Route rt in pProcedureList)
			{
				try
				{
					Procedure prc = new Procedure { pFeature = rt };
					//Procedure prc;				prc.pFeature = rt;
					//===========================================================+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

					cmpOps.Value = prc.pFeature.Identifier;
					opChoice.ComparisonOps = cmpOps;
					filters.Operation = opChoice;

					//List<RouteSegment> pSegmentLegList = (List<RouteSegment>)DBModule.pObjectDir.GetFeatureList(Aim.FeatureType.RouteSegment, filters);
					var tmpRouteList = DBModule.pObjectDir.GetFeatureList(Aim.FeatureType.RouteSegment, filters);
					if (tmpRouteList == null) continue;

					List<RouteSegment> pSegmentLegList = tmpRouteList.Cast<RouteSegment>().ToList();
					if (pSegmentLegList == null || pSegmentLegList.Count == 0)
						continue;

					int n = _allRouteLegs.Count;

					foreach (RouteSegment rs in pSegmentLegList)
					{
						try
						{
							if (rs.Start == null || rs.End == null)
								continue;

							if (rs.Start.PointChoice == null || rs.End.PointChoice == null)
								continue;

							Segment sgm = new Segment();
							//sgm.PBNType = ePBNClass.RNAV5;

							sgm.proc = prc.pFeature;
							sgm.pFeature = rs;

							sgm.Start = new FIX(GlobalVars.gAranEnv);
							sgm.Start.FlightPhase = eFlightPhase.Enroute;
							sgm.Start.PBNType = ePBNClass.RNAV5;
							sgm.Start.SensorType = eSensorType.VOR_DME;
							sgm.Start.IAS = EnRouteIAS;
							sgm.Start.Visible = false;

							sgm.End = new FIX(GlobalVars.gAranEnv);
							sgm.End.FlightPhase = eFlightPhase.Enroute;
							sgm.End.PBNType = ePBNClass.RNAV5;
							sgm.End.SensorType = eSensorType.VOR_DME;
							sgm.End.IAS = EnRouteIAS;
							sgm.End.Visible = false;

							if (rs.Start.PointChoice.Choice == Aim.SignificantPointChoice.DesignatedPoint)
							{
								Guid dptGuid = rs.Start.PointChoice.FixDesignatedPoint.Identifier;
								DesignatedPoint dpt = (DesignatedPoint)DBModule.pObjectDir.GetFeature(Aim.FeatureType.DesignatedPoint, dptGuid); //segPt.PointChoice.FixDesignatedPoint.Identifier;
								if (dpt == null)
									continue;
								if (dpt.Location == null)
									continue;
								if (dpt.Location.Geo == null)
									continue;

								Geometries.Point pPtGeo = new Geometries.Point(dpt.Location.Geo.X, dpt.Location.Geo.Y, dpt.Location.Geo.Z);

								sgm.Start.Id = dptGuid;
								sgm.Start.PrjPt = (Point)GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);
								sgm.Start.Name = dpt.Designator;
							}
							else if (rs.Start.PointChoice.Choice == Aim.SignificantPointChoice.Navaid)
							{
								Guid dptGuid = rs.Start.PointChoice.NavaidSystem.Identifier;
								Navaid dpt = (Navaid)DBModule.pObjectDir.GetFeature(Aim.FeatureType.Navaid, dptGuid);                               //segPt.PointChoice.FixDesignatedPoint.Identifier;
								if (dpt == null)
									continue;
								if (dpt.Location == null)
									continue;
								if (dpt.Location.Geo == null)
									continue;

								Geometries.Point pPtGeo = new Geometries.Point(dpt.Location.Geo.X, dpt.Location.Geo.Y, dpt.Location.Geo.Z);

								sgm.Start.Id = dptGuid;
								sgm.Start.PrjPt = (Point)GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);
								sgm.Start.Name = dpt.Designator;
							}
							else
								continue;

							if (rs.End.PointChoice.Choice == Aim.SignificantPointChoice.DesignatedPoint)
							{
								Guid dptGuid = rs.End.PointChoice.FixDesignatedPoint.Identifier;
								DesignatedPoint dpt = DBModule.pObjectDir.GetFeature(Aim.FeatureType.DesignatedPoint, dptGuid) as DesignatedPoint;  //segPt.PointChoice.FixDesignatedPoint.Identifier;
								if (dpt == null)
									continue;
								if (dpt.Location == null)
									continue;
								if (dpt.Location.Geo == null)
									continue;

								Geometries.Point pPtGeo = new Geometries.Point(dpt.Location.Geo.X, dpt.Location.Geo.Y, dpt.Location.Geo.Z);

								sgm.End.Id = dptGuid;
								sgm.End.PrjPt = (Point)GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);
								sgm.End.Name = dpt.Designator;
							}
							else if (rs.End.PointChoice.Choice == Aim.SignificantPointChoice.Navaid)
							{
								Guid dptGuid = rs.End.PointChoice.NavaidSystem.Identifier;
								Navaid dpt = (Navaid)DBModule.pObjectDir.GetFeature(Aim.FeatureType.Navaid, dptGuid);                               //segPt.PointChoice.FixDesignatedPoint.Identifier;
								if (dpt == null)
									continue;
								if (dpt.Location == null)
									continue;
								if (dpt.Location.Geo == null)
									continue;

								Geometries.Point pPtGeo = new Geometries.Point(dpt.Location.Geo.X, dpt.Location.Geo.Y, dpt.Location.Geo.Z);

								sgm.End.Id = dptGuid;
								sgm.End.PrjPt = (Geometries.Point)GlobalVars.pspatialReferenceOperation.ToPrj<Geometries.Point>(pPtGeo);
								sgm.End.Name = dpt.Designator;
							}
							else
								continue;

							//sgm.leg = new EnRouteLeg(Start ,End, CodeDirection.BOTH,  GlobalVars.gAranEnv);

							_allRouteLegs.Add(sgm);
						}
						catch (Exception)
						{
							throw;
						}
					}

					//===========================================================+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

					if (n < _allRouteLegs.Count)
						comboBox101.Items.Add(prc);

				}
				catch (Exception)
				{
					throw;
				}
			}
		}

		private void FillComboBox101()
		{
			List<Route> pProcedureList = DBModule.pObjectDir.GetRouteList(Guid.Empty);

			pProcedureList.Sort(CompareRouteByName);

			var tmpRouteSegmentList = DBModule.pObjectDir.GetFeatureList(Aim.FeatureType.RouteSegment).Cast<RouteSegment>().ToList();
			//var designatedPtList = DBModule.pObjectDir.GetFeatureList(Aim.FeatureType.DesignatedPoint);
			//var navaidList = DBModule.pObjectDir.GetFeatureList(Aim.FeatureType.DesignatedPoint);

			Func<EnRouteSegmentPoint, FIX> getFix = (segmentPt) =>
			{
				try
				{
					if (segmentPt == null) return null;

					var featType = Aim.FeatureType.DesignatedPoint;
					var identifer = new Guid();
					if (segmentPt.PointChoice.Choice == Aim.SignificantPointChoice.DesignatedPoint)
					{
						if (segmentPt.PointChoice.FixDesignatedPoint.Identifier != null)
							identifer = (Guid)segmentPt.PointChoice.FixDesignatedPoint.Identifier;
					}
					else if (segmentPt.PointChoice.Choice == Aim.SignificantPointChoice.Navaid)
					{
						if (segmentPt.PointChoice.NavaidSystem.Identifier != null)
							identifer = (Guid)segmentPt.PointChoice.NavaidSystem.Identifier;
						featType = Aim.FeatureType.Navaid;
					}

					dynamic feat = (dynamic)DBModule.pObjectDir.GetFeature(featType, identifer);
					var geo = feat.Location.Geo;

					var result = new FIX(GlobalVars.gAranEnv);
					var pPtGeo = new Aran.Geometries.Point(geo.X, geo.Y, geo.Z);

					result.FlightPhase = eFlightPhase.Enroute;
					result.PBNType = ePBNClass.RNAV5;
					result.SensorType = eSensorType.VOR_DME;

					result.Id = identifer;
					result.PrjPt = (Geometries.Point)GlobalVars.pspatialReferenceOperation.ToPrj<Geometries.Point>(pPtGeo);
					result.Name = feat.Designator;
					result.Visible = false;

					return result;
				}
				catch (Exception)
				{
					return null;
				}
			};

			var segment = (from routeSegment in tmpRouteSegmentList
						   join route in pProcedureList on routeSegment.RouteFormed.Identifier equals route.Identifier
						   where routeSegment.Start != null && routeSegment.End != null
						   select new Segment
						   {
							   proc = route,
							   pFeature = routeSegment,
							   Start = getFix(routeSegment.Start),
							   End = getFix(routeSegment.End),
						   }).ToList<Segment>();

			_allRouteLegs.Clear();
			_allRouteLegs.AddRange(segment);

			pProcedureList.ForEach(pro =>
			{
				if (_allRouteLegs.Count(tmpSegment => tmpSegment.pFeature.RouteFormed.Identifier == pro.Identifier) > 0)
					comboBox101.Items.Add(new Procedure { pFeature = pro });
			});
		}

		private void PrepareLegList()
		{
			List<Segment> routeLegs = new List<Segment>();

			foreach (Procedure prc in comboBox101.Items)
			{
				List<Segment> procLegs = _allRouteLegs.FindAll(
					delegate (Segment sg)
					{ return sg.proc == prc.pFeature; }
					);


				int i, j;

				int startIx = 0;
				for (i = 0; i < procLegs.Count; i++)
				{
					bool found = false;
					Segment seg0 = procLegs[i];
					for (j = 0; j < procLegs.Count; j++)
					{
						if (j == i)
							continue;

						Segment seg1 = procLegs[j];
						if (seg0.Start.Id == seg1.End.Id)
						{
							found = true;
							break;
						}
					}

					if (!found)
					{
						startIx = i;
						break;
					}
				}

				if (startIx != 0)
				{
					Segment seg0 = procLegs[0];
					Segment seg1 = procLegs[startIx];
					procLegs[0] = seg1;
					procLegs[startIx] = seg0;
				}

				for (i = 0; i < procLegs.Count - 1; i++)
				{
					j = i + 1;

					Segment seg0 = procLegs[i];
					Segment seg1 = procLegs[j];

					if (seg0.End.Id == seg1.Start.Id)
						continue;

					for (j++; j < procLegs.Count; j++)
					{
						seg1 = procLegs[j];

						if (seg0.End.Id == seg1.Start.Id)
						{
							procLegs[j] = procLegs[i + 1];
							procLegs[i + 1] = seg1;

							break;
						}
					}
				}

				routeLegs.AddRange(procLegs);
			}

			_allRouteLegs.Clear();
			_allRouteLegs.AddRange(routeLegs);
		}

		#endregion

		#region Common Form Events
		private void TextBoxes_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			//
		}

		private void ShowPanelBtn_CheckedChanged(object sender, EventArgs e)
		{
			//if (!bFormInitialised)
			//	return;

			if (ShowPanelBtn.Checked)
			{
				this.Width = Frame02.Left + Frame02.Width + 6;      // FontResizeFactorProvider.Scale(3);
				ShowPanelBtn.Image = Resources.HIDE_INFO;
			}
			else
			{
				this.Width = Frame02.Left + 16;
				ShowPanelBtn.Image = Resources.SHOW_INFO;
			}

			if (NextBtn.Enabled)
				NextBtn.Focus();
			else
				PrevBtn.Focus();
		}

		private void HelpBtn_Click(object sender, EventArgs e)
		{

		}

		private void PrevBtn_Click(object sender, EventArgs e)
		{
			switch (tabControl1.SelectedIndex)
			{
				case 1:
					BackToPageI();
					break;
				case 2:
					BackToPageII();
					break;
			}

			this.CurrPage = tabControl1.SelectedIndex - 1;
			NativeMethods.HidePandaBox();
		}

		private void NextBtn_Click(object sender, EventArgs e)
		{
			NativeMethods.ShowPandaBox(this.Handle.ToInt32());

			switch (tabControl1.SelectedIndex)
			{
				case 0:
					AddvanceToPageII();
					break;
				case 1:
					AddvanceToPageIII();
					break;
			}

			this.CurrPage = tabControl1.SelectedIndex + 1;
			if (this.CurrPage == 1)
				NextBtn.Enabled = false;

			NativeMethods.HidePandaBox();
		}

		private void ReportBtn_CheckedChanged(object sender, EventArgs e)
		{
			if (ReportBtn.Checked)
				_reportForm.Show(GlobalVars.Win32Window);
			else
				_reportForm.Hide();
		}

		private void OkBtn_Click(object sender, EventArgs e)
		{
			string RepFileName, RepFileTitle;

			if (!Utility.ShowSaveDialog(out RepFileName, out RepFileTitle))
				return;

			string sProcName = _firstRouteLeg.proc.Name;

			ReportHeader pReport = default(ReportHeader);
			pReport.Procedure = sProcName;
			pReport.Database = GlobalVars.gAranEnv.ConnectionInfo.Database;

			//pReport.Aerodrome = GlobalVars.CurrADHP.Name;

			SaveObstacleReports(RepFileName, RepFileTitle, pReport);

			//SaveRoutsLog(RepFileName, RepFileTitle, pReport);
			//ReportPoint[] GuidPoints;
			//ConvertTracToPoints(out GuidPoints);

			CReportFile.SaveGeometry(RepFileName, RepFileTitle, pReport, _legs, _TotalLen);

			////_reportForm.WriteTab(RepFileName, sProcName);
			//if (SaveProcedure(pReport))

			this.Close();
		}

		private void CancelBtn_Click(object sender, EventArgs e)
		{
			Close();
		}

		#endregion

		#region Page I

		private void comboBox101_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox101.SelectedIndex < 0)
				return;

			comboBox104.Items.Clear();

			_selectedProc = (Procedure)comboBox101.SelectedItem;

			_currProcLegs = _allRouteLegs.FindAll(
				delegate (Segment sg)
				{ return sg.proc == _selectedProc.pFeature; }
				);

			foreach (var sgm in _currProcLegs)
				comboBox104.Items.Add(sgm);

			if (comboBox104.Items.Count > 0)
				comboBox104.SelectedIndex = 0;
		}

		private void comboBox102_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox102.SelectedIndex < 0)
				_segDir = CodeDirection.BOTH;
			else
				_segDir = (CodeDirection)comboBox102.SelectedItem;      // _currRouteLeg.pFeature.Availability[0].Direction;

			//textBox102.Text = _currRouteLeg.pFeature.Availability[0].Direction.ToString();

			comboBox103.Enabled = _segDir == CodeDirection.BOTH;

			if (_segDir != CodeDirection.BOTH)
			{
				if (comboBox103.SelectedItem == null || (CodeDirection)comboBox103.SelectedItem != _segDir)
					comboBox103.SelectedItem = _segDir;
				else
					comboBox103_SelectedIndexChanged(comboBox103, null);
			}
			else if (comboBox103.SelectedItem == null)
				comboBox103.SelectedIndex = 0;
			else
				comboBox103_SelectedIndexChanged(comboBox103, null);
		}

		private void comboBox103_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ((CodeDirection)comboBox103.SelectedItem == CodeDirection.FORWARD)
				comboBox105.SelectedIndex = 0;
			else
				comboBox105.SelectedIndex = 1;
		}

		private void comboBox104_SelectedIndexChanged(object sender, EventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_currSegElem);
			if (comboBox104.SelectedIndex < 0)
				return;

			_firstRouteLeg = (Segment)comboBox104.SelectedItem;

			comboBox102.Items.Clear();

			if (_firstRouteLeg.pFeature.Availability != null)
			{
				foreach (var avail in _firstRouteLeg.pFeature.Availability)
				{
					if (avail.Direction != null)
						comboBox102.Items.Add(avail.Direction);
				}
			}

			_currDir = ARANFunctions.ReturnAngleInRadians(_firstRouteLeg.Start.PrjPt, _firstRouteLeg.End.PrjPt);

			//double maxDistance = ARANFunctions.ReturnDistanceInMeters(_firstRouteLeg.Start.PrjPt, _firstRouteLeg.End.PrjPt);
			textBox101.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(ConverterToSI.Convert(_firstRouteLeg.pFeature.Length, 0)).ToString();

			double trueTrack;

			if (_firstRouteLeg.pFeature.TrueTrack != null)
				trueTrack = (double)_firstRouteLeg.pFeature.TrueTrack;
			else
				trueTrack = ARANFunctions.DirToAzimuth(_firstRouteLeg.Start.PrjPt, _currDir, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			textBox102.Text = trueTrack.ToString("0.00");

			LineString ls = new LineString();
			ls.Add(_firstRouteLeg.Start.PrjPt);
			ls.Add(_firstRouteLeg.End.PrjPt);

			_currSegElem = GlobalVars.gAranGraphics.DrawLineString(ls, 2, ARANFunctions.RGB(32, 255, 64));

			var geoOp = new Geometries.Operators.JtsGeometryOperators();// GeometryOperators();
			double minDist = geoOp.GetDistance(ls, GlobalVars.CurrADHP.pPtPrj);

			_maxAltitude = Math.Min(GlobalVars.CurrADHP.pPtPrj.Z + minDist * 0.08, 9000.0);
			if (_firstRouteLeg.pFeature.UpperLimit != null)
				_maxAltitude = ConverterToSI.Convert(_firstRouteLeg.pFeature.UpperLimit, _maxAltitude);

			//_minAltitude = Math.Max(GlobalVars.CurrADHP.pPtPrj.Z + minDist * 0.03, GlobalVars.CurrADHP.pPtPrj.Z + 600.0);
			//if (_firstRouteLeg.pFeature.LowerLimit != null)
			//	_minAltitude = ConverterToSI.Convert(_firstRouteLeg.pFeature.LowerLimit, _minAltitude);

			textBox103.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_maxAltitude).ToString();

			comboBox105.Items.Clear();
			comboBox105.Items.Add(_firstRouteLeg.Start);
			comboBox105.Items.Add(_firstRouteLeg.End);
			comboBox105.SelectedIndex = 0;

			//if (radioButton101.Checked)
			//{
			//	textBox105.Tag = null;
			//	textBox105_Validating(textBox105);
			//}

			if (comboBox102.Items.Count > 0)
				comboBox102.SelectedIndex = 0;
			else
				comboBox102_SelectedIndexChanged(comboBox102, null);
		}

		private void AddvanceToPageII()
		{
			comboBox201.Enabled = true;
			comboBox202.Enabled = true;
			AddBtn.Enabled = true;

			_legs.Clear();

			_currIndex = comboBox104.SelectedIndex;
			_currRouteLeg = _firstRouteLeg;
			_currRouteLeg.Direction = (CodeDirection)comboBox103.SelectedItem;
			_currRouteLeg.Altitude = _maxAltitude;
			_currRouteLeg.MOC = _moc;

			if (_currRouteLeg.Direction == CodeDirection.FORWARD)
			{
				_currDir = ARANFunctions.ReturnAngleInRadians(_currRouteLeg.Start.PrjPt, _currRouteLeg.End.PrjPt);

				if (_currIndex > 0)
				{
					double tmpDir = ARANFunctions.ReturnAngleInRadians(_currProcLegs[_currIndex - 1].Start.PrjPt, _currProcLegs[_currIndex - 1].End.PrjPt);
					_currRouteLeg.Start.EntryDirection = tmpDir;		// _currProcLegs[_currIndex - 1].Dir;
				}
				else
					_currRouteLeg.Start.EntryDirection = _currDir;

				_currIndex++;

				if (_currProcLegs.Count > _currIndex)
				{
					double tmpDir = ARANFunctions.ReturnAngleInRadians(_currProcLegs[_currIndex].Start.PrjPt, _currProcLegs[_currIndex].End.PrjPt);
					_currRouteLeg.End.OutDirection = tmpDir;//_currProcLegs[_currIndex].Dir;
				}
				else
					_currRouteLeg.End.OutDirection = _currDir;
			}
			else
			{
				_currRouteLeg.Start = _firstRouteLeg.End;
				_currRouteLeg.End = _firstRouteLeg.Start;
				_currDir = ARANFunctions.ReturnAngleInRadians(_currRouteLeg.Start.PrjPt, _currRouteLeg.End.PrjPt);

				if (_currIndex < _currProcLegs.Count - 1)
				{
					double tmpDir = ARANFunctions.ReturnAngleInRadians(_currProcLegs[_currIndex + 1].End.PrjPt, _currProcLegs[_currIndex + 1].Start.PrjPt);
					_currRouteLeg.Start.EntryDirection = tmpDir;	// _currProcLegs[_currIndex + 1].Dir;
				}
				else
					_currRouteLeg.Start.EntryDirection = _currDir;

				_currIndex--;

				if (_currIndex >= 0)
				{
					double tmpDir = ARANFunctions.ReturnAngleInRadians(_currProcLegs[_currIndex].End.PrjPt, _currProcLegs[_currIndex].Start.PrjPt);
					_currRouteLeg.End.OutDirection = tmpDir;// _currProcLegs[_currIndex].Dir;
				}
				else
					_currRouteLeg.End.OutDirection = _currRouteLeg.Dir;
			}

			_currRouteLeg.Dir = _currDir;
			_currRouteLeg.TrueTrack = ARANFunctions.DirToAzimuth(_currRouteLeg.Start.PrjPt, _currRouteLeg.Dir, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			_currRouteLeg.Start.OutDirection = _currRouteLeg.Dir;
			_currRouteLeg.Start.ConstructAltitude = _currRouteLeg.Altitude;
			_currRouteLeg.Start.NomLineAltitude = _currRouteLeg.Altitude;
			_currRouteLeg.Start.DrawingEnabled = true;

			_currRouteLeg.End.EntryDirection = _currRouteLeg.Dir;
			_currRouteLeg.End.ConstructAltitude = _currRouteLeg.Altitude;
			_currRouteLeg.End.NomLineAltitude = _currRouteLeg.Altitude;
			_currRouteLeg.End.DrawingEnabled = true;
			_currRouteLeg.Length = ARANFunctions.ReturnDistanceInMeters(_currRouteLeg.Start.PrjPt, _currRouteLeg.End.PrjPt);

			_currRouteLeg.leg = new EnRouteLeg(_currRouteLeg.Start, _currRouteLeg.End, CodeDirection.FORWARD, GlobalVars.gAranEnv);

			textBox201.Text = _currRouteLeg.Start.Name;
			textBox209.Text = _currRouteLeg.End.Name;
			textBox203.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_currRouteLeg.Length).ToString();
			textBox211.Text = _currRouteLeg.TrueTrack.ToString("0.00");
			textBox212.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_currRouteLeg.Altitude).ToString();

			LineString ls;
			MultiLineString allLegs = new MultiLineString();

			foreach (var sgm in _currProcLegs)
			{
				ls = new LineString();
				ls.Add(sgm.Start.PrjPt);
				ls.Add(sgm.End.PrjPt);

				allLegs.Add(ls);
			}

			//_coverageElem = GlobalVars.gAranGraphics.DrawMultiPolygon(searchArea, ARANFunctions.RGB(0,255,255), eFillStyle.sfsDiagonalCross);
			_allSegElem = GlobalVars.gAranGraphics.DrawMultiLineString(allLegs, 2, 255);

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_currSegElem);

			ls = new LineString();
			ls.Add(_currRouteLeg.Start.PrjPt);
			ls.Add(_currRouteLeg.End.PrjPt);
			_currRouteLeg.NominalTracktElem = GlobalVars.gAranGraphics.DrawLineString(ls, 2, ARANFunctions.RGB(32, 255, 64));

			comboBox201.Items.Clear();
			comboBox202.Items.Clear();

			GeometryOperators geomOpp = new GeometryOperators();
			MultiPolygon searchArea = (MultiPolygon)geomOpp.Buffer(allLegs, DVORMaxRange);
			DBModule.FillNavaidList(out GlobalVars.NavaidList, out GlobalVars.DMEList, searchArea);

			foreach (NavaidType nav in GlobalVars.NavaidList)
			{
				double minDist = 0.0;	// (_currRouteLeg.Altitude - nav.pPtPrj.Z) / 1.7320508075688767;

				double dist = ARANFunctions.ReturnDistanceInMeters(_currRouteLeg.Start.PrjPt, nav.pPtPrj);
				if (dist >= minDist && dist <= nav.Range)
					comboBox201.Items.Add(nav);

				dist = ARANFunctions.ReturnDistanceInMeters(_currRouteLeg.End.PrjPt, nav.pPtPrj);
				if (dist >= minDist && dist <= nav.Range)
					comboBox202.Items.Add(nav);
			}

			if (comboBox201.Items.Count > 0)
				comboBox201.SelectedIndex = 0;

			if (comboBox202.Items.Count > 0)
				comboBox202.SelectedIndex = 0;

			AddBtn.Enabled = comboBox201.Items.Count > 0 && comboBox202.Items.Count > 0;
		}

		private void BackToPageI()
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_allSegElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_startNavRangeElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_endNavRangeElem);

			_currRouteLeg.DeleteGraphics();

			foreach (Segment sg in _legs)
				sg.DeleteGraphics();

			LineString ls = new LineString();
			ls.Add(_firstRouteLeg.Start.PrjPt);
			ls.Add(_firstRouteLeg.End.PrjPt);

			_currSegElem = GlobalVars.gAranGraphics.DrawLineString(ls, 2, ARANFunctions.RGB(32, 255, 64));
		}

		#endregion

		#region Page II
		private void comboBox201_SelectedIndexChanged(object sender, EventArgs e)
		{
			_currRouteLeg.StartVOR = (NavaidType)comboBox201.SelectedItem;
			_currRouteLeg.StartDME = GlobalVars.DMEList[_currRouteLeg.StartVOR.PairNavaidIndex];

			EnRouteLeg prevleg = null;
			if (_legs.Count > 0)
				prevleg = _legs[_legs.Count - 1].leg;

			_currRouteLeg.leg.CreateGeometry(prevleg, CodeDirection.FORWARD);
			_currRouteLeg.leg.RefreshGraphics();

			textBox202.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_currRouteLeg.StartVOR.Range).ToString();

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_startNavRangeElem);
			_startCoverageRangePoly = ARANFunctions.CreateCircleAsMultiPolyPrj(_currRouteLeg.StartDME.pPtPrj, _currRouteLeg.StartVOR.Range);
			_startNavRangeElem = GlobalVars.gAranGraphics.DrawMultiPolygon(_startCoverageRangePoly, eFillStyle.sfsHollow, ARANFunctions.RGB(255, 0, 255));
		}

		private void comboBox202_SelectedIndexChanged(object sender, EventArgs e)
		{
			_currRouteLeg.EndVOR = (NavaidType)comboBox202.SelectedItem;
			_currRouteLeg.EndDME = GlobalVars.DMEList[_currRouteLeg.EndVOR.PairNavaidIndex];

			EnRouteLeg prevleg = null;
			if (_legs.Count > 0)
				prevleg = _legs[_legs.Count - 1].leg;

			_currRouteLeg.leg.CreateGeometry(prevleg, CodeDirection.FORWARD);
			_currRouteLeg.leg.RefreshGraphics();

			textBox210.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_currRouteLeg.EndVOR.Range).ToString();

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_endNavRangeElem);
			_endCoverageRangePoly = ARANFunctions.CreateCircleAsMultiPolyPrj(_currRouteLeg.EndDME.pPtPrj, _currRouteLeg.EndVOR.Range);
			_endNavRangeElem = GlobalVars.gAranGraphics.DrawMultiPolygon(_endCoverageRangePoly, eFillStyle.sfsHollow, ARANFunctions.RGB(255, 0, 255));
		}

		private void comboBox203_SelectedIndexChanged(object sender, EventArgs e)
		{
			_moc = GlobalVars.unitConverter.HeightToInternalUnits(double.Parse(comboBox203.Text));
			_currRouteLeg.MOC = _moc;

		}

		private void InfoBtn_Click(object sender, EventArgs e)
		{
			Button so = (Button)sender;
			int index = Convert.ToInt32(so.Tag.ToString());
			//System.Drawing.Point pos = so.PointToScreen(new System.Drawing.Point(so.Width, so.Height));
			System.Drawing.Point pos = so.PointToScreen(new System.Drawing.Point(0, so.Height));
			_FIXInfoForm.ShowInfo(_currRouteLeg, index, pos);
		}

		private void AddBtn_Click(object sender, EventArgs e)
		{
			_legs.Add(_currRouteLeg);
			RemoveBtn.Enabled = true;
			NextBtn.Enabled = true;

			//_currRouteLeg.DeleteGraphics();

			if (_currIndex < 0 || _currIndex >= _currProcLegs.Count)
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_startNavRangeElem);
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_endNavRangeElem);

				comboBox201.Enabled = false;
				comboBox202.Enabled = false;
				AddBtn.Enabled = false;
				return;
			}

			_currRouteLeg = _currProcLegs[_currIndex];
			_currRouteLeg.Direction = (CodeDirection)comboBox103.SelectedItem;
			//_currRouteLeg.Direction = _segDir;
			_currRouteLeg.Altitude = _maxAltitude;
			_currRouteLeg.MOC = _moc;

			if (_currRouteLeg.Direction == CodeDirection.FORWARD )
			{
				_currDir = ARANFunctions.ReturnAngleInRadians(_currRouteLeg.Start.PrjPt, _currRouteLeg.End.PrjPt);

				if (_currIndex > 0)
				{
					double tmpDir = ARANFunctions.ReturnAngleInRadians(_currProcLegs[_currIndex - 1].Start.PrjPt, _currProcLegs[_currIndex - 1].End.PrjPt);
					_currRouteLeg.Start.EntryDirection = tmpDir;	// _currProcLegs[_currIndex - 1].Dir;
				}
				else
					_currRouteLeg.Start.EntryDirection = _currRouteLeg.Dir;

				_currIndex++;

				if (_currProcLegs.Count > _currIndex)
				{
					double tmpDir = ARANFunctions.ReturnAngleInRadians(_currProcLegs[_currIndex].Start.PrjPt, _currProcLegs[_currIndex].End.PrjPt);
					_currRouteLeg.End.OutDirection = tmpDir;	// _currProcLegs[_currIndex].Dir;
				}
				else
					_currRouteLeg.End.OutDirection = _currRouteLeg.Dir;
			}
			else
			{
				FIX tmpFix = _currRouteLeg.Start;
				_currRouteLeg.Start = _currRouteLeg.End;
				_currRouteLeg.End = tmpFix;

				_currDir = ARANFunctions.ReturnAngleInRadians(_currRouteLeg.Start.PrjPt, _currRouteLeg.End.PrjPt);

				if (_currIndex < _currProcLegs.Count - 1)
				{
					double tmpDir = ARANFunctions.ReturnAngleInRadians(_currProcLegs[_currIndex + 1].End.PrjPt, _currProcLegs[_currIndex + 1].Start.PrjPt);
					_currRouteLeg.Start.EntryDirection = tmpDir;	// _currProcLegs[_currIndex + 1].Dir;
				}
				else
					_currRouteLeg.Start.EntryDirection = _currRouteLeg.Dir;

				_currIndex--;

				if (_currIndex >= 0)
				{
					double tmpDir = ARANFunctions.ReturnAngleInRadians(_currProcLegs[_currIndex].End.PrjPt, _currProcLegs[_currIndex].Start.PrjPt);
					_currRouteLeg.End.OutDirection = tmpDir;// _currProcLegs[_currIndex].Dir;
				}
				else
					_currRouteLeg.End.OutDirection = _currRouteLeg.Dir;
			}

			if (_currRouteLeg.pFeature.UpperLimit != null)
				_maxAltitude = ConverterToSI.Convert(_currRouteLeg.pFeature.UpperLimit, _maxAltitude);

			_currRouteLeg.Dir = _currDir;
			_currRouteLeg.TrueTrack = ARANFunctions.DirToAzimuth(_currRouteLeg.Start.PrjPt, _currRouteLeg.Dir, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			_currRouteLeg.Start.OutDirection = _currRouteLeg.Dir;
			_currRouteLeg.Start.ConstructAltitude = _currRouteLeg.Altitude;
			_currRouteLeg.Start.NomLineAltitude = _currRouteLeg.Altitude;
			_currRouteLeg.Start.DrawingEnabled = true;

			_currRouteLeg.End.EntryDirection = _currRouteLeg.Dir;
			_currRouteLeg.End.ConstructAltitude = _currRouteLeg.Altitude;
			_currRouteLeg.End.NomLineAltitude = _currRouteLeg.Altitude;
			_currRouteLeg.End.DrawingEnabled = true;
			_currRouteLeg.Length = ARANFunctions.ReturnDistanceInMeters(_currRouteLeg.Start.PrjPt, _currRouteLeg.End.PrjPt);

			_currRouteLeg.leg = new EnRouteLeg(_currRouteLeg.Start, _currRouteLeg.End, CodeDirection.FORWARD, GlobalVars.gAranEnv);

			textBox201.Text = _currRouteLeg.Start.Name;
			textBox209.Text = _currRouteLeg.End.Name;
			textBox203.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_currRouteLeg.Length).ToString();
			textBox211.Text = _currRouteLeg.TrueTrack.ToString("0.00");
			textBox212.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_currRouteLeg.Altitude).ToString();

			LineString ls = new LineString();
			ls.Add(_currRouteLeg.Start.PrjPt);
			ls.Add(_currRouteLeg.End.PrjPt);
			_currRouteLeg.NominalTracktElem = GlobalVars.gAranGraphics.DrawLineString(ls, 2, ARANFunctions.RGB(32, 255, 64));

			comboBox201.Items.Clear();
			comboBox202.Items.Clear();

			foreach (NavaidType nav in GlobalVars.NavaidList)
			{
				double minDist = 0.0;		//(_currRouteLeg.Altitude - nav.pPtPrj.Z) / 1.7320508075688767;

				double dist = ARANFunctions.ReturnDistanceInMeters(_currRouteLeg.Start.PrjPt, nav.pPtPrj);
				if (dist >= minDist && dist <= nav.Range)
					comboBox201.Items.Add(nav);

				dist = ARANFunctions.ReturnDistanceInMeters(_currRouteLeg.End.PrjPt, nav.pPtPrj);
				if (dist >= minDist && dist <= nav.Range)
					comboBox202.Items.Add(nav);
			}

			if (comboBox201.Items.Count > 0)
				comboBox201.SelectedIndex = 0;

			if (comboBox202.Items.Count > 0)
				comboBox202.SelectedIndex = 0;

			AddBtn.Enabled = comboBox201.Items.Count > 0 && comboBox202.Items.Count > 0;
		}

		private void RemoveBtn_Click(object sender, EventArgs e)
		{
			_currRouteLeg.DeleteGraphics();

			if (_legs.Count != _currProcLegs.Count)
				if (comboBox105.SelectedIndex == 0)
					_currIndex--;
				else
					_currIndex++;

			_currRouteLeg = _legs[_legs.Count - 1];
			_legs.RemoveAt(_legs.Count - 1);

			comboBox201.Enabled = true;
			comboBox202.Enabled = true;
			AddBtn.Enabled = true;

			if (_legs.Count == 0)
			{
				RemoveBtn.Enabled = false;
				NextBtn.Enabled = false;
			}

			_currDir = _currRouteLeg.Dir;
			_maxAltitude = _currRouteLeg.Altitude;
			_moc = _currRouteLeg.MOC;
			comboBox203.SelectedItem = _moc;

			textBox201.Text = _currRouteLeg.Start.Name;
			textBox209.Text = _currRouteLeg.End.Name;
			textBox203.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_currRouteLeg.Length).ToString();
			textBox211.Text = _currRouteLeg.TrueTrack.ToString("0.00");
			textBox212.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_currRouteLeg.Altitude).ToString();

			//LineString ls = new LineString();
			//ls.Add(_currRouteLeg.Start.PrjPt);
			//ls.Add(_currRouteLeg.End.PrjPt);
			//_currRouteLeg.NominalTracktElem = GlobalVars.gAranGraphics.DrawLineString(ls, ARANFunctions.RGB(32, 255, 64), 2);

			comboBox201.Items.Clear();
			comboBox202.Items.Clear();

			foreach (NavaidType nav in GlobalVars.NavaidList)
			{
				double minDist = 0.0;	// (_currRouteLeg.Altitude - nav.pPtPrj.Z) / 1.7320508075688767;

				double dist = ARANFunctions.ReturnDistanceInMeters(_currRouteLeg.Start.PrjPt, nav.pPtPrj);
				if (dist >= minDist && dist <= nav.Range)
					comboBox201.Items.Add(nav);

				dist = ARANFunctions.ReturnDistanceInMeters(_currRouteLeg.End.PrjPt, nav.pPtPrj);
				if (dist >= minDist && dist <= nav.Range)
					comboBox202.Items.Add(nav);
			}

			if (comboBox201.Items.Count > 0)
				comboBox201.SelectedIndex = 0;

			if (comboBox202.Items.Count > 0)
				comboBox202.SelectedIndex = 0;

			AddBtn.Enabled = comboBox201.Items.Count > 0 && comboBox202.Items.Count > 0;
		}

		private void FillLegObstacles()
		{
			MultiPolygon mpFullPoly = new MultiPolygon();
			Segment currLeg;
			GeometryOperators geoOp = new GeometryOperators();
			int n = _legs.Count;

			for (int i = 0; i < n; i++)
			{
				currLeg = _legs[i];
				mpFullPoly = (MultiPolygon)geoOp.UnionGeometry(mpFullPoly, currLeg.leg.FullAssesmentArea);
			}

			ObstacleContainer Obstacles;
			DBModule.GetObstaclesByPoly(out Obstacles, mpFullPoly);

			for (int i = 0; i < n; i++)
			{
				currLeg = _legs[i];
				//currLeg.leg.AssesmentAreaOutline = ARANFunctions.PolygonToPolyLine(currLeg.leg.FullAssesmentArea);
				DBModule.GetLegObstList(Obstacles, ref currLeg);
				_legs[i] = currLeg;
			}

			_reportForm.Update(_legs);
		}

		public void Update(List<Segment> legs)
		{
			int n = legs.Count;

			dataGridView01.RowCount = 0;
			_TotalLen = 0.0;

			for (int i = 0; i < n; i++)
			{
				Segment leg = legs[i];
				leg.MagnTrack = NativeMethods.Modulus(leg.TrueTrack + leg.Start.MagVar);

				legs[i] = leg;

				DataGridViewRow row = new DataGridViewRow();
				row.Tag = legs[i];
				row.ReadOnly = true;

				_TotalLen += leg.Length;

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = (i + 1).ToString();
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = ePBNClass.RNAV5;   //leg.PBNType;
				row.Cells.Add(cell);

				//==================================================//

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = leg.Start.Name;

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = leg.End.Name;

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = Math.Round(leg.TrueTrack, 2).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				//row.Cells[5].Value = leg.Direction;

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(leg.Length, eRoundMode.NEAREST);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[7].Value = GlobalVars.unitConverter.HeightToDisplayUnits(leg.Altitude, eRoundMode.NEAREST);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[8].Value = GlobalVars.unitConverter.HeightToDisplayUnits(leg.MOC, eRoundMode.SPECIAL_CEIL);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[9].Value = GlobalVars.unitConverter.HeightToDisplayUnits(leg.MOCA, eRoundMode.SPECIAL_CEIL);

				dataGridView01.Rows.Add(row);
			}
		}

		private void SaveObstacleReports(string RepFileName, string RepFileTitle, ReportHeader pReport)
		{
			CReportFile RoutsObstRep = new CReportFile();
			RoutsObstRep.OpenFile(RepFileName + "_Protocol", Resources.str00127);
			RoutsObstRep.WriteString(System.Net.WebUtility.HtmlEncode(Resources.str00030 + " - " + Resources.str00127));

			RoutsObstRep.WriteString("");
			RoutsObstRep.WriteString(System.Net.WebUtility.HtmlEncode(RepFileTitle));
			RoutsObstRep.WriteHeader(pReport);

			RoutsObstRep.WriteString("");
			RoutsObstRep.WriteString("");

			int n = _legs.Count;

			RoutsObstRep.WriteString(System.Net.WebUtility.HtmlEncode(Resources.str00129 + n + Resources.str00130));

			for (int i = 0; i < n; i++)
			{
				Segment currLeg = _legs[i];
				ObstacleContainer Obstacles = currLeg.Obstacles;
				int m = Obstacles.Parts.Length;

				Utility.shall_SortfSortD(Obstacles.Parts);

				RoutsObstRep.SaveObstacleTable(currLeg.Obstacles, Resources.str00131 + currLeg.Start.Name + " - " + currLeg.End.Name);
				RoutsObstRep.WriteString("");
			}

			RoutsObstRep.WriteString("");
			RoutsObstRep.CloseFile();
		}

		private int ConvertTracToPoints(out ReportPoint[] GuidPoints)
		{
			int n = _legs.Count;
			if (n == 0)
			{
				GuidPoints = new ReportPoint[0];
				return 0;
			}

			GuidPoints = new ReportPoint[n + 1];
			Segment leg;

			for (int i = 0; i < n; i++)
			{
				leg = _legs[i];
				GuidPoints[i].Description = leg.Start.Name;

				GuidPoints[i].Lat = leg.Start.GeoPt.Y;
				GuidPoints[i].Lon = leg.Start.GeoPt.X;

				GuidPoints[i].TrueTrack = leg.TrueTrack;
				GuidPoints[i].MagnTrack = NativeMethods.Modulus(leg.TrueTrack + leg.Start.MagVar);
				GuidPoints[i].Altitude = leg.Altitude;

				GuidPoints[i].MOC = leg.MOC;
				GuidPoints[i].MOCA = leg.MOCA;

				GuidPoints[i].DistToNext = leg.Length;
			}

			leg = _legs[n - 1];

			GuidPoints[n].Description = leg.End.Name;
			GuidPoints[n].Lat = leg.End.GeoPt.Y;
			GuidPoints[n].Lon = leg.End.GeoPt.X;

			GuidPoints[n].TrueTrack = leg.TrueTrack;
			GuidPoints[n].MagnTrack = NativeMethods.Modulus(leg.TrueTrack + leg.Start.MagVar);
			GuidPoints[n].Altitude = leg.Altitude;

			GuidPoints[n].MOC = -1.0;
			GuidPoints[n].MOCA = -1.0;

			GuidPoints[n].DistToNext = -1.0;

			return n + 1;
		}

		private void AddvanceToPageIII()
		{
			FillLegObstacles();
			Update(_legs);
		}

		private void BackToPageII()
		{
			ReportBtn.Checked = false;
		}

		#endregion
	}
}
