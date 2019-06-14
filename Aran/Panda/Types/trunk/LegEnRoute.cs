#define UseSmallAngle

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
	public class EnRouteLeg
	{
		public const double minTurnAngle = 5.0;        //0.52359877559829882

		protected static Aran.PANDA.Constants.Constants _constants = null;
		protected WayPoint _StartFIX, _EndFIX;

		//int _PrimaryAreaEl, _FullAreaEl, _NomTrackEl;
		int _PrimaryAreaSMEl, _FullAreaSMEl;

		double _MinLegLength;
		double _IAS, _Gradient;

		IAranGraphics _UI;
		IAranEnvironment _aranEnv;

		MultiPolygon _PrimaryArea, _FullArea, _PrimaryAreaS,
				_FullAreaS, _PrimaryAreaM, _FullAreaM;

		LineString _NNSecondLine, _NomTrack;
		Geometry _KKLine;

		MultiLineString _AssesmentAreaOutline;

		FillSymbol _SecondaryAreaSymbol, _PrimaryAreSymbol;
		LineSymbol _NNSecondSymbol, _NomTrackSymbol;

		FillSymbol _SecondaryAreaSymbolNA, _PrimaryAreSymbolNA;
		LineSymbol _NNSecondSymbolNA, _NomTrackSymbolNA;

		bool _active;
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

		CodeDirection _direction;
		public CodeDirection Direction
		{
			get { return _direction; }
			set
			{
				if (value != _direction)
				{
					_direction = value;
					RefreshGraphics();
				}
			}
		}

		public CodeDirection Granted;

		public WayPoint StartFIX { get { return _StartFIX; } }
		public WayPoint EndFIX { get { return _EndFIX; } }

		//ObstacleType _DetObstacle;
		//public ObstacleType DetObstacle
		//{
		//	get { return _DetObstacle; }
		//	set { _DetObstacle = value; }
		//}

		ObstacleData _DetObstacle_2;
		public ObstacleData DetObstacle_2
		{
			get { return _DetObstacle_2; }
			set { _DetObstacle_2 = value; }
		}

		//ObstacleType[] _Obstacles;
		//public ObstacleType[] Obstacles
		//{
		//	get { return _Obstacles; }
		//	set { _Obstacles = value; }
		//}

		ObstacleContainer _Obstacles_2;
		public ObstacleContainer Obstacles_2
		{
			get { return _Obstacles_2; }
			set { _Obstacles_2 = value; }
		}

		//#if DEBUG
		//		public static void ProcessMessages(bool continuous = false)
		//		{
		//			do
		//				System.Windows.Forms.Application.DoEvents();
		//			while (continuous);
		//		}
		//#endif
		public EnRouteLeg(WayPoint FIX0, WayPoint FIX1, CodeDirection direction, IAranEnvironment aranEnv)
		{
			if (_constants == null)
				_constants = new Aran.PANDA.Constants.Constants();

			_aranEnv = aranEnv;
			_UI = _aranEnv.Graphics;

			//_currLeg.Backwardleg.StartFIX.Assign(_currLeg.Start);
			//_currLeg.Backwardleg.EndFIX.Assign(_currLeg.End);
			_StartFIX = (WayPoint)FIX0.Clone();
			_EndFIX = (WayPoint)FIX1.Clone();

			//_PrimaryAreaEl = -1;
			//_FullAreaEl = -1;

			_FullAreaSMEl = -1;
			_PrimaryAreaSMEl = -1;

			//FNNSecondLineEl = -1;
			//_NomTrackEl = -1;

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
			_direction = direction;

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

			//CreateNomTrack(PrevLeg);
		}

		public void CreateKKLine(EnRouteLeg PrevLeg = null) //, CodeDirection direction = (CodeDirection)(-1)
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

			//_UI.DrawRing(FullRing, -1, eFillStyle.sfsCross);
			//_UI.DrawLineString(kkLS, -1, 2);
			//LegBase.ProcessMessages();

			_KKLine = kkLS;
		}

		void CreateNNSecondLine(EnRouteLeg PrevLeg)
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
				Polygon FullPoly = (Polygon)geomOperators.UnionGeometry(PrevLeg._FullArea, _FullArea);
				FullRing.Assign(FullPoly.ExteriorRing);
			}
			else
				FullRing.Assign(_FullArea[0].ExteriorRing);

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
			//_UI.SafeDeleteGraphic(_PrimaryAreaEl);
			//_UI.SafeDeleteGraphic(_FullAreaEl);

			_UI.SafeDeleteGraphic(_FullAreaSMEl);
			_UI.SafeDeleteGraphic(_PrimaryAreaSMEl);

			////  _UI.SafeDeleteGraphic(_NNSecondLineEl);
			////	_UI.SafeDeleteGraphic(_KKLineEl)

			//_UI.SafeDeleteGraphic(_NomTrackEl);

			////	_StartFIX.DeleteGraphics();
			//_EndFIX.DeleteGraphics();
		}

		public void RefreshGraphics()
		{
			//_UI.SafeDeleteGraphic(_PrimaryAreaEl);
			//_UI.SafeDeleteGraphic(_FullAreaEl);

			_UI.SafeDeleteGraphic(_FullAreaSMEl);
			_UI.SafeDeleteGraphic(_PrimaryAreaSMEl);
			//	_UI.SafeDeleteGraphic(_NNSecondLineEl);
			//	_UI.SafeDeleteGraphic(_KKLineEl);
			//_UI.SafeDeleteGraphic(_NomTrackEl);

			//_PrimaryAreaEl = _UI.DrawMultiPolygon(_FullArea, ARANFunctions.RGB(0, 255, 0), eFillStyle.sfsBackwardDiagonal);
			//_FullAreaEl = _UI.DrawMultiPolygon(_PrimaryArea, ARANFunctions.RGB(0, 255, 0), eFillStyle.sfsForwardDiagonal);

			if (_active)
			{
				_FullAreaSMEl = _UI.DrawMultiPolygon(_FullAreaS, _SecondaryAreaSymbol);
				_PrimaryAreaSMEl = _UI.DrawMultiPolygon(_PrimaryAreaS, _PrimaryAreSymbol);

				//	_NNSecondLineEl = _UI.DrawPolyline(_NNSecondLine, _NNSecondSymbol);
				// _KKLineEl = _UI.DrawPolyline(_KKLine, _KKLineSymbol);

				//_NomTrackEl = _UI.DrawLineString(_NomTrack, _NomTrackSymbol);
			}
			else
			{
				_FullAreaSMEl = _UI.DrawMultiPolygon(_FullAreaS, _SecondaryAreaSymbolNA);
				_PrimaryAreaSMEl = _UI.DrawMultiPolygon(_PrimaryAreaS, _PrimaryAreSymbolNA);

				//	_NNSecondLineEl = _UI.DrawPolyline(_NNSecondLine, _NNSecondSymbol);
				// _KKLineEl = _UI.DrawPolyline(_KKLine, _KKLineSymbol);

				//_NomTrackEl = _UI.DrawLineString(_NomTrack, _NomTrackSymbolNA);
			}

			//_EndFIX.RefreshGraphics();
		}

		//double CalcDRNomDir()
		//{
		//	double fTurnDirection = -(int)(_StartFIX.TurnDirection);
		//	double r = _StartFIX.CalcTurnRadius();

		//	Point ptCenter = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, _StartFIX.EntryDirection, 0, -r * fTurnDirection);

		//	double dx = _EndFIX.PrjPt.X - ptCenter.X;
		//	double dy = _EndFIX.PrjPt.Y - ptCenter.Y;

		//	double dirDest = Math.Atan2(dy, dx);
		//	double distDest = ARANMath.Hypot(dy, dx);

		//	double TurnAngle = (_StartFIX.EntryDirection - dirDest) * fTurnDirection + ARANMath.C_PI_2 - Math.Acos(r / distDest);
		//	return ARANMath.Modulus(_StartFIX.EntryDirection - TurnAngle * fTurnDirection, ARANMath.C_2xPI);
		//}

		public void CreateNomTrack(EnRouteLeg PrevLeg = null)
		{
			Point ptTmp, PtTmp0;//, PtTmp1, PtTmp2, PtTmp3;
								//double DivergenceAngle, R2;

			double DivergenceAngle30 = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;
			double Bank15 = _constants.Pansops[ePANSOPSData.rnvFlyOInterBank].Value;

			double EntryDir = _StartFIX.EntryDirection;
			double OutDir = _EndFIX.EntryDirection;

			double fTurnDir = (int)_StartFIX.EffectiveTurnDirection;            //SideDef(_StartFIX.PrjPt, EntryDir, _EndFIX.PrjPt);
			double TurnAngle;

			if (Math.Abs(OutDir - EntryDir) < ARANMath.EpsilonRadian)
				TurnAngle = 0.0;
			else
			{
				TurnAngle = ARANMath.Modulus((OutDir - EntryDir) * fTurnDir, ARANMath.C_2xPI);

				if (Math.Abs(TurnAngle - ARANMath.C_2xPI) < ARANMath.EpsilonRadian)
					TurnAngle = 0.0;
			}
			//TurnAngle = (_StartFIX.EntryDirection - NomDIr)* fTurnDir;
			//TurnAngle = Modulus((_StartFIX.EntryDirection - EntryDir)* fTurnDirection, C_2xPI);

			double R1 = _StartFIX.ConstructTurnRadius;
			double fTmp = ARANMath.Modulus(TurnAngle, ARANMath.C_PI);

			MultiPoint TrackPoints = new MultiPoint();

			if (Math.Abs(TurnAngle) < ARANMath.DegToRad(0.5))
			{
				ptTmp = (Point)_StartFIX.PrjPt.Clone();
				ptTmp.M = _StartFIX.EntryDirection;
				TrackPoints.Add(ptTmp);

				ptTmp = (Point)_EndFIX.PrjPt.Clone();
				ptTmp.M = OutDir;
				TrackPoints.Add(ptTmp);
			}
			else //if (_StartFIX.FlyMode == eFlyMode.Flyby)
			{
				double L1 = R1 * Math.Sin(TurnAngle);

				PtTmp0 = ARANFunctions.PointAlongPlane(_StartFIX.PrjPt, EntryDir + Math.PI, L1);
				PtTmp0.M = EntryDir;
				TrackPoints.Add(PtTmp0);

				PtTmp0 = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir + TurnAngle * fTurnDir, L1, 0);
				PtTmp0.M = EntryDir + TurnAngle * fTurnDir;
				TrackPoints.Add(PtTmp0);

				PtTmp0 = (Point)_EndFIX.PrjPt.Clone();
				PtTmp0.M = OutDir;
				TrackPoints.Add(PtTmp0);                //	Point6
			}

			//_UI.DrawPointWithText(_StartFIX.PrjPt, -1, "StartFIX");
			//_UI.DrawPointWithText(_EndFIX.PrjPt, -1, "EndFIX");
			//LegBase.ProcessMessages();

			_NomTrack = ARANFunctions.ConvertPointsToTrackLIne(TrackPoints);

			//_UI.DrawLineString(_NomTrack, 255 << 8, 2);
			//for (int I = 0; I < TrackPoints.Count; I++)
			//    _UI.DrawPointWithText(TrackPoints[I], 255 << 14, "Pt_" + (I + 1).ToString());

			//_UI.DrawLineString(_NomTrack, 255 << 8, 2);
			//_UI.DrawLineString(PrevLeg._NomTrack, 255, 2);
			//LegBase.ProcessMessages();

			if (PrevLeg != null && PrevLeg._NomTrack != null)
				PrevLeg._NomTrack[PrevLeg._NomTrack.Count - 1].Assign(_NomTrack[0]);
		}

		public void CreateGeometry(EnRouteLeg PrevLeg, CodeDirection direction)
		{
			//CreateNomTrack(PrevLeg);

			createProtectionArea(PrevLeg, direction);
			CreateKKLine(PrevLeg);

			////CreateNNSecondLine(PrevLeg);

			createAssesmentArea(PrevLeg, direction);
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

		public double KKLength
		{
			get;
			set;
		}

		public double Length
		{
			get { return ARANFunctions.ReturnDistanceInMeters(StartFIX.PrjPt, EndFIX.PrjPt); }
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

		public LineString NomTrack
		{
			get { return _NomTrack; }
			set { _NomTrack.Assign(value); }
		}

		public MultiLineString AssesmentAreaOutline
		{
			get { return _AssesmentAreaOutline; }
		}

		//public FlightPhase flightPhase
		//{
		//	get { return _FlightPhase; }
		//}

		private const double OverlapDist = 0.5;

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

		void JoinSegments(Ring LegRing, bool IsOuter, bool IsPrimary, TurnDirection TurnDir, CodeDirection segDir)
		{
			WayPoint start, end;
			//TurnDirection TurnDir;
			double EntryDir;
			double OutDir;

			if (segDir == CodeDirection.FORWARD)
			{
				start = _StartFIX;
				end = _EndFIX;

				EntryDir = _StartFIX.EntryDirection;
				OutDir = _StartFIX.OutDirection;
				//TurnDir = _StartFIX.EffectiveTurnDirection;
			}
			else
			{
				start = _EndFIX;
				end = _StartFIX;

				EntryDir = _EndFIX.OutDirection + Math.PI;
				OutDir = _EndFIX.EntryDirection + Math.PI;
				//TurnDir = (TurnDirection)(TurnDirection.NONE - _EndFIX.EffectiveTurnDirection);
			}

			double LegLenght = ARANMath.Hypot(end.PrjPt.X - start.PrjPt.X, end.PrjPt.Y - start.PrjPt.Y);

			double DivergenceAngle30, SplayAngle15,
				SpiralDivergenceAngle = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;

			//if (IsPrimary)
			//{
			//	SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
			//	DivergenceAngle30 = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;
			//}
			//else
			{
				SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
				DivergenceAngle30 = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;
			}

			//=================================================================================
			double fSide = IsOuter ? -((int)TurnDir) : ((int)TurnDir);
			//double fSide = -(int)TurnDir;

			bool expansAtTransition = false;
			bool normalWidth = true;			// ARANMath.RadToDeg(start.TurnAngle) <= 10;

			//WayPoint lEndFIX = new WayPoint(_aranEnv);
			//lEndFIX.Assign(end);

			double ASW_1C = IsPrimary ? 0.5 * end.SemiWidth : end.SemiWidth;
			double ASW_0C = IsPrimary ? 0.5 * start.SemiWidth : start.SemiWidth;
			double DistToEndFIX;

			end.CalcTurnRangePoints();
			//_UI.DrawPointWithText(end.PrjPt, -1, "End");
			//LegBase.ProcessMessages();


			//double fTmp = (int)lEndFIX.EffectiveTurnDirection * ARANMath.C_PI * 1.333333333333333333;
			//lEndFIX.OutDirection = 0.0;// lEndFIX.EntryDirection + fTmp;

			//DistToEndFIX = end.LPT - OverlapDist;
			DistToEndFIX = Math.Min(end.LPT, -end.ATT) - OverlapDist;

			//=================================================================================
			Point ptCurr = (Point)LegRing[LegRing.Count - 1].Clone();
			Point ptCenter;

			//if(expansAtTransition)
			//    ptCenter = lEndFIX.PrjPt;
			//else
			ptCenter = ARANFunctions.LocalToPrj(end.PrjPt, OutDir, -DistToEndFIX, 0);

			//_UI.DrawPointWithText(ptCurr, -1, "ptCurr");
			//_UI.DrawPointWithText(ptCenter, -1, "ptCenter");
			//LegBase.ProcessMessages();

			double DistToCenter, TransitionDist;

			ARANFunctions.PrjToLocal(ptCurr, OutDir, ptCenter, out DistToCenter, out TransitionDist);

			TransitionDist = 0.0;
			if (!expansAtTransition)
				TransitionDist = DistToCenter + DistToEndFIX;

			if (DistToCenter > ARANMath.EpsilonDistance)
			{
				Point ptBase0, ptBase1;
				double BaseDir0, BaseDir1;

				if (start.SensorType != eSensorType.GNSS)
				{
					double dPhi1 = Math.Atan2(ASW_0C - ASW_1C, LegLenght);
					//if (dPhi1 > DivergenceAngle30) dPhi1 = DivergenceAngle30;

					if (dPhi1 < 0.0)
					{
						if (dPhi1 < -SplayAngle15)
							dPhi1 = -SplayAngle15;
					}
					else if (dPhi1 > DivergenceAngle30)
						dPhi1 = DivergenceAngle30;

					BaseDir1 = BaseDir0 = OutDir - dPhi1 * fSide;

					//ptBase0 = ARANFunctions.LocalToPrj(end.PrjPt, OutDir + ARANMath.C_PI_2 * fSide, ASW_0C, 0);
					ptBase0 = ARANFunctions.LocalToPrj(start.PrjPt, OutDir + ARANMath.C_PI_2 * fSide, ASW_0C, 0);
					ptBase1 = ARANFunctions.LocalToPrj(end.PrjPt, OutDir + ARANMath.C_PI_2 * fSide, ASW_1C, 0);
				}
				else
				{
					BaseDir1 = BaseDir0 = OutDir;

					//ptBase0 = ARANFunctions.LocalToPrj(ptCenter, OutDir + ARANMath.C_PI_2 * fSide, ASW_0C, 0);
					ptBase0 = ARANFunctions.LocalToPrj(ptCenter, OutDir + ARANMath.C_PI_2 * fSide, ASW_1C, 0);
					ptBase1 = ARANFunctions.LocalToPrj(ptCenter, OutDir + ARANMath.C_PI_2 * fSide, ASW_1C, 0);
				}

				//_UI.DrawPointWithText(ptBase0, -1, "ptBase0");
				//_UI.DrawPointWithText(ptBase1, -1, "ptBase1");
				//LegBase.ProcessMessages();

				double ASW_0F = fSide * ARANFunctions.PointToLineDistance(ptCurr, start.PrjPt, BaseDir0); //Abs ????????
				double SightDir;	// = fSide * ARANFunctions.PointToLineDistance(ptCurr, ptBase1, OutDir);

				if ((IsOuter || normalWidth) && Math.Abs(ASW_0F - ASW_0C) > ARANMath.EpsilonDistance)
				{
					if (ASW_0F > ASW_0C)
						SightDir = OutDir - DivergenceAngle30 * fSide;
					else
						SightDir = OutDir + SplayAngle15 * fSide;


					Point ptInter;

					if (start.SensorType == eSensorType.GNSS)
					{

						Point ptTmp = ARANFunctions.LocalToPrj(ptCenter, OutDir + ARANMath.C_PI, TransitionDist, -fSide * ASW_0C);
						ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, SightDir, ptTmp, OutDir);
						//LineString ls = new LineString();
						//ls.Add(ptTmp);
						//ls.Add(ARANFunctions.LocalToPrj(ptTmp, OutDir, -20000, 0));
						//_UI.DrawLineString(ls, 0, 1);
						//_UI.DrawPointWithText(ptTmp, -1, "ptTmp");
						//LegBase.ProcessMessages();
					}
					else
						ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, SightDir, ptBase0, BaseDir0);

					//LineString ls = new LineString();
					//ls.Add(ptInter);
					//ls.Add(ARANFunctions.LocalToPrj(ptInter, OutDir, 20000, 0));
					//_UI.DrawLineString(ls, 0, 1);

					//_UI.DrawPointWithText(ptInter, -1, "ptInter-11");
					//LegBase.ProcessMessages();

					//LegBase.ProcessMessages(true);

					DistToCenter = ARANFunctions.PointToLineDistance(ptInter, ptCenter, OutDir + 0.5 * ARANMath.C_PI);
					//Point ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, SightDir, ptBase1, BaseDir1);
					//DistToCenter = ARANFunctions.PointToLineDistance(ptInter, lEndFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI);
					//ptTmp = ARANFunctions.PrjToLocal(lEndFIX.PrjPt, OutDir, ptInter);

					//_UI.DrawPointWithText(ptInter, -1, "ptInter-02");
					//_UI.DrawPointWithText(ptCenter, -1, "ptCenter");
					//_UI.DrawPointWithText(ptCurr, -1, "ptCurr");
					//LegBase.ProcessMessages();

					double Dist0 = ARANFunctions.PointToLineDistance(ptInter, ptCurr, OutDir - 0.5 * ARANMath.C_PI);

					//Ring rr = new Ring();
					//rr.Assign(LegRing);
					//_UI.DrawRing(rr, -1, eFillStyle.sfsHorizontal);
					//_UI.DrawPointWithText(ptCurr, -1, "ptCurr-22");
					//LegBase.ProcessMessages();

					//LineString ls = new LineString();
					//ls.Add(ptCenter);
					//ls.Add(ARANFunctions.LocalToPrj(ptCenter, OutDir, -10000, 0));
					//_UI.DrawLineString(ls, 255, 2);
					//LegBase.ProcessMessages();

					if (DistToCenter < 0.0) //|| Dist0 < -ARANMath.EpsilonDistance)// || ARANFunctions.PointToLineDistance(ptInter, lEndFIX.PrjPt, OutDir - 0.5 * ARANMath.C_PI) > 0.0
					{
						ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, SightDir, ptCenter, OutDir + 0.5 * ARANMath.C_PI);
						//_UI.DrawPointWithText(ptInter, -1, "ptInter-03");
						//LegBase.ProcessMessages(true);
						DistToCenter = ARANFunctions.PointToLineDistance(ptInter, ptCenter, OutDir + 0.5 * ARANMath.C_PI);
					}
					//else if (Dist0 < -ARANMath.EpsilonDistance)
					//{
					//}

					//_UI.DrawPointWithText(ptCurr, -1, "ptCurr-2");
					//_UI.DrawPointWithText(ptInter, -1, "ptInter");
					//_UI.DrawPointWithText(lEndFIX.PrjPt, -1, "EndF");
					//LegBase.ProcessMessages();

					//ptTmp = ARANFunctions.PrjToLocal(lEndFIX.PrjPt, OutDir, ptInter);
					//if (ptTmp.X <= 0.0)

					if (Dist0 > 0)
					{
						ptCurr = ptInter;
						LegRing.Add(ptCurr);
					}

					ASW_0F = fSide * ARANFunctions.PointToLineDistance(ptCurr, start.PrjPt, OutDir);


					//Ring rr = new Ring();
					//rr.Assign(LegRing);
					//_UI.DrawRing(rr, -1, eFillStyle.sfsHorizontal);
					//LegBase.ProcessMessages();
				}

				if (IsPrimary)
				{
					DivergenceAngle30 = Math.Atan(0.5 * Math.Tan(SpiralDivergenceAngle));
					double fTmp = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
					SplayAngle15 = Math.Atan(0.5 * Math.Tan(fTmp));
				}

				normalWidth = true;

				//LineString ls = new LineString();
				//ls.Add(ptBase1);
				//ls.Add(ARANFunctions.LocalToPrj(ptBase1, BaseDir1, -10000, 0));
				//_UI.DrawLineString(ls, 255, 2);
				//_UI.DrawPointWithText(ARANFunctions.LocalToPrj(ptBase1, BaseDir1, -10000, 0), -1, "BaseDir");
				//LegBase.ProcessMessages();

				//ls = new LineString();
				//ls.Add(ptBase0);
				//ls.Add(ARANFunctions.LocalToPrj(ptBase0, BaseDir1, 10000, 0));
				//_UI.DrawLineString(ls, 255, 2);
				//_UI.DrawPointWithText(ARANFunctions.LocalToPrj(ptBase0, BaseDir1, 10000, 0), -1, "BaseDir");
				//LegBase.ProcessMessages();

				//ls = new LineString();
				//ls.Add(ptCurr);
				//ls.Add(ARANFunctions.LocalToPrj(ptCurr, BaseDir1, 10000, 0));
				//_UI.DrawLineString(ls, 255, 2);
				//_UI.DrawPointWithText(ARANFunctions.LocalToPrj(ptCurr, BaseDir1, 10000, 0), 255, "BaseDir");
				//LegBase.ProcessMessages();

				//_UI.DrawPointWithText(ptBase1, 255, "ptBase1");
				//LegBase.ProcessMessages();


				//_UI.DrawPointWithText(ptCurr, -1, "ptCurr-2");
				//LegBase.ProcessMessages(true);

				double Delta = fSide * ARANFunctions.PointToLineDistance(ptCurr, ptBase1, BaseDir1);

				if (DistToCenter > ARANMath.EpsilonDistance && Math.Abs(Delta) > ARANMath.EpsilonDistance)
				{
					if (TransitionDist > 0 || expansAtTransition)
					{
						if (TransitionDist < DistToEndFIX) TransitionDist = DistToEndFIX;

						if (TransitionDist < DistToCenter) //&& TransitionDist > 0.0
						{
							if (start.SensorType == eSensorType.GNSS)
								ptCurr = ARANFunctions.LocalToPrj(end.PrjPt, OutDir - ARANMath.C_PI, TransitionDist, -fSide * ASW_0F);
							else
							{
								Point ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, BaseDir1, ptBase1, OutDir);
								double fDist = ARANFunctions.PointToLineDistance(ptInter, end.PrjPt, OutDir + ARANMath.C_PI_2);

								//_UI.DrawPointWithText(ptInter, -1, "ptInter-1");
								//LegBase.ProcessMessages();

								if (fDist < -ARANMath.EpsilonDistance)   //if (DistToCenter < 0 && Dist0 < 0)	//
									ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, BaseDir1, end.PrjPt, OutDir + ARANMath.C_PI_2);

								//_UI.DrawPointWithText(ptInter, -1, "ptInter-2");
								//LegBase.ProcessMessages();

								//_UI.DrawPointWithText(ptInter, -1, "ptCurr-3");
								//LegBase.ProcessMessages();

								DistToCenter = ARANFunctions.PointToLineDistance(ptInter, ptCenter, OutDir + ARANMath.C_PI_2);
								ptCurr = ptInter;
							}


							//_UI.DrawPointWithText(ptCurr, -1, "ptCurr-3");
							//LegBase.ProcessMessages();
							//LegRing.Remove(LegRing.Count - 1);
							LegRing.Add(ptCurr);
							DistToCenter = ARANFunctions.PointToLineDistance(ptCurr, ptCenter, OutDir + ARANMath.C_PI_2);
						}
					}

					if (DistToCenter > ARANMath.EpsilonDistance)
					{
						//fSide = -fSide;
						if (Delta > ARANMath.EpsilonDistance)
							SightDir = OutDir - DivergenceAngle30 * fSide;
						else
							SightDir = OutDir + SplayAngle15 * fSide;

						Point ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, SightDir, ptBase1, BaseDir1);
						DistToCenter = ARANFunctions.PointToLineDistance(ptInter, ptCenter, OutDir + ARANMath.C_PI_2);
						double Dist0 = ARANFunctions.PointToLineDistance(ptInter, ptCurr, OutDir - ARANMath.C_PI_2);

						if (DistToCenter < -ARANMath.EpsilonDistance)   //if (DistToCenter < 0 && Dist0 < 0)	//
						{
							ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, SightDir, ptCenter, OutDir + ARANMath.C_PI_2);
							DistToCenter = ARANFunctions.PointToLineDistance(ptInter, ptCenter, OutDir + ARANMath.C_PI_2);
						}

						//_UI.DrawPointWithText(ptCurr, -1, "ptCurr-3");
						//_UI.DrawPointWithText(ptInter, -1, "ptInter-3");
						//LegBase.ProcessMessages();
						if (Dist0 > ARANMath.EpsilonDistance)
						{
							ptCurr = ptInter;
							LegRing.Add(ptCurr);
						}
					}
				}

				if (start.SensorType != eSensorType.GNSS)
				{
					double distToBase1, fTmpY;

					//ptCurr = LegRing[LegRing.Count - 1];

					ARANFunctions.PrjToLocal(ptCurr, OutDir, ptBase1, out distToBase1, out fTmpY);
					//ARANFunctions.PrjToLocal(ptCurr, OutDir, ptCenter, out DistToCenter, out fTmpY);
					if (distToBase1 > 0 && distToBase1 < DistToCenter)
					{
						ptCurr = ptBase1;
						LegRing.Add(ptCurr);

						//_UI.DrawPointWithText(ptCurr, -1, "ptCurr-5");
						//_UI.DrawPointWithText(ptBase1, -1, "ptBase1");
						//LegBase.ProcessMessages(true);

						DistToCenter -= distToBase1;
					}
				}

				//DistToCenter += 0.1;
				if (DistToCenter + 0.1 > ARANMath.EpsilonDistance)
				{
					ptCurr = ARANFunctions.LocalToPrj(ptCenter, OutDir, 0, ASW_1C * fSide);
					//_UI.DrawPointWithText(ptCurr, 255, "ptCurr-5");
					//LegBase.ProcessMessages();
					LegRing.Add(ptCurr);
				}
			}

			//Ring rr = new Ring();
			//rr.Assign(LegRing);
			//_UI.DrawRing(rr, -1, eFillStyle.sfsHorizontal);
			//LegBase.ProcessMessages();
		}

