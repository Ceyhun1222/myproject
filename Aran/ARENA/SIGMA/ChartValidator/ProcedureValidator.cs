using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Converters;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geometry;
using PDM;
using System.Windows.Forms;

namespace ChartValidator
{
	public class ProcedureValidator
	{
		private IApplication _esriApp;
		private MainForm _mainForm;
		private List<PDMObject> AllDesignatedPointList;
		private List<PDMObject> AllNavaidList;
		
		private const double bearingTolerance = 0.1;
		private ChartType _chartType;

		public ProcedureValidator (IApplication application, ChartType chartType)
		{
			_chartType = chartType;
			_esriApp = application;
			_mainForm = new MainForm ( chartType );
		}

		public void AddDestroyEventHandler (EventHandler eventHandler)
		{
			_mainForm.HandleDestroyed += eventHandler;
		}

		/// <summary>
		/// Checks data for SID chart and report about non valid data
		/// </summary>
		/// <param name="dataList">Data to check</param>
		/// <param name="errorMessage">Exception message if result is 2</param>
		/// <returns>Returns 0 if everything is ok
		/// Returns 1 if there was any not valid data
		/// Returns 2 if there was an exception
		/// </returns>
		public byte Check (List<PDMObject> dataList, out string errorMessage)
		{
			errorMessage = "";
			int index = 0;
			try
			{
				List<ResultLog> logList = new List<ResultLog> ();
				var segmenPntList = new Dictionary<string, List<PDM.SegmentPoint>> ();
				AllDesignatedPointList = dataList.Where (pdm => pdm is PDM.WayPoint).ToList ();
				//AllDesignatedPointList.Sort ((dsg1, dsg2) => (dsg1 as WayPoint).Designator.CompareTo ((dsg2 as WayPoint).Designator));
				AllNavaidList = dataList.Where (pdm => pdm is PDM.NavaidSystem).ToList ();
                CommonValidator commonValidator = new CommonValidator(AllDesignatedPointList, AllNavaidList, _chartType);
				List<PDMObject> procList = null;
				if ( _chartType == ChartType.SID )
					procList = dataList.Where ( pdm => pdm is StandardInstrumentDeparture ).ToList ( );
				else if ( _chartType == ChartType.Star )
					procList = dataList.Where ( pdm => pdm is StandardInstrumentArrival ).ToList ( );
				else if ( _chartType == ChartType.IAP )
					procList = dataList.Where ( pdm => pdm is InstrumentApproachProcedure ).ToList ( );
				var airport = dataList.FirstOrDefault (pdm => pdm is AirportHeliport) as AirportHeliport;
				StandardInstrumentDeparture sid = null;
				StandardInstrumentArrival star = null;
				InstrumentApproachProcedure iap = null;
				List<ProcedureLeg> legList = new List<ProcedureLeg> ();
				Dictionary<SegmentPointData, SegmentPointData> segmntPntDict = new Dictionary<SegmentPointData, SegmentPointData> ();
				string procDsg;
                ProcedureLeg leg;
				PDMObject proc;
				foreach (var item in procList)
				{
                    if (_chartType == ChartType.SID)
                    {
                        sid = (StandardInstrumentDeparture)item;
                        proc = sid;
                        if (string.IsNullOrEmpty(sid.Designator))
                        {
                            logList.Add(new ResultLog(sid, ReportType.Mandatory_Field_Absent, "Designator"));
                            procDsg = "";
                        }
                        else
                            procDsg = sid.Designator.Length > 5 ? sid.Designator.Substring(0, 5) : sid.Designator;
                    }
                    else if (_chartType == ChartType.IAP)
                    {
                        iap = (InstrumentApproachProcedure)item;
                        proc = iap;
                        if (string.IsNullOrEmpty(iap.ProcedureIdentifier))
                        {
                            logList.Add(new ResultLog(iap, ReportType.Mandatory_Field_Absent, "ProcedureIdentifier"));
                            procDsg = "";
                        }
                        else
                            procDsg = iap.ProcedureIdentifier.Length > 20 ? iap.ProcedureIdentifier.Substring(0, 20) : iap.ProcedureIdentifier;
                    }
                    else
                    {
                        star = (StandardInstrumentArrival)item;
                        proc = star;
                        if (string.IsNullOrEmpty(star.Designator))
                        {
                            logList.Add(new ResultLog(star, ReportType.Mandatory_Field_Absent, "Designator"));
                            procDsg = "";
                        }
                        else
                            procDsg = star.Designator.Length > 5 ? star.Designator.Substring(0, 5) : star.Designator;
                    }
					
					//if ( string.IsNullOrEmpty ( sid.Designator ) )
					//{
					//	logList.Add (new ResultLog (sid, ReportType.Mandatory_Field_Absent, "Designator"));
					//	procDsg = "";
					//}
					//else
					//	procDsg = sid.Designator.Substring (0, 5) ;

					legList.Clear ();
					segmntPntDict.Clear ();
					List<ProcedureTransitions> transitions = null;
					
					if ( _chartType == ChartType.SID && sid.Transitions != null )
						transitions = sid.Transitions;
					else if ( _chartType == ChartType.Star && star.Transitions != null )
						transitions = star.Transitions;
                    else if (_chartType == ChartType.IAP && iap.Transitions != null)
                        transitions = iap.Transitions;

                    if ( transitions != null )
					{
						foreach ( var transition in transitions )
						{
							if ( transition.Legs != null )
							{
								for ( int i = 0; i < transition.Legs.Count; i++ )
								{
									leg = transition.Legs[ i ];

									CheckMagneticCourse ( logList, leg );
									CheckLength ( logList, leg );

									#region Check Start Point Link Valiation
									if ( leg.StartPoint == null || leg.StartPoint.PointChoiceID == null )
									{
										if ( i > 0 && transition.Legs[ i - 1 ].LegTypeARINC.ToString ( ).EndsWith ( "F" ) &&
												transition.Legs[ i - 1 ].LegTypeARINC != CodeSegmentPath.IF )
											logList.Add ( new ResultLog ( leg, ReportType.No_SegmentPoint, "Start Point", proc ) );
									}
									else
									{
										if ( !segmenPntList.ContainsKey ( leg.ID ) )
											segmenPntList.Add ( leg.ID, new List<SegmentPoint> ( ) );
										segmenPntList[ leg.ID ].Add ( leg.StartPoint );
										switch ( leg.StartPoint.PointChoice )
										{
											case PointChoice.DesignatedPoint:
												commonValidator.CheckDsgPoint ( logList, leg.StartPoint, leg, proc );
												break;
											case PointChoice.Navaid:
												commonValidator.CheckNavaid ( logList, leg.StartPoint, leg.EndPoint, leg, proc );
												break;
                                            case PointChoice.RunwayCentrelinePoint:
                                                commonValidator.CheckDsgPoint(logList, leg.StartPoint, leg, proc);
                                                break;
                                            default:
													throw new Exception ( "Point Choice - " + leg.StartPoint.PointChoice +
														" (" + procDsg + " => " + leg.ToString ( ) + " => StartPoint ) is not implemented" );
										}
									}
									#endregion

									#region Check End Point Link Validation
									if ( leg.EndPoint == null || leg.EndPoint.PointChoiceID == null )
									{
										if ( leg.LegTypeARINC.ToString ( ).EndsWith ( "F" ) || leg.LegTypeARINC == CodeSegmentPath.CD ||
											leg.LegTypeARINC == CodeSegmentPath.VD || leg.LegTypeARINC == CodeSegmentPath.AF ||
											leg.LegTypeARINC == CodeSegmentPath.CF || leg.LegTypeARINC == CodeSegmentPath.FA ||
											leg.LegTypeARINC == CodeSegmentPath.FC || leg.LegTypeARINC == CodeSegmentPath.FD ||
											leg.LegTypeARINC == CodeSegmentPath.FM || leg.LegTypeARINC == CodeSegmentPath.PI ||
											leg.LegTypeARINC == CodeSegmentPath.CR || leg.LegTypeARINC == CodeSegmentPath.VR )
											logList.Add ( new ResultLog ( leg, ReportType.No_SegmentPoint, "End Point", proc ) );
									}
									else
									{
										CheckReqNavaid ( logList, leg, proc);
										if ( !segmenPntList.ContainsKey ( leg.ID ) )
											segmenPntList.Add ( leg.ID, new List<SegmentPoint> ( ) );
										segmenPntList[ leg.ID ].Add ( leg.EndPoint );
										switch ( leg.EndPoint.PointChoice )
										{
											case PointChoice.DesignatedPoint:
												commonValidator.CheckDsgPoint ( logList, leg.EndPoint, leg, proc, false );
												break;
											case PointChoice.Navaid:
												commonValidator.CheckNavaid ( logList, leg.StartPoint, leg.EndPoint, leg, proc );
												break;

											default:
													throw new Exception ( "Point Choice - " + leg.StartPoint.PointChoice +
														" (" + procDsg + " => " + leg.ToString ( ) + " => EndPoint ) is not implemented" );
										}
									}
									#endregion

									if ( !segmenPntList.ContainsKey ( leg.ID ) )
										continue;
									if ( segmenPntList[ leg.ID ].Count == 2 )
									{
										var prevInsertedKey = segmntPntDict.FirstOrDefault (
												 ( seg => seg.Value.Choice == leg.StartPoint.PointChoice &&
													 seg.Value.ChoiceId == leg.StartPoint.PointChoiceID ) );
										var prevInsertValue = segmntPntDict.FirstOrDefault (
												 ( seg => seg.Key.Choice == leg.EndPoint.PointChoice &&
													 seg.Key.ChoiceId == leg.EndPoint.PointChoiceID ) );
										SegmentPointData key, value;
										if ( prevInsertedKey.Key == null && prevInsertedKey.Value == null )
											key = new SegmentPointData ( leg.StartPoint.PointChoice, leg.StartPoint.PointChoiceID,
												leg.ToString ( ) );
										else
										{
											key = prevInsertedKey.Value;
											key.Description = leg.ToString ( );
										}

										if ( prevInsertValue.Key == null && prevInsertValue.Value == null )
											value = new SegmentPointData ( leg.EndPoint.PointChoice, leg.EndPoint.PointChoiceID,
												leg.ToString ( ) );
										else
										{
											value = prevInsertValue.Key;
										}
										if ( segmntPntDict.ContainsKey ( key ) && segmntPntDict.ContainsValue ( value ) )
											logList.Add ( new ResultLog ( leg, ReportType.Duplicate, proc ) );
										segmntPntDict.Add ( key, value );
									}
									else
										segmenPntList.Remove ( leg.ID );
								}
								legList.AddRange ( transition.Legs );
							}
						}
					}
					logList.AddRange (commonValidator.CheckRouteSegmentsBreak( proc, segmntPntDict, procDsg));
					index++;
				}
				commonValidator.CheckAirspaces (dataList, logList);

				commonValidator.CheckHoldings (dataList, logList);

				logList.AddRange (commonValidator.CheckAttributes (segmenPntList, procList));

				logList = logList.OrderBy (item => item.Type).ThenBy (item => item.Feature.PDM_Type).ToList ();

				if (logList.Count == 0)
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
				errorMessage = ex.StackTrace + " " + index;
				return 2;
			}
		}		

