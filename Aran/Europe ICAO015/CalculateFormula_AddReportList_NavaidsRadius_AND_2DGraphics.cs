using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Geometries;
using System.Windows.Forms;
using Aran.Geometries.Operators;
using Aran.Aim.Features;
using Aran.PANDA.Common;
using Europe_ICAO015;

namespace ICAO015
{
    //DMEN {
    public class CalculateForRadiusDMEN
    {
        double meterdist;
        double StartEndDistance;
        double elevation;
        long id;
        double penetrate;
        double AlphaRad;

        public void CalculateGetlistForDMEN300AND3000(Point PtStart, Point PtEnd, List<ReportForDme300r> lstfor300r, List<ReportForDme3000r> lstfor3000r, double elevatedpt, string TypeObs, long ObstclID,
            double LargeRadius, double SmallRadius, double Alpha, string geotype, double DmeHeight, string DmeName, string typeofnavigation)
        {
            AlphaRad = Alpha * Math.PI / 180;
            ReportForDme300r Ctr300 = new ReportForDme300r();
            ReportForDme3000r Ctrfor3000 = new ReportForDme3000r();
            meterdist = Math.Pow(PtEnd.X - PtStart.X, 2) + Math.Pow(PtEnd.Y - PtStart.Y, 2);

            if (meterdist < Math.Pow(LargeRadius, 2) && meterdist > Math.Pow(SmallRadius, 2))
            {

                //Europe_ICAO015.GlobalParams.AranEnvironment.Graphics.DrawPoint(PtEnd, 255 * 255, true, false);
                StartEndDistance = Math.Sqrt(meterdist);
                elevation = elevatedpt;
                Ctrfor3000.Obstacle = TypeObs;
                Ctrfor3000.ID = ObstclID;
                Ctrfor3000.Elevation = elevation;
                Ctrfor3000.Distance = StartEndDistance;
                Ctrfor3000.TypeGeo = geotype;
                Ctrfor3000.NavaidName = DmeName;
                Ctrfor3000.TypeOFNAvigation = typeofnavigation;
                Ctrfor3000.Radius = LargeRadius.ToString();
                var h = Common.DeConvertHeight(elevation) - DmeHeight;

                penetrate = h - StartEndDistance * Math.Tan(AlphaRad);//if(+) no penetrate else if(-) penetrate

                Ctrfor3000.Penetrate = penetrate;
                lstfor3000r.Add(Ctrfor3000);
            }
            if (meterdist < Math.Pow(SmallRadius, 2))
            {
                StartEndDistance = Math.Sqrt(meterdist);
                elevation = elevatedpt;
                id = ObstclID;
                Ctr300.Obstacle = TypeObs;
                Ctr300.ID = id;
                Ctr300.Elevation = elevation;
                Ctr300.Distance = StartEndDistance;
                Ctr300.TypeGeo = geotype;
                Ctr300.NavaidName = DmeName;
                Ctr300.Radius = SmallRadius.ToString();
                Ctr300.TypeOFNAvigation = typeofnavigation;
                penetrate = Europe_ICAO015.Common.DeConvertHeight(elevation) - DmeHeight;

                Ctr300.Penetrate = penetrate;
                lstfor300r.Add(Ctr300);

            }

        }

    }
    //DMEN }
    //NDB {
    public class CalculateForRadiusNDB
    {
        double meterdist;
        double StartEndDistance;
        double elevation;
        long id;
        double penetrate;
        double AlphaRad;

        public void CalculateGetlistForNDB200AND1000(Point PtStart, Point PtEnd, List<ReportForNDB200r> lstfor200r, List<ReportForNDB1000r> lstfor1000r, double elevatedpt, string TypeObs, long ObstclID,
            double LargeRadius, double SmallRadius, double Alpha, string geotype, double NDBHeight, string NdbName, string typeofnavigation)
        {
            AlphaRad = Alpha * Math.PI / 180;
            ReportForNDB200r Ctr200 = new ReportForNDB200r();
            ReportForNDB1000r Ctrfor1000 = new ReportForNDB1000r();
            meterdist = Math.Pow(PtEnd.X - PtStart.X, 2) + Math.Pow(PtEnd.Y - PtStart.Y, 2);

            if (meterdist < Math.Pow(LargeRadius, 2) && meterdist > Math.Pow(SmallRadius, 2))
            {

                //Europe_ICAO015.GlobalParams.AranEnvironment.Graphics.DrawPoint(PtEnd, 255 * 255, true, false);
                StartEndDistance = Math.Sqrt(meterdist);
                elevation = elevatedpt;
                Ctrfor1000.Obstacle = TypeObs;
                Ctrfor1000.ID = ObstclID;
                Ctrfor1000.Elevation = elevation;
                Ctrfor1000.Distance = StartEndDistance;
                Ctrfor1000.TypeGeo = geotype;
                Ctrfor1000.NavaidName = NdbName;
                Ctrfor1000.Radius = LargeRadius.ToString();
                Ctrfor1000.TypeOFNAvigation = typeofnavigation;
                var h = Common.DeConvertHeight(elevation) - NDBHeight;

                penetrate = h - StartEndDistance * Math.Tan(AlphaRad);//if(+) no penetrate else if(-) penetrate

                Ctrfor1000.Penetrate = penetrate;
                lstfor1000r.Add(Ctrfor1000);
            }
            if (meterdist < Math.Pow(SmallRadius, 2))
            {
                StartEndDistance = Math.Sqrt(meterdist);
                elevation = elevatedpt;
                id = ObstclID;
                Ctr200.Obstacle = TypeObs;
                Ctr200.ID = id;
                Ctr200.Elevation = elevation;
                Ctr200.Distance = StartEndDistance;
                Ctr200.TypeGeo = geotype;
                Ctr200.NavaidName = NdbName;
                Ctr200.Radius = SmallRadius.ToString();
                Ctr200.TypeOFNAvigation = typeofnavigation;
                penetrate = Europe_ICAO015.Common.DeConvertHeight(elevation) - NDBHeight;

                Ctr200.Penetrate = penetrate;
                lstfor200r.Add(Ctr200);

            }

        }

    }
    //NDB }
    //Markers {
    public class CalculateForRadiusMarkers
    {
        double meterdist;
        double StartEndDistance;
        double elevation;
        long id;
        double penetrate;
        double AlphaRad;

