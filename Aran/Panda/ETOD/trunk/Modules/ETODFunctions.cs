//3186801021

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;

using Aran.Aim.Features;
using Aran.Queries;
using Aran.Aim.Enums;
using ESRI.ArcGIS.DataSourcesRaster;

namespace ETOD.Modules
{
	public static class ETODFunctions
	{
		#region  constants
		public const double RWYCat1Tresh = 800.0;
		public const double RWYCat2Tresh = 1200.0;
		public const double RWYCat3Tresh = 1800.0;

		public const double RWYCat12Width = 75.0;
		public const double RWYCat34Width = 150.0;

		public const double RWYCat1NonEqMinClrWay = 30.0;
		public const double RWYMinClrWay = 60.0;

		public const double Area2Divergence = 0.15;	//15.0 %

		public const double Area2BRadius = 10000.0;
		public const double Area2BSlope = 0.012;		//1.2 %

		public const double Area2CRadius = 10000.0;
		public const double Area2CSlope = 0.012;		//1.2 %

		public const double Area2DRadius = 45000.0;
		public static double IntersectAngle = 9.1681854527004738;

		public const double Area3RunWayWidth = 90.0;
		public const double Area3MovementWidth = 50.0;

		public const double Area4Width = 60.0;
		public const double Area4Length = 900.0;

		#endregion

		private static int CompareCLPt(RWYCLPoint x, RWYCLPoint y)
		{
				if (x.fTmp > y.fTmp)			return 1;
			else if (x.fTmp < y.fTmp)			return -1;
												return 0;
		}

		private static void CalcPlaneParameters(IPoint pt0, IPoint pt1, IPoint pt2, ref D3DPlane Plane)
		{
			//pt0 = GlobalVars.pLocalCoord.TransformForward(pt0) as IPoint;
			//pt1 = GlobalVars.pLocalCoord.TransformForward(pt1) as IPoint;
			//pt2 = GlobalVars.pLocalCoord.TransformForward(pt2) as IPoint;

			Plane.A = pt0.Y * (pt1.Z - pt2.Z) + pt1.Y * (pt2.Z - pt0.Z) + pt2.Y * (pt0.Z - pt1.Z);
			Plane.B = pt0.Z * (pt1.X - pt2.X) + pt1.Z * (pt2.X - pt0.X) + pt2.Z * (pt0.X - pt1.X);
			Plane.C = pt0.X * (pt1.Y - pt2.Y) + pt1.X * (pt2.Y - pt0.Y) + pt2.X * (pt0.Y - pt1.Y);
			Plane.D = -(pt0.X * (pt1.Y * pt2.Z - pt2.Y * pt1.Z) +
						pt1.X * (pt2.Y * pt0.Z - pt0.Y * pt2.Z) +
						pt2.X * (pt0.Y * pt1.Z - pt1.Y * pt0.Z));
		}

		private static void NormalizePlane(ref D3DPlane Plane)
		{
			Plane.A = -Plane.A / Plane.C;
			Plane.B = -Plane.B / Plane.C;
			Plane.D = -Plane.D / Plane.C;
			Plane.C = -1.0;
		}

		public static void SetIntersectAngle()
		{
			double alphaInRad = Math.Atan(Area2Divergence);
			double sinAlpha = Math.Sin(alphaInRad);
			double cosAlpha = Math.Cos(alphaInRad);

			double SinAlpha_2 = sinAlpha * sinAlpha;
			double CosAlpha_2 = cosAlpha * cosAlpha;

			double A = SinAlpha_2 * SinAlpha_2 - 2 * CosAlpha_2 * sinAlpha;
			double B = 2.0 * (SinAlpha_2 * sinAlpha - CosAlpha_2);
			double C = SinAlpha_2;

			double x0, x1;

			int qRes = Functions.Quadric(A, B, C, out x0, out x1);

			if (qRes > 1)
			{
				double grd = Area2Divergence + Area2Divergence * x0 * sinAlpha + x0 * cosAlpha;
				IntersectAngle = Functions.RadToDeg(Math.Atan(grd));
			}
		}

		static double SurfaceElevation(D3DPolygon surface, double x, double y)
		{
			double res = surface.Plane.A * x + surface.Plane.B * y + surface.Plane.D;
			return res;
		}

		static double SurfaceElevation(D3DPolygon surface, D3DClinder clinder)
		{
			double l = Math.Sqrt(surface.Plane.A * surface.Plane.A + surface.Plane.B * surface.Plane.B);
			if (l == 0.0 || clinder.Radius == 0.0)
				return SurfaceElevation(surface, clinder.ptCentre.X, clinder.ptCentre.Y);
			else
			{
				l = clinder.Radius / l;
				double x = clinder.ptCentre.X - surface.Plane.A * l;
				double y = clinder.ptCentre.Y - surface.Plane.B * l;

				//Point ptSlided = new Point();
				//ptSlided.PutCoords(x, y);
				//ptSlided.Z = clinder.ptCentre.Z;

				//Functions.DrawPolygon(surface.Polygon, -1, true, 4);
				//Functions.DrawPointWithText(clinder.ptCentre, "Centre");
				//Functions.DrawPointWithText(ptSlided, "Slided");

				double res = surface.Plane.A * x + surface.Plane.B * y + surface.Plane.D;
				return res;
			}
		}

		static double ConusElevation(D3DComplex conus, double x, double y)
		{
			double dx = x - conus.Conus.ptCentre.X, dy = y - conus.Conus.ptCentre.Y;
			double dist = Math.Sqrt(dx * dx + dy * dy);
			double res = conus.Conus.ptCentre.Z + dist * conus.Conus.Slope;
			return res;
		}

