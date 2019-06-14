using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Queries;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.PANDA.Common;
using Aran.PANDA.RNAV.EnRoute.Properties;
using Aran.PANDA.Reports;

namespace Aran.PANDA.RNAV.EnRoute
{
    public partial class RouteForm : Form
    {
        //const double EnRouteIAS = 585.0 * 0.277777777777777777777777;

        private const double maxDistance = 3000000.0;
        private double[] EnrouteMOCValues = {300.0, 450.0, 600.0};

        #region Variable declarations

        private bool bFormInitialised;
        private bool bCreateGeometry;

        private ReportForm _reportForm;
        private List<Segment> _Legs;
        private Segment _currLeg;

        private double _endMagVar;
        private double _startMagVar;
        private double _IAS;
        private double _minLenght;
        private double _bankAngle;
        private double _FIXDistance;
        private double _TotalLen;
        private double _minAzimuth;
        private double _maxAzimuth;
        private double _moc;

        #endregion

        #region MainForm

        public RouteForm()
        {
            InitializeComponent();

            bFormInitialised = false;
            bCreateGeometry = true;

            Text = this.Text ;

            //_bankAngle = ARANMath.DegToRad(15.0);
            //_bankAngle = ARANMath.DegToRad(25.0);
            //_IAS = GlobalVars.constants.AircraftCategory[PANDA.Constants.aircraftCategoryData.enIAS].Value[PANDA.Constants.aircraftCategory.acD];

            _bankAngle = GlobalVars.constants.Pansops[PANDA.Constants.ePANSOPSData.enBankAngle].Value;
            _IAS = GlobalVars.constants.Pansops[PANDA.Constants.ePANSOPSData.enTurnIAS].Value;
            _TotalLen = 0.0;
            _endMagVar = 0.0;
            _startMagVar = 0.0;

            comboBox010.Items.Clear();
            int n = EnrouteMOCValues.Length;
            for (int i = 0; i < n; i++)
                comboBox010.Items.Add(
                    GlobalVars.unitConverter.HeightToDisplayUnits(EnrouteMOCValues[i], eRoundMode.SPECIAL_NEAREST));

            _Legs = new List<Segment>();
            _currLeg = new Segment();

            comboBox010.SelectedIndex = 0;

            _currLeg.Granted = CodeDirection.BOTH;
            _currLeg.TrueTrack = (double) numericUpDown004.Value;
            _currLeg.MOC = _moc;

            textBox001.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_IAS, eRoundMode.SPECIAL_NEAREST).ToString();
            textBox002.Text = Math.Round(ARANMath.RadToDeg(_bankAngle), 1).ToString();
            textBox010.Text = _endMagVar.ToString();
            txtStartMagVar.Text = _startMagVar.ToString();

            label002.Text = GlobalVars.unitConverter.SpeedUnit;
            label014.Text = GlobalVars.unitConverter.SpeedUnit;
            //label027.Text = GlobalVars.unitConverter.SpeedUnit; //Wind speed

            label024.Text = GlobalVars.unitConverter.DistanceUnit;
            label032.Text = GlobalVars.unitConverter.DistanceUnit;
            label030.Text = GlobalVars.unitConverter.HeightUnit;

            dataGridViewTextBoxColumn1.HeaderText = Resources.str02010;
            dataGridViewTextBoxColumn2.HeaderText = Resources.str02011;
            dataGridViewTextBoxColumn3.HeaderText = Resources.str02012;
            dataGridViewTextBoxColumn4.HeaderText = Resources.str02013;
            dataGridViewTextBoxColumn5.HeaderText = Resources.str02014 + " (°)";
            dataGridViewTextBoxColumn6.HeaderText = Resources.str02015;
            dataGridViewTextBoxColumn7.HeaderText = Resources.str02016 + " (" + GlobalVars.unitConverter.DistanceUnit +
                                                    ")";
            dataGridViewTextBoxColumn8.HeaderText = Resources.str02017 + " (" + GlobalVars.unitConverter.HeightUnit +
                                                    ")";
            dataGridViewTextBoxColumn9.HeaderText = Resources.str02023 + " (" + GlobalVars.unitConverter.HeightUnit +
                                                    ")";
            dataGridViewTextBoxColumn10.HeaderText = Resources.str02024 + " (" + GlobalVars.unitConverter.HeightUnit +
                                                     ")";

            checkBox001_CheckedChanged(checkBox001, null);

            comboBox004.Items.AddRange(new object[]
                {ePBNClass.RNAV1, ePBNClass.RNAV2, ePBNClass.RNP03, ePBNClass.RNP4, ePBNClass.RNAV5});
            comboBox004.SelectedIndex = 0;

            comboBox005.Items.AddRange(new object[] {eSensorType.GNSS, eSensorType.DME_DME});
            comboBox005.SelectedIndex = 0;

            comboBox008.Items.AddRange(new object[]
                {CodeDirection.BOTH, CodeDirection.FORWARD, CodeDirection.BACKWARD});
            comboBox008.SelectedIndex = 0;

            //numericUpDown002_ValueChanged(numericUpDown002, null);

            foreach (WPT_FIXType wpt in GlobalVars.WPTList)
                comboBox006.Items.Add(wpt);

            _reportForm = new ReportForm();
            _reportForm.Init(ReportBtn);

            bFormInitialised = true;

            comboBox006.SelectedIndex = 0;
        }

        private void RouteForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            DBModule.CloseDB();
            _reportForm.Close();

            GlobalVars.gAranGraphics.SafeDeleteGraphic(_currLeg.NominalTracktElem);
            _currLeg.Start.DeleteGraphics();
            if (_currLeg.End != null)
                _currLeg.End.DeleteGraphics();

            if (_currLeg.Forwardleg != null)
                _currLeg.Forwardleg.DeleteGraphics();

            if (_currLeg.Backwardleg != null)
                _currLeg.Backwardleg.DeleteGraphics();

