using System;
using Aran.AranEnvironment;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.PANDA.Constants;

namespace Aran.PANDA.Common
{

	[System.Runtime.InteropServices.ComVisible(false)]
	public class LegDep : LegBase
	{
		public LegDep(WayPoint FIX0, WayPoint FIX1, IAranEnvironment aranEnv, LegBase PrevLeg = null) :
			base(FIX0, FIX1, aranEnv, PrevLeg)
		{
		}

		override protected void JoinSegments(Ring LegRing, ADHPType ARP, bool IsOuter, bool IsPrimary, double EntryDir, TurnDirection TurnDir)
		{
			double OutDir = _StartFIX.OutDirection;
			int transitions = 0;

			WayPoint[] wptTransitions = new WayPoint[3];
			WayPoint prev = _StartFIX;
			Point ptTransition;			// = ARANFunctions.CircleVectorIntersect(ARP.pPtPrj, PANSOPSConstantList.PBNInternalTriggerDistance, _EndFIX.PrjPt, OutDir + Math.PI);

			//_UI.DrawPointWithText(ptTransition, -1, "ptTransition - 1");
			//ProcessMessages();

			if (_StartFIX.FlightPhase < eFlightPhase.SIDGE28)
			{
				ptTransition = ARANFunctions.CircleVectorIntersect(ARP.pPtPrj, PANSOPSConstantList.PBNInternalTriggerDistance, _EndFIX.PrjPt, OutDir);
				//_UI.DrawPointWithText(ptTransition, -1, "ptTransition-1a");
				//ProcessMessages();

				if (!ptTransition.IsEmpty)
				{
					Point ptS = ARANFunctions.PrjToLocal(ptTransition, OutDir, prev.PrjPt);
					Point ptE = ARANFunctions.PrjToLocal(ptTransition, OutDir, _EndFIX.PrjPt);

					if (ptS.X * ptE.X < 0.0)
					{
						WayPoint WPT_15NM = new WayPoint(eFIXRole.TP_, _aranEnv);
						WPT_15NM.Assign(prev);
						WPT_15NM.PrjPt = ptTransition;

						double dist = ARANFunctions.ReturnDistanceInMeters(prev.PrjPt, ptTransition);
						WPT_15NM.ConstructAltitude = prev.ConstructAltitude + dist * _constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpMaxPosPDG].Value;
						//if(_StartFIX.FlightPhase  == eFlightPhase.MApLT28)
						//	WPT_15NM.FlightPhase = eFlightPhase.MApGE28;
						//else
						//	WPT_15NM.FlightPhase = eFlightPhase.SIDGE28;

						WPT_15NM.FlyMode = eFlyMode.Flyby;	
						WPT_15NM.FlightPhase = eFlightPhase.SIDGE28;

						WPT_15NM.BankAngle = prev.BankAngle;
						WPT_15NM.EntryDirection = OutDir;
						//WPT_15NM.OutDirection = OutDir;

						wptTransitions[transitions] = WPT_15NM;
						transitions++;

						prev = WPT_15NM;

						double bank = _EndFIX.BankAngle;
						_EndFIX.FlightPhase = eFlightPhase.SIDGE28;
						//if (_EndFIX.FlightPhase == eFlightPhase.STAR && _EndFIX.PBNType == ePBNClass.RNAV5)							_EndFIX.PBNType = ePBNClass.RNAV1;
						_EndFIX.BankAngle = bank;
					}
				}
				//_UI.DrawPointWithText(ptTransition, -1, "ptTransition-1");
				//ProcessMessages();
			}

			if (prev.FlightPhase < eFlightPhase.SIDGE56)
			{
				ptTransition = ARANFunctions.CircleVectorIntersect(ARP.pPtPrj, PANSOPSConstantList.PBNTerminalTriggerDistance, _EndFIX.PrjPt, OutDir);
				//_UI.DrawPointWithText(ptTransition, -1, "ptTransition-2");
				//ProcessMessages();

				if (!ptTransition.IsEmpty)
				{
					Point ptS = ARANFunctions.PrjToLocal(ptTransition, OutDir, prev.PrjPt);
					Point ptE = ARANFunctions.PrjToLocal(ptTransition, OutDir, _EndFIX.PrjPt);

					if (ptS.X * ptE.X < 0.0)
					{
						WayPoint WPT_30NM = new WayPoint(eFIXRole.TP_, _aranEnv);
						WPT_30NM.Assign(prev);
						WPT_30NM.PrjPt = ptTransition;  // ARANFunctions.LocalToPrj(ptTransition, OutDir, -WPT_30NM.ATT, 0);

						double dist = ARANFunctions.ReturnDistanceInMeters(prev.PrjPt, ptTransition);
						WPT_30NM.ConstructAltitude = prev.ConstructAltitude + dist * _constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpMaxPosPDG].Value;

						WPT_30NM.FlyMode = eFlyMode.Flyby;
						WPT_30NM.FlightPhase = eFlightPhase.SIDGE56;
						WPT_30NM.BankAngle = prev.BankAngle;
						WPT_30NM.EntryDirection = OutDir;
						//WPT_30NM.OutDirection = OutDir;

						wptTransitions[transitions] = WPT_30NM;
						transitions++;

						double bank = _EndFIX.BankAngle;
						_EndFIX.FlightPhase = eFlightPhase.SIDGE56;
						_EndFIX.BankAngle = bank;
					}
				}
			}

			wptTransitions[transitions] = (WayPoint)_EndFIX.Clone();
			transitions++;

			double SpiralDivergenceAngle, SpiralSplayAngle,
				DivergenceAngle30 = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value,
				SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;

			if (IsPrimary)// && _StartFIX.TurnAngle < ARANMath.DegToRad(5))	//
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
			Point ptCurr;

			double fSide = IsOuter ? -((int)TurnDir) : ((int)TurnDir);
			bool normalWidth = (ARANMath.RadToDeg(_StartFIX.TurnAngle) <= 10 ||	(_StartFIX.FlyMode == eFlyMode.Atheight));

			WayPoint lStartFIX = new WayPoint(_aranEnv);
			WayPoint lEndFIX = new WayPoint(_aranEnv);

			lStartFIX.Assign(_StartFIX);

