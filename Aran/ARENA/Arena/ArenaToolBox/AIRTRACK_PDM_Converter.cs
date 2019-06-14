using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_DECODER_CORE.AIRTRACK_Objects;
//using ARINC_DECODER_CORE.AIRTRACK_Objects;
using ARINC_Types;
using PDM;
using ESRI.ArcGIS.Geometry;

namespace ARENA
{
    public class AIRTRACK_PDM_Converter
    {

        private static Dictionary<Type, Type> elementTypes = new Dictionary<Type, Type>();

        static AIRTRACK_PDM_Converter()
        {
            elementTypes.Add(typeof(AIRPORT_AIRTRACK), typeof(AIRPORT_PDM_Converter));
            elementTypes.Add(typeof(NDB_AIRTRACK), typeof(Navaid_PDM_Converter));
            elementTypes.Add(typeof(VOR_AIRTRACK), typeof(Navaid_PDM_Converter));
            elementTypes.Add(typeof(DME_AIRTRACK), typeof(Navaid_PDM_Converter));
            elementTypes.Add(typeof(TACAN_AIRTRACK), typeof(Navaid_PDM_Converter));
            elementTypes.Add(typeof(ILS_AIRTRACK), typeof(Navaid_PDM_Converter));
            elementTypes.Add(typeof(WayPoint_AIRTRACK), typeof(WayPoint_PDM_Converter));
            //elementTypes.Add(typeof(Procedure_AIRTRACK), typeof(Procedure_PDM_Converter));
            elementTypes.Add(typeof(AREA_AIRTRACK), typeof(AREA_PDM_Converter));
            elementTypes.Add(typeof(ROUTE_AIRTRACK), typeof(Route_PDM_Converter));
            elementTypes.Add(typeof(Airspace_AIRTRACK), typeof(Airspace_PDM_Converter));
            elementTypes.Add(typeof(Obstacle_AIRTRACK), typeof(Obstacle_PDM_Converter));

        }

        public static PDMObject AIRTRAC_Object_Convert(Object_AIRTRACK elem)
        {
            if (elementTypes.ContainsKey(elem.GetType()))
            {

                Type t = elementTypes[elem.GetType()];
                IAIRTRACK_PDM_Converter result = (IAIRTRACK_PDM_Converter)Activator.CreateInstance(t);
                PDMObject obj = result.Convert_AIRTRAC_Object(elem);
                return obj;

                
            }
            else
                return null;
        }
    }

    public interface IAIRTRACK_PDM_Converter
    {
        PDMObject Convert_AIRTRAC_Object(Object_AIRTRACK elem);
    }

    public class AIRPORT_PDM_Converter : IAIRTRACK_PDM_Converter
    {
        public PDMObject Convert_AIRTRAC_Object(Object_AIRTRACK elem)
        {
            AirportHeliport _Object = new AirportHeliport();

            AIRPORT_AIRTRACK AirtrackARP = (AIRPORT_AIRTRACK)elem;
            ARINC_Airport_Primary_Record ArincArp = (ARINC_Airport_Primary_Record)AirtrackARP.ARINC_OBJ;

            _Object.ID = elem.ID_AIRTRACK;

            _Object.Designator = ArincArp.Airport_ICAO_Identifier.Trim();
            _Object.DesignatorIATA = ArincArp.ATA_IATA_Designator.Trim();
            _Object.Elev = AirtrackARP.ValElev_m;
            _Object.Elev_UOM = UOM_DIST_VERT.M;
            _Object.Lat = ArincArp.Airport_Reference_Pt_Latitude.Trim();
            _Object.Lon = ArincArp.Airport_Reference_Pt_Longitude.Trim();
            _Object.MagneticVariation = AirtrackARP.Magnetic_Variation;
            _Object.Name = ArincArp.Airport_Name.Trim();
            

            if ((AirtrackARP.LinkedRWY != null) && (AirtrackARP.LinkedRWY.Count > 0))
            {
                _Object.RunwayList = new List<Runway>();

                foreach (RunWay_AIRTRACK Airtrack_rwy in AirtrackARP.LinkedRWY)
                {
                    Runway PDM_Airtrack = GetRwy(Airtrack_rwy);
                    if (PDM_Airtrack != null) _Object.RunwayList.Add(PDM_Airtrack);
                }
            }

            if (AirtrackARP.Shape != null) _Object.Geo = AirtrackARP.Shape.Geometry;

            return _Object;
        }

        public Runway GetRwy(Object_AIRTRACK objAirtrack)
        {

            Runway rwy = new Runway(); 

            RunwayCenterLinePoint clpEND1 = new RunwayCenterLinePoint();
            RunwayCenterLinePoint clpEND2 = new RunwayCenterLinePoint();
            RunwayCenterLinePoint clpStart1 = new RunwayCenterLinePoint();
            RunwayCenterLinePoint clpTHR1 = new RunwayCenterLinePoint();
            RunwayCenterLinePoint clpSTART2 = new RunwayCenterLinePoint();
            RunwayCenterLinePoint clpTHR2 = new RunwayCenterLinePoint();


            rwy.ID = Guid.NewGuid().ToString();//objAirtrack.ID_AIRTRACK;

            RunWay_AIRTRACK AirtrackRWY = (RunWay_AIRTRACK)objAirtrack;
            rwy.Designator = AirtrackRWY.RWY_Designator;
            rwy.Length = AirtrackRWY.RWY_Length_M;
            rwy.Width = AirtrackRWY.RWY_Width_M;
            rwy.Uom = UOM_DIST_HORZ.FT;

            if (AirtrackRWY.LinkedTHR != null)
            {
                rwy.RunwayDirectionList = new List<RunwayDirection>();

                foreach (RunWay_THR_AIRTRACK THR_AIRTRACK in AirtrackRWY.LinkedTHR)
                {
                    RunwayDirection PDM_RunwayDirection = GetRunwayDirection(THR_AIRTRACK);
                    if (PDM_RunwayDirection != null)
                    {
                        //PDM_RunwayDirection.Tag = THR_AIRTRACK.ARINC_OBJ;
                        rwy.RunwayDirectionList.Add(PDM_RunwayDirection);
                    }

                }

                if (rwy.RunwayDirectionList != null)
                {
                    #region

                    RunwayDirection rdn1 = rwy.RunwayDirectionList[0];
                    if (rdn1 != null)
                    {
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
                            //CreatedManually = true
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
                            //CreatedManually = true
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
                            //CreatedManually = true
                        };


                    }



                    RunwayDirection rdn2 = rwy.RunwayDirectionList.Count > 1 ? rwy.RunwayDirectionList[1] : null;
                    if (rdn2 != null)
                    {
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
                            //CreatedManually = true
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
                            //CreatedManually = true
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
                            //CreatedManually = true
                        };
                    }

                    if ((rdn1 != null) && (rdn2 != null))
                    {
                        //rdn1.CenterLinePoints.Add(rdn2.CenterLinePoints[1]);
                        rdn1.CenterLinePoints.Add(clpEND2);

                        //rdn2.CenterLinePoints.Add(rdn1.CenterLinePoints[1]);
                        rdn2.CenterLinePoints.Add(clpEND1);
                    }

                    #endregion
                }

            }

