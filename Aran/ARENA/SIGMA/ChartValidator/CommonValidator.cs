using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Converters;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Geometry;
using PDM;

namespace ChartValidator
{
	internal class CommonValidator
	{
		private List<string> _checkedObjList;
		private List<PDMObject> _designatedPointList;
		private List<PDMObject> _navaidList;
		private ChartType _chartType;

		private const double segmentPntTolerance = 0.00028;

		public CommonValidator (List<PDMObject> designatedPointList, List<PDMObject> navaidList, ChartType chartType)
		{
			_designatedPointList = designatedPointList;
			_navaidList = navaidList;
			_checkedObjList = new List<string> ();
			_chartType = chartType;
		}

		public void CheckNavaid (List<ResultLog> logList, SegmentPoint StartPoint, SegmentPoint EndPoint, PDMObject feat, PDMObject parentFeat, bool isStart = true)
		{
			PDM.NavaidSystem navaid;
			if (isStart)
			{
                if (StartPoint == null) return;
                if ( _checkedObjList.Contains(StartPoint.PointChoiceID))
					return;
				_checkedObjList.Add (StartPoint.PointChoiceID);
				navaid = _navaidList.Where (pdm => pdm.ID == StartPoint.PointChoiceID).FirstOrDefault () as NavaidSystem;
			}
			else
			{
                if (EndPoint == null) return;
				if (_checkedObjList.Contains (EndPoint.PointChoiceID))
					return;
				_checkedObjList.Add (EndPoint.PointChoiceID);
				navaid = _navaidList.Where (pdm => pdm.ID == EndPoint.PointChoiceID).FirstOrDefault () as NavaidSystem;
			}
			if (navaid == null)
			{
				if (isStart)
					logList.Add (new ResultLog (feat, ReportType.Segment_Point_Link, "Start Point's link to Navaid is not defined", parentFeat));
				else
					logList.Add (new ResultLog (feat, ReportType.Segment_Point_Link, "End Point's link to Navaid is not defined", parentFeat));
			}
			else
			{
				if (navaid.Geo == null)
					navaid.RebuildGeo ();
				if (navaid.Designator.Trim () == "")
					logList.Add (new ResultLog (navaid, ReportType.Mandatory_Field_Absent, "Designator"));
				foreach (var navComponent in navaid.Components)
				{
					if (navComponent.PDM_Type == PDM_ENUM.VOR)
					{
						PDM.VOR vor = navComponent as PDM.VOR;
						if (vor == null || vor.Frequency == null || !vor.Frequency.HasValue)
							logList.Add (new ResultLog (navaid, ReportType.Mandatory_Field_Absent, "Frequency"));
						if (vor.Geo == null)
							vor.RebuildGeo ();
						if (vor.Geo == null)
							logList.Add (new ResultLog (navaid, ReportType.Mandatory_Field_Absent, "Geometry"));
					}
					else if (navComponent.PDM_Type == PDM_ENUM.DME)
					{
						PDM.DME dme = navComponent as PDM.DME;
						if (dme == null || dme.Elev == null || !dme.Elev.HasValue)
							logList.Add (new ResultLog (navaid, ReportType.Mandatory_Field_Absent, "Elevation"));
						if (dme.Geo == null)
							dme.RebuildGeo ();
						if (dme.Geo == null)
							logList.Add (new ResultLog (navaid, ReportType.Mandatory_Field_Absent, "Geometry"));
					}
				}
				//if (navaid.Geo == null)
				//	logList.Add (new ResultLog (routeSegment, ReportType.Mandatory_Field_Absent, "Geometry"));
			}
		}