			for (int pass = 0; pass < transitions; pass++, lStartFIX.Assign(lEndFIX))
			{
				lEndFIX.Assign(wptTransitions[pass]);
				lEndFIX.CalcTurnRangePoints();

				//_UI.DrawPointWithText(lEndFIX.PrjPt, -1, "ptTransition-1");
				//ProcessMessages();

				bool expansAtTransition = pass != transitions - 1;
				//ProcessMessages(true);

				//=================================================================================
				double fTmp = lEndFIX.FlyMode == eFlyMode.Flyby ? lEndFIX.LPT - OverlapDist : -lEndFIX.LPT - OverlapDist;
				if (lEndFIX.FlyMode != eFlyMode.Flyby)
					fTmp = -lEndFIX.LPT - OverlapDist;

				Point ptEndOfLeg = ARANFunctions.LocalToPrj(lEndFIX.PrjPt, OutDir + ARANMath.C_PI, fTmp, 0);
				////LegRing.Remove(LegRing.Count - 1);
				ptCurr = (Point)LegRing[LegRing.Count - 1].Clone();
				Point ptExpExt;

				//_UI.DrawPointWithText(ptCurr, "ptCurr");
				//ProcessMessages();

				double DistToExp, DistToEndOfLeg = ARANFunctions.PointToLineDistance(ptCurr, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
				//else
				//{
				//	double fTmp = lEndFIX.FlyMode == eFlyMode.Flyby ? lEndFIX.LPT - OverlapDist : -lEndFIX.LPT - OverlapDist;
				//	if (lEndFIX.FlyMode != eFlyMode.Flyby)
				//		fTmp = -lEndFIX.LPT - OverlapDist;
				//	ptEndOfLeg = ARANFunctions.LocalToPrj(lEndFIX.PrjPt, OutDir + ARANMath.C_PI, fTmp, 0);

				//	DistToEndOfLeg = ARANFunctions.PointToLineDistance(ptCurr, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
				//}

				if (lStartFIX.SemiWidth > lEndFIX.SemiWidth)
				{
					double DistToTransit = (lStartFIX.SemiWidth - lEndFIX.SemiWidth) / Math.Tan(_constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value) - lEndFIX.ATT;
					ptExpExt = ARANFunctions.LocalToPrj(lEndFIX.PrjPt, OutDir, -DistToTransit, 0);
					DistToExp = ARANFunctions.PointToLineDistance(ptCurr, ptExpExt, OutDir + ARANMath.C_PI_2);

					if (DistToExp < ARANMath.EpsilonDistance)
					{
						ptExpExt = ARANFunctions.LocalToPrj(ptEndOfLeg, OutDir, -DistToEndOfLeg, 0);                //?????????????
						//DistToExp = ARANFunctions.PointToLineDistance(ptCurr, ptExpExt, OutDir + ARANMath.C_PI_2);  // 0.0 ?????????
					}
				}
				else if (lStartFIX.SemiWidth < lEndFIX.SemiWidth)                    //DistToTransit = lEndFIX.ATT;
					ptExpExt = ARANFunctions.LocalToPrj(lEndFIX.PrjPt, OutDir, -lEndFIX.ATT, 0);
				else
					ptExpExt = ptEndOfLeg;

				DistToExp = ARANFunctions.PointToLineDistance(ptCurr, ptExpExt, OutDir + ARANMath.C_PI_2);

				double ASW_0C = IsPrimary ? 0.5 * lStartFIX.SemiWidth : lStartFIX.SemiWidth;
				double ASW_1C = IsPrimary ? 0.5 * lEndFIX.SemiWidth : lEndFIX.SemiWidth;

				//_UI.DrawPointWithText(ptEndOfLeg, "ptEndOfLeg");
				//_UI.DrawPointWithText(ptExpExt, "ptExpExt");
				//_UI.DrawPointWithText(ptCurr, "ptCurr");
				//ProcessMessages();

				int exp0 = 0, exp1 = 0;

				if (DistToEndOfLeg > ARANMath.EpsilonDistance)
				{
					Point ptBase0, ptBase1;
					ptBase1 = ARANFunctions.LocalToPrj(ptEndOfLeg, OutDir + ARANMath.C_PI_2 * fSide, ASW_1C, 0);
					//Point ptBase0 = ARANFunctions.LocalToPrj(ptExpExt, OutDir + ARANMath.C_PI_2 * fSide, ASW_1C, 0);
					double BaseDir0 = 0, BaseDir1 = 0;

					if (lStartFIX.SensorType != eSensorType.GNSS)    // lStartFIX.Role == eFIXRole.FAF_ && lEndFIX.Role == eFIXRole.MAPt_
					{
						double LegLenght = ARANMath.Hypot(lEndFIX.PrjPt.X - lStartFIX.PrjPt.X, lEndFIX.PrjPt.Y - lStartFIX.PrjPt.Y);
						double dPhi1 = Math.Atan2(ASW_0C - ASW_1C, LegLenght);

						if (dPhi1 < 0.0)
						{
							if (dPhi1 < -SplayAngle15)
								dPhi1 = -SplayAngle15;
						}
						else if (dPhi1 > DivergenceAngle30)
							dPhi1 = DivergenceAngle30;

						BaseDir0 = OutDir - dPhi1 * fSide;
						BaseDir1 = OutDir - dPhi1 * fSide;

						ptBase0 = ARANFunctions.LocalToPrj(lStartFIX.PrjPt, OutDir + ARANMath.C_PI_2 * fSide, ASW_0C, 0);
						ptBase1 = ARANFunctions.LocalToPrj(lEndFIX.PrjPt, OutDir + ARANMath.C_PI_2 * fSide, ASW_1C, 0);

						//ptBase0 = ARANFunctions.LocalToPrj(lEndFIX.PrjPt, OutDir + ARANMath.C_PI_2 * fSide, ASW_0C, 0);
						//ptBase1 = ARANFunctions.LocalToPrj(lEndFIX.PrjPt, OutDir + ARANMath.C_PI_2 * fSide, ASW_1C, 0);
					}
					else
					{
						BaseDir0 = OutDir;
						BaseDir1 = OutDir;

						Point ptCenter = ARANFunctions.LocalToPrj(lEndFIX.PrjPt, OutDir + ARANMath.C_PI, fTmp);
						ptBase0 = ARANFunctions.LocalToPrj(ptCenter, OutDir + ARANMath.C_PI_2 * fSide, ASW_1C, 0);
						ptBase1 = ARANFunctions.LocalToPrj(ptCenter, OutDir + ARANMath.C_PI_2 * fSide, ASW_1C, 0);
					}

					//_UI.DrawPointWithText(lStartFIX.PrjPt, -1, "StartFIX");
					//ProcessMessages();

					double ASW_0F = fSide * ARANFunctions.PointToLineDistance(ptCurr, lEndFIX.PrjPt, OutDir);
					Point ptInter = null;

					if (DistToExp > 0.0)
					{
						if (Math.Abs(ASW_0F - ASW_0C) > ARANMath.EpsilonDistance)   //(IsOuter || normalWidth) && 
						{
							if (ASW_0F > ASW_0C)
							{
								BaseDir0 = OutDir - DivergenceAngle30 * fSide;
								exp0 = 1;
							}
							else
							{
								BaseDir0 = OutDir + SplayAngle15 * fSide;
								exp0 = 2;
							}

							//ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, BaseDir0, ptBase0, BaseDir1);

							//if (lStartFIX.SensorType == eSensorType.GNSS)    // lStartFIX.Role == eFIXRole.FAF_ && lEndFIX.Role == eFIXRole.MAPt_
							{
								Point ptTmp = ARANFunctions.LocalToPrj(ptExpExt, OutDir, 0, fSide * ASW_0C);
								Geometry pGeom = ARANFunctions.LineLineIntersect(ptCurr, BaseDir0, ptTmp, OutDir);
								//_UI.DrawPointWithText(ptTmp, "ptTmp");
								//ProcessMessages();

								if (pGeom.Type != GeometryType.Point)
									ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, BaseDir0, ptExpExt, OutDir + ARANMath.C_PI_2);
								else
									ptInter = (Point)pGeom;

								//_UI.DrawPointWithText(ptInter, "ptInter-1");
								//ProcessMessages();

								DistToExp = ARANFunctions.PointToLineDistance(ptInter, ptExpExt, OutDir + ARANMath.C_PI_2);
								DistToEndOfLeg = ARANFunctions.PointToLineDistance(ptInter, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
								double Dist0 = ARANFunctions.PointToLineDistance(ptInter, ptCurr, OutDir - ARANMath.C_PI_2);

								if ((DistToExp < 0.0 && DistToEndOfLeg < -ARANMath.EpsilonDistance) || Dist0 < -ARANMath.EpsilonDistance)
								{
									double delta = fSide * ARANFunctions.PointToLineDistance(ptInter, ptBase1, OutDir);

									if (delta < ARANMath.EpsilonDistance)
									{
										if (ASW_0F < ASW_0C && exp0 == 2)
											;
										else
											BaseDir0 = OutDir - DivergenceAngle30 * fSide;
									}
									else if (ASW_0F > ASW_0C)
										BaseDir0 = OutDir + SplayAngle15 * fSide;


									//ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, BaseDir0, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
									//_UI.DrawPointWithText(ptInter, -1, "ptInter-3");
									//ProcessMessages();
									//DistToEndOfLeg = 0.0;
								}
							}
							//else
							//	ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, BaseDir0, ptBase0, BaseDir1);

							//_UI.DrawPointWithText(ptCurr, "ptCurr-2");
							//_UI.DrawPointWithText(lEndFIX.PrjPt, "EndF");
							//_UI.DrawPointWithText(ptInter, "ptInter-3");
							//ProcessMessages();

							//ptTmp = ARANFunctions.PrjToLocal(lEndFIX.PrjPt, OutDir, ptInter);

							ptCurr = ptInter;
							LegRing.Add(ptCurr);
						}

						DistToExp = ARANFunctions.PointToLineDistance(ptCurr, ptExpExt, OutDir + ARANMath.C_PI_2);

						if (DistToExp > ARANMath.EpsilonDistance) //&& expansAtTransition
						{
							if (lStartFIX.SensorType == eSensorType.GNSS)
								ptCurr = (Point)ARANFunctions.LineLineIntersect(ptCurr, OutDir, ptExpExt, OutDir + ARANMath.C_PI_2);
							else
								ptCurr = (Point)ARANFunctions.LineLineIntersect(ptCurr, BaseDir1, ptExpExt, OutDir + ARANMath.C_PI_2);

							//_UI.DrawPointWithText(ptCurr, "ptCurr-3");
							//ProcessMessages();

							DistToExp = 0.0;
							LegRing.Add(ptCurr);
							exp0 = 0;
						}

						DistToEndOfLeg = ARANFunctions.PointToLineDistance(ptCurr, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
						//ASW_0F = fSide * ARANFunctions.PointToLineDistance(ptInter, lStartFIX.PrjPt, OutDir);
					}

					normalWidth = true;

					//	LineString ls = new LineString();
					//	ls.Add(ptCurr);
					//	ls.Add(ARANFunctions.LocalToPrj(ptCurr, BaseDir1, 10000, 0));
					//	_UI.DrawLineString(ls, 255, 2);
					//	ProcessMessages();

					//	_UI.DrawPointWithText(ARANFunctions.LocalToPrj(ptCurr, BaseDir1, 10000, 0), 255, "BaseDir");
					//	_UI.DrawPointWithText(ptBase1, 255, "ptBase1");
					//	ProcessMessages();

					double Delta = fSide * ARANFunctions.PointToLineDistance(ptCurr, ptBase1, OutDir);

					if (DistToEndOfLeg > ARANMath.EpsilonDistance && Math.Abs(Delta) > ARANMath.EpsilonDistance)
					{
						if (lStartFIX.SensorType == eSensorType.GNSS)
						{
							if (Delta > 0.0)
							{
								if (ASW_0C > ASW_1C)
									BaseDir1 = OutDir - SpiralDivergenceAngle * fSide;
								else
									BaseDir1 = OutDir - DivergenceAngle30 * fSide;

								exp1 = 1;

								//LineString ls = new LineString();
								//ls.Add(ptCurr);
								//ls.Add(ARANFunctions.LocalToPrj(ptCurr, BaseDir1, 10000, 0));
								//_UI.DrawLineString(ls, 2, 255);
								//ProcessMessages();
							}
							else
							{
								if (ASW_0C > ASW_1C)
									BaseDir0 = OutDir + SplayAngle15 * fSide;
								else
									BaseDir1 = OutDir + SpiralSplayAngle * fSide;

								exp1 = 2;
							}
						}

						if (expansAtTransition || DistToExp < DistToEndOfLeg)
						{
							Point ptTmp = ARANFunctions.LocalToPrj(ptExpExt, OutDir, 0, fSide * ASW_0C);
							//_UI.DrawPointWithText(ptTmp, "ptTmp1");
							//ProcessMessages();

							double x1, y1, k1 = -Math.Sign(Delta);
							ARANFunctions.PrjToLocal(ptTmp, BaseDir1, ptCurr, out x1, out y1);

							if (exp1 == 2 && k1 * y1 < -k1 * ARANMath.EpsilonDistance && x1 > 0.0)//ARANMath.EpsilonDistance
							{
								double x0 = 0.0, y0 = 0.0;

								while (k1 * y1 < 0.0 && x1 > 0.0 && LegRing.Count > 1)
								{
									x0 = x1; y0 = y1;

									//ProcessMessages(true);

									LegRing.Remove(LegRing.Count - 1);
									ptCurr = LegRing[LegRing.Count - 1];
									ARANFunctions.PrjToLocal(ptTmp, BaseDir1, ptCurr, out x1, out y1);
								}
								//_UI.DrawPointWithText(LegRing[LegRing.Count - 1], "ptCurr--" + LegRing.Count);
								//ProcessMessages();

								if (Math.Abs(y1 - y0) > ARANMath.Epsilon_2Distance)
								{
									double K = -y1 / (y1 - y0);

									//_UI.DrawPointWithText(ptTmp, "ptTmp1");
									//ProcessMessages();

									ptCurr = ARANFunctions.LocalToPrj(ptTmp, BaseDir1, x1 + K * (x1 - x0));
								}
								else
									ptCurr = ptTmp;

								//_UI.DrawPointWithText(ptCurr, "ptCurr-4");
								//_UI.DrawPointWithText(ptTmp, "ptTmp");
								//_UI.DrawPointWithText(ptExpExt, "ptExpExt");
								//_UI.DrawPointWithText(ptEndOfLeg, "ptEndOfLeg");
								//ProcessMessages();

								LegRing.Add(ptCurr);
								//if (x1 < 0.0)
								exp0 = 0;


								//_UI.DrawPointWithText(ptCurr, -1, "ptCurr");
								//ProcessMessages();

								//LineString ls = new LineString();
								//ls.Add(ptTmp);
								//ls.Add(ARANFunctions.LocalToPrj(ptTmp, BaseDir1, 10000, 0));
								//_UI.DrawLineString(ls, -1, 2);
								//ProcessMessages();
							}

							if (exp0 != 0 && exp0 != exp1)
								ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, BaseDir0, ptBase1, BaseDir1);
							else
								ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, BaseDir1, ptBase1, OutDir);

							//_UI.DrawPointWithText(ptInter,  "ptInter-2");
							//ProcessMessages();
						}
						else
							ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, BaseDir1, ptBase1, OutDir);

						//ProcessMessages(true);
						//_UI.DrawPointWithText(ptInter,  "ptInter-4");
						//ProcessMessages();

						DistToEndOfLeg = ARANFunctions.PointToLineDistance(ptInter, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
						double Dist0 = ARANFunctions.PointToLineDistance(ptInter, ptCurr, OutDir - ARANMath.C_PI_2);

						if (DistToEndOfLeg < -ARANMath.EpsilonDistance && !expansAtTransition)   //if (DistToCenter < 0 && Dist0 < 0)	//
						{
							ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, BaseDir1, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
							DistToEndOfLeg = ARANFunctions.PointToLineDistance(ptInter, ptEndOfLeg, OutDir + ARANMath.C_PI_2);
						}

						//_UI.DrawPointWithText(ptInter,  "ptInter-1");
						//ProcessMessages();

						ptCurr = ptInter;
						LegRing.Add(ptCurr);
					}

					if (DistToEndOfLeg > ARANMath.EpsilonDistance)
					{
						ptCurr = ARANFunctions.LocalToPrj(ptEndOfLeg, OutDir, 0, ASW_1C * fSide);
						LegRing.Add(ptCurr);
					}
				}
			}