        public void CalculateGetlistForMarkers50AND200(Point PtStart, Point PtEnd, List<ReportForMarkers50r> lstfor50r, List<ReportForMarkers200r> lstfor200r, double elevatedpt, string TypeObs, long ObstclID,
            double LargeRadius, double SmallRadius, double Alpha, string geotype, double markerHeight, string MarkerName, string typeofnavigation)
        {
            AlphaRad = Alpha * Math.PI / 180;
            ReportForMarkers50r Ctr50 = new ReportForMarkers50r();
            ReportForMarkers200r Ctrfor200 = new ReportForMarkers200r();

            meterdist = Math.Pow(PtEnd.X - PtStart.X, 2) + Math.Pow(PtEnd.Y - PtStart.Y, 2);

            if (meterdist < Math.Pow(LargeRadius, 2) && meterdist > Math.Pow(SmallRadius, 2))
            {

                //Europe_ICAO015.GlobalParams.AranEnvironment.Graphics.DrawPoint(PtEnd, 255 * 255, true, false);
                StartEndDistance = Math.Sqrt(meterdist);
                elevation = elevatedpt;
                Ctrfor200.Obstacle = TypeObs;
                Ctrfor200.ID = ObstclID;
                Ctrfor200.Elevation = elevation;
                Ctrfor200.Distance = StartEndDistance;
                Ctrfor200.TypeGeo = geotype;
                Ctrfor200.NavaidName = MarkerName;
                Ctrfor200.Radius = LargeRadius.ToString();
                Ctrfor200.TypeOFNAvigation = typeofnavigation;
                var h = Common.DeConvertHeight(elevation) - markerHeight;

                penetrate = h - StartEndDistance * Math.Tan(AlphaRad);//if(+) no penetrate else if(-) penetrate

                Ctrfor200.Penetrate = penetrate;
                lstfor200r.Add(Ctrfor200);
            }
            if (meterdist < Math.Pow(SmallRadius, 2))
            {
                StartEndDistance = Math.Sqrt(meterdist);
                elevation = elevatedpt;
                id = ObstclID;
                Ctr50.Obstacle = TypeObs;
                Ctr50.ID = id;
                Ctr50.Elevation = elevation;
                Ctr50.Distance = StartEndDistance;
                Ctr50.TypeGeo = geotype;
                Ctr50.NavaidName = MarkerName;
                Ctr50.Radius = SmallRadius.ToString();
                Ctr50.TypeOFNAvigation = typeofnavigation;
                penetrate = Europe_ICAO015.Common.DeConvertHeight(elevation) - markerHeight;

                Ctr50.Penetrate = penetrate;
                lstfor50r.Add(Ctr50);

            }

        }

    }

    //Markers }

    //CVOR {
    public class CalculateForRadiusCVOR
    {
        double meter;
        double StartEndDistance;
        double elevation;
        long id;
        double penetrate;
        double AlphaRad;

