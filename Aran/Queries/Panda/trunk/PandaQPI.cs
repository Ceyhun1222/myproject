#define VS_WITHOUT_TERRAIN

using System;
using System.Collections.Generic;
using System.IO;
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
using Aran.Queries;
using Aran.Aim.DataTypes;
using Aran.Aim.Data.InputRepository;

#if USE_AIM_STORAGE
using Aran.Aim.Data.Storage;
#endif

//using TestListAdapter;

namespace Aran.Queries.Panda_2
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
        private Dictionary<Guid, Feature> _srcFeatList;
        private Dictionary<Guid, NavaidEquipment> _allNavEquipmentDict;
        private Dictionary<Guid, SegmentLeg> _allSegmentLegDict;
        private readonly IInputDataRepository _inputDataRepository;


        public PandaSpecializedQPI()
        {
            _srcFeatList = new Dictionary<Guid, Feature>();
            _inputDataRepository = new InputDataRepository();
        }

        public PandaSpecializedQPI(IInputDataRepository inputRepository)
        {
            _srcFeatList = new Dictionary<Guid, Feature>();
            _inputDataRepository = inputRepository;
        }

        public override bool Commit(FeatureType[] featureTypes, bool showTimePanel)
        {
            if (!base.Commit(featureTypes, showTimePanel)) return false;
            var rootFeatures = GetRootFeatures();
            if (rootFeatures.Count > 0)
            {
                var feature = rootFeatures.First();
                if (feature.FeatureType == FeatureType.StandardInstrumentArrival
                    || feature.FeatureType == FeatureType.InstrumentApproachProcedure
                    || feature.FeatureType == FeatureType.StandardInstrumentDeparture)
                {
                    byte[] xml = _inputDataRepository.ToXml(this.DbProvider.DefaultEffectiveDate);
                    this.DbProvider.SetFeatureReport(new FeatureReport {Identifier = feature.Identifier, DateTime = DateTime.Now, ReportType = FeatureReportType.AIXM51, HtmlZipped = xml});
                }
            }
            return true;
        }

        public List<Descriptor> GetAirportHeliportList(Guid organisationIdentifier, bool checkILS)
        {
            Filter filter = null;
            if (organisationIdentifier != Guid.Empty)
            {
                ComparisonOps compOper1 = new ComparisonOps(ComparisonOpType.EqualTo,
                    "ResponsibleOrganisation.TheOrganisationAuthority",
                    organisationIdentifier);

                OperationChoice operChoice = new OperationChoice(compOper1);
                filter = new Filter(operChoice);
            }

            GettingResult gettingResult = DbProvider.GetVersionsOf(
                FeatureType.AirportHeliport,
                TimeSliceInterpretationType.BASELINE,
                default(Guid),
                true,
                null,
                null,
                filter);

            List<Descriptor> adhpList = new List<Descriptor>();

            if (gettingResult.IsSucceed)
            {
                foreach (AirportHeliport adhp in gettingResult.List)
                    adhpList.Add(new Descriptor(adhp.Identifier, adhp.Designator));

                _inputDataRepository?.AddFeatures(gettingResult.GetListAs<Feature>());
                return adhpList;
            }
            return adhpList;
        }

        public List<Route> GetRouteList(Guid orgId)
        {
            Filter filter = null;
            if (orgId != Guid.Empty)
            {
                ComparisonOps compOper1 = new ComparisonOps(ComparisonOpType.EqualTo,
                    "UserOrganisation",
                    orgId);

                OperationChoice operChoice = new OperationChoice(compOper1);
                filter = new Filter(operChoice);
            }

            GettingResult gettingResult = DbProvider.GetVersionsOf(
                FeatureType.Route,
                TimeSliceInterpretationType.BASELINE,
                default(Guid),
                true,
                null,
                null,
                filter);

            if (gettingResult.IsSucceed)
            {
                _inputDataRepository?.AddFeatures(gettingResult.GetListAs<Feature>());
                return gettingResult.GetListAs<Route>();
            }
            return new List<Route>();
        }

        public List<AirportHeliport> GetAirportHeliportList(Guid organisationIdentifier)
        {
            ComparisonOps compOper1 = new ComparisonOps(ComparisonOpType.EqualTo,
                "ResponsibleOrganisation.TheOrganisationAuthority",
                organisationIdentifier);

            OperationChoice operChoice = new OperationChoice(compOper1);
            Filter filter = new Filter(operChoice);

            GettingResult result = DbProvider.GetVersionsOf(
                FeatureType.AirportHeliport,
                TimeSliceInterpretationType.BASELINE,
                default(Guid), true, null, null, filter);

            List<Descriptor> adhpList = new List<Descriptor>();

            if (result.IsSucceed)
            {
                _inputDataRepository?.AddFeatures(result.GetListAs<Feature>());
                return result.GetListAs<AirportHeliport>();
            }
            return new List<AirportHeliport>();
        }

        public List<Descriptor> GetRunwayList(Guid airportIdentifier)
        {
            ComparisonOps compOper = new ComparisonOps(ComparisonOpType.EqualTo, "associatedAirportHeliport", airportIdentifier);
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

        public List<RunwayDirection> GetRunwayDirectionList(Guid runwayIdentifier)
        {
            ComparisonOps compOper = new ComparisonOps(ComparisonOpType.EqualTo, "usedRunway", runwayIdentifier);
            OperationChoice operChoise = new OperationChoice(compOper);
            Filter filter = new Filter(operChoise);
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.RunwayDirection, TimeSliceInterpretationType.BASELINE, default(Guid), false, null, null, filter);
            if (gettingResult.IsSucceed)
            {
                _inputDataRepository?.AddFeatures(gettingResult.GetListAs<Feature>());
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
                _inputDataRepository?.AddFeatures(gettingResult.GetListAs<Feature>());
                return gettingResult.GetListAs<RunwayCentrelinePoint>();
            }
            return new List<RunwayCentrelinePoint>();
        }

        public List<RunwayProtectArea> GetRunwayProtectAreaList(Guid rwyDirIdentifier)
        {
            ComparisonOps compOper = new ComparisonOps(ComparisonOpType.EqualTo, "protectedRunwayDirection", rwyDirIdentifier);
            OperationChoice operChoise = new OperationChoice(compOper);
            Filter filter = new Filter(operChoise);
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.RunwayProtectArea, TimeSliceInterpretationType.BASELINE, default(Guid), false, null, null, filter);
            if (gettingResult.IsSucceed)
            {
                _inputDataRepository?.AddFeatures(gettingResult.GetListAs<Feature>());
                return gettingResult.GetListAs<RunwayProtectArea>();
            }
            return new List<RunwayProtectArea>();
        }

        public List<Navaid> GetNavaidList(MultiPolygon polygon)
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(FeatureType.Navaid, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, null);

            if (gettingResult.IsSucceed)
            {
                var navEqDict = AllNavEquipmentDict;

                Func<Guid, bool> isInside = (navIdentifier) =>
                {
                    NavaidEquipment ne;
                    if (navEqDict.TryGetValue(navIdentifier, out ne))
                    {
                        if (ne.Location != null)
                            return polygon.IsPointInside(ne.Location.Geo);
                    }
                    return false;
                };


                var result = (from navaid in gettingResult.GetListAs<Navaid>()
                              where navaid.NavaidEquipment.Any(b => (b.TheNavaidEquipment != null && isInside(b.TheNavaidEquipment.Identifier)))
                              select navaid).ToList<Navaid>();

                _inputDataRepository?.AddFeatures(result.ToList<Feature>());
                return result;
            }

            throw new Exception(gettingResult.Message);
        }

        public Dictionary<Guid, NavaidEquipment> AllNavEquipmentDict
        {
            get
            {
                if (_allNavEquipmentDict == null)
                {
                    var gr = DbProvider.GetVersionsOf(AbstractFeatureType.NavaidEquipment, TimeSliceInterpretationType.BASELINE, true, null, null, null);

                    if (!gr.IsSucceed)
                        throw new Exception(gr.Message);

                    _allNavEquipmentDict = gr.GetListAs<NavaidEquipment>().ToDictionary(ne => ne.Identifier, ne => ne);
                }

                return _allNavEquipmentDict;
            }
        }

        public Dictionary<Guid, SegmentLeg> AllSegmentLegDict
        {
            get
            {
                if (_allSegmentLegDict == null)
                {
                    var gr = DbProvider.GetVersionsOf(AbstractFeatureType.SegmentLeg, TimeSliceInterpretationType.BASELINE, true, null, null, null);

                    if (!gr.IsSucceed)
                        throw new Exception(gr.Message);

                    _allSegmentLegDict = gr.GetListAs<SegmentLeg>().ToDictionary(sg => sg.Identifier, sg => sg);
                }

                return _allSegmentLegDict;
            }
        }

        public override Feature GetFeature(FeatureType featureType, Guid identifier)
        {
            var feat = base.GetFeature(featureType, identifier);

            if (feat != null)
                _inputDataRepository.AddFeature(feat);
            return feat;
        }

        public override Feature GetAbstractFeature(IAbstractFeatureRef abstractFeatureRef)
        {
            if (abstractFeatureRef is AbstractNavaidEquipmentRef)
            {
                NavaidEquipment ne;
                if (AllNavEquipmentDict.TryGetValue(abstractFeatureRef.Identifier, out ne))
                    return ne;
            }
            else if (abstractFeatureRef is AbstractSegmentLegRef)
            {
                SegmentLeg sl;
                if (AllSegmentLegDict.TryGetValue(abstractFeatureRef.Identifier, out sl))
                    return sl;
            }

            return base.GetAbstractFeature(abstractFeatureRef);
        }

        public override IList GetFeatureList(FeatureType featureType, Filter filter = null)
        {
            return base.GetFeatureList(featureType, filter);
        }


        public List<Navaid> GetNavaidList()
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(
                FeatureType.Navaid,
                TimeSliceInterpretationType.BASELINE,
                default(Guid), true, null, null, null);

            if (gettingResult.IsSucceed)
            {
                _inputDataRepository?.AddFeatures(gettingResult.GetListAs<Feature>());
                return gettingResult.GetListAs<Navaid>();
            }
            return new List<Navaid>();
        }

        public Navaid GetILSNavaid(Guid rwyDirIdentifier)
        {
            BinaryLogicOp binaryLogicOper = new BinaryLogicOp();
            binaryLogicOper.Type = BinaryLogicOpType.And;

            BinaryLogicOp typeBinOper = new BinaryLogicOp();
            typeBinOper.Type = BinaryLogicOpType.Or;

			ComparisonOps ilsCompOper = new ComparisonOps(ComparisonOpType.EqualTo, "type", CodeNavaidService.ILS);
			typeBinOper.OperationList.Add(new OperationChoice(ilsCompOper));

			ComparisonOps ilsDmeCompOps = new ComparisonOps(ComparisonOpType.EqualTo, "type", CodeNavaidService.ILS_DME);
            typeBinOper.OperationList.Add(new OperationChoice(ilsDmeCompOps));

			ComparisonOps locCompOps = new ComparisonOps(ComparisonOpType.EqualTo, "type", CodeNavaidService.LOC);
			typeBinOper.OperationList.Add(new OperationChoice(locCompOps));

			ComparisonOps locDmeCompOps = new ComparisonOps(ComparisonOpType.EqualTo, "type", CodeNavaidService.LOC_DME);
			typeBinOper.OperationList.Add(new OperationChoice(locDmeCompOps));


			binaryLogicOper.OperationList.Add(new OperationChoice(typeBinOper));


            ComparisonOps rwyDirCompOper = new ComparisonOps(ComparisonOpType.EqualTo, "runwayDirection", rwyDirIdentifier);
            binaryLogicOper.OperationList.Add(new OperationChoice(rwyDirCompOper));

            OperationChoice operChoice = new OperationChoice(binaryLogicOper);
            Filter filter = new Filter(operChoice);

            GettingResult gettingResult = DbProvider.GetVersionsOf(
                Aran.Aim.FeatureType.Navaid,
                TimeSliceInterpretationType.BASELINE,
                default(Guid), true, null, null, filter);

            if (gettingResult.IsSucceed)
            {
                if (gettingResult.List.Count > 0)
                {
                    var result = gettingResult.List[0] as Aran.Aim.Features.Navaid;
                    _inputDataRepository?.AddFeature(result);
                    return result;
                }
            }

            return null;
        }

        public List<VerticalStructure> GetVerticalStructureList(MultiPolygon polygon)
        {

            //var within = new Within ();
            //within.Geometry = polygon;
            //within.PropertyName = "part.horizontalProjection.location.geo";

            ////var co = new ComparisonOps (ComparisonOpType.NotLike, "Name", "Ter-");

            ////BinaryLogicOp blo = new BinaryLogicOp ();
            ////blo.Type = BinaryLogicOpType.And;
            ////blo.OperationList.Add (new OperationChoice (co));
            ////blo.OperationList.Add (new OperationChoice (within));

            ////OperationChoice operChoice = new OperationChoice(blo);
            ////var operChoice = new OperationChoice(co);

            ////var filter = new Filter(new OperationChoice(within));

            Filter filter = null;

            #region 2014.05.26

            if (DbProvider.ProviderType != DbProviderType.ComSoft)
            {

                var blo = new BinaryLogicOp();
                blo.Type = BinaryLogicOpType.Or;

                var within = new Within();
                within.Geometry = polygon;
                within.PropertyName = "part.horizontalProjection.location.geo";
                blo.OperationList.Add(new OperationChoice(within));

                within = new Within();
                within.Geometry = polygon;
                within.PropertyName = "part.horizontalProjection.linearExtent.geo";
                blo.OperationList.Add(new OperationChoice(within));

                within = new Within();
                within.Geometry = polygon;
                within.PropertyName = "part.horizontalProjection.surfaceExtent.geo";
                blo.OperationList.Add(new OperationChoice(within));

                filter = new Filter(new OperationChoice(blo));
            }

            #endregion

            var gettingResult = DbProvider.GetVersionsOf(
                FeatureType.VerticalStructure,
                TimeSliceInterpretationType.BASELINE,
                default(Guid),
                true,
                null,
                null,
                filter);

            if (!gettingResult.IsSucceed)
                throw new Exception(gettingResult.Message);

            var terrainDataList = DoTerrainDataReader(polygon);
            if (terrainDataList == null)
                return gettingResult.GetListAs<VerticalStructure>();


            var result = new List<VerticalStructure>();
            result.AddRange(gettingResult.GetListAs<VerticalStructure>());
            result.AddRange(terrainDataList);

            _inputDataRepository?.AddFeatures(result.ToList<Feature>());
            return result;

        }

        public List<VerticalStructure> GetVerticalStructureList()
        {

            var gettingResult = DbProvider.GetVersionsOf(
                FeatureType.VerticalStructure,
                TimeSliceInterpretationType.BASELINE,
                default(Guid),
                true,
                null,
                null,
                null);

            if (!gettingResult.IsSucceed)
                throw new Exception(gettingResult.Message);

            var vsList = gettingResult.GetListAs<VerticalStructure>();

            _inputDataRepository?.AddFeatures(vsList.ToList<Feature>());

            return vsList;
        }


        public List<VerticalStructure> GetVerticalStructureList(List<Guid> uuids)
        {
            var filter = Filter.CreateComparision(ComparisonOpType.In, "identifier", uuids);

            var gettingResult = DbProvider.GetVersionsOf(
                FeatureType.VerticalStructure,
                TimeSliceInterpretationType.BASELINE,
                default(Guid),
                true,
                null,
                null,
                filter);

            if (!gettingResult.IsSucceed)
                throw new Exception(gettingResult.Message);

            var vsList = gettingResult.GetListAs<VerticalStructure>();

            _inputDataRepository?.AddFeatures(vsList.ToList<Feature>());

            return vsList;
        }

        public List<DesignatedPoint> GetDesignatedPointList(MultiPolygon polygon)
        {
            Within within = new Within();
            within.Geometry = polygon;
            within.PropertyName = "location.geo";
            OperationChoice operChoice = new OperationChoice(within);
            Filter filter = new Filter(operChoice);
            GettingResult gettingResult = base.DbProvider.GetVersionsOf(Aran.Aim.FeatureType.DesignatedPoint, Aran.Aim.Enums.TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, filter);
            if (gettingResult.IsSucceed)
            {
                _inputDataRepository?.AddFeatures(gettingResult.GetListAs<Feature>());
                return gettingResult.GetListAs<DesignatedPoint>();
            }
            return new List<DesignatedPoint>();
        }

        public List<SafeAltitudeArea> GetSafeAltitudeAreaList(Guid navaidIdentifier)
        {
            ComparisonOps compOper = new ComparisonOps(ComparisonOpType.EqualTo, "centrePoint.navaidSystem", navaidIdentifier);
            OperationChoice operChoise = new OperationChoice(compOper);
            Filter filter = new Filter(operChoise);
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.SafeAltitudeArea, TimeSliceInterpretationType.BASELINE, default(Guid), false, null, null, filter);
            if (gettingResult.IsSucceed)
            {
                _inputDataRepository?.AddFeatures(gettingResult.GetListAs<Feature>());
                return gettingResult.GetListAs<SafeAltitudeArea>();
            }
            return new List<SafeAltitudeArea>();
        }

        public List<InstrumentApproachProcedure> GetIAPList(Guid airportIdentifier)
        {
            return GetIAPListPrivate(airportIdentifier, null);
            //ComparisonOps compOper = new ComparisonOps(ComparisonOpType.EqualTo, "airportHeliport", airportIdentifier);
            //OperationChoice operChoice = new OperationChoice(compOper);
            //Filter filter = new Filter(operChoice);
            //GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.InstrumentApproachProcedure, TimeSliceInterpretationType.BASELINE, default(Guid), false, null, null, filter);
            //if (gettingResult.IsSucceed)
            //{
            //    return gettingResult.GetListAs<InstrumentApproachProcedure> ();
            //}
            //return new List<InstrumentApproachProcedure>();
        }


        public List<InstrumentApproachProcedure> GetRnavIAPList(Guid airportIdentifier)
        {
            return GetIAPListPrivate(airportIdentifier, true);
        }

        public List<StandardInstrumentDeparture> GetSIDList(Guid airportIdentifier)
        {
            return GetProcedureList<StandardInstrumentDeparture>(airportIdentifier, null);
        }


        public List<StandardInstrumentDeparture> GetRNAVSIDList(Guid airportIdentifier)
        {
            return GetProcedureList<StandardInstrumentDeparture>(airportIdentifier, true);
        }

        public List<StandardInstrumentArrival> GetSTARList(Guid airportIdentifier)
        {
            return GetProcedureList<StandardInstrumentArrival>(airportIdentifier, true);
        }

        public List<StandardInstrumentArrival> GetRNAVSTARList(Guid airportIdentifier)
        {
            return GetProcedureList<StandardInstrumentArrival>(airportIdentifier, null);
        }


        private List<InstrumentApproachProcedure> GetIAPListPrivate(Guid airportIdentifier, bool? isRnav)
        {
            var operChoice = new OperationChoice(
                new ComparisonOps(ComparisonOpType.EqualTo, "airportHeliport", airportIdentifier));

            Filter filter = null;

            if (isRnav != null)
            {
                var blo = new BinaryLogicOp();
                blo.Type = BinaryLogicOpType.And;

                blo.OperationList.Add(operChoice);

                operChoice = new OperationChoice(
                    new ComparisonOps(ComparisonOpType.EqualTo, "RNAV", isRnav.Value));

                blo.OperationList.Add(operChoice);

                filter = new Filter(new OperationChoice(blo));
            }
            else
            {
                filter = new Filter(operChoice);
            }

            var gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.InstrumentApproachProcedure, TimeSliceInterpretationType.BASELINE, default(Guid), false, null, null, filter);
            if (gettingResult.IsSucceed)
            {
                _inputDataRepository?.AddFeatures(gettingResult.GetListAs<Feature>());

                return gettingResult.GetListAs<InstrumentApproachProcedure>();
            }
            return new List<InstrumentApproachProcedure>();
        }


        public List<T> GetProcedureList<T>(Guid airportIdentifier, bool? isRnav) where T : Procedure
        {
            var operChoice = new OperationChoice(
                new ComparisonOps(ComparisonOpType.EqualTo, "airportHeliport", airportIdentifier));

            Filter filter = null;

            if (isRnav != null)
            {
                var blo = new BinaryLogicOp();
                blo.Type = BinaryLogicOpType.And;

                blo.OperationList.Add(operChoice);

                operChoice = new OperationChoice(
                    new ComparisonOps(ComparisonOpType.EqualTo, "RNAV", isRnav.Value));

                blo.OperationList.Add(operChoice);

                filter = new Filter(new OperationChoice(blo));
            }
            else
            {
                filter = new Filter(operChoice);
            }

            var gettingResult = DbProvider.GetVersionsOf((FeatureType)Enum.Parse(typeof(FeatureType), typeof(T).Name), TimeSliceInterpretationType.BASELINE, default(Guid), false, null, null, filter);
            if (gettingResult.IsSucceed)
            {
                var list = gettingResult.GetListAs<T>();
                _inputDataRepository?.AddFeatures(list.Cast<Feature>().ToList());
                return list;
            }
            return new List<T>();
        }

        public List<VerticalStructure> GetVerticalStructureList(Aran.Geometries.Point ptCenter, double distance)
        {
            var dwithin = new DWithin { Point = ptCenter, PropertyName = "part.horizontalProjection.location.geo" };
            var valDistance = new ValDistance(distance, UomDistance.M);
            dwithin.Distance = valDistance;
            var operChoicePt = new OperationChoice(dwithin);

            var dWithin1 = new DWithin();
            dWithin1.Point = ptCenter;
            dWithin1.PropertyName = "part.horizontalProjection.linearExtent.geo";
            dWithin1.Distance = valDistance;
            var operChoiceCurve = new OperationChoice(dWithin1);

            var dWithin2 = new DWithin();
            dWithin2.Point = ptCenter;
            dWithin2.PropertyName = "part.horizontalProjection.surfaceExtent.geo";
            dWithin2.Distance = valDistance;
            var operChoiceSurface = new OperationChoice(dWithin2);

            var logicOp = new BinaryLogicOp();
            logicOp.OperationList.Add(operChoicePt);
            logicOp.OperationList.Add(operChoiceCurve);
            logicOp.OperationList.Add(operChoiceSurface);
            logicOp.Type = BinaryLogicOpType.Or;

            var mainOperChoice = new OperationChoice(logicOp);
            var filter = new Filter(mainOperChoice);

            var gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.VerticalStructure, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, filter);

            if (!gettingResult.IsSucceed)
                throw new Exception(gettingResult.Message);

            var terrainDataList = DoTerrainDataReader(ptCenter, distance);
            if (terrainDataList == null)
                return gettingResult.GetListAs<VerticalStructure>();

            var result = new List<VerticalStructure>();
            result.AddRange(gettingResult.GetListAs<VerticalStructure>());
            result.AddRange(terrainDataList);

            _inputDataRepository?.AddFeatures(result.ToList<Feature>());
            return result;
        }

        public void AddToSrcLocalStorage(Feature feature)
        {
#if USE_AIM_STORAGE
            if (_srcFeatList.ContainsKey(feature.Identifier))
                return;

            _srcFeatList.Add(feature.Identifier, feature);
#endif
        }

        public void AddToSrcLocalStorage(IEnumerable<Feature> features)
        {
#if USE_AIM_STORAGE
            foreach (var item in features)
                AddToSrcLocalStorage(item);
#endif
        }

        public void AddCreatedRefToSrcLocalStorage()
        {
#if USE_AIM_STORAGE
            foreach (var feat in _createdFeatureList.Values) {
                AddToSrcLocalStorage(feat);

                var refFeaturePropList = new List<Aran.Aim.Utilities.RefFeatureProp>();
                Aran.Aim.Utilities.AimMetadataUtility.GetReferencesFeatures(feat, refFeaturePropList);

                if (refFeaturePropList.Count > 0) {
                    foreach (var refFeatProp in refFeaturePropList) {
                        if (!_createdFeatureList.ContainsKey(refFeatProp.RefIdentifier)) {
                            var refFeature = GetFeature(refFeatProp.FeatureType, refFeatProp.RefIdentifier);
                            AddToSrcLocalStorage(refFeature);
                        }
                        
                    }
                }
            }
#endif
        }

        public void SaveSrcLocalStorage(string fileName)
        {
#if USE_AIM_STORAGE
            var storage = new AimStorage();
            storage.Open(fileName);

            foreach (var feat in _srcFeatList.Values) {
                storage.Insert(feat);
            }

            storage.Close();
#endif
        }

        public List<VerticalStructure> GetAnnexVerticalStructureList(Guid airportIdentifier)
        {
            var obstacleAreaList = GetObstacleAreaList();
            if (obstacleAreaList == null) return null;

            var adhp = GetFeature<AirportHeliport>(airportIdentifier);
            var runwayList = GetRunwayList(airportIdentifier);
            var rwyDirList = new List<RunwayDirection>();

            if (runwayList == null) throw new Exception("Runways are empty");

            runwayList.ForEach(runway => rwyDirList.AddRange(GetRunwayDirectionList(runway.Identifier)));

            var obstacleRefList = new HashSet<Guid>();
            foreach (var obstacleArea in obstacleAreaList)
            {
                if (obstacleArea.Reference.Choice == ObstacleAreaOriginChoice.AirportHeliport
                    && obstacleArea.Reference.OwnerAirport?.Identifier == airportIdentifier)
                {
                    obstacleArea.Obstacle.ForEach(obs =>
                    {
                        if (!obstacleRefList.Contains(obs.Feature.Identifier))
                            obstacleRefList.Add(obs.Feature.Identifier);
                    });

                    _inputDataRepository.AddFeature(obstacleArea);
                }
                else if (obstacleArea.Reference.Choice == ObstacleAreaOriginChoice.RunwayDirection
                    && obstacleArea.Reference.OwnerRunway != null)
                {
                    if (rwyDirList.Find(rwyDir => rwyDir.Identifier == obstacleArea.Reference.OwnerRunway.Identifier) != null)
                        obstacleArea.Obstacle.ForEach(obs =>
                        {
                            if (!obstacleRefList.Contains(obs.Feature.Identifier))
                                obstacleRefList.Add(obs.Feature.Identifier);
                        });
                    _inputDataRepository.AddFeature(obstacleArea);
                }
                else if (obstacleArea.Reference.Choice == ObstacleAreaOriginChoice.OrganisationAuthority
                    && obstacleArea.Reference.OwnerOrganisation != null)
                {
                    if (adhp.ResponsibleOrganisation.TheOrganisationAuthority.Identifier == obstacleArea.Reference.OwnerOrganisation.Identifier)
                        obstacleArea.Obstacle.ForEach(obs =>
                        {
                            if (!obstacleRefList.Contains(obs.Feature.Identifier))
                                obstacleRefList.Add(obs.Feature.Identifier);
                        });
                    _inputDataRepository.AddFeature(obstacleArea);
                }
            }

            var logicOp = new BinaryLogicOp();
            logicOp.Type = BinaryLogicOpType.Or;

            ComparisonOps compOper = new ComparisonOps(ComparisonOpType.In, "Identifier", obstacleRefList.ToList());
            var operChoice = new OperationChoice(compOper);
            logicOp.OperationList.Add(operChoice);

            var mainOperChoice = new OperationChoice(logicOp);
            var filter = new Filter(mainOperChoice);

            var vsList = GetFeatureList<Aran.Aim.Features.VerticalStructure>(filter);

            //var allVvList = GetVerticalStructureList();

            //var joinList = from vs in allVvList
            //           join refObs in obstacleRefList on vs.Identifier equals refObs
            //           select vs;

            _inputDataRepository?.AddFeatures(vsList.ToList<Feature>());
            return vsList;
        }

        private List<ObstacleArea> GetObstacleAreaList()
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.ObstacleArea, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null);
            if (gettingResult.IsSucceed)
            {
                var result = new List<ObstacleArea>();

                foreach (ObstacleArea obsArea in gettingResult.List)
                {
                    if (obsArea == null || !obsArea.Type.HasValue)
                        continue;

                    if (obsArea.Type == CodeObstacleArea.OTHER_AREA2B ||
                        obsArea.Type == CodeObstacleArea.OTHER_AREA2C ||
                        obsArea.Type == CodeObstacleArea.OTHER_AREA2D ||
                        obsArea.Type == CodeObstacleArea.AREA2 ||
                        obsArea.Type == CodeObstacleArea.AREA3 ||
                        obsArea.Type == CodeObstacleArea.AREA4 ||
                        obsArea.Type == CodeObstacleArea.FAR77 ||
                        obsArea.Type == CodeObstacleArea.MANAGED)
                        continue;

                    result.Add(obsArea);
                }

                return result;
            }

            throw new Exception(gettingResult.Message);
        }

        public bool SaveAsXml(string fileName)
        {
            _inputDataRepository.ToXml(this.DbProvider.DefaultEffectiveDate, fileName);
            return true;
        }
    }
}