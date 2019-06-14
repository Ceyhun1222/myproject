using System;
using System.Collections.Generic;
//using Aran.PANDA.CRMWall.Properties;
using System.Runtime.InteropServices;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.PANDA.Common;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace Aran.PANDA.CRMWall
{
	[System.Runtime.InteropServices.ComVisible(false)]
	class minCircle
	{
		static bool circlePPP(Point a, Point b, Point c, out Point o, out double r)
		{
			Point ba = new Point(b.X - a.X, b.Y - a.Y);
			Point ca = new Point(c.X - a.X, c.Y - a.Y);
			double p = ba.X * ca.Y - ba.Y * ca.X;

			o = new Point();

			if (p == 0)
			{
				r = 0;
				return false;
			}

			p += p;
			double a2 = a.X * a.X + a.Y * a.Y;
			double b2 = b.X * b.X + b.Y * b.Y - a2;
			double c2 = c.X * c.X + c.Y * c.Y - a2;

			o.X = (b2 * ca.Y - c2 * ba.Y) / p;
			o.Y = (c2 * ba.X - b2 * ca.X) / p;
			r = ARANMath.Hypot(a.X - o.X, a.Y - o.Y);
			return true;
		}

		public static Ring circlePPP(Point a, Point b, Point c)
		{
			Point ba = new Point(b.X - a.X, b.Y - a.Y);
			Point ca = new Point(c.X - a.X, c.Y - a.Y);
			double p = ba.X * ca.Y - ba.Y * ca.X;

			if (p == 0)
				return new Ring();

			p += p;
			double a2 = a.X * a.X + a.Y * a.Y;
			double b2 = b.X * b.X + b.Y * b.Y - a2;
			double c2 = c.X * c.X + c.Y * c.Y - a2;

			Point o = new Point();
			o.X = (b2 * ca.Y - c2 * ba.Y) / p;
			o.Y = (c2 * ba.X - b2 * ca.X) / p;

			double r = ARANMath.Hypot(a.X - o.X, a.Y - o.Y);
			return ARANFunctions.CreateCirclePrj(o, r);
		}

		public static bool minCircleAroundPoints(List<Point> P, out Point o, out double r)
		{
			int n = P.Count;

			if (n < 3)
			{
				r = 0.0;
				if (n == 0)
				{
					o = null;
					return false;
				}

				if (n == 1)
				{
					o = (Point)P[0].Clone();
					return true;
				}

				o = new Point(0.5 * (P[0].X + P[1].X), 0.5 * (P[0].Y + P[1].Y));
				r = ARANMath.Hypot(P[0].X - P[1].X, P[0].Y - P[1].Y);
				return true;
			}

			int i, im = 0;

			double max = 0, t;
			Point p0 = P[0];

			for (i = 1; i < n; ++i)
			{
				t = ARANMath.Hypot(P[i].X - p0.X, P[i].Y - p0.Y);

				if (max < t)
				{
					max = t;
					im = i;
				}
			}

			if (im == 0)
			{
				o = p0;
				r = 0;
				return true;
			}

			int np = 2;
			int[] ip = new int[3];

			ip[0] = 0;
			ip[1] = im;

			o = new Point();
			o.X = 0.5 * (p0.X + P[im].X);
			o.Y = 0.5 * (p0.Y + P[im].Y);

			double q = 0.25 * max, s;

			for (; ; )
			{
				max = 0.0;
				for (i = 0; i < n; ++i)
				{
					t = ARANMath.Hypot(P[i].X - o.X, P[i].Y - o.Y);

					if (max < t)
					{
						max = t;
						im = i;
					}
				}

				if (max <= q || im == ip[0] || im == ip[1])
					break;

				Point pm = P[im];
				int km = 0;

				s = ARANMath.Hypot(pm.X - P[ip[0]].X, pm.Y - P[ip[0]].Y);
				t = ARANMath.Hypot(pm.X - P[ip[1]].X, pm.Y - P[ip[1]].Y);

				if (s < t)
				{
					s = t;
					km = 1;
				}

				Point v = new Point();
				if (np == 2)
				{
					s *= 0.25;
					int iTmp = ip[km];
					ip[km] = ip[0];
					ip[0] = iTmp;

					v.X = 0.5 * (pm.X + P[ip[0]].X);
					v.Y = 0.5 * (pm.Y + P[ip[0]].Y);

					if (ARANMath.Hypot(v.X - P[ip[1]].X, v.Y - P[ip[1]].Y) > s)
					{
						np = 3;
						ip[2] = im;
						circlePPP(P[ip[0]], P[ip[1]], pm, out v, out s);
					}
					else
					{
						ip[1] = im;
					}
				}
				else
				{
					if (im == ip[2])
						break;
					t = ARANMath.Hypot(pm.X - P[ip[2]].X, pm.Y - P[ip[2]].Y);

					if (s < t)
					{
						s = t;
						km = 2;
					}

					s *= 0.25;
					int iTmp = ip[km];
					ip[km] = ip[0];
					ip[0] = iTmp;

					v.X = 0.5 * (pm.X + P[ip[0]].X);
					v.Y = 0.5 * (pm.Y + P[ip[0]].Y);

					double q1 = ARANMath.Hypot(v.X - P[ip[1]].X, v.Y - P[ip[1]].Y);
					double q2 = ARANMath.Hypot(v.X - P[ip[2]].X, v.Y - P[ip[2]].Y);
					if (q1 < q2)
					{
						ip[1] = ip[2];
						q1 = q2;
					}

					if (q1 > s)
					{
						circlePPP(P[ip[0]], P[ip[1]], pm, out v, out s);
						ip[2] = im;
					}
					else
					{
						np = 2;
						ip[1] = im;
					}
				}

				if (s <= q)
					break;

				q = s;
				o = v;
			}

			r = 2 * q;			//r = Math.Sqrt(q);
			return true;
		}

		public static Ring minCircleAroundPoints(List<Point> P)
		{
			int n = P.Count;

			if (n < 3)
			{
				if (n == 0) return new Ring();
				if (n == 1) return ARANFunctions.CreateCirclePrj(P[0], 0);
				Point ptCenter = new Point(0.5 * (P[0].X + P[1].X), 0.5 * (P[0].Y + P[1].Y));

				return ARANFunctions.CreateCirclePrj(ptCenter, ARANMath.Hypot(P[0].X - P[1].X, P[0].Y - P[1].Y));
			}

			Point p0 = P[0];
			int i, im = 0;
			double max = 0, t;

			for (i = 1; i < n; ++i)
			{
				Point pI = P[i];
				t = ARANMath.Hypot(pI.X - p0.X, pI.Y - p0.Y); // qmod ( pT );

				if (max < t)
				{
					max = t;
					im = i;
				}
			}

			if (im == 0)
				return ARANFunctions.CreateCirclePrj(p0, 0);

			int np = 2;
			int[] ip = new int[3];

			ip[0] = 0;
			ip[1] = im;

			Point o = new Point();
			o.X = 0.5 * (p0.X + P[im].X);
			o.Y = 0.5 * (p0.Y + P[im].Y);

			double q = 0.25 * max, s;

			for (; ; )
			{
				max = 0;
				for (i = 0; i < n; ++i)
				{
					Point pI = P[i];
					t = ARANMath.Hypot(pI.X - o.X, pI.Y - o.Y);

					if (max < t)
					{
						max = t;
						im = i;
					}
				}

				if (max <= q || im == ip[0] || im == ip[1])
					break;

				Point pm = P[im];
				int km = 0;

				s = ARANMath.Hypot(pm.X - P[ip[0]].X, pm.Y - P[ip[0]].Y);
				t = ARANMath.Hypot(pm.X - P[ip[1]].X, pm.Y - P[ip[1]].Y);
				if (s < t)
				{
					s = t;
					km = 1;
				}

				Point v = new Point();
				if (np == 2)
				{
					s *= 0.25;
					int iTmp = ip[km];
					ip[km] = ip[0];
					ip[0] = iTmp;

					v.X = 0.5 * (pm.X + P[ip[0]].X);
					v.Y = 0.5 * (pm.Y + P[ip[0]].Y);

					if (ARANMath.Hypot(v.X - P[ip[1]].X, v.Y - P[ip[1]].Y) > s)
					{
						np = 3;
						ip[2] = im;
						circlePPP(P[ip[0]], P[ip[1]], pm, out v, out s);
					}
					else
					{
						ip[1] = im;
					}
				}
				else
				{
					if (im == ip[2]) break;
					t = ARANMath.Hypot(pm.X - P[ip[2]].X, pm.Y - P[ip[2]].Y);

					if (s < t)
					{
						s = t;
						km = 2;
					}

					s *= 0.25;
					int iTmp = ip[km];
					ip[km] = ip[0];
					ip[0] = iTmp;

					v.X = 0.5 * (pm.X + P[ip[0]].X);
					v.Y = 0.5 * (pm.Y + P[ip[0]].Y);

					double q1 = ARANMath.Hypot(v.X - P[ip[1]].X, v.Y - P[ip[1]].Y);
					double q2 = ARANMath.Hypot(v.X - P[ip[2]].X, v.Y - P[ip[2]].Y);
					if (q1 < q2)
					{
						ip[1] = ip[2];
						q1 = q2;
					}

					if (q1 > s)
					{
						circlePPP(P[ip[0]], P[ip[1]], pm, out v, out s);
						ip[2] = im;
					}
					else
					{
						np = 2;
						ip[1] = im;
					}
				}
				if (s <= q) break;
				q = s;
				o = v;
			}

			return ARANFunctions.CreateCirclePrj(o, Math.Sqrt(q));
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public static class Functions
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

		[DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
		public static extern IntPtr GetWindow(IntPtr hWnd, int uCmd);

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

		public static string DD2DMSstring(double value)
		{
			int Deg = (int)value;
			int Min = (int)((value - Deg) * 60.0);
			double Sec = System.Math.Round((value - Deg - Min / 60.0) * 3600.0, 4);

			string DegStr = Deg.ToString("00");
			string MinStr = Min.ToString("00");
			string SecStr = Sec.ToString("00.0000");
			return DegStr + MinStr + SecStr;
		}

		public static void ExportPointObstacles(StreamWriter tw, List<int> data, double ILSDir, ref Obstacle[] ObstacleList, bool createWalls = false)
		{
			char iDelimiter = ';';
			string sDescrip = "Exported by \"PANDA " + GlobalVars.thisAssemName.Version + "\"";
			string sRemark = "Exported by \"PANDA\" softvare";
			string sDatum = "WGS84";

			//========================
			string sTmp, IDvalue;
			double fTmp;
			DateTime D;
			int n = data.Count;

			for (int i = 0; i < n; i++)
			{
				Obstacle obst = ObstacleList[data[i]];

				Point pPtGeo = (Point)obst.pGeomGeo;
				if (pPtGeo.IsEmpty)
					continue;

				//Nature ===============================================
				//NatureField
				char bNature = 'M';
				if (obst.TypeName == "NATURAL_HIGHPOINT")
					bNature = 'N';

				tw.Write(bNature);
				tw.Write(iDelimiter);

				//Type ===============================================
				IDvalue = obst.TypeName;
				if (IDvalue.Length > 25)
					IDvalue = IDvalue.Substring(0, 25);

				tw.Write(IDvalue);
				tw.Write(iDelimiter);

				//ID ===============================================
				IDvalue = obst.UnicalName;
				if (IDvalue.Length > 24)
					IDvalue = IDvalue.Substring(0, 25);
				tw.Write(IDvalue);
				tw.Write(iDelimiter);

				//Lat ===============================================
				fTmp = pPtGeo.Y;
				if (fTmp > 0)
					sTmp = "N";
				else
					sTmp = "S";

				tw.Write(sTmp);
				tw.Write(iDelimiter);

				sTmp = DD2DMSstring(System.Math.Abs(fTmp));

				tw.Write(sTmp);
				tw.Write(iDelimiter);

				//Lon ===============================================
				fTmp = pPtGeo.X;
				if (fTmp > 0)
					sTmp = "E";
				else
					sTmp = "W";

				tw.Write(sTmp);
				tw.Write(iDelimiter);

				sTmp = DD2DMSstring(System.Math.Abs(fTmp));

				if (Math.Abs(fTmp) < 100) sTmp = "0" + sTmp;

				tw.Write(sTmp);
				tw.Write(iDelimiter);

				//Datum ===============================================
				tw.Write(sDatum);
				tw.Write(iDelimiter);

				//Elev_M ===============================================
				tw.Write(obst.Height.ToString("00000.0"));
				tw.Write(iDelimiter);

				//Height_M ===============================================
				tw.Write(obst.Height.ToString("00000.0"));
				tw.Write(iDelimiter);

				//P1 ===============================================
				if (createWalls)
					tw.Write("P" + (i + 1));
				else
					tw.Write("P1");

				tw.Write(iDelimiter);

				//HorAcc ===============================================
				tw.Write('1');	//obst.HorAccuracy
				tw.Write(iDelimiter);

				//VerAcc ===============================================
				tw.Write('A');
				tw.Write(iDelimiter);

				//Verify Date ===============================================
				//CurrField = ObsInPutDateField
				//D = pFeature.Value(CurrField)
				D = DateTime.Now;

				sTmp = D.ToString("yyyyMMdd");
				tw.Write(sTmp);
				tw.Write(iDelimiter);

				//Remark ===============================================
				//CurrField = ObsRemarkField
				//sTmp = pFeature.Value(CurrField)
				//sRemark = "Nil";
				tw.Write(sRemark);
				tw.Write(iDelimiter);

				//Source Type ===============================================
				tw.Write("Survey");
				tw.Write(iDelimiter);

				//Source Descript ===============================================
				tw.Write("PANDA Software");
				tw.Write(iDelimiter);

				//Source Date ===============================================
				sTmp = D.ToString("yyyyMMdd");
				tw.Write(sTmp);
				tw.Write(iDelimiter);

				//Description ===============================================
				//CurrField = ObsDescripField
				//sTmp = pFeature.Value(CurrField)
				//sDescrip = "Nil";
				tw.Write(sDescrip);
				tw.Write(iDelimiter);

				//Transfer Date ===============================================
				sTmp = D.ToString("yyyyMMdd");
				tw.Write(sTmp);
				tw.WriteLine();
				//sCrLf ===============================================
			}
		}

		public static void ExportPolygonalObstacles(StreamWriter tw, SpanTable data, double ILSDir, ref Obstacle[] ObstacleList, bool createWalls = false)
		{
			char iDelimiter = ';';
			string sTmp, IDvalue;
			double fTmp;
			DateTime D;

			string sDescrip = "Exported by \"PANDA " + GlobalVars.thisAssemName.Version + "\"";
			string sRemark = "Exported by \"PANDA\" softvare";
			string sDatum = "WGS84";

			double dir = ILSDir - 0.5 * Math.PI;

			//========================

			foreach (SlicerBase.Span spn in data)
			{
				const double maxDist = 60.0;
				Point pPtGeo0 = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(spn.p0);
				if (pPtGeo0.IsEmpty)
					continue;

				Point pPtGeo1 = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(spn.p1);
				if (pPtGeo1.IsEmpty)
					continue;

				int n;
				Point[] pPtGeo;
				double wallLength = ARANFunctions.ReturnDistanceInMeters(spn.p0, spn.p1);

				if (createWalls || wallLength < maxDist)
				{
					n = 2;
					pPtGeo = new Point[2] { pPtGeo0, pPtGeo1 };
				}
				else
				{
					n = (int)Math.Ceiling(wallLength / maxDist) + 1;
					double h = wallLength / (n - 1);

					pPtGeo = new Point[n];
					pPtGeo[0] = pPtGeo0;
					pPtGeo[n - 1] = pPtGeo1;

					for (int j = 1; j < n - 1; j++)
					{
						Point ptt = ARANFunctions.LocalToPrj(spn.p0, dir, j * h);

						pPtGeo[j] = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(ptt);
					}
				}

				for (int j = 0; j < n; j++)
				{
					Obstacle obst = ObstacleList[spn.owner];

					//Nature ===============================================
					//NatureField
					char bNature = 'M';											//obst.Nature
					if (obst.TypeName == "NATURAL_HIGHPOINT")
						bNature = 'N';

					tw.Write(bNature);
					tw.Write(iDelimiter);

					//Type ===============================================
					IDvalue = obst.TypeName;
					if (IDvalue.Length > 25)
						IDvalue = IDvalue.Substring(0, 25);

					tw.Write(IDvalue);
					tw.Write(iDelimiter);

					//ID ===============================================
					IDvalue = obst.UnicalName;
					if (IDvalue.Length > 24)
						IDvalue = IDvalue.Substring(0, 25);
					tw.Write(IDvalue);
					tw.Write(iDelimiter);

					//Lat ===============================================
					fTmp = pPtGeo[j].Y;
					if (fTmp > 0)
						sTmp = "N";
					else
						sTmp = "S";

					tw.Write(sTmp);
					tw.Write(iDelimiter);

					sTmp = DD2DMSstring(System.Math.Abs(fTmp));

					tw.Write(sTmp);
					tw.Write(iDelimiter);

					//Lon ===============================================
					fTmp = pPtGeo[j].X;
					if (fTmp > 0)
						sTmp = "E";
					else
						sTmp = "W";

					tw.Write(sTmp);
					tw.Write(iDelimiter);

					sTmp = DD2DMSstring(System.Math.Abs(fTmp));

					if (Math.Abs(fTmp) < 100) sTmp = "0" + sTmp;

					tw.Write(sTmp);
					tw.Write(iDelimiter);

					//Datum ===============================================
					tw.Write(sDatum);
					tw.Write(iDelimiter);

					//Elev_M ===============================================
					tw.Write(obst.Height.ToString("00000.0"));
					tw.Write(iDelimiter);

					//Height_M ===============================================
					tw.Write(obst.Height.ToString("00000.0"));
					tw.Write(iDelimiter);

					//P1 ===============================================
					if (createWalls)
						tw.Write("P" + (j + 1));
					else
						tw.Write("P1");

					tw.Write(iDelimiter);

					//HorAcc ===============================================
					tw.Write('1');	//obst.HorAccuracy
					tw.Write(iDelimiter);

					//VerAcc ===============================================
					tw.Write('A');
					tw.Write(iDelimiter);

					//Verify Date ===============================================
					//CurrField = ObsInPutDateField
					//D = pFeature.Value(CurrField)
					D = DateTime.Now;

					sTmp = D.ToString("yyyyMMdd");
					tw.Write(sTmp);
					tw.Write(iDelimiter);

					//Remark ===============================================
					//CurrField = ObsRemarkField
					//sTmp = pFeature.Value(CurrField)
					//sRemark = "Nil";
					tw.Write(sRemark);
					tw.Write(iDelimiter);

					//Source Type ===============================================
					tw.Write("Survey");
					tw.Write(iDelimiter);

					//Source Descript ===============================================
					tw.Write("PANDA Software");
					tw.Write(iDelimiter);

					//Source Date ===============================================
					sTmp = D.ToString("yyyyMMdd");
					tw.Write(sTmp);
					tw.Write(iDelimiter);

					//Description ===============================================
					//CurrField = ObsDescripField
					//sTmp = pFeature.Value(CurrField)
					//sDescrip = "Nil";
					tw.Write(sDescrip);
					tw.Write(iDelimiter);

					//Transfer Date ===============================================
					sTmp = D.ToString("yyyyMMdd");
					tw.Write(sTmp);
					tw.WriteLine();
					//sCrLf ===============================================
				}
				//++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			}
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

		public static void GetObstaclesByPolygon(out Obstacle[] ObstacleList, MultiPolygon pPoly)
		{
			int n = GlobalVars.ObstacleList.Obstacles.Length;
			ObstacleList = new Obstacle[n];

			if (n == 0)
				return;

			GeometryOperators pGeoOp = new GeometryOperators() { CurrentGeometry = pPoly };

			//MessageBox.Show("Start");
			//Stopwatch st = new Stopwatch();
			//st.Start();

			int j = 0;
			for (int i = 0; i < n; i++)
			{
				if (!pGeoOp.Disjoint(GlobalVars.ObstacleList.Obstacles[i].pGeomPrj))
				{
					Geometry currGeom;

					switch (GlobalVars.ObstacleList.Obstacles[i].pGeomPrj.Type)
					{
						default:
							currGeom = GlobalVars.ObstacleList.Obstacles[i].pGeomPrj; break;
						case GeometryType.Line:
						case GeometryType.LineSegment:
						case GeometryType.LineString:
						case GeometryType.MultiLineString:
						case GeometryType.MultiPoint:
						case GeometryType.MultiPolygon:
						case GeometryType.Polygon:
						case GeometryType.Ring:
						case GeometryType.GeometryCollection:
							currGeom = pGeoOp.Intersect(GlobalVars.ObstacleList.Obstacles[i].pGeomPrj); break;
					}

					ObstacleList[j] = GlobalVars.ObstacleList.Obstacles[i];
					ObstacleList[j].pGeomPrj = currGeom;
					j++;
				}
			}
			//st.Stop();
			//MessageBox.Show(st.Elapsed.ToString());

			System.Array.Resize<Obstacle>(ref ObstacleList, j);
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

	}
}