		public void CheckDsgPoint (List<ResultLog> logList, SegmentPoint segmentPoint, PDMObject feat, PDMObject parentFeat, bool isStart = true)
		{
			PDM.WayPoint dsgPnt;

            if (segmentPoint == null) return;
			if (_checkedObjList.Contains (segmentPoint.PointChoiceID))
				return;
			_checkedObjList.Add (segmentPoint.PointChoiceID);
			dsgPnt = _designatedPointList.Where (pdm => pdm.ID == segmentPoint.PointChoiceID).FirstOrDefault () as PDM.WayPoint;
			if (dsgPnt == null)
			{
				if (isStart)
					logList.Add (new ResultLog (feat, ReportType.Segment_Point_Link, "Start Point's link to Designated Point is deleted", parentFeat));
				else
					logList.Add (new ResultLog (feat, ReportType.Segment_Point_Link, "End Point's link to Designated Point is deleted", parentFeat));
			}
			else
			{
				if (dsgPnt.Geo == null)
					dsgPnt.RebuildGeo ();
				if (dsgPnt.Geo == null)
					logList.Add (new ResultLog (feat, ReportType.Mandatory_Field_Absent, "Geometry", parentFeat));
				if (_chartType == ChartType.SID || _chartType == ChartType.Star)
				{
					if (string.IsNullOrEmpty (dsgPnt.Designator))
                        logList.Add(new ResultLog(dsgPnt, ReportType.Mandatory_Field_Absent, "Designator"));
				}
			}
		}

