using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Aran.Aim;
using Aran.Aim.Features;
using ARENA;
using DataModule;
using PDM;
using ArenaToolBox;
using Airspace = Aran.Aim.Features.Airspace;
using ARENA.Enums_Const;
using Aran.Delta.Model;

namespace Aran.Delta
{
// ReSharper disable once InconsistentNaming
    public class ArenaDBModule:IDBModule
    {
        private List<PDMObject> _pdmObjectList;
        private List<Aim.Features.Route> _routeList;
        private Aim.Features.AirportHeliport _airportHeliport;
        private List<Aran.Aim.Features.DesignatedPoint> _designatedPointList;
        private List<Aran.Aim.Features.Navaid> _navaidListByTypes;
        private List<Aim.Features.Airspace> _airspaceList;
        private List<Navaid> _navaidList;

        public ArenaDBModule()
        {
            if (DataCash.ProjectEnvironment == null)
            {
                MessageBox.Show("Error loading Database!");
                throw new Exception("Error loading Db");
            }

            _pdmObjectList = DataCash.ProjectEnvironment.Data.PdmObjectList;
            ProjectPath =System.IO.Path.GetDirectoryName(DataCash.ProjectEnvironment.Data.CurProjectName);
        }

        public List<Aim.Features.Route> RouteList
        {
            get
            {
                if (_routeList == null)
                {
                    if (_pdmObjectList != null)
                    {
                        var arenaRouteList =
                            _pdmObjectList.Where(pdmObject => pdmObject is PDM.Enroute).Cast<PDM.Enroute>().ToList();

                        _routeList = new List<Route>();
                        if (arenaRouteList.Count > 0)
                        {
                            foreach (var areanaEnroute in arenaRouteList)
                            {
                                var enroute = Model.ConvertPdmToAim.ToRoute(areanaEnroute);
                                enroute.SetLayerName("Route");
                                _routeList.Add(enroute);
                            }
                        }
                    }
                    if (GlobalParams.DesigningAreaReader != null)
                    {
                        var featList = GlobalParams.DesigningAreaReader.DesigningRoutes;
                        if (featList != null)
                        {
                            foreach (var designingItem in featList)
                            {
                                var aimPt = Model.ConvertDesigningClassToAim.ToRoute(designingItem);
                                if (aimPt != null)
                                {
                                    aimPt.SetLayerName("Designing_Segment");
                                    _routeList.Add(aimPt);
                                }
                            }
                        }
                    }
                }
             
                return _routeList;
            }
        }

        public List<Aim.Features.RouteSegment> GetRouteSegmentList(Guid routeIdentifier)
        {
            var result = new List<Aim.Features.RouteSegment>();
            var selRoute =(PDM.Enroute)_pdmObjectList.FirstOrDefault(pdmObject => pdmObject is PDM.Enroute && pdmObject.ID == routeIdentifier.ToString());
            if (selRoute != null)
            {
                foreach (var routeSegment in selRoute.Routes)
                {
                    var aimRouteSegment = Model.ConvertPdmToAim.ToRouteSegment(routeSegment);
                    if (aimRouteSegment!=null)
                        result.Add(aimRouteSegment);  
                }
            }
            return result;
        }

        public Aim.Features.AirportHeliport AirportHeliport
        {
            get
            {
                if (_airportHeliport == null)
                {
                    if (_pdmObjectList != null)
                    {
                        var adhp =
                            _pdmObjectList.First(
                                pdmObject =>
                                    pdmObject.ID == GlobalParams.Settings.DeltaQuery.Aeroport.ToString() &&
                                    pdmObject is PDM.AirportHeliport);

                        _airportHeliport = Model.ConvertPdmToAim.ToAirportHeliport((PDM.AirportHeliport) adhp);
                    }
                }
                return _airportHeliport;
            }
        }

        public List<Aim.Features.DesignatedPoint> DesignatedPointList
        {
            get
            {
                if (_designatedPointList == null)
                {
                    if (_pdmObjectList != null)
                    {
                        var arenaDpList =
                            _pdmObjectList.Where(pdmObject => pdmObject is PDM.WayPoint).Cast<PDM.WayPoint>().ToList();

                        _designatedPointList = new List<DesignatedPoint>();
                        foreach (var areanaDp in arenaDpList)
                            _designatedPointList.Add(Model.ConvertPdmToAim.ToDesignatedPoint(areanaDp));
                    }
                    if (GlobalParams.DesigningAreaReader != null)
                    {
                        var designingPoints = GlobalParams.DesigningAreaReader.GetDesigningPoints();
                        if (designingPoints != null) 
                        {
                            foreach (var designingPoint in designingPoints)
                            {
                                var aimPt = Model.ConvertDesigningClassToAim.ToDesignatedPoint(designingPoint);
                                if (aimPt != null)
                                    _designatedPointList.Add(aimPt);
                            }
                        }
                    }
                }

                return _designatedPointList;
            }
        }

