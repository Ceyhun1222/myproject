using System;
using Aran.AranEnvironment;
using Aran.Geometries;
using Aran.PANDA.Constants;
using Aran.Geometries.Operators;

namespace Aran.PANDA.Common
{

	[System.Runtime.InteropServices.ComVisible(false)]
	public class LegArrival : LegBase
	{
		public LegArrival(WayPoint FIX0, WayPoint FIX1, IAranEnvironment aranEnv, LegBase PrevLeg = null) :
			base(FIX0, FIX1, aranEnv, PrevLeg)
		{
		}

		override protected void JoinSegments(Ring LegRing, ADHPType ARP, bool IsOuter, bool IsPrimary, double EntryDir, TurnDirection TurnDir)
		{
			double OutDir = _StartFIX.OutDirection;
			int transitions = 0;

			WayPoint[] wptTransitions = new WayPoint[3];
			WayPoint prev = _StartFIX;
			Point ptTransition = ARANFunctions.CircleVectorIntersect(ARP.pPtPrj, PANSOPSConstantList.PBNTerminalTriggerDistance, _EndFIX.PrjPt, OutDir + Math.PI);

			//_UI.DrawPointWithText(ptTransition, -1, "ptTransition - 1");
			//ProcessMessages();

			if (!ptTransition.IsEmpty)
			{
				Point ptS = ARANFunctions.PrjToLocal(ptTransition, OutDir, prev.PrjPt);
				Point ptE = ARANFunctions.PrjToLocal(ptTransition, OutDir, _EndFIX.PrjPt);

				if (ptS.X * ptE.X < 0.0)
				{
					WayPoint WPT_30NM = new WayPoint(eFIXRole.TP_, _aranEnv);
					WPT_30NM.Assign(prev);
					WPT_30NM.PrjPt = ptTransition;

					WPT_30NM.FlightPhase = eFlightPhase.STAR;
					if (WPT_30NM.PBNType == ePBNClass.RNAV5)
						WPT_30NM.PBNType = ePBNClass.RNAV1;

					WPT_30NM.BankAngle = prev.BankAngle;
					WPT_30NM.EntryDirection = OutDir;
					WPT_30NM.OutDirection = OutDir;

					wptTransitions[transitions] = WPT_30NM;
					transitions++;

					prev = WPT_30NM;

					double bank = _EndFIX.BankAngle;
					_EndFIX.FlightPhase = eFlightPhase.STAR;
					if (_EndFIX.FlightPhase == eFlightPhase.STAR && _EndFIX.PBNType == ePBNClass.RNAV5)
						_EndFIX.PBNType = ePBNClass.RNAV1;

					_EndFIX.BankAngle = bank;
				}
			}

			ptTransition = ARANFunctions.CircleVectorIntersect(ARP.pPtPrj, PANSOPSConstantList.PBNTerminalTriggerDistance, _EndFIX.PrjPt, OutDir);
			//_UI.DrawPointWithText(ptTransition, -1, "ptTransition - 2");
			//ProcessMessages();

			if (!ptTransition.IsEmpty)
			{
				Point ptS = ARANFunctions.PrjToLocal(ptTransition, OutDir, prev.PrjPt);
				Point ptE = ARANFunctions.PrjToLocal(ptTransition, OutDir, _EndFIX.PrjPt);

				if (ptS.X * ptE.X < 0.0)
				{
					WayPoint WPT_30NM = new WayPoint(eFIXRole.TP_, _aranEnv);
					WPT_30NM.Assign(prev);
					WPT_30NM.PrjPt = ptTransition;

					WPT_30NM.FlightPhase = eFlightPhase.STARGE56;
					WPT_30NM.BankAngle = prev.BankAngle;
					WPT_30NM.EntryDirection = OutDir;
					WPT_30NM.OutDirection = OutDir;

					wptTransitions[transitions] = WPT_30NM;
					transitions++;

					double bank = _EndFIX.BankAngle;
					_EndFIX.FlightPhase = eFlightPhase.STARGE56;
					_EndFIX.BankAngle = bank;
				}
			}

			wptTransitions[transitions] = (WayPoint)_EndFIX.Clone();
			transitions++;

			double SpiralDivergenceAngle, SpiralSplayAngle,
				DivergenceAngle30 = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value,
				SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;

			if (IsPrimary)// && _StartFIX.TurnAngle < ARANMath.DegToRad(5))
			{
				SpiralDivergenceAngle = Math.Atan(0.5 * Math.Tan(DivergenceAngle30));
				SpiralSplayAngle = Math.Atan(0.5 * Math.Tan(SplayAngle15));
			}
			else
			{
				SpiralDivergenceAngle = DivergenceAngle30;
				SpiralSplayAngle = SplayAngle15;
			}

			//=================================================================================
			//double fSide = -(int)TurnDir;
			Point ptCurr;
			double fSide = IsOuter ? -((int)TurnDir) : ((int)TurnDir);
			bool normalWidth = (ARANMath.RadToDeg(_StartFIX.TurnAngle) <= 10 ||
				(_StartFIX.FlyMode == eFlyMode.Flyby && _StartFIX.Role != eFIXRole.FAF_ && ARANMath.RadToDeg(_StartFIX.TurnAngle) <= 30));

			WayPoint lStartFIX = new WayPoint(_aranEnv);
			WayPoint lEndFIX = new WayPoint(_aranEnv);

			lStartFIX.Assign(_StartFIX);

			for (int pass = 0; pass < transitions; pass++, lStartFIX.Assign(lEndFIX))
			{
				lEndFIX.Assign(wptTransitions[pass]);
				lEndFIX.CalcTurnRangePoints();

				bool expansAtTransition = pass != transitions - 1;

				double ASW_0C = IsPrimary ? 0.5 * lStartFIX.SemiWidth : lStartFIX.SemiWidth;
				double ASW_1C = IsPrimary ? 0.5 * lEndFIX.SemiWidth : lEndFIX.SemiWidth;

				double tmpDist = lEndFIX.FlyMode == eFlyMode.Flyby ? lEndFIX.LPT - OverlapDist : OverlapDist;
				if (!expansAtTransition)
					tmpDist = (lEndFIX.FlyMode == eFlyMode.Flyby ? lEndFIX.LPT : -lEndFIX.LPT) - OverlapDist;

				//=================================================================================
				Point ptEndOfLeg = ARANFunctions.LocalToPrj(lEndFIX.PrjPt, OutDir + ARANMath.C_PI, tmpDist, 0);
				ptCurr = (Point)LegRing[LegRing.Count - 1].Clone();
				Point ptExpExt;

				//_UI.DrawPointWithText(ptEndOfLeg, -1, "ptEndOfLeg");
				//ProcessMessages();

				if (expansAtTransition)
				{
					//ProcessMessages(true);
					//Point ptTmp = ARANFunctions.CircleVectorIntersect(ARP.pPtPrj, PANSOPSConstantList.PBNTerminalTriggerDistance, _EndFIX.PrjPt, OutDir + Math.PI);
					//_UI.DrawPointWithText(ptTmp, -1, "ptTran");
					//ProcessMessages();

					double DistToTransit;
					if (lStartFIX.SemiWidth > lEndFIX.SemiWidth)
						DistToTransit = (lStartFIX.SemiWidth - WayPoint.WPTBuffers[(int)lStartFIX.FlightPhase] - 1.5 * lEndFIX.XTT) / Math.Tan(DivergenceAngle30);
					else
						DistToTransit = (WayPoint.WPTBuffers[(int)lStartFIX.FlightPhase] + 1.5 * lEndFIX.XTT - lStartFIX.SemiWidth) / Math.Tan(SplayAngle15);

					ptExpExt = ARANFunctions.LocalToPrj(lEndFIX.PrjPt, OutDir, -DistToTransit, 0);
				}
				else
				{
					if (ASW_0C > ASW_1C)
					{
						double DistToTransit = (lStartFIX.SemiWidth - lEndFIX.SemiWidth) / Math.Tan(_constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value) - lEndFIX.ATT;
						ptExpExt = ARANFunctions.LocalToPrj(lEndFIX.PrjPt, OutDir, -DistToTransit, 0);
						//ptExpExt = ARANFunctions.LocalToPrj(lEndFIX.PrjPt, OutDir, -lEndFIX.ATT, 0);      //point	of begin of Expansion/Extract
					}
					else
						ptExpExt = ARANFunctions.LocalToPrj(lEndFIX.PrjPt, OutDir, -lEndFIX.ATT, 0);
				}

				//_constants.Pansops[ePANSOPSData.rnvImMinDist].Value;
				//_UI.DrawPointWithText(ptCurr, "ptCurr-1");
				//_UI.DrawPointWithText(ptEndOfLeg, "ptEndOfLeg");
				//_UI.DrawPointWithText(ptExpExt, "ptExpExt");
				//_UI.DrawPointWithText(lEndFIX.PrjPt, "EndFIX");
				//ProcessMessages();

				double DistToExp, DistToEndOfLeg = ARANFunctions.PointToLineDistance(ptCurr, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
				DistToExp = ARANFunctions.PointToLineDistance(ptCurr, ptExpExt, OutDir + ARANMath.C_PI_2);

				if (DistToExp < ARANMath.EpsilonDistance)
				{
					ptExpExt = ARANFunctions.LocalToPrj(ptEndOfLeg, OutDir, -DistToEndOfLeg, 0);
					DistToExp = ARANFunctions.PointToLineDistance(ptCurr, ptExpExt, OutDir + ARANMath.C_PI_2);
				}

				//_UI.DrawPointWithText(ptExpExt, -1, "ptExpExt-2");
				//ProcessMessages();
				Point ptBase0, ptBase1 = null;
				double BaseDir0 = 0;    //, BaseDir1;

				int exp0 = 0, exp1 = 0;

				if (DistToEndOfLeg > ARANMath.EpsilonDistance)
				{
					if (lStartFIX.Role == eFIXRole.FAF_ && lEndFIX.Role == eFIXRole.MAPt_)
					{
						double LegLenght = ARANMath.Hypot(_EndFIX.PrjPt.X - _StartFIX.PrjPt.X, _EndFIX.PrjPt.Y - _StartFIX.PrjPt.Y);
						double dPhi1 = Math.Atan2(ASW_0C - ASW_1C, LegLenght);

						if (dPhi1 > DivergenceAngle30)
							dPhi1 = DivergenceAngle30;

						BaseDir0 = OutDir + dPhi1 * fSide;
						ptBase0 = ARANFunctions.LocalToPrj(lEndFIX.PrjPt, OutDir + ARANMath.C_PI_2 * fSide, ASW_0C, 0);
						ptBase1 = ARANFunctions.LocalToPrj(lEndFIX.PrjPt, OutDir + ARANMath.C_PI_2 * fSide, ASW_1C, 0);
					}
					else
					{
						BaseDir0 = OutDir;
						ptBase0 = ARANFunctions.LocalToPrj(ptExpExt, OutDir + ARANMath.C_PI_2 * fSide, ASW_1C, 0);

						ptBase1 = ptBase0;
					}

					//_UI.DrawPointWithText(ptBase0, "ptBase0");
					//ProcessMessages();

					if (DistToExp > 0.0)
					{
						double ASW_0F = fSide * ARANFunctions.PointToLineDistance(ptCurr, lStartFIX.PrjPt, OutDir); //Abs ????????
						Point ptInter;

						if (Math.Abs(ASW_0F - ASW_0C) > ARANMath.EpsilonDistance)// && (IsOuter || normalWidth))
						{
							double Direction;
							if (ASW_0F > ASW_0C)
							{
								Direction = OutDir - DivergenceAngle30 * fSide;
								exp0 = 1;
							}
							else
							{
								Direction = OutDir + SplayAngle15 * fSide;
								exp0 = 2;
							}

							Point ptTmp = ARANFunctions.LocalToPrj(ptExpExt, OutDir, 0, fSide * ASW_0C);
							Geometry pGeom = ARANFunctions.LineLineIntersect(ptCurr, Direction, ptTmp, OutDir);
							//_UI.DrawPointWithText(ptTmp, "ptTmp");
							//_UI.DrawPointWithText(ptCurr, "ptCurr");
							//ProcessMessages();


							if (pGeom.Type != GeometryType.Point)
								ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, Direction, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
							else
								ptInter = (Point)pGeom;

							//_UI.DrawPointWithText(ptInter, "ptInter");
							//ProcessMessages();

							DistToExp = ARANFunctions.PointToLineDistance(ptInter, ptExpExt, OutDir + ARANMath.C_PI_2);
							DistToEndOfLeg = ARANFunctions.PointToLineDistance(ptInter, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
							double Dist0 = ARANFunctions.PointToLineDistance(ptInter, ptCurr, OutDir - ARANMath.C_PI_2);

							//if(DistToEndOfLeg < -ARANMath.EpsilonDistance || Dist0 < -ARANMath.EpsilonDistance) // || ARANFunctions.PointToLineDistance(ptInter, lEndFIX.PrjPt, OutDir - ARANMath.C_PI_2) > 0.0

							if (Dist0 < -ARANMath.EpsilonDistance)
							{
								ptInter = ptTmp;
							}
							else if ((DistToExp < 0.0 && DistToEndOfLeg < -ARANMath.EpsilonDistance) || Dist0 < -ARANMath.EpsilonDistance)
							{
								ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, Direction, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
							}

							//DistToEndOfLeg = ARANFunctions.PointToLineDistance(ptInter, ptEndOfLeg, OutDir + ARANMath.C_PI_2);

							//if ((DistToExp < 0.0 && DistToEndOfLeg < -ARANMath.EpsilonDistance) || Dist0 < -ARANMath.EpsilonDistance)
							//{
							//	double delta = fSide * ARANFunctions.PointToLineDistance(ptInter, ptBase1, OutDir);

							//	if (delta < ARANMath.EpsilonDistance)
							//	{
							//		if (ASW_0F < ASW_0C && exp0 == 2)
							//			;
							//		else
							//			BaseDir0 = OutDir - DivergenceAngle30 * fSide;
							//	}
							//	else if (ASW_0F > ASW_0C)
							//		BaseDir0 = OutDir + SplayAngle15 * fSide;


							//ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, BaseDir0, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
							//_UI.DrawPointWithText(ptInter, -1, "ptInter-3");
							//ProcessMessages();

							//DistToEndOfLeg = 0.0;
							//}

							//_UI.DrawPointWithText(ptInter, "ptInter-1");
							//ProcessMessages();

							ptCurr = ptInter;
							LegRing.Add(ptCurr);
						}

						DistToExp = ARANFunctions.PointToLineDistance(ptCurr, ptExpExt, OutDir + ARANMath.C_PI_2);

						if (DistToExp > ARANMath.EpsilonDistance) //&& expansAtTransition
						{
							ptCurr = (Point)ARANFunctions.LineLineIntersect(ptCurr, OutDir, ptExpExt, OutDir + ARANMath.C_PI_2);
							DistToExp = 0.0;
							LegRing.Add(ptCurr);
						}

						DistToEndOfLeg = ARANFunctions.PointToLineDistance(ptCurr, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
						//_UI.DrawPointWithText(ptCurr, "ptCurr-1");
						//ProcessMessages();

						//ASW_0F = fSide * ARANFunctions.PointToLineDistance(ptCurr, lStartFIX.PrjPt, OutDir);
					}

					normalWidth = true;

					//LineString ls = new LineString();
					//ls.Add(ptCurr);
					//ls.Add(ARANFunctions.LocalToPrj(ptCurr, BaseDir1, 10000, 0));
					//_UI.DrawLineString(ls, 255, 2);
					//ProcessMessages();

					//_UI.DrawPointWithText(ARANFunctions.LocalToPrj(ptCurr, BaseDir1, 10000, 0), 255, "BaseDir");
					//_UI.DrawPointWithText(ptBase1, 255, "ptBase1");
					//ProcessMessages();

					//_UI.DrawPointWithText(ptCurr, -1, "ptCurr-2");
					//ProcessMessages();

					double Delta = fSide * ARANFunctions.PointToLineDistance(ptCurr, ptBase1, BaseDir0);

					if (DistToEndOfLeg > ARANMath.EpsilonDistance && Math.Abs(Delta) > ARANMath.EpsilonDistance)
					{
						double Direction;

						if (Delta > ARANMath.EpsilonDistance)
						{
							Direction = OutDir - SpiralDivergenceAngle * fSide;
							exp1 = 1;
						}
						else
						{
							Direction = OutDir + SpiralSplayAngle * fSide;
							exp1 = 2;
						}

						//double DistToTransition = ARANFunctions.PointToLineDistance(ptCurr, ptExpExt, OutDir + ARANMath.C_PI_2);

						//if (DistToTransition < DistToEndOfLeg)
						//{
						//	ptCurr = ARANFunctions.LocalToPrj(ptExpExt, OutDir + ARANMath.C_PI, 0, -fSide * ASW_0C);
						//	LegRing.Add(ptCurr);
						//	DistToEndOfLeg = ARANFunctions.PointToLineDistance(ptCurr, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
						//}

						//_UI.DrawPointWithText(ptCurr, -1, "ptCurr-2");
						//ProcessMessages();

						//Delta = fSide * ARANFunctions.PointToLineDistance(ptCurr, ptBase1, BaseDir0);
						//}

						if (expansAtTransition || DistToExp < DistToEndOfLeg)
						{
							Point ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, Direction, ptBase1, BaseDir0);
							DistToEndOfLeg = ARANFunctions.PointToLineDistance(ptInter, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
							double Dist0 = ARANFunctions.PointToLineDistance(ptInter, ptCurr, OutDir - ARANMath.C_PI_2);

							if (DistToEndOfLeg < -ARANMath.EpsilonDistance && !expansAtTransition)   //if (DisstToEndOfLeg < 0 && Dist0 < 0)	//
							{
								ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, Direction, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
								DistToEndOfLeg = ARANFunctions.PointToLineDistance(ptInter, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
							}

							ptCurr = ptInter;

							//_UI.DrawPointWithText(ptCurr, -1, "ptCurr-3");
							//ProcessMessages();
							LegRing.Add(ptCurr);
						}
					}

					if (DistToEndOfLeg > ARANMath.EpsilonDistance)
					{
						ptCurr = ARANFunctions.LocalToPrj(ptEndOfLeg, OutDir, 0, ASW_1C * fSide);
						//_UI.DrawPointWithText(ptCurr, 255, "ptCurr-5");
						//ProcessMessages();
						LegRing.Add(ptCurr);
					}
				}
			}


			//double DistToFIX = (lEndFIX.FlyMode == eFlyMode.Flyby ? _EndFIX.LPT : -_EndFIX.LPT) - OverlapDist;
			//Point ptCnt = ARANFunctions.LocalToPrj(_EndFIX.PrjPt, OutDir + ARANMath.C_PI, DistToFIX, 0);

			//Ring rr = new Ring();
			//rr.Assign(LegRing);
			//_UI.DrawRing(rr, -1, AranEnvironment.Symbols.eFillStyle.sfsHorizontal);
			//ProcessMessages();

			//if (LegRing.Count > 1)
			//{
			//	ptCurr = LegRing[LegRing.Count - 1];
			//	Point ptPrev = LegRing[LegRing.Count - 2];
			//}

			//_UI.DrawPointWithText(ptCurr, -1, "ptCurr");
			//_UI.DrawPointWithText(ptCnt, -1, "ptCnt");
			//ProcessMessages();

			//double Dist1 = ARANFunctions.PointToLineDistance(ptCurr, ptCnt, OutDir + ARANMath.C_PI_2);

			//if (Dist1 < -ARANMath.EpsilonDistance)
			//{
			//	while (Dist1 < -ARANMath.EpsilonDistance && LegRing.Count > 1)
			//	{
			//		Point ptPrev = LegRing[LegRing.Count - 2];

			//		double Dist2 = ARANFunctions.PointToLineDistance(ptPrev, ptCnt, OutDir + ARANMath.C_PI_2);
			//		if (Dist2 < 0.0)
			//		{
			//			LegRing.Remove(LegRing.Count - 1);

			//			ptCurr = ptPrev;
			//			Dist1 = Dist2;
			//			continue;
			//		}

			//		double k = Dist2 / (Dist2 - Dist1);
			//		ptCurr.X = ptPrev.X + k * (ptCurr.X - ptPrev.X);
			//		ptCurr.Y = ptPrev.Y + k * (ptCurr.Y - ptPrev.Y);

			//		//_UI.DrawPointWithText(ptCurr, -1, "ptCurr-1");
			//		//ProcessMessages();

			//		//LegRing.Add(ptCurr);
			//		break;
			//	}
			//}

		}

		//override protected Ring CreateOuterTurnAreaLT(LegBase PrevLeg, ADHPType ARP, bool IsPrimary, double EntryDir, TurnDirection TurnDir)
		//{
		//	double OutDir = _StartFIX.OutDirection;
		//	double fSide = (int)TurnDir;
		//	double TurnAng = ARANMath.Modulus((OutDir - EntryDir) * fSide, ARANMath.C_2xPI);

		//	if (TurnAng < ARANMath.EpsilonRadian || Math.Abs(TurnAng - ARANMath.C_2xPI) < ARANMath.EpsilonRadian)
		//		TurnAng = 0.0;

		//	//double SpiralDivergenceAngle = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;
		//	//double SplayAngle15, DivergenceAngle30;


		//	double DivergenceAngle30, SplayAngle15,
		//		SpiralDivergenceAngle = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value,
		//		SpiralSplayAngle = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;

		//	if (IsPrimary && TurnAng < ARANMath.DegToRad(5))
		//	{
		//		DivergenceAngle30 = Math.Atan(0.5 * Math.Tan(SpiralDivergenceAngle));
		//		SplayAngle15 = Math.Atan(0.5 * Math.Tan(SpiralSplayAngle));
		//	}
		//	else
		//	{
		//		DivergenceAngle30 = SpiralDivergenceAngle;
		//		SplayAngle15 = SpiralSplayAngle;
		//	}

		//	//if (IsPrimary)
		//	//{
		//	//	DivergenceAngle30 = SpiralDivergenceAngle;
		//	//	SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
		//	//}
		//	//else
		//	//{
		//	//	DivergenceAngle30 = SpiralDivergenceAngle;
		//	//	SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
		//	//}


		//	//ptTmp = ARANFunctions.LocalToPrj(StartFIX.PrjPt, OutDir, -0.5, 0);
		//	//ptTmp = ARANFunctions.LocalToPrj(StartFIX.PrjPt, EntryDir - ARANMath.C_PI, -0.5, 0);
		//	//ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, FlyMode == eFlyMode.FlyBy ? EntryDir - ARANMath.C_PI : EntryDir, LPT, 0);
		//	//ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, -_StartFIX.EPT + 1.0, 0);
		//	//ptTmp = _StartFIX.PrjPt;

		//	double LPT, fTmp;
		//	//if (_StartFIX.SemiWidth < _EndFIX.SemiWidth)
		//	//	LPT = -_StartFIX.LPT;
		//	LPT = -_StartFIX.EPT - 1.0;


		//	//if (_StartFIX.DFTarget)
		//	//{
		//	//    if (TurnDir == TurnDirection.CW)
		//	//        LPT = _StartFIX.LPT_L;
		//	//    else
		//	//        LPT = _StartFIX.LPT_R;
		//	//}
		//	//else
		//	//    LPT = _StartFIX.LPT;


		//	Point ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, LPT, 0);

		//	if (TurnAng <= 0.034906585039886591)
		//		ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, -1, 0);

		//	GeometryOperators GeoOperators = new GeometryOperators();
		//	if (PrevLeg != null)
		//	{
		//		fTmp = GeoOperators.GetDistance(PrevLeg.PrimaryArea, ptTmp);
		//		if (fTmp > 0)
		//			ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, LPT - fTmp - 1.0);
		//	}

		//	//_UI.DrawPointWithText(ptTmp, "ptTmp");
		//	//ProcessMessages();

		//	//_UI.DrawRing(PrevLeg.PrimaryArea[0].ExteriorRing, 255, eFillStyle.sfsBackwardDiagonal);
		//	//_UI.DrawPoint(ptTmp, 255);
		//	//ProcessMessages();

		//	Point ptFrom = null;
		//	double ASW_OUT0F, ASW_OUT0C;

		//	if (TurnDir == TurnDirection.CCW)
		//	{
		//		if (IsPrimary)
		//		{
		//			ASW_OUT0C = 0.5 * _StartFIX.SemiWidth;
		//			ASW_OUT0F = _StartFIX.ASW_2_R;

		//			if (PrevLeg != null && PrevLeg.PrimaryArea.Count > 0)
		//			{
		//				ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDir - ARANMath.C_PI_2, out fTmp);
		//				//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, StartFIX.PrjPt, OutDir - ARANMath.C_PI_2, OutDir, out fTmp);

		//				if (ptFrom == null)
		//				{
		//					ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, -_StartFIX.EPT + 1.0, 0);
		//					ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDir - ARANMath.C_PI_2, out fTmp);
		//				}

		//				if (ptFrom != null)
		//					ASW_OUT0F = fTmp;
		//			}
		//		}
		//		else
		//		{
		//			ASW_OUT0C = _StartFIX.SemiWidth;
		//			ASW_OUT0F = _StartFIX.ASW_R;
		//			if (PrevLeg != null && PrevLeg.FullArea.Count > 0)
		//			{
		//				ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir - ARANMath.C_PI_2, out fTmp);

		//				//_UI.DrawRing(PrevLeg.FullArea[0].ExteriorRing, 255, eFillStyle.sfsCross);
		//				//_UI.DrawMultiPolygon(PrevLeg.FullArea, 255, eFillStyle.sfsCross);
		//				//_UI.DrawPointWithText(StartFIX.PrjPt, 0, "StartFIX");
		//				//ProcessMessages(true);
		//				//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, StartFIX.PrjPt, OutDir - ARANMath.C_PI_2, OutDir, out fTmp);
		//				if (ptFrom == null)
		//				{
		//					ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, -_StartFIX.EPT + 1.0, 0);
		//					ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir - ARANMath.C_PI_2, out fTmp);
		//				}

		//				if (ptFrom != null)
		//					ASW_OUT0F = fTmp;
		//			}
		//		}
		//	}
		//	else
		//	{
		//		if (IsPrimary)
		//		{
		//			ASW_OUT0C = 0.5 * _StartFIX.SemiWidth;
		//			ASW_OUT0F = _StartFIX.ASW_2_L;
		//			if (PrevLeg != null && PrevLeg.PrimaryArea.Count > 0)
		//			{
		//				ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDir + ARANMath.C_PI_2, out fTmp);
		//				//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, StartFIX.PrjPt, OutDir + ARANMath.C_PI_2, OutDir, out fTmp);
		//				if (ptFrom == null)
		//				{
		//					ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, -_StartFIX.EPT + 1.0, 0);
		//					ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDir + ARANMath.C_PI_2, out fTmp);
		//				}

		//				if (ptFrom != null)
		//					ASW_OUT0F = fTmp;
		//			}
		//		}
		//		else
		//		{
		//			ASW_OUT0C = _StartFIX.SemiWidth;
		//			ASW_OUT0F = _StartFIX.ASW_L;
		//			if (PrevLeg != null && PrevLeg.FullArea.Count > 0)
		//			{
		//				ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir + ARANMath.C_PI_2, out fTmp);
		//				//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, StartFIX.PrjPt, OutDir + ARANMath.C_PI_2, OutDir, out fTmp);
		//				if (ptFrom == null)
		//				{
		//					ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, -_StartFIX.EPT + 1.0, 0);
		//					ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir + ARANMath.C_PI_2, out fTmp);
		//				}

		//				if (ptFrom != null)
		//					ASW_OUT0F = fTmp;
		//			}
		//		}
		//	}

		//	//_UI.DrawPointWithText(ptFrom, 0, "ptFrom-O1");
		//	//ProcessMessages();

		//	Ring result = new Ring();
		//	if (ptFrom != null)
		//	{
		//		result.Add(ptFrom);
		//	}
		//	else
		//	{
		//		ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir - ARANMath.C_PI, _StartFIX.EPT + 10.0, ASW_OUT0F * fSide);
		//		//_UI.DrawPointWithText(ptTmp, 0, "ptTmp");
		//		//ProcessMessages();
		//		result.Add(ptTmp);
		//	}

		//	//if (ptFrom == null)
		//	//{
		//	//ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir - ARANMath.C_PI, _StartFIX.EPT + 10.0, ASW_OUT0F * fSide);
		//	//_UI.DrawPointWithText(ptTmp, 0, "ptTmp");
		//	//ProcessMessages();
		//	//result.Add(ptTmp);

		//	//ptFrom = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir - ARANMath.C_PI, _StartFIX.EPT, ASW_OUT0F * fSide);
		//	ptFrom = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, 0.0, -fSide * ASW_OUT0F);

		//	//_UI.DrawPointWithText(ptTmp, -1, "ptTmp");
		//	//_UI.DrawPointWithText(ptFrom, -1, "ptFrom");
		//	//ProcessMessages();
		//	//}

		//	//Point ptTo = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, OutDir - ARANMath.C_PI, _StartFIX.EPT, ASW_OUT0C * fSide);
		//	Point ptTo = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, OutDir, 0.0, -fSide * ASW_OUT0C);

		//	//ProcessMessages(true);
		//	//_UI.DrawPointWithText(StartFIX.PrjPt, -1, "StartFIX");
		//	//_UI.DrawPointWithText(ptFrom, -1, "ptFrom-O-" + (IsPrimary ? "P" : "S"));
		//	//_UI.DrawPointWithText(ptTo, -1, "ptTo-O-" + (IsPrimary ? "P" : "S"));
		//	//_UI.DrawPointWithText(ptTo, -1, "ptTo-O-" + (IsPrimary ? "P" : "S"));
		//	//ProcessMessages();

		//	double ptDir = Math.Atan2(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);

		//	fTmp = ARANMath.Modulus((ptDir - OutDir) * fSide, ARANMath.C_2xPI);     /////////////////////////?????????????????

		//	if (fTmp > ARANMath.C_PI)
		//		fTmp = fTmp - ARANMath.C_2xPI;

		//	double ptDist = ARANMath.Hypot(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);

		//	if (TurnAng <= 0.034906585039886591)
		//		result.Add(ptFrom);
		//	else if (ptDist > 1.0 && fTmp + ARANMath.EpsilonRadian >= -SplayAngle15 && fTmp - ARANMath.EpsilonRadian <= DivergenceAngle30)
		//	{
		//		ptTmp = ARANFunctions.LocalToPrj(ptFrom, ptDir, 0.5 * ptDist, 0);

		//		//_UI.DrawPointWithText(ptTmp, 128+(128<<8), "ptTmp");
		//		//ProcessMessages();

		//		Geometry Geom0 = ARANFunctions.LineLineIntersect(ptTmp, ptDir + ARANMath.C_PI_2, ptFrom, EntryDir + ARANMath.C_PI_2);
		//		Geometry Geom1 = ARANFunctions.LineLineIntersect(ptTmp, ptDir + ARANMath.C_PI_2, ptTo, OutDir + ARANMath.C_PI_2);
		//		Point ptCnt = null;

		//		if (Geom0.Type == GeometryType.Point)
		//		{
		//			ptCnt = (Point)Geom0;
		//			//_UI.DrawPointWithText(ptCnt, -1, "ptCnt1");
		//			//ProcessMessages();
		//			//_UI.DrawPointWithText((Point)Geom1, 128 + (255 << 8), "ptCnt2");
		//			//ProcessMessages(true);

		//			if (Geom1 != null && Geom1.Type == GeometryType.Point)
		//			{
		//				double Dist0 = ARANMath.Hypot(ptFrom.Y - ptCnt.Y, ptFrom.X - ptCnt.X);
		//				double Dist1 = ARANMath.Hypot(ptFrom.Y - ((Point)Geom1).Y, ptFrom.X - ((Point)Geom1).X);
		//				if (ASW_OUT0F > ASW_OUT0C && Dist1 < Dist0)
		//					ptCnt.Assign(Geom1);
		//			}
		//		}
		//		else if (Geom1.Type == GeometryType.Point)
		//			ptCnt = (Point)Geom1;

		//		if (ptCnt != null)
		//		{
		//			Ring tmpRing = ARANFunctions.CreateArcPrj(ptCnt, ptFrom, ptTo, TurnDir);
		//			//_UI.DrawRing(tmpRing, 0, eFillStyle.sfsSolid);
		//			//ProcessMessages();

		//			//	for (int ffg = 0; ffg < tmpRing.Count; ffg++)
		//			//		_UI.DrawPointWithText(tmpRing[ffg], 128 << 8, "pt" + ffg.ToString());
		//			//					ProcessMessages(true);

		//			result.AddMultiPoint(tmpRing);
		//		}
		//		else
		//			result.Add(ptFrom);
		//	}
		//	else if (fTmp + ARANMath.EpsilonRadian < -SplayAngle15)
		//	{
		//		Geometry Geom0 = ARANFunctions.LineLineIntersect(ptFrom, OutDir - SplayAngle15 * fSide, _StartFIX.PrjPt, OutDir + ARANMath.C_PI_2);
		//		if (Geom0.Type == GeometryType.Point)
		//		{
		//			ptTo = (Point)Geom0;

		//			////ProcessMessages(true);
		//			//_UI.DrawPointWithText(StartFIX.PrjPt, -1, "StartFIX");
		//			//_UI.DrawPointWithText(ptFrom, -1, "ptFrom-O-" + (IsPrimary ? "P" : "S"));
		//			//_UI.DrawPointWithText(ptTo, -1, "ptTo-O-" + (IsPrimary ? "P" : "S"));
		//			//ProcessMessages();

		//			ptDir = Math.Atan2(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);
		//			ptDist = ARANMath.Hypot(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);

		//			ptTmp = ARANFunctions.LocalToPrj(ptFrom, ptDir, 0.5 * ptDist, 0);
		//			Geom0 = ARANFunctions.LineLineIntersect(ptTmp, ptDir + ARANMath.C_PI_2, ptFrom, EntryDir + ARANMath.C_PI_2 - SplayAngle15 * fSide);
		//			Geometry Geom1 = ARANFunctions.LineLineIntersect(ptTmp, ptDir + ARANMath.C_PI_2, ptTo, OutDir + ARANMath.C_PI_2 - SplayAngle15 * fSide);
		//			Point ptCnt = null;

		//			if (Geom0.Type == GeometryType.Point)
		//			{
		//				ptCnt = (Point)Geom0;

		//				if (Geom1 != null && Geom1.Type == GeometryType.Point)
		//				{
		//					double Dist0 = ARANMath.Hypot(ptFrom.Y - ptCnt.Y, ptFrom.X - ptCnt.X);
		//					double Dist1 = ARANMath.Hypot(ptFrom.Y - ((Point)Geom1).Y, ptFrom.X - ((Point)Geom1).X);
		//					if (ASW_OUT0F > ASW_OUT0C && Dist1 < Dist0)
		//						ptCnt.Assign(Geom1);
		//				}
		//			}
		//			else if (Geom1.Type == GeometryType.Point)
		//				ptCnt = (Point)Geom1;

		//			if (ptCnt != null)
		//			{
		//				Ring tmpRing = ARANFunctions.CreateArcPrj(ptCnt, ptFrom, ptTo, TurnDir);
		//				result.AddMultiPoint(tmpRing);
		//			}
		//			else
		//				result.Add(ptFrom);
		//		}
		//		else
		//			result.Add(ptFrom);
		//	}
		//	else
		//		result.Add(ptFrom);

		//	//=============================================================================
		//	//_UI.DrawPointWithText(result[0], -1, "p-1");
		//	//_UI.DrawPointWithText(result[1], -1, "p-2");
		//	//ProcessMessages(true);

		//	//_UI.DrawRing(result, 128 << 8, eFillStyle.sfsCross );
		//	//ProcessMessages(true);

		//	JoinSegments(result, ARP, true, IsPrimary, EntryDir, TurnDir);

		//	//_UI.DrawRing(result, 128 << 8, eFillStyle.sfsCross );
		//	//ProcessMessages(true);

		//	//for (int ffg = 0; ffg < result.Count; ffg++)
		//	//	_UI.DrawPointWithText(result[ffg],128<<8, "pt" +ffg.ToString());
		//	//ProcessMessages(true);

		//	return result;
		//}

		//override protected Ring CreateInnerTurnAreaLT(LegBase PrevLeg, ADHPType ARP, bool IsPrimary, double EntryDir, TurnDirection TurnDir)
		//{
		//	double OutDir = _StartFIX.OutDirection;
		//	double fSide = (int)TurnDir;
		//	double TurnAng = ARANMath.Modulus((OutDir - EntryDir) * fSide, ARANMath.C_2xPI);

		//	if (TurnAng < ARANMath.EpsilonRadian || Math.Abs(TurnAng - ARANMath.C_2xPI) < ARANMath.EpsilonRadian)
		//		TurnAng = 0.0;

		//	//	DivergenceAngle30 = GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
		//	//	SplayAngle15 = GPANSOPSConstants.Constant[arafTrn_OSplay].Value;

		//	double SplayAngle15, DivergenceAngle30, SpiralDivergenceAngle = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;

		//	if (IsPrimary)
		//	{
		//		SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
		//		DivergenceAngle30 = SpiralDivergenceAngle;

		//		////	fTmp = GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
		//		//DivergenceAngle30 = Math.Atan(0.5 * Math.Tan(SpiralDivergenceAngle));
		//		//fTmp = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
		//		//SplayAngle15 = Math.Atan(0.5 * Math.Tan(fTmp));
		//	}
		//	else
		//	{
		//		SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
		//		DivergenceAngle30 = SpiralDivergenceAngle;
		//		//_constants.Constant[arSecAreaCutAngl].Value;
		//	}



		//	double LPT;

		//	//if (_StartFIX.IsDFTarget)
		//	//{
		//	//	if (TurnDir == TurnDirection.CW)
		//	//		LPT = _StartFIX.LPT_L;
		//	//	else
		//	//		LPT = _StartFIX.LPT_R;
		//	//}
		//	//else
		//	//	LPT = _StartFIX.LPT;

		//	LPT = -_StartFIX.EPT - 10.0;
		//	Point ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, LPT);
		//	double fTmp, ASW_IN0F, ASW_IN0C;

		//	GeometryOperators GeoOperators = new GeometryOperators();
		//	if (PrevLeg != null)
		//	{
		//		fTmp = GeoOperators.GetDistance(PrevLeg.PrimaryArea, ptTmp);
		//		if (fTmp > 0)
		//			ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, LPT - fTmp - 10.0);
		//	}

		//	//ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, FlyMode == eFlyMode.FlyBy ? EntryDir - ARANMath.C_PI : EntryDir, LPT, 0);
		//	//if(PrevLeg!=null)
		//	//	LPT = -PrevLeg.EndFIX.EPT - 10.0;
		//	//else

		//	//_UI.DrawPointWithText(ptTmp, "ptTmp-2");
		//	//_UI.DrawPointWithText(PrevLeg.EndFIX.PrjPt , "PrevLeg.EndFIX");
		//	//ProcessMessages();

		//	Point ptFrom = null;

		//	if (TurnDir == TurnDirection.CCW)
		//	{
		//		if (IsPrimary)
		//		{
		//			ASW_IN0C = 0.5 * _StartFIX.SemiWidth;
		//			ASW_IN0F = _StartFIX.ASW_2_L;
		//			if (PrevLeg != null && PrevLeg.PrimaryArea.Count > 0)
		//			{
		//				ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, OutDir + ARANMath.C_PI_2, out fTmp);
		//				//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, OutDir + ARANMath.C_PI_2, OutDir, out fTmp);
		//				if (ptFrom != null)
		//					ASW_IN0F = fTmp;

		//				//_UI.DrawPointWithText(ptTmp, "ptTmp");
		//				//_UI.DrawPointWithText(ptFrom, "ptFrom-I0");
		//				//ProcessMessages();

		//			}
		//		}
		//		else
		//		{
		//			ASW_IN0C = _StartFIX.SemiWidth;
		//			ASW_IN0F = _StartFIX.ASW_L;
		//			if (PrevLeg != null && PrevLeg.FullArea.Count > 0)
		//			{
		//				ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, OutDir + ARANMath.C_PI_2, out fTmp);
		//				//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, ptTmp, OutDir + ARANMath.C_PI_2, OutDir, out fTmp);
		//				if (ptFrom != null)
		//					ASW_IN0F = fTmp;
		//			}
		//		}
		//	}
		//	else
		//	{
		//		if (IsPrimary)
		//		{
		//			ASW_IN0C = 0.5 * _StartFIX.SemiWidth;
		//			ASW_IN0F = _StartFIX.ASW_2_R;
		//			if (PrevLeg != null && PrevLeg.PrimaryArea.Count > 0)
		//			{
		//				ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, OutDir - ARANMath.C_PI_2, out fTmp);
		//				//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, OutDir - ARANMath.C_PI_2, OutDir, out fTmp);
		//				if (ptFrom != null)
		//					ASW_IN0F = fTmp;
		//			}
		//		}
		//		else
		//		{
		//			ASW_IN0C = _StartFIX.SemiWidth;
		//			ASW_IN0F = _StartFIX.ASW_R;
		//			if (PrevLeg != null && PrevLeg.FullArea.Count > 0)
		//			{
		//				ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, OutDir - ARANMath.C_PI_2, out fTmp);
		//				//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, ptTmp, OutDir - ARANMath.C_PI_2, OutDir, out fTmp);
		//				if (ptFrom != null)
		//					ASW_IN0F = fTmp;
		//			}
		//		}
		//	}

		//	//	_UI.DrawPointWithText(ptFrom, 0, "ptFrom-I0");
		//	//	ProcessMessages();

		//	Ring result = new Ring();

		//	if (ptFrom == null)
		//	{
		//		ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir - ARANMath.C_PI, _StartFIX.EPT + 10.0, -ASW_IN0F * fSide);
		//		result.Add(ptTmp);
		//		//_UI.DrawPointWithText(ptTmp, -1, "ptTmp-I-" + (IsPrimary ? "P" : "S"));
		//		//ProcessMessages();

		//		ptFrom = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, OutDir - ARANMath.C_PI, _StartFIX.EPT, -ASW_IN0F * fSide);
		//		//ptFrom = (Point)ARANFunctions.LineLineIntersect(ptTmp, EntryDir, StartFIX.PrjPt, OutDir + ARANMath.C_PI_2);

		//		//_UI.DrawPointWithText(ptFrom, -1, "ptFrom-I-" + (IsPrimary ? "P" : "S"));
		//		//ProcessMessages();
		//	}

		//	result.Add(ptFrom);

		//	if (TurnAng > 0.034906585039886591)
		//	{
		//		fTmp = ASW_IN0C / Math.Cos(TurnAng);
		//		Point ptTo = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir + ARANMath.C_PI_2 * fSide, fTmp, 0);//ASW_IN0C

		//		//_UI.DrawPointWithText(ptTo, -1, "ptTo-I-" + (IsPrimary ? "P" : "S"));
		//		//ProcessMessages();

		//		double ptDist = ARANMath.Hypot(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);
		//		double ptDir = Math.Atan2(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);

		//		fTmp = ARANMath.Modulus((OutDir - ptDir) * fSide, 2 * ARANMath.C_PI);

		//		if (fTmp > ARANMath.C_PI)
		//			fTmp = fTmp - ARANMath.C_2xPI;

		//		if (ptDist > 1.0 && fTmp > -SplayAngle15 && fTmp < DivergenceAngle30)
		//			result.Add(ptTo);
		//		/*
		//		if IsPrimary then
		//		begin
		//			GUI.DrawPointWithText(ptFrom, 255, 'ptFrom');
		//			GUI.DrawPointWithText(ptTo, 255, 'ptTo');
		//			GUI.DrawPointWithText(PtTmp, 0, 'ptTmp');
		//		end;
		//		*/
		//	}

		//	JoinSegments(result, ARP, false, IsPrimary, EntryDir, TurnDir);

		//	//_UI.DrawPointWithText(result[2], 0, "result[2]");
		//	//_UI.DrawPointWithText(result[3], 0, "result[3]");
		//	//ProcessMessages(true);

		//	return result;
		//}

		override protected Ring CreateOuterTurnArea(LegBase PrevLeg, ADHPType ARP, bool IsPrimary, double EntryDir, TurnDirection TurnDir)
		{
			int i, n, t = 0;

			double R, K, dAlpha, AztEnd1, AztEnd2, SpAngle, fTmp, SpStartDir, SpStartRad,
				SpTurnAng, SpFromAngle, SpToAngle, dPhi1, SplayAngle, CurWidth, CurDist,
				Dist0, MaxDist, TransitionDist, Dist3700, Dist56000, LPTYDist, fDistTreshold,
				ptInterDist, dRad, SpAbeamDist, SplayAngle15, PrevX, PrevY, BulgeAngle,//CurrY,
				BaseDir, Delta, DivergenceAngle30, ASW_OUT0C, ASW_OUT0F, ASW_OUTMax, ASW_OUT1;

			Point OuterBasePoint, InnerBasePoint, ptTmp, ptInter, ptBase, ptFrom,
				ptCut, ptCnt, ptCurr, ptCurr1;

			bool bFlag, IsMAPt, HaveSecondSP;

			eFlyMode FlyMode = _StartFIX.FlyMode;
			//double EntryDir = _StartFIX.EntryDirection;
			double OutDir = _StartFIX.OutDirection;
			double SpiralDivergenceAngle = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;

			if (IsPrimary)
			{
				//fTmp = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;
				//DivergenceAngle30 = Math.Atan(0.5 * Math.Tan(SpiralDivergenceAngle));

				//fTmp = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
				//SplayAngle15 = Math.Atan(0.5 * Math.Tan(fTmp));

				DivergenceAngle30 = SpiralDivergenceAngle;  //GlobalVars.constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;
				SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
			}
			else
			{
				DivergenceAngle30 = SpiralDivergenceAngle;  //GlobalVars.constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;
				SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
			}

			//TurnDirection TurnDir = ARANMath.SideToTurn(ARANMath.SideDef(_StartFIX.PrjPt, EntryDir, _EndFIX.PrjPt));
			double fSide = (int)TurnDir;
			double TurnAng = ARANMath.Modulus((OutDir - EntryDir) * fSide, ARANMath.C_2xPI);

			if (TurnAng < ARANMath.EpsilonRadian || Math.Abs(TurnAng - ARANMath.C_2xPI) < ARANMath.EpsilonRadian)
				TurnAng = 0.0;

			double TurnR = _StartFIX.ConstructTurnRadius;   // ARANMath.BankToRadius(_StartFIX.BankAngle, _StartFIX.ConstructTAS);

			double Rv = 1765.27777777777777777 * Math.Tan(_StartFIX.BankAngle) / (ARANMath.C_PI * _StartFIX.ConstructTAS);
			if (Rv > 3.0)
				Rv = 3.0;

			double WSpeed = _constants.Pansops[ePANSOPSData.dpWind_Speed].Value;
			double coef = WSpeed / ARANMath.DegToRad(Rv);

			//MAX DISTANCE + =================================================================

			LPTYDist = (_EndFIX.FlyMode == eFlyMode.Flyby ? _EndFIX.LPT : -_EndFIX.LPT) - OverlapDist;

			WayPoint[] wptTransitions = new WayPoint[3];

			int transitions = 0;

			WayPoint prev = _StartFIX;
			Point ptTransition = ARANFunctions.CircleVectorIntersect(ARP.pPtPrj, PANSOPSConstantList.PBNTerminalTriggerDistance, _EndFIX.PrjPt, OutDir + Math.PI);

			//_UI.DrawPointWithText(ptTransition, -1, "ptTransition - 1");
			//ProcessMessages();
			if (!ptTransition.IsEmpty)
			{
				Point ptS = ARANFunctions.PrjToLocal(ptTransition, OutDir, _StartFIX.PrjPt);
				Point ptE = ARANFunctions.PrjToLocal(ptTransition, OutDir, _EndFIX.PrjPt);

				if (ptS.X * ptE.X < 0.0)
				{
					WayPoint WPT_30NM = new WayPoint(eFIXRole.TP_, _aranEnv);
					WPT_30NM.Assign(_StartFIX);
					WPT_30NM.PrjPt = ptTransition;

					WPT_30NM.FlightPhase = eFlightPhase.STAR;
					if (WPT_30NM.PBNType == ePBNClass.RNAV5)
						WPT_30NM.PBNType = ePBNClass.RNAV1;
					WPT_30NM.EntryDirection = OutDir;
					WPT_30NM.OutDirection = OutDir;
					WPT_30NM.BankAngle = _StartFIX.BankAngle;

					wptTransitions[transitions] = WPT_30NM;
					transitions++;

					prev = WPT_30NM;

					double bank = _EndFIX.BankAngle;
					_EndFIX.FlightPhase = eFlightPhase.STAR;
					if (_EndFIX.FlightPhase == eFlightPhase.STAR && _EndFIX.PBNType == ePBNClass.RNAV5)
						_EndFIX.PBNType = ePBNClass.RNAV1;

					_EndFIX.BankAngle = bank;
				}
			}

			ptTransition = ARANFunctions.CircleVectorIntersect(ARP.pPtPrj, PANSOPSConstantList.PBNTerminalTriggerDistance, _EndFIX.PrjPt, OutDir);
			if (!ptTransition.IsEmpty)
			{
				Point ptS = ARANFunctions.PrjToLocal(ptTransition, OutDir, prev.PrjPt);
				Point ptE = ARANFunctions.PrjToLocal(ptTransition, OutDir, _EndFIX.PrjPt);

				if (ptS.X * ptE.X < 0.0)
				{
					WayPoint WPT_30NM = new WayPoint(eFIXRole.TP_, _aranEnv);
					WPT_30NM.Assign(prev);
					WPT_30NM.PrjPt = ptTransition;

					WPT_30NM.FlightPhase = eFlightPhase.STARGE56;
					WPT_30NM.BankAngle = prev.BankAngle;
					WPT_30NM.EntryDirection = OutDir;
					WPT_30NM.OutDirection = OutDir;

					wptTransitions[transitions] = WPT_30NM;
					transitions++;

					double bank = _EndFIX.BankAngle;
					_EndFIX.FlightPhase = eFlightPhase.STARGE56;
					_EndFIX.BankAngle = bank;
				}
			}

			wptTransitions[transitions++] = (WayPoint)_EndFIX.Clone();

			Dist56000 = LPTYDist;
			TransitionDist = LPTYDist;
			fDistTreshold = LPTYDist;

			if (transitions > 1)
			{
				fDistTreshold = ARANMath.Hypot(ARP.pPtPrj.X - wptTransitions[0].PrjPt.X, ARP.pPtPrj.Y - wptTransitions[0].PrjPt.Y);
				t |= 1;

				ptTmp = ARANFunctions.PrjToLocal(wptTransitions[0].PrjPt, OutDir - ARANMath.C_PI, ARP.pPtPrj);
				Dist56000 = ptTmp.X - Math.Sqrt(ARANMath.Sqr(fDistTreshold) - ARANMath.Sqr(ptTmp.Y));

				TransitionDist = Dist56000;
			}

			if (_StartFIX.Role == eFIXRole.IF_ && _EndFIX.Role == eFIXRole.FAF_)
			{
				Dist3700 = 1.5 * (_StartFIX.XTT - _EndFIX.XTT) / Math.Tan(_constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value);
				//		Dist3700 := (StartFIX.SemiWidth - EndFIX.SemiWidth)/Tan(GPANSOPSConstants.Constant[arSecAreaCutAngl].Value);
				//GPANSOPSConstants.Constant[rnvImMinDist].Value;
				TransitionDist = Math.Max(TransitionDist, Dist3700);
			}

			MaxDist = Math.Max(LPTYDist, TransitionDist);
			//MAX DISTANCE - =================================================================

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

			//ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, FlyMode == eFlyMode.FlyBy ? EntryDir - ARANMath.C_PI : EntryDir, LPT, 0);
			ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, LPT - 1.0);

			//_UI.DrawPointWithText(_StartFIX.BasePoints[0], 255, "BasePoints[0]");
			//_UI.DrawPointWithText(ptTmp, -1, "LPT");
			//ProcessMessages();

			double dirN0 = ARANFunctions.ReturnAngleInRadians(_StartFIX.BasePoints[_StartFIX.BasePoints.Count - 1], _StartFIX.BasePoints[0]);
			double dir01 = ARANFunctions.ReturnAngleInRadians(_StartFIX.BasePoints[0], _StartFIX.BasePoints[1]);
			double CurrDir = ARANMath.SubtractAngles(dir01, dirN0);													//dir01- dirN0;//

			//_UI.DrawPointWithText(ptTmp, 255, "LPT");
			//	_UI.DrawPointWithText(_StartFIX.PrjPt, 255, "FIX");
			//	LineString lstr = new LineString();
			//	lstr.Add(ARANFunctions.LocalToPrj(ptTmp, EntryDir, 0, -10000));
			//	lstr.Add(ptTmp);
			//	lstr.Add(ARANFunctions.LocalToPrj(ptTmp, EntryDir, 0, 10000));
			//	_UI.DrawLineString(lstr, 255, 2);
			//ProcessMessages();

			ptFrom = null;

			//for (int gh = 0; gh < _StartFIX.BasePoints.Count; gh++)
			//    _UI.DrawPointWithText(_StartFIX.BasePoints[gh], 0, "pt-" + gh);

			//_UI.DrawRing(PrevLeg.PrimaryArea[0].ExteriorRing, 233,eFillStyle.sfsCross);
			//ProcessMessages();

			//List<Point> BasePoints = new List<Point>();

			if (TurnDir == TurnDirection.CCW)
			{
				if (IsPrimary)
				{
					ASW_OUT0F = _StartFIX.ASW_2_R;

					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDir - 0.5 * ARANMath.C_PI, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, StartFIX.PrjPt, OutDir - 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						//_UI.DrawPointWithText(ptFrom, 0, "ptFrom");
						//ProcessMessages();

						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}

					ASW_OUT0C = 0.5 * _StartFIX.SemiWidth;
					ASW_OUT1 = 0.5 * wptTransitions[0].SemiWidth;
					dRad = 0;
					InnerBasePoint = _StartFIX.BasePoints[1];
					OuterBasePoint = _StartFIX.BasePoints[0];
				}
				else
				{
					ASW_OUT0F = _StartFIX.ASW_R;
					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir - 0.5 * ARANMath.C_PI, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, StartFIX.PrjPt, OutDir - 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						//	_UI.DrawPointWithText(ptFrom, 0, "ptFrom");
						//	ProcessMessages();

						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}

					ASW_OUT0C = _StartFIX.SemiWidth;
					ASW_OUT1 = wptTransitions[0].SemiWidth;
					dRad = ASW_OUT0F - _StartFIX.ASW_2_R;

					InnerBasePoint = ARANFunctions.LocalToPrj(_StartFIX.BasePoints[1], dir01, _StartFIX.ASW_2_R);
					OuterBasePoint = ARANFunctions.LocalToPrj(_StartFIX.BasePoints[0], dir01, -_StartFIX.ASW_2_L);
				}

				//InnerBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir + 0.5 * ARANMath.C_PI, _StartFIX.ASW_2_L, 0);
				//OuterBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir - 0.5 * ARANMath.C_PI, StartFIX.ASW_2_R, 0);
			}
			else
			{
				if (IsPrimary)
				{
					ASW_OUT0F = _StartFIX.ASW_2_L;
					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDir + 0.5 * ARANMath.C_PI, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, StartFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						//	_UI.DrawPointWithText(ptTmp, 255, "LPT");
						//_UI.DrawPointWithText(ptFrom, 0, "ptFrom");
						//_UI.DrawRing(PrevLeg.PrimaryArea[0].ExteriorRing, 167, AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross);
						//ProcessMessages();

						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}

					ASW_OUT0C = 0.5 * _StartFIX.SemiWidth;
					ASW_OUT1 = 0.5 * wptTransitions[0].SemiWidth;
					dRad = 0;
					InnerBasePoint = _StartFIX.BasePoints[1];
					OuterBasePoint = _StartFIX.BasePoints[0];
				}
				else
				{
					ASW_OUT0F = _StartFIX.ASW_L;
					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir + 0.5 * ARANMath.C_PI, out fTmp);

						//_UI.DrawPointWithText(ptTmp, 255, "LPT");
						//_UI.DrawPointWithText(ptFrom, 0, "ptFrom");
						//_UI.DrawRing(PrevLeg.FullArea[0].ExteriorRing, -1, AranEnvironment.Symbols.eFillStyle.sfsCross);
						//ProcessMessages();
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, StartFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}

					ASW_OUT0C = _StartFIX.SemiWidth;
					ASW_OUT1 = wptTransitions[0].SemiWidth;
					dRad = ASW_OUT0F - _StartFIX.ASW_2_L;

					InnerBasePoint = ARANFunctions.LocalToPrj(_StartFIX.BasePoints[1], dir01, _StartFIX.ASW_2_L);
					OuterBasePoint = ARANFunctions.LocalToPrj(_StartFIX.BasePoints[0], dir01, -_StartFIX.ASW_2_R);
				}
				//InnerBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir - 0.5 * ARANMath.C_PI, _StartFIX.ASW_2_R, 0);
				//OuterBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir + 0.5 * ARANMath.C_PI, StartFIX.ASW_2_L, 0);
			}

