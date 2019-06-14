using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Aran.AranEnvironment;
using System.Globalization;
using Aran.PANDA.Conventional.Racetrack;
using ChoosePointNS;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using Aran.Aim.Features;
using System.Collections.ObjectModel;
using Aran.AranEnvironment.Symbols;
using MahApps.Metro.Controls;
using Aran.Aim.Enums;
using Aran.Queries.Common;
using Aran.Queries;
using Aran.Queries.Omega;
using Aran.Geometries.Operators;
using System.Reflection;
using System.Reflection.Emit;
using Aran.Omega.Properties;
using Aran.Omega.View;
using System.Threading;
using Aran.Aim;
using Aran.Aim.Data;
using System.Data.OleDb;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.ADF.CATIDs;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ArcMapUI;
using System.Diagnostics;
using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.PANDA.RNAV.Departure;
using ICAO015;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Europe_ICAO015
{
    public class Calculate_Obstcl_MethodFor_NavaidRadius
    {

        GeometryOperators GeomOperForRadiusCalc = new GeometryOperators();
        public void CalcLoadObstacleNearDistanceForOnlyRadius(List<VerticalStructure> VerticalStrList, CheckBox ChkBoxWindTurbine, List<ParameterForDmeN> ParamListDMEN, List<ParameterForCVOR> ParamListCVOR, List<ParameterForDVOR> ParamListDVOR
            , List<ParameterForMarkers> ParamListMarker, List<ParameterForNDB> ParamListNDB, AranTool arantoolitem, ref int calc_count)
        {
            //int progres = 0;
            List<VerticalStructure> MYObstacleList = VerticalStrList;

            ElevatedPoint pointelevated;
            ElevatedCurve PLinearElevated;
            MultiLineString LinearPrj;
            MultiPolygon PolygonPrj;
            ElevatedSurface PsurfaceElevated;

            //DMEN  {
            CalculateForRadiusDMEN CalcForDme = new CalculateForRadiusDMEN();
            List<ReportForDme300r> LstforDMEN300 = new List<ReportForDme300r>();
            List<ReportForDme3000r> LstforDMEN3000 = new List<ReportForDme3000r>();
            LstforDMEN300.Clear();
            LstforDMEN3000.Clear();


            if (ParamListDMEN.Count > 0)
            {
                for (int s = 0; s < ParamListDMEN.Count; s++)
                {

                    for (int r = 0; r < MYObstacleList.Count; r++)
                    {
                        long ObsID = MYObstacleList[r].Id;
                        foreach (VerticalStructurePart part in MYObstacleList[r].Part)
                        {

                            if (part.HorizontalProjection == null)
                                continue;
                            switch (part.HorizontalProjection.Choice)
                            {

                                case VerticalStructurePartGeometryChoice.ElevatedPoint:
                                    pointelevated = part.HorizontalProjection.Location;
                                    string geotype = pointelevated.Geo.Type.ToString();


                                    if (part.Type != null)
                                    {
                                        if (pointelevated == null) continue;
                                        if (pointelevated.Elevation == null) continue;
                                        Aran.Geometries.Point ptrjpoint = GlobalParams.SpatialRefOperation.ToPrj(pointelevated.Geo);
                                        CalcForDme.CalculateGetlistForDMEN300AND3000(ParamListDMEN[s].Coordinate, ptrjpoint, LstforDMEN300, LstforDMEN3000, pointelevated.Elevation.Value, part.Type.Value.ToString(), ObsID, ParamListDMEN[s].DmeForLargeRadius, ParamListDMEN[s].DmeForSmallRadius, ParamListDMEN[s].DmeForAlpha, geotype, ParamListDMEN[s].HeightDistance, ParamListDMEN[s].NavaidName, ParamListDMEN[s].TypeofNavigation);
                                        calc_count++;
                                    }
                                    else if (part.Type == null)
                                    {
                                        if (pointelevated == null) continue;
                                        if (pointelevated.Elevation == null) continue;
                                        Aran.Geometries.Point ptrj = GlobalParams.SpatialRefOperation.ToPrj(pointelevated.Geo);
                                        CalcForDme.CalculateGetlistForDMEN300AND3000(ParamListDMEN[s].Coordinate, ptrj, LstforDMEN300, LstforDMEN3000, pointelevated.Elevation.Value, "Empty", ObsID, ParamListDMEN[s].DmeForLargeRadius, ParamListDMEN[s].DmeForSmallRadius, ParamListDMEN[s].DmeForAlpha, geotype, ParamListDMEN[s].HeightDistance, ParamListDMEN[s].NavaidName, ParamListDMEN[s].TypeofNavigation);
                                        calc_count++;
                                    }
                                    break;
                                case VerticalStructurePartGeometryChoice.ElevatedCurve:
                                    PLinearElevated = part.HorizontalProjection.LinearExtent;
                                    string geotypeline = PLinearElevated.Geo.Type.ToString();
                                    if (part.Type != null)
                                    {
                                        if (PLinearElevated == null) continue;
                                        if (PLinearElevated.Elevation == null) continue;
                                        LinearPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(PLinearElevated.Geo);
                                        Aran.Geometries.Point ptrjline = GeomOperForRadiusCalc.GetNearestPoint(LinearPrj, ParamListDMEN[s].Coordinate);
                                        CalcForDme.CalculateGetlistForDMEN300AND3000(ParamListDMEN[s].Coordinate, ptrjline, LstforDMEN300, LstforDMEN3000, PLinearElevated.Elevation.Value, part.Type.Value.ToString(), ObsID, ParamListDMEN[s].DmeForLargeRadius, ParamListDMEN[s].DmeForSmallRadius, ParamListDMEN[s].DmeForAlpha, geotypeline, ParamListDMEN[s].HeightDistance, ParamListDMEN[s].NavaidName, ParamListDMEN[s].TypeofNavigation);
                                        calc_count++;
                                    }
                                    else if (part.Type == null)
                                    {
                                        if (PLinearElevated == null) continue;
                                        if (PLinearElevated.Elevation == null) continue;
                                        LinearPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(PLinearElevated.Geo);
                                        Aran.Geometries.Point ptrjline = GeomOperForRadiusCalc.GetNearestPoint(LinearPrj, ParamListDMEN[s].Coordinate);
                                        CalcForDme.CalculateGetlistForDMEN300AND3000(ParamListDMEN[s].Coordinate, ptrjline, LstforDMEN300, LstforDMEN3000, PLinearElevated.Elevation.Value, "Empty", ObsID, ParamListDMEN[s].DmeForLargeRadius, ParamListDMEN[s].DmeForSmallRadius, ParamListDMEN[s].DmeForAlpha, geotypeline, ParamListDMEN[s].HeightDistance, ParamListDMEN[s].NavaidName, ParamListDMEN[s].TypeofNavigation);
                                        calc_count++;
                                    }
                                    break;
                                case VerticalStructurePartGeometryChoice.ElevatedSurface:
                                    PsurfaceElevated = part.HorizontalProjection.SurfaceExtent;
                                    string geotypesurface = PsurfaceElevated.Geo.Type.ToString();
                                    if (part.Type != null)
                                    {

                                        if (PsurfaceElevated == null) continue;
                                        if (PsurfaceElevated.Elevation == null) continue;
                                        PolygonPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiPolygon>(PsurfaceElevated.Geo);
                                        Aran.Geometries.Point ptrjpolygon = GeomOperForRadiusCalc.GetNearestPoint(PolygonPrj, ParamListDMEN[s].Coordinate);
                                        CalcForDme.CalculateGetlistForDMEN300AND3000(ParamListDMEN[s].Coordinate, ptrjpolygon, LstforDMEN300, LstforDMEN3000, PsurfaceElevated.Elevation.Value, part.Type.Value.ToString(), ObsID, ParamListDMEN[s].DmeForLargeRadius, ParamListDMEN[s].DmeForSmallRadius, ParamListDMEN[s].DmeForAlpha, geotypesurface, ParamListDMEN[s].HeightDistance, ParamListDMEN[s].NavaidName, ParamListDMEN[s].TypeofNavigation);
                                        calc_count++;
                                    }
                                    else if (part.Type == null)
                                    {
                                        if (PsurfaceElevated == null) continue;
                                        if (PsurfaceElevated.Elevation == null) continue;
                                        PolygonPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiPolygon>(PsurfaceElevated.Geo);
                                        Aran.Geometries.Point ptrjpolygon = GeomOperForRadiusCalc.GetNearestPoint(PolygonPrj, ParamListDMEN[s].Coordinate);
                                        CalcForDme.CalculateGetlistForDMEN300AND3000(ParamListDMEN[s].Coordinate, ptrjpolygon, LstforDMEN300, LstforDMEN3000, PsurfaceElevated.Elevation.Value, "Empty", ObsID, ParamListDMEN[s].DmeForLargeRadius, ParamListDMEN[s].DmeForSmallRadius, ParamListDMEN[s].DmeForAlpha, geotypesurface, ParamListDMEN[s].HeightDistance, ParamListDMEN[s].NavaidName, ParamListDMEN[s].TypeofNavigation);
                                        calc_count++;
                                    }
                                    break;
                                default:
                                    continue;
                            }
                        }
                    }
                }
            }
            //DMEN  }   

            CalculateForRadiusNDB CalcForDNDB = new CalculateForRadiusNDB();
            List<ReportForNDB200r> LstforNDB200 = new List<ReportForNDB200r>();
            List<ReportForNDB1000r> LstforNDB1000 = new List<ReportForNDB1000r>();
            LstforNDB200.Clear();
            LstforNDB1000.Clear();
            //NDB {
            if (ParamListNDB.Count > 0)
            {

                pointelevated = null;
                PLinearElevated = null;
                LinearPrj = null;
                PolygonPrj = null;
                PsurfaceElevated = null;

                for (int s = 0; s < ParamListNDB.Count; s++)
                {

                    for (int r = 0; r < MYObstacleList.Count; r++)
                    {
                        long ObsID = MYObstacleList[r].Id;
                        foreach (VerticalStructurePart part in MYObstacleList[r].Part)
                        {

                            if (part.HorizontalProjection == null)
                                continue;
                            switch (part.HorizontalProjection.Choice)
                            {

                                case VerticalStructurePartGeometryChoice.ElevatedPoint:
                                    pointelevated = part.HorizontalProjection.Location;
                                    string geotype = pointelevated.Geo.Type.ToString();


                                    if (part.Type != null)
                                    {
                                        if (pointelevated == null) continue;
                                        if (pointelevated.Elevation == null) continue;
                                        Aran.Geometries.Point ptrjpoint = GlobalParams.SpatialRefOperation.ToPrj(pointelevated.Geo);
                                        CalcForDNDB.CalculateGetlistForNDB200AND1000(ParamListNDB[s].Coordinate, ptrjpoint, LstforNDB200, LstforNDB1000, pointelevated.Elevation.Value, part.Type.Value.ToString(), ObsID, ParamListNDB[s].NDBForLargeRadius, ParamListNDB[s].NDBForSmallRadius, ParamListNDB[s].NDBForAlpha, geotype, ParamListNDB[s].HeightDistance, ParamListNDB[s].NavaidName, ParamListNDB[s].TypeofNavigation);
                                        calc_count++;
                                    }
                                    else if (part.Type == null)
                                    {
                                        if (pointelevated == null) continue;
                                        if (pointelevated.Elevation == null) continue;
                                        Aran.Geometries.Point ptrj = GlobalParams.SpatialRefOperation.ToPrj(pointelevated.Geo);
                                        CalcForDNDB.CalculateGetlistForNDB200AND1000(ParamListNDB[s].Coordinate, ptrj, LstforNDB200, LstforNDB1000, pointelevated.Elevation.Value, "Empty", ObsID, ParamListNDB[s].NDBForLargeRadius, ParamListNDB[s].NDBForSmallRadius, ParamListNDB[s].NDBForAlpha, geotype, ParamListNDB[s].HeightDistance, ParamListNDB[s].NavaidName, ParamListNDB[s].TypeofNavigation);
                                        calc_count++;
                                    }
                                    break;
                                case VerticalStructurePartGeometryChoice.ElevatedCurve:
                                    PLinearElevated = part.HorizontalProjection.LinearExtent;
                                    string geotypeline = PLinearElevated.Geo.Type.ToString();
                                    if (part.Type != null)
                                    {
                                        if (PLinearElevated == null) continue;
                                        if (PLinearElevated.Elevation == null) continue;
                                        LinearPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(PLinearElevated.Geo);
                                        Aran.Geometries.Point ptrjline = GeomOperForRadiusCalc.GetNearestPoint(LinearPrj, ParamListNDB[s].Coordinate);
                                        CalcForDNDB.CalculateGetlistForNDB200AND1000(ParamListNDB[s].Coordinate, ptrjline, LstforNDB200, LstforNDB1000, PLinearElevated.Elevation.Value, part.Type.Value.ToString(), ObsID, ParamListNDB[s].NDBForLargeRadius, ParamListNDB[s].NDBForSmallRadius, ParamListNDB[s].NDBForAlpha, geotypeline, ParamListNDB[s].HeightDistance, ParamListNDB[s].NavaidName, ParamListNDB[s].TypeofNavigation);
                                        calc_count++;
                                    }
                                    else if (part.Type == null)
                                    {
                                        if (PLinearElevated == null) continue;
                                        if (PLinearElevated.Elevation == null) continue;
                                        LinearPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(PLinearElevated.Geo);
                                        Aran.Geometries.Point ptrjline = GeomOperForRadiusCalc.GetNearestPoint(LinearPrj, ParamListNDB[s].Coordinate);
                                        CalcForDNDB.CalculateGetlistForNDB200AND1000(ParamListNDB[s].Coordinate, ptrjline, LstforNDB200, LstforNDB1000, PLinearElevated.Elevation.Value, "Empty", ObsID, ParamListNDB[s].NDBForLargeRadius, ParamListNDB[s].NDBForSmallRadius, ParamListNDB[s].NDBForAlpha, geotypeline, ParamListNDB[s].HeightDistance, ParamListNDB[s].NavaidName, ParamListNDB[s].TypeofNavigation);
                                        calc_count++;
                                    }
                                    break;
                                case VerticalStructurePartGeometryChoice.ElevatedSurface:
                                    PsurfaceElevated = part.HorizontalProjection.SurfaceExtent;
                                    string geotypesurface = PsurfaceElevated.Geo.Type.ToString();
                                    if (part.Type != null)
                                    {

                                        if (PsurfaceElevated == null) continue;
                                        if (PsurfaceElevated.Elevation == null) continue;
                                        PolygonPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiPolygon>(PsurfaceElevated.Geo);
                                        Aran.Geometries.Point ptrjpolygon = GeomOperForRadiusCalc.GetNearestPoint(PolygonPrj, ParamListNDB[s].Coordinate);
                                        CalcForDNDB.CalculateGetlistForNDB200AND1000(ParamListNDB[s].Coordinate, ptrjpolygon, LstforNDB200, LstforNDB1000, PsurfaceElevated.Elevation.Value, part.Type.Value.ToString(), ObsID, ParamListNDB[s].NDBForLargeRadius, ParamListNDB[s].NDBForSmallRadius, ParamListNDB[s].NDBForAlpha, geotypesurface, ParamListNDB[s].HeightDistance, ParamListNDB[s].NavaidName, ParamListNDB[s].TypeofNavigation);
                                        calc_count++;
                                    }
                                    else if (part.Type == null)
                                    {
                                        if (PsurfaceElevated == null) continue;
                                        if (PsurfaceElevated.Elevation == null) continue;
                                        PolygonPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiPolygon>(PsurfaceElevated.Geo);
                                        Aran.Geometries.Point ptrjpolygon = GeomOperForRadiusCalc.GetNearestPoint(PolygonPrj, ParamListNDB[s].Coordinate);
                                        CalcForDNDB.CalculateGetlistForNDB200AND1000(ParamListNDB[s].Coordinate, ptrjpolygon, LstforNDB200, LstforNDB1000, PsurfaceElevated.Elevation.Value, "Empty", ObsID, ParamListNDB[s].NDBForLargeRadius, ParamListNDB[s].NDBForSmallRadius, ParamListNDB[s].NDBForAlpha, geotypesurface, ParamListNDB[s].HeightDistance, ParamListNDB[s].NavaidName, ParamListNDB[s].TypeofNavigation);
                                        calc_count++;
                                    }
                                    break;
                                default:
                                    continue;
                            }
                        }
                    }
                }
            }
            //NDB }
            //Markers {

            CalculateForRadiusMarkers CalcForMarkers = new CalculateForRadiusMarkers();
            List<ReportForMarkers50r> LstforMarker50 = new List<ReportForMarkers50r>();
            List<ReportForMarkers200r> LstforMarker200 = new List<ReportForMarkers200r>();
            LstforMarker50.Clear();
            LstforMarker200.Clear();

            if (ParamListMarker.Count > 0)
            {
                pointelevated = null;
                PLinearElevated = null;
                LinearPrj = null;
                PolygonPrj = null;
                PsurfaceElevated = null;

                for (int s = 0; s < ParamListMarker.Count; s++)
                {

                    for (int r = 0; r < MYObstacleList.Count; r++)
                    {
                        long ObsID = MYObstacleList[r].Id;
                        foreach (VerticalStructurePart part in MYObstacleList[r].Part)
                        {

                            if (part.HorizontalProjection == null)
                                continue;
                            switch (part.HorizontalProjection.Choice)
                            {

                                case VerticalStructurePartGeometryChoice.ElevatedPoint:
                                    pointelevated = part.HorizontalProjection.Location;
                                    string geotype = pointelevated.Geo.Type.ToString();


                                    if (part.Type != null)
                                    {
                                        if (pointelevated == null) continue;
                                        if (pointelevated.Elevation == null) continue;
                                        Aran.Geometries.Point ptrjpoint = GlobalParams.SpatialRefOperation.ToPrj(pointelevated.Geo);
                                        CalcForMarkers.CalculateGetlistForMarkers50AND200(ParamListMarker[s].Coordinate, ptrjpoint, LstforMarker50, LstforMarker200, pointelevated.Elevation.Value, part.Type.Value.ToString(), ObsID, ParamListMarker[s].MarkerForLargeRadius, ParamListMarker[s].MarkerForSmallRadius, ParamListMarker[s].MarkerForAlpha, geotype, ParamListMarker[s].HeightDistance, ParamListMarker[s].NavaidName, ParamListMarker[s].TypeOfNavigation);
                                        calc_count++;
                                    }
                                    else if (part.Type == null)
                                    {
                                        if (pointelevated == null) continue;
                                        if (pointelevated.Elevation == null) continue;
                                        Aran.Geometries.Point ptrj = GlobalParams.SpatialRefOperation.ToPrj(pointelevated.Geo);
                                        CalcForMarkers.CalculateGetlistForMarkers50AND200(ParamListMarker[s].Coordinate, ptrj, LstforMarker50, LstforMarker200, pointelevated.Elevation.Value, "Empty", ObsID, ParamListMarker[s].MarkerForLargeRadius, ParamListMarker[s].MarkerForSmallRadius, ParamListMarker[s].MarkerForAlpha, geotype, ParamListMarker[s].HeightDistance, ParamListMarker[s].NavaidName, ParamListMarker[s].TypeOfNavigation);
                                        calc_count++;
                                    }
                                    break;
                                case VerticalStructurePartGeometryChoice.ElevatedCurve:
                                    PLinearElevated = part.HorizontalProjection.LinearExtent;
                                    string geotypeline = PLinearElevated.Geo.Type.ToString();
                                    if (part.Type != null)
                                    {
                                        if (PLinearElevated == null) continue;
                                        if (PLinearElevated.Elevation == null) continue;
                                        LinearPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(PLinearElevated.Geo);
                                        Aran.Geometries.Point ptrjline = GeomOperForRadiusCalc.GetNearestPoint(LinearPrj, ParamListMarker[s].Coordinate);
                                        CalcForMarkers.CalculateGetlistForMarkers50AND200(ParamListMarker[s].Coordinate, ptrjline, LstforMarker50, LstforMarker200, PLinearElevated.Elevation.Value, part.Type.Value.ToString(), ObsID, ParamListMarker[s].MarkerForLargeRadius, ParamListMarker[s].MarkerForSmallRadius, ParamListMarker[s].MarkerForAlpha, geotypeline, ParamListMarker[s].HeightDistance, ParamListMarker[s].NavaidName, ParamListMarker[s].TypeOfNavigation);
                                        calc_count++;
                                    }
                                    else if (part.Type == null)
                                    {
                                        if (PLinearElevated == null) continue;
                                        if (PLinearElevated.Elevation == null) continue;
                                        LinearPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(PLinearElevated.Geo);
                                        Aran.Geometries.Point ptrjline = GeomOperForRadiusCalc.GetNearestPoint(LinearPrj, ParamListMarker[s].Coordinate);
                                        CalcForMarkers.CalculateGetlistForMarkers50AND200(ParamListMarker[s].Coordinate, ptrjline, LstforMarker50, LstforMarker200, PLinearElevated.Elevation.Value, "Empty", ObsID, ParamListMarker[s].MarkerForLargeRadius, ParamListMarker[s].MarkerForSmallRadius, ParamListMarker[s].MarkerForAlpha, geotypeline, ParamListMarker[s].HeightDistance, ParamListMarker[s].NavaidName, ParamListMarker[s].TypeOfNavigation);
                                        calc_count++;
                                    }
                                    break;
                                case VerticalStructurePartGeometryChoice.ElevatedSurface:
                                    PsurfaceElevated = part.HorizontalProjection.SurfaceExtent;
                                    string geotypesurface = PsurfaceElevated.Geo.Type.ToString();
                                    if (part.Type != null)
                                    {

                                        if (PsurfaceElevated == null) continue;
                                        if (PsurfaceElevated.Elevation == null) continue;
                                        PolygonPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiPolygon>(PsurfaceElevated.Geo);
                                        Aran.Geometries.Point ptrjpolygon = GeomOperForRadiusCalc.GetNearestPoint(PolygonPrj, ParamListMarker[s].Coordinate);
                                        CalcForMarkers.CalculateGetlistForMarkers50AND200(ParamListMarker[s].Coordinate, ptrjpolygon, LstforMarker50, LstforMarker200, PsurfaceElevated.Elevation.Value, part.Type.Value.ToString(), ObsID, ParamListMarker[s].MarkerForLargeRadius, ParamListMarker[s].MarkerForSmallRadius, ParamListMarker[s].MarkerForAlpha, geotypesurface, ParamListMarker[s].HeightDistance, ParamListMarker[s].NavaidName, ParamListMarker[s].TypeOfNavigation);
                                        calc_count++;
                                    }
                                    else if (part.Type == null)
                                    {
                                        if (PsurfaceElevated == null) continue;
                                        if (PsurfaceElevated.Elevation == null) continue;
                                        PolygonPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiPolygon>(PsurfaceElevated.Geo);
                                        Aran.Geometries.Point ptrjpolygon = GeomOperForRadiusCalc.GetNearestPoint(PolygonPrj, ParamListMarker[s].Coordinate);
                                        CalcForMarkers.CalculateGetlistForMarkers50AND200(ParamListMarker[s].Coordinate, ptrjpolygon, LstforMarker50, LstforMarker200, PsurfaceElevated.Elevation.Value, "Empty", ObsID, ParamListMarker[s].MarkerForLargeRadius, ParamListMarker[s].MarkerForSmallRadius, ParamListMarker[s].MarkerForAlpha, geotypesurface, ParamListMarker[s].HeightDistance, ParamListMarker[s].NavaidName, ParamListMarker[s].TypeOfNavigation);
                                        calc_count++;
                                    }
                                    break;
                                default:
                                    continue;
                            }
                        }
                    }
                }
            }
            //Markers }

            //----------------------------------------------------------------------------------------
            //CVOR {
            CalculateForRadiusCVOR CalcCVorr = new CalculateForRadiusCVOR();
            List<ReportForCVOR600r> ListCVORr600R = new List<ReportForCVOR600r>();
            List<ReportForCVOR3000r> ListCVORr3000R = new List<ReportForCVOR3000r>();
            List<ReportForCVOR15000r> ListCVORr15000R = new List<ReportForCVOR15000r>();
            List<ReportCvorForWindTurbine> ListCvorWindTurbine = new List<ReportCvorForWindTurbine>();

            if (ParamListCVOR.Count > 0)
            {
                pointelevated = null;
                PLinearElevated = null;
                LinearPrj = null;
                PolygonPrj = null;
                PsurfaceElevated = null;

                for (int s = 0; s < ParamListCVOR.Count; s++)
                {
                    if (ChkBoxWindTurbine.Checked == true)
                    {
                        CalcCVorr.ClalcForGetListWindTurbine(ParamListCVOR[s].TypeOfNavigation, ParamListCVOR[s].NavaidName, ChkBoxWindTurbine, ParamListCVOR[s].WindTurbineHeight, ListCvorWindTurbine, ParamListDVOR[s].HeightDistance);
                    }

                    for (int r = 0; r < MYObstacleList.Count; r++)
                    {//Calc Process {

                        long ObsID = MYObstacleList[r].Id;
                        foreach (VerticalStructurePart part in MYObstacleList[r].Part)
                        {

                            if (part.HorizontalProjection == null)
                                continue;
                            switch (part.HorizontalProjection.Choice)
                            {

                                case VerticalStructurePartGeometryChoice.ElevatedPoint:
                                    pointelevated = part.HorizontalProjection.Location;
                                    string geotype = pointelevated.Geo.Type.ToString();

                                    if (part.Type != null)
                                    {
                                        if (pointelevated == null) continue;
                                        if (pointelevated.Elevation == null) continue;
                                        Aran.Geometries.Point ptrjpoint = GlobalParams.SpatialRefOperation.ToPrj(pointelevated.Geo);
                                        CalcCVorr.CalculateGetlistForCVORR600AND3000AND15000(ParamListCVOR[s].Coordinate, ptrjpoint, ListCVORr600R, ListCVORr3000R, ListCVORr15000R, pointelevated.Elevation.Value, part.Type.Value.ToString(), ObsID, ParamListCVOR[s].CVORForLargeRadius, ParamListCVOR[s].CVORForMiddleRadius, ParamListCVOR[s].CVORForSmallRadius, ParamListCVOR[s].CVORForAlpha, geotype, ParamListCVOR[s].HeightDistance, ParamListCVOR[s].NavaidName, ParamListCVOR[s].TypeOfNavigation);
                                        calc_count++;
                                    }
                                    else if (part.Type == null)
                                    {
                                        if (pointelevated == null) continue;
                                        if (pointelevated.Elevation == null) continue;
                                        Aran.Geometries.Point ptrj = GlobalParams.SpatialRefOperation.ToPrj(pointelevated.Geo);
                                        CalcCVorr.CalculateGetlistForCVORR600AND3000AND15000(ParamListCVOR[s].Coordinate, ptrj, ListCVORr600R, ListCVORr3000R, ListCVORr15000R, pointelevated.Elevation.Value, "Empty", ObsID, ParamListCVOR[s].CVORForLargeRadius, ParamListCVOR[s].CVORForMiddleRadius, ParamListCVOR[s].CVORForSmallRadius, ParamListCVOR[s].CVORForAlpha, geotype, ParamListCVOR[s].HeightDistance, ParamListCVOR[s].NavaidName, ParamListCVOR[s].TypeOfNavigation);
                                        calc_count++;
                                    }
                                    break;
                                case VerticalStructurePartGeometryChoice.ElevatedCurve:
                                    PLinearElevated = part.HorizontalProjection.LinearExtent;
                                    string geotypeline = PLinearElevated.Geo.Type.ToString();
                                    if (part.Type != null)
                                    {
                                        if (PLinearElevated == null) continue;
                                        if (PLinearElevated.Elevation == null) continue;
                                        LinearPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(PLinearElevated.Geo);
                                        Aran.Geometries.Point ptrjline = GeomOperForRadiusCalc.GetNearestPoint(LinearPrj, ParamListDVOR[s].Coordinate);
                                        CalcCVorr.CalculateGetlistForCVORR600AND3000AND15000(ParamListCVOR[s].Coordinate, ptrjline, ListCVORr600R, ListCVORr3000R, ListCVORr15000R, PLinearElevated.Elevation.Value, part.Type.Value.ToString(), ObsID, ParamListCVOR[s].CVORForLargeRadius, ParamListCVOR[s].CVORForMiddleRadius, ParamListCVOR[s].CVORForSmallRadius, ParamListCVOR[s].CVORForAlpha, geotypeline, ParamListCVOR[s].HeightDistance, ParamListCVOR[s].NavaidName, ParamListCVOR[s].TypeOfNavigation);
                                        calc_count++;
                                    }
                                    else if (part.Type == null)
                                    {
                                        if (PLinearElevated == null) continue;
                                        if (PLinearElevated.Elevation == null) continue;
                                        LinearPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(PLinearElevated.Geo);
                                        Aran.Geometries.Point ptrjline = GeomOperForRadiusCalc.GetNearestPoint(LinearPrj, ParamListDVOR[s].Coordinate);
                                        CalcCVorr.CalculateGetlistForCVORR600AND3000AND15000(ParamListCVOR[s].Coordinate, ptrjline, ListCVORr600R, ListCVORr3000R, ListCVORr15000R, PLinearElevated.Elevation.Value, "Empty", ObsID, ParamListCVOR[s].CVORForLargeRadius, ParamListCVOR[s].CVORForMiddleRadius, ParamListCVOR[s].CVORForSmallRadius, ParamListCVOR[s].CVORForAlpha, geotypeline, ParamListCVOR[s].HeightDistance, ParamListCVOR[s].NavaidName, ParamListCVOR[s].TypeOfNavigation);
                                        calc_count++;
                                    }
                                    break;
                                case VerticalStructurePartGeometryChoice.ElevatedSurface:
                                    PsurfaceElevated = part.HorizontalProjection.SurfaceExtent;
                                    string geotypesurface = PsurfaceElevated.Geo.Type.ToString();
                                    if (part.Type != null)
                                    {
                                        if (PsurfaceElevated == null) continue;
                                        if (PsurfaceElevated.Elevation == null) continue;
                                        PolygonPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiPolygon>(PsurfaceElevated.Geo);
                                        Aran.Geometries.Point ptrjpolygon = GeomOperForRadiusCalc.GetNearestPoint(PolygonPrj, ParamListDVOR[s].Coordinate);
                                        CalcCVorr.CalculateGetlistForCVORR600AND3000AND15000(ParamListCVOR[s].Coordinate, ptrjpolygon, ListCVORr600R, ListCVORr3000R, ListCVORr15000R, PsurfaceElevated.Elevation.Value, part.Type.Value.ToString(), ObsID, ParamListCVOR[s].CVORForLargeRadius, ParamListCVOR[s].CVORForMiddleRadius, ParamListCVOR[s].CVORForSmallRadius, ParamListCVOR[s].CVORForAlpha, geotypesurface, ParamListCVOR[s].HeightDistance, ParamListCVOR[s].NavaidName, ParamListCVOR[s].TypeOfNavigation);
                                        calc_count++;
                                    }
                                    else if (part.Type == null)
                                    {
                                        if (PsurfaceElevated == null) continue;
                                        if (PsurfaceElevated.Elevation == null) continue;
                                        PolygonPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiPolygon>(PsurfaceElevated.Geo);
                                        Aran.Geometries.Point ptrjpolygon = GeomOperForRadiusCalc.GetNearestPoint(PolygonPrj, ParamListDVOR[s].Coordinate);
                                        CalcCVorr.CalculateGetlistForCVORR600AND3000AND15000(ParamListCVOR[s].Coordinate, ptrjpolygon, ListCVORr600R, ListCVORr3000R, ListCVORr15000R, PsurfaceElevated.Elevation.Value, "Empty", ObsID, ParamListCVOR[s].CVORForLargeRadius, ParamListCVOR[s].CVORForMiddleRadius, ParamListCVOR[s].CVORForSmallRadius, ParamListCVOR[s].CVORForAlpha, geotypesurface, ParamListCVOR[s].HeightDistance, ParamListCVOR[s].NavaidName, ParamListCVOR[s].TypeOfNavigation);
                                        calc_count++;
                                    }
                                    break;
                                default:
                                    continue;
                            }
                        }//Calc Process }
                    }
                }


            }//CVOR }
             //DVOR {
            CalculateForRadiusDVOR CalcDVor = new CalculateForRadiusDVOR();
            List<ReportForDVOR600R> ListDVOR600R = new List<ReportForDVOR600R>();
            List<ReportForDVOR3000R> ListDVOR3000R = new List<ReportForDVOR3000R>();
            List<ReportForDVOR10000R> ListDVOR10000R = new List<ReportForDVOR10000R>();
            List<ReportDvorForWindTurbine> ListDvorWindTurbine = new List<ReportDvorForWindTurbine>();

            if (ParamListDVOR.Count > 0)
            {
                pointelevated = null;
                PLinearElevated = null;
                LinearPrj = null;
                PolygonPrj = null;
                PsurfaceElevated = null;

                for (int s = 0; s < ParamListDVOR.Count; s++)
                {
                    if (ChkBoxWindTurbine.Checked == true)
                    {
                        CalcDVor.ClalcForGetListWindTurbine(ParamListDVOR[s].TypeOfNavigation, ParamListDVOR[s].NavaidName, ChkBoxWindTurbine, ParamListDVOR[s].WindTurbineHeight, ListDvorWindTurbine, ParamListDVOR[s].HeightDistance);
                    }

                    for (int r = 0; r < MYObstacleList.Count; r++)
                    {//Calc Process {

                        long ObsID = MYObstacleList[r].Id;
                        foreach (VerticalStructurePart part in MYObstacleList[r].Part)
                        {

                            if (part.HorizontalProjection == null)
                                continue;
                            switch (part.HorizontalProjection.Choice)
                            {

                                case VerticalStructurePartGeometryChoice.ElevatedPoint:
                                    pointelevated = part.HorizontalProjection.Location;
                                    string geotype = pointelevated.Geo.Type.ToString();

                                    if (part.Type != null)
                                    {
                                        if (pointelevated == null) continue;
                                        if (pointelevated.Elevation == null) continue;
                                        Aran.Geometries.Point ptrjpoint = GlobalParams.SpatialRefOperation.ToPrj(pointelevated.Geo);
                                        CalcDVor.CalculateGetlistForDVOR600AND3000AND10000(ParamListDVOR[s].Coordinate, ptrjpoint, ListDVOR600R, ListDVOR3000R, ListDVOR10000R, pointelevated.Elevation.Value, part.Type.Value.ToString(), ObsID, ParamListDVOR[s].DVORForLargeRadius, ParamListDVOR[s].DVORForMiddleRadius, ParamListDVOR[s].DVORForSmallRadius, ParamListDVOR[s].DVORForAlpha, geotype, ParamListDVOR[s].HeightDistance, ParamListDVOR[s].NavaidName, ParamListDVOR[s].TypeOfNavigation);
                                        calc_count++;
                                    }
                                    else if (part.Type == null)
                                    {
                                        if (pointelevated == null) continue;
                                        if (pointelevated.Elevation == null) continue;
                                        Aran.Geometries.Point ptrj = GlobalParams.SpatialRefOperation.ToPrj(pointelevated.Geo);
                                        CalcDVor.CalculateGetlistForDVOR600AND3000AND10000(ParamListDVOR[s].Coordinate, ptrj, ListDVOR600R, ListDVOR3000R, ListDVOR10000R, pointelevated.Elevation.Value, "Empty", ObsID, ParamListDVOR[s].DVORForLargeRadius, ParamListDVOR[s].DVORForMiddleRadius, ParamListDVOR[s].DVORForSmallRadius, ParamListDVOR[s].DVORForAlpha, geotype, ParamListDVOR[s].HeightDistance, ParamListDVOR[s].NavaidName, ParamListDVOR[s].TypeOfNavigation);
                                        calc_count++;
                                    }
                                    break;
                                case VerticalStructurePartGeometryChoice.ElevatedCurve:
                                    PLinearElevated = part.HorizontalProjection.LinearExtent;
                                    string geotypeline = PLinearElevated.Geo.Type.ToString();
                                    if (part.Type != null)
                                    {
                                        if (PLinearElevated == null) continue;
                                        if (PLinearElevated.Elevation == null) continue;
                                        LinearPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(PLinearElevated.Geo);
                                        Aran.Geometries.Point ptrjline = GeomOperForRadiusCalc.GetNearestPoint(LinearPrj, ParamListDVOR[s].Coordinate);
                                        CalcDVor.CalculateGetlistForDVOR600AND3000AND10000(ParamListDVOR[s].Coordinate, ptrjline, ListDVOR600R, ListDVOR3000R, ListDVOR10000R, PLinearElevated.Elevation.Value, part.Type.Value.ToString(), ObsID, ParamListDVOR[s].DVORForLargeRadius, ParamListDVOR[s].DVORForMiddleRadius, ParamListDVOR[s].DVORForSmallRadius, ParamListDVOR[s].DVORForAlpha, geotypeline, ParamListDVOR[s].HeightDistance, ParamListDVOR[s].NavaidName, ParamListDVOR[s].TypeOfNavigation);
                                        calc_count++;
                                    }
                                    else if (part.Type == null)
                                    {
                                        if (PLinearElevated == null) continue;
                                        if (PLinearElevated.Elevation == null) continue;
                                        LinearPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(PLinearElevated.Geo);
                                        Aran.Geometries.Point ptrjline = GeomOperForRadiusCalc.GetNearestPoint(LinearPrj, ParamListDVOR[s].Coordinate);
                                        CalcDVor.CalculateGetlistForDVOR600AND3000AND10000(ParamListDVOR[s].Coordinate, ptrjline, ListDVOR600R, ListDVOR3000R, ListDVOR10000R, PLinearElevated.Elevation.Value, "Empty", ObsID, ParamListDVOR[s].DVORForLargeRadius, ParamListDVOR[s].DVORForMiddleRadius, ParamListDVOR[s].DVORForSmallRadius, ParamListDVOR[s].DVORForAlpha, geotypeline, ParamListDVOR[s].HeightDistance, ParamListDVOR[s].NavaidName, ParamListDVOR[s].TypeOfNavigation);
                                        calc_count++;
                                    }
                                    break;
                                case VerticalStructurePartGeometryChoice.ElevatedSurface:
                                    PsurfaceElevated = part.HorizontalProjection.SurfaceExtent;
                                    string geotypesurface = PsurfaceElevated.Geo.Type.ToString();
                                    if (part.Type != null)
                                    {
                                        if (PsurfaceElevated == null) continue;
                                        if (PsurfaceElevated.Elevation == null) continue;
                                        PolygonPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiPolygon>(PsurfaceElevated.Geo);
                                        Aran.Geometries.Point ptrjpolygon = GeomOperForRadiusCalc.GetNearestPoint(PolygonPrj, ParamListDVOR[s].Coordinate);
                                        CalcDVor.CalculateGetlistForDVOR600AND3000AND10000(ParamListDVOR[s].Coordinate, ptrjpolygon, ListDVOR600R, ListDVOR3000R, ListDVOR10000R, PsurfaceElevated.Elevation.Value, part.Type.Value.ToString(), ObsID, ParamListDVOR[s].DVORForLargeRadius, ParamListDVOR[s].DVORForMiddleRadius, ParamListDVOR[s].DVORForSmallRadius, ParamListDVOR[s].DVORForAlpha, geotypesurface, ParamListDVOR[s].HeightDistance, ParamListDVOR[s].NavaidName, ParamListDVOR[s].TypeOfNavigation);
                                        calc_count++;
                                    }
                                    else if (part.Type == null)
                                    {
                                        if (PsurfaceElevated == null) continue;
                                        if (PsurfaceElevated.Elevation == null) continue;
                                        PolygonPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiPolygon>(PsurfaceElevated.Geo);
                                        Aran.Geometries.Point ptrjpolygon = GeomOperForRadiusCalc.GetNearestPoint(PolygonPrj, ParamListDVOR[s].Coordinate);
                                        CalcDVor.CalculateGetlistForDVOR600AND3000AND10000(ParamListDVOR[s].Coordinate, ptrjpolygon, ListDVOR600R, ListDVOR3000R, ListDVOR10000R, PsurfaceElevated.Elevation.Value, "Empty", ObsID, ParamListDVOR[s].DVORForLargeRadius, ParamListDVOR[s].DVORForMiddleRadius, ParamListDVOR[s].DVORForSmallRadius, ParamListDVOR[s].DVORForAlpha, geotypesurface, ParamListDVOR[s].HeightDistance, ParamListDVOR[s].NavaidName, ParamListDVOR[s].TypeOfNavigation);
                                        calc_count++;
                                    }
                                    break;
                                default:
                                    continue;
                            }
                        }//Calc Process }
                    }
                }



            }//DVOR }

            //DVOR {
            ObstacleReport.ListforDvor600 = ListDVOR600R;
            ObstacleReport.ListforDvor3000 = ListDVOR3000R;
            ObstacleReport.ListforDvor10000 = ListDVOR10000R;
            ObstacleReport.listforDvorWindTurbine = ListDvorWindTurbine;
            //DVOR }
            //CVOR {
            ObstacleReport.ListforCvor600 = ListCVORr600R;
            ObstacleReport.ListforCvor3000 = ListCVORr3000R;
            ObstacleReport.ListforCvor15000 = ListCVORr15000R;
            ObstacleReport.listforCvorWindTurbine = ListCvorWindTurbine;
            //CVOR }
            //NDB {
            ObstacleReport.ListForNDB200r = LstforNDB200;
            ObstacleReport.ListForNDB1000r = LstforNDB1000;
            //NDB }
            //Markers {
            ObstacleReport.ListForMarkers50r = LstforMarker50;
            ObstacleReport.ListForMarker200r = LstforMarker200;
            //Markers }
            //DMEN {
            ObstacleReport.ListForDMEN3000r = LstforDMEN3000;
            ObstacleReport.ListForDMEN300r = LstforDMEN300;
            //DMEN }

            //obstclreport.Show(GlobalParams.AranEnvironment.Win32Window);
        }
    }
}