            for (int i = 0, j = _Legs.Count - 1; i < _Legs.Count; i++, j--)
            {
                GlobalVars.gAranGraphics.SafeDeleteGraphic(_Legs[i].NominalTracktElem);
                _Legs[i].Start.DeleteGraphics();
                _Legs[i].End.DeleteGraphics();

                if (_Legs[i].Forwardleg != null)
                    _Legs[i].Forwardleg.DeleteGraphics();

                if (_Legs[j].Backwardleg != null)
                    _Legs[j].Backwardleg.DeleteGraphics();
            }
        }

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			// Get a handle to a copy of this form's system (window) menu
			IntPtr hSysMenu = CommonFunctions.GetSystemMenu(this.Handle, false);
			// Add a separator
			CommonFunctions.AppendMenu(hSysMenu, GlobalVars.MF_SEPARATOR, 0, string.Empty);
			// Add the About menu item
			CommonFunctions.AppendMenu(hSysMenu, GlobalVars.MF_STRING, GlobalVars.SYSMENU_ABOUT_ID, "&About…");
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

		#endregion

		#region Common Form Events

		private void TextBoxes_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
			if (e.KeyCode == Keys.Enter)
				e.IsInputKey = true;
		}

		private void FillLegObstacles()
        {
            MultiPolygon mpFullForward = new MultiPolygon(), mpFullBackward = new MultiPolygon(), mpFullPoly;
            Segment currLeg;
            GeometryOperators geoOp = new GeometryOperators();
            int n = _Legs.Count;

            for (int i = 0; i < n; i++)
            {
                currLeg = _Legs[i];

                if (currLeg.Forwardleg != null)
                    mpFullForward =
                        (MultiPolygon) geoOp.UnionGeometry(mpFullForward, currLeg.Forwardleg.FullAssesmentArea);

                if (currLeg.Backwardleg != null)
                    mpFullBackward =
                        (MultiPolygon) geoOp.UnionGeometry(mpFullBackward, currLeg.Backwardleg.FullAssesmentArea);
            }

            mpFullPoly = (MultiPolygon) geoOp.UnionGeometry(mpFullForward, mpFullBackward);

            mpFullPoly = (MultiPolygon) geoOp.Buffer(mpFullPoly, 150);

            var vsList = DBModule.GetObstaclesByPoly(mpFullPoly);
            //ObstacleContainer Obstacles;
            //DBModule.GetObstaclesByPoly(out Obstacles, mpFullPoly);

            for (int i = 0; i < n; i++)
            {
                currLeg = _Legs[i];
                currLeg.ObstacleList = DBModule.GetLegObstList(vsList, ref currLeg);
                _Legs[i] = currLeg;
            }

            _reportForm.Update(_Legs);
        }

        public void Update(List<Segment> legs)
        {
            int n = legs.Count;

            dataGridView01.RowCount = 0;
            double totalLenght = 0.0;

            for (int i = 0; i < n; i++)
            {
                Segment leg = legs[i];
                DataGridViewRow row = new DataGridViewRow();
                row.Tag = legs[i];
                row.ReadOnly = false;

                totalLenght += leg.Length;

                DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                cell.Value = (i + 1).ToString();
                row.Cells.Add(cell);

                cell = new DataGridViewTextBoxCell();
                cell.Value = leg.PBNType;
                row.Cells.Add(cell);

                //==================================================//
                row.Cells.Add(new DataGridViewTextBoxCell());
                row.Cells[2].Value = leg.Start.Name;

                row.Cells.Add(new DataGridViewTextBoxCell());
                row.Cells[3].Value = leg.End.Name;

                row.Cells.Add(new DataGridViewTextBoxCell());
                row.Cells[4].Value = Math.Round(leg.TrueTrack, 2).ToString();

                row.Cells.Add(new DataGridViewTextBoxCell());
                row.Cells[5].Value = Math.Round(ARANMath.Modulus(leg.TrueTrack - leg.Start.MagVar), 2);

                row.Cells.Add(new DataGridViewTextBoxCell());
                row.Cells[6].Value = leg.Direction;

                row.Cells.Add(new DataGridViewTextBoxCell());
                row.Cells[7].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(leg.Length, eRoundMode.NEAREST);

                row.Cells.Add(new DataGridViewTextBoxCell());
                row.Cells[8].Value = GlobalVars.unitConverter.HeightToDisplayUnits(leg.Altitude, eRoundMode.NEAREST);

                row.Cells.Add(new DataGridViewTextBoxCell());
                row.Cells[9].Value = GlobalVars.unitConverter.HeightToDisplayUnits(leg.MOC, eRoundMode.SPECIAL_CEIL);

                row.Cells.Add(new DataGridViewTextBoxCell());
                row.Cells[10].Value = GlobalVars.unitConverter.HeightToDisplayUnits(leg.MOCA, eRoundMode.SPECIAL_CEIL);

                dataGridView01.Rows.Add(row);
            }
        }

        private void NextBtn_Click(object sender, EventArgs e)
        {
            NativeMethods.ShowPandaBox(this.Handle.ToInt32());
            FillLegObstacles();
            Update(_Legs);

            NextBtn.Enabled = false;
            PrevBtn.Enabled = true;
            OkBtn.Enabled = true;
            tabControl1.SelectedIndex = 1;
            NativeMethods.HidePandaBox();
        }

        private void PrevBtn_Click(object sender, EventArgs e)
        {
            NextBtn.Enabled = true;
            PrevBtn.Enabled = false;
            OkBtn.Enabled = false;
            tabControl1.SelectedIndex = 0;
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
			string sProcName = textBox003.Text;
			string RepFileName, RepFileTitle= sProcName;

			if (!CommonFunctions.ShowSaveDialog(out RepFileName, ref RepFileTitle))
                return;

            ReportHeader pReport;
            pReport.Procedure = sProcName;
            pReport.Database = GlobalVars.gAranEnv.ConnectionInfo.Database;
			pReport.UserName = GlobalVars.gAranEnv.ConnectionInfo.UserName;

			pReport.Aerodrome = null;
            pReport.Category = null;

			ReportPoint[] GuidPoints;
			ConvertTracToPoints(out GuidPoints);

			SaveObstacleReports(RepFileName, RepFileTitle, pReport);
            SaveRoutsLog(RepFileName, RepFileTitle, pReport);
            CReportFile.SaveGeometry2(RepFileName, RepFileTitle, pReport, GuidPoints, _TotalLen, _currLeg.PBNType);
			DBModule.pObjectDir.SaveAsXml(RepFileName + "_InputData.xml");

			if (SaveProcedure())
                this.Close();
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

            int n = _Legs.Count;

            RoutsObstRep.WriteString(System.Net.WebUtility.HtmlEncode(Resources.str00129 + n + Resources.str00130));

            for (int i = 0; i < n; i++)
            {
                Segment currLeg = _Legs[i];
                var obstacles = currLeg.ObstacleList;
                //CommonFunctions.shall_SortfSortD(Obstacles.Parts);

                RoutsObstRep.SaveObstacleTable(currLeg.ObstacleList,
                    Resources.str00131 + currLeg.Start.Name + " - " + currLeg.End.Name);
                RoutsObstRep.WriteString("");
            }

            RoutsObstRep.WriteString("");
            RoutsObstRep.CloseFile();
        }

        private void SaveRoutsLog(string RepFileName, string RepFileTitle, ReportHeader pReport)
        {
            CReportFile RoutsLogRep = new CReportFile();

            RoutsLogRep.OpenFile(RepFileName + "_Log", Resources.str00128);
            RoutsLogRep.WriteString(System.Net.WebUtility.HtmlEncode(Resources.str00030 + " - " + Resources.str00128));

            RoutsLogRep.WriteString("");
            RoutsLogRep.WriteString(System.Net.WebUtility.HtmlEncode(RepFileTitle));
            RoutsLogRep.WriteHeader(pReport);

            RoutsLogRep.WriteString("");
            RoutsLogRep.WriteString("");

            //==========================================================================================================
            RoutsLogRep.WriteString("[ " + System.Net.WebUtility.HtmlEncode(tabControl1.TabPages[0].Text) + " ]");
            RoutsLogRep.WriteString("");
            RoutsLogRep.WriteString(groupBox001.Text);
            RoutsLogRep.Param(label001.Text, textBox001.Text, label002.Text);
            RoutsLogRep.Param(label003.Text, textBox002.Text, label004.Text);
            RoutsLogRep.Param(label005.Text, textBox003.Text, "");

            if (checkBox001.Checked)
                RoutsLogRep.Param(checkBox001.Text, "YES", "");
            else
                RoutsLogRep.Param(checkBox001.Text, "NO", "");
            RoutsLogRep.WriteString("");

            RoutsLogRep.WriteString(groupBox002.Text);
            RoutsLogRep.Param(label006.Text, comboBox001.Text, "");
            RoutsLogRep.Param(label007.Text, numericUpDown001.Text, "");
            RoutsLogRep.Param(label008.Text, comboBox002.Text, "");
            RoutsLogRep.Param(label009.Text, comboBox003.Text, "");
            RoutsLogRep.WriteString("");

            if (checkBox002.Checked)
                RoutsLogRep.Param(checkBox002.Text, "YES", "");
            else
                RoutsLogRep.Param(checkBox002.Text, "NO", "");

            RoutsLogRep.WriteString("");
            RoutsLogRep.WriteString("");

            if (tabControl1.SelectedIndex < 1)
            {
                RoutsLogRep.CloseFile();
                return;
            }
            //==========================================================================================================
            RoutsLogRep.WriteString("[ " + System.Net.WebUtility.HtmlEncode(tabControl1.TabPages[1].Text) + " ]");
            RoutsLogRep.WriteString("");
            RoutsLogRep.WriteTab(dataGridView01);

            RoutsLogRep.WriteString("");
            RoutsLogRep.WriteString("");

            RoutsLogRep.CloseFile();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }

        private void ReportBtn_CheckedChanged(object sender, EventArgs e)
        {
            //if (_Legs.Count == 0)
            //	return;

            if (ReportBtn.Checked)
                _reportForm.Show(GlobalVars.Win32Window);
            else
                _reportForm.Hide();
        }

        #endregion

        private int ConvertTracToPoints(out ReportPoint[] GuidPoints)
        {
            int n = _Legs.Count;
            if (n == 0)
            {
                GuidPoints = new ReportPoint[0];
                return 0;
            }

            GuidPoints = new ReportPoint[n + 1];
            Segment leg;

            for (int i = 0; i < n; i++)
            {
                leg = _Legs[i];
                GuidPoints[i].TrueTrack = leg.TrueTrack;
                GuidPoints[i].MagnTrack = NativeMethods.Modulus(leg.TrueTrack - leg.Start.MagVar);

                GuidPoints[i].ReverseTrueTrack = leg.ReverseTrueTrack;
                GuidPoints[i].ReverseMagnTrack = NativeMethods.Modulus(leg.ReverseTrueTrack - leg.End.MagVar);

                GuidPoints[i].DistToNext = leg.Length;
                GuidPoints[i].Height = -1.0; // leg.UpperLimit;
                GuidPoints[i].Radius = -1.0;

                GuidPoints[i].Description = leg.Start.Name;
                GuidPoints[i].Lat = leg.Start.GeoPt.Y;
                GuidPoints[i].Lon = leg.Start.GeoPt.X;
            }

            leg = _Legs[n - 1];
            GuidPoints[n].TrueTrack = leg.TrueTrack;
            GuidPoints[n].MagnTrack = NativeMethods.Modulus(leg.TrueTrack - leg.Start.MagVar);

            GuidPoints[n].ReverseTrueTrack = leg.ReverseTrueTrack;
            GuidPoints[n].ReverseMagnTrack = NativeMethods.Modulus(leg.ReverseTrueTrack - leg.End.MagVar);

            GuidPoints[n].DistToNext = -1.0;
            GuidPoints[n].Height = -1.0; // leg.UpperLimit;
            GuidPoints[n].Radius = -1.0;

            GuidPoints[n].Description = leg.End.Name;
            GuidPoints[n].Lat = leg.End.GeoPt.Y;
            GuidPoints[n].Lon = leg.End.GeoPt.X;

            return n + 1;
        }

        private RouteSegment CreateRouteSegment(int num, Route pProcedure, ref EnRouteSegmentPoint pEndPoint)
        {
            double PriorFixTolerance, PostFixTolerance, fTmp;

            SegmentPoint pSegmentPoint;

            RouteSegment pRouteSegment;
            EnRouteSegmentPoint pStartPoint;

            ValDistance pDistance;
            ValDistanceSigned pDistanceSigned;
            ValDistanceVertical pDistanceVertical;

            Feature pFixDesignatedPoint;
            SignificantPoint pFIXSignPt;
            PointReference pPointReference;
            Surface pSurface;

            UomSpeed mUomSpeed;
            UomDistance mUomHDistance;
            UomDistanceVertical mUomVDistance;

            UomSpeed[] uomSpeedTab;
            UomDistance[] uomDistHorTab;
            UomDistanceVertical[] uomDistVerTab;

            uomSpeedTab = new UomSpeed[]
            {
                UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN,
                UomSpeed.FT_SEC
            };
            uomDistHorTab = new UomDistance[]
                {UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI};
            uomDistVerTab = new UomDistanceVertical[] {UomDistanceVertical.M, UomDistanceVertical.FT};

            mUomSpeed = uomSpeedTab[GlobalVars.unitConverter.SpeedUnitIndex];
            mUomVDistance = uomDistVerTab[GlobalVars.unitConverter.HeightUnitIndex];
            mUomHDistance = uomDistHorTab[GlobalVars.unitConverter.DistanceUnitIndex];

            pRouteSegment = DBModule.pObjectDir.CreateFeature<RouteSegment>();

            pRouteSegment.UpperLimitReference = CodeVerticalReference.MSL;

            Segment currLeg = _Legs[num];
            FIX StartFix = currLeg.Start;
            FIX EndFix = currLeg.End;

            if (ARANMath.SubtractAngles(EndFix.EntryDirection, EndFix.OutDirection) < ARANMath.DegToRad(5))
            {
                if (EndFix.TurnDirection == TurnDirection.CW)
                    pRouteSegment.TurnDirection = CodeDirectionTurn.RIGHT;
                else
                    pRouteSegment.TurnDirection = CodeDirectionTurn.LEFT;
            }

            if (currLeg.Direction != 0)
            {
                RouteAvailability Availability = new RouteAvailability();
                Availability.Direction = currLeg.Direction;

                //if (currLeg.Direction == CodeDirection.FORWARD)
                //	Availability.Direction = CodeDirection.FORWARD;
                //else if (currLeg.Direction == CodeDirection.BACKWARD)
                //	Availability.Direction = CodeDirection.BACKWARD;
                //else if (currLeg.Direction == CodeDirection.BOTH)
                //	Availability.Direction = CodeDirection.BOTH;

                pRouteSegment.Availability.Add(Availability);
            }

            pRouteSegment.Length = new ValDistance(GlobalVars.unitConverter.DistanceToDisplayUnits(currLeg.Length),
                uomDistHorTab[GlobalVars.unitConverter.DistanceUnitIndex]);

            //pRouteSegment.Level = CodeLevel.
            //pRouteSegment.LowerLimit = new ValDistanceVertical()
            pRouteSegment.UpperLimit =
                new ValDistanceVertical(GlobalVars.unitConverter.HeightToDisplayUnits(currLeg.Altitude), mUomVDistance);

            pRouteSegment.NavigationType = CodeRouteNavigation.RNAV;
            pRouteSegment.PathType = CodeRouteSegmentPath.GDS;
            pRouteSegment.RouteFormed = pProcedure.GetFeatureRef();

            if (currLeg.PBNType == ePBNClass.RNP4)
                pRouteSegment.RequiredNavigationPerformance = 4.0;
            else
                pRouteSegment.RequiredNavigationPerformance = null;

            if (currLeg.Direction == CodeDirection.BOTH || currLeg.Direction == CodeDirection.BACKWARD)
            {
                fTmp = ARANFunctions.DirToAzimuth(EndFix.PrjPt, currLeg.Dir + Math.PI, GlobalVars.pSpRefPrj,
                    GlobalVars.pSpRefGeo);
                pRouteSegment.ReverseTrueTrack = fTmp;
                pRouteSegment.ReverseMagneticTrack = ARANMath.Modulus(fTmp - EndFix.MagVar);
            }

            if (currLeg.Direction == CodeDirection.BOTH || currLeg.Direction == CodeDirection.FORWARD)
            {
                fTmp = ARANFunctions.DirToAzimuth(StartFix.PrjPt, currLeg.Dir, GlobalVars.pSpRefPrj,
                    GlobalVars.pSpRefGeo);
                pRouteSegment.TrueTrack = fTmp;
                pRouteSegment.MagneticTrack = ARANMath.Modulus(fTmp - StartFix.MagVar);
            }

            //pRouteSegment.WidthLeft =
            //pRouteSegment.WidthRight = 

            //     pSegmentLeg.AltitudeOverrideATC =
            //     pSegmentLeg.AltitudeOverrideReference =
            //     pSegmentLeg.Duration = '???????????????????????????????? pLegs(I).valDur
            //     pSegmentLeg.Note
            //     pSegmentLeg.ReqNavPerformance
            //     pSegmentLeg.SpeedInterpretation =
            //     pSegmentLeg.ReqNavPerformance
            //     pSegmentLeg.CourseDirection =
            //     pSegmentLeg.ProcedureTransition
            // ====================================================================

            // Start Point ========================================================
            //pRouteSegment.Start

            pStartPoint = new EnRouteSegmentPoint();
            pStartPoint.Waypoint = true;
            pSegmentPoint = pStartPoint;

            pSegmentPoint.FlyOver = false;
            pSegmentPoint.RadarGuidance = false;
            pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT;

            //== ========================================================
            //StartFix.EntryDirection = currLeg.Dir;// +currLeg.MaxTurnAtStart;
            //StartFix.OutDirection = currLeg.Dir;
            StartFix.FlightPhase = eFlightPhase.Enroute;

            pFixDesignatedPoint = DBModule.CreateDesignatedPoint(StartFix.PrjPt, StartFix.Name, StartFix.OutDirection);
            pFIXSignPt = new SignificantPoint();

            if (pFixDesignatedPoint is DesignatedPoint)
                pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef();
            else if (pFixDesignatedPoint is Navaid)
                pFIXSignPt.NavaidSystem = pFixDesignatedPoint.GetFeatureRef();


            if (CommonFunctions.PriorPostFixTolerance(StartFix.TolerArea, StartFix.PrjPt, StartFix.OutDirection,
                out PriorFixTolerance, out PostFixTolerance))
            {
                pPointReference = new PointReference();
                //pPointReference.Role = CodeReferenceRole.RECNAV;

                pDistanceSigned = new ValDistanceSigned();
                pDistanceSigned.Uom = mUomHDistance;
                pDistanceSigned.Value = Math.Abs(GlobalVars.unitConverter.DistanceToDisplayUnits(PriorFixTolerance));
                pPointReference.PriorFixTolerance = pDistanceSigned;

                pDistanceSigned = new Aran.Aim.DataTypes.ValDistanceSigned();
                pDistanceSigned.Uom = mUomHDistance;
                pDistanceSigned.Value = Math.Abs(GlobalVars.unitConverter.DistanceToDisplayUnits(PostFixTolerance));
                pPointReference.PostFixTolerance = pDistanceSigned;

                pSurface = new Surface();
				MultiPolygon pl = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(StartFix.TolerArea);
                pSurface.Geo.Add(pl);

                pPointReference.FixToleranceArea = pSurface;

                pStartPoint.FacilityMakeup.Add(pPointReference);
            }

            pSegmentPoint.PointChoice = pFIXSignPt;
            pRouteSegment.Start = pStartPoint;

            //  End Of Start Point ================================================
            //=====================================================================
            //  Start Of End Point ================================================

            pEndPoint = new EnRouteSegmentPoint();
            pEndPoint.Waypoint = true;

            pSegmentPoint = pEndPoint;

            pSegmentPoint.FlyOver = false;
            pSegmentPoint.RadarGuidance = false;
            pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT;
            //pSegmentPoint.Waypoint = false; //pEndPoint.FromList

            //============================================================
            //EndFix.EntryDirection = currLeg.Dir;// +currLeg.MaxTurnAtEnd;
            //EndFix.OutDirection = currLeg.Dir;
            EndFix.FlightPhase = eFlightPhase.Enroute;

            pFIXSignPt = new SignificantPoint();
            pFixDesignatedPoint = DBModule.CreateDesignatedPoint(EndFix.PrjPt, EndFix.Name, EndFix.EntryDirection);

            if (pFixDesignatedPoint is DesignatedPoint)
                pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef();
            else if (pFixDesignatedPoint is Navaid)
                pFIXSignPt.NavaidSystem = pFixDesignatedPoint.GetFeatureRef();

            //=======================

            //EndFix.ReCreateArea();

            if (CommonFunctions.PriorPostFixTolerance(EndFix.TolerArea, EndFix.PrjPt, EndFix.EntryDirection,
                out PriorFixTolerance, out PostFixTolerance))
            {
                pPointReference = new PointReference();
                //pPointReference.Role = CodeReferenceRole.RECNAV;

                pDistanceSigned = new ValDistanceSigned();
                pDistanceSigned.Uom = mUomHDistance;
                pDistanceSigned.Value = Math.Abs(GlobalVars.unitConverter.DistanceToDisplayUnits(PriorFixTolerance));
                pPointReference.PriorFixTolerance = pDistanceSigned;

                pDistanceSigned = new Aran.Aim.DataTypes.ValDistanceSigned();
                pDistanceSigned.Uom = mUomHDistance;
                pDistanceSigned.Value = Math.Abs(GlobalVars.unitConverter.DistanceToDisplayUnits(PostFixTolerance));
                pPointReference.PostFixTolerance = pDistanceSigned;

                pSurface = new Surface();
				MultiPolygon pl = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(EndFix.TolerArea);
                pSurface.Geo.Add(pl);
                pPointReference.FixToleranceArea = pSurface;

                pEndPoint.FacilityMakeup.Add(pPointReference);
            }

            pSegmentPoint.PointChoice = pFIXSignPt;
            pRouteSegment.End = pEndPoint;

            // End of EndPoint ========================

            // Trajectory =====================================================

            Curve pCurve = new Curve();

            MultiLineString ls = GlobalVars.pspatialReferenceOperation.ToGeo<MultiLineString>(currLeg.NominalTrack);
            pCurve.Geo.Add(ls);

            pRouteSegment.CurveExtent = pCurve;

            // Protection Area ==================================================

            if (checkBox002.Checked)
            {
                ObstacleAssessmentArea pPrimProtectedArea = new ObstacleAssessmentArea();

                var obstacles = currLeg.ObstacleList.GroupBy(obs => obs.Obstacle.Identifier)
                    .Select(grp => grp.FirstOrDefault(obsGr => Math.Abs(obsGr.ReqH - grp.Max(y => y.ReqH)) < 0.0001))
                    .ToList();

                for (int i = 0; i < obstacles.Count; i++)
                {
                    var obstacle = obstacles[i];
                    Obstruction obs = new Obstruction
                    {
                        VerticalStructureObstruction = new FeatureRef(obstacles[i].Obstacle.Identifier)
                    };
                    //.pFeature.GetFeatureRef();

                    var minimumAltitude =
                        obstacle
                            .ReqH; //  Math.Max(MinimumAltitude, currLeg.MergedObstacles.Parts[currLeg.MergedObstacles.Obstacles[i].Parts[j]].ReqH);
                    var requiredClearance =
                        obstacle
                            .Moc; //Math.Max(RequiredClearance, currLeg.MergedObstacles.Parts[currLeg.MergedObstacles.Obstacles[i].Parts[j]].MOC);

                    //ReqH
                    pDistanceVertical = new ValDistanceVertical
                    {
                        Uom = mUomVDistance,
                        Value = GlobalVars.unitConverter.HeightToDisplayUnits(minimumAltitude)
                    };
                    obs.MinimumAltitude = pDistanceVertical;

                    //MOC
                    pDistance = new ValDistance
                    {
                        Uom = UomDistance.M,
                        Value = requiredClearance
                    };
                    obs.RequiredClearance = pDistance;

                    pPrimProtectedArea.SignificantObstacle.Add(obs);
                }

                GeometryOperators pGeOp = new GeometryOperators();
                MultiPolygon PrimaryArea, SecondArea, FullArea;

                if (currLeg.Direction == CodeDirection.FORWARD)
                {
                    PrimaryArea = currLeg.Forwardleg.PrimaryAssesmentArea;
                    FullArea = currLeg.Forwardleg.FullAssesmentArea;
                }
                else if (currLeg.Direction == CodeDirection.BACKWARD)
                {
                    PrimaryArea = currLeg.Backwardleg.PrimaryAssesmentArea;
                    FullArea = currLeg.Backwardleg.FullAssesmentArea;
                }
                else if (currLeg.Direction == CodeDirection.BOTH)
                {
                    PrimaryArea = (MultiPolygon) pGeOp.UnionGeometry(currLeg.Forwardleg.PrimaryAssesmentArea,
                        currLeg.Backwardleg.PrimaryAssesmentArea);
                    FullArea = (MultiPolygon) pGeOp.UnionGeometry(currLeg.Forwardleg.FullAssesmentArea,
                        currLeg.Backwardleg.FullAssesmentArea);
                }
                else
                    return pRouteSegment;

                SecondArea = (MultiPolygon) pGeOp.Difference(FullArea, PrimaryArea);

                // Primary protected Area =======================================================

                pSurface = new Surface();
                for (int i = 0; i < PrimaryArea.Count; i++)
                {
                    Polygon pl = GlobalVars.pspatialReferenceOperation.ToGeo<Polygon>(PrimaryArea[i]);
                    pSurface.Geo.Add(pl);
                }

                for (int i = 0; i < SecondArea.Count; i++)
                {
                    Polygon pl = GlobalVars.pspatialReferenceOperation.ToGeo<Polygon>(SecondArea[i]);
                    pSurface.Geo.Add(pl);
                }

                pPrimProtectedArea.Surface = pSurface;
                pPrimProtectedArea.SectionNumber = 0;
                //pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.SECONDARY;
                //pPrimProtectedArea.SurfaceZone = CodeObstructionIdSurfaceZone.HORIZONTAL;
                //pPrimProtectedArea.StartingCurve = null;
                pRouteSegment.EvaluationArea = pPrimProtectedArea;

                //pRouteSegment.
                //pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);

                // Full protected Area =======================================================
                ////pRouteSegment.CurveExtent
                //MultiPolygon pPolygon = (MultiPolygon)pTopo.Difference(currLeg.FullAssesmentArea, currLeg.PrimaryAssesmentArea);

                //if (pPolygon != null)
                //{
                //	pSurface = new Surface();
                //	for (int i = 0; i < pPolygon.Count; i++)
                //	{
                //		Polygon pl = GlobalVars.pspatialReferenceOperation.ToGeo<Polygon>(pPolygon[i]);
                //		pSurface.Geo.Add(pl);
                //	}

                //	ObstacleAssessmentArea pSecProtectedArea = new ObstacleAssessmentArea();
                //	pSecProtectedArea.Surface = pSurface;
                //	pSecProtectedArea.SectionNumber = 1;
                //	pSecProtectedArea.Type = CodeObstacleAssessmentSurface.SECONDARY;
                //	pSegmentLeg.DesignSurface.Add(pSecProtectedArea);
                //}
            }
            //  END =====================================================

            return pRouteSegment;
        }

        private bool SaveProcedure()
        {
            //  Procedure =================================================================================================
            string sProcName = textBox003.Text;
            Route pProcedure = DBModule.pObjectDir.CreateFeature<Route>();
            pProcedure.UserOrganisation = GlobalVars.CurrADHP.pAirportHeliport.ResponsibleOrganisation
                .TheOrganisationAuthority;

            // Prefix =================================================================================================
            CodeRouteDesignatorPrefix[] prefix = new CodeRouteDesignatorPrefix[]
                {CodeRouteDesignatorPrefix.K, CodeRouteDesignatorPrefix.K, CodeRouteDesignatorPrefix.U};

            if (comboBox002.SelectedIndex > 0)
                pProcedure.DesignatorPrefix = prefix[comboBox002.SelectedIndex];
            else
                pProcedure.DesignatorPrefix = null;
            // End of Prefix =================================================================================================

            // Basic Letter =================================================================================================
            CodeRouteDesignatorLetter[] basicLetter;
            if (checkBox001.Checked)
                basicLetter = new CodeRouteDesignatorLetter[]
                {
                    CodeRouteDesignatorLetter.L, CodeRouteDesignatorLetter.M, CodeRouteDesignatorLetter.N,
                    CodeRouteDesignatorLetter.P
                };
            else
                basicLetter = new CodeRouteDesignatorLetter[]
                {
                    CodeRouteDesignatorLetter.Q, CodeRouteDesignatorLetter.T, CodeRouteDesignatorLetter.Y,
                    CodeRouteDesignatorLetter.Z
                };

            if (comboBox001.SelectedIndex >= 0)
                pProcedure.DesignatorSecondLetter = basicLetter[comboBox001.SelectedIndex];
            else
                pProcedure.DesignatorSecondLetter = null;
            // End of Basic Letter ===============================================================================================

            // Designator Number =================================================================================================
            pProcedure.DesignatorNumber = (uint) numericUpDown001.Value;
            // End of Designator Number ==========================================================================================

            // Additional Letter =================================================================================================
            CodeUpperAlpha[] multipleIdentifier = new CodeUpperAlpha[]
                {CodeUpperAlpha.A, CodeUpperAlpha.Y, CodeUpperAlpha.Z, CodeUpperAlpha.F, CodeUpperAlpha.G};

            if (comboBox003.SelectedIndex > 0)
                pProcedure.MultipleIdentifier = multipleIdentifier[comboBox003.SelectedIndex];
            else
                pProcedure.MultipleIdentifier = null;

            // End of Additional Letter ==========================================================================================

            pProcedure.FlightRule = CodeFlightRule.IFR;
            pProcedure.InternationalUse = CodeRouteOrigin.BOTH;
            pProcedure.Name = sProcName;

            //pProcedure.Annotation = 
            ////pProcedure.Extension =
            //pProcedure.LocationDesignator = 
            //pProcedure.MilitaryTrainingType =
            //pProcedure.MilitaryUse
            //pProcedure.MultipleIdentifier = 
            //pProcedure.UserOrganisation

            // Legs ======================================================================================================
            EnRouteSegmentPoint pEndPoint = null;
            RouteSegment pSegmentLeg;

            uint NO_SEQ = 1;
            for (int i = 0; i < _Legs.Count; i++, NO_SEQ++)
            {
                pSegmentLeg = CreateRouteSegment(i, pProcedure, ref pEndPoint);
                //pSegmentLeg.
                //ProcedureTransitionLeg ptl = new ProcedureTransitionLeg();
                //ptl.SeqNumberARINC = NO_SEQ;
                //ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef<AbstractSegmentLegRef>();

                //pTransition.TransitionLeg.Add(ptl);
            }

            //=============================================================================

            try
            {
                DBModule.pObjectDir.SetRootFeatureType(Aim.FeatureType.Route);

                bool saveRes = DBModule.pObjectDir.Commit(new Aim.FeatureType[]
                {
                    Aim.FeatureType.Route,
                    Aim.FeatureType.RouteSegment
                });

                GlobalVars.gAranEnv.RefreshAllAimLayers();

                return saveRes;
            }
            catch (Exception ex)
            {
                throw new Exception("Error on commit." + "\r\n" + ex.Message);
            }
        }

        void FillComboBox001()
        {
            int i = Math.Max(0, comboBox001.SelectedIndex);

            comboBox001.Items.Clear();

            if (checkBox001.Checked)
                comboBox001.Items.AddRange(new object[] {'L', 'M', 'N', 'P'});
            else
                comboBox001.Items.AddRange(new object[] {'Q', 'T', 'Y', 'Z'});

            comboBox001.SelectedIndex = i;
        }

        string GenerateDesignator()
        {
            string result = comboBox002.Text + comboBox001.Text + numericUpDown001.Value.ToString("000") +
                            comboBox003.Text;
            return textBox003.Text = result;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CodeDirection granted = _currLeg.Granted;
            CodeDirection currDir = (CodeDirection) comboBox008.SelectedItem;
            _currLeg.Direction = currDir;
            _currLeg.End.Name = textBox006.Text;

            _currLeg.Start.MagVar = _startMagVar;
            _currLeg.End.MagVar = _endMagVar;

            //        if (_Legs.Count == 0)
            //_currLeg.Start.MagVar = _endMagVar;

            _currLeg.Length = _FIXDistance;

            _currLeg.UpperLimit = (int) numericUpDown002.Value;
            _currLeg.Altitude = 0.3048 * 100.0 * _currLeg.UpperLimit;
            _currLeg.Start.ConstructAltitude = _currLeg.Altitude;

            //commented by Agshin
            //_currLeg.Length = _currLeg.NominalTrack.Length;
            _TotalLen += _currLeg.Length;

            GlobalVars.gAranGraphics.SafeDeleteGraphic(_currLeg.NominalTracktElem);
            _currLeg.NominalTracktElem = GlobalVars.gAranGraphics.DrawMultiLineString(_currLeg.NominalTrack, 2, 255);

            _currLeg.ReverseTrueTrack = NativeMethods.ReturnGeodesicAzimuth(_currLeg.End.GeoPt, _currLeg.Start.GeoPt);

            _Legs.Add(_currLeg);

            Segment newLeg = new Segment();

            if (currDir == CodeDirection.BOTH)
                newLeg.Granted = granted;
            else
                newLeg.Granted = currDir;

            comboBox008.Items.Clear();
            comboBox008.Items.Add(CodeDirection.BOTH);
            if (newLeg.Granted == CodeDirection.BOTH)
            {
                comboBox008.Items.Add(CodeDirection.FORWARD);
                comboBox008.Items.Add(CodeDirection.BACKWARD);
            }
            else
                comboBox008.Items.Add(newLeg.Granted);

            newLeg.Start = (FIX) (_currLeg.End.Clone());

            newLeg.PBNType = _currLeg.PBNType;
            newLeg.SensorType = _currLeg.SensorType;

            newLeg.UpperLimit = _currLeg.UpperLimit;
            newLeg.Altitude = _currLeg.Altitude;

            newLeg.MaxTurnAtStart = _currLeg.MaxTurnAtStart;
            newLeg.MaxTurnAtEnd = _currLeg.MaxTurnAtEnd;

            newLeg.TAS = _currLeg.TAS;
            newLeg.IAS = _currLeg.IAS;
            newLeg.Dir = _currLeg.Dir;
            newLeg.MOC = _currLeg.MOC;

            newLeg.TrueTrack = _currLeg.TrueTrack;
            newLeg.Length = _currLeg.Length;

            groupBox001.Enabled = false;
            groupBox002.Enabled = false;
            comboBox006.Enabled = false;
            checkBox002.Enabled = false;
            button002.Enabled = true;

            comboBox006.Items.Clear();
            comboBox006.Items.Add(newLeg.Start);
            //label016.Text = label025.Text;

            _currLeg = newLeg;

            bCreateGeometry = false;
            try
            {
                comboBox008.SelectedItem = currDir;
                comboBox006.SelectedIndex = 0;
            }
            finally
            {
                bCreateGeometry = true;
            }

            CreateSegmentGeometry();

            _startMagVar = _endMagVar;
            txtStartMagVar.Text = _startMagVar.ToString();
            txtStartMagVar.Enabled = false;

            NextBtn.Enabled = _Legs.Count > 0 && checkBox002.Checked;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            radioButton001.Checked = true;
            radioButton003.Checked = true;

            if (_currLeg.Forwardleg != null)
                _currLeg.Forwardleg.DeleteGraphics();

            if (_currLeg.Backwardleg != null)
                _currLeg.Backwardleg.DeleteGraphics();

            _currLeg.Start.DeleteGraphics();
            _currLeg.End.DeleteGraphics();

            GlobalVars.gAranGraphics.SafeDeleteGraphic(_currLeg.NominalTracktElem);

            _currLeg = _Legs[_Legs.Count - 1];
            _Legs.RemoveAt(_Legs.Count - 1);

            GlobalVars.gAranGraphics.SafeDeleteGraphic(_currLeg.NominalTracktElem);
            _currLeg.NominalTracktElem =
                GlobalVars.gAranGraphics.DrawMultiLineString(_currLeg.NominalTrack, 1, ARANFunctions.RGB(255, 122, 122));

            textBox006.Text = _currLeg.End.Name;
            _FIXDistance = _currLeg.Length;
            _TotalLen -= _currLeg.Length;

            textBox005.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_FIXDistance, eRoundMode.CEIL).ToString();
            textBox005.Tag = textBox005.Text;

            comboBox008.Items.Clear();
            comboBox008.Items.Add(CodeDirection.BOTH);
            if (_currLeg.Granted == CodeDirection.BOTH)
            {
                comboBox008.Items.Add(CodeDirection.FORWARD);
                comboBox008.Items.Add(CodeDirection.BACKWARD);
            }
            else
                comboBox008.Items.Add(_currLeg.Granted);

            comboBox008.SelectedItem = _currLeg.Direction;
            numericUpDown002.Value = _currLeg.UpperLimit;
            numericUpDown003.Value = (decimal) ARANMath.RadToDeg(_currLeg.MaxTurnAtStart);
            numericUpDown005.Value = (decimal) ARANMath.RadToDeg(_currLeg.MaxTurnAtEnd);
            numericUpDown004.Value = (decimal) _currLeg.TrueTrack;

            comboBox006.Items.Clear();
            if (_Legs.Count > 0)
            {
                comboBox006.Items.Add(_currLeg.Start);
                txtStartMagVar.Text = _currLeg.Start.MagVar.ToString();
            }
            else
            {
                foreach (WPT_FIXType wpt in GlobalVars.WPTList)
                    comboBox006.Items.Add(wpt);

                groupBox001.Enabled = true;
                groupBox002.Enabled = true;
                comboBox006.Enabled = true;
                checkBox002.Enabled = true;
                button002.Enabled = false;
                txtStartMagVar.Enabled = true;
            }

            comboBox006.SelectedItem = _currLeg.Start;
            NextBtn.Enabled = _Legs.Count > 0 && checkBox002.Checked;
        }

        private void checkBox001_CheckedChanged(object sender, EventArgs e)
        {
            FillComboBox001();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateDesignator();
        }

        private void numericUpDown002_ValueChanged(object sender, EventArgs e)
        {
            _currLeg.UpperLimit = (int) numericUpDown002.Value;
            _currLeg.Altitude = 0.3048 * 100.0 * _currLeg.UpperLimit;
            _currLeg.Start.ConstructAltitude = _currLeg.Altitude;

            //_ws = CommonFunctions.WindSpeed(_currLeg.UpperLimit);
            //textBox007.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_ws, eRoundMode.SPECIAL_NERAEST).ToString();
            textBox004.Text = GlobalVars.unitConverter
                .SpeedToDisplayUnits(_currLeg.Start.ConstructTAS, eRoundMode.SPECIAL_NEAREST)
                .ToString();

            CalcMinLenght();
        }

		private void comboBox004_SelectedIndexChanged(object sender, EventArgs e)
		{
			_currLeg.PBNType = (ePBNClass)comboBox004.SelectedItem;

			if (!bFormInitialised)
				return;

			_currLeg.Start.PBNType = _currLeg.PBNType;

			bool radioButton003_Checked = radioButton003.Checked;
			FillComboBox009();

			if (radioButton003_Checked == radioButton003.Checked)
				CreateSegmentGeometry();
		}

        private void comboBox005_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currLeg.SensorType = (eSensorType) comboBox005.SelectedItem;

            if (!bFormInitialised)
                return;

            _currLeg.Start.SensorType = _currLeg.SensorType;

            bool radioButton003_Checked = radioButton003.Checked;
            FillComboBox009();

            if (radioButton003_Checked == radioButton003.Checked)
                CreateSegmentGeometry();
        }

        private void comboBox006_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!bFormInitialised)
                return;

            if (_currLeg.Start != null)
                _currLeg.Start.DeleteGraphics();

            if (comboBox006.SelectedItem is WPT_FIXType)
            {
                WPT_FIXType selected = (WPT_FIXType) comboBox006.SelectedItem;
                //label016.Text = selected.TypeCode.Tostring();

                _currLeg.Start = new FIX(eFIXRole.IAF_GT_56_, selected, GlobalVars.gAranEnv);
            }
            else
            {
                FIX selected = (FIX) comboBox006.SelectedItem;
                _currLeg.Start = (FIX) selected.Clone();
            }

            double dir = ARANFunctions.AztToDirection(_currLeg.Start.GeoPt, _currLeg.TrueTrack, GlobalVars.pSpRefGeo,
                GlobalVars.pSpRefPrj);

            _currLeg.Dir = dir;
            _currLeg.Start.BankAngle = _bankAngle;
            _currLeg.Start.IAS = _IAS;
			_currLeg.Start.FlightPhase = eFlightPhase.Enroute;

			_currLeg.Start.PBNType = (ePBNClass) comboBox004.SelectedItem;
            _currLeg.Start.SensorType = (eSensorType) comboBox005.SelectedItem;

			numericUpDown002_ValueChanged(numericUpDown002, null);

            WPT_FIXType oldWPT = default(WPT_FIXType);
            if (comboBox007.SelectedIndex >= 0)
                oldWPT = (WPT_FIXType) comboBox007.SelectedItem;

            double minangle = 0.0, maxangle = 0.0;

            if (_Legs.Count > 0)
            {
                double MaxTurn = ARANMath.DegToRad((double) numericUpDown003.Value);
                Segment prev = _Legs[_Legs.Count - 1];
                minangle = prev.Dir - MaxTurn;
                maxangle = prev.Dir + MaxTurn;
            }

            int k = 0;

            comboBox007.Items.Clear();
            foreach (WPT_FIXType wpt in GlobalVars.WPTList)
            {
                double minDist = 1000.0;
                double dist = ARANFunctions.ReturnDistanceInMeters(_currLeg.Start.PrjPt, wpt.pPtPrj);
                if (dist < minDist)
                    continue;

                if (_Legs.Count > 0)
                {
                    dir = ARANFunctions.ReturnAngleInRadians(_currLeg.Start.PrjPt, wpt.pPtPrj);
                    if (!ARANFunctions.AngleInSectorRad(dir, minangle, maxangle))
                        continue;
                }

                if (wpt.Identifier == oldWPT.Identifier)
                    k = comboBox007.Items.Count;

                comboBox007.Items.Add(wpt);
            }

            if (comboBox007.Items.Count > 0)
            {
                radioButton002.Enabled = true;
                comboBox007.SelectedIndex = k;
            }
            else
            {
                radioButton001.Checked = true;
                radioButton002.Enabled = false;
                label020.Text = "";
            }
        }

        private void CalcMinLenght()
        {
            double fromAngle = ARANMath.DegToRad((double) numericUpDown003.Value);
            double minOut = _currLeg.Start.CalcConstructionFromMinStablizationDistance(fromAngle);

            double ToAngle = ARANMath.DegToRad((double) numericUpDown005.Value);
            double minTo = _currLeg.Start.CalcConstructionInToMinStablizationDistance(ToAngle);

            _currLeg.MaxTurnAtStart = fromAngle;
            _currLeg.MaxTurnAtEnd = ToAngle;

            _minLenght = minOut + minTo;
            textBox008.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_minLenght).ToString();

            bool radioButton003_Checked = radioButton003.Checked;
            FillComboBox009();

            if (radioButton003.Checked)
            {
                textBox005.Tag = null;
                textBox005_Validating(textBox005, null);
            }
        }

        void CreateSegmentGeometry()
        {
            if (!bCreateGeometry)
                return;

            GlobalVars.gAranGraphics.SafeDeleteGraphic(_currLeg.NominalTracktElem);

            CodeDirection currSegDir = (CodeDirection) comboBox008.SelectedItem;
            _currLeg.Direction = currSegDir;

            if (_currLeg.End != null)
                _currLeg.End.DeleteGraphics();

            double currDir = ARANFunctions.AztToDirection(_currLeg.Start.GeoPt, _currLeg.TrueTrack,
                GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);

            if (radioButton004.Checked)
            {
                WPT_FIXType selected = (WPT_FIXType) comboBox009.SelectedItem;
                currDir = ARANFunctions.ReturnAngleInRadians(_currLeg.Start.PrjPt, selected.pPtPrj);
            }
            else if (radioButton002.Checked)
            {
                WPT_FIXType selected = (WPT_FIXType) comboBox007.SelectedItem;
                currDir = ARANFunctions.ReturnAngleInRadians(_currLeg.Start.PrjPt, selected.pPtPrj);
            }

            _currLeg.Dir = currDir;

            if (radioButton004.Checked)
            {
                _currLeg.End = new FIX(eFIXRole.TP_, (WPT_FIXType) comboBox009.SelectedItem, GlobalVars.gAranEnv);
            }
            else
            {
                if (_currLeg.End == null)
                    _currLeg.End = new FIX(eFIXRole.TP_, GlobalVars.gAranEnv);
				//_currLeg.End.PrjPt = ARANFunctions.LocalToPrj(_currLeg.Start.PrjPt, currDir, _FIXDistance);

				////added by Agshin
				var geoPt = ARANFunctions.PointAlongGeodesic(_currLeg.Start.GeoPt, _currLeg.TrueTrack, _FIXDistance);
				_currLeg.End.GeoPt.X = geoPt.X;
				_currLeg.End.GeoPt.Y = geoPt.Y;
				_currLeg.End.PrjPt = GlobalVars.pspatialReferenceOperation.ToPrj(geoPt);
			}

            _currLeg.Start.EntryDirection = currDir;
            _currLeg.Start.OutDirection = currDir;

            _currLeg.End.BankAngle = _bankAngle;
            _currLeg.End.IAS = _IAS;

			_currLeg.End.FlightPhase = eFlightPhase.Enroute;
			_currLeg.End.PBNType = _currLeg.PBNType;
            _currLeg.End.SensorType = _currLeg.SensorType;


			_currLeg.End.ConstructAltitude = _currLeg.Start.ConstructAltitude;

            _currLeg.End.EntryDirection = currDir;
            _currLeg.End.OutDirection = currDir;

            LineString ls = new LineString();
            ls.Add(_currLeg.Start.PrjPt);
            ls.Add(_currLeg.End.PrjPt);

            _currLeg.NominalTrack = new MultiLineString();
            _currLeg.NominalTrack.Add(ls);

            if (checkBox002.Checked)
            {
                if (_currLeg.Direction == CodeDirection.BOTH || _currLeg.Direction == CodeDirection.FORWARD)
                {
                    if (_currLeg.Forwardleg == null)
                        _currLeg.Forwardleg = new EnRouteLeg(_currLeg.Start, _currLeg.End, CodeDirection.FORWARD,
                            GlobalVars.gAranEnv);
                    else
                    {
                        _currLeg.Forwardleg.StartFIX.Assign(_currLeg.Start);
                        _currLeg.Forwardleg.EndFIX.Assign(_currLeg.End);
                    }

                    if (_Legs.Count == 0)
                        _currLeg.Forwardleg.StartFIX.EntryDirection = currDir;
                    else
                    {
                        _currLeg.Forwardleg.StartFIX.EntryDirection = _Legs[_Legs.Count - 1].End.EntryDirection;
                        if (_Legs[_Legs.Count - 1].Forwardleg != null)
                            _Legs[_Legs.Count - 1].Forwardleg.EndFIX.OutDirection =
                                _currLeg.Forwardleg.StartFIX.OutDirection;
                    }
                }
                else
                {
                    if (_currLeg.Forwardleg != null)
                        _currLeg.Forwardleg.DeleteGraphics();
                    _currLeg.Forwardleg = null;
                }

                if (_currLeg.Direction == CodeDirection.BOTH || _currLeg.Direction == CodeDirection.BACKWARD)
                {
                    if (_currLeg.Backwardleg == null)
                        _currLeg.Backwardleg = new EnRouteLeg(_currLeg.Start, _currLeg.End, CodeDirection.BACKWARD,
                            GlobalVars.gAranEnv);
                    else
                    {
                        _currLeg.Backwardleg.StartFIX.Assign(_currLeg.Start);
                        _currLeg.Backwardleg.EndFIX.Assign(_currLeg.End);
                    }

                    if (_Legs.Count == 0)
                        _currLeg.Backwardleg.StartFIX.EntryDirection = currDir;
                    else
                    {
                        _currLeg.Backwardleg.StartFIX.EntryDirection = _Legs[_Legs.Count - 1].End.EntryDirection;
                        if (_Legs[_Legs.Count - 1].Backwardleg != null)
                            _Legs[_Legs.Count - 1].Backwardleg.EndFIX.OutDirection =
                                _currLeg.Backwardleg.StartFIX.OutDirection;
                    }

                    _currLeg.Backwardleg.CreateGeometry(null, CodeDirection.BACKWARD);
                    _currLeg.Backwardleg.RefreshGraphics();
				}
				else
                {
                    if (_currLeg.Backwardleg != null)
                        _currLeg.Backwardleg.DeleteGraphics();
                    _currLeg.Backwardleg = null;
                }

                EnRouteLeg prevForward = null, prevBackward = _currLeg.Backwardleg;

                for (int i = 0, j = _Legs.Count - 1; i < _Legs.Count; i++, j--)
                {
                    if (_Legs[i].Forwardleg != null)
                    {
                        _Legs[i].Forwardleg.CreateGeometry(prevForward, CodeDirection.FORWARD);
                        _Legs[i].Forwardleg.RefreshGraphics();
					}

					prevForward = _Legs[i].Forwardleg;

                    if (_Legs[j].Backwardleg != null)
                    {
                        _Legs[j].Backwardleg.CreateGeometry(prevBackward, CodeDirection.BACKWARD);
                        _Legs[j].Backwardleg.RefreshGraphics();
					}
					prevBackward = _Legs[j].Backwardleg;

                    _Legs[i].Start.RefreshGraphics();
                    _Legs[i].End.RefreshGraphics();
                }

                if (_currLeg.Forwardleg != null)
                {
                    _currLeg.Forwardleg.CreateGeometry(prevForward, CodeDirection.FORWARD);
                    _currLeg.Forwardleg.RefreshGraphics();
                }
            }

            _currLeg.NominalTracktElem =
                GlobalVars.gAranGraphics.DrawMultiLineString(_currLeg.NominalTrack, 1, ARANFunctions.RGB(255, 122, 122));

            double TurnAngle = 0;
            if (_Legs.Count > 0)
                TurnAngle = ARANMath.SubtractAnglesInDegs(_Legs[_Legs.Count - 1].TrueTrack, _currLeg.TrueTrack);

            textBox009.Text = TurnAngle.ToString("0.00");
            _currLeg.Start.RefreshGraphics();
            _currLeg.End.RefreshGraphics();
        }

        #region Segment direction

        private void numericUpDown003_ValueChanged(object sender, EventArgs e)
        {
            if (_Legs.Count > 0)
                comboBox006_SelectedIndexChanged(comboBox006, null);
            else
                CalcMinLenght();
        }

        private void numericUpDown005_ValueChanged(object sender, EventArgs e)
        {
            CalcMinLenght();
        }

        private void radioButton001_CheckedChanged(object sender, EventArgs e)
        {
            if (!((RadioButton) sender).Checked)
                return;

            if (sender == radioButton001)
            {
                numericUpDown004.ReadOnly = false;
                comboBox007.Enabled = false;

                numericUpDown004_ValueChanged(numericUpDown004);
            }
            else
            {
                numericUpDown004.ReadOnly = true;
                comboBox007.Enabled = true;

                comboBox007_SelectedIndexChanged(comboBox007);
            }
        }

        private void numericUpDown004_ValueChanged(object sender, EventArgs e = null)
        {
            if (numericUpDown004.Value < 0)
            {
                numericUpDown004.Value += 360;
                return;
            }

            if (numericUpDown004.Value >= 360)
            {
                numericUpDown004.Value -= 360;
                return;
            }

            double nnAzt = (double) numericUpDown004.Value;
            if (_Legs.Count > 0)
            {
                Segment prev = _Legs[_Legs.Count - 1];
                double MaxTurn = (double) numericUpDown003.Value;

                _minAzimuth = NativeMethods.Modulus(prev.TrueTrack - MaxTurn);
                _maxAzimuth = NativeMethods.Modulus(prev.TrueTrack + MaxTurn);

                if (!ARANFunctions.AngleInSector(nnAzt, _minAzimuth, _maxAzimuth))
                {
                    double dMin, dMax;
                    dMin = ARANMath.SubtractAnglesInDegs(_minAzimuth, nnAzt);
                    dMax = ARANMath.SubtractAnglesInDegs(_maxAzimuth, nnAzt);

                    if (dMin < dMax)
                        numericUpDown004.Value = (decimal) _minAzimuth;
                    else
                        numericUpDown004.Value = (decimal) _maxAzimuth;

                    return;
                }
            }

            if (!radioButton001.Checked)
                return;

			_currLeg.TrueTrack = nnAzt;

			double dir = ARANFunctions.AztToDirection(_currLeg.Start.GeoPt, nnAzt, GlobalVars.pSpRefGeo,
                GlobalVars.pSpRefPrj);

            _currLeg.Dir = dir;

            bool radioButton003_Checked = radioButton003.Checked;
            FillComboBox009();

            if (radioButton003_Checked == radioButton003.Checked)
                CreateSegmentGeometry();
        }

		private void comboBox007_SelectedIndexChanged(object sender, EventArgs e = null)
		{
			WPT_FIXType selected = (WPT_FIXType)comboBox007.SelectedItem;
			label020.Text = selected.TypeCode.Tostring();

			if (!radioButton002.Checked)
				return;

			//double dir = ARANFunctions.ReturnAngleInRadians(_currLeg.Start.PrjPt, selected.pPtPrj);
			//double nnAzt = ARANFunctions.DirToAzimuth(_currLeg.Start.PrjPt, dir, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			//added by Agshin
			double dir, nnAzt;
			ARANFunctions.ReturnGeodesicAzimuth(_currLeg.Start.GeoPt, selected.pPtGeo, out nnAzt, out double backwardAzimuth);
			dir = ARANFunctions.AztToDirection(_currLeg.Start.GeoPt, nnAzt, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);

			numericUpDown004.Value = (decimal)nnAzt;

			_currLeg.TrueTrack = nnAzt;
			_currLeg.Dir = dir;

			bool radioButton003_Checked = radioButton003.Checked;
			FillComboBox009();

			if (radioButton003_Checked == radioButton003.Checked)
				CreateSegmentGeometry();
		}

        #endregion

        #region Segment distance

        private void checkBox002_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox002.Checked)
            {
                if (_currLeg.Forwardleg != null)
                    _currLeg.Forwardleg.DeleteGraphics();

                if (_currLeg.Backwardleg != null)
                    _currLeg.Backwardleg.DeleteGraphics();
            }

            CreateSegmentGeometry();
        }

        private void comboBox008_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!bFormInitialised || !checkBox002.Checked)
                return;

            CreateSegmentGeometry();
        }

        void FillComboBox009()
        {
            WPT_FIXType oldWPT = default(WPT_FIXType);
            if (comboBox009.SelectedIndex >= 0)
                oldWPT = (WPT_FIXType) comboBox009.SelectedItem;

            comboBox009.Items.Clear();
            int k = 0;

            foreach (WPT_FIXType wpt in GlobalVars.WPTList)
            {
                Point LocalSigPoint = ARANFunctions.PrjToLocal(_currLeg.Start.PrjPt, _currLeg.Dir, wpt.pPtPrj);

                if (LocalSigPoint.X > 0 && Math.Abs(LocalSigPoint.Y) <= 0.25 * _currLeg.Start.XTT)
                    if (LocalSigPoint.X >= _minLenght && LocalSigPoint.X <= maxDistance)
                    {
                        if (wpt.Identifier == oldWPT.Identifier)
                            k = comboBox009.Items.Count;

                        comboBox009.Items.Add(wpt);
                    }
            }

            if (comboBox009.Items.Count > 0)
            {
                radioButton004.Enabled = true;
                comboBox009.SelectedIndex = k;
            }
            else
            {
                radioButton003.Checked = true;
                radioButton004.Enabled = false;

                label025.Text = "";
            }
        }

        private void radioButton003_CheckedChanged(object sender, EventArgs e)
        {
            if (!((RadioButton) sender).Checked)
                return;

            if (sender == radioButton003)
            {
                textBox005.ReadOnly = false;
                textBox006.ReadOnly = false;
                comboBox009.Enabled = false;
                textBox006.Text = "WPT00";

                textBox005.Tag = null;
                textBox005_Validating(textBox005);
            }
            else
            {
                textBox005.ReadOnly = true;
                textBox006.ReadOnly = true;
                comboBox009.Enabled = true;

                comboBox009_SelectedIndexChanged(comboBox009);
            }
        }

        private void textBox005_KeyPress(object sender, KeyPressEventArgs e)
        {
            char eventChar = e.KeyChar;
            if (eventChar == 13)
                textBox005_Validating(textBox005, new System.ComponentModel.CancelEventArgs());
            else
                CommonFunctions.TextBoxFloat(ref eventChar, textBox005.Text);

            e.KeyChar = eventChar;
            if (eventChar == 0)
                e.Handled = true;
        }

        private void textBox005_Validating(object sender, CancelEventArgs e = null)
        {
            double fTmp;
            bool update = false;

            if (double.TryParse(textBox005.Text, out fTmp))
            {
                if (textBox005.Tag != null && textBox005.Tag.ToString() == textBox005.Text)
                    return;

                fTmp = _FIXDistance = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

                if (_FIXDistance < _minLenght)
                    _FIXDistance = _minLenght;

                if (_FIXDistance > maxDistance)
                    _FIXDistance = maxDistance;

                if (_FIXDistance != fTmp)
                    textBox005.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_FIXDistance, eRoundMode.CEIL)
                        .ToString();
                update = true;
            }
            else if (double.TryParse(textBox005.Tag.ToString(), out fTmp))
                textBox005.Text = textBox005.Tag.ToString();
            else
                textBox005.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_FIXDistance, eRoundMode.CEIL)
                    .ToString();

            textBox005.Tag = textBox005.Text;

            if (!update)
                return;

            CreateSegmentGeometry();
        }

        private void comboBox009_SelectedIndexChanged(object sender, EventArgs e = null)
        {
            WPT_FIXType selected = (WPT_FIXType) comboBox009.SelectedItem;
            label025.Text = selected.TypeCode.Tostring();

            if (!radioButton004.Checked)
                return;

            textBox006.Text = comboBox009.Text;

            //added by Agshin
            _FIXDistance = ARANFunctions.ReturnGeodesicDistance(_currLeg.Start.GeoPt, selected.pPtGeo);
            //_FIXDistance = ARANFunctions.ReturnDistanceInMeters(_currLeg.Start.PrjPt, selected.pPtPrj);
            textBox005.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_FIXDistance, eRoundMode.CEIL).ToString();
            textBox005.Tag = textBox005.Text;

            CreateSegmentGeometry();
        }

        private void comboBox010_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox010.SelectedIndex < 0)
                return;

            _moc = GlobalVars.unitConverter.HeightToInternalUnits(double.Parse(comboBox010.Text));
            _currLeg.MOC = _moc;
        }

        #endregion

        private void textBox010_KeyPress(object sender, KeyPressEventArgs e)
        {
            char eventChar = e.KeyChar;
            if (eventChar == 13)
                textBox010_Validating(textBox010, new System.ComponentModel.CancelEventArgs());
            else if (CommonFunctions.TextBoxSignedFloat(ref eventChar, textBox010.Text))
            {
                double fTmp;
                if (double.TryParse(textBox010.Text, out fTmp))
                {
                    fTmp = -fTmp;
                    textBox010.Text = fTmp.ToString();
                }
                else if (textBox010.Text.Trim() == "")
                    textBox010.Text = "-";
                else
                    textBox010.Text = "";
            }

            e.KeyChar = eventChar;
            if (eventChar == 0)
                e.Handled = true;
        }

        private void textBox010_Validating(object sender, CancelEventArgs e)
        {
            double fTmp;
            if (double.TryParse(textBox010.Text, out fTmp))
                _endMagVar = fTmp;
        }

        private void txtStartMagVar_Validating(object sender, CancelEventArgs e)
        {
            double fTmp;
            if (double.TryParse(txtStartMagVar.Text, out fTmp))
                _startMagVar = fTmp;
        }

        private void txtStartMagVar_KeyPress(object sender, KeyPressEventArgs e)
        {
            char eventChar = e.KeyChar;
            if (eventChar == 13)
                txtStartMagVar_Validating(textBox010, new System.ComponentModel.CancelEventArgs());
            else if (CommonFunctions.TextBoxSignedFloat(ref eventChar, textBox010.Text))
            {
                double fTmp;
                if (double.TryParse(textBox010.Text, out fTmp))
                {
                    fTmp = -fTmp;
                    txtStartMagVar.Text = fTmp.ToString();
                }
                else if (textBox010.Text.Trim() == "")
                    txtStartMagVar.Text = "-";
                else
                    txtStartMagVar.Text = "";
            }

            e.KeyChar = eventChar;
            if (eventChar == 0)
                e.Handled = true;
        }

    }
}
