using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ChartCompare;
using EnrouteChartCompare.Model;
using ESRI.ArcGIS.Geometry;
using PDM;

namespace EnrouteChartCompare.Helper
{
    public class EnrouteComparer
    {
        private List<PDMObject> _currAllDsgPnts;
        private List<PDMObject> _currAllNavaids;
        private List<PDMObject> _oldAllDsgPnts;
        private List<PDMObject> _oldAllNavaids;
        private readonly List<DetailedItem> _enrouteDetails;
        private readonly List<DetailedItem> _routeSegDetails;
        private readonly List<DetailedItem> _airspaceDetails;
        private readonly List<DetailedItem> _airportDeTails;
        private readonly List<DetailedItem> _navaidDetails;
        private readonly List<DetailedItem> _wayPointDetails;
        private readonly List<DetailedItem> _holdingDetails;
        private List<DetectFeatureType> _featureTypeList;
        private readonly List<PDMObject> _currentObjList;
        private readonly List<PDMObject> _oldObjList;
        private readonly double _tolerance = 0.001;
        private readonly CODE_ROUTE_SEGMENT_CODE_LVL _routeLevel;

       public EnrouteComparer(List<PDMObject> currentObjList, List<PDMObject> oldObjList, CODE_ROUTE_SEGMENT_CODE_LVL routeLevel)
        {
            _currentObjList = currentObjList;
            _oldObjList = oldObjList;
            _routeLevel = routeLevel;
            _enrouteDetails = new List<DetailedItem>();
            _routeSegDetails = new List<DetailedItem>();
            _airspaceDetails = new List<DetailedItem>();
            _navaidDetails = new List<DetailedItem>();
            _wayPointDetails = new List<DetailedItem>();
            _holdingDetails = new List<DetailedItem>();
            _airportDeTails = new List<DetailedItem>();
        }

        public List<DetectFeatureType> FeatureTypeList => _featureTypeList ?? (_featureTypeList = Compare());

        //public string ErrorMessage { get; set; }