        public void CalculateGetlistForCVORR600AND3000AND15000(Point PtStart, Point PtEnd, List<ReportForCVOR600r> lstfor600r, List<ReportForCVOR3000r> lstfor3000r, List<ReportForCVOR15000r> lstfor15000r, double elevated, string TypeObs, long ObstclID,
            double LargeRadius, double MiddleRadius, double SmallRadius, double Alpha, string geotype, double CVORHeight, string NavaidName, string typeofnavigation)
        {
            AlphaRad = Alpha * Math.PI / 180;
            ReportForCVOR600r Ctr600 = new ReportForCVOR600r();
            ReportForCVOR3000r Ctrfor3000 = new ReportForCVOR3000r();
            ReportForCVOR15000r Ctrfor15000 = new ReportForCVOR15000r();

            meter = Math.Pow(PtEnd.X - PtStart.X, 2) + Math.Pow(PtEnd.Y - PtStart.Y, 2);

            if (meter < Math.Pow(MiddleRadius, 2) && meter > Math.Pow(SmallRadius, 2))
            {
                StartEndDistance = Math.Sqrt(meter);

                //Europe_ICAO015.GlobalParams.AranEnvironment.Graphics.DrawPoint(PtEnd, 255 * 255, true, false);



                elevation = elevated;
                Ctrfor3000.Obstacle = TypeObs;
                Ctrfor3000.ID = ObstclID;
                Ctrfor3000.Elevation = elevation;
                Ctrfor3000.Distance = StartEndDistance;
                Ctrfor3000.TypeGeo = geotype;
                Ctrfor3000.NavaidName = NavaidName;
                Ctrfor3000.Radius = MiddleRadius.ToString();
                Ctrfor3000.TypeOFNAvigation = typeofnavigation;
                var h = Common.DeConvertHeight(elevation) - CVORHeight;

                penetrate = h - StartEndDistance * Math.Tan(AlphaRad);//if(+) no penetrate else if(-) penetrate

                Ctrfor3000.Penetrate = penetrate;

                lstfor3000r.Add(Ctrfor3000);
            }
            if (meter < Math.Pow(LargeRadius, 2) && meter > Math.Pow(MiddleRadius, 2))
            {
                StartEndDistance = Math.Sqrt(meter);
                elevation = elevated;
                Ctrfor15000.Obstacle = TypeObs;
                Ctrfor15000.ID = ObstclID;
                Ctrfor15000.Elevation = elevation;
                Ctrfor15000.Distance = StartEndDistance;
                Ctrfor15000.TypeGeo = geotype;
                Ctrfor15000.NavaidName = NavaidName;
                Ctrfor15000.Radius = LargeRadius.ToString();
                Ctrfor15000.TypeOFNAvigation = typeofnavigation;
                var h = Common.DeConvertHeight(elevation) - CVORHeight;

                penetrate = h - StartEndDistance * Math.Tan(AlphaRad);//if(+) no penetrate else if(-) penetrate

                Ctrfor15000.Penetrate = penetrate;



                lstfor15000r.Add(Ctrfor15000);
            }
            if (meter < Math.Pow(SmallRadius, 2))
            {
                StartEndDistance = Math.Sqrt(meter);
                elevation = elevated;
                id = ObstclID;
                Ctr600.Obstacle = TypeObs;
                Ctr600.ID = id;
                Ctr600.Elevation = elevation;
                Ctr600.Distance = StartEndDistance;
                Ctr600.TypeGeo = geotype;
                Ctr600.NavaidName = NavaidName;
                Ctr600.Radius = SmallRadius.ToString();
                Ctr600.TypeOFNAvigation = typeofnavigation;
                penetrate = Europe_ICAO015.Common.DeConvertHeight(elevation) - CVORHeight;

                Ctr600.Penetrate = penetrate;

                lstfor600r.Add(Ctr600);

            }

        }

        public void ClalcForGetListWindTurbine(string Typeofnavigation, string Navaidname, CheckBox ChkBoxWinturbine, double parwindturbineheight, List<ReportCvorForWindTurbine> ListCvorForWTurbine, double parCvorHieght)
        {
            ReportCvorForWindTurbine wturbine = new ReportCvorForWindTurbine();
            if (ChkBoxWinturbine.Checked == true)
            {
                wturbine.Height = parwindturbineheight;
                wturbine.Obstacle_name = "Wind Turbine";
                double penetrate = parwindturbineheight - parCvorHieght;
                wturbine.Penetrate = penetrate;
                wturbine.NavaidName = Navaidname;
                wturbine.TypeOfNavigation = Typeofnavigation;
                ListCvorForWTurbine.Add(wturbine);
            }

        }
    }
    //CVOR }

    //DVOR {
    public class CalculateForRadiusDVOR
    {
        double meter;
        double StartEndDistance;
        double elevation;
        long id;
        double penetrate;
        double AlphaRad;

