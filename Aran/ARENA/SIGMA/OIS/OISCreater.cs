using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Display;
using PDM;
using ESRI.ArcGIS.esriSystem;
using System.IO;
using ESRI.ArcGIS.Carto;
using System.Diagnostics;

namespace OIS
{
    public class OISCreater
    {
        private PDM.RunwayDirection _rwyDir;
		private List<int> _graphicHandles;
		private HookHelperClass _hookHelper;
		private SpatialReferenceOperation _spatialOperation;
		private Graphics _graphics;

		public OISCreater (  )
		{
			_graphicHandles = new List<int> ( );			
			//GlobalParams.Graphics = new Graphics ( );
			NativeMethods.InitAll ( );
		}

		/// <summary>
		/// Draws Areas polygons
		/// </summary>
		/// <param name="m_hookHelper"></param>
		/// <param name="rwyDir"></param>
		/// <param name="obstacleList"></param>
		/// <param name="h"></param>
		/// <param name="area1Dist"></param>
		/// <param name="k"></param>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public List<PDM.VerticalStructure> Check ( HookHelperClass m_hookHelper, RunwayDirection rwyDir, List<PDM.PDMObject> obstacleList, double h, double area1Dist, double k, out string errorMessage )
		{
			_hookHelper = m_hookHelper;
			_graphics = new Graphics ( _hookHelper.ActiveView );
			return Check ( m_hookHelper.FocusMap, rwyDir, obstacleList, h, area1Dist, k, out errorMessage );
		}

		/// <summary>
		/// Doesn't draw Area
		/// </summary>
		/// <param name="map"></param>
		/// <param name="rwyDir"></param>
		/// <param name="obstacleList"></param>
		/// <param name="h"></param>
		/// <param name="area1Dist"></param>
		/// <param name="k"></param>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public List<PDM.VerticalStructure> Check ( IMap map, RunwayDirection rwyDir, List<PDM.PDMObject> obstacleList, double h, double area1Dist, double k, out string errorMessage )
		{
			_rwyDir = rwyDir;
			if ( !_rwyDir.TrueBearing.HasValue )
			{
				errorMessage = "TrueBearing of RwyDirection is absent";
				return null;
			}
			_spatialOperation = new SpatialReferenceOperation ( map );
			ClearGraphic ( );
			var endRwyCntPnt = _rwyDir.CenterLinePoints.FirstOrDefault ( pdm => pdm.Role == PDM.CodeRunwayCenterLinePointRoleType.END ) as PDM.RunwayCenterLinePoint;
			var startRwyCntPnt = _rwyDir.CenterLinePoints.FirstOrDefault ( pdm => pdm.Role == PDM.CodeRunwayCenterLinePointRoleType.START ) as PDM.RunwayCenterLinePoint;
			if ( !endRwyCntPnt.Elev.HasValue )
			{
				errorMessage = "Elevation of RunwayCenterLinePoint (End) is absent";
				return null;
			}
			if ( endRwyCntPnt.Geo == null )
				endRwyCntPnt.RebuildGeo ( );
			IPoint endRwyPntGeo = endRwyCntPnt.Geo as IPoint;
			if ( startRwyCntPnt.Geo == null )
				startRwyCntPnt.RebuildGeo ( );
			IPoint startRwyCntPntGeo = startRwyCntPnt.Geo as IPoint;

			var toda = _rwyDir.RdnDeclaredDistance.FirstOrDefault ( pdm => pdm.DistanceType == CodeDeclaredDistance.TODA ) as DeclaredDistance;
			var tora = _rwyDir.RdnDeclaredDistance.FirstOrDefault ( pdm => pdm.DistanceType == CodeDeclaredDistance.TORA ) as DeclaredDistance;

			double todaDist = _rwyDir.ConvertValueToMeter ( toda.DistanceValue, toda.DistanceUOM.ToString ( ) );
			double toraDist = _rwyDir.ConvertValueToMeter ( tora.DistanceValue, tora.DistanceUOM.ToString ( ) );
			double cwy = todaDist - toraDist;

			var areaPolygon = CreateFigure ( h, area1Dist, k, endRwyPntGeo, startRwyCntPntGeo, cwy );
			var resultList = AnalyzeObstacles ( obstacleList, h, endRwyCntPnt, endRwyPntGeo, areaPolygon, cwy );
			errorMessage = "";
			//if ( resultList.Count > 0 )
			//	WriteToFile ( resultList );
			return resultList.Select ( pdm => pdm.Obstacle ).ToList ( );
		}

