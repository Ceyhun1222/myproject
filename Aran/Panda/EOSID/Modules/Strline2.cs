namespace EOSID
{
	[System.Runtime.InteropServices.ComVisible(false)]
	internal static class Functions
	{
		internal static void CreateSideLine(ref TrackLeg segment, ref IPointCollection ptColl, bool bestCase, int Side)
		{
			PointData StartFIX, EndFIX;
			LegData data;

			if (bestCase)
			{
				data = segment.BestCase;
				StartFIX = segment.ptStart.BestCase;
				EndFIX = segment.ptEnd.BestCase;
			}
			else
			{
				data = segment.WorstCase;
				StartFIX = segment.ptStart.WorstCase;
				EndFIX = segment.ptEnd.WorstCase;
			}

			#region  Head

			double splayRate = GlobalVars.SplayRate, currLength = 0.0,
				currWidth = StartFIX.Width, currDir = StartFIX.Direction,
				endWidth = segment.PlannedEndWidth;

			if (segment.SegmentCode == eLegType.arcPath)
			{
				if (currWidth < GlobalVars.ArcProtectWidth)
					currWidth = GlobalVars.ArcProtectWidth;

				if (endWidth < GlobalVars.ArcProtectWidth)
					endWidth = GlobalVars.ArcProtectWidth;
			}

			//Graphics.DrawPolyline(segment.BestCase.pNominalPoly, -1);
			//ProcessMessages();

			double splayLen = (endWidth - currWidth) / splayRate;
			double toSplay = splayLen;

			if (currWidth >= endWidth)
				splayRate = 0.0;

			IPoint pPtRefer = StartFIX.pPoint;
			IPoint pPtCurr = LocalToPrj(pPtRefer, currDir, 0.0, Side * currWidth);

			ptColl.AddPoint(pPtCurr);

			//Graphics.DrawPointWithText(pPtCurr,"c0");
			//ProcessMessages();

			#endregion

			#region  Turns

			double straigtLen;

			for (int iTurn = 0; iTurn < data.turns; iTurn++)
			{
				straigtLen = ReturnDistanceInMeters(pPtRefer, data.Turn[iTurn].ptStart);

				if (straigtLen > GlobalVars.distEps && currWidth < endWidth)	//???????????????????
				{
					if (toSplay < straigtLen)
					{
						currWidth = endWidth;
						splayRate = 0.0;
						if (toSplay > GlobalVars.distEps)
						{
							pPtCurr = LocalToPrj(pPtRefer, currDir, toSplay, Side * currWidth);
							ptColl.AddPoint(pPtCurr);
						}
					}
					else
						currWidth += straigtLen * splayRate;
				}

				//if (straigtLen > 0.0)
				//{
				//pPtCurr = LocalToPrj(pPtRefer, currDir, straigtLen, Side * currWidth);
				//	ptColl.AddPoint(pPtCurr);
				//Graphics.DrawPointWithText(pPtCurr,"c1");
				//Graphics.DrawPointWithText(pPtRefer, "pPtRefer");
				//ProcessMessages();
				//}

				currLength += straigtLen;
				toSplay -= currLength;

				double turnAngle = data.Turn[iTurn].Angle;
				double arcLen = data.Turn[iTurn].Radius * DegToRad(turnAngle);

				if (arcLen > GlobalVars.distEps)
				{
					if (currWidth < endWidth)
					{
						if (toSplay < arcLen)
						{
							turnAngle = RadToDeg(toSplay / data.Turn[iTurn].Radius);
							arcLen = GlobalVars.DegToRadValue * turnAngle * data.Turn[iTurn].Radius;
						}

						int n = (int)turnAngle;

						if (n < 1) n = 1;
						else if (n < 5) n = 5;
						else if (n < 10) n = 10;

						double angStep = turnAngle / n;
						IPoint pPtCenter = data.Turn[iTurn].ptCenter;

						pPtCurr = new Point();

						for (int i = 0; i < n; i++)
						{
							double angleInRad = GlobalVars.DegToRadValue * (currDir + (90.0 - i * angStep) * data.Turn[iTurn].TurnDir);
							double currRadius = data.Turn[iTurn].Radius + currWidth * Side * data.Turn[iTurn].TurnDir;

							pPtCurr.X = pPtCenter.X + currRadius * System.Math.Cos(angleInRad);
							pPtCurr.Y = pPtCenter.Y + currRadius * System.Math.Sin(angleInRad);
							ptColl.AddPoint(pPtCurr);

							currWidth += data.Turn[iTurn].Radius * GlobalVars.DegToRadValue * angStep * splayRate;
						}

						toSplay -= arcLen;
						currLength += arcLen;
						currDir -= turnAngle * data.Turn[iTurn].TurnDir;

						turnAngle = data.Turn[iTurn].Angle - turnAngle;
						arcLen = data.Turn[iTurn].Radius * DegToRad(turnAngle);
					}

					if (arcLen > 0.0)
					{
						int n = (int)turnAngle;

						if (n < 1) n = 1;
						else if (n < 5) n = 5;
						else if (n < 10) n = 10;

						double angStep = turnAngle / n;

						IPoint pPtCenter = data.Turn[iTurn].ptCenter;

						//Graphics.DrawPointWithText(data.Turn[iTurn].ptStart, "Start-" + iTurn.ToString());
						//Graphics.DrawPointWithText(data.Turn[iTurn].ptEnd, "End-" + iTurn.ToString());
						//Graphics.DrawPointWithText(pPtCenter, "Center-" + iTurn.ToString());
						//ProcessMessages();

						pPtCurr = new Point();

						for (int i = 0; i < n; i++)
						{
							double angleInRad = GlobalVars.DegToRadValue * (currDir + (90.0 - i * angStep) * data.Turn[iTurn].TurnDir);
							double currRadius = data.Turn[iTurn].Radius + currWidth * Side * data.Turn[iTurn].TurnDir;

							pPtCurr.X = pPtCenter.X + currRadius * System.Math.Cos(angleInRad);
							pPtCurr.Y = pPtCenter.Y + currRadius * System.Math.Sin(angleInRad);

							//Graphics.DrawPointWithText(pPtCurr, "Curr");
							//ProcessMessages();

							ptColl.AddPoint(pPtCurr);
						}

						currDir -= turnAngle * data.Turn[iTurn].TurnDir;
						currLength += arcLen;
					}
				}

				pPtRefer = data.Turn[iTurn].ptEnd;
				pPtCurr = LocalToPrj(pPtRefer, currDir, 0.0, Side * currWidth);
				ptColl.AddPoint(pPtCurr);

				//Graphics.DrawPolyline(ptColl, RGB(0, 0, 255), 2);
				//Graphics.DrawPolyline(ptColl, 0, 1);
				//ProcessMessages();
			}
			#endregion

			#region Trail

			straigtLen = ReturnDistanceInMeters(pPtRefer, EndFIX.pPoint);

			if (straigtLen > GlobalVars.distEps)	// && currWidth < endWidth)
			{
				if (toSplay < straigtLen)
				{
					currWidth = endWidth;
					splayRate = 0.0;
					if (toSplay > GlobalVars.distEps)
					{
						if (toSplay > 0.5 * straigtLen)
							pPtCurr = LocalToPrj(EndFIX.pPoint, currDir, straigtLen - toSplay, Side * currWidth);
						else
							pPtCurr = LocalToPrj(pPtRefer, currDir, toSplay, Side * currWidth);
						//pPtCurr = LocalToPrj(pPtRefer, currDir, splayLen, Side * currWidth);

						ptColl.AddPoint(pPtCurr);
					}
				}
				else
					currWidth += straigtLen * splayRate;

				//pPtCurr = LocalToPrj(pPtRefer, currDir, straigtLen, Side * currWidth);
				pPtCurr = LocalToPrj(EndFIX.pPoint, currDir, 0.0, Side * currWidth);
				ptColl.AddPoint(pPtCurr);
			}

			if (segment.SegmentCode == eLegType.directToFIX)
			{
				pPtCurr = LocalToPrj(pPtRefer, currDir, straigtLen, Side * currWidth);
				ptColl.AddPoint(pPtCurr);
			}

			#endregion

			if (bestCase)
				segment.ptEnd.BestCase.Width = currWidth;
			else
				segment.ptEnd.WorstCase.Width = currWidth;
		}

		internal static void CreateSideLine(LegData data, PointData StartFIX, PointData EndFIX, double PlannedEndWidth, ref IPointCollection ptColl, out double currWidth, int Side)
		{

	
			
			
			
			
			
			
			
			
			
			
			
			
			
			
			#region  Head

			double splayRate = GlobalVars.SplayRate, currLength = 0.0,
				currDir = StartFIX.Direction,
				endWidth = PlannedEndWidth;
			currWidth = StartFIX.Width;
			if (segment.SegmentCode == eLegType.arcPath)
			{
				if (currWidth < GlobalVars.ArcProtectWidth)
					currWidth = GlobalVars.ArcProtectWidth;

				if (endWidth < GlobalVars.ArcProtectWidth)
					endWidth = GlobalVars.ArcProtectWidth;
			}

			//Graphics.DrawPolyline(segment.BestCase.pNominalPoly, -1);
			//ProcessMessages();

			double splayLen = (endWidth - currWidth) / splayRate;
			double toSplay = splayLen;

			if (currWidth >= endWidth)
				splayRate = 0.0;

			IPoint pPtRefer = StartFIX.pPoint;
			IPoint pPtCurr = LocalToPrj(pPtRefer, currDir, 0.0, Side * currWidth);

			ptColl.AddPoint(pPtCurr);

			//Graphics.DrawPointWithText(pPtCurr,"c0");
			//ProcessMessages();
			#endregion

			#region  Turns

			double straigtLen;

			for (int iTurn = 0; iTurn < data.turns; iTurn++)
			{
				straigtLen = ReturnDistanceInMeters(pPtRefer, data.Turn[iTurn].ptStart);

				if (straigtLen > GlobalVars.distEps && currWidth < endWidth)	//???????????????????
				{
					if (toSplay < straigtLen)
					{
						currWidth = endWidth;
						splayRate = 0.0;
						if (toSplay > GlobalVars.distEps)
						{
							pPtCurr = LocalToPrj(pPtRefer, currDir, toSplay, Side * currWidth);
							ptColl.AddPoint(pPtCurr);
						}
					}
					else
						currWidth += straigtLen * splayRate;
				}

				//if (straigtLen > 0.0)
				//{
				//pPtCurr = LocalToPrj(pPtRefer, currDir, straigtLen, Side * currWidth);
				//	ptColl.AddPoint(pPtCurr);
				//Graphics.DrawPointWithText(pPtCurr,"c1");
				//Graphics.DrawPointWithText(pPtRefer, "pPtRefer");
				//ProcessMessages();
				//}

				currLength += straigtLen;
				toSplay -= currLength;

				double turnAngle = data.Turn[iTurn].Angle;
				double arcLen = data.Turn[iTurn].Radius * DegToRad(turnAngle);

				if (arcLen > GlobalVars.distEps)
				{
					if (currWidth < endWidth)
					{
						if (toSplay < arcLen)
						{
							turnAngle = RadToDeg(toSplay / data.Turn[iTurn].Radius);
							arcLen = GlobalVars.DegToRadValue * turnAngle * data.Turn[iTurn].Radius;
						}

						int n = (int)turnAngle;

						if (n < 1) n = 1;
						else if (n < 5) n = 5;
						else if (n < 10) n = 10;

						double angStep = turnAngle / n;
						IPoint pPtCenter = data.Turn[iTurn].ptCenter;

						pPtCurr = new Point();

						for (int i = 0; i < n; i++)
						{
							double angleInRad = GlobalVars.DegToRadValue * (currDir + (90.0 - i * angStep) * data.Turn[iTurn].TurnDir);
							double currRadius = data.Turn[iTurn].Radius + currWidth * Side * data.Turn[iTurn].TurnDir;

							pPtCurr.X = pPtCenter.X + currRadius * System.Math.Cos(angleInRad);
							pPtCurr.Y = pPtCenter.Y + currRadius * System.Math.Sin(angleInRad);
							ptColl.AddPoint(pPtCurr);

							currWidth += data.Turn[iTurn].Radius * GlobalVars.DegToRadValue * angStep * splayRate;
						}

						toSplay -= arcLen;
						currLength += arcLen;
						currDir -= turnAngle * data.Turn[iTurn].TurnDir;

						turnAngle = data.Turn[iTurn].Angle - turnAngle;
						arcLen = data.Turn[iTurn].Radius * DegToRad(turnAngle);
					}

					if (arcLen > 0.0)
					{
						int n = (int)turnAngle;

						if (n < 1) n = 1;
						else if (n < 5) n = 5;
						else if (n < 10) n = 10;

						double angStep = turnAngle / n;

						IPoint pPtCenter = data.Turn[iTurn].ptCenter;

						//Graphics.DrawPointWithText(data.Turn[iTurn].ptStart, "Start-" + iTurn.ToString());
						//Graphics.DrawPointWithText(data.Turn[iTurn].ptEnd, "End-" + iTurn.ToString());
						//Graphics.DrawPointWithText(pPtCenter, "Center-" + iTurn.ToString());
						//ProcessMessages();

						pPtCurr = new Point();

						for (int i = 0; i < n; i++)
						{
							double angleInRad = GlobalVars.DegToRadValue * (currDir + (90.0 - i * angStep) * data.Turn[iTurn].TurnDir);
							double currRadius = data.Turn[iTurn].Radius + currWidth * Side * data.Turn[iTurn].TurnDir;

							pPtCurr.X = pPtCenter.X + currRadius * System.Math.Cos(angleInRad);
							pPtCurr.Y = pPtCenter.Y + currRadius * System.Math.Sin(angleInRad);

							//Graphics.DrawPointWithText(pPtCurr, "Curr");
							//ProcessMessages();

							ptColl.AddPoint(pPtCurr);
						}

						currDir -= turnAngle * data.Turn[iTurn].TurnDir;
						currLength += arcLen;
					}
				}

				pPtRefer = data.Turn[iTurn].ptEnd;
				pPtCurr = LocalToPrj(pPtRefer, currDir, 0.0, Side * currWidth);
				ptColl.AddPoint(pPtCurr);

				//Graphics.DrawPolyline(ptColl, RGB(0, 0, 255), 2);
				//Graphics.DrawPolyline(ptColl, 0, 1);
				//ProcessMessages();
				//toSplay = splayLen - currLength;
			}
			#endregion

			#region Trail

			straigtLen = ReturnDistanceInMeters(pPtRefer, EndFIX.pPoint);

			if (straigtLen > GlobalVars.distEps)	// && currWidth < endWidth
			{
				if (toSplay < straigtLen)
				{
					currWidth = endWidth;
					splayRate = 0.0;

					if (toSplay > GlobalVars.distEps)
					{
						if (toSplay > 0.5 * straigtLen)
							pPtCurr = LocalToPrj(EndFIX.pPoint, currDir, straigtLen - toSplay, Side * currWidth);
						else
							pPtCurr = LocalToPrj(pPtRefer, currDir, toSplay, Side * currWidth);


						ptColl.AddPoint(pPtCurr);
					}
				}
				else
					currWidth += straigtLen * splayRate;


				pPtCurr = LocalToPrj(EndFIX.pPoint, currDir, 0.0, Side * currWidth);
				ptColl.AddPoint(pPtCurr);
			}


			#endregion
		}
	}
}