        public void CalculateGetlistForDVOR600AND3000AND10000(Point PtStart, Point PtEnd, List<ReportForDVOR600R> lstfor600r, List<ReportForDVOR3000R> lstfor3000r, List<ReportForDVOR10000R> lstfor10000r, double elevated, string TypeObs, long ObstclID,
            double LargeRadius, double MiddleRadius, double SmallRadius, double Alpha, string geotype, double DVORHeight, string NavaidName, string typeofnavigation)
        {
            AlphaRad = Alpha * Math.PI / 180;
            ReportForDVOR600R Ctr600 = new ReportForDVOR600R();
            ReportForDVOR3000R Ctrfor3000 = new ReportForDVOR3000R();
            ReportForDVOR10000R Ctrfor10000 = new ReportForDVOR10000R();

            meter = Math.Pow(PtEnd.X - PtStart.X, 2) + Math.Pow(PtEnd.Y - PtStart.Y, 2);

            if (meter < Math.Pow(MiddleRadius, 2) && meter > Math.Pow(SmallRadius, 2))
            {
                StartEndDistance = Math.Sqrt(meter);

                //Europe_ICAO015.GlobalParams.AranEnvironment.Graphics.DrawPoint(PtEnd, 255 * 255, true, false);



                elevation = elevated;
                Ctrfor3000.Obstacle = TypeObs;
                Ctrfor3000.ID = ObstclID;
                Ctrfor3000.Elevation = elevation;
                Ctrfor3000.Distance = StartEndDistance;
                Ctrfor3000.TypeGeo = geotype;
                Ctrfor3000.NavaidName = NavaidName;
                Ctrfor3000.Radius = MiddleRadius.ToString();
                Ctrfor3000.TypeOFNAvigation = typeofnavigation;
                var h = Common.DeConvertHeight(elevation) - DVORHeight;

                penetrate = h - StartEndDistance * Math.Tan(AlphaRad);//if(+) no penetrate else if(-) penetrate

                Ctrfor3000.Penetrate = penetrate;

                lstfor3000r.Add(Ctrfor3000);
            }
            if (meter < Math.Pow(LargeRadius, 2) && meter > Math.Pow(MiddleRadius, 2))
            {
                StartEndDistance = Math.Sqrt(meter);
                elevation = elevated;
                Ctrfor10000.Obstacle = TypeObs;
                Ctrfor10000.ID = ObstclID;
                Ctrfor10000.Elevation = elevation;
                Ctrfor10000.Distance = StartEndDistance;
                Ctrfor10000.TypeGeo = geotype;
                Ctrfor10000.NavaidName = NavaidName;
                Ctrfor10000.Radius = LargeRadius.ToString();
                Ctrfor10000.TypeOFNAvigation = typeofnavigation;
                var h = Common.DeConvertHeight(elevation) - DVORHeight;

                penetrate = h - StartEndDistance * Math.Tan(AlphaRad);//if(+) no penetrate else if(-) penetrate

                Ctrfor10000.Penetrate = penetrate;

                lstfor10000r.Add(Ctrfor10000);
            }
            if (meter < Math.Pow(SmallRadius, 2))
            {
                StartEndDistance = Math.Sqrt(meter);
                elevation = elevated;
                id = ObstclID;
                Ctr600.Obstacle = TypeObs;
                Ctr600.ID = id;
                Ctr600.Elevation = elevation;
                Ctr600.Distance = StartEndDistance;
                Ctr600.TypeGeo = geotype;
                Ctr600.NavaidName = NavaidName;
                Ctr600.Radius = SmallRadius.ToString();
                Ctr600.TypeOFNAvigation = typeofnavigation;
                penetrate = Europe_ICAO015.Common.DeConvertHeight(elevation) - DVORHeight;

                Ctr600.Penetrate = penetrate;

                lstfor600r.Add(Ctr600);

            }

        }
        public void ClalcForGetListWindTurbine(string TypeOFNAviagtion, string NavaidName, CheckBox ChkBoxWinturbine, double parwindturbineheight, List<ReportDvorForWindTurbine> ListDvorForWTurbine, double parDvorHieght)
        {
            ReportDvorForWindTurbine wturbine = new ReportDvorForWindTurbine();
            if (ChkBoxWinturbine.Checked == true)
            {
                wturbine.Height = parwindturbineheight;
                wturbine.Obstacle_name = "Wind Turbine";
                double penetrate = parwindturbineheight - parDvorHieght;
                wturbine.Penetrate = penetrate;
                wturbine.NavaidName = NavaidName;
                wturbine.TypeOfNavigation = TypeOFNAviagtion;
                ListDvorForWTurbine.Add(wturbine);
            }

        }

    }
    //DVOR }
    //HARMONISED GUIDANCE CALCULATOR {
    public class CalculateForHarmonisedGuidance
    {
        List<Lists_FOR_2DGraphics> AddList = new List<Lists_FOR_2DGraphics>();
        GeometryOperators geom_distance = new GeometryOperators();
        //Polygon_Segment {
        public List<Lists_FOR_2DGraphics> CalculateFOR_GP_LOC_DME_PolygonSegment_FORPOINT(double LOC_GP_DME_Height, string name, string parentxt, Aran.Geometries.Point Obstcl_Point, double Point_Elevation, string Obstacle_Type, long Obstcl_ID, string Geo_Type, Aran.Geometries.MultiLineString Poly_Seg_linestring, double A, double h, double radius)
        {
            double Obstcl_Distance;

            Lists_FOR_2DGraphics AddList2DGRAPHICS_ForObstclPoint = new Lists_FOR_2DGraphics();

            //ptsec = GlobalParams.SpatialRefOperation.ToPrj(ptsec);
            //ptthird = GlobalParams.SpatialRefOperation.ToPrj(ptthird); //These are only for Spetial Method"ShortestDistance"

            Poly_Seg_linestring = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(Poly_Seg_linestring);

            Obstcl_Distance = geom_distance.GetDistance(Poly_Seg_linestring, Obstcl_Point);

            double delta_r = radius - A;

            double Alpha_emsal = Math.Tan(h / delta_r);

            double Delta_H = Common.DeConvertHeight(Point_Elevation) - LOC_GP_DME_Height;

            double h_mustevi = Obstcl_Distance * Alpha_emsal;

            double Penatrate_NufuzEtme = h_mustevi - Delta_H;

            AddList2DGRAPHICS_ForObstclPoint.ID = Obstcl_ID;
            AddList2DGRAPHICS_ForObstclPoint.Obstacle = Obstacle_Type;
            AddList2DGRAPHICS_ForObstclPoint.Polygon_Type = "Polygon Segment";
            AddList2DGRAPHICS_ForObstclPoint.Penetrate = Penatrate_NufuzEtme;
            AddList2DGRAPHICS_ForObstclPoint.TypeGeo = Geo_Type;
            AddList2DGRAPHICS_ForObstclPoint.Distance = Obstcl_Distance;
            AddList2DGRAPHICS_ForObstclPoint.Elevation = Point_Elevation;
            AddList2DGRAPHICS_ForObstclPoint.ParentTxt = parentxt;
            AddList2DGRAPHICS_ForObstclPoint.ChildTxt = name;
            AddList.Add(AddList2DGRAPHICS_ForObstclPoint);

            return AddList;

            //Obstcl_Distance = ShortestDistance(ptsec.X, ptsec.Y, ptthird.X, ptthird.Y, Obstcl_Point.X, Obstcl_Point.Y);
        }
        //private double ShortestDistance(double x1, double y1, double x2, double y2, double x3, double y3)//Best Shortest finder Code, you don't delete this code
        //{
        //    double px = x2 - x1;
        //    double py = y2 - y1;
        //    double temp = (px * px) + (py * py);
        //    double u = ((x3 - x1) * px + (y3 - y1) * py) / (temp);
        //    if (u > 1)
        //    {
        //        u = 1;
        //    }
        //    else if (u < 0)
        //    {
        //        u = 0;
        //    }
        //    double x = x1 + u * px;
        //    double y = y1 + u * py;

