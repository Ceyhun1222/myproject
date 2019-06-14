using System;
using Aran.Aim;
using Aran.Geometries;
using Aran.Queries.Omega;
using System.Collections.Generic;
using Aran.Aim.Features;
using Aran.Aim.Data;
using System.Threading.Tasks;

namespace Aran.Omega
{
    public class DbModule
    {
        public DbModule()
        {
            try
            {
                OmegaQPI = OmegaQpiFactory.Create();
                var dbProvider = GlobalParams.AranEnvironment.DbProvider as DbProvider;
                OmegaQPI.Open(dbProvider);
                Organisation = GetOrganisation();
                AirportHeliport = GetAirportHeliport();
                Runways = OmegaQPI.GetRunwayList(AirportHeliport.Identifier);
                ObstacleAreaList = OmegaQPI.GetObstacleAreaList();
                AirspaceList = OmegaQPI.GetAirspaceList();
            }
            catch (Exception e)
            {

                throw new Exception("Database error! "+e.Message);
            }
           
        }

        private OrganisationAuthority GetOrganisation()
        {
            return OmegaQPI.GetFeature(FeatureType.OrganisationAuthority,GlobalParams.Settings.OLSQuery.Organization) as OrganisationAuthority;
        }

        public List<Runway> Runways { get;private set; }

        public OrganisationAuthority Organisation { get; private set; }

        public AirportHeliport AirportHeliport { get;private set; }

        public List<RunwayCentrelinePoint> RwyCenterLinePointList { get; private set; }

        public List<Airspace> AirspaceList { get; set; }

        public List<RunwayDirection> GetRunwayDirections(Guid runwayGuid)
        {
            return OmegaQPI.GetRunwayDirectionList(runwayGuid);
        }

        private AirportHeliport GetAirportHeliport()
        {
            return OmegaQPI.GetAdhp(GlobalParams.Settings.OLSQuery.Aeroport);
        }

        public List<RunwayCentrelinePoint> GetRunwayCenterLinePoints(Guid rwyDirIdentifier)
        {
            List<RunwayCentrelinePoint> rwyCntrLinePtList = OmegaQPI.GetRunwayCentrelinePointList(rwyDirIdentifier);
            return rwyCntrLinePtList;
        }

        public List<VerticalStructure> GetVerticalStructureList(Aran.Geometries.MultiPolygon extent) 
        {
            List<VerticalStructure> verticalStructureList = OmegaQPI.GetVerticalStructureList(extent);
            return verticalStructureList;
        }

        public List<VerticalStructure> GetVerticalStructureList(Aran.Geometries.Point ptCenter,double distance)
        {
            List<VerticalStructure> verticalStructureList = OmegaQPI.GetVerticalStructureList(ptCenter,distance);
            return verticalStructureList;
        }

        public async Task<List<VerticalStructure>> GetVerticalStructureListAsync(Aran.Geometries.Point ptCenter, double distance)
        {
            var result =await Task.Factory.StartNew<List<VerticalStructure>>(() =>
            {
                return OmegaQPI.GetVerticalStructureList(ptCenter, distance);
            });
            return result;
        }


        public List<VerticalStructure> GetVerticalStructureList()
        {
            List<VerticalStructure> verticalStructureList = OmegaQPI.GetVerticalStructureList();
            return verticalStructureList;
        }

        public Geometry GetTma() 
        {
            try
            {
                var tmaList = OmegaQPI.GetTMAList();
                var adhpPrj = GlobalParams.SpatialRefOperation.ToPrj(AirportHeliport.ARP.Geo);
                
                foreach (var tma in tmaList)
                {
                    foreach (var geometryComponent in tma.GeometryComponent)
                    {
                        var geo = geometryComponent.TheAirspaceVolume.HorizontalProjection.Geo;
                        if (geo != null)
                        {
                            var geoPrj = GlobalParams.SpatialRefOperation.ToPrj(geo);
                            if (geoPrj.IsPointInside(adhpPrj))
                                return geoPrj;
                        }
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;

            }
        }

        public List<GuidanceLine> GetGuidanceLine()
        {
            return OmegaQPI.GetTaxiwayGuidanceLineList(AirportHeliport.Identifier);
        }

        public List<ApronElement> GetApronElementList() 
        {
            return OmegaQPI.GetApronElementList(AirportHeliport.Identifier);
        }

        public RunwayElement GetRunwayelement(Guid rwyIdentifier)
        {
            return OmegaQPI.GetRunwayElement(rwyIdentifier);
        }

        public List<ObstacleArea> ObstacleAreaList { get; set; }


// ReSharper disable once InconsistentNaming
        public IOmegaQPI OmegaQPI { get; private set; }
        
    }
}