using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Objects;
using Aran.Geometries;

namespace TestAranGdbProvider
{
    public class GDBProviderBETA
    {
        private GDBProviderBETA _gdbProvider;
        private List<Feature> _featureList;

        public GDBProviderBETA ()
        {
            _gdbProvider = this;
            _featureList = new List<Feature> ();
        }

        public void Open ()
        {
            OrganisationAuthority org = CreateOrganisation ();
            _gdbProvider.SetFeature (org);
            AirportHeliport ah = CreateAirport (org.Identifier);
            _gdbProvider.SetFeature (ah);
            Runway runway = CreateRunway (ah.Identifier);
            _gdbProvider.SetFeature (runway);
            InsertRunwayGroup (runway.Identifier);
        }

        public IEnumerable<OrganisationAuthority> GetOrganisations ()
        {
            foreach (Feature feature in _featureList)
            {
                if (feature.FeatureType == Aran.Aim.FeatureType.OrganisationAuthority)
                    yield return (OrganisationAuthority) feature;
            }
        }

        public IEnumerable<AirportHeliport> GetAirports (Guid organisationIdentifier)
        {
            foreach (Feature feature in _featureList)
            {
                if (feature.FeatureType == Aran.Aim.FeatureType.AirportHeliport)
                    yield return (AirportHeliport) feature;
            }
        }

        public IEnumerable<Runway> GetRunways (Guid airportIdentifier)
        {
            foreach (Feature feature in _featureList)
            {
                if (feature.FeatureType == Aran.Aim.FeatureType.Runway)
                    yield return (Runway) feature;
            }
        }

        public IEnumerable<RunwayDirection> GetRunwayDirections (Guid runwayIdentifier)
        {
            foreach (Feature feature in _featureList)
            {
                if (feature.FeatureType == Aran.Aim.FeatureType.RunwayDirection)
                    yield return (RunwayDirection) feature;
            }
        }

        public IEnumerable<RunwayCentrelinePoint> GetRunwayCentrelinePoint (Guid rwyDirIdentifier)
        {
            foreach (Feature feature in _featureList)
            {
                if (feature.FeatureType == Aran.Aim.FeatureType.RunwayCentrelinePoint)
                {
                    RunwayCentrelinePoint rcp = (RunwayCentrelinePoint) feature;
                    if (rcp.OnRunway.Identifier == rwyDirIdentifier)
                        yield return rcp;
                }
            }
        }

        private OrganisationAuthority CreateOrganisation ()
        {
            OrganisationAuthority org = new OrganisationAuthority ();
            org.Identifier = Guid.NewGuid ();
            org.Designator = "EV";
            org.Name = "LATVIA";
            return org;
        }

        private AirportHeliport CreateAirport (Guid orgIdentifier)
        {
            AirportHeliport ah = new AirportHeliport ();
            ah.Identifier = Guid.NewGuid ();
            ah.ResponsibleOrganisation = new AirportHeliportResponsibilityOrganisation ();
            ah.ResponsibleOrganisation.TheOrganisationAuthority = GetAsFeatureRef (orgIdentifier);

            ah.Designator = "EVRA";
            ah.Name = "RIGA";
            ah.FieldElevation = new ValDistanceVertical (34, UomDistanceVertical.FT);
            ah.Type = CodeAirportHeliport.AD;

            ah.ARP = GetElevatedPoint (23.9712, 56.923663889, 34, UomDistanceVertical.FT);
            return ah;
        }

        private Runway CreateRunway (Guid airportIdentifier)
        {
            Runway runway = new Runway ();
            runway.Identifier = Guid.NewGuid ();
            runway.AssociatedAirportHeliport = GetAsFeatureRef (airportIdentifier);

            runway.Designator = "18/36";
            runway.Type = CodeRunway.RWY;


            return runway;
        }