        public List<ResultLog> CheckRouteSegmentsBreak(PDMObject feat, Dictionary<SegmentPointData, SegmentPointData> routeSegmentHashList, string endingPntName = "")
        {
            List<ResultLog> resultLogs = new List<ResultLog>();
            List<SegmentPointData> startKeys = new List<SegmentPointData>();
            bool hasKey;
            for (int i = 0; i < routeSegmentHashList.Count; i++)
			{
				hasKey = false;
				for (int j = 0; j < routeSegmentHashList.Count; j++)
				{
					if (i != j && routeSegmentHashList.ElementAt (j).Value == routeSegmentHashList.ElementAt (i).Key)
					{
						hasKey = true;
						break;
					}
				}
				if (!hasKey)
					startKeys.Add (routeSegmentHashList.ElementAt (i).Key);
			}
			if (startKeys.Count == 1)
			{
				if((_chartType == ChartType.SID || _chartType == ChartType.Star ) && endingPntName != "")
				{
					//Checks that key/value pairs is finished by endingPointName or not
					var startKey = startKeys[0];
					while (routeSegmentHashList.ContainsKey (routeSegmentHashList[startKey]))
					{
						startKey = routeSegmentHashList[startKey];
					}
					var endPnt = routeSegmentHashList[startKey];
					var endDsgPnt = _designatedPointList.FirstOrDefault (item => (item as WayPoint).Designator == endingPntName);
					if (endDsgPnt == null || endDsgPnt.ID != endPnt.ChoiceId)
						resultLogs.Add (new ResultLog (feat, ReportType.Rule_Invalid, "Procedure should be ended with " + endingPntName + " point"));
				}
				return resultLogs;
			}
			List<bool> isEndList = new List<bool> ();
			bool isIntersect = false;
			List<Dictionary<SegmentPointData, SegmentPointData>> sortedRoutes = new List<Dictionary<SegmentPointData, SegmentPointData>> ();
			for (int i = 0; i < startKeys.Count; i++)
			{
				Dictionary<SegmentPointData, SegmentPointData> route = new Dictionary<SegmentPointData, SegmentPointData> ();
				var key = startKeys[i];

				isEndList.Add (false);
				//routes.Add (key);
				route.Add (key, null);
				List<ResultLog> tmpList;
				while (!isEndList[i])
				{
					if(_chartType == ChartType.Enroute)
					{
						tmpList = CheckRouteSegmentIntersection (feat, sortedRoutes, route, key);
						if (tmpList.Count > 0)
						{
							resultLogs.AddRange (tmpList);
							isIntersect = true;
						}
					}
					if (routeSegmentHashList.ContainsKey (routeSegmentHashList[key]))
					{
						route[key] = routeSegmentHashList[key];
						var key2 = routeSegmentHashList[key];
						routeSegmentHashList.Remove (key);
						key = key2;
						route.Add (key, null);
					}
					else
						isEndList[i] = true;
				}
				route[key] = routeSegmentHashList[key];
				if (_chartType == ChartType.Enroute)
				{
					tmpList = CheckRouteSegmentIntersection (feat, sortedRoutes, route, route.ElementAt (route.Count - 1).Value);
					if (tmpList.Count > 0)
					{
						resultLogs.AddRange (tmpList);
						isIntersect = true;
					}
				}
				sortedRoutes.Add (route);
			}
			if (isIntersect)
				return resultLogs;
			int routeCount = startKeys.Count / 2;
			int l = 0;
			while (l < routeCount)
			{
				var route1 = sortedRoutes[2 * l];
				var route2 = sortedRoutes[2 * l + 1];
				string start1, start2, end1, end2;
				IPoint startPnt1, startPnt2, endPnt1, endPnt2;
				PDMObject pdmObj;
				if (route1.ElementAt (0).Key.Choice == PointChoice.DesignatedPoint)
				{
					pdmObj = _designatedPointList.Find (pdm => pdm.ID == route1.ElementAt (0).Key.ChoiceId);
					start1 = (pdmObj as WayPoint).Designator;
				}
				else
				{
					pdmObj = _navaidList.Find (pdm => pdm.ID == route1.ElementAt (0).Key.ChoiceId);
					start1 = (pdmObj as NavaidSystem).Designator;
				}
				startPnt1 = pdmObj.Geo as IPoint;

				if (route1.ElementAt (route1.Count - 1).Value.Choice == PointChoice.DesignatedPoint)
				{
					pdmObj = _designatedPointList.Find (pdm => pdm.ID == route1.ElementAt (route1.Count - 1).Value.ChoiceId);
					end1 = (pdmObj as WayPoint).Designator;
				}
				else
				{
					pdmObj = _navaidList.Find (pdm => pdm.ID == route1.ElementAt (route1.Count - 1).Value.ChoiceId);
					end1 = (pdmObj as NavaidSystem).Designator;
				}
				endPnt1 = pdmObj.Geo as IPoint;

				if (route2.ElementAt (0).Key.Choice == PointChoice.DesignatedPoint)
				{
					pdmObj = _designatedPointList.Find (pdm => pdm.ID == route2.ElementAt (0).Key.ChoiceId);
					start2 = (pdmObj as WayPoint).Designator;
				}
				else
				{
					pdmObj = _navaidList.Find (pdm => pdm.ID == route2.ElementAt (0).Key.ChoiceId);
					start2 = (pdmObj as NavaidSystem).Designator;
				}
				startPnt2 = pdmObj.Geo as IPoint;

				if (route2.ElementAt (route2.Count - 1).Value.Choice == PointChoice.DesignatedPoint)
				{
					pdmObj = _designatedPointList.Find (pdm => pdm.ID == route2.ElementAt (route2.Count - 1).Value.ChoiceId);
					end2 = (pdmObj as WayPoint).Designator;
				}
				else
				{
					pdmObj = _navaidList.Find (pdm => pdm.ID == route2.ElementAt (route2.Count - 1).Value.ChoiceId);
					end2 = (pdmObj as NavaidSystem).Designator;
				}
				endPnt2 = pdmObj.Geo as IPoint;

				double dist1 = ARANFunctions.ReturnGeodesicDistance (ConvertFromEsriGeom.ToPoint (startPnt1), ConvertFromEsriGeom.ToPoint (endPnt2));
				double dist2 = ARANFunctions.ReturnGeodesicDistance (ConvertFromEsriGeom.ToPoint (startPnt2), ConvertFromEsriGeom.ToPoint (endPnt1));
				if (dist1 > dist2)
					resultLogs.Add (new ResultLog (feat, ReportType.Route_Break, "There is a break in the legs" + ".\r\n" +
					"Solution is : [" + end1 + " : " + start2 + "]"));
				else
					resultLogs.Add (new ResultLog (feat, ReportType.Route_Break, "There is a break in the legs" + ".\r\n" +
					"Solution is : [" + end2 + " : " + start1 + "]"));
				l++;
			}
			return resultLogs;
		}

