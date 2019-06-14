using System;
using System.Collections.Generic;

using Aran.Geometries;
using Aran.Geometries.Operators;

using Aran.AranEnvironment;
using Aran.AranEnvironment.Symbols;

using Aran.PANDA.Constants;

namespace Aran.PANDA.Common
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eTurnAt { MApt = 0, Altitude = 1, TP = 2 };

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct TableCell
	{
		public double ATT, FTT, XTT, SemiWidth;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eAIXMType
	{
		Null,				//0
		DME,				//1
		VOR,				//2
		Obstacle,			//3
		AHP,				//4
		RWY,				//5
		RwyDirection,		//6
		NDB,				//7
		DesignatedPoint,	//8
		TACAN				//9
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public class WayPoint : AranObject, IAranCloneable
	{
		protected static Aran.PANDA.Constants.Constants _constants = null;
		protected static SpatialReferenceOperation _pspatialReferenceOperation = null;

		protected IAranGraphics _UI;
		protected IAranEnvironment _aranEnv;

		public readonly static double[] WPTBuffers = //new double[]
		{
			926,     926,    926,    1852,    1852,  1852, 1852,   3704,     3704,      3704
		};
		//  SID, MApLT28, FAFApch, SIDGE28, MApGE28, STAR, IIAP, SIDGE56, STARGE56, Enroute


		//	3704,	3704,	1852,	926,	3704,	1852, 1852,   926,     926,   1852
		//Enroute, SIDGE56, SIDGE28, SID, STARGE56, STAR, IIAP, FAFApch, MApLT28, MApGE28

		/*RNP_APCH, RNAV1, RNAV2, BASIC_RNP1, RNP4, RNAV5, GNSS, RNP_APCH_Final*/

		public readonly static TableCell[,] WPTParams = //new TableCell[,]
		{
			// RNP APCH
			{
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},						// SID			-
				new TableCell {ATT = 1481.6, FTT = 926, XTT = 1852, SemiWidth = 3704},			// MApLT28		*
				new TableCell {ATT = 444.48, FTT = 463, XTT = 555.6, SemiWidth = 1759.4},		// FAFApch		*
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},						// SIDGE28		-
				new TableCell {ATT = 1481.6, FTT = 926, XTT = 1852, SemiWidth = 4630},			// MApGE28		*
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},						// STAR			-
				new TableCell {ATT = 1481.6, FTT = 926, XTT = 1852, SemiWidth = 4630},			// IIAP			*
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},						// SIDGE56		-
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},						// STARGE56		-
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0}},						// Enroute		-

			// RNAV1
			{
				new TableCell {ATT = 1481.6, FTT = 926, XTT = 1852, SemiWidth = 3704},			// SID			*
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},						// MApLT28		-
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},						// FAFApch		-
				new TableCell {ATT = 1481.6, FTT = 926, XTT = 1852, SemiWidth = 4630},			// SIDGE28		*
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},						// MApGE28		-
				new TableCell {ATT = 1481.6, FTT = 926, XTT = 1852, SemiWidth = 4630},			// STAR			*
				new TableCell {ATT = 1481.6, FTT = 926, XTT = 1852, SemiWidth = 4630},			// IIAP			*
				new TableCell {ATT = 2963.2, FTT = 926, XTT = 3704, SemiWidth = 9260},			// SIDGE56		*
				new TableCell {ATT = 2963.2, FTT = 926, XTT = 3704, SemiWidth = 9260},			// STARGE56		*
				new TableCell {ATT = 2963.2, FTT = 926, XTT = 3704, SemiWidth = 9260}},			// Enroute		*

			// RNAV2
			{
				new TableCell {ATT = 1481.6, FTT = 1852, XTT = 1852, SemiWidth = 3704},			// SID			*
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},						// MApLT28		-
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},						// FAFApch		-
				new TableCell {ATT = 1481.6, FTT = 1852, XTT = 1852, SemiWidth = 4630},			// SIDGE28		*
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},						// MApGE28		-
				new TableCell {ATT = 1481.6, FTT = 1852, XTT = 1852, SemiWidth = 4630},			// STAR			*
				new TableCell {ATT = 1481.6, FTT = 1852, XTT = 1852, SemiWidth = 4630},			// IIAP			*
				new TableCell {ATT = 2963.2, FTT = 1852, XTT = 3704, SemiWidth = 9260},			// SIDGE56		*
				new TableCell {ATT = 2963.2, FTT = 1852, XTT = 3704, SemiWidth = 9260},			// STARGE56		*
				new TableCell {ATT = 2963.2, FTT = 1852, XTT = 3704, SemiWidth = 9260}},		// Enroute		*

			// RNP 0.3
			{
				new TableCell {ATT = 0.0, FTT = 0.0, XTT = 0.0, SemiWidth = 0.0},				// SID			-
				new TableCell {ATT = 0.0, FTT = 0.0, XTT = 0.0, SemiWidth = 0.0},				// MApLT28		-
				new TableCell {ATT = 0.0, FTT = 0.0, XTT = 0.0, SemiWidth = 0.0},				// FAFApch		-
				new TableCell {ATT = 0.0, FTT = 0.0, XTT = 0.0, SemiWidth = 0.0},				// SIDGE28		-
				new TableCell {ATT = 0.0, FTT = 0.0, XTT = 0.0, SemiWidth = 0.0},				// MApGE28		-
				new TableCell {ATT = 0.0, FTT = 0.0, XTT = 0.0, SemiWidth = 0.0},				// STAR			-
				new TableCell {ATT = 0.0, FTT = 0.0, XTT = 0.0, SemiWidth = 0.0},				// IIAP			-
				new TableCell {ATT = 0.0, FTT = 0.0, XTT = 0.0, SemiWidth = 0.0},				// SIDGE56		-
				new TableCell {ATT = 0.0, FTT = 0.0, XTT = 0.0, SemiWidth = 0.0},				// STARGE56		-
				new TableCell {ATT = 444.48, FTT = 277.8, XTT = 555.6, SemiWidth = 4537.4}},	// Enroute		*

			// RNP1
			{
				new TableCell {ATT = 1481.6, FTT = 926, XTT = 1852, SemiWidth = 3704},			// SID			*
				new TableCell {ATT = 0,	FTT = 0, XTT = 0, SemiWidth = 0},						// MApLT28		-
				new TableCell {ATT = 0,	FTT = 0, XTT = 0, SemiWidth = 0},						// FAFApch		-
				new TableCell {ATT = 1481.6, FTT = 926, XTT = 1852, SemiWidth = 4630},			// SIDGE28		*
				new TableCell {ATT = 0,	FTT = 0, XTT = 0, SemiWidth = 0},						// MApGE28		-
				new TableCell {ATT = 1481.6, FTT = 926, XTT = 1852, SemiWidth = 4630},			// STAR			*
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},						// IIAP			-
				new TableCell {ATT = 1481.6, FTT = 926, XTT = 1852, SemiWidth = 6482},			// SIDGE56		*
				new TableCell {ATT = 1481.6, FTT = 926, XTT = 1852, SemiWidth = 6482},			// STARGE56		*
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0}},						// Enroute		-

			// RNP4
			{
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},						// SID			-
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},						// MApLT28		-
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},						// FAFApch		-
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},				        // SIDGE28		-
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},						// MApGE28		-
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},						// STAR			-
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},						// IIAP			-
				new TableCell {ATT = 5926.4, FTT = 3704, XTT = 7408, SemiWidth = 14816},		// SIDGE56		*
				new TableCell {ATT = 5926.4, FTT = 3704, XTT = 7408, SemiWidth = 14816},		// STARGE56		*
				new TableCell {ATT = 5926.4, FTT = 3704, XTT = 7408, SemiWidth = 14816}},		// Enroute		*

			// RNAV5
			{
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},						// SID			-
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},						// MApLT28		-
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},						// FAFApch		-
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},					    // SIDGE28		-
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0 },						// MApGE28		-
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},						// STAR			-
				new TableCell {ATT = 0, FTT = 0, XTT = 0, SemiWidth = 0},						// IIAP			-
				new TableCell {ATT = 3722.52, FTT = 4630, XTT = 4648.52, SemiWidth = 10686.04},	// SIDGE56		*
				new TableCell {ATT = 3722.52, FTT = 4630, XTT = 4648.52, SemiWidth = 10686.04},	// STARGE56		*
				new TableCell {ATT = 3722.52, FTT = 4630, XTT = 4648.52, SemiWidth = 10686.04}	// Enroute		*
			} };

		protected eAIXMType _AIXMType;

		protected Point _PrjPt;
		protected Point _GeoPt;

		protected Point _PrjPtH;
		protected Point _GeoPtH;

		protected MultiPolygon _TolerArea;

		protected FillSymbol _ToleranceSymbol;
		protected PointSymbol _PointSymbol;

		protected eFlightPhase _FlightPhase;
		protected eFIXRole _FIXRole;								//_ConstMode, 
		protected eSensorType _SensorType;
		protected eFlyMode _FlyMode;
		protected ePBNClass _PBNType;
		//
		protected TurnDirection _Propagated, _TurnDir,
								_TurnDir_L, _TurnDir_R;

		protected string _Name;
		protected Guid  _Id;

		protected double _BankAngle, _PilotTime, _BankTime, _OCH;
		protected double _FTE, _XTT, _ATT;

		protected double _MagVar;
		protected double _TurnAngle, _TurnAngle_L, _TurnAngle_R;
		protected double _EntryDir, _EntryDir_L, _EntryDir_R;		//, _EntryDir_N
		protected double _outDir, _OutDir_L, _OutDir_R;				//, _OutDir_N

		protected double _EPT, _EPT_L, _EPT_R;
		protected double _LPT, _LPT_L, _LPT_R;

		protected double _ASW_L, _ASW_2_L, _ASW_R, _ASW_2_R, _SemiWidth;

		protected double _constructAltitude, 							//Protection area construction Altitude
						_nomLineAltitude;								//Nominal line construction Altitude
		protected double _constructionGradient,
						_nomLineGradient, _appliedGradient;							//, _Bank;
		protected double _ImMaxIntercept, _FlyOInterBank, _FlyByTechTol,
			_FlyOTechTol, _ISAtC, _IAS, _ConstructTAS, _NomLineTAS;

		protected bool _MultiCoverage, _DrawingEnabled;
		protected int _ToleranceElement, _PointElement, _ptHtElem;

		protected NavaidType _VOR_DME;

		virtual protected void CreateTolerArea() { }

		protected virtual void CalcWPTParams()
		{
			if ((int)_PBNType < 0 || (int)_FlightPhase < 0)
				return;

			const double SigmaSis = 92.6;
			const double ST = 463.0;
			const double SigmaMinAirRNAV = 157.42;
			const double SigmaMinAirRNAV5 = 692.955;
			const double DoplerRange = 75.0 * 1852.0;

			double dTT, SigmaAir, SigmaMinAir;

			_FTE = WPTParams[(int)_PBNType, (int)_FlightPhase].FTT;

			switch (_SensorType)
			{
				case eSensorType.GNSS:
					_XTT = WPTParams[(int)_PBNType, (int)_FlightPhase].XTT;
					_ATT = WPTParams[(int)_PBNType, (int)_FlightPhase].ATT;
					_SemiWidth = WPTParams[(int)_PBNType, (int)_FlightPhase].SemiWidth;
					break;
				case eSensorType.DME_DME:
					SigmaMinAir = _PBNType == ePBNClass.RNAV5 ? SigmaMinAirRNAV5 : SigmaMinAirRNAV;
					SigmaAir = Math.Max(4110 * 0.125 * 0.01 * Math.Sqrt(_constructAltitude), SigmaMinAir);

					dTT = 2 * ARANMath.C_SQRT2 * Math.Sqrt(SigmaSis * SigmaSis + SigmaAir * SigmaAir);

					if (!_MultiCoverage)
						dTT = 2 * dTT;

					_XTT = Math.Sqrt(dTT * dTT + _FTE * _FTE + ST * ST);
					_ATT = Math.Sqrt(dTT * dTT + ST * ST);
					_SemiWidth = 1.5 * _XTT + WPTBuffers[(int)_FlightPhase];
					break;
				case eSensorType.VOR_DME:
					double D = DoplerRange;
					if (_VOR_DME.pPtPrj != null)
						D = ARANFunctions.ReturnDistanceInMeters(_VOR_DME.pPtPrj, _PrjPt);
					SigmaAir = Math.Max(0.085 * 1852.0, 0.00125 * D);

					//=========================================================================
					//double dh = _constructAltitude - _VOR_DME.pPtPrj.Z;
					//double slantDist = Math.Sqrt(dh * dh + D * D);
					//SigmaAir = Math.Max(0.085 * 1852.0, 0.00125 * slantDist);
					//=========================================================================

					dTT = 2.0 * Math.Sqrt(SigmaSis * SigmaSis + SigmaAir * SigmaAir);

					double D1 = ARANMath.C_SQRT_2 * DoplerRange, D2 = ARANMath.C_SQRT_2 * DoplerRange, teta;
					if (_VOR_DME.pPtPrj != null)
						ARANFunctions.PrjToLocal(_PrjPt, _outDir, _VOR_DME.pPtPrj, out D2, out D1);

					D2 = Math.Abs(D2);
					D1 = Math.Abs(D1);

					if (D1 < ARANMath.EpsilonDistance)
						teta = 0.5 * Math.PI;
					else
						teta = Math.Atan(D2 / D1);

					double VT = D1 - D * Math.Cos(teta + _constants.NavaidConstants.VOR.IntersectingTolerance);
					double DT = dTT * Math.Cos(teta);
					double AVT = D2 - D * Math.Sin(teta - _constants.NavaidConstants.VOR.IntersectingTolerance);
					double ADT = dTT * Math.Sin(teta);

					_XTT = Math.Sqrt(VT * VT + DT * DT + ST * ST + _FTE * _FTE);
					_ATT = Math.Sqrt(AVT * AVT + ADT * ADT + ST * ST);

					_SemiWidth = 1.5 * _XTT + WPTBuffers[(int)_FlightPhase];
					break;
			}

			_ASW_R = _SemiWidth;
			_ASW_L = _SemiWidth;

			_ASW_2_R = 0.5 * _SemiWidth;
			_ASW_2_L = 0.5 * _SemiWidth;
			_BasePoints.Clear();
		}

		public void CalcExtraTurnRangePoints()
		{
			double R1Turn, L1, L1min;

			_TurnDir_L = ARANMath.SideFrom2Angle(_OutDir_L, _EntryDir_L);
			_TurnAngle_L = ARANMath.Modulus((_OutDir_L - _EntryDir_L) * ((int)_TurnDir_L), 2 * Math.PI);

			if (_TurnAngle_L > ARANMath.DegToRad(240))
				_TurnAngle_L = ARANMath.DegToRad(240);

			_TurnDir_R = ARANMath.SideFrom2Angle(_OutDir_R, _EntryDir_R);
			_TurnAngle_R = ARANMath.Modulus((_OutDir_R - _EntryDir_R) * ((int)_TurnDir_R), 2 * Math.PI);
			if (_TurnAngle_R > ARANMath.DegToRad(240))
				_TurnAngle_R = ARANMath.DegToRad(240);

			if (_FlyMode == eFlyMode.Atheight)
			{
				_EPT_L = _EPT_R = 0.0;
				_LPT_L = _LPT_R = _ConstructTAS * (_PilotTime + _BankTime);

				//if (Math.Abs(_TurnAngle_L) < ARANMath.EpsilonRadian)
				//    _LPT_L = 0.0;

				//if (Math.Abs(_TurnAngle_R) < ARANMath.EpsilonRadian)
				//    _LPT_R = 0.0;
			}
			else
			{
				if (_FlyMode == eFlyMode.Flyby)
				{
					R1Turn = ARANMath.BankToRadius(_BankAngle, _ConstructTAS);	//ConstructTurnRadius;			//
					//R1Turn = ARANMath.BankToRadiusForRnav(_BankAngle, _IAS, _TurnAltitude, _constants.Pansops[ePANSOPSData.ISA].Value);

					//if (Math.Abs(_TurnAngle_L) < ARANMath.EpsilonRadian)
					//{
					//    _EPT_L = _ATT;
					//    _LPT_L = 0;					///-_ATT
					//}
					//else
					{
						L1 = R1Turn * Math.Tan(0.5 * _TurnAngle_L);
						L1min = Math.Min(L1, R1Turn);

						_EPT_L = L1 + _ATT;
						_LPT_L = L1min - _ATT - _ConstructTAS * _PilotTime;
					}

					//if (Math.Abs(_TurnAngle_R) < ARANMath.EpsilonRadian)
					//{
					//    _EPT_R = _ATT;
					//    _LPT_R = 0;					///-_ATT
					//}
					//else
					{
						L1 = R1Turn * Math.Tan(0.5 * _TurnAngle_R);
						L1min = Math.Min(L1, R1Turn);

						_EPT_R = L1 + _ATT;
						_LPT_R = L1min - _ATT - _ConstructTAS * _PilotTime;
					}
				}
				else //if (_FlyMode == eFlyMode.FlyOver)
				{
					_EPT_L = _EPT_R = _ATT;
					_LPT_L = _LPT_R = _ATT + _ConstructTAS * (_PilotTime + _BankTime);

					//if (Math.Abs(_TurnAngle_L) < ARANMath.EpsilonRadian)
					//    _LPT_L = _ATT;

					//if (Math.Abs(_TurnAngle_R) < ARANMath.EpsilonRadian)
					//    _LPT_R = _ATT;
				}
			}
		}

		public void CalcTurnRangePoints(bool ToDf = false)
		{
			double R1Turn, L1, L1min;

			//double fAngle = ARANMath.Modulus(_EntryDir-_OutDir, ARANMath.C_2xPI);
			//if (Math.Abs(fAngle) < ARANMath.EpsilonRadian)
			//if (Math.Abs(_outDir - _EntryDir) < ARANMath.EpsilonRadian)


			if (ARANMath.SubtractAngles(_outDir, _EntryDir) < ARANMath.EpsilonRadian)
			{
				_TurnAngle = 0;
				TurnDirection = TurnDirection.NONE;
			}
			else
			{
				//_TurnAngle = (_OutDir - _EntryDir) * ((int)EffectiveTurnDirection);

				if (ToDf)
					_TurnDir = EffectiveTurnDirection;
				else
					TurnDirection = ARANMath.SideFrom2Angle(_outDir, _EntryDir);
				double fturn = (int)_TurnDir;

				_TurnAngle = ARANMath.Modulus((_outDir - _EntryDir) * fturn, 2 * Math.PI);
			}

			if (_TurnAngle > ARANMath.DegToRad(240))
				_TurnAngle = ARANMath.DegToRad(240);

			if (_FlyMode == eFlyMode.Atheight)
			{
				_EPT = 0.0;
				_LPT = _ConstructTAS * (_PilotTime + _BankTime);

				//if (Math.Abs(_TurnAngle) < ARANMath.EpsilonRadian)
				//    _LPT = 0.0;				// _TAS * (_PilotTime + _BankTime);
			}
			//else if (Math.Abs(_TurnAngle) < ARANMath.EpsilonRadian)
			//{
			//    _EPT = _ATT;

			//    if (_FlyMode == eFlyMode.FlyOver)
			//        _LPT = _ATT;
			//    else //if (_FlyMode == eFlyMode.FlyBy)
			//        _LPT = 0;					///-_ATT
			//}
			else if (_FlyMode == eFlyMode.Flyby)
			{
				R1Turn = ARANMath.BankToRadius(_BankAngle, _ConstructTAS);
				//R1Turn = ARANMath.BankToRadiusForRnav(_BankAngle, _IAS, _TurnAltitude, _constants.Pansops[ePANSOPSData.ISA].Value);
				L1 = R1Turn * Math.Tan(0.5 * _TurnAngle);

				L1min = Math.Min(L1, R1Turn);
				_EPT = L1 + _ATT;
				_LPT = L1min - _ATT - _ConstructTAS * _PilotTime;
			}
			else //if (_FlyMode == eFlyMode.FlyOver)
			{
				_EPT = _ATT;
				_LPT = _ATT + _ConstructTAS * (_PilotTime + _BankTime);
			}

			CalcExtraTurnRangePoints();
			_BasePoints.Clear();
		}

		//public WayPoint()
		//{
		//    FUI = null;
		//    FName = "";
		//    ID = "0";

		//    FPrjPt = new Point();
		//    FGeoPt = new Point();

		//    FTolerArea = new Polygon();

		//    FFlyByTechTol = GlobalVars.constants.Pansops[ePANSOPSData.rnvFlyByTechTol].Value;
		//    FFlyOTechTol = GlobalVars.constants.Pansops[ePANSOPSData.rnvFlyOTechTol].Value;

		//    FImMaxIntercept = GlobalVars.constants.Pansops[ePANSOPSData.arImMaxIntercept].Value;
		//    FFlyOInterBank = GlobalVars.constants.Pansops[ePANSOPSData.rnvFlyOInterBank].Value;

		//    FEntryDir = 0.0;
		//    FOutDir = 0.0;
		//    FTurnDir = SideDirection.sideOn;
		//    FPropagated = SideDirection.sideOn;

		//    FToleranceSymbol = new FillSymbol();

		//    //	FToleranceSymbol.Color := RGB(Random(256), Random(256), Random(256));
		//    FToleranceSymbol.Color = RGB(0, 0, 255);

		//    //	FToleranceSymbol.Style := TFillStyle(Random(8));
		//    FToleranceSymbol.Style = eFillStyle.sfsCross;

		//    //	FPointSymbol := TPointSymbol.Create(TPointStyle(Random(5)), RGB(Random(256), Random(256), Random(256)), 6);
		//    FPointSymbol = new PointSymbol(ePointStyle.smsCircle, 255, 6);

		//    FToleranceElement = -1;
		//    FPointElement = -1;

		//    FSensorType = (eSensorType)(-1);
		//    FPBNType = (ePBNType)(-1);

		//    //	FSensorType := stGNSS;
		//    //	FPBNType := ptRNPAPCH;
		//    FMultiCoverage = false;

		//    FFIXRole = (eFIXRole)(-1);
		//    FConstMode = FFIXRole;

		//    FPilotTime = 0.0;
		//    FBankTime = 0.0;

		//    FXXT = 0.0;
		//    FATT = 0.0;
		//    FSemiWidth = 0.0;
		//    FDrawingEnabled = true;
		//}

		public WayPoint(IAranEnvironment aranEnv)
		{
			if (_constants == null)
				_constants = new Aran.PANDA.Constants.Constants();

			if (_pspatialReferenceOperation == null)
				_pspatialReferenceOperation = new SpatialReferenceOperation(aranEnv);

			_aranEnv = aranEnv;
			_UI = aranEnv.Graphics;

			_Name = "";
			_Id = Guid.Empty;
			CallSign = "";

			_BasePoints = new List<Point>();
			_PrjPt = new Point();
			_GeoPt = new Point();

			_PrjPtH = new Point();
			_GeoPtH = new Point();

			_TolerArea = new MultiPolygon();

			_FlyByTechTol = _constants.Pansops[ePANSOPSData.rnvFlyByTechTol].Value;
			_FlyOTechTol = _constants.Pansops[ePANSOPSData.rnvFlyOTechTol].Value;

			_ImMaxIntercept = _constants.Pansops[ePANSOPSData.arImMaxIntercept].Value;
			_FlyOInterBank = _constants.Pansops[ePANSOPSData.rnvFlyOInterBank].Value;

			_MagVar = 0.0;
			//_EntryDir_N = 
				_EntryDir = 0.0;
			//_OutDir_N = 
				_outDir = 0.0;

			HorAccuracy =
			_constructAltitude = 0.0;
			_nomLineAltitude = 0.0;

			_TurnDir = TurnDirection.NONE;
			_Propagated = TurnDirection.NONE;

			_ToleranceSymbol = new FillSymbol();

			_ToleranceSymbol.Color = ARANFunctions.RGB(0, 0, 255);
			_ToleranceSymbol.Style = eFillStyle.sfsCross;
			_PointSymbol = new PointSymbol(ePointStyle.smsCircle, 255, 6);

			_ToleranceElement = -1;
			_PointElement = -1;
			_ptHtElem = -1;

			_SensorType = (eSensorType)(-1);
			_PBNType = (ePBNClass)(-1);

			//	FSensorType = stGNSS;
			//	FPBNType = ptRNPAPCH;

			_MultiCoverage = false;
			_FIXRole = (eFIXRole)(-1);
			_FlightPhase = (eFlightPhase)(-1);
			//_ConstMode = _FIXRole;

			_PilotTime = 0.0;
			_BankTime = 0.0;

			_XTT = 0.0;
			_ATT = 0.0;
			_SemiWidth = 0.0;
			_DrawingEnabled = true;
		}

		public WayPoint(eFIXRole InitialRole, IAranEnvironment aranEnv)
		{
			if (_constants == null)
				_constants = new Aran.PANDA.Constants.Constants();

			if (_pspatialReferenceOperation == null)
				_pspatialReferenceOperation = new SpatialReferenceOperation(aranEnv);

			_aranEnv = aranEnv;
			_UI = aranEnv.Graphics;

			_Name = "";
			_Id = Guid.Empty;
			CallSign = "";

			_BasePoints = new List<Point>();
			_PrjPt = new Point();
			_GeoPt = new Point();

			_PrjPtH = new Point();
			_GeoPtH = new Point();

			_TolerArea = new MultiPolygon();

			_FlyByTechTol = _constants.Pansops[ePANSOPSData.rnvFlyByTechTol].Value;
			_FlyOTechTol = _constants.Pansops[ePANSOPSData.rnvFlyOTechTol].Value;

			_ImMaxIntercept = _constants.Pansops[ePANSOPSData.arImMaxIntercept].Value;
			_FlyOInterBank = _constants.Pansops[ePANSOPSData.rnvFlyOInterBank].Value;

			_MagVar = 0.0;

			//_EntryDir_N = 
				_EntryDir = 0.0;
			//_OutDir_N = 
				_outDir = 0.0;

			HorAccuracy =
			_constructAltitude = 0.0;
			_nomLineAltitude = 0.0;

			_TurnDir = TurnDirection.NONE;
			_Propagated = TurnDirection.NONE;

			_ToleranceSymbol = new FillSymbol();

			_ToleranceSymbol.Color = ARANFunctions.RGB(0, 255, 255);
			_ToleranceSymbol.Style = eFillStyle.sfsCross;

			_PointSymbol = new PointSymbol(ePointStyle.smsCircle, ARANFunctions.RGB(2, 215, 72), 6);

			_ToleranceElement = -1;
			_PointElement = -1;
			_ptHtElem = -1;

			_MultiCoverage = false;

			_SensorType = eSensorType.GNSS;
			_PBNType = ePBNClass.RNP_APCH;
			//FlightPhase = eFlightPhase.Enroute;

			//_ConstMode = (eFIXRole)(-1);
			Role = InitialRole;

			switch (_FIXRole)
			{
				case eFIXRole.IAF_GT_56_:
					_PBNType = ePBNClass.RNAV1;
					FlightPhase = eFlightPhase.Enroute;
					break;
				case eFIXRole.IAF_LE_56_:
					FlightPhase = eFlightPhase.FAFApch;
					break;
				case eFIXRole.IF_:
					FlightPhase = eFlightPhase.IIAP;
					break;
				case eFIXRole.FAF_:
					FlightPhase = eFlightPhase.IIAP;
					break;
				case eFIXRole.MAPt_:
					FlightPhase = eFlightPhase.MApLT28;
					break;
				case eFIXRole.MATF_LE_56:
					FlightPhase = eFlightPhase.MApLT28;
					break;
				case eFIXRole.MATF_GT_56:
					FlightPhase = eFlightPhase.MApGE28;
					break;
				case eFIXRole.MAHF_LE_56:
					_PBNType = ePBNClass.RNAV1;
					FlightPhase = eFlightPhase.SIDGE28;
					break;
				case eFIXRole.MAHF_GT_56:
					_PBNType = ePBNClass.RNAV1;
					FlightPhase = eFlightPhase.SIDGE56;
					break;
				case eFIXRole.IDEP_:
					_PBNType = ePBNClass.RNAV1;
					FlightPhase = eFlightPhase.STAR;
					break;
				case eFIXRole.DEP_:
					_PBNType = ePBNClass.RNAV1;
					FlightPhase = eFlightPhase.STAR;
					break;
				case eFIXRole.TP_:
					_PBNType = ePBNClass.RNAV1;
					FlightPhase = eFlightPhase.STAR;
					break;
				case eFIXRole.PBN_IAF:
					FlightPhase = eFlightPhase.FAFApch;
					break;
				case eFIXRole.PBN_IF:
					FlightPhase = eFlightPhase.FAFApch;
					break;
				case eFIXRole.PBN_FAF:
					FlightPhase = eFlightPhase.FAFApch;
					break;
				case eFIXRole.PBN_MAPt:
					FlightPhase = eFlightPhase.MApLT28;
					break;
				case eFIXRole.PBN_MATF_LT_28:
					FlightPhase = eFlightPhase.MApLT28;
					break;
				case eFIXRole.PBN_MATF_GE_28:
					FlightPhase = eFlightPhase.MApGE28;
					break;
				case eFIXRole.DEP_ST:
					_PBNType = ePBNClass.RNAV1;
					FlightPhase = eFlightPhase.SID;
					break;
				//default:
				//	_PBNType = ePBNClass.RNAV1;
				//	_FlightPhase = eFlightPhase.Enroute;
				//	break;
			}

			//CalcWPTParams();
			_DrawingEnabled = true;
		}

		public WayPoint(eFIXRole InitialRole, WPT_FIXType fix, IAranEnvironment aranEnv)
		{
			if (_constants == null)
				_constants = new Aran.PANDA.Constants.Constants();

			if (_pspatialReferenceOperation == null)
				_pspatialReferenceOperation = new SpatialReferenceOperation(aranEnv);

			_aranEnv = aranEnv;
			_UI = aranEnv.Graphics;

			CallSign = fix.CallSign;
			_Name = fix.Name;
			_Id = fix.Identifier;
			
			_BasePoints = new List<Point>();
			_PrjPt = (Point)fix.pPtPrj.Clone();
			_GeoPt = _pspatialReferenceOperation.ToGeo<Point>(_PrjPt);

			_PrjPtH = (Point)_PrjPt.Clone();
			_GeoPtH = (Point)_GeoPt.Clone();

			_TolerArea = new MultiPolygon();

			_FlyByTechTol = _constants.Pansops[ePANSOPSData.rnvFlyByTechTol].Value;
			_FlyOTechTol = _constants.Pansops[ePANSOPSData.rnvFlyOTechTol].Value;

			_ImMaxIntercept = _constants.Pansops[ePANSOPSData.arImMaxIntercept].Value;
			_FlyOInterBank = _constants.Pansops[ePANSOPSData.rnvFlyOInterBank].Value;

			HorAccuracy = fix.HorAccuracy;

			_MagVar = 0.0;
			//_EntryDir_N = 
				_EntryDir = 0.0;
			//_OutDir_N = 
				_outDir = 0.0;

			_TurnDir = TurnDirection.NONE;
			_Propagated = TurnDirection.NONE;

			_ToleranceSymbol = new FillSymbol();
			_ToleranceSymbol.Color = ARANFunctions.RGB(0, 255, 255);
			_ToleranceSymbol.Style = eFillStyle.sfsCross;
			_PointSymbol = new PointSymbol(ePointStyle.smsCircle, ARANFunctions.RGB(2, 215, 72), 6);

			_ToleranceElement = -1;
			_PointElement = -1;
			_ptHtElem = -1;

			_MultiCoverage = false;

			_SensorType = eSensorType.GNSS;
			_PBNType = ePBNClass.RNP_APCH;
			//_FlightPhase = eFlightPhase.Enroute;

			//_ConstMode = (eFIXRole)(-1);
			Role = InitialRole;

			switch (_FIXRole)
			{
				case eFIXRole.IAF_GT_56_:
					_PBNType = ePBNClass.RNAV1;
					FlightPhase = eFlightPhase.Enroute;
					break;
				case eFIXRole.IAF_LE_56_:
					FlightPhase = eFlightPhase.FAFApch;
					break;
				case eFIXRole.IF_:
					FlightPhase = eFlightPhase.IIAP;
					break;
				case eFIXRole.FAF_:
					FlightPhase = eFlightPhase.IIAP;
					break;
				case eFIXRole.MAPt_:
					FlightPhase = eFlightPhase.MApLT28;
					break;
				case eFIXRole.MATF_LE_56:
					FlightPhase = eFlightPhase.MApLT28;
					break;
				case eFIXRole.MATF_GT_56:
					FlightPhase = eFlightPhase.MApGE28;
					break;
				case eFIXRole.MAHF_LE_56:
					_PBNType = ePBNClass.RNAV1;
					FlightPhase = eFlightPhase.SIDGE28;
					break;
				case eFIXRole.MAHF_GT_56:
					_PBNType = ePBNClass.RNAV1;
					FlightPhase = eFlightPhase.SIDGE56;
					break;
				case eFIXRole.IDEP_:
					_PBNType = ePBNClass.RNAV1;
					FlightPhase = eFlightPhase.STAR;
					break;
				case eFIXRole.DEP_:
					_PBNType = ePBNClass.RNAV1;
					FlightPhase = eFlightPhase.STAR;
					break;
				case eFIXRole.TP_:
					_PBNType = ePBNClass.RNAV1;
					FlightPhase = eFlightPhase.STAR;
					break;
				case eFIXRole.PBN_IAF:
					FlightPhase = eFlightPhase.FAFApch;
					break;
				case eFIXRole.PBN_IF:
					FlightPhase = eFlightPhase.FAFApch;
					break;
				case eFIXRole.PBN_FAF:
					FlightPhase = eFlightPhase.FAFApch;
					break;
				case eFIXRole.PBN_MAPt:
					FlightPhase = eFlightPhase.MApLT28;
					break;
				case eFIXRole.PBN_MATF_LT_28:
					FlightPhase = eFlightPhase.MApLT28;
					break;
				case eFIXRole.PBN_MATF_GE_28:
					FlightPhase = eFlightPhase.MApGE28;
					break;
				case eFIXRole.DEP_ST:
					_PBNType = ePBNClass.RNAV1;
					FlightPhase = eFlightPhase.SID;
					break;
				//default:
				//	_PBNType = ePBNClass.RNAV1;
				//	_FlightPhase = eFlightPhase.Enroute;
				//	break;
			}

			_DrawingEnabled = true;
		}

		public void AssignTo(IAranCloneable Dest)
		{
			Dest.Assign(this);
		}

		//public virtual void Assign(WPT_FIXType source)
		//{
		//	_Name = source.Name;
		//	//ID = source.Identifier;
		//	_PrjPt = (Point)source.pPtPrj.Clone();
		//	_GeoPt = _pspatialReferenceOperation.ToGeo<Point>(_PrjPt);
		//}

		public virtual void Assign(AranObject source)
		{
			if (source == this) return;

			WayPoint AnOther = (WayPoint)source;

			_AIXMType = AnOther._AIXMType;

			_PrjPt.Assign(AnOther._PrjPt);
			_GeoPt.Assign(AnOther._GeoPt);

			_PrjPtH.Assign(AnOther._PrjPtH);
			_GeoPtH.Assign(AnOther._GeoPtH);

			_TolerArea.Assign(AnOther._TolerArea);

			_TurnDir = AnOther._TurnDir;
			_TurnDir_L = AnOther._TurnDir_L;
			_TurnDir_R = AnOther._TurnDir_R;
			_Propagated = AnOther._Propagated;

			_VOR_DME = AnOther._VOR_DME;

			_MagVar = AnOther._MagVar;
			_EntryDir = AnOther._EntryDir;
			//_EntryDir_N = AnOther._EntryDir_N;
			_EntryDir_L = AnOther._EntryDir_L;
			_EntryDir_R = AnOther._EntryDir_R;

			_outDir = AnOther._outDir;
			//_OutDir_N = AnOther._OutDir_N;
			_OutDir_L = AnOther._OutDir_L;
			_OutDir_R = AnOther._OutDir_R;

			_TurnAngle = AnOther._TurnAngle;
			_TurnAngle_L = AnOther._TurnAngle_L;
			_TurnAngle_R = AnOther._TurnAngle_R;
			_OCH = AnOther._OCH;

			_BankAngle = AnOther._BankAngle;
			_PilotTime = AnOther._PilotTime;
			_BankTime = AnOther._BankTime;

			_FTE = AnOther._FTE;
			_XTT = AnOther._XTT;
			_ATT = AnOther._ATT;

			_EPT = AnOther._EPT;
			_EPT_L = AnOther._EPT_L;
			_EPT_R = AnOther._EPT_R;

			_LPT = AnOther._LPT;
			_LPT_L = AnOther._LPT_L;
			_LPT_R = AnOther._LPT_R;

			_ASW_L = AnOther._ASW_L;
			_ASW_2_L = AnOther._ASW_2_L;
			_ASW_R = AnOther._ASW_R;
			_ASW_2_R = AnOther._ASW_2_R;
			_SemiWidth = AnOther._SemiWidth;

			_FlightPhase = AnOther._FlightPhase;

			_FIXRole = AnOther._FIXRole;
			CallSign = AnOther.CallSign;
			_Id = AnOther._Id;
			_Name = AnOther._Name;

			_SensorType = AnOther._SensorType;
			_PBNType = AnOther._PBNType;

			HorAccuracy = AnOther.HorAccuracy;

			_constructAltitude = AnOther._constructAltitude;
			_nomLineAltitude = AnOther._nomLineAltitude;

			_constructionGradient = AnOther._constructionGradient;
			_nomLineGradient = AnOther._nomLineGradient;
			_appliedGradient = AnOther._appliedGradient;

			_FlyMode = AnOther._FlyMode;
			_ImMaxIntercept = AnOther._ImMaxIntercept;
			_FlyOInterBank = AnOther._FlyOInterBank;
			_FlyByTechTol = AnOther._FlyByTechTol;
			_FlyOTechTol = AnOther._FlyOTechTol;
			_ISAtC = AnOther._ISAtC;
			_IAS = AnOther._IAS;
			_ConstructTAS = AnOther._ConstructTAS;
			_NomLineTAS = AnOther._NomLineTAS;

			_UI = AnOther._UI;
			_IsDFTarget = AnOther._IsDFTarget;

			//AnOther._BasePoints.CopyTo(
			//_BasePoints.
			_BasePoints.Clear();
			_BasePoints.AddRange(AnOther._BasePoints);

			_ToleranceSymbol.Assign(AnOther._ToleranceSymbol);
			_PointSymbol.Assign(AnOther._PointSymbol);

			_MultiCoverage = AnOther._MultiCoverage;
			_DrawingEnabled = AnOther._DrawingEnabled;

			//DeleteGraphics();
			//_ToleranceElement = -1;
			//_PointElement = -1;
			//_ptHtElem = -1;
		}

		public virtual AranObject Clone()
		{
			WayPoint clone = new WayPoint(_aranEnv);
			AssignTo(clone);
			return clone;
		}

		public double _CalcFromMinStablizationDistance(double TurnAngle, double tas, bool IsDF = false)
		{
			double result = _ATT;

			if (ARANMath.RadToDeg(TurnAngle) > 2.0)
			{
				//double TurnR = ARANMath.BankToRadius(_BankAngle, _TAS);

				double Rv = (1.76527777777777777777 / Math.PI) * Math.Tan(_BankAngle) / tas;
				if (Rv > 0.003) Rv = 0.003;

				double R1Turn;
				if (Rv > 0.0) R1Turn = tas / ((5.555555555555555555555 * Math.PI) * Rv);
				else R1Turn = -1;

				if (IsDF)// || _FlyMode == eFlyMode.Atheight)
				{
					double WSpeed = _constants.Pansops[ePANSOPSData.dpWind_Speed].Value;
					double coeff = WSpeed / ARANMath.DegToRad(1000.0 * Rv);

					result = _ASW_L + TurnAngle * coeff;
				}
				else
				{
					//if (ARANMath.RadToDeg(TurnAngle) < 50.0)
					//	TurnAngle = ARANMath.DegToRad(50.0);

					if (_FlyMode == eFlyMode.Flyby)
						result = R1Turn * Math.Tan(0.5 * TurnAngle) + _FlyByTechTol * tas;
					else //if (_FlyMode == eFlyMode.Flyover)
					{
						double SinTurnAngle = Math.Sin(TurnAngle);
						double CosTurnAngle = Math.Cos(TurnAngle);

						double SinFImMaxIntercept = Math.Sin(_ImMaxIntercept);
						double CosFImMaxIntercept = Math.Cos(_ImMaxIntercept);

						double R2Turn = ARANMath.BankToRadius(_FlyOInterBank, tas);

						result = R1Turn * (SinTurnAngle + CosTurnAngle * SinFImMaxIntercept / CosFImMaxIntercept +
								1.0 / SinFImMaxIntercept - CosTurnAngle / (SinFImMaxIntercept * CosFImMaxIntercept)) +
								R2Turn * Math.Tan(0.5 * _ImMaxIntercept) + _FlyOTechTol * tas;
					}
				}
			}

			return Math.Max(result, _ATT);
		}


		//_Construction

		public double CalcConstructionFromMinStablizationDistance(double TurnAngle, bool IsDF = false)
		{
			double result = _ATT;

			if (ARANMath.RadToDeg(TurnAngle) > 2.0)
			{
				//double TurnR = ARANMath.BankToRadius(_BankAngle, _TAS);

				double Rv = (1.76527777777777777777 / Math.PI) * Math.Tan(_BankAngle) / _ConstructTAS;
				if (Rv > 0.003) Rv = 0.003;

				double R1Turn;
				if (Rv > 0.0) R1Turn = _ConstructTAS / ((5.555555555555555555555 * Math.PI) * Rv);
				else R1Turn = -1;

				if (IsDF)// || _FlyMode == eFlyMode.Atheight)
				{
					double WSpeed = _constants.Pansops[ePANSOPSData.dpWind_Speed].Value;
					double coeff = WSpeed / ARANMath.DegToRad(1000.0 * Rv);

					result = _ASW_L + TurnAngle * coeff;
				}
				else
				{
					//if (ARANMath.RadToDeg(TurnAngle) < 50.0)
					//	TurnAngle = ARANMath.DegToRad(50.0);

					if (_FlyMode == eFlyMode.Flyby)
						result = R1Turn * Math.Tan(0.5 * TurnAngle) + _FlyByTechTol * _ConstructTAS;
					else //if (_FlyMode == eFlyMode.Flyover)
					{
						double SinTurnAngle = Math.Sin(TurnAngle);
						double CosTurnAngle = Math.Cos(TurnAngle);

						double SinFImMaxIntercept = Math.Sin(_ImMaxIntercept);
						double CosFImMaxIntercept = Math.Cos(_ImMaxIntercept);

						double R2Turn = ARANMath.BankToRadius(_FlyOInterBank, _ConstructTAS);

						result = R1Turn * (SinTurnAngle + CosTurnAngle * SinFImMaxIntercept / CosFImMaxIntercept +
								1.0 / SinFImMaxIntercept - CosTurnAngle / (SinFImMaxIntercept * CosFImMaxIntercept)) +
								R2Turn * Math.Tan(0.5 * _ImMaxIntercept) + _FlyOTechTol * _ConstructTAS;
					}
				}
			}

			return Math.Max(result, _ATT);
		}

		public double CalcConstructionFromMinStablizationDistance(bool IsDF = false)
		{
			return CalcConstructionFromMinStablizationDistance(_TurnAngle, IsDF);
		}

		//_NomLine
		public double CalcNomLineFromMinStablizationDistance(double TurnAngle, bool IsDF = false)
		{
			double result = _ATT;

			if (ARANMath.RadToDeg(TurnAngle) > 2.0)
			{
				//double TurnR = ARANMath.BankToRadius(_BankAngle, _TAS);

				double Rv = (1.76527777777777777777 / Math.PI) * Math.Tan(_BankAngle) / _NomLineTAS;
				if (Rv > 0.003) Rv = 0.003;

				double R1Turn;
				if (Rv > 0.0) R1Turn = _NomLineTAS / ((5.555555555555555555555 * Math.PI) * Rv);
				else R1Turn = -1;

				if (IsDF)// || _FlyMode == eFlyMode.Atheight)
				{
					double WSpeed = _constants.Pansops[ePANSOPSData.dpWind_Speed].Value;
					double coeff = WSpeed / ARANMath.DegToRad(1000.0 * Rv);

					result = _ASW_L + TurnAngle * coeff;
				}
				else
				{
					//if (ARANMath.RadToDeg(TurnAngle) < 50.0)
					//	TurnAngle = ARANMath.DegToRad(50.0);

					if (_FlyMode == eFlyMode.Flyby)
						result = R1Turn * Math.Tan(0.5 * TurnAngle) + _FlyByTechTol * _NomLineTAS;
					else //if (_FlyMode == eFlyMode.Flyover)
					{
						double SinTurnAngle = Math.Sin(TurnAngle);
						double CosTurnAngle = Math.Cos(TurnAngle);

						double SinFImMaxIntercept = Math.Sin(_ImMaxIntercept);
						double CosFImMaxIntercept = Math.Cos(_ImMaxIntercept);

						double R2Turn = ARANMath.BankToRadius(_FlyOInterBank, _NomLineTAS);

						result = R1Turn * (SinTurnAngle + CosTurnAngle * SinFImMaxIntercept / CosFImMaxIntercept +
								1.0 / SinFImMaxIntercept - CosTurnAngle / (SinFImMaxIntercept * CosFImMaxIntercept)) +
								R2Turn * Math.Tan(0.5 * _ImMaxIntercept) + _FlyOTechTol * _NomLineTAS;
					}
				}
			}

			return Math.Max(result, _ATT);
		}

		public double CalcNomLineFromMinStablizationDistance(bool IsDF = false)
		{
			return CalcNomLineFromMinStablizationDistance(_TurnAngle, IsDF);
		}

		public double CalcConstructionInToMinStablizationDistance(double TurnAngle)
		{
			double result = _ATT;

			if (_FlyMode == eFlyMode.Flyby && ARANMath.RadToDeg(TurnAngle) > 2.0)
			{
				//if (ARANMath.RadToDeg(TurnAngle) < 50.0)
				//	TurnAngle = ARANMath.DegToRad(50.0);

				  result = ARANMath.BankToRadiusForRnav(_BankAngle, _IAS, _constructAltitude, _constants.Pansops[ePANSOPSData.ISA].Value) * Math.Tan(0.5 * TurnAngle) + _FlyByTechTol * _ConstructTAS;
			}

			return Math.Max(result, _ATT);
		}

		public double CalcConstructionInToMinStablizationDistance()
		{
			return CalcConstructionInToMinStablizationDistance(_TurnAngle);
		}

		public double CalcNomLineInToMinStablizationDistance(double TurnAngle)
		{
			double result = _ATT;

			if (_FlyMode == eFlyMode.Flyby && ARANMath.RadToDeg(TurnAngle) > 2.0)
			{
				//if (ARANMath.RadToDeg(TurnAngle) < 50.0)
				//	TurnAngle = ARANMath.DegToRad(50.0);

				result = ARANMath.BankToRadiusForRnav(_BankAngle, _IAS, _nomLineAltitude, _constants.Pansops[ePANSOPSData.ISA].Value) * Math.Tan(0.5 * TurnAngle) + _FlyByTechTol * _NomLineTAS;
			}

			return Math.Max(result, _ATT);
		}

		public double CalcNomLineInToMinStablizationDistance()
		{
			return CalcNomLineInToMinStablizationDistance(_TurnAngle);
		}
		
		public virtual void DeleteGraphics()
		{
			_UI.SafeDeleteGraphic(_ToleranceElement);
			_UI.SafeDeleteGraphic(_PointElement);
			_UI.SafeDeleteGraphic(_ptHtElem);

			_ToleranceElement = -1;
			_PointElement = -1;
			_ptHtElem = -1;
		}

		public virtual void RefreshGraphics()
		{
			DeleteGraphics();

			if (!_DrawingEnabled)
				return;

			String roleText = "";

			//LegBase.ProcessMessages();

			if (_FlyMode != eFlyMode.Atheight)			//?????
				_ToleranceElement = _UI.DrawMultiPolygon(_TolerArea, _ToleranceSymbol);
			//LegBase.ProcessMessages();

			if ((int)_FIXRole >= 0)
				roleText = SensorConstant.FIXRoleStyleNames[(int)_FIXRole];

			if (_Name != "" && roleText != "")
				roleText = roleText + " / " + _Name;
			else if (_Name != "")
				roleText = _Name;

			_PointElement = _UI.DrawPointWithText(_PrjPt, roleText, _PointSymbol);

			if (_FlyMode == eFlyMode.Atheight)
				_ptHtElem = _UI.DrawPointWithText(PrjPtH, "HT", ARANFunctions.RGB(127, 255, 0));
		}

		public void ReCreateArea(bool recalcTurndir = false )
		{
			CalcWPTParams();
			CalcTurnRangePoints(!recalcTurndir);
			//CalcExtraTurnRangePoints();
			if (!_PrjPt.IsEmpty)
				CreateTolerArea();
		}

		public double HorAccuracy { get; set; }

		public string CallSign { get; set; }

		public string Name
		{
			get
			{
				if (_Name != "")
					return _Name;
				else if ((int)_FIXRole >= 0)
					return SensorConstant.FIXRoleStyleNames[(int)_FIXRole];
				else
					return "";
			}

			set
			{
				_Name = value;
				RefreshGraphics();
			}
		}

		public Guid Id
		{
			get { return _Id; }

			set { _Id = value; }
		}

		public override string ToString()
		{
			return Name;
		}

		virtual public Point NomLinePrjPt
		{
			get
			{
				if (_FlyMode == eFlyMode.Atheight)
					return _PrjPtH;
				else
					return _PrjPt;
			}
		}

		virtual public Point NomLineGeoPt
		{
			get
			{
				if (_FlyMode == eFlyMode.Atheight)
					return _GeoPtH;
				else
					return _GeoPt;
			}
		}

		virtual public Point PrjPt
		{
			get { return _PrjPt; }

			set
			{
				_PrjPt.Assign(value);
				if (!_PrjPt.IsEmpty)
				{
					Point tmpPt = _pspatialReferenceOperation.ToGeo<Point>(_PrjPt);
					_GeoPt.Assign(tmpPt);
					ReCreateArea();
				}
				else
					_GeoPt.Assign(value);

				_BasePoints.Clear();
			}
		}

		public Point GeoPt { get { return _GeoPt; } }

		virtual public Point PrjPtH
		{
			get { return _PrjPtH; }

			set
			{
				_PrjPtH.Assign(value);
				if (!_PrjPtH.IsEmpty)
				{
					Point tmpPt = _pspatialReferenceOperation.ToGeo<Point>(_PrjPtH);
					_GeoPtH.Assign(tmpPt);
					//ReCreateArea();
				}
				else
					_GeoPtH.Assign(value);

				//_BasePoints.Clear();
			}
		}

		public Point GeoPtH
		{
			get { return _GeoPtH; }
		}

		public eFlightPhase FlightPhase
		{
			get { return _FlightPhase; }

			set
			{
				//if (_FlightPhase != value)
				{
					_FlightPhase = value;

					switch (_FlightPhase)
					{
						case eFlightPhase.Enroute:
							_PilotTime = _constants.Pansops[ePANSOPSData.enTechTolerance].Value;
							_BankTime = _constants.Pansops[ePANSOPSData.enBankTolerance].Value;
							_BankAngle = _constants.Pansops[ePANSOPSData.dpT_Bank].Value;
							if (_PBNType == ePBNClass.RNP_APCH)
								_PBNType = ePBNClass.RNAV1;
							if (_PBNType == ePBNClass.RNP1)
								_PBNType = ePBNClass.RNP4;
							break;

						case eFlightPhase.SIDGE56:
						case eFlightPhase.SIDGE28:
						case eFlightPhase.SID:
							_PilotTime = _constants.Pansops[ePANSOPSData.dpPilotTolerance].Value;
							_BankTime = _constants.Pansops[ePANSOPSData.dpBankTolerance].Value;
							_BankAngle = _constants.Pansops[ePANSOPSData.dpT_Bank].Value;
							if (_PBNType == ePBNClass.RNP_APCH)
								_PBNType = ePBNClass.RNAV1;
							break;

						case eFlightPhase.STARGE56:
						case eFlightPhase.STAR:
							_PilotTime = _constants.Pansops[ePANSOPSData.arIPilotToleranc].Value;
							_BankTime = _constants.Pansops[ePANSOPSData.arIBankTolerance].Value;
							_BankAngle = _constants.Pansops[ePANSOPSData.arBankAngle].Value;
							if (_PBNType == ePBNClass.RNP_APCH)
								_PBNType = ePBNClass.RNAV1;
							break;

						case eFlightPhase.IIAP:
							_PilotTime = _constants.Pansops[ePANSOPSData.arIPilotToleranc].Value;
							_BankTime = _constants.Pansops[ePANSOPSData.arIBankTolerance].Value;
							_BankAngle = _constants.Pansops[ePANSOPSData.arBankAngle].Value;
							break;

						case eFlightPhase.FAFApch:
							_PilotTime = _constants.Pansops[ePANSOPSData.arIPilotToleranc].Value;
							_BankTime = _constants.Pansops[ePANSOPSData.arIBankTolerance].Value;
							_BankAngle = _constants.Pansops[ePANSOPSData.arBankAngle].Value;
							_PBNType = ePBNClass.RNP_APCH;
							break;

						case eFlightPhase.MApLT28:
						case eFlightPhase.MApGE28:
							_PilotTime = _constants.Pansops[ePANSOPSData.arMAPilotToleran].Value;
							_BankTime = _constants.Pansops[ePANSOPSData.arMAPilotToleran].Value;
							_BankAngle = _constants.Pansops[ePANSOPSData.dpT_Bank].Value;
							_PBNType = ePBNClass.RNP_APCH;
							break;
					};

					ReCreateArea();
				}
			}
		}

		public eFIXRole Role
		{
			get { return _FIXRole; }

			set
			{
				if (_FIXRole == value)
					return;

					_FIXRole = value;

				//CalcWPTParams();
			}
		}

		//public eFIXRole ConstMode { get { return _ConstMode; } }

		public eFlyMode FlyMode
		{
			get { return _FlyMode; }
			set
			{
				if (_FlyMode != value)
				{
					_FlyMode = value;
					CalcTurnRangePoints();
				}
			}
		}

		public eSensorType SensorType
		{
			get { return _SensorType; }
			set
			{
				if (_SensorType != value)
				{
					_SensorType = value;
					ReCreateArea();
				}
			}
		}

		public ePBNClass PBNType
		{
			get { return _PBNType; }
			set
			{
				if (_PBNType != value)
				{
					_PBNType = value;
					ReCreateArea();
				}
			}
		}

		public bool MultiCoverage
		{
			get { return _MultiCoverage; }
			set
			{
				if (_MultiCoverage != value)
				{
					_MultiCoverage = value;
					if (_SensorType == eSensorType.DME_DME)
						ReCreateArea();
				}
			}
		}

		public double ISAtC
		{
			get { return _ISAtC; }
			set
			{
				if (_ISAtC != value)
				{
					_ISAtC = value;
					//_TAS = ARANMath.IASToTASForRnav(_IAS, _TurnAltitude, _ISA);
					//ReCreateArea();
				}
			}
		}

		bool _IsDFTarget;

		public bool IsDFTarget
		{
			get { return _IsDFTarget; }
			set
			{
				if (_IsDFTarget != value)
				{
					_IsDFTarget = value;
					_BasePoints.Clear();
				}
			}
		}

		public double CalcTurnRadius(double tas)
		{
			return ARANMath.BankToRadius(_BankAngle, tas);
		}

		public double ConstructTurnRadius
		{
			//Bank15
			get { return ARANMath.BankToRadius(_constants.Pansops[ePANSOPSData.rnvFlyOInterBank].Value, _ConstructTAS); }	//_BankAngle
		}

		public double NomLineTurnRadius
		{
			get { return ARANMath.BankToRadius(_BankAngle, _NomLineTAS); }
		}
		
		public double ConstructAltitude
		{
			get { return _constructAltitude; }

			set
			{
				if (_constructAltitude != value)
				{
					_constructAltitude = value;

					//_ConstructTAS = ARANMath.IASToTASForRnav(_IAS, _constructAltitude, _constants.Pansops[ePANSOPSData.ISA].Value);
					_ConstructTAS = ARANMath.IASToTAS(_IAS, _constructAltitude, _constants.Pansops[ePANSOPSData.ISA].Value);
					ReCreateArea();
				}
			}
		}

		public double NomLineAltitude
		{
			get { return _nomLineAltitude; }

			set
			{
				if (_nomLineAltitude != value)
				{
					_nomLineAltitude = value;

					_NomLineTAS = ARANMath.IASToTAS(_IAS, _nomLineAltitude, _constants.Pansops[ePANSOPSData.ISA].Value);
					//ReCreateArea();
				}
			}
		}

		public double IAS
		{
			get { return _IAS; }

			set
			{
				if (_IAS != value)
				{
					_IAS = value;
					//_ConstructTAS = ARANMath.IASToTASForRnav(_IAS, _constructAltitude, _constants.Pansops[ePANSOPSData.ISA].Value);	//_ISA
					_ConstructTAS = ARANMath.IASToTAS(_IAS, _constructAltitude, _constants.Pansops[ePANSOPSData.ISA].Value);	//_ISA
					_NomLineTAS = ARANMath.IASToTAS(_IAS, _nomLineAltitude, _constants.Pansops[ePANSOPSData.ISA].Value);
					//_TAS = ARANMath.IASToTAS(_IAS, _TurnAltitude, _ISA);
					ReCreateArea();
				}
			}
		}

		public double ConstructTAS { get { return _ConstructTAS; } }
		public double NomLineTAS { get { return _NomLineTAS; } }

		public double ConstructionGradient
		{
			get { return _constructionGradient; }
			set { _constructionGradient = value; }
		}

		public double NomLineGradient
		{
			get { return _nomLineGradient; }
			set { _nomLineGradient = value; }
		}

		public double AppliedGradient
		{
			get { return _appliedGradient; }
			set { _appliedGradient = value; }
		}
		public double BankAngle
		{
			get { return _BankAngle; }

			set
			{
				if (_BankAngle != value)
				{
					_BankAngle = value;
					ReCreateArea();
				}
			}
		}
		
		public double OutDirection
		{
			get { return _outDir; }

			set
			{
				if (_outDir == value)
					return;

				//if(DFTarget)
				//_OutDir_N = 
					_outDir = value;
				_OutDir_L = _OutDir_R = value;
				ReCreateArea();
			}
		}

		//public double OutDirection_N
		//{
		//	get { return _OutDir_N; }
		//	set { _OutDir_N = value; }
		//}

		public double OutDirection_L
		{
			get { return _OutDir_L; }

			set
			{
				if (_OutDir_L != value)
				{
					_OutDir_L = value;
					CalcExtraTurnRangePoints();
				}
			}
		}

		public double OutDirection_R
		{
			get { return _OutDir_R; }

			set
			{
				if (_OutDir_R != value)
				{
					_OutDir_R = value;
					CalcExtraTurnRangePoints();
				}
			}
		}

		public NavaidType VOR_DME
		{
			get { return _VOR_DME; }
			set { _VOR_DME = value; }
		}

		public double MagVar
		{
			get { return _MagVar; }

			set { _MagVar = value; }
		}

		public double EntryDirection
		{
			get { return _EntryDir; }

			set
			{
				if (_EntryDir == value)
					return;

				//_EntryDir_N = 
					_EntryDir = value;
				_EntryDir_L = _EntryDir_R = value;
				ReCreateArea();
			}
		}

		//public double EntryDirection_N
		//{
		//	get { return _EntryDir_N; }
		//	set { _EntryDir_N = value; }
		//}

		public double EntryDirection_L
		{
			get { return _EntryDir_L; }

			set
			{
				if (_EntryDir_L != value)
				{
					_EntryDir_L = value;
					CalcExtraTurnRangePoints();
				}
			}
		}

		public double EntryDirection_R
		{
			get { return _EntryDir_R; }

			set
			{
				if (_EntryDir_R != value)
				{
					_EntryDir_R = value;
					CalcExtraTurnRangePoints();
				}
			}
		}

		public TurnDirection TurnDirection
		{
			get { return _TurnDir; }

			set
			{
				if (_TurnDir == value)
					return;

				_TurnDir = value;
				_TurnDir_L = value;
				_TurnDir_R = value;
				if (value != TurnDirection.NONE)
					_Propagated = value;
				else
				{
					_OutDir_L = _OutDir_R = _outDir;
				}

				ReCreateArea();
			}
		}

		public TurnDirection TurnDirection_L
		{
			get { return _TurnDir_L; }

			set
			{
				_TurnDir_L = value;
				CalcExtraTurnRangePoints();
			}
		}

		public TurnDirection TurnDirection_R
		{
			get { return _TurnDir_R; }

			set
			{
				_TurnDir_R = value;
				CalcExtraTurnRangePoints();
			}
		}

		public TurnDirection PropagatedTurnDirection
		{
			get { return _Propagated; }

			set
			{
				if (_TurnDir != TurnDirection.NONE)
					_Propagated = _TurnDir;
				else
					_Propagated = value;
			}
		}

		public TurnDirection EffectiveTurnDirection
		{
			get
			{
				TurnDirection result = _TurnDir;
				if (result != TurnDirection.NONE)
					return result;

				result = _Propagated;
				if (result != TurnDirection.NONE)
					return result;

				result = ARANMath.SideFrom2Angle(_outDir, _EntryDir);
				if (result != TurnDirection.NONE)
					return result;

				return TurnDirection.CCW;
			}
		}

		public double TurnAngle { get { return _TurnAngle; } }
		public double TurnAngle_L { get { return _TurnAngle_L; } }
		public double TurnAngle_R { get { return _TurnAngle_R; } }
		public MultiPolygon TolerArea { get { return _TolerArea; } }

		public bool DrawingEnabled
		{
			get { return _DrawingEnabled; }

			set
			{
				if (value != _DrawingEnabled)
				{
					_DrawingEnabled = value;
					if (!value)
						DeleteGraphics();
				}
			}
		}

		virtual public bool Visible
		{
			get { return _DrawingEnabled; }

			set
			{
				_DrawingEnabled = value;
				RefreshGraphics();
			}
		}

		List<Point> _BasePoints;

		public List<Point> BasePoints
		{
			get
			{
				if (_BasePoints.Count == 0)
				{
					double lEntryDir_L, lEntryDir_R;
					if (_IsDFTarget)
					{
						lEntryDir_L = _EntryDir_L;
						lEntryDir_R = _EntryDir_R;
					}
					else
						lEntryDir_L = lEntryDir_R = _EntryDir;

					Point BasePointr, BasePointl, ptTmp;
					Point BasePoint0r, BasePoint0l;
					double lLPT = (_FlyMode == eFlyMode.Flyby ? -_LPT_L : _LPT_L);
					ptTmp = ARANFunctions.LocalToPrj(_PrjPt, lEntryDir_L, lLPT, 0);
					BasePointl = ARANFunctions.LocalToPrj(ptTmp, lEntryDir_L + 0.5 * ARANMath.C_PI, _ASW_2_L, 0);

					lLPT = (_FlyMode == eFlyMode.Flyby ? -_LPT_R : _LPT_R);
					ptTmp = ARANFunctions.LocalToPrj(_PrjPt, lEntryDir_R, lLPT, 0);
					BasePointr = ARANFunctions.LocalToPrj(ptTmp, lEntryDir_R - 0.5 * ARANMath.C_PI, _ASW_2_R, 0);

					ptTmp = ARANFunctions.LocalToPrj(_PrjPt, lEntryDir_L + Math.PI, _EPT_L, 0);
					BasePoint0l = ARANFunctions.LocalToPrj(ptTmp, lEntryDir_L + 0.5 * ARANMath.C_PI, _ASW_2_L, 0);

					ptTmp = ARANFunctions.LocalToPrj(_PrjPt, lEntryDir_R + Math.PI, _EPT_R, 0);
					BasePoint0r = ARANFunctions.LocalToPrj(ptTmp, lEntryDir_R - 0.5 * ARANMath.C_PI, _ASW_2_R, 0);

					if (EffectiveTurnDirection == TurnDirection.CCW)
					{
						_BasePoints.Add(BasePointr);
						_BasePoints.Add(BasePointl);
						_BasePoints.Add(BasePoint0l);
						_BasePoints.Add(BasePoint0r);
					}
					else
					{
						_BasePoints.Add(BasePointl);
						_BasePoints.Add(BasePointr);
						_BasePoints.Add(BasePoint0r);
						_BasePoints.Add(BasePoint0l);
					}
				}

				return _BasePoints;
			}
		}

		//public double CalcDFOuterDirection(WayPoint EndFIX, out Point ptOut, LegDep PrevLeg = null)
		//{
		//	ptOut = null;

		//	double fTurnSide = -(int)_TurnDir;

		//	double WSpeed = _constants.Pansops[ePANSOPSData.dpWind_Speed].Value;
		//	double Rv = (1.76527777777777777777 / Math.PI) * Math.Tan(_BankAngle) / _TAS;
		//	if (Rv > 0.003) Rv = 0.003;

		//	double TurnR;
		//	if (Rv > 0.0) TurnR = _TAS / ((5.555555555555555555555 * Math.PI) * Rv);
		//	else TurnR = -1;

		//	double coeff = WSpeed / ARANMath.DegToRad(1000.0 * Rv);
		//	double dirSpCenter = _EntryDir - ARANMath.C_PI_2 * fTurnSide;
		//	double SpStartRadial = _EntryDir + ARANMath.C_PI_2 * fTurnSide;

		//	double TurnRCurr = TurnR;
		//	double dirCurr = _EntryDir + Math.Atan2(coeff, TurnRCurr) * fTurnSide;

		//	IList<Point> points;
		//	if (this._FlyMode == eFlyMode.Atheight && PrevLeg != null)
		//	{
		//		//Ring PrevRing = PrevLeg.PrimaryArea[0].ExteriorRing;
		//		//Polygon pPoly = new Polygon();
		//		//pPoly.ExteriorRing = PrevRing;

		//		points = ARANFunctions.GetBasePoints(_PrjPt, _EntryDir, PrevLeg.PrimaryArea[0], _TurnDir);
		//		//for (int i = 0; i < points.Count; i++)
		//		//{
		//		//    _UI.DrawPointWithText(points[i], -1, "points[" + i + "]");
		//		//    Leg.ProcessMessages();
		//		//}
		//	}
		//	else
		//	{
		//		if (PrevLeg != null)
		//		{
		//			Point ptLPTr = ARANFunctions.LocalToPrj(PrjPt, EntryDirection_R, LPT_R, 0);
		//			Point ptLPTl = ARANFunctions.LocalToPrj(PrjPt, EntryDirection_L, LPT_L, 0);

		//			Point ptEPTr = ARANFunctions.LocalToPrj(PrjPt, EntryDirection_R, -EPT_R, 0);
		//			Point ptEPTl = ARANFunctions.LocalToPrj(PrjPt, EntryDirection_L, -EPT_L, 0);

		//			double ASW_OUT0Fr = ASW_2_R;
		//			double ASW_OUT0Fl = ASW_2_L;
		//			double fTmp;

		//			Point ptTmp = ARANFunctions.LocalToPrj(ptLPTr, EntryDirection_R, -0.001);
		//			Point ptFromr = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDirection_R - 0.5 * ARANMath.C_PI, out fTmp);
		//			if (ptFromr != null)
		//				ASW_OUT0Fr = fTmp;
		//			else
		//				ptFromr = ARANFunctions.LocalToPrj(ptLPTr, EntryDirection_R, 0.0, -ASW_OUT0Fr);

		//			ptTmp = ARANFunctions.LocalToPrj(ptLPTl, EntryDirection_L, -0.001);
		//			Point ptFroml = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDirection_L + 0.5 * ARANMath.C_PI, out fTmp);
		//			if (ptFroml != null)
		//				ASW_OUT0Fl = fTmp;
		//			else
		//				ptFroml = ARANFunctions.LocalToPrj(ptLPTl, EntryDirection_L, 0.0, ASW_OUT0Fl);

		//			//_UI.DrawPolygon(PrevLeg.PrimaryArea[0], -1, eFillStyle.sfsDiagonalCross);
		//			//_UI.DrawPointWithText(ptFromr, -1, "ptFromR");
		//			//_UI.DrawPointWithText(ptFroml, -1, "ptFromL");
		//			//Leg.ProcessMessages();

		//			Point ptFrom0r = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptEPTr, EntryDirection_R - 0.5 * ARANMath.C_PI, out fTmp);
		//			Point ptFrom0l = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptEPTl, EntryDirection_L + 0.5 * ARANMath.C_PI, out fTmp);

		//			//_UI.DrawPointWithText(ptFrom0r, -1, "ptFromR0");
		//			//_UI.DrawPointWithText(ptFrom0l, -1, "ptFromL0");

		//			//_UI.DrawPointWithText(ptEPTr, -1, "ptEPTr");
		//			//_UI.DrawPointWithText(ptEPTl, -1, "ptEPTl");
		//			//Leg.ProcessMessages();

		//			points= new List<Point>();
		//			if (_TurnDir == TurnDirection.CCW)
		//			{
		//				points.Add(ptFromr);
		//				points.Add(ptFroml);

		//				if (ptFrom0l == null)
		//					ptFrom0l = BasePoints[2];
		//				points.Add(ptFrom0l);

		//				if (ptFrom0r == null)
		//					ptFrom0r = BasePoints[3];
		//				points.Add(ptFrom0r);

		//				//_UI.DrawPointWithText(ptFrom0r, -1, "ptFromR0");
		//				//_UI.DrawPointWithText(ptFrom0l, -1, "ptFromL0");
		//				//Leg.ProcessMessages();
		//			}
		//			else
		//			{
		//				points.Add(ptFroml);
		//				points.Add(ptFromr);

		//				if (ptFrom0r == null)
		//					ptFrom0r = BasePoints[2];
		//				points.Add(ptFrom0r);

		//				if (ptFrom0l == null)
		//					ptFrom0l = BasePoints[3];
		//				points.Add(ptFrom0l);
		//			}
		//		}
		//		else
		//			points = BasePoints;
		//	}

		//	int n = points.Count;

		//	double dirTouch = 0, SpTurnAngle = 0;
		//	Point ptCnt = null;

		//	for (int i = 0; i < n; i++)
		//	{
		//		ptCnt = ARANFunctions.LocalToPrj(points[i], dirSpCenter, TurnR, 0);

		//		//_UI.DrawPointWithText(ptCnt, -1, "ptCnt[" + i + "]");
		//		//    Leg.ProcessMessages();

		//		int j = i + 1 < n ? i + 1 : 0;

		//		double dirRef = Math.Atan2(points[j].Y - points[i].Y, points[j].X - points[i].X);
		//		double dirCntToPtDst = Math.Atan2(EndFIX.PrjPt.Y - ptCnt.Y, EndFIX.PrjPt.X - ptCnt.X);
		//		double fTmp = ARANMath.Modulus((dirCntToPtDst - dirRef) * fTurnSide, ARANMath.C_2xPI);

		//		if (fTmp < Math.PI)
		//		{
		//			SpTurnAngle = ARANFunctions.SpiralTouchToPoint(ptCnt, TurnRCurr, coeff, dirCurr, _TurnDir, EndFIX.PrjPt);

		//			if (SpTurnAngle > 100)
		//				return 9999.0;

		//			Rv = TurnRCurr + coeff * SpTurnAngle;
		//			dirTouch = SpStartRadial - (SpTurnAngle + ARANMath.C_PI_2 - Math.Atan2(coeff, Rv)) * fTurnSide;

		//			fTmp = ARANMath.Modulus((dirTouch - dirRef) * fTurnSide, ARANMath.C_2xPI);

		//			if (fTmp < Math.PI)
		//				break;
		//		}

		//		SpTurnAngle = ARANFunctions.SpiralTouchAngle(TurnRCurr, coeff, SpStartRadial, dirCurr, dirRef, _TurnDir);

		//		if (SpTurnAngle <= Math.PI)
		//		{
		//			TurnRCurr += SpTurnAngle * coeff;
		//			SpStartRadial -= SpTurnAngle * fTurnSide;
		//		}

		//		dirCurr = dirRef;
		//	}

		//	ptOut = ARANFunctions.LocalToPrj(ptCnt, SpStartRadial - SpTurnAngle * fTurnSide, Rv, 0);

		//	double result = ARANMath.Modulus(dirTouch, ARANMath.C_2xPI);

		//	if (_TurnDir == TurnDirection.CW)
		//		_OutDir_L = result;
		//	else
		//		_OutDir_R = result;

		//	ptOut.M = result;

		//	return result;
		//}

		//public double CalcDFOuterDirection(WayPoint EndFIX, out Point ptOut, LegSBAS PrevLeg = null)
		//{
		//	ptOut = null;

		//	double fTurnSide = -(int)_TurnDir;

		//	double WSpeed = _constants.Pansops[ePANSOPSData.dpWind_Speed].Value;
		//	double Rv = (1.76527777777777777777 / Math.PI) * Math.Tan(_BankAngle) / _TAS;
		//	if (Rv > 0.003) Rv = 0.003;

		//	double TurnR;
		//	if (Rv > 0.0) TurnR = _TAS / ((5.555555555555555555555 * Math.PI) * Rv);
		//	else TurnR = -1;

		//	double coeff = WSpeed / ARANMath.DegToRad(1000.0 * Rv);
		//	double dirSpCenter = _EntryDir - ARANMath.C_PI_2 * fTurnSide;
		//	double SpStartRadial = _EntryDir + ARANMath.C_PI_2 * fTurnSide;

		//	double TurnRCurr = TurnR;
		//	double dirCurr = _EntryDir + Math.Atan2(coeff, TurnRCurr) * fTurnSide;

		//	IList<Point> points;
		//	if (this._FlyMode == eFlyMode.Atheight && PrevLeg != null)
		//	{
		//		//Ring PrevRing = PrevLeg.PrimaryArea[0].ExteriorRing;
		//		//Polygon pPoly = new Polygon();
		//		//pPoly.ExteriorRing = PrevRing;

		//		points = ARANFunctions.GetBasePoints(_PrjPt, _EntryDir, PrevLeg.PrimaryArea[0], _TurnDir);
		//		//for (int i = 0; i < points.Count; i++)
		//		//{
		//		//    _UI.DrawPointWithText(points[i], -1, "points[" + i + "]");
		//		//    Leg.ProcessMessages();
		//		//}
		//	}
		//	else
		//	{
		//		if (PrevLeg != null)
		//		{
		//			Point ptLPTr = ARANFunctions.LocalToPrj(PrjPt, EntryDirection_R, LPT_R, 0);
		//			Point ptLPTl = ARANFunctions.LocalToPrj(PrjPt, EntryDirection_L, LPT_L, 0);

		//			Point ptEPTr = ARANFunctions.LocalToPrj(PrjPt, EntryDirection_R, -EPT_R, 0);
		//			Point ptEPTl = ARANFunctions.LocalToPrj(PrjPt, EntryDirection_L, -EPT_L, 0);

		//			double ASW_OUT0Fr = ASW_2_R;
		//			double ASW_OUT0Fl = ASW_2_L;
		//			double fTmp;

		//			Point ptTmp = ARANFunctions.LocalToPrj(ptLPTr, EntryDirection_R, -0.001);
		//			Point ptFromr = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDirection_R - 0.5 * ARANMath.C_PI, out fTmp);
		//			if (ptFromr != null)
		//				ASW_OUT0Fr = fTmp;
		//			else
		//				ptFromr = ARANFunctions.LocalToPrj(ptLPTr, EntryDirection_R, 0.0, -ASW_OUT0Fr);

		//			ptTmp = ARANFunctions.LocalToPrj(ptLPTl, EntryDirection_L, -0.001);
		//			Point ptFroml = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDirection_L + 0.5 * ARANMath.C_PI, out fTmp);
		//			if (ptFroml != null)
		//				ASW_OUT0Fl = fTmp;
		//			else
		//				ptFroml = ARANFunctions.LocalToPrj(ptLPTl, EntryDirection_L, 0.0, ASW_OUT0Fl);

		//			//_UI.DrawPolygon(PrevLeg.PrimaryArea[0], -1, eFillStyle.sfsDiagonalCross);
		//			//_UI.DrawPointWithText(ptFromr, -1, "ptFromR");
		//			//_UI.DrawPointWithText(ptFroml, -1, "ptFromL");
		//			//Leg.ProcessMessages();

		//			Point ptFrom0r = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptEPTr, EntryDirection_R - 0.5 * ARANMath.C_PI, out fTmp);
		//			Point ptFrom0l = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptEPTl, EntryDirection_L + 0.5 * ARANMath.C_PI, out fTmp);

		//			//_UI.DrawPointWithText(ptFrom0r, -1, "ptFromR0");
		//			//_UI.DrawPointWithText(ptFrom0l, -1, "ptFromL0");

		//			//_UI.DrawPointWithText(ptEPTr, -1, "ptEPTr");
		//			//_UI.DrawPointWithText(ptEPTl, -1, "ptEPTl");
		//			//Leg.ProcessMessages();

		//			points = new List<Point>();
		//			if (_TurnDir == TurnDirection.CCW)
		//			{
		//				points.Add(ptFromr);
		//				points.Add(ptFroml);

		//				if (ptFrom0l == null)
		//					ptFrom0l = BasePoints[2];
		//				points.Add(ptFrom0l);

		//				if (ptFrom0r == null)
		//					ptFrom0r = BasePoints[3];
		//				points.Add(ptFrom0r);

		//				//_UI.DrawPointWithText(ptFrom0r, -1, "ptFromR0");
		//				//_UI.DrawPointWithText(ptFrom0l, -1, "ptFromL0");
		//				//Leg.ProcessMessages();
		//			}
		//			else
		//			{
		//				points.Add(ptFroml);
		//				points.Add(ptFromr);

		//				if (ptFrom0r == null)
		//					ptFrom0r = BasePoints[2];
		//				points.Add(ptFrom0r);

		//				if (ptFrom0l == null)
		//					ptFrom0l = BasePoints[3];
		//				points.Add(ptFrom0l);
		//			}
		//		}
		//		else
		//			points = BasePoints;
		//	}

		//	int n = points.Count;

		//	double dirTouch = 0, SpTurnAngle = 0;
		//	Point ptCnt = null;

		//	for (int i = 0; i < n; i++)
		//	{
		//		ptCnt = ARANFunctions.LocalToPrj(points[i], dirSpCenter, TurnR, 0);

		//		//_UI.DrawPointWithText(ptCnt, -1, "ptCnt[" + i + "]");
		//		//    Leg.ProcessMessages();

		//		int j = i + 1 < n ? i + 1 : 0;

		//		double dirRef = Math.Atan2(points[j].Y - points[i].Y, points[j].X - points[i].X);
		//		double dirCntToPtDst = Math.Atan2(EndFIX.PrjPt.Y - ptCnt.Y, EndFIX.PrjPt.X - ptCnt.X);
		//		double fTmp = ARANMath.Modulus((dirCntToPtDst - dirRef) * fTurnSide, ARANMath.C_2xPI);

		//		if (fTmp < Math.PI)
		//		{
		//			SpTurnAngle = ARANFunctions.SpiralTouchToPoint(ptCnt, TurnRCurr, coeff, dirCurr, _TurnDir, EndFIX.PrjPt);

		//			if (SpTurnAngle > 100)
		//				return 9999.0;

		//			Rv = TurnRCurr + coeff * SpTurnAngle;
		//			dirTouch = SpStartRadial - (SpTurnAngle + ARANMath.C_PI_2 - Math.Atan2(coeff, Rv)) * fTurnSide;

		//			fTmp = ARANMath.Modulus((dirTouch - dirRef) * fTurnSide, ARANMath.C_2xPI);

		//			if (fTmp < Math.PI)
		//				break;
		//		}

		//		SpTurnAngle = ARANFunctions.SpiralTouchAngle(TurnRCurr, coeff, SpStartRadial, dirCurr, dirRef, _TurnDir);

		//		if (SpTurnAngle <= Math.PI)
		//		{
		//			TurnRCurr += SpTurnAngle * coeff;
		//			SpStartRadial -= SpTurnAngle * fTurnSide;
		//		}

		//		dirCurr = dirRef;
		//	}

		//	ptOut = ARANFunctions.LocalToPrj(ptCnt, SpStartRadial - SpTurnAngle * fTurnSide, Rv, 0);

		//	double result = ARANMath.Modulus(dirTouch, ARANMath.C_2xPI);

		//	if (_TurnDir == TurnDirection.CW)
		//		_OutDir_L = result;
		//	else
		//		_OutDir_R = result;

		//	ptOut.M = result;

		//	return result;
		//}

		public double CalcDFOuterDirection(WayPoint EndFIX, out Point ptOut, MultiPolygon PrevPrimaryArea = null)
		{
			_OutDir_L = _OutDir_R = _outDir;
			ReCreateArea();

			ptOut = null;

			double fTurnSide = -(int)_TurnDir;

			double WSpeed = _constants.Pansops[ePANSOPSData.dpWind_Speed].Value;
			double Rv = (1.76527777777777777777 / Math.PI) * Math.Tan(_BankAngle) / _NomLineTAS;
			if (Rv > 0.003) Rv = 0.003;

			double TurnR;
			if (Rv > 0.0) TurnR = _ConstructTAS / ((5.555555555555555555555 * Math.PI) * Rv);//_NomLineTAS
			else TurnR = -1;

			double coeff = WSpeed / ARANMath.DegToRad(1000.0 * Rv);
			double dirSpCenter = _EntryDir - ARANMath.C_PI_2 * fTurnSide;
			double SpStartRadial = _EntryDir + ARANMath.C_PI_2 * fTurnSide;

			double TurnRCurr = TurnR;
			double dirCurr = _EntryDir + Math.Atan2(coeff, TurnRCurr) * fTurnSide;

			IList<Point> points;
			if (this._FlyMode == eFlyMode.Atheight && PrevPrimaryArea != null)
			{
				//Ring PrevRing = PrevLeg.PrimaryArea[0].ExteriorRing;
				//Polygon pPoly = new Polygon();
				//pPoly.ExteriorRing = PrevRing;

				points = ARANFunctions.GetBasePoints(_PrjPt, _EntryDir, PrevPrimaryArea[0], _TurnDir);
				//for (int i = 0; i < points.Count; i++)
				//{
				//    _UI.DrawPointWithText(points[i], -1, "points[" + i + "]");
				//    Leg.ProcessMessages();
				//}
			}
			else
			{
				if (PrevPrimaryArea != null)
				{
					Point ptLPTr = ARANFunctions.LocalToPrj(PrjPt, EntryDirection_R, LPT_R, 0);
					Point ptLPTl = ARANFunctions.LocalToPrj(PrjPt, EntryDirection_L, LPT_L, 0);

					Point ptEPTr = ARANFunctions.LocalToPrj(PrjPt, EntryDirection_R, -EPT_R, 0);
					Point ptEPTl = ARANFunctions.LocalToPrj(PrjPt, EntryDirection_L, -EPT_L, 0);

					double fTmp, ASW_OUT0Fr = ASW_2_R, ASW_OUT0Fl = ASW_2_L;

					Point ptTmp = ARANFunctions.LocalToPrj(ptLPTr, EntryDirection_R, -0.001);
					Point ptFromr = ARANFunctions.RingVectorIntersect(PrevPrimaryArea[0].ExteriorRing, ptTmp, EntryDirection_R - 0.5 * ARANMath.C_PI, out fTmp);
					if (ptFromr != null)
						ASW_OUT0Fr = fTmp;
					else
						ptFromr = ARANFunctions.LocalToPrj(ptLPTr, EntryDirection_R, 0.0, -ASW_OUT0Fr);

					ptTmp = ARANFunctions.LocalToPrj(ptLPTl, EntryDirection_L, -0.001);
					Point ptFroml = ARANFunctions.RingVectorIntersect(PrevPrimaryArea[0].ExteriorRing, ptTmp, EntryDirection_L + 0.5 * ARANMath.C_PI, out fTmp);
					if (ptFroml != null)
						ASW_OUT0Fl = fTmp;
					else
						ptFroml = ARANFunctions.LocalToPrj(ptLPTl, EntryDirection_L, 0.0, ASW_OUT0Fl);

					//_UI.DrawPolygon(PrevPrimaryArea[0], -1, eFillStyle.sfsDiagonalCross);
					//_UI.DrawPointWithText(ptFromr, -1, "ptFromR");
					//_UI.DrawPointWithText(ptFroml, -1, "ptFromL");
					//LegBase.ProcessMessages();

					Point ptFrom0r = ARANFunctions.RingVectorIntersect(PrevPrimaryArea[0].ExteriorRing, ptEPTr, EntryDirection_R - 0.5 * ARANMath.C_PI, out fTmp);
					Point ptFrom0l = ARANFunctions.RingVectorIntersect(PrevPrimaryArea[0].ExteriorRing, ptEPTl, EntryDirection_L + 0.5 * ARANMath.C_PI, out fTmp);

					//_UI.DrawPointWithText(ptFrom0r, -1, "ptFromR0");
					//_UI.DrawPointWithText(ptFrom0l, -1, "ptFromL0");
					//LegBase.ProcessMessages();

					//_UI.DrawPointWithText(ptLPTr, -1, "ptLPTr-1");
					//_UI.DrawPointWithText(ptLPTl, -1, "ptLPTl-1");

					//_UI.DrawPointWithText(ptEPTr, -1, "ptEPTr-1");
					//_UI.DrawPointWithText(ptEPTl, -1, "ptEPTl-1");
					//LegBase.ProcessMessages();

					points = new List<Point>();
					if (_TurnDir == TurnDirection.CCW)
					{
						points.Add(ptFromr);
						points.Add(ptFroml);

						if (ptFrom0l == null)
							ptFrom0l = BasePoints[2];
						points.Add(ptFrom0l);

						if (ptFrom0r == null)
							ptFrom0r = BasePoints[3];
						points.Add(ptFrom0r);

						//_UI.DrawPointWithText(ptFrom0r, -1, "ptFromR0");
						//_UI.DrawPointWithText(ptFrom0l, -1, "ptFromL0");
						//Leg.ProcessMessages();
					}
					else
					{
						points.Add(ptFroml);
						points.Add(ptFromr);

						if (ptFrom0r == null)
							ptFrom0r = BasePoints[2];
						points.Add(ptFrom0r);

						if (ptFrom0l == null)
							ptFrom0l = BasePoints[3];
						points.Add(ptFrom0l);
					}
				}
				else
					points = BasePoints;
			}

			int n = points.Count;

			double dirTouch = 0, SpTurnAngle = 0;
			Point ptCnt = null;

			for (int i = 0; i < n; i++)
			{
				ptCnt = ARANFunctions.LocalToPrj(points[i], dirSpCenter, TurnR, 0);

				//_UI.DrawPointWithText(points[i], -1, "points[" + i + "]");
				//_UI.DrawPointWithText(ptCnt, -1, "ptCnt[" + i + "]");
				//LegBase.ProcessMessages();

				int j = i + 1 < n ? i + 1 : 0;

				double dirRef = Math.Atan2(points[j].Y - points[i].Y, points[j].X - points[i].X);
				double dirCntToPtDst = Math.Atan2(EndFIX.PrjPt.Y - ptCnt.Y, EndFIX.PrjPt.X - ptCnt.X);
				double fTmp = ARANMath.Modulus((dirCntToPtDst - dirRef) * fTurnSide, ARANMath.C_2xPI);

				if (fTmp < Math.PI)
				{
					SpTurnAngle = ARANFunctions.SpiralTouchToPoint(ptCnt, TurnRCurr, coeff, dirCurr, _TurnDir, EndFIX.PrjPt);

					//double 
					//SpStartRadial = ARANMath.Modulus(_EntryDir + (ARANMath.C_PI_2 - Math.Atan2(coeff, TurnRCurr)) * fTurnSide, ARANMath.C_2xPI);
					//SpStartRadial = _EntryDir + ARANMath.C_PI_2 * fTurnSide;
					//MultiPoint pts = ARANFunctions.CreateWindSpiral(ptCnt, dirCurr, dirCurr, dirCurr + Math.PI, TurnRCurr, coeff,  (SideDirection)_TurnDir);

					//pts = ARANFunctions.CreateWindSpiral(ptCnt, dirCurr, dirCurr, dirCurr + Math.PI, TurnRCurr, coeff,  (SideDirection)_TurnDir);
					//LineString ls = new LineString();

					//ls.AddMultiPoint(pts);
					//_UI.DrawLineString(ls, -1, 1);
					//LegBase.ProcessMessages();

					if (SpTurnAngle > 100)
						return 9999.0;

					Rv = TurnRCurr + coeff * SpTurnAngle;
					dirTouch = SpStartRadial - (SpTurnAngle + ARANMath.C_PI_2 - Math.Atan2(coeff, Rv)) * fTurnSide;

					fTmp = ARANMath.Modulus((dirTouch - dirRef) * fTurnSide, ARANMath.C_2xPI);

					if (fTmp < Math.PI)
						break;
				}

				SpTurnAngle = ARANFunctions.SpiralTouchAngle(TurnRCurr, coeff, SpStartRadial, dirCurr, dirRef, _TurnDir);

				if (SpTurnAngle <= Math.PI)
				{
					TurnRCurr += SpTurnAngle * coeff;
					SpStartRadial -= SpTurnAngle * fTurnSide;
				}

				dirCurr = dirRef;
			}


			ptOut = ARANFunctions.LocalToPrj(ptCnt, SpStartRadial - SpTurnAngle * fTurnSide, Rv, 0);

			//_UI.DrawPointWithText(ptCnt, -1, "ptCnt-1");
			//_UI.DrawPointWithText(ptOut, -1, "ptOut");
			//LegBase.ProcessMessages();

			double result = ARANMath.Modulus(dirTouch, ARANMath.C_2xPI);

			if (_TurnDir == TurnDirection.CW)
				_OutDir_L = result;
			else
				_OutDir_R = result;

			ptOut.M = result;

			return result;
		}

		public double CalcDFInnerDirection(WayPoint EndFIX, out Point ptOutBase, LegBase PrevLeg = null)
		{
			double dirDst15 = 0, fTurnSide = -(int)_TurnDir,
				SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;

			IList<Point> points;
			if (this._FlyMode == eFlyMode.Atheight && PrevLeg != null)
			{
				//Ring PrevRing = PrevLeg.PrimaryArea[0].ExteriorRing;
				//Polygon pPoly = new Polygon();
				//pPoly.ExteriorRing = PrevRing;

				points = ARANFunctions.GetBasePoints(_PrjPt, _EntryDir, PrevLeg.PrimaryArea[0], _TurnDir);

				//Ring PrevRing = PrevLeg.PrimaryArea[0].ExteriorRing;
				//points = new List<Point>();
				//for (int i = 0; i < PrevRing.Count; i++)
				//    points.Add(PrevRing[i]);
				//    _UI.DrawPointWithText(PrevRing[i], )
			}
			else
				points = BasePoints;

			int n = points.Count;
			ptOutBase = null;

			for (int i = 0; i < n; i++)
			{
				bool bFlag = false;
				ptOutBase = points[i];

				//_UI.DrawPointWithText(ptOutBase, -1, "ptOutBase-" + (i + 1));
				//LegBase.ProcessMessages();

				//dirDst15 = Math.Atan2(EndFIX.PrjPt.Y - ptOutBase.Y, EndFIX.PrjPt.X - ptOutBase.X);
				dirDst15 = ARANMath.Modulus(Math.Atan2(EndFIX.PrjPt.Y - ptOutBase.Y, EndFIX.PrjPt.X - ptOutBase.X), ARANMath.C_2xPI);


				for (int j = 0; j < n; j++)
				{
					if (j != i)
					{
						Point ptTmp = ARANFunctions.PrjToLocal(ptOutBase, dirDst15, points[j]);
						if (ptTmp.Y * fTurnSide < 0)
						{
							bFlag = true;
							break;
						}
					}
				}
				if (!bFlag) break;
			}

			if (_TurnDir == TurnDirection.CW)
				_OutDir_R = dirDst15;
			else
				_OutDir_L = dirDst15;

			ptOutBase.M = dirDst15;
			return dirDst15;
		}

		public double CalcDFInnerDirection(WayPoint EndFIX, out Point ptOutBase, LegApch PrevLeg = null)
		{
			double dirDst15 = 0, fTurnSide = -(int)_TurnDir,
				SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;

			IList<Point> points;
			if (this._FlyMode == eFlyMode.Atheight && PrevLeg != null)
			{
				//Ring PrevRing = PrevLeg.PrimaryArea[0].ExteriorRing;
				//Polygon pPoly = new Polygon();
				//pPoly.ExteriorRing = PrevRing;

				points = ARANFunctions.GetBasePoints(_PrjPt, _EntryDir, PrevLeg.PrimaryArea[0], _TurnDir);

				//Ring PrevRing = PrevLeg.PrimaryArea[0].ExteriorRing;
				//points = new List<Point>();
				//for (int i = 0; i < PrevRing.Count; i++)
				//    points.Add(PrevRing[i]);
				//    _UI.DrawPointWithText(PrevRing[i], )
			}
			else
				points = BasePoints;

			int n = points.Count;
			ptOutBase = null;

			for (int i = 0; i < n; i++)
			{
				bool bFlag = false;
				ptOutBase = points[i];

				dirDst15 = Math.Atan2(EndFIX.PrjPt.Y - ptOutBase.Y, EndFIX.PrjPt.X - ptOutBase.X);

				for (int j = 0; j < n; j++)
				{
					if (j != i)
					{
						Point ptTmp = ARANFunctions.PrjToLocal(ptOutBase, dirDst15, points[j]);
						if (ptTmp.Y * fTurnSide < 0)
						{
							bFlag = true;
							break;
						}
					}
				}
				if (!bFlag) break;
			}

			if (_TurnDir == TurnDirection.CW)
				_OutDir_R = dirDst15;
			else
				_OutDir_L = dirDst15;

			ptOutBase.M = dirDst15;
			return dirDst15;
		}

		public void SetSemiWidth(double newVal)
		{
			_SemiWidth = newVal;
		}

		public int ToleranceElement { get { return _ToleranceElement; } }
		public int PointElement { get { return _PointElement; } }

		public double PilotTime { get { return _PilotTime; } }
		public double BankTime { get { return _BankTime; } }

		public double XTT { get { return _XTT; } }
		public double FTE { get { return _FTE; } }
		public double ATT { get { return _ATT; } }
		public double EPT { get { return _EPT; } }
		public double EPT_L { get { return _EPT_L; } }
		public double EPT_R { get { return _EPT_R; } }

		public double LPT { get { return _LPT; } }
		public double LPT_L { get { return _LPT_L; } }
		public double LPT_R { get { return _LPT_R; } }
		public double SemiWidth { get { return _SemiWidth; } }
		public double ASW_L { get { return _ASW_L; } set { _ASW_L = value; } }
		public double ASW_2_L { get { return _ASW_2_L; } set { _ASW_2_L = value; _BasePoints.Clear(); } }
		public double ASW_R { get { return _ASW_R; } set { _ASW_R = value; } }
		public double ASW_2_R { get { return _ASW_2_R; } set { _ASW_2_R = value; _BasePoints.Clear(); } }
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public class HeightPoint : WayPoint
	{
		public HeightPoint(IAranEnvironment aranEnv)
			: base(aranEnv)
		{
		}

		public HeightPoint(eFIXRole InitialRole, IAranEnvironment aranEnv)
			: base(InitialRole, aranEnv)
		{
		}

		public HeightPoint(eFIXRole InitialRole, WPT_FIXType fix, IAranEnvironment aranEnv)
			: base(InitialRole, fix, aranEnv)
		{
		}

		/*
		protected override void CalcWPTParams()
		{
			_FTE = ARANMath.Epsilon;// 0.0;
			_XXT = ARANMath.Epsilon;// 0.0;
			_ATT = ARANMath.Epsilon;// 0.0;
			_SemiWidth = ARANMath.Epsilon;// 0.0;

			_ASW_R = ARANMath.Epsilon;// 0.0;
			_ASW_L = ARANMath.Epsilon;// 0.0;

			_ASW_2_R = ARANMath.Epsilon;// 0.0;
			_ASW_2_L = ARANMath.Epsilon;// 0.0;
		}*/

		public override AranObject Clone()
		{
			HeightPoint clone = new HeightPoint(_aranEnv);
			AssignTo(clone);
			return clone;
		}

		public override void RefreshGraphics()
		{
			String Text;

			_UI.SafeDeleteGraphic(_PointElement);

			if (!_DrawingEnabled)
				return;

			Text = SensorConstant.FIXRoleStyleNames[(int)_FIXRole];
			if (_Name != "")
				Text = Text + " / " + _Name;

			_PointElement = _UI.DrawPointWithText(_PrjPt, Text, _PointSymbol);
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public class FIX : WayPoint
	{
		public FIX(IAranEnvironment aranEnv)
			: base(aranEnv)
		{
		}

		public FIX(eFIXRole InitialRole, IAranEnvironment aranEnv)
			: base(InitialRole, aranEnv)
		{
		}

		public FIX(eFIXRole InitialRole, WPT_FIXType fix, IAranEnvironment aranEnv)
			: base(InitialRole, fix, aranEnv)
		{
		}

		protected override void CreateTolerArea()
		{
			double dir = _EntryDir;

			Ring ring = new Ring();

			Point point = ARANFunctions.LocalToPrj(_PrjPt, dir, -_ATT, -_XTT);
			ring.Add(point);

			point = ARANFunctions.LocalToPrj(_PrjPt, dir, -_ATT, _XTT);
			ring.Add(point);

			point = ARANFunctions.LocalToPrj(_PrjPt, dir, _ATT, _XTT);
			ring.Add(point);

			point = ARANFunctions.LocalToPrj(_PrjPt, dir, _ATT, -_XTT);
			ring.Add(point);

			Polygon polygon = new Polygon();
			polygon.ExteriorRing = ring;
			_TolerArea.Clear();
			_TolerArea.Add(polygon);
		}

		public override AranObject Clone()
		{
			FIX clone = new FIX(_aranEnv);
			AssignTo(clone);
			return clone;
		}

		//public static implicit operator  WayPoint (FIX f)
		//{
		//    return (f as WayPoint);
		//}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public class MAPt : FIX
	{
		double _SOCDistance;
		FillSymbol _SOCAreaSymbol;

		Point _SOCPrjPt;
		Point _SOCGeoPt;

		Polygon _SOCArea;
		MultiPolygon _StMAArea;

		int _SOCAreaElement, _SOCElement;

		override public Point PrjPt
		{
			get { return _PrjPt; }

			set
			{
				_PrjPt.Assign(value);
				if (!_PrjPt.IsEmpty)
				{
					_GeoPt.Assign(_pspatialReferenceOperation.ToGeo<Point>(_PrjPt));
					_SOCPrjPt = ARANFunctions.LocalToPrj(_PrjPt, _outDir, _SOCDistance, 0.0);
					_SOCGeoPt.Assign(_pspatialReferenceOperation.ToGeo<Point>(_SOCPrjPt));
					ReCreateArea();
				}
				else
				{
					_GeoPt.Assign(value);
					_SOCPrjPt.Assign(value);
					_SOCGeoPt.Assign(value);
				}
			}
		}

		protected override void CreateTolerArea()
		{
			base.CreateTolerArea();
			GeometryOperators geomOperators = new GeometryOperators();

			//if (_StMAArea.IsEmpty || _StMAArea.Area< ARANMath.Epsilon)
			if (geomOperators.Disjoint(_StMAArea, PrjPt))
			{
				Ring ring = new Ring();

				Point tmpPt = ARANFunctions.LocalToPrj(_PrjPt, _outDir, -_ATT, -_ASW_2_R);
				ring.Add(tmpPt);

				tmpPt = ARANFunctions.LocalToPrj(_PrjPt, _outDir, -_ATT, _ASW_2_L);
				ring.Add(tmpPt);

				double Tan15 = Math.Tan(_constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value);
				double Tan15_2 = _ASW_2_L / _ASW_L * Tan15;
				double Width = _XTT + (_SOCDistance + _ATT) * Tan15_2;

				tmpPt = ARANFunctions.LocalToPrj(_PrjPt, _outDir, _SOCDistance, Width);
				ring.Add(tmpPt);

				Tan15_2 = _ASW_2_R / _ASW_R * Tan15;
				Width = _XTT + (_SOCDistance + _ATT) * Tan15_2;

				tmpPt = ARANFunctions.LocalToPrj(_PrjPt, _outDir, _SOCDistance, -Width);
				ring.Add(tmpPt);

				_SOCArea.Clear();
				_SOCArea.ExteriorRing = ring;
			}
			else
			{
				LineString CutterPline = new LineString();

				Point tmpPt = ARANFunctions.LocalToPrj(PrjPt, EntryDirection, _SOCDistance, 100000.0);
				CutterPline.Add(tmpPt);
				tmpPt = ARANFunctions.LocalToPrj(PrjPt, EntryDirection, _SOCDistance, -100000.0);
				CutterPline.Add(tmpPt);

				Geometry lefGeom, SOCGeom;

				//_UI.DrawMultiPolygon(_StMAArea, eFillStyle.sfsCross);
				//_UI.DrawLineString(CutterPline, 2);
				//_UI.DrawPointWithText(PrjPt, "PrjPt");
				//LegBase.ProcessMessages(true);

				try
				{
					geomOperators.Cut(_StMAArea, CutterPline, out lefGeom, out SOCGeom);
					_SOCArea.Assign(((MultiPolygon)SOCGeom)[0]);
				}
				catch
				{
					_SOCArea.Assign((_StMAArea)[0]);
				}
			}

			//_UI.DrawPolygon(_SOCArea, eFillStyle.sfsCross);
			//LegBase.ProcessMessages();
		}

		MAPt(IAranEnvironment aranEnv)
			: base(aranEnv)
		{
			_SOCDistance = 0.0;
			_SOCAreaSymbol = new FillSymbol();

			_SOCPrjPt = new Point();
			_SOCGeoPt = new Point();

			_SOCArea = new Polygon();
			_StMAArea = new MultiPolygon();

			_SOCAreaElement = -1;
			_SOCElement = -1;
		}

		public MAPt(eFIXRole InitialRole, IAranEnvironment aranEnv)
			: base(InitialRole, aranEnv)
		{
			_SOCDistance = 0.0;
			_SOCAreaSymbol = new FillSymbol();

			_SOCPrjPt = new Point();
			_SOCGeoPt = new Point();

			_SOCArea = new Polygon();
			_StMAArea = new MultiPolygon();

			_SOCAreaElement = -1;
			_SOCElement = -1;
		}

		public MAPt(eFIXRole InitialRole, WPT_FIXType fix, IAranEnvironment aranEnv)
			: base(InitialRole, fix, aranEnv)
		{
			_SOCDistance = 0.0;
			_SOCAreaSymbol = new FillSymbol();

			_SOCPrjPt = new Point();
			_SOCGeoPt = new Point();

			_SOCArea = new Polygon();
			_StMAArea = new MultiPolygon();

			_SOCAreaElement = -1;
			_SOCElement = -1;
		}

		public override void Assign(AranObject source)
		{
			base.Assign(source);
			MAPt AnOther = (MAPt)source;

			DeleteGraphics();

			//_SOCAreaElement = -1;
			//_SOCElement = -1;
			_SOCDistance = AnOther._SOCDistance;

			_SOCAreaSymbol.Assign(AnOther._SOCAreaSymbol);
			_SOCPrjPt.Assign(AnOther._SOCPrjPt);
			_SOCGeoPt.Assign(AnOther._SOCGeoPt);
			_StMAArea.Assign(AnOther._StMAArea);
			_SOCArea.Assign(AnOther._SOCArea);
		}

		public override AranObject Clone()
		{
			MAPt clone = new MAPt(_aranEnv);
			AssignTo(clone);
			return clone;
		}

		//public override void AssignTo(IAranCloneable Dest)
		//{
		//    base.AssignTo(Dest);
		//    MAPt AnOther = (MAPt)Dest;

		//    AnOther._SOCAreaElement = -1;
		//    AnOther._SOCElement = -1;
		//    AnOther._SOCDistance = _SOCDistance;

		//    AnOther._SOCAreaSymbol.Assign(_SOCAreaSymbol);
		//    AnOther._SOCPrjPt.Assign(_SOCPrjPt);
		//    AnOther._SOCGeoPt.Assign(_SOCGeoPt);
		//    AnOther._StMAArea.Assign(_StMAArea);
		//    AnOther._SOCArea.Assign(_SOCArea);
		//}

		public override void DeleteGraphics()
		{
			base.DeleteGraphics();
			//_UI.SafeDeleteGraphic(_SOCAreaElement);

			//LegBase.ProcessMessages();
			_UI.SafeDeleteGraphic(_SOCElement);
			//LegBase.ProcessMessages();

			//_SOCAreaElement = -1;
			_SOCElement = -1;
			//LegBase.ProcessMessages();
		}

		public override void RefreshGraphics()
		{
			//DeleteGraphics();
			//LegBase.ProcessMessages();

			//_UI.SafeDeleteGraphic(_SOCAreaElement);
			//_UI.SafeDeleteGraphic(_SOCElement);

			base.RefreshGraphics();

			if (!_DrawingEnabled)
				return;

			//_SOCAreaElement = _UI.DrawPolygon(_SOCArea, _SOCAreaSymbol);
			//LegBase.ProcessMessages();
			if (_FIXRole == eFIXRole.MAPt_)
				_SOCElement = _UI.DrawPointWithText(_SOCPrjPt, "SOC", _PointSymbol);
			//LegBase.ProcessMessages();
		}

		public double SOCDistance
		{
			get { return _SOCDistance; }
			set
			{
				_SOCDistance = value;
				if (_PrjPt != null && !_PrjPt.IsEmpty)
				{
					_SOCPrjPt = ARANFunctions.LocalToPrj(_PrjPt, _outDir, _SOCDistance, 0.0);
					_SOCGeoPt.Assign(_pspatialReferenceOperation.ToGeo<Point>(_SOCPrjPt));
					ReCreateArea();
				}
			}
		}

		public Point SOCPrjPt
		{
			get { return _SOCPrjPt; }
		}

		public Point SOCGeoPt
		{
			get { return _SOCGeoPt; }
		}

		public MultiPolygon StraightArea
		{
			get { return _StMAArea; }
			set
			{
				_StMAArea.Assign(value);
				ReCreateArea();
			}
		}

		public Polygon SOCArea { get { return _SOCArea; } }
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public class MATF : FIX
	{
		double _TurnDistance;
		eFlyPath _FlyPath;
		eTurnAt _TurnAt;

		MultiPolygon _TolerSecArea,
				_StraightInPrimPolygon,
				_StraightInSecPolygon;

		MAPt _MAPt;
		Geometry _ReferenceGeometry;

		protected override void CreateTolerArea()
		{
			Ring ring;
			Point point, tmpPt;

			LineString rfp, CutterPline;
			double spanWidth, dir, lptF, Dist6sec, PilotTolerance, BankTolerance;

			if (_TurnAt == eTurnAt.TP)//_MAPt==null || _MAPt.PrjPt == null ||
			{
				spanWidth = 0.5 * SemiWidth;

				//if (FFIXRole < MAPt_ )	dir = FOutDir;
				//else
				dir = _EntryDir;

				ring = new Ring();
				rfp = new LineString();

				point = ARANFunctions.LocalToPrj(_PrjPt, dir, -_EPT, -spanWidth);
				ring.Add(point);

				point = ARANFunctions.LocalToPrj(_PrjPt, dir, -_EPT, spanWidth);
				ring.Add(point);

				// lptF = (2*(byte)(FFlyMode != eFlyMode.fmFlyBy) - 1) * FLPT;

				lptF = _FlyMode != eFlyMode.Flyby ? _LPT : -_LPT;

				point = ARANFunctions.LocalToPrj(_PrjPt, dir, lptF, spanWidth);
				ring.Add(point);

				point = ARANFunctions.LocalToPrj(_PrjPt, dir, lptF, -spanWidth);
				ring.Add(point);

				Polygon polygon = new Polygon();
				polygon.ExteriorRing = ring;

				_TolerArea.Clear();
				_TolerArea.Add(polygon);
				//==
				ring = new Ring();

				point = ARANFunctions.LocalToPrj(_PrjPt, dir, -_EPT, -SemiWidth);
				ring.Add(point);
				rfp.Add(point);

				point = ARANFunctions.LocalToPrj(_PrjPt, dir, -_EPT, SemiWidth);
				ring.Add(point);
				rfp.Add(point);

				point = ARANFunctions.LocalToPrj(_PrjPt, dir, lptF, SemiWidth);
				ring.Add(point);

				point = ARANFunctions.LocalToPrj(_PrjPt, dir, lptF, -SemiWidth);
				ring.Add(point);

				polygon = new Polygon();
				polygon.ExteriorRing = ring;

				//FTolerSecArea.Clear();
				_TolerSecArea.Add(polygon);

				_ReferenceGeometry = new LineString();
				_ReferenceGeometry.Assign(rfp);
				//==
			}
			else
			{
				CutterPline = new LineString();

				PilotTolerance = _constants.Pansops[ePANSOPSData.arMAPilotToleran].Value;
				BankTolerance = _constants.Pansops[ePANSOPSData.arMAPilotToleran].Value;

				Dist6sec = (_NomLineTAS + _constants.Pansops[ePANSOPSData.dpWind_Speed].Value) * (PilotTolerance + BankTolerance);

				tmpPt = ARANFunctions.LocalToPrj(_MAPt.PrjPt, _MAPt.OutDirection, _TurnDistance + Dist6sec, 100000.0);
				CutterPline.Add(tmpPt);

				tmpPt = ARANFunctions.LocalToPrj(_MAPt.PrjPt, _MAPt.OutDirection, _TurnDistance + Dist6sec, -100000.0);
				CutterPline.Add(tmpPt);

				GeometryOperators geomOperators = new GeometryOperators();

				Geometry lefGeom, TIAGeom;

				//_UI.DrawMultiPolygon(_StraightInPrimPolygon, eFillStyle.sfsBackwardDiagonal);
				//_UI.DrawLineString(CutterPline, 2);
				//LegBase.ProcessMessages(true);
				try
				{
					geomOperators.Cut(_StraightInPrimPolygon, CutterPline, out lefGeom, out TIAGeom);
				}
				catch
				{
					TIAGeom = (Geometry)_StraightInPrimPolygon.Clone();
				}

				_TolerArea.Assign(TIAGeom);

				try
				{
					geomOperators.Cut(_StraightInSecPolygon, CutterPline, out lefGeom, out TIAGeom);
				}
				catch
				{
					TIAGeom = (Geometry)_StraightInSecPolygon.Clone();
				}

				_TolerSecArea.Assign(TIAGeom);

				//====================================================================================================
				CutterPline.Clear();

				tmpPt = ARANFunctions.LocalToPrj(_MAPt.PrjPt, _MAPt.OutDirection, _TurnDistance, 100000.0);
				CutterPline.Add(tmpPt);

				tmpPt = ARANFunctions.LocalToPrj(_MAPt.PrjPt, _MAPt.OutDirection, _TurnDistance, -100000.0);
				CutterPline.Add(tmpPt);
				try
				{
					geomOperators.Cut(_StraightInPrimPolygon, CutterPline, out lefGeom, out TIAGeom);
				}
				catch
				{
					TIAGeom = (Geometry)_StraightInPrimPolygon.Clone();
				}

				_ReferenceGeometry = new MultiPolygon();
				_ReferenceGeometry.Assign(TIAGeom);
			}
		}

		public MATF(IAranEnvironment aranEnv)
			: base(eFIXRole.MATF_LE_56, aranEnv)
		{
			_TurnDistance = 0.0;
			_OutDir_L = _OutDir_R = _outDir;
			_FlyPath = eFlyPath.DirectToFIX;
			_TurnAt = (eTurnAt)(-1);
			_TolerSecArea = new MultiPolygon();
			_ReferenceGeometry = null;
		}

		public MATF(WPT_FIXType fix, IAranEnvironment aranEnv)
			: base(eFIXRole.MATF_LE_56, fix, aranEnv)
		{
			_TurnDistance = 0.0;
			_OutDir_L = _OutDir_R = _outDir;
			_FlyPath = eFlyPath.DirectToFIX;
			_TurnAt = (eTurnAt)(-1);
			_TolerSecArea = new MultiPolygon();
			_ReferenceGeometry = null;
		}

		public override void Assign(AranObject source)
		{
			base.Assign(source);
			MATF AnOther = (MATF)source;

			_TurnDistance = AnOther._TurnDistance;
			_FlyPath = AnOther._FlyPath;
			_TurnAt = AnOther._TurnAt;
			_TolerSecArea = AnOther._TolerSecArea;

			_StraightInPrimPolygon = AnOther._StraightInPrimPolygon;
			_StraightInSecPolygon = AnOther._StraightInSecPolygon;
			_MAPt = AnOther._MAPt;

			if (AnOther._ReferenceGeometry != null)
			{
				if (_ReferenceGeometry != null &&
					_ReferenceGeometry.Type != AnOther._ReferenceGeometry.Type)
					_ReferenceGeometry = null;

				if (_ReferenceGeometry == null)
					if (AnOther._ReferenceGeometry.Type == GeometryType.LineString)
						_ReferenceGeometry = new LineString();
					else if (AnOther._ReferenceGeometry.Type == GeometryType.MultiPolygon)
						_ReferenceGeometry = new MultiPolygon();
					else
						_ReferenceGeometry = new Polygon();

				_ReferenceGeometry.Assign(AnOther._ReferenceGeometry);
			}
			else
				_ReferenceGeometry = null;
		}

		public override AranObject Clone()
		{
			MATF clone = new MATF(_aranEnv);
			AssignTo(clone);
			return clone;
		}

		public override void RefreshGraphics()
		{
			String Text;

			_UI.SafeDeleteGraphic(_ToleranceElement);
			_UI.SafeDeleteGraphic(_PointElement);

			if (!_DrawingEnabled)
				return;

			_ToleranceElement = _UI.DrawMultiPolygon(_TolerArea, _ToleranceSymbol);
			if (_TurnAt == eTurnAt.TP)
			{
				Text = SensorConstant.FIXRoleStyleNames[(int)_FIXRole];
				if (_Name != "")
					Text = Text + " / " + _Name;

				_PointElement = _UI.DrawPointWithText(_PrjPt, Text, _PointSymbol);
			}
			else
				_PointElement = _UI.DrawPointWithText(_PrjPt, "", _PointSymbol);
		}

		public MAPt maPt
		{
			get { return _MAPt; }
			set { _MAPt = value; }
		}

		public MultiPolygon StraightInPrimaryPolygon
		{
			get { return _StraightInPrimPolygon; }
			set { _StraightInPrimPolygon = value; }
		}

		public MultiPolygon StraightInSecondaryPolygon
		{
			get { return _StraightInSecPolygon; }
			set { _StraightInSecPolygon = value; }
		}

		public double TurnDistance
		{
			get { return _TurnDistance; }
			set { _TurnDistance = value; }
		}

		public eTurnAt TurnAt
		{
			get { return _TurnAt; }
			set
			{
				if (value != _TurnAt)
				{
					_TurnAt = value;
					ReCreateArea();
				}
			}
		}

		public eFlyPath FlyPath
		{
			get { return _FlyPath; }
			set
			{
				if (value != _FlyPath)
				{
					_FlyPath = value;
					ReCreateArea();
				}
			}
		}

		public MultiPolygon SecondaryTolerArea
		{
			get { return _TolerSecArea; }
		}

		public Geometry ReferenceGeometry
		{
			get { return _ReferenceGeometry; }
		}
	}
}