		static double ConusElevation(D3DComplex conus, double x0, double y0, double x1, double y1)
		{
			double dx = x1 - x0, dy = y1 - y0;
			double un = (conus.Conus.ptCentre.X - x0) * dx + (conus.Conus.ptCentre.Y - y0) * dy;
			double ud = dx * dx + dy * dy;
			double dist, res;

			double x = x0, y = y0;

			if (ud != 0.0)
			{
				if (un >= ud)
				{
					x = x1;
					y = y1;
				}
				else if (un > 0.0)
				{
					double u = un / ud;

					x = x0 + u * (x1 - x0);
					y = y0 + u * (y1 - y0);
				}
			}

			dx = x - conus.Conus.ptCentre.X;
			dy = y - conus.Conus.ptCentre.Y;

			dist = Math.Sqrt(dx * dx + dy * dy);
			res = conus.Conus.ptCentre.Z + dist * conus.Conus.Slope;
			return res;
		}

		static double ConusElevation(D3DComplex conus, D3DClinder clinder)
		{
			if(clinder.Radius == 0.0)
				return ConusElevation(conus, clinder.ptCentre.X, clinder.ptCentre.Y);

			double dx = conus.Conus.ptCentre.X - clinder.ptCentre.X;
			double dy = conus.Conus.ptCentre.Y - clinder.ptCentre.Y;
			double dist = Math.Sqrt(dx * dx + dy * dy);

			if (dist < clinder.Radius)
				return conus.Conus.ptCentre.Z;

			double x, y, k = 1/dist;
			x = clinder.ptCentre.X + dx * k;
			y = clinder.ptCentre.Y + dy * k;

			return ConusElevation(conus, x, y);
		}

		static double ConusElevation(D3DComplex conus, D3DClinder clinder0, D3DClinder clinder1)
		{
			double	dx = clinder1.ptCentre.X - clinder0.ptCentre.X,
					dy = clinder1.ptCentre.Y - clinder0.ptCentre.Y;
			double un = (conus.Conus.ptCentre.X - clinder0.ptCentre.X) * dx + (conus.Conus.ptCentre.Y - clinder0.ptCentre.Y) * dy;
			double ud = dx * dx + dy * dy;

			double x = clinder0.ptCentre.X, y = clinder0.ptCentre.Y;

			if (ud != 0.0)
			{
				if (un >= ud)
				{
					x = clinder1.ptCentre.X;
					y = clinder1.ptCentre.Y;
				}
				else if (un > 0.0)
				{
					double u = un / ud;

					x = clinder0.ptCentre.X + u * (clinder1.ptCentre.X - clinder0.ptCentre.X);
					y = clinder0.ptCentre.Y + u * (clinder1.ptCentre.Y - clinder0.ptCentre.Y);
				}
			}

			Point ptTmp = new Point();
			ptTmp.PutCoords(x, y);

			//Functions.DrawPointWithText(ptTmp, "ptT");
			//Functions.DrawPointWithText(clinder0.ptCentre, "Centre0");
			//Functions.DrawPointWithText(clinder1.ptCentre, "Centre1");

			D3DClinder clinder = new D3DClinder(ptTmp, 0.5 * (clinder0.Length + clinder1.Length), 0.5 * (clinder0.Radius + clinder1.Radius));

			return ConusElevation(conus, clinder);

			//dx = x - conus.Conus.ptCentre.X;
			//dy = y - conus.Conus.ptCentre.Y;

			//dist = Math.Sqrt(dx * dx + dy * dy);
			//res = conus.Conus.ptCentre.Z + dist * conus.Conus.Slope;
			//return res;
		}

		public static void Test(IPoint ptCon, double MainDir, int LeftOrRight)
		{
			double alphaInRad = Math.Atan(Area2Divergence);
			double sinAlpha = Math.Sin(alphaInRad);
			double cosAlpha = Math.Cos(alphaInRad);

			double SinAlpha_2 = sinAlpha * sinAlpha;
			double CosAlpha_2 = cosAlpha * cosAlpha;

			double SinAlpha_3 = SinAlpha_2 * sinAlpha;
			double SinAlpha_4 = SinAlpha_2 * SinAlpha_2;

			double A = SinAlpha_4 - 2 * CosAlpha_2 * sinAlpha;
			double B = 2.0 * (SinAlpha_3 - CosAlpha_2);
			double C = SinAlpha_2;

			double x0, x1;
			int qRes = Functions.Quadric(A, B, C, out x0, out x1);
			//if(qRes = 0)

			double grd = Area2Divergence + Area2Divergence * x0 * sinAlpha + x0 * cosAlpha;	//Math.Tan(alphaInRad)
			double direction;

			double grd0 = 9.168185;
			double grd1 = 9.168187;
			grd = Functions.RadToDeg(Math.Atan(grd));			// 0.5 * (grd0 + grd1);

			direction = MainDir + LeftOrRight * grd;

			double Line15Dir = MainDir + LeftOrRight * Functions.RadToDeg(Math.Atan(Area2Divergence));
			IPoint ptTest = null;

			for (int i = 1; i <= 100; i++)
			{
				direction = MainDir + LeftOrRight * grd;
				double distTest = 10000;

				ptTest = Functions.PointAlongPlane(ptCon, direction, distTest);					// Test noqtasi

				double distTo15 = Functions.Point2LineDistancePrj(ptTest, ptCon, Line15Dir);

				double ptConTo15 = Math.Sqrt(distTest * distTest - distTo15 * distTo15);

				IPoint ptOn15 = Functions.PointAlongPlane(ptCon, Line15Dir, ptConTo15);

				double distToMain = Functions.Point2LineDistancePrj(ptOn15, ptCon, MainDir);

				double X = Math.Sqrt(ptConTo15 * ptConTo15 - distToMain * distToMain);
				double error = (X + distTo15 - distTest);

				//Functions.DrawPointWithText(ptTest, error.ToString());

				if (Math.Abs(grd1 - grd0) < 0.000000001)
					break;

				if (error > 0)
				{
					grd1 = grd;
					grd = 0.5 * (grd1 + grd0);
				}
				else
				{
					grd0 = grd;
					grd = 0.5 * (grd1 + grd0);
				}
				//Functions.DrawPointWithText(ptTest, grd.ToString());
			}
			//Functions.DrawPointWithText(ptTest, grd.ToString());
		}

