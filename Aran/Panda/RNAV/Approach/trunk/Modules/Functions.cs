using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using System.Runtime.InteropServices;

namespace Aran.PANDA.RNAV.Approach
{
	[System.Runtime.InteropServices.ComVisible(false)]
	static class Functions
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool InsertMenu(IntPtr hMenu, int uPosition, int uFlags, int uIDNewItem, string lpNewItem);

		public static DateTime RetrieveLinkerTimestamp()
		{
			const int c_PeHeaderOffset = 60;
			const int c_LinkerTimestampOffset = 8;

			byte[] b = new byte[2048];
			System.IO.Stream s = null;

			try
			{
				string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
				s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
				s.Read(b, 0, 2048);
			}
			finally
			{
				if (s != null)
					s.Close();
			}

			int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
			int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);

			DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

			dt = dt.AddSeconds(secondsSince1970);
			dt = dt.ToLocalTime();
			return dt;
		}

		public static double CalcMaxRadius()
		{
			int i, n = GlobalVars.RWYList.Length;
			if (n <= 1)
				return 0;

			Point ptCentr;
			List<Point> pLineStr = new List<Point>();

			for (i = 0; i < n; i++)
			{
				ptCentr = GlobalVars.RWYList[i].pPtPrj[eRWY.ptTHR];
				pLineStr.Add(ptCentr);
			}

			if (pLineStr.Count < 3)
				return ARANMath.Hypot(pLineStr[0].X - pLineStr[1].X, pLineStr[0].Y - pLineStr[1].Y);

			double result;
			minCircle.minCircleAroundPoints(pLineStr, out ptCentr, out result);

			return 2 * result;
		}

		//public static void GetObstaclesByPolygon(ObstacleType[] LocalObstacles, out ObstacleType[] ObstacleList, MultiPolygon pPoly)
		//{
		//	int n = LocalObstacles.Length;
		//	ObstacleList = new ObstacleType[n];
		//	if (n == 0)
		//		return;

		//	//GeometryOperators pGeoOp = new GeometryOperators();

		//	int j = -1;
		//	for (int i = 0; i < n; i++)
		//	{
		//		Point ptCurr = (Point)LocalObstacles[i].pPrjGeometry;
		//		//if (pGeoOp.Contains(pPoly, ptCurr))
		//		if (pPoly.IsPointInside(ptCurr))
		//			ObstacleList[++j] = LocalObstacles[i];
		//	}

		//	System.Array.Resize<ObstacleType>(ref ObstacleList, j + 1);
		//}

		private static void SortByDist(ObstacleData[] obstacleArray, int iLo, int iHi)
		{
			int Lo = iLo;
			int Hi = iHi;
			double midDist = obstacleArray[(Lo + Hi) / 2].Dist;

			do
			{
				while (obstacleArray[Lo].Dist < midDist)
					Lo++;

				while (obstacleArray[Hi].Dist > midDist)
					Hi--;

				if (Lo <= Hi)
				{
					ObstacleData t = obstacleArray[Lo];
					obstacleArray[Lo] = obstacleArray[Hi];
					obstacleArray[Hi] = t;
					Lo++;
					Hi--;
				}
			}
			while (Lo <= Hi);

			if (Hi > iLo)
				SortByDist(obstacleArray, iLo, Hi);

			if (Lo < iHi)
				SortByDist(obstacleArray, Lo, iHi);
		}

		private static void SortByReqH(ObstacleData[] obstacleArray, int iLo, int iHi)
		{
			int Lo = iLo;
			int Hi = iHi;
			double MidReqH = obstacleArray[(Lo + Hi) / 2].ReqH;

			do
			{
				while (obstacleArray[Lo].ReqH > MidReqH)
					Lo++;

				while (obstacleArray[Hi].ReqH < MidReqH)
					Hi--;

				if (Lo <= Hi)
				{
					ObstacleData t = obstacleArray[Lo];
					obstacleArray[Lo] = obstacleArray[Hi];
					obstacleArray[Hi] = t;
					Lo++;
					Hi--;
				}
			}
			while (Lo <= Hi);

			if (Hi > iLo)
				SortByReqH(obstacleArray, iLo, Hi);

			if (Lo < iHi)
				SortByReqH(obstacleArray, Lo, iHi);
		}

		public static void Sort(ObstacleContainer A, int SortIx)
		{
			int Lo = A.Parts.GetLowerBound(0), Hi = A.Parts.GetUpperBound(0);

			if (Lo >= Hi) return;

			switch (SortIx)
			{
				case 0:
					SortByDist(A.Parts, Lo, Hi);
					break;

				//case 1:
				//	SortByTurnDist(A.Parts, Lo, Hi);
				//	break;

				case 2:
					SortByReqH(A.Parts, Lo, Hi);
					break;

				//case 3:
				//	SortByReqOCH(A.Parts, Lo, Hi);
				//	break;

				//case 4:
				//	SortByEffectiveHeight(A.Parts, Lo, Hi);
				//	break;

				//case 5:
				//	SortByHPenetrate(A.Parts, Lo, Hi);
				//	break;

				//case 4:
				//	SortByfTmp(A.Parts, Lo, Hi)
				//	break;

				//case 6:
				//	SortByMoc(A.Parts, Lo, Hi);
				//	break;

				//case 100:
				//	SortByfSort(A.Parts, Lo, Hi);
				//	break;

				//case 101:
				//	SortBysSort(A.Parts, Lo, Hi);
				//	break;
			}
		}

		public static bool PriorPostFixTolerance(MultiPolygon pPolygon, Point pPtPrj, double fDir, out double PriorDist, out double PostDist)
		{
			PriorDist = -1.0;
			PostDist = -1.0;
			MultiLineString ptIntersection;

			LineString pCutterPolyline = new LineString();
			pCutterPolyline.Add(ARANFunctions.LocalToPrj(pPtPrj, fDir, 1000000.0, 0));
			pCutterPolyline.Add(ARANFunctions.LocalToPrj(pPtPrj, fDir, -1000000.0, 0));

			try
			{
				GeometryOperators pTopological = new GeometryOperators();
				Geometry pIntersection = pTopological.Intersect(pCutterPolyline, pPolygon);
				if (pIntersection.IsEmpty)
					return false;
				ptIntersection = (MultiLineString)pIntersection;
			}
			catch
			{
				return false;
			}

			Point ptDist = ARANFunctions.PrjToLocal(pPtPrj, fDir, ptIntersection[0][0]);

			double fMinDist = ptDist.X;
			double fMaxDist = ptDist.X;
			int n = ptIntersection.Count;

			for (int j = 0; j < n; j++)
			{
				LineString ls = ptIntersection[j];
				int m = ls.Count;

				for (int i = 0; i < m; i++)
				{
					ptDist = ARANFunctions.PrjToLocal(pPtPrj, fDir, ls[i]);

					if (ptDist.X < fMinDist) fMinDist = ptDist.X;
					if (ptDist.X > fMaxDist) fMaxDist = ptDist.X;
				}
			}
			PriorDist = fMinDist;
			PostDist = fMaxDist;

			return true;
		}

		public static double MaxObstacleHeightInPoly(ObstacleContainer ObstSrcList, out ObstacleContainer OutObstList, MultiPolygon pPoly, out int Index)
		{
			int n = ObstSrcList.Obstacles.Length;
			OutObstList.Obstacles = new Obstacle[n];
			OutObstList.Parts = new ObstacleData[0];

			Index = -1;
			if (pPoly.IsEmpty || n < 0)
				return 0.0;

			GeometryOperators pProxiOperator = new GeometryOperators
			{
				CurrentGeometry = pPoly
			};

			double MaxHeight = double.MinValue;
			int j = 0;

			for (int i = 0; i < n; i++)
			{
				if (!pProxiOperator.Disjoint(ObstSrcList.Obstacles[i].pGeomPrj))
				{
					OutObstList.Obstacles[j] = ObstSrcList.Obstacles[i];

					if (OutObstList.Obstacles[j].Height > MaxHeight)
					{
						Index = j;
						MaxHeight = OutObstList.Obstacles[j].Height;
					}

					j++;
				}
			}

			if (j > 0)
				Array.Resize<Obstacle>(ref OutObstList.Obstacles, j);
			else
				Array.Resize<Obstacle>(ref OutObstList.Obstacles, 0);

			return MaxHeight;
		}

		public static void GetObstaclesByDistance(out ObstacleContainer obstList, Point ptCenter, double radius)
		{
			int n = GlobalVars.ObstacleList.Obstacles.Length;
			obstList.Obstacles = new Obstacle[n];
			obstList.Parts = new ObstacleData[0];
			if (n == 0)
				return;

			if (radius == 0)
			{
				obstList.Obstacles = new Obstacle[0];
				return;
			}

			int j = 0;
			GeometryOperators pTopooper = new GeometryOperators();
			pTopooper.CurrentGeometry = ptCenter;

			foreach (var obst in GlobalVars.ObstacleList.Obstacles)
			{
				double dist = pTopooper.GetDistance(obst.pGeomPrj);
				if (dist <= radius)
					obstList.Obstacles[j++] = obst;
			}

			System.Array.Resize<Obstacle>(ref obstList.Obstacles, j);
		}

		static int PointComparer(Point p0, Point p1)
		{
			if (p0.X > p1.X) return 1;
			if (p0.X < p1.X) return -1;

			if (p0.Y > p1.Y) return 1;
			if (p0.Y < p1.Y) return -1;

			return 0;
		}

		static int AddMultiPoint(this List<Point> ptList, MultiPoint multiPoint)
		{
			foreach (Point pt in multiPoint)
				ptList.Add(pt);

			return ptList.Count;
		}

		static void RemoveSeamPoints(ref List<Point> pPoints)
		{
			const double eps2 = ARANMath.EpsilonDistance * ARANMath.EpsilonDistance;

			int n = pPoints.Count;
			int j = 0;

			pPoints.Sort(PointComparer);

			while (j < n - 1)
			{
				Point pCurrPt = pPoints[j];
				int i = j + 1;

				while (i < n)
				{
					double dx = pCurrPt.X - pPoints[i].X;
					double dy = pCurrPt.Y - pPoints[i].Y;
					double fDist2 = dx * dx + dy * dy;

					if (fDist2 >= eps2)
						break;

					pPoints.RemoveAt(i);
					n--;
				}

				j++;
			}
		}

		static public ObstacleContainer GetLegProtectionAreaObstacles(LegBase currLeg, ObstacleContainer inObstacleList, double MOCLimit, double refElev)
		{
			ObstacleContainer OutObstList;
			int m = inObstacleList.Obstacles.Length;

			OutObstList.Obstacles = new Obstacle[m];
			OutObstList.Parts = new ObstacleData[10 * m];

			//int maxReqHil = maxReqHi = -1;
			//LegBase.ProcessMessages(true);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(currLeg.FullAssesmentArea, AranEnvironment.Symbols.eFillStyle.sfsCross);
			//LegBase.ProcessMessages();

			if (m == 0)
				return OutObstList;

			GeometryOperators fullGeoOp = new GeometryOperators
			{
				CurrentGeometry = currLeg.FullAssesmentArea
			};

			GeometryOperators primaryGeoOp = new GeometryOperators
			{
				CurrentGeometry = currLeg.PrimaryAssesmentArea
			};

			MultiPolygon pSecondaryPoly = (MultiPolygon)primaryGeoOp.Difference(currLeg.FullAssesmentArea);

			GeometryOperators secondGeoOp = null;

			if (pSecondaryPoly != null)
				secondGeoOp = new GeometryOperators
				{
					CurrentGeometry = pSecondaryPoly
				};

			GeometryOperators lineStrGeoOp = new GeometryOperators
			{
				CurrentGeometry = currLeg.FullProtectionAreaOutline()   // ARANFunctions.PolygonToPolyLine(currLeg.FullAssesmentArea[0])
			};

			//LegBase.ProcessMessages(true);
			//GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)lineStrGeoOp.CurrentGeometry, 2);
			//LegBase.ProcessMessages();

			GeometryOperators NNLineGeoOp = new GeometryOperators
			{
				CurrentGeometry = currLeg.NNSecondLine
			};

			//double LowerMOCLimit = GlobalVars.constants.Pansops[ePANSOPSData.dpObsClr].Value;
			//double maxAlt = double.MinValue;

			int c = 10 * m;
			int k = 0;
			int l = 0;

			List<Point> pObstPoints = new List<Point>();
			LineString pPart = new LineString();

			//MultiPolygon pTmpPoly = (MultiPolygon)fullGeoOp.Difference(CurrLeg.FullAssesmentArea, CurrLeg.PrimaryAssesmentArea);
			//double testTime = 0;
			//DateTime start = DateTime.Now;

			for (int i = 0; i < m; i++)
			//Parallel.For(0, m, delegate (int i)
			{
				Geometry pCurrGeom = inObstacleList.Obstacles[i].pGeomPrj;

				if (pCurrGeom.IsEmpty)
					goto conTinue;

				double distToFullPoly = fullGeoOp.GetDistance(pCurrGeom);
				if (distToFullPoly > inObstacleList.Obstacles[i].HorAccuracy)
					goto conTinue;

				pObstPoints.Clear();

				if (pCurrGeom.Type == GeometryType.Point)
					pObstPoints.Add((Point)pCurrGeom);
				else
				{
					Geometry pTmpPoints = primaryGeoOp.Intersect(pCurrGeom);
					pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());

					if (secondGeoOp != null)
					{
						pTmpPoints = secondGeoOp.Intersect(pCurrGeom);
						pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());
					}

					//pTmpPoints = fullGeoOp.Intersect(pCurrGeom);
					//pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());

					//DateTime start = DateTime.Now;

					RemoveSeamPoints(ref pObstPoints);

					//DateTime end = DateTime.Now;
					//TimeSpan span = end - start;
					//testTime += span.TotalMilliseconds;
				}

				int n = pObstPoints.Count;
				if (n <= 0)
					goto conTinue;

				OutObstList.Obstacles[l] = inObstacleList.Obstacles[i];
				OutObstList.Obstacles[l].PartsNum = n;
				//OutObstList.Obstacles[l].Parts = new int[n];

				for (int j = 0; j < n; j++)
				//Parallel.For(0, n, delegate (int j)
				{
					Point pCurrPt = pObstPoints[j];

					if (k >= c)
					{
						c += m;
						Array.Resize<ObstacleData>(ref OutObstList.Parts, c);
					}

					//OutObstList.Obstacles[l].Parts[j] = k;
					OutObstList.Parts[k].pPtPrj = pCurrPt;
					OutObstList.Parts[k].Owner = l;
					OutObstList.Parts[k].Index = j;

					OutObstList.Parts[k].Elev = OutObstList.Obstacles[l].Height;
					OutObstList.Parts[k].Height = OutObstList.Parts[k].Elev - refElev;

					double distToPrimaryPoly = primaryGeoOp.GetDistance(pCurrPt);
					OutObstList.Parts[k].Prima = distToPrimaryPoly <= OutObstList.Obstacles[l].HorAccuracy;

					OutObstList.Parts[k].fSecCoeff = 1.0;
					OutObstList.Parts[k].Flags = 0;

					if (!OutObstList.Parts[k].Prima)
					{
						double d1 = lineStrGeoOp.GetDistance(pCurrPt);

						double d = distToPrimaryPoly + d1;

						OutObstList.Parts[k].fSecCoeff = d1 / d;
						OutObstList.Parts[k].Flags = 1;

						if (OutObstList.Parts[k].fSecCoeff > 1.0)
						{
							OutObstList.Parts[k].Prima = true;
							OutObstList.Parts[k].fSecCoeff = 1.0;
							OutObstList.Parts[k].Flags = 0;
						}
					}

					ARANFunctions.PrjToLocal(currLeg.EndFIX.PrjPt, currLeg.StartFIX.OutDirection + Math.PI, pCurrPt, out OutObstList.Parts[k].Dist, out OutObstList.Parts[k].CLShift);

					OutObstList.Parts[k].MOC = MOCLimit * OutObstList.Parts[k].fSecCoeff;
					OutObstList.Parts[k].ReqH = OutObstList.Parts[k].MOC + OutObstList.Parts[k].Height;

					// + ObstacleList.Obstacles[l].VertAccuracy;
					//OutObstList.Parts[k].PDG = (OutObstList.Parts[k].ReqH - GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpH_abv_DER].Value) / OutObstList.Parts[k].DistStar;

					OutObstList.Parts[k].Ignored = false;

					k++;
				}//);
				l++;
				conTinue:;
			}//);

			//DateTime end = DateTime.Now;
			//TimeSpan span = end - start;
			//System.Windows.Forms.MessageBox.Show("Total time " + span.TotalMilliseconds);
			//System.Windows.Forms.MessageBox.Show("Total time " + testTime);

			Array.Resize<Obstacle>(ref OutObstList.Obstacles, l);
			Array.Resize<ObstacleData>(ref OutObstList.Parts, k);

			return OutObstList;
		}

		static public ObstacleContainer GetLegAreaObstacles(out ObstacleContainer OutObstList, LegBase CurrLeg, double distFromPrevs, double refElev, double MOCLimit, double MAClimb, WayPoint PtPrevFIX = null)
		{
			ObstacleContainer result;
			int m = GlobalVars.ObstacleList.Obstacles.Length;

			result.Obstacles = new Obstacle[0];
			result.Parts = new ObstacleData[0];

			if (m == 0)
			{
				OutObstList.Obstacles = new Obstacle[0];
				OutObstList.Parts = new ObstacleData[0];
				return result;
			}

			GeometryOperators fullGeoOp = new GeometryOperators
			{
				CurrentGeometry = CurrLeg.FullAssesmentArea
			};

			GeometryOperators primaryGeoOp = new GeometryOperators
			{
				CurrentGeometry = CurrLeg.PrimaryAssesmentArea
			};

			MultiPolygon pSecondaryPoly = (MultiPolygon)primaryGeoOp.Difference(CurrLeg.FullAssesmentArea);

			GeometryOperators secondGeoOp = new GeometryOperators
			{
				CurrentGeometry = pSecondaryPoly
			};

			GeometryOperators lineStrGeoOp = new GeometryOperators
			{
				CurrentGeometry = ARANFunctions.PolygonToPolyLine(CurrLeg.FullAssesmentArea[0])
			};

			GeometryOperators NNLineGeoOp = new GeometryOperators
			{
				CurrentGeometry = CurrLeg.KKLine
			};

			double LowerMOCLimit = GlobalVars.constants.Pansops[ePANSOPSData.dpObsClr].Value;

			int c = 10 * m;
			int k = 0;
			int l = 0;

			OutObstList.Obstacles = new Obstacle[m];
			OutObstList.Parts = new ObstacleData[c];

			MultiPoint pObstPoints = new MultiPoint();
			LineString pPart = new LineString();

			double maxO = double.MinValue;
			int resultObst = -1;
			int resultPart = -1;

			for (int i = 0; i < m; i++)
			{
				Geometry pCurrGeom = GlobalVars.ObstacleList.Obstacles[i].pGeomPrj;

				if (pCurrGeom.IsEmpty)
					continue;

				double distToFullPoly = fullGeoOp.GetDistance(pCurrGeom);
				if (distToFullPoly > GlobalVars.ObstacleList.Obstacles[i].HorAccuracy)
					continue;

				pObstPoints.Clear();

				if (pCurrGeom.Type == GeometryType.Point)
					pObstPoints.Add((Point)pCurrGeom);
				else
				{
					Geometry pTmpPoints = primaryGeoOp.Intersect(pCurrGeom);
					MultiPoint polyPoints = pTmpPoints.ToMultiPoint();
					Point ptTmp = GetNearestPoint(CurrLeg.NNSecondLine, polyPoints);

					if (ptTmp != null)
						pObstPoints.Add(ptTmp);

					pTmpPoints = secondGeoOp.Intersect(pCurrGeom);

					polyPoints = pTmpPoints.ToMultiPoint();
					ptTmp = GetNearestPoint(CurrLeg.NNSecondLine, polyPoints);

					if (ptTmp != null)
						pObstPoints.Add(ptTmp);

					//Geometry pTmpPoints = fullGeoOp.Intersect(pCurrGeom);
					//pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());

					//pTmpPoints = primaryGeoOp.Intersect(pCurrGeom);
					//pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());
					//RemoveSeamPoints(ref pObstPoints);
				}

				int n = pObstPoints.Count;
				if (n <= 0)
					continue;

				OutObstList.Obstacles[l] = GlobalVars.ObstacleList.Obstacles[i];

				for (int j = 0; j < n; j++)
				{
					Point pCurrPt = pObstPoints[j];

					if (k >= c)
					{
						c += m;
						Array.Resize<ObstacleData>(ref OutObstList.Parts, c);
					}

					OutObstList.Parts[k].pPtPrj = pCurrPt;
					OutObstList.Parts[k].Owner = l;
					OutObstList.Parts[k].Index = j;

					OutObstList.Parts[k].Elev = OutObstList.Obstacles[l].Height;
					OutObstList.Parts[k].Height = OutObstList.Parts[k].Elev - refElev;
					OutObstList.Parts[k].d0 = NNLineGeoOp.GetDistance(pCurrPt);

					//GlobalVars.gAranGraphics.DrawLineString(CurrLeg.NNSecondLine, 2, 255);
					//int pp = 0;
					//foreach(Point pt in CurrLeg.NNSecondLine)
					//	GlobalVars.gAranGraphics.DrawPointWithText(pt, "pt-" + pp++);
					//LegBase.ProcessMessages(true);

					//GlobalVars.gAranGraphics.DrawPointWithText(pCurrPt, "O-" + i);
					//LegBase.ProcessMessages();

					double distToPrimaryPoly = primaryGeoOp.GetDistance(pCurrPt);
					OutObstList.Parts[k].Prima = distToPrimaryPoly <= OutObstList.Obstacles[l].HorAccuracy;

					OutObstList.Parts[k].fSecCoeff = 1.0;
					OutObstList.Parts[k].Flags = 0;

					if (!OutObstList.Parts[k].Prima)
					{
						double d1 = lineStrGeoOp.GetDistance(pCurrPt);
						double d = distToPrimaryPoly + d1;

						OutObstList.Parts[k].fSecCoeff = d1 / d;
						OutObstList.Parts[k].Flags = 1;
					}

					OutObstList.Parts[k].DistStar = OutObstList.Parts[k].d0 + distFromPrevs;

					//GlobalVars.gAranGraphics.DrawLineString((LineString)CurrLeg.KKLine);
					//LegBase.ProcessMessages();
					//GlobalVars.gAranGraphics.DrawLineString((LineString)CurrLeg.NNSecondLine, 2);
					//LegBase.ProcessMessages();

					double pdgDist = OutObstList.Parts[k].DistStar;

					if (PtPrevFIX != null && CurrLeg.StartFIX.FlyMode == eFlyMode.Atheight)
					{
						Point ptNearest = NNLineGeoOp.GetNearestPoint(pCurrPt);

						double distFromS = ARANFunctions.Point2LineDistancePrj(ptNearest, PtPrevFIX.PrjPt, CurrLeg.StartFIX.EntryDirection + ARANMath.C_PI_2);
						pdgDist = OutObstList.Parts[k].d0 + Math.Abs(distFromS);
					}

					OutObstList.Parts[k].MOC = MOCLimit * OutObstList.Parts[k].fSecCoeff;
					OutObstList.Parts[k].ReqH = OutObstList.Parts[k].MOC + OutObstList.Parts[k].Height;
					OutObstList.Parts[k].ReqOCH = OutObstList.Parts[k].Elev + OutObstList.Parts[k].MOC - OutObstList.Parts[k].d0 * MAClimb;

					if (OutObstList.Parts[k].ReqOCH > maxO)
					{
						maxO = OutObstList.Parts[k].ReqOCH;
						resultObst = l;
						resultPart = k;
					}

					OutObstList.Parts[k].Ignored = false;
					k++;
				}
				l++;
			}

			if (k >= 0)
			{
				Array.Resize<Obstacle>(ref OutObstList.Obstacles, l);
				Array.Resize<ObstacleData>(ref OutObstList.Parts, k);

				if (resultPart >= 0)
				{
					result.Obstacles = new Obstacle[1];
					result.Parts = new ObstacleData[1];

					result.Obstacles[0] = OutObstList.Obstacles[resultObst];
					result.Parts[0] = OutObstList.Parts[resultPart];
				}
			}
			else
			{
				OutObstList.Obstacles = new Obstacle[0];
				OutObstList.Parts = new ObstacleData[0];
			}

			return result;
		}

		static public ObstacleContainer GetLegObstList(LegBase currLeg, ObstacleContainer inObstacleList, double MOCLimit, double refElev, out int maxReqHi)
		{
			ObstacleContainer ObstacleList;
			int n = inObstacleList.Obstacles.Length;

			ObstacleList.Parts = new ObstacleData[n];
			ObstacleList.Obstacles = new Obstacle[n];

			maxReqHi = -1;
			if (n == 0)
				return ObstacleList;

			var fullGeoOp = new JtsGeometryOperators();
			fullGeoOp.CurrentGeometry = fullGeoOp.Difference(currLeg.FullAssesmentArea, currLeg.PrimaryAssesmentArea);

			var primaryGeoOp = new JtsGeometryOperators()
			{
				CurrentGeometry = currLeg.PrimaryAssesmentArea
			};

			var lineStrGeoOp = new JtsGeometryOperators()
			{
				CurrentGeometry = ARANFunctions.PolygonToPolyLine(currLeg.FullAssesmentArea[0])
			};

			double maxAlt = double.MinValue;

			int maxParts = n;
			int l = -1, k = -1;

			for (int i = 0; i < n; i++)
			{
				var pGeomPrj = inObstacleList.Obstacles[i].pGeomPrj;

				var pObstPoints = new MultiPoint();

				var isPrima = false;
				var isInside = false;
				var obstacleHorAccuracy = inObstacleList.Obstacles[i].HorAccuracy;

				if (pGeomPrj.Type == GeometryType.Point)
				{
					if (fullGeoOp.GetDistance(pGeomPrj) - obstacleHorAccuracy <= 0)
						isInside = true;

					if (primaryGeoOp.GetDistance(pGeomPrj) - obstacleHorAccuracy <= 0)
					{
						isInside = true;
						isPrima = true;
					}

					//Is not inside primary and secondary area
					if (!isInside) continue;

					pObstPoints = new MultiPoint { (Point)pGeomPrj };
				}
				else
				{
					//If obstacle is inside of Primary area then there is no need to save all obstacles vertixes 
					if (!primaryGeoOp.Disjoint(pGeomPrj))
					{
						pObstPoints.Add(pGeomPrj.ToMultiPoint()[0]);
						isPrima = true;
					}
					else
					{
						var pts = fullGeoOp.Intersect(pGeomPrj);
						if (pts != null)
							pObstPoints.AddMultiPoint(pts.ToMultiPoint());
					}
				}

				int p = pObstPoints.Count;
				if (pObstPoints.Count == 0)
					continue;

				l++;
				k++;

				ObstacleList.Obstacles[l] = inObstacleList.Obstacles[i];
				//ObstacleList.Obstacles[l].PartsNum = 1;
				//ObstacleList.Obstacles[l].Parts = new int[1];
				ObstacleList.Parts[k].fSecCoeff = 1.0;

				var ptCurr = pObstPoints[0];

				if (!isPrima)
				{
					double distToPrimaryPoly;
					int minIndex = GetMinDistanceIndex(primaryGeoOp, pObstPoints, out distToPrimaryPoly);
					distToPrimaryPoly -= ObstacleList.Obstacles[l].HorAccuracy;
					ptCurr = pObstPoints[minIndex];

					double d1 = lineStrGeoOp.GetDistance(ptCurr);
					double d = distToPrimaryPoly + d1;
					ObstacleList.Parts[k].fSecCoeff = (d1 + ObstacleList.Obstacles[l].HorAccuracy) / d;

					if (ObstacleList.Parts[k].fSecCoeff > 1.0)
					{
						ObstacleList.Parts[k].fSecCoeff = 1.0;
						isPrima = true;
					}
				}

				ARANFunctions.PrjToLocal(currLeg.EndFIX.PrjPt, currLeg.StartFIX.OutDirection + Math.PI, ptCurr, out ObstacleList.Parts[k].Dist, out ObstacleList.Parts[k].CLShift);

				ObstacleList.Parts[k].MOC = ObstacleList.Parts[k].fSecCoeff * MOCLimit;
				ObstacleList.Parts[k].ReqH = ObstacleList.Parts[k].MOC + ObstacleList.Obstacles[l].Height;  // + ObstacleList.Obstacles[l].VertAccuracy;
				ObstacleList.Parts[k].Ignored = false;

				if (maxAlt < ObstacleList.Parts[k].ReqH)
				{
					maxAlt = ObstacleList.Parts[k].ReqH;
					maxReqHi = k;
				}

				ObstacleList.Parts[k].Prima = isPrima;
				ObstacleList.Parts[k].pPtPrj = ptCurr;
				ObstacleList.Parts[k].Owner = l;
				ObstacleList.Parts[k].Elev = ObstacleList.Obstacles[l].Height;
				ObstacleList.Parts[k].Height = ObstacleList.Parts[k].Elev - refElev;
				ObstacleList.Parts[k].Index = 0;
				//ObstacleList.Obstacles[l].Parts[0] = k;
			}

			l++;
			Array.Resize<Obstacle>(ref ObstacleList.Obstacles, l);
			Array.Resize<ObstacleData>(ref ObstacleList.Parts, k + 1);

			return ObstacleList;
			//return maxReqHi;
		}

		static public double GetMAObstAndMinOCH(LegBase leg, FIX FSOC, double course, double Gradient, double MinOCH, out int MaxIx, int AndMask = -1)
		{
			double FullFAMOC = GlobalVars.constants.Pansops[ePANSOPSData.arFASeg_FAF_MOC].Value;

			double result = MinOCH;

			MaxIx = -1;
			int n = leg.Obstacles.Parts.Length;

			for (int i = 0; i < n; i++)
			{
				leg.Obstacles.Parts[i].Dist = ARANFunctions.PointToLineDistance(leg.Obstacles.Parts[i].pPtPrj, FSOC.PrjPt, course - 0.5 * Math.PI);
				leg.Obstacles.Parts[i].Flags = leg.Obstacles.Parts[i].Flags & AndMask;

				if (leg.Obstacles.Parts[i].Dist >= 0)
				{
					leg.Obstacles.Parts[i].Plane = (int)eFlightProcedure.fpIntermediateMissedApproach;
					leg.Obstacles.Parts[i].ReqOCH = leg.Obstacles.Parts[i].ReqH - leg.Obstacles.Parts[i].Dist * Gradient;
				}
				else
				{
					leg.Obstacles.Parts[i].Plane = (int)eFlightProcedure.fpInitialMissedApproach;
					leg.Obstacles.Parts[i].ReqOCH = leg.Obstacles.Parts[i].Height + FullFAMOC * leg.Obstacles.Parts[i].fSecCoeff;
				}

				//			leg.Obstacles.Parts[i].ReqOCH = Math.Min(leg.Obstacles.Parts[i].ReqH - leg.Obstacles.Parts[i].Dist * Gradient,
				//										leg.Obstacles.Parts[i].Height + FullFAMOC * leg.Obstacles.Parts[i].fSecCoeff);

				if (result < leg.Obstacles.Parts[i].ReqOCH)
				{
					MaxIx = i;
					result = leg.Obstacles.Parts[i].ReqOCH;
				}
			}

			if (MaxIx >= 0)
				leg.Obstacles.Parts[MaxIx].Flags = leg.Obstacles.Parts[MaxIx].Flags | 2;

			return result;
		}

		static public double CalcMAMinOCA(LegBase leg, double Gradiend, double MinOCH, out int MaxIx, int AndMask = -1)
		{
			double result = MinOCH;
			double FullFAMOC = GlobalVars.constants.Pansops[ePANSOPSData.arFASeg_FAF_MOC].Value; //FlightPhases[fpFinalApproach].MOC;

			int n = leg.Obstacles.Parts.Length;
			MaxIx = -1;

			for (int i = 0; i < n; i++)
			{
				leg.Obstacles.Parts[i].Flags = leg.Obstacles.Parts[i].Flags & AndMask;

				if (leg.Obstacles.Parts[i].Dist >= 0)
					leg.Obstacles.Parts[i].ReqOCH = leg.Obstacles.Parts[i].ReqH - leg.Obstacles.Parts[i].Dist * Gradiend;
				else
					leg.Obstacles.Parts[i].ReqOCH = Math.Min(leg.Obstacles.Parts[i].ReqH - leg.Obstacles.Parts[i].Dist * Gradiend,
										leg.Obstacles.Parts[i].Height + FullFAMOC * leg.Obstacles.Parts[i].fSecCoeff);

				if (result < leg.Obstacles.Parts[i].ReqOCH)
				{
					MaxIx = i;
					result = leg.Obstacles.Parts[i].ReqOCH;
				}
			}

			if (MaxIx >= 0)
				leg.Obstacles.Parts[MaxIx].Flags = leg.Obstacles.Parts[MaxIx].Flags | 2;

			return result;
		}

		static Point GetNearestPoint(LineString pNNGeom, MultiPoint pPoints)
		{
			if (pNNGeom.IsEmpty)
				return null;

			double minDist = double.MaxValue;

			Point result = null;
			GeometryOperators pGeoOp = new GeometryOperators { CurrentGeometry = pNNGeom };

			foreach (Point pCurrPt in pPoints)
			{
				double dist = pGeoOp.GetDistance(pCurrPt);

				if (dist < minDist)
				{
					if (dist == 0)
						return pCurrPt;

					minDist = dist;
					result = pCurrPt;
				}
			}

			return result;
		}

		public static int GetMinDistanceIndex(JtsGeometryOperators geomOperators, MultiPoint mltGeo, out double minDistance)
		{
			int result = -1;
			minDistance = double.MaxValue;

			for (int i = 0; i < mltGeo.Count; i++)
			{
				var distance = geomOperators.GetDistance(mltGeo[i]);
				if (distance < minDistance)
				{
					result = i;
					minDistance = distance;
				}
			}

			return result;
		}

		public static void TextBoxFloat(ref char KeyChar, string BoxText)
		{
			if (KeyChar < 32)
				return;

			char DecSep = (1.1).ToString().ToCharArray()[1];

			if (((KeyChar < '0') || KeyChar > '9') && KeyChar != DecSep)
				KeyChar = '\0';
			else if (KeyChar == DecSep)
			{
				if (BoxText.Contains(DecSep.ToString()))
					KeyChar = '\0';
			}
		}

		public static void TextBoxSignedFloat(ref char KeyChar, string BoxText)
		{
			if (KeyChar < 32)
				return;

			char DecSep = (1.1).ToString().ToCharArray()[1];

			if (((KeyChar < '0') || KeyChar > '9') && KeyChar != DecSep && KeyChar != '-')
				KeyChar = '\0';
			else if (KeyChar == DecSep)
			{
				if (BoxText.Contains(DecSep.ToString()))
					KeyChar = '\0';
			}
		}

		public static void TextBoxInteger(ref char KeyChar)
		{
			if (KeyChar < ' ')
				return;
			if ((KeyChar < '0') || (KeyChar > '9'))
				KeyChar = '\0';
		}

		public static string[] NavTypeNames = new string[] { "VOR", "DME", "NDB", "LLZ", "TACAN", "Radar FIX" };

		public static string GetNavTypeName(eNavaidType navType)
		{
			if (navType == eNavaidType.NONE)
				return "WPT";
			else
				return NavTypeNames[(int)navType];
		}

		public static double VORFIXTolerArea(Point ptVor, double Aztin, double AbsH, out Polygon TolerArea)
		{
			double vORFIXTolerAreaReturn;

			Point ptCurr;
			double fTmp;
			double fTmpH = AbsH - ptVor.Z;
			double R = fTmpH * Math.Tan(GlobalVars.navaidsConstants.VOR.ConeAngle);

			TolerArea = new Polygon();
			TolerArea.ExteriorRing = new Ring();

			Point ptTmp = ARANFunctions.PointAlongPlane(ptVor, Aztin - (ARANMath.C_PI_2 + GlobalVars.navaidsConstants.VOR.TrackAccuracy), GlobalVars.navaidsConstants.VOR.LateralDeviationCoef * fTmpH);

			ptCurr = ARANFunctions.CircleVectorIntersect(ptVor, R, ptTmp, Aztin - GlobalVars.navaidsConstants.VOR.TrackAccuracy, out fTmp);
			TolerArea.ExteriorRing.Add(ptCurr);
			ptCurr = ARANFunctions.CircleVectorIntersect(ptVor, R, ptTmp, ARANMath.C_PI + Aztin - GlobalVars.navaidsConstants.VOR.TrackAccuracy, out fTmp);
			TolerArea.ExteriorRing.Add(ptCurr);
			ptTmp = ARANFunctions.PointAlongPlane(ptVor, Aztin + ARANMath.C_PI_2 + GlobalVars.navaidsConstants.VOR.TrackAccuracy, GlobalVars.navaidsConstants.VOR.LateralDeviationCoef * fTmpH);
			ptCurr = ARANFunctions.CircleVectorIntersect(ptVor, R, ptTmp, ARANMath.C_PI + Aztin + GlobalVars.navaidsConstants.VOR.TrackAccuracy, out fTmp);
			TolerArea.ExteriorRing.Add(ptCurr);

			ptCurr = ARANFunctions.CircleVectorIntersect(ptVor, R, ptTmp, Aztin + GlobalVars.navaidsConstants.VOR.TrackAccuracy, out fTmp);
			TolerArea.ExteriorRing.Add(ptCurr);

			vORFIXTolerAreaReturn = R;
			return vORFIXTolerAreaReturn;
		}

		public static double NDBFIXTolerArea(Point ptNDB, double Aztin, double AbsH, out Polygon TolerArea)
		{
			double nDBFIXTolerAreaReturn;
			double R;
			double qN;
			double fTmp;
			double fTmpH;

			Point ptTmp;
			Point ptCurr;

			fTmpH = AbsH - ptNDB.Z;
			R = fTmpH * Math.Tan(GlobalVars.navaidsConstants.NDB.ConeAngle);

			TolerArea = new Polygon();
			TolerArea.ExteriorRing = new Ring();

			qN = R * Math.Sin(ARANMath.DegToRad(GlobalVars.navaidsConstants.NDB.Entry2ConeAccuracy));
			ptTmp = ARANFunctions.PointAlongPlane(ptNDB, Aztin - ARANMath.C_PI_2, qN + Math.Sqrt(R * R - qN * qN) * Math.Tan(GlobalVars.navaidsConstants.NDB.TrackAccuracy));
			ptCurr = ARANFunctions.CircleVectorIntersect(ptNDB, R, ptTmp, Aztin - GlobalVars.navaidsConstants.NDB.TrackAccuracy, out fTmp);
			TolerArea.ExteriorRing.Add(ptCurr);
			ptCurr = ARANFunctions.CircleVectorIntersect(ptNDB, R, ptTmp, ARANMath.C_PI + Aztin - GlobalVars.navaidsConstants.NDB.TrackAccuracy, out fTmp);
			TolerArea.ExteriorRing.Add(ptCurr);
			ptTmp = ARANFunctions.PointAlongPlane(ptNDB, Aztin + ARANMath.C_PI_2, qN + Math.Sqrt(R * R - qN * qN) * Math.Tan(GlobalVars.navaidsConstants.NDB.TrackAccuracy));
			ptCurr = ARANFunctions.CircleVectorIntersect(ptNDB, R, ptTmp, ARANMath.C_PI + Aztin + GlobalVars.navaidsConstants.NDB.TrackAccuracy, out fTmp);
			TolerArea.ExteriorRing.Add(ptCurr);
			ptCurr = ARANFunctions.CircleVectorIntersect(ptNDB, R, ptTmp, Aztin + GlobalVars.navaidsConstants.NDB.TrackAccuracy, out fTmp);
			TolerArea.ExteriorRing.Add(ptCurr);

			nDBFIXTolerAreaReturn = R;
			return nDBFIXTolerAreaReturn;
		}

		public static bool ShowSaveDialog(out string FileName, out string FileTitle)
		{
			FileTitle = FileName = "";

			System.Windows.Forms.SaveFileDialog saveDialog = new System.Windows.Forms.SaveFileDialog()
			{

				FileName = "",
				Filter = "PANDA Report File (*.htm)|*.htm|All Files (*.*)|*.*",
				AddExtension = false,
			};

			if (saveDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
				return false;

			FileName = saveDialog.FileName;
			FileTitle = FileName;

			int pos = FileTitle.LastIndexOf('.');
			if (pos > 0)
				FileTitle = FileTitle.Substring(0, pos);

			int pos2 = FileTitle.LastIndexOf('\\');
			if (pos2 > 0)
				FileTitle = FileTitle.Substring(pos2 + 1);

			return true;
		}

		//public static bool ShowSaveDialog(out string FileName, out string FileTitle)
		//{
		//	FileTitle = FileName = "";

		//	System.Windows.Forms.SaveFileDialog saveDialog = new System.Windows.Forms.SaveFileDialog()
		//	{
		//		FileName = "",
		//		Filter = "PANDA Report File (*.htm)|*.htm|All Files (*.*)|*.*",
		//		AddExtension = false,
		//	};

		//	if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
		//	{
		//		FileName = saveDialog.FileName;

		//		int pos = FileName.LastIndexOf('.');
		//		if (pos > 0)
		//			FileName = FileName.Substring(0, pos);

		//		FileTitle = FileName;
		//		int pos2 = FileTitle.LastIndexOf('\\');
		//		if (pos2 > 0)
		//			FileTitle = FileTitle.Substring(pos2 + 1);

		//		return true;
		//	}

		//	return false;
		//}

		internal static string Degree2String(double X, Degree2StringMode Mode)
		{
			string sSign = "", sResult = "", sTmp;
			double xDeg, xMin, xIMin, xSec;
			bool lSign = false;

			if (Mode == Degree2StringMode.DMSLat)
			{
				lSign = Math.Sign(X) < 0;
				if (lSign)
					X = -X;

				xDeg = System.Math.Floor(X);
				xMin = (X - xDeg) * 60.0;
				xIMin = System.Math.Floor(xMin);
				xSec = (xMin - xIMin) * 60.0;   //		xSec = System.Math.Round((xMin - xIMin) * 60.0, 2);
				if (xSec >= 60.0)
				{
					xSec = 0.0;
					xIMin++;
				}

				if (xIMin >= 60.0)
				{
					xIMin = 0.0;
					xDeg++;
				}

				sTmp = xDeg.ToString("00");
				sResult = sTmp + "°";

				sTmp = xIMin.ToString("00");
				sResult = sResult + sTmp + "'";

				sTmp = xSec.ToString("00.00");
				sResult = sResult + sTmp + @"""";

				return sResult + (lSign ? "S" : "N");
			}

			if (Mode >= Degree2StringMode.DMSLon)
			{
				X = NativeMethods.Modulus(X);
				lSign = X > 180.0;
				if (lSign) X = 360.0 - X;

				xDeg = System.Math.Floor(X);
				xMin = (X - xDeg) * 60.0;
				xIMin = System.Math.Floor(xMin);
				xSec = (xMin - xIMin) * 60.0;
				if (xSec >= 60.0)
				{
					xSec = 0.0;
					xIMin++;
				}

				if (xIMin >= 60.0)
				{
					xIMin = 0.0;
					xDeg++;
				}

				sTmp = xDeg.ToString("000");
				sResult = sTmp + "°";

				sTmp = xIMin.ToString("00");
				sResult = sResult + sTmp + "'";

				sTmp = xSec.ToString("00.00");
				sResult = sResult + sTmp + @"""";

				return sResult + (lSign ? "W" : "E");
			}

			if (System.Math.Sign(X) < 0) sSign = "-";
			X = NativeMethods.Modulus(System.Math.Abs(X));

			switch (Mode)
			{
				case Degree2StringMode.DD:
					return sSign + X.ToString("#0.00##") + "°";
				case Degree2StringMode.DM:
					if (System.Math.Sign(X) < 0) sSign = "-";
					X = NativeMethods.Modulus(System.Math.Abs(X));

					xDeg = System.Math.Floor(X);
					xMin = (X - xDeg) * 60.0;
					if (xMin >= 60)
					{
						X++;
						xMin = 0;
					}

					sResult = sSign + xDeg.ToString() + "°";

					sTmp = xMin.ToString("00.00##");
					return sResult + sTmp + "'";
				case Degree2StringMode.DMS:
					if (System.Math.Sign(X) < 0) sSign = "-";
					X = NativeMethods.Modulus(System.Math.Abs(X));

					xDeg = System.Math.Floor(X);
					xMin = (X - xDeg) * 60.0;
					xIMin = System.Math.Floor(xMin);
					xSec = (xMin - xIMin) * 60.0;
					if (xSec >= 60.0)
					{
						xSec = 0.0;
						xIMin++;
					}

					if (xIMin >= 60.0)
					{
						xIMin = 0.0;
						xDeg++;
					}

					sResult = sSign + xDeg.ToString() + "°";

					sTmp = xIMin.ToString("00");
					sResult = sResult + sTmp + "'";

					sTmp = xSec.ToString("00.00");
					return sResult + sTmp + @"""";
			}
			return sResult;
		}

		//internal static void shall_SortsSort(ObstacleType[] obstacles)
		//{
		//	int lastRow = obstacles.GetUpperBound(0);
		//	if (lastRow < 0)
		//		return;

		//	int firstRow = obstacles.GetLowerBound(0);
		//	int numRows = lastRow - firstRow + 1;

		//	int gapSize = 0;
		//	do
		//		gapSize = gapSize * 3 + 1;
		//	while (gapSize <= numRows);

		//	do
		//	{
		//		gapSize = gapSize / 3;
		//		for (int i = gapSize + firstRow; i <= lastRow; i++)
		//		{
		//			int curPos = i;
		//			ObstacleType tmpVal = obstacles[i];

		//			while (String.Compare(obstacles[curPos - gapSize].sSort, tmpVal.sSort) > 0)
		//			{
		//				obstacles[curPos] = obstacles[curPos - gapSize];
		//				curPos = curPos - gapSize;
		//				if (curPos - gapSize < firstRow)
		//					break;
		//			}
		//			obstacles[curPos] = tmpVal;
		//		}
		//	}
		//	while (gapSize > 1);
		//}

		//internal static void shall_SortsSortD(ObstacleType[] obstacles)
		//{
		//	int lastRow = obstacles.GetUpperBound(0);
		//	if (lastRow < 0)
		//		return;

		//	int firstRow = obstacles.GetLowerBound(0);
		//	int numRows = lastRow - firstRow + 1;

		//	int gapSize = 0;
		//	do
		//		gapSize = gapSize * 3 + 1;
		//	while (gapSize <= numRows);

		//	do
		//	{
		//		gapSize = gapSize / 3;
		//		for (int i = gapSize + firstRow; i <= lastRow; i++)
		//		{
		//			int curPos = i;
		//			ObstacleType tmpVal = obstacles[i];

		//			while (String.Compare(obstacles[curPos - gapSize].sSort, tmpVal.sSort) < 0)
		//			{
		//				obstacles[curPos] = obstacles[curPos - gapSize];
		//				curPos = curPos - gapSize;
		//				if (curPos - gapSize < firstRow)
		//					break;
		//			}
		//			obstacles[curPos] = tmpVal;
		//		}
		//	}
		//	while (gapSize > 1);
		//}

		//internal static void shall_SortfSort(ObstacleType[] obstacles)
		//{
		//	int lastRow = obstacles.GetUpperBound(0);
		//	if (lastRow < 0)
		//		return;

		//	int firstRow = obstacles.GetLowerBound(0);
		//	int numRows = lastRow - firstRow + 1;

		//	int gapSize = 0;
		//	do
		//		gapSize = gapSize * 3 + 1;
		//	while (gapSize <= numRows);

		//	do
		//	{
		//		gapSize = gapSize / 3;
		//		for (int i = gapSize + firstRow; i <= lastRow; i++)
		//		{
		//			int curPos = i;
		//			ObstacleType tmpVal = obstacles[i];

		//			while (obstacles[curPos - gapSize].fSort > tmpVal.fSort)
		//			{
		//				obstacles[curPos] = obstacles[curPos - gapSize];
		//				curPos = curPos - gapSize;
		//				if (curPos - gapSize < firstRow)
		//					break;
		//			}
		//			obstacles[curPos] = tmpVal;
		//		}
		//	}
		//	while (gapSize > 1);
		//}

		//internal static void shall_SortfSortD(ObstacleType[] obstacles)
		//{
		//	int lastRow = obstacles.GetUpperBound(0);
		//	if (lastRow < 0)
		//		return;

		//	int firstRow = obstacles.GetLowerBound(0);
		//	int numRows = lastRow - firstRow + 1;

		//	int gapSize = 0;
		//	do
		//		gapSize = gapSize * 3 + 1;
		//	while (gapSize <= numRows);

		//	do
		//	{
		//		gapSize = gapSize / 3;
		//		for (int i = gapSize + firstRow; i <= lastRow; i++)
		//		{
		//			int curPos = i;
		//			ObstacleType tmpVal = obstacles[i];
		//			while (obstacles[curPos - gapSize].fSort < tmpVal.fSort)
		//			{
		//				obstacles[curPos] = obstacles[curPos - gapSize];
		//				curPos = curPos - gapSize;
		//				if (curPos - gapSize < firstRow)
		//					break;
		//			}
		//			obstacles[curPos] = tmpVal;
		//		}
		//	}
		//	while (gapSize > 1);
		//}

		public static double MinFlybyDistByHeightAtFIX(double dT, double derElev, double grd, double plannedAng, WayPoint wpt, double hHeight)
		{
			if (wpt.FlyMode != eFlyMode.Flyby)
			{
				if (wpt.FlyMode == eFlyMode.Atheight)
					return 0.0;

				return wpt.ATT;
			}

			if (hHeight < GlobalVars.constants.Pansops[ePANSOPSData.dpNGui_Ar1].Value)
				hHeight = GlobalVars.constants.Pansops[ePANSOPSData.dpNGui_Ar1].Value;              //120 m

			double BankInRadian = GlobalVars.constants.Pansops[ePANSOPSData.rnvFlyOInterBank].Value;
			double hAbovDer = GlobalVars.constants.Pansops[ePANSOPSData.dpH_abv_DER].Value;         //005 m
			double maxPDG = GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;

			double kTurn = Math.Tan(0.5 * plannedAng);
			double IASInMetrsInSec = wpt.IAS;

			double DistFixToFix = (hHeight - hAbovDer) / grd;
			double Altitude = derElev + hAbovDer + DistFixToFix * maxPDG;
			//double RTurn = ARANMath.BankToRadiusForRnav(BankInRadian, IASInMetrsInSec, Altitude, dT);
			double TASInMetrsInSec = ARANMath.IASToTAS(IASInMetrsInSec, Altitude, dT);
			double RTurn = ARANMath.BankToRadius(BankInRadian, TASInMetrsInSec);

			return RTurn * kTurn + wpt.ATT;
		}

		public static double MinFlybyDistByHeightAtKKLine(double dT, double derElev, double grd, double plannedAng, WayPoint wpt, double hHeight = -1)
		{
			if (wpt.FlyMode != eFlyMode.Flyby)
			{
				if (wpt.FlyMode == eFlyMode.Atheight)
					return 0.0;

				return wpt.ATT;
			}

			if (hHeight < GlobalVars.constants.Pansops[ePANSOPSData.dpNGui_Ar1].Value)
				hHeight = GlobalVars.constants.Pansops[ePANSOPSData.dpNGui_Ar1].Value;              //120 m

			double BankInRadian = GlobalVars.constants.Pansops[ePANSOPSData.rnvFlyOInterBank].Value;
			double hAbovDer = GlobalVars.constants.Pansops[ePANSOPSData.dpH_abv_DER].Value;         //005 m
			double maxPDG = GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;

			double kTurn = Math.Tan(0.5 * plannedAng);
			double IASInMetrsInSec = wpt.IAS;

			double d2KK = (hHeight - hAbovDer) / grd;

			double DistFixToFix = d2KK + wpt.ATT + 6071.35061;  //+ 6071.3506099238484;
			double Altitude = derElev + hAbovDer + DistFixToFix * maxPDG;

			double RTurn = ARANMath.BankToRadiusForRnav(BankInRadian, IASInMetrsInSec, Altitude, dT);

			double L1 = RTurn * kTurn;

			int io = 100;
			while (io >= 0)
			{
				double L1_Old = L1;

				DistFixToFix = d2KK + L1 + wpt.ATT;
				Altitude = derElev + hAbovDer + DistFixToFix * maxPDG;
				//RTurn = ARANMath.BankToRadiusForRnav(BankInRadian, IASInMetrsInSec, Altitude, dT);
				double TASInMetrsInSec = ARANMath.IASToTAS(IASInMetrsInSec, Altitude, dT);
				RTurn = ARANMath.BankToRadius(BankInRadian, TASInMetrsInSec);

				L1 = RTurn * kTurn;

				if (Math.Abs(L1 - L1_Old) < ARANMath.EpsilonDistance)
					break;
				io--;
			}
			return L1 + wpt.ATT;
		}

		internal static double CalcDFDirection(MATF fMATF, FIX fMAHF)
		{
			Point ptCnt;
			Ring Ring;
			List<Point> BasePoints;
			int I, J, N;
			double WSpeed, TurnR, TurnRCurr, SpTurnAngle, SpStartRadial, fTurnSide, dirSpCenter, Rv, coeff, fTmp, dirCurr, dirRef, dirCntToPtDst;

			double dirTouch = 9999.0;
			double result = dirTouch;

			if (fMATF.TolerArea.Count == 0)
				return dirTouch;

			BasePoints = ARANFunctions.GetBasePoints(fMATF.PrjPt, fMATF.EntryDirection, fMATF.TolerArea[0], fMATF.TurnDirection);
			//==============================================================================

			WSpeed = GlobalVars.constants.Pansops[ePANSOPSData.dpWind_Speed].Value;

			Rv = (1.76527777777777777777 / Math.PI) * Math.Tan(fMATF.BankAngle) / fMATF.ConstructTAS;
			if (Rv > 0.003) Rv = 0.003;

			if (Rv > 0.0) TurnR = fMATF.ConstructTAS / ((5.555555555555555555555 * Math.PI) * Rv);
			else TurnR = -1;

			coeff = WSpeed / ARANMath.DegToRad(1000.0 * Rv);

			fTurnSide = (int)fMATF.TurnDirection;

			ptCnt = new Point();
			Ring = new Ring();

			dirSpCenter = fMATF.EntryDirection - ARANMath.C_PI_2 * fTurnSide;
			SpStartRadial = fMATF.EntryDirection + ARANMath.C_PI_2 * fTurnSide;

			TurnRCurr = TurnR;
			dirCurr = fMATF.EntryDirection + Math.Atan2(coeff, TurnRCurr) * fTurnSide;
			N = BasePoints.Count;

			//	for I := 0 to N - 1 do
			//GUI.DrawPointWithText(TPoint(BasePoints.Items[I]), RGB(0, 255, 128+127*integer(MATF.TurnDirection)), IntToStr(I+1));

			for (I = 0; I < N; I++)
			{

				ARANFunctions.LocalToPrj(BasePoints[I], dirSpCenter, TurnR, 0, ptCnt);
				//GUI.DrawPoint(TPoint(BasePoints.Items[I]), RGB(0, 255, 0));

				J = (I + 1) & (0 - (I + 1 < N ? 1 : 0));

				dirRef = Math.Atan2(BasePoints[J].Y - BasePoints[I].Y, BasePoints[J].X - BasePoints[I].X);
				dirCntToPtDst = Math.Atan2(fMAHF.PrjPt.Y - ptCnt.Y, fMAHF.PrjPt.X - ptCnt.X);

				fTmp = ARANMath.Modulus((dirCntToPtDst - dirRef) * fTurnSide, ARANMath.C_2xPI);

				if (fTmp < Math.PI)
				{
					SpTurnAngle = ARANFunctions.SpiralTouchToPoint(ptCnt, TurnRCurr, coeff, dirCurr, fMATF.TurnDirection, fMAHF.PrjPt);

					if (SpTurnAngle > 100)
						return result;

					Rv = TurnRCurr + coeff * SpTurnAngle;
					dirTouch = SpStartRadial - (SpTurnAngle + ARANMath.C_PI_2 - Math.Atan2(coeff, Rv)) * fTurnSide;

					fTmp = ARANMath.Modulus((dirTouch - dirRef) * fTurnSide, ARANMath.C_2xPI);

					if (fTmp < Math.PI)
					{
						//GUI.DrawPoint(TPoint(BasePoints.Items[I]), RGB(0,0,255));
						break;
					}
				}

				SpTurnAngle = ARANFunctions.SpiralTouchAngle(TurnRCurr, coeff, SpStartRadial, dirCurr, dirRef, fMATF.TurnDirection);

				if (SpTurnAngle <= Math.PI)
				{
					TurnRCurr = TurnRCurr + coeff * SpTurnAngle;
					SpStartRadial = SpStartRadial - SpTurnAngle * fTurnSide;
				}

				dirCurr = dirRef;
			}


			return ARANMath.Modulus(dirTouch, ARANMath.C_2xPI);
		}

		internal static double CalcDRNomDir(MATF fMATF, FIX destFIX)
		{

			double dx, dy, TurnAngle, distDest, dirDest, fTurnDirection, PilotTolerance, BankTolerance, r, dist6sec;

			PilotTolerance = GlobalVars.constants.Pansops[ePANSOPSData.arMAPilotToleran].Value;
			BankTolerance = GlobalVars.constants.Pansops[ePANSOPSData.arMAPilotToleran].Value;

			fTurnDirection = (int)fMATF.TurnDirection;
			dist6sec = (fMATF.ConstructTAS + GlobalVars.constants.Pansops[ePANSOPSData.dpWind_Speed].Value) * (PilotTolerance + BankTolerance);
			double ftas = fMATF.ConstructTAS;
			r = fMATF.CalcTurnRadius(ftas);

			Point ptCenter = ARANFunctions.LocalToPrj(fMATF.PrjPt, fMATF.EntryDirection, dist6sec, -r * fTurnDirection);

			dx = destFIX.PrjPt.X - ptCenter.X;
			dy = destFIX.PrjPt.Y - ptCenter.Y;

			dirDest = Math.Atan2(dy, dx);
			distDest = ARANMath.Hypot(dy, dx);

			TurnAngle = (fMATF.EntryDirection - dirDest) * fTurnDirection + ARANMath.C_PI_2 - Math.Acos(r / distDest);
			return ARANMath.Modulus(fMATF.EntryDirection - TurnAngle * fTurnDirection, ARANMath.C_2xPI);
		}

		internal static MultiPolygon CreateMATurnArea(MATF MATF, FIX MAHF, double OutDir, bool Primary, bool Direct)
		{
			if (MATF.TolerArea.Count == 0)
				return null;

			//GlobalVars.gAranGraphics.DrawMultiPolygon(MATF.TolerArea, AranEnvironment.Symbols.eFillStyle.sfsSolid);

			double SplayAngle15 = GlobalVars.constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
			double DivergenceAngle30 = GlobalVars.constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;

			TurnDirection turnDirection = MATF.TurnDirection;
			if (turnDirection == TurnDirection.NONE)
				turnDirection = MATF.EffectiveTurnDirection;
			double fTurnSide = (int)turnDirection;
			//double fSide = IsOuter ? -((int)TurnDir) : ((int)TurnDir);

			double dirDst15 = OutDir - SplayAngle15 * fTurnSide;
			double WSpeed = GlobalVars.constants.Pansops[ePANSOPSData.dpWind_Speed].Value;

			double Rv = (1.76527777777777777777 / Math.PI) * Math.Tan(MATF.BankAngle) / MATF.ConstructTAS;
			if (Rv > 0.003)
				Rv = 0.003;

			double TurnR;
			if (Rv > 0.0) TurnR = MATF.ConstructTAS / ((5.555555555555555555555 * Math.PI) * Rv);
			else TurnR = -1;

			double coeff = WSpeed / ARANMath.DegToRad(1000.0 * Rv);

			//Outer side //===================================================================================

			List<Point> PrimBasePoints = ARANFunctions.GetBasePoints(MATF.PrjPt, MATF.EntryDirection, MATF.TolerArea[0], turnDirection);
			List<Point> SecBasePoints = ARANFunctions.GetBasePoints(MATF.PrjPt, MATF.EntryDirection, MATF.SecondaryTolerArea[0], turnDirection);

			//for (int i = 0; i < PrimBasePoints.Count; i++)
			//	GlobalVars.gAranGraphics.DrawPointWithText(PrimBasePoints[i], "pb-" + (i + 1).ToString());
			//LegBase.ProcessMessages();

			//for (int i = 0; i < SecBasePoints.Count; i++)
			//	GlobalVars.gAranGraphics.DrawPointWithText(SecBasePoints[i], "sb-" + (i + 1).ToString());
			//LegBase.ProcessMessages();
			//==============================================================================
			double dR, AreaWidth, SpTurnAngle;

			List<Point> BasePoints;

			if (Primary)
			{
				dR = 0.0;
				AreaWidth = 0.5 * MAHF.SemiWidth;
				BasePoints = PrimBasePoints;
			}
			else
			{
				dR = ARANMath.Hypot(PrimBasePoints[0].Y - SecBasePoints[0].Y, PrimBasePoints[0].X - SecBasePoints[0].X);
				AreaWidth = MAHF.SemiWidth;
				BasePoints = SecBasePoints;
			}

			Ring ring = new Ring();
			double dirSpCenter = MATF.EntryDirection + ARANMath.C_PI_2 * fTurnSide;
			double SpStartRadial = MATF.EntryDirection - ARANMath.C_PI_2 * fTurnSide;

			double TurnRCurr = TurnR + dR;
			double dirCurr = MATF.EntryDirection - Math.Atan2(coeff, TurnRCurr) * fTurnSide;
			double dirRef = 0.0;

			int n = BasePoints.Count, j = 0;
			Point ptCurr, ptTmp, ptCnt = null;

			if (ARANMath.Modulus((dirDst15-dirCurr) * fTurnSide, ARANMath.C_2xPI) < Math.PI || ARANMath.Modulus((OutDir-dirCurr) * fTurnSide, ARANMath.C_2xPI) > 2 * SplayAngle15)
			{
				for (int i = 0; i < n; i++)
				{
					if (Primary)
						ptCnt = ARANFunctions.LocalToPrj(BasePoints[i], dirSpCenter, TurnR);
					else
					{
						ptTmp = ARANFunctions.PrjToLocal(MATF.PrjPt, MATF.EntryDirection, BasePoints[i]);
						ptCnt = ARANFunctions.LocalToPrj(BasePoints[i], dirSpCenter, TurnR - Math.Sign(ptTmp.Y * fTurnSide) * dR);
					}

					//GlobalVars.gAranGraphics.DrawPointWithText(ptCnt, "ptCnt");
					//LegBase.ProcessMessages();

					j = (i + 1) & (0 - (i + 1 < n ? 1 : 0));

					dirRef = Math.Atan2(BasePoints[j].Y - BasePoints[i].Y, BasePoints[j].X - BasePoints[i].X);      // Side direction

					double dAlpha = ARANMath.Modulus((dirDst15 - dirCurr) * fTurnSide, ARANMath.C_2xPI);
					double fTmp = ARANMath.Modulus((dirRef - dirCurr) * fTurnSide, ARANMath.C_2xPI);
					//fTmp = ARANMath.Modulus(dirDst15 - dirRef * fTurnSide, ARANMath.C_2xPI);
					//if (fTmp < Math.PI)
					if (dAlpha < fTmp)
					{
						SpTurnAngle = ARANFunctions.SpiralTouchAngle(TurnRCurr, coeff, SpStartRadial, dirCurr, dirDst15, turnDirection);    //ARANMath.ChangeDirection()
						ARANFunctions.AddSpiralToRing(ptCnt, TurnRCurr, coeff, SpStartRadial, SpTurnAngle, turnDirection, ref ring);

						dirCurr = dirDst15;
						TurnRCurr += coeff * SpTurnAngle;
						SpStartRadial += SpTurnAngle * fTurnSide;
						break;
					}

					SpTurnAngle = ARANFunctions.SpiralTouchAngle(TurnRCurr, coeff, SpStartRadial, dirCurr, dirRef, turnDirection);

					if (SpTurnAngle > Math.PI)
					{
						if (ring.Count == 0)
						{
							Point ptCur = ARANFunctions.LocalToPrj(ptCnt, SpStartRadial, TurnRCurr);
							ring.Add(ptCur);
						}

						Geometry geom = ARANFunctions.LineLineIntersect(ring[ring.Count - 1], dirCurr, BasePoints[j], dirSpCenter);
						if (geom.Type == GeometryType.Point)
							ring.Add((Point)geom);
						//GlobalVars.gAranGraphics.DrawPointWithText((Point)geom, "Geom");
						//LegBase.ProcessMessages();
					}
					else
					{
						ARANFunctions.AddSpiralToRing(ptCnt, TurnRCurr, coeff, SpStartRadial,
										SpTurnAngle, turnDirection, ref ring);

						TurnRCurr += coeff * SpTurnAngle;
						SpStartRadial += SpTurnAngle * fTurnSide;
					}
					dirCurr = dirRef;
				}
				ptCurr = ring[ring.Count - 1];
			}
			else
			{
				if (Primary)
					ptCnt = ARANFunctions.LocalToPrj(BasePoints[0], dirSpCenter, TurnR);
				else
				{
					ptTmp = ARANFunctions.PrjToLocal(MATF.PrjPt, MATF.EntryDirection, BasePoints[0]);
					ptCnt = ARANFunctions.LocalToPrj(BasePoints[0], dirSpCenter, TurnR - Math.Sign(ptTmp.Y * fTurnSide) * dR);
				}

				ptCurr = BasePoints[0];
				ring.Add(ptCurr);
				j = 1;
				dirRef = Math.Atan2(BasePoints[1].Y - BasePoints[0].Y, BasePoints[1].X - BasePoints[0].X);
			}

			Point ptRef = ARANFunctions.LocalToPrj(MAHF.PrjPt, OutDir, 0, -AreaWidth * fTurnSide);

			//GlobalVars.gAranGraphics.DrawPointWithText(ptCnt, "ptCnt");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptRef, "ptRef");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptCurr, "ptCurr");
			//GlobalVars.gAranGraphics.DrawRing(ring, AranEnvironment.Symbols.eFillStyle.sfsCross);

			//LineString ls = new LineString();
			//ls.Add(ptRef);
			//ls.Add(ARANFunctions.LocalToPrj(ptRef, OutDir, -25000));
			//GlobalVars.gAranGraphics.DrawLineString(ls);
			//LegBase.ProcessMessages(true);

			ptTmp = ARANFunctions.PrjToLocal(ptRef, OutDir, ptCurr);
			bool refFlg = true;

			if (ptTmp.Y * fTurnSide > 0)
			{
				Geometry geom = ARANFunctions.LineLineIntersect(ptCurr, dirDst15, ptRef, OutDir);
				if (geom.Type == GeometryType.Point)
				{
					//GlobalVars.gAranGraphics.DrawRing(ring, AranEnvironment.Symbols.eFillStyle.sfsCross);
					//GlobalVars.gAranGraphics.DrawPointWithText((Point)geom, "Geom1");
					//LegBase.ProcessMessages(true);

					ptTmp = ARANFunctions.PrjToLocal(MAHF.PrjPt, OutDir, (Point)geom);
					if (ptTmp.X > 0)
					{

						geom = ARANFunctions.LineLineIntersect(ptCurr, dirDst15, ptRef, OutDir + ARANMath.C_PI_2);
						refFlg = false;
					}

					if (geom.Type == GeometryType.Point)
						ring.Add((Point)geom);
				}
			}
			else
			{
				double dirDst30 = OutDir + DivergenceAngle30 * fTurnSide;

				//LineString ls = new LineString();
				//ls.Add(ring[ring.Count - 1]);
				//ls.Add(ARANFunctions.LocalToPrj(ring[ring.Count - 1], dirDst30, 5000));
				//GlobalVars.gAranGraphics.DrawLineString(ls);
				//LegBase.ProcessMessages();

				if (ARANMath.Modulus((dirRef - dirDst30) * fTurnSide, ARANMath.C_2xPI) > Math.PI)
				{
					SpTurnAngle = ARANFunctions.SpiralTouchAngle(TurnRCurr, coeff, SpStartRadial, dirCurr, dirRef, turnDirection);

					ARANFunctions.AddSpiralToRing(ptCnt, TurnRCurr, coeff, SpStartRadial, SpTurnAngle, turnDirection, ref ring);
					dirCurr = dirRef;
					TurnRCurr += coeff * SpTurnAngle;
					SpStartRadial += SpTurnAngle * fTurnSide;

					if (Primary)
						ptCnt = ARANFunctions.LocalToPrj(BasePoints[j], dirSpCenter, TurnR);
					else
					{
						ptTmp = ARANFunctions.PrjToLocal(MATF.PrjPt, MATF.EntryDirection, BasePoints[j]);
						ptCnt = ARANFunctions.LocalToPrj(BasePoints[j], dirSpCenter, TurnR + Math.Sign(ptTmp.Y * fTurnSide) * dR);
					}
				}

				//GlobalVars.gAranGraphics.DrawPointWithText(ptCnt, "ptCnt");
				//GlobalVars.gAranGraphics.DrawPointWithText(ring[ring.Count - 1], "ptCurr");
				//GlobalVars.gAranGraphics.DrawRing(ring, AranEnvironment.Symbols.eFillStyle.sfsCross);
				//LegBase.ProcessMessages();

				//LineString ls = new LineString();
				//ls.Add(BasePoints[1]);
				//ls.Add(ARANFunctions.LocalToPrj(BasePoints[1], dirCurr, -10000));
				//GlobalVars.gAranGraphics.DrawLineString(ls);
				//LegBase.ProcessMessages();

				SpTurnAngle = ARANFunctions.SpiralTouchAngle(TurnRCurr, coeff, SpStartRadial, dirCurr, dirDst30, turnDirection);

				if (SpTurnAngle >= 0)
				{
					int m = (int)Math.Round(ARANMath.RadToDeg(SpTurnAngle));

					double dAlpha = 0;
					if (m > 0)
					{
						//if (m < 1) m = 1;	else 
						if (m < 5) m = 5;
						else if (m < 10) m = 10;
						dAlpha = SpTurnAngle / m;
					}
					bool bFlag = false;

					double PrevX = 0.0, PrevY = 0.0;
					for (int i = 0; i <= m; i++)
					{
						double R = TurnRCurr + i * dAlpha * coeff;
						ptCurr = ARANFunctions.LocalToPrj(ptCnt, SpStartRadial + i * dAlpha * fTurnSide, R);

						//GlobalVars.gAranGraphics.DrawPointWithText(ptCurr, "ptCurr");
						//LegBase.ProcessMessages();

						ptTmp = ARANFunctions.PrjToLocal(ptRef, OutDir, ptCurr);

						if ((i > 0) && ((ptTmp.Y * fTurnSide > 0) || (ptTmp.X >= 0)))
						{
							double K;
							refFlg = ptTmp.X <= 0;
							if (ptTmp.X < 0) K = -PrevY / (ptTmp.Y - PrevY);
							else K = -PrevX / (ptTmp.X - PrevX);

							ptCurr.X = ring[ring.Count - 1].X + K * (ptCurr.X - ring[ring.Count - 1].X);
							ptCurr.Y = ring[ring.Count - 1].Y + K * (ptCurr.Y - ring[ring.Count - 1].Y);

							ring.Add(ptCurr);
							bFlag = true;
							break;
						}

						PrevX = ptTmp.X;
						PrevY = ptTmp.Y;
						ring.Add(ptCurr);
					}

					//GlobalVars.gAranGraphics.DrawRing(ring, AranEnvironment.Symbols.eFillStyle.sfsCross);
					//LegBase.ProcessMessages(true);

					if ((!bFlag) && refFlg)
					{
						Geometry geom = ARANFunctions.LineLineIntersect(ptCurr, dirDst30, ptRef, OutDir);

						if (geom.Type == GeometryType.Point)
						{
							ptTmp = ARANFunctions.PrjToLocal(MAHF.PrjPt, OutDir, (Point)geom);
							if (ptTmp.X > 0)
							{

								geom = ARANFunctions.LineLineIntersect(ptCurr, dirDst30, ptRef, OutDir + ARANMath.C_PI_2);
								refFlg = false;
							}

							if (geom.Type == GeometryType.Point)
								ring.Add((Point)geom);
						}
					}

				}
			}

			if (refFlg)
				ring.Add(ptRef);

			ring.Add(MAHF.PrjPt);


			//Inner side
			//===================================================================================
			double dirDst = 0.0, OutDir2; 
			if (Direct && Primary)
			{
				for (int i = 0; i < n; i++)
				{
					bool bFlag = false;
					ptCurr = BasePoints[i];
					dirDst15 = Math.Atan2(MAHF.PrjPt.Y - ptCurr.Y, MAHF.PrjPt.X - ptCurr.X) - SplayAngle15 * fTurnSide;

					for (j = 0; j < n; j++)
					{
						if (j != i)
						{
							ptTmp = ARANFunctions.PrjToLocal(ptCurr, dirDst15, BasePoints[j]);
							if (ptTmp.Y * fTurnSide < 0)
							{
								bFlag = true;
								break;
							}
						}
					}
					if (!bFlag) break;
				}

				dirDst = dirDst15;
				OutDir2 = dirDst15 + SplayAngle15 * fTurnSide;
				MATF.OutDirection_R = OutDir2;
				//MATF.OutDirection_L = OutDir2;

				ptRef = ARANFunctions.LocalToPrj(MAHF.PrjPt, OutDir2, 0, AreaWidth * fTurnSide);

				//==
				ring.Add(ptRef);

				Geometry geom = ARANFunctions.LineLineIntersect(ptCurr, dirDst, ptRef, OutDir2);
				if (geom.Type == GeometryType.Point)
				{
					ptTmp = ARANFunctions.PrjToLocal(MAHF.PrjPt, OutDir2, (Point)geom);
					if (ptTmp.X > 0)
					{

						geom = ARANFunctions.LineLineIntersect(ptCurr, dirDst, ptRef, OutDir2 + ARANMath.C_PI_2);
						ring.Remove(ring.Count - 1);
					}

					if (geom.Type == GeometryType.Point)
						ring.Add((Point)geom);
				}

				ring.Add(ptCurr);
			}
			else
			{
				if (Direct)
					OutDir2 = MATF.OutDirection_R;
				else
					OutDir2 = OutDir;

				dirDst15 = OutDir2 + SplayAngle15 * fTurnSide;
				double dirDst30 = OutDir2 - DivergenceAngle30 * fTurnSide;
				ptRef = ARANFunctions.LocalToPrj(MAHF.PrjPt, OutDir2, 0, AreaWidth * fTurnSide);

				//GlobalVars.gAranGraphics.DrawPointWithText(ptRef, "ptRef1");
				//LegBase.ProcessMessages();

				int Ist = 0, c30 = 0;

				//LineString ls = new LineString();
				//ls.Add(ptRef);
				//ls.Add(ARANFunctions.LocalToPrj(ptRef, OutDir2, -25000));
				//GlobalVars.gAranGraphics.DrawLineString(ls);
				//LegBase.ProcessMessages();

				for (int i = 0; i < n; i++)
				{
					bool bFlag = false;
					ptCurr = BasePoints[i];
					ptTmp = ARANFunctions.PrjToLocal(ptRef, OutDir2, ptCurr);

					if (ptTmp.Y * fTurnSide < 0)
						dirDst = dirDst15;
					else
					{
						dirDst = dirDst30;
						Ist = i;
						c30++;
					}

					for (j = 0; j < n; j++)
					{
						if (j != i)
						{
							ptCnt = ARANFunctions.PrjToLocal(ptCurr, dirDst, BasePoints[j]);
							if (ptCnt.Y * fTurnSide > 0)
							{
								bFlag = true;
								break;
							}
						}
					}

					if (!bFlag)
						break;
				}

				//		if (bFlag)
				//		{
				//			if(bFlag)
				if (c30 == 1)
				{
					ptCurr = BasePoints[Ist];
					ptTmp = ARANFunctions.PrjToLocal(ptRef, OutDir2, ptCurr);
					dirDst = dirDst30;
				}

				//		end;
				//==
				ring.Add(ptRef);

				if (Math.Abs(ptTmp.Y) > ARANMath.EpsilonDistance)
				{
					Geometry geom = ARANFunctions.LineLineIntersect(ptCurr, dirDst, ptRef, OutDir2);

					//GlobalVars.gAranGraphics.DrawPointWithText((Point)geom, "Geom2");
					//LegBase.ProcessMessages();

					if (geom.Type == GeometryType.Point)
					{
						ptTmp = ARANFunctions.PrjToLocal(MAHF.PrjPt, OutDir2, (Point)geom);
						if (ptTmp.X > 0)
						{
							geom = ARANFunctions.LineLineIntersect(ptCurr, dirDst, ptRef, OutDir2 + ARANMath.C_PI_2);
							ring.Remove(ring.Count - 1);
						}

						if (geom.Type == GeometryType.Point)
							ring.Add((Point)geom);
					}
				}

				ring.Add(ptCurr);
			}

			Polygon tmPolygon = new Polygon();
			tmPolygon.ExteriorRing = ring;

			MultiPolygon result = new MultiPolygon();
			result.Add(tmPolygon);
			return result;
		}

	}
}

