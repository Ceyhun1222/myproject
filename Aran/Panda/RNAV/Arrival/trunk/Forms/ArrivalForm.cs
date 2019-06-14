using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Aran.Aim.Data.Filters;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Queries;
using Aran.Converters;
using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using Aran.PANDA.RNAV.Arrival.Properties;
using System.Linq;
using Aran.Aim.Data;
using Aran.AranEnvironment;

namespace Aran.PANDA.RNAV.Arrival
{
	public partial class ArrivalForm : Form
	{
		const double maxTurnAngle = 135.0;          // (double)numericUpDown101.Value;
													//const double minTurnAngle = 55.0 * ARANMath.DegToRadValue;
		const double minTurnAngle = 5.0 * ARANMath.DegToRadValue;

		const double maxGrad = 0.08;
		const double EnRouteIAS = 583.38 * 0.277777777777777777777777;

		private double[] EnrouteMOCValues = { 300.0, 450.0, 600.0 };

		private IScreenCapture screenCapture;
		private StandardInstrumentArrival pProcedure;
		private CReportFile arrivalLogRep;
		private CReportFile arrivalProtRep;
		private CReportFile arrivalGeomRep;

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
					Label1[i].ForeColor = System.Drawing.Color.Silver;
					Label1[i].Font = new System.Drawing.Font(Label1[i].Font, System.Drawing.FontStyle.Regular);
				}

				Label1[value].ForeColor = System.Drawing.Color.FromArgb(0XFF8000);
				Label1[value].Font = new System.Drawing.Font(Label1[value].Font, System.Drawing.FontStyle.Bold);

