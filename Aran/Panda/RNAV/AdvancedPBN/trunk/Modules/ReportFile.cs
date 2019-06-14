using Aran.Geometries;
using Aran.PANDA.Common;
using System;
using System.IO;
using System.Reflection;
//using Aran.PANDA.Departure.Properties;
//using ESRI.ArcGIS.Geometry;

namespace Aran.PANDA.RNAV.SGBAS
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public class CReportFile
	{
		private const string ReportFileExt = ".htm";
		public System.Windows.Forms.ListView lListView;
		public Point DerPtPrj;
		public Point ThrPtPrj;

		private string pFileName;
		private StreamWriter _stwr = null;

		public void OpenFile(string Name, string ReportTitle)
		{
			if (_stwr != null)
				return;

			pFileName = Name + ReportFileExt;

			_stwr = new StreamWriter(pFileName, false, System.Text.Encoding.UTF8);

			_stwr.WriteLine("<html>");
			_stwr.WriteLine("<head>");
			_stwr.WriteLine("<title>PANDA - " + ReportTitle + "</title>");
			_stwr.WriteLine("<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />");
			_stwr.WriteLine("<style>");
			_stwr.WriteLine("body {font-family: Arial, Sans-Serif; font-size:12;}");
			_stwr.WriteLine("table {font-family: Arial, Sans-Serif; font-size:10;}");
			_stwr.WriteLine("</style>");
			_stwr.WriteLine("</head>");
			_stwr.WriteLine("<body>");
		}

		public void CloseFile()
		{
			if (_stwr == null)
				return;

			_stwr.WriteLine("</body>");
			_stwr.WriteLine("</html>");

			_stwr.Flush();
			_stwr.Dispose();
			_stwr = null;
		}

		public void WriteHeader(ReportHeader pReport)
		{
			Assembly currentAssem = Assembly.GetExecutingAssembly();
			WriteString("<p><b>" + System.Net.WebUtility.HtmlEncode("PANDA " + currentAssem.GetName().Version.ToString()) + "</b>");
			WriteString("<p><b>" + System.Net.WebUtility.HtmlEncode(GlobalVars.PANSOPSVersion) + "</b>");
			//WriteString(Resources.str819 + GlobalVars.GetMapFileName());
			//WriteString(Resources.str90);
			WriteString("<p><b>AIM Environment: " + System.Net.WebUtility.HtmlEncode(pReport.Database )+ "</b>");

			if (GlobalVars.settings.AnnexObstalce)
				WriteString("Obstacle source: Obstacle area obstacles.");
			else
				WriteString("Obstacle source: All vertical structures from DB.");

			if (pReport.Aerodrome!=null && pReport.Aerodrome != "")
                WriteString(System.Net.WebUtility.HtmlEncode("Aerodrome:" + pReport.Aerodrome));

			if (pReport.Procedure != null && pReport.Procedure != "")
                WriteString(System.Net.WebUtility.HtmlEncode("Procedure name:" + pReport.Procedure) + "</p>");

			//if (pReport.RWY != null && pReport.RWY != "")
			//	WriteString("<p>" + Resources.str822 + pReport.RWY);

			if (pReport.Category != null && pReport.Category != "")
                WriteString(System.Net.WebUtility.HtmlEncode("Aircraft category: up to " + pReport.Category) + "</p>");

			WriteString("<p>User name: " + System.Net.WebUtility.HtmlEncode(GlobalVars.UserName) + "<br>");

            WriteString("<p><b>" + "Creation date: " + "</b>    " + DateTime.Now.Date.ToString("d"));
			//WriteString("<b>" + Resources.str703 + "</b>    " + DateTime.Now.TimeOfDay.ToString("T") + "</p>");
            WriteString("<b>" + "Creation time: " + "</b>    " + DateTime.Now.ToLongTimeString() + "</p>");
			
			//pReport.EffectiveDate = new DateTime(pReport.EffectiveDate.Year, pReport.EffectiveDate.Month, pReport.EffectiveDate.Day, 0, 0, 0);
			//WriteString("<b>" + Resources.str704 + pReport.EffectiveDate.ToString("d") + "</p>");

			WriteString("");
            //WriteString("<p>" + Resources.str806 + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit + ".");
            //WriteString(Resources.str807 + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit + ".");
            //WriteString(Resources.str808 + GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Unit + ".</p>");
			WriteString("");
		}

		public void WriteString(string Message)
		{
			string temp = Message;

			//if (Message != "")	temp = "<b>" + Message + "</b>";
			//System.Net.WebUtility.HtmlEncode(temp + "<br>", _stwr);
			_stwr.WriteLine(temp + "<br>");
		}

		public void WriteString()
		{
			WriteString("");
		}

		public void WriteParam(string ParamName, string ParamValue, string ParamUnit="")
		{
			string temp;

			if (ParamName == "" || ParamValue == "")
				return;

			temp = "<b>" + System.Net.WebUtility.HtmlEncode(ParamName) + ":</b> " + System.Net.WebUtility.HtmlEncode(ParamValue);
			if (ParamUnit != "")
				temp = temp + " " + System.Net.WebUtility.HtmlEncode(ParamUnit);

			_stwr.WriteLine(temp + " <br>");
		}

		public void WriteTab(string TabComment = "")
		{
			int n, m, i, j;
			System.Windows.Forms.ListViewItem itmX;

			if (lListView == null || lListView.Items.Count == 0)
				return;

			if (TabComment != "")
			{
				WriteString(System.Net.WebUtility.HtmlEncode(TabComment));
				WriteString("");
			}

			_stwr.WriteLine("<table border='1' cellspacing='0' cellpadding='1'>");

			n = lListView.Columns.Count;
			m = lListView.Items.Count;

			_stwr.WriteLine("<tr>");
			for (i = 0; i < n; i++)
				_stwr.WriteLine("<td><b>" + System.Net.WebUtility.HtmlEncode(lListView.Columns[i].Text) + "</b></td>");

			_stwr.WriteLine("</tr>");

			for (i = 0; i < m; i++)
			{
				itmX = lListView.Items[i];

				_stwr.WriteLine("<tr>");
				_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(itmX.Text) + "</td>");

				for (j = 1; j < n; j++)
				_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(itmX.SubItems[j].Text) + "</td>");

				_stwr.WriteLine("</tr>");
			}

			_stwr.WriteLine("</table>");
			_stwr.WriteLine("<br><br>");
		}

		public void WriteImage(string ImageName, string ImageComment = "")
		{
			if (ImageName == "")
				return;

			if (ImageComment != "")
			{
				WriteString(System.Net.WebUtility.HtmlEncode(ImageComment));
				WriteString("");
			}

			_stwr.WriteLine("<img src='" + System.Net.WebUtility.HtmlEncode(ImageName) + "' border='0'>");
		}


		public void WriteImageLink(string ImageName, string ImageComment = "")
		{
			if (ImageName == "")
				return;

			_stwr.WriteLine("<a href='" + System.Net.WebUtility.HtmlEncode(ImageName) + "'>" + System.Net.WebUtility.HtmlEncode(ImageComment) + "</a>");
		}

		public void WritePoint(ReportPoint RepPoint)
		{
			if (RepPoint.Description == "")
				return;

			WriteString(System.Net.WebUtility.HtmlEncode(RepPoint.Description));

            WriteParam("Latitude", Functions.Degree2String(RepPoint.Lat, Degree2StringMode.DMSLat), "");
            WriteParam("Longitude", Functions.Degree2String(RepPoint.Lon, Degree2StringMode.DMSLon), "");
			if (RepPoint.Radius > 0)
			{
                WriteParam("Latitude of the circle center ", Functions.Degree2String(RepPoint.CenterLat, Degree2StringMode.DMSLat), "");
                WriteParam("Longitude of the circle center ", Functions.Degree2String(RepPoint.CenterLon, Degree2StringMode.DMSLon), "");
			}

            WriteParam("Altitude", GlobalVars.unitConverter.HeightToDisplayUnits(RepPoint.Height).ToString(), GlobalVars.unitConverter.HeightUnit);
            WriteParam("Height", GlobalVars.unitConverter.HeightToDisplayUnits(RepPoint.Height).ToString(), GlobalVars.unitConverter.HeightUnit);

			if (RepPoint.Direction >= 0.0)
                WriteParam("True course", Math.Round(RepPoint.Direction, 2).ToString(), "°");

			if (RepPoint.Radius > 0)
			{
                WriteParam("Turning radius", GlobalVars.unitConverter.DistanceToDisplayUnits(RepPoint.Radius).ToString(), GlobalVars.unitConverter.DistanceUnit);
				if (RepPoint.Turn > 0)
                    WriteParam("Turn direction", "  right", "");
				else
                    WriteParam("Turn direction", " left", "");
                WriteParam("Angle of the turn", Math.Round(RepPoint.turnAngle, 2).ToString(), "°");
                WriteParam("Turn arc length", GlobalVars.unitConverter.DistanceToDisplayUnits(RepPoint.TurnArcLen).ToString(), GlobalVars.unitConverter.DistanceUnit);
			}

			if (RepPoint.DistToNext > 0.0)
                WriteParam("Distance to the next point", GlobalVars.unitConverter.DistanceToDisplayUnits(RepPoint.DistToNext).ToString(), GlobalVars.unitConverter.DistanceUnit);
			WriteString("");
		}

        //public void WriteTraceSegment(TraceSegment Segment, bool IsLastSegment)
        //{
        //    switch (Segment.SegmentCode)
        //    {
        //        case  eSegmentType.straight:
        //            WriteString(System.Net.WebUtility.HtmlEncode(Resources.str581 + Segment.RepComment));
        //            WriteParam(Resources.str582, Segment.StCoords, "");

        //            WriteParam(Resources.str568, Functions.ConvertHeight(Segment.HStart, eRoundMode.rmNERAEST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
        //            WriteParam(Resources.str569, Functions.ConvertHeight(Segment.HStart - ThrPtPrj.Z, eRoundMode.rmNERAEST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);

        //            WriteParam(Resources.str577, NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(DerPtPrj, Segment.DirIn), 2)).ToString(), "°");
        //            WriteParam(Resources.str578, Functions.ConvertDistance(Segment.Length, eRoundMode.rmNERAEST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);
        //            WriteString("");

        //            if (IsLastSegment)
        //            {
        //                WriteString(System.Net.WebUtility.HtmlEncode(Resources.str583 + Segment.RepComment));
        //                WriteParam(Resources.str582, Segment.FinCoords, "");

        //                WriteParam(Resources.str568, Functions.ConvertHeight(Segment.HFinish, eRoundMode.rmNERAEST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
        //                WriteParam(Resources.str569, Functions.ConvertHeight(Segment.HFinish - ThrPtPrj.Z, eRoundMode.rmNERAEST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);

        //                WriteParam(Resources.str577, NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(DerPtPrj, Segment.DirOut), 2)).ToString(), "°");
        //                WriteString("");
        //            }

        //            break;
        //        case eSegmentType.toHeading:
        //        case eSegmentType.directToFIX:
        //        //case  eSegmentType.courseIntercept:
        //            WriteString(System.Net.WebUtility.HtmlEncode(Resources.str584 + Segment.RepComment));
        //            WriteParam(Resources.str582, Segment.StCoords, "");

        //            WriteParam(Resources.str568, Functions.ConvertHeight(Segment.HStart, eRoundMode.rmNERAEST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
        //            WriteParam(Resources.str569, Functions.ConvertHeight(Segment.HStart - ThrPtPrj.Z, eRoundMode.rmNERAEST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);

        //            WriteParam(Resources.str577, NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(DerPtPrj, Segment.DirIn), 2)).ToString(), "°");
        //            WriteParam(Resources.str578, Functions.ConvertDistance(Segment.Length, eRoundMode.rmNERAEST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);
        //            WriteString("");
        //            WriteParam(Resources.str585, Segment.Center1Coords, "");
        //            WriteParam(Resources.str572, Functions.ConvertDistance(Segment.Turn1R, eRoundMode.rmNERAEST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);
        //            if ((Segment.Turn1Dir > 0))
        //                WriteParam(Resources.str573, Resources.str574, "");
        //            else
        //                WriteParam(Resources.str573, Resources.str575, "");

        //            WriteParam(Resources.str576, System.Math.Round(Segment.Turn1Angle, 2).ToString(), "°");
        //            WriteString("");

        //            if ((IsLastSegment))
        //            {
        //                WriteString(System.Net.WebUtility.HtmlEncode(Resources.str586 + Segment.RepComment));
        //                WriteParam(Resources.str582, Segment.FinCoords, "");

        //                WriteParam(Resources.str568, Functions.ConvertHeight(Segment.HFinish, eRoundMode.rmNERAEST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
        //                WriteParam(Resources.str569, Functions.ConvertHeight(Segment.HFinish - ThrPtPrj.Z, eRoundMode.rmNERAEST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);

        //                WriteParam(Resources.str577, System.Math.Round(NativeMethods.Modulus(Functions.Dir2Azt(DerPtPrj, Segment.DirOut)), 2).ToString(), "°");
        //                WriteString("");
        //            }

        //            break;
        //        case eSegmentType.courseIntercept:
        //            WriteString(System.Net.WebUtility.HtmlEncode(Resources.str581 + Segment.RepComment));
        //            WriteParam(Resources.str582, Segment.StCoords, "");

        //            WriteParam(Resources.str568, Functions.ConvertHeight(Segment.HStart, eRoundMode.rmNERAEST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
        //            WriteParam(Resources.str569, Functions.ConvertHeight(Segment.HStart - ThrPtPrj.Z, eRoundMode.rmNERAEST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);

        //            WriteParam(Resources.str577, NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(DerPtPrj, Segment.DirIn), 2)).ToString(), "°");
        //            WriteParam(Resources.str578, Functions.ConvertDistance(Segment.BetweenTurns, eRoundMode.rmNERAEST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);
        //            WriteString("");

        //            WriteString(System.Net.WebUtility.HtmlEncode(Resources.str584 + Segment.RepComment));
        //            WriteParam(Resources.str582, Segment.St1Coords, "");

        //            WriteParam(Resources.str568, Functions.ConvertHeight(Segment.H1, eRoundMode.rmNERAEST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
        //            WriteParam(Resources.str569, Functions.ConvertHeight(Segment.H1 - ThrPtPrj.Z, eRoundMode.rmNERAEST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);

        //            WriteParam(Resources.str577, NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(DerPtPrj, Segment.DirIn), 2)).ToString(), "°");
        //            WriteParam(Resources.str578, Functions.ConvertDistance(Segment.Turn1Length, eRoundMode.rmNERAEST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);
        //            WriteString("");

        //            WriteParam(Resources.str585, Segment.Center1Coords, "");
        //            WriteParam(Resources.str572, Functions.ConvertDistance(Segment.Turn1R, eRoundMode.rmNERAEST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);

        //            if ((Segment.Turn1Dir > 0))
        //                WriteParam(Resources.str573, Resources.str574, "");
        //            else
        //                WriteParam(Resources.str573, Resources.str575, "");

        //            WriteParam(Resources.str576, System.Math.Round(Segment.Turn1Angle, 2).ToString(), "°");
        //            WriteString("");

        //            if ((IsLastSegment))
        //            {
        //                WriteString(System.Net.WebUtility.HtmlEncode(Resources.str586 + Segment.RepComment));
        //                WriteParam(Resources.str582, Segment.FinCoords, "");

        //                WriteParam(Resources.str568, Functions.ConvertHeight(Segment.HFinish, eRoundMode.rmNERAEST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
        //                WriteParam(Resources.str569, Functions.ConvertHeight(Segment.HFinish - ThrPtPrj.Z, eRoundMode.rmNERAEST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);

        //                WriteParam(Resources.str577, NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(DerPtPrj, Segment.DirOut), 2)).ToString(), "°");
        //                WriteString("");
        //            }

        //            break;
        //        case eSegmentType.turnAndIntercept:
        //            WriteString(System.Net.WebUtility.HtmlEncode(Resources.str587 + Segment.RepComment));
        //            WriteParam(Resources.str582, Segment.StCoords, "");

        //            WriteParam(Resources.str568, Functions.ConvertHeight(Segment.HStart, eRoundMode.rmNERAEST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
        //            WriteParam(Resources.str569, Functions.ConvertHeight(Segment.HStart - ThrPtPrj.Z, eRoundMode.rmNERAEST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);

        //            WriteParam(Resources.str577, NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(DerPtPrj, Segment.DirIn), 2)).ToString(), "°");
        //            WriteParam(Resources.str578, Functions.ConvertDistance(Segment.Turn1Length, eRoundMode.rmNERAEST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);

        //            WriteString("");
        //            WriteParam(Resources.str588, Segment.Center1Coords, "");
        //            WriteParam(Resources.str589, Functions.ConvertDistance(Segment.Turn1R, eRoundMode.rmNERAEST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);

        //            if (Segment.Turn1Dir > 0)
        //                WriteParam(Resources.str590, Resources.str574, "");
        //            else
        //                WriteParam(Resources.str590, Resources.str575, "");

        //            WriteParam(Resources.str591, System.Math.Round(Segment.Turn1Angle, 2).ToString(), "°");
        //            WriteString("");

        //            WriteString(System.Net.WebUtility.HtmlEncode(Resources.str592 + Segment.RepComment));
        //            WriteParam(Resources.str582, Segment.Fin1Coords, "");

        //            WriteParam(Resources.str568, Functions.ConvertHeight(Segment.H1, eRoundMode.rmNERAEST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
        //            WriteParam(Resources.str569, Functions.ConvertHeight(Segment.H1 - ThrPtPrj.Z, eRoundMode.rmNERAEST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);

        //            WriteParam(Resources.str577, NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(DerPtPrj, Segment.DirBetween), 2)).ToString(), "°");
        //            WriteParam(Resources.str578, Functions.ConvertDistance(Segment.BetweenTurns, eRoundMode.rmNERAEST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);

        //            WriteString("");

        //            WriteString(System.Net.WebUtility.HtmlEncode(Resources.str593 + Segment.RepComment));
        //            WriteParam(Resources.str582, Segment.St2Coords, "");

        //            WriteParam(Resources.str568, Functions.ConvertHeight(Segment.H2, eRoundMode.rmNERAEST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
        //            WriteParam(Resources.str569, Functions.ConvertHeight(Segment.H2 - ThrPtPrj.Z, eRoundMode.rmNERAEST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);

        //            WriteParam(Resources.str577, NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(DerPtPrj, Segment.DirBetween), 2)).ToString(), "°");
        //            WriteParam(Resources.str578, Functions.ConvertDistance(Segment.Turn2Length, eRoundMode.rmNERAEST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);

        //            WriteString("");
        //            WriteParam(Resources.str594, Segment.Center2Coords, "");
        //            WriteParam(Resources.str595, Functions.ConvertDistance(Segment.Turn2R, eRoundMode.rmNERAEST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);

        //            if (Segment.Turn2Dir > 0)
        //                WriteParam(Resources.str596, Resources.str574, "");
        //            else
        //                WriteParam(Resources.str596, Resources.str575, "");

        //            WriteParam(Resources.str597, System.Math.Round(Segment.Turn2Angle, 2).ToString(), "°");
        //            WriteString("");

        //            if (IsLastSegment)
        //            {
        //                WriteString(System.Net.WebUtility.HtmlEncode(Resources.str598 + Segment.RepComment));
        //                WriteParam(Resources.str582, Segment.FinCoords, "");

        //                WriteParam(Resources.str568, Functions.ConvertHeight(Segment.HFinish, eRoundMode.rmNERAEST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
        //                WriteParam(Resources.str569, Functions.ConvertHeight(Segment.HFinish - ThrPtPrj.Z, eRoundMode.rmNERAEST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);

        //                WriteParam(Resources.str577, NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(DerPtPrj, Segment.DirOut), 2)).ToString(), "°");
        //                WriteString("");
        //            }
        //            break;
        //    }
        //}

		public static void SaveGeometry(string RepFileName, string RepFileTitle, ReportHeader pReport, RWYType DER, ReportPoint[] reportPoints, double allRoutsLen, bool guaded)
		{
			CReportFile RoutsGeomRep = new CReportFile();
			RoutsGeomRep.DerPtPrj = DER.pPtPrj[eRWY.ptEnd];
			RoutsGeomRep.ThrPtPrj = DER.pPtPrj[eRWY.ptEnd];

            RoutsGeomRep.OpenFile(RepFileName + "_Geometry", "The procedure geometry");
            //if (guaded)
            //    RoutsGeomRep.WriteString(System.Net.WebUtility.HtmlEncode(Resources.str15479 + " - " + "The procedure geometry"));
            //else
            //    RoutsGeomRep.WriteString(System.Net.WebUtility.HtmlEncode(Resources.str15271 + " - " + "The procedure geometry"));

			RoutsGeomRep.WriteString("");

			RoutsGeomRep.WriteString(System.Net.WebUtility.HtmlEncode(RepFileTitle));
			RoutsGeomRep.WriteHeader(pReport);

			RoutsGeomRep.WriteString("");
			RoutsGeomRep.WriteString("");

			int n = reportPoints.Length;
			for (int i = 0; i < n; i++)
				RoutsGeomRep.WritePoint(reportPoints[i]);

			RoutsGeomRep.WriteString("");

            //RoutsGeomRep.WriteParam("The procedure overall length", Functions.ConvertDistance(allRoutsLen, eRoundMode.rmNERAEST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);

			RoutsGeomRep.CloseFile();
			RoutsGeomRep = null;
		}

	}
}
