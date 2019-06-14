using Aran.AranEnvironment.Symbols;
using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using Aran.PANDA.RNAV.PBNAPVBaroVNav.Properties;
using System;
using System.Windows.Forms;

namespace Aran.PANDA.RNAV.PBNAPVBaroVNav
{
	public partial class MainForm : Form
	{
		// Input
		private FIX _IF;
		private FIX _FAF;
		private MAPt _MAPt;
		private MATF _MATF;
		private FIX _MAHF;

		// === Legs
		private LegBase _ISegment;			// IF - FAF
		private LegBase _FASegment;			// FAF - MAPt
		private LegBase _MASegment;			// MAPt - MATF

		// Local
		private int _HorzLPlaneGr;
		private int _HorzPPlaneGr;
		private int _HorzRPlaneGr;

		private int _MASLPlaneGr;
		private int _MASPPlaneGr;
		private int _MASRPlaneGr;

		private int _FASLPlaneGr;
		private int _FASPPlaneGr;
		private int _FASRPlaneGr;

		private int _FASLPlaneGrI;
		private int _FASPPlaneGrI;
		private int _FASRPlaneGrI;

		//===================================================================
		private Interval _MAHFTHRDistRange;
		private double _MAHFTHRDist;

		private Interval _FAPheightRange;
		private double _FAPheight;

		private Interval _MA_PDGRange;
		private double _MA_PDG;

		private Interval _VPARange;
		private double _VPA;

		private double _FAFTHRDist;
		private double _MAPtTHRDist;
		private double _THRSOCDist;

		//= 3 ============================================

		private Interval _IFDistanceRange;
		private double _IFDistance;

		private Interval _IFheightRange;
		private double _IFheight;

		private Interval _PlannedAngleRange;
		private double _PlannedAngle;

		private Interval _IFIASRange;

		private double _IntADG;
		private double _HorizSeg;

		// =====================================================================
		/// <summary>
		/// 
		/// </summary>
		private int CurrPage
		{
			get { return pageContMain.SelectedIndex; }

			set
			{
				pageContMain.SelectedIndex = value;

				PrevBtn.Enabled = value > 0;
				NextBtn.Enabled = value < pageContMain.TabPages.Count - 1;
				OkBtn.Enabled = value == pageContMain.TabPages.Count - 1;

				int n = _label1.Length;
				for (int i = 0; i < n; i++)
				{
					//Label1[i].Visible = tabControl1.TabPages[i].Visible;
					_label1[i].ForeColor = System.Drawing.Color.Silver;
					_label1[i].Font = new System.Drawing.Font(_label1[i].Font, System.Drawing.FontStyle.Regular);
				}

				_label1[value].ForeColor = System.Drawing.Color.FromArgb(0XFF8000);
				_label1[value].Font = new System.Drawing.Font(_label1[value].Font, System.Drawing.FontStyle.Bold);

				this.Text = Resources.str00033 + "  [" + _label1[value].Text + "]";
				this.HelpContext = 3100 + 100 * value;
			}
		}

		// ============================================================ Page 1
		private RWYType[] _RWYDATA;

		private RWYType _selectedRWY;
		private Point _RWYTHRPrj;

		private aircraftCategory _AirCat;

		private D3DPolygone[] _FASPlanes;
		private D3DPolygone[] _HorzPlanes;
		private D3DPolygone[] _MASPlanes;

		// ====
		private ObstacleContainer _FASObstList;
		private ObstacleContainer _HorzObstList;
		private ObstacleContainer _MASObstList;

		private ObstacleContainer _IAPVSObstList;
		private ObstacleContainer _InterApprObstList;
		// ====

		private double _xOrigin;
		private double _margin;
		private double _hLost;
		private double _TAS;
		private double _ArDir;
		private double _hMaxSS;

		private double _FAS;
		private double _FAS_;
		private double _FAS__;

		//====================================================================================================
		private int HelpContext;
		private bool bFormInitialised;

		private Label[] _label1;
		//private double[] _grTable = { 0.020, 0.025, 0.030, 0.035, 0.04, 0.045, 0.050, 0.055, 0.060 };
		ReportForm _reportForm;

		#region Form
		public MainForm()
		{
			bFormInitialised = false;

			InitializeComponent();

			_label1 = new Label[] { PageLabel0, PageLabel1, PageLabel2 };
			//screenCapture = GlobalVars.gAranEnv.GetScreenCapture(Aim.FeatureType.StandardInstrumentDeparture.ToString());

			pageContMain.TabPages[0].Text = Resources.str01000;
			pageContMain.TabPages[1].Text = Resources.str02000;
			pageContMain.TabPages[2].Text = Resources.str03000;

			PageLabel0.Text = Resources.str01000;
			PageLabel1.Text = Resources.str02000;
			PageLabel2.Text = Resources.str03000;

			CurrPage = 0;
			pageContMain.SelectedIndex = CurrPage;

			//// ===============================================================
			PrevBtn.Text = Resources.str00002;
			NextBtn.Text = Resources.str00003;
			OkBtn.Text = Resources.str00004;
			CancelBtn.Text = Resources.str00005;
			ReportBtn.Text = Resources.str00006;

			textBox01006.Text = GlobalVars.CurrADHP.LowestTemperature.ToString("00.0");

			// =======================================================================

			//=== IAF

			//=== IF
			_IF = new FIX(eFIXRole.IF_, GlobalVars.gAranEnv)
			{
				FlyMode = eFlyMode.Flyby,
				ISAtC = GlobalVars.constants.Pansops[ePANSOPSData.arISAmax].Value
			};

			_IFDistanceRange.Min = GlobalVars.constants.Pansops[ePANSOPSData.arImRange_Min].Value;
			_IFDistanceRange.Max = GlobalVars.constants.Pansops[ePANSOPSData.arImRange_Max].Value;

			//=== FAF
			//=== FAF
			_FAF = new FIX(eFIXRole.PBN_FAF, GlobalVars.gAranEnv)
			{
				FlyMode = eFlyMode.Flyby,
				Name = "FAP",
				SensorType = eSensorType.GNSS,
				ISAtC = GlobalVars.constants.Pansops[ePANSOPSData.arISAmax].Value
			};
			//=== MAPt

			_MAPt = new MAPt(eFIXRole.PBN_MAPt, GlobalVars.gAranEnv)
			{
				FlightPhase = eFlightPhase.FAFApch,
				FlyMode = eFlyMode.Flyover,
				Name = "MAPt",
				SensorType = eSensorType.GNSS,
				ISAtC = GlobalVars.constants.Pansops[ePANSOPSData.arISAmax].Value
			};

			//=== SOC
			//=== MATF

			_MATF = new MATF(GlobalVars.gAranEnv)
			{
				Role = eFIXRole.PBN_MATF_LT_28,
				maPt = _MAPt,
				ISAtC = GlobalVars.constants.Pansops[ePANSOPSData.arISAmax].Value
			};

			_FAF.PBNType = ePBNClass.RNP_APCH; 
			//=== MAHF
			_MAHF = new FIX(eFIXRole.MAHF_GT_56, GlobalVars.gAranEnv);
			_MAHF.ISAtC = GlobalVars.constants.Pansops[ePANSOPSData.arISAmax].Value;

			//=== Legs
			_ISegment = new LegApch(_IF, _FAF, GlobalVars.gAranEnv);			//fpIntermediateApproach	// IF - FAF
			_FASegment = new LegApch(_FAF, _MAPt, GlobalVars.gAranEnv);			//fpFinalApproach			// FAF - MAPt
			_MASegment = new LegApch(_MAPt, _MAHF, GlobalVars.gAranEnv);		//fpFinalMissedApproach		// MATF - MAHF	//

			_HorzLPlaneGr = -1;
			_HorzPPlaneGr = -1;
			_HorzRPlaneGr = -1;

			_MASLPlaneGr = -1;
			_MASPPlaneGr = -1;
			_MASRPlaneGr = -1;

			_FASLPlaneGr = -1;
			_FASPPlaneGr = -1;
			_FASRPlaneGr = -1;

			_FASLPlaneGrI = -1;
			_FASPPlaneGrI = -1;
			_FASRPlaneGrI = -1;

			//Page 1 =======================================================================
			label01008.Text = GlobalVars.unitConverter.HeightUnit;
			label01010.Text = GlobalVars.unitConverter.DistanceUnit;
			label01012.Text = GlobalVars.unitConverter.HeightUnit;
			label01016.Text = GlobalVars.unitConverter.DistanceUnit;

			groupBox01001.Text = Resources.str01002;

			label01001.Text = Resources.str01001;
			label01002.Text = Resources.str01003;
			label01003.Text = Resources.str01004;
			label01005.Text = Resources.str01005;
			label01007.Text = Resources.str01006;
			label01009.Text = Resources.str01008;	//Resources.str01007;
			label01011.Text = Resources.str01009;
			label01013.Text = Resources.str01010;
			label01015.Text = Resources.str01011;

			_MAHFTHRDistRange.Min = 5 * 1852.0;
			_MAHFTHRDistRange.Max = 30 * 1852.0;
			_MAHFTHRDist = _MAHFTHRDistRange.Min;

			textBox01007.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_MAHFTHRDist).ToString();
			textBox01007.Tag = textBox01007.Text;

			DBModule.FillRWYList(out _RWYDATA, GlobalVars.CurrADHP);
			int n = _RWYDATA.Length;

			if (n == 0)
			{
				//_IsClosing = true;
				Close();
				//gAranEnv.ShowError(My.Resources.str300);
				//MsgBox(My.Resources.str00300, MsgBoxStyle.Critical, "PANDA");
				throw new Exception("Wrong parameters of RWY.");
				//return;
			}