				this.Text = Resources.str00030 + "  [" + Label1[value].Text + "]";
				//this.HelpContextID = 4000 + 100 * (tabControl1.SelectedIndex + 1);
			}

			get { return tabControl1.SelectedIndex; }
		}

		#region Variable declarations

		//private int HelpContextID;
		private bool bFormInitialised = false;

		private ReportsForm _reportForm;
		private Label[] Label1;

		//int _wptNum;
		private int _circleElem;

		//double _ws;
		//double _minLenght;
		//double _bankAngle;
		//double _TotalLen;


		#region Page I variables

		private List<Segment> _routeLegs;
		private Segment _currRouteLeg;
		private LegArrival _forwardLeg;
		private LegArrival _backwardLeg;

		private FIX _forwardStart;
		private FIX _forwardEnd;

		private FIX _backwardStart;
		private FIX _backwardEnd;

		private FIX _startFIX;
		private int _currSegElem;
		private string _enRouteFIXName;

		private CodeDirection _segDir;
		private CodeDirection _leaveFromDir;
		private double _currDir;
		private double _maxDistance;

		//private double _segmAltitude;
		private double _minAltitude;
		private double _maxAltitude;

		private double _leavAltitude;
		private double _enRouteFIXDistance;
		private double _TrueTrack;
		private double _enRouteIAS;
		#endregion

		#region Page II variables

		private FIX _currBFIX;
		private FIX _currFIX;
		private FIX _nextFIX;

		private List<LegArrival> _arrivalLegs;
		private LegArrival _currArrivalLeg;
		private LegArrival _currArrivalLegB;

		private string _arrivalFIXName;
		private double _arrivalFIXDistance;
		private double _prevAzt;

		private CodeSegmentPath _PathAndTermination;
		private double _maxLegLenght;
		private double _minLegLenght;

		private double _plannedTurn;

		private double _DescGr;
		private double _IAS;
		private double _minIAS;
		private double _maxIAS;
		private double _BankAngle;

		private double _prevAltitude;
		private double _currAltitude;
		private double _minAlltitude;
		private double _maxAlltitude;
		private double _currMOCLimit;

		#endregion

		#endregion

		#region Form

		public ArrivalForm()
		{
			InitializeComponent();

			screenCapture = GlobalVars.gAranEnv.GetScreenCapture(Aim.FeatureType.StandardInstrumentArrival.ToString());

			label103.Text = GlobalVars.unitConverter.DistanceUnit;
			label111.Text = GlobalVars.unitConverter.HeightUnit;
			label115.Text = GlobalVars.unitConverter.SpeedUnit;
			label116.Text = GlobalVars.unitConverter.DistanceUnit;

			label204.Text = GlobalVars.unitConverter.DistanceUnit;
			label208.Text = GlobalVars.unitConverter.SpeedUnit;
			label210.Text = GlobalVars.unitConverter.SpeedUnit;
			label212.Text = GlobalVars.unitConverter.HeightUnit;
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

			//Page I ==========================================================================
			label101.Text = Resources.str01101;
			label102.Text = Resources.str01102;
			label104.Text = Resources.str01103;
			label105.Text = Resources.str01104;
			label106.Text = Resources.str01105;
			label107.Text = Resources.str01106;
			label108.Text = Resources.str01107;
			label110.Text = Resources.str01108;
			label112.Text = Resources.str01109;
			label113.Text = Resources.str01110;
			label114.Text = Resources.str01125;
			label118.Text = Resources.str01112;

			groupBox101.Text = Resources.str01111;
			radioButton101.Text = Resources.str01113;
			radioButton102.Text = Resources.str01114;
			//Page II==========================================================================
			groupBox201.Text = Resources.str01115;
			groupBox202.Text = Resources.str01116;
			groupBox203.Text = Resources.str01117;
			radioButton201.Text = Resources.str01118;
			radioButton202.Text = Resources.str01119;
			radioButton203.Text = Resources.str01120;
			radioButton204.Text = Resources.str01114;
			checkBox201.Text = Resources.str01121;
			button202.Text = Resources.str01122;
			button203.Text = Resources.str01123;

			label203.Text = Resources.str01124;
			label206.Text = Resources.str01112;
			label207.Text = Resources.str01125;
			label209.Text = Resources.str01126;
			label211.Text = Resources.str01127;
			label213.Text = Resources.str01128;
			label215.Text = Resources.str01129;
			label217.Text = Resources.str01130;

			label219.Text = Resources.str01131;
			label220.Text = Resources.str01132;
			label221.Text = Resources.str01133;
			label222.Text = Resources.str01134;
			label223.Text = Resources.str01135;

			//==========================================================================
			//comboBox102.Items.AddRange(new object[] { CodeDirection.FORWARD, CodeDirection.BACKWARD, CodeDirection.BOTH });

			_maxIAS = EnRouteIAS;
			_enRouteIAS = _maxIAS;
			textBox107.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_enRouteIAS, eRoundMode.SPECIAL_NEAREST).ToString();

			_circleElem = -1;
			_currSegElem = -1;

			this.CurrPage = 0;

			comboBox102.Items.Add(CodeDirection.FORWARD);
			comboBox102.Items.Add(CodeDirection.BACKWARD);
			comboBox102.Items.Add(CodeDirection.BOTH);

			_enRouteFIXName = Resources.str01050 + "00";
			textBox106.Text = _enRouteFIXName;

			_enRouteFIXDistance = 0.0;

			_startFIX = new FIX(eFIXRole.TP_, GlobalVars.gAranEnv);
			_startFIX.FlightPhase = eFlightPhase.STARGE56;
			_startFIX.PBNType = ePBNClass.RNAV1;
			_startFIX.SensorType = eSensorType.GNSS;
			_startFIX.Role = eFIXRole.TP_;
			_startFIX.FlyMode = eFlyMode.Flyby;
			_startFIX.IAS = _enRouteIAS;
			_startFIX.BankAngle = ARANMath.DegToRad(15.0);

			_forwardStart = new FIX(eFIXRole.TP_, GlobalVars.gAranEnv);
			_forwardStart.Assign(_startFIX);
			_forwardEnd = new FIX(eFIXRole.TP_, GlobalVars.gAranEnv);
			_forwardEnd.Assign(_startFIX);

			_backwardStart = new FIX(eFIXRole.TP_, GlobalVars.gAranEnv);
			_backwardStart.Assign(_startFIX);
			_backwardEnd = new FIX(eFIXRole.TP_, GlobalVars.gAranEnv);
			_backwardEnd.Assign(_startFIX);

			_forwardLeg = new LegArrival(_forwardStart, _forwardEnd, GlobalVars.gAranEnv);
			_backwardLeg = new LegArrival(_backwardStart, _backwardEnd, GlobalVars.gAranEnv);

			_forwardLeg.Visible = false;
			_backwardLeg.Visible = false;
			_forwardStart.Visible = false;
			_forwardEnd.Visible = false;
			_backwardStart.Visible = false;
			_forwardEnd.Visible = false;
			_startFIX.Visible = true;

			_currFIX = new FIX(GlobalVars.gAranEnv);
			_currBFIX = new FIX(GlobalVars.gAranEnv);

			_nextFIX = new FIX(GlobalVars.gAranEnv);
			//_currFIX.BankAngle = _startFIX.BankAngle;
			//_nextFIX.BankAngle = _startFIX.BankAngle;
			_routeLegs = new List<Segment>();

			_currArrivalLeg = new LegArrival(_currFIX, _nextFIX, GlobalVars.gAranEnv);
			_currArrivalLegB = new LegArrival(_currBFIX, _nextFIX, GlobalVars.gAranEnv);

			_arrivalLegs = new List<LegArrival>();

			_reportForm = new ReportsForm();
			_reportForm.Init(ReportBtn);

			if (GlobalVars.gAranEnv.DbProvider is Aran.Aim.Data.DbProvider)
				FillComboBox101_Aim();
			else
				FillComboBox101();

			if (comboBox101.Items.Count < 1)
				throw new Exception("There must at least one En-Route procedure in Your database.");//, null, MessageBoxButtons.OK, MessageBoxIcon.Asterisk

			comboBox103.Items.Add(eSensorType.GNSS);
			comboBox103.Items.Add(eSensorType.DME_DME);

			comboBox106.Items.Add(ePBNClass.RNAV1);
			comboBox106.Items.Add(ePBNClass.RNAV2);
			comboBox106.Items.Add(ePBNClass.RNP4);
			comboBox106.Items.Add(ePBNClass.RNAV5);

			comboBox101.SelectedIndex = 0;
			comboBox102.SelectedIndex = 0;
			comboBox103.SelectedIndex = 0;
			comboBox105.SelectedIndex = 2;
			comboBox106.SelectedIndex = 0;

			n = GlobalVars.RWYList.Length;
			for (i = 0; i < n; i++)
			{
				var rwy = GlobalVars.RWYList[i];
				checkedListBox101.Items.Add(rwy);
				checkedListBox101.SetItemChecked(i, true);
			}

			//==========================================================================

			comboBox204.Items.Add(15.0);
			comboBox204.Items.Add(20.0);
			comboBox204.Items.Add(25.0);

			comboBox205.Items.Add(eSensorType.GNSS);
			comboBox205.Items.Add(eSensorType.DME_DME);

			comboBox207.Items.Add(ePBNClass.RNAV1);
			comboBox207.Items.Add(ePBNClass.RNAV2);
			comboBox207.Items.Add(ePBNClass.RNP1);
			comboBox207.Items.Add(ePBNClass.RNAV5);

			comboBox208.Items.Add(CodeSegmentPath.CF);
			comboBox208.Items.Add(CodeSegmentPath.TF);
			comboBox208.Items.Add(CodeSegmentPath.DF);

			comboBox209.Items.Clear();
			n = EnrouteMOCValues.Length;
			for (i = 0; i < n; i++)
				comboBox209.Items.Add(GlobalVars.unitConverter.HeightToDisplayUnits(EnrouteMOCValues[i], eRoundMode.SPECIAL_NEAREST));

			comboBox202.SelectedIndex = 0;
			comboBox204.SelectedIndex = 0;
			comboBox205.SelectedIndex = 0;
			comboBox206.SelectedIndex = 0;
			comboBox207.SelectedIndex = 0;
			comboBox208.SelectedIndex = 1;
			comboBox209.SelectedIndex = 0;

			int hang = tabControl1.Size.Height + 3 - tabControl1.TabPages[0].Size.Height - ((this.Width - this.ClientSize.Width) >> 1);
			int dd = hang - tabControl1.Top;

			//hang = tabControl1.ItemSize.Height + ((this.Width - this.ClientSize.Width) >> 1);
			//Frame02.Top = -hang;

			tabControl1.Top = -hang;
			ShowPanelBtn.Top = -hang;

			Frame01.Top -= dd;
			this.Height -= dd;

			this.bFormInitialised = true;

			if (ShowPanelBtn.Checked)
				ShowPanelBtn.Checked = false;
			else
				ShowPanelBtn_CheckedChanged(ShowPanelBtn, null);

		}

		private void ArrivalForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			screenCapture.Rollback();

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_currSegElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_circleElem);
			_startFIX.DeleteGraphics();
			_currBFIX.DeleteGraphics();

			_currArrivalLeg.DeleteGraphics();
			_currArrivalLegB.DeleteGraphics();

			foreach (LegArrival leg in _arrivalLegs)
				leg.DeleteGraphics();

			GlobalVars.gAranGraphics.Refresh();

			DBModule.CloseDB();
			_reportForm.Close();
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			// Get a handle to a copy of this form's system (window) menu
			IntPtr hSysMenu = Functions.GetSystemMenu(this.Handle, false);
			// Add a separator
			Functions.AppendMenu(hSysMenu, GlobalVars.MF_SEPARATOR, 0, string.Empty);
			// Add the About menu item
			Functions.AppendMenu(hSysMenu, GlobalVars.MF_STRING, GlobalVars.SYSMENU_ABOUT_ID, "&About…");
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

		private Point GetNavLocation(Navaid nav)
		{
			NavaidEquipment equipment;
			switch (nav.Type)
			{
				case CodeNavaidService.NDB:
				case CodeNavaidService.NDB_DME:
				case CodeNavaidService.NDB_MKR:
					for (int i = 0; i < nav.NavaidEquipment.Count; i++)
					{
						equipment = (NavaidEquipment)nav.NavaidEquipment[i].TheNavaidEquipment.GetFeature();
						if (equipment is NDB)
						{
							ElevatedPoint pElevPoint = equipment.Location;
							if (pElevPoint == null)
								return null;

							Point pPtGeo = pElevPoint.Geo;
							if (pPtGeo == null)
								return null;

							pPtGeo.Z = ConverterToSI.Convert(pElevPoint.Elevation, 0.0);
							return pPtGeo;
						}
					}
					break;
				case CodeNavaidService.VOR:
				case CodeNavaidService.VORTAC:
				case CodeNavaidService.VOR_DME:
					for (int i = 0; i < nav.NavaidEquipment.Count; i++)
					{
						equipment = (NavaidEquipment)nav.NavaidEquipment[i].TheNavaidEquipment.GetFeature();
						if (equipment is VOR)
						{
							ElevatedPoint pElevPoint = equipment.Location;
							if (pElevPoint == null)
								return null;

							Point pPtGeo = pElevPoint.Geo;
							if (pPtGeo == null)
								return null;

							pPtGeo.Z = ConverterToSI.Convert(pElevPoint.Elevation, 0.0);
							return pPtGeo;
						}
					}
					break;
			}

			return null;
		}

		private void FillComboBox101_Aim()
		{
			List<Route> pProcedureList = DBModule.pObjectDir.GetRouteList(Guid.Empty);      //GlobalVars.CurrADHP.OrgID
			pProcedureList.Sort(CompareRouteByName);

			ComparisonOps cmpOps = new ComparisonOps(ComparisonOpType.EqualTo, "RouteFormed");
			OperationChoice opChoice = new OperationChoice(cmpOps);
			Aim.Data.Filters.Filter filters = new Aim.Data.Filters.Filter(opChoice);
			_routeLegs.Clear();

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

					int n = _routeLegs.Count;

					foreach (RouteSegment rs in pSegmentLegList)
					{
						try
						{
							if (rs.Start == null || rs.End == null)
								continue;

							if (rs.Start.PointChoice == null || rs.End.PointChoice == null)
								continue;

							Segment sgm = new Segment();
							sgm.proc = prc.pFeature;
							sgm.pFeature = rs;

							sgm.Start = new FIX(GlobalVars.gAranEnv);
							sgm.Start.Visible = false;
							sgm.End = new FIX(GlobalVars.gAranEnv);
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

								sgm.Start.PrjPt = (Point)GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);
								sgm.Start.Name = dpt.Designator;
							}
							else if (rs.Start.PointChoice.Choice == Aim.SignificantPointChoice.Navaid)
							{
								Guid dptGuid = rs.Start.PointChoice.NavaidSystem.Identifier;
								Navaid dpt = (Navaid)DBModule.pObjectDir.GetFeature(Aim.FeatureType.Navaid, dptGuid);                               //segPt.PointChoice.FixDesignatedPoint.Identifier;
								if (dpt == null)
									continue;

								Geometries.Point pPtGeo = GetNavLocation(dpt);
								if (pPtGeo == null)
									continue;

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

								sgm.End.PrjPt = (Point)GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);
								sgm.End.Name = dpt.Designator;
							}
							else if (rs.End.PointChoice.Choice == Aim.SignificantPointChoice.Navaid)
							{
								Guid dptGuid = rs.End.PointChoice.NavaidSystem.Identifier;
								Navaid dpt = (Navaid)DBModule.pObjectDir.GetFeature(Aim.FeatureType.Navaid, dptGuid);
								//segPt.PointChoice.FixDesignatedPoint.Identifier;
								if (dpt == null)
									continue;

								Geometries.Point pPtGeo = GetNavLocation(dpt);
								if (pPtGeo == null)
									continue;

								sgm.End.PrjPt = (Geometries.Point)GlobalVars.pspatialReferenceOperation.ToPrj<Geometries.Point>(pPtGeo);
								sgm.End.Name = dpt.Designator;
							}
							else
								continue;

							_routeLegs.Add(sgm);
						}
						catch (Exception)
						{
							throw;
						}
					}

					//===========================================================+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

					if (n < _routeLegs.Count)
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

			var tmpRouteSegmentList =				DBModule.pObjectDir.GetFeatureList(Aim.FeatureType.RouteSegment).Cast<RouteSegment>().ToList();
			var designatedPtList = DBModule.pObjectDir.GetFeatureList(Aim.FeatureType.DesignatedPoint);
			var navaidList = DBModule.pObjectDir.GetFeatureList(Aim.FeatureType.DesignatedPoint);

			Func<EnRouteSegmentPoint, FIX> getFix = (segmentPt) =>
			{
				try
				{
					if (segmentPt == null) return null;

					string Name = null;
					Point pPtGeo = null;
					var identifer = new Guid();

					if (segmentPt.PointChoice.Choice == Aim.SignificantPointChoice.DesignatedPoint)
					{
						if (segmentPt.PointChoice.FixDesignatedPoint.Identifier != null)
							identifer = segmentPt.PointChoice.FixDesignatedPoint.Identifier;

						DesignatedPoint dsp = (dynamic)DBModule.pObjectDir.GetFeature(Aim.FeatureType.DesignatedPoint, identifer);
						pPtGeo = dsp.Location.Geo;
						Name = dsp.Designator;
					}
					else if (segmentPt.PointChoice.Choice == Aim.SignificantPointChoice.Navaid)
					{
						if (segmentPt.PointChoice.NavaidSystem.Identifier != null)
							identifer = segmentPt.PointChoice.FixDesignatedPoint.Identifier;

						Navaid nav = (Navaid)DBModule.pObjectDir.GetFeature(Aim.FeatureType.Navaid, identifer);

						for (int i = 0; i < nav.NavaidEquipment.Count; i++)
						{
							NavaidEquipment equipment = (NavaidEquipment)nav.NavaidEquipment[i].TheNavaidEquipment.GetFeature();
							if (equipment is NDB || equipment is VOR)
							{
								ElevatedPoint pElevPoint = equipment.Location;
								if (pElevPoint == null)
									return null;

								pPtGeo = pElevPoint.Geo;
								pPtGeo.Z = ConverterToSI.Convert(pElevPoint.Elevation, 0.0);
								Name = equipment.Designator;
								break;
							}
						}
					}

					if (pPtGeo == null)
						return null;

					var result = new FIX(GlobalVars.gAranEnv);
					result.PrjPt = (Geometries.Point)GlobalVars.pspatialReferenceOperation.ToPrj<Geometries.Point>(pPtGeo);
					result.Name = Name;
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

			_routeLegs.AddRange(segment);

			pProcedureList.ForEach(pro =>
			{
				if (_routeLegs.Count(tmpSegment => tmpSegment.pFeature.RouteFormed.Identifier == pro.Identifier) > 0)
					comboBox101.Items.Add(new Procedure { pFeature = pro });
			});
		}

		#endregion

		#region Common Form Events
		private void TextBoxes_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			//
		}

		private void HelpBtn_Click(object sender, EventArgs e)
		{

		}

		private void PrevBtn_Click(object sender, EventArgs e)
		{
			screenCapture.Delete();

			switch (tabControl1.SelectedIndex)
			{
				case 1:
					BackToPageI();
					break;
				case 2:
					_reportForm.SetCurrentPage(2);

					_arrivalLegs[_arrivalLegs.Count - 1].EndFIX.OutDirection = _currArrivalLeg.EndFIX.EntryDirection;

					if (_arrivalLegs.Count == 1)
						_arrivalLegs[_arrivalLegs.Count - 1].CreateGeometry(_forwardLeg, GlobalVars.CurrADHP, minTurnAngle);
					else
						_arrivalLegs[_arrivalLegs.Count - 1].CreateGeometry(_arrivalLegs[_arrivalLegs.Count - 2], GlobalVars.CurrADHP, minTurnAngle);

					_arrivalLegs[_arrivalLegs.Count - 1].RefreshGraphics();

					_currArrivalLeg.Visible = true;

					//BackToPageII();
					break;
			}

			this.CurrPage = tabControl1.SelectedIndex - 1;
			NativeMethods.HidePandaBox();
		}

		private void NextBtn_Click(object sender, EventArgs e)
		{
			screenCapture.Save(this);
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

		private void CancelBtn_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void ShowPanelBtn_CheckedChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised)
				return;

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

		#endregion

		#region Page I

		private void SetPageI()
		{

		}

		private void BackToPageI()
		{
			_currFIX.DeleteGraphics();
			_currBFIX.DeleteGraphics();

			_nextFIX.DeleteGraphics();
			_currArrivalLeg.DeleteGraphics();
			_currArrivalLegB.DeleteGraphics();

			foreach (LegArrival leg in _arrivalLegs)
				leg.DeleteGraphics();

			_arrivalLegs.Clear();
		}

		private void comboBox101_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox101.SelectedIndex < 0)
				return;

			comboBox104.Items.Clear();

			Procedure prc = (Procedure)comboBox101.SelectedItem;

			List<Segment> prcLegs = _routeLegs.FindAll(
				delegate (Segment sg)
				{ return sg.proc == prc.pFeature; }
				);

			foreach (var sgm in prcLegs)
				comboBox104.Items.Add(sgm);

			if (comboBox104.Items.Count > 0)
				comboBox104.SelectedIndex = 0;
		}

		private void comboBox104_SelectedIndexChanged(object sender, EventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_currSegElem);
			if (comboBox104.SelectedIndex < 0)
				return;

			_currRouteLeg = (Segment)comboBox104.SelectedItem;

			comboBox108.Items.Clear();

			if (_currRouteLeg.pFeature.Availability != null)
			{
				foreach (var avail in _currRouteLeg.pFeature.Availability)
				{
					if (avail.Direction != null)
						comboBox108.Items.Add(avail.Direction);
				}

				//if (_currRouteLeg.pFeature.Availability.Count == 0)
				//	textBox102.Text = "";
				//else if (_currRouteLeg.pFeature.Availability[0].Direction != null)
				//{
				//	_segDir = (CodeDirection)_currRouteLeg.pFeature.Availability[0].Direction;
				//	textBox102.Text = _currRouteLeg.pFeature.Availability[0].Direction.ToString();
				//}
			}

			if (comboBox108.Items.Count > 0)
				comboBox108.SelectedIndex = 0;
			else
				comboBox108_SelectedIndexChanged(comboBox108, null);

			_currDir = ARANFunctions.ReturnAngleInRadians(_currRouteLeg.Start.PrjPt, _currRouteLeg.End.PrjPt);

			_maxDistance = ARANFunctions.ReturnDistanceInMeters(_currRouteLeg.Start.PrjPt, _currRouteLeg.End.PrjPt);
			textBox101.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(ConverterToSI.Convert(_currRouteLeg.pFeature.Length, 0)).ToString();

			if (_currRouteLeg.pFeature.TrueTrack != null)
				_TrueTrack = (double)_currRouteLeg.pFeature.TrueTrack;
			else
				_TrueTrack = ARANFunctions.DirToAzimuth(_currRouteLeg.Start.PrjPt, _currDir, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			textBox103.Text = _TrueTrack.ToString("0.00");

			LineString ls = new LineString();
			ls.Add(_currRouteLeg.Start.PrjPt);
			ls.Add(_currRouteLeg.End.PrjPt);

			_currSegElem = GlobalVars.gAranGraphics.DrawLineString(ls, 2, ARANFunctions.RGB(32, 255, 64));

			var geoOp = new Geometries.Operators.JtsGeometryOperators();// GeometryOperators();
			double minDist = geoOp.GetDistance(ls, GlobalVars.CurrADHP.pPtPrj);

			_maxAltitude = Math.Min(GlobalVars.CurrADHP.pPtPrj.Z + minDist * 0.08, 9000.0);
			_minAltitude = Math.Max(GlobalVars.CurrADHP.pPtPrj.Z + minDist * 0.03, GlobalVars.CurrADHP.pPtPrj.Z + 600.0);

			if (_currRouteLeg.pFeature.UpperLimit != null)
				_maxAltitude = ConverterToSI.Convert(_currRouteLeg.pFeature.UpperLimit, _maxAltitude);

			if (_currRouteLeg.pFeature.LowerLimit != null)
				_minAltitude = ConverterToSI.Convert(_currRouteLeg.pFeature.LowerLimit, _minAltitude);

			_leavAltitude = _maxAltitude;

			textBox104.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_leavAltitude).ToString();

			_startFIX.EntryDirection = _currDir;
			_startFIX.ConstructAltitude = _leavAltitude;

			double anticipationDist = _startFIX.CalcConstructionFromMinStablizationDistance(ARANMath.DegToRad(120.0)) + _startFIX.CalcConstructionInToMinStablizationDistance(ARANMath.DegToRad(120.0));
			//_forwardStart.PrjPt = _currRouteLeg.Start.PrjPt;
			_forwardStart.PrjPt = ARANFunctions.LocalToPrj(_currRouteLeg.Start.PrjPt, _currDir, -anticipationDist);
			_forwardStart.EntryDirection = _currDir;
			_forwardStart.OutDirection = _currDir;
			_forwardStart.ConstructAltitude = _maxAltitude;
			_forwardStart.Name = _currRouteLeg.Start.Name + "'";

			//_backwardStart.PrjPt = _currRouteLeg.End.PrjPt;
			_backwardStart.PrjPt = ARANFunctions.LocalToPrj(_currRouteLeg.End.PrjPt, _currDir, anticipationDist);
			_backwardStart.EntryDirection = _currDir + Math.PI;
			_backwardStart.OutDirection = _currDir + Math.PI;
			_backwardStart.ConstructAltitude = _maxAltitude;
			_backwardStart.Name = _currRouteLeg.End.Name + "'";

			Ring ring = new Ring();

			ring.Add(ARANFunctions.LocalToPrj(_currRouteLeg.Start.PrjPt, _currDir, -0.1, 100.0));
			ring.Add(ARANFunctions.LocalToPrj(_currRouteLeg.Start.PrjPt, _currDir, -0.1, -100.0));
			ring.Add(ARANFunctions.LocalToPrj(_currRouteLeg.End.PrjPt, _currDir, 0.1, -100.0));
			ring.Add(ARANFunctions.LocalToPrj(_currRouteLeg.End.PrjPt, _currDir, 0.1, 100.0));

			Polygon pPolygon = new Polygon();
			pPolygon.ExteriorRing = ring;

			MultiPolygon pARANPolygon = new MultiPolygon();
			pARANPolygon.Add(pPolygon);

			WPT_FIXType[] fixs;

			//DBModule.FillWPT_FIXList(out fixs, pARANPolygon, GlobalVars.CurrADHP.Elev);

			DBModule.FillWPT_FIXList(out fixs, _currRouteLeg.Start.PrjPt, _currRouteLeg.End.PrjPt, _currDir, GlobalVars.CurrADHP.Elev);

			comboBox107.Items.Clear();

			foreach (WPT_FIXType fix in fixs)
				comboBox107.Items.Add(fix);

			if (comboBox107.Items.Count > 0)
				comboBox107.SelectedIndex = 0;

			if (radioButton101.Checked)
			{
				textBox105.Tag = null;
				textBox105_Validating(textBox105);
			}
		}

		private void comboBox105_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox105.SelectedIndex < 0)
				return;         //comboBox105.SelectedIndex = 0;

			_minIAS = GlobalVars.constants.AircraftCategory[aircraftCategoryData.ViafMin].Value[(aircraftCategory)comboBox105.SelectedIndex];
			textBox107.Tag = null;
			textBox107_Validating(null, null);
		}

		private void comboBox108_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox108.SelectedIndex < 0)
				_segDir = CodeDirection.BOTH;
			else
				_segDir = (CodeDirection)comboBox108.SelectedItem;      // _currRouteLeg.pFeature.Availability[0].Direction;

			//textBox102.Text = _currRouteLeg.pFeature.Availability[0].Direction.ToString();

			comboBox102.Enabled = _segDir == CodeDirection.BOTH;
			if (_segDir != CodeDirection.BOTH)
				comboBox102.SelectedItem = _segDir;
		}

		private void checkBox101_CheckedChanged(object sender, EventArgs e)
		{
			_startFIX.MultiCoverage = checkBox101.Checked;
			_forwardStart.MultiCoverage = checkBox101.Checked;
			_backwardStart.MultiCoverage = checkBox101.Checked;

			_startFIX.RefreshGraphics();
		}

		private void comboBox103_SelectedIndexChanged(object sender, EventArgs e)
		{
			eSensorType sensorType = (eSensorType)comboBox103.SelectedItem;
			_startFIX.SensorType = sensorType;
			_forwardStart.SensorType = sensorType;
			_backwardStart.SensorType = sensorType;

			_startFIX.RefreshGraphics();

			checkBox101.Enabled = sensorType == eSensorType.DME_DME;
		}

		private void comboBox106_SelectedIndexChanged(object sender, EventArgs e)
		{
			_startFIX.PBNType = (ePBNClass)comboBox106.SelectedItem;
			_forwardStart.PBNType = _startFIX.PBNType;
			_backwardStart.PBNType = _startFIX.PBNType;

			_startFIX.RefreshGraphics();
		}

		private void radioButton101_CheckedChanged(object sender, EventArgs e)
		{
			if (!((RadioButton)sender).Checked)
				return;

			if (sender == radioButton101)
			{
				textBox105.ReadOnly = false;
				textBox106.Text = _enRouteFIXName;
				textBox106.ReadOnly = false;
				comboBox107.Enabled = false;
				_startFIX.Name = _enRouteFIXName;
				_startFIX.CallSign = _enRouteFIXName;
				_startFIX.Id = Guid.Empty;
				textBox105_Validating(textBox105);
			}
			else
			{
				textBox105.ReadOnly = true;
				textBox106.ReadOnly = true;
				comboBox107.Enabled = true;

				comboBox107_SelectedIndexChanged(comboBox107);
			}
		}

		private void comboBox107_SelectedIndexChanged(object sender, EventArgs e = null)
		{
			if (comboBox107.SelectedIndex < 0)
				return;

			WPT_FIXType fix = (WPT_FIXType)comboBox107.SelectedItem;

			label117.Text = fix.TypeCode.Tostring();
			if (!radioButton102.Checked)
				return;

			//_startFIX.Assign(fix);
			_startFIX.Name = fix.Name;
			_startFIX.CallSign = fix.CallSign;
			_startFIX.Id = fix.Identifier;
			_startFIX.PrjPt = fix.pPtPrj;

			double dist = ARANFunctions.ReturnDistanceInMeters(_startFIX.PrjPt, GlobalVars.CurrADHP.pPtPrj);
			_startFIX.FlightPhase = dist > PANSOPSConstantList.PBNTerminalTriggerDistance ? eFlightPhase.STARGE56 : eFlightPhase.STAR;

			_startFIX.RefreshGraphics();

			textBox106.Text = comboBox107.Text;
			_enRouteFIXDistance = ARANFunctions.ReturnDistanceInMeters(_currRouteLeg.Start.PrjPt, _startFIX.PrjPt);

			textBox105.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_enRouteFIXDistance).ToString();
		}

		private void checkedListBox101_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			//if (e.NewValue != CheckState.Checked)
			//{
			//	if (checkedListBox101.CheckedItems.Count == 1)
			//	{
			//		int n = checkedListBox101.Items.Count;

			//		for (int i = 0; i < n; i++)
			//			if (!checkedListBox101.GetItemChecked(i))
			//			{
			//				checkedListBox101.SetItemChecked(i, true);
			//				break;
			//			}
			//	}
			//}
		}

		private void textBox104_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox104_Validating(textBox104, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox104.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox104_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			double fTmp;
			//double _leavAltitude = _minAltitude;

			if (double.TryParse(textBox104.Text, out fTmp))
			{
				if (textBox104.Tag != null && textBox104.Tag.ToString() == textBox104.Text)
					return;

				_leavAltitude = fTmp = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);

				if (_leavAltitude < _minAltitude)
					_leavAltitude = _minAltitude;

				if (_leavAltitude > _maxAltitude)
					_leavAltitude = _maxAltitude;

				if (_leavAltitude != fTmp)
					textBox104.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_leavAltitude).ToString();

				_startFIX.ConstructAltitude = _leavAltitude;
				_startFIX.RefreshGraphics();
			}
			else if (double.TryParse(textBox104.Tag.ToString(), out fTmp))
				textBox104.Text = textBox104.Tag.ToString();
			else
				textBox104.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_leavAltitude).ToString();

			textBox104.Tag = textBox104.Text;

			//if (!update)
			//	return;
			//_startFIX.RefreshGraphics();
		}

		private void textBox105_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox105_Validating(textBox105, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox105.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox105_Validating(object sender, System.ComponentModel.CancelEventArgs e = null)
		{
			if (!radioButton101.Checked)
				return;

			double fTmp;
			bool update = false;

			if (double.TryParse(textBox105.Text, out fTmp))
			{
				if (textBox105.Tag != null && textBox105.Tag.ToString() == textBox105.Text)
					return;

				fTmp = _enRouteFIXDistance = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);
				_startFIX.Name = _enRouteFIXName;

				if (_enRouteFIXDistance <= 0.0)
				{
					_enRouteFIXDistance = 0.0;
					_startFIX.Name = _currRouteLeg.Start.Name;
				}

				if (_enRouteFIXDistance >= _maxDistance)
				{
					_enRouteFIXDistance = _maxDistance;
					_startFIX.Name = _currRouteLeg.End.Name;
				}

				if (_enRouteFIXDistance != fTmp)
					textBox105.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_enRouteFIXDistance).ToString();

				update = true;
			}
			else if (double.TryParse(textBox105.Tag.ToString(), out fTmp))
				textBox105.Text = textBox105.Tag.ToString();
			else
				textBox105.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_enRouteFIXDistance).ToString();

			textBox105.Tag = textBox105.Text;

			if (!update)
				return;

			_startFIX.PrjPt = ARANFunctions.LocalToPrj(_currRouteLeg.Start.PrjPt, _currDir, _enRouteFIXDistance);
			double dist = ARANFunctions.ReturnDistanceInMeters(_startFIX.PrjPt, GlobalVars.CurrADHP.pPtPrj);
			_startFIX.FlightPhase = dist > PANSOPSConstantList.PBNTerminalTriggerDistance ? eFlightPhase.STARGE56 : eFlightPhase.STAR;

			_startFIX.RefreshGraphics();

			//CreateSegmentGeometry();
		}

		private void textBox106_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox106_Validating(textBox106, new System.ComponentModel.CancelEventArgs());
			else
			{
				if (eventChar >= ' ')
				{
					if ((eventChar < '0') || (eventChar > '9'))         // || eventChar != '_')
					{
						char alfa = (char)(eventChar & 32);
						if (alfa < 'A' || alfa > 'Z')
							eventChar = '\0';
						else
							eventChar = alfa;
					}
				}
			}

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox106_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_enRouteFIXName = textBox106.Text;
			_startFIX.Name = _enRouteFIXName;
			_startFIX.RefreshGraphics();
		}

		private void textBox107_KeyPress(object sender, KeyPressEventArgs e)
		{

			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox107_Validating(textBox107, null);
			else
				Functions.TextBoxFloat(ref eventChar, textBox107.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox107_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			double fTmp;

			if (double.TryParse(textBox107.Text, out fTmp))
			{
				if (textBox107.Tag != null && textBox107.Tag.ToString() == textBox107.Text)
					return;

				_enRouteIAS = fTmp = GlobalVars.unitConverter.SpeedToInternalUnits(fTmp);

				if (_enRouteIAS < _minIAS)
					_enRouteIAS = _minIAS;

				if (_enRouteIAS > _maxIAS)
					_enRouteIAS = _maxIAS;

				if (_enRouteIAS != fTmp)
					textBox107.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_enRouteIAS, eRoundMode.SPECIAL_NEAREST).ToString();

				_startFIX.IAS = _enRouteIAS;
			}
			else if (double.TryParse(textBox107.Tag.ToString(), out fTmp))
				textBox107.Text = textBox107.Tag.ToString();
			else
				textBox107.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_enRouteIAS, eRoundMode.SPECIAL_NEAREST).ToString();

			textBox107.Tag = textBox107.Text;
		}

		#endregion

		#region Page II
		private bool AddvanceToPageII()
		{
			//_wptNum = 1;
			_arrivalFIXName = Resources.str01050 + "01";
			textBox202.Text = _arrivalFIXName;
			_startFIX.OutDirection = _startFIX.EntryDirection + 0.5 * Math.PI;

			_currFIX.Assign(_startFIX);
			_currBFIX.Assign(_startFIX);
			_currBFIX.EntryDirection = _currDir + Math.PI;

			_nextFIX.Assign(_startFIX);
			_nextFIX.Name = textBox202.Text;
			_nextFIX.PBNType = (ePBNClass)comboBox207.SelectedItem;
			_nextFIX.BankAngle = ARANMath.DegToRad((double)comboBox204.SelectedItem);
			_nextFIX.SensorType = (eSensorType)comboBox205.SelectedItem;
			_nextFIX.FlyMode = (eFlyMode)comboBox206.SelectedIndex;
			_nextFIX.MultiCoverage = checkBox201.Checked;
			_nextFIX.IAS = _enRouteIAS;
			_nextFIX.ConstructAltitude = _leavAltitude;
			_nextFIX.BankAngle = _BankAngle;

			_currFIX.BankAngle = _nextFIX.BankAngle;
			if (_leaveFromDir == CodeDirection.BOTH)
				_currBFIX.BankAngle = _nextFIX.BankAngle;

			_currMOCLimit = GlobalVars.unitConverter.HeightToInternalUnits((double)comboBox209.SelectedItem);

			_forwardLeg.IAS = _enRouteIAS;
			_forwardLeg.StartFIX.Assign(_forwardStart);                     //_forwardLeg = new LegSBAS(_forwardStart, _startFIX, GlobalVars.gAranEnv);
			_forwardLeg.EndFIX.Assign(_currFIX);

			_backwardLeg.IAS = _enRouteIAS;
			_backwardLeg.StartFIX.Assign(_backwardStart);                   // _backwardLeg= new LegSBAS(_backwardStart, _startFIX, GlobalVars.gAranEnv);
			_backwardLeg.EndFIX.Assign(_currBFIX);

			_leaveFromDir = _segDir;
			if (_leaveFromDir == CodeDirection.BOTH)
				_leaveFromDir = (CodeDirection)comboBox102.SelectedItem;

			if (_leaveFromDir == CodeDirection.BACKWARD)
			{
				_currFIX.EntryDirection = _currDir + Math.PI;
				//_forwardLeg.EndFIX.Assign(_currFIX);
				_forwardLeg.EndFIX.EntryDirection = _currDir + Math.PI;

				_forwardLeg.StartFIX.Assign(_backwardStart);
			}

			_forwardLeg.CreateGeometry(null, GlobalVars.CurrADHP, minTurnAngle);
			_backwardLeg.CreateGeometry(null, GlobalVars.CurrADHP, minTurnAngle);

			_PathAndTermination = (CodeSegmentPath)comboBox208.SelectedItem;
			_plannedTurn = ARANMath.DegToRad((double)numericUpDown202.Value);

			_currAltitude = _leavAltitude;
			_prevAltitude = _leavAltitude;

			_maxAlltitude = _leavAltitude;
			_minAlltitude = GlobalVars.CurrADHP.pPtPrj.Z;

			_IAS = _enRouteIAS;

			textBox203.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_IAS).ToString();
			textBox204.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_nextFIX.ConstructTAS).ToString();

			textBox205.Text = textBox104.Text;

			double currDist = ARANFunctions.ReturnDistanceInMeters(GlobalVars.CurrADHP.pPtPrj, _startFIX.PrjPt);
			double minFrom = _currFIX.CalcConstructionFromMinStablizationDistance((CodeSegmentPath)comboBox208.SelectedItem == CodeSegmentPath.DF);
			if (_leaveFromDir == CodeDirection.BOTH)
				minFrom = Math.Max(minFrom, _currBFIX.CalcConstructionFromMinStablizationDistance());

			_minLegLenght = minFrom + _nextFIX.CalcConstructionInToMinStablizationDistance(_plannedTurn);
			_maxLegLenght = Math.Max(GlobalVars.settings.Radius, currDist);

			_prevAzt = _TrueTrack;
			double analisingRadius = _maxLegLenght + _minLegLenght;
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_circleElem);
			Ring pCircle = ARANFunctions.CreateCirclePrj(GlobalVars.CurrADHP.pPtPrj, analisingRadius);
			_circleElem = GlobalVars.gAranGraphics.DrawRing(pCircle, AranEnvironment.Symbols.eFillStyle.sfsHollow, 255);

			DBModule.FillWPT_FIXList(out GlobalVars.WPTList, GlobalVars.CurrADHP, analisingRadius);

			if (GlobalVars.settings.AnnexObstalce)
				DBModule.GetAnnexObstacle(out GlobalVars.ObstacleList, GlobalVars.CurrADHP.pPtPrj, GlobalVars.CurrADHP.pAirportHeliport);
			else
				DBModule.GetObstaclesByDist(out GlobalVars.ObstacleList, GlobalVars.CurrADHP.pPtPrj, analisingRadius);

			FillComboBox201();

			bool radioButton201Checked = radioButton201.Checked;

			if (comboBox201.Items.Count > 0)
			{
				//comboBox201.SelectedIndex = 0;
				radioButton202.Enabled = true;
			}
			else
			{
				radioButton202.Enabled = false;
				radioButton201.Checked = true;
			}

			if (radioButton201Checked && radioButton201.Checked)
				numericUpDown201_ValueChanged(numericUpDown201);

			return true;
		}

		private void FillComboBox201()
		{
			double prevDir = _currFIX.EntryDirection;
			//prevDir = _currDir;
			//if (_arrivalLegs.Count == 0 && _leaveFromDir == CodeDirection.BACKWARD)
			//	prevDir += Math.PI;

			WPT_FIXType oldFix = default(WPT_FIXType);
			int k = 0;

			if (comboBox201.SelectedIndex >= 0)
				oldFix = (WPT_FIXType)comboBox201.SelectedItem;

			comboBox201.Items.Clear();

			//foreach (var fix in GlobalVars.WPTList)
			//{

			//	double x, y, outDir = ARANFunctions.ReturnAngleInRadians(_startFIX.PrjPt, fix.pPtPrj);
			//	ARANFunctions.PrjToLocal(_startFIX.PrjPt, prevDir, fix.pPtPrj, out x, out y);

			//	TurnDirection EffectiveTurnDirection = TurnDirection.CW;
			//	if (y > 0)
			//		EffectiveTurnDirection = TurnDirection.CCW;

			//	double turnAngle = ARANMath.Modulus((outDir - prevDir) * ((int)EffectiveTurnDirection), ARANMath.C_2xPI);
			//	if (turnAngle > a120)
			//	{
			//		if (_leaveFromDir != CodeDirection.BOTH)
			//			continue;

			//		turnAngle = ARANMath.Modulus((outDir - prevDir - Math.PI) * ((int)EffectiveTurnDirection), ARANMath.C_2xPI);
			//		if (turnAngle > a120)
			//			continue;
			//	}

			//	comboBox201.Items.Add(fix);
			//}

			/*============================================================================================================*/

			Interval outDirRange = new Interval();
			outDirRange.Circular = true;
			outDirRange.InDegree = false;

			if (_PathAndTermination == CodeSegmentPath.DF)
			{
				double a270 = ARANMath.DegToRad(270);

				if (_currFIX.EffectiveTurnDirection == TurnDirection.CW)
				{
					outDirRange.Min = NativeMethods.Modulus(prevDir - a270, ARANMath.C_2xPI);
					outDirRange.Max = prevDir;
				}
				else
				{
					outDirRange.Min = prevDir;
					outDirRange.Max = NativeMethods.Modulus(prevDir + a270, ARANMath.C_2xPI);
				}

				double fTurnDirection = -(int)(_currFIX.EffectiveTurnDirection);
				double r = _currFIX.NomLineTurnRadius;
				Point ptCenter = ARANFunctions.LocalToPrj(_currFIX.NomLinePrjPt, _currFIX.EntryDirection, 0, -r * fTurnDirection);

				//GlobalVars.gAranGraphics.DrawPointWithText(ptCenter, -1, "Center");
				//Application.DoEvents();
				//Leg.ProcessMessages(true);

				foreach (var SigPoint in GlobalVars.WPTList)
				{
					double dX = SigPoint.pPtPrj.X - ptCenter.X;
					double dY = SigPoint.pPtPrj.Y - ptCenter.Y;

					double dirDest = Math.Atan2(dY, dX);
					double distDest = ARANMath.Hypot(dX, dY);

					double TurnAngle = (_currFIX.EntryDirection - dirDest) * fTurnDirection + ARANMath.C_PI_2 - Math.Acos(r / distDest);
					double nomDir = ARANMath.Modulus(_currFIX.EntryDirection - TurnAngle * fTurnDirection, ARANMath.C_2xPI);

					//double MinStabPrev = _currFIX.CalcNomLineFromMinStablizationDistance(TurnAngle, true);
					//double MinStabCurr = _nextFIX.CalcNomLineInToMinStablizationDistance(_plannedTurn);
					double mindist = 1000.0;                    // MinStabPrev + MinStabCurr;

					if (outDirRange.CheckValue(nomDir) == nomDir)
						if (distDest > r && distDest >= mindist && distDest <= _maxLegLenght)
						{
							comboBox201.Items.Add(SigPoint);
							if (SigPoint.Identifier == oldFix.Identifier)
								k = comboBox201.Items.Count - 1;
						}
				}
			}
			else
			{
				double a120 = ARANMath.DegToRad(maxTurnAngle);
				outDirRange.Min = prevDir - a120;
				outDirRange.Max = prevDir + a120;

				foreach (var SigPoint in GlobalVars.WPTList)
				{
					double dX = SigPoint.pPtPrj.X - _currFIX.NomLinePrjPt.X;
					double dY = SigPoint.pPtPrj.Y - _currFIX.NomLinePrjPt.Y;
					double distDest = ARANMath.Hypot(dX, dY);

					double nomDir = ARANMath.Modulus(Math.Atan2(dY, dX), ARANMath.C_2xPI);
					double TurnAngle = ARANMath.Modulus((nomDir - _currFIX.EntryDirection) * ((int)ARANMath.SideFrom2Angle(nomDir, _currFIX.EntryDirection)), 2 * Math.PI);

					//GlobalVars.gAranGraphics.DrawPointWithText(SigPoint.PrjPt, -1, "DD100");
					//GlobalVars.gAranGraphics.DrawPointWithText(_ReferenceFIX.PrjPt, -1, "ReferenceFIX");
					//Leg.ProcessMessages();

					//double MinStabPrev = _currFIX.CalcNomLineFromMinStablizationDistance(TurnAngle);
					//double MinStabCurr = _nextFIX.CalcNomLineInToMinStablizationDistance(_plannedTurn);
					double mindist = 1000.0;                    // MinStabPrev + MinStabCurr;

					if (distDest >= mindist && distDest <= _maxLegLenght)
						if (outDirRange.CheckValue(nomDir) == nomDir)
						{
							comboBox201.Items.Add(SigPoint);
							if (SigPoint.Identifier == oldFix.Identifier)
								k = comboBox201.Items.Count - 1;
						}
				}
			}
			/*============================================================================================================*/

			if (comboBox201.Items.Count > 0)
				comboBox201.SelectedIndex = 0;
		}

		private void CreateSegmentGeometry()
		{
			_DescGr = (_prevAltitude - _currAltitude) / _arrivalFIXDistance;

			//_forwardLeg.Visible = true;
			//_backwardLeg.Visible = true;

			if (_DescGr > maxGrad)
			{
				_DescGr = maxGrad;
				double newAltitude = _prevAltitude - _arrivalFIXDistance * _DescGr;

				textBox205.Text = GlobalVars.unitConverter.HeightToDisplayUnits(newAltitude).ToString();
				textBox205_Validating(textBox205, null);
			}
			else            //??????????????
				textBox206.Text = (100.0 * _DescGr).ToString("0.0");

			double dd = ARANFunctions.ReturnDistanceInMeters(GlobalVars.CurrADHP.pPtPrj, _nextFIX.PrjPt);

			if (dd < Aran.PANDA.Constants.PANSOPSConstantList.PBNTerminalTriggerDistance)
				_nextFIX.FlightPhase = eFlightPhase.STAR;
			else
				_nextFIX.FlightPhase = eFlightPhase.STARGE56;

			_nextFIX.BankAngle = _BankAngle;

			LegArrival prevLeg = _forwardLeg;
			double minTurn = ARANMath.DegToRad(260.0);

			if (_arrivalLegs.Count > 0)
			{
				minTurn = minTurnAngle;
				prevLeg = _arrivalLegs[_arrivalLegs.Count - 1];
			}

			//_currFIX.ReCreateArea();
			//_nextFIX.ReCreateArea();
			_currArrivalLeg.StartFIX.Assign(_currFIX);
			_currArrivalLeg.EndFIX.Assign(_nextFIX);
			_currArrivalLeg.PathAndTermination = _PathAndTermination;
			//_currArrivalLeg.IAS = _IAS;

			_currArrivalLeg.CreateGeometry(prevLeg, GlobalVars.CurrADHP, minTurn);
			_currArrivalLeg.Visible = true;
			_currArrivalLeg.RefreshGraphics();

			/*----------------------------------------------------------------------------------------------------------------------*/
			ObstacleContainer Obstacles;
			int inx = Functions.GetLegObstList(GlobalVars.ObstacleList, out Obstacles, _currArrivalLeg, _currMOCLimit);
			_currArrivalLeg.Obstacles = Obstacles;

			if (inx >= 0)
				_currArrivalLeg.DetObstacle_2 = Obstacles.Parts[inx];
			else
				_currArrivalLeg.DetObstacle_2 = new ObstacleData();
			/*----------------------------------------------------------------------------------------------------------------------*/

			if (_arrivalLegs.Count == 0 && _leaveFromDir == CodeDirection.BOTH)
			{
				_currArrivalLegB.StartFIX.Assign(_currBFIX);
				_currArrivalLegB.EndFIX.Assign(_nextFIX);
				_currArrivalLegB.PathAndTermination = _PathAndTermination;

				_currArrivalLegB.CreateGeometry(_backwardLeg, GlobalVars.CurrADHP, minTurn);
				_currArrivalLegB.RefreshGraphics();

				/*----------------------------------------------------------------------------------------------------------------------*/

				inx = Functions.GetLegObstList(GlobalVars.ObstacleList, out Obstacles, _currArrivalLegB, _currMOCLimit);
				_currArrivalLegB.Obstacles = Obstacles;

				if (inx >= 0)
					_currArrivalLegB.DetObstacle_2 = Obstacles.Parts[inx];
				else
					_currArrivalLegB.DetObstacle_2 = new ObstacleData();
				/*----------------------------------------------------------------------------------------------------------------------*/
				_reportForm.FillCurrentLeg(_currArrivalLeg, _currArrivalLegB);
			}
			else
				_reportForm.FillCurrentLeg(_currArrivalLeg);
		}

		#region Direction

		private void FillComboBox203()
		{
			double yToler = 100.0;

			double minFrom = _currFIX.CalcConstructionFromMinStablizationDistance((CodeSegmentPath)comboBox208.SelectedItem == CodeSegmentPath.DF);

			if (_arrivalLegs.Count == 0 && _leaveFromDir == CodeDirection.BOTH)
				minFrom = Math.Max(minFrom, _currBFIX.CalcConstructionFromMinStablizationDistance());

			_minLegLenght = minFrom + _nextFIX.CalcConstructionInToMinStablizationDistance(_plannedTurn);

			WPT_FIXType oldFix = default(WPT_FIXType);

			if (comboBox203.SelectedIndex >= 0)
				oldFix = (WPT_FIXType)comboBox203.SelectedItem;

			comboBox203.Items.Clear();

			Point ptFrom = _currFIX.PrjPt;      //_currArrivalLeg.StartFIX.PrjPt;// 

			//GlobalVars.gAranGraphics.DrawPointWithText(ptFrom, -1, "ptFrom_0");
			//GlobalVars.gAranGraphics.DrawPointWithText(_currFIX.PrjPt, -1, "ptFrom_1");
			//Application.DoEvents();

			if (_PathAndTermination == CodeSegmentPath.DF)  //_nextFIX.IsDFTarget
			{
				TurnDirection TurnDir = _currFIX.EffectiveTurnDirection;
				double fTurnSide = (int)TurnDir;
				double r = _currFIX.NomLineTurnRadius;
				Point ptCnt = ARANFunctions.LocalToPrj(_currFIX.NomLinePrjPt, _currFIX.EntryDirection, 0.0, r * fTurnSide);
				ptFrom = ARANFunctions.LocalToPrj(ptCnt, _nextFIX.EntryDirection, 0.0, -r * fTurnSide);
				yToler = 0.5 * _currFIX.XTT;

				//GlobalVars.gAranGraphics.DrawPointWithText(ptFrom, -1, "ptFrom");
				//GlobalVars.gAranGraphics.DrawPointWithText(ptCnt, -1, "ptCnt");
				//LegBase.ProcessMessages();

				//LegBase.ProcessMessages(true);

				//MinDist = Math.Max(r, _DistanceRange.Min);
			}
			else if ((_currFIX.FlyMode == eFlyMode.Flyover || _currFIX.FlyMode == eFlyMode.Atheight) && _PathAndTermination == CodeSegmentPath.CF)
			{
				double fTurnDir = (int)_currFIX.EffectiveTurnDirection;             //SideDef(_ReferenceFIX.PrjPt, _ReferenceFIX.EntryDir, _CurrFIX.PrjPt);

				double DivergenceAngle30 = GlobalVars.constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;
				double Bank15 = GlobalVars.constants.Pansops[ePANSOPSData.rnvFlyOInterBank].Value;
				double R1 = _currFIX.NomLineTurnRadius;
				double R2 = ARANMath.BankToRadius(Bank15, _currFIX.NomLineTAS);
				double fTmp0 = Math.Acos((1 + R1 * Math.Cos(_currFIX.TurnAngle) / R2) / (1 + R1 / R2)) - ARANMath.EpsilonRadian;
				double DivergenceAngle = Math.Min(DivergenceAngle30, fTmp0);

				Point ptCnt = ARANFunctions.LocalToPrj(_currFIX.NomLinePrjPt, _currFIX.EntryDirection + ARANMath.C_PI_2 * fTurnDir, R1, 0);
				fTmp0 = _currFIX.EntryDirection + (_currFIX.TurnAngle + DivergenceAngle) * fTurnDir;

				Point ptRollOut1 = ARANFunctions.LocalToPrj(ptCnt, fTmp0 - ARANMath.C_PI_2 * fTurnDir, R1);

				if (ARANMath.SubtractAngles(fTmp0, _nextFIX.EntryDirection) > ARANMath.EpsilonRadian)
				{
					Point ptInter = (Point)ARANFunctions.LineLineIntersect(ptRollOut1, fTmp0, _nextFIX.PrjPt, _nextFIX.EntryDirection);
					double fTmp1 = R2 * Math.Tan(0.5 * DivergenceAngle);
					ptFrom = ARANFunctions.LocalToPrj(ptInter, _nextFIX.EntryDirection, fTmp1);
				}
			}

			double distCorrection = ARANFunctions.ReturnDistanceInMeters(_currFIX.NomLinePrjPt, ptFrom);
			double MaxDist = _maxLegLenght;
			double MinDist = _minLegLenght - distCorrection;

			//ring.Add(ARANFunctions.LocalToPrj(_currSeg.Start.PrjPt, _currDir, -0.1, 100.0));
			int k = 0;
			foreach (var fix in GlobalVars.WPTList)
			{
				double x, y;
				ARANFunctions.PrjToLocal(ptFrom, _nextFIX.EntryDirection, fix.pPtPrj, out x, out y);

				if (x >= MinDist && x <= MaxDist && Math.Abs(y) <= yToler)
				{
					comboBox203.Items.Add(fix);
					if (fix.Identifier == oldFix.Identifier)
						k = comboBox203.Items.Count - 1;
				}
			}

			radioButton204.Enabled = comboBox203.Items.Count > 0;

			if (radioButton204.Enabled)
				comboBox203.SelectedIndex = k;
		}

		private void DirectionChanged(double newDir)
		{
			_currArrivalLeg.Visible = false;
			//Application.DoEvents();
			//_arrivalLegs[_arrivalLegs.Count - 1].EndFIX.ReCreateArea();

			if (_arrivalLegs.Count == 0 && _leaveFromDir == CodeDirection.BOTH)
				_currArrivalLegB.Visible = false;

			_currFIX.OutDirection = newDir;
			if (_arrivalLegs.Count > 0)
				_arrivalLegs[_arrivalLegs.Count - 1].EndFIX.OutDirection = newDir;

			if (_arrivalLegs.Count == 0 && _leaveFromDir == CodeDirection.BOTH)
				_currBFIX.OutDirection = newDir;

			_nextFIX.EntryDirection = newDir;
			_nextFIX.OutDirection = newDir;

			FillComboBox203();

			if (!radioButton204.Enabled)
				radioButton203.Checked = true;

			if (radioButton203.Checked)
			{
				textBox201.Tag = null;
				textBox201_Validating(textBox201);
			}

			_currArrivalLeg.Visible = true;

			if (_arrivalLegs.Count == 0 && _leaveFromDir == CodeDirection.BOTH)
				_currArrivalLegB.Visible = true;
		}

		private void radioButton201_CheckedChanged(object sender, EventArgs e)
		{
			if (!((RadioButton)sender).Checked)
				return;

			if (sender == radioButton201)
			{
				numericUpDown201.Enabled = true;
				comboBox201.Enabled = false;

				numericUpDown201_ValueChanged(numericUpDown201);
			}
			else
			{
				numericUpDown201.Enabled = false;

				comboBox201.Enabled = true;
				comboBox201_SelectedIndexChanged(comboBox201);
			}
		}

		private void numericUpDown201_ValueChanged(object sender, EventArgs e = null)
		{
			if (numericUpDown201.Value < 0)
			{
				numericUpDown201.Value += 360;
				return;
			}

			if (numericUpDown201.Value >= 360)
			{
				numericUpDown201.Value -= 360;
				return;
			}

			//Application.DoEvents();
			//_currArrivalLeg.StartFIX.RefreshGraphics();
			//_arrivalLegs[_arrivalLegs.Count - 1].StartFIX.RefreshGraphics();
			//_arrivalLegs[_arrivalLegs.Count - 1].EndFIX.ReCreateArea();
			//_arrivalLegs[_arrivalLegs.Count - 1].EndFIX.RefreshGraphics();
			//LegBase.ProcessMessages();

			double nnAzt = (double)numericUpDown201.Value;
			double azimuth = nnAzt;

			if (_arrivalLegs.Count == 0)
			{
				double dMinF = double.MaxValue, dMaxF = double.MaxValue, dMinB = double.MaxValue, dMaxB = double.MaxValue;
				double minAzimuthF = 0.0, maxAzimuthF = 0.0, minAzimuthB = 0.0, maxAzimuthB = 0.0;

				double dMin = double.MaxValue;

				if (_leaveFromDir == CodeDirection.FORWARD || _leaveFromDir == CodeDirection.BOTH)
				{
					minAzimuthF = (double)((decimal)NativeMethods.Modulus(_prevAzt - maxTurnAngle));
					maxAzimuthF = (double)((decimal)NativeMethods.Modulus(_prevAzt + maxTurnAngle));

					if (!ARANFunctions.AngleInSector(nnAzt, minAzimuthF, maxAzimuthF))
					{
						dMinF = ARANMath.SubtractAnglesInDegs(minAzimuthF, nnAzt);
						dMaxF = ARANMath.SubtractAnglesInDegs(maxAzimuthF, nnAzt);

						if (dMinF < dMaxF)
						{
							dMin = dMinF;
							azimuth = minAzimuthF;
						}
						else
						{
							dMin = dMaxF;
							azimuth = maxAzimuthF;
						}
					}
				}

				if (_leaveFromDir == CodeDirection.BACKWARD || _leaveFromDir == CodeDirection.BOTH)
				{
					double BackTrack = ARANFunctions.DirToAzimuth(_currFIX.PrjPt, _currDir + Math.PI, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

					minAzimuthB = (double)((decimal)NativeMethods.Modulus(BackTrack - maxTurnAngle));
					maxAzimuthB = (double)((decimal)NativeMethods.Modulus(BackTrack + maxTurnAngle));

					if (!ARANFunctions.AngleInSector(nnAzt, minAzimuthB, maxAzimuthB))
					{
						dMinB = ARANMath.SubtractAnglesInDegs(minAzimuthB, nnAzt);
						dMaxB = ARANMath.SubtractAnglesInDegs(maxAzimuthB, nnAzt);

						if (dMinB < dMaxB)
						{
							if (dMinB < dMin)
								azimuth = minAzimuthB;
						}
						else if (dMaxB < dMin)
							azimuth = maxAzimuthB;
					}
				}
			}
			else
			{
				double TurnAngle = ARANMath.SubtractAnglesInDegs(nnAzt, _prevAzt);
				if (TurnAngle > maxTurnAngle)
				{
					double minAzimuth = NativeMethods.Modulus(_prevAzt - maxTurnAngle);
					double maxAzimuth = NativeMethods.Modulus(_prevAzt + maxTurnAngle);

					if (!ARANFunctions.AngleInSector(nnAzt, minAzimuth, maxAzimuth))
					{
						double dMin = ARANMath.SubtractAnglesInDegs(minAzimuth, nnAzt);
						double dMax = ARANMath.SubtractAnglesInDegs(maxAzimuth, nnAzt);

						if (dMin < dMax)
							azimuth = minAzimuth;
						else
							azimuth = maxAzimuth;
					}
				}
			}

			if (azimuth != nnAzt)
			{
				numericUpDown201.Value = (decimal)azimuth;
				return;
			}

			double newDir = ARANFunctions.AztToDirection(_currFIX.GeoPt, nnAzt, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);

			DirectionChanged(newDir);
		}

		private void comboBox201_SelectedIndexChanged(object sender, EventArgs e = null)
		{
			if (comboBox201.SelectedIndex < 0)
				return;

			WPT_FIXType sigPoint = (WPT_FIXType)comboBox201.SelectedItem;
			label202.Text = sigPoint.TypeCode.Tostring();

			if (!comboBox201.Enabled)
				return;

			if (radioButton201.Checked)
				return;

			double newDir, newAzt;

			if (_PathAndTermination == CodeSegmentPath.DF)
			{
				//_currFIX.TurnDirection = comboBox202.SelectedIndex == 0 ? TurnDirection.CCW : TurnDirection.CW;
				double fTurnDirection = -(int)_currFIX.EffectiveTurnDirection;

				//double fTurnDirection = -(int)(_currFIX.TurnDirection);
				double r = _currFIX.NomLineTurnRadius;
				Point ptCnt = ARANFunctions.LocalToPrj(_currFIX.PrjPt, _currFIX.EntryDirection, 0, -r * fTurnDirection);

				double dX = sigPoint.pPtPrj.X - ptCnt.X;
				double dY = sigPoint.pPtPrj.Y - ptCnt.Y;

				double distDest = ARANMath.Hypot(dX, dY);
				double dirDest = Math.Atan2(dY, dX);

				double TurnAngle = (_currFIX.EntryDirection - dirDest) * fTurnDirection + ARANMath.C_PI_2 - Math.Acos(r / distDest);

				newDir = ARANMath.Modulus(_currFIX.EntryDirection - TurnAngle * fTurnDirection, ARANMath.C_2xPI);

				//Point ptFrom = ARANFunctions.LocalToPrj(ptCnt, newDir, 0.0, -r * fTurnDirection);   //_currFIX.OutDirection
				//Point ptFrom = ARANFunctions.LocalToPrj(ptCnt, _currFIX.EntryDirection, 0.0, -r * fTurnDirection);
				//Point ptFrom = ARANFunctions.LocalToPrj(ptCnt, newDir, 0.0, r * fTurnDirection);   //_currFIX.OutDirection

				//GlobalVars.gAranGraphics.DrawPointWithText(ptFrom, -1, "ptFrom-3");
				//GlobalVars.gAranGraphics.DrawPointWithText(ptCnt, -1, "ptCnt");
				//GlobalVars.gAranGraphics.DrawPointWithText(sigPoint.pPtPrj, -1, sigPoint.Name);
				//LegBase.ProcessMessages();
			}
			else
				newDir = Math.Atan2(sigPoint.pPtPrj.Y - _currFIX.PrjPt.Y, sigPoint.pPtPrj.X - _currFIX.PrjPt.X);

			newAzt = ARANFunctions.DirToAzimuth(_currFIX.PrjPt, newDir, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			numericUpDown201.Value = (decimal)newAzt;
			newAzt = (double)numericUpDown201.Value;

			newDir = ARANFunctions.AztToDirection(_currFIX.GeoPt, newAzt, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);

			DirectionChanged(newDir);
		}

		private void comboBox202_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_PathAndTermination == CodeSegmentPath.DF)
			{
				_currFIX.TurnDirection = comboBox202.SelectedIndex == 0 ? TurnDirection.CCW : TurnDirection.CW;
				FillComboBox201();

				if (!comboBox201.Enabled)
					numericUpDown201_ValueChanged(numericUpDown201);
				//CreateSegmentGeometry();
			}
		}

		#endregion

		#region Distance

		private void radioButton203_CheckedChanged(object sender, EventArgs e)
		{
			if (!((RadioButton)sender).Checked)
				return;

			if (sender == radioButton203)
			{
				textBox201.ReadOnly = false;
				textBox202.ReadOnly = false;
				comboBox203.Enabled = false;
				textBox202.Text = _arrivalFIXName;
				_nextFIX.Name = textBox202.Text;
				_nextFIX.CallSign = _nextFIX.Name;
				_nextFIX.Id = Guid.Empty;

				textBox201.Tag = null;
				textBox201_Validating(textBox201);
			}
			else
			{
				textBox201.ReadOnly = true;
				textBox202.ReadOnly = true;

				comboBox203.Enabled = true;
				comboBox203_SelectedIndexChanged(comboBox203);
			}
		}

		private void comboBox203_SelectedIndexChanged(object sender, EventArgs e = null)
		{
			if (comboBox203.SelectedIndex < 0)
				return;

			WPT_FIXType fix = (WPT_FIXType)comboBox203.SelectedItem;
			label205.Text = fix.TypeCode.Tostring();

			if (!radioButton204.Checked)
				return;

			_nextFIX.Name = fix.Name;
			_nextFIX.CallSign = fix.CallSign;
			_nextFIX.Id = fix.Identifier;

			_nextFIX.PrjPt = fix.pPtPrj;

			textBox202.Text = comboBox203.Text;

			double dist;

			if (_PathAndTermination == CodeSegmentPath.DF)
			{
				//_currFIX.TurnDirection = comboBox202.SelectedIndex == 0 ? TurnDirection.CCW : TurnDirection.CW;
				double fTurnDirection = (int)_currFIX.EffectiveTurnDirection;

				double r = _currFIX.NomLineTurnRadius;
				Point ptCenter = ARANFunctions.LocalToPrj(_currFIX.NomLinePrjPt, _currFIX.EntryDirection, 0, r * fTurnDirection);
				Point ptFrom = ARANFunctions.LocalToPrj(ptCenter, _currFIX.OutDirection, 0.0, -r * fTurnDirection);

				//GlobalVars.gAranGraphics.DrawPointWithText(ptCenter, -1, "ptCenter");
				//GlobalVars.gAranGraphics.DrawPointWithText(ptFrom, -1, "ptFrom");
				//LegBase.ProcessMessages();

				dist = ARANMath.Hypot(_nextFIX.PrjPt.Y - ptFrom.Y, _nextFIX.PrjPt.X - ptFrom.X);
			}
			else
				dist = ARANMath.Hypot(_nextFIX.PrjPt.Y - _currFIX.NomLinePrjPt.Y, _nextFIX.PrjPt.X - _currFIX.NomLinePrjPt.X);

			_arrivalFIXDistance = dist;                     // ARANFunctions.ReturnDistanceInMeters(_currFIX.PrjPt, _nextFIX.PrjPt);

			textBox201.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_arrivalFIXDistance).ToString();

			//_nextFIX.RefreshGraphics();
			CreateSegmentGeometry();                        // DistanceChanged(_FIXDistance01);
		}

		private void numericUpDown202_ValueChanged(object sender, EventArgs e)
		{
			double plannedTurn = ARANMath.DegToRad((double)numericUpDown202.Value);
			if (plannedTurn != _plannedTurn)
			{
				_plannedTurn = plannedTurn;
				DirectionChanged(_currFIX.OutDirection);
			}
		}

		private void textBox201_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox201_Validating(textBox201, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox201.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox201_Validating(object sender, System.ComponentModel.CancelEventArgs e = null)
		{
			if (!radioButton203.Checked)
				return;

			double fTmp;
			bool update = false;

			if (double.TryParse(textBox201.Text, out fTmp))
			{
				fTmp = _arrivalFIXDistance = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

				if (_arrivalFIXDistance < _minLegLenght)
					_arrivalFIXDistance = _minLegLenght;

				if (_arrivalFIXDistance > _maxLegLenght)
					_arrivalFIXDistance = _maxLegLenght;

				if (_arrivalFIXDistance != fTmp)
					textBox201.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_arrivalFIXDistance).ToString();

				if (textBox201.Tag != null && textBox201.Tag.ToString() == textBox201.Text)
					return;

				update = true;
			}
			else if (double.TryParse(textBox201.Tag.ToString(), out fTmp))
				textBox201.Text = textBox201.Tag.ToString();
			else
				textBox201.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_arrivalFIXDistance).ToString();

			textBox201.Tag = textBox201.Text;

			if (!update)
				return;

			//====================================================================================================================================
			Point FIXPoint;

			if (_nextFIX.IsDFTarget)
			{
				TurnDirection TurnDir = _currFIX.EffectiveTurnDirection;
				double fTurnSide = (int)TurnDir;
				double r = _currFIX.NomLineTurnRadius;
				Point ptCnt = ARANFunctions.LocalToPrj(_currFIX.NomLinePrjPt, _currFIX.EntryDirection, 0.0, r * fTurnSide);
				Point ptFrom = ARANFunctions.LocalToPrj(ptCnt, _nextFIX.EntryDirection, 0.0, -r * fTurnSide);

				//ptFrom = ARANFunctions.LocalToPrj(ptCnt, _nextFIX.EntryDirection, 0.0, -r * fTurnSide);

				//if (_DistanceIndex < 0)
				//{

				//GlobalVars.gAranGraphics.DrawPointWithText(ptFrom, -1, "ptFrom");
				//GlobalVars.gAranGraphics.DrawPointWithText(ptCnt, -1, "ptCnt");
				//GlobalVars.gAranGraphics.DrawPointWithText(_currFIX.NomLinePrjPt, -1, "NomLinePrjPt");
				//GlobalVars.gAranGraphics.DrawPointWithText(_currFIX.PrjPt, -1, "PrjPt");

				//WPT_FIXType sigPoint = (WPT_FIXType)comboBox201.SelectedItem;

				//GlobalVars.gAranGraphics.DrawPointWithText(sigPoint.pPtPrj, -1, sigPoint.Name);
				//LegBase.ProcessMessages();

				//if (_PathAndTermination == CodeSegmentPath.DF)

				FIXPoint = ARANFunctions.LocalToPrj(ptFrom, _nextFIX.EntryDirection, _arrivalFIXDistance, 0.0);
				//else
				//	FIXPoint = ARANFunctions.CircleVectorIntersect(_currFIX.NomLinePrjPt, _arrivalFIXDistance, ptFrom, _nextFIX.EntryDirection);

				//GlobalVars.gAranGraphics.DrawPointWithText(FIXPoint, -1, "FIXPoint ");
				//LegBase.ProcessMessages();

				_nextFIX.PrjPt = FIXPoint;

				//GlobalVars.gAranGraphics.DrawPointWithText(FIXPoint, -1, "FIXPoint");
				//LegBase.ProcessMessages();

				//}
				//else
				//	FIXPoint = _CurrFIX.PrjPt;

				//if (TurnDir == TurnDirection.CW)
				//{
				//	_nextFIX.OutDirection = _nextFIX.EntryDirection + _plannedTurnLimits.Max;   // ARANMath.C_PI
				//	_nextFIX.EntryDirection_L = _currFIX.CalcDFOuterDirection(_nextFIX, out ptOutOutBase);
				//	_nextFIX.EntryDirection_R = _currFIX.CalcDFInnerDirection(_nextFIX, out ptInOutBase, _prevLeg);
				//}
				//else
				//{
				//	_nextFIX.OutDirection = _nextFIX.EntryDirection - _plannedTurnLimits.Max;   // ARANMath.C_PI
				//	_nextFIX.EntryDirection_R = _currFIX.CalcDFOuterDirection(_nextFIX, out ptOutOutBase);
				//	_nextFIX.EntryDirection_L = _currFIX.CalcDFInnerDirection(_nextFIX, out ptInOutBase, _prevLeg);
				//}
			}
			else
			{
				//if (_distanceIndex < 0)
				//{
				FIXPoint = ARANFunctions.LocalToPrj(_currFIX.NomLinePrjPt, _currFIX.OutDirection, _arrivalFIXDistance, 0.0);

				//GlobalVars.gAranGraphics.DrawPointWithText(FIXPoint, -1, "FIXPoint");
				//LegBase.ProcessMessages();

				_nextFIX.PrjPt = FIXPoint;

				//}
				//else
				//	FIXPoint = _nextFIX.PrjPt;

				//GlobalVars.gAranGraphics.DrawPointWithText(FIXPoint, -1, "FIXPoint");
				//LegBase.ProcessMessages();

				//if (_nextFIX.FlyMode == eFlyMode.Flyover)
				//	_nextFIX.OutDirection = _nextFIX.EntryDirection + _plannedTurnLimits.Max;   // ARANMath.C_PI
				//else
				//	_nextFIX.OutDirection = _nextFIX.EntryDirection;    // ARANMath.C_PI
			}

			double fDist = ARANMath.Hypot(FIXPoint.X - GlobalVars.CurrADHP.pPtPrj.X, FIXPoint.Y - GlobalVars.CurrADHP.pPtPrj.Y);

			if (_currFIX.FlightPhase == eFlightPhase.STAR)
			{
				if (fDist >= PANSOPSConstantList.PBNTerminalTriggerDistance)
					_nextFIX.FlightPhase = eFlightPhase.STARGE56;
			}
			else if (_currFIX.FlightPhase >= eFlightPhase.STARGE56)
			{
				if (fDist < PANSOPSConstantList.PBNTerminalTriggerDistance)
					_nextFIX.FlightPhase = eFlightPhase.STAR;
			}

			_nextFIX.BankAngle = _BankAngle;

			//====================================================================================================================================

			//_nextFIX.PrjPt = ARANFunctions.LocalToPrj(_nextFIX.PrjPt, _nextFIX.OutDirection, _arrivalFIXDistance);

			//_nextFIX.RefreshGraphics();
			//Application.DoEvents();

			CreateSegmentGeometry();
		}

		private void textBox202_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox202_Validating(textBox202, new System.ComponentModel.CancelEventArgs());
			else
			{
				if (eventChar >= ' ')
				{
					if (eventChar < '0' || eventChar > '9')         // || eventChar != '_')
					{
						char alfa = (char)(eventChar & ~32);
						if (alfa < 'A' || alfa > 'Z')
							eventChar = '\0';
						else
							eventChar = alfa;
					}
				}
				else
					eventChar = '\0';
			}

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox202_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_arrivalFIXName = textBox202.Text;
			_nextFIX.Name = _arrivalFIXName;
			_nextFIX.CallSign = _arrivalFIXName;
			_nextFIX.RefreshGraphics();
		}

		#endregion

		#region WPT construction parameters

		private void textBox203_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox203_Validating(textBox203, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox203.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox203_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			double fTmp, newIAS;
			bool update = false;

			if (double.TryParse(textBox203.Text, out fTmp))
			{
				if (textBox203.Tag != null && textBox203.Tag.ToString() == textBox203.Text)
					return;

				newIAS = fTmp = GlobalVars.unitConverter.SpeedToInternalUnits(fTmp);

				if (newIAS < _minIAS)
					newIAS = _minIAS;

				//if (newIAS > _maxIAS)
				//	newIAS = _maxIAS;

				if (newIAS > _currFIX.IAS)
					newIAS = _currFIX.IAS;


				if (newIAS != fTmp)
					textBox203.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(newIAS).ToString();

				if (_IAS != newIAS)
				{
					_IAS = newIAS;
					_nextFIX.IAS = _IAS;
					//_currArrivalLeg.IAS = _IAS;

					textBox204.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_nextFIX.ConstructTAS).ToString();
					update = true;
				}
			}
			else if (double.TryParse(textBox203.Tag.ToString(), out fTmp))
				textBox203.Text = textBox203.Tag.ToString();
			else
				textBox203.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_IAS).ToString();

			textBox203.Tag = textBox203.Text;

			if (!update)
				return;

			//if (_arrivalLegs.Count == 0)
			//{
			//	_currFIX.IAS = _nextFIX.IAS;
			//	if (_leaveFromDir == CodeDirection.BOTH)
			//	{
			//		_currBFIX.IAS = _nextFIX.IAS;
			//		//_currArrivalLegB.IAS = _currBFIX.IAS;
			//	}
			//}

			FillComboBox203();

			if (!radioButton204.Enabled)
				radioButton203.Checked = true;

			if (radioButton203.Checked)
			{
				textBox201.Tag = null;
				textBox201_Validating(textBox201);
			}
		}

		private void textBox205_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox205_Validating(textBox205, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox205.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox205_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			double fTmp, newAltitude;
			bool update = false;

			if (double.TryParse(textBox205.Text, out fTmp))
			{
				if (textBox205.Tag != null && textBox205.Tag.ToString() == textBox205.Text)
					return;

				newAltitude = fTmp = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);

				_DescGr = (_prevAltitude - newAltitude) / _arrivalFIXDistance;

				if (_DescGr > maxGrad)
				{
					_DescGr = maxGrad;
					newAltitude = _prevAltitude - _arrivalFIXDistance * _DescGr;
				}
				textBox206.Text = (100.0 * _DescGr).ToString("0.0");

				if (newAltitude < _minAlltitude)
					newAltitude = _minAlltitude;

				if (newAltitude > _maxAlltitude)
					newAltitude = _maxAlltitude;

				if (newAltitude != fTmp)
					textBox205.Text = GlobalVars.unitConverter.HeightToDisplayUnits(newAltitude).ToString();

				if (_currAltitude != newAltitude)
				{
					_currAltitude = newAltitude;
					_nextFIX.ConstructAltitude = _currAltitude;
					textBox204.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_nextFIX.ConstructTAS).ToString();
					update = true;
				}
			}
			else if (double.TryParse(textBox205.Tag.ToString(), out fTmp))
				textBox205.Text = textBox205.Tag.ToString();
			else
				textBox205.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_currAltitude).ToString();

			textBox205.Tag = textBox205.Text;

			if (!update)
				return;

			//double DescGr = (_prevAltitude - _Altitude) / _arrivalFIXDistance;
			//if(DescGr)
			//CreateSegmentGeometry();

			FillComboBox203();

			if (!radioButton204.Enabled)
				radioButton203.Checked = true;

			if (radioButton203.Checked)
			{
				textBox201.Tag = null;
				textBox201_Validating(textBox201);
			}
		}

		private void comboBox204_SelectedIndexChanged(object sender, EventArgs e)
		{
			_BankAngle = ARANMath.DegToRad((double)comboBox204.SelectedItem);

			if (CurrPage != 1) return;

			_nextFIX.BankAngle = _BankAngle;

			if (_arrivalLegs.Count == 0)
			{
				_currFIX.BankAngle = _nextFIX.BankAngle;
				if (_leaveFromDir == CodeDirection.BOTH)
					_currBFIX.BankAngle = _nextFIX.BankAngle;
			}

			FillComboBox203();

			if (!radioButton204.Enabled)
				radioButton203.Checked = true;

			if (radioButton203.Checked)
			{
				textBox201.Tag = null;
				textBox201_Validating(textBox201);
			}
		}

		private void comboBox205_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (CurrPage != 1) return;

			eSensorType sensorType = (eSensorType)comboBox205.SelectedItem;
			if (_nextFIX.SensorType != sensorType)
			{
				_nextFIX.SensorType = sensorType;
				CreateSegmentGeometry();
			}
		}

		private void checkBox201_CheckedChanged(object sender, EventArgs e)
		{
			if (CurrPage != 1) return;

			_nextFIX.MultiCoverage = checkBox201.Checked;
			CreateSegmentGeometry();
		}

		private void comboBox206_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (CurrPage != 1) return;

			eFlyMode flyMode = (eFlyMode)comboBox206.SelectedIndex;
			if (_nextFIX.FlyMode != flyMode)
			{
				_nextFIX.FlyMode = flyMode;
				CreateSegmentGeometry();
			}
		}

		private void comboBox207_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (CurrPage != 1) return;

			ePBNClass PBNClass = (ePBNClass)comboBox207.SelectedItem;

			if (_nextFIX.PBNType != PBNClass)
			{
				_nextFIX.PBNType = PBNClass;
				CreateSegmentGeometry();
			}
		}

		private void comboBox208_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (CurrPage != 1)
				return;

			CodeSegmentPath pathAndTermination = (CodeSegmentPath)comboBox208.SelectedItem;

			if (_PathAndTermination != pathAndTermination)
			{
				_PathAndTermination = pathAndTermination;

				comboBox202.Visible = _PathAndTermination == CodeSegmentPath.DF;
				label203.Visible = comboBox202.Visible;

				/**/
				//double nnAzt = ARANFunctions.DirToAzimuth(_currFIX.PrjPt, _currFIX.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

				if (_PathAndTermination == CodeSegmentPath.DF)
				{
					_currFIX.TurnDirection = comboBox202.SelectedIndex == 0 ? TurnDirection.CCW : TurnDirection.CW;
					//UpDown301Range.Min = ARANMath.Modulus(nnAzt - 270.0, 360.0);
					//UpDown301Range.Max = ARANMath.Modulus(nnAzt + 270.0, 360.0);
				}
				else
				{
					_currFIX.TurnDirection = TurnDirection.NONE;
					//UpDown301Range.Min = ARANMath.Modulus(nnAzt - 120.0, 360.0);
					//UpDown301Range.Max = ARANMath.Modulus(nnAzt + 120.0, 360.0);
				}
				/**/

				_nextFIX.IsDFTarget = _PathAndTermination == CodeSegmentPath.DF;
				//_PathAndTermination = pathAndTermination;

				FillComboBox201();
				if (!comboBox201.Enabled)
					numericUpDown201_ValueChanged(numericUpDown201);
				//CreateSegmentGeometry();
			}
		}

		private void comboBox209_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (CurrPage != 1) return;

			_currMOCLimit = GlobalVars.unitConverter.HeightToInternalUnits((double)comboBox209.SelectedItem);
		}

		#endregion

		private void button201_Click(object sender, EventArgs e)
		{
			FIXInfo.ShowFixInfo(Left + tabControl1.Left + tabPage2.Left + button201.Left, Top + tabPage2.Top + button201.Top, _currFIX);
		}

		#region Segments menegment

		private void button202_Click(object sender, EventArgs e)
		{
			screenCapture.Save(this);

			double prevDir = _nextFIX.EntryDirection;
			LegArrival prevLeg = _currArrivalLeg;

			_prevAzt = ARANFunctions.DirToAzimuth(_nextFIX.PrjPt, prevDir, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			_prevAltitude = _currAltitude;

			//_currFIX = _nextFIX;
			_currFIX = new FIX(GlobalVars.gAranEnv);
			_currFIX.Assign(_nextFIX);

			//Application.DoEvents();
			//_nextFIX.RefreshGraphics();
			//Application.DoEvents();

			_nextFIX = new FIX(GlobalVars.gAranEnv);
			//_nextFIX.Assign(_currFIX);

			_arrivalFIXName = Resources.str01050 + (_arrivalLegs.Count + 2).ToString("00");
			textBox202.Text = _arrivalFIXName;

			_nextFIX.Name = _arrivalFIXName;                        //	textBox202.Text;
			_nextFIX.PBNType = (ePBNClass)comboBox207.SelectedItem;
			_nextFIX.BankAngle = ARANMath.DegToRad((double)comboBox204.SelectedItem);
			_nextFIX.SensorType = (eSensorType)comboBox205.SelectedItem;
			_nextFIX.FlyMode = (eFlyMode)comboBox206.SelectedIndex;
			_nextFIX.MultiCoverage = checkBox201.Checked;
			_nextFIX.FlightPhase = _currFIX.FlightPhase;            //	eFlightPhase.STARGE56;
			_nextFIX.ConstructAltitude = _currAltitude;
			_nextFIX.IAS = _IAS;

			_currArrivalLeg.Active = true;
			_currArrivalLeg.Gradient = _DescGr;

			if (_arrivalLegs.Count == 0 && _leaveFromDir == CodeDirection.BOTH)
				_currArrivalLegB.Active = true;
			//	Application.DoEvents();

			_arrivalLegs.Add(_currArrivalLeg);

			_currArrivalLeg = new LegArrival(_currFIX, _nextFIX, GlobalVars.gAranEnv, prevLeg);
			_currArrivalLeg.Active = false;

			//prevLeg.EndFIX.ReCreateArea();
			//prevLeg.EndFIX.RefreshGraphics();
			//Application.DoEvents();

			//_arrivalLegs[_arrivalLegs.Count - 1].EndFIX.ReCreateArea();
			//_arrivalLegs[_arrivalLegs.Count - 1].EndFIX.RefreshGraphics();
			//Application.DoEvents();

			//==================================================================================
			double minFrom = _currFIX.CalcConstructionFromMinStablizationDistance((CodeSegmentPath)comboBox208.SelectedItem == CodeSegmentPath.DF);
			_minLegLenght = minFrom + _nextFIX.CalcConstructionInToMinStablizationDistance(_plannedTurn);

			FillComboBox201();

			//comboBox201.Items.Clear();
			//foreach (var fix in GlobalVars.WPTList)
			//{
			//	double x, y, outDir = ARANFunctions.ReturnAngleInRadians(_currFIX.PrjPt, fix.pPtPrj);
			//	ARANFunctions.PrjToLocal(_currFIX.PrjPt, prevDir, fix.pPtPrj, out x, out y);

			//	TurnDirection EffectiveTurnDirection = TurnDirection.CW;
			//	if (y > 0)
			//		EffectiveTurnDirection = TurnDirection.CCW;

			//	double turnAngle = ARANMath.Modulus((outDir - prevDir) * ((int)EffectiveTurnDirection), ARANMath.C_2xPI);
			//	if (turnAngle > _plannedTurn)
			//	{
			//		if (_leaveFromDir != CodeDirection.BOTH)
			//			continue;

			//		turnAngle = ARANMath.Modulus((outDir - prevDir - Math.PI) * ((int)EffectiveTurnDirection), ARANMath.C_2xPI);
			//		if (turnAngle > _plannedTurn)
			//			continue;
			//	}

			//	comboBox201.Items.Add(fix);
			//}

			bool radioButton201Checked = radioButton201.Checked;

			if (comboBox201.Items.Count > 0)
			{
				//comboBox201.SelectedIndex = 0;
				radioButton202.Enabled = true;
			}
			else
			{
				radioButton202.Enabled = false;
				radioButton201.Checked = true;
			}

			if (radioButton201Checked && radioButton201.Checked)
				numericUpDown201_ValueChanged(numericUpDown201);

			_reportForm.Update(_arrivalLegs);

			button203.Enabled = true;
			NextBtn.Enabled = true;

			//_arrivalLegs[_arrivalLegs.Count - 1].EndFIX.ReCreateArea();
			//_arrivalLegs[_arrivalLegs.Count - 1].EndFIX.RefreshGraphics();
		}

		private void button203_Click(object sender, EventArgs e)
		{
			screenCapture.Delete();

			//double prevDir = _nextFIX.EntryDirection;
			//LegSBAS prevLeg = _currArrivalLeg;
			//if (_arrivalLegs.Count > 1)
			//	prevLeg = _arrivalLegs[_arrivalLegs.Count - 2];
			//else
			//	prevLeg =

			_currArrivalLeg.DeleteGraphics();
			_currFIX.DeleteGraphics();
			_nextFIX.DeleteGraphics();

			_currArrivalLeg = _arrivalLegs[_arrivalLegs.Count - 1];
			_arrivalLegs.RemoveAt(_arrivalLegs.Count - 1);

			_currArrivalLeg.Active = false;
			if (_arrivalLegs.Count == 0 && _leaveFromDir == CodeDirection.BOTH)
				_currArrivalLegB.Active = false;

			_currFIX = (FIX)_currArrivalLeg.StartFIX;
			_nextFIX = (FIX)_currArrivalLeg.EndFIX;

			double prevDir = _currFIX.EntryDirection;
			_prevAzt = ARANFunctions.DirToAzimuth(_currFIX.PrjPt, _currFIX.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			_prevAltitude = _currFIX.ConstructAltitude;
			_currAltitude = _nextFIX.ConstructAltitude;

			_arrivalFIXName = _nextFIX.Name;
			textBox202.Text = _nextFIX.Name;

			comboBox207.SelectedItem = _nextFIX.PBNType;
			comboBox204.SelectedItem = ARANMath.RadToDeg(_nextFIX.BankAngle);
			comboBox205.SelectedItem = _nextFIX.SensorType;
			comboBox206.SelectedIndex = (int)_nextFIX.FlyMode;
			checkBox201.Checked = _nextFIX.MultiCoverage;

			//==================================================================================
			double minFrom = _currFIX.CalcConstructionFromMinStablizationDistance((CodeSegmentPath)comboBox208.SelectedItem == CodeSegmentPath.DF);
			_minLegLenght = minFrom + _nextFIX.CalcConstructionInToMinStablizationDistance(_plannedTurn);

			comboBox201.Items.Clear();
			foreach (var fix in GlobalVars.WPTList)
			{
				double x, y, outDir = ARANFunctions.ReturnAngleInRadians(_currFIX.PrjPt, fix.pPtPrj);
				ARANFunctions.PrjToLocal(_currFIX.PrjPt, prevDir, fix.pPtPrj, out x, out y);

				TurnDirection EffectiveTurnDirection = TurnDirection.CW;
				if (y > 0)
					EffectiveTurnDirection = TurnDirection.CCW;

				double turnAngle = ARANMath.Modulus((outDir - prevDir) * ((int)EffectiveTurnDirection), ARANMath.C_2xPI);
				if (turnAngle > _plannedTurn)
				{
					if (_leaveFromDir != CodeDirection.BOTH)
						continue;

					turnAngle = ARANMath.Modulus((outDir - prevDir - Math.PI) * ((int)EffectiveTurnDirection), ARANMath.C_2xPI);
					if (turnAngle > _plannedTurn)
						continue;
				}

				comboBox201.Items.Add(fix);
			}

			bool radioButton201Checked = radioButton201.Checked;

			if (comboBox201.Items.Count > 0)
			{
				comboBox201.SelectedIndex = 0;
				radioButton202.Enabled = true;
			}
			else
			{
				radioButton202.Enabled = false;
				radioButton201.Checked = true;
			}

			if (radioButton201Checked && radioButton201.Checked)
				numericUpDown201_ValueChanged(numericUpDown201);


			if (_arrivalLegs.Count == 0 && _leaveFromDir == CodeDirection.BOTH)
				_reportForm.FillCurrentLeg(_currArrivalLeg, _currArrivalLegB);
			else
				_reportForm.FillCurrentLeg(_currArrivalLeg);

			_reportForm.Update(_arrivalLegs);

			button203.Enabled = _arrivalLegs.Count > 0;
			NextBtn.Enabled = _arrivalLegs.Count > 0;
		}

		#endregion

		#region Page III

		bool AddvanceToPageIII()
		{
			//_reportForm.AddPage4(transitions.CurrentObstacleList, transitions.CurrentDetObs);
			_currArrivalLeg.Visible = false;
			_reportForm.SetCurrentPage(3);

			ObstacleContainer Obstacles;
			int inx;

			// = Functions.GetLegObstList(GlobalVars.ObstacleList, out Obstacles, _currArrivalLeg, _currMOCLimit);
			//_currArrivalLeg.Obstacles_2 = Obstacles;

			//if (inx >= 0)
			//	_currArrivalLeg.DetObstacle_2 = Obstacles.Parts[inx];
			//else
			//	_currArrivalLeg.DetObstacle_2 = new ObstacleData();

			_arrivalLegs[_arrivalLegs.Count - 1].EndFIX.OutDirection = _arrivalLegs[_arrivalLegs.Count - 1].EndFIX.EntryDirection;

			if (_arrivalLegs.Count == 1)
			{
				_arrivalLegs[0].CreateGeometry(_forwardLeg, GlobalVars.CurrADHP, minTurnAngle);
				_arrivalLegs[0].RefreshGraphics();

				if (_leaveFromDir == CodeDirection.BOTH)
				{
					_currArrivalLegB.CreateGeometry(_backwardLeg, GlobalVars.CurrADHP, minTurnAngle);
					_currArrivalLegB.RefreshGraphics();

					inx = Functions.GetLegObstList(GlobalVars.ObstacleList, out Obstacles, _currArrivalLegB, _currMOCLimit);
					_currArrivalLegB.Obstacles = Obstacles;

					if (inx >= 0)
						_currArrivalLegB.DetObstacle_2 = Obstacles.Parts[inx];
					else
						_currArrivalLegB.DetObstacle_2 = new ObstacleData();
				}
			}
			else
			{
				_arrivalLegs[_arrivalLegs.Count - 1].CreateGeometry(_arrivalLegs[_arrivalLegs.Count - 2], GlobalVars.CurrADHP, minTurnAngle);
				_arrivalLegs[_arrivalLegs.Count - 1].RefreshGraphics();
			}

			inx = Functions.GetLegObstList(GlobalVars.ObstacleList, out Obstacles, _arrivalLegs[_arrivalLegs.Count - 1], _currMOCLimit);
			_arrivalLegs[_arrivalLegs.Count - 1].Obstacles = Obstacles;

			if (inx >= 0)
				_arrivalLegs[_arrivalLegs.Count - 1].DetObstacle_2 = Obstacles.Parts[inx];
			else
				_arrivalLegs[_arrivalLegs.Count - 1].DetObstacle_2 = new ObstacleData();
			/**/
			/////////////////////////////////////////////////////////////////////////////**/
			/**/
			/////////////////////////////////////////////////////////////////////////////**/
			/**/
			/////////////////////////////////////////////////////////////////////////////**/

			label301.Text = "Applied PDG:";
			//label302.Text = "%";

			label303.Text = "Altitude at which a gradient in excess of 3.3% is no longer required:";
			label305.Text = "Altitude at KK line:";
			label306.Text = GlobalVars.unitConverter.HeightUnit;

			/*=============*/

			double kkSum = 0;
			//bool trigger = false;
			//bool trigger0 = false;
			dataGridView301.RowCount = 0;


			//_arrivalLegs[0].PathAndTermination = CodeSegmentPath.IF;
			DataGridViewRow row = new DataGridViewRow();
			row.Tag = null;
			DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();

			LegArrival leg = _arrivalLegs[0];
			cell.Value = leg.StartFIX.Name;     //(i + 1).ToString();
			row.Cells.Add(cell);

			row.Cells.Add(new DataGridViewTextBoxCell());
			row.Cells[1].Value = leg.StartFIX.Name;

			cell = new DataGridViewTextBoxCell();
			cell.Value = CodeSegmentPath.IF;
			row.Cells.Add(cell);

			row.Cells.Add(new DataGridViewTextBoxCell());
			row.Cells.Add(new DataGridViewTextBoxCell());
			row.Cells.Add(new DataGridViewTextBoxCell());

			row.Cells.Add(new DataGridViewTextBoxCell());
			row.Cells[6].Value = GlobalVars.unitConverter.HeightToDisplayUnits(leg.StartFIX.ConstructAltitude); //END

			row.Cells.Add(new DataGridViewTextBoxCell());
			row.Cells[7].Value = GlobalVars.unitConverter.SpeedToDisplayUnits(leg.StartFIX.IAS, eRoundMode.SPECIAL_NEAREST);//END

			row.Cells.Add(new DataGridViewTextBoxCell());
			row.Cells[8].Value = Math.Round(GlobalVars.CurrADHP.MagVar, 2).ToString() + "°";

			row.Cells.Add(new DataGridViewTextBoxCell());
			row.Cells[9].Value = 0.0;

			row.Cells.Add(new DataGridViewTextBoxCell());
			row.Cells[10].Value = leg.StartFIX.PBNType;     //END

			row.Cells.Add(new DataGridViewTextBoxCell());
			row.Cells[11].Value = 0.0;

			row.Cells.Add(new DataGridViewTextBoxCell());
			row.Cells[12].Value = 0.0;

			row.Cells.Add(new DataGridViewTextBoxCell());
			row.Cells[13].Value = GlobalVars.unitConverter.HeightToDisplayUnits(leg.StartFIX.ConstructAltitude);

			dataGridView301.Rows.Add(row);

			int n = _arrivalLegs.Count;

			for (int i = 0; i < n; i++)
			{
				row = new DataGridViewRow();
				leg = _arrivalLegs[i];
				row.Tag = _arrivalLegs[i];
				row.ReadOnly = false;

				kkSum += leg.MinLegLength;

				cell = new DataGridViewTextBoxCell();

				cell.Value = leg.StartFIX.Name;     //(i + 1).ToString();
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[1].Value = leg.EndFIX.Name;

				cell = new DataGridViewTextBoxCell();
				cell.Value = leg.PathAndTermination;

				//if (leg.EndFIX.FlyMode == eFlyMode.Atheight)
				//	cell.Value = "VA";
				//else if (leg.EndFIX.IsDFTarget)
				//	cell.Value = "DF";
				//else if (leg.StartFIX.FlyMode == eFlyMode.Atheight)
				//	cell.Value = "CF";
				//else
				//	cell.Value = "TF";

				row.Cells.Add(cell);

				//==================================================//

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = leg.EndFIX.FlyMode;

				row.Cells.Add(new DataGridViewTextBoxCell());
				double Val = ARANFunctions.DirToAzimuth(leg.EndFIX.PrjPt, leg.EndFIX.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				double Val1 = ARANMath.Modulus(Val - GlobalVars.CurrADHP.MagVar);
				row.Cells[4].Value = Math.Round(Val, 2).ToString() + "° (" + Math.Round(Val1, 2).ToString() + "°)";

				row.Cells.Add(new DataGridViewTextBoxCell());
				//if (i == n - 1)
				//	row.Cells[5].Value = "-";
				//else

				if (ARANMath.SubtractAngles(leg.StartFIX.OutDirection, leg.StartFIX.EntryDirection) < ARANMath.EpsilonRadian)
					leg.StartFIX.TurnDirection = TurnDirection.NONE;
				else
				{
					leg.StartFIX.TurnDirection = ARANMath.SideFrom2Angle(leg.StartFIX.OutDirection, leg.StartFIX.EntryDirection);
					if (leg.StartFIX.TurnDirection == TurnDirection.CW)
						row.Cells[5].Value = "Right";
					else
						row.Cells[5].Value = "Left";
				}

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = GlobalVars.unitConverter.HeightToDisplayUnits(leg.EndFIX.ConstructAltitude, eRoundMode.NEAREST);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[7].Value = GlobalVars.unitConverter.SpeedToDisplayUnits(leg.EndFIX.IAS, eRoundMode.SPECIAL_NEAREST);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[8].Value = Math.Round(GlobalVars.CurrADHP.MagVar, 2).ToString() + "°";

				row.Cells.Add(new DataGridViewTextBoxCell());
				Val = Math.Round(ARANMath.RadToDeg(Math.Atan(leg.Gradient)));
				row.Cells[9].Value = Val;

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[10].Value = leg.EndFIX.PBNType;

				//==================================================//
				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[11].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(leg.Length, eRoundMode.NEAREST);

				//row.Cells.Add(new DataGridViewTextBoxCell());
				//row.Cells[12].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(leg.MinLegLength, eRoundMode.NEAREST);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[12].Value = GlobalVars.unitConverter.GradientToDisplayUnits(leg.Gradient, eRoundMode.NEAREST);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[13].Value = GlobalVars.unitConverter.HeightToDisplayUnits(leg.EndFIX.ConstructAltitude, eRoundMode.NEAREST);

				//double alt = kkSum * AppliedPDG + hObovDer + ptDerPrj.Z;
				//if (alt <= AppliedhReturn)
				//{
				//	row.Cells[6].Value = GlobalVars.unitConverter.HeightToDisplayUnits(alt, eRoundMode.NERAEST);
				//	row.Cells[13].Value = GlobalVars.unitConverter.GradientToDisplayUnits(AppliedPDG, eRoundMode.CEIL);
				//}
				//else if (!trigger)
				//{
				//	alt = AppliedhReturn + (kkSum - XReturnToNomPDG) * NomPDG;

				//	row.Cells[6].Value = GlobalVars.unitConverter.HeightToDisplayUnits(alt, eRoundMode.NERAEST);
				//	row.Cells[13].Value = GlobalVars.unitConverter.GradientToDisplayUnits(AppliedPDG, eRoundMode.CEIL) + "/" + GlobalVars.unitConverter.GradientToDisplayUnits(NomPDG, eRoundMode.CEIL);
				//	trigger = true;
				//}
				//else
				//{
				//	alt = AppliedhReturn + (kkSum - XReturnToNomPDG) * NomPDG;
				//	if (alt <= Transitions.MaxAltitude)
				//	{
				//		row.Cells[6].Value = GlobalVars.unitConverter.HeightToDisplayUnits(alt, eRoundMode.NERAEST);
				//		row.Cells[13].Value = GlobalVars.unitConverter.GradientToDisplayUnits(NomPDG, eRoundMode.CEIL);
				//	}
				//	else if (!trigger0)
				//	{
				//		alt = Transitions.MaxAltitude;
				//		row.Cells[6].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Transitions.MaxAltitude, eRoundMode.NERAEST);
				//		row.Cells[13].Value = GlobalVars.unitConverter.GradientToDisplayUnits(NomPDG, eRoundMode.CEIL) + "/ 0.0";
				//		trigger0 = true;
				//	}
				//	else
				//	{
				//		alt = Transitions.MaxAltitude;
				//		row.Cells[6].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Transitions.MaxAltitude, eRoundMode.NERAEST);
				//		row.Cells[13].Value = "0.0";
				//	}
				//}

				//leg.Altitude = alt;

				dataGridView301.Rows.Add(row);
			}

			return true;
		}

		private void textBox303_KeyPress(object sender, KeyPressEventArgs e)
		{

		}

		private void textBox303_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{

		}
		#endregion

		#endregion

		#region Page III

		UomSpeed mUomSpeed;
		UomDistance mUomHDistance;
		UomDistanceVertical mUomVDistance;

		private ArrivalLeg CreateFirstLeg(AircraftCharacteristic IsLimitedTo, StandardInstrumentArrival pProcedure)
		{
			ArrivalLeg pArrivalLeg;

			TerminalSegmentPoint pStartPoint;

			Feature pFixDesignatedPoint;
			SignificantPoint pFIXSignPt;

			UomSpeed[] uomSpeedTab;
			UomDistance[] uomDistHorTab;
			UomDistanceVertical[] uomDistVerTab;

			uomSpeedTab = new UomSpeed[] { UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN, UomSpeed.FT_SEC };
			uomDistHorTab = new UomDistance[] { UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI };
			uomDistVerTab = new UomDistanceVertical[] { UomDistanceVertical.M, UomDistanceVertical.FT };

			mUomSpeed = uomSpeedTab[GlobalVars.unitConverter.SpeedUnitIndex];
			mUomVDistance = uomDistVerTab[GlobalVars.unitConverter.HeightUnitIndex];
			mUomHDistance = uomDistHorTab[GlobalVars.unitConverter.DistanceUnitIndex];

			pArrivalLeg = DBModule.pObjectDir.CreateFeature<ArrivalLeg>();
			pArrivalLeg.AircraftCategory.Add(IsLimitedTo);

			pArrivalLeg.UpperLimitReference = CodeVerticalReference.MSL;
			pArrivalLeg.LowerLimitReference = CodeVerticalReference.MSL;


			pArrivalLeg.LegPath = CodeTrajectory.STRAIGHT;
			pArrivalLeg.CourseType = CodeCourse.TRUE_TRACK;

			LegArrival currLeg = _arrivalLegs[0];
			WayPoint StartFix = currLeg.StartFIX;
			WayPoint EndFix = currLeg.EndFIX;

			////double fAngle = ARANMath.Modulus(EndFix.OutDirection - EndFix.EntryDirection, ARANMath.C_2xPI);
			//double fAngle = StartFix.OutDirection - StartFix.EntryDirection;

			//if (Math.Abs(fAngle) > ARANMath.DegToRad(5))
			//{
			//	if (fAngle < Math.PI)
			//		pArrivalLeg.TurnDirection = CodeDirectionTurn.RIGHT;
			//	else
			//		pArrivalLeg.TurnDirection = CodeDirectionTurn.LEFT;
			//}
			////else
			////	pArrivalLeg.TurnDirection = CodeDirectionTurn.EITHER;

			//if (ARANMath.SubtractAngles(EndFix.EntryDirection, EndFix.OutDirection) < ARANMath.DegToRad(5))
			//	pArrivalLeg.TurnDirection = CodeDirectionTurn.EITHER;
			//else if (ARANMath.SideFrom2Angle(EndFix.EntryDirection, EndFix.OutDirection) == TurnDirection.CW)
			//	pArrivalLeg.TurnDirection = CodeDirectionTurn.RIGHT;
			//else
			//	pArrivalLeg.TurnDirection = CodeDirectionTurn.LEFT;

			pArrivalLeg.LegTypeARINC = CodeSegmentPath.IF;
			pArrivalLeg.EndConditionDesignator = CodeSegmentTermination.ALTITUDE;
			pArrivalLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT;

			pArrivalLeg.ProcedureTurnRequired = false;

			if (EndFix.PBNType == ePBNClass.RNP4)
				pArrivalLeg.RequiredNavigationPerformance = 4.0;
			else
				pArrivalLeg.RequiredNavigationPerformance = null;

			// ====================================================================

			pArrivalLeg.LowerLimitAltitude = new ValDistanceVertical(GlobalVars.unitConverter.HeightToDisplayUnits(StartFix.ConstructAltitude, eRoundMode.SPECIAL_NEAREST), mUomVDistance);
			pArrivalLeg.AltitudeInterpretation = CodeAltitudeUse.ABOVE_LOWER;

			// ====================================================================

			pArrivalLeg.SpeedLimit = new ValSpeed(GlobalVars.unitConverter.SpeedToDisplayUnits(StartFix.IAS, eRoundMode.SPECIAL_NEAREST), mUomSpeed);
			pArrivalLeg.SpeedReference = CodeSpeedReference.IAS;

			// Start Point ========================================================

			pStartPoint = new TerminalSegmentPoint();

			if (StartFix.TurnAngle < ARANMath.DegToRad(5.0))
				pStartPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.SDF;
			else
				pStartPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.TP;

			pStartPoint.Waypoint = true;
			pStartPoint.FlyOver = StartFix.FlyMode == eFlyMode.Flyover;

			pStartPoint.RadarGuidance = false;
			pStartPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT;

			//== ========================================================

			pFixDesignatedPoint = DBModule.CreateDesignatedPoint(StartFix, StartFix.OutDirection);
			pFIXSignPt = new SignificantPoint();

			if (pFixDesignatedPoint is DesignatedPoint)
				pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef();
			else if (pFixDesignatedPoint is Navaid)
				pFIXSignPt.NavaidSystem = pFixDesignatedPoint.GetFeatureRef();

			//if (Functions.PriorPostFixTolerance(StartFix.TolerArea, StartFix.PrjPt, StartFix.OutDirection, out PriorFixTolerance, out PostFixTolerance))
			//{
			//	pPointReference = new PointReference();
			//	//pPointReference.Role = CodeReferenceRole.RECNAV;

			//	pPointReference.PriorFixTolerance = new ValDistanceSigned(Math.Abs(GlobalVars.unitConverter.DistanceToDisplayUnits(PriorFixTolerance)), mUomHDistance);
			//	pPointReference.PostFixTolerance = new Aran.Aim.DataTypes.ValDistanceSigned(Math.Abs(GlobalVars.unitConverter.DistanceToDisplayUnits(PostFixTolerance)), mUomHDistance);

			//	pSurface = new Surface();
			//	Polygon pl = GlobalVars.pspatialReferenceOperation.ToGeo<Polygon>(StartFix.TolerArea);
			//	pSurface.Geo.Add(pl);

			//	pPointReference.FixToleranceArea = pSurface;

			//	pStartPoint.FacilityMakeup.Add(pPointReference);
			//}

			pStartPoint.PointChoice = pFIXSignPt;
			pArrivalLeg.StartPoint = pStartPoint;

			//  End Of Start Point ================================================
			//=====================================================================
			//  END =====================================================

			return pArrivalLeg;
		}

		private ArrivalLeg CreateArrivalLeg(int num, AircraftCharacteristic IsLimitedTo,
			StandardInstrumentArrival pProcedure, ref TerminalSegmentPoint pEndPoint)
		{
			double PriorFixTolerance, PostFixTolerance;

			ArrivalLeg pArrivalLeg;

			TerminalSegmentPoint pStartPoint;
			//TerminalSegmentPoint pEndPoint;

			Feature pFixDesignatedPoint;
			SignificantPoint pFIXSignPt;
			PointReference pPointReference;
			Surface pSurface;

			//ValSpeed pSpeed;
			ValDistance pDistance;
			//ValDistanceSigned pDistanceSigned;
			ValDistanceVertical pDistanceVertical;

			//UomSpeed mUomSpeed;
			//UomDistance mUomHDistance;
			//UomDistanceVertical mUomVDistance;

			//UomSpeed[] uomSpeedTab;
			//UomDistance[] uomDistHorTab;
			//UomDistanceVertical[] uomDistVerTab;

			//uomSpeedTab = new UomSpeed[]{UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN, UomSpeed.FT_SEC};
			//uomDistHorTab = new UomDistance[]{UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI};
			//uomDistVerTab = new UomDistanceVertical[] { UomDistanceVertical.M, UomDistanceVertical.FT };

			//mUomSpeed = uomSpeedTab[GlobalVars.unitConverter.SpeedUnitIndex];
			//mUomVDistance = uomDistVerTab[GlobalVars.unitConverter.HeightUnitIndex];
			//mUomHDistance = uomDistHorTab[GlobalVars.unitConverter.DistanceUnitIndex];

			pArrivalLeg = DBModule.pObjectDir.CreateFeature<ArrivalLeg>();
			pArrivalLeg.AircraftCategory.Add(IsLimitedTo);

			pArrivalLeg.UpperLimitReference = CodeVerticalReference.MSL;
			pArrivalLeg.LowerLimitReference = CodeVerticalReference.MSL;

			pArrivalLeg.LegPath = CodeTrajectory.STRAIGHT;
			pArrivalLeg.CourseType = CodeCourse.TRUE_TRACK;

			LegArrival currLeg = _arrivalLegs[num];
			WayPoint StartFix = currLeg.StartFIX;
			WayPoint EndFix = currLeg.EndFIX;

			if (ARANMath.SubtractAngles(StartFix.OutDirection, StartFix.EntryDirection) < ARANMath.EpsilonRadian)
				StartFix.TurnDirection = TurnDirection.NONE;
			else
			{
				StartFix.TurnDirection = ARANMath.SideFrom2Angle(StartFix.OutDirection, StartFix.EntryDirection);
				if (StartFix.TurnDirection == TurnDirection.CW)
					pArrivalLeg.TurnDirection = CodeDirectionTurn.RIGHT;
				else
					pArrivalLeg.TurnDirection = CodeDirectionTurn.LEFT;
			}

			pArrivalLeg.LegTypeARINC = currLeg.PathAndTermination;

			if (EndFix.FlyMode == eFlyMode.Atheight)
			{
				pArrivalLeg.EndConditionDesignator = CodeSegmentTermination.ALTITUDE;
				pArrivalLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT;
			}
			else if (EndFix.IsDFTarget)
			{
				pArrivalLeg.EndConditionDesignator = CodeSegmentTermination.INTERCEPT;
				pArrivalLeg.Length = new ValDistance(GlobalVars.unitConverter.DistanceToDisplayUnits(currLeg.Length), mUomHDistance);
			}
			else if (num == 0 || StartFix.FlyMode == eFlyMode.Atheight)
			{
				pArrivalLeg.EndConditionDesignator = CodeSegmentTermination.INTERCEPT;
				pArrivalLeg.Length = new ValDistance(GlobalVars.unitConverter.DistanceToDisplayUnits(currLeg.Length), mUomHDistance);
			}
			else
			{
				pArrivalLeg.EndConditionDesignator = CodeSegmentTermination.INTERCEPT;
				pArrivalLeg.Length = new ValDistance(GlobalVars.unitConverter.DistanceToDisplayUnits(ARANFunctions.ReturnDistanceInMeters(StartFix.PrjPt, EndFix.PrjPt), eRoundMode.NEAREST), mUomHDistance);
			}

			pArrivalLeg.ProcedureTurnRequired = false;
			if (!EndFix.IsDFTarget)
				pArrivalLeg.Course = ARANFunctions.DirToAzimuth(EndFix.PrjPt, EndFix.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);


			if (EndFix.PBNType == ePBNClass.RNP4)
				pArrivalLeg.RequiredNavigationPerformance = 4.0;
			else
				pArrivalLeg.RequiredNavigationPerformance = null;

			//pArrivalLeg.Arrival = pProcedure.GetFeatureRef();
			// ====================================================================

			pArrivalLeg.LowerLimitAltitude =
				new ValDistanceVertical(
					GlobalVars.unitConverter.HeightToDisplayUnits(EndFix.ConstructAltitude, eRoundMode.SPECIAL_NEAREST),
					mUomVDistance);
			pArrivalLeg.AltitudeInterpretation = CodeAltitudeUse.ABOVE_LOWER;

			// ====================================================================

			pArrivalLeg.VerticalAngle = -ARANMath.RadToDeg(System.Math.Atan(currLeg.Gradient));

			pArrivalLeg.SpeedLimit = new ValSpeed(
				GlobalVars.unitConverter.SpeedToDisplayUnits(EndFix.IAS, eRoundMode.SPECIAL_NEAREST), mUomSpeed);
			pArrivalLeg.SpeedReference = CodeSpeedReference.IAS;

			//pArrivalLeg.WidthLeft =
			//pArrivalLeg.WidthRight = 

			//     pArrivalLeg.AltitudeOverrideATC =
			//     pArrivalLeg.AltitudeOverrideReference =
			//     pArrivalLeg.Duration = '???????????????????????????????? pLegs(I).valDur
			//     pArrivalLeg.Note
			//     pArrivalLeg.ReqNavPerformance
			//     pArrivalLeg.SpeedInterpretation =
			//     pArrivalLeg.ReqNavPerformance
			//     pArrivalLeg.CourseDirection =
			//     pArrivalLeg.ProcedureTransition
			// ====================================================================

			// Start Point ========================================================


			pStartPoint = new TerminalSegmentPoint();

			if (num == _arrivalLegs.Count - 1)
				pStartPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.IAF;
			else if (StartFix.TurnAngle < ARANMath.DegToRad(5.0))
				pStartPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.SDF;
			else
				pStartPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.TP;

			pStartPoint.Waypoint = true;
			pStartPoint.FlyOver = StartFix.FlyMode == eFlyMode.Flyover;

			pStartPoint.RadarGuidance = false;
			pStartPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT;

			//== ========================================================

			pFIXSignPt = new SignificantPoint();
			if (pEndPoint == null)
			{
				pFixDesignatedPoint = DBModule.CreateDesignatedPoint(StartFix, StartFix.OutDirection);
				if (pFixDesignatedPoint is DesignatedPoint)
					pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef();
				else if (pFixDesignatedPoint is Navaid)
					pFIXSignPt.NavaidSystem = pFixDesignatedPoint.GetFeatureRef();
			}
			else
				pFIXSignPt = pEndPoint.PointChoice;

			if (Functions.PriorPostFixTolerance(StartFix.TolerArea, StartFix.PrjPt, StartFix.OutDirection,
				out PriorFixTolerance, out PostFixTolerance))
			{
				pPointReference = new PointReference();
				//pPointReference.Role = CodeReferenceRole.RECNAV;

				pPointReference.PriorFixTolerance =
					new ValDistanceSigned(Math.Abs(GlobalVars.unitConverter.DistanceToDisplayUnits(PriorFixTolerance)),
						mUomHDistance);
				pPointReference.PostFixTolerance = new Aran.Aim.DataTypes.ValDistanceSigned(
					Math.Abs(GlobalVars.unitConverter.DistanceToDisplayUnits(PostFixTolerance)), mUomHDistance);

				pSurface = new Surface();
				MultiPolygon pl = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(StartFix.TolerArea);
				pSurface.Geo.Add(pl);

				pPointReference.FixToleranceArea = pSurface;

				pStartPoint.FacilityMakeup.Add(pPointReference);
			}

			pStartPoint.PointChoice = pFIXSignPt;
			pArrivalLeg.StartPoint = pStartPoint;


			//  End Of Start Point ================================================
			//=====================================================================
			//  Start Of End Point ================================================

			pEndPoint = new TerminalSegmentPoint();
			pEndPoint.Waypoint = true;
			pEndPoint.FlyOver = EndFix.FlyMode == eFlyMode.Flyover;

			pEndPoint.RadarGuidance = false;
			pEndPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT;


			//============================================================
			pFixDesignatedPoint = DBModule.CreateDesignatedPoint(EndFix, EndFix.EntryDirection);
			pFIXSignPt = new SignificantPoint();

			if (pFixDesignatedPoint is DesignatedPoint)
				pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef();
			else if (pFixDesignatedPoint is Navaid)
				pFIXSignPt.NavaidSystem = pFixDesignatedPoint.GetFeatureRef();

			//=======================

			//EndFix.ReCreateArea();

			if (Functions.PriorPostFixTolerance(EndFix.TolerArea, EndFix.PrjPt, EndFix.EntryDirection,
				out PriorFixTolerance, out PostFixTolerance))
			{
				pPointReference = new PointReference();
				//pPointReference.Role = CodeReferenceRole.RECNAV;

				pPointReference.PriorFixTolerance =
					new ValDistanceSigned(Math.Abs(GlobalVars.unitConverter.DistanceToDisplayUnits(PriorFixTolerance)),
						mUomHDistance);
				pPointReference.PostFixTolerance =
					new ValDistanceSigned(Math.Abs(GlobalVars.unitConverter.DistanceToDisplayUnits(PostFixTolerance)),
						mUomHDistance);

				pSurface = new Surface();
				MultiPolygon pl = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(EndFix.TolerArea);
				pSurface.Geo.Add(pl);
				pPointReference.FixToleranceArea = pSurface;

				pEndPoint.FacilityMakeup.Add(pPointReference);
			}

			pEndPoint.PointChoice = pFIXSignPt;
			pArrivalLeg.EndPoint = pEndPoint;

			// End of EndPoint ========================

			// Trajectory =====================================================
			Curve pCurve = new Curve();
			LineString ls = GlobalVars.pspatialReferenceOperation.ToGeo<LineString>(currLeg.NominalTrack);
			pCurve.Geo.Add(ls);

			pArrivalLeg.Trajectory = pCurve;

			// I protected Area =======================================================
			// var pTopo = new Geometries.Operators.JtsGeometryOperators();// GeometryOperators();
			var pTopo = new Geometries.Operators.GeometryOperators();
			MultiPolygon pPrimProtectedAreaSurface = currLeg.PrimaryAssesmentArea;

			//  GlobalVars.gAranGraphics.DrawMultiPolygon(pPrimProtectedAreaSurface, 100, AranEnvironment.Symbols.eFillStyle.sfsCross, true, false);

			if (num == 0 && _leaveFromDir == CodeDirection.BOTH)
				pPrimProtectedAreaSurface =
					(MultiPolygon)pTopo.UnionGeometry(currLeg.PrimaryAssesmentArea, _backwardLeg.PrimaryAssesmentArea);

			pSurface = new Surface();
			for (int i = 0; i < pPrimProtectedAreaSurface.Count; i++)
			{
				Polygon pl = GlobalVars.pspatialReferenceOperation.ToGeo<Polygon>(pPrimProtectedAreaSurface[i]);
				pSurface.Geo.Add(pl);
			}

			ObstacleAssessmentArea pPrimProtectedArea = new ObstacleAssessmentArea();
			pPrimProtectedArea.Surface = pSurface;
			pPrimProtectedArea.SectionNumber = 0;
			pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY;

			// II protected Area =======================================================
			MultiPolygon pSecProtectedAreaSurface;

			if (num == 0 && _leaveFromDir == CodeDirection.BOTH)
			{
				MultiPolygon pPoly = (MultiPolygon)pTopo.UnionGeometry(currLeg.FullAssesmentArea, _backwardLeg.FullAssesmentArea);
				pSecProtectedAreaSurface = (MultiPolygon)pTopo.Difference(pPoly, pPrimProtectedAreaSurface);
			}
			else
				pSecProtectedAreaSurface = (MultiPolygon)pTopo.Difference(currLeg.FullAssesmentArea, currLeg.PrimaryAssesmentArea);

			ObstacleAssessmentArea pSecProtectedArea = null;
			if (pSecProtectedAreaSurface != null)
			{
				pSurface = new Surface();
				for (int i = 0; i < pSecProtectedAreaSurface.Count; i++)
				{
					Polygon pl = GlobalVars.pspatialReferenceOperation.ToGeo<Polygon>(pSecProtectedAreaSurface[i]);
					pSurface.Geo.Add(pl);
				}

				pSecProtectedArea = new ObstacleAssessmentArea();
				pSecProtectedArea.Surface = pSurface;
				pSecProtectedArea.SectionNumber = 1;
				pSecProtectedArea.Type = CodeObstacleAssessmentSurface.SECONDARY;
			}

			// GlobalVars.gAranGraphics.DrawMultiPolygon(GlobalVars.pspatialReferenceOperation.ToPrj(pSurface.Geo), 0, AranEnvironment.Symbols.eFillStyle.sfsNull, true, false);
			// Protection Area Obstructions list ==================================================

			ObstacleContainer ostacles = currLeg.Obstacles;

			if (num == 0 && _leaveFromDir == CodeDirection.BOTH)
				Functions.mergeObstacleLists(currLeg, _backwardLeg, out ostacles);

			Array.Sort(ostacles.Parts, Functions.ComparePartsByOwner);

			int own = 0;
			int m = ostacles.Parts.Length;

			for (int i = 0; i < ostacles.Obstacles.Length; i++)
			{
				Obstruction obs = new Obstruction();
				obs.VerticalStructureObstruction = new FeatureRef(ostacles.Obstacles[i].Identifier); //ostacles.Obstacles[i].pFeature.GetFeatureRef();

				double MinimumAltitude = 0;
				double RequiredClearance = 0;
				int isPrimary = 0;

				while (own<m && ostacles.Parts[own].Owner == i)
				{
					MinimumAltitude = Math.Max(MinimumAltitude, ostacles.Parts[own].ReqH);
					RequiredClearance = Math.Max(RequiredClearance, ostacles.Parts[own].MOC);

					if (ostacles.Parts[own].Prima)
						isPrimary |= 1;
					else
						isPrimary |= 2;
					own++;
				}

				//for (int j = 0; j < ostacles.Obstacles[i].PartsNum; j++)
				//{
				//	MinimumAltitude = Math.Max(MinimumAltitude, ostacles.Parts[ostacles.Obstacles[i].Parts[j]].ReqH);
				//	RequiredClearance = Math.Max(RequiredClearance, ostacles.Parts[ostacles.Obstacles[i].Parts[j]].MOC);

				//	if (ostacles.Parts[ostacles.Obstacles[i].Parts[j]].Prima)
				//		isPrimary |= 1;
				//	else
				//		isPrimary |= 2;
				//}

				//ReqH
				pDistanceVertical = new ValDistanceVertical();
				pDistanceVertical.Uom = mUomVDistance;
				pDistanceVertical.Value = GlobalVars.unitConverter.HeightToDisplayUnits(MinimumAltitude);
				obs.MinimumAltitude = pDistanceVertical;

				//MOC
				pDistance = new ValDistance();
				pDistance.Uom = UomDistance.M;
				pDistance.Value = RequiredClearance;
				obs.RequiredClearance = pDistance;

				if ((isPrimary & 1) != 0)
					pPrimProtectedArea.SignificantObstacle.Add(obs);

				if (pSecProtectedArea != null && (isPrimary & 2) != 0)
					pSecProtectedArea.SignificantObstacle.Add(obs);
			}

			pArrivalLeg.DesignSurface.Add(pPrimProtectedArea);
			if (pSecProtectedArea != null)
				pArrivalLeg.DesignSurface.Add(pSecProtectedArea);

			//  END =====================================================

			return pArrivalLeg;
		}

		private bool SaveProcedure(ReportHeader pReport)
		{
			ProcedureTransition pTransition;
			AircraftCharacteristic IsLimitedTo;
			SegmentLeg pSegmentLeg;
			ProcedureTransitionLeg ptl;

			FeatureRef featureRef;
			FeatureRefObject featureRefObject;

			//GuidanceService pGuidanceServiceChose = new GuidanceService();

			//  Procedure =================================================================================================
			pProcedure = DBModule.pObjectDir.CreateFeature<StandardInstrumentArrival>();

			//pProcedure.CommunicationFailureDescription
			//pProcedure.Description =
			//pProcedure.ID
			//pProcedure.Note =
			//pProcedure.ProtectsSafeAltitudeAreaId =

			pProcedure.CodingStandard = CodeProcedureCodingStandard.PANS_OPS;
			pProcedure.DesignCriteria = CodeDesignStandard.PANS_OPS;
			pProcedure.RNAV = true;
			pProcedure.FlightChecked = false;

			LandingTakeoffAreaCollection pLandingTakeoffAreaCollection = new LandingTakeoffAreaCollection();

			foreach (var item in checkedListBox101.CheckedItems)
				pLandingTakeoffAreaCollection.Runway.Add(((RWYType)item).GetFeatureRefObject());

			pProcedure.Arrival = pLandingTakeoffAreaCollection;

			IsLimitedTo = new AircraftCharacteristic();

			switch (comboBox105.SelectedIndex)
			{
				case 0:
					IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.A;
					break;
				case 1:
					IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.B;
					break;
				case 2:
					IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.C;
					break;
				case 3:
					IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.D;
					break;
				case 4:
					IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.E;
					break;
			}

			pProcedure.AircraftCharacteristic.Add(IsLimitedTo);

			featureRefObject = new FeatureRefObject();
			featureRef = new FeatureRef();
			featureRef.Identifier = GlobalVars.CurrADHP.pAirportHeliport.Identifier;
			featureRefObject.Feature = featureRef;
			pProcedure.AirportHeliport.Add(featureRefObject);

			//pGuidanceServiceChose.Navaid = FinalNav.pFeature.GetFeatureRef();
			//pProcedure.GuidanceFacility.Add(pGuidanceServiceChose);

			pProcedure.Name = pReport.Procedure;

			// Transition ==========================================================================
			pTransition = new ProcedureTransition();
			pTransition.Type = CodeProcedurePhase.APPROACH;

			//pTransition.
			//pTransition.DepartureRunwayTransition = pLandingTakeoffAreaCollection;

			//pTransition.Description =
			//pTransition.ID =
			//pTransition.Procedure =
			//pTransition.TransitionId = TextBox0???.Text

			// Legs ======================================================================================================

			TerminalSegmentPoint pEndPoint = null;
			uint NO_SEQ = 1;

			pSegmentLeg = CreateFirstLeg(IsLimitedTo, pProcedure);
			ptl = new ProcedureTransitionLeg();
			ptl.SeqNumberARINC = NO_SEQ;
			ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef<AbstractSegmentLegRef>();
			pTransition.TransitionLeg.Add(ptl);

			NO_SEQ++;

			for (int i = 0; i < _arrivalLegs.Count; i++, NO_SEQ++)
			{
				pSegmentLeg = CreateArrivalLeg(i, IsLimitedTo, pProcedure, ref pEndPoint);

				ptl = new ProcedureTransitionLeg();
				ptl.SeqNumberARINC = NO_SEQ;
				ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef<AbstractSegmentLegRef>();
				pTransition.TransitionLeg.Add(ptl);
			}

			//=============================================================================
			pProcedure.FlightTransition.Add(pTransition);

			try
			{
				DBModule.pObjectDir.SetRootFeatureType(Aim.FeatureType.StandardInstrumentArrival);

				bool saveRes = DBModule.pObjectDir.Commit(new Aim.FeatureType[]{
						Aim.FeatureType.DesignatedPoint,
						Aim.FeatureType.AngleIndication,
						Aim.FeatureType.DistanceIndication,
						Aim.FeatureType.StandardInstrumentDeparture,
						Aim.FeatureType.StandardInstrumentArrival,
						Aim.FeatureType.InstrumentApproachProcedure,
						Aim.FeatureType.ArrivalFeederLeg,
						Aim.FeatureType.ArrivalLeg,
						Aim.FeatureType.DepartureLeg,
						Aim.FeatureType.FinalLeg,
						Aim.FeatureType.InitialLeg,
						Aim.FeatureType.IntermediateLeg,
						Aim.FeatureType.MissedApproachLeg});

				GlobalVars.gAranEnv.RefreshAllAimLayers();

				return saveRes;
			}
			catch (Exception ex)
			{
				throw new Exception("Error on commit." + "\r\n" + ex.Message);
			}

			//return false;
		}

		private double ConvertTracToPoints(out ReportPoint[] GuidPoints)
		{
			double result = 0.0;

			int n = _arrivalLegs.Count;
			if (n == 0)
			{
				GuidPoints = new ReportPoint[0];
				return 0.0;
			}

			GuidPoints = new ReportPoint[n + 1];
			LegArrival leg;

			for (int i = 0; i < n; i++)
			{
				leg = _arrivalLegs[i];
				GuidPoints[i].TrueCourse = ARANFunctions.DirToAzimuth(leg.EndFIX.PrjPt, leg.EndFIX.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				GuidPoints[i].DistToNext = leg.Length;
				GuidPoints[i].Height = -1.0;    // leg.UpperLimit;
				GuidPoints[i].Radius = -1.0;

				GuidPoints[i].Description = leg.StartFIX.Name;
				GuidPoints[i].Lat = leg.StartFIX.GeoPt.Y;
				GuidPoints[i].Lon = leg.StartFIX.GeoPt.X;

				result += leg.NominalTrack.Length;
				//result += leg.Length;
			}

			leg = _arrivalLegs[n - 1];
			GuidPoints[n].TrueCourse = ARANFunctions.DirToAzimuth(leg.EndFIX.PrjPt, leg.EndFIX.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			GuidPoints[n].DistToNext = -1.0;
			GuidPoints[n].Height = -1.0;        // leg.UpperLimit;
			GuidPoints[n].Radius = -1.0;

			GuidPoints[n].Description = leg.EndFIX.Name;
			GuidPoints[n].Lat = leg.EndFIX.GeoPt.Y;
			GuidPoints[n].Lon = leg.EndFIX.GeoPt.X;

			return result;
		}

		private void OkBtn_Click(object sender, EventArgs e)
		{
			string RepFileName, RepFileTitle;
			screenCapture.Save(this);

			if (!Functions.ShowSaveDialog(out RepFileName, out RepFileTitle))
				return;

			string sProcName = RepFileTitle;	// _startFIX.Name;

			ReportHeader pReport;
			pReport.Procedure = sProcName;
			pReport.Database = GlobalVars.gAranEnv.ConnectionInfo.Database;
			pReport.Aerodrome = GlobalVars.CurrADHP.Name;
			pReport.Category = comboBox105.Text;
			//pReport.
			////pReport.EffectiveDate = _Procedure.TimeSlice.ValidTime.BeginPosition;

			ReportPoint[] GuidPoints;
			double _TotalLen = ConvertTracToPoints(out GuidPoints);

			SaveLog(RepFileName, RepFileTitle, pReport);
			SaveProtocol(RepFileName, RepFileTitle, pReport);

			arrivalGeomRep = CReportFile.SaveGeometry(RepFileName, RepFileTitle, pReport, GuidPoints, _TotalLen);

			//_reportForm.WriteTab(RepFileName, sProcName);

			if (SaveProcedure(pReport))
			{
				saveReportToDB();
				SaveScreenshotToDB();
				this.Close();
			}
		}

		private void saveReportToDB()
		{
			saveReportToDB(arrivalLogRep, FeatureReportType.Log);
			saveReportToDB(arrivalProtRep, FeatureReportType.Protocol);
			saveReportToDB(arrivalGeomRep, FeatureReportType.Geometry);
		}

		private void saveReportToDB(CReportFile rp, FeatureReportType type)
		{
			if (rp.IsFinished)
			{
				FeatureReport report = new FeatureReport();
				report.Identifier = pProcedure.Identifier;
				report.ReportType = type;
				report.HtmlZipped = rp.Report;
				DBModule.pObjectDir.SetFeatureReport(report);
			}
		}

		private void SaveScreenshotToDB()
		{
			Screenshot screenshot = new Screenshot();
			screenshot.DateTime = DateTime.Now;
			screenshot.Identifier = pProcedure.Identifier;
			screenshot.Images = screenCapture.Commit(pProcedure.Identifier);
			DBModule.pObjectDir.SetScreenshot(screenshot);
		}

		private void SaveProtocol(string RepFileName, string RepFileTitle, ReportHeader pReport)
		{
			arrivalProtRep = new CReportFile();

			arrivalProtRep.OpenFile(RepFileName + "_Protocol", Resources.str00109);

			arrivalProtRep.WriteString(Resources.str00030 + " - " + Resources.str00109, true);
			arrivalProtRep.WriteString("");
			//arrivalProtRep.WriteString(RepFileTitle, true);

			arrivalProtRep.WriteHeader(pReport);
			arrivalProtRep.WriteString("");
			arrivalProtRep.WriteString("");

			int n = _arrivalLegs.Count;
			for (int i = 0; i < n; i++)
			{
				LegArrival currLeg = _arrivalLegs[i];

				string TabComment = "leg " + currLeg.StartFIX.Name + "/" + currLeg.EndFIX.Name;
				arrivalProtRep.SaveObstacles(TabComment, _reportForm.dataGridView02, currLeg.Obstacles);
			}

			arrivalProtRep.CloseFile();
		}

		private void SaveLog(string RepFileName, string RepFileTitle, ReportHeader pReport)
		{
			arrivalLogRep = new CReportFile();
			//GuidLogRep.DerPtPrj = SelectedRWY.pPtPrj[eRWY.ptEnd];
			//GuidLogRep.ThrPtPrj = SelectedRWY.pPtPrj[eRWY.ptEnd];

			arrivalLogRep.OpenFile(RepFileName + "_Log", Resources.str00124);

			arrivalLogRep.WriteString(Resources.str00030 + " - " + Resources.str00124, true);
			arrivalLogRep.WriteString("");
			//arrivalLogRep.WriteString(RepFileTitle, true);

			arrivalLogRep.WriteHeader(pReport);

			//     GuidLogRep.WriteParam LoadResString(518), CStr(Date) + " - " + CStr(Time)
			//     If AIRLayerInfo.Initialised Then GuidLogRep.WriteLayerInfo AIRLayerInfo
			//     If RWYLayerInfo.Initialised Then GuidLogRep.WriteLayerInfo RWYLayerInfo
			//     If NAVLayerInfo.Initialised Then GuidLogRep.WriteLayerInfo NAVLayerInfo
			//     If ObsLayerInfo.Initialised Then GuidLogRep.WriteLayerInfo ObsLayerInfo
			//     If FIXLayerInfo.Initialised Then GuidLogRep.WriteLayerInfo FIXLayerInfo
			//     If WARNINGLayerInfo.Initialised Then GuidLogRep.WriteLayerInfo WARNINGLayerInfo

			arrivalLogRep.WriteString("");
			arrivalLogRep.WriteString("");

			arrivalLogRep.ExH2(tabControl1.TabPages[0].Text);
			arrivalLogRep.HTMLString("[ " + tabControl1.TabPages[0].Text + " ]", true, false);
			arrivalLogRep.WriteString("");

			arrivalLogRep.Param(label112.Text, comboBox105.Text, "");
			arrivalLogRep.Param(label113.Text, comboBox106.Text, "");
			arrivalLogRep.Param(label106.Text, comboBox103.Text, "");
			arrivalLogRep.Param(checkBox101.Text, checkBox101.Checked.ToString(), "");
			arrivalLogRep.WriteString("");

			
			arrivalLogRep.Param(label101.Text, comboBox101.Text, "");
			arrivalLogRep.Param(label107.Text, comboBox104.Text, "");
			arrivalLogRep.Param(label102.Text, textBox101.Text, label103.Text);
			arrivalLogRep.Param(label104.Text, comboBox108.Text, "");
			arrivalLogRep.Param(label105.Text, comboBox102.Text, "");
			arrivalLogRep.Param(label110.Text, textBox104.Text, label111.Text);
			arrivalLogRep.WriteString("");

			arrivalLogRep.WriteString(groupBox101.Text);

			if (radioButton101.Checked)
				arrivalLogRep.Param(radioButton101.Text, textBox105.Text, label116.Text);
			else
				arrivalLogRep.Param(radioButton102.Text, comboBox107.Text, label117.Text);

			arrivalLogRep.Param(label118.Text, textBox106.Text, "");

			arrivalLogRep.WriteString("");
			//arrivalLogRep.WriteString(label119.Text);
			arrivalLogRep.Param(label119.Text, " ");

			foreach (var itm in checkedListBox101.CheckedItems)
				arrivalLogRep.WriteString(itm.ToString());

			arrivalLogRep.WriteString("");
			arrivalLogRep.WriteString("");

			arrivalLogRep.ExH2(tabControl1.TabPages[1].Text);
			arrivalLogRep.HTMLString("[ " + tabControl1.TabPages[1].Text + " ]", true, false);
			arrivalLogRep.WriteString("");
			arrivalLogRep.WriteString("- - -");
			arrivalLogRep.WriteString("");
			arrivalLogRep.WriteString("");


			arrivalLogRep.ExH2(tabControl1.TabPages[2].Text);
			arrivalLogRep.HTMLString("[ " + tabControl1.TabPages[2].Text + " ]", true, false);
			arrivalLogRep.WriteString("");

			arrivalLogRep.WriteTab(dataGridView301, RepFileTitle);
			arrivalLogRep.WriteString("");

			//arrivalLogRep.Param(label305.Text, textBox303.Text, label306.Text);
			//arrivalLogRep.WriteString("");
			//arrivalLogRep.WriteString(groupBox401.Text, true);
			//arrivalLogRep.Param(label301.Text, textBox301.Text, label302.Text);
			//arrivalLogRep.Param(label303.Text, textBox302.Text, label304.Text);

			arrivalLogRep.CloseFile();
		}

		#endregion

	}
}

/*
정우영

____━━____┗┓|::::::^━━^ 
____━━____━┗|:::::|｡◕‿‿­­­­◕｡| 
____━━____━━╰O--O-O--O ╯
*/
