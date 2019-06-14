using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;  
using Aran.Aim;
using Aran.Aim.CAWProvider;
using Aran.Aim.Features;
using Aran.Geometries;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Enums;

namespace Aran.Queries.Panda
{
	public static class PandaSQPIFactory
	{
		public static IPandaSpecializedQPI Create()
		{
			return new PandaSpecializedQPI();
		}
	}

    internal class PandaSpecializedQPI : CommonQPI, IPandaSpecializedQPI
    {
        public List<Descriptor> GetAirportHeliportList (Guid organisationIdentifier, bool checkILS)
        {
            LinkQuery lq = new LinkQuery ();
            lq.FeatureTypeList.Add (FeatureType.AirportHeliport);
            lq.TraverseTimeslicePropertyName = "responsibleOrganisation";

            lq.SimpleQuery = new SimpleQuery ();
            lq.SimpleQuery.FeatureType = FeatureType.OrganisationAuthority;
            lq.SimpleQuery.IdentifierList.Add (organisationIdentifier);
            lq.SimpleQuery.TemproalTimeslice = _timeSlice;
            lq.SimpleQuery.Interpretation = _interpretation;

            lq.SimpleQuery.SetProperties<PropertyAirportHeliport> (
                PropertyAirportHeliport.Designator);

            List<AirportHeliport> ahList = _cawService.GetFeature<AirportHeliport> (lq);

            return ahList.Select (
                ai => new Descriptor (ai.Identifier, ai.Designator)).ToList ();
        }

        public List<AirportHeliport> GetAirportHeliportList ( Guid organisationIdentifier )
        {
            LinkQuery lq = new LinkQuery ();
            lq.FeatureTypeList.Add ( FeatureType.AirportHeliport );
            lq.TraverseTimeslicePropertyName = "responsibleOrganisation";

            lq.SimpleQuery = new SimpleQuery ();
            lq.SimpleQuery.FeatureType = FeatureType.OrganisationAuthority;
            lq.SimpleQuery.IdentifierList.Add ( organisationIdentifier );
            lq.SimpleQuery.TemproalTimeslice = _timeSlice;
            lq.SimpleQuery.Interpretation = _interpretation;

            List<AirportHeliport> ahList = _cawService.GetFeature<AirportHeliport> ( lq );
            return ahList;
        }

        public List<Descriptor> GetRunwayList (Guid airportIdentifier)
        {
            LinkQuery lq = new LinkQuery ();
            lq.FeatureTypeList.Add (FeatureType.Runway);
            lq.TraverseTimeslicePropertyName = "associatedAirportHeliport";

            lq.SimpleQuery = new SimpleQuery ();
            lq.SimpleQuery.FeatureType = FeatureType.AirportHeliport;
            lq.SimpleQuery.IdentifierList.Add (airportIdentifier);
            lq.SimpleQuery.TemproalTimeslice = _timeSlice;
            lq.SimpleQuery.Interpretation = _interpretation;

            lq.SimpleQuery.SetProperties<PropertyRunway> (
                PropertyRunway.Designator);

            List<Runway> runwayList = _cawService.GetFeature<Runway> (lq);

            return runwayList.Select (
                rwy => new Descriptor (rwy.Identifier, rwy.Designator)).ToList ();
        }

        public List<RunwayDirection> GetRunwayDirectionList (Guid runwayIdentifier)
        {
            LinkQuery lq = new LinkQuery ();
            lq.FeatureTypeList.Add (FeatureType.RunwayDirection);
            lq.TraverseTimeslicePropertyName = "usedRunway";

            lq.SimpleQuery = new SimpleQuery ();
            lq.SimpleQuery.FeatureType = FeatureType.Runway;
            lq.SimpleQuery.IdentifierList.Add (runwayIdentifier);
            lq.SimpleQuery.TemproalTimeslice = _timeSlice;
            lq.SimpleQuery.Interpretation = _interpretation;

            return _cawService.GetFeature<RunwayDirection> (lq);
        }

		//===========================================
		public List<AngleIndication> GetAngleIndicationList(SignificantPoint pSignificantPoint)
		{
			Filter filter = new Filter();

			List<AngleIndication> result = GetFeatureList<AngleIndication>(filter);
			return result;
		}
		//===========================================

		public List<RunwayCentrelinePoint> GetRunwayCentrelinePointList(Guid rwyDirIdentifier)
        {
            LinkQuery lq = new LinkQuery ();
            lq.FeatureTypeList.Add (FeatureType.RunwayCentrelinePoint);
            lq.TraverseTimeslicePropertyName = "onRunway";

            lq.SimpleQuery = new SimpleQuery ();
            lq.SimpleQuery.FeatureType = FeatureType.RunwayDirection;
            lq.SimpleQuery.IdentifierList.Add (rwyDirIdentifier);
            lq.SimpleQuery.TemproalTimeslice = _timeSlice;
            lq.SimpleQuery.Interpretation = _interpretation;

            List <RunwayCentrelinePoint> list = _cawService.GetFeature<RunwayCentrelinePoint> (lq);

            for (int i = 0; i < list.Count; i++)
            {
                if (list [0].Identifier == Guid.Empty)
                {
                    list.RemoveAt (i);
                    i--;
                }
            }

            return list;
        }

