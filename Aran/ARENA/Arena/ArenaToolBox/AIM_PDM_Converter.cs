using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PDM;
using Aran.Temporality.Common.ArcGis;
using ESRI.ArcGIS.Geometry;
using System.Reflection;
using Aran.Aim;
using ESRI.ArcGIS.esriSystem;
using EsriWorkEnvironment;
using AranSupport;
using Aran.Converters;
using Aran.Geometries;
using ArenaLogManager;

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
            elementTypes.Add(typeof(Aran.Aim.Features.RunwayElement), typeof(AIM_RwyElement_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.Taxiway), typeof(AIM_TWY_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.TaxiwayElement), typeof(AIM_TwyElement_PDM_Converter));

            elementTypes.Add(typeof(Aran.Aim.Features.Apron), typeof(AIM_APRON_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.ApronMarking), typeof(AIM_ApronMarking_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.ApronElement), typeof(AIM_ApronElement_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.MarkingElement), typeof(AIM_MarkingElement_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.ApronLightSystem), typeof(AIM_APRON_LIGHT_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.RunwayMarking), typeof(AIM_RunwayMarking_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.AircraftStand), typeof(AIM_AircraftStand_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.SurfaceCharacteristics), typeof(AIM_SurfaceCharacteristics_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.StandMarking), typeof(AIM_StandMarking_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.GuidanceLine), typeof(AIM_GuidanceLine_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.GuidanceLineLightSystem), typeof(AIM_GuidanceLine_LIGHT_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.TaxiwayLightSystem), typeof(AIM_Taxiway_LIGHT_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.GuidanceLineMarking), typeof(AIM_GuidanceLineMarking_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.RadioCommunicationChannel), typeof(AIM_RadioCommunicationChanel_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.TaxiwayMarking), typeof(AIM_TaxiwayMarking_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.RunwayProtectArea), typeof(AIM_RwyProtectionArea_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.RunwayProtectAreaLightSystem), typeof(AIM_RPA_LIGHT_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.RunwayVisualRange), typeof(AIM_RwyVisualRange_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.AirportHotSpot), typeof(AIM_AirportHotSpot_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.TaxiHoldingPosition), typeof(AIM_TaxiHoldingPosition_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.TaxiHoldingPositionLightSystem), typeof(AIM_TaxiHolding_LIGHT_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.TaxiHoldingPositionMarking), typeof(AIM_TaxiHoldingMarking_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.VisualGlideSlopeIndicator), typeof(AIM_VisualGlideSlope_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.TouchDownLiftOff), typeof(AIM_TouchDownLiftOff_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.TouchDownLiftOffMarking), typeof(AIM_TouchDownLiftOffMarking_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.TouchDownLiftOffLightSystem), typeof(AIM_TouchDown_LIGHT_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.TouchDownLiftOffSafeArea), typeof(AIM_TouchDownSafeArea_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.WorkArea), typeof(AIM_WorkArea_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.Road), typeof(AIM_Road_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.Unit), typeof(AIM_Unit_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.NonMovementArea), typeof(AIM_NonMovementArea_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.DeicingArea), typeof(AIM_DeicingArea_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.DeicingAreaMarking), typeof(AIM_DeicingAreaMarking_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.RadioFrequencyArea), typeof(AIM_RadioFrequencyArea_PDM_Converter));


            elementTypes.Add(typeof(Aran.Aim.Features.Navaid), typeof(AIM_NAVAID_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.DME), typeof(AIM_DME_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.VOR), typeof(AIM_VOR_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.NDB), typeof(AIM_NDB_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.MarkerBeacon), typeof(AIM_Marker_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.TACAN), typeof(AIM_TACAN_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.Localizer), typeof(AIM_Localizer_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.Glidepath), typeof(AIM_Glidepath_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.RunwayCentrelinePoint), typeof(AIM_CLP_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.ApproachLightingSystem), typeof(AIM_APPROACH_LIGHT_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.CheckpointINS), typeof(AIM_CheckpointINS_PDM_Converter));
            elementTypes.Add(typeof(Aran.Aim.Features.CheckpointVOR), typeof(AIM_CheckpointVOR_PDM_Converter));

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
            elementTypes.Add(typeof(Aran.Aim.Features.HoldingPattern), typeof(AIM_Holding_PDM_Converter));

            elementTypes.Add(typeof(Aran.Aim.Features.SafeAltitudeArea), typeof(AIM_MSA_PDM_Converter));

            elementTypes.Add(typeof(Aran.Aim.Features.GeoBorder), typeof(AIM_GEOBORDER_PDM_Converter));


        }

        public static PDMObject AIM_Object_Convert(object aimFeature, List<IGeometry> aimGeo)
        {
            try
            {
                if (elementTypes.ContainsKey(aimFeature.GetType()))
                {
                    Type t = elementTypes[aimFeature.GetType()];
                    IAIM_PDM_Converter result = (IAIM_PDM_Converter)Activator.CreateInstance(t);
                    PDMObject obj = result.Convert_AIM_Object(aimFeature, aimGeo);
                    if (obj != null && aimFeature is Aran.Aim.Features.Feature && (((Aran.Aim.Features.Feature)aimFeature).TimeSlice != null))
                    {
                        dataInterpretation intrprtn;
                        Enum.TryParse<dataInterpretation>(((Aran.Aim.Features.Feature)aimFeature).TimeSlice.Interpretation.ToString(), out intrprtn);

                        obj.Interpritation = intrprtn;
                    }

                    if (obj != null)
                    {
                        CreateNotes(aimFeature, obj);
                        GetMetadata(aimFeature, obj);
                        GetTimePeriodProperties(aimFeature, obj);
                        GeoProperties(aimFeature, obj);
                    }
                    return obj;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(ex.TargetSite.Name).Error(ex, aimFeature.GetType().ToString() + " ID: " + ((Aran.Aim.Features.Feature)aimFeature).Identifier);
                return null;
            }
        }


        public static void CreateNotes(object aimFeature, PDMObject pdmObj)
        {

            PropertyInfo propInfo = aimFeature.GetType().GetProperty("Annotation");
            if (propInfo == null) return;

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

        public static void GetMetadata(object aimFeature, PDMObject pdmObj)
        {
            PropertyInfo propInfo = aimFeature.GetType().GetProperty("TimeSliceMetadata");
            if (propInfo == null) return;

            object objPropVal = propInfo.GetValue(aimFeature, null);
            if (objPropVal != null)
            {
                Aran.Aim.Metadata.FeatureTimeSliceMetadata timeSliceMetadata = (Aran.Aim.Metadata.FeatureTimeSliceMetadata)objPropVal;
                pdmObj.Metadata = new FeatureMetadata
                {
                    HorizontalResolution = timeSliceMetadata.HorizontalResolution,
                    VerticalResolution = timeSliceMetadata.VerticalResolution
                };


            }
        }

        public static void GetTimePeriodProperties(object aimFeature, PDMObject pdmObj)
        {
            PropertyInfo propInfo = aimFeature.GetType().GetProperty("TimeSlice");
            if (propInfo == null) return;

            object objPropVal = propInfo.GetValue(aimFeature, null);
            if (objPropVal != null)
            {
                Aran.Aim.DataTypes.TimeSlice timeSlice = (Aran.Aim.DataTypes.TimeSlice)objPropVal;
                if (timeSlice.FeatureLifetime != null)
                {
                    var secParam = timeSlice.FeatureLifetime.EndPosition ?? null;
                    pdmObj.FeatureLifeTime = new TimePeriod(timeSlice.FeatureLifetime.BeginPosition, secParam);

                }

                if (timeSlice.ValidTime != null)
                {
                    var secParam = timeSlice.ValidTime.EndPosition ?? null;
                    pdmObj.ValidTime = new TimePeriod(timeSlice.ValidTime.BeginPosition, secParam);

                }

            }


        }
        public static void GeoProperties(object aimFeature, PDMObject pdmObj)
        {

            var propList = aimFeature.GetType().GetProperties();
            var geoProp = propList.FirstOrDefault(p => p.PropertyType == typeof(Aran.Aim.Features.ElevatedPoint));
            if (geoProp != null)
            {
                object geoPropValue = geoProp.GetValue(aimFeature, null);
                if (geoPropValue != null)
                {
                    pdmObj.GeoProperties = new GeoProperties();
                    var featAccuracy = (Aran.Aim.Features.ElevatedPoint)geoPropValue;

                    pdmObj.GeoProperties.VerticalAccuracy = featAccuracy?.VerticalAccuracy?.Value;
                    pdmObj.GeoProperties.HorizontalAccuracy = featAccuracy?.HorizontalAccuracy?.Value;

                    UOM_DIST_HORZ uom;
                    if (Enum.TryParse<UOM_DIST_HORZ>(featAccuracy.VerticalAccuracy?.Uom.ToString(), out uom)) pdmObj.GeoProperties.VerticalAccuracy_UOM = uom;
                    if (Enum.TryParse<UOM_DIST_HORZ>(featAccuracy.HorizontalAccuracy?.Uom.ToString(), out uom)) pdmObj.GeoProperties.HorizontalAccuracy_UOM = uom;

                    pdmObj.GeoProperties.GeoidUndulation = featAccuracy?.GeoidUndulation?.Value;

                    CodeVerticalDatumType vertDatum;
                    if (Enum.TryParse<CodeVerticalDatumType>(featAccuracy?.VerticalDatum.ToString(), out vertDatum)) pdmObj.GeoProperties.VerticalDatum = vertDatum;

                    if (Enum.TryParse<UOM_DIST_HORZ>(featAccuracy?.GeoidUndulation?.Uom.ToString(), out uom)) pdmObj.GeoProperties.GeoidUndulation_UOM = uom;

                }
                return;
            }
            geoProp = propList.FirstOrDefault(p => p.PropertyType == typeof(Aran.Aim.Features.ElevatedCurve));
            if (geoProp != null)
            {
                object geoPropValue = geoProp.GetValue(aimFeature, null);
                if (geoPropValue != null)
                {
                    pdmObj.GeoProperties = new GeoProperties();
                    var featAccuracy = (Aran.Aim.Features.ElevatedCurve)geoPropValue;

                    pdmObj.GeoProperties.VerticalAccuracy = featAccuracy?.VerticalAccuracy?.Value;
                    pdmObj.GeoProperties.HorizontalAccuracy = featAccuracy?.HorizontalAccuracy?.Value;

                    UOM_DIST_HORZ uom;
                    if (Enum.TryParse<UOM_DIST_HORZ>(featAccuracy.VerticalAccuracy?.Uom.ToString(), out uom)) pdmObj.GeoProperties.VerticalAccuracy_UOM = uom;
                    if (Enum.TryParse<UOM_DIST_HORZ>(featAccuracy.HorizontalAccuracy?.Uom.ToString(), out uom)) pdmObj.GeoProperties.HorizontalAccuracy_UOM = uom;

                    pdmObj.GeoProperties.GeoidUndulation = featAccuracy?.GeoidUndulation?.Value;

                    CodeVerticalDatumType vertDatum;
                    if (Enum.TryParse<CodeVerticalDatumType>(featAccuracy?.VerticalDatum.ToString(), out vertDatum)) pdmObj.GeoProperties.VerticalDatum = vertDatum;

                    if (Enum.TryParse<UOM_DIST_HORZ>(featAccuracy?.GeoidUndulation?.Uom.ToString(), out uom)) pdmObj.GeoProperties.GeoidUndulation_UOM = uom;

                }
                return;
            }

            geoProp = propList.FirstOrDefault(p => p.PropertyType == typeof(Aran.Aim.Features.ElevatedSurface));
            if (geoProp != null)
            {
                object geoPropValue = geoProp.GetValue(aimFeature, null);
                if (geoPropValue != null)
                {
                    pdmObj.GeoProperties = new GeoProperties();
                    var featAccuracy = (Aran.Aim.Features.ElevatedSurface)geoPropValue;

                    pdmObj.GeoProperties.VerticalAccuracy = featAccuracy?.VerticalAccuracy?.Value;
                    pdmObj.GeoProperties.HorizontalAccuracy = featAccuracy?.HorizontalAccuracy?.Value;

                    UOM_DIST_HORZ uom;
                    if (Enum.TryParse<UOM_DIST_HORZ>(featAccuracy.VerticalAccuracy?.Uom.ToString(), out uom)) pdmObj.GeoProperties.VerticalAccuracy_UOM = uom;
                    if (Enum.TryParse<UOM_DIST_HORZ>(featAccuracy.HorizontalAccuracy?.Uom.ToString(), out uom)) pdmObj.GeoProperties.HorizontalAccuracy_UOM = uom;

                    pdmObj.GeoProperties.GeoidUndulation = featAccuracy?.GeoidUndulation?.Value;

                    CodeVerticalDatumType vertDatum;
                    if (Enum.TryParse<CodeVerticalDatumType>(featAccuracy?.VerticalDatum.ToString(), out vertDatum)) pdmObj.GeoProperties.VerticalDatum = vertDatum;

                    if (Enum.TryParse<UOM_DIST_HORZ>(featAccuracy?.GeoidUndulation?.Uom.ToString(), out uom)) pdmObj.GeoProperties.GeoidUndulation_UOM = uom;

                }
                return;
            }

        }
    }


    public interface IAIM_PDM_Converter
    {
        PDMObject Convert_AIM_Object(object  aimFeature, List<IGeometry>aimGeo);
    }

    /// <summary> airport group
    /// /////////
    /// </summary>
    /// 
    public class AIM_AIRPORT_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object  aimFeature, List<IGeometry>aimGeo)
        {

            Aran.Aim.Features.AirportHeliport aimObj = (Aran.Aim.Features.AirportHeliport)aimFeature;

            AirportHeliport pdmObj = new AirportHeliport
            {
                Designator = aimObj.Designator != null ? aimObj.Designator : "",
                DesignatorIATA = aimObj.DesignatorIATA != null ? aimObj.DesignatorIATA : "",
                ID = aimObj.Identifier.ToString(),
                Elev = aimObj.FieldElevation == null ? 0 : aimObj.FieldElevation.Value,
                Lat = aimObj.ARP != null && aimObj.ARP.Geo == null ? aimObj.ARP.Geo.X.ToString() : "",
                Lon = aimObj.ARP != null && aimObj.ARP.Geo == null ? aimObj.ARP.Geo.Y.ToString() : "",
                //MagneticVariation = aimObj.MagneticVariation != null && aimObj.MagneticVariation.HasValue ? aimObj.MagneticVariation.Value : Double.NaN,
                MagneticVariationChange = aimObj.MagneticVariationChange != null && aimObj.MagneticVariationChange.HasValue ? aimObj.MagneticVariationChange.Value : Double.NaN,
                Name = aimObj.Name != null ? aimObj.Name : "",
                TransitionAltitude = aimObj.TransitionAltitude == null ? Double.NaN : aimObj.TransitionAltitude.Value,
                DateMagneticVariation = aimObj.DateMagneticVariation == null ? "" : aimObj.DateMagneticVariation,
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
                ServedCity = aimObj.ServedCity != null && aimObj.ServedCity.Count > 0 ? aimObj.ServedCity[0].Name : "",
                //Country = aimObj.Contact[0].Address[0].Country,
                OrganisationAuthority = aimObj.ResponsibleOrganisation != null && aimObj.ResponsibleOrganisation.TheOrganisationAuthority != null ? aimObj.ResponsibleOrganisation.TheOrganisationAuthority.Identifier.ToString() : "",
                CertifiedICAO = aimObj.CertifiedICAO.HasValue ? aimObj.CertifiedICAO.Value : false,
            };

            if (aimObj.MagneticVariation != null)
                pdmObj.MagneticVariation = aimObj.MagneticVariation.Value;
            

            UOM_DIST_VERT uom_dist;
            if ((aimObj.TransitionAltitude!=null) && (Enum.TryParse<UOM_DIST_VERT>(aimObj.TransitionAltitude.Uom.ToString(), out uom_dist))) pdmObj.TransitionAltitudeUOM = uom_dist;
            if ((aimObj.FieldElevation!=null) && (Enum.TryParse<UOM_DIST_VERT>(aimObj.FieldElevation.Uom.ToString(), out uom_dist))) pdmObj.Elev_UOM = uom_dist;

           
            if (aimObj.ReferenceTemperature != null)
            {
                pdmObj.ReferenceTemperature = aimObj.ReferenceTemperature.Value;
                Uom_Temperature uom_temp;
                Enum.TryParse<Uom_Temperature>(aimObj.ReferenceTemperature.Uom.ToString(), out uom_temp); pdmObj.TemperatureUOM = uom_temp;

            }

            if (aimObj.Type != null && aimObj.Type.HasValue)
            {
                AirportHeliportType arptp;
                Enum.TryParse<AirportHeliportType>(aimObj.Type.ToString(), out arptp); pdmObj.AirportHeliportType = arptp;

            }

            if (aimObj.ControlType != null && aimObj.ControlType.HasValue)
            {
                CodeMilitaryOperationsType arptp;
                Enum.TryParse<CodeMilitaryOperationsType>(aimObj.ControlType.ToString(), out arptp); pdmObj.ControlType = arptp;

            }

            if (aimObj.AviationBoundary?.Geo != null)
            {

                var extent = ConvertToEsriGeom.FromGeometry(aimObj.AviationBoundary.Geo, true, GeometryFormatter.Wgs1984Reference());

                var zAware = extent as IZAware;
                zAware.ZAware = false;

                var mAware = extent as IMAware;
                mAware.MAware = false;

                pdmObj.Extent = extent;
            }

            if (aimGeo != null && aimGeo.Count >0)
            {
                var zAware = aimGeo[0] as IZAware;
                zAware.ZAware = true;
                ((IPoint)aimGeo[0]).Z =pdmObj.Elev.HasValue?  pdmObj.ConvertValueToMeter(pdmObj.Elev.Value, pdmObj.Elev_UOM.ToString()) : 0;


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
                CodeComposition = aimObj.SurfaceProperties != null && aimObj.SurfaceProperties.Composition != null ? aimObj.SurfaceProperties.Composition.Value.ToString() : "",
                CodeCondSfc = (aimObj.SurfaceProperties!=null && aimObj.SurfaceProperties.SurfaceCondition!=null)?  aimObj.SurfaceProperties.SurfaceCondition.Value.ToString() : null,
                Designator = aimObj.Designator,
                Length = aimObj.NominalLength?.Value ?? Double.NaN,
                Width = aimObj.NominalWidth?.Value ?? Double.NaN,
                ActualDate = aimObj.TimeSlice?.ValidTime.BeginPosition ?? DateTime.Now,
            };
            
            if (aimObj.SurfaceProperties != null)
            {
                SurfaceCharacteristics pdmsSurfaceCharacteristics = (SurfaceCharacteristics)AIM_PDM_Converter.AIM_Object_Convert(aimObj.SurfaceProperties, null);
                pdmsSurfaceCharacteristics.ID_Parent = aimObj.Identifier.ToString();
                pdmsSurfaceCharacteristics.ParentName = pdmObj.GetType().Name;
                pdmObj.SurfaceProperties = pdmsSurfaceCharacteristics;
            }

            if (aimObj.LengthStrip != null && aimObj.WidthStrip!=null)
            {
                RunwayStrip rwyStrip = new RunwayStrip();
                rwyStrip.ID_Runway = pdmObj.ID;
                rwyStrip.LengthStrip = aimObj.LengthStrip.Value;
                rwyStrip.WidthStrip = aimObj.WidthStrip.Value;

                UOM_DIST_HORZ uom;
                Enum.TryParse<UOM_DIST_HORZ>(aimObj.NominalLength.Uom.ToString(), out uom);
                rwyStrip.Strip_UOM = uom;

                pdmObj.StripProperties = rwyStrip;
            }

            UOM_DIST_HORZ uom_dist;
            if ((aimObj.NominalLength!=null) && (Enum.TryParse<UOM_DIST_HORZ>(aimObj.NominalLength.Uom.ToString(), out uom_dist))) pdmObj.Uom = uom_dist;

            return pdmObj;
        }
    }

    public class AIM_THR_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object  aimFeature, List<IGeometry>aimGeo)
        {

            Aran.Aim.Features.RunwayDirection aimObj = (Aran.Aim.Features.RunwayDirection)aimFeature;

            RunwayDirection pdmObj = new RunwayDirection
            {
                ID = aimObj.Identifier.ToString(),
                Designator = aimObj.Designator,
                Elev = aimObj.ElevationTDZ!=null?  aimObj.ElevationTDZ.Value: Double.NaN,
                LandingThresholdElevation = aimObj.ElevationTDZ != null ? aimObj.ElevationTDZ.Value : Double.NaN,
                Uom = UOM_DIST_HORZ.M,
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,

            };

            if (aimObj.MagneticBearing!=null)
            {
                pdmObj.MagBearing = aimObj.MagneticBearing.Value;
            }

            if (aimObj.TrueBearing!=null)
            {
                pdmObj.TrueBearing = aimObj.TrueBearing.Value;
            }

            UOM_DIST_VERT uom_dist;
            if ((aimObj.ElevationTDZAccuracy!=null) && (Enum.TryParse<UOM_DIST_VERT>(aimObj.ElevationTDZAccuracy.Uom.ToString(), out uom_dist))) pdmObj.Elev_UOM = uom_dist;
            

            if (aimGeo != null && aimGeo.Count >0)
            {
                pdmObj.Lat = ((IPoint)aimGeo[0]).Y > 0 ? ((IPoint)aimGeo[0]).Y.ToString() + "N" : ((IPoint)aimGeo[0]).Y.ToString() + "S";
                pdmObj.Lon = ((IPoint)aimGeo[0]).X > 0 ? ((IPoint)aimGeo[0]).X.ToString() + "E" : ((IPoint)aimGeo[0]).X.ToString() + "W";
                var zAware = aimGeo[0] as IZAware;
                zAware.ZAware = true;
                ((IPoint)aimGeo[0]).Z = pdmObj.Elev.HasValue ? pdmObj.ConvertValueToMeter(pdmObj.Elev.Value, pdmObj.Elev_UOM.ToString()) : 0;


                var mAware = aimGeo[0] as IMAware;
                mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
            }
            
            return pdmObj;
        }
    }

    public class AIM_RwyElement_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            //AranSupport.Utilitys util = new AranSupport.Utilitys();

            Aran.Aim.Features.RunwayElement aimObj = (Aran.Aim.Features.RunwayElement)aimFeature;

            RunwayElement pdmObj = new RunwayElement
            {
                ID = aimObj.Identifier.ToString(),
                Length = aimObj.Length != null ? aimObj.Length.Value : Double.NaN,
                Width = aimObj.Width != null ? aimObj.Width.Value : Double.NaN,
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
                CodeComposition = aimObj.SurfaceProperties != null && aimObj.SurfaceProperties.Composition != null && aimObj.SurfaceProperties.Composition.HasValue ? aimObj.SurfaceProperties.Composition.Value.ToString() : "",
            };

            if (aimObj.SurfaceProperties != null)
            {
                SurfaceCharacteristics pdmsSurfaceCharacteristics = (SurfaceCharacteristics)AIM_PDM_Converter.AIM_Object_Convert(aimObj.SurfaceProperties, null);
                pdmsSurfaceCharacteristics.ID_Parent = aimObj.Identifier.ToString();
                pdmsSurfaceCharacteristics.ParentName = pdmObj.GetType().Name;
                pdmObj.SurfaceProperties = pdmsSurfaceCharacteristics;
            }

            UOM_DIST_HORZ uom_dist;
            if ((aimObj.Length != null) && (Enum.TryParse<UOM_DIST_HORZ>(aimObj.Length.Uom.ToString(), out uom_dist))) pdmObj.LengthUom = uom_dist;
            if ((aimObj.Width != null) && (Enum.TryParse<UOM_DIST_HORZ>(aimObj.Width.Uom.ToString(), out uom_dist))) pdmObj.WidthUom = uom_dist;


            CodeRunwayElementType tp;
            if ((aimObj.Type != null && aimObj.Type.HasValue) && (Enum.TryParse<CodeRunwayElementType>(aimObj.Type.Value.ToString(), out tp))) pdmObj.RunwayElementType = tp;

            if (aimGeo != null && aimGeo.Count >0)
            {
                var zAware = aimGeo[0] as IZAware;
                zAware.ZAware = true;

                var mAware = aimGeo[0] as IMAware;
                mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
            }

            return pdmObj;
        }
    }

    public class AIM_TWY_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.Taxiway aimObj = (Aran.Aim.Features.Taxiway)aimFeature;

            Taxiway pdmObj = new Taxiway
            {
                ID = aimObj.Identifier.ToString(),
                Length = aimObj.Length != null ? aimObj.Length.Value : Double.NaN,
                Width = aimObj.Width != null ? aimObj.Width.Value : Double.NaN,
                CodeComposition = aimObj.SurfaceProperties != null && aimObj.SurfaceProperties.Composition != null && aimObj.SurfaceProperties.Composition.HasValue ? aimObj.SurfaceProperties.Composition.Value.ToString() : "",
                Designator = aimObj.Designator,
                
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
            };

            if (aimObj.SurfaceProperties != null)
            {
                SurfaceCharacteristics pdmsSurfaceCharacteristics = (SurfaceCharacteristics)AIM_PDM_Converter.AIM_Object_Convert(aimObj.SurfaceProperties, null);
                pdmsSurfaceCharacteristics.ID_Parent = aimObj.Identifier.ToString();
                pdmsSurfaceCharacteristics.ParentName = pdmObj.GetType().Name;
                pdmObj.SurfaceProperties = pdmsSurfaceCharacteristics;
            }

            UOM_DIST_HORZ uom_dist;
            if ((aimObj.Length != null) && (Enum.TryParse<UOM_DIST_HORZ>(aimObj.Length.Uom.ToString(), out uom_dist))) pdmObj.LengthUom = uom_dist;
            if ((aimObj.Width != null) && (Enum.TryParse<UOM_DIST_HORZ>(aimObj.Width.Uom.ToString(), out uom_dist))) pdmObj.WidthUom = uom_dist;

            CodeTaxiwayType tp;
            if ((aimObj.Type != null) && (Enum.TryParse<CodeTaxiwayType>(aimObj.Type.ToString(), out tp))) pdmObj.TaxiwayType = tp;

           
            return pdmObj;
        }
    }

    public class AIM_TwyElement_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.TaxiwayElement aimObj = (Aran.Aim.Features.TaxiwayElement)aimFeature;

            TaxiwayElement pdmObj = new TaxiwayElement
            {
                ID = aimObj.Identifier.ToString(),
                Length = aimObj.Length != null ? aimObj.Length.Value : Double.NaN,
                Width = aimObj.Width != null ? aimObj.Width.Value : Double.NaN,
                CodeComposition = aimObj.SurfaceProperties != null && aimObj.SurfaceProperties.Composition != null && aimObj.SurfaceProperties.Composition.HasValue ? aimObj.SurfaceProperties.Composition.Value.ToString() : "",

                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
            };

            if (aimObj.SurfaceProperties != null)
            {
                SurfaceCharacteristics pdmsSurfaceCharacteristics = (SurfaceCharacteristics)AIM_PDM_Converter.AIM_Object_Convert(aimObj.SurfaceProperties, null);
                pdmsSurfaceCharacteristics.ID_Parent = aimObj.Identifier.ToString();
                pdmsSurfaceCharacteristics.ParentName = pdmObj.GetType().Name;
                pdmObj.SurfaceProperties = pdmsSurfaceCharacteristics;
            }

            UOM_DIST_HORZ uom_dist;
            if ((aimObj.Length != null) && (Enum.TryParse<UOM_DIST_HORZ>(aimObj.Length.Uom.ToString(), out uom_dist))) pdmObj.LengthUom = uom_dist;
            if ((aimObj.Width != null) && (Enum.TryParse<UOM_DIST_HORZ>(aimObj.Width.Uom.ToString(), out uom_dist))) pdmObj.WidthUom = uom_dist;

            CodeTaxiwayElementType tp;
            if ((aimObj.Type != null) && (Enum.TryParse<CodeTaxiwayElementType>(aimObj.Type.ToString(), out tp))) pdmObj.TaxiwayElementType = tp;

            if (aimGeo != null && aimGeo.Count >0)
            {
                var zAware = aimGeo[0] as IZAware;
                zAware.ZAware = true;

                var mAware = aimGeo[0] as IMAware;
                mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
            }


            return pdmObj;
        }
    }

    public class AIM_CLP_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            //AranSupport.Utilitys util = new AranSupport.Utilitys();

            Aran.Aim.Features.RunwayCentrelinePoint aimObj = (Aran.Aim.Features.RunwayCentrelinePoint)aimFeature;

            RunwayCenterLinePoint pdmObj = new RunwayCenterLinePoint
            {
                ID = aimObj.Identifier.ToString(),
                Lat = ((IPoint)aimGeo[0]).Y.ToString(),
                Lon = ((IPoint)aimGeo[0]).X.ToString(),                
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
                Designator = aimObj.Designator

            };
            
            if (aimObj.Location?.Elevation != null)
            {
                pdmObj.Elev = aimObj.Location.Elevation.Value;
            }
         
            UOM_DIST_VERT uom_dist;
            if ((aimObj.Location != null && aimObj.Location.Elevation != null) && (Enum.TryParse<UOM_DIST_VERT>(aimObj.Location.Elevation.Uom.ToString(), out uom_dist))) pdmObj.Elev_UOM = uom_dist;

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
            ((IPoint)aimGeo[0]).Z = pdmObj.Elev.HasValue ? pdmObj.ConvertValueToMeter(pdmObj.Elev.Value, pdmObj.Elev_UOM.ToString()) : 0;


            var mAware = aimGeo[0] as IMAware;
            mAware.MAware = true;

            pdmObj.Geo = aimGeo[0];
            pdmObj.Lat = pdmObj.Y_to_NS_DDMMSS();
            pdmObj.Lon = pdmObj.X_to_EW_DDMMSS();
            return pdmObj;
        }
    }

    /// <summary> navaids
    /// ///////////////////////////////////////
    /// </summary>
    /// 

    public class AIM_APPROACH_LIGHT_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            //AranSupport.Utilitys util = new AranSupport.Utilitys();

            Aran.Aim.Features.ApproachLightingSystem aimObj = (Aran.Aim.Features.ApproachLightingSystem)aimFeature;

            ApproachLightingSystem pdmObj = new ApproachLightingSystem
            {
                ID = aimObj.Identifier.ToString(),
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
                EmergencyLighting = aimObj.EmergencyLighting != null && aimObj.EmergencyLighting.HasValue? aimObj.EmergencyLighting.Value : false,
                
            };

            CodeLightIntensity uom_light;
            if (aimObj.IntensityLevel !=null && (aimObj.IntensityLevel.HasValue ) && (Enum.TryParse<CodeLightIntensity>(aimObj.IntensityLevel.ToString(), out uom_light))) pdmObj.IntensityLevel = uom_light;

            ColourType uom_clr;
            if (aimObj.Colour!=null && (aimObj.Colour.HasValue) && (Enum.TryParse<ColourType>(aimObj.Colour.ToString(), out uom_clr))) pdmObj.Colour = uom_clr;


            if ((aimObj.Element != null) && (aimObj.Element.Count > 0))
            {
                pdmObj.Elements = new List<LightElement>();
                int geoIndx = 0;
                foreach (var el in aimObj.Element)
                {
                    Aran.Aim.Features.LightElement aimLightElement = (Aran.Aim.Features.LightElement)el;
                    LightElement pdmLightElement = new LightElement
                    {
                        ID = Guid.NewGuid().ToString(),
                        Intensity = aimLightElement.Intensity!=null ?  aimLightElement.Intensity.Value : 0,
                        Lat = ((IPoint)aimGeo[geoIndx]).Y.ToString(),
                        Lon = ((IPoint)aimGeo[geoIndx]).X.ToString(),
                        Elev = aimLightElement.Location.Elevation != null ? aimLightElement.Location.Elevation.Value : 0,
                        LightedElement="ApproachLightingSystem",                        

                    };

                    if (aimLightElement.IntensityLevel != null && (aimLightElement.IntensityLevel.HasValue) && (Enum.TryParse<CodeLightIntensity>(aimLightElement.IntensityLevel.ToString(), out uom_light))) pdmLightElement.IntensityLevel = uom_light;
                    if (aimLightElement.Colour != null && (aimLightElement.Colour.HasValue) && (Enum.TryParse<ColourType>(aimLightElement.Colour.ToString(), out uom_clr))) pdmLightElement.Colour = uom_clr;

                    CodeLightSource _uom;
                    if (aimLightElement.Type != null && (aimLightElement.Type.HasValue) && (Enum.TryParse<CodeLightSource>(aimLightElement.Type.ToString(), out _uom))) pdmLightElement.LightSourceType = _uom;

                    UOM_DIST_VERT uom_dist;
                    if ((aimLightElement.Location.Elevation != null) && (Enum.TryParse<UOM_DIST_VERT>(aimLightElement.Location.Elevation.Uom.ToString(), out uom_dist))) pdmLightElement.Elev_UOM = uom_dist;


                    var zAware = aimGeo[geoIndx] as IZAware;
                    zAware.ZAware = true;
                    ((IPoint)aimGeo[geoIndx]).Z = pdmLightElement.Elev.HasValue ? pdmLightElement.ConvertValueToMeter(pdmLightElement.Elev.Value, pdmLightElement.Elev_UOM.ToString()) : 0;


                    var mAware = aimGeo[geoIndx] as IMAware;
                    mAware.MAware = true;

                    pdmLightElement.Geo = aimGeo[geoIndx];

                    pdmObj.Elements.Add(pdmLightElement);

                    geoIndx++;

                }

            }

            
            return pdmObj;
        }
    }

    public class AIM_NAVAID_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object  aimFeature, List<IGeometry>aimGeo)
        {
            Aran.Aim.Features.Navaid aimObj = (Aran.Aim.Features.Navaid)aimFeature;

            if (aimObj.Identifier.ToString().StartsWith("5250583f-5f14-429e-94a5-ff70726fa2fb"))
                System.Diagnostics.Debug.WriteLine("");

            NavaidSystem pdmObj = new NavaidSystem
            {
                //ID = Guid.NewGuid().ToString(),//aimObj.Identifier.ToString(),
                ID = aimObj.Identifier.ToString(),
                ID_Feature = aimObj.Identifier.ToString(),
                Designator = aimObj.Designator,
                Name = (aimObj.Name != null && aimObj.Name.Length > 0) ? aimObj.Name : aimObj.Designator,
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
                ID_AirportHeliport = aimObj.ServedAirport != null && aimObj.ServedAirport.Count >0 ? aimObj.ServedAirport[0].Feature.Identifier.ToString() : "",
                
            };

           
             NavaidSystemType navType;
             if (Enum.TryParse<NavaidSystemType>(aimObj.Type.ToString(), out navType)) pdmObj.CodeNavaidSystemType = navType;

             if (aimObj.SignalPerformance.HasValue)
             {
                 CodeSignalPerformanceILS ilsCat;
                 if (Enum.TryParse<CodeSignalPerformanceILS>(aimObj.SignalPerformance.ToString(), out ilsCat)) pdmObj.SignalPerformance = ilsCat;

             }

            return pdmObj;
        }
    }

    public class AIM_DME_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object  aimFeature, List<IGeometry>aimGeo)
        {

            Aran.Aim.Features.DME aimObj = (Aran.Aim.Features.DME)aimFeature;

            DME pdmObj = new DME
            {
               ID = aimObj.Identifier.ToString(),
               Lat = ((IPoint)aimGeo[0]).Y.ToString(),
               Lon = ((IPoint)aimGeo[0]).X.ToString(),
               Designator = aimObj.Designator!=null ? aimObj.Designator :"",
               NavName = aimObj.Name!=null ? aimObj.Name : "",
               GhostFrequency = aimObj.GhostFrequency != null ? aimObj.GhostFrequency.Value : Double.NaN,
               Elev = aimObj.Location != null && aimObj.Location.Elevation!=null ? aimObj.Location.Elevation.Value : Double.NaN,
               Displace = aimObj.Displace != null ? aimObj.Displace.Value : Double.NaN,
               ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,              

            };

            if (aimObj.Location != null && aimObj.Location.Elevation != null)
            {
                UOM_DIST_VERT uom_vert;
                if (Enum.TryParse<UOM_DIST_VERT>(aimObj.Location.Elevation.Uom.ToString(), out uom_vert)) pdmObj.Elev_UOM = uom_vert;
            }

            if (aimObj.Type != null && aimObj.Type.HasValue)
            {
                PDM.CodeDME uom_codeDme;
                if (Enum.TryParse<PDM.CodeDME>(aimObj.Type.Value.ToString(), out uom_codeDme)) pdmObj.DmeType = uom_codeDme;
            }

            if (aimObj.Channel != null && aimObj.Channel.HasValue)
            {
                pdmObj.Channel = aimObj.Channel.Value.ToString();
            }
            var zAware = aimGeo[0] as IZAware;
            zAware.ZAware = true;
            ((IPoint)aimGeo[0]).Z = pdmObj.Elev.HasValue?  pdmObj.ConvertValueToMeter(pdmObj.Elev.Value, pdmObj.Elev_UOM.ToString()) : 0;


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

            Aran.Aim.Features.VOR aimObj = (Aran.Aim.Features.VOR)aimFeature;


           VOR pdmObj = new VOR
            {
                ID = aimObj.Identifier.ToString(),
                Lat = ((IPoint)aimGeo[0]).Y.ToString(),
                Lon = ((IPoint)aimGeo[0]).X.ToString(),
                Frequency = (aimObj.Frequency != null) ? aimObj.Frequency.Value : Double.NaN,
                StationDeclination = (aimObj.Declination !=null)  && (aimObj.Declination.HasValue) ? aimObj.Declination.Value.ToString() : "0",
                Designator = aimObj.Designator != null ? aimObj.Designator : "",
                NavName = aimObj.Name != null ? aimObj.Name : "",
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
                MagVar = aimObj.MagneticVariation.HasValue? aimObj.MagneticVariation.Value : Double.NaN,
            };

            if (aimObj.Frequency != null)
            {
                UOM_FREQ uom_freq;
                if (Enum.TryParse<UOM_FREQ>(aimObj.Frequency.Uom.ToString(), out uom_freq)) pdmObj.Frequency_UOM = uom_freq;
            }

            if (aimObj.Type.HasValue)
            {
                CodeVOR uom_vorType;
                if (Enum.TryParse<CodeVOR>(aimObj.Type.Value.ToString(), out uom_vorType)) pdmObj.VorType = uom_vorType;
            }
            
            var zAware = aimGeo[0] as IZAware;
            zAware.ZAware = true;
            ((IPoint)aimGeo[0]).Z = pdmObj.Elev.HasValue? pdmObj.ConvertValueToMeter(pdmObj.Elev.Value, pdmObj.Elev_UOM.ToString()) : 0;


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
            Aran.Aim.Features.NDB aimObj = (Aran.Aim.Features.NDB)aimFeature;

            NDB pdmObj = new NDB
            {
                ID = aimObj.Identifier.ToString(),
                Lat = ((IPoint)aimGeo[0]).Y.ToString(),
                Lon = ((IPoint)aimGeo[0]).X.ToString(),
                Frequency = aimObj.Frequency != null ? aimObj.Frequency.Value : Double.NaN,
                MagVar = aimObj.MagneticVariation !=null ? aimObj.MagneticVariation.Value :Double.NaN,
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
                
            };

            UOM_FREQ uom_freq;
            if ((aimObj.Frequency!=null) && (Enum.TryParse<UOM_FREQ>(aimObj.Frequency.Uom.ToString(), out uom_freq))) pdmObj.Frequency_UOM = uom_freq;

            var zAware = aimGeo[0] as IZAware;
            zAware.ZAware = true;
            ((IPoint)aimGeo[0]).Z = pdmObj.Elev.HasValue? pdmObj.ConvertValueToMeter(pdmObj.Elev.Value, pdmObj.Elev_UOM.ToString()) : 0;


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
                Lat = ((IPoint)aimGeo[0]).Y.ToString(),
                Lon = ((IPoint)aimGeo[0]).X.ToString(),
                Frequency = aimObj.Frequency != null ? aimObj.Frequency.Value : Double.NaN,
                Axis_Bearing = aimObj.AxisBearing != null && aimObj.AxisBearing.HasValue ? aimObj.AxisBearing.Value : Double.NaN,
                CodeMorse = aimObj.AuralMorseCode!=null ? aimObj.AuralMorseCode : "",
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
            };

            UOM_FREQ uom_freq;
            if ((aimObj.Frequency != null) && (Enum.TryParse<UOM_FREQ>(aimObj.Frequency.Uom.ToString(), out uom_freq))) pdmObj.Frequency_UOM = uom_freq;

            var zAware = aimGeo[0] as IZAware;
            zAware.ZAware = true;
            ((IPoint)aimGeo[0]).Z = pdmObj.Elev.HasValue? pdmObj.ConvertValueToMeter(pdmObj.Elev.Value, pdmObj.Elev_UOM.ToString()) : 0;


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

            Aran.Aim.Features.TACAN aimObj = (Aran.Aim.Features.TACAN)aimFeature;

            TACAN pdmObj = new TACAN
            {
                ID = aimObj.Identifier.ToString(),
                Lat = ((IPoint)aimGeo[0]).Y.ToString(),
                Lon = ((IPoint)aimGeo[0]).X.ToString(),
                Designator = aimObj.Designator != null ? aimObj.Designator : "",
                NavName = aimObj.Name != null ? aimObj.Name : "",
                Channel = aimObj.Channel != null && aimObj.Channel.HasValue? aimObj.Channel.Value.ToString():"",
                Elev = aimObj.Location != null && aimObj.Location.Elevation != null ? aimObj.Location.Elevation.Value : Double.NaN,
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
            };

            if (aimObj.Location != null && aimObj.Location.Elevation != null)
            {
                UOM_DIST_VERT uom_vert;
                if (Enum.TryParse<UOM_DIST_VERT>(aimObj.Location.Elevation.Uom.ToString(), out uom_vert)) pdmObj.Elev_UOM = uom_vert;
            }

             var zAware = aimGeo[0] as IZAware;
            zAware.ZAware = true;
            ((IPoint)aimGeo[0]).Z = pdmObj.Elev.HasValue? pdmObj.ConvertValueToMeter(pdmObj.Elev.Value, pdmObj.Elev_UOM.ToString()) : 0;


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

            Aran.Aim.Features.Localizer aimObj = (Aran.Aim.Features.Localizer)aimFeature;

            Localizer pdmObj = new Localizer
            {
                ID = aimObj.Identifier.ToString(),
                Lat = ((IPoint)aimGeo[0]).Y.ToString(),
                Lon = ((IPoint)aimGeo[0]).X.ToString(),
                Frequency = aimObj.Frequency!=null ?aimObj.Frequency.Value : Double.NaN,
                Declination = aimObj.Declination.HasValue ?  aimObj.Declination.Value.ToString() : "0",
                MagBrg = aimObj.MagneticBearing.HasValue? aimObj.MagneticBearing.Value : Double.NaN,
                trueBearing = aimObj.TrueBearing.HasValue ? aimObj.TrueBearing.Value : Double.NaN,
                Width = aimObj.WidthCourse.HasValue ? aimObj.WidthCourse.Value : Double.NaN,
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,

            };

            if (aimObj.Frequency != null)
            {
                UOM_FREQ uom_freq;
                if (Enum.TryParse<UOM_FREQ>(aimObj.Frequency.Uom.ToString(), out uom_freq)) pdmObj.Frequency_UOM = uom_freq;
            }

            var zAware = aimGeo[0] as IZAware;
            zAware.ZAware = true;
            ((IPoint)aimGeo[0]).Z = pdmObj.Elev.HasValue? pdmObj.ConvertValueToMeter(pdmObj.Elev.Value, pdmObj.Elev_UOM.ToString()) : 0;


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

            Aran.Aim.Features.Glidepath aimObj = (Aran.Aim.Features.Glidepath)aimFeature;

            GlidePath pdmObj = new GlidePath
            {
                ID = aimObj.Identifier.ToString(),
                Lat = ((IPoint)aimGeo[0]).Y.ToString(),
                Lon = ((IPoint)aimGeo[0]).X.ToString(),
                Angle = aimObj.Slope.HasValue ? aimObj.Slope.Value : Double.NaN,
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
                ThresholdCrossingHeight = aimObj.Rdh != null && aimObj.Rdh.Value!=null ? aimObj.Rdh.Value : Double.NaN,
            };

            if (aimObj.Rdh != null)
            {
                UOM_DIST_VERT uom_vert;
                if (Enum.TryParse<UOM_DIST_VERT>(aimObj.Rdh.Uom.ToString(), out uom_vert)) { pdmObj.Elev_UOM = uom_vert; pdmObj.UomDistVer = uom_vert; }
                
            }
            var zAware = aimGeo[0] as IZAware;
            zAware.ZAware = true;
            ((IPoint)aimGeo[0]).Z = pdmObj.Elev.HasValue? pdmObj.ConvertValueToMeter(pdmObj.Elev.Value, pdmObj.Elev_UOM.ToString()) : 0;


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

            Aran.Aim.Features.DesignatedPoint aimObj = (Aran.Aim.Features.DesignatedPoint)aimFeature;

           

            WayPoint pdmObj = new WayPoint
            {
                ID = aimObj.Identifier.ToString(),
                Lat = aimGeo!=null && aimGeo.Count > 0 ? ((IPoint)aimGeo[0]).Y.ToString() : "",
                Lon = aimGeo != null && aimGeo.Count > 0 ? ((IPoint)aimGeo[0]).X.ToString() : "",
                Designator = aimObj.Designator != null ? aimObj.Designator : aimObj.Name != null ? aimObj.Name : "",
                Name = aimObj.Name != null ? aimObj.Name : "",
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
                //ReportingATC = CodeATCReporting.NO_REPORT,
            };

            DesignatorType _uomDesType;
            if ((aimObj.Type != null) && (aimObj.Type.HasValue))
            {
                Enum.TryParse<DesignatorType>(aimObj.Type.Value.ToString(), out _uomDesType);
                pdmObj.Type = _uomDesType;
            }


            if (aimGeo != null && aimGeo.Count > 0)
            {
                var zAware = aimGeo[0] as IZAware;
                zAware.ZAware = true;
                ((IPoint)aimGeo[0]).Z = pdmObj.Elev.HasValue ? pdmObj.ConvertValueToMeter(pdmObj.Elev.Value, pdmObj.Elev_UOM.ToString()) : 0;


                var mAware = aimGeo[0] as IMAware;
                mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
            }
            
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
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
                LocalType = aimObj.LocalType!=null && aimObj.LocalType.Length>0 ? aimObj.LocalType : "",
            };


            if ((aimObj.Class != null) && (aimObj.Class.Count > 0))
            {
                //if (aimObj.Class[0].Classification.HasValue) pdmObj.Lat = aimObj.Class[0].Classification.Value.ToString();
                pdmObj.Class = new List<string>();
                foreach (var item in aimObj.Class)
                {
                    if (item.Classification.HasValue) pdmObj.Class.Add(item.Classification.Value.ToString());
                }


                pdmObj.ClassAirspace = new List<AirspaceClass>();
                foreach (var item in aimObj.Class)
                {
                    if (item.Classification.HasValue)
                    {
                        AirspaceClass cls = new AirspaceClass { Classification = item.Classification.Value.ToString() };
                        if (item.AssociatedLevels != null && item.AssociatedLevels.Count > 0)
                        {
                            cls.ClassAssociatedLevels = new List<AssociatedLevels>();
                            foreach (var lev in item.AssociatedLevels)
                            {
                                AssociatedLevels pdmLev = new AssociatedLevels();
                                CODE_DIST_VER _cd;
                                UOM_DIST_VERT _vrt;

                                if (lev.UpperLimitReference.HasValue)
                                {
                                    Enum.TryParse<CODE_DIST_VER>(lev.UpperLimitReference.Value.ToString(), out _cd);
                                    pdmLev.upperLimitReference = _cd;
                                }

                                if (lev.UpperLimit != null)
                                {
                                    Enum.TryParse<UOM_DIST_VERT>(lev.UpperLimit.Uom.ToString(), out _vrt);
                                    pdmLev.upperLimitUOM = _vrt;

                                    pdmLev.upperLimit = lev.UpperLimit.Value;
                                }

                                if (lev.LowerLimitReference.HasValue)
                                {
                                    Enum.TryParse<CODE_DIST_VER>(lev.LowerLimitReference.Value.ToString(), out _cd);
                                    pdmLev.lowerLimitReference = _cd;
                                }

                                if (lev.LowerLimit != null)
                                {
                                    Enum.TryParse<UOM_DIST_VERT>(lev.LowerLimit.Uom.ToString(), out _vrt);
                                    pdmLev.lowerLimitUOM = _vrt;

                                    pdmLev.lowerLimit = lev.LowerLimit.Value;
                                }

                                cls.ClassAssociatedLevels.Add(pdmLev);

                            }
                        }

                        pdmObj.ClassAirspace.Add(cls);
                    }
                }
            }

            if (aimObj.Type.HasValue)
            {
                AirspaceType _uom;
                var cType = ArenaStatic.ArenaStaticProc.airspaceCodeType_to_AirspaceType(aimObj.Type.Value.ToString());
                Enum.TryParse<AirspaceType>(cType, out _uom);
                pdmObj.CodeType = _uom;
            }
            else
                pdmObj.CodeType = AirspaceType.OTHER;

            if (aimObj.Activation != null && aimObj.Activation.Count > 0)
            {
                foreach (var actv in aimObj.Activation)
                {
                    if (pdmObj.ActivationDescription == null) pdmObj.ActivationDescription = new List<string>();
                    if (actv.Activity.HasValue)
                        pdmObj.ActivationDescription.Add(actv.Activity.Value.ToString());
                }
            }

            if (aimObj.GeometryComponent!=null && aimObj.GeometryComponent.Count >0)
            {
                pdmObj.VolumeGeometryComponents = new List<VolumeGeometryComponent>();
                foreach (var gC in aimObj.GeometryComponent)
                {
                    if (gC.TheAirspaceVolume.ContributorAirspace == null) continue;
                    if (gC.TheAirspaceVolume.ContributorAirspace.TheAirspace == null) continue;

                    CodeAirspaceDependency arspDep = CodeAirspaceDependency.OTHER;
                    if (gC.TheAirspaceVolume.ContributorAirspace!=null && gC.TheAirspaceVolume.ContributorAirspace.Dependency!=null &&  gC.TheAirspaceVolume.ContributorAirspace.Dependency.HasValue)
                    {
                        Enum.TryParse<CodeAirspaceDependency>(gC.TheAirspaceVolume.ContributorAirspace.Dependency.ToString(), out arspDep);
                    }
                   

                    string operation = gC.Operation!=null && gC.Operation.HasValue ? gC.Operation.Value.ToString() : "-";
                    

                    CodeAirspaceAggregation _uomAgg;
                    Enum.TryParse<CodeAirspaceAggregation>(operation, out _uomAgg);

                    int operationSequence = gC.OperationSequence.HasValue ? (int)gC.OperationSequence.Value : 999;
                    string theAirspace = gC.TheAirspaceVolume.ContributorAirspace!=null && gC.TheAirspaceVolume.ContributorAirspace.TheAirspace!=null && gC.TheAirspaceVolume.ContributorAirspace.TheAirspace.Identifier != null ? gC.TheAirspaceVolume.ContributorAirspace.TheAirspace.Identifier.ToString() : "-";
                    pdmObj.VolumeGeometryComponents.Add(new VolumeGeometryComponent {operation = _uomAgg, operationSequence = operationSequence, theAirspace = theAirspace, airspaceDependency = arspDep });

                }

                if (pdmObj.VolumeGeometryComponents.Count == 0) pdmObj.VolumeGeometryComponents = null;
            }
           return pdmObj;
        }
    }

    public class AIM_ARSPS_VOLUME_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.AirspaceVolume aimObj = (Aran.Aim.Features.AirspaceVolume)aimFeature;

            AirspaceVolume pdmObj = new AirspaceVolume();

            pdmObj.ID = Guid.NewGuid().ToString();
            UOM_DIST_VERT vert_uom;
            CODE_DIST_VER lim_ref_uom;
            //pdmObj.ActualDate = aimObj.TimeSlice!=null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,

            if (aimObj.UpperLimit != null)
            {
                pdmObj.ValDistVerUpper = aimObj.UpperLimit.Value;
                Enum.TryParse<UOM_DIST_VERT>(aimObj.UpperLimit.Uom.ToString(), out vert_uom);
                pdmObj.UomValDistVerUpper = vert_uom;

            }

            if (aimObj.LowerLimit != null)
            {
                pdmObj.ValDistVerLower = aimObj.LowerLimit.Value;
                Enum.TryParse<UOM_DIST_VERT>(aimObj.LowerLimit.Uom.ToString(), out vert_uom);
                pdmObj.UomValDistVerLower = vert_uom;
            }

            if (aimObj.MaximumLimit != null)
            {
                pdmObj.ValDistVerMax = aimObj.MaximumLimit.Value;
                Enum.TryParse<UOM_DIST_VERT>(aimObj.MaximumLimit.Uom.ToString(), out vert_uom);
                pdmObj.UomValDistVerMax = vert_uom;
            }


            if (aimObj.MinimumLimit != null)
            {
                pdmObj.ValDistVerMnm = aimObj.MinimumLimit.Value;
                Enum.TryParse<UOM_DIST_VERT>(aimObj.MinimumLimit.Uom.ToString(), out vert_uom);
                pdmObj.UomValDistVerMnm = vert_uom;
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



            if (aimGeo != null && aimGeo[0].GeometryType == esriGeometryType.esriGeometryPolygon)
            {
                var zAware = aimGeo[0] as IZAware;
                zAware.ZAware = true;

                var mAware = aimGeo[0] as IMAware;
                mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
            }
            return pdmObj;


        }
    }

    public class AIM_ENROUTE_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry>aimGeo)
        {
            Aran.Aim.Features.Route aimObj = (Aran.Aim.Features.Route)aimFeature;

            if (aimObj.Name == null && !aimObj.InternationalUse.HasValue) return null;
            
            Enroute pdmObj = new Enroute
            {
                ID = aimObj.Identifier.ToString(),
                TxtDesig = aimObj.Name,
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
                OrganisationAuthority = aimObj.UserOrganisation!=null? aimObj.UserOrganisation.Identifier.ToString() : "",
            };

            if (pdmObj.TxtDesig == null || pdmObj.TxtDesig.Length <= 0)
            {
                pdmObj.TxtDesig = "";
                pdmObj.TxtDesig = aimObj.DesignatorSecondLetter != null && aimObj.DesignatorSecondLetter.HasValue ? aimObj.DesignatorSecondLetter.Value.ToString() : "";

                pdmObj.TxtDesig = aimObj.DesignatorNumber != null && aimObj.DesignatorNumber.HasValue ? pdmObj.TxtDesig + aimObj.DesignatorNumber.Value.ToString() : pdmObj.TxtDesig;

                

            }

            if (pdmObj.TxtDesig == null || pdmObj.TxtDesig.Length <= 0)
            {
                pdmObj.TxtDesig = "UnNamed";
            }

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
                ValDistVerUpper = (aimObj.UpperLimit != null) ? aimObj.UpperLimit.Value : Double.NaN,
                ValDistVerLower = (aimObj.LowerLimit != null) ? aimObj.LowerLimit.Value : Double.NaN,
                ValDistVerMnm = (aimObj.MinimumEnrouteAltitude != null) ? aimObj.MinimumEnrouteAltitude.Value : Double.NaN,
                ValLen = (aimObj.Length != null) ? aimObj.Length.Value : Double.NaN,
                ValWidLeft = (aimObj.WidthLeft != null) ? aimObj.WidthLeft.Value : Double.NaN,
                ValWidRight = (aimObj.WidthRight != null) ? aimObj.WidthRight.Value : Double.NaN,
                ValMagTrack = (aimObj.MagneticTrack.HasValue) ? aimObj.MagneticTrack.Value : Double.NaN,
                ValTrueTrack = (aimObj.TrueTrack.HasValue) ? aimObj.TrueTrack.Value : Double.NaN,
                ValReversMagTrack = (aimObj.ReverseMagneticTrack.HasValue) ? aimObj.ReverseMagneticTrack.Value : Double.NaN,
                ValReversTrueTrack = (aimObj.ReverseTrueTrack.HasValue) ? aimObj.ReverseTrueTrack.Value : Double.NaN,
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
                
            };

            CodeRouteNavigation _uomNavigationType;
            if (aimObj.NavigationType != null)
            {
                Enum.TryParse<CodeRouteNavigation>(aimObj.NavigationType.ToString(), out _uomNavigationType);
                pdmObj.NavigationType = _uomNavigationType;
            }


            CODE_ROUTE_SEGMENT_DIR _uomDIR;
            if ((aimObj.Availability != null) && (aimObj.Availability.Count > 0))
            {
                foreach (var codeD in aimObj.Availability)
                {
                    if (codeD.Direction.HasValue)
                    {
                        Enum.TryParse<CODE_ROUTE_SEGMENT_DIR>(codeD.Direction.Value.ToString(), out _uomDIR);
                        //if (pdmObj.CodeDir == CODE_ROUTE_SEGMENT_DIR.OTHER) pdmObj.CodeDir = _uomDIR;
                        if (pdmObj.CodeDir == CODE_ROUTE_SEGMENT_DIR.BOTH) pdmObj.CodeDir = CODE_ROUTE_SEGMENT_DIR.BOTH;
                        else if (pdmObj.CodeDir == CODE_ROUTE_SEGMENT_DIR.FORWARD && _uomDIR == CODE_ROUTE_SEGMENT_DIR.BACKWARD) pdmObj.CodeDir = CODE_ROUTE_SEGMENT_DIR.BOTH;
                        else if (pdmObj.CodeDir == CODE_ROUTE_SEGMENT_DIR.BACKWARD && _uomDIR == CODE_ROUTE_SEGMENT_DIR.FORWARD) pdmObj.CodeDir = CODE_ROUTE_SEGMENT_DIR.BOTH;
                        else pdmObj.CodeDir = _uomDIR;
                    }
                }

               
            }

            UOM_DIST_VERT _uomVert;
            if (aimObj.UpperLimit != null)
            {
                Enum.TryParse<UOM_DIST_VERT>(aimObj.UpperLimit.Uom.ToString(), out _uomVert);
                pdmObj.UomValDistVerUpper = _uomVert;
            }

            if (aimObj.LowerLimit != null)
            {
                Enum.TryParse<UOM_DIST_VERT>(aimObj.LowerLimit.Uom.ToString(), out _uomVert);
                pdmObj.UomValDistVerLower = _uomVert;
            }

            CODE_ROUTE_SEGMENT_CODE_LVL lvl;
            if (aimObj.Level != null && aimObj.Level.HasValue)
            {
                Enum.TryParse<CODE_ROUTE_SEGMENT_CODE_LVL>(aimObj.Level.Value.ToString(), out lvl);
                pdmObj.CodeLvl = lvl;
            }

            UOM_DIST_HORZ _uomHOR;
            if (aimObj.WidthLeft != null)
            {
                Enum.TryParse<UOM_DIST_HORZ>(aimObj.WidthLeft.Uom.ToString(), out _uomHOR);
                pdmObj.UomValWid = _uomHOR;
            }

            if (aimObj.Length != null)
            {
                Enum.TryParse<UOM_DIST_HORZ>(aimObj.Length.Uom.ToString(), out _uomHOR);
                pdmObj.UomValLen = _uomHOR;
            }


            if (aimGeo != null && aimGeo.Count >0 )
            {
                foreach (var _geo in aimGeo)
                {
                    if (_geo.GeometryType != esriGeometryType.esriGeometryPolyline) continue;

                    var zAware = _geo as IZAware;
                    zAware.ZAware = true;
                   
                    var mAware = _geo as IMAware;
                    mAware.MAware = true;

                    pdmObj.Geo = _geo;
                }

            }
            return pdmObj;
        }
    }

    /// <summary> Procedures
    /// //////
    /// </summary>

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
                CopterTrack = aimObj.CopterTrack != null && aimObj.CopterTrack.HasValue ? aimObj.CopterTrack.Value : Double.NaN,
                CourseReversalInstruction = (aimObj.CourseReversalInstruction != null) ? aimObj.CourseReversalInstruction : "",
                ChannelGNSS = aimObj.ChannelGNSS != null && aimObj.ChannelGNSS.HasValue ? aimObj.ChannelGNSS.Value : Double.NaN,
                WAASReliable = aimObj.WAASReliable != null && aimObj.WAASReliable.HasValue ? aimObj.WAASReliable.Value : false,
                ID_MasterProc = aimObj.Identifier.ToString(),
                AircraftCharacteristic = GetAircraftCharacteristic(aimObj.AircraftCharacteristic),
                LandingArea = aimObj.Landing != null && aimObj.Landing.Runway != null && aimObj.Landing.Runway.Count > 0 ? new List<PDMObject>() : null,
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
                MissedInstruction = aimObj.MissedInstruction!=null && aimObj.MissedInstruction.Count>0 ? GetInsruction(aimObj.MissedInstruction[0]) : "",
            };

            if (pdmObj.LandingArea!=null && aimObj.Landing != null && aimObj.Landing.Runway != null && aimObj.Landing.Runway.Count > 0)
            {
                foreach (var rwy in aimObj.Landing.Runway)
                {
                    pdmObj.LandingArea.Add( new PDMObject { ID = rwy.Feature.Identifier.ToString() });
                }
            }


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
            if ((aimObj.ApproachType.HasValue) && (aimObj.ApproachPrefix != null) && (aimObj.ApproachPrefix.HasValue))
            {
                Enum.TryParse<ApproachType>(aimObj.ApproachType.Value.ToString(), out _uomApproachType);
                pdmObj.ApproachType = _uomApproachType;
            }
            else
            {
                pdmObj.ApproachType = ApproachType.OTHER;
            }


            CodeApproachEquipmentAdditional _uomAddEq;
            if ((aimObj.AdditionalEquipment != null) && (aimObj.AdditionalEquipment.HasValue))
            {
                Enum.TryParse<CodeApproachEquipmentAdditional>(aimObj.AdditionalEquipment.Value.ToString(), out _uomAddEq);
                pdmObj.AdditionalEquipment = _uomAddEq;
            }

            if (aimObj.Instruction != null && aimObj.Instruction.Length > 0)
                pdmObj.Instruction = aimObj.Instruction;

            #region FinalProfile

            if (aimObj.FinalProfile != null)
            {
                pdmObj.Profile = new FinalProfile { };

                #region Altitude

                if (aimObj.FinalProfile.Altitude != null)
                {
                    pdmObj.Profile.ApproachAltitudeTable = new List<ApproachAltitude>();
                    
                    foreach (var item in aimObj.FinalProfile.Altitude)
                    {
                        ApproachAltitude _alt = new ApproachAltitude();
                        _alt.Altitude = item.Altitude!= null ? item.Altitude.Value : Double.NaN;

                        UOM_DIST_VERT _uomVert;
                        if (item.Altitude !=null)
                        {
                            Enum.TryParse<UOM_DIST_VERT>(item.Altitude.Uom.ToString(), out _uomVert);
                            _alt.AltitudeUOM = _uomVert;
                        }
                        else
                        {
                            _alt.AltitudeUOM = UOM_DIST_VERT.SM;
                        }

                        CODE_DIST_VER _uomref;
                        if (item.AltitudeReference.HasValue)
                        {
                            Enum.TryParse<CODE_DIST_VER>(item.AltitudeReference.Value.ToString(), out _uomref);
                            _alt.AltitudeReference = _uomref;
                        }
                        else
                        {
                            _alt.AltitudeReference = CODE_DIST_VER.OTHER;
                        }

                        CodeProcedureDistance _uomprocDist;
                        if (item.MeasurementPoint.HasValue)
                        {
                            Enum.TryParse<CodeProcedureDistance>(item.MeasurementPoint.Value.ToString(), out _uomprocDist);
                            _alt.MeasurementPoint = _uomprocDist;
                        }
                        else
                        {
                            _alt.MeasurementPoint = CodeProcedureDistance.OTHER_SDF;
                        }


                        pdmObj.Profile.ApproachAltitudeTable.Add(_alt);
                    }
                }

                #endregion

                #region Distance

                if (aimObj.FinalProfile.Distance != null)
                {
                    pdmObj.Profile.ApproachDistancetable = new List<ApproachDistance>();

                    foreach (var item in aimObj.FinalProfile.Distance)
                    {
                        ApproachDistance _dist = new ApproachDistance();
                        _dist.ValueHAT = item.ValueHAT != null ? item.ValueHAT.Value : Double.NaN;

                        UOM_DIST_VERT _uomref;
                        if (item.ValueHAT!=null)
                        {
                            Enum.TryParse<UOM_DIST_VERT>(item.ValueHAT.Uom.ToString(), out _uomref);
                            _dist.ValueHATUOM = _uomref;
                        }
                        else
                        {
                            _dist.ValueHATUOM = UOM_DIST_VERT.SM;
                        }

                        CodeProcedureDistance _uomprocDist;
                        if (item.StartingMeasurementPoint.HasValue)
                        {
                            Enum.TryParse<CodeProcedureDistance>(item.StartingMeasurementPoint.Value.ToString(), out _uomprocDist);
                            _dist.StartingMeasurementPoint = _uomprocDist;
                        }
                        else
                        {
                            _dist.StartingMeasurementPoint = CodeProcedureDistance.OTHER_SDF;
                        }


                        if (item.EndingMeasurementPoint.HasValue)
                        {
                            Enum.TryParse<CodeProcedureDistance>(item.EndingMeasurementPoint.Value.ToString(), out _uomprocDist);
                            _dist.EndingMeasurementPoint = _uomprocDist;
                        }
                        else
                        {
                            _dist.EndingMeasurementPoint = CodeProcedureDistance.OTHER_SDF;
                        }


                        
                        _dist.Distance = item.Distance != null? item.Distance.Value : Double.NaN;
                        if (item.Distance != null)
                        {
                            UOM_DIST_HORZ _uomHorz;

                            Enum.TryParse<UOM_DIST_HORZ>(item.Distance.Uom.ToString(), out _uomHorz);
                            _dist.DistanceUOM = _uomHorz;
                        }
                        

                        pdmObj.Profile.ApproachDistancetable.Add(_dist);
                    }
                }

                #endregion

                #region Timing

                if (aimObj.FinalProfile.Timing != null)
                {
                    pdmObj.Profile.ApproachTimingTable = new List<ApproachTiming>();

                    foreach (var item in aimObj.FinalProfile.Timing)
                    {
                        ApproachTiming _timing = new ApproachTiming();
                        _timing.Time = item.Time != null ? item.Time.Value : Double.NaN;

                        DurationType _uomref;
                        if (item.Time != null)
                        {
                            Enum.TryParse<DurationType>(item.Time.Uom.ToString(), out _uomref);
                            _timing.TimeDuration = _uomref;
                        }
                        else
                        {
                            _timing.TimeDuration = DurationType.OTHER;
                        }

                        CodeProcedureDistance _uomprocDist;
                        if (item.StartingMeasurementPoint.HasValue)
                        {
                            Enum.TryParse<CodeProcedureDistance>(item.StartingMeasurementPoint.Value.ToString(), out _uomprocDist);
                            _timing.StartingMeasurementPoint = _uomprocDist;
                        }
                        else
                        {
                            _timing.StartingMeasurementPoint = CodeProcedureDistance.OTHER_SDF;
                        }


                        if (item.EndingMeasurementPoint.HasValue)
                        {
                            Enum.TryParse<CodeProcedureDistance>(item.EndingMeasurementPoint.Value.ToString(), out _uomprocDist);
                            _timing.EndingMeasurementPoint = _uomprocDist;
                        }
                        else
                        {
                            _timing.EndingMeasurementPoint = CodeProcedureDistance.OTHER_SDF;
                        }



                        _timing.Speed = item.Speed != null ? item.Speed.Value : Double.NaN;
                        if (item.Speed != null)
                        {
                            SpeedType _uomHorz;

                            Enum.TryParse<SpeedType>(item.Speed.Uom.ToString(), out _uomHorz);
                            _timing.SpeedUOM = _uomHorz;
                        }


                        pdmObj.Profile.ApproachTimingTable.Add(_timing);
                    }
                }

                #endregion


            }

            #endregion

            return pdmObj;
        }

        private string GetInsruction(Aran.Aim.Features.MissedApproachGroup missedApproachGroup)
        {
            return missedApproachGroup.Instruction != null ? missedApproachGroup.Instruction : "";
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
                ProcedureIdentifier = aimObj.Name != null && aimObj.Name.Trim().Length >0 && !aimObj.Name.StartsWith("_") ? aimObj.Name : aimObj.Designator,
                ProcedureType = PROC_TYPE_code.SID,
                CommunicationFailureDescription = aimObj.CommunicationFailureInstruction!=null ?  aimObj.CommunicationFailureInstruction : "",
                FlightChecked = aimObj.FlightChecked!=null &&  aimObj.FlightChecked.HasValue ? aimObj.FlightChecked.Value : false,
                RNAV = aimObj.RNAV!=null  &&  aimObj.RNAV.HasValue ? aimObj.RNAV.Value : false,
                Designator = aimObj.Designator,
                ContingencyRoute = aimObj.ContingencyRoute!=null && aimObj.ContingencyRoute.HasValue? aimObj.ContingencyRoute.Value :false,
                ID_MasterProc = aimObj.Identifier.ToString(),
                AircraftCharacteristic = GetAircraftCharacteristic(aimObj.AircraftCharacteristic),
                LandingArea = aimObj.Takeoff != null && aimObj.Takeoff.Runway != null && aimObj.Takeoff.Runway.Count > 0 ? new List<PDMObject>(): null,
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
                
            };

            if (pdmObj.LandingArea != null && aimObj.Takeoff != null && aimObj.Takeoff.Runway != null && aimObj.Takeoff.Runway.Count > 0)
            {
                foreach (var rwy in aimObj.Takeoff.Runway)
                {
                    pdmObj.LandingArea.Add(new PDMObject { ID = rwy.Feature.Identifier.ToString() });
                }
            }

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


            if (aimObj.Instruction != null && aimObj.Instruction.Length > 0)
                pdmObj.Instruction = aimObj.Instruction;

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
                ProcedureIdentifier = aimObj.Name != null && aimObj.Name.Trim().Length <= 8 && !aimObj.Name.StartsWith("_") ? aimObj.Name : aimObj.Designator,
                ProcedureType = PROC_TYPE_code.STAR,
                CommunicationFailureDescription = aimObj.CommunicationFailureInstruction!=null ? aimObj.CommunicationFailureInstruction : "",
                FlightChecked = aimObj.FlightChecked !=null && aimObj.FlightChecked.HasValue ? aimObj.FlightChecked.Value : false,
                RNAV = aimObj.RNAV!=null && aimObj.RNAV.HasValue ? aimObj.RNAV.Value : false,
                Designator = aimObj.Designator,
                ID_MasterProc = aimObj.Identifier.ToString(),
                AircraftCharacteristic = GetAircraftCharacteristic(aimObj.AircraftCharacteristic),
                LandingArea = aimObj.Arrival!=null && aimObj.Arrival.Runway!=null && aimObj.Arrival.Runway.Count>0 ? new List<PDMObject>() : null,
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
                //Instruction = aimObj.Instruction != null ? aimObj.Instruction : "",

            };

            if (pdmObj.LandingArea != null && aimObj.Arrival != null && aimObj.Arrival.Runway != null && aimObj.Arrival.Runway.Count > 0)
            {
                foreach (var rwy in aimObj.Arrival.Runway)
                {
                    pdmObj.LandingArea.Add(new PDMObject { ID = rwy.Feature.Identifier.ToString() });
                }
            }

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

            if (aimObj.Instruction != null && aimObj.Instruction.Length > 0)
                pdmObj.Instruction = aimObj.Instruction;


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
                ID = aimObj.Id != 0 ? aimObj.Id.ToString() : Guid.NewGuid().ToString(),
                FeatureGUID =Guid.NewGuid().ToString(),
                TransitionIdentifier = aimObj.TransitionId != null ? aimObj.TransitionId : Guid.NewGuid().ToString(),
                VectorHeading =  aimObj.VectorHeading!=null && aimObj.VectorHeading.HasValue ? aimObj.VectorHeading.Value : Double.NaN,
                //Description = aimObj.DepartureRunwayTransition != null && aimObj.DepartureRunwayTransition.Runway.Count >0? aimObj.DepartureRunwayTransition.Runway[0].Feature.Identifier.ToString() : "",
                //ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
            };

            if (aimObj.DepartureRunwayTransition != null && aimObj.DepartureRunwayTransition.Runway!=null && aimObj.DepartureRunwayTransition.Runway.Count > 0)
            {
                foreach (var rw in aimObj.DepartureRunwayTransition.Runway)
                {
                    pdmObj.Description = pdmObj.Description + rw.Feature.Identifier.ToString() + ":";
                }
            }

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
            pdmObj.AltitudeOverrideATC = aimObj.AltitudeOverrideATC != null ? aimObj.AltitudeOverrideATC.Value : Double.NaN;
            pdmObj.BankAngle = aimObj.BankAngle.HasValue ? aimObj.BankAngle.Value : Double.NaN;
            pdmObj.Course = aimObj.Course.HasValue ? aimObj.Course.Value : Double.NaN;
            pdmObj.Duration = aimObj.Duration != null ? aimObj.Duration.Value : Double.NaN;
            pdmObj.LowerLimitAltitude = aimObj.LowerLimitAltitude != null ? aimObj.LowerLimitAltitude.Value : Double.NaN;
            pdmObj.SpeedLimit = aimObj.SpeedLimit != null ? aimObj.SpeedLimit.Value : Double.NaN;
            pdmObj.UpperLimitAltitude = aimObj.UpperLimitAltitude != null ? aimObj.UpperLimitAltitude.Value : Double.NaN;
            pdmObj.VerticalAngle = aimObj.VerticalAngle.HasValue ? aimObj.VerticalAngle.Value : Double.NaN;
            if (aimObj.ProcedureTurnRequired.HasValue) pdmObj.ProcedureTurnRequired = aimObj.ProcedureTurnRequired.Value;
            pdmObj.AircraftCategory = GetAircraftCharacteristic(aimObj.AircraftCategory);
            pdmObj.Length = aimObj.Length != null ? aimObj.Length.Value : Double.NaN;
            pdmObj.ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now;

            ////////////////////////////////////////////
            
            CodeSegmentPath legTp;
            Enum.TryParse<CodeSegmentPath>(aimObj.LegTypeARINC.ToString(), out legTp);
            pdmObj.LegTypeARINC = legTp;

            SegmentLegSpecialization _uomSpec;
            Enum.TryParse<SegmentLegSpecialization>(aimObj.GetType().Name, out _uomSpec);
            pdmObj.LegSpecialization = _uomSpec;

            if (aimObj.Length != null)
            {
                UOM_DIST_HORZ horDist;
                Enum.TryParse<UOM_DIST_HORZ>(aimObj.Length.Uom.ToString(), out horDist);
                pdmObj.LengthUOM = horDist;
            }

            if (aimObj.SpeedInterpretation.HasValue)
            {
                AltitudeUseType speedInterpritationUom;
                Enum.TryParse<AltitudeUseType>(aimObj.SpeedInterpretation.Value.ToString(), out speedInterpritationUom);
                pdmObj.SpeedInterpritation = speedInterpritationUom;
            }
            else
                pdmObj.SpeedInterpritation = AltitudeUseType.OTHER;

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

                if (aimObj_FinalLeg.Condition != null && aimObj_FinalLeg.Condition.Count > 0)
                {
                    pdmObj_FinalLeg.Condition_Minima = new List<ApproachCondition>();

                    foreach (var cond in aimObj_FinalLeg.Condition)
                    {
                        ApproachCondition pdmCond = new ApproachCondition();
                        Aran.Aim.Features.ApproachCondition aimCond = (Aran.Aim.Features.ApproachCondition)cond;

                        if (aimCond.FinalApproachPath.HasValue)
                        {
                            CodeMinimaFinalApproachPath _FAP_UOM;
                            Enum.TryParse<CodeMinimaFinalApproachPath>(aimCond.FinalApproachPath.Value.ToString(), out _FAP_UOM);
                            pdmCond.FinalApproachPath = _FAP_UOM;
                        }

                        if (aimCond.MinimumSet != null)
                        {
                            pdmCond.MinAltitude = aimCond.MinimumSet.Altitude.Value;
                            if (aimCond.MinimumSet.AltitudeCode.HasValue)
                            {
                                CodeMinimumAltitude _uom;
                                Enum.TryParse<CodeMinimumAltitude>(aimCond.MinimumSet.AltitudeCode.ToString(), out _uom);
                                pdmCond.MinAltitudeCode = _uom;
                            }
                            if (aimCond.MinimumSet.Altitude.Uom != null)
                            {
                                UOM_DIST_VERT _uom;
                                Enum.TryParse<UOM_DIST_VERT>(aimCond.MinimumSet.Altitude.Uom.ToString(), out _uom);
                                pdmCond.MinAltitudeUOM = _uom;
                            }

                            pdmCond.MinHeight = aimCond.MinimumSet.Height.Value;
                            if (aimCond.MinimumSet.HeightCode.HasValue)
                            {
                                CodeMinimumHeight _uom;
                                Enum.TryParse<CodeMinimumHeight>(aimCond.MinimumSet.HeightCode.ToString(), out _uom);
                                pdmCond.MinHeightCode = _uom;
                            }
                            if (aimCond.MinimumSet.Height.Uom != null)
                            {
                                UOM_DIST_VERT _uom;
                                Enum.TryParse<UOM_DIST_VERT>(aimCond.MinimumSet.Height.Uom.ToString(), out _uom);
                                pdmCond.MinHeightUOM = _uom;
                            }
                            if (aimCond.MinimumSet.MandatoryRVR != null && aimCond.MinimumSet.MandatoryRVR.HasValue)
                            {
                                pdmCond.MinMandatoryRVR = aimCond.MinimumSet.MandatoryRVR.Value;
                            }
                            if (aimCond.MinimumSet.Visibility != null)
                            {
                                pdmCond.MinVisibility = aimCond.MinimumSet.Visibility.Value;

                                UOM_DIST_HORZ _uom;
                                Enum.TryParse<UOM_DIST_HORZ>(aimCond.MinimumSet.Visibility.Uom.ToString(), out _uom);
                                pdmCond.MinVisibilityUOM = _uom;
                            }
                        }

                        if (aimCond.AircraftCategory != null)
                        {
                            foreach (var aimAirCat in aimCond.AircraftCategory)
                            {
                                if (aimAirCat.AircraftLandingCategory.HasValue)
                                {
                                    AircraftCategoryType _AircraftCharacteristic;
                                    Enum.TryParse<AircraftCategoryType>(aimAirCat.AircraftLandingCategory.Value.ToString(), out _AircraftCharacteristic);
                                    if (pdmCond.AircraftCategory == null) pdmCond.AircraftCategory = new List<AircraftCategoryType>();
                                    pdmCond.AircraftCategory.Add(_AircraftCharacteristic);
                                }
                            }
                        }
                        pdmObj_FinalLeg.Condition_Minima.Add(pdmCond);
                    }
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

                if (aimObj_MissidLeg.RequiredNavigationPerformance!=null && aimObj_MissidLeg.RequiredNavigationPerformance.HasValue)
                {
                    pdmObj_MissedLeg.RequiredNavigationPerformance = aimObj_MissidLeg.RequiredNavigationPerformance.Value;
                }
            }

            #endregion

            #region DepartureLeg

            if (aimObj is Aran.Aim.Features.DepartureLeg)
            {

                Aran.Aim.Features.DepartureLeg aimObj_DepLeg = (Aran.Aim.Features.DepartureLeg)aimObj;

                if (aimObj_DepLeg.RequiredNavigationPerformance.HasValue)
                {
                    pdmObj.RequiredNavigationPerformance = aimObj_DepLeg.RequiredNavigationPerformance.Value;
                }
            }

            if (aimObj is Aran.Aim.Features.ArrivalLeg)
            {

                Aran.Aim.Features.ArrivalLeg aimObj_ArrLeg = (Aran.Aim.Features.ArrivalLeg)aimObj;

                if (aimObj_ArrLeg.RequiredNavigationPerformance.HasValue)
                {
                    pdmObj.RequiredNavigationPerformance = aimObj_ArrLeg.RequiredNavigationPerformance.Value;
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

            CodeCourse _uomBearingType;
            if (aimObj.CourseType.HasValue)
            {
                Enum.TryParse<CodeCourse>(aimObj.CourseType.Value.ToString(), out _uomBearingType);
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
            //else
            //    pdmObj.TurnDirection = DirectionTurnType.OTHER;

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
                        case PointChoice.RunwayCentrelinePoint:
                        //case PointChoice.RunwayPoint:
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

                    _strtPnt.ID = _strtPnt.PointChoiceID;


                }

                #endregion

                #region FacilityMakeUp

                if ((aimObj.StartPoint.FacilityMakeup != null) && (aimObj.StartPoint.FacilityMakeup.Count > 0))
                {
                    FacilityMakeUp _FacilityMakeUp = CreateFacilityMakeUp(aimObj.StartPoint.FacilityMakeup[0]);
                    _FacilityMakeUp.SegmentPointID = _strtPnt.ID;
                    _strtPnt.PointFacilityMakeUp = _FacilityMakeUp;


                    if ((aimObj.StartPoint.FacilityMakeup[0].FixToleranceArea != null) && (aimObj.StartPoint.FacilityMakeup[0].FixToleranceArea.Geo != null)) 
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
                        case PointChoice.RunwayCentrelinePoint:
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

                    _endPnt.ID = _endPnt.PointChoiceID;

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
                        case PointChoice.RunwayCentrelinePoint:
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

                    _arcPnt.ID = _arcPnt.PointChoiceID;

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


            if (aimObj.DesignSurface != null && aimObj.DesignSurface.Count >0)
            {

                #region DesignSurface
                
                pdmObj.AssessmentArea = new List<ObstacleAssessmentArea>();

                foreach (var surf in aimObj.DesignSurface)
                {
                    ObstacleAssessmentArea PdmObsAsArea = new ObstacleAssessmentArea();
                    if (surf.AssessedAltitude != null)
                    {
                        PdmObsAsArea.AssessedAltitude = surf.AssessedAltitude.Value;
                        UOM_DIST_VERT vUom;
                        Enum.TryParse<UOM_DIST_VERT>(surf.AssessedAltitude.Uom.ToString(), out vUom);
                        PdmObsAsArea.AssessedAltitudeUOM = vUom;
                    }

                    if (surf.SafetyRegulation!=null) PdmObsAsArea.SafetyRegulation = surf.SafetyRegulation;
                    PdmObsAsArea.SectionNumber = surf.SectionNumber.HasValue ? (int)surf.SectionNumber.Value : -1;
                    PdmObsAsArea.Slope = surf.Slope!=null && surf.Slope.HasValue ? surf.Slope.Value : Double.NaN;
                    if (surf.SlopeLowerAltitude != null)
                    {
                        PdmObsAsArea.SlopeLowerAltitude = surf.SlopeLowerAltitude.Value;
                         UOM_DIST_VERT vUom;
                         Enum.TryParse<UOM_DIST_VERT>(surf.SlopeLowerAltitude.Uom.ToString(), out vUom);
                        PdmObsAsArea.SlopeLowerAltitudeUOM = vUom;
                        
                    }

                    if (surf.Type.HasValue)
                    {
                        CodeObstacleAssessmentSurface CodeObstacleAssessmentSurfaceUOM;
                        Enum.TryParse<CodeObstacleAssessmentSurface>(surf.Type.Value.ToString(), out CodeObstacleAssessmentSurfaceUOM);
                        PdmObsAsArea.Type = CodeObstacleAssessmentSurfaceUOM;
                    }

                    //surf.SignificantObstacle
                    if (surf.SurfaceZone.HasValue)
                    {
                        CodeObstructionIdSurfaceZone znUom;
                        Enum.TryParse<CodeObstructionIdSurfaceZone>(surf.SurfaceZone.Value.ToString(), out znUom);
                        PdmObsAsArea.SurfaceZone = znUom;
                    }

                    if (surf.Surface!=null && surf.Surface.Geo != null)
                    {
                        PdmObsAsArea.SurfaceGeo = HelperClass.SetObjectToBlob(surf.Surface.Geo, "Surface");
                    }


                    if (surf.SignificantObstacle != null && surf.SignificantObstacle.Count > 0)
                    {
                        PdmObsAsArea.SignificantObstacle = new List<Obstruction>();

                        foreach (var obs in surf.SignificantObstacle)
                        {
                            Obstruction pdmObstruction = new Obstruction();

                            if (obs.CloseIn.HasValue) pdmObstruction.CloseIn = obs.CloseIn.Value;
                            if (obs.Controlling.HasValue) pdmObstruction.Controlling = obs.Controlling.Value;
                            if (obs.MinimumAltitude!=null) 
                            {
                                pdmObstruction.MinimumAltitude = obs.MinimumAltitude.Value;

                                UOM_DIST_HORZ uomH;
                                Enum.TryParse<UOM_DIST_HORZ>(obs.MinimumAltitude.Uom.ToString(), out uomH);
                                pdmObstruction.MinimumAltitudeUOM = uomH;
                            }
                            if (obs.RequiredClearance != null)
                            {
                                pdmObstruction.RequiredClearance = obs.RequiredClearance.Value;

                                UOM_DIST_HORZ uomH;
                                Enum.TryParse<UOM_DIST_HORZ>(obs.RequiredClearance.Uom.ToString(), out uomH);
                                pdmObstruction.RequiredClearanceUOM = uomH;
                            }
                            if (obs.SlopePenetration != null) pdmObstruction.SlopePenetration = obs.SlopePenetration.Value;
                            if (obs.SurfacePenetration != null) pdmObstruction.SurfacePenetration = obs.SurfacePenetration.Value;

                            if (obs.VerticalStructureObstruction !=null && obs.VerticalStructureObstruction.Identifier !=null)
                            pdmObstruction.VerticalStructure = (VerticalStructure)DataCash.GetPDMObject(obs.VerticalStructureObstruction.Identifier.ToString(), PDM_ENUM.VerticalStructure);

                            PdmObsAsArea.SignificantObstacle.Add(pdmObstruction);
                        }
                    }

                    pdmObj.AssessmentArea.Add(PdmObsAsArea);

                }

                #endregion

            }
            

            if (aimGeo.Count > 0 && aimGeo[0] != null)
            {
                var zAware = aimGeo[0] as IZAware;
                zAware.ZAware = true;

                var mAware = aimGeo[0] as IMAware;
                mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
            }
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

    /// <summary> VerticalStructure
    /// /////// 
    /// </summary>
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
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
                AirportAssociated = (aimObj.HostedPassengerService !=null || aimObj.SupportedGroundLight !=null || aimObj.HostedNavaidEquipment !=null ||
                 aimObj.HostedSpecialNavStation !=null || aimObj.HostedUnit !=null || aimObj.HostedOrganisation!=null || aimObj.SupportedService !=null)

            };

            if (aimObj.Annotation != null && aimObj.Annotation.Count > 0)
            {
                pdmObj.Notes = new List<string>();
                foreach (var item in  aimObj.Annotation)
                {
                    pdmObj.Notes.Add(item.PropertyName.ToString());
                }
           
           }
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
                    obs.ID = pdmObj.ID + "_" + i.ToString();
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

            if ((item.HorizontalProjection != null) && (item.HorizontalProjection.Choice == Aran.Aim.VerticalStructurePartGeometryChoice.ElevatedPoint) && (item.HorizontalProjection.Location.Elevation != null))
            {
                pdmObj.Elev = item.HorizontalProjection.Location.Elevation.Value;
                UOM_DIST_VERT _uomVert;
                if (Enum.TryParse<UOM_DIST_VERT>(item.HorizontalProjection.Location.Elevation.Uom.ToString(), out _uomVert)) pdmObj.Elev_UOM = _uomVert;

            }

            if ((item.HorizontalProjection != null) && (item.HorizontalProjection.Choice == Aran.Aim.VerticalStructurePartGeometryChoice.ElevatedSurface) && (item.HorizontalProjection.SurfaceExtent.Elevation != null))
            {
                pdmObj.Elev = item.HorizontalProjection.SurfaceExtent.Elevation.Value;
                UOM_DIST_VERT _uomVert;
                if (Enum.TryParse<UOM_DIST_VERT>(item.HorizontalProjection.SurfaceExtent.Elevation.Uom.ToString(), out _uomVert)) pdmObj.Elev_UOM = _uomVert;

            }

            if ((item.HorizontalProjection != null) && (item.HorizontalProjection.Choice == Aran.Aim.VerticalStructurePartGeometryChoice.ElevatedCurve) && (item.HorizontalProjection.LinearExtent.Elevation != null))
            {
                pdmObj.Elev = item.HorizontalProjection.LinearExtent.Elevation.Value;
                UOM_DIST_VERT _uomVert;
                if (Enum.TryParse<UOM_DIST_VERT>(item.HorizontalProjection.LinearExtent.Elevation.Uom.ToString(), out _uomVert)) pdmObj.Elev_UOM = _uomVert;

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

            if (aimGeo.GeometryType == esriGeometryType.esriGeometryPoint) ((IPoint)aimGeo).Z =pdmObj.Elev.HasValue? pdmObj.ConvertValueToMeter(pdmObj.Elev.Value, pdmObj.Elev_UOM.ToString()) : 0;
            //if (aimGeo.GeometryType == esriGeometryType.esriGeometry) ((IPoint)aimGeo).Z = pdmObj.ConvertValueToMeter(pdmObj.Elev, pdmObj.Elev_UOM.ToString());


            var mAware = aimGeo as IMAware;
            mAware.MAware = true;

            pdmObj.Geo = aimGeo;

            pdmObj.VSGeoType = VerticalStructureGeoType.OTHER;
            if (pdmObj.Geo.GeometryType == esriGeometryType.esriGeometryPoint) pdmObj.VSGeoType = VerticalStructureGeoType.POINT;
            if (pdmObj.Geo.GeometryType == esriGeometryType.esriGeometryPolygon)
                pdmObj.VSGeoType = VerticalStructureGeoType.POLYGON;
            if (pdmObj.Geo.GeometryType == esriGeometryType.esriGeometryPolyline || pdmObj.Geo.GeometryType == esriGeometryType.esriGeometryLine) pdmObj.VSGeoType = VerticalStructureGeoType.LINE;

            return pdmObj;
        }
    }

    public class AIM_Holding_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {

            Aran.Aim.Features.HoldingPattern aimObj = (Aran.Aim.Features.HoldingPattern)aimFeature;
            

            HoldingPattern pdmObj = new HoldingPattern();

            pdmObj.ID = aimObj.Identifier.ToString();
            pdmObj.OutboundCourse = aimObj.OutboundCourse != null ? aimObj.OutboundCourse.Value : Double.NaN;
            pdmObj.InboundCourse = aimObj.InboundCourse.HasValue ? aimObj.InboundCourse.Value : Double.NaN;
            pdmObj.UpperLimit = aimObj.UpperLimit != null ? aimObj.UpperLimit.Value : Double.NaN;
            pdmObj.LowerLimit = aimObj.LowerLimit != null ? aimObj.LowerLimit.Value : Double.NaN;
            pdmObj.SpeedLimit = aimObj.SpeedLimit != null ? aimObj.SpeedLimit.Value : Double.NaN;
            pdmObj.NonStandardHolding = aimObj.NonStandardHolding != null ? aimObj.NonStandardHolding.Value : false;
            pdmObj.Duration_Distance = aimObj.OutboundLegSpan != null && aimObj.OutboundLegSpan.Choice == Aran.Aim.HoldingPatternLengthChoice.HoldingPatternDistance ? aimObj.OutboundLegSpan.EndDistance.Length.Value : Double.NaN;
            pdmObj.Duration_Distance = aimObj.OutboundLegSpan != null && aimObj.OutboundLegSpan.Choice == Aran.Aim.HoldingPatternLengthChoice.HoldingPatternDuration ? aimObj.OutboundLegSpan.EndTime.Duration.Value : Double.NaN;
            pdmObj.ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now;


            #region properties

            CodeHoldingUsage _uomCodeHoldingUsage;
            if ((aimObj.Type != null) && (aimObj.Type.HasValue))
            {
                Enum.TryParse<CodeHoldingUsage>(aimObj.Type.Value.ToString(), out _uomCodeHoldingUsage);
                pdmObj.Type = _uomCodeHoldingUsage;
            }

            CodeCourse _uomCodeCourse;
            if (aimObj.OutboundCourse != null)
            {
                Enum.TryParse<CodeCourse>(aimObj.OutboundCourseType.ToString(), out _uomCodeCourse);
                pdmObj.OutboundCourseType = _uomCodeCourse;
            }

            DirectionTurnType _uomDirectionTurnType;
            if (aimObj.TurnDirection.HasValue)
            {
                Enum.TryParse<DirectionTurnType>(aimObj.TurnDirection.ToString(), out _uomDirectionTurnType);
                pdmObj.TurnDirection = _uomDirectionTurnType;
            }

            UOM_DIST_VERT _uom_DIST_VERT;
            if (aimObj.UpperLimit!= null)
            {
                Enum.TryParse<UOM_DIST_VERT>(aimObj.UpperLimit.Uom.ToString(), out _uom_DIST_VERT);
                pdmObj.UpperLimitUOM = _uom_DIST_VERT;
            }

            CODE_DIST_VER _uom_CODE_DIST_VER;
            if (aimObj.UpperLimitReference!= null)
            {
                Enum.TryParse<CODE_DIST_VER>(aimObj.UpperLimitReference.Value.ToString(), out _uom_CODE_DIST_VER);
                pdmObj.UpperLimitReference = _uom_CODE_DIST_VER;
            }

            if (aimObj.LowerLimit != null)
            {
                Enum.TryParse<UOM_DIST_VERT>(aimObj.LowerLimit.Uom.ToString(), out _uom_DIST_VERT);
                pdmObj.LowerLimitUOM = _uom_DIST_VERT;
            }

            if (aimObj.LowerLimitReference!= null)
            {
                Enum.TryParse<CODE_DIST_VER>(aimObj.LowerLimitReference.Value.ToString(), out _uom_CODE_DIST_VER);
                pdmObj.LowerLimitReference = _uom_CODE_DIST_VER;
            }

            SpeedType _uomSpeedType;
            if (aimObj.SpeedLimit != null)
            {
                Enum.TryParse<SpeedType>(aimObj.SpeedLimit.Uom.ToString(), out _uomSpeedType);
                pdmObj.SpeedLimitUOM = _uomSpeedType;
            }


            if ((aimObj.OutboundLegSpan != null) && (aimObj.OutboundLegSpan.Choice == Aran.Aim.HoldingPatternLengthChoice.HoldingPatternDistance) && (aimObj.OutboundLegSpan.EndDistance!=null))
            {
                pdmObj.Duration_Distance_UOM = aimObj.OutboundLegSpan.EndDistance.Length.Uom.ToString();
            }
            if ((aimObj.OutboundLegSpan != null) && (aimObj.OutboundLegSpan.Choice == Aran.Aim.HoldingPatternLengthChoice.HoldingPatternDuration) && (aimObj.OutboundLegSpan.EndTime != null))
            {
                pdmObj.Duration_Distance_UOM = aimObj.OutboundLegSpan.EndTime.Duration.Uom.ToString();
            }

            
            #endregion

            try
            {

               
                for (int i = 0; i < aimGeo.Count; i++)
                {
                    if (aimGeo[i].GeometryType != esriGeometryType.esriGeometryPolyline)
                        continue;

                    var zAware = aimGeo[i] as IZAware;
                    zAware.ZAware = true;

                    var mAware = aimGeo[i] as IMAware;
                    mAware.MAware = true;

                    pdmObj.Geo = aimGeo[i];
                    break;
                }

               
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return pdmObj;
        }


       }

    public class AIM_MSA_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {

            Aran.Aim.Features.SafeAltitudeArea aimObj = (Aran.Aim.Features.SafeAltitudeArea)aimFeature;


            SafeAltitudeArea pdmObj = new SafeAltitudeArea();


            pdmObj.ID = aimObj.Identifier.ToString();
            
            pdmObj.SafeAltitudeAreaSector = new List<SafeAltitudeAreaSector>();
            pdmObj.ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now;


            foreach (Aran.Aim.Features.SafeAltitudeAreaSector aimSector in aimObj.Sector)
            {
                if (aimSector.SectorDefinition == null) continue;


                SafeAltitudeAreaSector pdmsector = new SafeAltitudeAreaSector
                {
                    ID = Guid.NewGuid().ToString(),
                    FromAngle = aimSector.SectorDefinition.FromAngle.HasValue ? aimSector.SectorDefinition.FromAngle.Value : Double.NaN,
                    ToAngle = aimSector.SectorDefinition.ToAngle.HasValue ? aimSector.SectorDefinition.ToAngle.Value : Double.NaN,
                    InnerDistance = aimSector.SectorDefinition.InnerDistance != null ? aimSector.SectorDefinition.InnerDistance.Value : Double.NaN,
                    OuterDistance = aimSector.SectorDefinition.OuterDistance != null ? aimSector.SectorDefinition.OuterDistance.Value : Double.NaN,
                    UpperLimitVal = aimSector.SectorDefinition.UpperLimit != null ? aimSector.SectorDefinition.UpperLimit.Value : Double.NaN,
                    LowerLimitVal = aimSector.SectorDefinition.LowerLimit != null ? aimSector.SectorDefinition.LowerLimit.Value : Double.NaN,
                };

                #region UOM

                CodeArcDirection dir;
                if ((aimSector.SectorDefinition.ArcDirection != null) && (aimSector.SectorDefinition.ArcDirection.HasValue))
                {
                    Enum.TryParse<CodeArcDirection>(aimSector.SectorDefinition.ArcDirection.Value.ToString(), out dir);
                    pdmsector.ArcDirection = dir;
                }

                BearingType berTp;
                if ((aimSector.SectorDefinition.AngleType != null) && (aimSector.SectorDefinition.AngleType.HasValue))
                {
                    Enum.TryParse<BearingType>(aimSector.SectorDefinition.AngleType.Value.ToString(), out berTp);
                    pdmsector.AngleType = berTp;
                }

                CodeDirectionReference dirRef;
                if ((aimSector.SectorDefinition.AngleDirectionReference != null) && (aimSector.SectorDefinition.AngleDirectionReference.HasValue))
                {
                    Enum.TryParse<CodeDirectionReference>(aimSector.SectorDefinition.AngleDirectionReference.Value.ToString(), out dirRef);
                    pdmsector.AngleDirectionReference = dirRef;
                }

                UOM_DIST_VERT uomVert;
                if (aimSector.SectorDefinition.UpperLimit != null)
                {
                    Enum.TryParse<UOM_DIST_VERT>(aimSector.SectorDefinition.UpperLimit.Uom.ToString(), out uomVert);
                    pdmsector.UpperLimitUOM = uomVert;
                }

                if (aimSector.SectorDefinition.LowerLimit != null)
                {
                    Enum.TryParse<UOM_DIST_VERT>(aimSector.SectorDefinition.LowerLimit.Uom.ToString(), out uomVert);
                    pdmsector.LowerLimitUOM = uomVert;
                }

                CODE_DIST_VER refVert;
                if (aimSector.SectorDefinition.LowerLimitReference != null && aimSector.SectorDefinition.LowerLimitReference.HasValue)
                {
                    Enum.TryParse<CODE_DIST_VER>(aimSector.SectorDefinition.LowerLimitReference.Value.ToString(), out refVert);
                    pdmsector.LowerLimitReference = refVert;
                }

                if (aimSector.SectorDefinition.UpperLimitReference != null && aimSector.SectorDefinition.UpperLimitReference.HasValue)
                {
                    Enum.TryParse<CODE_DIST_VER>(aimSector.SectorDefinition.UpperLimitReference.Value.ToString(), out refVert);
                    pdmsector.UpperLimitReference = refVert;
                }

                #endregion

                

                pdmObj.SafeAltitudeAreaSector.Add(pdmsector);

            }

            return pdmObj;
        }


    }

    /// <summary>
    /// Aerordome
    /// </summary>

    public class AIM_APRON_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.Apron aimObj = (Aran.Aim.Features.Apron)aimFeature;

            Apron pdmObj = new Apron
            {
                ID = aimObj.Identifier.ToString(),

                Name = aimObj.Name,

                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
            };

            if (aimObj.SurfaceProperties != null)
            {
                SurfaceCharacteristics pdmsSurfaceCharacteristics = (SurfaceCharacteristics)AIM_PDM_Converter.AIM_Object_Convert(aimObj.SurfaceProperties, null);
                pdmsSurfaceCharacteristics.ID_Parent = aimObj.Identifier.ToString();
                pdmsSurfaceCharacteristics.ParentName = pdmObj.GetType().Name;
                pdmObj.SurfaceProperties = pdmsSurfaceCharacteristics;
            }


            return pdmObj;
        }
    }


    public class AIM_ApronElement_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.ApronElement aimObj = (Aran.Aim.Features.ApronElement)aimFeature;
            
            ApronElement pdmObj = new ApronElement
            {
                ID = aimObj.Identifier.ToString(),
                Elev = aimObj.Extent?.Elevation?.Value,                
                
            };
            
            if (aimObj.Type != null)
            {
                CodeApronElementType elementType;
                if (Enum.TryParse<CodeApronElementType>(aimObj.Type.ToString(), out elementType)) pdmObj.ElementType = elementType;
            }

            if (aimObj.Extent?.Elevation != null)
            {
                UOM_DIST_VERT uom;
                if (Enum.TryParse<UOM_DIST_VERT>(aimObj.Extent.Elevation.Uom.ToString(), out uom)) pdmObj.Elev_UOM = uom;
            }

            if (aimObj.SurfaceProperties != null)
            {
                SurfaceCharacteristics pdmsSurfaceCharacteristics = (SurfaceCharacteristics)AIM_PDM_Converter.AIM_Object_Convert(aimObj.SurfaceProperties, null);
                pdmsSurfaceCharacteristics.ID_Parent = aimObj.Identifier.ToString();
                pdmsSurfaceCharacteristics.ParentName = pdmObj.GetType().Name;
                pdmObj.SurfaceProperties = pdmsSurfaceCharacteristics;
            }


            if (aimGeo != null && aimGeo.Count > 0)
            {
                var zAware = aimGeo[0] as IZAware;
                zAware.ZAware = true;

                var mAware = aimGeo[0] as IMAware;
                mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
                pdmObj.Extent= aimGeo[0]; 
            }

            return pdmObj;
        }
    }

    public class AIM_ApronMarking_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.ApronMarking aimObj = (Aran.Aim.Features.ApronMarking)aimFeature;

            ApronMarking pdmObj = new ApronMarking
            {
                ID = aimObj.Identifier.ToString(),                
                
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
            };

            if (aimObj.Condition != null && aimObj.Condition.HasValue)
            {
                CodeMarkingConditionType arptp;
                Enum.TryParse<CodeMarkingConditionType>(aimObj.Condition.ToString(), out arptp); pdmObj.Condition = arptp;

            }
            
            if (aimObj.MarkingLocation != null && aimObj.MarkingLocation.HasValue)
            {
                CodeApronSectionType arptp;
                Enum.TryParse<CodeApronSectionType>(aimObj.MarkingLocation.ToString(), out arptp); pdmObj.MarkingLocation = arptp;

            }
            
            pdmObj.MarkingElementList = new List<MarkingElement>();

            for (int i = 0; i <= aimGeo.Count - 1; i++)
            {
                List<IGeometry> listGeom = new List<IGeometry>();
                listGeom.Add(aimGeo[i]);
                MarkingElement markElem = (MarkingElement)AIM_PDM_Converter.AIM_Object_Convert(aimObj.Element[i], listGeom);
                if (markElem != null)
                {
                    markElem.Marking_ID = pdmObj.ID;
                    markElem.MarkedElement = PDM_ENUM.ApronMarking.ToString();
                    pdmObj.MarkingElementList.Add(markElem);
                }
            }


            return pdmObj;
        }

    }

    public class AIM_RunwayMarking_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.RunwayMarking aimObj = (Aran.Aim.Features.RunwayMarking)aimFeature;

            RunwayMarking pdmObj = new RunwayMarking
            {
                ID = aimObj.Identifier.ToString(),

                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
            };

            if (aimObj.Condition != null && aimObj.Condition.HasValue)
            {
                CodeMarkingConditionType arptp;
                Enum.TryParse<CodeMarkingConditionType>(aimObj.Condition.ToString(), out arptp); pdmObj.Condition = arptp;

            }

            if (aimObj.MarkingLocation != null && aimObj.MarkingLocation.HasValue)
            {
                CodeRunwaySectionType arptp;
                Enum.TryParse<CodeRunwaySectionType>(aimObj.MarkingLocation.ToString(), out arptp); pdmObj.MarkingLocation = arptp;

            }

            pdmObj.MarkingElementList = new List<MarkingElement>();

            for (int i = 0; i <= aimGeo.Count - 1; i++)
            {
                List<IGeometry> listGeom = new List<IGeometry>();
                listGeom.Add(aimGeo[i]);
                MarkingElement markElem = (MarkingElement)AIM_PDM_Converter.AIM_Object_Convert(aimObj.Element[i], listGeom);
                if (markElem != null)
                {
                    markElem.Marking_ID = pdmObj.ID;
                    markElem.MarkedElement = PDM_ENUM.RunwayMarking.ToString();
                    pdmObj.MarkingElementList.Add(markElem);
                }
            }


            return pdmObj;
        }

    }

    public class AIM_TaxiwayMarking_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.TaxiwayMarking aimObj = (Aran.Aim.Features.TaxiwayMarking)aimFeature;

            TaxiwayMarking pdmObj = new TaxiwayMarking
            {
                ID = aimObj.Identifier.ToString(),
                //ID_TaxiwayElem = aimObj.MarkedElement.Identifier.ToString(),

                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
            };

            if (aimObj.Condition != null && aimObj.Condition.HasValue)
            {
                CodeMarkingConditionType arptp;
                Enum.TryParse<CodeMarkingConditionType>(aimObj.Condition.ToString(), out arptp); pdmObj.Condition = arptp;

            }

            if (aimObj.MarkingLocation != null && aimObj.MarkingLocation.HasValue)
            {
                CodeRunwaySectionType arptp;
                Enum.TryParse<CodeRunwaySectionType>(aimObj.MarkingLocation.ToString(), out arptp); pdmObj.MarkingLocation = arptp;

            }

            pdmObj.MarkingElementList = new List<MarkingElement>();

            for (int i = 0; i <= aimGeo.Count - 1; i++)
            {
                List<IGeometry> listGeom = new List<IGeometry>();
                listGeom.Add(aimGeo[i]);
                MarkingElement markElem = (MarkingElement)AIM_PDM_Converter.AIM_Object_Convert(aimObj.Element[i], listGeom);
                if (markElem != null)
                {
                    markElem.Marking_ID = pdmObj.ID;
                    markElem.MarkedElement = PDM_ENUM.TaxiwayMarking.ToString();
                    pdmObj.MarkingElementList.Add(markElem);
                }
            }


            return pdmObj;
        }

    }

    public class AIM_MarkingElement_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.MarkingElement aimObj = (Aran.Aim.Features.MarkingElement)aimFeature;

            MarkingElement pdmObj = new MarkingElement
            {
                ID = aimObj.Id.ToString(),
                
            };

            if (aimObj.Colour != null && aimObj.Colour.HasValue)
            {
                ColourType colourType;
                if (Enum.TryParse<ColourType>(aimObj.Colour.ToString(), out colourType)) pdmObj.Colour =colourType;
            }

            if (aimObj.Style != null && aimObj.Style.HasValue)
            {
                CodeMarkingStyleType styleType;
                if (Enum.TryParse<CodeMarkingStyleType>(aimObj.Style.ToString(), out styleType)) pdmObj.Style = styleType;
            }
            if (aimGeo != null && aimGeo.Count > 0)
            {
                var zAware = aimGeo[0] as IZAware;
                zAware.ZAware = true;

                var mAware = aimGeo[0] as IMAware;
                mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
            }

            return pdmObj;
        }
    }

    public class AIM_APRON_LIGHT_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            //AranSupport.Utilitys util = new AranSupport.Utilitys();

            Aran.Aim.Features.ApronLightSystem aimObj = (Aran.Aim.Features.ApronLightSystem)aimFeature;

            ApronLightSystem pdmObj = new ApronLightSystem
            {
                ID = aimObj.Identifier.ToString(),
                ActualDate = aimObj.TimeSlice?.ValidTime.BeginPosition ?? DateTime.Now,
                EmergencyLighting = aimObj.EmergencyLighting ?? false,

            };

            CodeLightIntensity uom_light;
            if (aimObj.IntensityLevel != null && (Enum.TryParse<CodeLightIntensity>(aimObj.IntensityLevel.ToString(), out uom_light))) pdmObj.IntensityLevel = uom_light;

            ColourType uom_clr;
            if (aimObj.Colour != null && (Enum.TryParse<ColourType>(aimObj.Colour.ToString(), out uom_clr))) pdmObj.Colour = uom_clr;


            if ((aimObj.Element != null) && (aimObj.Element.Count > 0))
            {
                pdmObj.Elements = new List<LightElement>();
                int geoIndx = 0;
                foreach (var el in aimObj.Element)
                {
                    Aran.Aim.Features.LightElement aimLightElement = (Aran.Aim.Features.LightElement)el;
                    LightElement pdmLightElement = new LightElement
                    {
                        ID = Guid.NewGuid().ToString(),
                        Intensity = aimLightElement.Intensity?.Value ?? 0,
                        Lat = ((IPoint)aimGeo[geoIndx]).Y.ToString(),
                        Lon = ((IPoint)aimGeo[geoIndx]).X.ToString(),
                        Elev = aimLightElement.Location.Elevation?.Value ?? 0,
                        LightedElement = "ApronLightingSystem",                        

                    };

                    if (aimLightElement.IntensityLevel != null && (Enum.TryParse<CodeLightIntensity>(aimLightElement.IntensityLevel.ToString(), out uom_light))) pdmLightElement.IntensityLevel = uom_light;
                    if (aimLightElement.Colour != null && (Enum.TryParse<ColourType>(aimLightElement.Colour.ToString(), out uom_clr))) pdmLightElement.Colour = uom_clr;

                    CodeLightSource _uom;
                    if (aimLightElement.Type != null && (Enum.TryParse<CodeLightSource>(aimLightElement.Type.ToString(), out _uom))) pdmLightElement.LightSourceType = _uom;

                    UOM_DIST_VERT uom_dist;
                    if ((aimLightElement.Location.Elevation != null) && (Enum.TryParse<UOM_DIST_VERT>(aimLightElement.Location.Elevation.Uom.ToString(), out uom_dist))) pdmLightElement.Elev_UOM = uom_dist;


                    var zAware = aimGeo[geoIndx] as IZAware;
                    zAware.ZAware = true;
                    ((IPoint)aimGeo[geoIndx]).Z = pdmLightElement.Elev.HasValue ? pdmLightElement.ConvertValueToMeter(pdmLightElement.Elev.Value, pdmLightElement.Elev_UOM.ToString()) : 0;


                    var mAware = aimGeo[geoIndx] as IMAware;
                    mAware.MAware = true;

                    pdmLightElement.Geo = aimGeo[geoIndx];

                    pdmObj.Elements.Add(pdmLightElement);

                    geoIndx++;

                }

            }


            return pdmObj;
        }
    }

    public class AIM_DeicingArea_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.DeicingArea aimObj = (Aran.Aim.Features.DeicingArea)aimFeature;

            DeicingArea pdmObj = new DeicingArea
            {
                ID = aimObj.Identifier.ToString(),

            };

            if (aimObj.AssociatedApron != null )
            {
                string guid = aimObj.AssociatedApron.Identifier.ToString();
                var type = PDM_ENUM.Apron.ToString();
                pdmObj.ParentList.Add(type, guid);
            }

            if (aimObj.StandLocation != null)
            {
                string guid = aimObj.StandLocation.Identifier.ToString();
                var type = PDM_ENUM.AircraftStand.ToString();
                pdmObj.ParentList.Add(type, guid);
            }


            if (aimObj.TaxiwayLocation != null)
            {
                string guid = aimObj.TaxiwayLocation.Identifier.ToString();
                var type = PDM_ENUM.Taxiway.ToString();
                pdmObj.ParentList.Add(type, guid);
            }

            if (aimObj.SurfaceProperties != null)
            {
                SurfaceCharacteristics pdmsSurfaceCharacteristics = (SurfaceCharacteristics)AIM_PDM_Converter.AIM_Object_Convert(aimObj.SurfaceProperties, null);
                pdmsSurfaceCharacteristics.ID_Parent = aimObj.Identifier.ToString();
                pdmsSurfaceCharacteristics.ParentName = pdmObj.GetType().Name;
                pdmObj.SurfaceProperties = pdmsSurfaceCharacteristics;
            }

            if (aimGeo != null && aimGeo.Count > 0)
            {
                var zAware = aimGeo[0] as IZAware;
                if (zAware != null) zAware.ZAware = true;

                var mAware = aimGeo[0] as IMAware;
                if (mAware != null) mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
            }

            return pdmObj;
        }
    }

    public class AIM_DeicingAreaMarking_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.DeicingAreaMarking aimObj = (Aran.Aim.Features.DeicingAreaMarking)aimFeature;

            DeicingAreaMarking pdmObj = new DeicingAreaMarking
            {
                ID = aimObj.Identifier.ToString(),

                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
            };

            if (aimObj.Condition != null && aimObj.Condition.HasValue)
            {
                CodeMarkingConditionType arptp;
                Enum.TryParse<CodeMarkingConditionType>(aimObj.Condition.ToString(), out arptp); pdmObj.Condition = arptp;

            }

            pdmObj.MarkingElementList = new List<MarkingElement>();

            for (int i = 0; i <= aimGeo.Count - 1; i++)
            {
                List<IGeometry> listGeom = new List<IGeometry>();
                listGeom.Add(aimGeo[i]);
                MarkingElement markElem = (MarkingElement)AIM_PDM_Converter.AIM_Object_Convert(aimObj.Element[i], listGeom);
                if (markElem != null)
                {
                    markElem.Marking_ID = pdmObj.ID;
                    markElem.MarkedElement = PDM_ENUM.DeicingAreaMarking.ToString();
                    pdmObj.MarkingElementList.Add(markElem);
                }
            }


            return pdmObj;
        }

    }

    public class AIM_AircraftStand_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.AircraftStand aimObj = (Aran.Aim.Features.AircraftStand)aimFeature;

            AircraftStand pdmObj = new AircraftStand
            {
                ID = aimObj.Identifier.ToString(),
                Lat = ((IPoint)aimGeo[0]).Y.ToString(),
                Lon = ((IPoint)aimGeo[0]).X.ToString(),


            };
            if (aimObj.Designator != null)
            {
                pdmObj.Designator = aimObj.Designator;
            }
            if (aimObj.Type != null)
            {
                CodeAircraftStandType standType;
                if (Enum.TryParse<CodeAircraftStandType>(aimObj.Type.ToString(), out standType)) pdmObj.AircraftStandType = standType;
            }

            if (aimObj.VisualDockingSystem != null && aimObj.VisualDockingSystem.HasValue)
            {
                CodeVisualDockingGuidanceType docGuidanceType;
                if (Enum.TryParse<CodeVisualDockingGuidanceType>(aimObj.VisualDockingSystem.ToString(), out docGuidanceType)) pdmObj.VisualDockingGuidanceType = docGuidanceType;
            }

            if (aimObj.SurfaceProperties != null)
            {
                SurfaceCharacteristics pdmsSurfaceCharacteristics = (SurfaceCharacteristics)AIM_PDM_Converter.AIM_Object_Convert(aimObj.SurfaceProperties, null);
                pdmsSurfaceCharacteristics.ID_Parent = aimObj.Identifier.ToString();
                pdmsSurfaceCharacteristics.ParentName = pdmObj.GetType().Name;
                pdmObj.SurfaceProperties = pdmsSurfaceCharacteristics;
            }

            if (aimObj.Extent?.Geo != null)
            {
                
                var extent = ConvertToEsriGeom.FromGeometry(aimObj.Extent.Geo,true,GeometryFormatter.Wgs1984Reference());

                var zAware = extent as IZAware;
                zAware.ZAware = false;

                var mAware = extent as IMAware;
                mAware.MAware = false;

                pdmObj.Extent = extent;
            }

            if (aimGeo != null && aimGeo.Count > 0)
            {
                var zAware = aimGeo[0] as IZAware;
                zAware.ZAware = true;

                //var mAware = aimGeo[0] as IMAware;
                //mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
            }

            return pdmObj;
        }
    }

    public class AIM_SurfaceCharacteristics_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.SurfaceCharacteristics aimObj = (Aran.Aim.Features.SurfaceCharacteristics)aimFeature;

            SurfaceCharacteristics pdmObj = new SurfaceCharacteristics
            {
                ID =Guid.NewGuid().ToString(),
                ClassLCN = aimObj.ClassLCN != null
                    ? aimObj.ClassLCN.Value
                    : Double.NaN,
                ClassPCN = aimObj.ClassPCN != null
                    ? aimObj.ClassPCN.Value
                    : Double.NaN,
                WeightSIWL = aimObj.WeightSIWL != null
                    ? aimObj.WeightSIWL.Value
                    : Double.NaN,
                WeightAUW = aimObj.WeightAUW != null
                    ? aimObj.WeightAUW.Value
                    : Double.NaN,
                TyrePressureSIWL = aimObj.TyrePressureSIWL != null
                    ? aimObj.TyrePressureSIWL.Value
                    : Double.NaN,
            };

            if (aimObj.Composition != null && aimObj.Composition.HasValue)
            {
                CodeSurfaceCompositionType compType;
                if (Enum.TryParse<CodeSurfaceCompositionType>(aimObj.Composition.ToString(), out compType)) pdmObj.Composition = compType;
            }

            if (aimObj.Preparation != null && aimObj.Preparation.HasValue)
            {
                CodeSurfacePreparationType prepType;
                if (Enum.TryParse<CodeSurfacePreparationType>(aimObj.Preparation.ToString(), out prepType)) pdmObj.Preparation = prepType;
            }

            if (aimObj.SurfaceCondition != null && aimObj.SurfaceCondition.HasValue)
            {
                CodeSurfaceConditionType condType;
                if (Enum.TryParse<CodeSurfaceConditionType>(aimObj.SurfaceCondition.ToString(), out condType)) pdmObj.SurfaceCondition = condType;
            }

            if (aimObj.PavementTypePCN != null && aimObj.PavementTypePCN.HasValue)
            {
                CodePCNPavementType pavTypePCN;
                if (Enum.TryParse<CodePCNPavementType>(aimObj.PavementTypePCN.ToString(), out pavTypePCN)) pdmObj.PavementTypePCN = pavTypePCN;
            }

            if (aimObj.PavementSubgradePCN != null && aimObj.PavementSubgradePCN.HasValue)
            {
                CodePCNSubgradeType subTypePCN;
                if (Enum.TryParse<CodePCNSubgradeType>(aimObj.PavementSubgradePCN.ToString(), out subTypePCN)) pdmObj.PavementSubgradePCN = subTypePCN;
            }

            if (aimObj.MaxTyrePressurePCN != null && aimObj.MaxTyrePressurePCN.HasValue)
            {
                CodePCNTyrePressureType maxTyrePressureTypeType;
                if (Enum.TryParse<CodePCNTyrePressureType>(aimObj.MaxTyrePressurePCN.ToString(), out maxTyrePressureTypeType)) pdmObj.MaxTyrePressurePCN = maxTyrePressureTypeType;
            }

            if (aimObj.EvaluationMethodPCN != null && aimObj.EvaluationMethodPCN.HasValue)
            {
                CodePCNMethodType methodType;
                if (Enum.TryParse<CodePCNMethodType>(aimObj.EvaluationMethodPCN.ToString(), out methodType)) pdmObj.EvaluationMethodPCN = methodType;
            }

            if (aimObj.TyrePressureSIWL != null )
            {                
                pdmObj.TyrePressureSIWL_UOM = aimObj.TyrePressureSIWL.Uom.ToString();
            }

            if (aimObj.TyrePressureSIWL != null)
            {
                pdmObj.TyrePressureSIWL_UOM = aimObj.TyrePressureSIWL.Uom.ToString();
            }

            if (aimObj.TyrePressureSIWL != null)
            {
                pdmObj.TyrePressureSIWL_UOM = aimObj.TyrePressureSIWL.Uom.ToString();
            }


            return pdmObj;
        }
    }

    public class AIM_StandMarking_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.StandMarking aimObj = (Aran.Aim.Features.StandMarking)aimFeature;

            StandMarking pdmObj = new StandMarking
            {
                ID = aimObj.Identifier.ToString(),

                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
            };

            if (aimObj.Condition != null && aimObj.Condition.HasValue)
            {
                CodeMarkingConditionType arptp;
                Enum.TryParse<CodeMarkingConditionType>(aimObj.Condition.ToString(), out arptp); pdmObj.Condition = arptp;

            }

            

            pdmObj.MarkingElementList = new List<MarkingElement>();

            for (int i = 0; i <= aimGeo.Count - 1; i++)
            {
                List<IGeometry> listGeom = new List<IGeometry>();
                listGeom.Add(aimGeo[i]);
                MarkingElement markElem = (MarkingElement)AIM_PDM_Converter.AIM_Object_Convert(aimObj.Element[i], listGeom);
                if (markElem != null)
                {
                    markElem.Marking_ID = pdmObj.ID;
                    markElem.MarkedElement = PDM_ENUM.StandMarking.ToString();
                    pdmObj.MarkingElementList.Add(markElem);
                }
            }


            return pdmObj;
        }

    }

    public class AIM_GuidanceLine_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.GuidanceLine aimObj = (Aran.Aim.Features.GuidanceLine)aimFeature;

            GuidanceLine pdmObj = new GuidanceLine
            {
                ID = aimObj.Id.ToString(),
                Designator = aimObj.Designator,
                MaxSpeed = aimObj.MaxSpeed?.Value ?? Double.NaN,
            };

            

            if (aimObj.ConnectedApron != null && aimObj.ConnectedApron.Count > 0)
            {
                foreach (var apron in aimObj.ConnectedApron)
                {
                    string guid = apron.Feature.Identifier.ToString();
                    var type = PDM_ENUM.Apron.ToString();
                    pdmObj.ParentList.Add(guid, type);
                }
                
            }

            if (aimObj.ConnectedRunwayCentrelinePoint != null && aimObj.ConnectedRunwayCentrelinePoint.Count>0)
            {
                foreach (var clp in aimObj.ConnectedRunwayCentrelinePoint)
                {
                    string guid = clp.Feature.Identifier.ToString();
                    var type = PDM_ENUM.RunwayCenterLinePoint.ToString();
                    pdmObj.ParentList.Add(guid, type);
                }
                
            }

            if (aimObj.ConnectedStand != null && aimObj.ConnectedStand.Count > 0)
            {
                foreach (var stand in aimObj.ConnectedStand)
                {
                    string guid = stand.Feature.Identifier.ToString();
                    var type = PDM_ENUM.AircraftStand.ToString();
                    pdmObj.ParentList.Add(guid, type);
                }
               
            }

            if (aimObj.ConnectedTaxiway != null && aimObj.ConnectedTaxiway.Count > 0)
            {
                foreach (var taxi in aimObj.ConnectedTaxiway)
                {
                    string guid = taxi.Feature.Identifier.ToString();
                    var type = PDM_ENUM.Taxiway.ToString();
                    pdmObj.ParentList.Add(guid,type);
                }

            }

            if (aimObj.ConnectedTouchDownLiftOff != null && aimObj.ConnectedTouchDownLiftOff.Count > 0)
            {
                string guid = aimObj.ConnectedTouchDownLiftOff[0].Feature.Identifier.ToString();
                var type = PDM_ENUM.TouchDownLiftOff.ToString();
                pdmObj.ParentList.Add(guid, type);
            }

            if (aimObj.MaxSpeed != null)
            {
                SpeedType arptp;
                Enum.TryParse<SpeedType>(aimObj.MaxSpeed.Uom.ToString(), out arptp); pdmObj.MaxSpeed_UOM = arptp;

            }

            if (aimObj.Type != null)
            {
                CodeGuidanceLineType arptp;
                Enum.TryParse<CodeGuidanceLineType>(aimObj.Type.ToString(), out arptp); pdmObj.Type = arptp;

            }

            if (aimObj.UsageDirection != null)
            {
                CodeDirectionType usageDir;
                Enum.TryParse<CodeDirectionType>(aimObj.UsageDirection.ToString(), out usageDir); pdmObj.UsageDirection = usageDir;

            }

            if (aimGeo != null && aimGeo.Count > 0)
            {
                var zAware = aimGeo[0] as IZAware;
                zAware.ZAware = true;

                var mAware = aimGeo[0] as IMAware;
                mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
            }

            return pdmObj;
        }
    }

    public class AIM_GuidanceLine_LIGHT_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            //AranSupport.Utilitys util = new AranSupport.Utilitys();

            Aran.Aim.Features.GuidanceLineLightSystem aimObj = (Aran.Aim.Features.GuidanceLineLightSystem)aimFeature;

            GuidanceLineLightSystem pdmObj = new GuidanceLineLightSystem
            {
                ID = aimObj.Identifier.ToString(),
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
                EmergencyLighting = aimObj.EmergencyLighting != null && aimObj.EmergencyLighting.HasValue ? aimObj.EmergencyLighting.Value : false,

            };

            CodeLightIntensity uom_light;
            if (aimObj.IntensityLevel != null && (aimObj.IntensityLevel.HasValue) && (Enum.TryParse<CodeLightIntensity>(aimObj.IntensityLevel.ToString(), out uom_light))) pdmObj.IntensityLevel = uom_light;

            ColourType uom_clr;
            if (aimObj.Colour != null && (aimObj.Colour.HasValue) && (Enum.TryParse<ColourType>(aimObj.Colour.ToString(), out uom_clr))) pdmObj.Colour = uom_clr;


            if ((aimObj.Element != null) && (aimObj.Element.Count > 0))
            {
                pdmObj.Elements = new List<LightElement>();
                int geoIndx = 0;
                foreach (var el in aimObj.Element)
                {
                    Aran.Aim.Features.LightElement aimLightElement = (Aran.Aim.Features.LightElement)el;
                    LightElement pdmLightElement = new LightElement
                    {
                        ID = Guid.NewGuid().ToString(),
                        Intensity = aimLightElement.Intensity != null ? aimLightElement.Intensity.Value : 0,
                        Lat = ((IPoint)aimGeo[geoIndx]).Y.ToString(),
                        Lon = ((IPoint)aimGeo[geoIndx]).X.ToString(),
                        Elev = aimLightElement.Location.Elevation != null ? aimLightElement.Location.Elevation.Value : 0,
                        LightedElement = "GuidanceLineLightingSystem",

                    };

                    if (aimLightElement.IntensityLevel != null && (aimLightElement.IntensityLevel.HasValue) && (Enum.TryParse<CodeLightIntensity>(aimLightElement.IntensityLevel.ToString(), out uom_light))) pdmLightElement.IntensityLevel = uom_light;
                    if (aimLightElement.Colour != null && (aimLightElement.Colour.HasValue) && (Enum.TryParse<ColourType>(aimLightElement.Colour.ToString(), out uom_clr))) pdmLightElement.Colour = uom_clr;

                    CodeLightSource _uom;
                    if (aimLightElement.Type != null && (aimLightElement.Type.HasValue) && (Enum.TryParse<CodeLightSource>(aimLightElement.Type.ToString(), out _uom))) pdmLightElement.LightSourceType = _uom;

                    UOM_DIST_VERT uom_dist;
                    if ((aimLightElement.Location.Elevation != null) && (Enum.TryParse<UOM_DIST_VERT>(aimLightElement.Location.Elevation.Uom.ToString(), out uom_dist))) pdmLightElement.Elev_UOM = uom_dist;


                    var zAware = aimGeo[geoIndx] as IZAware;
                    zAware.ZAware = true;
                    ((IPoint)aimGeo[geoIndx]).Z = pdmLightElement.Elev.HasValue ? pdmLightElement.ConvertValueToMeter(pdmLightElement.Elev.Value, pdmLightElement.Elev_UOM.ToString()) : 0;


                    var mAware = aimGeo[geoIndx] as IMAware;
                    mAware.MAware = true;

                    pdmLightElement.Geo = aimGeo[geoIndx];

                    pdmObj.Elements.Add(pdmLightElement);

                    geoIndx++;

                }

            }


            return pdmObj;
        }
    }

    public class AIM_Taxiway_LIGHT_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            //AranSupport.Utilitys util = new AranSupport.Utilitys();

            Aran.Aim.Features.TaxiwayLightSystem aimObj = (Aran.Aim.Features.TaxiwayLightSystem)aimFeature;

            TaxiwayLightSystem pdmObj = new TaxiwayLightSystem
            {
                ID = aimObj.Identifier.ToString(),
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
                EmergencyLighting = aimObj.EmergencyLighting != null && aimObj.EmergencyLighting.HasValue ? aimObj.EmergencyLighting.Value : false,

            };

            CodeTaxiwaySectionType taxSecType;
            if (aimObj.Position != null && (aimObj.Position.HasValue) && (Enum.TryParse<CodeTaxiwaySectionType>(aimObj.Position.ToString(), out taxSecType))) pdmObj.Position = taxSecType;

            CodeLightIntensity uom_light;
            if (aimObj.IntensityLevel != null && (aimObj.IntensityLevel.HasValue) && (Enum.TryParse<CodeLightIntensity>(aimObj.IntensityLevel.ToString(), out uom_light))) pdmObj.IntensityLevel = uom_light;

            ColourType uom_clr;
            if (aimObj.Colour != null && (aimObj.Colour.HasValue) && (Enum.TryParse<ColourType>(aimObj.Colour.ToString(), out uom_clr))) pdmObj.Colour = uom_clr;


            if ((aimObj.Element != null) && (aimObj.Element.Count > 0))
            {
                pdmObj.Elements = new List<LightElement>();
                int geoIndx = 0;
                foreach (var el in aimObj.Element)
                {
                    Aran.Aim.Features.LightElement aimLightElement = (Aran.Aim.Features.LightElement)el;
                    LightElement pdmLightElement = new LightElement
                    {
                        ID = Guid.NewGuid().ToString(),
                        Intensity = aimLightElement.Intensity != null ? aimLightElement.Intensity.Value : 0,
                        Lat = ((IPoint)aimGeo[geoIndx]).Y.ToString(),
                        Lon = ((IPoint)aimGeo[geoIndx]).X.ToString(),
                        Elev = aimLightElement.Location.Elevation != null ? aimLightElement.Location.Elevation.Value : 0,
                        LightedElement = "TaxiwayLightingSystem",

                    };

                    if (aimLightElement.IntensityLevel != null && (aimLightElement.IntensityLevel.HasValue) && (Enum.TryParse<CodeLightIntensity>(aimLightElement.IntensityLevel.ToString(), out uom_light))) pdmLightElement.IntensityLevel = uom_light;
                    if (aimLightElement.Colour != null && (aimLightElement.Colour.HasValue) && (Enum.TryParse<ColourType>(aimLightElement.Colour.ToString(), out uom_clr))) pdmLightElement.Colour = uom_clr;

                    CodeLightSource _uom;
                    if (aimLightElement.Type != null && (aimLightElement.Type.HasValue) && (Enum.TryParse<CodeLightSource>(aimLightElement.Type.ToString(), out _uom))) pdmLightElement.LightSourceType = _uom;

                    UOM_DIST_VERT uom_dist;
                    if ((aimLightElement.Location.Elevation != null) && (Enum.TryParse<UOM_DIST_VERT>(aimLightElement.Location.Elevation.Uom.ToString(), out uom_dist))) pdmLightElement.Elev_UOM = uom_dist;


                    var zAware = aimGeo[geoIndx] as IZAware;
                    zAware.ZAware = true;
                    ((IPoint)aimGeo[geoIndx]).Z = pdmLightElement.Elev.HasValue ? pdmLightElement.ConvertValueToMeter(pdmLightElement.Elev.Value, pdmLightElement.Elev_UOM.ToString()) : 0;


                    var mAware = aimGeo[geoIndx] as IMAware;
                    mAware.MAware = true;

                    pdmLightElement.Geo = aimGeo[geoIndx];

                    pdmObj.Elements.Add(pdmLightElement);

                    geoIndx++;

                }

            }


            return pdmObj;
        }
    }

    public class AIM_GuidanceLineMarking_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.GuidanceLineMarking aimObj = (Aran.Aim.Features.GuidanceLineMarking)aimFeature;

            GuidanceLineMarking pdmObj = new GuidanceLineMarking
            {
                ID = aimObj.Identifier.ToString(),

                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
            };

            if (aimObj.Condition != null && aimObj.Condition.HasValue)
            {
                CodeMarkingConditionType arptp;
                Enum.TryParse<CodeMarkingConditionType>(aimObj.Condition.ToString(), out arptp); pdmObj.Condition = arptp;

            }


            pdmObj.MarkingElementList = new List<MarkingElement>();

            for (int i = 0; i <= aimGeo.Count - 1; i++)
            {
                List<IGeometry> listGeom = new List<IGeometry>();
                listGeom.Add(aimGeo[i]);
                MarkingElement markElem = (MarkingElement)AIM_PDM_Converter.AIM_Object_Convert(aimObj.Element[i], listGeom);
                if (markElem != null)
                {
                    markElem.Marking_ID = pdmObj.ID;
                    markElem.MarkedElement = PDM_ENUM.GuidanceLineMarking.ToString();
                    pdmObj.MarkingElementList.Add(markElem);
                }
            }


            return pdmObj;
        }

    }

    public class AIM_RadioCommunicationChanel_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.RadioCommunicationChannel aimObj = (Aran.Aim.Features.RadioCommunicationChannel)aimFeature;

            RadioCommunicationChanel pdmObj = new RadioCommunicationChanel
            {
                ID = aimObj.Identifier.ToString(),
               
            };


            pdmObj.FrequencyReception = aimObj.FrequencyReception?.Value ?? 0;

            pdmObj.FrequencyTransmission = aimObj.FrequencyTransmission?.Value ?? 0;

            if (aimObj.Logon != null)
                pdmObj.Logon = aimObj.Logon;

                UOM_FREQ frqUom;
            if (aimObj.FrequencyReception != null)
            {
                Enum.TryParse<UOM_FREQ>(aimObj.FrequencyReception.Uom.ToString(), out frqUom);
                pdmObj.ReceptionFrequencyUOM = frqUom;
            }

            if (aimObj.FrequencyTransmission != null)
            {
                Enum.TryParse<UOM_FREQ>(aimObj.FrequencyTransmission.Uom.ToString(), out frqUom);
                pdmObj.TransmissionFrequencyUOM = frqUom;
            }

            PDM.CodeFacilityRanking rnk;
            if (aimObj.Rank != null)
            {
                Enum.TryParse<PDM.CodeFacilityRanking>(aimObj.Rank.ToString(), out rnk);
                pdmObj.Rank = rnk;
            }

            CodeCommunicationModeType mode;
            if (aimObj.Mode != null)
            {
                Enum.TryParse<CodeCommunicationModeType>(aimObj.Mode.ToString(), out mode);
                pdmObj.Mode = mode;
            }

            CodeRadioEmissionType emisType;
            if (aimObj.EmissionType != null)
            {
                Enum.TryParse<CodeRadioEmissionType>(aimObj.EmissionType.ToString(), out emisType);
                pdmObj.EmissionType = emisType;
            }

            CodeCommunicationDirectionType trafDir;
            if (aimObj.TrafficDirection != null)
            {
                Enum.TryParse<CodeCommunicationDirectionType>(aimObj.TrafficDirection.ToString(), out trafDir);
                pdmObj.TrafficDirection = trafDir;
            }

            if (aimGeo != null && aimGeo.Count > 0)
            {
                var zAware = aimGeo[0] as IZAware;
                zAware.ZAware = true;

                var mAware = aimGeo[0] as IMAware;
                mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
            }


            return pdmObj;
        }
    }

    public class AIM_CheckpointVOR_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.CheckpointVOR aimObj = (Aran.Aim.Features.CheckpointVOR)aimFeature;

            CheckpointVOR pdmObj = new CheckpointVOR
            {
                ID = aimObj.Identifier.ToString(),                
                UpperLimit = aimObj.UpperLimit?.Value ?? Double.NaN,
                LowerLimit = aimObj.LowerLimit?.Value ?? Double.NaN,
                Distance = aimObj.Distance?.Value ?? Double.NaN,
                Angle = aimObj.Angle ?? Double.NaN,
                Annotation = aimObj.Annotation.ToString(),
                ActualDate = aimObj.TimeSlice?.ValidTime.BeginPosition ?? DateTime.Now,
                ID_VOR = aimObj.CheckPointFacility.Identifier.ToString()
            };

            
            if (aimObj.Category != null)
            {
                CodeCommunicationDirectionType categ;
                Enum.TryParse<CodeCommunicationDirectionType>(aimObj.Category.ToString(), out categ);
                pdmObj.Category = categ;

            }
            
            if (aimObj.UpperLimitReference != null)
            {
                CodeVerticalReference uppLim;
                Enum.TryParse<CodeVerticalReference>(aimObj.UpperLimitReference.ToString(), out uppLim);
                pdmObj.UpperLimitReference = uppLim;

            }

            if (aimObj.LowerLimitReference != null)
            {
                CodeVerticalReference lowLim;
                Enum.TryParse<CodeVerticalReference>(aimObj.LowerLimitReference.ToString(), out lowLim);
                pdmObj.LowerLimitReference = lowLim;

            }

            if (aimObj.AltitudeInterpretation != null)
            {
                AltitudeUseType altInter;
                Enum.TryParse<AltitudeUseType>(aimObj.AltitudeInterpretation.ToString(), out altInter);
                pdmObj.AltitudeInterpretation = altInter;

            }

            if (aimObj.UpperLimit != null)
            {
                UOM_DIST_VERT categ;
                Enum.TryParse<UOM_DIST_VERT>(aimObj.UpperLimit.Uom.ToString(), out categ);
                pdmObj.UpperLimit_UOM = categ;

            }

            if (aimObj.LowerLimit != null)
            {
                UOM_DIST_VERT categ;
                Enum.TryParse<UOM_DIST_VERT>(aimObj.LowerLimit.Uom.ToString(), out categ);
                pdmObj.LowerLimit_UOM = categ;

            }

            if (aimObj.Distance != null)
            {
                UOM_DIST_HORZ categ;
                Enum.TryParse<UOM_DIST_HORZ>(aimObj.Distance.Uom.ToString(), out categ);
                pdmObj.Distance_UOM = categ;

            }

            if (aimGeo != null && aimGeo.Count > 0)
            {
                var zAware = aimGeo[0] as IZAware;
                zAware.ZAware = true;

                var mAware = aimGeo[0] as IMAware;
                mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
            }


            return pdmObj;
        }

    }

    public class AIM_CheckpointINS_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.CheckpointINS aimObj = (Aran.Aim.Features.CheckpointINS)aimFeature;

            CheckpointINS pdmObj = new CheckpointINS
            {
                ID = aimObj.Identifier.ToString(),
                UpperLimit = aimObj.UpperLimit?.Value ?? Double.NaN,
                LowerLimit = aimObj.LowerLimit?.Value ?? Double.NaN,
                Distance = aimObj.Distance?.Value ?? Double.NaN,
                Angle = aimObj.Angle ?? Double.NaN,
                Annotation = aimObj.Annotation.ToString(),
                ActualDate = aimObj.TimeSlice?.ValidTime.BeginPosition ?? DateTime.Now,
                
            };
            
            if (aimObj.Category != null)
            {
                CodeCommunicationDirectionType categ;
                Enum.TryParse<CodeCommunicationDirectionType>(aimObj.Category.ToString(), out categ);
                pdmObj.Category = categ;

            }
            
            if (aimObj.UpperLimitReference != null)
            {
                CodeVerticalReference categ;
                Enum.TryParse<CodeVerticalReference>(aimObj.UpperLimitReference.ToString(), out categ);
                pdmObj.UpperLimitReference = categ;

            }

            if (aimObj.LowerLimitReference != null)
            {
                CodeVerticalReference categ;
                Enum.TryParse<CodeVerticalReference>(aimObj.LowerLimitReference.ToString(), out categ);
                pdmObj.LowerLimitReference = categ;

            }

            if (aimObj.AltitudeInterpretation != null)
            {
                AltitudeUseType categ;
                Enum.TryParse<AltitudeUseType>(aimObj.AltitudeInterpretation.ToString(), out categ);
                pdmObj.AltitudeInterpretation = categ;

            }

            if (aimObj.UpperLimit != null )
            {
                UOM_DIST_VERT categ;
                Enum.TryParse<UOM_DIST_VERT>(aimObj.UpperLimit.Uom.ToString(), out categ);
                pdmObj.UpperLimit_UOM = categ;

            }
            
            if (aimObj.LowerLimit != null)
            {
                UOM_DIST_VERT categ;
                Enum.TryParse<UOM_DIST_VERT>(aimObj.LowerLimit.Uom.ToString(), out categ);
                pdmObj.LowerLimit_UOM = categ;

            }

            if (aimObj.Distance != null)
            {
                UOM_DIST_HORZ categ;
                Enum.TryParse<UOM_DIST_HORZ>(aimObj.Distance.Uom.ToString(), out categ);
                pdmObj.Distance_UOM = categ;

            }

            if (aimGeo != null && aimGeo.Count > 0)
            {
                var zAware = aimGeo[0] as IZAware;
                zAware.ZAware = true;

                var mAware = aimGeo[0] as IMAware;
                mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
            }


            return pdmObj;
        }

    }

    public class AIM_RwyProtectionArea_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.RunwayProtectArea aimObj = (Aran.Aim.Features.RunwayProtectArea)aimFeature;

            RunwayProtectArea pdmObj = new RunwayProtectArea
            {
                ID = aimObj.Identifier.ToString(),
                
                Width = aimObj.Width?.Value ?? Double.NaN,
                Length = aimObj.Length?.Value ?? Double.NaN,
                Lighting = aimObj.Lighting != null && aimObj.Lighting.Value,
                ObstacleFree = aimObj.ObstacleFree != null && aimObj.ObstacleFree.Value,
                ActualDate = aimObj.TimeSlice?.ValidTime.BeginPosition ?? DateTime.Now,

            };
            
            if (aimObj.Status != null)
            {
                CodeStatusOperationsType status;
                Enum.TryParse<CodeStatusOperationsType>(aimObj.Status.ToString(), out status);
                pdmObj.Status = status;

            }

            if (aimObj.Type != null)
            {
                CodeRunwayProtectionAreaType type;
                Enum.TryParse<CodeRunwayProtectionAreaType>(aimObj.Type.ToString(), out type);
                pdmObj.Type = type;

            }

            if (aimObj.Width != null)
            {
                UOM_DIST_HORZ status;
                Enum.TryParse<UOM_DIST_HORZ>(aimObj.Width.Uom.ToString(), out status);
                pdmObj.Width_UOM = status;

            }

            if (aimObj.Length != null)
            {
                UOM_DIST_HORZ status;
                Enum.TryParse<UOM_DIST_HORZ>(aimObj.Length.Uom.ToString(), out status);
                pdmObj.Length_UOM = status;

            }


            if (aimObj.SurfaceProperties != null)
            {
                SurfaceCharacteristics pdmsSurfaceCharacteristics = (SurfaceCharacteristics)AIM_PDM_Converter.AIM_Object_Convert(aimObj.SurfaceProperties, null);
                pdmsSurfaceCharacteristics.ID_Parent = aimObj.Identifier.ToString();
                pdmsSurfaceCharacteristics.ParentName = pdmObj.GetType().Name;
                pdmObj.SurfaceProperties = pdmsSurfaceCharacteristics;
            }

            if (aimGeo != null && aimGeo.Count > 0)
            {
                var zAware = aimGeo[0] as IZAware;
                if (zAware != null) zAware.ZAware = true;

                var mAware = aimGeo[0] as IMAware;
                if (mAware != null) mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
            }


            return pdmObj;
        }

    }


    public class AIM_RPA_LIGHT_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {

            Aran.Aim.Features.RunwayProtectAreaLightSystem aimObj = (Aran.Aim.Features.RunwayProtectAreaLightSystem)aimFeature;

            RunwayProtectAreaLightSystem pdmObj = new RunwayProtectAreaLightSystem
            {
                ID = aimObj.Identifier.ToString(),
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
                EmergencyLighting = aimObj.EmergencyLighting != null && aimObj.EmergencyLighting.HasValue ? aimObj.EmergencyLighting.Value : false,

            };

            CodeProtectAreaSectionType rpaSecType;
            if (aimObj.Position != null && (aimObj.Position.HasValue) && (Enum.TryParse<CodeProtectAreaSectionType>(aimObj.Position.ToString(), out rpaSecType))) pdmObj.Position = rpaSecType;

            CodeLightIntensity uom_light;
            if (aimObj.IntensityLevel != null && (aimObj.IntensityLevel.HasValue) && (Enum.TryParse<CodeLightIntensity>(aimObj.IntensityLevel.ToString(), out uom_light))) pdmObj.IntensityLevel = uom_light;

            ColourType uom_clr;
            if (aimObj.Colour != null && (aimObj.Colour.HasValue) && (Enum.TryParse<ColourType>(aimObj.Colour.ToString(), out uom_clr))) pdmObj.Colour = uom_clr;


            if ((aimObj.Element != null) && (aimObj.Element.Count > 0))
            {
                pdmObj.Elements = new List<LightElement>();
                int geoIndx = 0;
                foreach (var el in aimObj.Element)
                {
                    Aran.Aim.Features.LightElement aimLightElement = (Aran.Aim.Features.LightElement)el;
                    LightElement pdmLightElement = new LightElement
                    {
                        ID = Guid.NewGuid().ToString(),
                        Intensity = aimLightElement.Intensity != null ? aimLightElement.Intensity.Value : 0,
                        Lat = ((IPoint)aimGeo[geoIndx]).Y.ToString(),
                        Lon = ((IPoint)aimGeo[geoIndx]).X.ToString(),
                        Elev = aimLightElement.Location.Elevation != null ? aimLightElement.Location.Elevation.Value : 0,
                        LightedElement = "RunwayProtectAreaLightSystem",

                    };

                    if (aimLightElement.IntensityLevel != null && (aimLightElement.IntensityLevel.HasValue) && (Enum.TryParse<CodeLightIntensity>(aimLightElement.IntensityLevel.ToString(), out uom_light))) pdmLightElement.IntensityLevel = uom_light;
                    if (aimLightElement.Colour != null && (aimLightElement.Colour.HasValue) && (Enum.TryParse<ColourType>(aimLightElement.Colour.ToString(), out uom_clr))) pdmLightElement.Colour = uom_clr;

                    CodeLightSource _uom;
                    if (aimLightElement.Type != null && (aimLightElement.Type.HasValue) && (Enum.TryParse<CodeLightSource>(aimLightElement.Type.ToString(), out _uom))) pdmLightElement.LightSourceType = _uom;

                    UOM_DIST_VERT uom_dist;
                    if ((aimLightElement.Location.Elevation != null) && (Enum.TryParse<UOM_DIST_VERT>(aimLightElement.Location.Elevation.Uom.ToString(), out uom_dist))) pdmLightElement.Elev_UOM = uom_dist;


                    var zAware = aimGeo[geoIndx] as IZAware;
                    zAware.ZAware = true;
                    ((IPoint)aimGeo[geoIndx]).Z = pdmLightElement.Elev.HasValue ? pdmLightElement.ConvertValueToMeter(pdmLightElement.Elev.Value, pdmLightElement.Elev_UOM.ToString()) : 0;


                    var mAware = aimGeo[geoIndx] as IMAware;
                    mAware.MAware = true;

                    pdmLightElement.Geo = aimGeo[geoIndx];

                    pdmObj.Elements.Add(pdmLightElement);

                    geoIndx++;

                }

            }


            return pdmObj;
        }
    }

    public class AIM_RwyVisualRange_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.RunwayVisualRange aimObj = (Aran.Aim.Features.RunwayVisualRange)aimFeature;

            RunwayVisualRange pdmObj = new RunwayVisualRange
            {
                ID = aimObj.Identifier.ToString(),

                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,

            };

            pdmObj.ManyLinks = aimObj.AssociatedRunwayDirection?.Count > 1;

            if (aimObj.ReadingPosition != null && aimObj.ReadingPosition.HasValue)
            {
                CodeRVRReadingType rvr;
                Enum.TryParse<CodeRVRReadingType>(aimObj.ReadingPosition.ToString(), out rvr);
                pdmObj.ReadingPosition = rvr;

            }
            

            if (aimGeo != null && aimGeo.Count > 0)
            {
                var zAware = aimGeo[0] as IZAware;
                zAware.ZAware = true;

                var mAware = aimGeo[0] as IMAware;
                mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
            }


            return pdmObj;
        }

    }

    public class AIM_AirportHotSpot_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.AirportHotSpot aimObj = (Aran.Aim.Features.AirportHotSpot)aimFeature;

            AirportHotSpot pdmObj = new AirportHotSpot
            {
                ID = aimObj.Identifier.ToString(),
                Instruction = aimObj.Instruction,
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
                Designator = aimObj.Designator

            };

           
            if (aimGeo != null && aimGeo.Count > 0)
            {
                var zAware = aimGeo[0] as IZAware;
                zAware.ZAware = true;

                var mAware = aimGeo[0] as IMAware;
                mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
            }


            return pdmObj;
        }

    }

    public class AIM_RadioFrequencyArea_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.RadioFrequencyArea aimObj = (Aran.Aim.Features.RadioFrequencyArea)aimFeature;
            
            RadioFrequencyArea pdmObj = new RadioFrequencyArea
            {
                ID = aimObj.Identifier.ToString(),
                AngleScallop = aimObj.AngleScallop,
                RadioCommChannel_ID = aimObj.Equipment?.Frequency?.Identifier.ToString()

            };

            CodeRadioFrequencyAreaType type;
            if (aimObj.Type != null)
            {
                Enum.TryParse<PDM.CodeRadioFrequencyAreaType>(aimObj.Type.ToString(), out type);
                pdmObj.Type = type;
            }

            CodeRadioSignalType signalType;
            if (aimObj.SignalType != null)
            {
                Enum.TryParse<CodeRadioSignalType>(aimObj.SignalType.ToString(), out signalType);
                pdmObj.SignalType = signalType;
            }


            if (aimGeo != null && aimGeo.Count > 0)
            {
                var zAware = aimGeo[0] as IZAware;
                zAware.ZAware = true;

                var mAware = aimGeo[0] as IMAware;
                mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
            }


            return pdmObj;
        }
    }

    public class AIM_TaxiHoldingPosition_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.TaxiHoldingPosition aimObj = (Aran.Aim.Features.TaxiHoldingPosition)aimFeature;

            TaxiHoldingPosition pdmObj = new TaxiHoldingPosition
            {
                ID = aimObj.Identifier.ToString(),                
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
                
            };

            if (aimObj.AssociatedGuidanceLine != null)
            {
                pdmObj.ID_GuidanceLine = aimObj.AssociatedGuidanceLine.Identifier.ToString();
            }
            
            if (aimObj.LandingCategory != null && aimObj.LandingCategory.HasValue)
            {
                CodeHoldingCategoryType rvr;
                Enum.TryParse<CodeHoldingCategoryType>(aimObj.LandingCategory.ToString(), out rvr);
                pdmObj.LandingCategory = rvr;

            }

            if (aimObj.Status != null && aimObj.Status.HasValue)
            {
                CodeStatusOperationsType rvr;
                Enum.TryParse<CodeStatusOperationsType>(aimObj.Status.ToString(), out rvr);
                pdmObj.Status = rvr;

            }

            if (aimGeo != null && aimGeo.Count > 0)
            {
                var zAware = aimGeo[0] as IZAware;
                zAware.ZAware = true;

                var mAware = aimGeo[0] as IMAware;
                mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
            }


            return pdmObj;
        }

    }

    public class AIM_TaxiHolding_LIGHT_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            //AranSupport.Utilitys util = new AranSupport.Utilitys();

            Aran.Aim.Features.TaxiHoldingPositionLightSystem aimObj = (Aran.Aim.Features.TaxiHoldingPositionLightSystem)aimFeature;

            TaxiHoldingPositionLightSystem pdmObj = new TaxiHoldingPositionLightSystem
            {
                ID = aimObj.Identifier.ToString(),
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
                EmergencyLighting = aimObj.EmergencyLighting != null && aimObj.EmergencyLighting.HasValue ? aimObj.EmergencyLighting.Value : false,

            };

            CodeLightHoldingPositionType taxSecType;
            if (aimObj.Type != null && (aimObj.Type.HasValue) && (Enum.TryParse<CodeLightHoldingPositionType>(aimObj.Type.ToString(), out taxSecType))) pdmObj.Type = taxSecType;

            CodeLightIntensity uom_light;
            if (aimObj.IntensityLevel != null && (aimObj.IntensityLevel.HasValue) && (Enum.TryParse<CodeLightIntensity>(aimObj.IntensityLevel.ToString(), out uom_light))) pdmObj.IntensityLevel = uom_light;

            ColourType uom_clr;
            if (aimObj.Colour != null && (aimObj.Colour.HasValue) && (Enum.TryParse<ColourType>(aimObj.Colour.ToString(), out uom_clr))) pdmObj.Colour = uom_clr;


            if ((aimObj.Element != null) && (aimObj.Element.Count > 0))
            {
                pdmObj.Elements = new List<LightElement>();
                int geoIndx = 0;
                foreach (var el in aimObj.Element)
                {
                    Aran.Aim.Features.LightElement aimLightElement = (Aran.Aim.Features.LightElement)el;
                    LightElement pdmLightElement = new LightElement
                    {
                        ID = Guid.NewGuid().ToString(),
                        Intensity = aimLightElement.Intensity != null ? aimLightElement.Intensity.Value : 0,
                        Lat = ((IPoint)aimGeo[geoIndx]).Y.ToString(),
                        Lon = ((IPoint)aimGeo[geoIndx]).X.ToString(),
                        Elev = aimLightElement.Location.Elevation != null ? aimLightElement.Location.Elevation.Value : 0,
                        LightedElement = "TaxiHoldingLightSystem",

                    };

                    if (aimLightElement.IntensityLevel != null && (aimLightElement.IntensityLevel.HasValue) && (Enum.TryParse<CodeLightIntensity>(aimLightElement.IntensityLevel.ToString(), out uom_light))) pdmLightElement.IntensityLevel = uom_light;
                    if (aimLightElement.Colour != null && (aimLightElement.Colour.HasValue) && (Enum.TryParse<ColourType>(aimLightElement.Colour.ToString(), out uom_clr))) pdmLightElement.Colour = uom_clr;

                    CodeLightSource _uom;
                    if (aimLightElement.Type != null && (aimLightElement.Type.HasValue) && (Enum.TryParse<CodeLightSource>(aimLightElement.Type.ToString(), out _uom))) pdmLightElement.LightSourceType = _uom;

                    UOM_DIST_VERT uom_dist;
                    if ((aimLightElement.Location.Elevation != null) && (Enum.TryParse<UOM_DIST_VERT>(aimLightElement.Location.Elevation.Uom.ToString(), out uom_dist))) pdmLightElement.Elev_UOM = uom_dist;


                    var zAware = aimGeo[geoIndx] as IZAware;
                    zAware.ZAware = true;
                    ((IPoint)aimGeo[geoIndx]).Z = pdmLightElement.Elev.HasValue ? pdmLightElement.ConvertValueToMeter(pdmLightElement.Elev.Value, pdmLightElement.Elev_UOM.ToString()) : 0;


                    var mAware = aimGeo[geoIndx] as IMAware;
                    mAware.MAware = true;

                    pdmLightElement.Geo = aimGeo[geoIndx];

                    pdmObj.Elements.Add(pdmLightElement);

                    geoIndx++;

                }

            }


            return pdmObj;
        }
    }

    public class AIM_TaxiHoldingMarking_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.TaxiHoldingPositionMarking aimObj = (Aran.Aim.Features.TaxiHoldingPositionMarking)aimFeature;

            TaxiHoldingPositionMarking pdmObj = new TaxiHoldingPositionMarking
            {
                ID = aimObj.Identifier.ToString(),
                //ID_TaxiwayElem = aimObj.MarkedElement.Identifier.ToString(),

                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
            };

            if (aimObj.Condition != null && aimObj.Condition.HasValue)
            {
                CodeMarkingConditionType arptp;
                Enum.TryParse<CodeMarkingConditionType>(aimObj.Condition.ToString(), out arptp); pdmObj.Condition = arptp;

            }

            pdmObj.MarkingElementList = new List<MarkingElement>();

            for (int i = 0; i <= aimGeo.Count - 1; i++)
            {
                List<IGeometry> listGeom = new List<IGeometry>();
                listGeom.Add(aimGeo[i]);
                MarkingElement markElem = (MarkingElement)AIM_PDM_Converter.AIM_Object_Convert(aimObj.Element[i], listGeom);
                if (markElem != null)
                {
                    markElem.Marking_ID = pdmObj.ID;
                    markElem.MarkedElement = PDM_ENUM.TaxiHoldingPositionMarking.ToString();
                    pdmObj.MarkingElementList.Add(markElem);
                }
            }


            return pdmObj;
        }

    }

    public class AIM_VisualGlideSlope_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {

            Aran.Aim.Features.VisualGlideSlopeIndicator aimObj = (Aran.Aim.Features.VisualGlideSlopeIndicator)aimFeature;

            VisualGlideSlopeIndicator pdmObj = new VisualGlideSlopeIndicator
            {
                ID = aimObj.Identifier.ToString(),
                ActualDate = aimObj.TimeSlice?.ValidTime.BeginPosition ?? DateTime.Now,
                EmergencyLighting = aimObj.EmergencyLighting ?? false,
                NumberBox = aimObj.NumberBox ?? 0,
                Portable = aimObj.Portable ?? false,
                SlopeAngle = aimObj.SlopeAngle ?? Double.NaN,
                MinimumEyeHeightOverThreshold = aimObj.MinimumEyeHeightOverThreshold?.Value ?? Double.NaN,

            };
            
            CodeLightIntensity intensLevel;
            if (aimObj.IntensityLevel != null && (Enum.TryParse<CodeLightIntensity>(aimObj.IntensityLevel.ToString(), out intensLevel))) pdmObj.IntensityLevel = intensLevel;

            ColourType color;
            if (aimObj.Colour != null && (Enum.TryParse<ColourType>(aimObj.Colour.ToString(), out color))) pdmObj.Colour = color;

            CodeSide position;
            if (aimObj.Position != null && (Enum.TryParse<CodeSide>(aimObj.Position.ToString(), out position))) pdmObj.Position = position;


            CodeVASISType type;
            if (aimObj.Type != null && (Enum.TryParse<CodeVASISType>(aimObj.Type.ToString(), out type))) pdmObj.Type = type;

            UOM_DIST_VERT uom;
            if (aimObj.MinimumEyeHeightOverThreshold != null  && (Enum.TryParse<UOM_DIST_VERT>(aimObj.MinimumEyeHeightOverThreshold.Uom.ToString(), out uom))) pdmObj.MinimumEyeHeightOverThreshold_UOM = uom;




            if ((aimObj.Element != null) && (aimObj.Element.Count > 0))
            {
                pdmObj.Elements = new List<LightElement>();
                int geoIndx = 0;
                foreach (var el in aimObj.Element)
                {
                    Aran.Aim.Features.LightElement aimLightElement = (Aran.Aim.Features.LightElement)el;
                    LightElement pdmLightElement = new LightElement
                    {
                        ID = Guid.NewGuid().ToString(),
                        Intensity = aimLightElement.Intensity != null ? aimLightElement.Intensity.Value : 0,
                        Lat = ((IPoint)aimGeo[geoIndx]).Y.ToString(),
                        Lon = ((IPoint)aimGeo[geoIndx]).X.ToString(),
                        Elev = aimLightElement.Location.Elevation != null ? aimLightElement.Location.Elevation.Value : 0,
                        LightedElement = "VisualGlideSlopeIndicator",

                    };

                    if (aimLightElement.IntensityLevel != null && (Enum.TryParse<CodeLightIntensity>(aimLightElement.IntensityLevel.ToString(), out intensLevel))) pdmLightElement.IntensityLevel = intensLevel;
                    if (aimLightElement.Colour != null && (Enum.TryParse<ColourType>(aimLightElement.Colour.ToString(), out color))) pdmLightElement.Colour = color;

                    CodeLightSource _uom;
                    if (aimLightElement.Type != null && (Enum.TryParse<CodeLightSource>(aimLightElement.Type.ToString(), out _uom))) pdmLightElement.LightSourceType = _uom;

                    UOM_DIST_VERT uom_dist;
                    if ((aimLightElement.Location.Elevation != null) && (Enum.TryParse<UOM_DIST_VERT>(aimLightElement.Location.Elevation.Uom.ToString(), out uom_dist))) pdmLightElement.Elev_UOM = uom_dist;


                    var zAware = aimGeo[geoIndx] as IZAware;
                    zAware.ZAware = true;
                    ((IPoint)aimGeo[geoIndx]).Z = pdmLightElement.Elev.HasValue ? pdmLightElement.ConvertValueToMeter(pdmLightElement.Elev.Value, pdmLightElement.Elev_UOM.ToString()) : 0;


                    var mAware = aimGeo[geoIndx] as IMAware;
                    mAware.MAware = true;

                    pdmLightElement.Geo = aimGeo[geoIndx];

                    pdmObj.Elements.Add(pdmLightElement);

                    geoIndx++;

                }

            }


            return pdmObj;
        }
    }

    public class AIM_TouchDownLiftOff_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.TouchDownLiftOff aimObj = (Aran.Aim.Features.TouchDownLiftOff)aimFeature;

            TouchDownLiftOff pdmObj = new TouchDownLiftOff
            {
                ID = aimObj.Identifier.ToString(), 
                ID_Runway = aimObj.ApproachTakeOffArea?.Identifier.ToString(),
                Designator = aimObj.Designator,
                Length = aimObj.Length != null ? aimObj.Length.Value : Double.NaN,
                Width = aimObj.Width != null ? aimObj.Width.Value : Double.NaN,
                Slope = aimObj.Slope != null ? aimObj.Slope.Value : Double.NaN,
                HelicopterClass=aimObj.HelicopterClass.ToString(),
                Abandoned = aimObj.Abandoned,
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
            };

            if (aimObj.SurfaceProperties != null)
            {
                SurfaceCharacteristics pdmsSurfaceCharacteristics = (SurfaceCharacteristics)AIM_PDM_Converter.AIM_Object_Convert(aimObj.SurfaceProperties, null);
                pdmsSurfaceCharacteristics.ID_Parent = aimObj.Identifier.ToString();
                pdmsSurfaceCharacteristics.ParentName = pdmObj.GetType().Name;
                pdmObj.SurfaceProperties = pdmsSurfaceCharacteristics;
            }

            UOM_DIST_HORZ uom_dist;
            if ((aimObj.Width?.Uom != null) && (Enum.TryParse<UOM_DIST_HORZ>(aimObj.Width.Uom.ToString(), out uom_dist))) pdmObj.UOM_Width = uom_dist;

            if ((aimObj.Length?.Uom != null) && (Enum.TryParse<UOM_DIST_HORZ>(aimObj.Length.Uom.ToString(), out uom_dist))) pdmObj.UOM_Length = uom_dist;

            if (aimObj.AimingPoint?.Geo != null)
            {

                var pnt = ConvertToEsriGeom.FromGeometry(aimObj.AimingPoint.Geo, true, GeometryFormatter.Wgs1984Reference());

                var zAware = pnt as IZAware;
                zAware.ZAware = true;

                var mAware = pnt as IMAware;
                mAware.MAware = true;

                pdmObj.AimingPoint = pnt;
            }
            
            if (aimObj.Extent?.Geo!=null)
            {

                var extent = ConvertToEsriGeom.FromGeometry(aimObj.Extent.Geo, true, GeometryFormatter.Wgs1984Reference());

                var zAware = extent as IZAware;
                zAware.ZAware = true;
                (extent as IZ)?.SetConstantZ(0);

                var mAware = extent as IMAware;
                mAware.MAware = true;

                pdmObj.Geo = extent;
            }

            return pdmObj;
        }
    }

    public class AIM_TouchDownLiftOffMarking_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.TouchDownLiftOffMarking aimObj = (Aran.Aim.Features.TouchDownLiftOffMarking)aimFeature;

            TouchDownLiftOffMarking pdmObj = new TouchDownLiftOffMarking
            {
                ID = aimObj.Identifier.ToString(),
                //ID_TaxiwayElem = aimObj.MarkedElement.Identifier.ToString(),

                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
            };

            if (aimObj.Condition != null && aimObj.Condition.HasValue)
            {
                CodeMarkingConditionType arptp;
                Enum.TryParse<CodeMarkingConditionType>(aimObj.Condition.ToString(), out arptp); pdmObj.Condition = arptp;

            }

            if (aimObj.MarkingLocation != null && aimObj.MarkingLocation.HasValue)
            {
                CodeTLOFSectionType arptp;
                Enum.TryParse<CodeTLOFSectionType>(aimObj.MarkingLocation.ToString(), out arptp); pdmObj.MarkingLocation = arptp;

            }

            pdmObj.MarkingElementList = new List<MarkingElement>();

            for (int i = 0; i <= aimGeo.Count - 1; i++)
            {
                List<IGeometry> listGeom = new List<IGeometry>();
                listGeom.Add(aimGeo[i]);
                MarkingElement markElem = (MarkingElement)AIM_PDM_Converter.AIM_Object_Convert(aimObj.Element[i], listGeom);
                if (markElem != null)
                {
                    markElem.Marking_ID = pdmObj.ID;
                    markElem.MarkedElement = "TouchDownLiftOffMarking";
                    pdmObj.MarkingElementList.Add(markElem);
                }
            }


            return pdmObj;
        }

    }

    public class AIM_TouchDown_LIGHT_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {

            Aran.Aim.Features.TouchDownLiftOffLightSystem aimObj = (Aran.Aim.Features.TouchDownLiftOffLightSystem)aimFeature;

            TouchDownLiftOffLightSystem pdmObj = new TouchDownLiftOffLightSystem
            {
                ID = aimObj.Identifier.ToString(),
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,
                EmergencyLighting = aimObj.EmergencyLighting != null && aimObj.EmergencyLighting.HasValue ? aimObj.EmergencyLighting.Value : false,

            };

            CodeTLOFSectionType taxSecType;
            if (aimObj.Position != null && (aimObj.Position.HasValue) && (Enum.TryParse<CodeTLOFSectionType>(aimObj.Position.ToString(), out taxSecType))) pdmObj.Position = taxSecType;

            CodeLightIntensity uom_light;
            if (aimObj.IntensityLevel != null && (aimObj.IntensityLevel.HasValue) && (Enum.TryParse<CodeLightIntensity>(aimObj.IntensityLevel.ToString(), out uom_light))) pdmObj.IntensityLevel = uom_light;

            ColourType uom_clr;
            if (aimObj.Colour != null && (aimObj.Colour.HasValue) && (Enum.TryParse<ColourType>(aimObj.Colour.ToString(), out uom_clr))) pdmObj.Colour = uom_clr;


            if ((aimObj.Element != null) && (aimObj.Element.Count > 0))
            {
                pdmObj.Elements = new List<LightElement>();
                int geoIndx = 0;
                foreach (var el in aimObj.Element)
                {
                    Aran.Aim.Features.LightElement aimLightElement = (Aran.Aim.Features.LightElement)el;
                    LightElement pdmLightElement = new LightElement
                    {
                        ID = Guid.NewGuid().ToString(),
                        Intensity = aimLightElement.Intensity != null ? aimLightElement.Intensity.Value : 0,
                        Lat = ((IPoint)aimGeo[geoIndx]).Y.ToString(),
                        Lon = ((IPoint)aimGeo[geoIndx]).X.ToString(),
                        Elev = aimLightElement.Location.Elevation != null ? aimLightElement.Location.Elevation.Value : 0,
                        LightedElement = "TouchDownLiftOffLightSystem",

                    };

                    if (aimLightElement.IntensityLevel != null && (aimLightElement.IntensityLevel.HasValue) && (Enum.TryParse<CodeLightIntensity>(aimLightElement.IntensityLevel.ToString(), out uom_light))) pdmLightElement.IntensityLevel = uom_light;
                    if (aimLightElement.Colour != null && (aimLightElement.Colour.HasValue) && (Enum.TryParse<ColourType>(aimLightElement.Colour.ToString(), out uom_clr))) pdmLightElement.Colour = uom_clr;

                    CodeLightSource _uom;
                    if (aimLightElement.Type != null && (aimLightElement.Type.HasValue) && (Enum.TryParse<CodeLightSource>(aimLightElement.Type.ToString(), out _uom))) pdmLightElement.LightSourceType = _uom;

                    UOM_DIST_VERT uom_dist;
                    if ((aimLightElement.Location.Elevation != null) && (Enum.TryParse<UOM_DIST_VERT>(aimLightElement.Location.Elevation.Uom.ToString(), out uom_dist))) pdmLightElement.Elev_UOM = uom_dist;


                    var zAware = aimGeo[geoIndx] as IZAware;
                    zAware.ZAware = true;
                    ((IPoint)aimGeo[geoIndx]).Z = pdmLightElement.Elev.HasValue ? pdmLightElement.ConvertValueToMeter(pdmLightElement.Elev.Value, pdmLightElement.Elev_UOM.ToString()) : 0;


                    var mAware = aimGeo[geoIndx] as IMAware;
                    mAware.MAware = true;

                    pdmLightElement.Geo = aimGeo[geoIndx];

                    pdmObj.Elements.Add(pdmLightElement);

                    geoIndx++;

                }

            }


            return pdmObj;
        }
    }

    public class AIM_TouchDownSafeArea_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.TouchDownLiftOffSafeArea aimObj = (Aran.Aim.Features.TouchDownLiftOffSafeArea)aimFeature;

            TouchDownLiftOffSafeArea pdmObj = new TouchDownLiftOffSafeArea
            {
                ID = aimObj.Identifier.ToString(),

                Width = aimObj.Width?.Value ?? Double.NaN,
                Length = aimObj.Length?.Value ?? Double.NaN,
                Lighting = aimObj.Lighting != null && aimObj.Lighting.Value,
                ObstacleFree = aimObj.ObstacleFree != null && aimObj.ObstacleFree.Value,
                ActualDate = aimObj.TimeSlice?.ValidTime.BeginPosition ?? DateTime.Now,

            };

            if (aimObj.Width != null)
            {
                UOM_DIST_HORZ status;
                Enum.TryParse<UOM_DIST_HORZ>(aimObj.Width.Uom.ToString(), out status);
                pdmObj.Width_UOM = status;

            }

            if (aimObj.Length != null)
            {
                UOM_DIST_HORZ status;
                Enum.TryParse<UOM_DIST_HORZ>(aimObj.Length.Uom.ToString(), out status);
                pdmObj.Length_UOM = status;

            }

            if (aimObj.SurfaceProperties != null)
            {
                SurfaceCharacteristics pdmsSurfaceCharacteristics = (SurfaceCharacteristics)AIM_PDM_Converter.AIM_Object_Convert(aimObj.SurfaceProperties, null);
                pdmsSurfaceCharacteristics.ID_Parent = aimObj.Identifier.ToString();
                pdmsSurfaceCharacteristics.ParentName = pdmObj.GetType().Name;
                pdmObj.SurfaceProperties = pdmsSurfaceCharacteristics;
            }

            if (aimGeo != null && aimGeo.Count > 0)
            {
                var zAware = aimGeo[0] as IZAware;
                if (zAware != null) zAware.ZAware = true;

                var mAware = aimGeo[0] as IMAware;
                if (mAware != null) mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
            }


            return pdmObj;
        }

    }

    public class AIM_WorkArea_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.WorkArea aimObj = (Aran.Aim.Features.WorkArea)aimFeature;

            WorkArea pdmObj = new WorkArea
            {
                ID = aimObj.Identifier.ToString(),
                
                PlannedOperational = aimObj.PlannedOperational.ToString(),
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,

            };

            CodeWorkAreaType taxSecType;
            if (aimObj.Type != null && (aimObj.Type.HasValue) && (Enum.TryParse<CodeWorkAreaType>(aimObj.Type.ToString(), out taxSecType))) pdmObj.Type = taxSecType;

            if (aimGeo != null && aimGeo.Count > 0)
            {
                var zAware = aimGeo[0] as IZAware;
                zAware.ZAware = true;

                var mAware = aimGeo[0] as IMAware;
                mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
            }


            return pdmObj;
        }

    }


    public class AIM_Road_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.Road aimObj = (Aran.Aim.Features.Road)aimFeature;

            Road pdmObj = new Road
            {
                ID = aimObj.Identifier.ToString(),
                Designator = aimObj.Designator,
                
                ActualDate = aimObj.TimeSlice?.ValidTime.BeginPosition ?? DateTime.Now,

            };

            if (aimObj.Status != null)
            {
                CodeStatusOperationsType status;
                Enum.TryParse<CodeStatusOperationsType>(aimObj.Status.ToString(), out status);
                pdmObj.Status = status;

            }

            if (aimObj.Type != null)
            {
                CodeRoadType type;
                Enum.TryParse<CodeRoadType>(aimObj.Type.ToString(), out type);
                pdmObj.Type = type;

            }

            if (aimObj.Abandoned != null)
            {
                CodeYesNoType aban;
                Enum.TryParse<CodeYesNoType>(aimObj.Abandoned.ToString(), out aban);
                pdmObj.Abandoned = aban;

            }

            if (aimObj.AccessibleStand != null && aimObj.AccessibleStand.Count>0)
            {
                pdmObj.AccessibleStand=new List<AircraftStand>();
                foreach (var stand in aimObj.AccessibleStand)
                {
                    AircraftStand pdmstand = (AircraftStand)AIM_PDM_Converter.AIM_Object_Convert(stand, null);
                                        
                    pdmObj.AccessibleStand.Add(pdmstand);
                }
                
            }


            if (aimObj.SurfaceProperties != null)
            {
                SurfaceCharacteristics pdmsSurfaceCharacteristics = (SurfaceCharacteristics)AIM_PDM_Converter.AIM_Object_Convert(aimObj.SurfaceProperties, null);
                pdmsSurfaceCharacteristics.ID_Parent = aimObj.Identifier.ToString();
                pdmsSurfaceCharacteristics.ParentName = pdmObj.GetType().Name;
                pdmObj.SurfaceProperties = pdmsSurfaceCharacteristics;
            }

            if (aimGeo != null && aimGeo.Count > 0)
            {
                var zAware = aimGeo[0] as IZAware;
                if (zAware != null) zAware.ZAware = true;

                var mAware = aimGeo[0] as IMAware;
                if (mAware != null) mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
            }


            return pdmObj;
        }

    }

    public class AIM_Unit_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.Unit aimObj = (Aran.Aim.Features.Unit)aimFeature;

            Unit pdmObj = new Unit
            {
                ID = aimObj.Identifier.ToString(),
                Designator = aimObj.Designator,
                Name = aimObj.Name,

                ActualDate = aimObj.TimeSlice?.ValidTime.BeginPosition ?? DateTime.Now,

            };

            if (aimObj.CompliantICAO != null)
            {
                CodeYesNoType status;
                Enum.TryParse<CodeYesNoType>(aimObj.CompliantICAO.ToString(), out status);
                pdmObj.CompliantICAO = status;

            }

            if (aimObj.Type != null)
            {
                CodeUnitType type;
                Enum.TryParse<CodeUnitType>(aimObj.Type.ToString(), out type);
                pdmObj.Type = type;

            }

            if (aimObj.Military != null)
            {
                CodeMilitaryOperationsType aban;
                Enum.TryParse<CodeMilitaryOperationsType>(aimObj.Military.ToString(), out aban);
                pdmObj.Military = aban;

            }

            

            if (aimGeo != null && aimGeo.Count > 0)
            {
                var zAware = aimGeo[0] as IZAware;
                if (zAware != null) zAware.ZAware = true;

                var mAware = aimGeo[0] as IMAware;
                if (mAware != null) mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
            }


            return pdmObj;
        }

    }

    public class AIM_NonMovementArea_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {
            Aran.Aim.Features.NonMovementArea aimObj = (Aran.Aim.Features.NonMovementArea)aimFeature;

            NonMovementArea pdmObj = new NonMovementArea
            {
                ID = aimObj.Identifier.ToString(),
                
                ActualDate = aimObj.TimeSlice?.ValidTime.BeginPosition ?? DateTime.Now,

            };
           
            if (aimGeo != null && aimGeo.Count > 0)
            {
                var zAware = aimGeo[0] as IZAware;
                if (zAware != null) zAware.ZAware = true;

                var mAware = aimGeo[0] as IMAware;
                if (mAware != null) mAware.MAware = true;

                pdmObj.Geo = aimGeo[0];
            }


            return pdmObj;
        }

    }

    public class AIM_GEOBORDER_PDM_Converter : IAIM_PDM_Converter
    {
        public PDMObject Convert_AIM_Object(object aimFeature, List<IGeometry> aimGeo)
        {

            Aran.Aim.Features.GeoBorder aimObj = (Aran.Aim.Features.GeoBorder)aimFeature;


            GeoBorder pdmObj = new GeoBorder
            {
                ID = aimObj.Identifier.ToString(),
                GeoBorderName = aimObj.Name != null ? GetBorderName(aimObj.Name) : "NONE",
                NeighborName = aimObj.Name != null ? GetNeighborName(aimObj.Name) : "NONE",
                ActualDate = aimObj.TimeSlice != null ? aimObj.TimeSlice.ValidTime.BeginPosition : DateTime.Now,

            };

            if (aimObj.Type != null)
            {
                CodeGeoBorder uom_borderType;
                if (Enum.TryParse<CodeGeoBorder>(aimObj.Type.ToString(), out uom_borderType)) pdmObj.CodeGeoBorderType = uom_borderType;
            }

            var zAware = aimGeo[0] as IZAware;
            zAware.ZAware = true;

            var mAware = aimGeo[0] as IMAware;
            mAware.MAware = true;

            pdmObj.Geo = aimGeo[0];
            return pdmObj;
        }

        private string GetNeighborName(string BorderName)
        {
            string[] word = BorderName.Split('-');
            if (word.Length > 1) return word[1];
            else return "";
        }

        private string GetBorderName(string BorderName)
        {
            string[] word = BorderName.Split('-');
            if (word.Length > 0) return word[0];
            else return "";
        }

       

    }



}