			double DistToFIX = (lEndFIX.FlyMode == eFlyMode.Flyby ? _EndFIX.LPT : -_EndFIX.LPT) - OverlapDist;
			Point ptCnt = ARANFunctions.LocalToPrj(_EndFIX.PrjPt, OutDir + ARANMath.C_PI, DistToFIX, 0);

			//Ring rr = new Ring();
			//rr.Assign(LegRing);
			//_UI.DrawRing(rr, -1, AranEnvironment.Symbols.eFillStyle.sfsHorizontal);
			//ProcessMessages();

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
			//_UI.DrawRing(rr, -1, AranEnvironment.Symbols.eFillStyle.sfsHorizontal);
			//ProcessMessages();
		}

		override protected Ring CreateOuterTurnArea(LegBase PrevLeg, ADHPType ARP, bool IsPrimary, double EntryDir, TurnDirection TurnDir)
		{
			double OutDir = _StartFIX.OutDirection;
			int transitions = 0;

			WayPoint prev = _StartFIX;
			WayPoint[] wptTransitions = new WayPoint[3];
			Point ptTransition;

			if (_StartFIX.FlightPhase < eFlightPhase.SIDGE28)
			{
				ptTransition = ARANFunctions.CircleVectorIntersect(ARP.pPtPrj, PANSOPSConstantList.PBNInternalTriggerDistance, _EndFIX.PrjPt, OutDir);

				if (!ptTransition.IsEmpty)
				{
					Point ptS = ARANFunctions.PrjToLocal(ptTransition, OutDir, _StartFIX.PrjPt);
					Point ptE = ARANFunctions.PrjToLocal(ptTransition, OutDir, _EndFIX.PrjPt);
					//_UI.DrawPointWithText(ptTransition, ARANFunctions.RGB(0, 0, 255), "ptTransition1");
					//LegBase.ProcessMessages();

					if (ptS.X * ptE.X < 0.0)
					{
						WayPoint WPT_15NM = new WayPoint(eFIXRole.TP_, _aranEnv);
						WPT_15NM.Assign(prev);
						WPT_15NM.PrjPt = ptTransition;  //WPT_15NM.PrjPt = ARANFunctions.LocalToPrj(ptTransition, OutDir, -WPT_15NM.ATT, 0);

						double dist = ARANFunctions.ReturnDistanceInMeters(prev.PrjPt, ptTransition);
						WPT_15NM.ConstructAltitude = prev.ConstructAltitude + dist * _constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpMaxPosPDG].Value;

						WPT_15NM.FlyMode = eFlyMode.Flyby;
						WPT_15NM.FlightPhase = eFlightPhase.SIDGE28;
						WPT_15NM.BankAngle = prev.BankAngle;
						WPT_15NM.EntryDirection = OutDir;
						//WPT_15NM.OutDirection = OutDir;

						wptTransitions[transitions] = WPT_15NM;
						transitions++;

						prev = WPT_15NM;

						double bank = _EndFIX.BankAngle;
						_EndFIX.FlightPhase = eFlightPhase.SIDGE28;
						//if (_EndFIX.FlightPhase == eFlightPhase.STAR && _EndFIX.PBNType == ePBNClass.RNAV5)							_EndFIX.PBNType = ePBNClass.RNAV1;
						_EndFIX.BankAngle = bank;

						//_UI.DrawPointWithText(ptTransition, ARANFunctions.RGB(0, 0, 255), "ptTransition1");
					}
				}
			}

			if (_StartFIX.FlightPhase < eFlightPhase.SIDGE56)
			{
				ptTransition = ARANFunctions.CircleVectorIntersect(ARP.pPtPrj, PANSOPSConstantList.PBNTerminalTriggerDistance, _EndFIX.PrjPt, OutDir);

				if (!ptTransition.IsEmpty)
				{
					Point ptS = ARANFunctions.PrjToLocal(ptTransition, OutDir, _StartFIX.PrjPt);
					Point ptE = ARANFunctions.PrjToLocal(ptTransition, OutDir, _EndFIX.PrjPt);

					if (ptS.X * ptE.X < 0.0)
					{
						WayPoint WPT_30NM = new WayPoint(eFIXRole.TP_, _aranEnv);
						WPT_30NM.Assign(prev);
						WPT_30NM.PrjPt = ptTransition;          // ARANFunctions.LocalToPrj(ptTransition, OutDir, -WPT_30NM.ATT, 0);

						double dist = ARANFunctions.ReturnDistanceInMeters(prev.PrjPt, ptTransition);
						WPT_30NM.ConstructAltitude = prev.ConstructAltitude + dist * _constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpMaxPosPDG].Value;

						WPT_30NM.FlyMode = eFlyMode.Flyby;
						WPT_30NM.FlightPhase = eFlightPhase.SIDGE56;
						WPT_30NM.BankAngle = prev.BankAngle;
						//WPT_30NM.EntryDirection = OutDir;
						WPT_30NM.OutDirection = OutDir;

						wptTransitions[transitions] = WPT_30NM;
						transitions++;

						double bank = _EndFIX.BankAngle;
						_EndFIX.FlightPhase = eFlightPhase.SIDGE56;
						_EndFIX.BankAngle = bank;

						//ptTmp = ARANFunctions.PrjToLocal(EndFIX.PrjPt, OutDir - ARANMath.C_PI, ARP.pPtPrj);
						//Dist0 = ptTmp.X - Math.Sqrt(ARANMath.Sqr(fInternalTreshold) - ARANMath.Sqr(ptTmp.Y));
						//_UI.DrawPointWithText(ptTransition, ARANFunctions.RGB(0, 0, 255), "ptTransition2");
						//if (Dist0 > TransitionDist)					TransitionDist = Dist0;
					}
				}
			}

			wptTransitions[transitions++] = (WayPoint)_EndFIX.Clone();

			//MAX DISTANCE + =================================================================
			double K, dAlpha, AztEnd1, AztEnd2, fTmp, 
				SpTurnAng, SpToAngle,  CurWidth, CurDist,
				dRad, SpAbeamDist, BulgeAngle,
				Delta, ASW_OUT0C, ASW_OUT0F, ASW_OUTMax, ASW_OUT1;

			//Point ptInter, ptBase,	ptCut, ptCnt, ptCurr, ptCurr1;

			bool bFlag;

			//MAX DISTANCE + =================================================================
			Point ptTmp;
			double LPTYDist = (_EndFIX.FlyMode == eFlyMode.Flyby ? _EndFIX.LPT : -_EndFIX.LPT) - OverlapDist;
			double TransitionDist = LPTYDist;// (_EndFIX.FlyMode == eFlyMode.Flyby ? _EndFIX.LPT : -_EndFIX.LPT) - OverlapDist; // LPTYDist;

			if (transitions > 1)
			{
				double fDistTreshold = ARANMath.Hypot(ARP.pPtPrj.X - wptTransitions[0].PrjPt.X, ARP.pPtPrj.Y - wptTransitions[0].PrjPt.Y);

				ptTmp = ARANFunctions.PrjToLocal(wptTransitions[0].PrjPt, OutDir - ARANMath.C_PI, ARP.pPtPrj);
				TransitionDist = ptTmp.X - Math.Sqrt(ARANMath.Sqr(fDistTreshold) - ARANMath.Sqr(ptTmp.Y));
			}

			if (_StartFIX.Role == eFIXRole.IF_ && _EndFIX.Role == eFIXRole.FAF_)
			{
				double Dist3700 = 1.5 * (_StartFIX.XTT - _EndFIX.XTT) / Math.Tan(_constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value);
				TransitionDist = Math.Max(TransitionDist, Dist3700);
			}

			//MAX DISTANCE - =================================================================

			double fSide = (int)TurnDir;
			double TurnAng = ARANMath.Modulus((OutDir - EntryDir) * fSide, ARANMath.C_2xPI);
			if (TurnAng < ARANMath.EpsilonRadian || Math.Abs(TurnAng - ARANMath.C_2xPI) < ARANMath.EpsilonRadian)
				TurnAng = 0.0;

			//LPT = -_StartFIX.EPT - 1.0;
			//if (_StartFIX.SemiWidth < _EndFIX.SemiWidth)
			//	LPT = -_StartFIX.LPT;

			//_UI.DrawPointWithText(_StartFIX.BasePoints[0], 255, "BPt[0]");
			//_UI.DrawPointWithText(ptTmp, "LPT");
			//ProcessMessages();
			//List<Point> BasePoints = new List<Point>();

			System.Collections.Generic.List<Point> BasePoints;

			if (_StartFIX.FlyMode == eFlyMode.Atheight)
			{
				GeometryOperators GeoOperators = new GeometryOperators();
				LineString pCutter = new LineString();
				pCutter.Add(ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, 0.0, -2 * _StartFIX.ASW_R));
				pCutter.Add(ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, 0.0, 2 * _StartFIX.ASW_R));

				Polygon tmpPoly;
				Geometry geom1, geom2;

				try
				{
					GeoOperators.Cut(PrevLeg.PrimaryArea, pCutter, out geom1, out geom2);
					tmpPoly = ((MultiPolygon)geom1)[0];
				}
				catch
				{
					tmpPoly = PrevLeg.PrimaryArea[0];
				}

				BasePoints = ARANFunctions.GetBasePoints(_StartFIX.PrjPt, _StartFIX.EntryDirection, tmpPoly, TurnDir);  //PrevLeg.PrimaryArea[0]
			}
			else
			{
				//BasePoints = new System.Collections.Generic.List<Point>();
				BasePoints = _StartFIX.BasePoints;
			}

			//for (int gh = 0; gh < BasePoints.Count; gh++)
			//	_UI.DrawPointWithText(BasePoints[gh], "pt-" + gh, 0);
			//ProcessMessages();

			double dirN0 = ARANFunctions.ReturnAngleInRadians(BasePoints[BasePoints.Count - 1], BasePoints[0]);
			double dir01 = ARANFunctions.ReturnAngleInRadians(BasePoints[0], BasePoints[1]);
			double CurrDir = ARANMath.SubtractAngles(dir01, dirN0);             //dir01- dirN0;

			//_UI.DrawPointWithText(ptTmp, 255, "LPT");
			//	_UI.DrawPointWithText(_StartFIX.PrjPt, 255, "FIX");
			//	LineString lstr = new LineString();
			//	lstr.Add(ARANFunctions.LocalToPrj(ptTmp, EntryDir, 0, -10000));
			//	lstr.Add(ptTmp);
			//	lstr.Add(ARANFunctions.LocalToPrj(ptTmp, EntryDir, 0, 10000));
			//	_UI.DrawLineString(lstr, 255, 2);
			//ProcessMessages();

			//_UI.DrawRing(PrevLeg.PrimaryArea[0].ExteriorRing, 233,eFillStyle.sfsCross);
			//ProcessMessages();
			double LPT;
			//_StartFIX.CalcTurnRangePoints();

			if (_StartFIX.IsDFTarget)
			{
				if (TurnDir == TurnDirection.CW)
					LPT = (_StartFIX.FlyMode == eFlyMode.Flyby ? -_StartFIX.LPT_L : _StartFIX.LPT_L);
				else
					LPT = (_StartFIX.FlyMode == eFlyMode.Flyby ? -_StartFIX.LPT_R : _StartFIX.LPT_R);

				//if (_StartFIX.FlyMode == eFlyMode.Flyby && LPT > 0)			LPT = 0.0;
				//if (_StartFIX.FlyMode == eFlyMode.Flyby && (LPT > 200|| LPT < 750))					LPT = 0.1 * LPT;
			}
			else
			{
				LPT = (_StartFIX.FlyMode == eFlyMode.Flyby ? -_StartFIX.LPT : _StartFIX.LPT);

				if (ARANMath.RadToDeg(TurnAng) <= 5)
				{
					LPT = -_StartFIX.ATT;
					double ASW = TurnDir == TurnDirection.CW ? _StartFIX.ASW_R : _StartFIX.ASW_L;
					if (ASW < _StartFIX.SemiWidth - ARANMath.EpsilonDistance)
						LPT = -_StartFIX.ATT;
					else if (ASW > _StartFIX.SemiWidth + ARANMath.EpsilonDistance)
						LPT = -_StartFIX.LPT;
				}
			}

			ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, LPT - 0.5);

			//if
			//ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, FlyMode == eFlyMode.FlyBy ? EntryDir - ARANMath.C_PI : EntryDir, LPT, 0);
			//ptTmp = ARANFunctions.LocalToPrj(StartFIX.PrjPt, EntryDir - ARANMath.C_PI * BYTE(FlyMode = fmFlyBy), StartFIX.LPT, 0);

			//_UI.DrawPointWithText(ptTmp, "ptLTP_0");
			//ProcessMessages();

			Point OuterBasePoint, InnerBasePoint, ptFrom = null;
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
						//_UI.DrawRing(PrevLeg.PrimaryArea[0].ExteriorRing, -1, AranEnvironment.Symbols.eFillStyle.sfsNull);
						//ProcessMessages();
						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}

					ASW_OUT0C = 0.5 * _StartFIX.SemiWidth;
					ASW_OUT1 = 0.5 * wptTransitions[0].SemiWidth;
					dRad = 0;
					InnerBasePoint = BasePoints[1];
					OuterBasePoint = BasePoints[0];
				}
				else
				{
					ASW_OUT0F = _StartFIX.ASW_R;
					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir - 0.5 * ARANMath.C_PI, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, StartFIX.PrjPt, OutDir - 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						//	_UI.DrawPointWithText(ptFrom, 0, "ptFrom");
						//_UI.DrawPointWithText(ptTmp, 0, "ptTmp");
						//	ProcessMessages();
						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}

					ASW_OUT0C = _StartFIX.SemiWidth;
					ASW_OUT1 = wptTransitions[0].SemiWidth;
					dRad = ASW_OUT0F - _StartFIX.ASW_2_R;

					InnerBasePoint = ARANFunctions.LocalToPrj(BasePoints[1], dir01, _StartFIX.ASW_2_R);
					OuterBasePoint = ARANFunctions.LocalToPrj(BasePoints[0], dir01, -_StartFIX.ASW_2_L);
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
						//_UI.DrawRing(PrevLeg.PrimaryArea[0].ExteriorRing, 167, AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross);
						//_UI.DrawPointWithText(ptTmp, -1, "LPT");
						//_UI.DrawPointWithText(ptFrom, -1, "ptFrom-0");
						//ProcessMessages();

						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}

					ASW_OUT0C = 0.5 * _StartFIX.SemiWidth;
					ASW_OUT1 = 0.5 * wptTransitions[0].SemiWidth;
					dRad = 0;
					InnerBasePoint = BasePoints[1];
					OuterBasePoint = BasePoints[0];
				}
				else
				{
					ASW_OUT0F = _StartFIX.ASW_L;
					if (PrevLeg != null)
					{
						ptFrom = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir + 0.5 * ARANMath.C_PI, out fTmp);
						//ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, StartFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}

					ASW_OUT0C = _StartFIX.SemiWidth;
					ASW_OUT1 = wptTransitions[0].SemiWidth;
					dRad = ASW_OUT0F - _StartFIX.ASW_2_L;

					InnerBasePoint = ARANFunctions.LocalToPrj(BasePoints[1], dir01, _StartFIX.ASW_2_L);
					OuterBasePoint = ARANFunctions.LocalToPrj(BasePoints[0], dir01, -_StartFIX.ASW_2_R);
				}
				//InnerBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir - 0.5 * ARANMath.C_PI, _StartFIX.ASW_2_R, 0);
				//OuterBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir + 0.5 * ARANMath.C_PI, StartFIX.ASW_2_L, 0);
			}

			//_UI.DrawPointWithText(InnerBasePoint, -1, "InnerBasePoint");
			//_UI.DrawPointWithText(OuterBasePoint, -1, "OuterBasePoint");
			//ProcessMessages();

			//_UI.DrawPointWithText(ptFrom, "ptFrom");
			//ProcessMessages();

			ASW_OUTMax = ASW_OUT0C;

			Ring result = new Ring();

			//_UI.DrawRing(PrevLeg.PrimaryArea[0].ExteriorRing, -1, AranEnvironment.Symbols.eFillStyle.sfsVertical);
			//ProcessMessages();

			if (_StartFIX.FlyMode == eFlyMode.Atheight)
			{
				result.Add(BasePoints[0]);
				//_UI.DrawPointWithText(BasePoints[0], "BasePoints[0]");
				//ProcessMessages();

				if (!IsPrimary)
				{
					ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir - ARANMath.C_PI, _StartFIX.EPT + 10.0, 0);
					ptTmp = ARANFunctions.RingVectorIntersect(PrevLeg.FullArea[0].ExteriorRing, ptTmp, EntryDir - 0.5 * ARANMath.C_PI, out fTmp);
					//_UI.DrawPointWithText(ptFrom, -1, "ptFrom-1");
					//ProcessMessages();

					if (ptTmp != null)
						result.Add(ptTmp);
				}
			}

			//result.Add(_StartFIX.PrjPt);
			//result.Add(ptTmp);

			if (ptFrom == null)
			{
				ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir - ARANMath.C_PI, _StartFIX.EPT + 10.0, ASW_OUT0F * fSide);
				//_UI.DrawPointWithText(ptTmp, -1, "ptTmp1");
				//ProcessMessages();

				result.Add(ptTmp);

				ptFrom = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir - 0.5 * ARANMath.C_PI * fSide, ASW_OUT0F, 0);
				//	_UI.DrawPointWithText(ptFrom, -1, "ptFrom-f");
				//	ProcessMessages();
			}
			else
				OuterBasePoint = (Point)ptFrom;	//.Clone();

			//_UI.DrawPointWithText(ptFrom, "ptFrom");
			//ProcessMessages();

			result.Add(ptFrom);

			if (ARANMath.RadToDeg(TurnAng) <= 5)
			{
				//Ring rr = new Ring();
				//rr.Assign(result);
				//_UI.DrawRing(rr, AranEnvironment.Symbols.eFillStyle.sfsHorizontal);

				//_UI.DrawPointWithText(ptFrom,"pt-01");
				//ProcessMessages();


				JoinSegments(result, ARP, true, IsPrimary, EntryDir, TurnDir);

				//_UI.DrawRing(result, -1, AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross);
				//ProcessMessages();
				return result;
			}

			//_UI.DrawPointWithText(OuterBasePoint, 0, "OuterBasePoint");
			//_UI.DrawPointWithText(InnerBasePoint, 0, "InnerBasePoint");
			//_StartFIX.CalcExtraTurnRangePoints();

			//Point ptCurr = null;
			//=============================================================================

			double DivergenceAngle30 = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value, SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
			double SpiralDivergenceAngle, SpiralSplayAngle;

			if (IsPrimary)
			{
				//SpiralDivergenceAngle = Math.Atan(0.5 * Math.Tan(DivergenceAngle30));
				//SpiralSplayAngle = Math.Atan(0.5 * Math.Tan(SplayAngle15));

				SpiralDivergenceAngle = DivergenceAngle30;
				SpiralSplayAngle = SplayAngle15;
			}
			else
			{
				SpiralDivergenceAngle = DivergenceAngle30;
				SpiralSplayAngle = SplayAngle15;
			}

			//=================================================================
			double SpStartDir = EntryDir - ARANMath.C_PI_2 * fSide;
			double SpStartRad = SpStartDir;

			double Rv = 1765.27777777777777777 * Math.Tan(_StartFIX.BankAngle) / (ARANMath.C_PI * _StartFIX.ConstructTAS);
			if (Rv > 3.0)
				Rv = 3.0;
			double WSpeed = _constants.Pansops[ePANSOPSData.dpWind_Speed].Value;
			double coef = WSpeed / ARANMath.DegToRad(Rv);
			double TurnR = _StartFIX.ConstructTurnRadius + dRad;   // ARANMath.BankToRadius(_StartFIX.BankAngle, _StartFIX.ConstructTAS);

			BulgeAngle = Math.Atan2(coef, TurnR) * fSide;
			SpTurnAng = ARANFunctions.SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir - BulgeAngle, OutDir, TurnDir);
			//	SpTurnAng = SpiralTouchAngleOld(TurnR, coef, EntryDir, OutDir, TurnDir);

			double Dist0 = TurnR + SpTurnAng * coef;

			Point ptCurr = null, ptInter, ptCnt = ARANFunctions.LocalToPrj(OuterBasePoint, EntryDir + 0.5 * ARANMath.C_PI * fSide, TurnR, 0);
			ptTmp = ARANFunctions.LocalToPrj(ptCnt, SpStartDir + SpTurnAng * fSide, Dist0, 0);

			//_UI.DrawPointWithText(ptCnt, -1, "ptCnt-0");
			//_UI.DrawPointWithText(ptTmp, -1, "ptSp-1");
			//_UI.DrawPointWithText(StartFIX.PrjPt, -1, "FIX");
			//	ProcessMessages();

			SpAbeamDist = -fSide * ARANFunctions.PointToLineDistance(ptTmp, _EndFIX.PrjPt, OutDir);
			//??????? //SpAbeamDist = -fSide * ARANFunctions.PointToLineDistance(ptTmp, _StartFIX.PrjPt, OutDir);
			bool HaveSecondSP = (TurnAng >= CurrDir + SpiralSplayAngle) || ((TurnAng >= CurrDir - SpiralDivergenceAngle) && (SpAbeamDist > ASW_OUT0C));

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
			//SpToAngle = ARANFunctions.SpiralTouchAngleOld(TurnR, coef, EntryDir, AztEnd1, TurnDir);

			if (_StartFIX.FlyMode == eFlyMode.Flyby)
				SpTurnAng = ARANFunctions.SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir - BulgeAngle, EntryDir, TurnDir);      //		SpTurnAng = SpiralTouchAngleOld(TurnR, coef, EntryDir, EntryDir, TurnDir)
			else
				SpTurnAng = SpToAngle;

			int i, n = (int)Math.Ceiling(ARANMath.RadToDeg(SpTurnAng));

			if (n <= 1) n = 1;
			else if (n <= 5) n = 5;
			else if (n < 10) n = 10;

			dAlpha = SpTurnAng / n;

			for (i = 0; i <= n; i++)
			{
				double R = TurnR + i * dAlpha * coef;
				ptCurr = ARANFunctions.LocalToPrj(ptCnt, SpStartDir + i * dAlpha * fSide, R, 0);
				//_UI.DrawPointWithText(ptCurr,  (i + 1).ToString()); //,ARANFunctions.RGB(0, 128, 0)
				//ProcessMessages();
				result.Add(ptCurr);
			}

			//Ring rr = new Ring();
			//rr.Assign(result);
			//_UI.DrawRing(rr, AranEnvironment.Symbols.eFillStyle.sfsHorizontal);
			//ProcessMessages();

			//_UI.DrawPointWithText(ptCurr, -1, "ptC-1");
			//ProcessMessages();

			if (_StartFIX.FlyMode == eFlyMode.Flyby)
			{
				Dist0 = TurnR + SpToAngle * coef;

				Point ptCurr1 = ptCurr;
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
			//_UI.DrawRing(rr, -1, AranEnvironment.Symbols. eFillStyle.sfsHorizontal);
			//while(true)
			//ProcessMessages();

			//_UI.DrawPointWithText(ptCurr, ARANFunctions.RGB(0, 128, 0), "ptC-1");
			//ProcessMessages();

			//LineString lst = new LineString();
			//lst.Add(wptTransitions[0].PrjPt);
			//lst.Add(ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir - fSide * 0.5 * ARANMath.C_PI, ASW_OUTMax, 0));
			//_UI.DrawLineString(lst, 0, 2);
			//ProcessMessages();

			//_UI.DrawPointWithText(ptBase, ARANFunctions.RGB(0, 128, 0), "ptBase-0");
			//ProcessMessages();

			//_UI.DrawPointWithText(ptBase, ARANFunctions.RGB(0, 128, 0), "ptBase");
			//_UI.DrawPointWithText(wptTransitions[0].PrjPt, ARANFunctions.RGB(0, 128, 0), "ptStart");
			//ProcessMessages();

			//double BaseDir = OutDir;

			SpStartDir += SpToAngle * fSide;
			double SpFromAngle = SpToAngle;

			CurDist = ARANFunctions.PointToLineDistance(ptCurr, wptTransitions[0].PrjPt, OutDir + 0.5 * ARANMath.C_PI);
			CurWidth = -fSide * ARANFunctions.PointToLineDistance(ptCurr, _EndFIX.PrjPt, OutDir);

			Point ptBase;
			//bool IsMAPt = (_StartFIX.Role == eFIXRole.FAF_ && _EndFIX.Role == eFIXRole.MAPt_);

			//if (IsMAPt)
			//{
			//	Dist0 = ARANMath.Hypot(wptTransitions[0].PrjPt.X - _StartFIX.PrjPt.X, wptTransitions[0].PrjPt.Y - _StartFIX.PrjPt.Y);
			//	dPhi1 = Math.Atan2(ASW_OUT0C - ASW_OUT1, Dist0);
			//	if (dPhi1 > SpiralDivergenceAngle)
			//		dPhi1 = SpiralDivergenceAngle;

			//	SpAngle = OutDir + dPhi1 * fSide;

			//	ptBase = ARANFunctions.LocalToPrj(_EndFIX.PrjPt, OutDir - 0.5 * ARANMath.C_PI * fSide, ASW_OUT0C, 0);

			//	if (CurWidth < ASW_OUTMax)
			//		SplayAngle = OutDir - SpiralSplayAngle * fSide;
			//	else
			//		SplayAngle = OutDir + SpiralDivergenceAngle * fSide;

			//	ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, SplayAngle, ptBase, SpAngle);
			//	ptInterDist = ARANFunctions.PointToLineDistance(ptInter, wptTransitions[0].PrjPt, OutDir + 0.5 * ARANMath.C_PI);

			//	//MaxDist = Math.Max(MaxDist, ptInterDist);

			//	BaseDir = SpAngle;
			//	ASW_OUTMax = ARANFunctions.PointToLineDistance(ptInter, wptTransitions[0].PrjPt, OutDir);   //*fSide 
			//}

			ptBase = ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir - 0.5 * fSide * ARANMath.C_PI, ASW_OUTMax, 0);

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
							double PrevX = 0.0, PrevY = 0.0;

							//Point ptPrev = result[result.Count - 1];

							//while (true)
							//	ProcessMessages();

							for (i = 0; i <= n; i++)
							{
								double R = TurnR + (SpFromAngle + i * dAlpha) * coef;
								ptCurr = ARANFunctions.LocalToPrj(ptCnt, SpStartDir + i * dAlpha * fSide, R, 0);

								//_UI.DrawPointWithText(ptCurr, "ptC-2");
								//ProcessMessages();

								ptTmp = ARANFunctions.PrjToLocal(ptBase, OutDir, ptCurr);

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

								//_UI.DrawPointWithText(ptCurr, (i + 1).ToString());  //, ARANFunctions.RGB(0, 128, 0)
								//ProcessMessages();

								if (i > 0 && ((ptTmp.Y * fSide >= 0.0 && PrevY * fSide <= 0.0) || ptTmp.X >= 0.0))
								{
									if (ptTmp.X < 0.0) K = -PrevY / (ptTmp.Y - PrevY);
									else K = -PrevX / (ptTmp.X - PrevX);

									ptCurr.X = result[result.Count - 1].X + K * (ptCurr.X - result[result.Count - 1].X);
									ptCurr.Y = result[result.Count - 1].Y + K * (ptCurr.Y - result[result.Count - 1].Y);

									//_UI.DrawPointWithText(ptCurr, ARANFunctions.RGB(0, 128, 0), "__"+(i + 1).ToString());
									//ProcessMessages();
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

					double R = TurnR + SpFromAngle * coef;
					//ptCurr1 = ARANFunctions.LocalToPrj(ptCnt, SpStartDir + SpFromAngle * fSide, R, 0);
					Point ptCurr1 = ARANFunctions.LocalToPrj(ptCnt, SpStartDir, R, 0);

					//_UI.DrawPointWithText(ptCnt, 255, "ptCnt-2");
					//_UI.DrawPointWithText(InnerBasePoint, 255, "InnerBasePoint");
					//_UI.DrawPointWithText(ptCurr1, 0, "ptSp-2");
					//_UI.DrawPointWithText(ptBase, 0, "ptBase");
					//_UI.DrawPointWithText(ptCurr, "ptCurr-2");
					//ProcessMessages();
					////ProcessMessages(true);

					Delta = -fSide * ARANFunctions.PointToLineDistance(ptCurr1, ptBase, OutDir);   //

					//fTmp = ARANMath.Modulus(EntryDir + fSide * CurrDir - BaseDir, 2 * ARANMath.C_PI);
					fTmp = ARANMath.Modulus(dir01 - OutDir, 2 * ARANMath.C_PI);

					if (Math.Abs(fTmp) < ARANMath.EpsilonRadian || Math.Abs(fTmp - ARANMath.C_2xPI) < ARANMath.EpsilonRadian || Math.Abs(fTmp - ARANMath.C_PI) < ARANMath.EpsilonRadian)
					{ }
					//else if (Math.Abs(Delta) > ARANMath.EpsilonDistance)
					else if (Delta < -ARANMath.EpsilonDistance)
					{
						//ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, EntryDir + fSide * CurrDir, ptBase, BaseDir);
						ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, dir01, ptBase, OutDir);

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
			//		Dist0 = Hypot(wptTransitions[0].PrjPt.X - StartFIX.PrjPt.X, wptTransitions[0].PrjPt.Y - StartFIX.PrjPt.Y);
			//		dPhi1 = ArcTan2(ASW_OUT0C - ASW_OUT1, Dist0);
			//		if(dPhi1 > DivergenceAngle30) dPhi1 = DivergenceAngle30;

			//		SpAngle = OutDir - dPhi1 * fSide;
			//		LocalToPrj(StartFIX.PrjPt, OutDir + 0.5 * PI * fSide, ASW_OUT0C, 0, ptBase);

			//		if (CurWidth < ASW_OUTMax)
			//			SplayAngle = OutDir + SplayAngle15 * fSide;
			//		else
			//			SplayAngle = OutDir - DivergenceAngle30 * fSide;

			//		ptInter = LineLineIntersect(ptCurr, SplayAngle, InnerBasePoint, SpAngle).AsPoint;
			//		ptInterDist = PointToLineDistance(ptInter, wptTransitions[0].PrjPt, OutDir + 0.5 * PI);

			//		TransitionDist = Max(TransitionDist, ptInterDist);
			//		if (TransitionDist == ptInterDist)
			//		{
			//			IsMAPt = true;
			//			BaseDir = SpAngle;
			//			ASW_OUTMax = fSide * PointToLineDistance(ptInter, wptTransitions[0].PrjPt, OutDir);
			//		}
			//}

			//=============================================================================

			int tranI = 0;

			//_UI.DrawPointWithText(result[result.Count - 2], -1, "ptCurr_-1");
			//_UI.DrawPointWithText(result[result.Count - 1], -1, "ptCurr_0");
			//_UI.DrawPointWithText(ptCurr, -1, "ptCurr_1");
			//while(true)
			//	ProcessMessages();

			if (TransitionDist > CurDist)
			{
				ASW_OUTMax = ASW_OUT1;
				ptBase = ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir - fSide * ARANMath.C_PI_2, ASW_OUTMax, 0);    //???

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

					if (SpTurnAng >= ARANMath.EpsilonRadian)
					{
						n = (int)Math.Round(ARANMath.RadToDeg(SpTurnAng));
						if (n < 1) n = 1;
						else if (n < 5) n = 5;
						else if (n < 10) n = 10;

						dAlpha = SpTurnAng / n;
						bFlag = false;
						double PrevX = 0.0, PrevY = 0.0;

						for (i = 0; i <= n; i++)
						{
							double R = TurnR + (SpFromAngle + i * dAlpha) * coef;
							ptCurr = ARANFunctions.LocalToPrj(ptCnt, SpStartDir + i * dAlpha * fSide, R, 0);
							ptTmp = ARANFunctions.PrjToLocal(ptBase, OutDir, ptCurr);

							//_UI.DrawPointWithText(ptCnt, -1, "ptCnt");
							//LineString lst = new LineString();
							//lst.Add(ptBase);
							//lst.Add(ARANFunctions.LocalToPrj(ptBase, BaseDir + ARANMath.C_PI, 35000, 0));
							//_UI.DrawLineString(lst, -1, 2);

							//_UI.DrawPointWithText(ptBase, -1, "Base");

							//_UI.DrawPointWithText(ptCurr, (i + 1).ToString());	//, ARANFunctions.RGB(0, 128, 0)
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

								if (ptTmp.Y * fSide >= 0.0)//|| ptTmp.X >= 0.0)
								{
									if (ptTmp.X < 0) K = -PrevY / (ptTmp.Y - PrevY);
									else K = -PrevX / (ptTmp.X - PrevX);

									ptCurr.X = result[result.Count - 1].X + K * (ptCurr.X - result[result.Count - 1].X);
									ptCurr.Y = result[result.Count - 1].Y + K * (ptCurr.Y - result[result.Count - 1].Y);

									//_UI.DrawPointWithText(ptCurr, ARANFunctions.RGB(0, 128, 0), (i + 1).ToString());
									//ProcessMessages();
									if (K > 0)
										result.Add(ptCurr);
									break;
								}
							}

							//if (i == n && IsPrimary)			_EndFIX.JointFlags = _EndFIX.JointFlags | 1;

							PrevX = ptTmp.X;
							PrevY = ptTmp.Y;
							result.Add(ptCurr);
						}

						//ProcessMessages(true);
						//Ring rr = new Ring();
						//rr.Assign(result);
						//_UI.DrawRing(rr, AranEnvironment.Symbols.eFillStyle.sfsHorizontal);
						//ProcessMessages();
					}
				}
				else// if (ASW_OUTMax - CurWidth > ARANMath.EpsilonDistance)
				{
					double SplayAngle = OutDir - SpiralSplayAngle * fSide;

					//_UI.DrawPointWithText(ptCurr, 0, "ptCurr");
					//_UI.DrawPointWithText(ptBase, 0, "ptBase");
					//_UI.DrawPointWithText(wptTransitions[0].PrjPt, -1, "wptTrans-0");
					//_UI.DrawPointWithText(_StartFIX.PrjPt, -1, "_StartFIX");
					//ProcessMessages();

					ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, SplayAngle, ptBase, OutDir);
					//	_UI.DrawPointWithText(ptInter, 0, "ptInter - 2");
					//	ProcessMessages();

					double ptInterDist = ARANFunctions.PointToLineDistance(ptInter, wptTransitions[tranI].PrjPt, OutDir + 0.5 * ARANMath.C_PI);

					//_UI.DrawPointWithText(ptInter, 0, "ptInter - 2");
					//ProcessMessages(true);

					if (ptInterDist < TransitionDist)
					{
						Point ptCut = ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir + ARANMath.C_PI, TransitionDist, 0);
						ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, SplayAngle, ptCut, OutDir + 0.5 * ARANMath.C_PI);
						//if GlDRAWFLG then
						//		_UI.DrawPointWithText(ptInter, 0, "ptInter - 2");
					}
					result.Add(ptInter);
				}
			}

			//ProcessMessages(true);

			//_UI.DrawRing(result, -1, AranEnvironment.Symbols.eFillStyle.sfsCross);
			//ProcessMessages();

			JoinSegments(result, ARP, true, IsPrimary, EntryDir, TurnDir);

			//_UI.DrawRing(result, -1, AranEnvironment.Symbols.eFillStyle.sfsHorizontal);
			//ProcessMessages();

			//ProcessMessages(true);

			return result;
		}

		override protected Ring CreateInnerTurnArea(LegBase PrevLeg, ADHPType ARP, bool IsPrimary, double EntryDir, TurnDirection TurnDir)
		{
			double fDistTreshold, OutDir, TurnAng, Dist56000, CurDist, MaxDist, TransitionDist, CurWidth,
				LPTYDist, fTmp, DivergenceAngle30, SpiralDivergenceAngle, SplayAngle15,
				BaseDir, LegLenght, SplayAngle, fSide, ASW_INMax, ASW_IN0C, ASW_IN0F, ASW_IN1;

			Point InnerBasePoint, ptTmp, ptBase, ptCurr, ptInter, ptFrom = null;

			SpiralDivergenceAngle = _constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;

			if (IsPrimary)
			{
				//fTmp = GlobalVars.constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;
				DivergenceAngle30 = Math.Atan(0.5 * Math.Tan(SpiralDivergenceAngle));
				//SplayAngle15 = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
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

			WayPoint[] wptTransitions = new WayPoint[3];
			Point ptTransition;
			int transitions = 0;

			if (_StartFIX.FlightPhase < eFlightPhase.SIDGE28)
			{
				ptTransition = ARANFunctions.CircleVectorIntersect(ARP.pPtPrj, PANSOPSConstantList.PBNInternalTriggerDistance, _EndFIX.PrjPt, OutDir);

				if (!ptTransition.IsEmpty)
				{
					Point ptS = ARANFunctions.PrjToLocal(ptTransition, OutDir, _StartFIX.PrjPt);
					Point ptE = ARANFunctions.PrjToLocal(ptTransition, OutDir, _EndFIX.PrjPt);

					if (ptS.X * ptE.X < 0.0)
					{
						WayPoint WPT_15NM = new WayPoint(eFIXRole.TP_, _aranEnv);
						WPT_15NM.FlightPhase = eFlightPhase.SIDGE28;
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
			}

			if (_StartFIX.FlightPhase < eFlightPhase.SIDGE56)
			{
				ptTransition = ARANFunctions.CircleVectorIntersect(ARP.pPtPrj, PANSOPSConstantList.PBNTerminalTriggerDistance, _EndFIX.PrjPt, OutDir);

				if (!ptTransition.IsEmpty)
				{
					Point ptS = ARANFunctions.PrjToLocal(ptTransition, OutDir, _StartFIX.PrjPt);
					Point ptE = ARANFunctions.PrjToLocal(ptTransition, OutDir, _EndFIX.PrjPt);

					if (ptS.X * ptE.X < 0.0)
					{
						//ptTmp = ARANFunctions.PrjToLocal(EndFIX.PrjPt, OutDir - ARANMath.C_PI, ARP.pPtPrj);
						//Dist0 = ptTmp.X - Math.Sqrt(ARANMath.Sqr(fInternalTreshold) - ARANMath.Sqr(ptTmp.Y));

						WayPoint WPT_30NM = new WayPoint(eFIXRole.TP_, _aranEnv);
						WPT_30NM.FlightPhase = eFlightPhase.SIDGE56;
						WPT_30NM.SensorType = _StartFIX.SensorType;

						WPT_30NM.PBNType = _StartFIX.PBNType;

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
			}

			wptTransitions[transitions++] = (WayPoint)_EndFIX.Clone();

			//MAX DISTANCE + =================================================================

			LPTYDist = (_EndFIX.FlyMode == eFlyMode.Flyby ? _EndFIX.LPT : -_EndFIX.LPT) - OverlapDist;
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

			//if (_StartFIX.Role == eFIXRole.IF_ && _EndFIX.Role == eFIXRole.FAF_)
			//{
			//	Dist3700 = 1.5 * (_StartFIX.XXT - _EndFIX.XXT) / Math.Tan(_constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value);

			//	//		Dist3700 := (StartFIX.SemiWidth - EndFIX.SemiWidth)/Tan(GPANSOPSConstants.Constant[arSecAreaCutAngl].Value);
			//	//GPANSOPSConstants.Constant[rnvImMinDist].Value;
			//	TransitionDist = Math.Max(TransitionDist, Dist3700);
			//}

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
			//_UI.DrawPointWithText(ptTmp, -1, "ptTmpI");
			//ProcessMessages();

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

			//_UI.DrawPointWithText(ptFrom, -1, "ptFrom");
			//ProcessMessages();

			Ring result = new Ring();

			int i, j, n;
			System.Collections.Generic.List<Point> BasePoints;
			ptBase = ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir + 0.5 * fSide * ARANMath.C_PI, ASW_IN0C);

			//_UI.DrawPointWithText(ptBase, -1, "ptBase");
			//ProcessMessages();

			if (_StartFIX.FlyMode == eFlyMode.Atheight)
			{
				GeometryOperators GeoOperators = new GeometryOperators();
				LineString pCutter = new LineString();
				pCutter.Add(ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, 0.0, -2 * _StartFIX.ASW_R));
				pCutter.Add(ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, 0.0, 2 * _StartFIX.ASW_R));

				Polygon tmpPoly;
				Geometry geom1, geom2;

				try
				{
					GeoOperators.Cut(PrevLeg.PrimaryArea, pCutter, out geom1, out geom2);

					//_UI.DrawMultiPolygon((MultiPolygon)geom1,-1, AranEnvironment.Symbols.eFillStyle.sfsHorizontal);
					//_UI.DrawMultiPolygon((MultiPolygon)geom2, -1, AranEnvironment.Symbols.eFillStyle.sfsVertical);
					//ProcessMessages();

					tmpPoly = ((MultiPolygon)geom1)[0];
				}
				catch
				{
					tmpPoly = PrevLeg.PrimaryArea[0];
				}

				//ProcessMessages(true);
				//for (i = 0; i < tmpPoly.ExteriorRing.Count; i++)
				//	_UI.DrawPointWithText(tmpPoly.ExteriorRing[i], -1, "ptE-" + (i + 1).ToString());
				//ProcessMessages();

				BasePoints = ARANFunctions.GetBasePoints(_StartFIX.PrjPt, _StartFIX.EntryDirection, tmpPoly, TurnDir);  //PrevLeg.PrimaryArea[0]
				n = BasePoints.Count;

				//for (i = 0; i < n; i++)
				//	_UI.DrawPointWithText(BasePoints[i], "ptB-" + i.ToString());
				//ProcessMessages();
				//bpt = new Point[n];

				//ptBase = ptTmp;

				//_UI.DrawPointWithText(wptTransitions[0].PrjPt, -1, "ptTrans0");
				//_UI.DrawPointWithText(ptBase, "ptBase");
				//ProcessMessages();

				double x, y, splay = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;//Math.Max(0.5 * TurnAng, SpiralDivergenceAngle);//

				i = 0;

				//ARANFunctions.PrjToLocal(_StartFIX.PrjPt, _StartFIX.EntryDirection, _EndFIX.PrjPt, out x, out y);
				//if (x > 0.0)					i += 2;

				ptCurr = BasePoints[i];

				//ProcessMessages(true);
				//_UI.DrawPointWithText(ptCurr, "ptCurr");
				//ProcessMessages();

				//LineString ls = new LineString();
				//ls.Add(ptBase);
				//////ls.Add(ARANFunctions.LocalToPrj(ptBase, OutDir + fSide * SpiralDivergenceAngle, -40000.0));
				////ls.Add(ARANFunctions.LocalToPrj(ptBase, OutDir + fSide * SplayAngle15, -40000.0));
				//ls.Add(ARANFunctions.LocalToPrj(ptBase, OutDir + fSide * splay, -40000.0));
				//_UI.DrawLineString(ls, -1, 1);
				//ProcessMessages();

				//double area = 0;
				////	int j = i < this.Count - 2 ? i + 1 : this.Count - 1;
				//area += (ptBase.X - ptCurr.X) * (ptBase.Y + ptCurr.Y);
				//area += (ptCurr.X - BasePoints[1].X) * (ptCurr.Y + BasePoints[1].Y);

				ARANFunctions.PrjToLocal(ptBase, OutDir + fSide * splay, ptCurr, out x, out y);
				//ARANFunctions.PrjToLocal(ptBase, OutDir + fSide * SpiralDivergenceAngle, ptCurr, out x, out y);
				j = 0;
				MaxDist = y * fSide;

				for (i++; i < n; i++)
				{
					//LineString ls = new LineString();
					//ls.Add(ARANFunctions.LocalToPrj(ptBase, OutDir + fSide * splay, 40000.0));
					//////ls.Add(ARANFunctions.LocalToPrj(ptBase, OutDir + fSide * SpiralDivergenceAngle, -40000.0));
					////ls.Add(ARANFunctions.LocalToPrj(ptBase, OutDir + fSide * SplayAngle15, -40000.0));
					//ls.Add(ARANFunctions.LocalToPrj(ptBase, OutDir + fSide * splay, -40000.0));
					//_UI.DrawLineString(ls);
					//ProcessMessages();

					//bpt[i] = BasePoints[i];
					ARANFunctions.PrjToLocal(ptBase, OutDir + fSide * splay, BasePoints[i], out x, out y);
					//ARANFunctions.PrjToLocal(ptBase, OutDir + fSide * SpiralDivergenceAngle, BasePoints[i], out x, out y);
					y *= fSide;
					if (y > MaxDist)
					{
						//LineString ls = new LineString();
						//ls.Add(ptBase);
						//ls.Add(ARANFunctions.LocalToPrj(ptBase, OutDir + fSide * splay, -40000.0));
						//_UI.DrawLineString(ls, -1, 1);
						//ProcessMessages();

						MaxDist = y;
						j = i;
						//ptCurr = BasePoints[i];
					}

					//_UI.DrawPointWithText(BasePoints[i], -1, "pt-" + (i + 1).ToString());
					//ProcessMessages();
				}

				//_UI.DrawPointWithText(ptBase, -1, "ptBase");
				//_UI.DrawPointWithText(ptCurr, -1, "pt-I");
				//ProcessMessages();

				//if (j < n - 1)
				result.Add(BasePoints[(j + 1) % n]);

				//_UI.DrawPointWithText(BasePoints[(j + 1) % n], "pt_0");
				//_UI.DrawPointWithText(BasePoints[j], "ptCurr_1");
				//ProcessMessages();

				ptCurr = BasePoints[j];
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

					fTmp = _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;                   // _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
					ptCurr = (Point)ARANFunctions.LineLineIntersect(ptCurr, OutDir + fSide * fTmp, ptBase, OutDir);

					//_UI.DrawPointWithText(ptCurr, "pt-IIb");
					//ProcessMessages();

					result.Add(ptCurr);
				}
				else if (y > 0)
				{
					fTmp = Math.Max(0.5 * TurnAng, _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value);

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

					if (ptCurr == null || ARANMath.SideDef(BasePoints[j], OutDir + ARANMath.C_PI_2, ptCurr)== SideDirection.sideLeft )
					{
						while (y > 0)
						{
							j = (j - 1) % n;
							if (j < 0)
								j += n;

							ARANFunctions.PrjToLocal(ptBase, OutDir, BasePoints[j], out x, out y);
							y *= fSide;
							if (y < 0)
							{
								//_UI.DrawPointWithText(ptCurr, "ptCurr");
								//ProcessMessages();
								ptCurr = ptBase;
								result.Add(ptCurr);
								break;
							}

							ptCurr = BasePoints[j];
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
					// SpiralDivergenceAngle + _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
					//_UI.DrawPointWithText(ptCurr, -1, "ptCurr");
					//ARANFunctions.PrjToLocal(_EndFIX.PrjPt, EntryDir + Math.PI, _StartFIX.PrjPt, out x, out y);
					//_UI.DrawPointWithText(ptBase, -1, "ptBase");

					ARANFunctions.PrjToLocal(_EndFIX.PrjPt, OutDir + Math.PI, _StartFIX.PrjPt, out x, out y);

					double fTurnDir = (int)_StartFIX.EffectiveTurnDirection;
					double TurnR = _StartFIX.ConstructTurnRadius;
					double l = TurnR - y * fTurnDir;

					fTmp = 0.5 * (l + TurnR) / TurnR;
					if (Math.Abs(fTmp) > 1.0 && Math.Abs(fTmp) - 1 < 1.03)
						fTmp = Math.Sign(fTmp);

					double alpha = Math.Acos(fTmp);
					//ptCurr = (Point)ARANFunctions.LineLineIntersect(ptCurr, OutDir + fSide * alpha, ptBase, OutDir);

					fTmp = Math.Max(Math.Max(0.5 * TurnAng, _constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value), alpha);
					ptCurr = (Point)ARANFunctions.LineLineIntersect(ptCurr, OutDir + fSide * fTmp, ptBase, OutDir);

					//_UI.DrawPointWithText(ptCurr, "pt-IIa");
					//ProcessMessages();

					result.Add(ptCurr);
				}
				//TurnAng

				//for (i = 0; i < n; i++)
				//	_UI.DrawPointWithText(BasePoints[i], -1, "pt_" + (i + 1).ToString());
				//ProcessMessages();
			}
			else
			{

				ptCurr = InnerBasePoint;

				//_UI.DrawPointWithText(ptTmp, -1, "ptCurr-0");
				//_UI.DrawPointWithText(ptCurr, -1, "ptCurr-1");
				//ProcessMessages();

				result.Add(ptTmp);
				result.Add(ptCurr);
			}

			CurDist = ARANFunctions.PointToLineDistance(ptCurr, wptTransitions[0].PrjPt, OutDir + 0.5 * ARANMath.C_PI);

			//_UI.DrawPointWithText(ptFrom, 0, "ptCurr-0");
			//ProcessMessages(true);

			//	_UI.DrawPointWithText(ptFrom, 0, "ptFrom-I1");
			//	ProcessMessages(true);
			//result.Add(ptCurr);


			if (CurDist < TransitionDist)
				TransitionDist = CurDist;

			//MaxDist = Math.Max(LPTYDist, TransitionDist);
			//if (CurDist > MaxDist)
			//if (false)

			if (_StartFIX.FlyMode != eFlyMode.Atheight)
			{
				//==============================================================================
				LegLenght = ARANMath.Hypot(wptTransitions[0].PrjPt.X - _StartFIX.PrjPt.X, wptTransitions[0].PrjPt.Y - _StartFIX.PrjPt.Y);
				//if (_StartFIX.Role == eFIXRole.FAF_ && _EndFIX.Role == eFIXRole.MAPt_)
				//{
				//	dPhi1 = Math.Atan2(ASW_IN0C - ASW_IN1, LegLenght);
				//	if (dPhi1 > DivergenceAngle30)
				//		dPhi1 = DivergenceAngle30;

				//	BaseDir = OutDir - dPhi1 * fSide;
				//	ptBase = ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir + fSide * 0.5 * ARANMath.C_PI, ASW_IN1);

				//}
				//else
				//{
					BaseDir = OutDir;
					//ptBase = ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir + fSide * 0.5 * ARANMath.C_PI, ASW_IN0C);
				//}

				//_UI.DrawPointWithText(_EndFIX.PrjPt, "_EndFIX");
				//_UI.DrawPointWithText(ptBase, "ptBase");
				//_UI.DrawPointWithText(InnerBasePoint, "InnerBasePoint");
				//_UI.DrawPointWithText(ptCurr, "ptCurr");
				//ProcessMessages();

				CurWidth = fSide * ARANFunctions.PointToLineDistance(ptCurr, ptBase, BaseDir);
				if (Math.Abs(CurWidth) > ARANMath.EpsilonDistance && TurnAng != 0)
				{
					if (CurWidth > 0)
						SplayAngle = EntryDir + 0.5 * TurnAng * fSide;
					else
					{
						//SplayAngle15 = GlobalVars.constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
						SplayAngle = OutDir + SplayAngle15 * fSide;
					}

					ptInter = (Point)ARANFunctions.LineLineIntersect(InnerBasePoint, SplayAngle, ptBase, BaseDir);

					//_UI.DrawPointWithText(ptInter, "ptInter - 2");
					//ProcessMessages();

					//double ptInterDist = ARANFunctions.PointToLineDistance(ptInter, wptTransitions[0].PrjPt, OutDir + 0.5 * ARANMath.C_PI);

					//if (ptInterDist < TransitionDist)
					//{
					//	Point ptCut = ARANFunctions.LocalToPrj(wptTransitions[0].PrjPt, OutDir + ARANMath.C_PI, TransitionDist, 0);

					//	ptInter = (Point)ARANFunctions.LineLineIntersect(InnerBasePoint, SplayAngle, ptCut, OutDir + 0.5 * ARANMath.C_PI);

					//	//_UI.DrawPointWithText(ptInter, 0, "ptInter - 2");
					//	//ProcessMessages();
					//}
					result.Add(ptInter);
				}
			}

			//Ring rr = new Ring();
			//rr.Assign(result);
			//_UI.DrawRing(rr, -1, AranEnvironment.Symbols.eFillStyle.sfsHorizontal);

			//_UI.DrawPointWithText(ptCut, -1, "ptCut");
			//_UI.DrawPointWithText(ptInter, -1, "ptInter");
			//ProcessMessages();

			JoinSegments(result, ARP, false, IsPrimary, EntryDir, TurnDir);
			return result;
		}
	}
}