        //    double dx = x - x3;
        //    double dy = y - y3;
        //    double dist = Math.Sqrt(dx * dx + dy * dy);
        //    return dist;

        //}
        //public static double segmentDistToPoint(Aran.Geometries.Point segA, Aran.Geometries.Point segB, Aran.Geometries.Point p)
        //{
        //    Aran.Geometries.Point p2 = new Aran.Geometries.Point();
        //    //segB.X - segA.X, segB.Y - segA.Y
        //    p2.X = segB.X - segA.X;
        //    p2.Y = segB.Y - segA.Y;
        //    double something = p2.X * p2.X + p2.Y * p2.Y;
        //    double u = ((p.X - segA.X) * p2.X + (p.Y - segA.Y) * p2.Y); // something;

        //    if (u > 1)
        //        u = 1;
        //    else if (u < 0)
        //        u = 0;

        //    double x = segA.X + u * p2.X;
        //    double y = segA.Y + u * p2.Y;

        //    double dx = x - p.X;
        //    double dy = y - p.Y;

        //    double dist = Math.Sqrt(dx * dx + dy * dy);

        //    return dist;


        //}
        public List<Lists_FOR_2DGraphics> CalculateFOR_GP_LOC_DME_PolygonSegment_FORLINE(double LOC_GP_DME_Height, string name, string parentxt, Aran.Geometries.MultiLineString Obstcl_Line, double Line_Elevation, string Obstacle_Type, long Obstcl_ID, string Geo_Type, Aran.Geometries.MultiLineString Poly_Seg_linestring, double A, double h, double radius)
        {
            double Obstcl_Distance;

            Lists_FOR_2DGraphics AddList2DGRAPHICS_FOR_ObstclLine = new Lists_FOR_2DGraphics();

            Poly_Seg_linestring = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(Poly_Seg_linestring);

            Obstcl_Distance = geom_distance.GetDistance(Poly_Seg_linestring, Obstcl_Line);

            double delta_r = radius - A;

            double Alpha_emsal = Math.Tan(h / delta_r);

            double Delta_H = Common.DeConvertHeight(Line_Elevation) - LOC_GP_DME_Height;

            double h_mustevi = Obstcl_Distance * Alpha_emsal;

            double Penatrate_NufuzEtme = h_mustevi - Delta_H;

            AddList2DGRAPHICS_FOR_ObstclLine.ID = Obstcl_ID;
            AddList2DGRAPHICS_FOR_ObstclLine.Obstacle = Obstacle_Type;
            AddList2DGRAPHICS_FOR_ObstclLine.Polygon_Type = "Polygon Segment";
            AddList2DGRAPHICS_FOR_ObstclLine.Penetrate = Penatrate_NufuzEtme;
            AddList2DGRAPHICS_FOR_ObstclLine.TypeGeo = Geo_Type;
            AddList2DGRAPHICS_FOR_ObstclLine.Distance = Obstcl_Distance;
            AddList2DGRAPHICS_FOR_ObstclLine.Elevation = Line_Elevation;

            AddList2DGRAPHICS_FOR_ObstclLine.ParentTxt = parentxt;
            AddList2DGRAPHICS_FOR_ObstclLine.ChildTxt = name;
            AddList.Add(AddList2DGRAPHICS_FOR_ObstclLine);

            return AddList;

            //Aran.Geometries.MultiPoint Pt_Line = Obstcl_Line.ToMultiPoint();

        }
        public List<Lists_FOR_2DGraphics> CalculateFOR_GP_LOC_DME_PolygonSegment_FORPOLYGON(double LOC_GP_DME_Height, string name, string parentxt, Aran.Geometries.MultiPolygon Obstcl_Poly, double Polygon_Elevation, string Obstacle_Type, long Obstcl_ID, string Geo_Type, Aran.Geometries.MultiLineString Poly_Seg_linestring, double A, double h, double radius)
        {
            double Obstcl_Distance;

            Lists_FOR_2DGraphics AddList2DGRAPHICS_FOR_ObstclLine = new Lists_FOR_2DGraphics();

            Poly_Seg_linestring = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(Poly_Seg_linestring);

            Obstcl_Distance = geom_distance.GetDistance(Poly_Seg_linestring, Obstcl_Poly);

            double delta_r = radius - A;

            double Alpha_emsal = Math.Tan(h / delta_r);

            double Delta_H = Common.DeConvertHeight(Polygon_Elevation) - LOC_GP_DME_Height;

            double h_mustevi = Obstcl_Distance * Alpha_emsal;

            double Penatrate_NufuzEtme = h_mustevi - Delta_H;

            AddList2DGRAPHICS_FOR_ObstclLine.ID = Obstcl_ID;
            AddList2DGRAPHICS_FOR_ObstclLine.Obstacle = Obstacle_Type;
            AddList2DGRAPHICS_FOR_ObstclLine.Polygon_Type = "Polygon Segment";
            AddList2DGRAPHICS_FOR_ObstclLine.Penetrate = Penatrate_NufuzEtme;
            AddList2DGRAPHICS_FOR_ObstclLine.TypeGeo = Geo_Type;
            AddList2DGRAPHICS_FOR_ObstclLine.Distance = Obstcl_Distance;
            AddList2DGRAPHICS_FOR_ObstclLine.Elevation = Polygon_Elevation;
            AddList2DGRAPHICS_FOR_ObstclLine.ParentTxt = parentxt;
            AddList2DGRAPHICS_FOR_ObstclLine.ChildTxt = name;
            AddList.Add(AddList2DGRAPHICS_FOR_ObstclLine);

            return AddList;
        }
        //Polygon_Segment }
        //Polygon First Corner {
        public List<Lists_FOR_2DGraphics> CalculateFOR_GP_LOC_DME_PolygonFirstCorner_FORPOINT(double LOC_GP_DME_Height, string name, string parentxt, Aran.Geometries.Point Obstcl_Point, double Point_Elevation, string Obstacle_Type, long Obstcl_ID, string Geo_Type, Aran.Geometries.MultiLineString Poly_FrstCorner_linestring, double A, double H, double Delta_l)
        {
            double Obstcl_Distance;

            Lists_FOR_2DGraphics AddList2DGRAPHICS_FOR_ObstclPoint = new Lists_FOR_2DGraphics();

            Poly_FrstCorner_linestring = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(Poly_FrstCorner_linestring);

            Obstcl_Distance = geom_distance.GetDistance(Poly_FrstCorner_linestring, Obstcl_Point);

            double Alpha_emsal = Math.Tan(H / Delta_l);

            double Delta_H = Common.DeConvertHeight(Point_Elevation) - LOC_GP_DME_Height;

            double h_mustevi = Obstcl_Distance * Alpha_emsal;

            double Penatrate_NufuzEtme = h_mustevi - Delta_H;

            AddList2DGRAPHICS_FOR_ObstclPoint.ID = Obstcl_ID;
            AddList2DGRAPHICS_FOR_ObstclPoint.Obstacle = Obstacle_Type;
            AddList2DGRAPHICS_FOR_ObstclPoint.Polygon_Type = "First Corner Polygon";
            AddList2DGRAPHICS_FOR_ObstclPoint.Penetrate = Penatrate_NufuzEtme;
            AddList2DGRAPHICS_FOR_ObstclPoint.TypeGeo = Geo_Type;
            AddList2DGRAPHICS_FOR_ObstclPoint.Distance = Obstcl_Distance;
            AddList2DGRAPHICS_FOR_ObstclPoint.Elevation = Point_Elevation;
            AddList2DGRAPHICS_FOR_ObstclPoint.ParentTxt = parentxt;
            AddList2DGRAPHICS_FOR_ObstclPoint.ChildTxt = name;
            AddList.Add(AddList2DGRAPHICS_FOR_ObstclPoint);

            return AddList;
        }
        public List<Lists_FOR_2DGraphics> CalculateFOR_GP_LOC_DME_PolygonFirstCorner_FORLINE(double LOC_GP_DME_Height, string name, string parentxt, Aran.Geometries.MultiLineString Obstcl_Line, double Line_Elevation, string Obstacle_Type, long Obstcl_ID, string Geo_Type, Aran.Geometries.MultiLineString Poly_FrstCorner_linestring, double A, double H, double Delta_l)
        {
            double Obstcl_Distance;

            Lists_FOR_2DGraphics AddList2DGRAPHICS_FOR_ObstclLine = new Lists_FOR_2DGraphics();

            Poly_FrstCorner_linestring = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(Poly_FrstCorner_linestring);

            Obstcl_Distance = geom_distance.GetDistance(Poly_FrstCorner_linestring, Obstcl_Line);

            double Alpha_emsal = Math.Tan(H / Delta_l);

            double Delta_H = Common.DeConvertHeight(Line_Elevation) - LOC_GP_DME_Height;

            double h_mustevi = Obstcl_Distance * Alpha_emsal;

            double Penatrate_NufuzEtme = h_mustevi - Delta_H;

            AddList2DGRAPHICS_FOR_ObstclLine.ID = Obstcl_ID;
            AddList2DGRAPHICS_FOR_ObstclLine.Obstacle = Obstacle_Type;
            AddList2DGRAPHICS_FOR_ObstclLine.Polygon_Type = "First Corner Polygon";
            AddList2DGRAPHICS_FOR_ObstclLine.Penetrate = Penatrate_NufuzEtme;
            AddList2DGRAPHICS_FOR_ObstclLine.TypeGeo = Geo_Type;
            AddList2DGRAPHICS_FOR_ObstclLine.Distance = Obstcl_Distance;
            AddList2DGRAPHICS_FOR_ObstclLine.Elevation = Line_Elevation;
            AddList2DGRAPHICS_FOR_ObstclLine.ParentTxt = parentxt;
            AddList2DGRAPHICS_FOR_ObstclLine.ChildTxt = name;
            AddList.Add(AddList2DGRAPHICS_FOR_ObstclLine);

            return AddList;
        }
        public List<Lists_FOR_2DGraphics> CalculateFOR_GP_LOC_DME_PolygonFirstCorner_FORPLYGON(double LOC_GP_DME_Height, string name, string parentxt, Aran.Geometries.MultiPolygon Obstcl_Polygon, double Polygon_Elevation, string Obstacle_Type, long Obstcl_ID, string Geo_Type, Aran.Geometries.MultiLineString Poly_FrstCorner_linestring, double A, double H, double Delta_l)
        {
            double Obstcl_Distance;

            Lists_FOR_2DGraphics AddList2DGRAPHICS_FOR_ObstclPolygon = new Lists_FOR_2DGraphics();

            Poly_FrstCorner_linestring = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(Poly_FrstCorner_linestring);

            Obstcl_Distance = geom_distance.GetDistance(Poly_FrstCorner_linestring, Obstcl_Polygon);

            double Alpha_emsal = Math.Tan(H / Delta_l);

            double Delta_H = Common.DeConvertHeight(Polygon_Elevation) - LOC_GP_DME_Height;

            double h_mustevi = Obstcl_Distance * Alpha_emsal;

            double Penatrate_NufuzEtme = h_mustevi - Delta_H;

            AddList2DGRAPHICS_FOR_ObstclPolygon.ID = Obstcl_ID;
            AddList2DGRAPHICS_FOR_ObstclPolygon.Obstacle = Obstacle_Type;
            AddList2DGRAPHICS_FOR_ObstclPolygon.Polygon_Type = "First Corner Polygon";
            AddList2DGRAPHICS_FOR_ObstclPolygon.Penetrate = Penatrate_NufuzEtme;
            AddList2DGRAPHICS_FOR_ObstclPolygon.TypeGeo = Geo_Type;
            AddList2DGRAPHICS_FOR_ObstclPolygon.Distance = Obstcl_Distance;
            AddList2DGRAPHICS_FOR_ObstclPolygon.Elevation = Polygon_Elevation;
            AddList2DGRAPHICS_FOR_ObstclPolygon.ParentTxt = parentxt;
            AddList2DGRAPHICS_FOR_ObstclPolygon.ChildTxt = name;
            AddList.Add(AddList2DGRAPHICS_FOR_ObstclPolygon);

            return AddList;
        }
        //Polygon First Corner }

