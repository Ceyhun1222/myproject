using System;

using Aran.AranEnvironment;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries;
using Aran.PANDA.Common;

using System.Collections.Generic;
using System.Runtime.InteropServices;
using Aran.PANDA.Constants;

namespace Aran.PANDA.RNAV.Arrival
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class Functions
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

		public static string[] NavTypeNames = new string[] { "VOR", "DME", "NDB", "LOC", "TACAN", "Radar FIX" };

		public static string Tostring(this eNavaidType navType)
		{
			if (navType == eNavaidType.NONE)
				return "WPT";
			else
				return NavTypeNames[(int)navType];
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

		public static void TextBoxInteger(ref char KeyChar)
		{
			if (KeyChar < ' ')
				return;
			if ((KeyChar < '0') || (KeyChar > '9'))
				KeyChar = '\0';
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

			//string ProjectPath = GlobalVars.GetMapFileName();
			//int pos = ProjectPath.LastIndexOf('\\');
			//int pos2 = ProjectPath.LastIndexOf('.');

			//SaveDialog1.DefaultExt = "";
			//SaveDialog1.InitialDirectory = ProjectPath.Substring(0, pos);
			//SaveDialog1.Title = Properties.Resources.str00511;
			//SaveDialog1.FileName = ProjectPath.Substring(0, pos2 - 1) + ".htm";

			if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
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

			return false;
		}

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
				xSec = (xMin - xIMin) * 60.0;	//		xSec = System.Math.Round((xMin - xIMin) * 60.0, 2);
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

		static void RemoveSeamPoints(ref MultiPoint pPoints)
		{
			int n = pPoints.Count;
			int j = 0;
			while (j < n - 1)
			{
				Point pCurrPt = pPoints[j];
				int i = j + 1;
				while (i < n)
				{
					double dx = pCurrPt.X - pPoints[i].X;
					double dy = pCurrPt.Y - pPoints[i].Y;

					double fDist = dx * dx + dy * dy;
					if (fDist < ARANMath.EpsilonDistance)
					{
						pPoints.Remove(i);
						n--;
					}
					else
						i++;
				}
				j++;
			}
		}

		//static public int GetLegObstList(ObstacleContainer inObstacleList, out ObstacleContainer ObstacleList, LegBase currLeg, double MOCLimit, double fRefHeight = 0.0)
		//{
		//	int n = inObstacleList.Obstacles.Length;

		//	ObstacleList.Parts = new ObstacleData[n];
		//	ObstacleList.Obstacles = new Obstacle[n];

		//	if (n == 0)
		//		return -1;

		//	//GeometryOperators fullGeoOp = new GeometryOperators();
		//	//fullGeoOp.CurrentGeometry = currLeg.FullAssesmentArea;

		//	//GeometryOperators primaryGeoOp = new GeometryOperators();
		//	//primaryGeoOp.CurrentGeometry = currLeg.PrimaryAssesmentArea;

		//	//GeometryOperators lineStrGeoOp = new GeometryOperators();
		//	//lineStrGeoOp.CurrentGeometry = ARANFunctions.PolygonToPolyLine(currLeg.FullAssesmentArea[0]);

  //          var fullGeoOp = new Geometries.Operators.JtsGeometryOperators();
  //          fullGeoOp.CurrentGeometry = currLeg.FullAssesmentArea;

  //          var primaryGeoOp = new Geometries.Operators.JtsGeometryOperators();
  //          primaryGeoOp.CurrentGeometry = currLeg.PrimaryAssesmentArea;

  //          var lineStrGeoOp = new Geometries.Operators.JtsGeometryOperators();
  //          lineStrGeoOp.CurrentGeometry = ARANFunctions.PolygonToPolyLine(currLeg.FullAssesmentArea[0]);

  //          double maxAlt = double.MinValue;

		//	int maxParts = n;
		//	int l = -1, k = -1, result =-1;

		//	for (int i = 0; i < n; i++)
		//	{
		//		Geometry pGeomPrj = inObstacleList.Obstacles[i].pGeomPrj;
		//		if (fullGeoOp.Disjoint(pGeomPrj))
		//			continue;

		//		MultiPoint pObstPoints;

		//		if (pGeomPrj.Type == GeometryType.Point)
		//		{
		//			pObstPoints = new MultiPoint();
		//			pObstPoints.Add((Point)pGeomPrj);
		//		}
		//		else
		//		{
		//			Geometry pts = primaryGeoOp.Intersect(pGeomPrj);
		//			pObstPoints = pts.ToMultiPoint();

		//			pts = fullGeoOp.Intersect(pGeomPrj);
		//			pObstPoints.AddMultiPoint(pts.ToMultiPoint());

		//			//GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pts, -1, AranEnvironment.Symbols.eFillStyle.sfsHorizontal);
		//			//LegDep.ProcessMessages();

		//			RemoveSeamPoints(ref pObstPoints);
		//		}

		//		int p = pObstPoints.Count;
		//		if (p == 0)
		//			continue;

		//		l++;

		//		ObstacleList.Obstacles[l] = inObstacleList.Obstacles[i];
		//		ObstacleList.Obstacles[l].PartsNum = p;
		//		ObstacleList.Obstacles[l].Parts = new int[p];

		//		for (int j = 0; j < p; j++)
		//		{
		//			k++;
		//			if (k >= maxParts)
		//			{
		//				maxParts += n;
		//				System.Array.Resize<ObstacleData>(ref ObstacleList.Parts, maxParts);
		//			}

		//			Point ptCurr = pObstPoints[j];

		//			ObstacleList.Parts[k].pPtPrj = ptCurr;
		//			ObstacleList.Parts[k].Owner = l;
		//			ObstacleList.Parts[k].Height = ObstacleList.Obstacles[l].Height;
		//			ObstacleList.Parts[k].Index = j;
		//			ObstacleList.Obstacles[l].Parts[j] = k;

		//			ARANFunctions.PrjToLocal(currLeg.StartFIX.PrjPt, currLeg.StartFIX.OutDirection, ptCurr, out ObstacleList.Parts[k].Dist, out ObstacleList.Parts[k].CLShift);

		//			double distToPrimaryPoly = primaryGeoOp.GetDistance(ptCurr);
		//			ObstacleList.Parts[k].Prima = distToPrimaryPoly <= ObstacleList.Obstacles[l].HorAccuracy;
		//			ObstacleList.Parts[k].fSecCoeff = 1.0;

		//			if (!ObstacleList.Parts[k].Prima)
		//			{
		//				double d1 = lineStrGeoOp.GetDistance(ptCurr);
		//				double d = distToPrimaryPoly + d1;
		//				ObstacleList.Parts[k].fSecCoeff = (d1 + ObstacleList.Obstacles[l].HorAccuracy) / d;

		//				if (ObstacleList.Parts[k].fSecCoeff > 1.0)
		//				{
		//					ObstacleList.Parts[k].fSecCoeff = 1.0;
		//					ObstacleList.Parts[k].Prima = true;
		//				}
		//			}

		//			ObstacleList.Parts[k].MOC = ObstacleList.Parts[k].fSecCoeff * MOCLimit;
		//			ObstacleList.Parts[k].ReqH = ObstacleList.Parts[k].MOC + ObstacleList.Parts[k].Height;// + ObstacleList.Obstacles[l].VertAccuracy;
		//			ObstacleList.Parts[k].Ignored = false;

		//			if (maxAlt < ObstacleList.Parts[k].ReqH)
		//			{
		//				maxAlt = ObstacleList.Parts[k].ReqH;
		//				result = k;
		//			}
		//		}
		//	}

		//	l++;
		//	System.Array.Resize<Obstacle>(ref ObstacleList.Obstacles, l);
		//	System.Array.Resize<ObstacleData>(ref ObstacleList.Parts, k + 1);

		//	return result;
		//}

        static public int GetLegObstList(ObstacleContainer inObstacleList, out ObstacleContainer ObstacleList, LegBase currLeg, double MOCLimit, double fRefHeight = 0.0)
        {
            int n = inObstacleList.Obstacles.Length;

            ObstacleList.Parts = new ObstacleData[n];
            ObstacleList.Obstacles = new Obstacle[n];

            if (n == 0)
                return -1;

            var fullGeoOp = new Geometries.Operators.JtsGeometryOperators();
			//	currLeg.FullAssesmentArea = fullGeoOp.SimplifyGeometry(currLeg.FullAssesmentArea, 0.001) as Aran.Geometries.MultiPolygon;
			//currLeg.PrimaryAssesmentArea = (Aran.Geometries.MultiPolygon)fullGeoOp.SimplifyGeometry(currLeg.PrimaryAssesmentArea);
			fullGeoOp.CurrentGeometry = fullGeoOp.Difference(currLeg.FullAssesmentArea, currLeg.PrimaryAssesmentArea);

			//GlobalVars.gAranGraphics.DrawMultiPolygon(currLeg.FullAssesmentArea, -1, eFillStyle.sfsBackwardDiagonal);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(currLeg.PrimaryAssesmentArea, -1, eFillStyle.sfsForwardDiagonal);
			//GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)fullGeoOp.CurrentGeometry, -1, eFillStyle.sfsCross);
			//LegBase.ProcessMessages();

			var primaryGeoOp = new Geometries.Operators.JtsGeometryOperators();
            primaryGeoOp.CurrentGeometry = currLeg.PrimaryAssesmentArea;

            var lineStrGeoOp = new Geometries.Operators.JtsGeometryOperators();
            lineStrGeoOp.CurrentGeometry = currLeg.FullProtectionAreaOutline(); // ARANFunctions.PolygonToPolyLine(currLeg.FullAssesmentArea[0]);

			double maxAlt = double.MinValue;

            int maxParts = n;
            int l = -1, k = -1, result = -1;

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

                    pObstPoints = new MultiPoint {(Point) pGeomPrj};
                }
                else
                {
					//If obstacle is inside of Primary area then there is no need to save all obstacles vertixes 
					if (!primaryGeoOp.Disjoint(pGeomPrj))
                    {
                        //pObstPoints.Add(pGeomPrj.ToMultiPoint()[0]);
						pObstPoints.AddMultiPoint(pGeomPrj.ToMultiPoint());
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
                ObstacleList.Obstacles[l].PartsNum = 1;
                //ObstacleList.Obstacles[l].Parts = new int[1];
                ObstacleList.Parts[k].fSecCoeff = 1.0;

				Point ptCurr;// = pObstPoints[0];

				if (isPrima)
				{
					double distToEnd = double.MaxValue;
					int j, ix = -1;

					for (j = 0; j < pObstPoints.Count; j++)
					{
						var distance = ARANFunctions.ReturnDistanceInMeters(currLeg.EndFIX.PrjPt, pObstPoints[j]);
						if (distance < distToEnd)
						{
							ix = j;
							distToEnd = distance;
						}
					}

					ptCurr = pObstPoints[ix];
				}
				else
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
						ObstacleList.Parts[k].Prima = true;
					}
				}

				ARANFunctions.PrjToLocal(currLeg.StartFIX.PrjPt, currLeg.StartFIX.OutDirection, ptCurr, out ObstacleList.Parts[k].Dist, out ObstacleList.Parts[k].CLShift);

				ObstacleList.Parts[k].MOC = ObstacleList.Parts[k].fSecCoeff * MOCLimit;
				ObstacleList.Parts[k].ReqH = ObstacleList.Parts[k].MOC + ObstacleList.Obstacles[l].Height;// + ObstacleList.Obstacles[l].VertAccuracy;
                ObstacleList.Parts[k].Ignored = false;

                if (maxAlt < ObstacleList.Parts[k].ReqH)
                {
                    maxAlt = ObstacleList.Parts[k].ReqH;
                    result = k;
                }

                ObstacleList.Parts[k].Prima = isPrima;
                ObstacleList.Parts[k].pPtPrj = ptCurr;
                ObstacleList.Parts[k].Owner = l;
                ObstacleList.Parts[k].Height = ObstacleList.Obstacles[l].Height;
                ObstacleList.Parts[k].Index = 0;
                //ObstacleList.Obstacles[l].Parts[0] = k;
            }

            l++;
            Array.Resize<Obstacle>(ref ObstacleList.Obstacles, l);
            Array.Resize<ObstacleData>(ref ObstacleList.Parts, k + 1);

            return result;
        }

	    public static int GetMinDistanceIndex(Geometries.Operators.JtsGeometryOperators geomOperators, MultiPoint mltGeo,out double minDistance)
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

		public static int ComparePartsByOwner(ObstacleData d0, ObstacleData d1)
		{
			if (d0.Owner < d1.Owner) return -1;
			if (d0.Owner > d1.Owner) return 1;
			return 0;
		}

		public static int ownerStarts(ref ObstacleData[] parts, int owner, int pn)
		{
			if (parts[0].Owner > owner)
				return -1;

			int i, n = parts.Length;

			for (i = pn; i < n; i++)
				if (parts[i].Owner < 0)
					break;
				else if (parts[i].Owner == owner)
					return i;


			if (parts[pn - 1].Owner < owner)
				return -1;

			int start = 0, end = pn;
			//i = 0;
			while (true)
			{
				int curr = (end + start) >> 1;
				//int curr = ((end + start) >> 1) + 1;

				if (parts[curr].Owner == owner)
				{
					while (curr > 0 && parts[curr - 1].Owner == owner)
						curr--;

					return curr;
				}

				if (parts[curr].Owner > owner)
				{
					if (end > curr)
						end = curr;
					else
					{
						while (curr > 0 && parts[curr].Owner > owner)
							curr--;

						while (curr < n - 1 && parts[curr].Owner < owner)
							curr++;

						if (parts[curr].Owner != owner)
							return -1;
					}
				}
				else
				{
					if (start < curr)
						start = curr;
					else
					{
						while (curr > 0 && parts[curr].Owner > owner)
							curr--;

						while (curr < n - 1 && parts[curr].Owner < owner)
							curr++;

						if (parts[curr].Owner != owner)
							return -1;
					}
				}
				//i++;
			}
		}

		static public void mergeOstacleLists_old(LegBase forwardLeg, LegBase backwardLeg, out ObstacleContainer mergedObstacles)
		{
			int l = forwardLeg.Obstacles.Obstacles.Length;
			int k = forwardLeg.Obstacles.Parts.Length;

			int i = backwardLeg.Obstacles.Obstacles.Length;
			int j = backwardLeg.Obstacles.Parts.Length;

			ObstacleContainer tmpObstaclesFr;
			tmpObstaclesFr.Obstacles = new Obstacle[l];
			tmpObstaclesFr.Parts = new ObstacleData[k];
			Array.Copy(forwardLeg.Obstacles.Obstacles, tmpObstaclesFr.Obstacles, l);
			Array.Copy(forwardLeg.Obstacles.Parts, tmpObstaclesFr.Parts, k);
			tmpObstaclesFr.SortByGUID();

			ObstacleContainer tmpObstaclesBk;
			tmpObstaclesBk.Obstacles = new Obstacle[i];
			tmpObstaclesBk.Parts = new ObstacleData[j];
			Array.Copy(backwardLeg.Obstacles.Obstacles, tmpObstaclesBk.Obstacles, i);
			Array.Copy(backwardLeg.Obstacles.Parts, tmpObstaclesBk.Parts, j);
			tmpObstaclesBk.SortByGUID();

			int n = l + i;
			int m = k + j;

			List<Obstacle> mergedObstacleList = new List<Obstacle>();
			List<ObstacleData> mergedDataList = new List<ObstacleData>();

			foreach (var ff in tmpObstaclesFr.Obstacles)
				mergedObstacleList.Add(ff);

			foreach (var ff in tmpObstaclesFr.Parts)
				mergedDataList.Add(ff);

			for (int r = 0; r < j; r++)
			{
				ObstacleData obstBk = tmpObstaclesBk.Parts[r];
				int p = 0;

				while (p < k && tmpObstaclesBk.Obstacles[obstBk.Owner].Identifier.CompareTo(tmpObstaclesFr.Obstacles[tmpObstaclesFr.Parts[p].Owner].Identifier) > 0)
					p++;
				while (p >= 0 && tmpObstaclesBk.Obstacles[obstBk.Owner].Identifier.CompareTo(tmpObstaclesFr.Obstacles[tmpObstaclesFr.Parts[p].Owner].Identifier) < 0)
					p--;

				if (p >= k || p < 0)
				{
					mergedObstacleList.Add(tmpObstaclesBk.Obstacles[obstBk.Owner]);
					obstBk.Owner = mergedObstacleList.Count - 1;
					mergedDataList.Add(obstBk);

					//if (p >= k)
					//	mergedDataList.Add(obstBk);
					//else
					//	mergedDataList.Insert(0, obstBk);

					continue;
				}

				bool addToArray = true;

				while (p < k && tmpObstaclesBk.Obstacles[obstBk.Owner].Identifier.CompareTo(tmpObstaclesFr.Obstacles[tmpObstaclesFr.Parts[p].Owner].Identifier) == 0)
				{
					double dx = obstBk.pPtPrj.X - tmpObstaclesFr.Parts[p].pPtPrj.X;
					double dy = obstBk.pPtPrj.Y - tmpObstaclesFr.Parts[p].pPtPrj.Y;

					if (dx * dx + dy * dy < ARANMath.Epsilon_2Distance)
					{
						obstBk.MOC = Math.Max(obstBk.MOC, tmpObstaclesFr.Parts[p].MOC);
						obstBk.ReqH = Math.Max(obstBk.ReqH, tmpObstaclesFr.Parts[p].ReqH);
						obstBk.fSecCoeff = Math.Max(obstBk.fSecCoeff, tmpObstaclesFr.Parts[p].fSecCoeff);
						obstBk.Prima = obstBk.Prima || tmpObstaclesFr.Parts[p].Prima;
						mergedDataList[p] = obstBk;

						addToArray = false;
						break;
					}

					p++;
				}

				if (!addToArray)
					continue;

				mergedObstacleList.Add(tmpObstaclesBk.Obstacles[obstBk.Owner]);
				obstBk.Owner = mergedObstacleList.Count - 1;
				mergedDataList.Add(obstBk);
			}

			mergedObstacles.Obstacles = mergedObstacleList.ToArray();
			mergedObstacles.Parts = mergedDataList.ToArray();
		}

		static public void mergeObstacleLists(LegBase forwardLeg, LegBase backwardLeg, out ObstacleContainer mergedObstacles)
		{
			int l = forwardLeg.Obstacles.Obstacles.Length;
			int k = forwardLeg.Obstacles.Parts.Length;

			int n = l + forwardLeg.Obstacles.Obstacles.Length;
			int m = k + backwardLeg.Obstacles.Parts.Length;

			mergedObstacles.Obstacles = new Obstacle[n];
			mergedObstacles.Parts = new ObstacleData[m];

			Array.Copy(forwardLeg.Obstacles.Obstacles, mergedObstacles.Obstacles, l);
			Array.Copy(forwardLeg.Obstacles.Parts, mergedObstacles.Parts, k);

			Dictionary<Guid, int> map = new Dictionary<Guid, int>();

			int i, j;
			List<int>[] Parts = new List<int>[n];

			for (i = 0; i < l; i++)
			{
				Parts[i] = new List<int>();
				map[forwardLeg.Obstacles.Obstacles[i].Identifier] = i;
			}

			for (i = l; i < n; i++)
				Parts[i] = new List<int>();

			for (i = 0; i < k; i++)
				Parts[forwardLeg.Obstacles.Parts[i].Owner].Add(i);

			for ( i = 0; i < backwardLeg.Obstacles.Parts.Length; i++)
			{
				int oldOwner = backwardLeg.Obstacles.Parts[i].Owner, newOwner;

				if (map.TryGetValue(backwardLeg.Obstacles.Obstacles[oldOwner].Identifier, out newOwner))
				{
					bool addToArray = true;

					for (j = 0; j < mergedObstacles.Obstacles[newOwner].PartsNum; j++)
					//for (j = 0; j < Parts[newOwner].Count ; j++)
					{
						double dx = mergedObstacles.Parts[Parts[newOwner][j]].pPtPrj.X - backwardLeg.Obstacles.Parts[i].pPtPrj.X;
						double dy = mergedObstacles.Parts[Parts[newOwner][j]].pPtPrj.Y - backwardLeg.Obstacles.Parts[i].pPtPrj.Y;

						if (dx * dx + dy * dy < ARANMath.Epsilon_2Distance)
						{
							mergedObstacles.Parts[Parts[newOwner][j]].MOC = Math.Max(mergedObstacles.Parts[Parts[newOwner][j]].MOC, backwardLeg.Obstacles.Parts[i].MOC);
							mergedObstacles.Parts[Parts[newOwner][j]].ReqH = Math.Max(mergedObstacles.Parts[Parts[newOwner][j]].ReqH, backwardLeg.Obstacles.Parts[i].ReqH);
							mergedObstacles.Parts[Parts[newOwner][j]].fSecCoeff = Math.Max(mergedObstacles.Parts[Parts[newOwner][j]].fSecCoeff, backwardLeg.Obstacles.Parts[i].fSecCoeff);
							mergedObstacles.Parts[Parts[newOwner][j]].Prima = mergedObstacles.Parts[Parts[newOwner][j]].Prima || backwardLeg.Obstacles.Parts[i].Prima;

							addToArray = false;
							break;
						}
					}

					if (!addToArray)
						continue;
				}
				else
				{
					newOwner = l;
					l++;
					if (l >= mergedObstacles.Obstacles.Length)
						Array.Resize<Obstacle>(ref mergedObstacles.Obstacles, l + l);

					map[backwardLeg.Obstacles.Obstacles[oldOwner].Identifier] = newOwner;
					mergedObstacles.Obstacles[newOwner] = backwardLeg.Obstacles.Obstacles[oldOwner];
				}

				mergedObstacles.Parts[k] = backwardLeg.Obstacles.Parts[i];
				mergedObstacles.Parts[k].Owner = newOwner;

				mergedObstacles.Obstacles[newOwner].PartsNum++;
				Parts[newOwner].Add(k);

				k++;
				if (k >= mergedObstacles.Parts.Length)
					Array.Resize<ObstacleData>(ref mergedObstacles.Parts, k + k);
			}

			Array.Resize<Obstacle>(ref mergedObstacles.Obstacles, l);
			Array.Resize<ObstacleData>(ref mergedObstacles.Parts, k);
		}

		//static public void mergeOstacleLists_Old(LegBase forwardLeg, LegBase backwardLeg, out ObstacleContainer mergedObstacles)
		//{
		//	int l = forwardLeg.Obstacles.Obstacles.Length;
		//	int k = forwardLeg.Obstacles.Parts.Length;

		//	int n = l + backwardLeg.Obstacles.Obstacles.Length;
		//	int m = k + backwardLeg.Obstacles.Parts.Length;
		//	int i, j;

		//	Dictionary<Guid, int> forwardMap = new Dictionary<Guid, int>();
		//	for (i = 0; i < l; i++)
		//		forwardMap[forwardLeg.Obstacles.Obstacles[i].Identifier] = i;

		//	Dictionary<int, int> backwardMap = new Dictionary<int, int>();
		//	ObstacleContainer tmpObstacles;
		//	tmpObstacles.Parts = new ObstacleData[k];
		//	Array.Copy(forwardLeg.Obstacles.Parts, tmpObstacles.Parts, k);
		//	Array.Sort<ObstacleData>(tmpObstacles.Parts, ComparePartsByOwner);       //by owner	//0, k,

		//	//for (i = 0; i < k; i++)
		//	//	backwardMap[i] = backwardLeg.Obstacles.Parts[i].Owner;

		//	mergedObstacles.Obstacles = new Obstacle[n];
		//	Array.Copy(forwardLeg.Obstacles.Obstacles, mergedObstacles.Obstacles, l);

		//	mergedObstacles.Parts = new ObstacleData[m];
		//	Array.Copy(tmpObstacles.Parts, mergedObstacles.Parts, k);

		//	for (i = k; i < m; i++)
		//		mergedObstacles.Parts[i].Owner = -1;

		//	for (i = 0; i < backwardLeg.Obstacles.Parts.Length; i++)
		//	{
		//		int newOwner;
		//		int oldOwner = backwardLeg.Obstacles.Parts[i].Owner;
		//		Guid ownerGuid = backwardLeg.Obstacles.Obstacles[oldOwner].Identifier;

		//		if (forwardMap.TryGetValue(ownerGuid, out newOwner))
		//		{
		//			bool addToArray = true;


		//			for (j = 0; j < mergedObstacles.Obstacles[newOwner].PartsNum; j++)
		//			{
		//				double dx = mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].pPtPrj.X - backwardLeg.Obstacles.Parts[i].pPtPrj.X;
		//				double dy = mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].pPtPrj.Y - backwardLeg.Obstacles.Parts[i].pPtPrj.Y;

		//				if (dx * dx + dy * dy < ARANMath.Epsilon_2Distance)
		//				{
		//					mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].MOC = Math.Max(mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].MOC, backwardLeg.Obstacles.Parts[i].MOC);
		//					mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].ReqH = Math.Max(mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].ReqH, backwardLeg.Obstacles.Parts[i].ReqH);
		//					mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].fSecCoeff = Math.Max(mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].fSecCoeff, backwardLeg.Obstacles.Parts[i].fSecCoeff);
		//					mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].Prima = mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].Prima || backwardLeg.Obstacles.Parts[i].Prima;

		//					addToArray = false;
		//					break;
		//				}
		//			}

		//			if (!addToArray)
		//				continue;
		//		}
		//		//else
		//		//{
		//		//	newOwner = l;
		//		//	l++;
		//		//	if (l >= mergedObstacles.Obstacles.Length)
		//		//		Array.Resize(ref mergedObstacles.Obstacles, l + l);

		//		//	forwardMap[backwardLeg.Obstacles.Obstacles[oldOwner].Identifier] = newOwner;
		//		//	mergedObstacles.Obstacles[newOwner] = backwardLeg.Obstacles.Obstacles[oldOwner];
		//		//}

		//		//mergedObstacles.Parts[k] = backwardLeg.Obstacles.Parts[i];
		//		//mergedObstacles.Parts[k].Owner = newOwner;

		//		k++;
		//		if (k >= mergedObstacles.Parts.Length)
		//			Array.Resize<ObstacleData>(ref mergedObstacles.Parts, k + k);

		//		Array.Resize(ref mergedObstacles.Obstacles[newOwner].Parts, mergedObstacles.Obstacles[newOwner].PartsNum + 1);
		//		mergedObstacles.Obstacles[newOwner].Parts[mergedObstacles.Obstacles[newOwner].PartsNum] = k;
		//	}

		//	Array.Resize(ref mergedObstacles.Obstacles, l);
		//	Array.Resize(ref mergedObstacles.Parts, k);
		//}

		//static public void mergeOstacleLists_Old(LegBase forwardLeg, LegBase backwardLeg, out ObstacleContainer mergedObstacles)
		//{
		//	int l = forwardLeg.Obstacles.Obstacles.Length;
		//	int k = forwardLeg.Obstacles.Parts.Length;

		//	int n = l + backwardLeg.Obstacles.Obstacles.Length;
		//	int m = k + backwardLeg.Obstacles.Parts.Length;
		//	int i, j;

		//	Dictionary<Guid, int> forwardMap = new Dictionary<Guid, int>();
		//	for (i = 0; i < l; i++)
		//		forwardMap[forwardLeg.Obstacles.Obstacles[i].Identifier] = i;

		//	Dictionary<int, int> backwardMap = new Dictionary<int, int>();
		//	ObstacleContainer tmpObstacles;
		//	tmpObstacles.Parts = new ObstacleData[k];
		//	Array.Copy(forwardLeg.Obstacles.Parts, tmpObstacles.Parts, k);
		//	Array.Sort<ObstacleData>(tmpObstacles.Parts, ComparePartsByOwner);       //by owner	//0, k,

		//	//for (i = 0; i < k; i++)
		//	//	backwardMap[i] = backwardLeg.Obstacles.Parts[i].Owner;

		//	mergedObstacles.Obstacles = new Obstacle[n];
		//	Array.Copy(forwardLeg.Obstacles.Obstacles, mergedObstacles.Obstacles, l);

		//	mergedObstacles.Parts = new ObstacleData[m];
		//	Array.Copy(tmpObstacles.Parts, mergedObstacles.Parts, k);

		//	for (i = k; i < m; i++)
		//		mergedObstacles.Parts[i].Owner = -1;

		//	for (i = 0; i < backwardLeg.Obstacles.Parts.Length; i++)
		//		try
		//		{
		//			int newOwner;
		//			int oldOwner = backwardLeg.Obstacles.Parts[i].Owner;
		//			Guid ownerGuid = backwardLeg.Obstacles.Obstacles[oldOwner].Identifier;

		//			if (forwardMap.TryGetValue(ownerGuid, out newOwner))
		//			{
		//				bool addToArray = true;

		//				j = ownerStarts(ref mergedObstacles.Parts, newOwner, k);

		//				if (j >= 0)
		//					while (mergedObstacles.Parts[j].Owner == newOwner)
		//					{
		//						double dx = mergedObstacles.Parts[j].pPtPrj.X - backwardLeg.Obstacles.Parts[i].pPtPrj.X;
		//						double dy = mergedObstacles.Parts[j].pPtPrj.Y - backwardLeg.Obstacles.Parts[i].pPtPrj.Y;

		//						if (dx * dx + dy * dy < ARANMath.Epsilon_2Distance)
		//						{
		//							mergedObstacles.Parts[j].MOC = Math.Max(mergedObstacles.Parts[j].MOC, backwardLeg.Obstacles.Parts[i].MOC);
		//							mergedObstacles.Parts[j].ReqH = Math.Max(mergedObstacles.Parts[j].ReqH, backwardLeg.Obstacles.Parts[i].ReqH);
		//							mergedObstacles.Parts[j].fSecCoeff = Math.Max(mergedObstacles.Parts[j].fSecCoeff, backwardLeg.Obstacles.Parts[i].fSecCoeff);
		//							mergedObstacles.Parts[j].Prima = mergedObstacles.Parts[j].Prima || backwardLeg.Obstacles.Parts[i].Prima;

		//							addToArray = false;
		//							break;
		//						}

		//						j++;
		//					}


		//				//for (int j = 0; j < mergedObstacles.Obstacles[newOwner].PartsNum; j++)
		//				//{
		//				//	double dx = mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].pPtPrj.X - backwardLeg.Obstacles.Parts[i].pPtPrj.X;
		//				//	double dy = mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].pPtPrj.Y - backwardLeg.Obstacles.Parts[i].pPtPrj.Y;

		//				//	if (dx * dx + dy * dy < ARANMath.Epsilon_2Distance)
		//				//	{
		//				//		mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].MOC = Math.Max(mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].MOC, backwardLeg.Obstacles.Parts[i].MOC);
		//				//		mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].ReqH = Math.Max(mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].ReqH, backwardLeg.Obstacles.Parts[i].ReqH);
		//				//		mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].fSecCoeff = Math.Max(mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].fSecCoeff, backwardLeg.Obstacles.Parts[i].fSecCoeff);
		//				//		mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].Prima = mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].Prima || backwardLeg.Obstacles.Parts[i].Prima;

		//				//		addToArray = false;
		//				//		break;
		//				//	}
		//				//}

		//				if (!addToArray)
		//					continue;
		//			}
		//			else
		//			{
		//				newOwner = l;
		//				l++;
		//				if (l >= mergedObstacles.Obstacles.Length)
		//					Array.Resize(ref mergedObstacles.Obstacles, l + l);

		//				forwardMap[backwardLeg.Obstacles.Obstacles[oldOwner].Identifier] = newOwner;
		//				mergedObstacles.Obstacles[newOwner] = backwardLeg.Obstacles.Obstacles[oldOwner];
		//			}

		//			mergedObstacles.Parts[k] = backwardLeg.Obstacles.Parts[i];
		//			mergedObstacles.Parts[k].Owner = newOwner;

		//			k++;
		//			if (k >= mergedObstacles.Parts.Length)
		//				Array.Resize<ObstacleData>(ref mergedObstacles.Parts, k + k);

		//			//Array.Resize(ref mergedObstacles.Obstacles[newOwner].Parts, mergedObstacles.Obstacles[newOwner].PartsNum + 1);
		//			//mergedObstacles.Obstacles[newOwner].Parts[mergedObstacles.Obstacles[newOwner].PartsNum] = k;
		//		}
		//		catch
		//		{
		//			i = i;
		//		}

		//	Array.Resize(ref mergedObstacles.Obstacles, l);
		//	Array.Resize(ref mergedObstacles.Parts, k);
		//}

		public static bool PriorPostFixTolerance(MultiPolygon pPolygon, Point pPtPrj, double fDir, out double PriorDist, out double PostDist)
		{
			PriorDist = -1.0;
			PostDist = -1.0;
			MultiLineString ptIntersection;

			LineString pCutterPolyline = new LineString();
			pCutterPolyline.Add(ARANFunctions.LocalToPrj(pPtPrj, fDir, 1000000.0));
			pCutterPolyline.Add(ARANFunctions.LocalToPrj(pPtPrj, fDir, -1000000.0));

		    var pCutterMultiLine = new MultiLineString {pCutterPolyline};

			try
			{
				var pTopological = new Geometries.Operators.JtsGeometryOperators();
				Geometry pIntersection = pTopological.Intersect(pCutterMultiLine, pPolygon);
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

		//static bool circlePPP(Point a, Point b, Point c, out Point o, out double r)
		//{
		//	Point ba = new Point(b.X - a.X, b.Y - a.Y);
		//	Point ca = new Point(c.X - a.X, c.Y - a.Y);
		//	double p = ba.X * ca.Y - ba.Y * ca.X;

		//	o = new Point();

		//	if (p == 0)
		//	{
		//		r = 0;
		//		return false;
		//	}

		//	p += p;
		//	double a2 = a.X * a.X + a.Y * a.Y;
		//	double b2 = b.X * b.X + b.Y * b.Y - a2;
		//	double c2 = c.X * c.X + c.Y * c.Y - a2;

		//	o.X = (b2 * ca.Y - c2 * ba.Y) / p;
		//	o.Y = (c2 * ba.X - b2 * ca.X) / p;
		//	r = ARANMath.Hypot(a.X - o.X, a.Y - o.Y);
		//	return true;
		//}

		//public static bool minCircleAroundPoints(List<Point> P, out Point o, out double r)
		//{
		//	int n = P.Count;

		//	if (n < 3)
		//	{
		//		r = 0.0;
		//		if (n == 0)
		//		{
		//			o = null;
		//			return false;
		//		}

		//		if (n == 1)
		//		{
		//			o = (Point)P[0].Clone();
		//			return true;
		//		}

		//		o = new Point(0.5 * (P[0].X + P[1].X), 0.5 * (P[0].Y + P[1].Y));
		//		r = ARANMath.Hypot(P[0].X - P[1].X, P[0].Y - P[1].Y);
		//		return true;
		//	}

		//	int i, im = 0;

		//	double max = 0, t;
		//	Point p0 = P[0];

		//	for (i = 1; i < n; ++i)
		//	{
		//		t = ARANMath.Hypot(P[i].X - p0.X, P[i].Y - p0.Y);

		//		if (max < t)
		//		{
		//			max = t;
		//			im = i;
		//		}
		//	}

		//	if (im == 0)
		//	{
		//		o = p0;
		//		r = 0;
		//		return true;
		//	}

		//	int np = 2;
		//	int[] ip = new int[3];

		//	ip[0] = 0;
		//	ip[1] = im;

		//	o = new Point();
		//	o.X = 0.5 * (p0.X + P[im].X);
		//	o.Y = 0.5 * (p0.Y + P[im].Y);

		//	double q = 0.25 * max, s;

		//	for (; ; )
		//	{
		//		max = 0.0;
		//		for (i = 0; i < n; ++i)
		//		{
		//			t = ARANMath.Hypot(P[i].X - o.X, P[i].Y - o.Y);

		//			if (max < t)
		//			{
		//				max = t;
		//				im = i;
		//			}
		//		}

		//		if (max <= q || im == ip[0] || im == ip[1])
		//			break;

		//		Point pm = P[im];
		//		int km = 0;

		//		s = ARANMath.Hypot(pm.X - P[ip[0]].X, pm.Y - P[ip[0]].Y);
		//		t = ARANMath.Hypot(pm.X - P[ip[1]].X, pm.Y - P[ip[1]].Y);

		//		if (s < t)
		//		{
		//			s = t;
		//			km = 1;
		//		}

		//		Point v = new Point();
		//		if (np == 2)
		//		{
		//			s *= 0.25;
		//			int iTmp = ip[km];
		//			ip[km] = ip[0];
		//			ip[0] = iTmp;

		//			v.X = 0.5 * (pm.X + P[ip[0]].X);
		//			v.Y = 0.5 * (pm.Y + P[ip[0]].Y);

		//			if (ARANMath.Hypot(v.X - P[ip[1]].X, v.Y - P[ip[1]].Y) > s)
		//			{
		//				np = 3;
		//				ip[2] = im;
		//				circlePPP(P[ip[0]], P[ip[1]], pm, out v, out s);
		//			}
		//			else
		//			{
		//				ip[1] = im;
		//			}
		//		}
		//		else
		//		{
		//			if (im == ip[2])
		//				break;
		//			t = ARANMath.Hypot(pm.X - P[ip[2]].X, pm.Y - P[ip[2]].Y);

		//			if (s < t)
		//			{
		//				s = t;
		//				km = 2;
		//			}

		//			s *= 0.25;
		//			int iTmp = ip[km];
		//			ip[km] = ip[0];
		//			ip[0] = iTmp;

		//			v.X = 0.5 * (pm.X + P[ip[0]].X);
		//			v.Y = 0.5 * (pm.Y + P[ip[0]].Y);

		//			double q1 = ARANMath.Hypot(v.X - P[ip[1]].X, v.Y - P[ip[1]].Y);
		//			double q2 = ARANMath.Hypot(v.X - P[ip[2]].X, v.Y - P[ip[2]].Y);
		//			if (q1 < q2)
		//			{
		//				ip[1] = ip[2];
		//				q1 = q2;
		//			}

		//			if (q1 > s)
		//			{
		//				circlePPP(P[ip[0]], P[ip[1]], pm, out v, out s);
		//				ip[2] = im;
		//			}
		//			else
		//			{
		//				np = 2;
		//				ip[1] = im;
		//			}
		//		}

		//		if (s <= q)
		//			break;

		//		q = s;
		//		o = v;
		//	}

		//	r = 2 * q;			//r = Math.Sqrt(q);
		//	return true;
		//}

		//public static double CalcMaxRadius()
		//{
		//	int i, n = GlobalVars.RWYList.Length;
		//	if (n <= 1)
		//		return 0;

		//	Point ptCentr;
		//	List<Point> pLineStr = new List<Point>();

		//	for (i = 0; i < n; i++)
		//	{
		//		ptCentr = GlobalVars.RWYList[i].pPtPrj[eRWY.ptTHR];
		//		pLineStr.Add(ptCentr);
		//	}

		//	if (pLineStr.Count < 3)
		//		return ARANMath.Hypot(pLineStr[0].X - pLineStr[1].X, pLineStr[0].Y - pLineStr[1].Y);

		//	double result;
		//	minCircleAroundPoints(pLineStr, out ptCentr, out result);

		//	return 2 * result;
		//}

	}
}