		public void CheckHoldings (List<PDMObject> dataList, List<ResultLog> logList)
		{
			var holdingList = dataList.Where (pdm => pdm is PDM.HoldingPattern).ToList ();
			PDM.HoldingPattern holding;
			foreach (var item in holdingList)
			{
				holding = (PDM.HoldingPattern) item;
				if (_chartType == ChartType.Enroute)
				{
					if (holding.HoldingPoint == null || holding.HoldingPoint.SegmentPointDesignator.Trim () == "")
						logList.Add (new ResultLog (holding, ReportType.Mandatory_Field_Absent, "SegmentPointDesignator"));
					if (holding.OutboundCourse == null || double.IsNaN (holding.OutboundCourse.Value))
						logList.Add (new ResultLog (holding, ReportType.Mandatory_Field_Absent, "OutboundCourse"));
					if (holding.InboundCourse == null || double.IsNaN (holding.InboundCourse.Value))
						logList.Add (new ResultLog (holding, ReportType.Mandatory_Field_Absent, "InboundCourse"));
					if (holding.UpperLimit == null || double.IsNaN (holding.UpperLimit.Value))
						logList.Add (new ResultLog (holding, ReportType.Mandatory_Field_Absent, "UpperLimit"));
					if (holding.LowerLimit == null || double.IsNaN (holding.LowerLimit.Value))
						logList.Add (new ResultLog (holding, ReportType.Mandatory_Field_Absent, "LowerLimit"));
					if (holding.SpeedLimit == null || double.IsNaN (holding.SpeedLimit.Value))
						logList.Add (new ResultLog (holding, ReportType.Mandatory_Field_Absent, "SpeedLimit"));
					if (holding.Duration_Distance == null || double.IsNaN (holding.Duration_Distance.Value))
						logList.Add (new ResultLog (holding, ReportType.Mandatory_Field_Absent, "Duration"));
				}
				else if ( _chartType == ChartType.SID || _chartType == ChartType.Star )
				{
					if (holding.UpperLimit == null || double.IsNaN (holding.UpperLimit.Value))
						logList.Add (new ResultLog (holding, ReportType.Mandatory_Field_Absent, "UpperLimit"));
					if (holding.LowerLimit == null || double.IsNaN (holding.LowerLimit.Value))
						logList.Add (new ResultLog (holding, ReportType.Mandatory_Field_Absent, "LowerLimit"));
				}
			}
		}

		public void CheckAirspaces (List<PDMObject> dataList, List<ResultLog> logList)
		{

			var airspaceList = dataList.Where (pdm => pdm is PDM.Airspace).ToList ();
			PDM.Airspace airspace;
			foreach (var item in airspaceList)
			{
				airspace = (PDM.Airspace) item;

                if (airspace.CodeType == AirspaceType.AMA) continue;
                if (airspace.CodeType == AirspaceType.OTHER) continue;

				if (airspace.AirspaceVolumeList == null || airspace.AirspaceVolumeList.Count == 0)
					logList.Add (new ResultLog (airspace, ReportType.Mandatory_Field_Absent, "Geometry"));
				else
				{
					foreach (var volume in airspace.AirspaceVolumeList)
					{
						volume.RebuildGeo2 ();
						if (volume.Geo == null)
							logList.Add (new ResultLog (volume, ReportType.Mandatory_Field_Absent, "Geometry", airspace));
					}
				}
				if (_chartType == ChartType.Enroute)
				{
					if (airspace.CodeID == null || airspace.CodeID.Trim () == "")
                        logList.Add(new ResultLog(airspace, ReportType.Mandatory_Field_Absent, "CodeID"));
					if (airspace.AirspaceVolumeList.FirstOrDefault (arspc => arspc.ValDistVerUpper == null || !arspc.ValDistVerUpper.HasValue) != null)
                        logList.Add(new ResultLog(airspace, ReportType.Mandatory_Field_Absent, "ValDistVerUpper"));
                    if (airspace.AirspaceVolumeList.FirstOrDefault(arspc => arspc.UomValDistVerUpper.ToString().Trim() == "") != null)
                        logList.Add(new ResultLog(airspace, ReportType.Mandatory_Field_Absent, "UomDistVerUpper"));
					if (airspace.AirspaceVolumeList.FirstOrDefault (arspc => arspc.ValDistVerLower == null || !arspc.ValDistVerLower.HasValue) != null)
                        logList.Add(new ResultLog(airspace, ReportType.Mandatory_Field_Absent, "ValDistVerLower"));
                    if (airspace.AirspaceVolumeList.FirstOrDefault(arspc => arspc.UomValDistVerLower.ToString().Trim() == "") != null)
                        logList.Add(new ResultLog(airspace, ReportType.Mandatory_Field_Absent, "UomDistVerLower"));
				}
				else if ( _chartType == ChartType.SID || _chartType == ChartType.Star )
				{
					if (airspace.CodeID == null || airspace.CodeID.Trim () == "")
                        logList.Add(new ResultLog(airspace, ReportType.Mandatory_Field_Absent, "CodeID"));
					if (airspace.AirspaceVolumeList.FirstOrDefault (arspc => arspc.ValDistVerUpper == null || !arspc.ValDistVerUpper.HasValue) != null)
                        logList.Add(new ResultLog(airspace, ReportType.Mandatory_Field_Absent, "ValDistVerUpper"));
                    if (airspace.AirspaceVolumeList.FirstOrDefault(arspc => arspc.UomValDistVerUpper.ToString().Trim() == "") != null)
                        logList.Add(new ResultLog(airspace, ReportType.Mandatory_Field_Absent, "UomDistVerUpper"));
					if (airspace.AirspaceVolumeList.FirstOrDefault (arspc => arspc.ValDistVerLower == null || !arspc.ValDistVerLower.HasValue) != null)
                        logList.Add(new ResultLog(airspace, ReportType.Mandatory_Field_Absent, "ValDistVerLower"));
                    if (airspace.AirspaceVolumeList.FirstOrDefault(arspc => arspc.UomValDistVerLower.ToString().Trim() == "") != null)
                        logList.Add(new ResultLog(airspace, ReportType.Mandatory_Field_Absent, "UomDistVerLower"));
				}
			}
		}