			//if (StartFIX.FlyMode == eFlyMode.AtHeight)
			//{
			//    ASW_OUT0C = ASW_OUT0F;

			//    if (IsPrimary)
			//        InnerBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir + 0.5 * fSide * ARANMath.C_PI, ASW_OUT0F, 0);
			//    else
			//        InnerBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir + 0.5 * fSide * ARANMath.C_PI, 0.5 * ASW_OUT0F, 0);
			//}

			//_UI.DrawPointWithText(InnerBasePoint, 255, "InnerBasePoint");
			//_UI.DrawPointWithText(OuterBasePoint, 255, "OuterBasePoint");
			//	_UI.DrawPointWithText(ptFrom, 0, "ptFrom");
			//	_UI.DrawPointWithText(ptTmp, 255, "ptTmp");
			//ProcessMessages();

			//for (i = 0; i < _StartFIX.BasePoints.Count; i++)
			//	_UI.DrawPointWithText(_StartFIX.BasePoints[i], -1, "BasePt [ " + i + " ]");
			//ProcessMessages();

			TurnR += dRad;

			//if (t == 0)
			//	ASW_OUTMax = Math.Max(ASW_OUT0C, ASW_OUT1);
			//else
			ASW_OUTMax = ASW_OUT0C;

			Ring result = new Ring();

