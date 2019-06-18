﻿using System;
using System.IO;
using Aran.PANDA.Departure.Properties;
using ESRI.ArcGIS.Geometry;
using Aran.PANDA.Reports;
using System.Reflection;

namespace Aran.PANDA.Departure
{
    [System.Runtime.InteropServices.ComVisible(false)]
    public class ReportFile : ReportBase
    {
        public IPoint ThrPtPrj;
		//public double RefHeight;

		public void WriteHeader(ReportHeader pReport)
        {
			Assembly currentAssem = Assembly.GetExecutingAssembly();

			WriteString(currentAssem.GetName().Version.ToString(), true, true);
			WriteString(GlobalVars.PANSOPSVersion, true, true);
			WriteString(System.Net.WebUtility.HtmlEncode("AIM Environment: " + pReport.Database), true, true);

			if (GlobalVars.settings.AnnexObstalce)
				WriteString("Obstacle source: Obstacle area obstacles.", true);
			else
				WriteString("Obstacle source: All vertical structures from DB.", true);

			if (pReport.Aerodrome != null && pReport.Aerodrome != "")
				WriteString(Resources.str00073 + pReport.Aerodrome, true);

            if (pReport.Procedure != null && pReport.Procedure != "")
				WriteString(Resources.str00821 + pReport.Procedure, true);

            //if (pReport.RWY != null && pReport.RWY != "")
            //	WriteString("<p>" + Resources.str822 + pReport.RWY);

            if (pReport.Category != null && pReport.Category != "")
                WriteString(Resources.str00823 + pReport.Category, true);

            WriteString("User name: " + GlobalVars.UserName, true);

            WriteString();

            _excellExporter.CreateRow().Text(Resources.str00702, true, false, 12).Text(DateTime.Now.Date.ToString("d"));
            WriteText("<p><b>" + Resources.str00702 + "</b>    " + DateTime.Now.Date.ToString("d") + "</br>");
            _excellExporter.CreateRow().Text(Resources.str00703, true, false, 12).Text(DateTime.Now.Date.ToString("d"));
            WriteText("<b>" + Resources.str00703 + "</b>    " + DateTime.Now.ToLongTimeString() + "</p>" + "</br>");

            //pReport.EffectiveDate = new DateTime(pReport.EffectiveDate.Year, pReport.EffectiveDate.Month, pReport.EffectiveDate.Day, 0, 0, 0);
            //WriteString("<b>" + Resources.str704 + pReport.EffectiveDate.ToString("d") + "</p>");
            //WriteString();

            WriteString();
            WriteString(Resources.str00806 + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit + ".", true, false);
            WriteString(Resources.str00807 + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit + ".", true, false);
            WriteString(Resources.str00808 + GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Unit + ".", true, false);
            WriteString();
        }

        public void WriteText(string text)
        {
            _stwr.Write(text);
        }
   
        public void WriteTab(string TabComment = "")
        {
            int n, m, i, j;
            System.Windows.Forms.ListViewItem itmX;

            if (lListView == null)		// || lListView.Items.Count == 0)
                return;

            Page(TabComment);
            if (TabComment != "")
            {
                HTMLString(TabComment, true, false);
                HTMLString("", true, false);
            }

            _stwr.WriteLine("<table border='1' cellspacing='0' cellpadding='1'>");

            n = lListView.Columns.Count;
            m = lListView.Items.Count;

            _excellExporter.CreateRow();
            _stwr.WriteLine("<tr>");
            for (i = 0; i < n; i++)
            {
                _excellExporter.Text(lListView.Columns[i].Text, true, false, 14);
                _stwr.WriteLine("<td><b>" + System.Net.WebUtility.HtmlEncode(lListView.Columns[i].Text) + "</b></td>");
            }
            _stwr.WriteLine("</tr>");

            if (m == 0)
            {
                _stwr.WriteLine("<tr>");
                _stwr.WriteLine("<td> - </td>");

                for (j = 1; j < n; j++)
                    _stwr.WriteLine("<td> - </td>");

                _stwr.WriteLine("</tr>");
            }

            for (i = 0; i < m; i++)
            {
                itmX = lListView.Items[i];

                _excellExporter.CreateRow();
                _stwr.WriteLine("<tr>");
                _excellExporter.Text(itmX.Text);
                _stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(itmX.Text) + "</td>");

                for (j = 1; j < n; j++)
                {
                    _excellExporter.Text(itmX.SubItems[j].Text);
                    _stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(itmX.SubItems[j].Text) + "</td>");
                }

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