		private void CheckReqNavaid(List<ResultLog> logList, ProcedureLeg leg, PDMObject procedure)
        {
            if (leg.LegTypeARINC == CodeSegmentPath.AF || leg.LegTypeARINC == CodeSegmentPath.CF || 
                leg.LegTypeARINC == CodeSegmentPath.FA || leg.LegTypeARINC == CodeSegmentPath.FC ||
                leg.LegTypeARINC == CodeSegmentPath.FD || leg.LegTypeARINC == CodeSegmentPath.FM ||
                leg.LegTypeARINC == CodeSegmentPath.PI)
            {
                if (leg.EndPoint.PointFacilityMakeUp == null)
                    logList.Add(new ResultLog(leg, ReportType.Mandatory_Field_Absent, "EndPoint->FacilityMakeUp", procedure));
                else if(leg.EndPoint.PointFacilityMakeUp.AngleIndication == null)
                    logList.Add(new ResultLog(leg, ReportType.Mandatory_Field_Absent, "EndPoint->FacilityMakeUp->AngleIndication", procedure));
                else if (leg.EndPoint.PointFacilityMakeUp.DistanceIndication == null)
                    logList.Add(new ResultLog(leg, ReportType.Mandatory_Field_Absent, "EndPoint->FacilityMakeUp->DistanceIndication", procedure));
                else
                {
                    if(leg.EndPoint.PointChoice != PointChoice.Navaid)
                    {
						// ???
                        logList.Add(new ResultLog(leg, ReportType.Rule_Invalid, "EndPoint->PointChoice should be Navaid", procedure));
                    }
                    else
                    {
                        var navaid = AllNavaidList.FirstOrDefault(pdm => pdm.ID == leg.EndPoint.PointChoiceID) as NavaidSystem;
                        bool hasaAngle = navaid.Components.FirstOrDefault(pdm => pdm.PDM_Type == PDM_ENUM.VOR 
                            || pdm.PDM_Type == PDM_ENUM.NDB || pdm.PDM_Type == PDM_ENUM.Localizer || pdm.PDM_Type == PDM_ENUM.TACAN) != null;
                        bool hasDist = navaid.Components.FirstOrDefault(pdm => pdm.PDM_Type == PDM_ENUM.DME ||
                                 pdm.PDM_Type == PDM_ENUM.TACAN) != null;
                        string componentTypes = string.Join(" , ", navaid.Components.Select(pdm => pdm.PDM_Type).ToList());
                        if (!hasaAngle)
                            logList.Add(new ResultLog(leg, ReportType.Rule_Invalid, 
                                "Required navaid component should be one of the VOR / NDB / Localizer, but there is(are) " + componentTypes, procedure));
                        if (!hasDist)
                            logList.Add(new ResultLog(leg, ReportType.Rule_Invalid,
                                "Required navaid component should be one of the DME / TACAN, but there is(are) " + componentTypes, procedure));
                    }
                }
            }
            else if (leg.LegTypeARINC == CodeSegmentPath.CR || leg.LegTypeARINC == CodeSegmentPath.VR)
            {
                //THETA
                if (leg.EndPoint.PointFacilityMakeUp == null)
                    logList.Add(new ResultLog(leg, ReportType.Mandatory_Field_Absent, "EndPoint->FacilityMakeUp", procedure));
                else if (leg.EndPoint.PointFacilityMakeUp.AngleIndication == null)
                    logList.Add(new ResultLog(leg, ReportType.Mandatory_Field_Absent, "EndPoint->FacilityMakeUp->AngleIndication", procedure));
                else
                {
                    if (leg.EndPoint.PointChoice != PointChoice.Navaid)
                    {
                        logList.Add(new ResultLog(leg, ReportType.Rule_Invalid, "EndPoint->PointChoice should be Navaid", procedure));
                    }
                    else
                    {
                        var navaid = AllNavaidList.FirstOrDefault(pdm => pdm.ID == leg.EndPoint.PointChoiceID) as NavaidSystem;
                        bool hasaAngle = navaid.Components.FirstOrDefault(pdm => pdm.PDM_Type == PDM_ENUM.VOR
                            || pdm.PDM_Type == PDM_ENUM.NDB || pdm.PDM_Type == PDM_ENUM.Localizer || pdm.PDM_Type == PDM_ENUM.TACAN) != null;
                        string componentTypes = string.Join(" , ", navaid.Components.Select(pdm => pdm.PDM_Type).ToList());
                        if (!hasaAngle)
                            logList.Add(new ResultLog(leg, ReportType.Rule_Invalid,
                                "Required navaid component should be one of the VOR / NDB / Localizer, but there is(are) " + componentTypes, procedure));
                    }
                }
            }
            else if (leg.LegTypeARINC == CodeSegmentPath.CD || leg.LegTypeARINC == CodeSegmentPath.VD)
            {
                if (leg.EndPoint.PointFacilityMakeUp == null)
                    logList.Add(new ResultLog(leg, ReportType.Mandatory_Field_Absent, "EndPoint->FacilityMakeUp", procedure));
                else if (leg.EndPoint.PointFacilityMakeUp.DistanceIndication == null)
                    logList.Add(new ResultLog(leg, ReportType.Mandatory_Field_Absent, "EndPoint->FacilityMakeUp->DistanceIndication", procedure));
                else
                {
                    if (leg.EndPoint.PointChoice != PointChoice.Navaid)
                    {
                        logList.Add(new ResultLog(leg, ReportType.Rule_Invalid, "EndPoint->PointChoice should be Navaid", procedure));
                    }
                    else
                    {
                        var navaid = AllNavaidList.FirstOrDefault(pdm => pdm.ID == leg.EndPoint.PointChoiceID) as NavaidSystem;
                        bool hasDist = navaid.Components.FirstOrDefault(pdm => pdm.PDM_Type == PDM_ENUM.DME ||
                                 pdm.PDM_Type == PDM_ENUM.TACAN) != null;
                        string componentTypes = string.Join(" , ", navaid.Components.Select(pdm => pdm.PDM_Type).ToList());
                        if (!hasDist)
                            logList.Add(new ResultLog(leg, ReportType.Rule_Invalid,
                                "Required navaid component should be one of the DME / TACAN, but there is(are) " + componentTypes, procedure));
                    }
                }
            }
        }