			if (ptFrom == null)
			{
				ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir - ARANMath.C_PI, _StartFIX.EPT + 10.0, ASW_OUT0F * fSide);
				//_UI.DrawPointWithText(ptTmp, -1, "ptTmp1");
				//ProcessMessages();

				result.Add(ptTmp);

				ptFrom = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir - 0.5 * ARANMath.C_PI * fSide, ASW_OUT0F, 0);
				//_UI.DrawPointWithText(ptFrom, -1, "ptFrom-f");
				//ProcessMessages();
			}
			else
				OuterBasePoint = (Point)ptFrom;//.Clone();

			result.Add(ptFrom);
			//_UI.DrawPointWithText(OuterBasePoint, 0, "OuterBasePoint");
			//_UI.DrawPointWithText(InnerBasePoint, 0, "InnerBasePoint");
			//_UI.DrawPointWithText(ptFrom, 0, "ptFrom");
			//ProcessMessages();
			//	_StartFIX.CalcExtraTurnRangePoints();

			ptCurr = null;
			//=============================================================================
			IsMAPt = (_StartFIX.Role == eFIXRole.FAF_ && _EndFIX.Role == eFIXRole.MAPt_);

			//ptCnt = ARANFunctions.LocalToPrj(OuterBasePoint, EntryDir + 0.5 * ARANMath.C_PI * fSide, TurnR - dRad, 0);
			ptCnt = ARANFunctions.LocalToPrj(OuterBasePoint, EntryDir + 0.5 * ARANMath.C_PI * fSide, TurnR, 0);

			//	_UI.DrawPointWithText(ptCnt, 0, "ptCnt-0");
			//	ProcessMessages(true);

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
 			//??????? //SpAbeamDist = -fSide * ARANFunctions.PointToLineDistance(ptTmp, _StartFIX.PrjPt, OutDir);

			HaveSecondSP = (TurnAng >= CurrDir + SplayAngle15) || ((TurnAng >= CurrDir - DivergenceAngle30) && (SpAbeamDist > ASW_OUT0C));
			SpFromAngle = 0;

			//for (int gh = 0; gh < _StartFIX.BasePoints.Count; gh++)
			//	_UI.DrawPointWithText(_StartFIX.BasePoints[gh], 0, "pt-" + gh);
			//ProcessMessages();

			//if (TurnAng > 0.5 * ARANMath.C_PI)
			//    AztEnd1 = EntryDir + 0.5 * ARANMath.C_PI * fSide;
			//else
			//    AztEnd1 = OutDir;

			if (TurnAng > CurrDir)
				AztEnd1 = EntryDir + CurrDir * fSide;
			else
				AztEnd1 = OutDir;

			if (HaveSecondSP)
				AztEnd2 = EntryDir + CurrDir * fSide;//0.5 * ARANMath.C_PI 
			else
				AztEnd2 = AztEnd1;

			SpToAngle = ARANFunctions.SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir - BulgeAngle, AztEnd1, TurnDir);
			//	SpToAngle = ARANFunctions.SpiralTouchAngleOld(TurnR, coef, EntryDir, AztEnd1, TurnDir);

			if (FlyMode == eFlyMode.Flyby)
				SpTurnAng = ARANFunctions.SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir - BulgeAngle, EntryDir, TurnDir);
			//		SpTurnAng = SpiralTouchAngleOld(TurnR, coef, EntryDir, EntryDir, TurnDir)
			else
				SpTurnAng = SpToAngle;

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
			//_UI.DrawRing(rr, -1, AranEnvironment.Symbols.eFillStyle.sfsHorizontal);
			//ProcessMessages();

			//_UI.DrawPointWithText(ptCurr, ARANFunctions.RGB(0, 128, 0), (i + 1).ToString());
			//ProcessMessages();

			if (FlyMode == eFlyMode.Flyby)
			{
				Dist0 = TurnR + SpToAngle * coef;

				ptCurr1 = ptCurr;
				ptCurr = ARANFunctions.LocalToPrj(ptCnt, EntryDir + (SpToAngle - 0.5 * ARANMath.C_PI) * fSide, Dist0, 0);
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
			//lst.Add(ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir - 0.5 * ARANMath.C_PI * fSide, ASW_OUTMax, 0));
			//_UI.DrawLineString(lst, 0, 2);
			//ProcessMessages();

			ptBase = ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir - 0.5 * fSide * ARANMath.C_PI, ASW_OUTMax, 0);

			//_UI.DrawPointWithText(ptBase, ARANFunctions.RGB(0, 128, 0), "ptBase");
			//ProcessMessages();

			BaseDir = OutDir;

			SpStartDir += SpToAngle * fSide;
			SpFromAngle = SpToAngle;

			CurDist = ARANFunctions.PointToLineDistance(ptCurr, wptTransitions[0].PrjPt, OutDir + 0.5 * ARANMath.C_PI);
			CurWidth = -fSide * ARANFunctions.PointToLineDistance(ptCurr, _EndFIX.PrjPt, OutDir);

			if (IsMAPt)
			{
				Dist0 = ARANMath.Hypot(wptTransitions[0].PrjPt.X - _StartFIX.PrjPt.X, wptTransitions[0].PrjPt.Y - _StartFIX.PrjPt.Y);
				dPhi1 = Math.Atan2(ASW_OUT0C - ASW_OUT1, Dist0);
				if (dPhi1 > DivergenceAngle30)
					dPhi1 = DivergenceAngle30;

				SpAngle = OutDir + dPhi1 * fSide;

				ptBase = ARANFunctions.LocalToPrj(_EndFIX.PrjPt, OutDir - 0.5 * ARANMath.C_PI * fSide, ASW_OUT0C, 0);

				if (CurWidth < ASW_OUTMax)
					SplayAngle = OutDir - SplayAngle15 * fSide;
				else
					SplayAngle = OutDir + DivergenceAngle30 * fSide;

				ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, SplayAngle, ptBase, SpAngle);
				ptInterDist = ARANFunctions.PointToLineDistance(ptInter, wptTransitions[0].PrjPt, OutDir + 0.5 * ARANMath.C_PI);

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
					if (ARANMath.Modulus(AztEnd2 - AztEnd1, ARANMath.C_2xPI) > ARANMath.EpsilonRadian)
					//	if (fTmp > EpsilonRadian) and (fTmp < PI) then
					{
						SpToAngle = ARANFunctions.SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir - BulgeAngle, AztEnd2, TurnDir);
						//	SpToAngle = SpiralTouchAngleOld(TurnR, coef, EntryDir, AztEnd2, TurnDir);

						SpTurnAng = SpToAngle - SpFromAngle;
						if (SpTurnAng >= ARANMath.EpsilonRadian)
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

									if (K > 0)
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
					ptCnt = ARANFunctions.LocalToPrj(InnerBasePoint, EntryDir + 0.5 * ARANMath.C_PI * fSide, TurnR - dRad, 0);

					//	_UI.DrawPointWithText(ptCnt, 255, "ptCnt-2");
					//	_UI.DrawPointWithText(InnerBasePoint, 255, "InnerBasePoint");
					//	ProcessMessages();

					R = TurnR + SpFromAngle * coef;
					//ptCurr1 = ARANFunctions.LocalToPrj(ptCnt, SpStartDir + SpFromAngle * fSide, R, 0);
					ptCurr1 = ARANFunctions.LocalToPrj(ptCnt, SpStartDir, R, 0);

					//	_UI.DrawPointWithText(ptCurr1, 0, "ptSp-2");
					//	_UI.DrawPointWithText(ptBase, 0, "ptBase");
					//	ProcessMessages(true);
					//_UI.DrawPointWithText(ptCurr, 255, "ptCurr-2");
					//ProcessMessages();

					Delta = -fSide * ARANFunctions.PointToLineDistance(ptCurr1, ptBase, BaseDir);   //

					//fTmp = ARANMath.Modulus(EntryDir + fSide * CurrDir - BaseDir, 2 * ARANMath.C_PI);
					fTmp = ARANMath.Modulus(dir01 - BaseDir, 2 * ARANMath.C_PI);

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

						fTmp = ARANFunctions.PointToLineDistance(ptInter, wptTransitions[0].PrjPt, OutDir + 0.5 * ARANMath.C_PI);
						Dist0 = ARANFunctions.PointToLineDistance(ptInter, ptCurr, OutDir + 0.5 * ARANMath.C_PI);

						if (fTmp > 0 && Dist0 < 0)
						{
							ptCurr = ptInter;
							result.Add(ptCurr);
						}

						/*
						if( fTmp < 0 )
							ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, EntryDir + 0.5 * ARANMath.C_PI, wptTransitions[0].PrjPt, OutDir + 0.5 * ARANMath.C_PI);

						fTmp = PointToLineDistance(ptInter, ptCurr, OutDir + 0.5 * PI);
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


			CurWidth = -fSide * ARANFunctions.PointToLineDistance(ptCurr, _EndFIX.PrjPt, OutDir);
			CurDist = ARANFunctions.PointToLineDistance(ptCurr, wptTransitions[0].PrjPt, OutDir + 0.5 * ARANMath.C_PI);

			//if (IsMAPt)
			//{
			//Dist0 = Hypot(wptTransitions[0].PrjPt.X - StartFIX.PrjPt.X, wptTransitions[0].PrjPt.Y - StartFIX.PrjPt.Y);
			//dPhi1 = ArcTan2(ASW_OUT0C - ASW_OUT1, Dist0);
			//if dPhi1 > DivergenceAngle30 then dPhi1 := DivergenceAngle30;

			//		SpAngle := OutDir - dPhi1 * fSide;
			//		LocalToPrj(StartFIX.PrjPt, OutDir + 0.5 * PI * fSide, ASW_OUT0C, 0, ptBase);

			//		if (CurWidth < ASW_OUTMax) then
			//			SplayAngle := OutDir + SplayAngle15 * fSide
			//		else
			//			SplayAngle := OutDir - DivergenceAngle30 * fSide;

			//		ptInter := LineLineIntersect(ptCurr, SplayAngle, InnerBasePoint, SpAngle).AsPoint;
			//		ptInterDist := PointToLineDistance(ptInter, wptTransitions[0].PrjPt, OutDir + 0.5 * PI);

			//		TransitionDist := Max(TransitionDist, ptInterDist);
			//		if TransitionDist = ptInterDist then
			//		begin
			//			IsMAPt := True;
			//			BaseDir := SpAngle;
			//			ASW_OUTMax := fSide * PointToLineDistance(ptInter, wptTransitions[0].PrjPt, OutDir);
			//		end;
			//}

			//=============================================================================

			int tranI = 0;
			if (TransitionDist > CurDist)
			{
				ASW_OUTMax = ASW_OUT1;
				ptBase = ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir - 0.5 * fSide * ARANMath.C_PI, ASW_OUTMax, 0);    //???

				if (transitions > 1)
					tranI = 1;

				CurDist = ARANFunctions.PointToLineDistance(ptCurr, wptTransitions[tranI].PrjPt, OutDir + 0.5 * ARANMath.C_PI);
			}

			//if (CurDist - LPTYDist > ARANMath.EpsilonDistance && Math.Abs(CurWidth - ASW_OUTMax) > ARANMath.EpsilonDistance)
			if (CurDist - LPTYDist > ARANMath.EpsilonDistance && (HaveSecondSP || (Math.Abs(CurWidth - ASW_OUTMax) > ARANMath.EpsilonDistance)))
			{
				if (CurWidth - ASW_OUTMax > ARANMath.EpsilonDistance)
				{
					SpToAngle = ARANFunctions.SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir - BulgeAngle, OutDir + SpiralDivergenceAngle * fSide, TurnDir);
					//SpToAngle = ARANFunctions.SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir - BulgeAngle, OutDir - DivergenceAngle30 * fSide, TurnDir);
					SpTurnAng = SpToAngle - SpFromAngle;

					if (SpTurnAng >= ARANMath.EpsilonRadian)
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
								if (ptTmp.Y * fSide <= 0.0)// && ptTmp.X <= 0.0)
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

								if (ptTmp.Y * fSide >= 0.0)// || ptTmp.X >= 0.0)
								{
									if (ptTmp.X < 0) K = -PrevY / (ptTmp.Y - PrevY);
									else K = -PrevX / (ptTmp.X - PrevX);

									ptCurr.X = result[result.Count - 1].X + K * (ptCurr.X - result[result.Count - 1].X);
									ptCurr.Y = result[result.Count - 1].Y + K * (ptCurr.Y - result[result.Count - 1].Y);

									if (K > 0)
									result.Add(ptCurr);
									break;
								}
							}

							//if (i == n && IsPrimary)	_EndFIX.JointFlags = _EndFIX.JointFlags | 1;

							PrevX = ptTmp.X;
							PrevY = ptTmp.Y;
							result.Add(ptCurr);
						}

						//Ring rr = new Ring();
						//rr.Assign(result);
						//_UI.DrawRing(rr, -1, eFillStyle.sfsHorizontal);
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

					ptInterDist = ARANFunctions.PointToLineDistance(ptInter, wptTransitions[tranI].PrjPt, OutDir + 0.5 * ARANMath.C_PI);

					//_UI.DrawPointWithText(ptInter, 0, "ptInter - 2");
					//ProcessMessages(true);

					if (ptInterDist < TransitionDist)
					{
						ptCut = ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir + ARANMath.C_PI, TransitionDist, 0);
						ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, SplayAngle, ptCut, OutDir + 0.5 * ARANMath.C_PI);
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
			//_UI.DrawRing(rr, -1, eFillStyle.sfsHorizontal);
			//ProcessMessages();

			JoinSegments(result, ARP, true, IsPrimary, EntryDir, TurnDir);
			//	_UI.DrawRing(result, ARANFunctions.RGB(0, 0, 255), eFillStyle.sfsDiagonalCross);
			//	ProcessMessages();

			return result;
		}

		override protected Ring CreateInnerTurnArea(LegBase PrevLeg, ADHPType ARP, bool IsPrimary, double EntryDir, TurnDirection TurnDir)
		{
			double fDistTreshold, OutDir, dPhi1, TurnAng,
				Dist56000, Dist3700, CurDist, MaxDist, TransitionDist, CurWidth, ptInterDist, LPTYDist,
				fTmp, DivergenceAngle30, SpiralDivergenceAngle, SplayAngle15, BaseDir, LegLenght,
				SplayAngle, fSide, ASW_INMax, ASW_IN0C, ASW_IN0F, ASW_IN1;  //, Dist0, Dist1, dPhi2, azt0, Dist2;

			Point InnerBasePoint, ptTmp, ptBase, ptCurr, ptCut, ptInter, ptFrom = null;

			//bool ReCalcASW_IN0C;

			SpiralDivergenceAngle = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;

			if (IsPrimary)
			{
				//fTmp = GlobalVars.constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;
				DivergenceAngle30 = Math.Atan(0.5 * Math.Tan(SpiralDivergenceAngle));

				fTmp = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
				SplayAngle15 = Math.Atan(0.5 * Math.Tan(fTmp));
			}
			else
			{
				DivergenceAngle30 = SpiralDivergenceAngle;//GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
				SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
			}

			OutDir = _StartFIX.OutDirection;
			fSide = (int)TurnDir;

			TurnAng = ARANMath.Modulus((OutDir - EntryDir) * fSide, ARANMath.C_2xPI);

			if (TurnAng < ARANMath.EpsilonRadian || Math.Abs(TurnAng - ARANMath.C_2xPI) < ARANMath.EpsilonRadian)
				TurnAng = 0.0;

			//MAX DISTANCE + =================================================================

			LPTYDist = (_EndFIX.FlyMode == eFlyMode.Flyby ? _EndFIX.LPT : -_EndFIX.LPT) - OverlapDist;

			WayPoint[] wptTransitions = new WayPoint[3];

			int transitions = 0;

			WayPoint prev = _StartFIX;
			Point ptTransition = ARANFunctions.CircleVectorIntersect(ARP.pPtPrj, PANSOPSConstantList.PBNTerminalTriggerDistance, _EndFIX.PrjPt, OutDir + Math.PI);

			//_UI.DrawPointWithText(ptTransition, -1, "ptTransition - 1");
			//ProcessMessages();
			if (!ptTransition.IsEmpty)
			{
				Point ptS = ARANFunctions.PrjToLocal(ptTransition, OutDir, _StartFIX.PrjPt);
				Point ptE = ARANFunctions.PrjToLocal(ptTransition, OutDir, _EndFIX.PrjPt);

				if (ptS.X * ptE.X < 0.0)
				{
					WayPoint WPT_30NM = new WayPoint(eFIXRole.TP_, _aranEnv);
					WPT_30NM.Assign(_StartFIX);
					WPT_30NM.PrjPt = ptTransition;

					WPT_30NM.FlightPhase = eFlightPhase.STAR;
					if (WPT_30NM.PBNType == ePBNClass.RNAV5)
						WPT_30NM.PBNType = ePBNClass.RNAV1;
					WPT_30NM.EntryDirection = OutDir;
					WPT_30NM.OutDirection = OutDir;
					WPT_30NM.BankAngle = _StartFIX.BankAngle;

					wptTransitions[transitions] = WPT_30NM;
					transitions++;

					prev = WPT_30NM;

					double bank = _EndFIX.BankAngle;
					_EndFIX.FlightPhase = eFlightPhase.STAR;
					if (_EndFIX.FlightPhase == eFlightPhase.STAR && _EndFIX.PBNType == ePBNClass.RNAV5)
						_EndFIX.PBNType = ePBNClass.RNAV1;

					_EndFIX.BankAngle = bank;
				}
			}

			ptTransition = ARANFunctions.CircleVectorIntersect(ARP.pPtPrj, PANSOPSConstantList.PBNTerminalTriggerDistance, _EndFIX.PrjPt, OutDir);
			if (!ptTransition.IsEmpty)
			{
				Point ptS = ARANFunctions.PrjToLocal(ptTransition, OutDir, prev.PrjPt);
				Point ptE = ARANFunctions.PrjToLocal(ptTransition, OutDir, _EndFIX.PrjPt);

				if (ptS.X * ptE.X < 0.0)
				{
					WayPoint WPT_30NM = new WayPoint(eFIXRole.TP_, _aranEnv);
					WPT_30NM.Assign(prev);
					WPT_30NM.PrjPt = ptTransition;

					WPT_30NM.FlightPhase = eFlightPhase.STARGE56;
					WPT_30NM.BankAngle = prev.BankAngle;
					WPT_30NM.EntryDirection = OutDir;
					WPT_30NM.OutDirection = OutDir;

					wptTransitions[transitions] = WPT_30NM;
					transitions++;

					double bank = _EndFIX.BankAngle;
					_EndFIX.FlightPhase = eFlightPhase.STARGE56;
					_EndFIX.BankAngle = bank;
				}
			}

			wptTransitions[transitions++] = (WayPoint)_EndFIX.Clone();

			int t = 0;
			Dist56000 = LPTYDist;
			TransitionDist = LPTYDist;
			fDistTreshold = LPTYDist;
			if (transitions > 1)
			{
				fDistTreshold = ARANMath.Hypot(ARP.pPtPrj.X - wptTransitions[0].PrjPt.X, ARP.pPtPrj.Y - wptTransitions[0].PrjPt.Y);
				t |= 1;

				ptTmp = ARANFunctions.PrjToLocal(wptTransitions[0].PrjPt, OutDir - ARANMath.C_PI, ARP.pPtPrj);
				Dist56000 = ptTmp.X - Math.Sqrt(ARANMath.Sqr(fDistTreshold) - ARANMath.Sqr(ptTmp.Y));

				TransitionDist = Dist56000;
			}

			if (_StartFIX.Role == eFIXRole.IF_ && _EndFIX.Role == eFIXRole.FAF_)
			{
				Dist3700 = 1.5 * (_StartFIX.XTT - _EndFIX.XTT) / Math.Tan(_constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value);

				//		Dist3700 := (StartFIX.SemiWidth - EndFIX.SemiWidth)/Tan(GPANSOPSConstants.Constant[arSecAreaCutAngl].Value);
				//GPANSOPSConstants.Constant[rnvImMinDist].Value;
				TransitionDist = Math.Max(TransitionDist, Dist3700);
			}

			MaxDist = Math.Max(LPTYDist, TransitionDist);
			//MAX DISTANCE - =================================================================
			double EPT;
			if (_StartFIX.IsDFTarget)
			{
				if (TurnDir == TurnDirection.CW)
					EPT = _StartFIX.EPT_R;
				else
					EPT = _StartFIX.EPT_L;
			}
			else
				EPT = _StartFIX.EPT;

			ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir - ARANMath.C_PI, EPT, 0);
			//	_UI.DrawPointWithText(ptTmp, 0, "ptTmpI");
			//	ProcessMessages();

			if (TurnDir == TurnDirection.CCW)
			{
				if (IsPrimary)
				{
					ASW_IN0F = _StartFIX.ASW_2_L;
					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDir + 0.5 * ARANMath.C_PI, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, StartFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI, OutDir, out fTmp);
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
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir + 0.5 * ARANMath.C_PI, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, StartFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_IN0F = fTmp;
					}

					ASW_IN0C = _StartFIX.SemiWidth;
					ASW_IN1 = wptTransitions[0].SemiWidth;
				}
				InnerBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir + 0.5 * ARANMath.C_PI, ASW_IN0F, 0);
			}
			else
			{
				if (IsPrimary)
				{
					ASW_IN0F = _StartFIX.ASW_2_R;
					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.PrimaryArea[0].ExteriorRing, ptTmp, EntryDir - 0.5 * ARANMath.C_PI, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, StartFIX.PrjPt, OutDir - 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_IN0F = fTmp;
					}

					ASW_IN0C = 0.5 * _StartFIX.SemiWidth;
					ASW_IN1 = 0.5 * wptTransitions[0].SemiWidth;
				}
				else
				{
					ASW_IN0F = _StartFIX.ASW_R;
					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir - 0.5 * ARANMath.C_PI, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, StartFIX.PrjPt, OutDir - 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_IN0F = fTmp;
					}

					ASW_IN0C = _StartFIX.SemiWidth;
					ASW_IN1 = wptTransitions[0].SemiWidth;
				}
				InnerBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir - 0.5 * ARANMath.C_PI, ASW_IN0F, 0);
			}

			//if (StartFIX.FlyMode == eFlyMode.AtHeight)
			//    ASW_IN0C = ASW_IN0F;

			ASW_INMax = Math.Max(ASW_IN0C, ASW_IN1);
			Ring result = new Ring();

			ptCurr = InnerBasePoint;
			//if GlDRAWFLG then
			//	_UI.DrawPointWithText(ptCurr, 0, "ptCurr-0");
			//	_UI.DrawPointWithText(ptTmp, 0, "ptCurr-00");
			//	ProcessMessages();

			result.Add(ptTmp);
			result.Add(ptCurr);

			//_UI.DrawPointWithText(ptFrom, 0, "ptCurr-0");
			//ProcessMessages(true);

			//	_UI.DrawPointWithText(ptFrom, 0, "ptFrom-I1");
			//	ProcessMessages(true);
			//result.Add(ptCurr);

			CurDist = ARANFunctions.PointToLineDistance(ptCurr, wptTransitions[0].PrjPt, OutDir + 0.5 * ARANMath.C_PI);

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
					ptBase = ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir + 0.5 * ARANMath.C_PI * fSide, ASW_IN1, 0);
				}
				else
				{
					BaseDir = OutDir;
					ptBase = ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir + 0.5 * ARANMath.C_PI * fSide, ASW_IN0C, 0);
				}

				//	_UI.DrawPointWithText(ptBase, 0, "ptBase");
				//	ProcessMessages(true);


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

					//	_UI.DrawPointWithText(ptInter, 0, "ptInter - 1");
					//	ProcessMessages();

					ptInterDist = ARANFunctions.PointToLineDistance(ptInter, wptTransitions[0].PrjPt, OutDir + 0.5 * ARANMath.C_PI);

					if (ptInterDist < TransitionDist)
					{
						ptCut = ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir + ARANMath.C_PI, TransitionDist, 0);

						ptInter = (Point)ARANFunctions.LineLineIntersect(InnerBasePoint, SplayAngle, ptCut, OutDir + 0.5 * ARANMath.C_PI);

						//	_UI.DrawPointWithText(ptInter, 0, "ptInter - 2");

					}
					result.Add(ptInter);
				}
			}

			//Ring rr = new Ring();
			//rr.Assign(result);
			//_UI.DrawRing(rr, -1, eFillStyle.sfsHorizontal);
			//ProcessMessages();

			JoinSegments(result, ARP, false, IsPrimary, EntryDir, TurnDir);
			return result;
		}
	}
}