        private List<DetectFeatureType> Compare()
        {
            //ErrorMessage = "";
            try
            {
                _currAllDsgPnts = _currentObjList.Where(pdm => pdm is WayPoint).ToList();
                _currAllNavaids = _currentObjList.Where(pdm => pdm is NavaidSystem).ToList();

                _oldAllDsgPnts = _oldObjList.Where(pdm => pdm is WayPoint).ToList();
                _oldAllNavaids = _oldObjList.Where(pdm => pdm is NavaidSystem).ToList();

                //var oldRouteList = _oldObjList.Where(pdm => pdm.PDM_Type == PDM_ENUM.AirportHeliport).ToList();
                //var newRouteList = _currentObjList.Where(pdm => pdm.PDM_Type == PDM_ENUM.AirportHeliport).ToList();

                Check4NewAndChangedItems(_currentObjList, _oldObjList);
                Check4DeletedItems(_currentObjList, _oldObjList);

                return new List<DetectFeatureType>
                {
                    new DetectFeatureType("Enroute", _enrouteDetails, PDM_ENUM.Enroute),
                    new DetectFeatureType("Route Segment", _routeSegDetails, PDM_ENUM.RouteSegment),
                    new DetectFeatureType("Airspace", _airspaceDetails, PDM_ENUM.Airspace),
                    new DetectFeatureType("Navaid", _navaidDetails, PDM_ENUM.NavaidSystem),
                    new DetectFeatureType("Way Point", _wayPointDetails, PDM_ENUM.WayPoint),
                    new DetectFeatureType("Holding", _holdingDetails, PDM_ENUM.HoldingPattern),
                    new DetectFeatureType("AirportHeliport", _airportDeTails, PDM_ENUM.AirportHeliport)
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<DetailedItem> ResultList => _featureTypeList.SelectMany(t => t.FeatureList).ToList();

        private void Check4NewAndChangedItems(List<PDMObject> currentObjList, List<PDMObject> oldObjList)
        {
            foreach (var currObj in currentObjList)
            {
                switch (currObj.PDM_Type)
                {
                    case PDM_ENUM.Enroute:
                        CompareEnroute(oldObjList, currObj);
                        break;

                    case PDM_ENUM.Airspace:
                        CompareAirspace(oldObjList, currObj);
                        break;

                    case PDM_ENUM.HoldingPattern:
                        CompareHolding(oldObjList, currObj);
                        break;
                    case PDM_ENUM.AirportHeliport:
                        CompareAirport(oldObjList, currObj);
                        break;
                    case PDM_ENUM.NavaidSystem:
                        CompareNavaids(oldObjList, currObj as NavaidSystem);
                        break;
                }
            }
        }

        private void CompareNavaids(List<PDMObject> oldObjList, NavaidSystem currNavaid)
        {
            try
            {
                DetailedItem detItem = null;
                List<FieldLog> logList = new List<FieldLog>();
                if (!(oldObjList.FirstOrDefault(pdm => pdm.ID == currNavaid.ID) is NavaidSystem oldNavaid))
                {
                    var str = currNavaid.Name;
                    if (string.IsNullOrWhiteSpace(str))
                        str = currNavaid.Designator;
                    _navaidDetails.Add(new DetailedItem(currNavaid, str, Status.New));
                    return;
                }

                if (currNavaid.CodeNavaidSystemType != oldNavaid.CodeNavaidSystemType)
                {
                    logList.Add(new FieldLog(nameof(currNavaid.CodeNavaidSystemType),
                        oldNavaid.CodeNavaidSystemType.ToString(),
                        currNavaid.CodeNavaidSystemType.ToString()));
                }

                if (currNavaid.Designator != oldNavaid.Designator)
                {
                    logList.Add(new FieldLog(nameof(currNavaid.Designator), oldNavaid.Designator,
                        currNavaid.Designator));
                }

                if (currNavaid.Name != oldNavaid.Name)
                {
                    logList.Add(new FieldLog(nameof(currNavaid.Name), oldNavaid.Name, currNavaid.Name));
                }

                if (currNavaid.Lat != oldNavaid.Lat || currNavaid.Lon != oldNavaid.Lon)
                {
                    logList.Add(new FieldLog("Geometry",
                        oldNavaid.Lon + " " + oldNavaid.Lat, currNavaid.Lon + " " + currNavaid.Lat)
                    {
                        OldGeometry = oldNavaid.Geo
                    });
                }

                CompareNullableProp(currNavaid.Elev, oldNavaid.Elev, nameof(currNavaid.Elev), logList);

                if (currNavaid.Elev_UOM != oldNavaid.Elev_UOM)
                {
                    logList.Add(new FieldLog(nameof(currNavaid.Name), oldNavaid.Name, currNavaid.Name));
                }

                CheckNavaidComponents(currNavaid, oldNavaid, logList);

                if (logList.Count > 0)
                    _navaidDetails.Add(
                        new DetailedItem(currNavaid, currNavaid.Designator, Status.Changed)
                        {
                            FieldLogList = logList
                        });

            }

            catch (Exception ex)
            {
                throw new Exception("Airspace " + ex.Message);
            }
        }

        private void CheckNavaidComponents(NavaidSystem currNavaid, NavaidSystem navaidSystem, List<FieldLog> logList)
        {
            var vor = currNavaid.Components.FirstOrDefault(comp => comp.PDM_Type == PDM_ENUM.VOR) as VOR;
            var dme = currNavaid.Components.FirstOrDefault(comp => comp.PDM_Type == PDM_ENUM.DME) as DME;
            var oldVor = navaidSystem.Components.FirstOrDefault(comp => comp.PDM_Type == PDM_ENUM.VOR) as VOR;
            var oldDme = navaidSystem.Components.FirstOrDefault(comp => comp.PDM_Type == PDM_ENUM.DME) as DME;


            if (vor == null ^ oldVor == null)
            {
                logList.Add(new FieldLog(nameof(VOR), oldVor?.GetObjectLabel(), vor?.GetObjectLabel()));
            }
            else
            {
                if (vor != null)
                {
                    CompareNullableProp(vor.Frequency, oldVor.Frequency, nameof(vor.Frequency), logList);
                    if (vor.Frequency_UOM != oldVor.Frequency_UOM)
                    {
                        logList.Add(new FieldLog(nameof(vor.Frequency_UOM), oldVor.Frequency_UOM.ToString(),
                            vor.Frequency_UOM.ToString()));
                    }

                    if (vor.Lat != oldVor.Lat || vor.Lon != oldVor.Lon)
                    {
                        logList.Add(new FieldLog("VOR_Geometry",
                            oldVor.Lon + " " + oldVor.Lat, vor.Lon + " " + vor.Lat)
                        {
                            OldGeometry = oldVor.Geo
                        });
                    }
                }
            }

            if (dme == null ^ oldDme == null)
            {
                logList.Add(new FieldLog(nameof(DME), oldDme?.GetObjectLabel(), dme?.GetObjectLabel()));
            }
            else
            {
                if (dme != null)
                {
                    CompareNullableProp(dme.Elev, oldDme.Elev, nameof(dme.Elev), logList);
                    if (dme.Elev_UOM != oldDme.Elev_UOM)
                    {
                        logList.Add(new FieldLog(nameof(dme.Elev_UOM), oldDme.Elev_UOM.ToString(),
                            dme.Elev_UOM.ToString()));
                    }

                    if (dme.Lat != oldDme.Lat || dme.Lon != oldDme.Lon)
                    {
                        logList.Add(new FieldLog("DME_Geometry",
                            oldDme.Lon + " " + oldDme.Lat, dme.Lon + " " + dme.Lat)
                        {
                            OldGeometry = oldDme.Geo
                        });
                    }

                    if (dme.Channel != oldDme.Channel)
                        logList.Add(new FieldLog(nameof(dme.Channel), oldDme.Channel, dme.Channel));
                }
            }
        }

        private void CompareAirport(List<PDMObject> oldObjList, PDMObject currObj)
        {
            try
            {
                var airport = (AirportHeliport) currObj;
                if (!(oldObjList.FirstOrDefault(pdm => pdm.ID == airport.ID) is AirportHeliport oldObj))
                {
                    var str = airport.Name;
                    if (string.IsNullOrWhiteSpace(str))
                        str = airport.Designator;
                    if (string.IsNullOrWhiteSpace(str))
                        str = airport.DesignatorIATA;
                    _airportDeTails.Add(new DetailedItem(airport, str, Status.New));
                    return;
                }

                List<FieldLog> logList = new List<FieldLog>();
                if (airport.Name != oldObj.Name)
                {
                    logList.Add(new FieldLog("Name", oldObj.Name, airport.Name));
                }

                if (airport.Lat != oldObj.Lat || airport.Lon != oldObj.Lon)
                {
                    logList.Add(new FieldLog("Geometry",
                        oldObj.Lon + " " + oldObj.Lat, airport.Lon + " " + airport.Lat)
                    {
                        OldGeometry = oldObj.Geo
                    });
                }

                if (logList.Count > 0)
                    _airportDeTails.Add(new DetailedItem(airport, airport.Designator, Status.Changed)
                    {
                        FieldLogList = logList
                    });
            }
            catch (Exception ex)
            {
                throw new Exception("Airspace " + ex.Message);
            }
        }

        private void Check4DeletedItems(List<PDMObject> currentObjList, List<PDMObject> oldObjList)
        {
            foreach (var oldObj in oldObjList)
            {
                var pdmObj = currentObjList.FirstOrDefault(pdm => pdm.ID == oldObj.ID);
                switch (oldObj.PDM_Type)
                {
                    case PDM_ENUM.Enroute:
                        if (pdmObj == null || pdmObj.PDM_Type != PDM_ENUM.Enroute)
                        // || ShouldCheckEnroute(pdmObj as Enroute))
                        {
                            _enrouteDetails.Add(new DetailedItem(oldObj, (oldObj as Enroute)?.TxtDesig,
                                Status.Deleted));
                        }
                        else
                        {
                            if (((Enroute) pdmObj).Routes.All(rg => rg.CodeLvl != _routeLevel))
                            {
                                _enrouteDetails.Add(new DetailedItem(oldObj, (oldObj as Enroute)?.TxtDesig,
                                    Status.Deleted));
                            }
                        }
                        break;

                    case PDM_ENUM.Airspace:
                        if (pdmObj == null || pdmObj.PDM_Type != PDM_ENUM.Airspace)
                            _airspaceDetails.Add(new DetailedItem(oldObj, (oldObj as Airspace)?.TxtName,
                                Status.Deleted));
                        break;

                    case PDM_ENUM.HoldingPattern:
                        if (((HoldingPattern) oldObj).Type == CodeHoldingUsage.ENR &&
                            (pdmObj == null || pdmObj.PDM_Type != PDM_ENUM.HoldingPattern))
                            _holdingDetails.Add(new DetailedItem(oldObj, "Holding Pattern", Status.Deleted));
                        break;
                    case PDM_ENUM.AirportHeliport:
                        if (pdmObj == null || pdmObj.PDM_Type != PDM_ENUM.AirportHeliport)
                            _airportDeTails.Add(new DetailedItem(oldObj, (oldObj as AirportHeliport)?.Name,
                                Status.Deleted));
                        break;
                    case PDM_ENUM.NavaidSystem:
                        if (pdmObj == null || pdmObj.PDM_Type != PDM_ENUM.NavaidSystem)
                            _airportDeTails.Add(new DetailedItem(oldObj, (oldObj as NavaidSystem)?.Name,
                                Status.Deleted));

                        break;
                }
            }
        }

        private void CompareEnroute(List<PDMObject> oldObjList, PDMObject currObj)
        {
            var enroute = (Enroute) currObj;
            if (enroute.Routes.All(t => t.CodeLvl != _routeLevel))
                return;

            //if (!ShouldCheckEnroute(enroute))
            //    return;

            var oldEnroute = oldObjList.FirstOrDefault(pdm => pdm.ID == enroute.ID) as Enroute;
            DetailedItem item = null;
            if (oldEnroute == null)
            {
                item = new DetailedItem(enroute, enroute.TxtDesig, Status.New);
                _enrouteDetails.Add(item);
                return;
            }

            if (enroute.TxtDesig != oldEnroute.TxtDesig)
            {
                item = new DetailedItem(enroute, enroute.TxtDesig, Status.Changed);
                item.FieldLogList.Add(new FieldLog(nameof(enroute.TxtDesig), oldEnroute.TxtDesig, enroute.TxtDesig));
                _enrouteDetails.Add(item);
            }
            
            CompareRouteSegment(enroute, oldEnroute);
        }

        private bool ShouldCheckEnroute(Enroute enroute)
        {
            return enroute.Routes.All(t => t.CodeLvl == _routeLevel);
            //switch (_routeLevel)
            //{
            //    case CODE_ROUTE_SEGMENT_CODE_LVL.OTHER:
            //        return enroute.Routes.All(t => t.CodeLvl == CODE_ROUTE_SEGMENT_CODE_LVL.OTHER);
            //    case CODE_ROUTE_SEGMENT_CODE_LVL.LOWER:
            //        return enroute.Routes.All(t =>
            //            t.CodeLvl == CODE_ROUTE_SEGMENT_CODE_LVL.LOWER || t.CodeLvl == CODE_ROUTE_SEGMENT_CODE_LVL.BOTH);
            //    case CODE_ROUTE_SEGMENT_CODE_LVL.BOTH:
            //        return enroute.Routes.Any(t => t.CodeLvl == CODE_ROUTE_SEGMENT_CODE_LVL.OTHER);
            //    case CODE_ROUTE_SEGMENT_CODE_LVL.UPPER:
            //        return enroute.Routes.All(t =>
            //            t.CodeLvl == CODE_ROUTE_SEGMENT_CODE_LVL.UPPER || t.CodeLvl == CODE_ROUTE_SEGMENT_CODE_LVL.BOTH);
            //    default:
            //        throw new ArgumentOutOfRangeException();
            //}
        }

        private void CompareRouteSegment(Enroute enroute, Enroute oldEnroute)
        {
            try
            {
                foreach (var routeSeg in enroute.Routes)
                {
                    if (!(oldEnroute.Routes.FirstOrDefault(pdm => pdm.ID == routeSeg.ID) is RouteSegment oldRouteSeg))
                    {
                        if (routeSeg.CodeLvl == _routeLevel)
                            _routeSegDetails.Add(new DetailedItem(routeSeg,
                                enroute.TxtDesig + " => " + routeSeg.ToString(),
                                Status.New));
                        continue;
                    }
                    List<FieldLog> logList = new List<FieldLog>();
                    CompareRouteSegmentPoint(enroute.TxtDesig, routeSeg, routeSeg.StartPoint, oldRouteSeg.StartPoint,
                        nameof(routeSeg.StartPoint), logList);

                    CompareRouteSegmentPoint(enroute.TxtDesig, routeSeg, routeSeg.EndPoint, oldRouteSeg.EndPoint,
                        nameof(routeSeg.EndPoint), logList);

                    CompareNullableProp(routeSeg.ValMagTrack, oldRouteSeg.ValMagTrack,
                        nameof(routeSeg.ValMagTrack), logList);

                    CompareNullableProp(routeSeg.ValReversMagTrack, oldRouteSeg.ValReversMagTrack,
                        nameof(routeSeg.ValReversMagTrack), logList);

                    CompareNullableProp(routeSeg.ValLen, oldRouteSeg.ValLen, nameof(routeSeg.ValLen), logList);
                    
                    if (routeSeg.CodeDir != oldRouteSeg.CodeDir)
                    {
                        logList.Add(new FieldLog(nameof(routeSeg.CodeDir), oldRouteSeg.CodeDir.ToString(),
                            routeSeg.CodeDir.ToString()));
                    }

                    if (routeSeg.CodeLvl != oldRouteSeg.CodeLvl)
                    {
                        logList.Add(new FieldLog(nameof(routeSeg.CodeLvl), oldRouteSeg.CodeLvl.ToString(),
                            routeSeg.CodeLvl.ToString()));
                    }

                    #region Upper data comparison

                    CompareNullableProp(routeSeg.ValDistVerUpper, oldRouteSeg.ValDistVerUpper,
                        nameof(routeSeg.ValDistVerUpper), logList);

                    if (routeSeg.UomValDistVerUpper != oldRouteSeg.UomValDistVerUpper)
                    {
                        logList.Add(new FieldLog(nameof(routeSeg.UomValDistVerUpper),
                            oldRouteSeg.UomValDistVerUpper.ToString(),
                            routeSeg.UomValDistVerUpper.ToString()));
                    }

                    if (routeSeg.CodeDistVerUpper != oldRouteSeg.CodeDistVerUpper)
                    {
                        logList.Add(new FieldLog(nameof(routeSeg.CodeDistVerUpper),
                            oldRouteSeg.CodeDistVerUpper.ToString(),
                            routeSeg.CodeDistVerUpper.ToString()));
                    }
                    #endregion

                    #region Lower data comparison

                    CompareNullableProp(routeSeg.ValDistVerLower, oldRouteSeg.ValDistVerLower,
                        nameof(routeSeg.ValDistVerLower), logList);

                    if (routeSeg.UomValDistVerLower != oldRouteSeg.UomValDistVerLower)
                    {
                        logList.Add(new FieldLog(nameof(routeSeg.UomValDistVerLower),
                            oldRouteSeg.UomValDistVerLower.ToString(),
                            routeSeg.UomValDistVerLower.ToString()));
                    }

                    if (routeSeg.CodeDistVerLower != oldRouteSeg.CodeDistVerLower)
                    {
                        logList.Add(new FieldLog(nameof(routeSeg.CodeDistVerLower),
                            oldRouteSeg.CodeDistVerLower.ToString(),
                            routeSeg.CodeDistVerLower.ToString()));
                    }

                    #endregion

                    routeSeg.RebuildGeo();
                    oldRouteSeg.RebuildGeo();
                    if (routeSeg.Geo == null ^ oldRouteSeg.Geo == null)
                    {
                        logList.Add(new FieldLog("Geometry", "", "")
                        {
                            OldGeometry = oldRouteSeg.Geo as IGeometry
                        });
                    }
                    else
                    {
                        if (routeSeg.Geo != null)
                        {
                            var currGeo = (ICurve)GlobalParams.SpatialOperation.ToProject(routeSeg.Geo);
                            var oldGeo = (ICurve)GlobalParams.SpatialOperation.ToProject(oldRouteSeg.Geo);

                            if (Math.Abs(currGeo.Length - oldGeo.Length) > _tolerance)
                            {
                                logList.Add(new FieldLog("Geometry", "", "")
                                {
                                    OldGeometry = oldRouteSeg.Geo
                                });
                            }
                        }
                    }

                    if (logList.Count == 0)
                        return;

                    _routeSegDetails.Add(new DetailedItem(routeSeg,
                        enroute.TxtDesig + " => " + routeSeg.ToString(), Status.Changed)
                    {
                        FieldLogList = logList
                    });
                }

                Check4DeletedRouteSegments(enroute, oldEnroute);
            }
            catch (Exception ex)
            {
                throw new Exception("Route Segment " + ex.Message);
            }
        }

        private void CompareRouteSegmentPoint(string enrouteName, RouteSegment routeSeg, RouteSegmentPoint currPoint,
            RouteSegmentPoint oldPoint, string fieldName, List<FieldLog> logList)
        {
            if (currPoint != null ^ oldPoint != null)
            {
                logList.Add(new FieldLog(fieldName, (oldPoint == null) ? "Has not" : "Has",
                    (currPoint == null) ? "Has not" : "Has"));
            }
            else if (currPoint != null)
            {
                if (currPoint.PointChoice != oldPoint.PointChoice)
                {
                    logList.Add(new FieldLog(fieldName, currPoint.PointChoice.ToString(),
                        oldPoint.PointChoice.ToString()));
                }
                else
                {
                    if (currPoint.PointChoice != PointChoice.DesignatedPoint)
                        return;

                    var dsgPnt =
                        _currAllDsgPnts.FirstOrDefault(pdm => pdm.ID == currPoint.PointChoiceID) as
                            WayPoint;
                    var oldDsgPnt = _oldAllDsgPnts.FirstOrDefault(pdm =>
                        pdm.ID == oldPoint.PointChoiceID) as WayPoint;
                    if (dsgPnt == null ^ oldDsgPnt == null)
                    {
                        if (currPoint.PointChoiceID == oldPoint.PointChoiceID)
                        {
                            if (oldDsgPnt == null)
                            {
                                DetailedItem detItem = new DetailedItem(dsgPnt, dsgPnt.Designator, Status.New);
                                if (!_wayPointDetails.Exists(pdm => pdm.Feature.ID == detItem.Feature.ID))
                                    _wayPointDetails.Add(detItem);
                            }
                            else
                            {
                                DetailedItem detItem = new DetailedItem(oldDsgPnt, oldDsgPnt.Designator,
                                    Status.Deleted);
                                if (!_wayPointDetails.Exists(pdm => pdm.Feature.ID == detItem.Feature.ID))
                                    _wayPointDetails.Add(detItem);
                            }
                        }
                        else
                        {
                            if (oldDsgPnt == null)
                            {
                                throw new Exception("Not implemented start point in routesegment");
                            }
                        }
                    }
                    else if (dsgPnt != null)
                    {
                        DetailedItem detItem = null;
                        if (dsgPnt.Lat != oldDsgPnt.Lat || dsgPnt.Lon != oldDsgPnt.Lon)
                        {
                            detItem = new DetailedItem(dsgPnt, dsgPnt.Designator, Status.Changed);
                            detItem.FieldLogList.Add(new FieldLog("Geometry",
                                oldDsgPnt.Lon + " " + oldDsgPnt.Lat, dsgPnt.Lon + " " + dsgPnt.Lat)
                            {
                                OldGeometry = oldDsgPnt.Geo
                            });
                            if (!_wayPointDetails.Exists(pdm => pdm.Feature.ID == detItem.Feature.ID))
                                _wayPointDetails.Add(detItem);
                        }

                        if (dsgPnt.Designator != oldDsgPnt.Designator)
                        {
                            if (detItem == null)
                                detItem = new DetailedItem(dsgPnt, dsgPnt.Designator, Status.Changed);
                            detItem.FieldLogList.Add(new FieldLog(nameof(dsgPnt.Designator), oldDsgPnt.Designator,
                                dsgPnt.Designator));
                            if (!_wayPointDetails.Exists(det => det.Feature.ID == detItem.Feature.ID))
                                _wayPointDetails.Add(detItem);
                        }
                    }
                }
            }
        }

        private void Check4DeletedRouteSegments(Enroute enroute, Enroute oldEnroute)
        {
            foreach (var oldRouteSeg in oldEnroute.Routes)
            {
                if (!(enroute.Routes.FirstOrDefault(pdm => pdm.ID == oldRouteSeg.ID) is RouteSegment routeSeg))
                {
                    _routeSegDetails.Add(new DetailedItem(oldRouteSeg,
                        oldEnroute.TxtDesig + " => " + oldRouteSeg.ToString(), Status.Deleted));
                }
            }
        }

        private void CompareAirspace(List<PDMObject> oldObjList, PDMObject currObj)
        {
            try
            {
                var airspace = (Airspace) currObj;
                if (!(oldObjList.FirstOrDefault(pdm => pdm.ID == airspace.ID) is Airspace oldObj))
                {
                    var str = airspace.TxtName;
                    if (str == null || str.Trim() == "")
                        str = airspace.CodeID;
                    _airspaceDetails.Add(new DetailedItem(airspace, str, Status.New));
                    return;
                }

                List<FieldLog> logList = new List<FieldLog>();
                if (IsSpecialAirspace(airspace))
                {
                    if (airspace.TxtName != oldObj.TxtName)
                    {
                        logList.Add(new FieldLog(nameof(airspace.TxtName), oldObj.TxtName, airspace.TxtName));
                    }

                    if (airspace.AirTrafficControlServiceName != oldObj.AirTrafficControlServiceName)
                    {
                        logList.Add(new FieldLog(nameof(airspace.AirTrafficControlServiceName),
                            oldObj.AirTrafficControlServiceName, airspace.AirTrafficControlServiceName));
                    }

                    if (airspace.RadioCommunicationFrequencyTransmission !=
                        oldObj.RadioCommunicationFrequencyTransmission)
                    {
                        logList.Add(new FieldLog(nameof(airspace.RadioCommunicationFrequencyTransmission),
                            oldObj.RadioCommunicationFrequencyTransmission,
                            airspace.RadioCommunicationFrequencyTransmission));
                    }

                    if (airspace.CodeType != oldObj.CodeType)
                    {
                        logList.Add(new FieldLog(nameof(airspace.CodeType), oldObj.CodeType.ToString(),
                            airspace.CodeType.ToString()));
                    }

                    if (airspace.RadioCommunicationFrequencyReception !=
                        oldObj.RadioCommunicationFrequencyReception)
                    {
                        logList.Add(new FieldLog(nameof(airspace.RadioCommunicationFrequencyReception),
                            oldObj.RadioCommunicationFrequencyReception,
                            airspace.RadioCommunicationFrequencyReception));
                    }
                }

                foreach (var volume in airspace.AirspaceVolumeList)
                {
                    if (!(oldObj.AirspaceVolumeList.FirstOrDefault(pdm => pdm.ID == volume.ID) is AirspaceVolume
                        oldVolume))
                    {
                        logList.Add(new FieldLog(nameof(volume), "Has not", "Created"));
                        continue;
                    }

                    if (volume.CodeId != oldVolume.CodeId)
                    {
                        logList.Add(new FieldLog(nameof(volume.CodeId), oldVolume.CodeId,
                            volume.CodeId));
                    }

                    CompareNullableProp(volume.ValDistVerUpper, oldVolume.ValDistVerUpper, nameof(volume.ValDistVerUpper), logList);

                    if (volume.UomValDistVerUpper != oldVolume.UomValDistVerUpper)
                    {
                        logList.Add(new FieldLog(nameof(oldVolume.UomValDistVerUpper),
                            oldVolume.UomValDistVerUpper.ToString(), volume.UomValDistVerUpper.ToString()));
                    }

                    if (volume.CodeDistVerUpper != oldVolume.CodeDistVerUpper)
                    {
                        logList.Add(new FieldLog(nameof(volume.CodeDistVerUpper),
                            oldVolume.CodeDistVerUpper.ToString(),
                            volume.CodeDistVerUpper.ToString()));
                    }

                    CompareNullableProp(volume.ValDistVerLower, oldVolume.ValDistVerLower, nameof(volume.ValDistVerLower), logList);
                    if (volume.UomValDistVerLower != oldVolume.UomValDistVerLower)
                    {
                        logList.Add(new FieldLog(nameof(oldVolume.UomValDistVerLower),
                            oldVolume.UomValDistVerLower.ToString(), volume.UomValDistVerLower.ToString()));
                    }

                    if (volume.CodeDistVerLower != oldVolume.CodeDistVerLower)
                    {
                        logList.Add(new FieldLog(nameof(volume.CodeDistVerLower),
                            oldVolume.CodeDistVerLower.ToString(),
                            volume.CodeDistVerLower.ToString()));
                    }

                    volume.RebuildGeo2();
                    oldVolume.RebuildGeo2();
                    if (volume.Geo == null ^ oldVolume.Geo == null)
                    {
                        logList.Add(
                            new FieldLog("Geometry (Volume)", "", "")
                            {
                                OldGeometry = oldVolume.Geo
                            });

                    }
                    else
                    {
                        if (volume.Geo != null)
                        {
                            var volPrjGeo = GlobalParams.SpatialOperation.ToProject(volume.Geo);
                            var oldVolPrjGeo = GlobalParams.SpatialOperation.ToProject(oldVolume.Geo);
                            if (volPrjGeo is IPolygon volGeo && oldVolPrjGeo is IPolygon oldGeo)
                            {
                                if (Math.Abs(volGeo.Length - oldGeo.Length) > _tolerance)
                                {
                                    logList.Add(
                                        new FieldLog("Geometry (Volume)", "", "")
                                        {
                                            OldGeometry = oldVolume.Geo
                                        });
                                }
                            }
                        }
                    }
                }

                if (logList.Count == 0)
                    return;
                _airspaceDetails.Add(
                    new DetailedItem(airspace, airspace.TxtName, Status.Changed)
                    {
                        FieldLogList = logList
                    });
            }
            catch (Exception ex)
            {
                throw new Exception("Airspace " + ex.Message);
            }
        }

        /// <summary>
        /// Defines filter type (based on CodeType) by logic which Ruslan sent via mail
        /// </summary>
        /// <param name="airspace"></param>
        /// <returns></returns>
        private bool IsSpecialAirspace(Airspace airspace)
        {
            switch (airspace.CodeType)
            {
                case AirspaceType.TMA:
                case AirspaceType.TMA_P:
                case AirspaceType.CTR:
                case AirspaceType.CTR_P:
                case AirspaceType.ATZ:
                case AirspaceType.ATZ_P:
                    return true;
            }

            return false;
        }

        private void CompareHolding(List<PDMObject> oldObjList, PDMObject currObj)
        {
            try
            {
                var holding = (HoldingPattern) currObj;
                if (holding.Type != CodeHoldingUsage.ENR)
                    return;
                if (!(oldObjList.FirstOrDefault(pdm => pdm.ID == holding.ID) is HoldingPattern oldObj))
                {
                    _holdingDetails.Add(new DetailedItem(holding, holding.GetObjectLabel(), Status.New));
                    return;
                }

                var logList = new List<FieldLog>();

                CompareNullableProp(holding.OutboundCourse, oldObj.OutboundCourse, nameof(holding.OutboundCourse),
                    logList);

                CompareNullableProp(holding.InboundCourse, oldObj.InboundCourse, nameof(holding.InboundCourse),
                    logList);

                CompareNullableProp(holding.LowerLimit, oldObj.LowerLimit, nameof(holding.LowerLimit),
                    logList);
                if (holding.LowerLimitUOM != oldObj.LowerLimitUOM)
                {
                    logList.Add(new FieldLog(nameof(holding.LowerLimitUOM), oldObj.LowerLimitUOM.ToString(),
                        holding.LowerLimitUOM.ToString()));
                }

                CompareNullableProp(holding.UpperLimit, oldObj.UpperLimit, nameof(holding.UpperLimit),
                    logList);
                if (holding.UpperLimitUOM != oldObj.UpperLimitUOM)
                {
                    logList.Add(new FieldLog(nameof(holding.UpperLimitUOM), oldObj.UpperLimitUOM.ToString(),
                        holding.UpperLimitUOM.ToString()));
                }

                if (holding.Duration_Distance_UOM != oldObj.Duration_Distance_UOM)
                {
                    logList.Add(new FieldLog(nameof(holding.Duration_Distance_UOM), oldObj.Duration_Distance_UOM.ToString(),
                        holding.Duration_Distance_UOM.ToString()));
                }

                CompareNullableProp(holding.Duration_Distance, oldObj.Duration_Distance, nameof(holding.Duration_Distance),
                    logList);

                if (holding.HoldingPoint != null ^ oldObj.HoldingPoint != null)
                {
                    var newText = holding.HoldingPoint == null ? "Has not" : "Has";
                    var oldText = oldObj.HoldingPoint == null ? "Has not" : "Has";
                    logList.Add(new FieldLog(nameof(holding.HoldingPoint), oldText, newText));
                }
                else if (holding.HoldingPoint != null && oldObj.HoldingPoint != null)
                {
                    if (holding.HoldingPoint.SegmentPointDesignator != oldObj.HoldingPoint.SegmentPointDesignator)
                    {
                        logList.Add(new FieldLog(nameof(holding.HoldingPoint.SegmentPointDesignator),
                            oldObj.HoldingPoint.SegmentPointDesignator, holding.HoldingPoint.SegmentPointDesignator));
                    }
                }

                if (holding.TurnDirection != oldObj.TurnDirection)
                {
                    logList.Add(new FieldLog(nameof(holding.TurnDirection), oldObj.TurnDirection.ToString(),
                        holding.TurnDirection.ToString()));
                }

                holding.RebuildGeo2();
                oldObj.RebuildGeo2();
                if (holding.Geo == null ^ oldObj.Geo == null)
                {
                    logList.Add(new FieldLog("Geometry", "", "")
                    {
                        OldGeometry = oldObj.Geo as IGeometry
                    });
                }
                else
                {
                    if (holding.Geo != null)
                    {
                        var volPrjGeo = GlobalParams.SpatialOperation.ToProject(holding.Geo);
                        var oldVolPrjGeo = GlobalParams.SpatialOperation.ToProject(oldObj.Geo);

                        if (volPrjGeo is IArea geo && oldVolPrjGeo is IArea oldGeo)
                        {
                            if (Math.Abs(geo.Area - oldGeo.Area) > _tolerance)
                            {
                                logList.Add(new FieldLog("Geometry", "", "") { OldGeometry = oldObj.Geo as IGeometry });
                            }
                        }
                    }
                }               

                if (logList.Count == 0)
                    return;
                _holdingDetails.Add(
                    new DetailedItem(holding, holding.GetObjectLabel(), Status.Changed)
                    {
                        FieldLogList = logList
                    });
            }
            catch (Exception ex)
            {
                throw new Exception("Holding " + ex.Message);
            }
        }

        private void CompareNullableProp(double? currProp, double? oldProp, string fieldName,
            List<FieldLog> logList)
        {
            if (currProp == null ^ oldProp == null)
            {
                logList.Add(new FieldLog(fieldName,
                    oldProp?.ToString() ?? "",
                    currProp?.ToString() ?? ""));
            }
            else
            {
                if (currProp != null)
                {
                    if (double.IsNaN(currProp.Value) ^ double.IsNaN(oldProp.Value))
                    {
                        logList.Add(new FieldLog(fieldName,
                            oldProp.Value.ToString(CultureInfo.InvariantCulture),
                            currProp.Value.ToString(CultureInfo.InvariantCulture)));
                    }
                    else
                    {
                        if (!double.IsNaN(currProp.Value) || !double.IsNaN(oldProp.Value))
                        {
                            if (Math.Abs(currProp.Value - oldProp.Value) > _tolerance)
                            {
                                logList.Add(new FieldLog(fieldName,
                                    oldProp.Value.ToString(CultureInfo.InvariantCulture),
                                    currProp.Value.ToString(CultureInfo.InvariantCulture)));
                            }
                        }
                    }
                }
            }
        }
    }
}