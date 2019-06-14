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

namespace ARENA.DataLoaders
{
    public class TOOSM_DataConverter : IARENA_DATA_Converter
    {
        private Environment.Environment _CurEnvironment;

        public Environment.Environment CurEnvironment
        {
            get { return _CurEnvironment; }
            set { _CurEnvironment = value; }
        }

        public TOOSM_DataConverter()
        {
        }

        public TOOSM_DataConverter(Environment.Environment env)
        {
            this.CurEnvironment = env;
        }

        public bool Convert_Data(IFeatureClass _FeatureClass)
        {
            AlertForm alrtForm = new AlertForm();
            alrtForm.FormBorderStyle = FormBorderStyle.None;
            alrtForm.Opacity = 0.5;
            alrtForm.BackgroundImage = ARENA.Properties.Resources.ArenaSplash;
            //alrtForm.TopMost = true;

            if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();

            CurEnvironment.Data.OpenToss();

            //// Workspace Geo
            var workspaceEdit = (IWorkspaceEdit)_FeatureClass.FeatureDataset.Workspace;
            workspaceEdit.StartEditing(false);
            workspaceEdit.StartEditOperation();

            #region

            CurEnvironment.FillAirtrackTableDic(workspaceEdit);
            ITable noteTable = EsriUtils.getTableByname((IFeatureWorkspace)workspaceEdit, "Notes");

            #endregion

            #region Vertical structure

            if (CurEnvironment.Data.AIM_VerticalStructure_LIST != null)
            {
                foreach (var featureObs in CurEnvironment.Data.AIM_VerticalStructure_LIST)
                {
                    var aimObs = featureObs.Data.Feature;
                    GeometryFormatter.PrepareGeometry(featureObs.Data, true);
                    var geoDataList = GetGeometry(featureObs.Data, new List<int>()
                                            {
                                                (int)PropertyVerticalStructure.Part,

                                            });



                    if (geoDataList.Count > 0)
                    {
                        VerticalStructure pdmOBS = (VerticalStructure)AIM_PDM_Converter.AIM_Object_Convert(aimObs, geoDataList);
                        if (pdmOBS == null) continue;

                        foreach (var item in pdmOBS.Parts)
                        {
                            item.Vertex = HelperClass.SetObjectToBlob(item.Geo, "Vertex");
                        }

                        if (pdmOBS.StoreToDB(CurEnvironment.Data.TableDictionary))
                        {
                            CurEnvironment.Data.PdmObjectList.Add(pdmOBS);
                        }
                    }

                }
            }

            #endregion

            #region Airports/RWY/THR/NAVAID

            if (CurEnvironment.Data.AIM_ADHP_LIST != null)
            {
                foreach (var featureADHP in CurEnvironment.Data.AIM_ADHP_LIST)
                {
                    var aimADHP = featureADHP.Data.Feature;

                    GeometryFormatter.PrepareGeometry(featureADHP.Data, true);
                    var geoDataList = featureADHP.Data.PropertyExtensions.OfType<EsriPropertyExtension>().Select(t => t.EsriObject).ToList();

                    if ((geoDataList.Count > 0) && (geoDataList[0].GeometryType == esriGeometryType.esriGeometryPoint))
                    {
                        AirportHeliport pdmADHP = (AirportHeliport)AIM_PDM_Converter.AIM_Object_Convert(aimADHP, geoDataList);

                        if (pdmADHP == null) continue;

                        #region RWY/THR/NAVAID

                        if (CurEnvironment.Data.AIM_RWY_LIST != null)
                        {
                            var adhp_rwy = (from element in CurEnvironment.Data.AIM_RWY_LIST
                                            where (element != null) &&
                                                (element.FeatureTypeId == (int)Aran.Aim.FeatureType.Runway) &&
                                                (((Aran.Aim.Features.Runway)element.Data.Feature).AssociatedAirportHeliport.Identifier == aimADHP.Identifier)
                                            select element).ToList();

                            if (adhp_rwy != null)
                            {
                                pdmADHP.RunwayList = new List<Runway>();

                                foreach (var featureRWY in adhp_rwy)
                                {
                                    var aimRWY = featureRWY.Data.Feature;
                                    Runway pdmRWY = (Runway)AIM_PDM_Converter.AIM_Object_Convert(aimRWY, null);

                                    if (pdmRWY != null)
                                    {
                                        pdmRWY.ID_AirportHeliport = pdmADHP.ID;

                                        #region THR

                                        var rwy_thr = (from element in CurEnvironment.Data.AIM_RDN_LIST
                                                       where (element != null) &&
                                                           (element.FeatureTypeId == (int)Aran.Aim.FeatureType.RunwayDirection) &&
                                                           (((Aran.Aim.Features.RunwayDirection)element.Data.Feature).UsedRunway.Identifier == aimRWY.Identifier)
                                                       select element).ToList();

                                        if (rwy_thr != null)
                                        {
                                            pdmRWY.RunwayDirectionList = new List<RunwayDirection>();

                                            foreach (var featureTHR in rwy_thr)
                                            {
                                                var aimTHR = featureTHR.Data.Feature;

                                                #region Center Line Points THR Geometry

                                                var thr_clp = (from element in CurEnvironment.Data.AIM_CLP_LIST
                                                               where (element != null) &&
                                                                   (element.FeatureTypeId == (int)Aran.Aim.FeatureType.RunwayCentrelinePoint) &&
                                                                    (((Aran.Aim.Features.RunwayCentrelinePoint)element.Data.Feature).OnRunway != null) &&
                                                                   (((Aran.Aim.Features.RunwayCentrelinePoint)element.Data.Feature).OnRunway.Identifier == aimTHR.Identifier) &&
                                                                   (((Aran.Aim.Features.RunwayCentrelinePoint)element.Data.Feature).Role != null) &&
                                                                   (((Aran.Aim.Features.RunwayCentrelinePoint)element.Data.Feature).Role == Aran.Aim.Enums.CodeRunwayPointRole.THR)
                                                               select element).FirstOrDefault();

                                                if (thr_clp == null) continue;
                                                GeometryFormatter.PrepareGeometry(thr_clp.Data, true);
                                                geoDataList = thr_clp.Data.PropertyExtensions.OfType<EsriPropertyExtension>().Select(t => t.EsriObject).ToList();


                                                #endregion

                                                if ((geoDataList.Count > 0) && (geoDataList[0].GeometryType == esriGeometryType.esriGeometryPoint))
                                                {
                                                    RunwayDirection pdmTHR = (RunwayDirection)AIM_PDM_Converter.AIM_Object_Convert(aimTHR, geoDataList);

                                                    if (pdmTHR != null)
                                                    {
                                                        pdmTHR.ID_AirportHeliport = pdmADHP.ID;
                                                        pdmTHR.ID_Runway = pdmRWY.ID;

                                                        #region Navaids


                                                        var thr_nvd = (from element in CurEnvironment.Data.AIM_NVD_LIST
                                                                       where (element != null) &&
                                                                           (element.FeatureTypeId == (int)Aran.Aim.FeatureType.Navaid) &&
                                                                            (((Aran.Aim.Features.Navaid)element.Data.Feature).RunwayDirection != null) &&
                                                                            (((Aran.Aim.Features.Navaid)element.Data.Feature).RunwayDirection.Count > 0) &&
                                                                           (((Aran.Aim.Features.Navaid)element.Data.Feature).RunwayDirection[0].Feature.Identifier == aimTHR.Identifier)
                                                                       select element).ToList();

                                                        if (thr_nvd != null)
                                                        {
                                                            pdmTHR.Related_NavaidSystem = new List<NavaidSystem>();

                                                            foreach (var featureNav in thr_nvd)
                                                            {
                                                                var aimnav = featureNav.Data.Feature;
                                                                NavaidSystem pdmNavSys = (NavaidSystem)AIM_PDM_Converter.AIM_Object_Convert(aimnav, null);
                                                                if (pdmNavSys != null)
                                                                {
                                                                    CurEnvironment.Data.PdmObjectList.Add(pdmNavSys);

                                                                    pdmNavSys.ID_AirportHeliport = pdmADHP.ID;
                                                                    pdmNavSys.ID_RunwayDirection = pdmTHR.ID;
                                                                    pdmNavSys.Components = new List<PDMObject>();

                                                                    #region navaidEquipment

                                                                    if ((((Aran.Aim.Features.Navaid)aimnav).NavaidEquipment != null) && (((Aran.Aim.Features.Navaid)aimnav).NavaidEquipment.Count > 0))
                                                                    {
                                                                        foreach (var item in ((Aran.Aim.Features.Navaid)aimnav).NavaidEquipment)
                                                                        {
                                                                            var pdm_navEqpnt = GetComponent(item.TheNavaidEquipment.Type.ToString(), item.TheNavaidEquipment.Identifier);
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

                                                        var _clp = (from element in CurEnvironment.Data.AIM_CLP_LIST
                                                                    where (element != null) &&
                                                                        (element.FeatureTypeId == (int)Aran.Aim.FeatureType.RunwayCentrelinePoint) &&
                                                                         (((Aran.Aim.Features.RunwayCentrelinePoint)element.Data.Feature).OnRunway != null) &&
                                                                        (((Aran.Aim.Features.RunwayCentrelinePoint)element.Data.Feature).OnRunway.Identifier == aimTHR.Identifier)
                                                                    select element).ToList();

                                                        if (_clp != null)
                                                        {
                                                            pdmTHR.CenterLinePoints = new List<RunwayCenterLinePoint>();

                                                            foreach (var featureCLP in _clp)
                                                            {
                                                                var aimCLP = featureCLP.Data.Feature;

                                                                GeometryFormatter.PrepareGeometry(featureCLP.Data, true);
                                                                geoDataList = featureCLP.Data.PropertyExtensions.OfType<EsriPropertyExtension>().Select(t => t.EsriObject).ToList();

                                                                if ((geoDataList.Count > 0) && (geoDataList[0].GeometryType == esriGeometryType.esriGeometryPoint))
                                                                {
                                                                    RunwayCenterLinePoint pdmCLP = (RunwayCenterLinePoint)AIM_PDM_Converter.AIM_Object_Convert(aimCLP, geoDataList);
                                                                    if (pdmCLP != null)
                                                                    {
                                                                        pdmCLP.ID_RunwayDirection = pdmTHR.ID;
                                                                        pdmTHR.CenterLinePoints.Add(pdmCLP);
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

                        }
                        #endregion

                        if (pdmADHP.StoreToDB(CurEnvironment.Data.TableDictionary))
                        {
                            //pdmObj.Tag = obj.ARINC_OBJ;
                            CurEnvironment.Data.PdmObjectList.Add(pdmADHP);
                        }

                    }

                }
            }

            #endregion

            #region Navaids

            if (CurEnvironment.Data.AIM_NVD_LIST != null)
            {
                var _nvd = (from element in CurEnvironment.Data.AIM_NVD_LIST
                            where (element != null) &&
                                (element.FeatureTypeId == (int)Aran.Aim.FeatureType.Navaid) &&
                                 ((((Aran.Aim.Features.Navaid)element.Data.Feature).RunwayDirection == null) || (((Aran.Aim.Features.Navaid)element.Data.Feature).RunwayDirection.Count == 0))
                            select element).ToList();

                if (_nvd != null)
                {
                    foreach (var featureNav in _nvd)
                    {
                        var aimnav = featureNav.Data.Feature;
                        NavaidSystem pdmNavSys = (NavaidSystem)AIM_PDM_Converter.AIM_Object_Convert(aimnav, null);
                        if (pdmNavSys != null)
                        {
                            pdmNavSys.Components = new List<PDMObject>();

                            #region navaidEquipment

                            if ((((Aran.Aim.Features.Navaid)aimnav).NavaidEquipment != null) && (((Aran.Aim.Features.Navaid)aimnav).NavaidEquipment.Count > 0))
                            {
                                foreach (var item in ((Aran.Aim.Features.Navaid)aimnav).NavaidEquipment)
                                {
                                    var pdm_navEqpnt = GetComponent(item.TheNavaidEquipment.Type.ToString(), item.TheNavaidEquipment.Identifier);
                                    if (pdm_navEqpnt != null)
                                    {
                                        ((NavaidComponent)pdm_navEqpnt).ID_NavaidSystem = pdmNavSys.ID;
                                        pdmNavSys.Components.Add(pdm_navEqpnt);
                                    }

                                }
                            }
                            #endregion

                            if (pdmNavSys.StoreToDB(CurEnvironment.Data.TableDictionary))
                            {
                                CurEnvironment.Data.PdmObjectList.Add(pdmNavSys);
                            }

                        }
                    }
                }
            }

            #endregion

            #region Waypoint

            if (CurEnvironment.Data.AIM_WYP_LIST != null)
            {
                foreach (var featureWYP in CurEnvironment.Data.AIM_WYP_LIST)
                {
                    var aimDPN = featureWYP.Data.Feature;

                    GeometryFormatter.PrepareGeometry(featureWYP.Data, true);
                    var geoDataList = featureWYP.Data.PropertyExtensions.OfType<EsriPropertyExtension>().Select(t => t.EsriObject).ToList();

                    if ((geoDataList.Count > 0) && (geoDataList[0].GeometryType == esriGeometryType.esriGeometryPoint))
                    {
                        WayPoint pdmWYP = (WayPoint)AIM_PDM_Converter.AIM_Object_Convert(aimDPN, geoDataList);

                        if (pdmWYP == null) continue;

                        if (pdmWYP.StoreToDB(CurEnvironment.Data.TableDictionary))
                        {
                            //pdmObj.Tag = obj.ARINC_OBJ;
                            CurEnvironment.Data.PdmObjectList.Add(pdmWYP);
                        }
                    }
                }

            }



            #endregion

            #region Airspace

            if (CurEnvironment.Data.AIM_ARSP_LIST != null)
            {
                foreach (var featureARSPS in CurEnvironment.Data.AIM_ARSP_LIST)
                {
                    var aimARSP = featureARSPS.Data.Feature;

                    Airspace pdmARSPS = (Airspace)AIM_PDM_Converter.AIM_Object_Convert(aimARSP, null);

                    if (pdmARSPS == null) continue;

                    pdmARSPS.AirspaceVolumeList = new List<AirspaceVolume>();


                    GeometryFormatter.PrepareGeometry(featureARSPS.Data, true);
                    var geoDataList = featureARSPS.Data.PropertyExtensions.OfType<EsriPropertyExtension>().ToList();

                    for (int i = 0; i <= geoDataList.Count - 1; i++)
                    {
                        var geo = geoDataList[i];
                        var esri_gm = (IGeometry)HelperClass.EsriFromBytes(geo.EsriData, "esri");
                        if ((esri_gm != null) && (esri_gm.GeometryType == esriGeometryType.esriGeometryPolygon))
                        {
                            var aimArsp_vol = (((Aran.Aim.Features.Airspace)aimARSP).GeometryComponent[i].TheAirspaceVolume);
                            List<IGeometry> geoList = new List<IGeometry>();
                            geoList.Add(esri_gm);
                            AirspaceVolume pdmARSPS_Vol = (AirspaceVolume)AIM_PDM_Converter.AIM_Object_Convert(aimArsp_vol, geoList);

                            if (pdmARSPS_Vol == null) continue;


                            pdmARSPS_Vol.BrdrGeometry = HelperClass.SetObjectToBlob(pdmARSPS_Vol.Geo, "Border");
                            pdmARSPS_Vol.ID_Airspace = pdmARSPS.ID;
                            //pdmARSPS_Vol.TxtLocalType = ((Aran.Aim.Features.Airspace)aimARSP).Type.HasValue ? ((Aran.Aim.Features.Airspace)aimARSP).Type.Value.ToString() : "";
                            pdmARSPS_Vol.TxtName = ((Aran.Aim.Features.Airspace)aimARSP).Designator;
                            pdmARSPS_Vol.CodeId = ((Aran.Aim.Features.Airspace)aimARSP).Designator;
                            pdmARSPS_Vol.CodeClass = pdmARSPS.Lat; // временное харнение класса Airspace
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



                    if (pdmARSPS.StoreToDB(CurEnvironment.Data.TableDictionary))
                    {
                        //pdmObj.Tag = obj.ARINC_OBJ;
                        CurEnvironment.Data.PdmObjectList.Add(pdmARSPS);
                    }

                }

            }



            #endregion

            #region Enroute/RouteSegment/RouteSegmentPoint


            if (CurEnvironment.Data.AIM_ENRT_LIST != null)
            {
                foreach (var featureARSPS in CurEnvironment.Data.AIM_ENRT_LIST)
                {
                    var aimENRT = featureARSPS.Data.Feature;

                    Enroute pdmENRT = (Enroute)AIM_PDM_Converter.AIM_Object_Convert(aimENRT, null);

                    if (pdmENRT == null) continue;

                    #region RouteSegment

                    if (CurEnvironment.Data.AIM_RWY_LIST != null)
                    {
                        var enrt_rts = (from element in CurEnvironment.Data.AIM_ROUTE_LIST
                                        where (element != null) &&
                                            (element.FeatureTypeId == (int)Aran.Aim.FeatureType.RouteSegment) &&
                                            (((Aran.Aim.Features.RouteSegment)element.Data.Feature).RouteFormed.Identifier == aimENRT.Identifier)
                                        select element).ToList();

                        if (enrt_rts != null)
                        {
                            pdmENRT.Routes = new List<RouteSegment>();

                            foreach (var featureROUTE in enrt_rts)
                            {
                                var aimRoute = featureROUTE.Data.Feature;

                                GeometryFormatter.PrepareGeometry(featureROUTE.Data, true);
                                var geoDataList = featureROUTE.Data.PropertyExtensions.OfType<EsriPropertyExtension>().Select(t => t.EsriObject).ToList();

                                if ((geoDataList.Count > 0) && (geoDataList[0].GeometryType == esriGeometryType.esriGeometryPolyline))
                                {
                                    RouteSegment pdmRouteSeg = (RouteSegment)AIM_PDM_Converter.AIM_Object_Convert(aimRoute, geoDataList);
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
                                                    //PointChoiceID = ((SegmentPoint)segEndPnt).PointChoiceID,
                                                    SegmentPointDesignator = ((SegmentPoint)segEndPnt).PointChoiceID,
                                                };

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
                    }

                    #endregion


                    if (pdmENRT.StoreToDB(CurEnvironment.Data.TableDictionary))
                    {
                        //pdmObj.Tag = obj.ARINC_OBJ;
                        CurEnvironment.Data.PdmObjectList.Add(pdmENRT);
                    }
                }


            }

            #endregion

            #region Instrument Approach Procedures

            if (CurEnvironment.Data.AIM_IAP_LIST != null)
            {
                foreach (var featureProc in CurEnvironment.Data.AIM_IAP_LIST)
                {
                    var aimPROC = featureProc.Data.Feature;

                    InstrumentApproachProcedure pdmIAP = (InstrumentApproachProcedure)AIM_PDM_Converter.AIM_Object_Convert(aimPROC, null);

                    if (pdmIAP == null) continue;


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

                    if (pdmIAP.StoreToDB(CurEnvironment.Data.TableDictionary))
                    {
                        CurEnvironment.Data.PdmObjectList.Add(pdmIAP);
                    }

                }
            }






            #endregion

            #region Standard Instrument Departure Procedures

            if (CurEnvironment.Data.AIM_SID_LIST != null)
            {
                foreach (var featureProc in CurEnvironment.Data.AIM_SID_LIST)
                {
                    var aimProc = featureProc.Data.Feature;


                    //SID
                    StandardInstrumentDeparture pdmSID = (StandardInstrumentDeparture)AIM_PDM_Converter.AIM_Object_Convert(aimProc, null);

                    if (pdmSID == null) continue;


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

                    if (pdmSID.StoreToDB(CurEnvironment.Data.TableDictionary))
                    {
                        CurEnvironment.Data.PdmObjectList.Add(pdmSID);
                    }

                }
            }

            #endregion

            #region Standard Instrument Arrival Procedures

            if (CurEnvironment.Data.AIM_STAR_LIST != null)
            {
                foreach (var featureProc in CurEnvironment.Data.AIM_STAR_LIST)
                {
                    var aimProc = featureProc.Data.Feature;


                    //STAR
                    StandardInstrumentArrival pdmSTAR = (StandardInstrumentArrival)AIM_PDM_Converter.AIM_Object_Convert(aimProc, null);

                    if (pdmSTAR == null) continue;


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

                    if (pdmSTAR.StoreToDB(CurEnvironment.Data.TableDictionary))
                    {
                        CurEnvironment.Data.PdmObjectList.Add(pdmSTAR);
                    }

                }
            }


            #endregion


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

                    object _leg = null;

                    switch (aimLeg.TheSegmentLeg.Type)
                    {
                        #region

                        case Aran.Aim.SegmentLegType.ArrivalFeederLeg:

                            _leg = (from element in CurEnvironment.Data.AIM_ArrivalFeederLeg_LIST
                                    where (element != null) &&
                                        (element.FeatureTypeId == (int)Aran.Aim.FeatureType.ArrivalFeederLeg) &&
                                        (((Aran.Aim.Features.ArrivalFeederLeg)element.Data.Feature).Identifier == aimLeg.TheSegmentLeg.Identifier)
                                    select element).FirstOrDefault();

                            break;

                        case Aran.Aim.SegmentLegType.FinalLeg:

                            _leg = (from element in CurEnvironment.Data.AIM_FinalLeg_LIST
                                    where (element != null) &&
                                        (element.FeatureTypeId == (int)Aran.Aim.FeatureType.FinalLeg) &&
                                        (((Aran.Aim.Features.FinalLeg)element.Data.Feature).Identifier == aimLeg.TheSegmentLeg.Identifier)
                                    select element).FirstOrDefault();
                            break;

                        case Aran.Aim.SegmentLegType.InitialLeg:

                            _leg = (from element in CurEnvironment.Data.AIM_InitialLeg_LIST
                                    where (element != null) &&
                                        (element.FeatureTypeId == (int)Aran.Aim.FeatureType.InitialLeg) &&
                                        (((Aran.Aim.Features.InitialLeg)element.Data.Feature).Identifier == aimLeg.TheSegmentLeg.Identifier)
                                    select element).FirstOrDefault();

                            break;

                        case Aran.Aim.SegmentLegType.IntermediateLeg:

                            _leg = (from element in CurEnvironment.Data.AIM_IntermediateLeg_LIST
                                    where (element != null) &&
                                        (element.FeatureTypeId == (int)Aran.Aim.FeatureType.IntermediateLeg) &&
                                        (((Aran.Aim.Features.IntermediateLeg)element.Data.Feature).Identifier == aimLeg.TheSegmentLeg.Identifier)
                                    select element).FirstOrDefault();

                            break;

                        case Aran.Aim.SegmentLegType.MissedApproachLeg:

                            _leg = (from element in CurEnvironment.Data.AIM_MissedApproachLeg_LIST
                                    where (element != null) &&
                                        (element.FeatureTypeId == (int)Aran.Aim.FeatureType.MissedApproachLeg) &&
                                        (((Aran.Aim.Features.MissedApproachLeg)element.Data.Feature).Identifier == aimLeg.TheSegmentLeg.Identifier)
                                    select element).FirstOrDefault();

                            break;

                        case Aran.Aim.SegmentLegType.ArrivalLeg:

                            _leg = (from element in CurEnvironment.Data.AIM_ArrivalLeg_LIST
                                    where (element != null) &&
                                        (element.FeatureTypeId == (int)Aran.Aim.FeatureType.ArrivalLeg) &&
                                        (((Aran.Aim.Features.ArrivalLeg)element.Data.Feature).Identifier == aimLeg.TheSegmentLeg.Identifier)
                                    select element).FirstOrDefault();

                            break;

                        case Aran.Aim.SegmentLegType.DepartureLeg:

                            _leg = (from element in CurEnvironment.Data.AIM_DepartureLeg_LIST
                                    where (element != null) &&
                                        (element.FeatureTypeId == (int)Aran.Aim.FeatureType.DepartureLeg) &&
                                        (((Aran.Aim.Features.DepartureLeg)element.Data.Feature).Identifier == aimLeg.TheSegmentLeg.Identifier)
                                    select element).FirstOrDefault();

                            break;

                        default:
                            break;

                        #endregion
                    }

                    if (_leg != null)
                    {
                        var aimFeatureLeg = ((Aran.Temporality.Common.Aim.MetaData.AimState)_leg).Data;
                        GeometryFormatter.PrepareGeometry(aimFeatureLeg, true);


                        var geoDataList = GetGeometry(aimFeatureLeg, new List<int>()
                                            {
                                                (int)PropertySegmentLeg.Trajectory,
                                            });
                        if (geoDataList.Count > 0)
                        {
                            ProcedureLeg pdmLeg = (ProcedureLeg)AIM_PDM_Converter.AIM_Object_Convert(aimFeatureLeg.Feature, geoDataList);

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

            if ((facilityMakeUp.AngleIndication != null) && (CurEnvironment.Data.AIM_AngleIndication_LIST != null))
            {
                var _andleInd = (from element in CurEnvironment.Data.AIM_AngleIndication_LIST
                                 where (element != null) &&
                                     (element.FeatureTypeId == (int)Aran.Aim.FeatureType.AngleIndication) &&
                                      (((Aran.Aim.Features.AngleIndication)element.Data.Feature).Identifier.ToString().CompareTo(facilityMakeUp.AngleIndication.ID) == 0)
                                 select element).FirstOrDefault();

                facilityMakeUp.AngleIndication.ID = Guid.NewGuid().ToString();

                if (_andleInd != null)
                {
                    #region fill AngleIndication properies

                    var aimAngleInd = (Aran.Aim.Features.AngleIndication)_andleInd.Data.Feature;
                    if (aimAngleInd.Angle.HasValue) facilityMakeUp.AngleIndication.Angle = aimAngleInd.Angle.Value;

                    if (aimAngleInd.AngleType.HasValue)
                    {
                        CodeBearing _uomAnglType;
                        Enum.TryParse<CodeBearing>(aimAngleInd.AngleType.Value.ToString(), out _uomAnglType);
                        facilityMakeUp.AngleIndication.AngleType = _uomAnglType;
                    }

                    if (aimAngleInd.IndicationDirection.HasValue)
                    {
                        CodeDirectionReference _uomDir;
                        Enum.TryParse<CodeDirectionReference>(aimAngleInd.IndicationDirection.Value.ToString(), out _uomDir);
                        facilityMakeUp.AngleIndication.IndicationDirection = _uomDir;
                    }

                    if (aimAngleInd.TrueAngle.HasValue) facilityMakeUp.AngleIndication.TrueAngle = aimAngleInd.TrueAngle.Value;

                    if (aimAngleInd.CardinalDirection.HasValue)
                    {
                        CodeCardinalDirection _uomDir;
                        Enum.TryParse<CodeCardinalDirection>(aimAngleInd.CardinalDirection.Value.ToString(), out _uomDir);
                        facilityMakeUp.AngleIndication.CardinalDirection = _uomDir;
                    }

                    if (aimAngleInd.MinimumReceptionAltitude != null)
                    {
                        facilityMakeUp.AngleIndication.MinimumReceptionAltitude = aimAngleInd.MinimumReceptionAltitude.Value;

                        UOM_DIST_VERT _uomVert;
                        Enum.TryParse<UOM_DIST_VERT>(aimAngleInd.MinimumReceptionAltitude.Uom.ToString(), out _uomVert);
                        facilityMakeUp.AngleIndication.MinimumReceptionAltitudeUOM = _uomVert;
                    }

                    if (aimAngleInd.Fix.Identifier != null) facilityMakeUp.AngleIndication.FixID = aimAngleInd.Fix.Identifier.ToString();

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

            if ((facilityMakeUp.DistanceIndication != null) && (CurEnvironment.Data.AIM_DistanceIndication_LIST != null))
            {

                var _distInd = (from element in CurEnvironment.Data.AIM_DistanceIndication_LIST
                                where (element != null) &&
                                    (element.FeatureTypeId == (int)Aran.Aim.FeatureType.DistanceIndication) &&
                                     (((Aran.Aim.Features.DistanceIndication)element.Data.Feature).Identifier.ToString().CompareTo(facilityMakeUp.DistanceIndication.ID) == 0)
                                select element).FirstOrDefault();

                facilityMakeUp.DistanceIndication.ID = Guid.NewGuid().ToString();

                if (_distInd != null)
                {
                    #region fill DistanceIndication properties

                    var aimDistInd = (Aran.Aim.Features.DistanceIndication)_distInd.Data.Feature;
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

        public static List<IGeometry> GetGeometry(AimFeature aimFeature, List<int> propertyPath)
        {
            // Filter by type

            var extensions = aimFeature.PropertyExtensions.OfType<EsriPropertyExtension>().ToList();

            // Filter by propertyPath
            if (extensions.Count > 0)
            {

                string Pp = String.Join(",", propertyPath);
                extensions = extensions.Where(t => String.Join(",", t.PropertyPath).StartsWith(Pp)).ToList();

            }

            if (extensions.Count > 0)
            {
                return extensions.Select(t => t.EsriObject).ToList();
            }
            return new List<IGeometry>();
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> Define_AIM_List(string navEqType, Guid _Identifier)
        {
            IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _result = null;

            switch (navEqType)
            {
                case ("DME"):

                    _result = (from element in CurEnvironment.Data.AIM_DME_LIST
                               where (element != null) &&
                                   (element.FeatureTypeId == (int)Aran.Aim.FeatureType.DME) &&
                                   (((Aran.Aim.Features.DME)element.Data.Feature).Identifier == _Identifier)
                               select element).ToList();
                    break;

                case ("VOR"):

                    _result = (from element in CurEnvironment.Data.AIM_VOR_LIST
                               where (element != null) &&
                                   (element.FeatureTypeId == (int)Aran.Aim.FeatureType.VOR) &&
                                   (((Aran.Aim.Features.VOR)element.Data.Feature).Identifier == _Identifier)
                               select element).ToList();

                    break;
                case ("NDB"):

                    _result = (from element in CurEnvironment.Data.AIM_NDB_LIST
                               where (element != null) &&
                                   (element.FeatureTypeId == (int)Aran.Aim.FeatureType.NDB) &&
                                   (((Aran.Aim.Features.NDB)element.Data.Feature).Identifier == _Identifier)
                               select element).ToList();

                    break;
                case ("TACAN"):

                    _result = (from element in CurEnvironment.Data.AIM_TACAN_LIST
                               where (element != null) &&
                                   (element.FeatureTypeId == (int)Aran.Aim.FeatureType.TACAN) &&
                                   (((Aran.Aim.Features.TACAN)element.Data.Feature).Identifier == _Identifier)
                               select element).ToList();

                    break;

                case ("Localizer"):

                    _result = (from element in CurEnvironment.Data.AIM_LOC_LIST
                               where (element != null) &&
                                   (element.FeatureTypeId == (int)Aran.Aim.FeatureType.Localizer) &&
                                   (((Aran.Aim.Features.Localizer)element.Data.Feature).Identifier == _Identifier)
                               select element).ToList();

                    break;

                case ("Glidepath"):

                    _result = (from element in CurEnvironment.Data.AIM_GLD_P_LIST
                               where (element != null) &&
                                   (element.FeatureTypeId == (int)Aran.Aim.FeatureType.Glidepath) &&
                                   (((Aran.Aim.Features.Glidepath)element.Data.Feature).Identifier == _Identifier)
                               select element).ToList();

                    break;

                case ("Navaid"):

                    _result = (from element in CurEnvironment.Data.AIM_NVD_LIST
                               where (element != null) &&
                                   (element.FeatureTypeId == (int)Aran.Aim.FeatureType.Navaid) &&
                                   (((Aran.Aim.Features.Navaid)element.Data.Feature).Identifier == _Identifier)
                               select element).ToList();

                    break;

                case ("DesignatedPoint"):

                    _result = (from element in CurEnvironment.Data.AIM_WYP_LIST
                               where (element != null) &&
                                   (element.FeatureTypeId == (int)Aran.Aim.FeatureType.DesignatedPoint) &&
                                   (((Aran.Aim.Features.DesignatedPoint)element.Data.Feature).Identifier == _Identifier)
                               select element).ToList();

                    break;

            }

            return _result;
        }

        private PDMObject GetComponent(string navEqType, Guid _Identifier)
        {
            IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _list = Define_AIM_List(navEqType, _Identifier);
            PDMObject res = null;

            if (_list != null)
            {

                foreach (var feature in _list)
                {
                    var aimFeature = feature.Data.Feature;

                    GeometryFormatter.PrepareGeometry(feature.Data, true);
                    var geoDataList = feature.Data.PropertyExtensions.OfType<EsriPropertyExtension>().Select(t => t.EsriObject).ToList();

                    if ((geoDataList.Count > 0) && (geoDataList[0].GeometryType == esriGeometryType.esriGeometryPoint))
                    {
                        res = AIM_PDM_Converter.AIM_Object_Convert(aimFeature, geoDataList);
                    }
                }
            }

            return res;
        }

        private PDMObject GetSegmentPoint(string navEqType, Guid _Identifier)
        {
            IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _list = Define_AIM_List(navEqType, _Identifier);
            SegmentPoint res = null;

            if (_list != null)
            {

                foreach (var feature in _list)
                {
                    var aimFeature = feature.Data.Feature;

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

                    }
                }
            }

            return res;
        }
    }

}