/*
─────────────────────────▄▀▄  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─▀█▀█▄  
─────────────────────────█──█──█  
─────────────────────────█▄▄█──▀█  
────────────────────────▄█──▄█▄─▀█  
────────────────────────█─▄█─█─█─█  
────────────────────────█──█─█─█─█  
────────────────────────█──█─█─█─█  
────▄█▄──▄█▄────────────█──▀▀█─█─█  
──▄█████████────────────▀█───█─█▄▀  
─▄███████████────────────██──▀▀─█  
▄█████████████────────────█─────█  
██████████───▀▀█▄─────────▀█────█  
████████───▀▀▀──█──────────█────█  
██████───────██─▀█─────────█────█  
████──▄──────────▀█────────█────█ Look dude,
███──█──────▀▀█───▀█───────█────█ a good code!
███─▀─██──────█────▀█──────█────█  
███─────────────────▀█─────█────█  
███──────────────────█─────█────█  
███─────────────▄▀───█─────█────█  
████─────────▄▄██────█▄────█────█  
████────────██████────█────█────█  
█████────█──███████▀──█───▄█▄▄▄▄█  
██▀▀██────▀─██▄──▄█───█───█─────█  
██▄──────────██████───█───█─────█  
─██▄────────────▄▄────█───█─────█  
─███████─────────────▄█───█─────█  
──██████─────────────█───█▀─────█  
──▄███████▄─────────▄█──█▀──────█  
─▄█─────▄▀▀▀█───────█───█───────█  
▄█────────█──█────▄███▀▀▀▀──────█  
█──▄▀▀────────█──▄▀──█──────────█  
█────█─────────█─────█──────────█  
█────────▀█────█─────█─────────██  
█───────────────█──▄█▀─────────█  
█──────────██───█▀▀▀───────────█  
█───────────────█──────────────█  
█▄─────────────██──────────────█  
─█▄────────────█───────────────█  
──██▄────────▄███▀▀▀▀▀▄────────█  
─█▀─▀█▄────────▀█──────▀▄──────█  
─█────▀▀▀▀▄─────█────────▀─────█
------------------------------------------------
 
░░░░░░▄▀▒▒▒▒░░░░▒▒▒▒▒▒▒▒▒▒▒▒▒█
░░░░░█▒▒▒▒░░░░▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒█
░░░░█▒▒▄▀▀▀▀▀▄▄▒▒▒▒▒▒▒▒▒▄▄▀▀▀▀▀▀▄
░░▄▀▒▒▒▄█████▄▒█▒▒▒▒▒▒▒█▒▄█████▄▒█
░█▒▒▒▒▐██▄████▌▒█▒▒▒▒▒█▒▐██▄████▌▒█
▀▒▒▒▒▒▒▀█████▀▒▒█▒░▄▒▄█▒▒▀█████▀▒▒▒█
▒▒▐▒▒▒░░░░▒▒▒▒▒█▒░▒▒▀▒▒█▒▒▒▒▒▒▒▒▒▒▒▒█
▒▌▒▒▒░░░▒▒▒▒▒▄▀▒░▒▄█▄█▄▒▀▄▒▒▒▒▒▒▒▒▒▒▒▌
▒▌▒▒▒▒░▒▒▒▒▒▒▀▄▒▒█▌▌▌▌▌█▄▀▒▒▒▒▒▒▒▒▒▒▒▐
▒▐▒▒▒▒▒▒▒▒▒▒▒▒▒▌▒▒▀███▀▒▌▒▒▒▒▒▒▒▒▒▒▒▒▌
▀▀▄▒▒▒▒▒▒▒▒▒▒▒▌▒▒▒▒▒▒▒▒▒▐▒▒▒▒▒▒▒▒▒▒▒█
▀▄▒▀▄▒▒▒▒▒▒▒▒▐▒▒▒▒▒▒▒▒▒▄▄▄▄▒▒▒▒▒▒▄▄▀
▒▒▀▄▒▀▄▀▀▀▄▀▀▀▀▄▄▄▄▄▄▄▀░░░░▀▀▀▀▀▀
▒▒▒▒▀▄▐▒▒▒▒▒▒▒▒▒▒▒▒▒▐
 

─────────▄──────────────▄
────────▌▒█───────────▄▀▒▌
────────▌▒▒▀▄───────▄▀▒▒▒▐
───────▐▄▀▒▒▀▀▀▀▄▄▄▀▒▒▒▒▒▐
─────▄▄▀▒▒▒▒▒▒▒▒▒▒▒█▒▒▄█▒▐
───▄▀▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▀██▀▒▌
──▐▒▒▒▄▄▄▒▒▒▒▒▒▒▒▒▒▒▒▒▀▄▒▒▌
──▌▒▒▐▄█▀▒▒▒▒▄▀█▄▒▒▒▒▒▒▒█▒▐
─▐▒▒▒▒▒▒▒▒▒▒▒▌██▀▒▒▒▒▒▒▒▒▀▄▌
─▌▒▀▄██▄▒▒▒▒▒▒▒▒▒▒▒░░░░▒▒▒▒▌
─▌▀▐▄█▄█▌▄▒▀▒▒▒▒▒▒░░░░░░▒▒▒▐
▐▒▀▐▀▐▀▒▒▄▄▒▄▒▒▒▒▒░░░░░░▒▒▒▒▌
▐▒▒▒▀▀▄▄▒▒▒▄▒▒▒▒▒▒░░░░░░▒▒▒▐
─▌▒▒▒▒▒▒▀▀▀▒▒▒▒▒▒▒▒░░░░▒▒▒▒▌
─▐▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▐
──▀▄▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▄▒▒▒▒▌
────▀▄▒▒▒▒▒▒▒▒▒▒▄▄▄▀▒▒▒▒▄▀
───▐▀▒▀▄▄▄▄▄▄▀▀▀▒▒▒▒▒▄▄▀
──▐▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▀▀


------------------------------------------------ 

 ▔▔▔▔▔▔▔▔▔▔▔╲
▕╮╭┻┻╮╭┻┻╮╭▕╮╲
▕╯┃╭╮┃┃╭╮┃╰▕╯╭▏
▕╭┻┻┻┛┗┻┻┛  ▕  ╰▏
▕╰━━━┓┈┈┈╭╮▕╭╮▏
▕╭╮╰┳┳┳┳╯╰╯▕╰╯▏
▕╰╯┈┗┛┗┛┈╭╮▕╮┈▏

-------------------------------------------------
╭━━━━━━━━━━━━━━━━━━━━━━━━━━-╮
┃                    ════════                   ●  ┃
┃                      LG                          ┃
┃                                                  ┃
┃██████████████████████████████████████████████████┃
┃█  Missed call from John Cena      .:| [█] 17:28 █┃
┃█  ━━━━━━━━━━━━━━━━━━━━━━━   █┃
┃█                                                █┃
┃█                                                █┃
┃█        █     ████           ██     ██          █┃
┃█       ██        █   ██     █  █   █  █         █┃
┃█        █       █             █     ██          █┃
┃█        █      █     ██      █     █  █         █┃
┃█        █     █             ████    ██          █┃
┃█                                                █┃
┃█                             Wen 12 Jan.        █┃
┃█  ━━━━━━━━━━━━━━━━━━━━━━━━  █┃
┃█                                                █┃
┃█      13°C					                   █┃
┃█      21°|12°				                       █┃
┃█      Tangier | Morocco                         █┃
┃█      Partly cloudy                             █┃
┃█                                                █┃
┃█                                                █┃
┃█                                                █┃
┃█                                                █┃
┃█                                                █┃
┃█   ╭━(1)╮   ╭━(3)╮  ╭-(9)╮  ╭━━ ╮     █┃
┃█   ┃     ┃   ┃ █   ┃  ┃ V  ┃  ┃ ● ● ┃    █┃
┃█   ╰━━╯    ╰━━╯   ╰━━╯   ╰━━╯     █┃
┃█    Phone     Contacts   Messaging    Apps      █┃
┃█                                                █┃
┃█  ━━━━━━━━━━━━━━━━━━━━━━━━  █┃
┃█                 ╭━━━━-╮                   █┃
┃█    ╭-━-╮     ┃        ┃         <━╮      █┃
┃█    ┃ == ┃     ╰━━━━╯           ━╯      █┃
┃█                                                █┃
┃███████████████████████████████████████████████████
-------------------------------------------------
 
  ~~~~~~▄▌▐▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▌
  ~~~▄▄██▌█ ░░                      ░░░ ▌
  ▄▄▄▌▐██▌█ ░                         ░ ▌
  ███████▌█▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▌
 ▀▀(@)▀▀▀▀▀▀▀(@)(@)▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀(@)(@)
─────────────────────────────────────────────
 
┻┳|
┳┻| _
┻┳| •.• ) -Daddy,are all the copy and paste people gone 
┳┻|⊂ﾉ
┻┳|
 
 
 
 /﹋\
(҂`_´) -NOPE,SON GET BACK IN YOUR BEDROOM
<,︻╦╤─ ҉
 /﹋\
  
╭━━━╮╱╱╱╱╱╭╮
┃╭━╮┃╱╱╱╱╱┃┃
┃╰━━┳╮╭┳━━┫┃╭┳━━╮
╰━━╮┃╰╯┃╭╮┃╰╯┫┃━┫
┃╰━╯┃┃┃┃╰╯┃╭╮┫┃━┫
╰━━━┻┻┻┻━━┻╯╰┻━━╯
╭╮╭╮╭╮╱╱╱╱╱╱╱╭╮
┃┃┃┃┃┃╱╱╱╱╱╱╱┃┃
┃┃┃┃┃┣━━┳━━┳━╯┃
┃╰╯╰╯┃┃━┫┃━┫╭╮┃
╰╮╭╮╭┫┃━┫┃━┫╰╯┃
╱╰╯╰╯╰━━┻━━┻━━╯
╭━━━╮╱╱╱╱╱╱╱╱╱╱╱╱╱╭╮╱╱╱╱╱╱╱╭╮
┃╭━━╯╱╱╱╱╱╱╱╱╱╱╱╱╱┃┃╱╱╱╱╱╱╱┃┃
┃╰━━┳╮╭┳━━┳━┳╮╱╭┳━╯┣━━┳╮╱╭╮┃┃
┃╭━━┫╰╯┃┃━┫╭┫┃╱┃┃╭╮┃╭╮┃┃╱┃┃╰╯
┃╰━━╋╮╭┫┃━┫┃┃╰━╯┃╰╯┃╭╮┃╰━╯┃╭╮
╰━━━╯╰╯╰━━┻╯╰━╮╭┻━━┻╯╰┻━╮╭╯╰╯
╱╱╱╱╱╱╱╱╱╱╱╱╭━╯┃╱╱╱╱╱╱╭━╯┃
╱╱╱╱╱╱╱╱╱╱╱╱╰━━╯╱╱╱╱╱╱╰━━╯ 

╱╱┃┃╱╱┃┃╱╱╱╭╯╰╮
╭━╯┣╮╭┫╰━┳━┻╮╭╋━━┳━━╮
┃╭╮┃┃┃┃╭╮┃━━┫┃┃┃━┫╭╮┃
┃╰╯┃╰╯┃╰╯┣━━┃╰┫┃━┫╰╯┃				BEAAAST
╰━━┻━━┻━━┻━━┻━┻━━┫╭━╯
╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱┃┃
╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╰╯

 */
