using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PDM;
using System.Windows.Forms;
using ARINC_DECODER_CORE.AIRTRACK_Objects;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using Accent.MapElements;
using ARENA.Project;
using ESRI.ArcGIS.Geometry;
using EsriWorkEnvironment;
using ESRI.ArcGIS.esriSystem;
using AIXM45Loader;
using AreaManager;
using Microsoft.Win32;

namespace ARENA.DataLoaders
{
    public class AIXM45_DataConverter : IARENA_DATA_Converter
    {
        private Environment.Environment _CurEnvironment;

        public Environment.Environment CurEnvironment
        {
            get { return _CurEnvironment; }
            set { _CurEnvironment = value; }
        }

        private AranSupport.Utilitys util;
        private List<AIXM45_Object> aixm45List;
        private List<string> NavaidsList;

        public AIXM45_DataConverter()
        {
        }

        public AIXM45_DataConverter(Environment.Environment env)
        {
            this.CurEnvironment = env;
            util = new AranSupport.Utilitys();
            NavaidsList = new List<string>();
        }

        public bool Convert_Data(IFeatureClass _FeatureClass)
        {
             var frm = new AIXM45Loader.Form1();

             if (frm.ShowDialog() == DialogResult.OK)
             {
                 if (frm.Tag!=null)
                 {
                     {

                         AlertForm alrtForm = new AlertForm();
                         alrtForm.FormBorderStyle = FormBorderStyle.None;
                         alrtForm.Opacity = 0.5;
                         alrtForm.BackgroundImage = ARENA.Properties.Resources.ArenaSplash;
                         //alrtForm.TopMost = true;

                         if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();
                         aixm45List = ((List<AIXM45_Object>)frm.Tag);

                         List<PDMObject> lst = createPDM_objects();

                         Application.DoEvents();

                         //// Workspace Geo
                         var workspaceEdit = (IWorkspaceEdit)_FeatureClass.FeatureDataset.Workspace;
                         if (workspaceEdit.IsBeingEdited())
                         {
                             workspaceEdit.StopEditOperation();
                             workspaceEdit.StopEditing(false);
                         }
                         workspaceEdit.StartEditing(false);
                         workspaceEdit.StartEditOperation();

                         #region

                         CurEnvironment.FillAirtrackTableDic(workspaceEdit);



                         #endregion


                         ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                         if (lst != null)
                             foreach (var pdmObj in lst)
                             {

                                 
                                 if (pdmObj != null)
                                 {

                                     #region Obj is Airspace


                                     if (pdmObj is Airspace)
                                     {

                                         foreach (var vol in ((Airspace)pdmObj).AirspaceVolumeList)
                                         {
                                             
                                             if (vol.Geo == null)
                                             {
                                                 vol.Geo = BuildAirspaceBorder(((Airspace)pdmObj).Notes[0], CurEnvironment, _FeatureClass); 
                                                 //vol.BrdrGeometry = HelperClass.SetObjectToBlob(vol.Geo, "Border");
                                             }
                                         }
                                             
                                         
                                     }

                                     #endregion

                                     #region Obj is Obsracle

                                     //if (pdmObj is VerticalStructure)
                                     //{
                                     //    foreach (var item in ((VerticalStructure)pdmObj).Parts)
                                     //    {
                                     //        item.Vertex = HelperClass.SetObjectToBlob(item.Geo, "Vertex");

                                     //    }
                                     //}

                                     #endregion

                                     if (pdmObj.StoreToDB(CurEnvironment.Data.TableDictionary))
                                     {
                                         CurEnvironment.Data.PdmObjectList.Add(pdmObj);
                                     }

                                     #region obj is AIRPORT

                                     if (pdmObj is AirportHeliport)
                                     {
                                         var arp = (AirportHeliport)pdmObj;

                                         if ((arp.RunwayList != null) && (arp.RunwayList.Count > 0))
                                         {
                                             foreach (var rwy in arp.RunwayList)
                                             {

                                                 if ((rwy.RunwayDirectionList == null) || (rwy.RunwayDirectionList.Count <= 0)) continue;
                                                 foreach (var rdn in rwy.RunwayDirectionList)
                                                 {
                                                     if ((rdn.Related_NavaidSystem == null) || (rdn.Related_NavaidSystem.Count <= 0)) continue;
                                                     foreach (NavaidSystem nav in rdn.Related_NavaidSystem)
                                                         CurEnvironment.Data.PdmObjectList.Add(nav);

                                                 }


                                             }
                                         }

                                     }

                                     #endregion

                                 }
                             }

                         ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                         if (workspaceEdit.IsBeingEdited()) 

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

                        

                         alrtForm.Close();
                     }
                 }

             }
            return true;
        }

        private List<PDMObject> createPDM_objects()
        {
            List<PDMObject> res = new List<PDMObject>();
         
            AreaInfo Area = new AreaInfo();
            Area = AreaManager.AreaUtils.GetArea(GetPathToAreaFile());
                        double CM = (Area.AreaPolygon as IArea).Centroid.X;


            #region Ahp/Rwy/Rdn/RCP/ILS

            var ahpList = (from element in aixm45List where (element != null) && (element is AIXM45_Airport) select element).ToList();

            foreach (var ahp in ahpList)
            {
                AirportHeliport ahpPDM = GetAirport((AIXM45_Airport)ahp);

                if ((ahpPDM != null) && (ahpPDM.Geo != null) && (util.WithinPolygon(Area.AreaPolygon, ahpPDM.Geo as IPoint)))
                    res.Add(ahpPDM);
            }

            #endregion

            #region DME (VOR/DME)

            var dmeList = (from element in aixm45List where (element != null) && (element is AIXM45_DME) select element).ToList();

            foreach (var dme in dmeList)
            {
                if (NavaidsList.IndexOf(((AIXM45_DME)dme).R_mid) >= 0) continue;
                NavaidSystem navSystemPdm = GetNavaidsSystem((AIXM45_DME)dme);

                if (CheckLocation(navSystemPdm, Area)) res.Add(navSystemPdm);

            }

            #endregion

            #region VOR

            var vorList = (from element in aixm45List where (element != null) && (element is AIXM45_VOR) select element).ToList();

            foreach (var vor in vorList)
            {
                if (NavaidsList.IndexOf(((AIXM45_VOR)vor).R_mid) >= 0) continue;

                NavaidSystem navSystemPdm = GetNavaidsSystem((AIXM45_VOR)vor);

                if (CheckLocation(navSystemPdm, Area)) res.Add(navSystemPdm);

            }

            #endregion

            #region NDB

            var ndbList = (from element in aixm45List where (element != null) && (element is AIXM45_NDB) select element).ToList();

            foreach (var ndb in ndbList)
            {
                if (NavaidsList.IndexOf(((AIXM45_NDB)ndb).R_mid) >= 0) continue;

                NavaidSystem navSystemPdm = GetNavaidsSystem((AIXM45_NDB)ndb);

                if (CheckLocation(navSystemPdm, Area)) 
                    res.Add(navSystemPdm);

            }

            #endregion

            #region Marker

            var mkrList = (from element in aixm45List where (element != null) && (element is AIXM45_Marker) select element).ToList();

            foreach (var mkr in mkrList)
            {
                if (NavaidsList.IndexOf(((AIXM45_Marker)mkr).R_mid) >= 0) continue;
                NavaidSystem navSystemPdm = GetNavaidsSystem((AIXM45_Marker)mkr);

                if (CheckLocation(navSystemPdm, Area))
                    res.Add(navSystemPdm);

            }

            #endregion

            #region WayPoint

            var dpnList = (from element in aixm45List where (element != null) && (element is AIXM45_WayPoint) select element).ToList();

            foreach (var dpn in dpnList)
            {

                WayPoint ahpDPN = GetWaypoint((AIXM45_WayPoint)dpn);

                //if ((ahpDPN != null) && (ahpDPN.Geo != null) && (util.WithinPolygon(Area.AreaPolygon, ahpDPN.Geo as IPoint)))
                    res.Add(ahpDPN);

            }

            #endregion

            #region Enroute

             var routeList = (from element in aixm45List where (element != null) && (element is AIXM45_Enrote) select element).ToList();

             foreach (var route in routeList)
             {
                 Enroute rtePDM = GetEnroute((AIXM45_Enrote)route);

                //if (CheckLocation(rtePDM, Area)) 
                    res.Add(rtePDM);

             }

            #endregion

            #region Airspace

             var airspaceList = (from element in aixm45List where (element != null) && (element is AIXM45_Airspace) select element).ToList();

             foreach (var arsps in airspaceList)
             {
                 Airspace arspsPDM = GetAirspace((AIXM45_Airspace)arsps);
                 
                 //if (CheckLocation(arspsPDM, Area)) 
                     res.Add(arspsPDM);

             }

             #endregion

            #region obstacle

             var obsList = (from element in aixm45List where (element != null) && (element is AIXM45_Obstacles) select element).ToList();

             foreach (var obs in obsList)
             {
                 VerticalStructure vsPDM = GetVerticalStructure((AIXM45_Obstacles)obs);


                 if ((vsPDM != null) && (vsPDM.Parts != null))
                 {
                     foreach (var item in vsPDM.Parts)
                     {
                         if (util.WithinPolygon(Area.AreaPolygon, item.Geo))
                         {
                             res.Add(vsPDM);
                             break;
                         }
                     }


                     
                 }
             }

             #endregion


            res.Add(new AREA_PDM { Name = Area.CountryName, Geo = Area.AreaPolygon });

            return res;
        }

