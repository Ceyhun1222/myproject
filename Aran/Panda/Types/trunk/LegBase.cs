using System;
using System.Collections.Generic;

using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.PANDA.Constants;

using Aran.AranEnvironment;
using Aran.AranEnvironment.Symbols;
using Aran.Aim.Enums;

namespace Aran.PANDA.Common
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public class LegBase
	{
		protected const double OverlapDist = 0.5;

		protected static Aran.PANDA.Constants.Constants _constants = null;
		protected WayPoint _StartFIX, _EndFIX;

		protected int _PrimaryAreaEl, _FullAreaEl;
		protected int _PrimaryAreaSMEl, _FullAreaSMEl, _NomTrackEl, _NomTrackEl1, _NomTrackEl2;
		// int _KKLineEl;
		// int _NNSecondLineEl;
		//FlightPhase _FlightPhase;

		protected double _MinLegLength;
		protected double _IAS, _Gradient;

		protected IAranGraphics _UI;
		protected IAranEnvironment _aranEnv;

		protected MultiPolygon _PrimaryArea, _FullArea, _PrimaryAreaS,
				_FullAreaS, _PrimaryAreaM, _FullAreaM;

		protected LineString _NNSecondLine, _NomTrack, _NomTrack1, _NomTrack2;
		protected Geometry _KKLine;

		protected MultiLineString _AssesmentAreaOutline;

		protected FillSymbol _SecondaryAreaSymbol, _PrimaryAreSymbol;
		protected LineSymbol _NNSecondSymbol, _NomTrackSymbol, _NomTrackSymbol12;

		protected FillSymbol _SecondaryAreaSymbolNA, _PrimaryAreSymbolNA;
		protected LineSymbol _NNSecondSymbolNA, _NomTrackSymbolNA, _NomTrackSymbol12NA;

		public bool transferedOver56;

		protected bool _visible;
		protected bool _active;
		protected CodeSegmentPath _pathAndTermination;

		public bool Visible
		{
			get { return _visible; }
			set
			{
				if (value != _visible)
				{
					_visible = value;
					_EndFIX.Visible = _visible;

					RefreshGraphics();
				}
			}
		}

		public bool Active
		{
			get { return _active; }
			set
			{
				if (value != _active)
				{
					_active = value;
					RefreshGraphics();
				}
			}
		}

		public CodeSegmentPath PathAndTermination
		{
			get { return _pathAndTermination; }
			set
			{
				if (value != _pathAndTermination)
				{
					_pathAndTermination = value;
					_EndFIX.IsDFTarget = value == CodeSegmentPath.DF;
					//RefreshGraphics();
				}
			}
		}

		public double KKLength
		{
			get;
			set;
		}

		public double Length
		{
			get
			{
				if ((_StartFIX.FlyMode == eFlyMode.Atheight || _EndFIX.IsDFTarget) && _NomTrack != null)
					return _NomTrack.Length;

				return ARANFunctions.ReturnDistanceInMeters(_StartFIX.PrjPt, _EndFIX.PrjPt);
			}
		}

		public double MinLegLength
		{
			get { return _MinLegLength; }
			set { _MinLegLength = value; }
		}

		public double IAS
		{
			get { return _IAS; }
			set
			{
				_IAS = value;
				_StartFIX.IAS = value;
				_EndFIX.IAS = value;
			}
		}

		public double Gradient
		{
			get { return _Gradient; }
			set { _Gradient = value; }
		}

		public MultiPolygon FullAssesmentArea
		{
			get { return _FullAreaS; }
			set { _FullAreaS.Assign(value); }
		}

		public MultiPolygon PrimaryAssesmentArea
		{
			get { return _PrimaryAreaS; }
			set { _PrimaryAreaS.Assign(value); }
		}

		public MultiPolygon FullTotalArea
		{
			get { return _FullAreaM; }
			set { _FullAreaM.Assign(value); }
		}

		public MultiPolygon PrimaryTotalArea
		{
			get { return _PrimaryAreaM; }
			set { _PrimaryAreaM.Assign(value); }
		}

		public MultiPolygon FullArea
		{
			get { return _FullArea; }
			set
			{
				_FullArea.Assign(value);
				_FullAreaM.Assign(value);
				_FullAreaS.Assign(value);
			}
		}

		public MultiPolygon PrimaryArea
		{
			get { return _PrimaryArea; }
			set
			{
				_PrimaryArea.Assign(value);
				_PrimaryAreaM.Assign(value);
				_PrimaryAreaS.Assign(value);
			}
		}

		//in seconds
		public double Duration
		{
			get;
			set;
		}

		public double Altitude
		{
			get;
			set;
		}

		public LineString NNSecondLine
		{
			get { return _NNSecondLine; }
			set { _NNSecondLine.Assign(value); }
		}

		public Geometry KKLine
		{
			get { return _KKLine; }
			set { _KKLine = (Geometry)value.Clone(); }
		}

		public LineString NominalTrack
		{
			get { return _NomTrack; }
			set { _NomTrack.Assign(value); }
		}

		public MultiLineString AssesmentAreaOutline
		{
			get { return _AssesmentAreaOutline; }
		}

		/*	public FlightPhase flightPhase
		{
			get { return _FlightPhase; }
		}*/

		public WayPoint StartFIX { get { return _StartFIX; } }
		public WayPoint EndFIX { get { return _EndFIX; } }

		ObstacleContainer _DetObstacle_1;
		public ObstacleContainer DetObstacle_1
		{
			get { return _DetObstacle_1; }
			set { _DetObstacle_1 = value; }
		}

		ObstacleData _DetObstacle_2;
		public ObstacleData DetObstacle_2
		{
			get { return _DetObstacle_2; }
			set { _DetObstacle_2 = value; }
		}

		ObstacleContainer _Obstacles;
		public ObstacleContainer Obstacles
		{
			get { return _Obstacles; }
			set { _Obstacles = value; }
		}

#if DEBUG
		public static void ProcessMessages(bool continuous = false)
		{
			do
				System.Windows.Forms.Application.DoEvents();
			while (continuous);
		}
#endif

		public LegBase(WayPoint FIX0, WayPoint FIX1, IAranEnvironment aranEnv, LegBase PrevLeg = null)//FlightPhase _FlightPhase, 
		{
			if (_constants == null)
				_constants = new Aran.PANDA.Constants.Constants();

			_aranEnv = aranEnv;
			_UI = _aranEnv.Graphics;

			_StartFIX = FIX0;
			_EndFIX = FIX1;

			_PrimaryAreaEl = -1;
			_FullAreaEl = -1;

			_FullAreaSMEl = -1;
			_PrimaryAreaSMEl = -1;

			//FNNSecondLineEl = -1;
			_NomTrackEl = -1;
			_NomTrackEl1 = -1;
			_NomTrackEl2 = -1;

			if (PrevLeg != null)
				transferedOver56 = PrevLeg.transferedOver56;
			else
				transferedOver56 = false;

			_FullAreaM = new MultiPolygon();
			_PrimaryAreaM = new MultiPolygon();

			_FullAreaS = new MultiPolygon();
			_PrimaryAreaS = new MultiPolygon();

			_FullArea = new MultiPolygon();
			_PrimaryArea = new MultiPolygon();

			_NNSecondLine = new LineString();
			//_KKLine = new LineString();
			_KKLine = null;

			//FFlightPhase = _FlightPhase;
			_active = false;
			_visible = true;

			_SecondaryAreaSymbol = new FillSymbol();
			_SecondaryAreaSymbol.Color = ARANFunctions.RGB(0, 0, 255);
			_SecondaryAreaSymbol.Style = eFillStyle.sfsNull;
			_SecondaryAreaSymbol.Outline.Color = 0;
			_SecondaryAreaSymbol.Outline.Style = eLineStyle.slsSolid;

			_PrimaryAreSymbol = new FillSymbol();
			_PrimaryAreSymbol.Color = ARANFunctions.RGB(0, 255, 0);
			_PrimaryAreSymbol.Style = eFillStyle.sfsNull;
			_PrimaryAreSymbol.Outline.Color = 255;
			_PrimaryAreSymbol.Outline.Style = eLineStyle.slsSolid;

			_NNSecondSymbol = new LineSymbol(eLineStyle.slsSolid, ARANFunctions.RGB(0, 255, 0), 2);
			_NomTrackSymbol = new LineSymbol(eLineStyle.slsSolid, 255, 2);
			_NomTrackSymbol12 = new LineSymbol(eLineStyle.slsSolid, ARANFunctions.RGB(0, 255, 0), 1);

			_SecondaryAreaSymbolNA = new FillSymbol();
			_SecondaryAreaSymbolNA.Color = ARANFunctions.RGB(187, 187, 255);
			_SecondaryAreaSymbolNA.Style = eFillStyle.sfsNull;
			_SecondaryAreaSymbolNA.Outline.Color = ARANFunctions.RGB(187, 187, 187);
			_SecondaryAreaSymbolNA.Outline.Style = eLineStyle.slsSolid;

			_PrimaryAreSymbolNA = new FillSymbol();
			_PrimaryAreSymbolNA.Color = ARANFunctions.RGB(187, 255, 187);
			_PrimaryAreSymbolNA.Style = eFillStyle.sfsNull;
			_PrimaryAreSymbolNA.Outline.Color = ARANFunctions.RGB(255, 187, 187);
			_PrimaryAreSymbolNA.Outline.Style = eLineStyle.slsSolid;

			_NNSecondSymbolNA = new LineSymbol(eLineStyle.slsSolid, ARANFunctions.RGB(187, 255, 0), 2);
			_NomTrackSymbolNA = new LineSymbol(eLineStyle.slsSolid, ARANFunctions.RGB(255, 187, 187), 2);
			_NomTrackSymbol12NA = new LineSymbol(eLineStyle.slsSolid, ARANFunctions.RGB(187, 255, 187), 1);

			CreateNomTrack(PrevLeg);
		}

		public void CreateKKLine(LegBase PrevLeg = null)
		{
			double EntryDir = _StartFIX.EntryDirection;
			double Dir0 = EntryDir + 1.5 * Math.PI;
			double Dir2 = EntryDir + 0.5 * Math.PI;

			Point PtTmp1 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir + ARANMath.C_PI, _StartFIX.EPT, 0);

			Ring FullRing = new Ring();
			if (_FullArea.Count > 0)
			{
				if (PrevLeg != null && PrevLeg._FullArea.Count > 0)
				{
					//GeometryOperators geomOperators = new GeometryOperators();
					//MultiPolygon FullPoly = (MultiPolygon)geomOperators.UnionGeometry(PrevLeg._FullArea, _FullArea)
					//FullRing.Assign(FullPoly[0].ExteriorRing);

					FullRing.Assign(PrevLeg._FullArea[0].ExteriorRing);
				}
				else
					FullRing.Assign(_FullArea[0].ExteriorRing);
			}

			double fTmp;
			Point PtTmp0 = ARANFunctions.RingVectorIntersect(FullRing, PtTmp1, Dir0, out fTmp, true);
			if (PtTmp0 == null)
				PtTmp0 = ARANFunctions.LocalToPrj(PtTmp1, Dir0, _EndFIX.ASW_L, 0);

			Point PtTmp2 = ARANFunctions.RingVectorIntersect(FullRing, PtTmp1, Dir2, out fTmp, true);
			if (PtTmp2 == null)
				PtTmp2 = ARANFunctions.LocalToPrj(PtTmp1, Dir2, _EndFIX.ASW_R, 0);

			LineString kkLS = new LineString();

			kkLS.Add(PtTmp0);
			kkLS.Add(PtTmp1);
			kkLS.Add(PtTmp2);

			//_UI.DrawRing(FullRing, eFillStyle.sfsCross, -1);
			//_UI.DrawLineString(kkLS, 2, -1);
			//ProcessMessages();

			_KKLine = kkLS;
		}

		void CreateNNSecondLine(LegBase PrevLeg)
		{
			double TurnAng, theta, d, fSide, LineLen, Dir0, Dir2, EntryDir;
			Point PtTmp0, PtTmp1, PtTmp2;

			EntryDir = _StartFIX.EntryDirection;
			TurnAng = _StartFIX.TurnAngle;

			TurnDirection TurnDir = _StartFIX.TurnDirection;
			fSide = (int)TurnDir;

			LineLen = 10.0 * _EndFIX.ASW_R + _EndFIX.ASW_L;

			if (TurnDir == TurnDirection.NONE)
			{
				PtTmp1 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir + ARANMath.C_PI, _StartFIX.EPT, 0);
				Dir0 = EntryDir + 1.5 * Math.PI;
				Dir2 = EntryDir + 0.5 * Math.PI;
			}
			else
			{
				theta = 0.25 * TurnAng;
				d = _StartFIX.ATT * Math.Tan(theta);
				PtTmp1 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir + ARANMath.C_PI, _StartFIX.ATT, -d * fSide);

				if (TurnDir == TurnDirection.CCW)
				{
					Dir0 = EntryDir + 1.5 * Math.PI;
					Dir2 = EntryDir + 0.5 * (Math.PI + TurnAng);
				}
				else
				{
					Dir0 = EntryDir - 0.5 * (Math.PI + TurnAng);
					Dir2 = EntryDir + 0.5 * Math.PI;
				}
			}

			Ring FullRing = new Ring();

			if (PrevLeg != null)
			{
				GeometryOperators geomOperators = new GeometryOperators();
				MultiPolygon FullPoly = (MultiPolygon)geomOperators.UnionGeometry(PrevLeg._FullArea, _FullArea);

				FullRing.Assign(FullPoly[0].ExteriorRing);
			}
			else
				FullRing.Assign(_FullArea[0].ExteriorRing);


			//LegBase.ProcessMessages(true);
			//_UI.DrawRing(FullRing, eFillStyle.sfsCross);
			//_UI.DrawMultiPolygon(PrevLeg._FullArea, eFillStyle.sfsDiagonalCross);
			//_UI.DrawMultiPolygon(_FullArea, eFillStyle.sfsDiagonalCross);
			//LegBase.ProcessMessages();

			PtTmp0 = ARANFunctions.RingVectorIntersect(FullRing, PtTmp1, Dir0, out d, true);
			if (PtTmp0 == null)
				PtTmp0 = ARANFunctions.LocalToPrj(PtTmp1, Dir0, LineLen, 0);

			PtTmp2 = ARANFunctions.RingVectorIntersect(FullRing, PtTmp1, Dir2, out d, true);
			if (PtTmp2 == null)
				PtTmp2 = ARANFunctions.LocalToPrj(PtTmp1, Dir2, LineLen, 0);

			_NNSecondLine.Clear();

			_NNSecondLine.Add(PtTmp0);
			_NNSecondLine.Add(PtTmp1);
			_NNSecondLine.Add(PtTmp2);
		}

		public void DeleteGraphics()
		{
			_UI.SafeDeleteGraphic(_PrimaryAreaEl);
			_UI.SafeDeleteGraphic(_FullAreaEl);

			_UI.SafeDeleteGraphic(_FullAreaSMEl);
			_UI.SafeDeleteGraphic(_PrimaryAreaSMEl);

			//  _UI.SafeDeleteGraphic(_NNSecondLineEl);
			//	_UI.SafeDeleteGraphic(_KKLineEl)

			_UI.SafeDeleteGraphic(_NomTrackEl);

			_UI.SafeDeleteGraphic(_NomTrackEl1);
			_UI.SafeDeleteGraphic(_NomTrackEl2);

			_StartFIX.DeleteGraphics();
			_EndFIX.DeleteGraphics();
		}

		public void RefreshGraphics()
		{
			//_UI.SafeDeleteGraphic(_PrimaryAreaEl);
			//_UI.SafeDeleteGraphic(_FullAreaEl);

			_UI.SafeDeleteGraphic(_FullAreaSMEl);
			_UI.SafeDeleteGraphic(_PrimaryAreaSMEl);
			//	_UI.SafeDeleteGraphic(_NNSecondLineEl);
			//	_UI.SafeDeleteGraphic(_KKLineEl);
			_UI.SafeDeleteGraphic(_NomTrackEl);

			_UI.SafeDeleteGraphic(_NomTrackEl1);
			_UI.SafeDeleteGraphic(_NomTrackEl2);

			//_PrimaryAreaEl = _UI.DrawMultiPolygon(_FullArea, ARANFunctions.RGB(0, 255, 0), eFillStyle.sfsBackwardDiagonal);
			//_FullAreaEl = _UI.DrawMultiPolygon(_PrimaryArea, ARANFunctions.RGB(0, 255, 0), eFillStyle.sfsForwardDiagonal);

			if (!_visible)
				return;

			//if (_StartFIX.PrjPt.IsEmpty || _EndFIX.PrjPt.IsEmpty)
			//	return;

			if (_active)
			{
				_FullAreaSMEl = _UI.DrawMultiPolygon(_FullAreaS, _SecondaryAreaSymbol);
				_PrimaryAreaSMEl = _UI.DrawMultiPolygon(_PrimaryAreaS, _PrimaryAreSymbol);

				//	_NNSecondLineEl = _UI.DrawPolyline(_NNSecondLine, _NNSecondSymbol);
				// _KKLineEl = _UI.DrawPolyline(_KKLine, _KKLineSymbol);

				_NomTrackEl = _UI.DrawLineString(_NomTrack, _NomTrackSymbol);

				if (_EndFIX.IsDFTarget && _NomTrack1 != null && _NomTrack2 != null)
				{
					_NomTrackEl1 = _UI.DrawLineString(_NomTrack1, _NomTrackSymbol12);
					_NomTrackEl2 = _UI.DrawLineString(_NomTrack2, _NomTrackSymbol12);
				}
			}
			else
			{
				_FullAreaSMEl = _UI.DrawMultiPolygon(_FullAreaS, _SecondaryAreaSymbolNA);
				_PrimaryAreaSMEl = _UI.DrawMultiPolygon(_PrimaryAreaS, _PrimaryAreSymbolNA);

				//	_NNSecondLineEl = _UI.DrawPolyline(_NNSecondLine, _NNSecondSymbol);
				// _KKLineEl = _UI.DrawPolyline(_KKLine, _KKLineSymbol);

				_NomTrackEl = _UI.DrawLineString(_NomTrack, _NomTrackSymbolNA);

				if (_EndFIX.IsDFTarget && _NomTrack1 != null && _NomTrack2 != null)
				{
					_NomTrackEl1 = _UI.DrawLineString(_NomTrack1, _NomTrackSymbol12NA);
					_NomTrackEl2 = _UI.DrawLineString(_NomTrack2, _NomTrackSymbol12NA);
				}
			}

			_StartFIX.RefreshGraphics();
			_EndFIX.RefreshGraphics();
		}

		public double CalcDRNomDir()
		{
			double fTurnDirection = -(int)(_StartFIX.TurnDirection);
			double r = _StartFIX.ConstructTurnRadius;

			Point ptCenter = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection, 0, -r * fTurnDirection);

			double dx = _EndFIX.PrjPt.X - ptCenter.X;
			double dy = _EndFIX.PrjPt.Y - ptCenter.Y;

			double dirDest = Math.Atan2(dy, dx);
			double distDest = ARANMath.Hypot(dy, dx);

			double TurnAngle = (_StartFIX.EntryDirection - dirDest) * fTurnDirection + ARANMath.C_PI_2 - Math.Acos(r / distDest);
			return ARANMath.Modulus(_StartFIX.EntryDirection - TurnAngle * fTurnDirection, ARANMath.C_2xPI);
		}

		protected Point _ptInOutBase;
		protected Point _ptOutOutBase;

		virtual public void CreateNomTrack(LegBase PrevLeg = null, double minTurnAngle = 0.0)
		{
			Point ptTmp, PtTmp0, PtTmp1, PtTmp2, PtTmp3;

			double DivergenceAngle, R2;

			double DivergenceAngle30 = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;
			double Bank150 = _StartFIX.BankAngle;

			double EntryDir = _StartFIX.EntryDirection;
			double OutDir = _EndFIX.EntryDirection; //_StartFIX.OutDirection;	//

			Point ptStart;

			//if (_StartFIX is MATF)
			//{
			//	//ptStart = (Point)((MATF)_StartFIX).maPt.PrjPt.Clone();
			//	//EntryDir = ((MATF)_StartFIX).maPt.EntryDirection;
			//	ptStart = (Point)_StartFIX.PrjPt.Clone();
			//}
			//else
			if (_StartFIX.FlyMode == eFlyMode.Atheight)
				ptStart = (Point)_StartFIX.PrjPtH.Clone();
			else
				ptStart = (Point)_StartFIX.PrjPt.Clone();

			double fTurnDir, TurnAngle;

			if (EndFIX.IsDFTarget)
				fTurnDir = (int)_StartFIX.EffectiveTurnDirection;
			else
				fTurnDir = (int)ARANMath.SideFrom2Angle(OutDir, EntryDir);

			if (Math.Abs(OutDir - EntryDir) < ARANMath.EpsilonRadian)
				TurnAngle = 0.0;
			else
			{
				TurnAngle = ARANMath.Modulus((OutDir - EntryDir) * fTurnDir, ARANMath.C_2xPI);

				if (Math.Abs(TurnAngle - ARANMath.C_2xPI) < ARANMath.EpsilonRadian)
					TurnAngle = 0.0;
			}

			double R1 = _StartFIX.NomLineTurnRadius;
			double fTmp = ARANMath.Modulus(TurnAngle, ARANMath.C_PI);

			ptStart.M = EntryDir;

			//ProcessMessages(true);
			//_UI.DrawPointWithText(ptStart, "ptStart");
			//ProcessMessages();

			MultiPoint TrackPoints = new MultiPoint();
			MultiPoint TrackPoints1 = null, TrackPoints2 = null;

			if (Math.Abs(TurnAngle) < ARANMath.DegToRad(0.5))
			{
				if (_StartFIX is MATF)
				{
					ptTmp = (Point)((MATF)_StartFIX).maPt.PrjPt.Clone();
					ptTmp.M = _StartFIX.EntryDirection;
					TrackPoints.Add(ptTmp);
				}

				TrackPoints.Add(ptStart);

				ptTmp = (Point)_EndFIX.PrjPt.Clone();

				ptTmp.M = OutDir;
				TrackPoints.Add(ptTmp);

				_NomTrack1 = null;
				_NomTrack2 = null;
			}
			else if (_EndFIX.IsDFTarget)
			{
				double OuterDir, InnerDir;
				if (StartFIX.EffectiveTurnDirection == TurnDirection.CW)
				{
					_EndFIX.EntryDirection_L = StartFIX.CalcDFOuterDirection(_EndFIX, out _ptOutOutBase, PrevLeg.PrimaryArea);
					_EndFIX.EntryDirection_R = StartFIX.CalcDFInnerDirection(_EndFIX, out _ptInOutBase, PrevLeg);
					OuterDir = _EndFIX.EntryDirection_L;
					InnerDir = _EndFIX.EntryDirection_R;
				}
				else
				{
					_EndFIX.EntryDirection_R = StartFIX.CalcDFOuterDirection(_EndFIX, out _ptOutOutBase, PrevLeg.PrimaryArea);
					_EndFIX.EntryDirection_L = StartFIX.CalcDFInnerDirection(_EndFIX, out _ptInOutBase, PrevLeg);
					OuterDir = _EndFIX.EntryDirection_R;
					InnerDir = _EndFIX.EntryDirection_L;
				}

				if (_StartFIX is MATF)
				{
					MATF Matf = (MATF)_StartFIX;
					if (Matf.TurnAt != eTurnAt.TP)
					{
						ptTmp = (Point)Matf.maPt.PrjPt.Clone();
						ptTmp.M = _StartFIX.EntryDirection;
					}
					else
					{
						ptTmp = (Point)Matf.PrjPt.Clone();
						ptTmp.M = _StartFIX.EntryDirection;
					}

					TrackPoints.Add(ptTmp);                         //	Point1

					ptStart = (Point)_StartFIX.PrjPt.Clone();
					ptStart.M = EntryDir;
				}

				double NomDir = _EndFIX.EntryDirection;

				TrackPoints.Add(ptStart);

				Point ptCnt = ARANFunctions.LocalToPrj(ptStart, _StartFIX.EntryDirection, 0.0, R1 * fTurnDir);
				PtTmp1 = ARANFunctions.LocalToPrj(ptCnt, _EndFIX.EntryDirection, 0.0, -R1 * fTurnDir);

				PtTmp1.M = NomDir;
				TrackPoints.Add(PtTmp1);

				if (ARANFunctions.PointToLineDistance(PtTmp1, _EndFIX.PrjPt, NomDir + ARANMath.C_PI_2) > 0.0)
				{
					ptTmp = (Point)_EndFIX.PrjPt.Clone();
					ptTmp.M = NomDir;
					TrackPoints.Add(ptTmp);
				}

				//================================================================
				TrackPoints1 = new MultiPoint();
				if (_ptOutOutBase != null)
					TrackPoints1.Add(_ptOutOutBase);

				ptTmp = (Point)_EndFIX.PrjPt.Clone();

				ptTmp.M = OuterDir;
				TrackPoints1.Add(ptTmp);

				_NomTrack1 = ARANFunctions.ConvertPointsToTrackLIne(TrackPoints1);

				//================================================================
				TrackPoints2 = new MultiPoint();
				TrackPoints2.Add(_ptInOutBase);

				ptTmp = (Point)_EndFIX.PrjPt.Clone();
				ptTmp.M = InnerDir;
				TrackPoints2.Add(ptTmp);

				_NomTrack2 = ARANFunctions.ConvertPointsToTrackLIne(TrackPoints2);
			}
			else if (_StartFIX is MATF)
			{
				MATF Matf = (MATF)_StartFIX;
				double NomDir;

				ptStart = (Point)_StartFIX.PrjPt.Clone();
				ptStart.M = EntryDir;

				if (Matf.FlyPath == eFlyPath.DirectToFIX)
				{
					NomDir = CalcDRNomDir();
					TurnAngle = (NomDir - _StartFIX.EntryDirection) * fTurnDir; //????
				}
				else
					NomDir = Matf.OutDirection;

				//TurnAngle = ARANMath.Modulus((OutDir - EntryDir) * fTurnDir, ARANMath.C_2xPI);

				if ((Matf.FlyPath == eFlyPath.TrackToFIX || Matf.FlyPath == eFlyPath.CourseToFIX) &&     //
					(Matf.TurnAt == eTurnAt.MApt || Matf.FlyMode == eFlyMode.Flyover || Matf.FlyMode == eFlyMode.Atheight))
				{
					R2 = ARANMath.BankToRadius(Bank150, _StartFIX.NomLineTAS);

					fTmp = Math.Acos((1.0 + R1 * Math.Cos(TurnAngle) / R2) / (1.0 + R1 / R2)) - ARANMath.EpsilonRadian;


					//_UI.DrawPointWithText(ptStart, "ptStart");
					//ProcessMessages();

					DivergenceAngle = Math.Min(DivergenceAngle30, fTmp);

					if (Matf.TurnAt != eTurnAt.TP)
					{
						ptTmp = (Point)Matf.maPt.PrjPt.Clone();
						//ptTmp = (Point)Matf.PrjPt.Clone();
						ptTmp.M = _StartFIX.EntryDirection;
					}
					else
					{
						ptTmp = (Point)Matf.PrjPt.Clone();
						ptTmp.M = _StartFIX.EntryDirection;
						//TrackPoints.Add(ptTmp);						//	Point1
					}

					TrackPoints.Add(ptTmp);                         //	Point1

					TrackPoints.Add(ptStart);                       //	Point2

					PtTmp0 = ARANFunctions.LocalToPrj(ptStart, _StartFIX.EntryDirection + ARANMath.C_PI_2 * fTurnDir, R1, 0);

					fTmp = _StartFIX.EntryDirection + (TurnAngle + DivergenceAngle) * fTurnDir;

					PtTmp1 = ARANFunctions.PointAlongPlane(PtTmp0, fTmp - ARANMath.C_PI_2 * fTurnDir, R1);
					PtTmp1.M = fTmp;

					//_UI.DrawPointWithText(PtTmp0, "ptCnt");
					//_UI.DrawPointWithText(PtTmp1, "Point2");
					//_UI.DrawPointWithText(ptTmp, "maPt");
					//_UI.DrawPointWithText(_StartFIX.PrjPt, "StartFIX");


					////ProcessMessages(true);
					//_UI.DrawPointWithText(ptStart, "ptStart");
					//ProcessMessages();

					TrackPoints.Add(PtTmp1);                    //	Point2

					PtTmp0 = (Point)ARANFunctions.LineLineIntersect(PtTmp1, PtTmp1.M, _EndFIX.PrjPt, _EndFIX.EntryDirection);   //OutDir);

					fTmp = R2 * Math.Tan(0.5 * DivergenceAngle);

					PtTmp2 = ARANFunctions.LocalToPrj(PtTmp0, PtTmp1.M + ARANMath.C_PI, fTmp, 0);
					PtTmp2.M = PtTmp1.M;

					PtTmp3 = ARANFunctions.LocalToPrj(PtTmp0, _EndFIX.EntryDirection, fTmp, 0);
					PtTmp3.M = _EndFIX.EntryDirection;      //OutDir;

					TrackPoints.Add(PtTmp2);                    //	Point3
					TrackPoints.Add(PtTmp3);                    //	Point4

					PtTmp0.Assign(_EndFIX.PrjPt);
					PtTmp0.M = _EndFIX.EntryDirection;      //OutDir;
					TrackPoints.Add(PtTmp0);
				}
				else if (fTmp > ARANMath.EpsilonRadian && ARANMath.C_PI - fTmp > ARANMath.EpsilonRadian)
				{

					//_UI.DrawPointWithText(Matf.PrjPt, "Matf", 255 << 14);
					//_UI.DrawPointWithText(Matf.maPt.PrjPt, "maPt", 255 << 14);
					//ProcessMessages();

					if (Matf.TurnAt == eTurnAt.MApt)
						ptTmp = (Point)Matf.maPt.PrjPt.Clone();
					else
						ptTmp = (Point)Matf.PrjPt.Clone();

					Point ptFlyBy = (Point)ARANFunctions.LineLineIntersect(ptTmp, _StartFIX.EntryDirection, _EndFIX.PrjPt, NomDir);

					double TurnDist = R1 / Math.Tan(0.5 * (ARANMath.C_PI - TurnAngle));
					PtTmp1 = ARANFunctions.LocalToPrj(ptFlyBy, _StartFIX.EntryDirection, -TurnDist, 0);
					PtTmp1.M = _StartFIX.EntryDirection;

					if (ARANFunctions.PointToLineDistance(PtTmp1, ptStart, _StartFIX.EntryDirection + ARANMath.C_PI_2) < 0)
						TrackPoints.Add(ptStart);

					TrackPoints.Add(PtTmp1);

					PtTmp1 = ARANFunctions.LocalToPrj(ptFlyBy, NomDir, TurnDist, 0);
					PtTmp1.M = NomDir;
					TrackPoints.Add(PtTmp1);

					if (ARANFunctions.PointToLineDistance(PtTmp1, _EndFIX.PrjPt, NomDir + ARANMath.C_PI_2) > 0.0)
					{
						ptTmp = (Point)_EndFIX.PrjPt.Clone();
						ptTmp.M = NomDir;
						TrackPoints.Add(ptTmp);
					}
				}
				else
				{
					TrackPoints.Add(ptStart);
					Point pt6sec = new Point();

					if (Matf.TurnAt == eTurnAt.MApt || Matf.TurnAt == eTurnAt.Altitude)
					{
						double PilotTolerance = _constants.Pansops[ePANSOPSData.arMAPilotToleran].Value;
						double BankTolerance = _constants.Pansops[ePANSOPSData.arMAPilotToleran].Value;
						double Dist6sec = (Matf.NomLineTAS + _constants.Pansops[ePANSOPSData.dpWind_Speed].Value) * (PilotTolerance + BankTolerance);
						pt6sec = ARANFunctions.LocalToPrj(Matf.PrjPt, Matf.EntryDirection, Dist6sec);
						pt6sec.M = Matf.EntryDirection;
						TrackPoints.Add(pt6sec);
					}
					else
						pt6sec.Assign(Matf.PrjPt);

					PtTmp1 = ARANFunctions.PrjToLocal(pt6sec, Matf.EntryDirection, _EndFIX.PrjPt);

					if (PtTmp1.X > 0)
					{
						PtTmp1 = ARANFunctions.LocalToPrj(pt6sec, Matf.EntryDirection, PtTmp1.X);
						PtTmp1.M = NomDir;
						TrackPoints.Add(PtTmp1);
					}
					else
					{
						PtTmp1 = ARANFunctions.LocalToPrj(_EndFIX.PrjPt, NomDir, PtTmp1.X);
						PtTmp1.M = NomDir;
						TrackPoints.Add(PtTmp1);
					}

					ptTmp = _EndFIX.PrjPt;
					ptTmp.M = NomDir;
					TrackPoints.Add(ptTmp);
				}
			}
			else
			{
				if (_StartFIX.FlyMode == eFlyMode.Flyby)
				{
					double L1 = R1 * Math.Tan(0.5 * TurnAngle);

					PtTmp0 = ARANFunctions.PointAlongPlane(ptStart, EntryDir + Math.PI, L1);
					PtTmp0.M = EntryDir;
					TrackPoints.Add(PtTmp0);

					PtTmp0 = ARANFunctions.LocalToPrj(ptStart, EntryDir + TurnAngle * fTurnDir, L1);
					PtTmp0.M = OutDir;

					TrackPoints.Add(PtTmp0);
				}
				else if (_StartFIX.FlyMode == eFlyMode.Flyover || _StartFIX.FlyMode == eFlyMode.Atheight)
				{
					double x, y;

					Point PtCenter0 = ARANFunctions.LocalToPrj(ptStart, EntryDir + ARANMath.C_PI_2 * fTurnDir, R1);
					ARANFunctions.PrjToLocal(_EndFIX.PrjPt, OutDir + Math.PI, PtCenter0, out x, out y);

					R2 = ARANMath.BankToRadius(Bank150, _StartFIX.NomLineTAS);

					fTmp = R1 + R2;
					double fTmp1 = R2 - fTurnDir * y;
					double radik = 0;
					if (fTmp > fTmp1)
						radik = fTmp * fTmp - fTmp1 * fTmp1;

					double dx = Math.Sqrt(radik);

					Point PtCenter1 = ARANFunctions.LocalToPrj(_EndFIX.PrjPt, OutDir + Math.PI, x - dx, fTurnDir * R2);

					TrackPoints.Add(ptStart);

					//_UI.DrawPointWithText(PtCenter0, "PtCenter0");
					//_UI.DrawPointWithText(PtCenter1, "PtCenter1");
					//_UI.DrawPointWithText(_EndFIX.PrjPt, "EndFIX");
					//_UI.DrawPointWithText(_StartFIX.PrjPt, "StartFIX");
					//ProcessMessages();

					double rollDir = ARANFunctions.ReturnAngleInRadians(PtCenter0, PtCenter1);

					PtTmp1 = ARANFunctions.LocalToPrj(PtCenter0, rollDir, R1);
					PtTmp1.M = rollDir + ARANMath.C_PI_2 * fTurnDir;

					TrackPoints.Add(PtTmp1);

					PtTmp2 = ARANFunctions.LocalToPrj(_EndFIX.PrjPt, OutDir + Math.PI, x - dx);
					PtTmp2.M = OutDir;
					//_UI.DrawPointWithText(PtTmp2, "PtTmp2");
					//ProcessMessages();

					TrackPoints.Add(PtTmp2);                                            //	Point4
				}
				else
				{
					//if(_EndFIX
				}

				PtTmp0 = (Point)_EndFIX.PrjPt.Clone();
				PtTmp0.M = OutDir;

				TrackPoints.Add(PtTmp0);                //	Point6
			}

			if (Math.Abs(TurnAngle) <= minTurnAngle)
			{
				_NomTrack = new LineString();
				_NomTrack.Add(ptStart);
				_NomTrack.Add(TrackPoints[TrackPoints.Count - 1]);
			}
			else
				_NomTrack = ARANFunctions.ConvertPointsToTrackLIne(TrackPoints);

			//_UI.DrawLineString(_NomTrack, 2, 255 << 16);
			//ProcessMessages(true);
			//_UI.DrawLineString(_NomTrack, 2, 255 << 8);
			//_UI.DrawPointWithText(ptStart, "ptStart");
			//_UI.DrawPointWithText(_EndFIX.PrjPt, "ptEnd");
			//ProcessMessages();

			//_UI.DrawLineString(PrevLeg._NomTrack, 1);
			//ProcessMessages();

			//for (int I = 0; I < TrackPoints.Count; I++)
			//    _UI.DrawPointWithText(TrackPoints[I], 255 << 14, "Pt_" + (I + 1).ToString());

			//_UI.DrawLineString(PrevLeg._NomTrack, 1, 255 << 16);
			//_UI.DrawLineString(_NomTrack, 1, 255 << 8);
			//ProcessMessages();

			//_UI.DrawPointWithText(ptStart, "ptStart");
			//ProcessMessages();

			if (PrevLeg != null && PrevLeg._NomTrack != null)
			{
				PrevLeg._NomTrack.Remove(PrevLeg._NomTrack.Count - 1);
				PrevLeg._NomTrack.Add(_NomTrack[0]);
			}
		}

		public void CreateGeometry(LegBase PrevLeg, ADHPType ARP, double minTurnAngle = 0.0, bool isPbn = false)
		{
			//if (_StartFIX.PrjPt.IsEmpty || _EndFIX.PrjPt.IsEmpty)
			//	return;

			//_StartFIX.FlyMode = eFlyMode.Flyby;
			CreateNomTrack(PrevLeg, minTurnAngle);

			//_UI.DrawLineString(_NomTrack, 2, 255 << 8);
			//_UI.DrawLineString(PrevLeg._NomTrack, 2, 255);
			//PrevLeg.RefreshGraphics();
			//ProcessMessages();

			createProtectionArea(PrevLeg, ARP);

			CreateKKLine(PrevLeg);
			CreateNNSecondLine(PrevLeg);
			createAssesmentArea(PrevLeg, isPbn);

			//ProcessMessages(true);
			//_UI.DrawMultiPolygon(FullAssesmentArea, eFillStyle.sfsBackwardDiagonal);
			//_UI.DrawMultiPolygon(PrevLeg.FullAssesmentArea, eFillStyle.sfsBackwardDiagonal);
			//ProcessMessages();

			if (PrevLeg != null)
				PrevLeg.RefreshGraphics();
		}

		public void CreateOutline(List<MultiLineString> CapLines)
		{
			const double EPS = 0.05;

			MultiPolygon PolygonL = ARANFunctions.RemoveAgnails(_FullAreaS);
			MultiLineString tmpPoly = new MultiLineString();

			foreach (Polygon tmpPolygon in PolygonL)
			{
				for (int j = 0; j <= tmpPolygon.InteriorRingList.Count; j++)
				{
					Ring ring;

					if (j == 0)
						ring = tmpPolygon.ExteriorRing;
					else
						ring = tmpPolygon.InteriorRingList[j - 1];

					LineString tmpPart = new LineString();

					for (int L = 0, L1 = 0; L <= ring.Count; L++)
					{
						int L0 = L1;
						L1 = (L + 1) % ring.Count;

						Point ptl0 = ring[L0];
						Point ptl1 = ring[L1];

						double dx0 = ptl1.X - ptl0.X;
						double dy0 = ptl1.Y - ptl0.Y;

						bool bCoincident = false;

						foreach (MultiLineString pLine in CapLines)
						{
							foreach (LineString Part in pLine)
							{
								for (int m = 0; m < Part.Count - 1; m++)
								{
									Point ptr0 = Part[m];
									Point ptr1 = Part[m + 1];

									double dx1 = ptr1.X - ptr0.X;
									double dy1 = ptr1.Y - ptr0.Y;

									double num1 = dx0 * (ptr0.Y - ptl0.Y) - dy0 * (ptr0.X - ptl0.X);
									double num2 = dx1 * (ptr0.Y - ptl0.Y) - dy1 * (ptr0.X - ptl0.X);
									double den = dy0 * dx1 - dx0 * dy1;

									// Are the line coincident?
									if (Math.Abs(den) < EPS)
									{
										bCoincident = (Math.Abs(num1) < EPS && Math.Abs(num2) < EPS);
										if (!bCoincident)
											bCoincident = ARANFunctions.PointToSegmentDistance(ptl0, ptr0, ptr1) < 0.6;
									}

									if (bCoincident)
										break;
								}

								if (bCoincident)
									break;
							}

							if (bCoincident)
								break;
						}

						if (bCoincident)
						{
							if (tmpPart.Count > 0)
							{
								tmpPart.Add(ptl0);
								tmpPoly.Add(tmpPart);

								tmpPart = new LineString();
							}
						}
						else
							tmpPart.Add(ptl0);
					}
				}
			}
			_AssesmentAreaOutline = tmpPoly;
		}

		protected Ring CreateDtFSegment(LegBase PrevLeg, bool IsPrimary)
		{
			Geometry Geom;
			Point ptCurr;
			double EntryDir1, EntryDir2;
			double OutDir1, OutDir2, LPT1, LPT2, TurnR, fTmp;

			//==============================================================================
			double SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
			double DivergenceAngle30 = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;

			TurnDirection TurnDir = _StartFIX.EffectiveTurnDirection;
			double fTurnSide = -(int)TurnDir;
			double WSpeed = _constants.Pansops[ePANSOPSData.dpWind_Speed].Value;
			double Rv = (1.76527777777777777777 / Math.PI) * Math.Tan(_StartFIX.BankAngle) / _StartFIX.ConstructTAS;
			if (Rv > 0.003) Rv = 0.003;

			if (Rv > 0.0) TurnR = _StartFIX.ConstructTAS / ((5.555555555555555555555 * Math.PI) * Rv);
			else TurnR = -1;
			double coeff = WSpeed / ARANMath.DegToRad(1000.0 * Rv);

			if (_StartFIX.IsDFTarget)
			{
				if (_StartFIX.TurnDirection == TurnDirection.CW)
				{
					EntryDir1 = _StartFIX.EntryDirection_L;
					EntryDir2 = _StartFIX.EntryDirection_R;
					LPT1 = _StartFIX.LPT_L;
					LPT2 = _StartFIX.LPT_R;
				}
				else
				{
					EntryDir1 = _StartFIX.EntryDirection_R;
					EntryDir2 = _StartFIX.EntryDirection_L;
					LPT1 = _StartFIX.LPT_R;
					LPT2 = _StartFIX.LPT_L;
				}
			}
			else
			{
				EntryDir1 = _StartFIX.EntryDirection;
				EntryDir2 = EntryDir1;
				LPT1 = _StartFIX.LPT;
				LPT2 = LPT1;
			}

			//double outerTurn, innerTurn;
			TurnDirection innerTurnDir;

			if (_StartFIX.TurnDirection == TurnDirection.CW)
			{
				OutDir1 = _StartFIX.OutDirection_L;
				OutDir2 = _StartFIX.OutDirection_R;

				//outerTurnDir = _StartFIX.TurnDirection_L;
				innerTurnDir = _StartFIX.TurnDirection_R;

				//outerTurn = -(int)outerTurnDir;
				//innerTurn = -(int)innerTurnDir;
			}
			else
			{
				OutDir1 = _StartFIX.OutDirection_R;
				OutDir2 = _StartFIX.OutDirection_L;

				//outerTurnDir = _StartFIX.TurnDirection_R;
				innerTurnDir = _StartFIX.TurnDirection_L;

				//outerTurn = -(int)outerTurnDir;
				//innerTurn = -(int)innerTurnDir;
			}

			//ProcessMessages(true);

			double dRad = 0;
			double ASW_OUT0Fr, ASW_OUT0Fl;
			double ASW_OUT0Cr, ASW_OUT0Cl;
			double ASW_OUT1r, ASW_OUT1l;
			Point ptTmp, ptStrt = null, ptFromr = null, ptFroml = null, ptFrom0r = null, ptFrom0l = null;

			#region Setup

			WayPoint startFIX = (WayPoint)_StartFIX;
			//Ring prevRing;
			//if (_StartFIX is MATF)
			//{
			//	MATF Matf = (MATF)_StartFIX;
			//	startFIX = Matf.maPt;

			//	//	MAPt mAPt = Matf.maPt;
			//	//	startFIX = mAPt;

			//	//	if (IsPrimary)
			//	//		prevRing = Matf.TolerArea[0].ExteriorRing;
			//	//	else
			//	//		prevRing = Matf.SecondaryTolerArea[0].ExteriorRing;
			//}
			//else
			//{
			//	if (IsPrimary)
			//		prevRing = PrevLeg.PrimaryArea[0].ExteriorRing;
			//	else
			//		prevRing = PrevLeg.FullArea[0].ExteriorRing;
			//}

			//_UI.DrawRing(prevRing, eFillStyle.sfsDiagonalCross);
			//_UI.DrawPointWithText(startFIX.PrjPt, "mapt");
			//_UI.DrawPointWithText(_StartFIX.PrjPt, "StartFIX");
			//ProcessMessages();

			//LineString ls = new LineString();
			//ls.Add(_StartFIX.PrjPt);
			//ls.Add(ARANFunctions.LocalToPrj(_StartFIX.PrjPt, OutDir1, 10000.0, 0));
			//_UI.DrawLineString(ls,-1,1);
			//ProcessMessages();

			//ls.Clear();
			//ls.Add(_StartFIX.PrjPt);
			//ls.Add(ARANFunctions.LocalToPrj(_StartFIX.PrjPt, OutDir2, 10000.0, 0));
			//_UI.DrawLineString(ls, -1, 1);

			//Point ptLPTr = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_R, _StartFIX.LPT_R);
			//Point ptLPTl = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_L, _StartFIX.LPT_L);


			Point ptLPTr = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_R, _StartFIX.LPT_R);
			Point ptLPTl = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_L, _StartFIX.LPT_L);

			//ProcessMessages(true);

			Point ptEPTr = ARANFunctions.LocalToPrj(startFIX.PrjPt, startFIX.EntryDirection_R, -startFIX.EPT_R + 0.01);
			Point ptEPTl = ARANFunctions.LocalToPrj(startFIX.PrjPt, startFIX.EntryDirection_L, -startFIX.EPT_L + 0.01);

			//_UI.DrawPointWithText(ptLPTr, "ptLPTr-1");
			//_UI.DrawPointWithText(ptLPTl, "ptLPTl-1");
			//ProcessMessages();

			//_UI.DrawPointWithText(ptEPTr, "ptEPTr-1");
			//_UI.DrawPointWithText(ptEPTl, "ptEPTl-1");

			//ProcessMessages();

			if (IsPrimary)
			{
				ASW_OUT0Fr = _StartFIX.ASW_2_R;
				ASW_OUT0Fl = _StartFIX.ASW_2_L;

				if (PrevLeg != null)
				{
					if (TurnDir == TurnDirection.CCW)
						ptStrt = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, _StartFIX.PrjPt, _StartFIX.EntryDirection_R - ARANMath.C_PI_2, out fTmp);
					else
						ptStrt = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, _StartFIX.PrjPt, _StartFIX.EntryDirection_L + ARANMath.C_PI_2, out fTmp);

					ptTmp = ARANFunctions.LocalToPrj(ptLPTr, _StartFIX.EntryDirection_R, -0.001);
					ptFromr = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, _StartFIX.EntryDirection_R - ARANMath.C_PI_2, out fTmp);
					if (ptFromr != null)
						ASW_OUT0Fr = fTmp;
					else
						ptFromr = ARANFunctions.LocalToPrj(ptLPTr, _StartFIX.EntryDirection_R, 0.0, -ASW_OUT0Fr);

					ptTmp = ARANFunctions.LocalToPrj(ptLPTl, _StartFIX.EntryDirection_L, -0.01);
					ptFroml = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, _StartFIX.EntryDirection_L + ARANMath.C_PI_2, out fTmp);
					if (ptFroml != null)
						ASW_OUT0Fl = fTmp;
					else
						ptFroml = ARANFunctions.LocalToPrj(ptLPTl, _StartFIX.EntryDirection_L, 0.0, ASW_OUT0Fl);

					ptFrom0r = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptEPTr, _StartFIX.EntryDirection_R - ARANMath.C_PI_2, out fTmp);
					ptFrom0l = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptEPTl, _StartFIX.EntryDirection_L + ARANMath.C_PI_2, out fTmp);

					//_UI.DrawPolygon(PrevLeg.PrimaryArea[0], -1, eFillStyle.sfsDiagonalCross);
					//_UI.DrawPointWithText(ptFromr, "ptFromR");
					//_UI.DrawPointWithText(ptFroml, "ptFromL");
					//_UI.DrawPointWithText(ptFrom0r,"ptFromR0");
					//_UI.DrawPointWithText(ptFrom0l, "ptFromL0");
					//ProcessMessages();
				}

				ASW_OUT0Cr = 0.5 * _StartFIX.SemiWidth;
				ASW_OUT1r = 0.5 * _EndFIX.SemiWidth;

				ASW_OUT0Cl = 0.5 * _StartFIX.SemiWidth;
				ASW_OUT1l = 0.5 * _EndFIX.SemiWidth;
			}
			else
			{
				ASW_OUT0Fr = _StartFIX.ASW_R;
				ASW_OUT0Fl = _StartFIX.ASW_L;

				if (PrevLeg != null)
				{
					if (TurnDir == TurnDirection.CCW)
						ptStrt = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, _StartFIX.PrjPt, _StartFIX.EntryDirection_R - ARANMath.C_PI_2, out fTmp);
					else
						ptStrt = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, _StartFIX.PrjPt, _StartFIX.EntryDirection_L + ARANMath.C_PI_2, out fTmp);

					ptTmp = ARANFunctions.LocalToPrj(ptLPTr, _StartFIX.EntryDirection_R, -0.01);
					ptFromr = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, _StartFIX.EntryDirection_R - ARANMath.C_PI_2, out fTmp, true);

					//_UI.DrawPointWithText(ptTmp, "ptTmp");
					//_UI.DrawPointWithText(ptStrt, "ptStrt");
					////_UI.DrawPointWithText(ptFromr, "ptFromr");
					//_UI.DrawMultiPolygon(PrevLeg.FullArea,eFillStyle.sfsCross);

					//LineString ls = new LineString();
					//ls.Add(ptTmp);
					//ls.Add(ARANFunctions.LocalToPrj(ptTmp, _StartFIX.EntryDirection_R - ARANMath.C_PI_2, 5000));
					//_UI.DrawLineString(ls);
					//ProcessMessages();

					if (ptFromr != null)
						ASW_OUT0Fr = fTmp;
					else
						ptFromr = ARANFunctions.LocalToPrj(ptLPTr, _StartFIX.EntryDirection_R, 0.0, -ASW_OUT0Fr);

					ptTmp = ARANFunctions.LocalToPrj(ptLPTl, _StartFIX.EntryDirection_L, -0.001);
					ptFroml = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, _StartFIX.EntryDirection_L + ARANMath.C_PI_2, out fTmp, true);

					//_UI.DrawPointWithText(ptFroml, "ptFroml");
					//ProcessMessages();

					if (ptFroml != null)
						ASW_OUT0Fl = fTmp;
					else
						ptFroml = ARANFunctions.LocalToPrj(ptLPTl, _StartFIX.EntryDirection_L, 0.0, ASW_OUT0Fl);

					ptFrom0r = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptEPTr, _StartFIX.EntryDirection_R - ARANMath.C_PI_2, out fTmp, true);
					ptFrom0l = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptEPTl, _StartFIX.EntryDirection_L + ARANMath.C_PI_2, out fTmp, true);

					//_UI.DrawMultiPolygon(PrevLeg.FullArea, eFillStyle.sfsCross);
					//_UI.DrawPointWithText(ptFromr, "ptFromr");
					//_UI.DrawPointWithText(ptFroml, "ptFroml");

					//_UI.DrawPointWithText(ptFrom0r, "ptFrom0r");
					//_UI.DrawPointWithText(ptFrom0l, "ptFrom0l");
					//ProcessMessages();
				}

				ASW_OUT0Cr = _StartFIX.SemiWidth;
				ASW_OUT1r = _EndFIX.SemiWidth;

				ASW_OUT0Cl = _StartFIX.SemiWidth;
				ASW_OUT1l = _EndFIX.SemiWidth;

				if (TurnDir == TurnDirection.CCW)
					dRad = ASW_OUT0Fr - _StartFIX.ASW_2_R;
				else
					dRad = ASW_OUT0Fl - _StartFIX.ASW_2_L;

				if (dRad < 0.0) dRad = 0.0;

				////_UI.DrawPolygon(PrevLeg.PrimaryArea[0], -1, eFillStyle.sfsDiagonalCross);
				////_UI.DrawPointWithText(ptStrt, -1, "ptStrt");
				//_UI.DrawPointWithText(ptFromr, -1, "ptFromR");
				//_UI.DrawPointWithText(ptFroml, -1, "ptFromL");
				//_UI.DrawPointWithText(ptFrom0r, -1, "ptFromR0");
				//_UI.DrawPointWithText(ptFrom0l, -1, "ptFromL0");
				//ProcessMessages();
			}

			int i, n;
			List<Point> BasePoints;
			Point[] bpt;

			if (_StartFIX.FlyMode == eFlyMode.Atheight)
			{
				if (IsPrimary)
					BasePoints = ARANFunctions.GetBasePoints(_StartFIX.PrjPt, _StartFIX.EntryDirection, PrevLeg.PrimaryArea[0], TurnDir);
				else
					BasePoints = ARANFunctions.GetBasePoints(_StartFIX.PrjPt, _StartFIX.EntryDirection, PrevLeg.FullArea[0], TurnDir);

				n = BasePoints.Count;

				//for (i = 0; i < n; i++)
				//	_UI.DrawPointWithText(BasePoints[i], "pt_" + (i + 1).ToString());
				//ProcessMessages();

				bpt = new Point[n];

				for (i = 0; i < n; i++)
					bpt[i] = BasePoints[i];

			}
			else
			{
				n = 4;      //n = _StartFIX.BasePoints.Count;
				bpt = new Point[n];
				BasePoints = new List<Point>();

				if (TurnDir == TurnDirection.CCW)
				{
					bpt[0] = ptFromr;
					bpt[1] = ptFroml;
					bpt[2] = ptFrom0l;
					bpt[3] = ptFrom0r;
				}
				else
				{
					bpt[0] = ptFroml;
					bpt[1] = ptFromr;
					bpt[2] = ptFrom0r;
					bpt[3] = ptFrom0l;
				}

				for (i = 0; i < n; i++)
				{
					if (IsPrimary)
					{
						if (bpt[i] == null)
							bpt[i] = _StartFIX.BasePoints[i];
						BasePoints.Add(bpt[i]);
					}
					else
					{
						if (bpt[i] == null)
						{
							ptTmp = ARANFunctions.PrjToLocal(_StartFIX.PrjPt, _StartFIX.EntryDirection, _StartFIX.BasePoints[i]);

							if (ptTmp.Y < 0)
							{
								//ptTmp = ARANFunctions.LocalToPrj(_StartFIX.BasePoints[i], EntryDir2, 0, -dRad);
								bpt[i] = ARANFunctions.LocalToPrj(_StartFIX.BasePoints[i], EntryDir2, 0, -dRad);
							}
							else
							{
								//ptTmp = ARANFunctions.LocalToPrj(_StartFIX.BasePoints[i], EntryDir1, 0, dRad);
								bpt[i] = ARANFunctions.LocalToPrj(_StartFIX.BasePoints[i], EntryDir1, 0, dRad);
							}
							ptTmp = _StartFIX.BasePoints[i];
						}
						else
						{
							//ptTmp = ARANFunctions.PrjToLocal(_StartFIX.PrjPt, _StartFIX.EntryDirection, bpt[i]);

							//if (ptTmp.Y < 0)
							//	ptTmp = ARANFunctions.LocalToPrj(bpt[i], EntryDir2, 0, dRad);
							//else
							//	ptTmp = ARANFunctions.LocalToPrj(bpt[i], EntryDir1, 0, -dRad);

							ptTmp = (Point)bpt[i].Clone();
						}

						BasePoints.Add(ptTmp);
					}
				}
			}

			//for (i = 0; i < n; i++)
			//	_UI.DrawPointWithText(_StartFIX.BasePoints[i], "ptB-" + (i + 1).ToString());
			//ProcessMessages();

			Ring result = new Ring();

			//_UI.DrawPointWithText(ptStrt, "ptStrt");
			//ProcessMessages();

			if (ptStrt != null)
				result.Add(ptStrt);

			if (TurnDir == TurnDirection.CCW)
			{
				if (ptFromr != null)
					result.Add(ptFromr);
				else
					result.Add(bpt[0]);
			}
			else if (ptFroml != null)
				result.Add(ptFroml);
			else
				result.Add(bpt[0]);

			//_UI.DrawPointWithText(ptFromr, -1, "ptFromR");
			//_UI.DrawPointWithText(ptFromr, -1, "ptFroml");
			//_UI.DrawPointWithText(ptStrt, -1, "ptStart");
			//ProcessMessages();
			#endregion

			#region Outer side

			//Outer side ===================================================================================
			//outerTurn = fTurnSide;
			//innerTurn = fTurnSide;

			//EntryDir1 = NativeMethods.Modulus(EntryDir1, 2 * Math.PI);
			//EntryDir2 = NativeMethods.Modulus(EntryDir2, 2 * Math.PI);

			double dirSpCenter = EntryDir1 - ARANMath.C_PI_2 * fTurnSide;
			double SpStartRadial = EntryDir1 + ARANMath.C_PI_2 * fTurnSide;

			double TurnRCurr = TurnR + dRad;
			double dirDst30 = OutDir1 - DivergenceAngle30 * fTurnSide;
			double SpTurnAngle = 0;

			int j = 1;
			int continueMode = 0;
			double dirRef = ARANMath.Modulus(Math.Atan2(BasePoints[1].Y - BasePoints[0].Y, BasePoints[1].X - BasePoints[0].X), ARANMath.C_2xPI);

			Point ptRef = ARANFunctions.LocalToPrj(_EndFIX.PrjPt, OutDir1, 0, ASW_OUT1l * fTurnSide);
			//Point ptRef = ARANFunctions.LocalToPrj(_EndFIX.PrjPt, OutDir1, 0, ASW_OUT0Cl * fTurnSide);

			Point ptCnt = null;

			//_UI.DrawPointWithText(ptRef, -1, "ptRef");
			//_UI.DrawPointWithText(_EndFIX.PrjPt, -1, "_EndFIX");
			//ProcessMessages();


			double dAngle = Math.Atan2(coeff, TurnRCurr);
			double outerTurnAngle = ARANMath.Modulus((EntryDir1 - OutDir1) * fTurnSide, ARANMath.C_2xPI);

			double dirCurr;         // = EntryDir1 + dAngle * outerTurn;
			double dirDst15;

			if (outerTurnAngle + dAngle < SplayAngle15)
			{
				dirDst15 = OutDir1 + SplayAngle15 * fTurnSide;
				dirCurr = dirDst15;
			}
			else
			{
				dirDst15 = OutDir1 + SplayAngle15 * fTurnSide;
				dirCurr = EntryDir1 + dAngle * fTurnSide;
			}


			//LineString lst = new LineString();
			//lst.Add(BasePoints[0]);
			//lst.Add(ARANFunctions.LocalToPrj(BasePoints[0], dirDst15, -30000, 0));
			//_UI.DrawLineString(lst, -1, 2);
			//ProcessMessages();
			//_UI.DrawPointWithText(ptRef, -1, "ptRef");
			//ProcessMessages();
			//double turnAngle = ARANMath.Modulus((dirCurr - OutDir1) * fTurnSide, ARANMath.C_2xPI);

			double turnAngle = ARANMath.Modulus((dirCurr - dirDst15) * fTurnSide, ARANMath.C_2xPI);

			//if (ARANMath.Modulus((dirCurr - dirDst15) * fTurnSide, ARANMath.C_2xPI) < Math.PI || turnAngle > 2 * SplayAngle15)
			if (turnAngle < Math.PI || turnAngle > 2 * SplayAngle15)
			{
				for (i = 0; i < n; i++)
				{
					//ptCnt = ARANFunctions.LocalToPrj(BasePoints[i], dirSpCenter, TurnR);
					ptCnt = ARANFunctions.LocalToPrj(BasePoints[i], dirSpCenter, TurnRCurr);

					//ProcessMessages(true);

					//_UI.DrawPointWithText(BasePoints[i], "BasePoints[" + i.ToString() + "]");
					//_UI.DrawPointWithText(ptCnt, "ptCnt-P_" + i.ToString());
					//ProcessMessages();

					j = i + 1 < n ? i + 1 : 0;

					dirRef = ARANMath.Modulus(Math.Atan2(BasePoints[j].Y - BasePoints[i].Y, BasePoints[j].X - BasePoints[i].X), ARANMath.C_2xPI);

					//LineString lst = new LineString();
					//lst.Add(_EndFIX.PrjPt);
					//lst.Add(ARANFunctions.LocalToPrj(_EndFIX.PrjPt, OutDir1, -30000, 0));
					//_UI.DrawLineString(lst, -1, 2);
					//ProcessMessages();

					double dAlpha = ARANMath.Modulus((dirCurr - dirDst15) * fTurnSide, ARANMath.C_2xPI);
					//double dAlpha = ARANMath.Modulus((dirCurr - OutDir1) * fTurnSide, ARANMath.C_2xPI);
					fTmp = ARANMath.Modulus((dirCurr - dirRef) * fTurnSide, ARANMath.C_2xPI);

					if (dAlpha < fTmp)
					{
						SpTurnAngle = ARANFunctions.SpiralTouchAngle(TurnRCurr, coeff, SpStartRadial, dirCurr, dirDst15, TurnDir);
						//SpTurnAngle = ARANFunctions.SpiralTouchAngle(TurnRCurr, coeff, SpStartRadial, dirCurr, OutDir1, outerTurnDir);

						//ptTmp = ARANFunctions.LocalToPrj(ptCnt, SpStartRadial , TurnRCurr , 0);
						//_UI.DrawPointWithText(ptTmp, IsPrimary ? 255 : 0, "ptSp");
						//ProcessMessages();

						ptTmp = ARANFunctions.LocalToPrj(ptCnt, SpStartRadial - SpTurnAngle * fTurnSide, TurnRCurr + SpTurnAngle * coeff, 0);
						//_UI.DrawPointWithText(ptTmp, 255, "pt-15");
						//ProcessMessages();

						//LineString lst = new LineString();
						//lst.Add(ARANFunctions.LocalToPrj(ptRef, OutDir1, 30000, 0));
						//lst.Add(ARANFunctions.LocalToPrj(ptRef, OutDir1, -30000, 0));
						//_UI.DrawLineString(lst, 255, 2);
						//_UI.DrawPointWithText(ptTmp, "pt-15");
						//_UI.DrawPointWithText(ptRef, "ptRef");
						//_UI.DrawRing(result, -1, eFillStyle.sfsCross);
						//ProcessMessages();

						if (ARANMath.SideDef(ptRef, OutDir1, ptTmp) == (SideDirection)TurnDir)      // (SideDirection)outerTurnDir
						{
							ARANFunctions.AddSpiralToRing(ptCnt, TurnRCurr, coeff, SpStartRadial, SpTurnAngle, TurnDir, ref result);

							//_UI.DrawRing(result, -1, eFillStyle.sfsCross);
							//ProcessMessages();

							//dirCurr = OutDir1;
							dirCurr = dirDst15;
							TurnRCurr = TurnRCurr + coeff * SpTurnAngle;
							SpStartRadial = SpStartRadial - SpTurnAngle * fTurnSide;
							continueMode = 1; //15 degree intersecting
							break;
						}
						else
						{
							//Ring rr = new Ring();
							//ARANFunctions.AddSpiralToRing(ptCnt, TurnRCurr, coeff, SpStartRadial,
							//						ARANMath.C_PI_2, outerTurnDir, ref rr);
							//_UI.DrawRing(rr, 255, eFillStyle.sfsCross);
							//ProcessMessages();

							double dBetta;
							dBetta = ARANMath.Modulus((dirDst30 - dirRef) * fTurnSide, ARANMath.C_2xPI);
							//dBetta = ARANMath.Modulus((dirRef - dirDst30) * fTurnSide, ARANMath.C_2xPI);
							continueMode = 2; //30 degree intersecting
							if (dBetta < ARANMath.C_PI)
								break;

							//dBetta = ARANMath.Modulus((dirCurr - dirDst30) * fTurnSide, ARANMath.C_2xPI);
							//if (dBetta < fTmp)
							//	break;
						}
					}

					//ProcessMessages(true);
					SpTurnAngle = ARANFunctions.SpiralTouchAngle(TurnRCurr, coeff, SpStartRadial, dirCurr, dirRef, TurnDir);

					if (SpTurnAngle > Math.PI)
					{
						Geom = null;
						if (result.Count > 0)
							Geom = ARANFunctions.LineLineIntersect(result[result.Count - 1], dirCurr, bpt[j], dirSpCenter);
						else
						{
							Point ptFom = TurnDir == TurnDirection.CCW ? ptFroml : ptFromr;

							if (ptFom != null)
								Geom = ARANFunctions.LineLineIntersect(ptFom, dirCurr, bpt[j], dirSpCenter);
						}

						if (Geom != null && Geom.Type == GeometryType.Point)
							result.Add((Point)Geom);
					}
					else
					{
						//_UI.DrawPointWithText(ARANFunctions.LocalToPrj(ptCnt, SpStartRadial, TurnRCurr), "ptStartSp");
						//ProcessMessages();

						ARANFunctions.AddSpiralToRing(ptCnt, TurnRCurr, coeff, SpStartRadial,
												SpTurnAngle, TurnDir, ref result);

						//_UI.DrawRing(result, eFillStyle.sfsCross);
						//ProcessMessages();

						TurnRCurr = TurnRCurr + coeff * SpTurnAngle;
						SpStartRadial = SpStartRadial - SpTurnAngle * fTurnSide;
					}
					dirCurr = dirRef;
				}
				ptCurr = result[result.Count - 1];
			}
			else
			{
				//ptCnt = ARANFunctions.LocalToPrj(BasePoints[0], dirSpCenter, TurnR + dRad, 0);
				ptCnt = ARANFunctions.LocalToPrj(bpt[0], dirSpCenter, TurnR, 0);
				ptCurr = bpt[0];
				result.Add(ptCurr);
			}


			//	ptTmp = ARANFunctions.LocalToPrj(ptRef, OutDir1, -DistToEndFIX, 0);
			//	_UI.DrawPointWithText(ptTmp, 0, "ptTmp_O");

			//Ring rr = new Ring();
			//rr.Assign(result);
			//_UI.DrawRing(rr, -1, eFillStyle.sfsCross);
			//ProcessMessages();

			//_UI.DrawPointWithText(ptCnt, 255, "ptCnt");
			//_UI.DrawPointWithText(ptRef, 0, "ptRef_O");
			//ptTmp = ARANFunctions.LocalToPrj(BasePoints[1], dirSpCenter, TurnR + dRad, 0);
			//_UI.DrawPointWithText(ptTmp, 0, "ptCnt_1");

			//_UI.DrawPointWithText(ptCurr, 0, "ptCurr");
			//ProcessMessages();

			//LineString lst = new LineString();
			//lst.Add(ptRef);
			//lst.Add(ARANFunctions.LocalToPrj(ptRef, OutDir1, -30000, 0));
			//_UI.DrawLineString(lst, 255, 2);
			//_UI.DrawRing(result, -1, eFillStyle.sfsCross);
			//ProcessMessages();

			bool refFlg = true;
			bool bFlag;
			ptRef = ARANFunctions.LocalToPrj(_EndFIX.PrjPt, OutDir1, 0, ASW_OUT1l * fTurnSide);

			//Ring rr = new Ring();
			//rr.Assign(result); 
			//_UI.DrawRing(rr, -1, eFillStyle.sfsCross);
			//ProcessMessages();

			//ptTmp = ARANFunctions.PrjToLocal(ptRef, OutDir1, ptCurr);
			//if (ptTmp.Y * fTurnSide < 0)

			if (continueMode == 1)
			{
				Geom = ARANFunctions.LineLineIntersect(ptCurr, dirDst15, ptRef, OutDir1);

				//_UI.DrawPointWithText((Point)Geom, -1, "ptX-2");
				//ProcessMessages();

				if (Geom.Type == GeometryType.Point)
				{
					ptTmp = ARANFunctions.PrjToLocal(_EndFIX.PrjPt, OutDir1, (Point)Geom);
					if (ptTmp.X > 0)
					{
						Geom = ARANFunctions.LineLineIntersect(ptCurr, dirDst15, ptRef, OutDir1 + ARANMath.C_PI_2);
						refFlg = false;
					}

					//_UI.DrawPointWithText((Point)Geom, -1, "ptX-1");
					//ProcessMessages();

					if (Geom.Type == GeometryType.Point)
					{
						ptTmp = (Point)Geom;

						//LineString lst = new LineString();
						//lst.Add(ARANFunctions.LocalToPrj(ptTmp, OutDir1, 30000, 0));
						//lst.Add(ARANFunctions.LocalToPrj(ptTmp, OutDir1, -30000, 0));
						//_UI.DrawLineString(lst, -1, 2);
						//_UI.DrawPointWithText(ptTmp, -1, "ptTmp");
						//_UI.DrawPointWithText(ptRef, -1, "ptRef");
						//ProcessMessages();

						//if (_StartFIX.FlyMode == eFlyMode.Atheight || ARANMath.SideDef(ptTmp, OutDir1, ptRef) == (SideDirection)TurnDir)
						result.Add(ptTmp);
						//else
						//	refFlg = true;
						//result.Add(ptRef);
					}
				}

				//_UI.DrawPointWithText((Point)Geom, -1, "ptX-1");
				//_UI.DrawPointWithText(ptRef, -1, "ptRef");
				//ProcessMessages();

				//LineString lst = new LineString();
				//lst.Add(ptRef);
				//lst.Add(ARANFunctions.LocalToPrj(ptRef, OutDir1, -30000, 0));
				//_UI.DrawLineString(lst, -1, 2);
				//ProcessMessages();

				//lst.Clear();
				//lst.Add(ptCurr);
				//lst.Add(ARANFunctions.LocalToPrj(ptCurr, dirDst15, 30000, 0));
				//_UI.DrawLineString(lst, -1, 2);
				//ProcessMessages();

				Geom = null;

				//Ring rr = new Ring();
				//rr.Assign(result);
				//_UI.DrawRing(rr, -1, eFillStyle.sfsCross);
				//ProcessMessages();
			}
			else
			{
				if (ARANMath.Modulus((dirRef - dirDst30) * fTurnSide, ARANMath.C_2xPI) > Math.PI)
				{
					SpTurnAngle = ARANFunctions.SpiralTouchAngle(TurnRCurr, coeff, SpStartRadial, dirCurr, dirRef, TurnDir);

					ARANFunctions.AddSpiralToRing(ptCnt, TurnRCurr, coeff, SpStartRadial,
											SpTurnAngle, TurnDir, ref result);
					dirCurr = dirRef;
					TurnRCurr += coeff * SpTurnAngle;
					SpStartRadial -= SpTurnAngle * fTurnSide;
				}

				//Ring rr = new Ring();
				//rr.Assign(result);
				//_UI.DrawRing(rr, -1, eFillStyle.sfsCross);
				//ProcessMessages();

				SpTurnAngle = ARANFunctions.SpiralTouchAngle(TurnRCurr, coeff, SpStartRadial, dirCurr, dirDst30, TurnDir);
				if (SpTurnAngle >= 0)
				{
					bFlag = false;
					int m = (int)Math.Round(ARANMath.RadToDeg(SpTurnAngle));

					if (m > 0)
					{
						if (m < 5) m = 5;
						else if (m < 10) m = 10;

						double dAlpha = SpTurnAngle / m, PrevX = 0.0, PrevY = 0.0;

						for (i = 0; i < m; i++)
						{
							double R = TurnRCurr + i * dAlpha * coeff;
							ptCurr = ARANFunctions.LocalToPrj(ptCnt, SpStartRadial - i * dAlpha * fTurnSide, R, 0);
							//_UI.DrawPointWithText(ptCurr, 255, "ptCurr" + i);
							//ProcessMessages();

							ptTmp = ARANFunctions.PrjToLocal(ptRef, OutDir1, ptCurr);

							if (i > 0 && ((ptTmp.Y * fTurnSide <= 0 && PrevY * fTurnSide > 0) || ptTmp.X >= 0))
							{
								refFlg = ptTmp.X <= 0;
								double K;

								if (ptTmp.X < 0) K = -PrevY / (ptTmp.Y - PrevY);
								else K = -PrevX / (ptTmp.X - PrevX);

								ptCurr.X = result[result.Count - 1].X + K * (ptCurr.X - result[result.Count - 1].X);
								ptCurr.Y = result[result.Count - 1].Y + K * (ptCurr.Y - result[result.Count - 1].Y);

								result.Add(ptCurr);
								//_UI.DrawPointWithText(ptCurr, -1, "ptCurr" + i);
								//ProcessMessages();

								bFlag = true;
								break;
							}

							PrevX = ptTmp.X;
							PrevY = ptTmp.Y;

							//_UI.DrawPointWithText(ptCurr, -1, "ptCurr" + i);
							//ProcessMessages();

							result.Add(ptCurr);
						}
					}
					else
						ptCurr = result[result.Count - 1];

					if ((!bFlag) && refFlg)
					{
						Geom = ARANFunctions.LineLineIntersect(ptCurr, dirDst30, ptRef, OutDir1);
						if (Geom.Type == GeometryType.Point)
						{
							ptTmp = ARANFunctions.PrjToLocal(_EndFIX.PrjPt, OutDir1, (Point)Geom);
							if (ptTmp.X > 0)
							{
								Geom = ARANFunctions.LineLineIntersect(ptCurr, dirDst30, ptRef, OutDir1 + ARANMath.C_PI_2);
								refFlg = false;
							}

							if (Geom.Type == GeometryType.Point)
								result.Add((Point)Geom);
						}
						Geom = null;
					}
					//ptCurr = null;
				}
			}

			if (refFlg)
				result.Add(ptRef);

			//_UI.DrawPointWithText(ptRef, -1, "ptRef");
			//ProcessMessages();

			//Ring rr = new Ring();
			//rr.Assign(result);
			//_UI.DrawRing(rr, eFillStyle.sfsCross);
			//ProcessMessages();

			double LPT = _EndFIX.FlyMode == eFlyMode.Flyby ? _EndFIX.ConstructTAS * _EndFIX.PilotTime + _EndFIX.ATT : _EndFIX.LPT;

			//_EndFIX.OutDirection = _EndFIX.EntryDirection + ARANMath.C_PI;
			//_EndFIX.CalcTurnRangePoints();
			//LPT = Math.Abs(_EndFIX.LPT_L) > Math.Abs(_EndFIX.LPT_R) ? _EndFIX.LPT_L : _EndFIX.LPT_R;
			//LPT = (_EndFIX.FlyMode == eFlyMode.FlyBy ? -LPT : LPT);

			double DistToEndFIX = LPT + OverlapDist;

			//if (_EndFIX.FlyMode == eFlyMode.FlyBy)
			//	DistToEndFIX *= 5.0;

			int nr = result.Count;

			if (nr > 1)
			{
				//ptTmp = ARANFunctions.LocalToPrj(_EndFIX.PrjPt, OutDir1, LPT, 0);
				//_UI.DrawPointWithText(ptTmp, 128, "Fix_O");
				//_UI.DrawPointWithText(result[nr - 1], 128, "[nr - 1]");
				//ProcessMessages();

				dirCurr = ARANFunctions.ReturnAngleInRadians(result[nr - 2], result[nr - 1]);

				//ptCurr = (Point)ARANFunctions.LineLineIntersect(ptTmp, dirCurr + ARANMath.C_PI, result[nr - 1], dirCurr);
				ptCurr = ARANFunctions.LocalToPrj(result[nr - 1], dirCurr, DistToEndFIX, 0);

				//_UI.DrawPointWithText(result[nr - 1], 0, "-1");
				//_UI.DrawPointWithText(ptCurr, "ptOuter", IsPrimary ? 255 : 0);
				//_UI.DrawPointWithText(ptRef,  "ptRef_O");
				//ProcessMessages();

				result.Add(ptCurr);
			}

			//Ring rr = new Ring();
			//rr.Assign(result);
			//_UI.DrawRing(rr, eFillStyle.sfsCross);
			//ProcessMessages();

			//result.Add(_EndFIX.PrjPt);

			#endregion

			#region Inner side

			//Inner side ===================================================================================

			int iSt = 0, c30 = 0, i0 = 0;

			ptRef = ARANFunctions.LocalToPrj(_EndFIX.PrjPt, OutDir2, 0, -ASW_OUT1l * fTurnSide);

			dirDst15 = OutDir2 - SplayAngle15 * fTurnSide;
			dirDst30 = OutDir2 + DivergenceAngle30 * fTurnSide;
			double dirDst = dirDst15;

			//LineString lst = new LineString();
			//lst.Add(ARANFunctions.LocalToPrj(ptRef, OutDir2, 30000, 0));
			//lst.Add(ARANFunctions.LocalToPrj(ptRef, OutDir2, -30000, 0));
			//_UI.DrawLineString(lst, -1, 2);
			//_UI.DrawPointWithText(ptRef, -1, "ptRef");
			////_UI.DrawRing(result, -1, eFillStyle.sfsCross);
			//ProcessMessages();

			for (i = 0; i < n; i++)
			{
				//_UI.DrawPointWithText(bpt[i], "bpt[" + i + "]");
				//ProcessMessages();

				bFlag = false;
				ptCurr = bpt[i];
				ptTmp = ARANFunctions.PrjToLocal(ptRef, OutDir2, ptCurr);

				if (ptTmp.Y * fTurnSide > 0)
					dirDst = dirDst15;
				else
				{
					dirDst = dirDst30;
					iSt = i;
					c30++;
				}

				for (j = 0; j < n; j++)
					if (j != i)
					{
						//LineString lst = new LineString();
						//lst.Add(ptCurr);
						//lst.Add(ARANFunctions.LocalToPrj(ptCurr, dirDst, -30000, 0));
						//_UI.DrawLineString(lst, -1, 2);
						//ProcessMessages();

						ptCnt = ARANFunctions.PrjToLocal(ptCurr, dirDst, bpt[j]);
						if (ptCnt.Y * fTurnSide < 0)
						{
							bFlag = true;
							break;
						}
					}

				//LineString lst = new LineString();
				//lst.Add(ptCurr);
				//lst.Add(ARANFunctions.LocalToPrj(ptCurr, dirDst, 8000, 0));
				//_UI.DrawLineString(lst, 2);
				//ProcessMessages();

				if (!bFlag)
				{
					i0 = i;
					break;
				}
			}

			//if (true)
			//{

			ptCnt = ARANFunctions.LocalToPrj(ptRef, OutDir2, DistToEndFIX, 0);

			//_UI.DrawPointWithText(bpt[i0], "bpt[" + i0 + "]");
			//_UI.DrawPointWithText(ptRef, "ptRef-I");
			//_UI.DrawPointWithText(ptCnt, "ptCnt-ptInner", IsPrimary ? 255 : 0);
			//_UI.DrawPointWithText(ptCurr, "ptCurr");
			//ProcessMessages();

			//fTmp = ARANFunctions.Point2LineDistancePrj(ptRef, ptCnt, OutDir2);
			//fTmp = ARANFunctions.ReturnDistanceInMeters (ptRef, ptCnt);
			//result.Add(ptCnt);
			//}

			if (c30 == 1)   //if (bFlag )
			{
				ptCurr = bpt[iSt];
				dirDst = dirDst30;
			}

			//ProcessMessages(true);

			//LineString lst = new LineString();
			//lst.Add(ptCnt);
			//lst.Add(ARANFunctions.LocalToPrj(ptCurr, dirDst, 25000, 0));
			//_UI.DrawLineString(lst, 0, 2);
			//ProcessMessages();

			//if (ARANMath.SideDef(ptCnt, OutDir2 + ARANMath.C_PI_2, ptRef) > 0)
			//	ptTmp = ARANFunctions.PrjToLocal(ptCnt, OutDir2, ptCurr);
			//else
			//	ptTmp = ARANFunctions.PrjToLocal(ptRef, OutDir2, ptCurr);

			//LineString lst = new LineString();
			//lst.Add(ptCnt);
			//lst.Add(ARANFunctions.LocalToPrj(ptCnt, OutDir2, -25000, 0));
			//_UI.DrawLineString(lst, 0, 2);
			//_UI.DrawPointWithText(ptCurr, -1, "Curr");
			//ProcessMessages();

			ptTmp = ARANFunctions.PrjToLocal(ptCnt, OutDir2, ptCurr);

			//result.Add(ptRef);
			//_UI.DrawPointWithText(ptRef, -1, "ptRef-I");
			//_UI.DrawPointWithText(result[result.Count - 1], -1, "- 1");
			//ProcessMessages();

			if (Math.Abs(ptTmp.Y) > ARANMath.EpsilonDistance)
			{
				Geom = ARANFunctions.LineLineIntersect(ptCurr, dirDst, ptCnt, OutDir2);

				if (Geom.Type == GeometryType.Point)
				{
					//_UI.DrawPointWithText((Point)Geom, "X-2");
					//_UI.DrawPointWithText(ptCnt, "ptCnt");
					//ProcessMessages();

					ptTmp = ARANFunctions.PrjToLocal(_EndFIX.PrjPt, OutDir2, (Point)Geom);

					if (ptTmp.X > DistToEndFIX)
					{
						//result.Add((Point)Geom);
						Geom = ARANFunctions.LineLineIntersect(ptCurr, dirDst, ptCnt, OutDir2 + ARANMath.C_PI_2);
						//_UI.DrawPointWithText((Point)Geom, -1, "X-1");
						//ProcessMessages();
					}
					else
						result.Add(ptCnt);

					//fTmp = ARANFunctions.ReturnDistanceInMeters((Point)Geom, ptCnt);

					if (Geom.Type == GeometryType.Point)
						result.Add((Point)Geom);

					//Ring rr = new Ring();
					//rr.Assign(result);
					//_UI.DrawRing(rr, -1, eFillStyle.sfsCross);
					//ProcessMessages();

					//LineString lst = new LineString();
					//lst.Add(ptCnt);
					//lst.Add(ARANFunctions.LocalToPrj(ptCurr, dirDst, 25000, 0));
					//_UI.DrawLineString(lst, 0, 2);

					//_UI.DrawPointWithText(ptTmp, -1, "ptTmp");
					//_UI.DrawPointWithText((Point)Geom, -1, "X-2");
					//ProcessMessages();

					//_UI.DrawPointWithText(ptCurr, -1, "ptCurr");
					//ProcessMessages();

					double fDist = ARANFunctions.ReturnDistanceInMeters(result[result.Count - 1], ptCurr);

					ptCnt = ARANFunctions.RingVectorIntersect(result, ptCurr, dirDst, out fTmp, true);
					if (ptCnt != null)
						fTmp = ARANFunctions.ReturnDistanceInMeters(result[result.Count - 1], ptCnt);
					else
						fTmp = -1.0;

					//_UI.DrawPointWithText(ptCnt, "pt X");
					//ProcessMessages();

					if (fDist > ARANMath.EpsilonDistance && fTmp > fDist)
					{
						i = 0;
						while (i < result.Count)
						{
							ptTmp = ARANFunctions.PrjToLocal(ptCurr, dirDst, result[i]);
							if (Math.Sign(ptTmp.Y) != (int)TurnDir)
								result.Remove(i);
							else
								break;
						}

						result.Add(ptCnt);
					}
					else
					{
						//_UI.DrawPointWithText(ptCurr, "ptCurr-1");
						//ProcessMessages();

						result.Add(ptCurr);
						if (_StartFIX.FlyMode != eFlyMode.Atheight)
						{
							//ProcessMessages(true);
							////_UI.DrawPointWithText(ptFrom0l, "ptFromL0");
							//_UI.DrawPointWithText(bpt[0], "bpt[0]");
							//_UI.DrawPointWithText(bpt[1], "bpt[1]");
							//_UI.DrawPointWithText(bpt[2], "bpt[2]");
							//_UI.DrawPointWithText(bpt[3], "bpt[3]");
							//ProcessMessages();

							if (i0 == 1)
								result.Add(bpt[2]);
							if (i0 <= 2)
								result.Add(bpt[3]);
						}
						else if (i0 == 1)
						{
							//_UI.DrawPointWithText(ptFrom0l, "ptFromL0");
							//ProcessMessages();

							if (TurnDir == TurnDirection.CCW)
								result.Add(ptFrom0l);
							else
								result.Add(ptFrom0r);
						}

						//_UI.DrawPointWithText(bpt[2], "bpt[2]");
						//_UI.DrawPointWithText(bpt[3], "bpt[3]");
						//_UI.DrawPointWithText(bpt[0], "bpt[0]");
						//_UI.DrawPointWithText(bpt[1], "bpt[1]");
						//ProcessMessages();

						//if (i == 3)
						//	result.Add(bpt[0]);
					}
				}
			}
			else
			{
				//_UI.DrawPointWithText(ptCurr, -1, "ptCurr");
				//ProcessMessages();

				result.Add(ptCnt);
				result.Add(ptCurr);
			}

			//Geom = ARANFunctions.LineLineIntersect(ptCurr, dirDst, _StartFIX.PrjPt, _StartFIX.EntryDirection);
			//result.Add((Point)Geom);

			//_UI.DrawPointWithText((Point)Geom, -1, "Geom");
			//_UI.DrawPointWithText(ptCnt, -1, "ptCurr-1");

			//ProcessMessages();

			//_UI.DrawPointWithText(ptCnt, IsPrimary ? 255 : 0, "ptInner");
			//_UI.DrawPointWithText(ptCurr, -1, "ptCurr");
			//ProcessMessages();

			//Ring rr = new Ring();
			//rr.Assign(result);
			//_UI.DrawRing(rr, eFillStyle.sfsCross);
			//ProcessMessages();

			#endregion

			return result;
		}

		virtual protected void JoinSegments(Ring LegRing, ADHPType ARP, bool IsOuter, bool IsPrimary, double EntryDir, TurnDirection TurnDir)
		{
			double OutDir = _StartFIX.OutDirection;     //ProcessMessages(true);
			double LegLenght = ARANMath.Hypot(_EndFIX.PrjPt.X - _StartFIX.PrjPt.X, _EndFIX.PrjPt.Y - _StartFIX.PrjPt.Y);

			double SpiralDivergenceAngle, SpiralSplayAngle,
				DivergenceAngle30 = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value,
				SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;

			if (IsPrimary && LegRing.Count < 10)
			//if (IsPrimary && _StartFIX.TurnAngle < ARANMath.DegToRad(60))
			{
				SpiralDivergenceAngle = Math.Atan(0.5 * Math.Tan(DivergenceAngle30));
				SpiralSplayAngle = Math.Atan(0.5 * Math.Tan(SplayAngle15));
				//SpiralDivergenceAngle = DivergenceAngle30;
				//SpiralSplayAngle = SplayAngle15;
			}
			else
			{
				SpiralDivergenceAngle = DivergenceAngle30;
				SpiralSplayAngle = SplayAngle15;
			}

			WayPoint[] wptTransitions = new WayPoint[3];
			int transitions = 0;

			WayPoint prev = _StartFIX;
			//Point ptTransition =	ARANFunctions.CircleVectorIntersect(ARP.pPtPrj, PANSOPSConstantList.PBNInternalTriggerDistance, _StartFIX.PrjPt, OutDir);
			Point ptTransition = ARANFunctions.CircleVectorIntersect(ARP.pPtPrj, PANSOPSConstantList.PBNInternalTriggerDistance, _EndFIX.PrjPt, OutDir);

			if (!ptTransition.IsEmpty)
			{
				Point ptS = ARANFunctions.PrjToLocal(ptTransition, OutDir, prev.PrjPt);
				Point ptE = ARANFunctions.PrjToLocal(ptTransition, OutDir, _EndFIX.PrjPt);

				if (ptS.X * ptE.X < 0.0)
				{
					WayPoint WPT_15NM = new WayPoint(eFIXRole.TP_, _aranEnv);
					WPT_15NM.SensorType = _StartFIX.SensorType;
					WPT_15NM.PBNType = _StartFIX.PBNType;
					WPT_15NM.FlightPhase = _StartFIX.FlightPhase <= eFlightPhase.SID ? eFlightPhase.SIDGE28 : eFlightPhase.MApGE28;

					WPT_15NM.EntryDirection = OutDir;
					WPT_15NM.OutDirection = OutDir;
					WPT_15NM.BankAngle = _EndFIX.BankAngle;

					WPT_15NM.PrjPt = ARANFunctions.LocalToPrj(ptTransition, OutDir, -WPT_15NM.ATT, 0);

					wptTransitions[transitions] = WPT_15NM;

					_EndFIX.FlightPhase = WPT_15NM.FlightPhase;
					_EndFIX.BankAngle = WPT_15NM.BankAngle;

					transitions++;
				}
			}

			//ptTransition = ARANFunctions.CircleVectorIntersect(ARP.pPtPrj, PANSOPSConstantList.PBNTerminalTriggerDistance, _StartFIX.PrjPt, OutDir);
			ptTransition = ARANFunctions.CircleVectorIntersect(ARP.pPtPrj, PANSOPSConstantList.PBNTerminalTriggerDistance, _EndFIX.PrjPt, OutDir);

			if (!ptTransition.IsEmpty)
			{
				Point ptS = ARANFunctions.PrjToLocal(ptTransition, OutDir, prev.PrjPt);
				Point ptE = ARANFunctions.PrjToLocal(ptTransition, OutDir, _EndFIX.PrjPt);

				if (ptS.X * ptE.X < 0.0)
				{
					WayPoint WPT_30NM = new WayPoint(eFIXRole.TP_, _aranEnv);
					WPT_30NM.FlightPhase = _StartFIX.FlightPhase <= eFlightPhase.SID ? eFlightPhase.SIDGE56 : eFlightPhase.STARGE56;
					WPT_30NM.SensorType = _StartFIX.SensorType;

					if (_StartFIX.FlightPhase >= eFlightPhase.FAFApch && _StartFIX.PBNType == ePBNClass.RNP_APCH)
					{
						WPT_30NM.PBNType = ePBNClass.RNAV1;
						_EndFIX.PBNType = ePBNClass.RNAV1;
					}

					WPT_30NM.EntryDirection = OutDir;
					WPT_30NM.OutDirection = OutDir;
					WPT_30NM.BankAngle = _EndFIX.BankAngle;

					WPT_30NM.PrjPt = ARANFunctions.LocalToPrj(ptTransition, OutDir, -WPT_30NM.ATT, 0);

					wptTransitions[transitions] = WPT_30NM;

					_EndFIX.FlightPhase = WPT_30NM.FlightPhase;
					_EndFIX.BankAngle = WPT_30NM.BankAngle;

					transitions++;
				}
			}

			wptTransitions[transitions] = (WayPoint)_EndFIX.Clone();
			transitions++;

			//=================================================================================
			double fSide = IsOuter ? -((int)TurnDir) : ((int)TurnDir);
			//double fSide = -(int)TurnDir;

			bool expansAtTransition = true;
			bool normalWidth = (ARANMath.RadToDeg(_StartFIX.TurnAngle) <= 10 ||
				(_StartFIX.FlyMode == eFlyMode.Flyby && _StartFIX.Role != eFIXRole.FAF_ && ARANMath.RadToDeg(_StartFIX.TurnAngle) <= 30));

			WayPoint lStartFIX = new WayPoint(_aranEnv);
			WayPoint lEndFIX = new WayPoint(_aranEnv);

			lStartFIX.Assign(_StartFIX);
			lEndFIX.Assign(_EndFIX);
			Point ptCurr;

			for (int pass = 0; pass < transitions; pass++, lStartFIX = lEndFIX)
			{
				lEndFIX = wptTransitions[pass];
				lEndFIX.CalcTurnRangePoints();

				double ASW_1C = IsPrimary ? 0.5 * lEndFIX.SemiWidth : lEndFIX.SemiWidth;
				double ASW_0C = IsPrimary ? 0.5 * lStartFIX.SemiWidth : lStartFIX.SemiWidth;

				//_UI.DrawPointWithText(lEndFIX.PrjPt, -1, "lEnd");
				//ProcessMessages();

				double DistToEndFIX = lEndFIX.FlyMode == eFlyMode.Flyby ? lEndFIX.LPT - OverlapDist : OverlapDist;

				if (pass == transitions - 1)
				{
					//double fTmp = (int)lEndFIX.EffectiveTurnDirection * ARANMath.C_PI * 1.333333333333333333;
					//lEndFIX.OutDirection = 0.0;// lEndFIX.EntryDirection + fTmp;

					DistToEndFIX = (lEndFIX.FlyMode == eFlyMode.Flyby ? lEndFIX.LPT : -lEndFIX.LPT) - OverlapDist;
					//DistToEndFIX = (lEndFIX.FlyMode == eFlyMode.FlyBy ? -lEndFIX.LPT : lEndFIX.LPT) - OverlapDist;
					//if (DistToEndFIX > lEndFIX.ATT) DistToEndFIX = lEndFIX.ATT;
					if (DistToEndFIX > 0.0) DistToEndFIX = 0.0;

				}

				Point ptEndOfLeg = ARANFunctions.LocalToPrj(lEndFIX.PrjPt, OutDir + ARANMath.C_PI, DistToEndFIX);

				//=================================================================================

				ptCurr = (Point)LegRing[LegRing.Count - 1].Clone();

				//Point ptEndOfLeg;
				//if(expansAtTransition)
				//    ptEndOfLeg = lEndFIX.PrjPt;
				//else
				//ptEndOfLeg = ARANFunctions.LocalToPrj(lEndFIX.PrjPt, OutDir + ARANMath.C_PI, DistToEndFIX);

				//_UI.DrawPointWithText(ptCurr, "ptCurr");
				//_UI.DrawPointWithText(ptEndOfLeg, "ptEndOfLeg");
				//ProcessMessages();

				double DistToEndOfLeg = ARANFunctions.PointToLineDistance(ptCurr, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
				double TransitionDist = 0.0;

				if (!expansAtTransition)
					TransitionDist = DistToEndOfLeg + DistToEndFIX;

				if (DistToEndOfLeg > ARANMath.EpsilonDistance)
				{
					Point ptBase0, ptBase1;
					double BaseDir0, BaseDir1;

					if (lStartFIX.Role == eFIXRole.FAF_ && lEndFIX.Role == eFIXRole.MAPt_)
					{
						double dPhi1 = Math.Atan2(ASW_0C - ASW_1C, LegLenght);
						if (dPhi1 > SpiralDivergenceAngle) dPhi1 = SpiralDivergenceAngle;

						BaseDir0 = OutDir + dPhi1 * fSide;
						ptBase0 = ARANFunctions.LocalToPrj(lEndFIX.PrjPt, OutDir + ARANMath.C_PI_2 * fSide, ASW_0C);

						BaseDir1 = OutDir + dPhi1 * fSide;
						ptBase1 = ARANFunctions.LocalToPrj(lEndFIX.PrjPt, OutDir + ARANMath.C_PI_2 * fSide, ASW_1C);
					}
					else
					{
						BaseDir0 = OutDir;
						ptBase0 = ARANFunctions.LocalToPrj(ptEndOfLeg, OutDir + ARANMath.C_PI_2 * fSide, ASW_1C);
						//ptBase0 = ARANFunctions.LocalToPrj(ptEndOfLeg, OutDir + ARANMath.C_PI_2 * fSide, ASW_0C, 0);

						BaseDir1 = OutDir;
						ptBase1 = ptBase0;// ARANFunctions.LocalToPrj(ptEndOfLeg, OutDir + ARANMath.C_PI_2 * fSide, ASW_1C);
					}

					//_UI.DrawPointWithText(ptBase0, "ptBase0");
					//_UI.DrawPointWithText(ptBase1, "ptBase1");
					//ProcessMessages();

					double ASW_0F = fSide * ARANFunctions.PointToLineDistance(ptCurr, ptEndOfLeg, OutDir); //Abs ????????
					double Direction;

					if ((IsOuter || normalWidth) && Math.Abs(ASW_0F - ASW_0C) > ARANMath.EpsilonDistance)
					{
						Point ptTmp = ARANFunctions.LocalToPrj(ptEndOfLeg, OutDir + ARANMath.C_PI, TransitionDist, -fSide * ASW_0C);
						//		ptTmp = ARANFunctions.LocalToPrj(ptEndOfLeg, OutDir, 0, fSide * ASW_0C);
						//_UI.DrawPointWithText(ptTmp, "ptTmp");
						//ProcessMessages();

						if (ASW_0F > ASW_0C)
							Direction = OutDir - SpiralDivergenceAngle * fSide;
						else
							Direction = OutDir + SpiralSplayAngle * fSide;

						Point ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, Direction, ptTmp, OutDir);
						DistToEndOfLeg = ARANFunctions.PointToLineDistance(ptInter, ptEndOfLeg, OutDir + ARANMath.C_PI_2);

						//Geometry pGeom = ARANFunctions.LineLineIntersect(ptCurr, BaseDir0, ptTmp, OutDir);
						//if (pGeom.Type != GeometryType.Point)
						//	ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, BaseDir0, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
						//else
						//	ptInter = (Point)pGeom;

						//_UI.DrawPointWithText(ptInter, "ptInter-1");
						//ProcessMessages();


						double Dist0 = ARANFunctions.PointToLineDistance(ptInter, ptCurr, OutDir - ARANMath.C_PI_2);

						//Ring rr = new Ring();
						//rr.Assign(LegRing);
						//_UI.DrawRing(rr, eFillStyle.sfsHorizontal);


						//_UI.DrawPointWithText(ptTmp, "ptTmp");
						//_UI.DrawPointWithText(ptCurr, "ptCurr-2");
						//_UI.DrawPointWithText(ptInter, "ptInter-1");
						//ProcessMessages();

						//LineString ls = new LineString();
						//ls.Add(ptTmp);
						//ls.Add(ARANFunctions.LocalToPrj(ptTmp, OutDir, -30000, 0));
						//_UI.DrawLineString(ls, 2,0);
						//ProcessMessages();

						if (DistToEndOfLeg < 0.0 || Dist0 < 0.0) // || ARANFunctions.PointToLineDistance(ptInter, lEndFIX.PrjPt, OutDir - ARANMath.C_PI_2) > 0.0
						{
							if (pass == transitions - 1)
							{
								ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, Direction, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
								//_UI.DrawPointWithText(ptInter, -1, "ptInter-2");
								//ProcessMessages();
							}
							else
							{
								ptTmp = ARANFunctions.LocalToPrj(ptEndOfLeg, OutDir + ARANMath.C_PI, TransitionDist, -fSide * ASW_1C);
								ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, Direction, ptTmp, OutDir);
							}

							DistToEndOfLeg = ARANFunctions.PointToLineDistance(ptInter, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
						}

						//_UI.DrawPointWithText(ptCurr, "ptCurr-2");
						//_UI.DrawPointWithText(ptInter, "ptInter");
						//_UI.DrawPointWithText(lEndFIX.PrjPt, "EndF");
						//ProcessMessages();

						//ptTmp = ARANFunctions.PrjToLocal(lEndFIX.PrjPt, OutDir, ptInter);
						//if (ptTmp.X <= 0.0)
						//{
						ptCurr = ptInter;
						LegRing.Add(ptCurr);
						//}

						ASW_0F = fSide * ARANFunctions.PointToLineDistance(ptCurr, ptEndOfLeg, OutDir);

						//_UI.DrawPointWithText(ptTmp, "ptTmp");
						//_UI.DrawPointWithText(ptInter, "ptInter");

						//Ring rr = new Ring();
						//rr.Assign(LegRing);
						//_UI.DrawRing(rr, -1, eFillStyle.sfsHorizontal);
						//ProcessMessages();
					}

					normalWidth = true;

					if (IsPrimary)
					{
						SpiralDivergenceAngle = Math.Atan(0.5 * Math.Tan(DivergenceAngle30));
						SpiralSplayAngle = Math.Atan(0.5 * Math.Tan(SplayAngle15));
					}

					//	LineString ls = new LineString();
					//	ls.Add(ptCurr);
					//	ls.Add(ARANFunctions.LocalToPrj(ptCurr, BaseDir1, 10000, 0));
					//	_UI.DrawLineString(ls, 255, 2);
					//	ProcessMessages();

					//	_UI.DrawPointWithText(ARANFunctions.LocalToPrj(ptCurr, BaseDir1, 10000, 0), 255, "BaseDir");
					//	_UI.DrawPointWithText(ptBase1, 255, "ptBase1");
					//	ProcessMessages();

					double Delta = fSide * ARANFunctions.PointToLineDistance(ptCurr, ptBase1, BaseDir1);

					if (DistToEndOfLeg > ARANMath.EpsilonDistance && Math.Abs(Delta) > ARANMath.EpsilonDistance)
					{
						if (TransitionDist > 0 || expansAtTransition)
						{
							if (TransitionDist < DistToEndFIX) TransitionDist = DistToEndFIX;

							if (TransitionDist < DistToEndOfLeg)
							{
								Point ptTmp = ARANFunctions.LocalToPrj(lEndFIX.PrjPt, OutDir - ARANMath.C_PI, TransitionDist, -fSide * ASW_0F);
								//Point ptTmp = ARANFunctions.LocalToPrj(lEndFIX.PrjPt, OutDir, -TransitionDist, fSide * ASW_0F);

								////ProcessMessages(true);
								//_UI.DrawPointWithText(ptTmp, "ptCurr-3");
								//_UI.DrawPointWithText(ptCurr, "ptCurr-2");
								//_UI.DrawPointWithText(ptEndOfLeg, "ptEndOfLeg");
								//_UI.DrawPointWithText(ptBase1, "ptBase1");
								//_UI.DrawPointWithText(lEndFIX.PrjPt, "lEndFIX");
								//ProcessMessages();

								double Dist0 = ARANFunctions.PointToLineDistance(ptTmp, ptCurr, OutDir - ARANMath.C_PI_2);

								if (Dist0 < -ARANMath.EpsilonDistance)
								{
									double tmpDir = OutDir;
									if (lStartFIX.SensorType == eSensorType.GNSS)
									{
										if (Delta > 0.0)
										{
											if (ASW_0C > ASW_1C)
												tmpDir = OutDir - SpiralDivergenceAngle * fSide;
											else
												tmpDir = OutDir - DivergenceAngle30 * fSide;
										}
										else
										{
											if (ASW_0C > ASW_1C)
												tmpDir = OutDir + SplayAngle15 * fSide;
											else
												tmpDir = OutDir + SpiralSplayAngle * fSide;
										}
									}

									double x1, y1, k1 = -Math.Sign(Delta);
									ARANFunctions.PrjToLocal(ptTmp, tmpDir, ptCurr, out x1, out y1);

									if (k1 * y1 > k1 * ARANMath.EpsilonDistance && x1 > 0.0)//ARANMath.EpsilonDistance
									{
										double x0 = 0.0, y0 = 0.0;

										while (k1 * y1 > 0.0 && x1 > 0.0 && LegRing.Count > 1)
										{
											x0 = x1; y0 = y1;

											LegRing.Remove(LegRing.Count - 1);
											ptCurr = LegRing[LegRing.Count - 1];
											ARANFunctions.PrjToLocal(ptTmp, tmpDir, ptCurr, out x1, out y1);
										}

										if (Math.Abs(y1 - y0) > ARANMath.Epsilon_2Distance)
										{
											double K = -y1 / (y1 - y0);
											ptCurr = ARANFunctions.LocalToPrj(ptTmp, tmpDir, x1 + K * (x1 - x0));
										}
										else
											ptCurr = ptTmp;
									}
									else
										ptCurr = ptTmp;
								}
								else
									ptCurr = ptTmp;

								LegRing.Add(ptCurr);
								DistToEndOfLeg = ARANFunctions.PointToLineDistance(ptCurr, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
							}
						}

						if (DistToEndOfLeg > ARANMath.EpsilonDistance)
						{
							//fSide = -fSide;
							if (Delta > ARANMath.EpsilonDistance)
								Direction = OutDir - SpiralDivergenceAngle * fSide;
							else
								Direction = OutDir + SpiralSplayAngle * fSide;

							Point ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, Direction, ptBase1, BaseDir1);
							DistToEndOfLeg = ARANFunctions.PointToLineDistance(ptInter, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
							double Dist0 = ARANFunctions.PointToLineDistance(ptInter, ptCurr, OutDir - ARANMath.C_PI_2);

							//_UI.DrawPointWithText(ptInter, "ptInter-2");
							//ProcessMessages();
							if (DistToEndOfLeg < -ARANMath.EpsilonDistance && pass == transitions - 1)   //if (DistToCenter < 0 && Dist0 < 0)	//
							{
								ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, Direction, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
								DistToEndOfLeg = ARANFunctions.PointToLineDistance(ptInter, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
							}

							ptCurr = ptInter;

							//_UI.DrawPointWithText(ptCurr, "ptCurr-4");
							//ProcessMessages();

							LegRing.Add(ptCurr);
						}
					}

					//if (IsPrimary && (Math.Abs(ASW_0F - ASW_0C) < ARANMath.EpsilonDistance))
					//{
					//	SpiralDivergenceAngle = Math.Atan(0.5 * Math.Tan(DivergenceAngle30));
					//	SpiralSplayAngle = Math.Atan(0.5 * Math.Tan(SplayAngle15));
					//}

					if (DistToEndOfLeg > ARANMath.EpsilonDistance)
					{
						ptCurr = ARANFunctions.LocalToPrj(ptEndOfLeg, OutDir, 0, ASW_1C * fSide);
						//_UI.DrawPointWithText(ptCurr, "ptCurr-5", 255);
						//ProcessMessages();
						LegRing.Add(ptCurr);
					}
				}
			}

			//double DistToFIX = (lEndFIX.FlyMode == eFlyMode.Flyby ? _EndFIX.LPT : -_EndFIX.LPT) - OverlapDist;
			//lEndFIX.CalcTurnRangePoints();


			//====================================================================================================
			/////double lLPT = (_EndFIX.FlyMode == eFlyMode.Flyby ? -_EndFIX.LPT : _EndFIX.LPT) - OverlapDist;
			double DistToFIX = (_EndFIX.FlyMode == eFlyMode.Flyby ? -_EndFIX.LPT : _EndFIX.LPT) - OverlapDist;
			//if (DistToFIX < _EndFIX.ATT)	DistToFIX = _EndFIX.ATT;
			if (DistToFIX < 0) DistToFIX = 0;
			Point ptCnt = ARANFunctions.LocalToPrj(_EndFIX.PrjPt, OutDir, DistToFIX);

			//foreach(var pt in _EndFIX.BasePoints)
			//	_UI.DrawPointWithText(pt, "ptBase");
			//ProcessMessages();

			////Point ptTmp0 = ARANFunctions.LocalToPrj(_EndFIX.PrjPt, _EndFIX.EntryDirection, lLPT, 0);

			//_UI.DrawPointWithText(ptCnt, "ptCnt");
			//ProcessMessages();

			//if (lEndFIX.FlyMode != eFlyMode.Flyby)	DistToFIX = -lEndFIX.LPT - OverlapDist;
			//Point ptEndOfLeg = ARANFunctions.LocalToPrj(lEndFIX.PrjPt, OutDir + ARANMath.C_PI, DistToFIX, 0);

			//_UI.DrawMultiPolygon(_EndFIX. );

			ptCurr = LegRing[LegRing.Count - 1];
			double Dist1 = ARANFunctions.PointToLineDistance(ptCurr, ptCnt, OutDir + ARANMath.C_PI_2);

			if (Dist1 < -ARANMath.EpsilonDistance)
			{
				while (Dist1 < -ARANMath.EpsilonDistance && LegRing.Count > 1)
				{
					Point ptPrev = LegRing[LegRing.Count - 2];

					double Dist2 = ARANFunctions.PointToLineDistance(ptPrev, ptCnt, OutDir + ARANMath.C_PI_2);
					if (Dist2 < 0.0)
					{
						LegRing.Remove(LegRing.Count - 1);

						ptCurr = ptPrev;
						Dist1 = Dist2;
						continue;
					}

					double k = Dist2 / (Dist2 - Dist1);
					ptCurr.X = ptPrev.X + k * (ptCurr.X - ptPrev.X);
					ptCurr.Y = ptPrev.Y + k * (ptCurr.Y - ptPrev.Y);

					LegRing.Add(ptCurr);
					break;
				}
			}

			//Ring rr = new Ring();
			//rr.Assign(LegRing);
			//_UI.DrawRing(rr, -1, eFillStyle.sfsHorizontal);
			//_UI.DrawPointWithText(ptCnt, "ptCnt");
			//ProcessMessages();
		}

		virtual protected Ring CreateOuterTurnArea(LegBase PrevLeg, ADHPType ARP, bool IsPrimary, double EntryDir, TurnDirection TurnDir)
		{
			int i, n;//, t = 0;

			double R, K, dAlpha, AztEnd1, AztEnd2, SpAngle, fTmp, SpStartDir, SpStartRad,
				SpTurnAng, SpFromAngle, SpToAngle, dPhi1, SplayAngle, CurWidth, CurDist,
				Dist0, Dist3700, ptInterDist, dRad, SpAbeamDist, PrevX, PrevY, BulgeAngle,
				BaseDir, Delta, ASW_OUT0C, ASW_OUT0F, ASW_OUTMax, ASW_OUT1;

			Point OuterBasePoint, InnerBasePoint, ptTmp, ptInter, ptBase, ptFrom,
				ptCut, ptCnt, ptCurr, ptCurr1;

			bool bFlag, IsMAPt, HaveSecondSP;

			double fSide = (int)TurnDir;
			double OutDir = _StartFIX.OutDirection;
			double TurnAng = ARANMath.Modulus((OutDir - EntryDir) * fSide, ARANMath.C_2xPI);

			if (TurnAng < ARANMath.EpsilonRadian || Math.Abs(TurnAng - ARANMath.C_2xPI) < ARANMath.EpsilonRadian)
				return CreateInnerTurnArea(PrevLeg, ARP, IsPrimary, EntryDir, ARANMath.InversDirection(TurnDir));             //TurnAng = 0.0;

			eFlyMode FlyMode = _StartFIX.FlyMode;

			//double EntryDir = _StartFIX.EntryDirection;
			double SpiralDivergenceAngle, SpiralSplayAngle,
				DivergenceAngle30 = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value,
				SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;

			if (IsPrimary)// && TurnAng < ARANMath.DegToRad(60))
			{
				SpiralDivergenceAngle = Math.Atan(0.5 * Math.Tan(DivergenceAngle30));
				SpiralSplayAngle = Math.Atan(0.5 * Math.Tan(SplayAngle15));
				//SpiralDivergenceAngle = DivergenceAngle30;
				//SpiralSplayAngle = SplayAngle15;
			}
			else
			{
				SpiralDivergenceAngle = DivergenceAngle30;
				SpiralSplayAngle = SplayAngle15;
			}

			//TurnDirection TurnDir = ARANMath.SideToTurn(ARANMath.SideDef(_StartFIX.PrjPt, EntryDir, _EndFIX.PrjPt));

			double TurnR = _StartFIX.ConstructTurnRadius;   // ARANMath.BankToRadius(_StartFIX.BankAngle, _StartFIX.ConstructTAS);

			double Rv = 1765.27777777777777777 * Math.Tan(_StartFIX.BankAngle) / (ARANMath.C_PI * _StartFIX.ConstructTAS);
			if (Rv > 3.0)
				Rv = 3.0;

			double WSpeed = _constants.Pansops[ePANSOPSData.dpWind_Speed].Value;
			double coef = WSpeed / ARANMath.DegToRad(Rv);

			//MAX DISTANCE + =================================================================

			WayPoint[] wptTransitions = new WayPoint[3];
			Point ptTransition;
			int transitions = 0;

			if ((_StartFIX.FlightPhase > eFlightPhase.SIDGE28 && _StartFIX.FlightPhase <= eFlightPhase.SID) || (_StartFIX.FlightPhase >= eFlightPhase.FAFApch && _StartFIX.FlightPhase < eFlightPhase.MApGE28))
			{
				ptTransition = ARANFunctions.CircleVectorIntersect(ARP.pPtPrj, PANSOPSConstantList.PBNInternalTriggerDistance, _StartFIX.PrjPt, OutDir);
				if (!ptTransition.IsEmpty && ARANMath.SideDef(_StartFIX.PrjPt, OutDir + ARANMath.C_PI_2, ptTransition) == SideDirection.sideRight && ARANMath.SideDef(_EndFIX.PrjPt, OutDir + ARANMath.C_PI_2, ptTransition) == SideDirection.sideLeft)
				{
					WayPoint WPT_15NM = new WayPoint(eFIXRole.TP_, _aranEnv);

					WPT_15NM.BankAngle = _EndFIX.BankAngle;
					WPT_15NM.EntryDirection = OutDir;
					WPT_15NM.OutDirection = OutDir;

					WPT_15NM.SensorType = _StartFIX.SensorType;
					WPT_15NM.PBNType = _StartFIX.PBNType;
					WPT_15NM.FlightPhase = _StartFIX.FlightPhase <= eFlightPhase.SID ? eFlightPhase.SIDGE28 : eFlightPhase.MApGE28;
					WPT_15NM.PrjPt = ARANFunctions.LocalToPrj(ptTransition, OutDir, -WPT_15NM.ATT, 0);
					wptTransitions[transitions++] = WPT_15NM;

					_EndFIX.FlightPhase = WPT_15NM.FlightPhase;
					_EndFIX.BankAngle = WPT_15NM.BankAngle;

					//_UI.DrawPointWithText(ptTransition, ARANFunctions.RGB(0, 0, 255), "ptTransition1");
				}
			}

			if ((_StartFIX.FlightPhase > eFlightPhase.SIDGE56 && _StartFIX.FlightPhase <= eFlightPhase.SID) || _StartFIX.FlightPhase < eFlightPhase.FAFApch)
			{
				ptTransition = ARANFunctions.CircleVectorIntersect(ARP.pPtPrj, PANSOPSConstantList.PBNTerminalTriggerDistance, _StartFIX.PrjPt, OutDir);
				//_UI.DrawPointWithText(ptTransition, ARANFunctions.RGB(0, 0, 255), "ptTransition-56");
				//ProcessMessages();

				if (!ptTransition.IsEmpty && ARANMath.SideDef(_StartFIX.PrjPt, OutDir + ARANMath.C_PI_2, ptTransition) == SideDirection.sideRight && ARANMath.SideDef(_EndFIX.PrjPt, OutDir + ARANMath.C_PI_2, ptTransition) == SideDirection.sideLeft)
				{
					//ptTmp = ARANFunctions.PrjToLocal(EndFIX.PrjPt, OutDir - ARANMath.C_PI, ARP.pPtPrj);
					//Dist0 = ptTmp.X - Math.Sqrt(ARANMath.Sqr(fInternalTreshold) - ARANMath.Sqr(ptTmp.Y));

					WayPoint WPT_30NM = new WayPoint(eFIXRole.TP_, _aranEnv);
					WPT_30NM.FlightPhase = _StartFIX.FlightPhase <= eFlightPhase.SID ? eFlightPhase.SIDGE56 : eFlightPhase.STARGE56;
					WPT_30NM.SensorType = _StartFIX.SensorType;

					if (_StartFIX.FlightPhase >= eFlightPhase.FAFApch && _StartFIX.PBNType == ePBNClass.RNP_APCH)
					{
						WPT_30NM.PBNType = ePBNClass.RNAV1;
						_EndFIX.PBNType = ePBNClass.RNAV1;
					}

					WPT_30NM.EntryDirection = OutDir;
					WPT_30NM.OutDirection = OutDir;
					WPT_30NM.BankAngle = _EndFIX.BankAngle;
					WPT_30NM.PrjPt = ARANFunctions.LocalToPrj(ptTransition, OutDir, -WPT_30NM.ATT, 0);
					wptTransitions[transitions++] = WPT_30NM;

					_EndFIX.FlightPhase = WPT_30NM.FlightPhase;
					_EndFIX.BankAngle = WPT_30NM.BankAngle;

					//_UI.DrawPointWithText(ptTransition, ARANFunctions.RGB(0, 0, 255), "ptTransition2");
					//if (Dist0 > TransitionDist)					TransitionDist = Dist0;
				}
			}

			wptTransitions[transitions++] = (WayPoint)_EndFIX.Clone();

			double LPTYDist = (_EndFIX.FlyMode == eFlyMode.Flyby ? _EndFIX.LPT : -_EndFIX.LPT) - OverlapDist;

			double Dist56000 = LPTYDist;
			double TransitionDist = LPTYDist;
			double fDistTreshold = LPTYDist;

			if (transitions > 1)
			{
				fDistTreshold = ARANMath.Hypot(ARP.pPtPrj.X - wptTransitions[0].PrjPt.X, ARP.pPtPrj.Y - wptTransitions[0].PrjPt.Y);
				//t |= 1;

				ptTmp = ARANFunctions.PrjToLocal(wptTransitions[0].PrjPt, OutDir - ARANMath.C_PI, ARP.pPtPrj);
				Dist56000 = ptTmp.X - Math.Sqrt(ARANMath.Sqr(fDistTreshold) - ARANMath.Sqr(ptTmp.Y));

				TransitionDist = Dist56000;
			}

			if (_StartFIX.Role == eFIXRole.IF_ && _EndFIX.Role == eFIXRole.FAF_)
			{
				Dist3700 = 1.5 * (_StartFIX.XTT - _EndFIX.XTT) / Math.Tan(_constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value);
				//Dist3700 = (StartFIX.SemiWidth - EndFIX.SemiWidth)/Tan(GPANSOPSConstants.Constant[arSecAreaCutAngl].Value);
				//GPANSOPSConstants.Constant[rnvImMinDist].Value;
				TransitionDist = Math.Max(TransitionDist, Dist3700);
			}

			double MaxDist = Math.Max(LPTYDist, TransitionDist);
			//MAX DISTANCE - =================================================================
			double x, y;
			List<Point> basePoints;// = _StartFIX.BasePoints;

			//FIX startFIX = (FIX)_StartFIX;
			if (_StartFIX is MATF)
			{
				MATF Matf = (MATF)_StartFIX;
				//MAPt mAPt = Matf.maPt;
				//startFIX = mAPt;

				if (IsPrimary)
					basePoints = ARANFunctions.GetBasePoints(Matf.PrjPt, Matf.EntryDirection, Matf.TolerArea[0], TurnDir);
				else
					basePoints = ARANFunctions.GetBasePoints(Matf.PrjPt, Matf.EntryDirection, Matf.SecondaryTolerArea[0], TurnDir);

				//_UI.DrawMultiPolygon(Matf.TolerArea, eFillStyle.sfsCross);
				//ProcessMessages();
			}
			else
			{
				basePoints = _StartFIX.BasePoints;

				if (!IsPrimary)
				{
					for (i = 0; i < basePoints.Count; i++)
					{
						Point ptt = basePoints[i];
						ARANFunctions.PrjToLocal(_StartFIX.PrjPt, _StartFIX.EntryDirection, ptt, out x, out y);
						if (y < 0)
							ptt = ARANFunctions.LocalToPrj(ptt, _StartFIX.EntryDirection, 0.0, -_StartFIX.ASW_R);
						else
							ptt = ARANFunctions.LocalToPrj(ptt, _StartFIX.EntryDirection, 0.0, _StartFIX.ASW_L);

						basePoints[i] = ptt;
					}
				}
			}

			//for (int gh = 0; gh < _StartFIX.BasePoints.Count; gh++)
			//    _UI.DrawPointWithText(basePoints[gh], "pt-" + gh);
			//ProcessMessages();

			//ptTmp = ARANFunctions.LocalToPrj(StartFIX.PrjPt, EntryDir - ARANMath.C_PI * BYTE(FlyMode = fmFlyBy), StartFIX.LPT, 0);
			double LPT;

			//_StartFIX.CalcTurnRangePoints();

			if (_StartFIX.IsDFTarget)
			{
				if (TurnDir == TurnDirection.CW)
					LPT = (_StartFIX.FlyMode == eFlyMode.Flyby ? -_StartFIX.LPT_L : _StartFIX.LPT_L);
				else
					LPT = (_StartFIX.FlyMode == eFlyMode.Flyby ? -_StartFIX.LPT_R : _StartFIX.LPT_R);
			}
			else
				LPT = (_StartFIX.FlyMode == eFlyMode.Flyby ? -_StartFIX.LPT : _StartFIX.LPT);

			//if (startFIX.IsDFTarget)
			//{
			//	if (TurnDir == TurnDirection.CW)
			//		LPT = (startFIX.FlyMode == eFlyMode.Flyby ? -startFIX.LPT_L : startFIX.LPT_L);
			//	else
			//		LPT = (startFIX.FlyMode == eFlyMode.Flyby ? -startFIX.LPT_R : startFIX.LPT_R);
			//}
			//else
			//	LPT = (startFIX.FlyMode == eFlyMode.Flyby ? -startFIX.LPT : startFIX.LPT);

			//ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, FlyMode == eFlyMode.FlyBy ? EntryDir - ARANMath.C_PI : EntryDir, LPT, 0);
			ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, LPT - 1.0, 0);

			//_UI.DrawPointWithText(basePoints[0], "BasePoints[0]", 192);
			//_UI.DrawPointWithText(ptTmp, "LPT");
			//ProcessMessages();

			double dirN0 = ARANFunctions.ReturnAngleInRadians(basePoints[basePoints.Count - 1], basePoints[0]);
			double dir01 = ARANFunctions.ReturnAngleInRadians(basePoints[0], basePoints[1]);        //NextDir		//CurrDir
			double CurrDiff = ARANMath.SubtractAngles(dir01, EntryDir);//dir01- dirN0;//

			//_UI.DrawPointWithText(ptTmp, 255, "LPT");
			//	_UI.DrawPointWithText(_StartFIX.PrjPt, 255, "FIX");
			//	LineString lstr = new LineString();
			//	lstr.Add(ARANFunctions.LocalToPrj(ptTmp, EntryDir, 0, -10000));
			//	lstr.Add(ptTmp);
			//	lstr.Add(ARANFunctions.LocalToPrj(ptTmp, EntryDir, 0, 10000));
			//	_UI.DrawLineString(lstr, 2, 255);
			//ProcessMessages();

			ptFrom = null;

			//for (int gh = 0; gh < _StartFIX.BasePoints.Count; gh++)
			//    _UI.DrawPointWithText(_StartFIX.BasePoints[gh], "pt-" + gh, 0);

			//_UI.DrawRing(PrevLeg.PrimaryArea[0].ExteriorRing, eFillStyle.sfsCross, 233);
			//ProcessMessages();

			//List<Point> BasePoints = new List<Point>();

			if (TurnDir == TurnDirection.CCW)
			{
				if (IsPrimary)
				{
					ASW_OUT0F = _StartFIX.ASW_2_R;

					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDir - ARANMath.C_PI_2, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, StartFIX.PrjPt, OutDir - ARANMath.C_PI_2, OutDir, out fTmp);
						if (ptFrom == null)
							ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, StartFIX.PrjPt, EntryDir - ARANMath.C_PI_2, out fTmp);
						//_UI.DrawPointWithText(ptFrom, "ptFrom-1");
						//ProcessMessages();

						//_UI.DrawMultiPolygon(PrevLeg.PrimaryArea, eFillStyle.sfsCross);
						//LineString lstr = new LineString();
						//lstr.Add(ARANFunctions.LocalToPrj(ptTmp, EntryDir, 0, -10000));
						//lstr.Add(ptTmp);
						//lstr.Add(ARANFunctions.LocalToPrj(ptTmp, EntryDir, 0, 10000));
						//_UI.DrawLineString(lstr, 2, 255);
						//_UI.DrawPointWithText(ptTmp, "LPT");
						//_UI.DrawPointWithText(StartFIX.PrjPt, "StartFIX");
						//ProcessMessages();

						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}

					ASW_OUT0C = 0.5 * _StartFIX.SemiWidth;
					ASW_OUT1 = 0.5 * wptTransitions[0].SemiWidth;
					dRad = 0;
					InnerBasePoint = basePoints[1];
					OuterBasePoint = basePoints[0];
				}
				else
				{
					ASW_OUT0F = _StartFIX.ASW_R;
					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir - ARANMath.C_PI_2, out fTmp);
						if (ptFrom == null)
							ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, StartFIX.PrjPt, EntryDir - ARANMath.C_PI_2, out fTmp);

						//_UI.DrawPointWithText(ptFrom, "ptFrom", 0);
						//ProcessMessages();

						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}

					ASW_OUT0C = _StartFIX.SemiWidth;
					ASW_OUT1 = wptTransitions[0].SemiWidth;
					dRad = ASW_OUT0F - _StartFIX.ASW_2_R;

					//InnerBasePoint = ARANFunctions.LocalToPrj(_StartFIX.BasePoints[1], dir01, _StartFIX.ASW_2_R);
					//OuterBasePoint = ARANFunctions.LocalToPrj(_StartFIX.BasePoints[0], dir01, -_StartFIX.ASW_2_L);
					InnerBasePoint = basePoints[1];
					OuterBasePoint = basePoints[0];
				}

				//InnerBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir + ARANMath.C_PI_2, _StartFIX.ASW_2_L, 0);
				//OuterBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir - ARANMath.C_PI_2, StartFIX.ASW_2_R, 0);
			}
			else
			{
				if (IsPrimary)
				{
					ASW_OUT0F = _StartFIX.ASW_2_L;
					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDir + ARANMath.C_PI_2, out fTmp);
						if (ptFrom == null)
							ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, StartFIX.PrjPt, EntryDir + ARANMath.C_PI_2, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, StartFIX.PrjPt, EntryDir + ARANMath.C_PI_2, OutDir, out fTmp);

						//	_UI.DrawPointWithText(ptTmp, "LPT", 255);
						//_UI.DrawPointWithText(ptFrom, "ptFrom");
						//_UI.DrawRing(PrevLeg.PrimaryArea[0].ExteriorRing, 167, eFillStyle.sfsDiagonalCross);
						//ProcessMessages();

						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}

					ASW_OUT0C = 0.5 * _StartFIX.SemiWidth;
					ASW_OUT1 = 0.5 * wptTransitions[0].SemiWidth;
					dRad = 0;
					InnerBasePoint = basePoints[1];
					OuterBasePoint = basePoints[0];
				}
				else
				{
					ASW_OUT0F = _StartFIX.ASW_L;
					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir + ARANMath.C_PI_2, out fTmp);
						if (ptFrom == null)
							ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, StartFIX.PrjPt, EntryDir + ARANMath.C_PI_2, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, StartFIX.PrjPt, EntryDir + ARANMath.C_PI_2, OutDir, out fTmp);

						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}

					ASW_OUT0C = _StartFIX.SemiWidth;
					ASW_OUT1 = wptTransitions[0].SemiWidth;
					dRad = ASW_OUT0F - _StartFIX.ASW_2_L;

					//InnerBasePoint = ARANFunctions.LocalToPrj(_StartFIX.BasePoints[1], dir01, _StartFIX.ASW_2_L);
					//OuterBasePoint = ARANFunctions.LocalToPrj(_StartFIX.BasePoints[0], dir01, -_StartFIX.ASW_2_R);

					InnerBasePoint = basePoints[1];
					OuterBasePoint = basePoints[0];
				}
				//InnerBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir - ARANMath.C_PI_2, _StartFIX.ASW_2_R, 0);
				//OuterBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir + ARANMath.C_PI_2, StartFIX.ASW_2_L, 0);
			}

			//if (StartFIX.FlyMode == eFlyMode.AtHeight)
			//{
			//    ASW_OUT0C = ASW_OUT0F;

			//    if (IsPrimary)
			//        InnerBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir + fSide * ARANMath.C_PI_2, ASW_OUT0F, 0);
			//    else
			//        InnerBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir + fSide * ARANMath.C_PI_2, 0.5 * ASW_OUT0F, 0);
			//}

			//	_UI.DrawPointWithText(InnerBasePoint, 255, "InnerBasePoint");
			//	_UI.DrawPointWithText(OuterBasePoint, 255, "OuterBasePoint");
			//	_UI.DrawPointWithText(ptFrom, 0, "ptFrom");
			//	_UI.DrawPointWithText(ptTmp, 255, "ptTmp");
			//	ProcessMessages();

			TurnR += dRad;

			//if (t == 0)
			//	ASW_OUTMax = Math.Max(ASW_OUT0C, ASW_OUT1);
			//else
			ASW_OUTMax = ASW_OUT0C;

			Ring result = new Ring();

			if (ptFrom == null)
			{
				ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir - ARANMath.C_PI, _StartFIX.EPT + 10.0, ASW_OUT0F * fSide);
				//_UI.DrawPointWithText(ptTmp, 0, "ptTmp1");
				//ProcessMessages();

				result.Add(ptTmp);

				ptFrom = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir - ARANMath.C_PI_2 * fSide, ASW_OUT0F, 0);
				//	_UI.DrawPointWithText(ptFrom, 0, "ptFrom");
				//	ProcessMessages();
			}
			//else				OuterBasePoint = (Point)ptFrom;//.Clone();

			//_UI.DrawPointWithText(OuterBasePoint, "OuterBasePoint");
			//_UI.DrawPointWithText(InnerBasePoint, "InnerBasePoint");
			//_UI.DrawPointWithText(ptFrom, "ptFrom");
			//ProcessMessages();

			result.Add(ptFrom);
			//	_StartFIX.CalcExtraTurnRangePoints();

			ptCurr = null;
			//=============================================================================
			IsMAPt = (_StartFIX.Role == eFIXRole.FAF_ && _EndFIX.Role == eFIXRole.MAPt_);

			//ptCnt = ARANFunctions.LocalToPrj(OuterBasePoint, EntryDir + ARANMath.C_PI_2 * fSide, TurnR - dRad, 0);
			ptCnt = ARANFunctions.LocalToPrj(OuterBasePoint, EntryDir + ARANMath.C_PI_2 * fSide, TurnR, 0);

			//_UI.DrawPointWithText(ptCnt, "ptCnt-0");
			//_UI.DrawPointWithText(OuterBasePoint, "OuterBasePoint");
			//_UI.DrawPointWithText(ptTmp, "LPT");
			//ProcessMessages();

			SpStartDir = EntryDir - ARANMath.C_PI_2 * fSide;
			SpStartRad = SpStartDir;
			BulgeAngle = Math.Atan2(coef, TurnR) * fSide;

			SpTurnAng = ARANFunctions.SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir - BulgeAngle, OutDir, TurnDir);
			//	SpTurnAng = SpiralTouchAngleOld(TurnR, coef, EntryDir, OutDir, TurnDir);

			Dist0 = TurnR + SpTurnAng * coef;
			ptTmp = ARANFunctions.LocalToPrj(ptCnt, SpStartDir + SpTurnAng * fSide, Dist0, 0);

			//	_UI.DrawPointWithText(ptCnt, 255, "ptCnt-0");
			//	_UI.DrawPointWithText(ptTmp, 0, "ptSp-1");
			//	_UI.DrawPointWithText(StartFIX.PrjPt, 0, "FIX");
			//	ProcessMessages();

			SpAbeamDist = -fSide * ARANFunctions.PointToLineDistance(ptTmp, _EndFIX.PrjPt, OutDir);

			HaveSecondSP = (TurnAng >= CurrDiff + SpiralSplayAngle) || ((TurnAng >= CurrDiff - SpiralDivergenceAngle) && (SpAbeamDist > ASW_OUT0C));
			SpFromAngle = 0;

			//for (int gh = 0; gh < _StartFIX.BasePoints.Count; gh++)
			//	_UI.DrawPointWithText(_StartFIX.BasePoints[gh], 0, "pt-" + gh);
			//ProcessMessages();

			//if (TurnAng > ARANMath.C_PI_2)
			//    AztEnd1 = EntryDir + ARANMath.C_PI_2 * fSide;
			//else
			//    AztEnd1 = OutDir;

			if (TurnAng > CurrDiff)
				AztEnd1 = EntryDir + CurrDiff * fSide;
			else
				AztEnd1 = OutDir;

			//LineString ls = new LineString();
			//ls.Add(basePoints[0]);
			//ls.Add(ARANFunctions.LocalToPrj(basePoints[0], AztEnd1 * fSide, 10000.0));
			//_UI.DrawLineString(ls, 2);
			//ProcessMessages();

			if (HaveSecondSP)
				AztEnd2 = EntryDir + CurrDiff * fSide;       //ARANMath.C_PI_2 
			else
				AztEnd2 = AztEnd1;

			SpToAngle = ARANFunctions.SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir - BulgeAngle, AztEnd1, TurnDir);
			//SpToAngle = ARANFunctions.SpiralTouchAngleOld(TurnR, coef, EntryDir, AztEnd1, TurnDir);


			if (FlyMode == eFlyMode.Flyby)
				SpTurnAng = ARANFunctions.SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir - BulgeAngle, EntryDir, TurnDir);      //SpTurnAng = SpiralTouchAngleOld(TurnR, coef, EntryDir, EntryDir, TurnDir)
			else
				SpTurnAng = SpToAngle;

			//LineString ls = new LineString();
			//ls.Add(basePoints[0]);
			//ls.Add(ARANFunctions.LocalToPrj(basePoints[0], SpStartDir * fSide, 10000.0));
			//_UI.DrawLineString(ls, 2);
			//ProcessMessages();

			//ls = new LineString();
			//ls.Add(basePoints[0]);
			//ls.Add(ARANFunctions.LocalToPrj(basePoints[0], SpToAngle * fSide, 10000.0));
			//_UI.DrawLineString(ls, 2);

			//ProcessMessages();

			n = (int)Math.Ceiling(ARANMath.RadToDeg(SpTurnAng));

			if (n <= 1) n = 1;
			else if (n <= 5) n = 5;
			else if (n < 10) n = 10;

			dAlpha = SpTurnAng / n;

			for (i = 0; i <= n; i++)
			{
				R = TurnR + i * dAlpha * coef;
				ptCurr = ARANFunctions.LocalToPrj(ptCnt, SpStartDir + i * dAlpha * fSide, R, 0);
				//_UI.DrawPointWithText(ptCurr, ARANFunctions.RGB(0, 128, 0), (i + 1).ToString());
				//ProcessMessages();
				result.Add(ptCurr);
			}

			//Ring rr = new Ring();
			//rr.Assign(result);
			//_UI.DrawRing(rr, eFillStyle.sfsHorizontal);
			//ProcessMessages();

			//_UI.DrawPointWithText(ptCurr, "ptCurr", ARANFunctions.RGB(0, 128, 0));
			//ProcessMessages();

			if (FlyMode == eFlyMode.Flyby)
			{
				Dist0 = TurnR + SpToAngle * coef;

				ptCurr1 = ptCurr;
				ptCurr = ARANFunctions.LocalToPrj(ptCnt, EntryDir + (SpToAngle - ARANMath.C_PI_2) * fSide, Dist0, 0);
				ptInter = ARANFunctions.LineLineIntersect(ptCurr1, EntryDir, ptCurr, AztEnd1) as Point;

				//_UI.DrawPointWithText(ptInter, ARANFunctions.RGB(0, 128, 0), "ptIn-0");
				//_UI.DrawPointWithText(ptCurr1, ARANFunctions.RGB(0, 128, 0), "ptC-0");
				//_UI.DrawPointWithText(ptCurr, ARANFunctions.RGB(0, 128, 0), "ptC-1");
				//ProcessMessages();
				if (ptInter != null)
				{
					if (IsPrimary)
						result.Add(ptInter);
					else
					{
						double TurnAng1 = ARANMath.Modulus((AztEnd1 - EntryDir) * fSide, ARANMath.C_2xPI);
						double radius = dRad;               // 0.5 * ASW_OUT0F;	//?????????????
						Dist0 = radius * Math.Tan(0.5 * TurnAng1);
						Point ptCnt1p = ARANFunctions.LocalToPrj(ptInter, EntryDir, -Dist0, radius * fSide);

						//_UI.DrawPointWithText(ptCnt1p, -1, "ptCnt1p");
						//ProcessMessages();

						n = (int)Math.Round(ARANMath.RadToDeg(TurnAng1));
						if (n <= 1) n = 1;
						else if (n <= 5) n = 5;
						else if (n < 10) n = 10;

						dAlpha = TurnAng1 / n;
						for (i = 0; i <= n; i++)
						{
							ptCurr1 = ARANFunctions.LocalToPrj(ptCnt1p, EntryDir + (i * dAlpha - ARANMath.C_PI_2) * fSide, radius);
							result.Add(ptCurr1);
						}
					}
				}

				result.Add(ptCurr);
			}

			//Ring rr = new Ring();
			//rr.Assign(result);
			//_UI.DrawRing(rr, -1, eFillStyle.sfsHorizontal);
			//ProcessMessages();

			//LineString lst = new LineString();
			//lst.Add(wptTransitions[0].PrjPt);
			//lst.Add(ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir - ARANMath.C_PI_2 * fSide, ASW_OUTMax, 0));
			//_UI.DrawLineString(lst, 0, 2);
			//ProcessMessages();

			ptBase = ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir - ARANMath.C_PI_2 * fSide, ASW_OUTMax, 0);

			//_UI.DrawPointWithText(ptBase, "ptBase", ARANFunctions.RGB(0, 128, 0));
			//ProcessMessages();

			BaseDir = OutDir;

			SpStartDir += SpToAngle * fSide;
			SpFromAngle = SpToAngle;

			//ProcessMessages(true);
			//LineString ls = new LineString();
			//ls.Add(basePoints[0]);
			//ls.Add(ARANFunctions.LocalToPrj(basePoints[0], SpStartDir, 10000.0));
			//_UI.DrawLineString(ls, 2);
			//ProcessMessages();

			CurDist = ARANFunctions.PointToLineDistance(ptCurr, wptTransitions[0].PrjPt, OutDir + ARANMath.C_PI_2);
			CurWidth = -fSide * ARANFunctions.PointToLineDistance(ptCurr, _EndFIX.PrjPt, OutDir);

			if (IsMAPt)
			{
				Dist0 = ARANMath.Hypot(wptTransitions[0].PrjPt.X - _StartFIX.PrjPt.X, wptTransitions[0].PrjPt.Y - _StartFIX.PrjPt.Y);
				dPhi1 = Math.Atan2(ASW_OUT0C - ASW_OUT1, Dist0);
				if (dPhi1 > SpiralDivergenceAngle)
					dPhi1 = SpiralDivergenceAngle;

				SpAngle = OutDir + dPhi1 * fSide;

				ptBase = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, OutDir - ARANMath.C_PI_2 * fSide, ASW_OUT0C, 0);

				if (CurWidth < ASW_OUTMax)
					SplayAngle = OutDir - SpiralSplayAngle * fSide;
				else
					SplayAngle = OutDir + SpiralDivergenceAngle * fSide;

				ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, SplayAngle, ptBase, SpAngle);
				ptInterDist = ARANFunctions.PointToLineDistance(ptInter, wptTransitions[0].PrjPt, OutDir + ARANMath.C_PI_2);

				MaxDist = Math.Max(MaxDist, ptInterDist);

				BaseDir = SpAngle;
				ASW_OUTMax = ARANFunctions.PointToLineDistance(ptInter, wptTransitions[0].PrjPt, OutDir);   //*fSide 
			}

			//		fTmp = Modulus((AztEnd2 - AztEnd1) * fSide, 2*PI);
			//		(HaveSecondSP or ((fTmp > EpsilonRadian) and (fTmp < PI)))then
			if (CurDist - LPTYDist > ARANMath.EpsilonDistance && (HaveSecondSP || (ARANMath.Modulus(AztEnd2 - AztEnd1, ARANMath.C_2xPI) > ARANMath.EpsilonRadian)))
			{
				if (TransitionDist > CurDist)
					ASW_OUTMax = ASW_OUT1;

				if (CurWidth - ASW_OUTMax > ARANMath.EpsilonDistance)
				{
					//_UI.DrawPointWithText(ptCurr, "ptC-1", ARANFunctions.RGB(0, 128, 0));
					//ProcessMessages();

					if (ARANMath.Modulus(AztEnd2 - AztEnd1, ARANMath.C_2xPI) > ARANMath.EpsilonRadian)
					//	if (fTmp > EpsilonRadian) and (fTmp < PI) then
					{
						SpToAngle = ARANFunctions.SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir - BulgeAngle, AztEnd2, TurnDir);
						//	SpToAngle = SpiralTouchAngleOld(TurnR, coef, EntryDir, AztEnd2, TurnDir);

						SpTurnAng = SpToAngle - SpFromAngle;
						if (SpTurnAng >= 0)
						{
							n = (int)Math.Round(ARANMath.RadToDeg(SpTurnAng));
							if (n < 1) n = 1;
							else if (n < 5) n = 5;
							else if (n < 10) n = 10;

							dAlpha = SpTurnAng / n;
							bFlag = false;
							PrevX = PrevY = 0;

							for (i = 0; i <= n; i++)
							{
								R = TurnR + (SpFromAngle + i * dAlpha) * coef;
								ptCurr = ARANFunctions.LocalToPrj(ptCnt, SpStartDir + i * dAlpha * fSide, R, 0);

								ptTmp = ARANFunctions.PrjToLocal(ptBase, BaseDir, ptCurr);

								//if ((!bFlag) && (ptTmp.Y * fSide <= 0.0 && ptTmp.X <= 0.0))
								//    bFlag = true;

								//if (bFlag && ptTmp.Y * fSide >= 0 && i > 0)
								//{
								//    K = -PrevY / (ptTmp.Y - PrevY);
								//    ptCurr.X = result[result.Count - 1].X + K * (ptCurr.X - result[result.Count - 1].X);
								//    ptCurr.Y = result[result.Count - 1].Y + K * (ptCurr.Y - result[result.Count - 1].Y);
								//    result.Add(ptCurr);
								//    //_UI.DrawPoint(ptTmp, 128);
								//    //ProcessMessages();
								//    break;
								//}
								if (i > 0 && ((ptTmp.Y * fSide >= 0.0 && PrevY * fSide <= 0.0) || ptTmp.X >= 0.0))
								{
									if (ptTmp.X < 0.0) K = -PrevY / (ptTmp.Y - PrevY);
									else K = -PrevX / (ptTmp.X - PrevX);

									ptCurr.X = result[result.Count - 1].X + K * (ptCurr.X - result[result.Count - 1].X);
									ptCurr.Y = result[result.Count - 1].Y + K * (ptCurr.Y - result[result.Count - 1].Y);

									result.Add(ptCurr);
									//bFlag = true;
									break;
								}

								PrevX = ptTmp.X;
								PrevY = ptTmp.Y;
								result.Add(ptCurr);
							}

							SpStartDir += SpTurnAng * fSide;
							SpFromAngle = SpToAngle;

							//Ring rr = new Ring();
							//rr.Assign(result);
							//_UI.DrawRing(rr, -1, eFillStyle.sfsHorizontal);
							//ProcessMessages();
						}
					}
				}

				//_UI.DrawRing(result, 0, eFillStyle.sfsForwardDiagonal);
				//ProcessMessages();

				if (HaveSecondSP)
				{
					ptCnt = ARANFunctions.LocalToPrj(InnerBasePoint, EntryDir + ARANMath.C_PI_2 * fSide, TurnR - dRad, 0);

					//_UI.DrawPointWithText(ptCnt, "ptCnt-2", 255);
					//_UI.DrawPointWithText(InnerBasePoint, "InnerBasePoint", 255);
					//ProcessMessages();

					R = TurnR + SpFromAngle * coef;
					//ptCurr1 = ARANFunctions.LocalToPrj(ptCnt, SpStartDir + SpFromAngle * fSide, R, 0);
					ptCurr1 = ARANFunctions.LocalToPrj(ptCnt, SpStartDir, R, 0);

					//	_UI.DrawPointWithText(ptBase, "ptBase", 0);
					//	ProcessMessages(true);
					//_UI.DrawPointWithText(ptCurr1, "ptSp-2", 0);
					//_UI.DrawPointWithText(ptCnt, "ptCnt-2", 255);
					//_UI.DrawPointWithText(ptCurr, "ptCurr-2", 255);
					//ProcessMessages();

					Delta = -fSide * ARANFunctions.PointToLineDistance(ptCurr1, ptBase, BaseDir);   //

					//fTmp = ARANMath.Modulus(EntryDir + fSide * CurrDir - BaseDir, ARANMath.C_2xPI);
					fTmp = ARANMath.Modulus(dir01 - BaseDir, ARANMath.C_2xPI);

					if (Math.Abs(fTmp) < ARANMath.EpsilonRadian || Math.Abs(fTmp - ARANMath.C_2xPI) < ARANMath.EpsilonRadian || Math.Abs(fTmp - ARANMath.C_PI) < ARANMath.EpsilonRadian)
					{ }
					//else if (Math.Abs(Delta) > ARANMath.EpsilonDistance)
					else if (Delta < -ARANMath.EpsilonDistance)
					{
						//ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, EntryDir + fSide * CurrDir, ptBase, BaseDir);
						ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, dir01, ptBase, BaseDir);

						/*
							LineString lst = new LineString();
							lst.Add(ptCurr);
							lst.Add(ARANFunctions.LocalToPrj(ptCurr, dir01 + ARANMath.C_PI, 15000, 0));
							_UI.DrawLineString(lst, 255<<8,2);
							ProcessMessages();
						*/

						fTmp = ARANFunctions.PointToLineDistance(ptInter, wptTransitions[0].PrjPt, OutDir + ARANMath.C_PI_2);
						Dist0 = ARANFunctions.PointToLineDistance(ptInter, ptCurr, OutDir + ARANMath.C_PI_2);

						if (fTmp > 0 && Dist0 < 0)
						{
							ptCurr = ptInter;
							result.Add(ptCurr);
						}

						/*
						if( fTmp < 0 )
							ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, EntryDir + ARANMath.C_PI_2, wptTransitions[0].PrjPt, OutDir + ARANMath.C_PI_2);

						fTmp = PointToLineDistance(ptInter, ptCurr, OutDir + ARANMath.C_PI_2);
						if (false )
						//if (fTmp < 0 )
						{
								ptCurr = ptInter;
								result.Add(ptCurr);
						}

						*/
					}
				}
			}


			//CurWidth = -fSide * ARANFunctions.PointToLineDistance(ptCurr, _StartFIX.PrjPt, OutDir);
			CurWidth = -fSide * ARANFunctions.PointToLineDistance(ptCurr, _EndFIX.PrjPt, OutDir);
			CurDist = ARANFunctions.PointToLineDistance(ptCurr, wptTransitions[0].PrjPt, OutDir + ARANMath.C_PI_2);

			//if (IsMAPt)
			//{
			//Dist0 = Hypot(wptTransitions[0].PrjPt.X - StartFIX.PrjPt.X, wptTransitions[0].PrjPt.Y - StartFIX.PrjPt.Y);
			//dPhi1 = ArcTan2(ASW_OUT0C - ASW_OUT1, Dist0);
			//if dPhi1 > SpiralDivergenceAngle then dPhi1 := SpiralDivergenceAngle;

			//		SpAngle = OutDir - dPhi1 * fSide;
			//		LocalToPrj(StartFIX.PrjPt, OutDir + 0.5 * PI * fSide, ASW_OUT0C, 0, ptBase);

			//		if (CurWidth < ASW_OUTMax) then
			//			SplayAngle := OutDir + SpiralSplayAngle * fSide
			//		else
			//			SplayAngle := OutDir - SpiralDivergenceAngle * fSide;

			//		ptInter = LineLineIntersect(ptCurr, SplayAngle, InnerBasePoint, SpAngle).AsPoint;
			//		ptInterDist = PointToLineDistance(ptInter, wptTransitions[0].PrjPt, OutDir + 0.5 * PI);

			//		TransitionDist = Max(TransitionDist, ptInterDist);
			//		if TransitionDist = ptInterDist then
			//		begin
			//			IsMAPt = True;
			//			BaseDir = SpAngle;
			//			ASW_OUTMax = fSide * PointToLineDistance(ptInter, wptTransitions[0].PrjPt, OutDir);
			//		end;
			//}

			//=============================================================================

			int tranI = 0;
			if (TransitionDist > CurDist)
			{
				ASW_OUTMax = ASW_OUT1;
				ptBase = ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir - ARANMath.C_PI_2 * fSide, ASW_OUTMax, 0);    //???

				if (transitions > 1)
					tranI = 1;

				CurDist = ARANFunctions.PointToLineDistance(ptCurr, wptTransitions[tranI].PrjPt, OutDir + ARANMath.C_PI_2);
			}

			//if (CurDist - LPTYDist > ARANMath.EpsilonDistance && Math.Abs(CurWidth - ASW_OUTMax) > ARANMath.EpsilonDistance)
			if (CurDist - LPTYDist > ARANMath.EpsilonDistance && (HaveSecondSP || (Math.Abs(CurWidth - ASW_OUTMax) > ARANMath.EpsilonDistance)))
			{
				if (CurWidth - ASW_OUTMax > ARANMath.EpsilonDistance)
				{
					SpToAngle = ARANFunctions.SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir - BulgeAngle, OutDir + DivergenceAngle30 * fSide, TurnDir);
					//SpToAngle = ARANFunctions.SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir - BulgeAngle, OutDir - DivergenceAngle30 * fSide, TurnDir);
					SpTurnAng = SpToAngle - SpFromAngle;

					if (SpTurnAng >= 0)
					{
						n = (int)Math.Round(ARANMath.RadToDeg(SpTurnAng));
						if (n < 1) n = 1;
						else if (n < 5) n = 5;
						else if (n < 10) n = 10;

						dAlpha = SpTurnAng / n;
						bFlag = false;
						PrevX = PrevY = 0.0;

						for (i = 0; i <= n; i++)
						{
							R = TurnR + (SpFromAngle + i * dAlpha) * coef;
							ptCurr = ARANFunctions.LocalToPrj(ptCnt, SpStartDir + i * dAlpha * fSide, R, 0);
							ptTmp = ARANFunctions.PrjToLocal(ptBase, BaseDir, ptCurr);

							//LineString lst = new LineString();
							//lst.Add(ptBase);
							//lst.Add(ARANFunctions.LocalToPrj(ptBase, BaseDir + ARANMath.C_PI, 35000, 0));
							//_UI.DrawLineString(lst, -1, 2);
							//_UI.DrawPointWithText(ptBase, -1, "Base");
							//ProcessMessages();

							if (!bFlag)
							{
								if (ptTmp.Y * fSide <= 0.0 && ptTmp.X <= 0.0)
									bFlag = true;
								if (!bFlag)
									break;
							}
							else if (i > 0)
							{
								//if (ptTmp.X >= 0)
								//{
								//    K = -PrevX / (ptTmp.X - PrevX);
								//    ptCurr.X = result[result.Count - 1].X + K * (ptCurr.X - result[result.Count - 1].X);
								//    ptCurr.Y = result[result.Count - 1].Y + K * (ptCurr.Y - result[result.Count - 1].Y);

								//    result.Add(ptCurr);
								//    break;
								//}
								//else if (ptTmp.Y * fSide >= 0.0)
								//{
								//    K = -PrevY / (ptTmp.Y - PrevY);
								//    ptCurr.X = result[result.Count - 1].X + K * (ptCurr.X - result[result.Count - 1].X);
								//    ptCurr.Y = result[result.Count - 1].Y + K * (ptCurr.Y - result[result.Count - 1].Y);

								//    result.Add(ptCurr);
								//    break;
								//}

								if (ptTmp.Y * fSide >= 0.0 || ptTmp.X >= 0.0)
								{
									if (ptTmp.X < 0) K = -PrevY / (ptTmp.Y - PrevY);
									else K = -PrevX / (ptTmp.X - PrevX);

									ptCurr.X = result[result.Count - 1].X + K * (ptCurr.X - result[result.Count - 1].X);
									ptCurr.Y = result[result.Count - 1].Y + K * (ptCurr.Y - result[result.Count - 1].Y);

									result.Add(ptCurr);
									break;
								}
							}

							PrevX = ptTmp.X;
							PrevY = ptTmp.Y;
							result.Add(ptCurr);
						}

						//Ring rr = new Ring();
						//rr.Assign(result);
						//_UI.DrawRing(rr, eFillStyle.sfsHorizontal);
						//ProcessMessages();
					}
				}
				else //if (ASW_OUTMax - CurWidth > ARANMath.EpsilonDistance)
				{
					SplayAngle = OutDir - SplayAngle15 * fSide;

					//_UI.DrawPointWithText(ptCurr, 0, "ptCurr");
					//_UI.DrawPointWithText(ptBase, 0, "ptBase");
					//_UI.DrawPointWithText(wptTransitions[0].PrjPt, -1, "wptTrans-0");
					//_UI.DrawPointWithText(_StartFIX.PrjPt, -1, "_StartFIX");
					//ProcessMessages();

					ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, SplayAngle, ptBase, BaseDir);
					//	_UI.DrawPointWithText(ptInter, 0, "ptInter - 2");
					//	ProcessMessages();

					ptInterDist = ARANFunctions.PointToLineDistance(ptInter, wptTransitions[tranI].PrjPt, OutDir + ARANMath.C_PI_2);

					//_UI.DrawPointWithText(ptInter, 0, "ptInter - 2");
					//ProcessMessages(true);

					if (ptInterDist < TransitionDist)
					{
						ptCut = ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir + ARANMath.C_PI, TransitionDist, 0);
						ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, SplayAngle, ptCut, OutDir + ARANMath.C_PI_2);
						//if GlDRAWFLG then
						//		_UI.DrawPointWithText(ptInter, 0, "ptInter - 2");
					}
					result.Add(ptInter);
				}
			}

			//ProcessMessages(true);
			//_UI.DrawRing(result, ARANFunctions.RGB(0, 0, 255), eFillStyle.sfsDiagonalCross);
			//ProcessMessages();

			//Ring rr = new Ring();
			//rr.Assign(result);
			//_UI.DrawRing(rr, eFillStyle.sfsHorizontal);

			//_UI.DrawPointWithText(ptCurr, "ptCurr");
			//ProcessMessages();

			JoinSegments(result, ARP, true, IsPrimary, EntryDir, TurnDir);
			//	_UI.DrawRing(result, ARANFunctions.RGB(0, 0, 255), eFillStyle.sfsDiagonalCross);
			//	ProcessMessages();

			return result;
		}

		virtual protected Ring CreateInnerTurnArea(LegBase PrevLeg, ADHPType ARP, bool IsPrimary, double EntryDir, TurnDirection TurnDir)
		{
			double OutDir, dPhi1, TurnAng, Dist3700, CurDist, MaxDist, CurWidth, ptInterDist, LPTYDist,
				fTmp, BaseDir, LegLenght, SplayAngle, fSide, ASW_INMax, ASW_IN0C, ASW_IN0F, ASW_IN1;
			//, Dist0, Dist1, dPhi2, azt0, Dist2;

			Point InnerBasePoint, ptTmp, ptBase, ptCurr, ptCut, ptInter, ptFrom = null;

			//bool  ReCalcASW_IN0C;

			double SpiralDivergenceAngle, SpiralSplayAngle,
					DivergenceAngle30 = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value,
					SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;

			OutDir = _StartFIX.OutDirection;
			fSide = (int)TurnDir;

			TurnAng = ARANMath.Modulus((OutDir - EntryDir) * fSide, ARANMath.C_2xPI);

			if (TurnAng < ARANMath.EpsilonRadian || Math.Abs(TurnAng - ARANMath.C_2xPI) < ARANMath.EpsilonRadian)
				TurnAng = 0.0;

			if (IsPrimary && TurnAng < ARANMath.DegToRad(60))
			{
				SpiralDivergenceAngle = Math.Atan(0.5 * Math.Tan(DivergenceAngle30));
				SpiralSplayAngle = Math.Atan(0.5 * Math.Tan(SplayAngle15));
				//SpiralDivergenceAngle = DivergenceAngle30;
				//SpiralSplayAngle = SplayAngle15;
			}
			else
			{
				SpiralDivergenceAngle = DivergenceAngle30;
				SpiralSplayAngle = SplayAngle15;
			}

			//ProcessMessages(true);
			//for (int i = 0; i < basePoints.Count; i++)
			//	_UI.DrawPointWithText(basePoints[i], "mpt_ptS-" + i.ToString());
			//ProcessMessages();

			//MAX DISTANCE + =================================================================

			LPTYDist = (_EndFIX.FlyMode == eFlyMode.Flyby ? _EndFIX.LPT : -_EndFIX.LPT) - OverlapDist;

			WayPoint[] wptTransitions = new WayPoint[3];
			Point ptTransition;
			int transitions = 0;

			if ((_StartFIX.FlightPhase > eFlightPhase.SIDGE28 && _StartFIX.FlightPhase <= eFlightPhase.SID) || (_StartFIX.FlightPhase >= eFlightPhase.FAFApch && _StartFIX.FlightPhase < eFlightPhase.MApGE28))
			{
				ptTransition = ARANFunctions.CircleVectorIntersect(ARP.pPtPrj, PANSOPSConstantList.PBNInternalTriggerDistance, _StartFIX.PrjPt, OutDir);
				if (!ptTransition.IsEmpty && ARANMath.SideDef(_StartFIX.PrjPt, OutDir + ARANMath.C_PI_2, ptTransition) == SideDirection.sideRight && ARANMath.SideDef(_EndFIX.PrjPt, OutDir + ARANMath.C_PI_2, ptTransition) == SideDirection.sideLeft)
				{
					WayPoint WPT_15NM = new WayPoint(eFIXRole.TP_, _aranEnv);
					WPT_15NM.FlightPhase = _StartFIX.FlightPhase <= eFlightPhase.SID ? eFlightPhase.SIDGE28 : eFlightPhase.MApGE28;
					WPT_15NM.SensorType = _StartFIX.SensorType;
					WPT_15NM.PBNType = _StartFIX.PBNType;

					WPT_15NM.EntryDirection = OutDir;
					WPT_15NM.OutDirection = OutDir;
					WPT_15NM.BankAngle = _EndFIX.BankAngle;

					WPT_15NM.PrjPt = ARANFunctions.LocalToPrj(ptTransition, OutDir, -WPT_15NM.ATT, 0);

					wptTransitions[transitions++] = WPT_15NM;

					_EndFIX.FlightPhase = WPT_15NM.FlightPhase;
					_EndFIX.BankAngle = WPT_15NM.BankAngle;

					//_UI.DrawPointWithText(ptTransition, ARANFunctions.RGB(0, 0, 255), "ptTransition");
				}
			}

			if ((_StartFIX.FlightPhase > eFlightPhase.SIDGE56 && _StartFIX.FlightPhase <= eFlightPhase.SID) || _StartFIX.FlightPhase >= eFlightPhase.FAFApch)
			{
				ptTransition = ARANFunctions.CircleVectorIntersect(ARP.pPtPrj, PANSOPSConstantList.PBNTerminalTriggerDistance, _StartFIX.PrjPt, OutDir);
				if (!ptTransition.IsEmpty && ARANMath.SideDef(_StartFIX.PrjPt, OutDir + ARANMath.C_PI_2, ptTransition) == SideDirection.sideRight && ARANMath.SideDef(_EndFIX.PrjPt, OutDir + ARANMath.C_PI_2, ptTransition) == SideDirection.sideLeft)
				{
					//ptTmp = ARANFunctions.PrjToLocal(EndFIX.PrjPt, OutDir - ARANMath.C_PI, ARP.pPtPrj);
					//Dist0 = ptTmp.X - Math.Sqrt(ARANMath.Sqr(fInternalTreshold) - ARANMath.Sqr(ptTmp.Y));

					WayPoint WPT_30NM = new WayPoint(eFIXRole.TP_, _aranEnv);
					WPT_30NM.FlightPhase = _StartFIX.FlightPhase <= eFlightPhase.SID ? eFlightPhase.SIDGE56 : eFlightPhase.STARGE56;
					WPT_30NM.SensorType = _StartFIX.SensorType;

					if (_StartFIX.FlightPhase >= eFlightPhase.FAFApch && _StartFIX.PBNType == ePBNClass.RNP_APCH)
					{
						WPT_30NM.PBNType = ePBNClass.RNAV1;
						_EndFIX.PBNType = ePBNClass.RNAV1;
					}

					WPT_30NM.EntryDirection = OutDir;
					WPT_30NM.OutDirection = OutDir;
					WPT_30NM.BankAngle = _EndFIX.BankAngle;

					WPT_30NM.PrjPt = ARANFunctions.LocalToPrj(ptTransition, OutDir, -WPT_30NM.ATT, 0);

					wptTransitions[transitions++] = WPT_30NM;

					_EndFIX.FlightPhase = WPT_30NM.FlightPhase;
					_EndFIX.BankAngle = WPT_30NM.BankAngle;

					//_UI.DrawPointWithText(ptTransition, ARANFunctions.RGB(0, 0, 255), "ptTransition");
					//if (Dist0 > TransitionDist)					TransitionDist = Dist0;
				}
			}

			wptTransitions[transitions++] = (WayPoint)_EndFIX.Clone();

			double Dist56000 = LPTYDist;
			double TransitionDist = LPTYDist;
			double fDistTreshold = LPTYDist;
			if (transitions > 1)
			{
				fDistTreshold = ARANMath.Hypot(ARP.pPtPrj.X - wptTransitions[0].PrjPt.X, ARP.pPtPrj.Y - wptTransitions[0].PrjPt.Y);

				ptTmp = ARANFunctions.PrjToLocal(wptTransitions[0].PrjPt, OutDir - ARANMath.C_PI, ARP.pPtPrj);
				Dist56000 = ptTmp.X - Math.Sqrt(ARANMath.Sqr(fDistTreshold) - ARANMath.Sqr(ptTmp.Y));

				TransitionDist = Dist56000;
			}

			if (_StartFIX.Role == eFIXRole.IF_ && _EndFIX.Role == eFIXRole.FAF_)
			{
				Dist3700 = 1.5 * (_StartFIX.XTT - _EndFIX.XTT) / Math.Tan(_constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value);
				//Dist3700 = (StartFIX.SemiWidth - EndFIX.SemiWidth)/Tan(GPANSOPSConstants.Constant[arSecAreaCutAngl].Value);
				//GPANSOPSConstants.Constant[rnvImMinDist].Value;
				TransitionDist = Math.Max(TransitionDist, Dist3700);
			}

			MaxDist = Math.Max(LPTYDist, TransitionDist);
			int i, j;
			double x, y;
			//MAX DISTANCE - =================================================================
			List<Point> basePoints;// = _StartFIX.BasePoints;

			WayPoint startFIX = (WayPoint)_StartFIX;
			if (_StartFIX is MATF)
			{
				MATF Matf = (MATF)_StartFIX;
				MAPt mAPt = Matf.maPt;
				startFIX = mAPt;

				if (IsPrimary)
					basePoints = ARANFunctions.GetBasePoints(Matf.PrjPt, Matf.EntryDirection, Matf.TolerArea[0], TurnDir);
				else
					basePoints = ARANFunctions.GetBasePoints(Matf.PrjPt, Matf.EntryDirection, Matf.SecondaryTolerArea[0], TurnDir);

				//_UI.DrawMultiPolygon(Matf.TolerArea, eFillStyle.sfsCross);
				//ProcessMessages();
			}
			else
			{
				basePoints = _StartFIX.BasePoints;

				if (!IsPrimary)
				{
					for (i = 0; i < basePoints.Count; i++)
					{
						Point ptt = basePoints[i];
						ARANFunctions.PrjToLocal(_StartFIX.PrjPt, _StartFIX.EntryDirection, ptt, out x, out y);
						if (y < 0)
							ptt = ARANFunctions.LocalToPrj(ptt, _StartFIX.EntryDirection, 0.0, -_StartFIX.ASW_R);
						else
							ptt = ARANFunctions.LocalToPrj(ptt, _StartFIX.EntryDirection, 0.0, _StartFIX.ASW_L);

						basePoints[i] = ptt;
					}
				}
			}

			double EPT;
			//if (_StartFIX.IsDFTarget)
			//{
			//	if (TurnDir == TurnDirection.CW)
			//		EPT = _StartFIX.EPT_R;
			//	else
			//		EPT = _StartFIX.EPT_L;
			//}
			//else
			//	EPT = _StartFIX.EPT;

			//ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir - ARANMath.C_PI, EPT, 0);

			if (startFIX.IsDFTarget)
			{
				if (TurnDir == TurnDirection.CW)
					EPT = startFIX.EPT_R;
				else
					EPT = startFIX.EPT_L;
			}
			else
				EPT = startFIX.EPT;

			ptTmp = ARANFunctions.LocalToPrj(startFIX.PrjPt, EntryDir - ARANMath.C_PI, EPT - 0.01);

			//_UI.DrawPointWithText(ptTmp, "ptTmp");
			//_UI.DrawPointWithText(startFIX.PrjPt, "startFIX");
			//ProcessMessages();

			if (TurnDir == TurnDirection.CCW)
			{
				if (IsPrimary)
				{
					ASW_IN0F = _StartFIX.ASW_2_L;
					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDir + ARANMath.C_PI_2, out fTmp);

						//_UI.DrawMultiPolygon(PrevLeg.PrimaryArea, eFillStyle.sfsCross);
						//_UI.DrawPointWithText(ptTmp, "ptTmp");
						//ProcessMessages();

						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, StartFIX.PrjPt, OutDir + ARANMath.C_PI_2, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_IN0F = fTmp;
					}

					ASW_IN0C = 0.5 * _StartFIX.SemiWidth;
					ASW_IN1 = 0.5 * wptTransitions[0].SemiWidth;
				}
				else
				{
					ASW_IN0F = _StartFIX.ASW_L;
					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir + ARANMath.C_PI_2, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, StartFIX.PrjPt, OutDir + ARANMath.C_PI_2, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_IN0F = fTmp;
					}

					ASW_IN0C = _StartFIX.SemiWidth;
					ASW_IN1 = wptTransitions[0].SemiWidth;
				}
				InnerBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir + ARANMath.C_PI_2, ASW_IN0F, 0);
			}
			else
			{
				if (IsPrimary)
				{
					ASW_IN0F = _StartFIX.ASW_2_R;
					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDir - ARANMath.C_PI_2, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, StartFIX.PrjPt, OutDir - ARANMath.C_PI_2, OutDir, out fTmp);

						//Ring rr = new Ring();
						//rr.Assign(PrevLeg.PrimaryArea[0].ExteriorRing);
						//_UI.DrawRing(rr, eFillStyle.sfsHorizontal);
						//ProcessMessages();

						if (ptFrom != null)
							ASW_IN0F = fTmp;
						//_UI.DrawPointWithText(ptFrom, "ptFrom");
						//ProcessMessages();
					}

					ASW_IN0C = 0.5 * _StartFIX.SemiWidth;
					ASW_IN1 = 0.5 * wptTransitions[0].SemiWidth;
				}
				else
				{
					ASW_IN0F = _StartFIX.ASW_R;
					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir - ARANMath.C_PI_2, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, StartFIX.PrjPt, OutDir - ARANMath.C_PI_2, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_IN0F = fTmp;
					}

					ASW_IN0C = _StartFIX.SemiWidth;
					ASW_IN1 = wptTransitions[0].SemiWidth;
				}
				InnerBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir - ARANMath.C_PI_2, ASW_IN0F, 0);
			}

			//ProcessMessages(true);

			//if (StartFIX.FlyMode == eFlyMode.AtHeight)
			//    ASW_IN0C = ASW_IN0F;
			//ProcessMessages();
			//for (i = 0; i < startFIX.BasePoints.Count; i++)
			//	_UI.DrawPointWithText(startFIX.BasePoints[i], "ptB-" + i.ToString());

			//for (i = 0; i < basePoints.Count; i++)
			//	_UI.DrawPointWithText(basePoints[i], "ptB-" + i.ToString());
			//ProcessMessages();

			//_UI.DrawPointWithText(ptFrom, "ptFrom");
			//ProcessMessages();

			//_UI.DrawPointWithText(InnerBasePoint, "InnerBasePoint");
			//ProcessMessages();

			ASW_INMax = Math.Max(ASW_IN0C, ASW_IN1);
			Ring result = new Ring();

			//if ===========================================================================================================================================
			ptBase = ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir + 0.5 * fSide * ARANMath.C_PI, ASW_IN0C);

			//_UI.DrawPointWithText(ptBase, "ptBase");
			//ProcessMessages();

			int n = basePoints.Count;

			double splay = SplayAngle15;    //SpiralSplayAngle;	// Math.Max(0.5 * TurnAng, SpiralDivergenceAngle);//SpiralDivergenceAngle
			i = j = 0;
			ptCurr = basePoints[i];

			//ProcessMessages(true);
			//LineString ls = new LineString();
			//ls.Add(ARANFunctions.LocalToPrj(ptCurr, OutDir + fSide * splay, 10000.0));
			//ls.Add(ARANFunctions.LocalToPrj(ptCurr, OutDir + fSide * splay, -8000.0));
			//_UI.DrawLineString(ls);
			//ProcessMessages();

			//////ls.Add(ARANFunctions.LocalToPrj(ptBase, OutDir + fSide * SpiralDivergenceAngle, -40000.0));
			////ls.Add(ARANFunctions.LocalToPrj(ptBase, OutDir + fSide * SplayAngle15, -40000.0));
			//ls.Add(ARANFunctions.LocalToPrj(ptBase, OutDir + fSide * splay, 10000.0));


			ARANFunctions.PrjToLocal(ptBase, OutDir + fSide * splay, ptCurr, out x, out y);

			MaxDist = y * fSide;

			for (i++; i < n; i++)
			{

				//LineString ls = new LineString();
				//ls.Add(ptBase);
				//ls.Add(ARANFunctions.LocalToPrj(ptBase, OutDir + fSide * splay, -40000.0));
				//_UI.DrawLineString(ls, -1, 1);
				//ProcessMessages();


				ARANFunctions.PrjToLocal(ptBase, OutDir + fSide * splay, basePoints[i], out x, out y);
				//ARANFunctions.PrjToLocal(ptBase, OutDir + fSide * SpiralDivergenceAngle, BasePoints[i], out x, out y);
				y *= fSide;
				if (y > MaxDist)
				{
					MaxDist = y;
					j = i;
				}

				//_UI.DrawPointWithText(basePoints[i], "pt-" + (i + 1).ToString());
				//ProcessMessages();
			}

			//_UI.DrawPointWithText(ptBase, "ptBase");
			//_UI.DrawPointWithText(ptCurr, "pt-I");
			//ProcessMessages();

			//_UI.DrawPointWithText(basePoints[(j + 1) % n], "pt_0");
			//_UI.DrawPointWithText(basePoints[j], "ptCurr_1");
			//ProcessMessages();

			//if (j < n - 1)
			result.Add(basePoints[(j + 1) % n]);

			ptCurr = basePoints[j];
			result.Add(ptCurr);

			ARANFunctions.PrjToLocal(ptBase, OutDir, ptCurr, out x, out y);
			y *= fSide;

			//LineString ls = new LineString();
			//ls.Add(ptBase);
			//ls.Add(ARANFunctions.LocalToPrj(ptBase, OutDir, -40000.0));
			//_UI.DrawLineString(ls, -1, 1);
			//ProcessMessages(true);

			if (ARANMath.RadToDeg(TurnAng) <= 1.0)
			{
				//JoinSegments(result, ARP, false, IsPrimary, EntryDir, TurnDir);
				//return result;
				//ptCurr = result[result.Count - 1];

				fTmp = SpiralSplayAngle;    //SplayAngle15;	//
				ptCurr = (Point)ARANFunctions.LineLineIntersect(ptCurr, OutDir + fSide * fTmp, ptBase, OutDir);

				//_UI.DrawPointWithText(ptCurr, "pt-IIa");
				//ProcessMessages();

				result.Add(ptCurr);
			}
			else if (y > 0)
			{
				fTmp = Math.Max(0.5 * TurnAng, SplayAngle15);   //splay

				//LineString ls = new LineString();
				//_UI.DrawPointWithText(ptBase, "ptBase");
				//ls.Add(ARANFunctions.LocalToPrj(ptBase, OutDir, 40000.0));
				//ls.Add(ARANFunctions.LocalToPrj(ptBase, OutDir, -40000.0));
				//_UI.DrawLineString(ls);
				//ProcessMessages();

				//ls.Clear();
				//ls.Add(ARANFunctions.LocalToPrj(ptBase, OutDir + fSide * splay, 40000.0));
				//ls.Add(ARANFunctions.LocalToPrj(ptBase, OutDir + fSide * splay, -40000.0));
				//_UI.DrawLineString(ls);
				//ProcessMessages();

				//ls.Clear();
				//_UI.DrawPointWithText(ptCurr, "pt-II");
				//ls.Add(ARANFunctions.LocalToPrj(ptCurr, EntryDir + fSide * fTmp, -40000.0));
				//ls.Add(ARANFunctions.LocalToPrj(ptCurr, EntryDir + fSide * fTmp, 40000.0));
				//_UI.DrawLineString(ls);
				//ProcessMessages();

				ptCurr = (Point)ARANFunctions.LineLineIntersect(ptCurr, EntryDir + fSide * fTmp, ptBase, OutDir);

				if (ptCurr == null || ARANMath.SideDef(basePoints[j], OutDir + ARANMath.C_PI_2, ptCurr) == SideDirection.sideLeft)
				{
					while (y > 0)
					{
						j = (j - 1) % n;
						if (j < 0)
							j += n;

						ARANFunctions.PrjToLocal(ptBase, OutDir, basePoints[j], out x, out y);
						y *= fSide;
						if (y < 0)
						{
							//_UI.DrawPointWithText(ptCurr, "ptCurr");
							//ProcessMessages();
							ptCurr = ptBase;
							result.Add(ptCurr);
							break;
						}

						ptCurr = basePoints[j];
						result.Add(ptCurr);
					}
				}
				else
					result.Add(ptCurr);


				//_UI.DrawPointWithText(result[result.Count-1],  "pt-II");
				//ProcessMessages();

				//ptInter = (Point)ARANFunctions.LineLineIntersect(InnerBasePoint, SplayAngle, ptBase, BaseDir);
				//BaseDir = OutDir;
				//ptBase = ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir + 0.5 * ARANMath.C_PI * fSide, ASW_IN0C, 0);

				//ptCurr = (Point)ARANFunctions.LineLineIntersect(ptCurr, EntryDir + fSide * fTmp, ptBase, OutDir);

				//if (ptCurr == null)
				//	ptCurr = ptBase;

				//result.Add(ptCurr);

				//Ring rr = new Ring();
				//rr.Assign(result);
				//_UI.DrawRing(rr, AranEnvironment.Symbols.eFillStyle.sfsHorizontal);

				//ProcessMessages(true);

			}
			else
			{
				// SpiralDivergenceAngle + SplayAngle15;
				//_UI.DrawPointWithText(ptCurr, -1, "ptCurr");
				//ARANFunctions.PrjToLocal(_EndFIX.PrjPt, EntryDir + Math.PI, _StartFIX.PrjPt, out x, out y);
				//_UI.DrawPointWithText(ptBase, -1, "ptBase");

				//ARANFunctions.PrjToLocal(_EndFIX.PrjPt, OutDir + Math.PI, _StartFIX.PrjPt, out x, out y);

				//double fTurnDir = (int)_StartFIX.EffectiveTurnDirection;
				//double TurnR = _StartFIX.ConstructTurnRadius;
				//double l = TurnR - y * fTurnDir;

				//fTmp = 0.5 * (l + TurnR) / TurnR;
				//if (Math.Abs(fTmp) > 1.0 && Math.Abs(fTmp) - 1 < 1.03)
				//	fTmp = Math.Sign(fTmp);

				//double alpha = Math.Acos(fTmp);
				//ptCurr = (Point)ARANFunctions.LineLineIntersect(ptCurr, OutDir + fSide * alpha, ptBase, OutDir);

				fTmp = splay;// Math.Max(Math.Max(0.5 * TurnAng, splay), alpha);     //DivergenceAngle30,//SpiralDivergenceAngle
							 //fTmp = Math.Max(0.5 * TurnAng, alpha);     //DivergenceAngle30
				ptCurr = (Point)ARANFunctions.LineLineIntersect(basePoints[j], OutDir + fSide * fTmp, ptBase, OutDir);

				//_UI.DrawPointWithText(basePoints[j], "ptCurr");
				//_UI.DrawPointWithText(ptCurr, "pt-IIa-1");
				//ProcessMessages();

				result.Add(ptCurr);
			}
			//ProcessMessages();


			//else ===========================================================================================================================================