            if (AirtrackRWY.Shape != null) rwy.Geo = AirtrackRWY.Shape.Geometry;

            return rwy;

        }

        public RunwayDirection GetRunwayDirection(Object_AIRTRACK objAirtrack)
        {

            RunwayDirection rdn = new RunwayDirection();
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();

            rdn.ID = objAirtrack.ID_AIRTRACK;

            RunWay_THR_AIRTRACK AirtrackTHR = (RunWay_THR_AIRTRACK)objAirtrack;

            rdn.TrueBearing = AirtrackTHR.True_Bearing;
            rdn.Designator = AirtrackTHR.RDN_TXT_DESIG;
            //rdn.DisplacedThresholdDistance = AirtrackTHR.Displaced_Threshold_Distance_M;
            rdn.RdnDeclaredDistance = new List<DeclaredDistance>();
            rdn.RdnDeclaredDistance.Add(new DeclaredDistance { DistanceType = CodeDeclaredDistance.DPLM, DistanceValue = AirtrackTHR.Displaced_Threshold_Distance_M, DistanceUOM = UOM_DIST_HORZ.M });
            rdn.Uom = UOM_DIST_HORZ.M;
            rdn.Elev = AirtrackTHR.ThrElev_M;
            rdn.Elev_UOM = UOM_DIST_VERT.M;
            //rdn.Elev_m = ArnUtil.ConvertValueToMeter( AirtrackTHR.ThrElev_FT.ToString(),"FT");
            rdn.CenterLinePoints = new List<RunwayCenterLinePoint>();

            rdn.LandingThresholdElevation = AirtrackTHR.ThrElev_M;
            rdn.Stopway = AirtrackTHR.THR_Stopway_M;
            rdn.Lat = ((ARINC_Runway_Primary_Records)AirtrackTHR.ARINC_OBJ).Runway_Latitude;
            rdn.Lon = ((ARINC_Runway_Primary_Records)AirtrackTHR.ARINC_OBJ).Runway_Longitude;

            rdn.MagBearing = AirtrackTHR.Magnetic_Bearing;

            if (AirtrackTHR.RelatedIls != null)
            {
                rdn.Related_NavaidSystem = new List<NavaidSystem>();

                AIRTRACK_PDM_Converter.AIRTRAC_Object_Convert(AirtrackTHR.RelatedIls);

                NavaidSystem ils = (NavaidSystem)AIRTRACK_PDM_Converter.AIRTRAC_Object_Convert(AirtrackTHR.RelatedIls);

                rdn.Related_NavaidSystem.Add(ils);

               
            }

            if ((AirtrackTHR.RelatedMarker != null) && (AirtrackTHR.RelatedMarker.Count >0))
            {
                NavaidSystem mkrList = new NavaidSystem();

                mkrList.Name =((Marker_AIRTRACK) AirtrackTHR.RelatedMarker[0]).AirportCode;
                mkrList.Designator = ((Marker_AIRTRACK)AirtrackTHR.RelatedMarker[0]).LoalizerCode +"_"+((Marker_AIRTRACK)AirtrackTHR.RelatedMarker[0]).RdnCode;
                mkrList.ID_Feature = mkrList.ID;
                mkrList.CodeNavaidSystemType = NavaidSystemType.MKR;
                mkrList.Components = new List<NavaidComponent>();
                foreach (var airtrackMkr in AirtrackTHR.RelatedMarker)
                {
                    Marker markerPDM = GetMarker(airtrackMkr);
                    if (markerPDM != null)
                    {
                        markerPDM.ID_NavaidSystem = mkrList.ID;
                        mkrList.Components.Add(markerPDM);
                    }
                }


                rdn.Related_NavaidSystem.Add(mkrList);
            }

            if (AirtrackTHR.Shape!=null) rdn.Geo = AirtrackTHR.Shape.Geometry;

            return rdn;

        }



        public Marker GetMarker(Object_AIRTRACK objAirtrack)
        {
            Marker mkr = new Marker();

            Marker_AIRTRACK AirtrackMarker = (Marker_AIRTRACK)objAirtrack;
            ARINC_Airport_Marker ArincMkr = (ARINC_Airport_Marker)AirtrackMarker.ARINC_OBJ;

            mkr.ID = objAirtrack.ID_AIRTRACK;

            double d = 0;
            Double.TryParse(ArincMkr.Minor_Axis_Bearing, out d);

            mkr.Axis_Bearing = d / 10;

            mkr.Lat = ArincMkr.Marker_Latitude.Trim();
            mkr.Lon = ArincMkr.Marker_Longitude.Trim();
            mkr.CodeId = ArincMkr.Marker_Type;
            //mkr.CodeMorse = ArincMkr.mo
            Double.TryParse(ArincMkr.Locator_Frequency, out d);
            mkr.Frequency = d;
            mkr.Frequency_UOM = UOM_FREQ.KHZ;
            if (AirtrackMarker.Shape != null) mkr.Geo = AirtrackMarker.Shape.Geometry;

            return mkr;
        }

    }

    public class Navaid_PDM_Converter : IAIRTRACK_PDM_Converter
    {
        public PDMObject Convert_AIRTRAC_Object(Object_AIRTRACK elem)
        {
            NavaidSystem _Object = GetNavaidSystem(elem);
            if (_Object.CodeNavaidSystemType == NavaidSystemType.OTHER) _Object = null;
            return _Object;
        }

        public NavaidSystem GetNavaidSystem(Object_AIRTRACK objAirtrack)
        {
            NavaidSystem ns = new NavaidSystem();
            ns.ID_Feature = objAirtrack.ID_AIRTRACK;
            //ns.ID_NavaidSystem = Guid.NewGuid().ToString();
            ns.Components = new List<NavaidComponent>();


            #region ILS

            if (objAirtrack is ILS_AIRTRACK)
            {
                ILS_AIRTRACK ILSAIRTRACK = (ILS_AIRTRACK)objAirtrack;

                
                ns.CodeNavaidSystemType = NavaidSystemType.ILS;
                
                ARINC_LocalizerGlideSlope_Primary_Record _Record = (ARINC_LocalizerGlideSlope_Primary_Record)ILSAIRTRACK.ARINC_OBJ;
                ns.Name = _Record.Localizer_Identifier;
                ns.Designator = _Record.Localizer_Identifier;

                ns.NAVAID_Class = _Record.ILS_Category;

                #region GlidePath

                GlidePath gp = new GlidePath();

                AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();

                gp.UomDistVer = UOM_DIST_VERT.FT;
                gp.UomFreq = UOM_FREQ.KHZ;//"KHZ";
                gp.UomDistHor = UOM_DIST_HORZ.FT; //"FT";
                double dbl;
                Double.TryParse(_Record.Glide_Slope_Elevation, out dbl);
                gp.Elev = dbl;
                //gp.Elev_m = ArnUtil.ConvertValueToMeter(dbl.ToString(), "FT");

                Double.TryParse(_Record.Glide_Slope_Height_at_Landing_Threshold, out dbl);
                gp.ThresholdCrossingHeight = dbl;
                Double.TryParse(_Record.Glide_Slope_Angle, out dbl);
                gp.Angle = dbl / 100;
                Double.TryParse(_Record.Glide_Slope_Position, out dbl);
                gp.AntennaPosition = dbl;

                gp.Lat = _Record.Glide_Slope_Latitude;
                gp.Lon = _Record.Glide_Slope_Longitude;

                if (ILSAIRTRACK.GlideSlope_Shape !=null)
                    gp.Geo = ILSAIRTRACK.GlideSlope_Shape.Geometry;

                //gp.Tag = _Record;
                gp.ID_NavaidSystem = ns.ID;
                #endregion

                #region Localizer

                Localizer lc = new Localizer();


                lc.Declination = _Record.Station_Declination;

                lc.Lat = _Record.Localizer_Latitude;
                lc.Lon = _Record.Localizer_Longitude;

                lc.Frequency_UOM = UOM_FREQ.KHZ;//"KHZ";
                //double dbl = 0;

                Double.TryParse(_Record.Localizer_Frequency, out dbl);
                lc.Frequency = dbl;
                if (_Record.Localizer_Frequency.Contains(".")) lc.Frequency_UOM = UOM_FREQ.MHZ;//"MHZ";

                if (_Record.Localizer_Bearing.EndsWith("T"))
                {
                    _Record.Localizer_Bearing = _Record.Localizer_Bearing.Remove(_Record.Localizer_Bearing.Length - 1, 1);
                    Double.TryParse(_Record.Localizer_Bearing, out dbl);
                    lc.trueBearing = dbl / 10;
                    lc.MagBrg = Double.NaN;
                }
                else if (_Record.Localizer_Bearing.Trim().Length > 0)
                {
                    Double.TryParse(_Record.Localizer_Bearing, out dbl);
                    lc.MagBrg = dbl / 10;
                    lc.trueBearing = Double.NaN;
                }
                else
                {
                    lc.MagBrg = Double.NaN;
                    lc.trueBearing = Double.NaN;
                }


                if (_Record.Localizer_Width.Trim().Length > 0)
                {
                    Double.TryParse(_Record.Localizer_Width, out dbl);
                    lc.Width = dbl;
                }
                else
                    lc.Width = Double.NaN;

                if (_Record.Localizer_Position.Trim().Length > 0)
                {
                    Double.TryParse(_Record.Localizer_Position, out dbl);
                    lc.Localizer_Position = dbl;
                }
                else
                    lc.Localizer_Position = Double.NaN;

                lc.Uom = UOM_DIST_HORZ.FT;//"FT";

                if (ILSAIRTRACK.Shape!=null) 
                    lc.Geo = ILSAIRTRACK.Shape.Geometry;

                //lc.Tag = _Record;
                lc.ID_NavaidSystem = ns.ID;
                #endregion

                if ((ILSAIRTRACK.RelatedDME != null) && (ILSAIRTRACK.RelatedDME is DME_AIRTRACK))
                {
                    ns.CodeNavaidSystemType = NavaidSystemType.ILS_DME;
                    DME dme = new DME();
                    DME_AIRTRACK AirtrackDME = (DME_AIRTRACK)ILSAIRTRACK.RelatedDME;
                    dme.Lat = (AirtrackDME.ARINC_OBJ as ARINC_Navaid_VHF_Primary_Record).DME_Latitude;
                    dme.Lon = (AirtrackDME.ARINC_OBJ as ARINC_Navaid_VHF_Primary_Record).DME_Longitude;
                    dme.Designator = (AirtrackDME.ARINC_OBJ as ARINC_Navaid_VHF_Primary_Record).DME_Ident;
                    dme.NavName = (AirtrackDME.ARINC_OBJ as ARINC_Navaid_VHF_Primary_Record).Name;
                    if (AirtrackDME.Shape != null) dme.Geo = AirtrackDME.Shape.Geometry;
                    //dme.Tag = AirtrackDME.ARINC_OBJ;

                    dme.ID_NavaidSystem = ns.ID;
                    ns.Components.Add(dme);

                }

                else if ((ILSAIRTRACK.RelatedDME != null) && (ILSAIRTRACK.RelatedDME is TACAN_AIRTRACK))
                {
                    ns.CodeNavaidSystemType = NavaidSystemType.TLS;
                    TACAN tacan = new TACAN();
                    TACAN_AIRTRACK AirtrackDME = (TACAN_AIRTRACK)ILSAIRTRACK.RelatedDME;
                    tacan.Lat = (AirtrackDME.ARINC_OBJ as ARINC_Navaid_VHF_Primary_Record).DME_Latitude;
                    tacan.Lon = (AirtrackDME.ARINC_OBJ as ARINC_Navaid_VHF_Primary_Record).DME_Longitude;

                    if (AirtrackDME.Shape != null) tacan.Geo = AirtrackDME.Shape.Geometry;

                    //tacan.Tag = AirtrackDME.ARINC_OBJ;
                    tacan.ID_NavaidSystem = ns.ID;

                    ns.Components.Add(tacan);

                }

                ns.Components.Add(gp);
                ns.Components.Add(lc);
            }

            #endregion

            #region NDB

            if (objAirtrack is NDB_AIRTRACK)
            {
                NDB ndb = new NDB();
                NDB_AIRTRACK AirtrackNDB = (NDB_AIRTRACK)objAirtrack;
                ARINC_Navaid_NDB_Primary_Record ArincNDB = (ARINC_Navaid_NDB_Primary_Record)AirtrackNDB.ARINC_OBJ;

                ns.Designator = ArincNDB.Navaid_Identifier;
                ns.Name = ArincNDB.NDB_Name;
                ns.CodeNavaidSystemType = NavaidSystemType.NDB;
                ns.ID_AirportHeliport = ArincNDB.Airport_ICAO_Identifier;

                ndb.Frequency = AirtrackNDB.ValFreq_MHZ;
                ndb.Frequency_UOM = UOM_FREQ.MHZ;
                double d;
                Double.TryParse(ArincNDB.Magnetic_Variation, out d);
                ndb.MagVar = d;
                ndb.Lat = ArincNDB.NDB_Latitude;
                ndb.Lon = ArincNDB.NDB_Longitude;

                if (AirtrackNDB.Shape != null) ndb.Geo = AirtrackNDB.Shape.Geometry;
                //ndb.Tag = ArincNDB;
                ndb.ID_NavaidSystem = ns.ID;
                ns.Components.Add(ndb);

            }
            #endregion

            #region VOR

            if (objAirtrack is VOR_AIRTRACK)
            {
                VOR vor = new VOR();
                VOR_AIRTRACK AirtrackVOR = (VOR_AIRTRACK)objAirtrack;
                ARINC_Navaid_VHF_Primary_Record ArincVOR = (ARINC_Navaid_VHF_Primary_Record)AirtrackVOR.ARINC_OBJ;

                ns.Designator = ArincVOR.Navaid_Identifier;
                ns.Name = ArincVOR.VHF_Name;
                ns.NAVAID_Class = ArincVOR.NAVAID_Class;
                ns.CodeNavaidSystemType = NavaidSystemType.VOR;

               
                ns.ID_AirportHeliport = ArincVOR.Airport_ICAO_Identifier;

                vor.Lat = ArincVOR.VOR_Latitude;
                vor.Lon = ArincVOR.VOR_Longitude;
                vor.Designator = ArincVOR.Navaid_Identifier;
                vor.NavName = ArincVOR.Name;

                vor.Frequency = ArincVOR.VOR_Frequency.Trim().Length > 0 ? Convert.ToDouble(ArincVOR.VOR_Frequency) : 0;
                vor.Frequency_UOM = UOM_FREQ.MHZ;
                if (AirtrackVOR.Shape != null) vor.Geo = AirtrackVOR.Shape.Geometry;
                //vor.Tag = ArincVOR;
                vor.ID_NavaidSystem = ns.ID;
                ns.Components.Add(vor);

                if (AirtrackVOR.DME != null)
                {
                    ns.CodeNavaidSystemType = NavaidSystemType.VOR_DME;
                    DME dme = new DME();
                    DME_AIRTRACK AirtrackDME = (DME_AIRTRACK)AirtrackVOR.DME;
                    dme.Lat = ArincVOR.DME_Latitude;
                    dme.Lon = ArincVOR.DME_Longitude;
                    dme.Designator = ArincVOR.DME_Ident;
                    dme.NavName = ArincVOR.DME_Ident;
                   if (AirtrackDME.Shape != null) dme.Geo = AirtrackDME.Shape.Geometry;
                   //dme.Tag = ArincVOR;
                   dme.ID_NavaidSystem = ns.ID;
                   ns.Components.Add(dme);

                }

                if (AirtrackVOR.TACAN != null)
                {
                    ns.CodeNavaidSystemType = NavaidSystemType.VORTAC;
                    TACAN tacan = new TACAN();
                    TACAN_AIRTRACK AirtrackTAC = (TACAN_AIRTRACK)AirtrackVOR.TACAN;
                    tacan.Lat = ArincVOR.DME_Latitude;
                    tacan.Lon = ArincVOR.DME_Longitude;

                    if (AirtrackTAC.Shape != null) tacan.Geo = AirtrackTAC.Shape.Geometry;
                    tacan.ID_NavaidSystem = ns.ID;
                    //tacan.Tag = ArincVOR;
                    ns.Components.Add(tacan);

                }

               

            }
            #endregion

            #region DME

            if ((objAirtrack is DME_AIRTRACK) && ((objAirtrack as DME_AIRTRACK).VHF_code == VHF_NAVAID_AIRTRACK.VHF_NAVAID_code.DME))
            {
                DME dme = new DME();
                DME_AIRTRACK AirtrackDME = (DME_AIRTRACK)objAirtrack;
                ARINC_Navaid_VHF_Primary_Record ArincDME = (ARINC_Navaid_VHF_Primary_Record)AirtrackDME.ARINC_OBJ;

                ns.Designator = ArincDME.Navaid_Identifier;
                ns.Name = ArincDME.VHF_Name;
                ns.NAVAID_Class = ArincDME.NAVAID_Class;
                ns.CodeNavaidSystemType = NavaidSystemType.DME;
                ns.Elev = AirtrackDME.ValElevM;
                ns.Elev_UOM = UOM_DIST_VERT.M;
                //if ((objAirtrack as DME_AIRTRACK).VHF_code == VHF_NAVAID_AIRTRACK.VHF_NAVAID_code.DME_ILS)
                //    ns.CodeNavaidSystemType = NavaidSystemType.ILS_DME;


                ns.ID_AirportHeliport = ArincDME.Airport_ICAO_Identifier;

                dme.Lat = ArincDME.DME_Latitude;
                dme.Lon = ArincDME.DME_Longitude;
                dme.Designator = ArincDME.DME_Ident;
                dme.NavName = ArincDME.Name;

                if (AirtrackDME.Shape != null) dme.Geo = AirtrackDME.Shape.Geometry;
                //dme.Tag = ArincDME;
                dme.ID_NavaidSystem = ns.ID;
                ns.Components.Add(dme);

            }

            #endregion

            #region TACAN

            if ((objAirtrack is TACAN_AIRTRACK) && ((objAirtrack as TACAN_AIRTRACK).VHF_code == VHF_NAVAID_AIRTRACK.VHF_NAVAID_code.TACAN))
            {
                TACAN tacan = new TACAN();
                TACAN_AIRTRACK AirtrackTAC = (TACAN_AIRTRACK)objAirtrack;
                ARINC_Navaid_VHF_Primary_Record ArincTACAN = (ARINC_Navaid_VHF_Primary_Record)AirtrackTAC.ARINC_OBJ;

                ns.Designator = ArincTACAN.Navaid_Identifier;
                ns.Name = ArincTACAN.VHF_Name;
                ns.NAVAID_Class = ArincTACAN.NAVAID_Class;
                ns.CodeNavaidSystemType = NavaidSystemType.TACAN;
                ns.ID_AirportHeliport = ArincTACAN.Airport_ICAO_Identifier;

                tacan.Lat = ArincTACAN.DME_Latitude;
                tacan.Lon = ArincTACAN.DME_Longitude;
                tacan.Designator = ArincTACAN.DME_Ident;
                tacan.NavName = ArincTACAN.Name;
                if (AirtrackTAC.Shape != null) tacan.Geo = AirtrackTAC.Shape.Geometry;

                //tacan.Tag = ArincTACAN;
                tacan.ID_NavaidSystem = ns.ID;
                ns.Components.Add(tacan);

            }

            #endregion

            if (ns.Components.Count == 0) ns.CodeNavaidSystemType = NavaidSystemType.OTHER;

            return ns;

        }

    }

    public class WayPoint_PDM_Converter : IAIRTRACK_PDM_Converter
    {
        public PDMObject Convert_AIRTRAC_Object(Object_AIRTRACK objAirtrack)
        {
            WayPoint _Object = new WayPoint();


            WayPoint_AIRTRACK AirtrackWaypoint = (WayPoint_AIRTRACK)objAirtrack;
            ARINC_WayPoint_Primary_Record ArincWyp = (ARINC_WayPoint_Primary_Record)AirtrackWaypoint.ARINC_OBJ;

            _Object.ID = objAirtrack.ID_AIRTRACK;

            _Object.Designator = ArincWyp.Waypoint_Identifier.Trim();

            _Object.Lat = ArincWyp.Waypoint_Latitude.Trim();
            _Object.Lon = ArincWyp.Waypoint_Longitude.Trim();
            _Object.Name = ArincWyp.Waypoint_Name_Description.Trim();

            _Object.Enroute_Terminal = AirtrackWaypoint.Terminal_Enroute;
            if (!_Object.Enroute_Terminal.StartsWith("ENROUTE"))
            {
                _Object.ID_AirportHeliport = _Object.Enroute_Terminal;
                _Object.Enroute_Terminal = "TERMINAL";
            }

            _Object.Using = AirtrackWaypoint.Using;
            _Object.Function = AirtrackWaypoint.Function;
            _Object.Type = DesignatorType.ICAO;

            //_Object.ReportingATC = CodeATCReporting.NO_REPORT;

            if (AirtrackWaypoint.Shape != null) _Object.Geo = AirtrackWaypoint.Shape.Geometry;


            return _Object;
        }
    }

    public class Procedure_PDM_Converter : IAIRTRACK_PDM_Converter
    {
        public PDMObject Convert_AIRTRAC_Object(Object_AIRTRACK elem)
        {
            Procedure _Object = new Procedure();
            return _Object;
        }
    }

    public class AREA_PDM_Converter : IAIRTRACK_PDM_Converter
    {
        public PDMObject Convert_AIRTRAC_Object(Object_AIRTRACK elem)
        {
            AREA_PDM _Object = new AREA_PDM();
            _Object.Geo = elem.Shape.Geometry;
            _Object.Name = ((AREA_AIRTRACK)elem).AreaName;

            return _Object;
        }
    }

    public class Route_PDM_Converter : IAIRTRACK_PDM_Converter
    {
        public PDMObject Convert_AIRTRAC_Object(Object_AIRTRACK elem)
        {
            Enroute _Object = GetEnroute(elem);
            return _Object;
        }

        public Enroute GetEnroute(Object_AIRTRACK objAirtrack)
        {
            Enroute Enrt = new Enroute();
            ROUTE_AIRTRACK RouteAIRTRACK = (ROUTE_AIRTRACK)objAirtrack;

            Enrt.ID = Guid.NewGuid().ToString();

            Enrt.TxtDesig = RouteAIRTRACK.RouteName;
            Enrt.InternationalUse = CodeRouteOrigin.OTHER;
            if ((RouteAIRTRACK.Segments != null) && (RouteAIRTRACK.Segments.Count > 0))
            {
                Enrt.Routes = new List<RouteSegment>();
                Enrt.RouteLength_UOM = UOM_DIST_HORZ.KM;
                foreach (SEGMENT_AIRTRACK Airtrack_seg in RouteAIRTRACK.Segments)
                {
                    RouteSegment Segment = GetRouteSegment(Airtrack_seg);
                    if (Segment != null)
                    {
                        Segment.ID_Route = Enrt.ID;

                        Enrt.RouteLength = Enrt.RouteLength + Enrt.ConvertValueToMeter(Segment.ValLen.Value, Segment.UomValLen.ToString());

                        Enrt.Routes.Add(Segment);
                    }
                }

                Enrt.RouteLength = Enrt.RouteLength * 0.0005399568;
                Enrt.RouteLength_UOM = UOM_DIST_HORZ.NM;
            }

            if (RouteAIRTRACK.Shape != null) Enrt.Geo = RouteAIRTRACK.Shape.Geometry;

            return Enrt;
            //RebuildGeo();
        }


        public RouteSegment GetRouteSegment(Object_AIRTRACK objAirtrack)
        {

            RouteSegment rsg = new RouteSegment();
            SEGMENT_AIRTRACK SegmentAIRTRACK = (SEGMENT_AIRTRACK)objAirtrack;
            ARINC_Enroute_Airways_Primary_Record ARINC_ENROUTE = SegmentAIRTRACK.StartPnt.ARINC_OBJ as ARINC_Enroute_Airways_Primary_Record;

            rsg.ID = Guid.NewGuid().ToString();

            double d;
            if (SegmentAIRTRACK.StartPnt != null)
            {
                RouteSegmentPoint stPnt = GetRouteSegmentPoint(SegmentAIRTRACK.StartPnt);
                if (stPnt != null)
                {
                    rsg.StartPoint = stPnt;
                    rsg.StartPoint.Route_LEG_ID = rsg.ID;
                    rsg.StartPoint.PointUse = ProcedureSegmentPointUse.START_POINT;
                    rsg.StartPoint.IsWaypoint = rsg.StartPoint.PointType == SegmentPointType.WayPoint;
                    if (rsg.StartPoint.IsWaypoint) 
                        rsg.StartPoint.PointChoice = PointChoice.DesignatedPoint;
                    else 
                        rsg.StartPoint.PointChoice = PointChoice.Navaid;
                    rsg.StartPoint.PointChoiceID = stPnt.ID;//SegmentPointDesignator;

                    rsg.StartPoint.PointRole = ProcedureFixRoleType.ENRT;
                }
            }
            if (SegmentAIRTRACK.EndPnt != null)
            {
                RouteSegmentPoint endPnt = GetRouteSegmentPoint(SegmentAIRTRACK.EndPnt);
                if (endPnt != null)
                {
                    rsg.EndPoint = endPnt;
                    rsg.EndPoint.Route_LEG_ID = rsg.ID;
                    rsg.EndPoint.PointUse = ProcedureSegmentPointUse.END_POINT;
                    rsg.EndPoint.IsWaypoint = rsg.EndPoint.PointType == SegmentPointType.WayPoint;
                    if (rsg.EndPoint.IsWaypoint)
                        rsg.EndPoint.PointChoice = PointChoice.DesignatedPoint;
                    else
                        rsg.EndPoint.PointChoice = PointChoice.Navaid;
                    rsg.EndPoint.PointChoiceID = endPnt.ID;//SegmentPointDesignator;

                    rsg.EndPoint.PointRole = ProcedureFixRoleType.ENRT;

                }
            }

            if (SegmentAIRTRACK.Shape != null) rsg.Geo = SegmentAIRTRACK.Shape.Geometry;

            rsg.CodeDir = CODE_ROUTE_SEGMENT_DIR.BOTH;
            if (ARINC_ENROUTE.Direction_Restriction.StartsWith("F")) rsg.CodeDir = CODE_ROUTE_SEGMENT_DIR.FORWARD;
            if (ARINC_ENROUTE.Direction_Restriction.StartsWith("B")) rsg.CodeDir = CODE_ROUTE_SEGMENT_DIR.BACKWARD;

            //rsg.CodeDistVerLower = CODE_DIST_VER.
            //rsg.CodeDistVerLowerOvrde = CODE_DIST_VER.
            //rsg.CodeDistVerMnm = CODE_DIST_VER.
            //rsg.CodeDistVerUpper = CODE_DIST_VER.
            rsg.CodeIntl = CODE_ROUTE_SEGMENT_CODE_INTL.OTHER;

            rsg.CodeLvl = CODE_ROUTE_SEGMENT_CODE_LVL.BOTH;
            if (ARINC_ENROUTE.Level.StartsWith("H")) rsg.CodeLvl = CODE_ROUTE_SEGMENT_CODE_LVL.UPPER;
            if (ARINC_ENROUTE.Level.StartsWith("L")) rsg.CodeLvl = CODE_ROUTE_SEGMENT_CODE_LVL.LOWER;

            rsg.CodeType = ARINC_ENROUTE.Route_Type;

            rsg.UomValDistVerLower = UOM_DIST_VERT.FT;
            string s = ARINC_ENROUTE.Minimum_Altitude1;
            if (ARINC_ENROUTE.Minimum_Altitude1.StartsWith("FL"))
            {
                rsg.UomValDistVerLower = UOM_DIST_VERT.FL;
                s = s.Remove(0, 2);
            }
            Double.TryParse(s, out d);
            rsg.ValDistVerLower = d;

            rsg.UomValDistVerLowerOvrde = UOM_DIST_VERT.FT;
            s = ARINC_ENROUTE.Minimum_Altitude2;
            if (ARINC_ENROUTE.Minimum_Altitude2.StartsWith("FL"))
            {
                rsg.UomValDistVerLowerOvrde = UOM_DIST_VERT.FL;
                s = s.Remove(0, 2);
            }
            Double.TryParse(s, out d);
            rsg.ValDistVerLowerOvrde = d;

            rsg.UomValDistVerUpper = UOM_DIST_VERT.FL;
            s = ARINC_ENROUTE.Maximum_Altitude;
            if (ARINC_ENROUTE.Maximum_Altitude.StartsWith("FL"))
            {
                rsg.UomValDistVerUpper = UOM_DIST_VERT.FL;
                s = s.Remove(0, 2);

            }
            Double.TryParse(s, out d);
            rsg.ValDistVerUpper = d;

            s = ARINC_ENROUTE.Inbound_Magnetic_Course;
            if (ARINC_ENROUTE.Inbound_Magnetic_Course.EndsWith("T"))
            {
                s = s.Remove(s.Length - 1, 1);
                Double.TryParse(s, out d);
                rsg.ValTrueTrack = d;
            }
            else
            {
                Double.TryParse(s, out d);
                rsg.ValMagTrack = d / 10;
            }

            s = ARINC_ENROUTE.Route_Distance_From;
            rsg.UomValLen = UOM_DIST_HORZ.NM;
            Double.TryParse(s, out d);
            rsg.ValLen = d / 10;

            s = ARINC_ENROUTE.Outbound_Magnetic_Course;
            if (ARINC_ENROUTE.Outbound_Magnetic_Course.EndsWith("T"))
            {
                s = s.Remove(s.Length - 1, 1);
                Double.TryParse(s, out d);
                rsg.ValReversTrueTrack = d;
            }
            else
            {
                Double.TryParse(s, out d);
                rsg.ValReversMagTrack = d / 10;
            }


            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();
            if (rsg.Geo != null)
            {
                if (rsg.ValLen <= 0)
                {
                    rsg.ValLen = ArnUtil.GetDistanceBetweenPoints_Elips((rsg.Geo as IPolyline).FromPoint, (rsg.Geo as IPolyline).ToPoint) * 0.0005399568;//NM


                    rsg.UomValLen = UOM_DIST_HORZ.NM;

                }

                AranSupport.Utilitys.AranSupportStruct_Azimuth az = ArnUtil.Return_Azimuth_ReverseAzimuth((rsg.Geo as IPolyline).FromPoint, (rsg.Geo as IPolyline).ToPoint);

                if (rsg.ValTrueTrack== null ||rsg.ValTrueTrack <= 0)
                {
                    rsg.ValTrueTrack = az.Azimuth;
                }

                if (rsg.ValReversTrueTrack==null || rsg.ValReversTrueTrack <= 0)
                {
                    rsg.ValReversTrueTrack = az.ReverseAzimuth;
                }

                if (rsg.ValMagTrack == null || rsg.ValMagTrack <= 0)
                {
//                    ExternalMagVariation.MagVar(lat, lon, altitude.Value,
//routeSegment.ActualDate.Day, routeSegment.ActualDate.Month, routeSegment.ActualDate.Year, 1);

                    rsg.ValMagTrack = az.Azimuth;
                }

                if (rsg.ValReversMagTrack == null || rsg.ValReversMagTrack <= 0)
                {
                    rsg.ValReversMagTrack = az.ReverseAzimuth;
                }
            }

            return rsg;
        }


        public RouteSegmentPoint GetRouteSegmentPoint(Object_AIRTRACK objAirtrack)
        {
            RouteSegmentPoint rsg = new RouteSegmentPoint();
            SEGMENT_POINT_AIRTRACK SegmentPointAIRTRACK = (SEGMENT_POINT_AIRTRACK)objAirtrack;

            rsg.ID = objAirtrack.ID_AIRTRACK;//Guid.NewGuid().ToString();
            rsg.SegmentPointDesignator = SegmentPointAIRTRACK.SegmentPoint_Designator;

            rsg.PointType = getPointType(((ARINC_Enroute_Airways_Primary_Record)objAirtrack.ARINC_OBJ).Section_Code, ((ARINC_Enroute_Airways_Primary_Record)objAirtrack.ARINC_OBJ).Subsection); 
            rsg.Lat = SegmentPointAIRTRACK.Shape.GeoLat;
            rsg.Lon = SegmentPointAIRTRACK.Shape.GeoLong;

            return rsg;
        }

        private SegmentPointType getPointType(string sectionCode, string subSectionCode)
        {
            SegmentPointType res = SegmentPointType.OTHER;

            switch (sectionCode.Trim() + subSectionCode.Trim())
            {
                case ("D"): 
                    res = SegmentPointType.NAVAID_VHF;
                    break;
                case ("DB"):
                case ("PN"): 
                    res = SegmentPointType.NDB;
                    break;
                case ("EA"):
                case ("PC"):
                    res = SegmentPointType.WayPoint;
                    break;
            }

            return res;
        }

    }

    public class Airspace_PDM_Converter : IAIRTRACK_PDM_Converter
    {
        public PDMObject Convert_AIRTRAC_Object(Object_AIRTRACK elem)
        {
            Airspace _Object = GetAirspace(elem);
            return _Object;
        }

        public Airspace GetAirspace(Object_AIRTRACK objAirtrack)
        {
            Airspace arsps = new Airspace();
            arsps.ID = Guid.NewGuid().ToString();
            Airspace_AIRTRACK AirspaceAIRTRACK = (Airspace_AIRTRACK)objAirtrack;
            arsps.CodeID = AirspaceAIRTRACK.AirspaceVolumeList[0].CodeId;
            arsps.TxtName = AirspaceAIRTRACK.AirspaceVolumeList[0].TxtName;

            arsps.AirspaceVolumeList = new List<AirspaceVolume>();
            arsps.ActivationDescription = new List<string>();

            foreach (AirspaceVolume_AIRTRACK VolumeAIRTRACK in AirspaceAIRTRACK.AirspaceVolumeList)
            {
                arsps.AirspaceVolumeList.Add(GetAirspaceVolume(VolumeAIRTRACK));

            }

            arsps.CodeType = arsps.AirspaceVolumeList.Count > 0 ? arsps.AirspaceVolumeList[0].CodeType : AirspaceType.OTHER;

            return arsps;

        }

        public AirspaceVolume GetAirspaceVolume(Object_AIRTRACK objAirtrack)
        {
            AirspaceVolume vol = new AirspaceVolume();
            AranSupport.Utilitys util = new AranSupport.Utilitys();
            AirspaceVolume_AIRTRACK AirtrackAispace = (AirspaceVolume_AIRTRACK)objAirtrack;

            vol.ID = objAirtrack.ID_AIRTRACK;
            vol.AirspaceCenterUID = AirtrackAispace.AirspaceCenter;
            vol.AirspaceCenterType = GetAirspaceCenterType(AirtrackAispace.AirspaceCenterType);
            vol.CodeDistVerLower = GetCodeDist(AirtrackAispace.CodeDistVerLower);
            vol.CodeDistVerUpper = GetCodeDist(AirtrackAispace.CodeDistVerUpper);
            vol.CodeId = AirtrackAispace.CodeId.Trim();
            var cType = ArenaStatic.ArenaStaticProc.airspaceCodeType_to_AirspaceType(AirtrackAispace.CodeType);
            vol.CodeType = (AirspaceType)Enum.Parse(typeof(AirspaceType), cType, true);
            vol.TxtName = AirtrackAispace.TxtName;

            double d;
            string tmp = "";
            #region ValDistVerLower

            tmp = AirtrackAispace.ValDistVerLower;

            if (AirtrackAispace.ValDistVerLower.StartsWith("FL"))
            {
                vol.UomValDistVerLower = UOM_DIST_VERT.FL;
                tmp = AirtrackAispace.ValDistVerLower.Remove(0, 2);
            }

            Double.TryParse(tmp, out d);
            vol.ValDistVerLower = d;


            if (AirtrackAispace.ValDistVerLower.StartsWith("FL"))
            {
                vol.ValDistVerLower_M = util.ConvertValueToMeter(d.ToString(), "FL");

            }
            else if (AirtrackAispace.VertUom.ToString().StartsWith("M"))
            {
                vol.UomValDistVerLower = UOM_DIST_VERT.M;
                vol.ValDistVerLower_M = d;
            }
            else
            {
                vol.UomValDistVerLower = UOM_DIST_VERT.FT;
                vol.ValDistVerLower_M = util.ConvertValueToMeter(d.ToString(), "FT");

            }

            #endregion

            #region ValDistVerUpper

            tmp = AirtrackAispace.ValDistVerUpper;

            if (AirtrackAispace.ValDistVerUpper.StartsWith("FL"))
            {
                vol.UomValDistVerUpper = UOM_DIST_VERT.FL;
                tmp = AirtrackAispace.ValDistVerUpper.Remove(0, 2);
                //vol.ValDistVerLower_M = util.ConvertValueToMeter(d.ToString(), "FL");
            }

            Double.TryParse(tmp, out d);
            vol.ValDistVerUpper = d;



            if (AirtrackAispace.ValDistVerUpper.StartsWith("FL"))
            {
                vol.ValDistVerUpper_M = util.ConvertValueToMeter(d.ToString(), "FL");

            }
            else if (AirtrackAispace.VertUom.ToString().StartsWith("M"))
            {
                vol.UomValDistVerUpper = UOM_DIST_VERT.M;
                vol.ValDistVerUpper_M = d;
            }
            else
            {
                vol.UomValDistVerUpper = UOM_DIST_VERT.FT;
                vol.ValDistVerUpper_M = util.ConvertValueToMeter(d.ToString(), "FT");

            }

            #endregion

            return vol;
        }

        private string GetAirspaceCenterType(string CenterType)
        {
            string res = "";

            switch (CenterType.Trim())
            {
                case ("PA"):
                    res = "Airport";
                    break;
                case ("EA"):
                case ("PC"):
                    res = "Waypoint";
                    break;
                case ("DB"):
                case ("PN"):
                    res = "NDB_Navaid";
                    break;
                case ("D"):
                    res = "VHF_Navaid";
                    break;
                case ("PI"):
                    res = "ILS";
                    break;
            }

            return res;
        }

        private CODE_DIST_VER GetCodeDist(string CodeDist)
        {
            if (CodeDist == null) return CODE_DIST_VER.OTHER;
            if (CodeDist.Trim().StartsWith("M")) return CODE_DIST_VER.ALT;
            else if (CodeDist.Trim().StartsWith("A")) return CODE_DIST_VER.HEI;
            else return CODE_DIST_VER.OTHER;
        }



    }

    public class Obstacle_PDM_Converter : IAIRTRACK_PDM_Converter
    {
        public PDMObject Convert_AIRTRAC_Object(Object_AIRTRACK elem)
        {
            VerticalStructure _Object = GetVerticalStructure(elem);
            return _Object;
        }

        public VerticalStructure GetVerticalStructure(Object_AIRTRACK elem)
        {
            VerticalStructure vs = new VerticalStructure();
            Obstacle_AIRTRACK obsAIRTRACK = (Obstacle_AIRTRACK)elem;

            vs.ID = Guid.NewGuid().ToString();
            
            vs.Name = obsAIRTRACK.INFO_AIRTRACK;
            vs.Group = false;
            vs.Lighted = obsAIRTRACK.Lighting;
            vs.LightingICAOStandard = false;
            vs.MarkingICAOStandard = false;
            vs.SynchronisedLighting = false;
            vs.Type = VerticalStructureType.OTHER;
            vs.Parts = new List<VerticalStructurePart>();

            VerticalStructurePart part = GetVerticalStructurePart(elem);
            if (part != null)
            {
                part.VerticalStructure_ID = vs.ID;
                vs.Parts.Add(part);
            }

            return vs;


        }

        public VerticalStructurePart GetVerticalStructurePart(Object_AIRTRACK elem)
        {
            VerticalStructurePart vsp = new VerticalStructurePart();

            Obstacle_AIRTRACK obsAIRTRACK = (Obstacle_AIRTRACK)elem;

            vsp.ID = Guid.NewGuid().ToString();

            vsp.Lat = obsAIRTRACK.Shape.GeoLat;
            vsp.Lon = obsAIRTRACK.Shape.GeoLong;
            vsp.Elev = obsAIRTRACK.Elev;
            //vsp.Elev_m = obsAIRTRACK.ElevM;
            vsp.Elev_UOM = (UOM_DIST_VERT)Enum.Parse(typeof(UOM_DIST_VERT), obsAIRTRACK.ElevUOM);
            vsp.Height = obsAIRTRACK.Height;
            vsp.Height_UOM = (UOM_DIST_VERT)Enum.Parse(typeof(UOM_DIST_VERT), obsAIRTRACK.HeightUOM);
            //vsp.HeightM = obsAIRTRACK.HeightM;
            vsp.Designator = obsAIRTRACK.ID;
            vsp.ConstructionStatus = StatusConstructionType.OTHER;
            vsp.Frangible = false; ;
            vsp.MarkingFirstColour = ColourType.OTHER;
            vsp.MarkingPattern = VerticalStructureMarkingType.OTHER;
            vsp.MarkingSecondColour = ColourType.OTHER;
            vsp.Mobile = false;
            vsp.Type = VerticalStructureType.OTHER;
            vsp.VerticalExtent = obsAIRTRACK.Elev;
            vsp.VerticalExtent_UOM =  (UOM_DIST_VERT)Enum.Parse(typeof(UOM_DIST_VERT), obsAIRTRACK.ElevUOM);
            vsp.VerticalExtentAccuracy = obsAIRTRACK.AccVert_M;
            vsp.VerticalExtentAccuracy_UOM = UOM_DIST_VERT.M;
            vsp.VisibleMaterial = VerticalStructureMaterialType.OTHER;

         
            if (obsAIRTRACK.Shape != null) vsp.Geo = obsAIRTRACK.Shape.Geometry;

            return vsp;
        }


    }
}
 