#if UseSmallAngle
		Ring CreateOuterTurnAreaLT(EnRouteLeg PrevLeg, bool IsPrimary, CodeDirection direction)
		{
			WayPoint start, end, measureFIX;
			TurnDirection TurnDir;
			double EntryDir;
			double OutDir;

			if (direction == CodeDirection.FORWARD)
			{
				start = _StartFIX;
				end = _EndFIX;

				if (PrevLeg != null)
					measureFIX = (WayPoint)PrevLeg.EndFIX.Clone();
				else
					measureFIX = (WayPoint)start.Clone();

				EntryDir = _StartFIX.EntryDirection;
				OutDir = _StartFIX.OutDirection;
				TurnDir = _StartFIX.EffectiveTurnDirection;
			}
			else
			{
				start = _EndFIX;
				end = _StartFIX;

				if (PrevLeg != null)
					measureFIX = (WayPoint)PrevLeg.StartFIX.Clone();
				else
					measureFIX = (WayPoint)start.Clone();

				EntryDir = _EndFIX.OutDirection + Math.PI;
				OutDir = _EndFIX.EntryDirection + Math.PI;
				TurnDir = (TurnDirection)(TurnDirection.NONE - _EndFIX.EffectiveTurnDirection);

				measureFIX.TurnDirection = TurnDir;
				measureFIX.EntryDirection = EntryDir;
				measureFIX.OutDirection = OutDir;
			}

			double fSide = (int)TurnDir;
			double TurnAng = ARANMath.Modulus((OutDir - EntryDir) * fSide, ARANMath.C_2xPI);

			if (TurnAng < ARANMath.EpsilonRadian || Math.Abs(TurnAng - ARANMath.C_2xPI) < ARANMath.EpsilonRadian)
				TurnAng = 0.0;

			double SpiralDivergenceAngle = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;
			double SplayAngle15, DivergenceAngle30;

			if (IsPrimary)
			{
				DivergenceAngle30 = SpiralDivergenceAngle;
				SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;

				//				DivergenceAngle30 = Math.Atan(0.5 * Math.Tan(SpiralDivergenceAngle));
				//				fTmp = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
				//				SplayAngle15 = Math.Atan(0.5 * Math.Tan(fTmp));
			}
			else
			{
				DivergenceAngle30 = SpiralDivergenceAngle;
				//_constants.Constant[arSecAreaCutAngl].Value;
				SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
			}

			Point ptTmp;
			//double LPT;
			//ptTmp = ARANFunctions.LocalToPrj(start.PrjPt, EntryDir, -start.EPT + 1.0, 0);

			//if (start.DFTarget)
			//{
			//    if (TurnDir == TurnDirection.CW)
			//        LPT = start.LPT_L;
			//    else
			//        LPT = start.LPT_R;
			//}
			//else
			//    LPT = start.LPT;

			//ptTmp = ARANFunctions.LocalToPrj(start.PrjPt, EntryDir, LPT, 0);

			//ptTmp = start.PrjPt;	// ARANFunctions.LocalToPrj(StartFIX.PrjPt, EntryDir - ARANMath.C_PI, -0.5, 0);

			double EPT = measureFIX.EPT, LPT = -measureFIX.LPT;

			//ptTmp = ARANFunctions.LocalToPrj(start.PrjPt, OutDir, -0.5, 0);
			ptTmp = start.PrjPt;

			//_UI.DrawRing(PrevLeg.PrimaryArea[0].ExteriorRing, 255, eFillStyle.sfsBackwardDiagonal);
			//_UI.DrawPoint(ptTmp, 255);
			//LegBase.ProcessMessages();

			Point ptFrom = null;
			double ASW_OUT0F, ASW_OUT1, ASW_OUT0C;
			double fTmp;

			if (TurnDir == TurnDirection.CCW)
			{
				if (IsPrimary)
				{
					ASW_OUT1 = 0.5 * measureFIX.SemiWidth;  //start.SemiWidth;
					ASW_OUT0C = ASW_OUT0F = measureFIX.ASW_2_R;

					if (PrevLeg != null && PrevLeg.PrimaryArea.Count > 0)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDir - 0.5 * ARANMath.C_PI, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, start.PrjPt, OutDir - 0.5 * ARANMath.C_PI, OutDir, out fTmp);

						if (ptFrom == null)
						{
							ptTmp = ARANFunctions.LocalToPrj(start.PrjPt, EntryDir, -start.EPT + 1.0, 0);
							ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDir - 0.5 * ARANMath.C_PI, out fTmp);
						}

						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}
				}
				else
				{
					ASW_OUT1 = measureFIX.SemiWidth;    //start.SemiWidth;
					ASW_OUT0C = ASW_OUT0F = measureFIX.ASW_R;

					if (PrevLeg != null && PrevLeg.FullArea.Count > 0)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir - 0.5 * ARANMath.C_PI, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, start.PrjPt, OutDir - 0.5 * ARANMath.C_PI, OutDir, out fTmp);

						//_UI.DrawRing(PrevLeg.FullArea[0].ExteriorRing, 255, eFillStyle.sfsCross);
						//_UI.DrawMultiPolygon(PrevLeg.FullArea, 255, eFillStyle.sfsCross);
						//_UI.DrawPointWithText(StartFIX.PrjPt, 0, "StartFIX");
						//LegBase.ProcessMessages(true);

						if (ptFrom == null)
						{
							ptTmp = ARANFunctions.LocalToPrj(start.PrjPt, EntryDir, -start.EPT + 1.0, 0);
							ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir - 0.5 * ARANMath.C_PI, out fTmp);
						}

						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}
				}
			}
			else
			{
				if (IsPrimary)
				{
					ASW_OUT1 = 0.5 * measureFIX.SemiWidth;  //start.SemiWidth;
					ASW_OUT0C = ASW_OUT0F = measureFIX.ASW_2_L;

					if (PrevLeg != null && PrevLeg.PrimaryArea.Count > 0)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDir + 0.5 * ARANMath.C_PI, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, start.PrjPt, OutDir + 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						if (ptFrom == null)
						{
							ptTmp = ARANFunctions.LocalToPrj(start.PrjPt, EntryDir, -start.EPT + 1.0, 0);
							ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDir + 0.5 * ARANMath.C_PI, out fTmp);
						}

						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}
				}
				else
				{
					ASW_OUT1 = measureFIX.SemiWidth;    // start.SemiWidth;
					ASW_OUT0C = ASW_OUT0F = measureFIX.ASW_L;

					if (PrevLeg != null && PrevLeg.FullArea.Count > 0)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir + 0.5 * ARANMath.C_PI, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, start.PrjPt, OutDir + 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						if (ptFrom == null)
						{
							ptTmp = ARANFunctions.LocalToPrj(start.PrjPt, EntryDir, -start.EPT + 1.0, 0);
							ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir + 0.5 * ARANMath.C_PI, out fTmp);
						}

						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}
				}
			}

			Ring result = new Ring();

			//_UI.DrawPointWithText(ptFrom, -1, "ptFrom");
			//LegBase.ProcessMessages();

			if (start.SensorType == eSensorType.GNSS)
			{
				if (-LPT < EPT || ptFrom == null)
				//if (-LPT < EPT && ptFrom == null)
				{
					ptTmp = ARANFunctions.LocalToPrj(start.PrjPt, EntryDir, -EPT - 0.5, -ASW_OUT0C * fSide);
					//_UI.DrawPointWithText(ptTmp, -1, "ptTmp-EPT");
					//LegBase.ProcessMessages();

					result.Add(ptTmp);
				}
				else
				{
					ptTmp = ARANFunctions.LocalToPrj(start.PrjPt, EntryDir, LPT, -ASW_OUT0C * fSide);
					//_UI.DrawPointWithText(ptTmp, -1, "ptTmp-LPT");
					//LegBase.ProcessMessages();
					result.Add(ptTmp);
				}
			}
			//================================================================+++++++++++++++++++++++++++++++++++++++++++++++++++++++++

			if (ptFrom == null)
			{
				ptTmp = ARANFunctions.LocalToPrj(start.PrjPt, EntryDir, -EPT, -ASW_OUT0F * fSide);
				//ptTmp = ARANFunctions.LocalToPrj(start.PrjPt, EntryDir - ARANMath.C_PI, start.EPT + 10.0, ASW_OUT0F * fSide);

				//_UI.DrawPointWithText(ptTmp, -1, "ptFrom_01");
				//LegBase.ProcessMessages();

				result.Add(ptTmp);

				ptFrom = ARANFunctions.LocalToPrj(start.PrjPt, EntryDir - 0.5 * ARANMath.C_PI * fSide, ASW_OUT0F);
				//ptFrom = ARANFunctions.LocalToPrj(start.PrjPt, OutDir + 0.5 * ARANMath.C_PI * fSide, ASW_OUT0F);
				//ptFrom = (Point)ARANFunctions.LineLineIntersect(ptTmp, EntryDir, StartFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI);
			}

			//_UI.DrawPointWithText(ptFrom, -1, "ptFrom_01");
			//LegBase.ProcessMessages();

			double toDir = OutDir;
			if (measureFIX.SemiWidth > start.SemiWidth)
				toDir = OutDir + DivergenceAngle30 * fSide;
			else if (measureFIX.SemiWidth < start.SemiWidth)
				toDir = OutDir - SplayAngle15 * fSide;

			//double TurnAng1 = ARANMath.Modulus((toDir - EntryDir) * fSide, ARANMath.C_2xPI);
			//if (TurnAng1 > ARANMath.C_PI)
			//	toDir = EntryDir;
			double ptDir, ptDist, x, y;
			Point ptTo = ARANFunctions.LocalToPrj(start.PrjPt, toDir - 0.5 * ARANMath.C_PI * fSide, ASW_OUT1, 0);
			ARANFunctions.PrjToLocal(ptFrom, EntryDir, ptTo, out x, out y);

			if (x <= 0.0)       //	if (false)
				fTmp = ptDir = ptDist = 0.0;
			else
			{
				ptDir = Math.Atan2(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);
				ptDist = ARANMath.Hypot(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);

				if (measureFIX.SemiWidth > start.SemiWidth)
					fTmp = ARANMath.Modulus((ptDir - OutDir) * fSide, ARANMath.C_2xPI);     /////////////////////////?????????????????
				else //if (measureFIX.SemiWidth < start.SemiWidth)
					fTmp = ARANMath.Modulus((ptDir - toDir) * fSide, ARANMath.C_2xPI);      /////////////////////////?????????????????

				if (fTmp > ARANMath.C_PI)
					fTmp = fTmp - ARANMath.C_2xPI;
			}

			if (TurnAng < ARANMath.DegToRadValue || ptDist < 1.0)   //0.017453292519943295
				result.Add(ptFrom);
			else if (ptDist > 1.0 && fTmp >= -SplayAngle15 && fTmp <= DivergenceAngle30)
			{
				//toDir = OutDir + DivergenceAngle30 * fSide;
				//ptTo = ARANFunctions.LocalToPrj(start.PrjPt, toDir - 0.5 * ARANMath.C_PI * fSide, ASW_OUT1, 0);
				//ptDir = Math.Atan2(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);
				//ptDist = ARANMath.Hypot(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);

				ptTmp = ARANFunctions.LocalToPrj(ptFrom, ptDir, 0.5 * ptDist, 0);

				//	_UI.DrawPointWithText(ptTmp, 128+(128<<8), "ptTmp");
				//_UI.DrawPointWithText(ptTo, -1, "ptTo");
				//_UI.DrawPointWithText(ptFrom, -1, "ptFrom");
				//LegBase.ProcessMessages();

				Geometry Geom0 = ARANFunctions.LineLineIntersect(ptTmp, ptDir + 0.5 * ARANMath.C_PI, ptFrom, EntryDir + 0.5 * ARANMath.C_PI);
				Geometry Geom1 = ARANFunctions.LineLineIntersect(ptTmp, ptDir + 0.5 * ARANMath.C_PI, ptTo, toDir + 0.5 * ARANMath.C_PI);
				Point ptCnt = null;

				if (Geom0.Type == GeometryType.Point)
				{
					ptCnt = (Point)Geom0;
					//	_UI.DrawPointWithText((Point)Geom1, 128 + (255 << 8), "Geom1");
					//	LegBase.ProcessMessages();

					if (Geom1 != null && Geom1.Type == GeometryType.Point)
					{
						double Dist0 = ARANMath.Hypot(ptFrom.Y - ptCnt.Y, ptFrom.X - ptCnt.X);
						double Dist1 = ARANMath.Hypot(ptFrom.Y - ((Point)Geom1).Y, ptFrom.X - ((Point)Geom1).X);
						if (ASW_OUT0F > ASW_OUT1 && Dist1 < Dist0)
							ptCnt.Assign(Geom1);
					}
				}
				else if (Geom1.Type == GeometryType.Point)
					ptCnt = (Point)Geom1;

				if (ptCnt != null)
				{
					Ring tmpRing = ARANFunctions.CreateArcPrj(ptCnt, ptFrom, ptTo, TurnDir);
					//_UI.DrawRing(tmpRing, 0, eFillStyle.sfsSolid);
					//LegBase.ProcessMessages();

					//	for (int ffg = 0; ffg < tmpRing.Count; ffg++)
					//		_UI.DrawPointWithText(tmpRing[ffg], 128 << 8, "pt" + ffg.ToString());
					//					LegBase.ProcessMessages(true);

					result.AddMultiPoint(tmpRing);
				}
				else
					result.Add(ptFrom);
			}
			//else if (fTmp < -SplayAngle15)	//?????????????????????????????????????????
			//{
			//	//toDir = OutDir - SplayAngle15 * fSide;
			//	//ptTo = ARANFunctions.LocalToPrj(start.PrjPt, toDir - 0.5 * ARANMath.C_PI * fSide, ASW_OUT1, 0);
			//	//ptDir = Math.Atan2(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);
			//	//ptDist = ARANMath.Hypot(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);

			//	Geometry Geom0 = ARANFunctions.LineLineIntersect(ptFrom, toDir, start.PrjPt, OutDir + 0.5 * ARANMath.C_PI);
			//	if (Geom0.Type == GeometryType.Point)
			//	{
			//		//ptFrom = ptTo;
			//		ptTo = (Point)Geom0;

			//		//_UI.DrawPointWithText(ptTo, -1, "ptTo-1");
			//		//LegBase.ProcessMessages();

			//		ptDir = Math.Atan2(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);
			//		ptDist = ARANMath.Hypot(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);

			//		ptTmp = ARANFunctions.LocalToPrj(ptFrom, ptDir, 0.5 * ptDist, 0);
			//		Geom0 = ARANFunctions.LineLineIntersect(ptTmp, ptDir + 0.5 * ARANMath.C_PI, ptFrom, EntryDir + 0.5 * ARANMath.C_PI - SplayAngle15 * fSide);
			//		Geometry Geom1 = ARANFunctions.LineLineIntersect(ptTmp, ptDir + 0.5 * ARANMath.C_PI, ptTo, OutDir + 0.5 * ARANMath.C_PI - SplayAngle15 * fSide);
			//		Point ptCnt = null;

			//		if (Geom0.Type == GeometryType.Point)
			//		{
			//			ptCnt = (Point)Geom0;
			//			//_UI.DrawPointWithText(ptCnt, -1, "ptCnt");
			//			//LegBase.ProcessMessages();

			//			if (Geom1 != null && Geom1.Type == GeometryType.Point)
			//			{
			//				double Dist0 = ARANMath.Hypot(ptFrom.Y - ptCnt.Y, ptFrom.X - ptCnt.X);
			//				double Dist1 = ARANMath.Hypot(ptFrom.Y - ((Point)Geom1).Y, ptFrom.X - ((Point)Geom1).X);
			//				if (ASW_OUT0F > ASW_OUT1 && Dist1 < Dist0)
			//					ptCnt.Assign(Geom1);
			//			}
			//		}
			//		else if (Geom1.Type == GeometryType.Point)
			//			ptCnt = (Point)Geom1;

			//		if (ptCnt != null)
			//		{
			//			Ring tmpRing = ARANFunctions.CreateArcPrj(ptCnt, ptFrom, ptTo, TurnDir);
			//			result.AddMultiPoint(tmpRing);
			//		}
			//		else
			//			result.Add(ptFrom);
			//	}
			//	else
			//		result.Add(ptFrom);
			//}
			else
				result.Add(ptFrom);

			//=============================================================================
			//_UI.DrawRing(result, 128 << 8, eFillStyle.sfsCross );
			//LegBase.ProcessMessages();

			JoinSegments(result, true, IsPrimary, TurnDir, direction);

			//_UI.DrawRing(result, 128 << 8, eFillStyle.sfsCross );
			//LegBase.ProcessMessages();

			//for (int ffg = 0; ffg < result.Count; ffg++)
			//	_UI.DrawPointWithText(result[ffg],128<<8, "pt" +ffg.ToString());
			//LegBase.ProcessMessages(true);

			return result;
		}

		Ring CreateInnerTurnAreaLT(EnRouteLeg PrevLeg, bool IsPrimary, CodeDirection direction)
		{
			WayPoint start, end, measureFIX;
			TurnDirection TurnDir;
			double EntryDir;
			double OutDir;

			if (direction == CodeDirection.FORWARD)
			{
				start = _StartFIX;
				end = _EndFIX;

				if (PrevLeg != null)
					measureFIX = (WayPoint)PrevLeg.EndFIX.Clone();
				else
					measureFIX = (WayPoint)start.Clone();

				EntryDir = _StartFIX.EntryDirection;
				OutDir = _StartFIX.OutDirection;
				TurnDir = _StartFIX.EffectiveTurnDirection;
			}
			else
			{
				start = _EndFIX;
				end = _StartFIX;

				if (PrevLeg != null)
					measureFIX = (WayPoint)PrevLeg.StartFIX.Clone();
				else
					measureFIX = (WayPoint)start.Clone();

				EntryDir = _EndFIX.OutDirection + Math.PI;
				OutDir = _EndFIX.EntryDirection + Math.PI;
				TurnDir = (TurnDirection)(TurnDirection.NONE - _EndFIX.EffectiveTurnDirection);

				measureFIX.TurnDirection = TurnDir;
				measureFIX.EntryDirection = EntryDir;
				measureFIX.OutDirection = OutDir;
			}

			double fSide = (int)TurnDir;
			double TurnAng = ARANMath.Modulus((OutDir - EntryDir) * fSide, ARANMath.C_2xPI);

			if (TurnAng < ARANMath.EpsilonRadian || Math.Abs(TurnAng - ARANMath.C_2xPI) < ARANMath.EpsilonRadian)
				TurnAng = 0.0;

			double SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value,
				DivergenceAngle30 = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;

			//if (IsPrimary)
			//{

			//SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
			//DivergenceAngle30 = SpiralDivergenceAngle;

			////	fTmp = GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
			//DivergenceAngle30 = Math.Atan(0.5 * Math.Tan(SpiralDivergenceAngle));
			//fTmp = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
			//SplayAngle15 = Math.Atan(0.5 * Math.Tan(fTmp));
			//}
			//else
			//{
			//	SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
			//	DivergenceAngle30 = SpiralDivergenceAngle;
			//	//_constants.Constant[arSecAreaCutAngl].Value;
			//}

			double EPT = measureFIX.EPT, LPT = -measureFIX.LPT;
			//ptTmp = ARANFunctions.LocalToPrj(start.PrjPt, EntryDir, measureFIX.LPT, 0);

			Point ptTmp;
			ptTmp = start.PrjPt;

			//if (true)
			//{
			//	double LPT = measureFIX.LPT;
			//	ptTmp = ARANFunctions.LocalToPrj(start.PrjPt, EntryDir, LPT, 0);
			//}
			//else
			//	ptTmp = start.PrjPt;

			//_UI.DrawPointWithText(ptTmp, 0, "ptTmp");
			//LegBase.ProcessMessages(true);

			double fTmp, ASW_IN0F, ASW_IN1, ASW_IN0C, ASW_1C;
			Point ptFrom = null;

			if (TurnDir == TurnDirection.CCW)
			{
				if (IsPrimary)
				{
					ASW_1C = 0.5 * end.SemiWidth;
					ASW_IN1 = 0.5 * start.SemiWidth;
					//ASW_IN0C = 0.5 * measureFIX.SemiWidth;
					ASW_IN0C = ASW_IN0F = measureFIX.ASW_2_L;

					if (PrevLeg != null && PrevLeg.PrimaryArea.Count > 0)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, OutDir + 0.5 * ARANMath.C_PI, out fTmp);
						//_UI.DrawPointWithText(ptFrom, -1, "ptFrom");
						//LegBase.ProcessMessages();

						if (ptFrom != null)
							ASW_IN0F = fTmp;
					}
				}
				else
				{
					ASW_1C = end.SemiWidth;
					ASW_IN1 = start.SemiWidth;
					//ASW_IN0C = measureFIX.SemiWidth;
					ASW_IN0C = ASW_IN0F = measureFIX.ASW_L;

					if (PrevLeg != null && PrevLeg.FullArea.Count > 0)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, OutDir + 0.5 * ARANMath.C_PI, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, ptTmp, OutDir + 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_IN0F = fTmp;
					}
				}
			}
			else
			{
				if (IsPrimary)
				{
					ASW_1C = 0.5 * end.SemiWidth;
					ASW_IN1 = 0.5 * start.SemiWidth;
					//ASW_IN0C = 0.5 * measureFIX.SemiWidth;
					ASW_IN0C = ASW_IN0F = measureFIX.ASW_2_R;
					if (PrevLeg != null && PrevLeg.PrimaryArea.Count > 0)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, OutDir - 0.5 * ARANMath.C_PI, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, OutDir - 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_IN0F = fTmp;
					}
				}
				else
				{
					ASW_1C = end.SemiWidth;
					ASW_IN1 = start.SemiWidth;
					//ASW_IN0C = measureFIX.SemiWidth;
					ASW_IN0C = ASW_IN0F = measureFIX.ASW_R;
					if (PrevLeg != null && PrevLeg.FullArea.Count > 0)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, OutDir - 0.5 * ARANMath.C_PI, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, ptTmp, OutDir - 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_IN0F = fTmp;
					}
				}
			}

			//	_UI.DrawPointWithText(ptFrom, 0, "ptFrom-I0");
			//	LegBase.ProcessMessages();

			//_UI.DrawMultiPolygon(PrevLeg.PrimaryArea, -1, eFillStyle.sfsHorizontal);
			//LegBase.ProcessMessages();

			Ring result = new Ring();

			if (start.SensorType == eSensorType.GNSS)
			{
				if (-LPT < EPT || ptFrom == null)
				{
					ptTmp = ARANFunctions.LocalToPrj(start.PrjPt, EntryDir, -EPT - 0.5, ASW_IN0C * fSide);
					//_UI.DrawPointWithText(ptTmp, -1, "ptTmp-2");
					//_UI.DrawPointWithText(ptFrom, -1, "ptFrom-2");
					//LegBase.ProcessMessages();
					//fTmp = ARANFunctions.ReturnDistanceInMeters(ptTmp, ptFrom);

					result.Add(ptTmp);
				}
				else
				{
					ptTmp = ARANFunctions.LocalToPrj(start.PrjPt, EntryDir, LPT, ASW_IN0C * fSide);
					//_UI.DrawPointWithText(ptFrom, -1, "ptFrom-2");
					//LegBase.ProcessMessages();
					result.Add(ptTmp);
				}
			}

			if (ptFrom == null)
			{
				if (PrevLeg == null)
				{
					ptTmp = ARANFunctions.LocalToPrj(start.PrjPt, EntryDir, -EPT, ASW_IN0F * fSide);

					//_UI.DrawPointWithText(ptTmp, -1, "ptTmp_02");
					//LegBase.ProcessMessages();

					result.Add(ptTmp);
				}

				//ptFrom = ARANFunctions.LocalToPrj(start.PrjPt, OutDir + 0.5 * ARANMath.C_PI * fSide, ASW_IN0F);
				//ptFrom = (Point)ARANFunctions.LineLineIntersect(ptTmp, EntryDir, StartFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI);

				fTmp = ASW_IN0F / Math.Cos(TurnAng);
				ptFrom = ARANFunctions.LocalToPrj(start.PrjPt, OutDir + 0.5 * ARANMath.C_PI * fSide, fTmp);
			}

			//_UI.DrawPointWithText(ptFrom, -1, "ptFrom_02");
			//LegBase.ProcessMessages();

			//fTmp = ARANFunctions.ReturnDistanceInMeters(ptTmp, ptFrom);
			result.Add(ptFrom);


			//fTmp = ASW_IN0F / Math.Cos(TurnAng);
			//ptFrom = ARANFunctions.LocalToPrj(start.PrjPt, OutDir + 0.5 * ARANMath.C_PI * fSide, fTmp);
			//result.Add(ptFrom);

			//_UI.DrawPointWithText(ptFrom, 0, "ptFrom-2");
			//LegBase.ProcessMessages();


			//fTmp = ASW_IN1 / Math.Cos(TurnAng);

			//double LegLenght = ARANFunctions.ReturnDistanceInMeters(start.PrjPt, end.PrjPt);
			double LegLenght = ARANMath.Hypot(end.PrjPt.X - start.PrjPt.X, end.PrjPt.Y - start.PrjPt.Y);

			double dPhi1 = Math.Atan2(ASW_IN0C - ASW_1C, LegLenght);
			if (dPhi1 < 0.0)
			{
				if (dPhi1 < -SplayAngle15) dPhi1 = -SplayAngle15;
			}
			else if (dPhi1 > DivergenceAngle30)
				dPhi1 = DivergenceAngle30;

			double BaseDir0 = OutDir - dPhi1 * fSide;

			Point ptTo;

			fTmp = ASW_IN0C * Math.Sin(ARANMath.C_PI_2 - dPhi1) / Math.Cos(TurnAng - dPhi1);

			//fTmp = ASW_IN0C / Math.Cos(TurnAng);		// for test

			ptTo = ARANFunctions.LocalToPrj(start.PrjPt, EntryDir + 0.5 * ARANMath.C_PI * fSide, fTmp);

			//_UI.DrawPointWithText(ptTo, 0, "ptTo-3");
			//LegBase.ProcessMessages();

			//LineString ls = new LineString();
			//ptTmp = ARANFunctions.LocalToPrj(start.PrjPt, OutDir + 0.5 * ARANMath.C_PI* fSide, ASW_IN0C);
			//ls.Add(ptTmp);
			//ptTmp = ARANFunctions.LocalToPrj(end.PrjPt, OutDir + 0.5 * ARANMath.C_PI * fSide, end.SemiWidth);
			//ls.Add(ptTmp);
			//_UI.DrawLineString(ls, -1, 1);
			//_UI.DrawPointWithText(ptTmp, -1, "Base-1");
			//LegBase.ProcessMessages();

			//LegBase.ProcessMessages(true);

			double ptDist = ARANMath.Hypot(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);
			double ptDir = Math.Atan2(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);

			fTmp = ARANMath.Modulus((OutDir - ptDir) * fSide, ARANMath.C_2xPI);

			if (fTmp > ARANMath.C_PI)
				fTmp = fTmp - ARANMath.C_2xPI;

			//if (fTmp > ARANMath.C_2xPI)
			//	fTmp = fTmp - ARANMath.C_2xPI;
			if (ptDist > 1.0)
			{
				if (fTmp > -SplayAngle15 && fTmp < DivergenceAngle30)
					result.Add(ptTo);
				else
				{
					if (fTmp < -SplayAngle15)
						fTmp = -SplayAngle15;
					else //if (fTmp > DivergenceAngle30)
						fTmp = DivergenceAngle30;

					double SplayAngle = OutDir - fTmp * fSide;

					Point ptBase = ARANFunctions.LocalToPrj(start.PrjPt, OutDir, 0.0, ASW_IN1 * fSide);
					Point ptInter = ARANFunctions.LineLineIntersect(ptFrom, SplayAngle, ptBase, OutDir) as Point;

					//_UI.DrawPointWithText(ptBase, -1, "ptBase");
					//_UI.DrawPointWithText(ptInter, 0, "ptInter");
					//LegBase.ProcessMessages();

					double CurDist = ARANFunctions.PointToLineDistance(ptFrom, end.PrjPt, OutDir + 0.5 * ARANMath.C_PI);
					double ptInterDist = ARANFunctions.PointToLineDistance(ptInter, end.PrjPt, OutDir + 0.5 * ARANMath.C_PI);

					if (ptInterDist > CurDist)
					{
						ptInter = ARANFunctions.LineLineIntersect(ptFrom, SplayAngle, end.PrjPt, OutDir + 0.5 * ARANMath.C_PI) as Point;

						//_UI.DrawPointWithText(ptInter, -1, "ptInter - 3");
						//_UI.DrawPointWithText(ptCut, -1, "ptCut");
						//LegBase.ProcessMessages();
					}

					//_UI.DrawPointWithText(ptInter, 0, "ptInter - 2");
					//LegBase.ProcessMessages(true);

					result.Add(ptInter);
				}
			}
			//fTmp = ASW_IN0F / Math.Cos(TurnAng);
			//ptFrom = ARANFunctions.LocalToPrj(start.PrjPt, OutDir + 0.5 * ARANMath.C_PI * fSide, fTmp, 0);
			//_UI.DrawPointWithText(ptFrom, 0, "ptFrom-1");
			//LegBase.ProcessMessages();

			//_UI.DrawPointWithText(ptFrom, 255, "ptFrom");
			//_UI.DrawPointWithText(ptTo, 255, "ptTo");
			//_UI.DrawPointWithText(ptTmp, 0, "ptTmp");
			///LegBase.ProcessMessages(true);

			JoinSegments(result, false, IsPrimary, TurnDir, direction);

			//_UI.DrawPointWithText(result[2], 0, "result[2]");
			//_UI.DrawPointWithText(result[3], 0, "result[3]");
			//LegBase.ProcessMessages(true);

			return result;
		}
