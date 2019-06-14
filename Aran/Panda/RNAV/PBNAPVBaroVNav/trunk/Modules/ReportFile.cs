using System;
using System.IO;
using Aran.PANDA.Common;
using Aran.Geometries;
using Aran.PANDA.RNAV.PBNAPVBaroVNav.Properties;
using Aran.PANDA.Reports;
using System.Windows.Forms;

namespace Aran.PANDA.RNAV.PBNAPVBaroVNav
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public class ReportFile : ReportBase
	{
		public void WriteHeader()
		{
			//Paragragh(GlobalVars.thisAssemName.Name +" "+ GlobalVars.thisAssemName.Version.ToString(), true, true);
			Paragragh("PANDA " + Resources.str00033 /*+ " " + GlobalVars.thisAssemName.Version.ToString()*/, true, true);
			Paragragh(GlobalVars.PANSOPSVersion, true, true);

			WriteString("<p>User name: " + System.Net.WebUtility.HtmlEncode(GlobalVars.UserName) + "<br>");

			WriteString("<p><b>" + Resources.str00102 + "</b>    " + DateTime.Now.Date.ToString("d"));
			WriteString("<b>" + Resources.str00103 + "</b>    " + DateTime.Now.ToLongTimeString() + "</p>");

			//pReport.EffectiveDate = new DateTime(pReport.EffectiveDate.Year, pReport.EffectiveDate.Month, pReport.EffectiveDate.Day, 0, 0, 0);
			//WriteString("<b>" + Resources.str704 + pReport.EffectiveDate.ToString("d") + "</p>");

			WriteString("");
			//WriteString("<p>" + Resources.str00104 + GlobalVars.unitConverter.DistanceUnit + ".");
			//WriteString(Resources.str00105 + GlobalVars.unitConverter.HeightUnit + ".");
			//WriteString(Resources.str00106 + GlobalVars.unitConverter.SpeedUnit + ".</p>");
			//WriteString("");
		}

		public void WriteReportHeader(ReportHeader pReport)
		{
			Paragragh("PANDA " + Resources.str00033, true, true);
			Paragragh(GlobalVars.PANSOPSVersion, true, true);
			Paragragh("AIM Environment: " + pReport.Database, true, true);

			if (pReport.Aerodrome != null && pReport.Aerodrome != "")
				Paragragh(Resources.str00100 + pReport.Aerodrome, true);

			if (pReport.Procedure != null && pReport.Procedure != "")
				Paragragh(Resources.str00101 + pReport.Procedure, true);
		}

		public void WriteHeader(ReportHeader pReport)
		{
			Paragragh("PANDA " + Resources.str00033, true, true);
			Paragragh(GlobalVars.PANSOPSVersion, true, true);
			Paragragh("AIM Environment: " + pReport.Database, true, true);

			if (pReport.Aerodrome != null && pReport.Aerodrome != "")
				Paragragh(Resources.str00100 + pReport.Aerodrome, true);

			if (pReport.Procedure != null && pReport.Procedure != "")
				Paragragh(Resources.str00101 + pReport.Procedure, true);

			WriteString("<p>User name: " + System.Net.WebUtility.HtmlEncode(GlobalVars.UserName) + "<br>");

			WriteString("<p><b>" + Resources.str00102 + "</b>    " + DateTime.Now.Date.ToString("d"));
			WriteString("<b>" + Resources.str00103 + "</b>    " + DateTime.Now.ToLongTimeString() + "</p>");

			//pReport.EffectiveDate = new DateTime(pReport.EffectiveDate.Year, pReport.EffectiveDate.Month, pReport.EffectiveDate.Day, 0, 0, 0);
			//WriteString("<b>" + Resources.str704 + pReport.EffectiveDate.ToString("d") + "</p>");

			WriteString("");
			WriteString("<p>" + Resources.str00104 + GlobalVars.unitConverter.DistanceUnit + ".");
			WriteString(Resources.str00105 + GlobalVars.unitConverter.HeightUnit + ".");
			WriteString(Resources.str00106 + GlobalVars.unitConverter.SpeedUnit + ".</p>");
			WriteString("");
		}

		public void SaveObstacles(string TabComment, System.Windows.Forms.DataGridView gridView, ObstacleContainer Obstacles)
		{
			Page(TabComment);
			if (TabComment != "")
			{
				HTMLString(TabComment, true);
				HTMLString();
			}

			_stwr.WriteLine("<table border='1' cellspacing='0' cellpadding='1'>");

			int n = gridView.Columns.Count;
			int m = Obstacles.Parts.Length;

			_excellExporter.CreateRow();
			_stwr.WriteLine("<tr>");

			for (int i = 0; i < n; i++)
			{
				_excellExporter.Text(gridView.Columns[i].HeaderText, true, false, 14);
				_stwr.WriteLine("<td><b>" + System.Net.WebUtility.HtmlEncode(gridView.Columns[i].HeaderText) +
								"</b></td>");
			}

			_stwr.WriteLine("</tr>");

			if (m == 0)
			{
				_stwr.WriteLine("<tr>");
				_stwr.WriteLine("<td> - </td>");

				for (int i = 1; i < n; i++)
					_stwr.WriteLine("<td> - </td>");

				_stwr.WriteLine("</tr>");
			}

			for (int i = 0; i < m; i++)
			{
				_excellExporter.CreateRow();
				_stwr.WriteLine("<tr>");

				string strArea;

				if (Obstacles.Parts[i].Prima)
					strArea = Resources.str02030;
				else
					strArea = Resources.str02031;

				_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(Obstacles.Obstacles[Obstacles.Parts[i].Owner].TypeName) + "</td>");
				_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(Obstacles.Obstacles[Obstacles.Parts[i].Owner].UnicalName) + "</td>");
				_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(GlobalVars.unitConverter.DistanceToDisplayUnits(Obstacles.Parts[i].Dist, eRoundMode.NEAREST).ToString()) + "</td>");
				_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(GlobalVars.unitConverter.DistanceToDisplayUnits(Obstacles.Parts[i].CLShift, eRoundMode.NEAREST).ToString()) + "</td>");
				_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Parts[i].Height, eRoundMode.NEAREST).ToString()) + "</td>");
				_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Parts[i].MOC, eRoundMode.NEAREST).ToString()) + "</td>");
				_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Parts[i].ReqH, eRoundMode.NEAREST).ToString()) + "</td>");
				_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Obstacles[Obstacles.Parts[i].Owner].HorAccuracy, eRoundMode.NEAREST).ToString()) + "</td>");
				_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Obstacles[Obstacles.Parts[i].Owner].VertAccuracy, eRoundMode.NEAREST).ToString()) + "</td>");
				_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(strArea) + "</td>");
				_stwr.WriteLine("</tr>");

				_excellExporter.Text(Obstacles.Obstacles[Obstacles.Parts[i].Owner].TypeName);
				_excellExporter.Text(Obstacles.Obstacles[Obstacles.Parts[i].Owner].UnicalName);
				_excellExporter.Text(GlobalVars.unitConverter.DistanceToDisplayUnits(Obstacles.Parts[i].Dist, eRoundMode.NEAREST).ToString());
				_excellExporter.Text(GlobalVars.unitConverter.DistanceToDisplayUnits(Obstacles.Parts[i].CLShift, eRoundMode.NEAREST).ToString());
				_excellExporter.Text(GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Parts[i].Height, eRoundMode.NEAREST).ToString());
				_excellExporter.Text(GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Parts[i].MOC, eRoundMode.NEAREST).ToString());
				_excellExporter.Text(GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Parts[i].ReqH, eRoundMode.NEAREST).ToString());
				_excellExporter.Text(GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Obstacles[Obstacles.Parts[i].Owner].HorAccuracy, eRoundMode.NEAREST).ToString());
				_excellExporter.Text(GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Obstacles[Obstacles.Parts[i].Owner].VertAccuracy, eRoundMode.NEAREST).ToString());
				_excellExporter.Text(strArea);
			}

			_stwr.WriteLine("</table>");
			_stwr.WriteLine("<br><br>");
		}

		public void WriteTab(System.Windows.Forms.ListView lListView, string TabComment = "")
		{
			int n, m, i, j;
			
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

			_excellExporter.CreateRow();
			_stwr.WriteLine("<tr>");

			for (i = 0; i < n; i++)
			{
				_excellExporter.Text(lListView.Columns[i].Text, true, false, 14);
				_stwr.WriteLine("<td><b>" + System.Net.WebUtility.HtmlEncode(lListView.Columns[i].Text) + "</b></td>");
			}

			_stwr.WriteLine("</tr>");

			for (i = 0; i < m; i++)
			{
				ListViewItem itmX = lListView.Items[i];

				_excellExporter.CreateRow();
				_stwr.WriteLine("<tr>");

				_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(itmX.Text) + "</td>");
				_excellExporter.Text(itmX.Text);

				for (j = 1; j < n; j++)
				{
					_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(itmX.SubItems[j].Text) + "</td>");
					_excellExporter.Text(itmX.SubItems[j].Text);
				}

				_stwr.WriteLine("</tr>");
			}

			_stwr.WriteLine("</table>");
			_stwr.WriteLine("<br><br>");
		}

		public void WriteTab(System.Windows.Forms.DataGridView gridView, string TabComment = "")
		{
			int n, m, i, j;

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

				for (j = 0; j < n; j++)
					if (row.Cells[j].Value == null)
						_stwr.WriteLine("<td>" + "" + "</td>");
					else
						_stwr.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(row.Cells[j].Value.ToString()) + "</td>");

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
			if (RepPoint.Description != "")
				WriteString(System.Net.WebUtility.HtmlEncode(RepPoint.Description));

			Param(Resources.str00110, Functions.Degree2String(RepPoint.Lat, Degree2StringMode.DMSLat), "");
			Param(Resources.str00111, Functions.Degree2String(RepPoint.Lon, Degree2StringMode.DMSLon), "");

			if (RepPoint.Radius > 0)
			{
				Param(Resources.str00112, Functions.Degree2String(RepPoint.CenterLat, Degree2StringMode.DMSLat), "");
				Param(Resources.str00113, Functions.Degree2String(RepPoint.CenterLon, Degree2StringMode.DMSLon), "");
			}

			if (RepPoint.Height > 0)
				Param(Resources.str00114, GlobalVars.unitConverter.HeightToDisplayUnits(RepPoint.Height, eRoundMode.NEAREST).ToString(), GlobalVars.unitConverter.HeightUnit);
			//Param(Resources.str00115, GlobalVars.unitConverter.HeightToDisplayUnits(RepPoint.Height, eRoundMode.NERAEST).ToString(), GlobalVars.unitConverter.HeightUnit);

			Param(Resources.str00116, Math.Round(RepPoint.TrueCourse, 2).ToString(), "°");

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


		static public void WriteTabToTXT(ListView lListView, string FileName, string TabComment = "")
		{
			if (lListView == null || lListView.Items.Count == 0)
				return;

			StreamWriter sw = new StreamWriter(FileName);

			sw.WriteLine("PANDA " + Resources.str00033);
			sw.WriteLine(GlobalVars.PANSOPSVersion);

			sw.WriteLine("User name: " + System.Net.WebUtility.HtmlEncode(GlobalVars.UserName));

			sw.WriteLine(Resources.str00102 + "    " + DateTime.Now.Date.ToString("d"));
			sw.WriteLine(Resources.str00103 + "    " + DateTime.Now.ToLongTimeString());

			//pReport.EffectiveDate = new DateTime(pReport.EffectiveDate.Year, pReport.EffectiveDate.Month, pReport.EffectiveDate.Day, 0, 0, 0);
			//WriteString("<b>" + Resources.str704 + pReport.EffectiveDate.ToString("d") + "</p>");

			sw.WriteLine();
			sw.WriteLine(Resources.str00104 + GlobalVars.unitConverter.DistanceUnit + ".");
			sw.WriteLine(Resources.str00105 + GlobalVars.unitConverter.HeightUnit + ".");
			sw.WriteLine(Resources.str00106 + GlobalVars.unitConverter.SpeedUnit + ".");
			sw.WriteLine();

			if (TabComment != "")
			{
				sw.WriteLine(TabComment);
				sw.WriteLine();
			}

			int i, n = lListView.Columns.Count, m, maxLen = 0;

			string[] HeadersText = new string[n];
			int[] HeadersLen = new int[n];

			for (i = 0; i < n; i++)
			{
				ColumnHeader hdrX = lListView.Columns[i];
				HeadersText[i] = @"""" + hdrX.Text + @"""";
				HeadersLen[i] = HeadersText[i].Length;
				if (HeadersLen[i] > maxLen)
					maxLen = HeadersLen[i];
			}

			string StrOut = "", tmpStr;

			for (i = 0; i < n; i++)
			{
				int j = maxLen - HeadersLen[i];
				m = j / 2;
				if (i < n)
					tmpStr = System.String.Empty.PadLeft(m) + "|";
				else
					tmpStr = "";

				StrOut = StrOut + System.String.Empty.PadLeft(j - m) + HeadersText[i] + tmpStr;
			}

			sw.WriteLine(StrOut);
			StrOut = "";
			tmpStr = new string('-', maxLen) + "+";
			for (i = 1; i < n; i++)
				StrOut = StrOut + tmpStr;

			StrOut = StrOut + new string('-', maxLen);
			sw.WriteLine(StrOut);

			m = lListView.Items.Count;
			for (i = 0; i < m; i++)
			{
				ListViewItem itmX = lListView.Items[i];

				int TmpLen = itmX.Text.Length;
				if (TmpLen > maxLen)
					StrOut = itmX.Text.Substring(0, maxLen - 1) + "*";
				else
					StrOut = System.String.Empty.PadLeft(maxLen - TmpLen) + itmX.Text;

				for (int j = 1; j < n; j++)
				{
					tmpStr = itmX.SubItems[j].Text;

					TmpLen = tmpStr.Length;

					if (TmpLen > maxLen)
						tmpStr = tmpStr.Substring(0, maxLen - 1) + "*";
					else if (j < n - 1 || TmpLen > 0)
						tmpStr = System.String.Empty.PadLeft(maxLen - TmpLen) + tmpStr;

					StrOut = StrOut + "|" + tmpStr;
				}
				sw.WriteLine(StrOut);
			}
			sw.WriteLine();

			sw.Flush();
			sw.Dispose();
			sw = null;
		}

		public static void WriteTabToTXT(System.Windows.Forms.DataGridView gridView, string FileName, string TabComment = "")
		{
			if (gridView == null)
				return;

			StreamWriter sw = File.CreateText(FileName);

			if (TabComment != "")
			{
				sw.WriteLine(TabComment);
				sw.WriteLine();
			}

			int i, n = gridView.Columns.Count, m, maxLen = 0;
			string[] headersText = new string[n];
			int[] headersLen = new int[n];

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
				m = j / 2;
				//m = Convert.ToInt32(j / 2);
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

		public static ReportFile SaveGeometry(string RepFileName, string RepFileTitle, ReportHeader pReport, ReportPoint[] reportPoints, double allRoutsLen)
		{
			ReportFile RoutsGeomRep = new ReportFile();
			//RoutsGeomRep.DerPtPrj = DER.pPtPrj[eRWY.ptEnd];
			//RoutsGeomRep.ThrPtPrj = DER.pPtPrj[eRWY.ptEnd];

			RoutsGeomRep.OpenFile(RepFileName + "_Geometry", Resources.str00107);
			RoutsGeomRep.WriteString(System.Net.WebUtility.HtmlEncode(Resources.str00033 + " - " + Resources.str00107));

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
			return RoutsGeomRep;
		}
	}
}