		public List<ResultLog> CheckAttributes (Dictionary<string, List<SegmentPoint>> segmenPntList, List<PDMObject> dataList)
		{
			List<ResultLog> resultLogs = new List<ResultLog> ();
			KeyValuePair<string, List<SegmentPoint>> pair1, pair2;
			var dsgPntList = new Dictionary<string, List<SegmentPointLink>> ();
			var navaidList = new Dictionary<string, List<SegmentPointLink>> ();
            try
            {
                for (int i = 0; i < segmenPntList.Count - 1; i++)
                {
                    pair1 = segmenPntList.ElementAt(i);
                    for (int j = i + 1; j < segmenPntList.Count; j++)
                    {
                        pair2 = segmenPntList.ElementAt(j);
                        // Check compatibility ReportingATC & FlyOver attributes of start points
                        if (pair1.Value[0].PointChoice == pair2.Value[0].PointChoice && pair1.Value[0].PointChoiceID == pair2.Value[0].PointChoiceID)
                        {
                            if (pair1.Value[0].FlyOver != pair2.Value[0].FlyOver)
                            {
                                if (pair1.Value[0].PointChoice == PointChoice.DesignatedPoint)
                                    AddIntoList(dataList, pair1.Key, pair2.Key, pair1.Value[0], pair2.Value[0], AttributeEnum.FlyOver, dsgPntList);
                                else
                                    AddIntoList(dataList, pair1.Key, pair2.Key, pair1.Value[0], pair2.Value[0], AttributeEnum.FlyOver, navaidList);
                            }

                            if (pair1.Value[0].ReportingATC != pair2.Value[0].ReportingATC)
                            {
                                if (pair1.Value[0].PointChoice == PointChoice.DesignatedPoint)
                                    AddIntoList(dataList, pair1.Key, pair2.Key, pair1.Value[0], pair2.Value[0], AttributeEnum.ReportingATC, dsgPntList);
                                else if(pair1.Value[0].PointChoice == PointChoice.Navaid)
                                    AddIntoList(dataList, pair1.Key, pair2.Key, pair1.Value[0], pair2.Value[0], AttributeEnum.ReportingATC, navaidList);
                            }
                        }
                        // Check compatilibilty ReportingATC of start of first route and end of second route
                        else if (pair1.Value[0].PointChoice == pair2.Value[1].PointChoice && pair1.Value[0].PointChoiceID == pair2.Value[1].PointChoiceID)
                        {
                            if (pair1.Value[0].FlyOver != pair2.Value[1].FlyOver)
                            {
                                if(pair1.Value[0].PointChoice == PointChoice.DesignatedPoint)
                                    AddIntoList(dataList, pair1.Key, pair2.Key, pair1.Value[0], pair2.Value[1], AttributeEnum.FlyOver, dsgPntList);
                                else if(pair1.Value[0].PointChoice == PointChoice.Navaid)
                                    AddIntoList(dataList, pair1.Key, pair2.Key, pair1.Value[0], pair2.Value[1], AttributeEnum.FlyOver, navaidList);
                            }
                            if (pair1.Value[0].ReportingATC != pair2.Value[1].ReportingATC)
                            {
                                if(pair1.Value[0].PointChoice == PointChoice.DesignatedPoint)
                                    AddIntoList(dataList, pair1.Key, pair2.Key, pair1.Value[0], pair2.Value[1], AttributeEnum.ReportingATC, dsgPntList);
                                else if(pair1.Value[0].PointChoice == PointChoice.Navaid)
                                    AddIntoList(dataList, pair1.Key, pair2.Key, pair1.Value[0], pair2.Value[1], AttributeEnum.ReportingATC, navaidList);
                            }
                        }
                        // Check compatilibilty ReportingATC of end of first route and start of second route
                        else if (pair1.Value[1].PointChoice == pair2.Value[0].PointChoice && pair1.Value[1].PointChoiceID == pair2.Value[0].PointChoiceID)
                        {
                            if (pair1.Value[1].FlyOver != pair2.Value[0].FlyOver)
                            {
                                if(pair1.Value[1].PointChoice == PointChoice.DesignatedPoint)
                                    AddIntoList(dataList, pair1.Key, pair2.Key, pair1.Value[1], pair2.Value[0], AttributeEnum.FlyOver, dsgPntList);
                                else if(pair1.Value[1].PointChoice == PointChoice.Navaid)
                                    AddIntoList(dataList, pair1.Key, pair2.Key, pair1.Value[1], pair2.Value[0], AttributeEnum.FlyOver, navaidList);
                            }
                            if (pair1.Value[1].ReportingATC != pair2.Value[0].ReportingATC)
                            {
                                if (pair1.Value[1].PointChoice == PointChoice.DesignatedPoint)
                                    AddIntoList(dataList, pair1.Key, pair2.Key, pair1.Value[1], pair2.Value[0], AttributeEnum.ReportingATC, dsgPntList);
                                else if (pair1.Value[1].PointChoice == PointChoice.Navaid)
                                    AddIntoList(dataList, pair1.Key, pair2.Key, pair1.Value[1], pair2.Value[0], AttributeEnum.ReportingATC, navaidList);
                            }
                        }
                        // Check compatilibilty ReportingATC of end points
                        else if (pair1.Value[1].PointChoice == pair2.Value[1].PointChoice && pair1.Value[1].PointChoiceID == pair2.Value[1].PointChoiceID)
                        {
                            if (pair1.Value[1].FlyOver != pair2.Value[1].FlyOver)
                            {
                                if (pair1.Value[1].PointChoice == PointChoice.DesignatedPoint)
                                    AddIntoList(dataList, pair1.Key, pair2.Key, pair1.Value[1], pair2.Value[1], AttributeEnum.FlyOver, dsgPntList);
                                else if (pair1.Value[1].PointChoice == PointChoice.Navaid)
                                    AddIntoList(dataList, pair1.Key, pair2.Key, pair1.Value[1], pair2.Value[1], AttributeEnum.FlyOver, navaidList);
                            }
                            if (pair1.Value[1].ReportingATC != pair2.Value[1].ReportingATC)
                            {
                                if (pair1.Value[1].PointChoice == PointChoice.DesignatedPoint)
                                    AddIntoList(dataList, pair1.Key, pair2.Key, pair1.Value[1], pair2.Value[1], AttributeEnum.ReportingATC, dsgPntList);
                                else if (pair1.Value[1].PointChoice == PointChoice.Navaid)
                                    AddIntoList(dataList, pair1.Key, pair2.Key, pair1.Value[1], pair2.Value[1], AttributeEnum.ReportingATC, navaidList);
                            }
                        }
                    }
                }

                foreach (var item in dsgPntList)
                {
				    var dsgPnt = _designatedPointList.Where(pdm => pdm.ID == item.Key).FirstOrDefault() as WayPoint;
                    if(item.Value.Count == 0 || dsgPnt == null)
                        continue;
                    resultLogs.Add(CreateLog(dsgPnt, item));
                }

                foreach (var item in navaidList)
                {
                    var navaid = _navaidList.Where(pdm => pdm.ID == item.Key).FirstOrDefault() as PDM.NavaidSystem;
                    if (item.Value.Count == 0 ||  navaid == null) continue;
                    resultLogs.Add(CreateLog(navaid, item));
                }
            }
            catch (Exception ex)
            {
				string t = ex.StackTrace;
				throw ex;
            }
			return resultLogs;
		}