#endif

		Ring CreateOuterTurnArea(EnRouteLeg PrevLeg, bool IsPrimary, CodeDirection direction)
		{
			int i, n;

			double R, K, dAlpha, AztEnd1, AztEnd2, fTmp, SpStartDir, SpStartRad,
				SpTurnAng, SpToAngle, SplayAngle, CurWidth, CurDist,    //SpFromAngle, 
				Dist0, MaxDist, LPTYDist, fDistTreshold,
				ptInterDist, dRad, SpAbeamDist, SplayAngle15, PrevX, PrevY, BulgeAngle,
				BaseDir, Delta, DivergenceAngle30, ASW_OUT0C, ASW_OUT0F, ASW_OUTMax, ASW_OUT1;

			Point OuterBasePoint, InnerBasePoint, ptTmp, ptInter, ptBase, ptFrom,
				ptCnt, ptCurr, ptCurr1;

			bool bFlag, HaveSecondSP;

			WayPoint start, end, measureFIX;
			TurnDirection TurnDir;
			double EntryDir, OutDir;

			if (direction == CodeDirection.FORWARD)
			{
				start = _StartFIX;
				end = _EndFIX;

				if (PrevLeg != null)
					measureFIX = (WayPoint)PrevLeg.EndFIX.Clone();
				else
					measureFIX = (WayPoint)start;

				EntryDir = _StartFIX.EntryDirection;
				OutDir = _StartFIX.OutDirection;
				TurnDir = _StartFIX.EffectiveTurnDirection;
			}
			else
			{
				start = _EndFIX;
				end = _StartFIX;

				if (PrevLeg != null)
					measureFIX = (WayPoint)PrevLeg.StartFIX.Clone();
				else
					measureFIX = (WayPoint)start.Clone();

				EntryDir = _EndFIX.OutDirection + Math.PI;
				OutDir = _EndFIX.EntryDirection + Math.PI;
				TurnDir = (TurnDirection)(TurnDirection.NONE - _EndFIX.EffectiveTurnDirection);

				measureFIX.TurnDirection = TurnDir;
				measureFIX.EntryDirection = EntryDir;
				measureFIX.OutDirection = OutDir;

				//measureFIX.CalcTurnRangePoints();
				//_StartFIX.CalcTurnRangePoints();
				//_EndFIX.CalcTurnRangePoints();
				//PrevLeg.StartFIX.RefreshGraphics();

				//_UI.DrawPointWithText(measureFIX.PrjPt, -1, "measureFIX");
				//LegBase.ProcessMessages();
			}

			//TurnDirection TurnDir = ARANMath.SideToTurn(ARANMath.SideDef(start.PrjPt, EntryDir, _EndFIX.PrjPt));
			double fSide = (int)TurnDir;
			double TurnAng = ARANMath.Modulus((OutDir - EntryDir) * fSide, ARANMath.C_2xPI);

			if (TurnAng < ARANMath.EpsilonRadian || Math.Abs(TurnAng - ARANMath.C_2xPI) < ARANMath.EpsilonRadian)
				TurnAng = 0.0;

			if (TurnAng <= ARANMath.DegToRad(minTurnAngle))
				return CreateOuterTurnAreaLT(PrevLeg, IsPrimary, direction);

			DivergenceAngle30 = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;
			SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;

			//double SpiralDivergenceAngle = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;

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

			double TurnR = ARANMath.BankToRadius(start.BankAngle, start.ConstructTAS);

			double Rv = 1765.27777777777777777 * Math.Tan(start.BankAngle) / (ARANMath.C_PI * start.ConstructTAS);
			if (Rv > 3.0)
				Rv = 3.0;

			double WSpeed = ARANMath.EnrouteWindSpeed(start.ConstructAltitude);
			double coef = WSpeed / ARANMath.DegToRad(Rv);

			//MAX DISTANCE + =================================================================

			LPTYDist = end.LPT - OverlapDist;
			fDistTreshold = LPTYDist;

			MaxDist = LPTYDist;
			//MAX DISTANCE - =================================================================

			//ptTmp = ARANFunctions.LocalToPrj(StartFIX.PrjPt, EntryDir - ARANMath.C_PI * BYTE(FlyMode = fmFlyBy), StartFIX.LPT, 0);
			//start.CalcTurnRangePoints();
			double LPT = -measureFIX.LPT;
			//if (PrevLeg != null)
			//	LPT = -PrevLeg.EndFIX.LPT;

			ptTmp = ARANFunctions.LocalToPrj(measureFIX.PrjPt, EntryDir, LPT - 1.0, 0);

			//_UI.DrawPointWithText(BasePoints[0], 255, "BasePoints[0]");
			//_UI.DrawPointWithText(measureFIX.BasePoints[0], 255, "Base [0]");
			//_UI.DrawPointWithText(ptTmp, -1, "LPT");

			//List<Point> BasePoints;
			//Point[] bpt;

			//BasePoints = ARANFunctions.GetBasePoints(start.PrjPt, EntryDir, PrevLeg.PrimaryArea[0], TurnDir);
			//for (int gh = 0; gh < BasePoints.Count; gh++)
			//    _UI.DrawPointWithText(BasePoints[gh], 0, "pt-" + gh);

			//i = 1;
			//foreach (Point pppt in measureFIX.BasePoints)
			//{
			//	_UI.DrawPointWithText(pppt, -1, "BPt-" + i);
			//	i++;
			//}
			//LegBase.ProcessMessages();

			double dirN0 = ARANFunctions.ReturnAngleInRadians(measureFIX.BasePoints[measureFIX.BasePoints.Count - 1], measureFIX.BasePoints[0]);
			double dir01 = ARANFunctions.ReturnAngleInRadians(measureFIX.BasePoints[0], measureFIX.BasePoints[1]);
			double angleChange = ARANMath.SubtractAngles(dir01, dirN0);                                             //dir01- dirN0;//

			//_UI.DrawPointWithText(ptTmp, 255, "LPT");
			//_UI.DrawPointWithText(start.PrjPt, 255, "FIX");
			//LineString lstr = new LineString();
			//lstr.Add(ARANFunctions.LocalToPrj(ptTmp, EntryDir, 0, -10000));
			//lstr.Add(ptTmp);
			//lstr.Add(ARANFunctions.LocalToPrj(ptTmp, EntryDir, 0, 10000));
			//_UI.DrawLineString(lstr, 255, 2);
			//LegBase.ProcessMessages();

			ptFrom = null;

			//for (int gh = 0; gh < start.BasePoints.Count; gh++)
			//    _UI.DrawPointWithText(start.BasePoints[gh], 0, "pt-" + gh);

			//_UI.DrawRing(PrevLeg.PrimaryArea[0].ExteriorRing, 233,eFillStyle.sfsCross);
			//LegBase.ProcessMessages();

			//List<Point> BasePoints = new List<Point>();

			if (TurnDir == TurnDirection.CCW)
			{
				if (IsPrimary)
				{
					ASW_OUT0F = measureFIX.ASW_2_R;

					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDir - 0.5 * ARANMath.C_PI, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, StartFIX.PrjPt, OutDir - 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						//_UI.DrawPointWithText(ptFrom, 0, "ptFrom");
						//LegBase.ProcessMessages();

						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}

					ASW_OUT0C = 0.5 * measureFIX.SemiWidth;
					ASW_OUT1 = 0.5 * end.SemiWidth;
					dRad = 0;
					InnerBasePoint = measureFIX.BasePoints[1];
				}
				else
				{
					ASW_OUT0F = measureFIX.ASW_R;
					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir - 0.5 * ARANMath.C_PI, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, StartFIX.PrjPt, OutDir - 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						//	_UI.DrawPointWithText(ptFrom, 0, "ptFrom");
						//	LegBase.ProcessMessages();

						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}

					ASW_OUT0C = measureFIX.SemiWidth;
					ASW_OUT1 = end.SemiWidth;
					dRad = ASW_OUT0F - measureFIX.ASW_2_R;

					InnerBasePoint = ARANFunctions.LocalToPrj(measureFIX.BasePoints[1], dir01, measureFIX.ASW_2_R, 0);
				}

				//InnerBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir + 0.5 * ARANMath.C_PI, start.ASW_2_L, 0);
				//OuterBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir - 0.5 * ARANMath.C_PI, start.ASW_2_R, 0);
			}
			else
			{
				if (IsPrimary)
				{
					ASW_OUT0F = measureFIX.ASW_2_L;
					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDir + 0.5 * ARANMath.C_PI, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, StartFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						//_UI.DrawPointWithText(ptFrom, 0, "ptFrom");

						//_UI.DrawRing(PrevLeg.PrimaryArea[0].ExteriorRing, 167, eFillStyle.sfsDiagonalCross);
						//_UI.DrawPointWithText(ptTmp, 255, "LPT");
						//LegBase.ProcessMessages();

						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}

					ASW_OUT0C = 0.5 * measureFIX.SemiWidth;
					ASW_OUT1 = 0.5 * end.SemiWidth;
					dRad = 0;
					InnerBasePoint = measureFIX.BasePoints[1];
				}
				else
				{
					ASW_OUT0F = measureFIX.ASW_L;
					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir + 0.5 * ARANMath.C_PI, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, StartFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}

					ASW_OUT0C = measureFIX.SemiWidth;
					ASW_OUT1 = end.SemiWidth;
					dRad = ASW_OUT0F - measureFIX.ASW_2_L;

					InnerBasePoint = ARANFunctions.LocalToPrj(measureFIX.BasePoints[1], dir01, measureFIX.ASW_2_L, 0);
				}
				//InnerBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir - 0.5 * ARANMath.C_PI, start.ASW_2_R, 0);
				//OuterBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir + 0.5 * ARANMath.C_PI, StartFIX.ASW_2_L, 0);
			}

			//for (int gh = 0; gh < start.BasePoints.Count; gh++)
			//	_UI.DrawPointWithText(start.BasePoints[gh], 0, "pt-" + gh);
			//LegBase.ProcessMessages();

			//if (StartFIX.FlyMode == eFlyMode.AtHeight)
			//{
			//	ASW_OUT0C = ASW_OUT0F;

			//	if (IsPrimary)
			//		InnerBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir + 0.5 * fSide * ARANMath.C_PI, ASW_OUT0F, 0);
			//	else
			//		InnerBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir + 0.5 * fSide * ARANMath.C_PI, 0.5 * ASW_OUT0F, 0);
			//}
			//_UI.DrawPointWithText(InnerBasePoint, 255, "InnerBasePoint");
			//_UI.DrawPointWithText(OuterBasePoint, 255, "OuterBasePoint");
			//_UI.DrawPointWithText(ptFrom, 0, "ptFrom");
			//_UI.DrawPointWithText(ptTmp, 255, "ptTmp");
			//LegBase.ProcessMessages();

			TurnR = TurnR + dRad;

			//if (t == 0)
			ASW_OUTMax = Math.Max(ASW_OUT0C, ASW_OUT1);
			//else
			//ASW_OUTMax = ASW_OUT0C;

			Ring result = new Ring();

			//_UI.DrawPointWithText(ptFrom, 0, "ptFrom");
			//LegBase.ProcessMessages();
			//Point ptLPT = ARANFunctions.LocalToPrj(start.PrjPt, EntryDir, LPT, 0);

			if (-LPT < measureFIX.EPT)
			{
				ptTmp = ARANFunctions.LocalToPrj(start.PrjPt, EntryDir - ARANMath.C_PI, measureFIX.EPT + 0.5, ASW_OUT0F * fSide);

				//_UI.DrawPointWithText(ptTmp, -1, "ptTmp");
				//LegBase.ProcessMessages();

				result.Add(ptTmp);
			}

			if (ptFrom == null)
				ptFrom = ARANFunctions.LocalToPrj(start.PrjPt, EntryDir, -measureFIX.EPT, -ASW_OUT0F * fSide);

			//_UI.DrawPointWithText(ptFrom, 0, "ptFrom");
			//LegBase.ProcessMessages();

			result.Add(ptFrom);

			OuterBasePoint = (Point)ptFrom.Clone();

			//InnerBasePoint = ARANFunctions.LocalToPrj(start.BasePoints[1], dir01, start.ASW_2_R, 0);
			//_UI.DrawPointWithText(OuterBasePoint, 0, "OuterBasePoint");
			//_UI.DrawPointWithText(InnerBasePoint, 0, "InnerBasePoint--");
			///_UI.DrawPointWithText(ptFrom, 0, "ptFrom");
			//LegBase.ProcessMessages();
			//	start.CalcExtraTurnRangePoints();
			//InnerBasePoint = ARANFunctions.LocalToPrj(start.BasePoints[1], dir01, measureFIX.ASW_2_R, 0);

			ptCurr = null;
			//=============================================================================
			if (TurnAng > angleChange)
				AztEnd1 = EntryDir + angleChange * fSide;
			else
				AztEnd1 = OutDir;

			SpStartDir = EntryDir - ARANMath.C_PI_2 * fSide;
			//SpStartDir += SpTurnAng * fSide;
			SpStartRad = SpStartDir;
			BulgeAngle = Math.Atan2(coef, TurnR) * fSide;

			SpToAngle = ARANFunctions.SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir - BulgeAngle, AztEnd1, TurnDir);

			//ptCnt = ARANFunctions.LocalToPrj(InnerBasePoint, EntryDir + 0.5 * ARANMath.C_PI * fSide, TurnR - dRad, 0);
			//Dist0 = TurnR + SpToAngle * coef;
			//ptTmp = ARANFunctions.LocalToPrj(ptCnt, SpStartDir, Dist0, 0);

			//ptCnt = ARANFunctions.LocalToPrj(OuterBasePoint, EntryDir + ARANMath.C_PI_2 * fSide, TurnR - dRad, 0);
			//_UI.DrawPointWithText(ptCnt, 0, "ptCnt-0");
			//LegBase.ProcessMessages();
			//	SpTurnAng = SpiralTouchAngleOld(TurnR, coef, EntryDir, OutDir, TurnDir);

			SpTurnAng = ARANFunctions.SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir - BulgeAngle, OutDir, TurnDir);
			ptCnt = ARANFunctions.LocalToPrj(OuterBasePoint, EntryDir + ARANMath.C_PI_2 * fSide, TurnR, 0);

			Dist0 = TurnR + SpTurnAng * coef;
			ptTmp = ARANFunctions.LocalToPrj(ptCnt, SpStartDir + SpTurnAng * fSide, Dist0, 0);

			//Dist0 = TurnR + SpToAngle * coef;
			//ptTmp = ARANFunctions.LocalToPrj(ptCnt, EntryDir + (SpToAngle - 0.5 * ARANMath.C_PI) * fSide, Dist0, 0);

			//	_UI.DrawPointWithText(ptCnt, -1, "ptCnt-1");
			//	_UI.DrawPointWithText(ptTmp, 0, "ptSp-1");
			//	_UI.DrawPointWithText(StartFIX.PrjPt, 0, "FIX");
			//	_UI.DrawPointWithText(OuterBasePoint, -1, "OuterBasePoint-0");
			//	LegBase.ProcessMessages();

			SpAbeamDist = -fSide * ARANFunctions.PointToLineDistance(ptTmp, start.PrjPt, OutDir);

			HaveSecondSP = (TurnAng >= angleChange + SplayAngle15) || ((TurnAng >= angleChange - DivergenceAngle30) && (SpAbeamDist > ASW_OUT0C));

			if (HaveSecondSP)
				AztEnd2 = EntryDir + angleChange * fSide;           //0.5 * ARANMath.C_PI 
			else
				AztEnd2 = AztEnd1;

			//for (int gh = 0; gh < start.BasePoints.Count; gh++)
			//	_UI.DrawPointWithText(start.BasePoints[gh], 0, "pt-" + gh);
			//LegBase.ProcessMessages();

			//if (TurnAng > 0.5 * ARANMath.C_PI)
			//    AztEnd1 = EntryDir + 0.5 * ARANMath.C_PI * fSide;
			//else
			//    AztEnd1 = OutDir;

			//SpToAngle = ARANFunctions.SpiralTouchAngleOld(TurnR, coef, EntryDir, AztEnd1, TurnDir);
			SpTurnAng = ARANFunctions.SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir - BulgeAngle, EntryDir, TurnDir);

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
				//LegBase.ProcessMessages();
				result.Add(ptCurr);
			}

			//Ring rr = new Ring();
			//rr.Assign(result);
			//_UI.DrawRing(rr, -1, eFillStyle.sfsHorizontal);
			//_UI.DrawPointWithText(ptCurr, ARANFunctions.RGB(0, 128, 0), (i + 1).ToString());
			//while(true)

			//_UI.DrawRing(result, -1, eFillStyle.sfsHorizontal);
			//LegBase.ProcessMessages();

			//_UI.DrawPointWithText(ptCurr, ARANFunctions.RGB(0, 128, 0), (i + 1).ToString());
			//LegBase.ProcessMessages();

			Dist0 = TurnR + SpToAngle * coef;
			ptCurr1 = ptCurr;
			ptCurr = ARANFunctions.LocalToPrj(ptCnt, EntryDir + (SpToAngle - 0.5 * ARANMath.C_PI) * fSide, Dist0, 0);

			//ptTmp = ARANFunctions.LocalToPrj(ptCnt, SpStartDir + SpTurnAng * fSide, Dist0, 0);

			ptInter = ARANFunctions.LineLineIntersect(ptCurr1, EntryDir, ptCurr, AztEnd1) as Point; //Center of buffer circle

			//_UI.DrawPointWithText(ptCurr1, ARANFunctions.RGB(0, 128, 0), "ptCurr0");
			//_UI.DrawPointWithText(ptCurr, ARANFunctions.RGB(0, 128, 0), "ptCurr");
			//_UI.DrawPointWithText(ptInter, ARANFunctions.RGB(0, 128, 0), "ptInter");
			//LegBase.ProcessMessages();

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
					//LegBase.ProcessMessages();

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

			//_UI.DrawRing(result, -1, eFillStyle.sfsHorizontal);
			//_UI.DrawPointWithText(ptCurr, -1, "ptCurr");
			//LegBase.ProcessMessages();

			result.Add(ptCurr);

			//LineString lst = new LineString();
			//lst.Add(wptTransitions[0].PrjPt);
			//lst.Add(ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir - 0.5 * ARANMath.C_PI * fSide, ASW_OUTMax, 0));
			//_UI.DrawLineString(lst, 0, 2);
			//LegBase.ProcessMessages();

			//_UI.DrawPointWithText(ptBase, ARANFunctions.RGB(0, 128, 0), "ptBase");
			//LegBase.ProcessMessages();

			BaseDir = OutDir;

			SpStartDir += SpToAngle * fSide;
			double SpFromAngle = SpToAngle;

			CurDist = ARANFunctions.PointToLineDistance(ptCurr, end.PrjPt, OutDir + 0.5 * ARANMath.C_PI);
			CurWidth = -fSide * ARANFunctions.PointToLineDistance(ptCurr, start.PrjPt, OutDir);

			//_UI.DrawPointWithText(ptCurr, -1, "ptCurr1");
			//LegBase.ProcessMessages();
			//double SpAngle = OutDir;

			if (start.SensorType != eSensorType.GNSS)
			{
				Dist0 = ARANMath.Hypot(start.PrjPt.X - end.PrjPt.X, start.PrjPt.Y - end.PrjPt.Y);
				//double LegLenght = ARANMath.Hypot(end.PrjPt.X - start.PrjPt.X, end.PrjPt.Y - start.PrjPt.Y);

				double dPhi1 = Math.Atan2(ASW_OUT0C - ASW_OUT1, Dist0);
				//if (dPhi1 > DivergenceAngle30)
				//	dPhi1 = DivergenceAngle30;

				if (dPhi1 < 0.0)
				{
					if (dPhi1 < -SplayAngle15)
						dPhi1 = -SplayAngle15;
				}
				else if (dPhi1 > DivergenceAngle30)
					dPhi1 = DivergenceAngle30;

				BaseDir = OutDir + dPhi1 * fSide;

				ptBase = ARANFunctions.LocalToPrj(end.PrjPt, OutDir - 0.5 * ARANMath.C_PI * fSide, ASW_OUT1, 0);

				if (CurWidth < ASW_OUTMax)
					SplayAngle = OutDir - SplayAngle15 * fSide;
				else
					SplayAngle = OutDir + DivergenceAngle30 * fSide;

				ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, SplayAngle, ptBase, BaseDir);
				ptInterDist = ARANFunctions.PointToLineDistance(ptInter, end.PrjPt, OutDir + 0.5 * ARANMath.C_PI);

				//_UI.DrawPointWithText(ptCurr, -1, "ptCurr1");
				//_UI.DrawPointWithText(ptInter, -1, "ptInter-1");
				//_UI.DrawPointWithText(ptBase, -1, "ptBase");
				//LegBase.ProcessMessages();

				MaxDist = Math.Max(MaxDist, ptInterDist);
				//ASW_OUTMax = -fSide * ARANFunctions.PointToLineDistance(ptInter, end.PrjPt, BaseDir);
				//ASW_OUTMax = -fSide * ARANFunctions.PointToLineDistance(ptInter, end.PrjPt, OutDir);
			}
			else
				ptBase = ARANFunctions.LocalToPrj(end.PrjPt, OutDir - 0.5 * ARANMath.C_PI * fSide, ASW_OUTMax, 0);



			fTmp = ARANMath.Modulus(AztEnd2 - AztEnd1, ARANMath.C_2xPI);
			//(HaveSecondSP or ((fTmp > EpsilonRadian) and (fTmp < PI)))then
			if (CurDist - LPTYDist > ARANMath.EpsilonDistance && (HaveSecondSP || fTmp > ARANMath.EpsilonRadian))
			{
				//_UI.DrawPointWithText(ptCurr, -1, "ptCurr-1");
				//_UI.DrawRing(result, -1, eFillStyle.sfsForwardDiagonal);
				//LegBase.ProcessMessages();

				//fTmp = ARANFunctions.PointToLineDistance(ptCurr, ptBase, BaseDir - ARANMath.C_PI_2);
				//_UI.DrawPointWithText(ARANFunctions.LocalToPrj(ptBase, BaseDir, fTmp), -1, "ptCurr-1'");
				//_UI.DrawPointWithText(ptBase, -1, "ptBase");

				//_UI.DrawPointWithText(ptCnt, -1, "ptCnt");
				//LegBase.ProcessMessages();

				Delta = -fSide * ARANFunctions.PointToLineDistance(ptCurr, ptBase, BaseDir);

				if (Delta > ARANMath.EpsilonDistance && fTmp > ARANMath.EpsilonRadian)
				//if (CurWidth - ASW_OUTMax > ARANMath.EpsilonDistance && fTmp > ARANMath.EpsilonRadian)
				{
					//if (fTmp > ARANMath.EpsilonRadian && fTmp < ARANMath.C_PI) 
					//if (fTmp > ARANMath.EpsilonRadian)
					//{
					SpToAngle = ARANFunctions.SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir - BulgeAngle, AztEnd2, TurnDir);
					//SpToAngle = SpiralTouchAngleOld(TurnR, coef, EntryDir, AztEnd2, TurnDir);
					SpTurnAng = SpToAngle - SpFromAngle;

					if (SpTurnAng >= 0)
					{
						n = (int)Math.Round(ARANMath.RadToDeg(SpTurnAng));
						if (n < 1) n = 1;
						else if (n < 5) n = 5;
						else if (n < 10) n = 10;

						dAlpha = SpTurnAng / n;
						bFlag = false;

						ptTmp = ARANFunctions.PrjToLocal(ptBase, BaseDir, ptCurr);
						PrevX = ptTmp.X;
						PrevY = ptTmp.Y;

						for (i = 1; i <= n; i++)
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
							//    //LegBase.ProcessMessages();
							//    break;
							//}

							if ( ((ptTmp.Y * fSide >= 0.0 && PrevY * fSide <= 0.0) || ptTmp.X >= 0.0))  //i > 0 &&
							{
								if (ptTmp.X < 0.0) K = -PrevY / (ptTmp.Y - PrevY);
								else K = -PrevX / (ptTmp.X - PrevX);

								ptCurr.X = result[result.Count - 1].X + K * (ptCurr.X - result[result.Count - 1].X);
								ptCurr.Y = result[result.Count - 1].Y + K * (ptCurr.Y - result[result.Count - 1].Y);

								result.Add(ptCurr);
								//_UI.DrawPointWithText(ptCurr, ARANFunctions.RGB(0, 128, 0), (i + 1).ToString());

								//bFlag = true;
								break;
							}

							PrevX = ptTmp.X;
							PrevY = ptTmp.Y;
							result.Add(ptCurr);
							//_UI.DrawPointWithText(ptCurr, ARANFunctions.RGB(0, 128, 0), (i + 1).ToString());
						}

						//LegBase.ProcessMessages();
						SpStartDir += SpTurnAng * fSide;
						SpFromAngle = SpToAngle;

						//_UI.DrawRing(result, -1, eFillStyle.sfsHorizontal);
						//LegBase.ProcessMessages();
					}
					//}
				}

				//_UI.DrawRing(result, -1, eFillStyle.sfsForwardDiagonal);
				//LegBase.ProcessMessages();

				if (HaveSecondSP)
				{
					ptCnt = ARANFunctions.LocalToPrj(InnerBasePoint, EntryDir + 0.5 * ARANMath.C_PI * fSide, TurnR - 2.0 * dRad, 0);
					R = TurnR + SpFromAngle * coef;
					ptCurr1 = ARANFunctions.LocalToPrj(ptCnt, SpStartDir, R, 0);

					//LineString lst = new LineString();
					//lst.Add(ptCurr1);
					//lst.Add(ARANFunctions.LocalToPrj(ptCurr1, dir01, 500, 0));
					//_UI.DrawLineString(lst, -1, 1);

					//lst.Add(ptBase);
					//lst.Add(ARANFunctions.LocalToPrj(ptBase, BaseDir + ARANMath.C_PI, CurDist, 0));
					//_UI.DrawLineString(lst, 255 << 8, 1);
					//_UI.DrawPointWithText(ptCurr1, -1, "ptCurr1");
					//_UI.DrawPointWithText(ptCnt, -1, "ptCnt-1");
					//LegBase.ProcessMessages();

					Delta = -fSide * ARANFunctions.PointToLineDistance(ptCurr1, ptBase, BaseDir);   //
					fTmp = ARANMath.Modulus(dir01 - BaseDir, 2 * ARANMath.C_PI);

					if (Math.Abs(fTmp) < ARANMath.EpsilonRadian || Math.Abs(fTmp - ARANMath.C_2xPI) < ARANMath.EpsilonRadian || Math.Abs(fTmp - ARANMath.C_PI) < ARANMath.EpsilonRadian)
					{
						ptCurr = ptCurr1;
						result.Add(ptCurr1);
					}
					else if (Delta < -ARANMath.EpsilonDistance)
					{
						ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, dir01, ptBase, BaseDir);
						//_UI.DrawPointWithText(ptInter, -1, "ptInter");
						//LegBase.ProcessMessages();

						if (ptInter != null)
						{
							fTmp = ARANFunctions.PointToLineDistance(ptInter, end.PrjPt, OutDir + ARANMath.C_PI_2);
							Dist0 = ARANFunctions.PointToLineDistance(ptInter, ptCurr, OutDir + ARANMath.C_PI_2);

							if (Dist0 < 0)
							{
								if (fTmp >= 0)
								{
									ptCurr = ptInter;
									result.Add(ptCurr);
								}
								else
								{
									ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, dir01, end.PrjPt, OutDir + ARANMath.C_PI_2);

									//_UI.DrawPointWithText(ptInter, -1, "ptInter-1");
									//LegBase.ProcessMessages();

									ptCurr = ptInter;
									result.Add(ptCurr);
								}
							}
						}

						//LineString lst = new LineString();
						//lst.Add(ptCurr);
						//lst.Add(ARANFunctions.LocalToPrj(ptCurr, dir01 + ARANMath.C_PI, 15000, 0));
						//_UI.DrawLineString(lst, 255<<8,2);

						//_UI.DrawRing(result, -1, eFillStyle.sfsBackwardDiagonal); 
						//LegBase.ProcessMessages();
					}
					else
					{
						ptCurr = ptCurr1;
						result.Add(ptCurr1);
					}
				}
			}


			CurWidth = -fSide * ARANFunctions.PointToLineDistance(ptCurr, start.PrjPt, OutDir);
			CurDist = ARANFunctions.PointToLineDistance(ptCurr, end.PrjPt, OutDir + 0.5 * ARANMath.C_PI);

			//=============================================================================

			//if (start.SensorType != eSensorType.GNSS)
			//{
			//Dist0 = Hypot(wptTransitions[0].PrjPt.X - StartFIX.PrjPt.X, wptTransitions[0].PrjPt.Y - StartFIX.PrjPt.Y);
			//dPhi1 = ArcTan2(ASW_OUT0C - ASW_OUT1, Dist0);
			//if dPhi1 > DivergenceAngle30 then dPhi1 = DivergenceAngle30;

			//		SpAngle = OutDir - dPhi1 * fSide;
			//		LocalToPrj(StartFIX.PrjPt, OutDir + 0.5 * PI * fSide, ASW_OUT0C, 0, ptBase);

			//		if (CurWidth < ASW_OUTMax) then
			//			SplayAngle := OutDir + SplayAngle15 * fSide
			//		else
			//			SplayAngle := OutDir - DivergenceAngle30 * fSide;

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
			Delta = -fSide * ARANFunctions.PointToLineDistance(ptCurr, ptBase, BaseDir);

			if (CurDist - LPTYDist > ARANMath.EpsilonDistance && (HaveSecondSP || (Math.Abs(Delta) > ARANMath.EpsilonDistance)))
			{
				if (Delta > ARANMath.EpsilonDistance)
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

						ptTmp = ARANFunctions.PrjToLocal(ptBase, BaseDir, ptCurr);
						PrevX = ptTmp.X;
						PrevY = ptTmp.Y;

						//_UI.DrawRing(result, -1, eFillStyle.sfsBackwardDiagonal);
						//LegBase.ProcessMessages();

						for (i = 1; i <= n; i++)
						{
							R = TurnR + (SpFromAngle + i * dAlpha) * coef;
							ptCurr = ARANFunctions.LocalToPrj(ptCnt, SpStartDir + i * dAlpha * fSide, R, 0);
							ptTmp = ARANFunctions.PrjToLocal(ptBase, BaseDir, ptCurr);

							//LineString lst = new LineString();
							//lst.Add(ptBase);
							//lst.Add(ARANFunctions.LocalToPrj(ptBase, BaseDir, 63000, 0));
							//_UI.DrawLineString(lst, -1, 2);

							//_UI.DrawPointWithText(ptBase, -1, "Base");
							//_UI.DrawPointWithText(ptCurr, -1, "ptCurr-" + i.ToString());
							//LegBase.ProcessMessages();

							//Experiment  6:20 PM 4/5/2016
							if (!bFlag)
							{
								if (ptTmp.Y * fSide <= 0.0 && ptTmp.X <= 0.0)
									bFlag = true;
								if (!bFlag)
									break;
							}
							else //if (i > 0)
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

									//if (ptTmp.X > 0) K = -PrevY / (ptTmp.Y - PrevY);
									//else K = -PrevX / (ptTmp.X - PrevX);

									ptCurr.X = result[result.Count - 1].X + K * (ptCurr.X - result[result.Count - 1].X);
									ptCurr.Y = result[result.Count - 1].Y + K * (ptCurr.Y - result[result.Count - 1].Y);

									result.Add(ptCurr);

									//_UI.DrawPointWithText(ptCurr, 0, "spOut - 2");
									//LegBase.ProcessMessages();
									break;
								}
							}

							//if (i == n && IsPrimary)	end.JointFlags = end.JointFlags | 1;

							PrevX = ptTmp.X;
							PrevY = ptTmp.Y;
							result.Add(ptCurr);

							//_UI.DrawPointWithText(ptCurr, -1, "ptCurr - " + i.ToString());
							//LegBase.ProcessMessages(true);
						}

						//_UI.DrawRing(result, -1, eFillStyle.sfsHorizontal);
						//LegBase.ProcessMessages();
					}
				}
				else if (Delta > ARANMath.EpsilonDistance)
				{
					SplayAngle = OutDir - SplayAngle15 * fSide;

					//_UI.DrawPointWithText(ptCurr, 0, "ptCurr");
					//_UI.DrawPointWithText(ptBase, 0, "ptBase");
					//_UI.DrawPointWithText(wptTransitions[0].PrjPt, -1, "wptTrans-0");
					//_UI.DrawPointWithText(start.PrjPt, -1, "start");
					//LegBase.ProcessMessages();

					ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, SplayAngle, ptBase, BaseDir);
					//	_UI.DrawPointWithText(ptInter, 0, "ptInter - 2");
					//	LegBase.ProcessMessages();

					ptInterDist = ARANFunctions.PointToLineDistance(ptInter, end.PrjPt, OutDir + 0.5 * ARANMath.C_PI);

					if (ptInterDist < CurDist)
					{
						Point ptCut = ARANFunctions.LocalToPrj(end.PrjPt, OutDir + ARANMath.C_PI, CurDist, 0);
						ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, SplayAngle, ptCut, OutDir + 0.5 * ARANMath.C_PI);

						//_UI.DrawPointWithText(ptInter, -1, "ptInter - 3");
						//_UI.DrawPointWithText(ptCut, -1, "ptCut");
						//LegBase.ProcessMessages();
					}

					//_UI.DrawPointWithText(ptInter, 0, "ptInter - 2");
					//LegBase.ProcessMessages(true);

					result.Add(ptInter);
				}
			}

			//LegBase.ProcessMessages(true);
			//	_UI.DrawRing(result, ARANFunctions.RGB(0, 0, 255), eFillStyle.sfsDiagonalCross);
			//	LegBase.ProcessMessages();

			//Ring rr = new Ring();
			//rr.Assign(result);
			//_UI.DrawRing(rr, -1, eFillStyle.sfsHorizontal);
			//LegBase.ProcessMessages();

			JoinSegments(result, true, IsPrimary, TurnDir, direction);
			//	_UI.DrawRing(result, ARANFunctions.RGB(0, 0, 255), eFillStyle.sfsDiagonalCross);
			//	LegBase.ProcessMessages(true);

			return result;
		}

		Ring CreateInnerTurnArea(EnRouteLeg PrevLeg, bool IsPrimary, CodeDirection direction)
		{
			double TurnAng,
				CurDist, MaxDist, CurWidth, ptInterDist, LPTYDist,
				fTmp, DivergenceAngle30, SplayAngle15, BaseDir, LegLenght,//SpiralDivergenceAngle, 
				SplayAngle, fSide, ASW_INMax, ASW_IN0C, ASW_IN0F, ASW_IN1;  //, Dist0, Dist1, dPhi2, azt0, Dist2;

			Point InnerBasePoint, ptTmp, ptBase, ptCurr, ptCut, ptInter, ptFrom = null;

			//bool ReCalcASW_IN0C;

			WayPoint start, end, measureFIX;
			TurnDirection TurnDir;
			double EntryDir;
			double OutDir;

			if (direction == CodeDirection.FORWARD)
			{
				start = _StartFIX;
				end = _EndFIX;

				if (PrevLeg != null)
					measureFIX = (WayPoint)PrevLeg.EndFIX.Clone();
				else
					measureFIX = (WayPoint)start.Clone();

				EntryDir = _StartFIX.EntryDirection;
				OutDir = _StartFIX.OutDirection;
				TurnDir = _StartFIX.EffectiveTurnDirection;
			}
			else
			{
				start = _EndFIX;
				end = _StartFIX;

				if (PrevLeg != null)
					measureFIX = (WayPoint)PrevLeg.StartFIX.Clone();
				else
					measureFIX = (WayPoint)start.Clone();

				EntryDir = _EndFIX.OutDirection + Math.PI;
				OutDir = _EndFIX.EntryDirection + Math.PI;
				TurnDir = (TurnDirection)(TurnDirection.NONE - _EndFIX.EffectiveTurnDirection);

				measureFIX.TurnDirection = TurnDir;
				measureFIX.EntryDirection = EntryDir;
				measureFIX.OutDirection = OutDir;
			}

			fSide = (int)TurnDir;

			TurnAng = ARANMath.Modulus((OutDir - EntryDir) * fSide, ARANMath.C_2xPI);

			if (TurnAng < ARANMath.EpsilonRadian || Math.Abs(TurnAng - ARANMath.C_2xPI) < ARANMath.EpsilonRadian)
				TurnAng = 0.0;

			if (TurnAng <= ARANMath.DegToRad(minTurnAngle))
				return CreateInnerTurnAreaLT(PrevLeg, IsPrimary, direction);

			//SpiralDivergenceAngle = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;

			//if (IsPrimary)
			//{
			//	//fTmp = GlobalVars.constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;
			//	DivergenceAngle30 = Math.Atan(0.5 * Math.Tan(SpiralDivergenceAngle));

			//	fTmp = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
			//	SplayAngle15 = Math.Atan(0.5 * Math.Tan(fTmp));
			//}
			//else

			DivergenceAngle30 = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;
			SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;

			//MAX DISTANCE + =================================================================

			LPTYDist = end.LPT - OverlapDist;

			MaxDist = LPTYDist;
			//MAX DISTANCE - =================================================================
			double EPT = measureFIX.EPT;

			ptTmp = ARANFunctions.LocalToPrj(start.PrjPt, EntryDir - ARANMath.C_PI, EPT, 0);

				//_UI.DrawPointWithText(ptTmp, -1, "EPT");
				//LegBase.ProcessMessages();

			if (TurnDir == TurnDirection.CCW)
			{
				if (IsPrimary)
				{
					ASW_IN0F = measureFIX.ASW_2_L;
					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDir + 0.5 * ARANMath.C_PI, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, StartFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_IN0F = fTmp;
					}

					ASW_IN0C = 0.5 * measureFIX.SemiWidth;
					ASW_IN1 = 0.5 * end.SemiWidth;
				}
				else
				{
					ASW_IN0F = measureFIX.ASW_L;
					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir + 0.5 * ARANMath.C_PI, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, StartFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_IN0F = fTmp;
					}

					ASW_IN0C = measureFIX.SemiWidth;
					ASW_IN1 = end.SemiWidth;
				}
				InnerBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir + 0.5 * ARANMath.C_PI, ASW_IN0F, 0);
			}
			else
			{
				if (IsPrimary)
				{
					ASW_IN0F = measureFIX.ASW_2_R;
					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDir - 0.5 * ARANMath.C_PI, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, StartFIX.PrjPt, OutDir - 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_IN0F = fTmp;
					}

					ASW_IN0C = 0.5 * measureFIX.SemiWidth;
					ASW_IN1 = 0.5 * end.SemiWidth;
				}
				else
				{
					ASW_IN0F = measureFIX.ASW_R;
					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir - 0.5 * ARANMath.C_PI, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, StartFIX.PrjPt, OutDir - 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_IN0F = fTmp;
					}

					ASW_IN0C = measureFIX.SemiWidth;
					ASW_IN1 = end.SemiWidth;
				}
				InnerBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir - 0.5 * ARANMath.C_PI, ASW_IN0F, 0);
			}

			ASW_INMax = Math.Max(ASW_IN0C, ASW_IN1);
			Ring result = new Ring();
			ptCurr = InnerBasePoint;

			//_UI.DrawPointWithText(ptTmp, 0, "ptTmp");
			//_UI.DrawPointWithText(ptFrom, 0, "ptFrom");
			//_UI.DrawPointWithText(ptCurr, 0, "InnerBasePoint");
			//LegBase.ProcessMessages();

			result.Add(ptTmp);
			result.Add(ptCurr);

			//_UI.DrawPointWithText(ptFrom, 0, "ptCurr-0");
			//LegBase.ProcessMessages(true);

			//	_UI.DrawPointWithText(ptFrom, 0, "ptFrom-I1");
			//	LegBase.ProcessMessages(true);
			//result.Add(ptCurr);

			double tmpY;
			ARANFunctions.PrjToLocal(ptCurr, OutDir, end.PrjPt, out CurDist, out tmpY);
			//CurDist = ARANFunctions.PointToLineDistance(ptCurr, end.PrjPt, OutDir + 0.5 * ARANMath.C_PI);

			LegLenght = ARANMath.Hypot(end.PrjPt.X - start.PrjPt.X, end.PrjPt.Y - start.PrjPt.Y);
			//	if (CurDist > MaxDist)
			{
				//==============================================================================

				if (start.SensorType != eSensorType.GNSS)
				{
					double dPhi1 = Math.Atan2(ASW_IN0C - ASW_IN1, LegLenght);

					if (dPhi1 < 0.0)
					{
						if (dPhi1 < -SplayAngle15)
							dPhi1 = -SplayAngle15;
					}
					else if (dPhi1 > DivergenceAngle30)
						dPhi1 = DivergenceAngle30;

					BaseDir = OutDir - dPhi1 * fSide;
					ptBase = ARANFunctions.LocalToPrj(end.PrjPt, OutDir + 0.5 * ARANMath.C_PI * fSide, ASW_IN1, 0);
				}
				else
				{
					BaseDir = OutDir;
					//ptBase = ARANFunctions.LocalToPrj(end.PrjPt, OutDir + 0.5 * ARANMath.C_PI * fSide, ASW_IN0C, 0);
					ptBase = ARANFunctions.LocalToPrj(end.PrjPt, OutDir + 0.5 * ARANMath.C_PI * fSide, ASW_IN1, 0);     //?????????????????????
				}

				//_UI.DrawPointWithText(ptBase, 0, "ptBase");
				//LegBase.ProcessMessages();

				CurWidth = fSide * ARANFunctions.PointToLineDistance(ptCurr, ptBase, BaseDir);
				if (Math.Abs(CurWidth) > ARANMath.EpsilonDistance)
				{
					if (CurWidth > 0)
						SplayAngle = EntryDir + 0.5 * TurnAng * fSide;
					else
					{
						//SplayAngle15 = GlobalVars.constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
						SplayAngle = OutDir + SplayAngle15 * fSide;
					}

					ptInter = (Point)ARANFunctions.LineLineIntersect(InnerBasePoint, SplayAngle, ptBase, BaseDir);

					//_UI.DrawPointWithText(ptBase, 0, "ptBase - 1");
					//_UI.DrawPointWithText(ptInter, 0, "ptInter - 1");
					//LegBase.ProcessMessages();

					//ptInterDist = ARANFunctions.PointToLineDistance(ptInter, end.PrjPt, OutDir + 0.5 * ARANMath.C_PI);
					ARANFunctions.PrjToLocal(ptInter, OutDir, end.PrjPt, out ptInterDist, out tmpY);

					if (ptInterDist > CurDist)
					{
						ptCut = ARANFunctions.LocalToPrj(end.PrjPt, OutDir + ARANMath.C_PI, CurDist, 0);
						ptInter = (Point)ARANFunctions.LineLineIntersect(InnerBasePoint, SplayAngle, ptCut, OutDir + 0.5 * ARANMath.C_PI);
						//_UI.DrawPointWithText(ptInter, 0, "ptInter - 2");
						//LegBase.ProcessMessages();
					}
					else if (ptInterDist < 0.0)
						ptInter = (Point)ARANFunctions.LineLineIntersect(InnerBasePoint, SplayAngle, end.PrjPt, OutDir + 0.5 * ARANMath.C_PI);

					result.Add(ptInter);
				}
			}

			//Ring rr = new Ring();
			//rr.Assign(result);
			//_UI.DrawRing(rr, -1, eFillStyle.sfsHorizontal);
			//LegBase.ProcessMessages();

			JoinSegments(result, false, IsPrimary, TurnDir, direction);
			return result;
		}

		bool createProtectionArea(EnRouteLeg PrevLeg, CodeDirection direction)
		{
			Point ptTmp, ptLPTMeasure, ptEPTMeasure;
			Ring InnerRingP = null, OuterRingP = null;
			Ring InnerRingB = null, OuterRingB = null;

			if (PrevLeg == null)
			{
				//double dir = ARANFunctions.ReturnAngleInRadians(_StartFIX.PrjPt, _EndFIX.PrjPt);

				if (direction == CodeDirection.FORWARD)
					_StartFIX.EntryDirection = _StartFIX.OutDirection;
				else
					_EndFIX.OutDirection = _EndFIX.EntryDirection;
			}

			_StartFIX.CalcTurnRangePoints();
			_EndFIX.CalcTurnRangePoints();

			OuterRingP = CreateOuterTurnArea(PrevLeg, true, direction);
			//LegBase.ProcessMessages(true);
			//_UI.DrawRing(OuterRingP, ARANFunctions.RGB(255, 0, 0), eFillStyle.sfsCross);
			//LegBase.ProcessMessages();

			InnerRingP = CreateInnerTurnArea(PrevLeg, true, direction);
			//_UI.DrawRing(InnerRingP, ARANFunctions.RGB(0, 255, 0), eFillStyle.sfsCross);
			//LegBase.ProcessMessages();

			OuterRingB = CreateOuterTurnArea(PrevLeg, false, direction);
			//_UI.DrawRing(OuterRingB, ARANFunctions.RGB(255, 0, 0), eFillStyle.sfsCross);
			//LegBase.ProcessMessages();

			InnerRingB = CreateInnerTurnArea(PrevLeg, false, direction);
			//_UI.DrawRing(InnerRingB, ARANFunctions.RGB(0, 255, 0), eFillStyle.sfsCross);
			//LegBase.ProcessMessages();

			OuterRingP.AddReverse(InnerRingP);
			//_UI.DrawRing(OuterRingP, ARANFunctions.RGB(255, 0, 0), eFillStyle.sfsCross);
			//LegBase.ProcessMessages();

			OuterRingB.AddReverse(InnerRingB);
			//_UI.DrawRing(OuterRingB, ARANFunctions.RGB(255, 0, 0), eFillStyle.sfsDiagonalCross);
			//LegBase.ProcessMessages();

			//=====================================================================
			MultiPolygon PrimaryArePolygon, BufferPolygon = null;
			MultiPolygon FullArePolygon;

			GeometryOperators GeoOperators = new GeometryOperators();

			Polygon tmpPolygon = new Polygon();
			tmpPolygon.ExteriorRing = OuterRingP;

			PrimaryArePolygon = new MultiPolygon();
			PrimaryArePolygon.Add(tmpPolygon);

			tmpPolygon = new Polygon();
			tmpPolygon.ExteriorRing = OuterRingB;

			BufferPolygon = new MultiPolygon();
			BufferPolygon.Add(tmpPolygon);

			//LegBase.ProcessMessages(true);
			//_UI.DrawMultiPolygon(BufferPolygon, -1, eFillStyle.sfsDiagonalCross);
			//LegBase.ProcessMessages();

			FullArePolygon = (MultiPolygon)GeoOperators.UnionGeometry(BufferPolygon, PrimaryArePolygon);

			//_UI.DrawMultiPolygon(FullArePolygon, -1, eFillStyle.sfsDiagonalCross);
			//LegBase.ProcessMessages();

			double OutDir = _StartFIX.OutDirection;
			this.PrimaryArea = PrimaryArePolygon;
			this.FullArea = FullArePolygon;

			TurnDirection TurnDir = _EndFIX.TurnDirection;

			if (TurnDir == TurnDirection.NONE)
			{
				ptLPTMeasure = (Point)_EndFIX.PrjPt.Clone();
				ptEPTMeasure = (Point)_EndFIX.PrjPt.Clone();
				TurnDir = TurnDirection.CW;
			}
			else
			{
				ptLPTMeasure = ARANFunctions.LocalToPrj(_EndFIX.PrjPt, OutDir - ARANMath.C_PI, _EndFIX.LPT, 0);
				ptEPTMeasure = ARANFunctions.LocalToPrj(_EndFIX.PrjPt, OutDir - ARANMath.C_PI, _EndFIX.EPT, 0);
			}

			//====================================================================================================================
			double fSide = (int)TurnDir, ASW_OUT1F = 0.0, ASW_IN1F = 0.0;

			if (PrimaryArePolygon.Count > 0)
			{
				ptTmp = ARANFunctions.RingVectorIntersect(PrimaryArePolygon[0].ExteriorRing, ptEPTMeasure, OutDir - 0.5 * ARANMath.C_PI * fSide, out ASW_IN1F);
				if (ptTmp != null)
				{
					if (TurnDir == TurnDirection.CCW)
						_EndFIX.ASW_2_L = ASW_IN1F;
					else
						_EndFIX.ASW_2_R = ASW_IN1F;
				}

				ptTmp = ARANFunctions.RingVectorIntersect(PrimaryArePolygon[0].ExteriorRing, ptLPTMeasure, OutDir + 0.5 * ARANMath.C_PI * fSide, out ASW_OUT1F);
				if (ptTmp != null)
				{
					if (TurnDir == TurnDirection.CCW)
						_EndFIX.ASW_2_R = ASW_OUT1F;
					else
						_EndFIX.ASW_2_L = ASW_OUT1F;
				}
			}
			//====================================================================================================================

			//_UI.DrawPolygon(FullArePolygon[2], -1, eFillStyle.sfsDiagonalCross);
			//LegBase.ProcessMessages(true);
			//MultiPolygon faPolygon = ARANFunctions.RemoveAgnails(FullArePolygon);

			//faPolygon = ARANFunctions.RemoveAgnails(faPolygon);
			//_UI.DrawMultiPolygon(faPolygon, -1, eFillStyle.sfsDiagonalCross);
			//LegBase.ProcessMessages();

			if (FullArePolygon.Count > 0)
			{
				ptTmp = ARANFunctions.RingVectorIntersect(FullArePolygon[0].ExteriorRing, ptEPTMeasure, OutDir - 0.5 * ARANMath.C_PI * fSide, out ASW_IN1F);
				if (ptTmp != null)
				{
					if (TurnDir == TurnDirection.CCW)
						_EndFIX.ASW_L = ASW_IN1F;
					else
						_EndFIX.ASW_R = ASW_IN1F;
				}

				ptTmp = ARANFunctions.RingVectorIntersect(FullArePolygon[0].ExteriorRing, ptLPTMeasure, OutDir + 0.5 * ARANMath.C_PI * fSide, out ASW_OUT1F);
				if (ptTmp != null)
				{
					if (TurnDir == TurnDirection.CCW)
						_EndFIX.ASW_R = ASW_OUT1F;
					else
						_EndFIX.ASW_L = ASW_OUT1F;
				}
			}
			//====================================================================================================================
			return true;
		}

		void createAssesmentArea(EnRouteLeg PrevLeg, CodeDirection direction)
		{
			if (PrevLeg == null)
			{
				this.FullAssesmentArea = (MultiPolygon)this.FullArea;
				this.PrimaryAssesmentArea = (MultiPolygon)this.PrimaryArea;
				return;
			}

			WayPoint start, end, measureFIX;
			TurnDirection TurnDir;
			double EntryDir, OutDir;

			if (direction == CodeDirection.FORWARD)
			{
				start = _StartFIX;
				end = _EndFIX;

				if (PrevLeg != null)
					measureFIX = PrevLeg.EndFIX;
				else
					measureFIX = start;

				EntryDir = _StartFIX.EntryDirection;
				OutDir = _StartFIX.OutDirection;
				TurnDir = _StartFIX.EffectiveTurnDirection;
			}
			else
			{
				start = _EndFIX;
				end = _StartFIX;

				if (PrevLeg != null)
					measureFIX = PrevLeg.StartFIX;
				else
					measureFIX = start;

				EntryDir = _EndFIX.OutDirection + Math.PI;
				OutDir = _EndFIX.EntryDirection + Math.PI;
				TurnDir = (TurnDirection)(TurnDirection.NONE - _EndFIX.EffectiveTurnDirection);
			}

			double Dir0 = EntryDir - ARANMath.C_PI_2;
			double Dir1 = EntryDir + ARANMath.C_PI_2;
			double LineLen = 10 * end.ASW_R + end.ASW_L;

			Point ptTmp0, ptTmp1;
			//current leg =======================================================================================

			ptTmp0 = ARANFunctions.LocalToPrj(measureFIX.PrjPt, EntryDir, -measureFIX.EPT, -LineLen);   //startFIX.ASW_L + 0.15
			ptTmp1 = ARANFunctions.LocalToPrj(measureFIX.PrjPt, EntryDir, -measureFIX.EPT, LineLen);    //startFIX.ASW_R + 0.15

			LineString KKLinePart = new LineString();
			KKLinePart.Add(ptTmp0);
			KKLinePart.Add(ptTmp1);

			//_UI.DrawMultiPolygon(this._FullArea, -1, eFillStyle.sfsBackwardDiagonal);
			//_UI.DrawLineString(KKLinePart, -1, 2);
			//LegBase.ProcessMessages();
			// =======================================================================================

			ptTmp0 = ARANFunctions.LocalToPrj(measureFIX.PrjPt, EntryDir, measureFIX.ATT, -LineLen);
			ptTmp1 = ARANFunctions.LocalToPrj(measureFIX.PrjPt, EntryDir, measureFIX.ATT, LineLen);

			LineString LSPLinePart = new LineString();
			LSPLinePart.Add(ptTmp0);
			LSPLinePart.Add(ptTmp1);

			//_UI.DrawLineString(LSPLinePart, -1, 2);
			//LegBase.ProcessMessages();
			// =======================================================================================

			ptTmp0 = ARANFunctions.LocalToPrj(measureFIX.PrjPt, EntryDir, 0.0, -LineLen);
			ptTmp1 = ARANFunctions.LocalToPrj(measureFIX.PrjPt, EntryDir, 0.0, LineLen);

			LineString FIXLinePart = new LineString();
			FIXLinePart.Add(ptTmp0);
			FIXLinePart.Add(ptTmp1);

			//_UI.DrawLineString(FIXLinePart, -1, 2);
			//LegBase.ProcessMessages();
			// =======================================================================================


			Geometry geom1, geom2;
			MultiPolygon poly1, poly2;

			GeometryOperators GeoOperators = new GeometryOperators();
			if (!GeoOperators.Disjoint(this._FullArea, KKLinePart))
			{
				try
				{
					//_UI.DrawMultiPolygon(this._FullArea, -1, eFillStyle.sfsBackwardDiagonal);
					//_UI.DrawLineString(KKLinePart, -1, 2);
					//LegBase.ProcessMessages(true);

					GeoOperators.Cut(this._FullArea, KKLinePart, out geom1, out geom2);

					poly1 = (MultiPolygon)geom1;
					poly2 = (MultiPolygon)geom2;

					if (poly1.Area < poly2.Area)
						poly1 = poly2;

					if (poly1.IsEmpty)
						this.FullAssesmentArea = this._FullArea;
					else
						this.FullAssesmentArea = poly1;
				}
				catch
				{
					this.FullAssesmentArea = this._FullArea;
				}
			}
			else
			{
				try
				{
					MultiPolygon tmpPoly;
					GeoOperators.Cut(PrevLeg._FullArea, KKLinePart, out geom1, out geom2);
					GeoOperators.Cut(geom2, FIXLinePart, out geom1, out geom2);
					tmpPoly = (MultiPolygon)GeoOperators.UnionGeometry(geom1, this._FullArea);

					if (tmpPoly.IsEmpty)
						this.FullAssesmentArea = this._FullArea;
					else
						this.FullAssesmentArea = tmpPoly;
				}
				catch
				{
					this.FullAssesmentArea = this._FullArea;
				}
			}

			if (!GeoOperators.Disjoint(this._PrimaryArea, KKLinePart))
			{
				try
				{

					GeoOperators.Cut(this._PrimaryArea, KKLinePart, out geom1, out geom2);
					poly1 = (MultiPolygon)geom1;
					poly2 = (MultiPolygon)geom2;

					if (poly1.Area < poly2.Area)
						poly1 = poly2;

					if (poly1.IsEmpty)
						this.PrimaryAssesmentArea = this._PrimaryArea;
					else
						this.PrimaryAssesmentArea = poly1;
				}
				catch
				{
					this.PrimaryAssesmentArea = this._PrimaryArea;
				}
			}
			else
			{
				try
				{
					MultiPolygon tmpPoly;
					GeoOperators.Cut(PrevLeg._PrimaryArea, KKLinePart, out geom1, out geom2);
					GeoOperators.Cut(geom2, FIXLinePart, out geom1, out geom2);
					tmpPoly = (MultiPolygon)GeoOperators.UnionGeometry(geom1, this._PrimaryArea);

					//_UI.DrawMultiPolygon(tmpPoly, -1, eFillStyle.sfsBackwardDiagonal);
					//LegBase.ProcessMessages();

					if (tmpPoly.IsEmpty)
						this.PrimaryAssesmentArea = this._PrimaryArea;
					else
						this.PrimaryAssesmentArea = tmpPoly;
				}
				catch
				{
					this.PrimaryAssesmentArea = this._PrimaryArea;
				}
			}

			//previus leg =======================================================================================
			//_UI.DrawPointWithText(ptTmp0, -1, "ptTmp0");
			//_UI.DrawPointWithText(ptTmp1, -1, "ptTmp1");
			//_UI.DrawLineString(KKLinePart, -1, 2);
			//LegBase.ProcessMessages();

			//_UI.DrawMultiPolygon(this.FullAssesmentArea, -1, eFillStyle.sfsBackwardDiagonal);
			//_UI.DrawMultiPolygon(this.PrimaryAssesmentArea, -1, eFillStyle.sfsForwardDiagonal);
			//LegBase.ProcessMessages();

			//LegBase.ProcessMessages();
			//_UI.DrawMultiPolygon(PrevLeg.FullAssesmentArea, -1, eFillStyle.sfsBackwardDiagonal);
			//_UI.DrawMultiPolygon(PrevLeg.PrimaryAssesmentArea, -1, eFillStyle.sfsForwardDiagonal);
			//LegBase.ProcessMessages();

			GeoOperators.Cut(PrevLeg.FullAssesmentArea, LSPLinePart, out geom1, out geom2);
			PrevLeg.FullAssesmentArea = (MultiPolygon)geom1;

			GeoOperators.Cut(PrevLeg.PrimaryAssesmentArea, LSPLinePart, out geom1, out geom2);
			PrevLeg.PrimaryAssesmentArea = (MultiPolygon)geom1;

			double fSide = (int)TurnDir;
			double TurnAng = ARANMath.Modulus((OutDir - EntryDir) * fSide, ARANMath.C_2xPI);

			if (TurnAng < ARANMath.EpsilonRadian || Math.Abs(TurnAng - ARANMath.C_2xPI) < ARANMath.EpsilonRadian)
				TurnAng = 0.0;

			if (TurnAng > 0.0 && TurnAng <= ARANMath.DegToRad(minTurnAngle))
			{
				GeoOperators.Cut(PrevLeg.FullAssesmentArea, KKLinePart, out geom1, out geom2);

				//MultiPolygon BaseAssesmentArea = (MultiPolygon)geom1;
				MultiPolygon AddAssesmentArea = (MultiPolygon)GeoOperators.Intersect(geom2, FullAssesmentArea);
				PrevLeg.FullAssesmentArea = (MultiPolygon)GeoOperators.UnionGeometry(geom1, AddAssesmentArea);

				GeoOperators.Cut(PrevLeg.PrimaryAssesmentArea, KKLinePart, out geom1, out geom2);
				//BaseAssesmentArea = (MultiPolygon)geom1;
				AddAssesmentArea = (MultiPolygon)GeoOperators.Intersect(geom2, PrimaryAssesmentArea);
				PrevLeg.PrimaryAssesmentArea = (MultiPolygon)GeoOperators.UnionGeometry(geom1, AddAssesmentArea);
			}
		}
		//====================================================================================================
	}
}
