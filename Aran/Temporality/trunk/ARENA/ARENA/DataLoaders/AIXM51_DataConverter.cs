using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PDM;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using Aran.Temporality.Common.ArcGis;
using Aran.Temporality.Common.Aim.Extension.Property;
using ESRI.ArcGIS.Geometry;
using Aran.Temporality.Common.Aim.MetaData;
using Accent.MapElements;
using Aran.Aim.PropertyEnum;
using EsriWorkEnvironment;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Enums;
using TimeSlice = Aran.Temporality.Common.MetaData.TimeSlice;
using Aran.Temporality.Common.Enum;

namespace ARENA.DataLoaders
{
    public class AIXM51_DataConverter : IARENA_DATA_Converter
    {
        private Environment.Environment _CurEnvironment;

        public Environment.Environment CurEnvironment
        {
            get { return _CurEnvironment; }
            set { _CurEnvironment = value; }
        }

        public AIXM51_DataConverter()
        {
        }

        public AIXM51_DataConverter(Environment.Environment env)
        {
            this.CurEnvironment = env;
        }

        public bool Convert_Data(IFeatureClass _FeatureClass)
        {
           
           
            var openFileDialog1 = new OpenFileDialog { Filter = @"Aran DB AIXM 5.1 snapshot (*.xml)|*.xml" };
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return false;

            AlertForm alrtForm = new AlertForm();
            alrtForm.FormBorderStyle = FormBorderStyle.None;
            alrtForm.Opacity = 0.5;
            alrtForm.BackgroundImage = ARENA.Properties.Resources.ArenaSplash;
            //alrtForm.TopMost = true;
            
            if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();

            this.CurEnvironment.Data.ReadAIXM51Data(openFileDialog1.FileName);

            //// Workspace Geo
            var workspaceEdit = (IWorkspaceEdit)_FeatureClass.FeatureDataset.Workspace;
            workspaceEdit.StartEditing(false);
            workspaceEdit.StartEditOperation();

            #region

            CurEnvironment.FillAirtrackTableDic(workspaceEdit);
            ITable noteTable = EsriUtils.getTableByname((IFeatureWorkspace)workspaceEdit, "Notes");

            #endregion

            if (this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features != null)
            {

                #region Airports/RWY/THR/NAVAID

                var adhpList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                where (element != null) &&
                                    (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.AirportHeliport)
                                select element).ToList();

                foreach (var aimADHP in adhpList)
                {
                    AirportHeliport pdmADHP = (AirportHeliport)AIM_PDM_Converter.AIM_Object_Convert(aimADHP.AIXM51_Feature, aimADHP.AixmGeo);
                    if (pdmADHP == null) continue;

                    #region RWY/THR/NAVAID


                    var adhp_rwy = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                    where (element != null) &&
                                        (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.Runway) &&
                                        (((Aran.Aim.Features.Runway)element.AIXM51_Feature).AssociatedAirportHeliport.Identifier == aimADHP.AIXM51_Feature.Identifier)
                                    select element).ToList();

                    if ((adhp_rwy != null) && (adhp_rwy.Count > 0))
                    {
                        pdmADHP.RunwayList = new List<Runway>();

                        foreach (var featureRWY in adhp_rwy)
                        {
                            var aimRWY = featureRWY.AIXM51_Feature;
                            Runway pdmRWY = (Runway)AIM_PDM_Converter.AIM_Object_Convert(aimRWY, null);

                            if (pdmRWY != null) 
                            {
                                pdmRWY.ID_AirportHeliport = pdmADHP.ID;

                                #region THR

                                var rwy_thr = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                               where (element != null) &&
                                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RunwayDirection) && (((Aran.Aim.Features.RunwayDirection)element.AIXM51_Feature).UsedRunway != null) && 
                                                   (((Aran.Aim.Features.RunwayDirection)element.AIXM51_Feature).UsedRunway.Identifier == aimRWY.Identifier)
                                               select element).ToList();

                                if ((rwy_thr != null) && (rwy_thr.Count > 0))
                                {
                                    pdmRWY.RunwayDirectionList = new List<RunwayDirection>();

                                    foreach (var featureTHR in rwy_thr)
                                    {
                                        var aimTHR = featureTHR.AIXM51_Feature;

                                        #region Center Line Points THR Geometry

                                        var thr_clp = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                       where (element != null) &&
                                                           (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RunwayCentrelinePoint) &&
                                                            (((Aran.Aim.Features.RunwayCentrelinePoint)element.AIXM51_Feature).OnRunway != null) &&
                                                           (((Aran.Aim.Features.RunwayCentrelinePoint)element.AIXM51_Feature).OnRunway.Identifier == aimTHR.Identifier) &&
                                                           (((Aran.Aim.Features.RunwayCentrelinePoint)element.AIXM51_Feature).Role != null) &&
                                                           (((Aran.Aim.Features.RunwayCentrelinePoint)element.AIXM51_Feature).Role == Aran.Aim.Enums.CodeRunwayPointRole.THR)
                                                       select element).FirstOrDefault();

                                        if (thr_clp == null)  continue;

                                        #endregion

                                        if ((thr_clp.AixmGeo.Count > 0) && (thr_clp.AixmGeo[0].GeometryType == esriGeometryType.esriGeometryPoint))
                                        {
                                            RunwayDirection pdmTHR = (RunwayDirection)AIM_PDM_Converter.AIM_Object_Convert(aimTHR, thr_clp.AixmGeo);

                                            if (pdmTHR != null)
                                            {
                                                 
                                                pdmTHR.ClearWay = getClearWaylength(aimTHR.Identifier);
                                                pdmTHR.Stopway = getStopWaylength(aimTHR.Identifier);
                                                pdmTHR.Uom = UOM_DIST_HORZ.M;

                                                pdmTHR.ID_AirportHeliport = pdmADHP.ID;
                                                pdmTHR.ID_Runway = pdmRWY.ID;

                                                #region Navaids


                                                var thr_nvd = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                               where (element != null) &&
                                                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.Navaid) &&
                                                                    (((Aran.Aim.Features.Navaid)element.AIXM51_Feature).RunwayDirection != null) &&
                                                                    (((Aran.Aim.Features.Navaid)element.AIXM51_Feature).RunwayDirection.Count > 0) &&
                                                                   (((Aran.Aim.Features.Navaid)element.AIXM51_Feature).RunwayDirection[0].Feature.Identifier == aimTHR.Identifier)
                                                               select element).ToList();

                                                if ((thr_nvd != null) && (thr_nvd.Count > 0))
                                                {
                                                    pdmTHR.Related_NavaidSystem = new List<NavaidSystem>();

                                                    foreach (var featureNav in thr_nvd)
                                                    {
                                                        var aimnav = featureNav.AIXM51_Feature;
                                                        NavaidSystem pdmNavSys = (NavaidSystem)AIM_PDM_Converter.AIM_Object_Convert(aimnav, null);
                                                        if (pdmNavSys != null)
                                                        {
                                                            //CurEnvironment.Data.PdmObjectList.Add(pdmNavSys);

                                                            pdmNavSys.ID_AirportHeliport = pdmADHP.ID;
                                                            pdmNavSys.ID_RunwayDirection = pdmTHR.ID;
                                                            pdmNavSys.Components = new List<PDMObject>();

                                                            #region navaidEquipment

                                                            if ((((Aran.Aim.Features.Navaid)aimnav).NavaidEquipment != null) && (((Aran.Aim.Features.Navaid)aimnav).NavaidEquipment.Count > 0))
                                                            {
                                                                foreach (var item in ((Aran.Aim.Features.Navaid)aimnav).NavaidEquipment)
                                                                {
                                                                    //var pdm_navEqpnt = GetComponent(item.TheNavaidEquipment.Type.ToString(), item.TheNavaidEquipment.Identifier);
                                                                    var pdm_navEqpnt = GetComponent(item.TheNavaidEquipment.Identifier);
                                                                    if (pdm_navEqpnt != null)
                                                                    {
                                                                        ((NavaidComponent)pdm_navEqpnt).ID_NavaidSystem = pdmNavSys.ID;
                                                                        pdmNavSys.Components.Add(pdm_navEqpnt);
                                                                    }

                                                                }
                                                            }
                                                            #endregion


                                                            pdmTHR.Related_NavaidSystem.Add(pdmNavSys);

                                                        }
                                                    }
                                                }

                                                #endregion

                                                #region Center Line Points THR

                                                var _clp = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                            where (element != null) &&
                                                                (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RunwayCentrelinePoint) &&
                                                                 (((Aran.Aim.Features.RunwayCentrelinePoint)element.AIXM51_Feature).OnRunway != null) &&
                                                                (((Aran.Aim.Features.RunwayCentrelinePoint)element.AIXM51_Feature).OnRunway.Identifier == aimTHR.Identifier)
                                                            select element).ToList();

                                                if ((_clp != null) && (_clp.Count > 0))
                                                {
                                                    pdmTHR.CenterLinePoints = new List<RunwayCenterLinePoint>();

                                                    foreach (var featureCLP in _clp)
                                                    {
                                                        var aimCLP = featureCLP.AIXM51_Feature;


                                                        if ((featureCLP.AixmGeo.Count > 0) && (featureCLP.AixmGeo[0].GeometryType == esriGeometryType.esriGeometryPoint))
                                                        {
                                                            RunwayCenterLinePoint pdmCLP = (RunwayCenterLinePoint)AIM_PDM_Converter.AIM_Object_Convert(aimCLP, featureCLP.AixmGeo);
                                                            if (pdmCLP != null)
                                                            {
                                                                pdmCLP.ID_RunwayDirection = pdmTHR.ID;
                                                                pdmTHR.CenterLinePoints.Add(pdmCLP);

                                                                if ((pdmCLP.DeclDist != null) && (pdmCLP.DeclDist.Count > 0))
                                                                    pdmTHR.RdnDeclaredDistance = pdmCLP.DeclDist;
                                                            }
                                                        }
                                                    }

                                                }
                                                #endregion


                                                pdmRWY.RunwayDirectionList.Add(pdmTHR);


                                            }
                                        }


                                    }
                                }


                                #endregion

                                ((AirportHeliport)pdmADHP).RunwayList.Add(pdmRWY as Runway);
                            }
                        }

                    }


                    #endregion

                    //if (pdmADHP.StoreToDB(CurEnvironment.Data.TableDictionary))
                    {
                        CurEnvironment.Data.PdmObjectList.Add(pdmADHP);
                    }

                }

                #endregion

                #region Navaids


                var _nvd = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                            where (element != null) &&
                                (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.Navaid) &&
                                 ((((Aran.Aim.Features.Navaid)element.AIXM51_Feature).RunwayDirection == null) || (((Aran.Aim.Features.Navaid)element.AIXM51_Feature).RunwayDirection.Count == 0))
                            select element).ToList();

                if ((_nvd != null) && (_nvd.Count > 0))
                {
                    foreach (var featureNav in _nvd)
                    {
                        var aimnav = featureNav.AIXM51_Feature;
                        NavaidSystem pdmNavSys = (NavaidSystem)AIM_PDM_Converter.AIM_Object_Convert(aimnav, null);
                        if (pdmNavSys != null)
                        {
                            pdmNavSys.Components = new List<PDMObject>();

                            #region navaidEquipment

                            if ((((Aran.Aim.Features.Navaid)aimnav).NavaidEquipment != null) && (((Aran.Aim.Features.Navaid)aimnav).NavaidEquipment.Count > 0))
                            {
                                foreach (var item in ((Aran.Aim.Features.Navaid)aimnav).NavaidEquipment)
                                {
                                    //var pdm_navEqpnt = GetComponent(item.TheNavaidEquipment.Type.ToString(), item.TheNavaidEquipment.Identifier);
                                    var pdm_navEqpnt = GetComponent(item.TheNavaidEquipment.Identifier);
                                    if (pdm_navEqpnt != null)
                                    {
                                        ((NavaidComponent)pdm_navEqpnt).ID_NavaidSystem = pdmNavSys.ID;
                                        pdmNavSys.Components.Add(pdm_navEqpnt);
                                    }

                                }
                            }
                            #endregion

                            //if (pdmNavSys.StoreToDB(CurEnvironment.Data.TableDictionary))
                            {
                                CurEnvironment.Data.PdmObjectList.Add(pdmNavSys);
                            }

                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("null " + aimnav.Identifier.ToString());
                        }
                    }
                }


                #endregion

                #region Waypoint

                var wypList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.DesignatedPoint)
                               select element).ToList();

                foreach (var featureWYP in wypList)
                {
                    var aimDPN = featureWYP.AIXM51_Feature;


                    if ((featureWYP.AixmGeo.Count > 0) && (featureWYP.AixmGeo[0].GeometryType == esriGeometryType.esriGeometryPoint))
                    {
                        WayPoint pdmWYP = (WayPoint)AIM_PDM_Converter.AIM_Object_Convert(aimDPN, featureWYP.AixmGeo);

                        if (pdmWYP == null) continue;

                        //if (pdmWYP.StoreToDB(CurEnvironment.Data.TableDictionary))
                        {
                            CurEnvironment.Data.PdmObjectList.Add(pdmWYP);
                        }
                    }
                }


                #endregion

                #region Vertical structure

                var obsList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.VerticalStructure)
                               select element).ToList();

                foreach (var featureObs in obsList)
                {
                    var aimObs = featureObs.AIXM51_Feature;

                    if (featureObs.AixmGeo.Count > 0)
                    {
                        VerticalStructure pdmOBS = (VerticalStructure)AIM_PDM_Converter.AIM_Object_Convert(aimObs, featureObs.AixmGeo);
                        if (pdmOBS == null) continue;

                        foreach (var item in pdmOBS.Parts)
                        {
                            item.Vertex = HelperClass.SetObjectToBlob(item.Geo, "Vertex");
                        }

                        //if (pdmOBS.StoreToDB(CurEnvironment.Data.TableDictionary))
                        {
                            CurEnvironment.Data.PdmObjectList.Add(pdmOBS);
                        }
                    }

                }


                #endregion

                #region Airspace

                var arspsList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                 where (element != null) &&
                                     (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.Airspace)
                                 select element).ToList();

                foreach (var featureARSPS in arspsList)
                {
                    var aimARSP = featureARSPS.AIXM51_Feature;



                    Airspace pdmARSPS = (Airspace)AIM_PDM_Converter.AIM_Object_Convert(aimARSP, null);

                    if (pdmARSPS == null) continue;

                    pdmARSPS.AirspaceVolumeList = new List<AirspaceVolume>();

                    for (int i = 0; i <= featureARSPS.AixmGeo.Count - 1; i++)
                    {
                        var esri_gm = featureARSPS.AixmGeo[i];

                        if ((esri_gm != null) && (esri_gm.GeometryType == esriGeometryType.esriGeometryPolygon))
                        {
                            var aimArsp_vol = (((Aran.Aim.Features.Airspace)aimARSP).GeometryComponent[i].TheAirspaceVolume);
                            List<IGeometry> geoList = new List<IGeometry>();
                            geoList.Add(esri_gm);
                            AirspaceVolume pdmARSPS_Vol = (AirspaceVolume)AIM_PDM_Converter.AIM_Object_Convert(aimArsp_vol, geoList);

                            if (pdmARSPS_Vol == null) continue;


                            //pdmARSPS_Vol.BrdrGeometry = HelperClass.SetObjectToBlob(pdmARSPS_Vol.Geo, "Border");
                            pdmARSPS_Vol.ID_Airspace = pdmARSPS.ID;
                            //pdmARSPS_Vol.TxtLocalType = ((Aran.Aim.Features.Airspace)aimARSP).Type.HasValue ? ((Aran.Aim.Features.Airspace)aimARSP).Type.Value.ToString() : "";
                            pdmARSPS_Vol.TxtName = ((Aran.Aim.Features.Airspace)aimARSP).Designator;
                            pdmARSPS_Vol.CodeId = ((Aran.Aim.Features.Airspace)aimARSP).Designator;
                            if (!pdmARSPS.Lat.StartsWith("00")) pdmARSPS_Vol.CodeClass = pdmARSPS.Lat; // временное хранение класса Airspace
                            pdmARSPS.Lat = "";

                            pdmARSPS.AirspaceVolumeList.Add(pdmARSPS_Vol);

                            if (((Aran.Aim.Features.Airspace)aimARSP).Type.HasValue)
                            {
                                AirspaceType _uom;

                                Enum.TryParse<AirspaceType>(((Aran.Aim.Features.Airspace)aimARSP).Type.Value.ToString(), out _uom);
                                pdmARSPS_Vol.CodeType = _uom;
                            }



                        }
                    }



                    {
                        CurEnvironment.Data.PdmObjectList.Add(pdmARSPS);
                    }

                }



                #endregion

                #region Enroute/RouteSegment/RouteSegmentPoint

                var routeList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                 where (element != null) &&
                                     (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.Route)
                                 select element).ToList();

                foreach (var featureARSPS in routeList)
                {
                    var aimENRT = featureARSPS.AIXM51_Feature;

                    Enroute pdmENRT = (Enroute)AIM_PDM_Converter.AIM_Object_Convert(aimENRT, null);

                    if (pdmENRT == null) continue;

                    #region RouteSegment

                    var enrt_rts = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                    where (element != null) &&
                                        (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RouteSegment) && (((Aran.Aim.Features.RouteSegment)element.AIXM51_Feature).RouteFormed != null) &&
                                        (((Aran.Aim.Features.RouteSegment)element.AIXM51_Feature).RouteFormed.Identifier == aimENRT.Identifier)
                                    select element).ToList();

                    if (enrt_rts != null)
                    {
                        pdmENRT.Routes = new List<RouteSegment>();

                        foreach (var featureROUTE in enrt_rts)
                        {
                            var aimRoute = featureROUTE.AIXM51_Feature;

                            if ((featureROUTE.AixmGeo.Count > 0) && (featureROUTE.AixmGeo[0].GeometryType == esriGeometryType.esriGeometryPolyline))
                            {
                                RouteSegment pdmRouteSeg = (RouteSegment)AIM_PDM_Converter.AIM_Object_Convert(aimRoute, featureROUTE.AixmGeo);
                                if (pdmRouteSeg != null)
                                {

                                    #region StartPoint

                                    if ((((Aran.Aim.Features.RouteSegment)aimRoute).Start != null) &&
                                        (((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice != null) &&
                                        (((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice.AimingPoint != null))
                                    {
                                        PDMObject segStartPnt = GetSegmentPoint(((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice.Choice.ToString(), ((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice.AimingPoint.Identifier);
                                        if (segStartPnt != null)
                                        {
                                            PDMObject _obj = DefinePointSegmentDesignator(((SegmentPoint)segStartPnt).PointChoice, segStartPnt.ID);

                                            pdmRouteSeg.StartPoint = new RouteSegmentPoint
                                            {
                                                ID = Guid.NewGuid().ToString(),
                                                Route_LEG_ID = pdmRouteSeg.ID,
                                                PointRole = ProcedureFixRoleType.ENRT,
                                                PointUse = ProcedureSegmentPointUse.START_POINT,
                                                IsWaypoint = ((SegmentPoint)segStartPnt).IsWaypoint,
                                                PointChoice = ((SegmentPoint)segStartPnt).PointChoice,
                                                SegmentPointDesignator = ((SegmentPoint)segStartPnt).PointChoiceID,

                                            };

                                            pdmRouteSeg.StartPoint.IsWaypoint = ((Aran.Aim.Features.RouteSegment)aimRoute).Start.Waypoint.HasValue ? ((Aran.Aim.Features.RouteSegment)aimRoute).Start.Waypoint.Value : false;

                                            if (((Aran.Aim.Features.RouteSegment)aimRoute).Start.RadarGuidance.HasValue) { pdmRouteSeg.StartPoint.RadarGuidance = ((Aran.Aim.Features.RouteSegment)aimRoute).Start.RadarGuidance.Value; }

                                            if (((Aran.Aim.Features.RouteSegment)aimRoute).Start.ReportingATC.HasValue)
                                            {
                                                PDM.CodeATCReporting _uomATCRRep;
                                                Enum.TryParse<PDM.CodeATCReporting>(((Aran.Aim.Features.RouteSegment)aimRoute).Start.ReportingATC.Value.ToString(), out _uomATCRRep);
                                                pdmRouteSeg.StartPoint.ReportingATC = _uomATCRRep;

                                                if (pdmRouteSeg.StartPoint.PointChoice == PointChoice.DesignatedPoint)
                                                {
                                                    UpdateWayPointReportingATCvalue(segStartPnt.ID, _uomATCRRep);
                                                }

                                            }


                                            if (((Aran.Aim.Features.RouteSegment)aimRoute).Start.FlyOver.HasValue) { pdmRouteSeg.StartPoint.FlyOver = ((Aran.Aim.Features.RouteSegment)aimRoute).Start.FlyOver.Value; }


                                            if (_obj != null)
                                            {
                                                pdmRouteSeg.StartPoint.PointChoiceID = segStartPnt.ID;
                                                pdmRouteSeg.StartPoint.Lat = _obj.Lat;
                                                pdmRouteSeg.StartPoint.Lon = _obj.Lon;
                                            }
                                        }
                                    }

                                    #endregion


                                    #region EndPoint

                                    if ((((Aran.Aim.Features.RouteSegment)aimRoute).End != null) &&
                                        (((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice != null) &&
                                        (((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice.AimingPoint != null))
                                    {
                                        PDMObject segEndPnt = GetSegmentPoint(((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice.Choice.ToString(), ((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice.AimingPoint.Identifier);
                                        if (segEndPnt != null)
                                        {
                                            PDMObject _obj = DefinePointSegmentDesignator(((SegmentPoint)segEndPnt).PointChoice, segEndPnt.ID);

                                            pdmRouteSeg.EndPoint = new RouteSegmentPoint
                                            {
                                                ID = Guid.NewGuid().ToString(),
                                                Route_LEG_ID = pdmRouteSeg.ID,
                                                PointRole = ProcedureFixRoleType.ENRT,
                                                PointUse = ProcedureSegmentPointUse.END_POINT,
                                                IsWaypoint = ((SegmentPoint)segEndPnt).IsWaypoint,
                                                PointChoice = ((SegmentPoint)segEndPnt).PointChoice,
                                                SegmentPointDesignator = ((SegmentPoint)segEndPnt).PointChoiceID,
                                            };

                                            pdmRouteSeg.EndPoint.IsWaypoint = ((Aran.Aim.Features.RouteSegment)aimRoute).End.Waypoint.HasValue ? ((Aran.Aim.Features.RouteSegment)aimRoute).End.Waypoint.Value : false;

                                            if (((Aran.Aim.Features.RouteSegment)aimRoute).End.RadarGuidance.HasValue) { pdmRouteSeg.EndPoint.RadarGuidance = ((Aran.Aim.Features.RouteSegment)aimRoute).End.RadarGuidance.Value; }

                                            if (((Aran.Aim.Features.RouteSegment)aimRoute).End.ReportingATC.HasValue)
                                            {
                                                PDM.CodeATCReporting _uomATCRRep;
                                                Enum.TryParse<PDM.CodeATCReporting>(((Aran.Aim.Features.RouteSegment)aimRoute).End.ReportingATC.Value.ToString(), out _uomATCRRep);
                                                pdmRouteSeg.EndPoint.ReportingATC = _uomATCRRep;

                                                if (pdmRouteSeg.EndPoint.PointChoice == PointChoice.DesignatedPoint)
                                                {
                                                    UpdateWayPointReportingATCvalue(segEndPnt.ID, _uomATCRRep);
                                                }
                                            }


                                            if (((Aran.Aim.Features.RouteSegment)aimRoute).End.FlyOver.HasValue) { pdmRouteSeg.EndPoint.FlyOver = ((Aran.Aim.Features.RouteSegment)aimRoute).End.FlyOver.Value; }


                                            if (_obj != null)
                                            {
                                                pdmRouteSeg.EndPoint.PointChoiceID = segEndPnt.ID;
                                                pdmRouteSeg.EndPoint.Lat = _obj.Lat;
                                                pdmRouteSeg.EndPoint.Lon = _obj.Lon;
                                            }
                                        }
                                    }

                                    #endregion


                                    if ((pdmRouteSeg.StartPoint != null) && (pdmRouteSeg.EndPoint != null))
                                    {
                                        pdmRouteSeg.ID_Route = pdmENRT.ID;
                                        pdmENRT.RouteLength = pdmENRT.RouteLength + Math.Round(pdmRouteSeg.ConvertValueToMeter(pdmRouteSeg.ValLen, pdmRouteSeg.UomValLen.ToString()), 1);

                                        pdmENRT.Routes.Add(pdmRouteSeg);
                                    }

                                }

                            }
                        }

                    }


                    #endregion


                    //if (pdmENRT.StoreToDB(CurEnvironment.Data.TableDictionary))
                    {
                        CurEnvironment.Data.PdmObjectList.Add(pdmENRT);
                    }
                }




                #endregion

                #region Instrument Approach Procedures

                var iapList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.InstrumentApproachProcedure)
                               select element).ToList();

                foreach (var featureProc in iapList)
                {
                    var aimPROC = featureProc.AIXM51_Feature;

                    InstrumentApproachProcedure pdmIAP = (InstrumentApproachProcedure)AIM_PDM_Converter.AIM_Object_Convert(aimPROC, null);

                    if (pdmIAP == null) continue;
                    pdmIAP.Airport_ICAO_Code = GetAirportCode(pdmIAP.AirportIdentifier);
                    if (pdmIAP.LandingArea != null)
                    {
                        string Id = pdmIAP.LandingArea.ID;
                        pdmIAP.LandingArea = null;
                        pdmIAP.LandingArea = LandingTakeoffArea(Id);
                    }


                    #region Create Transitions

                    if (((Aran.Aim.Features.InstrumentApproachProcedure)aimPROC).FlightTransition != null)
                    {
                        pdmIAP.Transitions = new List<ProcedureTransitions>();


                        foreach (var aimFlightTransition in ((Aran.Aim.Features.InstrumentApproachProcedure)aimPROC).FlightTransition)
                        {

                            ProcedureTransitions pdmProcedureTransitions = CreateProcedureTransition(aimFlightTransition, pdmIAP.ID);

                            if (pdmProcedureTransitions != null)
                                pdmIAP.Transitions.Add(pdmProcedureTransitions);

                        }

                    }

                    #endregion

                    //if (pdmIAP.StoreToDB(CurEnvironment.Data.TableDictionary))
                    {
                        CurEnvironment.Data.PdmObjectList.Add(pdmIAP);
                    }

                }

                #endregion

                #region Standard Instrument Departure Procedures

                var sidList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.StandardInstrumentDeparture)
                               select element).ToList();

                foreach (var featureProc in sidList)
                {
                    var aimProc = featureProc.AIXM51_Feature;


                    //SID
                    StandardInstrumentDeparture pdmSID = (StandardInstrumentDeparture)AIM_PDM_Converter.AIM_Object_Convert(aimProc, null);

                    if (pdmSID == null) continue;
                    pdmSID.Airport_ICAO_Code = GetAirportCode(pdmSID.AirportIdentifier);

                    if (pdmSID.LandingArea != null)
                    {
                        string Id = pdmSID.LandingArea.ID;
                        pdmSID.LandingArea = null;
                        pdmSID.LandingArea = LandingTakeoffArea(Id);
                    }

                    #region Create Transitions

                    if (((Aran.Aim.Features.StandardInstrumentDeparture)aimProc).FlightTransition != null)
                    {
                        pdmSID.Transitions = new List<ProcedureTransitions>();


                        foreach (var aimFlightTransition in ((Aran.Aim.Features.StandardInstrumentDeparture)aimProc).FlightTransition)
                        {

                            ProcedureTransitions pdmProcedureTransitions = CreateProcedureTransition(aimFlightTransition, pdmSID.ID);

                            if (pdmProcedureTransitions != null)
                                pdmSID.Transitions.Add(pdmProcedureTransitions);

                        }

                    }

                    #endregion

                    //if (pdmSID.StoreToDB(CurEnvironment.Data.TableDictionary))
                    {
                        CurEnvironment.Data.PdmObjectList.Add(pdmSID);
                    }

                }


                #endregion

                #region Standard Instrument Arrival Procedures

                var starList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                where (element != null) &&
                                    (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.StandardInstrumentArrival)
                                select element).ToList();

                foreach (var featureProc in starList)
                {
                    var aimProc = featureProc.AIXM51_Feature;


                    //STAR
                    StandardInstrumentArrival pdmSTAR = (StandardInstrumentArrival)AIM_PDM_Converter.AIM_Object_Convert(aimProc, null);

                    if (pdmSTAR == null) continue;
                    pdmSTAR.Airport_ICAO_Code = GetAirportCode(pdmSTAR.AirportIdentifier);
                    if (pdmSTAR.LandingArea != null)
                    {
                        string Id = pdmSTAR.LandingArea.ID;
                        pdmSTAR.LandingArea = null;
                        pdmSTAR.LandingArea = LandingTakeoffArea(Id);
                    }

                    #region Create Transitions

                    if (((Aran.Aim.Features.StandardInstrumentArrival)aimProc).FlightTransition != null)
                    {
                        pdmSTAR.Transitions = new List<ProcedureTransitions>();


                        foreach (var aimFlightTransition in ((Aran.Aim.Features.StandardInstrumentArrival)aimProc).FlightTransition)
                        {

                            ProcedureTransitions pdmProcedureTransitions = CreateProcedureTransition(aimFlightTransition, pdmSTAR.ID);

                            if (pdmProcedureTransitions != null)
                                pdmSTAR.Transitions.Add(pdmProcedureTransitions);

                        }

                    }

                    #endregion

                    //if (pdmSTAR.StoreToDB(CurEnvironment.Data.TableDictionary))
                    {
                        CurEnvironment.Data.PdmObjectList.Add(pdmSTAR);
                    }

                }



                #endregion


            }

            List<string> IDS = new List<string>();
            List<PDMObject> duplIDS = new List<PDMObject>();
            foreach (var item in CurEnvironment.Data.PdmObjectList)
            {
                if (IDS.IndexOf(item.ID) >= 0)
                {
                    //item.ID = Guid.NewGuid().ToString();
                    duplIDS.Add(item);
                    continue;
                }

                item.StoreToDB(CurEnvironment.Data.TableDictionary);

                IDS.Add(item.ID);

            }

            CurEnvironment.Data.PdmObjectList.RemoveAll(duplIDS.Contains);

            var e_notes = (from element in CurEnvironment.Data.PdmObjectList where (element.Notes != null) && (element.Notes.Count > 0) select element).ToList();

            foreach (var _pdmObj in e_notes)
            {
                _pdmObj.StoreNotes(noteTable);
            }

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);


            foreach (var pair in CurEnvironment.Data.AirdromeHeliportDictionary)
            {
                var adhpID = pair.Key;
                var adhp = pair.Value;

                _FeatureClass.FeatureDataset.Workspace.ExecuteSQL("UPDATE WayPoint SET WayPoint.ID_AirportHeliport = '" + adhpID + "' WHERE (WayPoint.ID_AirportHeliport='" + adhp.Designator + "')");
                _FeatureClass.FeatureDataset.Workspace.ExecuteSQL("UPDATE NavaidSystem SET NavaidSystem.ID_AirportHeliport = '" + adhpID + "' WHERE (NavaidSystem.ID_AirportHeliport='" + adhp.Designator + "')");

                if (adhp.RunwayList != null)
                {
                    foreach (Runway rwy in adhp.RunwayList)
                    {
                        if (rwy.RunwayDirectionList != null)
                        {
                            foreach (RunwayDirection rdn in rwy.RunwayDirectionList)
                            {
                                _FeatureClass.FeatureDataset.Workspace.ExecuteSQL("UPDATE NavaidSystem SET NavaidSystem.ID_RunwayDirection = '" + rdn.ID + "' WHERE (NavaidSystem.ID_RunwayDirection='" + rdn.Designator + "')");

                            }
                        }
                    }
                }

            }




            CurEnvironment.SetCenter_and_Projection();
            CurEnvironment.Data.CloseToss();

            alrtForm.Close();

            return true;
        }

        private string GetAirportCode(string _id)
        {
            string res = "";
            PDMObject adhp = (from element in CurEnvironment.Data.PdmObjectList
                             where (element != null) &&
                                 (element is AirportHeliport) && (((AirportHeliport)element).ID.CompareTo(_id)==0)
                             select element).FirstOrDefault();
            if (adhp != null) res = ((AirportHeliport)adhp).Designator;

            return res;

        }

        private PDMObject LandingTakeoffArea(string _id)
        {
            PDMObject res = null;
            List<PDMObject> _objAdhp = (from element in CurEnvironment.Data.PdmObjectList
                             where (element != null) &&
                                 (element is AirportHeliport) && (((AirportHeliport)element).RunwayList != null) && (((AirportHeliport)element).RunwayList.Count>0) 
                             select element).ToList();

            foreach (AirportHeliport item in _objAdhp)
            {
                if (item.RunwayList == null) continue;

                foreach (var rwy in item.RunwayList)
                {
                    if (rwy.RunwayDirectionList == null) continue;


                    res = (from element in rwy.RunwayDirectionList
                             where (element != null) && (element.ID.CompareTo(_id)==0)
                             select element).FirstOrDefault();

                    if (res != null) return res;
                }

              
            }

            return res;
        }

        private double getStopWaylength(Guid aimThrIdentifier)
        {
            PDMObject temp = new PDMObject();

            var swy_thr = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                           where (element != null) && (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RunwayProtectArea) &&
                                (((Aran.Aim.Features.RunwayProtectArea)element.AIXM51_Feature).Type == CodeRunwayProtectionArea.STOPWAY) &&
                               (((Aran.Aim.Features.RunwayProtectArea)element.AIXM51_Feature).ProtectedRunwayDirection.Identifier == aimThrIdentifier)
                           select element).FirstOrDefault();


            if (swy_thr != null)
            {
                return temp.ConvertValueToMeter(((Aran.Aim.Features.RunwayProtectArea)swy_thr.AIXM51_Feature).Length.Value, ((Aran.Aim.Features.RunwayProtectArea)swy_thr.AIXM51_Feature).Length.Uom.ToString());
            }
            else return 0;
        }

        private double getClearWaylength(Guid aimThrIdentifier)
        {
            PDMObject temp = new PDMObject();

            var cwy_thr = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                           where (element != null) && (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RunwayProtectArea) &&
                                (((Aran.Aim.Features.RunwayProtectArea)element.AIXM51_Feature).Type == CodeRunwayProtectionArea.CWY) &&
                               (((Aran.Aim.Features.RunwayProtectArea)element.AIXM51_Feature).ProtectedRunwayDirection.Identifier == aimThrIdentifier)
                           select element).FirstOrDefault();


            if (cwy_thr != null)
            {
                return temp.ConvertValueToMeter(((Aran.Aim.Features.RunwayProtectArea)cwy_thr.AIXM51_Feature).Length.Value, ((Aran.Aim.Features.RunwayProtectArea)cwy_thr.AIXM51_Feature).Length.Uom.ToString());
            }
            else return 0;
        }

        private void UpdateWayPointReportingATCvalue(string _designatedpointIdentifier, PDM.CodeATCReporting _segmentPointATCReportingValue)
        {

           PDMObject _result = (from element in CurEnvironment.Data.PdmObjectList
                       where (element != null) &&
                           ((element.ID.CompareTo(_designatedpointIdentifier)==0))
                       select element).FirstOrDefault();

           if ((_result != null) && (_result is WayPoint))
           {
               if ((int)((WayPoint)_result).ReportingATC < (int)_segmentPointATCReportingValue) ((WayPoint)_result).ReportingATC = _segmentPointATCReportingValue;
           }

        }

        private IList<Intermediate_AIXM51_Arena> Define_AIM_List(string navEqType, Guid _Identifier)
        {
            IList<Intermediate_AIXM51_Arena> _result = null;

            switch (navEqType)
            {
                case ("DME"):

                    _result = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.DME) &&
                                   (((Aran.Aim.Features.DME)element.AIXM51_Feature).Identifier == _Identifier)
                               select element).ToList();
                    break;

                case ("VOR"):

                    _result = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.VOR) &&
                                   (((Aran.Aim.Features.VOR)element.AIXM51_Feature).Identifier == _Identifier)
                               select element).ToList();

                    break;
                case ("NDB"):

                    _result = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.NDB) &&
                                   (((Aran.Aim.Features.NDB)element.AIXM51_Feature).Identifier == _Identifier)
                               select element).ToList();

                    break;
                case ("TACAN"):

                    _result = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.TACAN) &&
                                   (((Aran.Aim.Features.TACAN)element.AIXM51_Feature).Identifier == _Identifier)
                               select element).ToList();

                    break;

                case ("Localizer"):

                    _result = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.Localizer) &&
                                   (((Aran.Aim.Features.Localizer)element.AIXM51_Feature).Identifier == _Identifier)
                               select element).ToList();

                    break;

                case ("Glidepath"):

                    _result = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.Glidepath) &&
                                   (((Aran.Aim.Features.Glidepath)element.AIXM51_Feature).Identifier == _Identifier)
                               select element).ToList();

                    break;

                case ("Navaid"):

                    _result = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.Navaid) &&
                                   (((Aran.Aim.Features.Navaid)element.AIXM51_Feature).Identifier == _Identifier)
                               select element).ToList();

                    break;

                case ("DesignatedPoint"):

                    _result = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.DesignatedPoint) &&
                                   (((Aran.Aim.Features.DesignatedPoint)element.AIXM51_Feature).Identifier == _Identifier)
                               select element).ToList();

                    break;

                case ("AirportHeliport"):
                    _result = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.AirportHeliport) &&
                                   (((Aran.Aim.Features.AirportHeliport)element.AIXM51_Feature).Identifier == _Identifier)
                               select element).ToList();
                                        break;

            }

            return _result;
        }

        private IList<Intermediate_AIXM51_Arena> Define_AIM_List(Guid _Identifier)
        {
            IList<Intermediate_AIXM51_Arena> _result = null;



                    _result = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   ((element.AIXM51_Feature).Identifier == _Identifier)
                               select element).ToList();

                    return _result;
           
        }

        private PDMObject GetComponent(string navEqType, Guid _Identifier)
        {
            IList<Intermediate_AIXM51_Arena> _list = Define_AIM_List(navEqType, _Identifier);
            PDMObject res = null;

            if (_list != null)
            {

                foreach (var feature in _list)
                {
                    var aimFeature = feature.AIXM51_Feature;

                    if ((feature.AixmGeo.Count > 0) && (feature.AixmGeo[0].GeometryType == esriGeometryType.esriGeometryPoint))
                    {
                        res = AIM_PDM_Converter.AIM_Object_Convert(aimFeature, feature.AixmGeo);
                    }
                }
            }

            return res;
        }

        private PDMObject GetComponent( Guid _Identifier)
        {
            IList<Intermediate_AIXM51_Arena> _list = Define_AIM_List( _Identifier);
            PDMObject res = null;

            if (_list != null)
            {

                foreach (var feature in _list)
                {
                    var aimFeature = feature.AIXM51_Feature;

                    if ((feature.AixmGeo.Count > 0) && (feature.AixmGeo[0].GeometryType == esriGeometryType.esriGeometryPoint))
                    {
                        res = AIM_PDM_Converter.AIM_Object_Convert(aimFeature, feature.AixmGeo);
                    }
                }
            }

            return res;
        }

        private PDMObject DefinePointSegmentDesignator(PointChoice pointChoice, string Identifier)
        {
            PDMObject res = new PDMObject();
            switch (pointChoice)
            {
                case PointChoice.DesignatedPoint:

                    PDMObject _dpn = (from element in CurEnvironment.Data.PdmObjectList where (element != null) && (element is WayPoint) && (((WayPoint)element).ID.CompareTo(Identifier) == 0) select element).FirstOrDefault();

                    if (_dpn != null)
                    {
                        res.ID = ((WayPoint)_dpn).Designator;

                        if ((((WayPoint)_dpn).Geo != null) && (((WayPoint)_dpn).Geo.GeometryType == esriGeometryType.esriGeometryPoint))
                        {
                            res.Lon = ((IPoint)(_dpn.Geo)).X.ToString();
                            res.Lat = ((IPoint)(_dpn.Geo)).Y.ToString();
                        }
                    }

                    break;

                case PointChoice.Navaid:

                    PDMObject _nvd = (from element in CurEnvironment.Data.PdmObjectList where (element != null) && (element is NavaidSystem) && (((NavaidSystem)element).ID.CompareTo(Identifier) == 0) select element).FirstOrDefault();

                    if (_nvd != null)
                    {
                        res.ID = ((NavaidSystem)_nvd).Designator;
                        if ((((NavaidSystem)_nvd).Components != null) && (((NavaidSystem)_nvd).Components.Count > 0))
                        {
                            res.Lon = ((IPoint)(((NavaidSystem)_nvd).Components[0].Geo)).X.ToString();
                            res.Lat = ((IPoint)(((NavaidSystem)_nvd).Components[0].Geo)).Y.ToString();
                        }
                    }

                    break;

                case PointChoice.RunwayCenterlinePoint:

                    PDMObject _rcp = (from element in CurEnvironment.Data.PdmObjectList where (element != null) && (element is RunwayCenterLinePoint) && (((RunwayCenterLinePoint)element).ID.CompareTo(Identifier) == 0) select element).FirstOrDefault();

                    if (_rcp != null)
                    {
                        res.ID = ((RunwayCenterLinePoint)_rcp).Designator;
                        if ((((RunwayCenterLinePoint)_rcp).Geo != null) && (((RunwayCenterLinePoint)_rcp).Geo.GeometryType == esriGeometryType.esriGeometryPoint))
                        {
                            res.Lon = ((IPoint)(_rcp.Geo)).X.ToString();
                            res.Lat = ((IPoint)(_rcp.Geo)).Y.ToString();
                        }
                    }

                    break;

                case PointChoice.AirportHeliport:

                    PDMObject _ahp = (from element in CurEnvironment.Data.PdmObjectList where (element != null) && (element is AirportHeliport) && (((AirportHeliport)element).ID.CompareTo(Identifier) == 0) select element).FirstOrDefault();

                    if (_ahp != null)
                    {
                        res.ID = ((AirportHeliport)_ahp).Designator;
                        if ((((AirportHeliport)_ahp).Geo != null) && (((AirportHeliport)_ahp).Geo.GeometryType == esriGeometryType.esriGeometryPoint))
                        {
                            res.Lon = ((IPoint)(_ahp.Geo)).X.ToString();
                            res.Lat = ((IPoint)(_ahp.Geo)).Y.ToString();
                        }
                    }

                    break;

                default:
                    res.ID = "";
                    res.Lon = "0";
                    res.Lat = "0";
                    break;
            }

            return res;
        }

        private PDMObject GetSegmentPoint(string navEqType, Guid _Identifier)
        {
            IList<Intermediate_AIXM51_Arena> _list = Define_AIM_List(navEqType, _Identifier);
            SegmentPoint res = null;

            if (_list != null)
            {

                foreach (var feature in _list)
                {
                    var aimFeature = feature.AIXM51_Feature;

                    switch (navEqType)
                    {
                        case ("DME"):
                        case ("VOR"):
                        case ("NDB"):
                        case ("TACAN"):
                        case ("Localizer"):
                        case ("Glidepath"):
                            res = new SegmentPoint
                            {
                                ID = ((Aran.Aim.Features.NavaidEquipment)aimFeature).Identifier.ToString(),
                                IsWaypoint = false,
                                PointChoice = PointChoice.Navaid,
                                PointChoiceID = ((Aran.Aim.Features.NavaidEquipment)aimFeature).Designator,
                            };

                            break;
                        case ("Navaid"):

                            res = new SegmentPoint
                            {
                                ID = ((Aran.Aim.Features.Navaid)aimFeature).Identifier.ToString(),
                                IsWaypoint = false,
                                PointChoice = PointChoice.Navaid,
                                PointChoiceID = ((Aran.Aim.Features.Navaid)aimFeature).Designator,
                            };

                            break;

                        case ("DesignatedPoint"):

                            res = new SegmentPoint
                            {
                                ID = ((Aran.Aim.Features.DesignatedPoint)aimFeature).Identifier.ToString(),
                                IsWaypoint = false,
                                PointChoice = PointChoice.DesignatedPoint,
                                PointChoiceID = ((Aran.Aim.Features.DesignatedPoint)aimFeature).Designator,
                            };

                            break;

                        case ("AirportHeliport"):
                            res = new SegmentPoint
                            {
                                ID = ((Aran.Aim.Features.AirportHeliport)aimFeature).Identifier.ToString(),
                                IsWaypoint = false,
                                PointChoice = PointChoice.AirportHeliport,
                                PointChoiceID = ((Aran.Aim.Features.AirportHeliport)aimFeature).Designator,
                            };
                            break;


                    }
                }
            }

            return res;
        }

        private ProcedureTransitions CreateProcedureTransition(Aran.Aim.Features.ProcedureTransition aimFlightTransition, string ProcID)
        {
            ProcedureTransitions pdmProcedureTransitions = (ProcedureTransitions)AIM_PDM_Converter.AIM_Object_Convert(aimFlightTransition, null);
            if (pdmProcedureTransitions != null)
            {
                pdmProcedureTransitions.ID_procedure = ProcID;
                pdmProcedureTransitions.Legs = new List<ProcedureLeg>();

                foreach (Aran.Aim.Features.ProcedureTransitionLeg aimLeg in aimFlightTransition.TransitionLeg)
                {
                    #region ProcedureLeg

                    Intermediate_AIXM51_Arena aimFeatureLeg = null;

                    switch (aimLeg.TheSegmentLeg.Type)
                    {
                        #region

                        case Aran.Aim.SegmentLegType.ArrivalFeederLeg:

                            aimFeatureLeg = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                    where (element != null) &&
                                        (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.ArrivalFeederLeg) &&
                                        (((Aran.Aim.Features.ArrivalFeederLeg)element.AIXM51_Feature).Identifier == aimLeg.TheSegmentLeg.Identifier)
                                    select element).FirstOrDefault();

                            break;

                        case Aran.Aim.SegmentLegType.FinalLeg:

                            aimFeatureLeg = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                    where (element != null) &&
                                        (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.FinalLeg) &&
                                        (((Aran.Aim.Features.FinalLeg)element.AIXM51_Feature).Identifier == aimLeg.TheSegmentLeg.Identifier)
                                    select element).FirstOrDefault();
                            break;

                        case Aran.Aim.SegmentLegType.InitialLeg:

                            aimFeatureLeg = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                    where (element != null) &&
                                        (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.InitialLeg) &&
                                        (((Aran.Aim.Features.InitialLeg)element.AIXM51_Feature).Identifier == aimLeg.TheSegmentLeg.Identifier)
                                    select element).FirstOrDefault();

                            break;

                        case Aran.Aim.SegmentLegType.IntermediateLeg:

                            aimFeatureLeg = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                    where (element != null) &&
                                        (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.IntermediateLeg) &&
                                        (((Aran.Aim.Features.IntermediateLeg)element.AIXM51_Feature).Identifier == aimLeg.TheSegmentLeg.Identifier)
                                    select element).FirstOrDefault();

                            break;

                        case Aran.Aim.SegmentLegType.MissedApproachLeg:

                            aimFeatureLeg = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                    where (element != null) &&
                                        (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.MissedApproachLeg) &&
                                        (((Aran.Aim.Features.MissedApproachLeg)element.AIXM51_Feature).Identifier == aimLeg.TheSegmentLeg.Identifier)
                                    select element).FirstOrDefault();

                            break;

                        case Aran.Aim.SegmentLegType.ArrivalLeg:

                            aimFeatureLeg = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                    where (element != null) &&
                                        (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.ArrivalLeg) &&
                                        (((Aran.Aim.Features.ArrivalLeg)element.AIXM51_Feature).Identifier == aimLeg.TheSegmentLeg.Identifier)
                                    select element).FirstOrDefault();

                            break;

                        case Aran.Aim.SegmentLegType.DepartureLeg:

                            aimFeatureLeg = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                    where (element != null) &&
                                        (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.DepartureLeg) &&
                                        (((Aran.Aim.Features.DepartureLeg)element.AIXM51_Feature).Identifier == aimLeg.TheSegmentLeg.Identifier)
                                    select element).FirstOrDefault();

                            break;

                        default:
                            break;

                        #endregion
                    }

                    if (aimFeatureLeg != null)
                    {
                        

                        if (aimFeatureLeg.AixmGeo.Count > 0)
                        {

                            var geoDataList = GetGeometry(aimFeatureLeg.AixmGeo);
                            ProcedureLeg pdmLeg = (ProcedureLeg)AIM_PDM_Converter.AIM_Object_Convert(aimFeatureLeg.AIXM51_Feature, geoDataList);

                            if (pdmLeg != null)
                            {
                                pdmLeg.ProcedureIdentifier = ProcID;
                                pdmLeg.TransitionIdentifier = pdmProcedureTransitions.ID;
                                pdmLeg.SeqNumberARINC = aimLeg.SeqNumberARINC.HasValue ? Convert.ToInt32(aimLeg.SeqNumberARINC.Value) : 0;
                                //pdmLeg.LegBlobGeometry = HelperClass.SetObjectToBlob(pdmLeg.Geo, "Leg");

                                if (pdmLeg.StartPoint != null)
                                {
                                    PDMObject _obj = DefinePointSegmentDesignator(pdmLeg.StartPoint.PointChoice, pdmLeg.StartPoint.PointChoiceID);
                                    pdmLeg.StartPoint.PointChoiceID = pdmLeg.StartPoint.PointChoiceID;
                                    pdmLeg.StartPoint.Lat = _obj.Lat;
                                    pdmLeg.StartPoint.Lon = _obj.Lon;
                                    pdmLeg.StartPoint.SegmentPointDesignator = _obj.ID;

                                    if (pdmLeg.StartPoint.PointFacilityMakeUp != null)
                                    {
                                        FillPointFacilityMakeUpProperies(pdmLeg.StartPoint.PointFacilityMakeUp);
                                    }

                                }

                                if (pdmLeg.EndPoint != null)
                                {
                                    PDMObject _obj = DefinePointSegmentDesignator(pdmLeg.EndPoint.PointChoice, pdmLeg.EndPoint.PointChoiceID);
                                    pdmLeg.EndPoint.PointChoiceID = pdmLeg.EndPoint.PointChoiceID;
                                    pdmLeg.EndPoint.Lat = _obj.Lat;
                                    pdmLeg.EndPoint.Lon = _obj.Lon;
                                    pdmLeg.EndPoint.SegmentPointDesignator = _obj.ID;

                                    if (pdmLeg.EndPoint.PointFacilityMakeUp != null)
                                    {
                                        FillPointFacilityMakeUpProperies(pdmLeg.EndPoint.PointFacilityMakeUp);
                                    }

                                }


                                if (pdmLeg.ArcCentre != null)
                                {
                                    PDMObject _obj = DefinePointSegmentDesignator(pdmLeg.ArcCentre.PointChoice, pdmLeg.ArcCentre.PointChoiceID);
                                    pdmLeg.ArcCentre.PointChoiceID = pdmLeg.ArcCentre.PointChoiceID;
                                    pdmLeg.ArcCentre.Lat = _obj.Lat;
                                    pdmLeg.ArcCentre.Lon = _obj.Lon;
                                    pdmLeg.ArcCentre.SegmentPointDesignator = _obj.ID;

                                    if (pdmLeg.ArcCentre.PointFacilityMakeUp != null)
                                    {
                                        FillPointFacilityMakeUpProperies(pdmLeg.ArcCentre.PointFacilityMakeUp);
                                    }

                                }

                                pdmProcedureTransitions.Legs.Add(pdmLeg);

                            }

                            else
                            {
                                System.Diagnostics.Debug.WriteLine("pdmLeg = null");
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("geoDataList.Count = 0");
                        }


                    }

                    #endregion

                }


            }

            return pdmProcedureTransitions;
        }

        private void FillPointFacilityMakeUpProperies(FacilityMakeUp facilityMakeUp)
        {
            #region AngleIndication

            if (facilityMakeUp.AngleIndication != null) 
            {
                var _andleInd = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                 where (element != null) &&
                                     (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.AngleIndication) &&
                                      (((Aran.Aim.Features.AngleIndication)element.AIXM51_Feature).Identifier.ToString().CompareTo(facilityMakeUp.AngleIndication.ID) == 0)
                                 select element).FirstOrDefault();

                facilityMakeUp.AngleIndication.ID = Guid.NewGuid().ToString();

                if (_andleInd != null)
                {
                    #region fill AngleIndication properies

                    var aimAngleInd = (Aran.Aim.Features.AngleIndication)_andleInd.AIXM51_Feature;
                    if (aimAngleInd.Angle.HasValue) facilityMakeUp.AngleIndication.Angle = aimAngleInd.Angle.Value;

                    if (aimAngleInd.AngleType.HasValue)
                    {
                        PDM.CodeBearing _uomAnglType;
                        Enum.TryParse<PDM.CodeBearing>(aimAngleInd.AngleType.Value.ToString(), out _uomAnglType);
                        facilityMakeUp.AngleIndication.AngleType = _uomAnglType;
                    }

                    if (aimAngleInd.IndicationDirection.HasValue)
                    {
                        PDM.CodeDirectionReference _uomDir;
                        Enum.TryParse<PDM.CodeDirectionReference>(aimAngleInd.IndicationDirection.Value.ToString(), out _uomDir);
                        facilityMakeUp.AngleIndication.IndicationDirection = _uomDir;
                    }

                    if (aimAngleInd.TrueAngle.HasValue) facilityMakeUp.AngleIndication.TrueAngle = aimAngleInd.TrueAngle.Value;

                    if (aimAngleInd.CardinalDirection.HasValue)
                    {
                        PDM.CodeCardinalDirection _uomDir;
                        Enum.TryParse<PDM.CodeCardinalDirection>(aimAngleInd.CardinalDirection.Value.ToString(), out _uomDir);
                        facilityMakeUp.AngleIndication.CardinalDirection = _uomDir;
                    }

                    if (aimAngleInd.MinimumReceptionAltitude != null)
                    {
                        facilityMakeUp.AngleIndication.MinimumReceptionAltitude = aimAngleInd.MinimumReceptionAltitude.Value;

                        UOM_DIST_VERT _uomVert;
                        Enum.TryParse<UOM_DIST_VERT>(aimAngleInd.MinimumReceptionAltitude.Uom.ToString(), out _uomVert);
                        facilityMakeUp.AngleIndication.MinimumReceptionAltitudeUOM = _uomVert;
                    }

                    if ((aimAngleInd.Fix!=null)&&(aimAngleInd.Fix.Identifier != null)) facilityMakeUp.AngleIndication.FixID = aimAngleInd.Fix.Identifier.ToString();

                    if (aimAngleInd.PointChoice != null)
                    {
                        facilityMakeUp.AngleIndication.SignificantPointID = DefineSignifantPointID(aimAngleInd.PointChoice);
                        facilityMakeUp.AngleIndication.SignificantPointType = aimAngleInd.PointChoice.Choice.ToString();

                    }

                    

                    #endregion
                }
            }

            #endregion

            #region DistanceIndication

            if (facilityMakeUp.DistanceIndication != null)
            {

                var _distInd = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                where (element != null) &&
                                    (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.DistanceIndication) &&
                                     (((Aran.Aim.Features.DistanceIndication)element.AIXM51_Feature).Identifier.ToString().CompareTo(facilityMakeUp.DistanceIndication.ID) == 0)
                                select element).FirstOrDefault();

                facilityMakeUp.DistanceIndication.ID = Guid.NewGuid().ToString();

                if (_distInd != null)
                {
                    #region fill DistanceIndication properties

                    var aimDistInd = (Aran.Aim.Features.DistanceIndication)_distInd.AIXM51_Feature;
                    if (aimDistInd.Distance != null)
                    {
                        facilityMakeUp.DistanceIndication.Distance = aimDistInd.Distance.Value;

                        UOM_DIST_HORZ _uomHor;
                        Enum.TryParse<UOM_DIST_HORZ>(aimDistInd.Distance.Uom.ToString(), out _uomHor);
                        facilityMakeUp.DistanceIndication.DistanceUOM = _uomHor;

                    }

                    if (aimDistInd.MinimumReceptionAltitude != null)
                    {
                        facilityMakeUp.DistanceIndication.MinimumReceptionAltitude = aimDistInd.MinimumReceptionAltitude.Value;

                        UOM_DIST_VERT _uomVer;
                        Enum.TryParse<UOM_DIST_VERT>(aimDistInd.MinimumReceptionAltitude.Uom.ToString(), out _uomVer);
                        facilityMakeUp.DistanceIndication.MinimumReceptionAltitudeUOM = _uomVer;
                    }

                    if (aimDistInd.PointChoice != null)
                    {
                        facilityMakeUp.DistanceIndication.SignificantPointID = DefineSignifantPointID(aimDistInd.PointChoice);
                        facilityMakeUp.DistanceIndication.SignificantPointType = aimDistInd.PointChoice.Choice.ToString();

                    }

                    #endregion
                }


            }

            #endregion

        }

        private string DefineSignifantPointID(Aran.Aim.Features.SignificantPoint pointChoice)
        {
            string res = "";

            switch (pointChoice.Choice)
            {
                case Aran.Aim.SignificantPointChoice.DesignatedPoint:
                    res = pointChoice.FixDesignatedPoint.Identifier.ToString();
                    break;
                case Aran.Aim.SignificantPointChoice.Navaid:
                    res = pointChoice.NavaidSystem.Identifier.ToString();
                    break;
                case Aran.Aim.SignificantPointChoice.AirportHeliport:
                    res = pointChoice.AirportReferencePoint.Identifier.ToString();
                    break;
                default:
                    break;
            }

            return res;
        }

        private List<IGeometry> GetGeometry(List<IGeometry> GeomList)
        {
            // Filter by type
            return (from element in GeomList where (element != null) && (element.GeometryType == esriGeometryType.esriGeometryPolyline) select element).ToList();
            
        }


    }
}
