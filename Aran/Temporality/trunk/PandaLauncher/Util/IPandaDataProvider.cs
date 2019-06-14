using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Geometries;

namespace PandaLauncher.Util
{
    public class Descriptor
    {
        public Descriptor() : this(Guid.Empty, string.Empty)
        {
        }

        public Descriptor(Guid identifier, string name)
        {
            Identifier = identifier;
            Name = name;
        }

        public Guid Identifier { get; set; }

        public string Name { get; set; }

        public bool IsEmpty
        {
            get { return Identifier == Guid.Empty && Name == string.Empty; }
        }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }

    public interface IPandaDataProvider
    {
        //common part
        DateTime GetActualDate();

        void SetRootFeatureType(FeatureType rootFeatureType);

        bool CommitWithoutViewer(FeatureType[] featureTypes);
        
        //with viewer
        bool Commit();
        bool Commit(FeatureType[] featureTypes);

        void ExcludeFeature(Guid identifier);
        void SetFeature(Feature feature);
        TFeature CreateFeature<TFeature>() where TFeature : Feature, new();

        Feature GetFeature(FeatureType featureType, Guid identifier);
        Feature GetAbstractFeature(IAbstractFeatureRef abstractFeatureRef);

        //panda part

        //list airport (Designator+Guid) responsibleOrganization
        //if checkILS==true return only having ILS
        List<Descriptor> GetAirportHeliportList(Guid organisationIdentifier, bool checkILS);

        List<AirportHeliport> GetAirportHeliportList(Guid organisationIdentifier);

        List<Descriptor> GetRunwayList(Guid airportIdentifier);

        List<RunwayDirection> GetRunwayDirectionList(Guid runwayIdentifier);

        List<RunwayCentrelinePoint> GetRunwayCentrelinePointList(Guid rwyDirIdentifier);//RunwayDirection id

        List<RunwayProtectArea> GetRunwayProtectAreaList(Guid rwyDirIdentifier);

        List<Navaid> GetNavaidList(MultiPolygon polygon);

        List<Navaid> GetNavaidList();

        Navaid GetILSNavaid(Guid rwyDirIdentifier);//navaid having ILS

        List<VerticalStructure> GetVerticalStructureList(MultiPolygon polygon);

        List<DesignatedPoint> GetDesignatedPointList(MultiPolygon polygon);

        List<SafeAltitudeArea> GetSafeAltitudeAreaList(Guid navaidIdentifier);

        List<InstrumentApproachProcedure> GetIAPList(Guid airportIdentifier);
    }
}
