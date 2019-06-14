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
    public class Calculate_MethodFor_2D_Graphic
    {
        public List<Lists_FOR_2DGraphics> CalcObstacleNearDistanceFor2DGraphic(List<Obstacle_ParamListPolygons> ListsGP_LOC_DME)
        {
            string message_for_PolygonSegment = "0";
            string message_for_First_CornerPolygon = "0";
            string message_for_Second_CornerPolygon = "0";

            CalculateForHarmonisedGuidance calculate = new CalculateForHarmonisedGuidance();
            List<Lists_FOR_2DGraphics> List_2DGraphics = new List<Lists_FOR_2DGraphics>();

            ElevatedPoint pointelevated;
            ElevatedCurve PLinearElevated;
            MultiLineString LinearPrj;
            MultiPolygon PolygonPrj;
            ElevatedSurface PsurfaceElevated;

            if (ListsGP_LOC_DME.Count > 0)
            {
                for (int z = 0; z < ListsGP_LOC_DME.Count; z++)
                {
                    List<VerticalStructure> ObsTaclelListFirstCornerPolygon = GetObstaclesfromPolygon(ListsGP_LOC_DME[z].First_Corner_Polygon); //First Corner Polygon
                    List<VerticalStructure> ObsTaclelListSecondCornerPolygon = GetObstaclesfromPolygon(ListsGP_LOC_DME[z].Second_Corner_Polygon); //Second Corner Polygon

                    //CalculateFor_OnlyPolygonSegmentObtacles {
                    List<VerticalStructure> ObsTaclelListForPolySegment = GetObstaclesfromPolygon(ListsGP_LOC_DME[z].Segment_Polygon); // Segment Polygon

                    //Navaid Check TreeView


                    if (ObsTaclelListForPolySegment.Count > 0)
                    {
                        //message_for_PolygonSegment = Convert.ToString(ObsTaclelList.Count - 1);

                        for (int i = 0; i < ObsTaclelListForPolySegment.Count; i++)
                        {
                            long ObsID = ObsTaclelListForPolySegment[i].Id;

                            foreach (VerticalStructurePart part in ObsTaclelListForPolySegment[i].Part)
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
                                            //GlobalParams.AranEnvironment.Graphics.DrawPoint(ptrjpoint, 255 * 255, true, false);
                                            List_2DGraphics = calculate.CalculateFOR_GP_LOC_DME_PolygonSegment_FORPOINT(ListsGP_LOC_DME[z].Navaid_Height, ListsGP_LOC_DME[z].Child_NodeTxt, ListsGP_LOC_DME[z].Parent_NodeTxt, ptrjpoint, pointelevated.Elevation.Value, part.Type.Value.ToString(), ObsID, geotype, ListsGP_LOC_DME[z].SegmentPoly_Line, ListsGP_LOC_DME[z].A, ListsGP_LOC_DME[z].Segment_Polygon_h, ListsGP_LOC_DME[z].Radius);
                                        }
                                        else if (part.Type == null)
                                        {
                                            if (pointelevated == null) continue;
                                            if (pointelevated.Elevation == null) continue;
                                            Aran.Geometries.Point ptrj = GlobalParams.SpatialRefOperation.ToPrj(pointelevated.Geo);
                                            //GlobalParams.AranEnvironment.Graphics.DrawPoint(ptrj, 255 * 255, true, false);
                                            List_2DGraphics = calculate.CalculateFOR_GP_LOC_DME_PolygonSegment_FORPOINT(ListsGP_LOC_DME[z].Navaid_Height, ListsGP_LOC_DME[z].Child_NodeTxt, ListsGP_LOC_DME[z].Parent_NodeTxt, ptrj, pointelevated.Elevation.Value, "Empty", ObsID, geotype, ListsGP_LOC_DME[z].SegmentPoly_Line, ListsGP_LOC_DME[z].A, ListsGP_LOC_DME[z].Segment_Polygon_h, ListsGP_LOC_DME[z].Radius);
                                        }
                                        break;
                                    case VerticalStructurePartGeometryChoice.ElevatedCurve:
                                        PLinearElevated = part.HorizontalProjection.LinearExtent;
                                        string geotypeline = PLinearElevated.Geo.Type.ToString();
                                        if (part.Type != null)
                                        {
                                            if (PLinearElevated == null) continue;
                                            if (PLinearElevated.Elevation == null) continue;
                                            LinearPrj = PLinearElevated.Geo;
                                            LinearPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(LinearPrj);
                                            List_2DGraphics = calculate.CalculateFOR_GP_LOC_DME_PolygonSegment_FORLINE(ListsGP_LOC_DME[z].Navaid_Height, ListsGP_LOC_DME[z].Child_NodeTxt, ListsGP_LOC_DME[z].Parent_NodeTxt, LinearPrj, PLinearElevated.Elevation.Value, part.Type.Value.ToString(), ObsID, geotypeline, ListsGP_LOC_DME[z].SegmentPoly_Line, ListsGP_LOC_DME[z].A, ListsGP_LOC_DME[z].Segment_Polygon_h, ListsGP_LOC_DME[z].Radius);
                                        }
                                        else if (part.Type == null)
                                        {
                                            if (PLinearElevated == null) continue;
                                            if (PLinearElevated.Elevation == null) continue;
                                            LinearPrj = PLinearElevated.Geo;
                                            LinearPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(LinearPrj);
                                            //GlobalParams.AranEnvironment.Graphics.DrawMultiLineString(LinearPrj, 3, 156 * 132 * 150, true, true);
                                            List_2DGraphics = calculate.CalculateFOR_GP_LOC_DME_PolygonSegment_FORLINE(ListsGP_LOC_DME[z].Navaid_Height, ListsGP_LOC_DME[z].Child_NodeTxt, ListsGP_LOC_DME[z].Parent_NodeTxt, LinearPrj, PLinearElevated.Elevation.Value, "Empty", ObsID, geotypeline, ListsGP_LOC_DME[z].SegmentPoly_Line, ListsGP_LOC_DME[z].A, ListsGP_LOC_DME[z].Segment_Polygon_h, ListsGP_LOC_DME[z].Radius);
                                        }

                                        break;
                                    case VerticalStructurePartGeometryChoice.ElevatedSurface:
                                        PsurfaceElevated = part.HorizontalProjection.SurfaceExtent;
                                        string geotypesurface = PsurfaceElevated.Geo.Type.ToString();
                                        if (part.Type != null)
                                        {
                                            if (PsurfaceElevated == null) continue;
                                            if (PsurfaceElevated.Elevation == null) continue;
                                            PolygonPrj = PsurfaceElevated.Geo;
                                            PolygonPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiPolygon>(PolygonPrj);
                                            List_2DGraphics = calculate.CalculateFOR_GP_LOC_DME_PolygonSegment_FORPOLYGON(ListsGP_LOC_DME[z].Navaid_Height, ListsGP_LOC_DME[z].Child_NodeTxt, ListsGP_LOC_DME[z].Parent_NodeTxt, PolygonPrj, PsurfaceElevated.Elevation.Value, part.Type.Value.ToString(), ObsID, geotypesurface, ListsGP_LOC_DME[z].SegmentPoly_Line, ListsGP_LOC_DME[z].A, ListsGP_LOC_DME[z].Segment_Polygon_h, ListsGP_LOC_DME[z].Radius);
                                        }
                                        else if (part.Type == null)
                                        {
                                            if (PsurfaceElevated == null) continue;
                                            if (PsurfaceElevated.Elevation == null) continue;
                                            PolygonPrj = PsurfaceElevated.Geo;
                                            PolygonPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiPolygon>(PsurfaceElevated.Geo);
                                            List_2DGraphics = calculate.CalculateFOR_GP_LOC_DME_PolygonSegment_FORPOLYGON(ListsGP_LOC_DME[z].Navaid_Height, ListsGP_LOC_DME[z].Child_NodeTxt, ListsGP_LOC_DME[z].Parent_NodeTxt, PolygonPrj, PsurfaceElevated.Elevation.Value, "Empty", ObsID, geotypesurface, ListsGP_LOC_DME[z].SegmentPoly_Line, ListsGP_LOC_DME[z].A, ListsGP_LOC_DME[z].Segment_Polygon_h, ListsGP_LOC_DME[z].Radius);
                                        }
                                        break;
                                    default:
                                        continue;

                                }
                            }
                        }
                    }

                    //CalculateFor_OnlyPolygonSegmentObtacles }



                    //CalculateFor_FirstCornerPolygon {

                    pointelevated = null;
                    PLinearElevated = null;
                    LinearPrj = null;
                    PolygonPrj = null;
                    PsurfaceElevated = null;



                    if (ObsTaclelListFirstCornerPolygon.Count > 0)
                    {

                        //message_for_First_CornerPolygon = Convert.ToString(ObsTaclelListFirstCornerPolygon.Count - 1);

                        for (int i = 0; i < ObsTaclelListFirstCornerPolygon.Count; i++)
                        {
                            long ObsID = ObsTaclelListFirstCornerPolygon[i].Id;

                            foreach (VerticalStructurePart part in ObsTaclelListFirstCornerPolygon[i].Part)
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
                                            //GlobalParams.AranEnvironment.Graphics.DrawPoint(ptrjpoint, 255 * 255, true, false);
                                            List_2DGraphics = calculate.CalculateFOR_GP_LOC_DME_PolygonFirstCorner_FORPOINT(ListsGP_LOC_DME[z].Navaid_Height, ListsGP_LOC_DME[z].Child_NodeTxt, ListsGP_LOC_DME[z].Parent_NodeTxt, ptrjpoint, pointelevated.Elevation.Value, part.Type.Value.ToString(), ObsID, geotype, ListsGP_LOC_DME[z].FirstCornerPoly_Line, ListsGP_LOC_DME[z].A, ListsGP_LOC_DME[z].Corners_Polygon_H, ListsGP_LOC_DME[z].Delta_L);
                                        }
                                        else if (part.Type == null)
                                        {
                                            if (pointelevated == null) continue;
                                            if (pointelevated.Elevation == null) continue;
                                            Aran.Geometries.Point ptrj = GlobalParams.SpatialRefOperation.ToPrj(pointelevated.Geo);
                                            //GlobalParams.AranEnvironment.Graphics.DrawPoint(ptrj, 255 * 255, true, false);
                                            List_2DGraphics = calculate.CalculateFOR_GP_LOC_DME_PolygonFirstCorner_FORPOINT(ListsGP_LOC_DME[z].Navaid_Height, ListsGP_LOC_DME[z].Child_NodeTxt, ListsGP_LOC_DME[z].Parent_NodeTxt, ptrj, pointelevated.Elevation.Value, "Empty", ObsID, geotype, ListsGP_LOC_DME[z].FirstCornerPoly_Line, ListsGP_LOC_DME[z].A, ListsGP_LOC_DME[z].Corners_Polygon_H, ListsGP_LOC_DME[z].Delta_L);
                                        }
                                        break;
                                    case VerticalStructurePartGeometryChoice.ElevatedCurve:
                                        PLinearElevated = part.HorizontalProjection.LinearExtent;
                                        string geotypeline = PLinearElevated.Geo.Type.ToString();
                                        if (part.Type != null)
                                        {
                                            if (PLinearElevated == null) continue;
                                            if (PLinearElevated.Elevation == null) continue;
                                            LinearPrj = PLinearElevated.Geo;
                                            LinearPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(PLinearElevated.Geo);
                                            List_2DGraphics = calculate.CalculateFOR_GP_LOC_DME_PolygonFirstCorner_FORLINE(ListsGP_LOC_DME[z].Navaid_Height, ListsGP_LOC_DME[z].Child_NodeTxt, ListsGP_LOC_DME[z].Parent_NodeTxt, LinearPrj, PLinearElevated.Elevation.Value, part.Type.Value.ToString(), ObsID, geotypeline, ListsGP_LOC_DME[z].FirstCornerPoly_Line, ListsGP_LOC_DME[z].A, ListsGP_LOC_DME[z].Corners_Polygon_H, ListsGP_LOC_DME[z].Delta_L);
                                        }
                                        else if (part.Type == null)
                                        {
                                            if (PLinearElevated == null) continue;
                                            if (PLinearElevated.Elevation == null) continue;
                                            LinearPrj = PLinearElevated.Geo;
                                            LinearPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(PLinearElevated.Geo);
                                            List_2DGraphics = calculate.CalculateFOR_GP_LOC_DME_PolygonFirstCorner_FORLINE(ListsGP_LOC_DME[z].Navaid_Height, ListsGP_LOC_DME[z].Child_NodeTxt, ListsGP_LOC_DME[z].Parent_NodeTxt, LinearPrj, PLinearElevated.Elevation.Value, "Empty", ObsID, geotypeline, ListsGP_LOC_DME[z].FirstCornerPoly_Line, ListsGP_LOC_DME[z].A, ListsGP_LOC_DME[z].Corners_Polygon_H, ListsGP_LOC_DME[z].Delta_L);
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
                                            List_2DGraphics = calculate.CalculateFOR_GP_LOC_DME_PolygonFirstCorner_FORPLYGON(ListsGP_LOC_DME[z].Navaid_Height, ListsGP_LOC_DME[z].Child_NodeTxt, ListsGP_LOC_DME[z].Parent_NodeTxt, PolygonPrj, PsurfaceElevated.Elevation.Value, part.Type.Value.ToString(), ObsID, geotypesurface, ListsGP_LOC_DME[z].FirstCornerPoly_Line, ListsGP_LOC_DME[z].A, ListsGP_LOC_DME[z].Corners_Polygon_H, ListsGP_LOC_DME[z].Delta_L);
                                        }
                                        else if (part.Type == null)
                                        {
                                            if (PsurfaceElevated == null) continue;
                                            if (PsurfaceElevated.Elevation == null) continue;
                                            PolygonPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiPolygon>(PsurfaceElevated.Geo);
                                            List_2DGraphics = calculate.CalculateFOR_GP_LOC_DME_PolygonFirstCorner_FORPLYGON(ListsGP_LOC_DME[z].Navaid_Height, ListsGP_LOC_DME[z].Child_NodeTxt, ListsGP_LOC_DME[z].Parent_NodeTxt, PolygonPrj, PsurfaceElevated.Elevation.Value, "Empty", ObsID, geotypesurface, ListsGP_LOC_DME[z].FirstCornerPoly_Line, ListsGP_LOC_DME[z].A, ListsGP_LOC_DME[z].Corners_Polygon_H, ListsGP_LOC_DME[z].Delta_L);
                                        }
                                        break;
                                    default:
                                        continue;

                                }
                            }
                        }
                    }

                    //CalculateFor_FirstCornerPolygon }

                    //CalculateFor_SecondCornerPolygon {
                    pointelevated = null;
                    PLinearElevated = null;
                    LinearPrj = null;
                    PolygonPrj = null;
                    PsurfaceElevated = null;


                    if (ObsTaclelListSecondCornerPolygon.Count > 0)
                    {

                        //message_for_Second_CornerPolygon = Convert.ToString(ObsTaclelListSecondCornerPolygon.Count - 1);

                        for (int i = 0; i < ObsTaclelListSecondCornerPolygon.Count; i++)
                        {
                            long ObsID = ObsTaclelListSecondCornerPolygon[i].Id;

                            foreach (VerticalStructurePart part in ObsTaclelListSecondCornerPolygon[i].Part)
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
                                            //GlobalParams.AranEnvironment.Graphics.DrawPoint(ptrjpoint, 255 * 255, true, false);
                                            List_2DGraphics = calculate.CalculateFOR_GP_LOC_DME_PolygonSecondCorner_FORPOINT(ListsGP_LOC_DME[z].Navaid_Height, ListsGP_LOC_DME[z].Child_NodeTxt, ListsGP_LOC_DME[z].Parent_NodeTxt, ptrjpoint, pointelevated.Elevation.Value, part.Type.Value.ToString(), ObsID, geotype, ListsGP_LOC_DME[z].SecondCornerPoly_Line, ListsGP_LOC_DME[z].A, ListsGP_LOC_DME[z].Corners_Polygon_H, ListsGP_LOC_DME[z].Delta_L);
                                        }
                                        else if (part.Type == null)
                                        {
                                            if (pointelevated == null) continue;
                                            if (pointelevated.Elevation == null) continue;
                                            Aran.Geometries.Point ptrj = GlobalParams.SpatialRefOperation.ToPrj(pointelevated.Geo);
                                            //GlobalParams.AranEnvironment.Graphics.DrawPoint(ptrj, 255 * 255, true, false);
                                            List_2DGraphics = calculate.CalculateFOR_GP_LOC_DME_PolygonSecondCorner_FORPOINT(ListsGP_LOC_DME[z].Navaid_Height, ListsGP_LOC_DME[z].Child_NodeTxt, ListsGP_LOC_DME[z].Parent_NodeTxt, ptrj, pointelevated.Elevation.Value, "Empty", ObsID, geotype, ListsGP_LOC_DME[z].SecondCornerPoly_Line, ListsGP_LOC_DME[z].A, ListsGP_LOC_DME[z].Corners_Polygon_H, ListsGP_LOC_DME[z].Delta_L);
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
                                            List_2DGraphics = calculate.CalculateFOR_GP_LOC_DME_PolygonSecondCorner_FORLINE(ListsGP_LOC_DME[z].Navaid_Height, ListsGP_LOC_DME[z].Child_NodeTxt, ListsGP_LOC_DME[z].Parent_NodeTxt, LinearPrj, PLinearElevated.Elevation.Value, part.Type.Value.ToString(), ObsID, geotypeline, ListsGP_LOC_DME[z].SecondCornerPoly_Line, ListsGP_LOC_DME[z].A, ListsGP_LOC_DME[z].Corners_Polygon_H, ListsGP_LOC_DME[z].Delta_L);
                                        }
                                        else if (part.Type == null)
                                        {
                                            if (PLinearElevated == null) continue;
                                            if (PLinearElevated.Elevation == null) continue;
                                            LinearPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(PLinearElevated.Geo);
                                            List_2DGraphics = calculate.CalculateFOR_GP_LOC_DME_PolygonSecondCorner_FORLINE(ListsGP_LOC_DME[z].Navaid_Height, ListsGP_LOC_DME[z].Child_NodeTxt, ListsGP_LOC_DME[z].Parent_NodeTxt, LinearPrj, PLinearElevated.Elevation.Value, "Empty", ObsID, geotypeline, ListsGP_LOC_DME[z].SecondCornerPoly_Line, ListsGP_LOC_DME[z].A, ListsGP_LOC_DME[z].Corners_Polygon_H, ListsGP_LOC_DME[z].Delta_L);

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
                                            List_2DGraphics = calculate.CalculateFOR_GP_LOC_DME_PolygonSecondCorner_FORPOLYGON(ListsGP_LOC_DME[z].Navaid_Height, ListsGP_LOC_DME[z].Child_NodeTxt, ListsGP_LOC_DME[z].Parent_NodeTxt, PolygonPrj, PsurfaceElevated.Elevation.Value, part.Type.Value.ToString(), ObsID, geotypesurface, ListsGP_LOC_DME[z].SecondCornerPoly_Line, ListsGP_LOC_DME[z].A, ListsGP_LOC_DME[z].Corners_Polygon_H, ListsGP_LOC_DME[z].Delta_L);
                                        }
                                        else if (part.Type == null)
                                        {
                                            if (PsurfaceElevated == null) continue;
                                            if (PsurfaceElevated.Elevation == null) continue;
                                            PolygonPrj = GlobalParams.SpatialRefOperation.ToPrj<MultiPolygon>(PsurfaceElevated.Geo);
                                            List_2DGraphics = calculate.CalculateFOR_GP_LOC_DME_PolygonSecondCorner_FORPOLYGON(ListsGP_LOC_DME[z].Navaid_Height, ListsGP_LOC_DME[z].Child_NodeTxt, ListsGP_LOC_DME[z].Parent_NodeTxt, PolygonPrj, PsurfaceElevated.Elevation.Value, "Empty", ObsID, geotypesurface, ListsGP_LOC_DME[z].SecondCornerPoly_Line, ListsGP_LOC_DME[z].A, ListsGP_LOC_DME[z].Corners_Polygon_H, ListsGP_LOC_DME[z].Delta_L);
                                        }
                                        break;
                                    default:
                                        continue;

                                }
                            }
                        }
                    }
                    //CalculateFor_SecondCornerPolygon }



                    //Navaid TreeView Check

                }
            }



            //ObstacleReport ObstclReport = new ObstacleReport(arantoolitem);
            //ObstclReport.List_2DGrpahics_Calculation = List_2DGraphics;
            //ObstacleReport.AddcheckedChildParent = AddcheckParentChildList;
            //ObstclReport.Show(GlobalParams.AranEnvironment.Win32Window);

            return List_2DGraphics;
        }
        public List<VerticalStructure> GetObstaclesfromPolygon(Aran.Geometries.Polygon polygon)
        {
            Aran.Geometries.MultiPolygon multipolygon = new MultiPolygon();
            multipolygon.Add(polygon);
            multipolygon = GlobalParams.SpatialRefOperation.ToGeo(multipolygon);
            List<VerticalStructure> VerticalStructureList = DBModule.OmegaQPI.GetVerticalStructureList(multipolygon);
            return VerticalStructureList;
        }
    }
}