			_reportForm = new ReportForm();
			_reportForm.SetBtn(ReportBtn, 3700);

			comboBox01001.SelectedIndex = 0;


			for (int i = 0; i < n; i++)
				comboBox01002.Items.Add(_RWYDATA[i]);


			comboBox01002.SelectedIndex = 0;

			//Page 2 =======================================================================
			label02002.Text = GlobalVars.unitConverter.HeightUnit;
			label02004.Text = GlobalVars.unitConverter.DistanceUnit;

			label02012.Text = GlobalVars.unitConverter.HeightUnit;
			label02014.Text = GlobalVars.unitConverter.HeightUnit;

			label02022.Text = GlobalVars.unitConverter.DistanceUnit;
			label02024.Text = GlobalVars.unitConverter.DistanceUnit;
			label02026.Text = GlobalVars.unitConverter.DistanceUnit;
			label02028.Text = GlobalVars.unitConverter.DistanceUnit;

			label02001.Text = Resources.str02001;
			label02003.Text = Resources.str02002;
			label02005.Text = Resources.str02003;
			label02007.Text = Resources.str02004;
			label02011.Text = Resources.str02005;
			label02013.Text = Resources.str02006;
			label02015.Text = Resources.str02007;
			label02017.Text = Resources.str02008;
			label02019.Text = Resources.str02009;

			label02021.Text = Resources.str02011;
			label02023.Text = Resources.str02012;
			label02025.Text = Resources.str02013;
			label02027.Text = Resources.str02014;

			label02029.Text = Resources.str02030;
			button02001.Text = Resources.str02031;

			//Page 3 =======================================================================
			label03002.Text = GlobalVars.unitConverter.DistanceUnit;
			label03004.Text = GlobalVars.unitConverter.HeightUnit;
			label03010.Text = GlobalVars.unitConverter.DistanceUnit;
			label03012.Text = GlobalVars.unitConverter.SpeedUnit;

			label03001.Text = Resources.str03001;
			label03003.Text = Resources.str03002;
			label03005.Text = Resources.str03003;
			label03007.Text = Resources.str03004;
			label03009.Text = Resources.str03005;
			label03011.Text = Resources.str03006;

			// =============================================================================

			int hang = pageContMain.Size.Height + 3 - pageContMain.TabPages[0].Size.Height - ((this.Width - this.ClientSize.Width) >> 1);
			int dd = hang - pageContMain.Top;

			//hang = pageContMain.ItemSize.Height + ((this.Width - this.ClientSize.Width) >> 1);
			//PageFrame.Top = -hang;

			pageContMain.Top = -hang;
			ShowPanelBtn.Top = -hang;

			CommandFrame.Top -= dd;
			this.Height -= dd;

			this.bFormInitialised = true;

			if (ShowPanelBtn.Checked)
				ShowPanelBtn.Checked = false;
			else
				ShowPanelBtn_CheckedChanged(ShowPanelBtn, null);
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			//screenCapture.Rollback();
			DBModule.CloseDB();

			if (_reportForm != null)
				_reportForm.Close();

			DeleteAllGraphics();
			GlobalVars.gAranGraphics.Refresh();
			GlobalVars.CurrCmd = -1;
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

		#endregion

		#region utilities

		private void TextBoxes_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				e.IsInputKey = true;
		}