        private VerticalStructure GetVerticalStructure(AIXM45_Obstacles aIXM45_Obstacles)
        {

            VerticalStructure res = new VerticalStructure
            {
                ID = Guid.NewGuid().ToString(),
                Name = aIXM45_Obstacles.txtName,
                Group = false,
                Lighted = aIXM45_Obstacles.codeLgt,
                LightingICAOStandard = false,
                MarkingICAOStandard = false,
                SynchronisedLighting = false,
                Type = VerticalStructureType.OTHER,
                Parts = CreateVerticalStructureParts(aIXM45_Obstacles),
            };

            return res;
        }

        private List<VerticalStructurePart> CreateVerticalStructureParts(AIXM45_Obstacles aIXM45_Obstacles)
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();
            List<VerticalStructurePart> res = new List<VerticalStructurePart>();

            VerticalStructurePart part = new VerticalStructurePart
            {

                ID = Guid.NewGuid().ToString(),
                Lat = aIXM45_Obstacles.R_geoLat,
                Lon = aIXM45_Obstacles.R_geoLong,
                Elev = aIXM45_Obstacles.valElev,
                //Elev_m = obsAIRTRACK.ElevM,
                Elev_UOM = aIXM45_Obstacles.uomDistVer.Length>0 ? (UOM_DIST_VERT)Enum.Parse(typeof(UOM_DIST_VERT), aIXM45_Obstacles.uomDistVer) : UOM_DIST_VERT.M,
                Height = aIXM45_Obstacles.valHgt,
                Height_UOM = aIXM45_Obstacles.uomDistVer.Length > 0 ? (UOM_DIST_VERT)Enum.Parse(typeof(UOM_DIST_VERT), aIXM45_Obstacles.uomDistVer) : UOM_DIST_VERT.M,
                //HeightM = obsAIRTRACK.HeightM,
                Designator = aIXM45_Obstacles.txtName.Length > 0 ? aIXM45_Obstacles.txtName : aIXM45_Obstacles.txtDescrType,
                ConstructionStatus = StatusConstructionType.OTHER,
                Frangible = false,
                MarkingFirstColour = ColourType.OTHER,
                MarkingPattern = VerticalStructureMarkingType.OTHER,
                MarkingSecondColour = ColourType.OTHER,
                Mobile = false,
                Type = VerticalStructureType.OTHER,
                VerticalExtent = aIXM45_Obstacles.valElev,
                VerticalExtent_UOM = aIXM45_Obstacles.uomDistVer.Length > 0 ? (UOM_DIST_VERT)Enum.Parse(typeof(UOM_DIST_VERT), aIXM45_Obstacles.uomDistVer) : UOM_DIST_VERT.M,
                VerticalExtentAccuracy = aIXM45_Obstacles.valElevAccuracy,
                VerticalExtentAccuracy_UOM = aIXM45_Obstacles.uomGeoAccuracy.Length>0 ? (UOM_DIST_VERT)Enum.Parse(typeof(UOM_DIST_VERT), aIXM45_Obstacles.uomGeoAccuracy) : UOM_DIST_VERT.M,
                VisibleMaterial = VerticalStructureMaterialType.OTHER,
            };

            if ((part.Lat.Trim().Length > 0) && (part.Lon.Trim().Length > 0))
            {

                IPoint pnt = ArnUtil.Create_ESRI_POINT(part.Lat.Trim(), part.Lon.Trim(), part.Elev.ToString(), part.Elev_UOM.ToString());
                if (pnt != null)
                {
                    part.Geo = pnt;
                    part.Vertex = HelperClass.SetObjectToBlob(part.Geo, "Vertex");
                }


            }

            res.Add(part);
            return res;
        }

        private Airspace GetAirspace(AIXM45_Airspace aIXM45_Airspace)
        {
            string masterID = Guid.NewGuid().ToString();
            Airspace _Airspace = new Airspace
            {
                ID = masterID,
                CodeID = aIXM45_Airspace.R_codeId,
                TxtName = aIXM45_Airspace.TxtName,
                AirspaceVolumeList = GetVolumeList(aIXM45_Airspace, masterID),
                Notes = new List<string>(),
            };

            _Airspace.Notes.Add(aIXM45_Airspace.R_mid);
            return _Airspace;

        }

        private List<AirspaceVolume> GetVolumeList(AIXM45_Airspace aIXM45_Airspace,string _Master_ID)
        {
            List<AirspaceVolume> res = new List<AirspaceVolume>();

            AirspaceVolume vol = new AirspaceVolume
            {
                ID = Guid.NewGuid().ToString(),
                ID_Airspace = _Master_ID,
                CodeActivity = aIXM45_Airspace.CodeActivity,
                CodeId = aIXM45_Airspace.R_codeId,
                CodeClass = aIXM45_Airspace.CodeClass,
                CodeLocInd = aIXM45_Airspace.CodeLocInd,
                CodeMil = aIXM45_Airspace.CodeMil,
                TxtLocalType = aIXM45_Airspace.TxtLocalType,
                TxtName = aIXM45_Airspace.TxtName,
                ValDistVerLower = aIXM45_Airspace.ValDistVerLower,
                ValDistVerMax = aIXM45_Airspace.ValDistVerMax,
                ValDistVerMnm = aIXM45_Airspace.ValDistVerMnm,
                ValDistVerUpper = aIXM45_Airspace.ValDistVerUpper,
                ValLowerLimit = aIXM45_Airspace.ValLowerLimit,

            };

            CODE_DIST_VER _uomDist_code;
            Enum.TryParse<CODE_DIST_VER>(aIXM45_Airspace.CodeDistVerLower.ToString(), out _uomDist_code);
            vol.CodeDistVerLower = _uomDist_code;

            Enum.TryParse<CODE_DIST_VER>(aIXM45_Airspace.CodeDistVerMax.ToString(), out _uomDist_code);
            vol.CodeDistVerMax = _uomDist_code;

            Enum.TryParse<CODE_DIST_VER>(aIXM45_Airspace.CodeDistVerMnm.ToString(), out _uomDist_code);
            vol.CodeDistVerMnm = _uomDist_code;

            Enum.TryParse<CODE_DIST_VER>(aIXM45_Airspace.CodeDistVerUpper.ToString(), out _uomDist_code);
            vol.CodeDistVerUpper = _uomDist_code;

            AirspaceType _uomType;
            Enum.TryParse<AirspaceType>(aIXM45_Airspace.R_codeType.ToString(), out _uomType);
            vol.CodeType = _uomType;


            UOM_DIST_VERT _uomVert;
            Enum.TryParse<UOM_DIST_VERT>(aIXM45_Airspace.UomDistVerLower.ToString(), out _uomVert);
            vol.UomDistVerLower = _uomVert;

            Enum.TryParse<UOM_DIST_VERT>(aIXM45_Airspace.UomDistVerMax.ToString(), out _uomVert);
            vol.UomDistVerMax = _uomVert;

            Enum.TryParse<UOM_DIST_VERT>(aIXM45_Airspace.UomDistVerMnm.ToString(), out _uomVert);
            vol.UomDistVerMnm = _uomVert;

            Enum.TryParse<UOM_DIST_VERT>(aIXM45_Airspace.UomDistVerUpper.ToString(), out _uomVert);
            vol.UomDistVerUpper = _uomVert;


            if (aIXM45_Airspace.Geometry != null)
            {
                vol.Geo = aIXM45_Airspace.Geometry;
                vol.BrdrGeometry = HelperClass.SetObjectToBlob(vol.Geo, "Border");
            }

            res.Add(vol);

            

            return res;
        }

