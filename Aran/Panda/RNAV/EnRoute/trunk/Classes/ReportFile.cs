using System;
using System.Collections.Generic;
using System.IO;
using Aran.PANDA.Common;
using Aran.Geometries;
using Aran.PANDA.RNAV.EnRoute.Modules;
using Aran.PANDA.RNAV.EnRoute.Properties;
using System.Linq;
using System.Reflection;
using Aran.PANDA.Reports;

namespace Aran.PANDA.RNAV.EnRoute
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public class CReportFile : ReportBase
	{
		public override void Open(string ReportTitle)
		{
			//if (_stwr != null) return;

			_excellExporter = ExcellExporter.Get(ReportTitle);
			_excellExporter.CreateSheet("Info");

			_stwr = new StreamWriter(pFileName + ReportFileExt, false, System.Text.Encoding.UTF8);
			_stwr.WriteLine("<html>");
			_stwr.WriteLine("<head>");
			_stwr.WriteLine("<title>PANDA - " + ReportTitle + "</title>");
			_stwr.WriteLine("<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />");
			_stwr.WriteLine("<style>");
			_stwr.WriteLine("body {font-family: Arial; Sans-Serif; font-size:12;}");
			_stwr.WriteLine("table {width:100%;Sans-Serif; font-size:10;}");
			_stwr.WriteLine(".Table-row-type-1 {background-color: #aaaacc;border-color: black;}");
			_stwr.WriteLine(".Table-row-type-2 {background-color: #dddddd;border-color: black;}");
			_stwr.WriteLine(" .Table-row-type-3 {background-color: #eeeeee;border-color: black;}");
			_stwr.WriteLine(".Table-row-type-3 th, .Table-row-type-3 td {border-left: 1px solid #dddddd;text-align:center;}");
			_stwr.WriteLine("</style>");
			_stwr.WriteLine("</head>");
			_stwr.WriteLine("<body>");
		}

		public void WriteHeader(ReportHeader pReport)
		{
			Assembly currentAssem = Assembly.GetExecutingAssembly();

			WriteString(currentAssem.GetName().Version.ToString(), true, true);
			WriteString(GlobalVars.PANSOPSVersion, true, true);
			WriteString("AIM Environment: " + pReport.Database, true, true);

			WriteString("Obstacle source: All vertical structures from DB.", true);


			if (pReport.Procedure != null && pReport.Procedure != "")
				WriteString(Resources.str00100 + pReport.Procedure, true);

			WriteString(Resources.str00109 + pReport.UserName, true);

			//=====================================================================
			WriteString();

			_excellExporter.CreateRow().Text(Resources.str00101, true, false, 12).Text(DateTime.Now.Date.ToString("d"));
			_stwr.WriteLine("<p><b>" + Resources.str00101 + "</b>    " + DateTime.Now.Date.ToString("d") + "</br>");
			_excellExporter.CreateRow().Text(Resources.str00102, true, false, 12).Text(DateTime.Now.ToLongTimeString());
			_stwr.WriteLine("<b>" + Resources.str00102 + "</b>    " + DateTime.Now.ToLongTimeString() + "</p>" + "</br>");

			//pReport.EffectiveDate = New Date(pReport.EffectiveDate.Year, pReport.EffectiveDate.Month, pReport.EffectiveDate.Day, 0, 0, 0)
			//WriteString("<b>" + My.Resources.str853 + pReport.EffectiveDate.ToString("d") + "</p>")

			WriteString();
			WriteString();

			WriteString(Resources.str00103 + GlobalVars.unitConverter.DistanceUnit + ".", true);
			WriteString(Resources.str00104 + GlobalVars.unitConverter.HeightUnit + ".", true);
			WriteString(Resources.str00105 + GlobalVars.unitConverter.SpeedUnit + ".", true);

			WriteString();

			//========================================================================
			//pReport.EffectiveDate = new DateTime(pReport.EffectiveDate.Year, pReport.EffectiveDate.Month, pReport.EffectiveDate.Day, 0, 0, 0);
			//WriteString("<b>" + Resources.str704 + pReport.EffectiveDate.ToString("d") + "</p>");
		}

		public void WriteTab(System.Windows.Forms.DataGridView gridView, string TabComment = "")
		{
			int n, m, i, j;
			//System.Windows.Forms.ListViewItem itmX;

			if (gridView == null)// || lListView.Rows.Count == 0)
				return;

			if (TabComment != "")
			{
				WriteString(System.Net.WebUtility.HtmlEncode(TabComment));
				WriteString("");
			}

			_stwr.WriteLine("<table border='1' cellspacing='0' cellpadding='1'>");

			n = gridView.Columns.Count;
			m = gridView.Rows.Count;

			_stwr.WriteLine("<tr>");
			for (i = 0; i < n; i++)
				_stwr.WriteLine("<td><b>" + System.Net.WebUtility.HtmlEncode(gridView.Columns[i].HeaderText) + "</b></td>");
			_stwr.WriteLine("</tr>");

			for (i = 0; i < m; i++)
			{
				System.Windows.Forms.DataGridViewRow row = gridView.Rows[i];

				_stwr.WriteLine("<tr>");
				for (j = 0; j < n; j++)
					_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(row.Cells[j].Value.ToString()) + "</td>");
				_stwr.WriteLine("</tr>");
			}

			_stwr.WriteLine("</table>");
			_stwr.WriteLine("<br><br>");
		}

		//public void Param(string ParamName, string ParamValue, string ParamUnit)
		//{
		//	string temp;

		//	if (ParamName == "" || ParamValue == "")
		//		return;

		//	temp = "<b>" + System.Net.WebUtility.HtmlEncode(ParamName) + ":</b> " + System.Net.WebUtility.HtmlEncode(ParamValue);
		//	if (ParamUnit != "")
		//		temp = temp + " " + System.Net.WebUtility.HtmlEncode(ParamUnit);

		//	_stwr.WriteLine(temp + " <br>");
		//}

		//public void WriteParam(string ParamName, string ParamValue)
		//{
		//	WriteParam(ParamName, ParamValue, "");
		//}

		//public void WriteTab(System.Windows.Forms.ListView lListView, string TabComment = "")
		//{
		//	int n, m, i, j;
		//	System.Windows.Forms.ListViewItem itmX;

		//	if (lListView == null || lListView.Items.Count == 0)
		//		return;

		//	if (TabComment != "")
		//	{
		//		WriteString(System.Net.WebUtility.HtmlEncode(TabComment));
		//		WriteString("");
		//	}

		//	_stwr.WriteLine("<table border='1' cellspacing='0' cellpadding='1'>");

		//	n = lListView.Columns.Count;
		//	m = lListView.Items.Count;

		//	_stwr.WriteLine("<tr>");
		//	for (i = 0; i < n; i++)
		//		_stwr.WriteLine("<td><b>" + System.Net.WebUtility.HtmlEncode(lListView.Columns[i].Text) + "</b></td>");
		//	_stwr.WriteLine("</tr>");

		//	for (i = 0; i < m; i++)
		//	{
		//		itmX = lListView.Items[i];

		//		_stwr.WriteLine("<tr>");
		//		_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(itmX.Text) + "</td>");

		//		for (j = 1; j < n; j++)
		//			_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(itmX.SubItems[j].Text) + "</td>");

		//		_stwr.WriteLine("</tr>");
		//	}

		//	_stwr.WriteLine("</table>");
		//	_stwr.WriteLine("<br><br>");
		//}

		//public void WriteImage(string ImageName, string ImageComment = "")
		//{
		//	if (ImageName == "")
		//		return;

		//	if (ImageComment != "")
		//	{
		//		WriteString(System.Net.WebUtility.HtmlEncode(ImageComment));
		//		WriteString("");
		//	}

		//	_stwr.WriteLine("<img src='" + System.Net.WebUtility.HtmlEncode(ImageName) + "' border='0'>");
		//}

		//public void WriteImageLink(string ImageName, string ImageComment = "")
		//{
		//	if (ImageName == "")
		//		return;

		//	_stwr.WriteLine("<a href='" + System.Net.WebUtility.HtmlEncode(ImageName) + "'>" + System.Net.WebUtility.HtmlEncode(ImageComment) + "</a>");
		//}

		public void WritePoint(ReportPoint RepPoint)
		{
			if (RepPoint.Description != "")
				WriteString(System.Net.WebUtility.HtmlEncode(RepPoint.Description));

			Param(Resources.str00110, CommonFunctions.Degree2String(RepPoint.Lat, Degree2StringMode.DMSLat), "");
			Param(Resources.str00111, CommonFunctions.Degree2String(RepPoint.Lon, Degree2StringMode.DMSLon), "");

			if (RepPoint.Radius > 0)
			{
				Param(Resources.str00112, CommonFunctions.Degree2String(RepPoint.CenterLat, Degree2StringMode.DMSLat), "");
				Param(Resources.str00113, CommonFunctions.Degree2String(RepPoint.CenterLon, Degree2StringMode.DMSLon), "");
			}

			if (RepPoint.Height > 0)
				Param(Resources.str00114, GlobalVars.unitConverter.HeightToDisplayUnits(RepPoint.Height, eRoundMode.NEAREST).ToString(), GlobalVars.unitConverter.HeightUnit);
			//WriteParam(Resources.str00569, GlobalVars.unitConverter.HeightToDisplayUnits(RepPoint.Height, eRoundMode.NERAEST).ToString(), GlobalVars.unitConverter.HeightUnit);

			Param(Resources.str00116, NativeMethods.Modulus(Math.Round(RepPoint.TrueTrack, 2)).ToString(), "°");
			Param(Resources.str00124, NativeMethods.Modulus(Math.Round(RepPoint.MagnTrack, 2)).ToString(), "°");

			Param(Resources.str00125, NativeMethods.Modulus(Math.Round(RepPoint.ReverseTrueTrack, 2)).ToString(), "°");
			Param(Resources.str00126, NativeMethods.Modulus(Math.Round(RepPoint.ReverseMagnTrack, 2)).ToString(), "°");

			if (RepPoint.Radius > 0)
			{
				Param(Resources.str00117, GlobalVars.unitConverter.DistanceToDisplayUnits(RepPoint.Radius, eRoundMode.NEAREST).ToString(), GlobalVars.unitConverter.DistanceUnit);
				if (RepPoint.Turn > 0)
					Param(Resources.str00118, Resources.str00119, "");
				else
					Param(Resources.str00118, Resources.str00120, "");
				Param(Resources.str00121, Math.Round(RepPoint.turnAngle, 2).ToString(), "°");
				Param(Resources.str00122, GlobalVars.unitConverter.DistanceToDisplayUnits(RepPoint.TurnArcLen, eRoundMode.NEAREST).ToString(), GlobalVars.unitConverter.DistanceUnit);
			}

			if (RepPoint.DistToNext > 0)
				Param(Resources.str00123, GlobalVars.unitConverter.DistanceToDisplayUnits(RepPoint.DistToNext, eRoundMode.NEAREST).ToString(), GlobalVars.unitConverter.DistanceUnit);
			WriteString("");
		}

		//public void WriteTraceSegment(TraceSegment Segment, bool IsLastSegment)
		//public void WriteTraceSegment(Leg Segment, bool IsLastSegment)
		//{
		//    switch (Segment.SegmentCode)
		//    {
		//        case eSegmentType.straight:
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
		//        case eSegmentType.courseIntercept:
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
		//        case eSegmentType.directToFIX:
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

		public static void WriteTabToHTML(System.Windows.Forms.DataGridView gridView, string FileName, string TabComment = "")
		{
			if (gridView == null)
				return;

			int n, m, i, j;

			StreamWriter sw = File.CreateText(FileName);

			//(char)9;

			sw.WriteLine("<html>");
			sw.WriteLine("<head>");
			sw.WriteLine("<title>PANDA - " + TabComment + "</title>");
			sw.WriteLine("<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />");
			sw.WriteLine("<style>");
			sw.WriteLine("body {font-family: Arial, Sans-Serif; font-size:12;}");
			sw.WriteLine("table {font-family: Arial, Sans-Serif; font-size:10;}");
			sw.WriteLine("</style>");
			sw.WriteLine("</head>");
			sw.WriteLine("<body>");

			sw.WriteLine(System.Net.WebUtility.HtmlEncode(TabComment) + "<br>");
			sw.WriteLine("<br>");

			sw.WriteLine("<table border='1' cellspacing='0' cellpadding='1'>");

			n = gridView.Columns.Count;
			m = gridView.Rows.Count;

			sw.WriteLine("<tr>");
			for (i = 0; i < n; i++)
				sw.WriteLine("<td><b>" + System.Net.WebUtility.HtmlEncode(gridView.Columns[i].HeaderText) + "</b></td>");
			sw.WriteLine("</tr>");

			for (i = 0; i < m; i++)
			{
				System.Windows.Forms.DataGridViewRow row = gridView.Rows[i];

				sw.WriteLine("<tr>");
				for (j = 0; j < n; j++)
					sw.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(row.Cells[j].Value.ToString()) + "</td>");
				sw.WriteLine("</tr>");
			}

			sw.WriteLine("</table>");
			sw.WriteLine("</body>");
			sw.WriteLine("</html>");

			sw.Flush();
			sw.Dispose();
			sw = null;
		}

		public static void WriteTabToTXT(System.Windows.Forms.DataGridView gridView, string FileName, string TabComment = "")
		{
			if (gridView == null)
				return;

			int i, n = gridView.Columns.Count, m, maxLen = 0;

			string[] headersText = new string[n];
			int[] headersLen = new int[n];

			StreamWriter sw = File.CreateText(FileName);

			sw.WriteLine(TabComment);
			sw.WriteLine();

			for (i = 0; i < n; i++)
			{
				System.Windows.Forms.DataGridViewColumn column = gridView.Columns[i];
				headersText[i] = @"""" + column.HeaderText + @"""";
				headersLen[i] = headersText[i].Length;
				if (headersLen[i] > maxLen)
					maxLen = headersLen[i];
			}

			string strOut = "", tmpStr;

			for (i = 0; i < n; i++)
			{
				int j = maxLen - headersLen[i];
				m = Convert.ToInt32(j / 2);
				tmpStr = "";

				if (i < n)
					tmpStr = String.Empty.PadLeft(m) + "|";

				strOut = strOut + String.Empty.PadLeft(j - m) + headersText[i] + tmpStr;
			}

			sw.WriteLine(strOut);
			strOut = "";

			tmpStr = new string('-', maxLen) + "+";
			for (i = 1; i < n; i++)
				strOut = strOut + tmpStr;

			strOut = strOut + new string('-', maxLen);

			sw.WriteLine(strOut);

			m = gridView.RowCount;
			for (i = 0; i < m; i++)
			{
				System.Windows.Forms.DataGridViewRow row = gridView.Rows[i];

				tmpStr = row.Cells[0].Value.ToString();
				int tmpLen = tmpStr.Length;

				if (tmpLen > maxLen)
					strOut = tmpStr.Substring(0, maxLen - 1) + "*";
				else
					strOut = String.Empty.PadLeft(maxLen - tmpLen) + tmpStr;

				for (int j = 1; j < n; j++)
				{
					tmpStr = row.Cells[j].Value.ToString();

					tmpLen = tmpStr.Length;

					if (tmpLen > maxLen)
						tmpStr = tmpStr.Substring(0, maxLen - 1) + "*";
					else if (j < n - 1 || tmpLen > 0)
						tmpStr = String.Empty.PadLeft(maxLen - tmpLen) + tmpStr;

					strOut = strOut + "|" + tmpStr;
				}
				sw.WriteLine(strOut);
			}

			sw.Flush();
			sw.Dispose();
			sw = null;
		}

		public void SaveObstacleTable(List<ObstacleReport> obstacles, string tabComment = "")
		{
			if (tabComment != "")
			{
				WriteString(System.Net.WebUtility.HtmlEncode(tabComment));
				WriteString("");
			}

			string[] ColumnsHeaderText = new string[] { Resources.str02018, Resources.str02019, Resources.str02020 + " (" + GlobalVars.unitConverter.DistanceUnit + ")",
			Resources.str02021 + " (" + GlobalVars.unitConverter.DistanceUnit + ")", Resources.str02022 + " (" + GlobalVars.unitConverter.HeightUnit + ")",
			Resources.str02023 + " (" + GlobalVars.unitConverter.HeightUnit + ")", Resources.str02024 + " (" + GlobalVars.unitConverter.HeightUnit + ")",
			Resources.str02025 + " (" + GlobalVars.unitConverter.HeightUnit + ")", Resources.str02026 + " (" + GlobalVars.unitConverter.HeightUnit + ")", Resources.str02027};

			int i, n = ColumnsHeaderText.Length;

			_stwr.WriteLine("<table border='1' cellspacing='0' cellpadding='1'>");

			_stwr.WriteLine("<tr>");
			for (i = 0; i < n; i++)
				_stwr.WriteLine("<td><b>" + System.Net.WebUtility.HtmlEncode(ColumnsHeaderText[i]) + "</b></td>");
			_stwr.WriteLine("</tr>");

			var sortedObstacles = obstacles.OrderByDescending(obs => obs.ReqH).ToList();
			int m = sortedObstacles.Count;
			for (i = 0; i < m; i++)
			{
				string strArea;

				if (sortedObstacles[i].Prima)
					strArea = Resources.str02030;
				else
					strArea = Resources.str02031;

				_stwr.WriteLine("<tr>");
				_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(sortedObstacles[i].TypeName) + "</td>");
				_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(sortedObstacles[i].UnicalName) + "</td>");
				_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(GlobalVars.unitConverter.DistanceToDisplayUnits(sortedObstacles[i].Dist, eRoundMode.NEAREST).ToString()) + "</td>");
				_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(GlobalVars.unitConverter.DistanceToDisplayUnits(sortedObstacles[i].CLShift, eRoundMode.NEAREST).ToString()) + "</td>");
				_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(GlobalVars.unitConverter.HeightToDisplayUnits(sortedObstacles[i].Height, eRoundMode.NEAREST).ToString()) + "</td>");
				_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(GlobalVars.unitConverter.HeightToDisplayUnits(sortedObstacles[i].Moc, eRoundMode.NEAREST).ToString()) + "</td>");
				_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(GlobalVars.unitConverter.HeightToDisplayUnits(sortedObstacles[i].ReqH, eRoundMode.NEAREST).ToString()) + "</td>");
				_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(GlobalVars.unitConverter.HeightToDisplayUnits(sortedObstacles[i].HorAccuracy, eRoundMode.NEAREST).ToString()) + "</td>");
				_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(GlobalVars.unitConverter.HeightToDisplayUnits(sortedObstacles[i].VerAccuracy, eRoundMode.NEAREST).ToString()) + "</td>");
				_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(strArea) + "</td>");
				_stwr.WriteLine("</tr>");
			}

			_stwr.WriteLine("</table>");
			_stwr.WriteLine("<br><br>");
		}

		public static void SaveGeometry(string RepFileName, string RepFileTitle, ReportHeader pReport, ReportPoint[] reportPoints, double allRoutsLen)
		{
			CReportFile RoutsGeomRep = new CReportFile();
			//RoutsGeomRep.DerPtPrj = DER.pPtPrj[eRWY.ptEnd];
			//RoutsGeomRep.ThrPtPrj = DER.pPtPrj[eRWY.ptEnd];

			RoutsGeomRep.OpenFile(RepFileName + "_Geometry", Resources.str00107);
			RoutsGeomRep.WriteString(System.Net.WebUtility.HtmlEncode(Resources.str00030 + " - " + Resources.str00107));

			RoutsGeomRep.WriteString("");

			RoutsGeomRep.WriteString(System.Net.WebUtility.HtmlEncode(RepFileTitle));
			RoutsGeomRep.WriteHeader(pReport);

			RoutsGeomRep.WriteString("");
			RoutsGeomRep.WriteString("");

			int n = reportPoints.Length;
			for (int i = 0; i < n; i++)
				RoutsGeomRep.WritePoint(reportPoints[i]);

			RoutsGeomRep.WriteString("");

			RoutsGeomRep.Param(Resources.str00108, GlobalVars.unitConverter.DistanceToDisplayUnits(allRoutsLen, eRoundMode.NEAREST).ToString(), GlobalVars.unitConverter.DistanceUnit);

			RoutsGeomRep.CloseFile();
			RoutsGeomRep = null;
		}

		public static void SaveGeometry2(string RepFileName, string RepFileTitle, ReportHeader pReport, ReportPoint[] reportPoints, double allRoutsLen, ePBNClass pbnType)
		{
			CReportFile RoutsGeomRep = new CReportFile();
			//RoutsGeomRep.DerPtPrj = DER.pPtPrj[eRWY.ptEnd];
			//RoutsGeomRep.ThrPtPrj = DER.pPtPrj[eRWY.ptEnd];

			RoutsGeomRep.OpenFile(RepFileName + "_Geometry", Resources.str00107);
			RoutsGeomRep.WriteString(System.Net.WebUtility.HtmlEncode(Resources.str00030 + " - " + Resources.str00107));

			RoutsGeomRep.WriteString("");

			RoutsGeomRep.WriteString(System.Net.WebUtility.HtmlEncode(RepFileTitle));
			RoutsGeomRep.WriteHeader(pReport);

			RoutsGeomRep.WriteString("");
			RoutsGeomRep.WriteString("");


			var table = "<table width='800px' xmlns:x='http://www.w3.org/1999/xhtml' border='1' cellspacing='0' id='L72' class='ENR-table w70pc'>" +
					   "<thead>" +
				"<tr class='Table-row-type-1'>" +
				"<th colspan='2'> Route Designator<br>{RNP Type}</th>" +
				 "<th colspan='5'></th>" +
				"</tr>" +
				"<tr class='Table-row-type-2'>" +
				"<th colspan='2'>Significant Point Name</th>" +
				"<th colspan = '5' > Significant Point Coordinates</th>" +
				"</tr>" +
				"<tr class='Table-row-type-3'>" +
				"<th rowspan = '2' colspan='2'>{RNP Type}</th>" +
				"<th rowspan = '2' > Dist </th>" +
				"<th rowspan = '2' colspan='2'>" +
				"<div class='UpperAndLower'>" +
				"<table>" +
				"<tbody>" +
				"<tr>" +
				"<td class='Upper'>True course</td>" +
				"</tr>" +
				"<tr>" +
				"<td class='Lower'>Reverse true course</td>" +
				"</tr>" +
				"</tbody>" +
				"</table>" +
				"</div></th>" +
				"<th colspan = '2' rowspan='2'>" +
				"<div class='UpperAndLower'>" +
				"<table>" +
				"<tbody>" +
				"<tr>" +
				"<td class='Upper'>Magnetic course</td>" +
				"</tr>" +
				"<tr>" +
				"<td class='Lower'>Reverse magnetic true course</td>" +
				"</tr>" +
				"</tbody>" +
				"</table>" +
				"</div>" +
				"</th>" +
				"</tr>" +
				"</thead>" +
				"<tfoot>" +
				"</tfoot>" +
				"<tbody>";

			RoutsGeomRep.WriteString(table);
			int n = reportPoints.Length;
			RoutsGeomRep.WriteRouteName(pReport.Procedure, pbnType.ToString());

			for (int i = 0; i < n; i++)
			{
				RoutsGeomRep.WriteCoordinate(reportPoints[i]);
				RoutsGeomRep.WriteOtherParams(reportPoints[i], i == n - 1);
			}

			RoutsGeomRep.WriteString("	</tbody>" +
									 "</ table > ");

			RoutsGeomRep.WriteString("");

			RoutsGeomRep.Param(Resources.str00108, GlobalVars.unitConverter.DistanceToDisplayUnits(allRoutsLen, eRoundMode.NEAREST).ToString(), GlobalVars.unitConverter.DistanceUnit);

			RoutsGeomRep.CloseFile();
			RoutsGeomRep = null;
		}

		public void WriteRouteName(string routeName, string rnpType)
		{
			var html = "<tr class='Table-row-type-1'>" +
					   "<td colspan = '2'>" +
				"<p class='Route-designator line Route-designator'>" + routeName + "</p>" +
				"<p> (" + rnpType + ")</p>" +
				"</td>" +
				"<td colspan = '5'/>" +
				"</tr> ";

			_stwr.WriteLine(html);
		}

		public void WriteCoordinate(ReportPoint repPoint)
		{
			var lat = CommonFunctions.Degree2String(repPoint.Lat, Degree2StringMode.DMSLat);
			var longtitude = CommonFunctions.Degree2String(repPoint.Lon, Degree2StringMode.DMSLon);


			var html = "<tr class='Table-row-type-2 '>" +
					   "<td colspan='2'>▲" +
					   "<a href = '#' >" +
					   "<span>" + repPoint.Description + "</span>" +
					   "</a>" +
					   "</td>" +
					   "<td colspan = '5' > " + lat + "  " + longtitude + "</td>" +
					   "</tr>";

			_stwr.WriteLine(html);
		}

		public void WriteOtherParams(ReportPoint repPoint, bool isLast)
		{

			var distance = "";

			string trueTrack = "", magnTrack = "", reverseTrueTrack = "", reverseMagnTrack = "";

			if (!isLast)
			{
				trueTrack = NativeMethods.Modulus(Math.Round(repPoint.TrueTrack, 2)).ToString() + "°";
				magnTrack = NativeMethods.Modulus(Math.Round(repPoint.MagnTrack, 2)).ToString() + "°";

				reverseTrueTrack = NativeMethods.Modulus(Math.Round(repPoint.ReverseTrueTrack, 2)).ToString() + "°";
				reverseMagnTrack = NativeMethods.Modulus(Math.Round(repPoint.ReverseMagnTrack, 2)).ToString() + "°";
				distance = GlobalVars.unitConverter.DistanceToDisplayUnits(repPoint.DistToNext, eRoundMode.NEAREST).ToString() + " " + GlobalVars.unitConverter.DistanceUnit;
			}

			var html = "<tr class='Table-row-type-3 '>" +
					   "<td colspan = '2' > &nbsp; &nbsp;</td>" +
					"<td align = 'center'>" +
						"<span class='Route-segment-length'>" + distance + "</span>" +
		  "          </td>" +
				"<td align = 'center' colspan='2' >" +
					   "<div class='UpperAndLower'>" +
						   "<table>" +
							   "<tbody>" +
								   "<tr>" +
									   "<td class='Upper'>" +
										   "<span class='Route-segment-upper'>" + trueTrack + "</span>" +
										"</td>" +
									"</tr>" +
									"<tr>" +
										"<td class='Lower'>" +
										   "<span class='Route-segment-lower'>" + reverseTrueTrack + "</span>" +
									   "</td>" +
								   "</tr>" +
							   "</tbody>" +
						   "</table>" +
					   "</div>" +
				"</td>" +
				"<td colspan = '2' >" +
					   "<div class='UpperAndLower'>" +
					   "<table>" +
						   "<tbody>" +
						   "<tr>" +
							   "<td class='Upper'>" +
							   "<span class='Route-segment-upper'>" + magnTrack + "</span>" +
							   "</td>" +
						   "</tr>" +
						   "<tr>" +
							   "<td class='Lower'>" +
							   "<span class='Route-segment-lower'>" + reverseMagnTrack + "</span>" +
							   "</td>" +
						   "</tr>" +
						   "</tbody>" +
					   "</table>" +
					   "</div>" +
				"</td>" +
				"</tr>";

			_stwr.WriteLine(html);
		}
	}
}
