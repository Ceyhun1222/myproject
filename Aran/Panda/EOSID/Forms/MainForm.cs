using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EOSID.Properties;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using AIXM.Features;
using AIXM;
using AIXM.DataTypes;
using AIXM.Features.Geometry;
using GML;
using AIXM.DataTypes.Enums;


namespace EOSID
{
	public partial class MainForm : Form
	{
		#region constants
		private const double AreaMinSeminwidth = 2.0 * 1852.0;
		private const double AreaMaxSeminwidth = 4.0 * 1852.0;

		private const double rDMEMin = 4.0 * 1852.0;
		private const double rDMEMax = 40.0 * 1852.0;

		private const double cMinH = 30.0;
		private const double cMaxH = 12800.0;

		private const double cMinDist = 500.0;
		private const double cMaxDist = 100000.0;

		private const double WPTFarness = 100.0;

		private const double tStabl = 6.0;	// m/sec

		private const double minAcselertedPhaseNetGradient = 0.0;	//APNG
		private const double maxAcselertedPhaseNetGradient = 0.007;

		private const double minBankAngle = 15;
		private const double maxBankAngle = 25;

		private const double minTailWind = 19.0;	// km/hour
		private const double maxTailWind = 56.0;	// km/hour

		private const double minHeadWind = 19.0;	// km/hour
		private const double maxHeadWind = 56.0;	// km/hour

		private double minAircraftSpeed2 = 270.0;	// km/hour
		private double maxAircraftSpeed2 = 295.0;	// km/hour

		private double minAircraftSpeed4 = 270.0;	// km/hour
		private double maxAircraftSpeed4 = 295.0;	// km/hour
		#endregion

		#region varaibles
		private double BestMaxNetGrad2;
		private double WorstMaxNetGrad2;

		private double BestMaxGrossGrad;
		private double WorstMaxGrossGrad;

		private double BestMaxNetGrad4;
		private double WorstMaxNetGrad4;

		private IElement pCircleElem;
		private IElement DerElem;

		private IElement pNominalElem;
		private IElement protectionAreaElem;

		private IElement pCurrBestNominalTrackElem;
		private IElement pCurrWorstNominalTrackElem;
		private IElement pCurrProtectAreaElem;

		//private double SplayAngle;

		private bool m_bFormInitialised;
		private Label[] NavLabels;

		private RWYData[] RWYList;
		private IPoint m_ptDerPrj;
		private IPoint m_ptDerGeo;
		private IPoint m_ptCenter;
		private IPolygon m_pCircle;

		private double m_DepDir;
		private double m_DepAzt;
		private double m_AcselertedPhaseNetGradient;

		private ObstacleData[] AllObsstacles;
		private TrackLeg FirstLeg;

		private LegsInfoForm InfoFrm;
		private ReportForm ReportsFrm;

		private bool HaveReport;
		private bool HaveLeg;

		private RadioButton[] radioButton0300;
		private int m_CurrPage;
		private int HelpContextID;

		#endregion

		#region Legs declerations

		private int maxSegs;
		private int LegCount;

		private double m_minAcceleDist;
		private double m_AcceleDist;
		private int m_AcceleDistLeg;
		private double m_BankAngle;
		private double m_TailWind;
		private double m_HeadWind;
		//private double m_IAS;

		private double m_CurrAzt;

		//private double m_NetHFinish;
		private double m_GrossHFinish;

		private double m_MagVar;

		private LegPoint m_CurrPnt;

		//private eLegType prevSegType;
		private eLegType m_SegType;

		private TrackLeg CurrSegment;
		private TrackLeg PrevSegment;
		private TrackLeg[] LegList;

		#region construction variables

		private double Type1_CurDist;
		private double Type1_CurDir;
		private double Type1_CurHeight;

		private Interval Type1_RadInterval;
		private NavaidData Type1_CurNavaid;
		private NavaidData[] ComboBox102List;

		private double Type2_OutDir;
		private double Type3_OutDir;

		private int Type3_TurnDir;

		private WPT_FIXData Type3_CurNav;
		private Interval Type3_Interval;
		private Interval Type3_FullInterval;

		private Interval[] ComboBox301_LIntervals;
		private Interval[] ComboBox301_RIntervals;

		private double Type5_CurDir;
		private double Type5_SnapAngle;
		private WPT_FIXData Type5_CurNav;
		private Interval[] Type5_Intervals;

		private NavaidData Type6_CurNav;
		private Interval[] Type6_Intervals;

		private NavaidData Type7_CurFix;
		private NavaidData Type7_CurNav;
		private Interval[] Type7_Intervals;
		#endregion

		#endregion

		#region Form

		private void FocusStepCaption(int StIndex)
		{
			int n = NavLabels.Length;
			for (int i = 0; i < n; i++)
			{
				NavLabels[i].ForeColor = System.Drawing.Color.FromArgb(0XC0C0C0);
				NavLabels[i].Font = new System.Drawing.Font(NavLabels[i].Font, System.Drawing.FontStyle.Regular);
			}
			if (StIndex >= n)
				StIndex = n - 1;
			NavLabels[StIndex].ForeColor = System.Drawing.Color.FromArgb(0XFF8000);
			NavLabels[StIndex].Font = new System.Drawing.Font(NavLabels[StIndex].Font, System.Drawing.FontStyle.Bold);

			System.Reflection.AssemblyName thisAssemName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
			Version ver = thisAssemName.Version;

			this.Text = "EO SID - v:" + ver.Major.ToString() + "." + ver.Minor.ToString() + "." + ver.Revision.ToString() + "   " + MultiPage1.TabPages[StIndex].Text;

			//Microsoft.VisualBasic.ApplicationServices.ConsoleApplicationBase consoleBase = new Microsoft.VisualBasic.ApplicationServices.ConsoleApplicationBase();
			//this.Text = Resources.str33 + " v:" + consoleBase.Info.Version.Major.ToString() + "." + consoleBase.Info.Version.Minor.ToString() + "." + consoleBase.Info.Version.Revision.ToString() + "   " + MultiPage1.TabPages[StIndex].Text;
		}

		public MainForm()
		{
			InitializeComponent();

			m_bFormInitialised = false;
			NavLabels = new Label[] { Label01, Label02, Label03, Label04 };

			int i, n = GlobalVars.ADHPList.Length;
			if (n <= 0)
			{
				MessageBox.Show("thNo Aeroports", "EOSID", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				Close();
				return;
			}

			for (i = 0; i < NavLabels.Length - 1; i++)
				NavLabels[i].Text = MultiPage1.TabPages[i].Text;

			NavLabels[3].Text = MultiPage1.TabPages[MultiPage1.TabCount - 1].Text;

			radioButton0300 = new RadioButton[] { radioButton0301, radioButton0302, radioButton0303 };

			numericUpDown0301.Maximum = (decimal)UnitConverter.HeightToDisplayUnits(GlobalVars.ProtectionWidt[GlobalVars.ProtectionWidt.Length - 1], eRoundMode.NONE);
			numericUpDown0301.Minimum = (decimal)UnitConverter.HeightToDisplayUnits(GlobalVars.ProtectionWidt[0], eRoundMode.NONE);
			numericUpDown0301.Value = numericUpDown0301.Minimum;
			numericUpDown0301.Increment = (decimal)UnitConverter.HeightToDisplayUnits(300.0, eRoundMode.NONE);

			WorstMaxNetGrad2 = 0.024;
			textBox0103.Text = UnitConverter.GradientToDisplayUnits(WorstMaxNetGrad2, eRoundMode.NERAEST).ToString();

			BestMaxNetGrad2 = 0.04;
			textBox0107.Text = UnitConverter.GradientToDisplayUnits(BestMaxNetGrad2, eRoundMode.NERAEST).ToString();

			WorstMaxNetGrad4 = 0.012;
			textBox0104.Text = UnitConverter.GradientToDisplayUnits(WorstMaxNetGrad4, eRoundMode.NERAEST).ToString();

			BestMaxNetGrad4 = 0.02;
			textBox0108.Text = UnitConverter.GradientToDisplayUnits(BestMaxNetGrad4, eRoundMode.NERAEST).ToString();

			/*
			Label1[0].Text = Resources.str100;
			Label1[1].Text = Resources.str110;
			Label1[2].Text = Resources.str120;
			Label1[3].Text = Resources.str130;
			Label1[4].Text = Resources.str140;
			Label1[5].Text = Resources.str150;
			*/

			label0005.Text = UnitConverter.DistanceUnit;
			label0009.Text = UnitConverter.HeightUnit;
			label0013.Text = UnitConverter.HeightUnit;
			//=================================================================
			label0106.Text = UnitConverter.SpeedUnit;
			label0108.Text = UnitConverter.SpeedUnit;
			label0117.Text = UnitConverter.SpeedUnit;
			label0119.Text = UnitConverter.SpeedUnit;

			//=================================================================

			label0301.Text = UnitConverter.HeightUnit;
			label0305.Text = UnitConverter.SpeedUnit;
			label0307.Text = UnitConverter.SpeedUnit;
			//label0309.Text = UnitConverter.SpeedUnit;

			label0313.Text = UnitConverter.DistanceUnit;
			//label0315.Text = UnitConverter.DistanceUnit;
			label0303.Text = UnitConverter.HeightUnit;
			//=================================================================

			label103.Text = UnitConverter.DistanceUnit;
			label108.Text = UnitConverter.HeightUnit;

			Label207.Text = UnitConverter.DistanceUnit;

			Label310.Text = UnitConverter.DistanceUnit;
			Label314.Text = UnitConverter.DistanceUnit;

			Label409.Text = UnitConverter.DistanceUnit;

			Label506.Text = UnitConverter.DistanceUnit;
			Label522.Text = UnitConverter.DistanceUnit;

			Label607.Text = UnitConverter.DistanceUnit;
			Label610.Text = UnitConverter.DistanceUnit;
			Label613.Text = UnitConverter.DistanceUnit;
			Label615.Text = UnitConverter.DistanceUnit;

			label0402.Text = UnitConverter.HeightUnit;
			label0404.Text = UnitConverter.DistanceUnit;
			label0413.Text = UnitConverter.DistanceUnit;
			label0415.Text = UnitConverter.HeightUnit;

			maxSegs = 256;
			LegCount = 0;

			LegList = new TrackLeg[maxSegs];
			for (i = 0; i < maxSegs; i++)
				LegList[i].Initialize();

			this.m_CurrPage = 0;
			//this.HelpContextID = 4000;
			MultiPage1.SelectedIndex = this.m_CurrPage;

			comboBox0004.Items.Add(UnitConverter.HeightUnitM);
			comboBox0004.Items.Add(UnitConverter.HeightUnitFt);

			comboBox0005.Items.Add(UnitConverter.HeightUnitM);
			comboBox0005.Items.Add(UnitConverter.HeightUnitFt);

			comboBox0301.Items.Add("Абсолютная высота:");
			comboBox0301.Items.Add("Относит. высота:");

			comboBox0401.Items.Add("Абсолютная высота:");
			comboBox0401.Items.Add("Относит. высота:");

			comboBox0402.Items.Add("Абсолютная высота:");
			comboBox0402.Items.Add("Относит. высота:");

			comboBox0003.SelectedIndex = 0;
			comboBox0004.SelectedIndex = 0;
			comboBox0005.SelectedIndex = 0;
			//comboBox0201.SelectedIndex = 0;

			GlobalVars.heightAboveDER = GlobalVars.heightAboveDERMax;
			textBox0001.Text = UnitConverter.DistanceToDisplayUnits(GlobalVars.RModel, eRoundMode.NERAEST).ToString();
			textBox0007.Text = UnitConverter.HeightToDisplayUnits(GlobalVars.heightAboveDER, eRoundMode.NERAEST).ToString();

			TextBox103.Text = UnitConverter.HeightToDisplayUnits(cMinH, eRoundMode.CEIL).ToString();

			//ComboBox104.Items.Add("Абсолютная высота:");
			//ComboBox104.Items.Add("Относит. высота:");
			ComboBox104.SelectedIndex = 0;

			ComboBox201.Items.Add("Левый");
			ComboBox201.Items.Add("Правый");
			ComboBox201.SelectedIndex = 0;

			ComboBox302.Items.Add("Левый");
			ComboBox302.Items.Add("Правый");
			ComboBox302.SelectedIndex = 0;

			ComboBox402.Items.Add("Левый");
			ComboBox402.Items.Add("Правый");
			ComboBox402.SelectedIndex = 0;

			ComboBox502.Items.Add("Левый");
			ComboBox502.Items.Add("Правый");
			ComboBox502.SelectedIndex = 0;

			TextBox603.Text = UnitConverter.DistanceToDisplayUnits(rDMEMin, eRoundMode.NERAEST).ToString();
			TextBox604.Text = UnitConverter.DistanceToDisplayUnits(rDMEMax, eRoundMode.NERAEST).ToString();
			TextBox605.Text = UnitConverter.DistanceToDisplayUnits(rDMEMin, eRoundMode.NERAEST).ToString();

			ComboBox601.SelectedIndex = 0;
			ComboBox602.SelectedIndex = 0;
			ComboBox702.SelectedIndex = 0;

			//==========================================================================
			HaveReport = false;
			HaveLeg = false;

			ReportsFrm = new ReportForm();
			InfoFrm = new LegsInfoForm();

			ReportsFrm.Init(ReportBtn, 0);
			InfoFrm.Init(LegsBtn, 0);

			FocusStepCaption(0);
			MultiPage1.Top = -MultiPage1.ItemSize.Height - 3; // -MultiPage1.ItemSize.Height-3
			Height = Height - MultiPage1.ItemSize.Height - 3;
			frame01.Top = frame01.Top - MultiPage1.ItemSize.Height - 3;

			ShowPanelBtn.Checked = false;
			this.Width = frame02.Left + 6;
			//this.Width = 495;

			comboBox0001.Items.Clear();
			for (i = 0; i < n; i++)
				comboBox0001.Items.Add(GlobalVars.ADHPList[i]);

			this.m_bFormInitialised = true;

			comboBox0001.SelectedIndex = 0;

			m_bFormInitialised = true;
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (InfoFrm != null)
				InfoFrm.Close();
			InfoFrm = null;

			if (ReportsFrm != null)
				ReportsFrm.Close();
			ReportsFrm = null;

			ClearSegmentDrawings(false);

			for (int i = 0; i < LegCount; i++)
			{
				Graphics.DeleteElement(LegList[i].BestCase.pNominalElement);
				LegList[i].BestCase.pNominalElement = null;

				Graphics.DeleteElement(LegList[i].WorstCase.pNominalElement);
				LegList[i].WorstCase.pNominalElement = null;

				Graphics.DeleteElement(LegList[i].pProtectionElement);
				LegList[i].pProtectionElement = null;
			}

			Graphics.DeleteElement(pCircleElem);
			Graphics.DeleteElement(DerElem);

			Graphics.DeleteElement(pNominalElem);
			Graphics.DeleteElement(protectionAreaElem);
			Graphics.RefreshGraphics();
		}

		private void MainForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				e.IsInputKey = true;
		}

