using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Geometries;

namespace PandaLauncher.Util
{
    public abstract class AbstractPandaDataProvider : IPandaDataProvider
    {
        #region Abstract implementation of IPandaDataProvider

        public abstract DateTime GetActualDate();
        
        public abstract bool CommitWithoutViewer(FeatureType[] featureTypes);
        public abstract bool Commit();
        public abstract bool Commit(FeatureType[] featureTypes);

        public abstract Feature GetFeature(FeatureType featureType, Guid identifier);
        public Feature GetAbstractFeature(IAbstractFeatureRef abstractFeatureRef)
        {
            return GetFeature((FeatureType) abstractFeatureRef.FeatureTypeIndex, abstractFeatureRef.Identifier);
        }

        #endregion

        #region Implementation of IPandaDataProvider

        public void SetRootFeatureType(FeatureType rootFeatureType)
        {
        }

        public void ExcludeFeature(Guid identifier)
        {
            if (CreatedFeatureList.ContainsKey(identifier))
            {
                CreatedFeatureList.Remove(identifier);
            }
        }

        public void SetFeature(Feature feature)
        {
            if (!CreatedFeatureList.ContainsKey(feature.Identifier))
            {
                CreatedFeatureList.Add(feature.Identifier, feature);
            }
        }

        public TFeature CreateFeature<TFeature>() where TFeature : Feature, new()
        {
            var feature = new TFeature
                {
                    Id = -1,
                    Identifier = Guid.NewGuid(),
                    TimeSlice = new TimeSlice
                        {
                            Interpretation = TimeSliceInterpretationType.PERMDELTA,
                            ValidTime = new TimePeriod(GetActualDate()),
                            FeatureLifetime = new TimePeriod(GetActualDate()),
                            SequenceNumber = 1,
                            CorrectionNumber = 0
                        }
                };

            CreatedFeatureList.Add(feature.Identifier, feature);

            return feature;
        }

        public List<Descriptor> GetAirportHeliportList(Guid organisationIdentifier, bool checkILS)
        {
            if (checkILS)
            {
                //TODO: implement
                #warning
                return GetList(FeatureType.AirportHeliport).Cast<AirportHeliport>().
                    Where(
                        t => 
                        t.ResponsibleOrganisation != null && t.ResponsibleOrganisation.TheOrganisationAuthority != null &&
                        t.ResponsibleOrganisation.TheOrganisationAuthority.Identifier == organisationIdentifier).
                    Select(t => new Descriptor(t.Identifier, t.Designator)).ToList();
            }

            return GetList(FeatureType.AirportHeliport).Cast<AirportHeliport>().
                Where(
                    t =>
                    t.ResponsibleOrganisation != null && t.ResponsibleOrganisation.TheOrganisationAuthority != null &&
                    t.ResponsibleOrganisation.TheOrganisationAuthority.Identifier == organisationIdentifier).
                Select(t => new Descriptor(t.Identifier, t.Designator)).ToList();
        }

        public List<AirportHeliport> GetAirportHeliportList(Guid organisationIdentifier)
        {
            return GetList(FeatureType.AirportHeliport).Cast<AirportHeliport>().
             Where(t => t.ResponsibleOrganisation!=null &&  t.ResponsibleOrganisation.TheOrganisationAuthority !=null &&
                 t.ResponsibleOrganisation.TheOrganisationAuthority.Identifier == organisationIdentifier).ToList();
        }

        public List<Descriptor> GetRunwayList(Guid airportIdentifier)
        {
            return GetList(FeatureType.Runway).Cast<Runway>().
              Where(t => t.AssociatedAirportHeliport!=null &&
                  t.AssociatedAirportHeliport.Identifier == airportIdentifier).
              Select(t=>new Descriptor(t.Identifier, t.Designator)).ToList();
        }

        public List<RunwayDirection> GetRunwayDirectionList(Guid runwayIdentifier)
        {
            return GetList(FeatureType.RunwayDirection).Cast<RunwayDirection>().
              Where(t => t.UsedRunway!=null &&
                  t.UsedRunway.Identifier == runwayIdentifier).ToList();
        }

        public List<RunwayCentrelinePoint> GetRunwayCentrelinePointList(Guid rwyDirIdentifier)
        {
            return GetList(FeatureType.RunwayCentrelinePoint).Cast<RunwayCentrelinePoint>().
              Where(t => t.OnRunway!=null &&
                  t.OnRunway.Identifier == rwyDirIdentifier).ToList();
        }

        public List<RunwayProtectArea> GetRunwayProtectAreaList(Guid rwyDirIdentifier)
        {
            return GetList(FeatureType.RunwayProtectArea).Cast<RunwayProtectArea>().
               Where(t => t.ProtectedRunwayDirection!=null &&
                   t.ProtectedRunwayDirection.Identifier == rwyDirIdentifier).ToList();
        }

		public List<Navaid> GetNavaidList(MultiPolygon polygon)
		{
            return GetList(FeatureType.Navaid, polygon).Cast<Navaid>().ToList();
		}


		public List<Navaid> GetNavaidList()
		{
		    return GetList(FeatureType.InstrumentApproachProcedure).Cast<Navaid>().ToList();
		}

        public Navaid GetILSNavaid(Guid rwyDirIdentifier)
        {
            return GetList(FeatureType.InstrumentApproachProcedure).Cast<Navaid>().
                Where(t => t.Type == CodeNavaidService.ILS_DME || t.Type == CodeNavaidService.ILS &&
                            t.RunwayDirection!=null &&
                            t.RunwayDirection.Any(t1 => t1.Feature.Identifier == rwyDirIdentifier)).FirstOrDefault();
        }

        public List<VerticalStructure> GetVerticalStructureList(MultiPolygon polygon)
        {
            return GetList(FeatureType.VerticalStructure, polygon).Cast<VerticalStructure>().ToList();
        }

        public List<DesignatedPoint> GetDesignatedPointList(MultiPolygon polygon)
        {
            return GetList(FeatureType.DesignatedPoint, polygon).Cast<DesignatedPoint>().ToList();
        }

        public List<SafeAltitudeArea> GetSafeAltitudeAreaList(Guid navaidIdentifier)
        {
            return GetList(FeatureType.SafeAltitudeArea).Cast<SafeAltitudeArea>().
                    Where(t => t.CentrePoint!=null && t.CentrePoint.NavaidSystem!=null &&
                    t.CentrePoint.NavaidSystem.Identifier == navaidIdentifier).ToList();
        }

        public List<InstrumentApproachProcedure> GetIAPList(Guid airportIdentifier)
        {
            return GetList(FeatureType.InstrumentApproachProcedure).Cast<InstrumentApproachProcedure>().
                    Where(t => t.AirportHeliport!=null &&
                    t.AirportHeliport.Any(t1 => t1.Feature.Identifier == airportIdentifier)).ToList();
        }


        #endregion


        protected Dictionary<Guid, Feature> CreatedFeatureList;


        public abstract List<Feature> GetList(FeatureType featureType);
        public abstract List<Feature> GetList(FeatureType featureType, MultiPolygon polygon);
    }
}