        //Polygon Second Corner {
        public List<Lists_FOR_2DGraphics> CalculateFOR_GP_LOC_DME_PolygonSecondCorner_FORPOINT(double LOC_GP_DME_Height, string name, string parentxt, Aran.Geometries.Point Obstcl_Point, double Point_Elevation, string Obstacle_Type, long Obstcl_ID, string Geo_Type, Aran.Geometries.MultiLineString Poly_SecCorner_linestring, double A, double H, double Delta_l)
        {


            double Obstcl_Distance;

            Lists_FOR_2DGraphics AddList2DGRAPHICS_FOR_ObstclPoint = new Lists_FOR_2DGraphics();

            Poly_SecCorner_linestring = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(Poly_SecCorner_linestring);

            Obstcl_Distance = geom_distance.GetDistance(Poly_SecCorner_linestring, Obstcl_Point);

            double Alpha_emsal = Math.Tan(H / Delta_l);

            double Delta_H = Common.DeConvertHeight(Point_Elevation) - LOC_GP_DME_Height;

            double h_mustevi = Obstcl_Distance * Alpha_emsal;

            double Penatrate_NufuzEtme = h_mustevi - Delta_H;

            AddList2DGRAPHICS_FOR_ObstclPoint.ID = Obstcl_ID;
            AddList2DGRAPHICS_FOR_ObstclPoint.Obstacle = Obstacle_Type;
            AddList2DGRAPHICS_FOR_ObstclPoint.Polygon_Type = "Second Corner Polygon";
            AddList2DGRAPHICS_FOR_ObstclPoint.Penetrate = Penatrate_NufuzEtme;
            AddList2DGRAPHICS_FOR_ObstclPoint.TypeGeo = Geo_Type;
            AddList2DGRAPHICS_FOR_ObstclPoint.Distance = Obstcl_Distance;
            AddList2DGRAPHICS_FOR_ObstclPoint.Elevation = Point_Elevation;
            AddList2DGRAPHICS_FOR_ObstclPoint.ParentTxt = parentxt;
            AddList2DGRAPHICS_FOR_ObstclPoint.ChildTxt = name;
            AddList.Add(AddList2DGRAPHICS_FOR_ObstclPoint);
            return AddList;
        }
        public List<Lists_FOR_2DGraphics> CalculateFOR_GP_LOC_DME_PolygonSecondCorner_FORLINE(double LOC_GP_DME_Height, string name, string parentxt, Aran.Geometries.MultiLineString Obstcl_Line, double Line_Elevation, string Obstacle_Type, long Obstcl_ID, string Geo_Type, Aran.Geometries.MultiLineString Poly_SecCorner_linestring, double A, double H, double Delta_l)
        {

            double Obstcl_Distance;

            Lists_FOR_2DGraphics AddList2DGRAPHICS_FOR_ObstclLine = new Lists_FOR_2DGraphics();

            Poly_SecCorner_linestring = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(Poly_SecCorner_linestring);

            Obstcl_Distance = geom_distance.GetDistance(Poly_SecCorner_linestring, Obstcl_Line);

            double Alpha_emsal = Math.Tan(H / Delta_l);

            double Delta_H = Common.DeConvertHeight(Line_Elevation) - LOC_GP_DME_Height;

            double h_mustevi = Obstcl_Distance * Alpha_emsal;

            double Penatrate_NufuzEtme = h_mustevi - Delta_H;

            AddList2DGRAPHICS_FOR_ObstclLine.ID = Obstcl_ID;
            AddList2DGRAPHICS_FOR_ObstclLine.Obstacle = Obstacle_Type;
            AddList2DGRAPHICS_FOR_ObstclLine.Polygon_Type = "Second Corner Polygon";
            AddList2DGRAPHICS_FOR_ObstclLine.Penetrate = Penatrate_NufuzEtme;
            AddList2DGRAPHICS_FOR_ObstclLine.TypeGeo = Geo_Type;
            AddList2DGRAPHICS_FOR_ObstclLine.Distance = Obstcl_Distance;
            AddList2DGRAPHICS_FOR_ObstclLine.Elevation = Line_Elevation;
            AddList2DGRAPHICS_FOR_ObstclLine.ParentTxt = parentxt;
            AddList2DGRAPHICS_FOR_ObstclLine.ChildTxt = name;
            AddList.Add(AddList2DGRAPHICS_FOR_ObstclLine);

            return AddList;
        }
        public List<Lists_FOR_2DGraphics> CalculateFOR_GP_LOC_DME_PolygonSecondCorner_FORPOLYGON(double LOC_GP_DME_Height, string name, string parentxt, Aran.Geometries.MultiPolygon Obstcl_Polygon, double Polygon_Elevation, string Obstacle_Type, long Obstcl_ID, string Geo_Type, Aran.Geometries.MultiLineString Poly_SecCorner_linestring, double A, double H, double Delta_l)
        {


            double Obstcl_Distance;

            Lists_FOR_2DGraphics AddList2DGRAPHICS_FOR_ObstclPolygon = new Lists_FOR_2DGraphics();

            Poly_SecCorner_linestring = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(Poly_SecCorner_linestring);

            Obstcl_Distance = geom_distance.GetDistance(Poly_SecCorner_linestring, Obstcl_Polygon);

            double Alpha_emsal = Math.Tan(H / Delta_l);

            double Delta_H = Common.DeConvertHeight(Polygon_Elevation) - LOC_GP_DME_Height;

            double h_mustevi = Obstcl_Distance * Alpha_emsal;

            double Penatrate_NufuzEtme = h_mustevi - Delta_H;

            AddList2DGRAPHICS_FOR_ObstclPolygon.ID = Obstcl_ID;
            AddList2DGRAPHICS_FOR_ObstclPolygon.Obstacle = Obstacle_Type;
            AddList2DGRAPHICS_FOR_ObstclPolygon.Polygon_Type = "Second Corner Polygon";
            AddList2DGRAPHICS_FOR_ObstclPolygon.Penetrate = Penatrate_NufuzEtme;
            AddList2DGRAPHICS_FOR_ObstclPolygon.TypeGeo = Geo_Type;
            AddList2DGRAPHICS_FOR_ObstclPolygon.Distance = Obstcl_Distance;
            AddList2DGRAPHICS_FOR_ObstclPolygon.Elevation = Polygon_Elevation;
            AddList2DGRAPHICS_FOR_ObstclPolygon.ParentTxt = parentxt;
            AddList2DGRAPHICS_FOR_ObstclPolygon.ChildTxt = name;
            AddList.Add(AddList2DGRAPHICS_FOR_ObstclPolygon);

            return AddList;
        }
        //Polygon Second Corner }

        //HARMONISED GUIDANCE CALCULATOR }
    }

}
