using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PDM;
using Aran.Temporality.Common.ArcGis;
using ESRI.ArcGIS.Geometry;
using System.Reflection;
using ESRI.ArcGIS.esriSystem;

namespace ARENA
{

    public class AIM_PDM_Converter
    {

        private static Dictionary<Type, Type> elementTypes = new Dictionary<Type, Type>();

        static AIM_PDM_Converter()
        {
            elementTypes.Add(typeof(Aran.Aim.Features.AirportHeliport), typeof(AIM_AIRPORT_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.Runway), typeof(AIM_RWY_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.RunwayDirection), typeof(AIM_THR_PDM_Converter));
            
            elementTypes.Add(typeof(Aran.Aim.Features.Navaid), typeof(AIM_NAVAID_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.DME), typeof(AIM_DME_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.VOR), typeof(AIM_VOR_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.NDB), typeof(AIM_NDB_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.MarkerBeacon), typeof(AIM_Marker_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.TACAN), typeof(AIM_TACAN_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.Localizer), typeof(AIM_Localizer_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.Glidepath), typeof(AIM_Glidepath_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.RunwayCentrelinePoint), typeof(AIM_CLP_PDM_Converter));
            
            elementTypes.Add(typeof(Aran.Aim.Features.DesignatedPoint), typeof(AIM_DPN_PDM_Converter));
            
            elementTypes.Add(typeof(Aran.Aim.Features.Airspace), typeof(AIM_ARSPS_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.AirspaceVolume), typeof(AIM_ARSPS_VOLUME_PDM_Converter));
            
            elementTypes.Add(typeof(Aran.Aim.Features.Route), typeof(AIM_ENROUTE_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.RouteSegment), typeof(AIM_ROUTESEGMENT_PDM_Converter));

            elementTypes.Add(typeof(Aran.Aim.Features.InstrumentApproachProcedure), typeof(AIM_InstrumentApproachProcedure_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.StandardInstrumentDeparture), typeof(AIM_StandardInstrumentDeparture_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.StandardInstrumentArrival), typeof(AIM_StandardInstrumentArrival_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.ProcedureTransition), typeof(AIM_ProcedureTransition_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.ArrivalFeederLeg), typeof(AIM_NormalLeg_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.ArrivalLeg), typeof(AIM_NormalLeg_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.DepartureLeg), typeof(AIM_NormalLeg_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.InitialLeg), typeof(AIM_NormalLeg_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.IntermediateLeg), typeof(AIM_NormalLeg_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.MissedApproachLeg), typeof(AIM_NormalLeg_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.FinalLeg), typeof(AIM_NormalLeg_PDM_Converter));

            elementTypes.Add(typeof(Aran.Aim.Features.VerticalStructure), typeof(AIM_VerticalStructure_PDM_Converter));

        }

        public static PDMObject AIM_Object_Convert(object  aimFeature, List<IGeometry> aimGeo)
        {
            if (elementTypes.ContainsKey(aimFeature.GetType()))
            {

                Type t = elementTypes[aimFeature.GetType()];
                IAIM_PDM_Converter result = (IAIM_PDM_Converter)Activator.CreateInstance(t);
                PDMObject obj = result.Convert_AIM_Object(aimFeature,aimGeo);
                CreateNotes(aimFeature, obj);
                return obj;
            }
            else
                return null;
        }


        public static void CreateNotes(object aimFeature,PDMObject pdmObj)
        {

            PropertyInfo propInfo = aimFeature.GetType().GetProperty("Annotation");
            if (propInfo == null) return;
            try
            {
                object objPropVal = propInfo.GetValue(aimFeature, null);
                if (objPropVal != null)
                {
                    List<Aran.Aim.Features.Note> _Annotation = (List<Aran.Aim.Features.Note>)objPropVal;
                    if ((_Annotation != null) && (_Annotation.Count > 0))
                    {
                        pdmObj.Notes = new List<string>();

                        foreach (var item in _Annotation)
                        {
                            foreach (var _note in item.TranslatedNote)
                            {
                                pdmObj.Notes.Add(_note.Note.Value.ToString());
                            }


                        }
                    }
                }
            }
            catch
            {

            }

            
        }

       
    }

    public interface IAIM_PDM_Converter
    {
        PDMObject Convert_AIM_Object(object  aimFeature, List<IGeometry>aimGeo);

 

    }