        public List<Aim.Features.Navaid> NavaidListByTypes
        {
            get
            {
                if (_navaidListByTypes == null)
                {
                    var pdmNavaidList = _pdmObjectList.Where(pdmObject => pdmObject is PDM.NavaidSystem).ToList();

                    _navaidListByTypes = new List<Aran.Aim.Features.Navaid>();

                    foreach (var aranNav in pdmNavaidList)
                    {
                        var pdmNavaid = Model.ConvertPdmToAim.ToNavaid((NavaidSystem)aranNav);
                        if (pdmNavaid!=null)
                            _navaidListByTypes.Add(pdmNavaid);
                    }
                }

                return _navaidListByTypes;
            }
        }

        public List<Aim.Features.Airspace> GetAirspaceList
        {
            get
            {
                if (_airspaceList == null)
                {
                    if (_pdmObjectList != null)
                    {
                        var pdmAirspaceList = _pdmObjectList.Where(pdmObject => pdmObject is PDM.Airspace).ToList();

                        _airspaceList = new List<Airspace>();

                        foreach (var pdmAirspace in pdmAirspaceList)
                        {
                            try
                            {
                                var aimAirspace = Model.ConvertPdmToAim.ToAirspace((PDM.Airspace) pdmAirspace);
                                if (aimAirspace != null)
                                {
                                    aimAirspace.SetLayerName("Airspace");
                                    _airspaceList.Add(aimAirspace);
                                }
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }

                        if (GlobalParams.DesigningAreaReader != null)
                        {
                            var featList = GlobalParams.DesigningAreaReader.GetDesigningAreas();
                            if (featList != null)
                            {
                                foreach (var designingItem in featList)
                                {
                                    var aimPt = Model.ConvertDesigningClassToAim.ToAirspace(designingItem);
                                    if (aimPt != null)
                                    {
                                        aimPt.SetLayerName("Designing_Area");
                                        _airspaceList.Add(aimPt);
                                    }
                                }
                            }

                            var bufferList = GlobalParams.DesigningAreaReader.GetDesigningBuffers();
                            if (bufferList != null)
                            {
                                foreach (var designingItem in bufferList)
                                {
                                    var aimPt = Model.ConvertDesigningClassToAim.ToAirspace(designingItem);

                                    if (aimPt != null)
                                    {
                                        aimPt.SetLayerName("Designing_Buffer");
                                        _airspaceList.Add(aimPt);
                                    }
                                }
                            
                            }
                        }
                    }
                }
                return _airspaceList;
            }
        }

        public Queries.DeltaQPI.IDeltaQPI DeltaQPI
        {
            get { return null; }
        }

        public string GetFeatureName(Aim.FeatureType featureType, Guid guid)
        {
            if (featureType == FeatureType.AirportHeliport)
            {
                var resultFeat = _pdmObjectList.Find(pdmObject => pdmObject is PDM.AirportHeliport && pdmObject.ID == guid.ToString()) as PDM.AirportHeliport;
                if (resultFeat != null) return resultFeat.Designator;
            }
            if (featureType == FeatureType.DesignatedPoint)
            {
                var resultFeat = _pdmObjectList.Find(pdmObject => pdmObject is PDM.WayPoint && pdmObject.ID == guid.ToString()) as PDM.WayPoint;
                if (resultFeat != null) return resultFeat.Designator;
            }
            if (featureType == FeatureType.Navaid)
            {
                var resultFeat = _pdmObjectList.Find(pdmObject => pdmObject is PDM.NavaidSystem && pdmObject.ID == guid.ToString()) as PDM.NavaidSystem;
                if (resultFeat != null) return resultFeat.Designator;
            }
            if (featureType == FeatureType.RunwayCentrelinePoint)
            {
                var resultFeat = _pdmObjectList.Find(pdmObject => pdmObject is PDM.RunwayCenterLinePoint && pdmObject.ID == guid.ToString()) as PDM.RunwayCenterLinePoint;
                if (resultFeat != null) return resultFeat.Designator;
            }
            return "";
        }

        public bool CommitRouteFeature(RouteSegmentModel routeSegment)
        {
            throw new NotImplementedException();
        }

        public string ProjectPath { get; set; }

        public List<RunwayCentrelinePoint> RunwayCenterlineList => throw new NotImplementedException();

        public List<Navaid> NavaidList{
            get
            {
                if (_navaidList == null)
                {
                    var pdmNavaidList = _pdmObjectList.Where(pdmObject => pdmObject is PDM.NavaidSystem).ToList();

                    _navaidList = new List<Aran.Aim.Features.Navaid>();

                    foreach (var aranNav in pdmNavaidList)
                    {
                        var pdmNavaid = Model.ConvertPdmToAim.ToNavaid((NavaidSystem)aranNav);
                        if (pdmNavaid!=null)
                            _navaidList.Add(pdmNavaid);
                    }
                }

                return _navaidList;
            }
        }
    }
}