		private List<PenetrateModel> AnalyzeObstacles ( List<PDM.PDMObject> obstacleList, double h, RunwayCenterLinePoint endRwyCntPnt, IPoint endRwyPntGeo, IPolygon areaPolygon, double clearway )
		{
			List<PenetrateModel> resultList = new List<PenetrateModel> ( );
			PDM.VerticalStructure obstacle = null;
			IProximityOperator proximtyOperator = areaPolygon as IProximityOperator;
			double endElev, obsElev, gradient = double.NaN, dist = double.NaN, tmp = 0, x = 0, y = 0;
			bool found;
			IPoint pnt, newGeo, pntPrj;
			IClone clone, cl;
			endElev = endRwyCntPnt.ConvertValueToMeter ( endRwyCntPnt.Elev.Value, endRwyCntPnt.Elev_UOM.ToString ( ) );
			foreach ( var item in obstacleList )
			{
				obstacle = item as PDM.VerticalStructure;
				if ( obstacle.Name == "7013" )
				{
				}
				if ( obstacle.Parts == null || obstacle.Parts.Count == 0 )
					continue;
				found = false;
				foreach ( var obsPart in obstacle.Parts )
				{
					if ( !obsPart.Elev.HasValue )
						continue;
					if ( obsPart == null )
						continue;
					if ( obsPart.Geo == null )
						obsPart.RebuildGeo ( );
					if ( obsPart.Geo is IPoint )
					{
						pnt = obsPart.Geo as IPoint;
						clone = pnt as IClone;
						cl = clone.Clone ( );
						newGeo = cl as IPoint;
						pntPrj = _spatialOperation.ToProject ( newGeo ) as IPoint;

						dist = proximtyOperator.ReturnDistance ( pntPrj );
						obsElev = obsPart.ConvertValueToMeter ( obsPart.Elev.Value, obsPart.Elev_UOM.ToString ( ) );
						if ( dist > 0 )
						{
							//gradient = ( obsElev - endElev - h ) / dist;
							//if ( gradient >= 0.025 )
							//{
							//	found = true;
							//	break;
							//}
						}
						else
						{
							dist = NativeMethods.DistFromPointToLine ( pnt.X, pnt.Y, endRwyPntGeo.X, endRwyPntGeo.Y, ARANMath.Modulus ( _rwyDir.TrueBearing.Value + 90 ), ref x, ref y, ref tmp );
							dist = dist - clearway;
							gradient = ( obsElev - endElev - 5) / dist;
							if ( gradient >= 0.025 )
							{
								if ( IsInsideRwyElement ( pnt, endRwyPntGeo, ARANMath.Modulus ( _rwyDir.TrueBearing.Value + 90 ) ) )
								{
									found = true;
									break;
								}
							}
						}
					}
				}
				if ( found )
				{
					resultList.Add ( new PenetrateModel ( obstacle.Name, dist, gradient * 100, obstacle ) );
				}
			}
			return resultList;
		}

		private bool IsInsideRwyElement ( IPoint pntGeo, IPoint endRwyPntGeo, double angle )
		{
			double fdY = pntGeo.Y - endRwyPntGeo.Y;
			double fdX = pntGeo.X - endRwyPntGeo.X;
			double fDist = fdY * fdY + fdX * fdX;

			if ( fDist < 0.001 )
				return true;

			double Angle12 = Math.Atan2 ( fdY, fdX );
			double rAngle = ARANMath.Modulus ( angle - Angle12, 360 );

			if ( ( rAngle < 0.1 ) || ( Math.Abs ( rAngle - 180 ) < 0.1 ) )
				return true;

			if ( rAngle < ARANMath.C_PI )
				return true;

			return false;
		}