		public static List<IRasterLayer> GetValidRasterLayers()
		{
			IRasterLayer RLayer;
			IRasterProps RasterProps;
			List<IRasterLayer> result = new List<IRasterLayer>();
			int n = GlobalVars.pMap.LayerCount;

			for (int i = 0; i < n; i++)
			{
				RLayer = null;
				//if(Supports(m_Map.Layer[i], IRasterLayer, RLayer)and Assigned(RLayer)then
				if (GlobalVars.pMap.Layer[i] is IRasterLayer)
				{
					RLayer = GlobalVars.pMap.Layer[i] as IRasterLayer;

					try
					{
						RasterProps = RLayer.Raster as IRasterProps;
					}
					catch
					{
						continue;
					}

					result.Add(RLayer);

				}
			}
			return result;
		}

		#region Area 2

		static public ObstacleArea2 ConvertArea2Code(CodeObstacleArea code)
		{
			ObstacleArea2 result = ObstacleArea2.NotArea2;
			switch (code)
			{
				case CodeObstacleArea.OTHER_AREA2A:
					result = ObstacleArea2.Area2A;
					break;
				case CodeObstacleArea.OTHER_AREA2B:
					result = ObstacleArea2.Area2B;
					break;
				case CodeObstacleArea.OTHER_AREA2C:
					result = ObstacleArea2.Area2C;
					break;
				case CodeObstacleArea.OTHER_AREA2D:
					result = ObstacleArea2.Area2D;
					break;
			}
			return result;
		}

		static public List<ObstacleType> ExtractAre2Obstacles(List<ObstacleType> ObstacleList, IPolygon pPoly)
		{
			List<ObstacleType> result = new List<ObstacleType>();
			IProximityOperator pProxiOperator = pPoly as IProximityOperator;
			int i = 0, n = ObstacleList.Count;

			while (i < n)
			{
				if (pProxiOperator.ReturnDistance(ObstacleList[i].pGeoPrj) <= ObstacleList[i].HorAccuracy)
				{
					result.Add(ObstacleList[i]);
					ObstacleList[i] = ObstacleList[n - 1];
					n--;
				}
				else
					i++;
			}

			return result;
		}

		static D3DPolygon CreateArea2A(RWYCLPoint pCLPoint0, RWYCLPoint pCLPoint1, double mainDir, double RWYWidth)
		{
			D3DPolygon result = new D3DPolygon();
			result.codeObstacleArea = CodeObstacleArea.OTHER_AREA2A;

			IPointCollection pointCollection = new Polygon() as IPointCollection;

			IPoint point = Functions.PointAlongPlane(pCLPoint0.pPtPrj, mainDir + 90.0, RWYWidth);
			point.Z = pCLPoint0.pPtPrj.Z + 3.0;
			pointCollection.AddPoint(point);

			point = Functions.PointAlongPlane(pCLPoint1.pPtPrj, mainDir + 90.0, RWYWidth);
			point.Z = pCLPoint1.pPtPrj.Z + 3.0;
			pointCollection.AddPoint(point);

			point = Functions.PointAlongPlane(pCLPoint1.pPtPrj, mainDir - 90.0, RWYWidth);
			point.Z = pCLPoint1.pPtPrj.Z + 3.0;
			pointCollection.AddPoint(point);

			point = Functions.PointAlongPlane(pCLPoint0.pPtPrj, mainDir - 90.0, RWYWidth);
			point.Z = pCLPoint0.pPtPrj.Z + 3.0;
			pointCollection.AddPoint(point);

			CalcPlaneParameters(pointCollection.Point[0], pointCollection.Point[1], pointCollection.Point[2], ref result.Plane);
			NormalizePlane(ref result.Plane);

			ITopologicalOperator2 topologicalOperator = pointCollection as ITopologicalOperator2;
			topologicalOperator.IsKnownSimple_2 = false;
			topologicalOperator.Simplify();

			result.Polygon = pointCollection as IPolygon;
			return result;
		}

		static D3DPolygon CreateArea2B(RWYCLPoint pCLPoint, double mainDir, double RWYWidth)
		{
			D3DPolygon result = new D3DPolygon();
			result.codeObstacleArea = CodeObstacleArea.OTHER_AREA2B;
			IPointCollection pointCollection = new Polygon() as IPointCollection;

			IPoint pt = Functions.PointAlongPlane(pCLPoint.pPtPrj, mainDir, Area2BRadius);

			IPoint point = Functions.PointAlongPlane(pCLPoint.pPtPrj, mainDir + 90.0, RWYWidth);
			point.Z = pCLPoint.pPtPrj.Z;
			pointCollection.AddPoint(point);

			point = Functions.PointAlongPlane(pt, mainDir + 90.0, RWYWidth + Area2BRadius * Area2Divergence);
			point.Z = pCLPoint.pPtPrj.Z + Area2BRadius * Area2BSlope;
			pointCollection.AddPoint(point);

			point = Functions.PointAlongPlane(pt, mainDir - 90.0, RWYWidth + Area2BRadius * Area2Divergence);
			point.Z = pCLPoint.pPtPrj.Z + Area2BRadius * Area2BSlope;
			pointCollection.AddPoint(point);

			point = Functions.PointAlongPlane(pCLPoint.pPtPrj, mainDir - 90.0, RWYWidth);
			point.Z = pCLPoint.pPtPrj.Z;
			pointCollection.AddPoint(point);

			CalcPlaneParameters(pointCollection.Point[0], pointCollection.Point[1], pointCollection.Point[2], ref result.Plane);
			NormalizePlane(ref result.Plane);

			ITopologicalOperator2 topologicalOperator = pointCollection as ITopologicalOperator2;
			topologicalOperator.IsKnownSimple_2 = false;
			topologicalOperator.Simplify();

			result.Polygon = pointCollection as IPolygon;
			return result;
		}