        private ResultLog CreateLog(PDMObject pdm, KeyValuePair<string, List<SegmentPointLink>> item)
        {
            ResultLog log = new ResultLog(pdm, ReportType.WayPoint_Attribute);                        
            string description;
            foreach (var item2 in item.Value)
            {
                foreach (var root in item2.SegmntPntTrees)
                {
                    description = "";
                    SegmentPoint lastSegPntChild = null;
                    if (_chartType == ChartType.Enroute)
                    {
                        description = root.Item.Value.ToString() + " => " +
                            root.Item.Child.Value.ToString() +
                              "   " + root.SegmentType;
                    }
                    else if (_chartType == ChartType.SID || _chartType == ChartType.Star)
                    {
                        description = root.Item.Value.ToString() + " => " +
                            (root.Item.Child.Value as ProcedureTransitions).GetObjectLabel() + " => " +
                              root.Item.Child.Child.Value.ToString() + "(" +
                              (root.Item.Child.Child.Value as ProcedureLeg).SeqNumberARINC + ") => " + root.SegmentType;
                    }                    
                    if (root.SegmentType == SegmentPointType.Start)
                    {
                        if (_chartType == ChartType.Enroute)
                            lastSegPntChild = (root.Item.Child.Value as PDM.RouteSegment).StartPoint;
                        else if (_chartType == ChartType.SID || _chartType == ChartType.Star)
                            lastSegPntChild = (root.Item.Child.Child.Value as PDM.ProcedureLeg).StartPoint;
                    }
                    else
                    {
                        if (_chartType == ChartType.Enroute)
                            lastSegPntChild = (root.Item.Child.Value as PDM.RouteSegment).EndPoint;
                        else if (_chartType == ChartType.SID || _chartType == ChartType.Star)
                            lastSegPntChild = (root.Item.Child.Child.Value as PDM.ProcedureLeg).EndPoint;
                    }
                    if (item2.AttributeIndex == AttributeEnum.ReportingATC)
                        description += " " + lastSegPntChild.ReportingATC;
                    else
                        description += " " + lastSegPntChild.FlyOver;
                    log.DescriptionList.Add(description);
                }
                log.DescriptionList.Sort();
                if (item2.AttributeIndex == AttributeEnum.FlyOver)
                    log.DescriptionList.Insert(0, "Conflicting attribute : FlyOver ");
                else if (item2.AttributeIndex == AttributeEnum.ReportingATC)
                    log.DescriptionList.Insert(0, "Conflicting attribute : ReportingATC ");

                log.GetReportText();
            }
            return log;
        }