		/// <summary>
		/// // Doc : 8168_v2_cons_en 13 NOV 2014; Figure I-3-4-1;
		/// </summary>
		/// <param name="h"></param>
		/// <param name="area1Dist"></param>
		/// <param name="k"></param>
		/// <param name="endRwyPntGeo"></param>
		/// <returns></returns>
		private IPolygon CreateFigure ( double h, double area1Dist, double k, IPoint endRwyPntGeo, IPoint startRwyPntGeo, double clearWay)
		{
			IPointCollection pointColl = new ESRI.ArcGIS.Geometry.PolygonClass ( );
			
			// Calculating and drawing figure

			double x, y;
			double area2Dist = ( h - 5 ) / k;			

			double dist = 600.0;
			NativeMethods.PointAlongGeodesic ( startRwyPntGeo.X, startRwyPntGeo.Y, dist, ARANMath.Modulus ( _rwyDir.TrueBearing.Value ), out x, out y );
			//var start = CreatePoint ( startRwyPntGeo.X, startRwyPntGeo.Y );
			//DrawPOint ( start, "Start" );
			//var end = CreatePoint ( endRwyPntGeo.X, endRwyPntGeo.Y );
			//DrawPOint ( end, "End" );
			var tmp = CreatePoint ( x, y );
			//DrawPOint ( tmp, "tmp" );
			//double t = NativeMethods.ReturnGeodesicDistance ( start.X, start.Y, tmp.X, tmp.Y );

			dist = 150.0;
			NativeMethods.PointAlongGeodesic ( tmp.X, tmp.Y, dist, ARANMath.Modulus ( _rwyDir.TrueBearing.Value +  90), out x, out y );
			var pntRight_0 = CreatePoint ( x, y );
			//DrawPOint ( pntRight_0, "Right_0" );

			dist = clearWay;
			NativeMethods.PointAlongGeodesic ( endRwyPntGeo.X, endRwyPntGeo.Y, dist, ARANMath.Modulus ( _rwyDir.TrueBearing.Value ), out x, out y );
			tmp = CreatePoint ( x, y );

			dist = 150.0;
			NativeMethods.PointAlongGeodesic ( tmp.X, tmp.Y, dist, ARANMath.Modulus ( _rwyDir.TrueBearing.Value + 90 ), out x, out y );
			var pntRight_1 = CreatePoint ( x, y );

			dist = area1Dist / Math.Cos ( ARANMath.DegToRad ( 15 ) );
			NativeMethods.PointAlongGeodesic ( pntRight_1.X, pntRight_1.Y, dist, ARANMath.Modulus ( _rwyDir.TrueBearing.Value + 15 ), out x, out y );
			IPoint pntRight_2 = CreatePoint ( x, y );

			dist = ( area2Dist - area1Dist ) / Math.Cos ( ARANMath.DegToRad ( 30 ) );
			NativeMethods.PointAlongGeodesic ( pntRight_2.X, pntRight_2.Y, dist, ARANMath.Modulus ( _rwyDir.TrueBearing.Value + 30 ), out x, out y );
			var pntRight_3 = CreatePoint ( x, y );

			dist = 600.0;
			NativeMethods.PointAlongGeodesic ( startRwyPntGeo.X, startRwyPntGeo.Y, dist, ARANMath.Modulus ( _rwyDir.TrueBearing.Value ), out x, out y );
			tmp = CreatePoint ( x, y );

			dist = 150.0;
			NativeMethods.PointAlongGeodesic ( tmp.X, tmp.Y, dist, ARANMath.Modulus ( _rwyDir.TrueBearing.Value - 90 ), out x, out y );
			var pntLeft_0 = CreatePoint ( x, y );
			//DrawPOint ( pntLeft_0, "Left_0" );

			dist = clearWay;
			NativeMethods.PointAlongGeodesic ( endRwyPntGeo.X, endRwyPntGeo.Y, dist, ARANMath.Modulus ( _rwyDir.TrueBearing.Value ), out x, out y );
			tmp = CreatePoint ( x, y );

			dist = 150.0;
			NativeMethods.PointAlongGeodesic ( tmp.X, tmp.Y, dist, ARANMath.Modulus ( _rwyDir.TrueBearing.Value - 90 ), out x, out y );
			var pntLeft_1 = CreatePoint ( x, y );

			dist = area1Dist / Math.Cos ( ARANMath.DegToRad ( 15 ) );
			NativeMethods.PointAlongGeodesic ( pntLeft_1.X, pntLeft_1.Y, dist, ARANMath.Modulus ( _rwyDir.TrueBearing.Value - 15 ), out x, out y );
			IPoint pntLeft_2 = CreatePoint ( x, y );

			dist = ( area2Dist - area1Dist ) / Math.Cos ( ARANMath.DegToRad ( 30 ) );
			NativeMethods.PointAlongGeodesic ( pntLeft_2.X, pntLeft_2.Y, dist, ARANMath.Modulus ( _rwyDir.TrueBearing.Value - 30 ), out x, out y );
			var pntLeft_3 = CreatePoint ( x, y );

			AddPoint ( pointColl, pntRight_0 );
			AddPoint ( pointColl, pntRight_1 );
			AddPoint ( pointColl, pntRight_2 );
			AddPoint ( pointColl, pntRight_3 );
			AddPoint ( pointColl, pntLeft_3 );
			AddPoint ( pointColl, pntLeft_2 );
			AddPoint ( pointColl, pntLeft_1 );
			AddPoint ( pointColl, pntLeft_0 );
			AddPoint ( pointColl, pntRight_0 );
			if ( _hookHelper != null )
				_graphicHandles.Add ( _graphics.DrawEsriDefaultMultiPolygon ( pointColl as IPolygon, true ) );

			IPointCollection polyline = new PolylineClass ( );
			AddPoint ( polyline, pntLeft_1 );
			AddPoint ( polyline, pntRight_1 );
			if ( _hookHelper != null )
				_graphicHandles.Add ( _graphics.DrawPolyline ( polyline as IPolyline, ARANFunctions.RGB ( 0, 255, 0 ), esriSimpleLineStyle.esriSLSDash ) );
			return pointColl as IPolygon;
		}

