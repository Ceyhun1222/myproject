using System;
using System.Collections;
using System.Collections.Generic;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Package;
using Aran.Aim.Enums;
using Aran.Aim.Metadata;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim
{
	public static partial class AimObjectFactory
	{
		public static ADataType CreateADataType (DataType aDataType)
		{
			switch (aDataType)
			{
				case DataType.ValDistance:
					return new ValDistance ();
				case DataType.ValDistanceVertical:
					return new ValDistanceVertical ();
				case DataType.ValDuration:
					return new ValDuration ();
				case DataType.ValFL:
					return new ValFL ();
				case DataType.ValFrequency:
					return new ValFrequency ();
				case DataType.ValPressure:
					return new ValPressure ();
				case DataType.ValSpeed:
					return new ValSpeed ();
				case DataType.ValTemperature:
					return new ValTemperature ();
				case DataType.ValWeight:
					return new ValWeight ();
				case DataType.ValDistanceSigned:
					return new ValDistanceSigned ();
				case DataType.TextNote:
					return new TextNote ();
				case DataType.ValDepth:
					return new ValDepth ();
				case DataType.ValLightIntensity:
					return new ValLightIntensity ();
				case DataType.TimePeriod:
					return new TimePeriod ();
				case DataType.ResponsibleParty:
					return new ResponsibleParty ();
				case DataType.CIResponsibleParty:
					return new CIResponsibleParty ();
				case DataType.CIAddress:
					return new CIAddress ();
				case DataType.CIContact:
					return new CIContact ();
				case DataType.Telephone:
					return new Telephone ();
				case DataType.Contact:
					return new Contact ();
				case DataType.Citiation:
					return new Citiation ();
				case DataType.CIDate:
					return new CIDate ();
				case DataType.CICitation:
					return new CICitation ();
				case DataType.DataQualityElement:
					return new DataQualityElement ();
				case DataType.TimeSlice:
					return new TimeSlice ();
				case DataType.FeatureRef:
					return new FeatureRef ();
				case DataType.AbstractGroundLightSystemRef:
					return new AbstractGroundLightSystemRef ();
				case DataType.AbstractMarkingRef:
					return new AbstractMarkingRef ();
				case DataType.AbstractAirportHeliportProtectionAreaRef:
					return new AbstractAirportHeliportProtectionAreaRef ();
				case DataType.AbstractRadarEquipmentRef:
					return new AbstractRadarEquipmentRef ();
				case DataType.AbstractSurveillanceRadarRef:
					return new AbstractSurveillanceRadarRef ();
				case DataType.AbstractServiceRef:
					return new AbstractServiceRef ();
				case DataType.AbstractTrafficSeparationServiceRef:
					return new AbstractTrafficSeparationServiceRef ();
				case DataType.AbstractAirportGroundServiceRef:
					return new AbstractAirportGroundServiceRef ();
				case DataType.AbstractNavaidEquipmentRef:
					return new AbstractNavaidEquipmentRef ();
				case DataType.AbstractNavigationSystemCheckpointRef:
					return new AbstractNavigationSystemCheckpointRef ();
				case DataType.AbstractProcedureRef:
					return new AbstractProcedureRef ();
				case DataType.AbstractSegmentLegRef:
					return new AbstractSegmentLegRef ();
				case DataType.AbstractApproachLegRef:
					return new AbstractApproachLegRef ();
				default:
					throw new Exception ("Create DataType is not supported for type: " + aDataType);
			}
		}

		public static IList CreateADataTypeList (DataType aDataType)
		{
			switch (aDataType)
			{
				case DataType.ValDistance:
					return new List<ValDistance> ();
				case DataType.ValDistanceVertical:
					return new List<ValDistanceVertical> ();
				case DataType.ValDuration:
					return new List<ValDuration> ();
				case DataType.ValFL:
					return new List<ValFL> ();
				case DataType.ValFrequency:
					return new List<ValFrequency> ();
				case DataType.ValPressure:
					return new List<ValPressure> ();
				case DataType.ValSpeed:
					return new List<ValSpeed> ();
				case DataType.ValTemperature:
					return new List<ValTemperature> ();
				case DataType.ValWeight:
					return new List<ValWeight> ();
				case DataType.ValDistanceSigned:
					return new List<ValDistanceSigned> ();
				case DataType.TextNote:
					return new List<TextNote> ();
				case DataType.ValDepth:
					return new List<ValDepth> ();
				case DataType.ValLightIntensity:
					return new List<ValLightIntensity> ();
				case DataType.TimePeriod:
					return new List<TimePeriod> ();
				case DataType.ResponsibleParty:
					return new List<ResponsibleParty> ();
				case DataType.CIResponsibleParty:
					return new List<CIResponsibleParty> ();
				case DataType.CIAddress:
					return new List<CIAddress> ();
				case DataType.CIContact:
					return new List<CIContact> ();
				case DataType.Telephone:
					return new List<Telephone> ();
				case DataType.Contact:
					return new List<Contact> ();
				case DataType.Citiation:
					return new List<Citiation> ();
				case DataType.CIDate:
					return new List<CIDate> ();
				case DataType.CICitation:
					return new List<CICitation> ();
				case DataType.DataQualityElement:
					return new List<DataQualityElement> ();
				case DataType.TimeSlice:
					return new List<TimeSlice> ();
				case DataType.FeatureRef:
					return new List<FeatureRef> ();
				case DataType.AbstractGroundLightSystemRef:
					return new List<AbstractGroundLightSystemRef> ();
				case DataType.AbstractMarkingRef:
					return new List<AbstractMarkingRef> ();
				case DataType.AbstractAirportHeliportProtectionAreaRef:
					return new List<AbstractAirportHeliportProtectionAreaRef> ();
				case DataType.AbstractRadarEquipmentRef:
					return new List<AbstractRadarEquipmentRef> ();
				case DataType.AbstractSurveillanceRadarRef:
					return new List<AbstractSurveillanceRadarRef> ();
				case DataType.AbstractServiceRef:
					return new List<AbstractServiceRef> ();
				case DataType.AbstractTrafficSeparationServiceRef:
					return new List<AbstractTrafficSeparationServiceRef> ();
				case DataType.AbstractAirportGroundServiceRef:
					return new List<AbstractAirportGroundServiceRef> ();
				case DataType.AbstractNavaidEquipmentRef:
					return new List<AbstractNavaidEquipmentRef> ();
				case DataType.AbstractNavigationSystemCheckpointRef:
					return new List<AbstractNavigationSystemCheckpointRef> ();
				case DataType.AbstractProcedureRef:
					return new List<AbstractProcedureRef> ();
				case DataType.AbstractSegmentLegRef:
					return new List<AbstractSegmentLegRef> ();
				case DataType.AbstractApproachLegRef:
					return new List<AbstractApproachLegRef> ();
				default:
					throw new Exception ("Create DataType List is not supported for type: " + aDataType);
			}
		}

		public static AObject CreateAObject (ObjectType aObjectType)
		{
			switch (aObjectType)
			{
				case ObjectType.ArrestingGearExtent:
					return new ArrestingGearExtent ();
				case ObjectType.NavaidEquipmentDistance:
					return new NavaidEquipmentDistance ();
				case ObjectType.RunwayDeclaredDistance:
					return new RunwayDeclaredDistance ();
				case ObjectType.RunwayDeclaredDistanceValue:
					return new RunwayDeclaredDistanceValue ();
				case ObjectType.ManoeuvringAreaAvailability:
					return new ManoeuvringAreaAvailability ();
				case ObjectType.ManoeuvringAreaUsage:
					return new ManoeuvringAreaUsage ();
				case ObjectType.ApronAreaAvailability:
					return new ApronAreaAvailability ();
				case ObjectType.ApronAreaUsage:
					return new ApronAreaUsage ();
				case ObjectType.LightActivation:
					return new LightActivation ();
				case ObjectType.GroundLightingAvailability:
					return new GroundLightingAvailability ();
				case ObjectType.MarkingElement:
					return new MarkingElement ();
				case ObjectType.MarkingExtent:
					return new MarkingExtent ();
				case ObjectType.Ridge:
					return new Ridge ();
				case ObjectType.RunwayContamination:
					return new RunwayContamination ();
				case ObjectType.TaxiwayContamination:
					return new TaxiwayContamination ();
				case ObjectType.SurfaceContaminationLayer:
					return new SurfaceContaminationLayer ();
				case ObjectType.RunwaySectionContamination:
					return new RunwaySectionContamination ();
				case ObjectType.TouchDownLiftOffContamination:
					return new TouchDownLiftOffContamination ();
				case ObjectType.ApronContamination:
					return new ApronContamination ();
				case ObjectType.AircraftStandContamination:
					return new AircraftStandContamination ();
				case ObjectType.AirportHeliportContamination:
					return new AirportHeliportContamination ();
				case ObjectType.SurfaceCharacteristics:
					return new SurfaceCharacteristics ();
				case ObjectType.City:
					return new City ();
				case ObjectType.AirportHeliportResponsibilityOrganisation:
					return new AirportHeliportResponsibilityOrganisation ();
				case ObjectType.AltimeterSourceStatus:
					return new AltimeterSourceStatus ();
				case ObjectType.WorkareaActivity:
					return new WorkareaActivity ();
				case ObjectType.ConditionCombination:
					return new ConditionCombination ();
				case ObjectType.AirportHeliportAvailability:
					return new AirportHeliportAvailability ();
				case ObjectType.AirportHeliportUsage:
					return new AirportHeliportUsage ();
				case ObjectType.AirspaceGeometryComponent:
					return new AirspaceGeometryComponent ();
				case ObjectType.AirspaceVolume:
					return new AirspaceVolume ();
				case ObjectType.AirspaceActivation:
					return new AirspaceActivation ();
				case ObjectType.AirspaceLayerClass:
					return new AirspaceLayerClass ();
				case ObjectType.AirspaceVolumeDependency:
					return new AirspaceVolumeDependency ();
				case ObjectType.Curve:
					return new Curve ();
				case ObjectType.ElevatedCurve:
					return new ElevatedCurve ();
				case ObjectType.ElevatedPoint:
					return new ElevatedPoint ();
				case ObjectType.ElevatedSurface:
					return new ElevatedSurface ();
				case ObjectType.AixmPoint:
					return new AixmPoint ();
				case ObjectType.Surface:
					return new Surface ();
				case ObjectType.RadarComponent:
					return new RadarComponent ();
				case ObjectType.Reflector:
					return new Reflector ();
				case ObjectType.SurveillanceGroundStation:
					return new SurveillanceGroundStation ();
				case ObjectType.ObstacleAssessmentArea:
					return new ObstacleAssessmentArea ();
				case ObjectType.Obstruction:
					return new Obstruction ();
				case ObjectType.AltitudeAdjustment:
					return new AltitudeAdjustment ();
				case ObjectType.ObstaclePlacement:
					return new ObstaclePlacement ();
				case ObjectType.StandardLevel:
					return new StandardLevel ();
				case ObjectType.EquipmentChoice:
					return new EquipmentChoice ();
				case ObjectType.ContactInformation:
					return new ContactInformation ();
				case ObjectType.OnlineContact:
					return new OnlineContact ();
				case ObjectType.PostalAddress:
					return new PostalAddress ();
				case ObjectType.TelephoneContact:
					return new TelephoneContact ();
				case ObjectType.AircraftCharacteristic:
					return new AircraftCharacteristic ();
				case ObjectType.FlightCharacteristic:
					return new FlightCharacteristic ();
				case ObjectType.LightElement:
					return new LightElement ();
				case ObjectType.LightElementStatus:
					return new LightElementStatus ();
				case ObjectType.AirspaceLayer:
					return new AirspaceLayer ();
				case ObjectType.CircleSector:
					return new CircleSector ();
				case ObjectType.Timesheet:
					return new Timesheet ();
				case ObjectType.Meteorology:
					return new Meteorology ();
				case ObjectType.CallsignDetail:
					return new CallsignDetail ();
				case ObjectType.Fuel:
					return new Fuel ();
				case ObjectType.Nitrogen:
					return new Nitrogen ();
				case ObjectType.Oil:
					return new Oil ();
				case ObjectType.Oxygen:
					return new Oxygen ();
				case ObjectType.ServiceOperationalStatus:
					return new ServiceOperationalStatus ();
				case ObjectType.RadioCommunicationOperationalStatus:
					return new RadioCommunicationOperationalStatus ();
				case ObjectType.PointReference:
					return new PointReference ();
				case ObjectType.TerminalSegmentPoint:
					return new TerminalSegmentPoint ();
				case ObjectType.EnRouteSegmentPoint:
					return new EnRouteSegmentPoint ();
				case ObjectType.AngleUse:
					return new AngleUse ();
				case ObjectType.AuthorityForNavaidEquipment:
					return new AuthorityForNavaidEquipment ();
				case ObjectType.AuthorityForSpecialNavigationStation:
					return new AuthorityForSpecialNavigationStation ();
				case ObjectType.AuthorityForSpecialNavigationSystem:
					return new AuthorityForSpecialNavigationSystem ();
				case ObjectType.NavaidComponent:
					return new NavaidComponent ();
				case ObjectType.NavaidOperationalStatus:
					return new NavaidOperationalStatus ();
				case ObjectType.NavaidEquipmentMonitoring:
					return new NavaidEquipmentMonitoring ();
				case ObjectType.SpecialNavigationStationStatus:
					return new SpecialNavigationStationStatus ();
				case ObjectType.SignificantPoint:
					return new SignificantPoint ();
				case ObjectType.GuidanceService:
					return new GuidanceService ();
				case ObjectType.Note:
					return new Note ();
				case ObjectType.LinguisticNote:
					return new LinguisticNote ();
				case ObjectType.OrganisationAuthorityAssociation:
					return new OrganisationAuthorityAssociation ();
				case ObjectType.UnitDependency:
					return new UnitDependency ();
				case ObjectType.UnitAvailability:
					return new UnitAvailability ();
				case ObjectType.VerticalStructurePartGeometry:
					return new VerticalStructurePartGeometry ();
				case ObjectType.VerticalStructurePart:
					return new VerticalStructurePart ();
				case ObjectType.ObstacleAreaOrigin:
					return new ObstacleAreaOrigin ();
				case ObjectType.VerticalStructureLightingStatus:
					return new VerticalStructureLightingStatus ();
				case ObjectType.CirclingRestriction:
					return new CirclingRestriction ();
				case ObjectType.Minima:
					return new Minima ();
				case ObjectType.EquipmentUnavailableAdjustment:
					return new EquipmentUnavailableAdjustment ();
				case ObjectType.EquipmentUnavailableAdjustmentColumn:
					return new EquipmentUnavailableAdjustmentColumn ();
				case ObjectType.TerminalArrivalAreaSector:
					return new TerminalArrivalAreaSector ();
				case ObjectType.FASDataBlock:
					return new FASDataBlock ();
				case ObjectType.ApproachAltitudeTable:
					return new ApproachAltitudeTable ();
				case ObjectType.ApproachDistanceTable:
					return new ApproachDistanceTable ();
				case ObjectType.ApproachTimingTable:
					return new ApproachTimingTable ();
				case ObjectType.FinalProfile:
					return new FinalProfile ();
				case ObjectType.MissedApproachGroup:
					return new MissedApproachGroup ();
				case ObjectType.ApproachCondition:
					return new ApproachCondition ();
				case ObjectType.NavigationAreaSector:
					return new NavigationAreaSector ();
				case ObjectType.DepartureArrivalCondition:
					return new DepartureArrivalCondition ();
				case ObjectType.SectorDesign:
					return new SectorDesign ();
				case ObjectType.ProcedureAvailability:
					return new ProcedureAvailability ();
				case ObjectType.ProcedureTransition:
					return new ProcedureTransition ();
				case ObjectType.HoldingUse:
					return new HoldingUse ();
				case ObjectType.ProcedureTransitionLeg:
					return new ProcedureTransitionLeg ();
				case ObjectType.LandingTakeoffAreaCollection:
					return new LandingTakeoffAreaCollection ();
				case ObjectType.SafeAltitudeAreaSector:
					return new SafeAltitudeAreaSector ();
				case ObjectType.HoldingPatternLength:
					return new HoldingPatternLength ();
				case ObjectType.HoldingPatternDuration:
					return new HoldingPatternDuration ();
				case ObjectType.HoldingPatternDistance:
					return new HoldingPatternDistance ();
				case ObjectType.DirectFlightSegment:
					return new DirectFlightSegment ();
				case ObjectType.FlightConditionCombination:
					return new FlightConditionCombination ();
				case ObjectType.FlightConditionCircumstance:
					return new FlightConditionCircumstance ();
				case ObjectType.FlightRestrictionLevel:
					return new FlightRestrictionLevel ();
				case ObjectType.FlightRestrictionRoute:
					return new FlightRestrictionRoute ();
				case ObjectType.FlightRoutingElement:
					return new FlightRoutingElement ();
				case ObjectType.FlightConditionElement:
					return new FlightConditionElement ();
				case ObjectType.FlightConditionElementChoice:
					return new FlightConditionElementChoice ();
				case ObjectType.FlightRoutingElementChoice:
					return new FlightRoutingElementChoice ();
				case ObjectType.RouteAvailability:
					return new RouteAvailability ();
				case ObjectType.RoutePortion:
					return new RoutePortion ();
				case ObjectType.AerialRefuellingPoint:
					return new AerialRefuellingPoint ();
				case ObjectType.AerialRefuellingAnchor:
					return new AerialRefuellingAnchor ();
				case ObjectType.AerialRefuellingTrack:
					return new AerialRefuellingTrack ();
				case ObjectType.AuthorityForAerialRefuelling:
					return new AuthorityForAerialRefuelling ();
				case ObjectType.MDMetadata:
					return new MDMetadata ();
				case ObjectType.MessageMetadata:
					return new MessageMetadata ();
				case ObjectType.MDConstraints:
					return new MDConstraints ();
				case ObjectType.IdentificationMessage:
					return new IdentificationMessage ();
				case ObjectType.MDLegalConstraints:
					return new MDLegalConstraints ();
				case ObjectType.LegalConstraints:
					return new LegalConstraints ();
				case ObjectType.MDSecurityConstraints:
					return new MDSecurityConstraints ();
				case ObjectType.SecurityConstraints:
					return new SecurityConstraints ();
				case ObjectType.FeatureMetadata:
					return new FeatureMetadata ();
				case ObjectType.IdentificationFeature:
					return new IdentificationFeature ();
				case ObjectType.FeatureTimeSliceMetadata:
					return new FeatureTimeSliceMetadata ();
				case ObjectType.DataQuality:
					return new DataQuality ();
				case ObjectType.IdentificationTimesliceFeature:
					return new IdentificationTimesliceFeature ();
				case ObjectType.FeatureRefObject:
					return new FeatureRefObject ();
				case ObjectType.AbstractGroundLightSystemRefObject:
					return new AbstractGroundLightSystemRefObject ();
				case ObjectType.AbstractMarkingRefObject:
					return new AbstractMarkingRefObject ();
				case ObjectType.AbstractAirportHeliportProtectionAreaRefObject:
					return new AbstractAirportHeliportProtectionAreaRefObject ();
				case ObjectType.AbstractRadarEquipmentRefObject:
					return new AbstractRadarEquipmentRefObject ();
				case ObjectType.AbstractSurveillanceRadarRefObject:
					return new AbstractSurveillanceRadarRefObject ();
				case ObjectType.AbstractServiceRefObject:
					return new AbstractServiceRefObject ();
				case ObjectType.AbstractTrafficSeparationServiceRefObject:
					return new AbstractTrafficSeparationServiceRefObject ();
				case ObjectType.AbstractAirportGroundServiceRefObject:
					return new AbstractAirportGroundServiceRefObject ();
				case ObjectType.AbstractNavaidEquipmentRefObject:
					return new AbstractNavaidEquipmentRefObject ();
				case ObjectType.AbstractNavigationSystemCheckpointRefObject:
					return new AbstractNavigationSystemCheckpointRefObject ();
				case ObjectType.AbstractProcedureRefObject:
					return new AbstractProcedureRefObject ();
				case ObjectType.AbstractSegmentLegRefObject:
					return new AbstractSegmentLegRefObject ();
				case ObjectType.AbstractApproachLegRefObject:
					return new AbstractApproachLegRefObject ();
				case ObjectType.PointExtension:
					return new PointExtension ();
				case ObjectType.SurfaceExtension:
					return new SurfaceExtension ();
				case ObjectType.CurveExtension:
					return new CurveExtension ();

                #region ISO METADATA


                case ObjectType.BtString:
                    return new BtString();
                case ObjectType.BtDateTime:
                    return new BtDateTime();
                case ObjectType.BtTimePosition:
                    return new BtTimePosition();
                case ObjectType.BtTimePeriod:
                    return new BtTimePeriod();
                case ObjectType.CiTelephone:
                    return new CiTelephone();
                case ObjectType.CiAddress:
                    return new CiAddress();
                case ObjectType.CiOnlineResource:
                    return new CiOnlineResource();
                case ObjectType.CiContact:
                    return new CiContact();
                case ObjectType.CiResponsibleParty:
                    return new CiResponsibleParty();
                case ObjectType.MdIdentifier:
                    return new MdIdentifier();
                case ObjectType.CiPresentationFormCodeObject:
                    return new CiPresentationFormCodeObject();
                case ObjectType.CiSeries:
                    return new CiSeries();
                case ObjectType.CiCitation:
                    return new CiCitation();
                case ObjectType.MdConstraintsObject:
                    return new MdConstraintsObject();
                case ObjectType.MdRestrictionCodeObject:
                    return new MdRestrictionCodeObject();
                case ObjectType.MdLegalConstraints:
                    return new MdLegalConstraints();
                case ObjectType.MdSecurityConstraints:
                    return new MdSecurityConstraints();
                case ObjectType.MdProgressCodeObject:
                    return new MdProgressCodeObject();
                case ObjectType.MdAbstractIdentificationObject:
                    return new MdAbstractIdentificationObject();
                case ObjectType.ExTemporalExtent:
                    return new ExTemporalExtent();
                case ObjectType.ExVerticalExtent:
                    return new ExVerticalExtent();
                case ObjectType.ExExtent:
                    return new ExExtent();
                case ObjectType.MdDataIdentification:
                    return new MdDataIdentification();
			    case ObjectType.DqAbstractResultObject:
			        return new DqAbstractResultObject();
			    case ObjectType.DqAbstractElementObject:
			        return new DqAbstractElementObject();
                case ObjectType.DqQuantitativeResult:
                    return new DqQuantitativeResult();
                case ObjectType.DqQuantitativeAttributeAccuracy:
                    return new DqQuantitativeAttributeAccuracy();
                case ObjectType.RsIdentifier:
                    return new RsIdentifier();
                case ObjectType.LiSource:
                    return new LiSource();
                case ObjectType.LiProcessStep:
                    return new LiProcessStep();
                case ObjectType.LiLineage:
                    return new LiLineage();
                case ObjectType.DqDataQuality:
                    return new DqDataQuality();
                case ObjectType.MdMetadata:
                    return new MdMetadata();

                #endregion

                default:
					throw new Exception ("Create ObjectType is not supported for type: " + aObjectType);
			}
		}

		public static IList CreateAObjectList (ObjectType aObjectType)
		{
			switch (aObjectType)
			{
				case ObjectType.ArrestingGearExtent:
					return new AObjectList<ArrestingGearExtent> ();
				case ObjectType.NavaidEquipmentDistance:
					return new AObjectList<NavaidEquipmentDistance> ();
				case ObjectType.RunwayDeclaredDistance:
					return new AObjectList<RunwayDeclaredDistance> ();
				case ObjectType.RunwayDeclaredDistanceValue:
					return new AObjectList<RunwayDeclaredDistanceValue> ();
				case ObjectType.ManoeuvringAreaAvailability:
					return new AObjectList<ManoeuvringAreaAvailability> ();
				case ObjectType.ManoeuvringAreaUsage:
					return new AObjectList<ManoeuvringAreaUsage> ();
				case ObjectType.ApronAreaAvailability:
					return new AObjectList<ApronAreaAvailability> ();
				case ObjectType.ApronAreaUsage:
					return new AObjectList<ApronAreaUsage> ();
				case ObjectType.LightActivation:
					return new AObjectList<LightActivation> ();
				case ObjectType.GroundLightingAvailability:
					return new AObjectList<GroundLightingAvailability> ();
				case ObjectType.MarkingElement:
					return new AObjectList<MarkingElement> ();
				case ObjectType.MarkingExtent:
					return new AObjectList<MarkingExtent> ();
				case ObjectType.Ridge:
					return new AObjectList<Ridge> ();
				case ObjectType.RunwayContamination:
					return new AObjectList<RunwayContamination> ();
				case ObjectType.TaxiwayContamination:
					return new AObjectList<TaxiwayContamination> ();
				case ObjectType.SurfaceContaminationLayer:
					return new AObjectList<SurfaceContaminationLayer> ();
				case ObjectType.RunwaySectionContamination:
					return new AObjectList<RunwaySectionContamination> ();
				case ObjectType.TouchDownLiftOffContamination:
					return new AObjectList<TouchDownLiftOffContamination> ();
				case ObjectType.ApronContamination:
					return new AObjectList<ApronContamination> ();
				case ObjectType.AircraftStandContamination:
					return new AObjectList<AircraftStandContamination> ();
				case ObjectType.AirportHeliportContamination:
					return new AObjectList<AirportHeliportContamination> ();
				case ObjectType.SurfaceCharacteristics:
					return new AObjectList<SurfaceCharacteristics> ();
				case ObjectType.City:
					return new AObjectList<City> ();
				case ObjectType.AirportHeliportResponsibilityOrganisation:
					return new AObjectList<AirportHeliportResponsibilityOrganisation> ();
				case ObjectType.AltimeterSourceStatus:
					return new AObjectList<AltimeterSourceStatus> ();
				case ObjectType.WorkareaActivity:
					return new AObjectList<WorkareaActivity> ();
				case ObjectType.ConditionCombination:
					return new AObjectList<ConditionCombination> ();
				case ObjectType.AirportHeliportAvailability:
					return new AObjectList<AirportHeliportAvailability> ();
				case ObjectType.AirportHeliportUsage:
					return new AObjectList<AirportHeliportUsage> ();
				case ObjectType.AirspaceGeometryComponent:
					return new AObjectList<AirspaceGeometryComponent> ();
				case ObjectType.AirspaceVolume:
					return new AObjectList<AirspaceVolume> ();
				case ObjectType.AirspaceActivation:
					return new AObjectList<AirspaceActivation> ();
				case ObjectType.AirspaceLayerClass:
					return new AObjectList<AirspaceLayerClass> ();
				case ObjectType.AirspaceVolumeDependency:
					return new AObjectList<AirspaceVolumeDependency> ();
				case ObjectType.Curve:
					return new AObjectList<Curve> ();
				case ObjectType.ElevatedCurve:
					return new AObjectList<ElevatedCurve> ();
				case ObjectType.ElevatedPoint:
					return new AObjectList<ElevatedPoint> ();
				case ObjectType.ElevatedSurface:
					return new AObjectList<ElevatedSurface> ();
				case ObjectType.AixmPoint:
					return new AObjectList<AixmPoint> ();
				case ObjectType.Surface:
					return new AObjectList<Surface> ();
				case ObjectType.RadarComponent:
					return new AObjectList<RadarComponent> ();
				case ObjectType.Reflector:
					return new AObjectList<Reflector> ();
				case ObjectType.SurveillanceGroundStation:
					return new AObjectList<SurveillanceGroundStation> ();
				case ObjectType.ObstacleAssessmentArea:
					return new AObjectList<ObstacleAssessmentArea> ();
				case ObjectType.Obstruction:
					return new AObjectList<Obstruction> ();
				case ObjectType.AltitudeAdjustment:
					return new AObjectList<AltitudeAdjustment> ();
				case ObjectType.ObstaclePlacement:
					return new AObjectList<ObstaclePlacement> ();
				case ObjectType.StandardLevel:
					return new AObjectList<StandardLevel> ();
				case ObjectType.EquipmentChoice:
					return new AObjectList<EquipmentChoice> ();
				case ObjectType.ContactInformation:
					return new AObjectList<ContactInformation> ();
				case ObjectType.OnlineContact:
					return new AObjectList<OnlineContact> ();
				case ObjectType.PostalAddress:
					return new AObjectList<PostalAddress> ();
				case ObjectType.TelephoneContact:
					return new AObjectList<TelephoneContact> ();
				case ObjectType.AircraftCharacteristic:
					return new AObjectList<AircraftCharacteristic> ();
				case ObjectType.FlightCharacteristic:
					return new AObjectList<FlightCharacteristic> ();
				case ObjectType.LightElement:
					return new AObjectList<LightElement> ();
				case ObjectType.LightElementStatus:
					return new AObjectList<LightElementStatus> ();
				case ObjectType.AirspaceLayer:
					return new AObjectList<AirspaceLayer> ();
				case ObjectType.CircleSector:
					return new AObjectList<CircleSector> ();
				case ObjectType.Timesheet:
					return new AObjectList<Timesheet> ();
				case ObjectType.Meteorology:
					return new AObjectList<Meteorology> ();
				case ObjectType.CallsignDetail:
					return new AObjectList<CallsignDetail> ();
				case ObjectType.Fuel:
					return new AObjectList<Fuel> ();
				case ObjectType.Nitrogen:
					return new AObjectList<Nitrogen> ();
				case ObjectType.Oil:
					return new AObjectList<Oil> ();
				case ObjectType.Oxygen:
					return new AObjectList<Oxygen> ();
				case ObjectType.ServiceOperationalStatus:
					return new AObjectList<ServiceOperationalStatus> ();
				case ObjectType.RadioCommunicationOperationalStatus:
					return new AObjectList<RadioCommunicationOperationalStatus> ();
				case ObjectType.PointReference:
					return new AObjectList<PointReference> ();
				case ObjectType.TerminalSegmentPoint:
					return new AObjectList<TerminalSegmentPoint> ();
				case ObjectType.EnRouteSegmentPoint:
					return new AObjectList<EnRouteSegmentPoint> ();
				case ObjectType.AngleUse:
					return new AObjectList<AngleUse> ();
				case ObjectType.AuthorityForNavaidEquipment:
					return new AObjectList<AuthorityForNavaidEquipment> ();
				case ObjectType.AuthorityForSpecialNavigationStation:
					return new AObjectList<AuthorityForSpecialNavigationStation> ();
				case ObjectType.AuthorityForSpecialNavigationSystem:
					return new AObjectList<AuthorityForSpecialNavigationSystem> ();
				case ObjectType.NavaidComponent:
					return new AObjectList<NavaidComponent> ();
				case ObjectType.NavaidOperationalStatus:
					return new AObjectList<NavaidOperationalStatus> ();
				case ObjectType.NavaidEquipmentMonitoring:
					return new AObjectList<NavaidEquipmentMonitoring> ();
				case ObjectType.SpecialNavigationStationStatus:
					return new AObjectList<SpecialNavigationStationStatus> ();
				case ObjectType.SignificantPoint:
					return new AObjectList<SignificantPoint> ();
				case ObjectType.GuidanceService:
					return new AObjectList<GuidanceService> ();
				case ObjectType.Note:
					return new AObjectList<Note> ();
				case ObjectType.LinguisticNote:
					return new AObjectList<LinguisticNote> ();
				case ObjectType.OrganisationAuthorityAssociation:
					return new AObjectList<OrganisationAuthorityAssociation> ();
				case ObjectType.UnitDependency:
					return new AObjectList<UnitDependency> ();
				case ObjectType.UnitAvailability:
					return new AObjectList<UnitAvailability> ();
				case ObjectType.VerticalStructurePartGeometry:
					return new AObjectList<VerticalStructurePartGeometry> ();
				case ObjectType.VerticalStructurePart:
					return new AObjectList<VerticalStructurePart> ();
				case ObjectType.ObstacleAreaOrigin:
					return new AObjectList<ObstacleAreaOrigin> ();
				case ObjectType.VerticalStructureLightingStatus:
					return new AObjectList<VerticalStructureLightingStatus> ();
				case ObjectType.CirclingRestriction:
					return new AObjectList<CirclingRestriction> ();
				case ObjectType.Minima:
					return new AObjectList<Minima> ();
				case ObjectType.EquipmentUnavailableAdjustment:
					return new AObjectList<EquipmentUnavailableAdjustment> ();
				case ObjectType.EquipmentUnavailableAdjustmentColumn:
					return new AObjectList<EquipmentUnavailableAdjustmentColumn> ();
				case ObjectType.TerminalArrivalAreaSector:
					return new AObjectList<TerminalArrivalAreaSector> ();
				case ObjectType.FASDataBlock:
					return new AObjectList<FASDataBlock> ();
				case ObjectType.ApproachAltitudeTable:
					return new AObjectList<ApproachAltitudeTable> ();
				case ObjectType.ApproachDistanceTable:
					return new AObjectList<ApproachDistanceTable> ();
				case ObjectType.ApproachTimingTable:
					return new AObjectList<ApproachTimingTable> ();
				case ObjectType.FinalProfile:
					return new AObjectList<FinalProfile> ();
				case ObjectType.MissedApproachGroup:
					return new AObjectList<MissedApproachGroup> ();
				case ObjectType.ApproachCondition:
					return new AObjectList<ApproachCondition> ();
				case ObjectType.NavigationAreaSector:
					return new AObjectList<NavigationAreaSector> ();
				case ObjectType.DepartureArrivalCondition:
					return new AObjectList<DepartureArrivalCondition> ();
				case ObjectType.SectorDesign:
					return new AObjectList<SectorDesign> ();
				case ObjectType.ProcedureAvailability:
					return new AObjectList<ProcedureAvailability> ();
				case ObjectType.ProcedureTransition:
					return new AObjectList<ProcedureTransition> ();
				case ObjectType.HoldingUse:
					return new AObjectList<HoldingUse> ();
				case ObjectType.ProcedureTransitionLeg:
					return new AObjectList<ProcedureTransitionLeg> ();
				case ObjectType.LandingTakeoffAreaCollection:
					return new AObjectList<LandingTakeoffAreaCollection> ();
				case ObjectType.SafeAltitudeAreaSector:
					return new AObjectList<SafeAltitudeAreaSector> ();
				case ObjectType.HoldingPatternLength:
					return new AObjectList<HoldingPatternLength> ();
				case ObjectType.HoldingPatternDuration:
					return new AObjectList<HoldingPatternDuration> ();
				case ObjectType.HoldingPatternDistance:
					return new AObjectList<HoldingPatternDistance> ();
				case ObjectType.DirectFlightSegment:
					return new AObjectList<DirectFlightSegment> ();
				case ObjectType.FlightConditionCombination:
					return new AObjectList<FlightConditionCombination> ();
				case ObjectType.FlightConditionCircumstance:
					return new AObjectList<FlightConditionCircumstance> ();
				case ObjectType.FlightRestrictionLevel:
					return new AObjectList<FlightRestrictionLevel> ();
				case ObjectType.FlightRestrictionRoute:
					return new AObjectList<FlightRestrictionRoute> ();
				case ObjectType.FlightRoutingElement:
					return new AObjectList<FlightRoutingElement> ();
				case ObjectType.FlightConditionElement:
					return new AObjectList<FlightConditionElement> ();
				case ObjectType.FlightConditionElementChoice:
					return new AObjectList<FlightConditionElementChoice> ();
				case ObjectType.FlightRoutingElementChoice:
					return new AObjectList<FlightRoutingElementChoice> ();
				case ObjectType.RouteAvailability:
					return new AObjectList<RouteAvailability> ();
				case ObjectType.RoutePortion:
					return new AObjectList<RoutePortion> ();
				case ObjectType.AerialRefuellingPoint:
					return new AObjectList<AerialRefuellingPoint> ();
				case ObjectType.AerialRefuellingAnchor:
					return new AObjectList<AerialRefuellingAnchor> ();
				case ObjectType.AerialRefuellingTrack:
					return new AObjectList<AerialRefuellingTrack> ();
				case ObjectType.AuthorityForAerialRefuelling:
					return new AObjectList<AuthorityForAerialRefuelling> ();
				case ObjectType.MDMetadata:
					return new AObjectList<MDMetadata> ();
				case ObjectType.MessageMetadata:
					return new AObjectList<MessageMetadata> ();
				case ObjectType.MDConstraints:
					return new AObjectList<MDConstraints> ();
				case ObjectType.IdentificationMessage:
					return new AObjectList<IdentificationMessage> ();
				case ObjectType.MDLegalConstraints:
					return new AObjectList<MDLegalConstraints> ();
				case ObjectType.LegalConstraints:
					return new AObjectList<LegalConstraints> ();
				case ObjectType.MDSecurityConstraints:
					return new AObjectList<MDSecurityConstraints> ();
				case ObjectType.SecurityConstraints:
					return new AObjectList<SecurityConstraints> ();
				case ObjectType.FeatureMetadata:
					return new AObjectList<FeatureMetadata> ();
				case ObjectType.IdentificationFeature:
					return new AObjectList<IdentificationFeature> ();
				case ObjectType.FeatureTimeSliceMetadata:
					return new AObjectList<FeatureTimeSliceMetadata> ();
				case ObjectType.DataQuality:
					return new AObjectList<DataQuality> ();
				case ObjectType.IdentificationTimesliceFeature:
					return new AObjectList<IdentificationTimesliceFeature> ();
				case ObjectType.FeatureRefObject:
					return new AObjectList<FeatureRefObject> ();
				case ObjectType.AbstractGroundLightSystemRefObject:
					return new AObjectList<AbstractGroundLightSystemRefObject> ();
				case ObjectType.AbstractMarkingRefObject:
					return new AObjectList<AbstractMarkingRefObject> ();
				case ObjectType.AbstractAirportHeliportProtectionAreaRefObject:
					return new AObjectList<AbstractAirportHeliportProtectionAreaRefObject> ();
				case ObjectType.AbstractRadarEquipmentRefObject:
					return new AObjectList<AbstractRadarEquipmentRefObject> ();
				case ObjectType.AbstractSurveillanceRadarRefObject:
					return new AObjectList<AbstractSurveillanceRadarRefObject> ();
				case ObjectType.AbstractServiceRefObject:
					return new AObjectList<AbstractServiceRefObject> ();
				case ObjectType.AbstractTrafficSeparationServiceRefObject:
					return new AObjectList<AbstractTrafficSeparationServiceRefObject> ();
				case ObjectType.AbstractAirportGroundServiceRefObject:
					return new AObjectList<AbstractAirportGroundServiceRefObject> ();
				case ObjectType.AbstractNavaidEquipmentRefObject:
					return new AObjectList<AbstractNavaidEquipmentRefObject> ();
				case ObjectType.AbstractNavigationSystemCheckpointRefObject:
					return new AObjectList<AbstractNavigationSystemCheckpointRefObject> ();
				case ObjectType.AbstractProcedureRefObject:
					return new AObjectList<AbstractProcedureRefObject> ();
				case ObjectType.AbstractSegmentLegRefObject:
					return new AObjectList<AbstractSegmentLegRefObject> ();
				case ObjectType.AbstractApproachLegRefObject:
					return new AObjectList<AbstractApproachLegRefObject> ();

                #region ISO METADATA

                case ObjectType.BtString:
                    return new AObjectList<BtString>();
                case ObjectType.BtDateTime:
                    return new AObjectList<BtDateTime>();
                case ObjectType.BtTimePosition:
                    return new AObjectList<BtTimePosition>();
                case ObjectType.BtTimePeriod:
                    return new AObjectList<BtTimePeriod>();
                case ObjectType.CiTelephone:
                    return new AObjectList<CiTelephone>();
                case ObjectType.CiAddress:
                    return new AObjectList<CiAddress>();
                case ObjectType.CiOnlineResource:
                    return new AObjectList<CiOnlineResource>();
                case ObjectType.CiContact:
                    return new AObjectList<CiContact>();
                case ObjectType.CiResponsibleParty:
                    return new AObjectList<CiResponsibleParty>();
                case ObjectType.MdIdentifier:
                    return new AObjectList<MdIdentifier>();
                case ObjectType.CiPresentationFormCodeObject:
                    return new AObjectList<CiPresentationFormCodeObject>();
                case ObjectType.CiSeries:
                    return new AObjectList<CiSeries>();
                case ObjectType.CiCitation:
                    return new AObjectList<CiCitation>();
                case ObjectType.MdConstraintsObject:
                    return new AObjectList<MdConstraintsObject>();
                case ObjectType.MdRestrictionCodeObject:
                    return new AObjectList<MdRestrictionCodeObject>();
                case ObjectType.MdLegalConstraints:
                    return new AObjectList<MdLegalConstraints>();
                case ObjectType.MdSecurityConstraints:
                    return new AObjectList<MdSecurityConstraints>();
                case ObjectType.MdProgressCodeObject:
                    return new AObjectList<MdProgressCodeObject>();
                case ObjectType.MdAbstractIdentificationObject:
                    return new AObjectList<MdAbstractIdentificationObject>();
                case ObjectType.ExTemporalExtent:
                    return new AObjectList<ExTemporalExtent>();
                case ObjectType.ExVerticalExtent:
                    return new AObjectList<ExVerticalExtent>();
                case ObjectType.ExExtent:
                    return new AObjectList<ExExtent>();
                case ObjectType.MdDataIdentification:
                    return new AObjectList<MdDataIdentification>();
                case ObjectType.DqAbstractResultObject:
                    return new AObjectList<DqAbstractResultObject>();
                case ObjectType.DqAbstractElementObject:
                    return new AObjectList<DqAbstractElementObject>();
                case ObjectType.DqQuantitativeResult:
                    return new AObjectList<DqQuantitativeResult>();
                case ObjectType.DqQuantitativeAttributeAccuracy:
                    return new AObjectList<DqQuantitativeAttributeAccuracy>();
                case ObjectType.RsIdentifier:
                    return new AObjectList<RsIdentifier>();
                case ObjectType.LiSource:
                    return new AObjectList<LiSource>();
                case ObjectType.LiProcessStep:
                    return new AObjectList<LiProcessStep>();
                case ObjectType.LiLineage:
                    return new AObjectList<LiLineage>();
                case ObjectType.DqDataQuality:
                    return new AObjectList<DqDataQuality>();
                case ObjectType.MdMetadata:
                    return new AObjectList<MdMetadata>();

                #endregion

                default:
					throw new Exception ("Create ObjectType List is not supported for type: " + aObjectType);
			}
		}

		public static Feature CreateFeature (FeatureType aFeatureType)
		{
			switch (aFeatureType)
			{
				case FeatureType.RunwayProtectArea:
					return new RunwayProtectArea ();
				case FeatureType.RunwayDirection:
					return new RunwayDirection ();
				case FeatureType.RunwayCentrelinePoint:
					return new RunwayCentrelinePoint ();
				case FeatureType.Runway:
					return new Runway ();
				case FeatureType.ArrestingGear:
					return new ArrestingGear ();
				case FeatureType.RunwayElement:
					return new RunwayElement ();
				case FeatureType.VisualGlideSlopeIndicator:
					return new VisualGlideSlopeIndicator ();
				case FeatureType.RunwayVisualRange:
					return new RunwayVisualRange ();
				case FeatureType.RunwayBlastPad:
					return new RunwayBlastPad ();
				case FeatureType.TaxiHoldingPosition:
					return new TaxiHoldingPosition ();
				case FeatureType.Taxiway:
					return new Taxiway ();
				case FeatureType.TaxiwayElement:
					return new TaxiwayElement ();
				case FeatureType.GuidanceLine:
					return new GuidanceLine ();
				case FeatureType.Apron:
					return new Apron ();
				case FeatureType.ApronElement:
					return new ApronElement ();
				case FeatureType.AircraftStand:
					return new AircraftStand ();
				case FeatureType.Road:
					return new Road ();
				case FeatureType.DeicingArea:
					return new DeicingArea ();
				case FeatureType.PassengerLoadingBridge:
					return new PassengerLoadingBridge ();
				case FeatureType.TouchDownLiftOffSafeArea:
					return new TouchDownLiftOffSafeArea ();
				case FeatureType.TouchDownLiftOff:
					return new TouchDownLiftOff ();
				case FeatureType.ApronLightSystem:
					return new ApronLightSystem ();
				case FeatureType.TaxiwayLightSystem:
					return new TaxiwayLightSystem ();
				case FeatureType.RunwayDirectionLightSystem:
					return new RunwayDirectionLightSystem ();
				case FeatureType.TouchDownLiftOffLightSystem:
					return new TouchDownLiftOffLightSystem ();
				case FeatureType.GuidanceLineLightSystem:
					return new GuidanceLineLightSystem ();
				case FeatureType.RunwayProtectAreaLightSystem:
					return new RunwayProtectAreaLightSystem ();
				case FeatureType.TaxiHoldingPositionLightSystem:
					return new TaxiHoldingPositionLightSystem ();
				case FeatureType.ApproachLightingSystem:
					return new ApproachLightingSystem ();
				case FeatureType.TaxiwayMarking:
					return new TaxiwayMarking ();
				case FeatureType.ApronMarking:
					return new ApronMarking ();
				case FeatureType.AirportProtectionAreaMarking:
					return new AirportProtectionAreaMarking ();
				case FeatureType.TouchDownLiftOffMarking:
					return new TouchDownLiftOffMarking ();
				case FeatureType.RunwayMarking:
					return new RunwayMarking ();
				case FeatureType.GuidanceLineMarking:
					return new GuidanceLineMarking ();
				case FeatureType.DeicingAreaMarking:
					return new DeicingAreaMarking ();
				case FeatureType.TaxiHoldingPositionMarking:
					return new TaxiHoldingPositionMarking ();
				case FeatureType.StandMarking:
					return new StandMarking ();
				case FeatureType.FloatingDockSite:
					return new FloatingDockSite ();
				case FeatureType.MarkingBuoy:
					return new MarkingBuoy ();
				case FeatureType.SeaplaneLandingArea:
					return new SeaplaneLandingArea ();
				case FeatureType.SeaplaneRampSite:
					return new SeaplaneRampSite ();
				case FeatureType.WorkArea:
					return new WorkArea ();
				case FeatureType.SurveyControlPoint:
					return new SurveyControlPoint ();
				case FeatureType.NonMovementArea:
					return new NonMovementArea ();
				case FeatureType.AirportHeliportCollocation:
					return new AirportHeliportCollocation ();
				case FeatureType.AirportHeliport:
					return new AirportHeliport ();
				case FeatureType.AltimeterSource:
					return new AltimeterSource ();
				case FeatureType.AirportHotSpot:
					return new AirportHotSpot ();
				case FeatureType.AuthorityForAirspace:
					return new AuthorityForAirspace ();
				case FeatureType.Airspace:
					return new Airspace ();
				case FeatureType.GeoBorder:
					return new GeoBorder ();
				case FeatureType.PrecisionApproachRadar:
					return new PrecisionApproachRadar ();
				case FeatureType.PrimarySurveillanceRadar:
					return new PrimarySurveillanceRadar ();
				case FeatureType.RadarSystem:
					return new RadarSystem ();
				case FeatureType.SecondarySurveillanceRadar:
					return new SecondarySurveillanceRadar ();
				case FeatureType.HoldingAssessment:
					return new HoldingAssessment ();
				case FeatureType.StandardLevelTable:
					return new StandardLevelTable ();
				case FeatureType.StandardLevelSector:
					return new StandardLevelSector ();
				case FeatureType.StandardLevelColumn:
					return new StandardLevelColumn ();
				case FeatureType.RadioFrequencyArea:
					return new RadioFrequencyArea ();
				case FeatureType.SpecialDate:
					return new SpecialDate ();
				case FeatureType.RadioCommunicationChannel:
					return new RadioCommunicationChannel ();
				case FeatureType.PilotControlledLighting:
					return new PilotControlledLighting ();
				case FeatureType.InformationService:
					return new InformationService ();
				case FeatureType.GroundTrafficControlService:
					return new GroundTrafficControlService ();
				case FeatureType.AirTrafficControlService:
					return new AirTrafficControlService ();
				case FeatureType.AirTrafficManagementService:
					return new AirTrafficManagementService ();
				case FeatureType.SearchRescueService:
					return new SearchRescueService ();
				case FeatureType.PassengerService:
					return new PassengerService ();
				case FeatureType.AircraftGroundService:
					return new AircraftGroundService ();
				case FeatureType.FireFightingService:
					return new FireFightingService ();
				case FeatureType.AirportClearanceService:
					return new AirportClearanceService ();
				case FeatureType.AirportSuppliesService:
					return new AirportSuppliesService ();
				case FeatureType.AngleIndication:
					return new AngleIndication ();
				case FeatureType.DistanceIndication:
					return new DistanceIndication ();
				case FeatureType.Azimuth:
					return new Azimuth ();
				case FeatureType.CheckpointINS:
					return new CheckpointINS ();
				case FeatureType.CheckpointVOR:
					return new CheckpointVOR ();
				case FeatureType.DME:
					return new DME ();
				case FeatureType.Elevation:
					return new Elevation ();
				case FeatureType.Glidepath:
					return new Glidepath ();
				case FeatureType.Localizer:
					return new Localizer ();
				case FeatureType.MarkerBeacon:
					return new MarkerBeacon ();
				case FeatureType.Navaid:
					return new Navaid ();
				case FeatureType.SDF:
					return new SDF ();
				case FeatureType.NDB:
					return new NDB ();
				case FeatureType.SpecialNavigationStation:
					return new SpecialNavigationStation ();
				case FeatureType.VOR:
					return new VOR ();
				case FeatureType.TACAN:
					return new TACAN ();
				case FeatureType.SpecialNavigationSystem:
					return new SpecialNavigationSystem ();
				case FeatureType.DirectionFinder:
					return new DirectionFinder ();
				case FeatureType.DesignatedPoint:
					return new DesignatedPoint ();
				case FeatureType.SignificantPointInAirspace:
					return new SignificantPointInAirspace ();
				case FeatureType.AeronauticalGroundLight:
					return new AeronauticalGroundLight ();
				case FeatureType.OrganisationAuthority:
					return new OrganisationAuthority ();
				case FeatureType.Unit:
					return new Unit ();
				case FeatureType.ObstacleArea:
					return new ObstacleArea ();
				case FeatureType.VerticalStructure:
					return new VerticalStructure ();
				case FeatureType.CirclingArea:
					return new CirclingArea ();
				case FeatureType.TerminalArrivalArea:
					return new TerminalArrivalArea ();
				case FeatureType.InstrumentApproachProcedure:
					return new InstrumentApproachProcedure ();
				case FeatureType.StandardInstrumentDeparture:
					return new StandardInstrumentDeparture ();
				case FeatureType.NavigationArea:
					return new NavigationArea ();
				case FeatureType.StandardInstrumentArrival:
					return new StandardInstrumentArrival ();
				case FeatureType.NavigationAreaRestriction:
					return new NavigationAreaRestriction ();
				case FeatureType.ArrivalFeederLeg:
					return new ArrivalFeederLeg ();
				case FeatureType.ArrivalLeg:
					return new ArrivalLeg ();
				case FeatureType.DepartureLeg:
					return new DepartureLeg ();
				case FeatureType.FinalLeg:
					return new FinalLeg ();
				case FeatureType.InitialLeg:
					return new InitialLeg ();
				case FeatureType.IntermediateLeg:
					return new IntermediateLeg ();
				case FeatureType.MissedApproachLeg:
					return new MissedApproachLeg ();
				case FeatureType.ProcedureDME:
					return new ProcedureDME ();
				case FeatureType.SafeAltitudeArea:
					return new SafeAltitudeArea ();
				case FeatureType.HoldingPattern:
					return new HoldingPattern ();
				case FeatureType.UnplannedHolding:
					return new UnplannedHolding ();
				case FeatureType.AirspaceBorderCrossing:
					return new AirspaceBorderCrossing ();
				case FeatureType.FlightRestriction:
					return new FlightRestriction ();
				case FeatureType.RouteSegment:
					return new RouteSegment ();
				case FeatureType.RouteDME:
					return new RouteDME ();
				case FeatureType.Route:
					return new Route ();
				case FeatureType.ChangeOverPoint:
					return new ChangeOverPoint ();
				case FeatureType.AerialRefuelling:
					return new AerialRefuelling ();
				case FeatureType.RulesProcedures:
					return new RulesProcedures ();
				default:
					throw new Exception ("Create FeatureType is not supported for type: " + aFeatureType);
			}
		}

		public static IList CreateFeatureList (FeatureType aFeatureType)
		{
			switch (aFeatureType)
			{
				case FeatureType.RunwayProtectArea:
					return new List<RunwayProtectArea> ();
				case FeatureType.RunwayDirection:
					return new List<RunwayDirection> ();
				case FeatureType.RunwayCentrelinePoint:
					return new List<RunwayCentrelinePoint> ();
				case FeatureType.Runway:
					return new List<Runway> ();
				case FeatureType.ArrestingGear:
					return new List<ArrestingGear> ();
				case FeatureType.RunwayElement:
					return new List<RunwayElement> ();
				case FeatureType.VisualGlideSlopeIndicator:
					return new List<VisualGlideSlopeIndicator> ();
				case FeatureType.RunwayVisualRange:
					return new List<RunwayVisualRange> ();
				case FeatureType.RunwayBlastPad:
					return new List<RunwayBlastPad> ();
				case FeatureType.TaxiHoldingPosition:
					return new List<TaxiHoldingPosition> ();
				case FeatureType.Taxiway:
					return new List<Taxiway> ();
				case FeatureType.TaxiwayElement:
					return new List<TaxiwayElement> ();
				case FeatureType.GuidanceLine:
					return new List<GuidanceLine> ();
				case FeatureType.Apron:
					return new List<Apron> ();
				case FeatureType.ApronElement:
					return new List<ApronElement> ();
				case FeatureType.AircraftStand:
					return new List<AircraftStand> ();
				case FeatureType.Road:
					return new List<Road> ();
				case FeatureType.DeicingArea:
					return new List<DeicingArea> ();
				case FeatureType.PassengerLoadingBridge:
					return new List<PassengerLoadingBridge> ();
				case FeatureType.TouchDownLiftOffSafeArea:
					return new List<TouchDownLiftOffSafeArea> ();
				case FeatureType.TouchDownLiftOff:
					return new List<TouchDownLiftOff> ();
				case FeatureType.ApronLightSystem:
					return new List<ApronLightSystem> ();
				case FeatureType.TaxiwayLightSystem:
					return new List<TaxiwayLightSystem> ();
				case FeatureType.RunwayDirectionLightSystem:
					return new List<RunwayDirectionLightSystem> ();
				case FeatureType.TouchDownLiftOffLightSystem:
					return new List<TouchDownLiftOffLightSystem> ();
				case FeatureType.GuidanceLineLightSystem:
					return new List<GuidanceLineLightSystem> ();
				case FeatureType.RunwayProtectAreaLightSystem:
					return new List<RunwayProtectAreaLightSystem> ();
				case FeatureType.TaxiHoldingPositionLightSystem:
					return new List<TaxiHoldingPositionLightSystem> ();
				case FeatureType.ApproachLightingSystem:
					return new List<ApproachLightingSystem> ();
				case FeatureType.TaxiwayMarking:
					return new List<TaxiwayMarking> ();
				case FeatureType.ApronMarking:
					return new List<ApronMarking> ();
				case FeatureType.AirportProtectionAreaMarking:
					return new List<AirportProtectionAreaMarking> ();
				case FeatureType.TouchDownLiftOffMarking:
					return new List<TouchDownLiftOffMarking> ();
				case FeatureType.RunwayMarking:
					return new List<RunwayMarking> ();
				case FeatureType.GuidanceLineMarking:
					return new List<GuidanceLineMarking> ();
				case FeatureType.DeicingAreaMarking:
					return new List<DeicingAreaMarking> ();
				case FeatureType.TaxiHoldingPositionMarking:
					return new List<TaxiHoldingPositionMarking> ();
				case FeatureType.StandMarking:
					return new List<StandMarking> ();
				case FeatureType.FloatingDockSite:
					return new List<FloatingDockSite> ();
				case FeatureType.MarkingBuoy:
					return new List<MarkingBuoy> ();
				case FeatureType.SeaplaneLandingArea:
					return new List<SeaplaneLandingArea> ();
				case FeatureType.SeaplaneRampSite:
					return new List<SeaplaneRampSite> ();
				case FeatureType.WorkArea:
					return new List<WorkArea> ();
				case FeatureType.SurveyControlPoint:
					return new List<SurveyControlPoint> ();
				case FeatureType.NonMovementArea:
					return new List<NonMovementArea> ();
				case FeatureType.AirportHeliportCollocation:
					return new List<AirportHeliportCollocation> ();
				case FeatureType.AirportHeliport:
					return new List<AirportHeliport> ();
				case FeatureType.AltimeterSource:
					return new List<AltimeterSource> ();
				case FeatureType.AirportHotSpot:
					return new List<AirportHotSpot> ();
				case FeatureType.AuthorityForAirspace:
					return new List<AuthorityForAirspace> ();
				case FeatureType.Airspace:
					return new List<Airspace> ();
				case FeatureType.GeoBorder:
					return new List<GeoBorder> ();
				case FeatureType.PrecisionApproachRadar:
					return new List<PrecisionApproachRadar> ();
				case FeatureType.PrimarySurveillanceRadar:
					return new List<PrimarySurveillanceRadar> ();
				case FeatureType.RadarSystem:
					return new List<RadarSystem> ();
				case FeatureType.SecondarySurveillanceRadar:
					return new List<SecondarySurveillanceRadar> ();
				case FeatureType.HoldingAssessment:
					return new List<HoldingAssessment> ();
				case FeatureType.StandardLevelTable:
					return new List<StandardLevelTable> ();
				case FeatureType.StandardLevelSector:
					return new List<StandardLevelSector> ();
				case FeatureType.StandardLevelColumn:
					return new List<StandardLevelColumn> ();
				case FeatureType.RadioFrequencyArea:
					return new List<RadioFrequencyArea> ();
				case FeatureType.SpecialDate:
					return new List<SpecialDate> ();
				case FeatureType.RadioCommunicationChannel:
					return new List<RadioCommunicationChannel> ();
				case FeatureType.PilotControlledLighting:
					return new List<PilotControlledLighting> ();
				case FeatureType.InformationService:
					return new List<InformationService> ();
				case FeatureType.GroundTrafficControlService:
					return new List<GroundTrafficControlService> ();
				case FeatureType.AirTrafficControlService:
					return new List<AirTrafficControlService> ();
				case FeatureType.AirTrafficManagementService:
					return new List<AirTrafficManagementService> ();
				case FeatureType.SearchRescueService:
					return new List<SearchRescueService> ();
				case FeatureType.PassengerService:
					return new List<PassengerService> ();
				case FeatureType.AircraftGroundService:
					return new List<AircraftGroundService> ();
				case FeatureType.FireFightingService:
					return new List<FireFightingService> ();
				case FeatureType.AirportClearanceService:
					return new List<AirportClearanceService> ();
				case FeatureType.AirportSuppliesService:
					return new List<AirportSuppliesService> ();
				case FeatureType.AngleIndication:
					return new List<AngleIndication> ();
				case FeatureType.DistanceIndication:
					return new List<DistanceIndication> ();
				case FeatureType.Azimuth:
					return new List<Azimuth> ();
				case FeatureType.CheckpointINS:
					return new List<CheckpointINS> ();
				case FeatureType.CheckpointVOR:
					return new List<CheckpointVOR> ();
				case FeatureType.DME:
					return new List<DME> ();
				case FeatureType.Elevation:
					return new List<Elevation> ();
				case FeatureType.Glidepath:
					return new List<Glidepath> ();
				case FeatureType.Localizer:
					return new List<Localizer> ();
				case FeatureType.MarkerBeacon:
					return new List<MarkerBeacon> ();
				case FeatureType.Navaid:
					return new List<Navaid> ();
				case FeatureType.SDF:
					return new List<SDF> ();
				case FeatureType.NDB:
					return new List<NDB> ();
				case FeatureType.SpecialNavigationStation:
					return new List<SpecialNavigationStation> ();
				case FeatureType.VOR:
					return new List<VOR> ();
				case FeatureType.TACAN:
					return new List<TACAN> ();
				case FeatureType.SpecialNavigationSystem:
					return new List<SpecialNavigationSystem> ();
				case FeatureType.DirectionFinder:
					return new List<DirectionFinder> ();
				case FeatureType.DesignatedPoint:
					return new List<DesignatedPoint> ();
				case FeatureType.SignificantPointInAirspace:
					return new List<SignificantPointInAirspace> ();
				case FeatureType.AeronauticalGroundLight:
					return new List<AeronauticalGroundLight> ();
				case FeatureType.OrganisationAuthority:
					return new List<OrganisationAuthority> ();
				case FeatureType.Unit:
					return new List<Unit> ();
				case FeatureType.ObstacleArea:
					return new List<ObstacleArea> ();
				case FeatureType.VerticalStructure:
					return new List<VerticalStructure> ();
				case FeatureType.CirclingArea:
					return new List<CirclingArea> ();
				case FeatureType.TerminalArrivalArea:
					return new List<TerminalArrivalArea> ();
				case FeatureType.InstrumentApproachProcedure:
					return new List<InstrumentApproachProcedure> ();
				case FeatureType.StandardInstrumentDeparture:
					return new List<StandardInstrumentDeparture> ();
				case FeatureType.NavigationArea:
					return new List<NavigationArea> ();
				case FeatureType.StandardInstrumentArrival:
					return new List<StandardInstrumentArrival> ();
				case FeatureType.NavigationAreaRestriction:
					return new List<NavigationAreaRestriction> ();
				case FeatureType.ArrivalFeederLeg:
					return new List<ArrivalFeederLeg> ();
				case FeatureType.ArrivalLeg:
					return new List<ArrivalLeg> ();
				case FeatureType.DepartureLeg:
					return new List<DepartureLeg> ();
				case FeatureType.FinalLeg:
					return new List<FinalLeg> ();
				case FeatureType.InitialLeg:
					return new List<InitialLeg> ();
				case FeatureType.IntermediateLeg:
					return new List<IntermediateLeg> ();
				case FeatureType.MissedApproachLeg:
					return new List<MissedApproachLeg> ();
				case FeatureType.ProcedureDME:
					return new List<ProcedureDME> ();
				case FeatureType.SafeAltitudeArea:
					return new List<SafeAltitudeArea> ();
				case FeatureType.HoldingPattern:
					return new List<HoldingPattern> ();
				case FeatureType.UnplannedHolding:
					return new List<UnplannedHolding> ();
				case FeatureType.AirspaceBorderCrossing:
					return new List<AirspaceBorderCrossing> ();
				case FeatureType.FlightRestriction:
					return new List<FlightRestriction> ();
				case FeatureType.RouteSegment:
					return new List<RouteSegment> ();
				case FeatureType.RouteDME:
					return new List<RouteDME> ();
				case FeatureType.Route:
					return new List<Route> ();
				case FeatureType.ChangeOverPoint:
					return new List<ChangeOverPoint> ();
				case FeatureType.AerialRefuelling:
					return new List<AerialRefuelling> ();
				case FeatureType.RulesProcedures:
					return new List<RulesProcedures> ();
				default:
					throw new Exception ("Create FeatureType List is not supported for type: " + aFeatureType);
			}
		}


		public static IList CreateAbstractList (AbstractType aAbstractType)
		{
			switch (aAbstractType)
			{
				case AbstractType.GroundLightSystem:
					return new List<GroundLightSystem> ();
				case AbstractType.Marking:
					return new List<Marking> ();
				case AbstractType.SurfaceContamination:
					return new List<SurfaceContamination> ();
				case AbstractType.UsageCondition:
					return new List<UsageCondition> ();
				case AbstractType.AirportHeliportProtectionArea:
					return new List<AirportHeliportProtectionArea> ();
				case AbstractType.RadarEquipment:
					return new List<RadarEquipment> ();
				case AbstractType.SurveillanceRadar:
					return new List<SurveillanceRadar> ();
				case AbstractType.PropertiesWithSchedule:
					return new List<PropertiesWithSchedule> ();
				case AbstractType.Service:
					return new List<Service> ();
				case AbstractType.TrafficSeparationService:
					return new List<TrafficSeparationService> ();
				case AbstractType.AirportGroundService:
					return new List<AirportGroundService> ();
				case AbstractType.SegmentPoint:
					return new List<SegmentPoint> ();
				case AbstractType.NavaidEquipment:
					return new List<NavaidEquipment> ();
				case AbstractType.NavigationSystemCheckpoint:
					return new List<NavigationSystemCheckpoint> ();
				case AbstractType.Procedure:
					return new List<Procedure> ();
				case AbstractType.SegmentLeg:
					return new List<SegmentLeg> ();
				case AbstractType.ApproachLeg:
					return new List<ApproachLeg> ();
				case AbstractType.DirectFlight:
					return new List<DirectFlight> ();
				case AbstractType.MDIdentification:
					return new List<MDIdentification> ();
			    case AbstractType.BtAbstractObject:
			        return new List<BtAbstractObject>();
			    case AbstractType.BtAbstractTimePrimitive:
			        return new List<BtAbstractTimePrimitive>();
			    case AbstractType.MdConstraints:
			        return new List<MdConstraints>();
			    case AbstractType.MdAbstractIdentification:
			        return new List<MdAbstractIdentification>();
			    case AbstractType.DqAbstractResult:
			        return new List<DqAbstractResult>();
			    case AbstractType.DqAbstractElement:
			        return new List<DqAbstractElement>();
                default:
					throw new Exception ("Create AbstractType List is not supported for type: " + aAbstractType);
			}
		}

		public static AimField CreateEnumType (EnumType aEnumType)
		{
			switch (aEnumType)
			{
				case EnumType.CIRoleCode:
					return new AimField<CIRoleCode> ();
				case EnumType.CIDateTypeCode:
					return new AimField<CIDateTypeCode> ();
				case EnumType.DQEvaluationMethodTypeCode:
					return new AimField<DQEvaluationMethodTypeCode> ();
				case EnumType.MDProgressCode:
					return new AimField<MDProgressCode> ();
				case EnumType.UomFL:
					return new AimField<UomFL> ();
				case EnumType.UomDistance:
					return new AimField<UomDistance> ();
				case EnumType.UomDistanceVertical:
					return new AimField<UomDistanceVertical> ();
				case EnumType.UomDuration:
					return new AimField<UomDuration> ();
				case EnumType.UomFrequency:
					return new AimField<UomFrequency> ();
				case EnumType.UomPressure:
					return new AimField<UomPressure> ();
				case EnumType.UomSpeed:
					return new AimField<UomSpeed> ();
				case EnumType.UomTemperature:
					return new AimField<UomTemperature> ();
				case EnumType.UomWeight:
					return new AimField<UomWeight> ();
				case EnumType.UomDepth:
					return new AimField<UomDepth> ();
				case EnumType.UomLightIntensity:
					return new AimField<UomLightIntensity> ();
				case EnumType.CodeAircraftEngineNumber:
					return new AimField<CodeAircraftEngineNumber> ();
				case EnumType.CodeAirspaceActivity:
					return new AimField<CodeAirspaceActivity> ();
				case EnumType.CodeAircraftCategory:
					return new AimField<CodeAircraftCategory> ();
				case EnumType.CodeFireFighting:
					return new AimField<CodeFireFighting> ();
				case EnumType.CodeFuel:
					return new AimField<CodeFuel> ();
				case EnumType.CodeOil:
					return new AimField<CodeOil> ();
				case EnumType.CodeDMEChannel:
					return new AimField<CodeDMEChannel> ();
				case EnumType.CodeMLSChannel:
					return new AimField<CodeMLSChannel> ();
				case EnumType.CodeTACANChannel:
					return new AimField<CodeTACANChannel> ();
				case EnumType.CodeHelicopterPerformance:
					return new AimField<CodeHelicopterPerformance> ();
				case EnumType.CodeMarkerBeaconSignal:
					return new AimField<CodeMarkerBeaconSignal> ();
				case EnumType.CodeNDBUsage:
					return new AimField<CodeNDBUsage> ();
				case EnumType.CodeColour:
					return new AimField<CodeColour> ();
				case EnumType.CodeTimeEventCombination:
					return new AimField<CodeTimeEventCombination> ();
				case EnumType.CodeComparison:
					return new AimField<CodeComparison> ();
				case EnumType.CodeSurfaceComposition:
					return new AimField<CodeSurfaceComposition> ();
				case EnumType.CodeSurfaceCondition:
					return new AimField<CodeSurfaceCondition> ();
				case EnumType.CodeDay:
					return new AimField<CodeDay> ();
				case EnumType.CodeAltitudeUse:
					return new AimField<CodeAltitudeUse> ();
				case EnumType.CodeDirection:
					return new AimField<CodeDirection> ();
				case EnumType.CodeDirectionReference:
					return new AimField<CodeDirectionReference> ();
				case EnumType.CodeDirectionTurn:
					return new AimField<CodeDirectionTurn> ();
				case EnumType.CodeVerticalReference:
					return new AimField<CodeVerticalReference> ();
				case EnumType.CodeRadioEmission:
					return new AimField<CodeRadioEmission> ();
				case EnumType.CodeFlowConditionOperation:
					return new AimField<CodeFlowConditionOperation> ();
				case EnumType.CodeFlightStatus:
					return new AimField<CodeFlightStatus> ();
				case EnumType.CodeFlight:
					return new AimField<CodeFlight> ();
				case EnumType.CodeProcedureFixRole:
					return new AimField<CodeProcedureFixRole> ();
				case EnumType.CodeLevelTableDesignator:
					return new AimField<CodeLevelTableDesignator> ();
				case EnumType.CodeRouteOrigin:
					return new AimField<CodeRouteOrigin> ();
				case EnumType.CodeLightIntensity:
					return new AimField<CodeLightIntensity> ();
				case EnumType.CodeAviationStandards:
					return new AimField<CodeAviationStandards> ();
				case EnumType.CodeLevel:
					return new AimField<CodeLevel> ();
				case EnumType.CodeMilitaryStatus:
					return new AimField<CodeMilitaryStatus> ();
				case EnumType.CodeAirspaceAggregation:
					return new AimField<CodeAirspaceAggregation> ();
				case EnumType.CodeFlightOrigin:
					return new AimField<CodeFlightOrigin> ();
				case EnumType.CodePCNMethod:
					return new AimField<CodePCNMethod> ();
				case EnumType.CodePCNTyrePressure:
					return new AimField<CodePCNTyrePressure> ();
				case EnumType.CodePCNSubgrade:
					return new AimField<CodePCNSubgrade> ();
				case EnumType.CodePCNPavement:
					return new AimField<CodePCNPavement> ();
				case EnumType.CodeProcedurePhase:
					return new AimField<CodeProcedurePhase> ();
				case EnumType.CodeSurfacePreparation:
					return new AimField<CodeSurfacePreparation> ();
				case EnumType.CodeApronSection:
					return new AimField<CodeApronSection> ();
				case EnumType.CodePositionInILS:
					return new AimField<CodePositionInILS> ();
				case EnumType.CodeSide:
					return new AimField<CodeSide> ();
				case EnumType.CodeRunwaySection:
					return new AimField<CodeRunwaySection> ();
				case EnumType.CodeTLOFSection:
					return new AimField<CodeTLOFSection> ();
				case EnumType.CodeTaxiwaySection:
					return new AimField<CodeTaxiwaySection> ();
				case EnumType.CodeFlightPurpose:
					return new AimField<CodeFlightPurpose> ();
				case EnumType.CodeLocationQualifier:
					return new AimField<CodeLocationQualifier> ();
				case EnumType.CodeATCReporting:
					return new AimField<CodeATCReporting> ();
				case EnumType.CodeRouteAvailability:
					return new AimField<CodeRouteAvailability> ();
				case EnumType.CodeRVSMPointRole:
					return new AimField<CodeRVSMPointRole> ();
				case EnumType.CodeSpecialDate:
					return new AimField<CodeSpecialDate> ();
				case EnumType.CodeSpeedReference:
					return new AimField<CodeSpeedReference> ();
				case EnumType.CodeTimeEvent:
					return new AimField<CodeTimeEvent> ();
				case EnumType.CodeTimeReference:
					return new AimField<CodeTimeReference> ();
				case EnumType.CodeAircraft:
					return new AimField<CodeAircraft> ();
				case EnumType.CodeAircraftEngine:
					return new AimField<CodeAircraftEngine> ();
				case EnumType.CodeAirportHeliport:
					return new AimField<CodeAirportHeliport> ();
				case EnumType.CodeAirportHeliportCollocation:
					return new AimField<CodeAirportHeliportCollocation> ();
				case EnumType.CodeGroundLighting:
					return new AimField<CodeGroundLighting> ();
				case EnumType.CodeApproachLighting:
					return new AimField<CodeApproachLighting> ();
				case EnumType.CodeBearing:
					return new AimField<CodeBearing> ();
				case EnumType.CodeAirspace:
					return new AimField<CodeAirspace> ();
				case EnumType.CodeAuthority:
					return new AimField<CodeAuthority> ();
				case EnumType.CodeAirspacePointRole:
					return new AimField<CodeAirspacePointRole> ();
				case EnumType.CodeOrganisationHierarchy:
					return new AimField<CodeOrganisationHierarchy> ();
				case EnumType.CodeMLSAzimuth:
					return new AimField<CodeMLSAzimuth> ();
				case EnumType.CodeCourse:
					return new AimField<CodeCourse> ();
				case EnumType.CodeDeclaredDistance:
					return new AimField<CodeDeclaredDistance> ();
				case EnumType.CodeDesignatedPoint:
					return new AimField<CodeDesignatedPoint> ();
				case EnumType.CodeDME:
					return new AimField<CodeDME> ();
				case EnumType.CodeFlightRule:
					return new AimField<CodeFlightRule> ();
				case EnumType.CodeGeoBorder:
					return new AimField<CodeGeoBorder> ();
				case EnumType.CodeHoldingUsage:
					return new AimField<CodeHoldingUsage> ();
				case EnumType.CodeLightSource:
					return new AimField<CodeLightSource> ();
				case EnumType.CodeMilitaryOperations:
					return new AimField<CodeMilitaryOperations> ();
				case EnumType.CodeRadioFrequencyArea:
					return new AimField<CodeRadioFrequencyArea> ();
				case EnumType.CodeNitrogen:
					return new AimField<CodeNitrogen> ();
				case EnumType.CodeNorthReference:
					return new AimField<CodeNorthReference> ();
				case EnumType.CodeOrganisation:
					return new AimField<CodeOrganisation> ();
				case EnumType.CodeOxygen:
					return new AimField<CodeOxygen> ();
				case EnumType.CodeSegmentPath:
					return new AimField<CodeSegmentPath> ();
				case EnumType.CodeRunwayProtectionArea:
					return new AimField<CodeRunwayProtectionArea> ();
				case EnumType.CodeFlightRestriction:
					return new AimField<CodeFlightRestriction> ();
				case EnumType.CodeRouteNavigation:
					return new AimField<CodeRouteNavigation> ();
				case EnumType.CodeRouteSegmentPath:
					return new AimField<CodeRouteSegmentPath> ();
				case EnumType.CodeAircraftGroundService:
					return new AimField<CodeAircraftGroundService> ();
				case EnumType.CodeSpecialNavigationStation:
					return new AimField<CodeSpecialNavigationStation> ();
				case EnumType.CodeSpecialNavigationSystem:
					return new AimField<CodeSpecialNavigationSystem> ();
				case EnumType.CodeAircraftStand:
					return new AimField<CodeAircraftStand> ();
				case EnumType.CodeTaxiway:
					return new AimField<CodeTaxiway> ();
				case EnumType.CodeUnit:
					return new AimField<CodeUnit> ();
				case EnumType.CodeILSBackCourse:
					return new AimField<CodeILSBackCourse> ();
				case EnumType.CodeVASIS:
					return new AimField<CodeVASIS> ();
				case EnumType.CodeUsageLimitation:
					return new AimField<CodeUsageLimitation> ();
				case EnumType.CodeStatusConstruction:
					return new AimField<CodeStatusConstruction> ();
				case EnumType.CodeStatusOperations:
					return new AimField<CodeStatusOperations> ();
				case EnumType.CodeVerticalStructure:
					return new AimField<CodeVerticalStructure> ();
				case EnumType.CodeVerticalStructureMarking:
					return new AimField<CodeVerticalStructureMarking> ();
				case EnumType.CodeCardinalDirection:
					return new AimField<CodeCardinalDirection> ();
				case EnumType.CodeFreeFlight:
					return new AimField<CodeFreeFlight> ();
				case EnumType.CodeNavaidService:
					return new AimField<CodeNavaidService> ();
				case EnumType.CodeVerticalDatum:
					return new AimField<CodeVerticalDatum> ();
				case EnumType.CodeRouteDesignatorPrefix:
					return new AimField<CodeRouteDesignatorPrefix> ();
				case EnumType.CodeRVSM:
					return new AimField<CodeRVSM> ();
				case EnumType.CodeRouteDesignatorLetter:
					return new AimField<CodeRouteDesignatorLetter> ();
				case EnumType.CodeRouteDesignatorSuffix:
					return new AimField<CodeRouteDesignatorSuffix> ();
				case EnumType.CodeStatusAirspace:
					return new AimField<CodeStatusAirspace> ();
				case EnumType.CodeRunwayPointRole:
					return new AimField<CodeRunwayPointRole> ();
				case EnumType.CodeRunway:
					return new AimField<CodeRunway> ();
				case EnumType.CodeMarkingStyle:
					return new AimField<CodeMarkingStyle> ();
				case EnumType.CodeFinalGuidance:
					return new AimField<CodeFinalGuidance> ();
				case EnumType.CodeMissedApproach:
					return new AimField<CodeMissedApproach> ();
				case EnumType.CodeUpperAlpha:
					return new AimField<CodeUpperAlpha> ();
				case EnumType.CodeApproachPrefix:
					return new AimField<CodeApproachPrefix> ();
				case EnumType.CodeApproach:
					return new AimField<CodeApproach> ();
				case EnumType.CodeApproval:
					return new AimField<CodeApproval> ();
				case EnumType.CodeDesignStandard:
					return new AimField<CodeDesignStandard> ();
				case EnumType.CodeApproachEquipmentAdditional:
					return new AimField<CodeApproachEquipmentAdditional> ();
				case EnumType.CodeEquipmentUnavailable:
					return new AimField<CodeEquipmentUnavailable> ();
				case EnumType.CodeHoldingUse:
					return new AimField<CodeHoldingUse> ();
				case EnumType.CodeProcedureDistance:
					return new AimField<CodeProcedureDistance> ();
				case EnumType.CodeMinimaFinalApproachPath:
					return new AimField<CodeMinimaFinalApproachPath> ();
				case EnumType.CodeMinimumAltitude:
					return new AimField<CodeMinimumAltitude> ();
				case EnumType.CodeMinimumHeight:
					return new AimField<CodeMinimumHeight> ();
				case EnumType.CodeProcedureCodingStandard:
					return new AimField<CodeProcedureCodingStandard> ();
				case EnumType.CodeSafeAltitude:
					return new AimField<CodeSafeAltitude> ();
				case EnumType.CodeObstacleAssessmentSurface:
					return new AimField<CodeObstacleAssessmentSurface> ();
				case EnumType.CodeAltitudeAdjustment:
					return new AimField<CodeAltitudeAdjustment> ();
				case EnumType.CodeTAA:
					return new AimField<CodeTAA> ();
				case EnumType.CodeArcDirection:
					return new AimField<CodeArcDirection> ();
				case EnumType.CodeLevelSeries:
					return new AimField<CodeLevelSeries> ();
				case EnumType.CodeSegmentTermination:
					return new AimField<CodeSegmentTermination> ();
				case EnumType.CodeReferenceRole:
					return new AimField<CodeReferenceRole> ();
				case EnumType.CodeTrajectory:
					return new AimField<CodeTrajectory> ();
				case EnumType.CodeRunwayMarking:
					return new AimField<CodeRunwayMarking> ();
				case EnumType.CodeRVRReading:
					return new AimField<CodeRVRReading> ();
				case EnumType.CodeStatusNavaid:
					return new AimField<CodeStatusNavaid> ();
				case EnumType.CodeOperationAirportHeliport:
					return new AimField<CodeOperationAirportHeliport> ();
				case EnumType.CodeDistanceIndication:
					return new AimField<CodeDistanceIndication> ();
				case EnumType.CodeObstacleArea:
					return new AimField<CodeObstacleArea> ();
				case EnumType.CodeAirspaceClassification:
					return new AimField<CodeAirspaceClassification> ();
				case EnumType.CodeApronElement:
					return new AimField<CodeApronElement> ();
				case EnumType.CodeRunwayElement:
					return new AimField<CodeRunwayElement> ();
				case EnumType.CodeAuthorityRole:
					return new AimField<CodeAuthorityRole> ();
				case EnumType.CodeVOR:
					return new AimField<CodeVOR> ();
				case EnumType.CodeApproachLightingICAO:
					return new AimField<CodeApproachLightingICAO> ();
				case EnumType.CodeLightingJAR:
					return new AimField<CodeLightingJAR> ();
				case EnumType.CodeWorkArea:
					return new AimField<CodeWorkArea> ();
				case EnumType.CodeVerticalStructureMaterial:
					return new AimField<CodeVerticalStructureMaterial> ();
				case EnumType.CodeGuidanceLine:
					return new AimField<CodeGuidanceLine> ();
				case EnumType.CodeAirportWarning:
					return new AimField<CodeAirportWarning> ();
				case EnumType.CodeProtectAreaSection:
					return new AimField<CodeProtectAreaSection> ();
				case EnumType.CodeRoute:
					return new AimField<CodeRoute> ();
				case EnumType.CodeAirspacePointPosition:
					return new AimField<CodeAirspacePointPosition> ();
				case EnumType.CodeContamination:
					return new AimField<CodeContamination> ();
				case EnumType.CodeFrictionDevice:
					return new AimField<CodeFrictionDevice> ();
				case EnumType.CodeFrictionEstimate:
					return new AimField<CodeFrictionEstimate> ();
				case EnumType.CodeHeightReference:
					return new AimField<CodeHeightReference> ();
				case EnumType.CodeAerialRefuellingPoint:
					return new AimField<CodeAerialRefuellingPoint> ();
				case EnumType.CodeAerialRefuelling:
					return new AimField<CodeAerialRefuelling> ();
				case EnumType.CodeLoadingBridge:
					return new AimField<CodeLoadingBridge> ();
				case EnumType.CodeApproachGuidance:
					return new AimField<CodeApproachGuidance> ();
				case EnumType.CodeAircraftWingspanClass:
					return new AimField<CodeAircraftWingspanClass> ();
				case EnumType.CodeArrestingGearEngageDevice:
					return new AimField<CodeArrestingGearEngageDevice> ();
				case EnumType.CodeObstructionIdSurfaceZone:
					return new AimField<CodeObstructionIdSurfaceZone> ();
				case EnumType.CodeBuoy:
					return new AimField<CodeBuoy> ();
				case EnumType.CodeRoad:
					return new AimField<CodeRoad> ();
				case EnumType.CodeAerialRefuellingPrefix:
					return new AimField<CodeAerialRefuellingPrefix> ();
				case EnumType.CodeMilitaryTraining:
					return new AimField<CodeMilitaryTraining> ();
				case EnumType.CodeFacilityRanking:
					return new AimField<CodeFacilityRanking> ();
				case EnumType.CodeMilitaryRoutePoint:
					return new AimField<CodeMilitaryRoutePoint> ();
				case EnumType.CodeArrestingGearEnergyAbsorb:
					return new AimField<CodeArrestingGearEnergyAbsorb> ();
				case EnumType.CodeEmissionBand:
					return new AimField<CodeEmissionBand> ();
				case EnumType.CodeMeteoConditions:
					return new AimField<CodeMeteoConditions> ();
				case EnumType.CodeValueInterpretation:
					return new AimField<CodeValueInterpretation> ();
				case EnumType.CodeNavigationArea:
					return new AimField<CodeNavigationArea> ();
				case EnumType.CodeProcedureAvailability:
					return new AimField<CodeProcedureAvailability> ();
				case EnumType.CodePilotControlledLighting:
					return new AimField<CodePilotControlledLighting> ();
				case EnumType.CodeIntensityStandBy:
					return new AimField<CodeIntensityStandBy> ();
				case EnumType.CodeTelecomNetwork:
					return new AimField<CodeTelecomNetwork> ();
				case EnumType.CodeFlightDestination:
					return new AimField<CodeFlightDestination> ();
				case EnumType.CodeServiceInformation:
					return new AimField<CodeServiceInformation> ();
				case EnumType.CodeServiceATFM:
					return new AimField<CodeServiceATFM> ();
				case EnumType.CodeServiceATC:
					return new AimField<CodeServiceATC> ();
				case EnumType.CodeServiceSAR:
					return new AimField<CodeServiceSAR> ();
				case EnumType.CodeCommunicationChannel:
					return new AimField<CodeCommunicationChannel> ();
				case EnumType.CodePassengerService:
					return new AimField<CodePassengerService> ();
				case EnumType.CodeServiceGroundControl:
					return new AimField<CodeServiceGroundControl> ();
				case EnumType.CodeStatusService:
					return new AimField<CodeStatusService> ();
				case EnumType.CodeCommunicationMode:
					return new AimField<CodeCommunicationMode> ();
				case EnumType.CodeCommunicationDirection:
					return new AimField<CodeCommunicationDirection> ();
				case EnumType.CodeCheckpointCategory:
					return new AimField<CodeCheckpointCategory> ();
				case EnumType.CodeLightHoldingPosition:
					return new AimField<CodeLightHoldingPosition> ();
				case EnumType.CodeNavigationAreaRestriction:
					return new AimField<CodeNavigationAreaRestriction> ();
				case EnumType.CodeSystemActivation:
					return new AimField<CodeSystemActivation> ();
				case EnumType.CodeWakeTurbulence:
					return new AimField<CodeWakeTurbulence> ();
				case EnumType.CodePAR:
					return new AimField<CodePAR> ();
				case EnumType.CodePrimaryRadar:
					return new AimField<CodePrimaryRadar> ();
				case EnumType.CodeRadarService:
					return new AimField<CodeRadarService> ();
				case EnumType.CodeReflector:
					return new AimField<CodeReflector> ();
				case EnumType.CodeStandbyPower:
					return new AimField<CodeStandbyPower> ();
				case EnumType.CodeTransponder:
					return new AimField<CodeTransponder> ();
				case EnumType.CodeAirspaceDependency:
					return new AimField<CodeAirspaceDependency> ();
				case EnumType.CodeUnitDependency:
					return new AimField<CodeUnitDependency> ();
				case EnumType.CodeVisualDockingGuidance:
					return new AimField<CodeVisualDockingGuidance> ();
				case EnumType.CodeMarkingCondition:
					return new AimField<CodeMarkingCondition> ();
				case EnumType.CodeNavaidPurpose:
					return new AimField<CodeNavaidPurpose> ();
				case EnumType.CodeRuleProcedure:
					return new AimField<CodeRuleProcedure> ();
				case EnumType.CodeRuleProcedureTitle:
					return new AimField<CodeRuleProcedureTitle> ();
				case EnumType.CodeGradeSeparation:
					return new AimField<CodeGradeSeparation> ();
				case EnumType.CodeTaxiwayElement:
					return new AimField<CodeTaxiwayElement> ();
				case EnumType.CodeRadioSignal:
					return new AimField<CodeRadioSignal> ();
				case EnumType.CodeSignalPerformanceILS:
					return new AimField<CodeSignalPerformanceILS> ();
				case EnumType.CodeCourseQualityILS:
					return new AimField<CodeCourseQualityILS> ();
				case EnumType.CodeIntegrityLevelILS:
					return new AimField<CodeIntegrityLevelILS> ();
				case EnumType.CodeHoldingCategory:
					return new AimField<CodeHoldingCategory> ();
				case EnumType.CodeNotePurpose:
					return new AimField<CodeNotePurpose> ();
				case EnumType.CodeNavigationEquipment:
					return new AimField<CodeNavigationEquipment> ();
				case EnumType.CodeNavigationSpecification:
					return new AimField<CodeNavigationSpecification> ();
				case EnumType.CodeEquipmentAntiCollision:
					return new AimField<CodeEquipmentAntiCollision> ();
				case EnumType.CodeLogicalOperator:
					return new AimField<CodeLogicalOperator> ();
				case EnumType.CodeStatusAirport:
					return new AimField<CodeStatusAirport> ();
				case EnumType.CodeOperationManoeuvringArea:
					return new AimField<CodeOperationManoeuvringArea> ();
				case EnumType.CodeRelativePosition:
					return new AimField<CodeRelativePosition> ();
				case EnumType.TimeSliceInterpretationType:
					return new AimField<TimeSliceInterpretationType> ();
				case EnumType.PhoneCodeType:
					return new AimField<PhoneCodeType> ();
				case EnumType.LanguageCodeType:
					return new AimField<LanguageCodeType> ();
				case EnumType.RestrictionCode:
					return new AimField<RestrictionCode> ();
				case EnumType.ClassificationCode:
					return new AimField<ClassificationCode> ();
				case EnumType.MeasureClassCode:
					return new AimField<MeasureClassCode> ();
				case EnumType.GroundLightSystemType:
					return new AimField<int> ();
				case EnumType.MarkingType:
					return new AimField<int> ();
				case EnumType.AirportHeliportProtectionAreaType:
					return new AimField<int> ();
				case EnumType.RadarEquipmentType:
					return new AimField<int> ();
				case EnumType.SurveillanceRadarType:
					return new AimField<int> ();
				case EnumType.ServiceType:
					return new AimField<int> ();
				case EnumType.TrafficSeparationServiceType:
					return new AimField<int> ();
				case EnumType.AirportGroundServiceType:
					return new AimField<int> ();
				case EnumType.NavaidEquipmentType:
					return new AimField<int> ();
				case EnumType.NavigationSystemCheckpointType:
					return new AimField<int> ();
				case EnumType.ProcedureType:
					return new AimField<int> ();
				case EnumType.SegmentLegType:
					return new AimField<int> ();
				case EnumType.ApproachLegType:
					return new AimField<int> ();
				case EnumType.language:
					return new AimField<language> ();
			    case EnumType.MdOnLineFunctionCode:
			        return new AimField<MdOnLineFunctionCode>();
			    case EnumType.CiDateTypeCode:
			        return new AimField<CiDateTypeCode>();
			    case EnumType.CiRoleCode:
			        return new AimField<CiRoleCode>();
			    case EnumType.CiPresentationFormCode:
			        return new AimField<CiPresentationFormCode>();
			    case EnumType.MdTimeIndeterminateValueType:
			        return new AimField<MdTimeIndeterminateValueType>();
			    case EnumType.DqEvaluationMethodTypeCode:
			        return new AimField<DqEvaluationMethodTypeCode>();
			    case EnumType.MdRestrictionCode:
			        return new AimField<MdRestrictionCode>();
			    case EnumType.MdClassificationCode:
			        return new AimField<MdClassificationCode>();
			    case EnumType.MdProgressCode:
			        return new AimField<MdProgressCode>();
                default:
					throw new Exception ("Create EnumType is not supported for type: " + aEnumType);
			}
		}

		public static IList CreateEnumTypeList (EnumType aEnumType)
		{
			switch (aEnumType)
			{
				case EnumType.CIRoleCode:
					return new List<AimField<CIRoleCode>> ();
				case EnumType.CIDateTypeCode:
					return new List<AimField<CIDateTypeCode>> ();
				case EnumType.DQEvaluationMethodTypeCode:
					return new List<AimField<DQEvaluationMethodTypeCode>> ();
				case EnumType.MDProgressCode:
					return new List<AimField<MDProgressCode>> ();
				case EnumType.UomFL:
					return new List<AimField<UomFL>> ();
				case EnumType.UomDistance:
					return new List<AimField<UomDistance>> ();
				case EnumType.UomDistanceVertical:
					return new List<AimField<UomDistanceVertical>> ();
				case EnumType.UomDuration:
					return new List<AimField<UomDuration>> ();
				case EnumType.UomFrequency:
					return new List<AimField<UomFrequency>> ();
				case EnumType.UomPressure:
					return new List<AimField<UomPressure>> ();
				case EnumType.UomSpeed:
					return new List<AimField<UomSpeed>> ();
				case EnumType.UomTemperature:
					return new List<AimField<UomTemperature>> ();
				case EnumType.UomWeight:
					return new List<AimField<UomWeight>> ();
				case EnumType.UomDepth:
					return new List<AimField<UomDepth>> ();
				case EnumType.UomLightIntensity:
					return new List<AimField<UomLightIntensity>> ();
				case EnumType.CodeAircraftEngineNumber:
					return new List<AimField<CodeAircraftEngineNumber>> ();
				case EnumType.CodeAirspaceActivity:
					return new List<AimField<CodeAirspaceActivity>> ();
				case EnumType.CodeAircraftCategory:
					return new List<AimField<CodeAircraftCategory>> ();
				case EnumType.CodeFireFighting:
					return new List<AimField<CodeFireFighting>> ();
				case EnumType.CodeFuel:
					return new List<AimField<CodeFuel>> ();
				case EnumType.CodeOil:
					return new List<AimField<CodeOil>> ();
				case EnumType.CodeDMEChannel:
					return new List<AimField<CodeDMEChannel>> ();
				case EnumType.CodeMLSChannel:
					return new List<AimField<CodeMLSChannel>> ();
				case EnumType.CodeTACANChannel:
					return new List<AimField<CodeTACANChannel>> ();
				case EnumType.CodeHelicopterPerformance:
					return new List<AimField<CodeHelicopterPerformance>> ();
				case EnumType.CodeMarkerBeaconSignal:
					return new List<AimField<CodeMarkerBeaconSignal>> ();
				case EnumType.CodeNDBUsage:
					return new List<AimField<CodeNDBUsage>> ();
				case EnumType.CodeColour:
					return new List<AimField<CodeColour>> ();
				case EnumType.CodeTimeEventCombination:
					return new List<AimField<CodeTimeEventCombination>> ();
				case EnumType.CodeComparison:
					return new List<AimField<CodeComparison>> ();
				case EnumType.CodeSurfaceComposition:
					return new List<AimField<CodeSurfaceComposition>> ();
				case EnumType.CodeSurfaceCondition:
					return new List<AimField<CodeSurfaceCondition>> ();
				case EnumType.CodeDay:
					return new List<AimField<CodeDay>> ();
				case EnumType.CodeAltitudeUse:
					return new List<AimField<CodeAltitudeUse>> ();
				case EnumType.CodeDirection:
					return new List<AimField<CodeDirection>> ();
				case EnumType.CodeDirectionReference:
					return new List<AimField<CodeDirectionReference>> ();
				case EnumType.CodeDirectionTurn:
					return new List<AimField<CodeDirectionTurn>> ();
				case EnumType.CodeVerticalReference:
					return new List<AimField<CodeVerticalReference>> ();
				case EnumType.CodeRadioEmission:
					return new List<AimField<CodeRadioEmission>> ();
				case EnumType.CodeFlowConditionOperation:
					return new List<AimField<CodeFlowConditionOperation>> ();
				case EnumType.CodeFlightStatus:
					return new List<AimField<CodeFlightStatus>> ();
				case EnumType.CodeFlight:
					return new List<AimField<CodeFlight>> ();
				case EnumType.CodeProcedureFixRole:
					return new List<AimField<CodeProcedureFixRole>> ();
				case EnumType.CodeLevelTableDesignator:
					return new List<AimField<CodeLevelTableDesignator>> ();
				case EnumType.CodeRouteOrigin:
					return new List<AimField<CodeRouteOrigin>> ();
				case EnumType.CodeLightIntensity:
					return new List<AimField<CodeLightIntensity>> ();
				case EnumType.CodeAviationStandards:
					return new List<AimField<CodeAviationStandards>> ();
				case EnumType.CodeLevel:
					return new List<AimField<CodeLevel>> ();
				case EnumType.CodeMilitaryStatus:
					return new List<AimField<CodeMilitaryStatus>> ();
				case EnumType.CodeAirspaceAggregation:
					return new List<AimField<CodeAirspaceAggregation>> ();
				case EnumType.CodeFlightOrigin:
					return new List<AimField<CodeFlightOrigin>> ();
				case EnumType.CodePCNMethod:
					return new List<AimField<CodePCNMethod>> ();
				case EnumType.CodePCNTyrePressure:
					return new List<AimField<CodePCNTyrePressure>> ();
				case EnumType.CodePCNSubgrade:
					return new List<AimField<CodePCNSubgrade>> ();
				case EnumType.CodePCNPavement:
					return new List<AimField<CodePCNPavement>> ();
				case EnumType.CodeProcedurePhase:
					return new List<AimField<CodeProcedurePhase>> ();
				case EnumType.CodeSurfacePreparation:
					return new List<AimField<CodeSurfacePreparation>> ();
				case EnumType.CodeApronSection:
					return new List<AimField<CodeApronSection>> ();
				case EnumType.CodePositionInILS:
					return new List<AimField<CodePositionInILS>> ();
				case EnumType.CodeSide:
					return new List<AimField<CodeSide>> ();
				case EnumType.CodeRunwaySection:
					return new List<AimField<CodeRunwaySection>> ();
				case EnumType.CodeTLOFSection:
					return new List<AimField<CodeTLOFSection>> ();
				case EnumType.CodeTaxiwaySection:
					return new List<AimField<CodeTaxiwaySection>> ();
				case EnumType.CodeFlightPurpose:
					return new List<AimField<CodeFlightPurpose>> ();
				case EnumType.CodeLocationQualifier:
					return new List<AimField<CodeLocationQualifier>> ();
				case EnumType.CodeATCReporting:
					return new List<AimField<CodeATCReporting>> ();
				case EnumType.CodeRouteAvailability:
					return new List<AimField<CodeRouteAvailability>> ();
				case EnumType.CodeRVSMPointRole:
					return new List<AimField<CodeRVSMPointRole>> ();
				case EnumType.CodeSpecialDate:
					return new List<AimField<CodeSpecialDate>> ();
				case EnumType.CodeSpeedReference:
					return new List<AimField<CodeSpeedReference>> ();
				case EnumType.CodeTimeEvent:
					return new List<AimField<CodeTimeEvent>> ();
				case EnumType.CodeTimeReference:
					return new List<AimField<CodeTimeReference>> ();
				case EnumType.CodeAircraft:
					return new List<AimField<CodeAircraft>> ();
				case EnumType.CodeAircraftEngine:
					return new List<AimField<CodeAircraftEngine>> ();
				case EnumType.CodeAirportHeliport:
					return new List<AimField<CodeAirportHeliport>> ();
				case EnumType.CodeAirportHeliportCollocation:
					return new List<AimField<CodeAirportHeliportCollocation>> ();
				case EnumType.CodeGroundLighting:
					return new List<AimField<CodeGroundLighting>> ();
				case EnumType.CodeApproachLighting:
					return new List<AimField<CodeApproachLighting>> ();
				case EnumType.CodeBearing:
					return new List<AimField<CodeBearing>> ();
				case EnumType.CodeAirspace:
					return new List<AimField<CodeAirspace>> ();
				case EnumType.CodeAuthority:
					return new List<AimField<CodeAuthority>> ();
				case EnumType.CodeAirspacePointRole:
					return new List<AimField<CodeAirspacePointRole>> ();
				case EnumType.CodeOrganisationHierarchy:
					return new List<AimField<CodeOrganisationHierarchy>> ();
				case EnumType.CodeMLSAzimuth:
					return new List<AimField<CodeMLSAzimuth>> ();
				case EnumType.CodeCourse:
					return new List<AimField<CodeCourse>> ();
				case EnumType.CodeDeclaredDistance:
					return new List<AimField<CodeDeclaredDistance>> ();
				case EnumType.CodeDesignatedPoint:
					return new List<AimField<CodeDesignatedPoint>> ();
				case EnumType.CodeDME:
					return new List<AimField<CodeDME>> ();
				case EnumType.CodeFlightRule:
					return new List<AimField<CodeFlightRule>> ();
				case EnumType.CodeGeoBorder:
					return new List<AimField<CodeGeoBorder>> ();
				case EnumType.CodeHoldingUsage:
					return new List<AimField<CodeHoldingUsage>> ();
				case EnumType.CodeLightSource:
					return new List<AimField<CodeLightSource>> ();
				case EnumType.CodeMilitaryOperations:
					return new List<AimField<CodeMilitaryOperations>> ();
				case EnumType.CodeRadioFrequencyArea:
					return new List<AimField<CodeRadioFrequencyArea>> ();
				case EnumType.CodeNitrogen:
					return new List<AimField<CodeNitrogen>> ();
				case EnumType.CodeNorthReference:
					return new List<AimField<CodeNorthReference>> ();
				case EnumType.CodeOrganisation:
					return new List<AimField<CodeOrganisation>> ();
				case EnumType.CodeOxygen:
					return new List<AimField<CodeOxygen>> ();
				case EnumType.CodeSegmentPath:
					return new List<AimField<CodeSegmentPath>> ();
				case EnumType.CodeRunwayProtectionArea:
					return new List<AimField<CodeRunwayProtectionArea>> ();
				case EnumType.CodeFlightRestriction:
					return new List<AimField<CodeFlightRestriction>> ();
				case EnumType.CodeRouteNavigation:
					return new List<AimField<CodeRouteNavigation>> ();
				case EnumType.CodeRouteSegmentPath:
					return new List<AimField<CodeRouteSegmentPath>> ();
				case EnumType.CodeAircraftGroundService:
					return new List<AimField<CodeAircraftGroundService>> ();
				case EnumType.CodeSpecialNavigationStation:
					return new List<AimField<CodeSpecialNavigationStation>> ();
				case EnumType.CodeSpecialNavigationSystem:
					return new List<AimField<CodeSpecialNavigationSystem>> ();
				case EnumType.CodeAircraftStand:
					return new List<AimField<CodeAircraftStand>> ();
				case EnumType.CodeTaxiway:
					return new List<AimField<CodeTaxiway>> ();
				case EnumType.CodeUnit:
					return new List<AimField<CodeUnit>> ();
				case EnumType.CodeILSBackCourse:
					return new List<AimField<CodeILSBackCourse>> ();
				case EnumType.CodeVASIS:
					return new List<AimField<CodeVASIS>> ();
				case EnumType.CodeUsageLimitation:
					return new List<AimField<CodeUsageLimitation>> ();
				case EnumType.CodeStatusConstruction:
					return new List<AimField<CodeStatusConstruction>> ();
				case EnumType.CodeStatusOperations:
					return new List<AimField<CodeStatusOperations>> ();
				case EnumType.CodeVerticalStructure:
					return new List<AimField<CodeVerticalStructure>> ();
				case EnumType.CodeVerticalStructureMarking:
					return new List<AimField<CodeVerticalStructureMarking>> ();
				case EnumType.CodeCardinalDirection:
					return new List<AimField<CodeCardinalDirection>> ();
				case EnumType.CodeFreeFlight:
					return new List<AimField<CodeFreeFlight>> ();
				case EnumType.CodeNavaidService:
					return new List<AimField<CodeNavaidService>> ();
				case EnumType.CodeVerticalDatum:
					return new List<AimField<CodeVerticalDatum>> ();
				case EnumType.CodeRouteDesignatorPrefix:
					return new List<AimField<CodeRouteDesignatorPrefix>> ();
				case EnumType.CodeRVSM:
					return new List<AimField<CodeRVSM>> ();
				case EnumType.CodeRouteDesignatorLetter:
					return new List<AimField<CodeRouteDesignatorLetter>> ();
				case EnumType.CodeRouteDesignatorSuffix:
					return new List<AimField<CodeRouteDesignatorSuffix>> ();
				case EnumType.CodeStatusAirspace:
					return new List<AimField<CodeStatusAirspace>> ();
				case EnumType.CodeRunwayPointRole:
					return new List<AimField<CodeRunwayPointRole>> ();
				case EnumType.CodeRunway:
					return new List<AimField<CodeRunway>> ();
				case EnumType.CodeMarkingStyle:
					return new List<AimField<CodeMarkingStyle>> ();
				case EnumType.CodeFinalGuidance:
					return new List<AimField<CodeFinalGuidance>> ();
				case EnumType.CodeMissedApproach:
					return new List<AimField<CodeMissedApproach>> ();
				case EnumType.CodeUpperAlpha:
					return new List<AimField<CodeUpperAlpha>> ();
				case EnumType.CodeApproachPrefix:
					return new List<AimField<CodeApproachPrefix>> ();
				case EnumType.CodeApproach:
					return new List<AimField<CodeApproach>> ();
				case EnumType.CodeApproval:
					return new List<AimField<CodeApproval>> ();
				case EnumType.CodeDesignStandard:
					return new List<AimField<CodeDesignStandard>> ();
				case EnumType.CodeApproachEquipmentAdditional:
					return new List<AimField<CodeApproachEquipmentAdditional>> ();
				case EnumType.CodeEquipmentUnavailable:
					return new List<AimField<CodeEquipmentUnavailable>> ();
				case EnumType.CodeHoldingUse:
					return new List<AimField<CodeHoldingUse>> ();
				case EnumType.CodeProcedureDistance:
					return new List<AimField<CodeProcedureDistance>> ();
				case EnumType.CodeMinimaFinalApproachPath:
					return new List<AimField<CodeMinimaFinalApproachPath>> ();
				case EnumType.CodeMinimumAltitude:
					return new List<AimField<CodeMinimumAltitude>> ();
				case EnumType.CodeMinimumHeight:
					return new List<AimField<CodeMinimumHeight>> ();
				case EnumType.CodeProcedureCodingStandard:
					return new List<AimField<CodeProcedureCodingStandard>> ();
				case EnumType.CodeSafeAltitude:
					return new List<AimField<CodeSafeAltitude>> ();
				case EnumType.CodeObstacleAssessmentSurface:
					return new List<AimField<CodeObstacleAssessmentSurface>> ();
				case EnumType.CodeAltitudeAdjustment:
					return new List<AimField<CodeAltitudeAdjustment>> ();
				case EnumType.CodeTAA:
					return new List<AimField<CodeTAA>> ();
				case EnumType.CodeArcDirection:
					return new List<AimField<CodeArcDirection>> ();
				case EnumType.CodeLevelSeries:
					return new List<AimField<CodeLevelSeries>> ();
				case EnumType.CodeSegmentTermination:
					return new List<AimField<CodeSegmentTermination>> ();
				case EnumType.CodeReferenceRole:
					return new List<AimField<CodeReferenceRole>> ();
				case EnumType.CodeTrajectory:
					return new List<AimField<CodeTrajectory>> ();
				case EnumType.CodeRunwayMarking:
					return new List<AimField<CodeRunwayMarking>> ();
				case EnumType.CodeRVRReading:
					return new List<AimField<CodeRVRReading>> ();
				case EnumType.CodeStatusNavaid:
					return new List<AimField<CodeStatusNavaid>> ();
				case EnumType.CodeOperationAirportHeliport:
					return new List<AimField<CodeOperationAirportHeliport>> ();
				case EnumType.CodeDistanceIndication:
					return new List<AimField<CodeDistanceIndication>> ();
				case EnumType.CodeObstacleArea:
					return new List<AimField<CodeObstacleArea>> ();
				case EnumType.CodeAirspaceClassification:
					return new List<AimField<CodeAirspaceClassification>> ();
				case EnumType.CodeApronElement:
					return new List<AimField<CodeApronElement>> ();
				case EnumType.CodeRunwayElement:
					return new List<AimField<CodeRunwayElement>> ();
				case EnumType.CodeAuthorityRole:
					return new List<AimField<CodeAuthorityRole>> ();
				case EnumType.CodeVOR:
					return new List<AimField<CodeVOR>> ();
				case EnumType.CodeApproachLightingICAO:
					return new List<AimField<CodeApproachLightingICAO>> ();
				case EnumType.CodeLightingJAR:
					return new List<AimField<CodeLightingJAR>> ();
				case EnumType.CodeWorkArea:
					return new List<AimField<CodeWorkArea>> ();
				case EnumType.CodeVerticalStructureMaterial:
					return new List<AimField<CodeVerticalStructureMaterial>> ();
				case EnumType.CodeGuidanceLine:
					return new List<AimField<CodeGuidanceLine>> ();
				case EnumType.CodeAirportWarning:
					return new List<AimField<CodeAirportWarning>> ();
				case EnumType.CodeProtectAreaSection:
					return new List<AimField<CodeProtectAreaSection>> ();
				case EnumType.CodeRoute:
					return new List<AimField<CodeRoute>> ();
				case EnumType.CodeAirspacePointPosition:
					return new List<AimField<CodeAirspacePointPosition>> ();
				case EnumType.CodeContamination:
					return new List<AimField<CodeContamination>> ();
				case EnumType.CodeFrictionDevice:
					return new List<AimField<CodeFrictionDevice>> ();
				case EnumType.CodeFrictionEstimate:
					return new List<AimField<CodeFrictionEstimate>> ();
				case EnumType.CodeHeightReference:
					return new List<AimField<CodeHeightReference>> ();
				case EnumType.CodeAerialRefuellingPoint:
					return new List<AimField<CodeAerialRefuellingPoint>> ();
				case EnumType.CodeAerialRefuelling:
					return new List<AimField<CodeAerialRefuelling>> ();
				case EnumType.CodeLoadingBridge:
					return new List<AimField<CodeLoadingBridge>> ();
				case EnumType.CodeApproachGuidance:
					return new List<AimField<CodeApproachGuidance>> ();
				case EnumType.CodeAircraftWingspanClass:
					return new List<AimField<CodeAircraftWingspanClass>> ();
				case EnumType.CodeArrestingGearEngageDevice:
					return new List<AimField<CodeArrestingGearEngageDevice>> ();
				case EnumType.CodeObstructionIdSurfaceZone:
					return new List<AimField<CodeObstructionIdSurfaceZone>> ();
				case EnumType.CodeBuoy:
					return new List<AimField<CodeBuoy>> ();
				case EnumType.CodeRoad:
					return new List<AimField<CodeRoad>> ();
				case EnumType.CodeAerialRefuellingPrefix:
					return new List<AimField<CodeAerialRefuellingPrefix>> ();
				case EnumType.CodeMilitaryTraining:
					return new List<AimField<CodeMilitaryTraining>> ();
				case EnumType.CodeFacilityRanking:
					return new List<AimField<CodeFacilityRanking>> ();
				case EnumType.CodeMilitaryRoutePoint:
					return new List<AimField<CodeMilitaryRoutePoint>> ();
				case EnumType.CodeArrestingGearEnergyAbsorb:
					return new List<AimField<CodeArrestingGearEnergyAbsorb>> ();
				case EnumType.CodeEmissionBand:
					return new List<AimField<CodeEmissionBand>> ();
				case EnumType.CodeMeteoConditions:
					return new List<AimField<CodeMeteoConditions>> ();
				case EnumType.CodeValueInterpretation:
					return new List<AimField<CodeValueInterpretation>> ();
				case EnumType.CodeNavigationArea:
					return new List<AimField<CodeNavigationArea>> ();
				case EnumType.CodeProcedureAvailability:
					return new List<AimField<CodeProcedureAvailability>> ();
				case EnumType.CodePilotControlledLighting:
					return new List<AimField<CodePilotControlledLighting>> ();
				case EnumType.CodeIntensityStandBy:
					return new List<AimField<CodeIntensityStandBy>> ();
				case EnumType.CodeTelecomNetwork:
					return new List<AimField<CodeTelecomNetwork>> ();
				case EnumType.CodeFlightDestination:
					return new List<AimField<CodeFlightDestination>> ();
				case EnumType.CodeServiceInformation:
					return new List<AimField<CodeServiceInformation>> ();
				case EnumType.CodeServiceATFM:
					return new List<AimField<CodeServiceATFM>> ();
				case EnumType.CodeServiceATC:
					return new List<AimField<CodeServiceATC>> ();
				case EnumType.CodeServiceSAR:
					return new List<AimField<CodeServiceSAR>> ();
				case EnumType.CodeCommunicationChannel:
					return new List<AimField<CodeCommunicationChannel>> ();
				case EnumType.CodePassengerService:
					return new List<AimField<CodePassengerService>> ();
				case EnumType.CodeServiceGroundControl:
					return new List<AimField<CodeServiceGroundControl>> ();
				case EnumType.CodeStatusService:
					return new List<AimField<CodeStatusService>> ();
				case EnumType.CodeCommunicationMode:
					return new List<AimField<CodeCommunicationMode>> ();
				case EnumType.CodeCommunicationDirection:
					return new List<AimField<CodeCommunicationDirection>> ();
				case EnumType.CodeCheckpointCategory:
					return new List<AimField<CodeCheckpointCategory>> ();
				case EnumType.CodeLightHoldingPosition:
					return new List<AimField<CodeLightHoldingPosition>> ();
				case EnumType.CodeNavigationAreaRestriction:
					return new List<AimField<CodeNavigationAreaRestriction>> ();
				case EnumType.CodeSystemActivation:
					return new List<AimField<CodeSystemActivation>> ();
				case EnumType.CodeWakeTurbulence:
					return new List<AimField<CodeWakeTurbulence>> ();
				case EnumType.CodePAR:
					return new List<AimField<CodePAR>> ();
				case EnumType.CodePrimaryRadar:
					return new List<AimField<CodePrimaryRadar>> ();
				case EnumType.CodeRadarService:
					return new List<AimField<CodeRadarService>> ();
				case EnumType.CodeReflector:
					return new List<AimField<CodeReflector>> ();
				case EnumType.CodeStandbyPower:
					return new List<AimField<CodeStandbyPower>> ();
				case EnumType.CodeTransponder:
					return new List<AimField<CodeTransponder>> ();
				case EnumType.CodeAirspaceDependency:
					return new List<AimField<CodeAirspaceDependency>> ();
				case EnumType.CodeUnitDependency:
					return new List<AimField<CodeUnitDependency>> ();
				case EnumType.CodeVisualDockingGuidance:
					return new List<AimField<CodeVisualDockingGuidance>> ();
				case EnumType.CodeMarkingCondition:
					return new List<AimField<CodeMarkingCondition>> ();
				case EnumType.CodeNavaidPurpose:
					return new List<AimField<CodeNavaidPurpose>> ();
				case EnumType.CodeRuleProcedure:
					return new List<AimField<CodeRuleProcedure>> ();
				case EnumType.CodeRuleProcedureTitle:
					return new List<AimField<CodeRuleProcedureTitle>> ();
				case EnumType.CodeGradeSeparation:
					return new List<AimField<CodeGradeSeparation>> ();
				case EnumType.CodeTaxiwayElement:
					return new List<AimField<CodeTaxiwayElement>> ();
				case EnumType.CodeRadioSignal:
					return new List<AimField<CodeRadioSignal>> ();
				case EnumType.CodeSignalPerformanceILS:
					return new List<AimField<CodeSignalPerformanceILS>> ();
				case EnumType.CodeCourseQualityILS:
					return new List<AimField<CodeCourseQualityILS>> ();
				case EnumType.CodeIntegrityLevelILS:
					return new List<AimField<CodeIntegrityLevelILS>> ();
				case EnumType.CodeHoldingCategory:
					return new List<AimField<CodeHoldingCategory>> ();
				case EnumType.CodeNotePurpose:
					return new List<AimField<CodeNotePurpose>> ();
				case EnumType.CodeNavigationEquipment:
					return new List<AimField<CodeNavigationEquipment>> ();
				case EnumType.CodeNavigationSpecification:
					return new List<AimField<CodeNavigationSpecification>> ();
				case EnumType.CodeEquipmentAntiCollision:
					return new List<AimField<CodeEquipmentAntiCollision>> ();
				case EnumType.CodeLogicalOperator:
					return new List<AimField<CodeLogicalOperator>> ();
				case EnumType.CodeStatusAirport:
					return new List<AimField<CodeStatusAirport>> ();
				case EnumType.CodeOperationManoeuvringArea:
					return new List<AimField<CodeOperationManoeuvringArea>> ();
				case EnumType.CodeRelativePosition:
					return new List<AimField<CodeRelativePosition>> ();
				case EnumType.TimeSliceInterpretationType:
					return new List<AimField<TimeSliceInterpretationType>> ();
				case EnumType.PhoneCodeType:
					return new List<AimField<PhoneCodeType>> ();
				case EnumType.LanguageCodeType:
					return new List<AimField<LanguageCodeType>> ();
				case EnumType.RestrictionCode:
					return new List<AimField<RestrictionCode>> ();
				case EnumType.ClassificationCode:
					return new List<AimField<ClassificationCode>> ();
				case EnumType.MeasureClassCode:
					return new List<AimField<MeasureClassCode>> ();
				default:
					throw new Exception ("Create EnumType List is not supported for type: " + aEnumType);
			}
		}

	}
}
