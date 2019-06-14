using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using System;
using System.Runtime.InteropServices;
//using System.IO;

namespace Aran.PANDA.RNAV.PBNAPVBaroVNav
{
	public static class Functions
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool InsertMenu(IntPtr hMenu, int uPosition, int uFlags, int uIDNewItem, string lpNewItem);

		public static void TextBoxFloat(ref char KeyChar, string BoxText)
		{
			if (KeyChar < 32)
				return;

			char DecSep = (1.1).ToString().ToCharArray()[1];

			if (((KeyChar < '0') || KeyChar > '9') && KeyChar != DecSep)
				KeyChar = '\0';
			else if (KeyChar == DecSep && BoxText.IndexOf(DecSep) >= 0)
				KeyChar = '\0';
		}

		public static void TextBoxFloatWithSign(ref char KeyChar, string BoxText)
		{
			if (KeyChar < 32)
				return;

			char DecSep = (1.1).ToString().ToCharArray()[1];

			if (((KeyChar < '0') || KeyChar > '9') && KeyChar != DecSep && KeyChar != '-' && KeyChar != '+')
				KeyChar = '\0';
			else if (KeyChar == DecSep && BoxText.IndexOf(DecSep) >= 0)
				KeyChar = '\0';
			else if (KeyChar == '-' && BoxText.IndexOf('-') >= 0)
				KeyChar = '\0';
			else if (KeyChar == '+' && BoxText.IndexOf('+') >= 0)
				KeyChar = '\0';
		}

		public static void TextBoxFloatWithMinus(ref char KeyChar, string BoxText)
		{
			if (KeyChar < 32)
				return;

			char DecSep = (1.1).ToString().ToCharArray()[1];

			if (((KeyChar < '0') || KeyChar > '9') && KeyChar != DecSep && KeyChar != '-' && KeyChar != '+')
				KeyChar = '\0';
			else if (KeyChar == DecSep && BoxText.IndexOf(DecSep) >= 0)
				KeyChar = '\0';
			else if (KeyChar == '-' && BoxText != "")
				KeyChar = '\0';
		}

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

		public static double IntersectW_FASPlanes(D3DPlane PlaneW, D3DPlane PlaneFAS)
		{
			return (PlaneFAS.D - PlaneW.D) / (PlaneW.A - PlaneFAS.A);
		}

		public static double CallcObstacleReqOCA(ObstacleContainer ObstList, double xOrigin, double hLost, double VPA, double MAPDG, LegBase MASegment)
		{
			double CotVPA = 1.0 / Math.Tan(VPA);
			double CotZ = 1.0 / MAPDG;

			double MaxOCH = hLost;
			int n = ObstList.Parts.Length;

			for (int i = 0; i < n; i++)
			{
				double z = ObstList.Parts[i].hSurface - (1.0 - ObstList.Parts[i].fSecCoeff) * ObstList.Parts[i].MOC;
				double h_ = z + ObstList.Parts[i].hPenet;
				double ha = (h_ * CotZ + ObstList.Parts[i].hSurface + (ObstList.Parts[i].Dist - xOrigin)) / (CotZ + CotVPA);

				ObstList.Parts[i].EffectiveHeight = ha;

				if (ObstList.Parts[i].hPenet > 0.0)
					ObstList.Parts[i].ReqOCH = Math.Min(ObstList.Parts[i].EffectiveHeight, h_) + hLost;
				else
					ObstList.Parts[i].ReqOCH = 0.0;

				if (ObstList.Parts[i].hPenet > 0.0 && ObstList.Parts[i].ReqOCH > MaxOCH)
					MaxOCH = ObstList.Parts[i].ReqOCH;

				if (ObstList.Parts[i].hPenet > 0.0 && ObstList.Parts[i].Plane != (int)Plane.MissedApproachSurface)
					ObstList.Parts[i].ReqH = ObstList.Parts[i].Height + hLost * ObstList.Parts[i].fSecCoeff;
				else
					ObstList.Parts[i].ReqH = ObstList.Parts[i].ReqOCH;
			}

			return MaxOCH;
		}

		public static void CreateFASPlanes(Point RWYTHRPrj, double hFAP, double hFASS, double hMaxSS, double VPA, double FAS, LegBase ISegment, LegBase FASegment, out D3DPolygone[] FASPlanes)
		{
			double TanVPA = Math.Tan(VPA);
			double CoTanVPA = 1.0 / TanVPA;

			double x = ISegment.EndFIX.ATT + (hMaxSS - GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value) * CoTanVPA;
			double InterFASX = (hFAP - GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value - GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value) / FAS + x;
			double InterSideX = (hFAP - hFASS - GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value - GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value) / FAS + x;

			double x0 = FASegment.EndFIX.ATT + (hMaxSS - GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value) * CoTanVPA;
			double x1 = (hFAP - GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value) * CoTanVPA;

			double y0 = 50000.0;

			Ring ring = new Ring();

			Point pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x0, y0);
			ring.Add(pt0);

			pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x0, -y0);
			ring.Add(pt0);

			pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x1, -y0);
			ring.Add(pt0);

			pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x1, y0);
			ring.Add(pt0);

			Polygon ExtendPoly = new Polygon
			{
				ExteriorRing = ring
			};

			bool haveFASIAsurfase = InterSideX < x1;

			Polygon FASextendPoly = null;

			if (haveFASIAsurfase)
			{
				ring.Clear();
				pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x0, y0);
				ring.Add(pt0);

				pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x0, -y0);
				ring.Add(pt0);

				pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -InterFASX, -y0);
				ring.Add(pt0);

				pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -InterFASX, y0);
				ring.Add(pt0);

				FASextendPoly = new Polygon
				{
					ExteriorRing = ring
				};

				FASPlanes = new D3DPolygone[7];
			}
			else
				FASPlanes = new D3DPolygone[4];

			GeometryOperators geoOp = new GeometryOperators();

			MultiPolygon PrimPoly = (MultiPolygon)geoOp.UnionGeometry(ISegment.PrimaryArea, FASegment.PrimaryArea);
			MultiPolygon FullPoly = (MultiPolygon)geoOp.UnionGeometry(ISegment.FullArea, FASegment.FullArea);

			Point ptLeftFAS = null, ptRightFAS = null;
			Point ptLeftSide = null, ptRightSide = null;

			Geometry LeftPoly, RightPoly;
			LineString ls = new LineString();
			MultiLineString mls;

			MultiPolygon tmpPoly = (MultiPolygon)geoOp.Intersect(PrimPoly, ExtendPoly);

			if (haveFASIAsurfase)
			{
				pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -InterFASX, 10000.0);
				ls.Add(pt0);

				pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -InterFASX, -10000.0);
				ls.Add(pt0);

				if (InterFASX + ARANMath.EpsilonDistance < x1)
				{
					mls = (MultiLineString)geoOp.Intersect(ls, tmpPoly);

					ptLeftFAS = mls[0][0];
					ptRightFAS = mls[0][1];

					geoOp.Cut(tmpPoly, ls, out LeftPoly, out RightPoly);

					FASPlanes[1].Poly = ((MultiPolygon)LeftPoly)[0];
					FASPlanes[4].Poly = ((MultiPolygon)RightPoly)[0];
				}
				else
				{
					FASPlanes[1].Poly = tmpPoly[0];
					FASPlanes[4].Poly = null;
					tmpPoly = (MultiPolygon)geoOp.Intersect(PrimPoly, FASextendPoly);

					mls = (MultiLineString)geoOp.Intersect(ls, tmpPoly);
					ptLeftFAS = mls[0][0];
					ptRightFAS = mls[0][1];

				}

				ls.Clear();
			}
			else
				FASPlanes[1].Poly = tmpPoly[0];

			tmpPoly = (MultiPolygon)geoOp.Intersect(FullPoly, ExtendPoly);
			FASPlanes[FASPlanes.Length - 1].Poly = tmpPoly[0];

			if (haveFASIAsurfase)
			{
				pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -InterSideX, 10000.0);
				ls.Add(pt0);

				pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -InterSideX, -10000.0);
				ls.Add(pt0);

				mls = (MultiLineString)geoOp.Intersect(ls, tmpPoly);
				ls.Clear();

				//GlobalVars.gAranGraphics.DrawPointWithText(mls[0][0], -1, "ptLeftSide");
				//GlobalVars.gAranGraphics.DrawPointWithText(mls[0][1], -1, "ptRightSide");
				//GlobalVars.gAranGraphics.DrawLineString(ls, -1, 2);
				//LegBase.ProcessMessages();

				ptLeftSide = mls[0][0];
				ptRightSide = mls[0][1];
			}

			pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x1 - 100, 0);
			ls.Add(pt0);

			pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x0 + 100, 0);
			ls.Add(pt0);

			geoOp.Cut(tmpPoly, ls, out LeftPoly, out RightPoly);

			if (haveFASIAsurfase)
			{
				ls.Clear();

				double dir = ARANFunctions.ReturnAngleInRadians(ptLeftFAS, ptLeftSide);

				ls.Add(ARANFunctions.LocalToPrj(ptLeftSide, dir, 100.0));
				ls.Add(ptLeftFAS);

				dir = ARANFunctions.ReturnAngleInRadians(ptRightFAS, ptRightSide);

				ls.Add(ptRightFAS);
				ls.Add(ARANFunctions.LocalToPrj(ptRightSide, dir, 100.0));
			}

			tmpPoly = (MultiPolygon)geoOp.Difference(LeftPoly, PrimPoly);
			if (haveFASIAsurfase)
			{
				Geometry LeftPoly1, RightPoly1;
				geoOp.Cut(tmpPoly, ls, out LeftPoly1, out RightPoly1);

				FASPlanes[0].Poly = ((MultiPolygon)LeftPoly1)[0];
				FASPlanes[3].Poly = ((MultiPolygon)RightPoly1)[0];
			}
			else
				FASPlanes[0].Poly = tmpPoly[0];

			tmpPoly = (MultiPolygon)geoOp.Difference(RightPoly, PrimPoly);
			if (haveFASIAsurfase)
			{
				geoOp.Cut(tmpPoly, ls, out LeftPoly, out RightPoly);

				FASPlanes[2].Poly = ((MultiPolygon)LeftPoly)[0];
				FASPlanes[5].Poly = ((MultiPolygon)RightPoly)[0];
			}
			else
				FASPlanes[2].Poly = tmpPoly[0];

			//ptOrigin = LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x0, 0);
			//FASPlanes[1].Plane.Origin = ptOrigin;
			FASPlanes[1].Plane.A = FAS;
			FASPlanes[1].Plane.B = 0;
			FASPlanes[1].Plane.C = -1;
			FASPlanes[1].Plane.D = -x0 * FAS;

			//====================================================================================================================
			int i;
			Point pt1;
			double z;

			ring = FASPlanes[0].Poly.ExteriorRing;
			for (i = 0; i < ring.Count; i++)
			{
				pt1 = ring[i];
				pt0 = ARANFunctions.PrjToLocal(RWYTHRPrj, RWYTHRPrj.M, pt1);

				pt1.T = 0;

				z = FASPlanes[1].Plane.D - pt0.X * FASPlanes[1].Plane.A;
				if (geoOp.GetDistance(pt1, PrimPoly) > ARANMath.EpsilonDistance)
				{
					z += hMaxSS;
					pt1.T = 1;
				}

				pt1.Z = z;
				ring[i] = pt1;
			}

			FASPlanes[0].Poly.ExteriorRing = ring;
			//====================================================================================================================

			ring = FASPlanes[1].Poly.ExteriorRing;
			for (i = 0; i < ring.Count; i++)
			{
				pt1 = ring[i];
				pt0 = ARANFunctions.PrjToLocal(RWYTHRPrj, RWYTHRPrj.M, pt1);

				pt1.Z = FASPlanes[1].Plane.D - pt0.X * FASPlanes[1].Plane.A;
				ring[i] = pt1;
			}

			FASPlanes[1].Poly.ExteriorRing = ring;
			//====================================================================================================================
			ring = FASPlanes[2].Poly.ExteriorRing;
			for (i = 0; i < ring.Count; i++)
			{
				pt1 = ring[i];
				pt0 = ARANFunctions.PrjToLocal(RWYTHRPrj, RWYTHRPrj.M, pt1);

				pt1.T = 0;
				z = FASPlanes[1].Plane.D - pt0.X * FASPlanes[1].Plane.A;
				if (geoOp.GetDistance(pt1, PrimPoly) > ARANMath.EpsilonDistance)
				{
					z += hMaxSS;
					pt1.T = 1;
				}

				pt1.Z = z;
				ring[i] = pt1;
			}
			FASPlanes[2].Poly.ExteriorRing = ring;
			//	ReArrangePoly(FASPlanes[0].Poly);
			//	ReArrangePoly(FASPlanes[2].Poly);
		}

		public static void CreateHorzPlanes(Point RWYTHRPrj, double xOrigin, double hMaxSS, double VPA, LegBase FASegment, LegBase MASegment, out D3DPolygone[] HorzPlanes)
		{
			double x0 = MASegment.StartFIX.ATT + (hMaxSS - GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value) / Math.Tan(VPA);
			double x1 = xOrigin;

			Point ptC = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x1, 0);

			double y0 = 50000.0;

			Ring ring = new Ring();
			Point pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x0, y0);
			ring.Add(pt0);

			pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x0, -y0);
			ring.Add(pt0);

			pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x1, -y0);
			ring.Add(pt0);

			pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x1, y0);
			ring.Add(pt0);

			Polygon ExtendPoly = new Polygon
			{
				ExteriorRing = ring
			};

			HorzPlanes = new D3DPolygone[4];
			GeometryOperators geoOp = new GeometryOperators();

			MultiPolygon PrimPoly = (MultiPolygon)geoOp.UnionGeometry(FASegment.PrimaryArea, MASegment.PrimaryArea);
			MultiPolygon FullPoly = (MultiPolygon)geoOp.UnionGeometry(FASegment.FullArea, MASegment.FullArea);

			HorzPlanes[1].Plane.A = x1;
			HorzPlanes[1].Plane.B = x0;

			MultiPolygon tmpPoly = (MultiPolygon)geoOp.Intersect(PrimPoly, ExtendPoly);
			HorzPlanes[1].Poly = tmpPoly[0];

			LineString ls = new LineString();

			MultiLineString cutter = new MultiLineString();

			pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x0 - 100, 0);
			ls.Add(pt0);
			pt0 = ARANFunctions.LocalToPrj(ptC, RWYTHRPrj.M, 100, 0);
			ls.Add(pt0);

			cutter.Add(ls);

			tmpPoly = (MultiPolygon)geoOp.Intersect(FullPoly, ExtendPoly);
			HorzPlanes[3].Poly = tmpPoly[0];

			geoOp.Cut(tmpPoly, cutter, out Geometry LeftPoly, out Geometry RightPoly);

			tmpPoly = (MultiPolygon)geoOp.Difference(LeftPoly, PrimPoly);
			HorzPlanes[0].Poly = tmpPoly[0];

			tmpPoly = (MultiPolygon)geoOp.Difference(RightPoly, PrimPoly);
			HorzPlanes[2].Poly = tmpPoly[0];

			// ==============================================================================
			int i;
			Point pt1;
			//double d0 = fTAS * GlobalVars.constants.Pansops[ePANSOPSData.dpPilotTolerance].Value;
			//double d1 = fTAS * GlobalVars.constants.Pansops[ePANSOPSData.arSOCdelayTime].Value;
			double MA_InterMOC = GlobalVars.constants.Pansops[ePANSOPSData.arMA_InterMOC].Value;
			double D = MA_InterMOC;

			double Slope = (hMaxSS - MA_InterMOC) / (x0 - x1);

			ring = HorzPlanes[0].Poly.ExteriorRing;
			for (i = 0; i < ring.Count; i++)
			{
				pt1 = ring[i];
				if (geoOp.GetDistance(pt1, PrimPoly) > ARANMath.EpsilonDistance)
				{
					pt0 = ARANFunctions.PrjToLocal(ptC, RWYTHRPrj.Z, HorzPlanes[0].Poly.ExteriorRing[i]);
					pt1.Z = D - pt0.X * Slope;
					pt1.T = 1;
				}
				else
				{
					pt1.Z = 0;
					pt1.T = 0;
				}
				ring[i] = pt1;
			}
			HorzPlanes[0].Poly.ExteriorRing = ring;

			// ==============================================================================
			ring = HorzPlanes[1].Poly.ExteriorRing;
			for (i = 0; i < ring.Count; i++)
			{
				//pt0 = ARANFunctions.PrjToLocal(RWYTHRPrj, RWYTHRPrj.M, ring[I]);
				pt1 = ring[i];
				pt1.Z = 0.0;
				ring[i] = pt1;
			}
			HorzPlanes[1].Poly.ExteriorRing = ring;
			// ==============================================================================
			ring = HorzPlanes[2].Poly.ExteriorRing;
			for (i = 0; i < ring.Count; i++)
			{
				pt1 = ring[i];

				if (geoOp.GetDistance(ring[i], PrimPoly) > ARANMath.EpsilonDistance)
				{
					pt0 = ARANFunctions.PrjToLocal(ptC, RWYTHRPrj.M, ring[i]);
					pt1.Z = D - pt0.X * Slope;
					pt1.T = 1;

				}
				else
				{
					pt1.Z = 0;
					pt1.T = 0;
				}

				ring[i] = pt1;
			}

			HorzPlanes[2].Poly.ExteriorRing = ring;
			// ==============================================================================
		}

		public static void CreateMASPlanes(Point RWYTHRPrj, double xOrigin, double modelRadius, double MAPDG, LegBase FASegment, LegBase MASegment, out D3DPolygone[] MASPlanes)
		{
			double x0 = xOrigin;
			double x1 = modelRadius;
			//=================================================================   0

			double y0 = 50000.0;

			Ring ring = new Ring();

			Point pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x0, y0);
			ring.Add(pt0);

			pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x0, -y0);
			ring.Add(pt0);
			pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, x1, -y0);
			ring.Add(pt0);
			pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, x1, y0);
			ring.Add(pt0);

			Polygon ExtendPoly = new Polygon
			{
				ExteriorRing = ring
			};

			GeometryOperators geoOp = new GeometryOperators();
			MASPlanes = new D3DPolygone[4];

			MultiPolygon PrimPoly = (MultiPolygon)geoOp.UnionGeometry(FASegment.PrimaryArea, MASegment.PrimaryArea);
			MultiPolygon FullPoly = (MultiPolygon)geoOp.UnionGeometry(FASegment.FullArea, MASegment.FullArea);
			MultiPolygon tmpPoly = (MultiPolygon)geoOp.Intersect(PrimPoly, ExtendPoly);

			MASPlanes[1].Poly = tmpPoly[0];

			LineString ls = new LineString();
			pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, x1 + 100, 0);
			ls.Add(pt0);

			pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x0 - 100, 0);
			ls.Add(pt0);

			MultiLineString cutter = new MultiLineString();
			cutter.Add(ls);

			tmpPoly = (MultiPolygon)geoOp.Intersect(FullPoly, ExtendPoly);
			MASPlanes[3].Poly = tmpPoly[0];

			geoOp.Cut(tmpPoly, cutter, out Geometry LeftPoly, out Geometry RightPoly);

			tmpPoly = (MultiPolygon)geoOp.Difference(LeftPoly, PrimPoly);
			MASPlanes[0].Poly = tmpPoly[0];

			tmpPoly = (MultiPolygon)geoOp.Difference(RightPoly, PrimPoly);
			MASPlanes[2].Poly = tmpPoly[0];

			//ptOrigin = LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x0, 0);
			//FASPlanes[1].Plane.Origin = ptOrigin;

			MASPlanes[1].Plane.A = -MAPDG;
			MASPlanes[1].Plane.B = 0;
			MASPlanes[1].Plane.C = -1;
			MASPlanes[1].Plane.D = x0 * MAPDG;

			//====================================================================================================================
			//double TanVPA = Math.Tan(VPA);
			//double d0 = fTAS * GlobalVars.constants.Pansops[ePANSOPSData.dpPilotTolerance].Value;
			//double d1 = fTAS * GlobalVars.constants.Pansops[ePANSOPSData.arSOCdelayTime].Value;
			double MA_InterMOC = GlobalVars.constants.Pansops[ePANSOPSData.arMA_InterMOC].Value;
			double D = MASPlanes[1].Plane.D;
			int i;
			Point pt1;

			ring = MASPlanes[0].Poly.ExteriorRing;
			for (i = 0; i < ring.Count; i++)
			{
				pt1 = ring[i];
				pt0 = ARANFunctions.PrjToLocal(RWYTHRPrj, RWYTHRPrj.M, pt1);

				pt1.T = 0;
				double z = D + pt0.X * MAPDG;

				if (geoOp.GetDistance(MASPlanes[0].Poly.ExteriorRing[i], PrimPoly) > ARANMath.EpsilonDistance)
				{
					z += MA_InterMOC;
					pt1.T = 1;
				}

				pt1.Z = z;
				ring[i] = pt1;
			}

			MASPlanes[0].Poly.ExteriorRing = ring;

			//====================================================================================================================
			ring = MASPlanes[1].Poly.ExteriorRing;

			for (i = 0; i < ring.Count; i++)
			{
				pt1 = ring[i];
				pt0 = ARANFunctions.PrjToLocal(RWYTHRPrj, RWYTHRPrj.M, pt1);

				pt1.Z = D + pt0.X * MAPDG;
				ring[i] = pt1;
			}
			MASPlanes[1].Poly.ExteriorRing = ring;

			//====================================================================================================================
			ring = MASPlanes[2].Poly.ExteriorRing;

			for (i = 0; i < ring.Count; i++)
			{
				pt1 = ring[i];
				pt0 = ARANFunctions.PrjToLocal(RWYTHRPrj, RWYTHRPrj.M, pt1);

				pt1.T = 0;
				double z = D + pt0.X * MAPDG;

				if (geoOp.GetDistance(MASPlanes[2].Poly.ExteriorRing[i], PrimPoly) > ARANMath.EpsilonDistance)
				{
					z += MA_InterMOC;
					pt1.T = 1;
				}

				pt1.Z = z;
				ring[i] = pt1;
			}

			MASPlanes[2].Poly.ExteriorRing = ring;
		}

		//public static void CreateOFZPlanes(Point RWYTHRPrj, aircraftCategory cat, double H, double VPA, double ATT, out D3DPolygone[] OFZPlanes)
		//{
		//	double x0 = -(H - 27 + 1.2) * 50;
		//	double x1 = -60;

		//	switch (cat)
		//	{
		//		case aircraftCategory.acA:
		//		case aircraftCategory.acB:
		//			x1 = -900;
		//			break;
		//		case aircraftCategory.acC:
		//			x1 = -1100;
		//			break;
		//		case aircraftCategory.acD:
		//		case aircraftCategory.acDL:
		//		case aircraftCategory.acE:
		//		case aircraftCategory.acH:
		//			x1 = -1400;
		//			break;
		//		default:
		//			break;
		//	}

		//	if (H > 900 || ARANMath.RadToDeg(VPA) > 3.2)
		//	{
		//		double HL = 115.0;

		//		double TAS = GlobalVars.constants.AircraftCategory[aircraftCategoryData.VatMin][cat];
		//		double gamma = 0.08 * (2.56 * 0.3048);
		//		double Vw = 10.0 * 1.852 / 3.6;// kt
		//		double RDH = RWYTHRPrj.Z;

		//		x1 = Math.Min(x1, (HL - RDH) / Math.Tan(VPA) - (ATT + 2 * TAS * Math.Sin(VPA) / gamma * (TAS + Vw)));
		//	}

		//	// a) CAT A and B: Xz = –900 m
		//	// b) CAT C: Xz = –1 100 m
		//	// c) CAT D: Xz = –1 400 m

		//	double x2 = 1800;
		//	double x3 = (H + 59.94) * 30.03;

		//	double y0 = 60;
		//	double y1 = 3.003 * (21.18 - 1.2 + 27);
		//	double y2 = 3.003 * (H + 21.18 - 0.02 * 60);
		//	double y3 = 3.003 * (H + 19.98);

		//	Point pt1 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, x1, 0);
		//	pt1.Z = 0;

		//	//pt3 = LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, x2, 0);
		//	//pt3.Z = 0;
		//	OFZPlanes = new D3DPolygone[8];

		//	int i, n = 7;

		//	for (i = 0; i <= n; i++)
		//		OFZPlanes[i].Poly = new Polygon();

		//	//=================================================================   0
		//	Ring ring = new Ring();

		//	Point pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, x1, -y0);
		//	pt0.Z = 0;
		//	ring.Add(pt0);

		//	//GlobalVars.gAranGraphics.DrawPointWithText(pt0, -1, "-1");
		//	//LegBase.ProcessMessages();

		//	pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, x1, y0);
		//	ring.Add(pt0);

		//	pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, x2, y0);
		//	ring.Add(pt0);

		//	pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, x2, -y0);
		//	ring.Add(pt0);

		//	OFZPlanes[(int)OFZPlane.ZeroPlane].Poly.ExteriorRing = ring;
		//	OFZPlanes[(int)OFZPlane.ZeroPlane].Plane.Origin = pt1;

		//	OFZPlanes[(int)OFZPlane.ZeroPlane].Plane.X = 0;
		//	OFZPlanes[(int)OFZPlane.ZeroPlane].Plane.Y = 0;
		//	OFZPlanes[(int)OFZPlane.ZeroPlane].Plane.Z = 1;

		//	OFZPlanes[(int)OFZPlane.ZeroPlane].Plane.A = 0;
		//	OFZPlanes[(int)OFZPlane.ZeroPlane].Plane.B = 0;
		//	OFZPlanes[(int)OFZPlane.ZeroPlane].Plane.C = -1;
		//	OFZPlanes[(int)OFZPlane.ZeroPlane].Plane.D = 0;

		//	//=================================================================    1
		//	pt0.Z = H - 27;
		//	ring = new Ring();

		//	pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, x0, -y0);
		//	ring.Add(pt0);

		//	pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, x0, y0);
		//	ring.Add(pt0);

		//	ring.Add(OFZPlanes[(int)OFZPlane.ZeroPlane].Poly.ExteriorRing[1]);
		//	ring.Add(OFZPlanes[(int)OFZPlane.ZeroPlane].Poly.ExteriorRing[0]);

		//	OFZPlanes[(int)OFZPlane.InnerApproachPlane].Poly.ExteriorRing = ring;
		//	OFZPlanes[(int)OFZPlane.InnerApproachPlane].Plane.Origin = pt1;

		//	OFZPlanes[(int)OFZPlane.InnerApproachPlane].Plane.A = 0.02;
		//	OFZPlanes[(int)OFZPlane.InnerApproachPlane].Plane.B = 0;
		//	OFZPlanes[(int)OFZPlane.InnerApproachPlane].Plane.C = -1;
		//	OFZPlanes[(int)OFZPlane.InnerApproachPlane].Plane.D = -1.2;

		//	//=================================================================    2
		//	pt0.Z = H;
		//	ring = new Ring();

		//	ring.Add(OFZPlanes[(int)OFZPlane.InnerApproachPlane].Poly.ExteriorRing[1]);

		//	pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, x0, y1);
		//	ring.Add(pt0);

		//	pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, x1, y2);
		//	ring.Add(pt0);

		//	ring.Add(OFZPlanes[(int)OFZPlane.InnerApproachPlane].Poly.ExteriorRing[2]);

		//	OFZPlanes[(int)OFZPlane.InnerTransition1lPlane].Poly.ExteriorRing = ring;
		//	OFZPlanes[(int)OFZPlane.InnerTransition1lPlane].Plane.Origin = OFZPlanes[(int)OFZPlane.InnerTransition1lPlane].Poly.ExteriorRing[3];
		//	//OFZPlanes[(int)OFZPlane.InnerTransition1lPlane].Plane.Origin.Z = 0;

		//	OFZPlanes[(int)OFZPlane.InnerTransition1lPlane].Plane.A = 0.02;
		//	OFZPlanes[(int)OFZPlane.InnerTransition1lPlane].Plane.B = -0.333;
		//	OFZPlanes[(int)OFZPlane.InnerTransition1lPlane].Plane.C = -1;
		//	OFZPlanes[(int)OFZPlane.InnerTransition1lPlane].Plane.D = -21.18;

		//	//=================================================================    3
		//	ring = new Ring();

		//	ring.Add(OFZPlanes[(int)OFZPlane.ZeroPlane].Poly.ExteriorRing[1]);
		//	ring.Add(OFZPlanes[(int)OFZPlane.InnerTransition1lPlane].Poly.ExteriorRing[2]);

		//	pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, x3, y3);
		//	ring.Add(pt0);

		//	ring.Add(OFZPlanes[(int)OFZPlane.ZeroPlane].Poly.ExteriorRing[2]);

		//	OFZPlanes[(int)OFZPlane.InnerTransition2lPlane].Poly.ExteriorRing = ring;
		//	OFZPlanes[(int)OFZPlane.InnerTransition2lPlane].Plane.Origin = OFZPlanes[(int)OFZPlane.InnerTransition2lPlane].Poly.ExteriorRing[3];
		//	//OFZPlanes[(int)OFZPlane.InnerTransition2lPlane].Plane.Origin.Z = 0;

		//	OFZPlanes[(int)OFZPlane.InnerTransition2lPlane].Plane.A = 0;
		//	OFZPlanes[(int)OFZPlane.InnerTransition2lPlane].Plane.B = -0.333;
		//	OFZPlanes[(int)OFZPlane.InnerTransition2lPlane].Plane.C = -1;
		//	OFZPlanes[(int)OFZPlane.InnerTransition2lPlane].Plane.D = -19.98;

		//	//=================================================================      4
		//	ring = new Ring();
		//	ring.Add(OFZPlanes[(int)OFZPlane.InnerTransition2lPlane].Poly.ExteriorRing[3]);
		//	ring.Add(OFZPlanes[(int)OFZPlane.InnerTransition2lPlane].Poly.ExteriorRing[2]);

		//	pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, x3, -y3);
		//	ring.Add(pt0);

		//	ring.Add(OFZPlanes[(int)OFZPlane.ZeroPlane].Poly.ExteriorRing[3]);

		//	OFZPlanes[(int)OFZPlane.BalkedLandingPlane].Poly.ExteriorRing = ring;

		//	OFZPlanes[(int)OFZPlane.BalkedLandingPlane].Plane.Origin = OFZPlanes[(int)OFZPlane.BalkedLandingPlane].Poly.ExteriorRing[3];
		//	//OFZPlanes[(int)OFZPlane.BalkedLandingPlane].Plane.Origin.Z = 0;

		//	OFZPlanes[(int)OFZPlane.BalkedLandingPlane].Plane.A = -0.0333;
		//	OFZPlanes[(int)OFZPlane.BalkedLandingPlane].Plane.B = 0;
		//	OFZPlanes[(int)OFZPlane.BalkedLandingPlane].Plane.C = -1;
		//	OFZPlanes[(int)OFZPlane.BalkedLandingPlane].Plane.D = -59.94;
		//	//=================================================================     5
		//	ring = new Ring();
		//	ring.Add(OFZPlanes[(int)OFZPlane.ZeroPlane].Poly.ExteriorRing[0]);
		//	ring.Add(OFZPlanes[(int)OFZPlane.ZeroPlane].Poly.ExteriorRing[3]);
		//	ring.Add(OFZPlanes[(int)OFZPlane.BalkedLandingPlane].Poly.ExteriorRing[2]);

		//	pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, x1, -y2);
		//	ring.Add(pt0);

		//	OFZPlanes[(int)OFZPlane.InnerTransition2rPlane].Poly.ExteriorRing = ring;
		//	OFZPlanes[(int)OFZPlane.InnerTransition2rPlane].Plane.Origin = OFZPlanes[(int)OFZPlane.InnerTransition2rPlane].Poly.ExteriorRing[0];
		//	//OFZPlanes[(int)OFZPlane.InnerTransition2rPlane].Plane.Origin.Z = 0;

		//	OFZPlanes[(int)OFZPlane.InnerTransition2rPlane].Plane.A = 0;
		//	OFZPlanes[(int)OFZPlane.InnerTransition2rPlane].Plane.B = 0.333;
		//	OFZPlanes[(int)OFZPlane.InnerTransition2rPlane].Plane.C = -1;
		//	OFZPlanes[(int)OFZPlane.InnerTransition2rPlane].Plane.D = -19.98;
		//	//=================================================================      6
		//	ring = new Ring();
		//	ring.Add(OFZPlanes[(int)OFZPlane.InnerApproachPlane].Poly.ExteriorRing[0]);
		//	ring.Add(OFZPlanes[(int)OFZPlane.InnerApproachPlane].Poly.ExteriorRing[3]);
		//	ring.Add(OFZPlanes[(int)OFZPlane.InnerTransition2rPlane].Poly.ExteriorRing[3]);

		//	pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, x0, -y1);
		//	ring.Add(pt0);

		//	OFZPlanes[(int)OFZPlane.InnerTransition1rPlane].Poly.ExteriorRing = ring;

		//	OFZPlanes[(int)OFZPlane.InnerTransition1rPlane].Plane.Origin = OFZPlanes[(int)OFZPlane.InnerTransition1rPlane].Poly.ExteriorRing[1];
		//	//OFZPlanes[(int)OFZPlane.InnerTransition1rPlane].Plane.Origin.Z = 0;

		//	OFZPlanes[(int)OFZPlane.InnerTransition1rPlane].Plane.A = 0.02;
		//	OFZPlanes[(int)OFZPlane.InnerTransition1rPlane].Plane.B = 0.333;
		//	OFZPlanes[(int)OFZPlane.InnerTransition1rPlane].Plane.C = -1;
		//	OFZPlanes[(int)OFZPlane.InnerTransition1rPlane].Plane.D = -21.18;
		//	//=================================================================     7

		//	GeometryOperators geoOp = new GeometryOperators();

		//	for (i = 0; i < n; i++)
		//	{
		//		MultiPolygon TmpPoly = (MultiPolygon)geoOp.UnionGeometry(OFZPlanes[i].Poly, OFZPlanes[n].Poly);
		//		OFZPlanes[n].Poly = TmpPoly[0];
		//	}
		//}

		//public static void CreateIMASPlanes(Point RWYTHRPrj, double fTAS, double Horg, double VPA, double MAPDG, LegBase FASegment, LegBase MASegment, out D3DPolygone[] FIMASPlanes)
		//{
		//	double TanVPA = Math.Tan(VPA);

		//	double d0 = fTAS * GlobalVars.constants.Pansops[ePANSOPSData.dpPilotTolerance].Value;
		//	double d1 = fTAS * GlobalVars.constants.Pansops[ePANSOPSData.arSOCdelayTime].Value;
		//	double MA_InterMOC = GlobalVars.constants.Pansops[ePANSOPSData.arMA_InterMOC].Value;

		//	double x0 = (Horg - GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value) / TanVPA - MASegment.StartFIX.ATT - d0 - d1 + (Horg - MA_InterMOC) / MAPDG;
		//	double y0 = 50000.0;
		//	double x1 = ARANFunctions.ReturnDistanceInMeters(RWYTHRPrj, MASegment.EndFIX.PrjPt) + 10.0;

		//	Ring ring = new Ring();

		//	Point pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x0, y0);
		//	ring.Add(pt0);

		//	pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x0, -y0);
		//	ring.Add(pt0);
		//	pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, x1, -y0);
		//	ring.Add(pt0);
		//	pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, x1, y0);
		//	ring.Add(pt0);

		//	Polygon ExtendPoly = new Polygon();
		//	ExtendPoly.ExteriorRing = ring;

		//	GeometryOperators geoOp = new GeometryOperators();
		//	FIMASPlanes = new D3DPolygone[3];

		//	MultiPolygon PrimPoly = (MultiPolygon)geoOp.UnionGeometry(FASegment.PrimaryArea, MASegment.PrimaryArea);
		//	MultiPolygon FullPoly = (MultiPolygon)geoOp.UnionGeometry(FASegment.FullArea, MASegment.FullArea);
		//	MultiPolygon tmpPoly = (MultiPolygon)geoOp.Intersect(PrimPoly, ExtendPoly);

		//	FIMASPlanes[1].Poly = tmpPoly[0];

		//	LineString ls = new LineString();
		//	pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, x1 + 100, 0);
		//	ls.Add(pt0);

		//	pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x0 - 100, 0);
		//	ls.Add(pt0);

		//	MultiLineString cutter = new MultiLineString();
		//	cutter.Add(ls);

		//	tmpPoly = (MultiPolygon)geoOp.Intersect(FullPoly, ExtendPoly);
		//	geoOp.Cut(tmpPoly, cutter, out Geometry LeftPoly, out Geometry RightPoly);

		//	tmpPoly = (MultiPolygon)geoOp.Difference(LeftPoly, PrimPoly);
		//	FIMASPlanes[0].Poly = tmpPoly[0];

		//	tmpPoly = (MultiPolygon)geoOp.Difference(RightPoly, PrimPoly);
		//	FIMASPlanes[2].Poly = tmpPoly[0];

		//	//ptOrigin = LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x0, 0);
		//	//FASPlanes[1].Plane.Origin = ptOrigin;
		//	FIMASPlanes[1].Plane.A = -MAPDG;
		//	FIMASPlanes[1].Plane.B = 0;
		//	FIMASPlanes[1].Plane.C = -1;
		//	FIMASPlanes[1].Plane.D = x0 * MAPDG;

		//	//====================================================================================================================
		//	double D = FIMASPlanes[1].Plane.D;
		//	int i;

		//	ring = FIMASPlanes[0].Poly.ExteriorRing;
		//	for (i = 0; i < ring.Count; i++)
		//	{
		//		Point pt1 = ring[i];
		//		pt0 = ARANFunctions.PrjToLocal(RWYTHRPrj, RWYTHRPrj.M, pt1);

		//		pt1.T = 0;
		//		double z = D + pt0.X * MAPDG;

		//		if (geoOp.GetDistance(FIMASPlanes[0].Poly.ExteriorRing[i], PrimPoly) > ARANMath.EpsilonDistance)
		//		{
		//			z += MA_InterMOC;
		//			pt1.T = 1;
		//		}

		//		pt1.Z = z;
		//		ring[i] = pt1;
		//	}

		//	FIMASPlanes[0].Poly.ExteriorRing = ring;

		//	//====================================================================================================================
		//	ring = FIMASPlanes[1].Poly.ExteriorRing;

		//	for (i = 0; i < ring.Count; i++)
		//	{
		//		Point pt1 = ring[i];
		//		pt0 = ARANFunctions.PrjToLocal(RWYTHRPrj, RWYTHRPrj.M, pt1);

		//		pt1.Z = D + pt0.X * MAPDG;
		//		ring[i] = pt1;
		//	}
		//	FIMASPlanes[1].Poly.ExteriorRing = ring;

		//	//====================================================================================================================
		//	ring = FIMASPlanes[2].Poly.ExteriorRing;

		//	for (i = 0; i < ring.Count; i++)
		//	{
		//		Point pt1 = ring[i];
		//		pt0 = ARANFunctions.PrjToLocal(RWYTHRPrj, RWYTHRPrj.M, pt1);

		//		pt1.T = 0;
		//		double z = D + pt0.X * MAPDG;

		//		if (geoOp.GetDistance(FIMASPlanes[2].Poly.ExteriorRing[i], PrimPoly) > ARANMath.EpsilonDistance)
		//		{
		//			z += MA_InterMOC;
		//			pt1.T = 1;
		//		}

		//		pt1.Z = z;
		//		ring[i] = pt1;
		//	}

		//	FIMASPlanes[2].Poly.ExteriorRing = ring;
		//}

		//internal static void CreateFMASPlanes(Point RWYTHRPrj, double fTAS, double Horg, double VPA, double MAPDG, LegBase FASegment, LegBase MASegment, out D3DPolygone[] FFMASPlanes)
		//{
		//	double TanVPA = Math.Tan(VPA);			// d+X = 18 san

		//	double d0 = fTAS * GlobalVars.constants.Pansops[ePANSOPSData.dpPilotTolerance].Value;
		//	double d1 = fTAS * GlobalVars.constants.Pansops[ePANSOPSData.arSOCdelayTime].Value;
		//	double MA_FinalMOC = GlobalVars.constants.Pansops[ePANSOPSData.arMA_FinalMOC].Value;

		//	double x0 = (Horg - GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value) / TanVPA - MASegment.StartFIX.ATT - d0 - d1 + (Horg - MA_FinalMOC) / MAPDG;
		//	double y0 = 50000.0;
		//	double x1 = ARANFunctions.ReturnDistanceInMeters(RWYTHRPrj, MASegment.EndFIX.PrjPt) + 10.0;

		//	Ring ring = new Ring();

		//	Point pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x0, y0);
		//	ring.Add(pt0);

		//	pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x0, -y0);
		//	ring.Add(pt0);
		//	pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, x1, -y0);
		//	ring.Add(pt0);
		//	pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, x1, y0);
		//	ring.Add(pt0);

		//	Polygon ExtendPoly = new Polygon();
		//	ExtendPoly.ExteriorRing = ring;

		//	GeometryOperators geoOp = new GeometryOperators();
		//	FFMASPlanes = new D3DPolygone[3];

		//	MultiPolygon PrimPoly = (MultiPolygon)geoOp.UnionGeometry(FASegment.PrimaryArea, MASegment.PrimaryArea);
		//	MultiPolygon FullPoly = (MultiPolygon)geoOp.UnionGeometry(FASegment.FullArea, MASegment.FullArea);
		//	MultiPolygon tmpPoly = (MultiPolygon)geoOp.Intersect(PrimPoly, ExtendPoly);

		//	FFMASPlanes[1].Poly = tmpPoly[0];

		//	LineString ls = new LineString();
		//	pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, x1 + 100, 0);
		//	ls.Add(pt0);

		//	pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x0 - 100, 0);
		//	ls.Add(pt0);

		//	MultiLineString cutter = new MultiLineString();
		//	cutter.Add(ls);

		//	tmpPoly = (MultiPolygon)geoOp.Intersect(FullPoly, ExtendPoly);
		//	geoOp.Cut(tmpPoly, cutter, out Geometry LeftPoly, out Geometry RightPoly);

		//	tmpPoly = (MultiPolygon)geoOp.Difference(LeftPoly, PrimPoly);
		//	FFMASPlanes[0].Poly = tmpPoly[0];

		//	tmpPoly = (MultiPolygon)geoOp.Difference(RightPoly, PrimPoly);
		//	FFMASPlanes[2].Poly = tmpPoly[0];

		//	//ptOrigin = LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -x0, 0);
		//	//FFMASPlanes[1].Plane.Origin = ptOrigin;
		//	FFMASPlanes[1].Plane.A = -MAPDG;
		//	FFMASPlanes[1].Plane.B = 0;
		//	FFMASPlanes[1].Plane.C = -1;
		//	FFMASPlanes[1].Plane.D = x0 * MAPDG;

		//	//====================================================================================================================
		//	double D = -FFMASPlanes[1].Plane.D;
		//	int i;

		//	ring = FFMASPlanes[0].Poly.ExteriorRing;
		//	for (i = 0; i < ring.Count; i++)
		//	{
		//		Point pt1 = ring[i];
		//		pt0 = ARANFunctions.PrjToLocal(RWYTHRPrj, RWYTHRPrj.M, pt1);

		//		pt1.T = 0;
		//		double z = D + pt0.X * MAPDG;

		//		if (geoOp.GetDistance(pt1, PrimPoly) > ARANMath.EpsilonDistance)
		//		{
		//			z = z + MA_FinalMOC;
		//			pt1.T = 1;
		//		}

		//		pt1.Z = z;
		//		ring[i] = pt1;
		//	}

		//	FFMASPlanes[0].Poly.ExteriorRing = ring;
		//	//====================================================================================================================
		//	ring = FFMASPlanes[1].Poly.ExteriorRing;

		//	for (i = 0; i < ring.Count; i++)
		//	{
		//		Point pt1 = ring[i];
		//		pt0 = ARANFunctions.PrjToLocal(RWYTHRPrj, RWYTHRPrj.M, pt1);

		//		pt1.Z = D + pt0.X * MAPDG;

		//		ring[i] = pt1;
		//	}

		//	FFMASPlanes[1].Poly.ExteriorRing = ring;
		//	//====================================================================================================================
		//	ring = FFMASPlanes[2].Poly.ExteriorRing;

		//	for (i = 0; i < ring.Count; i++)
		//	{
		//		Point pt1 = ring[i];
		//		pt0 = ARANFunctions.PrjToLocal(RWYTHRPrj, RWYTHRPrj.M, pt1);

		//		pt1.T = 0;
		//		double z = D + pt0.X * MAPDG;

		//		if (geoOp.GetDistance(pt1, PrimPoly) > ARANMath.EpsilonDistance)
		//		{
		//			z = z + MA_FinalMOC;
		//			pt1.T = 1;
		//		}

		//		pt1.Z = z;
		//		ring[i] = pt1;
		//	}

		//	FFMASPlanes[2].Poly.ExteriorRing = ring;
		//}


		//public static void CreatWPlanes(Point RWYTHRPrj, aircraftCategory AirCat, double HFAP, double MOCAppr, double VPA, LegBase FASegment, D3DPolygone[] FASPlanes, out D3DPolygone[] WPlanes)
		//{
		//	//const int NodeCount = 3;
		//	double[] NodeAngle = { 2.5 * Math.PI / 180, 3.0 * Math.PI / 180, 3.5 * Math.PI / 180 };
		//	double[] WxOrg = { 394, 281, 195 };
		//	double[] TanW = { 0.0239, 0.0285, 0.0331 };

		//	double dA01 = NodeAngle[0] - NodeAngle[1];
		//	double dA02 = NodeAngle[0] - NodeAngle[2];
		//	double dA12 = NodeAngle[1] - NodeAngle[2];
		//	double dA10 = -dA01;
		//	double dA20 = -dA02;
		//	double dA21 = -dA12;
		//	double WxOrga = WxOrg[0] * (VPA - NodeAngle[1]) * (VPA - NodeAngle[2]) / (dA01 * dA02) + WxOrg[1] * (VPA - NodeAngle[0]) * (VPA - NodeAngle[2]) / (dA10 * dA12) + WxOrg[2] * (VPA - NodeAngle[0]) * (VPA - NodeAngle[1]) / (dA20 * dA21);
		//	double TanWa = TanW[0] * (VPA - NodeAngle[1]) * (VPA - NodeAngle[2]) / (dA01 * dA02) + TanW[1] * (VPA - NodeAngle[0]) * (VPA - NodeAngle[2]) / (dA10 * dA12) + TanW[2] * (VPA - NodeAngle[0]) * (VPA - NodeAngle[1]) / (dA20 * dA21);

		//	WPlanes = new D3DPolygone[3];
		//	WPlanes[1].Plane.A = TanWa;
		//	WPlanes[1].Plane.B = 0;
		//	WPlanes[1].Plane.C = -1;
		//	WPlanes[1].Plane.D = -WxOrga * TanWa;

		//	if (AirCat != aircraftCategory.acA && AirCat != aircraftCategory.acB && AirCat != aircraftCategory.acH)
		//	{
		//		WPlanes[1].Plane.D = WPlanes[1].Plane.D - (GlobalVars.constants.AircraftCategory[aircraftCategoryData.arVerticalSize].Value[AirCat] - GlobalVars.constants.AircraftCategory[aircraftCategoryData.arVerticalSize].Value[aircraftCategory.acA]);
		//		WxOrga = -WPlanes[1].Plane.D / TanWa;
		//	}

		//	double Xin = (WPlanes[1].Plane.D - FASPlanes[1].Plane.D) / (FASPlanes[1].Plane.A - WPlanes[1].Plane.A);
		//	double y0 = 50000.0;

		//	LineString part = new LineString();
		//	Point pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -Xin, y0);
		//	part.Add(pt0);

		//	pt0 = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -Xin, -y0);
		//	part.Add(pt0);

		//	MultiLineString cutter = new MultiLineString();
		//	cutter.Add(part);

		//	// GUI.DrawPolygon(FASPlanes[0].Poly, (255 shl 8) + 255, sfsDiagonalCross);
		//	// GUI.DrawPolyline(cutter, 255, 2);

		//	GeometryOperators geoOp = new GeometryOperators();

		//	geoOp.Cut(FASPlanes[0].Poly, cutter, out Geometry leftGeom, out Geometry rightGeom);
		//	WPlanes[0].Poly = ((MultiPolygon)rightGeom)[0];

		//	geoOp.Cut(FASPlanes[1].Poly, cutter, out leftGeom, out rightGeom);
		//	WPlanes[1].Poly = ((MultiPolygon)rightGeom)[0];

		//	geoOp.Cut(FASPlanes[2].Poly, cutter, out leftGeom, out rightGeom);
		//	WPlanes[2].Poly = ((MultiPolygon)rightGeom)[0];

		//	// ==============================================================================
		//	int i;
		//	double z, D = (Xin - WxOrga) * TanWa;
		//	Point pt1, ptC = ARANFunctions.LocalToPrj(RWYTHRPrj, RWYTHRPrj.M, -Xin);

		//	Ring ring = WPlanes[0].Poly.ExteriorRing;
		//	for (i = 0; i < ring.Count; i++)
		//	{
		//		pt1 = ring[i];
		//		pt0 = ARANFunctions.PrjToLocal(ptC, RWYTHRPrj.M, pt1);

		//		pt1.T = 0;
		//		z = D - pt0.X * TanWa;

		//		if (geoOp.GetDistance(pt1, FASPlanes[1].Poly) > ARANMath.EpsilonDistance)
		//		{
		//			z += MOCAppr;
		//			pt1.T = 1;
		//		}

		//		pt1.Z = z;
		//		ring[i] = pt1;
		//	}

		//	WPlanes[0].Poly.ExteriorRing = ring;
		//	// ==============================================================================
		//	ring = WPlanes[1].Poly.ExteriorRing;

		//	for (i = 0; i < ring.Count; i++)
		//	{
		//		pt1 = ring[i];
		//		pt0 = ARANFunctions.PrjToLocal(ptC, RWYTHRPrj.M, pt1);

		//		pt0.Z = D - pt0.X * TanWa;
		//		ring[i] = pt1;
		//	}

		//	WPlanes[1].Poly.ExteriorRing = ring;
		//	// ==============================================================================
		//	ring = WPlanes[2].Poly.ExteriorRing;

		//	for (i = 0; i < ring.Count; i++)
		//	{
		//		pt1 = ring[i];
		//		pt0 = ARANFunctions.PrjToLocal(ptC, RWYTHRPrj.M, pt1);

		//		pt1.T = 0;
		//		z = D - pt0.X * TanWa;

		//		if (geoOp.GetDistance(pt1, FASPlanes[1].Poly) > ARANMath.EpsilonDistance)
		//		{
		//			z = z + MOCAppr;
		//			pt1.T = 1;
		//		}

		//		pt1.Z = z;
		//		ring[i] = pt1;
		//	}

		//	WPlanes[2].Poly.ExteriorRing = ring;
		//}

		public static double IntermMALenght(ObstacleContainer OutObstList, double MOC, double OCH, double MAPDG, double THRToSOCDist)
		{
			double result = 0.0;
			double InvPDG = 1.0 / MAPDG;

			int n = OutObstList.Parts.Length;
			for (int i = 0; i < n; i++)
			{
				double ReqDistFromSOC = (OutObstList.Parts[i].Height + MOC * OutObstList.Parts[i].fSecCoeff - OCH) * InvPDG;

				if (result < ReqDistFromSOC && THRToSOCDist - ReqDistFromSOC < OutObstList.Parts[i].Dist)
					result = ReqDistFromSOC;
			}

			return result;
		}

		static void RemoveSeamPoints(ref MultiPoint pPoints)
		{
			const double eps2 = ARANMath.EpsilonDistance * ARANMath.EpsilonDistance;
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
					double fDist2 = dx * dx + dy * dy;

					if (fDist2 < eps2)
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

		public static int AnaliseFASObstacles(ObstacleContainer InObstList, Point RWYTHRPrj, D3DPolygone[] planes, LegBase FASegment, double VPA, double FAS, double FAS_, double FAS__, out ObstacleContainer OutObstList)
		{
			//Plane Surface = Plane.FinalApproachSurface;
			int m = InObstList.Obstacles.Length;

			if (m == 0)
			{
				OutObstList.Obstacles = new Obstacle[0];
				OutObstList.Parts = new ObstacleData[0];
				return 0;
			}

			Point pCurrPt;
			Geometry pCurrGeom;

			GeometryOperators pTopoOper = new GeometryOperators();
			GeometryOperators pRelation = new GeometryOperators();
			GeometryOperators pRelationFull = new GeometryOperators();

			int p = planes.Length;
			pRelationFull.CurrentGeometry = planes[p - 1].Poly;
			OutObstList.Obstacles = new Obstacle[m];

			int c = 10 * m;
			OutObstList.Parts = new ObstacleData[c];

			int k = 0;
			int l = 0;
			int result = 0;

			double TanVPA = Math.Tan(VPA);
			double CotVPA = 1.0 / TanVPA;

			LineString ls = new LineString();
			MultiLineString mls;
			MultiPoint pObstPoints = new MultiPoint();

			for (int i = 0; i < m; i++)
			{
				pCurrGeom = InObstList.Obstacles[i].pGeomPrj;

				if (pCurrGeom.IsEmpty || pRelationFull.Disjoint(pCurrGeom))
					continue;

				pObstPoints.Clear();

				if (pCurrGeom.Type == GeometryType.Point)
					pObstPoints.Add((Point)pCurrGeom);
				else
				{
					for (int o = 0; o < p - 1; o++)
					{
						if(planes[o].Poly == null)
							continue;
						//pRelation.CurrentGeometry = planes[o].Poly;
						if (pRelation.Disjoint(planes[o].Poly, pCurrGeom))
							continue;

						var pTmpPoints = pTopoOper.Intersect(pCurrGeom, planes[o].Poly);
						pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());
					}

					RemoveSeamPoints(ref pObstPoints);
				}

				int n = pObstPoints.Count;
				if (n <= 0)
					continue;

				OutObstList.Obstacles[l] = InObstList.Obstacles[i];
				OutObstList.Obstacles[l].PartsNum = n;
				//OutObstList.Obstacles[l].Parts = new int[n];

				//if (InObstList.Obstacles[i].pGeomPrj.Type == GeometryType.Point)
				//	OutObstList.Obstacles[l].Height = ((Point)InObstList.Obstacles[i].pGeomPrj).Z - RWYTHRPrj.Z;
				//else
				//{
				//	//pZv = ObstacleList.Obstacles[i].pGeomPrj;
				//	//OutObstList.Obstacles[L].Height = pZv.ZMax - ptLHPrj.Z;
				//}
				double abscissa = RWYTHRPrj.M + Math.PI;
				double hFAP = ARANFunctions.ReturnDistanceInMeters(RWYTHRPrj, FASegment.StartFIX.PrjPt) * TanVPA + GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value;
				double MA_InterMOC = GlobalVars.constants.Pansops[ePANSOPSData.arMA_InterMOC].Value;


				for (int j = 0; j < n; j++)
				{
					pCurrPt = pObstPoints[j];

					//GlobalVars.gAranGraphics.DrawPointWithText(pCurrPt, -1, OutObstList.Obstacles[l].TypeName + "/" + OutObstList.Obstacles[l].UnicalName);
					//System.Windows.Forms.Application.DoEvents();

					if (k >= c)
					{
						c += m;
						Array.Resize<ObstacleData>(ref OutObstList.Parts, c);
					}

					OutObstList.Parts[k].pPtPrj = pCurrPt;
					OutObstList.Parts[k].Owner = l;
					OutObstList.Parts[k].Height = OutObstList.Obstacles[l].Height;
					OutObstList.Parts[k].Index = j;
					//OutObstList.Obstacles[l].Parts[j] = k;

					ARANFunctions.PrjToLocal(RWYTHRPrj, abscissa, pCurrPt, out double x, out double y);

					//if (MaxDist >= 1 && x > MaxDist)						continue;

					OutObstList.Parts[k].Dist = x;
					OutObstList.Parts[k].CLShift = y;

					//GlobalVars.gAranGraphics.DrawPointWithText(pCurrPt, -1, OutObstList.Obstacles[l].TypeName + "/" + OutObstList.Obstacles[l].UnicalName);
					//System.Windows.Forms.Application.DoEvents();

					for (int o = 0; o < p - 1; o++)
					{
						if (planes[o].Poly == null)
							continue;

						//pRelation.CurrentGeometry = planes[o].Poly;
						if (!pRelation.Contains(planes[o].Poly, pCurrPt))
							continue;

						if (o > 2)
						{
							OutObstList.Parts[k].fSecCoeff = 1.0;
							OutObstList.Parts[k].MOC = 0.0;
							OutObstList.Parts[k].hSurface = hFAP - MA_InterMOC;
							OutObstList.Parts[k].Plane = (int)Plane.IntermediateApproach;
						}
						else
						{
							double Coeff = 1.0;
							double hmaxSS = GlobalVars.H0;
							double x0 = FASegment.EndFIX.ATT + (hmaxSS - GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value) * CotVPA;
							double z = FAS * (x - x0);
							double z1 = z;
							double elev = z + RWYTHRPrj.Z;

							if (elev > GlobalVars.H1h)
							{
								hmaxSS = GlobalVars.H5000;
								x0 = FASegment.EndFIX.ATT + (hmaxSS - GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value) * CotVPA;
								z1 = FAS_ * (x - x0);
								z = Math.Max(z1, z);

								elev = z + RWYTHRPrj.Z;

								if (elev > GlobalVars.H2h)
								{
									hmaxSS = GlobalVars.H10000;
									x0 = FASegment.EndFIX.ATT + (hmaxSS - GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value) * CotVPA;
									z1 = FAS__ * (x - x0);
									z = Math.Max(z1, z);
								}
							}

							OutObstList.Parts[k].Flags = 0;

							if (o != 1)
							{
								OutObstList.Parts[k].Flags = 1;
								ls.Clear();

								Point pt0 = ARANFunctions.LocalToPrj(pCurrPt, abscissa, 0, 1000000);
								ls.Add(pt0);

								pt0 = ARANFunctions.LocalToPrj(pCurrPt, abscissa, 0, -1000000);
								ls.Add(pt0);

								mls = (MultiLineString)pTopoOper.Intersect(ls, planes[o].Poly);

								//if (pCuttLine.Count == 0)
								//{
								//GlobalVars.gAranGraphics.DrawLineString(pPart, -1, 2);
								//GlobalVars.gAranGraphics.DrawPointWithText(pCurrPt, -1, OutObstList.Obstacles[l].TypeName + "/" + OutObstList.Obstacles[l].UnicalName);
								//GlobalVars.gAranGraphics.DrawPolygon(planes[o].Poly, -1, AranEnvironment.Symbols.eFillStyle.sfsCross);
								//GlobalVars.gAranGraphics.DrawPolygon(planes[2].Poly, -1, AranEnvironment.Symbols.eFillStyle.sfsBackwardDiagonal);

								//GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pCurrGeom, -1, AranEnvironment.Symbols.eFillStyle.sfsBackwardDiagonal);
								//LegBase.ProcessMessages();

								//	int intir = pPart.Count;
								//}

								double y0, y1, yN;
								ARANFunctions.PrjToLocal(RWYTHRPrj, abscissa, mls[0][0], out x, out y0);
								ARANFunctions.PrjToLocal(RWYTHRPrj, abscissa, mls[0][1], out x, out y1);

								y0 = Math.Abs(y0);
								y1 = Math.Abs(y1);
								yN = Math.Abs(y);

								if (y1 < y0)
								{
									double fTmp = y1;
									y1 = y0;
									y0 = fTmp;
								}

								if (p > 4)
								{
									mls = (MultiLineString)pTopoOper.Intersect(ls, planes[o + 3].Poly);
									y1 += mls.Length;
								}

								Coeff = (y1 - yN) / (y1 - y0);
								z += (1.0 - Coeff) * hmaxSS;
							}

							OutObstList.Parts[k].fSecCoeff = Coeff;
							OutObstList.Parts[k].MOC = hmaxSS;
							OutObstList.Parts[k].hSurface = z;
							OutObstList.Parts[k].Plane = (int)Plane.FinalApproachSurface;
						}

						OutObstList.Parts[k].Elev = OutObstList.Obstacles[l].Height;
						OutObstList.Parts[k].Height = OutObstList.Obstacles[l].Height - RWYTHRPrj.Z;

						OutObstList.Parts[k].hPenet = OutObstList.Parts[k].Height - OutObstList.Parts[k].hSurface;

						if (OutObstList.Parts[k].hPenet > 0.0)
							result++;

						k++;
						break;
					}
				}

				l++;
			}

			if (k >= 0)
			{
				Array.Resize<Obstacle>(ref OutObstList.Obstacles, l);
				Array.Resize<ObstacleData>(ref OutObstList.Parts, k);
			}
			else
			{
				OutObstList.Obstacles = new Obstacle[0];
				OutObstList.Parts = new ObstacleData[0];
			}

			return result;
		}

		public static int AnaliseHorzObstacles(ObstacleContainer InObstList, Point RWYTHRPrj, D3DPolygone[] planes, LegBase FASegment, double hmaxSS, out ObstacleContainer OutObstList)
		{
			int m = InObstList.Obstacles.Length;

			if (m == 0)
			{
				OutObstList.Obstacles = new Obstacle[0];
				OutObstList.Parts = new ObstacleData[0];
				return 0;
			}

			Point pCurrPt;
			Geometry pCurrGeom;

			GeometryOperators pTopoOper = new GeometryOperators();
			GeometryOperators pRelation = new GeometryOperators();
			GeometryOperators pRelationFull = new GeometryOperators();

			int p = planes.Length;
			pRelationFull.CurrentGeometry = planes[p - 1].Poly;
			OutObstList.Obstacles = new Obstacle[m];

			int c = 10 * m;
			OutObstList.Parts = new ObstacleData[c];

			int k = 0;
			int l = 0;
			int result = 0;

			LineString pPart = new LineString();
			MultiLineString pCuttLine;
			MultiPoint pObstPoints = new MultiPoint();

			for (int i = 0; i < m; i++)
			{
				pCurrGeom = InObstList.Obstacles[i].pGeomPrj;

				if (pCurrGeom.IsEmpty || pRelationFull.Disjoint(pCurrGeom))
					continue;

				pObstPoints.Clear();

				if (pCurrGeom.Type == GeometryType.Point)
					pObstPoints.Add((Point)pCurrGeom);
				else
				{
					for (int o = 0; o < p - 1; o++)
					{
						pRelation.CurrentGeometry = planes[o].Poly;
						if (pRelation.Disjoint(pCurrGeom))
							continue;

						var pTmpPoints = pTopoOper.Intersect(pCurrGeom, planes[o].Poly);
						pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());
					}

					RemoveSeamPoints(ref pObstPoints);
				}

				int n = pObstPoints.Count;
				if (n <= 0)
					continue;

				OutObstList.Obstacles[l] = InObstList.Obstacles[i];
				OutObstList.Obstacles[l].PartsNum = n;
				//OutObstList.Obstacles[l].Parts = new int[n];

				//if (InObstList.Obstacles[i].pGeomPrj.Type == GeometryType.Point)
				//	OutObstList.Obstacles[l].Height = ((Point)InObstList.Obstacles[i].pGeomPrj).Z - RWYTHRPrj.Z;
				//else
				//{
				//	//pZv = ObstacleList.Obstacles[i].pGeomPrj;
				//	//OutObstList.Obstacles[L].Height = pZv.ZMax - ptLHPrj.Z;
				//}

				double abscissa = RWYTHRPrj.M + Math.PI;

				for (int j = 0; j < n; j++)
				{
					pCurrPt = pObstPoints[j];

					//GlobalVars.gAranGraphics.DrawPointWithText(pCurrPt, -1, OutObstList.Obstacles[l].TypeName + "/" + OutObstList.Obstacles[l].UnicalName);
					//System.Windows.Forms.Application.DoEvents();

					if (k >= c)
					{
						c += m;
						Array.Resize<ObstacleData>(ref OutObstList.Parts, c);
					}

					OutObstList.Parts[k].pPtPrj = pCurrPt;
					OutObstList.Parts[k].Owner = l;
					OutObstList.Parts[k].Height = OutObstList.Obstacles[l].Height;
					OutObstList.Parts[k].Index = j;
					//OutObstList.Obstacles[l].Parts[j] = k;

					ARANFunctions.PrjToLocal(RWYTHRPrj, abscissa, pCurrPt, out double x, out double y);

					OutObstList.Parts[k].Dist = x;
					OutObstList.Parts[k].CLShift = y;

					//GlobalVars.gAranGraphics.DrawPointWithText(pCurrPt, -1, OutObstList.Obstacles[l].TypeName + "/" + OutObstList.Obstacles[l].UnicalName);
					//System.Windows.Forms.Application.DoEvents();

					for (int o = 0; o < p - 1; o++)
					{
						pRelation.CurrentGeometry = planes[o].Poly;
						if (!pRelation.Contains(pCurrPt))
							continue;

						double z = 0.0;
						double Coeff = 1.0;

						OutObstList.Parts[k].Flags = 0;
						if (o != 1)
						{
							OutObstList.Parts[k].Flags = 1;
							pPart.Clear();

							Point pt0 = ARANFunctions.LocalToPrj(pCurrPt, abscissa, 0, 1000000);
							pPart.Add(pt0);

							pt0 = ARANFunctions.LocalToPrj(pCurrPt, abscissa, 0, -1000000);
							pPart.Add(pt0);

							pCuttLine = (MultiLineString)pTopoOper.Intersect(pPart, planes[o].Poly);

							//if (pCuttLine.Count == 0)
							//{
							//GlobalVars.gAranGraphics.DrawLineString(pPart, -1, 2);
							//GlobalVars.gAranGraphics.DrawPointWithText(pCurrPt, -1, OutObstList.Obstacles[l].TypeName + "/" + OutObstList.Obstacles[l].UnicalName);
							//GlobalVars.gAranGraphics.DrawPolygon(planes[o].Poly, -1, AranEnvironment.Symbols.eFillStyle.sfsCross);
							//GlobalVars.gAranGraphics.DrawPolygon(planes[2].Poly, -1, AranEnvironment.Symbols.eFillStyle.sfsBackwardDiagonal);

							//GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pCurrGeom, -1, AranEnvironment.Symbols.eFillStyle.sfsBackwardDiagonal);
							//LegBase.ProcessMessages();

							//	int intir = pPart.Count;
							//}
							double Y0, Y1, YN;
							ARANFunctions.PrjToLocal(RWYTHRPrj, abscissa, pCuttLine[0][0], out x, out Y0);
							ARANFunctions.PrjToLocal(RWYTHRPrj, abscissa, pCuttLine[0][1], out x, out Y1);

							Y0 = Math.Abs(Y0);
							Y1 = Math.Abs(Y1);
							YN = Math.Abs(y);

							if (Y1 < Y0)
							{
								double fTmp = Y1;
								Y1 = Y0;
								Y0 = fTmp;
							}

							Coeff = (Y1 - YN) / (Y1 - Y0);
							z = (1.0 - Coeff) * hmaxSS;
						}

						OutObstList.Parts[k].fSecCoeff = Coeff;
						OutObstList.Parts[k].MOC = hmaxSS;

						OutObstList.Parts[k].Plane = (int)Plane.HorizontalSurface;

						OutObstList.Parts[k].Elev = OutObstList.Obstacles[l].Height;
						OutObstList.Parts[k].Height = OutObstList.Obstacles[l].Height - RWYTHRPrj.Z;

						OutObstList.Parts[k].hSurface = z;
						OutObstList.Parts[k].hPenet = OutObstList.Parts[k].Height - z;

						if (OutObstList.Parts[k].hPenet > 0.0)
							result++;

						k++;
						break;
					}
				}

				l++;
			}

			if (k >= 0)
			{
				Array.Resize<Obstacle>(ref OutObstList.Obstacles, l);
				Array.Resize<ObstacleData>(ref OutObstList.Parts, k);
			}
			else
			{
				OutObstList.Obstacles = new Obstacle[0];
				OutObstList.Parts = new ObstacleData[0];
			}

			return result;
		}

		public static int AnaliseMASObstacles(ObstacleContainer InObstList, Point RWYTHRPrj, D3DPolygone[] planes, double xOrigin, double MASG, out ObstacleContainer OutObstList)
		{
			//LegBase FASegment, double VPA,
			int m = InObstList.Obstacles.Length;

			if (m == 0)
			{
				OutObstList.Obstacles = new Obstacle[0];
				OutObstList.Parts = new ObstacleData[0];
				return 0;
			}

			Point pCurrPt;
			Geometry pCurrGeom;

			GeometryOperators pTopoOper = new GeometryOperators();
			GeometryOperators pRelation = new GeometryOperators();
			GeometryOperators pRelationFull = new GeometryOperators();

			int p = planes.Length;
			pRelationFull.CurrentGeometry = planes[p - 1].Poly;
			OutObstList.Obstacles = new Obstacle[m];

			int c = 10 * m;
			OutObstList.Parts = new ObstacleData[c];

			int k = 0;
			int l = 0;
			int result = 0;

			//double Y0, Y1, YN, fTmp, Coeff, z;

			LineString pPart = new LineString();
			MultiLineString pCuttLine;
			MultiPoint pObstPoints = new MultiPoint();

			for (int i = 0; i < m; i++)
			{
				pCurrGeom = InObstList.Obstacles[i].pGeomPrj;

				if (pCurrGeom.IsEmpty || pRelationFull.Disjoint(pCurrGeom))
					continue;

				pObstPoints.Clear();

				if (pCurrGeom.Type == GeometryType.Point)
					pObstPoints.Add((Point)pCurrGeom);
				else
				{
					for (int o = 0; o < p - 1; o++)
					{
						pRelation.CurrentGeometry = planes[o].Poly;
						if (pRelation.Disjoint(pCurrGeom))
							continue;

						var pTmpPoints = pTopoOper.Intersect(pCurrGeom, planes[o].Poly);
						pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());
					}

					RemoveSeamPoints(ref pObstPoints);
				}

				int n = pObstPoints.Count;
				if (n <= 0)
					continue;

				OutObstList.Obstacles[l] = InObstList.Obstacles[i];
				OutObstList.Obstacles[l].PartsNum = n;
				//OutObstList.Obstacles[l].Parts = new int[n];

				//if (InObstList.Obstacles[i].pGeomPrj.Type == GeometryType.Point)
				//	OutObstList.Obstacles[l].Height = ((Point)InObstList.Obstacles[i].pGeomPrj).Z - RWYTHRPrj.Z;
				//else
				//{
				//	//pZv = ObstacleList.Obstacles[i].pGeomPrj;
				//	//OutObstList.Obstacles[L].Height = pZv.ZMax - ptLHPrj.Z;
				//}
				double abscissa = RWYTHRPrj.M + Math.PI;
				//double x0 = xOrigin;
				double hmaxSS = GlobalVars.constants.Pansops[ePANSOPSData.arMA_InterMOC].Value;

				for (int j = 0; j < n; j++)
				{
					pCurrPt = pObstPoints[j];

					//GlobalVars.gAranGraphics.DrawPointWithText(pCurrPt, -1, OutObstList.Obstacles[l].TypeName + "/" + OutObstList.Obstacles[l].UnicalName);
					//System.Windows.Forms.Application.DoEvents();

					if (k >= c)
					{
						c += m;
						Array.Resize<ObstacleData>(ref OutObstList.Parts, c);
					}

					OutObstList.Parts[k].pPtPrj = pCurrPt;
					OutObstList.Parts[k].Owner = l;
					OutObstList.Parts[k].Height = OutObstList.Obstacles[l].Height;
					OutObstList.Parts[k].Index = j;
					//OutObstList.Obstacles[l].Parts[j] = k;

					ARANFunctions.PrjToLocal(RWYTHRPrj, abscissa, pCurrPt, out double x, out double y);

					OutObstList.Parts[k].Dist = x;
					OutObstList.Parts[k].CLShift = y;

					//GlobalVars.gAranGraphics.DrawPointWithText(pCurrPt, -1, OutObstList.Obstacles[l].TypeName + "/" + OutObstList.Obstacles[l].UnicalName);
					//System.Windows.Forms.Application.DoEvents();

					for (int o = 0; o < p - 1; o++)
					{
						pRelation.CurrentGeometry = planes[o].Poly;
						if (!pRelation.Contains(pCurrPt))
							continue;

						double Coeff = 1.0;

						double z = MASG * (xOrigin - x);


						OutObstList.Parts[k].Flags = 0;
						if (o != 1)
						{
							OutObstList.Parts[k].Flags = 1;
							pPart.Clear();

							Point pt0 = ARANFunctions.LocalToPrj(pCurrPt, abscissa, 0, 1000000);
							pPart.Add(pt0);

							pt0 = ARANFunctions.LocalToPrj(pCurrPt, abscissa, 0, -1000000);
							pPart.Add(pt0);

							pCuttLine = (MultiLineString)pTopoOper.Intersect(pPart, planes[o].Poly);

							//if (pCuttLine.Count == 0)
							//{
							//GlobalVars.gAranGraphics.DrawLineString(pPart, -1, 2);
							//GlobalVars.gAranGraphics.DrawPointWithText(pCurrPt, -1, OutObstList.Obstacles[l].TypeName + "/" + OutObstList.Obstacles[l].UnicalName);
							//GlobalVars.gAranGraphics.DrawPolygon(planes[o].Poly, -1, AranEnvironment.Symbols.eFillStyle.sfsCross);
							//GlobalVars.gAranGraphics.DrawPolygon(planes[2].Poly, -1, AranEnvironment.Symbols.eFillStyle.sfsBackwardDiagonal);

							//GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pCurrGeom, -1, AranEnvironment.Symbols.eFillStyle.sfsBackwardDiagonal);
							//LegBase.ProcessMessages();

							//	int intir = pPart.Count;
							//}
							double Y0, Y1, YN;
							ARANFunctions.PrjToLocal(RWYTHRPrj, abscissa, pCuttLine[0][0], out x, out Y0);
							ARANFunctions.PrjToLocal(RWYTHRPrj, abscissa, pCuttLine[0][1], out x, out Y1);

							Y0 = Math.Abs(Y0);
							Y1 = Math.Abs(Y1);
							YN = Math.Abs(y);

							if (Y1 < Y0)
							{
								double fTmp = Y1;
								Y1 = Y0;
								Y0 = fTmp;
							}

							Coeff = (Y1 - YN) / (Y1 - Y0);
							z += (1.0 - Coeff) * hmaxSS;
						}

						OutObstList.Parts[k].fSecCoeff = Coeff;
						OutObstList.Parts[k].MOC = hmaxSS;

						OutObstList.Parts[k].Plane = (int)Plane.MissedApproachSurface;

						OutObstList.Parts[k].Elev = OutObstList.Obstacles[l].Height;
						OutObstList.Parts[k].Height = OutObstList.Obstacles[l].Height - RWYTHRPrj.Z;

						OutObstList.Parts[k].hSurface = z;
						OutObstList.Parts[k].hPenet = OutObstList.Parts[k].Height - z;

						if (OutObstList.Parts[k].hPenet > 0.0)
							result++;

						k++;
						break;
					}
				}

				l++;
			}

			if (k >= 0)
			{
				Array.Resize<Obstacle>(ref OutObstList.Obstacles, l);
				Array.Resize<ObstacleData>(ref OutObstList.Parts, k);
			}
			else
			{
				OutObstList.Obstacles = new Obstacle[0];
				OutObstList.Parts = new ObstacleData[0];
			}

			return result;
		}

		//public static int AnaliseObstacles0(ObstacleContainer InObstList, Point RWYTHRPrj, D3DPolygone[] planes, Plane Surface, out ObstacleContainer OutObstList, double Heigh0, double Heigh1 = 0, double MaxDist = 0)
		//{
		//	int m = InObstList.Obstacles.Length;

		//	if (m == 0)
		//	{
		//		OutObstList.Obstacles = new Obstacle[0];
		//		OutObstList.Parts = new ObstacleData[0];
		//		return 0;
		//	}

		//	Point pCurrPt;
		//	Geometry pCurrGeom;

		//	GeometryOperators pTopoOper = new GeometryOperators();
		//	GeometryOperators pRelation = new GeometryOperators();
		//	GeometryOperators pRelationFull = new GeometryOperators();

		//	int p = planes.Length;
		//	pRelationFull.CurrentGeometry = planes[p - 1].Poly;
		//	OutObstList.Obstacles = new Obstacle[m];

		//	int c = 10 * m;
		//	OutObstList.Parts = new ObstacleData[c];

		//	int k = 0;
		//	int l = 0;
		//	int result = 0;

		//	double Heigh = Heigh0;
		//	double Y0, Y1, YN, fTmp, Coeff, z;

		//	LineString pPart = new LineString();
		//	MultiLineString pCuttLine;
		//	MultiPoint pObstPoints = new MultiPoint();

		//	for (int i = 0; i < m; i++)
		//	{
		//		pCurrGeom = InObstList.Obstacles[i].pGeomPrj;

		//		if (pCurrGeom.IsEmpty || pRelationFull.Disjoint(pCurrGeom))
		//			continue;

		//		pObstPoints.Clear();

		//		if (pCurrGeom.Type == GeometryType.Point)
		//			pObstPoints.Add((Point)pCurrGeom);
		//		else
		//		{
		//			for (int o = 0; o < p - 1; o++)
		//			{
		//				pRelation.CurrentGeometry = planes[o].Poly;
		//				if (pRelation.Disjoint(pCurrGeom))
		//					continue;

		//				var pTmpPoints = pTopoOper.Intersect(pCurrGeom, planes[o].Poly);
		//				pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());
		//			}

		//			RemoveSeamPoints(ref pObstPoints);
		//		}

		//		int n = pObstPoints.Count;
		//		if (n <= 0)
		//			continue;

		//		OutObstList.Obstacles[l] = InObstList.Obstacles[i];
		//		OutObstList.Obstacles[l].PartsNum = n;
		//		OutObstList.Obstacles[l].Parts = new int[n];

		//		//if (InObstList.Obstacles[i].pGeomPrj.Type == GeometryType.Point)
		//		//	OutObstList.Obstacles[l].Height = ((Point)InObstList.Obstacles[i].pGeomPrj).Z - RWYTHRPrj.Z;
		//		//else
		//		//{
		//		//	//pZv = ObstacleList.Obstacles[i].pGeomPrj;
		//		//	//OutObstList.Obstacles[L].Height = pZv.ZMax - ptLHPrj.Z;
		//		//}
		//		double abscissa = RWYTHRPrj.M + Math.PI;

		//		for (int j = 0; j < n; j++)
		//		{
		//			pCurrPt = pObstPoints[j];

		//			//GlobalVars.gAranGraphics.DrawPointWithText(pCurrPt, -1, OutObstList.Obstacles[l].TypeName + "/" + OutObstList.Obstacles[l].UnicalName);
		//			//System.Windows.Forms.Application.DoEvents();

		//			if (k >= c)
		//			{
		//				c += m;
		//				Array.Resize<ObstacleData>(ref OutObstList.Parts, c);
		//			}

		//			OutObstList.Parts[k].pPtPrj = pCurrPt;
		//			OutObstList.Parts[k].Owner = l;
		//			OutObstList.Parts[k].Height = OutObstList.Obstacles[l].Height;
		//			OutObstList.Parts[k].Index = j;
		//			OutObstList.Obstacles[l].Parts[j] = k;

		//			ARANFunctions.PrjToLocal(RWYTHRPrj, abscissa, pCurrPt, out double x, out double y);

		//			if (MaxDist >= 1 && x > MaxDist)
		//				continue;

		//			OutObstList.Parts[k].Dist = x;
		//			OutObstList.Parts[k].CLShift = y;

		//			//GlobalVars.gAranGraphics.DrawPointWithText(pCurrPt, -1, OutObstList.Obstacles[l].TypeName + "/" + OutObstList.Obstacles[l].UnicalName);
		//			//System.Windows.Forms.Application.DoEvents();

		//			for (int o = 0; o < p - 1; o++)
		//			{
		//				pRelation.CurrentGeometry = planes[o].Poly;
		//				if (!pRelation.Contains(pCurrPt))
		//					continue;

		//				if (Surface != Plane.HorizontalSurface)
		//					z = planes[o].Plane.A * x + planes[o].Plane.B * y + planes[o].Plane.D;
		//				else
		//				{
		//					z = 0;
		//					Heigh = Heigh1 + (x - planes[2].Plane.A) * (Heigh0 - Heigh1) / (planes[2].Plane.B - planes[2].Plane.A);
		//				}

		//				Coeff = 1.0;

		//				//int K = 0;
		//				if (o != 1)
		//				{
		//					//K = 16;

		//					pPart.Clear();

		//					Point pt0 = ARANFunctions.LocalToPrj(pCurrPt, abscissa, 0, 1000000);
		//					pPart.Add(pt0);

		//					pt0 = ARANFunctions.LocalToPrj(pCurrPt, abscissa, 0, -1000000);
		//					pPart.Add(pt0);

		//					//pCutter.Clear();
		//					//pCutter.Add(pPart);

		//					pCuttLine = (MultiLineString)pTopoOper.Intersect(pPart, planes[o].Poly);

		//					//if (pCuttLine.Count == 0)
		//					//{
		//					//GlobalVars.gAranGraphics.DrawLineString(pPart, -1, 2);
		//					//GlobalVars.gAranGraphics.DrawPointWithText(pCurrPt, -1, OutObstList.Obstacles[l].TypeName + "/" + OutObstList.Obstacles[l].UnicalName);
		//					//GlobalVars.gAranGraphics.DrawPolygon(planes[o].Poly, -1, AranEnvironment.Symbols.eFillStyle.sfsCross);
		//					//GlobalVars.gAranGraphics.DrawPolygon(planes[2].Poly, -1, AranEnvironment.Symbols.eFillStyle.sfsBackwardDiagonal);

		//					//GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pCurrGeom, -1, AranEnvironment.Symbols.eFillStyle.sfsBackwardDiagonal);
		//					//LegBase.ProcessMessages();

		//					//	int intir = pPart.Count;
		//					//}

		//					ARANFunctions.PrjToLocal(RWYTHRPrj, abscissa, pCuttLine[0][0], out x, out Y0);
		//					ARANFunctions.PrjToLocal(RWYTHRPrj, abscissa, pCuttLine[0][1], out x, out Y1);

		//					Y0 = Math.Abs(Y0);
		//					Y1 = Math.Abs(Y1);
		//					YN = Math.Abs(y);

		//					if (Y1 < Y0)
		//					{
		//						fTmp = Y1;
		//						Y1 = Y0;
		//						Y0 = fTmp;
		//					}

		//					Coeff = (Y1 - YN) / (Y1 - Y0);
		//					z += (1 - Coeff) * Heigh;
		//				}

		//				OutObstList.Parts[k].fSecCoeff = Coeff;
		//				OutObstList.Parts[k].MOC = Coeff * Heigh;

		//				OutObstList.Parts[k].Plane = (int)Surface;      // + K;

		//				OutObstList.Parts[k].Elev = OutObstList.Obstacles[l].Height;
		//				OutObstList.Parts[k].Height = OutObstList.Obstacles[l].Height - RWYTHRPrj.Z;

		//				OutObstList.Parts[k].hSurface = z;
		//				OutObstList.Parts[k].hPenet = OutObstList.Parts[k].Height - OutObstList.Parts[k].hSurface;

		//				if (OutObstList.Parts[k].hPenet > 0.0)
		//					result++;
		//				k++;
		//				break;
		//			}
		//		}

		//		l++;
		//	}

		//	if (k >= 0)
		//	{
		//		Array.Resize<Obstacle>(ref OutObstList.Obstacles, l);
		//		Array.Resize<ObstacleData>(ref OutObstList.Parts, k);
		//	}
		//	else
		//	{
		//		OutObstList.Obstacles = new Obstacle[0];
		//		OutObstList.Parts = new ObstacleData[0];
		//	}

		//	return result;
		//}

		//public static int AnaliseObstacles1(ObstacleContainer InObstList, Point RWYTHRPrj, D3DPolygone[] OFZPlanes, out ObstacleContainer OutObstList)
		//{
		//	int m = InObstList.Obstacles.Length;

		//	if (m == 0)
		//	{
		//		OutObstList.Obstacles = new Obstacle[0];
		//		OutObstList.Parts = new ObstacleData[0];
		//		return 0;
		//	}

		//	double abscissa = RWYTHRPrj.M + Math.PI;

		//	GeometryOperators pTopoOper = new GeometryOperators();
		//	GeometryOperators pRelation = new GeometryOperators();
		//	GeometryOperators pRelationFull = new GeometryOperators();


		//	int p = OFZPlanes.Length;
		//	int c = 10 * m;
		//	int k = 0;
		//	int l = 0;
		//	int result = 0;

		//	pRelationFull.CurrentGeometry = OFZPlanes[p - 1].Poly;
		//	OutObstList.Obstacles = new Obstacle[m];
		//	OutObstList.Parts = new ObstacleData[c];
		//	MultiPoint pObstPoints = new MultiPoint();

		//	for (int i = 0; i < m; i++)
		//	{
		//		Geometry pCurrGeom = InObstList.Obstacles[i].pGeomPrj;

		//		if (pCurrGeom.IsEmpty || pRelationFull.Disjoint(pCurrGeom))
		//			continue;

		//		pObstPoints.Clear();

		//		if (pCurrGeom.Type == GeometryType.Point)
		//			pObstPoints.Add((Point)pCurrGeom);
		//		else
		//		{
		//			for (int o = 0; o < p - 1; o++)
		//			{
		//				pRelation.CurrentGeometry = OFZPlanes[o].Poly;
		//				if (pRelation.Disjoint(pCurrGeom))
		//					continue;

		//				var pTmpPoints = pTopoOper.Intersect(pCurrGeom, OFZPlanes[o].Poly);
		//				pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());
		//			}

		//			RemoveSeamPoints(ref pObstPoints);
		//		}

		//		int n = pObstPoints.Count;
		//		if (n <= 0)
		//			continue;

		//		OutObstList.Obstacles[l] = InObstList.Obstacles[i];
		//		OutObstList.Obstacles[l].PartsNum = n;
		//		OutObstList.Obstacles[l].Parts = new int[n];

		//		/*
		//		if (InObstList.Obstacles[i].pGeomPrj.Type == GeometryType.Point)
		//			OutObstList.Obstacles[l].Height = ((Point)InObstList.Obstacles[i].pGeomPrj).Z - RWYTHRPrj.Z;
		//		else
		//		{
		//			//pZv = ObstacleList.Obstacles[i].pGeomPrj;
		//			//OutObstList.Obstacles[L].Height = pZv.ZMax - RWYTHRPrj.Z;
		//		}
		//		*/

		//		for (int j = 0; j < n; j++)
		//		{
		//			Point pCurrPt = pObstPoints[j];

		//			//GlobalVars.gAranGraphics.DrawPointWithText(pCurrPt, -1, OutObstList.Obstacles[l].TypeName + "/" + OutObstList.Obstacles[l].UnicalName);
		//			//System.Windows.Forms.Application.DoEvents();

		//			if (k >= c)
		//			{
		//				c += m;
		//				Array.Resize<ObstacleData>(ref OutObstList.Parts, c);
		//			}

		//			OutObstList.Parts[k].pPtPrj = pCurrPt;
		//			OutObstList.Parts[k].Owner = l;
		//			OutObstList.Parts[k].Height = OutObstList.Obstacles[l].Height - RWYTHRPrj.Z;
		//			OutObstList.Parts[k].Index = j;
		//			OutObstList.Obstacles[l].Parts[j] = k;

		//			ARANFunctions.PrjToLocal(RWYTHRPrj, abscissa, pCurrPt, out double x, out double y);

		//			OutObstList.Parts[k].Dist = x;
		//			OutObstList.Parts[k].CLShift = y;

		//			//GlobalVars.gAranGraphics.DrawPointWithText(pCurrPt, -1, OutObstList.Obstacles[l].TypeName + "/" + OutObstList.Obstacles[l].UnicalName);
		//			//System.Windows.Forms.Application.DoEvents();

		//			for (int o = 0; o < p - 1; o++)
		//			{
		//				pRelation.CurrentGeometry = OFZPlanes[o].Poly;
		//				if (!pRelation.Contains(pCurrPt))
		//					continue;

		//				double z = OFZPlanes[o].Plane.A * x + OFZPlanes[o].Plane.B * y + OFZPlanes[o].Plane.D;

		//				OutObstList.Parts[k].hSurface = z;
		//				OutObstList.Parts[k].hPenet = OutObstList.Parts[k].Height - z;

		//				OutObstList.Parts[k].Plane = o;
		//				OutObstList.Parts[k].minZPlane = z;

		//				if (OutObstList.Parts[k].hPenet > 0.0)
		//					result++;
		//				k++;
		//				break;
		//			}
		//		}

		//		l++;
		//	}

		//	if (k >= 0)
		//	{
		//		Array.Resize<Obstacle>(ref OutObstList.Obstacles, l);
		//		Array.Resize<ObstacleData>(ref OutObstList.Parts, k);
		//	}
		//	else
		//	{
		//		OutObstList.Obstacles = new Obstacle[0];
		//		OutObstList.Parts = new ObstacleData[0];
		//	}

		//	return result;
		//}

		public static void AnaliseObstacles2(ObstacleContainer InObstList, Point RWYDir, LegBase Leg, double MOC, out ObstacleContainer OutObstList)
		{
			int m = InObstList.Obstacles.Length;

			if (m == 0)
			{
				OutObstList.Obstacles = new Obstacle[0];
				OutObstList.Parts = new ObstacleData[0];
				return;
			}

			double abscissa = RWYDir.M + Math.PI;

			GeometryOperators pTopoPrimary = new GeometryOperators
			{
				CurrentGeometry = Leg.PrimaryAssesmentArea
			};

			GeometryOperators pTopoFull = new GeometryOperators
			{
				CurrentGeometry = Leg.FullAssesmentArea
			};

			int c = 10 * m;
			int k = 0;
			int l = 0;

			OutObstList.Obstacles = new Obstacle[m];
			OutObstList.Parts = new ObstacleData[c];

			Point pt0;
			MultiPoint pObstPoints = new MultiPoint();
			LineString pPart = new LineString();
			MultiLineString pCuttLine, pCutter = new MultiLineString();

			MultiPolygon pTmpPoly = (MultiPolygon)pTopoFull.Difference(Leg.FullAssesmentArea, Leg.PrimaryAssesmentArea);

			for (int i = 0; i < m; i++)
			{
				Geometry pCurrGeom = InObstList.Obstacles[i].pGeomPrj;

				if (pCurrGeom.IsEmpty || pTopoFull.Disjoint(pCurrGeom))
					continue;

				pObstPoints.Clear();

				if (pCurrGeom.Type == GeometryType.Point)
					pObstPoints.Add((Point)pCurrGeom);
				else
				{
					Geometry pTmpPoints = pTopoFull.Intersect(pCurrGeom);
					pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());

					pTmpPoints = pTopoPrimary.Intersect(pCurrGeom);
					pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());

					RemoveSeamPoints(ref pObstPoints);
				}

				int n = pObstPoints.Count;
				if (n <= 0)
					continue;

				OutObstList.Obstacles[l] = InObstList.Obstacles[i];
				OutObstList.Obstacles[l].PartsNum = n;
				//OutObstList.Obstacles[l].Parts = new int[n];

				for (int j = 0; j < n; j++)
				{
					Point pCurrPt = pObstPoints[j];

					if (k >= c)
					{
						c += m;
						Array.Resize<ObstacleData>(ref OutObstList.Parts, c);
					}

					ARANFunctions.PrjToLocal(RWYDir, abscissa, pCurrPt, out double x, out double y);

					OutObstList.Parts[k].Flags = 0;
					double Coeff = 1.0;

					if (!pTopoPrimary.Disjoint(pCurrPt))
					{
						OutObstList.Parts[k].Flags = 1;

						pPart.Clear();
						if (y > 0)
						{
							pt0 = ARANFunctions.LocalToPrj(pCurrPt, abscissa, 0, 1000000);
							pPart.Add(pt0);

							pt0 = ARANFunctions.LocalToPrj(RWYDir, abscissa, x, 0);
							pPart.Add(pt0);
						}
						else
						{
							pt0 = ARANFunctions.LocalToPrj(RWYDir, abscissa, x, 0);
							pPart.Add(pt0);

							pt0 = ARANFunctions.LocalToPrj(pCurrPt, abscissa, 0, -1000000);
							pPart.Add(pt0);
						}

						pCutter.Clear();
						pCutter.Add(pPart);

						pCuttLine = (MultiLineString)pTopoPrimary.Intersect(pCutter, pTmpPoly);

						//GUI.DrawPolyline(pCuttLine, 255, 2);

						double Y0, Y1, YN;
						ARANFunctions.PrjToLocal(RWYDir, abscissa, pCuttLine[0][0], out x, out Y0);
						ARANFunctions.PrjToLocal(RWYDir, abscissa, pCuttLine[0][1], out x, out Y1);

						Y0 = Math.Abs(Y0);
						Y1 = Math.Abs(Y1);
						YN = Math.Abs(y);

						if (Y1 < Y0)
						{
							double fTmp = Y1;
							Y1 = Y0;
							Y0 = fTmp;
						}

						Coeff = (Y1 - YN) / (Y1 - Y0);
					}
					//OutObstList.Obstacles[l].Parts[j] = k;

					OutObstList.Parts[k].Dist = x;
					OutObstList.Parts[k].CLShift = y;
					OutObstList.Parts[k].pPtPrj = pCurrPt;
					OutObstList.Parts[k].Owner = l;
					OutObstList.Parts[k].Height = OutObstList.Obstacles[l].Height - RWYDir.Z;
					OutObstList.Parts[k].Index = j;

					OutObstList.Parts[k].fSecCoeff = Coeff;
					OutObstList.Parts[k].MOC = Coeff * MOC;
					OutObstList.Parts[k].ReqOCH = OutObstList.Parts[k].Height + OutObstList.Parts[k].MOC;
					k++;
				}
				l++;
			}

			if (k >= 0)
			{
				Array.Resize<Obstacle>(ref OutObstList.Obstacles, l);
				Array.Resize<ObstacleData>(ref OutObstList.Parts, k);
			}
			else
			{
				OutObstList.Obstacles = new Obstacle[0];
				OutObstList.Parts = new ObstacleData[0];
			}
		}

		public static double TempCorr(double dt, double h, double HAerodrome)
		{
			const double L0m = -1.98 / 1000 / 0.3048;
			const double T0 = 288.15;

			double T0C = GlobalVars.constants.Pansops[ePANSOPSData.arISAmax].Value;
			// -10C
			dt -= L0m * HAerodrome;
			//double ha0 = h - HAerodrome;
			//double dha =
			return ((T0C - dt) / L0m) * Math.Log(1 + L0m * (h - HAerodrome) / (T0 + L0m * HAerodrome));
			//return dha;


			//double ha1 = ha0;
			//do{
			//	ha0 = ha1;
			//	ha1 = h - HAerodrome + ((T0C - dt)/L0m)*ln(1 + L0m*ha0/(T0 + L0m*HAerodrome));
			//	dha = abs(ha1 - ha0);
			//}while( dha >= 0.001);

			// return ha1 - h + HAerodrome;
		}

		public static bool ShowSaveDialog(out string FileName, out string FileTitle)
		{
			System.Windows.Forms.SaveFileDialog saveDialog = new System.Windows.Forms.SaveFileDialog();

			//string ProjectPath = GlobalVars.GetMapFileName();
			//int pos = ProjectPath.LastIndexOf('\\');
			//int pos2 = ProjectPath.LastIndexOf('.');

			//saveDialog.DefaultExt = "";
			//saveDialog.InitialDirectory = ProjectPath.Substring(0, pos);
			//saveDialog.Title = Properties.Resources.str00511;
			//saveDialog.FileName = ProjectPath.Substring(0, pos2 - 1) + ".htm";

			saveDialog.FileName = "";
			saveDialog.Filter = "PANDA Report File (*.htm)|*.htm|All Files (*.*)|*.*";

			FileTitle = "";
			FileName = "";

			if (saveDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
				return false;

			FileName = saveDialog.FileName;

			int pos = FileName.LastIndexOf('.');
			if (pos > 0)
				FileName = FileName.Substring(0, pos);

			FileTitle = FileName;
			int pos2 = FileTitle.LastIndexOf('\\');
			if (pos2 > 0)
				FileTitle = FileTitle.Substring(pos2 + 1);

			return true;
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

		public static void Shall_SortfSort(ObstacleData[] obsArray)
		{
			int LastRow = obsArray.GetUpperBound(0);

			if (LastRow < 0)
				return;

			int FirstRow = obsArray.GetLowerBound(0);
			int NumRows = LastRow - FirstRow + 1;
			int GapSize = 0;

			do
				GapSize = GapSize * 3 + 1;
			while (GapSize <= NumRows);

			do
			{
				GapSize = GapSize / 3;
				for (int i = (GapSize + FirstRow); i <= LastRow; i++)
				{
					int CurrPos = i;
					ObstacleData TempVal = obsArray[i];
					while (obsArray[CurrPos - GapSize].fSort > TempVal.fSort)
					{
						obsArray[CurrPos] = obsArray[CurrPos - GapSize];
						CurrPos = CurrPos - GapSize;
						if (CurrPos - GapSize < FirstRow)
							break;
					}
					obsArray[CurrPos] = TempVal;
				}
			}
			while (GapSize > 1);
		}

		public static void Shall_SortfSortD(ObstacleData[] obsArray)
		{
			int LastRow = obsArray.GetUpperBound(0);
			if (LastRow < 0)
				return;

			int FirstRow = obsArray.GetLowerBound(0);
			int NumRows = LastRow - FirstRow + 1;
			int GapSize = 0;

			do
				GapSize = GapSize * 3 + 1;
			while (GapSize <= NumRows);

			do
			{
				GapSize = GapSize / 3;
				for (int i = GapSize + FirstRow; i <= LastRow; i++)
				{
					int CurrPos = i;
					ObstacleData TempVal = obsArray[i];
					while (obsArray[CurrPos - GapSize].fSort < TempVal.fSort)
					{
						obsArray[CurrPos] = obsArray[CurrPos - GapSize];
						CurrPos = CurrPos - GapSize;
						if (CurrPos - GapSize < FirstRow)
							break;
					}
					obsArray[CurrPos] = TempVal;
				}
			}
			while (GapSize > 1);
		}

		public static void Shall_SortsSort(ObstacleData[] obsArray)
		{
			int LastRow = obsArray.GetUpperBound(0);

			if (LastRow < 0)
				return;

			int FirstRow = obsArray.GetLowerBound(0);
			int NumRows = LastRow - FirstRow + 1;

			int GapSize = 0;
			do
				GapSize = GapSize * 3 + 1;
			while (GapSize <= NumRows);

			do
			{
				GapSize = GapSize / 3;
				for (int i = (GapSize + FirstRow); i <= LastRow; i++)
				{
					int CurrPos = i;
					ObstacleData TempVal = obsArray[i];
					while (String.Compare(obsArray[CurrPos - GapSize].sSort, TempVal.sSort) > 0)
					{
						obsArray[CurrPos] = obsArray[CurrPos - GapSize];
						CurrPos = CurrPos - GapSize;
						if (CurrPos - GapSize < FirstRow)
							break;
					}
					obsArray[CurrPos] = TempVal;
				}
			}
			while (GapSize > 1);
		}

		public static void Shall_SortsSortD(ObstacleData[] obsArray)
		{

			int LastRow = obsArray.GetUpperBound(0);
			if (LastRow < 0)
				return;

			int FirstRow = obsArray.GetLowerBound(0);
			int NumRows = LastRow - FirstRow + 1;

			int GapSize = 0;
			do
				GapSize = GapSize * 3 + 1;
			while (GapSize <= NumRows);

			do
			{
				GapSize = GapSize / 3;
				for (int i = (GapSize + FirstRow); i <= LastRow; i++)
				{
					int CurrPos = i;
					ObstacleData TempVal = obsArray[i];

					while (String.Compare(obsArray[CurrPos - GapSize].sSort, TempVal.sSort) < 0)
					{
						obsArray[CurrPos] = obsArray[CurrPos - GapSize];
						CurrPos = CurrPos - GapSize;
						if (CurrPos - GapSize < FirstRow)
							break;
					}
					obsArray[CurrPos] = TempVal;
				}
			}
			while (GapSize > 1);
		}

	}
}