		private void MainForm_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F1)
			{
				//NativeMethods.HtmlHelp(0, GlobalVars.HelpFile, GlobalVars.HH_HELP_CONTEXT, HelpContextID);
				//e.Handled = true;
			}
		}

		private void TextBoxes_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				e.IsInputKey = true;
		}

		private void ShowPanelBtn_CheckedChanged(object sender, EventArgs e)
		{
			if (!m_bFormInitialised)
				return;

			if (ShowPanelBtn.Checked)
			{
				this.Width = frame02.Left + frame02.Width + 6;// FontResizeFactorProvider.Scale(3);
				ShowPanelBtn.Image = Resources.HIDE_INFO;
			}
			else
			{
				this.Width = frame02.Left + 6;
				ShowPanelBtn.Image = Resources.SHOW_INFO;
			}

			if (NextBtn.Enabled)
				NextBtn.Focus();
			else
				PrevBtn.Focus();
		}

		private void PrevBtn_Click(object sender, EventArgs e)
		{
			this.m_CurrPage = MultiPage1.SelectedIndex - 1;
			if (this.m_CurrPage == 9)
				this.m_CurrPage = 2;

			if (this.m_CurrPage == 1)
			{
				ClearSegmentDrawings();

				for (int i = 0; i < LegCount; i++)
				{
					Graphics.DeleteElement(LegList[i].pProtectionElement);
					Graphics.DeleteElement(LegList[i].BestCase.pNominalElement);
					Graphics.DeleteElement(LegList[i].WorstCase.pNominalElement);
				}

				InfoFrm.RemoveAllLegs();
				ReportsFrm.RemoveAllLegs();

				LegCount = 0;
				HaveLeg = false;
				CreateBtn.Enabled = false;
				RemoveSegmentBtn.Enabled = false;
				PrevSegment.SegmentCode = eLegType.NONE;
			}
			else if (this.m_CurrPage == 2)
			{
				RemoveSegmentBtn.Enabled = HaveLeg;
				CreateBtn.Enabled = true;
			}

			MultiPage1.SelectedIndex = this.m_CurrPage;

			FocusStepCaption(this.m_CurrPage);

			PrevBtn.Enabled = MultiPage1.SelectedIndex > 0;
			NextBtn.Enabled = true;
			OkBtn.Enabled = false;

			//NativeMethods.HidePandaBox();
		}

		private void NextBtn_Click(object sender, EventArgs e)
		{
			NativeMethods.ShowPandaBox(this.Handle.ToInt32());

			switch (MultiPage1.SelectedIndex)
			{
				case 0:
					if (comboBox0002.SelectedIndex < 0)
					{
						NativeMethods.HidePandaBox();
						MessageBox.Show("Select RWY.");
						return;
					}
					AddvanceToPageII();
					break;
				case 1:
					NextBtn.Enabled = false;
					AddvanceToPageIII();
					AddvanceToPageIV();
					break;
				case 2:
					AddvanceToPageVI();
					break;
			}

			NativeMethods.HidePandaBox();

			this.m_CurrPage = MultiPage1.SelectedIndex + 1;

			if (this.m_CurrPage == 3)
			{
				MultiPage1.SelectedIndex = MultiPage1.TabCount - 1;
				OkBtn.Enabled = true;
				NextBtn.Enabled = false;
			}
			else
			{
				MultiPage1.SelectedIndex = this.m_CurrPage;
				OkBtn.Enabled = false;
			}

			FocusStepCaption(this.m_CurrPage);
			PrevBtn.Enabled = true;
			//this.HelpContextID = 4000 + 100 * (MultiPage1.SelectedIndex + 1);
		}

		private void LegsBtn_CheckedChanged(object sender, EventArgs e)
		{
			if (!HaveLeg)
				return;

			if (LegsBtn.Checked)
				InfoFrm.Show(GlobalVars.win32Window);
			else
				InfoFrm.Hide();
		}

		private void ReportBtn_CheckedChanged(object sender, EventArgs e)
		{
			if (!HaveReport)
				return;

			if (ReportBtn.Checked)
				ReportsFrm.Show(GlobalVars.win32Window);
			else
				ReportsFrm.Hide();
		}

		private void OkBtn_Click(object sender, EventArgs e)
		{
			String RepFileName;
			String RepFileTitle;

			if (Functions.ShowSaveDialog(out RepFileName, out RepFileTitle))
			{
				ReportHeader pReport;

				pReport.Procedure = "EOSID";
				pReport.Aerodrome = comboBox0001.Text;
				pReport.RWY = comboBox0002.Text;

				if (comboBox0003.SelectedIndex == 0)
					pReport.Category = "C";
				else
					pReport.Category = "D";

				pReport.Database = null;
				pReport.EffectiveDate = null;

				//ConvertTracToPoints();
				//SaveLog(RepFileName, RepFileTitle, pReport);
				//SaveProtocol(RepFileName, RepFileTitle, pReport);
				CReportFile.SaveGeometry(RepFileName, RepFileTitle, pReport, GlobalVars.m_SelectedRWY, LegList, LegCount, radioButton0401.Checked);

				if (SaveProcedure(RepFileTitle))
					this.Close();
			}
		}

		private void CancelBtn_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void AddSegmentBtn_Click(object sender, EventArgs e)
		{
			numericUpDown0301.Minimum = numericUpDown0301.Value;
			m_GrossHFinish = CurrSegment.ptEnd.BestCase.GrossHeight;

			ClearSegmentDrawings();

			CurrSegment.pProtectionElement = Graphics.DrawPolygon(CurrSegment.pProtectionArea, Functions.RGB(0, 0, 255));
			CurrSegment.pProtectionElement.Locked = true;

			CurrSegment.BestCase.pNominalElement = Graphics.DrawPolyline(CurrSegment.BestCase.pNominalPoly, Functions.RGB(255, 0, 0));
			CurrSegment.BestCase.pNominalElement.Locked = true;

			CurrSegment.WorstCase.pNominalElement = Graphics.DrawPolyline(CurrSegment.WorstCase.pNominalPoly, Functions.RGB(0, 0, 255), 3);
			CurrSegment.WorstCase.pNominalElement.Locked = true;

			if (LegCount >= maxSegs)
			{
				maxSegs += 256;

				Array.Resize<TrackLeg>(ref LegList, maxSegs);
				for (int i = maxSegs - 256; i < maxSegs; i++)
					LegList[i].Initialize();
			}

			if (CurrSegment.ObsMaxAcceleDist >= 0)
			{
				if (CurrSegment.ObstacleList[CurrSegment.ObsMaxAcceleDist].AcceleStartDist > m_AcceleDist)
				{
					m_AcceleDist = CurrSegment.ObstacleList[CurrSegment.ObsMaxAcceleDist].AcceleStartDist;
					m_AcceleDistLeg = LegCount;
				}
				textBox0306.Text = UnitConverter.DistanceToDisplayUnits(m_AcceleDist, eRoundMode.NERAEST).ToString();
			}

			CurrSegment.AcceleDist = m_AcceleDist;
			CurrSegment.AcceleLeg = m_AcceleDistLeg;

			LegList[LegCount++] = CurrSegment;
			HaveLeg = true;

			InfoFrm.AddLeg(CurrSegment);
			ReportsFrm.AddLeg(CurrSegment);

			PrevSegment = CurrSegment;

			m_CurrPnt = PrevSegment.ptEnd;
			m_CurrPnt.BestCase.pPoint.M = m_CurrPnt.BestCase.Direction;
			m_CurrPnt.WorstCase.pPoint.M = m_CurrPnt.WorstCase.Direction;

			CurrSegment = CreateNextSegment(m_CurrPnt);
			//											PrevSegment
			CurrSegment.BestCase.PrevTotalLength = LegList[LegCount - 1].BestCase.PrevTotalLength + LegList[LegCount - 1].BestCase.Length;
			CurrSegment.BestCase.PrevTotalFlightTime = LegList[LegCount - 1].BestCase.PrevTotalFlightTime + LegList[LegCount - 1].BestCase.FlightTime;

			CurrSegment.WorstCase.PrevTotalLength = LegList[LegCount - 1].WorstCase.PrevTotalLength + LegList[LegCount - 1].WorstCase.Length;
			CurrSegment.WorstCase.PrevTotalFlightTime = LegList[LegCount - 1].WorstCase.PrevTotalFlightTime + LegList[LegCount - 1].WorstCase.FlightTime;

			textBox0309.Text = Math.Round(CurrSegment.BestCase.PrevTotalFlightTime, 2).ToString();

			CurrSegment.ptStart.WorstCase.Width = LegList[LegCount - 1].ptEnd.WorstCase.Width;
			CurrSegment.ptStart.BestCase.Width = LegList[LegCount - 1].ptEnd.BestCase.Width;
			//			PrevSegment
			if (LegList[LegCount - 1].SegmentCode == eLegType.arcIntercept)
			{
				if (CurrSegment.ptStart.BestCase.Width < GlobalVars.ArcProtectWidth)
					CurrSegment.ptStart.BestCase.Width = GlobalVars.ArcProtectWidth;

				if (CurrSegment.ptStart.WorstCase.Width < GlobalVars.ArcProtectWidth)
					CurrSegment.ptStart.WorstCase.Width = GlobalVars.ArcProtectWidth;
			}

			PrevBtn.Enabled = true;
			RemoveSegmentBtn.Enabled = true;
			CreateBtn.Enabled = true;
			AddSegmentBtn.Enabled = false;
			ReturnBtn.Enabled = false;

			NavLabels[2].Text = MultiPage1.TabPages[2].Text;
			FocusStepCaption(1);

			numericUpDown0301.Enabled = true;
			numericUpDown0301.Minimum = numericUpDown0301.Value;

			MultiPage1.SelectedIndex = 2;
			NextBtn.Enabled = true;
		}

		private void ReturnBtn_Click(object sender, EventArgs e)
		{
			ClearSegmentDrawings();

			PrevBtn.Enabled = true;
			NextBtn.Enabled = HaveLeg;
			CreateBtn.Enabled = true;

			AddSegmentBtn.Enabled = false;
			RemoveSegmentBtn.Enabled = HaveLeg;
			ReturnBtn.Enabled = false;

			NavLabels[2].Text = MultiPage1.TabPages[2].Text;
			FocusStepCaption(1);

			MultiPage1.SelectedIndex = 2;
		}

		private void RemoveSegmentBtn_Click(object sender, EventArgs e)
		{
			ClearSegmentDrawings();

			LegCount--;
			HaveLeg = LegCount > 0;

			Graphics.DeleteElement(LegList[LegCount].pProtectionElement);
			Graphics.DeleteElement(LegList[LegCount].BestCase.pNominalElement);
			Graphics.DeleteElement(LegList[LegCount].WorstCase.pNominalElement);

			InfoFrm.RemoveLastLeg();
			ReportsFrm.RemoveLastLeg();
			NextBtn.Enabled = LegCount > 0;
			RemoveSegmentBtn.Enabled = LegCount > 0;


			PrevSegment.SegmentCode = eLegType.NONE;

			if (HaveLeg)
			{
				CurrSegment = LegList[LegCount - 1];

				m_GrossHFinish = CurrSegment.ptEnd.BestCase.GrossHeight;
				m_AcceleDist = CurrSegment.AcceleDist;
				m_AcceleDistLeg = CurrSegment.AcceleLeg;

				textBox0306.Text = UnitConverter.DistanceToDisplayUnits(m_AcceleDist, eRoundMode.NERAEST).ToString();

				m_CurrPnt = CurrSegment.ptEnd;
				if (LegCount > 1)
				{
					PrevSegment = LegList[LegCount - 2];
					numericUpDown0301.Minimum = (decimal)UnitConverter.HeightToDisplayUnits(LegList[LegCount - 2].PlannedEndWidth, eRoundMode.NONE);
				}
				else
					numericUpDown0301.Minimum = (decimal)UnitConverter.HeightToDisplayUnits(GlobalVars.ProtectionWidt[0], eRoundMode.NONE);

				numericUpDown0301.Value = (decimal)UnitConverter.HeightToDisplayUnits(CurrSegment.PlannedEndWidth, eRoundMode.NONE);
				m_SegType = CurrSegment.SegmentCode;

				//m_MagVar = CurrSegment.OutMagVar;
				m_GrossHFinish = CurrSegment.ptEnd.BestCase.GrossHeight;
				m_CurrAzt = NativeMethods.Modulus((Functions.Dir2Azt(m_CurrPnt.BestCase.pPoint, m_CurrPnt.BestCase.Direction)));
				m_BankAngle = CurrSegment.BankAngle;

				//textBox0302.Text = UnitConverter.SpeedToDisplayUnits(m_TailWind, eRoundMode.NERAEST).ToString();
				//textBox0303.Text = UnitConverter.SpeedToDisplayUnits(m_HeadWind, eRoundMode.NERAEST).ToString();
				textBox0305.Text = m_BankAngle.ToString();
				textBox0308.Text = System.Math.Round(NativeMethods.Modulus(m_CurrAzt - m_MagVar), 2).ToString();
				textBox0309.Text = Math.Round(CurrSegment.BestCase.FlightTime, 2).ToString();

				//CurrSegment = CreateNextSegment(PrevSegment.ptEnd);

				if (m_SegType == eLegType.NONE)
				{
					radioButton0301.Enabled = true;
					radioButton0301.Checked = true;
					m_SegType = eLegType.straight;

					radioButton0302.Enabled = false;
					radioButton0303.Enabled = false;
					radioButton0304.Enabled = false;
					radioButton0305.Enabled = false;
					radioButton0306.Enabled = false;
					radioButton0307.Enabled = false;
				}
				else if (m_SegType == eLegType.arcIntercept)
				{
					radioButton0301.Enabled = false;
					radioButton0302.Enabled = false;
					radioButton0303.Enabled = false;
					radioButton0304.Enabled = false;
					radioButton0305.Enabled = false;
					radioButton0306.Enabled = false;

					radioButton0307.Enabled = true;
					radioButton0307.Checked = true;
					m_SegType = eLegType.arcPath;
				}
				else if (CurrSegment.ptEnd.atHeight)
				{
					radioButton0301.Enabled = false;
					radioButton0302.Enabled = true;
					radioButton0302.Checked = true;
					m_SegType = eLegType.toHeading;

					radioButton0303.Enabled = false;	// FillComboBox301Stations() > 0;
					radioButton0304.Enabled = FillComboBox401Stations() > 0;
					radioButton0305.Enabled = FillComboBox501Stations() > 0;
					radioButton0306.Enabled = false;	// FillComboBox603DMEStations() > 0;
					radioButton0307.Enabled = false;
				}
				else
				{
					double Delta = Functions.SubtractAngles(m_CurrPnt.BestCase.Direction, m_CurrPnt.WorstCase.Direction);

					radioButton0301.Enabled = Delta < 5.0;
					radioButton0302.Enabled = true;

					if (PrevSegment.SegmentCode == eLegType.arcPath)
					{
						if (radioButton0301.Enabled)
							radioButton0301.Checked = true;
						else
							radioButton0302.Checked = true;
					}

					radioButton0303.Enabled = FillComboBox301Stations() > 0;
					radioButton0304.Enabled = FillComboBox401Stations() > 0;
					radioButton0305.Enabled = FillComboBox501Stations() > 0;

					double x, y;
					Functions.PrjToLocal(m_CurrPnt.BestCase.pPoint, m_CurrPnt.BestCase.Direction, m_CurrPnt.WorstCase.pPoint, out x, out y);

					if (Delta < GlobalVars.degEps && Math.Abs(y) < 5.0)
						radioButton0306.Enabled = FillComboBox603DMEStations() > 0;
					else
						radioButton0306.Enabled = false;

					radioButton0307.Enabled = false;
				}
			}
			else
			{
				AddvanceToPageIII();
				AddvanceToPageIV();
			}
		}

		#endregion

		#region Page I

		void AddvanceToPageII()
		{
			DBModule.FillWPT_FIXList(out GlobalVars.WPTList, GlobalVars.m_CurrADHP, GlobalVars.MaxNAVDist);
			DBModule.FillNavaidList(out GlobalVars.NavaidList, out GlobalVars.DMEList, GlobalVars.m_CurrADHP, GlobalVars.MaxNAVDist);
			DBModule.GetObstacles(out AllObsstacles, m_ptCenter, GlobalVars.RModel, m_ptDerPrj.Z);
			comboBox0101.Items.Clear();
			comboBox0102.Items.Clear();
			//240 300 365 392
			if (comboBox0003.SelectedIndex == 1)
			{
				int n = GlobalVars.mass_AN124.Length;
				for (int i = 0; i < n; i++)
				{
					comboBox0101.Items.Add(GlobalVars.mass_AN124[i]);
					comboBox0102.Items.Add(GlobalVars.mass_AN124[i]);
				}
			}
			else
			{
				int n = GlobalVars.mass_IL76.Length;
				for (int i = 0; i < n; i++)
				{
					comboBox0101.Items.Add(GlobalVars.mass_IL76[i]);
					comboBox0102.Items.Add(GlobalVars.mass_IL76[i]);
				}
			}
			comboBox0101.SelectedIndex = 0;
			comboBox0102.SelectedIndex = 0;
		}

		private void comboBox0001_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!m_bFormInitialised)
				return;

			comboBox0002.Items.Clear();

			int i = comboBox0001.SelectedIndex;
			if (i < 0)
				return;

			GlobalVars.m_CurrADHP = GlobalVars.ADHPList[i];

			DBModule.FillADHPFields(ref GlobalVars.m_CurrADHP);
			if (GlobalVars.m_CurrADHP.pPtGeo == null)
			{
				MessageBox.Show("Error reading ADHP.", "EOSID", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}

			DBModule.FillRWYList(out RWYList, GlobalVars.m_CurrADHP);

			int n = RWYList.Length;

			if (n <= 0)
			{
				MessageBox.Show("RWY ERROR.", "EOSID", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}

			//ptCenter = CurrADHP.pPtPrj;
			//textBox001.Tag = null;
			//textBox001_Validating(textBox001, new CancelEventArgs());

			for (i = 0; i < n; i++)
				comboBox0002.Items.Add(RWYList[i].Name);

			comboBox0002.SelectedIndex = 0;
		}

		private void comboBox0002_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = comboBox0002.SelectedIndex;
			if (i < 0)
				return;

			GlobalVars.m_SelectedRWY = RWYList[i];

			m_ptDerPrj = GlobalVars.m_SelectedRWY.pPtPrj[eRWY.PtDER];
			m_ptDerGeo = GlobalVars.m_SelectedRWY.pPtGeo[eRWY.PtDER];

			m_ptCenter = m_ptDerPrj;
			textBox0001.Tag = null;
			textBox0001_Validating(textBox0001, new CancelEventArgs());

			Graphics.DeleteElement(DerElem);
			DerElem = Graphics.DrawPointWithText(m_ptDerPrj, "DER", 255);
			DerElem.Locked = true;

			m_DepDir = m_ptDerPrj.M;
			m_DepAzt = m_ptDerGeo.M;

			double fTmp = NativeMethods.Modulus(m_DepAzt - GlobalVars.m_CurrADHP.MagVar);
			textBox0002.Text = Functions.Degree2String(fTmp, Degree2StringMode.DMS);
			textBox0003.Text = Functions.Degree2String(m_DepAzt, Degree2StringMode.DMS);

			textBox0004.Text = UnitConverter.HeightToDisplayUnits(m_ptDerPrj.Z, eRoundMode.NERAEST).ToString();
			comboBox0004_SelectedIndexChanged(comboBox0004, new EventArgs());
			comboBox0005_SelectedIndexChanged(comboBox0005, new EventArgs());

			// =====================================================================
			IPolyline pLine = (IPolyline)new Polyline();

			pLine.FromPoint = m_ptDerPrj;
			pLine.ToPoint = Functions.LocalToPrj(m_ptDerPrj, m_DepDir, 2 * GlobalVars.RModel + 1000.0, 0.0);

			ITopologicalOperator2 pTopo = (ITopologicalOperator2)m_pCircle;

			FirstLeg.BestCase.pNominalPoly = (IPolyline)pTopo.Intersect(pLine, esriGeometryDimension.esriGeometry1Dimension);
			if (Functions.ReturnDistanceInMeters(FirstLeg.BestCase.pNominalPoly.FromPoint, m_ptDerPrj) > 1.0)
				FirstLeg.BestCase.pNominalPoly.ReverseOrientation();

			FirstLeg.WorstCase.pNominalPoly = FirstLeg.BestCase.pNominalPoly;


			// =====================================================================
			Graphics.DeleteElement(pNominalElem);

			pNominalElem = Graphics.DrawPolyline(FirstLeg.BestCase.pNominalPoly, Functions.RGB(0, 0, 255), 1);
			pNominalElem.Locked = true;
		}

		private void comboBox0003_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void comboBox0004_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox0004.SelectedIndex == 0)
				textBox0005.Text = UnitConverter.HeightToM(GlobalVars.m_SelectedRWY.TORA, eRoundMode.NERAEST).ToString();
			else
				textBox0005.Text = UnitConverter.HeightToFt(GlobalVars.m_SelectedRWY.TORA, eRoundMode.NERAEST).ToString();
		}

		private void comboBox0005_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox0005.SelectedIndex == 0)
				textBox0006.Text = UnitConverter.HeightToM(GlobalVars.m_SelectedRWY.TODA, eRoundMode.NERAEST).ToString();
			else
				textBox0006.Text = UnitConverter.HeightToFt(GlobalVars.m_SelectedRWY.TODA, eRoundMode.NERAEST).ToString();
		}

		private void textBox0001_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox0001_Validating(textBox0001, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox0001.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox0001_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox0001.Text, out fTmp))
			{
				if (double.TryParse((string)textBox0001.Tag, out fTmp))
					textBox0001.Text = (string)textBox0001.Tag;
				else
					textBox0001.Text = UnitConverter.DistanceToDisplayUnits(GlobalVars.RModel, eRoundMode.CEIL).ToString();

				return;
			}

			if (textBox0001.Tag != null && textBox0001.Tag.ToString() == textBox0001.Text)
				return;

			double NewR = UnitConverter.DistanceToInternalUnits(fTmp);

			if (NewR < GlobalVars.RMin)
				NewR = GlobalVars.RMin;
			if (NewR > GlobalVars.RMax)
				NewR = GlobalVars.RMax;

			GlobalVars.RModel = NewR;

			textBox0001.Text = UnitConverter.DistanceToDisplayUnits(NewR, eRoundMode.CEIL).ToString();
			textBox0001.Tag = textBox0001.Text;

			// =====================================================================
			m_pCircle = Functions.CreateCirclePrj(m_ptCenter, GlobalVars.RModel);

			Graphics.DeleteElement(pCircleElem);

			IRgbColor pRGB = new RgbColor();
			pRGB.RGB = 255;

			ILineSymbol pLineSimbol = new SimpleLineSymbol();
			pLineSimbol.Color = pRGB;
			pLineSimbol.Width = 2;

			ISimpleFillSymbol pEmptyFillSym = new SimpleFillSymbol();
			pEmptyFillSym.Color = pRGB;
			pEmptyFillSym.Style = esriSimpleFillStyle.esriSFSNull;
			pEmptyFillSym.Outline = pLineSimbol;

			pCircleElem = Graphics.DrawPolygonSFS(m_pCircle, pEmptyFillSym);
		}

		private void textBox0007_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox0007_Validating(textBox0007, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox0007.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox0007_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox0007.Text, out fTmp))
			{
				if (double.TryParse((string)textBox0007.Tag, out fTmp))
					textBox0007.Text = (string)textBox0007.Tag;
				else
					textBox0007.Text = UnitConverter.DistanceToDisplayUnits(GlobalVars.heightAboveDER, eRoundMode.CEIL).ToString();

				return;
			}

			if (textBox0007.Tag != null && textBox0007.Tag.ToString() == textBox0007.Text)
				return;

			double NewHeight = UnitConverter.HeightToInternalUnits(fTmp);

			if (NewHeight < GlobalVars.heightAboveDERMin)
				NewHeight = GlobalVars.heightAboveDERMin;
			if (NewHeight > GlobalVars.heightAboveDERMax)
				NewHeight = GlobalVars.heightAboveDERMax;

			GlobalVars.heightAboveDER = NewHeight;

			textBox0007.Text = UnitConverter.HeightToDisplayUnits(NewHeight, eRoundMode.CEIL).ToString();
			textBox0007.Tag = textBox0007.Text;
		}

		#endregion

		#region Page II

		void AddvanceToPageIII()
		{
			FirstLeg.WorstCase.NetGrd = WorstMaxNetGrad2;
			FirstLeg.BestCase.NetGrd = BestMaxNetGrad2;

			m_minAcceleDist = (GlobalVars.minAccelerationHeight - GlobalVars.heightAboveDERMax) / WorstMaxNetGrad2;
			m_AcceleDist = m_minAcceleDist;
			m_AcceleDistLeg = -1;

			m_AcselertedPhaseNetGradient = minAcselertedPhaseNetGradient;
			textBox0203.Text = UnitConverter.GradientToDisplayUnits(m_AcselertedPhaseNetGradient, eRoundMode.NERAEST).ToString();

			FirstLeg.PlannedEndWidth = GlobalVars.MandatorySemiWidth;

			FirstLeg.ptStart.BestCase.pPoint = m_ptDerPrj;
			FirstLeg.ptStart.BestCase.Direction = m_DepDir;
			FirstLeg.ptStart.BestCase.Width = GlobalVars.BaseSemiWidth;

			FirstLeg.ptStart.WorstCase.pPoint = m_ptDerPrj;
			FirstLeg.ptStart.WorstCase.Direction = m_DepDir;
			FirstLeg.ptStart.WorstCase.Width = GlobalVars.BaseSemiWidth;

			FirstLeg.ptEnd.BestCase.Direction = m_DepDir;
			FirstLeg.ptEnd.BestCase.pPoint = FirstLeg.BestCase.pNominalPoly.ToPoint;
			FirstLeg.ptEnd.BestCase.Width = FirstLeg.PlannedEndWidth;

			FirstLeg.ptEnd.WorstCase.Direction = m_DepDir;
			FirstLeg.ptEnd.WorstCase.pPoint = FirstLeg.WorstCase.pNominalPoly.ToPoint;
			FirstLeg.ptEnd.WorstCase.Width = FirstLeg.PlannedEndWidth;

			double splayEnd = (FirstLeg.PlannedEndWidth - FirstLeg.ptStart.BestCase.Width) / GlobalVars.SplayRate;

			IPoint pt1 = Functions.LocalToPrj(FirstLeg.ptStart.BestCase.pPoint, FirstLeg.ptStart.BestCase.Direction, 0.0, FirstLeg.ptStart.BestCase.Width);
			IPoint pt6 = Functions.LocalToPrj(FirstLeg.ptStart.BestCase.pPoint, FirstLeg.ptStart.BestCase.Direction, 0.0, -FirstLeg.ptStart.BestCase.Width);

			IPoint pt2 = Functions.LocalToPrj(FirstLeg.ptStart.BestCase.pPoint, FirstLeg.ptStart.BestCase.Direction, splayEnd, FirstLeg.ptEnd.BestCase.Width);
			IPoint pt5 = Functions.LocalToPrj(FirstLeg.ptStart.BestCase.pPoint, FirstLeg.ptStart.BestCase.Direction, splayEnd, -FirstLeg.ptEnd.BestCase.Width);

			IPoint pt3 = Functions.LocalToPrj(FirstLeg.ptStart.BestCase.pPoint, FirstLeg.ptStart.BestCase.Direction, 2.0 * GlobalVars.RModel, FirstLeg.PlannedEndWidth);
			IPoint pt4 = Functions.LocalToPrj(FirstLeg.ptStart.BestCase.pPoint, FirstLeg.ptStart.BestCase.Direction, 2.0 * GlobalVars.RModel, -FirstLeg.PlannedEndWidth);

			IPolygon pTmpPoly = (IPolygon)new Polygon();

			pTmpPoly = (IPolygon)new Polygon();

			IPointCollection ptColl = (IPointCollection)pTmpPoly;
			ptColl.AddPoint(pt1);
			ptColl.AddPoint(pt2);

			ptColl.AddPoint(pt3);
			ptColl.AddPoint(pt4);

			ptColl.AddPoint(pt5);
			ptColl.AddPoint(pt6);

			ptColl.AddPoint(pt1);

			ITopologicalOperator2 pTopo = (ITopologicalOperator2)pTmpPoly;
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();

			FirstLeg.pProtectionArea = (IPolygon)pTopo.Intersect(m_pCircle, esriGeometryDimension.esriGeometry2Dimension);

			pTopo = (ITopologicalOperator2)FirstLeg.pProtectionArea;
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();

			Graphics.DeleteElement(protectionAreaElem);
			protectionAreaElem = Graphics.DrawPolygon(FirstLeg.pProtectionArea, Functions.RGB(0, 255, 0));

			Functions.GetLegObstacles(AllObsstacles, ref FirstLeg);

			HaveReport = true;
			ReportsFrm.FillPage1(FirstLeg);


			//if (FirstLeg.ObsMaxNetGrd >= 0)
			//{
			//    textBox0201.Text = UnitConverter.GradientToDisplayUnits(FirstLeg.ObstacleList[FirstLeg.ObsMaxNetGrd].ReqNetGradient, eRoundMode.CEIL).ToString();
			//    textBox0202.Text = FirstLeg.ObstacleList[FirstLeg.ObsMaxNetGrd].ID;
			//    textBox0204.Text = UnitConverter.DistanceToDisplayUnits(FirstLeg.ObstacleList[FirstLeg.ObsMaxNetGrd].X, eRoundMode.NERAEST).ToString();
			//}
			//else
			//{
			//    textBox0201.Text = (0.0).ToString("f1");// "";
			//    textBox0202.Text = "";
			//    textBox0204.Text = "";
			//}
		}

		private void comboBox0101_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox0101.SelectedIndex < 0)
				return;

			if (comboBox0003.SelectedIndex == 1)
			{
				minAircraftSpeed2 = GlobalVars.v2_AN124[comboBox0101.SelectedIndex];	// km/hour
				minAircraftSpeed4 = GlobalVars.v4_AN124[comboBox0101.SelectedIndex];	// km/hour
			}
			else
			{
				minAircraftSpeed2 = GlobalVars.v2_IL76[comboBox0101.SelectedIndex];	// km/hour
				minAircraftSpeed4 = GlobalVars.v4_IL76[comboBox0101.SelectedIndex];	// km/hour
			}

			textBox0101.Text = UnitConverter.SpeedToDisplayUnits(minAircraftSpeed2, eRoundMode.NERAEST).ToString();
			textBox0102.Text = UnitConverter.SpeedToDisplayUnits(minAircraftSpeed4, eRoundMode.NERAEST).ToString();

			if (comboBox0102.SelectedIndex > comboBox0101.SelectedIndex)
				comboBox0102.SelectedIndex = comboBox0101.SelectedIndex;
		}

		private void comboBox0102_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox0102.SelectedIndex < 0)
				return;

			if (comboBox0003.SelectedIndex == 1)
			{
				maxAircraftSpeed2 = GlobalVars.v2_AN124[comboBox0102.SelectedIndex];	// km/hour
				maxAircraftSpeed4 = GlobalVars.v4_AN124[comboBox0102.SelectedIndex];	// km/hour
			}
			else
			{
				maxAircraftSpeed2 = GlobalVars.v2_IL76[comboBox0102.SelectedIndex];	// km/hour
				maxAircraftSpeed4 = GlobalVars.v4_IL76[comboBox0102.SelectedIndex];	// km/hour
			}

			textBox0105.Text = UnitConverter.SpeedToDisplayUnits(maxAircraftSpeed2, eRoundMode.NERAEST).ToString();
			textBox0106.Text = UnitConverter.SpeedToDisplayUnits(maxAircraftSpeed4, eRoundMode.NERAEST).ToString();

			if (comboBox0101.SelectedIndex < comboBox0102.SelectedIndex)
				comboBox0101.SelectedIndex = comboBox0102.SelectedIndex;
		}

		private void textBox0103_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox0103_Validating(textBox0103, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox0103.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox0103_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox0103.Text, out fTmp))
			{
				if (double.TryParse((string)textBox0103.Tag, out fTmp))
					textBox0103.Text = (string)textBox0103.Tag;
				else
					textBox0103.Text = UnitConverter.GradientToDisplayUnits(WorstMaxNetGrad2, eRoundMode.NERAEST).ToString();

				return;
			}

			if (textBox0103.Tag != null && textBox0103.Tag.ToString() == textBox0103.Text)
				return;

			double NewMaxGrad2 = UnitConverter.GradientToInternalUnits(fTmp);

			if (NewMaxGrad2 < 0.01)
				NewMaxGrad2 = 0.01;
			if (NewMaxGrad2 > 0.1)
				NewMaxGrad2 = 0.1;

			WorstMaxNetGrad2 = NewMaxGrad2;

			textBox0103.Text = UnitConverter.GradientToDisplayUnits(WorstMaxNetGrad2, eRoundMode.NERAEST).ToString();
			textBox0103.Tag = textBox0103.Text;

			if (BestMaxNetGrad2 < WorstMaxNetGrad2 + 0.01)
			{
				textBox0107.Text = UnitConverter.GradientToDisplayUnits(WorstMaxNetGrad2 + 0.01, eRoundMode.NERAEST).ToString();
				textBox0107_Validating(textBox0107, new CancelEventArgs());
			}
		}

		private void textBox0104_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox0104_Validating(textBox0104, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox0104.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox0104_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox0104.Text, out fTmp))
			{
				if (double.TryParse((string)textBox0104.Tag, out fTmp))
					textBox0104.Text = (string)textBox0104.Tag;
				else
					textBox0104.Text = UnitConverter.GradientToDisplayUnits(WorstMaxNetGrad4, eRoundMode.NERAEST).ToString();

				return;
			}

			if (textBox0104.Tag != null && textBox0104.Tag.ToString() == textBox0104.Text)
				return;

			double NewMaxGrad4 = UnitConverter.GradientToInternalUnits(fTmp);

			if (NewMaxGrad4 < 0.005)
				NewMaxGrad4 = 0.005;
			if (NewMaxGrad4 > 0.05)
				NewMaxGrad4 = 0.05;

			WorstMaxNetGrad4 = NewMaxGrad4;

			textBox0104.Text = UnitConverter.GradientToDisplayUnits(WorstMaxNetGrad4, eRoundMode.NERAEST).ToString();
			textBox0104.Tag = textBox0104.Text;

			if (BestMaxNetGrad4 < WorstMaxNetGrad4 + 0.005)
			{
				textBox0108.Text = UnitConverter.GradientToDisplayUnits(WorstMaxNetGrad4 + 0.005, eRoundMode.NERAEST).ToString();
				textBox0108_Validating(textBox0108, new CancelEventArgs());
			}
		}

		private void textBox0107_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox0107_Validating(textBox0107, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox0107.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox0107_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox0107.Text, out fTmp))
			{
				if (double.TryParse((string)textBox0107.Tag, out fTmp))
					textBox0107.Text = (string)textBox0107.Tag;
				else
					textBox0107.Text = UnitConverter.GradientToDisplayUnits(BestMaxNetGrad2, eRoundMode.NERAEST).ToString();

				return;
			}

			if (textBox0107.Tag != null && textBox0107.Tag.ToString() == textBox0107.Text)
				return;

			double NewMaxGrad2 = UnitConverter.GradientToInternalUnits(fTmp);

			if (NewMaxGrad2 < 0.01)
				NewMaxGrad2 = 0.01;
			if (NewMaxGrad2 > 0.1)
				NewMaxGrad2 = 0.1;

			BestMaxNetGrad2 = NewMaxGrad2;

			textBox0107.Text = UnitConverter.GradientToDisplayUnits(BestMaxNetGrad2, eRoundMode.NERAEST).ToString();
			textBox0107.Tag = textBox0107.Text;

			if (BestMaxNetGrad2 - 0.01 < WorstMaxNetGrad2)
			{
				textBox0103.Text = UnitConverter.GradientToDisplayUnits(BestMaxNetGrad2 - 0.01, eRoundMode.NERAEST).ToString();
				textBox0103_Validating(textBox0103, new CancelEventArgs());
			}
		}

		private void textBox0108_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox0108_Validating(textBox0108, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox0108.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox0108_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox0108.Text, out fTmp))
			{
				if (double.TryParse((string)textBox0108.Tag, out fTmp))
					textBox0108.Text = (string)textBox0108.Tag;
				else
					textBox0108.Text = UnitConverter.GradientToDisplayUnits(BestMaxNetGrad4, eRoundMode.NERAEST).ToString();

				return;
			}

			if (textBox0108.Tag != null && textBox0108.Tag.ToString() == textBox0108.Text)
				return;

			double NewMaxGrad4 = UnitConverter.GradientToInternalUnits(fTmp);

			if (NewMaxGrad4 < 0.005)
				NewMaxGrad4 = 0.005;
			if (NewMaxGrad4 > 0.05)
				NewMaxGrad4 = 0.05;

			BestMaxNetGrad4 = NewMaxGrad4;

			textBox0108.Text = UnitConverter.GradientToDisplayUnits(BestMaxNetGrad4, eRoundMode.NERAEST).ToString();
			textBox0108.Tag = textBox0108.Text;

			if (BestMaxNetGrad4 - 0.005 < WorstMaxNetGrad4)
			{
				textBox0104.Text = UnitConverter.GradientToDisplayUnits(BestMaxNetGrad4 - 0.005, eRoundMode.NERAEST).ToString();
				textBox0104_Validating(textBox0104, new CancelEventArgs());
			}
		}

		#endregion

		#region Page III

		private void textBox0203_KeyPress(object sender, KeyPressEventArgs e)
		{
			//char eventChar = e.KeyChar;

			//if (e.KeyChar == 13)
			//    textBox0203_Validating(textBox0203, new System.ComponentModel.CancelEventArgs());
			//else
			//    Functions.TextBoxFloat(ref eventChar, textBox0203.Text);

			//e.KeyChar = eventChar;
			//if (e.KeyChar == 0)
			//    e.Handled = true;
		}

		private void textBox0203_Validating(object sender, CancelEventArgs e)
		{
			//double fTmp;
			//if (!double.TryParse(textBox0203.Text, out fTmp))
			//{
			//    if (double.TryParse((string)textBox0203.Tag, out fTmp))
			//        textBox0203.Text = (string)textBox0203.Tag;
			//    else
			//        textBox0203.Text = UnitConverter.GradientToDisplayUnits(m_AcselertedPhaseNetGradient, eRoundMode.NERAEST).ToString();

			//    return;
			//}

			//fTmp = UnitConverter.GradientToInternalUnits(fTmp);
			//m_AcselertedPhaseNetGradient = fTmp;

			//if (m_AcselertedPhaseNetGradient < minAcselertedPhaseNetGradient)
			//    m_AcselertedPhaseNetGradient = minAcselertedPhaseNetGradient;
			//else if (m_AcselertedPhaseNetGradient > maxAcselertedPhaseNetGradient)
			//    m_AcselertedPhaseNetGradient = maxAcselertedPhaseNetGradient;

			//if (m_AcselertedPhaseNetGradient != fTmp)
			//    textBox0203.Text = UnitConverter.GradientToDisplayUnits(m_AcselertedPhaseNetGradient, eRoundMode.NERAEST).ToString();
		}

		#endregion

		#region Page IV

		void AddvanceToPageIV()
		{
			LegCount = 0;

			WorstMaxGrossGrad = WorstMaxNetGrad2 + 0.01;
			BestMaxGrossGrad = BestMaxNetGrad2 + 0.01;

			m_BankAngle = minBankAngle;
			m_TailWind = minTailWind;
			m_HeadWind = minHeadWind;

			//SplayAngle = Functions.RadToDeg(Math.Atan(GlobalVars.SplayRate));

			textBox0302.Text = UnitConverter.SpeedToDisplayUnits(m_TailWind, eRoundMode.NERAEST).ToString();
			textBox0303.Text = UnitConverter.SpeedToDisplayUnits(m_HeadWind, eRoundMode.NERAEST).ToString();
			textBox0305.Text = m_BankAngle.ToString();

			m_MagVar = GlobalVars.m_CurrADHP.MagVar;
			m_CurrAzt = m_DepAzt;

			m_GrossHFinish = PANS_OPS_DataBase.dpGui_Ar1.Value;//m_ptDerPrj.Z + 

			m_CurrPnt.BestCase.pPoint = m_ptDerPrj;
			m_CurrPnt.BestCase.Direction = m_DepDir;
			m_CurrPnt.BestCase.Width = GlobalVars.BaseSemiWidth;
			m_CurrPnt.BestCase.GrossHeight = GlobalVars.heightAboveDERMax;	//m_ptDerPrj.Z + 
			m_CurrPnt.BestCase.NetHeight = m_CurrPnt.BestCase.GrossHeight;

			m_CurrPnt.WorstCase.pPoint = m_ptDerPrj;
			m_CurrPnt.WorstCase.Direction = m_DepDir;
			m_CurrPnt.WorstCase.Width = GlobalVars.BaseSemiWidth;
			m_CurrPnt.WorstCase.GrossHeight = m_CurrPnt.BestCase.GrossHeight;
			m_CurrPnt.WorstCase.NetHeight = m_CurrPnt.BestCase.GrossHeight;

			m_CurrPnt.BestCase.pPoint.M = m_CurrPnt.BestCase.Direction;
			m_CurrPnt.WorstCase.pPoint.M = m_CurrPnt.WorstCase.Direction;

			PrevSegment.SegmentCode = eLegType.NONE;
			PrevSegment.GuidanceNav.TypeCode = eNavaidClass.CodeNONE;
			PrevSegment.GuidanceNav.TypeCode = eNavaidClass.CodeNONE;

			m_SegType = eLegType.straight;
			CreateBtn.Enabled = true;

			CurrSegment = CreateNextSegment(m_CurrPnt);

			CurrSegment.ptStart = m_CurrPnt;
			CurrSegment.BestCase.PrevTotalLength = 0.0;
			CurrSegment.BestCase.PrevTotalFlightTime = 0.0;

			CurrSegment.WorstCase.PrevTotalLength = 0.0;
			CurrSegment.WorstCase.PrevTotalFlightTime = 0.0;

			CurrSegment.PlannedEndWidth = UnitConverter.HeightToInternalUnits((double)numericUpDown0301.Value);
			//CurrSegment.PlannedEndWidth = GlobalVars.MandatorySemiWidth;

			textBox0306.Text = UnitConverter.DistanceToDisplayUnits(m_AcceleDist, eRoundMode.NERAEST).ToString();
		}

		private void radioButton0301_CheckedChanged(object sender, EventArgs e)
		{
			if (((RadioButton)sender).Checked)
				m_SegType = (eLegType)(Convert.ToByte(((RadioButton)sender).Tag) + 1);
		}

		private void radioButton0301_Click(object sender, EventArgs e)
		{
			if (((RadioButton)sender).Checked)
				m_SegType = (eLegType)(Convert.ToByte(((RadioButton)sender).Tag) + 1);
		}

		private void comboBox0301_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox0301.SelectedIndex == 0)
				textBox0301.Text = UnitConverter.HeightToDisplayUnits(m_CurrPnt.WorstCase.NetHeight + m_ptDerPrj.Z, eRoundMode.NERAEST).ToString();
			else
				textBox0301.Text = UnitConverter.HeightToDisplayUnits(m_CurrPnt.WorstCase.NetHeight, eRoundMode.NERAEST).ToString();
		}

		private void textBox0302_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;

			if (e.KeyChar == 13)
				textBox0302_Validating(textBox0302, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox0302.Text);

			e.KeyChar = eventChar;
			if (e.KeyChar == 0)
				e.Handled = true;
		}

		private void textBox0302_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;
			if (!double.TryParse(textBox0302.Text, out fTmp))
				return;
			double fTmp1;

			fTmp1 = fTmp;

			if (fTmp1 < minTailWind)
				fTmp1 = minTailWind;
			else if (fTmp1 > maxTailWind)
				fTmp1 = maxTailWind;

			if (fTmp1 != fTmp)
				textBox0302.Text = fTmp1.ToString();

			m_TailWind = UnitConverter.SpeedToInternalUnits(fTmp1);
		}

		private void textBox0303_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;

			if (e.KeyChar == 13)
				textBox0303_Validating(textBox0303, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox0303.Text);

			e.KeyChar = eventChar;
			if (e.KeyChar == 0)
				e.Handled = true;
		}

		private void textBox0303_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;
			if (!double.TryParse(textBox0303.Text, out fTmp))
				return;
			double fTmp1;

			fTmp1 = fTmp;

			if (fTmp1 < minHeadWind)
				fTmp1 = minHeadWind;
			else if (fTmp1 > maxHeadWind)
				fTmp1 = maxHeadWind;

			if (fTmp1 != fTmp)
				textBox0303.Text = fTmp1.ToString();

			m_HeadWind = UnitConverter.SpeedToInternalUnits(fTmp1);
		}

		//private void textBox0304_KeyPress(object sender, KeyPressEventArgs e)
		//{
		//    char eventChar = e.KeyChar;

		//    if (e.KeyChar == 13)
		//        textBox0304_Validating(textBox0304, new System.ComponentModel.CancelEventArgs());
		//    else
		//        Functions.TextBoxFloat(ref eventChar, textBox0304.Text);

		//    e.KeyChar = eventChar;
		//    if (e.KeyChar == 0)
		//        e.Handled = true;
		//}

		//private void textBox0304_Validating(object sender, CancelEventArgs e)
		//{
		//    double fTmp;
		//    if (!double.TryParse(textBox0304.Text, out fTmp))
		//        return;
		//    double fTmp1;

		//    fTmp1 = fTmp;

		//    if (fTmp1 < minAircraftSpeed)
		//        fTmp1 = minAircraftSpeed;
		//    else if (fTmp1 > maxAircraftSpeed)
		//        fTmp1 = maxAircraftSpeed;

		//    if (fTmp1 != fTmp)
		//        textBox0304.Text = fTmp1.ToString();

		//    m_IAS = UnitConverter.SpeedToInternalUnits(fTmp1);
		//}

		private void textBox0305_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;

			if (e.KeyChar == 13)
				textBox0305_Validating(textBox0305, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox0305.Text);

			e.KeyChar = eventChar;
			if (e.KeyChar == 0)
				e.Handled = true;
		}

		private void textBox0305_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;
			if (!double.TryParse(textBox0305.Text, out fTmp))
				return;
			double fTmp1;

			fTmp1 = fTmp;

			if (fTmp1 < minBankAngle)
				fTmp1 = minBankAngle;
			else if (fTmp1 > maxBankAngle)
				fTmp1 = maxBankAngle;

			m_BankAngle = fTmp1;
			if (fTmp1 != fTmp)
				textBox0305.Text = fTmp1.ToString();
		}

		#endregion

		#region Page V

		public TrackLeg CreateNextSegment(LegPoint CurrPt)
		{
			m_CurrAzt = NativeMethods.Modulus((Functions.Dir2Azt(CurrPt.BestCase.pPoint, CurrPt.BestCase.Direction)));
			textBox0308.Text = System.Math.Round(NativeMethods.Modulus(m_CurrAzt - m_MagVar), 2).ToString();

			if (comboBox0301.SelectedIndex < 0)
				comboBox0301.SelectedIndex = 0;
			else
				comboBox0301_SelectedIndexChanged(comboBox0301, new System.EventArgs());

			if (PrevSegment.SegmentCode == eLegType.NONE)
			{
				radioButton0301.Enabled = true;
				radioButton0301.Checked = true;
				m_SegType = eLegType.straight;

				radioButton0302.Enabled = false;
				radioButton0303.Enabled = false;
				radioButton0304.Enabled = false;
				radioButton0305.Enabled = false;
				radioButton0306.Enabled = false;
				radioButton0307.Enabled = false;
			}
			else if (PrevSegment.SegmentCode == eLegType.arcIntercept)
			{
				radioButton0301.Enabled = false;
				radioButton0302.Enabled = false;
				radioButton0303.Enabled = false;
				radioButton0304.Enabled = false;
				radioButton0305.Enabled = false;
				radioButton0306.Enabled = false;

				radioButton0307.Enabled = true;
				radioButton0307.Checked = true;
				m_SegType = eLegType.arcPath;
			}
			else if (CurrPt.atHeight)
			{
				radioButton0301.Enabled = true;		//false;
				radioButton0302.Enabled = true;
				//m_SegType = eLegType.toHeading;

				radioButton0303.Enabled = FillComboBox301Stations() > 0;//false;	// 
				radioButton0304.Enabled = FillComboBox401Stations() > 0;
				radioButton0305.Enabled = FillComboBox501Stations() > 0;
				radioButton0306.Enabled = false;	// FillComboBox603DMEStations() > 0;
				radioButton0307.Enabled = false;
			}
			else
			{
				double Delta = Functions.SubtractAngles(CurrPt.BestCase.Direction, CurrPt.WorstCase.Direction);
				double AngleTreshold = 5.0;

				if (PrevSegment.SegmentCode == eLegType.directToFIX)
					AngleTreshold = 25.0;

				radioButton0301.Enabled = Delta < AngleTreshold;
				radioButton0302.Enabled = true;

				if (PrevSegment.SegmentCode == eLegType.arcPath)
				{
					if (radioButton0301.Enabled)
						radioButton0301.Checked = true;
					else
						radioButton0302.Checked = true;
				}

				radioButton0303.Enabled = FillComboBox301Stations() > 0;
				radioButton0304.Enabled = FillComboBox401Stations() > 0;
				radioButton0305.Enabled = FillComboBox501Stations() > 0;

				double x, y;
				Functions.PrjToLocal(CurrPt.BestCase.pPoint, CurrPt.BestCase.Direction, CurrPt.WorstCase.pPoint, out x, out y);

				if (Delta < GlobalVars.degEps && Math.Abs(y) < 5.0)
					radioButton0306.Enabled = FillComboBox603DMEStations() > 0;
				else
					radioButton0306.Enabled = false;

				radioButton0307.Enabled = false;
			}

			TrackLeg result = new TrackLeg();
			result.ptStart = CurrPt;
			result.SegmentCode = m_SegType;
			return result;
		}

		LegData[] Cases = new LegData[0];
		ObstacleData[][] CasesObsstacles;

		internal void CreateTurnAndInterceptProtectionArea(ref TrackLeg segment)
		{
			int i, n;

			int turnDir = (ComboBox502.SelectedIndex << 1) - 1;

			double fuzzyDist = Functions.ReturnDistanceInMeters(segment.ptStart.BestCase.pPoint, segment.ptStart.WorstCase.pPoint);
			double fuzzyDir = Functions.ReturnAngleInDegrees(segment.ptStart.BestCase.pPoint, segment.ptStart.WorstCase.pPoint);
			double step = 550.0;

			n = (int)Math.Ceiling(fuzzyDist / step);
			if (n < 5) n = 5;

			double invN = 1.0 / n;

			step = fuzzyDist * invN;

			double dStGross = (segment.ptStart.WorstCase.GrossHeight - segment.ptStart.BestCase.GrossHeight) * invN;
			double dStNet = (segment.ptStart.WorstCase.NetHeight - segment.ptStart.BestCase.NetHeight) * invN;

			double EndDist = Functions.ReturnDistanceInMeters(segment.ptEnd.WorstCase.pPoint, segment.ptEnd.BestCase.pPoint);
			double EndDir = Functions.ReturnAngleInDegrees(segment.ptEnd.WorstCase.pPoint, segment.ptEnd.BestCase.pPoint);
			double EndStep = EndDist * invN;

			double dEndGross = (segment.ptEnd.WorstCase.GrossHeight - segment.ptEnd.BestCase.GrossHeight) * invN;
			double dEndNet = (segment.ptEnd.WorstCase.NetHeight - segment.ptEnd.BestCase.NetHeight) * invN;
			double dNetGrd = (segment.WorstCase.NetGrd - segment.BestCase.NetGrd) * invN;
			double dSpeed = (minAircraftSpeed2 - maxAircraftSpeed2) * invN;

			double dDir = NativeMethods.Modulus(segment.ptStart.WorstCase.Direction - segment.ptStart.BestCase.Direction);// *invN;
			if (dDir > 180.0)
				dDir = (dDir - 360.0) * invN;
			else
				dDir *= invN;

			IPointCollection ptColl;
			ITopologicalOperator2 pTopo;
			IPolygon pTmpPoly;

			Cases = new LegData[n + 1];
			IPolygon[] src = new IPolygon[n + 1];
			PointData[] ptStartCases = new PointData[n];
			PointData[] ptEndCases = new PointData[n];
			CasesObsstacles = new ObstacleData[n + 1][];

			double fTAS, fTurnR, deadReck;
			IPointCollection MyPC;

			IPointCollection pHeadPolyPtColl = (IPointCollection)new Polygon();
			pHeadPolyPtColl.AddPoint(Functions.LocalToPrj(segment.ptEnd.BestCase.pPoint, segment.ptEnd.BestCase.Direction, 0, segment.ptEnd.BestCase.Width));
			pHeadPolyPtColl.AddPoint(Functions.LocalToPrj(segment.ptEnd.BestCase.pPoint, segment.ptEnd.BestCase.Direction, 0, -segment.ptEnd.BestCase.Width));

			IPolygon pHeadPoly = null;

			IPoint ptPrevEnd = segment.ptEnd.BestCase.pPoint;
			IPoint ptCurrEnd;

			for (i = 1; i < n; i++)
			{
				Cases[i].Initialize();
				ptStartCases[i].pPoint = Functions.LocalToPrj(segment.ptStart.BestCase.pPoint, fuzzyDir, step * i);
				ptStartCases[i].Direction = segment.ptStart.BestCase.Direction + dDir * i;
				ptStartCases[i].Width = segment.ptStart.BestCase.Width;
				ptStartCases[i].GrossHeight = segment.ptStart.BestCase.GrossHeight + dStGross * i;
				ptStartCases[i].NetHeight = segment.ptStart.BestCase.NetHeight + dStNet * i;
				double fIAS = maxAircraftSpeed2 + dSpeed * i;
				fTAS = Functions.IAS2TAS(fIAS, ptStartCases[i].GrossHeight + m_ptDerPrj.Z, GlobalVars.m_CurrADHP.ISAtC);
				fTurnR = Functions.Bank2Radius(m_BankAngle, fTAS);

				MyPC = Functions.CalcTouchByFixDir(ref Cases[i], ptStartCases[i], Type5_CurNav.pPtPrj, fTurnR, Type5_CurDir, turnDir, Type5_SnapAngle, out deadReck);
				Cases[i].pNominalPoly = Functions.CalcTrajectoryFromMultiPoint(MyPC);
				ptCurrEnd = MyPC.Point[MyPC.PointCount - 1];

				//Graphics.DrawPolyline(Cases[i].pNominalPoly);
				//Functions.ProcessMessages();

				double fDist = Functions.ReturnDistanceInMeters(ptPrevEnd, ptCurrEnd);
				if (fDist > 1.2 * EndStep)
				{
					pHeadPolyPtColl.AddPoint(Functions.LocalToPrj(ptPrevEnd, segment.ptEnd.BestCase.Direction, 0, -segment.ptEnd.BestCase.Width));
					pHeadPolyPtColl.AddPoint(Functions.LocalToPrj(ptPrevEnd, segment.ptEnd.BestCase.Direction, 0, segment.ptEnd.BestCase.Width));

					pTopo = (ITopologicalOperator2)pHeadPolyPtColl;
					pTopo.IsKnownSimple_2 = false;
					pTopo.Simplify();

					if (pHeadPolyPtColl.PointCount > 3)
					{
						if (pHeadPoly == null)
							pHeadPoly = (IPolygon)pHeadPolyPtColl;
						else
						{
							pHeadPoly = (IPolygon)pTopo.Union(pHeadPoly);
							pTopo = (ITopologicalOperator2)pHeadPoly;
							pTopo.IsKnownSimple_2 = false;
							pTopo.Simplify();
						}
					}

					pHeadPolyPtColl = (IPointCollection)new Polygon();
					pHeadPolyPtColl.AddPoint(Functions.LocalToPrj(ptCurrEnd, segment.ptEnd.BestCase.Direction, 0, segment.ptEnd.BestCase.Width));
					pHeadPolyPtColl.AddPoint(Functions.LocalToPrj(ptCurrEnd, segment.ptEnd.BestCase.Direction, 0, -segment.ptEnd.BestCase.Width));

				}
				//EndDist -= fDist;	//?????
				EndDist = Functions.ReturnDistanceInMeters(segment.ptEnd.WorstCase.pPoint, ptCurrEnd);
				EndStep = EndDist / (n - i);

				IPointCollection ptCollLeft = (IPointCollection)new Polyline();
				IPointCollection ptCollRight = (IPointCollection)new Polyline();

				double currWidth;

				ptEndCases[i].pPoint = ptCurrEnd;		//Functions.LocalToPrj(segment.ptEnd.BestCase.pPoint, EndDir, EndStep * i);
				ptEndCases[i].Direction = segment.ptEnd.BestCase.Direction;

				ptEndCases[i].Width = segment.ptEnd.BestCase.Width;
				ptEndCases[i].GrossHeight = segment.ptEnd.BestCase.GrossHeight + dEndGross * i;
				ptEndCases[i].NetHeight = segment.ptEnd.BestCase.NetHeight + dEndNet * i;

				Functions.CreateTurnAndInterceptSideLine(Cases[i], ptStartCases[i], ptEndCases[i], segment.PlannedEndWidth, ref ptCollLeft, out currWidth, SideDirection.rightSide);
				Functions.CreateTurnAndInterceptSideLine(Cases[i], ptStartCases[i], ptEndCases[i], segment.PlannedEndWidth, ref ptCollRight, out currWidth, SideDirection.leftSide);
				//1=========================================================
				((IPolyline)ptCollRight).ReverseOrientation();

				pTmpPoly = (IPolygon)new Polygon();
				ptColl = (IPointCollection)pTmpPoly;
				ptColl.AddPointCollection(ptCollLeft);
				ptColl.AddPointCollection(ptCollRight);
				pTmpPoly.Close();

				pTopo = (ITopologicalOperator2)pTmpPoly;
				pTopo.IsKnownSimple_2 = false;
				pTopo.Simplify();
				src[i] = pTmpPoly;

				ptPrevEnd = ptCurrEnd;
				double NetGrd = segment.BestCase.NetGrd + dNetGrd * i;
				Functions.GetLegObstacles3Turn(AllObsstacles, NetGrd, segment.BestCase.PrevTotalLength + i * step, pTmpPoly, Cases[i].pNominalPoly, MyPC, ref CasesObsstacles[i]);
			}

			pHeadPolyPtColl.AddPoint(Functions.LocalToPrj(segment.ptEnd.WorstCase.pPoint, segment.ptEnd.WorstCase.Direction, 0, -segment.ptEnd.WorstCase.Width));
			pHeadPolyPtColl.AddPoint(Functions.LocalToPrj(segment.ptEnd.WorstCase.pPoint, segment.ptEnd.WorstCase.Direction, 0, segment.ptEnd.WorstCase.Width));
			pTopo = (ITopologicalOperator2)pHeadPolyPtColl;
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();

			if (pHeadPolyPtColl.PointCount > 3)
			{
				if (pHeadPoly == null)
					pHeadPoly = (IPolygon)pHeadPolyPtColl;
				else
				{
					pHeadPoly = (IPolygon)pTopo.Union(pHeadPoly);
					pTopo = (ITopologicalOperator2)pHeadPoly;
					pTopo.IsKnownSimple_2 = false;
					pTopo.Simplify();
				}
			}

			//Graphics.DrawPolygon(pHeadPolyPtColl, -1);
			//Graphics.DrawPolygon(pHeadPoly, -1);
			//Functions.ProcessMessages();

			//1=========================================================
			IPointCollection ptCollBestLeft = (IPointCollection)new Polyline();
			IPointCollection ptCollBestRight = (IPointCollection)new Polyline();

			Functions.CreateSideLine(ref segment, ref ptCollBestLeft, true, SideDirection.rightSide);
			Functions.CreateSideLine(ref segment, ref ptCollBestRight, true, SideDirection.leftSide);
			((IPolyline)ptCollBestRight).ReverseOrientation();

			pTmpPoly = (IPolygon)new Polygon();
			ptColl = (IPointCollection)pTmpPoly;
			ptColl.AddPointCollection(ptCollBestLeft);
			ptColl.AddPointCollection(ptCollBestRight);
			pTmpPoly.Close();
			pTopo = (ITopologicalOperator2)pTmpPoly;
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();
			src[0] = pTmpPoly;
			//Cases[0].pNominalElement = Graphics.DrawPolygon(pTmpPoly, -1);

			//==================================================================================================
			fTAS = Functions.IAS2TAS(maxAircraftSpeed2, m_CurrPnt.BestCase.GrossHeight + m_ptDerPrj.Z, GlobalVars.m_CurrADHP.ISAtC);
			fTurnR = Functions.Bank2Radius(m_BankAngle, fTAS);
			MyPC = Functions.CalcTouchByFixDir(ref segment.BestCase, m_CurrPnt.BestCase, Type5_CurNav.pPtPrj, fTurnR, Type5_CurDir, turnDir, Type5_SnapAngle, out deadReck);

			Functions.GetLegObstacles3Turn(AllObsstacles, segment.BestCase.NetGrd, segment.BestCase.PrevTotalLength, pTmpPoly, segment.BestCase.pNominalPoly, MyPC, ref CasesObsstacles[0]);
			//2=========================================================
			IPointCollection ptCollWorstLeft = (IPointCollection)new Polyline();
			IPointCollection ptCollWorstRight = (IPointCollection)new Polyline();

			Functions.CreateSideLine(ref segment, ref ptCollWorstLeft, false, SideDirection.rightSide);
			Functions.CreateSideLine(ref segment, ref ptCollWorstRight, false, SideDirection.leftSide);
			((IPolyline)ptCollWorstRight).ReverseOrientation();

			pTmpPoly = (IPolygon)new Polygon();
			ptColl = (IPointCollection)pTmpPoly;
			ptColl.AddPointCollection(ptCollWorstLeft);
			ptColl.AddPointCollection(ptCollWorstRight);
			pTmpPoly.Close();
			pTopo = (ITopologicalOperator2)pTmpPoly;
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();
			src[n] = pTmpPoly;
			//Cases[n].pNominalElement = Graphics.DrawPolygon(pTmpPoly, -1);

			//---------------------------------
			fTAS = Functions.IAS2TAS(minAircraftSpeed2, m_CurrPnt.WorstCase.GrossHeight + m_ptDerPrj.Z, GlobalVars.m_CurrADHP.ISAtC);
			fTurnR = Functions.Bank2Radius(m_BankAngle, fTAS);
			MyPC = Functions.CalcTouchByFixDir(ref segment.WorstCase, m_CurrPnt.WorstCase, Type5_CurNav.pPtPrj, fTurnR, Type5_CurDir, turnDir, Type5_SnapAngle, out deadReck);

			Functions.GetLegObstacles3Turn(AllObsstacles, segment.WorstCase.NetGrd, segment.WorstCase.PrevTotalLength, pTmpPoly, segment.WorstCase.pNominalPoly, MyPC, ref CasesObsstacles[n]);
			//3=========================================================

			IPointCollection ptBasePoly = (IPointCollection)new Polygon();
			ptBasePoly.AddPoint(Functions.LocalToPrj(m_CurrPnt.BestCase.pPoint, m_CurrPnt.BestCase.Direction, 0, m_CurrPnt.BestCase.Width));
			ptBasePoly.AddPoint(Functions.LocalToPrj(m_CurrPnt.BestCase.pPoint, m_CurrPnt.BestCase.Direction, 0, -m_CurrPnt.BestCase.Width));

			ptBasePoly.AddPoint(Functions.LocalToPrj(m_CurrPnt.WorstCase.pPoint, m_CurrPnt.WorstCase.Direction, 0, -m_CurrPnt.WorstCase.Width));
			ptBasePoly.AddPoint(Functions.LocalToPrj(m_CurrPnt.WorstCase.pPoint, m_CurrPnt.WorstCase.Direction, 0, m_CurrPnt.WorstCase.Width));

			pTopo = (ITopologicalOperator2)ptBasePoly;
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();

			if (pHeadPoly != null)
			{
				ptBasePoly = (IPointCollection)pTopo.Union(pHeadPoly);
				pTopo = (ITopologicalOperator2)ptBasePoly;
				pTopo.IsKnownSimple_2 = false;
				pTopo.Simplify();
			}

			CurrSegment.pProtectionArea = (IPolygon)ptBasePoly;

			int k = -1, m = AllObsstacles.Length;
			CurrSegment.ObstacleList = new ObstacleData[m];
			CurrSegment.ObsMaxNetGrd = -1;
			CurrSegment.ObsMaxAcceleDist = -1;

			for (i = 0; i <= n; i++)
			{
				//============================================================
				//Cases[i].pNominalElement = Graphics.DrawPolygon(src[i], -1);
				//============================================================

				pTopo = (ITopologicalOperator2)CurrSegment.pProtectionArea;
				pTmpPoly = (IPolygon)pTopo.Union(src[i]);

				pTopo = (ITopologicalOperator2)pTmpPoly;
				pTopo.IsKnownSimple_2 = false;
				pTopo.Simplify();
				CurrSegment.pProtectionArea = pTmpPoly;

				m = CasesObsstacles[i].Length;

				for (int j = 0; j < m; j++)
				{
					//if (CasesObsstacles[i][j].ID == "Ter-9115" || CasesObsstacles[i][j].ID == "Ter-9116")
					//    Functions.ProcessMessages();

					//if (CasesObsstacles[i][j].ID == "Ter-1831")
					//{
					//    Functions.ProcessMessages();
					//}

					int found = -1, compare = -1;
					for (int l = k; l >= 0; l--)
					{
						if (CurrSegment.ObstacleList[l].ID == CasesObsstacles[i][j].ID)
						{
							found = l;
							break;
						}
					}

					if (found >= 0)
					{
						if (CurrSegment.ObstacleList[found].ApplNetGradient2 > CasesObsstacles[i][j].ApplNetGradient2)
						{
							CurrSegment.ObstacleList[found] = CasesObsstacles[i][j];
							compare = found;
						}
					}
					else
					{
						k++;
						CurrSegment.ObstacleList[k] = CasesObsstacles[i][j];
						compare = k;
					}


					if (compare >= 0)
					{
						if (CurrSegment.ObsMaxNetGrd < 0)
							CurrSegment.ObsMaxNetGrd = compare;
						else if (CurrSegment.ObstacleList[compare].ReqNetGradient > CurrSegment.ObstacleList[CurrSegment.ObsMaxNetGrd].ReqNetGradient)
							CurrSegment.ObsMaxNetGrd = compare;

						if (CurrSegment.ObsMaxAcceleDist < 0)
							CurrSegment.ObsMaxAcceleDist = compare;
						else if (CurrSegment.ObstacleList[compare].AcceleStartDist > CurrSegment.ObstacleList[CurrSegment.ObsMaxAcceleDist].AcceleStartDist)
							CurrSegment.ObsMaxAcceleDist = compare;
					}
				}
			}
			//Functions.ProcessMessages();

			Array.Resize<ObstacleData>(ref CurrSegment.ObstacleList, k + 1);
			//pTmpPoly = (IPolygon)new Polygon();
		}

		private void ConstructNextSegment()
		{
			CurrSegment.pProtectionArea = null;
			CurrSegment.Initialize();

			ClearSegmentDrawings(false);
			bool Assigned = false;

			switch (m_SegType)
			{
				case eLegType.straight:
					//if (OptionButton101.Checked)
					//	Assigned = Type1SegmentOnDistance(Type1_CurDist, ref CurrSegment);
					//else
					if (OptionButton102.Checked)
						Assigned = Type1SegmentOnWpt(ref CurrSegment);
					else if (OptionButton103.Checked)
						Assigned = Type1SegmentAtHeight(Type1_CurHeight, ref CurrSegment);
					else if (OptionButton104.Checked)
						Assigned = Type1SegmentOnNewFIX(Type1_CurNavaid, Type1_CurDir, ref CurrSegment);
					break;
				case eLegType.toHeading:
					Assigned = Type2Segment(Type2_OutDir, ref CurrSegment);
					break;
				case eLegType.courseIntercept:
					Assigned = Type3Segment(ref CurrSegment);
					break;
				case eLegType.directToFIX:
					Assigned = Type4Segment(ref CurrSegment);
					break;
				case eLegType.turnAndIntercept:
					Assigned = Type5Segment(ref CurrSegment);
					break;
				case eLegType.arcIntercept:
					Assigned = Type6Segment(ref CurrSegment);
					break;
				case eLegType.arcPath:
					//									PrevSegment
					Assigned = Type7Segment(Type7_CurNav, LegList[LegCount - 1], ref CurrSegment);
					break;
			}

			if (Assigned)
			{
				CurrSegment.BestCase.NetGrd = BestMaxNetGrad2;
				CurrSegment.WorstCase.NetGrd = WorstMaxNetGrad2;
				CurrSegment.BankAngle = m_BankAngle;

				double fTAS = Functions.IAS2TAS(maxAircraftSpeed2, m_CurrPnt.BestCase.GrossHeight + m_ptDerPrj.Z, GlobalVars.m_CurrADHP.ISAtC);
				CurrSegment.BestCase.FlightTime = 0.06 * CurrSegment.BestCase.Length / fTAS;

				if (m_SegType == eLegType.turnAndIntercept)
				{
					//CurrSegment.ObstacleList = new ObstacleData[0];
					//CurrSegment.ObsMaxNetGrd = -1;
					//CurrSegment.pProtectionArea = (IPolygon)new Polygon();

					CreateTurnAndInterceptProtectionArea(ref CurrSegment);
				}
				else
				{
					Functions.CreateProtectionArea(ref CurrSegment);
					Functions.GetLegObstacles(AllObsstacles, ref CurrSegment);
				}

				pCurrProtectAreaElem = Graphics.DrawPolygon(CurrSegment.pProtectionArea, Functions.RGB(0, 0, 255));
				pCurrWorstNominalTrackElem = Graphics.DrawPolyline(CurrSegment.WorstCase.pNominalPoly, Functions.RGB(127, 127, 127), 3);
				pCurrBestNominalTrackElem = Graphics.DrawPolyline(CurrSegment.BestCase.pNominalPoly, Functions.RGB(255, 0, 255));

				ReportsFrm.FillCurrentLeg(CurrSegment);

				bool enable = true;
				for (int i = 0; i < CurrSegment.ObstacleList.Length; i++)
					if (CurrSegment.ObstacleList[i].ReqNetGradient > CurrSegment.ObstacleList[i].ApplNetGradient2)
					{
						enable = false;
						break;
					}

				AddSegmentBtn.Enabled = enable;

				if (!enable)
					MessageBox.Show("Требуемый препятствием градиент превышает установленное значение!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			GlobalVars.GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, null, null);
		}

		private void ClearSegmentDrawings(bool bRefresh = true)
		{
			Graphics.DeleteElement(pCurrBestNominalTrackElem);
			pCurrBestNominalTrackElem = null;

			Graphics.DeleteElement(pCurrWorstNominalTrackElem);
			pCurrWorstNominalTrackElem = null;

			Graphics.DeleteElement(pCurrProtectAreaElem);
			pCurrProtectAreaElem = null;

			if (bRefresh)
				GlobalVars.GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, null, null);
		}

		private void CreateBtn_Click(object sender, EventArgs e)
		{
			ClearSegmentDrawings(true);

			CurrSegment.PlannedEndWidth = UnitConverter.HeightToInternalUnits((double)numericUpDown0301.Value);// 9000;		//
			if (CurrSegment.ptStart.BestCase.Width > CurrSegment.PlannedEndWidth)
				CurrSegment.PlannedEndWidth = CurrSegment.ptStart.BestCase.Width;
			CurrSegment.SegmentCode = m_SegType;

			PrevBtn.Enabled = false;
			NextBtn.Enabled = false;
			CreateBtn.Enabled = false;
			RemoveSegmentBtn.Enabled = false;

			AddSegmentBtn.Enabled = true;
			ReturnBtn.Enabled = true;

			switch (m_SegType)
			{
				case eLegType.straight:
					doStraight();
					break;
				case eLegType.toHeading:
					doToHeading();
					break;
				case eLegType.courseIntercept:
					doCourseIntercept();
					break;
				case eLegType.directToFIX:
					doDirectToFIX();
					break;
				case eLegType.turnAndIntercept:
					doTurnAndIntercept();
					break;
				case eLegType.arcIntercept:
					doArcIntercept();
					break;
				case eLegType.arcPath:
					doArcPath();
					break;
			}

			NavLabels[2].Text = MultiPage1.SelectedTab.Text;
			FocusStepCaption(2);
		}

		#endregion

		#region Straight

		private void doStraight()
		{
			OptionButton102.Enabled = FillComboBox101Stations() > 0;
			OptionButton104.Enabled = FillComboBox102Stations() > 0;
			OptionButton103.Enabled = LegCount == 0 || !PrevSegment.ptEnd.atHeight;

			if (OptionButton103.Enabled)
			{
				if (OptionButton103.Checked)
					OptionButton103_CheckedChanged(OptionButton103, new System.EventArgs());
				else
					OptionButton103.Checked = true;
			}
			else if (OptionButton102.Enabled)
			{
				if (OptionButton102.Checked)
					OptionButton102_CheckedChanged(OptionButton102, new System.EventArgs());
				else
					OptionButton102.Checked = true;
			}
			else if (OptionButton104.Enabled)
			{
				if (OptionButton104.Checked)
					OptionButton104_CheckedChanged(OptionButton104, new System.EventArgs());
				else
					OptionButton104.Checked = true;
			}
			else
			{
				OptionButton103.Checked = false;
				OptionButton102.Checked = false;
				OptionButton103.Checked = false;

				ComboBox104.Enabled = false;
				TextBox103.Enabled = false;
				label108.Enabled = false;

				ComboBox101.Enabled = false;

				ComboBox102.Enabled = false;
				ComboBox103.Enabled = false;
				TextBox102.Enabled = false;
				label104.Enabled = false;

				label106.Visible = false;
				label107.Visible = false;
			}

			MultiPage1.SelectedIndex = 3;
		}

		private int FillComboBox101Stations()
		{
			ComboBox101.Items.Clear();
			int n = GlobalVars.WPTList.Length;

			int j = 0;

			for (int i = 0; i < n; i++)
			{
				bool Type1PossibleWpt = (Functions.SideDef(m_CurrPnt.BestCase.pPoint, m_CurrPnt.BestCase.Direction + 90.0, GlobalVars.WPTList[i].pPtPrj) < SideDirection.onSide) &&
										(Math.Abs(Functions.PointToLineDistance(GlobalVars.WPTList[i].pPtPrj, m_CurrPnt.BestCase.pPoint, m_CurrPnt.BestCase.Direction)) < WPTFarness) &&
										(Functions.ReturnDistanceInMeters(m_CurrPnt.BestCase.pPoint, GlobalVars.WPTList[i].pPtPrj) > 1);

				if (Type1PossibleWpt)
				{
					ComboBox101.Items.Add(GlobalVars.WPTList[i]);
					j++;
				}
			}

			return j;
		}

		private int FillComboBox102Stations()
		{
			ComboBox102.Items.Clear();

			int n = GlobalVars.NavaidList.Length;
			int m = GlobalVars.DMEList.Length;

			int j = 0;
			ComboBox102List = new NavaidData[n + m + 1];

			double minInterceptAngle = 30.0;
			double maxInterceptAngle = 180.0 - minInterceptAngle;

			double VORConDist = m_CurrPnt.BestCase.NetHeight * Math.Tan(Functions.DegToRad(Navaids_DataBase.VOR.ConeAngle));
			double NDBConDist = m_CurrPnt.BestCase.NetHeight * Math.Tan(Functions.DegToRad(Navaids_DataBase.NDB.ConeAngle));

			for (int i = 0; i < n; i++)
			{
				double navX, navY, ConDist;
				double interLeft, interRight;

				IPoint pPtNav = GlobalVars.NavaidList[i].pPtPrj;
				Functions.PrjToLocal(m_CurrPnt.BestCase.pPoint, m_CurrPnt.BestCase.Direction, pPtNav, out navX, out navY);

				if (GlobalVars.NavaidList[i].TypeCode == eNavaidClass.CodeVOR || GlobalVars.NavaidList[i].TypeCode == eNavaidClass.CodeTACAN)
					ConDist = VORConDist;
				else
					ConDist = NDBConDist;

				double dH = Math.Abs(m_CurrPnt.BestCase.NetHeight - pPtNav.Z);
				double MaxDist = 4130.0 * Math.Sqrt(dH);

				//DrawPointWithText pPtNav, "SL"
				//DrawPointWithText m_CurrPnt, "CurrPnt"
				//DrawPointWithText PointAlongPlane(m_CurrPnt.Pnt , m_CurrPnt.Dir + 90#, navY), "navY"

				if (Math.Abs(navY) < ConDist) continue;
				if (Math.Abs(navY) > MaxDist) continue;

				if (navX < -0.5 * MaxDist) continue;
				if (navX > MaxDist) continue;

				double fTmp = NativeMethods.Modulus(Functions.RadToDeg(0.5 * Math.PI - Math.Atan(navX / navY)), 180.0);

				if (navY > 0.0)
				{
					interLeft = Math.Max(fTmp, minInterceptAngle);
					if (interLeft > maxInterceptAngle) continue;
					interLeft = interLeft + 180.0;
					interRight = maxInterceptAngle + 180.0;
				}
				else
				{
					interRight = Math.Min(fTmp, maxInterceptAngle);
					if (interRight < minInterceptAngle) continue;
					interLeft = minInterceptAngle;
				}

				ComboBox102List[j] = GlobalVars.NavaidList[i];
				ComboBox102List[j].ValMax = new double[1];
				ComboBox102List[j].ValMin = new double[1];

				ComboBox102List[j].ValMin[0] = interLeft;
				ComboBox102List[j].ValMax[0] = interRight;
				ComboBox102.Items.Add(ComboBox102List[j]);

				j++;
			}

			double cotan23 = 1 / System.Math.Tan(GlobalVars.DegToRadValue * PANS_OPS_DataBase.dpTP_by_DME_div.Value);
			double cotan55 = 1 / System.Math.Tan(GlobalVars.DegToRadValue * Navaids_DataBase.DME.SlantAngle);

			for (int i = 0; i < m; i++)
			{
				double navX, navY;

				IPoint pPtNav = GlobalVars.DMEList[i].pPtPrj;
				Functions.PrjToLocal(m_CurrPnt.BestCase.pPoint, m_CurrPnt.BestCase.Direction, pPtNav, out navX, out navY);
				navY = Math.Abs(navY);

				double navL = navY * cotan23;
				double Y55 = (m_CurrPnt.BestCase.NetHeight - pPtNav.Z) * cotan55;

				if (Y55 > navY)
				{
					double L55 = Math.Sqrt(Y55 * Y55 - navY * navY);
					navL = Math.Max(navL, L55);
				}

				Interval IntRange, Int23;

				double dH = Math.Abs(m_CurrPnt.BestCase.NetHeight - pPtNav.Z);
				double MaxDist = 4130.0 * Math.Sqrt(dH);

				IntRange.Left = navX - Navaids_DataBase.DME.Range;
				IntRange.Right = navX + Navaids_DataBase.DME.Range;
				if (IntRange.Left < cMinDist) IntRange.Left = cMinDist;
				if (IntRange.Right > MaxDist) IntRange.Right = MaxDist;

				Int23.Left = navX - navL;
				Int23.Right = navX + navL;

				Interval[] IntrRes = Functions.IntervalsDifference(IntRange, Int23);
				int l = IntrRes.Length;

				//			Interval[] IntrRes1;
				//			For ii = 0 To L
				//            IntrRes2 = IntervalsDifference(IntrRes(ii), Intr55)
				//            If UBound(IntrRes1) < 0 Then
				//                IntrRes1 = IntrRes2
				//            Else
				//                ll = UBound(IntrRes1)
				//                mm = UBound(IntrRes2)
				//                If mm >= 0 Then
				//                    ReDim Preserve IntrRes1(ll + mm + 1)

				//                    For jj = 0 To mm
				//                        IntrRes1(jj + ll + 1) = IntrRes2(jj)
				//                    Next jj
				//                End If
				//            End If
				//        Next ii

				//        IntrRes = IntrRes1

				//        n = UBound(IntrRes)

				int ii = 0, jj;
				if (l > 0)
					do
					{
						if (IntrRes[ii].Left == IntrRes[ii].Right)
						{
							for (jj = ii; jj < l - 1; jj++)
								IntrRes[jj] = IntrRes[jj + 1];

							l--;
						}
						else
							ii++;

					} while (ii < l - 1);

				ii = 0;
				while (ii < l - 1)
				{
					if (IntrRes[ii].Right == IntrRes[ii + 1].Left)
					{
						IntrRes[ii].Right = IntrRes[ii + 1].Right;
						for (jj = ii + 1; jj < l - 1; jj++)
							IntrRes[jj] = IntrRes[jj + 1];

						l--;
					}
					else
						ii++;
				}

				if (l < 1)
					continue;

				Interval[] IntrRes1 = new Interval[l];
				int ll = -1;
				int kk = 0;

				for (ii = 0; ii < l; ii++)
				{
					//            ptFarD = PointAlongPlane(ptBase, NomDir, IntrRes(ii).Right)

					//            pCutter.FromPoint = PointAlongPlane(ptNearD, NomDir - 90#, RModel)
					//            pCutter.ToPoint = PointAlongPlane(ptNearD, NomDir + 90#, RModel)

					//            KKhMinDME = pTopoOper.Intersect(pCutter, esriGeometry1Dimension)

					//            If SideDef(ptNearD, NomDir, KKhMinDME.FromPoint) < 0 Then KKhMinDME.ReverseOrientation

					//            pCutter.FromPoint = PointAlongPlane(ptFarD, NomDir - 90#, RModel)
					//            pCutter.ToPoint = PointAlongPlane(ptFarD, NomDir + 90#, RModel)

					//            KKhMaxDME = pTopoOper.Intersect(pCutter, esriGeometry1Dimension)

					//            If SideDef(ptFarD, NomDir, KKhMaxDME.FromPoint) < 0 Then KKhMaxDME.ReverseOrientation
					//dpOv_Nav_PDG.Value
					//            IntrRes1(M) = CalcDMERange(ptBase, dpH_abv_DER.Value, ptBase.Z, NomDir, PDG, ptFNavPrj, KKhMinDME, KKhMaxDME)
					//            If IntrRes1(ll).Left < IntrRes1(M).Right Then
					//                ll = ll + 1
					//                ValidNavs(j).ValCnt = SideDef(KKhMinDME.FromPoint, NomDir + 90#, ptFNavPrj)
					//            End If

					Int23.Left = IntrRes[ii].Left - navX;
					Int23.Right = IntrRes[ii].Right - navX;

					//IntrRes1(ll).Left = IntrRes(ii).Left - navX
					//IntrRes1(ll).Right = IntrRes(ii).Right - navX

					if (Int23.Left < Int23.Right)
					{
						ll++;

						if (Int23.Left < 0)
						{
							kk = -1;
							IntrRes1[ll].Right = Math.Abs(Int23.Left);
							IntrRes1[ll].Left = Math.Abs(Int23.Right);
						}
						else
						{
							kk = 1;
							IntrRes1[ll].Left = Int23.Left;
							IntrRes1[ll].Right = Int23.Right;
						}

						//Set ptNearD = PointAlongPlane(m_CurrPnt, m_CurrDir, IntrRes(ii).Left)
						//kk = SideDef(KKhMinDME.FromPoint, m_CurrDir + 90#, pPtNav)
						//ComboBox102List(j).ValCnt = SideDef(KKhMinDME.FromPoint, NomDir + 90#, ptFNavPrj)
					}
				}

				if (ll < 0) continue;
				if (ll > 0) kk = 0;

				//if( IntrRes1[0].Right < IntrRes1[0].Left) continue;

				ComboBox102List[j] = GlobalVars.DMEList[i];

				ComboBox102List[j].ValCnt = kk;
				ComboBox102List[j].ValMax = new double[ll + 1];
				ComboBox102List[j].ValMin = new double[ll + 1];

				for (ii = 0; ii <= ll; ii++)
				{
					ComboBox102List[j].ValMin[ii] = IntrRes1[ii].Left;
					ComboBox102List[j].ValMax[ii] = IntrRes1[ii].Right;
				}

				ComboBox102.Items.Add(ComboBox102List[j]);

				j++;

				//if (ComboBox102List[j].Name = "" )
				//    ComboBox102.Items.Add(ComboBox102List[j].CallSign);
				//else
				//    ComboBox102.Items.Add(ComboBox102List[j].Name);

			}

			Array.Resize<NavaidData>(ref ComboBox102List, j);
			return j;
		}

		private bool Type1SegmentOnDistance(double Dist, ref TrackLeg Segment)
		{
			Segment.ptStart = m_CurrPnt;

			Segment.BestCase.turns = 0;
			Segment.WorstCase.turns = 0;

			IPoint ptBestEnd = Functions.LocalToPrj(m_CurrPnt.BestCase.pPoint, m_CurrPnt.BestCase.Direction, Dist, 0.0);
			Segment.ptEnd.BestCase.pPoint = ptBestEnd;
			Segment.ptEnd.BestCase.Direction = m_CurrPnt.BestCase.Direction;
			ptBestEnd.M = m_CurrPnt.BestCase.Direction;

			Segment.BestCase.Length = Dist;
			Segment.BestCase.pNominalPoly = (IPolyline)new ESRI.ArcGIS.Geometry.Polyline();
			Segment.BestCase.pNominalPoly.FromPoint = m_CurrPnt.BestCase.pPoint;
			Segment.BestCase.pNominalPoly.ToPoint = ptBestEnd;

			IPoint ptWorstEnd = Functions.LocalToPrj(m_CurrPnt.WorstCase.pPoint, m_CurrPnt.WorstCase.Direction, Dist, 0.0);
			Segment.ptEnd.WorstCase.pPoint = ptWorstEnd;
			Segment.ptEnd.WorstCase.Direction = m_CurrPnt.WorstCase.Direction;
			ptWorstEnd.M = m_CurrPnt.WorstCase.Direction;

			Segment.WorstCase.Length = Dist;
			Segment.WorstCase.pNominalPoly = (IPolyline)new ESRI.ArcGIS.Geometry.Polyline();
			Segment.WorstCase.pNominalPoly.FromPoint = m_CurrPnt.WorstCase.pPoint;
			Segment.WorstCase.pNominalPoly.ToPoint = ptWorstEnd;

			Segment.ptEnd.atHeight = false;

			Segment.ptEnd.BestCase.NetHeight = Math.Min(m_CurrPnt.BestCase.NetHeight + Segment.BestCase.Length * BestMaxNetGrad2, cMaxH);
			Segment.ptEnd.BestCase.GrossHeight = Math.Min(m_CurrPnt.BestCase.GrossHeight + Segment.BestCase.Length * BestMaxGrossGrad, cMaxH);

			Segment.ptEnd.WorstCase.NetHeight = Math.Min(m_CurrPnt.WorstCase.NetHeight + Segment.WorstCase.Length * WorstMaxNetGrad2, cMaxH);
			Segment.ptEnd.WorstCase.GrossHeight = Math.Min(m_CurrPnt.WorstCase.GrossHeight + Segment.WorstCase.Length * WorstMaxGrossGrad, cMaxH);

			Segment.Comment = "Прямой сегмент до высоты " + UnitConverter.HeightToDisplayUnits(CurrSegment.ptEnd.BestCase.NetHeight, eRoundMode.NERAEST) + " " + UnitConverter.HeightUnit;
			//Segment.RepComment = Segment.Comment;

			Segment.bOnWPTFIX = false;
			Segment.SegmentCode = eLegType.straight;

			if (PrevSegment.SegmentCode == eLegType.arcPath)
				Segment.GuidanceNav = PrevSegment.InterceptionNav;
			else
				Segment.GuidanceNav = PrevSegment.GuidanceNav;

			Segment.InterceptionNav.TypeCode = eNavaidClass.CodeNONE;
			//public Navaid InterceptionNav;

			if (PrevSegment.SegmentCode == eLegType.courseIntercept || PrevSegment.SegmentCode == eLegType.directToFIX || PrevSegment.SegmentCode == eLegType.turnAndIntercept)
				Segment.PathCode = SegmentPathType.CA;
			else
				Segment.PathCode = SegmentPathType.VA;

			return true;
		}

		private bool Type1SegmentOnWpt(ref TrackLeg Segment)
		{
			Segment.ptStart = m_CurrPnt;

			Segment.BestCase.turns = 0;
			Segment.WorstCase.turns = 0;
			WPT_FIXData WPtFix = (WPT_FIXData)ComboBox101.SelectedItem;
			ESRI.ArcGIS.esriSystem.IClone pClone = (ESRI.ArcGIS.esriSystem.IClone)WPtFix.pPtPrj;

			IPoint ptEnd = (IPoint)pClone.Clone();
			ptEnd.M = m_CurrPnt.BestCase.Direction;
			Segment.ptEnd.BestCase.pPoint = ptEnd;
			Segment.ptEnd.BestCase.Direction = m_CurrPnt.BestCase.Direction;

			Segment.BestCase.Length = Functions.ReturnDistanceInMeters(m_CurrPnt.BestCase.pPoint, ptEnd);
			Segment.BestCase.pNominalPoly = (IPolyline)new ESRI.ArcGIS.Geometry.Polyline();
			Segment.BestCase.pNominalPoly.FromPoint = m_CurrPnt.BestCase.pPoint;
			Segment.BestCase.pNominalPoly.ToPoint = ptEnd;
			Segment.BestCase.NetGrd = BestMaxNetGrad2;

			ptEnd = (IPoint)pClone.Clone();
			ptEnd.M = m_CurrPnt.WorstCase.Direction;
			Segment.ptEnd.WorstCase.pPoint = ptEnd;
			Segment.ptEnd.WorstCase.Direction = m_CurrPnt.WorstCase.Direction;

			Segment.WorstCase.Length = Functions.ReturnDistanceInMeters(m_CurrPnt.WorstCase.pPoint, ptEnd);
			Segment.WorstCase.pNominalPoly = (IPolyline)new ESRI.ArcGIS.Geometry.Polyline();
			Segment.WorstCase.pNominalPoly.FromPoint = m_CurrPnt.WorstCase.pPoint;
			Segment.WorstCase.pNominalPoly.ToPoint = ptEnd;
			Segment.WorstCase.NetGrd = WorstMaxNetGrad2;

			Segment.ptEnd.atHeight = false;

			Segment.ptEnd.BestCase.NetHeight = Math.Min(m_CurrPnt.BestCase.NetHeight + Segment.BestCase.Length * BestMaxNetGrad2, cMaxH);
			Segment.ptEnd.BestCase.GrossHeight = Math.Min(m_CurrPnt.BestCase.GrossHeight + Segment.BestCase.Length * BestMaxGrossGrad, cMaxH);

			Segment.ptEnd.WorstCase.NetHeight = Math.Min(m_CurrPnt.WorstCase.NetHeight + Segment.WorstCase.Length * WorstMaxNetGrad2, cMaxH);
			Segment.ptEnd.WorstCase.GrossHeight = Math.Min(m_CurrPnt.WorstCase.GrossHeight + Segment.WorstCase.Length * WorstMaxGrossGrad, cMaxH);

			TextBox101.Text = UnitConverter.DistanceToDisplayUnits(Segment.BestCase.Length, eRoundMode.FLOOR).ToString();

			double azt = NativeMethods.Modulus(m_CurrAzt - GlobalVars.m_CurrADHP.MagVar);
			string aztStr = Math.Round(azt).ToString();

			while (aztStr.Length < 3)
				aztStr = "0" + aztStr;
			aztStr = aztStr + "°";

			Segment.Comment = "До " + ComboBox101.SelectedItem + " с курсом " + aztStr;
			label109.Text = Segment.Comment;

			Segment.bOnWPTFIX = true;
			Segment.WptFix = WPtFix;

			if (PrevSegment.SegmentCode == eLegType.arcPath)
				Segment.GuidanceNav = PrevSegment.InterceptionNav;
			else
				Segment.GuidanceNav = PrevSegment.GuidanceNav;

			if (WPtFix.TypeCode != eNavaidClass.CodeNONE)
			{
				Segment.InterceptionNav = Navaids_DataBase.WPT_FIXToNavaid(WPtFix);
				Segment.InterceptionNav.Tag = -1;
				Segment.GuidanceNav = Segment.InterceptionNav;	//
			}
			else
				Segment.InterceptionNav.TypeCode = WPtFix.TypeCode;

			Segment.SegmentCode = eLegType.straight;
			Segment.PathCode = SegmentPathType.CF;
			return true;
		}

		private bool Type1SegmentAtHeight(double AltHeight, ref TrackLeg Segment)
		{
			Segment.ptStart = m_CurrPnt;

			Segment.BestCase.turns = 0;
			Segment.WorstCase.turns = 0;

			double Dist1 = (AltHeight - m_CurrPnt.BestCase.NetHeight) / BestMaxNetGrad2;
			IPoint ptBestEnd = Functions.LocalToPrj(m_CurrPnt.BestCase.pPoint, m_CurrPnt.BestCase.Direction, Dist1);
			ptBestEnd.M = m_CurrPnt.BestCase.Direction;
			Segment.BestCase.Length = Dist1;
			Segment.ptEnd.BestCase.pPoint = ptBestEnd;
			Segment.ptEnd.BestCase.Direction = m_CurrPnt.BestCase.Direction;
			Segment.BestCase.NetGrd = BestMaxNetGrad2;

			Segment.BestCase.pNominalPoly = (IPolyline)new ESRI.ArcGIS.Geometry.Polyline();
			Segment.BestCase.pNominalPoly.FromPoint = m_CurrPnt.BestCase.pPoint;
			Segment.BestCase.pNominalPoly.ToPoint = ptBestEnd;

			double Dist2 = (AltHeight - m_CurrPnt.WorstCase.NetHeight) / WorstMaxNetGrad2;
			IPoint ptWorstEnd = Functions.LocalToPrj(m_CurrPnt.WorstCase.pPoint, m_CurrPnt.WorstCase.Direction, Dist2);
			ptWorstEnd.M = m_CurrPnt.WorstCase.Direction;
			Segment.WorstCase.Length = Dist2;
			Segment.ptEnd.WorstCase.pPoint = ptWorstEnd;
			Segment.ptEnd.WorstCase.Direction = m_CurrPnt.WorstCase.Direction;

			Segment.WorstCase.pNominalPoly = (IPolyline)new ESRI.ArcGIS.Geometry.Polyline();
			Segment.WorstCase.pNominalPoly.FromPoint = m_CurrPnt.WorstCase.pPoint;
			Segment.WorstCase.pNominalPoly.ToPoint = ptWorstEnd;
			Segment.WorstCase.NetGrd = WorstMaxNetGrad2;

			Segment.ptEnd.atHeight = true;

			Segment.ptEnd.BestCase.NetHeight = AltHeight;	// Math.Min(m_NetHStart + Segment.Length * ultraMaxNetGrad2, cMaxH);
			Segment.ptEnd.BestCase.GrossHeight = Math.Min(m_CurrPnt.BestCase.GrossHeight + Dist1 * BestMaxGrossGrad, cMaxH);

			Segment.ptEnd.WorstCase.NetHeight = AltHeight;	// Math.Min(m_NetHStart + Segment.Length * maxNetGrad2, cMaxH);
			Segment.ptEnd.WorstCase.GrossHeight = Math.Min(m_CurrPnt.WorstCase.GrossHeight + Dist2 * WorstMaxGrossGrad, cMaxH);

			double azt = NativeMethods.Modulus(m_CurrAzt - GlobalVars.m_CurrADHP.MagVar);
			string aztStr = Math.Round(azt).ToString();

			while (aztStr.Length < 3)
				aztStr = "0" + aztStr;
			aztStr = aztStr + "°";

			if (PrevSegment.SegmentCode == eLegType.courseIntercept || PrevSegment.SegmentCode == eLegType.turnAndIntercept)
			{
				Segment.Comment = "Набрать высоту по линии пути " + aztStr + ", на " + UnitConverter.HeightToDisplayUnits(AltHeight + m_ptDerPrj.Z, eRoundMode.NERAEST) + " " + UnitConverter.HeightUnit;
				Segment.PathCode = SegmentPathType.CA;
			}
			else if (PrevSegment.SegmentCode == eLegType.directToFIX)
				//									PrevSegment.Tag		//??????????????????
				Segment.Comment = "От " + LegList[LegCount - 1].Tag + " до " + UnitConverter.HeightToDisplayUnits(AltHeight + m_ptDerPrj.Z, eRoundMode.NERAEST) + " " + UnitConverter.HeightUnit + " по направлению " + aztStr;
			else
				Segment.Comment = "Набрать высоту по направлению " + aztStr + ", на " + UnitConverter.HeightToDisplayUnits(AltHeight + m_ptDerPrj.Z, eRoundMode.NERAEST) + " " + UnitConverter.HeightUnit;

			label109.Text = Segment.Comment;

			Segment.bOnWPTFIX = false;

			if (PrevSegment.SegmentCode == eLegType.arcPath)
				Segment.GuidanceNav = PrevSegment.InterceptionNav;
			else
				Segment.GuidanceNav = PrevSegment.GuidanceNav;

			Segment.InterceptionNav.TypeCode = eNavaidClass.CodeNONE;
			Segment.InterceptionNav.Tag = -1;

			Segment.SegmentCode = eLegType.straight;
			Segment.PathCode = SegmentPathType.VA;

			return true;
		}

		private bool Type1SegmentOnNewFIX(NavaidData Navaid, double NavDirDist, ref TrackLeg Segment)
		{
			Segment.ptStart = m_CurrPnt;

			Segment.BestCase.turns = 0;
			Segment.WorstCase.turns = 0;

			IPoint ptEnd;
			if (Navaid.TypeCode == eNavaidClass.CodeDME)
			{
				double NewAztDist;

				if (Navaid.ValCnt != 0)
					NewAztDist = 90.0 * (Navaid.ValCnt - 1);
				else
				{
					int inx = 1 - ComboBox103.SelectedIndex;
					NewAztDist = 180.0 * inx;
				}

				ptEnd = Functions.CircleVectorIntersect(Navaid.pPtPrj, NavDirDist, m_CurrPnt.BestCase.pPoint, m_CurrPnt.BestCase.Direction + NewAztDist);
			}
			else
			{
				ptEnd = new ESRI.ArcGIS.Geometry.Point();
				IConstructPoint ConstPt = (IConstructPoint)ptEnd;
				ConstPt.ConstructAngleIntersection(m_CurrPnt.BestCase.pPoint, Functions.DegToRad(m_CurrPnt.BestCase.Direction), Navaid.pPtPrj, Functions.DegToRad(NavDirDist));
			}

			if (ptEnd.IsEmpty)
				return false;

			ptEnd.M = Segment.ptStart.BestCase.Direction;
			Segment.ptEnd.BestCase.pPoint = ptEnd;

			Segment.ptEnd.BestCase.Direction = Segment.ptStart.BestCase.Direction;
			Segment.BestCase.Length = Functions.ReturnDistanceInMeters(m_CurrPnt.BestCase.pPoint, ptEnd);
			TextBox101.Text = UnitConverter.DistanceToDisplayUnits(Segment.BestCase.Length, eRoundMode.FLOOR).ToString();

			Segment.BestCase.pNominalPoly = (IPolyline)new ESRI.ArcGIS.Geometry.Polyline();
			Segment.BestCase.pNominalPoly.FromPoint = m_CurrPnt.BestCase.pPoint;
			Segment.BestCase.pNominalPoly.ToPoint = ptEnd;
			Segment.BestCase.NetGrd = BestMaxNetGrad2;

			if (Navaid.TypeCode == eNavaidClass.CodeDME)
			{
				double NewAztDist;

				if (Navaid.ValCnt != 0)
					NewAztDist = 90.0 * (Navaid.ValCnt - 1);
				else
				{
					int inx = 1 - ComboBox103.SelectedIndex;
					NewAztDist = 180.0 * inx;
				}

				ptEnd = Functions.CircleVectorIntersect(Navaid.pPtPrj, NavDirDist, m_CurrPnt.WorstCase.pPoint, m_CurrPnt.WorstCase.Direction + NewAztDist);
			}
			else
			{
				ptEnd = new ESRI.ArcGIS.Geometry.Point();
				IConstructPoint ConstPt = (IConstructPoint)ptEnd;
				ConstPt.ConstructAngleIntersection(m_CurrPnt.WorstCase.pPoint, Functions.DegToRad(m_CurrPnt.WorstCase.Direction), Navaid.pPtPrj, Functions.DegToRad(NavDirDist));
			}

			if (ptEnd.IsEmpty)
				return false;

			ptEnd.M = Segment.ptStart.WorstCase.Direction;
			Segment.ptEnd.WorstCase.pPoint = ptEnd;

			Segment.ptEnd.WorstCase.Direction = Segment.ptStart.WorstCase.Direction;
			Segment.WorstCase.Length = Functions.ReturnDistanceInMeters(m_CurrPnt.WorstCase.pPoint, ptEnd);
			//TextBox101.Text = UnitConverter.DistanceToDisplayUnits(Segment.WorstCase.Length, eRoundMode.FLOOR).ToString();

			Segment.WorstCase.pNominalPoly = (IPolyline)new ESRI.ArcGIS.Geometry.Polyline();
			Segment.WorstCase.pNominalPoly.FromPoint = m_CurrPnt.WorstCase.pPoint;
			Segment.WorstCase.pNominalPoly.ToPoint = ptEnd;
			Segment.WorstCase.NetGrd = WorstMaxNetGrad2;

			Segment.ptEnd.atHeight = false;

			Segment.ptEnd.BestCase.NetHeight = Math.Min(m_CurrPnt.BestCase.NetHeight + Segment.BestCase.Length * BestMaxNetGrad2, cMaxH);
			Segment.ptEnd.BestCase.GrossHeight = Math.Min(m_CurrPnt.BestCase.GrossHeight + Segment.BestCase.Length * BestMaxGrossGrad, cMaxH);

			Segment.ptEnd.WorstCase.NetHeight = Math.Min(m_CurrPnt.WorstCase.NetHeight + Segment.WorstCase.Length * WorstMaxNetGrad2, cMaxH);
			Segment.ptEnd.WorstCase.GrossHeight = Math.Min(m_CurrPnt.WorstCase.GrossHeight + Segment.WorstCase.Length * WorstMaxGrossGrad, cMaxH);

			string navString;
			if (Navaid.TypeCode == eNavaidClass.CodeVOR)
				navString = " до пересечения с радиалом " + TextBox102.Text + "° " + ComboBox102.SelectedItem;
			else if (Navaid.TypeCode == eNavaidClass.CodeDME)
				navString = " до пересечения с DME расстоянием " + TextBox102.Text + " " + UnitConverter.DistanceUnit + " " + ComboBox102.SelectedItem;
			else
				navString = " до пересечения с пеленгом " + TextBox102.Text + "° " + ComboBox102.SelectedItem;

			double azt = NativeMethods.Modulus(m_CurrAzt - GlobalVars.m_CurrADHP.MagVar);
			string aztStr = Math.Round(azt).ToString();

			while (aztStr.Length < 3)
				aztStr = "0" + aztStr;
			aztStr = aztStr + "°";

			if (PrevSegment.SegmentCode == eLegType.courseIntercept || PrevSegment.SegmentCode == eLegType.turnAndIntercept)
				Segment.Comment = "По линии пути " + aztStr + ", " + navString;
			else if (PrevSegment.SegmentCode == eLegType.directToFIX)
				//							PrevSegment		????????????
				Segment.Comment = "От " + LegList[LegCount - 1].Tag + " по направлению " + aztStr + navString;
			else
				Segment.Comment = "По направлению " + aztStr + navString;

			label109.Text = Segment.Comment;

			//------------------------------------------------
			Segment.bOnWPTFIX = false;
			Segment.SegmentCode = eLegType.straight;

			if (PrevSegment.SegmentCode == eLegType.arcPath)
				Segment.GuidanceNav = PrevSegment.InterceptionNav;
			else
				Segment.GuidanceNav = PrevSegment.GuidanceNav;

			Segment.InterceptionNav = Navaid;

			if (PrevSegment.SegmentCode == eLegType.courseIntercept || PrevSegment.SegmentCode == eLegType.directToFIX || PrevSegment.SegmentCode == eLegType.turnAndIntercept)
			{
				if (Navaid.TypeCode == eNavaidClass.CodeDME)
					Segment.PathCode = SegmentPathType.CD;
				else
					Segment.PathCode = SegmentPathType.CR;
			}
			else
			{
				if (Navaid.TypeCode == eNavaidClass.CodeDME)
					Segment.PathCode = SegmentPathType.VD;
				else
					Segment.PathCode = SegmentPathType.VR;
			}

			return true;
		}

		#region straight events

		private void OptionButton101_CheckedChanged(object sender, EventArgs e)
		{
			if (!((RadioButton)sender).Checked)
				return;

			TextBox101.ReadOnly = false;
			TextBox101.BackColor = System.Drawing.SystemColors.Window;

			ComboBox101.Enabled = false;
			ComboBox102.Enabled = false;
			ComboBox103.Enabled = false;
			ComboBox104.Enabled = false;
			TextBox102.Enabled = false;
			TextBox103.Enabled = false;

			label106.Visible = false;
			label107.Visible = false;

			double fTmp;
			if (!double.TryParse(TextBox101.Text, out fTmp))
				TextBox101.Text = UnitConverter.DistanceToDisplayUnits(cMinDist, eRoundMode.CEIL).ToString();

			TextBox101_Validating(TextBox101, new System.ComponentModel.CancelEventArgs());
		}

		private void OptionButton102_CheckedChanged(object sender, EventArgs e)
		{
			if (!((RadioButton)sender).Checked)
				return;

			TextBox101.ReadOnly = true;
			TextBox101.BackColor = System.Drawing.SystemColors.Control;

			ComboBox101.Enabled = true;
			ComboBox102.Enabled = false;
			ComboBox103.Enabled = false;
			ComboBox104.Enabled = false;
			TextBox102.Enabled = false;
			label104.Enabled = false;
			TextBox103.Enabled = false;

			label106.Visible = false;
			label107.Visible = false;

			if (ComboBox101.SelectedIndex < 0)
				ComboBox101.SelectedIndex = 0;
			else
				ComboBox101_SelectedIndexChanged(ComboBox101, new System.EventArgs());
		}

		private void OptionButton103_CheckedChanged(object sender, EventArgs e)
		{
			if (!((RadioButton)sender).Checked)
				return;

			TextBox101.ReadOnly = true;
			TextBox101.BackColor = System.Drawing.SystemColors.Control;

			ComboBox101.Enabled = false;
			ComboBox102.Enabled = false;
			ComboBox103.Enabled = false;
			TextBox102.Enabled = false;
			label104.Enabled = false;

			ComboBox104.Enabled = true;
			TextBox103.Enabled = true;

			label106.Visible = false;
			label107.Visible = false;

			double fTmp;

			double fMinh = Math.Max(m_CurrPnt.BestCase.NetHeight, m_CurrPnt.WorstCase.NetHeight);
			fMinh = Math.Max(fMinh, cMinH);

			if (!double.TryParse(TextBox103.Text, out fTmp))
				TextBox103.Text = UnitConverter.HeightToDisplayUnits(fMinh, eRoundMode.CEIL).ToString();

			TextBox103_Validating(TextBox103, new System.ComponentModel.CancelEventArgs());
		}

		private void OptionButton104_CheckedChanged(object sender, EventArgs e)
		{
			if (!((RadioButton)sender).Checked)
				return;

			TextBox101.ReadOnly = true;
			TextBox101.BackColor = System.Drawing.SystemColors.Control;

			ComboBox101.Enabled = false;
			ComboBox102.Enabled = true;
			ComboBox103.Enabled = true;
			ComboBox104.Enabled = false;
			TextBox102.Enabled = true;
			label104.Enabled = true;
			TextBox103.Enabled = false;

			label106.Visible = true;
			label107.Visible = true;

			if (ComboBox102.SelectedIndex < 0)
				ComboBox102.SelectedIndex = 0;
			else
				ComboBox102_SelectedIndexChanged(ComboBox102, new System.EventArgs());
		}

		private void TextBox101_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;

			if (e.KeyChar == 13)
				TextBox101_Validating(TextBox101, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, TextBox101.Text);

			e.KeyChar = eventChar;
			if (e.KeyChar == 0)
				e.Handled = true;
		}

		private void TextBox101_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox101.Text, out fTmp))
				return;

			Type1_CurDist = UnitConverter.DistanceToInternalUnits(fTmp);


			if (Type1_CurDist < cMinDist)
			{
				Type1_CurDist = cMinDist;
				TextBox101.Text = UnitConverter.DistanceToDisplayUnits(Type1_CurDist, eRoundMode.CEIL).ToString();
			}
			else if (Type1_CurDist > cMaxDist)
			{
				Type1_CurDist = cMaxDist;
				TextBox101.Text = UnitConverter.DistanceToDisplayUnits(Type1_CurDist, eRoundMode.FLOOR).ToString();
			}

			ConstructNextSegment();
		}

		private void TextBox102_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;

			if (e.KeyChar == 13)
				TextBox102_Validating(TextBox102, new System.ComponentModel.CancelEventArgs());
			else
			{
				if (Type1_CurNavaid.TypeCode == eNavaidClass.CodeDME)
					Functions.TextBoxFloat(ref eventChar, TextBox102.Text);
				else
					Functions.TextBoxInteger(ref eventChar);
			}

			e.KeyChar = eventChar;
			if (e.KeyChar == 0)
				e.Handled = true;
		}

		private void TextBox102_Validating(object sender, CancelEventArgs e)
		{
			double fTmp, NewVal;
			if (!double.TryParse(TextBox102.Text, out fTmp))
				return;

			if (Type1_CurNavaid.TypeCode != eNavaidClass.CodeDME)
			{
				NewVal = Functions.Azt2Dir(Type1_CurNavaid.pPtGeo, fTmp + m_MagVar);
				fTmp = NewVal;

				if (!Functions.AngleInInterval(NewVal, Type1_RadInterval))
				{
					if (Functions.SubtractAngles(Type1_RadInterval.Left, NewVal) < Functions.SubtractAngles(Type1_RadInterval.Right, NewVal))
						NewVal = Type1_RadInterval.Left;
					else
						NewVal = Type1_RadInterval.Right;
				}

				if (fTmp != NewVal)
					TextBox102.Text = Math.Round(NativeMethods.Modulus(Functions.Dir2Azt(Type1_CurNavaid.pPtPrj, NewVal) - m_MagVar)).ToString();
			}
			else
			{
				int inx = ComboBox103.SelectedIndex;

				NewVal = UnitConverter.DistanceToInternalUnits(fTmp);
				fTmp = NewVal;

				if (NewVal < Type1_CurNavaid.ValMin[inx])
					NewVal = Type1_CurNavaid.ValMin[inx];
				else if (NewVal > Type1_CurNavaid.ValMax[inx])
					NewVal = Type1_CurNavaid.ValMax[inx];

				if (fTmp != NewVal)
					TextBox102.Text = UnitConverter.DistanceToDisplayUnits(NewVal, eRoundMode.NERAEST).ToString();
			}

			Type1_CurDir = NewVal;
			ConstructNextSegment();
		}

		private void TextBox103_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;

			if (e.KeyChar == 13)
				TextBox103_Validating(TextBox103, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, TextBox103.Text);

			e.KeyChar = eventChar;
			if (e.KeyChar == 0)
				e.Handled = true;
		}

		private void TextBox103_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox103.Text, out fTmp))
				return;
			fTmp = UnitConverter.HeightToInternalUnits(fTmp);
			if (ComboBox104.SelectedIndex != 1)
				fTmp -= m_ptDerPrj.Z;

			Type1_CurHeight = fTmp;

			double fMinh = Math.Max(m_CurrPnt.BestCase.NetHeight, m_CurrPnt.WorstCase.NetHeight) + 5.0;
			fMinh = Math.Max(fMinh,  cMinH);

			if (Type1_CurHeight < fMinh)
			{
				Type1_CurHeight = fMinh;
				ComboBox104_SelectedIndexChanged(ComboBox104, new EventArgs());
			}
			else if (Type1_CurHeight > cMaxH)
			{
				Type1_CurHeight = cMaxH;
				ComboBox104_SelectedIndexChanged(ComboBox104, new EventArgs());
			}

			ConstructNextSegment();
		}

		private void ComboBox101_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ComboBox101.SelectedIndex < 0)
				return;

			ConstructNextSegment();
		}

		private void ComboBox104_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ComboBox104.SelectedIndex < 0)
				return;
			if (m_ptDerPrj == null)
				return;

			double fTmp = Type1_CurHeight;

			if (ComboBox104.SelectedIndex != 1)
				fTmp += m_ptDerPrj.Z;

			TextBox103.Text = UnitConverter.HeightToDisplayUnits(fTmp, eRoundMode.NERAEST).ToString();
		}

		private void ComboBox102_SelectedIndexChanged(object sender, EventArgs e)
		{
			Interval inter;

			int k = ComboBox102.SelectedIndex;
			if (k < 0)
				return;

			Type1_CurNavaid = ComboBox102List[k];
			label104.Text = Navaids_DataBase.GetNavTypeName(Type1_CurNavaid.TypeCode);

			double fTmp;

			if (Type1_CurNavaid.TypeCode != eNavaidClass.CodeDME)
			{
				ComboBox103.Enabled = false;
				label105.Text = "°";

				inter.Left = ComboBox102List[k].ValMin[0];
				inter.Right = ComboBox102List[k].ValMax[0];

				Type1_RadInterval.Left = m_CurrPnt.BestCase.Direction + inter.Left;
				Type1_RadInterval.Right = m_CurrPnt.BestCase.Direction + inter.Right;

				inter.Left = Math.Round(NativeMethods.Modulus(Functions.Dir2Azt(Type1_CurNavaid.pPtPrj, Type1_RadInterval.Right) - m_MagVar));
				inter.Right = Math.Round(NativeMethods.Modulus(Functions.Dir2Azt(Type1_CurNavaid.pPtPrj, Type1_RadInterval.Left) - m_MagVar));

				label107.Text = Functions.FormatInterval(inter);
				if (!double.TryParse(TextBox102.Text, out fTmp))
					TextBox102.Text = Math.Round(inter.Left).ToString();

				TextBox102_Validating(TextBox102, new System.ComponentModel.CancelEventArgs());
			}
			else
			{
				label105.Text = UnitConverter.DistanceUnit;

				int inx = ComboBox102List[k].ValMin.Length;
				ComboBox103.Enabled = inx > 1;

				if (inx > 1)
				{
					if (ComboBox103.SelectedIndex < 0)
						ComboBox103.SelectedIndex = 0;
					else
						ComboBox103_SelectedIndexChanged(ComboBox103, new System.EventArgs());
				}
				else
					if (ComboBox103.SelectedIndex == 0)
						//if (ComboBox103.ListIndex == 0 || ! ComboBox103.Enabled)
						//    ComboBox103.ListIndex = 0;
						ComboBox103_SelectedIndexChanged(ComboBox103, new System.EventArgs());
					else
						ComboBox103.SelectedIndex = 0;
			}
		}

		private void ComboBox103_SelectedIndexChanged(object sender, EventArgs e)
		{
			int k = ComboBox102.SelectedIndex;
			int inx = ComboBox103.SelectedIndex;

			double ValMin = ComboBox102List[k].ValMin[inx];
			double ValMax = ComboBox102List[k].ValMax[inx];

			label107.Text = "От " + UnitConverter.DistanceToDisplayUnits(ValMin, eRoundMode.NERAEST).ToString() + " " + UnitConverter.DistanceUnit +
							" " + UnitConverter.DistanceToDisplayUnits(ValMax, eRoundMode.NERAEST).ToString() + " " + UnitConverter.DistanceUnit;

			double fTmp;
			if (!double.TryParse(TextBox102.Text, out fTmp))
				TextBox102.Text = UnitConverter.DistanceToDisplayUnits(ValMin, eRoundMode.NERAEST).ToString();

			TextBox102_Validating(TextBox102, new System.ComponentModel.CancelEventArgs());
		}
		#endregion

		#endregion

		#region ToHeading

		private void doToHeading()
		{
			TextBox201.Text = "0";
			TextBox201_Validating(TextBox201, new System.ComponentModel.CancelEventArgs());
			MultiPage1.SelectedIndex = 4;
		}

		private bool Type2Segment(double OutDir, ref TrackLeg Segment)
		{
			Segment.ptStart = m_CurrPnt;

			int TurnDir = 1 - (ComboBox201.SelectedIndex << 1);
			double TurnAngle = NativeMethods.Modulus((OutDir - m_CurrPnt.BestCase.Direction) * TurnDir);
			double fTAS = Functions.IAS2TAS(maxAircraftSpeed2, m_CurrPnt.BestCase.GrossHeight + m_ptDerPrj.Z, GlobalVars.m_CurrADHP.ISAtC);
			double fTurnR = Functions.Bank2Radius(m_BankAngle, fTAS);

			TextBox202.Text = System.Math.Round(TurnAngle).ToString();
			TextBox203.Text = UnitConverter.DistanceToDisplayUnits(fTurnR, eRoundMode.NERAEST).ToString();

			Segment.BestCase.turns = 1;
			Segment.BestCase.Turn[0].ptCenter = Functions.LocalToPrj(m_CurrPnt.BestCase.pPoint, m_CurrPnt.BestCase.Direction + 90.0 * TurnDir, fTurnR, 0.0);
			Segment.BestCase.Turn[0].ptStart = m_CurrPnt.BestCase.pPoint;

			Segment.BestCase.Turn[0].ptEnd = Functions.LocalToPrj(Segment.BestCase.Turn[0].ptCenter, OutDir - 90.0 * TurnDir, fTurnR, 0.0);
			Segment.BestCase.Turn[0].ptEnd.M = OutDir;

			Segment.BestCase.Turn[0].TurnDir = -TurnDir;
			Segment.BestCase.Turn[0].Angle = TurnAngle;
			Segment.BestCase.Turn[0].Radius = fTurnR;
			Segment.BestCase.Turn[0].StartDist = 0.0;

			//---------------------------------

			IPointCollection MyPC = new ESRI.ArcGIS.Geometry.Multipoint();
			MyPC.AddPoint(m_CurrPnt.BestCase.pPoint);
			MyPC.AddPoint(Segment.BestCase.Turn[0].ptEnd);

			Segment.BestCase.pNominalPoly = Functions.CalcTrajectoryFromMultiPoint(MyPC);

			//Graphics.DrawPolyline(Segment.BestCase.pNominalPoly, 255,2);
			//Functions.ProcessMessages();

			Segment.BestCase.Length = Segment.BestCase.pNominalPoly.Length;
			Segment.BestCase.Turn[0].Length = Segment.BestCase.Length;

			Segment.ptEnd.BestCase.pPoint = Segment.BestCase.Turn[0].ptEnd;
			Segment.ptEnd.BestCase.Direction = OutDir;

			//========================================================================

			TurnAngle = NativeMethods.Modulus((OutDir - m_CurrPnt.WorstCase.Direction) * TurnDir);
			fTAS = Functions.IAS2TAS(minAircraftSpeed2, m_CurrPnt.WorstCase.GrossHeight + m_ptDerPrj.Z, GlobalVars.m_CurrADHP.ISAtC);
			fTurnR = Functions.Bank2Radius(m_BankAngle, fTAS);

			//TextBox202.Text = System.Math.Round(TurnAngle).ToString();
			//TextBox203.Text = UnitConverter.DistanceToDisplayUnits(fTurnR, eRoundMode.NERAEST).ToString();

			Segment.WorstCase.turns = 1;
			Segment.WorstCase.Turn[0].ptCenter = Functions.LocalToPrj(m_CurrPnt.WorstCase.pPoint, m_CurrPnt.WorstCase.Direction + 90.0 * TurnDir, fTurnR, 0.0);
			Segment.WorstCase.Turn[0].ptStart = m_CurrPnt.WorstCase.pPoint;

			Segment.WorstCase.Turn[0].ptEnd = Functions.LocalToPrj(Segment.WorstCase.Turn[0].ptCenter, OutDir - 90.0 * TurnDir, fTurnR, 0.0);
			Segment.WorstCase.Turn[0].ptEnd.M = OutDir;

			Segment.WorstCase.Turn[0].TurnDir = -TurnDir;
			Segment.WorstCase.Turn[0].Angle = TurnAngle;
			Segment.WorstCase.Turn[0].Radius = fTurnR;
			Segment.WorstCase.Turn[0].StartDist = 0.0;

			//---------------------------------

			MyPC = new ESRI.ArcGIS.Geometry.Multipoint();
			MyPC.AddPoint(m_CurrPnt.WorstCase.pPoint);
			MyPC.AddPoint(Segment.WorstCase.Turn[0].ptEnd);

			Segment.WorstCase.pNominalPoly = Functions.CalcTrajectoryFromMultiPoint(MyPC);

			Segment.WorstCase.Length = Segment.WorstCase.pNominalPoly.Length;
			Segment.WorstCase.Turn[0].Length = Segment.WorstCase.Length;

			Segment.ptEnd.WorstCase.pPoint = Segment.WorstCase.Turn[0].ptEnd;
			Segment.ptEnd.WorstCase.Direction = OutDir;
			//========================================================================

			Segment.ptEnd.atHeight = false;

			CurrSegment.ptEnd.BestCase.NetHeight = Math.Min(m_CurrPnt.BestCase.NetHeight + Segment.BestCase.Length * BestMaxNetGrad2, cMaxH);
			CurrSegment.ptEnd.BestCase.GrossHeight = Math.Min(m_CurrPnt.BestCase.GrossHeight + Segment.BestCase.Length * BestMaxGrossGrad, cMaxH);

			CurrSegment.ptEnd.WorstCase.NetHeight = Math.Min(m_CurrPnt.WorstCase.NetHeight + Segment.WorstCase.Length * WorstMaxNetGrad2, cMaxH);
			CurrSegment.ptEnd.WorstCase.GrossHeight = Math.Min(m_CurrPnt.WorstCase.GrossHeight + Segment.WorstCase.Length * WorstMaxGrossGrad, cMaxH);

			//Functions.double prevcourse = Math.Round(m_CurrAzt, 2);

			string[] sTurnDir = { "вправо", "", "влево" };
			double azt = NativeMethods.Modulus(Functions.Dir2Azt(Segment.BestCase.Turn[0].ptEnd, OutDir) - GlobalVars.m_CurrADHP.MagVar);
			string aztStr = Math.Round(azt).ToString();

			while (aztStr.Length < 3)
				aztStr = "0" + aztStr;
			aztStr = aztStr + "°";

			Segment.Comment = "Разворот " + sTurnDir[TurnDir + 1] + " на направлении " + TextBox201.Text + "°";

			Label208.Text = Segment.Comment;

			Segment.bOnWPTFIX = false;
			Segment.GuidanceNav.TypeCode = eNavaidClass.CodeNONE;
			Segment.InterceptionNav.TypeCode = eNavaidClass.CodeNONE;

			Segment.PathCode = SegmentPathType.CF;
			Segment.SegmentCode = eLegType.toHeading;

			return true;
		}

		#region toHeading events

		private void TextBox201_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;

			if (e.KeyChar == 13)
				TextBox201_Validating(TextBox201, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxInteger(ref eventChar);
			//Functions.TextBoxFloat(ref eventChar, TextBox201.Text);

			e.KeyChar = eventChar;
			if (e.KeyChar == 0)
				e.Handled = true;
		}

		private void TextBox201_Validating(object sender, CancelEventArgs e)
		{
			double Azt;
			if (!double.TryParse(TextBox201.Text, out Azt))
				return;

			IPoint pPtGeo = (IPoint)Functions.ToGeo(m_CurrPnt.BestCase.pPoint);
			Type2_OutDir = Functions.Azt2Dir(pPtGeo, NativeMethods.Modulus(Azt, 360.0) + m_MagVar);

			ConstructNextSegment();
		}

		private void ComboBox201_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ComboBox201.SelectedIndex < 0)
				return;

			if (!m_bFormInitialised)
				return;

			ConstructNextSegment();
		}

		#endregion

		#endregion

		#region CourseIntercept

		private void doCourseIntercept()
		{
			FillComboBox301Stations();
			if (ComboBox301.Items.Count > 0)
				ComboBox301.SelectedIndex = 0;
			MultiPage1.SelectedIndex = 5;
		}

		private int FillComboBox301Stations()
		{
			IPoint pPtRefB = m_CurrPnt.BestCase.pPoint;
			double refDirectionB = m_CurrPnt.BestCase.Direction;
			double turnAltB = m_CurrPnt.BestCase.GrossHeight;

			IPoint pPtRefW = m_CurrPnt.WorstCase.pPoint;
			double refDirectionW = m_CurrPnt.WorstCase.Direction;
			double turnAltW = m_CurrPnt.WorstCase.GrossHeight;

			double fCurrMinH = Math.Min(m_CurrPnt.BestCase.NetHeight, m_CurrPnt.WorstCase.NetHeight);

			ComboBox301.Items.Clear();

			int n = GlobalVars.WPTList.Length;
			int j = 0;

			ComboBox301_RIntervals = new Interval[n];
			ComboBox301_LIntervals = new Interval[n];

			for (int i = 0; i < n; i++)
			{
				IPoint pPtNav = GlobalVars.WPTList[i].pPtPrj;

				double fDistB = Functions.ReturnDistanceInMeters(pPtRefB, pPtNav);
				double fDistW = Functions.ReturnDistanceInMeters(pPtRefW, pPtNav);
				double fDistMax = Math.Max(fDistB, fDistW);

				if (fDistMax > 0.5 * GlobalVars.MaxNAVDist)
					continue;

				double dH = Math.Abs(fCurrMinH - pPtNav.Z + m_ptDerPrj.Z);
				double MaxDist = 4130.0 * 2.0 * Math.Sqrt(dH);

				if (fDistMax > MaxDist)
					continue;

				Interval RIntervalB = Functions.CalcNavInterval(pPtRefB, refDirectionB, SideDirection.rightSide, pPtNav, maxAircraftSpeed2, turnAltB + m_ptDerPrj.Z, m_BankAngle, BestMaxGrossGrad);
				Interval LIntervalB = Functions.CalcNavInterval(pPtRefB, refDirectionB, SideDirection.leftSide, pPtNav, maxAircraftSpeed2, turnAltB + m_ptDerPrj.Z, m_BankAngle, BestMaxGrossGrad);

				Interval RIntervalW = Functions.CalcNavInterval(pPtRefW, refDirectionW, SideDirection.rightSide, pPtNav, minAircraftSpeed2, turnAltW + m_ptDerPrj.Z, m_BankAngle, WorstMaxGrossGrad);
				Interval LIntervalW = Functions.CalcNavInterval(pPtRefW, refDirectionW, SideDirection.leftSide, pPtNav, minAircraftSpeed2, turnAltW + m_ptDerPrj.Z, m_BankAngle, WorstMaxGrossGrad);

				Interval LInterval;
				Interval RInterval;

				LInterval.Left = Math.Max(LIntervalB.Left, LIntervalW.Left);
				LInterval.Right = Math.Min(LIntervalB.Right, LIntervalW.Right);

				RInterval.Left = Math.Max(RIntervalB.Left, RIntervalW.Left);
				RInterval.Right = Math.Min(RIntervalB.Right, RIntervalW.Right);

				if (LInterval.Right > 0.0 || RInterval.Right > 0.0)
				{
					ComboBox301_LIntervals[j] = LInterval;
					ComboBox301_RIntervals[j] = RInterval;

					ComboBox301.Items.Add(GlobalVars.WPTList[i]);
					j++;
				}
			}

			Array.Resize<Interval>(ref ComboBox301_LIntervals, j);
			Array.Resize<Interval>(ref ComboBox301_RIntervals, j);

			return j;
		}

		private bool Type3Segment(ref TrackLeg Segment)
		{
			/////**/////
			//double Type3_OutDir

			if (CheckBox301.Checked && CheckBox301.Enabled)
			{
				int k = ComboBox303.SelectedIndex;

				IPoint pToPoint = ((WPT_FIXData)ComboBox303.SelectedItem).pPtPrj;
				int side = (int)Functions.SideDef(Type3_CurNav.pPtPrj, m_CurrPnt.BestCase.Direction, pToPoint);

				//if (Type3_FullInterval.Left >= 0 && Type3_FullInterval.Right <= 0)

				if (side != Type3_TurnDir)
					Type3_OutDir = Functions.ReturnAngleInDegrees(pToPoint, Type3_CurNav.pPtPrj);
				else
					Type3_OutDir = Functions.ReturnAngleInDegrees(Type3_CurNav.pPtPrj, pToPoint);
			}
			/////**/////

			double fDistToIntersect, dist0, turnAlt, fTAS, fTurnR, AddDist, fRefX, fRefY;

			IPointCollection MyPC;

			Segment.ptStart = m_CurrPnt;
			Functions.PrjToLocal(m_CurrPnt.BestCase.pPoint, m_CurrPnt.BestCase.Direction, Type3_CurNav.pPtPrj, out fRefX, out fRefY);

			double alpha = GlobalVars.DegToRadValue * NativeMethods.Modulus(Type3_TurnDir * (Type3_OutDir - m_CurrPnt.BestCase.Direction));

			if (Math.Abs(alpha) < GlobalVars.degEps)		//GlobalVars.DegToRadValue
			{
				ESRI.ArcGIS.esriSystem.IClone pClone = (ESRI.ArcGIS.esriSystem.IClone)Type3_CurNav.pPtPrj;
				Segment.BestCase.turns = 0;

				Segment.ptEnd.BestCase.pPoint = (IPoint)pClone.Clone();
				Segment.ptEnd.BestCase.pPoint.M = m_CurrPnt.BestCase.Direction;
				Segment.ptEnd.BestCase.Direction = m_CurrPnt.BestCase.Direction;

				Segment.BestCase.pNominalPoly = (IPolyline)new Polyline();
				Segment.BestCase.pNominalPoly.FromPoint = m_CurrPnt.BestCase.pPoint;
				Segment.BestCase.pNominalPoly.ToPoint = Segment.ptEnd.BestCase.pPoint;
				Segment.BestCase.Length = Segment.BestCase.pNominalPoly.Length;

				TextBox303.Text = "-";
				TextBox304.Text = "-";
			}
			else
			{
				turnAlt = m_CurrPnt.BestCase.GrossHeight + m_ptDerPrj.Z;

				//======================= iterate
				fTAS = Functions.IAS2TAS(maxAircraftSpeed2, turnAlt, GlobalVars.m_CurrADHP.ISAtC);
				fTurnR = Functions.Bank2Radius(m_BankAngle, fTAS);

				dist0 = fTurnR * Math.Tan(0.5 * alpha);

				fDistToIntersect = fRefX - Type3_TurnDir * fRefY / System.Math.Tan(alpha);
				turnAlt = m_CurrPnt.BestCase.GrossHeight + (fDistToIntersect - dist0) * BestMaxGrossGrad + m_ptDerPrj.Z;
				//=======================

				fTAS = Functions.IAS2TAS(maxAircraftSpeed2, turnAlt, GlobalVars.m_CurrADHP.ISAtC);
				fTurnR = Functions.Bank2Radius(m_BankAngle, fTAS);

				TextBox304.Text = UnitConverter.DistanceToDisplayUnits(fTurnR, eRoundMode.NERAEST).ToString();

				AddDist = fDistToIntersect - fTurnR * System.Math.Tan(0.5 * alpha);			//fRefX - Type3_CurTurnDir * fRefY / System.Math.Tan(alpha);
				TextBox303.Text = UnitConverter.DistanceToDisplayUnits(AddDist, eRoundMode.NERAEST).ToString();

				Segment.BestCase.turns = 1;

				Segment.BestCase.Turn[0].ptStart = Functions.LocalToPrj(m_CurrPnt.BestCase.pPoint, m_CurrPnt.BestCase.Direction, AddDist);
				Segment.BestCase.Turn[0].ptStart.M = m_CurrPnt.BestCase.Direction;

				Segment.BestCase.Turn[0].ptCenter = Functions.LocalToPrj(Segment.BestCase.Turn[0].ptStart, m_CurrPnt.BestCase.Direction + 90.0 * Type3_TurnDir, fTurnR);

				Segment.BestCase.Turn[0].ptEnd = Functions.LocalToPrj(Segment.BestCase.Turn[0].ptCenter, Type3_OutDir - 90.0 * Type3_TurnDir, fTurnR);
				Segment.BestCase.Turn[0].ptEnd.M = Type3_OutDir;

				//---------------------------------

				Segment.BestCase.Turn[0].Angle = NativeMethods.Modulus((Type3_OutDir - m_CurrPnt.BestCase.Direction) * Type3_TurnDir);
				Segment.BestCase.Turn[0].TurnDir = -Type3_TurnDir;
				Segment.BestCase.Turn[0].StartDist = AddDist;
				Segment.BestCase.Turn[0].Radius = fTurnR;

				Segment.BestCase.DRLength = AddDist;

				//---------------------------------

				MyPC = new ESRI.ArcGIS.Geometry.Multipoint();

				MyPC.AddPoint(m_CurrPnt.BestCase.pPoint);
				MyPC.AddPoint(Segment.BestCase.Turn[0].ptStart);
				MyPC.AddPoint(Segment.BestCase.Turn[0].ptEnd);

				Segment.BestCase.pNominalPoly = Functions.CalcTrajectoryFromMultiPoint(MyPC);
				Segment.BestCase.Length = Segment.BestCase.pNominalPoly.Length;

				Segment.BestCase.Turn[0].Length = Segment.BestCase.Length - Segment.BestCase.DRLength;


				Segment.ptEnd.BestCase.pPoint = Segment.BestCase.Turn[0].ptEnd;
				Segment.ptEnd.BestCase.Direction = Type3_OutDir;
			}
			//==================================================================================================

			Functions.PrjToLocal(m_CurrPnt.WorstCase.pPoint, m_CurrPnt.WorstCase.Direction, Type3_CurNav.pPtPrj, out fRefX, out fRefY);
			alpha = GlobalVars.DegToRadValue * NativeMethods.Modulus(Type3_TurnDir * (Type3_OutDir - m_CurrPnt.WorstCase.Direction));

			if (Math.Abs(alpha) < GlobalVars.degEps)//GlobalVars.DegToRadValue
			{
				ESRI.ArcGIS.esriSystem.IClone pClone = (ESRI.ArcGIS.esriSystem.IClone)Type3_CurNav.pPtPrj;
				Segment.WorstCase.turns = 0;

				Segment.ptEnd.WorstCase.pPoint = (IPoint)pClone.Clone();
				Segment.ptEnd.WorstCase.pPoint.M = m_CurrPnt.WorstCase.Direction;
				Segment.ptEnd.WorstCase.Direction = m_CurrPnt.WorstCase.Direction;

				Segment.WorstCase.pNominalPoly = (IPolyline)new Polyline();
				Segment.WorstCase.pNominalPoly.FromPoint = m_CurrPnt.WorstCase.pPoint;
				Segment.WorstCase.pNominalPoly.ToPoint = Segment.ptEnd.WorstCase.pPoint;
				Segment.WorstCase.Length = Segment.WorstCase.pNominalPoly.Length;
			}
			else
			{
				turnAlt = m_CurrPnt.WorstCase.GrossHeight + m_ptDerPrj.Z;
				//==========================================================================================================
				fTAS = Functions.IAS2TAS(maxAircraftSpeed2, turnAlt, GlobalVars.m_CurrADHP.ISAtC);
				fTurnR = Functions.Bank2Radius(m_BankAngle, fTAS);

				dist0 = fTurnR * Math.Tan(0.5 * alpha);
				fDistToIntersect = fRefX - Type3_TurnDir * fRefY / System.Math.Tan(alpha);
				turnAlt = m_CurrPnt.WorstCase.GrossHeight + (fDistToIntersect - dist0) * WorstMaxGrossGrad + m_ptDerPrj.Z;
				//==========================================================================================================
				fTAS = Functions.IAS2TAS(minAircraftSpeed2, turnAlt, GlobalVars.m_CurrADHP.ISAtC);
				fTurnR = Functions.Bank2Radius(m_BankAngle, fTAS);

				AddDist = fDistToIntersect - fTurnR * System.Math.Tan(0.5 * alpha);

				Segment.WorstCase.turns = 1;

				Segment.WorstCase.Turn[0].ptStart = Functions.LocalToPrj(m_CurrPnt.WorstCase.pPoint, m_CurrPnt.WorstCase.Direction, AddDist);
				Segment.WorstCase.Turn[0].ptStart.M = m_CurrPnt.WorstCase.Direction;

				Segment.WorstCase.Turn[0].ptCenter = Functions.LocalToPrj(Segment.WorstCase.Turn[0].ptStart, m_CurrPnt.WorstCase.Direction + 90.0 * Type3_TurnDir, fTurnR);

				Segment.WorstCase.Turn[0].ptEnd = Functions.LocalToPrj(Segment.WorstCase.Turn[0].ptCenter, Type3_OutDir - 90.0 * Type3_TurnDir, fTurnR);
				Segment.WorstCase.Turn[0].ptEnd.M = Type3_OutDir;
				//---------------------------------

				Segment.WorstCase.Turn[0].Angle = NativeMethods.Modulus((Type3_OutDir - m_CurrPnt.WorstCase.Direction) * Type3_TurnDir);
				Segment.WorstCase.Turn[0].TurnDir = -Type3_TurnDir;
				Segment.WorstCase.Turn[0].StartDist = AddDist;
				Segment.WorstCase.Turn[0].Radius = fTurnR;

				Segment.WorstCase.DRLength = AddDist;

				//---------------------------------

				MyPC = new ESRI.ArcGIS.Geometry.Multipoint();

				MyPC.AddPoint(m_CurrPnt.WorstCase.pPoint);
				MyPC.AddPoint(Segment.WorstCase.Turn[0].ptStart);
				MyPC.AddPoint(Segment.WorstCase.Turn[0].ptEnd);

				Segment.WorstCase.pNominalPoly = Functions.CalcTrajectoryFromMultiPoint(MyPC);
				Segment.WorstCase.Length = Segment.WorstCase.pNominalPoly.Length;

				Segment.WorstCase.Turn[0].Length = Segment.WorstCase.Length - Segment.WorstCase.DRLength;

				Segment.ptEnd.WorstCase.pPoint = Segment.WorstCase.Turn[0].ptEnd;
				Segment.ptEnd.WorstCase.Direction = Type3_OutDir;
			}

			//==================================================================================================
			Segment.ptEnd.atHeight = false;

			Segment.ptEnd.BestCase.NetHeight = Math.Min(m_CurrPnt.BestCase.NetHeight + Segment.BestCase.Length * BestMaxNetGrad2, cMaxH);
			Segment.ptEnd.BestCase.GrossHeight = Math.Min(m_CurrPnt.BestCase.GrossHeight + Segment.BestCase.Length * BestMaxGrossGrad, cMaxH);

			Segment.ptEnd.WorstCase.NetHeight = Math.Min(m_CurrPnt.WorstCase.NetHeight + Segment.WorstCase.Length * WorstMaxNetGrad2, cMaxH);
			Segment.ptEnd.WorstCase.GrossHeight = Math.Min(m_CurrPnt.WorstCase.GrossHeight + Segment.WorstCase.Length * WorstMaxGrossGrad, cMaxH);

			double NewAzt = NativeMethods.Modulus(Functions.Dir2Azt(Type3_CurNav.pPtPrj, Type3_OutDir) - Type3_CurNav.MagVar);

			TextBox301.Text = Math.Round(Segment.BestCase.Turn[0].Angle, 1).ToString();
			TextBox302.Text = Math.Round(NewAzt, 2).ToString();

			//double prevcourse = Math.Round(m_CurrAzt, 2);

			string[] sTurnDir = { "вправо.", "", "влево." };

			double aztIn = NativeMethods.Modulus(m_CurrAzt - GlobalVars.m_CurrADHP.MagVar);
			string aztStrIn = Math.Round(aztIn).ToString();

			while (aztStrIn.Length < 3)
				aztStrIn = "0" + aztStrIn;
			aztStrIn = aztStrIn + "°";

			double aztOut = NativeMethods.Modulus(Functions.Dir2Azt(Segment.ptEnd.BestCase.pPoint, Type3_OutDir) - GlobalVars.m_CurrADHP.MagVar);
			string aztStrOut = Math.Round(aztOut).ToString();

			while (aztStrOut.Length < 3)
				aztStrOut = "0" + aztStrOut;
			aztStrOut = aztStrOut + "° ";

			if (PrevSegment.SegmentCode == eLegType.courseIntercept || PrevSegment.SegmentCode == eLegType.turnAndIntercept)
			{
				Segment.PathCode = SegmentPathType.CI;
				Segment.Comment = "По линии пути " + aztStrIn + ", до перехвата линии пути " + aztStrOut + Type3_CurNav + ", с разворотом " + sTurnDir[Type3_TurnDir + 1];
			}
			else
			{
				Segment.PathCode = SegmentPathType.VI;
				Segment.Comment = "По направлению " + aztStrIn + ", до перехвата линии пути " + aztStrOut + Type3_CurNav + ", с разворотом " + sTurnDir[Type3_TurnDir + 1];
			}

			Label315.Text = Segment.Comment;
			//------------------------------------------------------------
			Segment.bOnWPTFIX = false;
			Segment.SegmentCode = eLegType.courseIntercept;

			switch (PrevSegment.PathCode)
			{
				case SegmentPathType.TF:
				case SegmentPathType.CA:
				case SegmentPathType.CD:
				case SegmentPathType.CI:
				case SegmentPathType.CR:
				case SegmentPathType.CF:
				case SegmentPathType.DF:
					Segment.PathCode = SegmentPathType.CI;
					break;
				default:
					Segment.PathCode = SegmentPathType.VI;
					break;
			}

			if (PrevSegment.SegmentCode == eLegType.arcPath)
				Segment.GuidanceNav = PrevSegment.InterceptionNav;
			else
				Segment.GuidanceNav = PrevSegment.GuidanceNav;

			Segment.InterceptionNav.TypeCode = eNavaidClass.CodeNONE;

			return true;
		}

		#region courseIntercept events

		private void Type3_SetNewDirection(double NewDir)
		{
			if (!Functions.AngleInInterval(NewDir, Type3_Interval))
			{
				if (Functions.SubtractAngles(Type3_Interval.Left, NewDir) < Functions.SubtractAngles(Type3_Interval.Right, NewDir))
					NewDir = Type3_Interval.Left;
				else
					NewDir = Type3_Interval.Right;
			}

			Type3_OutDir = NewDir;
			ConstructNextSegment();
		}

		private void TextBox302_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;

			if (e.KeyChar == 13)
				TextBox302_Validating(TextBox302, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, TextBox302.Text);
			//Functions.TextBoxInteger(ref eventChar);

			e.KeyChar = eventChar;
			if (e.KeyChar == 0)
				e.Handled = true;
		}

		private void TextBox302_Validating(object sender, CancelEventArgs e)
		{
			double fDir;
			if (!double.TryParse(TextBox302.Text, out fDir))
				return;

			Type3_SetNewDirection(Functions.Azt2Dir(Type3_CurNav.pPtGeo, fDir + Type3_CurNav.MagVar));
		}

		private void CheckBox301_CheckedChanged(object sender, EventArgs e)
		{
			if (!CheckBox301.Checked)
			{
				ComboBox303.Enabled = false;
				TextBox302.ReadOnly = false;
				TextBox302.BackColor = System.Drawing.SystemColors.Window;
				label304.Enabled = false;
			}
			else
			{
				ComboBox303.Enabled = true;
				TextBox302.ReadOnly = true;
				TextBox302.BackColor = System.Drawing.SystemColors.Control;
				ComboBox303_SelectedIndexChanged(ComboBox303, new System.EventArgs());
				label304.Enabled = true;
			}
		}

		private void ComboBox301_SelectedIndexChanged(object sender, EventArgs e)
		{
			int k = ComboBox301.SelectedIndex;
			if (k < 0)
				return;

			Type3_CurNav = (WPT_FIXData)ComboBox301.SelectedItem;
			label302.Text = Navaids_DataBase.GetNavTypeName(Type3_CurNav.TypeCode);

			int i = ComboBox302.SelectedIndex;
			ComboBox302.Enabled = (ComboBox301_LIntervals[k].Right > 0) && (ComboBox301_RIntervals[k].Right > 0);

			if (ComboBox301_LIntervals[k].Right < 0 && i == 0)
				ComboBox302.SelectedIndex = 1;
			else if (ComboBox301_RIntervals[k].Right < 0 && i == 1)
				ComboBox302.SelectedIndex = 0;

			if (i == ComboBox302.SelectedIndex)
				ComboBox302_SelectedIndexChanged(ComboBox302, new System.EventArgs());
		}

		private void ComboBox302_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!m_bFormInitialised)
				return;

			if (ComboBox302.SelectedIndex < 0)
				return;

			int k = ComboBox301.SelectedIndex;
			Interval inter;

			if (ComboBox302.SelectedIndex == 0)
			{
				Type3_TurnDir = SideDirection.leftSide;

				inter = ComboBox301_LIntervals[k];

				Type3_Interval.Left = m_CurrPnt.BestCase.Direction + inter.Left;
				Type3_Interval.Right = m_CurrPnt.BestCase.Direction + inter.Right;
			}
			else
			{
				Type3_TurnDir = SideDirection.rightSide;

				inter = ComboBox301_RIntervals[k];

				Type3_Interval.Left = m_CurrPnt.BestCase.Direction - inter.Right;
				Type3_Interval.Right = m_CurrPnt.BestCase.Direction - inter.Left;
			}

			Type3_FullInterval.Left = -1;
			Type3_FullInterval.Right = 1;

			if (ComboBox301_LIntervals[k].Left == 0 && ComboBox301_RIntervals[k].Left == 0)
			{
				Type3_FullInterval.Left = ComboBox301_LIntervals[k].Right;
				Type3_FullInterval.Right = -ComboBox301_RIntervals[k].Right;
			}

			ComboBox303.Items.Clear();
			int n = GlobalVars.WPTList.Length;

			for (int i = 0; i < n; i++)
			{
				if (GlobalVars.WPTList[i].Name != ComboBox301.Text)
				{
					double NavDir;
					//if (GlobalVars.WPTList[i].Name == "NAMES")
					//{
					//    NavDir = 0.0;
					//}

					if ((int)Functions.SideDef(Type3_CurNav.pPtPrj, m_CurrPnt.BestCase.Direction, GlobalVars.WPTList[i].pPtPrj) == Type3_TurnDir)
						NavDir = Functions.ReturnAngleInDegrees(GlobalVars.WPTList[i].pPtPrj, Type3_CurNav.pPtPrj);
					else
						NavDir = Functions.ReturnAngleInDegrees(Type3_CurNav.pPtPrj, GlobalVars.WPTList[i].pPtPrj);

					double TurnAngle = NativeMethods.Modulus(Type3_TurnDir * (NavDir - m_CurrPnt.BestCase.Direction), 360.0);

					if (TurnAngle >= inter.Left && TurnAngle <= inter.Right)
						ComboBox303.Items.Add(GlobalVars.WPTList[i]);
					else
					{
						TurnAngle = NativeMethods.Modulus(TurnAngle + 180.0, 360.0);
						if (TurnAngle >= inter.Left && TurnAngle <= inter.Right)
							ComboBox303.Items.Add(GlobalVars.WPTList[i]);
						else if (Type3_FullInterval.Left >= 0 && Type3_FullInterval.Right<=0)
						{
							TurnAngle = NativeMethods.Modulus(Type3_TurnDir * (m_CurrPnt.BestCase.Direction - NavDir), 360.0);

							if (TurnAngle >= inter.Left && TurnAngle <= inter.Right)
								ComboBox303.Items.Add(GlobalVars.WPTList[i]);
							else
							{
								TurnAngle = NativeMethods.Modulus(TurnAngle + 180.0, 360.0);
								if (TurnAngle >= inter.Left && TurnAngle <= inter.Right)
									ComboBox303.Items.Add(GlobalVars.WPTList[i]);
							}
						}
					}
				}
			}

			//if
			inter.Left = Math.Round(NativeMethods.Modulus(Functions.Dir2Azt(Type3_CurNav.pPtPrj, Type3_Interval.Right) - m_MagVar), 1);
			inter.Right = Math.Round(NativeMethods.Modulus(Functions.Dir2Azt(Type3_CurNav.pPtPrj, Type3_Interval.Left) - m_MagVar), 1);
			Label312.Text = Functions.FormatInterval(inter);

			CheckBox301.Enabled = ComboBox303.Items.Count > 0;	//j > 0;
			//Array.Resize<WPT_FIXData>(ref ComboBox303List, j);

			if (ComboBox303.Items.Count > 0)
				ComboBox303.SelectedIndex = 0;
			else
				CheckBox301.Checked = false;

			if (!(CheckBox301.Checked && CheckBox301.Enabled))
				TextBox302_Validating(TextBox302, new System.ComponentModel.CancelEventArgs());
		}

		private void ComboBox303_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!(CheckBox301.Checked && CheckBox301.Enabled))
				return;

			int k = ComboBox303.SelectedIndex;
			if (k < 0)
				return;

			IPoint pToPoint = ((WPT_FIXData)ComboBox303.SelectedItem).pPtPrj;
			int side = (int)Functions.SideDef(Type3_CurNav.pPtPrj, m_CurrPnt.BestCase.Direction, pToPoint);

			//IPolyline pPolyl = (IPolyline)new Polyline();
			//pPolyl.FromPoint = Type3_CurNav.pPtPrj;
			//pPolyl.ToPoint = Functions.LocalToPrj(Type3_CurNav.pPtPrj, m_CurrPnt.BestCase.Direction, 100000.0);
			//Graphics.DrawPolyline(pPolyl, -1, 2);
			//Graphics.DrawPointWithText(Type3_CurNav.pPtPrj, "Nav");
			//Graphics.DrawPointWithText(pToPoint, "pToPoint");

			//if (Type3_FullInterval.Left >= 0 && Type3_FullInterval.Right <= 0)
				ConstructNextSegment();
			//else if (side != Type3_TurnDir)
			//    Type3_SetNewDirection(Functions.ReturnAngleInDegrees(pToPoint, Type3_CurNav.pPtPrj));
			//else
			//    Type3_SetNewDirection(Functions.ReturnAngleInDegrees(Type3_CurNav.pPtPrj, pToPoint));
		}

		#endregion

		#endregion

		/*#region CourseIntercept

		private void doCourseIntercept()
		{
			FillComboBox301Stations();
			if (ComboBox301.Items.Count > 0)
				ComboBox301.SelectedIndex = 0;
			MultiPage1.SelectedIndex = 5;
		}

		private int FillComboBox301Stations()
		{
			IPoint pPtRefB = m_CurrPnt.BestCase.pPoint;
			double refDirectionB = m_CurrPnt.BestCase.Direction;
			double turnAltB = m_CurrPnt.BestCase.GrossHeight;

			IPoint pPtRefW = m_CurrPnt.WorstCase.pPoint;
			double refDirectionW = m_CurrPnt.WorstCase.Direction;
			double turnAltW = m_CurrPnt.WorstCase.GrossHeight;

			double fCurrMinH = Math.Min(m_CurrPnt.BestCase.NetHeight, m_CurrPnt.WorstCase.NetHeight);

			ComboBox301.Items.Clear();

			int n = GlobalVars.WPTList.Length;
			int j = 0;

			ComboBox301_LIntervals = new Interval[n];
			ComboBox301_RIntervals = new Interval[n];

			for (int i = 0; i < n; i++)
			{
				IPoint pPtNav = GlobalVars.WPTList[i].pPtPrj;

				double fDistB = Functions.ReturnDistanceInMeters(pPtRefB, pPtNav);
				double fDistW = Functions.ReturnDistanceInMeters(pPtRefW, pPtNav);
				double fDistMax = Math.Max(fDistB, fDistW);

				if (fDistMax > 0.5 * GlobalVars.MaxNAVDist)
					continue;

				double dH = Math.Abs(fCurrMinH - pPtNav.Z + m_ptDerPrj.Z);
				double MaxDist = 4130.0 * 2.0 * Math.Sqrt(dH);

				if (fDistMax > MaxDist)
					continue;

				Interval RIntervalB = Functions.CalcNavInterval(pPtRefB, refDirectionB, SideDirection.rightSide, pPtNav, maxAircraftSpeed2, turnAltB + m_ptDerPrj.Z, m_BankAngle, BestMaxGrossGrad);
				Interval LIntervalB = Functions.CalcNavInterval(pPtRefB, refDirectionB, SideDirection.leftSide, pPtNav, maxAircraftSpeed2, turnAltB + m_ptDerPrj.Z, m_BankAngle, BestMaxGrossGrad);

				Interval RIntervalW = Functions.CalcNavInterval(pPtRefW, refDirectionW, SideDirection.rightSide, pPtNav, minAircraftSpeed2, turnAltW + m_ptDerPrj.Z, m_BankAngle, WorstMaxGrossGrad);
				Interval LIntervalW = Functions.CalcNavInterval(pPtRefW, refDirectionW, SideDirection.leftSide, pPtNav, minAircraftSpeed2, turnAltW + m_ptDerPrj.Z, m_BankAngle, WorstMaxGrossGrad);

				Interval LInterval;
				Interval RInterval;

				LInterval.Left = Math.Max(LIntervalB.Left, LIntervalW.Left);
				LInterval.Right = Math.Min(LIntervalB.Right, LIntervalW.Right);

				RInterval.Left = Math.Max(RIntervalB.Left, RIntervalW.Left);
				RInterval.Right = Math.Min(RIntervalB.Right, RIntervalW.Right);

				if (LInterval.Right > 0.0 || RInterval.Right > 0.0)
				{
					ComboBox301_LIntervals[j] = LInterval;
					ComboBox301_RIntervals[j] = RInterval;

					ComboBox301.Items.Add(GlobalVars.WPTList[i]);
					j++;
				}
			}

			Array.Resize<Interval>(ref ComboBox301_LIntervals, j);
			Array.Resize<Interval>(ref ComboBox301_RIntervals, j);

			return j;
		}

		private bool Type3Segment(double OutDir, ref TrackLeg Segment)
		{
			double fDistToIntersect, dist0, turnAlt,
					fTAS, fTurnR, AddDist, fRefX, fRefY;

			IPointCollection MyPC;

			Segment.ptStart = m_CurrPnt;
			Functions.PrjToLocal(m_CurrPnt.BestCase.pPoint, m_CurrPnt.BestCase.Direction, Type3_CurNav.pPtPrj, out fRefX, out fRefY);

			double alpha = GlobalVars.DegToRadValue * NativeMethods.Modulus(Type3_TurnDir * (OutDir - m_CurrPnt.BestCase.Direction));

			if (Math.Abs(alpha) < GlobalVars.degEps)		//GlobalVars.DegToRadValue
			{
				ESRI.ArcGIS.esriSystem.IClone pClone = (ESRI.ArcGIS.esriSystem.IClone)Type3_CurNav.pPtPrj;
				Segment.BestCase.turns = 0;

				Segment.ptEnd.BestCase.pPoint = (IPoint)pClone.Clone();
				Segment.ptEnd.BestCase.pPoint.M = m_CurrPnt.BestCase.Direction;
				Segment.ptEnd.BestCase.Direction = m_CurrPnt.BestCase.Direction;

				Segment.BestCase.pNominalPoly = (IPolyline)new Polyline();
				Segment.BestCase.pNominalPoly.FromPoint = m_CurrPnt.BestCase.pPoint;
				Segment.BestCase.pNominalPoly.ToPoint = Segment.ptEnd.BestCase.pPoint;
				Segment.BestCase.Length = Segment.BestCase.pNominalPoly.Length;

				TextBox303.Text = "-";
				TextBox304.Text = "-";
			}
			else
			{
				turnAlt = m_CurrPnt.BestCase.GrossHeight + m_ptDerPrj.Z;

				//======================= iterate
				fTAS = Functions.IAS2TAS(maxAircraftSpeed2, turnAlt, GlobalVars.m_CurrADHP.ISAtC);
				fTurnR = Functions.Bank2Radius(m_BankAngle, fTAS);

				dist0 = fTurnR * Math.Tan(0.5 * alpha);

				fDistToIntersect = fRefX - Type3_TurnDir * fRefY / System.Math.Tan(alpha);
				turnAlt = m_CurrPnt.BestCase.GrossHeight + (fDistToIntersect - dist0) * BestMaxGrossGrad + m_ptDerPrj.Z;
				//=======================

				fTAS = Functions.IAS2TAS(maxAircraftSpeed2, turnAlt, GlobalVars.m_CurrADHP.ISAtC);
				fTurnR = Functions.Bank2Radius(m_BankAngle, fTAS);

				TextBox304.Text = UnitConverter.DistanceToDisplayUnits(fTurnR, eRoundMode.NERAEST).ToString();

				AddDist = fDistToIntersect - fTurnR * System.Math.Tan(0.5 * alpha);			//fRefX - Type3_CurTurnDir * fRefY / System.Math.Tan(alpha);
				TextBox303.Text = UnitConverter.DistanceToDisplayUnits(AddDist, eRoundMode.NERAEST).ToString();

				Segment.BestCase.turns = 1;

				Segment.BestCase.Turn[0].ptStart = Functions.LocalToPrj(m_CurrPnt.BestCase.pPoint, m_CurrPnt.BestCase.Direction, AddDist);
				Segment.BestCase.Turn[0].ptStart.M = m_CurrPnt.BestCase.Direction;

				Segment.BestCase.Turn[0].ptCenter = Functions.LocalToPrj(Segment.BestCase.Turn[0].ptStart, m_CurrPnt.BestCase.Direction + 90.0 * Type3_TurnDir, fTurnR);

				Segment.BestCase.Turn[0].ptEnd = Functions.LocalToPrj(Segment.BestCase.Turn[0].ptCenter, OutDir - 90.0 * Type3_TurnDir, fTurnR);
				Segment.BestCase.Turn[0].ptEnd.M = OutDir;

				//---------------------------------

				Segment.BestCase.Turn[0].Angle = NativeMethods.Modulus((OutDir - m_CurrPnt.BestCase.Direction) * Type3_TurnDir);
				Segment.BestCase.Turn[0].TurnDir = -Type3_TurnDir;
				Segment.BestCase.Turn[0].StartDist = AddDist;
				Segment.BestCase.Turn[0].Radius = fTurnR;

				Segment.BestCase.DRLength = AddDist;

				//---------------------------------

				MyPC = new ESRI.ArcGIS.Geometry.Multipoint();

				MyPC.AddPoint(m_CurrPnt.BestCase.pPoint);
				MyPC.AddPoint(Segment.BestCase.Turn[0].ptStart);
				MyPC.AddPoint(Segment.BestCase.Turn[0].ptEnd);

				Segment.BestCase.pNominalPoly = Functions.CalcTrajectoryFromMultiPoint(MyPC);
				Segment.BestCase.Length = Segment.BestCase.pNominalPoly.Length;

				Segment.BestCase.Turn[0].Length = Segment.BestCase.Length - Segment.BestCase.DRLength;


				Segment.ptEnd.BestCase.pPoint = Segment.BestCase.Turn[0].ptEnd;
				Segment.ptEnd.BestCase.Direction = OutDir;
			}
			//==================================================================================================

			Functions.PrjToLocal(m_CurrPnt.WorstCase.pPoint, m_CurrPnt.WorstCase.Direction, Type3_CurNav.pPtPrj, out fRefX, out fRefY);
			alpha = GlobalVars.DegToRadValue * NativeMethods.Modulus(Type3_TurnDir * (OutDir - m_CurrPnt.WorstCase.Direction));

			if (Math.Abs(alpha) < GlobalVars.degEps)//GlobalVars.DegToRadValue
			{
				ESRI.ArcGIS.esriSystem.IClone pClone = (ESRI.ArcGIS.esriSystem.IClone)Type3_CurNav.pPtPrj;
				Segment.WorstCase.turns = 0;

				Segment.ptEnd.WorstCase.pPoint = (IPoint)pClone.Clone();
				Segment.ptEnd.WorstCase.pPoint.M = m_CurrPnt.WorstCase.Direction;
				Segment.ptEnd.WorstCase.Direction = m_CurrPnt.WorstCase.Direction;

				Segment.WorstCase.pNominalPoly = (IPolyline)new Polyline();
				Segment.WorstCase.pNominalPoly.FromPoint = m_CurrPnt.WorstCase.pPoint;
				Segment.WorstCase.pNominalPoly.ToPoint = Segment.ptEnd.WorstCase.pPoint;
				Segment.WorstCase.Length = Segment.WorstCase.pNominalPoly.Length;
			}
			else
			{
				turnAlt = m_CurrPnt.WorstCase.GrossHeight + m_ptDerPrj.Z;
				//==========================================================================================================
				fTAS = Functions.IAS2TAS(maxAircraftSpeed2, turnAlt, GlobalVars.m_CurrADHP.ISAtC);
				fTurnR = Functions.Bank2Radius(m_BankAngle, fTAS);

				dist0 = fTurnR * Math.Tan(0.5 * alpha);
				fDistToIntersect = fRefX - Type3_TurnDir * fRefY / System.Math.Tan(alpha);
				turnAlt = m_CurrPnt.WorstCase.GrossHeight + (fDistToIntersect - dist0) * WorstMaxGrossGrad + m_ptDerPrj.Z;
				//==========================================================================================================
				fTAS = Functions.IAS2TAS(minAircraftSpeed2, turnAlt , GlobalVars.m_CurrADHP.ISAtC);
				fTurnR = Functions.Bank2Radius(m_BankAngle, fTAS);

				AddDist = fDistToIntersect - fTurnR * System.Math.Tan(0.5 * alpha);

				Segment.WorstCase.turns = 1;

				Segment.WorstCase.Turn[0].ptStart = Functions.LocalToPrj(m_CurrPnt.WorstCase.pPoint, m_CurrPnt.WorstCase.Direction, AddDist);
				Segment.WorstCase.Turn[0].ptStart.M = m_CurrPnt.WorstCase.Direction;

				Segment.WorstCase.Turn[0].ptCenter = Functions.LocalToPrj(Segment.WorstCase.Turn[0].ptStart, m_CurrPnt.WorstCase.Direction + 90.0 * Type3_TurnDir, fTurnR);

				Segment.WorstCase.Turn[0].ptEnd = Functions.LocalToPrj(Segment.WorstCase.Turn[0].ptCenter, OutDir - 90.0 * Type3_TurnDir, fTurnR);
				Segment.WorstCase.Turn[0].ptEnd.M = OutDir;
				//---------------------------------

				Segment.WorstCase.Turn[0].Angle = NativeMethods.Modulus((OutDir - m_CurrPnt.WorstCase.Direction) * Type3_TurnDir);
				Segment.WorstCase.Turn[0].TurnDir = -Type3_TurnDir;
				Segment.WorstCase.Turn[0].StartDist = AddDist;
				Segment.WorstCase.Turn[0].Radius = fTurnR;

				Segment.WorstCase.DRLength = AddDist;

				//---------------------------------

				MyPC = new ESRI.ArcGIS.Geometry.Multipoint();

				MyPC.AddPoint(m_CurrPnt.WorstCase.pPoint);
				MyPC.AddPoint(Segment.WorstCase.Turn[0].ptStart);
				MyPC.AddPoint(Segment.WorstCase.Turn[0].ptEnd);

				Segment.WorstCase.pNominalPoly = Functions.CalcTrajectoryFromMultiPoint(MyPC);
				Segment.WorstCase.Length = Segment.WorstCase.pNominalPoly.Length;

				Segment.WorstCase.Turn[0].Length = Segment.WorstCase.Length - Segment.WorstCase.DRLength;

				Segment.ptEnd.WorstCase.pPoint = Segment.WorstCase.Turn[0].ptEnd;
				Segment.ptEnd.WorstCase.Direction = OutDir;
			}

			//==================================================================================================
			Segment.ptEnd.atHeight = false;

			Segment.ptEnd.BestCase.NetHeight = Math.Min(m_CurrPnt.BestCase.NetHeight + Segment.BestCase.Length * BestMaxNetGrad2, cMaxH);
			Segment.ptEnd.BestCase.GrossHeight = Math.Min(m_CurrPnt.BestCase.GrossHeight + Segment.BestCase.Length * BestMaxGrossGrad, cMaxH);

			Segment.ptEnd.WorstCase.NetHeight = Math.Min(m_CurrPnt.WorstCase.NetHeight + Segment.WorstCase.Length * WorstMaxNetGrad2, cMaxH);
			Segment.ptEnd.WorstCase.GrossHeight = Math.Min(m_CurrPnt.WorstCase.GrossHeight + Segment.WorstCase.Length * WorstMaxGrossGrad, cMaxH);

			double NewAzt = NativeMethods.Modulus(Functions.Dir2Azt(Type3_CurNav.pPtPrj, OutDir) - Type3_CurNav.MagVar);

			TextBox301.Text = Math.Round(Segment.BestCase.Turn[0].Angle, 1).ToString();
			TextBox302.Text = Math.Round(NewAzt, 2).ToString();

			//double prevcourse = Math.Round(m_CurrAzt, 2);

			string[] sTurnDir = { "вправо.", "", "влево." };

			double aztIn = NativeMethods.Modulus(m_CurrAzt - GlobalVars.m_CurrADHP.MagVar);
			string aztStrIn = Math.Round(aztIn).ToString();

			while (aztStrIn.Length < 3)
				aztStrIn = "0" + aztStrIn;
			aztStrIn = aztStrIn + "°";

			double aztOut = NativeMethods.Modulus(Functions.Dir2Azt(Segment.ptEnd.BestCase.pPoint, OutDir) - GlobalVars.m_CurrADHP.MagVar);
			string aztStrOut = Math.Round(aztOut).ToString();

			while (aztStrOut.Length < 3)
				aztStrOut = "0" + aztStrOut;
			aztStrOut = aztStrOut + "° ";

			if (PrevSegment.SegmentCode == eLegType.courseIntercept || PrevSegment.SegmentCode == eLegType.turnAndIntercept)
			{
				Segment.PathCode = SegmentPathType.CI;
				Segment.Comment = "По линии пути " + aztStrIn + ", до перехвата линии пути " + aztStrOut + Type3_CurNav + ", с разворотом " + sTurnDir[Type3_TurnDir + 1];
			}
			else
			{
				Segment.PathCode = SegmentPathType.VI;
				Segment.Comment = "По направлению " + aztStrIn + ", до перехвата линии пути " + aztStrOut + Type3_CurNav + ", с разворотом " + sTurnDir[Type3_TurnDir + 1];
			}

			Label315.Text = Segment.Comment;
			//------------------------------------------------------------
			Segment.bOnWPTFIX = false;
			Segment.SegmentCode = eLegType.courseIntercept;

			switch (PrevSegment.PathCode)
			{
				case SegmentPathType.TF:
				case SegmentPathType.CA:
				case SegmentPathType.CD:
				case SegmentPathType.CI:
				case SegmentPathType.CR:
				case SegmentPathType.CF:
				case SegmentPathType.DF:
					Segment.PathCode = SegmentPathType.CI;
					break;
				default:
					Segment.PathCode = SegmentPathType.VI;
					break;
			}

			if (PrevSegment.SegmentCode == eLegType.arcPath)
				Segment.GuidanceNav = PrevSegment.InterceptionNav;
			else
				Segment.GuidanceNav = PrevSegment.GuidanceNav;

			Segment.InterceptionNav.TypeCode = eNavaidClass.CodeNONE;

			return true;
		}

		#region courseIntercept events

		private void Type3_SetNewDirection(double NewDir)
		{
			if (!Functions.AngleInInterval(NewDir, Type3_Interval))
			{
				if (Functions.SubtractAngles(Type3_Interval.Left, NewDir) < Functions.SubtractAngles(Type3_Interval.Right, NewDir))
					NewDir = Type3_Interval.Left;
				else
					NewDir = Type3_Interval.Right;
			}

			Type3_OutDir = NewDir;
			ConstructNextSegment();
		}

		private void TextBox302_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;

			if (e.KeyChar == 13)
				TextBox302_Validating(TextBox302, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, TextBox302.Text);
			//Functions.TextBoxInteger(ref eventChar);

			e.KeyChar = eventChar;
			if (e.KeyChar == 0)
				e.Handled = true;
		}

		private void TextBox302_Validating(object sender, CancelEventArgs e)
		{
			double fDir;
			if (!double.TryParse(TextBox302.Text, out fDir))
				return;

			Type3_SetNewDirection(Functions.Azt2Dir(Type3_CurNav.pPtGeo, fDir + Type3_CurNav.MagVar));
		}

		private void CheckBox301_CheckedChanged(object sender, EventArgs e)
		{
			if (!CheckBox301.Checked)
			{
				ComboBox303.Enabled = false;
				TextBox302.ReadOnly = false;
				TextBox302.BackColor = System.Drawing.SystemColors.Window;
				label304.Enabled = false;
			}
			else
			{
				ComboBox303.Enabled = true;
				TextBox302.ReadOnly = true;
				TextBox302.BackColor = System.Drawing.SystemColors.Control;
				ComboBox303_SelectedIndexChanged(ComboBox303, new System.EventArgs());
				label304.Enabled = true;
			}
		}

		private void ComboBox301_SelectedIndexChanged(object sender, EventArgs e)
		{
			int k = ComboBox301.SelectedIndex;
			if (k < 0)
				return;

			Type3_CurNav = (WPT_FIXData)ComboBox301.SelectedItem;
			label302.Text = Navaids_DataBase.GetNavTypeName(Type3_CurNav.TypeCode);

			int i = ComboBox302.SelectedIndex;
			ComboBox302.Enabled = (ComboBox301_LIntervals[k].Right > 0) && (ComboBox301_RIntervals[k].Right > 0);

			if (ComboBox301_LIntervals[k].Right < 0 && i == 0)
				ComboBox302.SelectedIndex = 1;
			else if (ComboBox301_RIntervals[k].Right < 0 && i == 1)
				ComboBox302.SelectedIndex = 0;

			if (i == ComboBox302.SelectedIndex)
				ComboBox302_SelectedIndexChanged(ComboBox302, new System.EventArgs());
		}

		private void ComboBox302_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!m_bFormInitialised)
				return;

			if (ComboBox302.SelectedIndex < 0)
				return;

			int k = ComboBox301.SelectedIndex;
			Interval inter;

			if (ComboBox302.SelectedIndex == 0)
			{
				Type3_TurnDir = SideDirection.leftSide;

				inter = ComboBox301_LIntervals[k];

				Type3_Interval.Left = m_CurrPnt.BestCase.Direction + inter.Left;
				Type3_Interval.Right = m_CurrPnt.BestCase.Direction + inter.Right;
			}
			else
			{
				Type3_TurnDir = SideDirection.rightSide;

				inter = ComboBox301_RIntervals[k];

				Type3_Interval.Left = m_CurrPnt.BestCase.Direction - inter.Right;
				Type3_Interval.Right = m_CurrPnt.BestCase.Direction - inter.Left;
			}

			ComboBox303.Items.Clear();
			int n = GlobalVars.WPTList.Length;

			for (int i = 0; i < n; i++)
			{
				if (GlobalVars.WPTList[i].Name != ComboBox301.Text)
				{
					double NavDir;
					if ((int)Functions.SideDef(Type3_CurNav.pPtPrj, m_CurrPnt.BestCase.Direction, GlobalVars.WPTList[i].pPtPrj) == Type3_TurnDir)
						NavDir = Functions.ReturnAngleInDegrees(GlobalVars.WPTList[i].pPtPrj, Type3_CurNav.pPtPrj);
					else
						NavDir = Functions.ReturnAngleInDegrees(Type3_CurNav.pPtPrj, GlobalVars.WPTList[i].pPtPrj);

					double TurnAngle = NativeMethods.Modulus(Type3_TurnDir * (NavDir - m_CurrPnt.BestCase.Direction), 360.0);

					if (TurnAngle >= inter.Left && TurnAngle <= inter.Right)
						ComboBox303.Items.Add(GlobalVars.WPTList[i]);
					else
					{
						TurnAngle = NativeMethods.Modulus(TurnAngle + 180.0, 360.0);
						if (TurnAngle >= inter.Left && TurnAngle <= inter.Right)
							ComboBox303.Items.Add(GlobalVars.WPTList[i]);
					}
				}
			}

			inter.Left = Math.Round(NativeMethods.Modulus(Functions.Dir2Azt(Type3_CurNav.pPtPrj, Type3_Interval.Right) - m_MagVar),1);
			inter.Right = Math.Round(NativeMethods.Modulus(Functions.Dir2Azt(Type3_CurNav.pPtPrj, Type3_Interval.Left) - m_MagVar), 1);
			Label312.Text = Functions.FormatInterval(inter);

			CheckBox301.Enabled = ComboBox303.Items.Count > 0;	//j > 0;
			//Array.Resize<WPT_FIXData>(ref ComboBox303List, j);

			if (ComboBox303.Items.Count > 0)
				ComboBox303.SelectedIndex = 0;
			else
				CheckBox301.Checked = false;

			if (!(CheckBox301.Checked && CheckBox301.Enabled))
				TextBox302_Validating(TextBox302, new System.ComponentModel.CancelEventArgs());
		}

		private void ComboBox303_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!(CheckBox301.Checked && CheckBox301.Enabled))
				return;

			int k = ComboBox303.SelectedIndex;
			if (k < 0)
				return;

			IPoint pToPoint = ((WPT_FIXData)ComboBox303.SelectedItem).pPtPrj;
			int side = (int)Functions.SideDef(Type3_CurNav.pPtPrj, m_CurrPnt.BestCase.Direction, pToPoint);

			//IPolyline pPolyl = (IPolyline)new Polyline();
			//pPolyl.FromPoint = Type3_CurNav.pPtPrj;
			//pPolyl.ToPoint = Functions.LocalToPrj(Type3_CurNav.pPtPrj, m_CurrPnt.BestCase.Direction, 100000.0);
			//Graphics.DrawPolyline(pPolyl, -1, 2);
			//Graphics.DrawPointWithText(Type3_CurNav.pPtPrj, "Nav");
			//Graphics.DrawPointWithText(pToPoint, "pToPoint");

			if (side != Type3_TurnDir)
				Type3_SetNewDirection(Functions.ReturnAngleInDegrees(pToPoint, Type3_CurNav.pPtPrj));
			else
				Type3_SetNewDirection(Functions.ReturnAngleInDegrees(Type3_CurNav.pPtPrj, pToPoint));
		}

		#endregion

		#endregion
		*/

		#region DirectToFIX

		private void doDirectToFIX()
		{
			FillComboBox401Stations();
			if (ComboBox401.Items.Count > 0)
				ComboBox401.SelectedIndex = 0;
			MultiPage1.SelectedIndex = 6;
		}

		private int FillComboBox401Stations()
		{
			double fTAS = Functions.IAS2TAS(maxAircraftSpeed2, m_CurrPnt.BestCase.GrossHeight + m_ptDerPrj.Z, GlobalVars.m_CurrADHP.ISAtC);
			double fTurnR = Functions.Bank2Radius(m_BankAngle, fTAS);

			ComboBox401.Items.Clear();

			int n = GlobalVars.WPTList.Length;

			for (int i = 0; i < n; i++)
			{
				double fDist = Functions.ReturnDistanceInMeters(m_CurrPnt.BestCase.pPoint, GlobalVars.WPTList[i].pPtPrj);

				if (fDist > fTurnR && fDist < 0.5 * GlobalVars.MaxNAVDist)
					ComboBox401.Items.Add(GlobalVars.WPTList[i]);
			}

			return ComboBox401.Items.Count;
		}

		private bool Type4Segment(ref TrackLeg Segment)
		{
			Segment.ptStart = m_CurrPnt;

			int TurnDir = 1 - (ComboBox402.SelectedIndex << 1);
			WPT_FIXData WPtFix = (WPT_FIXData)ComboBox401.SelectedItem;
			//Label406.Text = Navaids_DataBase.GetNavTypeName(WPtFix.TypeCode);

			//Best ==================================================================================================
			double fTAS = Functions.IAS2TAS(maxAircraftSpeed2, m_CurrPnt.BestCase.GrossHeight + m_ptDerPrj.Z, GlobalVars.m_CurrADHP.ISAtC);
			double fTurnR = Functions.Bank2Radius(m_BankAngle, fTAS);

			IPointCollection MyPC = Functions.TurnToFixPrj(m_CurrPnt.BestCase.pPoint, fTurnR, TurnDir, WPtFix.pPtPrj);

			Segment.BestCase.turns = 1;
			Segment.BestCase.Turn[0].ptStart = m_CurrPnt.BestCase.pPoint;
			Segment.BestCase.Turn[0].ptEnd = MyPC.Point[1];
			double OutDir = Segment.BestCase.Turn[0].ptEnd.M;

			Segment.BestCase.Turn[0].ptCenter = Functions.LocalToPrj(m_CurrPnt.BestCase.pPoint, m_CurrPnt.BestCase.Direction + 90.0 * TurnDir, fTurnR);
			Segment.BestCase.Turn[0].TurnDir = -TurnDir;		//SideDef(m_CurrPnt, m_CurrPnt.M, Pt1);
			Segment.BestCase.Turn[0].Angle = NativeMethods.Modulus((OutDir - m_CurrPnt.BestCase.Direction) * TurnDir);
			Segment.BestCase.Turn[0].Radius = fTurnR;
			Segment.BestCase.Turn[0].StartDist = 0.0;

			//---------------------------------
			IPolyline turnPoly = Functions.CalcTrajectoryFromMultiPoint(MyPC);
			Segment.BestCase.Turn[0].Length = turnPoly.Length;

			//---------------------------------
			//Segment.ptEnd.BestCase.pPoint = (IPoint)new ESRI.ArcGIS.Geometry.Point();
			//Segment.ptEnd.BestCase.pPoint.PutCoords(WPtFix.pPtPrj.X, WPtFix.pPtPrj.Y);

			Segment.ptEnd.BestCase.pPoint = (IPoint)new ESRI.ArcGIS.Geometry.Point();
			Segment.ptEnd.BestCase.pPoint.PutCoords(MyPC.Point[1].X, MyPC.Point[1].Y);

			Segment.ptEnd.BestCase.pPoint.M = OutDir;
			Segment.ptEnd.BestCase.Direction = OutDir;
			//MyPC.AddPoint(Segment.ptEnd.BestCase.pPoint);
			//---------------------------------

			Segment.BestCase.pNominalPoly = Functions.CalcTrajectoryFromMultiPoint(MyPC);
			Segment.BestCase.Length = Segment.BestCase.pNominalPoly.Length;

			TextBox401.Text = Math.Round(NativeMethods.Modulus(Functions.Dir2Azt(Segment.BestCase.Turn[0].ptEnd, OutDir) - WPtFix.MagVar, 360.0)).ToString();
			TextBox402.Text = Math.Round(Segment.BestCase.Turn[0].Angle).ToString();
			TextBox403.Text = UnitConverter.DistanceToDisplayUnits(fTurnR, eRoundMode.NERAEST).ToString();
			//Worst ==================================================================================================

			fTAS = Functions.IAS2TAS(minAircraftSpeed2, m_CurrPnt.WorstCase.GrossHeight + m_ptDerPrj.Z, GlobalVars.m_CurrADHP.ISAtC);
			fTurnR = Functions.Bank2Radius(m_BankAngle, fTAS);

			MyPC = Functions.TurnToFixPrj(m_CurrPnt.WorstCase.pPoint, fTurnR, TurnDir, WPtFix.pPtPrj);

			Segment.WorstCase.turns = 1;
			Segment.WorstCase.Turn[0].ptStart = m_CurrPnt.WorstCase.pPoint;
			Segment.WorstCase.Turn[0].ptEnd = MyPC.Point[1];
			OutDir = Segment.WorstCase.Turn[0].ptEnd.M;

			Segment.WorstCase.Turn[0].ptCenter = Functions.LocalToPrj(m_CurrPnt.WorstCase.pPoint, m_CurrPnt.WorstCase.Direction + 90.0 * TurnDir, fTurnR);
			Segment.WorstCase.Turn[0].TurnDir = -TurnDir;		//SideDef(m_CurrPnt, m_CurrPnt.M, Pt1);
			Segment.WorstCase.Turn[0].Angle = NativeMethods.Modulus((OutDir - m_CurrPnt.WorstCase.Direction) * TurnDir);
			Segment.WorstCase.Turn[0].Radius = fTurnR;
			Segment.WorstCase.Turn[0].StartDist = 0.0;

			//---------------------------------
			turnPoly = Functions.CalcTrajectoryFromMultiPoint(MyPC);
			Segment.WorstCase.Turn[0].Length = turnPoly.Length;

			//---------------------------------
			//Segment.ptEnd.WorstCase.pPoint = (IPoint)new ESRI.ArcGIS.Geometry.Point();
			//Segment.ptEnd.WorstCase.pPoint.PutCoords(WPtFix.pPtPrj.X, WPtFix.pPtPrj.Y);

			Segment.ptEnd.WorstCase.pPoint = (IPoint)new ESRI.ArcGIS.Geometry.Point();
			Segment.ptEnd.WorstCase.pPoint.PutCoords(MyPC.Point[1].X, MyPC.Point[1].Y);

			Segment.ptEnd.WorstCase.pPoint.M = OutDir;
			Segment.ptEnd.WorstCase.Direction = OutDir;

			MyPC.AddPoint(Segment.ptEnd.WorstCase.pPoint);
			//---------------------------------
			Segment.WorstCase.pNominalPoly = Functions.CalcTrajectoryFromMultiPoint(MyPC);
			Segment.WorstCase.Length = Segment.WorstCase.pNominalPoly.Length;

			//Segment.ptEnd.WorstCase.pPoint = Segment.WorstCase.Turn1.ptEnd;
			//TextBox401.Text = Math.Round(NativeMethods.Modulus(Functions.Dir2Azt(Segment.BestCase.Turn1.ptEnd, OutDir) - WPtFix.MagVar, 360.0)).ToString();
			//TextBox402.Text = Math.Round(Segment.BestCase.Turn1.Angle).ToString();
			//TextBox403.Text = UnitConverter.DistanceToDisplayUnits(fTurnR, eRoundMode.NERAEST).ToString();
			//Label406.Text = Navaids_DataBase.GetNavTypeName(WPtFix.TypeCode);
			//==================================================================================================

			Segment.ptEnd.atHeight = false;

			Segment.ptEnd.BestCase.NetHeight = Math.Min(m_CurrPnt.BestCase.NetHeight + Segment.BestCase.Length * BestMaxNetGrad2, cMaxH);
			Segment.ptEnd.BestCase.GrossHeight = Math.Min(m_CurrPnt.BestCase.GrossHeight + Segment.BestCase.Length * BestMaxGrossGrad, cMaxH);

			Segment.ptEnd.WorstCase.NetHeight = Math.Min(m_CurrPnt.WorstCase.NetHeight + Segment.WorstCase.Length * WorstMaxNetGrad2, cMaxH);
			Segment.ptEnd.WorstCase.GrossHeight = Math.Min(m_CurrPnt.WorstCase.GrossHeight + Segment.WorstCase.Length * WorstMaxGrossGrad, cMaxH);
			Segment.Tag = ComboBox401.SelectedItem;

			//string[] sTurnDir = { "вправо", "", "влево" };
			//Segment.Comment = "Прямо до " + WPtFix + " с разворотом " + sTurnDir[TurnDir + 1];

			string[] sTurnDir = { "вправо", "", "влево" };
			Segment.Comment = "Выйти на линию пути на " + WPtFix + " с разворотом " + sTurnDir[TurnDir + 1];

			Label410.Text = Segment.Comment;
			//---------------------------------------------------------------------------

			Segment.bOnWPTFIX = true;
			Segment.WptFix = WPtFix;

			Segment.SegmentCode = eLegType.directToFIX;
			Segment.PathCode = SegmentPathType.DF;

			Segment.InterceptionNav.TypeCode = eNavaidClass.CodeNONE;

			if (WPtFix.TypeCode == eNavaidClass.CodeNONE)
				Segment.GuidanceNav.TypeCode = eNavaidClass.CodeNONE;
			else
				Segment.GuidanceNav = Navaids_DataBase.WPT_FIXToNavaid(WPtFix);

			return true;
		}

		#region directToFIX events

		private void ComboBox401_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!m_bFormInitialised) return;
			if (ComboBox401.SelectedIndex < 0) return;
			Label406.Text = Navaids_DataBase.GetNavTypeName(((WPT_FIXData)ComboBox401.SelectedItem).TypeCode);
			ConstructNextSegment();
		}

		private void ComboBox402_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!m_bFormInitialised) return;
			if (ComboBox402.SelectedIndex < 0) return;

			ConstructNextSegment();
		}

		#endregion

		#endregion

		#region TurnAndIntercept

		private void doTurnAndIntercept()
		{
			TextBox506.Text = "30";

			TextBox506_Validating(TextBox506, new System.ComponentModel.CancelEventArgs());

			if (ComboBox501.SelectedIndex < 0)
				ComboBox501.SelectedIndex = 0;

			MultiPage1.SelectedIndex = 7;
		}

		private int FillComboBox501Stations()
		{
			const double MaxCoverage = 74080.0;
			const double SightCoeff = 4130.0;

			ComboBox503.Enabled = false;
			CheckBox501.CheckState = CheckState.Unchecked;

			ComboBox501.Items.Clear();
			int n = GlobalVars.WPTList.Length;

			for (int i = 0; i < n; i++)
			{
				double dHB = Math.Abs(m_CurrPnt.BestCase.NetHeight - GlobalVars.WPTList[i].pPtPrj.Z);
				double dHW = Math.Abs(m_CurrPnt.WorstCase.NetHeight - GlobalVars.WPTList[i].pPtPrj.Z);

				double BestSearchArea = Math.Min(SightCoeff * Math.Sqrt(dHB), MaxCoverage);
				double WorstSearchArea = Math.Min(SightCoeff * Math.Sqrt(dHW), MaxCoverage);

				double DistB = Functions.ReturnDistanceInMeters(m_CurrPnt.BestCase.pPoint, GlobalVars.WPTList[i].pPtPrj);
				double DistW = Functions.ReturnDistanceInMeters(m_CurrPnt.WorstCase.pPoint, GlobalVars.WPTList[i].pPtPrj);

				if (DistB < BestSearchArea && DistW < WorstSearchArea)
					ComboBox501.Items.Add(GlobalVars.WPTList[i]);
			}

			return ComboBox501.Items.Count;
		}

		private int FillComboBox503Stations()
		{
			const double MaxCoverage = 74080.0;

			ComboBox503.Items.Clear();
			int n = GlobalVars.WPTList.Length;

			for (int i = 0; i < n; i++)
			{
				IPoint pCurrPt = GlobalVars.WPTList[i].pPtPrj;
				double DistB = Functions.ReturnDistanceInMeters(m_CurrPnt.BestCase.pPoint, pCurrPt);
				double DistW = Functions.ReturnDistanceInMeters(m_CurrPnt.WorstCase.pPoint, pCurrPt);

				if (DistB < MaxCoverage && DistW < MaxCoverage)
				{
					double DirToNav = Functions.ReturnAngleInDegrees(Type5_CurNav.pPtPrj, pCurrPt);
					double AztToNav = Functions.Dir2Azt(Type5_CurNav.pPtPrj, DirToNav) - m_MagVar;

					//ComboBox503.Items.Add(GlobalVars.WPTList[i]);

					int m = Type5_Intervals.Length;
					for (int j = 0; j < m; j++)
					{
						if (Functions.AngleInSector(AztToNav, Type5_Intervals[j].Left, Type5_Intervals[j].Right))
						{
							ComboBox503.Items.Add(GlobalVars.WPTList[i]);
							break;
						}
						else if (Functions.AngleInSector(AztToNav + 180.0, Type5_Intervals[j].Left, Type5_Intervals[j].Right))
						{
							ComboBox503.Items.Add(GlobalVars.WPTList[i]);
							break;
						}
					}
				}
			}

			if (ComboBox503.Items.Count > 0)
				ComboBox503.SelectedIndex = 0;

			return ComboBox503.Items.Count;
		}

		private void UpdateType5Intervals()
		{
			if (Type5_CurNav.pPtPrj == null)
				return;

			int turnDir = 1 - (ComboBox502.SelectedIndex << 1);

			Interval bMinMaxInterval;
			Interval wMinMaxInterval;
			int i, j, n;
			int bCnt, wCnt;

			//IPolygon tmpPoly;
			//tmpPoly = Functions.CreateCirclePrj(m_CurrPnt.BestCase.pPoint, MaxTrackAbeam);
			//Graphics.DrawPolygon(tmpPoly);
			//tmpPoly = Functions.CreateCirclePrj(m_CurrPnt.WorstCase.pPoint, MaxTrackAbeam);
			//Graphics.DrawPolygon(tmpPoly);
			//Application.DoEvents();

			if (turnDir < 0)
			{
				bMinMaxInterval.Left = NativeMethods.Modulus(m_CurrPnt.BestCase.Direction - GlobalVars.MaxTurnAngle);
				bMinMaxInterval.Right = m_CurrPnt.BestCase.Direction;

				wMinMaxInterval.Left = NativeMethods.Modulus(m_CurrPnt.WorstCase.Direction - GlobalVars.MaxTurnAngle);
				wMinMaxInterval.Right = m_CurrPnt.WorstCase.Direction;
			}
			else
			{
				bMinMaxInterval.Left = m_CurrPnt.BestCase.Direction;
				bMinMaxInterval.Right = NativeMethods.Modulus(m_CurrPnt.BestCase.Direction + GlobalVars.MaxTurnAngle);

				wMinMaxInterval.Left = m_CurrPnt.WorstCase.Direction;
				wMinMaxInterval.Right = NativeMethods.Modulus(m_CurrPnt.WorstCase.Direction + GlobalVars.MaxTurnAngle);
			}

			double bDist = Functions.ReturnDistanceInMeters(m_CurrPnt.BestCase.pPoint, Type5_CurNav.pPtPrj);
			Interval[] bResultIntervals;

			if (bDist <= GlobalVars.MaxTrackAbeam)
			{
				bResultIntervals = new Interval[1];
				bResultIntervals[0] = bMinMaxInterval;
				bCnt = 1;
			}
			else
			{
				double bDir = Functions.ReturnAngleInDegrees(Type5_CurNav.pPtPrj, m_CurrPnt.BestCase.pPoint);
				double betha = Functions.RadToDeg(Math.Asin(GlobalVars.MaxTrackAbeam / bDist));

				Interval[] bIntervals = new Interval[2];

				bIntervals[0].Left = NativeMethods.Modulus(bDir - betha);
				bIntervals[0].Right = NativeMethods.Modulus(bDir + betha);

				bIntervals[1].Left = NativeMethods.Modulus(bDir - betha + 180.0);
				bIntervals[1].Right = NativeMethods.Modulus(bDir + betha + 180.0);

				bResultIntervals = new Interval[4];
				bCnt = 0;

				for (i = 0; i < 2; i++)
				{
					Interval[] tmpIntervals = Functions.CiclicIntervalsIntersection(bMinMaxInterval, bIntervals[i]);
					n = tmpIntervals.Length;
					for (j = 0; j < n; j++)
					{
						bResultIntervals[bCnt] = tmpIntervals[j];
						bCnt++;
					}
				}

				Array.Resize<Interval>(ref bResultIntervals, bCnt);
			}

			double wDist = Functions.ReturnDistanceInMeters(m_CurrPnt.WorstCase.pPoint, Type5_CurNav.pPtPrj);
			Interval[] wResultIntervals;

			if (wDist <= GlobalVars.MaxTrackAbeam)
			{
				wResultIntervals = new Interval[1];
				wResultIntervals[0] = wMinMaxInterval;
				wCnt = 1;
			}
			else
			{
				double bDir = Functions.ReturnAngleInDegrees(Type5_CurNav.pPtPrj, m_CurrPnt.WorstCase.pPoint);
				double betha = Functions.RadToDeg(Math.Asin(GlobalVars.MaxTrackAbeam / wDist));
				Interval[] wIntervals = new Interval[2];

				wIntervals[0].Left = NativeMethods.Modulus(bDir - betha);
				wIntervals[0].Right = NativeMethods.Modulus(bDir + betha);

				wIntervals[1].Left = NativeMethods.Modulus(bDir - betha + 180.0);
				wIntervals[1].Right = NativeMethods.Modulus(bDir + betha + 180.0);

				wResultIntervals = new Interval[4];

				wCnt = 0;

				for (i = 0; i < 2; i++)
				{
					Interval[] tmpIntervals = Functions.CiclicIntervalsIntersection(bMinMaxInterval, wIntervals[i]);
					n = tmpIntervals.Length;
					for (j = 0; j < n; j++)
					{
						wResultIntervals[wCnt] = tmpIntervals[j];
						wCnt++;
					}
				}

				Array.Resize<Interval>(ref wResultIntervals, wCnt);
			}

			Type5_Intervals = new Interval[bCnt + wCnt];
			n = 0;

			for (i = 0; i < bCnt; i++)
			{
				for (j = 0; j < wCnt; j++)
				{
					Interval[] tmpIntervals = Functions.CiclicIntervalsIntersection(bResultIntervals[i], wResultIntervals[j]);

					//IPolyline pPolyline = (IPolyline)new Polyline();
					//pPolyline.FromPoint = Type5_CurNav.pPtPrj;
					//pPolyline.ToPoint = Functions.LocalToPrj(Type5_CurNav.pPtPrj, bResultIntervals[0].Right, 2 * bDist);
					//Graphics.DrawPolyline(pPolyline, Functions.RGB(0,0, 255));

					//pPolyline.ToPoint = Functions.LocalToPrj(Type5_CurNav.pPtPrj, bResultIntervals[0].Left, 2 * bDist);
					//Graphics.DrawPolyline(pPolyline, Functions.RGB(0, 0, 255));

					//pPolyline.ToPoint = Functions.LocalToPrj(Type5_CurNav.pPtPrj, wResultIntervals[0].Right, 2 * bDist);
					//Graphics.DrawPolyline(pPolyline, Functions.RGB(0, 255, 0));

					//pPolyline.ToPoint = Functions.LocalToPrj(Type5_CurNav.pPtPrj, wResultIntervals[0].Left, 2 * bDist);
					//Graphics.DrawPolyline(pPolyline, Functions.RGB(0, 255, 0));

					//Application.DoEvents();

					for (int k = 0; k < tmpIntervals.Length; k++)
					{
						Type5_Intervals[n] = tmpIntervals[k];
						n++;
					}
				}
			}

			Array.Resize<Interval>(ref Type5_Intervals, n);

			for (i = 0; i < n; i++)
			{
				double fTmp = NativeMethods.Modulus(Functions.Dir2Azt(Type5_CurNav.pPtPrj, Type5_Intervals[i].Right) - m_MagVar);
				Type5_Intervals[i].Right = NativeMethods.Modulus(Functions.Dir2Azt(Type5_CurNav.pPtPrj, Type5_Intervals[i].Left) - m_MagVar);
				Type5_Intervals[i].Left = fTmp;
			}

			//===================================================================================================================================
			j = 0;

			while (j < n - 1)
				if (Functions.SubtractAngles(Type5_Intervals[j].Right, Type5_Intervals[j + 1].Left) <= 1.0)
				{
					Type5_Intervals[j].Right = Type5_Intervals[j + 1].Right;
					n--;

					for (i = j + 1; i < n; i++)
						Type5_Intervals[i] = Type5_Intervals[i + 1];
				}
				else
					j++;

			if (n > 1)
				if (Functions.SubtractAngles(Type5_Intervals[0].Left, Type5_Intervals[n - 1].Right) <= 1.0)
					Type5_Intervals[0].Left = Type5_Intervals[--n].Left;

			Array.Resize<Interval>(ref Type5_Intervals, n);

			for (i = 0; i < n; i++)
				if (Functions.SubtractAngles(Math.Round(Type5_Intervals[i].Left), Math.Round(Type5_Intervals[i].Right)) < 1.0)
				{
					Type5_Intervals[i].Left = Math.Round(Type5_Intervals[i].Left);
					Type5_Intervals[i].Right = Type5_Intervals[i].Left;
				}
				else
				{
					Type5_Intervals[i].Left = Math.Round(Type5_Intervals[i].Left + 0.4999);
					Type5_Intervals[i].Right = Math.Round(Type5_Intervals[i].Right - 0.4999);
				}

			Functions.SortIntervals(Type5_Intervals);

			string tmpStr = "";

			for (i = 0; i < n; i++)
			{
				if (Functions.SubtractAngles(Type5_Intervals[i].Left, Type5_Intervals[i].Right) <= GlobalVars.degEps)
					tmpStr = Type5_Intervals[i].Left.ToString() + "°";
				else
					tmpStr = tmpStr + "От " + Type5_Intervals[i].Left.ToString() +
										"° До " + Type5_Intervals[i].Right.ToString() + "°";

				//if(i == 0) TextBox505.Text = CStr(Type5_Intervals(0).Left);

				if (i != n - 1)
					tmpStr = tmpStr + "\n\r";
			}

			FillComboBox503Stations();
			TextBox505_Validating(TextBox505, new System.ComponentModel.CancelEventArgs());
			Label516.Text = tmpStr;
		}

		private bool Type5Segment(ref TrackLeg Segment)
		{
			double deadReck;

			Segment.ptStart = m_CurrPnt;
			int turnDir = (ComboBox502.SelectedIndex << 1) - 1;
			//==================================================================================================
			double fTAS = Functions.IAS2TAS(maxAircraftSpeed2, m_CurrPnt.BestCase.GrossHeight + m_ptDerPrj.Z, GlobalVars.m_CurrADHP.ISAtC);
			double fTurnR = Functions.Bank2Radius(m_BankAngle, fTAS);
			TextBox507.Text = UnitConverter.DistanceToDisplayUnits(fTurnR, eRoundMode.NERAEST).ToString();
			//---------------------------------
			IPointCollection MyPC = Functions.CalcTouchByFixDir(ref Segment.BestCase, m_CurrPnt.BestCase, Type5_CurNav.pPtPrj, fTurnR, Type5_CurDir, turnDir, Type5_SnapAngle, out deadReck);

			Segment.BestCase.pNominalPoly = Functions.CalcTrajectoryFromMultiPoint(MyPC);
			//Graphics.DrawPolyline(Segment.BestCase.pNominalPoly, -1);
			//Functions.ProcessMessages();

			Segment.BestCase.Length = Segment.BestCase.pNominalPoly.Length;

			Segment.ptEnd.BestCase.pPoint = Segment.BestCase.pNominalPoly.ToPoint;
			Segment.ptEnd.BestCase.Direction = Segment.ptEnd.BestCase.pPoint.M;
			//==================================================================================================

			fTAS = Functions.IAS2TAS(minAircraftSpeed2, m_CurrPnt.WorstCase.GrossHeight + m_ptDerPrj.Z, GlobalVars.m_CurrADHP.ISAtC);
			fTurnR = Functions.Bank2Radius(m_BankAngle, fTAS);
			//TextBox507.Text = UnitConverter.DistanceToDisplayUnits(fTurnR, eRoundMode.NERAEST).ToString();
			//---------------------------------

			MyPC = Functions.CalcTouchByFixDir(ref Segment.WorstCase, m_CurrPnt.WorstCase, Type5_CurNav.pPtPrj, fTurnR, Type5_CurDir, turnDir, Type5_SnapAngle, out deadReck);
			Segment.WorstCase.pNominalPoly = Functions.CalcTrajectoryFromMultiPoint(MyPC);
			//Graphics.DrawPolyline(Segment.WorstCase.pNominalPoly, -1);
			//Functions.ProcessMessages();

			Segment.WorstCase.Length = Segment.WorstCase.pNominalPoly.Length;

			Segment.ptEnd.WorstCase.pPoint = Segment.WorstCase.pNominalPoly.ToPoint;
			Segment.ptEnd.WorstCase.Direction = Segment.ptEnd.WorstCase.pPoint.M;

			//==================================================================================================

			Segment.ptEnd.atHeight = false;

			Segment.ptEnd.BestCase.NetHeight = Math.Min(m_CurrPnt.BestCase.NetHeight + Segment.BestCase.Length * BestMaxNetGrad2, cMaxH);
			Segment.ptEnd.BestCase.GrossHeight = Math.Min(m_CurrPnt.BestCase.GrossHeight + Segment.BestCase.Length * BestMaxGrossGrad, cMaxH);

			Segment.ptEnd.WorstCase.NetHeight = Math.Min(m_CurrPnt.WorstCase.NetHeight + Segment.WorstCase.Length * WorstMaxNetGrad2, cMaxH);
			Segment.ptEnd.WorstCase.GrossHeight = Math.Min(m_CurrPnt.WorstCase.GrossHeight + Segment.WorstCase.Length * WorstMaxGrossGrad, cMaxH);

			int iToFacility = (int)Functions.SideDef(MyPC.Point[MyPC.PointCount - 1], Type5_CurDir + 90.0, Type5_CurNav.pPtPrj);
			string toFrom;
			if (iToFacility > 0)
				toFrom = "от ";
			else
				toFrom = "на ";

			Label520.Text = toFrom + "РНС";

			TextBox503.Text = UnitConverter.DistanceToDisplayUnits(deadReck, eRoundMode.NERAEST).ToString();

			double fCourse = NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(Type5_CurNav.pPtPrj, Type5_CurDir) - m_MagVar, 2));
			TextBox505.Text = fCourse.ToString();

			string[] sTurnDir = { "влево", "", "вправо" };
			string sAzt = System.Math.Round(fCourse).ToString();

			while (sAzt.Length < 3)
				sAzt = "0" + sAzt;

			Segment.Comment = "Разворот " + sTurnDir[turnDir + 1] + " и перехват линии пути " + sAzt + "° " + toFrom + Type5_CurNav;
			Label523.Text = Segment.Comment;

			//------------------------------------------------------------------------------
			Segment.bOnWPTFIX = false;
			Segment.SegmentCode = eLegType.turnAndIntercept;
			Segment.PathCode = SegmentPathType.CF;
			//Segment.PathCode = Aim.Enums.CodeSegmentPath.CI;

			Segment.InterceptionNav.TypeCode = eNavaidClass.CodeNONE;

			//Segment.
			Segment.GuidanceNav = Navaids_DataBase.WPT_FIXToNavaid(Type5_CurNav);		//.TypeCode = eNavaidClass.CodeNONE;		//

			return true;
		}

		#region turnAndIntercept events

		private void TextBox501_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;

			if (e.KeyChar == 13)
				TextBox505_Validating(TextBox505, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxInteger(ref eventChar);
			//Functions.TextBoxFloat(ref eventChar, TextBox505.Text);

			e.KeyChar = eventChar;
			if (e.KeyChar == 0)
				e.Handled = true;
		}

		private void TextBox505_Validating(object sender, CancelEventArgs e)
		{
			double fCourse;

			if (!double.TryParse(TextBox505.Text, out fCourse))
				return;

			int i, j = 0, n = Type5_Intervals.Length;
			bool inRange = false, leftSide = false;
			double MaxDx = 360.0;
			for (i = 0; i < n; i++)
			{
				if (Functions.AngleInSector(fCourse, Type5_Intervals[i].Left, Type5_Intervals[i].Right))
					inRange = true;
				else
				{
					double dl = Functions.SubtractAngles(Type5_Intervals[i].Left, fCourse);
					double dr = Functions.SubtractAngles(Type5_Intervals[i].Right, fCourse);
					if (dl < dr)
					{
						if (dl < MaxDx)
						{
							MaxDx = dl;
							j = i;
							leftSide = true;
						}
					}
					else
					{
						if (dr < MaxDx)
						{
							MaxDx = dr;
							j = i;
							leftSide = false;
						}
					}
				}
			}

			if (!inRange)
			{
				if (leftSide)
					fCourse = Type5_Intervals[j].Left;
				else
					fCourse = Type5_Intervals[j].Right;

				TextBox505.Text = System.Math.Round(fCourse, 2).ToString();
			}

			Type5_CurDir = Functions.Azt2Dir(Type5_CurNav.pPtGeo, fCourse + m_MagVar);

			ConstructNextSegment();
		}

		private void TextBox506_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;

			if (e.KeyChar == 13)
				TextBox506_Validating(TextBox506, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxInteger(ref eventChar);
			//Functions.TextBoxFloat(ref eventChar, TextBox506.Text);

			e.KeyChar = eventChar;
			if (e.KeyChar == 0)
				e.Handled = true;
		}

		private void TextBox506_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;
			if (!double.TryParse(TextBox506.Text, out fTmp))
				return;

			Type5_SnapAngle = fTmp;
			if (Type5_CurNav.pPtPrj != null)
				ConstructNextSegment();
		}

		private void CheckBox501_CheckedChanged(object sender, EventArgs e)
		{
			if (!CheckBox501.Checked)
			{
				ComboBox503.Enabled = false;
				TextBox505.ReadOnly = false;
				TextBox505.BackColor = System.Drawing.SystemColors.Window;
				Label519.Enabled = false;
			}
			else
			{
				ComboBox503.Enabled = true;
				TextBox505.ReadOnly = true;
				TextBox505.BackColor = System.Drawing.SystemColors.Control;
				Label519.Enabled = true;

				ComboBox503_SelectedIndexChanged(ComboBox503, new EventArgs());

				//ComboBox503.Items.Clear();
				//int n = GlobalVars.WPTList.Length;

				//int j = -1;

				//for (int i = 0; i < n; i++)
				//{
				//    double dH = Math.Abs(m_CurrPnt.BestCase.NetHeight - GlobalVars.WPTList[i].pPtPrj.Z);
				//    double MaxDist = 4130.0 * Math.Sqrt(dH);

				//    if (GlobalVars.WPTList[i].Name != ComboBox501.Text && Functions.ReturnDistanceInMeters(m_CurrPnt.BestCase.pPoint, GlobalVars.WPTList[i].pPtPrj) < MaxDist)
				//    {
				//        j++;
				//        ComboBox503.Items.Add(GlobalVars.WPTList[i]);
				//    }
				//}

				//if (ComboBox503.Items.Count > 0)
				//    ComboBox503.SelectedIndex = 0;
			}
		}

		private void ComboBox501_Click(object sender, EventArgs e)
		{
			//
		}

		private void ComboBox501_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ComboBox501.SelectedIndex < 0)
				return;

			Type5_CurNav = (WPT_FIXData)ComboBox501.SelectedItem;
			Label512.Text = Navaids_DataBase.GetNavTypeName(Type5_CurNav.TypeCode);

			UpdateType5Intervals();
		}

		private void ComboBox502_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!m_bFormInitialised)
				return;
			if (ComboBox502.SelectedIndex < 0)
				return;

			UpdateType5Intervals();
		}

		private void ComboBox503_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ComboBox503.SelectedIndex < 0)
				return;
			if (!CheckBox501.Checked)
				return;

			WPT_FIXData DirNav = (WPT_FIXData)ComboBox503.SelectedItem;

			Type5_CurDir = Functions.ReturnAngleInDegrees(Type5_CurNav.pPtPrj, DirNav.pPtPrj);
			ConstructNextSegment();
		}

		#endregion

		#endregion

		#region ArcIntercept

		private void doArcIntercept()
		{
			FillComboBox603DMEStations();
			if (ComboBox603.Items.Count > 0)
				ComboBox603.SelectedIndex = 0;
			MultiPage1.SelectedIndex = 8;
		}

		private int FillComboBox603DMEStations()
		{
			double CosA1 = -2.0;
			double CosA2 = 1;

			//if (!double.TryParse(TextBox605.Text, out fTmp))
			//{
			//}
			//double R = UnitConverter.DistanceToInternalUnits(fTmp);

			ComboBox603.Items.Clear();

			int n = GlobalVars.DMEList.Length;
			Type6_Intervals = new Interval[n];

			int Side = (2 * ComboBox601.SelectedIndex - 1);		//(SideDirection)
			int TurnDir = (2 * ComboBox602.SelectedIndex - 1);	//(TurnDirection)
			int j = 0;

			double fTASB = Functions.IAS2TAS(maxAircraftSpeed2, m_CurrPnt.BestCase.GrossHeight + m_ptDerPrj.Z, GlobalVars.m_CurrADHP.ISAtC);
			double fTurnRB = Functions.Bank2Radius(m_BankAngle, fTASB);

			double fTASW = Functions.IAS2TAS(minAircraftSpeed2, m_CurrPnt.WorstCase.GrossHeight + m_ptDerPrj.Z, GlobalVars.m_CurrADHP.ISAtC);
			double fTurnRW = Functions.Bank2Radius(m_BankAngle, fTASB);

			for (int i = 0; i < n; i++)
			{
				double minR1R2, maxR1R2;

				IPoint pPtRef = GlobalVars.DMEList[i].pPtPrj;
				double phi = Functions.ReturnAngleInDegrees(m_CurrPnt.BestCase.pPoint, pPtRef);
				double L = Functions.ReturnDistanceInMeters(m_CurrPnt.BestCase.pPoint, pPtRef);

				double Bheta = m_CurrPnt.BestCase.Direction - phi;

				double R11 = CosA1 * (Side * fTurnRB - TurnDir * L * Math.Sin(GlobalVars.DegToRadValue * Bheta)) - Side * fTurnRB;			//if Cos(A1) = -0.5
				double R12 = -1000000.0 * (Side * fTurnRB - TurnDir * L * Math.Sin(GlobalVars.DegToRadValue * Bheta)) - Side * fTurnRB;		//if Cos(A1) = -0.5

				double R21 = CosA2 * (Side * fTurnRB - TurnDir * L * Math.Sin(GlobalVars.DegToRadValue * Bheta)) - Side * fTurnRB;			//if Cos(A2) = 1
				double R22 = 1000000.0 * (Side * fTurnRB - TurnDir * L * Math.Sin(GlobalVars.DegToRadValue * Bheta)) - Side * fTurnRB;		//if Cos(A2) = 1

				if (R12 < 0.0)
				{
					maxR1R2 = Math.Max(R21, R22);
					minR1R2 = Math.Min(R21, R22);
				}
				else
				{
					maxR1R2 = Math.Max(R11, R12);
					minR1R2 = Math.Min(R11, R12);
				}

				//        If (B > -90) And (B < 90) Then
				//        End If

				if (minR1R2 < rDMEMax && maxR1R2 > rDMEMin)
				{
					double R2 = Math.Min(maxR1R2, rDMEMax);
					double R1 = Math.Max(minR1R2, rDMEMin);

					if (Side > 0.0)
					{
						double fTmp = L - fTurnRB * 1.5;
						if (R2 > fTmp) R2 = fTmp;
						if (Math.Cos(GlobalVars.DegToRadValue * Bheta) < 0) R2 = R1;
					}
					else
					{
						double fTmp = L + fTurnRB * 1.5;
						if (R1 < fTmp) R1 = fTmp;
					}

					if (R1 < R2)
					{
						ComboBox603.Items.Add(GlobalVars.DMEList[i]);

						Type6_Intervals[j].Left = R1;
						Type6_Intervals[j].Right = R2;
						j++;
					}
				}
			}

			return j;
		}

		private bool Type6Segment(ref TrackLeg Segment)
		{
			double rDME = UnitConverter.DistanceToInternalUnits(double.Parse(TextBox605.Text));
			double dDME = Functions.ReturnDistanceInMeters(m_CurrPnt.BestCase.pPoint, Type6_CurNav.pPtPrj);

			int Side = (ComboBox601.SelectedIndex << 1) - 1;
			int ArcDir = (ComboBox602.SelectedIndex << 1) - 1;
			Segment.ptStart = m_CurrPnt;

			//==================================================================================================
			Segment.BestCase.turns = 1;
			Segment.BestCase.Turn[1].Radius = rDME;
			Segment.BestCase.Turn[1].TurnDir = (ComboBox602.SelectedIndex << 1) - 1;
			Segment.BestCase.Turn[1].ptCenter = Type6_CurNav.pPtPrj;

			double phi = Functions.ReturnAngleInDegrees(m_CurrPnt.BestCase.pPoint, Type6_CurNav.pPtPrj);
			double Bheta = m_CurrPnt.BestCase.Direction - phi;

			double fY = dDME * System.Math.Sin(Functions.DegToRad(Bheta));
			double fX = dDME * System.Math.Cos(Functions.DegToRad(Bheta));

			///double fTrX = Math.Sqrt(dDME * dDME - fY * fY) ;
			double TurnDist = fX - Side * Math.Sqrt(rDME * rDME - fY * fY);
			double turnAlt = m_CurrPnt.BestCase.GrossHeight + TurnDist * BestMaxGrossGrad;

			double fTAS = Functions.IAS2TAS(maxAircraftSpeed2, turnAlt + m_ptDerPrj.Z, GlobalVars.m_CurrADHP.ISAtC);
			double fTurnR = Functions.Bank2Radius(m_BankAngle, fTAS);
			TextBox606.Text = UnitConverter.DistanceToDisplayUnits(fTurnR, eRoundMode.NERAEST).ToString();

			double fTmp = (Side * fTurnR - ArcDir * dDME * System.Math.Sin(Functions.DegToRad(Bheta))) / (rDME + Side * fTurnR);
			if (Math.Abs(fTmp) - 1 > 0 && Math.Abs(fTmp) - 1 < 0.001)
				fTmp = Math.Sign(fTmp);

			double alpha = Functions.RadToDeg(Math.Acos(fTmp));
			double Gamma = m_CurrPnt.BestCase.Direction + Side * ArcDir * (90.0 - alpha) + 90.0 * (1 + Side);

			double Lp = rDME + Side * fTurnR;

			//Xe = (yt - y0) * Sin(t) * Cos(t) + xt * Cos(t) ^ 2 + X0 * Sin(t) ^ 2
			//Ye = y0 + (Xe - X0) * tg(t)

			Segment.BestCase.Turn[0].ptCenter = Functions.LocalToPrj(Type6_CurNav.pPtPrj, Gamma, Lp);

			Segment.BestCase.Turn[0].ptStart = Functions.LocalToPrj(Segment.BestCase.Turn[0].ptCenter, m_CurrPnt.BestCase.Direction + 90.0 * Side * ArcDir, fTurnR);
			Segment.BestCase.Turn[0].ptStart.M = m_CurrPnt.BestCase.Direction;

			Segment.BestCase.Turn[0].ptEnd = Functions.LocalToPrj(Type6_CurNav.pPtPrj, Gamma, Lp - Side * fTurnR);
			Segment.BestCase.Turn[0].ptEnd.M = Gamma + 90.0 * ArcDir;

			Segment.BestCase.Turn[0].TurnDir = -(int)Functions.SideDef(Segment.BestCase.Turn[0].ptStart, Segment.BestCase.Turn[0].ptStart.M, Segment.BestCase.Turn[0].ptEnd);
			Segment.BestCase.Turn[0].Angle = NativeMethods.Modulus((Segment.BestCase.Turn[0].ptStart.M - Segment.BestCase.Turn[0].ptEnd.M) * Segment.BestCase.Turn[0].TurnDir);
			Segment.BestCase.Turn[0].Radius = fTurnR;

			//---------------------------------

			IPointCollection pPC = new ESRI.ArcGIS.Geometry.Polyline();
			pPC.AddPoint(Segment.BestCase.Turn[0].ptStart);
			pPC.AddPoint(Segment.BestCase.Turn[0].ptEnd);

			IPolyline pPoly = Functions.CalcTrajectoryFromMultiPoint(pPC);

			Segment.BestCase.Turn[0].Length = pPoly.Length;
			Segment.BestCase.Turn[0].StartDist = Functions.ReturnDistanceInMeters(Segment.BestCase.Turn[0].ptStart, m_CurrPnt.BestCase.pPoint);
			//Segment.Turn1.HeightAtEnd = m_NetHStart + (Functions.ReturnDistanceInMeters(m_CurrPnt, Segment.Turn1.ptStart) + pPoly.Length) * maxNetGrad;
			//GrossHAtEnd1 = m_GrossHStart + (Functions.ReturnDistanceInMeters(m_CurrPnt, ...) + pPoly.Length) * ultramaxGrossGrad;

			//---------------------------------
			IPointCollection MyPC = new ESRI.ArcGIS.Geometry.Multipoint();
			MyPC.AddPoint(m_CurrPnt.BestCase.pPoint);
			MyPC.AddPoint(Segment.BestCase.Turn[0].ptStart);
			MyPC.AddPoint(Segment.BestCase.Turn[0].ptEnd);

			Segment.BestCase.pNominalPoly = Functions.CalcTrajectoryFromMultiPoint(MyPC);

			Segment.BestCase.Length = Segment.BestCase.pNominalPoly.Length;

			Segment.ptEnd.BestCase.pPoint = Segment.BestCase.Turn[0].ptEnd;
			Segment.ptEnd.BestCase.Direction = Segment.BestCase.Turn[0].ptEnd.M;
			//==================================================================================================

			Segment.WorstCase.turns = 1;
			Segment.WorstCase.Turn[1].Radius = rDME;
			Segment.WorstCase.Turn[1].TurnDir = (ComboBox602.SelectedIndex << 1) - 1;
			Segment.WorstCase.Turn[1].ptCenter = Type6_CurNav.pPtPrj;

			phi = Functions.ReturnAngleInDegrees(m_CurrPnt.WorstCase.pPoint, Type6_CurNav.pPtPrj);
			Bheta = m_CurrPnt.WorstCase.Direction - phi;

			fY = dDME * System.Math.Sin(Functions.DegToRad(Bheta));
			fX = dDME * System.Math.Cos(Functions.DegToRad(Bheta));

			///double fTrX = Math.Sqrt(dDME * dDME - fY * fY) ;
			TurnDist = fX - Side * Math.Sqrt(rDME * rDME - fY * fY);
			turnAlt = m_CurrPnt.WorstCase.GrossHeight + TurnDist * WorstMaxGrossGrad;

			fTAS = Functions.IAS2TAS(minAircraftSpeed2, turnAlt + m_ptDerPrj.Z, GlobalVars.m_CurrADHP.ISAtC);
			fTurnR = Functions.Bank2Radius(m_BankAngle, fTAS);
			TextBox606.Text = UnitConverter.DistanceToDisplayUnits(fTurnR, eRoundMode.NERAEST).ToString();

			fTmp = (Side * fTurnR - ArcDir * dDME * System.Math.Sin(Functions.DegToRad(Bheta))) / (rDME + Side * fTurnR);
			if (Math.Abs(fTmp) - 1 > 0 && Math.Abs(fTmp) - 1 < 0.001)
				fTmp = Math.Sign(fTmp);

			alpha = Functions.RadToDeg(Math.Acos(fTmp));
			Gamma = m_CurrPnt.WorstCase.Direction + Side * ArcDir * (90.0 - alpha) + 90.0 * (1 + Side);

			Lp = rDME + Side * fTurnR;

			//Xe = (yt - y0) * Sin(t) * Cos(t) + xt * Cos(t) ^ 2 + X0 * Sin(t) ^ 2
			//Ye = y0 + (Xe - X0) * tg(t)

			Segment.WorstCase.Turn[0].ptCenter = Functions.LocalToPrj(Type6_CurNav.pPtPrj, Gamma, Lp);

			Segment.WorstCase.Turn[0].ptStart = Functions.LocalToPrj(Segment.WorstCase.Turn[0].ptCenter, m_CurrPnt.WorstCase.Direction + 90.0 * Side * ArcDir, fTurnR);
			Segment.WorstCase.Turn[0].ptStart.M = m_CurrPnt.WorstCase.Direction;

			Segment.WorstCase.Turn[0].ptEnd = Functions.LocalToPrj(Type6_CurNav.pPtPrj, Gamma, Lp - Side * fTurnR);
			Segment.WorstCase.Turn[0].ptEnd.M = Gamma + 90.0 * ArcDir;

			Segment.WorstCase.Turn[0].TurnDir = -(int)Functions.SideDef(Segment.WorstCase.Turn[0].ptStart, Segment.WorstCase.Turn[0].ptStart.M, Segment.WorstCase.Turn[0].ptEnd);
			Segment.WorstCase.Turn[0].Angle = NativeMethods.Modulus((Segment.WorstCase.Turn[0].ptStart.M - Segment.WorstCase.Turn[0].ptEnd.M) * Segment.WorstCase.Turn[0].TurnDir);
			Segment.WorstCase.Turn[0].Radius = fTurnR;

			//---------------------------------

			pPC = new ESRI.ArcGIS.Geometry.Polyline();
			pPC.AddPoint(Segment.WorstCase.Turn[0].ptStart);
			pPC.AddPoint(Segment.WorstCase.Turn[0].ptEnd);

			pPoly = Functions.CalcTrajectoryFromMultiPoint(pPC);

			Segment.WorstCase.Turn[0].Length = pPoly.Length;
			Segment.WorstCase.Turn[0].StartDist = Functions.ReturnDistanceInMeters(Segment.WorstCase.Turn[0].ptStart, m_CurrPnt.WorstCase.pPoint);

			//---------------------------------
			MyPC = new ESRI.ArcGIS.Geometry.Multipoint();
			MyPC.AddPoint(m_CurrPnt.WorstCase.pPoint);
			MyPC.AddPoint(Segment.WorstCase.Turn[0].ptStart);
			MyPC.AddPoint(Segment.WorstCase.Turn[0].ptEnd);

			Segment.WorstCase.pNominalPoly = Functions.CalcTrajectoryFromMultiPoint(MyPC);

			Segment.WorstCase.Length = Segment.WorstCase.pNominalPoly.Length;

			Segment.ptEnd.WorstCase.pPoint = Segment.WorstCase.Turn[0].ptEnd;
			Segment.ptEnd.WorstCase.Direction = Segment.WorstCase.Turn[0].ptEnd.M;
			//==================================================================================================
			Segment.ptEnd.atHeight = false;

			CurrSegment.ptEnd.BestCase.NetHeight = Math.Min(m_CurrPnt.BestCase.NetHeight + Segment.BestCase.Length * BestMaxNetGrad2, cMaxH);
			CurrSegment.ptEnd.BestCase.GrossHeight = Math.Min(m_CurrPnt.BestCase.GrossHeight + Segment.BestCase.Length * BestMaxGrossGrad, cMaxH);

			CurrSegment.ptEnd.WorstCase.NetHeight = Math.Min(m_CurrPnt.WorstCase.NetHeight + Segment.WorstCase.Length * WorstMaxNetGrad2, cMaxH);
			CurrSegment.ptEnd.WorstCase.GrossHeight = Math.Min(m_CurrPnt.WorstCase.GrossHeight + Segment.WorstCase.Length * WorstMaxGrossGrad, cMaxH);

			//string[] sTurnDir = { "вправо", "", "влево" };
			//double azt = NativeMethods.Modulus(Functions.Dir2Azt(Segment.BestCase.Turn[0].ptEnd, OutDir) - GlobalVars.m_CurrADHP.MagVar);
			//string aztStr = Math.Round(azt).ToString();

			//while (aztStr.Length < 3)
			//    aztStr = "0" + aztStr;
			//aztStr = aztStr + "°";


			//if (prevSegType == LegType.courseIntercept || prevSegType == LegType.turnAndIntercept)
			//    Segment.Comment = "по линии пути " + aztStr + ", на " + UnitConverter.HeightToDisplayUnits(CurrSegment.ptEnd.WorstCase.NetHeight, eRoundMode.NERAEST) + " " + UnitConverter.HeightUnit;
			//else
			//    Segment.Comment = "по направлению " + aztStr + ", на " + UnitConverter.HeightToDisplayUnits(CurrSegment.ptEnd.WorstCase.NetHeight, eRoundMode.NERAEST) + " " + UnitConverter.HeightUnit;

			//if (prevSegType == LegType.courseIntercept || prevSegType == LegType.turnAndIntercept)

			string[] sTurnDir = { "вправо", "", "влево" };

			double azt = NativeMethods.Modulus(m_CurrAzt - GlobalVars.m_CurrADHP.MagVar);
			string aztStr = Math.Round(azt).ToString();

			while (aztStr.Length < 3)
				aztStr = "0" + aztStr;
			aztStr = aztStr + "°";

			Segment.Comment = "По линии пути " + aztStr + " до перехвата дуги " + TextBox605.Text + " " + Label613.Text + " " + Type6_CurNav + " DME с разворотом " + sTurnDir[ArcDir + 1];		//LoadResString(608) + ComboBox601.Text + LoadResString(522) + ComboBox603.Text
			Label616.Text = Segment.Comment;

			//------------------------------------------------------------------
			Segment.bOnWPTFIX = false;

			//switch (PrevSegment.PathCode)
			//{
			//    case SegmentPathType.TF:
			//    case SegmentPathType.CA:
			//    case SegmentPathType.CD:
			//    case SegmentPathType.CI:
			//    case SegmentPathType.CR:
			//    case SegmentPathType.CF:
			//    case SegmentPathType.DF:
			//        Segment.PathCode = SegmentPathType.CD;
			//        break;
			//    default:
			//        Segment.PathCode = SegmentPathType.VD;
			//        break;
			//}
			Segment.PathCode = SegmentPathType.CI;

			if (PrevSegment.SegmentCode == eLegType.arcPath)
				Segment.GuidanceNav = PrevSegment.InterceptionNav;
			else
				Segment.GuidanceNav = PrevSegment.GuidanceNav;

			Segment.InterceptionNav = Type6_CurNav;

			Segment.SegmentCode = eLegType.arcIntercept;

			return true;
		}

		#region arcIntercept events

		private void TextBox605_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;

			if (e.KeyChar == 13)
				TextBox605_Validating(TextBox605, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, TextBox605.Text);

			e.KeyChar = eventChar;
			if (e.KeyChar == 0)
				e.Handled = true;
		}

		private void TextBox605_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;
			if (!double.TryParse(TextBox605.Text, out fTmp))
				return;

			double NewVal = UnitConverter.DistanceToInternalUnits(fTmp);

			int k = ComboBox603.SelectedIndex;

			if (NewVal < Type6_Intervals[k].Left)
				NewVal = Type6_Intervals[k].Left;
			else if (NewVal > Type6_Intervals[k].Right)
				NewVal = Type6_Intervals[k].Right;

			if (NewVal != fTmp)
				TextBox605.Text = UnitConverter.DistanceToDisplayUnits(NewVal, eRoundMode.NERAEST).ToString();

			ConstructNextSegment();
		}

		private void ComboBox601_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!m_bFormInitialised)
				return;

			FillComboBox603DMEStations();
			if (ComboBox603.Items.Count > 0)
				ComboBox603.SelectedIndex = 0;
		}

		private void ComboBox602_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!m_bFormInitialised)
				return;

			FillComboBox603DMEStations();
			if (ComboBox603.Items.Count > 0)
				ComboBox603.SelectedIndex = 0;
		}

		private void ComboBox603_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!m_bFormInitialised)
				return;

			int k = ComboBox603.SelectedIndex;
			if (k < 0)
				return;

			Type6_CurNav = (NavaidData)ComboBox603.SelectedItem;
			//ToolTip1.SetToolTip(TextBox605, "Min: " + UnitConverter.DistanceToDisplayUnits(Type6_Intervals(k).Left_Renamed, eRoundMode.NERAEST).ToString() +
			//								" Max: " + UnitConverter.DistanceToDisplayUnits(Type6_Intervals(k).Right_Renamed, eRoundMode.NERAEST).ToString());
			TextBox605_Validating(TextBox605, new System.ComponentModel.CancelEventArgs());
		}
		#endregion

		#endregion

		#region ArcPath

		private void doArcPath()
		{
			//										PrevSegment //???????????
			FillComboBox701Stations(LegList[LegCount - 1]);

			if (OptionButton701.Checked)
				OptionButton701_CheckedChanged(OptionButton701, new System.EventArgs());
			else
				OptionButton701.Checked = true;

			MultiPage1.SelectedIndex = 9;
		}

		private void FillComboBox701Stations(TrackLeg PrevSegment)
		{
			if (PrevSegment.SegmentCode != eLegType.arcIntercept)
				return;

			ComboBox701.Items.Clear();

			int n = GlobalVars.NavaidList.Length;
			//ComboBox701List = new NavaidType[n];
			Type7_Intervals = new Interval[n];

			int ExitSide = 1 - 2 * ComboBox702.SelectedIndex;
			double CosaMax = -0.5;
			int j = 0;

			for (int i = 0; i < n; i++)
			{
				double minInter, maxInter;
				double minAngle, maxAngle;
				double L = Functions.ReturnDistanceInMeters(GlobalVars.NavaidList[i].pPtPrj, PrevSegment.BestCase.Turn[1].ptCenter);

				double TurnDist = 0;
				double turnAlt = m_CurrPnt.BestCase.GrossHeight + TurnDist * BestMaxGrossGrad;

				double fTAS = Functions.IAS2TAS(maxAircraftSpeed2, turnAlt + m_ptDerPrj.Z, GlobalVars.m_CurrADHP.ISAtC);
				double fTurnR = Functions.Bank2Radius(m_BankAngle, fTAS);

				if (PrevSegment.BestCase.Turn[1].Radius < L)
					continue;

				if (PrevSegment.BestCase.Turn[1].TurnDir > 0)
				{
					if (PrevSegment.BestCase.Turn[1].Radius < L)
					{
						minInter = Functions.RadToDeg(Math.Asin(-PrevSegment.BestCase.Turn[1].Radius / L));
						maxInter = 180.0 - minInter;
					}
					else
					{
						minInter = 0.0;
						maxInter = 360.0;
					}

					double fTmp = ExitSide * fTurnR - (PrevSegment.BestCase.Turn[1].Radius + ExitSide * PrevSegment.BestCase.Turn[1].Radius) * CosaMax;
					if (Math.Abs(fTmp) < L)
					{
						maxAngle = Functions.RadToDeg(Math.Asin(fTmp / L));
						minAngle = -180.0 - maxAngle;
					}
					else
					{
						minAngle = 0.0;
						maxAngle = 360.0;
					}
				}
				else
				{
					if (PrevSegment.BestCase.Turn[1].Radius < L)
					{
						maxInter = Functions.RadToDeg(Math.Asin(PrevSegment.BestCase.Turn[1].Radius / L));
						minInter = -180.0 - maxInter;
					}
					else
					{
						minInter = 0.0;
						maxInter = 360.0;
					}

					double fTmp = -ExitSide * fTurnR + (PrevSegment.BestCase.Turn[1].Radius + ExitSide * PrevSegment.BestCase.Turn[1].Radius) * CosaMax;
					if (Math.Abs(fTmp) < L)
					{
						minAngle = Functions.RadToDeg(Math.Asin(fTmp / L));
						maxAngle = 180.0 - minAngle;
					}
					else
					{
						minAngle = 0.0;
						maxAngle = 360.0;
					}
				}

				if (maxInter < minInter)
					maxInter += 360.0;
				if (maxAngle < minAngle)
					maxAngle += 360.0;

				double MinOut = Math.Max(minInter, minAngle);
				double MaxOut = Math.Min(maxInter, maxAngle);

				if (MinOut < MaxOut)
				{
					double phi = Functions.ReturnAngleInDegrees(GlobalVars.NavaidList[i].pPtPrj, PrevSegment.BestCase.Turn[1].ptCenter);
					//ComboBox701List[j] = GlobalVars.NavaidList[i];
					ComboBox701.Items.Add(GlobalVars.NavaidList[i]);

					if (MinOut == 0.0 && MaxOut == 360.0)
					{
						Type7_Intervals[j].Left = 0.0;
						Type7_Intervals[j].Right = 360.0;
					}
					else
					{
						Type7_Intervals[j].Left = NativeMethods.Modulus(phi + MinOut, 360.0);
						Type7_Intervals[j].Right = NativeMethods.Modulus(phi + MaxOut, 360.0);
					}
					j++;
				}
			}

			//Array.Resize<NavaidType>(ref ComboBox701List, j);
			//Array.Resize<Interval>(ref Type7_Intervals, j);

			if (ComboBox701.Items.Count > 0)
				ComboBox701.SelectedIndex = 0;
		}

		private void FillComboBox703List()
		{
			ComboBox703.Items.Clear();
			//int n = GlobalVars.WPTList.Length;
			int n = GlobalVars.NavaidList.Length;

			int k = ComboBox701.SelectedIndex;

			for (int i = 0; i < n; i++)
			{
				double L = Functions.ReturnDistanceInMeters(GlobalVars.NavaidList[i].pPtPrj, Type7_CurNav.pPtPrj);

				//if (GlobalVars.WPTList[i].Name != ComboBox701.Text)
				if (L > 100.0 && L < 0.5 * GlobalVars.MaxNAVDist)
				{
					double NewVal = Functions.ReturnAngleInDegrees(Type7_CurNav.pPtPrj, GlobalVars.NavaidList[i].pPtPrj);

					if (Type7_Intervals[k].Left == 0.0 && Type7_Intervals[k].Right == 360.0)
						ComboBox703.Items.Add(GlobalVars.NavaidList[i]);
					else if (Functions.AngleInSector(NewVal, Type7_Intervals[k].Left, Type7_Intervals[k].Right))
						ComboBox703.Items.Add(GlobalVars.NavaidList[i]);
					else if (Functions.AngleInSector(NewVal + 180.0, Type7_Intervals[k].Left, Type7_Intervals[k].Right))
						ComboBox703.Items.Add(GlobalVars.NavaidList[i]);
				}
			}

			//Array.Resize<WPT_FIXType>(ref ComboBox703List, j);

			if (ComboBox703.Items.Count > 0)
			{
				OptionButton702.Enabled = true;
				ComboBox703.SelectedIndex = 0;
			}
			else
			{
				OptionButton701.Checked = true;
				OptionButton702.Enabled = false;
			}
		}

		private bool Type7Segment(NavaidData pNav, TrackLeg PrevSegment, ref TrackLeg Segment)
		{
			double ExitDir = Functions.Azt2Dir(Type7_CurNav.pPtGeo, double.Parse(TextBox703.Text) + m_MagVar);
			int ExitSide = 1 - 2 * ComboBox702.SelectedIndex;
			Segment.ptStart = m_CurrPnt;

			//==================================================================================================
			double DirToArcCenter = Functions.ReturnAngleInDegrees(Type7_CurNav.pPtPrj, PrevSegment.BestCase.Turn[1].ptCenter);
			double DistToArcCenter = Functions.ReturnDistanceInMeters(Type7_CurNav.pPtPrj, PrevSegment.BestCase.Turn[1].ptCenter);
			double betta = Functions.DegToRad(ExitDir - DirToArcCenter);

			// ExitDir + turn*(90°- TurnAngle) + 180 или более универсально,
			// ExitDir + ExitSide * PrevSegment.turn2.TurnDir * (90 - TurnAngle) + 90 * (1 + ExitSide)
			//? - uchushun direksion istiqametidir. ? - donme bucagidir.
			//? = arcos{(side*r -turn* L*sin?)/(R + side*r)}

			if (Segment.ptStart.BestCase.Width < 1852.0)
				Segment.ptStart.BestCase.Width = 1852.0;
			if (Segment.ptEnd.BestCase.Width < 1852.0)
				Segment.ptEnd.BestCase.Width = 1852.0;

			double TurnDist = 0;
			double turnAlt = m_CurrPnt.BestCase.GrossHeight + TurnDist * BestMaxGrossGrad;

			double fTAS = Functions.IAS2TAS(maxAircraftSpeed2, turnAlt + m_ptDerPrj.Z, GlobalVars.m_CurrADHP.ISAtC);
			double fTurnR = Functions.Bank2Radius(m_BankAngle, fTAS);

			double fTmp = (ExitSide * fTurnR - PrevSegment.BestCase.Turn[1].TurnDir * DistToArcCenter * System.Math.Sin(betta)) / (PrevSegment.BestCase.Turn[1].Radius + ExitSide * fTurnR);

			if (Math.Abs(fTmp) - 1 > 0 && Math.Abs(fTmp) - 1 < 0.001)
				fTmp = Math.Sign(fTmp);

			double TurnAngle = Functions.RadToDeg(Math.Acos(fTmp));
			double startRad = ExitDir - ExitSide * PrevSegment.BestCase.Turn[1].TurnDir * (90.0 - TurnAngle) + 90.0 * (1 - ExitSide);

			Segment.BestCase.turns = 2;
			Segment.BestCase.Turn[0].ptStart = m_CurrPnt.BestCase.pPoint;
			Segment.BestCase.Turn[0].ptStart.M = m_CurrPnt.BestCase.Direction;

			Segment.BestCase.Turn[0].ptEnd = Functions.LocalToPrj(PrevSegment.BestCase.Turn[1].ptCenter, startRad, PrevSegment.BestCase.Turn[1].Radius);
			Segment.BestCase.Turn[0].ptEnd.M = startRad + PrevSegment.BestCase.Turn[1].TurnDir * 90.0;

			Segment.BestCase.Turn[0].ptCenter = PrevSegment.BestCase.Turn[1].ptCenter;
			Segment.BestCase.Turn[0].TurnDir = -PrevSegment.BestCase.Turn[1].TurnDir;
			Segment.BestCase.Turn[0].Angle = NativeMethods.Modulus((m_CurrPnt.BestCase.Direction - Segment.BestCase.Turn[0].ptEnd.M) * Segment.BestCase.Turn[0].TurnDir);
			Segment.BestCase.Turn[0].Radius = PrevSegment.BestCase.Turn[1].Radius;
			Segment.BestCase.Turn[0].StartDist = 0.0;

			Segment.BestCase.Turn[1].ptStart = Segment.BestCase.Turn[0].ptEnd;
			Segment.BestCase.Turn[1].ptCenter = Functions.LocalToPrj(Segment.BestCase.Turn[1].ptStart, startRad, ExitSide * fTurnR);
			Segment.BestCase.Turn[1].ptEnd = Functions.LocalToPrj(Segment.BestCase.Turn[1].ptCenter, ExitDir + PrevSegment.BestCase.Turn[1].TurnDir * ExitSide * 90.0, fTurnR);
			Segment.BestCase.Turn[1].ptEnd.M = ExitDir;

			Segment.BestCase.Turn[1].TurnDir = -(int)Functions.SideDef(Segment.BestCase.Turn[1].ptStart, Segment.BestCase.Turn[1].ptStart.M, Segment.BestCase.Turn[1].ptEnd);
			Segment.BestCase.Turn[1].Angle = TurnAngle;	// NativeMethods.Modulus((Segment.Turn2.ptEnd.M - Segment.Turn2.ptStart.M) * Segment.Turn2.TurnDir);

			//---------------------------------

			IPointCollection MyPC = new ESRI.ArcGIS.Geometry.Multipoint();
			MyPC.AddPoint(m_CurrPnt.BestCase.pPoint);
			MyPC.AddPoint(Segment.BestCase.Turn[1].ptStart);
			MyPC.AddPoint(Segment.BestCase.Turn[1].ptEnd);

			IPointCollection pPC = new ESRI.ArcGIS.Geometry.Polyline();
			pPC.AddPoint(MyPC.Point[0]);
			pPC.AddPoint(MyPC.Point[1]);

			IPolyline pPoly = Functions.CalcTrajectoryFromMultiPoint(pPC);

			Segment.BestCase.Turn[0].Length = pPoly.Length;

			//Segment.Turn1.HeightAtEnd = m_NetHStart + pPoly.Length * maxNetGrad;
			//GrossHAtEnd1 = m_GrossHStart + pPoly.Length * ultramaxGrossGrad;

			//---------------------------------
			pPC = new ESRI.ArcGIS.Geometry.Polyline();
			pPC.AddPoint(MyPC.Point[1]);
			pPC.AddPoint(MyPC.Point[2]);

			pPoly = Functions.CalcTrajectoryFromMultiPoint(pPC);

			Segment.BestCase.Turn[1].StartDist = Segment.BestCase.Turn[0].Length;
			Segment.BestCase.Turn[1].Length = pPoly.Length;

			//Segment.Turn2.HeightAtEnd = Segment.Turn1.HeightAtEnd + pPoly.Length * maxNetGrad;
			//GrossHAtEnd2 =  GrossHAtEnd1 + pPoly.Length * ultramaxGrossGrad;

			//---------------------------------

			Segment.BestCase.pNominalPoly = Functions.CalcTrajectoryFromMultiPoint(MyPC);
			Segment.BestCase.Length = Segment.BestCase.pNominalPoly.Length;

			Segment.ptEnd.BestCase.Direction = ExitDir;
			Segment.BestCase.Turn[1].Radius = fTurnR;

			Segment.ptEnd.BestCase.pPoint = Segment.BestCase.Turn[1].ptEnd;
			//==================================================================================================

			DirToArcCenter = Functions.ReturnAngleInDegrees(Type7_CurNav.pPtPrj, PrevSegment.WorstCase.Turn[1].ptCenter);
			DistToArcCenter = Functions.ReturnDistanceInMeters(Type7_CurNav.pPtPrj, PrevSegment.WorstCase.Turn[1].ptCenter);
			betta = Functions.DegToRad(ExitDir - DirToArcCenter);

			if (Segment.ptStart.WorstCase.Width < 1852.0)
				Segment.ptStart.WorstCase.Width = 1852.0;
			if (Segment.ptEnd.WorstCase.Width < 1852.0)
				Segment.ptEnd.WorstCase.Width = 1852.0;

			TurnDist = 0;
			turnAlt = m_CurrPnt.WorstCase.GrossHeight + TurnDist * WorstMaxGrossGrad;

			fTAS = Functions.IAS2TAS(minAircraftSpeed2, turnAlt + m_ptDerPrj.Z, GlobalVars.m_CurrADHP.ISAtC);
			fTurnR = Functions.Bank2Radius(m_BankAngle, fTAS);

			fTmp = (ExitSide * fTurnR - PrevSegment.WorstCase.Turn[1].TurnDir * DistToArcCenter * System.Math.Sin(betta)) / (PrevSegment.WorstCase.Turn[1].Radius + ExitSide * fTurnR);

			if (Math.Abs(fTmp) - 1 > 0 && Math.Abs(fTmp) - 1 < 0.001)
				fTmp = Math.Sign(fTmp);

			TurnAngle = Functions.RadToDeg(Math.Acos(fTmp));
			startRad = ExitDir - ExitSide * PrevSegment.WorstCase.Turn[1].TurnDir * (90.0 - TurnAngle) + 90.0 * (1 - ExitSide);

			Segment.WorstCase.turns = 2;
			Segment.WorstCase.Turn[0].ptStart = m_CurrPnt.WorstCase.pPoint;
			Segment.WorstCase.Turn[0].ptStart.M = m_CurrPnt.WorstCase.Direction;

			Segment.WorstCase.Turn[0].ptEnd = Functions.LocalToPrj(PrevSegment.WorstCase.Turn[1].ptCenter, startRad, PrevSegment.WorstCase.Turn[1].Radius);
			Segment.WorstCase.Turn[0].ptEnd.M = startRad + PrevSegment.WorstCase.Turn[1].TurnDir * 90.0;

			Segment.WorstCase.Turn[0].ptCenter = PrevSegment.WorstCase.Turn[1].ptCenter;
			Segment.WorstCase.Turn[0].TurnDir = -PrevSegment.WorstCase.Turn[1].TurnDir;
			Segment.WorstCase.Turn[0].Angle = NativeMethods.Modulus((m_CurrPnt.WorstCase.Direction - Segment.WorstCase.Turn[0].ptEnd.M) * Segment.WorstCase.Turn[0].TurnDir);
			Segment.WorstCase.Turn[0].Radius = PrevSegment.WorstCase.Turn[1].Radius;
			Segment.WorstCase.Turn[0].StartDist = 0.0;

			Segment.WorstCase.Turn[1].ptStart = Segment.WorstCase.Turn[0].ptEnd;
			Segment.WorstCase.Turn[1].ptCenter = Functions.LocalToPrj(Segment.WorstCase.Turn[1].ptStart, startRad, ExitSide * fTurnR);
			Segment.WorstCase.Turn[1].ptEnd = Functions.LocalToPrj(Segment.WorstCase.Turn[1].ptCenter, ExitDir + PrevSegment.WorstCase.Turn[1].TurnDir * ExitSide * 90.0, fTurnR);
			Segment.WorstCase.Turn[1].ptEnd.M = ExitDir;

			Segment.WorstCase.Turn[1].TurnDir = -(int)Functions.SideDef(Segment.WorstCase.Turn[1].ptStart, Segment.WorstCase.Turn[1].ptStart.M, Segment.WorstCase.Turn[1].ptEnd);
			Segment.WorstCase.Turn[1].Angle = TurnAngle;	// NativeMethods.Modulus((Segment.Turn2.ptEnd.M - Segment.Turn2.ptStart.M) * Segment.Turn2.TurnDir);

			//---------------------------------

			MyPC = new ESRI.ArcGIS.Geometry.Multipoint();
			MyPC.AddPoint(m_CurrPnt.WorstCase.pPoint);
			MyPC.AddPoint(Segment.WorstCase.Turn[1].ptStart);
			MyPC.AddPoint(Segment.WorstCase.Turn[1].ptEnd);

			pPC = new ESRI.ArcGIS.Geometry.Polyline();
			pPC.AddPoint(MyPC.Point[0]);
			pPC.AddPoint(MyPC.Point[1]);

			pPoly = Functions.CalcTrajectoryFromMultiPoint(pPC);

			Segment.WorstCase.Turn[0].Length = pPoly.Length;

			//Segment.Turn1.HeightAtEnd = m_NetHStart + pPoly.Length * maxNetGrad;
			//GrossHAtEnd1 = m_GrossHStart + pPoly.Length * maxGrossGrad;

			//---------------------------------
			pPC = new ESRI.ArcGIS.Geometry.Polyline();
			pPC.AddPoint(MyPC.Point[1]);
			pPC.AddPoint(MyPC.Point[2]);

			pPoly = Functions.CalcTrajectoryFromMultiPoint(pPC);

			Segment.WorstCase.Turn[1].StartDist = Segment.WorstCase.Turn[0].Length;
			Segment.WorstCase.Turn[1].Length = pPoly.Length;

			//Segment.Turn2.HeightAtEnd = Segment.Turn1.HeightAtEnd + pPoly.Length * maxNetGrad;
			//GrossHAtEnd2 =  GrossHAtEnd1 + pPoly.Length * maxGrossGrad;

			//---------------------------------

			Segment.WorstCase.pNominalPoly = Functions.CalcTrajectoryFromMultiPoint(MyPC);
			Segment.WorstCase.Length = Segment.WorstCase.pNominalPoly.Length;

			Segment.ptEnd.WorstCase.Direction = ExitDir;
			Segment.WorstCase.Turn[1].Radius = fTurnR;
			Segment.ptEnd.WorstCase.pPoint = Segment.WorstCase.Turn[1].ptEnd;

			//==================================================================================================
			Segment.ptEnd.atHeight = false;

			CurrSegment.ptEnd.BestCase.NetHeight = Math.Min(m_CurrPnt.BestCase.NetHeight + Segment.BestCase.Length * BestMaxNetGrad2, cMaxH);
			CurrSegment.ptEnd.BestCase.GrossHeight = Math.Min(m_CurrPnt.BestCase.GrossHeight + Segment.BestCase.Length * BestMaxGrossGrad, cMaxH);

			CurrSegment.ptEnd.WorstCase.NetHeight = Math.Min(m_CurrPnt.WorstCase.NetHeight + Segment.WorstCase.Length * WorstMaxNetGrad2, cMaxH);
			CurrSegment.ptEnd.WorstCase.GrossHeight = Math.Min(m_CurrPnt.WorstCase.GrossHeight + Segment.WorstCase.Length * WorstMaxGrossGrad, cMaxH);

			string[] sTurnDir = { "вправо", "", "влево" };

			double outAzt = double.Parse(TextBox703.Text);
			string outStr = Math.Round(outAzt).ToString();

			while (outStr.Length < 3)
				outStr = "0" + outStr;
			outStr = outStr + "°";

			//double azt = NativeMethods.Modulus(m_CurrAzt - GlobalVars.m_CurrADHP.MagVar);
			//string aztStr = Math.Round(azt).ToString();
			//while (aztStr.Length < 3)
			//    aztStr = "0" + aztStr;
			//aztStr = aztStr + "°";

			Segment.Comment = "Перехват линии пути " + outStr + Type7_CurNav + " " + Navaids_DataBase.GetNavTypeName(Type7_CurNav.TypeCode);
			Label710.Text = Segment.Comment;
			//---------------------------------------------------------------------------
			Segment.bOnWPTFIX = false;
			Segment.SegmentCode = eLegType.arcPath;

			Segment.GuidanceNav = PrevSegment.InterceptionNav;
			Segment.InterceptionNav = pNav;

			Segment.PathCode = SegmentPathType.AF;

			return true;
		}

		#region arcPath events

		private void TextBox703_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;

			if (e.KeyChar == 13)
				TextBox703_Validating(TextBox703, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, TextBox703.Text);

			e.KeyChar = eventChar;
			if (e.KeyChar == 0)
				e.Handled = true;
		}

		private void TextBox703_Validating(object sender, CancelEventArgs e)
		{
			double magNewVal;

			if (!double.TryParse(TextBox703.Text, out magNewVal))
				return;

			int k = ComboBox701.SelectedIndex;
			if (k < 0)
				return;

			//Type7_CurNav = (NavaidType)ComboBox701.SelectedItem;//ComboBox701List[k];

			double dirNewVal = Functions.Azt2Dir(Type7_CurNav.pPtGeo, magNewVal + m_MagVar);

			if ((Type7_Intervals[k].Left != 0 || Type7_Intervals[k].Right != 360.0) && (!Functions.AngleInSector(dirNewVal, Type7_Intervals[k].Left, Type7_Intervals[k].Right)))
			{
				if (Functions.AnglesSideDef(dirNewVal, Type7_Intervals[k].Left) < 0)
					dirNewVal = Type7_Intervals[k].Left;
				else
					dirNewVal = Type7_Intervals[k].Right;

				TextBox703.Text = Math.Round(Functions.Dir2Azt(Type7_CurNav.pPtPrj, dirNewVal - m_MagVar), 2).ToString();
			}

			ConstructNextSegment();
		}

		private void OptionButton701_CheckedChanged(object sender, EventArgs e)
		{
			if (!OptionButton701.Checked)
				return;

			//TextBox703.ReadOnly = false;
			//TextBox703.BackColor = System.Drawing.SystemColors.Window;

			ComboBox703.Enabled = false;

			TextBox703.Visible = true;
			ComboBox704.Visible = false;

			TextBox703_Validating(TextBox703, new System.ComponentModel.CancelEventArgs());
		}

		private void OptionButton702_CheckedChanged(object sender, EventArgs e)
		{
			if (!OptionButton702.Checked)
				return;

			ComboBox703.Enabled = true;
			ComboBox703_SelectedIndexChanged(ComboBox703, new System.EventArgs());
		}

		private void ComboBox701_SelectedIndexChanged(object sender, EventArgs e)
		{
			int k = ComboBox701.SelectedIndex;
			if (k < 0)
				return;

			Type7_CurNav = (NavaidData)ComboBox701.SelectedItem;		//ComboBox701List[k];

			if (Type7_Intervals[k].Left != 0 || Type7_Intervals[k].Right != 360.0)
			{
				//ToolTip1.SetToolTip(TextBox703, "Min: " + Math.Round(Functions.Dir2Azt(Type7_CurNav.pPtPrj, Type7_Intervals[k].Right_Renamed), 2).ToString() +
				//								" Max: " + Math.Round(Functions.Dir2Azt(Type7_CurNav.pPtPrj, Type7_Intervals[k].Left_Renamed), 2).ToString());
			}
			else
			{
				//ToolTip1.SetToolTip(TextBox703, "Min: 0 Max: 360");
			}

			FillComboBox703List();
			if (OptionButton701.Checked)
			{
				TextBox703.Text = Math.Round(NativeMethods.Modulus(Functions.Dir2Azt(Type7_CurNav.pPtPrj, Type7_Intervals[k].Right) - m_MagVar), 2).ToString();
				TextBox703_Validating(TextBox703, new System.ComponentModel.CancelEventArgs());
			}
		}

		private void ComboBox702_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!m_bFormInitialised)
				return;
			//							PrevSegment
			FillComboBox701Stations(LegList[LegCount - 1]);
		}

		private void ComboBox703_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!OptionButton702.Checked)
				return;

			Type7_CurFix = (NavaidData)ComboBox703.SelectedItem;

			int k = ComboBox701.SelectedIndex;
			Label702.Text = Navaids_DataBase.GetNavTypeName(Type7_CurFix.TypeCode);

			double fTmp = Functions.ReturnAngleInDegrees(Type7_CurNav.pPtPrj, Type7_CurFix.pPtPrj);

			if (Type7_Intervals[k].Left == 0 && Type7_Intervals[k].Right == 360.0)
			{
				ComboBox704.Items.Clear();
				ComboBox704.Items.Add(Math.Round(NativeMethods.Modulus(Functions.Dir2Azt(Type7_CurNav.pPtPrj, fTmp) - m_MagVar), 2)).ToString();
				ComboBox704.Items.Add(Math.Round(NativeMethods.Modulus(Functions.Dir2Azt(Type7_CurNav.pPtPrj, fTmp + 180.0) - m_MagVar), 2)).ToString();

				//TextBox703.ReadOnly = true;
				//TextBox703.BackColor = System.Drawing.SystemColors.ButtonFace;
				//ComboBox704.Enabled = True

				TextBox703.Visible = false;
				ComboBox704.Visible = true;

				ComboBox704.SelectedIndex = 0;
			}
			else
			{
				//TextBox703.ReadOnly = false;
				//TextBox703.BackColor = System.Drawing.SystemColors.Window;
				//ComboBox704.Enabled = False

				TextBox703.Visible = true;
				ComboBox704.Visible = false;

				if (!Functions.AngleInSector(fTmp, Type7_Intervals[k].Left, Type7_Intervals[k].Right))
					fTmp = fTmp + 180.0;

				TextBox703.Text = Math.Round(NativeMethods.Modulus(Functions.Dir2Azt(Type7_CurNav.pPtPrj, fTmp) - m_MagVar), 2).ToString();
				TextBox703_Validating(TextBox703, new System.ComponentModel.CancelEventArgs());
			}
		}

		private void ComboBox704_SelectedIndexChanged(object sender, EventArgs e)
		{
			TextBox703.Text = ComboBox704.Text;
			TextBox703_Validating(TextBox703, new System.ComponentModel.CancelEventArgs());
		}
		#endregion

		#endregion

		#region Page VI

		private int Ret4Leg;
		private int Ret4Obst;
		private ObstacleData det;

		private double _maxAccelerationHeight;
		private double _minAccelerationHeight;
		private double totalPathLeng;

		private double _transitionH;		//????????

		private double _phase3MinTime;
		private double _phase3MaxTime;
		private double _phase3MaxLength;

		private double _Phase3ActualTime;
		private double _Phase3ActualLengt;
		private double _TerminationHeight;

		void AddvanceToPageVI()
		{
			RemoveSegmentBtn.Enabled = false;
			CreateBtn.Enabled = false;

			_minAccelerationHeight = GlobalVars.minAccelerationHeight;
			_maxAccelerationHeight = GlobalVars.minAccelerationHeight;
			_transitionH = _minAccelerationHeight;

			for (int j = 0; j < LegCount; j++)
			{
				for (int i = 0; i < LegList[j].ObstacleList.Length; i++)
				{
					double appl2 = LegList[j].ObstacleList[i].ApplNetGradient2;
					double k = (appl2 - WorstMaxNetGrad2) / (BestMaxNetGrad2 - WorstMaxNetGrad2);
					double appl4 = WorstMaxNetGrad4 + k * (BestMaxNetGrad4 - WorstMaxNetGrad4);
					LegList[j].ObstacleList[i].ApplNetGradient4 = appl4;


					double x1 = (LegList[j].ObstacleList[i].MOCH - _transitionH) / appl4;
					if (x1 > 0.0)
					{
						double x = LegList[j].ObstacleList[i].TotalDist - x1;

						double trH = (LegList[j].ObstacleList[i].MOCH * appl2 - GlobalVars.heightAboveDER * appl4 - LegList[j].ObstacleList[i].TotalDist * appl2 * appl4) / (appl2 - appl4);
						if (trH > _minAccelerationHeight)
						{
							_minAccelerationHeight = trH;
							_transitionH = trH;
						}

						double trMaxh = LegList[j].ObstacleList[i].AcceleStartDist * appl2 + GlobalVars.heightAboveDERMax;
						if (trMaxh > _maxAccelerationHeight)
							_maxAccelerationHeight = trMaxh;
					}
				}
			}

			_transitionH = _minAccelerationHeight;

			if (comboBox0402.SelectedIndex == 0)
				comboBox0402_SelectedIndexChanged(comboBox0402, new EventArgs());
			else
				comboBox0402.SelectedIndex = 0;

			//_maxAccelerationHeight = m_AcceleDist * WorstMaxNetGrad2 + GlobalVars.heightAboveDERMax;	//ultraMaxNetGrad2
			//								PrevSegment							PrevSegment
			totalPathLeng = LegList[LegCount - 1].WorstCase.PrevTotalLength + LegList[LegCount - 1].WorstCase.Length;
			_phase3MinTime = 1.5;


			textBox0401.Tag = "";
			textBox0401_Validating(textBox0401, new CancelEventArgs());

			//_TerminationHeight = m_CurrPnt.WorstCase.NetHeight;
		}

		private void CalculateActualTime()
		{
			for (int j = 0; j < LegCount; j++)
			{
				for (int i = 0; i < LegList[j].ObstacleList.Length; i++)
				{
					double xPhase2End = (_transitionH - GlobalVars.heightAboveDER) / LegList[j].ObstacleList[i].ApplNetGradient2;
					double HActual = 0.0;
					HActual = GlobalVars.heightAboveDER + xPhase2End * LegList[j].ObstacleList[i].ApplNetGradient2;
					LegList[j].ObstacleList[i].Phase = 1;

					if (xPhase2End > LegList[j].ObstacleList[i].TotalDist)
					{
						HActual = GlobalVars.heightAboveDER + LegList[j].ObstacleList[i].TotalDist * LegList[j].ObstacleList[i].ApplNetGradient2;
						LegList[j].ObstacleList[i].Phase = 0;
					}
					else if (xPhase2End + _Phase3ActualLengt < LegList[j].ObstacleList[i].TotalDist)
					{
						double fDist = LegList[j].ObstacleList[i].TotalDist - (xPhase2End + _Phase3ActualLengt);
						double dH = fDist * LegList[j].ObstacleList[i].ApplNetGradient4;
						HActual += dH;
						LegList[j].ObstacleList[i].Phase = 2;
					}
					LegList[j].ObstacleList[i].ActualHeight = HActual;
				}
			}

			TrackLeg[] legs = new TrackLeg[LegCount];
			Array.Copy(LegList, legs, LegCount);
			ReportsFrm.FillPage4Legs(legs);

			_TerminationHeight = _transitionH + (totalPathLeng - (_transitionH - GlobalVars.heightAboveDER) / WorstMaxNetGrad2 - _Phase3ActualLengt) * WorstMaxNetGrad4;

			if (comboBox0401.SelectedIndex == 0)
				comboBox0401_SelectedIndexChanged(comboBox0401, new EventArgs());
			else
				comboBox0401.SelectedIndex = 0;
		}

		private void CalculateAvailableTime()
		{
			ObstacleData[] sel = new ObstacleData[AllObsstacles.Length];

			int j1 = -1, i1 = -1, k = -1, k1 = -1;
			double fAccelLenght = totalPathLeng;					// double.MaxValue;	//xMin;

			for (int j = 0; j < LegCount; j++)
			{
				for (int i = 0; i < LegList[j].ObstacleList.Length; i++)
				{
					double x1 = (LegList[j].ObstacleList[i].MOCH - _transitionH) / LegList[j].ObstacleList[i].ApplNetGradient4;
					if (x1 > 0.0)
					{
						LegList[j].ObstacleList[i].AcceleEndDist = LegList[j].ObstacleList[i].TotalDist - x1;

						if (LegList[j].ObstacleList[i].AcceleEndDist <= totalPathLeng)
						{
							LegList[j].ObstacleList[i].AcceleStartDist = (_transitionH - GlobalVars.heightAboveDER) / LegList[j].ObstacleList[i].ApplNetGradient2;

							++k;
							sel[k] = LegList[j].ObstacleList[i];

							double fHoris = Math.Max(LegList[j].ObstacleList[i].AcceleEndDist, LegList[j].ObstacleList[i].AcceleStartDist) - LegList[j].ObstacleList[i].AcceleStartDist;

							if (fHoris < fAccelLenght)
							{
								j1 = j;
								i1 = i;
								k1 = k;
								fAccelLenght = fHoris;
							}
						}

						if (LegList[j].ObstacleList[i].AcceleEndDist > totalPathLeng)
						{
							MessageBox.Show("Unsufficient total path lenght !");
						}
					}
				}
			}

			Ret4Leg = j1;
			Ret4Obst = i1;

			if (k1 >= 0)
			{
				_phase3MaxLength = fAccelLenght;		// totalPathLeng - xAcceleStartDist;
				det = sel[k1];
			}
			else
			{
				_phase3MaxLength =  totalPathLeng - (_transitionH - GlobalVars.heightAboveDER) / WorstMaxNetGrad2;
				det = new ObstacleData();
			}

			textBox0404.Text = det.ID;

			textBox0402.Text = UnitConverter.DistanceToDisplayUnits(_phase3MaxLength, eRoundMode.NERAEST).ToString();

			double fTAS = Functions.IAS2TAS(0.5 * (minAircraftSpeed2 + minAircraftSpeed4), _transitionH + m_ptDerPrj.Z, GlobalVars.m_CurrADHP.ISAtC);

			_phase3MaxTime = 0.06 * _phase3MaxLength / fTAS;
			textBox0403.Text = Math.Round(_phase3MaxTime, 2).ToString();

			fTAS = Functions.IAS2TAS( minAircraftSpeed2 , _transitionH + m_ptDerPrj.Z, GlobalVars.m_CurrADHP.ISAtC);

			double fStartDistTime = 0.06 * (_transitionH - GlobalVars.heightAboveDER)/(WorstMaxNetGrad2 * fTAS );
			textBox0407.Text = Math.Round(fStartDistTime, 2).ToString();

			textBox0405.Text = Math.Round( Math.Min(_phase3MinTime, _phase3MaxTime),2).ToString();
			textBox0405.Tag = "";
			textBox0405_Validating(textBox0405, new CancelEventArgs());
			Array.Resize<ObstacleData>(ref sel, k + 1);
			ReportsFrm.FillPage5(sel, det, _transitionH);
		}

		private void comboBox0401_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox0401.SelectedIndex == 0)
				textBox0408.Text = UnitConverter.HeightToDisplayUnits(_TerminationHeight + m_ptDerPrj.Z, eRoundMode.NERAEST).ToString();
			else
				textBox0408.Text = UnitConverter.HeightToDisplayUnits(_TerminationHeight, eRoundMode.NERAEST).ToString();
		}

		private void comboBox0402_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox0402.SelectedIndex == 0)
				textBox0401.Text = UnitConverter.HeightToDisplayUnits(_transitionH + m_ptDerPrj.Z, eRoundMode.CEIL).ToString();
			else
				textBox0401.Text = UnitConverter.HeightToDisplayUnits(_transitionH, eRoundMode.CEIL).ToString();
		}

		private void textBox0401_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox0401_Validating(textBox0401, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox0401.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox0401_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox0401.Text, out fTmp))
			{
				if (double.TryParse((string)textBox0401.Tag, out fTmp))
					textBox0401.Text = (string)textBox0401.Tag;
				else
				{
					if (comboBox0402.SelectedIndex == 0)
						textBox0401.Text = UnitConverter.HeightToDisplayUnits(GlobalVars.heightAboveDER + m_ptDerPrj.Z, eRoundMode.CEIL).ToString();
					else
						textBox0401.Text = UnitConverter.HeightToDisplayUnits(GlobalVars.heightAboveDER , eRoundMode.CEIL).ToString();
				}
				return;
			}

			if (textBox0401.Tag != null && textBox0401.Tag.ToString() == textBox0401.Text)
				return;

			double NewHeight = UnitConverter.HeightToInternalUnits(fTmp);
			if (comboBox0402.SelectedIndex != 1)
				NewHeight -= m_ptDerPrj.Z;

			if (NewHeight < _minAccelerationHeight)
				NewHeight = _minAccelerationHeight;
			if (NewHeight > m_CurrPnt.WorstCase.NetHeight)
				NewHeight = m_CurrPnt.WorstCase.NetHeight;

			_transitionH = NewHeight;

			if (comboBox0402.SelectedIndex == 0)
				textBox0401.Text = UnitConverter.HeightToDisplayUnits(_transitionH + m_ptDerPrj.Z, eRoundMode.CEIL).ToString();
			else
				textBox0401.Text = UnitConverter.HeightToDisplayUnits(_transitionH , eRoundMode.CEIL).ToString();

			textBox0401.Tag = textBox0401.Text;

			CalculateAvailableTime();
		}

		private void textBox0405_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox0405_Validating(textBox0405, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox0405.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox0405_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox0405.Text, out fTmp))
			{
				if (double.TryParse((string)textBox0405.Tag, out fTmp))
					textBox0405.Text = (string)textBox0405.Tag;
				else
					textBox0405.Text = Math.Round( _phase3MinTime, 2).ToString();

				return;
			}

			if (textBox0405.Tag != null && textBox0405.Tag.ToString() == textBox0405.Text)
				return;

			double NewTime = fTmp;

			if (NewTime < _phase3MinTime)
				NewTime = _phase3MinTime;

			if (NewTime > _phase3MaxTime)
				NewTime = _phase3MaxTime;

			if (NewTime != fTmp)
			{
				textBox0405.Text = Math.Round(NewTime, 2).ToString();
				textBox0405.Tag = textBox0405.Text;
			}

			_Phase3ActualTime = NewTime;
			double fTAS = Functions.IAS2TAS(0.5 * (minAircraftSpeed2 + minAircraftSpeed4), _transitionH + m_ptDerPrj.Z, GlobalVars.m_CurrADHP.ISAtC);
			_Phase3ActualLengt = _Phase3ActualTime * fTAS * 16.66666666666666666666666;

			textBox0406.Text = UnitConverter.DistanceToDisplayUnits(_Phase3ActualLengt, eRoundMode.CEIL).ToString();

			CalculateActualTime();
		}

		#endregion

		private void SaveProtocol(string RepFileName, string RepFileTitle, ReportHeader pReport)
		{
			/*
			CReportFile RoutsProtRep = new CReportFile();

			RoutsProtRep.DerPtPrj = GlobalVars.m_SelectedRWY.pPtPrj[eRWY.PtEnd];
			RoutsProtRep.ThrPtPrj = GlobalVars.m_SelectedRWY.pPtPrj[eRWY.PtEnd];

			RoutsProtRep.OpenFile(RepFileName + "_Protocol", Resources.str701); // "???????? ?????????"

			RoutsProtRep.WriteString(Resources.str15271 + " - " + Resources.str701); // " - ???????? ?????????"
			RoutsProtRep.WriteString("");
			RoutsProtRep.WriteString(RepFileTitle);

			RoutsProtRep.WriteHeader(pReport);
			RoutsProtRep.WriteString("");
			RoutsProtRep.WriteString("");

			RoutsProtRep.lListView = ReportsFrm.ListView1;
			RoutsProtRep.WriteTab(ReportsFrm.GetTabPageText(0));

			RoutsProtRep.lListView = ReportsFrm.ListView4;
			RoutsProtRep.WriteTab(ReportsFrm.GetTabPageText(3));

			RoutsProtRep.lListView = ReportsFrm.ListView5;
			RoutsProtRep.WriteTab(ReportsFrm.GetTabPageText(4));

			RoutsProtRep.CloseFile();
			RoutsProtRep = null;
			*/
		}

		private void SaveLog(string RepFileName, string RepFileTitle, ReportHeader pReport)
		{
			/*			CReportFile RoutsLogRep = new CReportFile();
						RoutsLogRep.DerPtPrj = ((ESRI.ArcGIS.Geometry.IPoint)(GlobalVars.m_SelectedRWY.pPtPrj[eRWY.PtEnd]));
						RoutsLogRep.ThrPtPrj = ((ESRI.ArcGIS.Geometry.IPoint)(GlobalVars.m_SelectedRWY.pPtPrj[eRWY.PtEnd]));

						RoutsLogRep.OpenFile(RepFileName + "_Log", Resources.str520);

						RoutsLogRep.WriteString(Resources.str15271 + " - " + Resources.str520);
						RoutsLogRep.WriteString("");
						RoutsLogRep.WriteString(RepFileTitle);

						RoutsLogRep.WriteHeader(pReport);


						RoutsLogRep.WriteString("");
						RoutsLogRep.WriteString("");

						RoutsLogRep.WriteString("[ " + MultiPage1.TabPages[0].Text + " ]");
						RoutsLogRep.WriteString("");

						RoutsLogRep.WriteParam(Label001.Text, ComboBox001.Text, "");
						RoutsLogRep.WriteParam(Label002.Text, TextBox001.Text, "");
						RoutsLogRep.WriteParam(Label006.Text, TextBox004.Text, "");
						RoutsLogRep.WriteParam(Label003.Text, TextBox002.Text, Label011.Text);
						RoutsLogRep.WriteString("");

						RoutsLogRep.WriteParam(Label004.Text, TextBox003.Text, Label009.Text);
						if (Option4.Checked)
							RoutsLogRep.WriteParam(Frame001.Text, Option4.Text, "");
						else
							RoutsLogRep.WriteParam(Frame001.Text, Option3.Text, "");

						RoutsLogRep.WriteString("");

						RoutsLogRep.WriteString(Frame002.Text);
						RoutsLogRep.WriteParam(Label007.Text, TextBox005.Text, "");
						RoutsLogRep.WriteParam(Label008.Text, TextBox006.Text, Label012.Text);
						RoutsLogRep.WriteParam(Label013.Text, TextBox007.Text, Label010.Text);
						RoutsLogRep.WriteString("");
						RoutsLogRep.WriteString("");

						RoutsLogRep.WriteString("[ " + MultiPage1.TabPages[1].Text + " ]");
						RoutsLogRep.WriteString("");

						RoutsLogRep.WriteParam(Resources.str40024, SpinButton1.Value.ToString(), "? " + ComboBox101.Text);
						RoutsLogRep.WriteParam(Label102.Text, TextBox102.Text, "%");
						RoutsLogRep.WriteParam(Label103.Text, TextBox103.Text, "%");
						RoutsLogRep.WriteParam(Label104.Text, TextBox104.Text, Label108.Text);
						RoutsLogRep.WriteString("");
						RoutsLogRep.WriteString("");


						RoutsLogRep.WriteString("[ " + MultiPage1.TabPages[2].Text + " ]");
						RoutsLogRep.WriteString("");

						RoutsLogRep.WriteParam(Label201.Text, TextBox201.Text, "°");
						RoutsLogRep.WriteParam(Label202.Text, TextBox202.Text, Label205.Text);
						RoutsLogRep.WriteParam(Label203.Text, TextBox203.Text, "%");
						RoutsLogRep.WriteParam(Label207.Text, TextBox204.Text, Label212.Text);
						RoutsLogRep.WriteParam(Label208.Text, TextBox205.Text, "");
						RoutsLogRep.WriteParam(Label209.Text, TextBox206.Text, "");
						RoutsLogRep.WriteParam(_Label200_2.Text, ComboBox201.Text, "");

						RoutsLogRep.WriteString("");
						RoutsLogRep.WriteString("");

						if (CurrPage > 2)
						{
							RoutsLogRep.WriteString("[ " + MultiPage1.TabPages[3].Text + " ]");
							RoutsLogRep.WriteString("");

							RoutsLogRep.WriteParam(Resources.str902, Label309.Text, Label304.Text);
							RoutsLogRep.WriteParam(Resources.str903, Label310.Text, Label304.Text);
							RoutsLogRep.WriteParam(Label302.Text, TextBox301.Text, Label304.Text);
							RoutsLogRep.WriteParam(Label303.Text, TextBox302.Text, "°");
							RoutsLogRep.WriteParam(Label311.Text, ComboBox302.Text, "");
							RoutsLogRep.WriteString("");

							if (OptionButton301.Checked)
								RoutsLogRep.WriteParam(Frame301.Text, OptionButton301.Text, "");
							else
								RoutsLogRep.WriteParam(Frame301.Text, OptionButton302.Text, "");

							if (CheckBox301.Checked)
								RoutsLogRep.WriteParam(CheckBox301.Text, Resources.str39015, "");
							else
								RoutsLogRep.WriteParam(CheckBox301.Text, Resources.str39014, "");

							RoutsLogRep.WriteString("");
							RoutsLogRep.WriteString("");

							RoutsLogRep.WriteString("[ " + MultiPage1.TabPages[4].Text + " ]");
							RoutsLogRep.WriteString("");

							RoutsLogRep.WriteParam((Label404.Text), (TextBox403.Text), "°");
							RoutsLogRep.WriteString("");

							RoutsLogRep.WriteString(Label401.Text);
							RoutsLogRep.WriteParam(Label402.Text, TextBox401.Text, Label413.Text);
							RoutsLogRep.WriteParam(Label403.Text, TextBox402.Text, Label414.Text);
							RoutsLogRep.WriteString("");

							RoutsLogRep.WriteString(Label407.Text);
							RoutsLogRep.WriteParam(Label408.Text, SpinButton2.Value.ToString(), "? " + ComboBox401.Text);
							RoutsLogRep.WriteParam(Label409.Text, TextBox405.Text, "%");
							RoutsLogRep.WriteString("");
							RoutsLogRep.WriteString("");

							RoutsLogRep.WriteString("[ " + MultiPage1.TabPages[5].Text + " ]");
							RoutsLogRep.WriteString("");

							RoutsLogRep.WriteString((Frame501.Text));
							if (ComboBox501.Visible)
							{
								RoutsLogRep.WriteParam(Label501.Text, ComboBox501.Text + " (" + Label503.Text + ")", "");
								RoutsLogRep.WriteParam(Label502.Text, TextBox501.Text, Label526.Text);

								if (TurnInterDat[ComboBox501.SelectedIndex].TypeCode == eNavaidType.CodeDME)
								{
									if (Option501.Checked)
										RoutsLogRep.WriteParam(Resources.str521, Option501.Text, "");
									else
										RoutsLogRep.WriteParam(Resources.str521, Option502.Text, "");
								}
							}
							RoutsLogRep.WriteString("");

							RoutsLogRep.WriteParam((Label525.Text), (TextBox503.Text), (Label527.Text));
							RoutsLogRep.WriteParam((Label504.Text), (TextBox502.Text), (Label528.Text));
							RoutsLogRep.WriteParam((Label506.Text), (Text501.Text), (Label507.Text));
							RoutsLogRep.WriteString("");

							RoutsLogRep.WriteParam(Label509.Text, Text502.Text, Label529.Text);
							RoutsLogRep.WriteParam(Label511.Text, Text503.Text, Label530.Text);
							RoutsLogRep.WriteParam(Label513.Text, Text504.Text, Label531.Text);
							RoutsLogRep.WriteParam(Label515.Text, Text505.Text, Label532.Text);
							RoutsLogRep.WriteParam(Label517.Text, Text506.Text, Label533.Text);
							RoutsLogRep.WriteParam(Label519.Text, Text507.Text, Label534.Text);
							if (Text508.Visible)
								RoutsLogRep.WriteParam(Label521.Text, Text508.Text, Label535.Text);

							if (Text509.Visible)
								RoutsLogRep.WriteParam(Label523.Text, Text509.Text, Label536.Text);

							RoutsLogRep.WriteString("");
							RoutsLogRep.WriteString("");

							RoutsLogRep.WriteString("[ " + MultiPage1.TabPages[6].Text + " ]");
							RoutsLogRep.WriteString("");

							if (OptionButton601.Checked)
							{
								RoutsLogRep.WriteString(OptionButton601.Text);
								RoutsLogRep.WriteParam(Label605.Text, TextBox602.Text, "");
								RoutsLogRep.WriteString("");
							}

							if (OptionButton602.Checked)
							{
								RoutsLogRep.WriteString(OptionButton602.Text);
								RoutsLogRep.WriteParam(Label601.Text, ComboBox601.Text + " (" + Label602.Text + ")", "");
								RoutsLogRep.WriteParam(Label605.Text, TextBox602.Text, "");
								RoutsLogRep.WriteString("");

								RoutsLogRep.WriteParam(Label47.Text, TextBox601.Text, Label48.Text);
								RoutsLogRep.WriteParam(Label607.Text, TextBox603.Text, "°");
								RoutsLogRep.WriteParam(Label603.Text, Label604.Text, "");
								RoutsLogRep.WriteString("");
							}

							if (OptionButton603.Checked)
							{
								RoutsLogRep.WriteString(OptionButton603.Text);
								RoutsLogRep.WriteParam(Label601.Text, ComboBox601.Text + " (" + Label602.Text + ")", "");
								RoutsLogRep.WriteParam(Label605.Text, TextBox602.Text, "");
								RoutsLogRep.WriteString("");
							}

							RoutsLogRep.WriteString("");

							if (OptionButton602.Checked)
							{
								RoutsLogRep.WriteString("[ " + MultiPage1.TabPages[8].Text + " ]");
								RoutsLogRep.WriteString("");

								RoutsLogRep.WriteString(Frame801.Text);
								if (CheckBox801.Checked)
									RoutsLogRep.WriteParam(CheckBox801.Text, Resources.str39015, "");
								else
									RoutsLogRep.WriteParam(CheckBox801.Text, Resources.str39014, "");

								if (CheckBox802.Checked)
									RoutsLogRep.WriteParam(CheckBox802.Text, Resources.str39015, "");
								else
									RoutsLogRep.WriteParam(CheckBox802.Text, Resources.str39014, "");

								if (CheckBox803.Checked)
									RoutsLogRep.WriteParam(CheckBox803.Text, Resources.str39015, "");
								else
									RoutsLogRep.WriteParam(CheckBox803.Text, Resources.str39014, "");

								RoutsLogRep.WriteString("");

								RoutsLogRep.WriteString(Frame802.Text);
								if (CheckBox804.Checked)
									RoutsLogRep.WriteParam(CheckBox804.Text, Resources.str39015, "");
								else
									RoutsLogRep.WriteParam(CheckBox804.Text, Resources.str39014, "");

								if (CheckBox805.Checked)
									RoutsLogRep.WriteParam(CheckBox805.Text, Resources.str39015, "");
								else
									RoutsLogRep.WriteParam(CheckBox805.Text, Resources.str39014, "");

								if (CheckBox806.Checked)
									RoutsLogRep.WriteParam(CheckBox806.Text, Resources.str39015, "");
								else
									RoutsLogRep.WriteParam(CheckBox806.Text, Resources.str39014, "");

								RoutsLogRep.WriteString("");
								RoutsLogRep.WriteString("");
							}

							RoutsLogRep.WriteString("[ " + MultiPage1.TabPages[9].Text + " ]");
							RoutsLogRep.WriteString("");

							RoutsLogRep.WriteString(Resources.str39001);
							RoutsLogRep.WriteParam(Label901.Text, TextBox901.Text, "%");
							RoutsLogRep.WriteParam(Label902.Text, TextBox902.Text, "%");
							RoutsLogRep.WriteParam(Label907.Text, TextBox904.Text, UnitConverter.HeightUnit);
							RoutsLogRep.WriteString("");

							RoutsLogRep.WriteString(Resources.str39002);
							RoutsLogRep.WriteParam(OptionButton901.Text, TextBox911.Text, "%");
							RoutsLogRep.WriteParam(OptionButton902.Text, TextBox912.Text, "%");
							RoutsLogRep.WriteString("");

							RoutsLogRep.WriteString("");
							RoutsLogRep.WriteString("");
						}

						RoutsLogRep.CloseFile();
						RoutsLogRep = null;
						*/
		}

		IDepartureLeg CreateDepartureLeg(int NO_SEQ, ref TerminalSegmentPoint pTerminalSegmentPoint)
		{
			UomDistance[] uomDistHorzTab = new UomDistance[] { UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI };
			UomDistance[] uomDistVerHorzTab = new UomDistance[] { UomDistance.M, UomDistance.FT, UomDistance.KM, UomDistance.MI, UomDistance.NM };
			UomDistanceVertical[] uomDistVerTab = new UomDistanceVertical[] { UomDistanceVertical.M, UomDistanceVertical.FT, UomDistanceVertical.OTHER, UomDistanceVertical.OTHER, UomDistanceVertical.OTHER };
			UomSpeed[] uomSpeedTab = new UomSpeed[] { UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN, UomSpeed.FT_SEC };

			UomDistance mHUomDistance = uomDistHorzTab[GlobalVars.DistanceUnitIndex];
			UomDistance mVUomDistance = uomDistVerHorzTab[GlobalVars.HeightUnitIndex];
			UomDistanceVertical mVUomDistanceV = uomDistVerTab[GlobalVars.HeightUnitIndex];
			UomSpeed mUomSpeed = uomSpeedTab[GlobalVars.SpeedUnitIndex];

			TrackLeg CurrLeg = LegList[NO_SEQ - 1];

			IDepartureLeg result = new DepartureLeg();
			ISegmentLeg pSegmentLeg = result.AsISegmentLeg;

			pSegmentLeg.AltitudeInterpretation = AltitudeUseType.ABOVE_LOWER;
			pSegmentLeg.UpperLimitReference = VerticalReferenceType.MSL;
			pSegmentLeg.LowerLimitReference = VerticalReferenceType.MSL;
			pSegmentLeg.TurnDirection = DirectionTurnType.EITHER;
			pSegmentLeg.CourseType = CourseType.TRUE_TRACK;

			if (CurrLeg.SegmentCode == eLegType.arcPath)
				pSegmentLeg.LegPath = TrajectoryType.ARC;
			else
				pSegmentLeg.LegPath = TrajectoryType.STRAIGHT;

			//    pSegmentLeg.AltitudeOverrideATC =
			//    pSegmentLeg.AltitudeOverrideReference =
			//    pSegmentLeg.Duration = '???????????????????????????????? pLegs(I).valDur
			//    pSegmentLeg.Note
			//    pSegmentLeg.ReqNavPerformance
			//    pSegmentLeg.SpeedInterpretation =
			//    pSegmentLeg.ReqNavPerformance
			//    pSegmentLeg.CourseDirection =
			//    pSegmentLeg.ProcedureTransition

			// ======================================================================
			IInt32 pInt32 = new Int32Class();
			pInt32.Value = NO_SEQ;
			pSegmentLeg.SeqNumberARINC = pInt32;

			pSegmentLeg.LegTypeARINC = CurrLeg.PathCode;
			if (CurrLeg.ptEnd.atHeight)
				pSegmentLeg.EndConditionDesignator = SegmentTerminationType.ALTITUDE;

			IBoolean pBoolean = new BooleanClass();
			pBoolean.Value = false;
			pSegmentLeg.ProcedureTurnRequired = pBoolean;

			double fCourseDir;
			IDouble pDouble = new DoubleClass();
			if (radioButton0401.Checked)
			{
				CurrLeg.TraceCase = CurrLeg.WorstCase;
				CurrLeg.ptStart.TraceCase = CurrLeg.ptStart.WorstCase;
				CurrLeg.ptEnd.TraceCase = CurrLeg.ptEnd.WorstCase;
			}
			else
			{
				CurrLeg.TraceCase = CurrLeg.BestCase;
				CurrLeg.ptStart.TraceCase = CurrLeg.ptStart.BestCase;
				CurrLeg.ptEnd.TraceCase = CurrLeg.ptEnd.BestCase;
			}

			switch (CurrLeg.SegmentCode)
			{
				case eLegType.courseIntercept:				// "На заданную WPT"
				case eLegType.arcIntercept:
					fCourseDir = CurrLeg.ptStart.TraceCase.Direction;
					pDouble.Value = Functions.Dir2Azt(CurrLeg.ptStart.TraceCase.pPoint, fCourseDir);
					break;
				default:
					fCourseDir = CurrLeg.ptEnd.TraceCase.Direction;
					pDouble.Value = Functions.Dir2Azt(CurrLeg.ptEnd.TraceCase.pPoint, fCourseDir);
					break;
			}
			pSegmentLeg.Course = pDouble;

			//=======================================================================================
			pDouble = new DoubleClass();
			pDouble.Value = CurrLeg.BankAngle;
			pSegmentLeg.BankAngle = pDouble;
			//=======================================================================================

			IDistanceVertical pDistanceVertical = new DistanceVertical();
			pDistanceVertical.Uom = mVUomDistanceV;
			pDistanceVertical.Value = UnitConverter.HeightToDisplayUnits(CurrLeg.ptEnd.TraceCase.NetHeight + m_ptDerPrj.Z, eRoundMode.NERAEST).ToString();
			pSegmentLeg.LowerLimitAltitude = pDistanceVertical;

			//=======================================================================================
			//    pDistance = new Distance();
			//    pDistance.Uom = mVUomDistance;
			//    pDistance.Value = UnitConverter.HeightToDisplayUnits( LegList[i].ptEnd.TraceCase.NetHeight, eRoundMode.NERAEST) .ToString();
			//    pSegmentLeg.UpperLimitAltitude = pDistance;
			// Length =====================================================

			double fSegmentLength = CurrLeg.TraceCase.Length;

			IDistance pDistance = new Distance();
			pDistance.Uom = mHUomDistance;
			pDistance.Value = UnitConverter.DistanceToDisplayUnits(fSegmentLength, eRoundMode.NERAEST).ToString();
			pSegmentLeg.Length = pDistance;

			//=================

			pDouble = new DoubleClass();
			pDouble.Value = Functions.RadToDeg(System.Math.Atan(CurrLeg.TraceCase.NetGrd));
			pSegmentLeg.VerticalAngle = pDouble;
			//=================
			ISpeed pSpeed = new Speed();
			pSpeed.Uom = mUomSpeed;
			pSpeed.Value = UnitConverter.SpeedToDisplayUnits(minAircraftSpeed2, eRoundMode.NERAEST);
			pSegmentLeg.SpeedLimit = pSpeed;
			pSegmentLeg.SpeedReference = SpeedReferenceType.IAS;

			// Start Point ========================
			pSegmentLeg.StartPoint = pTerminalSegmentPoint;
			// End Of Start Point ========================

			NavaidData GuidNav = CurrLeg.GuidanceNav;
			NavaidData SttIntersectNav = CurrLeg.InterceptionNav;

			// EndPoint ========================
			pTerminalSegmentPoint = null;

			if (CurrLeg.PathCode == SegmentPathType.AF || CurrLeg.PathCode == SegmentPathType.HF || CurrLeg.PathCode == SegmentPathType.IF ||
				CurrLeg.PathCode == SegmentPathType.TF || CurrLeg.PathCode == SegmentPathType.CF || CurrLeg.PathCode == SegmentPathType.DF ||
				CurrLeg.PathCode == SegmentPathType.RF || CurrLeg.PathCode == SegmentPathType.CD || CurrLeg.PathCode == SegmentPathType.CR ||
				CurrLeg.PathCode == SegmentPathType.VD || CurrLeg.PathCode == SegmentPathType.VR)
			{
				pTerminalSegmentPoint = new TerminalSegmentPoint();
				//        pTerminalSegmentPoint.IndicatorFACF =      ??????????
				//        pTerminalSegmentPoint.LeadDME =            ??????????
				//        pTerminalSegmentPoint.LeadRadial =         ??????????
				pTerminalSegmentPoint.Role = ProcedureFixRoleType.OTHER;

				ISegmentPoint pSegmentPoint = pTerminalSegmentPoint;
				pBoolean = new BooleanClass();
				pBoolean.Value = true;
				pSegmentPoint.FlyOver = pBoolean;

				pBoolean = new BooleanClass();
				pBoolean.Value = false;
				pSegmentPoint.RadarGuidance = pBoolean;

				pSegmentPoint.ReportingATC = ATCReportingType.NO_REPORT;
				pBoolean = new BooleanClass();
				pBoolean.Value = false;
				pSegmentPoint.Waypoint = pBoolean;

				//=======================================================================

				bool OnNav = false;
				if (SttIntersectNav.TypeCode != eNavaidClass.CodeNONE && GuidNav.TypeCode != eNavaidClass.CodeNONE && Functions.ReturnDistanceInMeters(SttIntersectNav.pPtPrj, GuidNav.pPtPrj) < GlobalVars.distEps)
				{
					if (Functions.ReturnDistanceInMeters(GuidNav.pPtPrj, CurrLeg.ptEnd.TraceCase.pPoint) < GlobalVars.distEps &&
					Functions.ReturnDistanceInMeters(SttIntersectNav.pPtPrj, CurrLeg.ptEnd.TraceCase.pPoint) < GlobalVars.distEps)
						OnNav = SttIntersectNav.TypeCode > eNavaidClass.CodeDME;
				}

				ISignificantPoint pSignificantPoint = new SignificantPoint();

				if (OnNav)
				{
					IString pString = new StringClass();
					pString.Value = SttIntersectNav.ID + ";" + Navaids_DataBase.GetNavTypeName(SttIntersectNav.TypeCode);

					pSignificantPoint.NavaidSystemId = pString;

					IAixmPoint pAixmPoint = new AixmPoint();
					pAixmPoint.AsIGMLPoint.PutCoord(SttIntersectNav.pPtGeo.X, SttIntersectNav.pPtGeo.Y);
					pSignificantPoint.Position = pAixmPoint;
				}
				else
				{
					IDesignatedPoint pFixDesignatedPoint = new DesignatedPoint();

					if (CurrLeg.bOnWPTFIX)
					{
						pFixDesignatedPoint.Id = CurrLeg.WptFix.ID;
						pFixDesignatedPoint.Designator = CurrLeg.WptFix.Name;
						pFixDesignatedPoint.Name = CurrLeg.WptFix.Name;

						pFixDesignatedPoint.Point = new AixmPoint();
						pFixDesignatedPoint.Point.AsIGMLPoint.PutCoord(CurrLeg.WptFix.pPtGeo.X, CurrLeg.WptFix.pPtGeo.Y);

						//            pFixDesignatedPoint.Note
						//            pFixDesignatedPoint.Tag
						pFixDesignatedPoint.Type = DesignatedPointType.OTHER;
					}
					else
					{
						pFixDesignatedPoint.Id = "0";
						pFixDesignatedPoint.Designator = "COORD";
						pFixDesignatedPoint.Name = "TP";

						pFixDesignatedPoint.Point = new AixmPoint();

						IPoint ptTmp = (IPoint)Functions.ToGeo(CurrLeg.ptEnd.TraceCase.pPoint);
						pFixDesignatedPoint.Point.AsIGMLPoint.PutCoord(ptTmp.X, ptTmp.Y);

						//            pFixDesignatedPoint.Note
						//            pFixDesignatedPoint.Tag
						pFixDesignatedPoint.Type = DesignatedPointType.DESIGNED;
					}
					pSignificantPoint.FixDesignatedPoint = pFixDesignatedPoint;
				}

				pSegmentPoint.PointChoice = pSignificantPoint;
				pSegmentLeg.EndPoint = pTerminalSegmentPoint;

				// Indication ======================================================================
				if (SttIntersectNav.TypeCode != eNavaidClass.CodeNONE)
				{
					IString pString = new StringClass();
					pString.Value = SttIntersectNav.ID + ";" + Navaids_DataBase.GetNavTypeName(SttIntersectNav.TypeCode);

					IAixmPoint pAixmPoint = new AixmPoint();
					pAixmPoint.AsIGMLPoint.PutCoord(SttIntersectNav.pPtGeo.X, SttIntersectNav.pPtGeo.Y);

					pSignificantPoint = new SignificantPoint();
					pSignificantPoint.NavaidSystemId = pString;
					pSignificantPoint.Position = pAixmPoint;

					if (SttIntersectNav.TypeCode == eNavaidClass.CodeDME)
					{
						double fDistToNav = Functions.ReturnDistanceInMeters(SttIntersectNav.pPtPrj, CurrLeg.ptEnd.TraceCase.pPoint);
						double fAltitude = SttIntersectNav.pPtPrj.Z - CurrLeg.ptEnd.TraceCase.NetHeight;			// .pPoint.Z;
						double fTmp = System.Math.Sqrt(fDistToNav * fDistToNav + fAltitude * fAltitude);

						pDistance = new Distance();
						pDistance.Uom = mHUomDistance;
						pDistance.Value = UnitConverter.DistanceToDisplayUnits(fTmp, eRoundMode.NERAEST).ToString();

						IDistanceIndication pDistanceIndication = new DistanceIndication();
						pDistanceIndication.Distance = pDistance;
						pDistanceIndication.PointChoice = pSignificantPoint;
						pSegmentLeg.DistanceIndication = pDistanceIndication;
					}
					else
					{
						double fDirToNav = Functions.ReturnAngleInDegrees(SttIntersectNav.pPtPrj, CurrLeg.ptEnd.TraceCase.pPoint);

						pDouble = new DoubleClass();
						pDouble.Value = NativeMethods.Modulus(Functions.Dir2Azt(SttIntersectNav.pPtPrj, fDirToNav) - SttIntersectNav.MagVar);

						IAngleIndication pAngleIndication = new AngleIndication();
						pAngleIndication.Angle = pDouble;
						pAngleIndication.AngleType = BearingType.MAG;
						pAngleIndication.PointChoice = pSignificantPoint;
						pSegmentLeg.AngleIndication = pAngleIndication;
					}
				}
				// End Of Indication ======================================================================
			}
			// End Of EndPoint ============================================================================

			// Trajectory =================================================================================
			IGMLPolyline pGMLPolyline = new GMLPolyline();
			IPointCollection mPoly = (IPointCollection)CurrLeg.TraceCase.pNominalPoly;

			//for each parts
			{
				IGMLPart pGMLPart = new GMLPart();
				for (int i = 0; i < mPoly.PointCount; i++)
				{
					IGMLPoint pGMLPoint = new GMLPoint();
					IPoint ptTmp = (IPoint)Functions.ToGeo(mPoly.Point[i]);

					pGMLPoint.PutCoord(ptTmp.X, ptTmp.Y);
					pGMLPart.Add(pGMLPoint);
				}

				pGMLPolyline.Add(pGMLPart);
			}

			IAIXMCurve pAIXMCurve = new AIXMCurve();
			pAIXMCurve.Polyline = pGMLPolyline;
			pSegmentLeg.Trajectory = pAIXMCurve;

			return result;
		}

		TerminalSegmentPoint DERPoint()
		{
			TerminalSegmentPoint pTerminalSegmentPoint = new TerminalSegmentPoint();
			//        pTerminalSegmentPoint.IndicatorFACF =      ??????????
			//        pTerminalSegmentPoint.LeadDME =            ??????????
			//        pTerminalSegmentPoint.LeadRadial =         ??????????

			pTerminalSegmentPoint.Role = ProcedureFixRoleType.OTHER;
			//==
			ISegmentPoint pSegmentPoint = pTerminalSegmentPoint;

			IBoolean pBoolean = new BooleanClass();
			pBoolean.Value = true;
			pSegmentPoint.FlyOver = pBoolean;

			pBoolean = new BooleanClass();
			pBoolean.Value = false;
			pSegmentPoint.RadarGuidance = pBoolean;

			pSegmentPoint.ReportingATC = ATCReportingType.NO_REPORT;
			pBoolean = new BooleanClass();
			pBoolean.Value = false;
			pSegmentPoint.Waypoint = pBoolean;
			//==
			SignificantPoint pSignificantPoint = new SignificantPoint();

			IString pString = new StringClass();
			pString.Value = GlobalVars.m_SelectedRWY.ptID[eRWY.PtEnd];
			pSignificantPoint.RunwayPointId = pString;

			AixmPoint pAixmPoint = new AixmPoint();
			pAixmPoint.AsIGMLPoint.PutCoord(GlobalVars.m_SelectedRWY.pPtGeo[eRWY.PtEnd].X, GlobalVars.m_SelectedRWY.pPtGeo[eRWY.PtEnd].Y);
			pSignificantPoint.Position = pAixmPoint;
			return pTerminalSegmentPoint;
		}

		bool SaveProcedure(string procName)
		{
			IDepartureLeg pDepartureLeg;

			ISegmentLegList pSegmentLegList;
			IProcedureTransition pTransition;

			IAIXMProcedure pProcedure;
			IAircraftCharacteristic IsLimitedTo;
			IString pString;
			IBoolean pBoolean;

			//Legs ======================================================================================================
			pSegmentLegList = new DepartureLegList();
			int NO_SEQ;
			TerminalSegmentPoint pTerminalSegmentPoint = DERPoint();
			//Leg 1 Straight Departure ===============================================================================
			//Standart Departure ===============================================================================
			for (NO_SEQ = 1; NO_SEQ <= LegCount; NO_SEQ++)
			{
				pDepartureLeg = CreateDepartureLeg(NO_SEQ, ref pTerminalSegmentPoint);
				pSegmentLegList.AsISIDList.Add(pDepartureLeg);
			}

			//Transition ==========================================================================
			pTransition = new ProcedureTransition();

			//    pTransition.Description =
			//    pTransition.ID =

			pTransition.LegList = pSegmentLegList;

			//    pTransition.Procedure =
			pString = new StringClass();
			pString.Value = GlobalVars.m_SelectedRWY.ID;
			pTransition.RunwayDirectionId = pString;

			//    pTransition.TransitionId = TextBox0???.Text

			pTransition.Type = ProcedurePhaseType.RWY;

			// Procedure =================================================================================================
			pProcedure = new StandardInstrumentDeparture();

			pProcedure.CodingStandard = ProcedureCodingStandardType.ARINC_424_18;
			//    pProcedure.CommunicationFailureDescription
			//    pProcedure.Description =
			pProcedure.DesignCriteria = DesignStandardType.PANS_OPS;
			//    pProcedure.FlightChecked =
			//    pProcedure.ID

			IsLimitedTo = new AircraftCharacteristic();
			IsLimitedTo.AircraftLandingCategory = AircraftCategoryType.C;

			if (comboBox0003.SelectedIndex == 1)
				IsLimitedTo.AircraftLandingCategory = AircraftCategoryType.D;

			pProcedure.IsLimitedTo = IsLimitedTo;

			//string sProcName = procName;

			//		if(sProcName = "")  //Must be FIX Name
			//		{
			//		}

			pProcedure.Name = procName;

			//    pProcedure.Note =
			//    pProcedure.ProtectsSafeAltitudeAreaId =
			pBoolean = new BooleanClass();
			pBoolean.Value = false;

			pProcedure.RNAV = pBoolean;
			pProcedure.ServicesAirportId = GlobalVars.m_CurrADHP.ID;

			pProcedure.TransitionList.Add(pTransition);

			try
			{
				DBModule.pObjectDir.SetProcedure(pProcedure);
				return true;
			}
			catch
			{
			}

			return false;
		}

	}
}