#if OldStyle

			ptCurr = InnerBasePoint;

			//_UI.DrawPointWithText(ptCurr, "ptCurr-0");
			//_UI.DrawPointWithText(ptTmp, "ptCurr-00");
			//_UI.DrawPointWithText(ptTmp, "ptTmpI");
			//ProcessMessages();

			result.Add(ptTmp);
			result.Add(ptCurr);
#endif
			//===========================================================================================================================================

			//_UI.DrawPointWithText(ptFrom, 0, "ptCurr-0");
			//ProcessMessages(true);

			//	_UI.DrawPointWithText(ptFrom, 0, "ptFrom-I1");
			//	ProcessMessages(true);
			//result.Add(ptCurr);

			CurDist = ARANFunctions.PointToLineDistance(ptCurr, wptTransitions[0].PrjPt, OutDir + ARANMath.C_PI_2);

			if (CurDist < TransitionDist)
				TransitionDist = CurDist;

			//	if (CurDist > MaxDist)
			{
				//==============================================================================
				LegLenght = ARANMath.Hypot(wptTransitions[0].PrjPt.X - _StartFIX.PrjPt.X, wptTransitions[0].PrjPt.Y - _StartFIX.PrjPt.Y);
				if (_StartFIX.Role == eFIXRole.FAF_ && _EndFIX.Role == eFIXRole.MAPt_)
				{
					dPhi1 = Math.Atan2(ASW_IN0C - ASW_IN1, LegLenght);
					if (dPhi1 > DivergenceAngle30)
						dPhi1 = DivergenceAngle30;

					BaseDir = OutDir - dPhi1 * fSide;
					ptBase = ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir + ARANMath.C_PI_2 * fSide, ASW_IN1, 0);
				}
				else
				{
					BaseDir = OutDir;
					ptBase = ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir + ARANMath.C_PI_2 * fSide, ASW_IN0C, 0);
				}

				//_UI.DrawPointWithText(ptBase, "ptBase");
				//ProcessMessages();

				CurWidth = fSide * ARANFunctions.PointToLineDistance(ptCurr, ptBase, BaseDir);
				if (Math.Abs(CurWidth) > ARANMath.EpsilonDistance)
				{
					if (CurWidth > 0)
						SplayAngle = EntryDir + 0.5 * TurnAng * fSide;
					else
					{
						//SplayAngle15 = GlobalVars.constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
						SplayAngle = OutDir + SplayAngle15 * fSide;  //SpiralSplayAngle
					}

					ptInter = (Point)ARANFunctions.LineLineIntersect(InnerBasePoint, SplayAngle, ptBase, BaseDir);

					//_UI.DrawPointWithText(ptInter, "ptInter - 1");
					//ProcessMessages();

					ptInterDist = ARANFunctions.PointToLineDistance(ptInter, wptTransitions[0].PrjPt, OutDir + ARANMath.C_PI_2);

					if (ptInterDist < TransitionDist)
					{
						ptCut = ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir + ARANMath.C_PI, TransitionDist, 0);

						ptInter = (Point)ARANFunctions.LineLineIntersect(InnerBasePoint, SplayAngle, ptCut, OutDir + ARANMath.C_PI_2);

						//	_UI.DrawPointWithText(ptInter, 0, "ptInter - 2");

					}
					result.Add(ptInter);
				}
			}

			//Ring rr = new Ring();
			//rr.Assign(result);
			//_UI.DrawRing(rr, eFillStyle.sfsHorizontal);
			//ProcessMessages();

			JoinSegments(result, ARP, false, IsPrimary, EntryDir, TurnDir);
			return result;
		}

		virtual protected Ring CreateOuterTurnAreaLT(LegBase PrevLeg, ADHPType ARP, bool IsPrimary, double EntryDir, TurnDirection TurnDir)
		{
			double OutDir = _StartFIX.OutDirection;
			double fSide = (int)TurnDir;
			double TurnAng = ARANMath.Modulus((OutDir - EntryDir) * fSide, ARANMath.C_2xPI);

			if (TurnAng < ARANMath.EpsilonRadian || Math.Abs(TurnAng - ARANMath.C_2xPI) < ARANMath.EpsilonRadian)
				TurnAng = 0.0;

			//double SpiralDivergenceAngle = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;
			//double SplayAngle15, DivergenceAngle30;


			double DivergenceAngle30, SplayAngle15,
				SpiralDivergenceAngle = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value,
				SpiralSplayAngle = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;

			if (IsPrimary && TurnAng < ARANMath.DegToRad(5))
			{
				DivergenceAngle30 = Math.Atan(0.5 * Math.Tan(SpiralDivergenceAngle));
				SplayAngle15 = Math.Atan(0.5 * Math.Tan(SpiralSplayAngle));
			}
			else
			{
				DivergenceAngle30 = SpiralDivergenceAngle;
				SplayAngle15 = SpiralSplayAngle;
			}

			//if (IsPrimary)
			//{
			//	DivergenceAngle30 = SpiralDivergenceAngle;
			//	SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
			//}
			//else
			//{
			//	DivergenceAngle30 = SpiralDivergenceAngle;
			//	SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
			//}


			//ptTmp = ARANFunctions.LocalToPrj(StartFIX.PrjPt, OutDir, -0.5, 0);
			//ptTmp = ARANFunctions.LocalToPrj(StartFIX.PrjPt, EntryDir - ARANMath.C_PI, -0.5, 0);
			//ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, FlyMode == eFlyMode.FlyBy ? EntryDir - ARANMath.C_PI : EntryDir, LPT, 0);
			//ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, -_StartFIX.EPT + 1.0, 0);
			//ptTmp = _StartFIX.PrjPt;

			double LPT, fTmp;
			//if (_StartFIX.SemiWidth < _EndFIX.SemiWidth)
			//	LPT = -_StartFIX.LPT;
			LPT = -_StartFIX.EPT - 1.0;


			//if (_StartFIX.DFTarget)
			//{
			//    if (TurnDir == TurnDirection.CW)
			//        LPT = _StartFIX.LPT_L;
			//    else
			//        LPT = _StartFIX.LPT_R;
			//}
			//else
			//    LPT = _StartFIX.LPT;


			Point ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, LPT, 0);

			if (TurnAng <= 0.034906585039886591) ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, -1, 0);

			GeometryOperators GeoOperators = new GeometryOperators();
			if (PrevLeg != null)
			{
				fTmp = GeoOperators.GetDistance(PrevLeg.PrimaryArea, ptTmp);
				if (fTmp > 0)
					ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, LPT - fTmp - 1.0);
			}

			//_UI.DrawPointWithText(ptTmp, "ptTmp");
			//ProcessMessages();

			//_UI.DrawRing(PrevLeg.PrimaryArea[0].ExteriorRing, 255, eFillStyle.sfsBackwardDiagonal);
			//_UI.DrawPoint(ptTmp, 255);
			//ProcessMessages();

			Point ptFrom = null;
			double ASW_OUT0F, ASW_OUT0C;

			if (TurnDir == TurnDirection.CCW)
			{
				if (IsPrimary)
				{
					ASW_OUT0C = 0.5 * _StartFIX.SemiWidth;
					ASW_OUT0F = _StartFIX.ASW_2_R;

					if (PrevLeg != null && PrevLeg.PrimaryArea.Count > 0)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDir - ARANMath.C_PI_2, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, StartFIX.PrjPt, OutDir - ARANMath.C_PI_2, OutDir, out fTmp);

						if (ptFrom == null)
						{
							ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, -_StartFIX.EPT + 1.0, 0);
							ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDir - ARANMath.C_PI_2, out fTmp);
						}

						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}
				}
				else
				{
					ASW_OUT0C = _StartFIX.SemiWidth;
					ASW_OUT0F = _StartFIX.ASW_R;
					if (PrevLeg != null && PrevLeg.FullArea.Count > 0)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir - ARANMath.C_PI_2, out fTmp);

						//_UI.DrawRing(PrevLeg.FullArea[0].ExteriorRing, 255, eFillStyle.sfsCross);
						//_UI.DrawMultiPolygon(PrevLeg.FullArea, 255, eFillStyle.sfsCross);
						//_UI.DrawPointWithText(StartFIX.PrjPt, 0, "StartFIX");
						//ProcessMessages();
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, StartFIX.PrjPt, OutDir - ARANMath.C_PI_2, OutDir, out fTmp);

						if (ptFrom == null)
						{
							ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, -_StartFIX.EPT + 1.0, 0);
							ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir - ARANMath.C_PI_2, out fTmp);
						}

						if (ptFrom != null)
							ASW_OUT0F = fTmp;

						//_UI.DrawPointWithText(ptTmp, "ptTmp");
						//_UI.DrawPointWithText(ptFrom, "ptFrom");
						//ProcessMessages();
					}
				}
			}
			else
			{
				if (IsPrimary)
				{
					ASW_OUT0C = 0.5 * _StartFIX.SemiWidth;
					ASW_OUT0F = _StartFIX.ASW_2_L;
					if (PrevLeg != null && PrevLeg.PrimaryArea.Count > 0)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDir + ARANMath.C_PI_2, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, StartFIX.PrjPt, OutDir + ARANMath.C_PI_2, OutDir, out fTmp);
						if (ptFrom == null)
						{
							ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, -_StartFIX.EPT + 1.0, 0);
							ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDir + ARANMath.C_PI_2, out fTmp);
						}

						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}
				}
				else
				{
					ASW_OUT0C = _StartFIX.SemiWidth;
					ASW_OUT0F = _StartFIX.ASW_L;
					if (PrevLeg != null && PrevLeg.FullArea.Count > 0)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir + ARANMath.C_PI_2, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, StartFIX.PrjPt, OutDir + ARANMath.C_PI_2, OutDir, out fTmp);
						if (ptFrom == null)
						{
							ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, -_StartFIX.EPT + 1.0, 0);
							ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir + ARANMath.C_PI_2, out fTmp);
						}

						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}
				}
			}

			//_UI.DrawPointWithText(ptFrom, "ptFrom-O1");
			//ProcessMessages();

			//_UI.DrawPointWithText(ptTmp, "ptTmp");
			//_UI.DrawPointWithText(ptFrom, "ptFrom");
			//ProcessMessages();

			Ring result = new Ring();
			if (ptFrom != null)
			{
				result.Add(ptFrom);
			}
			else
			{
				ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir - ARANMath.C_PI, _StartFIX.EPT + 10.0, ASW_OUT0F * fSide);
				//_UI.DrawPointWithText(ptTmp, 0, "ptTmp");
				//ProcessMessages();
				result.Add(ptTmp);
			}

			//if (ptFrom == null)
			//{
			//ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir - ARANMath.C_PI, _StartFIX.EPT + 10.0, ASW_OUT0F * fSide);
			//_UI.DrawPointWithText(ptTmp, 0, "ptTmp");
			//ProcessMessages();
			//result.Add(ptTmp);

			//ptFrom = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir - ARANMath.C_PI, _StartFIX.EPT, ASW_OUT0F * fSide);
			ptFrom = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, 0.0, -fSide * ASW_OUT0F);

			//_UI.DrawPointWithText(ptTmp, "ptTmp");
			//_UI.DrawPointWithText(ptFrom, "ptFrom-1");
			//ProcessMessages();
			//}

			//Point ptTo = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, OutDir - ARANMath.C_PI, _StartFIX.EPT, ASW_OUT0C * fSide);
			Point ptTo = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, OutDir, 0.0, -fSide * ASW_OUT0C);

			//ProcessMessages(true);
			//_UI.DrawPointWithText(StartFIX.PrjPt, -1, "StartFIX");
			//_UI.DrawPointWithText(ptFrom, -1, "ptFrom-O-" + (IsPrimary ? "P" : "S"));
			//_UI.DrawPointWithText(ptTo, -1, "ptTo-O-" + (IsPrimary ? "P" : "S"));
			//_UI.DrawPointWithText(ptTo, -1, "ptTo-O-" + (IsPrimary ? "P" : "S"));
			//_UI.DrawPointWithText(ptTo, "ptTo-O-" + (IsPrimary ? "P" : "S"));
			//ProcessMessages();

			double ptDir = Math.Atan2(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);

			fTmp = ARANMath.Modulus((ptDir - OutDir) * fSide, ARANMath.C_2xPI);     /////////////////////////?????????????????

			if (fTmp > ARANMath.C_PI)
				fTmp = fTmp - ARANMath.C_2xPI;

			double ptDist = ARANMath.Hypot(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);

			if (TurnAng <= 0.034906585039886591)
				result.Add(ptFrom);
			else if (ptDist > 1.0 && fTmp + ARANMath.EpsilonRadian >= -SplayAngle15 && fTmp - ARANMath.EpsilonRadian <= DivergenceAngle30)
			{
				ptTmp = ARANFunctions.LocalToPrj(ptFrom, ptDir, 0.5 * ptDist, 0);

				//_UI.DrawPointWithText(ptTmp, 128+(128<<8), "ptTmp");
				//ProcessMessages();

				Geometry Geom0 = ARANFunctions.LineLineIntersect(ptTmp, ptDir + ARANMath.C_PI_2, ptFrom, EntryDir + ARANMath.C_PI_2);
				Geometry Geom1 = ARANFunctions.LineLineIntersect(ptTmp, ptDir + ARANMath.C_PI_2, ptTo, OutDir + ARANMath.C_PI_2);
				Point ptCnt = null;

				if (Geom0.Type == GeometryType.Point)
				{
					ptCnt = (Point)Geom0;
					//_UI.DrawPointWithText(ptCnt, -1, "ptCnt1");
					//ProcessMessages();
					//_UI.DrawPointWithText((Point)Geom1, 128 + (255 << 8), "ptCnt2");
					//ProcessMessages(true);

					if (Geom1 != null && Geom1.Type == GeometryType.Point)
					{
						double Dist0 = ARANMath.Hypot(ptFrom.Y - ptCnt.Y, ptFrom.X - ptCnt.X);
						double Dist1 = ARANMath.Hypot(ptFrom.Y - ((Point)Geom1).Y, ptFrom.X - ((Point)Geom1).X);
						if (ASW_OUT0F > ASW_OUT0C && Dist1 < Dist0)
							ptCnt.Assign(Geom1);
					}
				}
				else if (Geom1.Type == GeometryType.Point)
					ptCnt = (Point)Geom1;

				if (ptCnt != null)
				{
					Ring tmpRing = ARANFunctions.CreateArcPrj(ptCnt, ptFrom, ptTo, TurnDir);
					//_UI.DrawRing(tmpRing, 0, eFillStyle.sfsSolid);
					//ProcessMessages();

					//	for (int ffg = 0; ffg < tmpRing.Count; ffg++)
					//		_UI.DrawPointWithText(tmpRing[ffg], 128 << 8, "pt" + ffg.ToString());
					//					ProcessMessages(true);

					result.AddMultiPoint(tmpRing);
				}
				else
					result.Add(ptFrom);
			}
			else if (fTmp + ARANMath.EpsilonRadian < -SplayAngle15)
			{
				Geometry Geom0 = ARANFunctions.LineLineIntersect(ptFrom, OutDir - SplayAngle15 * fSide, _StartFIX.PrjPt, OutDir + ARANMath.C_PI_2);
				if (Geom0.Type == GeometryType.Point)
				{
					ptTo = (Point)Geom0;

					////ProcessMessages(true);
					//_UI.DrawPointWithText(StartFIX.PrjPt, -1, "StartFIX");
					//_UI.DrawPointWithText(ptFrom, -1, "ptFrom-O-" + (IsPrimary ? "P" : "S"));
					//_UI.DrawPointWithText(ptTo, -1, "ptTo-O-" + (IsPrimary ? "P" : "S"));
					//ProcessMessages();

					ptDir = Math.Atan2(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);
					ptDist = ARANMath.Hypot(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);

					ptTmp = ARANFunctions.LocalToPrj(ptFrom, ptDir, 0.5 * ptDist, 0);
					Geom0 = ARANFunctions.LineLineIntersect(ptTmp, ptDir + ARANMath.C_PI_2, ptFrom, EntryDir + ARANMath.C_PI_2 - SplayAngle15 * fSide);
					Geometry Geom1 = ARANFunctions.LineLineIntersect(ptTmp, ptDir + ARANMath.C_PI_2, ptTo, OutDir + ARANMath.C_PI_2 - SplayAngle15 * fSide);
					Point ptCnt = null;

					if (Geom0.Type == GeometryType.Point)
					{
						ptCnt = (Point)Geom0;

						if (Geom1 != null && Geom1.Type == GeometryType.Point)
						{
							double Dist0 = ARANMath.Hypot(ptFrom.Y - ptCnt.Y, ptFrom.X - ptCnt.X);
							double Dist1 = ARANMath.Hypot(ptFrom.Y - ((Point)Geom1).Y, ptFrom.X - ((Point)Geom1).X);
							if (ASW_OUT0F > ASW_OUT0C && Dist1 < Dist0)
								ptCnt.Assign(Geom1);
						}
					}
					else if (Geom1.Type == GeometryType.Point)
						ptCnt = (Point)Geom1;

					if (ptCnt != null)
					{
						Ring tmpRing = ARANFunctions.CreateArcPrj(ptCnt, ptFrom, ptTo, TurnDir);
						result.AddMultiPoint(tmpRing);
					}
					else
						result.Add(ptFrom);
				}
				else
					result.Add(ptFrom);
			}
			else
				result.Add(ptFrom);

			//=============================================================================
			//_UI.DrawPointWithText(result[0], -1, "p-1");
			//_UI.DrawPointWithText(result[1], -1, "p-2");
			//ProcessMessages(true);

			//_UI.DrawRing(result, 128 << 8, eFillStyle.sfsCross );
			//ProcessMessages(true);

			JoinSegments(result, ARP, true, IsPrimary, EntryDir, TurnDir);

			//_UI.DrawRing(result, 128 << 8, eFillStyle.sfsCross );
			//ProcessMessages(true);

			//for (int ffg = 0; ffg < result.Count; ffg++)
			//	_UI.DrawPointWithText(result[ffg],128<<8, "pt" +ffg.ToString());
			//ProcessMessages(true);

			return result;
		}

		virtual protected Ring CreateInnerTurnAreaLT(LegBase PrevLeg, ADHPType ARP, bool IsPrimary, double EntryDir, TurnDirection TurnDir)
		{
			double OutDir = _StartFIX.OutDirection;
			double fSide = (int)TurnDir;
			double TurnAng = ARANMath.Modulus((OutDir - EntryDir) * fSide, ARANMath.C_2xPI);

			if (TurnAng < ARANMath.EpsilonRadian || Math.Abs(TurnAng - ARANMath.C_2xPI) < ARANMath.EpsilonRadian)
				TurnAng = 0.0;

			//	DivergenceAngle30 = GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
			//	SplayAngle15 = GPANSOPSConstants.Constant[arafTrn_OSplay].Value;

			double SplayAngle15, DivergenceAngle30, SpiralDivergenceAngle = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;

			if (IsPrimary)
			{
				SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
				DivergenceAngle30 = SpiralDivergenceAngle;

				////	fTmp = GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
				//DivergenceAngle30 = Math.Atan(0.5 * Math.Tan(SpiralDivergenceAngle));
				//fTmp = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
				//SplayAngle15 = Math.Atan(0.5 * Math.Tan(fTmp));
			}
			else
			{
				SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
				DivergenceAngle30 = SpiralDivergenceAngle;
				//_constants.Constant[arSecAreaCutAngl].Value;
			}



			double LPT;

			//if (_StartFIX.IsDFTarget)
			//{
			//	if (TurnDir == TurnDirection.CW)
			//		LPT = _StartFIX.LPT_L;
			//	else
			//		LPT = _StartFIX.LPT_R;
			//}
			//else
			//	LPT = _StartFIX.LPT;

			LPT = -_StartFIX.EPT - 10.0;
			Point ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, LPT);
			double fTmp, ASW_IN0F, ASW_IN0C;

			GeometryOperators GeoOperators = new GeometryOperators();
			if (PrevLeg != null)
			{
				fTmp = GeoOperators.GetDistance(PrevLeg.PrimaryArea, ptTmp);
				if (fTmp > 0)
					ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, LPT - fTmp - 10.0);
			}

			//ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, FlyMode == eFlyMode.FlyBy ? EntryDir - ARANMath.C_PI : EntryDir, LPT, 0);
			//if(PrevLeg!=null)
			//	LPT = -PrevLeg.EndFIX.EPT - 10.0;
			//else

			//_UI.DrawPointWithText(ptTmp, "ptTmp-2");
			//_UI.DrawPointWithText(PrevLeg.EndFIX.PrjPt , "PrevLeg.EndFIX");
			//ProcessMessages();

			Point ptFrom = null;

			if (TurnDir == TurnDirection.CCW)
			{
				if (IsPrimary)
				{
					ASW_IN0C = 0.5 * _StartFIX.SemiWidth;
					ASW_IN0F = _StartFIX.ASW_2_L;
					if (PrevLeg != null && PrevLeg.PrimaryArea.Count > 0)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, OutDir + ARANMath.C_PI_2, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, OutDir + ARANMath.C_PI_2, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_IN0F = fTmp;

						//_UI.DrawPointWithText(ptTmp, "ptTmp");
						//_UI.DrawPointWithText(ptFrom, "ptFrom-I0");
						//ProcessMessages();

					}
				}
				else
				{
					ASW_IN0C = _StartFIX.SemiWidth;
					ASW_IN0F = _StartFIX.ASW_L;
					if (PrevLeg != null && PrevLeg.FullArea.Count > 0)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, OutDir + ARANMath.C_PI_2, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, ptTmp, OutDir + ARANMath.C_PI_2, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_IN0F = fTmp;
					}
				}
			}
			else
			{
				if (IsPrimary)
				{
					ASW_IN0C = 0.5 * _StartFIX.SemiWidth;
					ASW_IN0F = _StartFIX.ASW_2_R;
					if (PrevLeg != null && PrevLeg.PrimaryArea.Count > 0)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, OutDir - ARANMath.C_PI_2, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, OutDir - ARANMath.C_PI_2, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_IN0F = fTmp;
					}
				}
				else
				{
					ASW_IN0C = _StartFIX.SemiWidth;
					ASW_IN0F = _StartFIX.ASW_R;
					if (PrevLeg != null && PrevLeg.FullArea.Count > 0)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, OutDir - ARANMath.C_PI_2, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, ptTmp, OutDir - ARANMath.C_PI_2, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_IN0F = fTmp;
					}
				}
			}

			//	_UI.DrawPointWithText(ptFrom, 0, "ptFrom-I0");
			//	ProcessMessages();

			Ring result = new Ring();

			if (ptFrom == null)
			{
				ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir - ARANMath.C_PI, _StartFIX.EPT + 10.0, -ASW_IN0F * fSide);
				result.Add(ptTmp);
				//_UI.DrawPointWithText(ptTmp, -1, "ptTmp-I-" + (IsPrimary ? "P" : "S"));
				//ProcessMessages();

				ptFrom = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, OutDir - ARANMath.C_PI, _StartFIX.EPT, -ASW_IN0F * fSide);
				//ptFrom = (Point)ARANFunctions.LineLineIntersect(ptTmp, EntryDir, StartFIX.PrjPt, OutDir + ARANMath.C_PI_2);

				//_UI.DrawPointWithText(ptFrom, -1, "ptFrom-I-" + (IsPrimary ? "P" : "S"));
				//ProcessMessages();
			}

			result.Add(ptFrom);

			if (TurnAng > 0.034906585039886591)
			{
				fTmp = ASW_IN0C / Math.Cos(TurnAng);
				Point ptTo = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir + ARANMath.C_PI_2 * fSide, fTmp, 0);//ASW_IN0C

				//_UI.DrawPointWithText(ptTo, -1, "ptTo-I-" + (IsPrimary ? "P" : "S"));
				//ProcessMessages();

				double ptDist = ARANMath.Hypot(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);
				double ptDir = Math.Atan2(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);

				fTmp = ARANMath.Modulus((OutDir - ptDir) * fSide, 2 * ARANMath.C_PI);

				if (fTmp > ARANMath.C_PI)
					fTmp = fTmp - ARANMath.C_2xPI;

				if (ptDist > 1.0 && fTmp > -SplayAngle15 && fTmp < DivergenceAngle30)
					result.Add(ptTo);
				/*
				if IsPrimary then
				begin
					GUI.DrawPointWithText(ptFrom, 255, 'ptFrom');
					GUI.DrawPointWithText(ptTo, 255, 'ptTo');
					GUI.DrawPointWithText(PtTmp, 0, 'ptTmp');
				end;
				*/
			}

			JoinSegments(result, ARP, false, IsPrimary, EntryDir, TurnDir);

			//_UI.DrawPointWithText(result[2], 0, "result[2]");
			//_UI.DrawPointWithText(result[3], 0, "result[3]");
			//ProcessMessages(true);

			return result;
		}

		protected bool createProtectionArea(LegBase PrevLeg, ADHPType ARP)
		{
			Point ptTmp, ptLPTMeasure, ptEPTMeasure;
			Ring InnerRingP, OuterRingP;
			Ring InnerRingB, OuterRingB;

			MultiPolygon PrimaryArePolygon, BufferPolygon;
			MultiPolygon FullArePolygon;
			bool smallAngle = false;

			//if (_StartFIX.)
			_StartFIX.CalcTurnRangePoints(EndFIX.IsDFTarget);
			_EndFIX.CalcTurnRangePoints(false);

			double TurnAngle = _StartFIX.TurnAngle;
			double OutDir = _StartFIX.OutDirection;
			//double OutDir = _EndFIX.EntryDirection;
			double OutDir1, OutDir2;

			if (_EndFIX.IsDFTarget)
			{
				OutDir1 = _StartFIX.OutDirection_L;
				OutDir2 = _StartFIX.OutDirection_R;
			}
			else
				OutDir1 = OutDir2 = OutDir;

			double EntryDir = _StartFIX.EntryDirection;
			double EntryDir1, EntryDir2;
			TurnDirection TurnDir, TurnDir1, TurnDir2;

			if (_StartFIX.IsDFTarget)
			{
				TurnDir = ARANMath.SideToTurn(ARANMath.SideDef(_StartFIX.PrjPt, EntryDir, _EndFIX.PrjPt));

				if (TurnDir == TurnDirection.CW)
				{
					EntryDir1 = _StartFIX.EntryDirection_L;
					EntryDir2 = _StartFIX.EntryDirection_R;
				}
				else
				{
					EntryDir2 = _StartFIX.EntryDirection_L;
					EntryDir1 = _StartFIX.EntryDirection_R;
				}

				TurnDir1 = ARANMath.SideToTurn(ARANMath.SideDef(_StartFIX.PrjPt, EntryDir1, _EndFIX.PrjPt));
				TurnDir2 = ARANMath.SideToTurn(ARANMath.SideDef(_StartFIX.PrjPt, EntryDir2, _EndFIX.PrjPt));
			}
			else
			{
				EntryDir1 = EntryDir;
				EntryDir2 = EntryDir;

				if (EndFIX.IsDFTarget)
					TurnDir = _StartFIX.EffectiveTurnDirection;
				else
				{
					TurnDir = ARANMath.SideFrom2Angle(OutDir, EntryDir);
					if (TurnDir == TurnDirection.NONE)
						TurnDir = _StartFIX.EffectiveTurnDirection;
				}

				TurnDir1 = TurnDir;
				TurnDir2 = TurnDir;
			}

			if (_EndFIX.IsDFTarget)
			{
				OuterRingP = CreateDtFSegment(PrevLeg, true);       //OuterRingP.AddMultiPoint(InnerRingP);
				OuterRingB = CreateDtFSegment(PrevLeg, false);
			}
			else
			{
				//(ARANMath.RadToDeg(TurnAngle) <= 10 || (_StartFIX.FlyMode == eFlyMode.Flyby && _StartFIX.Role <= eFIXRole.IF_ && ARANMath.RadToDeg(TurnAngle) <= 30)))
				if (_StartFIX.FlyMode == eFlyMode.Flyby &&
					((_StartFIX.FlightPhase == eFlightPhase.FAFApch && ARANMath.RadToDeg(TurnAngle) <= 10) ||
					((_StartFIX.FlightPhase == eFlightPhase.STAR || _StartFIX.FlightPhase == eFlightPhase.STARGE56 || _StartFIX.FlightPhase == eFlightPhase.IIAP) && ARANMath.RadToDeg(TurnAngle) <= 30)))
				{
					smallAngle = true;
					if (_StartFIX.IsDFTarget)
					{
						if (TurnDir1 == TurnDir2)
						{
							OuterRingP = CreateOuterTurnAreaLT(PrevLeg, ARP, true, EntryDir1, TurnDir1);
							InnerRingP = CreateInnerTurnAreaLT(PrevLeg, ARP, true, EntryDir2, TurnDir2);

							OuterRingB = CreateOuterTurnAreaLT(PrevLeg, ARP, false, EntryDir1, TurnDir1);
							InnerRingB = CreateInnerTurnAreaLT(PrevLeg, ARP, false, EntryDir2, TurnDir2);
						}
						else
						{
							OuterRingP = CreateInnerTurnAreaLT(PrevLeg, ARP, true, EntryDir1, TurnDir1);
							InnerRingP = CreateInnerTurnAreaLT(PrevLeg, ARP, true, EntryDir2, TurnDir2);

							OuterRingB = CreateInnerTurnAreaLT(PrevLeg, ARP, false, EntryDir1, TurnDir1);
							InnerRingB = CreateInnerTurnAreaLT(PrevLeg, ARP, false, EntryDir2, TurnDir2);
						}
					}
					else
					{
						OuterRingP = CreateOuterTurnAreaLT(PrevLeg, ARP, true, EntryDir, TurnDir);
						InnerRingP = CreateInnerTurnAreaLT(PrevLeg, ARP, true, EntryDir, TurnDir);

						//_UI.DrawRing(OuterRingP, eFillStyle.sfsForwardDiagonal);
						//_UI.DrawRing(InnerRingP, eFillStyle.sfsBackwardDiagonal);
						//ProcessMessages();
						//ProcessMessages(true);

						OuterRingB = CreateOuterTurnAreaLT(PrevLeg, ARP, false, EntryDir, TurnDir);
						InnerRingB = CreateInnerTurnAreaLT(PrevLeg, ARP, false, EntryDir, TurnDir);
						//_UI.DrawRing(OuterRingB, eFillStyle.sfsBackwardDiagonal);
						//ProcessMessages();
					}
				}
				else
				{
					if (_StartFIX.IsDFTarget)
					{
						if (TurnDir1 == TurnDir2)
						{
							OuterRingP = CreateOuterTurnArea(PrevLeg, ARP, true, EntryDir1, TurnDir1);
							InnerRingP = CreateInnerTurnArea(PrevLeg, ARP, true, EntryDir2, TurnDir2);

							//_UI.DrawRing(OuterRingP, eFillStyle.sfsForwardDiagonal);
							//_UI.DrawRing(InnerRingP, eFillStyle.sfsBackwardDiagonal);
							//ProcessMessages();

							OuterRingB = CreateOuterTurnArea(PrevLeg, ARP, false, EntryDir1, TurnDir1);
							InnerRingB = CreateInnerTurnArea(PrevLeg, ARP, false, EntryDir2, TurnDir2);
						}
						else
						{
							OuterRingP = CreateInnerTurnArea(PrevLeg, ARP, true, EntryDir1, TurnDir1);
							InnerRingP = CreateInnerTurnArea(PrevLeg, ARP, true, EntryDir2, TurnDir2);

							OuterRingB = CreateInnerTurnArea(PrevLeg, ARP, false, EntryDir1, TurnDir1);
							InnerRingB = CreateInnerTurnArea(PrevLeg, ARP, false, EntryDir2, TurnDir2);
						}
					}
					else
					{
						OuterRingP = CreateOuterTurnArea(PrevLeg, ARP, true, EntryDir, TurnDir);
						InnerRingP = CreateInnerTurnArea(PrevLeg, ARP, true, EntryDir, TurnDir);

						//_UI.DrawRing(OuterRingP, eFillStyle.sfsForwardDiagonal);
						//_UI.DrawRing(InnerRingP, eFillStyle.sfsBackwardDiagonal);
						//ProcessMessages();

						OuterRingB = CreateOuterTurnArea(PrevLeg, ARP, false, EntryDir, TurnDir);
						InnerRingB = CreateInnerTurnArea(PrevLeg, ARP, false, EntryDir, TurnDir);

						//_UI.DrawRing(OuterRingB, eFillStyle.sfsForwardDiagonal);
						//_UI.DrawRing(InnerRingB, eFillStyle.sfsBackwardDiagonal);
						//ProcessMessages();
					}

					//if (_EndFIX.Role != eFIXRole.MAPt_ && (ARANMath.RadToDeg(_EndFIX.TurnAngle) <= 10 ||
					//    (_EndFIX.FlyMode == eFlyMode.FlyBy && _EndFIX.Role != eFIXRole.FAF_ &&
					//        ARANMath.RadToDeg(_EndFIX.TurnAngle) <= 30)))
					//{
					//    ptTmp = ARANFunctions.LocalToPrj(_EndFIX.PrjPt, _EndFIX.EntryDirection, 0.5, 0);
					//    OuterRingP.Add(ptTmp);		//EndFIX.PrjPt
					//    OuterRingB.Add(ptTmp);		//EndFIX.PrjPt
					//}
				}

				////ProcessMessages(true);
				//_UI.DrawRing(OuterRingP, eFillStyle.sfsForwardDiagonal);
				//_UI.DrawRing(InnerRingP, eFillStyle.sfsBackwardDiagonal);
				//ProcessMessages();

				//_UI.DrawRing(OuterRingB, eFillStyle.sfsHorizontal);
				//_UI.DrawRing(InnerRingB, eFillStyle.sfsVertical);
				//ProcessMessages();

				OuterRingP.AddReverse(InnerRingP);
				OuterRingB.AddReverse(InnerRingB);
			}

			//ProcessMessages(true);
			//_UI.DrawRing(OuterRingB, eFillStyle.sfsVertical);
			//_UI.DrawRing(OuterRingP, eFillStyle.sfsBackwardDiagonal);
			//ProcessMessages();

			GeometryOperators GeoOperators = new GeometryOperators();

			Polygon tmpPolygon = new Polygon();
			tmpPolygon.ExteriorRing = OuterRingP;

			PrimaryArePolygon = new MultiPolygon();
			PrimaryArePolygon.Add(tmpPolygon);

			tmpPolygon = new Polygon();
			tmpPolygon.ExteriorRing = OuterRingB;

			BufferPolygon = new MultiPolygon();
			BufferPolygon.Add(tmpPolygon);

			//ProcessMessages(true);
			//_UI.DrawMultiPolygon(BufferPolygon, eFillStyle.sfsBackwardDiagonal);
			//_UI.DrawMultiPolygon(PrimaryArePolygon, eFillStyle.sfsForwardDiagonal);
			//ProcessMessages();

			//if (_StartFIX.FlyMode == eFlyMode.Atheight)
			//	GeometryOperators GeoOperators = new GeometryOperators();
			//	GeoOperators.Cut(PrevLeg.PrimaryArea, pCutter, out geom1, out geom2);

			if (PrevLeg != null)
			{
				double StartLTP;

				if (_StartFIX.IsDFTarget)
				{
					if (TurnDir == TurnDirection.CW)
						StartLTP = (_StartFIX.FlyMode == eFlyMode.Flyby ? -_StartFIX.LPT_L : _StartFIX.LPT_L);
					else
						StartLTP = (_StartFIX.FlyMode == eFlyMode.Flyby ? -_StartFIX.LPT_R : _StartFIX.LPT_R);
				}
				else
				{
					StartLTP = (_StartFIX.FlyMode == eFlyMode.Flyby ? -_StartFIX.LPT : _StartFIX.LPT);
					if (ARANMath.RadToDeg(TurnAngle) <= 5)
					{
						StartLTP = -_StartFIX.ATT;
						double ASW = TurnDir == TurnDirection.CW ? _StartFIX.ASW_R : _StartFIX.ASW_L;
						if (ASW < _StartFIX.SemiWidth - ARANMath.EpsilonDistance)
							StartLTP = -_StartFIX.ATT;
						else if (ASW > _StartFIX.SemiWidth + ARANMath.EpsilonDistance)
							StartLTP = -_StartFIX.LPT;
					}
				}

				LineString ls = new LineString();

				ls.Add(ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_L, -_StartFIX.EPT_L, 2 * Math.Max(_StartFIX.SemiWidth, 5 * _StartFIX.ASW_L)));

				if (_StartFIX.EPT_L != _StartFIX.EPT_R)
				{
					ls.Add(ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_L, -_StartFIX.EPT_L));
					ls.Add(ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_R, -_StartFIX.EPT_R));
				}

				ls.Add(ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_R, -_StartFIX.EPT_R, -2 * Math.Max(_StartFIX.SemiWidth, 5 * _StartFIX.ASW_R)));

				//_UI.DrawMultiPolygon(PrevLeg.FullArea, eFillStyle.sfsBackwardDiagonal);
				//_UI.DrawMultiPolygon(PrevLeg.PrimaryArea, eFillStyle.sfsBackwardDiagonal);
				//_UI.DrawLineString(ls, 2);
				//ProcessMessages();
				//ProcessMessages(true);

				if (!_EndFIX.IsDFTarget && !GeoOperators.Disjoint(PrevLeg.FullArea, ls) && !GeoOperators.Disjoint(PrevLeg.PrimaryArea, ls))
				{
					Geometry geomL, geomR;

					GeoOperators.Cut(PrevLeg.FullArea, ls, out geomL, out geomR);
					MultiPolygon BufferPolygon1 = (MultiPolygon)geomL;

					////ProcessMessages(true);
					//_UI.DrawMultiPolygon(PrevLeg.FullArea, eFillStyle.sfsBackwardDiagonal);
					//_UI.DrawMultiPolygon(PrevLeg.PrimaryArea, eFillStyle.sfsForwardDiagonal);
					//_UI.DrawLineString(ls, 2);
					//ProcessMessages();

					GeoOperators.Cut(PrevLeg.PrimaryArea, ls, out geomL, out geomR);
					MultiPolygon PrimaryArePolygon1 = (MultiPolygon)geomL;

					//======================
					ls.Clear();

					if (smallAngle)
					{
						ls.Add(ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection, 0.0, 5.0 * (TurnDir == TurnDirection.CW ? -_StartFIX.ASW_R : _StartFIX.ASW_L)));
						ls.Add(ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection, 0.0, 5.0 * (TurnDir == TurnDirection.CW ? _StartFIX.ASW_R : -_StartFIX.ASW_L)));
					}
					else
					{
						Point pt0 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection, -_StartFIX.EPT, (int)TurnDir * _StartFIX.SemiWidth);
						//_UI.DrawPointWithText(pt0, "pt0", -1);
						//ProcessMessages();

						double dir = ARANFunctions.ReturnAngleInRadians(_StartFIX.PrjPt, pt0);
						double dis = ARANFunctions.ReturnDistanceInMeters(_StartFIX.PrjPt, pt0);

						ls.Add(ARANFunctions.LocalToPrj(_StartFIX.PrjPt, dir, 15 * dis));
						ls.Add(_StartFIX.PrjPt);
						if (_StartFIX.FlightPhase == eFlightPhase.STAR || _StartFIX.FlightPhase == eFlightPhase.STARGE56)
							ls.Add(ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection, 0.0, 5.0 * (TurnDir == TurnDirection.CW ? _StartFIX.ASW_R : -_StartFIX.ASW_L)));
						else
						{
							if (TurnDir == TurnDirection.CW)
							{
								ls.Add(ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_L, StartLTP));
								ls.Add(ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_L, StartLTP, 5.0 * _StartFIX.ASW_L));
							}
							else
							{
								ls.Add(ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_R, StartLTP));
								ls.Add(ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_R, StartLTP, -5.0 * _StartFIX.ASW_R));
							}
						}
					}

					////ProcessMessages(true);
					//_UI.DrawMultiPolygon(BufferPolygon1, eFillStyle.sfsBackwardDiagonal);
					//_UI.DrawLineString(ls, 2);
					//ProcessMessages();

					//if (!GeoOperators.Disjoint(BufferPolygon1, ls))
					try
					{
						GeoOperators.Cut(BufferPolygon1, ls, out geomL, out geomR);

						if (TurnDir == TurnDirection.CW)
							BufferPolygon1 = (MultiPolygon)geomL;
						else
							BufferPolygon1 = (MultiPolygon)geomR;

						//_UI.DrawMultiPolygon(BufferPolygon, eFillStyle.sfsDiagonalCross);
						//_UI.DrawLineString(ls, 2);
						//ProcessMessages();

						BufferPolygon = (MultiPolygon)GeoOperators.UnionGeometry(BufferPolygon, BufferPolygon1);
					}
					catch
					{ }

					//ProcessMessages(true);
					//_UI.DrawMultiPolygon(BufferPolygon, eFillStyle.sfsDiagonalCross);
					//ProcessMessages();

					if (!smallAngle)
					{
						ls.Clear();
						Point pt0 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection, -_StartFIX.EPT, 0.5 * (int)TurnDir * _StartFIX.SemiWidth);
						//_UI.DrawPointWithText(pt0, -1, "pt1");
						//ProcessMessages();
						double dir = ARANFunctions.ReturnAngleInRadians(_StartFIX.PrjPt, pt0);
						double dis = ARANFunctions.ReturnDistanceInMeters(_StartFIX.PrjPt, pt0);

						ls.Add(ARANFunctions.LocalToPrj(_StartFIX.PrjPt, dir, 15 * dis));
						ls.Add(_StartFIX.PrjPt);
						if (_StartFIX.FlightPhase == eFlightPhase.STAR || _StartFIX.FlightPhase == eFlightPhase.STARGE56)
							ls.Add(ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection, 0.0, 5.0 * (TurnDir == TurnDirection.CW ? _StartFIX.ASW_R : -_StartFIX.ASW_L)));
						else
						{
							if (TurnDir == TurnDirection.CW)
							{
								ls.Add(ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_L, StartLTP));
								ls.Add(ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_L, StartLTP, 5.0 * _StartFIX.ASW_L));
							}
							else
							{
								ls.Add(ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_R, StartLTP));
								ls.Add(ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_R, StartLTP, -5.0 * _StartFIX.ASW_R));
							}
						}
					}

					////ProcessMessages(true);
					//_UI.DrawMultiPolygon(PrimaryArePolygon1, eFillStyle.sfsForwardDiagonal);
					//_UI.DrawLineString(ls, 2);
					//ProcessMessages();

					//if (!GeoOperators.Disjoint(PrimaryArePolygon1, ls))
					try
					{
						GeoOperators.Cut(PrimaryArePolygon1, ls, out geomL, out geomR);

						if (TurnDir == TurnDirection.CW)
							PrimaryArePolygon1 = (MultiPolygon)geomL;
						else
							PrimaryArePolygon1 = (MultiPolygon)geomR;

						//ProcessMessages(true);
						//_UI.DrawMultiPolygon(BufferPolygon1, eFillStyle.sfsBackwardDiagonal);
						//_UI.DrawMultiPolygon(PrimaryArePolygon1, eFillStyle.sfsForwardDiagonal);
						//ProcessMessages();

						PrimaryArePolygon = (MultiPolygon)GeoOperators.UnionGeometry(PrimaryArePolygon, PrimaryArePolygon1);
					}
					catch
					{ }
				}

				//ProcessMessages(true);
				//_UI.DrawMultiPolygon(PrimaryArePolygon, eFillStyle.sfsForwardDiagonal);
				//_UI.DrawLineString(ls, 2);
				//ProcessMessages();

				if (false && ARANMath.RadToDeg(TurnAngle) < 10)
				{
					MultiPolygon PrimaryArePolygon1 = (MultiPolygon)GeoOperators.UnionGeometry(PrimaryArePolygon, PrevLeg.PrimaryArea);
					MultiPolygon BufferPolygon1 = (MultiPolygon)GeoOperators.UnionGeometry(BufferPolygon, PrevLeg.FullArea);

					PrimaryArePolygon = (MultiPolygon)GeoOperators.Difference(PrimaryArePolygon1, PrevLeg.PrimaryAssesmentArea);
					BufferPolygon = (MultiPolygon)GeoOperators.Difference(BufferPolygon1, PrevLeg.FullAssesmentArea);

					PrimaryArePolygon = (MultiPolygon)GeoOperators.UnionGeometry(PrimaryArePolygon, PrimaryArePolygon1);
					BufferPolygon = (MultiPolygon)GeoOperators.UnionGeometry(BufferPolygon, BufferPolygon1);
				}

				if (_StartFIX.FlyMode == eFlyMode.Atheight)
				{
					if (_EndFIX.IsDFTarget)     // || ARANMath.RadToDeg(TurnAngle) < 30.0)
					{
						//ProcessMessages(true);

						//PrimaryArePolygon = (MultiPolygon)GeoOperators.Difference(PrimaryArePolygon, PrevLeg.FullAssesmentArea);
						PrimaryArePolygon = (MultiPolygon)GeoOperators.Difference(PrimaryArePolygon, PrevLeg.PrimaryAssesmentArea);
						BufferPolygon = (MultiPolygon)GeoOperators.Difference(BufferPolygon, PrevLeg.FullAssesmentArea);
					}
					else if (ARANMath.RadToDeg(TurnAngle) > 15.0)
					{
						PrimaryArePolygon = (MultiPolygon)GeoOperators.UnionGeometry(PrimaryArePolygon, PrevLeg.PrimaryArea);
						BufferPolygon = (MultiPolygon)GeoOperators.UnionGeometry(BufferPolygon, PrevLeg.FullArea);

						//ProcessMessages(true);
						//_UI.DrawMultiPolygon(BufferPolygon, -1, eFillStyle.sfsBackwardDiagonal);
						//_UI.DrawMultiPolygon(PrimaryArePolygon, -1, eFillStyle.sfsForwardDiagonal);
						//ProcessMessages();

						double fMinDist = double.MaxValue, fMaxDist = double.MinValue, fTurnDir = (double)TurnDir;
						Point stPt = null, ptEnd = null;
						SortedList<double, Point> stPoints = new SortedList<double, Point>();
						Ring PrevRing = PrevLeg.PrimaryAssesmentArea[0].ExteriorRing;

						for (int i = 0; i < PrevRing.Count; i++)
						{
							//_UI.DrawPointWithText(PrevRing[i], -1, "----" + (i + 1).ToString());
							//ProcessMessages();

							ptTmp = ARANFunctions.PrjToLocal(PrevLeg._StartFIX.PrjPt, _StartFIX.EntryDirection, PrevRing[i]);

							if (fTurnDir * ptTmp.Y <= ARANMath.EpsilonDistance || stPoints.ContainsKey(ptTmp.X))
								continue;

							stPoints.Add(ptTmp.X, PrevRing[i]);
							if (ptTmp.X > fMaxDist)
							{
								fMaxDist = ptTmp.X;
								ptEnd = PrevRing[i];
							}

							if (ptTmp.X < fMinDist)
							{
								fMinDist = ptTmp.X;
								stPt = PrevRing[i];
							}
						}

						//_UI.DrawPointWithText(stPt, -1, "stPt");
						//_UI.DrawPointWithText(ptEnd, -1, "ptEnd");
						//_UI.DrawPointWithText(_StartFIX.PrjPt, -1, "StartFIX");
						//ProcessMessages();
						//_UI.DrawPointWithText(ptTmp, -1, "ptInter`");
						//ProcessMessages();

						Ring tmpRing = new Ring();

						foreach (var pair in stPoints)
							tmpRing.Add(pair.Value);

						//if (_StartFIX.FlyMode == eFlyMode.Atheight)
						//	ptTmp = (Point)ARANFunctions.LineLineIntersect(_StartFIX.PrjPtH, OutDir, stPt, EntryDir + 0.5 * fTurnDir * TurnAngle);
						//else
						//	ptTmp = (Point)ARANFunctions.LineLineIntersect(_StartFIX.PrjPt, OutDir, stPt, EntryDir + 0.5 * fTurnDir * TurnAngle);

						ptTmp = (Point)ARANFunctions.LineLineIntersect(_EndFIX.PrjPt, OutDir, stPt, EntryDir + 0.5 * fTurnDir * TurnAngle);

						if (ptTmp == null)
							ptTmp = _StartFIX.PrjPt;

						//_UI.DrawPointWithText(ptTmp, -1, "ptTmp--3");
						//ProcessMessages();

						if (ARANMath.SideDef(_StartFIX.PrjPt, EntryDir + ARANMath.C_PI_2, ptTmp) == SideDirection.sideRight)//<= SideDirection.sideOn)
							tmpRing.Add(_StartFIX.PrjPt);
						//tmpRing.Add(_StartFIX.PrjPt);

						//_UI.DrawRing(tmpRing, -1, eFillStyle.sfsHorizontal);
						//ProcessMessages();

						double LPT;
						if (_EndFIX.IsDFTarget)
						{
							if (TurnDir == TurnDirection.CW)
								LPT = (_EndFIX.FlyMode == eFlyMode.Flyby ? -_EndFIX.LPT_L : _EndFIX.LPT_L);
							else
								LPT = (_EndFIX.FlyMode == eFlyMode.Flyby ? -_EndFIX.LPT_R : _EndFIX.LPT_R);
						}
						else
							LPT = (_EndFIX.FlyMode == eFlyMode.Flyby ? -_EndFIX.LPT : _EndFIX.LPT);

						Point ptLPT = ARANFunctions.LocalToPrj(_EndFIX.PrjPt, OutDir, LPT, 0);

						//_UI.DrawPointWithText(ptLPT, -1, "ptLPT");
						//ProcessMessages();

						if (ARANMath.SideDef(ptLPT, OutDir + ARANMath.C_PI_2, ptTmp) == SideDirection.sideRight)
						{
							tmpRing.Add(ptLPT);
							ptTmp = (Point)ARANFunctions.LineLineIntersect(ptLPT, OutDir + ARANMath.C_PI_2, stPt, EntryDir + 0.5 * fTurnDir * TurnAngle);

							//_UI.DrawPointWithText(ptTmp, -1, "ptTmp-1");
							//ProcessMessages();
						}

						tmpRing.Add(ptTmp);

						//_UI.DrawPointWithText(ptTmp, -1, "ptTmp");
						//ProcessMessages();

						tmpPolygon = new Polygon();
						tmpPolygon.ExteriorRing = tmpRing;

						//_UI.DrawPolygon(tmpPolygon, -1, eFillStyle.sfsHorizontal);
						//_UI.DrawMultiPolygon(PrimaryArePolygon, -1, eFillStyle.sfsVertical);
						//ProcessMessages();

						MultiPolygon HeightPrimaryArePolygon = new MultiPolygon();
						HeightPrimaryArePolygon.Add(tmpPolygon);

						//_UI.DrawMultiPolygon(PrimaryArePolygon, -1, eFillStyle.sfsHorizontal);
						//ProcessMessages();

						PrimaryArePolygon = (MultiPolygon)GeoOperators.UnionGeometry(HeightPrimaryArePolygon, PrimaryArePolygon);
					}
				}
			}

			//ProcessMessages(true);
			//_UI.DrawMultiPolygon(BufferPolygon, eFillStyle.sfsBackwardDiagonal);
			//_UI.DrawMultiPolygon(PrimaryArePolygon, eFillStyle.sfsForwardDiagonal);
			//ProcessMessages();

			FullArePolygon = (MultiPolygon)GeoOperators.UnionGeometry(BufferPolygon, PrimaryArePolygon);

			JtsGeometryOperators fullGeoOp = new Geometries.Operators.JtsGeometryOperators();
			PrimaryArePolygon = (MultiPolygon)fullGeoOp.SimplifyGeometry(PrimaryArePolygon);
			FullArePolygon = (MultiPolygon)fullGeoOp.SimplifyGeometry(FullArePolygon);

			//ProcessMessages(true);
			//_UI.DrawMultiPolygon(FullArePolygon, eFillStyle.sfsBackwardDiagonal);
			//_UI.DrawMultiPolygon(PrimaryArePolygon, eFillStyle.sfsForwardDiagonal);
			//ProcessMessages();

			this.PrimaryArea = PrimaryArePolygon;
			this.FullArea = FullArePolygon;

			TurnDir = _EndFIX.TurnDirection;

			if (TurnDir == TurnDirection.NONE)
			{
				ptLPTMeasure = (Point)_EndFIX.PrjPt.Clone();
				ptEPTMeasure = (Point)_EndFIX.PrjPt.Clone();
				TurnDir = TurnDirection.CW;
			}
			else
			{
				ptLPTMeasure = ARANFunctions.LocalToPrj(_EndFIX.PrjPt, OutDir - (_EndFIX.FlyMode == eFlyMode.Flyby ? ARANMath.C_PI : 0), _EndFIX.LPT, 0);
				ptEPTMeasure = ARANFunctions.LocalToPrj(_EndFIX.PrjPt, OutDir - ARANMath.C_PI, _EndFIX.EPT, 0);
			}

			//====================================================================================================================
			double fSide = (int)TurnDir, ASW_OUT1F = 0.0, ASW_IN1F = 0.0;

			if (PrimaryArePolygon.Count > 0)
			{
				ptTmp = ARANFunctions.RingVectorIntersect(PrimaryArePolygon[0].ExteriorRing, ptEPTMeasure, OutDir - ARANMath.C_PI_2 * fSide, out ASW_IN1F);

				if (ptTmp != null)
				{
					if (TurnDir == TurnDirection.CCW)
						_EndFIX.ASW_2_L = ASW_IN1F;
					else
						_EndFIX.ASW_2_R = ASW_IN1F;
				}

				ptTmp = ARANFunctions.RingVectorIntersect(PrimaryArePolygon[0].ExteriorRing, ptLPTMeasure, OutDir + ARANMath.C_PI_2 * fSide, out ASW_OUT1F);
				if (ptTmp != null)
				{
					if (TurnDir == TurnDirection.CCW)
						_EndFIX.ASW_2_R = ASW_OUT1F;
					else
						_EndFIX.ASW_2_L = ASW_OUT1F;
				}
			}
			//====================================================================================================================

			if (FullArePolygon.Count > 0)
			{
				ptTmp = ARANFunctions.RingVectorIntersect(FullArePolygon[0].ExteriorRing, ptEPTMeasure, OutDir - ARANMath.C_PI_2 * fSide, out ASW_IN1F);
				if (ptTmp != null)
				{
					if (TurnDir == TurnDirection.CCW)
						_EndFIX.ASW_L = ASW_IN1F;
					else
						_EndFIX.ASW_R = ASW_IN1F;
				}

				ptTmp = ARANFunctions.RingVectorIntersect(FullArePolygon[0].ExteriorRing, ptLPTMeasure, OutDir + ARANMath.C_PI_2 * fSide, out ASW_OUT1F);

				//_UI.DrawMultiPolygon(FullArePolygon, eFillStyle.sfsForwardDiagonal);
				//_UI.DrawPointWithText(ptTmp, "ptX");
				//_UI.DrawPointWithText(ptLPTMeasure, "ptLPTM");
				//ProcessMessages();

				if (ptTmp != null)
				{
					if (TurnDir == TurnDirection.CCW)
						_EndFIX.ASW_R = ASW_OUT1F;
					else
						_EndFIX.ASW_L = ASW_OUT1F;
				}
			}
			//====================================================================================================================

			//ProcessMessages(true);
			//_UI.DrawMultiPolygon(PrevLeg.FullArea, eFillStyle.sfsBackwardDiagonal);
			//_UI.DrawMultiPolygon(PrevLeg.PrimaryArea, eFillStyle.sfsForwardDiagonal);
			//ProcessMessages();
			//ProcessMessages(true);

			return true;
		}

		protected void createAssesmentAreaForDF(LegBase PrevLeg)
		{
			double EntryDir = _StartFIX.EntryDirection;
			double EntryDir1, EntryDir2;
			double Dir0, Dir1;

			TurnDirection TurnDir, TurnDir1, TurnDir2;
			TurnDir = ARANMath.SideToTurn(ARANMath.SideDef(_StartFIX.PrjPt, EntryDir, _EndFIX.PrjPt));

			if (TurnDir == TurnDirection.CW)
			{
				EntryDir1 = _StartFIX.EntryDirection_L;
				EntryDir2 = _StartFIX.EntryDirection_R;

				Dir0 = EntryDir2 - ARANMath.C_PI_2;
				Dir1 = EntryDir1 + ARANMath.C_PI_2;
			}
			else
			{
				EntryDir2 = _StartFIX.EntryDirection_L;
				EntryDir1 = _StartFIX.EntryDirection_R;
				Dir0 = EntryDir1 - ARANMath.C_PI_2;
				Dir1 = EntryDir2 + ARANMath.C_PI_2;
			}

			TurnDir1 = ARANMath.SideToTurn(ARANMath.SideDef(_StartFIX.PrjPt, EntryDir1, _EndFIX.PrjPt));
			TurnDir2 = ARANMath.SideToTurn(ARANMath.SideDef(_StartFIX.PrjPt, EntryDir2, _EndFIX.PrjPt));

			double OutDir = _StartFIX.OutDirection;
			double LineLen = 10 * _EndFIX.ASW_R + _EndFIX.ASW_L;
			double TurnAngle = _StartFIX.TurnAngle;

			double OutDir1;
			double OutDir2;

			if (_EndFIX.IsDFTarget)
			{
				if (TurnDir == TurnDirection.CW)
				{
					OutDir1 = _StartFIX.OutDirection_L;
					OutDir2 = _StartFIX.OutDirection_R;
				}
				else
				{
					OutDir2 = _StartFIX.OutDirection_L;
					OutDir1 = _StartFIX.OutDirection_R;
				}
			}
			else
			{
				OutDir1 = OutDir;
				OutDir2 = OutDir;
			}

			Point PtTmp1, PtTmp0, PtTmp2, PtTmp3;
			LineString KKLinePart = new LineString();

			PtTmp1 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_R + ARANMath.C_PI, _StartFIX.EPT_R, 0);
			PtTmp0 = ARANFunctions.LocalToPrj(PtTmp1, Dir0, LineLen, 0);    //startFIX.ASW_L + 0.15

			PtTmp2 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_L + ARANMath.C_PI, _StartFIX.EPT_L, 0);
			PtTmp3 = ARANFunctions.LocalToPrj(PtTmp2, Dir1, LineLen, 0);    //startFIX.ASW_R + 0.15

			KKLinePart.Add(PtTmp0);
			KKLinePart.Add(PtTmp1);
			KKLinePart.Add(PtTmp2);
			KKLinePart.Add(PtTmp3);

			MultiPolygon FullPoly, PrimPoly;
			GeometryOperators GeoOperators = new GeometryOperators();
			Ring PrevPrimaryRing, PrevFullRing, PrimaryRing, FullRing;
			Polygon tmpPoly;

			LineString K1K1LinePart = new LineString();

			//_StartFIX.DFTarget || 
			if (ARANMath.RadToDeg(TurnAngle) <= 10 || (_StartFIX.FlyMode == eFlyMode.Flyby && _StartFIX.Role != eFIXRole.FAF_ && ARANMath.RadToDeg(TurnAngle) <= 30))
			{
				PtTmp0 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_R + ARANMath.C_PI, 0, LineLen);
				PtTmp1 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_L + ARANMath.C_PI, 0, -LineLen);

				K1K1LinePart.Add(PtTmp0);
				K1K1LinePart.Add(_StartFIX.PrjPt);
				K1K1LinePart.Add(PtTmp1);

				Ring tmpRing1, tmpRing2;
				MultiPolygon tmpMultiPoly;

				ARANFunctions.CutRingByLine(PrevLeg.FullArea[0].ExteriorRing, KKLinePart, out PrevFullRing, out FullRing);
				if (FullRing != null)
				{
					//ARANFunctions.CutRingByNNLine(FullRing, K1K1LinePart, out tmpRing1, out tmpRing2);
					ARANFunctions.CutRingByLine(FullRing, K1K1LinePart, out tmpRing1, out tmpRing2);

					tmpPoly = new Polygon();
					tmpPoly.ExteriorRing = tmpRing1;

					FullPoly = new MultiPolygon();
					FullPoly.Add(tmpPoly);

					this.FullAssesmentArea = (MultiPolygon)GeoOperators.UnionGeometry(this.FullArea, FullPoly);
				}

				ARANFunctions.CutRingByLine(PrevLeg.PrimaryArea[0].ExteriorRing, KKLinePart, out PrevPrimaryRing, out PrimaryRing);

				if (PrimaryRing != null)
				{
					//ARANFunctions.CutRingByNNLine(PrimaryRing, K1K1LinePart, out tmpRing1, out tmpRing2);
					ARANFunctions.CutRingByLine(PrimaryRing, K1K1LinePart, out tmpRing1, out tmpRing2);

					tmpPoly = new Polygon();
					tmpPoly.ExteriorRing = tmpRing1;

					PrimPoly = new MultiPolygon();
					PrimPoly.Add(tmpPoly);

					this.PrimaryAssesmentArea = (MultiPolygon)GeoOperators.UnionGeometry(this.PrimaryArea, PrimPoly);
				}

				//=================================================================================

				if (PrevFullRing != null)
				{
					tmpPoly = new Polygon();
					tmpPoly.ExteriorRing = PrevFullRing;

					tmpMultiPoly = new MultiPolygon();
					tmpMultiPoly.Add(tmpPoly);

					PrevLeg.FullAssesmentArea = tmpMultiPoly;
				}
				else
					PrevLeg.FullAssesmentArea = PrevLeg.FullArea;


				if (PrevPrimaryRing != null)
				{
					tmpPoly = new Polygon();
					tmpPoly.ExteriorRing = PrevPrimaryRing;

					tmpMultiPoly = new MultiPolygon();
					tmpMultiPoly.Add(tmpPoly);

					PrevLeg.PrimaryAssesmentArea = tmpMultiPoly;
				}
				else
					PrevLeg.PrimaryAssesmentArea = PrevLeg.PrimaryArea;

				//========================================================================================

				Geometry geom1, geom2;

				try
				{
					GeoOperators.Cut(this.FullAssesmentArea, KKLinePart, out geom1, out geom2);

					MultiPolygon tmpPoly1 = (MultiPolygon)geom1;
					MultiPolygon tmpPoly2 = (MultiPolygon)geom2;

					if (!tmpPoly1.IsEmpty && !tmpPoly2.IsEmpty)
					{
						this.FullAssesmentArea = tmpPoly2;
						PrevLeg.FullAssesmentArea = (MultiPolygon)GeoOperators.UnionGeometry(PrevLeg.FullAssesmentArea, tmpPoly1);
					}
				}
				catch
				{
				}

				try
				{
					GeoOperators.Cut(this.PrimaryAssesmentArea, KKLinePart, out geom1, out geom2);

					MultiPolygon tmpPoly1 = (MultiPolygon)geom1;
					MultiPolygon tmpPoly2 = (MultiPolygon)geom2;

					if (!tmpPoly1.IsEmpty && !tmpPoly2.IsEmpty)
					{
						this.PrimaryAssesmentArea = tmpPoly2;
						PrevLeg.PrimaryAssesmentArea = (MultiPolygon)GeoOperators.UnionGeometry(PrevLeg.PrimaryAssesmentArea, tmpPoly1);
					}
				}
				catch
				{
				}

			}
			else
			{
				if (TurnDir1 != TurnDir2)
				{
					PtTmp0 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_R + ARANMath.C_PI, 0.5 * _StartFIX.EPT_R, LineLen);
					PtTmp1 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_R + ARANMath.C_PI, 0.5 * _StartFIX.EPT_R, 0);
					PtTmp2 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_L + ARANMath.C_PI, 0.5 * _StartFIX.EPT_L, 0);
					PtTmp3 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_L + ARANMath.C_PI, 0.5 * _StartFIX.EPT_L, -LineLen);
				}
				else if (TurnDir1 == TurnDirection.CW)
				{
					PtTmp0 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_R + ARANMath.C_PI, _StartFIX.EPT_R, LineLen);
					PtTmp1 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_R + ARANMath.C_PI, _StartFIX.EPT_R, 0);
					PtTmp2 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_L + ARANMath.C_PI, _StartFIX.LPT_L, 0);
					PtTmp3 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_L + ARANMath.C_PI, _StartFIX.LPT_L, -LineLen);
				}
				else
				{
					PtTmp0 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_R + ARANMath.C_PI, _StartFIX.LPT_R, LineLen);
					PtTmp1 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_R + ARANMath.C_PI, _StartFIX.LPT_R, 0);
					PtTmp2 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_L + ARANMath.C_PI, _StartFIX.EPT_L, 0);
					PtTmp3 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection_L + ARANMath.C_PI, _StartFIX.EPT_L, -LineLen);
				}

				K1K1LinePart.Add(PtTmp0);
				K1K1LinePart.Add(PtTmp1);
				K1K1LinePart.Add(PtTmp2);
				K1K1LinePart.Add(PtTmp3);

				ARANFunctions.CutRingByLine(PrevLeg.FullArea[0].ExteriorRing, KKLinePart, out PrevFullRing, out FullRing);
				ARANFunctions.CutRingByLine(PrevLeg.PrimaryArea[0].ExteriorRing, KKLinePart, out PrevPrimaryRing, out PrimaryRing);
				//_UI.DrawRing(PrevFullRing, -1, eFillStyle.sfsVertical);
				//_UI.DrawRing(FullRing, -1, eFillStyle.sfsForwardDiagonal);
				//ProcessMessages();

				//_UI.DrawRing(PrevPrimaryRing, -1, eFillStyle.sfsHorizontal);
				//_UI.DrawRing(PrimaryRing, -1, eFillStyle.sfsBackwardDiagonal);
				//ProcessMessages();

				if (FullRing != null)
				{
					Geometry geom1, geom2;
					tmpPoly = new Polygon();
					tmpPoly.ExteriorRing = FullRing;
					MultiPolygon FullRingPoly = new MultiPolygon();
					FullRingPoly.Add(tmpPoly);

					try
					{
						GeoOperators.Cut(FullRingPoly, K1K1LinePart, out geom1, out geom2);

						//_UI.DrawMultiPolygon((MultiPolygon)geom1, -1, eFillStyle.sfsForwardDiagonal);
						//_UI.DrawMultiPolygon((MultiPolygon)geom2, -1, eFillStyle.sfsVertical);
						//_UI.DrawLineString(K1K1LinePart, -1, 2);
						//ProcessMessages();

						MultiPolygon tmpPoly1 = (MultiPolygon)geom1;//, tmpPoly2 = (MultiPolygon)geom2;

						if (!tmpPoly1.IsEmpty)
							FullPoly = tmpPoly1;
						else
							FullPoly = FullRingPoly;
					}
					catch
					{
						FullPoly = FullRingPoly;
					}

					this.FullAssesmentArea = (MultiPolygon)GeoOperators.UnionGeometry(FullPoly, this.FullArea);
					this.FullArea.Assign(this.FullAssesmentArea);
				}
				else
					this.FullAssesmentArea = (MultiPolygon)this.FullArea;

				if (PrimaryRing != null)
				{
					Geometry geom1, geom2;
					tmpPoly = new Polygon();
					tmpPoly.ExteriorRing = PrimaryRing;
					MultiPolygon PrimaryRingPoly = new MultiPolygon();
					PrimaryRingPoly.Add(tmpPoly);

					try
					{
						GeoOperators.Cut(PrimaryRingPoly, K1K1LinePart, out geom1, out geom2);

						//_UI.DrawMultiPolygon((MultiPolygon)geom1, -1, eFillStyle.sfsBackwardDiagonal);
						//_UI.DrawMultiPolygon((MultiPolygon)geom2, -1, eFillStyle.sfsHorizontal);
						//_UI.DrawLineString(K1K1LinePart, -1, 2);
						//ProcessMessages();

						MultiPolygon tmpPoly1 = (MultiPolygon)geom1;


						if (!tmpPoly1.IsEmpty)
							PrimPoly = tmpPoly1;
						else
							PrimPoly = PrimaryRingPoly;
					}
					catch
					{
						PrimPoly = PrimaryRingPoly;
					}

					this.PrimaryAssesmentArea = (MultiPolygon)GeoOperators.UnionGeometry(PrimPoly, this.PrimaryArea);
					this.PrimaryArea.Assign(this.PrimaryAssesmentArea);
				}
				else
					this.PrimaryAssesmentArea = this.PrimaryArea;

				if (PrevFullRing != null)
				{
					tmpPoly = new Polygon();
					tmpPoly.ExteriorRing = PrevFullRing;

					FullPoly = new MultiPolygon();
					FullPoly.Add(tmpPoly);
					PrevLeg.FullAssesmentArea = FullPoly;
				}

				if (PrevPrimaryRing != null)
				{
					tmpPoly = new Polygon();
					tmpPoly.ExteriorRing = PrevPrimaryRing;

					PrimPoly = new MultiPolygon();
					PrimPoly.Add(tmpPoly);
					PrevLeg.PrimaryAssesmentArea = PrimPoly;
				}
			}
		}

		protected void createAssesmentArea(LegBase PrevLeg, bool isPbn = false)
		{
			if (PrevLeg == null)
			{
				this.FullAssesmentArea = (MultiPolygon)this.FullArea;
				this.PrimaryAssesmentArea = (MultiPolygon)this.PrimaryArea;
				return;
			}

			if (_StartFIX.IsDFTarget)
			{
				createAssesmentAreaForDF(PrevLeg);
				return;
			}


			double EntryDir = _StartFIX.EntryDirection;
			double OutDir = _StartFIX.OutDirection; //_EndFIX.EntryDirection; //
			double EntryDir1, EntryDir2;
			double Dir0, Dir1;

			TurnDirection TurnDir, TurnDir1, TurnDir2;

			if (EndFIX.IsDFTarget)
				TurnDir = _StartFIX.EffectiveTurnDirection;
			else
			{
				TurnDir = ARANMath.SideFrom2Angle(OutDir, EntryDir);
				if (TurnDir == TurnDirection.NONE)
					TurnDir = _StartFIX.EffectiveTurnDirection;
			}

			TurnDir1 = TurnDir;
			TurnDir2 = TurnDir;

			EntryDir1 = EntryDir;
			EntryDir2 = EntryDir;

			Dir0 = EntryDir - ARANMath.C_PI_2;
			Dir1 = EntryDir + ARANMath.C_PI_2;

			//_UI.DrawPointWithText(_EndFIX.PrjPt, "_EndFIX");
			//ProcessMessages();

			double LineLen = 10.0 * Math.Abs(_EndFIX.ASW_R) + Math.Abs(_EndFIX.ASW_L);
			double TurnAngle = _StartFIX.TurnAngle;

			double OutDir1;
			double OutDir2;

			if (_EndFIX.IsDFTarget)
			{
				if (TurnDir == TurnDirection.CW)
				{
					OutDir1 = _StartFIX.OutDirection_L;
					OutDir2 = _StartFIX.OutDirection_R;
				}
				else
				{
					OutDir2 = _StartFIX.OutDirection_L;
					OutDir1 = _StartFIX.OutDirection_R;
				}
			}
			else
			{
				OutDir1 = OutDir;
				OutDir2 = OutDir;
			}

			Point PtTmp1, PtTmp0, PtTmp2, PtTmp3;
			if (isPbn)
				PtTmp1 = _StartFIX.PrjPt;
			else
				PtTmp1 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir + ARANMath.C_PI, _StartFIX.EPT, 0.0);

			PtTmp0 = ARANFunctions.LocalToPrj(PtTmp1, Dir0, LineLen, 0);    //startFIX.ASW_L + 0.15
			PtTmp2 = ARANFunctions.LocalToPrj(PtTmp1, Dir1, LineLen, 0);    //startFIX.ASW_R + 0.15

			LineString KKLinePart = new LineString();
			KKLinePart.Add(PtTmp0);
			//KKLinePart.Add(PtTmp1);
			KKLinePart.Add(PtTmp2);

			//_UI.DrawPointWithText(PtTmp0, "PtTmp0");
			//_UI.DrawPointWithText(PtTmp2, "PtTmp2");
			//_UI.DrawLineString(KKLinePart, 2);
			//ProcessMessages();

			MultiPolygon FullPoly, PrimPoly;
			GeometryOperators GeoOperators = new GeometryOperators();
			Ring PrevPrimaryRing, PrevFullRing, PrimaryRing, FullRing;
			Polygon tmpPoly;

			LineString K1K1LinePart = new LineString();

			if (ARANMath.RadToDeg(TurnAngle) <= 10 || (_StartFIX.FlyMode == eFlyMode.Flyby && _StartFIX.Role != eFIXRole.FAF_ && ARANMath.RadToDeg(TurnAngle) <= 30))
			{
				if (TurnDir == TurnDirection.CW)
				{
					PtTmp0 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, OutDir + ARANMath.C_PI, 0, LineLen);
					PtTmp1 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir + ARANMath.C_PI, 0, -LineLen);
				}
				else
				{
					PtTmp0 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir + ARANMath.C_PI, 0, LineLen);
					PtTmp1 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, OutDir + ARANMath.C_PI, 0, -LineLen);
				}

				K1K1LinePart.Add(PtTmp0);
				K1K1LinePart.Add(_StartFIX.PrjPt);
				K1K1LinePart.Add(PtTmp1);

				PtTmp1 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir + ARANMath.C_PI, _StartFIX.EPT + 0.25, 0);
				PtTmp0 = ARANFunctions.LocalToPrj(PtTmp1, Dir0, LineLen, 0);        //startFIX.ASW_L + 0.15
				PtTmp2 = ARANFunctions.LocalToPrj(PtTmp1, Dir1, LineLen, 0);        //startFIX.ASW_R + 0.15

				LineString KKOffsetLine = new LineString();
				KKOffsetLine.Add(PtTmp0);
				KKOffsetLine.Add(PtTmp1);
				KKOffsetLine.Add(PtTmp2);

				Ring PrevPrimaryRing1, PrevFullRing1, PrimaryRing1, FullRing1;
				Ring tmpRing1, tmpRing2;
				MultiPolygon tmpMultiPoly;

				//ARANFunctions.CutRingByNNLine(PrevLeg.FullArea[0].ExteriorRing, K1K1LinePart, out PrevFullRing, out FullRing);
				ARANFunctions.CutRingByLine(PrevLeg.FullArea[0].ExteriorRing, K1K1LinePart, out PrevFullRing, out FullRing);

				//_UI.DrawLineString(KKOffsetLine, 2);
				//_UI.DrawLineString(KKLinePart,  2);
				//_UI.DrawLineString(K1K1LinePart,  2);
				//_UI.DrawPointWithText(PtTmp1, "PtTmp1");

				//_UI.DrawRing(this.FullAssesmentArea[0].ExteriorRing, eFillStyle.sfsForwardDiagonal);
				//_UI.DrawRing(PrevLeg.FullArea[0].ExteriorRing, eFillStyle.sfsForwardDiagonal);

				////ProcessMessages(true);
				//_UI.DrawRing(PrevFullRing, -1, eFillStyle.sfsForwardDiagonal);
				//_UI.DrawRing(FullRing, -1, eFillStyle.sfsBackwardDiagonal);
				//ProcessMessages();

				if (PrevFullRing != null)
				{
					//ARANFunctions.CutRingByNNLine(PrevFullRing, KKOffsetLine, out tmpRing1, out tmpRing2);
					ARANFunctions.CutRingByLine(PrevFullRing, KKOffsetLine, out tmpRing1, out tmpRing2);

					//_UI.DrawLineString(KKOffsetLine, 2);
					//ProcessMessages();

					if (tmpRing2 != null)
					{
						tmpPoly = new Polygon();
						tmpPoly.ExteriorRing = tmpRing2;

						FullPoly = new MultiPolygon();
						FullPoly.Add(tmpPoly);

						this.FullAssesmentArea = (MultiPolygon)GeoOperators.UnionGeometry(this.FullArea, FullPoly);

						//ProcessMessages(true);
						//_UI.DrawMultiPolygon(this.FullArea, eFillStyle.sfsForwardDiagonal);
						//ProcessMessages();
						//_UI.DrawMultiPolygon(FullPoly, eFillStyle.sfsBackwardDiagonal);
						//ProcessMessages();
						//_UI.DrawMultiPolygon(this.FullAssesmentArea, eFillStyle.sfsForwardDiagonal);
						//ProcessMessages();
						//ProcessMessages(true);
					}
				}

				//ARANFunctions.CutRingByNNLine(PrevLeg.PrimaryArea[0].ExteriorRing, K1K1LinePart, out PrevPrimaryRing, out PrimaryRing);
				ARANFunctions.CutRingByLine(PrevLeg.PrimaryArea[0].ExteriorRing, K1K1LinePart, out PrevPrimaryRing, out PrimaryRing);

				//_UI.DrawRing(PrevPrimaryRing, -1, eFillStyle.sfsForwardDiagonal);
				//_UI.DrawRing(PrimaryRing, -1, eFillStyle.sfsBackwardDiagonal);
				//_UI.DrawMultiPolygon(this.FullAssesmentArea, eFillStyle.sfsForwardDiagonal);
				//ProcessMessages();

				if (PrevPrimaryRing != null)
				{
					//ARANFunctions.CutRingByNNLine(PrevPrimaryRing, KKOffsetLine, out tmpRing1, out tmpRing2);
					ARANFunctions.CutRingByLine(PrevPrimaryRing, KKOffsetLine, out tmpRing1, out tmpRing2);

					if (tmpRing2 != null)
					{
						tmpPoly = new Polygon();
						tmpPoly.ExteriorRing = tmpRing2;

						PrimPoly = new MultiPolygon();
						PrimPoly.Add(tmpPoly);

						this.PrimaryAssesmentArea = (MultiPolygon)GeoOperators.UnionGeometry(this.PrimaryArea, PrimPoly);

						//_UI.DrawMultiPolygon(this.PrimaryAssesmentArea, eFillStyle.sfsForwardDiagonal);
						//ProcessMessages();
					}
				}

				//=================================================================================

				LineString MaPtLinePart = new LineString();

				if (_StartFIX.Role == eFIXRole.FAF_)
				{
					PtTmp0 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, 0.0, -LineLen);
					PtTmp2 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, 0.0, LineLen);
				}
				else
				{
					PtTmp0 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, _StartFIX.ATT, -LineLen);    //startFIX.ASW_L + 0.15
					PtTmp2 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, _StartFIX.ATT, LineLen);    //startFIX.ASW_R + 0.15
				}

				MaPtLinePart.Add(PtTmp0);
				MaPtLinePart.Add(PtTmp2);

				int i;
				double maxArea = 0.0;

				for (i = 0; i < this.FullAssesmentArea.Count; i++)
				{
					if (this.FullAssesmentArea[i].Area > maxArea)
						maxArea = this.FullAssesmentArea[i].Area;
				}

				i = 0;
				while (i < this.FullAssesmentArea.Count)
				{
					if (this.FullAssesmentArea[i].Area < maxArea)
						this.FullAssesmentArea.Remove(i);
					else
						i++;
				}


				ARANFunctions.CutRingByLine(this.FullAssesmentArea[0].ExteriorRing, KKLinePart, out PrevFullRing1, out FullRing1);
				//_UI.DrawRing(PrevFullRing1, eFillStyle.sfsVertical);
				//_UI.DrawMultiPolygon(FullPoly, eFillStyle.sfsForwardDiagonal);

				//_UI.DrawMultiPolygon(FullAssesmentArea, eFillStyle.sfsForwardDiagonal);
				//_UI.DrawLineString(KKLinePart, 2);
				//ProcessMessages();
				if (_StartFIX.Role != eFIXRole.MAPt_ && _StartFIX.Role != eFIXRole.FAF_)
				{
					if (PrevFullRing1 != null)  //&& (PrevFullRing1.Area / PrevFullRing1.Length > 10))
					{
						tmpPoly = new Polygon();
						tmpPoly.ExteriorRing = PrevFullRing1;

						tmpMultiPoly = new MultiPolygon();
						tmpMultiPoly.Add(tmpPoly);

						FullPoly = (MultiPolygon)GeoOperators.UnionGeometry(tmpMultiPoly, PrevLeg.FullArea);
						ARANFunctions.CutRingByLine(FullPoly[0].ExteriorRing, KKLinePart, out PrevFullRing, out FullRing);
					}
				}
				else
				{
					FullPoly = (MultiPolygon)GeoOperators.UnionGeometry(FullAssesmentArea, PrevLeg.FullArea);
					ARANFunctions.CutRingByLine(FullPoly[0].ExteriorRing, MaPtLinePart, out PrevFullRing, out FullRing);
				}

				//ProcessMessages(true);
				//_UI.DrawRing(FullRing, eFillStyle.sfsHorizontal);
				//_UI.DrawRing(PrevFullRing, eFillStyle.sfsVertical);
				//_UI.DrawMultiPolygon(tmpMultiPoly, eFillStyle.sfsHorizontal);
				//_UI.DrawMultiPolygon(FullPoly, eFillStyle.sfsForwardDiagonal);
				//ProcessMessages();


				if (PrevFullRing != null && !PrevFullRing.IsEmpty)
				{
					tmpPoly = new Polygon();
					tmpPoly.ExteriorRing = PrevFullRing;

					tmpMultiPoly = new MultiPolygon();
					tmpMultiPoly.Add(tmpPoly);

					PrevLeg.FullAssesmentArea = tmpMultiPoly;
				}

				if (_StartFIX.Role != eFIXRole.PBN_MATF_LT_28 && FullRing1 != null && !FullRing1.IsEmpty)
				{
					tmpPoly = new Polygon();
					tmpPoly.ExteriorRing = FullRing1;

					FullPoly = new MultiPolygon();
					FullPoly.Add(tmpPoly);

					//ProcessMessages(true);
					//_UI.DrawMultiPolygon(FullPoly, eFillStyle.sfsForwardDiagonal);
					//ProcessMessages();

					this.FullAssesmentArea = FullPoly;
					this.FullArea.Assign(this.FullAssesmentArea);
				}


				maxArea = 0.0;

				for (i = 0; i < this.PrimaryAssesmentArea.Count; i++)
				{
					if (this.PrimaryAssesmentArea[i].Area > maxArea)
						maxArea = this.PrimaryAssesmentArea[i].Area;
				}

				i = 0;
				while (i < this.PrimaryAssesmentArea.Count)
				{
					if (this.PrimaryAssesmentArea[i].Area < maxArea)
						this.PrimaryAssesmentArea.Remove(i);
					else
						i++;
				}

				ARANFunctions.CutRingByLine(this.PrimaryAssesmentArea[0].ExteriorRing, KKLinePart, out PrevPrimaryRing1, out PrimaryRing1);

				if (_StartFIX.Role != eFIXRole.MAPt_ && _StartFIX.Role != eFIXRole.FAF_)
				{
					if (PrevPrimaryRing1 != null)   // && (PrevPrimaryRing1.Area / PrevPrimaryRing1.Length > 10))
					{
						tmpPoly = new Polygon();
						tmpPoly.ExteriorRing = PrevPrimaryRing1;

						tmpMultiPoly = new MultiPolygon();
						tmpMultiPoly.Add(tmpPoly);

						PrimPoly = (MultiPolygon)GeoOperators.UnionGeometry(tmpMultiPoly, PrevLeg.PrimaryArea);
						ARANFunctions.CutRingByLine(PrimPoly[0].ExteriorRing, KKLinePart, out PrevPrimaryRing, out PrimaryRing);
					}
				}
				else
				{
					PrimPoly = (MultiPolygon)GeoOperators.UnionGeometry(PrimaryAssesmentArea, PrevLeg.PrimaryArea);
					ARANFunctions.CutRingByLine(PrimPoly[0].ExteriorRing, MaPtLinePart, out PrevPrimaryRing, out PrimaryRing);
				}

				if (PrevPrimaryRing != null && !PrevPrimaryRing.IsEmpty)
				{
					tmpPoly = new Polygon();
					tmpPoly.ExteriorRing = PrevPrimaryRing;

					tmpMultiPoly = new MultiPolygon();
					tmpMultiPoly.Add(tmpPoly);
					PrevLeg.PrimaryAssesmentArea = tmpMultiPoly;
				}

				if (_StartFIX.Role != eFIXRole.PBN_MATF_LT_28 && PrimaryRing1 != null && !PrimaryRing1.IsEmpty)
				{
					tmpPoly = new Polygon();
					tmpPoly.ExteriorRing = PrimaryRing1;

					PrimPoly = new MultiPolygon();
					PrimPoly.Add(tmpPoly);

					this.PrimaryAssesmentArea = PrimPoly;
					this.PrimaryArea.Assign(this.PrimaryAssesmentArea);
				}
			}
			else //========================================================================================
			{
				double LPT = (_StartFIX.FlyMode == eFlyMode.Flyby ? -_StartFIX.LPT : _StartFIX.LPT);

				if (TurnDir == TurnDirection.CW)
				{
					PtTmp0 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, -_StartFIX.EPT, LineLen);
					PtTmp1 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, -_StartFIX.EPT, 0);
					PtTmp2 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, LPT, 0);
					PtTmp3 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, LPT, LineLen);
				}
				else
				{
					PtTmp0 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, LPT + 1, -LineLen);
					PtTmp1 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, LPT, 0);
					PtTmp2 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, -_StartFIX.EPT, 0);
					PtTmp3 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, -_StartFIX.EPT - 1, -LineLen);
				}

				K1K1LinePart.Clear();   //?????

				K1K1LinePart.Add(PtTmp0);
				K1K1LinePart.Add(PtTmp1);
				K1K1LinePart.Add(PtTmp2);
				K1K1LinePart.Add(PtTmp3);

				ARANFunctions.CutRingByLine(PrevLeg.FullArea[0].ExteriorRing, KKLinePart, out PrevFullRing, out FullRing);
				ARANFunctions.CutRingByLine(PrevLeg.PrimaryArea[0].ExteriorRing, KKLinePart, out PrevPrimaryRing, out PrimaryRing);

				//ProcessMessages(true);
				//_UI.DrawLineString(KKLinePart, 2);
				//_UI.DrawPointWithText(KKLinePart.Start, "ptSt");
				//_UI.DrawPointWithText(PtTmp0, "PtTmp0");
				//ProcessMessages();
				//_UI.DrawLineString(K1K1LinePart, 2);

				//_UI.DrawMultiPolygon(PrevLeg.PrimaryAssesmentArea, eFillStyle.sfsBackwardDiagonal);
				//_UI.DrawMultiPolygon(PrevLeg.FullAssesmentArea, eFillStyle.sfsForwardDiagonal);
				//ProcessMessages();

				//_UI.DrawRing(FullArea[0].ExteriorRing, -1, eFillStyle.sfsDiagonalCross);
				//_UI.DrawRing(PrevFullRing, eFillStyle.sfsBackwardDiagonal);
				//_UI.DrawRing(FullRing, eFillStyle.sfsForwardDiagonal);
				//ProcessMessages();

				//JtsGeometryOperators fullGeoOp = new Geometries.Operators.JtsGeometryOperators();
				//ProcessMessages(true);

				if (FullRing != null)
				{
					tmpPoly = new Polygon();
					tmpPoly.ExteriorRing = FullRing;
					MultiPolygon FullRingPoly = new MultiPolygon();
					FullRingPoly.Add(tmpPoly);

					if (_StartFIX.IsDFTarget)
						FullPoly = FullRingPoly;
					else
					{
						Geometry geom1 = null, geom2;

						try
						{
							GeoOperators.Cut(FullRingPoly, K1K1LinePart, out geom1, out geom2);
							FullPoly = (MultiPolygon)geom1;
							if (FullPoly == null)
								FullPoly = FullRingPoly;
						}
						catch
						{
							FullPoly = FullRingPoly;
						}
					}

					//ProcessMessages(true);
					//_UI.DrawMultiPolygon(FullPoly, eFillStyle.sfsBackwardDiagonal);
					//_UI.DrawMultiPolygon(this.FullArea, eFillStyle.sfsForwardDiagonal);
					//ProcessMessages();

					this.FullAssesmentArea = (MultiPolygon)GeoOperators.UnionGeometry(FullPoly, this.FullArea);

					//ProcessMessages(true);
					//_UI.DrawMultiPolygon(this.FullAssesmentArea, eFillStyle.sfsBackwardDiagonal);
					//ProcessMessages();

					this.FullArea.Assign(this.FullAssesmentArea);
				}
				else
					this.FullAssesmentArea = (MultiPolygon)this.FullArea;

				if (PrimaryRing != null)
				{
					tmpPoly = new Polygon();
					tmpPoly.ExteriorRing = PrimaryRing;
					MultiPolygon PrimaryRingPoly = new MultiPolygon();
					PrimaryRingPoly.Add(tmpPoly);

					if (_StartFIX.IsDFTarget)
						PrimPoly = PrimaryRingPoly;
					else
					{
						Geometry geom1 = null, geom2;

						try
						{
							GeoOperators.Cut(PrimaryRingPoly, K1K1LinePart, out geom1, out geom2);
							PrimPoly = (MultiPolygon)geom1;
							if (PrimPoly == null)
								PrimPoly = PrimaryRingPoly;
						}
						catch
						{
							PrimPoly = PrimaryRingPoly;
						}
					}

					this.PrimaryAssesmentArea = (MultiPolygon)GeoOperators.UnionGeometry(PrimPoly, this.PrimaryArea);

					//_UI.DrawMultiPolygon(this.PrimaryAssesmentArea, eFillStyle.sfsForwardDiagonal);
					//ProcessMessages();

					this.PrimaryArea.Assign(this.PrimaryAssesmentArea);
				}
				else
					this.PrimaryAssesmentArea = (MultiPolygon)this.PrimaryArea.Clone();

				if (_StartFIX.Role == eFIXRole.FAF_ && !isPbn)
				{
					for (int i = 0; i < KKLinePart.Count; i++)
					{
						Point pt = KKLinePart[i];
						pt = ARANFunctions.LocalToPrj(pt, EntryDir, _StartFIX.EPT);
						KKLinePart[i] = pt;
					}

					ARANFunctions.CutRingByLine(PrevLeg.FullArea[0].ExteriorRing, KKLinePart, out PrevFullRing, out FullRing);
					ARANFunctions.CutRingByLine(PrevLeg.PrimaryArea[0].ExteriorRing, KKLinePart, out PrevPrimaryRing, out PrimaryRing);

					Geometry geom1, geom2;
					MultiPolygon tmpMPoly, tmpMPoly0;

					try
					{
						GeoOperators.Cut(this.FullArea, KKLinePart, out geom1, out geom2);

						tmpMPoly = (MultiPolygon)geom1;

						if (tmpMPoly != null)
						{
							//tmpMPoly0 = new MultiPolygon();
							tmpPoly = new Polygon();
							tmpPoly.ExteriorRing = PrevFullRing;
							tmpMPoly0 = (MultiPolygon)GeoOperators.UnionGeometry(tmpPoly, tmpMPoly);

							if (!tmpMPoly0.IsEmpty)
								PrevFullRing = tmpMPoly0[0].ExteriorRing;
						}
					}
					catch (Exception ex)
					{
					}

					//ARANFunctions.CutRingByLine(this.PrimaryArea[0].ExteriorRing, KKLinePart, out PrevPrimaryRing, out PrimaryRing);
					try
					{
						GeoOperators.Cut(this.PrimaryArea, KKLinePart, out geom1, out geom2);

						tmpMPoly = (MultiPolygon)geom1;

						if (tmpMPoly != null)
						{
							tmpPoly = new Polygon();
							tmpPoly.ExteriorRing = PrevPrimaryRing;
							tmpMPoly0 = (MultiPolygon)GeoOperators.UnionGeometry(tmpPoly, tmpMPoly);
							if (!tmpMPoly0.IsEmpty)
								PrevPrimaryRing = tmpMPoly0[0].ExteriorRing;
						}
					}
					catch (Exception ex)
					{
					}
				}

				//_UI.DrawLineString(KKLinePart, 2);
				//ProcessMessages();

				if (PrevFullRing != null)
				{
					tmpPoly = new Polygon();
					tmpPoly.ExteriorRing = PrevFullRing;

					FullPoly = new MultiPolygon();
					FullPoly.Add(tmpPoly);
					PrevLeg.FullAssesmentArea = FullPoly;
				}

				if (PrevPrimaryRing != null)
				{
					tmpPoly = new Polygon();
					tmpPoly.ExteriorRing = PrevPrimaryRing;

					PrimPoly = new MultiPolygon();
					PrimPoly.Add(tmpPoly);
					PrevLeg.PrimaryAssesmentArea = PrimPoly;
				}
			}
		}

		public MultiLineString FullProtectionRingOutline(Ring ring)
		{
			const double Eps = 0.00001;

			double sina = Math.Sin(_StartFIX.OutDirection);
			double cosa = Math.Cos(_StartFIX.OutDirection);

			double sinb = Math.Sin(_EndFIX.EntryDirection);
			double cosb = Math.Cos(_EndFIX.EntryDirection);

			double v3xs = -sina;
			double v3ys = cosa;

			double v3xe = -sinb;
			double v3ye = cosb;

			MultiLineString result = new MultiLineString();

			Ring ringL = ring.RemoveAgnails();

			int n = ringL.Count;

			int st = n - 1;
			int p1 = st;

			for (int i = 0; i < n; i++)
			{
				int p0 = p1;
				p1 = i;

				Point ptl0 = ringL[p0];
				Point ptl1 = ringL[p1];

				double v2x = ptl1.X - ptl0.X;
				double v2y = ptl1.Y - ptl0.Y;

				double v2v3 = v2x * v3xs + v2y * v3ys;

				if (v2v3 >= -Eps && v2v3 <= Eps)
					continue;

				double v1x = _StartFIX.PrjPt.X - ptl0.X;
				double v1y = _StartFIX.PrjPt.Y - ptl0.Y;

				double t1 = (v2x * v1y - v2y * v1x) / v2v3;
				double t2 = (v1x * v3xs + v1y * v3ys) / v2v3;

				if (t1 <= _StartFIX.ATT && t2 >= 0.0 && t2 <= 1.0)
				{
					double v0v2 = v2x * cosa + v2y * sina;
					if (v0v2 > -Eps && v0v2 < Eps)
					{
						st = p1;
						break;
					}
				}
			}

			LineString tmpLine = new LineString();

			p1 = st;
			for (int i = 0; i < n; i++)
			{
				bool bInterrupt = false;
				int p0 = p1;
				p1 = (i + st + 1) % n;

				Point ptl0 = ringL[p0];
				Point ptl1 = ringL[p1];

				double v2x = ptl1.X - ptl0.X;
				double v2y = ptl1.Y - ptl0.Y;

				double v2v3 = v2x * v3xs + v2y * v3ys;

				if (v2v3 < -Eps || v2v3 > Eps)
				{
					double v1x = _StartFIX.PrjPt.X - ptl0.X;
					double v1y = _StartFIX.PrjPt.Y - ptl0.Y;

					double t2 = (v1x * v3xs + v1y * v3ys) / v2v3;
					double t1 = (v2x * v1y - v2y * v1x) / v2v3;

					if (t1 <= _StartFIX.ATT && t2 >= 0.0 && t2 <= 1.0)
					{
						double v0v2 = v2x * cosa + v2y * sina;
						bInterrupt = Math.Abs(v0v2) < Eps;
					}
				}

				if (!bInterrupt)
				{
					v2v3 = v2x * v3xe + v2y * v3ye;
					if (v2v3 < -Eps || v2v3 > Eps)
					{
						double v1x = _EndFIX.PrjPt.X - ptl0.X;
						double v1y = _EndFIX.PrjPt.Y - ptl0.Y;

						double t2 = (v1x * v3xe + v1y * v3ye) / v2v3;
						double t1 = (v2x * v1y - v2y * v1x) / v2v3;

						if (t1 >= -_EndFIX.ATT && t2 >= 0.0 && t2 <= 1.0)
						{
							double v0v2 = v2x * cosb + v2y * sinb;
							bInterrupt = Math.Abs(v0v2) < Eps;
						}
					}
				}

				//_UI.DrawPointWithText(ptl0, "pt-3");
				//_UI.DrawPointWithText(ptl1, "pt-4");
				//ProcessMessages();

				if (!bInterrupt)
					tmpLine.Add(ptl0);
				else if (tmpLine.Count > 0)
				{
					tmpLine.Add(ptl0);
					result.Add(tmpLine);

					tmpLine = new LineString();
				}
			}

			if (tmpLine.Count > 1)
				result.Add(tmpLine);

			return result;
		}

		public MultiLineString ProtectionPolygonOutline(Polygon poly)
		{
			MultiLineString result = FullProtectionRingOutline(poly.ExteriorRing);

			for (int i = 0; i < poly.InteriorRingList.Count; i++)
			{
				MultiLineString tmpPartial = FullProtectionRingOutline(poly.InteriorRingList[i]);
				for (int j = 0; j < tmpPartial.Count; j++)
					result.Add(tmpPartial[j]);
			}

			return result;
		}

		public MultiLineString FullProtectionAreaOutline()
		{
			if (_FullAreaS.Count == 0)
				return new MultiLineString();

			MultiLineString result = ProtectionPolygonOutline(_FullAreaS[0]);

			for (int i = 1; i < _FullAreaS.Count; i++)
			{
				MultiLineString tmpPartial = ProtectionPolygonOutline(_FullAreaS[i]);
				for (int j = 0; j < tmpPartial.Count; j++)
					result.Add(tmpPartial[j]);
			}

			return result;
		}

		//public MultiLineString FullProtectionAreaBorder()
		//{
		//	return ARANFunctions.CreateOutline(_StartFIX.PrjPt, _StartFIX.OutDirection, _EndFIX.PrjPt, _EndFIX.EntryDirection, _FullAreaS);

		//	Polygon geom = ARANFunctions.RemoveAgnails(_FullAreaS[0]);

		//	MultiLineString polyLine = new MultiLineString();
		//	int n = geom.ExteriorRing.Count;

		//	if (n > 1 && ARANFunctions.ReturnDistanceInMeters(geom.ExteriorRing[0], geom.ExteriorRing[n - 1]) < ARANMath.EpsilonDistance)
		//	{
		//		n--;
		//		geom.ExteriorRing.Remove(n);
		//	}

		//	if (n > 0)
		//	{
		//		LineString lineString = new LineString();
		//		for (int k = 0; k <= n; k++)
		//		{
		//			int i = k % n;
		//			int j = (i + 1) % n;

		//			double ang = ARANFunctions.ReturnAngleInRadians(geom.ExteriorRing[i], geom.ExteriorRing[j]);

		//			double sub = ARANMath.SubtractAngles(ang, _StartFIX.OutDirection + 0.5 * Math.PI);

		//			if (sub < ARANMath.EpsilonRadian || Math.Abs(sub - Math.PI) < ARANMath.EpsilonRadian)
		//			{
		//				if (lineString.Count > 1)
		//				{
		//					lineString.Add(geom.ExteriorRing[i]);
		//					polyLine.Add(lineString);
		//					lineString = new LineString();
		//				}
		//				else
		//					lineString.Clear();
		//			}
		//			else
		//				lineString.Add(geom.ExteriorRing[i]);
		//		}

		//		polyLine.Add(lineString);
		//	}

		//	foreach (Ring ring in geom.InteriorRingList)
		//	{
		//		n = ring.Count;
		//		if (n > 1 && ARANFunctions.ReturnDistanceInMeters(ring[0], ring[n - 1]) < ARANMath.EpsilonDistance)
		//		{
		//			n--;
		//			ring.Remove(n);
		//		}

		//		if (n > 0)
		//		{
		//			LineString lineString = new LineString();

		//			for (int i = 0; i < n; i++)
		//			{
		//				int j = (i + 1) % n;

		//				double ang = ARANFunctions.ReturnAngleInRadians(ring[i], ring[j]);
		//				if (ARANMath.SubtractAngles(ang, _StartFIX.OutDirection) > ARANMath.EpsilonRadian)
		//					lineString.Add(ring[i]);
		//				else
		//				{
		//					polyLine.Add(lineString);
		//					lineString = new LineString();
		//					lineString.Add(ring[i]);
		//				}
		//			}

		//			polyLine.Add(lineString);
		//		}
		//	}

		//	return polyLine;
		//}

		//public MultiLineString FullProtectionAreaBorder(LegBase leg)
		//{
		//	MultiLineString polyLine = new MultiLineString();

		//	foreach (Aran.Geometries.Polygon geom in leg._FullAreaS)
		//	{
		//		int n = geom.ExteriorRing.Count;

		//		if (n > 1 && ARANFunctions.ReturnDistanceInMeters(geom.ExteriorRing[0], geom.ExteriorRing[n - 1]) < ARANMath.EpsilonDistance)
		//		{
		//			n--;
		//			geom.ExteriorRing.Remove(n);
		//		}

		//		if (n > 0)
		//		{
		//			LineString lineString = new LineString();
		//			for (int k = 0; k <= n; k++)
		//			{
		//				int i = k % n;
		//				int j = (i + 1) % n;

		//				double ang = ARANFunctions.ReturnAngleInRadians(geom.ExteriorRing[i], geom.ExteriorRing[j]);

		//				double sub = ARANMath.SubtractAngles(ang, _StartFIX.OutDirection + 0.5 * Math.PI);

		//				if (sub < ARANMath.EpsilonRadian || Math.Abs(sub - Math.PI) < ARANMath.EpsilonRadian)
		//				{
		//					if (lineString.Count > 1)
		//					{
		//						lineString.Add(geom.ExteriorRing[i]);
		//						polyLine.Add(lineString);
		//						lineString = new LineString();
		//					}
		//					else
		//						lineString.Clear();
		//				}
		//				else
		//					lineString.Add(geom.ExteriorRing[i]);
		//			}

		//			polyLine.Add(lineString);
		//		}

		//		foreach (Ring ring in geom.InteriorRingList)
		//		{
		//			n = ring.Count;
		//			if (n > 1 && ARANFunctions.ReturnDistanceInMeters(ring[0], ring[n - 1]) < ARANMath.EpsilonDistance)
		//			{
		//				n--;
		//				ring.Remove(n);
		//			}

		//			if (n > 0)
		//			{
		//				LineString lineString = new LineString();

		//				for (int i = 0; i < n; i++)
		//				{
		//					int j = (i + 1) % n;

		//					double ang = ARANFunctions.ReturnAngleInRadians(ring[i], ring[j]);
		//					if (ARANMath.SubtractAngles(ang, _StartFIX.OutDirection) > ARANMath.EpsilonRadian)
		//						lineString.Add(ring[i]);
		//					else
		//					{
		//						polyLine.Add(lineString);
		//						lineString = new LineString();
		//						lineString.Add(ring[i]);
		//					}
		//				}

		//				polyLine.Add(lineString);
		//			}
		//		}

		//	}
		//	return polyLine;
		//}
	}
}