        private static void CheckLength(List<ResultLog> logList, ProcedureLeg leg)
        {
            switch (leg.LegTypeARINC)
            {
                case CodeSegmentPath.CD:
                case CodeSegmentPath.CF:
                case CodeSegmentPath.FC:
                case CodeSegmentPath.FD:
                case CodeSegmentPath.HA:
                case CodeSegmentPath.HF:
                case CodeSegmentPath.HM:
                case CodeSegmentPath.PI:
                case CodeSegmentPath.RF:
                case CodeSegmentPath.TF:
                case CodeSegmentPath.VD:
                    if (!leg.Length.HasValue)
                        logList.Add(new ResultLog(leg, ReportType.Mandatory_Field_Absent, "Length"));
                    break;
            }
        }

        private void CheckMagneticCourse(List<ResultLog> logList, ProcedureLeg leg)
        {
            switch (leg.LegTypeARINC)
            {
                case CodeSegmentPath.AF:
                case CodeSegmentPath.CA:
                case CodeSegmentPath.CD:
                case CodeSegmentPath.CF:
                case CodeSegmentPath.CI:
                case CodeSegmentPath.CR:
                case CodeSegmentPath.FA:
                case CodeSegmentPath.FC:
                case CodeSegmentPath.FD:
                case CodeSegmentPath.FM:
                case CodeSegmentPath.HA:
                case CodeSegmentPath.HF:
                case CodeSegmentPath.HM:
                case CodeSegmentPath.PI:
                case CodeSegmentPath.RF:
                case CodeSegmentPath.VA:
                case CodeSegmentPath.VD:
                case CodeSegmentPath.VI:
                case CodeSegmentPath.VM:
                case CodeSegmentPath.VR:
                    if (!leg.Course.HasValue)
                        logList.Add(new ResultLog(leg, ReportType.Mandatory_Field_Absent, "Course"));
                    break;
            }
        }
	}
}