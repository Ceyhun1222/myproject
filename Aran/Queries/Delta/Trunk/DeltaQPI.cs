using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim;

namespace Aran.Queries.DeltaQPI
{
    public static class DeltaQpiFactory
    {
        public static IDeltaQPI Create()
        {
            return new DeltaQPI();
        }
    }
    internal class DeltaQPI : CommonQPI,IDeltaQPI

    {
        public AirportHeliport GetAdhp(Guid identifier)
        {
            GettingResult getResult = DbProvider.GetVersionsOf(Aim.FeatureType.AirportHeliport, Interpretation, identifier);
            if (getResult.IsSucceed)
            {
                List<AirportHeliport> adhpList = getResult.GetListAs<AirportHeliport>();
                if (adhpList.Count > 0)
                    return adhpList[0];
                throw new Exception("Airport not found in database !");
            }
            throw new Exception(getResult.Message);
        }

        public List<DesignatedPoint> GetDesignatedPointList()
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.DesignatedPoint, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null);
            if (gettingResult.IsSucceed)
            {
                return gettingResult.GetListAs<DesignatedPoint>();
            }
            throw new Exception(gettingResult.Message);
        }

        public List<Aim.Features.DesignatedPoint> GetDesignatedPointList(Geometries.Point ptCenter, double radius)
        {
            BinaryLogicOp binaryLogic = new BinaryLogicOp();
            binaryLogic.Type = BinaryLogicOpType.And;

            DWithin dWithin = new DWithin();
            dWithin.Distance = new Aran.Aim.DataTypes.ValDistance(radius, UomDistance.M);
            dWithin.Point = ptCenter;
            dWithin.PropertyName = "location.geo"; ;
            OperationChoice operChoice = new OperationChoice(dWithin);
            binaryLogic.OperationList.Add(operChoice);

            ComparisonOps compOperDsg = new ComparisonOps(ComparisonOpType.NotNull, "Designator", null);
            OperationChoice operChoiceDsg = new OperationChoice(compOperDsg);
            binaryLogic.OperationList.Add(operChoiceDsg);

            OperationChoice opChoice = new OperationChoice(binaryLogic);
            Filter filter = new Filter(opChoice);
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.DesignatedPoint, Aran.Aim.Enums.TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, filter);

            if (gettingResult.IsSucceed)
            {
                return gettingResult.GetListAs<DesignatedPoint>();
            }

            throw new Exception(gettingResult.Message);
        }

        public List<Aim.Features.Navaid> GetNavaidListByTypes(params CodeNavaidService[] navaidServiceTypes)
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.Navaid, Aran.Aim.Enums.TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, null);
            if (gettingResult.IsSucceed)
            {
                var result = gettingResult.GetListAs<Navaid>();

                return (from navaid in result
                    where navaid.Type!=null && navaidServiceTypes.Contains(navaid.Type.Value) && navaid.Location != null
                    select navaid).ToList<Navaid>();
            }
            throw new Exception(gettingResult.Message);
        }

        public List<Aim.Features.Navaid> GetNavaidListByTypes(Geometries.Point centerGeo, double radius, params CodeNavaidService[] navaidServiceTypes)
        {
            DWithin dWithin = new DWithin();
            dWithin.Point = centerGeo;
            dWithin.PropertyName = "location.geo";
            dWithin.Distance = new Aran.Aim.DataTypes.ValDistance(radius, UomDistance.M);
            OperationChoice withinOperChoice = new OperationChoice(dWithin);
            Filter filter = new Filter(withinOperChoice);

            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.Navaid, Aran.Aim.Enums.TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, filter);
            if (gettingResult.IsSucceed)
            {
                var result = gettingResult.GetListAs<Navaid> ();

                return (from navaid in result
                        where navaidServiceTypes.Contains(navaid.Type.Value) && navaid.Location != null
                        select navaid).ToList<Navaid>();
            }
			throw new Exception ( gettingResult.Message );
        }

        public List<Route> GetRouteList()
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.Route, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null);
            if (gettingResult.IsSucceed)
            {
                return gettingResult.GetListAs<Route>();
            }
            throw new Exception(gettingResult.Message);
        }

        public List<Airspace> GetAirspaceList()
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.Airspace, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null);
            if (gettingResult.IsSucceed)
            {
                return gettingResult.GetListAs<Airspace>().OrderBy(airspace=>airspace.Designator).ToList();
            }
            throw new Exception(gettingResult.Message);
        }

        public List<RouteSegment> GetRouteSegmentList(Guid routeidentifier)
        {
            var compOper = new ComparisonOps(ComparisonOpType.EqualTo, "routeFormed", routeidentifier);
            var operChoice = new OperationChoice(compOper);
            var filter = new Filter(operChoice);
            var gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.RouteSegment, TimeSliceInterpretationType.BASELINE, default(Guid), false, null, null, filter);
            if (gettingResult.IsSucceed)
                return gettingResult.GetListAs<RouteSegment>();

            throw new Exception(gettingResult.Message);
        }

        public Airspace GetFir()
        {
            try
            {
                var compOper = new ComparisonOps(ComparisonOpType.EqualTo, "Type", CodeAirspace.FIR);
                var operChoice = new OperationChoice(compOper);
                var filter = new Filter(operChoice);
                var gettingResult = DbProvider.GetVersionsOf(FeatureType.Airspace, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, filter);
                if (gettingResult.IsSucceed)
                {
                    var airspaceList = gettingResult.GetListAs<Airspace>();
                    foreach (var airspace in airspaceList)
                    {
                        if (airspaceList.Count == 1)
                            return airspace;
                        if (airspace.Designator.ToUpper() == "BORDER")
                            return airspace;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception("Error Loading Airspace Data:" + e.Message);
            }
        }

        public List<RunwayCentrelinePoint> GetRunwayCenterlinePointList()
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.RunwayCentrelinePoint, TimeSliceInterpretationType.BASELINE, default(Guid), true);
            if (gettingResult.IsSucceed)
            {
                return gettingResult.GetListAs<RunwayCentrelinePoint>();
            }
            throw new Exception(gettingResult.Message);
        }

        public List<Navaid> GetNavaidList()
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.Navaid, Aran.Aim.Enums.TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, null);
            if (gettingResult.IsSucceed)
            {
                var result = gettingResult.GetListAs<Navaid>();

                return (from navaid in result
                    where navaid.Type != null && navaid.Location != null
                    select navaid).ToList();
            }
            throw new Exception(gettingResult.Message);
        }
    }
}
