using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using System.Linq;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Geometries;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Enums;
using Aran.Aim.Data.Filters;
using Aran.Aim.Data;
using System.Collections;
using Aran.Aim.DataTypes;
using Aran.Queries;
using Aran.Aim.Data.InputRepository;

namespace Europe_ICAO015
{

    public static class OmegaQpiFactory
    {
        public static IOmegaQPI Create()
        {
            return new OmegaQPI();
        }
    }

    internal class OmegaQPI : Aran.Queries.CommonQPI, IOmegaQPI
    {
        public AirportHeliport GetAdhp(Guid identifier)
        {
            GettingResult getResult = DbProvider.GetVersionsOf(FeatureType.AirportHeliport, Interpretation, identifier);
            if (getResult.IsSucceed)
            {
                List<AirportHeliport> adhpList = getResult.GetListAs<AirportHeliport>();
                if (adhpList.Count > 0)
                    return adhpList[0];
                throw new Exception("Airport not found in database !");
            }
            throw new Exception(getResult.Message);
        }


        public List<Runway> GetRunwayList(Guid airportIdentifier)
        {
            //BinaryLogicOp binaryLogicOper = new BinaryLogicOp();
            //binaryLogicOper.Type = BinaryLogicOpType.And;

            var compOper = new ComparisonOps(ComparisonOpType.EqualTo, "associatedAirportHeliport", airportIdentifier);
            //binaryLogicOper.OperationList.Add(new OperationChoice(compOper));
            //ComparisonOps compDesignator = new ComparisonOps(ComparisonOpType.NotNull, "designator");
            //binaryLogicOper.OperationList.Add(new OperationChoice(compDesignator));
            //OperationChoice operChoice = new OperationChoice(binaryLogicOper);
            var operChoice = new OperationChoice(compOper);
            var filter = new Filter(operChoice);
            var gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.Runway, TimeSliceInterpretationType.BASELINE, default(Guid), false, null, null, filter);
            if (gettingResult.IsSucceed)
                return gettingResult.GetListAs<Runway>();