		static D3DPolygon CreateArea2C(RWYCLPoint pCLPoint0, RWYCLPoint pCLPoint1, double mainDir, double RWYWidth)
		{
			D3DPolygon result = new D3DPolygon();
			result.codeObstacleArea = CodeObstacleArea.OTHER_AREA2C;
			IPointCollection pointCollection = new Polygon() as IPointCollection;

			IPoint point = Functions.PointAlongPlane(pCLPoint0.pPtPrj, mainDir + 90.0, RWYWidth);
			point.Z = pCLPoint0.pPtPrj.Z + 3.0;
			pointCollection.AddPoint(point);

			point = Functions.PointAlongPlane(pCLPoint1.pPtPrj, mainDir + 90.0, RWYWidth);
			point.Z = pCLPoint1.pPtPrj.Z + 3.0;
			pointCollection.AddPoint(point);

			point = Functions.PointAlongPlane(pCLPoint1.pPtPrj, mainDir + 90.0, RWYWidth + Area2BRadius);
			point.Z = pCLPoint1.pPtPrj.Z + 3.0 + Area2BRadius * Area2BSlope;
			pointCollection.AddPoint(point);

			point = Functions.PointAlongPlane(pCLPoint0.pPtPrj, mainDir + 90.0, RWYWidth + Area2BRadius);
			point.Z = pCLPoint0.pPtPrj.Z + 3.0 + Area2BRadius * Area2BSlope;
			pointCollection.AddPoint(point);

			CalcPlaneParameters(pointCollection.Point[0], pointCollection.Point[1], pointCollection.Point[2], ref result.Plane);
			NormalizePlane(ref result.Plane);

			ITopologicalOperator2 topologicalOperator = pointCollection as ITopologicalOperator2;
			topologicalOperator.IsKnownSimple_2 = false;
			topologicalOperator.Simplify();

			result.Polygon = pointCollection as IPolygon;
			return result;
		}

		static D3DComplex CreateArea2CConic(RWYCLPoint ClrPt, double RWYDirDir, double RWYWidth, int LeftOrRight)
		{
			IPoint point, ptCentre;
			IPointCollection pointCollection;
			ITopologicalOperator2 topologicalOperator;
			D3DComplex d3DComplex = new D3DComplex();
			d3DComplex.codeObstacleArea = CodeObstacleArea.OTHER_AREA2C;

			double alpha15 = Functions.RadToDeg(Math.Atan(Area2Divergence));

			pointCollection = new Polygon() as IPointCollection;
			ptCentre = Functions.PointAlongPlane(ClrPt.pPtPrj, RWYDirDir + 90.0 * LeftOrRight, RWYWidth);
			ptCentre.Z = ClrPt.pPtPrj.Z + 3.0;
			pointCollection.AddPoint(ptCentre);

			point = Functions.PointAlongPlane(ptCentre, RWYDirDir + IntersectAngle * LeftOrRight, Area2CRadius);
			point.Z = ClrPt.pPtPrj.Z + 3.0 + Area2CSlope * Area2CRadius;
			pointCollection.AddPoint(point);

			point = Functions.PointAlongPlane(ptCentre, RWYDirDir + alpha15 * LeftOrRight, Area2CRadius);
			point.Z = ClrPt.pPtPrj.Z + 3.0 + Area2BSlope * Area2CRadius * Math.Cos(Math.Atan(Area2Divergence));
			pointCollection.AddPoint(point);

			CalcPlaneParameters(pointCollection.Point[0], pointCollection.Point[1], pointCollection.Point[2], ref d3DComplex.Plane);
			NormalizePlane(ref d3DComplex.Plane);

			topologicalOperator = pointCollection as ITopologicalOperator2;
			topologicalOperator.IsKnownSimple_2 = false;
			topologicalOperator.Simplify();

			d3DComplex.Polygon = pointCollection as IPolygon;

			//======== Cone ========================================
			d3DComplex.Conus.ptCentre = ptCentre;
			d3DComplex.Conus.Slope = Area2CSlope;

			double z = ClrPt.pPtPrj.Z + 3.0 + Area2CSlope * Area2CRadius;

			pointCollection = new Polygon() as IPointCollection;
			pointCollection.AddPoint(ptCentre);

			point = Functions.PointAlongPlane(ptCentre, RWYDirDir + IntersectAngle * LeftOrRight, Area2CRadius);
			point.Z = z;
			pointCollection.AddPoint(point);

			for (int i = (int)Math.Ceiling(IntersectAngle); i <= 90; i++)
			{
				point = Functions.PointAlongPlane(ptCentre, RWYDirDir + i * LeftOrRight, Area2CRadius);
				point.Z = z;
				pointCollection.AddPoint(point);
			}

			topologicalOperator = pointCollection as ITopologicalOperator2;
			topologicalOperator.IsKnownSimple_2 = false;
			topologicalOperator.Simplify();

			d3DComplex.ConusPolygon = pointCollection as IPolygon;

			return d3DComplex;
		}