        private void InsertRunwayGroup (Guid runwayIdentifier)
        {
            #region RunwayDirection => 18

            RunwayDirection rwyDir1 = new RunwayDirection ();
            rwyDir1.Identifier = Guid.NewGuid ();
            rwyDir1.UsedRunway = GetAsFeatureRef (runwayIdentifier);
            rwyDir1.Designator = "18";
            _gdbProvider.SetFeature (rwyDir1);

            RunwayCentrelinePoint rcp = null;

            #region RunwayCentrelinePoint => START

            rcp = new RunwayCentrelinePoint ();
            rcp.Identifier = Guid.NewGuid ();
            rcp.OnRunway = GetAsFeatureRef (rwyDir1.Identifier);
            rcp.Role = CodeRunwayPointRole.START;
            rcp.Location = GetElevatedPoint (23.973075, 56.935069444, 31, UomDistanceVertical.FT);

            RunwayDeclaredDistance rdd = new RunwayDeclaredDistance ();
            rdd.Type = CodeDeclaredDistance.ASDA;
            RunwayDeclaredDistanceValue rddValue = new RunwayDeclaredDistanceValue ();
            rddValue.Distance = new ValDistance (3200, UomDistance.M);
            rdd.DeclaredValue.Add (rddValue);
            rcp.AssociatedDeclaredDistance.Add (rdd);

            rdd = new RunwayDeclaredDistance ();
            rdd.Type = CodeDeclaredDistance.TORA;
            rddValue = new RunwayDeclaredDistanceValue ();
            rddValue.Distance = new ValDistance (3200, UomDistance.M);
            rdd.DeclaredValue.Add (rddValue);
            rcp.AssociatedDeclaredDistance.Add (rdd);

            rdd = new RunwayDeclaredDistance ();
            rdd.Type = CodeDeclaredDistance.TODA;
            rddValue = new RunwayDeclaredDistanceValue ();
            rddValue.Distance = new ValDistance (3200, UomDistance.M);
            rdd.DeclaredValue.Add (rddValue);
            rcp.AssociatedDeclaredDistance.Add (rdd);

            rdd = new RunwayDeclaredDistance ();
            rdd.Type = CodeDeclaredDistance.LDA;
            rddValue = new RunwayDeclaredDistanceValue ();
            rddValue.Distance = new ValDistance (3200, UomDistance.M);
            rdd.DeclaredValue.Add (rddValue);
            rcp.AssociatedDeclaredDistance.Add (rdd);

            _gdbProvider.SetFeature (rcp);

            #endregion

            rcp = new RunwayCentrelinePoint ();
            rcp.Identifier = Guid.NewGuid ();
            rcp.OnRunway = GetAsFeatureRef (rwyDir1.Identifier);
            rcp.Role = CodeRunwayPointRole.THR;
            rcp.Location = GetElevatedPoint (23.973075, 56.935069444, 31, UomDistanceVertical.FT);
            _gdbProvider.SetFeature (rcp);

            rcp = new RunwayCentrelinePoint ();
            rcp.Identifier = Guid.NewGuid ();
            rcp.OnRunway = GetAsFeatureRef (rwyDir1.Identifier);
            rcp.Role = CodeRunwayPointRole.MID;
            rcp.Location = GetElevatedPoint (23.972338889, 56.930561111, 31, UomDistanceVertical.FT);
            _gdbProvider.SetFeature (rcp);

            rcp = new RunwayCentrelinePoint ();
            rcp.Identifier = Guid.NewGuid ();
            rcp.OnRunway = GetAsFeatureRef (rwyDir1.Identifier);
            rcp.Role = CodeRunwayPointRole.MID;
            rcp.Location = GetElevatedPoint (23.971605556, 56.926125, 31, UomDistanceVertical.FT);
            _gdbProvider.SetFeature (rcp);

            rcp = new RunwayCentrelinePoint ();
            rcp.Identifier = Guid.NewGuid ();
            rcp.OnRunway = GetAsFeatureRef (rwyDir1.Identifier);
            rcp.Role = CodeRunwayPointRole.MID;
            rcp.Location = GetElevatedPoint (23.9712, 56.923663889, 34, UomDistanceVertical.FT);
            _gdbProvider.SetFeature (rcp);

            rcp = new RunwayCentrelinePoint ();
            rcp.Identifier = Guid.NewGuid ();
            rcp.OnRunway = GetAsFeatureRef (rwyDir1.Identifier);
            rcp.Role = CodeRunwayPointRole.MID;
            rcp.Location = GetElevatedPoint (23.971125, 56.923205556, 31, UomDistanceVertical.FT);
            _gdbProvider.SetFeature (rcp);

            rcp = new RunwayCentrelinePoint ();
            rcp.Identifier = Guid.NewGuid ();
            rcp.OnRunway = GetAsFeatureRef (rwyDir1.Identifier);
            rcp.Role = CodeRunwayPointRole.MID;
            rcp.Location = GetElevatedPoint (23.970675, 56.920480556, 31, UomDistanceVertical.FT);
            _gdbProvider.SetFeature (rcp);

            rcp = new RunwayCentrelinePoint ();
            rcp.Identifier = Guid.NewGuid ();
            rcp.OnRunway = GetAsFeatureRef (rwyDir1.Identifier);
            rcp.Role = CodeRunwayPointRole.MID;
            rcp.Location = GetElevatedPoint (23.970236111, 56.917819444, 32, UomDistanceVertical.FT);
            _gdbProvider.SetFeature (rcp);

            rcp = new RunwayCentrelinePoint ();
            rcp.Identifier = Guid.NewGuid ();
            rcp.OnRunway = GetAsFeatureRef (rwyDir1.Identifier);
            rcp.Role = CodeRunwayPointRole.END;
            rcp.Location = GetElevatedPoint (23.968355556, 56.90645, 36, UomDistanceVertical.FT);
            _gdbProvider.SetFeature (rcp);

            #endregion

            #region RunwayDirection => 36

            RunwayDirection rwyDir2 = new RunwayDirection ();
            rwyDir2.Identifier = Guid.NewGuid ();
            rwyDir2.UsedRunway = GetAsFeatureRef (runwayIdentifier);
            rwyDir2.Designator = "36";

            _gdbProvider.SetFeature (rwyDir2);

            #region RunwayCentrelinePoint => START

            rcp = new RunwayCentrelinePoint ();
            rcp.Identifier = Guid.NewGuid ();
            rcp.OnRunway = GetAsFeatureRef (rwyDir1.Identifier);
            rcp.Role = CodeRunwayPointRole.START;
            rcp.Location = GetElevatedPoint (23.968355556, 56.90645, 36, UomDistanceVertical.FT);

            rdd = new RunwayDeclaredDistance ();
            rdd.Type = CodeDeclaredDistance.ASDA;
            rddValue = new RunwayDeclaredDistanceValue ();
            rddValue.Distance = new ValDistance (3200, UomDistance.M);
            rdd.DeclaredValue.Add (rddValue);
            rcp.AssociatedDeclaredDistance.Add (rdd);

            rdd = new RunwayDeclaredDistance ();
            rdd.Type = CodeDeclaredDistance.TORA;
            rddValue = new RunwayDeclaredDistanceValue ();
            rddValue.Distance = new ValDistance (3200, UomDistance.M);
            rdd.DeclaredValue.Add (rddValue);
            rcp.AssociatedDeclaredDistance.Add (rdd);

            rdd = new RunwayDeclaredDistance ();
            rdd.Type = CodeDeclaredDistance.TODA;
            rddValue = new RunwayDeclaredDistanceValue ();
            rddValue.Distance = new ValDistance (3200, UomDistance.M);
            rdd.DeclaredValue.Add (rddValue);
            rcp.AssociatedDeclaredDistance.Add (rdd);

            rdd = new RunwayDeclaredDistance ();
            rdd.Type = CodeDeclaredDistance.LDA;
            rddValue = new RunwayDeclaredDistanceValue ();
            rddValue.Distance = new ValDistance (3200, UomDistance.M);
            rdd.DeclaredValue.Add (rddValue);
            rcp.AssociatedDeclaredDistance.Add (rdd);

            _gdbProvider.SetFeature (rcp);

            #endregion

            rcp = new RunwayCentrelinePoint ();
            rcp.Identifier = Guid.NewGuid ();
            rcp.OnRunway = GetAsFeatureRef (rwyDir2.Identifier);
            rcp.Role = CodeRunwayPointRole.THR;
            rcp.Location = GetElevatedPoint (23.968355556, 56.90645, 36, UomDistanceVertical.FT);
            _gdbProvider.SetFeature (rcp);

            rcp = new RunwayCentrelinePoint ();
            rcp.Identifier = Guid.NewGuid ();
            rcp.OnRunway = GetAsFeatureRef (rwyDir2.Identifier);
            rcp.Role = CodeRunwayPointRole.MID;
            rcp.Location = GetElevatedPoint (23.970236111, 56.917819444, 32, UomDistanceVertical.FT);
            _gdbProvider.SetFeature (rcp);

            rcp = new RunwayCentrelinePoint ();
            rcp.Identifier = Guid.NewGuid ();
            rcp.OnRunway = GetAsFeatureRef (rwyDir2.Identifier);
            rcp.Role = CodeRunwayPointRole.MID;
            rcp.Location = GetElevatedPoint (23.970675, 56.920480556, 31, UomDistanceVertical.FT);
            _gdbProvider.SetFeature (rcp);

            rcp = new RunwayCentrelinePoint ();
            rcp.Identifier = Guid.NewGuid ();
            rcp.OnRunway = GetAsFeatureRef (rwyDir2.Identifier);
            rcp.Role = CodeRunwayPointRole.MID;
            rcp.Location = GetElevatedPoint (23.971125, 56.923205556, 31, UomDistanceVertical.FT);
            _gdbProvider.SetFeature (rcp);

            rcp = new RunwayCentrelinePoint ();
            rcp.Identifier = Guid.NewGuid ();
            rcp.OnRunway = GetAsFeatureRef (rwyDir2.Identifier);
            rcp.Role = CodeRunwayPointRole.MID;
            rcp.Location = GetElevatedPoint (23.9712, 56.923663889, 34, UomDistanceVertical.FT);
            _gdbProvider.SetFeature (rcp);

            rcp = new RunwayCentrelinePoint ();
            rcp.Identifier = Guid.NewGuid ();
            rcp.OnRunway = GetAsFeatureRef (rwyDir2.Identifier);
            rcp.Role = CodeRunwayPointRole.MID;
            rcp.Location = GetElevatedPoint (23.971605556, 56.926125, 31, UomDistanceVertical.FT);
            _gdbProvider.SetFeature (rcp);

            rcp = new RunwayCentrelinePoint ();
            rcp.Identifier = Guid.NewGuid ();
            rcp.OnRunway = GetAsFeatureRef (rwyDir2.Identifier);
            rcp.Role = CodeRunwayPointRole.MID;
            rcp.Location = GetElevatedPoint (23.972338889, 56.930561111, 31, UomDistanceVertical.FT);
            _gdbProvider.SetFeature (rcp);

            rcp = new RunwayCentrelinePoint ();
            rcp.Identifier = Guid.NewGuid ();
            rcp.OnRunway = GetAsFeatureRef (rwyDir2.Identifier);
            rcp.Role = CodeRunwayPointRole.END;
            rcp.Location = GetElevatedPoint (23.973075, 56.935069444, 31, UomDistanceVertical.FT);
            _gdbProvider.SetFeature (rcp);

            #endregion
        }

        private ElevatedPoint GetElevatedPoint (double x, double y, double z, UomDistanceVertical zUom)
        {
            ElevatedPoint ep = new ElevatedPoint ();
            ep.Geo.SetCoords (x, y);
            ep.Elevation = new ValDistanceVertical (z, zUom);
            return ep;
        }

        private FeatureRef GetAsFeatureRef (Guid identifier)
        {
            FeatureRef fr = new FeatureRef ();
            fr.Identifier = identifier;
            return fr;
        }

        private void SetFeature (Feature feature)
        {
            _featureList.Add (feature);
        }
    }
}
