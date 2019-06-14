#define WhileDebug

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aran.AranEnvironment;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using Aran.PANDA.RNAV.Approach.Properties;
using Aran.Geometries.Operators;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;
using Aran.Aim.Objects;
using Aran.Queries;

namespace Aran.PANDA.RNAV.Approach
{
	public partial class ApproachForm : Form
	{
		const int acA = 0;
		const int acB = 1;
		const int acC = 2;
		const int acD = 3;
		const int acDL = 4;

		const double AbvTrshldMinValue = 12;
		const double AbvTrshldMaxValue = 18;

		const double MaxIFDistance = 15.0 * 1852.0;

		const double MinMATurnDist = 2 * 1852.0;
		const double MaxMATurnDist = 100000.0;

		const double maxTurnAngle = 135.0;

		private const int saveFeatureCount = 5;

		private double[] banks = new double[] { 15, 20, 25 };

		#region variables

		private bool bFormInitialised;
		private bool Report = false;
		private int HelpContextID = 4100;

		private bool _skipEvents;
		private bool _showReport;

		private int CurrPage
		{
			set
			{
				tabControl1.SelectedIndex = value;
				Text = Resources.str00033 + " " + GlobalVars.thisAssemName.Version.ToString() + "   [" + Label1[value].Text + "]";
				//this.HelpContextID = 4000 + 100 * (value + 1);

				PrevBtn.Enabled = value > 0;

				if (value == 5)
				{
					NextBtn.Enabled = checkBox05_01.Checked;
					OkBtn.Enabled = !checkBox05_01.Checked;
				}
				else
				{
					NextBtn.Enabled = value < tabControl1.TabPages.Count - 1;
					OkBtn.Enabled = value == tabControl1.TabPages.Count - 1;
				}

				for (int i = 0; i < Label1.Length; i++)
				{
					//Label1[i].Visible = MultiPage1.TabPages[i].Visible;
					Label1[i].ForeColor = System.Drawing.Color.FromArgb(0XC0C0C0);
					Label1[i].Font = new System.Drawing.Font(Label1[i].Font, System.Drawing.FontStyle.Regular);
				}

				Label1[value].ForeColor = System.Drawing.Color.FromArgb(0XFF8000);
				Label1[value].Font = new System.Drawing.Font(Label1[value].Font, System.Drawing.FontStyle.Bold);

			}

			get { return tabControl1.SelectedIndex; }
		}

		private IScreenCapture screenCapture;
		private Label[] Label1;
		ReportFrm _reportForm;

		private UomSpeed mUomSpeed;
		private UomDistance mUomHDistance;
		private UomDistanceVertical mUomVDistance;

		private double _refElevation;
		private double _visualIAS;
		private double _visualTAS;
		private double TASwWind;

		//private double FRefElevation;
		//private double FMSA;
		//private double FRange;

		//FSignificantCollection:		TPandaCollection;
		//FRWYList:						TRWYList;
		//FRWYList:						TPandaList;
		//FRWYDirectionList:			TRwyDirectionList;
		//FObstacleList:				TPandaCollection;

		//============================================================ Page ??
		double FFinalTurnAltitude, FMATIAOCA, FMATAOCA, FMAFinalOCA;

		string FMATIAOCA_CO;//, FAppCaption;
		//string FMATAOCA_CO;

		#region page 1

		RWYType _selectedRWY;
		//private Point _RWYTHRPrj;
		Point FRWYDirection;                //_selectedTHR		_RWYTHRPrj
		Point FRWYDirectionGeo;
		Point _FicTHR;                      // TO DO:: Fill it (x, y ,z) 

		aircraftCategory _AirCat;           //_aircraftCategory;
		MultiPolygon _convexPoly;

		double FRWYCLDir;                   //_RWYCLDir;
		double prmAbvTrshld;

		double R__;
		double _Bank1;

		double _alignedApproachDir;
		double _notAlignedApproachDir;

		bool _listView0001InUse;

		MultiPoint _RWYCollection;
		Geometry _RWYsGeometry;

		#endregion

		#region page 2 - Final Approach track

		// visual ================================================================

		bool isOmni;
		bool haveIdeal;
		double idealDir;
		double selectedDir; //Final Approach track

		object vsSelected1;
		Point _centroid;
		Point vsAnchPt1;

		double minDesAngle, maxDesAngle;

		Interval visualInterval1, visualInterval2;
		Interval AztInterval0, AztInterval1;

		//========================================================================
		bool FAligned;
		eApproachType _approachType;
		eLandingType _landingType;
		double _tan_5;
		//double arMaxInterDist;
		double _arMaxInterDist;
		double _arMaxMaxInterDist;

		double _circlingMinOCH;

		//========================================================================

		Interval prmCourseInterval;
		double _approachTrack;    //Final Approach track

		double prmAbeamMinValue;
		double prmAbeamMaxValue;
		double _AbeamValue;

		double prmAlongMinValue;
		double prmAlongMaxValue;
		double _AlongValue;

		List<WPT_FIXType> FAlignedSgfPoint;
		List<WPT_FIXType> FNotAlignedSgfPoint;
		List<object> FCirclingSgfPoint;

		List<IntervalList> FNotAlignedSgfPointRange;

		//Input
		bool bFAFfromList;
		//bool bIFFromList;
		//bool bSDFFromList;

		int FFarMAPtHandle;
		int FTrackHandle;
		int FCLHandle;
		int VisualAreaElement;

		//List<List<LegBase>> FIABranchs;

		Point FFarMAPt;
		Point FAlignedAnchorPoint1;//, FAlignedAnchorPoint2;
		Point FNotAlignedAnchorPoint1;//, FNotAlignedAnchorPoint2;

		//Aligned
		bool FAlignedExistingAnchorPoint;

		Interval FAlignedTrackInterval;
		//Not Aligned
		bool FNotAlignedExistingAnchorPoint;


		Interval[] FNotAlignedTrackIntervals;

		#endregion

		#region page 3
		//=============================================================================
		string _patDefNamae, _FafDefNamae, _IfDefNamae, _MaptDefName, _MAHFDefName, _MATFDefName;
		double _FAFOCH, _FinalOCH;
		double _FAFSDF1OCH, _FAFSDF2OCH;
		double _prevImArea;
		int ixIF;

		Interval FAFAltitudeInterval;
		double _FAFAltitude;
		double prmFAFAltitude
		{
			get { return _FAFAltitude; }

			set
			{
				textBox02_01.Text = GlobalVars.unitConverter.HeightToDisplayUnits(value).ToString();
				textBox02_01.Tag = textBox02_01.Text;

				if (value == _FAFAltitude)
					return;

				_FAFAltitude = value;
				ApplayFAFAltitude();
			}
		}
		//=============================================================================

		Interval FAFDistanceInterval;
		double _FAFDistance;
		double prmFAFDistance
		{
			get { return _FAFDistance; }
			set
			{
				textBox02_02.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(value).ToString();
				textBox02_02.Tag = textBox02_02.Text;

				if (value == _FAFDistance)
					return;

				_FAFDistance = value;
				ApplayFAFDistance();
			}
		}

		//=============================================================================
		Interval FAFGradientInterval;
		double _FAFGradient;
		double prmFAFGradient
		{
			get { return _FAFGradient; }

			set
			{
				textBox02_05.Text = GlobalVars.unitConverter.GradientToDisplayUnits(value).ToString();
				textBox02_05.Tag = textBox02_05.Text;

				if (value == _FAFGradient)
					return;
				_FAFGradient = value;

				//FFAF.Gradient = _FAFGradient;

				ApplayFAFGradient();
			}
		}
		//=============================================================================

		Interval FAFIASInterval;
		double _FAFIAS;
		double prmFAFIAS
		{
			get { return _FAFIAS; }
			set
			{
				textBox02_04.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(value).ToString();
				textBox02_04.Tag = textBox02_04.Text;

				if (value == _FAFIAS)
					return;

				_FAFIAS = value;
				ApplayFAFIAS();
			}
		}

		//IF =========================================================================================

		Interval IFCourseInterval;
		double _IFCourse;
		double prmIFCourse
		{
			get { return _IFCourse; }

			set
			{
				double AppAzt = ARANFunctions.DirToAzimuth(_FAF.PrjPt, value, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

				textBox02_10.Text = GlobalVars.unitConverter.AzimuthToDisplayUnits(AppAzt).ToString();
				textBox02_10.Tag = textBox02_10.Text;

				if (ARANMath.SubtractAngles(_IFCourse, value) > ARANMath.EpsilonRadian) return;
				if (value == _IFCourse)
					return;

				_IFCourse = value;
				ApplayIFCourse();
			}
		}
		//=============================================================================

		Interval _IFIASInterval;
		double _IFSpeed;
		double prmIFSpeed
		{
			get { return _IFSpeed; }

			set
			{
				textBox02_08.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(value).ToString();
				textBox02_08.Tag = textBox02_08.Text;

				if (value == _IFSpeed)
					return;

				_IFSpeed = value;
				ApplayIFSpeed();
			}
		}
		//=============================================================================

		Interval _IFAltitudeInterval;
		double _IFAltitude;
		double prmIFAltitude
		{
			get { return _IFAltitude; }

			set
			{
				textBox02_13.Text = GlobalVars.unitConverter.HeightToDisplayUnits(value).ToString();
				textBox02_13.Tag = textBox02_13.Text;

				if (value == _IFAltitude)
					return;

				_IFAltitude = value;
				ApplayIFAltitude();
			}
		}
		//=============================================================================

		Interval _IFDistanceInterval;
		double _IFDistance;
		double prmIFDistance
		{
			get { return _IFDistance; }

			set
			{
				textBox02_09.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(value).ToString();
				textBox02_09.Tag = textBox02_09.Text;

				if (value == _IFDistance)
					return;

				_IFDistance = value;
				ApplayIFDist();
			}
		}

		//=============================================================================
		Interval _IFMaxAngleInterval;
		double _IFMaxAngle;
		double prmIFMaxAngle
		{
			get { return _IFMaxAngle; }
			set
			{
				textBox02_12.Text = GlobalVars.unitConverter.AngleToDisplayUnits(value).ToString();
				textBox02_12.Tag = textBox02_12.Text;

				if (value == _IFMaxAngle)
					return;

				_IFMaxAngle = value;
				ApplyIFDistanceAndDirection(_IFDistance, _IFCourse);
			}
		}

		//int maxReqHi;
		//int maxReqHf;

		ObstacleContainer _intermediateSignObsacles;

		double F3ApproachDir;
		double _TAS;

		//=== FAF
		//FIXInfo FFAFInfo;
		//=== IF
		//FIXInfo FIFInfo;

		MAPt FMAPt;
		//FIXInfo FMAPtInfo;

		FIX FMAHFstr;
		FIX FMAHFturn;
		//FIXInfo FMAHFInfo;

		//LegDep _straightMissedLeq;
		//Local

		//MultiPolygon FullPolyList, PrimaryPolyList;
		//List<ObstacleContainer> FObstacleListList;

		#endregion

		#region page 4
		//=== Final SDF
		LegApch _finalLeq;
		LegApch _finalSDFLeg;
		LegApch _finalSDF1Leg;
		LegApch _finalSDF2Leg;

		FIX _FAF;
		FIX _FinalFSDF;
		FIX _FinalFSDF1;
		FIX _FinalFSDF2;
		int _FinalSDFCnt;

		//=====//=====//=====//=====//=====//=====//=====

		MultiPolygon _FinalPrimary;
		MultiPolygon _FinalFull;

		MultiPolygon _FinalSDFPrimary;
		MultiPolygon _FinalSDFFull;

		MultiPolygon _FinalSDF1Primary;
		MultiPolygon _FinalSDF1Full;

		//MultiPolygon _FinalSDF2Primary;
		//MultiPolygon _FinalSDF2Full;

		ObstacleContainer FFinalObstalces;
		ObstacleContainer _FinalFSDFObstalces;

		string _FinalSDF0DefNamae, _FinalSDF1DefNamae;
		string _InterSDF0DefNamae, _InterSDF1DefNamae;

		Interval FinalSDFAltitudeInterval;
		double _FinalSDFAltitude;
		double prmFinalSDFAltitude
		{
			get { return _FinalSDFAltitude; }

			set
			{
				double chk = FinalSDFAltitudeInterval.CheckValue(value);

				textBox03_01.Text = GlobalVars.unitConverter.HeightToDisplayUnits(value).ToString();
				textBox03_01.Tag = textBox03_01.Text;

				if (value == _FinalSDFAltitude)
					return;

				_FinalSDFAltitude = value;
				ApplayFinalSDFAltitude();
			}
		}

		//==============================

		Interval FinalSDFDistanceInterval;
		double _FinalSDFDistance;
		double prmFinalSDFDistance
		{
			get { return _FinalSDFDistance; }

			set
			{
				textBox03_02.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(value).ToString();
				textBox03_02.Tag = textBox03_02.Text;

				if (value == _FinalSDFDistance)
					return;

				_FinalSDFDistance = value;
				ApplayFinalSDFDist();
			}
		}

		#endregion

		//=============================================================================

		#region page 5
		int _IntermSDFCnt;

		LegApch _intermediateLeq;
		LegApch _currIntermSDFLeg;
		LegApch _IntermSDF1Leg;
		LegApch _IntermSDF2Leg;

		//ObstacleContainer _IntermObstalces;

		FIX fixFAF;
		FIX fixIF;

		FIX _IntermSDF1;
		FIX _IntermSDF2;
		FIX _firstIF;
		FIX _prevIF;
		FIX _currIF;

		//=============================================================================

		Interval _IFSDFDistanceInterval;
		double _IFSDFDistance;
		double prmIFSDFDistance
		{
			get { return _IFSDFDistance; }

			set
			{
				textBox04_01.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(value).ToString();
				textBox04_01.Tag = textBox04_01.Text;

				if (value == _IFSDFDistance)
					return;

				_IFSDFDistance = value;
				ApplayIntermedSDFDist();
			}
		}

		//=============================================================================

		Interval _IFSDFAltitudeInterval;
		double _IntermedSDFAltitude;
		double prmIFSDFAltitude
		{
			get { return _IntermedSDFAltitude; }

			set
			{
				textBox04_03.Text = GlobalVars.unitConverter.HeightToDisplayUnits(value).ToString();
				textBox04_03.Tag = textBox04_03.Text;

				if (value == _IntermedSDFAltitude)
					return;

				_IntermedSDFAltitude = value;
				ApplayIntermedSDFAltitude();
			}
		}

		#endregion

		#region page 6

		double _currMOCLimit;
		//List<LegBase> Branch;

		Interval MAPtDistanceInterval;
		double _MAPtDistance;
		double prmMAPtDistance
		{
			get { return _MAPtDistance; }
			set
			{
				double dist = MAPtDistanceInterval.Max - value;

				textBox05_01.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(dist).ToString();
				textBox05_01.Tag = textBox05_01.Text;

				//if (value == _MAPtDistance)					return;
				if (Math.Abs(value - _MAPtDistance) < ARANMath.Epsilon_2Distance)
					return;

				_MAPtDistance = value;
				ApplayMAPtDistance();
			}
		}

		Interval MACourseInterval;
		double _MACourse;
		double prmMACourse
		{
			get { return _MACourse; }
			set
			{
				double dir = MACourseInterval.CheckValue(value);
				double AppAzt = ARANFunctions.DirToAzimuth(FMAPt.PrjPt, value, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

				textBox05_02.Text = GlobalVars.unitConverter.AzimuthToDisplayUnits(AppAzt).ToString();
				textBox05_02.Tag = textBox05_02.Text;

				if (value == _MACourse)
					return;

				_MACourse = value;
				ApplayMACourse();
			}
		}


		//Input
		//bool bMAPtFromList;

		MATF FMATF;
		//FIXInfo FMATFInfo;

		double _OCA, _OCH;
		double prmOCA
		{
			get { return _OCA; }
			set
			{
				_OCA = value;
				_OCH = _OCA - _refElevation;

				textBox05_05.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_OCA).ToString();
				textBox05_06.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_OCH).ToString();
				//textBox05_05.Tag = textBox05_05.Text;
				//textBox05_06.Tag = textBox05_06.Text;
			}
		}

		double _MAClimb;
		Interval _MAClimbInterval;

		double prmMAClimb
		{
			get { return _MAClimb; }
			set
			{
				textBox05_09.Text = GlobalVars.unitConverter.GradientToDisplayUnits(value).ToString();
				textBox05_09.Tag = textBox05_09.Text;

				if (value == _MAClimb)
					return;

				_MAClimb = value;
				ApplyMAClimb();
			}
		}

		double _SOCFromMAPtDistance;
		//Interval _SOCFromMAPtDistanceInterval;

		double prmSOCFromMAPtDistance
		{
			get { return _SOCFromMAPtDistance; }
			set
			{
				textBox05_04.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(value).ToString();
				_SOCFromMAPtDistance = value;
				FMAPt.SOCDistance = value;
			}
		}

		double _MAHFFromMAPtDist;
		Interval prmMAHFFromMAPtDistInterval;
		double prmMAHFFromMAPtDist
		{
			get { return _MAHFFromMAPtDist; }
			set
			{
				textBox05_13.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(value).ToString();
				textBox05_13.Tag = textBox05_13.Text;

				if (value == _MAHFFromMAPtDist)
					return;

				_MAHFFromMAPtDist = value;
				ApplyMAHFFromMAPtDist();
			}
		}

		#endregion

		#region page 7
		//bool bTurnExistingPoint;

		//int FMAFullPolyGr, FMAPrimPolyGr;

		LegApch _straightMissedLeq;

		//SOC
		double FMAMinOCH;
		double FFAAligOCA;
		double FFAMAPtPosOCA;
		//	FFAObstOCA,

		double FFAOCA;
		double FIMALenght;
		double FMAHFLenght;

		FIX FSOC;
		//	FptSOC:							TPoint;
		//FIXInfo FSOCInfo;

		MultiPolygon SMASFullPolyList, SMASPrimaryPolyList;

		//ObstacleContainer SMASFObstacleListList;
		//MultiPolygon SMATFullPolyList, SMATPrimaryPolyList;

		ObstacleContainer MATAObstacleList;
		ObstacleContainer TIAObstacleList;
		#endregion

		#region page 8
		int kkLineElem;//, primePolyEl, secondPolyEl;

		MultiPolygon SMATFullPolyList, SMATPrimaryPolyList;
		MultiPolygon primeTAPoly, secondTAPoly;


		LegBase _straightTIALeq;
		LegBase _turnLeg;
		//========================================================

		double _preTurnAltitude;
		Interval prmPreTurnAltitudeInterval;

		double prmPreTurnAltitude
		{
			get { return _preTurnAltitude; }

			set
			{
				textBox06_04.Text = GlobalVars.unitConverter.HeightToDisplayUnits(value).ToString();
				textBox06_04.Tag = textBox06_04.Text;

				if (Math.Abs(value - _preTurnAltitude) < ARANMath.EpsilonDistance)
					return;

				_preTurnAltitude = value;
				if (!textBox06_04.ReadOnly)     //????
					ApplyPreTurnAltitude();
			}
		}

		bool prmPreTurnAltitudeReadOnly
		{
			set { textBox06_04.ReadOnly = value; }
		}
		//========================================================

		double _MATBankAngle;
		Interval prmMATBankAngleInterval;

		double prmMATBankAngle
		{
			get { return _MATBankAngle; }
			set
			{
				textBox06_01.Text = GlobalVars.unitConverter.AngleToDisplayUnits(value).ToString();
				textBox06_01.Tag = textBox06_01.Text;

				if (value == _MATBankAngle)
					return;

				_MATBankAngle = value;
				ApplyMATBankAngle();
			}
		}
		//========================================================

		double _PreOCA;

		double prmPreOCA
		{
			get { return _PreOCA; }
			set
			{
				textBox06_05.Text = GlobalVars.unitConverter.HeightToDisplayUnits(value).ToString();
				textBox06_05.Tag = textBox06_05.Text;
				_PreOCA = value;
			}
		}

		//========================================================

		double _TurnFromMAPtDist;
		Interval prmTurnFromMAPtDistInterval;

		double prmTurnFromMAPtDist
		{
			get { return _TurnFromMAPtDist; }
			set
			{
				textBox06_03.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(value).ToString();
				textBox06_03.Tag = textBox06_03.Text;

				if (value == _TurnFromMAPtDist)
					return;

				_TurnFromMAPtDist = value;
				ApplyTurnFromMAPtDist();
			}
		}

		bool prmTurnFromMAPtDistReadOnly
		{
			set { textBox06_03.ReadOnly = value; }
		}

		//========================================================

		double _MAIAS;
		Interval prmMAIASInterval;

		double prmMAIAS
		{
			get { return _MAIAS; }

			set
			{
				textBox06_02.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(value).ToString();
				textBox06_02.Tag = textBox06_02.Text;

				if (value == _MAIAS)
					return;

				_MAIAS = value;
				ApplyMAIAS();
			}
		}

		//bool prmMAIASReadOnly
		//{
		//	set { }
		//}
		//========================================================

		double _NWPTFromTPDist;
		Interval prmNWPTFromTPDistInterval;

		double prmNWPTFromTPDist
		{
			get { return _NWPTFromTPDist; }
			set
			{
				textBox06_10.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(value).ToString();
				textBox06_10.Tag = textBox06_10.Text;

				if (value == _NWPTFromTPDist)
					return;

				_NWPTFromTPDist = value;

				if (!textBox06_10.ReadOnly)
					ReCreateMATurnArea();// ApplyNWPTFromTPDist();
			}
		}

		bool prmNWPTFromTPDistReadOnly
		{
			set { textBox06_10.ReadOnly = value; }
		}

		//========================================================

		double _TurnCourse;
		Interval prmTurnCourseInterval;

		double prmTurnCourse
		{
			get { return _TurnCourse; }
			set
			{
				double AppAzt;
				//double AppAzt = ARANFunctions.DirToAzimuth(FMAPt.PrjPt, value, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

				if ((eTurnAt)comboBox06_01.SelectedItem == eTurnAt.MApt)
					AppAzt = ARANFunctions.DirToAzimuth(FMAPt.PrjPt, value, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				else
					AppAzt = ARANFunctions.DirToAzimuth(FMATF.PrjPt, value, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

				textBox06_11.Text = GlobalVars.unitConverter.AzimuthToDisplayUnits(AppAzt).ToString();
				textBox06_11.Tag = textBox06_11.Text;

				if (value == _TurnCourse)
					return;

				_TurnCourse = value;
				//if(!textBox06_11.ReadOnly)
				ApplyTurnCourse();
			}
		}

		bool prmTurnCourseReadOnly
		{
			set { textBox06_11.ReadOnly = value; }
		}

		//========================================================

		double _TerminationAltitude;
		//Interval prmTerminationAltitudeInterval;

		double prmTerminationAltitude
		{
			get { return _TerminationAltitude; }

			set
			{
				textBox06_06.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(value).ToString();
				textBox06_06.Tag = textBox06_06.Text;

				if (value == _TerminationAltitude)
					return;

				_TerminationAltitude = value;
				FMAHFturn.ConstructAltitude = FMAHFturn.NomLineAltitude = value;
				//ApplyTerminationAltitude();
			}
		}

		//bool prmTerminationAltitudeReadOnly
		//{
		//	set { }
		//}

		//========================================================
		//double prmTurn1
		//{
		//	set
		//	{
		//		double AppAzt;

		//		if ((eTurnAt)comboBox06_01.SelectedItem == eTurnAt.MApt)
		//			AppAzt = ARANFunctions.DirToAzimuth(FMAPt.PrjPt, value, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
		//		else
		//			AppAzt = ARANFunctions.DirToAzimuth(FMATF.PrjPt, value, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

		//		textBox06_12.Text = GlobalVars.unitConverter.AzimuthToDisplayUnits(AppAzt).ToString();
		//	}
		//}

		//bool prmTurn1Visible
		//{
		//	set
		//	{
		//		label06_28.Visible = value;
		//		textBox06_12.Visible = value;
		//		label06_29.Visible = value;
		//	}
		//}

		//double prmTurn2
		//{
		//	set
		//	{
		//		double AppAzt;

		//		if ((eTurnAt)comboBox06_01.SelectedItem == eTurnAt.MApt)
		//			AppAzt = ARANFunctions.DirToAzimuth(FMAPt.PrjPt, value, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
		//		else
		//			AppAzt = ARANFunctions.DirToAzimuth(FMATF.PrjPt, value, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

		//		textBox06_13.Text = GlobalVars.unitConverter.AzimuthToDisplayUnits(AppAzt).ToString();
		//	}
		//}

		//double prmTurn3
		//{
		//	set
		//	{
		//		double AppAzt;// = ARANFunctions.DirToAzimuth(FMAPt.PrjPt, value, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

		//		if ((eTurnAt)comboBox06_01.SelectedItem == eTurnAt.MApt)
		//			AppAzt = ARANFunctions.DirToAzimuth(FMAPt.PrjPt, value, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
		//		else
		//			AppAzt = ARANFunctions.DirToAzimuth(FMATF.PrjPt, value, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

		//		textBox06_14.Text = GlobalVars.unitConverter.AzimuthToDisplayUnits(AppAzt).ToString();
		//	}
		//}

		//bool prmTurn3Visible
		//{
		//	set
		//	{
		//		label06_32.Visible = value;
		//		textBox06_14.Visible = value;
		//		label06_33.Visible = value;
		//	}
		//}

		#endregion

		#region page 9
		double _MOCLimit;

		Interval numericUpDown07_01Range;
		List<WayPoint> _SignificantPoints;
		Transitions transitions;
		private double[] EnrouteMOCValues = { 300.0, 450.0, 600.0 };
		WayPoint _AddedFIX;

		#endregion

		#region page 10
		InstrumentApproachProcedure Procedure;

		#endregion

		#endregion

		#region Main Form

		public ApproachForm()
		{
			bFormInitialised = false;
			_skipEvents = false;
			InitializeComponent();

			int DecimalPlaces = 0;
			if (GlobalVars.settings.AnglePrecision < 1.0)
				DecimalPlaces = (int)Math.Ceiling(-Math.Log10(GlobalVars.settings.AnglePrecision));

			numericUpDown01_01.DecimalPlaces = DecimalPlaces;

			Label1 = new Label[] { label01, label02, label03, label04, label05, label06, label07, label08, label09 };

			UomDistance[] uomDistHorTab = new UomDistance[] { UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI };
			UomDistanceVertical[] uomDistVerTab = new UomDistanceVertical[] { UomDistanceVertical.M, UomDistanceVertical.FT }; //, UomDistanceVertical.OTHER, UomDistanceVertical.OTHER, UomDistanceVertical.OTHER};
			UomSpeed[] uomSpeedTab = new UomSpeed[] { UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN, UomSpeed.FT_SEC };

			mUomHDistance = uomDistHorTab[GlobalVars.unitConverter.DistanceUnitIndex];
			mUomVDistance = uomDistVerTab[GlobalVars.unitConverter.HeightUnitIndex];
			mUomSpeed = uomSpeedTab[GlobalVars.unitConverter.SpeedUnitIndex];

			screenCapture = GlobalVars.gAranEnv.GetScreenCapture(Aim.FeatureType.StandardInstrumentDeparture.ToString());

			Label1[0].Text = Resources.str00300;
			Label1[1].Text = Resources.str00301;
			Label1[2].Text = Resources.str00302;
			Label1[3].Text = Resources.str00303;
			Label1[4].Text = Resources.str00304;
			Label1[5].Text = Resources.str00305;
			Label1[6].Text = Resources.str00306;
			Label1[7].Text = Resources.str00307;
			Label1[8].Text = Resources.str00308;

			tabControl1.TabPages[0].Text = Resources.str00300;
			tabControl1.TabPages[1].Text = Resources.str00301;
			tabControl1.TabPages[2].Text = Resources.str00302;
			tabControl1.TabPages[3].Text = Resources.str00303;
			tabControl1.TabPages[4].Text = Resources.str00304;
			tabControl1.TabPages[5].Text = Resources.str00305;
			tabControl1.TabPages[6].Text = Resources.str00306;
			tabControl1.TabPages[7].Text = Resources.str00307;
			tabControl1.TabPages[8].Text = Resources.str00308;

			this.CurrPage = 0;

			PrevBtn.Text = Resources.str00002;
			NextBtn.Text = Resources.str00003;
			OkBtn.Text = Resources.str00004;
			CancelBtn.Text = Resources.str00005;
			ReportBtn.Text = Resources.str00006;

			//Page 1 ==================================================================================================================================
			//label00_01.Text = Resources.str01000;
			//groupBox00_01.Text = Resources.str01007;
			radioButton00_01.Text = Resources.str01008;
			radioButton00_02.Text = Resources.str01009;

			label00_02.Text = Resources.str01001;
			label00_03.Text = Resources.str01002;
			label00_05.Text = Resources.str01003;
			label00_07.Text = Resources.str01004;
			label00_09.Text = Resources.str01005;
			label00_11.Text = Resources.str01006;

			label00_08.Text = GlobalVars.unitConverter.HeightUnit;
			label00_10.Text = GlobalVars.unitConverter.HeightUnit;

			prmAbvTrshld = GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value;

			textBox00_04.Text = GlobalVars.unitConverter.HeightToDisplayUnits(prmAbvTrshld).ToString();

			comboBox00_02.Items.Clear();

			foreach (RWYType rwy in GlobalVars.RWYList)
				comboBox00_02.Items.Add(rwy);

			if (comboBox00_02.Items.Count == 0)
			{
				MessageBox.Show("There is not ani RwyDirections.");
				Close();
			}

			listView00_01.Items.Clear();

			//fRefHeight = CurrADHP.pPtGeo.Z

			_listView0001InUse = false;
			_RWYCollection = new Geometries.MultiPoint();

			int n = GlobalVars.RWYList.Length;
			for (int i = 0; i < n; i++)
			{
				GlobalVars.RWYList[i].Selected = true;
				if ((i & 1) == 0)
				{
					listView00_01.Items.Add(GlobalVars.RWYList[i].Name + " / " + GlobalVars.RWYList[i].PairName);
					listView00_01.Items[i / 2].Checked = true;
				}
			}

			comboBox00_02.SelectedIndex = 0;
			comboBox00_03.SelectedIndex = 0;

			//prmAbvTrshld
			//Page 2 ====================================================================================================================================
			label01_02.Text = GlobalVars.unitConverter.HeightUnit;
			label01_06.Text = GlobalVars.unitConverter.HeightUnit;
			label01_11.Text = GlobalVars.unitConverter.HeightUnit;
			label01_13.Text = GlobalVars.unitConverter.HeightUnit;
			label01_17.Text = GlobalVars.unitConverter.DistanceUnit;

			groupBox01_01.Text = Resources.str02000;
			groupBox01_02.Text = Resources.str02001;
			groupBox01_04.Text = Resources.str02022;        //Resources.str02002;
			groupBox01_06.Text = Resources.str02022;

			radioButton01_01.Text = Resources.str02003;
			radioButton01_02.Text = Resources.str02004;
			radioButton01_03.Text = Resources.str02005;
			radioButton01_04.Text = Resources.str02006;
			radioButton01_05.Text = Resources.str02024;     // Resources.str02007;
			radioButton01_06.Text = Resources.str02006;     // Resources.str02025;     // Resources.str02008;
			radioButton01_07.Text = Resources.str02024;
			radioButton01_08.Text = Resources.str02006;     // Resources.str02025;

			//label01_01.Text = Resources.str02009;           //Resources.str02010;
			//label01_03.Text = Resources.str02011;
			label01_05.Text = Resources.str02012;
			//label01_07.Text = Resources.str02013;
			//label01_08.Text = Resources.str02014;
			label01_10.Text = Resources.str02015;
			label01_12.Text = Resources.str02016;
			label01_14.Text = Resources.str02020;
			label01_16.Text = Resources.str02021;
			label01_18.Text = Resources.str02023;

			tabPage01_01.Text = Resources.str02018;
			tabPage01_02.Text = Resources.str02019;

			dataGridView01_01.Columns[0].HeaderText = Resources.str02026;
			dataGridView01_01.Columns[1].HeaderText = Resources.str02027;
			dataGridView01_01.Columns[2].HeaderText = Resources.str02028;

			_approachType = eApproachType.LNAV;
			_landingType = eLandingType.StraightIn;
			FAligned = true;
			_tan_5 = Math.Tan(GlobalVars.constants.Pansops[ePANSOPSData.arStrInAlignment].Value);
			_arMaxInterDist = GlobalVars.constants.Pansops[ePANSOPSData.arMinInterDist].Value + GlobalVars.constants.Pansops[ePANSOPSData.arMinInterToler].Value / _tan_5;
			_arMaxMaxInterDist = 2.0 * _arMaxInterDist;

			FNotAlignedTrackIntervals = new Interval[2];

			FAlignedSgfPoint = new List<WPT_FIXType>();
			FNotAlignedSgfPoint = new List<WPT_FIXType>();
			FCirclingSgfPoint = new List<object>();

			FNotAlignedSgfPointRange = new List<IntervalList>();

			//FIABranchs = new List<List<LegBase>>();

			prmCourseInterval.Circular = true;
			prmCourseInterval.InDegree = false;

			FAlignedTrackInterval.Circular = true;
			FAlignedTrackInterval.InDegree = false;

			MACourseInterval.Circular = true;
			MACourseInterval.InDegree = false;

			prmTurnCourseInterval.Circular = true;
			prmTurnCourseInterval.InDegree = false;

			visualInterval1.Circular = true;
			visualInterval2.Circular = true;

			AztInterval0.Circular = true;
			AztInterval1.Circular = true;

			AztInterval0.InDegree = true;
			AztInterval1.InDegree = true;

			//FAlignedTrackInterval.Circular = true;
			//FAlignedTrackInterval.InDegree = false;

			prmAbeamMinValue = -GlobalVars.constants.Pansops[ePANSOPSData.arMinInterToler].Value;
			prmAbeamMaxValue = GlobalVars.constants.Pansops[ePANSOPSData.arMinInterToler].Value;

			prmAlongMinValue = GlobalVars.constants.Pansops[ePANSOPSData.arMinInterDist].Value;
			prmAlongMaxValue = _arMaxInterDist;

			textBox01_02.Text = GlobalVars.unitConverter.HeightToDisplayUnits(prmAlongMaxValue).ToString();
			textBox01_02.Tag = textBox01_02.Text;

			//Page 3 ===============================================================

			groupBox02_01.Text = Resources.str03001;

			radioButton02_01.Text = Resources.str03004;
			radioButton02_02.Text = Resources.str03005;
			radioButton02_03.Text = Resources.str03006;

			label02_06.Text = Resources.str03007;
			label02_09.Text = Resources.str03024;
			label02_10.Text = Resources.str03020;


			label02_01.Text = GlobalVars.unitConverter.HeightUnit;
			label02_03.Text = GlobalVars.unitConverter.DistanceUnit;
			label02_05.Text = GlobalVars.unitConverter.SpeedUnit;

			label02_08.Text = GlobalVars.unitConverter.HeightUnit;

			comboBox02_02.Items.Add(Resources.str03022);
			comboBox02_02.Items.Add(Resources.str03023);
			comboBox02_02.SelectedIndex = 0;

			groupBox02_02.Text = Resources.str03002;
			groupBox02_03.Text = Resources.str03003;

			radioButton02_04.Text = Resources.str03008;
			radioButton02_05.Text = Resources.str03009;
			checkBox02_01.Text = Resources.str03010;
			label02_11.Text = Resources.str03011;
			label02_12.Text = Resources.str03012;

			radioButton02_06.Text = Resources.str03013;
			radioButton02_07.Text = Resources.str03014;

			label02_14.Text = Resources.str03015;
			label02_16.Text = Resources.str03016;
			label02_18.Text = Resources.str03017;
			label02_20.Text = Resources.str03018;
			label02_22.Text = Resources.str03026;

			checkBox02_02.Text = Resources.str03019;
			checkBox02_03.Text = Resources.str03021;

			label02_13.Text = GlobalVars.unitConverter.SpeedUnit;
			label02_15.Text = GlobalVars.unitConverter.DistanceUnit;
			label02_23.Text = GlobalVars.unitConverter.HeightUnit;

			_intermediateSignObsacles.Obstacles = new Obstacle[0];
			_intermediateSignObsacles.Parts = new ObstacleData[0];

			_currMOCLimit = EnrouteMOCValues[0];

			//eFIXRole.MATF_LE_56
			FMAHFstr = new MAPt(eFIXRole.MATF_LE_56, GlobalVars.gAranEnv);
			FMAHFstr.FlightPhase = eFlightPhase.MApLT28;
			//FMAHFstr.SensorType = eSensorType.GNSS;
			FMAHFstr.ISAtC = GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value;

			FMAHFturn = new MAPt(eFIXRole.MATF_LE_56, GlobalVars.gAranEnv);
			FMAHFturn.SensorType = eSensorType.GNSS;
			//FMAHFturn.PBNType = ePBNClass.RNP_APCH;
			FMAHFturn.FlightPhase = eFlightPhase.MApGE28;
			FMAHFturn.ISAtC = GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value;

			//FIX FMAHFstr;
			//FIX FMAHFturn;

			FMAPt = new MAPt(eFIXRole.MAPt_, GlobalVars.gAranEnv);
			FMAPt.SensorType = eSensorType.GNSS;
			FMAPt.ISAtC = GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value;
			FMAPt.FlyMode = eFlyMode.Flyover;
			//FMAPt.PBNType               = ePBNClass.RNP_APCH;
			FMAPt.FlightPhase = eFlightPhase.FAFApch;

			string adhpName = GlobalVars.CurrADHP.ToString();

			_patDefNamae = "";
			if (adhpName.Length > 2)
				_patDefNamae = adhpName.Substring(2, adhpName.Length > 4 ? 2 : adhpName.Length - 2);

			Random rnd = new Random();

			_FafDefNamae = _patDefNamae + (rnd.Next() % 1000).ToString("000");

			_FinalSDF0DefNamae = _patDefNamae + (rnd.Next() % 1000).ToString("000");

			while (_FinalSDF0DefNamae == _FafDefNamae)
				_FinalSDF0DefNamae = _patDefNamae + (rnd.Next() % 1000).ToString("000");

			_FinalSDF1DefNamae = _patDefNamae + (rnd.Next() % 1000).ToString("000");

			while (_FinalSDF1DefNamae == _FafDefNamae || _FinalSDF1DefNamae == _FinalSDF0DefNamae)
				_FinalSDF1DefNamae = _patDefNamae + (rnd.Next() % 1000).ToString("000");

			_IfDefNamae = _patDefNamae + (rnd.Next() % 1000).ToString("000");
			while (_IfDefNamae == _FafDefNamae || _IfDefNamae == _FinalSDF0DefNamae || _IfDefNamae == _FinalSDF1DefNamae)
				_IfDefNamae = _patDefNamae + (rnd.Next() % 1000).ToString("000");

			_InterSDF0DefNamae = _patDefNamae + (rnd.Next() % 1000).ToString("000");
			while (_InterSDF0DefNamae == _FafDefNamae || _InterSDF0DefNamae == _FinalSDF0DefNamae || _InterSDF0DefNamae == _FinalSDF1DefNamae || _InterSDF0DefNamae == _IfDefNamae)
				_InterSDF0DefNamae = _patDefNamae + (rnd.Next() % 1000).ToString("000");

			_InterSDF1DefNamae = _patDefNamae + (rnd.Next() % 1000).ToString("000");
			while (_InterSDF1DefNamae == _FafDefNamae || _InterSDF1DefNamae == _FinalSDF0DefNamae || _InterSDF1DefNamae == _FinalSDF1DefNamae || _InterSDF1DefNamae == _IfDefNamae || _InterSDF1DefNamae == _InterSDF0DefNamae)
				_InterSDF1DefNamae = _patDefNamae + (rnd.Next() % 1000).ToString("000");

			_MaptDefName = _patDefNamae + (rnd.Next() % 1000).ToString("000");
			while (_MaptDefName == _InterSDF1DefNamae || _MaptDefName == _FafDefNamae || _MaptDefName == _FinalSDF0DefNamae || _MaptDefName == _FinalSDF1DefNamae || _MaptDefName == _IfDefNamae || _MaptDefName == _InterSDF0DefNamae)
				_MaptDefName = _patDefNamae + (rnd.Next() % 1000).ToString("000");

			_MAHFDefName = _patDefNamae + (rnd.Next() % 1000).ToString("000");
			while (_MAHFDefName == _MaptDefName || _MAHFDefName == _InterSDF1DefNamae || _MAHFDefName == _FafDefNamae || _MAHFDefName == _FinalSDF0DefNamae || _MAHFDefName == _FinalSDF1DefNamae || _MAHFDefName == _IfDefNamae || _MAHFDefName == _InterSDF0DefNamae)
				_MAHFDefName = _patDefNamae + (rnd.Next() % 1000).ToString("000");

			_MATFDefName = _patDefNamae + (rnd.Next() % 1000).ToString("000");
			while (_MATFDefName == _MAHFDefName || _MATFDefName == _MaptDefName || _MATFDefName == _InterSDF1DefNamae || _MATFDefName == _FafDefNamae || _MATFDefName == _FinalSDF0DefNamae || _MATFDefName == _FinalSDF1DefNamae || _MATFDefName == _IfDefNamae || _MATFDefName == _InterSDF0DefNamae)
				_MATFDefName = _patDefNamae + (rnd.Next() % 1000).ToString("000");

			textBox02_03.Text = _FafDefNamae;
			textBox02_11.Text = _IfDefNamae;
			textBox03_03.Text = _FinalSDF0DefNamae;
			textBox04_02.Text = _InterSDF1DefNamae;

			textBox05_03.Text = _MaptDefName;
			textBox06_15.Text = _MAHFDefName;

			FMAPt.Name = FMAPt.CallSign = _MaptDefName;
			FMAHFstr.Name = FMAHFstr.CallSign = _MAHFDefName;
			FMAHFturn.Name = FMAHFturn.CallSign = _MAHFDefName;

			_FAF = new FIX(eFIXRole.FAF_, GlobalVars.gAranEnv);
			_FAF.SensorType = eSensorType.GNSS;
			_FAF.ISAtC = GlobalVars.CurrADHP.ISAtC;         //FFAF.ISAtC = GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value;
			_FAF.FlightPhase = eFlightPhase.FAFApch;

			fixFAF = new FIX(eFIXRole.FAF_, GlobalVars.gAranEnv);
			fixIF = new FIX(eFIXRole.IF_, GlobalVars.gAranEnv);

			_IntermSDFCnt = 0;

			_IntermSDF2 = new FIX(eFIXRole.IF_, GlobalVars.gAranEnv);
			_IntermSDF2.SensorType = eSensorType.GNSS;
			_IntermSDF2.ISAtC = GlobalVars.CurrADHP.ISAtC;
			//_IntermSDF2.FlightPhase = eFlightPhase.IIAP;
			_IntermSDF2.Role = eFIXRole.SDF;
			_IntermSDF2.Name = _InterSDF1DefNamae;

			_IntermSDF1 = new FIX(eFIXRole.IF_, GlobalVars.gAranEnv);
			_IntermSDF1.SensorType = eSensorType.GNSS;
			_IntermSDF1.ISAtC = GlobalVars.CurrADHP.ISAtC;
			//_IntermSDF1.FlightPhase = eFlightPhase.IIAP;
			_IntermSDF1.Role = eFIXRole.SDF;
			_IntermSDF1.Name = _InterSDF0DefNamae;

			_firstIF = new FIX(eFIXRole.IF_, GlobalVars.gAranEnv);
			_firstIF.SensorType = eSensorType.GNSS;
			_firstIF.ISAtC = GlobalVars.CurrADHP.ISAtC;
			//_firstIF.FlightPhase = eFlightPhase.IIAP;

			_IntermSDF2Leg = new LegApch(_IntermSDF2, _IntermSDF1, GlobalVars.gAranEnv);
			_IntermSDF1Leg = new LegApch(_IntermSDF1, _firstIF, GlobalVars.gAranEnv);

			_intermediateLeq = new LegApch(_firstIF, _FAF, GlobalVars.gAranEnv);
			_finalLeq = new LegApch(_FAF, FMAPt, GlobalVars.gAranEnv, _intermediateLeq);
			//_straightMissedLeq = new LegDep(FMAPt, FMAHF, GlobalVars.gAranEnv, _finalLeq);

			//_firstIF = _IF;

			//Page 4 ===============================================================
			groupBox03_01.Text = Resources.str04001;
			radioButton03_01.Text = Resources.str04002;
			radioButton03_02.Text = Resources.str04003;
			radioButton03_03.Text = Resources.str04004;

			label03_05.Text = Resources.str03024;
			label03_01.Text = GlobalVars.unitConverter.HeightUnit;
			label03_03.Text = GlobalVars.unitConverter.DistanceUnit;
			label03_04.Text = GlobalVars.unitConverter.HeightUnit;

			comboBox03_02.Items.Add(Resources.str03022);
			comboBox03_02.Items.Add(Resources.str03023);
			//comboBox03_02.SelectedIndex = 0;

			button03_01.Text = Resources.str04005;
			button03_02.Text = Resources.str04006;

			//FSDF = new FIX(eFIXRole.SDF, GlobalVars.gAranEnv);
			//FSDF.SensorType = eSensorType.GNSS;
			//FSDF.ISAtC = GlobalVars.CurrADHP.ISAtC;
			//FSDF.FlightPhase = eFlightPhase.FAFApch;

			_FinalFSDF1 = new FIX(eFIXRole.SDF, GlobalVars.gAranEnv);
			_FinalFSDF1.SensorType = eSensorType.GNSS;
			_FinalFSDF1.ISAtC = GlobalVars.CurrADHP.ISAtC;
			_FinalFSDF1.FlightPhase = eFlightPhase.FAFApch;
			_FinalFSDF1.Name = _FinalSDF0DefNamae;

			_FinalFSDF2 = new FIX(eFIXRole.SDF, GlobalVars.gAranEnv);
			_FinalFSDF2.SensorType = eSensorType.GNSS;
			_FinalFSDF2.ISAtC = GlobalVars.CurrADHP.ISAtC;
			_FinalFSDF2.FlightPhase = eFlightPhase.FAFApch;
			_FinalFSDF2.Name = _FinalSDF1DefNamae;

			_finalSDF1Leg = new LegApch(_FinalFSDF1, FMAPt, GlobalVars.gAranEnv, _intermediateLeq);
			_finalSDF2Leg = new LegApch(_FinalFSDF2, FMAPt, GlobalVars.gAranEnv, _intermediateLeq);

			//Page 5 ===============================================================
			ixIF = -1;

			groupBox04_01.Text = Resources.str05001;
			radioButton04_01.Text = Resources.str05002;
			radioButton04_02.Text = Resources.str05003;
			label04_01.Text = Resources.str05004;
			label04_03.Text = Resources.str03017;
			label04_05.Text = Resources.str03026;
			label04_08.Text = Resources.str03024;

			label04_02.Text = GlobalVars.unitConverter.DistanceUnit;
			label04_06.Text = GlobalVars.unitConverter.HeightUnit;
			label04_07.Text = GlobalVars.unitConverter.HeightUnit;

			comboBox04_02.Items.Add(Resources.str03022);
			comboBox04_02.Items.Add(Resources.str03023);
			//comboBox04_02.SelectedIndex = 0;

			button04_01.Text = Resources.str04005;
			button04_02.Text = Resources.str04006;

			//Page 6 ===============================================

			//comboBox05_02.Items.Clear();
			//comboBox05_02.Items.Add("RNP APCH");
			//comboBox05_02.Items.Add("RNAV 1");
			//comboBox05_02.Items.Add("RNAV 2");

			comboBox05_02.SelectedIndex = 0;
			//Branch = new List<LegBase>();

			FSOC = new FIX(GlobalVars.gAranEnv);

			FMATF = new MATF(GlobalVars.gAranEnv);
			//FMATF.Role = eFIXRole.MATF_LE_56; 
			//FMATF.FlightPhase = eFlightPhase.MApLT28;
			//FMATF.SensorType = eSensorType.GNSS;
			//FMATF.PBNType = ePBNClass.RNP_APCH;

			FMATF.maPt = FMAPt;
			//FMATF.ISAtC = GlobalVars.CurrADHP.ISAtC;
			FMATF.ISAtC = GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value;
			FMATF.Name = FMATF.CallSign = _MATFDefName;

			_straightMissedLeq = new LegApch(FMAPt, FMAHFstr, GlobalVars.gAranEnv); //FMATF
			_straightTIALeq = new LegApch(FMAPt, FMATF, GlobalVars.gAranEnv);
			_turnLeg = new LegBase(FMATF, FMAHFturn, GlobalVars.gAranEnv, _straightTIALeq);

			groupBox05_01.Text = Resources.str06001;
			radioButton05_01.Text = Resources.str06002;
			radioButton05_02.Text = Resources.str06003;

			label05_02.Text = Resources.str02024;
			label05_04.Text = Resources.str03017;
			label05_05.Text = Resources.str06004;
			label05_07.Text = Resources.str06005;
			label05_09.Text = Resources.str06006;
			label05_11.Text = Resources.str06007;
			label05_13.Text = Resources.str06008;
			label05_15.Text = Resources.str06009;

			label05_17.Text = Resources.str06010;
			label05_18.Text = Resources.str06010;
			label05_19.Text = Resources.str06010;

			radioButton05_03.Text = Resources.str03008;
			radioButton05_04.Text = Resources.str03009;
			checkBox05_02.Text = Resources.str03010;
			label05_20.Text = Resources.str03011;

			label05_21.Text = Resources.str06011;
			label05_23.Text = Resources.str06012;
			checkBox05_01.Text = Resources.str06013;

			groupBox05_03.Text = Resources.str06014;
			label05_25.Text = Resources.str06015;
			label05_27.Text = Resources.str06010;
			label05_28.Text = Resources.str06016;

			label05_01.Text = GlobalVars.unitConverter.DistanceUnit;
			label05_06.Text = GlobalVars.unitConverter.DistanceUnit;
			label05_08.Text = GlobalVars.unitConverter.HeightUnit;
			label05_10.Text = GlobalVars.unitConverter.HeightUnit;
			label05_12.Text = GlobalVars.unitConverter.HeightUnit;
			label05_14.Text = GlobalVars.unitConverter.HeightUnit;
			label05_22.Text = GlobalVars.unitConverter.DistanceUnit;
			label05_24.Text = GlobalVars.unitConverter.HeightUnit;
			label05_26.Text = GlobalVars.unitConverter.DistanceUnit;
			label05_29.Text = GlobalVars.unitConverter.HeightUnit;

			//Page 7 ===============================================
			//groupBox06_01.Text = Resources.str07001;
			//groupBox06_03.Text = Resources.str07002;
			label06_01.Text = Resources.str07002;
			label06_02.Text = Resources.str07003;
			label06_04.Text = Resources.str07004;
			label06_05.Text = Resources.str07005;
			label06_08.Text = Resources.str07006;
			label06_10.Text = Resources.str07007;
			label06_12.Text = Resources.str07008;
			label06_14.Text = Resources.str07009;
			label06_16.Text = Resources.str07010;
			label06_18.Text = Resources.str07011;
			label06_28.Text = label06_20.Text = Resources.str07012;
			label06_21.Text = Resources.str07013;
			label06_22.Text = Resources.str07014;
			radioButton06_03.Text = Resources.str07015;
			radioButton06_04.Text = Resources.str07016;
			label06_23.Text = Resources.str07017;
			label06_24.Text = Resources.str07018;
			label06_26.Text = Resources.str07019;

			label06_06.Text = GlobalVars.unitConverter.SpeedUnit;
			label06_09.Text = GlobalVars.unitConverter.DistanceUnit;
			label06_11.Text = GlobalVars.unitConverter.HeightUnit;
			label06_13.Text = GlobalVars.unitConverter.HeightUnit;
			label06_15.Text = GlobalVars.unitConverter.HeightUnit;
			label06_17.Text = GlobalVars.unitConverter.HeightUnit;
			label06_19.Text = GlobalVars.unitConverter.HeightUnit;
			label06_25.Text = GlobalVars.unitConverter.DistanceUnit;

			comboBox06_01.Items.Clear();
			comboBox06_01.Items.Add(eTurnAt.MApt);
			comboBox06_01.Items.Add(eTurnAt.Altitude);
			comboBox06_01.Items.Add(eTurnAt.TP);

			comboBox06_01.SelectedIndex = 0;
			comboBox06_02.SelectedIndex = 0;
			comboBox06_04.SelectedIndex = 0;

			comboBox06_05.Items.Clear();
			comboBox06_05.Items.Add(eFlyPath.CourseToFIX);
			comboBox06_05.Items.Add(eFlyPath.DirectToFIX);
			comboBox06_05.SelectedIndex = 0;

			comboBox06_07.SelectedIndex = 0;

			//Page 8 ===============================================
			groupBox07_01.Text = Resources.str08001;
			groupBox07_02.Text = Resources.str08002;
			groupBox07_03.Text = Resources.str08003;
			radioButton07_01.Text = Resources.str08004;
			radioButton07_02.Text = Resources.str08005;
			label07_03.Text = Resources.str08006;
			radioButton07_03.Text = Resources.str08007;
			radioButton07_04.Text = Resources.str08008;
			label07_06.Text = Resources.str08009;
			label07_07.Text = Resources.str08010;
			label07_08.Text = Resources.str08011;
			label07_09.Text = Resources.str08012;
			checkBox07_01.Text = Resources.str08013;
			label07_10.Text = Resources.str08014;
			label07_17.Text = Resources.str08016;
			label07_31.Text = Resources.str08015;

			label07_04.Text = GlobalVars.unitConverter.DistanceUnit;
			label07_12.Text = GlobalVars.unitConverter.SpeedUnit;
			label07_18.Text = GlobalVars.unitConverter.HeightUnit;
			label07_20.Text = GlobalVars.unitConverter.HeightUnit;
			label07_22.Text = GlobalVars.unitConverter.SpeedUnit;

			label07_30.Text = GlobalVars.unitConverter.HeightUnit;

			comboBox07_06.Items.Clear();
			comboBox07_06.Items.Add(ePBNClass.RNP_APCH);
			comboBox07_06.Items.Add(ePBNClass.RNAV1);
			comboBox07_06.SelectedIndex = 0;

			comboBox07_09.Items.Clear();
			n = EnrouteMOCValues.Length;

			for (int i = 0; i < n; i++)
				comboBox07_09.Items.Add(GlobalVars.unitConverter.HeightToDisplayUnits(EnrouteMOCValues[i], eRoundMode.SPECIAL_NEAREST).ToString());

			comboBox07_09.SelectedIndex = 0;

			//===============================================
			bFormInitialised = true;
			_showReport = false;

			_reportForm = new ReportFrm();
			_reportForm.Init(ReportBtn);

			System.Drawing.Point pt = new System.Drawing.Point(0, 0);
			System.Drawing.Point pt1 = tabControl1.PointToClient(pt);
			System.Drawing.Point pt2 = tabPage1.PointToClient(pt);

			//System.Drawing.Point pt3 = this.PointToClient(pt);

			int cr = pt1.Y - pt2.Y;         // tabControl1.ItemSize.Height + 3;  //21
			tabControl1.Top = -cr;

			this.Height = this.Height - cr;
			Frame01.Top = Frame01.Top - cr;

			ShowPanelBtn.Checked = false;
			this.Width = Frame02.Left + 10;
		}

		private void ApproachForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			screenCapture.Rollback();
			DBModule.CloseDB();
			_reportForm.Close();

			DeleteAllGraphics();

			if (transitions != null)
				transitions.Clean();

			GlobalVars.gAranGraphics.Refresh();
			GlobalVars.CurrCmd = -1;
		}

		private void ApproachForm_KeyUp(object sender, KeyEventArgs e)
		{
			//if (e.KeyCode == Keys.F1)	ShowHelp(Handle, 3100 + 100 * pageContMain.SelectedIndex);
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

		private void TextBoxes_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				e.IsInputKey = true;
		}

		private void DeleteAllGraphics()
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(VisualAreaElement);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(FFarMAPtHandle);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(FCLHandle);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(FTrackHandle);

			_intermediateLeq.DeleteGraphics();
			_finalLeq.DeleteGraphics();
			_straightMissedLeq.DeleteGraphics();

			_finalSDF1Leg.DeleteGraphics();
			_finalSDF2Leg.DeleteGraphics();

			_IntermSDF1Leg.DeleteGraphics();
			_IntermSDF2Leg.DeleteGraphics();

			_turnLeg.DeleteGraphics();
		}

		private void HelpBtn_Click(object sender, EventArgs e)
		{

		}

		private void PrevBtn_Click(object sender, EventArgs e)
		{
			NativeMethods.ShowPandaBox(this.Handle.ToInt32());
			screenCapture.Delete();
			int dec = 1;
			switch (CurrPage)
			{
				case 1:
					BackToPageI();
					break;
				case 2:
					BackToPageII();
					break;
				case 3:
					BackToPageIII();
					break;
				case 4:
					BackToPageIV();
					break;
				case 5:
					if (!checkBox02_03.Checked)
					{
						BackToPageIV();
						dec = 2;
					}
					else
						BackToPageV();
					break;
				case 6:
					//BackToPageVI();
					_turnLeg.DeleteGraphics();
					_straightTIALeq.DeleteGraphics();
					_straightMissedLeq.RefreshGraphics();
					break;
				case 7:
					transitions.Clean();
					transitions = null;

					while (_reportForm.ObstacleLists > 0)
						_reportForm.RemoveLastLegP4();

					ReCreateMATurnArea();

					break;
				case 8:
					transitions.Remove(true);
					//transitions.ReDraw();
					_reportForm.RemoveLastLegP4();
					break;
			}

			this.CurrPage = CurrPage - dec;
			NativeMethods.HidePandaBox();
		}

		private void NextBtn_Click(object sender, EventArgs e)
		{
			NativeMethods.ShowPandaBox(this.Handle.ToInt32());
			screenCapture.Save(this);
			int inc = 1;
			switch (CurrPage)
			{
				case 0:
					AddvanceToPageII();
					break;
				case 1:
					AddvanceToPageIII();
					break;
				case 2:
					ToFAFSDFs();
					break;
				case 3:
					CloseFAFSDFs();

					if (!checkBox02_03.Checked)
					{
						ToStraightMAPtPage();
						inc = 2;
					}
					else
						ToIFSDFs();
					break;
				case 4:
					ToStraightMAPtPage();
					break;
				case 5:
					ToTurnMissedApproachPage();
					break;
				case 6:
					ToMultiTurnPage();
					break;
				case 7:
					ToPageIX();
					break;
			}

			this.CurrPage = CurrPage + inc;
			NativeMethods.HidePandaBox();
		}

		private void ReportBtn_CheckedChanged(object sender, EventArgs e)
		{
			if (!_showReport)
				return;

			if (ReportBtn.Checked)
				_reportForm.Show(GlobalVars.Win32Window);
			else
				_reportForm.Hide();
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
			pReport.Category = comboBox00_03.Text;

			////pReport.RWY = ComboBox001.Text;
			////pReport.Procedure = _Procedure.Name;
			////pReport.EffectiveDate = _Procedure.TimeSlice.ValidTime.BeginPosition;


			//SaveAccuracy(RepFileName, RepFileTitle, pReport);
			SaveLog(RepFileName, RepFileTitle, pReport);
			SaveProtocol(RepFileName, RepFileTitle, pReport);

			ReportPoint[] GuidPoints;
			double totalLen = ConvertTracToPoints(out GuidPoints);

			ReportFile.SaveGeometry(RepFileName, RepFileTitle, pReport, _refElevation, GuidPoints, totalLen);

			if (SaveProcedure())
				this.Close();
		}

		private void CancelBtn_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void ShowPanelBtn_CheckedChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised)
				return;

			if (ShowPanelBtn.Checked)
			{
				this.Width = Frame02.Left + Frame02.Width + 6;// FontResizeFactorProvider.Scale(3);
				ShowPanelBtn.Image = Resources.HIDE_INFO;
			}
			else
			{
				this.Width = Frame02.Left + 10;
				ShowPanelBtn.Image = Resources.SHOW_INFO;
			}

			if (NextBtn.Enabled)
				NextBtn.Focus();
			else
				PrevBtn.Focus();
		}

		#endregion

		#region Page 1

		void VsManev()
		{
			ObstacleContainer MSAObstacleList;

			double arObsClearanceValues = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arObsClearance].Value[_AirCat];

			_circlingMinOCH = Functions.MaxObstacleHeightInPoly(GlobalVars.ObstacleList, out MSAObstacleList, _convexPoly, out int OIx);
			_circlingMinOCH -= _refElevation;
			_circlingMinOCH += arObsClearanceValues;

			if (_circlingMinOCH < GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMinOCH].Value[_AirCat])
			{
				_circlingMinOCH = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMinOCH].Value[_AirCat];
				OIx = -1;
			}

			_reportForm.SetRefElev(_refElevation);
			_reportForm.FillPage01(MSAObstacleList, arObsClearanceValues, OIx);
			_showReport = true;

			//TextBox0011.Text = CStr(ConvertHeight(FCirclingMinOCH, eRoundMode.NEAREST));
			//TextBox0012.Text = CStr(ConvertHeight(FCirclingMinOCH + GlobalVars.CurrADHP.pPtGeo.Z, eRoundMode.NEAREST));

			//if (OIx >= 0)
			//	TextBox0013.Text = MSAObstacleList[OIx].UnicalName;
			//else
			//	TextBox0013.Text = "";
			//=============================================================
		}

		private void CreateConvexPoly()
		{
			GeometryOperators pTopo = new GeometryOperators();
			MultiPolygon sumPoly = new MultiPolygon();

			for (int i = 0; i < GlobalVars.RWYList.Length; i++)
			{
				if (GlobalVars.RWYList[i].Selected)
				{
					Point ptCentr = GlobalVars.RWYList[i].pPtPrj[eRWY.ptTHR];

					if (sumPoly.IsEmpty)
					{
						Ring tmpRing = ARANFunctions.CreateCirclePrj(ptCentr, R__);
						Polygon tmpPoly = new Polygon { ExteriorRing = tmpRing };
						sumPoly.Add(tmpPoly);
					}
					else
					{
						Ring tmpRing = ARANFunctions.CreateCirclePrj(ptCentr, R__);
						Polygon tmpPoly = new Polygon { ExteriorRing = tmpRing };
						Geometry geom = pTopo.UnionGeometry(sumPoly, tmpPoly);
						sumPoly = (MultiPolygon)geom;
					}
				}
			}

			_convexPoly = (MultiPolygon)pTopo.ConvexHull(sumPoly);

			GlobalVars.gAranGraphics.SafeDeleteGraphic(VisualAreaElement);
			VisualAreaElement = GlobalVars.gAranGraphics.DrawMultiPolygon(_convexPoly, eFillStyle.sfsHollow, ARANFunctions.RGB(0, 0, 255));
		}

		private void listView00_01_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			if (comboBox00_03.SelectedIndex < 0)
				return;
			if (_listView0001InUse)
				return;

			_listView0001InUse = true;

			ListViewItem Item = e.Item;

			try
			{
				int n = 0;
				bool ItemChecked;

				for (int i = 0; i < listView00_01.Items.Count; i++)
				{
					ItemChecked = listView00_01.Items[i].Checked;
					GlobalVars.RWYList[i * 2].Selected = ItemChecked;
					GlobalVars.RWYList[i * 2 + 1].Selected = ItemChecked;

					if (ItemChecked)
						n += 2;
				}

				if (n == 0)         //	msgbox "Выберите ВПП для выполнения данной команды.", vbExclamation'        Return
				{
					Item.Checked = true;
					int i = Item.Index;
					GlobalVars.RWYList[i * 2].Selected = true;
					GlobalVars.RWYList[i * 2 + 1].Selected = true;
				}
				else
					CreateConvexPoly();
			}
			finally
			{
				_listView0001InUse = false;
			}
		}

		private void comboBox00_02_SelectedIndexChanged(object sender, EventArgs e)
		{
			int k = comboBox00_02.SelectedIndex;
			NextBtn.Enabled = k >= 0;

			if (k < 0)
			{
				textBox00_01.Text = "";
				textBox00_02.Text = "";
				textBox00_03.Text = "";
				return;
			}

			_selectedRWY = (RWYType)comboBox00_02.SelectedItem;
			FRWYDirection = _selectedRWY.pPtPrj[eRWY.ptTHR];
			FRWYDirectionGeo = _selectedRWY.pPtGeo[eRWY.ptTHR];
			//fRWYCLDir = AztToDirection(FRWYDirection.Geo, FRWYDirection.TrueBearing, GGeoSR, GPrjSR);

			if (radioButton00_02.Checked || GlobalVars.CurrADHP.Elev > FRWYDirection.Z + 2.0)
			{
				_refElevation = FRWYDirection.Z;
				//"OCH определяется относительно уровня порога ВПП"
				label00_12.Text = Resources.str01011 + " ref. elev = " + GlobalVars.unitConverter.HeightToDisplayUnits(_refElevation).ToString() + " " + GlobalVars.unitConverter.HeightUnit;
			}
			else
			{
				_refElevation = GlobalVars.CurrADHP.Elev;
				//"OCH определяется относительно превышения аэродрома"
				label00_12.Text = Resources.str01010 + " ref. elev = " + GlobalVars.unitConverter.HeightToDisplayUnits(_refElevation).ToString() + " " + GlobalVars.unitConverter.HeightUnit;
			}


			//textBox00_01.Text = _selectedRWY.TrueBearing.ToString("0.00");
			//textBox00_02.Text = _selectedRWY.MagneticBearing.ToString("0.00");

			textBox00_01.Text = GlobalVars.unitConverter.AzimuthToDisplayUnits(_selectedRWY.TrueBearing).ToString();
			textBox00_02.Text = GlobalVars.unitConverter.AzimuthToDisplayUnits(_selectedRWY.MagneticBearing).ToString();
			textBox00_03.Text = GlobalVars.unitConverter.HeightToDisplayUnits(FRWYDirection.Z, eRoundMode.NEAREST).ToString();
		}

		private void comboBox00_03_SelectedIndexChanged(object sender, EventArgs e)
		{
			_AirCat = (aircraftCategory)comboBox00_03.SelectedIndex;

			_visualIAS = GlobalVars.constants.AircraftCategory[aircraftCategoryData.Vva].Value[_AirCat];

			//_visualTAS = ARANMath.IASToTAS(_visualIAS, GlobalVars.CurrADHP.pPtGeo.Z + 300.0, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			_visualTAS = ARANMath.IASToTASForRnav(_visualIAS, GlobalVars.CurrADHP.pPtGeo.Z + 300.0, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			TASwWind = 3.6 * (_visualTAS + GlobalVars.arVisualWS);

			_Bank1 = System.Math.Atan(0.003 * Math.PI * TASwWind / 6.355);

			if (_Bank1 > GlobalVars.constants.Pansops[ePANSOPSData.arVisAverBank].Value)
				_Bank1 = GlobalVars.constants.Pansops[ePANSOPSData.arVisAverBank].Value;

			double Res = ARANMath.BankToRadius(_Bank1, TASwWind / 3.6);
			R__ = 2.0 * Res + GlobalVars.constants.AircraftCategory[aircraftCategoryData.arStraightSegmen].Value[_AirCat];

			//TextBox0002.Text = System.Math.Round(_Bank1).ToString();
			//TextBox0003.Text = System.Math.Round(6355.0 * System.Math.Tan(_Bank1) / (Math.PI * TASwWind), 1).ToString();
			//TextBox0004.Text = GlobalVars.unitConverter.HeightToDisplayUnits(GlobalVars.CurrADHP.pPtGeo.Z).ToString();
			//TextBox0005.Text = GlobalVars.unitConverter.HeightToDisplayUnits(GlobalVars.constants.AircraftCategory[aircraftCategoryData.arObsClearance].Value[_AirCat]).ToString();
			//TextBox0006.Text = GlobalVars.unitConverter.HeightToDisplayUnits(GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMinOCH].Value[_AirCat], eRoundMode.NEAREST).ToString();
			//TextBox0007.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TASwWind * 0.277777777777778, eRoundMode.NEAREST).ToString();
			//TextBox0008.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(Res).ToString();
			//TextBox0009.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(GlobalVars.constants.AircraftCategory[aircraftCategoryData.arStraightSegmen].Value[_AirCat]).ToString();
			//TextBox0010.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(R__).ToString();

			CreateConvexPoly();
		}

		private void radioButton00_01_CheckedChanged(object sender, EventArgs e)
		{
			if (!((RadioButton)sender).Checked)
				return;

			if (radioButton00_02.Checked || GlobalVars.CurrADHP.Elev > FRWYDirection.Z + 2.0)
				_refElevation = FRWYDirection.Z;
			else
				_refElevation = GlobalVars.CurrADHP.Elev;

			if (sender == radioButton00_01)
			{
				textBox00_04.Tag = prmAbvTrshld;
				prmAbvTrshld = GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value;
				textBox00_04.ReadOnly = true;
				_approachType = eApproachType.LNAV;
			}
			else
			{
				double fTmp;
				if (textBox00_04.Tag != null && double.TryParse(textBox00_04.Tag.ToString(), out fTmp))
					prmAbvTrshld = fTmp;
				textBox00_04.ReadOnly = false;
				_approachType = eApproachType.VNAV;
			}

			textBox00_04.Text = GlobalVars.unitConverter.HeightToDisplayUnits(prmAbvTrshld).ToString();
		}

		private void textBox00_04_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox00_04_Validating(textBox00_04, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox00_04.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox00_04_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (double.TryParse(textBox00_04.Text, out fTmp))
			{
				if (textBox00_04.Tag != null && textBox00_04.Tag.ToString() == textBox00_04.Text)
					return;

				prmAbvTrshld = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);
				fTmp = prmAbvTrshld;

				if (prmAbvTrshld < AbvTrshldMinValue)
					prmAbvTrshld = AbvTrshldMinValue;

				if (prmAbvTrshld > AbvTrshldMaxValue)
					prmAbvTrshld = AbvTrshldMaxValue;

				if (prmAbvTrshld != fTmp)
					textBox00_04.Text = GlobalVars.unitConverter.HeightToDisplayUnits(prmAbvTrshld).ToString();
			}
			else if (double.TryParse((string)textBox00_04.Tag, out fTmp))
			{
				prmAbvTrshld = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);
				textBox00_04.Text = (string)textBox00_04.Tag;
			}
			else
			{
				prmAbvTrshld = GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value;
				textBox00_04.Text = GlobalVars.unitConverter.HeightToDisplayUnits(prmAbvTrshld).ToString();
			}
		}

		void AddvanceToPageII()
		{
			FRWYCLDir = FRWYDirection.M;
			//_circlingMinOCH = 300.0;

			VsManev();

			_alignedApproachDir = FRWYCLDir;
			///FAlignedApproachDir
			_notAlignedApproachDir = FRWYCLDir;

			FillFirstAnchorPoints();

			comboBox01_04.Items.Clear();
			foreach (var itm in FCirclingSgfPoint)
				comboBox01_04.Items.Add(itm);

			comboBox01_04.SelectedIndex = 0;

			SetAlignedMode(radioButton01_03.Checked);

			_AbeamValue = 1.0;
			SetAlignedDistance(0);
		}

		void BackToPageI()
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(FFarMAPtHandle);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(FCLHandle);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(FTrackHandle);
			//GlobalVars.gAranGraphics.SafeDeleteGraphic(VisualAreaElement);
		}

		#endregion

		#region Page 2

		void FillFirstAnchorPoints()
		{
			//	Treshold15_30 := Tan(DegToRad(d));
			FAlignedSgfPoint.Clear();

			FNotAlignedSgfPoint.Clear();
			FNotAlignedSgfPointRange.Clear();

			FCirclingSgfPoint.Clear();
			_RWYCollection.Clear();

			//==========================================
			FCirclingSgfPoint.Add(GlobalVars.CurrADHP);

			foreach (var rwy in GlobalVars.RWYList)
				if (rwy.Selected)
				{
					FCirclingSgfPoint.Add(rwy);
					_RWYCollection.Add(rwy.pPtPrj[eRWY.ptTHR]);
				}

			_centroid = _RWYCollection.Centroid;

			GeometryOperators pTopo = new GeometryOperators();
			_RWYsGeometry = pTopo.ConvexHull(_RWYCollection);

			Interval fullInterval = default(Interval);
			fullInterval.Min = GlobalVars.constants.Pansops[ePANSOPSData.arMinInterDist].Value;
			fullInterval.Max = _arMaxInterDist;
			IntervalList tmpIntervalList = new IntervalList(fullInterval);

			double d = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMaxInterAngle].Value[_AirCat];

			Point local_0_0 = new Point(0.0, 0.0);
			Line axis = new Line(new Point(0.0, 0.0), 0.0);
			Line line5 = new Line();
			Line lineD = new Line();
			Vector vector = new Vector();
			Interval tmpInterval = default(Interval);

			WPT_FIXType RWYPoint = new WPT_FIXType();
			RWYPoint.pPtPrj = _selectedRWY.pPtPrj[eRWY.ptTHR];
			RWYPoint.pPtGeo = _selectedRWY.pPtGeo[eRWY.ptTHR];
			RWYPoint.Name = "THR " + _selectedRWY.Name;
			RWYPoint.CallSign = RWYPoint.Name;
			RWYPoint.Identifier = _selectedRWY.Identifier;
			RWYPoint.MagVar = _selectedRWY.TrueBearing - _selectedRWY.MagneticBearing;
			RWYPoint.TypeCode = eNavaidType.NONE;

			FAlignedSgfPoint.Add(RWYPoint);

			foreach (WPT_FIXType sigPoint in GlobalVars.WPTList)
			{
				// Circling ------------------------------------------------------------------------------------------------
				double dist = ARANFunctions.ReturnDistanceInMeters(GlobalVars.CurrADHP.pPtPrj, sigPoint.pPtPrj);

				if (dist < 2.0 * GlobalVars.constants.Pansops[ePANSOPSData.arMaxRangeFAS].Value)
					FCirclingSgfPoint.Add(sigPoint);


				dist = ARANMath.Hypot(sigPoint.pPtPrj.X - FRWYDirection.X, sigPoint.pPtPrj.Y - FRWYDirection.Y);

				if (dist > GlobalVars.maxWptDistance)
					continue;
				// Aligned =================================================================================================

				Point localsigPoint = ARANFunctions.PrjToLocal(FRWYDirection, FRWYCLDir + Math.PI, sigPoint.pPtPrj);

				double Denom = Math.Abs(localsigPoint.X - GlobalVars.constants.Pansops[ePANSOPSData.arMinInterDist].Value);
				double k, Num = Math.Abs(localsigPoint.Y) - GlobalVars.constants.Pansops[ePANSOPSData.arMinInterToler].Value;

				if (Denom > 0.0) k = Math.Abs(Num / Denom);
				else if (Num == 0) k = 0;
				else k = 10.0;

				if (k <= _tan_5)
					FAlignedSgfPoint.Add(sigPoint);

				// Not aligned =============================================================================================
				vector.Direction = GlobalVars.constants.Pansops[ePANSOPSData.arStrInAlignment].Value;
				line5.RefPoint = localsigPoint;
				line5.DirVector = vector;

				vector.Direction = d;
				lineD.RefPoint = localsigPoint;
				lineD.DirVector = vector;

				IntervalList intervalList1 = new IntervalList(fullInterval);

				Geometry leftPoint = ARANFunctions.LineLineIntersect(axis, line5);
				Geometry rightPoint = ARANFunctions.LineLineIntersect(axis, lineD);

				tmpInterval.Min = ((Point)leftPoint).X;
				tmpInterval.Max = ((Point)rightPoint).X;

				if (tmpInterval.Min > tmpInterval.Max)
				{
					k = tmpInterval.Min;
					tmpInterval.Min = tmpInterval.Max;
					tmpInterval.Max = k;
				}
				//tmpIntervalList := TIntervalList.Create(TmpInterval);

				tmpIntervalList[0] = tmpInterval;
				intervalList1.Intersect(tmpIntervalList);
				//=====================================================================================================

				line5.DirVector.Direction = -GlobalVars.constants.Pansops[ePANSOPSData.arStrInAlignment].Value;
				lineD.DirVector.Direction = -d;
				IntervalList intervalList2 = new IntervalList(fullInterval);

				leftPoint = ARANFunctions.LineLineIntersect(axis, line5);
				rightPoint = ARANFunctions.LineLineIntersect(axis, lineD);

				tmpInterval.Min = ((Point)leftPoint).X;
				tmpInterval.Max = ((Point)rightPoint).X;

				if (tmpInterval.Min > tmpInterval.Max)
				{
					k = tmpInterval.Min;
					tmpInterval.Min = tmpInterval.Max;
					tmpInterval.Max = k;
				}
				//			tmpIntervalList := TIntervalList.Create(TmpInterval);
				tmpIntervalList[0] = tmpInterval;
				intervalList2.Intersect(tmpIntervalList);
				//=====================================================================================================
				intervalList1.Union(intervalList2);
				if (intervalList1.Count <= 0)
					continue;

				FNotAlignedSgfPoint.Add(sigPoint);
				FNotAlignedSgfPointRange.Add(intervalList1);
			}
		}

		void SetAlignedMode(bool aligned)
		{
			FAligned = aligned;

			label01_04.Text = "";

			comboBox01_02.Visible = !FAligned;
			comboBox01_01.Items.Clear();

			if (FAligned)
			{
				radioButton01_03.Text = Resources.str02005;
				//label01_01.Text = Resources.str02010;

				label01_05.Visible = false;
				textBox01_02.Visible = false;
				label01_06.Visible = false;

				label01_10.Visible = false;
				textBox01_04.Visible = false;
				label01_11.Visible = false;

				label01_12.Visible = false;
				textBox01_05.Visible = false;
				label01_13.Visible = false;


				double d = ARANMath.RadToDeg(GlobalVars.constants.Pansops[ePANSOPSData.arStrInAlignment].Value);
				//Interval range = new Interval();

				FAlignedTrackInterval.Min = ARANFunctions.AztToDirection(FRWYDirectionGeo, _selectedRWY.TrueBearing + d, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);
				FAlignedTrackInterval.Max = ARANFunctions.AztToDirection(FRWYDirectionGeo, _selectedRWY.TrueBearing - d, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);
				FAlignedTrackInterval.Tag = prmCourseInterval.Tag;

				prmCourseInterval = FAlignedTrackInterval;

				//prmCourse = FAlignedApproachDir;							//??????????????????
				//SetTrackIntervalInDir(range);
				//prmCourseMinValue = range.Min;
				//prmCourseMaxValue = range.Max;

				groupBox01_02.Enabled = FAlignedSgfPoint.Count > 0;

				if (groupBox01_02.Enabled)
				{
					for (int i = 0; i < FAlignedSgfPoint.Count; i++)
						comboBox01_01.Items.Add(FAlignedSgfPoint[i]);

					if (comboBox01_01.Items.Count > 0)
						comboBox01_01.SelectedIndex = 0;
				}

				_approachTrack = 2 * _alignedApproachDir;
				SetNewCourse(_alignedApproachDir);

				FFAOCA = GlobalVars.constants.Pansops[ePANSOPSData.arFASeg_FAF_MOC].Value + _refElevation;
			}
			else
			{
				radioButton01_03.Text = Resources.str02017;
				//label01_01.Text = Resources.str02009;

				label01_05.Visible = true;
				textBox01_02.Visible = true;
				label01_06.Visible = true;

				label01_10.Visible = true;
				textBox01_04.Visible = true;
				label01_11.Visible = true;

				label01_12.Visible = true;
				textBox01_05.Visible = true;
				label01_13.Visible = true;

				//prmCourse = _notAlignedApproachDir;									//??????????????????

				groupBox01_02.Enabled = FNotAlignedSgfPoint.Count > 0;
				//groupBox01_04.Enabled = true;

				if (groupBox01_02.Enabled)
				{
					for (int i = 0; i < FNotAlignedSgfPoint.Count; i++)
						comboBox01_01.Items.Add(FNotAlignedSgfPoint[i]);

					if (comboBox01_01.Items.Count > 0)
						comboBox01_01.SelectedIndex = 0;
				}

				double fTmp = _AlongValue;
				_AlongValue = 0.0;

				SetNotAlignedDistance(fTmp);

				SetNotAlignedTrackInterval(FNotAlignedTrackIntervals[0]);

				SetNewCourse(_notAlignedApproachDir);

				double fTAS = ARANMath.IASToTAS(GlobalVars.constants.AircraftCategory[aircraftCategoryData.VfafMax].Value[_AirCat],
					   GlobalVars.CurrADHP.Elev, /*GlobalVars.CurrADHP.ISAtC -*/ GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value) +
					   GlobalVars.constants.Pansops[ePANSOPSData.arNearTerrWindSp].Value;

				double r = ARANMath.BankToRadius(GlobalVars.constants.Pansops[ePANSOPSData.arBankAngle].Value, fTAS);
				double turnAngle = _approachTrack - FRWYDirection.M;
				if (turnAngle < 0) turnAngle = turnAngle + 2 * Math.PI;
				if (turnAngle > Math.PI) turnAngle = 2 * Math.PI - turnAngle;

				double maxTurnAngle = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMaxInterAngle].Value[_AirCat];
				if (_AirCat <= aircraftCategory.acB && turnAngle < GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMaxInterAngle].Value[acC])
					maxTurnAngle = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMaxInterAngle].Value[acC];

				FFAOCA = _refElevation + prmAbvTrshld +
					(_AlongValue + r * Math.Tan(0.5 * maxTurnAngle) + 5.0 * fTAS) * GlobalVars.constants.Pansops[ePANSOPSData.arFADescent_Nom].Value;
			}

			textBox01_04.Text = GlobalVars.unitConverter.HeightToDisplayUnits(FFAOCA).ToString();
			textBox01_05.Text = GlobalVars.unitConverter.HeightToDisplayUnits(FFAOCA - _refElevation).ToString();
		}

		void SetAlignedAnchorPoint(Point AnchorPoint)
		{
			FAlignedAnchorPoint1 = AnchorPoint;

			double rad5 = GlobalVars.constants.Pansops[ePANSOPSData.arStrInAlignment].Value;
			double d1400 = GlobalVars.constants.Pansops[ePANSOPSData.arMinInterDist].Value;
			double d150 = GlobalVars.constants.Pansops[ePANSOPSData.arMinInterToler].Value;

			Interval interval = new Interval
			{
				Circular = true,
				Min = FRWYCLDir - rad5,
				Max = FRWYCLDir + rad5
			};

			Point ptLeft = ARANFunctions.LocalToPrj(FRWYDirection, FRWYCLDir, -d1400, d150);

			SideDirection side = ARANMath.SideDef(ptLeft, FRWYCLDir + ARANMath.C_PI_2, AnchorPoint);

			if (side == SideDirection.sideOn)
				FAlignedTrackInterval = interval;
			else
			{
				Point ptRight = ARANFunctions.LocalToPrj(FRWYDirection, FRWYCLDir, -d1400, -d150);
				double dirMin, dirMax;

				if (side == SideDirection.sideRight)
				{
					dirMin = ARANFunctions.ReturnAngleInRadians(ptLeft, FAlignedAnchorPoint1);
					dirMax = ARANFunctions.ReturnAngleInRadians(ptRight, FAlignedAnchorPoint1);

				}
				else
				{
					dirMin = ARANFunctions.ReturnAngleInRadians(FAlignedAnchorPoint1, ptRight);
					dirMax = ARANFunctions.ReturnAngleInRadians(FAlignedAnchorPoint1, ptLeft);
				}

				FAlignedTrackInterval.Min = interval.CheckValue(dirMin);
				FAlignedTrackInterval.Max = interval.CheckValue(dirMax);
			}

			FillAlignedDirectionPoints(FAlignedTrackInterval);

			if (comboBox01_03.Items.Count > 0)
				radioButton01_06.Enabled = true;
			else
			{
				radioButton01_05.Checked = true;
				radioButton01_06.Enabled = false;
			}

			FAlignedTrackInterval.Tag = prmCourseInterval.Tag;
			prmCourseInterval = FAlignedTrackInterval;

			double fTmp = FAlignedTrackInterval.CheckValue(_alignedApproachDir);
			if (fTmp != _alignedApproachDir)
				SetNewCourse(fTmp);
			else
			{
				if (FAlignedExistingAnchorPoint)
				{
					Point pt1400 = ARANFunctions.LocalToPrj(FRWYDirection, FRWYCLDir, -GlobalVars.constants.Pansops[ePANSOPSData.arMinInterDist].Value);
					Point ptX = (Point)ARANFunctions.LineLineIntersect(pt1400, FRWYCLDir + ARANMath.C_PI_2, FAlignedAnchorPoint1, _alignedApproachDir);

					ARANFunctions.PrjToLocal(pt1400, FRWYCLDir, ptX, out double x, out double y);

					_AbeamValue = -y;
					textBox01_01.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_AbeamValue).ToString();
					textBox01_01.Tag = textBox01_01.Text;
				}

				DrawCLine();
			}
		}

		void FillAlignedDirectionPoints(Interval TrackIntervalList)
		{
			Guid oldPtId = default(Guid);

			if (comboBox01_03.SelectedIndex >= 0)
				oldPtId = ((WPT_FIXType)(comboBox01_03.SelectedItem)).Identifier;

			comboBox01_03.Items.Clear();

			int oldpt = 0;

			foreach (WPT_FIXType sigPoint in FAlignedSgfPoint)
			{
				Point currPt = sigPoint.pPtPrj;

				double x, y;
				ARANFunctions.PrjToLocal(FAlignedAnchorPoint1, FRWYCLDir, currPt, out x, out y);

				if (Math.Abs(x) <= ARANMath.EpsilonDistance)
					continue;

				double dir = ARANFunctions.ReturnAngleInRadians(FAlignedAnchorPoint1, currPt);

				if (x < 0)
				{
					if (dir < Math.PI) dir += Math.PI;
					else dir -= Math.PI;
				}

				double fTmp = TrackIntervalList.CheckValue(dir);
				if (fTmp == dir)
				{
					if (sigPoint.Identifier == oldPtId)
						oldpt = comboBox01_03.Items.Count;

					comboBox01_03.Items.Add(sigPoint);
				}
			}

			if (comboBox01_03.Items.Count > 0)
				comboBox01_03.SelectedIndex = oldpt;
			else
				label01_09.Text = "";
		}

		void SetNotAlignedAnchorPoint(Point AnchorPoint, int Index)
		{
			Interval interval;
			IntervalList intervalList;

			//FNotAlignedAnchorPoint1.Assign(AnchorPoint);
			FNotAlignedAnchorPoint1 = AnchorPoint;

			comboBox01_02.Items.Clear();
			comboBox01_02.Visible = true;

			prmCourseInterval.Tag = 0;

			Point localAnchor = ARANFunctions.PrjToLocal(FRWYDirection, FRWYCLDir + Math.PI, AnchorPoint);

			if (Math.Abs(localAnchor.Y) < ARANMath.EpsilonDistance) //! FExistingAnchorPoint
			{
				FNotAlignedTrackIntervals[0].Min = NativeMethods.Modulus(Math.Ceiling(_selectedRWY.TrueBearing + ARANMath.RadToDeg(GlobalVars.constants.Pansops[ePANSOPSData.arStrInAlignment].Value)));
				FNotAlignedTrackIntervals[0].Max = NativeMethods.Modulus(Math.Floor(_selectedRWY.TrueBearing + ARANMath.RadToDeg(GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMaxInterAngle].Value[_AirCat])));

				FNotAlignedTrackIntervals[1].Min = NativeMethods.Modulus(Math.Ceiling(_selectedRWY.TrueBearing - ARANMath.RadToDeg(GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMaxInterAngle].Value[_AirCat])));
				FNotAlignedTrackIntervals[1].Max = NativeMethods.Modulus(Math.Floor(_selectedRWY.TrueBearing - ARANMath.RadToDeg(GlobalVars.constants.Pansops[ePANSOPSData.arStrInAlignment].Value)));

				comboBox01_02.Items.Add(Math.Round(FNotAlignedTrackIntervals[0].Min).ToString() + ".." + Math.Round(FNotAlignedTrackIntervals[0].Max).ToString());
				comboBox01_02.Items.Add(Math.Round(FNotAlignedTrackIntervals[1].Min).ToString() + ".." + Math.Round(FNotAlignedTrackIntervals[1].Max).ToString());

				intervalList = new IntervalList(FNotAlignedTrackIntervals[0]);
				intervalList.AddInterval(FNotAlignedTrackIntervals[1]);

				FillNotAlignedDirectionPoints(intervalList);
			}
			else
			{
				if (Index < 0)
					return;

				intervalList = FNotAlignedSgfPointRange[Index];

				for (int i = 0; i < intervalList.Count; i++)
				{
					interval = intervalList[i];
					double d = localAnchor.X - interval.Min;
					if (Math.Abs(d) < ARANMath.EpsilonDistance)
						throw new Exception(string.Format("LocalAnchor.X - Interval.Min({0}) is less than EpsilonDistance({1})", d, ARANMath.EpsilonDistance));

					double dAngL = -ARANMath.RadToDeg(Math.Atan(localAnchor.Y / d));

					d = localAnchor.X - interval.Max;
					if (Math.Abs(d) < ARANMath.EpsilonDistance)
						throw new Exception(string.Format("LocalAnchor.X - Interval.Max({0}) is less than EpsilonDistance({1})", d, ARANMath.EpsilonDistance));

					double dAngR = -ARANMath.RadToDeg(Math.Atan(localAnchor.Y / d));

					if (dAngL > dAngR)
					{
						double fTmp = dAngR;
						dAngR = dAngL;
						dAngL = fTmp;
					}

					FNotAlignedTrackIntervals[i].Min = NativeMethods.Modulus(Math.Ceiling(_selectedRWY.TrueBearing + dAngL));
					FNotAlignedTrackIntervals[i].Max = NativeMethods.Modulus(Math.Floor(_selectedRWY.TrueBearing + dAngR));

					if (FNotAlignedTrackIntervals[i].Min == FNotAlignedTrackIntervals[i].Max)
						comboBox01_02.Items.Add(FNotAlignedTrackIntervals[i].Min);
					else if (NativeMethods.Modulus(FNotAlignedTrackIntervals[i].Max - FNotAlignedTrackIntervals[i].Min) > 90.0)
					{
						//int gr = (int)Math.Floor(Math.Log10(dAngR - dAngL));
						//value = Math.Round(_selectedRWY.TrueBearing + 0.5*(dAngR + dAngL), gr);

						double value = _selectedRWY.TrueBearing + 0.5 * (dAngR + dAngL);

						FNotAlignedTrackIntervals[i].Min = value;
						FNotAlignedTrackIntervals[i].Max = value;

						comboBox01_02.Items.Add(value);
						comboBox01_02.Visible = false;
					}
					else
						comboBox01_02.Items.Add(Math.Round(FNotAlignedTrackIntervals[i].Min).ToString() + ".." +
								Math.Round(FNotAlignedTrackIntervals[i].Max).ToString());
				}

				FillNotAlignedDirectionPoints(intervalList);
			}

			comboBox01_02.SelectedIndex = 0;

			//interval.Min = ARANFunctions.AztToDirection(FRWYDirectionGeo , FNotAlignedTrackIntervals[0].Max, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);
			//interval.Max = ARANFunctions.AztToDirection(FRWYDirectionGeo , FNotAlignedTrackIntervals[0].Min, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);

			SetNotAlignedTrackInterval(FNotAlignedTrackIntervals[0]);

			if (comboBox01_03.Items.Count > 0)
				radioButton01_06.Enabled = true;
			else
			{
				radioButton01_05.Checked = true;
				radioButton01_06.Enabled = false;
			}

			if (prmCourseInterval.Tag == 0)
			{
				_approachTrack = 0.0;
				prmCourseInterval.Tag = 1;
				SetNewCourse(_notAlignedApproachDir);
			}
		}

		void FillNotAlignedDirectionPoints(IntervalList TrackIntervalList)
		{
			//double tresholdL, tresholdR, fTmp, denom, num, k;

			Point localBasePoint, localPoint;
			Line trackLine;
			Geometry tmpGeom;

			bool isSuitable;

			Line axisLine = new Line(FRWYDirection, FRWYCLDir);
			double tresholdL = Math.Tan(GlobalVars.constants.Pansops[ePANSOPSData.arStrInAlignment].Value);
			double tresholdR = Math.Tan(GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMaxInterAngle].Value[_AirCat]);


			bool smallGap = Math.Abs(tresholdL - tresholdR) < ARANMath.EpsilonRadian;

			int oldpt = 0;
			double tresholdMiddle = 0.5 * (tresholdL + tresholdR);

			Guid oldPtId = default(Guid);
			if (comboBox01_03.SelectedIndex >= 0)
				oldPtId = ((WPT_FIXType)(comboBox01_03.SelectedItem)).Identifier;

			comboBox01_03.Items.Clear();
			label01_09.Text = "";

			localBasePoint = ARANFunctions.PrjToLocal(FRWYDirection, FRWYCLDir + Math.PI, FNotAlignedAnchorPoint1);            //LocalPoint = new Point();

			foreach (WPT_FIXType sigPoint in FAlignedSgfPoint)
			{
				double fTmp = ARANMath.Sqr(sigPoint.pPtPrj.X - FNotAlignedAnchorPoint1.X) + ARANMath.Sqr(sigPoint.pPtPrj.Y - FNotAlignedAnchorPoint1.Y);

				if (fTmp <= ARANMath.EpsilonDistance)
					continue;

				localPoint = ARANFunctions.PrjToLocal(FRWYDirection, FRWYCLDir + Math.PI, sigPoint.pPtPrj);

				if (Math.Abs(localBasePoint.Y) < ARANMath.EpsilonDistance)
				{
					double denom = localPoint.X - localBasePoint.X;
					double k, num = localPoint.Y - localBasePoint.Y;

					if (Math.Abs(denom) > 0.0) k = Math.Abs(num / denom);
					else if (num == 0) k = 0;
					else k = 10.0;

					isSuitable = (tresholdL <= k) && (tresholdR >= k);
				}
				else
				{
					int ix = -1;
					trackLine = new Line(sigPoint.pPtPrj, FNotAlignedAnchorPoint1);
					tmpGeom = ARANFunctions.LineLineIntersect(axisLine, trackLine);

					if (tmpGeom.Type == GeometryType.Point)
					{
						localPoint = ARANFunctions.PrjToLocal(FRWYDirection, FRWYCLDir + Math.PI, (Point)tmpGeom);
						ix = TrackIntervalList.Contains(localPoint.X);
					}

					isSuitable = ix >= 0;
				}

				if (isSuitable)
				{
					if (sigPoint.Identifier == oldPtId)
						oldpt = comboBox01_03.Items.Count;

					comboBox01_03.Items.Add(sigPoint);
				}
			}

			if (comboBox01_03.Items.Count > 0)
				comboBox01_03.SelectedIndex = oldpt;

			//return comboBox01_03.Items.Count;
		}

		void SetNotAlignedTrackInterval(Interval trackInterval)
		{
			prmCourseInterval.Min = ARANFunctions.AztToDirection(FRWYDirectionGeo, trackInterval.Max, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);
			prmCourseInterval.Max = ARANFunctions.AztToDirection(FRWYDirectionGeo, trackInterval.Min, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);

			double fTmp = prmCourseInterval.CheckValue(_approachTrack);
			if (_approachTrack != fTmp)
			{
				prmCourseInterval.Tag = 1;
				SetNewCourse(fTmp);
			}
		}

		void FillCirclingDirectionPoints(Interval chkInter)
		{
			comboBox01_06.Items.Clear();
			chkInter.Circular = true;

			foreach (var dspt in comboBox01_04.Items)
			{
				if (dspt == comboBox01_04.SelectedItem)
					continue;

				Point vsAnchPt2;
				if (dspt is ADHPType)
					vsAnchPt2 = ((ADHPType)dspt).pPtPrj;
				else if (dspt is RWYType)
					vsAnchPt2 = ((RWYType)dspt).pPtPrj[eRWY.ptTHR];
				else
					vsAnchPt2 = ((WPT_FIXType)dspt).pPtPrj;

				double dist = ARANFunctions.ReturnDistanceInMeters(vsAnchPt1, vsAnchPt2);
				if (dist == 0)
					continue;

				if (isOmni)
				{
					comboBox01_06.Items.Add(dspt);
					continue;
				}

				double dir = ARANFunctions.ReturnAngleInRadians(vsAnchPt1, vsAnchPt2);

				if (chkInter.InsideInterval(dir))
					comboBox01_06.Items.Add(dspt);
			}

			if (comboBox01_06.Items.Count > 0)
			{
				radioButton01_08.Enabled = true;
				comboBox01_06.SelectedIndex = 0;
			}
			else
			{
				radioButton01_08.Enabled = false;
				radioButton01_07.Checked = true;
			}

			if (radioButton01_07.Checked)
			{
				GeometryOperators geoOper = new GeometryOperators();
				Point ptTmp = (Point)geoOper.GeoTransformations((Geometry)vsAnchPt1, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

				double dir = ARANFunctions.AztToDirection(ptTmp, (double)numericUpDown01_01.Value, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);
				dir = chkInter.CheckValue(dir);
				numericUpDown01_01.Value = (decimal)GlobalVars.unitConverter.AzimuthToDisplayUnits(ARANFunctions.DirToAzimuth(vsAnchPt1, dir, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo));
			}
		}


		private bool CreateMAPt(double fTrackDir, Point AnchorPoint)
		{
			if (AnchorPoint == null)
				return false;

			Line line1 = new Line(AnchorPoint, fTrackDir);
			Geometry geometry;
			if (_landingType == eLandingType.Circkling)
			{
				foreach (var rwy in GlobalVars.RWYList)
					if (rwy.Selected)
					{
						double clDir = ARANFunctions.AztToDirection(FRWYDirectionGeo, rwy.TrueBearing, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);
						geometry = ARANFunctions.LineLineIntersect(AnchorPoint, fTrackDir, rwy.pPtPrj[eRWY.ptTHR], clDir, out bool interRes);

						//rwy.pPtPrj[eRWY.ptTHR]
						//FCirclingSgfPoint.Add(rwy);
						//_RWYCollection.Add(rwy.pPtPrj[eRWY.ptTHR]);
					}


				return true;
			}

			Line line2;
			FFarMAPt = null;

			if (FAligned)
				line2 = new Line(FRWYDirection, fTrackDir + 0.5 * Math.PI);
			else
				line2 = new Line(FRWYDirection, FRWYCLDir);

			geometry = ARANFunctions.LineLineIntersect(line1, line2);

			bool result = geometry.Type == GeometryType.Point;
			if (result)
				FFarMAPt = (Point)geometry;

			return result;
		}

		private void DrawCLine()
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(FFarMAPtHandle);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(FCLHandle);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(FTrackHandle);

			double trackDir;
			Point anchorPoint;

			if (tabControl01_01.SelectedIndex == 1)
			{
				trackDir = selectedDir;
				anchorPoint = vsAnchPt1;
			}
			else if (FAligned)
			{
				trackDir = _alignedApproachDir;
				anchorPoint = FAlignedAnchorPoint1;
			}
			else
			{
				trackDir = _notAlignedApproachDir;
				anchorPoint = FNotAlignedAnchorPoint1;
			}

			if (CreateMAPt(trackDir, anchorPoint))
				FFarMAPtHandle = GlobalVars.gAranGraphics.DrawPoint(FFarMAPt, ePointStyle.smsSquare, 255);

			//==============================================================
			Point farFAP = ARANFunctions.LocalToPrj(FRWYDirection, FRWYCLDir + Math.PI, GlobalVars.constants.Pansops[ePANSOPSData.arFAPMaxRange].Value, 0.0);
			LineString pCLPolyline = new LineString();

			pCLPolyline.Add(FRWYDirection);
			pCLPolyline.Add(farFAP);

			FCLHandle = GlobalVars.gAranGraphics.DrawLineString(pCLPolyline, 1, 255);

			//==============================================================
#if WhileDebug
			if (FFarMAPt != null)
			{
#endif
				Point tmpPt = ARANFunctions.LocalToPrj(FFarMAPt, trackDir + Math.PI, GlobalVars.constants.Pansops[ePANSOPSData.arFAPMaxRange].Value, 0.0);
				LineString pTrackPolyline = new LineString();

				pTrackPolyline.Add(FFarMAPt);
				pTrackPolyline.Add(tmpPt);

				FTrackHandle = GlobalVars.gAranGraphics.DrawLineString(pTrackPolyline, 1, 0);
#if WhileDebug
			}
#endif
		}

		private void SetAlignedDistance(double newValue)
		{
			if (Math.Abs(newValue - _AbeamValue) < ARANMath.Epsilon_2Distance)
				return;

			if (newValue < prmAbeamMinValue)
				newValue = prmAbeamMinValue;
			if (newValue > prmAbeamMaxValue)
				newValue = prmAbeamMaxValue;

			_AbeamValue = newValue;
			textBox01_01.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_AbeamValue).ToString();
			textBox01_01.Tag = textBox01_01.Text;

			FFAOCA = GlobalVars.constants.Pansops[ePANSOPSData.arFASeg_FAF_MOC].Value + _refElevation;

			Point tmpPoint = ARANFunctions.LocalToPrj(FRWYDirection, FRWYCLDir + Math.PI, GlobalVars.constants.Pansops[ePANSOPSData.arMinInterDist].Value, _AbeamValue);
			SetAlignedAnchorPoint(tmpPoint);

			textBox01_04.Text = GlobalVars.unitConverter.HeightToDisplayUnits(FFAOCA).ToString();
			textBox01_05.Text = GlobalVars.unitConverter.HeightToDisplayUnits(FFAOCA - _refElevation).ToString();
		}

		private void SetNotAlignedDistance(double newValue)
		{
			if (Math.Abs(newValue - _AlongValue) < ARANMath.EpsilonDistance)
				return;

			if (newValue < prmAlongMinValue) newValue = prmAlongMinValue;
			if (newValue > prmAlongMaxValue) newValue = prmAlongMaxValue;

			_AlongValue = newValue;
			textBox01_01.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_AlongValue).ToString();
			textBox01_01.Tag = textBox01_01.Text;

			Point tmpPoint = ARANFunctions.LocalToPrj(FRWYDirection, FRWYCLDir + Math.PI, _AlongValue, 0.0);
			SetNotAlignedAnchorPoint(tmpPoint, -1);

			//if (_notAlignedApproachDir == prmCourse)		DrawCLine();

			double fTAS = ARANMath.IASToTAS(GlobalVars.constants.AircraftCategory[aircraftCategoryData.VfafMax].Value[_AirCat],
				   GlobalVars.CurrADHP.Elev, /*GlobalVars.CurrADHP.ISAtC -*/ GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value) +
					   GlobalVars.constants.Pansops[ePANSOPSData.arNearTerrWindSp].Value;

			double R = ARANMath.BankToRadius(GlobalVars.constants.Pansops[ePANSOPSData.arBankAngle].Value, fTAS);
			double TurnAngle = _approachTrack - FRWYDirection.M;

			if (TurnAngle < 0)
				TurnAngle = TurnAngle + 2 * Math.PI;

			if (TurnAngle > Math.PI)
				TurnAngle = 2 * Math.PI - TurnAngle;

			double MaxTurnAngle = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMaxInterAngle].Value[_AirCat];

			if (_AirCat <= aircraftCategory.acB && TurnAngle < GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMaxInterAngle].Value[acC])
				MaxTurnAngle = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMaxInterAngle].Value[acC];

			FFAOCA = _refElevation + prmAbvTrshld +
					(_AlongValue + R * Math.Tan(0.5 * MaxTurnAngle) + 5.0 * fTAS) * GlobalVars.constants.Pansops[ePANSOPSData.arFADescent_Nom].Value;

			textBox01_04.Text = GlobalVars.unitConverter.HeightToDisplayUnits(FFAOCA).ToString();
			textBox01_05.Text = GlobalVars.unitConverter.HeightToDisplayUnits(FFAOCA - _refElevation).ToString();
		}

		private void SetNewCourse(double newCourse)
		{
			if (newCourse == _approachTrack)
				return;

			_approachTrack = prmCourseInterval.CheckValue(newCourse);

			double azCourrse = ARANFunctions.DirToAzimuth(FRWYDirection, _approachTrack, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			textBox01_03.Text = GlobalVars.unitConverter.AzimuthToDisplayUnits(azCourrse).ToString();
			textBox01_03.Tag = textBox01_03.Text;

			if (FAligned)
			{
				_alignedApproachDir = _approachTrack;

				if (FAlignedExistingAnchorPoint)
				{
					Point pt1400 = ARANFunctions.LocalToPrj(FRWYDirection, FRWYCLDir, -GlobalVars.constants.Pansops[ePANSOPSData.arMinInterDist].Value);
					Point ptX = (Point)ARANFunctions.LineLineIntersect(pt1400, FRWYCLDir + ARANMath.C_PI_2, FAlignedAnchorPoint1, _alignedApproachDir);

					//GlobalVars.gAranGraphics.DrawPointWithText(pt1400, "pt1400");
					//GlobalVars.gAranGraphics.DrawPointWithText(ptX, "ptX");
					//LegBase.ProcessMessages();

					ARANFunctions.PrjToLocal(pt1400, FRWYCLDir, ptX, out double x, out double y);

					_AbeamValue = -y;
					textBox01_01.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_AbeamValue).ToString();
					textBox01_01.Tag = textBox01_01.Text;

					FFAOCA = GlobalVars.constants.Pansops[ePANSOPSData.arFASeg_FAF_MOC].Value + _refElevation;
				}
			}
			else
			{
				_notAlignedApproachDir = _approachTrack;
				if (FNotAlignedExistingAnchorPoint)
				{
					Point pt00 = new Point(0, 0);
					Point LocalSigPt = ARANFunctions.PrjToLocal(FRWYDirection, FRWYCLDir + Math.PI, FNotAlignedAnchorPoint1);
					Geometry ptInter = ARANFunctions.LineLineIntersect(pt00, 0, LocalSigPt, _notAlignedApproachDir - FRWYCLDir);

					double newAlong = 0.0;
					if (ptInter.Type == GeometryType.Point)
						newAlong = ((Point)ptInter).X;

					SetNotAlignedDistance(newAlong);
				}

				double fTAS = ARANMath.IASToTAS(GlobalVars.constants.AircraftCategory[aircraftCategoryData.VfafMax].Value[_AirCat],
								 GlobalVars.CurrADHP.Elev, /*GlobalVars.CurrADHP.ISAtC -*/ GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value) +
										   GlobalVars.constants.Pansops[ePANSOPSData.arNearTerrWindSp].Value;

				double r = ARANMath.BankToRadius(GlobalVars.constants.Pansops[ePANSOPSData.arBankAngle].Value, fTAS);
				double TurnAngle = _approachTrack - FRWYDirection.M;
				if (TurnAngle < 0) TurnAngle = TurnAngle + 2 * Math.PI;
				if (TurnAngle > Math.PI) TurnAngle = 2 * Math.PI - TurnAngle;

				double maxTurnAngle = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMaxInterAngle].Value[_AirCat];
				if (_AirCat <= aircraftCategory.acB && TurnAngle < GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMaxInterAngle].Value[acC])
					maxTurnAngle = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMaxInterAngle].Value[acC];

				FFAOCA = _refElevation + prmAbvTrshld +
							(_AlongValue + r * Math.Tan(0.5 * maxTurnAngle) + 5.0 * fTAS) * GlobalVars.constants.Pansops[ePANSOPSData.arFADescent_Nom].Value;
			}

			textBox01_04.Text = GlobalVars.unitConverter.HeightToDisplayUnits(FFAOCA).ToString();
			textBox01_05.Text = GlobalVars.unitConverter.HeightToDisplayUnits(FFAOCA - _refElevation).ToString();

			DrawCLine();
		}

		private void SetNewAlongMaxValue(double newAlongMax)
		{
			if (newAlongMax == prmAlongMaxValue)
				return;

			double fTmp = newAlongMax;

			if (newAlongMax < _arMaxInterDist) newAlongMax = _arMaxInterDist;
			if (newAlongMax > _arMaxMaxInterDist) newAlongMax = _arMaxMaxInterDist;

			prmAlongMaxValue = newAlongMax;

			textBox01_02.Text = GlobalVars.unitConverter.HeightToDisplayUnits(prmAlongMaxValue).ToString();
			textBox01_02.Tag = textBox01_02.Text;

			if (_AlongValue > prmAlongMaxValue)
				SetNotAlignedDistance(prmAlongMaxValue);
		}

		/*
		//////Events
		*/

		private void tabControl01_01_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabControl01_01.SelectedIndex == 0)
			{
				_landingType = eLandingType.StraightIn;
				if (radioButton01_01.Checked)
					radioButton01_01_CheckedChanged(radioButton01_01, null);
				else
					radioButton01_01_CheckedChanged(radioButton01_02, null);
			}
			else
			{
				_landingType = eLandingType.Circkling;
				numericUpDown01_01_ValueChanged(numericUpDown01_01, null);
			}
		}

		private void radioButton01_01_CheckedChanged(object sender, EventArgs e)
		{
			if (!((RadioButton)sender).Checked)
				return;

			SetAlignedMode(sender == radioButton01_01);
		}

		private void radioButton01_03_CheckedChanged(object sender, EventArgs e)
		{
			if (!((RadioButton)sender).Checked)
				return;

			bool Existing = sender == radioButton01_04;

			if (FAligned)
				FAlignedExistingAnchorPoint = Existing;
			else
				FNotAlignedExistingAnchorPoint = Existing;

			comboBox01_01.Enabled = Existing;
			textBox01_01.ReadOnly = Existing;

			if (Existing && comboBox01_01.Items.Count > 0)
			{
				if (comboBox01_01.SelectedIndex < 0)
					comboBox01_01.SelectedIndex = 0;
				else
					comboBox01_01_SelectedIndexChanged(comboBox01_01, null);
			}
			else
				SetAlignedMode(FAligned);
		}

		private void radioButton01_05_CheckedChanged(object sender, EventArgs e)
		{
			if (!((RadioButton)sender).Checked)
				return;

			bool Existing = sender == radioButton01_06;

			comboBox01_03.Enabled = Existing;

			textBox01_03.ReadOnly = Existing;
			comboBox01_02.Enabled = !Existing;

			if (!Existing)
			{
				if (FAligned)
				{
					FAlignedTrackInterval.Tag = prmCourseInterval.Tag;
					prmCourseInterval = FAlignedTrackInterval;

					double fTmp = prmCourseInterval.CheckValue(_approachTrack);
					if (_approachTrack != fTmp)
						SetNewCourse(fTmp);
				}
				else
				{
					if (radioButton01_03.Checked)
						SetNotAlignedDistance(_AlongValue);
					else
						SetNotAlignedAnchorPoint(FNotAlignedAnchorPoint1, -1);
				}
			}
			else //if (comboBox01_03.Items.Count > 0)
			{
				if (comboBox01_03.SelectedIndex < 0)
					comboBox01_03.SelectedIndex = 0;
				else
					comboBox01_03_SelectedIndexChanged(comboBox01_03, null);
			}
		}

		private void comboBox01_01_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox01_01.SelectedIndex < 0)
				return;

			WPT_FIXType sigPoint = (WPT_FIXType)(comboBox01_01.SelectedItem);
			label01_04.Text = sigPoint.TypeCode.ToString();

			if (!radioButton01_04.Checked)
				return;

			if (FAligned)
				SetAlignedAnchorPoint(sigPoint.pPtPrj);
			else
				SetNotAlignedAnchorPoint(sigPoint.pPtPrj, comboBox01_01.SelectedIndex);
		}

		private void comboBox01_02_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox01_02.SelectedIndex < 0)
				return;

			SetNotAlignedTrackInterval(FNotAlignedTrackIntervals[comboBox01_02.SelectedIndex]);
		}

		private void comboBox01_03_SelectedIndexChanged(object sender, EventArgs e)
		{
			int k = comboBox01_03.SelectedIndex;
			if (k < 0)
				return;

			WPT_FIXType sigPoint2 = (WPT_FIXType)comboBox01_03.SelectedItem;

			label01_09.Text = sigPoint2.TypeCode.ToString();
			if (!radioButton01_06.Checked)
				return;

			double fTrackDir;
			if (FAligned)
				fTrackDir = ARANFunctions.ReturnAngleInRadians(FAlignedAnchorPoint1, sigPoint2.pPtPrj);
			else
				fTrackDir = ARANFunctions.ReturnAngleInRadians(FNotAlignedAnchorPoint1, sigPoint2.pPtPrj);

			if (NativeMethods.Modulus(Math.Abs(fTrackDir - FRWYCLDir), 2.0 * Math.PI) > 0.5 * Math.PI)
				fTrackDir = NativeMethods.Modulus(fTrackDir + Math.PI, 2.0 * Math.PI);

			SetNewCourse(fTrackDir);
		}

		private void comboBox01_04_SelectedIndexChanged(object sender, EventArgs e)
		{
			int k = comboBox01_04.SelectedIndex;
			if (k < 0)
				return;

			vsSelected1 = comboBox01_04.SelectedItem;

			if (k == 0)
			{
				label01_15.Text = Resources.str00010;
				vsAnchPt1 = ((ADHPType)vsSelected1).pPtPrj;
			}
			else if (vsSelected1 is RWYType)
			{
				label01_15.Text = Resources.str00011;
				vsAnchPt1 = ((RWYType)vsSelected1).pPtPrj[eRWY.ptTHR];
			}
			else
			{
				vsAnchPt1 = ((WPT_FIXType)vsSelected1).pPtPrj;
				label01_15.Text = ((WPT_FIXType)vsSelected1).TypeCode.ToString();// Resources.str00011;
			}

			double dist = ARANFunctions.ReturnDistanceInMeters(vsAnchPt1, GlobalVars.CurrADHP.pPtPrj);

			textBox01_06.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(dist).ToString();
			haveIdeal = dist > 0;

			GeometryOperators pTopo = new GeometryOperators();
			isOmni = pTopo.GetDistance(_RWYsGeometry, vsAnchPt1) <= GlobalVars.constants.Pansops[ePANSOPSData.arCirclAprShift].Value;

			dataGridView01_01.RowCount = 0;
			DataGridViewRow row = new DataGridViewRow();
			DataGridViewTextBoxCell cell;

			comboBox01_05.Items.Clear();
			idealDir = 0.0;

			if (haveIdeal)
			{
				cell = new DataGridViewTextBoxCell();

				idealDir = ARANFunctions.ReturnAngleInRadians(vsAnchPt1, _centroid);
				cell.Value = Resources.str02029;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = GlobalVars.unitConverter.AzimuthToDisplayUnits(ARANFunctions.DirToAzimuth(vsAnchPt1, idealDir, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo));
				row.Cells.Add(cell);
				dataGridView01_01.Rows.Add(row);
				row = new DataGridViewRow();

				cell = new DataGridViewTextBoxCell();
				cell.Value = Resources.str02029;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = GlobalVars.unitConverter.AzimuthToDisplayUnits(ARANFunctions.DirToAzimuth(vsAnchPt1, idealDir + Math.PI, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo));
				row.Cells.Add(cell);
				dataGridView01_01.Rows.Add(row);
				row = new DataGridViewRow();
			}

			if (isOmni)
			{
				cell = new DataGridViewTextBoxCell();
				cell.Value = Resources.str02030;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = "0";
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = "360";
				row.Cells.Add(cell);

				visualInterval1.Max = 2 * Math.PI;
				visualInterval1.Min = 0;

				visualInterval2.Max = visualInterval1.Max;
				visualInterval2.Min = visualInterval1.Min;

				comboBox01_05.Items.Add("0..360");
				//comboBox01_05.Enabled = false;
			}
			else
			{
				minDesAngle = double.MaxValue;
				maxDesAngle = double.MinValue;
				//double maxMaxLimit, minMaxLimit;

				Point minDesPt = null, maxDesPt = null;

				foreach (Point pt in _RWYCollection)
				{
					ARANFunctions.PrjToLocal(vsAnchPt1, idealDir, pt, out double x, out double y);
					double tanAlpha = y / x;

					if (tanAlpha > maxDesAngle)
					{
						maxDesAngle = tanAlpha;
						maxDesPt = pt;
					}

					if (tanAlpha < minDesAngle)
					{
						minDesAngle = tanAlpha;
						minDesPt = pt;
					}
				}

				minDesAngle = idealDir + Math.Atan(minDesAngle);
				maxDesAngle = idealDir + Math.Atan(maxDesAngle);

				cell = new DataGridViewTextBoxCell();
				cell.Value = Resources.str02031;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = GlobalVars.unitConverter.AzimuthToDisplayUnits(ARANFunctions.DirToAzimuth(vsAnchPt1, maxDesAngle, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo));
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = GlobalVars.unitConverter.AzimuthToDisplayUnits(ARANFunctions.DirToAzimuth(vsAnchPt1, minDesAngle, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo));
				row.Cells.Add(cell);

				dataGridView01_01.Rows.Add(row);
				row = new DataGridViewRow();

				cell = new DataGridViewTextBoxCell();
				cell.Value = Resources.str02031;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = GlobalVars.unitConverter.AzimuthToDisplayUnits(ARANFunctions.DirToAzimuth(vsAnchPt1, maxDesAngle + Math.PI, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo));
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = GlobalVars.unitConverter.AzimuthToDisplayUnits(ARANFunctions.DirToAzimuth(vsAnchPt1, minDesAngle + Math.PI, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo));
				row.Cells.Add(cell);

				dataGridView01_01.Rows.Add(row);
				row = new DataGridViewRow();

				//==================================================
				double sinMax = GlobalVars.constants.Pansops[ePANSOPSData.arCirclAprShift].Value / ARANFunctions.ReturnDistanceInMeters(vsAnchPt1, maxDesPt);
				double sinMin = GlobalVars.constants.Pansops[ePANSOPSData.arCirclAprShift].Value / ARANFunctions.ReturnDistanceInMeters(vsAnchPt1, minDesPt);

				visualInterval1.Min = minDesAngle - Math.Asin(sinMin);
				visualInterval1.Max = maxDesAngle + Math.Asin(sinMax);

				visualInterval2.Min = visualInterval1.Min + Math.PI;
				visualInterval2.Max = visualInterval1.Max + Math.PI;


				AztInterval0.Min = GlobalVars.unitConverter.AzimuthToDisplayUnits(ARANFunctions.DirToAzimuth(vsAnchPt1, visualInterval1.Max, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo));
				AztInterval0.Max = GlobalVars.unitConverter.AzimuthToDisplayUnits(ARANFunctions.DirToAzimuth(vsAnchPt1, visualInterval1.Min, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo));

				cell = new DataGridViewTextBoxCell();
				cell.Value = Resources.str02032;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = AztInterval0.Min;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = AztInterval0.Max;
				row.Cells.Add(cell);
				comboBox01_05.Items.Add(AztInterval0.Min.ToString() + " - " + AztInterval0.Max.ToString());

				dataGridView01_01.Rows.Add(row);
				row = new DataGridViewRow();

				AztInterval1.Min = GlobalVars.unitConverter.AzimuthToDisplayUnits(ARANFunctions.DirToAzimuth(vsAnchPt1, visualInterval2.Max, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo));
				AztInterval1.Max = GlobalVars.unitConverter.AzimuthToDisplayUnits(ARANFunctions.DirToAzimuth(vsAnchPt1, visualInterval2.Min, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo));

				cell = new DataGridViewTextBoxCell();
				cell.Value = Resources.str02032;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = AztInterval1.Min;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = AztInterval1.Max;
				row.Cells.Add(cell);

				comboBox01_05.Items.Add(AztInterval1.Min.ToString() + " - " + AztInterval1.Max.ToString());
			}

			dataGridView01_01.Rows.Add(row);
			comboBox01_05.SelectedIndex = 0;
		}

		private void comboBox01_05_SelectedIndexChanged(object sender, EventArgs e)
		{
			Interval chkInter = visualInterval1;
			if (comboBox01_05.SelectedIndex == 1)
				chkInter = visualInterval2;

			FillCirclingDirectionPoints(chkInter);
		}

		private void comboBox01_06_SelectedIndexChanged(object sender, EventArgs e)
		{
			int k = comboBox01_06.SelectedIndex;
			if (k < 0)
				return;

			Point vsAnchPt2;
			object vsSelected2 = comboBox01_06.SelectedItem;

			if (vsSelected2 is ADHPType)
			{
				label01_20.Text = Resources.str00010;
				vsAnchPt2 = ((ADHPType)vsSelected2).pPtPrj;
			}
			else if (vsSelected2 is RWYType)
			{
				label01_20.Text = Resources.str00011;
				vsAnchPt2 = ((RWYType)vsSelected2).pPtPrj[eRWY.ptTHR];
			}
			else
			{
				label01_20.Text = Resources.str00012;
				vsAnchPt2 = ((WPT_FIXType)vsSelected2).pPtPrj;
			}

			if (radioButton01_07.Checked)
				return;

			double dir = ARANFunctions.ReturnAngleInRadians(vsAnchPt1, vsAnchPt2);

			numericUpDown01_01.Value = (decimal)GlobalVars.unitConverter.AzimuthToDisplayUnits(ARANFunctions.DirToAzimuth(vsAnchPt1, dir, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo));
		}

		private void radioButton01_07_CheckedChanged(object sender, EventArgs e)
		{
			RadioButton snd = (RadioButton)sender;
			if (!snd.Checked)
				return;

			if (snd == radioButton01_07)
			{
				numericUpDown01_01.Enabled = true;
				comboBox01_06.Enabled = false;
			}
			else
			{
				numericUpDown01_01.Enabled = false;
				comboBox01_06.Enabled = true;
				comboBox01_06_SelectedIndexChanged(comboBox01_06, null);
			}
		}

		private void numericUpDown01_01_ValueChanged(object sender, EventArgs e)
		{
			const double tolerance = 0.5 * ARANMath.DegToRadValue;

			if (numericUpDown01_01.Value >= 360)
			{
				numericUpDown01_01.Value -= 360;
				return;
			}

			if (numericUpDown01_01.Value < 0)
			{
				numericUpDown01_01.Value += 360;
				return;
			}

			Interval chkInter = visualInterval1;
			if (comboBox01_05.SelectedIndex == 1)
				chkInter = visualInterval2;

			GeometryOperators geoOper = new GeometryOperators();
			Point ptTmp = (Point)geoOper.GeoTransformations((Geometry)vsAnchPt1, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			double dir1 = ARANFunctions.AztToDirection(ptTmp, (double)numericUpDown01_01.Value, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);
			double dir = chkInter.CheckValue(dir1);

			if (dir != dir1)
			{
				decimal OldValue = numericUpDown01_01.Value;
				numericUpDown01_01.Value = (decimal)GlobalVars.unitConverter.AzimuthToDisplayUnits(ARANFunctions.DirToAzimuth(vsAnchPt1, dir, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo));
				if (OldValue != numericUpDown01_01.Value)
					return;
			}

			Interval chkIdeal1 = default(Interval), chkIdeal2 = default(Interval);
			Interval chkDesir1 = default(Interval), chkDesir2 = default(Interval);
			chkIdeal2.Circular = chkIdeal1.Circular = true;

			chkIdeal1.Min = idealDir - tolerance;
			chkIdeal1.Max = idealDir + tolerance;

			chkIdeal2.Min = chkIdeal1.Min + Math.PI;
			chkIdeal2.Max = chkIdeal1.Max + Math.PI;

			chkDesir1.Min = minDesAngle;
			chkDesir1.Max = maxDesAngle;
			chkDesir2.Min = minDesAngle + Math.PI;
			chkDesir2.Max = maxDesAngle + Math.PI;

			if (haveIdeal && (chkIdeal1.InsideInterval(dir) || chkIdeal2.InsideInterval(dir)))
				label01_21.Text = "Ideal";
			else if (isOmni)
				label01_21.Text = "";
			else if (chkDesir1.InsideInterval(dir) || chkDesir2.InsideInterval(dir))
				label01_21.Text = "In desirable limit";
			else
				label01_21.Text = "In maximal limit";

			selectedDir = dir;

			DrawCLine();
		}

		private void textBox01_01_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox01_01_Validating(textBox01_01, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxSignedFloat(ref eventChar, textBox01_01.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox01_01_Validating(object sender, CancelEventArgs e)
		{
			if (textBox01_01.ReadOnly)
				return;

			double fTmp;
			if (double.TryParse(textBox01_01.Text, out fTmp))
			{
				if (textBox01_01.Tag != null && textBox01_01.Tag.ToString() == textBox01_01.Text)
					return;

				fTmp = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);

				if (FAligned)
					SetAlignedDistance(fTmp);                //	SetNewAbeamValue(fTmp);
				else
					SetNotAlignedDistance(fTmp);                //	SetNewAlongValue(fTmp);
			}
			else if (textBox01_01.Tag != null && double.TryParse(textBox01_01.Tag.ToString(), out fTmp))
				textBox01_01.Text = textBox01_01.Tag.ToString();
			else if (FAligned)
				textBox01_01.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_AbeamValue).ToString();
			else
				textBox01_01.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_AlongValue).ToString();
		}

		private void textBox01_02_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox01_02_Validating(textBox01_02, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox01_02.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox01_02_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (double.TryParse(textBox01_02.Text, out fTmp))
			{
				if (textBox01_02.Tag != null && textBox01_02.Tag.ToString() == textBox01_02.Text)
					return;

				SetNewAlongMaxValue(GlobalVars.unitConverter.HeightToInternalUnits(fTmp));
			}
			else if (textBox01_02.Tag != null && double.TryParse(textBox01_02.Tag.ToString(), out fTmp))
				textBox01_02.Text = (string)textBox01_02.Tag;
			else
				textBox01_02.Text = GlobalVars.unitConverter.HeightToDisplayUnits(prmAlongMaxValue).ToString();
		}

		private void textBox01_03_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox01_03_Validating(textBox01_03, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox01_03.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox01_03_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (double.TryParse(textBox01_03.Text, out fTmp))
			{
				if (textBox01_03.Tag != null && textBox01_03.Tag.ToString() == textBox01_03.Text)
					return;

				SetNewCourse(ARANFunctions.AztToDirection(FRWYDirectionGeo, fTmp, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj));
			}
			else if (textBox01_03.Tag != null && double.TryParse(textBox01_03.Tag.ToString(), out fTmp))
				textBox01_03.Text = (string)textBox01_03.Tag;
			else
			{
				double azCourrse = ARANFunctions.DirToAzimuth(FRWYDirection, _approachTrack, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				textBox01_03.Text = GlobalVars.unitConverter.AzimuthToDisplayUnits(azCourrse).ToString();
			}
		}

		void AddvanceToPageIII()
		{
			_prevImArea = 0.0;

			F3ApproachDir = _approachTrack;
			double azCourrse = ARANFunctions.DirToAzimuth(FRWYDirection, F3ApproachDir, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			textBox05_02.Text = GlobalVars.unitConverter.AzimuthToDisplayUnits(azCourrse).ToString();

			//============================================================================================================
			FAFDistanceInterval.Min = GlobalVars.constants.Pansops[ePANSOPSData.arMinRangeFAS].Value;
			FAFDistanceInterval.Max = GlobalVars.constants.Pansops[ePANSOPSData.arMaxRangeFAS].Value;

			FAFIASInterval.Min = GlobalVars.constants.AircraftCategory[aircraftCategoryData.VfafMin][_AirCat];
			FAFIASInterval.Max = GlobalVars.constants.AircraftCategory[aircraftCategoryData.VfafMax][_AirCat];

			FAFGradientInterval.Min = GlobalVars.constants.Pansops[ePANSOPSData.arFADescent_Min].Value;
			FAFGradientInterval.Max = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arFADescent_Max].Value[_AirCat];

			//double hTas = _RWYTHRPrj.Z > 900.0 ? _RWYTHRPrj.Z : 900.0;
			//double hTas = _refElevation;
			_TAS = ARANMath.IASToTAS(FAFIASInterval.Max, _refElevation, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			// GlobalVars.CurrADHP.ISAtC) + GlobalVars.constants.Pansops[ePANSOPSData.arNearTerrWindSp].Value;
			_FicTHR = FRWYDirection;        //?????????????

			//_MApt ============================================================================================================
			//GlobalVars.gAranGraphics.DrawPointWithText(FFarMAPt, "FFarMAPt ");
			//LegBase.ProcessMessages();

			if (FFarMAPt != null)
				FMAPt.PrjPt = FFarMAPt;
			else
				FMAPt.PrjPt = _FicTHR;

			FMAPt.OutDirection = F3ApproachDir;
			FMAPt.EntryDirection = F3ApproachDir;

			double fTAS = ARANMath.IASToTAS(FAFIASInterval.Max, _refElevation, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value) + GlobalVars.constants.Pansops[ePANSOPSData.arNearTerrWindSp].Value;
			double t0 = GlobalVars.constants.Pansops[ePANSOPSData.arSOCdelayTime].Value + GlobalVars.constants.Pansops[ePANSOPSData.dpPilotTolerance].Value;
			FMAPt.SOCDistance = t0 * fTAS + FMAPt.ATT;
			FMAPt.ConstructAltitude = FMAPt.NomLineAltitude = _refElevation;
			FMAPt.IAS = FAFIASInterval.Max;

			// _FMAHF ============================================================================================================
			FMAHFstr.PrjPt = ARANFunctions.LocalToPrj(_FicTHR, F3ApproachDir, 25000.0);
			FMAHFstr.OutDirection = F3ApproachDir;
			FMAHFstr.EntryDirection = F3ApproachDir;

			FMAHFstr.ConstructAltitude = FMAHFstr.NomLineAltitude = _refElevation;
			FMAHFstr.IAS = FAFIASInterval.Max;

			FMAHFturn.OutDirection = F3ApproachDir;
			FMAHFturn.EntryDirection = F3ApproachDir;

			FMAHFturn.ConstructAltitude = FMAHFturn.NomLineAltitude = _refElevation;
			FMAHFturn.IAS = FAFIASInterval.Max;

			// _FAF ============================================================================================================

			int n = GlobalVars.WPTList.Length;
			comboBox02_01.Items.Clear();

			for (int i = 0; i < n; i++)
			{
				Point LocalPoint = ARANFunctions.PrjToLocal(_FicTHR, F3ApproachDir, GlobalVars.WPTList[i].pPtPrj);

				if (-LocalPoint.X >= FAFDistanceInterval.Min && -LocalPoint.X <= FAFDistanceInterval.Max && Math.Abs(LocalPoint.Y) <= 0.25 * GlobalVars.constants.GNSS[eFIXRole.FAF_].XTT)
					comboBox02_01.Items.Add(GlobalVars.WPTList[i]);
			}

			radioButton02_03.Enabled = comboBox02_01.Items.Count > 0;


			if (!radioButton02_03.Enabled || !(radioButton02_01.Checked || radioButton02_02.Checked || radioButton02_03.Checked))
				radioButton02_01.Checked = true;

			_FAF.OutDirection = F3ApproachDir;
			_FAF.EntryDirection = F3ApproachDir;

			if (radioButton02_03.Checked)
				_FAF.Name = comboBox02_01.Text;
			else
				_FAF.Name = textBox02_03.Text;

			_FAF.ConstructAltitude = _FAF.NomLineAltitude = _refElevation;
			_FAF.IAS = FAFIASInterval.Max;

			//fTAS = IASToTAS(GAircraftCategoryConstants.Constant[cVfafMax].Value[FAircraftCategory], FAerodrome.Elevation, AerodromeTemperature - GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value) +
			// GlobalVars.constants.Pansops[ePANSOPSData.arNearTerrWindSp].Value;
			// FAF
			// FFAF.Bank = GlobalVars.constants.Pansops[ePANSOPSData.arBankAngle].Value;
			// FAFGradient =================================================
			_FAFGradient = FAFGradientInterval.Min;
			if (_landingType == eLandingType.StraightIn)
			{
				FAFAltitudeInterval.Min = FAFDistanceInterval.Min * prmFAFGradient + prmAbvTrshld + _refElevation;
				FAFAltitudeInterval.Max = FAFDistanceInterval.Max * prmFAFGradient + prmAbvTrshld + _refElevation;
			}
			else
			{
				FAFAltitudeInterval.Min = FAFDistanceInterval.Min * prmFAFGradient + _circlingMinOCH + _refElevation;
				FAFAltitudeInterval.Max = FAFDistanceInterval.Max * prmFAFGradient + _circlingMinOCH + _refElevation;
			}

			textBox02_05.Text = GlobalVars.unitConverter.GradientToDisplayUnits(_FAFGradient).ToString();
			textBox02_05.Tag = textBox02_05.Text;

			// IF FIX ======================================================================
			_firstIF.ISAtC = GlobalVars.CurrADHP.ISAtC;
			//_firstIF.BankAngle = GlobalVars.constants.Pansops[ePANSOPSData.arBankAngle].Value;
			_firstIF.AppliedGradient = GlobalVars.constants.Pansops[ePANSOPSData.arFADescent_Min].Value;

			_firstIF.OutDirection = F3ApproachDir;
			_firstIF.EntryDirection = F3ApproachDir;

			if (checkBox02_03.Checked)
				_firstIF.Role = eFIXRole.SDF;
			else
				_firstIF.Role = eFIXRole.IF_;

			if (radioButton02_07.Checked)
				_firstIF.Name = comboBox02_04.Text;
			else
				_firstIF.Name = textBox02_11.Text;

			// IF Distance ===========================================================================
			_IFDistanceInterval.Min = GlobalVars.constants.Pansops[ePANSOPSData.arImRange_Min].Value;
			_IFDistanceInterval.Max = GlobalVars.constants.Pansops[ePANSOPSData.arImRange_Max].Value;
			_IFDistance = _IFDistanceInterval.Max;

			textBox02_09.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_IFDistance).ToString();
			textBox02_09.Tag = textBox02_09.Text;

			// IF Speed ===========================================================================
			_IFIASInterval.Min = GlobalVars.constants.AircraftCategory[aircraftCategoryData.ViafMin].Value[_AirCat];
			_IFIASInterval.Max = GlobalVars.constants.AircraftCategory[aircraftCategoryData.ViafMax].Value[_AirCat];

			_IFSpeed = _IFIASInterval.Max;
			_firstIF.IAS = _IFSpeed;

			textBox02_08.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_IFSpeed).ToString();
			textBox02_08.Tag = textBox02_08.Text;

			// IF Altitude ===========================================================================
			_IFAltitudeInterval.Min = FAFAltitudeInterval.Min;
			_IFAltitudeInterval.Max = FAFAltitudeInterval.Max + (prmIFDistance - GlobalVars.constants.AircraftCategory[aircraftCategoryData.arImHorSegLen].Value[_AirCat]) * GlobalVars.constants.Pansops[ePANSOPSData.arFADescent_Min].Value;

			_IFAltitude = _IFAltitudeInterval.Min;

			_firstIF.ConstructAltitude = _firstIF.NomLineAltitude = FAFAltitudeInterval.Max + _IFDistance * FAFGradientInterval.Min;
			//_firstIF.ConstructAltitude = _firstIF.NomLineAltitude = prmFAFAltitude;

			textBox02_13.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_IFAltitude).ToString();
			textBox02_13.Tag = textBox02_13.Text;

			// IF Course ===========================================================================
			IFCourseInterval.InDegree = false;
			IFCourseInterval.Circular = true;

			if (_approachType == eApproachType.LNAV)
			{
				IFCourseInterval.Min = F3ApproachDir - GlobalVars.constants.Pansops[ePANSOPSData.arImMaxIntercept].Value;
				IFCourseInterval.Max = F3ApproachDir + GlobalVars.constants.Pansops[ePANSOPSData.arImMaxIntercept].Value;
			}
			else
			{
				IFCourseInterval.Min = F3ApproachDir - GlobalVars.constants.Pansops[ePANSOPSData.arImMaxIntercept].Value;
				IFCourseInterval.Max = F3ApproachDir + GlobalVars.constants.Pansops[ePANSOPSData.arImMaxIntercept].Value;
			}
			//TmpRange.Left = Modulus(F3ApproachDir - GlobalVars.constants.Pansops[ePANSOPSData.arImMaxIntercept].Value, 2 * PI);
			//TmpRange.Right = Modulus(F3ApproachDir + GlobalVars.constants.Pansops[ePANSOPSData.arImMaxIntercept].Value, 2 * PI);

			_IFCourse = F3ApproachDir;

			_firstIF.EntryDirection = _IFCourse;
			_firstIF.OutDirection = _IFCourse;

			// IF MaxAngle ===========================================================================
			_IFMaxAngleInterval.Min = 0.0;
			_IFMaxAngleInterval.Max = 0.5 * Math.PI;

			_IFMaxAngle = _IFMaxAngleInterval.Max;
			textBox02_12.Text = GlobalVars.unitConverter.AngleToDisplayUnits(_IFMaxAngle).ToString();
			textBox02_12.Tag = textBox02_12.Text;

			// FAF =========================================================
			//_FAFDistance = FAFDistanceInterval.Max;
			//textBox02_02.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_FAFDistance).ToString();
			//textBox02_02.Tag = textBox02_02.Text;
			//prmFAFGradient = FAFGradientInterval.Min;
			_FAFAltitude = 0;
			prmFAFAltitude = FAFAltitudeInterval.Max;


			prmFAFIAS = FAFIASInterval.Max;

			//Interval TmpRange = default(Interval);
			//if (!FAligned) TmpRange.Left = TmpRange.Left + Hypot(FRWYDirection.Prj.X - FFarMAPt.X, FRWYDirection.Prj.Y - FFarMAPt.Y);
			//Application.DoEvents();
			//FFAF.PrjPt = ARANFunctions.LocalToPrj(_FicTHR, F3ApproachDir + Math.PI, FAFDistanceInterval.Max, 0.0);

			if (radioButton02_04.Checked)
				radioButton02_04_CheckedChanged(radioButton02_04, null);
			else
				radioButton02_04.Checked = true;
		}

		void BackToPageII()
		{
			_intermediateLeq.DeleteGraphics();
			_finalLeq.DeleteGraphics();
			_straightMissedLeq.DeleteGraphics();
		}

		#endregion

		#region Page 3

		#region FAF

		#region Routines

		void ApplayFAFAltitude()
		{
			_FAF.ConstructAltitude = _FAF.NomLineAltitude = prmFAFAltitude;

			_IFAltitudeInterval.Min = prmFAFAltitude;
			_IFAltitudeInterval.Max = prmFAFAltitude + (prmIFDistance - GlobalVars.constants.AircraftCategory[aircraftCategoryData.arImHorSegLen].Value[_AirCat]) * GlobalVars.constants.Pansops[ePANSOPSData.arFADescent_Min].Value;

			if (radioButton02_01.Checked)
			{
				if (_landingType == eLandingType.StraightIn)
					prmFAFDistance = (prmFAFAltitude - prmAbvTrshld - _refElevation) / prmFAFGradient;
				else
					prmFAFDistance = (prmFAFAltitude - _circlingMinOCH - _refElevation) / prmFAFGradient;
			}

			if (_IFAltitude < _IFAltitudeInterval.Min)
				prmIFAltitude = _IFAltitudeInterval.Min;
			else if (_IFAltitude > _IFAltitudeInterval.Max)
				prmIFAltitude = _IFAltitudeInterval.Max;
		}

		void ApplayFAFDistance()
		{
			//if (!radioButton02_03.Checked)
			{
				_FAF.PrjPt = ARANFunctions.LocalToPrj(_FicTHR, F3ApproachDir + Math.PI, _FAFDistance, 0.0);
				_firstIF.PrjPt = ARANFunctions.LocalToPrj(_FAF.PrjPt, _IFCourse + Math.PI, _IFDistance, 0.0);
			}
			//FFAF.RefreshGraphics();

			if (!radioButton02_01.Checked)
			{
				if (_landingType == eLandingType.StraightIn)
					prmFAFAltitude = prmFAFDistance * prmFAFGradient + prmAbvTrshld + _refElevation;
				else
					prmFAFAltitude = prmFAFDistance * prmFAFGradient + _circlingMinOCH + _refElevation;
			}

			RecreateFinalLegGeometry();
		}

		void ApplayFAFGradient()
		{
			if (_landingType == eLandingType.StraightIn)
			{
				FAFAltitudeInterval.Min = FAFDistanceInterval.Min * prmFAFGradient + prmAbvTrshld + _refElevation;
				FAFAltitudeInterval.Max = FAFDistanceInterval.Max * prmFAFGradient + prmAbvTrshld + _refElevation;
			}
			else
			{
				FAFAltitudeInterval.Min = FAFDistanceInterval.Min * prmFAFGradient + _circlingMinOCH + _refElevation;
				FAFAltitudeInterval.Max = FAFDistanceInterval.Max * prmFAFGradient + _circlingMinOCH + _refElevation;
			}

			if (radioButton02_01.Checked)
			{
				double fTmp = prmFAFAltitude;

				if (fTmp < FAFAltitudeInterval.Min)
					prmFAFAltitude = FAFAltitudeInterval.Min;
				else if (fTmp > FAFAltitudeInterval.Max)
					prmFAFAltitude = FAFAltitudeInterval.Max;

				if (fTmp == prmFAFAltitude)
				{
					if (_landingType == eLandingType.StraightIn)
						prmFAFDistance = (prmFAFAltitude - prmAbvTrshld - _refElevation) / prmFAFGradient;
					else
						prmFAFDistance = (prmFAFAltitude - _circlingMinOCH - _refElevation) / prmFAFGradient;
				}
			}
			else
			{
				if (_landingType == eLandingType.StraightIn)
					prmFAFAltitude = prmFAFDistance * prmFAFGradient + prmAbvTrshld + _refElevation;
				else
					prmFAFAltitude = prmFAFDistance * prmFAFGradient + _circlingMinOCH + _refElevation;
			}
		}

		void ApplayFAFIAS()
		{
			//_firstIF.IAS = _FAFIAS;
			_FAF.IAS = _FAFIAS;
			FMAPt.IAS = _FAFIAS;
			FMAHFstr.IAS = _FAFIAS;
			FMAHFturn.IAS = _FAFIAS;

			//FMAHF.PrjPt = ARANFunctions.LocalToPrj(_FicTHR, F3ApproachDir, 25000.0);

			RecreateFinalLegGeometry();
		}

		void RecreateFinalLegGeometry()
		{
			FMAHFstr.ReCreateArea();
			FMAPt.ReCreateArea();
			_FAF.ReCreateArea();

			_firstIF.OutDirection = _firstIF.EntryDirection;
			_firstIF.ReCreateArea();

			_intermediateLeq.CreateGeometry(null, GlobalVars.CurrADHP);

			//GlobalVars.gAranGraphics.DrawMultiPolygon(_intermediateLeq.FullArea, eFillStyle.sfsBackwardDiagonal);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(_intermediateLeq.PrimaryArea, eFillStyle.sfsForwardDiagonal);
			//LegBase.ProcessMessages();

			_finalLeq.CreateGeometry(_intermediateLeq, GlobalVars.CurrADHP);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(_finalLeq.FullArea, eFillStyle.sfsBackwardDiagonal);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(_finalLeq.PrimaryArea, eFillStyle.sfsForwardDiagonal);
			//LegBase.ProcessMessages();

			_straightMissedLeq.CreateGeometry(_finalLeq, GlobalVars.CurrADHP);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(_straightMissedLeq.FullArea, eFillStyle.sfsBackwardDiagonal);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(_straightMissedLeq.PrimaryArea, eFillStyle.sfsForwardDiagonal);
			//LegBase.ProcessMessages();

			_straightMissedLeq.RefreshGraphics();

			/*===============================================================*/
			//Application.DoEvents();
			//_intermediateLeq.Obstacles = Functions.GetLegProtectionAreaObstacles(_intermediateLeq, GlobalVars.ObstacleList, GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value, _refElevation);
			//_reportForm.FillPage03(_intermediateLeq.Obstacles);

			//int m = _intermediateLeq.Obstacles.Obstacles.Length;
			//int n = _intermediateLeq.Obstacles.Parts.Length;

			//for (int j = 0; j < m; j++)
			//	_intermediateLeq.Obstacles.Obstacles[j].NIx = -1;


			//ObstacleData[] SignObsaclesParts = new ObstacleData[n];
			//Array.Copy(_intermediateLeq.Obstacles.Parts, SignObsaclesParts, n);
			//Array.Sort(SignObsaclesParts, Comparers.PartComparerDist);

			//Array.Resize(ref _intermediateSignObsacles.Obstacles, m);
			//Array.Resize(ref _intermediateSignObsacles.Parts, n);

			//int k = -1, l = -1;
			//double startA = _FAFAltitude - _refElevation;
			//double nextA = startA;

			//for (int i = 0; i < n; i++)
			//{
			//	if (SignObsaclesParts[i].ReqH <= nextA)
			//		continue;

			//	nextA = SignObsaclesParts[i].ReqH;

			//	k++;
			//	_intermediateSignObsacles.Parts[k] = SignObsaclesParts[i];

			//	if (_intermediateLeq.Obstacles.Obstacles[SignObsaclesParts[i].Owner].NIx < 0)
			//	{
			//		l++;
			//		_intermediateSignObsacles.Obstacles[l] = _intermediateLeq.Obstacles.Obstacles[SignObsaclesParts[i].Owner];
			//		_intermediateSignObsacles.Obstacles[l].PartsNum = 0;
			//		_intermediateLeq.Obstacles.Obstacles[SignObsaclesParts[i].Owner].NIx = l;
			//	}

			//	_intermediateSignObsacles.Parts[k].Owner = _intermediateLeq.Obstacles.Obstacles[SignObsaclesParts[i].Owner].NIx;
			//	_intermediateSignObsacles.Parts[k].Index = _intermediateSignObsacles.Obstacles[_intermediateSignObsacles.Parts[k].Owner].PartsNum;  //?????????

			//	_intermediateSignObsacles.Parts[k].PDG = (_intermediateSignObsacles.Parts[k].ReqH - startA) / _intermediateSignObsacles.Parts[k].Dist;
			//	_intermediateSignObsacles.Obstacles[_intermediateSignObsacles.Parts[k].Owner].PartsNum++;
			//}

			////TimeSpan span = DateTime.Now - start;
			////MessageBox.Show("Items " + n + "\n\rTotal time " + span.TotalMilliseconds);

			//Array.Resize(ref _intermediateSignObsacles.Obstacles, l + 1);
			//Array.Resize(ref _intermediateSignObsacles.Parts, k + 1);

			//_reportForm.FillPage02(_intermediateSignObsacles);

			//Application.DoEvents();

			/*===============================================================*/

			//label02_10.Visible = false;
			//groupBox03_01.Enabled = true;
			//NextBtn.Enabled = true;

			////FAFGradientInterval.Min = GlobalVars.constants.Pansops[ePANSOPSData.arFADescent_Min].Value;
			////FAFGradientInterval.Max = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arFADescent_Max].Value[_AirCat];

			//if (k >= 0)
			//{
			//	if ((_intermediateSignObsacles.Parts[0].Dist < 2 * 1852.0 || (_intermediateSignObsacles.Parts[0].Dist < 5 * 1852.0 && _intermediateSignObsacles.Parts[0].PDG > FAFGradientInterval.Min)))
			//	{
			//		label02_10.Visible = true;
			//		groupBox03_01.Enabled = false;
			//		NextBtn.Enabled = false;
			//	}
			//}

			///*===============================================================*/
			//_finalLeq.Obstacles = Functions.GetLegProtectionAreaObstacles(_finalLeq, GlobalVars.ObstacleList, GlobalVars.constants.Pansops[ePANSOPSData.arFASeg_FAF_MOC].Value, _refElevation);

			//Point pt0 = ARANFunctions.LocalToPrj(_FAF.PrjPt, F3ApproachDir + Math.PI, _FAF.ATT);
			//double fAppHeight = prmFAFAltitude - _refElevation - GlobalVars.constants.Pansops[ePANSOPSData.arFASeg_FAF_MOC].Value;      //arISegmentMOC
			//double fL = fAppHeight / GlobalVars.constants.Pansops[ePANSOPSData.arFixMaxIgnorGrd].Value;

			//int k = -1;
			//_FAFOCH = FFAOCA - _refElevation;

			//for (int i = 0; i < _finalLeq.Obstacles.Parts.Length; i++)
			//{
			//	double fReqH = _finalLeq.Obstacles.Parts[i].ReqH;
			//	_finalLeq.Obstacles.Parts[i].Ignored = false;

			//	ARANFunctions.PrjToLocal(pt0, F3ApproachDir, _finalLeq.Obstacles.Parts[i].pPtPrj, out double fDist, out double y);

			//	if (fDist < GlobalVars.constants.Pansops[ePANSOPSData.arFIX15PlaneRang].Value)
			//	{
			//		double fPlane15h = GlobalVars.constants.Pansops[ePANSOPSData.arFixMaxIgnorGrd].Value * (fL - fDist);
			//		if (fPlane15h > _finalLeq.Obstacles.Parts[i].Height)
			//		{
			//			fReqH = 0.0;
			//			_finalLeq.Obstacles.Parts[i].Ignored = true;
			//		}
			//	}

			//	if (fReqH > _FAFOCH)
			//	{
			//		_FAFOCH = fReqH;
			//		k = i;
			//	}
			//}

			//comboBox02_02_SelectedIndexChanged(comboBox02_02, null);

			//textBox02_07.Text = "";
			//if (k >= 0)
			//	textBox02_07.Text = _finalLeq.Obstacles.Obstacles[_finalLeq.Obstacles.Parts[k].Owner].UnicalName;

			//_reportForm.FillPage06(_finalLeq.Obstacles);

			RecreateInitialLegGeometry();
		}

		#endregion

		#region Events

		private void comboBox02_02_SelectedIndexChanged(object sender, EventArgs e)
		{
			double fTmp = _FAFOCH;
			if (comboBox02_02.SelectedIndex == 0)
				fTmp += _refElevation;

			double roundingFactor = 5.0 + 5.0 * GlobalVars.unitConverter.HeightUnitIndex;

			fTmp = System.Math.Round(fTmp * GlobalVars.unitConverter.HeightConverter[GlobalVars.unitConverter.HeightUnitIndex].Multiplier / roundingFactor + 0.4999) * roundingFactor;

			textBox02_06.Text = fTmp.ToString();

			//if (comboBox02_02.SelectedIndex == 0)
			//	textBox02_06.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_FinalMOC + _refElevation, eRoundMode.SPECIAL_CEIL).ToString();
			//else
			//	textBox02_06.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_FinalMOC, eRoundMode.SPECIAL_CEIL).ToString();
		}

		private void comboBox02_01_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox02_01.SelectedIndex < 0)
				return;

			if (!radioButton02_03.Checked)
				return;

			WPT_FIXType sigPoint = (WPT_FIXType)(comboBox02_01.SelectedItem);
			label02_02.Text = sigPoint.TypeCode.ToString(); //sigPoint.Name;

			_FAF.Name = sigPoint.Name;
			_FAF.CallSign = sigPoint.CallSign;
			_FAF.Id = sigPoint.Identifier;

			//_FAF.PrjPt = sigPoint.pPtPrj;

			//prmFAFDistance = Hypot(sigPoint.Prj.Y - FMAPt.PrjPt.Y, sigPoint.Prj.X - FMAPt.PrjPt.X);
			prmFAFDistance = Math.Abs(ARANFunctions.PointToLineDistance(FMAPt.PrjPt, sigPoint.pPtPrj, F3ApproachDir + 0.5 * Math.PI));
		}

		private void radioButton02_01_CheckedChanged(object sender, EventArgs e)
		{
			if (!((RadioButton)sender).Checked)
				return;

			bFAFfromList = sender == radioButton02_03;

			comboBox02_01.Enabled = bFAFfromList;
			textBox02_03.Enabled = !bFAFfromList;

			//label02_02.Enabled = bFromList;

			if (bFAFfromList)
			{
				textBox02_01.ReadOnly = true;
				textBox02_02.ReadOnly = true;

				if (comboBox02_01.SelectedIndex < 0)
					comboBox02_01.SelectedIndex = 0;
				else if (comboBox02_01.SelectedIndex >= comboBox02_01.Items.Count)
					comboBox02_01.SelectedIndex = comboBox02_01.Items.Count - 1;
				else
					comboBox02_01_SelectedIndexChanged(null, null);
			}
			else
			{
				label02_02.Text = "";
				//label02_02.Text = "Designated Point";
				_FAF.Id = Guid.Empty;
				_FAF.Name = textBox02_03.Text;
				_FAF.CallSign = textBox02_03.Text;
				//_FAF.RefreshGraphics();

				if (sender == radioButton02_01)
				{
					textBox02_01.ReadOnly = false;
					textBox02_02.ReadOnly = true;

					_FAFAltitude = 0;
					textBox02_01.Tag = "";

					textBox02_01_Validating(textBox02_01, null);
				}
				else
				{
					textBox02_01.ReadOnly = true;
					textBox02_02.ReadOnly = false;
					_FAFDistance = 0;
					textBox02_02.Tag = "";
					textBox02_02_Validating(textBox02_02, null);
				}
			}
		}

		private void textBox02_01_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox02_01_Validating(textBox02_01, new CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox02_01.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox02_01_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox02_01.Text, out fTmp))
			{
				textBox02_01.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_FAFAltitude).ToString();
				return;
			}

			if (textBox02_01.Tag != null && textBox02_01.Tag.ToString() == textBox02_01.Text)
				return;

			fTmp = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);

			if (fTmp < FAFAltitudeInterval.Min)
				fTmp = FAFAltitudeInterval.Min;

			if (fTmp > FAFAltitudeInterval.Max)
				fTmp = FAFAltitudeInterval.Max;

			prmFAFAltitude = fTmp;
		}

		private void textBox02_02_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox02_02_Validating(textBox02_02, null);
			else
				Functions.TextBoxFloat(ref eventChar, textBox02_02.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox02_02_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox02_02.Text, out fTmp))
			{
				textBox02_02.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_FAFDistance).ToString();
				return;
			}

			if (textBox02_02.Tag != null && textBox02_02.Tag.ToString() == textBox02_02.Text)
				return;

			fTmp = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			if (fTmp < FAFDistanceInterval.Min)
				fTmp = FAFDistanceInterval.Min;

			if (fTmp > FAFDistanceInterval.Max)
				fTmp = FAFDistanceInterval.Max;

			prmFAFDistance = fTmp;
		}

		private void textBox02_03_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
			{
				textBox02_03_Validating(textBox02_03, null);
				eventChar = '\0';
			}
			else if (eventChar >= ' ')// && (eventChar < '0' || eventChar > '9'))         // || eventChar != '_')
			{
				char alfa = (char)(eventChar & ~32);

				if (alfa < 'A' || alfa > 'Z')
					eventChar = '\0';
				else
					eventChar = alfa;
			}

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox02_03_Validating(object sender, CancelEventArgs e)
		{
			_FAF.Name = textBox02_03.Text;
			_FAF.CallSign = textBox02_03.Text;
			_FAF.RefreshGraphics();
		}

		private void textBox02_04_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox02_04_Validating(textBox02_04, null);
			else
				Functions.TextBoxFloat(ref eventChar, textBox02_04.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox02_04_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox02_04.Text, out fTmp))
			{
				textBox02_04.Text = GlobalVars.unitConverter.SpeedToInternalUnits(_FAFIAS).ToString();
				return;
			}

			if (textBox02_04.Tag != null && textBox02_04.Tag.ToString() == textBox02_04.Text)
				return;

			fTmp = GlobalVars.unitConverter.SpeedToInternalUnits(fTmp);

			if (fTmp < FAFIASInterval.Min)
				fTmp = FAFIASInterval.Min;

			if (fTmp > FAFIASInterval.Max)
				fTmp = FAFIASInterval.Max;

			prmFAFIAS = fTmp;
		}

		private void textBox02_05_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox02_05_Validating(textBox02_05, new CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox02_05.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox02_05_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox02_05.Text, out fTmp))
			{
				textBox02_05.Text = GlobalVars.unitConverter.GradientToDisplayUnits(_FAFGradient).ToString();
				return;
			}

			if (textBox02_05.Tag != null && textBox02_05.Tag.ToString() == textBox02_05.Text)
				return;

			fTmp = GlobalVars.unitConverter.GradientToInternalUnits(fTmp);

			if (fTmp < FAFGradientInterval.Min)
				fTmp = FAFGradientInterval.Min;

			if (fTmp > FAFGradientInterval.Max)
				fTmp = FAFGradientInterval.Max;

			prmFAFGradient = fTmp;
		}

		#endregion

		#endregion

		#region IF

		#region Routines
		void RecreateInitialLegGeometry()
		{
			double AppAzt = ARANFunctions.DirToAzimuth(_FAF.PrjPt, _IFCourse, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			textBox02_10.Text = GlobalVars.unitConverter.AzimuthToDisplayUnits(AppAzt).ToString();
			textBox02_10.Tag = textBox02_10.Text;

			//Application.DoEvents();
			double fTmp = _IFAltitude;

			if (fTmp < _IFAltitudeInterval.Min)
				fTmp = _IFAltitudeInterval.Min;
			if (fTmp > _IFAltitudeInterval.Max)
				fTmp = _IFAltitudeInterval.Max;

			_IFAltitude = 0.0;
			prmIFAltitude = fTmp;

			//Application.DoEvents();
			//FlightPhaseUnit.InitModule(GPANSOPSConstants, GAircraftCategoryConstants, _AirCat);
			//FFAOCA = GlobalVars.constants.Pansops[ePANSOPSData.arFASeg_FAF_MOC].Value + _refElevation;
			//label04_09.Text= "";
			//FFAF.RefreshGraphics();

			double fAngle1 = ARANMath.DegToRad(2.0);
			double fAngle2 = ARANMath.DegToRad(50.0);

			double fDist = _firstIF.CalcConstructionFromMinStablizationDistance(0.5 * Math.PI);
			double fDistTreshol1 = _FAF.CalcConstructionInToMinStablizationDistance(fAngle1) + fDist;
			double fDistTreshol2 = _FAF.CalcConstructionInToMinStablizationDistance(fAngle2) + fDist;

			int ix = 0;

			WPT_FIXType prevFIX = default(WPT_FIXType);
			if (radioButton02_07.Checked)
				prevFIX = (WPT_FIXType)comboBox02_04.SelectedItem;

			comboBox02_04.Items.Clear();

			foreach (var sigPoint in GlobalVars.WPTList)
			{
				ARANFunctions.PrjToLocal(_FAF.PrjPt, F3ApproachDir + Math.PI, sigPoint.pPtPrj, out double x, out double y);

				if (x <= 0)
					continue;

				fDist = ARANMath.Hypot(y, x);
				double fAngle = Math.Abs(Math.Atan2(y, x));

				double fMinDist = fDistTreshol1;
				if (fAngle > fAngle1)
					fMinDist = fDistTreshol2;

				if (fDist <= GlobalVars.constants.Pansops[ePANSOPSData.arImRange_Max].Value &&
					 fDist > fMinDist &&
					 fAngle <= GlobalVars.constants.Pansops[ePANSOPSData.arImMaxIntercept].Value)
				{
					if (prevFIX.Identifier == sigPoint.Identifier)
						ix = comboBox02_04.Items.Count;
					comboBox02_04.Items.Add(sigPoint);
				}
			}

			radioButton02_07.Enabled = comboBox02_04.Items.Count > 0;

			if (radioButton02_07.Enabled)   //radioButton04_04.Checked and
				comboBox02_04.SelectedIndex = ix;
		}

		void ApplayIFAltitude()
		{
			_firstIF.ConstructAltitude = _firstIF.NomLineAltitude = _IFAltitude;
			ApplyIFDistanceAndDirection(prmIFDistance, prmIFCourse);
		}

		void ApplayIFSpeed()
		{
			//_FAF.IAS = _IFSpeed;
			_firstIF.IAS = _IFSpeed;


			ApplyIFDistanceAndDirection(prmIFDistance, prmIFCourse);
		}

		void ApplayIFCourse()
		{
			ApplyIFDistanceAndDirection(prmIFDistance, prmIFCourse);
		}

		void ApplayIFDist()
		{
			_IFAltitudeInterval.Max = prmFAFAltitude + (_IFDistance - GlobalVars.constants.AircraftCategory[aircraftCategoryData.arImHorSegLen].Value[_AirCat]) * GlobalVars.constants.Pansops[ePANSOPSData.arFADescent_Min].Value;
			_IFAltitudeInterval.Min = prmFAFAltitude;

			if (ixIF >= 0)
				_IFAltitudeInterval.Min = GlobalVars.constants.Pansops[ePANSOPSData.arFixMaxIgnorGrd].Value * (_IFDistance - _intermediateSignObsacles.Parts[ixIF].Dist + _firstIF.ATT) + _intermediateSignObsacles.Parts[ixIF].Elev;

			double hIF = _IFAltitude;
			if (hIF > _IFAltitudeInterval.Max)
				hIF = _IFAltitudeInterval.Max;

			if (hIF < _IFAltitudeInterval.Min)
				hIF = _IFAltitudeInterval.Min;
			if (Math.Abs(hIF - _IFAltitude) > ARANMath.EpsilonDistance)
				prmIFAltitude = hIF;

			ApplyIFDistanceAndDirection(_IFDistance, prmIFCourse);
		}

		#endregion

		#region Events

		private void comboBox02_03_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox02_03.SelectedIndex < 0)
				return;

			_firstIF.PBNType = (ePBNClass)comboBox02_03.SelectedItem;
			//FIF.RefreshGraphics();
			_intermediateLeq.CreateGeometry(null, GlobalVars.CurrADHP);
			_intermediateLeq.RefreshGraphics();
		}

		private void comboBox02_04_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!radioButton02_07.Checked)
				return;

			int K = comboBox02_04.SelectedIndex;

			if (K < 0)
				return;

			WPT_FIXType sigPoint = (WPT_FIXType)(comboBox02_04.SelectedItem);
			_firstIF.Name = sigPoint.Name;
			_firstIF.CallSign = sigPoint.CallSign;
			_firstIF.Id = sigPoint.Identifier;
			_firstIF.PrjPt = sigPoint.pPtPrj;

			label02_19.Text = sigPoint.TypeCode.ToString();

			double Angle = Math.Atan2(_FAF.PrjPt.Y - sigPoint.pPtPrj.Y, _FAF.PrjPt.X - sigPoint.pPtPrj.X);
			double Dist = ARANMath.Hypot(_FAF.PrjPt.Y - sigPoint.pPtPrj.Y, _FAF.PrjPt.X - sigPoint.pPtPrj.X);

			_IFCourse = Angle;
			prmIFCourse = Angle;

			_IFDistance = Dist;
			prmIFDistance = Dist;

			ApplayIFDist();
		}

		private void radioButton02_04_CheckedChanged(object sender, EventArgs e)
		{
			if (!((RadioButton)sender).Checked)
				return;

			comboBox02_03.Items.Clear();

			if (sender == radioButton02_04)
			{
				_firstIF.SensorType = eSensorType.GNSS;
				checkBox02_01.Enabled = false;
				comboBox02_03.Items.Add(ePBNClass.RNP_APCH);
				comboBox02_03.Items.Add(ePBNClass.RNAV1);
				comboBox02_03.Items.Add(ePBNClass.RNAV2);
			}
			else
			{
				_firstIF.SensorType = eSensorType.DME_DME;
				checkBox02_01.Enabled = true;
				comboBox02_03.Items.Add(ePBNClass.RNAV1);
				comboBox02_03.Items.Add(ePBNClass.RNAV2);
			}

			comboBox02_03.SelectedIndex = 0;
		}

		private void radioButton02_06_CheckedChanged(object sender, EventArgs e)
		{
			if (!((RadioButton)sender).Checked)
				return;
			//comboBox04_02

			bool bIFFromList = sender == radioButton02_07;

			textBox02_09.ReadOnly = bIFFromList;
			textBox02_10.ReadOnly = bIFFromList || checkBox02_02.Checked;
			textBox02_11.Visible = !bIFFromList;
			comboBox02_04.Visible = bIFFromList;
			checkBox02_02.Enabled = !bIFFromList;
			//comboBox02_04.Enabled = bIFFromList;
			//label04_09.Enabled = bFromList;

			if (bIFFromList)
			{
				checkBox02_02.Checked = false;

				if (comboBox02_04.SelectedIndex < 0)
					comboBox02_04.SelectedIndex = 0;
				else
					comboBox02_04_SelectedIndexChanged(comboBox02_04, null);
			}
			else
			{
				label02_19.Text = "";
				_firstIF.Id = Guid.Empty;
				_firstIF.Name = textBox02_11.Text;
				_firstIF.CallSign = textBox02_11.Text;
				//_firstIF.RefreshGraphics();

				textBox02_09_Validating(textBox02_09, null);
			}
		}

		private void checkBox02_01_CheckedChanged(object sender, EventArgs e)
		{
			_firstIF.MultiCoverage = checkBox02_01.Checked;
			_intermediateLeq.CreateGeometry(null, GlobalVars.CurrADHP);
			_intermediateLeq.RefreshGraphics();
		}

		private void checkBox02_02_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox02_02.Checked)
			{
				if (radioButton02_07.Checked)
				{
					if (ARANMath.SubtractAngles(prmIFCourse, F3ApproachDir) > ARANMath.EpsilonRadian)
						radioButton02_06.Checked = true;
				}
				prmIFCourse = F3ApproachDir;
			}

			textBox02_10.ReadOnly = radioButton02_07.Checked || checkBox02_02.Checked;
		}

		private void checkBox02_03_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox02_03.Checked)
			{
				//if (!radioButton02_07.Checked)
				//	_firstIF.Name = _InterSDF0DefNamae;
				//else
				//	_firstIF.Name = comboBox02_04.Text;
				_firstIF.Role = eFIXRole.SDF;
			}
			else
			{
				//if (!radioButton02_07.Checked)
				//	_firstIF.Name = textBox02_11.Text;
				//else
				//	_firstIF.Name = comboBox02_04.Text;
				_reportForm.RemovePage06();
				_firstIF.Role = eFIXRole.IF_;
			}

			//_firstIF.RefreshGraphics();

			ApplyIFDistanceAndDirection(_IFDistance, _IFCourse);
		}

		int FillSignificanObsacles(double Dir)
		{
			double dist = ARANFunctions.ReturnDistanceInMeters(fixFAF.PrjPt, _FAF.PrjPt);

			if (!double.IsNaN(dist) && Math.Abs(dist) < ARANMath.EpsilonDistance && _firstIF.EntryDirection == Dir && Math.Abs(_prevImArea - _intermediateLeq.FullAssesmentArea.Area) < ARANMath.Epsilon_2Distance)
				return -1;

			_prevImArea = _intermediateLeq.FullAssesmentArea.Area;

			_firstIF.EntryDirection = Dir;

			fixFAF.Assign(_FAF);
			fixIF.Assign(_firstIF);

			fixIF.EntryDirection = Dir;
			fixIF.OutDirection = Dir;
			fixFAF.EntryDirection = Dir;

			//_FAF = new FIX(eFIXRole.FAF_, GlobalVars.gAranEnv);
			//_firstIF = new FIX(eFIXRole.IF_, GlobalVars.gAranEnv);
			fixIF.PrjPt = ARANFunctions.LocalToPrj(_FAF.PrjPt, Dir + Math.PI, MaxIFDistance);

			LegApch legApch = new LegApch(fixIF, fixFAF, GlobalVars.gAranEnv);
			legApch.CreateGeometry(null, GlobalVars.CurrADHP);

			MultiPolygon tmpMultiPolygon;
			GeometryOperators fullGeoOp = new GeometryOperators();

			tmpMultiPolygon = (MultiPolygon)fullGeoOp.UnionGeometry(legApch.PrimaryAssesmentArea, _intermediateLeq.PrimaryAssesmentArea);
			legApch.PrimaryAssesmentArea = tmpMultiPolygon;
			//_intermediateLeq
			tmpMultiPolygon = (MultiPolygon)fullGeoOp.UnionGeometry(legApch.FullAssesmentArea, _intermediateLeq.FullAssesmentArea);
			legApch.FullAssesmentArea = tmpMultiPolygon;

			//legApch.RefreshGraphics();
			//Application.DoEvents();

			/*===============================================================*/
			legApch.Obstacles = Functions.GetLegProtectionAreaObstacles(legApch, GlobalVars.ObstacleList, GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value, _refElevation);

			int m = legApch.Obstacles.Obstacles.Length;
			int n = legApch.Obstacles.Parts.Length;

			for (int j = 0; j < m; j++)
				legApch.Obstacles.Obstacles[j].NIx = -1;

			ObstacleData[] SignObsaclesParts = new ObstacleData[n];
			Array.Copy(legApch.Obstacles.Parts, SignObsaclesParts, n);
			Array.Sort(SignObsaclesParts, Comparers.PartComparerDist);

			Array.Resize(ref _intermediateSignObsacles.Obstacles, m);
			Array.Resize(ref _intermediateSignObsacles.Parts, n);

			int k = -1, l = -1;
			double startA = _FAFAltitude - _refElevation;
			double nextA = startA;

			for (int i = 0; i < n; i++)
			{
				if (SignObsaclesParts[i].ReqH <= nextA)
					continue;

				nextA = SignObsaclesParts[i].ReqH;

				k++;
				_intermediateSignObsacles.Parts[k] = SignObsaclesParts[i];

				if (legApch.Obstacles.Obstacles[SignObsaclesParts[i].Owner].NIx < 0)
				{
					l++;
					_intermediateSignObsacles.Obstacles[l] = legApch.Obstacles.Obstacles[SignObsaclesParts[i].Owner];
					_intermediateSignObsacles.Obstacles[l].PartsNum = 0;
					legApch.Obstacles.Obstacles[SignObsaclesParts[i].Owner].NIx = l;
				}

				_intermediateSignObsacles.Parts[k].Owner = legApch.Obstacles.Obstacles[SignObsaclesParts[i].Owner].NIx;
				_intermediateSignObsacles.Parts[k].Index = _intermediateSignObsacles.Obstacles[_intermediateSignObsacles.Parts[k].Owner].PartsNum;  //?????????

				_intermediateSignObsacles.Parts[k].PDG = (_intermediateSignObsacles.Parts[k].ReqH - startA) / _intermediateSignObsacles.Parts[k].Dist;
				_intermediateSignObsacles.Obstacles[_intermediateSignObsacles.Parts[k].Owner].PartsNum++;
			}

			//TimeSpan span = DateTime.Now - start;
			//MessageBox.Show("Items " + n + "\n\rTotal time " + span.TotalMilliseconds);
			l++;
			k++;
			Array.Resize(ref _intermediateSignObsacles.Obstacles, l);
			Array.Resize(ref _intermediateSignObsacles.Parts, k);

			_reportForm.FillPage05(_intermediateSignObsacles);
			//========================================================================================
			label02_10.Visible = false;
			groupBox03_01.Enabled = true;
			NextBtn.Enabled = true;

			if (k > 0)
			{
				double dist0 = _intermediateSignObsacles.Parts[0].Dist;

				bool disabled = dist0 <= _IFDistanceInterval.Min || (dist0 < 2 * 1852.0 || (dist0 < 5 * 1852.0 && _intermediateSignObsacles.Parts[0].PDG > FAFGradientInterval.Min));

				//if ((dist0 < 2 * 1852.0 || (dist0 < 5 * 1852.0 && _intermediateSignObsacles.Parts[0].PDG > FAFGradientInterval.Min)))
				if (disabled)
				{
					label02_10.Visible = true;
					groupBox03_01.Enabled = false;
					NextBtn.Enabled = false;
				}
			}
			//========================================================================================

			return k;
		}

		void ApplyIFDistanceAndDirection(double Dist, double Dir)
		{
			if (!radioButton02_07.Checked)
				_firstIF.PrjPt = ARANFunctions.LocalToPrj(_FAF.PrjPt, Dir + Math.PI, Dist);

			//=========================================================================
			_firstIF.EntryDirection = Dir;
			_firstIF.OutDirection = Dir;

			_FAF.EntryDirection = Dir;

			double fTurnAngle = ARANMath.Modulus(_FAF.OutDirection - _firstIF.OutDirection, 2 * Math.PI);
			if (fTurnAngle > Math.PI)
				fTurnAngle = 2 * Math.PI - fTurnAngle;

			//=========================================================================
			double MinStabFAF = _FAF.CalcConstructionInToMinStablizationDistance(fTurnAngle);
			double MinStabIF;

			if (checkBox02_03.Checked)
				MinStabIF = _firstIF.ATT;     // _firstIF.CalcConstructionFromMinStablizationDistance(0.0);
			else
				MinStabIF = _firstIF.CalcConstructionFromMinStablizationDistance(_IFMaxAngle);

			_IFDistanceInterval.Min = MinStabFAF + MinStabIF + GlobalVars.constants.Pansops[ePANSOPSData.rnvImMinDist].Value;

			if (_IFDistance < _IFDistanceInterval.Min)
			{
				prmIFDistance = _IFDistanceInterval.Min;
				return;
			}

			//============================================================================================================
			_intermediateLeq.CreateGeometry(null, GlobalVars.CurrADHP);
			_finalLeq.CreateGeometry(_intermediateLeq, GlobalVars.CurrADHP);
			_straightMissedLeq.CreateGeometry(_finalLeq, GlobalVars.CurrADHP);
			//_straightMissedLeq.RefreshGraphics();
			//LegBase.ProcessMessages();

			int n = FillSignificanObsacles(Dir);

			double fFADescent_Min = GlobalVars.constants.Pansops[ePANSOPSData.arFADescent_Min].Value;
			double horDist = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arImHorSegLen].Value[_AirCat];
			double fPlane15MaxRange = GlobalVars.constants.Pansops[ePANSOPSData.arFIX15PlaneRang].Value;
			double fMaxIgnorGrd = GlobalVars.constants.Pansops[ePANSOPSData.arFixMaxIgnorGrd].Value;

			double moc, fAppHeight;

			double maxDist = GlobalVars.constants.Pansops[ePANSOPSData.arImRange_Max].Value;
			if (checkBox02_03.Checked)
				maxDist -= _firstIF.CalcConstructionFromMinStablizationDistance(_IFMaxAngle) + _firstIF.ATT;    //_firstIF.CalcConstructionInToMinStablizationDistance(0.0);

			double maxIFh = _FAF.ConstructAltitude - _refElevation + (maxDist - horDist) * fFADescent_Min;

			moc = GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value;
			if (!checkBox02_03.Checked)
				moc = GlobalVars.constants.Pansops[ePANSOPSData.arIASegmentMOC].Value;

			if (n >= 0)
				ixIF = -1;
			//=+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

			if (n > 0)
			{
				for (int i = 0; i < n; i++)
				{
					double dist0 = _intermediateSignObsacles.Parts[i].Dist;
					if (dist0 > maxDist + _firstIF.ATT)
						break;

					double fDist = maxDist + _firstIF.ATT - dist0;

					if (fDist < fPlane15MaxRange)
					{
						fAppHeight = maxIFh - moc * _intermediateSignObsacles.Parts[i].fSecCoeff;

						double fPlane15h = fAppHeight - fDist * fMaxIgnorGrd;
						if (fPlane15h > _intermediateSignObsacles.Parts[i].Height)
							continue;
					}

					double x = ((dist0 - _firstIF.ATT - horDist) * fFADescent_Min - moc * _intermediateSignObsacles.Parts[i].fSecCoeff - (_intermediateSignObsacles.Parts[i].Elev - _FAF.ConstructAltitude))
							/ (fMaxIgnorGrd - fFADescent_Min);

					if (x > fPlane15MaxRange)
						x = fPlane15MaxRange;

					ixIF = i;
					maxDist = Math.Min(dist0 + x - _firstIF.ATT, maxDist);
					maxIFh = _FAF.ConstructAltitude - _refElevation + (maxDist - horDist) * fFADescent_Min;
				}
				//IF_minimum Elevation = GlobalVars.constants.Pansops[ePANSOPSData.arFixMaxIgnorGrd].Value * (IF_distance - _intermediateSignObsacles.Parts[ixIF].Dist + _firstIF.ATT + GlobalVars.constants.AircraftCategory[aircraftCategoryData.arImHorSegLen].Value[_AirCat]) + _intermediateSignObsacles.Parts[i].Elev;
			}

			//=+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

			if (n >= 0 || (ixIF < 0 && _IFDistanceInterval.Max != maxDist))
				_IFDistanceInterval.Max = maxDist;

			if (_IFDistance > _IFDistanceInterval.Max)
			{
				prmIFDistance = _IFDistanceInterval.Max;
				return;
			}

			_IFAltitudeInterval.Max = prmFAFAltitude + (_IFDistance - GlobalVars.constants.AircraftCategory[aircraftCategoryData.arImHorSegLen].Value[_AirCat]) * fFADescent_Min;
			_IFAltitudeInterval.Min = prmFAFAltitude;

			if (ixIF >= 0)
				_IFAltitudeInterval.Min = fMaxIgnorGrd * (_IFDistance + _firstIF.ATT - _intermediateSignObsacles.Parts[ixIF].Dist) + _intermediateSignObsacles.Parts[ixIF].Elev + moc * _intermediateSignObsacles.Parts[ixIF].fSecCoeff;

			double hIF = _IFAltitude;
			if (hIF > _IFAltitudeInterval.Max)
				hIF = _IFAltitudeInterval.Max;

			if (hIF < _IFAltitudeInterval.Min)
				hIF = _IFAltitudeInterval.Min;

			if (Math.Abs(hIF - _IFAltitude) > ARANMath.EpsilonDistance)
			{
				prmIFAltitude = hIF;
				return;     ///???
			}

			//============================================================================================================

			_straightMissedLeq.RefreshGraphics();

			/*===============================================================*/
			moc = GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value;

			//_intermediateLeq.Obstacles = Functions.GetLegProtectionAreaObstacles(_intermediateLeq, legApch.Obstacles, moc, _refElevation);
			_intermediateLeq.Obstacles = Functions.GetLegProtectionAreaObstacles(_intermediateLeq, GlobalVars.ObstacleList, moc, _refElevation);

			//Point pt0 = ARANFunctions.LocalToPrj(_firstIF.PrjPt, _firstIF.OutDirection + Math.PI, _firstIF.ATT);

			if (checkBox02_02.Checked)
				moc = GlobalVars.constants.Pansops[ePANSOPSData.arIASegmentMOC].Value;

			double FIXheight = prmIFAltitude - _refElevation;
			//fPlane15MaxRange = GlobalVars.constants.Pansops[ePANSOPSData.arFIX15PlaneRang].Value;
			//fMaxIgnorGrd = GlobalVars.constants.Pansops[ePANSOPSData.arFixMaxIgnorGrd].Value;

			for (int i = 0; i < _intermediateLeq.Obstacles.Parts.Length; i++)
			{
				_intermediateLeq.Obstacles.Parts[i].Ignored = false;

				//ARANFunctions.PrjToLocal(pt0, _firstIF.OutDirection, _intermediateLeq.Obstacles.Parts[i].pPtPrj, out double fDist, out double y);
				double fDist = _IFDistance + _firstIF.ATT - _intermediateLeq.Obstacles.Parts[i].Dist;

				if (fDist < fPlane15MaxRange)
				{
					//fAppHeight = _IFAltitudeInterval.Min - _refElevation - moc * _intermediateLeq.Obstacles.Parts[i].fSecCoeff;
					//double hf = prmFAFAltitude + (_IFDistance - horDist) * 0.052;
					//double hf = fMaxIgnorGrd * (_IFDistance + _firstIF.ATT - _intermediateSignObsacles.Parts[0].Dist) + _intermediateSignObsacles.Parts[0].Elev;
					//fAppHeight = hf - _refElevation - moc * _intermediateLeq.Obstacles.Parts[i].fSecCoeff;

					//double minClearance = FIXheight - _refElevation - moc * _intermediateLeq.Obstacles.Parts[i].fSecCoeff- _refElevation - _intermediateLeq.Obstacles.Parts[i].Height;
					//fAppHeight = fMaxIgnorGrd * fDist;

					//if (minClearance > fAppHeight)

					fAppHeight = FIXheight - moc * _intermediateLeq.Obstacles.Parts[i].fSecCoeff;
					double fPlane15h = fAppHeight - fMaxIgnorGrd * fDist;
					if (fPlane15h - _intermediateLeq.Obstacles.Parts[i].Height > -ARANMath.EpsilonDistance)
						_intermediateLeq.Obstacles.Parts[i].Ignored = true;
				}
			}

			if (checkBox02_03.Checked)
				_reportForm.FillPage06(_intermediateLeq.Obstacles);
			else
				_reportForm.FillPage08(_intermediateLeq.Obstacles);

			/*===============================================================*/
			moc = GlobalVars.constants.Pansops[ePANSOPSData.arFASeg_FAF_MOC].Value;             //arISegmentMOC
			_finalLeq.Obstacles = Functions.GetLegProtectionAreaObstacles(_finalLeq, GlobalVars.ObstacleList, moc, _refElevation);

			//pt0 = ARANFunctions.LocalToPrj(_FAF.PrjPt, F3ApproachDir + Math.PI, _FAF.ATT);

			int k = -1;
			_FAFOCH = FFAOCA - _refElevation;

			for (int i = 0; i < _finalLeq.Obstacles.Parts.Length; i++)
			{
				double fReqH = _finalLeq.Obstacles.Parts[i].ReqH;
				double fDist = _FAFDistance + _FAF.ATT - _finalLeq.Obstacles.Parts[i].Dist;

				_finalLeq.Obstacles.Parts[i].Ignored = false;

				if (fDist < fPlane15MaxRange)
				{
					fAppHeight = prmFAFAltitude - _refElevation - fReqH;

					double fPlane15h = fAppHeight - fMaxIgnorGrd * fDist;
					if (fPlane15h > 0.0)
					{
						fReqH = 0.0;
						_finalLeq.Obstacles.Parts[i].Ignored = true;
					}
				}

				if (fReqH > _FAFOCH)
				{
					_FAFOCH = fReqH;
					k = i;
				}
			}

			comboBox02_02_SelectedIndexChanged(comboBox02_02, null);

			textBox02_07.Text = "";
			if (k >= 0)
				textBox02_07.Text = _finalLeq.Obstacles.Obstacles[_finalLeq.Obstacles.Parts[k].Owner].UnicalName;

			_reportForm.FillPage04(_finalLeq.Obstacles);
		}

		private void textBox02_08_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox02_08_Validating(textBox02_08, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox02_08.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox02_08_Validating(object sender, CancelEventArgs e)
		{
			if (!double.TryParse(textBox02_08.Text, out double fTmp))
			{
				textBox02_08.Text = GlobalVars.unitConverter.SpeedToInternalUnits(_IFSpeed).ToString();
				return;
			}

			if (textBox02_08.Tag != null && textBox02_08.Tag.ToString() == textBox02_08.Text)
				return;

			fTmp = GlobalVars.unitConverter.SpeedToInternalUnits(fTmp);

			if (fTmp < _IFIASInterval.Min)
				fTmp = _IFIASInterval.Min;

			if (fTmp > _IFIASInterval.Max)
				fTmp = _IFIASInterval.Max;

			prmIFSpeed = fTmp;
		}

		private void textBox02_09_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox02_09_Validating(textBox02_09, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox02_09.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox02_09_Validating(object sender, CancelEventArgs e)
		{
			if (!radioButton02_06.Checked)
				return;

			if (!double.TryParse(textBox02_09.Text, out double fTmp))
			{
				textBox02_09.Text = GlobalVars.unitConverter.DistanceToInternalUnits(_IFSpeed).ToString();
				return;
			}

			if (textBox02_09.Tag != null && textBox02_09.Tag.ToString() == textBox02_09.Text)
				return;

			fTmp = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			if (fTmp < _IFDistanceInterval.Min)
				fTmp = _IFDistanceInterval.Min;

			if (fTmp > _IFDistanceInterval.Max)
				fTmp = _IFDistanceInterval.Max;

			prmIFDistance = fTmp;
		}

		private void textBox02_10_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox02_10_Validating(textBox02_10, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox02_10.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox02_10_Validating(object sender, CancelEventArgs e)
		{
			if (radioButton02_07.Checked || checkBox02_02.Checked)
				return;

			double AppAzt;

			if (!double.TryParse(textBox02_10.Text, out double fTmp))
			{
				AppAzt = ARANFunctions.DirToAzimuth(_FAF.PrjPt, _IFCourse, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				textBox02_10.Text = GlobalVars.unitConverter.AzimuthToDisplayUnits(AppAzt).ToString();
				return;
			}

			if (textBox02_10.Tag != null && textBox02_10.Tag.ToString() == textBox02_10.Text)
				return;

			AppAzt = GlobalVars.unitConverter.AzimuthToInternalUnits(fTmp);
			fTmp = ARANFunctions.AztToDirection(_FAF.GeoPt, AppAzt, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);
			fTmp = IFCourseInterval.CheckValue(fTmp);

			prmIFCourse = fTmp;
		}

		private void textBox02_11_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
			{
				textBox02_11_Validating(textBox02_11, null);
				eventChar = '\0';
			}
			else if (eventChar >= ' ')// && (eventChar < '0' || eventChar > '9'))         // || eventChar != '_')
			{
				char alfa = (char)(eventChar & ~32);

				if (alfa < 'A' || alfa > 'Z')
					eventChar = '\0';
				else
					eventChar = alfa;
			}

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox02_11_Validating(object sender, CancelEventArgs e)
		{
			_firstIF.Name = textBox02_11.Text;
			_firstIF.CallSign = textBox02_11.Text;
			_firstIF.RefreshGraphics();
		}

		private void textBox02_12_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox02_12_Validating(textBox02_12, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox02_12.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox02_12_Validating(object sender, CancelEventArgs e)
		{
			if (!double.TryParse(textBox02_12.Text, out double fTmp))
			{
				textBox02_12.Text = GlobalVars.unitConverter.AngleToDisplayUnits(_IFMaxAngle).ToString();
				return;
			}

			if (textBox02_12.Tag != null && textBox02_12.Tag.ToString() == textBox02_12.Text)
				return;

			fTmp = GlobalVars.unitConverter.AngleToInternalUnits(fTmp);

			if (fTmp < _IFMaxAngleInterval.Min)
				fTmp = _IFMaxAngleInterval.Min;

			if (fTmp > _IFMaxAngleInterval.Max)
				fTmp = _IFMaxAngleInterval.Max;

			prmIFMaxAngle = fTmp;
		}

		private void textBox02_13_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox02_13_Validating(textBox02_13, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox02_13.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox02_13_Validating(object sender, CancelEventArgs e)
		{
			if (!double.TryParse(textBox02_13.Text, out double fTmp))
			{
				textBox02_13.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_IFAltitude).ToString();
				return;
			}

			if (textBox02_13.Tag != null && textBox02_13.Tag.ToString() == textBox02_13.Text)
				return;

			fTmp = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);

			if (fTmp < _IFAltitudeInterval.Min)
				fTmp = _IFAltitudeInterval.Min;

			if (fTmp > _IFAltitudeInterval.Max)
				fTmp = _IFAltitudeInterval.Max;

			prmIFAltitude = fTmp;
		}

		#endregion

		#endregion

		/*===============================================================*/
		void ToFAFSDFs()
		{
			_FinalFSDF1.EntryDirection = F3ApproachDir;
			_FinalFSDF1.OutDirection = F3ApproachDir;
			_FinalFSDF1.IAS = _FAFIAS;

			_FinalFSDF2.EntryDirection = F3ApproachDir;
			_FinalFSDF2.OutDirection = F3ApproachDir;
			_FinalFSDF1.IAS = _FAFIAS;

			if (comboBox03_02.SelectedIndex < 0)
				comboBox03_02.SelectedIndex = 0;
			else
				comboBox03_02_SelectedIndexChanged(comboBox03_02, null);

			_FinalSDFCnt = 0;
			button03_02.Enabled = false;
			button03_01.Enabled = PrepareFAFSDF1();
			if (button03_01.Enabled)
			{
				groupBox03_01.Enabled = true;
				_FinalSDFDistance = _FinalSDFAltitude = 0.0;
				radioButton03_01.Checked = true;
				prmFinalSDFAltitude = FinalSDFAltitudeInterval.Max;
			}
			else
				groupBox03_01.Enabled = false;
		}

		void BackToPageIII()
		{
			_finalSDF1Leg.DeleteGraphics();
			_finalSDF2Leg.DeleteGraphics();

			_reportForm.RemovePage02();
			_reportForm.RemovePage03();
		}

		#endregion

		#region Page 4

		void CloseFAFSDFs()
		{
			_finalLeq.Active = true;

			if (_FinalSDFCnt == 0)
			{
				_FinalFSDF = _FAF;
				_finalSDFLeg = _finalLeq;

				_FinalOCH = _FAFOCH;
				_reportForm.RemovePage02();
				_finalSDF1Leg.DeleteGraphics();
				return;
			}

			_finalLeq.NominalTrack.Remove(_finalLeq.NominalTrack.Count - 1);
			_finalLeq.NominalTrack.Add(_FinalFSDF1.PrjPt);

			_finalLeq.FullAssesmentArea = _FinalFull;
			_finalLeq.PrimaryAssesmentArea = _FinalPrimary;

			//ObstacleContainer obstacleContainer;

			_finalLeq.Obstacles = Functions.GetLegProtectionAreaObstacles(_finalLeq, GlobalVars.ObstacleList, GlobalVars.constants.Pansops[ePANSOPSData.arFASeg_FAF_MOC].Value, _refElevation);

			double fPlane15MaxRange = GlobalVars.constants.Pansops[ePANSOPSData.arFIX15PlaneRang].Value;
			double fMaxIgnorGrd = GlobalVars.constants.Pansops[ePANSOPSData.arFixMaxIgnorGrd].Value;

			//_FAFOCH = FFAOCA - _refElevation;

			for (int i = 0; i < _finalLeq.Obstacles.Parts.Length; i++)
			{
				double fReqH = _finalLeq.Obstacles.Parts[i].ReqH;
				double fDist = _FAFDistance + _FAF.ATT - _finalLeq.Obstacles.Parts[i].Dist;

				_finalLeq.Obstacles.Parts[i].Ignored = false;

				if (fDist < fPlane15MaxRange)
				{
					double fAppHeight = prmFAFAltitude - _refElevation - fReqH;
					double fPlane15h = fAppHeight - fMaxIgnorGrd * fDist;

					if (fPlane15h > 0.0)
					{
						fReqH = 0.0;
						_finalLeq.Obstacles.Parts[i].Ignored = true;
					}
				}

				//if (fReqH > _FAFOCH)
				//{
				//	_FAFOCH = fReqH;
				//	k = i;
				//}
			}

			_reportForm.FillPage04(_finalLeq.Obstacles);

			_finalSDF1Leg.Active = true;

			//_finalLeq.RefreshGraphics();
			//Application.DoEvents();
			//_finalSDF1Leg.RefreshGraphics();

			if (_FinalSDFCnt == 1)
			{
				_FinalOCH = _FAFSDF1OCH;
				_reportForm.RemovePage03();
				_finalSDF2Leg.DeleteGraphics();

				_FinalFSDF = _FinalFSDF1;
				_finalSDFLeg = _finalSDF1Leg;

				return;
			}

			if (_FinalSDFCnt == 2)
			{
				_FinalOCH = _FAFSDF2OCH;

				//=================================================================
				LineString ls = new LineString();
				Point pt0 = ARANFunctions.LocalToPrj(_FinalFSDF.PrjPt, F3ApproachDir + Math.PI, _FinalFSDF.ATT, 5000);
				Point pt1 = ARANFunctions.LocalToPrj(_FinalFSDF.PrjPt, F3ApproachDir + Math.PI, _FinalFSDF.ATT, -5000);
				ls.Add(pt0);
				ls.Add(pt1);

				GeometryOperators pTopo = new GeometryOperators();
				Geometry geomLeft, geomRight;
				MultiPolygon full, prim;

				pTopo.Cut(_FinalSDF1Full, ls, out geomLeft, out geomRight);
				full = (MultiPolygon)geomLeft;

				pTopo.Cut(_FinalSDF1Primary, ls, out geomLeft, out geomRight);
				prim = (MultiPolygon)geomLeft;

				_finalSDF1Leg.FullAssesmentArea = full;
				_finalSDF1Leg.PrimaryAssesmentArea = prim;

				//=================================================================
				LineString NominalTrack = new LineString();
				NominalTrack.Add(_FinalFSDF1.PrjPt);
				NominalTrack.Add(_FinalFSDF2.PrjPt);
				_finalSDF1Leg.NominalTrack = NominalTrack;

				_finalSDF2Leg.Active = true;

				_FinalFSDF = _FinalFSDF2;
				_finalSDFLeg = _finalSDF2Leg;

				//_finalLeq.RefreshGraphics();
				//_finalSDF1Leg.RefreshGraphics();
				//_finalSDF2Leg.RefreshGraphics();

				//==========================================

				double startA;
				if (_landingType == eLandingType.StraightIn)
					startA = prmAbvTrshld;
				else
					startA = _circlingMinOCH;

				pt0 = ARANFunctions.LocalToPrj(_FinalFSDF1.PrjPt, F3ApproachDir + Math.PI, _FinalFSDF1.ATT);

				double fAppHeight = _FinalFSDF1.ConstructAltitude - _refElevation - GlobalVars.constants.Pansops[ePANSOPSData.arFASeg_FAF_MOC].Value;      //arISegmentMOC
				double fL = fAppHeight / GlobalVars.constants.Pansops[ePANSOPSData.arFixMaxIgnorGrd].Value;
				double SDFOCH = FFAOCA - _refElevation;

				int m = FFinalObstalces.Obstacles.Length;
				int n = FFinalObstalces.Parts.Length;

				for (int j = 0; j < m; j++)
					FFinalObstalces.Obstacles[j].NIx = -1;

				//ObstacleData[] SignObsaclesParts = new ObstacleData[n];
				//Array.Copy(_intermediateLeq.Obstacles.Parts, SignObsaclesParts, n);

				_FinalFSDFObstalces = _finalSDF1Leg.Obstacles;

				Array.Resize(ref _FinalFSDFObstalces.Obstacles, m);
				Array.Resize(ref _FinalFSDFObstalces.Parts, n);

				int k = -1, l = -1, ik = -1;

				pTopo.CurrentGeometry = full;

				for (int i = 0; i < n; i++)
				{
					if (pTopo.Disjoint(FFinalObstalces.Parts[i].pPtPrj))
						continue;

					k++;
					_FinalFSDFObstalces.Parts[k] = FFinalObstalces.Parts[i];

					if (FFinalObstalces.Obstacles[FFinalObstalces.Parts[i].Owner].NIx < 0)
					{
						l++;
						_FinalFSDFObstalces.Obstacles[l] = FFinalObstalces.Obstacles[FFinalObstalces.Parts[i].Owner];
						_FinalFSDFObstalces.Obstacles[l].PartsNum = 0;
						FFinalObstalces.Obstacles[FFinalObstalces.Parts[i].Owner].NIx = l;
					}

					double fDist, fReqH = _FinalFSDFObstalces.Parts[k].ReqH;

					_FinalFSDFObstalces.Parts[k].Owner = FFinalObstalces.Obstacles[FFinalObstalces.Parts[i].Owner].NIx;
					_FinalFSDFObstalces.Parts[k].Index = _FinalFSDFObstalces.Obstacles[_FinalFSDFObstalces.Parts[k].Owner].PartsNum;  //?????????

					_FinalFSDFObstalces.Parts[k].PDG = (fReqH - startA) / _FinalFSDFObstalces.Parts[k].Dist;
					_FinalFSDFObstalces.Obstacles[_FinalFSDFObstalces.Parts[k].Owner].PartsNum++;

					//*************************************************************************************
					_FinalFSDFObstalces.Parts[k].Ignored = false;

					ARANFunctions.PrjToLocal(pt0, F3ApproachDir, _FinalFSDFObstalces.Parts[k].pPtPrj, out fDist, out double y);

					if (fDist < GlobalVars.constants.Pansops[ePANSOPSData.arFIX15PlaneRang].Value)
					{
						double fPlane15h = GlobalVars.constants.Pansops[ePANSOPSData.arFixMaxIgnorGrd].Value * (fL - fDist);
						if (fPlane15h > _FinalFSDFObstalces.Parts[k].Height)
						{
							fReqH = 0.0;
							_FinalFSDFObstalces.Parts[k].Ignored = true;
						}
					}

					if (fReqH > SDFOCH)
					{
						SDFOCH = fReqH;
						ik = k;
					}
				}

				//TimeSpan span = DateTime.Now - start;
				//MessageBox.Show("Items " + n + "\n\rTotal time " + span.TotalMilliseconds);

				Array.Resize(ref _FinalFSDFObstalces.Obstacles, l + 1);
				Array.Resize(ref _FinalFSDFObstalces.Parts, k + 1);

				_finalSDF1Leg.Obstacles = _FinalFSDFObstalces;
				_reportForm.FillPage02(_finalSDF1Leg.Obstacles);
			}
		}

		void ToIFSDFs()
		{
			_intermediateLeq.Active = true;

			_IntermSDF1.EntryDirection = prmIFCourse;
			_IntermSDF1.OutDirection = prmIFCourse;

			_IntermSDF2.EntryDirection = prmIFCourse;
			_IntermSDF2.OutDirection = prmIFCourse;

			_IntermSDF1.IAS = _IFSpeed;
			_IntermSDF2.IAS = _IFSpeed;

			_IntermSDFCnt = 1;

			groupBox04_01.Enabled = PrepareIFSDF();
			checkBox04_01.Enabled = groupBox04_01.Enabled;
			button04_01.Enabled = groupBox04_01.Enabled;

			if (comboBox04_02.SelectedIndex < 0)
				comboBox04_02.SelectedIndex = 0;
			else
				comboBox04_02_SelectedIndexChanged(comboBox04_02, null);
		}

		void BackToPageIV()
		{
			_finalLeq.Active = false;
			_finalSDF1Leg.Active = false;
			_finalSDF2Leg.Active = false;

			if (_FinalSDFCnt == 0)
			{
				_FinalFSDF = _FinalFSDF1;
				_finalSDFLeg = _finalSDF1Leg;

				_reportForm.FillPage02(_finalSDF1Leg.Obstacles);
				if (_finalSDF1Leg.Length > 0)
					_finalSDF1Leg.RefreshGraphics();
			}
			else if (_FinalSDFCnt == 1)
			{
				_FinalFSDF = _FinalFSDF2;
				_finalSDFLeg = _finalSDF2Leg;

				_reportForm.FillPage03(_finalSDF2Leg.Obstacles);

				_finalLeq.CreateGeometry(_intermediateLeq, GlobalVars.CurrADHP);
				_straightMissedLeq.CreateGeometry(_finalLeq, GlobalVars.CurrADHP);

				_finalLeq.RefreshGraphics();
				_finalSDF1Leg.RefreshGraphics();
				if (_finalSDF2Leg.Length > 0)
					_finalSDF2Leg.RefreshGraphics();

				_finalLeq.Obstacles = Functions.GetLegProtectionAreaObstacles(_finalLeq, GlobalVars.ObstacleList, GlobalVars.constants.Pansops[ePANSOPSData.arFASeg_FAF_MOC].Value, _refElevation);
				_reportForm.FillPage04(_finalLeq.Obstacles);
			}
			else if (_FinalSDFCnt == 2)
			{
				_FinalFSDF = _FinalFSDF2;
				_finalSDFLeg = _finalSDF2Leg;

				_finalLeq.CreateGeometry(_intermediateLeq, GlobalVars.CurrADHP);
				_straightMissedLeq.CreateGeometry(_finalLeq, GlobalVars.CurrADHP);

				//GlobalVars.gAranGraphics.DrawMultiPolygon(SDF1Full, eFillStyle.sfsBackwardDiagonal);
				//GlobalVars.gAranGraphics.DrawMultiPolygon(SDF1Primary, eFillStyle.sfsForwardDiagonal);
				//Application.DoEvents();
				////LegBase.ProcessMessages(true);

				_finalSDF1Leg.FullAssesmentArea = _FinalSDF1Full;
				_finalSDF1Leg.PrimaryAssesmentArea = _FinalSDF1Primary;

				_finalLeq.RefreshGraphics();
				_finalSDF1Leg.RefreshGraphics();
				_finalSDF2Leg.RefreshGraphics();
				_finalLeq.Obstacles = Functions.GetLegProtectionAreaObstacles(_finalLeq, GlobalVars.ObstacleList, GlobalVars.constants.Pansops[ePANSOPSData.arFASeg_FAF_MOC].Value, _refElevation);
				FFinalObstalces = _finalLeq.Obstacles;

				_reportForm.FillPage04(_finalLeq.Obstacles);

				//=============================================================================
				GeometryOperators pTopo = new GeometryOperators();

				double startA;
				if (_landingType == eLandingType.StraightIn)
					startA = prmAbvTrshld;
				else
					startA = _circlingMinOCH;

				Point pt0 = ARANFunctions.LocalToPrj(_FinalFSDF1.PrjPt, F3ApproachDir + Math.PI, _FinalFSDF1.ATT);

				double fAppHeight = _FinalFSDF1.ConstructAltitude - _refElevation - GlobalVars.constants.Pansops[ePANSOPSData.arFASeg_FAF_MOC].Value;      //arISegmentMOC
				double fL = fAppHeight / GlobalVars.constants.Pansops[ePANSOPSData.arFixMaxIgnorGrd].Value;
				double SDFOCH = FFAOCA - _refElevation;

				int m = FFinalObstalces.Obstacles.Length;
				int n = FFinalObstalces.Parts.Length;

				for (int j = 0; j < m; j++)
					FFinalObstalces.Obstacles[j].NIx = -1;

				//ObstacleData[] SignObsaclesParts = new ObstacleData[n];
				//Array.Copy(_intermediateLeq.Obstacles.Parts, SignObsaclesParts, n);

				_FinalFSDFObstalces = _finalSDF1Leg.Obstacles;

				Array.Resize(ref _FinalFSDFObstalces.Obstacles, m);
				Array.Resize(ref _FinalFSDFObstalces.Parts, n);

				int k = -1, l = -1, ik = -1;

				pTopo.CurrentGeometry = _finalSDF1Leg.FullAssesmentArea;

				for (int i = 0; i < n; i++)
				{
					if (pTopo.Disjoint(FFinalObstalces.Parts[i].pPtPrj))
						continue;

					k++;
					_FinalFSDFObstalces.Parts[k] = FFinalObstalces.Parts[i];

					if (FFinalObstalces.Obstacles[FFinalObstalces.Parts[i].Owner].NIx < 0)
					{
						l++;
						_FinalFSDFObstalces.Obstacles[l] = FFinalObstalces.Obstacles[FFinalObstalces.Parts[i].Owner];
						_FinalFSDFObstalces.Obstacles[l].PartsNum = 0;
						FFinalObstalces.Obstacles[FFinalObstalces.Parts[i].Owner].NIx = l;
					}

					double fReqH = _FinalFSDFObstalces.Parts[k].ReqH;

					_FinalFSDFObstalces.Parts[k].Owner = FFinalObstalces.Obstacles[FFinalObstalces.Parts[i].Owner].NIx;
					_FinalFSDFObstalces.Parts[k].Index = _FinalFSDFObstalces.Obstacles[_FinalFSDFObstalces.Parts[k].Owner].PartsNum;  //?????????

					_FinalFSDFObstalces.Parts[k].PDG = (fReqH - startA) / _FinalFSDFObstalces.Parts[k].Dist;
					_FinalFSDFObstalces.Obstacles[_FinalFSDFObstalces.Parts[k].Owner].PartsNum++;

					//*************************************************************************************
					_FinalFSDFObstalces.Parts[k].Ignored = false;

					ARANFunctions.PrjToLocal(pt0, F3ApproachDir, _FinalFSDFObstalces.Parts[k].pPtPrj, out double fDist, out double y);

					if (fDist < GlobalVars.constants.Pansops[ePANSOPSData.arFIX15PlaneRang].Value)
					{
						double fPlane15h = GlobalVars.constants.Pansops[ePANSOPSData.arFixMaxIgnorGrd].Value * (fL - fDist);
						if (fPlane15h > _FinalFSDFObstalces.Parts[k].Height)
						{
							fReqH = 0.0;
							_FinalFSDFObstalces.Parts[k].Ignored = true;
						}
					}

					if (fReqH > SDFOCH)
					{
						SDFOCH = fReqH;
						ik = k;
					}
				}

				//TimeSpan span = DateTime.Now - start;
				//MessageBox.Show("Items " + n + "\n\rTotal time " + span.TotalMilliseconds);

				Array.Resize(ref _FinalFSDFObstalces.Obstacles, l + 1);
				Array.Resize(ref _FinalFSDFObstalces.Parts, k + 1);

				_finalSDF1Leg.Obstacles = _FinalFSDFObstalces;
				_reportForm.FillPage02(_finalSDF1Leg.Obstacles);

				//=============================================================================

			}

			_IntermSDF1Leg.DeleteGraphics();
			_IntermSDF2Leg.DeleteGraphics();
			_firstIF.RefreshGraphics();
		}

		private void comboBox03_01_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox03_01.SelectedIndex < 0)
				return;

			if (!radioButton03_03.Checked)
				return;

			WPT_FIXType sigPoint = (WPT_FIXType)(comboBox03_01.SelectedItem);
			label03_02.Text = sigPoint.TypeCode.ToString(); //sigPoint.Name;

			_FinalFSDF.Name = sigPoint.Name;
			_FinalFSDF.CallSign = sigPoint.CallSign;
			_FinalFSDF.Id = sigPoint.Identifier;

			//_FinalFSDF.PrjPt = sigPoint.pPtPrj;

			//prmFSDFDistance = Hypot(sigPoint.Prj.Y - FMAPt.PrjPt.Y, sigPoint.Prj.X - FMAPt.PrjPt.X);
			prmFinalSDFDistance = Math.Abs(ARANFunctions.PointToLineDistance(FMAPt.PrjPt, sigPoint.pPtPrj, F3ApproachDir + 0.5 * Math.PI));
		}

		private void radioButton03_03_CheckedChanged(object sender, EventArgs e)
		{
			if (!((RadioButton)sender).Checked)
				return;

			bool bSDFfromList = sender == radioButton03_03;

			comboBox03_01.Enabled = bSDFfromList;
			textBox03_03.Enabled = !bSDFfromList;

			//label02_02.Enabled = bFromList;

			if (bSDFfromList)
			{
				textBox03_01.ReadOnly = true;
				textBox03_02.ReadOnly = true;

				if (comboBox03_01.SelectedIndex < 0)
					comboBox03_01.SelectedIndex = 0;
				else if (comboBox03_01.SelectedIndex >= comboBox03_01.Items.Count)
					comboBox03_01.SelectedIndex = comboBox03_01.Items.Count - 1;
				else
					comboBox03_01_SelectedIndexChanged(null, null);
			}
			else
			{
				_FinalFSDF.Id = Guid.Empty;

				if (_FinalSDFCnt == 0)
					_FinalFSDF.Name = _FinalSDF0DefNamae;
				else
					_FinalFSDF.Name = _FinalSDF1DefNamae;

				label03_02.Text = "";   //label03_02.Text = "Designated Point";

				textBox03_03.Text = _FinalFSDF.Name;
				_FinalFSDF.CallSign = textBox03_03.Text;
				//_FinalFSDF.RefreshGraphics();

				if (sender == radioButton03_01)
				{
					textBox03_01.ReadOnly = false;
					textBox03_02.ReadOnly = true;

					_FinalSDFAltitude = 0;
					textBox03_01.Tag = "";
					textBox03_01_Validating(textBox03_01, null);
				}
				else
				{
					textBox03_01.ReadOnly = true;
					textBox03_02.ReadOnly = false;

					_FinalSDFDistance = 0;
					textBox03_02.Tag = "";
					textBox03_02_Validating(textBox03_02, null);
				}
			}
		}

		private void comboBox03_02_SelectedIndexChanged(object sender, EventArgs e)
		{
			double fTmp;
			if (_FinalSDFCnt == 0)
				fTmp = _FAFSDF1OCH;
			else
				fTmp = _FAFSDF2OCH;

			if (comboBox03_02.SelectedIndex == 0)
				fTmp += _refElevation;

			double roundingFactor = 5.0 + 5.0 * GlobalVars.unitConverter.HeightUnitIndex;

			fTmp = System.Math.Round(fTmp * GlobalVars.unitConverter.HeightConverter[GlobalVars.unitConverter.HeightUnitIndex].Multiplier / roundingFactor + 0.4999) * roundingFactor;

			textBox03_04.Text = fTmp.ToString();
		}

		private void textBox03_01_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox03_01_Validating(textBox03_01, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox03_01.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox03_01_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox03_01.Text, out fTmp))
			{
				textBox03_01.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_FAFAltitude).ToString();
				return;
			}

			if (textBox03_01.Tag != null && textBox03_01.Tag.ToString() == textBox03_01.Text)
				return;

			fTmp = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);
			fTmp = FinalSDFAltitudeInterval.CheckValue(fTmp);

			//if (fTmp < FinalSDFAltitudeInterval.Min)
			//	fTmp = FinalSDFAltitudeInterval.Min;

			//if (fTmp > FinalSDFAltitudeInterval.Max)
			//	fTmp = FinalSDFAltitudeInterval.Max;

			prmFinalSDFAltitude = fTmp;
		}

		private void textBox03_02_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox03_02_Validating(textBox03_02, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox03_02.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox03_02_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox03_02.Text, out fTmp))
			{
				textBox03_02.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_FinalSDFDistance).ToString();
				return;
			}

			if (textBox03_02.Tag != null && textBox03_02.Tag.ToString() == textBox03_02.Text)
				return;

			fTmp = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			if (fTmp < FinalSDFDistanceInterval.Min)
				fTmp = FinalSDFDistanceInterval.Min;

			if (fTmp > FinalSDFDistanceInterval.Max)
				fTmp = FinalSDFDistanceInterval.Max;

			prmFinalSDFDistance = fTmp;
		}

		private void textBox03_03_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
			{
				textBox03_03_Validating(textBox03_03, null);
				eventChar = '\0';
			}
			else if (eventChar >= ' ')// && (eventChar < '0' || eventChar > '9'))         // || eventChar != '_')
			{
				char alfa = (char)(eventChar & ~32);

				if (alfa < 'A' || alfa > 'Z')
					eventChar = '\0';
				else
					eventChar = alfa;
			}

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox03_03_Validating(object sender, CancelEventArgs e)
		{
			_FinalFSDF.Name = textBox03_03.Text;
			_FinalFSDF.CallSign = textBox03_03.Text;
			_FinalFSDF.RefreshGraphics();
		}

		private void button03_01_Click(object sender, EventArgs e)
		{
			_FinalSDFCnt++;
			button03_02.Enabled = true;
			button03_01.Enabled = _FinalSDFCnt < 2;
			groupBox03_01.Enabled = true;

			if (_FinalSDFCnt == 1)
			{
				button03_01.Enabled = PrepareFAFSDF2();

				if (button03_01.Enabled)
				{
					_FinalSDFDistance = _FinalSDFAltitude = 0.0;
					radioButton03_01.Checked = true;
					prmFinalSDFAltitude = FinalSDFAltitudeInterval.Max;
				}
				else
					groupBox03_01.Enabled = false;
			}
		}

		private void button03_02_Click(object sender, EventArgs e)
		{
			_FinalSDFCnt--;
			button03_01.Enabled = true;
			button03_02.Enabled = _FinalSDFCnt > 0;
			groupBox03_01.Enabled = true;

			if (_FinalSDFCnt == 0)
			{
				_finalSDF2Leg.DeleteGraphics();
				_reportForm.RemovePage03();

				PrepareFAFSDF1();

				_FinalSDFAltitude = 0.0;
				prmFinalSDFAltitude = FinalSDFAltitudeInterval.Max;
			}
		}

		bool PrepareFAFSDF1()
		{
			_FinalFSDF = _FinalFSDF1;
			_finalSDFLeg = _finalSDF1Leg;
			textBox03_03.Text = _FinalFSDF.Name;

			FinalSDFDistanceInterval.Max = ARANFunctions.ReturnDistanceInMeters(_FAF.PrjPt, FMAPt.PrjPt) - 2.0 * 1852.0;

			double fTmp = FinalSDFDistanceInterval.Max * _FAFGradient + _refElevation;

			if (_landingType == eLandingType.StraightIn)
			{
				FinalSDFDistanceInterval.Min = (_FAFOCH - prmAbvTrshld) / _FAFGradient;
				fTmp += prmAbvTrshld;
			}
			else
			{
				FinalSDFDistanceInterval.Min = (_FAFOCH - _circlingMinOCH) / _FAFGradient;
				fTmp += _circlingMinOCH;
			}

			if (FinalSDFDistanceInterval.Min > FinalSDFDistanceInterval.Max)
				return false;

			FinalSDFAltitudeInterval.Min = _FAFOCH + _refElevation;
			FinalSDFAltitudeInterval.Max = _FAFAltitude - 2.0 * 1852.0 * _FAFGradient;
			if (FinalSDFAltitudeInterval.Max > fTmp)
				FinalSDFAltitudeInterval.Max = fTmp;

			if (FinalSDFAltitudeInterval.Min > FinalSDFAltitudeInterval.Max)
				return false;

			int n = GlobalVars.WPTList.Length;
			comboBox03_01.Items.Clear();

			for (int i = 0; i < n; i++)
			{
				Point LocalPoint = ARANFunctions.PrjToLocal(FMAPt.PrjPt, F3ApproachDir + Math.PI, GlobalVars.WPTList[i].pPtPrj);

				if (LocalPoint.X >= FinalSDFDistanceInterval.Min && LocalPoint.X < FinalSDFDistanceInterval.Max && Math.Abs(LocalPoint.Y) <= 0.25 * GlobalVars.constants.GNSS[eFIXRole.FAF_].XTT)
					comboBox03_01.Items.Add(GlobalVars.WPTList[i]);
			}

			n = _finalLeq.Obstacles.Obstacles.Length;
			FFinalObstalces.Obstacles = new Obstacle[n];
			Array.Copy(_finalLeq.Obstacles.Obstacles, FFinalObstalces.Obstacles, n);

			n = _finalLeq.Obstacles.Parts.Length;
			FFinalObstalces.Parts = new ObstacleData[n];
			Array.Copy(_finalLeq.Obstacles.Parts, FFinalObstalces.Parts, n);

			radioButton03_03.Enabled = comboBox03_01.Items.Count > 0;

			if (!radioButton03_03.Enabled || !(radioButton03_01.Checked || radioButton03_02.Checked || radioButton03_03.Checked))
				radioButton03_01.Checked = true;

			if (radioButton03_03.Enabled)
				comboBox03_01.SelectedIndex = 0;

			return true;
		}

		bool PrepareFAFSDF2()
		{
			_FinalFSDF = _FinalFSDF2;
			_finalSDFLeg = _finalSDF2Leg;
			textBox03_03.Text = _FinalFSDF.Name;

			FinalSDFDistanceInterval.Max = ARANFunctions.ReturnDistanceInMeters(_FinalFSDF1.PrjPt, FMAPt.PrjPt) - 2.0 * 1852.0;

			if (_landingType == eLandingType.StraightIn)
				FinalSDFDistanceInterval.Min = (_FAFSDF1OCH - prmAbvTrshld) / _FAFGradient;
			else
				FinalSDFDistanceInterval.Min = (_FAFSDF1OCH - _circlingMinOCH) / _FAFGradient;
			if (FinalSDFDistanceInterval.Min > FinalSDFDistanceInterval.Max)
				return false;

			FinalSDFAltitudeInterval.Min = _FAFSDF1OCH + _refElevation;
			FinalSDFAltitudeInterval.Max = _FinalFSDF1.ConstructAltitude - 2.0 * 1852.0 * _FAFGradient;
			if (FinalSDFAltitudeInterval.Min > FinalSDFAltitudeInterval.Max)
				return false;

			int n = GlobalVars.WPTList.Length;
			comboBox03_01.Items.Clear();

			for (int i = 0; i < n; i++)
			{
				Point LocalPoint = ARANFunctions.PrjToLocal(_FinalFSDF1.PrjPt, F3ApproachDir + Math.PI, GlobalVars.WPTList[i].pPtPrj);

				if (LocalPoint.X >= FinalSDFDistanceInterval.Min && LocalPoint.X < FinalSDFDistanceInterval.Max && Math.Abs(LocalPoint.Y) <= 0.25 * GlobalVars.constants.GNSS[eFIXRole.FAF_].XTT)
					comboBox03_01.Items.Add(GlobalVars.WPTList[i]);
			}

			n = _finalSDF1Leg.Obstacles.Obstacles.Length;
			FFinalObstalces.Obstacles = new Obstacle[n];
			Array.Copy(_finalSDF1Leg.Obstacles.Obstacles, FFinalObstalces.Obstacles, n);

			n = _finalSDF1Leg.Obstacles.Parts.Length;
			FFinalObstalces.Parts = new ObstacleData[n];
			Array.Copy(_finalSDF1Leg.Obstacles.Parts, FFinalObstalces.Parts, n);

			radioButton03_03.Enabled = comboBox03_01.Items.Count > 0;

			if (!radioButton03_03.Enabled || !(radioButton03_01.Checked || radioButton03_02.Checked || radioButton03_03.Checked))
				radioButton03_01.Checked = true;

			if (radioButton03_03.Enabled)
				comboBox03_01.SelectedIndex = 0;

			return true;
		}

		void ApplayFinalSDFAltitude()
		{
			_FinalFSDF.ConstructAltitude = _FinalFSDF.NomLineAltitude = prmFinalSDFAltitude;

			if (radioButton03_01.Checked)
			{
				if (_landingType == eLandingType.StraightIn)
					prmFinalSDFDistance = (prmFinalSDFAltitude - prmAbvTrshld - _refElevation) / prmFAFGradient;
				else
					prmFinalSDFDistance = (prmFinalSDFAltitude - _circlingMinOCH - _refElevation) / prmFAFGradient;
			}
		}

		void ApplayFinalSDFDist()
		{
			//if (!radioButton03_03.Checked)
			_FinalFSDF.PrjPt = ARANFunctions.LocalToPrj(_FicTHR, F3ApproachDir + Math.PI, _FinalSDFDistance);

			if (!radioButton03_01.Checked)
			{
				if (_landingType == eLandingType.StraightIn)
					prmFinalSDFAltitude = prmFinalSDFDistance * prmFAFGradient + prmAbvTrshld + _refElevation;
				else
					prmFinalSDFAltitude = prmFinalSDFDistance * prmFAFGradient + _circlingMinOCH + _refElevation;
			}

			RecreateFinalSDFGeometry();
		}

		void RecreateFinalSDFGeometry()
		{
			_FinalFSDF.ReCreateArea();

			LineString ls = new LineString();
			Point pt0 = ARANFunctions.LocalToPrj(_FinalFSDF.PrjPt, F3ApproachDir + Math.PI, _FinalFSDF.ATT, 5000);
			Point pt1 = ARANFunctions.LocalToPrj(_FinalFSDF.PrjPt, F3ApproachDir + Math.PI, _FinalFSDF.ATT, -5000);
			ls.Add(pt0);
			ls.Add(pt1);

			GeometryOperators pTopo = new GeometryOperators();
			Geometry geomLeft, geomRight;
			MultiPolygon prevFull, prevPrim;

			//GlobalVars.gAranGraphics.DrawMultiPolygon(_finalLeq.FullAssesmentArea, eFillStyle.sfsCross);
			//GlobalVars.gAranGraphics.DrawLineString(ls, 2);
			//LegBase.ProcessMessages();

			pTopo.Cut(_finalLeq.FullAssesmentArea, ls, out geomLeft, out geomRight);

			prevFull = (MultiPolygon)geomLeft;
			_FinalSDFFull = (MultiPolygon)geomRight;

			pTopo.Cut(_finalLeq.PrimaryAssesmentArea, ls, out geomLeft, out geomRight);
			prevPrim = (MultiPolygon)geomLeft;
			_FinalSDFPrimary = (MultiPolygon)geomRight;

			//_finalSDFLeg.PrimaryAssesmentArea = _FinalSDFPrimary;
			//_finalSDFLeg.FullAssesmentArea = _FinalSDFFull;

			_finalSDFLeg.PrimaryArea = _FinalSDFPrimary;
			_finalSDFLeg.FullArea = _FinalSDFFull;

			LineString NominalTrack = new LineString();
			NominalTrack.Add(_FinalFSDF.PrjPt);
			NominalTrack.Add(FMAPt.PrjPt);

			_finalSDFLeg.NominalTrack = NominalTrack;
			//==========================================
			double startA;
			if (_landingType == eLandingType.StraightIn)
				startA = prmAbvTrshld;
			else
				startA = _circlingMinOCH;

			pt0 = ARANFunctions.LocalToPrj(_FinalFSDF.PrjPt, F3ApproachDir + Math.PI, _FinalFSDF.ATT);

			double fAppHeight = _FinalSDFAltitude - _refElevation - GlobalVars.constants.Pansops[ePANSOPSData.arFASeg_FAF_MOC].Value;      //arISegmentMOC
			double fL = fAppHeight / GlobalVars.constants.Pansops[ePANSOPSData.arFixMaxIgnorGrd].Value;
			double SDFOCH = FFAOCA - _refElevation;

			int m = FFinalObstalces.Obstacles.Length;
			int n = FFinalObstalces.Parts.Length;

			for (int j = 0; j < m; j++)
				FFinalObstalces.Obstacles[j].NIx = -1;

			//ObstacleData[] SignObsaclesParts = new ObstacleData[n];
			//Array.Copy(_intermediateLeq.Obstacles.Parts, SignObsaclesParts, n);

			Array.Resize(ref _FinalFSDFObstalces.Obstacles, m);
			Array.Resize(ref _FinalFSDFObstalces.Parts, n);

			int k = -1, l = -1, ik = -1;

			pTopo.CurrentGeometry = _FinalSDFFull;

			for (int i = 0; i < n; i++)
			{
				if (pTopo.Disjoint(FFinalObstalces.Parts[i].pPtPrj))
					continue;

				k++;
				_FinalFSDFObstalces.Parts[k] = FFinalObstalces.Parts[i];

				if (FFinalObstalces.Obstacles[FFinalObstalces.Parts[i].Owner].NIx < 0)
				{
					l++;
					_FinalFSDFObstalces.Obstacles[l] = FFinalObstalces.Obstacles[FFinalObstalces.Parts[i].Owner];
					_FinalFSDFObstalces.Obstacles[l].PartsNum = 0;
					FFinalObstalces.Obstacles[FFinalObstalces.Parts[i].Owner].NIx = l;
				}

				double fReqH = _FinalFSDFObstalces.Parts[k].ReqH;

				_FinalFSDFObstalces.Parts[k].Owner = FFinalObstalces.Obstacles[FFinalObstalces.Parts[i].Owner].NIx;
				_FinalFSDFObstalces.Parts[k].Index = _FinalFSDFObstalces.Obstacles[_FinalFSDFObstalces.Parts[k].Owner].PartsNum;  //?????????

				_FinalFSDFObstalces.Parts[k].PDG = (fReqH - startA) / _FinalFSDFObstalces.Parts[k].Dist;
				_FinalFSDFObstalces.Obstacles[_FinalFSDFObstalces.Parts[k].Owner].PartsNum++;

				//*************************************************************************************
				_FinalFSDFObstalces.Parts[k].Ignored = false;

				ARANFunctions.PrjToLocal(pt0, F3ApproachDir, _FinalFSDFObstalces.Parts[k].pPtPrj, out double fDist, out double y);

				if (fDist < GlobalVars.constants.Pansops[ePANSOPSData.arFIX15PlaneRang].Value)
				{
					double fPlane15h = GlobalVars.constants.Pansops[ePANSOPSData.arFixMaxIgnorGrd].Value * (fL - fDist);
					if (fPlane15h > _FinalFSDFObstalces.Parts[k].Height)
					{
						fReqH = 0.0;
						_FinalFSDFObstalces.Parts[k].Ignored = true;
					}
				}

				if (fReqH > SDFOCH)
				{
					SDFOCH = fReqH;
					ik = k;
				}
			}

			//TimeSpan span = DateTime.Now - start;
			//MessageBox.Show("Items " + n + "\n\rTotal time " + span.TotalMilliseconds);

			Array.Resize(ref _FinalFSDFObstalces.Obstacles, l + 1);
			Array.Resize(ref _FinalFSDFObstalces.Parts, k + 1);

			//====================================================================================
			_finalSDFLeg.RefreshGraphics();

			//GlobalVars.gAranGraphics.SafeDeleteGraphic(sdfPrimaElem1);
			//GlobalVars.gAranGraphics.SafeDeleteGraphic(sdfFullElem1);

			//sdfFullElem1 = GlobalVars.gAranGraphics.DrawMultiPolygon(SDFFull);
			//sdfPrimaElem1 = GlobalVars.gAranGraphics.DrawMultiPolygon(SDFPrimary);
			//Application.DoEvents();

			if (_FinalSDFCnt == 0)
			{
				_FinalSDF1Primary = _FinalSDFPrimary;
				_FinalSDF1Full = _FinalSDFFull;

				_FinalPrimary = prevPrim;
				_FinalFull = prevFull;

				_FAFSDF1OCH = SDFOCH;
				_finalSDF1Leg.Obstacles = _FinalFSDFObstalces;
				_reportForm.FillPage02(_finalSDF1Leg.Obstacles);
			}
			else
			{
				//_FinalSDF2Primary = _FinalSDFPrimary;
				//_FinalSDF2Full = _FinalSDFFull;

				_FAFSDF2OCH = SDFOCH;
				_finalSDF2Leg.Obstacles = _FinalFSDFObstalces;
				_reportForm.FillPage03(_finalSDF2Leg.Obstacles);
			}

			comboBox03_02_SelectedIndexChanged(comboBox03_02, null);

			textBox03_05.Text = "";
			if (ik >= 0)
				textBox03_05.Text = _FinalFSDFObstalces.Obstacles[_FinalFSDFObstalces.Parts[ik].Owner].UnicalName;
		}

		#endregion

		#region Page 5
		void BackToPageV()
		{
			_IntermSDF1Leg.DeleteGraphics();
			_IntermSDF2Leg.DeleteGraphics();
		}
		private void radioButton04_01_CheckedChanged(object sender, EventArgs e)
		{
			if (!((RadioButton)sender).Checked)
				return;

			bool bIFFromList = sender == radioButton04_02;

			textBox04_01.ReadOnly = bIFFromList;
			textBox04_02.Visible = !bIFFromList;
			//comboBox04_01.Visible = bIFFromList;
			comboBox04_01.Enabled = bIFFromList;

			if (bIFFromList)
			{
				if (comboBox04_01.SelectedIndex < 0)
					comboBox04_01.SelectedIndex = 0;
				else
					comboBox04_01_SelectedIndexChanged(comboBox04_01, null);
			}
			else
			{
				label04_04.Text = "";
				_currIF.Id = Guid.Empty;
				_currIF.Name = textBox04_02.Text;
				_currIF.CallSign = textBox04_02.Text;
				_currIF.RefreshGraphics();
				//textBox04_01_Validating(textBox04_01, null);
			}
		}

		private void comboBox04_01_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox04_01.SelectedIndex < 0)
				return;

			if (!radioButton04_02.Checked)
				return;

			WPT_FIXType sigPoint = (WPT_FIXType)(comboBox04_01.SelectedItem);
			label04_04.Text = sigPoint.TypeCode.ToString(); //sigPoint.Name;

			_currIF.Name = sigPoint.Name;
			_currIF.CallSign = sigPoint.CallSign;
			_currIF.Id = sigPoint.Identifier;

			//_currIF.PrjPt = sigPoint.pPtPrj;

			prmIFSDFDistance = Math.Abs(ARANFunctions.PointToLineDistance(_prevIF.PrjPt, sigPoint.pPtPrj, F3ApproachDir + 0.5 * Math.PI));
			//_IFSDFDistance = Math.Abs(ARANFunctions.PointToLineDistance(_prevIF.PrjPt, sigPoint.pPtPrj, F3ApproachDir + 0.5 * Math.PI));
		}

		private void comboBox04_02_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox04_02.SelectedIndex < 0)
				return;

			double fTmp;
			if (_FinalSDFCnt == 0)
				fTmp = _FAFSDF1OCH;
			else
				fTmp = _FAFSDF2OCH;

			if (comboBox04_02.SelectedIndex == 0)
				fTmp += _refElevation;

			double roundingFactor = 5.0 + 5.0 * GlobalVars.unitConverter.HeightUnitIndex;

			fTmp = System.Math.Round(fTmp * GlobalVars.unitConverter.HeightConverter[GlobalVars.unitConverter.HeightUnitIndex].Multiplier / roundingFactor + 0.4999) * roundingFactor;

			textBox04_04.Text = fTmp.ToString();
		}

		private void checkBox04_01_CheckedChanged(object sender, EventArgs e)
		{
			if (!checkBox04_01.Enabled)
				return;

			//eFIXRole oldRole = _currIF.Role;
			_currIF.Role = checkBox04_01.Checked ? eFIXRole.SDF : eFIXRole.IF_;
			//if (_currIF.Role != oldRole)
			ApplayIntermedSDFDist();
		}

		private void button04_01_Click(object sender, EventArgs e)
		{
			if (!checkBox04_01.Checked)
				button04_01.Enabled = false;

			button04_02.Enabled = true;
			_IntermSDFCnt++;

			if (_IntermSDFCnt == 2)
			{
				checkBox04_01.Enabled = false;
				checkBox04_01.Checked = false;
			}

			bool res = false;
			if (_IntermSDFCnt < 3)
			{
				res = PrepareIFSDF();
				//unlock controls
				groupBox04_01.Enabled = true;
			}
			else
			{
				//lock controls
				groupBox04_01.Enabled = false;
			}

			if (button04_01.Enabled)
				button04_01.Enabled = res;
		}

		private void button04_02_Click(object sender, EventArgs e)
		{
			checkBox04_01.Enabled = true;
			_currIntermSDFLeg.DeleteGraphics();
			_IntermSDFCnt--;

			switch (_IntermSDFCnt)
			{
				case 1:
					checkBox04_01.Enabled = true;
					PrepareIFSDF();
					groupBox04_01.Enabled = true;
					break;
				case 2:
					checkBox04_01.Enabled = false;
					checkBox04_01.Checked = false;

					PrepareIFSDF();
					groupBox04_01.Enabled = true;
					break;
			}

			button04_01.Enabled = true;
			button04_02.Enabled = _IntermSDFCnt != 1;
		}

		private void textBox04_01_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox04_01_Validating(textBox04_01, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox04_01.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox04_01_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox04_01.Text, out fTmp))
			{
				textBox04_01.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_IFSDFDistance).ToString();
				return;
			}

			if (textBox04_01.Tag != null && textBox04_01.Tag.ToString() == textBox04_01.Text)
				return;

			fTmp = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			if (fTmp < _IFSDFDistanceInterval.Min)
				fTmp = _IFSDFDistanceInterval.Min;

			if (fTmp > _IFSDFDistanceInterval.Max)
				fTmp = _IFSDFDistanceInterval.Max;

			prmIFSDFDistance = fTmp;
		}

		private void textBox04_02_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
			{
				textBox04_02_Validating(textBox04_02, null);
				eventChar = '\0';
			}
			else if (eventChar >= ' ')// && (eventChar < '0' || eventChar > '9'))         // || eventChar != '_')
			{
				char alfa = (char)(eventChar & ~32);

				if (alfa < 'A' || alfa > 'Z')
					eventChar = '\0';
				else
					eventChar = alfa;
			}

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox04_02_Validating(object sender, CancelEventArgs e)
		{
			_currIF.Name = textBox04_02.Text;
			_currIF.CallSign = textBox04_02.Text;
			_currIF.RefreshGraphics();
		}

		private void textBox04_03_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox04_03_Validating(textBox04_03, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox04_03.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox04_03_Validating(object sender, CancelEventArgs e)
		{
			if (!double.TryParse(textBox04_03.Text, out double fTmp))
			{
				textBox04_03.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_IFAltitude).ToString();
				return;
			}

			if (textBox04_03.Tag != null && textBox04_03.Tag.ToString() == textBox04_03.Text)
				return;

			fTmp = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);

			if (fTmp < _IFSDFAltitudeInterval.Min)
				fTmp = _IFSDFAltitudeInterval.Min;

			if (fTmp > _IFSDFAltitudeInterval.Max)
				fTmp = _IFSDFAltitudeInterval.Max;

			prmIFSDFAltitude = fTmp;
		}

		bool PrepareIFSDF()
		{
			if (_IntermSDFCnt == 1)
			{
				_prevIF = _firstIF;
				_currIF = _IntermSDF1;
				//_IntermSDF1.Name = _InterSDF0DefNamae;

				_currIntermSDFLeg = _IntermSDF1Leg;
			}
			else if (_IntermSDFCnt == 2)
			{
				_prevIF = _IntermSDF1;
				_currIF = _IntermSDF2;
				//_IntermSDF2.Name = _InterSDF1DefNamae;
				_currIntermSDFLeg = _IntermSDF2Leg;
			}

			prmIFSDFAltitude = _prevIF.ConstructAltitude;
			textBox04_02.Text = _currIF.Name;

			//============================================================================================================================================
			double MinStabFAF = _prevIF.ATT;    //_firstIF.CalcConstructionInToMinStablizationDistance(0.0);//_firstIF.ATT 
			double MinStabIF;                   //= GlobalVars.constants.Pansops[ePANSOPSData.rnvImMinDist].Value;

			if (checkBox04_01.Checked)
			{
				_currIF.Role = eFIXRole.SDF;
				MinStabIF = _currIF.ATT;        // _IntermSDF1.CalcConstructionFromMinStablizationDistance(0.0); //
			}
			else
			{
				_currIF.Role = eFIXRole.IF_;
				MinStabIF = _currIF.CalcConstructionFromMinStablizationDistance(_IFMaxAngle);
			}

			double dFAF_SDF1 = ARANFunctions.ReturnDistanceInMeters(_FAF.PrjPt, _prevIF.PrjPt);

			_IFSDFDistanceInterval.Min = MinStabFAF + MinStabIF;
			// _firstIF.ATT + _firstIF.ATT;		+ GlobalVars.constants.Pansops[ePANSOPSData.rnvImMinDist].Value;
			//_IFSDFDistanceInterval.Min = MinStabFAF + MinStabIF + GlobalVars.constants.Pansops[ePANSOPSData.rnvImMinDist].Value;

			_IFSDFDistanceInterval.Max = GlobalVars.constants.Pansops[ePANSOPSData.arImRange_Max].Value - dFAF_SDF1;    // - 2.0 * 1852.0;

			if (checkBox04_01.Checked)
				_IFSDFDistanceInterval.Max -= _currIF.CalcConstructionFromMinStablizationDistance(_IFMaxAngle) + _currIF.ATT;   // _firstIF.CalcConstructionInToMinStablizationDistance(0.0);

			if (_IFSDFDistanceInterval.Min > _IFSDFDistanceInterval.Max)
			{
				if (_IFSDFDistanceInterval.Min - _IFSDFDistanceInterval.Max > ARANMath.EpsilonDistance)
					return false;

				_IFSDFDistanceInterval.Max = _IFSDFDistanceInterval.Min;
			}

			//============================================================================================================================================

			_IFSDFAltitudeInterval.Min = _prevIF.ConstructAltitude;
			_IFSDFAltitudeInterval.Max = _IFSDFAltitudeInterval.Min + _IFSDFDistanceInterval.Max * _FAFGradient;    // 2.0 * 1852.0 * _FAFGradient;
			if (_IFSDFAltitudeInterval.Min > _IFSDFAltitudeInterval.Max)
				return false;

			prmIFSDFAltitude = _IFSDFAltitudeInterval.Max;

			int n = GlobalVars.WPTList.Length;
			comboBox04_01.Items.Clear();

			for (int i = 0; i < n; i++)
			{
				Point LocalPoint = ARANFunctions.PrjToLocal(_prevIF.PrjPt, prmIFCourse + Math.PI, GlobalVars.WPTList[i].pPtPrj);

				if (LocalPoint.X >= _IFSDFDistanceInterval.Min && LocalPoint.X < _IFSDFDistanceInterval.Max && Math.Abs(LocalPoint.Y) <= 0.25 * GlobalVars.constants.GNSS[eFIXRole.IF_].XTT)
					comboBox04_01.Items.Add(GlobalVars.WPTList[i]);
			}

			radioButton04_02.Enabled = comboBox04_01.Items.Count > 0;
			comboBox04_01.Enabled = radioButton04_02.Enabled && radioButton04_02.Checked;

			if (!(radioButton04_01.Checked || radioButton04_02.Checked))
				radioButton04_01.Checked = true;

			if (radioButton04_02.Enabled)
				comboBox04_01.SelectedIndex = 0;

			_IFSDFDistance = 0.0;
			prmIFSDFDistance = _IFSDFDistanceInterval.Max;
			return true;
		}

		void ApplayIntermedSDFDist()
		{
			double MinStabFAF = _prevIF.ATT;       // _firstIF.CalcConstructionInToMinStablizationDistance(0.0);
			double MinStabIF;

			if (checkBox04_01.Checked)
				MinStabIF = _currIF.ATT;    // _firstIF.CalcConstructionFromMinStablizationDistance(0.0);
			else
				MinStabIF = _currIF.CalcConstructionFromMinStablizationDistance(_IFMaxAngle);

			double dFAF_SDF = ARANFunctions.ReturnDistanceInMeters(_FAF.PrjPt, _prevIF.PrjPt);

			_IFSDFDistanceInterval.Min = MinStabFAF + MinStabIF;    // + GlobalVars.constants.Pansops[ePANSOPSData.rnvImMinDist].Value;

			_IFSDFDistanceInterval.Max = GlobalVars.constants.Pansops[ePANSOPSData.arImRange_Max].Value - dFAF_SDF;        // - 2.0 * 1852.0;

			if (checkBox04_01.Checked)
				_IFSDFDistanceInterval.Max -= _currIF.CalcConstructionFromMinStablizationDistance(_IFMaxAngle) + _currIF.ATT;   // _firstIF.CalcConstructionInToMinStablizationDistance(0.0);

			if (_IFSDFDistanceInterval.Max < _IFSDFDistanceInterval.Min)
				_IFSDFDistanceInterval.Max = _IFSDFDistanceInterval.Min;

			if (_IFSDFDistance < _IFSDFDistanceInterval.Min)
			{
				prmIFSDFDistance = _IFSDFDistanceInterval.Min;
				return;
			}

			if (_IFSDFDistance > _IFSDFDistanceInterval.Max)
			{
				prmIFSDFDistance = _IFSDFDistanceInterval.Max;
				return;
			}

			//if (!radioButton04_02.Checked)
			_currIF.PrjPt = ARANFunctions.LocalToPrj(_prevIF.PrjPt, prmIFCourse + Math.PI, _IFSDFDistance);

			_currIntermSDFLeg.CreateGeometry(null, GlobalVars.CurrADHP);
			_currIntermSDFLeg.RefreshGraphics();
			//======================================================================

			double moc = GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value;

			_currIntermSDFLeg.Obstacles = Functions.GetLegProtectionAreaObstacles(_currIntermSDFLeg, GlobalVars.ObstacleList, moc, _refElevation);

			if (!checkBox04_01.Checked)
				moc = GlobalVars.constants.Pansops[ePANSOPSData.arIASegmentMOC].Value;

			double FIXheight = prmIFSDFAltitude - _refElevation;
			double fPlane15MaxRange = GlobalVars.constants.Pansops[ePANSOPSData.arFIX15PlaneRang].Value;
			double fMaxIgnorGrd = GlobalVars.constants.Pansops[ePANSOPSData.arFixMaxIgnorGrd].Value;

			double fulldist = _IFSDFDistance + _currIF.ATT;

			//Point pt0 = ARANFunctions.LocalToPrj(_currIF.PrjPt, _currIF.OutDirection + Math.PI, _currIF.ATT);

			for (int i = 0; i < _currIntermSDFLeg.Obstacles.Parts.Length; i++)
			{
				_currIntermSDFLeg.Obstacles.Parts[i].Ignored = false;

				//ARANFunctions.PrjToLocal(pt0, _currIF.OutDirection, _currIntermSDFLeg.Obstacles.Parts[i].pPtPrj, out double fDist1, out double y);
				double fDist = fulldist - _currIntermSDFLeg.Obstacles.Parts[i].Dist;

				//GlobalVars.gAranGraphics.DrawPointWithText(_IntermSDF1Leg.Obstacles.Parts[i].pPtPrj, "O-" + (i + 1));
				//Application.DoEvents();

				if (fDist < fPlane15MaxRange)
				{
					//fAppHeight = _IFAltitudeInterval.Min - _refElevation - moc * _intermediateLeq.Obstacles.Parts[i].fSecCoeff;
					//double hf = prmFAFAltitude + (_IFDistance - horDist) * 0.052;
					//double hf = fMaxIgnorGrd * (_IFDistance + _firstIF.ATT - _intermediateSignObsacles.Parts[0].Dist) + _intermediateSignObsacles.Parts[0].Elev;
					//fAppHeight = hf - _refElevation - moc * _intermediateLeq.Obstacles.Parts[i].fSecCoeff;

					//double minClearance = FIXheight - _refElevation - moc * _intermediateLeq.Obstacles.Parts[i].fSecCoeff- _refElevation - _intermediateLeq.Obstacles.Parts[i].Height;
					//fAppHeight = fMaxIgnorGrd * fDist;

					//if (minClearance > fAppHeight)

					double fAppHeight = FIXheight - moc * _currIntermSDFLeg.Obstacles.Parts[i].fSecCoeff;

					//GlobalVars.gAranGraphics.DrawPointWithText(_IntermSDF1Leg.Obstacles.Parts[i].pPtPrj, "O-" + (i + 1));
					//Application.DoEvents();

					double fPlane15h = fAppHeight - fMaxIgnorGrd * fDist;
					if (fPlane15h - _currIntermSDFLeg.Obstacles.Parts[i].Height > -ARANMath.EpsilonDistance)
						_currIntermSDFLeg.Obstacles.Parts[i].Ignored = true;
				}
			}

			if (checkBox04_01.Checked)
			{
				//if (_IntermSDFCnt == 1)
				//	_reportForm.FillPage06(_currIntermSDFLeg.Obstacles);
				//else
				_reportForm.FillPage07(_currIntermSDFLeg.Obstacles);
			}
			else
			{
				//if (_IntermSDFCnt == 1)
				//	_reportForm.RemovePage06();
				//else
				if (_IntermSDFCnt == 1)
					_reportForm.RemovePage07();

				_reportForm.FillPage08(_currIntermSDFLeg.Obstacles);
			}
		}

		void ApplayIntermedSDFAltitude()
		{
			_currIF.ConstructAltitude = _currIF.NomLineAltitude = _IntermedSDFAltitude;
		}

		#endregion

		#region Page 6

		void CreateStraightInSegment()
		{
			Point refPt, ptFar28, ptFar56, ptASWL28, ptASWR28, ptASWL56, ptASWR56, ptLE00, ptRE00, ptLE01, ptRE01, ptLE02, ptRE02, ptLE03, ptRE03, ptLE04, ptRE04, ptLE05, ptRE05;
			double Toler28, Toler56, SplayAngle15, ASW_C28, ASW_C56, fTmp;

			SplayAngle15 = GlobalVars.constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;

			//_straightMissedLeq.RefreshGraphics();
			//LegBase.ProcessMessages();

			LegApch prevleg = _finalLeq;
			if (_FinalSDFCnt > 0)
				prevleg = _finalSDFLeg;

			MultiPolygon FullPolyMAPt = prevleg.FullAssesmentArea;
			MultiPolygon PrimaryPolyMAPt = prevleg.PrimaryAssesmentArea;

			FSOC.PrjPt = ARANFunctions.LocalToPrj(FMAPt.PrjPt, F3ApproachDir, _SOCFromMAPtDistance);
			FSOC.EntryDirection = F3ApproachDir;
			FSOC.OutDirection = _MACourse;

			refPt = ARANFunctions.LocalToPrj(FMAPt.PrjPt, _MACourse + Math.PI, FMAPt.ATT);

			//GlobalVars.gAranGraphics.DrawPointWithText(refPt, "refPt");
			//LegBase.ProcessMessages();

			//SocDist = DistTPFromARP := Hypot(ptSOC.X - GlobalVars.CurrADHP.pPtPrj.X, ptSOC.Y - GlobalVars.CurrADHP.pPtPrj.Y);
			//==========================================================================================================
			WPT_FIXType fix = new WPT_FIXType();
			fix.Name = null;
			fix.pPtPrj = ARANFunctions.CircleVectorIntersect(GlobalVars.CurrADHP.pPtPrj, PANSOPSConstantList.PBNInternalTriggerDistance, FMAPt.PrjPt, _MACourse);

			FIX MApLT15NM = new FIX(eFIXRole.MAPt_, fix, GlobalVars.gAranEnv);
			MApLT15NM.IAS = GlobalVars.constants.AircraftCategory[aircraftCategoryData.Vva].Value[_AirCat];
			//MApLT15NM.IAS = GlobalVars.constants.AircraftCategory[aircraftCategoryData.ViafStar].Value[_AirCat];
			MApLT15NM.FlightPhase = eFlightPhase.MApLT28;
			MApLT15NM.PBNType = ePBNClass.RNP_APCH;

			MApLT15NM.PrjPt = ARANFunctions.LocalToPrj(MApLT15NM.PrjPt, _MACourse, -MApLT15NM.ATT, 0);
			MApLT15NM.EntryDirection = _MACourse;
			MApLT15NM.OutDirection = _MACourse;

			//fix.Name = "";
			fix.pPtPrj = ARANFunctions.CircleVectorIntersect(GlobalVars.CurrADHP.pPtPrj, PANSOPSConstantList.PBNTerminalTriggerDistance, FMAPt.PrjPt, _MACourse);

			FIX MApGE30NM = new FIX(eFIXRole.MAPt_, fix, GlobalVars.gAranEnv);
			MApGE30NM.IAS = GlobalVars.constants.AircraftCategory[aircraftCategoryData.Vva].Value[_AirCat];
			//MApGE30NM.IAS = GlobalVars.constants.AircraftCategory[aircraftCategoryData.ViafStar].Value[_AirCat];
			MApGE30NM.FlightPhase = eFlightPhase.MApGE28;
			MApGE30NM.PBNType = ePBNClass.RNP_APCH;

			MApGE30NM.PrjPt = ARANFunctions.LocalToPrj(MApGE30NM.PrjPt, _MACourse, -MApGE30NM.ATT, 0.0);
			MApGE30NM.EntryDirection = _MACourse;
			MApGE30NM.OutDirection = _MACourse;

			//==========================================================================================================
			Toler28 = MApLT15NM.ATT;
			ptFar28 = ARANFunctions.CircleVectorIntersect(GlobalVars.CurrADHP.pPtPrj, PANSOPSConstantList.PBNInternalTriggerDistance, FMAPt.PrjPt, _MACourse, out fTmp);
			ARANFunctions.LocalToPrj(ptFar28, _MACourse, -Toler28, 0, ptFar28);

			ASW_C28 = MApLT15NM.SemiWidth;
			ptASWL28 = ARANFunctions.LocalToPrj(FMAPt.PrjPt, _MACourse, 0, ASW_C28);
			ptASWR28 = ARANFunctions.LocalToPrj(FMAPt.PrjPt, _MACourse, 0, -ASW_C28);

			Toler56 = MApGE30NM.ATT;
			ptFar56 = ARANFunctions.CircleVectorIntersect(GlobalVars.CurrADHP.pPtPrj, PANSOPSConstantList.PBNTerminalTriggerDistance, FMAPt.PrjPt, _MACourse, out fTmp);
			ARANFunctions.LocalToPrj(ptFar56, _MACourse, -Toler56, 0, ptFar56);

			ASW_C56 = MApGE30NM.SemiWidth;
			ptASWL56 = ARANFunctions.LocalToPrj(FMAPt.PrjPt, _MACourse, 0, ASW_C56);
			ptASWR56 = ARANFunctions.LocalToPrj(FMAPt.PrjPt, _MACourse, 0, -ASW_C56);
			//==========================================================================================================

			//GlobalVars.gAranGraphics.DrawMultiPolygon(FullPolyMAPt, eFillStyle.sfsCross);
			//GlobalVars.gAranGraphics.DrawPointWithText(refPt, "refPt");
			//LegBase.ProcessMessages();

			ptRE00 = ARANFunctions.RingVectorIntersect(FullPolyMAPt[0].ExteriorRing, refPt, _MACourse - 0.5 * Math.PI, out fTmp, false);

			if (ptRE00 != null && fTmp > 0)
				FMAPt.ASW_R = fTmp;
			else
				ptRE00 = ARANFunctions.LocalToPrj(FMAPt.PrjPt, _MACourse + Math.PI, FMAPt.ATT, FMAPt.ASW_R);

			ptRE01 = (Point)ARANFunctions.LineLineIntersect(ptRE00, _MACourse - SplayAngle15, FSOC.PrjPt, _MACourse + 0.5 * Math.PI);
			ptRE02 = (Point)ARANFunctions.LineLineIntersect(ptRE00, _MACourse - SplayAngle15, ptASWR28, _MACourse);
			ptRE03 = (Point)ARANFunctions.LineLineIntersect(ptRE02, _MACourse, ptFar28, _MACourse + 0.5 * Math.PI);

			ptRE04 = (Point)ARANFunctions.LineLineIntersect(ptRE03, _MACourse - SplayAngle15, ptASWR56, _MACourse);
			ptRE05 = (Point)ARANFunctions.LineLineIntersect(ptRE04, _MACourse, ptFar56, _MACourse + 0.5 * Math.PI);

			//==========================================================================================================
			ptLE00 = ARANFunctions.RingVectorIntersect(FullPolyMAPt[0].ExteriorRing, refPt, _MACourse + 0.5 * Math.PI, out fTmp, false);

			//GlobalVars.gAranGraphics.DrawMultiPolygon(FullPolyMAPt, eFillStyle.sfsCross);
			//GlobalVars.gAranGraphics.DrawPointWithText(ptLE00, "ptLE00");
			//LegBase.ProcessMessages();

			if (ptLE00 != null && fTmp > 0)
				FMAPt.ASW_L = fTmp;
			else
				ptLE00 = ARANFunctions.LocalToPrj(FMAPt.PrjPt, _MACourse + Math.PI, FMAPt.ATT, -FMAPt.ASW_L);

			ptLE01 = (Point)ARANFunctions.LineLineIntersect(ptLE00, _MACourse + SplayAngle15, FSOC.PrjPt, _MACourse + 0.5 * Math.PI);
			ptLE02 = (Point)ARANFunctions.LineLineIntersect(ptLE00, _MACourse + SplayAngle15, ptASWL28, _MACourse);
			ptLE03 = (Point)ARANFunctions.LineLineIntersect(ptLE02, _MACourse, ptFar28, _MACourse + 0.5 * Math.PI);

			ptLE04 = (Point)ARANFunctions.LineLineIntersect(ptLE03, _MACourse + SplayAngle15, ptASWL56, _MACourse);
			ptLE05 = (Point)ARANFunctions.LineLineIntersect(ptLE04, _MACourse, ptFar56, _MACourse + 0.5 * Math.PI);

			Ring tpmRing = new Ring();

			tpmRing.Add(ptRE00);
			tpmRing.Add(ptRE01);
			tpmRing.Add(ptRE02);
			tpmRing.Add(ptRE03);
			tpmRing.Add(ptRE04);
			tpmRing.Add(ptRE05);

			tpmRing.Add(ptLE05);
			tpmRing.Add(ptLE04);
			tpmRing.Add(ptLE03);
			tpmRing.Add(ptLE02);
			tpmRing.Add(ptLE01);
			tpmRing.Add(ptLE00);

			Polygon tmpPoly = new Polygon();
			tmpPoly.ExteriorRing = tpmRing;

			MultiPolygon FullPoly = new MultiPolygon();
			FullPoly.Add(tmpPoly);

			//LegBase.ProcessMessages(true);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(FullPoly, eFillStyle.sfsCross);
			//LegBase.ProcessMessages();

			//==========================================================================================================
			ptLE00 = ARANFunctions.RingVectorIntersect(PrimaryPolyMAPt[0].ExteriorRing, refPt, _MACourse + 0.5 * Math.PI, out fTmp, false);

			if (ptLE00 != null && fTmp > 0)
				FMAPt.ASW_2_L = fTmp;
			else
				ptLE00 = ARANFunctions.LocalToPrj(FMAPt.PrjPt, _MACourse + Math.PI, FMAPt.ATT, -FMAPt.ASW_2_L);

			ptRE00 = ARANFunctions.RingVectorIntersect(PrimaryPolyMAPt[0].ExteriorRing, refPt, _MACourse - 0.5 * Math.PI, out fTmp, false);
			if (ptRE00 != null && fTmp > 0)
				FMAPt.ASW_2_R = fTmp;
			else
				ptRE00 = ARANFunctions.LocalToPrj(FMAPt.PrjPt, _MACourse + Math.PI, FMAPt.ATT, FMAPt.ASW_2_R);

			//LegBase.ProcessMessages(true);
			//GlobalVars.gAranGraphics.DrawPointWithText(ptLE00, "ptLE00");
			//LegBase.ProcessMessages();

			//GlobalVars.gAranGraphics.DrawPointWithText(ptRE00, "ptRE00");
			//LegBase.ProcessMessages();


			ptRE01 = ARANFunctions.LocalToPrj(ptRE02, _MACourse, 0, 0.5 * ASW_C28);
			ptRE02 = ARANFunctions.LocalToPrj(ptRE03, _MACourse, 0, 0.5 * ASW_C28);
			ptRE03 = ARANFunctions.LocalToPrj(ptRE04, _MACourse, 0, 0.5 * ASW_C56);
			ptRE04 = ARANFunctions.LocalToPrj(ptRE05, _MACourse, 0, 0.5 * ASW_C56);
			//==========================================================================================================
			//FMAPt.ReCreateArea();
			//FMAPt.RefreshGraphics();

			ptLE01 = ARANFunctions.LocalToPrj(ptLE02, _MACourse, 0, -0.5 * ASW_C28);
			ptLE02 = ARANFunctions.LocalToPrj(ptLE03, _MACourse, 0, -0.5 * ASW_C28);
			ptLE03 = ARANFunctions.LocalToPrj(ptLE04, _MACourse, 0, -0.5 * ASW_C56);
			ptLE04 = ARANFunctions.LocalToPrj(ptLE05, _MACourse, 0, -0.5 * ASW_C56);

			tpmRing = new Ring();

			tpmRing.Add(ptRE00);
			tpmRing.Add(ptRE01);
			tpmRing.Add(ptRE02);
			tpmRing.Add(ptRE03);
			tpmRing.Add(ptRE04);

			tpmRing.Add(ptLE04);
			tpmRing.Add(ptLE03);
			tpmRing.Add(ptLE02);
			tpmRing.Add(ptLE01);
			tpmRing.Add(ptLE00);

			tmpPoly = new Polygon();
			tmpPoly.ExteriorRing = tpmRing;

			MultiPolygon PrimaryPoly = new MultiPolygon();
			PrimaryPoly.Add(tmpPoly);

			//GlobalVars.gAranGraphics.DrawMultiPolygon(FullPoly);                                //, eFillStyle.sfsForwardDiagonal
			//GlobalVars.gAranGraphics.DrawMultiPolygon(PrimaryPoly);                             //, eFillStyle.sfsHorizontal

			//GlobalVars.gAranGraphics.DrawPointWithText(MApLT15NM.PrjPt, "MApLT15NM");
			//GlobalVars.gAranGraphics.DrawPointWithText(MApGE30NM.PrjPt, "MApGE30NM");
			//LegBase.ProcessMessages();

			SMASFullPolyList = FullPoly;
			SMASPrimaryPolyList = PrimaryPoly;

			//LegBase.ProcessMessages(true);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(SMASFullPolyList, eFillStyle.sfsForwardDiagonal);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(SMASPrimaryPolyList, eFillStyle.sfsHorizontal);
			//LegBase.ProcessMessages();
		}

		double CalcFAOCA(double MinOCA, ObstacleContainer obstacles, out int MaxIx)
		{
			MaxIx = -1;
			double OCH = MinOCA - _refElevation;
			int n = obstacles.Parts.Length;

			for (int i = 0; i < n; i++)
				if (!obstacles.Parts[i].Ignored && obstacles.Parts[i].ReqH > OCH)
				{
					OCH = obstacles.Parts[i].ReqH;
					MaxIx = i;
				}

			return OCH + _refElevation;
		}

		void ToStraightMAPtPage()
		{
			// MAPt ======================================================================
			//FMAPt.SensorType = eSensorType.GNSS;
			//if (bMAPtFromList )
			//	FMapt.Name = cbMAPtList.Text
			//else
			//	FMapt.Name = editMAPtName.Text;

			//if (FAligned)
			//	FMAPtInfo.Distance = 0.0;
			//else
			//	FMAPtInfo.Distance = prmAlong.Value;

			// MAPt & SOC

			//prmMAPtCourse.BeginUpdate(false);
			//prmMAPtCourse.InConvertionPoint = FMAPt.GeoPt.Clone;
			//prmMAPtCourse.OutConvertionPoint = FMAPt.PrjPt.Clone;
			//prmMAPtCourse.Value = _MACourse;
			//prmMAPtCourse.EndUpdate();

			//FMAPt.OutDirection = _MACourse;
			//FMAPt.EntryDirection = F3ApproachDir;
			//FMAPt.SOCDistance = t0 * fTAS + FMAPt.ATT;

			//prmSOCFromMAPtDistance.ChangeValue(FMapt.SOCDistance);
			//double _SOCFromMAPtDistance;
			//Interval _SOCFromMAPtDistanceInterval;

			////	FMAPt.Bank := GlobalVars.constants.Pansops[ePANSOPSData.dpT_Bank].Value;
			//FMAPt.ISA = AerodromeTemperature;
			//SetMAPt(FFarMAPt);

			//int i, n, MaxIx, MaxFAIx, MaxL, IMALenghtIx, MAHFLenghtIx;
			//======================================================
			//Branch.Clear();

			//if (_IntermSDFCnt == 2)
			//	Branch.Add(_IntermSDF2Leg);
			//if (_IntermSDFCnt >= 1)
			//	Branch.Add(_IntermSDF1Leg);

			//Branch.Add(_intermediateLeq);
			//Branch.Add(_finalLeq);

			//if (_FinalSDFCnt == 2)
			//	Branch.Add(_finalSDF2Leg);
			//if (_FinalSDFCnt >= 1)
			//	Branch.Add(_finalSDF1Leg);

			//Branch.Add(_straightMissedLeq);

			//======================================================

			MACourseInterval.Min = F3ApproachDir - ARANMath.DegToRad(15);
			MACourseInterval.Max = F3ApproachDir + ARANMath.DegToRad(15);

			_MACourse = F3ApproachDir;
			prmMACourse = F3ApproachDir;

			//_MACourse = MACourseInterval.Max;
			//prmMACourse = MACourseInterval.Max;

			_MAClimbInterval.Min = 0.02;
			_MAClimbInterval.Max = 0.07;
			_MAClimb = 0.025;           //prmMAClimb = 0.1;

			textBox05_09.Text = GlobalVars.unitConverter.GradientToDisplayUnits(_MAClimb).ToString();
			textBox05_09.Tag = textBox05_09.Text;

			double fTAS = ARANMath.IASToTAS(GlobalVars.constants.AircraftCategory[aircraftCategoryData.VfafMax].Value[_AirCat],
			GlobalVars.CurrADHP.Elev, /*GlobalVars.CurrADHP.ISAtC -*/GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value) +
			GlobalVars.constants.Pansops[ePANSOPSData.arNearTerrWindSp].Value;

			double t0 = GlobalVars.constants.Pansops[ePANSOPSData.arSOCdelayTime].Value + GlobalVars.constants.Pansops[ePANSOPSData.dpPilotTolerance].Value;

			prmSOCFromMAPtDistance = t0 * fTAS + FMAPt.ATT;

			if (!FAligned)
			{
				double r = ARANMath.BankToRadius(GlobalVars.constants.Pansops[ePANSOPSData.arBankAngle].Value, fTAS);
				double turnAngle = F3ApproachDir - FRWYDirection.M;
				if (turnAngle < 0) turnAngle = turnAngle + 2 * Math.PI;
				if (turnAngle > Math.PI) turnAngle = 2 * Math.PI - turnAngle;

				double maxTurnAngle = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMaxInterAngle].Value[_AirCat];
				if (_AirCat <= aircraftCategory.acB && turnAngle < GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMaxInterAngle].Value[acC])
					maxTurnAngle = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMaxInterAngle].Value[acC];

				FFAAligOCA = _refElevation + prmAbvTrshld +
					(_AlongValue + r * Math.Tan(0.5 * maxTurnAngle) + 5.0 * fTAS) * GlobalVars.constants.Pansops[ePANSOPSData.arFADescent_Nom].Value;
			}
			else
				FFAAligOCA = GlobalVars.constants.Pansops[ePANSOPSData.arFASeg_FAF_MOC].Value + _refElevation;

			//======================================================================
			// prmFAFDistance;
			// prevleg.Length - prevleg.StartFIX.ATT - prevleg.EndFIX.ATT;
			// ARANFunctions.PointToLineDistance(prevleg.StartFIX.PrjPt, FMAPt.PrjPt, F3ApproachDir + 0.5 * Math.PI);

			//radioButton05_01.Text = Resources.str06002;

			LegApch prevleg = _finalLeq;
			if (_FinalSDFCnt > 0)
			{
				prevleg = _finalSDFLeg;
				//radioButton05_01.Text = Resources.str05002;
			}

			Point refPt = prevleg.StartFIX.PrjPt;

			MAPtDistanceInterval.Max = ARANFunctions.PointToLineDistance(refPt, FRWYDirection, F3ApproachDir + 0.5 * Math.PI);// - prevleg.StartFIX.ATT;// - prevleg.EndFIX.ATT;
			MAPtDistanceInterval.Min = 2.0 * 1852.0;                //GlobalVars.constants.Pansops[ePANSOPSData.arMinRangeFAS].Value;//

			//	MinDist = GlobalVars.constants.Pansops[ePANSOPSData.arMinRangeFAS].Value;
			//	MaxDist = GlobalVars.constants.Pansops[ePANSOPSData.arMaxRangeFAS].Value;

			comboBox05_01.Items.Clear();
			int n = GlobalVars.WPTList.Length;

			for (int i = 0; i < n; i++)
			{
				WPT_FIXType sigPoint = GlobalVars.WPTList[i];
				ARANFunctions.PrjToLocal(refPt, F3ApproachDir, sigPoint.pPtPrj, out double x, out double y);
				if (x >= MAPtDistanceInterval.Min && x <= MAPtDistanceInterval.Max && Math.Abs(y) <= 0.25 * FMAPt.XTT)
				{
					comboBox05_01.Items.Add(sigPoint);
					//GlobalVars.gAranGraphics.DrawPointWithText(sigPoint.pPtPrj, "pt-" + i);
					//LegBase.ProcessMessages();
				}
			}

			radioButton05_02.Enabled = comboBox05_01.Items.Count > 0;

			//_MAPtDistance = ARANFunctions.PointToLineDistance(refPt, FMAPt.PrjPt, F3ApproachDir + 0.5 * Math.PI);// prmMAPtDistance
			_MAPtDistance = MAPtDistanceInterval.CheckValue(ARANFunctions.PointToLineDistance(refPt, FMAPt.PrjPt, F3ApproachDir + 0.5 * Math.PI));// prmMAPtDistance

			//_MAPtDistance = prmFAFDistance;
			prmMAPtDistance = _MAPtDistance;
			//======================================================================

			FFAMAPtPosOCA = prevleg.StartFIX.ConstructAltitude - prmMAPtDistance * prmFAFGradient;

			prmOCA = CalcFAOCA(Math.Max(FFAAligOCA, FFAMAPtPosOCA), prevleg.Obstacles, out int MaxFAIx);

			int MaxL = MaxFAIx;

			FMAPt.ConstructAltitude = FMAPt.NomLineAltitude = prmOCA;

			if (MaxFAIx >= 0)
				textBox05_10.Text = prevleg.Obstacles.Obstacles[prevleg.Obstacles.Parts[MaxFAIx].Owner].UnicalName;
			else if (FFAAligOCA > FFAMAPtPosOCA)
				textBox05_10.Text = "Track alignment.";
			else
				textBox05_10.Text = "MAPt position.";

			//=================================================================

			//FMAMinOCA = Functions.GetMAObstAndMinOCA(_straightMissedLeq, FSOC, FSOC.EntryDirection, _MAClimb, _OCH, out int MaxIx, 1);
			//FIMALenght = 0;
			//if (FMAHFLenght < 0) FMAHFLenght = 0;

			FMAHFLenght = (GlobalVars.constants.Pansops[ePANSOPSData.enrMOC].Value - _OCH) / _MAClimb;

			prmMAHFFromMAPtDistInterval.Min = FMAHFLenght + FMAPt.SOCDistance;
			prmMAHFFromMAPtDistInterval.Max = PANSOPSConstantList.PBNTerminalTriggerDistance;
			_MAHFFromMAPtDist = prmMAHFFromMAPtDistInterval.Max;

			textBox05_13.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_MAHFFromMAPtDist).ToString();
			textBox05_13.Tag = textBox05_13.Text;

			FMAHFstr.SensorType = radioButton05_03.Checked ? eSensorType.GNSS : eSensorType.DME_DME;
			FMAHFstr.PrjPt = ARANFunctions.LocalToPrj(FMAPt.PrjPt, _MACourse, _MAHFFromMAPtDist);

			//FMAHF.EntryDirection = FMAPt.EntryDirection;
			//FMAHF.OutDirection = FMAPt.EntryDirection;

			FMAHFstr.ConstructAltitude = FMAHFstr.NomLineAltitude = _OCA + (_MAHFFromMAPtDist - FMAPt.SOCDistance) * _MAClimb;
			textBox05_14.Text = GlobalVars.unitConverter.HeightToDisplayUnits(FMAHFstr.ConstructAltitude).ToString();

			//=================================================================
			FSOC.PrjPt = ARANFunctions.LocalToPrj(FMAPt.PrjPt, F3ApproachDir, _SOCFromMAPtDistance);
			FSOC.EntryDirection = F3ApproachDir;
			FSOC.OutDirection = _MACourse;

			_straightMissedLeq.CreateGeometry(prevleg, GlobalVars.CurrADHP);
			SMASFullPolyList = _straightMissedLeq.FullAssesmentArea;            // new MultiPolygon();
			SMASPrimaryPolyList = _straightMissedLeq.PrimaryAssesmentArea;      // new MultiPolygon();

			//CreateStraightInSegment();

			//FMAFullPolyGr = GlobalVars.gAranGraphics.DrawMultiPolygon(SMASFullPolyList, eFillStyle.sfsBackwardDiagonal, ARANFunctions.RGB(0, 255, 0));
			//FMAPrimPolyGr = GlobalVars.gAranGraphics.DrawMultiPolygon(SMASPrimaryPolyList, eFillStyle.sfsForwardDiagonal, ARANFunctions.RGB(255, 0, 0));
			//LegBase.ProcessMessages();

			//_straightMissedLeq.PrimaryAssesmentArea = SMASPrimaryPolyList;
			//_straightMissedLeq.FullAssesmentArea = SMASFullPolyList;

			FMAPt.StraightArea = SMASPrimaryPolyList;
			//FMAPt.RefreshGraphics();

			_straightMissedLeq.RefreshGraphics();

			//==============================================================================================
			//LegBase prevLeg = _finalLeq;
			//if (_FinalSDFCnt > 0)
			//	prevLeg = _finalSDFLeg;

			//_straightMissedLeq.CreateGeometry(_finalLeq, GlobalVars.CurrADHP);
			//_straightMissedLeq.RefreshGraphics();
			//LegBase.ProcessMessages();

			//GlobalVars.gAranGraphics.SafeDeleteGraphic(FMAFullPolyGr);
			//GlobalVars.gAranGraphics.SafeDeleteGraphic(FMAPrimPolyGr);
			//FMAPt.DeleteGraphics();

			//FMAFullPolyGr = GlobalVars.gAranGraphics.DrawMultiPolygon(FullPoly, eFillStyle.sfsHollow ,ARANFunctions.RGB(0, 255, 0));
			//FMAPrimPolyGr = GlobalVars.gAranGraphics.DrawMultiPolygon(PrimaryPoly,  eFillStyle.sfsHollow, ARANFunctions.RGB(255, 0, 0));
			//==============================================================================================

			_straightMissedLeq.Obstacles = Functions.GetLegProtectionAreaObstacles(_straightMissedLeq, GlobalVars.ObstacleList, GlobalVars.constants.Pansops[ePANSOPSData.arMA_InterMOC].Value, _refElevation);  //MOC =30 m

			//==============================================================================

			if (comboBox05_01.Items.Count > 0)
			{
				comboBox05_01.SelectedIndex = 0;
				if (radioButton05_01.Checked)
					ApplayMAPtDistance();
			}
			else
			{
				if (!radioButton05_01.Checked)
					radioButton05_01.Checked = true;
				else
					radioButton05_01_CheckedChanged(radioButton05_01, null);

				ApplayMAPtDistance();
			}
		}

		void ApplyMAClimb()
		{
			FMATF.AppliedGradient = _MAClimb;

			_straightMissedLeq.Obstacles = Functions.GetLegProtectionAreaObstacles(_straightMissedLeq, GlobalVars.ObstacleList, GlobalVars.constants.Pansops[ePANSOPSData.arMA_InterMOC].Value, _refElevation);  //MOC =30 m

			FMAMinOCH = Functions.GetMAObstAndMinOCH(_straightMissedLeq, FSOC, _MACourse, _MAClimb, _OCH, out int MaxIx, 1);

			FMAHFLenght = (GlobalVars.constants.Pansops[ePANSOPSData.enrMOC].Value - FMAMinOCH) / _MAClimb;
			if (FMAHFLenght < 0)
				FMAHFLenght = 0;

			//ObstacleContainer ObstacleList = _straightMissedLeq.Obstacles;

			FIMALenght = 0;

			double fTmp;
			int IMALenghtIx = -1;
			int MAHFLenghtIx = -1;
			int n = _straightMissedLeq.Obstacles.Parts.Length;

			for (int i = 0; i < n; i++)
			{
				fTmp = (_straightMissedLeq.Obstacles.Parts[i].Height + GlobalVars.constants.Pansops[ePANSOPSData.arMA_FinalMOC].Value * _straightMissedLeq.Obstacles.Parts[i].fSecCoeff - FMAMinOCH) / _MAClimb;
				if (fTmp > _straightMissedLeq.Obstacles.Parts[i].Dist && fTmp > FIMALenght)
				{
					FIMALenght = fTmp;
					IMALenghtIx = i;
				}

				fTmp = (_straightMissedLeq.Obstacles.Parts[i].Height + GlobalVars.constants.Pansops[ePANSOPSData.enrMOC].Value * _straightMissedLeq.Obstacles.Parts[i].fSecCoeff - FMAMinOCH) / _MAClimb;
				if (FMAHFLenght < fTmp)
				{
					FMAHFLenght = fTmp;
					MAHFLenghtIx = i;
				}
			}

			//============================================

			textBox05_07.Text = GlobalVars.unitConverter.HeightToDisplayUnits(FMAMinOCH + _refElevation).ToString();
			textBox05_08.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(FIMALenght).ToString();
			textBox05_15.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(FMAHFLenght + FMAPt.SOCDistance).ToString();

			prmMAHFFromMAPtDistInterval.Min = FMAHFLenght + FMAPt.SOCDistance;
			prmMAHFFromMAPtDistInterval.Max = PANSOPSConstantList.PBNTerminalTriggerDistance;

			if (_MAHFFromMAPtDist < prmMAHFFromMAPtDistInterval.Min)
			{
				prmMAHFFromMAPtDist = prmMAHFFromMAPtDistInterval.Min;
				return;
			}

			textBox05_11.Text = "";
			textBox05_12.Text = "";
			textBox05_16.Text = "";

			if (MaxIx >= 0)
			{
				_straightMissedLeq.Obstacles.Parts[MaxIx].Flags = _straightMissedLeq.Obstacles.Parts[MaxIx].Flags | 2;
				textBox05_11.Text = _straightMissedLeq.Obstacles.Obstacles[_straightMissedLeq.Obstacles.Parts[MaxIx].Owner].UnicalName;
			}

			if (IMALenghtIx >= 0)
			{
				_straightMissedLeq.Obstacles.Parts[IMALenghtIx].Flags = _straightMissedLeq.Obstacles.Parts[IMALenghtIx].Flags | 4;
				textBox05_12.Text = _straightMissedLeq.Obstacles.Obstacles[_straightMissedLeq.Obstacles.Parts[IMALenghtIx].Owner].UnicalName;
			}

			if (MAHFLenghtIx >= 0)
			{
				_straightMissedLeq.Obstacles.Parts[MAHFLenghtIx].Flags = _straightMissedLeq.Obstacles.Parts[MAHFLenghtIx].Flags | 8;
				textBox05_16.Text = _straightMissedLeq.Obstacles.Obstacles[_straightMissedLeq.Obstacles.Parts[MAHFLenghtIx].Owner].UnicalName;
			}

			_reportForm.AddMissedApproach(_straightMissedLeq.Obstacles);

			fTmp = FMAMinOCH + FMAHFLenght * _MAClimb;
			textBox05_17.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fTmp).ToString();
		}

		void ApplayMAPtDistance()
		{
			LegBase prevLeg = _intermediateLeq;
			LegBase Leg = _finalLeq;
			if (_FinalSDFCnt > 0)
			{
				prevLeg = _FinalSDFCnt == 1 ? _finalLeq : _finalSDF1Leg;
				Leg = _finalSDFLeg;
			}

			Point refPt = Leg.StartFIX.PrjPt;
			//if (radioButton05_02.Checked && comboBox05_01.SelectedIndex >= 0)
			//	FMAPt.PrjPt.Assign(((WPT_FIXType)comboBox05_01.SelectedItem).pPtPrj);
			//else

			FMAPt.PrjPt = ARANFunctions.LocalToPrj(refPt, F3ApproachDir, _MAPtDistance);

			// MAPT Shifting ======================================================================

			double moc = GlobalVars.constants.Pansops[ePANSOPSData.arFASeg_FAF_MOC].Value;
			double distance = ARANFunctions.ReturnDistanceInMeters(refPt, FRWYDirection);

			Leg.EndFIX.Assign(FMAPt);
			Leg.CreateGeometry(prevLeg, GlobalVars.CurrADHP);

			// MAPT Shifting ===============================================================
			_straightMissedLeq.CreateGeometry(Leg, GlobalVars.CurrADHP);

			SMASPrimaryPolyList = _straightMissedLeq.PrimaryAssesmentArea;
			SMASFullPolyList = _straightMissedLeq.FullAssesmentArea;

			//CreateStraightInSegment();

			//_straightMissedLeq.PrimaryAssesmentArea = SMASPrimaryPolyList;
			//_straightMissedLeq.FullAssesmentArea = SMASFullPolyList;
			//FMAPt.StraightArea = SMASPrimaryPolyList;			//????

			// MAPT Shifting ===============================================================

			Leg.Obstacles = Functions.GetLegProtectionAreaObstacles(Leg, GlobalVars.ObstacleList, moc, _refElevation);  //MOC =30 m

			//===================================================================================
			if (_FinalSDFCnt == 2)
				moc = GlobalVars.constants.Pansops[ePANSOPSData.arFASeg_FAF_MOC].Value;

			double fPlane15MaxRange = GlobalVars.constants.Pansops[ePANSOPSData.arFIX15PlaneRang].Value;
			double fMaxIgnorGrd = GlobalVars.constants.Pansops[ePANSOPSData.arFixMaxIgnorGrd].Value;

			int n = Leg.Obstacles.Parts.Length;

			for (int i = 0; i < n; i++)
			{
				double fReqH = Leg.Obstacles.Parts[i].ReqH;
				double fDist = distance + _FAF.ATT - Leg.Obstacles.Parts[i].Dist;

				Leg.Obstacles.Parts[i].Ignored = false;

				if (fDist < fPlane15MaxRange)
				{
					double fAppHeight = prmFAFAltitude - _refElevation - fReqH;

					double fPlane15h = fAppHeight - fMaxIgnorGrd * fDist;
					if (fPlane15h > 0.0)
					{
						fReqH = 0.0;
						Leg.Obstacles.Parts[i].Ignored = true;
					}
				}
			}

			if (_FinalSDFCnt == 0)
				_reportForm.FillPage04(Leg.Obstacles);
			else if (_FinalSDFCnt == 1)
				_reportForm.FillPage02(_finalSDF1Leg.Obstacles);
			else
				_reportForm.FillPage03(_finalSDF2Leg.Obstacles);

			//==============================================================================
			//FFAMAPtPosOCA = prmFAFAltitude - prmMAPtDistance * prmFAFGradient;
			FFAMAPtPosOCA = Leg.StartFIX.ConstructAltitude - prmMAPtDistance * prmFAFGradient;

			prmOCA = CalcFAOCA(Math.Max(FFAAligOCA, FFAMAPtPosOCA), Leg.Obstacles, out int MaxFAIx);
			int MaxL = MaxFAIx;

			FMAPt.NomLineAltitude = FMAPt.ConstructAltitude = prmOCA;

			if (MaxFAIx >= 0)
				textBox05_10.Text = Leg.Obstacles.Obstacles[Leg.Obstacles.Parts[MaxFAIx].Owner].UnicalName;
			else if (FFAAligOCA > FFAMAPtPosOCA)
				textBox05_10.Text = "Track alignment.";
			else
				textBox05_10.Text = "MAPt position.";

			// MAPT Shifting ===============================================================

			_straightMissedLeq.RefreshGraphics();

			//==============================================================================
			ApplyMAClimb();
		}

		void ApplayMACourse()
		{
			FMAPt.OutDirection = _MACourse;
			FMAHFstr.EntryDirection = _MACourse;
			FMAHFstr.OutDirection = _MACourse;

			ApplyMAHFFromMAPtDist();
		}

		void ApplyMAHFFromMAPtDist()
		{
			//if (checkBox05_01.Checked)			return;

			FMAHFstr.PrjPt = ARANFunctions.LocalToPrj(FMAPt.PrjPt, _MACourse, _MAHFFromMAPtDist);

			//FMAPt.OutDirection = _MACourse;
			//FMAHFstr.EntryDirection = _MACourse;
			//FMAHFstr.OutDirection = _MACourse;
			FMAHFstr.SensorType = radioButton05_03.Checked ? eSensorType.GNSS : eSensorType.DME_DME;

			FMAHFstr.ConstructAltitude = FMAHFstr.NomLineAltitude = FMAMinOCH + (_MAHFFromMAPtDist - FMAPt.SOCDistance) * _MAClimb + _refElevation;

			textBox05_14.Text = GlobalVars.unitConverter.HeightToDisplayUnits(FMAHFstr.ConstructAltitude).ToString();

			double TriggerDistance = PANSOPSConstantList.PBNTerminalTriggerDistance;

			double DistTPFromARP = ARANMath.Hypot(FMAHFstr.PrjPt.X - GlobalVars.CurrADHP.pPtPrj.X, FMAHFstr.PrjPt.Y - GlobalVars.CurrADHP.pPtPrj.Y);

			if (DistTPFromARP >= TriggerDistance) FMAHFstr.Role = eFIXRole.MAHF_GT_56;
			else FMAHFstr.Role = eFIXRole.MAHF_LE_56;

			//CreateMATIASegment(FMAHF);
			LegApch prevleg = _finalLeq;
			if (_FinalSDFCnt > 0)
				prevleg = _finalSDFLeg;

			//_straightMissedLeq.DeleteGraphics();
			_straightMissedLeq.CreateGeometry(prevleg, GlobalVars.CurrADHP);

			//LegBase.ProcessMessages();
			//_straightMissedLeq.RefreshGraphics();
			//LegBase.ProcessMessages();

			SMASPrimaryPolyList = _straightMissedLeq.PrimaryAssesmentArea;
			SMASFullPolyList = _straightMissedLeq.FullAssesmentArea;

			//CreateStraightInSegment();

			//_straightMissedLeq.PrimaryAssesmentArea = SMASPrimaryPolyList;
			//_straightMissedLeq.FullAssesmentArea = SMASFullPolyList;
			//FMAPt.StraightArea = SMASPrimaryPolyList;		//????????

			_straightMissedLeq.RefreshGraphics();
			//LegBase.ProcessMessages();

			FMAPt.StraightArea = _straightMissedLeq.PrimaryAssesmentArea;// SMATPrimaryPolyList;
			FMATF.ReCreateArea();

			ApplyMAClimb();
		}

		private void comboBox05_01_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox05_01.SelectedIndex < 0 || !radioButton05_02.Checked)
				return;

			WPT_FIXType sigPoint = (WPT_FIXType)comboBox05_01.SelectedItem;

			LegApch prevleg = _finalLeq;
			if (_FinalSDFCnt > 0)
				prevleg = _finalSDFLeg;


			double Dist = ARANFunctions.PointToLineDistance(prevleg.StartFIX.PrjPt, sigPoint.pPtPrj, F3ApproachDir + 0.5 * Math.PI);

			prmMAPtDistance = Dist;
		}

		private void comboBox05_02_SelectedIndexChanged(object sender, EventArgs e)
		{
			//FMAPt.PBNType = (ePBNClass)(radioButton05_03.Checked ? comboBox05_02.SelectedIndex : comboBox05_02.SelectedIndex + 1);
			//FMAPt.RefreshGraphics();
		}

		private void checkBox05_01_CheckedChanged(object sender, EventArgs e)
		{
			bool goToNextPage = checkBox05_01.Checked;

			OkBtn.Enabled = !goToNextPage;
			NextBtn.Enabled = goToNextPage;

			textBox05_13.Enabled = !goToNextPage;
			label05_21.Enabled = !goToNextPage;
			label05_22.Enabled = !goToNextPage;

			textBox05_14.Enabled = !goToNextPage;
			label05_23.Enabled = !goToNextPage;
			label05_24.Enabled = !goToNextPage;

			//??????????????????????????????????????????????????????????????
			if (!goToNextPage)
			{
				prmMAHFFromMAPtDist = _MAHFFromMAPtDist;
				ApplyMAHFFromMAPtDist();
			}
			else
			{
				//_reportForm.ClearAddMIAS_TIA(List, "MA straight segment till MAHF");
				FMAHFstr.DeleteGraphics();
				prmMAPtDistance = _MAPtDistance;
				ApplayMAPtDistance();
			}
		}

		private void checkBox05_02_CheckedChanged(object sender, EventArgs e)
		{
			FMAPt.MultiCoverage = checkBox05_02.Checked;

			if (radioButton05_04.Checked)
				ApplayMAPtDistance();
			//FMAPt.RefreshGraphics();
		}

		private void checkBox05_03_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox05_03.Checked)
			{
				if (radioButton05_02.Checked)
				{
					if (ARANMath.SubtractAngles(prmMACourse, F3ApproachDir) > ARANMath.EpsilonRadian)
						radioButton05_01.Checked = true;
				}
				prmMACourse = F3ApproachDir;
			}

			textBox05_02.ReadOnly = radioButton05_02.Checked || checkBox05_03.Checked;
		}

		private void radioButton05_01_CheckedChanged(object sender, EventArgs e)
		{
			if (!((RadioButton)sender).Checked)
				return;

			bool bMAPtFromList = sender == radioButton05_02;

			textBox05_01.ReadOnly = bMAPtFromList;
			textBox05_03.Visible = !bMAPtFromList;
			checkBox05_03.Enabled = !bMAPtFromList;
			//textBox05_03.Enabled = !bMAPtFromList;

			comboBox05_01.Enabled = bMAPtFromList;

			if (bMAPtFromList)
				comboBox05_01_SelectedIndexChanged(comboBox05_01, null);
			else
				textBox05_01_Validating(textBox05_01, null);
		}

		private void radioButton05_03_CheckedChanged(object sender, EventArgs e)
		{
			if (!((RadioButton)sender).Checked)
				return;

			//int prevType = 0;
			//if (comboBox05_02.SelectedIndex >= 0)
			//	prevType = radioButton05_03.Checked ? comboBox05_02.SelectedIndex + 1 : comboBox05_02.SelectedIndex;

			//comboBox05_02.Items.Clear();

			if (radioButton05_03.Checked)
			{
				FMAPt.SensorType = eSensorType.GNSS;
				checkBox05_02.Enabled = false;
				//comboBox05_02.Items.Add("RNP APCH");
				//comboBox05_02.Items.Add("RNAV 1");
				//comboBox05_02.Items.Add("RNAV 2");
			}
			else
			{
				FMAPt.SensorType = eSensorType.DME_DME;
				checkBox05_02.Enabled = true;
				//comboBox05_02.Items.Add("RNAV 1");
				//comboBox05_02.Items.Add("RNAV 2");
				//prevType--;
				//if (prevType < 0)
				//	prevType = 0;
			}

			FMAHFstr.SensorType = FMAPt.SensorType;
			//comboBox05_02.SelectedIndex = prevType;
			ApplayMAPtDistance();
		}

		private void textBox05_01_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox05_01_Validating(textBox05_01, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox05_01.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox05_01_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox05_01.Text, out fTmp))
			{
				textBox05_01.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_MAPtDistance).ToString();
				return;
			}

			if (textBox05_01.Tag != null && textBox05_01.Tag.ToString() == textBox05_01.Text)
				return;

			fTmp = MAPtDistanceInterval.Max - GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			if (fTmp < MAPtDistanceInterval.Min)
				fTmp = MAPtDistanceInterval.Min;

			if (fTmp > MAPtDistanceInterval.Max)
				fTmp = MAPtDistanceInterval.Max;

			prmMAPtDistance = fTmp;
		}

		private void textBox05_02_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox05_02_Validating(textBox05_02, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox05_02.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox05_02_Validating(object sender, CancelEventArgs e)
		{
			double AppAzt;
			double fTmp;

			if (!double.TryParse(textBox05_02.Text, out fTmp))
			{
				AppAzt = ARANFunctions.DirToAzimuth(FMAPt.PrjPt, _MACourse, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				textBox05_02.Text = GlobalVars.unitConverter.AzimuthToDisplayUnits(AppAzt).ToString();
				return;
			}

			if (textBox05_02.Tag != null && textBox05_02.Tag.ToString() == textBox05_02.Text)
				return;

			AppAzt = GlobalVars.unitConverter.AzimuthToInternalUnits(fTmp);
			fTmp = ARANFunctions.AztToDirection(FMAPt.GeoPt, AppAzt, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);
			fTmp = MACourseInterval.CheckValue(fTmp);
			prmMACourse = fTmp;
		}

		private void textBox05_03_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
			{
				textBox05_03_Validating(textBox05_03, null);
				eventChar = '\0';
			}
			else if (eventChar >= ' ')// && (eventChar < '0' || eventChar > '9'))         // || eventChar != '_')
			{
				char alfa = (char)(eventChar & ~32);

				if (alfa < 'A' || alfa > 'Z')
					eventChar = '\0';
				else
					eventChar = alfa;
			}

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox05_03_Validating(object sender, CancelEventArgs e)
		{
			FMAPt.Name = textBox05_03.Text;
			FMAPt.CallSign = textBox05_03.Text;
			FMAPt.RefreshGraphics();
		}

		private void textBox05_09_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox05_09_Validating(textBox05_09, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox05_09.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox05_09_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox05_09.Text, out fTmp))
			{
				textBox05_09.Text = GlobalVars.unitConverter.GradientToDisplayUnits(_MAClimb).ToString();
				return;
			}

			if (textBox05_09.Tag != null && textBox05_09.Tag.ToString() == textBox05_09.Text)
				return;

			fTmp = GlobalVars.unitConverter.GradientToInternalUnits(fTmp);

			if (fTmp < _MAClimbInterval.Min)
				fTmp = _MAClimbInterval.Min;

			if (fTmp > _MAClimbInterval.Max)
				fTmp = _MAClimbInterval.Max;

			prmMAClimb = fTmp;
		}

		private void textBox05_13_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox05_13_Validating(textBox05_13, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox05_13.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox05_13_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox05_13.Text, out fTmp))
			{
				textBox05_13.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_MAHFFromMAPtDist).ToString();
				return;
			}

			if (textBox05_13.Tag != null && textBox05_13.Tag.ToString() == textBox05_13.Text)
				return;

			fTmp = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			if (fTmp < prmMAHFFromMAPtDistInterval.Min)
				fTmp = prmMAHFFromMAPtDistInterval.Min;

			if (fTmp > prmMAHFFromMAPtDistInterval.Max)
				fTmp = prmMAHFFromMAPtDistInterval.Max;

			prmMAHFFromMAPtDist = fTmp;
		}

		#endregion

		#region Page 7
		void ToTurnMissedApproachPage()
		{
			_straightMissedLeq.DeleteGraphics();

			if (Math.Abs(ARANMath.Modulus(_MACourse, 2 * Math.PI) - ARANMath.Modulus(F3ApproachDir, 2 * Math.PI)) > ARANMath.EpsilonRadian && comboBox06_01.Items.Count == 3)
			{
				int prev = comboBox06_01.SelectedIndex - 1;
				if (prev < 0)
					prev = 0;

				comboBox06_01.Items.Clear();
				comboBox06_01.Items.Add(eTurnAt.Altitude);
				comboBox06_01.Items.Add(eTurnAt.TP);
				bFormInitialised = false;
				comboBox06_01.SelectedIndex = prev;
				bFormInitialised = true;
			}

			comboBox06_04.Enabled = (eTurnAt)comboBox06_01.SelectedItem == eTurnAt.TP;

			if ((eTurnAt)comboBox06_01.SelectedItem <= eTurnAt.Altitude)
				comboBox06_04.SelectedIndex = 1;

			prmMATBankAngleInterval.Min = GlobalVars.constants.Pansops[ePANSOPSData.arMABankAngle].Value;
			prmMATBankAngleInterval.Max = GlobalVars.constants.Pansops[ePANSOPSData.arBankAngle].Value;

			_MATBankAngle = prmMATBankAngleInterval.Min;
			prmMATBankAngle = prmMATBankAngleInterval.Min;

			FMATF.TurnAt = (eTurnAt)comboBox06_01.SelectedItem;
			FMATF.TurnDirection = (TurnDirection)(1 - 2 * comboBox06_02.SelectedIndex);

			FMATF.StraightInPrimaryPolygon = SMASPrimaryPolyList;
			FMATF.StraightInSecondaryPolygon = SMASFullPolyList;


			FMATF.BankAngle = prmMATBankAngle;
			FMATF.EntryDirection = _MACourse;// FMAPt.OutDirection;	//
			FMATF.SensorType = FMAPt.SensorType;


			prmPreOCA = prmOCA;

			prmMAIASInterval.Min = GlobalVars.constants.AircraftCategory[aircraftCategoryData.VmaInter].Value[_AirCat];
			prmMAIASInterval.Max = GlobalVars.constants.AircraftCategory[aircraftCategoryData.VmaFaf].Value[_AirCat];
			//prmMAIAS = prmMAIASInterval.Max;

			_MAIAS = FMATF.IAS = prmMAIASInterval.Max;
			textBox06_02.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_MAIAS).ToString();
			textBox06_02.Tag = textBox06_02.Text;

			prmTurnFromMAPtDistInterval.Min = FMAPt.SOCDistance + ((eTurnAt)comboBox06_01.SelectedItem == eTurnAt.TP ? FMATF.ATT : 0);
			prmTurnFromMAPtDistInterval.Max = 80000.0;

			_TurnFromMAPtDist = prmTurnFromMAPtDistInterval.Min;
			textBox06_03.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_TurnFromMAPtDist).ToString();
			textBox06_03.Tag = textBox06_03.Text;

			FMATF.PrjPt = ARANFunctions.LocalToPrj(FMAPt.PrjPt, _MACourse, _TurnFromMAPtDist);
			FMATF.PrjPtH = FMATF.PrjPt;

			//prmTurnFromSOCDist.PerformCheck(comboBox06_01.SelectedItem == eTurnAt.TP);
			//prmTurnFromMAPtDist.Active = comboBox06_01.SelectedItem == eTurnAt.TP;

			prmTurnFromMAPtDistReadOnly = (eTurnAt)comboBox06_01.SelectedItem < eTurnAt.TP;
			///prmTurnFromMAPtDist.ShowMinMaxHint = prmTurnFromMAPtDist.Active;

			//prmPreTurnAltitude.Active = false;

			//////prmPreTurnAltitudeInterval.Min = FMAMinOCA + FMAHFLenght * _MAClimb;
			//////prmPreTurnAltitudeInterval.Max = FMAMinOCA + FMSA;
			//////prmPreTurnAltitude = fTmp;

			prmPreTurnAltitudeInterval.Min = prmOCA;
			prmPreTurnAltitudeInterval.Max = prmOCA + 2000;

			_preTurnAltitude = prmOCA;
			prmPreTurnAltitude = _preTurnAltitude;
			//textBox06_04.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_preTurnAltitude).ToString();
			//textBox06_04.Tag = textBox06_04.Text;
			//prmPreTurnAltitude = prmOCA;

			//prmPreTurnAltitude.Active = comboBox06_01.SelectedItem  == eTurnAt.Altitude;
			prmPreTurnAltitudeReadOnly = (eTurnAt)comboBox06_01.SelectedItem != eTurnAt.Altitude;
			//prmPreTurnAltitude.ShowMinMaxHint = prmPreTurnAltitude.Active;

			//prmMAIAS.Active = false;
			//_MAIAS = prmMAIASInterval.Max;
			//textBox06_02.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_MAIAS).ToString();
			//textBox06_02.Tag = textBox06_02.Text;
			//prmMAIAS = prmMAIASInterval.Max;
			//prmMAIAS.Active = true;

			//MinMATurnDist = FIMALenght +
			//prmNWPTFromTPDist.Active = false;

			//double fMin = Math.Max(MinMATurnDist, FMATF.CalcConstructionFromMinStablizationDistance((eFlyPath)comboBox06_05.SelectedItem == eFlyPath.DirectToFIX));

			double plannedTurnAngle = ARANMath.DegToRad((double)numericUpDown06_01.Value);

			double MinStabPrev = FMATF.CalcConstructionFromMinStablizationDistance((eFlyPath)comboBox06_05.SelectedItem == eFlyPath.DirectToFIX);
			double MinStabCurr = FMAHFturn.CalcNomLineInToMinStablizationDistance(plannedTurnAngle);

			double mindist = MinStabPrev + MinStabCurr;
			double fMin = Math.Max(MinMATurnDist, mindist);

			prmNWPTFromTPDistInterval.Min = fMin;
			prmNWPTFromTPDistInterval.Max = MaxMATurnDist;

			_NWPTFromTPDist = MaxMATurnDist; // MinMATurnDist;
			textBox06_10.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_NWPTFromTPDist).ToString();
			textBox06_10.Tag = textBox06_10.Text;
			//prmNWPTFromTPDist = MinMATurnDist;
			//prmNWPTFromTPDist.Active = true;

			prmNWPTFromTPDistReadOnly = radioButton06_03.Checked;

			//double delta = ARANMath.DegToRad(240 - 120 * comboBox06_05.SelectedIndex);
			double delta = ARANMath.DegToRad(240 - 120 * ((eFlyPath)comboBox06_05.SelectedItem == eFlyPath.DirectToFIX ? 0 : 1));

			if ((eTurnAt)comboBox06_01.SelectedItem == eTurnAt.TP && comboBox06_04.SelectedIndex == 0)
			{
				double fTAS = ARANMath.IASToTAS(_MAIAS, _preTurnAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value) + GlobalVars.constants.Pansops[ePANSOPSData.arNearTerrWindSp].Value;
				double limit = 2 * Math.Atan((prmTurnFromMAPtDist - FMATF.ATT - FMAPt.SOCDistance) / FMATF.CalcTurnRadius(fTAS));
				if (delta > limit) delta = limit;
			}

			prmTurnCourseReadOnly = radioButton06_03.Checked;

			prmTurnCourseInterval.Min = _MACourse - comboBox06_02.SelectedIndex * delta;
			prmTurnCourseInterval.Max = _MACourse + (1 - comboBox06_02.SelectedIndex) * delta;
			prmTurnCourse = _MACourse;

			//prmMATBankAngle = GlobalVars.constants.Pansops[ePANSOPSData.arMABankAngle].Value;
			ApplyMATBankAngle();

			//	rbExistingPoint.Checked = true;
			//	FMAPtLeg := TLeg.Create(FMApt, FMATF, fpIntermediateMissedApproach);
		}

		int FillMATurnPoints()
		{
			Guid prevPoint = Guid.Empty;

			if (comboBox06_06.SelectedIndex >= 0)
				prevPoint = ((WPT_FIXType)comboBox06_06.SelectedItem).Identifier;

			//double fTurn = 2 * comboBox06_02.SelectedIndex - 1;         //Right = -1;	Left = 1
			double fTurn = 1 - 2 * comboBox06_02.SelectedIndex;     //Right = -1;	Left = 1

			double delta = ARANMath.DegToRad(240 - 120 * ((eFlyPath)comboBox06_05.SelectedItem == eFlyPath.DirectToFIX ? 0 : 1));
			double fTAS = ARANMath.IASToTAS(_MAIAS, _preTurnAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value) + GlobalVars.constants.Pansops[ePANSOPSData.arNearTerrWindSp].Value;

			if ((eTurnAt)comboBox06_01.SelectedItem == eTurnAt.TP && comboBox06_04.SelectedIndex == 0)
			{
				double limit = 2 * Math.Atan((prmTurnFromMAPtDist - FMATF.ATT - FMAPt.SOCDistance) / FMATF.CalcTurnRadius(fTAS));
				if (delta > limit) delta = limit;
			}

			int j = 0;
			comboBox06_06.Items.Clear();
			//labelTurnWPTType.Text = "";

			foreach (WPT_FIXType sigPoint in GlobalVars.WPTList)
			{
				double dX = sigPoint.pPtPrj.X - FMATF.PrjPt.X;
				double dY = sigPoint.pPtPrj.Y - FMATF.PrjPt.Y;
				double fDist = ARANMath.Hypot(dX, dY);
				if (fDist < prmNWPTFromTPDistInterval.Min || fDist > prmNWPTFromTPDistInterval.Max)
					continue;

				double fTurnAngle = ARANMath.Modulus((Math.Atan2(dY, dX) - _MACourse) * fTurn, ARANMath.C_2xPI);
				if (fTurnAngle > delta)
					continue;

				double fMinDist;

				if (comboBox06_04.SelectedIndex == 0 || (eFlyPath)comboBox06_05.SelectedItem == eFlyPath.DirectToFIX)
					fMinDist = MinMATurnDist;
				else
					//fMinDist = FMATF.CalcConstructionFromMinStablizationDistance(fTurnAngle);
					fMinDist = FMATF._CalcFromMinStablizationDistance(fTurnAngle, fTAS, (eFlyPath)comboBox06_05.SelectedItem == eFlyPath.DirectToFIX);

				if (fMinDist > fDist)
					continue;

				if (sigPoint.Identifier == prevPoint)
					j = comboBox06_06.Items.Count;

				comboBox06_06.Items.Add(sigPoint);
			}

			int result = comboBox06_06.Items.Count;
			if (result > 0)
				comboBox06_06.SelectedIndex = j;

			return result;
		}

		void ApplyPreTurnAltitude()
		{
			FMATF.ConstructAltitude = FMATF.NomLineAltitude = _preTurnAltitude;
			DoTurnOn();
		}

		void ApplyMATBankAngle()
		{
			FMATF.BankAngle = prmMATBankAngle;

			if (comboBox06_01.SelectedIndex < 0)
				comboBox06_01.SelectedIndex = 0;
			else
				comboBox06_01_SelectedIndexChanged(comboBox06_01, null);
		}

		void ApplyMAIAS()
		{
			FMATF.IAS = _MAIAS;
			if (comboBox06_01.SelectedIndex < 0)
				comboBox06_01.SelectedIndex = 0;
			else
				comboBox06_01_SelectedIndexChanged(comboBox06_01, null);
		}

		void ApplyTurnFromMAPtDist()
		{
			CalcCourseInterval();
			DoTurnOn();
		}

		void ApplyNWPTFromTPDist()
		{
			ReCreateMATurnArea();
		}

		void ApplyTurnCourse()
		{
			FMATF.OutDirection = _TurnCourse;

			//	if FMATF.SensorType = stGNSS then		TriggerDistance := GNSSTriggerDistance
			//	else									TriggerDistance := SBASTriggerDistance + GNSSTriggerDistance;
			//	TriggerDistance := PBNInternalTriggerDistance;

			double DistTPFromARP = ARANMath.Hypot(FMATF.PrjPt.X - GlobalVars.CurrADHP.pPtPrj.X, FMATF.PrjPt.Y - GlobalVars.CurrADHP.pPtPrj.Y);

			if (DistTPFromARP >= PANSOPSConstantList.PBNTerminalTriggerDistance) FMATF.Role = eFIXRole.MATF_GT_56;
			else if (DistTPFromARP >= PANSOPSConstantList.PBNInternalTriggerDistance) FMATF.Role = eFIXRole.PBN_MATF_GE_28;
			else FMATF.Role = eFIXRole.PBN_MATF_LT_28;

			//==========================================================================
			//if (comboBox06_04.SelectedIndex = 0) or (comboBox06_05.SelectedIndex = 0) then
			//	prmNWPTFromTPDist.Min = MinMATurnDist
			//else
			//{
			double fTurn = 1 - 2 * comboBox06_02.SelectedIndex;      //Right = -1;	Left = 1
			double fTurnAngle = ARANMath.Modulus((prmTurnCourse - _MACourse) * fTurn, ARANMath.C_2xPI);

			double fTAS = ARANMath.IASToTAS(_MAIAS, _preTurnAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value) + GlobalVars.constants.Pansops[ePANSOPSData.arNearTerrWindSp].Value;
			double MinStabPrev = FMATF._CalcFromMinStablizationDistance(fTurnAngle, fTAS, (eFlyPath)comboBox06_05.SelectedItem == eFlyPath.DirectToFIX);

			//double MinStabPrev = FMATF.CalcConstructionFromMinStablizationDistance((eFlyPath)comboBox06_05.SelectedItem == eFlyPath.DirectToFIX);
			double plannedTurnAngle = ARANMath.DegToRad((double)numericUpDown06_01.Value);
			double MinStabCurr = FMAHFturn.CalcNomLineInToMinStablizationDistance(plannedTurnAngle);

			double fMinDist = MinStabPrev + MinStabCurr;
			//double fMin = Math.Max(MinMATurnDist, mindist);

			prmNWPTFromTPDistInterval.Min = Math.Max(MinMATurnDist, fMinDist);

			double fTmp = prmNWPTFromTPDistInterval.CheckValue(_NWPTFromTPDist);
			if (_NWPTFromTPDist != fTmp)
				prmNWPTFromTPDist = fTmp;
			else
				ReCreateMATurnArea();
			//}

			//==========================================================================

			//ReCreateMATurnArea();
		}

		//====================================================

		void CalcCourseInterval()
		{
			FMATF.TurnDirection = (TurnDirection)(1 - 2 * comboBox06_02.SelectedIndex);

			double fTAS = ARANMath.IASToTAS(_MAIAS, _preTurnAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value) + GlobalVars.constants.Pansops[ePANSOPSData.arNearTerrWindSp].Value;
			double delta = ARANMath.DegToRad(240 - 120 * ((eFlyPath)comboBox06_05.SelectedItem == eFlyPath.DirectToFIX ? 0 : 1));
			if ((eTurnAt)comboBox06_01.SelectedItem == eTurnAt.TP && comboBox06_04.SelectedIndex == 0)
			{
				double limit = 2 * Math.Atan((prmTurnFromMAPtDist - FMATF.ATT - FMAPt.SOCDistance) / FMATF.CalcTurnRadius(fTAS));
				if (delta > limit) delta = limit;
			}

			prmTurnCourseInterval.Min = _MACourse - comboBox06_02.SelectedIndex * delta;
			prmTurnCourseInterval.Max = _MACourse + (1 - comboBox06_02.SelectedIndex) * delta;

			int n = FillMATurnPoints();
			radioButton06_03.Enabled = radioButton06_04.Enabled = n > 0;

			bool bTst = radioButton06_04.Checked;
			if (n == 0)
				radioButton06_04.Checked = true;

			if (bTst)
				ApplyTurnCourse();
		}


		void DoTurnOn()
		{
			if ((eTurnAt)comboBox06_01.SelectedItem < eTurnAt.TP)
			{
				//double LatestDist;

				if ((eTurnAt)comboBox06_01.SelectedItem == eTurnAt.MApt)
					_TurnFromMAPtDist = FMAPt.SOCDistance;
				else
					_TurnFromMAPtDist = FMAPt.SOCDistance + (prmPreTurnAltitude - prmOCA) / _MAClimb;

				//prmTurnFromMAPtDist = _TurnFromMAPtDist;
				//_TurnFromMAPtDist = LatestDist;
				textBox06_03.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_TurnFromMAPtDist).ToString();
				textBox06_03.Tag = textBox06_03.Text;
			}

			//FMATF.StraightInPrimaryPolygon = SMASPrimaryPolyList;
			//FMATF.StraightInSecondaryPolygon = SMASFullPolyList;

			//FMATF.StraightInPrimaryPolygon = SMATPrimaryPolyList;
			//FMATF.StraightInSecondaryPolygon = SMATFullPolyList;

			FMATF.TurnDistance = _TurnFromMAPtDist;
			FMATF.maPt = FMAPt;
			FMATF.PrjPt = ARANFunctions.LocalToPrj(FMAPt.PrjPt, _MACourse, _TurnFromMAPtDist);

			FMATF.TurnAt = (eTurnAt)comboBox06_01.SelectedItem;
			FMATF.FlyPath = (eFlyPath)comboBox06_05.SelectedItem;
			//FMATF.SensorType = FMAPt.SensorType;		//TSensorType(rgMAPtSensorType.SelectedIndex);
			FMATF.IAS = prmMAIAS;
			FMATF.NomLineAltitude = FMATF.ConstructAltitude = prmPreTurnAltitude;

			//	if FMATF.SensorType = stGNSS then		TriggerDistance := GNSSTriggerDistance
			//	else									TriggerDistance := SBASTriggerDistance + GNSSTriggerDistance;
			//	TriggerDistance := PBNInternalTriggerDistance;

			double DistTPFromARP = ARANMath.Hypot(FMATF.PrjPt.X - GlobalVars.CurrADHP.pPtPrj.X, FMATF.PrjPt.Y - GlobalVars.CurrADHP.pPtPrj.Y);

			if (DistTPFromARP >= PANSOPSConstantList.PBNTerminalTriggerDistance) FMATF.Role = eFIXRole.MATF_GT_56;
			else if (DistTPFromARP >= PANSOPSConstantList.PBNInternalTriggerDistance) FMATF.Role = eFIXRole.PBN_MATF_GE_28;
			else FMATF.Role = eFIXRole.PBN_MATF_LT_28;


			//==============================================================================
			CreateMATIASegment(FMATF);
#if ttt
			GlobalVars.gAranGraphics.SafeDeleteGraphic(FMAFullPolyGr);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(FMAPrimPolyGr);

			FMAFullPolyGr = GlobalVars.gAranGraphics.DrawMultiPolygon(SMATFullPolyList, eFillStyle.sfsHollow, ARANFunctions.RGB(0, 255, 0));
			FMAPrimPolyGr = GlobalVars.gAranGraphics.DrawMultiPolygon(SMATPrimaryPolyList, eFillStyle.sfsHollow, ARANFunctions.RGB(255, 0, 0));
#endif
			FMAPt.StraightArea = SMATPrimaryPolyList;
			FMATF.ReCreateArea();

			FMAPt.RefreshGraphics();
			FMATF.RefreshGraphics();

			int n = FillMATurnPoints();
			radioButton06_03.Enabled = radioButton06_04.Enabled = n > 0;

			bool bTst = radioButton06_04.Checked;
			if (n == 0)
				radioButton06_04.Checked = true;

			if (bTst)
				ApplyTurnCourse();
		}

		//		void TurnOnTP()
		//		{
		//			FMATF.TurnDistance = _TurnFromMAPtDist;
		//			FMATF.maPt = FMAPt;
		//			FMATF.PrjPt = ARANFunctions.LocalToPrj(FMAPt.PrjPt, _MACourse, _TurnFromMAPtDist);

		//			FMATF.TurnAt = (eTurnAt)comboBox06_01.SelectedItem;
		//			FMATF.FlyPath = (eFlyPath)comboBox06_05.SelectedItem;
		//			//FMATF.SensorType = FMAPt.SensorType;		//TSensorType(rgMAPtSensorType.SelectedIndex);
		//			FMATF.IAS = prmMAIAS;

		//			FMATF.NomLineAltitude = FMATF.ConstructAltitude = prmPreTurnAltitude;

		//			//	if FMATF.SensorType = stGNSS then		TriggerDistance := GNSSTriggerDistance
		//			//	else									TriggerDistance := SBASTriggerDistance + GNSSTriggerDistance;
		//			//	TriggerDistance := PBNTerminalTriggerDistance;

		//			double DistTPFromARP = ARANMath.Hypot(FMATF.PrjPt.X - GlobalVars.CurrADHP.pPtPrj.X, FMATF.PrjPt.Y - GlobalVars.CurrADHP.pPtPrj.Y);

		//			if (DistTPFromARP >= PANSOPSConstantList.PBNTerminalTriggerDistance) FMATF.Role = eFIXRole.MATF_GT_56;
		//			else if (DistTPFromARP >= PANSOPSConstantList.PBNInternalTriggerDistance) FMATF.Role = eFIXRole.PBN_MATF_GE_28;
		//			else FMATF.Role = eFIXRole.PBN_MATF_LT_28;

		//			//==============================================================================
		//			CreateMATIASegment(FMATF);
		//#if ttt
		//			GlobalVars.gAranGraphics.SafeDeleteGraphic(FMAFullPolyGr);
		//			GlobalVars.gAranGraphics.SafeDeleteGraphic(FMAPrimPolyGr);

		//			FMAFullPolyGr = GlobalVars.gAranGraphics.DrawMultiPolygon(SMATFullPolyList, eFillStyle.sfsHollow, ARANFunctions.RGB(0, 255, 0));
		//			FMAPrimPolyGr = GlobalVars.gAranGraphics.DrawMultiPolygon(SMATPrimaryPolyList, eFillStyle.sfsHollow, ARANFunctions.RGB(255, 0, 0));
		//#endif
		//			//
		//			FMAPt.StraightArea = SMATPrimaryPolyList;
		//			FMATF.ReCreateArea();

		//			FMAPt.RefreshGraphics();
		//			FMATF.RefreshGraphics();

		//			int n = FillMATurnPoints();
		//			radioButton06_03.Enabled = radioButton06_04.Enabled = n > 0;

		//			bool bTst = radioButton06_04.Checked;
		//			if (n == 0)
		//				radioButton06_04.Checked = true;

		//			if (bTst)
		//				ApplyTurnCourse();
		//		}

		void CreateMATIASegment(MATF matf)
		{
			Point ptTmp, ptFar, ptEnd, ptLE01, ptRE01, ptLE02, ptRE02, ptLE03, ptRE03, ptLE04, ptRE04;

			double DistTurn, fTmp;

			double SplayAngle15 = GlobalVars.constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
			double TanSplayAngle15 = Math.Tan(SplayAngle15);

			double ASW_C28 = WayPoint.WPTParams[(int)ePBNClass.RNP_APCH, (int)eFlightPhase.MApLT28].SemiWidth;
			double ASW_C56 = WayPoint.WPTParams[(int)ePBNClass.RNP_APCH, (int)eFlightPhase.MApGE28].SemiWidth;

			double LPT = (matf.FlyMode != eFlyMode.Flyby ? 1 : -1) * matf.LPT;

			LegBase Leg = _finalLeq;
			if (_FinalSDFCnt > 0)
				Leg = _finalSDFLeg;

			MultiPolygon FullPolyMAPt = Leg.FullAssesmentArea;
			MultiPolygon PrimaryPolyMAPt = Leg.PrimaryAssesmentArea;

			//GlobalVars.gAranGraphics.DrawMultiPolygon(FullPolyMAPt, eFillStyle.sfsBackwardDiagonal);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(PrimaryPolyMAPt, eFillStyle.sfsCross);
			//LegBase.ProcessMessages(true);

			FSOC.PrjPt = ARANFunctions.LocalToPrj(FMAPt.PrjPt, F3ApproachDir, prmSOCFromMAPtDistance, 0);
			//FSOC.EntryDirection = F3ApproachDir;
			FSOC.OutDirection = _MACourse;
			//GlobalVars.gAranGraphics.DrawPointWithText (FSOC.PrjPt, "FSOC");
			//LegBase.ProcessMessages();

			Point ptASWL28 = ARANFunctions.LocalToPrj(FMAPt.PrjPt, _MACourse, 0, ASW_C28);
			Point ptASWR28 = ARANFunctions.LocalToPrj(FMAPt.PrjPt, _MACourse, 0, -ASW_C28);

			Point ptASWL56 = ARANFunctions.LocalToPrj(FMAPt.PrjPt, _MACourse, 0, ASW_C56);
			Point ptASWR56 = ARANFunctions.LocalToPrj(FMAPt.PrjPt, _MACourse, 0, -ASW_C56);

			Point refPt = ARANFunctions.LocalToPrj(FMAPt.PrjPt, F3ApproachDir, -FMAPt.ATT, 0);

			//GlobalVars.gAranGraphics.DrawPointWithText (refPt, "refPt");
			//LegBase.ProcessMessages();

			//==========================================================================================================
			//Point ptRE00 = ARANFunctions.RingVectorIntersect(FullPolyMAPt[0].ExteriorRing, refPt, F3ApproachDir - 0.5 * Math.PI, out fTmp, true);
			//FMAPt.ASW_R = fTmp;
			Point ptRE00 = ARANFunctions.RingVectorIntersect(FullPolyMAPt[0].ExteriorRing, refPt, F3ApproachDir - 0.5 * Math.PI, out fTmp, true);

			if (ptRE00 != null && fTmp > 0)
				FMAPt.ASW_R = fTmp;
			else
				ptRE00 = ARANFunctions.LocalToPrj(refPt, F3ApproachDir + Math.PI, 0, FMAPt.ASW_R);

			//GlobalVars.gAranGraphics.DrawPointWithText (ptRE00, "ptRE00-1");
			//GlobalVars.gAranGraphics.DrawPointWithText (FMAPt.PrjPt, "FMAPt");
			//LegBase.ProcessMessages();

			Point ptLE00 = ARANFunctions.RingVectorIntersect(FullPolyMAPt[0].ExteriorRing, refPt, F3ApproachDir + 0.5 * Math.PI, out fTmp, true);
			if (ptLE00 != null && fTmp > 0)
				FMAPt.ASW_L = fTmp;
			else
				ptLE00 = ARANFunctions.LocalToPrj(refPt, F3ApproachDir, 0, FMAPt.ASW_L);

			//GlobalVars.gAranGraphics.DrawPointWithText (ptLE00, "ptLE00-1");
			//LegBase.ProcessMessages();

			fTmp = Math.Max(FMAPt.ASW_R, FMAPt.ASW_L);
			double DistFullWidth28 = (ASW_C28 - fTmp) / TanSplayAngle15;
			double DistFullWidth56 = (ASW_C56 - ASW_C28) / TanSplayAngle15;
			double SOCWidth = fTmp + (prmSOCFromMAPtDistance + FMAPt.ATT) * TanSplayAngle15;
			double DistTP = ARANMath.Hypot(matf.PrjPt.X - refPt.X, matf.PrjPt.Y - refPt.Y);

			bool SpecialCase = ((eTurnAt)comboBox06_01.SelectedItem == eTurnAt.TP) && ((SOCWidth > matf.SemiWidth) || (DistFullWidth28 > DistTP - matf.EPT));

			ptRE02 = ptLE02 = ptRE03 = ptLE03 = ptRE04 = ptLE04 = null;
			MultiPolygon FullPoly = new MultiPolygon();
			MultiPolygon PrimaryPoly = new MultiPolygon();
			Ring tpmRing = new Ring();
			Polygon tmpPoly = new Polygon();

			if (SpecialCase)
			{
				if (SOCWidth > matf.SemiWidth)
				{
					ptRE01 = ARANFunctions.LocalToPrj(FMAPt.PrjPt, _MACourse, prmSOCFromMAPtDistance, -SOCWidth);
					ptRE02 = ARANFunctions.LocalToPrj(matf.PrjPt, _MACourse, LPT, -matf.SemiWidth);
					ptLE02 = ARANFunctions.LocalToPrj(matf.PrjPt, _MACourse, LPT, matf.SemiWidth);
					ptLE01 = ARANFunctions.LocalToPrj(FMAPt.PrjPt, _MACourse, prmSOCFromMAPtDistance, SOCWidth);

					tpmRing.Add(ptRE00);
					tpmRing.Add(ptRE01);
					tpmRing.Add(ptRE02);
					tpmRing.Add(ptLE02);
					tpmRing.Add(ptLE01);
					tpmRing.Add(ptLE00);
					tmpPoly.ExteriorRing = tpmRing;
					FullPoly.Add(tmpPoly);
					//==================================================================
					tpmRing = new Ring();
					tmpPoly = new Polygon();

					ptLE00 = ARANFunctions.RingVectorIntersect(PrimaryPolyMAPt[0].ExteriorRing, refPt, F3ApproachDir + 0.5 * Math.PI, out fTmp, true);
					if (ptLE00 != null && fTmp > 0)
						FMAPt.ASW_2_L = fTmp;
					else
						ptLE00 = ARANFunctions.LocalToPrj(refPt, F3ApproachDir, 0, FMAPt.ASW_2_L);

					ptRE00 = ARANFunctions.RingVectorIntersect(PrimaryPolyMAPt[0].ExteriorRing, refPt, F3ApproachDir - 0.5 * Math.PI, out fTmp, true);
					if (ptRE00 != null && fTmp > 0)
						FMAPt.ASW_2_R = fTmp;
					else
						ptRE00 = ARANFunctions.LocalToPrj(refPt, F3ApproachDir + Math.PI, 0, FMAPt.ASW_2_R);

					ptRE01 = ARANFunctions.LocalToPrj(FMAPt.PrjPt, _MACourse, prmSOCFromMAPtDistance, -0.5 * SOCWidth);
					ptRE02 = ARANFunctions.LocalToPrj(matf.PrjPt, _MACourse, LPT, -0.5 * matf.SemiWidth);
					ptLE02 = ARANFunctions.LocalToPrj(matf.PrjPt, _MACourse, LPT, 0.5 * matf.SemiWidth);
					ptLE01 = ARANFunctions.LocalToPrj(FMAPt.PrjPt, _MACourse, prmSOCFromMAPtDistance, 0.5 * SOCWidth);

					tpmRing.Add(ptRE00);
					tpmRing.Add(ptRE01);
					tpmRing.Add(ptRE02);
					tpmRing.Add(ptLE02);
					tpmRing.Add(ptLE01);
					tpmRing.Add(ptLE00);

					tmpPoly.ExteriorRing = tpmRing;
					PrimaryPoly.Add(tmpPoly);
				}
				else if (DistFullWidth28 > DistTP - matf.EPT)
				{
					ptRE01 = ARANFunctions.LocalToPrj(matf.PrjPt, _MACourse, -matf.EPT, -matf.SemiWidth);
					ptRE02 = ARANFunctions.LocalToPrj(matf.PrjPt, _MACourse, LPT, -matf.SemiWidth);

					ptLE02 = ARANFunctions.LocalToPrj(matf.PrjPt, _MACourse, LPT, matf.SemiWidth);
					ptLE01 = ARANFunctions.LocalToPrj(matf.PrjPt, _MACourse, -matf.EPT, matf.SemiWidth);

					//GlobalVars.gAranGraphics.DrawPointWithText(ptRE01, "ptRE01");
					//GlobalVars.gAranGraphics.DrawPointWithText(ptRE02, "ptRE02");

					//GlobalVars.gAranGraphics.DrawPointWithText(ptLE01, "ptLE01");
					//GlobalVars.gAranGraphics.DrawPointWithText(ptLE02, "ptLE02");
					//LegBase.ProcessMessages();

					tpmRing.Add(ptRE00);
					tpmRing.Add(ptRE01);
					tpmRing.Add(ptRE02);
					tpmRing.Add(ptLE02);
					tpmRing.Add(ptLE01);
					tpmRing.Add(ptLE00);

					tmpPoly.ExteriorRing = tpmRing;
					FullPoly.Add(tmpPoly);
					//==================================================================
					tpmRing = new Ring();
					tmpPoly = new Polygon();

					ptLE00 = ARANFunctions.RingVectorIntersect(PrimaryPolyMAPt[0].ExteriorRing, refPt, F3ApproachDir + 0.5 * Math.PI, out fTmp, true);
					if (ptLE00 != null && fTmp > 0)
						FMAPt.ASW_2_L = fTmp;
					else
						ptLE00 = ARANFunctions.LocalToPrj(refPt, F3ApproachDir, 0, FMAPt.ASW_2_L);

					ptRE00 = ARANFunctions.RingVectorIntersect(PrimaryPolyMAPt[0].ExteriorRing, refPt, F3ApproachDir - 0.5 * Math.PI, out fTmp, true);
					if (ptRE00 != null && fTmp > 0)
						FMAPt.ASW_2_R = fTmp;
					else
						ptRE00 = ARANFunctions.LocalToPrj(refPt, F3ApproachDir + Math.PI, 0, FMAPt.ASW_2_R);

					//GlobalVars.gAranGraphics.DrawPointWithText(ptLE00, "ptLE00");
					//GlobalVars.gAranGraphics.DrawPointWithText(ptRE00, "ptRE00");
					//LegBase.ProcessMessages();

					ptRE01 = ARANFunctions.LocalToPrj(matf.PrjPt, _MACourse, -matf.EPT, -0.5 * matf.SemiWidth);
					ptRE02 = ARANFunctions.LocalToPrj(matf.PrjPt, _MACourse, LPT, -0.5 * matf.SemiWidth);
					ptLE02 = ARANFunctions.LocalToPrj(matf.PrjPt, _MACourse, LPT, 0.5 * matf.SemiWidth);
					ptLE01 = ARANFunctions.LocalToPrj(matf.PrjPt, _MACourse, -matf.EPT, 0.5 * matf.SemiWidth);

					tpmRing.Add(ptRE00);
					tpmRing.Add(ptRE01);
					tpmRing.Add(ptRE02);
					tpmRing.Add(ptLE02);
					tpmRing.Add(ptLE01);
					tpmRing.Add(ptLE00);
					tmpPoly.ExteriorRing = tpmRing;
					PrimaryPoly.Add(tmpPoly);
				}
			}
			else
			{
				if ((eTurnAt)comboBox06_01.SelectedItem == eTurnAt.TP)
				{
					DistTurn = DistTP - matf.EPT;
					ptEnd = ARANFunctions.LocalToPrj(matf.PrjPt, _MACourse, LPT);
				}
				else
				{
					DistTurn = DistTP;

					double PilotTolerance = GlobalVars.constants.Pansops[ePANSOPSData.arMAPilotToleran].Value;
					double BankTolerance = GlobalVars.constants.Pansops[ePANSOPSData.arMAPilotToleran].Value;
					double TAS = ARANMath.IASToTAS(prmMAIAS, prmPreTurnAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value); //GlobalVars.CurrADHP.LowestTemperature -)+ GlobalVars.constants.Pansops[ePANSOPSData.arNearTerrWindSp].Value;
					double Dist6sec = (TAS + GlobalVars.constants.Pansops[ePANSOPSData.dpWind_Speed].Value) * (PilotTolerance + BankTolerance);

					ptEnd = ARANFunctions.LocalToPrj(matf.PrjPt, _MACourse, Dist6sec);
				}

				//GlobalVars.gAranGraphics.DrawPointWithText (ptEnd, "ptEnd");
				//GlobalVars.gAranGraphics.DrawPointWithText (matf.PrjPt, "Matf");
				//LegBase.ProcessMessages();


				ptTmp = ARANFunctions.CircleVectorIntersect(GlobalVars.CurrADHP.pPtPrj, PANSOPSConstantList.PBNInternalTriggerDistance, FMAPt.PrjPt, _MACourse, out fTmp);
				double TriggerDistance28 = ARANMath.Hypot(ptTmp.X - refPt.X, ptTmp.Y - refPt.Y) - WayPoint.WPTParams[(int)ePBNClass.RNP_APCH, (int)eFlightPhase.MApGE28].ATT;

				if (DistTP >= TriggerDistance28)
					ptFar = ARANFunctions.LocalToPrj(refPt, _MACourse, TriggerDistance28);
				else
					ptFar = (Point)ptEnd.Clone();

				//==========================================================================================================
				if (DistTP < DistFullWidth28)
					ptRE01 = (Point)ARANFunctions.LineLineIntersect(ptRE00, _MACourse - SplayAngle15, ptFar, _MACourse + 0.5 * Math.PI);
				else
				{
					ptRE01 = (Point)ARANFunctions.LineLineIntersect(ptRE00, _MACourse - SplayAngle15, ptASWR28, _MACourse);
					ptRE02 = (Point)ARANFunctions.LineLineIntersect(ptRE01, _MACourse, ptFar, _MACourse + 0.5 * Math.PI);

					if (DistTP >= TriggerDistance28)
					{
						if (DistTP < TriggerDistance28 + DistFullWidth56)
							ptRE03 = (Point)ARANFunctions.LineLineIntersect(ptRE02, _MACourse - SplayAngle15, ptEnd, _MACourse + 0.5 * Math.PI);
						else
						{
							ptRE03 = (Point)ARANFunctions.LineLineIntersect(ptRE02, _MACourse - SplayAngle15, ptASWR56, _MACourse);
							ptRE04 = (Point)ARANFunctions.LineLineIntersect(ptRE03, _MACourse, ptEnd, _MACourse + 0.5 * Math.PI);
						}
					}
				}

				//GlobalVars.gAranGraphics.DrawPointWithText(ptRE01, "ptRE01");
				//LegBase.ProcessMessages();

				//==========================================================================================================
				if (DistTP < DistFullWidth28)
					ptLE01 = (Point)ARANFunctions.LineLineIntersect(ptLE00, _MACourse + SplayAngle15, ptFar, _MACourse + 0.5 * Math.PI);
				else
				{
					ptLE01 = (Point)ARANFunctions.LineLineIntersect(ptLE00, _MACourse + SplayAngle15, ptASWL28, _MACourse);
					ptLE02 = (Point)ARANFunctions.LineLineIntersect(ptLE01, _MACourse, ptFar, _MACourse + 0.5 * Math.PI);

					if (DistTP >= TriggerDistance28)
					{
						if (DistTP < TriggerDistance28 + DistFullWidth56)
							ptLE03 = (Point)ARANFunctions.LineLineIntersect(ptLE02, _MACourse + SplayAngle15, ptEnd, _MACourse + 0.5 * Math.PI);
						else
						{
							ptLE03 = (Point)ARANFunctions.LineLineIntersect(ptLE02, _MACourse + SplayAngle15, ptASWL56, _MACourse);
							ptLE04 = (Point)ARANFunctions.LineLineIntersect(ptLE03, _MACourse, ptEnd, _MACourse - 0.5 * Math.PI);
						}
					}
				}

				//GlobalVars.gAranGraphics.DrawPointWithText(ptLE01, "ptLE01");
				//LegBase.ProcessMessages();
				//==========================================================================================================

				//GlobalVars.gAranGraphics.DrawPointWithText(ptRE00, "ptRE00");
				//GlobalVars.gAranGraphics.DrawPointWithText(ptRE01, "ptRE01");
				//GlobalVars.gAranGraphics.DrawPointWithText(ptFar, "ptFar");
				//LegBase.ProcessMessages();

				tpmRing.Add(ptRE00);
				tpmRing.Add(ptRE01);

				if (DistTP >= DistFullWidth28)
				{
					tpmRing.Add(ptRE02);
					if (DistTP >= TriggerDistance28)
					{
						tpmRing.Add(ptRE03);
						if (DistTP >= TriggerDistance28 + DistFullWidth56)
						{
							tpmRing.Add(ptRE04);
							tpmRing.Add(ptLE04);
						}
						tpmRing.Add(ptLE03);
					}
					tpmRing.Add(ptLE02);
				}

				tpmRing.Add(ptLE01);
				tpmRing.Add(ptLE00);

				tmpPoly.ExteriorRing = tpmRing;
				FullPoly.Add(tmpPoly);

				tpmRing = new Ring();
				tmpPoly = new Polygon();

				//==========================================================================================================
				ptLE00 = ARANFunctions.RingVectorIntersect(PrimaryPolyMAPt[0].ExteriorRing, refPt, F3ApproachDir + 0.5 * Math.PI, out fTmp, true);

				if (ptLE00 != null && fTmp > 0)
					FMAPt.ASW_2_L = fTmp;
				else
					ptLE00 = ARANFunctions.LocalToPrj(refPt, F3ApproachDir, 0, FMAPt.ASW_2_L);

				//GlobalVars.gAranGraphics.DrawPointWithText(ptLE00, "*ptLE00");
				//LegBase.ProcessMessages();

				ptRE00 = ARANFunctions.RingVectorIntersect(PrimaryPolyMAPt[0].ExteriorRing, refPt, F3ApproachDir - 0.5 * Math.PI, out fTmp, true);

				if (ptRE00 != null && fTmp > 0)
					FMAPt.ASW_2_R = fTmp;
				else
					ptRE00 = ARANFunctions.LocalToPrj(refPt, F3ApproachDir + Math.PI, 0, FMAPt.ASW_2_R);

				//GlobalVars.gAranGraphics.DrawPointWithText(ptRE00, "*ptRE00");
				//LegBase.ProcessMessages();

				//
				fTmp = ARANFunctions.PointToLineDistance(ptRE01, FMAPt.PrjPt, _MACourse);
				ptRE01 = ARANFunctions.LocalToPrj(ptRE01, _MACourse, 0, -0.5 * fTmp);

				//GlobalVars.gAranGraphics.DrawPointWithText(ptRE01, "*ptRE01");
				//LegBase.ProcessMessages();

				fTmp = ARANFunctions.PointToLineDistance(ptLE01, FMAPt.PrjPt, _MACourse);
				ptLE01 = ARANFunctions.LocalToPrj(ptLE01, _MACourse, 0, -0.5 * fTmp);

				//GlobalVars.gAranGraphics.DrawPointWithText(ptLE01, "*ptLE01");
				//LegBase.ProcessMessages();

				if (DistTP >= DistFullWidth28)
				{
					fTmp = ARANFunctions.PointToLineDistance(ptRE02, FMAPt.PrjPt, _MACourse);
					ptRE02 = ARANFunctions.LocalToPrj(ptRE02, _MACourse, 0, -0.5 * fTmp);

					fTmp = ARANFunctions.PointToLineDistance(ptLE02, FMAPt.PrjPt, _MACourse);
					ptLE02 = ARANFunctions.LocalToPrj(ptLE02, _MACourse, 0, -0.5 * fTmp);

					if (DistTP >= TriggerDistance28)
					{
						fTmp = ARANFunctions.PointToLineDistance(ptRE03, FMAPt.PrjPt, _MACourse);
						ptRE03 = ARANFunctions.LocalToPrj(ptRE03, _MACourse, 0, -0.5 * fTmp);

						fTmp = ARANFunctions.PointToLineDistance(ptLE03, FMAPt.PrjPt, _MACourse);
						ptLE03 = ARANFunctions.LocalToPrj(ptLE03, _MACourse, 0, -0.5 * fTmp);

						if (DistTP >= TriggerDistance28 + DistFullWidth56)
						{
							fTmp = ARANFunctions.PointToLineDistance(ptRE04, FMAPt.PrjPt, _MACourse);
							ptRE04 = ARANFunctions.LocalToPrj(ptRE04, _MACourse, 0, -0.5 * fTmp);

							fTmp = ARANFunctions.PointToLineDistance(ptLE04, FMAPt.PrjPt, _MACourse);
							ptLE04 = ARANFunctions.LocalToPrj(ptLE04, _MACourse, 0, -0.5 * fTmp);
						}
					}
				}

				//==========================================================================================================
				tpmRing.Add(ptRE00);
				tpmRing.Add(ptRE01);

				if (DistFullWidth28 < DistTurn)
				{
					tpmRing.Add(ptRE02);
					if (DistTP >= TriggerDistance28)
					{
						tpmRing.Add(ptRE03);
						if (DistTP >= TriggerDistance28 + DistFullWidth56)
						{
							tpmRing.Add(ptRE04);
							tpmRing.Add(ptLE04);
						}
						tpmRing.Add(ptLE03);
					}
					tpmRing.Add(ptLE02);
				}

				tpmRing.Add(ptLE01);
				tpmRing.Add(ptLE00);

				tmpPoly.ExteriorRing = tpmRing;
				PrimaryPoly.Add(tmpPoly);
			}

			//GlobalVars.gAranGraphics.DrawMultiPolygon(FullPoly, eFillStyle.sfsBackwardDiagonal);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(PrimaryPoly, eFillStyle.sfsCross);

			_straightTIALeq.CreateGeometry(Leg, GlobalVars.CurrADHP);

			//_straightTIALeq.RefreshGraphics();
			//Leg.RefreshGraphics();
			//_straightTIALeq.RefreshGraphics();

			//GlobalVars.gAranGraphics.DrawLineString(_straightTIALeq.NominalTrack,-2 );
			//LegBase.ProcessMessages();

			SMATFullPolyList = FullPoly;
			SMATPrimaryPolyList = PrimaryPoly;

			_straightTIALeq.FullArea = SMATFullPolyList;
			_straightTIALeq.PrimaryArea = SMATPrimaryPolyList;

			//_straightTIALeq.RefreshGraphics();
			//LegBase.ProcessMessages();
		}

		void ReCreateMATurnArea()
		{
			Point ptTmp0, ptTmp1, ptTmp2;

			LineString CutterPart;
			MultiLineString CutterPline;

			//MultiPolygon pLeft, pRight, PrimaryPoly, FullPoly;

			FMATF.TurnDistance = _TurnFromMAPtDist;

			//	CalcFromMinStablizationDistance
			// comboBox06_06

			double Dir = prmTurnCourse;          //ArcTan2(ptDst.Y - FMATF.PrjPt.Y, ptDst.X - FMATF.PrjPt.X);
			Point ptDst;

			if (radioButton06_03.Checked)
			{
				if (comboBox06_06.SelectedIndex < 0) return;
				ptDst = (Point)(((WPT_FIXType)comboBox06_06.SelectedItem).pPtPrj.Clone());
			}
			else if ((eTurnAt)comboBox06_01.SelectedItem == eTurnAt.MApt)
				ptDst = ARANFunctions.LocalToPrj(FMAPt.PrjPt, Dir, prmNWPTFromTPDist);
			else
				ptDst = ARANFunctions.LocalToPrj(FMATF.PrjPt, Dir, prmNWPTFromTPDist);

			//GlobalVars.gAranGraphics.DrawPointWithText(ptDst,"ptDest");
			//LegBase.ProcessMessages();

			FMAHFturn.EntryDirection = Dir;
			FMAHFturn.PrjPt = ptDst;

			//=============================================================================================================

			GeometryOperators GGeoOperators = new GeometryOperators();
			Point point;
			if (FMATF.TurnAt == eTurnAt.TP)
			{
				point = ((LineString)(FMATF.ReferenceGeometry))[0];
				_straightTIALeq.Visible = true;
				//_turnLeg.StartFIX = FMATF;
			}
			else
			{
				point = FMATF.PrjPt;
				_straightTIALeq.Visible = false;
				//_turnLeg.StartFIX = FMAPt;
			}

			//GlobalVars.gAranGraphics.DrawPointWithText(point, "point");
			//LegBase.ProcessMessages();

			FMAFinalOCA = _PreOCA;
			double DistKKToSOC = ARANFunctions.PointToLineDistance(point, FSOC.PrjPt, _MACourse - 0.5 * Math.PI);   //F3ApproachDir
			DistKKToSOC = _TurnFromMAPtDist - FMAPt.SOCDistance;

			double Dist = GGeoOperators.GetDistance(ptDst, FMATF.ReferenceGeometry) + DistKKToSOC;
			prmTerminationAltitude = FMAFinalOCA + Dist * _MAClimb;

			//=================================================================================================================
			double DistMAHFFromARP = ARANMath.Hypot(ptDst.X - GlobalVars.CurrADHP.pPtPrj.X, ptDst.Y - GlobalVars.CurrADHP.pPtPrj.Y);

			//	if (FMAHF.SensorType == stGNSS)		TriggerDistance = GNSSTriggerDistance
			//	else								TriggerDistance = SBASTriggerDistance + GNSSTriggerDistance;
			//	TriggerDistance = PBNTerminalTriggerDistance;
			//????????????????????????????????????????????????
			/*
				if rbIAPCH_DME_DME.Checked then
					FMAHF.SensorType = stDME_DME
				else
					FMAHF.SensorType = stGNSS;
			*/
			//????????????????????????????????????????????????
			FMATF.OutDirection = Dir;
			FMATF.FlyPath = (eFlyPath)(comboBox06_05.SelectedItem);

			//FMATF.SensorType = eSensorType.GNSS;
			//FMAHFturn.SensorType = eSensorType.GNSS;

			//if (radioButton06_01.Checked)
			//	FMAHFturn.SensorType = eSensorType.GNSS;
			//else
			//	FMAHFturn.SensorType = eSensorType.DME_DME;

			if (DistMAHFFromARP >= PANSOPSConstantList.PBNTerminalTriggerDistance)
				FMAHFturn.Role = eFIXRole.MAHF_GT_56;
			else
				FMAHFturn.Role = eFIXRole.MAHF_LE_56;

			if (DistMAHFFromARP >= PANSOPSConstantList.PBNInternalTriggerDistance)
				FMAHFturn.FlightPhase = eFlightPhase.MApGE28;
			else
				FMAHFturn.FlightPhase = eFlightPhase.MApLT28;

			LegApch prevleg = _finalLeq;
			if (_FinalSDFCnt > 0)
				prevleg = _finalSDFLeg;

			//_straightTIALeq.CreateGeometry(prevleg, GlobalVars.CurrADHP);

			//GlobalVars.gAranGraphics.DrawMultiPolygon(_straightTIALeq.FullArea, eFillStyle.sfsHorizontal);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(_straightTIALeq.PrimaryArea, eFillStyle.sfsVertical);
			////_straightTIALeq.RefreshGraphics();
			//LegBase.ProcessMessages();

			CreateMATIASegment(FMATF);

			//_straightTIALeq.RefreshGraphics();

			//LegBase.ProcessMessages(true);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(_straightTIALeq.FullArea, eFillStyle.sfsHorizontal);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(_straightTIALeq.PrimaryArea, eFillStyle.sfsVertical);
			//LegBase.ProcessMessages();

			//_straightTIALeq.RefreshGraphics();
			//LegBase.ProcessMessages();

			if ((eTurnAt)comboBox06_01.SelectedItem == eTurnAt.TP)
			{
				ptTmp2 = ARANFunctions.LocalToPrj(FMATF.PrjPt, _MACourse, -FMATF.EPT);
				ptTmp0 = ARANFunctions.LocalToPrj(ptTmp2, _MACourse, 0, 100000);
				ptTmp1 = ARANFunctions.LocalToPrj(ptTmp2, _MACourse, 0, -100000);

				CutterPart = new LineString();
				CutterPart.Add(ptTmp0);
				CutterPart.Add(ptTmp1);

				CutterPline = new MultiLineString();
				CutterPline.Add(CutterPart);

				GGeoOperators.Cut(SMATFullPolyList, CutterPline, out Geometry gLeft, out Geometry gRight);
				MultiPolygon pLeft = (MultiPolygon)gLeft;
				//pRight = (MultiPolygon)gRight;

				FMATF.SecondaryTolerArea.Assign(pLeft);

				GGeoOperators.Cut(SMATPrimaryPolyList, CutterPline, out gLeft, out gRight);
				pLeft = (MultiPolygon)gLeft;
				//pRight = (MultiPolygon)gRight;

				FMATF.TolerArea.Assign(pLeft);
			}
			else
			{
				FMATF.ReCreateArea();
				//FMATF.CreateTolerArea();
				//FMATF.SecondaryTolerArea.Assign(_straightTIALeq.FullArea);
				//FMATF.TolerArea.Assign(_straightTIALeq.PrimaryArea);
			}

			//FMATFLeg = TLeg.Create(FMATF, FMAHF, fpFinalMissedApproach, GUI, FMAPtLeg);
			//_turnLeg.CreateGeometry(_straightTIALeq, GlobalVars.CurrADHP);

			//FMAPtLeg.FullArea = SMATFullPolyList;
			//FMAPtLeg.PrimaryArea = SMATPrimaryPolyList;

			_turnLeg.CreateGeometry(_straightTIALeq, GlobalVars.CurrADHP);

			//if (FMATF.FlyPath == eFlyPath.DirectToFIX)
			//{
			//	prmTurn2 = _turnLeg.CalcDRNomDir(); // StartFIX.OutDirection; //
			//	if (_turnLeg.StartFIX.TurnDirection == TurnDirection.CCW)
			//	{
			//		prmTurn1 = _turnLeg.StartFIX.OutDirection_R;
			//		prmTurn3 = _turnLeg.StartFIX.OutDirection_L;
			//	}
			//	else
			//	{
			//		prmTurn1 = _turnLeg.StartFIX.OutDirection_L;
			//		prmTurn3 = _turnLeg.StartFIX.OutDirection_R;
			//	}
			//}
			//else
			//	prmTurn2 = Dir;

			//GlobalVars.gAranGraphics.DrawMultiPolygon(_turnLeg.FullAssesmentArea, eFillStyle.sfsHorizontal);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(_turnLeg.PrimaryAssesmentArea, eFillStyle.sfsVertical);
			//LegBase.ProcessMessages();
			bool variant = true;

			if (variant)
			{
				//PrimaryPoly = _turnLeg.PrimaryAssesmentArea;
				//FullPoly = _turnLeg.FullAssesmentArea;

				primeTAPoly = (MultiPolygon)GGeoOperators.UnionGeometry(FMATF.TolerArea, _turnLeg.PrimaryAssesmentArea);
				secondTAPoly = (MultiPolygon)GGeoOperators.UnionGeometry(FMATF.SecondaryTolerArea, _turnLeg.FullAssesmentArea);
			}
			else
			{
				//_turnLeg.CreateNomTrack();
				//_turnLeg.CreateKKLine();

				primeTAPoly = Functions.CreateMATurnArea(FMATF, FMAHFturn, Dir, true, FMATF.FlyPath == eFlyPath.DirectToFIX);
				secondTAPoly = Functions.CreateMATurnArea(FMATF, FMAHFturn, Dir, false, FMATF.FlyPath == eFlyPath.DirectToFIX);

				primeTAPoly = (MultiPolygon)GGeoOperators.UnionGeometry(FMATF.TolerArea, primeTAPoly);
				secondTAPoly = (MultiPolygon)GGeoOperators.UnionGeometry(FMATF.SecondaryTolerArea, secondTAPoly);
			}

			_turnLeg.PrimaryAssesmentArea = primeTAPoly;
			_turnLeg.FullAssesmentArea = secondTAPoly;

			//_turnLeg.RefreshGraphics();
			//LegBase.ProcessMessages();

			GlobalVars.gAranGraphics.SafeDeleteGraphic(kkLineElem);

			//GlobalVars.gAranGraphics.SafeDeleteGraphic(secondPolyEl);
			//GlobalVars.gAranGraphics.SafeDeleteGraphic(primePolyEl);
			//secondPolyEl = GlobalVars.gAranGraphics.DrawMultiPolygon(secondTAPoly, eFillStyle.sfsHorizontal);
			//primePolyEl = GlobalVars.gAranGraphics.DrawMultiPolygon(primeTAPoly, eFillStyle.sfsVertical);
			//LegBase.ProcessMessages();
			//GlobalVars.gAranGraphics.SafeDeleteGraphic(secondPolyEl);
			//GlobalVars.gAranGraphics.SafeDeleteGraphic(primePolyEl);

			if (FMATF.ReferenceGeometry != null)
				if (FMATF.ReferenceGeometry.Type == GeometryType.LineString)
					kkLineElem = GlobalVars.gAranGraphics.DrawLineString((LineString)FMATF.ReferenceGeometry, 2, ARANFunctions.RGB(255, 0, 255));
				else
					kkLineElem = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)FMATF.ReferenceGeometry, eFillStyle.sfsHollow, ARANFunctions.RGB(255, 0, 255));

			FMAPt.StraightArea = SMATPrimaryPolyList;

			//FullPoly = SMATFullPolyList;
			//PrimaryPoly = SMATPrimaryPolyList;

#if ttt
			GlobalVars.gAranGraphics.SafeDeleteGraphic(FMAFullPolyGr);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(FMAPrimPolyGr);

			FMAFullPolyGr = GlobalVars.gAranGraphics.DrawMultiPolygon(FullPoly, eFillStyle.sfsHollow, ARANFunctions.RGB(0, 255, 0));
			FMAPrimPolyGr = GlobalVars.gAranGraphics.DrawMultiPolygon(PrimaryPoly, eFillStyle.sfsHollow, ARANFunctions.RGB(255, 0, 0));
#endif
			//_straightTIALeq.RefreshGraphics();
			_turnLeg.RefreshGraphics();
			//LegBase.ProcessMessages();


			GetTurnObstacles();
		}

		void GetTurnObstacles()
		{
			//===========================================

			//FAF2THRDist = Hypot(FFAF.PrjPt.X - FFarMAPt.X , FFAF.PrjPt.Y - FFarMAPt.Y);
			//if (!FAligned)
			//	FAF2THRDist = FAF2THRDist +
			//		Hypot(FRWYDirection.Prj.X - FFarMAPt.X , FRWYDirection.Prj.Y - FFarMAPt.Y);

			//Dist = FAF2THRDist - GPANSOPSConstants.Constant[arMOCChangeDist].Value;
			//AddMOC = (Dist * GPANSOPSConstants.Constant[arAddMOCCoef].Value) * Integer(Dist > 0);

			//LegApch prevleg = _finalLeq;
			//if (_FinalSDFCnt > 0)
			//	prevleg = _finalSDFLeg;
			//_straightTIALeq.CreateGeometry(prevleg, GlobalVars.CurrADHP);

			//double X = 0.0;
			//if (FMATF.TurnAt == eTurnAt.TP)
			//	X = -FMATF.EPT;

			//LineString Part = new LineString();
			//Point point;
			//point = ARANFunctions.LocalToPrj(FMATF.PrjPt, FMATF.EntryDirection, X, 100000.0);
			//Part.Add(point);

			//point = ARANFunctions.LocalToPrj(FMATF.PrjPt, FMATF.EntryDirection, X, -100000.0);
			//Part.Add(point);

			//MultiLineString CutterPline = new MultiLineString();
			//CutterPline.Add(Part);

			//GeometryOperators GGeoOperators = new GeometryOperators();

			//MultiPolygon FullPoly = SMATFullPolyList;

			//GGeoOperators.Cut(FullPoly, CutterPline, out Geometry gLeft, out Geometry gRight);

			//MultiPolygon TIAFullPoly = (MultiPolygon)gRight;

			//FullAreaOutline = CreateOutline(FMAPt.PrjPt, _MACourse + Math.PI, FMAPt.PrjPt, _MACourse, TIAFullPoly);

			//MultiPolygon PrimaryPoly = SMATPrimaryPolyList;
			//GGeoOperators.Cut(PrimaryPoly, CutterPline, out gLeft, out gRight);
			//MultiPolygon TIAPrimaryPoly = (MultiPolygon)gRight;

			//TIAObstacleList= GetObstacleInfoList(TIAPrimaryPoly, TIAFullPoly, FullAreaOutline, TIAMOC, FObstalces);
			//_turnLeg.CreateGeometry(_straightTIALeq, GlobalVars.CurrADHP);

			//FullAreaOutline = CreateOutline(FMAPt.PrjPt, _MACourse + Math.PI, FMATF.PrjPt, FMATF.EntryDirection, secondTAPoly);
			//MATAObstacleList= GetObstacleInfoList(primeTAPoly, secondTAPoly, FullAreaOutline, fMOC, FObstalces);

			//PolygonL = RemoveAgnails(secondTAPoly);
			//GUI.DrawPolygon(secondTAPoly, 255, sfsHorizontal);
			//GUI.DrawPolygon(RemoveAgnails(secondTAPoly), 0, sfsBackwardDiagonal);
			//GUI.DrawPolyline(FullAreaOutline, 0, 2);

			double fMOC30 = GlobalVars.constants.Pansops[ePANSOPSData.arMA_InterMOC].Value;
			double fMOC50 = GlobalVars.constants.Pansops[ePANSOPSData.arMA_FinalMOC].Value;
			//=======================================================

			FMATIAOCA = _PreOCA;
			double Dist, FullFAMOC = GlobalVars.constants.Pansops[ePANSOPSData.arFASeg_FAF_MOC].Value;

			double TIAMOC = (eTurnAt)comboBox06_01.SelectedItem == eTurnAt.TP ? fMOC30 : fMOC50;

			_straightTIALeq.Obstacles = Functions.GetLegProtectionAreaObstacles(_straightTIALeq, GlobalVars.ObstacleList, TIAMOC, _refElevation);
			TIAObstacleList = _straightTIALeq.Obstacles;

			int IxTIA = -1, n = TIAObstacleList.Parts.Length;

			for (int i = 0; i < n; i++)
			{
				Dist = ARANFunctions.PointToLineDistance(TIAObstacleList.Parts[i].pPtPrj, FSOC.PrjPt, _MACourse - 0.5 * Math.PI);
				TIAObstacleList.Parts[i].Dist = Dist;

				TIAObstacleList.Parts[i].ReqOCH = TIAObstacleList.Parts[i].Elev + TIAObstacleList.Parts[i].fSecCoeff * fMOC30 - Dist * _MAClimb;
				double fTmp = TIAObstacleList.Parts[i].Elev + TIAObstacleList.Parts[i].fSecCoeff * FullFAMOC;

				if (Dist < 0 && fTmp < TIAObstacleList.Parts[i].ReqOCH)
					TIAObstacleList.Parts[i].ReqOCH = fTmp;

				//ObstacleInfo.ReqOCH = //ObstacleInfo.Elev + 
				//Math.Min(ObstacleInfo.fSecCoeff * TIAMOC - Dist * _MAClimb, ObstacleInfo.fSecCoeff * FullFAMOC);

				if (TIAObstacleList.Parts[i].ReqOCH > FMATIAOCA)
				{
					FMATIAOCA = TIAObstacleList.Parts[i].ReqOCH;
					IxTIA = i;
				}
			}
			//=======================================================

			FMATAOCA = _PreOCA;
			FMAFinalOCA = _PreOCA;
			int IxTA = -1;
			Point point;

			if (FMATF.TurnAt == eTurnAt.TP)
				point = ((LineString)(FMATF.ReferenceGeometry))[0];
			else
				point = FMATF.PrjPt;

			double DistKKToSOC = ARANFunctions.PointToLineDistance(point, FSOC.PrjPt, _MACourse - 0.5 * Math.PI);   //F3ApproachDir
			DistKKToSOC = _TurnFromMAPtDist - FMAPt.SOCDistance;

			//FMAFinalOCA + DistKKToSOC * _MAClimb
			GeometryOperators GGeoOperators = new GeometryOperators();

			Dist = GGeoOperators.GetDistance(FMAHFturn.PrjPt, FMATF.ReferenceGeometry) + DistKKToSOC;
			prmTerminationAltitude = FMAFinalOCA + Dist * _MAClimb;

			double fMOC = FMATF.TurnAngle <= GlobalVars.constants.Pansops[ePANSOPSData.arMATurnTrshAngl].Value ? fMOC30 : fMOC50;

			_turnLeg.Obstacles = Functions.GetLegProtectionAreaObstacles(_turnLeg, GlobalVars.ObstacleList, fMOC, _refElevation);
			MATAObstacleList = _turnLeg.Obstacles;

			n = MATAObstacleList.Parts.Length;
			for (int i = 0; i < n; i++)
			{
				Dist = GGeoOperators.GetDistance(MATAObstacleList.Parts[i].pPtPrj, FMATF.ReferenceGeometry);
				MATAObstacleList.Parts[i].Dist = Dist;

				MATAObstacleList.Parts[i].ReqOCH = MATAObstacleList.Parts[i].Elev + MATAObstacleList.Parts[i].fSecCoeff * fMOC - (DistKKToSOC + Dist) * _MAClimb;
				if (MATAObstacleList.Parts[i].ReqOCH > FMATAOCA)
				{
					FMATAOCA = MATAObstacleList.Parts[i].ReqOCH;
					IxTA = i;
				}
			}

			FMATIAOCA_CO = "";

			if (FMATAOCA > FMATIAOCA)
			{
				FMAFinalOCA = FMATAOCA;
				FMATIAOCA_CO = MATAObstacleList.Obstacles[MATAObstacleList.Parts[IxTA].Owner].UnicalName;
			}
			else if (FMATIAOCA > _PreOCA)
			{
				FMAFinalOCA = FMATIAOCA;
				FMATIAOCA_CO = TIAObstacleList.Obstacles[TIAObstacleList.Parts[IxTIA].Owner].UnicalName;
			}

			textBox06_08.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(FMAFinalOCA).ToString();

			//DistKKToSOC = _TurnFromMAPtDist - FMAPt.SOCDistance;
			prmPreTurnAltitude = prmOCA + DistKKToSOC * _MAClimb;

			FFinalTurnAltitude = FMAFinalOCA + DistKKToSOC * _MAClimb;

			textBox06_07.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(FFinalTurnAltitude).ToString();
			textBox06_09.Text = FMATIAOCA_CO;

			_reportForm.FillMA_TurnInitiationArea(TIAObstacleList, "MA straight segment/TIA");
			_reportForm.FillMA_TurnArea(MATAObstacleList);
		}

		/** /
		void GetStraightMAHFObstacles()
		{
			double TIAMOC;

			MultiPolygon FullPoly = SMATFullPolyList;
			MultiPolygon PrimaryPoly = SMATPrimaryPolyList;

			//===========================================

			if ((eTurnAt)comboBox06_01.SelectedItem < eTurnAt.TP)
				TIAMOC = GlobalVars.constants.Pansops[ePANSOPSData.arMA_InterMOC].Value;
			else
				TIAMOC = GlobalVars.constants.Pansops[ePANSOPSData.arMA_FinalMOC].Value;

			//MultiLineString FullAreaOutline = Functions.CreateOutline(FMAPt.PrjPt, _MACourse + ARANMath.C_PI, FMAPt.PrjPt, _MACourse, FullPoly);
			//TIAObstacleList = Functions.GetObstacleInfoList(PrimaryPoly, FullPoly, FullAreaOutline, TIAMOC, GlobalVars.ObstacleList);
			TIAObstacleList = Functions.GetLegProtectionAreaObstacles(_straightTIALeq, GlobalVars.ObstacleList, TIAMOC, _refElevation);

			FMATIAOCA = prmOCA;
			double FullFAMOC = GlobalVars.constants.Pansops[ePANSOPSData.arFASeg_FAF_MOC].Value;
			double fMOC = GlobalVars.constants.Pansops[ePANSOPSData.arMA_InterMOC].Value;
			int n = TIAObstacleList.Parts.Length;
			int IxTIA = -1;

			for (int i = 0; i < n; i++)
			{
				double Dist = ARANFunctions.PointToLineDistance(TIAObstacleList.Parts[i].pPtPrj, FSOC.PrjPt, _MACourse - 0.5 * Math.PI);
				TIAObstacleList.Parts[i].Dist = Dist;

				TIAObstacleList.Parts[i].ReqOCH = TIAObstacleList.Parts[i].Elev + TIAObstacleList.Parts[i].fSecCoeff * fMOC - Dist * _MAClimb;
				double fTmp = TIAObstacleList.Parts[i].Elev + TIAObstacleList.Parts[i].fSecCoeff * FullFAMOC;

				if (TIAObstacleList.Parts[i].Dist < 0 && fTmp < TIAObstacleList.Parts[i].ReqOCH)
					TIAObstacleList.Parts[i].ReqOCH = fTmp;

				if (TIAObstacleList.Parts[i].ReqOCH > FMATIAOCA)
				{
					FMATIAOCA = TIAObstacleList.Parts[i].ReqOCH;
					IxTIA = i;
				}
			}

			string UnicalName = "";

			if (FMATIAOCA > prmOCA)
				UnicalName = TIAObstacleList.Obstacles[TIAObstacleList.Parts[i].Owner].UnicalName;

			textBox05_11.Text = UnicalName;
			textBox05_07.Text = GlobalVars.unitConverter.HeightToDisplayUnits(FMATIAOCA).ToString();

			_reportForm.AddMIAS_TIA(TIAObstacleList, "MA straight segment till MAHF");
		}
/**/

		///
		/// ==========================================================
		///
		private void comboBox06_01_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised || comboBox06_01.SelectedIndex < 0)
				return;

			try
			{
				_skipEvents = true;
				//prmTurnFromMAPtDist.Active = comboBox06_01.SelectedItem == eTurnAt.TP;
				prmTurnFromMAPtDistInterval.Min = FMAPt.SOCDistance + ((eTurnAt)comboBox06_01.SelectedItem > eTurnAt.Altitude ? FMATF.ATT : 0);
				//prmTurnFromSOCDist.MaxValue :=  80000.0;
				//prmTurnFromMAPtDist.EndUpdate;

				if ((eTurnAt)comboBox06_01.SelectedItem == eTurnAt.TP)
				{
					//TO DO check prmTurnFromMAPtDist value 
				}

				comboBox06_04.Enabled = (eTurnAt)comboBox06_01.SelectedItem == eTurnAt.TP;

				if ((eTurnAt)comboBox06_01.SelectedItem <= eTurnAt.Altitude)
					comboBox06_04.SelectedIndex = 1;

				//comboBox06_04.Enabled = true;
				//if (comboBox06_01.SelectedItem == eTurnAt.Mapt)
				//{
				//	prmPreTurnAltitude = prmPreTurnAltitudeInterval.Min;
				//	comboBox06_04.SelectedIndex = 1;
				//	comboBox06_04.Enabled = false;
				//}

				//comboBox06_05.Enabled = false;

				if ((eTurnAt)comboBox06_01.SelectedItem == eTurnAt.Altitude)
					comboBox06_05.Items[0] = eFlyPath.CourseToFIX;  // "Course to FIX";
				else
					comboBox06_05.Items[0] = eFlyPath.TrackToFIX;   // "Track to FIX";

				//comboBox06_05.Enabled = true;
				//if (comboBox06_01.SelectedItem == eTurnAt.Altitude)
				//{
				//	comboBox06_05.SelectedIndex = 2;
				//	//comboBox06_05.Enabled = false;
				//}

				FMATF.TurnAt = (eTurnAt)comboBox06_01.SelectedItem;
				if (FMATF.TurnAt == eTurnAt.Altitude)
					FMATF.FlyMode = eFlyMode.Atheight;
				else
					FMATF.FlyMode = (eFlyMode)comboBox06_04.SelectedIndex;

				FMATF.FlyPath = (eFlyPath)comboBox06_05.SelectedIndex;

				//prmPreTurnAltitude.Active = comboBox06_01.SelectedItem == eTurnAt.Altitude;
				prmPreTurnAltitudeReadOnly = (eTurnAt)comboBox06_01.SelectedItem != eTurnAt.Altitude;
				//prmPreTurnAltitudeShowMinMaxHint = comboBox06_01.SelectedItem == eTurnAt.Altitude;

				//prmTurnFromMAPtDistActive = comboBox06_01.SelectedItem > eTurnAt.Altitude;
				prmTurnFromMAPtDistReadOnly = (eTurnAt)comboBox06_01.SelectedItem <= eTurnAt.Altitude;
				//prmTurnFromMAPtDistShowMinMaxHint = comboBox06_01.SelectedItem = eTurnAt.TP;

				//==========================================================================

				//if (comboBox06_04.SelectedIndex = 0) or (comboBox06_05.SelectedIndex = 0) then
				//	prmNWPTFromTPDist.MinValue := MinMATurnDist
				//else

				double fTurn = 1 - 2 * comboBox06_02.SelectedIndex;             //Right = -1;	Left = 1
				double fTurnAngle = ARANMath.Modulus((prmTurnCourse - _MACourse) * fTurn, ARANMath.C_2xPI);
				double fTAS = ARANMath.IASToTAS(_MAIAS, _preTurnAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value) + GlobalVars.constants.Pansops[ePANSOPSData.arNearTerrWindSp].Value;

				double MinStabPrev = FMATF.CalcConstructionFromMinStablizationDistance(fTurnAngle, (eFlyPath)comboBox06_05.SelectedItem == eFlyPath.DirectToFIX);
				//double MinStabPrev = FMATF.CalcConstructionFromMinStablizationDistance((eFlyPath)comboBox06_05.SelectedItem == eFlyPath.DirectToFIX);

				double plannedTurnAngle = ARANMath.DegToRad((double)numericUpDown06_01.Value);
				double MinStabCurr = FMAHFturn.CalcNomLineInToMinStablizationDistance(plannedTurnAngle);
				double fMinDist = MinStabPrev + MinStabCurr;
				//double fMin = Math.Max(MinMATurnDist, fMinDist);

				prmNWPTFromTPDistInterval.Min = Math.Max(MinMATurnDist, fMinDist);

				//==========================================================================
				//==

				double delta = ARANMath.DegToRad(240 - 120 * ((eFlyPath)comboBox06_05.SelectedItem == eFlyPath.DirectToFIX ? 0 : 1));

				if ((eTurnAt)comboBox06_01.SelectedItem == eTurnAt.TP && comboBox06_04.SelectedIndex == 0)
				{
					double limit = 2 * Math.Atan((prmTurnFromMAPtDist - FMATF.ATT - FMAPt.SOCDistance) / FMATF.CalcTurnRadius(fTAS));
					if (delta > limit) delta = limit;
				}

				prmTurnCourseInterval.Min = _MACourse - comboBox06_02.SelectedIndex * delta;
				prmTurnCourseInterval.Max = _MACourse + (1 - comboBox06_02.SelectedIndex) * delta;

				_TurnCourse = prmTurnCourseInterval.CheckValue(_TurnCourse);
				prmTurnCourse = _TurnCourse;
			}
			finally
			{
				_skipEvents = false;
			}


			DoTurnOn();
			//if (comboBox06_01.SelectedItem < eTurnAt.Tp)
			//	TurnOnMAPt();
			//else if (comboBox06_01.SelectedItem == eTuenAt.TP)
			//	TurnOnTP();
		}

		private void comboBox06_02_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised || comboBox06_02.SelectedIndex < 0)
				return;

			FMATF.TurnDirection = (TurnDirection)(1 - 2 * comboBox06_02.SelectedIndex);
			//	FMATF.ReCreateArea;

			double delta = ARANMath.DegToRad(240 - 120 * ((eFlyPath)comboBox06_05.SelectedItem == eFlyPath.DirectToFIX ? 0 : 1));
			if ((eTurnAt)comboBox06_01.SelectedItem == eTurnAt.TP && comboBox06_04.SelectedIndex == 0)
			{
				double fTAS = ARANMath.IASToTAS(_MAIAS, _preTurnAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value) + GlobalVars.constants.Pansops[ePANSOPSData.arNearTerrWindSp].Value;
				double limit = 2 * Math.Atan((prmTurnFromMAPtDist - FMATF.ATT - FMAPt.SOCDistance) / FMATF.CalcTurnRadius(fTAS));
				if (delta > limit) delta = limit;
			}

			//	prmTurnCourse.Active := False;
			//prmTurnCourse.BeginUpdate;
			prmTurnCourseInterval.Min = _MACourse - comboBox06_02.SelectedIndex * delta;
			prmTurnCourseInterval.Max = _MACourse + (1 - comboBox06_02.SelectedIndex) * delta;
			// TO DO - chek prmTurnCourse value

			//	prmTurnCourse.Active := True;
			//	prmTurnCourse.PerformCheck(True);

			int n = FillMATurnPoints();
			radioButton06_03.Enabled = radioButton06_04.Enabled = n > 0;

			bool bTst = radioButton06_04.Checked;
			if (n == 0)
				radioButton06_04.Checked = true;

			if (bTst)
			{
				double fTmp = prmTurnCourseInterval.CheckValue(_TurnCourse);
				if (fTmp != _TurnCourse)
					prmTurnCourse = fTmp;
				else
					ApplyTurnCourse();
			}
		}

		private void comboBox06_04_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised || _skipEvents || comboBox06_04.SelectedIndex < 0)
				return;

			//FMATF.FlyMode = (eFlyMode)(comboBox06_04.SelectedIndex);

			if (FMATF.TurnAt == eTurnAt.Altitude)
				FMATF.FlyMode = eFlyMode.Atheight;
			else
				FMATF.FlyMode = (eFlyMode)comboBox06_04.SelectedIndex;

			if (!comboBox06_04.Enabled)
				return;

			if ((eTurnAt)comboBox06_01.SelectedItem < eTurnAt.TP || comboBox06_04.SelectedIndex > 0)
				comboBox06_05.Enabled = true;
			else
			{
				comboBox06_05.SelectedIndex = 1;
				comboBox06_05.Enabled = false;
			}

			double delta = ARANMath.DegToRad(240 - 120 * ((eFlyPath)comboBox06_05.SelectedItem == eFlyPath.DirectToFIX ? 0 : 1));
			if ((eTurnAt)comboBox06_01.SelectedItem == eTurnAt.TP && comboBox06_04.SelectedIndex == 0)
			{
				double fTAS = ARANMath.IASToTAS(_MAIAS, _preTurnAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value) + GlobalVars.constants.Pansops[ePANSOPSData.arNearTerrWindSp].Value;
				double limit = 2 * Math.Atan((prmTurnFromMAPtDist - FMATF.ATT - FMAPt.SOCDistance) / FMATF.CalcTurnRadius(fTAS));
				if (delta > limit) delta = limit;
			}

			//prmTurnCourse.Active := True;//False;
			prmTurnCourseInterval.Min = _MACourse - comboBox06_02.SelectedIndex * delta;
			prmTurnCourseInterval.Max = _MACourse + (1 - comboBox06_02.SelectedIndex) * delta;
			// TO DO - chek prmTurnCourse value

			int n = FillMATurnPoints();

			radioButton06_03.Enabled = radioButton06_04.Enabled = n > 0;

			bool bTst = radioButton06_04.Checked;
			if (n == 0)
				radioButton06_04.Checked = true;

			if (bTst)
				ApplyTurnCourse();
		}

		private void comboBox06_05_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised || _skipEvents || comboBox06_05.SelectedIndex < 0)
				return;

			double delta = ARANMath.DegToRad(240 - 120 * ((eFlyPath)comboBox06_05.SelectedItem == eFlyPath.DirectToFIX ? 0 : 1));

			if ((eTurnAt)comboBox06_01.SelectedItem == eTurnAt.TP && comboBox06_04.SelectedIndex == 0)
			{
				double fTAS = ARANMath.IASToTAS(_MAIAS, _preTurnAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value) + GlobalVars.constants.Pansops[ePANSOPSData.arNearTerrWindSp].Value;
				double limit = 2 * Math.Atan((prmTurnFromMAPtDist - FMATF.ATT - FMAPt.SOCDistance) / FMATF.CalcTurnRadius(fTAS));
				if (delta > limit) delta = limit;
			}

			FMATF.FlyPath = (eFlyPath)comboBox06_05.SelectedItem;

			//prmTurn3Visible = prmTurn1Visible = 
			FMAHFturn.IsDFTarget = FMATF.FlyPath == eFlyPath.DirectToFIX;

			//prmTurnCourse.Active = False;
			prmTurnCourseInterval.Min = _MACourse - comboBox06_02.SelectedIndex * delta;
			prmTurnCourseInterval.Max = _MACourse + (1 - comboBox06_02.SelectedIndex) * delta;
			// TO DO - chek prmTurnCourse value

			int n = FillMATurnPoints();

			radioButton06_03.Enabled = radioButton06_04.Enabled = n > 0;

			bool bTst = radioButton06_04.Checked;
			if (n == 0)
				radioButton06_04.Checked = true;

			if (bTst)
				ApplyTurnCourse();
		}

		private void comboBox06_06_SelectedIndexChanged(object sender, EventArgs e)
		{
			//labelTurnWPTType.Text = "";
			if (!bFormInitialised || !radioButton06_03.Checked || comboBox06_06.SelectedIndex < 0)
				return;

			WPT_FIXType sigPoint = (WPT_FIXType)comboBox06_06.SelectedItem;
			//labelTurnWPTType.Text = AIXMTypeNames[sigPoint.AIXMType];

			double dX, dY;

			if ((eTurnAt)comboBox06_01.SelectedItem == eTurnAt.MApt)
			{
				dX = sigPoint.pPtPrj.X - FMAPt.PrjPt.X;
				dY = sigPoint.pPtPrj.Y - FMAPt.PrjPt.Y;
			}
			else
			{
				dX = sigPoint.pPtPrj.X - FMATF.PrjPt.X;
				dY = sigPoint.pPtPrj.Y - FMATF.PrjPt.Y;
			}

			double fDist = ARANMath.Hypot(dX, dY);
			//prmNWPTFromTPDist = fDist;
			_NWPTFromTPDist = fDist;
			prmNWPTFromTPDist = fDist;

			double fAngle = Math.Atan2(dY, dX);
#if first
			_TurnCourse = fAngle + 1;
			prmTurnCourse = fAngle;
#else
			_TurnCourse = fAngle;
			prmTurnCourse = fAngle;
			ReCreateMATurnArea();
#endif
		}

		private void comboBox06_07_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised || comboBox06_07.SelectedIndex < 0)
				return;

			FMAHFturn.FlyMode = (eFlyMode)comboBox06_07.SelectedIndex;
			numericUpDown06_01_ValueChanged(numericUpDown06_01, null);
			//ReCreateMATurnArea();
		}

		private void numericUpDown06_01_ValueChanged(object sender, EventArgs e)
		{
			double plannedTurnAngle = ARANMath.DegToRad((double)numericUpDown06_01.Value);
			double MinStabPrev, MinStabCurr;
			MinStabPrev = FMATF.CalcConstructionFromMinStablizationDistance((eFlyPath)comboBox06_05.SelectedItem == eFlyPath.DirectToFIX);
			MinStabCurr = FMAHFturn.CalcNomLineInToMinStablizationDistance(plannedTurnAngle);

			double mindist = MinStabPrev + MinStabCurr;
			double fMin = Math.Max(MinMATurnDist, mindist);

			prmNWPTFromTPDistInterval.Min = fMin;

			if (_NWPTFromTPDist < fMin)
				prmNWPTFromTPDist = fMin;
		}

		private void radioButton06_03_CheckedChanged(object sender, EventArgs e)
		{
			if (!((RadioButton)sender).Checked)
				return;

			bool bTurnExistingPoint = sender == radioButton06_03;

			comboBox06_06.Visible = bTurnExistingPoint;
			textBox06_15.Visible = !bTurnExistingPoint;

			prmNWPTFromTPDistReadOnly = bTurnExistingPoint;
			//prmNWPTFromTPDistActive = !bTurnExistingPoint;

			prmTurnCourseReadOnly = bTurnExistingPoint;

			//if (tabControl1.SelectedIndex !=6 )				return;

			if (bTurnExistingPoint)
			{
				if (comboBox06_06.SelectedIndex < 0)
					comboBox06_06.SelectedIndex = 0;
				else
					comboBox06_06_SelectedIndexChanged(comboBox06_06, null);
			}
			else
				ApplyTurnCourse();
		}

		private void textBox06_01_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox06_01_Validating(textBox06_01, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox06_01.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox06_01_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox06_01.Text, out fTmp))
			{
				textBox06_01.Text = GlobalVars.unitConverter.AngleToDisplayUnits(_MATBankAngle).ToString();
				return;
			}

			if (textBox06_01.Tag != null && textBox06_01.Tag.ToString() == textBox06_01.Text)
				return;

			double newValue = GlobalVars.unitConverter.AngleToInternalUnits(fTmp);

			if (newValue < prmMATBankAngleInterval.Min)
				newValue = prmMATBankAngleInterval.Min;

			if (newValue > prmMATBankAngleInterval.Max)
				newValue = prmMATBankAngleInterval.Max;

			prmMATBankAngle = newValue;
		}
		private void textBox06_02_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox06_02_Validating(textBox06_02, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox06_02.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox06_02_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox06_02.Text, out fTmp))
			{
				textBox06_02.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_MAIAS).ToString();
				return;
			}

			if (textBox06_02.Tag != null && textBox06_02.Tag.ToString() == textBox06_02.Text)
				return;

			double newValue = GlobalVars.unitConverter.SpeedToInternalUnits(fTmp);

			if (newValue < prmMAIASInterval.Min)
				newValue = prmMAIASInterval.Min;

			if (newValue > prmMAIASInterval.Max)
				newValue = prmMAIASInterval.Max;

			prmMAIAS = newValue;
		}
		private void textBox06_03_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox06_03_Validating(textBox06_03, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox06_03.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox06_03_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox06_03.Text, out fTmp))
			{
				textBox06_03.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_TurnFromMAPtDist).ToString();
				return;
			}

			if (textBox06_03.Tag != null && textBox06_03.Tag.ToString() == textBox06_03.Text)
				return;

			double newValue = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			prmTurnFromMAPtDist = prmTurnFromMAPtDistInterval.CheckValue(newValue);
		}

		private void textBox06_04_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox06_04_Validating(textBox06_04, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox06_04.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox06_04_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox06_04.Text, out fTmp))
			{
				textBox06_04.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_preTurnAltitude).ToString();
				return;
			}

			if (textBox06_04.Tag != null && textBox06_04.Tag.ToString() == textBox06_04.Text)
				return;

			double newValue = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);

			prmPreTurnAltitude = prmPreTurnAltitudeInterval.CheckValue(newValue);
		}

		private void textBox06_10_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox06_10_Validating(textBox06_10, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox06_10.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox06_10_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox06_10.Text, out fTmp))
			{
				textBox06_10.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_NWPTFromTPDist).ToString();
				return;
			}

			if (textBox06_10.Tag != null && textBox06_10.Tag.ToString() == textBox06_10.Text)
				return;

			double newValue = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			if (newValue < prmNWPTFromTPDistInterval.Min)
				newValue = prmNWPTFromTPDistInterval.Min;

			if (newValue > prmNWPTFromTPDistInterval.Max)
				newValue = prmNWPTFromTPDistInterval.Max;

			prmNWPTFromTPDist = newValue;
		}

		private void textBox06_11_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox06_11_Validating(textBox06_11, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox06_11.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox06_11_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox06_11.Text, out fTmp))
			{
				double AppAzt = ARANFunctions.DirToAzimuth(FMATF.PrjPt, _TurnCourse, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				//double AppAzt = ARANFunctions.DirToAzimuth(FMAPt.PrjPt, value, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				textBox06_11.Text = GlobalVars.unitConverter.AzimuthToDisplayUnits(AppAzt).ToString();
			}

			if (textBox06_11.Tag != null && textBox06_11.Tag.ToString() == textBox06_11.Text)
				return;

			double newValue;
			//double newValue = GlobalVars.unitConverter.AngleToInternalUnits(fTmp);
			if ((eTurnAt)comboBox06_01.SelectedItem == eTurnAt.MApt)
				newValue = ARANFunctions.AztToDirection(FMAPt.GeoPt, fTmp, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);
			else
				newValue = ARANFunctions.AztToDirection(FMATF.GeoPt, fTmp, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);

			prmTurnCourse = prmTurnCourseInterval.CheckValue(newValue);
		}

		private void textBox06_15_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
			{
				textBox06_15_Validating(textBox06_15, null);
				eventChar = '\0';
			}
			else if (eventChar >= ' ')// && (eventChar < '0' || eventChar > '9'))         // || eventChar != '_')
			{
				char alfa = (char)(eventChar & ~32);

				if (alfa < 'A' || alfa > 'Z')
					eventChar = '\0';
				else
					eventChar = alfa;
			}

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox06_15_Validating(object sender, CancelEventArgs e)
		{
			FMAHFturn.Name = FMAHFturn.CallSign = textBox06_15.Text;
			FMAHFturn.RefreshGraphics();
		}

		#endregion

		#region Page 8
		void ToMultiTurnPage()
		{
			LegBase leg = _turnLeg;

			//leg.Obstacles = _turnLeg.InnerObstacleList;
			//leg.DetObstacle_1 = _turnLeg.DetObs;
			//leg.EndFIX.Name = textBox214.Text;

			//leg.DetObstacle_1 = CalcPDGToTop(leg.Obstacles);
			GeometryOperators go = new GeometryOperators();

			if (leg.EndFIX.FlyMode == eFlyMode.Atheight)
			{
				leg.PathAndTermination = CodeSegmentPath.CA;
				leg.MinLegLength = ARANFunctions.ReturnDistanceInMeters(leg.StartFIX.PrjPt, leg.EndFIX.NomLinePrjPt);
			}
			else
			{
				//leg.EndFIX.FlyMode = eFlyMode.Flyover;
				leg.PathAndTermination = CodeSegmentPath.CF;
				if (radioButton06_03.Checked)
				{
					WPT_FIXType SigPoint = (WPT_FIXType)comboBox06_06.SelectedItem;
					leg.EndFIX.Name = SigPoint.Name;
					leg.EndFIX.CallSign = SigPoint.CallSign;
					leg.EndFIX.Id = SigPoint.Identifier;
				}

				Point ptTmp = ARANFunctions.LocalToPrj(leg.EndFIX.PrjPt, leg.EndFIX.EntryDirection, -leg.EndFIX.EPT, 0);
				leg.MinLegLength = ARANFunctions.ReturnDistanceInMeters(leg.StartFIX.NomLinePrjPt, ptTmp);
			}

			leg.Duration = leg.MinLegLength / leg.StartFIX.NomLineTAS;
			textBox07_10.Text = (leg.Duration / 60.0).ToString("0.00");

			for (int i = 0; i < leg.Obstacles.Parts.Length; i++)
			{
				double fTmp = go.GetDistance(_straightTIALeq.KKLine, leg.Obstacles.Parts[i].pPtPrj);
				leg.Obstacles.Parts[i].DistStar = leg.Obstacles.Parts[i].d0 = fTmp;// leg.Obstacles.Parts[i].Dist;
			}
			_reportForm.FillPageCurrPage(leg.Obstacles, leg.DetObstacle_1);
			_reportForm.AddPage4(leg.Obstacles, leg.DetObstacle_1);
			Report = true;

			//=====================================================================================================
			numericUpDown07_01Range.Circular = true;
			numericUpDown07_01Range.InDegree = true;
			radioButton07_01.Checked = true;
			radioButton07_03.Checked = true;

			//segment1Term.Clean();

			_SignificantPoints = new List<WayPoint>();
			for (int i = 0; i < GlobalVars.WPTList.Length; i++)
				_SignificantPoints.Add(new WayPoint(eFIXRole.TP_, GlobalVars.WPTList[i], GlobalVars.gAranEnv));

			_turnLeg.MinLegLength = go.GetDistance(_straightTIALeq.KKLine, _turnLeg.KKLine);

			transitions = new Transitions(_SignificantPoints, GlobalVars.CurrADHP, _turnLeg, _refElevation, _MAClimb, 0.5 * Math.PI, 300000.0, _AirCat, GlobalVars.gAranEnv, InitNewPoint);

			transitions.OnFIXUpdated = FIXUpdated;
			transitions.OnDistanceChanged = OnLegDistanceChanged;
			//transitions.OnConstructAltitudeChanged = OnConstructionAltitudeChanged;
			transitions.OnNomlineAltitudeChanged = OnNomLineAltitudeChanged;
			//transitions.AccelerationAltitude = _accelerationAltitude;

			transitions.BankAngle = prmMATBankAngle;
			transitions.OnUpdateDirList = OnUpdateDirList;
			transitions.OnUpdateDistList = OnUpdateDistList;

			//transitions.MOCLimit = EnrouteMOCValues[comboBox07_09.SelectedIndex];

			numericUpDown07_01Range.Max = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Min, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			numericUpDown07_01Range.Min = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Max, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			//transitions.NomLineGradient = _MAClimb;

			//comboBox07_06.Items.Clear();
			//foreach (ePBNClass pbn in firstTurnPBNClass)
			//	comboBox07_06.Items.Add(pbn);

			comboBox07_07.Items.Clear();

			if (leg.EndFIX.FlyMode == eFlyMode.Atheight)
			{
				comboBox07_07.Items.Add(CodeSegmentPath.CF);
				comboBox07_07.Items.Add(CodeSegmentPath.DF);
			}
			else if (leg.EndFIX.FlyMode == eFlyMode.Flyover)
			{
				comboBox07_07.Items.Add(CodeSegmentPath.TF);
				comboBox07_07.Items.Add(CodeSegmentPath.DF);
			}
			else
				comboBox07_07.Items.Add(CodeSegmentPath.TF);

			comboBox07_07.SelectedIndex = 0;

			if (comboBox07_05.SelectedIndex != 0)
				comboBox07_05.SelectedIndex = 0;
			else
				transitions.FlyMode = (eFlyMode)comboBox07_05.SelectedIndex;

			if (comboBox07_04.SelectedIndex != 0)
				comboBox07_04.SelectedIndex = 0;
			else
				transitions.SensorType = (eSensorType)comboBox07_04.SelectedIndex;

			if (comboBox07_06.SelectedIndex != 0)
				comboBox07_06.SelectedIndex = 0;
			else
				transitions.PBNClass = (ePBNClass)comboBox07_06.SelectedItem;

			if (comboBox07_07.SelectedIndex != 0)
				comboBox07_07.SelectedIndex = 0;
			else
				transitions.PathAndTermination = (CodeSegmentPath)comboBox07_07.SelectedItem;

			numericUpDown07_02.Value = (decimal)ARANMath.RadToDeg(transitions.PlannedTurnAngle);

			textBox07_02.Text = transitions.FIXName;
			textBox07_03.Text = textBox06_02.Text;
			textBox07_03.Tag = null;

			double nnAzt = ARANFunctions.DirToAzimuth(transitions.ReferenceFIX.NomLinePrjPt, transitions.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			numericUpDown07_01.Value = (decimal)nnAzt;

			transitions.UpdateEnabled = true;

			textBox07_01.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(transitions.Distance, eRoundMode.NEAREST).ToString();

			double TAS = ARANMath.IASToTAS(transitions.IAS, transitions.NomLineAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);

			textBox07_06.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.SPECIAL_NEAREST).ToString();
			textBox07_05.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.NomLineAltitude, eRoundMode.NEAREST).ToString();
			//textBox07_04.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.NomLineGradient, eRoundMode.NEAREST).ToString();

			//TAS = ARANMath.IASToTAS(transitions.IAS, transitions.NomLineAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			//textBox309.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.NEAREST).ToString();
			//textBox308.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.NomLineAltitude, eRoundMode.NEAREST).ToString();
			//textBox307.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.NomLineGradient, eRoundMode.NEAREST).ToString();

			UpDateComboBox07_08(true);
		}

		private void UpDateComboBox07_08(bool decreasingEnabled = false)
		{
			comboBox07_08.SelectedIndex = 0;

			//int SelectedIndex = 0;
			//for (int i = 0; i < heights.Length; i++)
			//	if (transitions.NomLineAltitude - ptDerPrj.Z + 30.0 < heights[i])
			//	{
			//		SelectedIndex = i;
			//		break;
			//	}

			//if (decreasingEnabled || SelectedIndex > comboBox308.SelectedIndex)
			//	comboBox308.SelectedIndex = SelectedIndex;
		}

		#region transitions events

		void InitNewPoint(object sender, WayPoint ReferenceFIX, WayPoint AddedFIX)
		{
			ReferenceFIX.ISAtC = GlobalVars.CurrADHP.ISAtC;
			AddedFIX.ISAtC = GlobalVars.CurrADHP.ISAtC;

			_AddedFIX = AddedFIX;

			if (transitions != null)
			{
				numericUpDown07_01Range.Max = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Min, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				numericUpDown07_01Range.Min = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Max, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

				textBox07_02.Text = transitions.FIXName;
			}
		}

		void FIXUpdated(object sender, WayPoint CurrFIX)
		{
			CurrFIX.ISAtC = GlobalVars.CurrADHP.ISAtC;

			//transitions.CurrLeg.DetObstacle_1

			textBox07_04.ForeColor = System.Drawing.Color.Black;
			if (transitions.CurrentDetObs.Parts.Length > 0)
			{
				if (FMAFinalOCA < transitions.CurrentDetObs.Parts[0].ReqOCH)
					textBox07_04.ForeColor = System.Drawing.Color.Red;

				textBox07_04.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.CurrentDetObs.Parts[0].ReqOCH).ToString();
			}

			_reportForm.FillPageCurrPage(transitions.CurrentObstacleList, transitions.CurrentDetObs);
			Report = true;

			if (ReportBtn.Checked && !_reportForm.Visible)
				_reportForm.Show(GlobalVars.Win32Window);
		}

		//void OnConstructionAltitudeChanged(object sender, double value)
		//{
		//	double TAS = ARANMath.IASToTAS(transitions.IAS, value, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
		//	textBox07_06.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS).ToString();

		//	textBox07_05.Text = GlobalVars.unitConverter.HeightToDisplayUnits(value).ToString();
		//	textBox07_05.Tag = textBox07_05.Text;
		//	textBox07_04.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.ConstructGradient, eRoundMode.NEAREST).ToString();
		//}

		void OnNomLineAltitudeChanged(object sender, double value)
		{
			double TAS = ARANMath.IASToTAS(transitions.IAS, transitions.NomLineAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);

			textBox07_06.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.SPECIAL_NEAREST).ToString();

			textBox07_05.Text = GlobalVars.unitConverter.HeightToDisplayUnits(value, eRoundMode.NEAREST).ToString();
			textBox07_05.Tag = textBox07_05.Text;
			//textBox07_04.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.NomLineGradient, eRoundMode.NEAREST).ToString();
		}

		void OnLegDistanceChanged(object sender, double value)
		{
			textBox07_01.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(value, eRoundMode.NEAREST).ToString();
			textBox07_01.Tag = textBox07_01.Text;
			//textBox07_04.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.NomLineGradient, eRoundMode.NEAREST).ToString();
		}

		void OnUpdateDirList(object sender, EventArgs e)
		{
			if (transitions == null)
				return;

			transitions.turnDirection = comboBox07_02.SelectedIndex == 0 ? TurnDirection.CCW : TurnDirection.CW;

			WayPoint OldWpt = null;
			if (comboBox07_01.SelectedIndex >= 0)
				OldWpt = (WayPoint)comboBox07_01.SelectedItem;

			int wptIndex = -1;
			comboBox07_01.Items.Clear();

			foreach (WayPoint wpt in transitions.CourseSgfPoint)
			{
				if (wpt.Equals(OldWpt))
					wptIndex = comboBox07_01.Items.Count;

				try
				{
					comboBox07_01.Items.Add(wpt);
				}
				catch
				{
					break;
				}
			}

			radioButton07_02.Enabled = comboBox07_01.Items.Count > 0;
			if (radioButton07_02.Enabled)
			{
				if (wptIndex >= 0)
					comboBox07_01.SelectedIndex = wptIndex;
				else
					comboBox07_01.SelectedIndex = 0;
			}
			else
				radioButton07_01.Checked = true;
		}

		void OnUpdateDistList(object sender, EventArgs e)
		{
			if (transitions == null)
				return;

			WayPoint OldWpt = null;
			int OldIndex = -1;

			if (comboBox07_03.SelectedIndex >= 0)
				OldWpt = (WayPoint)comboBox07_03.SelectedItem;

			comboBox07_03.Items.Clear();

			foreach (WayPoint wpt in transitions.DistanceSgfPoint)
			{
				if (OldWpt != null && wpt.Equals(OldWpt))
					OldIndex = comboBox07_03.Items.Count;
				comboBox07_03.Items.Add(wpt);
			}

			radioButton07_04.Enabled = comboBox07_03.Items.Count > 0;
			if (radioButton07_04.Enabled)
			{
				if (OldIndex >= 0)
					comboBox07_03.SelectedIndex = OldIndex;
				else
					comboBox07_03.SelectedIndex = 0;
			}
			else
				radioButton07_03.Checked = true;
		}

		#endregion

		private void button07_01_Click(object sender, EventArgs e)
		{
			LegsInfoForm.ShowFixInfo(Left + tabControl1.Left + tabPage8.Left + button07_01.Left, Top + tabPage8.Top + button07_01.Top, transitions.LegPoints);
		}

		private void radioButton07_01_CheckedChanged(object sender, EventArgs e)
		{
			if (transitions != null)
				transitions.DirectionIndex = -1;

			bool isSame = false;
			if (radioButton07_02.Checked && radioButton07_04.Checked && _AddedFIX.FlyMode == eFlyMode.Atheight && transitions.PathAndTermination == CodeSegmentPath.CF)
			{
				WayPoint dirPoint = (WayPoint)comboBox07_01.SelectedItem;
				WayPoint sigPoint = (WayPoint)comboBox07_03.SelectedItem;
				isSame = sigPoint.Id == dirPoint.Id;
			}

			numericUpDown07_01.Enabled = radioButton07_01.Checked || isSame;// (radioButton07_04.Checked && _AddedFIX.FlyMode == eFlyMode.Atheight && transitions.PathAndTermination == ePathAndTermination.CF);

			comboBox07_01.Enabled = !radioButton07_01.Checked;

			if (!radioButton07_01.Checked)
				comboBox07_01_SelectedIndexChanged(comboBox07_01, null);
		}

		private void radioButton07_03_CheckedChanged(object sender, EventArgs e)
		{
			textBox07_01.ReadOnly = !radioButton07_03.Checked;
			comboBox07_03.Enabled = !radioButton07_03.Checked;
			textBox07_02.ReadOnly = !radioButton07_03.Checked;

			bool isSame = false;
			if (radioButton07_02.Checked && radioButton07_04.Checked && _AddedFIX.FlyMode == eFlyMode.Atheight && transitions.PathAndTermination == CodeSegmentPath.CF)
			{
				WayPoint dirPoint = (WayPoint)comboBox07_01.SelectedItem;
				WayPoint sigPoint = (WayPoint)comboBox07_03.SelectedItem;
				isSame = sigPoint.Id == dirPoint.Id;
			}

			radioButton07_01.Enabled = radioButton07_01.Checked || isSame;// (radioButton07_04.Checked && _AddedFIX.FlyMode == eFlyMode.Atheight && transitions.PathAndTermination == ePathAndTermination.CF);

			if (transitions == null)
				return;

			if (radioButton07_04.Checked)
				comboBox07_03_SelectedIndexChanged(comboBox07_03, null);
			else
				transitions.DistanceIndex = -1;

			textBox07_02.Text = transitions.FIXName;
		}

		private bool PreventRecurse301 = false;

		private void numericUpDown07_01_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				if (numericUpDown07_01.Value == numericUpDown07_01.Minimum)
					numericUpDown07_01.Value = numericUpDown07_01.Maximum;
				else if (numericUpDown07_01.Value == numericUpDown07_01.Maximum)
					numericUpDown07_01.Value = numericUpDown07_01.Minimum;
			}
		}

		private void numericUpDown07_01_ValueChanged(object sender, EventArgs e)
		{
			double newAzt = (double)numericUpDown07_01.Value;
			double Direction;
			Interval OutDirRange = transitions.OutDirRange;

			bool isSame = false;
			if (radioButton07_02.Checked && radioButton07_04.Checked && _AddedFIX.FlyMode == eFlyMode.Atheight && transitions.PathAndTermination == CodeSegmentPath.CF)
			{
				WayPoint dirPoint = (WayPoint)comboBox07_01.SelectedItem;
				WayPoint sigPoint = (WayPoint)comboBox07_03.SelectedItem;
				isSame = sigPoint.Id == dirPoint.Id;
			}

			if (isSame)
			{
				WayPoint dirSig = (WayPoint)comboBox07_01.SelectedItem;

				Direction = ARANFunctions.AztToDirection(_AddedFIX.NomLineGeoPt, newAzt, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);

				double OutDir = Math.Atan2(dirSig.PrjPt.Y - _AddedFIX.NomLinePrjPt.Y, dirSig.PrjPt.X - _AddedFIX.NomLinePrjPt.X);

				double legWidth = _AddedFIX.TurnDirection == TurnDirection.CW ? _AddedFIX.ASW_L : _AddedFIX.ASW_R;
				double sinAlpha = legWidth / transitions.Distance;
				if (sinAlpha > 0.5)
					sinAlpha = 0.5;
				double maxTrackOffset = Math.Asin(sinAlpha);

				if (_AddedFIX.TurnDirection == TurnDirection.CCW)
				{
					OutDirRange.Min = OutDir - maxTrackOffset;  //OutDir1;	// 
					OutDirRange.Max = OutDir;
				}
				else
				{
					OutDirRange.Min = OutDir;
					OutDirRange.Max = OutDir + maxTrackOffset;  //OutDir1;	//
				}
			}

			if (!PreventRecurse301)
			{
				if (newAzt <= (double)numericUpDown07_01.Minimum)
					newAzt = (double)numericUpDown07_01.Maximum - 1;
				else if (newAzt >= (double)numericUpDown07_01.Maximum)
					newAzt = (double)numericUpDown07_01.Minimum + 1;

				numericUpDown07_01Range.Max = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, OutDirRange.Min, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				numericUpDown07_01Range.Min = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, OutDirRange.Max, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

				double OldVal = newAzt;
				double newValue = numericUpDown07_01Range.CheckValue(newAzt);

				PreventRecurse301 = numericUpDown07_01.Value != (decimal)newValue;

				if (PreventRecurse301)
				{
					numericUpDown07_01.Value = (decimal)newValue;
					return;
				}
			}

			PreventRecurse301 = false;

			//if (_AddedFIX != null && _AddedFIX.GeoPt != null)
			Direction = ARANFunctions.AztToDirection(_AddedFIX.NomLineGeoPt, newAzt, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);
			//else
			//	Direction = ARANFunctions.AztToDirection(MaptGeo, newAzt, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);

			Direction = ARANMath.Modulus(Direction, ARANMath.C_2xPI);       ///????????????????????????????????

			if (isSame)
				Direction = OutDirRange.CheckValue(Direction);

			transitions.OutDirection = Direction;

			textBox07_01.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(transitions.Distance).ToString();
			textBox07_01.Tag = textBox07_01.Text;

			double TAS = ARANMath.IASToTAS(transitions.IAS, transitions.NomLineAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			textBox07_06.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.SPECIAL_NEAREST).ToString();
			textBox07_05.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.NomLineAltitude).ToString();
			textBox07_05.Tag = textBox07_05.Text;
			//textBox07_04.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.NomLineGradient).ToString();

			UpDateComboBox07_08();
		}

		private void numericUpDown07_02_ValueChanged(object sender, EventArgs e)
		{
			transitions.PlannedTurnAngle = ARANMath.DegToRad((double)numericUpDown07_02.Value);

			textBox07_01.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(transitions.Distance, eRoundMode.NEAREST).ToString();
			textBox07_01.Tag = textBox07_01.Text;

			textBox07_05.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.NomLineAltitude, eRoundMode.NEAREST).ToString();
			textBox07_05.Tag = textBox07_05.Text;

			//textBox07_04.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.NomLineGradient, eRoundMode.NEAREST).ToString();

			UpDateComboBox07_08();
		}

		private void comboBox07_01_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (radioButton07_01.Checked)
				return;

			if (!comboBox07_01.Enabled)
				return;

			int k = comboBox07_01.SelectedIndex;
			if (k < 0)
				return;

			transitions.DirectionIndex = k;

			WayPoint sigPoint = (WayPoint)comboBox07_01.SelectedItem;
			Point FixPt = _AddedFIX.NomLinePrjPt;

			label07_02.Text = sigPoint.Role.ToString();               // AIXMTypeNames[sigPoint.AIXMType];

			double newAzt, Direction;

			if (transitions.PathAndTermination == CodeSegmentPath.DF)
			{
				double fTurnDirection = -(int)(_AddedFIX.TurnDirection);
				double r = _AddedFIX.NomLineTurnRadius;
				Point ptCenter = ARANFunctions.LocalToPrj(FixPt, _AddedFIX.EntryDirection, 0, -r * fTurnDirection);

				//GlobalVars.gAranGraphics.DrawPointWithText(ptCenter, "ptCenter-2");
				//LegBase.ProcessMessages();

				double dX = sigPoint.PrjPt.X - ptCenter.X;
				double dY = sigPoint.PrjPt.Y - ptCenter.Y;

				double distDest = ARANMath.Hypot(dX, dY);
				double dirDest = Math.Atan2(dY, dX);

				double TurnAngle = (_AddedFIX.EntryDirection - dirDest) * fTurnDirection + ARANMath.C_PI_2 - Math.Acos(r / distDest);

				Direction = ARANMath.Modulus(_AddedFIX.EntryDirection - TurnAngle * fTurnDirection, ARANMath.C_2xPI);

				//Point ptFrom = ARANFunctions.LocalToPrj(ptCenter, Direction, 0.0, r * fTurnDirection);
				//GlobalVars.gAranGraphics.DrawPointWithText(ptFrom, "ptFrom");
				//LegBase.ProcessMessages();
			}
			else
				Direction = ARANMath.Modulus(Math.Atan2(sigPoint.PrjPt.Y - FixPt.Y, sigPoint.PrjPt.X - FixPt.X), ARANMath.C_2xPI);
			//??????????????????????????
			//else if ((_AddedFIX.FlyMode == eFlyMode.Flyover || _AddedFIX.FlyMode == eFlyMode.Atheight) && transitions.PathAndTermination == ePathAndTermination.CF)
			//{
			//}

			//if (FixPt != null)
			newAzt = ARANFunctions.DirToAzimuth(FixPt, Direction, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			//else
			//	newAzt = ARANFunctions.DirToAzimuth(ptMatp, Direction, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			if (numericUpDown07_01.Value != (decimal)newAzt)
			{
				numericUpDown07_01.Value = (decimal)newAzt;
				return;
			}

			transitions.OutDirection = Direction;

			textBox07_01.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(transitions.Distance, eRoundMode.NEAREST).ToString();
			textBox07_01.Tag = textBox07_01.Text;

			double TAS = ARANMath.IASToTAS(transitions.IAS, transitions.NomLineAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			textBox07_06.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.SPECIAL_NEAREST).ToString();

			textBox07_05.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.NomLineAltitude, eRoundMode.NEAREST).ToString();
			textBox07_05.Tag = textBox07_05.Text;

			//textBox07_04.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.NomLineGradient, eRoundMode.NEAREST).ToString();

			UpDateComboBox07_08();
		}

		private void comboBox07_02_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (transitions == null)
				return;

			double fTmp = transitions.OutDirection;

			transitions.turnDirection = comboBox07_02.SelectedIndex == 0 ? TurnDirection.CCW : TurnDirection.CW;
			if (fTmp != transitions.OutDirection)
			{
				double newAzt;
				//if (_AddedFIX != null && _AddedFIX.NomLinePrjPt != null)
				newAzt = ARANFunctions.DirToAzimuth(_AddedFIX.NomLinePrjPt, transitions.OutDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				//else
				//	newAzt = ARANFunctions.DirToAzimuth(maptPrj, transitions.OutDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

				numericUpDown07_01.Value = (decimal)newAzt;
			}
		}

		private void comboBox07_03_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!radioButton07_04.Checked)
				return;

			int k = comboBox07_03.SelectedIndex;
			if (k < 0)
				return;

			transitions.DistanceIndex = k;
			textBox07_02.Text = transitions.FIXName;

			WayPoint sigPoint = (WayPoint)comboBox07_03.SelectedItem;
			label07_05.Text = sigPoint.Role.ToString();

			if (radioButton07_03.Checked)   //????????????????
				return;                     //????????????????

			double dist;

			if (transitions.PathAndTermination == CodeSegmentPath.DF)
			{
				double fTurnDirection = (int)(_AddedFIX.EffectiveTurnDirection);
				double r = _AddedFIX.NomLineTurnRadius;
				Point ptCenter = ARANFunctions.LocalToPrj(_AddedFIX.NomLinePrjPt, _AddedFIX.EntryDirection, 0, r * fTurnDirection);
				Point ptFrom = ARANFunctions.LocalToPrj(ptCenter, _AddedFIX.OutDirection, 0.0, -r * fTurnDirection);

				//GlobalVars.gAranGraphics.DrawPointWithText(ptCenter, "ptCenter");
				//GlobalVars.gAranGraphics.DrawPointWithText(ptFrom, "ptFrom");
				//LegBase.ProcessMessages();

				dist = ARANMath.Hypot(sigPoint.PrjPt.Y - ptFrom.Y, sigPoint.PrjPt.X - ptFrom.X);
			}
			else
				dist = ARANMath.Hypot(sigPoint.PrjPt.Y - _AddedFIX.NomLinePrjPt.Y, sigPoint.PrjPt.X - _AddedFIX.NomLinePrjPt.X);

			bool isSame = false;
			if (radioButton07_02.Checked && radioButton07_04.Checked && _AddedFIX.FlyMode == eFlyMode.Atheight && transitions.PathAndTermination == CodeSegmentPath.CF)
			{
				WayPoint dirPoint = (WayPoint)comboBox07_01.SelectedItem;
				isSame = sigPoint.Id == dirPoint.Id;
			}

			if (isSame)
			{
				numericUpDown07_01.Enabled = true;
				numericUpDown07_01_ValueChanged(numericUpDown07_01, null);
			}
			else
				numericUpDown07_01.Enabled = radioButton07_01.Checked;

			transitions.Distance = dist;

			textBox07_05.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.NomLineAltitude, eRoundMode.NEAREST).ToString();
			textBox07_05.Tag = textBox07_05.Text;

			textBox07_01.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(transitions.Distance, eRoundMode.NEAREST).ToString();
			textBox07_01.Tag = textBox07_01.Text;

			UpDateComboBox07_08(true);
		}

		private void comboBox07_04_SelectedIndexChanged(object sender, EventArgs e)
		{
			checkBox07_01.Enabled = comboBox07_04.SelectedIndex == 1;
			if (transitions == null)
				return;

			transitions.SensorType = (eSensorType)comboBox07_04.SelectedIndex;
		}

		private void comboBox07_05_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (transitions == null)
				return;

			transitions.FlyMode = (eFlyMode)comboBox07_05.SelectedIndex;
		}

		private void comboBox07_06_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (transitions == null)
				return;

			transitions.PBNClass = (ePBNClass)comboBox07_06.SelectedItem;
		}

		private void comboBox07_07_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (transitions == null)
				return;

			if (comboBox07_07.SelectedIndex < 0)
				return;

			transitions.PathAndTermination = (CodeSegmentPath)comboBox07_07.SelectedItem;

			bool isSame = false;
			if (radioButton07_02.Checked && radioButton07_04.Checked && _AddedFIX.FlyMode == eFlyMode.Atheight && transitions.PathAndTermination == CodeSegmentPath.CF)
			{
				WayPoint dirPoint = (WayPoint)comboBox07_01.SelectedItem;
				WayPoint sigPoint = (WayPoint)comboBox07_03.SelectedItem;
				isSame = sigPoint.Id == dirPoint.Id;
			}

			numericUpDown07_01.Enabled = radioButton07_01.Checked || isSame;// (radioButton304.Checked && _AddedFIX.FlyMode == eFlyMode.Atheight && transitions.PathAndTermination == ePathAndTermination.CF);

			comboBox07_02.Visible = transitions.PathAndTermination == CodeSegmentPath.DF;
			label07_03.Visible = comboBox07_02.Visible;

			double nnAzt = ARANFunctions.DirToAzimuth(transitions.ReferenceFIX.NomLinePrjPt, transitions.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			if ((CodeSegmentPath)comboBox07_07.SelectedItem == CodeSegmentPath.DF)
			{
				transitions.turnDirection = comboBox07_02.SelectedIndex == 0 ? TurnDirection.CCW : TurnDirection.CW;
				numericUpDown07_01Range.Min = ARANMath.Modulus(nnAzt - 270.0, 360.0);
				numericUpDown07_01Range.Max = ARANMath.Modulus(nnAzt + 270.0, 360.0);
			}
			else
			{
				numericUpDown07_01Range.Min = ARANMath.Modulus(nnAzt - maxTurnAngle, 360.0);
				numericUpDown07_01Range.Max = ARANMath.Modulus(nnAzt + maxTurnAngle, 360.0);
			}
		}

		private void comboBox07_08_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (transitions == null)
				return;

			int k = comboBox07_08.SelectedIndex;
			if (k < 0)
				return;

			if (!bFormInitialised)
				return;

			if (transitions == null)
				return;

			if (transitions.BankAngle >= ARANMath.DegToRad(banks[k]))
				return;

			transitions.BankAngle = ARANMath.DegToRad(banks[k]);
		}

		private void comboBox07_09_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (transitions == null)
				return;
			_MOCLimit = EnrouteMOCValues[comboBox07_09.SelectedIndex];
			//_reportForm.FillPageCurrPage(transitions.CurrentObstacleList, transitions.CurrentDetObs);
			Report = true;
		}

		private void checkBox07_01_CheckedChanged(object sender, EventArgs e)
		{
			transitions.MultiCoverage = checkBox07_01.Checked;
		}

		private void button07_02_Click(object sender, EventArgs e)
		{
			screenCapture.Save(this);

			_reportForm.AddPage4(transitions.CurrentObstacleList, transitions.CurrentDetObs);

			transitions.UpdateEnabled = false;
			double prevDuration = transitions.Legs[transitions.Legs.Count - 1].Duration;

			if (!transitions.Add())
			{
				transitions.UpdateEnabled = true;
				return;
			}

			Report = true;

			LegBase leg = transitions.Legs[transitions.Legs.Count - 1];
			WayPoint EndFIX = (WayPoint)leg.EndFIX;
			WayPoint StartFIX = (WayPoint)leg.StartFIX;

			leg.Duration = prevDuration + leg.NominalTrack.Length / leg.StartFIX.NomLineTAS;
			textBox07_10.Text = (leg.Duration / 60.0).ToString("0.00");

			numericUpDown07_01Range.Max = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Min, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			numericUpDown07_01Range.Min = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Max, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			//if (transitions.LegPoints.Count == 2)
			//{
			//	comboBox07_06.Items.Clear();
			//	foreach (ePBNClass pbn in regularTurnPBNClass)
			//		comboBox07_06.Items.Add(pbn);
			//	comboBox07_06.SelectedIndex = 0;
			//}

			comboBox07_07.Items.Clear();

			if (leg.EndFIX.FlyMode == eFlyMode.Flyover)
			{
				comboBox07_07.Items.Add(CodeSegmentPath.TF);
				comboBox07_07.Items.Add(CodeSegmentPath.DF);
			}
			else
				comboBox07_07.Items.Add(CodeSegmentPath.TF);

			comboBox07_07.SelectedIndex = 0;

			textBox07_02.Text = transitions.FIXName;
			transitions.FlyMode = (eFlyMode)comboBox07_05.SelectedIndex;
			transitions.SensorType = (eSensorType)comboBox07_04.SelectedIndex;
			transitions.PBNClass = (ePBNClass)comboBox07_06.SelectedItem;
			transitions.PathAndTermination = (CodeSegmentPath)comboBox07_07.SelectedItem;

			if (radioButton07_02.Checked)
				comboBox07_01_SelectedIndexChanged(comboBox07_01, new EventArgs());

			if (radioButton07_04.Checked)
				comboBox07_03_SelectedIndexChanged(comboBox07_03, new EventArgs());

			button07_03.Enabled = true;
			Report = true;

			//textBox07_04.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.NomLineGradient, eRoundMode.NEAREST).ToString();

			transitions.UpdateEnabled = true;
		}

		private void button07_03_Click(object sender, EventArgs e)
		{
			transitions.UpdateEnabled = false;
			if (!transitions.Remove())
			{
				transitions.UpdateEnabled = true;
				return;
			}

			screenCapture.Delete();

			_reportForm.RemoveLastLegP4();

			numericUpDown07_01Range.Max = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Min, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			numericUpDown07_01Range.Min = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Max, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			_AddedFIX = transitions.LegPoints[transitions.LegPoints.Count - 1];
			int selectedInex;//, i;

			//if (transitions.LegPoints.Count == 1)
			//{
			//	selectedInex = i = 0;
			//	comboBox07_06.Items.Clear();

			//	foreach (ePBNClass pbn in firstTurnPBNClass)
			//	{
			//		comboBox07_06.Items.Add(pbn);
			//		if (transitions.PBNClass == pbn)
			//			selectedInex = i;

			//		i++;
			//	}

			//	comboBox07_06.SelectedIndex = selectedInex;

			//	button07_03.Enabled = false;
			//}

			selectedInex = 0;
			comboBox07_07.Items.Clear();

			if (_AddedFIX.FlyMode == eFlyMode.Flyover)
			{
				comboBox07_07.Items.Add(CodeSegmentPath.TF);
				comboBox07_07.Items.Add(CodeSegmentPath.DF);
				if (transitions.PathAndTermination == CodeSegmentPath.DF)
					selectedInex = 1;
			}
			else
				comboBox07_07.Items.Add(CodeSegmentPath.TF);

			comboBox07_07.SelectedIndex = selectedInex;

			//numericUpDown07_01Range.Max = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Min, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			//numericUpDown07_01Range.Min = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Max, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			double nnAzt = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			numericUpDown07_01.Value = (decimal)nnAzt;

			comboBox07_05.SelectedIndex = (int)transitions.FlyMode;
			comboBox07_04.SelectedIndex = (int)transitions.SensorType;
			comboBox07_06.SelectedItem = (int)transitions.PBNClass;
			comboBox07_07.SelectedItem = (int)transitions.PathAndTermination;

			transitions.UpdateEnabled = true;

			textBox07_10.Text = (transitions.Legs[transitions.Legs.Count - 1].Duration / 60.0).ToString("0.00");

			textBox07_01.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(transitions.Distance, eRoundMode.NEAREST).ToString();
			double TAS = ARANMath.IASToTAS(transitions.IAS, transitions.NomLineAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			textBox07_06.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.SPECIAL_NEAREST).ToString();

			textBox07_05.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.NomLineAltitude, eRoundMode.NEAREST).ToString();

			//textBox07_04.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.NomLineGradient, eRoundMode.NEAREST).ToString();
			textBox07_02.Text = transitions.FIXName;

			UpDateComboBox07_08(true);
		}

		private void textBox07_01_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox07_01_Validating(textBox07_01, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox07_01.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox07_01_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox07_01.Text, out fTmp))
				return;

			if (textBox07_01.Tag != null && textBox07_01.Tag.ToString() == textBox07_01.Text)
				return;

			double _Distance = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			transitions.Distance = _Distance;

			textBox07_01.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(transitions.Distance, eRoundMode.NEAREST).ToString();
			textBox07_01.Tag = textBox07_01.Text;

			textBox07_05.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.NomLineAltitude, eRoundMode.NEAREST).ToString();
			textBox07_05.Tag = textBox07_05.Text;

			//textBox07_04.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.NomLineGradient, eRoundMode.NEAREST).ToString();
			UpDateComboBox07_08(true);
		}

		private void textBox07_02_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (textBox07_02.ReadOnly)
				return;

			char eventChar = e.KeyChar;
			if (eventChar == 13)
			{
				textBox07_02_Validating(textBox07_02, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		private void textBox07_02_Validating(object sender, CancelEventArgs e)
		{
			if (textBox07_02.ReadOnly)
				return;

			textBox07_02.Text = textBox07_02.Text.ToUpper();
			transitions.FIXName = textBox07_02.Text;
			textBox07_02.Tag = textBox07_02.Text;
		}

		private void textBox07_03_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox07_03_Validating(textBox07_03, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox07_03.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox07_03_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox07_03.Text, out fTmp))
				return;

			if (textBox07_03.Tag != null && textBox07_03.Tag.ToString() == textBox07_03.Text)
				return;

			double fIAS = GlobalVars.unitConverter.SpeedToInternalUnits(fTmp);

			transitions.IAS = fIAS;

			if (transitions.IAS != fIAS)
				textBox07_03.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(transitions.IAS, eRoundMode.NEAREST).ToString();

			double TAS = ARANMath.IASToTAS(transitions.IAS, transitions.NomLineAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			textBox07_06.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS).ToString();

			textBox07_03.Tag = textBox07_03.Text;
		}

		//private void textBox07_04_KeyPress(object sender, KeyPressEventArgs e)
		//{
		//	char eventChar = e.KeyChar;
		//	if (eventChar == 13)
		//		textBox07_04_Validating(textBox07_04, new System.ComponentModel.CancelEventArgs());
		//	else
		//		Functions.TextBoxFloat(ref eventChar, textBox07_04.Text);

		//	e.KeyChar = eventChar;
		//	if (eventChar == 0)
		//		e.Handled = true;
		//}

		//private void textBox07_04_Validating(object sender, CancelEventArgs e)
		//{
		//	if (!double.TryParse(textBox07_04.Text, out double fTmp))
		//		return;

		//	if (textBox07_04.Tag != null && textBox07_04.Tag.ToString() == textBox07_04.Text)
		//		return;

		//	double _PDG = GlobalVars.unitConverter.GradientToInternalUnits(fTmp);
		//	transitions.NomLineGradient = _PDG;

		//	if (transitions.NomLineGradient != _PDG)
		//		textBox07_04.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.NomLineGradient, eRoundMode.NEAREST).ToString();

		//	textBox07_04.Tag = textBox07_04.Text;

		//	double TAS = ARANMath.IASToTAS(transitions.IAS, transitions.NomLineAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
		//	textBox07_06.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.SPECIAL_NEAREST).ToString();

		//	//UpDateComboBox07_08(true);
		//}

		#endregion

		#region Page 9

		ObstacleContainer DetObstacle;
		//double XReturnToNomPDG;
		//double AppliedhReturn;

		void ToPageIX()
		{
			_reportForm.AddPage4(transitions.CurrentObstacleList, transitions.CurrentDetObs);
			transitions.Add(true);

			//_RecalceAppliedPDG = _appliedPDG;
			GeometryOperators go = new GeometryOperators();
			int i, j, k = -1, n = transitions.Legs.Count;

			double NomPDG = GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;

			LegBase leg = transitions.Legs[0];

			DetObstacle.Obstacles = new Obstacle[1];
			DetObstacle.Parts = new ObstacleData[1];

			DetObstacle.Parts[0].PDG = leg.Gradient;
			double legPDG = NomPDG;
			GeometryOperators GGeoOperators = new GeometryOperators();

			//leg.Obstacles = segment1Term.InnerObstacleList;

			if (leg.EndFIX.FlyMode == eFlyMode.Atheight)
				leg.MinLegLength = ARANFunctions.ReturnDistanceInMeters(leg.StartFIX.PrjPt, leg.EndFIX.PrjPt);
			else
			{
				Point ptTmp = ARANFunctions.LocalToPrj(leg.EndFIX.PrjPt, leg.EndFIX.EntryDirection, -leg.EndFIX.EPT, 0);
				leg.MinLegLength = ARANFunctions.ReturnDistanceInMeters(leg.StartFIX.PrjPt, ptTmp);
			}

			//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			double finalOCA = _PreOCA;  //FMATAOCA 

			double fMOC30 = GlobalVars.constants.Pansops[ePANSOPSData.arMA_InterMOC].Value;
			double fMOC50 = GlobalVars.constants.Pansops[ePANSOPSData.arMA_FinalMOC].Value;
			double Dist, TIAMOC, fMOC;

			if ((eTurnAt)comboBox06_01.SelectedItem == eTurnAt.TP)
				TIAMOC = fMOC30;
			else
				TIAMOC = fMOC50;

			if (FMATF.TurnAngle <= GlobalVars.constants.Pansops[ePANSOPSData.arMATurnTrshAngl].Value)
				fMOC = fMOC30;
			else
				fMOC = fMOC50;

			//int IxTA = -1;

			Point point;
			if (FMATF.TurnAt == eTurnAt.TP)
				point = ((LineString)(FMATF.ReferenceGeometry))[0];
			else
				point = FMATF.PrjPt;

			double DistKKToSOC = ARANFunctions.PointToLineDistance(point, FSOC.PrjPt, _MACourse - 0.5 * Math.PI);   //F3ApproachDir

			//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			double legOCA = finalOCA;

			for (j = 0; j < transitions.Legs[0].Obstacles.Parts.Length; j++)
			{
				//if (transitions.Legs[0].Obstacles.Parts[j].Ignored)
				//	continue;
				//double tmpPDG = transitions.Legs[0].Obstacles.Parts[j].PDG;
				//if (tmpPDG > legPDG)
				//{
				//	legPDG = tmpPDG;
				//	k = j;
				//}

				//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

				Dist = GGeoOperators.GetDistance(transitions.Legs[0].Obstacles.Parts[j].pPtPrj, FMATF.ReferenceGeometry);
				transitions.Legs[0].Obstacles.Parts[j].Dist = Dist;

				transitions.Legs[0].Obstacles.Parts[j].ReqOCH = transitions.Legs[0].Obstacles.Parts[j].Elev +transitions.Legs[0].Obstacles.Parts[j].fSecCoeff * fMOC - (DistKKToSOC + Dist) * _MAClimb;
				if (transitions.Legs[0].Obstacles.Parts[j].ReqOCH > legOCA)
				{
					legOCA = transitions.Legs[0].Obstacles.Parts[j].ReqOCH;
					k = j;
				}
				//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			}

			//if (_RecalceAppliedPDG < legPDG)				_RecalceAppliedPDG = legPDG;

			if (k >= 0)
			{
				DetObstacle.Parts[0] = transitions.Legs[0].Obstacles.Parts[k];
				DetObstacle.Obstacles[0] = transitions.Legs[0].Obstacles.Obstacles[transitions.Legs[0].Obstacles.Parts[k].Owner];

				transitions.Legs[0].DetObstacle_1 = DetObstacle;
			}

			if (legOCA > transitions.Legs[0].Gradient)  //finalOCA
				transitions.Legs[0].Gradient = legOCA;
			else
			{
				DetObstacle.Obstacles[0].ID = -1;
				DetObstacle.Parts[0].ReqOCH = finalOCA;
			}

			for (i = 1; i < n; i++)
			{
				k = -1;
				legOCA = finalOCA;
				transitions.Legs[i].Obstacles = _reportForm.ObstaclesP4[i];
				leg = transitions.Legs[i];

				if (i == transitions.Legs.Count - 1)
					leg.MinLegLength = go.GetDistance(leg.KKLine, leg.EndFIX.PrjPt) + leg.EndFIX.ATT;
				else
					leg.MinLegLength = go.GetDistance(transitions.Legs[i + 1].KKLine, leg.KKLine);

				//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
				//Point point;
				//if (FMATF.TurnAt == eTurnAt.TP)
				//	point = ((LineString)(FMATF.ReferenceGeometry))[0];
				//else
				//	point = FMATF.PrjPt;

				//FMATAOCA = _PreOCA;
				//FMAFinalOCA = _PreOCA;
				//int IxTA = -1;

				//FMAFinalOCA + DistKKToSOC * _MAClimb
				//Dist = GGeoOperators.GetDistance(FMAHFturn.PrjPt, FMATF.ReferenceGeometry) + DistKKToSOC;
				//prmTerminationAltitude = FMAFinalOCA + Dist * _MAClimb;

				//GGeoOperators.CurrentGeometry = transitions.Legs[i].KKLine;

				for (j = 1; j < transitions.Legs[i].Obstacles.Parts.Length; j++)
				{
					//Dist = GGeoOperators.GetDistance(transitions.Legs[i].Obstacles.Parts[j].pPtPrj);
					transitions.Legs[i].Obstacles.Parts[j].Dist = Dist= transitions.Legs[i].Obstacles.Parts[j].d0;


					transitions.Legs[i].Obstacles.Parts[j].ReqOCH = transitions.Legs[i].Obstacles.Parts[j].Elev + transitions.Legs[i].Obstacles.Parts[j].fSecCoeff * fMOC - (leg.MinLegLength + Dist) * _MAClimb;
					if (transitions.Legs[i].Obstacles.Parts[j].ReqOCH > finalOCA)
					{
						finalOCA = transitions.Legs[i].Obstacles.Parts[j].ReqOCH;
						k = j;
					}
				}

				//for (j = 0; j < transitions.Legs[i].Obstacles.Parts.Length; j++)
				//{
				//	double tmpPDG = transitions.Legs[i].Obstacles.Parts[j].PDG;
				//	if (tmpPDG > legPDG)
				//	{
				//		legPDG = tmpPDG;
				//		k = j;
				//	}
				//}

				//if (_RecalceAppliedPDG < legPDG)					_RecalceAppliedPDG = legPDG;
				//transitions.Legs[i].Gradient = legPDG;

				if (k >= 0)
				{
					transitions.Legs[i].DetObstacle_1.Parts[0] = transitions.Legs[i].Obstacles.Parts[k];
					transitions.Legs[i].DetObstacle_1.Obstacles[0] = transitions.Legs[i].Obstacles.Obstacles[transitions.Legs[i].Obstacles.Parts[k].Owner];

					if (DetObstacle.Parts[0].ReqOCH < transitions.Legs[i].DetObstacle_1.Parts[0].ReqOCH)
					{
						DetObstacle.Parts[0] = transitions.Legs[i].DetObstacle_1.Parts[0];
						DetObstacle.Obstacles[0] = transitions.Legs[i].DetObstacle_1.Obstacles[0];
					}
				}
			}

			/*==============================================================================================================================*/
			//MinPDG = _RecalceAppliedPDG;
			//textBox401.Text = GlobalVars.unitConverter.GradientToDisplayUnits(MinPDG, eRoundMode.CEIL).ToString();

			//double hObovDer = GlobalVars.constants.Pansops[ePANSOPSData.dpH_abv_DER].Value;
			//double hReturn;

			//if (FMATF.TurnAt == eTurnAt.Altitude)
			//{
			//	hReturn = transitions.Legs[0].MinLegLength * MinPDG + _refElevation + hObovDer;        // HeightAtEndFIX + ptDerPrj.Z;	// Math.Max(hReturn , HeightAtEndFIX);
			//	XReturnToNomPDG = transitions.Legs[0].MinLegLength;
			//}
			//else
			//{
			//	hReturn = _refElevation + GlobalVars.constants.Pansops[ePANSOPSData.dpGui_Ar1].Value;
			//	XReturnToNomPDG = (hReturn - _refElevation - hObovDer) / MinPDG;
			//}

			////AppliedhReturn = hReturn;
			////AppliedXReturnToNomPDG = XReturnToNomPDG;
			////if (_RecalceAppliedPDG > _appliedPDG)
			////ObstacleData tmpObst = DetObstacle.Parts[0];

			//double AppliedXReturnToNomPDG = XReturnToNomPDG;

			//if (MinPDG != NomPDG)
			//{
			//	for (i = 0; i < n; i++)
			//	{
			//		leg = transitions.Legs[i];
			//		int m = leg.Obstacles.Parts.Length;

			//		for (j = 0; j < m; j++)
			//		{
			//			ObstacleData CurrObst = transitions.Legs[i].Obstacles.Parts[j];

			//			double tmpXReturnToNomPDG = (CurrObst.ReqH - hObovDer - CurrObst.DistStar * NomPDG) / (MinPDG - NomPDG);    /*-ptDerPrj.Z*/

			//			if (tmpXReturnToNomPDG > XReturnToNomPDG)
			//				XReturnToNomPDG = tmpXReturnToNomPDG;
			//		}
			//	}

			//	AppliedXReturnToNomPDG = XReturnToNomPDG;
			//	AppliedhReturn = AppliedXReturnToNomPDG * MinPDG + hObovDer + _refElevation;
			//	textBox402.Text = GlobalVars.unitConverter.HeightToDisplayUnits(AppliedhReturn, eRoundMode.NEAREST).ToString();
			//}
			//else
			//{
			//	AppliedhReturn = AppliedXReturnToNomPDG * MinPDG + hObovDer + _refElevation;
			//	textBox402.Text = "-";
			//}

			/*==============================================================================================================================*/

			//double kkSum = 0;
			double nomSum = 0;
			//bool trigger = false, trigger0 = false;

			dataGridView08_01.RowCount = 0;
			//_reportForm.ClearPage5Obstacles();

			for (i = 0; i < n; i++)
			{
				leg = transitions.Legs[i];

				int m = leg.Obstacles.Parts.Length;

				//for (j = 0; j < m; j++)
				//{
				//	ObstacleData CurrObst = transitions.Legs[i].Obstacles.Parts[j];

				//	if (CurrObst.DistStar < AppliedXReturnToNomPDG)
				//		leg.Obstacles.Parts[j].EffectiveHeight = CurrObst.DistStar * MinPDG + hObovDer;
				//	else
				//		leg.Obstacles.Parts[j].EffectiveHeight = AppliedXReturnToNomPDG * MinPDG + (CurrObst.DistStar - AppliedXReturnToNomPDG) * NomPDG + hObovDer;
				//}

				//_reportForm.AddPage5(leg.Obstacles, leg.DetObstacle_1);

				DataGridViewRow row = new DataGridViewRow();

				row.Tag = transitions.Legs[i];
				row.ReadOnly = false;

				//kkSum += leg.MinLegLength;
				//double trDist = leg.Length;
				double trDist;
				double Re = leg.EndFIX.NomLineTurnRadius;
				double alpe = 0.5 * leg.EndFIX.TurnAngle;

				if (leg.EndFIX.IsDFTarget || leg.StartFIX.FlyMode == eFlyMode.Atheight)
					trDist = leg.Length + Re * alpe;
				else
				{
					double Rs = leg.StartFIX.NomLineTurnRadius;
					double alps = 0.5 * leg.StartFIX.TurnAngle;

					trDist = leg.Length - Rs * Math.Tan(alps) + Rs * alps;

					if (leg.EndFIX.FlyMode == eFlyMode.Flyby)
						trDist += Re * alpe - Re * Math.Tan(alpe);
				}

				nomSum += trDist;

				//double altWPT = nomSum * MinPDG + hObovDer + _refElevation;

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = (i + 1).ToString();
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[1].Value = leg.EndFIX.Name;

				cell = new DataGridViewTextBoxCell();
				cell.Value = leg.PathAndTermination;
				row.Cells.Add(cell);

				//==================================================//

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = leg.EndFIX.FlyMode;

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = leg.EndFIX.PBNType;

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = GlobalVars.unitConverter.SpeedToDisplayUnits(leg.EndFIX.IAS, eRoundMode.SPECIAL_NEAREST);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = leg.EndFIX.TurnDirection == TurnDirection.CW ? "Right" : "Left";

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[7].Value = Math.Round(GlobalVars.CurrADHP.MagVar, 2).ToString() + "°";

				row.Cells.Add(new DataGridViewTextBoxCell());
				double Val = ARANFunctions.DirToAzimuth(leg.EndFIX.PrjPt, leg.EndFIX.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				double Val1 = ARANMath.Modulus(Val - GlobalVars.CurrADHP.MagVar);
				row.Cells[8].Value = Math.Round(Val, 2).ToString() + "° (" + Math.Round(Val1, 2).ToString() + "°)";

				row.Cells.Add(new DataGridViewTextBoxCell());

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[10].Value = GlobalVars.unitConverter.HeightToDisplayUnits(leg.EndFIX.ConstructAltitude, eRoundMode.NEAREST);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[11].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(leg.MinLegLength, eRoundMode.NEAREST);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[12].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(trDist, eRoundMode.NEAREST);

				//==================================================//
				row.Cells.Add(new DataGridViewTextBoxCell());

				row.Cells.Add(new DataGridViewTextBoxCell());

				//double dDist = leg.Length - leg.MinLegLength;   // -leg.EndFIX.EPT;

				//if (altWPT <= AppliedhReturn)
				//{
				//	leg.Altitude = altWPT;
				//	leg.Gradient = MinPDG;

				//	row.Cells[9].Value = GlobalVars.unitConverter.HeightToDisplayUnits(altWPT, eRoundMode.NEAREST);

				//	row.Cells[13].Value = GlobalVars.unitConverter.GradientToDisplayUnits(MinPDG, eRoundMode.SPECIAL_NEAREST);
				//	row.Cells[14].Value = Math.Round(ARANMath.RadToDeg(Math.Atan(MinPDG)), 2);
				//}
				//else if (!trigger)
				//{
				//	altWPT = AppliedXReturnToNomPDG * MinPDG + hObovDer + _refElevation + (nomSum - AppliedXReturnToNomPDG) * NomPDG;

				//	leg.Altitude = altWPT;
				//	leg.Gradient = NomPDG;

				//	row.Cells[9].Value = GlobalVars.unitConverter.HeightToDisplayUnits(altWPT, eRoundMode.NEAREST);

				//	row.Cells[13].Value = GlobalVars.unitConverter.GradientToDisplayUnits(MinPDG, eRoundMode.SPECIAL_NEAREST) + "/" + GlobalVars.unitConverter.GradientToDisplayUnits(NomPDG, eRoundMode.SPECIAL_NEAREST);
				//	row.Cells[14].Value = Math.Round(ARANMath.RadToDeg(Math.Atan(MinPDG)), 2) + "/" + Math.Round(ARANMath.RadToDeg(Math.Atan(NomPDG)), 2);
				//	trigger = true;
				//}
				//else
				//{
				//	altWPT = AppliedXReturnToNomPDG * MinPDG + hObovDer + _refElevation + (nomSum - AppliedXReturnToNomPDG) * NomPDG;

				//	if (altWPT <= GlobalVars.MaxAltitude)
				//	{
				//		leg.Altitude = altWPT;
				//		leg.Gradient = NomPDG;

				//		row.Cells[9].Value = GlobalVars.unitConverter.HeightToDisplayUnits(altWPT, eRoundMode.NEAREST);

				//		row.Cells[13].Value = GlobalVars.unitConverter.GradientToDisplayUnits(NomPDG, eRoundMode.SPECIAL_NEAREST);
				//		row.Cells[14].Value = Math.Round(ARANMath.RadToDeg(Math.Atan(NomPDG)), 2);
				//	}
				//	else if (!trigger0)
				//	{
				//		altWPT = GlobalVars.MaxAltitude;

				//		leg.Altitude = altWPT;
				//		leg.Gradient = 0.0;

				//		row.Cells[9].Value = GlobalVars.unitConverter.HeightToDisplayUnits(GlobalVars.MaxAltitude, eRoundMode.NEAREST);

				//		row.Cells[13].Value = GlobalVars.unitConverter.GradientToDisplayUnits(NomPDG, eRoundMode.SPECIAL_NEAREST) + "/ 0.0";
				//		row.Cells[14].Value = Math.Round(ARANMath.RadToDeg(Math.Atan(NomPDG)), 2) + "/ 0.0";
				//		trigger0 = true;
				//	}
				//	else
				//	{
				//		altWPT = GlobalVars.MaxAltitude;

				//		leg.Altitude = altWPT;
				//		leg.Gradient = 0.0;

				//		row.Cells[9].Value = GlobalVars.unitConverter.HeightToDisplayUnits(GlobalVars.MaxAltitude, eRoundMode.NEAREST);

				//		row.Cells[13].Value = "0.0";
				//		row.Cells[14].Value = "0.0";
				//	}
				//}

				//leg.Altitude = altWPT;
				dataGridView08_01.Rows.Add(row);
			}

		}

		private double ConvertTracToPoints(out ReportPoint[] GuidPoints)
		{
			double result = 0.0;
			int n = 8;
			if (checkBox05_01.Checked)
				n += transitions.Legs.Count;

			GuidPoints = new ReportPoint[n];

			//			_intermediateLeq
			//_finalSDF1Leg
			//_finalSDF2Leg
			//_finalLeq

			//_straightMissedLeq
			//_straightTIALeq

			////_turnLeg
			//			transitions.Legs;

			int i = 0;
			// IF
			LegBase leg = _intermediateLeq;
			GuidPoints[0].TrueCourse = ARANFunctions.DirToAzimuth(leg.EndFIX.PrjPt, leg.EndFIX.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			GuidPoints[0].DistToNext = leg.Length;
			GuidPoints[0].Height = leg.EndFIX.ConstructAltitude;         //leg.UpperLimit;//-1.0;        // 
			GuidPoints[0].Radius = -1.0;

			GuidPoints[0].Description = leg.StartFIX.Name;
			GuidPoints[0].Lat = leg.StartFIX.GeoPt.Y;
			GuidPoints[0].Lon = leg.StartFIX.GeoPt.X;
			//result += leg.Length;
			result += leg.NominalTrack.Length;

			if(_IntermSDFCnt>=1)
			{
				i++;
				leg = _IntermSDF1Leg;
				GuidPoints[i].TrueCourse = ARANFunctions.DirToAzimuth(leg.EndFIX.PrjPt, leg.EndFIX.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				GuidPoints[i].DistToNext = leg.Length;
				GuidPoints[i].Height = leg.EndFIX.ConstructAltitude;         //-1.0;        // leg.UpperLimit;
				GuidPoints[i].Radius = -1.0;

				GuidPoints[i].Description = leg.StartFIX.Name;
				GuidPoints[i].Lat = leg.StartFIX.GeoPt.Y;
				GuidPoints[i].Lon = leg.StartFIX.GeoPt.X;
				//result += leg.Length;
				result += leg.NominalTrack.Length;
			}

			if (_IntermSDFCnt > 1)
			{
				i++;
				leg = _IntermSDF2Leg;
				GuidPoints[i].TrueCourse = ARANFunctions.DirToAzimuth(leg.EndFIX.PrjPt, leg.EndFIX.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				GuidPoints[i].DistToNext = leg.Length;
				GuidPoints[i].Height = leg.EndFIX.ConstructAltitude;// leg.Altitude;         //-1.0;        // leg.UpperLimit;
				GuidPoints[i].Radius = -1.0;

				GuidPoints[i].Description = leg.StartFIX.Name;
				GuidPoints[i].Lat = leg.StartFIX.GeoPt.Y;
				GuidPoints[i].Lon = leg.StartFIX.GeoPt.X;
				//result += leg.Length;
				result += leg.NominalTrack.Length;
			}

			if (_FinalSDFCnt >= 1)
			{
				i++;
				leg = _finalSDF2Leg;
				GuidPoints[i].TrueCourse = ARANFunctions.DirToAzimuth(leg.EndFIX.PrjPt, leg.EndFIX.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				GuidPoints[i].DistToNext = leg.Length;
				GuidPoints[i].Height = leg.EndFIX.ConstructAltitude;// -1.0;        // leg.UpperLimit;
				GuidPoints[i].Radius = -1.0;

				GuidPoints[i].Description = leg.StartFIX.Name;
				GuidPoints[i].Lat = leg.StartFIX.GeoPt.Y;
				GuidPoints[i].Lon = leg.StartFIX.GeoPt.X;
				//result += leg.Length;
				result += leg.NominalTrack.Length;
			}

			if (_FinalSDFCnt > 1)
			{
				i++;
				leg = _finalSDF1Leg;
				GuidPoints[i].TrueCourse = ARANFunctions.DirToAzimuth(leg.EndFIX.PrjPt, leg.EndFIX.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				GuidPoints[i].DistToNext = leg.Length;
				GuidPoints[i].Height = leg.EndFIX.ConstructAltitude;// -1.0;        // leg.UpperLimit;
				GuidPoints[i].Radius = -1.0;

				GuidPoints[i].Description = leg.StartFIX.Name;
				GuidPoints[i].Lat = leg.StartFIX.GeoPt.Y;
				GuidPoints[i].Lon = leg.StartFIX.GeoPt.X;
				//result += leg.Length;
				result += leg.NominalTrack.Length;
			}

			// FAF
			i++;
			leg = _finalLeq;
			GuidPoints[i].TrueCourse = ARANFunctions.DirToAzimuth(leg.EndFIX.PrjPt, leg.EndFIX.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			GuidPoints[i].DistToNext = leg.Length;
			GuidPoints[i].Height = leg.EndFIX.ConstructAltitude;// -1.0;        // leg.UpperLimit;
			GuidPoints[i].Radius = -1.0;

			GuidPoints[i].Description = leg.StartFIX.Name;
			GuidPoints[i].Lat = leg.StartFIX.GeoPt.Y;
			GuidPoints[i].Lon = leg.StartFIX.GeoPt.X;
			//result += leg.Length;
			result += leg.NominalTrack.Length;

			// MAPt
			i++;
			leg = _straightMissedLeq;
			GuidPoints[i].TrueCourse = ARANFunctions.DirToAzimuth(leg.EndFIX.PrjPt, leg.EndFIX.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			GuidPoints[i].DistToNext = leg.Length;
			GuidPoints[i].Height = leg.EndFIX.ConstructAltitude;// leg.Altitude;         //-1.0; // leg.UpperLimit;	
			GuidPoints[i].Radius = -1.0;

			GuidPoints[i].Description = leg.StartFIX.Name;
			GuidPoints[i].Lat = leg.StartFIX.GeoPt.Y;
			GuidPoints[i].Lon = leg.StartFIX.GeoPt.X;
			//result += leg.Length;
			result += leg.NominalTrack.Length;

			if (checkBox05_01.Checked)
			{
				// MATF
				for (int j = 1; j < transitions.Legs.Count; j++)
				{
					i++;
					leg = transitions.Legs[j];
					GuidPoints[i].TrueCourse = ARANFunctions.DirToAzimuth(leg.EndFIX.PrjPt, leg.EndFIX.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
					GuidPoints[i].DistToNext = leg.Length;
					GuidPoints[i].Height = leg.EndFIX.ConstructAltitude;	// -1.0;    // leg.Altitude; // leg.UpperLimit; 
					GuidPoints[i].Radius = -1.0;

					GuidPoints[i].Description = leg.StartFIX.Name;
					GuidPoints[i].Lat = leg.StartFIX.GeoPt.Y;
					GuidPoints[i].Lon = leg.StartFIX.GeoPt.X;
					//result += leg.Length;
					result += leg.NominalTrack.Length;
				}

				i++;
				GuidPoints[i].TrueCourse = ARANFunctions.DirToAzimuth(leg.EndFIX.PrjPt, leg.EndFIX.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				GuidPoints[i].DistToNext = -1;
				GuidPoints[i].Height = leg.EndFIX.ConstructAltitude;// -1.0;    // leg.Altitude; //leg.UpperLimit; 
				GuidPoints[i].Radius = -1.0;

				GuidPoints[i].Description = leg.EndFIX.Name;
				GuidPoints[i].Lat = leg.EndFIX.GeoPt.Y;
				GuidPoints[i].Lon = leg.EndFIX.GeoPt.X;
			}

			Array.Resize(ref GuidPoints, i + 1);

			//// MATF
			//GuidPoints[3].TrueCourse = ARANFunctions.DirToAzimuth(leg.EndFIX.PrjPt, leg.EndFIX.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			//GuidPoints[3].DistToNext = -1.0;
			//GuidPoints[3].Height = -1.0;        // leg.UpperLimit;
			//GuidPoints[3].Radius = -1.0;

			//GuidPoints[3].Description = leg.EndFIX.Name;
			//GuidPoints[3].Lat = leg.EndFIX.GeoPt.Y;
			//GuidPoints[3].Lon = leg.EndFIX.GeoPt.X;

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

			obstacleReport.ReferenceElevation = _refElevation;


			obstacleReport.SaveObstacles1(Resources.str10010, _reportForm.dataGridView01, GlobalVars.constants.AircraftCategory[aircraftCategoryData.arObsClearance].Value[_AirCat] );

			if (_FinalSDFCnt >= 1)
				obstacleReport.SaveObstacles2_4(Resources.str10014, _reportForm.dataGridView02, _reportForm.obstaclesPage02);
			if (_FinalSDFCnt == 2)
				obstacleReport.SaveObstacles2_4(Resources.str10015, _reportForm.dataGridView03, _reportForm.obstaclesPage03);

			obstacleReport.SaveObstacles2_4(Resources.str10013, _reportForm.dataGridView04, _reportForm.obstaclesPage04);

			obstacleReport.SaveObstacles5_8(Resources.str10012, _reportForm.dataGridView05, _reportForm.obstaclesPage05, true);
			if (_IntermSDFCnt >= 1)
				obstacleReport.SaveObstacles5_8(Resources.str10016, _reportForm.dataGridView06, _reportForm.obstaclesPage06);
			if (_IntermSDFCnt == 2)
				obstacleReport.SaveObstacles5_8(Resources.str10017, _reportForm.dataGridView07, _reportForm.obstaclesPage07);

			obstacleReport.SaveObstacles5_8(Resources.str10011, _reportForm.dataGridView08, _reportForm.obstaclesPage08);

			obstacleReport.SaveObstacles9(Resources.str10018, _reportForm.dataGridView09, _reportForm.obstaclesPage09);

			if (!checkBox05_01.Checked)
			{
				obstacleReport.CloseFile();
				return;
			}

			obstacleReport.SaveObstacles10(Resources.str10019, _reportForm.dataGridView10, _reportForm.obstaclesPage10);
			obstacleReport.SaveObstacles11(Resources.str10007, _reportForm.dataGridView11, _reportForm.obstaclesPage11);

			for (int i = 0; i < _reportForm.obstaclesPage13.Count; i++)
				obstacleReport.SaveObstacles13(Resources.str10009 + " " + i, _reportForm.dataGridView13, _reportForm.obstaclesPage13[i]);

			//_reportForm.SortForSave();

			//obstacleReport.WriteTab(_reportForm.dataGridView01, _reportForm.GetTabPageText(0));
			//obstacleReport.WriteTab(_reportForm.listView2, _reportForm.GetTabPageText(1));

			obstacleReport.CloseFile();
		}

		private void SaveLog(string RepFileName, string RepFileTitle, ReportHeader pReport)
		{
			ReportFile logReport = new ReportFile();

			logReport.OpenFile(RepFileName + "_Log", Resources.str00124);

			logReport.WriteString(Resources.str00033 + " - " + Resources.str00124, true);
			logReport.WriteString();
			logReport.WriteString(RepFileTitle, true);

			logReport.WriteHeader(pReport);

			//====================================================================================
			logReport.WriteString();
			logReport.WriteString();

			logReport.ExH2(tabControl1.TabPages[0].Text);
			logReport.HTMLString("[ " + tabControl1.TabPages[0].Text + " ]", true, false);
			logReport.WriteString();
			if (radioButton00_01.Checked)
				logReport.Param(Resources.str00130, radioButton00_01.Text);
			else
				logReport.Param(Resources.str00130, radioButton00_02.Text);

			logReport.Param(label00_02.Text, comboBox00_02.Text);

			logReport.Param(label00_03.Text, textBox00_01.Text, label00_04.Text);
			logReport.Param(label00_05.Text, textBox00_02.Text, label00_06.Text);
			logReport.Param(label00_07.Text, textBox00_03.Text, label00_08.Text);
			logReport.Param(label00_09.Text, textBox00_04.Text, label00_10.Text);
			logReport.Param(label00_11.Text, comboBox00_03.Text);

			logReport.Param(label00_12.Text, ".");


			for (int i = 0; i < listView00_01.Items.Count; i++)
			{
				if (listView00_01.Items[i].Checked)
					logReport.Param(listView00_01.Columns[0].Text, listView00_01.Items[i].Text);
			}

			//====================================================================================
			logReport.WriteString();
			logReport.WriteString();

			logReport.ExH2(tabControl1.TabPages[1].Text);
			logReport.HTMLString("[ " + tabControl1.TabPages[1].Text + " ]", true, false);
			logReport.WriteString();

			logReport.WriteString(tabControl01_01.SelectedTab.Text);

			if (tabControl01_01.SelectedIndex == 0)
			{
				if (radioButton01_01.Checked)
					logReport.Param(groupBox01_01.Text, radioButton01_01.Text);
				else
					logReport.Param(groupBox01_01.Text, radioButton01_02.Text);

				logReport.Param(label01_05.Text, textBox01_02.Text, label01_06.Text);

				logReport.WriteString(groupBox01_02.Text);
				//logReport.Param(groupBox01_02.Text, radioButton01_03.Text);
				//logReport.Param(groupBox01_02.Text, radioButton01_04.Text);
				if (radioButton01_04.Checked)
					logReport.Param(radioButton01_04.Text, comboBox01_01.Text, label01_04.Text);

				logReport.Param(radioButton01_03.Text, textBox01_01.Text, label01_02.Text);

				logReport.WriteString(groupBox01_04.Text);
				//logReport.Param(groupBox01_04.Text, radioButton01_05.Text);
				if (radioButton01_06.Checked)
					logReport.Param(radioButton01_06.Text, comboBox01_03.Text, label01_09.Text);

				logReport.Param(radioButton01_05.Text, textBox01_03.Text, comboBox01_02.Text);
			}
			else
			{
				logReport.Param(label01_14.Text, comboBox01_04.Text, label01_15.Text);
				logReport.Param(label01_16.Text, textBox01_06.Text, label01_17.Text);

				logReport.WriteString(groupBox01_06.Text);
				logReport.Param(label01_18.Text, comboBox01_05.Text);
				if (radioButton01_07.Checked)
					logReport.Param(radioButton01_07.Text, numericUpDown01_01.Text, label01_19.Text);
				else
					logReport.Param(radioButton01_08.Text, comboBox01_06.Text, label01_20.Text);

				logReport.WriteString(label01_21.Text);

				//dataGridView01_01
			}

			logReport.Param(label01_10.Text, textBox01_04.Text, label01_11.Text);
			logReport.Param(label01_12.Text, textBox01_05.Text, label01_13.Text);

			//if(label02029.Visible)
			//{
			//		logReport.WriteString(label02029.Text, true);
			//}

			//====================================================================================
			logReport.WriteString();
			logReport.WriteString();

			logReport.ExH2(tabControl1.TabPages[2].Text);
			logReport.HTMLString("[ " + tabControl1.TabPages[2].Text + " ]", true, false);
			logReport.WriteString();

			logReport.WriteString(groupBox02_01.Text);
			if (radioButton02_01.Checked)
				logReport.Param(radioButton02_01.Text, textBox02_01.Text, label02_01.Text);
			else if (radioButton02_02.Checked)
				logReport.Param(radioButton02_02.Text, textBox02_02.Text, label02_03.Text);
			else
				logReport.Param(radioButton02_03.Text, comboBox02_01.Text);

			logReport.Param(label02_04.Text, textBox02_04.Text, label02_05.Text);
			logReport.Param(label02_06.Text, textBox02_05.Text, label02_07.Text);
			logReport.Param(comboBox02_02.Text, textBox02_06.Text, label02_08.Text);
			logReport.Param(label02_09.Text, textBox02_07.Text);

			if (radioButton02_04.Checked)
				logReport.Param(groupBox02_02.Text, radioButton02_04.Text);
			else
			{
				logReport.WriteString(groupBox02_02.Text);
				logReport.Param(radioButton02_04.Text, checkBox02_01.Text);
			}

			logReport.Param(label02_11.Text, comboBox02_03.Text);
			logReport.Param(label02_12.Text, textBox02_08.Text, label02_13.Text);

			if (radioButton02_06.Checked)
			{
				logReport.Param(groupBox02_03.Text, radioButton02_06.Text);
				logReport.Param(label02_14.Text, textBox02_09.Text, label02_15.Text);
				logReport.Param(label02_18.Text, textBox02_11.Text, label02_19.Text);
			}
			else
			{
				logReport.Param(groupBox02_03.Text, radioButton02_07.Text);
				logReport.Param(label02_18.Text, comboBox02_04.Text, label02_19.Text);
			}

			logReport.Param(label02_16.Text, textBox02_10.Text, label02_17.Text);
			logReport.Param(label02_20.Text, textBox02_12.Text, label02_21.Text);
			logReport.Param(label02_22.Text, textBox02_13.Text, label02_23.Text);

			//checkBox02_02
			//checkBox02_03

			//====================================================================================
			if (_FinalSDFCnt > 0)
			{
				logReport.WriteString();
				logReport.WriteString();

				logReport.ExH2(tabControl1.TabPages[3].Text);
				logReport.HTMLString("[ " + tabControl1.TabPages[3].Text + " ]", true, false);
				logReport.WriteString();
				logReport.WriteString(groupBox03_01.Text);

				if (radioButton03_01.Checked)
					logReport.Param(radioButton03_01.Text, textBox03_01.Text, label03_01.Text);
				else if (radioButton03_02.Checked)
					logReport.Param(radioButton03_02.Text, textBox03_02.Text, label03_03.Text);
				else
					logReport.Param(radioButton03_03.Text, comboBox03_01.Text, label03_02.Text);

				logReport.Param(comboBox03_02.Text, textBox03_04.Text, label03_04.Text);
				logReport.Param(label03_05.Text, textBox03_05.Text);

				//logReport.Param(label03011.Text, textBox03006.Text, label03012.Text);
				//logReport.Param(label03011.Text, textBox03006.Text, label03012.Text);
			}
			//====================================================================================
			if (_IntermSDFCnt > 0)
			{
				logReport.WriteString();
				logReport.WriteString();

				logReport.ExH2(tabControl1.TabPages[4].Text);
				logReport.HTMLString("[ " + tabControl1.TabPages[4].Text + " ]", true, false);
				logReport.WriteString();
				logReport.WriteString(groupBox04_01.Text);

				if (radioButton04_01.Checked)
				{
					logReport.WriteString(radioButton04_01.Text);
					logReport.Param(label04_01.Text, textBox04_01.Text, label04_02.Text);
					logReport.Param(label04_03.Text, textBox04_02.Text);
				}
				else
				{
					logReport.Param(radioButton04_02.Text, comboBox04_01.Text, label04_04.Text);
				}

				logReport.Param(label04_05.Text, textBox04_03.Text, label04_06.Text);
				logReport.WriteString();

				logReport.Param(comboBox04_02.Text, textBox04_04.Text, label04_07.Text);
				logReport.Param(label04_08.Text, textBox04_05.Text);
			}
			//====================================================================================
			logReport.WriteString();
			logReport.WriteString();

			logReport.ExH2(tabControl1.TabPages[5].Text);
			logReport.HTMLString("[ " + tabControl1.TabPages[5].Text + " ]", true, false);
			logReport.WriteString();

			logReport.WriteString(groupBox05_01.Text);

			if (radioButton05_01.Checked)
			{
				logReport.Param(radioButton05_01.Text, textBox05_01.Text, label05_01.Text);
				logReport.Param(label05_04.Text, textBox05_03.Text);
			}
			else
				logReport.Param(radioButton05_02.Text, comboBox05_01.Text);

			if (checkBox05_03.Checked)
				logReport.WriteString(checkBox05_03.Text);
			logReport.Param(label05_02.Text, textBox05_02.Text, label05_03.Text);
			logReport.WriteString();

			logReport.Param(label05_05.Text, textBox05_04.Text, label05_06.Text);

			logReport.Param(label05_05.Text, textBox05_04.Text, label05_06.Text);
			logReport.Param(label05_07.Text, textBox05_05.Text, label05_08.Text);
			logReport.Param(label05_09.Text, textBox05_06.Text, label05_10.Text);
			logReport.Param(label05_11.Text, textBox05_07.Text, label05_12.Text);
			logReport.Param(label05_13.Text, textBox05_08.Text, label05_14.Text);
			logReport.Param(label05_15.Text, textBox05_09.Text, label05_16.Text);
			logReport.Param(label05_17.Text, textBox05_10.Text);
			logReport.Param(label05_18.Text, textBox05_11.Text);
			logReport.Param(label05_19.Text, textBox05_12.Text);

			if (radioButton05_03.Checked)
				logReport.WriteString(radioButton05_03.Text);
			else
			{
				logReport.WriteString(radioButton05_04.Text);
				if (checkBox05_02.Checked)
					logReport.WriteString(checkBox05_02.Text);
			}

			logReport.Param(label05_20.Text, comboBox05_02.Text);

			logReport.Param(label05_21.Text, textBox05_13.Text, label05_22.Text);
			logReport.Param(label05_23.Text, textBox05_14.Text, label05_24.Text);

			logReport.WriteString();
			logReport.WriteString(groupBox05_03.Text);
			logReport.Param(label05_25.Text, textBox05_15.Text, label05_26.Text);
			logReport.Param(label05_27.Text, textBox05_16.Text);
			logReport.Param(label05_28.Text, textBox05_17.Text, label05_29.Text);
			if (checkBox05_01.Checked)
				logReport.WriteString(checkBox05_01.Text);
			else
			{
				logReport.CloseFile();
				return;
			}
			//====================================================================================
			logReport.WriteString();
			logReport.WriteString();

			logReport.ExH2(tabControl1.TabPages[6].Text);
			logReport.HTMLString("[ " + tabControl1.TabPages[6].Text + " ]", true, false);
			logReport.WriteString();

			logReport.Param(label06_01.Text, comboBox06_01.Text);
			logReport.Param(label06_04.Text, comboBox06_02.Text);
			logReport.Param(label06_02.Text, textBox06_01.Text, label06_03.Text);
			logReport.Param(label06_05.Text, textBox06_02.Text, label06_06.Text);
			logReport.Param(label06_20.Text, comboBox06_04.Text);
			logReport.Param(label06_21.Text, comboBox06_05.Text);

			logReport.Param(label06_08.Text, textBox06_03.Text, label06_09.Text);
			logReport.Param(label06_10.Text, textBox06_04.Text, label06_11.Text);
			logReport.Param(label06_12.Text, textBox06_05.Text, label06_13.Text);
			logReport.Param(label06_14.Text, textBox06_06.Text, label06_15.Text);
			logReport.Param(label06_16.Text, textBox06_07.Text, label06_17.Text);
			logReport.Param(label06_18.Text, textBox06_08.Text, label06_19.Text);
			logReport.Param(label06_22.Text, textBox06_09.Text);
			if (radioButton06_03.Checked)
			{
				logReport.Param(radioButton06_03.Text, comboBox06_06.Text);
			}
			else
			{
				logReport.Param(radioButton06_04.Text, textBox06_15.Text);
			}
			logReport.Param(label06_24.Text, textBox06_10.Text, label06_25.Text);
			logReport.Param(label06_26.Text, textBox06_11.Text, label06_27.Text);
			logReport.Param(label06_28.Text, comboBox06_07.Text);
			logReport.Param(label06_29.Text, numericUpDown06_01.Text, label06_30.Text);
			//====================================================================================
			logReport.WriteString();
			logReport.WriteString();

			logReport.ExH2(tabControl1.TabPages[7].Text);
			logReport.HTMLString("[ " + tabControl1.TabPages[7].Text + " ]", true, false);
			logReport.WriteString();

			logReport.WriteString(groupBox07_01.Text);
			if (radioButton07_02.Checked)
				logReport.Param(radioButton07_02.Text, comboBox07_01.Text, label07_02.Text);

			logReport.Param(radioButton07_01.Text, numericUpDown07_01.Text, label07_01.Text);
			if ((CodeSegmentPath)comboBox07_07.SelectedItem == CodeSegmentPath.DF)
				logReport.Param(label07_03.Text, comboBox07_02.Text);

			logReport.WriteString(groupBox07_02.Text);
			if(radioButton07_04.Checked)
				logReport.Param(radioButton07_04.Text, comboBox07_03.Text, label07_05.Text);

			if (radioButton07_03.Checked)
				logReport.Param(label07_06.Text, textBox07_02.Text);

			logReport.Param(label07_07.Text, comboBox07_04.Text);
			if((eSensorType)comboBox07_04.SelectedIndex== eSensorType.DME_DME && checkBox07_01.Checked)
				logReport.WriteString(checkBox07_01.Text);

			logReport.Param(label07_08.Text, comboBox07_05.Text);
			logReport.Param(label07_09.Text, comboBox07_06.Text);
			logReport.Param(label07_10.Text, comboBox07_07.Text);
			logReport.Param(label07_31.Text, textBox07_10.Text, label07_31.Text);


			logReport.WriteString(groupBox07_03.Text);
			logReport.Param(label07_11.Text, textBox07_03.Text, label07_12.Text);
			logReport.Param(label07_13.Text, comboBox07_08.Text, label07_14.Text);  //textBox3777
			logReport.Param(label07_15.Text, numericUpDown07_02.Text, label07_16.Text);

			logReport.WriteString(label07_33.Text);
			logReport.Param(label07_17.Text, textBox07_04.Text, label07_18.Text);
			logReport.Param(label07_19.Text, textBox07_05.Text, label07_20.Text);
			logReport.Param(label07_21.Text, textBox07_06.Text, label07_22.Text);

			logReport.WriteString();
			logReport.Param(label07_29.Text, comboBox07_09.Text, label07_30.Text);

			//LegsInfoForm.InfoForm.InitInfoForm(0, 0, transitions.LegPoints);
			//logReport.WriteTab(LegsInfoForm.InfoForm.DataGrid, RepFileTitle);

			//====================================================================================
			logReport.WriteString();
			logReport.WriteString();

			logReport.ExH2(tabControl1.TabPages[8].Text);
			logReport.HTMLString("[ " + tabControl1.TabPages[8].Text + " ]", true, false);
			logReport.WriteString();

			logReport.WriteTab(dataGridView08_01, RepFileTitle);

			logReport.Param(label08_01.Text, textBox08_01.Text, label08_02.Text);
			logReport.WriteString(groupBox08_01.Text);
			logReport.Param(label08_03.Text, textBox08_02.Text, label08_04.Text);

			//====================================================================================
			logReport.CloseFile();
		}

		private void FillObstacleAssesment(ObstacleAssessmentArea pPrimProtectedArea, ObstacleContainer obstacles, double PDG, bool isPrima)
		{
			if (pPrimProtectedArea == null)
				throw new ArgumentNullException();

			if (obstacles.Parts == null)
				return;

			var uomDistVerTab = new UomDistanceVertical[] { UomDistanceVertical.M, UomDistanceVertical.FT };
			var uomDistVer = uomDistVerTab[GlobalVars.unitConverter.DistanceUnitIndex];

			var sortedObstacleList = obstacles.Parts.ToList().
				OrderByDescending(obs => obs.PDG).ToList();

			if (!GlobalVars.settings.AnnexObstalce)
			{
				var closeInObsCount = sortedObstacleList.Count(s => s.PDG >= PDG);
				sortedObstacleList = sortedObstacleList.Take(Math.Max(closeInObsCount, saveFeatureCount)).ToList();
			}

			foreach (var obstacle in sortedObstacleList)
			{
				if (obstacle.Prima != isPrima) return;

				Obstruction obs = new Obstruction
				{
					VerticalStructureObstruction = new FeatureRef(obstacles.Obstacles[obstacle.Owner].Identifier),
					SurfacePenetration = obstacle.PDG >= PDG,
						CloseIn = obstacle.PDG > GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value && obstacle.Ignored
				};

				var minimumAltitude = obstacle.Height + obstacle.MOC;

				var pDistanceVertical = new ValDistanceVertical
				{
					Uom = uomDistVer,
					Value = GlobalVars.unitConverter.HeightToDisplayUnits(minimumAltitude)
				};
				obs.MinimumAltitude = pDistanceVertical;

				//MOC
				var pDistance = new ValDistance
				{
					Uom = GlobalVars.unitConverter.DistanceUnitIndex == 0 ? UomDistance.M : UomDistance.FT,
					Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstacle.MOC)
				};

				obs.RequiredClearance = pDistance;
				pPrimProtectedArea.SignificantObstacle.Add(obs);
			}
		}

		private SegmentLeg IntermediateApproachLeg(InstrumentApproachProcedure procedure, AircraftCharacteristic isLimitedTo, ref TerminalSegmentPoint pEndPoint)
		{

			return null;
		}

		private SegmentLeg FinalApproachLeg(InstrumentApproachProcedure procedure, AircraftCharacteristic isLimitedTo, ref TerminalSegmentPoint pEndPoint)
		{
			SegmentLeg pSegmentLeg;
			ApproachLeg pApproachLeg;
			SegmentPoint pSegmentPoint;
			Feature pFixDesignatedPoint;
			SignificantPoint pFIXSignPt;
			PointReference pPointReference;

			ValSpeed pSpeed;
			ValDistance pDistance;
			ValDistanceSigned pDistanceSigned;
			ValDistanceVertical pDistanceVertical;

			//pApproachLeg = DBModule.pObjectDir.CreateFeature<IntermediateLeg>();
			pApproachLeg = DBModule.pObjectDir.CreateFeature<FinalLeg>();

			//pApproachLeg.Approach = pProcedure.GetFeatureRef()

			pApproachLeg.AircraftCategory.Add(isLimitedTo);
			pSegmentLeg = pApproachLeg;

			//	pSegmentLeg.AltitudeOverrideATC =
			//	pSegmentLeg.AltitudeOverrideReference =
			//	pSegmentLeg.Duration = ' ???????????????????????????????? pLegs(I).valDur
			//	pSegmentLeg.Note
			//	pSegmentLeg.ProcedureTurnRequired
			//	pSegmentLeg.ReqNavPerformance
			//	pSegmentLeg.SpeedInterpretation =

			pSegmentLeg.UpperLimitReference = CodeVerticalReference.MSL;
			pSegmentLeg.LowerLimitReference = CodeVerticalReference.MSL;

			//pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.EITHER;
			//SegmentLeg

			pSegmentLeg.UpperLimitReference = CodeVerticalReference.MSL;
			pSegmentLeg.LowerLimitReference = CodeVerticalReference.MSL;
			pSegmentLeg.LegPath = CodeTrajectory.STRAIGHT;
			pSegmentLeg.CourseType = CodeCourse.TRUE_TRACK;

			LegBase currLeg = _finalLeq;
			WayPoint StartFix = currLeg.StartFIX;
			WayPoint EndFix = currLeg.EndFIX;

			if (EndFix.IsDFTarget || ARANMath.SubtractAngles(EndFix.EntryDirection, EndFix.OutDirection) < ARANMath.DegToRad(5))
			{
				if (EndFix.TurnDirection == TurnDirection.CW)
					pSegmentLeg.TurnDirection = CodeDirectionTurn.RIGHT;
				else
					pSegmentLeg.TurnDirection = CodeDirectionTurn.LEFT;
			}

			pSegmentLeg.LegTypeARINC = CodeSegmentPath.TF;


			if (EndFix.FlyMode == eFlyMode.Atheight)
			{
				pSegmentLeg.EndConditionDesignator = CodeSegmentTermination.ALTITUDE;
				pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT;
			}
			else
				pSegmentLeg.EndConditionDesignator = CodeSegmentTermination.INTERCEPT;

			pSegmentLeg.ProcedureTurnRequired = false;
			// ====================================================================
			if (!EndFix.IsDFTarget)
			{
				pSegmentLeg.Course = ARANFunctions.DirToAzimuth(EndFix.PrjPt, EndFix.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				pSegmentLeg.CourseDirection = CodeDirectionReference.TO;
			}

			//========================================================================
			pSegmentLeg.AltitudeInterpretation = CodeAltitudeUse.BETWEEN;
			pDistanceVertical = new ValDistanceVertical();
			pDistanceVertical.Uom = mUomVDistance;
			pDistanceVertical.Value = GlobalVars.unitConverter.HeightToDisplayUnits(StartFix.ConstructAltitude, eRoundMode.SPECIAL_NEAREST);
			pSegmentLeg.UpperLimitAltitude = pDistanceVertical;

			// =================                                                  ==============
			pDistanceVertical = new ValDistanceVertical();
			pDistanceVertical.Uom = mUomVDistance;
			pDistanceVertical.Value = GlobalVars.unitConverter.HeightToDisplayUnits(EndFix.ConstructAltitude, eRoundMode.SPECIAL_NEAREST);
			pSegmentLeg.LowerLimitAltitude = pDistanceVertical;
			// ====================================================================

			pDistance = new ValDistance();
			pDistance.Uom = mUomHDistance;

			double trDist = ARANFunctions.ReturnDistanceInMeters(StartFix.PrjPt, EndFix.PrjPt);
			if (EndFix.IsDFTarget || StartFix.FlyMode == eFlyMode.Atheight)
			{
				double Re = EndFix.NomLineTurnRadius;
				double alpe = 0.5 * EndFix.TurnAngle;

				trDist = currLeg.Length + Re * alpe;
			}

			pDistance.Value = GlobalVars.unitConverter.DistanceToDisplayUnits(trDist, eRoundMode.NEAREST);
			pSegmentLeg.Length = pDistance;
			// ====================================================================
			pSegmentLeg.VerticalAngle = -ARANMath.RadToDeg(System.Math.Atan(prmFAFGradient));
			// ====================================================================
			pSpeed = new ValSpeed();
			pSpeed.Uom = mUomSpeed;
			pSpeed.Value = GlobalVars.unitConverter.SpeedToDisplayUnits(_FAF.IAS, eRoundMode.SPECIAL_NEAREST);
			pSegmentLeg.SpeedLimit = pSpeed;
			pSegmentLeg.SpeedReference = CodeSpeedReference.IAS;

			//=======================================================================================
			//pSegmentLeg.BankAngle.Value = fBankAngle;
			//=======================================================================================

			//Feature pFixDesignatedPoint;
			//SignificantPoint pFIXSignPt;
			//PointReference pPointReference;
			Surface pSurface;

			// Start Point ========================================================

			if (pEndPoint == null && StartFix.FlyMode != eFlyMode.Atheight)
			{
				TerminalSegmentPoint pStartPoint = new TerminalSegmentPoint();
				//if (StartFix.Role == eFIXRole.TP_)
				//	pStartPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.TP;
				//else if (StartFix.Role == eFIXRole.IAF_GT_56_ || StartFix.Role == eFIXRole.IAF_LE_56_ || StartFix.Role == eFIXRole.PBN_IAF)
				//	pStartPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.IAF;

				pStartPoint.Role = CodeProcedureFixRole.IF;
				pSegmentPoint = pStartPoint;

				pSegmentPoint.FlyOver = StartFix.FlyMode == eFlyMode.Flyover;

				pSegmentPoint.RadarGuidance = false;
				pSegmentPoint.ReportingATC = CodeATCReporting.NO_REPORT;
				pSegmentPoint.Waypoint = false;

				//== ==============================================================

				pFixDesignatedPoint = DBModule.CreateDesignatedPoint(StartFix);
				pFIXSignPt = new SignificantPoint();

				if (pFixDesignatedPoint is DesignatedPoint)
					pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef();
				else if (pFixDesignatedPoint is Navaid)
					pFIXSignPt.NavaidSystem = pFixDesignatedPoint.GetFeatureRef();
				else if (pFixDesignatedPoint is RunwayCentrelinePoint)
					pFIXSignPt.RunwayPoint = pFixDesignatedPoint.GetFeatureRef();

				if (Functions.PriorPostFixTolerance(StartFix.TolerArea, StartFix.PrjPt, StartFix.EntryDirection, out double PriorFixTolerance, out double PostFixTolerance))
				{
					pPointReference = new PointReference();

					pDistanceSigned = new ValDistanceSigned();
					pDistanceSigned.Uom = mUomHDistance;
					pDistanceSigned.Value = Math.Abs(GlobalVars.unitConverter.DistanceToDisplayUnits(PriorFixTolerance, eRoundMode.NEAREST));
					pPointReference.PriorFixTolerance = pDistanceSigned;

					pDistanceSigned = new Aran.Aim.DataTypes.ValDistanceSigned();
					pDistanceSigned.Uom = mUomHDistance;
					pDistanceSigned.Value = Math.Abs(GlobalVars.unitConverter.DistanceToDisplayUnits(PostFixTolerance, eRoundMode.NEAREST));
					pPointReference.PostFixTolerance = pDistanceSigned;

					pSurface = new Surface();
					MultiPolygon pl = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(StartFix.TolerArea);
					if (!pl.IsEmpty)
					{
						pSurface.Geo.Add(pl);
						pPointReference.FixToleranceArea = pSurface;
					}

					pStartPoint.FacilityMakeup.Add(pPointReference);
				}

				pSegmentPoint.PointChoice = pFIXSignPt;
				pSegmentLeg.StartPoint = pStartPoint;
			}
			else
				pSegmentLeg.StartPoint = pEndPoint;

			//  End Of Start Point ================================================
			//=====================================================================
			//  Start Of End Point ================================================
			pEndPoint = null;
			if (EndFix.FlyMode != eFlyMode.Atheight)
			{
				pEndPoint = new TerminalSegmentPoint();

				//if (EndFix.Role == eFIXRole.TP_)
				//	pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.TP;
				//else if (EndFix.Role == eFIXRole.IAF_GT_56_ || EndFix.Role == eFIXRole.IAF_LE_56_ || EndFix.Role == eFIXRole.PBN_IAF)
				//	pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.IAF;

				pEndPoint.Role = CodeProcedureFixRole.FAF;

				pSegmentPoint = pEndPoint;

				pSegmentPoint.FlyOver = EndFix.FlyMode == eFlyMode.Flyover;

				pSegmentPoint.RadarGuidance = false;
				pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT;
				pSegmentPoint.Waypoint = false;

				//============================================================

				pFixDesignatedPoint = DBModule.CreateDesignatedPoint(EndFix);//.PrjPt, EndFix.Name, EndFix.EntryDirection);
				pFIXSignPt = new SignificantPoint();

				if (pFixDesignatedPoint is DesignatedPoint)
					pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef();
				else if (pFixDesignatedPoint is Navaid)
					pFIXSignPt.NavaidSystem = pFixDesignatedPoint.GetFeatureRef();
				else if (pFixDesignatedPoint is RunwayCentrelinePoint)
					pFIXSignPt.RunwayPoint = pFixDesignatedPoint.GetFeatureRef();

				//=======================

				if (Functions.PriorPostFixTolerance(EndFix.TolerArea, EndFix.PrjPt, EndFix.EntryDirection, out double  PriorFixTolerance, out double PostFixTolerance))
				{
					pPointReference = new PointReference();

					pDistanceSigned = new ValDistanceSigned();
					pDistanceSigned.Uom = mUomHDistance;
					pDistanceSigned.Value = Math.Abs(GlobalVars.unitConverter.DistanceToDisplayUnits(PriorFixTolerance, eRoundMode.NEAREST));
					pPointReference.PriorFixTolerance = pDistanceSigned;

					pDistanceSigned = new Aran.Aim.DataTypes.ValDistanceSigned();
					pDistanceSigned.Uom = mUomHDistance;
					pDistanceSigned.Value = Math.Abs(GlobalVars.unitConverter.DistanceToDisplayUnits(PostFixTolerance, eRoundMode.NEAREST));
					pPointReference.PostFixTolerance = pDistanceSigned;

					pSurface = new Surface();
					MultiPolygon pl = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(EndFix.TolerArea);
					if (!pl.IsEmpty)
					{
						pSurface.Geo.Add(pl);
						pPointReference.FixToleranceArea = pSurface;
					}

					pEndPoint.FacilityMakeup.Add(pPointReference);
				}

				pSegmentPoint.PointChoice = pFIXSignPt;
				pSegmentLeg.EndPoint = pEndPoint;
			}

			// End of EndPoint ========================

			// Trajectory =====================================================

			Curve pCurve = new Curve();

			LineString ls = GlobalVars.pspatialReferenceOperation.ToGeo<LineString>(currLeg.NominalTrack);
			pCurve.Geo.Add(ls);
			pSegmentLeg.Trajectory = pCurve;

			// I protected Area =======================================================
			pSurface = new Surface();
			for (int i = 0; i < currLeg.PrimaryAssesmentArea.Count; i++)
			{
				Polygon pl = GlobalVars.pspatialReferenceOperation.ToGeo<Polygon>(currLeg.PrimaryAssesmentArea[i]);
				if (!pl.IsEmpty)
					pSurface.Geo.Add(pl);
			}

			ObstacleAssessmentArea pPrimProtectedArea = new ObstacleAssessmentArea();

			if (!pSurface.Geo.IsEmpty)
				pPrimProtectedArea.Surface = pSurface;

			pPrimProtectedArea.SectionNumber = 0;
			pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY;
			//FillObstacleAssesment(pPrimProtectedArea, currLeg.Obstacles, prmFAFGradient, true);
			pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);

			// II protected Area =======================================================
			GeometryOperators pTopo = new GeometryOperators();
			MultiPolygon pPolygon = (MultiPolygon)pTopo.Difference(currLeg.FullAssesmentArea, currLeg.PrimaryAssesmentArea);

			ObstacleAssessmentArea pSecProtectedArea = null;

			if (pPolygon != null)
			{
				pSurface = new Surface();

				for (int i = 0; i < pPolygon.Count; i++)
				{
					Polygon pl = GlobalVars.pspatialReferenceOperation.ToGeo<Polygon>(pPolygon[i]);
					if (!pl.IsEmpty)
						pSurface.Geo.Add(pl);
				}

				pSecProtectedArea = new ObstacleAssessmentArea();

				if (!pSurface.Geo.IsEmpty)
					pSecProtectedArea.Surface = pSurface;

				pSecProtectedArea.SectionNumber = 1;
				pSecProtectedArea.Type = CodeObstacleAssessmentSurface.SECONDARY;
				//FillObstacleAssesment(pPrimProtectedArea, currLeg.Obstacles, prmFAFGradient, false);

				pSegmentLeg.DesignSurface.Add(pSecProtectedArea);
			}

			// Protection Area Obstructions list ==================================================
			ObstacleContainer ostacles = currLeg.Obstacles;

			GeometryOperators pFullReleational = new GeometryOperators();
			pFullReleational.CurrentGeometry = currLeg.FullAssesmentArea;

			GeometryOperators pPrimReleational = new GeometryOperators();
			pPrimReleational.CurrentGeometry = currLeg.PrimaryAssesmentArea;

			//sort by Req.H
			//Added by Agshin
			Functions.Sort(ostacles, 2);
			int saveCount = Math.Min(ostacles.Obstacles.Length, saveFeatureCount);

			for (int l = 0; l < ostacles.Obstacles.Length; l++)
				ostacles.Obstacles[l].NIx = -1;

			for (int i = 0; i < ostacles.Parts.Length; i++)
			{
				if (saveCount == 0)
					break;
				double requiredClearance = 0.0;
				int isPrimary = 0;

				Obstacle tmpObstacle = ostacles.Obstacles[ostacles.Parts[i].Owner];

				if (tmpObstacle.NIx > -1)
					continue;

				ostacles.Obstacles[ostacles.Parts[i].Owner].NIx = 0;

				//MinimumAltitude = Math.Max(MinimumAltitude, ostacles.Parts[ostacles.Obstacles[i].Parts[j]].ReqH)

				requiredClearance = ostacles.Parts[i].MOC;

				if (!pPrimReleational.Disjoint(ostacles.Parts[i].pPtPrj))
					isPrimary |= 1;
				else
					isPrimary |= 2;

				Obstruction obs = new Obstruction();

				obs.VerticalStructureObstruction = new FeatureRef(tmpObstacle.Identifier);

				//ReqH
				double minimumAltitude = tmpObstacle.Height + requiredClearance + _refElevation;

				pDistanceVertical = new ValDistanceVertical();
				pDistanceVertical.Uom = mUomVDistance;
				pDistanceVertical.Value = GlobalVars.unitConverter.HeightToDisplayUnits(minimumAltitude);

				obs.MinimumAltitude = pDistanceVertical;

				//MOC

				pDistance = new ValDistance();
				pDistance.Uom = UomDistance.M;
				pDistance.Value = requiredClearance;

				obs.RequiredClearance = pDistance;


				if ((isPrimary & 1) != 0)
					pPrimProtectedArea.SignificantObstacle.Add(obs);

				if (pSecProtectedArea != null && ((isPrimary & 2) != 0))
					pSecProtectedArea.SignificantObstacle.Add(obs);

				saveCount--;
			}

			//pSegmentLeg.DesignSurface.Add(pPrimProtectedArea)
			//pSegmentLeg.DesignSurface.Add(pSecProtectedArea)
			//  END ======================================================

			return pApproachLeg;
		}

		bool SaveProcedure()
		{
			ProcedureTransition pTransition;
			AircraftCharacteristic IsLimitedTo;
			LandingTakeoffAreaCollection pLandingTakeoffAreaCollection;
			TerminalSegmentPoint pEndPoint = null;
			SegmentLeg pSegmentLeg;
			ProcedureTransitionLeg ptl;

			FeatureRef featureRef;
			FeatureRefObject featureRefObject;
			DBModule.pObjectDir.ClearAllFeatures();

			//  Procedure =================================================================================================

			Procedure = DBModule.pObjectDir.CreateFeature<InstrumentApproachProcedure>();

			pLandingTakeoffAreaCollection = new LandingTakeoffAreaCollection();
			string sProcName;

			if (tabControl01_01.SelectedIndex == 0)
			{
				sProcName = "RNAV" + "_RWY" + comboBox00_02.Text + "_CAT_" + comboBox00_03.Text;
				pLandingTakeoffAreaCollection.Runway.Add(_selectedRWY.GetFeatureRefObject());
			}
			else
			{
				sProcName = "RNAV" + "_CAT_" + comboBox00_03.Text;

				foreach (RWYType rwy in GlobalVars.RWYList)
					if (rwy.Selected)
						pLandingTakeoffAreaCollection.Runway.Add(rwy.GetFeatureRefObject());
			}

			Procedure.Landing = pLandingTakeoffAreaCollection;
			Procedure.Name = sProcName;
			Procedure.CodingStandard = CodeProcedureCodingStandard.PANS_OPS;
			Procedure.DesignCriteria = CodeDesignStandard.PANS_OPS;
			Procedure.RNAV = true;
			Procedure.FlightChecked = false;

			IsLimitedTo = new AircraftCharacteristic();

			switch (comboBox00_03.SelectedIndex)
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

			Procedure.AircraftCharacteristic.Add(IsLimitedTo);

			featureRefObject = new FeatureRefObject();
			featureRef = new FeatureRef();
			featureRef.Identifier = GlobalVars.CurrADHP.pAirportHeliport.Identifier;
			featureRefObject.Feature = featureRef;
			Procedure.AirportHeliport.Add(featureRefObject);

			// Transition ==========================================================================
			pTransition = new ProcedureTransition();
			pTransition.Type = CodeProcedurePhase.FINAL;

			//pTransition.DepartureRunwayTransition = pLandingTakeoffAreaCollection;

			// Legs ======================================================================================================

			//Leg 1 Intermediate Approach ========================================================================

			uint NO_SEQ = 1;

			pSegmentLeg = IntermediateApproachLeg(Procedure, IsLimitedTo, ref pEndPoint);

			ptl = new ProcedureTransitionLeg();
			ptl.SeqNumberARINC = NO_SEQ;
			ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef<AbstractSegmentLegRef>();

			pTransition.TransitionLeg.Add(ptl);

			//Leg 2 Final Approach ===============================================================================

			NO_SEQ++;

			pSegmentLeg = FinalApproachLeg(Procedure, IsLimitedTo, ref pEndPoint);

			ptl = new ProcedureTransitionLeg();
			ptl.SeqNumberARINC = NO_SEQ;
			ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef<AbstractSegmentLegRef>();

			pTransition.TransitionLeg.Add(ptl);

			//=============================================================================
			Procedure.FlightTransition.Add(pTransition);

			//===============================================================
			return false;
		}

		#endregion
	}
}