		public static List<D3DPolygon> CreateArea2(RWYType Rwy1, RWYType Rwy2)
		{
			#region setup
			IPoint ptSort, ptStart;

			List<D3DPolygon> Area2A;
			List<D3DPolygon> Area2B;
			List<D3DPolygon> Area2C;

			List<RWYCLPoint> pCLPoint = new List<RWYCLPoint>();
			RWYCLPoint tmpRWYCLPt;
			RWYCLPoint ClrPt0, ClrPt1;
			ClrPt0 = new RWYCLPoint(); ClrPt1 = new RWYCLPoint();

			int i, n;
			double clr0, clr1, minDist, maxDist, midDist, mainDir;
			double alpha15 = Functions.RadToDeg(Math.Atan(Area2Divergence));

			pCLPoint.AddRange(Rwy1.CLPointArray);
			pCLPoint.AddRange(Rwy2.CLPointArray);

			n = pCLPoint.Count;
			if (n < 2)
				return null;

			if (Rwy2.NameID < Rwy1.NameID)
			{
				ptSort = Functions.PointAlongPlane(Rwy2.pPtPrj[eRWY.PtTHR], Rwy2.pPtPrj[eRWY.PtTHR].M + 180.0, 20000.0);
				clr0 = Rwy2.ClearWay;
				clr1 = Rwy1.ClearWay;
			}
			else
			{
				ptSort = Functions.PointAlongPlane(Rwy1.pPtPrj[eRWY.PtTHR], Rwy1.pPtPrj[eRWY.PtTHR].M + 180.0, 20000.0);
				clr0 = Rwy1.ClearWay;
				clr1 = Rwy2.ClearWay;
			}

			for (i = 0; i < n; i++)
			{
				tmpRWYCLPt = pCLPoint[i];
				tmpRWYCLPt.fTmp = Functions.ReturnDistanceInMeters(ptSort, pCLPoint[i].pPtPrj);
				pCLPoint[i] = tmpRWYCLPt;
			}

			pCLPoint.Sort(CompareCLPt);

			while (pCLPoint.Count > 0 && pCLPoint[0].pCLPoint.Role != CodeRunwayPointRole.START)
				pCLPoint.RemoveAt(0);

			while (pCLPoint.Count > 0 && pCLPoint[pCLPoint.Count - 1].pCLPoint.Role != CodeRunwayPointRole.END)
				pCLPoint.RemoveAt(pCLPoint.Count - 1);

			n = pCLPoint.Count;
			if (n < 2)
				return null;

			minDist = pCLPoint[0].fTmp;
			maxDist = pCLPoint[n - 1].fTmp;
			midDist = 0.5 * (minDist + maxDist);

			for (i = 0; i < pCLPoint.Count - 1; )
			{
				if (pCLPoint[i + 1].fTmp - pCLPoint[i].fTmp <= 0.5)
					if (0.5 * (pCLPoint[i].fTmp + pCLPoint[i + 1].fTmp) < midDist)
					{
						if (pCLPoint[i].pCLPoint.Role != CodeRunwayPointRole.START)
							pCLPoint.RemoveAt(i);
						else if (pCLPoint[i + 1].pCLPoint.Role != CodeRunwayPointRole.START)
							pCLPoint.RemoveAt(i + 1);
						else
							new System.Exception("2 START points detected !");
					}
					else if (0.5 * (pCLPoint[i].fTmp + pCLPoint[i + 1].fTmp) > midDist)
					{
						if (pCLPoint[i].pCLPoint.Role != CodeRunwayPointRole.END)
							pCLPoint.RemoveAt(i);
						else if (pCLPoint[i + 1].pCLPoint.Role != CodeRunwayPointRole.END)
							pCLPoint.RemoveAt(i + 1);
						else
							new System.Exception("2 END points detected !");
					}
					else
						pCLPoint.RemoveAt(i + 1);
				else
					i++;
			}

			n = pCLPoint.Count;
			if (n < 2)
				return null;

			ptStart = pCLPoint[0].pPtPrj;
			mainDir = Functions.ReturnAngleInDegrees(ptStart, pCLPoint[n - 1].pPtPrj);
			GlobalVars.pLocalCoord = new LocalCoordinatSystem(mainDir, ptStart);

			for (i = 0; i < n - 1; i++)
			{
				tmpRWYCLPt = pCLPoint[i];
				IPoint ptTmp = GlobalVars.pLocalCoord.TransformForward(tmpRWYCLPt.pPtPrj) as IPoint;
				ptTmp.Y = 0.0;

				tmpRWYCLPt.pPtPrj = GlobalVars.pLocalCoord.TransformReverse(ptTmp) as IPoint;
				pCLPoint[i] = tmpRWYCLPt;
			}

			double RWYLen = maxDist - minDist;
			int CodeNum;

			if (RWYLen < RWYCat1Tresh)
				CodeNum = 1;
			else if (RWYLen < RWYCat2Tresh)
				CodeNum = 2;
			else if (RWYLen < RWYCat3Tresh)
				CodeNum = 3;
			else
				CodeNum = 4;

			double RWYWidth = (CodeNum <= 2) ? RWYCat12Width : RWYCat34Width;

			ClrPt0.pPtPrj = Functions.PointAlongPlane(pCLPoint[0].pPtPrj, mainDir + 180.0, Math.Max(RWYMinClrWay, clr0));
			ClrPt0.pPtPrj.Z = pCLPoint[0].pPtPrj.Z;

			ClrPt1.pPtPrj = Functions.PointAlongPlane(pCLPoint[pCLPoint.Count - 1].pPtPrj, mainDir, Math.Max(RWYMinClrWay, clr1));
			ClrPt1.pPtPrj.Z = pCLPoint[pCLPoint.Count - 1].pPtPrj.Z;

			pCLPoint.Insert(0, ClrPt0);
			pCLPoint.Add(ClrPt1);

			n = pCLPoint.Count;

			#endregion

			#region  Area 2A
			Area2A = new List<D3DPolygon>();

			for (i = 0; i < n - 1; i++)
				Area2A.Add(CreateArea2A(pCLPoint[i], pCLPoint[i + 1], mainDir, RWYWidth));

			#endregion

			#region  Area 2B
			Area2B = new List<D3DPolygon>();

			Area2B.Add(CreateArea2B(ClrPt0, mainDir + 180.0, RWYWidth));
			Area2B.Add(CreateArea2B(ClrPt1, mainDir, RWYWidth));

			#endregion  Area 2B

			//========================================
			//Test(Functions.PointAlongPlane(pCLPoint[pCLPoint.Count - 1].pPtPrj, mainDir - 90.0, RWYWidth), mainDir, -1);
			//Test(Functions.PointAlongPlane(pCLPoint[pCLPoint.Count - 1].pPtPrj, mainDir + 90.0, RWYWidth), mainDir, 1);
			//========================================
			SetIntersectAngle();

			#region  Area 2C

			Area2C = new List<D3DPolygon>();
			int ix;
			for (i = 0, ix = 0; i < n - 1; i++, ix++)
			{
				Area2C.Add(CreateArea2C(pCLPoint[i], pCLPoint[i + 1], mainDir, RWYWidth));
				Area2C.Add(CreateArea2C(pCLPoint[i], pCLPoint[i + 1], mainDir + 180.0, RWYWidth));
			}

			D3DComplex d3DComplex = CreateArea2CConic(ClrPt0, mainDir + 180.0, RWYWidth, 1);
			Area2C.Insert(0, d3DComplex);

			d3DComplex = CreateArea2CConic(ClrPt0, mainDir + 180.0, RWYWidth, -1);
			Area2C.Insert(1, d3DComplex);

			d3DComplex = CreateArea2CConic(ClrPt1, mainDir, RWYWidth, 1);
			Area2C.Add(d3DComplex);

			d3DComplex = CreateArea2CConic(ClrPt1, mainDir, RWYWidth, -1);
			Area2C.Add(d3DComplex);

			#endregion  Area 2C

			#region  result
			List<D3DPolygon> result = new List<D3DPolygon>();

			result.AddRange(Area2A);
			result.AddRange(Area2B);
			result.AddRange(Area2C);
			#endregion

			return result;
		}

