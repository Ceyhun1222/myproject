using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Geometries;
using Europe_ICAO015;
using Aran.Geometries.Operators;


namespace ICAO015
{
    public class ObstacleInputCalculatorForOnlyRadius
    {
        List<AddReportListObstInputForOnlyRadiusCalculated> calcreportlist = new List<AddReportListObstInputForOnlyRadiusCalculated>();

        public List<AddReportListObstInputForOnlyRadiusCalculated> CalculatorForDVOR(List<ParameterForDVOR> ParamlistDvor, Point ObstclPtPrj, double _Elevation)
        {


            double distance;
            double StartEndDistance;
            double Elevation;
            double penetrate;
            double AlphaRad;
            Point PtStartPrj = new Point();
            double paramalpharad;
            double paramsmallradius;
            double parammiddleradius;
            double paramlargeradius;
            double paramdvorheight;
            string id_navaidname;

            for (int i = 0; i < ParamlistDvor.Count; i++)
            {
                int prosesscount = 0;
                AddReportListObstInputForOnlyRadiusCalculated calcreport = new AddReportListObstInputForOnlyRadiusCalculated();

                PtStartPrj = ParamlistDvor[i].Coordinate;
                paramalpharad = ParamlistDvor[i].DVORForAlpha;
                paramsmallradius = ParamlistDvor[i].DVORForSmallRadius;
                parammiddleradius = ParamlistDvor[i].DVORForMiddleRadius;
                paramlargeradius = ParamlistDvor[i].DVORForLargeRadius;
                paramdvorheight = ParamlistDvor[i].HeightDistance;
                id_navaidname = ParamlistDvor[i].NavaidName;  //navaid name

                AlphaRad = paramalpharad * Math.PI / 180;
                distance = Math.Pow(ObstclPtPrj.X - PtStartPrj.X, 2) + Math.Pow(ObstclPtPrj.Y - PtStartPrj.Y, 2);

                Elevation = _Elevation;

                if (distance < Math.Pow(paramsmallradius, 2))
                {
                    StartEndDistance = Math.Sqrt(distance);
                    penetrate = Common.DeConvertHeight(Elevation) - paramdvorheight;

                    calcreport.Distance = StartEndDistance;
                    calcreport.NavaidName = id_navaidname;
                    calcreport.Penetrate = penetrate;
                    calcreport.Radius = paramsmallradius;
                    calcreport.TypeOfNAvigation = ParamlistDvor[i].TypeOfNavigation;
                    prosesscount++;
                }
                if (distance > Math.Pow(paramsmallradius, 2) && distance < Math.Pow(parammiddleradius, 2))
                {
                    StartEndDistance = Math.Sqrt(distance);
                    var h = Common.DeConvertHeight(Elevation) - paramdvorheight;
                    penetrate = h - StartEndDistance * Math.Tan(AlphaRad);

                    calcreport.Distance = StartEndDistance;
                    calcreport.NavaidName = id_navaidname;
                    calcreport.Penetrate = penetrate;
                    calcreport.Radius = parammiddleradius;
                    calcreport.TypeOfNAvigation = ParamlistDvor[i].TypeOfNavigation;
                    prosesscount++;
                }
                if (distance < Math.Pow(paramlargeradius, 2) && distance > Math.Pow(parammiddleradius, 2))
                {
                    StartEndDistance = Math.Sqrt(distance);
                    var h = Common.DeConvertHeight(Elevation) - paramdvorheight;
                    penetrate = h - StartEndDistance * Math.Tan(AlphaRad);

                    calcreport.Distance = StartEndDistance;
                    calcreport.NavaidName = id_navaidname;
                    calcreport.Penetrate = penetrate;
                    calcreport.Radius = paramlargeradius;
                    calcreport.TypeOfNAvigation = ParamlistDvor[i].TypeOfNavigation;
                    prosesscount++;
                }

                if (prosesscount > 0)
                {
                    calcreportlist.Add(calcreport);
                }

            }

            return calcreportlist;

        }
        public List<AddReportListObstInputForOnlyRadiusCalculated> CalculatorForCVOR(List<ParameterForCVOR> ParamlistCVOR, Point ObstclPtPrj, double _Elevation)
        {

            double distance;
            double StartEndDistance;
            double Elevation;
            double penetrate;
            double AlphaRad;
            Point PtStartPrj = new Point();
            double paramalpharad;
            double paramsmallradius;
            double parammiddleradius;
            double paramlargeradius;
            double paramdvorheight;
            string id_navaidname;

            for (int i = 0; i < ParamlistCVOR.Count; i++)
            {
                int prosesscount = 0;
                AddReportListObstInputForOnlyRadiusCalculated calcreport = new AddReportListObstInputForOnlyRadiusCalculated();

                PtStartPrj = ParamlistCVOR[i].Coordinate;
                paramalpharad = ParamlistCVOR[i].CVORForAlpha;
                paramsmallradius = ParamlistCVOR[i].CVORForSmallRadius;
                parammiddleradius = ParamlistCVOR[i].CVORForMiddleRadius;
                paramlargeradius = ParamlistCVOR[i].CVORForLargeRadius;
                paramdvorheight = ParamlistCVOR[i].HeightDistance;
                id_navaidname = ParamlistCVOR[i].NavaidName;  //navaid name

                AlphaRad = paramalpharad * Math.PI / 180;
                distance = Math.Pow(ObstclPtPrj.X - PtStartPrj.X, 2) + Math.Pow(ObstclPtPrj.Y - PtStartPrj.Y, 2);

                Elevation = _Elevation;

                if (distance < Math.Pow(paramsmallradius, 2))
                {
                    StartEndDistance = Math.Sqrt(distance);
                    penetrate = Common.DeConvertHeight(Elevation) - paramdvorheight;

                    calcreport.Distance = StartEndDistance;
                    calcreport.NavaidName = id_navaidname;
                    calcreport.Penetrate = penetrate;
                    calcreport.Radius = paramsmallradius;
                    calcreport.TypeOfNAvigation = ParamlistCVOR[i].TypeOfNavigation;
                    prosesscount++;

                }
                if (distance > Math.Pow(paramsmallradius, 2) && distance < Math.Pow(parammiddleradius, 2))
                {
                    StartEndDistance = Math.Sqrt(distance);
                    var h = Common.DeConvertHeight(Elevation) - paramdvorheight;
                    penetrate = h - StartEndDistance * Math.Tan(AlphaRad);

                    calcreport.Distance = StartEndDistance;
                    calcreport.NavaidName = id_navaidname;
                    calcreport.Penetrate = penetrate;
                    calcreport.Radius = parammiddleradius;
                    calcreport.TypeOfNAvigation = ParamlistCVOR[i].TypeOfNavigation;
                    prosesscount++;
                }
                if (distance < Math.Pow(paramlargeradius, 2) && distance > Math.Pow(parammiddleradius, 2))
                {
                    StartEndDistance = Math.Sqrt(distance);
                    var h = Common.DeConvertHeight(Elevation) - paramdvorheight;
                    penetrate = h - StartEndDistance * Math.Tan(AlphaRad);

                    calcreport.Distance = StartEndDistance;
                    calcreport.NavaidName = id_navaidname;
                    calcreport.Penetrate = penetrate;
                    calcreport.Radius = paramlargeradius;
                    calcreport.TypeOfNAvigation = ParamlistCVOR[i].TypeOfNavigation;
                    prosesscount++;
                }

                if (prosesscount > 0)
                {
                    calcreportlist.Add(calcreport);
                }

            }


            return calcreportlist;
        }
        public List<AddReportListObstInputForOnlyRadiusCalculated> CalculatorForDMEN(List<ParameterForDmeN> ParamlistDMEN, Point ObstclPtPrj, double _Elevation)
        {
            double distance;
            double StartEndDistance;
            double Elevation;
            double penetrate;
            double AlphaRad;
            Point PtStartPrj = new Point();
            double paramalpharad;
            double paramsmallradius;
            //double parammiddleradius;
            double paramlargeradius;
            double paramdvorheight;
            string id_navaidname;

            for (int i = 0; i < ParamlistDMEN.Count; i++)
            {
                int prosesscount = 0;
                AddReportListObstInputForOnlyRadiusCalculated calcreport = new AddReportListObstInputForOnlyRadiusCalculated();

                PtStartPrj = ParamlistDMEN[i].Coordinate;
                paramalpharad = ParamlistDMEN[i].DmeForAlpha;
                paramsmallradius = ParamlistDMEN[i].DmeForSmallRadius;
                paramlargeradius = ParamlistDMEN[i].DmeForLargeRadius;
                paramdvorheight = ParamlistDMEN[i].HeightDistance;
                id_navaidname = ParamlistDMEN[i].NavaidName;  //navaid name

                AlphaRad = paramalpharad * Math.PI / 180;
                distance = Math.Pow(ObstclPtPrj.X - PtStartPrj.X, 2) + Math.Pow(ObstclPtPrj.Y - PtStartPrj.Y, 2);

                Elevation = _Elevation;

                if (distance < Math.Pow(paramsmallradius, 2))
                {
                    StartEndDistance = Math.Sqrt(distance);
                    penetrate = Common.DeConvertHeight(Elevation) - paramdvorheight;

                    calcreport.Distance = StartEndDistance;
                    calcreport.NavaidName = id_navaidname;
                    calcreport.Penetrate = penetrate;
                    calcreport.Radius = paramsmallradius;
                    calcreport.TypeOfNAvigation = ParamlistDMEN[i].TypeofNavigation;
                    prosesscount++;
                }
                if (distance < Math.Pow(paramlargeradius, 2) && distance > Math.Pow(paramsmallradius, 2))
                {
                    StartEndDistance = Math.Sqrt(distance);
                    var h = Common.DeConvertHeight(Elevation) - paramdvorheight;
                    penetrate = h - StartEndDistance * Math.Tan(AlphaRad);

                    calcreport.Distance = StartEndDistance;
                    calcreport.NavaidName = id_navaidname;
                    calcreport.Penetrate = penetrate;
                    calcreport.Radius = paramlargeradius;
                    calcreport.TypeOfNAvigation = ParamlistDMEN[i].TypeofNavigation;
                    prosesscount++;
                }

                if (prosesscount > 0)
                {
                    calcreportlist.Add(calcreport);
                }

            }
            return calcreportlist;
        }
        public List<AddReportListObstInputForOnlyRadiusCalculated> CalculatorForMarker(List<ParameterForMarkers> ParamlistMarker, Point ObstclPtPrj, double _Elevation)
        {

            double distance;
            double StartEndDistance;
            double Elevation;
            double penetrate;
            double AlphaRad;
            Point PtStartPrj = new Point();
            double paramalpharad;
            double paramsmallradius;
            //double parammiddleradius;
            double paramlargeradius;
            double paramdvorheight;
            string id_navaidname;

            for (int i = 0; i < ParamlistMarker.Count; i++)
            {
                int prosesscount = 0;
                AddReportListObstInputForOnlyRadiusCalculated calcreport = new AddReportListObstInputForOnlyRadiusCalculated();

                PtStartPrj = ParamlistMarker[i].Coordinate;
                paramalpharad = ParamlistMarker[i].MarkerForAlpha;
                paramsmallradius = ParamlistMarker[i].MarkerForSmallRadius;
                paramlargeradius = ParamlistMarker[i].MarkerForLargeRadius;
                paramdvorheight = ParamlistMarker[i].HeightDistance;
                id_navaidname = ParamlistMarker[i].NavaidName;  //navaid name

                AlphaRad = paramalpharad * Math.PI / 180;
                distance = Math.Pow(ObstclPtPrj.X - PtStartPrj.X, 2) + Math.Pow(ObstclPtPrj.Y - PtStartPrj.Y, 2);

                Elevation = _Elevation;

                if (distance < Math.Pow(paramsmallradius, 2))
                {
                    StartEndDistance = Math.Sqrt(distance);
                    penetrate = Common.DeConvertHeight(Elevation) - paramdvorheight;

                    calcreport.Distance = StartEndDistance;
                    calcreport.NavaidName = id_navaidname;
                    calcreport.Penetrate = penetrate;
                    calcreport.Radius = paramsmallradius;
                    calcreport.TypeOfNAvigation = ParamlistMarker[i].TypeOfNavigation;
                    prosesscount++;
                }
                if (distance < Math.Pow(paramlargeradius, 2) && distance > Math.Pow(paramsmallradius, 2))
                {
                    StartEndDistance = Math.Sqrt(distance);
                    var h = Common.DeConvertHeight(Elevation) - paramdvorheight;
                    penetrate = h - StartEndDistance * Math.Tan(AlphaRad);

                    calcreport.Distance = StartEndDistance;
                    calcreport.NavaidName = id_navaidname;
                    calcreport.Penetrate = penetrate;
                    calcreport.Radius = paramlargeradius;
                    calcreport.TypeOfNAvigation = ParamlistMarker[i].TypeOfNavigation;
                    prosesscount++;
                }

                if (prosesscount > 0)
                {
                    calcreportlist.Add(calcreport);
                }

            }
            return calcreportlist;
        }
        public List<AddReportListObstInputForOnlyRadiusCalculated> CalculatorForNDB(List<ParameterForNDB> ParamlistNDB, Point ObstclPtPrj, double _Elevation)
        {

            double distance;
            double StartEndDistance;
            double Elevation;
            double penetrate;
            double AlphaRad;
            Point PtStartPrj = new Point();
            double paramalpharad;
            double paramsmallradius;
            //double parammiddleradius;
            double paramlargeradius;
            double paramdvorheight;
            string id_navaidname;



            for (int i = 0; i < ParamlistNDB.Count; i++)
            {
                int prosesscount = 0;
                AddReportListObstInputForOnlyRadiusCalculated calcreport = new AddReportListObstInputForOnlyRadiusCalculated();

                PtStartPrj = ParamlistNDB[i].Coordinate;
                paramalpharad = ParamlistNDB[i].NDBForAlpha;
                paramsmallradius = ParamlistNDB[i].NDBForSmallRadius;
                paramlargeradius = ParamlistNDB[i].NDBForLargeRadius;
                paramdvorheight = ParamlistNDB[i].HeightDistance;
                id_navaidname = ParamlistNDB[i].NavaidName;  //navaid name

                AlphaRad = paramalpharad * Math.PI / 180;
                distance = Math.Pow(ObstclPtPrj.X - PtStartPrj.X, 2) + Math.Pow(ObstclPtPrj.Y - PtStartPrj.Y, 2);

                Elevation = _Elevation;

                if (distance < Math.Pow(paramsmallradius, 2))
                {
                    StartEndDistance = Math.Sqrt(distance);
                    penetrate = Common.DeConvertHeight(Elevation) - paramdvorheight;

                    calcreport.Distance = StartEndDistance;
                    calcreport.NavaidName = id_navaidname;
                    calcreport.Penetrate = penetrate;
                    calcreport.Radius = paramsmallradius;
                    calcreport.TypeOfNAvigation = ParamlistNDB[i].TypeofNavigation;
                    prosesscount++;
                }
                if (distance < Math.Pow(paramlargeradius, 2) && distance > Math.Pow(paramsmallradius, 2))
                {
                    StartEndDistance = Math.Sqrt(distance);
                    var h = Common.DeConvertHeight(Elevation) - paramdvorheight;
                    penetrate = h - StartEndDistance * Math.Tan(AlphaRad);

                    calcreport.Distance = StartEndDistance;
                    calcreport.NavaidName = id_navaidname;
                    calcreport.Penetrate = penetrate;
                    calcreport.Radius = paramlargeradius;
                    calcreport.TypeOfNAvigation = ParamlistNDB[i].TypeofNavigation;
                    prosesscount++;
                }

                if (prosesscount > 0)
                {
                    calcreportlist.Add(calcreport);
                }
            }
            return calcreportlist;
        }
    }
    public class ObstacleInputCalculatorForOnly2DGraphic
    {
        public List<AddReportListObstInputForOnly2DGraphicCalculated> Calc_2DGraphics(List<Obstacle_ParamListPolygons> Paramlist2DGraphic, Point Obstcl_Point, double Obstcl_Elevetion)
        {

            List<AddReportListObstInputForOnly2DGraphicCalculated> calcreportlist = new List<AddReportListObstInputForOnly2DGraphicCalculated>();

            for (int i = 0; i < Paramlist2DGraphic.Count; i++)
            {
                int prosesscount = 0;
                double Delta_r;
                double Delta_h;
                double Alpha_emsal;
                double h_mustevi;
                double Penetrate;
                double Obstcl_Distance = 0;

                GeometryOperators Geom_Distance = new GeometryOperators();

                AddReportListObstInputForOnly2DGraphicCalculated calcreport = new AddReportListObstInputForOnly2DGraphicCalculated();

                if (Paramlist2DGraphic[i].First_Corner_Polygon.IsPointInside(Obstcl_Point))
                {
                    prosesscount++;


                    prosesscount++;
                    Obstcl_Distance = 0;
                    Aran.Geometries.MultiLineString Poly_FirstCorner_linestringw = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(Paramlist2DGraphic[i].FirstCornerPoly_Line);

                    Obstcl_Distance = Geom_Distance.GetDistance(Poly_FirstCorner_linestringw, Obstcl_Point);

                    Alpha_emsal = Math.Tan(Paramlist2DGraphic[i].Corners_Polygon_H / Paramlist2DGraphic[i].Delta_L);

                    Delta_h = Common.DeConvertHeight(Obstcl_Elevetion) - Paramlist2DGraphic[i].Navaid_Height;

                    h_mustevi = Obstcl_Distance * Alpha_emsal;

                    Penetrate = h_mustevi - Delta_h;

                    calcreport.TypeOfNavigation = Paramlist2DGraphic[i].Parent_NodeTxt;
                    calcreport.NavaidName = Paramlist2DGraphic[i].Child_NodeTxt;
                    calcreport.Polygon = "First Corner Polygon";
                    calcreport.Penetrate = Penetrate;
                    calcreport.ObstclDistance = Obstcl_Distance;

                }
                if (Paramlist2DGraphic[i].Second_Corner_Polygon.IsPointInside(Obstcl_Point))
                {
                    prosesscount++;
                    Obstcl_Distance = 0;
                    Aran.Geometries.MultiLineString Poly_SecondCorner_linestringw = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(Paramlist2DGraphic[i].SecondCornerPoly_Line);

                    Obstcl_Distance = Geom_Distance.GetDistance(Poly_SecondCorner_linestringw, Obstcl_Point);

                    Alpha_emsal = Math.Tan(Paramlist2DGraphic[i].Corners_Polygon_H / Paramlist2DGraphic[i].Delta_L);

                    Delta_h = Common.DeConvertHeight(Obstcl_Elevetion) - Paramlist2DGraphic[i].Navaid_Height;

                    h_mustevi = Obstcl_Distance * Alpha_emsal;

                    Penetrate = h_mustevi - Delta_h;

                    calcreport.TypeOfNavigation = Paramlist2DGraphic[i].Parent_NodeTxt;
                    calcreport.NavaidName = Paramlist2DGraphic[i].Child_NodeTxt;
                    calcreport.Polygon = "Second Corner Polygon";
                    calcreport.Penetrate = Penetrate;
                    calcreport.ObstclDistance = Obstcl_Distance;

                }
                if (Paramlist2DGraphic[i].Segment_Polygon.IsPointInside(Obstcl_Point))
                {
                    prosesscount++;

                    Obstcl_Distance = 0;
                    Aran.Geometries.MultiLineString Poly_Seg_linestringw = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(Paramlist2DGraphic[i].SegmentPoly_Line);

                    Obstcl_Distance = Geom_Distance.GetDistance(Poly_Seg_linestringw, Obstcl_Point);

                    Delta_r = Paramlist2DGraphic[i].Radius - Paramlist2DGraphic[i].A;

                    Alpha_emsal = Math.Tan(Paramlist2DGraphic[i].Segment_Polygon_h / Delta_r);

                    Delta_h = Common.DeConvertHeight(Obstcl_Elevetion) - Paramlist2DGraphic[i].Navaid_Height;

                    h_mustevi = Obstcl_Distance * Alpha_emsal;

                    Penetrate = h_mustevi - Delta_h;

                    calcreport.TypeOfNavigation = Paramlist2DGraphic[i].Parent_NodeTxt;
                    calcreport.NavaidName = Paramlist2DGraphic[i].Child_NodeTxt;
                    calcreport.Polygon = "Segment Polygon";
                    calcreport.Penetrate = Penetrate;
                    calcreport.ObstclDistance = Obstcl_Distance;

                }

                if (prosesscount > 0)
                {
                    calcreportlist.Add(calcreport);
                }
            }

            return calcreportlist;

        }
    }
}
