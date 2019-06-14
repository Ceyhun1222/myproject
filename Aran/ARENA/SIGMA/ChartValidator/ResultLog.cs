using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.Features;
using PDM;

namespace ChartValidator
{
	public class ResultLog
	{
		public ResultLog (PDMObject feat)
		{
			Feature = feat;
			DescriptionList = new List<string> ();
			_type = ResultType.Warning;
			ConflictedRouteSegments = new List<List<string>> ();
		}

		public ResultLog (PDMObject feat, ReportType reportType, string description)
			: this (feat)
		{
			ReportType = reportType;
			DescriptionList.Add (description);
			GetReportText ();
		}

		public ResultLog (PDMObject feat, ReportType reportType, string description, PDMObject otherFeat)
			: this (feat)
		{
			ReportType = reportType;
			DescriptionList.Add (description);
			_otherFeat = otherFeat;
			GetReportText ();
		}

		public ResultLog (PDMObject feat, ReportType reportType, PDMObject otherFeat)
			: this (feat)
		{
			_otherFeat = otherFeat;
			ReportType = reportType;
			GetReportText ();
		}

		public ResultLog (PDMObject feat, ReportType reportType)
			: this (feat)
		{
			ReportType = reportType;
			//if (reportType == ReportType.No_SegmentPoint || reportType == ReportType.Geometry || reportType == ReportType.Route_Break
			//|| reportType == ReportType.Bearing_inappropriate || reportType == ReportType.Length_inappropirate)
			//	_type = ResultType.Error;
			GetReportText ();
		}

		public PDM.PDMObject Feature { get; set; }
		public List<string> DescriptionList { get; set; }
		public ReportType ReportType { get; set; }
		public List<List<string>> ConflictedRouteSegments { get; set; }
		public string FeatName
		{
			get
			{
				return Feature.PDM_Type.ToString ();
			}
		}
		public string Name { get; private set; }
		public string ReportText { get; private set; }
		
		public ResultType Type
		{
			get
			{
				return _type;
			}
		}