        private IGeometry BuildAirspaceBorder(string arspsId, Environment.Environment Environment, IFeatureClass fc)
        {

            PolygonClass arspBorder;
            try
            {
                AIXM45_Airspace arsps = (AIXM45_Airspace)(from element in aixm45List where (element != null) && (element is AIXM45_Airspace) && (((AIXM45_Airspace)element).R_mid.CompareTo(arspsId) == 0) select element).FirstOrDefault();

                var wksPointsList = new List<WKSPoint>();

                Environment.Data.SpatialReference = ((IGeoDataset)fc).SpatialReference;

                IPointCollection Crcl = new MultipointClass();

                arspBorder = new PolygonClass();
                IGeometryBridge2 geometryBridge = new GeometryEnvironmentClass();

                AIXM45_AirspaceVertex arspVrtx;
                WKSPoint wksPnt;
                IPoint pPnt;
                List<AIXM45_AirspaceVertex> partGbr = new List<AIXM45_AirspaceVertex>();

                for (int i = 0; i <= arsps.Border.Count - 1; i++)
                {

                    int startIndx = -1;
                    int endIndx = -1;

                    AIXM45_AirspaceBorderItem brdr = arsps.Border[i];
                    switch (brdr.CodeType)
                    {
                        case BorderItemCodeType.FNT:

                            #region

                            var objStart = (from element in brdr.Gbr where (element.CrcCode == brdr.FinalPoint.CrcCode) select element).FirstOrDefault();
                            startIndx = brdr.Gbr.ToList().IndexOf(objStart);

                            if (i != arsps.Border.Count - 1)
                                arspVrtx = arsps.Border[i + 1].FinalPoint;
                            else
                                arspVrtx = arsps.Border[i].FinalPoint;

                            var objEnd = (from element in brdr.Gbr where (element.CrcCode == arspVrtx.CrcCode)  select element).FirstOrDefault();
                            endIndx = brdr.Gbr.ToList().IndexOf(objEnd);


                            if ((startIndx >= 0) && (endIndx >= 0))
                            {
                                if (startIndx <= endIndx)
                                {
                                    partGbr = brdr.Gbr.ToList().GetRange(startIndx, endIndx - startIndx);

                                }
                                else
                                {
                                    partGbr = brdr.Gbr.ToList().GetRange(endIndx, startIndx - endIndx);
                                    partGbr.Reverse();
                                }
                            }


                            foreach (var item in partGbr)
                            {
                                wksPointsList.Add(item.Vrtx);
                            }

                            #endregion

                            break;

                        case BorderItemCodeType.CWA:
                        case BorderItemCodeType.CCA:

                            #region


                            IPoint pPntCntr = new PointClass();
                            IPoint pPntFrom = new PointClass();
                            IPoint pPntTo = new PointClass();

                            pPntCntr.PutCoords(brdr.CenterPoint.Vrtx.X, brdr.CenterPoint.Vrtx.Y);
                            pPntCntr = EsriUtils.ToProject(pPntCntr, Environment.MapControl.Map, Environment.Data.SpatialReference) as IPoint;

                            pPntFrom.PutCoords(brdr.FinalPoint.Vrtx.X, brdr.FinalPoint.Vrtx.Y);
                            pPntFrom = EsriUtils.ToProject(pPntFrom, Environment.MapControl.Map, Environment.Data.SpatialReference) as IPoint;

                            if (i != arsps.Border.Count - 1)
                                pPntTo.PutCoords(arsps.Border[i + 1].FinalPoint.Vrtx.X, arsps.Border[i + 1].FinalPoint.Vrtx.Y);
                            else
                                pPntTo.PutCoords(wksPointsList[0].X, wksPointsList[0].Y);

                            pPntTo = EsriUtils.ToProject(pPntTo, Environment.MapControl.Map, Environment.Data.SpatialReference) as IPoint;

                            var arcCurse = -1;
                            if (brdr.CodeType == BorderItemCodeType.CCA) arcCurse = 1;

                            Crcl = Environment.Data.GeometryFunctions.CreateArcPrj2(pPntCntr, pPntFrom, pPntTo, arcCurse);

                            for (int j = 0; j <= Crcl.PointCount - 1; j++)
                            {
                                pPnt = new PointClass();
                                pPnt.PutCoords(Crcl.Point[j].X, Crcl.Point[j].Y);
                                pPnt = EsriUtils.ToGeo(pPnt, Environment.MapControl.Map, Environment.Data.SpatialReference) as IPoint;

                                wksPnt = new WKSPoint();
                                wksPnt.X = pPnt.X;
                                wksPnt.Y = pPnt.Y;
                                wksPointsList.Add(wksPnt);
                            }

                            #endregion

                            break;

                        case BorderItemCodeType.PNT:

                            wksPointsList.Add(brdr.FinalPoint.Vrtx);

                            break;

                        case BorderItemCodeType.CRCL:

                            #region

                            pPnt = new PointClass();
                            pPnt.PutCoords(brdr.CenterPoint.Vrtx.X, brdr.CenterPoint.Vrtx.Y);
                            pPnt = EsriUtils.ToProject(pPnt, Environment.MapControl.Map, Environment.Data.SpatialReference) as IPoint;

                            Crcl = Environment.Data.GeometryFunctions.CreatePrjCircle(pPnt, util.ConvertValueToMeter(brdr.Radius.ToString(), brdr.UomRadius));

                            for (int j = 0; j <= Crcl.PointCount - 1; j++)
                            {
                                pPnt = new PointClass();
                                pPnt.PutCoords(Crcl.Point[j].X, Crcl.Point[j].Y);
                                pPnt = EsriUtils.ToGeo(pPnt, Environment.MapControl.Map, Environment.Data.SpatialReference) as IPoint;

                                wksPnt = new WKSPoint { X = pPnt.X, Y = pPnt.Y };
                                wksPointsList.Add(wksPnt);
                            }

                            #endregion

                            break;
                        default:
                            break;
                    }

                }

                WKSPoint[] pointArray = wksPointsList.ToArray();

                geometryBridge.AddWKSPoints(arspBorder, ref pointArray);
                arspBorder.Simplify();

                var zAware = arspBorder as IZAware;
                zAware.ZAware = true;
                arspBorder.SetConstantZ(0);


                var mAware = arspBorder as IMAware;
                mAware.MAware = true;



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace + " ERROR " + ex.Message);
                throw;
            }

