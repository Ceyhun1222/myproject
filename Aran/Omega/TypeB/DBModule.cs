using System;
using Aran.Aim;
using Aran.Geometries;
using Aran.Queries.Omega;
using System.Collections.Generic;
using Aran.Aim.Features;
using Aran.Aim.Data.Filters;
using Aran.Aim.Data;
using Aran.Panda.Common;
using Aran.Aim.Enums;
using System.Linq;

namespace Aran.Omega.TypeB
{
    public class DbModule
    {
        public DbModule()
        {
            try
            {
                OmegaQPI = OmegaQpiFactory.Create();
                var dbProvider = GlobalParams.AranEnvironment.DbProvider as IDbProvider;
                OmegaQPI.Open(dbProvider);
                AirportHeliport = GetAirportHeliport();
                Runways = OmegaQPI.GetRunwayList(AirportHeliport.Identifier);
            }
            catch (Exception)
            {

                throw new Exception("Database error!");
            }
           
        }

        public List<Runway> Runways { get;private set; }
        
        public AirportHeliport AirportHeliport { get;private set; }

        public List<RunwayCentrelinePoint> RwyCenterLinePointList { get; private set; }

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
            //Ring circle = ARANFunctions.CreateCirclePrj(GlobalParams.SpatialRefOperation.ToPrj(ptCenter), distance);

            //MultiPolygon mlt = new MultiPolygon();
            //mlt.Add(new Polygon());
            //mlt[0].ExteriorRing = circle;

            //Func<VerticalStructurePart, bool> isInside = vsPart =>
            //{
            //    if (vsPart.HorizontalProjection.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
            //        return mlt.IsPointInside(GlobalParams.SpatialRefOperation.ToPrj(vsPart.HorizontalProjection.Location.Geo));

            //    else if (vsPart.HorizontalProjection.Choice == VerticalStructurePartGeometryChoice.ElevatedCurve)
            //    {
            //        Geometry intersect = GlobalParams.GlobalParams.GeomOperators(mlt,
            //            GlobalParams.SpatialRefOperation.ToPrj(vsPart.HorizontalProjection.LinearExtent.Geo));
            //        return intersect != null && !intersect.IsEmpty;
            //    }
            //    else if (vsPart.HorizontalProjection.Choice == VerticalStructurePartGeometryChoice.ElevatedSurface)
            //    {
            //        Geometry intersect = GlobalParams.GlobalParams.GeomOperators(mlt,
            //            GlobalParams.SpatialRefOperation.ToPrj(vsPart.HorizontalProjection.SurfaceExtent.Geo));
            //        return intersect != null && !intersect.IsEmpty;
            //    }
            //    return false;
            //};


            //    var vsList = (from vs in verticalStructureList
            //        from vsPart in vs.Part
            //        where isInside(vsPart)
            //        select vs).ToList<VerticalStructure>();

            //    return vsList;
        }

        public List<VerticalStructure> GetVerticalStructureList()
        {
            List<VerticalStructure> verticalStructureList = OmegaQPI.GetVerticalStructureList();
            return verticalStructureList;
        }

        public RunwayElement GetRunwayelement(Guid rwyIdentifier)
        {
            return OmegaQPI.GetRunwayElement(rwyIdentifier);
        }

// ReSharper disable once InconsistentNaming
        public IOmegaQPI OmegaQPI { get; private set; }
        
    }
}