/*
─────────────────────────▄▀▄  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─▀█▀█▄  
─────────────────────────█──█──█  
─────────────────────────█▄▄█──▀█  
────────────────────────▄█──▄█▄─▀█  
────────────────────────█─▄█─█─█─█  
────────────────────────█──█─█─█─█  
────────────────────────█──█─█─█─█  
────▄█▄──▄█▄────────────█──▀▀█─█─█  
──▄█████████────────────▀█───█─█▄▀  
─▄███████████────────────██──▀▀─█  
▄█████████████────────────█─────█  
██████████───▀▀█▄─────────▀█────█  
████████───▀▀▀──█──────────█────█  
██████───────██─▀█─────────█────█  
████──▄──────────▀█────────█────█ Look dude,
███──█──────▀▀█───▀█───────█────█ a good code!
███─▀─██──────█────▀█──────█────█  
███─────────────────▀█─────█────█  
███──────────────────█─────█────█  
███─────────────▄▀───█─────█────█  
████─────────▄▄██────█▄────█────█  
████────────██████────█────█────█  
█████────█──███████▀──█───▄█▄▄▄▄█  
██▀▀██────▀─██▄──▄█───█───█─────█  
██▄──────────██████───█───█─────█  
─██▄────────────▄▄────█───█─────█  
─███████─────────────▄█───█─────█  
──██████─────────────█───█▀─────█  
──▄███████▄─────────▄█──█▀──────█  
─▄█─────▄▀▀▀█───────█───█───────█  
▄█────────█──█────▄███▀▀▀▀──────█  
█──▄▀▀────────█──▄▀──█──────────█  
█────█─────────█─────█──────────█  
█────────▀█────█─────█─────────██  
█───────────────█──▄█▀─────────█  
█──────────██───█▀▀▀───────────█  
█───────────────█──────────────█  
█▄─────────────██──────────────█  
─█▄────────────█───────────────█  
──██▄────────▄███▀▀▀▀▀▄────────█  
─█▀─▀█▄────────▀█──────▀▄──────█  
─█────▀▀▀▀▄─────█────────▀─────█
*/