		public static void AnaliseArea2Obstacles(List<ObstacleType> ObstacleList, out List<ObstacleType> ObstacleListOut, List<D3DPolygon> Area2)
		{
			//IPolygon SumArea2 = new Polygon() as IPolygon;
			//ITopologicalOperator2 topo = SumArea2 as ITopologicalOperator2;

			double fTmp, z, z1;

			ObstacleListOut = new List<ObstacleType>();

			ITopologicalOperator2 topo = null;
			IPolygon BufferPoly;

			ObstacleType tmpObstacle;
			IPoint pTmpPt;
			IPointCollection pTmpColl;
			ObstacleArea2 surfArea2;
			ObstaclePlaneReleation opReleation;

			for (int i = 0; i < ObstacleList.Count; i++)
			{
				tmpObstacle = ObstacleList[i];
				tmpObstacle.Releation = new List<ObstaclePlaneReleation>();

				tmpObstacle.obstacleArea2 = ObstacleArea2.NotArea2;

				D3DClinder d3DClinder = null;

				z = 9999.0;
				if (tmpObstacle.pGeoPrj.GeometryType == esriGeometryType.esriGeometryPoint)
				{
					pTmpPt = tmpObstacle.pGeoPrj as IPoint;
					d3DClinder = new D3DClinder(pTmpPt, tmpObstacle.VertAccuracy + tmpObstacle.Elevation, tmpObstacle.HorAccuracy);
				}
				else
				{
					topo = tmpObstacle.pGeoPrj as ITopologicalOperator2;
					if (tmpObstacle.HorAccuracy > 0)
					{
						BufferPoly = topo.Buffer(tmpObstacle.HorAccuracy) as IPolygon;
						topo = BufferPoly as ITopologicalOperator2;
						topo.IsKnownSimple_2 = false;
						topo.Simplify();
					}
				}

				for (int j = 0; j < Area2.Count; j++)
				{
					surfArea2 = ConvertArea2Code(Area2[j].codeObstacleArea);
					IProximityOperator proxi = Area2[j].Polygon as IProximityOperator;

					if (proxi.ReturnDistance(tmpObstacle.pGeoPrj) <= tmpObstacle.HorAccuracy)
					{
						opReleation = new ObstaclePlaneReleation();
						opReleation.obstacleArea2 = surfArea2;

						tmpObstacle.obstacleArea2 = tmpObstacle.obstacleArea2 | surfArea2;

						if (tmpObstacle.pGeoPrj.GeometryType == esriGeometryType.esriGeometryPoint)
						{
							opReleation.Hsurface = SurfaceElevation(Area2[j], d3DClinder);
							if (opReleation.Hsurface < z)
								z = opReleation.Hsurface;
							//tmpObstacle.Hsurface = SurfaceElevation(Area2[j], pTmpPt.X, pTmpPt.Y);
						}
						else
						{
							if (tmpObstacle.pGeoPrj.GeometryType == esriGeometryType.esriGeometryPolygon || tmpObstacle.HorAccuracy > 0.0)
								pTmpColl = topo.Intersect(Area2[j].Polygon, esriGeometryDimension.esriGeometry2Dimension) as IPointCollection;
							else
								pTmpColl = topo.Intersect(Area2[j].Polygon, esriGeometryDimension.esriGeometry1Dimension) as IPointCollection;

							//Functions.DrawPolygon(Area2[j].Polygon,-1,true,3);
							//Functions.DrawPolygon(topo as IPolygon, -1, true, 4);
							//Functions.DrawPolygon(pTmpColl as IPolygon, -1, true, 5);
							//Functions.DrawPolyline(topo as IPointCollection, 255,2);

							//if (tmpObstacle.pGeoPrj.GeometryType == esriGeometryType.esriGeometryPolygon)
							//{
							//    Functions.DrawPolygon(topo as IPolygon, -1, true, 4);
							//}

							z1 = 9999.0;
							for (int k = 0; k < pTmpColl.PointCount; k++)
							{
								fTmp = SurfaceElevation(Area2[j], pTmpColl.Point[k].X, pTmpColl.Point[k].Y);
								if (fTmp < z)
									z = fTmp;

								if (fTmp < z1)
									z1 = fTmp;
							}

							//pTmpColl = tmpObstacle.pGeoPrj as IPointCollection;

							//for (int k = 0; k < pTmpColl.PointCount; k++)
							//{
							//    D3DClinder d3DClinder = new D3DClinder(pTmpColl.Point[k], tmpObstacle.VertAccuracy + tmpObstacle.Elevation, tmpObstacle.HorAccuracy);
							//    fTmp = SurfaceElevation(Area2[j], d3DClinder);
							//    //fTmp = SurfaceElevation(Area2[j], pTmpColl.Point[k].X, pTmpColl.Point[k].Y);
							//    if (fTmp < z)
							//        z = fTmp;
							//}

							//Functions.DrawPolygon(Area2[j].Polygon,-1,true, 2);
							//Functions.DrawPolygon(tmpObstacle.pGeoPrj as  IPolygon, -1, true, 4);
							//Functions.DrawPolyline(tmpObstacle.pGeoPrj as IPointCollection, -1);

							opReleation.Hsurface = z1;
							opReleation.hPent = tmpObstacle.Elevation - opReleation.Hsurface;
							opReleation.Ignored = opReleation.hPent < 0.0;
							//opReleation.LocalPlane = ;
							tmpObstacle.Releation.Add(opReleation);
						}
					}

					if (Area2[j].IsComplex)
					{
						D3DComplex d3DComplex = Area2[j] as D3DComplex;
						proxi = d3DComplex.ConusPolygon as IProximityOperator;

						if (proxi.ReturnDistance(tmpObstacle.pGeoPrj) <= tmpObstacle.HorAccuracy)
						{
							tmpObstacle.obstacleArea2 = tmpObstacle.obstacleArea2 | surfArea2;

							opReleation = new ObstaclePlaneReleation();
							opReleation.obstacleArea2 = surfArea2;

							//tmpObstacle.Hsurface = ConusElevation(d3DComplex, tmpObstacle.pPtPrj.X, tmpObstacle.pPtPrj.Y);
							if (tmpObstacle.pGeoPrj.GeometryType == esriGeometryType.esriGeometryPoint)
							{
								opReleation.Hsurface = ConusElevation(d3DComplex, d3DClinder);
								if (opReleation.Hsurface < z)
									z = opReleation.Hsurface;
								//tmpObstacle.Hsurface = SurfaceElevation(Area2[j], pTmpPt.X, pTmpPt.Y);
							}
							else
							{
								proxi = tmpObstacle.pGeoPrj as IProximityOperator;

								if (proxi.ReturnDistance(d3DComplex.Conus.ptCentre) <= tmpObstacle.HorAccuracy)
								{
									//Functions.DrawPolygon(tmpObstacle.pGeoPrj as IPolygon);
									opReleation.Hsurface = d3DComplex.Conus.ptCentre.Z;
									if (opReleation.Hsurface < z)
										z = opReleation.Hsurface;
								}
								else
								{
									if (tmpObstacle.pGeoPrj.GeometryType == esriGeometryType.esriGeometryPolygon || tmpObstacle.HorAccuracy > 0.0)
										pTmpColl = topo.Intersect(d3DComplex.ConusPolygon, esriGeometryDimension.esriGeometry2Dimension) as IPointCollection;
									else
										pTmpColl = topo.Intersect(d3DComplex.ConusPolygon, esriGeometryDimension.esriGeometry1Dimension) as IPointCollection;

									//pTmpColl = topo.Intersect(d3DComplex.ConusPolygon, esriGeometryDimension.esriGeometry2Dimension) as IPointCollection;
									//Functions.DrawPolygon(topo as IPolygon, -1, true, 4);
									//Functions.DrawPolygon(d3DComplex.ConusPolygon, -1, true, 4);
									//Functions.DrawPolygon(pTmpColl as IPolygon, -1, true, 3);
									z1 = 9999.0;
									for (int k = 0; k < pTmpColl.PointCount; k++)
									{
										int l = (k + 1) % pTmpColl.PointCount;
										fTmp = ConusElevation(d3DComplex, pTmpColl.Point[k].X, pTmpColl.Point[k].Y, pTmpColl.Point[l].X, pTmpColl.Point[l].Y);
										if (fTmp < z)
											z = fTmp;

										if (fTmp < z1)
											z1 = fTmp;
									}

									//pTmpColl = tmpObstacle.pGeoPrj as IPointCollection;

									//for (int k = 0; k < pTmpColl.PointCount; k++)
									//{
									//    int l = (k + 1) % pTmpColl.PointCount;
									//    D3DClinder d3DClinder0 = new D3DClinder(pTmpColl.Point[k], tmpObstacle.Elevation + tmpObstacle.VertAccuracy, tmpObstacle.HorAccuracy);
									//    D3DClinder d3DClinder1 = new D3DClinder(pTmpColl.Point[l], tmpObstacle.Elevation + tmpObstacle.VertAccuracy, tmpObstacle.HorAccuracy);

									//    fTmp = ConusElevation(d3DComplex, d3DClinder0, d3DClinder1);
									//    //fTmp = ConusElevation(Area2[j], pTmpColl.Point[k].X, pTmpColl.Point[k].Y);
									//    if (fTmp < z)
									//        z = fTmp;
									//}

									opReleation.Hsurface = z1;
									opReleation.hPent = tmpObstacle.Elevation - opReleation.Hsurface;
									opReleation.Ignored = opReleation.hPent < 0.0;

									//opReleation.LocalPlane = ;
									tmpObstacle.Releation.Add(opReleation);
								}
							}
						}
					}
					//if (tmpObstacle.codeObstacleArea != CodeObstacleArea.OTHER_AREA2D)
					//	break;
				}

				tmpObstacle.Hsurface = z;

				if (tmpObstacle.obstacleArea2 == ObstacleArea2.NotArea2)
				{
					tmpObstacle.obstacleArea2 = ObstacleArea2.Area2D;

					if (tmpObstacle.Height > -9990)
						tmpObstacle.Hsurface = tmpObstacle.Elevation - tmpObstacle.Height + 100.0;
					else
						tmpObstacle.Hsurface = 0.0;
				}

				tmpObstacle.hPent = tmpObstacle.Elevation - tmpObstacle.Hsurface;
				tmpObstacle.Ignored = tmpObstacle.hPent < 0.0;
				ObstacleListOut.Add(tmpObstacle);
				//ObstacleList[i] = tmpObstacle;
			}
		}