        private void AddIntoList(List<PDMObject> dataList, string key1, string key2, SegmentPoint segPnt1, SegmentPoint segPnt2,
    AttributeEnum attributeIndex, Dictionary<string, List<SegmentPointLink>> resultDict)
        {
            try
            {
                SegmentPointLink foundItems;
                byte equalItems;
                if (!resultDict.ContainsKey(segPnt1.PointChoiceID))
                    resultDict.Add(segPnt1.PointChoiceID, new List<SegmentPointLink>());
                foundItems = FindSegPntLink(key1, key2, segPnt1, dataList, attributeIndex);
                var savedItems = resultDict[segPnt1.PointChoiceID].FirstOrDefault(pdm => pdm.AttributeIndex == attributeIndex);

                if (savedItems == null)
                    resultDict[segPnt1.PointChoiceID].Add(foundItems);
                else
                {
                    TreeItem foundItem, savedItem;
                    for (int k = 0; k < foundItems.SegmntPntTrees.Count; k++)
                    {
                        //var foundItem = foundSegmntPnt.SegmntPntTrees[k].Item;
                        bool contains = false;
                        for (int i = 0; i < savedItems.SegmntPntTrees.Count; i++)
                        {
                            foundItem = foundItems.SegmntPntTrees[k].Item;
                            savedItem = savedItems.SegmntPntTrees[i].Item;
                            equalItems = 0;
                            do
                            {
                                if (savedItem.Value.ID != foundItem.Value.ID)
                                    break;
                                else
                                    equalItems++;
                                savedItem = savedItem.Child;
                                foundItem = foundItem.Child;
                            }
                            while (savedItem != null && foundItem != null);
                            if (equalItems == foundItems.SegmntPntTrees[k].Item.ChildCount + 1)
                            {
                                contains = true;
                                break;
                            }
                        }
                        if (!contains)
                        {
                            PDMSegmentPntTree result = new PDMSegmentPntTree(foundItems.SegmntPntTrees[k]);
                            savedItems.SegmntPntTrees.Add(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private SegmentPointLink FindSegPntLink(string routeSegId1, string routeSegId2, PDM.SegmentPoint segmntPnt, List<PDMObject> dataList, AttributeEnum attributeIndex)
        {
            SegmentPointLink result = new SegmentPointLink(attributeIndex);
            try
            {
                if (_chartType == ChartType.Enroute)
                {
                    foreach (var enroute in dataList)
                    {
                        foreach (var routeSegment in (enroute as PDM.Enroute).Routes)
                        {
                            if (routeSegment.ID == routeSegId1 || routeSegment.ID == routeSegId2)
                            {
                                var item = new PDMSegmentPntTree(enroute);
                                item.Item.Child = new TreeItem(routeSegment);
                                if (routeSegment.StartPoint.PointChoiceID == segmntPnt.PointChoiceID)
                                    item.SegmentType = SegmentPointType.Start;
                                else if (routeSegment.EndPoint.PointChoiceID == segmntPnt.PointChoiceID)
                                    item.SegmentType = SegmentPointType.End;
                                result.SegmntPntTrees.Add(item);
                            }
                        }
                    }
                }
                else if(_chartType == ChartType.SID || _chartType == ChartType.Star)
                {
                    foreach (var proc in dataList)
                    {
                        List<ProcedureTransitions> transitions = null;
                        if(_chartType == ChartType.SID)
                            transitions = (proc as StandardInstrumentDeparture).Transitions;
                        else if(_chartType == ChartType.Star)
                            transitions = (proc as StandardInstrumentArrival).Transitions;
                        foreach (var transition in transitions)
                        {
                            foreach (var leg in transition.Legs)
                            {
                                if(leg.ID == routeSegId1 || leg.ID == routeSegId2)
                                {
                                    var root = new PDMSegmentPntTree(proc);
                                    root.Item.Child = new TreeItem(transition);
                                    root.Item.Child.Child = new TreeItem(leg);
                                    if (leg.StartPoint.PointChoiceID == segmntPnt.PointChoiceID)
                                        root.SegmentType = SegmentPointType.Start;
                                    else if (leg.EndPoint.PointChoiceID == segmntPnt.PointChoiceID)
                                        root.SegmentType = SegmentPointType.End;
                                    result.SegmntPntTrees.Add(root);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

		private List<ResultLog> CheckRouteSegmentIntersection (PDMObject feat, List<Dictionary<SegmentPointData, SegmentPointData>> sortedRoutes,
Dictionary<SegmentPointData, SegmentPointData> route, SegmentPointData value)
		{
			List<ResultLog> resultLogs = new List<ResultLog> ();
			foreach (var item in sortedRoutes)
			{
				var keyItem = item.FirstOrDefault (pair => pair.Key == value);
				var valueItem = item.FirstOrDefault (pair => pair.Value == value);
				if (keyItem.Key != null && keyItem.Value != null)
				{
					List<string> idList = new List<string> ();
					KeyValuePair<SegmentPointData, SegmentPointData> pair = item.FirstOrDefault (item2 => item2.Value == keyItem.Key);
					while (pair.Key != null && keyItem.Value != null)
					{
						idList.Add (pair.Key.Description);
						pair = item.FirstOrDefault (item2 => item2.Value == pair.Key);
					}
					ResultLog log = new ResultLog (feat, ReportType.Route_Intersects);
					List<string> idList1 = new List<string> ();
					int k = 0;
					do
					{
						idList1.Add (route.ElementAt (k).Key.Description);
						k++;
					} while (k < route.Count - 1);
					log.ConflictedRouteSegments.Add (idList1);
					log.ConflictedRouteSegments.Add (idList);
					resultLogs.Add (log);
				}
				else if (valueItem.Key != null && valueItem.Value != null)
				{
				}
			}
			return resultLogs;
		}
	}
}
