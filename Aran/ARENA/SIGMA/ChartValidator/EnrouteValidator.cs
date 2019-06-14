using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.PANDA.Common;
//using ARENA;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geometry;
using PDM;

namespace ChartValidator
{
	public class EnrouteValidator
	{
		private List<PDMObject> AllDesignatedPointList;
		private List<PDMObject> AllNavaidList;
		private const double segmentPntTolerance = 0.00028;
		private const double bearingTolerance = 1;
		private const double lengthToleranceInM = 1000;
		private const double lengthToleranceInNM = 1850;
		private const double isNullInt = -999;		
		private IApplication _esriApp;
		private MainForm _mainForm;

		public EnrouteValidator (IApplication application)
		{
			_esriApp = application;
			_mainForm = new MainForm (ChartType.Enroute);
		}

		public void AddDestroyEventHandler (EventHandler eventHandler)
		{
			_mainForm.HandleDestroyed += eventHandler;
		}

		/// <summary>
		/// Checks data for enroute chart and report about non valid data
		/// </summary>
		/// <param name="dataList">Data to check</param>
		/// <param name="errorMessage">Exception message if result is 2</param>
		/// <returns>Returns 0 if everything is ok
		/// Returns 1 if there was any not valid data
		/// Returns 2 if there was an exception
		/// </returns>
		public byte Check(List<PDMObject> dataList, out string errorMessage)
		{
			errorMessage = "";
			try
			{
				List<ResultLog> logList = new List<ResultLog> ();
				var enrouteList = dataList.Where (pdm => pdm is Enroute).ToList ();
				Enroute enroute;
				var segmenPntList = new Dictionary<string, List<PDM.SegmentPoint>> ();
				AllDesignatedPointList = dataList.Where (pdm => pdm is PDM.WayPoint).ToList ();
				//AllDesignatedPointList.Sort ((dsg1, dsg2) => (dsg1 as WayPoint).Designator.CompareTo ((dsg2 as WayPoint).Designator));
				AllNavaidList = dataList.Where (pdm => pdm is PDM.NavaidSystem).ToList ();
				CommonValidator commonValidator = new CommonValidator (AllDesignatedPointList, AllNavaidList, ChartType.Enroute);
				Dictionary<SegmentPointData, SegmentPointData> routeSgmntPntDict = new Dictionary<SegmentPointData, SegmentPointData> ();
				ARANFunctions.InitEllipsoid();
				double trueBearing, reverseBearing, tolerance, length;
				for (int i = 0; i < enrouteList.Count; i++)
				{
					enroute = enrouteList[i] as Enroute;
					if (string.IsNullOrEmpty (enroute.TxtDesig))
						logList.Add (new ResultLog (enroute, ReportType.Mandatory_Field_Absent, "TxtDesig"));
					if (enroute.Routes == null || enroute.Routes.Count == 0)
						logList.Add (new ResultLog (enroute, ReportType.No_RouteSegment));
					else if (enroute.Routes.Count == 1)
						logList.Add (new ResultLog (enroute, ReportType.One_RouteSegment));
					else
					{
						routeSgmntPntDict.Clear ();
						foreach (var routeSegment in enroute.Routes)
						{							
							length = 0;
							trueBearing = double.NaN;
							reverseBearing = double.NaN;

							#region Check Start Point Link Valiation
							if (routeSegment.StartPoint == null || routeSegment.StartPoint.PointChoiceID == null)
								logList.Add (new ResultLog (routeSegment, ReportType.No_SegmentPoint, "Start Point", enroute));
							else
							{
								if (!segmenPntList.ContainsKey (routeSegment.ID))
									segmenPntList.Add (routeSegment.ID, new List<PDM.SegmentPoint> ());
								segmenPntList[routeSegment.ID].Add (routeSegment.StartPoint);
								switch (routeSegment.StartPoint.PointChoice)
								{
									case PointChoice.DesignatedPoint:
										commonValidator.CheckDsgPoint(logList, routeSegment.StartPoint, routeSegment, enroute);
										break;
									case PointChoice.Navaid:
										commonValidator.CheckNavaid (logList, routeSegment.StartPoint, routeSegment.EndPoint, routeSegment, enroute);
										break;
									default:
										throw new Exception ("Point Choice - " + routeSegment.StartPoint.PointChoice +
											" (" + enroute.ToString () + " => " + routeSegment.ToString () + " => StartPoint ) is not implemented");
								}
							}
							#endregion

							#region Check End Point Link Validation
							if (routeSegment.EndPoint == null || routeSegment.EndPoint.PointChoiceID == null)
								logList.Add (new ResultLog (routeSegment, ReportType.No_SegmentPoint, "End Point", enroute));
							else
							{
								if (!segmenPntList.ContainsKey (routeSegment.ID))
									segmenPntList.Add (routeSegment.ID, new List<PDM.SegmentPoint> ());
								segmenPntList[routeSegment.ID].Add (routeSegment.EndPoint);
								switch (routeSegment.EndPoint.PointChoice)
								{
									case PointChoice.DesignatedPoint:
										commonValidator.CheckDsgPoint (logList, routeSegment.EndPoint, routeSegment, enroute, false);
										break;
									case PointChoice.Navaid:
										commonValidator.CheckNavaid (logList, routeSegment.StartPoint, routeSegment.EndPoint, routeSegment, enroute);
										break;

									default:
										throw new Exception ("Point Choice - " + routeSegment.StartPoint.PointChoice +
											" (" + enroute.ToString () + " => " + routeSegment.ToString () + " => EndPoint ) is not implemented");
								}
							}
							#endregion

							if (!segmenPntList.ContainsKey (routeSegment.ID))
								continue;

							if (segmenPntList[routeSegment.ID].Count == 2)
							{
								logList.AddRange (CheckGeometryConflicts (segmenPntList, routeSegment, enroute, ref length, ref trueBearing, ref reverseBearing));

								if (routeSegment.CodeDir == CODE_ROUTE_SEGMENT_DIR.BOTH)
								{
									CheckTrueTracks (logList, trueBearing, routeSegment, enroute);
									CheckReverseTracks (logList, reverseBearing, routeSegment, enroute);
								}
								else if (routeSegment.CodeDir == CODE_ROUTE_SEGMENT_DIR.FORWARD)
								{
									CheckTrueTracks (logList, trueBearing, routeSegment, enroute);
								}
								else if (routeSegment.CodeDir == CODE_ROUTE_SEGMENT_DIR.BACKWARD)
								{
									CheckReverseTracks (logList, reverseBearing, routeSegment, enroute);
								}
								else if (routeSegment.CodeDir == CODE_ROUTE_SEGMENT_DIR.OTHER)
								{
									logList.Add (new ResultLog (routeSegment, ReportType.Mandatory_Field_Absent, "CodeDir", enroute));
								}

								var prevInsertedKey = routeSgmntPntDict.FirstOrDefault (
										 (item => item.Value.Choice == routeSegment.StartPoint.PointChoice &&
											 item.Value.ChoiceId == routeSegment.StartPoint.PointChoiceID));
								var prevInsertValue = routeSgmntPntDict.FirstOrDefault (
										 (item => item.Key.Choice == routeSegment.EndPoint.PointChoice &&
											 item.Key.ChoiceId == routeSegment.EndPoint.PointChoiceID));
								SegmentPointData key, value;
								if (prevInsertedKey.Key == null && prevInsertedKey.Value == null)
									key = new SegmentPointData (routeSegment.StartPoint.PointChoice, routeSegment.StartPoint.PointChoiceID,
										/*routeSegment.ID, */routeSegment.ToString ());
								else
								{
									key = prevInsertedKey.Value;
									//key.RouteSegmentId = routeSegment.ID;
									key.Description = routeSegment.ToString ();
								}

								if (prevInsertValue.Key == null && prevInsertValue.Value == null)
									value = new SegmentPointData (routeSegment.EndPoint.PointChoice, routeSegment.EndPoint.PointChoiceID,
											/*routeSegment.ID, */routeSegment.ToString ());
								else
								{
									value = prevInsertValue.Key;
									//value.RouteSegmentId = routeSegment.ID;
									//value.RouteSegmentDsg = routeSegment.ToString ();
								}
								if (routeSgmntPntDict.ContainsKey (key) && routeSgmntPntDict.ContainsValue (value))
									logList.Add (new ResultLog (routeSegment, ReportType.Duplicate, enroute));
								routeSgmntPntDict.Add (key, value);

								if (routeSegment.ValLen != null || routeSegment.ValLen.HasValue)
								{
									var lengthInPdm = routeSegment.ConvertValueToMeter (routeSegment.ValLen, routeSegment.UomValLen.ToString ());
									double lengthValue = double.MinValue;
									tolerance = lengthToleranceInM;
									switch (routeSegment.UomValLen)
									{
										case UOM_DIST_HORZ.M:
											lengthValue = length;
											break;

										case UOM_DIST_HORZ.NM:
											tolerance = lengthToleranceInNM;
											lengthValue = length / 1852;
											break;
										
										case UOM_DIST_HORZ.KM:
											lengthValue = length / 1000;
											break;
										
										case UOM_DIST_HORZ.FT:
											lengthValue = length * 3.28084;
											break;
										
										default:
											break;
									}

									if (Math.Abs (lengthInPdm.Value - length) > tolerance)
									{
										logList.Add (new ResultLog (routeSegment, ReportType.Length_inappropirate,
											Math.Round (lengthValue, 1) + "/" + routeSegment.ValLen + " " + routeSegment.UomValLen, enroute));
									}
								}
								else
								{
									length = length / 1000;
									logList.Add (new ResultLog (routeSegment, ReportType.Length_inappropirate,
										Math.Round (length, 1).ToString () + "/-- KM", enroute));
								}
							}
							else 
								segmenPntList.Remove(routeSegment.ID);

							if (routeSegment.ValDistVerUpper == null || !routeSegment.ValDistVerUpper.HasValue)
                                logList.Add(new ResultLog(routeSegment, ReportType.Mandatory_Field_Absent, "ValDistVerUpper", enroute));
							if (routeSegment.ValDistVerLower == null || !routeSegment.ValDistVerLower.HasValue)
                                logList.Add(new ResultLog(routeSegment, ReportType.Mandatory_Field_Absent, "ValDistVerLower", enroute));
						}
						logList.AddRange (commonValidator.CheckRouteSegmentsBreak ((enroute as Enroute), routeSgmntPntDict));
					}
				}

				commonValidator.CheckAirspaces (dataList, logList);

				commonValidator.CheckHoldings (dataList, logList);

				logList.AddRange (commonValidator.CheckAttributes (segmenPntList, enrouteList));
				
				logList = logList.OrderBy (item => item.Type).ThenBy (item => item.Feature.PDM_Type).ToList ();
				//WriteToFile (logList, enrouteList, pathReport);

				if (logList.Count == 0 )					
				//logList.FirstOrDefault (log => log.Type == ResultType.Error) == null))
					return 0;
				else
				{					
					_mainForm.SetData (logList, dataList, _esriApp);
					var res = _mainForm.ShowDialog (new Win32Windows (_esriApp.hWnd));
                    if (res == DialogResult.Cancel) return 4;
                    else return 1;
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return 2; 
			}
		}

		private void CheckReverseTracks (List<ResultLog> logList, double reverseBearing, PDM.RouteSegment routeSegment, PDM.Enroute enroute)
		{
			bool isMagChecked = false;
			if (routeSegment.ValReversTrueTrack == null || double.IsNaN (routeSegment.ValReversTrueTrack.Value))
				logList.Add (new ResultLog (routeSegment, ReportType.ReverseBearing_inappropriate, Math.Round (reverseBearing, 1).ToString () + "/--", enroute));
			else if (/*routeSegment.ValReversTrueTrack != isNullInt && */ Math.Abs (reverseBearing - routeSegment.ValReversTrueTrack.Value) > bearingTolerance)
				logList.Add (new ResultLog (routeSegment, ReportType.ReverseBearing_inappropriate,
					Math.Round (reverseBearing, 1).ToString () + "/" + routeSegment.ValReversTrueTrack.ToString (), enroute));
			else
			{
				if (routeSegment.EndPoint.Geo == null)
					routeSegment.EndPoint.RebuildGeo ();
				double lat = (routeSegment.EndPoint.Geo as IPoint).Y;
				double lon = (routeSegment.EndPoint.Geo as IPoint).X;
				double time = routeSegment.EndPoint.ActualDate.Year + routeSegment.EndPoint.ActualDate.DayOfYear / 365.0;
				//magVar = isogonFunction.wmm (lat, lon, routeSegment.ConvertValueToMeter (routeSegment.EndPoint.Elev, routeSegment.EndPoint.Elev_UOM.ToString ()),
				//	time);
				double? altitude = routeSegment.ConvertValueToMeter (routeSegment.EndPoint.Elev, routeSegment.EndPoint.Elev_UOM.ToString ());
				if (!altitude.HasValue)
					altitude = 0;
				double magVar = ExternalMagVariation.MagVar (lat, lon, altitude.Value,
routeSegment.ActualDate.Day, routeSegment.ActualDate.Month, routeSegment.ActualDate.Year, 1);
				if (routeSegment.ValReversMagTrack == null || double.IsNaN (routeSegment.ValReversMagTrack.Value))
				{
					logList.Add (new ResultLog (routeSegment, ReportType.ReverseMag_inappropriate,
						   ARANMath.Modulus (Math.Round (routeSegment.ValReversTrueTrack.Value - magVar, 1)).ToString () + "/--", enroute));
					isMagChecked = true;
				}
				else if (routeSegment.ValReversTrueTrack != isNullInt && Math.Abs (routeSegment.ValReversMagTrack.Value - ARANMath.Modulus (routeSegment.ValReversTrueTrack.Value - magVar)) > bearingTolerance)
					logList.Add (new ResultLog (routeSegment, ReportType.ReverseMag_inappropriate,
						ARANMath.Modulus (Math.Round (routeSegment.ValReversTrueTrack.Value - magVar, 1)) + "/" + routeSegment.ValReversMagTrack.Value, enroute));
			}
			if (!isMagChecked)
			{
				if (routeSegment.ValReversMagTrack == null || double.IsNaN (routeSegment.ValReversMagTrack.Value))
				{
					logList.Add (new ResultLog (routeSegment, ReportType.Mandatory_Field_Absent, "ReverseMagTrack", enroute));
				}
			}
		}

		private void CheckTrueTracks (List<ResultLog> logList, double trueBearing, PDM.RouteSegment routeSegment, PDM.Enroute enroute)
		{
			bool isMagChecked = false;
			if (routeSegment.ValTrueTrack == null || double.IsNaN (routeSegment.ValTrueTrack.Value))
				logList.Add (new ResultLog (routeSegment, ReportType.Bearing_inappropriate, Math.Round (trueBearing, 1).ToString () + "/--", enroute));
			else if (/*routeSegment.ValTrueTrack != isNullInt && */ Math.Abs (trueBearing - routeSegment.ValTrueTrack.Value) > bearingTolerance)
				logList.Add (new ResultLog (routeSegment, ReportType.Bearing_inappropriate,
					Math.Round (trueBearing, 1) + " / " + routeSegment.ValTrueTrack.Value, enroute));
			else
			{
				if (routeSegment.StartPoint.Geo == null)
					routeSegment.StartPoint.RebuildGeo ();
				double lat = (routeSegment.StartPoint.Geo as IPoint).Y;
				double lon = (routeSegment.StartPoint.Geo as IPoint).X;
				double time = routeSegment.StartPoint.ActualDate.Year + routeSegment.StartPoint.ActualDate.DayOfYear / 365.0;
				//magVar = isogonFunction.wmm (lat, lon, routeSegment.ConvertValueToMeter (routeSegment.StartPoint.Elev, routeSegment.StartPoint.Elev_UOM.ToString ()), time);									

				double? altitude = routeSegment.ConvertValueToMeter (routeSegment.StartPoint.Elev, routeSegment.StartPoint.Elev_UOM.ToString ());
				if (!altitude.HasValue)
					altitude = 0;

				double magVar = ExternalMagVariation.MagVar (lat, lon, altitude.Value,
					routeSegment.ActualDate.Day, routeSegment.ActualDate.Month, routeSegment.ActualDate.Year, 1);

				//if(!isMagChecked)
				if (routeSegment.ValMagTrack == null || double.IsNaN (routeSegment.ValMagTrack.Value))
				{
					logList.Add (new ResultLog (routeSegment, ReportType.Mandatory_Field_Absent,
						   ARANMath.Modulus (Math.Round (routeSegment.ValTrueTrack.Value - magVar, 1)).ToString () + "/--", enroute));
					isMagChecked = true;
				}
				else if (routeSegment.ValMagTrack != isNullInt && Math.Abs (routeSegment.ValMagTrack.Value - ARANMath.Modulus (routeSegment.ValTrueTrack.Value - magVar)) > bearingTolerance)
					logList.Add (new ResultLog (routeSegment, ReportType.MagTrack_inappropriate,
						ARANMath.Modulus (Math.Round (routeSegment.ValTrueTrack.Value - magVar, 1)) + "/" + routeSegment.ValMagTrack.Value, enroute));
			}
			if (!isMagChecked)
			{
				if (routeSegment.ValMagTrack == null || double.IsNaN (routeSegment.ValMagTrack.Value))
				{
					logList.Add (new ResultLog (routeSegment, ReportType.Mandatory_Field_Absent, "MagTrack", enroute));
				} 
			}
		}

		private List<ResultLog> CheckGeometryConflicts (Dictionary<string, List<PDM.SegmentPoint>> segmenPntList, PDM.RouteSegment routeSegment, PDM.Enroute enroute,
			ref double length, ref double true_bearing, ref double reverse_bearing)
		{
			List<ResultLog> resultLogs = new List<ResultLog> ();
			true_bearing = double.NaN;
			reverse_bearing = double.NaN;
			if (routeSegment.Geo == null)
				routeSegment.RebuildGeo ();
			Polyline curve = routeSegment.Geo as Polyline;
			if (curve == null || curve.PointCount != 2)
				resultLogs.Add (new ResultLog (routeSegment, ReportType.Geometry, enroute));
			else
			{
				IPoint curveStartPnt, curveEndPnt, startSegmenPnt = null, endSegmentPnt = null;
				curveStartPnt = curve.get_Point (0);
				curveEndPnt = curve.get_Point (1);

				if (segmenPntList[routeSegment.ID][0].PointChoice == PointChoice.DesignatedPoint)
				{
					var dsgPnt = AllDesignatedPointList.FirstOrDefault (pdm => pdm.ID == segmenPntList[routeSegment.ID][0].PointChoiceID);
					if (dsgPnt != null)
					{
						if (dsgPnt.Geo == null)
							dsgPnt.RebuildGeo ();
						startSegmenPnt = (dsgPnt.Geo as IPoint);
					}
				}
				else if (segmenPntList[routeSegment.ID][0].PointChoice == PointChoice.Navaid)
				{
					var navaid = AllNavaidList.FirstOrDefault (pdm => pdm.ID == segmenPntList[routeSegment.ID][0].PointChoiceID);
					if (navaid != null)
					{
						var navComp = (navaid as NavaidSystem).Components[0];
						if (navComp.Geo == null)
							navComp.RebuildGeo ();
						startSegmenPnt = (navComp.Geo as IPoint);
					}
				}

				if (segmenPntList[routeSegment.ID][1].PointChoice == PointChoice.DesignatedPoint)
				{
					var dsgPnt = AllDesignatedPointList.FirstOrDefault (pdm => pdm.ID == segmenPntList[routeSegment.ID][1].PointChoiceID);
					if (dsgPnt != null)
					{
						if (dsgPnt.Geo == null)
							dsgPnt.RebuildGeo ();
						endSegmentPnt = (dsgPnt.Geo as IPoint);
					}
				}
				else if (segmenPntList[routeSegment.ID][1].PointChoice == PointChoice.Navaid)
				{
					var navaid = AllNavaidList.FirstOrDefault (pdm => pdm.ID == segmenPntList[routeSegment.ID][1].PointChoiceID);
					if (navaid != null)
					{
						var navComp = (navaid as NavaidSystem).Components[0];
						if (navComp.Geo == null)
							navComp.RebuildGeo ();
						endSegmentPnt = (navComp.Geo as IPoint);
					}
				}

				if (startSegmenPnt != null && endSegmentPnt != null)
				{
					var startPnt = ConvertFromEsriGeom.ToPoint (startSegmenPnt);
					var endPnt = ConvertFromEsriGeom.ToPoint (endSegmentPnt);
					length = ARANFunctions.ReturnGeodesicDistance (startPnt, endPnt);
					ARANFunctions.ReturnGeodesicAzimuth (startPnt, endPnt, out true_bearing, out reverse_bearing);
					if (Math.Abs (curveStartPnt.X - startSegmenPnt.X) > segmentPntTolerance ||
						Math.Abs (curveStartPnt.Y - startSegmenPnt.Y) > segmentPntTolerance)
					{
						if (Math.Abs (curveStartPnt.X - endSegmentPnt.X) > segmentPntTolerance ||
							Math.Abs (curveStartPnt.Y - endSegmentPnt.Y) > segmentPntTolerance)
							resultLogs.Add (new ResultLog (routeSegment, ReportType.Geometry_Conflict, enroute));
					}

					if (Math.Abs (curveEndPnt.X - endSegmentPnt.X) > segmentPntTolerance ||
						Math.Abs (curveEndPnt.Y - endSegmentPnt.Y) > segmentPntTolerance)
					{
						if (Math.Abs (curveEndPnt.X - startSegmenPnt.X) > segmentPntTolerance ||
							Math.Abs (curveEndPnt.Y - startSegmenPnt.Y) > segmentPntTolerance)
							resultLogs.Add (new ResultLog (routeSegment, ReportType.Geometry_Conflict, enroute));
					}
				}
			}
			return resultLogs;
		}
	}
}