		#endregion

		#region Area 4
		public static List<ObstacleType> GetArea4Obstacles(List<ObstacleType> ObstacleArea2List, List<IPolygon> Area4List, List<RWYType> RWYDirList)
		{
			int i, j, n = ObstacleArea2List.Count, m = Area4List.Count;
			IProximityOperator pProxiOperator;
			List<ObstacleType> result = new List<ObstacleType>();
			ObstacleType tmpObst;

			for (i = 0; i < n; i++)
			{
				for (j = 0; j < m; j++)
				{
					//Functions.DrawPolygon(Area4List[j], 255, true, 3);

					pProxiOperator = Area4List[j] as IProximityOperator;

					if (pProxiOperator.ReturnDistance(ObstacleArea2List[i].pGeoPrj) <= 0.0)
					{
						tmpObst = ObstacleArea2List[i];
						tmpObst.Hsurface = Area4List[j].ToPoint.Z;
						tmpObst.hPent = tmpObst.Elevation - tmpObst.Hsurface;

						if (tmpObst.hPent >= 0.0)
						{
							//tmpObst.codeObstacleArea = CodeObstacleArea.AREA4;
							tmpObst.Tag = RWYDirList[j];

							result.Add(tmpObst);
						}
						break;
					}
				}
			}
			return result;
		}