            return arspBorder;



           
        }

        private Enroute GetEnroute(AIXM45_Enrote aIXM_Enrote)
        {
            string masterID = Guid.NewGuid().ToString();
            Enroute _Enroute = new Enroute
            {
                #region

                ID = masterID,//aIXM_Enrote.R_mid,
                TxtDesig = aIXM_Enrote.R_txtDesig,
                Routes = GetRouteSegments(aIXM_Enrote.R_mid, masterID),

                #endregion
            };

            _Enroute.RebuildGeo();

            return _Enroute;
        }

        private List<RouteSegment> GetRouteSegments(string _MID_Enroute, string _Master_ID)
        {
            List<RouteSegment> res = new List<RouteSegment>();

            var segmentList = (from element in aixm45List where (element != null) && (element is AIXM45_RouteSegment) && (((AIXM45_RouteSegment)element).R_RteMid.CompareTo(_MID_Enroute) == 0) select element).ToList();

            foreach (var segment in segmentList)
            {
                AIXM45_RouteSegment aixmSegment = (AIXM45_RouteSegment)segment;

                RouteSegment pdmSegment = new RouteSegment
               {
                   ID = Guid.NewGuid().ToString(),//aixmSegment.R_mid,
                   ID_Route = _Master_ID,
                   ValDistVerUpper = aixmSegment.ValDistVerUpper,
                   ValDistVerLower = aixmSegment.ValDistVerLower,
                   ValDistVerMnm = aixmSegment.ValDistVerMnm,
                   ValLen = aixmSegment.ValLen,
                   ValWidLeft = aixmSegment.ValWid,
                   ValWidRight = aixmSegment.ValWid,
                   ValMagTrack = aixmSegment.ValReversMagTrack,
                   ValTrueTrack = aixmSegment.ValReversTrueTrack,
                   ValReversMagTrack = aixmSegment.ValReversMagTrack,
                   ValReversTrueTrack = aixmSegment.ValReversTrueTrack,
                   StartPoint = DefinePoint(aixmSegment,true),
                   EndPoint = DefinePoint(aixmSegment, false),
               };


                CODE_ROUTE_SEGMENT_DIR _uomDIR;
                //if (((aimObj.Availability != null) && (aimObj.Availability.Count > 0) && (aimObj.Availability[0].Direction.HasValue)))
                //{
                //    Enum.TryParse<CODE_ROUTE_SEGMENT_DIR>(aimObj.Availability[0].Direction.Value.ToString(), out _uomDIR);
                //    pdmObj.CodeDir = _uomDIR;
                //}

                UOM_DIST_VERT _uomVert;
                Enum.TryParse<UOM_DIST_VERT>(aixmSegment.UomDistVerUpper.ToString(), out _uomVert);
                pdmSegment.UomDistVerUpper = _uomVert;

                Enum.TryParse<UOM_DIST_VERT>(aixmSegment.UomDistVerLower.ToString(), out _uomVert);
                pdmSegment.UomDistVerLower = _uomVert;


                UOM_DIST_HORZ _uomHOR;
                Enum.TryParse<UOM_DIST_HORZ>(aixmSegment.UomWid.ToString(), out _uomHOR);
                pdmSegment.UomWid = _uomHOR;

                Enum.TryParse<UOM_DIST_HORZ>(aixmSegment.UomDist.ToString(), out _uomHOR);
                pdmSegment.UomValLen = _uomHOR;

                if (aixmSegment.Geometry == null)
                    pdmSegment.RebuildGeo();
                else
                    pdmSegment.Geo = aixmSegment.Geometry;

                res.Add(pdmSegment);

            }

            return res;
        }

        private RouteSegmentPoint DefinePoint(AIXM45_RouteSegment aixmSegment, bool isItStartPoint)
        {
            RouteSegmentPoint _Point = new RouteSegmentPoint
            {
                ID = Guid.NewGuid().ToString(),
                Route_LEG_ID = aixmSegment.ID,
                PointRole = ProcedureFixRoleType.ENRT,
                PointUse = isItStartPoint?  ProcedureSegmentPointUse.START_POINT : ProcedureSegmentPointUse.END_POINT,
                IsWaypoint = true,
                PointChoice = isItStartPoint && aixmSegment.R_SignificantPointSta_PointChoice == AIXM45_RouteSegment.AIXM45_PointChoice.DesignatedPoint ? PointChoice.DesignatedPoint :
                               isItStartPoint && aixmSegment.R_SignificantPointSta_PointChoice == AIXM45_RouteSegment.AIXM45_PointChoice.Navaid ? PointChoice.Navaid :
                               isItStartPoint && aixmSegment.R_SignificantPointSta_PointChoice == AIXM45_RouteSegment.AIXM45_PointChoice.VHFNavaid ? PointChoice.Navaid :
                               !isItStartPoint && aixmSegment.R_SignificantPointEnd_PointChoice == AIXM45_RouteSegment.AIXM45_PointChoice.DesignatedPoint ? PointChoice.DesignatedPoint :
                               !isItStartPoint && aixmSegment.R_SignificantPointEnd_PointChoice == AIXM45_RouteSegment.AIXM45_PointChoice.Navaid ? PointChoice.Navaid :
                               !isItStartPoint && aixmSegment.R_SignificantPointEnd_PointChoice == AIXM45_RouteSegment.AIXM45_PointChoice.VHFNavaid ? PointChoice.Navaid : PointChoice.OTHER,
                SegmentPointDesignator = isItStartPoint ? aixmSegment.R_SignificantPointStacode_Id : aixmSegment.R_SignificantPointEndcode_Id,
                Lat = isItStartPoint ? aixmSegment.R_SignificantPointSta_LAT : aixmSegment.R_SignificantPointEnd_LAT,
                Lon = isItStartPoint ? aixmSegment.R_SignificantPointSta_LONG : aixmSegment.R_SignificantPointEnd_LONG,
                PointChoiceID = isItStartPoint ? aixmSegment.R_SignificantPointStaMid : aixmSegment.R_SignificantPointEndMid,
                ReportingATC = isItStartPoint && aixmSegment.CodeRepAtcStart.CompareTo("C")==0? CodeATCReporting.COMPULSORY :
                               isItStartPoint && aixmSegment.CodeRepAtcStart.CompareTo("R")==0? CodeATCReporting.ON_REQUEST :
                               isItStartPoint && aixmSegment.CodeRepAtcStart.CompareTo("N")==0? CodeATCReporting.NO_REPORT : 
                               !isItStartPoint && aixmSegment.CodeRepAtcEnd.CompareTo("C")==0? CodeATCReporting.COMPULSORY :
                               !isItStartPoint && aixmSegment.CodeRepAtcEnd.CompareTo("R")==0? CodeATCReporting.ON_REQUEST :
                               !isItStartPoint && aixmSegment.CodeRepAtcEnd.CompareTo("N") == 0 ? CodeATCReporting.NO_REPORT : CodeATCReporting.OTHER,
                PointType = isItStartPoint && aixmSegment.R_SignificantPointSta_PointChoice == AIXM45_RouteSegment.AIXM45_PointChoice.DesignatedPoint ? SegmentPointType.WayPoint :
                               isItStartPoint && aixmSegment.R_SignificantPointSta_PointChoice == AIXM45_RouteSegment.AIXM45_PointChoice.Navaid ? SegmentPointType.NDB :
                               isItStartPoint && aixmSegment.R_SignificantPointSta_PointChoice == AIXM45_RouteSegment.AIXM45_PointChoice.VHFNavaid ? SegmentPointType.NAVAID_VHF :
                               !isItStartPoint && aixmSegment.R_SignificantPointEnd_PointChoice == AIXM45_RouteSegment.AIXM45_PointChoice.DesignatedPoint ? SegmentPointType.WayPoint :
                               !isItStartPoint && aixmSegment.R_SignificantPointEnd_PointChoice == AIXM45_RouteSegment.AIXM45_PointChoice.Navaid ? SegmentPointType.NDB :
                               !isItStartPoint && aixmSegment.R_SignificantPointEnd_PointChoice == AIXM45_RouteSegment.AIXM45_PointChoice.VHFNavaid ? SegmentPointType.NAVAID_VHF : SegmentPointType.OTHER

                
            };

            if (isItStartPoint)
                _Point.IsWaypoint = aixmSegment.R_SignificantPointSta_PointChoice == AIXM45_RouteSegment.AIXM45_PointChoice.DesignatedPoint;
            else
                _Point.IsWaypoint = aixmSegment.R_SignificantPointEnd_PointChoice == AIXM45_RouteSegment.AIXM45_PointChoice.DesignatedPoint;


            return _Point;

        }

        private WayPoint GetWaypoint(AIXM45_WayPoint aIXM45_WayPoint)
        {

            WayPoint pdmDPN = new WayPoint
            {
                ID = Guid.NewGuid().ToString(),//aIXM45_WayPoint.R_mid,
                Lat = aIXM45_WayPoint.R_geoLat,
                Lon = aIXM45_WayPoint.R_geoLong,
                Designator = aIXM45_WayPoint.R_codeId,
                Function = aIXM45_WayPoint.CodeType,
                Name = aIXM45_WayPoint.TxtName,
                ID_AirportHeliport = aIXM45_WayPoint.R_Ahpmid,
            };


            if (aIXM45_WayPoint.Geometry == null)
                pdmDPN.RebuildGeo();
            else
                pdmDPN.Geo = aIXM45_WayPoint.Geometry;

            return pdmDPN;
        }

        private bool CheckLocation(NavaidSystem navSystemPdm, AreaInfo Area)
        {
            bool res = false;
            foreach (var dmePdm in navSystemPdm.Components)
            {
                if ((dmePdm != null) && (dmePdm.Geo != null) && (util.WithinPolygon(Area.AreaPolygon, dmePdm.Geo as IPoint)))
                {
                    res = true;
                    break;
                }
            }

            return res;
        }

        private bool _CheckLocation(Enroute Enrt, AreaInfo Area)
        {
            bool res = false;
            foreach (RouteSegment seg in Enrt.Routes)
            {
                if ((seg != null) && (seg.Geo != null) && (util.WithinPolygon(Area.AreaPolygon, seg.Geo as ILine)))
                {
                    res = true;
                    break;
                }
            }

            return res;
        }

        private bool CheckLocation(Enroute Enrt, AreaInfo Area)
        {
            bool res = false;
            List<RouteSegment> checkedSegments = new List<RouteSegment>();

            foreach (RouteSegment seg in Enrt.Routes)
            {

                if ((seg.StartPoint != null) && (seg.StartPoint.Geo != null) && (seg.EndPoint != null) && (seg.EndPoint.Geo != null))
                {
                    if ((util.WithinPolygon(Area.AreaPolygon, (IPoint)seg.StartPoint.Geo)) || (util.WithinPolygon(Area.AreaPolygon, (IPoint)seg.EndPoint.Geo)))
                    {
                        checkedSegments.Add(seg);
                    }
                }

            }

            Enrt.Routes.Clear();
            Enrt.Routes.AddRange(checkedSegments);

            return res = Enrt.Routes.Count >0;
        }

        private bool CheckLocation(Airspace Arsps, AreaInfo Area)
        {
            return true;
        }

        private NavaidSystem GetNavaidsSystem(AIXM45_DME aIXM45_DME)
        {


            NavaidSystem navSystem = new NavaidSystem
            {
                ID = Guid.NewGuid().ToString(),
                ID_RunwayDirection = aIXM45_DME.R_RdnMid,
                ID_AirportHeliport = aIXM45_DME.R_AhpMid,
                CodeNavaidSystemType = NavaidSystemType.DME,
                Designator = aIXM45_DME.R_codeId,
                Components = new List<PDMObject>(),
                Name = aIXM45_DME.TxtName,
            };


            #region DME

               navSystem.Components.Add(GetDme(aIXM45_DME,navSystem.ID));


            #endregion

            #region VOR

            if (aIXM45_DME.R_VorMid.Trim().Length > 0)
            {
                AIXM45_Object vor = (from element in aixm45List where (element != null) && (element is AIXM45_VOR) && (((AIXM45_VOR)element).R_mid.CompareTo(aIXM45_DME.R_VorMid.Trim()) == 0) select element).FirstOrDefault();

                AIXM45_VOR vorAIXM45 = (AIXM45_VOR)vor;
                if (vorAIXM45 != null)
                {
                    navSystem.CodeNavaidSystemType = NavaidSystemType.VOR_DME;

                    navSystem.Components.Add(GetVor(vorAIXM45, navSystem.ID ));
                }
            }

            

            #endregion

            FilldecodedNavaidsList(navSystem);
            return navSystem;
        }

        private NavaidSystem GetNavaidsSystem(AIXM45_VOR aIXM45_VOR)
        {
            NavaidSystem navSystem = new NavaidSystem
            {
                ID = Guid.NewGuid().ToString(),
                ID_RunwayDirection = aIXM45_VOR.R_RdnMid,
                ID_AirportHeliport = aIXM45_VOR.R_AhpMid,
                CodeNavaidSystemType = NavaidSystemType.VOR,
                Designator = aIXM45_VOR.R_codeId,
                Components = new List<PDMObject>(),
                Name = aIXM45_VOR.TxtName,
            };



            navSystem.Components.Add(GetVor(aIXM45_VOR, navSystem.ID));

            FilldecodedNavaidsList(navSystem);
            return navSystem;
        }

        private List<NavaidSystem> GetNavaidSystem(string rdn_MID)
        {
            var ilsList = (from element in aixm45List where (element != null) && (element is AIXM45_ILS) && (((AIXM45_ILS)element).R_RdnMid.CompareTo(rdn_MID) == 0) select element).ToList();
            List<NavaidSystem> res = new List<NavaidSystem>();
            foreach (var ils in ilsList)
            {
                AIXM45_ILS ilsAIXM45 = (AIXM45_ILS)ils;
                string masterID = Guid.NewGuid().ToString();
                NavaidSystem navSystem = new NavaidSystem
                {
                    ID = masterID, //ilsAIXM45.R_mid,
                    ID_RunwayDirection = ilsAIXM45.R_RdnMid,
                    ID_AirportHeliport = ilsAIXM45.R_AhpMid,
                    CodeNavaidSystemType = NavaidSystemType.ILS,
                    Designator = ilsAIXM45.ILZ.CodeId, 
                    Components = new List<PDMObject>(),
                    
                };

                #region GlidePath

                GlidePath gp = new GlidePath
                {
                    ID = Guid.NewGuid().ToString(),
                    ID_NavaidSystem = masterID,//ilsAIXM45.R_mid,
                    Angle = ilsAIXM45.IGP.ValSlope,
                    Lat = ilsAIXM45.IGP.GeoLat,
                    Lon = ilsAIXM45.IGP.GeoLong,
                    Elev = Math.Round(util.ConvertValueToMeter(ilsAIXM45.IGP.ValElev.ToString(), ilsAIXM45.IGP.UomDistVer), 2),
                    Elev_UOM = UOM_DIST_VERT.M,
                    ThresholdCrossingHeight = Math.Round(util.ConvertValueToMeter(ilsAIXM45.IGP.ValRdh.ToString(), ilsAIXM45.IGP.UomRdh), 2),
                    Freq = ilsAIXM45.IGP.ValFreq,

                };


                UOM_FREQ _uomFreq = UOM_FREQ.KHZ;
                if (ilsAIXM45.IGP.UomFreq != null) Enum.TryParse<UOM_FREQ>(ilsAIXM45.IGP.UomFreq.ToString(), out _uomFreq); gp.UomFreq = _uomFreq;

                if (ilsAIXM45.IGP.Geometry == null)
                    gp.RebuildGeo();
                else
                    gp.Geo = ilsAIXM45.IGP.Geometry;

                navSystem.Components.Add(gp);

                #endregion

                #region Localizer

                Localizer lz = new Localizer
                {
                    ID = Guid.NewGuid().ToString(),
                    ID_NavaidSystem = masterID,//ilsAIXM45.R_mid,
                    Width = ilsAIXM45.ILZ.ValWidCourse,
                    Elev = Math.Round(util.ConvertValueToMeter(ilsAIXM45.ILZ.ValElev.ToString(), ilsAIXM45.ILZ.UomDistVer), 2),
                    Elev_UOM = UOM_DIST_VERT.M,
                    Frequency = ilsAIXM45.ILZ.ValFreq,
                    MagBrg = ilsAIXM45.ILZ.ValMagBrg,
                    trueBearing = ilsAIXM45.ILZ.ValTrueBrg,
                    Lat = ilsAIXM45.ILZ.GeoLat,
                    Lon = ilsAIXM45.ILZ.GeoLong,


                };

                _uomFreq = UOM_FREQ.KHZ;
                if (ilsAIXM45.ILZ.UomFreq != null) Enum.TryParse<UOM_FREQ>(ilsAIXM45.ILZ.UomFreq.ToString(), out _uomFreq); lz.Frequency_UOM = _uomFreq;

                if (ilsAIXM45.ILZ.Geometry == null)
                    lz.RebuildGeo();
                else 
                    lz.Geo = ilsAIXM45.ILZ.Geometry;

                navSystem.Components.Add(lz);

                #endregion

                #region DME

                if (ilsAIXM45.R_DmeMid.Trim().Length > 0)
                {
                    AIXM45_Object dme = (from element in aixm45List where (element != null) && (element is AIXM45_DME) && (((AIXM45_DME)element).R_mid.CompareTo(ilsAIXM45.R_DmeMid.Trim()) == 0) select element).FirstOrDefault();

                    AIXM45_DME dmeAIXM45 = (AIXM45_DME)dme;
                    if (dmeAIXM45 != null)
                    {
                        navSystem.CodeNavaidSystemType = NavaidSystemType.ILS_DME;
                        navSystem.Components.Add(GetDme(dmeAIXM45, masterID));
                    }

                }

                #endregion

                #region Marker

                //List<AIXM45_Object> aIXM45_List = (from element in aixm45List where (element != null) && (element is AIXM45_Marker) && (((AIXM45_Marker)element).R_ILSMID.CompareTo(ilsAIXM45.R_mid.Trim()) == 0) select element).ToList();
                //if ((aIXM45_List != null) && (aIXM45_List.Count > 0))
                //{
                //    foreach (var item in aIXM45_List)
                //    {
                //        navSystem.Components.Add(GetMarker((AIXM45_Marker)item, navSystem.ID));
                //    }

                //    navSystem.CodeNavaidSystemType = NavaidSystemType.LOC;
                //}

                

                #endregion

                res.Add(navSystem);

                FilldecodedNavaidsList(navSystem);


            }

            return res;
        }

        private NavaidSystem GetNavaidsSystem(AIXM45_NDB aIXM45_NDB)
        {
            NavaidSystem navSystem = new NavaidSystem
            {
                ID = Guid.NewGuid().ToString(),
                ID_RunwayDirection = aIXM45_NDB.R_RdnMid,
                ID_AirportHeliport = aIXM45_NDB.R_AhpMid,
                CodeNavaidSystemType = NavaidSystemType.NDB,
                Designator = aIXM45_NDB.R_codeId,
                Components = new List<PDMObject>(),
                Name = aIXM45_NDB.TxtName,
            };



            navSystem.Components.Add(GetNDB(aIXM45_NDB, navSystem.ID));

            List<AIXM45_Object> aIXM45_List = (from element in aixm45List where (element != null) && (element is AIXM45_Marker) && (((AIXM45_Marker)element).R_NDBMID.CompareTo(aIXM45_NDB.R_mid.Trim()) == 0) select element).ToList();
            if ((aIXM45_List != null) && (aIXM45_List.Count > 0))
            {
                foreach (var item in aIXM45_List)
                {
                    navSystem.Components.Add(GetMarker((AIXM45_Marker)item, navSystem.ID));
                }

                navSystem.CodeNavaidSystemType = NavaidSystemType.NDB_MKR;
            }

            FilldecodedNavaidsList(navSystem);
            return navSystem;
        }

        private NavaidSystem GetNavaidsSystem(AIXM45_Marker aIXM45_MKR)
        {
            NavaidSystem navSystem = new NavaidSystem
            {
                ID = Guid.NewGuid().ToString(),
                ID_RunwayDirection = aIXM45_MKR.R_RdnMid,
                ID_AirportHeliport = aIXM45_MKR.R_AhpMid,
                CodeNavaidSystemType = NavaidSystemType.MKR,
                Designator = aIXM45_MKR.R_codeId,
                Components = new List<PDMObject>(),
                Name = aIXM45_MKR.TxtName,
            };



            navSystem.Components.Add(GetMarker(aIXM45_MKR, navSystem.ID));

            FilldecodedNavaidsList(navSystem);
            return navSystem;
        }

        private PDMObject GetMarker(AIXM45_Marker aIXM45_MKR, string _ID_NavaidSystem)
        {
            Marker pdmMarker = new Marker
            {
                ID = aIXM45_MKR.R_mid,
                ID_NavaidSystem = _ID_NavaidSystem,
                CodeId = aIXM45_MKR.R_codeId,
                Lat = aIXM45_MKR.R_geoLat,
                Lon = aIXM45_MKR.R_geoLong,
                TxtName = aIXM45_MKR.TxtName,
                Frequency = aIXM45_MKR.ValFreq,
                Axis_Bearing = aIXM45_MKR.ValAxisBrg,
            };

            UOM_FREQ _uom;
            Enum.TryParse<UOM_FREQ>(pdmMarker.Frequency_UOM.ToString(), out _uom); pdmMarker.Frequency_UOM = _uom;


            if (aIXM45_MKR.Geometry == null)
                pdmMarker.RebuildGeo();
            else
                pdmMarker.Geo = aIXM45_MKR.Geometry;

            return pdmMarker;
        }

        private PDMObject GetNDB(AIXM45_NDB aIXM45_NDB, string _ID_NavaidSystem)
        {
            NDB pdmNDB = new NDB
            {
                ID = aIXM45_NDB.R_mid,
                ID_NavaidSystem = _ID_NavaidSystem,
                CodeId = aIXM45_NDB.R_codeId,
                Lat = aIXM45_NDB.R_geoLat,
                Lon = aIXM45_NDB.R_geoLong,
                TxtName = aIXM45_NDB.TxtName,
                Frequency = aIXM45_NDB.ValFreq
            };

            UOM_FREQ _uom;
            Enum.TryParse<UOM_FREQ>(pdmNDB.Frequency_UOM.ToString(), out _uom); pdmNDB.Frequency_UOM = _uom;


            if (aIXM45_NDB.Geometry == null)
                pdmNDB.RebuildGeo();
            else
                pdmNDB.Geo = aIXM45_NDB.Geometry;

            return pdmNDB;
        }

        private PDMObject GetVor(AIXM45_VOR vorAIXM45, string _ID_NavaidSystem)
        {

            VOR pdmVOR = new VOR
            {
                ID = Guid.NewGuid().ToString(), ////vorAIXM45.R_mid,
                ID_NavaidSystem = _ID_NavaidSystem,
                VorIdentifier = vorAIXM45.R_codeId,
                Elev = Math.Round(util.ConvertValueToMeter(vorAIXM45.ValElev.ToString(), vorAIXM45.UomDistVer), 2),
                Elev_UOM = UOM_DIST_VERT.M,
                Lat = vorAIXM45.R_geoLat,
                Lon = vorAIXM45.R_geoLong,
                CodeId = vorAIXM45.R_codeId,
                
            };

            if (vorAIXM45.Geometry == null)
                pdmVOR.RebuildGeo();
            else
                pdmVOR.Geo = vorAIXM45.Geometry;

            return pdmVOR;
        }

        private string GetPathToAreaFile()
        {

            const string userRoot = "HKEY_CURRENT_USER";
            const string subkey = "Software\\RISK\\ARENA\\ARINC";
            const string keyName = userRoot + "\\" + subkey;
            string Res = (string)Registry.GetValue(keyName, "AreaFile", "Area not exist");

            return Res;

        }

        private AirportHeliport GetAirport(AIXM45_Airport ahpAIXM45)
        {
            AirportHeliport ahpPDM = new AirportHeliport
            {
                #region

                Designator = ahpAIXM45.CodeIcao,
                DesignatorIATA = ahpAIXM45.CodeIata,
                Elev = ahpAIXM45.ValElev,
                ID = Guid.NewGuid().ToString(), //ahpAIXM45.R_MID,
                MagneticVariation = ahpAIXM45.ValMagVar,
                Name = ahpAIXM45.TxtName,
                TransitionAltitude = ahpAIXM45.ValTransitionAlt,
                Lat = ahpAIXM45.GeoLat,
                Lon = ahpAIXM45.GeoLong,
                RunwayList = GetRwyList(ahpAIXM45.R_MID),

                #endregion
            };


            UOM_DIST_VERT uom_dist_vert = UOM_DIST_VERT.M;
            if (ahpAIXM45.UomDistVer != null) Enum.TryParse<UOM_DIST_VERT>(ahpAIXM45.UomDistVer.ToString(), out uom_dist_vert); ahpPDM.Elev_UOM = uom_dist_vert;
            if (ahpAIXM45.UomTransitionAlt != null) Enum.TryParse<UOM_DIST_VERT>(ahpAIXM45.UomTransitionAlt.ToString(), out uom_dist_vert); ahpPDM.TransitionAltitudeUOM = uom_dist_vert;

            if (ahpAIXM45.Geometry == null)
                ahpPDM.RebuildGeo();
            else
                ahpPDM.Geo = ahpAIXM45.Geometry;


                return ahpPDM;
        }

        private List<Runway> GetRwyList(string ahpPDM_ID)
        {
            List<Runway> result = new List<Runway>();

            var rwyList = (from element in aixm45List where (element != null) && (element is AIXM45_RWY) && (((AIXM45_RWY)element).R_AHPmid.CompareTo(ahpPDM_ID) == 0) select element).ToList();


            foreach (var rwy in rwyList)
            {

                AIXM45_RWY rwyAIXM45 = (AIXM45_RWY)rwy;
                Runway rwyPDM = new Runway
                {
                    #region

                    ID = Guid.NewGuid().ToString(), //rwyAIXM45.R_mid,
                    Designator = rwyAIXM45.R_txtDesig,
                    Length = Math.Round(util.ConvertValueToMeter(rwyAIXM45.ValLen.ToString(), rwyAIXM45.UomDimRwy), 2),
                    Width = Math.Round(util.ConvertValueToMeter(rwyAIXM45.ValWid.ToString(), rwyAIXM45.UomDimRwy), 2),
                    Uom = UOM_DIST_HORZ.M,
                    RunwayDirectionList = GetRdnList(rwyAIXM45.R_mid),

                    #endregion
                };

                foreach (var item in rwyPDM.RunwayDirectionList)
                {
                    if ((item.CenterLinePoints == null) || (item.CenterLinePoints.Count == 0))
                    {
                        #region

                        RunwayCenterLinePoint clpEND1 = new RunwayCenterLinePoint();
                        RunwayCenterLinePoint clpEND2 = new RunwayCenterLinePoint();
                        RunwayCenterLinePoint clpStart1 = new RunwayCenterLinePoint();
                        RunwayCenterLinePoint clpTHR1 = new RunwayCenterLinePoint();
                        RunwayCenterLinePoint clpSTART2 = new RunwayCenterLinePoint();
                        RunwayCenterLinePoint clpTHR2 = new RunwayCenterLinePoint();

                        RunwayDirection rdn1 = rwyPDM.RunwayDirectionList[0];
                        if (rdn1 != null)
                        {
                            if (rdn1.Geo == null) rdn1.RebuildGeo();

                            if (rdn1.CenterLinePoints == null) rdn1.CenterLinePoints = new List<RunwayCenterLinePoint>();
                            clpStart1 = new RunwayCenterLinePoint
                            {
                                ID = Guid.NewGuid().ToString(),
                                Designator = rdn1.Designator,
                                Role = CodeRunwayCenterLinePointRoleType.START,
                                Lat = rdn1.Lat,
                                Lon = rdn1.Lon,
                                Elev = rdn1.Elev,
                                Elev_UOM = rdn1.Elev_UOM,
                                Geo = rdn1.Geo,
                                ID_RunwayDirection = rdn1.ID,
                                VisibilityFlag = true
                            };
                            if (clpStart1 != null) rdn1.CenterLinePoints.Add(clpStart1);


                            clpTHR1 = new RunwayCenterLinePoint
                            {
                                ID = Guid.NewGuid().ToString(),
                                Designator = rdn1.Designator,
                                Role = CodeRunwayCenterLinePointRoleType.THR,
                                Lat = rdn1.Lat,
                                Lon = rdn1.Lon,
                                Elev = rdn1.Elev,
                                Elev_UOM = rdn1.Elev_UOM,
                                Geo = rdn1.Geo,
                                ID_RunwayDirection = rdn1.ID,
                                VisibilityFlag = true
                            };
                            if (clpTHR1 != null) rdn1.CenterLinePoints.Add(clpTHR1);

                            clpEND1 = new RunwayCenterLinePoint
                            {
                                ID = Guid.NewGuid().ToString(),
                                Designator = rdn1.Designator,
                                Role = CodeRunwayCenterLinePointRoleType.END,
                                Lat = rdn1.Lat,
                                Lon = rdn1.Lon,
                                Elev = rdn1.Elev,
                                Elev_UOM = rdn1.Elev_UOM,
                                Geo = rdn1.Geo,
                                ID_RunwayDirection = rdn1.ID,
                                VisibilityFlag = true
                            };


                        }



                        RunwayDirection rdn2 = rwyPDM.RunwayDirectionList.Count > 1 ? rwyPDM.RunwayDirectionList[1] : null;
                        if (rdn2 != null)
                        {

                            if (rdn2.Geo == null) rdn2.RebuildGeo();

                            if (rdn2.CenterLinePoints == null) rdn2.CenterLinePoints = new List<RunwayCenterLinePoint>();
                            clpSTART2 = new RunwayCenterLinePoint
                            {
                                ID = Guid.NewGuid().ToString(),
                                Designator = rdn2.Designator,
                                Role = CodeRunwayCenterLinePointRoleType.START,
                                Lat = rdn2.Lat,
                                Lon = rdn2.Lon,
                                Elev = rdn2.Elev,
                                Elev_UOM = rdn2.Elev_UOM,
                                Geo = rdn2.Geo,
                                ID_RunwayDirection = rdn2.ID,
                                VisibilityFlag = true
                            };
                            if (clpSTART2 != null) rdn2.CenterLinePoints.Add(clpSTART2);

                            clpTHR2 = new RunwayCenterLinePoint
                            {
                                ID = Guid.NewGuid().ToString(),
                                Designator = rdn2.Designator,
                                Role = CodeRunwayCenterLinePointRoleType.THR,
                                Lat = rdn2.Lat,
                                Lon = rdn2.Lon,
                                Elev = rdn2.Elev,
                                Elev_UOM = rdn2.Elev_UOM,
                                Geo = rdn2.Geo,
                                ID_RunwayDirection = rdn2.ID,
                                VisibilityFlag = true
                            };
                            rdn2.CenterLinePoints.Add(clpTHR2);

                            clpEND2 = new RunwayCenterLinePoint
                            {
                                ID = Guid.NewGuid().ToString(),
                                Designator = rdn2.Designator,
                                Role = CodeRunwayCenterLinePointRoleType.END,
                                Lat = rdn2.Lat,
                                Lon = rdn2.Lon,
                                Elev = rdn2.Elev,
                                Elev_UOM = rdn2.Elev_UOM,
                                Geo = rdn2.Geo,
                                ID_RunwayDirection = rdn2.ID,
                                VisibilityFlag = true
                            };
                        }

                        if ((rdn1 != null) && (rdn2 != null))
                        {
                            rdn1.CenterLinePoints.Add(rdn2.CenterLinePoints[1]);
                            rdn1.CenterLinePoints.Add(clpEND2);

                            rdn2.CenterLinePoints.Add(rdn1.CenterLinePoints[1]);
                            rdn2.CenterLinePoints.Add(clpEND1);
                        }

                        #endregion
                    }
                }
                

               
                result.Add(rwyPDM);
            }

            return result;
        }

        private List<RunwayDirection> GetRdnList(string rwyPDM_ID)
        {
            List<RunwayDirection> result = new List<RunwayDirection>();

            var rdnList = (from element in aixm45List where (element != null) && (element is AIXM45_RDN) && (((AIXM45_RDN)element).R_RWYmid.CompareTo(rwyPDM_ID) == 0) select element).ToList();

            foreach (var rdn in rdnList)
            {
                AIXM45_RDN rdnAIXM45 = (AIXM45_RDN)rdn;
                RunwayDirection rdnPDM = new RunwayDirection
                {
                    #region
                    ID = Guid.NewGuid().ToString(), //rdnAIXM45.R_mid,
                    Designator = rdnAIXM45.R_txtDesig,
                    Elev = rdnAIXM45.ValElevTdz,
                    LandingThresholdElevation =Math.Round( util.ConvertValueToMeter( rdnAIXM45.R_RdnElev.ToString(),rdnAIXM45.R_RdnElevUom),2),
                    Elev_UOM = UOM_DIST_VERT.M,
                    Lat = rdnAIXM45.GeoLat,
                    Lon = rdnAIXM45.GeoLong,
                    MagBearing = rdnAIXM45.ValMagBrg,
                    TrueBearing = rdnAIXM45.ValTrueBrg,
                    RdnDeclaredDistance = GetDeclaredDistance(rdnAIXM45.R_mid),
                    Stopway = GetStopway(rdnAIXM45.R_mid),
                    Uom = UOM_DIST_HORZ.M,
                    Related_NavaidSystem = GetNavaidSystem(rdnAIXM45.R_mid),
                    ClearWay = GetClearway(rdnAIXM45.R_mid),
                    #endregion
                };


                UOM_DIST_VERT uom_dist_vert = UOM_DIST_VERT.M;
                if (rdnAIXM45.R_RdnElevUom != null) Enum.TryParse<UOM_DIST_VERT>(rdnAIXM45.R_RdnElevUom.ToString(), out uom_dist_vert); rdnPDM.Elev_UOM = uom_dist_vert;

                if (rdnAIXM45.Geometry == null)
                    rdnPDM.RebuildGeo();
                else
                    rdnPDM.Geo = rdnAIXM45.Geometry;

                rdnPDM.CenterLinePoints = GetCenterLinePounts(rdnAIXM45.R_RWYmid,rdnAIXM45.R_mid, rdnPDM.Geo);


                result.Add(rdnPDM);
            }


        


            return result;
        }

        private PDMObject GetDme(AIXM45_DME dmeAIXM45, string _ID_NavaidSystem)
        {
            DME pdmDME = new DME
            {
                ID = Guid.NewGuid().ToString(), //dmeAIXM45.R_mid,
                ID_NavaidSystem =_ID_NavaidSystem,
                DmeIdentifier = dmeAIXM45.R_codeId,
                Elev = Math.Round(util.ConvertValueToMeter(dmeAIXM45.ValElev.ToString(), dmeAIXM45.UomDistVer), 2),
                Elev_UOM = UOM_DIST_VERT.M,
                Lat = dmeAIXM45.R_geoLat,
                Lon = dmeAIXM45.R_geoLong,
                CodeId = dmeAIXM45.R_codeId,
            };

            pdmDME.RebuildGeo();

            return pdmDME;
        }

        private void FilldecodedNavaidsList(NavaidSystem navSystem)
        {
            foreach (var item in navSystem.Components)
            {
                NavaidsList.Add(item.ID);
            }
        }

        private List<RunwayCenterLinePoint> GetCenterLinePounts(string rwyPDM_ID,string rdnPDM_ID, IGeometry RdnGeo)
        {
            List<RunwayCenterLinePoint> res = new List<RunwayCenterLinePoint>();
            List<AIXM45_Object> rcpAIXM45 = (from element in aixm45List where (element != null) && (element is AIXM45_RCP) && (((AIXM45_RCP)element).R_RWYMID.CompareTo(rwyPDM_ID) == 0) select element).ToList();

            if (rcpAIXM45 != null)
            {
                List<RunwayCenterLinePoint> temporarityList = new List<RunwayCenterLinePoint>();

                foreach (var rcp in rcpAIXM45)
                {
                    AIXM45_RCP aixm45rcp = (AIXM45_RCP)rcp;
                    RunwayCenterLinePoint pdmRCP = new RunwayCenterLinePoint
                    {
                        ID = Guid.NewGuid().ToString(),
                        Designator = "Clp",
                        Elev = Math.Round( util.ConvertValueToMeter( aixm45rcp.valElev.ToString(),aixm45rcp.uomDistVer),2),
                        Elev_UOM = UOM_DIST_VERT.M,
                        Lat = aixm45rcp.R_geoLat,
                        Lon = aixm45rcp.R_geoLong,
                        Role = CodeRunwayCenterLinePointRoleType.MID,
                        DeclDist = new List<DeclaredDistance>(),
                        ID_RunwayDirection = rdnPDM_ID,
                        
                    };

                    if (aixm45rcp.Geometry == null)
                        pdmRCP.RebuildGeo();
                    else
                        pdmRCP.Geo = aixm45rcp.Geometry;

                    ILine ln = new LineClass();
                    ln.FromPoint = (IPoint)RdnGeo;
                    ln.ToPoint = (IPoint)pdmRCP.Geo;

                    DeclaredDistance temp = new DeclaredDistance { DistanceValue = ln.Length };
                    pdmRCP.DeclDist.Add(temp);
                    temporarityList.Add(pdmRCP);
                }

                var pointsByDist = from element in temporarityList where element.DeclDist!=null
                              orderby element.DeclDist[0].DistanceValue
                              select element;

                int stepCounter = 0;
                int pointsCount = pointsByDist.Count();
                foreach (var item in pointsByDist)
                {
                    item.Designator = "clp" + (stepCounter+1);

                    if (stepCounter == 0)
                        item.Role = CodeRunwayCenterLinePointRoleType.START;

                    if (stepCounter == (pointsCount-1))
                        item.Role = CodeRunwayCenterLinePointRoleType.END;

                    item.DeclDist = null;
                    res.Add(item);
                    stepCounter++;
                }



            }



            return res;
        }

        private List<DeclaredDistance> GetDeclaredDistance(string rdnPDM_ID)
        {
            List<DeclaredDistance> res = null;

            List<AIXM45_Object> rddAIXM45 = (from element in aixm45List where (element != null) && (element is AIXM45_RDN_DECLARED_DISTANCE) && (((AIXM45_RDN_DECLARED_DISTANCE)element).R_RdnMid.CompareTo(rdnPDM_ID) == 0) select element).ToList();

            if (rddAIXM45 != null)
            {
                res = new List<DeclaredDistance>();

                foreach (var rdd in rddAIXM45)
                {
                    AIXM45_RDN_DECLARED_DISTANCE decldistAIXM45 = (AIXM45_RDN_DECLARED_DISTANCE)rdd;
                    DeclaredDistance declDistPDM = new DeclaredDistance { DistanceValue = decldistAIXM45.valDist };
                    UOM_DIST_HORZ _uomDist;
                    if (Enum.TryParse<UOM_DIST_HORZ>(decldistAIXM45.uomDist.ToString(), out _uomDist)) declDistPDM.DistanceUOM = _uomDist;
                    CodeDeclaredDistance _uomCode;
                    if (Enum.TryParse<CodeDeclaredDistance>(decldistAIXM45.R_codeType.ToString(), out _uomCode)) declDistPDM.DistanceType = _uomCode;

                    res.Add(declDistPDM);
                }
            }



            return res;
        }

        private double GetStopway(string rdnPDM_ID)
        {
            double resM =0;
            
            AIXM45_Object swyAIXM45 = (from element in aixm45List where (element != null) && (element is AIXM45_RDN_SWY) && (((AIXM45_RDN_SWY)element).R_RdnMid.CompareTo(rdnPDM_ID) == 0) select element).FirstOrDefault();

            if (swyAIXM45 !=null)
            {
                resM = Math.Round(util.ConvertValueToMeter(((AIXM45_RDN_SWY)swyAIXM45).valLen.ToString(), ((AIXM45_RDN_SWY)swyAIXM45).uomDim), 2);
            }


            return resM;


        }


        private double GetClearway(string rdnPDM_ID)
        {
            double resM = 0;

            AIXM45_Object cwyAIXM45 = (from element in aixm45List where (element != null) && (element is AIXM45_RPA) && (((AIXM45_RPA)element).R_RdnMid.CompareTo(rdnPDM_ID) == 0) select element).FirstOrDefault();

            if (cwyAIXM45 != null)
            {
                resM = Math.Round(util.ConvertValueToMeter(((AIXM45_RPA)cwyAIXM45).valLen.ToString(), ((AIXM45_RPA)cwyAIXM45).UomDim), 2);
            }


            return resM;


        }
       
    }
}