		private void WriteToFile ( List<PenetrateModel> resultList )
		{
			using ( StreamWriter writetext = new StreamWriter ( "ObstacleList.txt" ) )
			{
				writetext.WriteLine ( _rwyDir.Designator );
				foreach ( var item in resultList)
				{
					writetext.WriteLine ( item.Name + "; " + item.Distance.ToString ( ) + "; " + item.Gradient );
				}				
			}
			Process.Start ( @"ObstacleList.txt" );
		}

        private void AddPoint(IPointCollection pointcollection, IPoint pnt)
        {
            IClone clone = pnt as IClone;
            var cl = clone.Clone();
            var newGeo = cl as IPoint;
            var pntPrj = _spatialOperation.ToProject(newGeo) as IPoint;
            pointcollection.AddPoint(pntPrj);
        }

		private IPoint ToPrj ( IPoint pnt )
		{
			IClone clone = pnt as IClone;
			var cl = clone.Clone ( );
			var newGeo = cl as IPoint;
			var pntPrj = _spatialOperation.ToProject ( newGeo ) as IPoint;
			return pntPrj;
		}

		private void DrawPOint ( IPoint pnt,string text )
		{
			var pntPrj = ToPrj ( pnt );
			_graphicHandles.Add ( _graphics.DrawPointWithText ( pntPrj, text, 5, 255 ) );
		}

        private IPoint CreatePoint(double x, double y)
        {
            IPoint result = new PointClass();
            result.PutCoords(x, y);
            return result;
        }

		private void ClearGraphic ( )
		{
			foreach ( var item in _graphicHandles )
			{
				_graphics.SafeDeleteGraphic ( item );
			}
			_graphicHandles.Clear ( );
		}
	}
}
