using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;

namespace ARINC_DECODER_CORE.AIRTRACK_Objects
{
    public class ROUTE_AIRTRACK : Object_AIRTRACK
    {
        private List<SEGMENT_AIRTRACK> _Segments;

        public List<SEGMENT_AIRTRACK> Segments
        {
            get { return _Segments; }
            set { _Segments = value; }
        }

        private string _RouteName;

        public string RouteName
        {
            get { return _RouteName; }
            set { _RouteName = value; }
        }

        public ROUTE_AIRTRACK()
        {
        }

        public ROUTE_AIRTRACK(List<Object_AIRTRACK> segPoints)
        {
            if ((segPoints != null) && (segPoints.Count > 1))
            {

                //ESRI.ArcGIS.Geometry.IGeometryBridge2 pGeoBrg = new ESRI.ArcGIS.Geometry.GeometryEnvironment() as ESRI.ArcGIS.Geometry.IGeometryBridge2;

                //ESRI.ArcGIS.Geometry.IPointCollection4 pPointColl = new ESRI.ArcGIS.Geometry.PolylineClass();

                //ESRI.ArcGIS.esriSystem.WKSPoint[] aWKSPointBuffer = new   ESRI.ArcGIS.esriSystem.WKSPoint[segPoints.Count];

                this.Segments = new List<SEGMENT_AIRTRACK>();
                this.RouteName = (segPoints[0] as SEGMENT_POINT_AIRTRACK).Route_Identifier;

                for (int i = 0; i <= segPoints.Count - 2; i++)
                {
                    this.Segments.Add( new SEGMENT_AIRTRACK(segPoints[i], segPoints[i + 1]));

                    //ESRI.ArcGIS.esriSystem.WKSPoint wksP = new ESRI.ArcGIS.esriSystem.WKSPoint();
                    //wksP.X = (segPoints[i].Shape.Geometry as IPoint).X;
                    //wksP.Y = (segPoints[i].Shape.Geometry as IPoint).Y;

                    //aWKSPointBuffer[i] = wksP;
                }

                //ESRI.ArcGIS.esriSystem.WKSPoint wksLast = new ESRI.ArcGIS.esriSystem.WKSPoint();
                //wksLast.X = (segPoints[segPoints.Count - 1].Shape.Geometry as IPoint).X;
                //wksLast.Y = (segPoints[segPoints.Count - 1].Shape.Geometry as IPoint).Y;

                //aWKSPointBuffer[segPoints.Count - 1] = wksLast;

                //pGeoBrg.SetWKSPoints(pPointColl, ref aWKSPointBuffer);

                //IZAware zAware = pPointColl as IZAware;
                //zAware.ZAware = true;


                //IMAware mAware = pPointColl as IMAware;
                //mAware.MAware = true;

                //for (int i = 0; i <= pPointColl.PointCount - 1; i++)
                //{
                //    zAware = pPointColl.get_Point(i) as IZAware;
                //    zAware.ZAware = true;
                //    pPointColl.get_Point(i).Z = 0;

                //    mAware = pPointColl.get_Point(i) as IMAware;
                //    mAware.MAware = true;
                //    pPointColl.get_Point(i).M = 0;
                //}


                //this.Shape = new Shape_AIRTRACK();
                //this.Shape.Geometry = pPointColl as IGeometry;

            }
        } 
    }
}
