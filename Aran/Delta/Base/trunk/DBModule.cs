using System;
using Aran.Aim;
using Aran.Geometries;
using Aran.Queries.DeltaQPI;
using System.Collections.Generic;
using Aran.Aim.Features;
using Aran.Aim.Data.Filters;
using Aran.Aim.Data;
using Aran.PANDA.Common;
using Aran.Aim.Enums;
using System.Linq;
using Aran.Delta.Model;
using NetTopologySuite.Features;

namespace Aran.Delta
{
    public class DbModule : IDBModule
    {
        private List<Route> _routeList;
        private readonly Settings.DeltaSettings _settings;
        private List<Airspace> _airspaceList;
        private List<RunwayCentrelinePoint> _runwayCntList;
        private AirportHeliport _airportHeliport;
        private List<DesignatedPoint> _designatedPointList;
        private List<Navaid> _navaidListByTypes;
        private List<Navaid> _navaidList;

        public DbModule(IDeltaQPI deltaQpi, Settings.DeltaSettings settings)
        {
            DeltaQPI = deltaQpi;
            _settings = settings;
        }

        public List<Route> RouteList
        {
            get
            {
                return _routeList ?? (_routeList = DeltaQPI.GetRouteList()
                           .OrderBy(route => route.Name)
                           .ToList<Route>());
            }
        }

        public List<RouteSegment> GetRouteSegmentList(Guid routeIdentifier)
        {
            return DeltaQPI.GetRouteSegmentList(routeIdentifier);
        }

        public AirportHeliport AirportHeliport => _airportHeliport ??(_airportHeliport= GetAirportHeliport());

        public List<DesignatedPoint> DesignatedPointList
        {
            get
            {
                return _designatedPointList ?? (_designatedPointList = DeltaQPI
                  .GetDesignatedPointList()
                  .OrderBy(dp => dp.Designator)
                  .ToList());
            }
        }

        public  List<Navaid> NavaidListByTypes
        {
            get
            {
                if (_navaidListByTypes==null)
                    _navaidListByTypes = DeltaQPI
                        .GetNavaidListByTypes(CodeNavaidService.DME, CodeNavaidService.VOR,
                            CodeNavaidService.TACAN, CodeNavaidService.VOR_DME, CodeNavaidService.NDB_DME,
                            CodeNavaidService.ILS_DME)
                        .OrderBy(nav => nav.Designator)
                        .ToList<Navaid>();
                return _navaidListByTypes;
            }
        }

        public List<Navaid> NavaidList
        {
            get
            {
                if (_navaidList == null)
                    _navaidList = DeltaQPI
                        .GetNavaidList()
                        .OrderBy(nav => nav.Designator)
                        .ToList();
                return _navaidList;
            }
        }

        public List<Airspace> GetAirspaceList
        {
            get
            {
                if (_airspaceList == null)
                    _airspaceList = DeltaQPI.GetAirspaceList();
                return _airspaceList;
            }
        }

        public string GetFeatureName(Aim.FeatureType featureType, Guid guid)
        {
            var feat = DeltaQPI.GetFeature(featureType, guid);
            if (feat != null && featureType == FeatureType.AirportHeliport)
            {
                var resultFeat = feat as AirportHeliport;
                if (resultFeat != null) return resultFeat.Designator;
            }
            if (feat != null && featureType == FeatureType.Navaid)
            {
                var resultFeat = feat as Navaid;
                if (resultFeat != null) return resultFeat.Designator;
            }

            if (feat != null && featureType == FeatureType.DesignatedPoint)
            {
                var resultFeat = feat as DesignatedPoint;
                if (resultFeat != null) return resultFeat.Designator;
            }
            if (feat != null && featureType == FeatureType.RunwayCentrelinePoint)
            {
                var resultFeat = feat as RunwayCentrelinePoint;
                if (resultFeat != null) return resultFeat.Designator;
            }
            return "";
        }

        private AirportHeliport GetAirportHeliport()
        {
            return DeltaQPI.GetAdhp(_settings.DeltaQuery.Aeroport);
        }

// ReSharper disable once InconsistentNaming
        public IDeltaQPI DeltaQPI { get; }

        public List<RunwayCentrelinePoint> RunwayCenterlineList
        {
            get
            {
                return _runwayCntList ?? (_runwayCntList = DeltaQPI
                           .GetRunwayCenterlinePointList()
                           .OrderBy(dp => dp.Designator)
                           .ToList<RunwayCentrelinePoint>());
            }
            
        }
    }
}