            throw new Exception(gettingResult.Message);
        }
        public RunwayElement GetRunwayElement(Guid rwyIdentifier)
        {
            ComparisonOps compOper = new ComparisonOps(ComparisonOpType.EqualTo, "associatedRunway", rwyIdentifier);
            OperationChoice operChoise = new OperationChoice(compOper);
            Filter filter = new Filter(operChoise);
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.RunwayElement, TimeSliceInterpretationType.BASELINE, default(Guid), false, null, null, filter);
            if (gettingResult.IsSucceed)
            {
                if (gettingResult.List.Count > 0)
                    return gettingResult.GetListAs<RunwayElement>()[0];
                return null;
            }
            return null;
        }

        public List<RunwayDirection> GetRunwayDirectionList(Guid runwayIdentifier)
        {
            ComparisonOps compOper = new ComparisonOps(ComparisonOpType.EqualTo, "usedRunway", runwayIdentifier);
            OperationChoice operChoise = new OperationChoice(compOper);
            Filter filter = new Filter(operChoise);
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.RunwayDirection, TimeSliceInterpretationType.BASELINE, default(Guid), false, null, null, filter);
            if (gettingResult.IsSucceed)
            {
                return gettingResult.GetListAs<RunwayDirection>();
            }
            return new List<RunwayDirection>();
        }

        public List<RunwayCentrelinePoint> GetRunwayCentrelinePointList(Guid rwyDirIdentifier)
        {

            ComparisonOps compOper = new ComparisonOps(ComparisonOpType.EqualTo, "onRunway", rwyDirIdentifier);
            OperationChoice operChoise = new OperationChoice(compOper);
            Filter filter = new Filter(operChoise);
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.RunwayCentrelinePoint, TimeSliceInterpretationType.BASELINE, default(Guid), false, null, null, filter);
            if (gettingResult.IsSucceed)
            {
                return gettingResult.GetListAs<RunwayCentrelinePoint>();
            }
            throw new Exception(gettingResult.Message);
        }
        public virtual Feature GetFeatureLst(FeatureType featureType, Guid identifier)
        {

            GettingResult gettingResult = DbProvider.GetVersionsOf(featureType, Interpretation, identifier, true, null, null, null);
            if (gettingResult.IsSucceed && gettingResult.List.Count > 0)
            {
                return gettingResult.List[0] as Feature;
            }
            return null;
        }
        public Aran.Geometries.MultiPolygon Geopoly { get; set; }
        public List<VerticalStructure> GetVerticalStructureList(MultiPolygon polygon)
        {
            var withinLocation = new Within
            {
                Geometry = polygon,
                PropertyName = "part.horizontalProjection.location.geo"

            };
            var operChoiceLocation = new OperationChoice(withinLocation);

            var withinSurface = new Within
            {
                Geometry = polygon,
                PropertyName = "part.horizontalProjection.surfaceExtent.geo"
            };
            var operChoiceSurface = new OperationChoice(withinSurface);

            var withinLinear = new Within
            {
                Geometry = polygon,
                PropertyName = "part.horizontalProjection.linearExtent.geo"
            };
            var operChoiceLinear = new OperationChoice(withinLinear);

            var logicOp = new BinaryLogicOp();
            logicOp.OperationList.Add(operChoiceLocation);
            logicOp.OperationList.Add(operChoiceSurface);
            logicOp.OperationList.Add(operChoiceLinear);
            logicOp.Type = BinaryLogicOpType.Or;

            var operChoice = new OperationChoice(logicOp);

            var filter = new Filter(operChoice);

            var gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.VerticalStructure, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, filter);

            if (gettingResult.IsSucceed)
            {
                return gettingResult.GetListAs<VerticalStructure>();
            }
            throw new Exception(gettingResult.Message);
        }

        public List<VerticalStructure> GetVerticalStructureList(Aran.Geometries.Point ptCenter, double distance)
        {
            var dwithin = new DWithin { Point = ptCenter, PropertyName = "part.horizontalProjection.location.geo" };
            ValDistance valDistance = new ValDistance(distance, UomDistance.M);
            dwithin.Distance = valDistance;
            var operChoicePt = new OperationChoice(dwithin);

            DWithin dWithin1 = new DWithin();
            dWithin1.Point = ptCenter;
            dWithin1.PropertyName = "part.horizontalProjection.linearExtent.geo";
            dWithin1.Distance = valDistance;
            var operChoiceCurve = new OperationChoice(dWithin1);

            DWithin dWithin2 = new DWithin();
            dWithin2.Point = ptCenter;
            dWithin2.PropertyName = "part.horizontalProjection.surfaceExtent.geo";
            dWithin2.Distance = valDistance;
            var operChoiceSurface = new OperationChoice(dWithin2);

            BinaryLogicOp logicOp = new BinaryLogicOp();
            logicOp.OperationList.Add(operChoicePt);
            logicOp.OperationList.Add(operChoiceCurve);
            logicOp.OperationList.Add(operChoiceSurface);
            logicOp.Type = BinaryLogicOpType.Or;

            OperationChoice mainOperChoice = new OperationChoice(logicOp);
            Filter filter = new Filter(mainOperChoice);

            GettingResult gettingResult;
            if (DbProvider.ProviderType == DbProviderType.ComSoft)
            {
                //All obstacles
                gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.VerticalStructure,
                    TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, null);
            }
            else
            {
                gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.VerticalStructure,
                    TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, filter);
            }

            //  GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.VerticalStructure, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, filter);

            //var result = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.VerticalStructure, TimeSliceInterpretationType.BASELINE, default(Guid), false, null, null);
            if (gettingResult.IsSucceed)
            {
                return gettingResult.GetListAs<VerticalStructure>();
            }
            throw new Exception(gettingResult.Message);
        }

        public List<Airspace> GetTMAList()
        {
            try
            {
                var compOper = new ComparisonOps(ComparisonOpType.EqualTo, "Type", CodeAirspace.TMA);
                var operChoice = new OperationChoice(compOper);
                var filter = new Filter(operChoice);
                var gettingResult = DbProvider.GetVersionsOf(FeatureType.Airspace, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, filter);
                if (gettingResult.IsSucceed)
                    return gettingResult.GetListAs<Airspace>();
                return null;
            }
            catch (Exception e)
            {
                throw new Exception("Error Loading Airspace Data:" + e.Message);
            }
        }

        public List<VerticalStructure> GetVerticalStructureList()
        {
            DWithin dWithin1 = new DWithin();
            //dWithin1.Point = ptCenter;
            dWithin1.PropertyName = "part.horizontalProjection.location.geo";
            //dWithin1.Distance = valDistance;
            var operChoiceCurve = new OperationChoice(dWithin1);
            //var mainOperChoice = new OperationChoice(logicOp);
            var filter = new Filter(operChoiceCurve);

            GettingResult gettingResult;
            if (DbProvider.ProviderType == DbProviderType.ComSoft)
            {
                //All obstacles
                gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.VerticalStructure,
                    TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, null);
            }
            else
            {
                gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.VerticalStructure,
                    TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, filter);
            }
            if (gettingResult.IsSucceed)
            {
                return gettingResult.GetListAs<VerticalStructure>();
            }
            throw new Exception(gettingResult.Message);

            //GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.VerticalStructure, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, filter);
            //if (gettingResult.IsSucceed)
            //{
            //    return gettingResult.GetListAs<VerticalStructure>();
            //}
            //throw new Exception(gettingResult.Message);
        }
        public List<ObstacleArea> GetObstacleArea()
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.ObstacleArea, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null);
            if (gettingResult.IsSucceed)
            {
                return gettingResult.GetListAs<ObstacleArea>();
            }
            throw new Exception(gettingResult.Message);
        }

        public List<ObstacleArea> GetObstacleAreaList()
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.ObstacleArea, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null);
            if (gettingResult.IsSucceed)
            {
                return gettingResult.GetListAs<ObstacleArea>();
            }
            throw new Exception(gettingResult.Message);
        }

        public List<Airspace> GetAirspaceList()
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.Airspace, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null);
            if (gettingResult.IsSucceed)
            {
                return gettingResult.GetListAs<Airspace>().OrderBy(airspace => airspace.Designator).ToList();
            }
            throw new Exception(gettingResult.Message);
        }
        public List<DME> GetDMEList()
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.DME, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null);
            if (gettingResult.IsSucceed)
            {
                return gettingResult.GetListAs<DME>().OrderBy(dme => dme.Name).ToList();
            }
            throw new Exception(gettingResult.Message);
        }
        public List<VOR> GetVORList()
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.VOR, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null);
            if (gettingResult.IsSucceed)
            {
                return gettingResult.GetListAs<VOR>().OrderBy(vor => vor.Name).ToList();
            }
            throw new Exception(gettingResult.Message);
        }
        public List<Navaid> GetNavaidList()
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.Navaid, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null);
            if (gettingResult.IsSucceed)
            {
                return gettingResult.GetListAs<Navaid>().OrderBy(nav => nav.Name).ToList();
            }
            throw new Exception(gettingResult.Message);
        }
        public List<Localizer> GetLocalizerList()
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.Localizer, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null);
            if (gettingResult.IsSucceed)
            {
                return gettingResult.GetListAs<Localizer>().OrderBy(loc => loc.Name).ToList();
            }
            throw new Exception(gettingResult.Message);
        }
        public List<Glidepath> GetGlidePathList()
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.Glidepath, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null);
            if (gettingResult.IsSucceed)
            {
                return gettingResult.GetListAs<Glidepath>().OrderBy(gp => gp.Name).ToList();
            }
            throw new Exception(gettingResult.Message);
        }
        public List<NDB> GetNDBList()
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.NDB, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null);
            if (gettingResult.IsSucceed)
            {
                return gettingResult.GetListAs<NDB>().OrderBy(ndb => ndb.Name).ToList();
            }
            throw new Exception(gettingResult.Message);
        }
        public List<MarkerBeacon> GetMArkersList()
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.MarkerBeacon, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null);
            if (gettingResult.IsSucceed)
            {
                return gettingResult.GetListAs<MarkerBeacon>().OrderBy(marker => marker.Name).ToList();
            }
            throw new Exception(gettingResult.Message);
        }

        private readonly IInputDataRepository _inputDataRepository;

        public List<Descriptor> GetRunwayListDescriptor(Guid airportidentifier)
        {
            ComparisonOps compOper = new ComparisonOps(ComparisonOpType.EqualTo, "associatedAirportHeliport", airportidentifier);
            OperationChoice opChoise = new OperationChoice(compOper);
            Filter filter = new Filter(opChoise);
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.Runway, TimeSliceInterpretationType.BASELINE, default(Guid), false, null, null, filter);
            List<Descriptor> runwayList = new List<Descriptor>();

            if (gettingResult.IsSucceed)
            {
                foreach (Runway rwy in gettingResult.List)
                    runwayList.Add(new Descriptor(rwy.Identifier, rwy.Designator));

                _inputDataRepository?.AddFeatures(gettingResult.GetListAs<Feature>());
                return runwayList;
            }
            return runwayList;
        }
    }
}