		public void GetReportText ()
		{
			//if (_name_created)
			//	return;
			if (ReportType == ReportType.No_RouteSegment || ReportType == ReportType.One_RouteSegment)
			{
				Name = (Feature as PDM.Enroute).TxtDesig;
				if (string.IsNullOrEmpty (Name))
					Name = Feature.ID;
				if (ReportType == ReportType.No_RouteSegment)
					ReportText = "Has no Route Segment";
				else
					ReportText = "Has 1 Route Segment";
			}
			else if (ReportType == ReportType.Duplicate)
			{
				Name = (_otherFeat as PDM.Enroute).TxtDesig + (Feature as PDM.RouteSegment).ToString ();
				ReportText = "This route segment is duplicated in this enroute";
			}
			else if (ReportType == ReportType.Geometry_Conflict)
			{
				Name = (_otherFeat as PDM.Enroute).TxtDesig + " => " + Feature.ToString ();
				ReportText = "Line conflicts with Start and End points";
			}
			else if (ReportType == ReportType.No_SegmentPoint)
			{
                if (Feature is PDM.RouteSegment)
                {
                    Name = (_otherFeat as PDM.Enroute).TxtDesig + " => " + Feature.ToString();
                }
				else if ( Feature is PDM.ProcedureLeg )
				{
					Name = _otherFeat.ToString ( ) + " => " + Feature.ToString ( ) +
						   " (" + ( Feature as ProcedureLeg ).SeqNumberARINC + ")";
				}
				ReportText = "Has no " + DescriptionList[ 0 ];
			}
			else if (ReportType == ReportType.Geometry)
			{
				Name = (_otherFeat as PDM.Enroute).TxtDesig + " => " + Feature.ToString ();
				ReportText = "Geometry is empty";
			}
			else if (ReportType == ReportType.Bearing_inappropriate)
			{
				Name = (_otherFeat as PDM.Enroute).TxtDesig + " => " + Feature.ToString ();
				ReportText = "True bearing [" + DescriptionList[0] + "]";
			}
			else if (ReportType == ReportType.ReverseBearing_inappropriate)
			{
				Name = (_otherFeat as PDM.Enroute).TxtDesig + " => " + Feature.ToString ();
				ReportText = "Reverse bearing [" + DescriptionList[0] + "]";
			}
			else if (ReportType == ReportType.MagTrack_inappropriate)
			{
				Name = (_otherFeat as PDM.Enroute).TxtDesig + " => " + Feature.ToString ();
				ReportText = "Magnetic track [" + DescriptionList[0] + "]";
			}
			else if (ReportType == ReportType.ReverseMag_inappropriate)
			{
				Name = (_otherFeat as PDM.Enroute).TxtDesig + " => " + Feature.ToString ();
				ReportText = "Reverse magnetic bearing [" + DescriptionList[0] + "]";
			}
			else if (ReportType == ReportType.Length_inappropirate)
			{
				Name = (_otherFeat as PDM.Enroute).TxtDesig + " => " + Feature.ToString ();
				ReportText = "Length [" + DescriptionList[0] + "]";
			}
			else if (ReportType == ReportType.Segment_Point_Link)
			{
				Name = _otherFeat.GetObjectLabel() + " => " + Feature.ToString ();
				ReportText = DescriptionList[0];
			}
			else if (ReportType == ReportType.Mandatory_Field_Absent)
			{
				//row.Cells[0].Value = _logList[i].Feature.PDM_Type.ToString ();
				if(Feature is PDM.Enroute)
				{
					Name = ((PDM.Enroute) Feature).TxtDesig;
					if (string.IsNullOrEmpty (Name))
						Name = Feature.ID;
				}
                if (Feature is PDM.Airspace)
                {
                    Name = ((PDM.Airspace)Feature).TxtName;
                    if (Name == null || Name.Trim() == "")
                        Name = ((PDM.Airspace)Feature).CodeID;
                }
                else if (Feature is PDM.AirspaceVolume)
                {
                    Name = (_otherFeat as PDM.Airspace).ToString() + " => " + (Feature as PDM.AirspaceVolume).TxtName;
                }
                else if (Feature is PDM.HoldingPattern)
                {
                    Name = "HoldingPattern";
                }
                else if (Feature is PDM.RouteSegment)
                {
                    Name = (_otherFeat as PDM.Enroute).TxtDesig + " => " + Feature.ToString();
                }
                else if (Feature is PDM.NavaidSystem)
                {
                    //row.Cells[0].Value = ((PDM.NavaidSystem) _logList[i].Feature).CodeNavaidSystemType.ToString ();
                    Name = ((PDM.NavaidSystem)Feature).Designator;
                }
                else if (Feature is PDM.WayPoint)
                {
                    Name = ((PDM.WayPoint)Feature).Name;
                    if (string.IsNullOrEmpty(Name))
                        Name = ((PDM.WayPoint)Feature).Designator;
                    if (string.IsNullOrEmpty(Name))
                        Name = ((PDM.WayPoint)Feature).ID;
                }
                else if (Feature is PDM.StandardInstrumentDeparture || Feature is PDM.StandardInstrumentArrival)
                    Name = Feature.ToString();
                else if(Feature is PDM.ProcedureLeg)
                    Name = _otherFeat.ToString() + " => " + Feature.ToString() +
                        " (" + (Feature as ProcedureLeg).SeqNumberARINC + ")";
                ReportText = DescriptionList[0] + " field is absent";
			}
			else if (ReportType == ReportType.Route_Break)
			{
				//row.Cells[0].Value = _logList[i].Feature.PDM_Type.ToString ();
				Name = Feature.ToString ();
				ReportText = DescriptionList[0];
			}
			else if (ReportType == ReportType.Route_Intersects)
			{
				Name = Feature.ToString ();
				ReportText = "Route segments are intersected";
			}
			else if (ReportType == ReportType.WayPoint_Attribute)
			{
                Name = Feature.GetObjectLabel();// is PDM.WayPoint ? (Feature as PDM.WayPoint).Designator : (Feature as PDM.NavaidSystem).Designator;
				string str = string.Join ("\r\n", DescriptionList);
				ReportText = str;
			}
			else if(ReportType == ChartValidator.ReportType.Rule_Invalid)
			{
                if (Feature is PDM.StandardInstrumentDeparture || Feature is PDM.StandardInstrumentArrival)
                    Name = Feature.ToString();
                else if(Feature is ProcedureLeg)
                {
                    Name = _otherFeat .ToString() + " => " + Feature.ToString() +
                        " (" + (Feature as ProcedureLeg).SeqNumberARINC + ")";
                }
                //string tmp = Feature.GetObjectLabel();
                ReportText = DescriptionList[0];
			}
		}

		public override string ToString ()
		{
			return Feature.PDM_Type + " " + Feature.ToString ();
		}

		private ResultType _type;
		private PDMObject _otherFeat;
	}

	public enum ResultType
	{ 
		Error,
		Warning
	}

	public enum ReportType
	{
		No_RouteSegment,
		One_RouteSegment,
		Duplicate,
		No_SegmentPoint,
		Segment_Point_Link,
		Geometry,
		Geometry_Conflict,
		Bearing_inappropriate,
		ReverseBearing_inappropriate,
		MagTrack_inappropriate,
		ReverseMag_inappropriate,
		Length_inappropirate,
		Mandatory_Field_Absent,
		Route_Break,
		Route_Intersects,
		WayPoint_Attribute,
		Rule_Invalid
	}

	public enum ChartType
	{
		Enroute,
		SID,
		Star,
		IAP
	}
}