            _stwr.WriteLine(System.Net.WebUtility.HtmlEncode("<a href='" + System.Net.WebUtility.HtmlEncode(ImageName) + "'>" + ImageComment) + "</a>");
        }

        public void WritePoint(ReportPoint RepPoint)
        {
            if (RepPoint.Description == "")
                return;

            _excellExporter.CreateRow();
            _excellExporter.H2(RepPoint.Description);
            HTMLMessage("[" + RepPoint.Description + "]");

            Param(Resources.str00564, Functions.Degree2String(RepPoint.Lat, Degree2StringMode.DMSLat), "");
            Param(Resources.str00565, Functions.Degree2String(RepPoint.Lon, Degree2StringMode.DMSLon), "");
            if (RepPoint.Radius > 0)
            {
                Param(Resources.str00566, Functions.Degree2String(RepPoint.CenterLat, Degree2StringMode.DMSLat), "");
                Param(Resources.str00567, Functions.Degree2String(RepPoint.CenterLon, Degree2StringMode.DMSLon), "");
            }

            Param(Resources.str00568, Functions.ConvertHeight(RepPoint.Height, eRoundMode.NEAREST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
            Param(Resources.str00569, Functions.ConvertHeight(RepPoint.Height - ThrPtPrj.Z, eRoundMode.NEAREST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);

            if (RepPoint.Direction >= 0.0)
                Param(Resources.str00577, Math.Round(RepPoint.Direction, 2).ToString(), "°");

            if (RepPoint.Radius > 0)
            {
                Param(Resources.str00572, Functions.ConvertDistance(RepPoint.Radius, eRoundMode.NEAREST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);
                if (RepPoint.Turn > 0)
                    Param(Resources.str00573, Resources.str00574, "");
                else
                    Param(Resources.str00573, Resources.str00575, "");
                Param(Resources.str00576, Math.Round(RepPoint.turnAngle, 2).ToString(), "°");
                Param(Resources.str00579, Functions.ConvertDistance(RepPoint.TurnArcLen, eRoundMode.NEAREST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);
            }

            if (RepPoint.DistToNext > 0.0)
                Param(Resources.str00578, Functions.ConvertDistance(RepPoint.DistToNext, eRoundMode.NEAREST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);
            WriteString("");
        }

        public void WriteTraceSegment(TraceSegment Segment, bool IsLastSegment)
        {
            WriteString();
            switch (Segment.SegmentCode)
            {
                case eSegmentType.straight:
                    WriteString(Resources.str00581 + Segment.RepComment, true);
                    Param(Resources.str00582, Segment.StCoords, "");

                    Param(Resources.str00568, Functions.ConvertHeight(Segment.HStart, eRoundMode.NEAREST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
                    Param(Resources.str00569, Functions.ConvertHeight(Segment.HStart - ThrPtPrj.Z, eRoundMode.NEAREST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);

                    Param(Resources.str00577, NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(Segment.ptIn, Segment.DirIn), 2)).ToString(), "°");
                    Param(Resources.str00578, Functions.ConvertDistance(Segment.Length, eRoundMode.NEAREST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);
                    WriteString("");

                    if (IsLastSegment)
                    {
                        WriteString(Resources.str00583 + Segment.RepComment, true);
                        Param(Resources.str00582, Segment.FinCoords, "");

                        Param(Resources.str00568, Functions.ConvertHeight(Segment.HFinish, eRoundMode.NEAREST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
                        Param(Resources.str00569, Functions.ConvertHeight(Segment.HFinish - ThrPtPrj.Z, eRoundMode.NEAREST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);

                        Param(Resources.str00577, NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(Segment.ptOut, Segment.DirOut), 2)).ToString(), "°");
                        WriteString("");
                    }

                    break;
                case eSegmentType.toHeading:
                case eSegmentType.directToFIX:
                    //case  eSegmentType.courseIntercept:
                    WriteString(Resources.str00584 + Segment.RepComment, true);
                    Param(Resources.str00582, Segment.StCoords, "");

                    Param(Resources.str00568, Functions.ConvertHeight(Segment.HStart, eRoundMode.NEAREST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
                    Param(Resources.str00569, Functions.ConvertHeight(Segment.HStart - ThrPtPrj.Z, eRoundMode.NEAREST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);

                    Param(Resources.str00577, NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(Segment.ptIn, Segment.DirIn), 2)).ToString(), "°");
                    Param(Resources.str00578, Functions.ConvertDistance(Segment.Length, eRoundMode.NEAREST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);
                    WriteString("");
                    Param(Resources.str00585, Segment.Center1Coords, "");
                    Param(Resources.str00572, Functions.ConvertDistance(Segment.Turn1R, eRoundMode.NEAREST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);
                    if ((Segment.Turn1Dir > 0))
                        Param(Resources.str00573, Resources.str00574, "");
                    else
                        Param(Resources.str00573, Resources.str00575, "");

                    Param(Resources.str00576, System.Math.Round(Segment.Turn1Angle, 2).ToString(), "°");
                    WriteString("");

                    if ((IsLastSegment))
                    {
                        WriteString(Resources.str00586 + Segment.RepComment, true);
                        Param(Resources.str00582, Segment.FinCoords, "");

                        Param(Resources.str00568, Functions.ConvertHeight(Segment.HFinish, eRoundMode.NEAREST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
                        Param(Resources.str00569, Functions.ConvertHeight(Segment.HFinish - ThrPtPrj.Z, eRoundMode.NEAREST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);

                        Param(Resources.str00577, System.Math.Round(NativeMethods.Modulus(Functions.Dir2Azt(Segment.ptOut, Segment.DirOut)), 2).ToString(), "°");
                        WriteString("");
                    }

                    break;
                case eSegmentType.courseIntercept:
                    WriteString(Resources.str00581 + Segment.RepComment, true);
                    Param(Resources.str00582, Segment.StCoords, "");

                    Param(Resources.str00568, Functions.ConvertHeight(Segment.HStart, eRoundMode.NEAREST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
                    Param(Resources.str00569, Functions.ConvertHeight(Segment.HStart - ThrPtPrj.Z, eRoundMode.NEAREST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);

                    Param(Resources.str00577, NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(Segment.ptIn, Segment.DirIn), 2)).ToString(), "°");
                    Param(Resources.str00578, Functions.ConvertDistance(Segment.BetweenTurns, eRoundMode.NEAREST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);
                    WriteString("");

                    WriteString(Resources.str00584 + Segment.RepComment, true);
                    Param(Resources.str00582, Segment.St1Coords, "");

                    Param(Resources.str00568, Functions.ConvertHeight(Segment.H1, eRoundMode.NEAREST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
                    Param(Resources.str00569, Functions.ConvertHeight(Segment.H1 - ThrPtPrj.Z, eRoundMode.NEAREST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);

                    Param(Resources.str00577, NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(Segment.ptIn, Segment.DirIn), 2)).ToString(), "°");
                    Param(Resources.str00578, Functions.ConvertDistance(Segment.Turn1Length, eRoundMode.NEAREST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);
                    WriteString("");

                    Param(Resources.str00585, Segment.Center1Coords, "");
                    Param(Resources.str00572, Functions.ConvertDistance(Segment.Turn1R, eRoundMode.NEAREST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);

                    if ((Segment.Turn1Dir > 0))
                        Param(Resources.str00573, Resources.str00574, "");
                    else
                        Param(Resources.str00573, Resources.str00575, "");

                    Param(Resources.str00576, System.Math.Round(Segment.Turn1Angle, 2).ToString(), "°");
                    WriteString("");

                    if ((IsLastSegment))
                    {
                        WriteString(Resources.str00586 + Segment.RepComment, true);
                        Param(Resources.str00582, Segment.FinCoords, "");

                        Param(Resources.str00568, Functions.ConvertHeight(Segment.HFinish, eRoundMode.NEAREST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
                        Param(Resources.str00569, Functions.ConvertHeight(Segment.HFinish - ThrPtPrj.Z, eRoundMode.NEAREST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);

                        Param(Resources.str00577, NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(Segment.ptOut, Segment.DirOut), 2)).ToString(), "°");
                        WriteString("");
                    }

                    break;
                case eSegmentType.turnAndIntercept:
                    WriteString(Resources.str00587 + Segment.RepComment, true);
                    Param(Resources.str00582, Segment.StCoords, "");

                    Param(Resources.str00568, Functions.ConvertHeight(Segment.HStart, eRoundMode.NEAREST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
                    Param(Resources.str00569, Functions.ConvertHeight(Segment.HStart - ThrPtPrj.Z, eRoundMode.NEAREST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);

                    Param(Resources.str00577, NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(Segment.ptIn, Segment.DirIn), 2)).ToString(), "°");
                    Param(Resources.str00578, Functions.ConvertDistance(Segment.Turn1Length, eRoundMode.NEAREST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);

                    WriteString("");
                    Param(Resources.str00588, Segment.Center1Coords, "");
                    Param(Resources.str00589, Functions.ConvertDistance(Segment.Turn1R, eRoundMode.NEAREST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);

                    if (Segment.Turn1Dir > 0)
                        Param(Resources.str00590, Resources.str00574, "");
                    else
                        Param(Resources.str00590, Resources.str00575, "");

                    Param(Resources.str00591, System.Math.Round(Segment.Turn1Angle, 2).ToString(), "°");
                    WriteString("");

                    WriteString(Resources.str00592 + Segment.RepComment, true);
                    Param(Resources.str00582, Segment.Fin1Coords, "");

                    Param(Resources.str00568, Functions.ConvertHeight(Segment.H1, eRoundMode.NEAREST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
                    Param(Resources.str00569, Functions.ConvertHeight(Segment.H1 - ThrPtPrj.Z, eRoundMode.NEAREST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);

                    if (Segment.Turn2R > 0)
                    {
                        Param(Resources.str00577, "≈" + NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(Segment.PtCenter1, Segment.DirBetween), 2)).ToString(), "°");
                        Param(Resources.str00578, Functions.ConvertDistance(Segment.BetweenTurns, eRoundMode.NEAREST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);

                        WriteString("");

                        WriteString(Resources.str00593 + Segment.RepComment, true);
                        Param(Resources.str00582, Segment.St2Coords, "");

                        Param(Resources.str00568, Functions.ConvertHeight(Segment.H2, eRoundMode.NEAREST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
                        Param(Resources.str00569, Functions.ConvertHeight(Segment.H2 - ThrPtPrj.Z, eRoundMode.NEAREST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);

                        Param(Resources.str00577, "≈" + NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(Segment.PtCenter2, Segment.DirBetween), 2)).ToString(), "°");
                        Param(Resources.str00578, Functions.ConvertDistance(Segment.Turn2Length, eRoundMode.NEAREST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);

                        WriteString("");
                        Param(Resources.str00594, Segment.Center2Coords, "");
                        Param(Resources.str00595, Functions.ConvertDistance(Segment.Turn2R, eRoundMode.NEAREST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);

                        if (Segment.Turn2Dir > 0)
                            Param(Resources.str00596, Resources.str00574, "");
                        else
                            Param(Resources.str00596, Resources.str00575, "");

                        Param(Resources.str00597, System.Math.Round(Segment.Turn2Angle, 2).ToString(), "°");
                        WriteString("");
                    }

                    if (IsLastSegment)
                    {
                        WriteString(Resources.str00598 + Segment.RepComment, true);
                        Param(Resources.str00582, Segment.FinCoords, "");

                        Param(Resources.str00568, Functions.ConvertHeight(Segment.HFinish, eRoundMode.NEAREST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
                        Param(Resources.str00569, Functions.ConvertHeight(Segment.HFinish - ThrPtPrj.Z, eRoundMode.NEAREST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);

                        Param(Resources.str00577, NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(Segment.ptOut, Segment.DirOut), 2)).ToString(), "°");
                        WriteString("");
                    }
                    break;
            }
        }

        public static ReportFile SaveGeometry(string RepFileName, string RepFileTitle, ReportHeader pReport, RWYType DER, ReportPoint[] reportPoints, double allRoutsLen, bool guaded)
        {
            ReportFile RoutsGeomRep = new ReportFile();

            RoutsGeomRep.ThrPtPrj = DER.pPtPrj[eRWY.PtDER];

            RoutsGeomRep.OpenFile(RepFileName + "_Geometry", RepFileTitle + ": " + Resources.str00517);
            if (guaded)
                RoutsGeomRep.WriteString(Resources.str15479 + " - " + RepFileTitle + ": " + Resources.str00517, true);
            else
                RoutsGeomRep.WriteString(Resources.str15271 + " - " + RepFileTitle + ": " + Resources.str00517, true);

            RoutsGeomRep.WriteString("");
            RoutsGeomRep.WriteString(RepFileTitle, true);
            RoutsGeomRep.WriteHeader(pReport);

            RoutsGeomRep.WriteString("");
            RoutsGeomRep.WriteString("");

            int n = reportPoints.Length;
            for (int i = 0; i < n; i++)
                RoutsGeomRep.WritePoint(reportPoints[i]);

            RoutsGeomRep.WriteString("");

            RoutsGeomRep.Param(Resources.str00519, Functions.ConvertDistance(allRoutsLen, eRoundMode.NEAREST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);

            RoutsGeomRep.CloseFile();
            return RoutsGeomRep;
        }

    }
}