		public static List<IPolygon> CreateArea4(List<RWYType> SelectedRWYDirs)
		{
			IPointCollection pPointCollection;
			IPolygon pTmpPoly;
			IPoint pPoint;
			ITopologicalOperator2 pTopoOper;

			int i, n = SelectedRWYDirs.Count;
			List<IPolygon> result = new List<IPolygon>();
			IZAware pZAware;

			for (i = 0; i < n; i++)
			{
				pTmpPoly = new Polygon() as IPolygon;
				pZAware = pTmpPoly as IZAware;
				pZAware.ZAware = true;

				pPointCollection = pTmpPoly as IPointCollection;

				pPoint = Functions.PointAlongPlane(SelectedRWYDirs[i].pPtPrj[eRWY.PtTHR], SelectedRWYDirs[i].pPtPrj[eRWY.PtTHR].M + 90.0, Area4Width);
				pPoint.Z = SelectedRWYDirs[i].pPtPrj[eRWY.PtTHR].Z;
				pPointCollection.AddPoint(pPoint);

				pPoint = Functions.PointAlongPlane(pPointCollection.Point[0], SelectedRWYDirs[i].pPtPrj[eRWY.PtTHR].M + 180.0, Area4Length);
				pPoint.Z = SelectedRWYDirs[i].pPtPrj[eRWY.PtTHR].Z;
				pPointCollection.AddPoint(pPoint);

				pPoint = Functions.PointAlongPlane(pPointCollection.Point[1], SelectedRWYDirs[i].pPtPrj[eRWY.PtTHR].M - 90.0, 2 * Area4Width);
				pPoint.Z = SelectedRWYDirs[i].pPtPrj[eRWY.PtTHR].Z;
				pPointCollection.AddPoint(pPoint);

				pPoint = Functions.PointAlongPlane(pPointCollection.Point[2], SelectedRWYDirs[i].pPtPrj[eRWY.PtTHR].M, Area4Length);
				pPoint.Z = SelectedRWYDirs[i].pPtPrj[eRWY.PtTHR].Z;
				pPointCollection.AddPoint(pPoint);

				pPoint = Functions.PointAlongPlane(pPointCollection.Point[3], SelectedRWYDirs[i].pPtPrj[eRWY.PtTHR].M + 90.0, 2 * Area4Width);
				pPoint.Z = SelectedRWYDirs[i].pPtPrj[eRWY.PtTHR].Z;
				pPointCollection.AddPoint(pPoint);

				pTopoOper = pTmpPoly as ITopologicalOperator2;
				pTopoOper.IsKnownSimple_2 = false;
				pTopoOper.Simplify();

				result.Add(pTmpPoly);
			}
			return result;
		}

		#endregion

		#region Area 3
		public static List<ObstacleType> GetArea3Obstacles(List<ObstacleType> ObstacleArea2List, List<IPolygon> Area3List, List<RWYType> RWYDirList)
		{
			return null;
		}

		public static List<IPolygon> CreateArea3(List<RWYType> SelectedRWYDirs)
		{
			return null;
		}

		#endregion

	}
}