		private void DeleteAllGraphics()
		{
			// IF - FAF
			if (_ISegment != null)
				_ISegment.DeleteGraphics();

			// FAF - MAPt
			if (_FASegment != null)
				_FASegment.DeleteGraphics();

			// MAPt - MATF
			if (_MASegment != null)
				_MASegment.DeleteGraphics();

			// MATF - MAHF
			//if (FMATASegment != null)				FMATASegment.DeleteGraphics();

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_HorzLPlaneGr);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_HorzPPlaneGr);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_HorzRPlaneGr);

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_MASLPlaneGr);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_MASPPlaneGr);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_MASRPlaneGr);

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_FASLPlaneGr);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_FASPPlaneGr);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_FASRPlaneGr);

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_FASLPlaneGrI);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_FASPPlaneGrI);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_FASRPlaneGrI);
		}

		private void MergeLists(ref ObstacleContainer dest, ObstacleContainer src)
		{
			int md = dest.Obstacles.Length;
			int ms = src.Obstacles.Length;

			Array.Resize<Obstacle>(ref dest.Obstacles, md + ms);
			Array.Copy(src.Obstacles, 0, dest.Obstacles, md, ms);

			int nd = dest.Parts.Length;
			int ns = src.Parts.Length;

			for (int i = 0; i < ns; i++)
				src.Parts[i].Owner += md;

			Array.Resize<ObstacleData>(ref dest.Parts, nd + ns);
			Array.Copy(src.Parts, 0, dest.Parts, nd, ns);
		}

		private double ConvertTracToPoints(out ReportPoint[] GuidPoints)
		{
			double result = 0.0;
			GuidPoints = new ReportPoint[4];

			// IF
			LegBase leg = _ISegment;
			GuidPoints[0].TrueCourse = ARANFunctions.DirToAzimuth(leg.EndFIX.PrjPt, leg.EndFIX.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			GuidPoints[0].DistToNext = leg.Length;
			GuidPoints[0].Height = -1.0;		// leg.UpperLimit;
			GuidPoints[0].Radius = -1.0;

			GuidPoints[0].Description = leg.StartFIX.Name;
			GuidPoints[0].Lat = leg.StartFIX.GeoPt.Y;
			GuidPoints[0].Lon = leg.StartFIX.GeoPt.X;
			//result += leg.Length;
			result += leg.NominalTrack.Length;

			// FAF
			leg = _FASegment;
			GuidPoints[1].TrueCourse = ARANFunctions.DirToAzimuth(leg.EndFIX.PrjPt, leg.EndFIX.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			GuidPoints[1].DistToNext = leg.Length;
			GuidPoints[1].Height = -1.0;        // leg.UpperLimit;
			GuidPoints[1].Radius = -1.0;

			GuidPoints[1].Description = leg.StartFIX.Name;
			GuidPoints[1].Lat = leg.StartFIX.GeoPt.Y;
			GuidPoints[1].Lon = leg.StartFIX.GeoPt.X;
			//result += leg.Length;
			result += leg.NominalTrack.Length;

			// MAPt
			leg = _MASegment;
			GuidPoints[2].TrueCourse = ARANFunctions.DirToAzimuth(leg.EndFIX.PrjPt, leg.EndFIX.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			GuidPoints[2].DistToNext = leg.Length;
			GuidPoints[2].Height = -1.0;        // leg.UpperLimit;
			GuidPoints[2].Radius = -1.0;

			GuidPoints[2].Description = leg.StartFIX.Name;
			GuidPoints[2].Lat = leg.StartFIX.GeoPt.Y;
			GuidPoints[2].Lon = leg.StartFIX.GeoPt.X;
			//result += leg.Length;
			result += leg.NominalTrack.Length;

			// MATF
			GuidPoints[3].TrueCourse = ARANFunctions.DirToAzimuth(leg.EndFIX.PrjPt, leg.EndFIX.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			GuidPoints[3].DistToNext = -1.0;
			GuidPoints[3].Height = -1.0;		// leg.UpperLimit;
			GuidPoints[3].Radius = -1.0;

			GuidPoints[3].Description = leg.EndFIX.Name;
			GuidPoints[3].Lat = leg.EndFIX.GeoPt.Y;
			GuidPoints[3].Lon = leg.EndFIX.GeoPt.X;

			return result;
		}

		private void SaveProtocol(string RepFileName, string RepFileTitle, ReportHeader pReport)
		{
			ReportFile obstacleReport = new ReportFile();

			obstacleReport.OpenFile(RepFileName + "_Protocol", Resources.str00109);

			obstacleReport.WriteString(Resources.str00033 + " - " + Resources.str00109, true);
			obstacleReport.WriteString("");
			obstacleReport.WriteString(RepFileTitle, true);

			obstacleReport.WriteHeader(pReport);
			obstacleReport.WriteString("");
			obstacleReport.WriteString("");

			_reportForm.SortForSave();

			obstacleReport.WriteTab(_reportForm.listView1, _reportForm.GetTabPageText(0));
			obstacleReport.WriteTab(_reportForm.listView2, _reportForm.GetTabPageText(1));

			obstacleReport.CloseFile();
		}

		private void SaveLog(string RepFileName, string RepFileTitle, ReportHeader pReport)
		{
			ReportFile logReport = new ReportFile();

			logReport.OpenFile(RepFileName + "_Log", Resources.str00124);

			logReport.WriteString(Resources.str00033 + " - " + Resources.str00124, true);
			logReport.WriteString("");
			logReport.WriteString(RepFileTitle, true);

			logReport.WriteHeader(pReport);

			//====================================================================================
			logReport.WriteString("");
			logReport.WriteString("");

			logReport.ExH2(pageContMain.TabPages[0].Text);
			logReport.HTMLString("[ " + pageContMain.TabPages[0].Text + " ]", true, false);
			logReport.WriteString("");

			logReport.Param(label01001.Text, comboBox01001.Text, "");

			logReport.Param(label01011.Text, textBox01005.Text, label01012.Text);
			logReport.Param(label01013.Text, textBox01006.Text, label01014.Text);
			logReport.Param(label01015.Text, textBox01007.Text, label01016.Text);


			logReport.Param(groupBox01001.Text, ".", "");   //logReport.WriteString(groupBox01001.Text);

			logReport.Param(label01002.Text, comboBox01002.Text, "");
			logReport.Param(label01003.Text, textBox01001.Text, label01004.Text);
			logReport.Param(label01005.Text, textBox01002.Text, label01006.Text);
			logReport.Param(label01007.Text, textBox01003.Text, label01008.Text);
			logReport.Param(label01009.Text, textBox01004.Text, label01010.Text);

			//====================================================================================
			logReport.WriteString("");
			logReport.WriteString("");

			logReport.ExH2(pageContMain.TabPages[1].Text);
			logReport.HTMLString("[ " + pageContMain.TabPages[1].Text + " ]", true, false);
			logReport.WriteString("");

			logReport.Param(label02001.Text, textBox02001.Text, label02002.Text);
			logReport.Param(label02003.Text, textBox02002.Text, label02004.Text);
			logReport.Param(label02005.Text, textBox02003.Text, label02006.Text);
			logReport.Param(label02007.Text, textBox02004.Text, label02008.Text);
			logReport.Param(label02009.Text, textBox02005.Text, label02010.Text);
			logReport.Param(label02030.Text, textBox02015.Text, label02031.Text);

			logReport.WriteString("");

			logReport.Param(label02011.Text, textBox02006.Text, label02012.Text);
			logReport.Param(label02013.Text, textBox02007.Text, label02014.Text);
			logReport.Param(label02015.Text, textBox02008.Text, label02016.Text);
			logReport.Param(label02017.Text, textBox02009.Text, label02018.Text);
			logReport.Param(label02019.Text, textBox02010.Text, label02020.Text);
			logReport.WriteString("");

			logReport.Param(label02021.Text, textBox02011.Text, label02022.Text);
			logReport.Param(label02023.Text, textBox02012.Text, label02024.Text);
			logReport.Param(label02025.Text, textBox02013.Text, label02026.Text);
			logReport.Param(label02027.Text, textBox02014.Text, label02028.Text);

			//if(label02029.Visible)
			//{
			//		logReport.WriteString(label02029.Text, true);
			//}

			//====================================================================================
			logReport.WriteString("");
			logReport.WriteString("");

			logReport.ExH2(pageContMain.TabPages[2].Text);
			logReport.HTMLString("[ " + pageContMain.TabPages[2].Text + " ]", true, false);
			logReport.WriteString("");

			logReport.Param(label03001.Text, textBox03001.Text, label03002.Text);
			logReport.Param(label03003.Text, textBox03002.Text, label03004.Text);
			logReport.Param(label03005.Text, textBox03003.Text, label03006.Text);

			logReport.Param(label03007.Text, textBox03004.Text, label03008.Text);
			logReport.Param(label03009.Text, textBox03005.Text, label03010.Text);
			logReport.Param(label03011.Text, textBox03006.Text, label03012.Text);

			logReport.CloseFile();
		}

		#endregion

		private void MainForm_KeyUp(object sender, KeyEventArgs e)
		{
			//if (e.KeyCode == Keys.F1)	ShowHelp(Handle, 3100 + 100 * pageContMain.SelectedIndex);
		}

		private void bHelp_Click(object sender, EventArgs e)
		{
			//ShowHelp(Handle, 3100 + 100 * pageContMain.SelectedIndex);
		}

		private void ShowPanelBtn_CheckedChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised)
				return;

			if (ShowPanelBtn.Checked)
			{
				this.Width = PageFrame.Left + PageFrame.Width + 6;      // FontResizeFactorProvider.Scale(3);
				ShowPanelBtn.Image = Resources.HIDE_INFO;
			}
			else
			{
				this.Width = PageFrame.Left + 16;
				ShowPanelBtn.Image = Resources.SHOW_INFO;
			}

			if (NextBtn.Enabled)
				NextBtn.Focus();
			else
				PrevBtn.Focus();
		}

		private void OkBtn_Click(object sender, EventArgs e)
		{
			string RepFileName, RepFileTitle;
			//screenCapture.Save(this);

			if (!Functions.ShowSaveDialog(out RepFileName, out RepFileTitle))
				return;

			string sProcName = RepFileTitle;

			ReportHeader pReport;
			pReport.Procedure = sProcName;

			pReport.Database = GlobalVars.gAranEnv.ConnectionInfo.Database;
			pReport.Aerodrome = GlobalVars.CurrADHP.Name;
			pReport.Category = comboBox01001.Text;

			////pReport.RWY = ComboBox001.Text;
			////pReport.Procedure = _Procedure.Name;
			////pReport.EffectiveDate = _Procedure.TimeSlice.ValidTime.BeginPosition;

			SaveLog(RepFileName, RepFileTitle, pReport);
			SaveProtocol(RepFileName, RepFileTitle, pReport);

			ReportPoint[] GuidPoints;
			double TotalLen = ConvertTracToPoints(out GuidPoints);


			//ReportFile geometryReport = 
				ReportFile.SaveGeometry(RepFileName, RepFileTitle, pReport, GuidPoints, TotalLen);

			if (SaveProcedure())
			{
				this.Close();
			}
		}

		private void CancelBtn_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void PrevBtn_Click(object sender, EventArgs e)
		{
			//screenCapture.Delete();
			NativeMethods.ShowPandaBox(this.Handle.ToInt32());

			switch (CurrPage)
			{
				case 1:
					DeleteAllGraphics();
					break;
				case 2:
					break;
			}

			this.CurrPage = CurrPage - 1;
			NativeMethods.HidePandaBox();
		}

		private void NextBtn_Click(object sender, EventArgs e)
		{
			//screenCapture.Save(this);
			NativeMethods.ShowPandaBox(this.Handle.ToInt32());
			//NativeMethods.ShowPandaBox(GlobalVars.gAranEnv.Win32Window.Handle.ToInt32());

			switch (CurrPage)
			{
				case 0:
					ToAPVSegment();
					break;
				case 1:
					ToIASegment();
					NextBtn.Enabled = false;
					break;
			}

			CurrPage = CurrPage + 1;
			NativeMethods.HidePandaBox();
		}

		private void ReportBtn_CheckedChanged(object sender, EventArgs e)
		{
			//if (!Report)				return;
			if (ReportBtn.Checked)
				_reportForm.Show(GlobalVars.Win32Window);
			else
				_reportForm.Hide();
		}

		#region PAGE I

		private void comboBox01001_SelectedIndexChanged(object sender, EventArgs e)
		{
			int k = comboBox01001.SelectedIndex;
			if (k < 0) return;

			_margin = 0.24298056155508 * GlobalVars.constants.AircraftCategory[aircraftCategoryData.VatMax].Value[k] + 28.3;                    // 0.2448
			textBox01005.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_margin, eRoundMode.NEAREST).ToString();
		}

		private void comboBox01002_SelectedIndexChanged(object sender, EventArgs e)
		{
			textBox01001.Text = "";
			textBox01002.Text = "";
			textBox01003.Text = "";
			//editRwyDirectionClearway.Text = "";
			textBox01004.Text = "";

			int k = comboBox01002.SelectedIndex;
			if (k < 0)
				return;

			_selectedRWY = GlobalVars.RWYList[k];
			_RWYTHRPrj = _selectedRWY.pPtPrj[eRWY.ptTHR];
			_ArDir = _RWYTHRPrj.M;

			_hMaxSS = GlobalVars.H0;
			if (_RWYTHRPrj.Z > GlobalVars.H2h)
				_hMaxSS = GlobalVars.H10000;
			else if (_RWYTHRPrj.Z > GlobalVars.H1h)
				_hMaxSS = GlobalVars.H5000;

			NextBtn.Enabled = true;
			textBox01001.Text = _selectedRWY.TrueBearing.ToString("00.0");
			textBox01002.Text = _selectedRWY.MagneticBearing.ToString("00.0");

			textBox01003.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_RWYTHRPrj.Z).ToString();
			//editRwyDirectionClearway.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_SelectedRWY.ClearWay).ToString();
			textBox01004.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_selectedRWY.Length).ToString();
			//fTmp= CConverters[puAltitude].ConvertFunction(RWYTHRPrj.displacement, cdToOuter, nil);

			double fMOCCorrH = 0.0;
			if (_RWYTHRPrj.Z > 900.0)
				fMOCCorrH = _RWYTHRPrj.Z * (0.34406047516199 * GlobalVars.constants.AircraftCategory[aircraftCategoryData.VatMax].Value[k] - 3.2) / 1500.0;

			_hLost = _margin + fMOCCorrH;

			double hTas = _RWYTHRPrj.Z > 900.0 ? _RWYTHRPrj.Z : 900.0;
			_TAS = ARANMath.IASToTAS(GlobalVars.constants.AircraftCategory[aircraftCategoryData.VfafMax][_AirCat],
				   hTas, GlobalVars.CurrADHP.ISAtC) + GlobalVars.constants.Pansops[ePANSOPSData.arNearTerrWindSp].Value;
		}

		//private int _checkedItem = 0;
		//private void ListView1_ItemChecked(object sender, ItemCheckedEventArgs e)
		//{

		//	if (e.Item.Checked)
		//		_checkedItem = e.Item.Index;

		//	var checkedItems = listView01001.CheckedItems;

		//	if (checkedItems.Count == 0 && listView01001.Items.Count > 0)
		//	{
		//		if (_checkedItem < 0)
		//			_checkedItem = 0;
		//		listView01001.Items[_checkedItem].Checked = true;
		//	}
		//}

		//private void rgApproachType_CheckedChanged(object sender, EventArgs e)
		//{
		//	if (!((RadioButton)sender).Checked)
		//		return;

		//	groupBox01001.Visible = radioButton01001.Checked;
		//	listView01001.Visible = radioButton01002.Checked;
		//}

		private void textBox01007_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox01007_Validating(textBox01007, null);
			else
				Functions.TextBoxFloat(ref eventChar, textBox01007.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox01007_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!double.TryParse(textBox01007.Text, out double fTmp))
				return;

			if (textBox01007.Tag != null && textBox01007.Tag.ToString() == textBox01007.Text)
				return;

			fTmp = _MAHFTHRDist = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			if (_MAHFTHRDist < _MAHFTHRDistRange.Min) _MAHFTHRDist = _MAHFTHRDistRange.Min;
			if (_MAHFTHRDist > _MAHFTHRDistRange.Max) _MAHFTHRDist = _MAHFTHRDistRange.Max;

			if (fTmp != _MAHFTHRDist)
				textBox01007.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_MAHFTHRDist).ToString();
			textBox01007.Tag = textBox01007.Text;

			//ApplyMAHFTHRDistValue();
		}

		void ApplyMAHFTHRDistValue()
		{
			//textBox01007.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_MAHFTHRDist).ToString();
			//textBox01007.Tag = textBox01007.Text;

			_MAHF.ConstructAltitude = _MAPt.NomLineAltitude = _RWYTHRPrj.Z + GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value + _MAHFTHRDist * _MA_PDG;
			_MAHF.PrjPt = ARANFunctions.LocalToPrj(_RWYTHRPrj, _ArDir, _MAHFTHRDist, 0);            //ARANFunctions.CircleVectorIntersect(GlobalVars.CurrADHP.pPtPrj, _MATHRDist, _MAPt.PrjPt, _ArDir, out double fTmp);

			double fixDist = ARANFunctions.ReturnDistanceInMeters(_MAHF.PrjPt, GlobalVars.CurrADHP.pPtPrj);

			if (fixDist < PANSOPSConstantList.PBNInternalTriggerDistance)
			{
				_MAHF.FlightPhase = eFlightPhase.MApLT28;
				_MAHF.Role = eFIXRole.MAHF_LE_56;
			}
			else
			{
				_MAHF.FlightPhase = eFlightPhase.MApGE28;
				_MAHF.Role = eFIXRole.MAHF_LE_56;
			}

			Functions.CreateMASPlanes(_RWYTHRPrj, _xOrigin, _MAHFTHRDist, _MA_PDG, _FASegment, _MASegment, out _MASPlanes);

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_MASLPlaneGr);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_MASPPlaneGr);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_MASRPlaneGr);

			_MASLPlaneGr = GlobalVars.gAranGraphics.DrawPolygon(_MASPlanes[0].Poly, eFillStyle.sfsDiagonalCross, ARANFunctions.RGB(0, 127, 255));
			_MASPPlaneGr = GlobalVars.gAranGraphics.DrawPolygon(_MASPlanes[1].Poly, eFillStyle.sfsDiagonalCross, ARANFunctions.RGB(0, 255, 127));
			_MASRPlaneGr = GlobalVars.gAranGraphics.DrawPolygon(_MASPlanes[2].Poly, eFillStyle.sfsDiagonalCross, ARANFunctions.RGB(0, 127, 255));

			Functions.AnaliseMASObstacles(GlobalVars.ObstacleList, _RWYTHRPrj, _MASPlanes, _xOrigin, _MA_PDG, out _MASObstList);

			CalcObstOCH();
		}

		#endregion

		#region ToAPVSegment

		private void ToAPVSegment()
		{
			//_FAFTHRDistRange.Min = GlobalVars.constants.Pansops[ePANSOPSData.arMinRangeFAS].Value;
			//_FAFTHRDistRange.Max = GlobalVars.constants.Pansops[ePANSOPSData.arMaxRangeFAS].Value;

			_AirCat = (aircraftCategory)comboBox01001.SelectedIndex;

			_MAPtTHRDist = 0.0;
			textBox02011.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_MAPtTHRDist).ToString();

			//===========================================================================================------------------------
			_MA_PDGRange.Min = 0.02;
			_MA_PDGRange.Max = 0.07;
			_MA_PDG = 0.025;

			textBox02003.Text = GlobalVars.unitConverter.GradientToDisplayUnits(_MA_PDG).ToString();
			textBox02003.Tag = textBox02003.Text;

			//==============================================================================
			_VPARange.Min = ARANMath.DegToRad(2.7);
			_VPARange.Max = ARANMath.DegToRad(3.5);
			_VPA = ARANMath.DegToRad(3.0);

			//===========================================================================================------------------------
			_FAFTHRDist = GlobalVars.constants.Pansops[ePANSOPSData.arFAFOptimalDist].Value;
			textBox02002.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_FAFTHRDist).ToString();

			_FAF.PrjPt = ARANFunctions.LocalToPrj(_RWYTHRPrj, _ArDir, -_FAFTHRDist, 0);

			_FAPheight = _FAFTHRDist * Math.Tan(_VPA) + GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value;
			//===========================================================================================------------------------

			//Creating Lateral Nav Areas and Vertical planes ===============================

			_IF.EntryDirection = _ArDir;
			_IF.OutDirection = _ArDir;
			//_IF.ConstructAltitude = _IF.NomLineAltitude = _FAPheight + _RWYTHRPrj.Z +
			//		(GlobalVars.constants.Pansops[ePANSOPSData.arImRange_Max].Value - GlobalVars.constants.AircraftCategory[aircraftCategoryData.arImHorSegLen][_AirCat]) *
			//		GlobalVars.constants.Pansops[ePANSOPSData.arImDescent_Max].Value;

			//_IF.PrjPt = ARANFunctions.LocalToPrj(_RWYTHRPrj, _ArDir, -(_FAFTHRDist + GlobalVars.constants.Pansops[ePANSOPSData.arImRange_Max].Value), 0);

			//=====
			_FAF.EntryDirection = _ArDir;
			_FAF.OutDirection = _ArDir;
			//_FAF.ConstructAltitude = _FAF.NomLineAltitude = _FAPheight + _RWYTHRPrj.Z;
			//_FAF.PrjPt = ARANFunctions.LocalToPrj(_RWYTHRPrj, _ArDir, -prmFAFTHRDist, 0);

			//=====
			_MAPt.ConstructAltitude = _MAPt.NomLineAltitude = _RWYTHRPrj.Z + GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value;
			_MAPt.EntryDirection = _ArDir;
			_MAPt.OutDirection = _ArDir;

			_MAPt.PrjPt = ARANFunctions.LocalToPrj(_RWYTHRPrj, _ArDir, _MAPtTHRDist, 0);

			//=================================
			_MAHF.EntryDirection = _ArDir;
			_MAHF.OutDirection = _ArDir;

			_MAHF.ConstructAltitude = _MAPt.NomLineAltitude = _RWYTHRPrj.Z + GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value + _MAHFTHRDist * _MA_PDG;
			_MAHF.PrjPt = ARANFunctions.LocalToPrj(_RWYTHRPrj, _ArDir, _MAHFTHRDist, 0);			//ARANFunctions.CircleVectorIntersect(GlobalVars.CurrADHP.pPtPrj, _MATHRDist, _MAPt.PrjPt, _ArDir, out double fTmp);

			double fixDist = ARANFunctions.ReturnDistanceInMeters(_MAHF.PrjPt, GlobalVars.CurrADHP.pPtPrj);

			if (fixDist < PANSOPSConstantList.PBNInternalTriggerDistance)
			{
				_MAHF.FlightPhase = eFlightPhase.MApLT28;
				_MAHF.Role = eFIXRole.MAHF_LE_56;
			}
			else
			{
				_MAHF.FlightPhase = eFlightPhase.MApGE28;
				_MAHF.Role = eFIXRole.MAHF_LE_56;
			}

			//===

			double MOCAprDist = _FAFTHRDist - 4 * _MAPt.ATT;

			//_MAPtTHRDistRange.Min = 0;
			//_MAPtTHRDistRange.Max = MOCAprDist;

			double Dist0 = _TAS * GlobalVars.constants.Pansops[ePANSOPSData.arSOCdelayTime].Value;
			double Dist1 = _TAS * GlobalVars.constants.Pansops[ePANSOPSData.dpPilotTolerance].Value;

			_THRSOCDist = MOCAprDist - (Dist0 + Dist1 + _MAPt.ATT);
			textBox02012.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_THRSOCDist).ToString();

			//prmTHRSOCDist = _MAPt.ATT - Dist0 - Dist1;

			_MAPt.SOCDistance = _MAPtTHRDist - _THRSOCDist;

			//=================================

			ApplyVPAValue();
		}

		#endregion

		#region PAGE II - APV segment

		private void button02001_Click(object sender, EventArgs e)
		{
			//int I, J, K, N, VertexNum, FaceNum;
			//_3DForm _3Dform;
			//Object3D object3D;

			//if (sender == Panel1)
			//	Panel1.BorderStyle = BorderStyle.Fixed3D;


			//N = FOFZPlanes.Length - 1;
			//VertexNum = 4 * N;
			//FaceNum = 2 * N;

			//object3D.x = new double[VertexNum];
			//object3D.y = new double[VertexNum];
			//object3D.z = new double[VertexNum];

			//object3D.xS = new int[VertexNum];
			//object3D.yS = new int[VertexNum];
			//object3D.zS = new int[VertexNum];

			//object3D.illuminance = new int[VertexNum];
			//object3D.Face = new Face3D[FaceNum];
			//object3D.Face = new Face3D[FaceNum];

			//object3D.zScale = 1;

			//object3D.xMax = FOFZPlanes[0].Poly.Ring[0].Point[0].X;
			//object3D.xMin = FOFZPlanes[0].Poly.Ring[0].Point[0].X;

			//object3D.yMax = FOFZPlanes[0].Poly.Ring[0].Point[0].Y;
			//object3D.yMin = FOFZPlanes[0].Poly.Ring[0].Point[0].Y;

			//object3D.zMax = FOFZPlanes[0].Poly.Ring[0].Point[0].Z;
			//object3D.zMin = FOFZPlanes[0].Poly.Ring[0].Point[0].Z;
			//K = -1;

			//for (I = 0; I < N; I++)
			//{
			//	for (J = 0; J < FOFZPlanes[I].Poly.Ring[0].Count; J++)
			//	{

			//		K++;
			//		object3D.x[K] = FOFZPlanes[I].Poly.Ring[0].Point[J].X;
			//		object3D.y[K] = FOFZPlanes[I].Poly.Ring[0].Point[J].Y;
			//		object3D.z[K] = FOFZPlanes[I].Poly.Ring[0].Point[J].Z;

			//		if (object3D.xMax < object3D.x[K]) object3D.xMax = object3D.x[K];
			//		if (object3D.xMin > object3D.x[K]) object3D.xMin = object3D.x[K];

			//		if (object3D.yMax < object3D.y[K]) object3D.yMax = object3D.y[K];
			//		if (object3D.yMin > object3D.y[K]) object3D.yMin = object3D.y[K];

			//		if (object3D.zMax < object3D.z[K]) object3D.zMax = object3D.z[K];
			//		if (object3D.zMin > object3D.z[K]) object3D.zMin = object3D.z[K];
			//	}

			//	object3D.Face[2 * I].A = K - 3;
			//	object3D.Face[2 * I].B = K - 2;
			//	object3D.Face[2 * I].C = K - 1;
			//	object3D.Face[2 * I].Texture = null;
			//	object3D.Face[2 * I].Color.R = 255 - 32 * (I + 1);
			//	object3D.Face[2 * I].Color.G = 0;
			//	object3D.Face[2 * I].Color.B = 0;
			//	object3D.Face[2 * I].ShadeType = 0;

			//	object3D.Face[2 * I + 1].A = K - 3;
			//	object3D.Face[2 * I + 1].B = K - 1;
			//	object3D.Face[2 * I + 1].C = K;
			//	object3D.Face[2 * I + 1].Texture = null;
			//	object3D.Face[2 * I + 1].Color.R = 255 - 32 * (I + 1);
			//	object3D.Face[2 * I + 1].Color.G = 0;
			//	object3D.Face[2 * I + 1].Color.B = 0;
			//	object3D.Face[2 * I + 1].ShadeType = 0;
			//}

			//object3D.VertexNum = VertexNum;
			//object3D.FaceNum = FaceNum;


			////===================================================

			//Triangulate.AddPlanesTo3D(FFASPlanes, ARANFunctions.RGB(128, 255, 128), Object3D);
			//Triangulate.AddPlanesTo3D(FHorzPlanes, ARANFunctions.RGB(128, 128, 255), Object3D);
			//Triangulate.AddPlanesTo3D(FWPlanes, ARANFunctions.RGB(128, 128, 255), Object3D);

			//_3Dform = new _3DForm();
			//_3Dform.AssignMesh(object3D);

			//if (IsWindow(GHandle))
			//	SetWindowLong(_3DForm.Handle, GWL_HWNDPARENT, GHandle);

			//_3DForm.Show;

			//if (sender == Panel1)
			//	Panel1.BorderStyle = BorderStyle.None;
		}

		void ApplyVPAValue()
		{
			double fMOCCorrH = 0.0, fMOCCorrGP = 0.0, VPAinDeg = ARANMath.RadToDeg(_VPA);
			double TanVPA = Math.Tan(_VPA);

			textBox02004.Text = VPAinDeg.ToString("0.00");
			textBox02004.Tag = textBox02004.Text;

			//=================================================================================================
			double rMargin = 0.34406047516199 * GlobalVars.constants.AircraftCategory[aircraftCategoryData.VatMax].Value[(int)_AirCat] - 3.2;

			if (_RWYTHRPrj.Z > 900.0)
				fMOCCorrH = _RWYTHRPrj.Z * rMargin / 1500.0;

			if (VPAinDeg > 3.2)
				fMOCCorrGP = (VPAinDeg - 3.2) * rMargin * 0.5;

			_hLost = _margin + fMOCCorrGP + fMOCCorrH;

			//=================================================================================================

			switch (_AirCat)
			{
				case aircraftCategory.acA:
				case aircraftCategory.acB:
					_xOrigin = -900;
					break;
				case aircraftCategory.acC:
					_xOrigin = -1100;
					break;
				default:
					_xOrigin = -1400;
					break;
			}

			if (_RWYTHRPrj.Z > 900.0 || VPAinDeg > 3.2)
			{
				double gamma = 2.56 * 0.3048;
				double Vw = GlobalVars.constants.Pansops[ePANSOPSData.arNearTerrWindSp].Value;
				double RDH = GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value;

				_xOrigin = Math.Min(_xOrigin, (_hLost - RDH) / TanVPA - (_MASegment.StartFIX.ATT + 2 * (_TAS - Vw) * Math.Sin(_VPA) / gamma * _TAS));
			}

			textBox02014.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_xOrigin).ToString();

			//===================================================================

			double AbvTreshold = GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value;

			_FAPheightRange.Max = AbvTreshold + GlobalVars.constants.Pansops[ePANSOPSData.arMaxRangeFAS].Value * TanVPA;
			_FAPheightRange.Min = AbvTreshold + GlobalVars.constants.Pansops[ePANSOPSData.arMinRangeFAS].Value * TanVPA;


			double fTmp = _FAFTHRDist;
			if (_FAFTHRDist > GlobalVars.constants.Pansops[ePANSOPSData.arMaxRangeFAS].Value)
				_FAFTHRDist = GlobalVars.constants.Pansops[ePANSOPSData.arMinRangeFAS].Value;

			if (_FAFTHRDist < GlobalVars.constants.Pansops[ePANSOPSData.arMaxRangeFAS].Value)
				_FAFTHRDist = GlobalVars.constants.Pansops[ePANSOPSData.arMinRangeFAS].Value;

			if (_FAFTHRDist != fTmp)
				textBox02002.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_FAFTHRDist).ToString();

			_FAPheight = AbvTreshold + _FAFTHRDist * TanVPA;

			//if (_FAPheight < _FAPheightRange.Min) _FAPheight = _FAPheightRange.Min;
			//if (_FAPheight > _FAPheightRange.Max) _FAPheight = _FAPheightRange.Max;

			//textBox02001.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_FAPheight + _RWYTHRPrj.Z).ToString();
			ApplyFAPheightValue();
		}

		void ApplyFAPheightValue()
		{
			const double MinVPAValue = 2.5 * Math.PI / 180.0;

			double fapAltitude = _FAPheight + _RWYTHRPrj.Z;


			textBox02001.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fapAltitude).ToString();
			textBox02001.Tag = textBox02001.Text;

			_FAF.ConstructAltitude = _FAF.NomLineAltitude = fapAltitude;
			_IF.ConstructAltitude = _IF.NomLineAltitude = fapAltitude +
				(GlobalVars.constants.Pansops[ePANSOPSData.arImRange_Max].Value - GlobalVars.constants.AircraftCategory[aircraftCategoryData.arImHorSegLen][_AirCat]) *
						GlobalVars.constants.Pansops[ePANSOPSData.arImDescent_Max].Value;

			double TanVPA = Math.Tan(_VPA);
			double AbvTreshold = GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value;

			_FAFTHRDist = (_FAPheight - AbvTreshold) / TanVPA;
			textBox02002.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_FAFTHRDist).ToString();

			_FAF.PrjPt = ARANFunctions.LocalToPrj(_RWYTHRPrj, _ArDir, -_FAFTHRDist, 0);
			_IF.PrjPt = ARANFunctions.LocalToPrj(_RWYTHRPrj, _ArDir, -(_FAFTHRDist + GlobalVars.constants.Pansops[ePANSOPSData.arImRange_Max].Value), 0);

			_ISegment.CreateGeometry(null, GlobalVars.CurrADHP);
			//_ISegment.RefreshGraphics();

			_FASegment.CreateGeometry(_ISegment, GlobalVars.CurrADHP, 0.0, true);
			//_FASegment.RefreshGraphics();

			_MASegment.CreateGeometry(_FASegment, GlobalVars.CurrADHP);
			_MASegment.RefreshGraphics();

			//=================================================================================================

			double tempCorr = 0;
			if (GlobalVars.CurrADHP.LowestTemperature < GlobalVars.constants.Pansops[ePANSOPSData.arISAmax].Value)
				tempCorr = Functions.TempCorr(GlobalVars.CurrADHP.LowestTemperature, fapAltitude, _RWYTHRPrj.Z);
			
			textBox02006.Text = GlobalVars.unitConverter.HeightToDisplayUnits(tempCorr).ToString();

			double minVPA = Math.Atan(TanVPA * (1.0 - tempCorr / _FAPheight));
			textBox02005.Text = ARANMath.RadToDeg(minVPA).ToString("0.0");

			if (minVPA < MinVPAValue)
			{
				MessageBox.Show(GlobalVars.Win32Window, "Please, increaset VPA value.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			//===================================================================================================	?????++++

			_FAS = TanVPA * (1.0 - tempCorr / (_FAPheight - GlobalVars.H0));                 // <=5000 ft
			_FAS_ = TanVPA * (1.0 - tempCorr / (_FAPheight - GlobalVars.H5000));             // <=10000 ft
			_FAS__ = TanVPA * (1.0 - tempCorr / (_FAPheight - GlobalVars.H10000));           // >10000 ft

			textBox02008.Text = GlobalVars.unitConverter.GradientToDisplayUnits(_FAS).ToString();
			textBox02009.Text = GlobalVars.unitConverter.GradientToDisplayUnits(_FAS_).ToString();
			textBox02010.Text = GlobalVars.unitConverter.GradientToDisplayUnits(_FAS__).ToString();

			const double L0m = -1.98 / 1000 / 0.3048;
			const double T0 = 288.15;

			double Tan_Max_VPA = Math.Tan(ARANMath.DegToRad(3.5));                      // = 0.061162620150484306

			double dh_max = fapAltitude * (1.0 - Tan_Max_VPA / TanVPA);						// = -222.28074619255
			double dT_max = -dh_max * L0m / Math.Log(1 + L0m * fapAltitude / (T0 + L0m * _RWYTHRPrj.Z));      // = 46.616659748216
			double T_max = dT_max + 15 + L0m * _RWYTHRPrj.Z;                                        // = 56.867723348216

			textBox02015.Text = T_max.ToString("0.00");
			//==============================================================================
			Functions.CreateHorzPlanes(_RWYTHRPrj, _xOrigin, _hMaxSS, _VPA, _FASegment, _MASegment, out _HorzPlanes);

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_HorzLPlaneGr);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_HorzPPlaneGr);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_HorzRPlaneGr);

			_HorzLPlaneGr = GlobalVars.gAranGraphics.DrawPolygon(_HorzPlanes[0].Poly, eFillStyle.sfsDiagonalCross, ARANFunctions.RGB(0, 0, 127));
			_HorzPPlaneGr = GlobalVars.gAranGraphics.DrawPolygon(_HorzPlanes[1].Poly, eFillStyle.sfsDiagonalCross, ARANFunctions.RGB(0, 127, 0));
			_HorzRPlaneGr = GlobalVars.gAranGraphics.DrawPolygon(_HorzPlanes[2].Poly, eFillStyle.sfsDiagonalCross, ARANFunctions.RGB(0, 0, 127));

			Functions.AnaliseHorzObstacles(GlobalVars.ObstacleList, _RWYTHRPrj, _HorzPlanes, _FASegment, _hMaxSS, out _HorzObstList);

			//=================================================================================================== prmFAPaltitude 
			double hFASS = GlobalVars.H0;
			if (_FAPheight - tempCorr + _RWYTHRPrj.Z > GlobalVars.H2h)
				hFASS = GlobalVars.H10000;
			else if (_FAPheight - tempCorr + _RWYTHRPrj.Z > GlobalVars.H1h)
				hFASS = GlobalVars.H5000;

			Functions.CreateFASPlanes(_RWYTHRPrj, _FAPheight, hFASS, _hMaxSS, _VPA, _FAS, _ISegment, _FASegment, out _FASPlanes);

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_FASLPlaneGr);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_FASPPlaneGr);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_FASRPlaneGr);

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_FASLPlaneGrI);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_FASPPlaneGrI);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_FASRPlaneGrI);

			_FASLPlaneGrI = -1;
			_FASPPlaneGrI = -1;
			_FASRPlaneGrI = -1;

			_FASLPlaneGr = GlobalVars.gAranGraphics.DrawPolygon(_FASPlanes[0].Poly, eFillStyle.sfsDiagonalCross, ARANFunctions.RGB(0, 0, 255));
			_FASPPlaneGr = GlobalVars.gAranGraphics.DrawPolygon(_FASPlanes[1].Poly, eFillStyle.sfsDiagonalCross, ARANFunctions.RGB(0, 255, 255));
			_FASRPlaneGr = GlobalVars.gAranGraphics.DrawPolygon(_FASPlanes[2].Poly, eFillStyle.sfsDiagonalCross, ARANFunctions.RGB(0, 0, 255));

			if(_FASPlanes.Length == 7)
			{
				_FASLPlaneGrI = GlobalVars.gAranGraphics.DrawPolygon(_FASPlanes[3].Poly, eFillStyle.sfsDiagonalCross, ARANFunctions.RGB(0, 0, 255));
				if (_FASPlanes[4].Poly != null)
					_FASPPlaneGrI = GlobalVars.gAranGraphics.DrawPolygon(_FASPlanes[4].Poly, eFillStyle.sfsDiagonalCross, ARANFunctions.RGB(0, 255, 255));
				_FASRPlaneGrI = GlobalVars.gAranGraphics.DrawPolygon(_FASPlanes[5].Poly, eFillStyle.sfsDiagonalCross, ARANFunctions.RGB(0, 0, 255));
			}

			Functions.AnaliseFASObstacles(GlobalVars.ObstacleList, _RWYTHRPrj, _FASPlanes, _FASegment, _VPA, _FAS, _FAS_, _FAS__, out _FASObstList);

			//==============================================================================	_MA_PDG, _MAHFTHRDist
			Functions.CreateMASPlanes(_RWYTHRPrj, _xOrigin, _MAHFTHRDist, _MA_PDG, _FASegment, _MASegment, out _MASPlanes);

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_MASLPlaneGr);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_MASPPlaneGr);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_MASRPlaneGr);

			_MASLPlaneGr = GlobalVars.gAranGraphics.DrawPolygon(_MASPlanes[0].Poly, eFillStyle.sfsDiagonalCross, ARANFunctions.RGB(0, 127, 255));
			_MASPPlaneGr = GlobalVars.gAranGraphics.DrawPolygon(_MASPlanes[1].Poly, eFillStyle.sfsDiagonalCross, ARANFunctions.RGB(0, 255, 127));
			_MASRPlaneGr = GlobalVars.gAranGraphics.DrawPolygon(_MASPlanes[2].Poly, eFillStyle.sfsDiagonalCross, ARANFunctions.RGB(0, 127, 255));

			Functions.AnaliseMASObstacles(GlobalVars.ObstacleList, _RWYTHRPrj, _MASPlanes, _xOrigin, _MA_PDG, out _MASObstList);

			//============================================================================== prmFAPaltitude , prmMA_PDG
			CalcObstOCH();
		}

		void ApplyMA_PDGValue()
		{
			Functions.CreateMASPlanes(_RWYTHRPrj, _xOrigin, _MAHFTHRDist, _MA_PDG, _FASegment, _MASegment, out _MASPlanes);

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_MASLPlaneGr);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_MASPPlaneGr);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_MASRPlaneGr);

			_MASLPlaneGr = GlobalVars.gAranGraphics.DrawPolygon(_MASPlanes[0].Poly, eFillStyle.sfsDiagonalCross, ARANFunctions.RGB(0, 127, 255));
			_MASPPlaneGr = GlobalVars.gAranGraphics.DrawPolygon(_MASPlanes[1].Poly, eFillStyle.sfsDiagonalCross, ARANFunctions.RGB(0, 255, 127));
			_MASRPlaneGr = GlobalVars.gAranGraphics.DrawPolygon(_MASPlanes[2].Poly, eFillStyle.sfsDiagonalCross, ARANFunctions.RGB(0, 127, 255));

			Functions.AnaliseMASObstacles(GlobalVars.ObstacleList, _RWYTHRPrj, _MASPlanes, _xOrigin, _MA_PDG, out _MASObstList);

			//============================================================================== prmFAPaltitude , prmMA_PDG
			CalcObstOCH();
		}

		void CalcObstOCH()
		{
			double FASOCH = Functions.CallcObstacleReqOCA(_FASObstList, _xOrigin, _hLost, _VPA, _MA_PDG, _MASegment);
			double HorizOCH = Functions.CallcObstacleReqOCA(_HorzObstList, _xOrigin, _hLost, _VPA, _MA_PDG, _MASegment);
			double MASOCH = Functions.CallcObstacleReqOCA(_MASObstList, _xOrigin, _hLost, _VPA, _MA_PDG, _MASegment);

			double ObstOCH = Math.Max(MASOCH, Math.Max(HorizOCH, FASOCH));
			textBox02007.Text = GlobalVars.unitConverter.HeightToDisplayUnits(ObstOCH).ToString();

			ApplyObstOCHCValue(ObstOCH);

			_IAPVSObstList.Clear();
			MergeLists(ref _IAPVSObstList, _FASObstList);
			MergeLists(ref _IAPVSObstList, _HorzObstList);
			MergeLists(ref _IAPVSObstList, _MASObstList);
			_reportForm.FillAPVList(_IAPVSObstList);

			//===================================================================================================

			double IMALenght = Functions.IntermMALenght(_IAPVSObstList, GlobalVars.constants.Pansops[ePANSOPSData.arMA_FinalMOC].Value, ObstOCH, _MA_PDG, _THRSOCDist);
			textBox02013.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(IMALenght).ToString();

			//===================================================================================================
			Functions.AnaliseObstacles2(GlobalVars.ObstacleList, _RWYTHRPrj, _ISegment, 150.0, out _InterApprObstList);
			_reportForm.FillInterApprList(_InterApprObstList);
		}

		void ApplyObstOCHCValue(double ObstOCH)
		{
			double TanVPA = Math.Tan(_VPA);

			_THRSOCDist = _xOrigin + (ObstOCH - _hLost) / TanVPA;
			textBox02012.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_THRSOCDist).ToString();

			_MAPt.SOCDistance = _MAPtTHRDist - _THRSOCDist;
			_MAPt.RefreshGraphics();

			//GlobalVars.gAranGraphics.SafeDeleteGraphic(_HorzLPlaneGr);
			//GlobalVars.gAranGraphics.SafeDeleteGraphic(_HorzPPlaneGr);
			//GlobalVars.gAranGraphics.SafeDeleteGraphic(_HorzRPlaneGr);

			//GlobalVars.gAranGraphics.SafeDeleteGraphic(_MASLPlaneGr);
			//GlobalVars.gAranGraphics.SafeDeleteGraphic(_MASPPlaneGr);
			//GlobalVars.gAranGraphics.SafeDeleteGraphic(_MASRPlaneGr);

			//GlobalVars.gAranGraphics.SafeDeleteGraphic(_FASLPlaneGr);
			//GlobalVars.gAranGraphics.SafeDeleteGraphic(_FASPPlaneGr);
			//GlobalVars.gAranGraphics.SafeDeleteGraphic(_FASRPlaneGr);

			//double TanVPA = Math.Tan(_VPA);
			//double OCH = prmObstOCH;

			//double PrevOCH = -1;

			//label02026.Visible = false;
			//double dOCH, MAPtThrDist;
			//int I = 0;

			//do
			//{
			//	double PrevOCH0 = PrevOCH;
			//	PrevOCH = OCH;
			//	MAPtThrDist = (PrevOCH - GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value) / TanVPA;

			//	if (MAPtThrDist > prmFAFTHRDist - 150.0 / TanVPA)
			//	{
			//		MAPtThrDist = 0;
			//		I = 45;
			//		label02026.Visible = true;
			//	}

			//	_MAPtTHRDist = MAPtThrDist;
			//	//==============================================================================
			//	_MAPt.NomLineAltitude = _MAPt.ConstructAltitude = _MOCAppr + _RWYTHRPrj.Z;
			//	_MAPt.EntryDirection = _RWYTHRPrj.M;
			//	_MAPt.OutDirection = _RWYTHRPrj.M;

			//	_MAPt.PrjPt = ARANFunctions.LocalToPrj(_RWYTHRPrj, _RWYTHRPrj.M, -_MAPtTHRDist, 0);


			//	//==============================================================================

			//	//double fTAS = ARANMath.IASToTAS(GlobalVars.constants.AircraftCategory[aircraftCategoryData.VfafMax].Value[_AirCat], GlobalVars.CurrADHP.Elev, GlobalVars.CurrADHP.ISAtC) + GlobalVars.constants.Pansops[ePANSOPSData.arNearTerrWindSp].Value;

			//	double Dist0 = fTAS * GlobalVars.constants.Pansops[ePANSOPSData.arSOCdelayTime].Value;
			//	double Dist1 = fTAS * GlobalVars.constants.Pansops[ePANSOPSData.dpPilotTolerance].Value;
			//	double SOCDist = MAPtThrDist - (Dist0 + Dist1 + _MAPt.ATT);

			//	_MAPt.SOCDistance = _MAPtTHRDist - SOCDist;
			//	prmTHRSOCDist = _MAPt.SOCDistance;

			//	//===
			//	_ISegment.CreateGeometry(null, GlobalVars.CurrADHP);
			//	_FASegment.CreateGeometry(_ISegment, GlobalVars.CurrADHP);
			//	_MASegment.CreateGeometry(_FASegment, GlobalVars.CurrADHP);

			//	//==============================================================================

			//	Functions.CreateHorzPlanes(_RWYTHRPrj, fTAS, _MOCAppr, _VPA, prmMA_PDG, _FASegment, _MASegment, out _HorzPlanes);
			//	Functions.CreateMASPlanes(_RWYTHRPrj, fTAS, _MOCAppr, _VPA, prmMA_PDG, _FASegment, _MASegment, out _MASPlanes);
			//	Functions.CreateFASPlanes(_RWYTHRPrj, prmFAPaltitude - _RWYTHRPrj.Z, _hMaxSS, _VPA, _FAS, _ISegment, _FASegment, out _FASPlanes);

			//	//==============================================================================

			//	Functions.AnaliseObstacles0(GlobalVars.ObstacleList, _RWYTHRPrj, _FASPlanes, Plane.FinalApproachSurface, out _FASObstList, _MOCAppr);
			//	Functions.AnaliseObstacles0(GlobalVars.ObstacleList, _RWYTHRPrj, _HorzPlanes, Plane.HorizontalSurface, out _HorzObstList, _MOCAppr, GlobalVars.constants.Pansops[ePANSOPSData.arMA_InterMOC].Value);
			//	Functions.AnaliseObstacles0(GlobalVars.ObstacleList, _RWYTHRPrj, _MASPlanes, Plane.MissedApproachSurface, out _MASObstList, GlobalVars.constants.Pansops[ePANSOPSData.arMA_InterMOC].Value);

			//	OCH = Functions.CallcObstacleReqOCA(_FASObstList, fTAS, _MOCAppr, _VPA, prmMA_PDG, _MASegment);
			//	double fTmp;	// = Functions.CallcObstacleReqOCA(_WObstList, fTAS, _MOCAppr, _VPA, prmMA_PDG, _MASegment);
			//	//OCH = Math.Max(fTmp, OCH);

			//	fTmp = Functions.CallcObstacleReqOCA(_HorzObstList, fTAS, _MOCAppr, _VPA, prmMA_PDG, _MASegment);
			//	OCH = Math.Max(fTmp, OCH);

			//	fTmp = Functions.CallcObstacleReqOCA(_MASObstList, fTAS, _MOCAppr, _VPA, prmMA_PDG, _MASegment);
			//	OCH = Math.Max(fTmp, OCH);

			//	//Accuracy = CConverters[puAltitude].ConvertFunction(GAltitudeAccuracy, cdToInner, nil);
			//	//OCH = AdvancedRound(OCH, Accuracy, rtCeil);

			//	if (PrevOCH0 == OCH)
			//		break;

			//	dOCH = Math.Abs(OCH - PrevOCH);
			//	I++;
			//}
			//while (I < 20 && dOCH > ARANMath.EpsilonDistance);//CConverters[puAltitude].Accuracy

			//MAPtThrDist = (PrevOCH - GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value) / TanVPA;

			//_ObstOCH = OCH;
			//textBox02011.Text = GlobalVars.unitConverter.HeightToDisplayUnits(OCH).ToString();
			//textBox02011.Tag = textBox02011.Text;

			////prmObstOCH = OCH;

			//_MAPtOCH = GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value + MAPtThrDist * TanVPA;
			//textBox02010.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_MAPtOCH).ToString();
			//textBox02010.Tag = textBox02010.Text;

			////prmMAPtOCH = GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value + MAPtThrDist * TanVPA;

			//_ISegment.RefreshGraphics();
			//_FASegment.RefreshGraphics();
			//_MASegment.RefreshGraphics();

			//_HorzLPlaneGr = GlobalVars.gAranGraphics.DrawPolygon(_HorzPlanes[0].Poly, ARANFunctions.RGB(0, 0, 127), eFillStyle.sfsDiagonalCross);
			//_HorzPPlaneGr = GlobalVars.gAranGraphics.DrawPolygon(_HorzPlanes[1].Poly, ARANFunctions.RGB(0, 127, 0), eFillStyle.sfsDiagonalCross);
			//_HorzRPlaneGr = GlobalVars.gAranGraphics.DrawPolygon(_HorzPlanes[2].Poly, ARANFunctions.RGB(0, 0, 127), eFillStyle.sfsDiagonalCross);

			//_MASLPlaneGr = GlobalVars.gAranGraphics.DrawPolygon(_MASPlanes[0].Poly, ARANFunctions.RGB(0, 95, 127), eFillStyle.sfsDiagonalCross);
			//_MASPPlaneGr = GlobalVars.gAranGraphics.DrawPolygon(_MASPlanes[1].Poly, ARANFunctions.RGB(0, 127, 95), eFillStyle.sfsDiagonalCross);
			//_MASRPlaneGr = GlobalVars.gAranGraphics.DrawPolygon(_MASPlanes[2].Poly, ARANFunctions.RGB(0, 95, 127), eFillStyle.sfsDiagonalCross);


			//_FASLPlaneGr = GlobalVars.gAranGraphics.DrawPolygon(_FASPlanes[0].Poly, ARANFunctions.RGB(0, 0, 255), eFillStyle.sfsDiagonalCross);
			//_FASPPlaneGr = GlobalVars.gAranGraphics.DrawPolygon(_FASPlanes[1].Poly, ARANFunctions.RGB(0, 255, 255), eFillStyle.sfsDiagonalCross);
			//_FASRPlaneGr = GlobalVars.gAranGraphics.DrawPolygon(_FASPlanes[2].Poly, ARANFunctions.RGB(0, 0, 255), eFillStyle.sfsDiagonalCross);

			//_IF.RefreshGraphics();
			//_FAF.RefreshGraphics();
			//_MAPt.RefreshGraphics();
			//_MAHF.RefreshGraphics();

			//_IAPVSObstList.Clear();
			//MergeLists(ref _IAPVSObstList, _FASObstList);
			////MergeLists(ref _IAPVSObstList, _WObstList);
			//MergeLists(ref _IAPVSObstList, _HorzObstList);
			//MergeLists(ref _IAPVSObstList, _MASObstList);

			//_reportForm.FillAPVList(_IAPVSObstList);

			////prmApliedOCH = Math.Max(prmObstOCH, prmMAPtOCH);

			//double IMALenght = Functions.IntermMALenght(_IAPVSObstList, GlobalVars.constants.Pansops[ePANSOPSData.arMA_FinalMOC].Value,
			//			Math.Max(prmObstOCH, prmMAPtOCH), prmMA_PDG, _THRSOCDist);

			////*** edit1.Text = FloatToStr(IMALenght);
			//prmIMALenght = IMALenght;

			//Functions.AnaliseObstacles0(GlobalVars.ObstacleList, _RWYTHRPrj, _MASPlanes, Plane.FinalMissedApproachSurface, out _MASObstList, _MOCAppr);
			//_reportForm.FillFMASList(_MASObstList);
		}

		private void textBox02001_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox02001_Validating(textBox02001, null);
			else
				Functions.TextBoxFloat(ref eventChar, textBox02001.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox02001_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!double.TryParse(textBox02001.Text, out double fTmp))
				return;

			if (textBox02001.Tag != null && textBox02001.Tag.ToString() == textBox02001.Text)
				return;

			_FAPheight = GlobalVars.unitConverter.HeightToInternalUnits(fTmp) - _RWYTHRPrj.Z;

			if (_FAPheight < _FAPheightRange.Min) _FAPheight = _FAPheightRange.Min;
			if (_FAPheight > _FAPheightRange.Max) _FAPheight = _FAPheightRange.Max;

			ApplyFAPheightValue();
		}

		private void textBox02003_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox02003_Validating(textBox02003, null);
			else
				Functions.TextBoxFloat(ref eventChar, textBox02003.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}
		
		private void textBox02003_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!double.TryParse(textBox02003.Text, out double fTmp))
				return;

			if (textBox02003.Tag != null && textBox02003.Tag.ToString() == textBox02003.Text)
				return;

			fTmp = _MA_PDG = GlobalVars.unitConverter.GradientToInternalUnits(fTmp);
			if (_MA_PDG < _MA_PDGRange.Min) _MA_PDG = _MA_PDGRange.Min;
			if (_MA_PDG > _MA_PDGRange.Max) _MA_PDG = _MA_PDGRange.Max;

			if (fTmp != _MA_PDG)
				textBox02003.Text = GlobalVars.unitConverter.GradientToDisplayUnits(_MA_PDG).ToString();
			textBox02003.Tag = textBox02003.Text;

			ApplyMA_PDGValue();
		}

		private void textBox02004_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox02004_Validating(textBox02004, null);
			else
				Functions.TextBoxFloat(ref eventChar, textBox02004.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox02004_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!double.TryParse(textBox02004.Text, out double fTmp))
				return;

			if (textBox02004.Tag != null && textBox02004.Tag.ToString() == textBox02004.Text)
				return;

			_VPA = ARANMath.DegToRad(fTmp);
			if (_VPA < _VPARange.Min) _VPA = _VPARange.Min;
			if (_VPA > _VPARange.Max) _VPA = _VPARange.Max;

			ApplyVPAValue();
		}

		#endregion

		#region ToIASegment
		private void ToIASegment()
		{
			_IFIASRange.Min = GlobalVars.constants.AircraftCategory[aircraftCategoryData.ViafMin][_AirCat];
			_IFIASRange.Max = GlobalVars.constants.AircraftCategory[aircraftCategoryData.ViafMax][_AirCat];

			_IF.IAS = _IFIASRange.Max;
			textBox03006.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_IF.IAS).ToString();

			_IntADG = GlobalVars.constants.Pansops[ePANSOPSData.arImDescent_Max].Value;
			textBox03003.Text = GlobalVars.unitConverter.GradientToDisplayUnits(_IntADG).ToString();

			_PlannedAngleRange.Min = 0;
			_PlannedAngleRange.Max = ARANMath.DegToRad(90);
			_PlannedAngle = _PlannedAngleRange.Max;

			ApplyPlannedAngleValue();
		}

		#endregion

		#region PAGE III - IA segment

		private void ApplyIFDistanceValue()
		{
			textBox03001.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_IFDistance).ToString();
			textBox03001.Tag = textBox03001.Text;

			double TrD = _IFDistance - _IF.CalcTurnRadius(_IF.IAS) * (Math.Tan(0.5 * _PlannedAngle) - 0.5 * _PlannedAngle);
			double HorSeg = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arImHorSegLen].Value[_AirCat];

			_HorizSeg = TrD - (_IFheight - _FAPheight) / _IntADG;
			textBox03005.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_HorizSeg).ToString();

			_IFheightRange.Min = _FAPheight;
			_IFheightRange.Max = HorSeg + (TrD - HorSeg) * _IntADG;

			double fTmp = _IFheight;

			if (_IFheight < _IFheightRange.Min)
				_IFheight = _IFheightRange.Min;

			if (_IFheight > _IFheightRange.Max)
				_IFheight = _IFheightRange.Max;

			if(fTmp!= _IFheight)
				ApplyIFAltitudeValue();
			//=======================================================

			_IF.PrjPt = ARANFunctions.LocalToPrj(_FAF.PrjPt, _RWYTHRPrj.M, -_IFDistance, 0);

			//FIF.PrjPt = pPoint;
			//GUI.SafeDeleteGraphic(FISFullPolyGr);
			//GUI.SafeDeleteGraphic(FISPrimPolyGr);
			//FISegment.StartFIX.Assign(FIF);
			_ISegment.CreateGeometry(null, GlobalVars.CurrADHP, 0.0, true);

			//CreateProtectionArea(nil, FISegment, FAerodrome);
			//CreateAssesmentArea(nil, FISegment);
			//FISFullPolyGr = GUI.DrawPolygon(FISegment.FullAssesmentArea, RGB(0, 255, 0), sfsDiagonalCross);
			//FISPrimPolyGr = GUI.DrawPolygon(FISegment.PrimaryAssesmentArea, RGB(0, 0, 255), sfsDiagonalCross);

			_ISegment.RefreshGraphics();
			_IF.RefreshGraphics();

			Functions.AnaliseObstacles2(GlobalVars.ObstacleList, _RWYTHRPrj, _ISegment, 150.0, out _InterApprObstList);
			_reportForm.FillInterApprList(_InterApprObstList);
		}

		private void ApplyIFAltitudeValue()
		{
			textBox03002.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_IFheight + _RWYTHRPrj.Z).ToString();
			textBox03002.Tag = textBox03002.Text;

			double TrD = _IFDistance - _IF.CalcTurnRadius(_IF.ConstructTAS) * (Math.Tan(0.5 * _PlannedAngle) - 0.5 * _PlannedAngle);
			_HorizSeg = TrD - (_IFheight - _FAPheight) / _IntADG;

			textBox03005.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_HorizSeg).ToString();
		}

		private void ApplyPlannedAngleValue()
		{
			textBox03004.Text = ARANMath.RadToDeg(_PlannedAngle).ToString("0.00");
			textBox03004.Tag = textBox03004.Text;

			double MinStabIF = _IF._CalcFromMinStablizationDistance(_PlannedAngle, _IF.ConstructTAS);

			_IFDistanceRange.Min = MinStabIF + GlobalVars.constants.Pansops[ePANSOPSData.rnvImMinDist].Value;

			if (_IFDistance < _IFDistanceRange.Min)
			{
				_IFDistance = _IFDistanceRange.Min;
				ApplyIFDistanceValue();
			}
		}

		private void editIFDistance_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				editIFDistance_Validating(textBox03001, null);
			else
				Functions.TextBoxFloat(ref eventChar, textBox03001.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void editIFDistance_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!double.TryParse(textBox03001.Text, out double fTmp))
				return;

			if (textBox03001.Tag != null && textBox03001.Tag.ToString() == textBox03001.Text)
				return;

			_IFDistance = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);
			if (_IFDistance < _IFDistanceRange.Min) _IFDistance = _IFDistanceRange.Min;
			if (_IFDistance > _IFDistanceRange.Max) _IFDistance = _IFDistanceRange.Max;

			ApplyIFDistanceValue();
		}

		private void editIFAltitude_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				editIFAltitude_Validating(textBox03002, null);
			else
				Functions.TextBoxFloat(ref eventChar, textBox03002.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void editIFAltitude_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!double.TryParse(textBox03002.Text, out double fTmp))
				return;

			if (textBox03002.Tag != null && textBox03002.Tag.ToString() == textBox03002.Text)
				return;

			fTmp = _IFheight = GlobalVars.unitConverter.HeightToInternalUnits(fTmp) - _RWYTHRPrj.Z;
			if (_IFheight < _IFheightRange.Min) _IFheight = _IFheightRange.Min;
			if (_IFheight > _IFheightRange.Max) _IFheight = _IFheightRange.Max;

			ApplyIFAltitudeValue();
		}

		private void editPlannedAngle_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				editPlannedAngle_Validating(textBox03004, null);
			else
				Functions.TextBoxFloat(ref eventChar, textBox03004.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void editPlannedAngle_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!double.TryParse(textBox03004.Text, out double fTmp))
				return;

			if (textBox03004.Tag != null && textBox03004.Tag.ToString() == textBox03004.Text)
				return;

			fTmp = _PlannedAngle = ARANMath.DegToRad(fTmp);
			if (_PlannedAngle < _PlannedAngleRange.Min) _PlannedAngle = _PlannedAngleRange.Min;
			if (_PlannedAngle > _PlannedAngleRange.Max) _PlannedAngle = _PlannedAngleRange.Max;

			ApplyPlannedAngleValue();
		}

		private void editIFIAS_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				editIFIAS_Validating(textBox03006, null);
			else
				Functions.TextBoxFloat(ref eventChar, textBox03006.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void editIFIAS_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!double.TryParse(textBox03006.Text, out double fTmp))
				return;

			if (textBox03006.Tag != null && textBox03006.Tag.ToString() == textBox03006.Text)
				return;

			double IFIAS = fTmp = GlobalVars.unitConverter.SpeedToInternalUnits(fTmp);

			if (IFIAS < _IFIASRange.Min) IFIAS = _IFIASRange.Min;
			if (IFIAS > _IFIASRange.Max) IFIAS = _IFIASRange.Max;

			_IF.IAS = IFIAS;

			if (fTmp != IFIAS)
				textBox03006.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(IFIAS).ToString();
			textBox03006.Tag = textBox03006.Text;

			ApplyIFAltitudeValue();
			ApplyIFDistanceValue();
		}

		bool SaveProcedure()
		{

			return true;
		}

		#endregion
	}
}