        public List<RunwayProtectArea> GetRunwayProtectAreaList (Guid rwyDirIdentifier)
        {
            LinkQuery lq = new LinkQuery ();
            lq.FeatureTypeList.Add (FeatureType.RunwayProtectArea);
            lq.TraverseTimeslicePropertyName = "protectedRunwayDirection";

            lq.SimpleQuery = new SimpleQuery ();
            lq.SimpleQuery.FeatureType = FeatureType.RunwayDirection;
            lq.SimpleQuery.IdentifierList.Add (rwyDirIdentifier);
            lq.SimpleQuery.TemproalTimeslice = _timeSlice;
            lq.SimpleQuery.Interpretation = _interpretation;

            return _cawService.GetFeature<RunwayProtectArea> (lq);
        }

        public List<Navaid> GetNavaidList (MultiPolygon polygon)
        {
            BBox box = new BBox ();
            box.PropertyName = "location";
            box.Envelope = PolygonToBox (polygon);

            Filter filter = new Filter ();
            filter.Operation = new OperationChoice (box);

            return GetFeatureList<Navaid> (filter);
        }

        public Navaid GetILSNavaid (Guid rwyDirIdentifier)
        {
            LinkQuery lq = new LinkQuery ();
            lq.FeatureTypeList.Add (FeatureType.Navaid);
            lq.TraverseTimeslicePropertyName = "runwayDirection";

            lq.SimpleQuery = new SimpleQuery ();
            lq.SimpleQuery.FeatureType = FeatureType.RunwayDirection;
            lq.SimpleQuery.IdentifierList.Add (rwyDirIdentifier);
            lq.SimpleQuery.TemproalTimeslice = _timeSlice;
            lq.SimpleQuery.Interpretation = _interpretation;

            List<Navaid> navList = _cawService.GetFeature<Navaid> (lq);

            if (navList.Count == 0)
                return null;

            foreach (Navaid navaid in navList)
            {
                if (navaid.Type == CodeNavaidService.ILS ||
                    navaid.Type == CodeNavaidService.ILS_DME)
                {
                    return navaid;
                }
            }

            return null;
        }

        public List<VerticalStructure> GetVerticalStructureList (MultiPolygon polygon)
        {
            BBox box = new BBox ();
            box.PropertyName = "part";
            box.Envelope = PolygonToBox (polygon);

            Filter filter = new Filter ();
            filter.Operation = new OperationChoice (box);

            return GetFeatureList<VerticalStructure> (filter);
        }

        public List<DesignatedPoint> GetDesignatedPointList (MultiPolygon polygon)
        {
            DWithin within = new DWithin ();
            within.Geometry = polygon;
            within.PropertyName = "location";
            within.Distance = new Aim.DataTypes.ValDistance (0, Aim.Enums.UomDistance.M);

            Filter filter = new Filter ();
            filter.Operation = new OperationChoice (within);

            return GetFeatureList<DesignatedPoint> (filter);
        }

        public List<SafeAltitudeArea> GetSafeAltitudeAreaList (Guid navaidIdentifier)
        {
            LinkQuery lq = new LinkQuery ();
            lq.FeatureTypeList.Add (FeatureType.SafeAltitudeArea);
            lq.TraverseTimeslicePropertyName = "centrePoint_navaidSystem";

            lq.SimpleQuery = new SimpleQuery ();
            lq.SimpleQuery.FeatureType = FeatureType.Navaid;
            lq.SimpleQuery.IdentifierList.Add (navaidIdentifier);
            lq.SimpleQuery.TemproalTimeslice = _timeSlice;
            lq.SimpleQuery.Interpretation = _interpretation;

            return _cawService.GetFeature<SafeAltitudeArea> (lq);
        }

        public List<InstrumentApproachProcedure> GetIAPList (Guid airportIdentifier)
        {
            LinkQuery lq = new LinkQuery ();
            lq.FeatureTypeList.Add (FeatureType.InstrumentApproachProcedure);
            lq.TraverseTimeslicePropertyName = "airportHeliport";

            lq.SimpleQuery = new SimpleQuery ();
            lq.SimpleQuery.FeatureType = FeatureType.AirportHeliport;
            lq.SimpleQuery.IdentifierList.Add (airportIdentifier);
            lq.SimpleQuery.TemproalTimeslice = _timeSlice;
            lq.SimpleQuery.Interpretation = _interpretation;

            return _cawService.GetFeature<InstrumentApproachProcedure> (lq);
        }

        private Box PolygonToBox (MultiPolygon multiPolygon)
        {
            if (multiPolygon.Count == 0 ||
                multiPolygon [0].ExteriorRing.Count == 0)
            {
                return null;
            }
            Aran.Geometries.Point leftLowerCorner = (Aran.Geometries.Point)
                multiPolygon [0].ExteriorRing [0].Clone ();
            Aran.Geometries.Point rightUpperCorner = (Aran.Geometries.Point)
                multiPolygon [0].ExteriorRing [0].Clone ();

            foreach (Polygon polygon in multiPolygon)
            {
                foreach (Aran.Geometries.Point point in polygon.ExteriorRing)
                {
                    if (leftLowerCorner.X > point.X)
                        leftLowerCorner.X = point.X;

                    if (leftLowerCorner.Y > point.Y)
                        leftLowerCorner.Y = point.Y;

                    if (rightUpperCorner.X < point.X)
                        rightUpperCorner.X = point.X;

                    if (rightUpperCorner.Y < point.Y)
                        rightUpperCorner.Y = point.Y;
                }
            }

            Box box = new Box ();
            box [0] = leftLowerCorner;
            box [1] = rightUpperCorner;
            return box;
            
        }
    }
}