    public class AIM_AIRPORT_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object  aimFeature, List<IGeometry>aimGeo)
        {
            //AranSupport.Utilitys util = new AranSupport.Utilitys();

            Aran.Aim.Features.AirportHeliport aimObj = (Aran.Aim.Features.AirportHeliport)aimFeature;

            //if (aimObj.Designator.StartsWith("EVSM")) System.Windows.Forms.MessageBox.Show("");

            AirportHeliport pdmObj = new AirportHeliport 
            { 
                Designator = aimObj.Designator, 
                DesignatorIATA = aimObj.DesignatorIATA, 
                ID = aimObj.Identifier.ToString(),
                Elev = aimObj.FieldElevation == null? 0: aimObj.FieldElevation.Value,
                Lat = aimObj.ARP.Geo == null? aimObj.ARP.Geo.X.ToString() : "",
                Lon = aimObj.ARP.Geo == null ? aimObj.ARP.Geo.Y.ToString() : "",
                MagneticVariation = aimObj.MagneticVariation.HasValue? aimObj.MagneticVariation.Value : 0,
                Name = aimObj.Name,
                TransitionAltitude = aimObj.TransitionAltitude == null? 0: aimObj.TransitionAltitude.Value,
            };

            UOM_DIST_VERT uom_dist;
            if ((aimObj.TransitionAltitude!=null) && (Enum.TryParse<UOM_DIST_VERT>(aimObj.TransitionAltitude.Uom.ToString(), out uom_dist))) pdmObj.TransitionAltitudeUOM = uom_dist;
            if ((aimObj.FieldElevation!=null) && (Enum.TryParse<UOM_DIST_VERT>(aimObj.FieldElevation.Uom.ToString(), out uom_dist))) pdmObj.Elev_UOM = uom_dist;


            if (aimGeo != null)
            {
                var zAware = aimGeo[0] as IZAware;
                zAware.ZAware = true;
                ((IPoint)aimGeo[0]).Z = pdmObj.ConvertValueToMeter(pdmObj.Elev, pdmObj.Elev_UOM.ToString());


                var mAware = aimGeo[0] as IMAware;
                mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
            }

            return pdmObj;
        }
    }

    public class AIM_RWY_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object  aimFeature, List<IGeometry>aimGeo)
        {
            Aran.Aim.Features.Runway aimObj = (Aran.Aim.Features.Runway)aimFeature;

            Runway pdmObj = new Runway
            {
                ID = aimObj.Identifier.ToString(),
                CodeComposition = aimObj.SurfaceProperties != null && aimObj.SurfaceProperties.Composition != null && aimObj.SurfaceProperties.Composition.HasValue ? aimObj.SurfaceProperties.Composition.Value.ToString() : "",
                CodeCondSfc = (aimObj.SurfaceProperties!=null && aimObj.SurfaceProperties.SurfaceCondition!=null &&  aimObj.SurfaceProperties.SurfaceCondition.HasValue)?  aimObj.SurfaceProperties.SurfaceCondition.Value.ToString() : null,
                Designator = aimObj.Designator,
                Length = aimObj.NominalLength!=null? aimObj.NominalLength.Value : 0,
                Width = aimObj.NominalWidth != null ? aimObj.NominalWidth.Value : 0
            };

            UOM_DIST_HORZ uom_dist;
            if ((aimObj.NominalLength!=null) && (Enum.TryParse<UOM_DIST_HORZ>(aimObj.NominalLength.Uom.ToString(), out uom_dist))) pdmObj.Uom = uom_dist;

            return pdmObj;
        }
    }

    public class AIM_THR_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object  aimFeature, List<IGeometry>aimGeo)
        {
            //AranSupport.Utilitys util = new AranSupport.Utilitys();

            Aran.Aim.Features.RunwayDirection aimObj = (Aran.Aim.Features.RunwayDirection)aimFeature;

            RunwayDirection pdmObj = new RunwayDirection
            {
                ID = aimObj.Identifier.ToString(),
                Lat = ((IPoint)aimGeo[0]).X > 0 ? ((IPoint)aimGeo[0]).X.ToString()+"N" : ((IPoint)aimGeo[0]).X.ToString()+"S",
                Lon = ((IPoint)aimGeo[0]).Y > 0 ? ((IPoint)aimGeo[0]).Y.ToString()+"E" : ((IPoint)aimGeo[0]).Y.ToString()+"W",
                Designator = aimObj.Designator,
                Elev = aimObj.ElevationTDZ!=null?  aimObj.ElevationTDZ.Value: 0,
                LandingThresholdElevation =  aimObj.ElevationTDZ!=null? aimObj.ElevationTDZ.Value : 0,
                MagBearing = aimObj.MagneticBearing.HasValue ? aimObj.MagneticBearing.Value : 0,
                TrueBearing = aimObj.TrueBearing.HasValue ? aimObj.TrueBearing.Value : 0,
                Uom = UOM_DIST_HORZ.M,
            };

            UOM_DIST_VERT uom_dist;
            if ((aimObj.ElevationTDZAccuracy!=null) && (Enum.TryParse<UOM_DIST_VERT>(aimObj.ElevationTDZAccuracy.Uom.ToString(), out uom_dist))) pdmObj.Elev_UOM = uom_dist;
            //if (Enum.TryParse<UOM_DIST_VERT>(aimObj.FieldElevation.Uom.ToString(), out uom_dist)) pdmObj.Elev_UOM = uom_dist;

            var zAware = aimGeo[0] as IZAware;
            zAware.ZAware = true;
            ((IPoint)aimGeo[0]).Z = pdmObj.ConvertValueToMeter(pdmObj.Elev, pdmObj.Elev_UOM.ToString());


            var mAware = aimGeo[0] as IMAware;
            mAware.MAware = true;

            pdmObj.Geo = aimGeo[0];
            return pdmObj;
        }
    }

    public class AIM_NAVAID_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object  aimFeature, List<IGeometry>aimGeo)
        {
            Aran.Aim.Features.Navaid aimObj = (Aran.Aim.Features.Navaid)aimFeature;

            NavaidSystem pdmObj = new NavaidSystem
            {
                ID = Guid.NewGuid().ToString(),//aimObj.Identifier.ToString(),
                ID_Feature = aimObj.Identifier.ToString(),
                Designator = aimObj.Designator,
                Name = (aimObj.Name != null && aimObj.Name.Length > 0) ? aimObj.Name : aimObj.Designator,
            };

             NavaidSystemType navType;
             if (Enum.TryParse<NavaidSystemType>(aimObj.Type.ToString(), out navType)) pdmObj.CodeNavaidSystemType = navType;

            return pdmObj;
        }
    }

    public class AIM_DME_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object  aimFeature, List<IGeometry>aimGeo)
        {
            //AranSupport.Utilitys util = new AranSupport.Utilitys();

            Aran.Aim.Features.DME aimObj = (Aran.Aim.Features.DME)aimFeature;

            DME pdmObj = new DME
            {
                ID = aimObj.Identifier.ToString(),
                Lat = ((IPoint)aimGeo[0]).X.ToString(),
                Lon = ((IPoint)aimGeo[0]).Y.ToString(),
               DmeIdentifier = aimObj.Name,
               FrequencyProtection =  aimObj.GhostFrequency!=null ? aimObj.GhostFrequency.Value : 0,
            };

            //UOM_DIST_VERT uom_dist;
            //if (Enum.TryParse<UOM_DIST_VERT>(aimObj.ElevationTDZAccuracy.Uom.ToString(), out uom_dist)) pdmObj.Elev_UOM = uom_dist;

            var zAware = aimGeo[0] as IZAware;
            zAware.ZAware = true;
            ((IPoint)aimGeo[0]).Z = pdmObj.ConvertValueToMeter(pdmObj.Elev, pdmObj.Elev_UOM.ToString());


            var mAware = aimGeo[0] as IMAware;
            mAware.MAware = true;

            pdmObj.Geo = aimGeo[0];
            return pdmObj;
        }
    }

    public class AIM_VOR_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object  aimFeature, List<IGeometry>aimGeo)
        {
            //AranSupport.Utilitys util = new AranSupport.Utilitys();

            Aran.Aim.Features.VOR aimObj = (Aran.Aim.Features.VOR)aimFeature;

            VOR pdmObj = new VOR
            {
                ID = aimObj.Identifier.ToString(),
                Lat = ((IPoint)aimGeo[0]).X.ToString(),
                Lon = ((IPoint)aimGeo[0]).Y.ToString(),
                Frequency = (aimObj.Frequency != null) ? aimObj.Frequency.Value : 0,
                StationDeclination = (aimObj.Declination !=null)  && (aimObj.Declination.HasValue) ? aimObj.Declination.Value.ToString() : "0",
                VorIdentifier = aimObj.Designator,
            };

            if (aimObj.Frequency != null)
            {
                UOM_FREQ uom_freq;
                if (Enum.TryParse<UOM_FREQ>(aimObj.Frequency.Uom.ToString(), out uom_freq)) pdmObj.Frequency_UOM = uom_freq;
            }

            var zAware = aimGeo[0] as IZAware;
            zAware.ZAware = true;
            ((IPoint)aimGeo[0]).Z = pdmObj.ConvertValueToMeter(pdmObj.Elev, pdmObj.Elev_UOM.ToString());


            var mAware = aimGeo[0] as IMAware;
            mAware.MAware = true;

            pdmObj.Geo = aimGeo[0];
            return pdmObj;
        }
    }

    public class AIM_NDB_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object  aimFeature, List<IGeometry>aimGeo)
        {
            //AranSupport.Utilitys util = new AranSupport.Utilitys();

            Aran.Aim.Features.NDB aimObj = (Aran.Aim.Features.NDB)aimFeature;

            NDB pdmObj = new NDB
            {
                ID = aimObj.Identifier.ToString(),
                Lat = ((IPoint)aimGeo[0]).X.ToString(),
                Lon = ((IPoint)aimGeo[0]).Y.ToString(),
                Frequency =  aimObj.Frequency!=null ? aimObj.Frequency.Value : 0,
                MagVar = aimObj.MagneticVariation !=null ? aimObj.MagneticVariation.Value.ToString() : "",
                
            };

            UOM_FREQ uom_freq;
            if ((aimObj.Frequency!=null) && (Enum.TryParse<UOM_FREQ>(aimObj.Frequency.Uom.ToString(), out uom_freq))) pdmObj.Frequency_UOM = uom_freq;

            var zAware = aimGeo[0] as IZAware;
            zAware.ZAware = true;
            ((IPoint)aimGeo[0]).Z = pdmObj.ConvertValueToMeter(pdmObj.Elev, pdmObj.Elev_UOM.ToString());


            var mAware = aimGeo[0] as IMAware;
            mAware.MAware = true;

            pdmObj.Geo = aimGeo[0];
            return pdmObj;
        }
    }

    public class AIM_Marker_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            //AranSupport.Utilitys util = new AranSupport.Utilitys();

            Aran.Aim.Features.MarkerBeacon aimObj = (Aran.Aim.Features.MarkerBeacon)aimFeature;

            Marker pdmObj = new Marker
            {
                ID = aimObj.Identifier.ToString(),
                Lat = ((IPoint)aimGeo[0]).X.ToString(),
                Lon = ((IPoint)aimGeo[0]).Y.ToString(),
                Frequency = aimObj.Frequency != null ? aimObj.Frequency.Value : 0,
                Axis_Bearing = aimObj.AxisBearing != null &&  aimObj.AxisBearing.HasValue ? aimObj.AxisBearing.Value : 0,
                CodeMorse = aimObj.AuralMorseCode!=null ? aimObj.AuralMorseCode : "",
            };

            UOM_FREQ uom_freq;
            if ((aimObj.Frequency != null) && (Enum.TryParse<UOM_FREQ>(aimObj.Frequency.Uom.ToString(), out uom_freq))) pdmObj.Frequency_UOM = uom_freq;

            var zAware = aimGeo[0] as IZAware;
            zAware.ZAware = true;
            ((IPoint)aimGeo[0]).Z = pdmObj.ConvertValueToMeter(pdmObj.Elev, pdmObj.Elev_UOM.ToString());


            var mAware = aimGeo[0] as IMAware;
            mAware.MAware = true;

            pdmObj.Geo = aimGeo[0];
            return pdmObj;
        }
    }


    public class AIM_TACAN_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object  aimFeature, List<IGeometry>aimGeo)
        {
            //AranSupport.Utilitys util = new AranSupport.Utilitys();

            Aran.Aim.Features.TACAN aimObj = (Aran.Aim.Features.TACAN)aimFeature;

            TACAN pdmObj = new TACAN
            {
                ID = aimObj.Identifier.ToString(),
                Lat = ((IPoint)aimGeo[0]).X.ToString(),
                Lon = ((IPoint)aimGeo[0]).Y.ToString(),
                TacanIdentifier = aimObj.Name,
                
            };

            //UOM_FREQ uom_freq;
            //if (Enum.TryParse<UOM_FREQ>(aimObj.Frequency.Uom.ToString(), out uom_freq)) pdmObj.Frequency_UOM = uom_freq;

            var zAware = aimGeo[0] as IZAware;
            zAware.ZAware = true;
            ((IPoint)aimGeo[0]).Z = pdmObj.ConvertValueToMeter(pdmObj.Elev, pdmObj.Elev_UOM.ToString());


            var mAware = aimGeo[0] as IMAware;
            mAware.MAware = true;

            pdmObj.Geo = aimGeo[0];
            return pdmObj;
        }
    }

    public class AIM_Localizer_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object  aimFeature, List<IGeometry>aimGeo)
        {
            //AranSupport.Utilitys util = new AranSupport.Utilitys();

            Aran.Aim.Features.Localizer aimObj = (Aran.Aim.Features.Localizer)aimFeature;

            Localizer pdmObj = new Localizer
            {
                ID = aimObj.Identifier.ToString(),
                Lat = ((IPoint)aimGeo[0]).X.ToString(),
                Lon = ((IPoint)aimGeo[0]).Y.ToString(),
                Frequency = aimObj.Frequency.Value,
                Declination = aimObj.Declination.HasValue ?  aimObj.Declination.Value.ToString() : "0",
                MagBrg = aimObj.MagneticBearing.HasValue? aimObj.MagneticBearing.Value :0,
                trueBearing = aimObj.TrueBearing.HasValue ? aimObj.TrueBearing.Value :0,
                Width = aimObj.WidthCourse.HasValue ? aimObj.WidthCourse.Value : 0,

            };

            UOM_FREQ uom_freq;
            if (Enum.TryParse<UOM_FREQ>(aimObj.Frequency.Uom.ToString(), out uom_freq)) pdmObj.Frequency_UOM = uom_freq;

            var zAware = aimGeo[0] as IZAware;
            zAware.ZAware = true;
            ((IPoint)aimGeo[0]).Z = pdmObj.ConvertValueToMeter(pdmObj.Elev, pdmObj.Elev_UOM.ToString());


            var mAware = aimGeo[0] as IMAware;
            mAware.MAware = true;

            pdmObj.Geo = aimGeo[0];
            return pdmObj;
        }
    }

    public class AIM_Glidepath_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object  aimFeature, List<IGeometry>aimGeo)
        {
            //AranSupport.Utilitys util = new AranSupport.Utilitys();

            Aran.Aim.Features.Glidepath aimObj = (Aran.Aim.Features.Glidepath)aimFeature;

            GlidePath pdmObj = new GlidePath
            {
                ID = aimObj.Identifier.ToString(),
                Lat = ((IPoint)aimGeo[0]).X.ToString(),
                Lon = ((IPoint)aimGeo[0]).Y.ToString(),
                Angle = aimObj.Slope.HasValue ? aimObj.Slope.Value : 0,
            };

            //UOM_FREQ uom_freq;
            //if (Enum.TryParse<UOM_FREQ>(aimObj.Frequency.Uom.ToString(), out uom_freq)) pdmObj.Frequency_UOM = uom_freq;

            var zAware = aimGeo[0] as IZAware;
            zAware.ZAware = true;
            ((IPoint)aimGeo[0]).Z = pdmObj.ConvertValueToMeter(pdmObj.Elev, pdmObj.Elev_UOM.ToString());


            var mAware = aimGeo[0] as IMAware;
            mAware.MAware = true;

            pdmObj.Geo = aimGeo[0];
            return pdmObj;
        }
    }

    public class AIM_CLP_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object  aimFeature, List<IGeometry>aimGeo)
        {
            //AranSupport.Utilitys util = new AranSupport.Utilitys();

            Aran.Aim.Features.RunwayCentrelinePoint aimObj = (Aran.Aim.Features.RunwayCentrelinePoint)aimFeature;

            RunwayCenterLinePoint pdmObj = new RunwayCenterLinePoint
            {
                ID = aimObj.Identifier.ToString(),
                Lat = ((IPoint)aimGeo[0]).X.ToString(),
                Lon = ((IPoint)aimGeo[0]).Y.ToString(),
              
            };

            if (aimObj.Role.HasValue)
            {
                CodeRunwayCenterLinePointRoleType uom_role;
                if (Enum.TryParse<CodeRunwayCenterLinePointRoleType>(aimObj.Role.ToString(), out uom_role)) pdmObj.Role = uom_role;
            }
            else
                pdmObj.Role = CodeRunwayCenterLinePointRoleType.OTHER;


            if ((aimObj.AssociatedDeclaredDistance != null) && (aimObj.AssociatedDeclaredDistance.Count > 0))
            {
                pdmObj.DeclDist = new List<DeclaredDistance>();
                foreach (var decldist in aimObj.AssociatedDeclaredDistance)
                {
                    if (decldist.DeclaredValue.Count > 0)
                    {
                        DeclaredDistance DD = new DeclaredDistance { DistanceValue = decldist.DeclaredValue[0].Distance.Value };
                        UOM_DIST_HORZ _uomDist;
                        if (Enum.TryParse<UOM_DIST_HORZ>(decldist.DeclaredValue[0].Distance.Uom.ToString(), out _uomDist)) DD.DistanceUOM = _uomDist;
                        CodeDeclaredDistance _uomCode;
                        if (Enum.TryParse<CodeDeclaredDistance>(decldist.Type.Value.ToString(), out _uomCode)) DD.DistanceType = _uomCode;

                        pdmObj.DeclDist.Add(DD);
                    }
                }
            }

            var zAware = aimGeo[0] as IZAware;
            zAware.ZAware = true;
            ((IPoint)aimGeo[0]).Z = pdmObj.ConvertValueToMeter(pdmObj.Elev, pdmObj.Elev_UOM.ToString());


            var mAware = aimGeo[0] as IMAware;
            mAware.MAware = true;

            pdmObj.Geo = aimGeo[0];
            return pdmObj;
        }
    }

    public class AIM_DPN_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object  aimFeature, List<IGeometry>aimGeo)
        {
            //AranSupport.Utilitys util = new AranSupport.Utilitys();

            Aran.Aim.Features.DesignatedPoint aimObj = (Aran.Aim.Features.DesignatedPoint)aimFeature;

            WayPoint pdmObj = new WayPoint
            {
                ID = aimObj.Identifier.ToString(),
                Lat = ((IPoint)aimGeo[0]).X.ToString(),
                Lon = ((IPoint)aimGeo[0]).Y.ToString(),
                Designator = aimObj.Designator,
                Name = aimObj.Name == null ? aimObj.Name : "",
                ReportingATC = CodeATCReporting.NO_REPORT,

            };

            DesignatorType _uomDesType;
            if ((aimObj.Type != null) && (aimObj.Type.HasValue))
            {
                Enum.TryParse<DesignatorType>(aimObj.Type.Value.ToString(), out _uomDesType);
                pdmObj.Type = _uomDesType;
            }


            var zAware = aimGeo[0] as IZAware;
            zAware.ZAware = true;
            ((IPoint)aimGeo[0]).Z = pdmObj.ConvertValueToMeter(pdmObj.Elev, pdmObj.Elev_UOM.ToString());


            var mAware = aimGeo[0] as IMAware;
            mAware.MAware = true;

            pdmObj.Geo = aimGeo[0];
            return pdmObj;
        }
    }

    public class AIM_ARSPS_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object  aimFeature, List<IGeometry>aimGeo)
        {
            Aran.Aim.Features.Airspace aimObj = (Aran.Aim.Features.Airspace)aimFeature;

            Airspace pdmObj = new Airspace
            {
                ID = aimObj.Identifier.ToString(),
                TxtName = aimObj.Name,
                CodeID = aimObj.Designator,
                

            };

            if ((aimObj.Class!=null) && (aimObj.Class.Count>0))
            {
                if (aimObj.Class[0].Classification.HasValue) pdmObj.Lat = aimObj.Class[0].Classification.Value.ToString();
            }
           return pdmObj;
        }
    }

    public class AIM_ARSPS_VOLUME_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry>aimGeo)
        {
            Aran.Aim.Features.AirspaceVolume aimObj = (Aran.Aim.Features.AirspaceVolume)aimFeature;

            AirspaceVolume pdmObj = new AirspaceVolume();

            pdmObj.ID = Guid.NewGuid().ToString();
            UOM_DIST_VERT vert_uom;
            CODE_DIST_VER lim_ref_uom;

            if (aimObj.UpperLimit != null)
            {
                pdmObj.ValDistVerUpper = aimObj.UpperLimit.Value;
                Enum.TryParse<UOM_DIST_VERT>(aimObj.UpperLimit.Uom.ToString(), out vert_uom);
                pdmObj.UomDistVerUpper = vert_uom;

            }

            if (aimObj.LowerLimit != null)
            {
                pdmObj.ValDistVerLower = aimObj.LowerLimit.Value;
                Enum.TryParse<UOM_DIST_VERT>(aimObj.LowerLimit.Uom.ToString(), out vert_uom);
                pdmObj.UomDistVerLower = vert_uom;
            }

            if (aimObj.MaximumLimit != null)
            {
                pdmObj.ValDistVerMax = aimObj.MaximumLimit.Value;
                Enum.TryParse<UOM_DIST_VERT>(aimObj.MaximumLimit.Uom.ToString(), out vert_uom);
                pdmObj.UomDistVerMax = vert_uom;
            }


            if (aimObj.MinimumLimit != null)
            {
                pdmObj.ValDistVerMnm = aimObj.MinimumLimit.Value;
                Enum.TryParse<UOM_DIST_VERT>(aimObj.MinimumLimit.Uom.ToString(), out vert_uom);
                pdmObj.UomDistVerMnm = vert_uom;
            }

            if (aimObj.UpperLimitReference.HasValue)
            {
                Enum.TryParse<CODE_DIST_VER>(aimObj.UpperLimitReference.Value.ToString(), out lim_ref_uom);
                pdmObj.CodeDistVerUpper = lim_ref_uom;
            }

            if (aimObj.LowerLimitReference.HasValue)
            {
                Enum.TryParse<CODE_DIST_VER>(aimObj.LowerLimitReference.Value.ToString(), out lim_ref_uom);
                pdmObj.CodeDistVerLower = lim_ref_uom;
            }

            if (aimObj.MaximumLimitReference.HasValue)
            {
                Enum.TryParse<CODE_DIST_VER>(aimObj.MaximumLimitReference.Value.ToString(), out lim_ref_uom);
                pdmObj.CodeDistVerMax = lim_ref_uom;
            }

            if (aimObj.MinimumLimitReference.HasValue)
            {
                Enum.TryParse<CODE_DIST_VER>(aimObj.MinimumLimitReference.Value.ToString(), out lim_ref_uom);
                pdmObj.CodeDistVerMnm = lim_ref_uom;
            }
           
            var zAware = aimGeo[0] as IZAware;
            zAware.ZAware = true;
             //pdmObj.ConvertValueToMeter(pdmObj.ValDistVerUpper.ToString(), pdmObj.UomDistVerUpper.ToString());


            var mAware = aimGeo[0] as IMAware;
            mAware.MAware = true;

            pdmObj.Geo = aimGeo[0];
            return pdmObj;


        }
    }

    public class AIM_ENROUTE_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry>aimGeo)
        {
            Aran.Aim.Features.Route aimObj = (Aran.Aim.Features.Route)aimFeature;

            Enroute pdmObj = new Enroute
            {
                ID = aimObj.Identifier.ToString(),
                TxtDesig = aimObj.Name,
               
            };

            CodeRouteOrigin _uom;
            if (aimObj.InternationalUse.HasValue)
            {
                Enum.TryParse<CodeRouteOrigin>(aimObj.InternationalUse.Value.ToString(), out _uom);
                pdmObj.InternationalUse = _uom;
            }
              return pdmObj;
        }
    }

    public class AIM_ROUTESEGMENT_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry>aimGeo)
        {
            Aran.Aim.Features.RouteSegment aimObj = (Aran.Aim.Features.RouteSegment)aimFeature;

            RouteSegment pdmObj = new RouteSegment
            {
                ID = aimObj.Identifier.ToString(),
                ValDistVerUpper = (aimObj.UpperLimit != null) ? aimObj.UpperLimit.Value : 0,
                ValDistVerLower = (aimObj.LowerLimit != null) ? aimObj.LowerLimit.Value : 0,
                ValDistVerMnm = (aimObj.MinimumEnrouteAltitude != null) ? aimObj.MinimumEnrouteAltitude.Value : 0,
                ValLen = (aimObj.Length != null) ? aimObj.Length.Value : 0,
                ValWidLeft = (aimObj.WidthLeft != null) ? aimObj.WidthLeft.Value : 0,
                ValWidRight = (aimObj.WidthRight != null) ? aimObj.WidthRight.Value : 0,
                ValMagTrack = (aimObj.MagneticTrack.HasValue) ? aimObj.MagneticTrack.Value : 0,
                ValTrueTrack = (aimObj.TrueTrack.HasValue) ? aimObj.TrueTrack.Value : 0,
                ValReversMagTrack = (aimObj.ReverseMagneticTrack.HasValue) ? aimObj.ReverseMagneticTrack.Value : 0,
                ValReversTrueTrack = (aimObj.ReverseTrueTrack.HasValue) ? aimObj.ReverseTrueTrack.Value : 0,
                
            };


            CODE_ROUTE_SEGMENT_DIR _uomDIR;
            if (((aimObj.Availability != null) && (aimObj.Availability.Count >0) && (aimObj.Availability[0].Direction.HasValue)))
            {
                Enum.TryParse<CODE_ROUTE_SEGMENT_DIR>(aimObj.Availability[0].Direction.Value.ToString(), out _uomDIR);
                pdmObj.CodeDir = _uomDIR;
            }

            UOM_DIST_VERT _uomVert;
            if (aimObj.UpperLimit != null)
            {
                Enum.TryParse<UOM_DIST_VERT>(aimObj.UpperLimit.Uom.ToString(), out _uomVert);
                pdmObj.UomDistVerUpper = _uomVert;
            }

            if (aimObj.LowerLimit != null)
            {
                Enum.TryParse<UOM_DIST_VERT>(aimObj.LowerLimit.Uom.ToString(), out _uomVert);
                pdmObj.UomDistVerLower = _uomVert;
            }

            UOM_DIST_HORZ _uomHOR;
            if (aimObj.WidthLeft != null)
            {
                Enum.TryParse<UOM_DIST_HORZ>(aimObj.WidthLeft.Uom.ToString(), out _uomHOR);
                pdmObj.UomWid = _uomHOR;
            }

            if (aimObj.Length != null)
            {
                Enum.TryParse<UOM_DIST_HORZ>(aimObj.Length.Uom.ToString(), out _uomHOR);
                pdmObj.UomValLen = _uomHOR;
            }

            var zAware = aimGeo[0] as IZAware;
            zAware.ZAware = true;
            //pdmObj.ConvertValueToMeter(pdmObj.ValDistVerUpper.ToString(), pdmObj.UomDistVerUpper.ToString());


            var mAware = aimGeo[0] as IMAware;
            mAware.MAware = true;

            pdmObj.Geo = aimGeo[0];

            return pdmObj;
        }
    }

    public class AIM_InstrumentApproachProcedure_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry>aimGeo)
        {


            Aran.Aim.Features.InstrumentApproachProcedure aimObj = (Aran.Aim.Features.InstrumentApproachProcedure)aimFeature;

            if (aimObj.Name == null) return null;



            InstrumentApproachProcedure pdmObj = new InstrumentApproachProcedure
            {
                ID = aimObj.Identifier.ToString(),
                //FeatureGUID = Guid.NewGuid().ToString(),
                AirportIdentifier = aimObj.AirportHeliport != null && aimObj.AirportHeliport.Count > 0 ? aimObj.AirportHeliport[0].Feature.Identifier.ToString() : "",
                ProcedureIdentifier = aimObj.Name,
                ProcedureType = PROC_TYPE_code.Approach,
                CommunicationFailureDescription = aimObj.CommunicationFailureInstruction != null ? aimObj.CommunicationFailureInstruction : "",
                FlightChecked = aimObj.FlightChecked != null && aimObj.FlightChecked.HasValue ? aimObj.FlightChecked.Value : false,
                RNAV = aimObj.RNAV != null && aimObj.RNAV.HasValue ? aimObj.RNAV.Value : false,
                CopterTrack = aimObj.CopterTrack != null && aimObj.CopterTrack.HasValue ? aimObj.CopterTrack.Value : 0,
                CourseReversalInstruction = (aimObj.CourseReversalInstruction != null) ? aimObj.CourseReversalInstruction : "",
                ChannelGNSS = aimObj.ChannelGNSS != null && aimObj.ChannelGNSS.HasValue ? aimObj.ChannelGNSS.Value : 0,
                WAASReliable = aimObj.WAASReliable != null && aimObj.WAASReliable.HasValue ? aimObj.WAASReliable.Value : false,
                ID_MasterProc = aimObj.Identifier.ToString(),
                AircraftCharacteristic = GetAircraftCharacteristic(aimObj.AircraftCharacteristic),
                LandingArea = aimObj.Landing != null && aimObj.Landing.Runway != null && aimObj.Landing.Runway.Count > 0 ? new PDMObject { ID = aimObj.Landing.Runway[0].Feature.Identifier.ToString() } : null,


            };


            ProcedureCodingStandardType _uomCodding;
            if ((aimObj.CodingStandard != null) && (aimObj.CodingStandard.HasValue))
            {
                Enum.TryParse<ProcedureCodingStandardType>(aimObj.CodingStandard.Value.ToString(), out _uomCodding);
                pdmObj.CodingStandard = _uomCodding;
            }

            

            ProcedureDesignStandardType _uomDesCrit;
            if ((aimObj.DesignCriteria != null) && (aimObj.DesignCriteria.HasValue))
            {
                Enum.TryParse<ProcedureDesignStandardType>(aimObj.DesignCriteria.Value.ToString(), out _uomDesCrit);
                pdmObj.DesignCriteria = _uomDesCrit;
            }
            
            CodeApproachPrefix _uomApproachPref;
            if ((aimObj.ApproachPrefix != null) && (aimObj.ApproachPrefix.HasValue))
            {
                Enum.TryParse<CodeApproachPrefix>(aimObj.ApproachPrefix.Value.ToString(), out _uomApproachPref);
                pdmObj.ApproachPrefix = _uomApproachPref;
            }

            ApproachType _uomApproachType;
            if ((aimObj.ApproachPrefix != null) && (aimObj.ApproachPrefix.HasValue))
            {
                Enum.TryParse<ApproachType>(aimObj.ApproachType.Value.ToString(), out _uomApproachType);
                pdmObj.ApproachType = _uomApproachType;
            }


            CodeApproachEquipmentAdditional _uomAddEq;
            if ((aimObj.AdditionalEquipment != null) && (aimObj.AdditionalEquipment.HasValue))
            {
                Enum.TryParse<CodeApproachEquipmentAdditional>(aimObj.AdditionalEquipment.Value.ToString(), out _uomAddEq);
                pdmObj.AdditionalEquipment = _uomAddEq;
            }

            return pdmObj;
        }


        private List<AircraftCharacteristic> GetAircraftCharacteristic(List<Aran.Aim.Features.AircraftCharacteristic> list)
        {
            List<AircraftCharacteristic> res = new List<AircraftCharacteristic>();
            AircraftCategoryType _aircraftCategoryType;
            foreach (var item in list)
            {
                AircraftCharacteristic Characteristic = new AircraftCharacteristic();
                if ((item.AircraftLandingCategory != null) && (item.AircraftLandingCategory.HasValue))
                {
                    Enum.TryParse<AircraftCategoryType>(item.AircraftLandingCategory.Value.ToString(), out _aircraftCategoryType);
                    Characteristic.AircraftLandingCategory = _aircraftCategoryType;
                    res.Add(Characteristic);
                }
            }

            return res;
        }
    }

    public class AIM_StandardInstrumentDeparture_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry>aimGeo)
        {


            Aran.Aim.Features.StandardInstrumentDeparture aimObj = (Aran.Aim.Features.StandardInstrumentDeparture)aimFeature;


            StandardInstrumentDeparture pdmObj = new StandardInstrumentDeparture
            {
                ID = aimObj.Identifier.ToString(),
                //FeatureGUID = Guid.NewGuid().ToString(),
                AirportIdentifier = aimObj.AirportHeliport!=null && aimObj.AirportHeliport.Count>0 ? aimObj.AirportHeliport[0].Feature.Identifier.ToString() : "",
                ProcedureIdentifier = aimObj.Name,
                ProcedureType = PROC_TYPE_code.SID,
                CommunicationFailureDescription = aimObj.CommunicationFailureInstruction!=null ?  aimObj.CommunicationFailureInstruction : "",
                FlightChecked = aimObj.FlightChecked!=null &&  aimObj.FlightChecked.HasValue ? aimObj.FlightChecked.Value : false,
                RNAV = aimObj.RNAV!=null  &&  aimObj.RNAV.HasValue ? aimObj.RNAV.Value : false,
                Designator = aimObj.Designator,
                ContingencyRoute = aimObj.ContingencyRoute!=null && aimObj.ContingencyRoute.HasValue? aimObj.ContingencyRoute.Value :false,
                ID_MasterProc = aimObj.Identifier.ToString(),
                AircraftCharacteristic = GetAircraftCharacteristic(aimObj.AircraftCharacteristic),
                LandingArea = aimObj.Takeoff != null && aimObj.Takeoff.Runway != null && aimObj.Takeoff.Runway.Count > 0 ? new PDMObject { ID = aimObj.Takeoff.Runway[0].Feature.Identifier.ToString() } : null,
                
            };


            ProcedureCodingStandardType _uomCodding;
            if ((aimObj.CodingStandard != null) && (aimObj.CodingStandard.HasValue))
            {
                Enum.TryParse<ProcedureCodingStandardType>(aimObj.CodingStandard.Value.ToString(), out _uomCodding);
                pdmObj.CodingStandard = _uomCodding;
            }



            ProcedureDesignStandardType _uomDesCrit;
            if ((aimObj.DesignCriteria != null) && (aimObj.DesignCriteria.HasValue))
            {
                Enum.TryParse<ProcedureDesignStandardType>(aimObj.DesignCriteria.Value.ToString(), out _uomDesCrit);
                pdmObj.DesignCriteria = _uomDesCrit;
            }


            return pdmObj;
        }

        private List<AircraftCharacteristic> GetAircraftCharacteristic(List<Aran.Aim.Features.AircraftCharacteristic> list)
        {
            List<AircraftCharacteristic> res = new List<AircraftCharacteristic>();
            AircraftCategoryType _aircraftCategoryType;
            foreach (var item in list)
            {
                if ((item.AircraftLandingCategory != null) && (item.AircraftLandingCategory.HasValue))
                {
                    AircraftCharacteristic Characteristic = new AircraftCharacteristic();
                    Enum.TryParse<AircraftCategoryType>(item.AircraftLandingCategory.Value.ToString(), out _aircraftCategoryType);
                    Characteristic.AircraftLandingCategory = _aircraftCategoryType;
                    res.Add(Characteristic);
                }
            }

            return res;
        }
    }

    public class AIM_StandardInstrumentArrival_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry>aimGeo)
        {


            Aran.Aim.Features.StandardInstrumentArrival aimObj = (Aran.Aim.Features.StandardInstrumentArrival)aimFeature;


            StandardInstrumentArrival pdmObj = new StandardInstrumentArrival
            {
                ID = aimObj.Identifier.ToString(),
                //FeatureGUID = Guid.NewGuid().ToString(),
                AirportIdentifier = aimObj.AirportHeliport!=null && aimObj.AirportHeliport.Count>0 ? aimObj.AirportHeliport[0].Feature.Identifier.ToString() : "",
                ProcedureIdentifier = aimObj.Name,
                ProcedureType = PROC_TYPE_code.STAR,
                CommunicationFailureDescription = aimObj.CommunicationFailureInstruction!=null ? aimObj.CommunicationFailureInstruction : "",
                FlightChecked = aimObj.FlightChecked !=null && aimObj.FlightChecked.HasValue ? aimObj.FlightChecked.Value : false,
                RNAV = aimObj.RNAV!=null && aimObj.RNAV.HasValue ? aimObj.RNAV.Value : false,
                Designator = aimObj.Designator,
                ID_MasterProc = aimObj.Identifier.ToString(),
                AircraftCharacteristic = GetAircraftCharacteristic(aimObj.AircraftCharacteristic),
                LandingArea = aimObj.Arrival!=null && aimObj.Arrival.Runway!=null && aimObj.Arrival.Runway.Count>0 ? new PDMObject { ID = aimObj.Arrival.Runway[0].Feature.Identifier.ToString() } : null,
            };


            ProcedureCodingStandardType _uomCodding;
            if ((aimObj.CodingStandard != null) && (aimObj.CodingStandard.HasValue))
            {
                Enum.TryParse<ProcedureCodingStandardType>(aimObj.CodingStandard.Value.ToString(), out _uomCodding);
                pdmObj.CodingStandard = _uomCodding;
            }



            ProcedureDesignStandardType _uomDesCrit;
            if ((aimObj.DesignCriteria != null) && (aimObj.DesignCriteria.HasValue))
            {
                Enum.TryParse<ProcedureDesignStandardType>(aimObj.DesignCriteria.Value.ToString(), out _uomDesCrit);
                pdmObj.DesignCriteria = _uomDesCrit;
            }


            return pdmObj;
        }

        private List<AircraftCharacteristic> GetAircraftCharacteristic(List<Aran.Aim.Features.AircraftCharacteristic> list)
        {
            List<AircraftCharacteristic> res = new List<AircraftCharacteristic>();
            AircraftCategoryType _aircraftCategoryType;
            foreach (var item in list)
            {
                if ((item.AircraftLandingCategory != null) && (item.AircraftLandingCategory.HasValue))
                {
                    AircraftCharacteristic Characteristic = new AircraftCharacteristic();
                    Enum.TryParse<AircraftCategoryType>(item.AircraftLandingCategory.Value.ToString(), out _aircraftCategoryType);
                    Characteristic.AircraftLandingCategory = _aircraftCategoryType;
                    res.Add(Characteristic);
                }
            }

            return res;
        }
    }

    public class AIM_ProcedureTransition_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry>aimGeo)
        {
            Aran.Aim.Features.ProcedureTransition aimObj = (Aran.Aim.Features.ProcedureTransition)aimFeature;

            

            ProcedureTransitions pdmObj = new ProcedureTransitions
            {
                ID = aimObj.Id.ToString(),
                FeatureGUID =Guid.NewGuid().ToString(),
                TransitionIdentifier = aimObj.TransitionId,
                VectorHeading = aimObj.VectorHeading.HasValue? aimObj.VectorHeading.Value : 0,
                Description = aimObj.DepartureRunwayTransition!=null?  aimObj.DepartureRunwayTransition.Runway[0].Id.ToString() : "",
            };


            ProcedurePhaseType _uomPhase;
            if ((aimObj.Type != null) && (aimObj.Type.HasValue))
            {
                Enum.TryParse<ProcedurePhaseType>(aimObj.Type.Value.ToString(), out _uomPhase);
                pdmObj.RouteType = _uomPhase;
            }

            return pdmObj;
        }
    }

    public class AIM_NormalLeg_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry>aimGeo)
        {

            Aran.Aim.Features.SegmentLeg aimObj = (Aran.Aim.Features.SegmentLeg)CreateAIMLeg(aimFeature);

           
            ProcedureLeg pdmObj = new ProcedureLeg();
            if (aimObj is Aran.Aim.Features.FinalLeg) pdmObj = new FinalLeg();
            if (aimObj is Aran.Aim.Features.MissedApproachLeg) pdmObj = new MissaedApproachLeg();

            pdmObj.ID = aimObj.Identifier.ToString();
            pdmObj.AltitudeOverrideATC = aimObj.AltitudeOverrideATC != null ? aimObj.AltitudeOverrideATC.Value : 0;
            pdmObj.BankAngle = aimObj.BankAngle.HasValue ? aimObj.BankAngle.Value : 0;
            pdmObj.Course = aimObj.Course.HasValue ? aimObj.Course.Value : 0;
            pdmObj.Duration = aimObj.Duration != null ? aimObj.Duration.Value : 0;
            //pdmObj.LegTypeARINC = aimObj.LegTypeARINC.ToString();
            pdmObj.LowerLimitAltitude = aimObj.LowerLimitAltitude != null ? aimObj.LowerLimitAltitude.Value : 0;
            pdmObj.SpeedLimit = aimObj.SpeedLimit != null ? aimObj.SpeedLimit.Value : 0;
            pdmObj.UpperLimitAltitude = aimObj.UpperLimitAltitude != null ? aimObj.UpperLimitAltitude.Value : 0;
            pdmObj.VerticalAngle = aimObj.VerticalAngle.HasValue ? aimObj.VerticalAngle.Value : 0;
            if (aimObj.ProcedureTurnRequired.HasValue) pdmObj.ProcedureTurnRequired = aimObj.ProcedureTurnRequired.Value;
            pdmObj.AircraftCategory = GetAircraftCharacteristic(aimObj.AircraftCategory);


            CodeSegmentPath legTp;
            Enum.TryParse<CodeSegmentPath>(aimObj.LegTypeARINC.ToString(), out legTp);
            pdmObj.LegTypeARINC = legTp;

            SegmentLegSpecialization _uomSpec;
            Enum.TryParse<SegmentLegSpecialization>(aimObj.GetType().Name, out _uomSpec);
            pdmObj.LegSpecialization = _uomSpec;

            UOM_DIST_HORZ _uomdistHorz;

            #region FinalLeg

            if (aimObj is Aran.Aim.Features.FinalLeg)
            {
                FinalLeg pdmObj_FinalLeg = (FinalLeg)pdmObj;
                Aran.Aim.Features.FinalLeg aimObj_FinalLeg = (Aran.Aim.Features.FinalLeg)aimObj;

                pdmObj_FinalLeg.ID_ProcedreLeg = pdmObj.ID;

                if (aimObj_FinalLeg.MinimumBaroVnavTemperature != null)
                {
                    pdmObj_FinalLeg.MinimumBaroVnavTemperature = aimObj_FinalLeg.MinimumBaroVnavTemperature.Value;
                    Uom_Temperature _uomTemp;
                    Enum.TryParse<Uom_Temperature>(aimObj_FinalLeg.MinimumBaroVnavTemperature.Uom.ToString(), out _uomTemp);
                    pdmObj_FinalLeg.MinimumBaroVnavTemperatureUOM = _uomTemp;
                }

                if (aimObj_FinalLeg.RnpDMEAuthorized.HasValue) pdmObj_FinalLeg.RnpDMEAuthorized = aimObj_FinalLeg.RnpDMEAuthorized.Value;
                if (aimObj_FinalLeg.CourseOffsetAngle.HasValue) pdmObj_FinalLeg.CourseOffsetAngle = aimObj_FinalLeg.CourseOffsetAngle.Value;


                if (aimObj_FinalLeg.CourseOffsetDistance != null)
                {
                    pdmObj_FinalLeg.CourseOffsetDistance = aimObj_FinalLeg.CourseOffsetDistance.Value;

                    Enum.TryParse<UOM_DIST_HORZ>(aimObj_FinalLeg.CourseOffsetDistance.Uom.ToString(), out _uomdistHorz);
                    pdmObj_FinalLeg.CourseOffsetDistanceUOM = _uomdistHorz;
                }

                if (aimObj_FinalLeg.CourseCentrelineDistance != null)
                {
                    pdmObj_FinalLeg.CourseCentrelineDistance = aimObj_FinalLeg.CourseCentrelineDistance.Value;
                    Enum.TryParse<UOM_DIST_HORZ>(aimObj_FinalLeg.CourseCentrelineDistance.Uom.ToString(), out _uomdistHorz);
                    pdmObj_FinalLeg.CourseCentrelineDistanceUOM = _uomdistHorz;
                }

                if (aimObj_FinalLeg.GuidanceSystem.HasValue)
                {
                    CodeFinalGuidance _uomFinGued;
                    Enum.TryParse<CodeFinalGuidance>(aimObj_FinalLeg.GuidanceSystem.ToString(), out _uomFinGued);
                    pdmObj_FinalLeg.GuidanceSystem = _uomFinGued;
                }

                if (aimObj_FinalLeg.LandingSystemCategory.HasValue)
                {
                    CodeApproachGuidance _uomAppGued;
                    Enum.TryParse<CodeApproachGuidance>(aimObj_FinalLeg.LandingSystemCategory.ToString(), out _uomAppGued);
                    pdmObj_FinalLeg.LandingSystemCategory = _uomAppGued;
                }

                if (aimObj_FinalLeg.CourseOffsetSide.HasValue)
                {
                    CodeSide _uomSide;
                    Enum.TryParse<CodeSide>(aimObj_FinalLeg.CourseOffsetSide.ToString(), out _uomSide);
                    pdmObj_FinalLeg.CourseOffsetSide = _uomSide;
                }

                if (aimObj_FinalLeg.CourseCentrelineIntersect.HasValue)
                {
                    CodeRelativePosition _uomRelPos;
                    Enum.TryParse<CodeRelativePosition>(aimObj_FinalLeg.CourseCentrelineIntersect.ToString(), out _uomRelPos);
                    pdmObj_FinalLeg.CourseCentrelineIntersect = _uomRelPos;
                }

            }

            #endregion

            #region MissedApproachLeg

            if (aimObj is Aran.Aim.Features.MissedApproachLeg)
            {
                MissaedApproachLeg pdmObj_MissedLeg = (MissaedApproachLeg)pdmObj;
                Aran.Aim.Features.MissedApproachLeg aimObj_MissidLeg = (Aran.Aim.Features.MissedApproachLeg)aimObj;

                pdmObj_MissedLeg.ID = pdmObj.ID;

                if (aimObj_MissidLeg.Type.HasValue)
                {
                    CodeMissedApproach _uomMissedApp;
                    Enum.TryParse<CodeMissedApproach>(aimObj_MissidLeg.Type.Value.ToString(), out _uomMissedApp);

                    pdmObj_MissedLeg.Type = _uomMissedApp;
                }

                if (aimObj_MissidLeg.ThresholdAfterMAPT.HasValue)
                {
                    pdmObj_MissedLeg.ThresholdAfterMAPT = aimObj_MissidLeg.ThresholdAfterMAPT.Value;
                }

                if (aimObj_MissidLeg.HeightMAPT != null)
                {
                    pdmObj_MissedLeg.HeightMAPT = aimObj_MissidLeg.HeightMAPT.Value;

                    Enum.TryParse<UOM_DIST_HORZ>(aimObj_MissidLeg.HeightMAPT.Uom.ToString(), out _uomdistHorz);
                    pdmObj_MissedLeg.HeightMAPTUOM = _uomdistHorz;
                }

                if (aimObj_MissidLeg.RequiredNavigationPerformance.HasValue)
                {
                    pdmObj_MissedLeg.RequiredNavigationPerformance = aimObj_MissidLeg.RequiredNavigationPerformance.Value;
                }
            }

            #endregion

            #region UOM properties

            AltitudeUseType _uomAltitudeUse;
            if ((aimObj.AltitudeInterpretation != null) && (aimObj.AltitudeInterpretation.HasValue))
            {
                Enum.TryParse<AltitudeUseType>(aimObj.AltitudeInterpretation.Value.ToString(), out _uomAltitudeUse);
                pdmObj.AltitudeInterpretation = _uomAltitudeUse;
            }

            UOM_DIST_VERT _uomAltitude;
            if (aimObj.AltitudeOverrideATC != null)
            {
                Enum.TryParse<UOM_DIST_VERT>(aimObj.AltitudeOverrideATC.Uom.ToString(), out _uomAltitude);
                pdmObj.AltitudeUOM = _uomAltitude;
            }

            CodeDirectionReference _uomDir;
            if (aimObj.CourseDirection.HasValue)
            {
                Enum.TryParse<CodeDirectionReference>(aimObj.CourseDirection.Value.ToString(), out _uomDir);
                pdmObj.CourseDirection = _uomDir;
            }

            BearingType _uomBearingType;
            if (aimObj.CourseType.HasValue)
            {
                Enum.TryParse<BearingType>(aimObj.CourseType.Value.ToString(), out _uomBearingType);
                pdmObj.CourseType = _uomBearingType;
            }

            DurationType _uomDuration;
            if (aimObj.Duration != null)
            {
                Enum.TryParse<DurationType>(aimObj.Duration.Uom.ToString(), out _uomDuration);
                pdmObj.DurationUOM = _uomDuration;
            }

            TrajectoryType _uomTrajectoryType;
            if (aimObj.LegPath.HasValue)
            {
                Enum.TryParse<TrajectoryType>(aimObj.LegPath.Value.ToString(), out _uomTrajectoryType);
                pdmObj.LegPathField = _uomTrajectoryType;
            }


            if (aimObj.LowerLimitAltitude != null)
            {
                Enum.TryParse<UOM_DIST_VERT>(aimObj.LowerLimitAltitude.Uom.ToString(), out _uomAltitude);
                pdmObj.LowerLimitAltitudeUOM = _uomAltitude;
            }

            CodeVerticalReference _uomLowerLimitReference;
            if ((aimObj.LowerLimitReference != null) && (aimObj.LowerLimitReference.HasValue))
            {
                Enum.TryParse<CodeVerticalReference>(aimObj.LowerLimitReference.Value.ToString(), out _uomLowerLimitReference);
                pdmObj.LowerLimitReference = _uomLowerLimitReference;
            }

            SpeedType _uomSpeed;
            if (aimObj.SpeedLimit != null)
            {
                Enum.TryParse<SpeedType>(aimObj.SpeedLimit.Uom.ToString(), out _uomSpeed);
                pdmObj.SpeedUOM = _uomSpeed;
            }

            CodeSpeedReference _speedReference;
            if ((aimObj.SpeedReference != null) && (aimObj.SpeedReference.HasValue))
            {
                Enum.TryParse<CodeSpeedReference>(aimObj.SpeedReference.Value.ToString(), out _speedReference);
                pdmObj.SpeedReference = _speedReference;
            }

            DirectionTurnType _uomDirectionTurn;
            if (aimObj.TurnDirection.HasValue)
            {
                Enum.TryParse<DirectionTurnType>(aimObj.TurnDirection.ToString(), out _uomDirectionTurn);
                pdmObj.TurnDirection = _uomDirectionTurn;
            }


            if (aimObj.UpperLimitAltitude != null)
            {
                Enum.TryParse<UOM_DIST_VERT>(aimObj.UpperLimitAltitude.Uom.ToString(), out _uomAltitude);
                pdmObj.UpperLimitAltitudeUOM = _uomAltitude;
            }


            CodeVerticalReference _uomUpperLimitReference;
            if ((aimObj.UpperLimitReference != null) && (aimObj.UpperLimitReference.HasValue))
            {
                Enum.TryParse<CodeVerticalReference>(aimObj.UpperLimitReference.Value.ToString(), out _uomUpperLimitReference);
                pdmObj.UpperLimitReference = _uomUpperLimitReference;
            }

            if (aimObj.EndConditionDesignator.HasValue)
            {
                CodeSegmentTermination _uomSegTermination;
                Enum.TryParse<CodeSegmentTermination>(aimObj.EndConditionDesignator.Value.ToString(), out _uomSegTermination);
                pdmObj.EndConditionDesignator = _uomSegTermination;
            }

            #endregion

            if (aimObj.StartPoint != null)
            {

                #region Fill startpoints properies
                SegmentPoint _strtPnt = new SegmentPoint();

                
                _strtPnt.ID = Guid.NewGuid().ToString();
                _strtPnt.Route_LEG_ID = pdmObj.ID;
                _strtPnt.PointUse = ProcedureSegmentPointUse.START_POINT;

                if (aimObj.StartPoint.RadarGuidance.HasValue) { _strtPnt.RadarGuidance = aimObj.StartPoint.RadarGuidance.Value; }

                if (aimObj.StartPoint.ReportingATC.HasValue)
                {
                    CodeATCReporting _uomATCRRep;
                    Enum.TryParse<CodeATCReporting>(aimObj.StartPoint.ReportingATC.Value.ToString(), out _uomATCRRep);
                    _strtPnt.ReportingATC = _uomATCRRep;
                }

                if (aimObj.StartPoint.Waypoint.HasValue) { _strtPnt.IsWaypoint = aimObj.StartPoint.Waypoint.Value; }

                if (aimObj.StartPoint.FlyOver.HasValue) { _strtPnt.FlyOver = aimObj.StartPoint.FlyOver.Value; }

                if (aimObj.StartPoint.Role.HasValue)
                {
                    ProcedureFixRoleType _uom;
                    Enum.TryParse<ProcedureFixRoleType>(aimObj.StartPoint.Role.Value.ToString(), out _uom);
                    _strtPnt.PointRole = _uom;
                }

                if (aimObj.StartPoint.LeadRadial.HasValue) { _strtPnt.LeadRadial = aimObj.StartPoint.LeadRadial.Value; }

                if (aimObj.StartPoint.LeadDME != null)
                {
                    _strtPnt.LeadDME = aimObj.StartPoint.LeadDME.Value;
                    UOM_DIST_HORZ _uom;
                    Enum.TryParse<UOM_DIST_HORZ>(aimObj.StartPoint.LeadDME.Uom.ToString(), out _uom);
                    _strtPnt.LeadDMEUOM = _uom;
                }

                if (aimObj.StartPoint.IndicatorFACF.HasValue) { _strtPnt.IndicatorFACF = aimObj.StartPoint.IndicatorFACF.Value; }

                if (aimObj.StartPoint.PointChoice != null)
                {
                    PointChoice _uom;
                    Enum.TryParse<PointChoice>(aimObj.StartPoint.PointChoice.Choice.ToString(), out _uom);
                    _strtPnt.PointChoice = _uom;

                    switch (_strtPnt.PointChoice)
                    {
                        case PointChoice.DesignatedPoint:
                            _strtPnt.PointChoiceID = aimObj.StartPoint.PointChoice.FixDesignatedPoint.Identifier.ToString();
                            break;
                        case PointChoice.Navaid:
                            _strtPnt.PointChoiceID = aimObj.StartPoint.PointChoice.NavaidSystem.Identifier.ToString();
                            break;
                        case PointChoice.RunwayCenterlinePoint:
                            _strtPnt.PointChoiceID = aimObj.StartPoint.PointChoice.RunwayPoint.Identifier.ToString();
                            break;
                        case PointChoice.AirportHeliport:
                            _strtPnt.PointChoiceID = aimObj.StartPoint.PointChoice.AirportReferencePoint.Identifier.ToString();
                            break;
                        case PointChoice.NONE:
                            _strtPnt.PointChoiceID = "";
                            break;
                        default:
                            _strtPnt.PointChoiceID = aimObj.StartPoint.PointChoice.FixDesignatedPoint.Identifier.ToString();
                            break;
                    }


                }

                #endregion

                #region FacilityMakeUp

                if ((aimObj.StartPoint.FacilityMakeup != null) && (aimObj.StartPoint.FacilityMakeup.Count > 0))
                {
                    FacilityMakeUp _FacilityMakeUp = CreateFacilityMakeUp(aimObj.StartPoint.FacilityMakeup[0]);
                    _FacilityMakeUp.SegmentPointID = _strtPnt.ID;
                    _strtPnt.PointFacilityMakeUp = _FacilityMakeUp;


                    if ((aimObj.StartPoint.FacilityMakeup[0].FixToleranceArea!=null) && (aimObj.StartPoint.FacilityMakeup[0].FixToleranceArea.Geo !=null)) 
                    {
                        var wksPointsList = new List<WKSPoint>();

                        foreach (var item in aimObj.StartPoint.FacilityMakeup[0].FixToleranceArea.Geo.ToMultiPoint().ToList())
                        {
                            WKSPoint pnt = new WKSPoint();
                            pnt.X = item.X;
                            pnt.Y = item.Y;
                            wksPointsList.Add(pnt);
                        }

                        WKSPoint[] pointArray = wksPointsList.ToArray();

                        PolygonClass  _Border = new PolygonClass();
                        IGeometryBridge2 geometryBridge = new GeometryEnvironmentClass();

                        geometryBridge.AddWKSPoints(_Border, ref pointArray);
                        _Border.Simplify();

                        _FacilityMakeUp.Geo = _Border;
                        
                    }
                }

                #endregion

                pdmObj.StartPoint = _strtPnt;
               
             }

            if (aimObj.EndPoint != null)
            {

                #region Fill endpoints properies

                SegmentPoint _endPnt = new SegmentPoint();


                _endPnt.ID = Guid.NewGuid().ToString();
                _endPnt.Route_LEG_ID = pdmObj.ID;
                _endPnt.PointUse = ProcedureSegmentPointUse.END_POINT;

                if (aimObj.EndPoint.RadarGuidance.HasValue) { _endPnt.RadarGuidance = aimObj.EndPoint.RadarGuidance.Value; }

                if (aimObj.EndPoint.ReportingATC.HasValue)
                {
                    CodeATCReporting _uomATCRRep;
                    Enum.TryParse<CodeATCReporting>(aimObj.EndPoint.ReportingATC.Value.ToString(), out _uomATCRRep);
                    _endPnt.ReportingATC = _uomATCRRep;
                }

                if (aimObj.EndPoint.Waypoint.HasValue) { _endPnt.IsWaypoint = aimObj.EndPoint.Waypoint.Value; }

                if (aimObj.EndPoint.FlyOver.HasValue) { _endPnt.FlyOver = aimObj.EndPoint.FlyOver.Value; }

                if (aimObj.EndPoint.Role.HasValue)
                {
                    ProcedureFixRoleType _uom;
                    Enum.TryParse<ProcedureFixRoleType>(aimObj.EndPoint.Role.Value.ToString(), out _uom);
                    _endPnt.PointRole = _uom;
                }

                if (aimObj.EndPoint.LeadRadial.HasValue) { _endPnt.LeadRadial = aimObj.EndPoint.LeadRadial.Value; }

                if (aimObj.EndPoint.LeadDME != null)
                {
                    _endPnt.LeadDME = aimObj.EndPoint.LeadDME.Value;
                    UOM_DIST_HORZ _uom;
                    Enum.TryParse<UOM_DIST_HORZ>(aimObj.EndPoint.LeadDME.Uom.ToString(), out _uom);
                    _endPnt.LeadDMEUOM = _uom;
                }

                if (aimObj.EndPoint.IndicatorFACF.HasValue) { _endPnt.IndicatorFACF = aimObj.EndPoint.IndicatorFACF.Value; }

                if (aimObj.EndPoint.PointChoice != null)
                {
                    PointChoice _uom;
                    Enum.TryParse<PointChoice>(aimObj.EndPoint.PointChoice.Choice.ToString(), out _uom);
                    _endPnt.PointChoice = _uom;

                    switch (_endPnt.PointChoice)
                    {
                        case PointChoice.DesignatedPoint:
                            _endPnt.PointChoiceID = aimObj.EndPoint.PointChoice.FixDesignatedPoint.Identifier.ToString();
                            break;
                        case PointChoice.Navaid:
                            _endPnt.PointChoiceID = aimObj.EndPoint.PointChoice.NavaidSystem.Identifier.ToString();
                            break;
                        case PointChoice.RunwayCenterlinePoint:
                            _endPnt.PointChoiceID = aimObj.EndPoint.PointChoice.RunwayPoint.Identifier.ToString();
                            break;
                        case PointChoice.AirportHeliport:
                            _endPnt.PointChoiceID = aimObj.EndPoint.PointChoice.AirportReferencePoint.Identifier.ToString();
                            break;
                        case PointChoice.NONE:
                            _endPnt.PointChoiceID = "";
                            break;
                        default:
                            _endPnt.PointChoiceID = aimObj.EndPoint.PointChoice.FixDesignatedPoint.Identifier.ToString();
                            break;
                    }


                }

                #endregion

                #region FacilityMakeUp

                if ((aimObj.EndPoint.FacilityMakeup != null) && (aimObj.EndPoint.FacilityMakeup.Count > 0))
                {
                    FacilityMakeUp _FacilityMakeUp = CreateFacilityMakeUp(aimObj.EndPoint.FacilityMakeup[0]);
                    _FacilityMakeUp.SegmentPointID = _endPnt.ID;
                    _endPnt.PointFacilityMakeUp = _FacilityMakeUp;

                }

                #endregion

                pdmObj.EndPoint = _endPnt;

            }


            if (aimObj.ArcCentre != null)
            {

                #region Fill arcpoints properies

                SegmentPoint _arcPnt = new SegmentPoint();


                _arcPnt.ID = Guid.NewGuid().ToString();
                _arcPnt.Route_LEG_ID = pdmObj.ID;
                _arcPnt.PointUse = ProcedureSegmentPointUse.ARC_CENTER;

                if (aimObj.ArcCentre.RadarGuidance.HasValue) { _arcPnt.RadarGuidance = aimObj.ArcCentre.RadarGuidance.Value; }

                if (aimObj.ArcCentre.ReportingATC.HasValue)
                {
                    CodeATCReporting _uomATCRRep;
                    Enum.TryParse<CodeATCReporting>(aimObj.ArcCentre.ReportingATC.Value.ToString(), out _uomATCRRep);
                    _arcPnt.ReportingATC = _uomATCRRep;
                }

                if (aimObj.ArcCentre.Waypoint.HasValue) { _arcPnt.IsWaypoint = aimObj.ArcCentre.Waypoint.Value; }

                if (aimObj.ArcCentre.FlyOver.HasValue) { _arcPnt.FlyOver = aimObj.ArcCentre.FlyOver.Value; }

                if (aimObj.ArcCentre.Role.HasValue)
                {
                    ProcedureFixRoleType _uom;
                    Enum.TryParse<ProcedureFixRoleType>(aimObj.ArcCentre.Role.Value.ToString(), out _uom);
                    _arcPnt.PointRole = _uom;
                }

                if (aimObj.ArcCentre.LeadRadial.HasValue) { _arcPnt.LeadRadial = aimObj.ArcCentre.LeadRadial.Value; }

                if (aimObj.ArcCentre.LeadDME != null)
                {
                    _arcPnt.LeadDME = aimObj.ArcCentre.LeadDME.Value;
                    UOM_DIST_HORZ _uom;
                    Enum.TryParse<UOM_DIST_HORZ>(aimObj.ArcCentre.LeadDME.Uom.ToString(), out _uom);
                    _arcPnt.LeadDMEUOM = _uom;
                }

                if (aimObj.ArcCentre.IndicatorFACF.HasValue) { _arcPnt.IndicatorFACF = aimObj.ArcCentre.IndicatorFACF.Value; }

                if (aimObj.ArcCentre.PointChoice != null)
                {
                    PointChoice _uom;
                    Enum.TryParse<PointChoice>(aimObj.ArcCentre.PointChoice.Choice.ToString(), out _uom);
                    _arcPnt.PointChoice = _uom;

                    switch (_arcPnt.PointChoice)
                    {
                        case PointChoice.DesignatedPoint:
                            _arcPnt.PointChoiceID = aimObj.ArcCentre.PointChoice.FixDesignatedPoint.Identifier.ToString();
                            break;
                        case PointChoice.Navaid:
                            _arcPnt.PointChoiceID = aimObj.ArcCentre.PointChoice.NavaidSystem.Identifier.ToString();
                            break;
                        case PointChoice.RunwayCenterlinePoint:
                            _arcPnt.PointChoiceID = aimObj.ArcCentre.PointChoice.RunwayPoint.Identifier.ToString();
                            break;
                        case PointChoice.AirportHeliport:
                            _arcPnt.PointChoiceID = aimObj.ArcCentre.PointChoice.AirportReferencePoint.Identifier.ToString();
                            break;
                        case PointChoice.NONE:
                            _arcPnt.PointChoiceID = "";
                            break;
                        default:
                            _arcPnt.PointChoiceID = aimObj.ArcCentre.PointChoice.FixDesignatedPoint.Identifier.ToString();
                            break;
                    }


                }

                #endregion

                #region FacilityMakeUp

                if ((aimObj.ArcCentre.FacilityMakeup != null) && (aimObj.ArcCentre.FacilityMakeup.Count > 0))
                {
                    FacilityMakeUp _arcFacilityMakeUp = CreateFacilityMakeUp(aimObj.ArcCentre.FacilityMakeup[0]);
                    _arcFacilityMakeUp.SegmentPointID = _arcPnt.ID;
                    _arcPnt.PointFacilityMakeUp = _arcFacilityMakeUp;

                }

                #endregion

                pdmObj.ArcCentre = _arcPnt;

            }


            var zAware = aimGeo[0] as IZAware;
            zAware.ZAware = true;

            var mAware = aimGeo[0] as IMAware;
            mAware.MAware = true;

            pdmObj.Geo = aimGeo[0];
            return pdmObj;
        }

        private FacilityMakeUp CreateFacilityMakeUp(Aran.Aim.Features.PointReference aim_FacilityMakeUp)
        {
            FacilityMakeUp pdm_FacilityMakeUp = new FacilityMakeUp();
            pdm_FacilityMakeUp.ID = Guid.NewGuid().ToString();

            CodeReferenceRole _uomRole;
            if (aim_FacilityMakeUp.Role.HasValue)
            {
                Enum.TryParse<CodeReferenceRole>(aim_FacilityMakeUp.Role.Value.ToString(), out _uomRole);
                pdm_FacilityMakeUp.Role = _uomRole;
            }

            #region AngleIndication

            if ((aim_FacilityMakeUp.FacilityAngle != null) && (aim_FacilityMakeUp.FacilityAngle.Count > 0))
            {
                pdm_FacilityMakeUp.AngleIndication = new AngleIndication();

                if (aim_FacilityMakeUp.FacilityAngle[0].AlongCourseGuidance.HasValue) pdm_FacilityMakeUp.AngleIndication.AlongCourseGuidance = aim_FacilityMakeUp.FacilityAngle[0].AlongCourseGuidance.Value;

                pdm_FacilityMakeUp.AngleIndication.ID = aim_FacilityMakeUp.FacilityAngle[0].TheAngleIndication.Identifier.ToString();
                pdm_FacilityMakeUp.AngleIndication.FacilityMakeUp_ID = pdm_FacilityMakeUp.ID;
            }

            #endregion

            #region DistanceIndication

            if ((aim_FacilityMakeUp.FacilityDistance != null) && (aim_FacilityMakeUp.FacilityDistance.Count > 0))
            {

                pdm_FacilityMakeUp.DistanceIndication = new DistanceIndication();

                pdm_FacilityMakeUp.DistanceIndication.ID = aim_FacilityMakeUp.FacilityDistance[0].Feature.Identifier.ToString();
                pdm_FacilityMakeUp.DistanceIndication.FacilityMakeUp_ID = pdm_FacilityMakeUp.ID;

            }

            #endregion

            if (aim_FacilityMakeUp.FixToleranceArea != null)
            {
              
               // aim_FacilityMakeUp.FixToleranceArea.Geo[0]
            }

            return pdm_FacilityMakeUp;
        }


        private object CreateAIMLeg(object _aimLeg)
        {
            object res =  null;
            if (_aimLeg is Aran.Aim.Features.ArrivalFeederLeg) { res = (Aran.Aim.Features.ArrivalFeederLeg)_aimLeg; }
            if (_aimLeg is Aran.Aim.Features.ArrivalLeg) { res = (Aran.Aim.Features.ArrivalLeg)_aimLeg; }
            if (_aimLeg is Aran.Aim.Features.ApproachLeg) { res = (Aran.Aim.Features.ApproachLeg)_aimLeg; }
            if (_aimLeg is Aran.Aim.Features.DepartureLeg) { res = (Aran.Aim.Features.DepartureLeg)_aimLeg; }
            if (_aimLeg is Aran.Aim.Features.InitialLeg) { res = (Aran.Aim.Features.InitialLeg)_aimLeg; }
            if (_aimLeg is Aran.Aim.Features.IntermediateLeg) { res = (Aran.Aim.Features.IntermediateLeg)_aimLeg; }


            if (_aimLeg is Aran.Aim.Features.FinalLeg) { res = (Aran.Aim.Features.FinalLeg)_aimLeg; }
            if (_aimLeg is Aran.Aim.Features.MissedApproachLeg) { res = (Aran.Aim.Features.MissedApproachLeg)_aimLeg; }

            if (_aimLeg == null)
                System.Diagnostics.Debug.WriteLine("_aimLeg = null");
            
            return res;
        }

        private List<AircraftCharacteristic> GetAircraftCharacteristic(List<Aran.Aim.Features.AircraftCharacteristic> list)
        {
            List<AircraftCharacteristic> res = new List<AircraftCharacteristic>();
            AircraftCategoryType _aircraftCategoryType;
            foreach (var item in list)
            {
                AircraftCharacteristic Characteristic = new AircraftCharacteristic();
                Enum.TryParse<AircraftCategoryType>(item.AircraftLandingCategory.Value.ToString(), out _aircraftCategoryType);
                Characteristic.AircraftLandingCategory = _aircraftCategoryType;
                res.Add(Characteristic);
            }

            return res;
        }
    }

    public class AIM_VerticalStructure_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry>aimGeo)
        {
            Aran.Aim.Features.VerticalStructure aimObj = (Aran.Aim.Features.VerticalStructure)aimFeature;

            VerticalStructure pdmObj = new VerticalStructure
            {
                ID = aimObj.Identifier.ToString(),
                Name = (aimObj.Name != null && aimObj.Name.Length > 0) ? aimObj.Name : "NONAME",
                Lighted = aimObj.Lighted.HasValue ? aimObj.Lighted.Value : false,
                Group = aimObj.Group.HasValue ? aimObj.Group.Value : false,
                MarkingICAOStandard = aimObj.MarkingICAOStandard.HasValue ? aimObj.MarkingICAOStandard.Value : false,
                LightingICAOStandard = aimObj.LightingICAOStandard.HasValue ? aimObj.LightingICAOStandard.Value : false,
                SynchronisedLighting = aimObj.SynchronisedLighting.HasValue ? aimObj.SynchronisedLighting.Value : false,
            };

            VerticalStructureType strucType;
            if (Enum.TryParse<VerticalStructureType>(aimObj.Type.ToString(), out strucType)) pdmObj.Type = strucType;


            UOM_DIST_HORZ _uomdistHor;
            if (aimObj.Length != null)
            {
                pdmObj.Length = aimObj.Length.Value;
                if (Enum.TryParse<UOM_DIST_HORZ>(aimObj.Length.Uom.ToString(), out _uomdistHor)) pdmObj.Length_UOM = _uomdistHor;
            }

            if (aimObj.Width != null)
            {
                pdmObj.Width = aimObj.Width.Value;
                if (Enum.TryParse<UOM_DIST_HORZ>(aimObj.Width.Uom.ToString(), out _uomdistHor)) pdmObj.Width_UOM = _uomdistHor;
            }

            if (aimObj.Radius != null)
            {
                pdmObj.Radius = aimObj.Radius.Value;
                if (Enum.TryParse<UOM_DIST_HORZ>(aimObj.Radius.Uom.ToString(), out _uomdistHor)) pdmObj.Radius_UOM = _uomdistHor;
            }

            pdmObj.Parts = new List<VerticalStructurePart>();

            for (int i = 0; i <= aimObj.Part.Count-1; i++)
            {
                VerticalStructurePart obs = AIM_VerticalStructurePart_PDM_Converter(aimObj.Part[i], aimGeo[i]);
                if (obs != null)
                {
                    obs.VerticalStructure_ID = pdmObj.ID;
                    pdmObj.Parts.Add(obs);
                }
            }

            return pdmObj;
        }

        private VerticalStructurePart AIM_VerticalStructurePart_PDM_Converter(Aran.Aim.Features.VerticalStructurePart item, IGeometry aimGeo)
        {
            VerticalStructurePart pdmObj = new VerticalStructurePart
            {
                ID = Guid.NewGuid().ToString(),
                Designator = (item.Designator != null && item.Designator.Length > 0) ? item.Designator : "NONAME",
                Mobile = item.Mobile.HasValue ? item.Mobile.Value : false,
                Frangible = item.Frangible.HasValue ? item.Frangible.Value : false,
            };

            VerticalStructureType strucType;
            if (Enum.TryParse<VerticalStructureType>(item.Type.ToString(), out strucType)) pdmObj.Type = strucType;

            StatusConstructionType _uomStatus;
            if (Enum.TryParse<StatusConstructionType>(item.ConstructionStatus.ToString(), out _uomStatus)) pdmObj.ConstructionStatus = _uomStatus;

            VerticalStructureMarkingType _uomMarking;
            if (Enum.TryParse<VerticalStructureMarkingType>(item.MarkingPattern.ToString(), out _uomMarking)) pdmObj.MarkingPattern = _uomMarking;

            ColourType _uomColor;
            if (Enum.TryParse<ColourType>(item.MarkingFirstColour.ToString(), out _uomColor)) pdmObj.MarkingFirstColour = _uomColor;
            if (Enum.TryParse<ColourType>(item.MarkingSecondColour.ToString(), out _uomColor)) pdmObj.MarkingSecondColour = _uomColor;

            VerticalStructureMaterialType _uomMaterial;
            if (Enum.TryParse<VerticalStructureMaterialType>(item.VisibleMaterial.ToString(), out _uomMaterial)) pdmObj.VisibleMaterial = _uomMaterial;


            if ((item.HorizontalProjection != null) && (item.HorizontalProjection.ObjectType == Aran.Aim.ObjectType.ElevatedPoint) && (item.HorizontalProjection.Location.Elevation != null))
            {
                pdmObj.Elev = item.HorizontalProjection.Location.Elevation.Value;
                UOM_DIST_VERT _uomVert;
                if (Enum.TryParse<UOM_DIST_VERT>(item.HorizontalProjection.Location.Elevation.Uom.ToString(), out _uomVert)) pdmObj.Elev_UOM = _uomVert;

            }

            if (item.VerticalExtent!=null)
            {
               pdmObj.VerticalExtent = item.VerticalExtent.Value;
                UOM_DIST_VERT _uomVert;
                if (Enum.TryParse<UOM_DIST_VERT>(item.VerticalExtent.Uom.ToString(), out _uomVert)) pdmObj.VerticalExtent_UOM = _uomVert;
            }

            if (item.VerticalExtentAccuracy!=null)
            {
               pdmObj.VerticalExtentAccuracy = item.VerticalExtentAccuracy.Value;
                UOM_DIST_VERT _uomVert;
                if (Enum.TryParse<UOM_DIST_VERT>(item.VerticalExtentAccuracy.Uom.ToString(), out _uomVert)) pdmObj.VerticalExtentAccuracy_UOM = _uomVert;
            }

            var zAware = aimGeo as IZAware;
            zAware.ZAware = true;

            if (aimGeo.GeometryType == esriGeometryType.esriGeometryPoint) ((IPoint)aimGeo).Z = pdmObj.ConvertValueToMeter(pdmObj.Elev, pdmObj.Elev_UOM.ToString());
            //if (aimGeo.GeometryType == esriGeometryType.esriGeometry) ((IPoint)aimGeo).Z = pdmObj.ConvertValueToMeter(pdmObj.Elev, pdmObj.Elev_UOM.ToString());


            var mAware = aimGeo as IMAware;
            mAware.MAware = true;

            pdmObj.Geo = aimGeo;

            return pdmObj;
        }
    }

}
