using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.PANDA.Common;
using Aran.PANDA.RNAV.Enroute.VD.Properties;

namespace Aran.PANDA.RNAV.Enroute.VD
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public class CReportFile
	{
		private const string ReportFileExt = ".htm";
		//public Aran.Geometries.Point DerPtPrj;
		//public Aran.Geometries.Point ThrPtPrj;

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
			WriteString("<p><b>" + System.Net.WebUtility.HtmlEncode(Resources.str00040) + "</b>");
			WriteString("<p><b>" + System.Net.WebUtility.HtmlEncode(GlobalVars.PANSOPSVersion) + "</b>");

			WriteString("<p><b>AIM Environment: " + System.Net.WebUtility.HtmlEncode(pReport.Database) + "</b>");


			if (pReport.Procedure != null && pReport.Procedure != "")
				WriteString(System.Net.WebUtility.HtmlEncode(Resources.str00100 + pReport.Procedure) + "</p>");

			WriteString("<p>User name: " + System.Net.WebUtility.HtmlEncode(GlobalVars.UserName) + "<br>");

			WriteString("<p><b>" + Resources.str00101 + "</b>    " + DateTime.Now.Date.ToString("d"));
			WriteString("<b>" + Resources.str00102 + "</b>    " + DateTime.Now.ToLongTimeString() + "</p>");

			//pReport.EffectiveDate = new DateTime(pReport.EffectiveDate.Year, pReport.EffectiveDate.Month, pReport.EffectiveDate.Day, 0, 0, 0);
			//WriteString("<b>" + Resources.str704 + pReport.EffectiveDate.ToString("d") + "</p>");

			WriteString("");
			WriteString("<p>" + Resources.str00103 + GlobalVars.unitConverter.DistanceUnit + ".");
			WriteString(Resources.str00104 + GlobalVars.unitConverter.HeightUnit + ".");
			WriteString(Resources.str00105 + GlobalVars.unitConverter.SpeedUnit + ".</p>");
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

		public void WriteParam(string ParamName, string ParamValue, string ParamUnit)
		{
			string temp;

			if (ParamName == "" || ParamValue == "")
				return;

			temp = "<b>" + System.Net.WebUtility.HtmlEncode(ParamName) + ":</b> " + System.Net.WebUtility.HtmlEncode(ParamValue);
			if (ParamUnit != "")
				temp = temp + " " + System.Net.WebUtility.HtmlEncode(ParamUnit);

			_stwr.WriteLine(temp + " <br>");
		}

		public void WriteParam(string ParamName, string ParamValue)
		{
			WriteParam(ParamName, ParamValue, "");
		}

		public void WriteTab(System.Windows.Forms.ListView lListView, string TabComment = "")
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

				for (j = 0; j < n; j++)
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

		//public void WritePoint(ReportPoint RepPoint)
		//{
		//		WriteString(System.Net.WebUtility.HtmlEncode(RepPoint.Description));

		//	WriteParam(Resources.str00110, ARANFunctions.Degree2String(RepPoint.Lat, Degree2StringMode.DMSLat), "");
		//	WriteParam(Resources.str00111, ARANFunctions.Degree2String(RepPoint.Lon, Degree2StringMode.DMSLon), "");

		//	WriteParam(Resources.str00114, GlobalVars.unitConverter.HeightToDisplayUnits(RepPoint.Altitude, eRoundMode.NERAEST).ToString(), GlobalVars.unitConverter.HeightUnit);

		//	WriteParam(Resources.str00116, NativeMethods.Modulus(Math.Round(RepPoint.TrueTrack, 2)).ToString(), "°");
		//	WriteParam(Resources.str00117, NativeMethods.Modulus(Math.Round(RepPoint.MagnTrack, 2)).ToString(), "°");

		//	if (RepPoint.MOC > 0)
		//	{
		//		WriteParam(Resources.str02023, GlobalVars.unitConverter.HeightToDisplayUnits(RepPoint.MOC, eRoundMode.NERAEST).ToString(), GlobalVars.unitConverter.HeightUnit);
		//		WriteParam(Resources.str02024, GlobalVars.unitConverter.HeightToDisplayUnits(RepPoint.MOCA, eRoundMode.NERAEST).ToString(), GlobalVars.unitConverter.HeightUnit);
		//	}

		//	if (RepPoint.DistToNext > 0)
		//		WriteParam(Resources.str00123, GlobalVars.unitConverter.DistanceToDisplayUnits(RepPoint.DistToNext, eRoundMode.NERAEST).ToString(), GlobalVars.unitConverter.DistanceUnit);
		//	WriteString("");
		//}

		public void WriteSegment(Segment segment)
		{
			WriteParam(Resources.str02012, segment.Start.Name, "");
			WriteParam(Resources.str00110, ARANFunctions.Degree2String(segment.Start.GeoPt.Y, Degree2StringMode.DMSLat), "");
			WriteParam(Resources.str00111, ARANFunctions.Degree2String(segment.Start.GeoPt.X, Degree2StringMode.DMSLon), "");
			WriteParam(Resources.str00112, segment.StartVOR.CallSign, "");
			WriteParam(Resources.str00110, ARANFunctions.Degree2String(segment.StartVOR.pPtGeo.Y, Degree2StringMode.DMSLat), "");
			WriteParam(Resources.str00111, ARANFunctions.Degree2String(segment.StartVOR.pPtGeo.X, Degree2StringMode.DMSLon), "");
			WriteParam(Resources.str01211, NativeMethods.Modulus(Math.Round(segment.NavDirStart, 2)).ToString(), "°");
			WriteParam(Resources.str01204, GlobalVars.unitConverter.DistanceToDisplayUnits(segment.Dstart, eRoundMode.NEAREST).ToString(), GlobalVars.unitConverter.DistanceUnit);
			WriteParam(Resources.str01205, GlobalVars.unitConverter.DistanceToDisplayUnits(segment.D1start, eRoundMode.NEAREST).ToString(), GlobalVars.unitConverter.DistanceUnit);
			WriteParam(Resources.str01206, GlobalVars.unitConverter.DistanceToDisplayUnits(segment.D2start, eRoundMode.NEAREST).ToString(), GlobalVars.unitConverter.DistanceUnit);
			WriteParam(Resources.str01208, GlobalVars.unitConverter.DistanceToDisplayUnits(segment.Start.ATT, eRoundMode.NEAREST).ToString(), GlobalVars.unitConverter.DistanceUnit);
			WriteParam(Resources.str01207, GlobalVars.unitConverter.DistanceToDisplayUnits(segment.Start.XTT, eRoundMode.NEAREST).ToString(), GlobalVars.unitConverter.DistanceUnit);
			WriteParam(Resources.str01209, GlobalVars.unitConverter.DistanceToDisplayUnits(segment.Start.ASW_L, eRoundMode.NEAREST).ToString(), GlobalVars.unitConverter.DistanceUnit);
			WriteString("");

			WriteParam(Resources.str02013, segment.End.Name, "");
			WriteParam(Resources.str00110, ARANFunctions.Degree2String(segment.End.GeoPt.Y, Degree2StringMode.DMSLat), "");
			WriteParam(Resources.str00111, ARANFunctions.Degree2String(segment.End.GeoPt.X, Degree2StringMode.DMSLon), "");
			WriteParam(Resources.str00112, segment.EndVOR.CallSign, "");
			WriteParam(Resources.str00110, ARANFunctions.Degree2String(segment.EndVOR.pPtGeo.Y, Degree2StringMode.DMSLat), "");
			WriteParam(Resources.str00111, ARANFunctions.Degree2String(segment.EndVOR.pPtGeo.X, Degree2StringMode.DMSLon), "");
			WriteParam(Resources.str01211, NativeMethods.Modulus(Math.Round(segment.NavDirEnd, 2)).ToString(), "°");
			WriteParam(Resources.str01204, GlobalVars.unitConverter.DistanceToDisplayUnits(segment.Dend, eRoundMode.NEAREST).ToString(), GlobalVars.unitConverter.DistanceUnit);
			WriteParam(Resources.str01205, GlobalVars.unitConverter.DistanceToDisplayUnits(segment.D1end, eRoundMode.NEAREST).ToString(), GlobalVars.unitConverter.DistanceUnit);
			WriteParam(Resources.str01206, GlobalVars.unitConverter.DistanceToDisplayUnits(segment.D2end, eRoundMode.NEAREST).ToString(), GlobalVars.unitConverter.DistanceUnit);
			WriteParam(Resources.str01208, GlobalVars.unitConverter.DistanceToDisplayUnits(segment.End.ATT, eRoundMode.NEAREST).ToString(), GlobalVars.unitConverter.DistanceUnit);
			WriteParam(Resources.str01207, GlobalVars.unitConverter.DistanceToDisplayUnits(segment.End.XTT, eRoundMode.NEAREST).ToString(), GlobalVars.unitConverter.DistanceUnit);
			WriteParam(Resources.str01209, GlobalVars.unitConverter.DistanceToDisplayUnits(segment.End.ASW_L, eRoundMode.NEAREST).ToString(), GlobalVars.unitConverter.DistanceUnit);
			WriteString("");

			WriteParam(Resources.str00116, NativeMethods.Modulus(Math.Round(segment.TrueTrack, 2)).ToString(), "°");
			WriteParam(Resources.str00117, NativeMethods.Modulus(Math.Round(segment.MagnTrack, 2)).ToString(), "°");

			WriteParam(Resources.str02016, GlobalVars.unitConverter.DistanceToDisplayUnits(segment.Length, eRoundMode.NEAREST).ToString(), GlobalVars.unitConverter.DistanceUnit);

			WriteParam(Resources.str02017, GlobalVars.unitConverter.HeightToDisplayUnits(segment.Altitude, eRoundMode.NEAREST).ToString(), GlobalVars.unitConverter.HeightUnit);
			WriteParam(Resources.str02023, GlobalVars.unitConverter.HeightToDisplayUnits(segment.MOC, eRoundMode.NEAREST).ToString(), GlobalVars.unitConverter.HeightUnit);
			WriteParam(Resources.str02024, GlobalVars.unitConverter.HeightToDisplayUnits(segment.MOCA, eRoundMode.NEAREST).ToString(), GlobalVars.unitConverter.HeightUnit);
			WriteString("");
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

		public void SaveObstacleTable(ObstacleContainer Obstacles, string TabComment = "")
		{
			if (TabComment != "")
			{
				WriteString(System.Net.WebUtility.HtmlEncode(TabComment));
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

			int m = Obstacles.Parts.Length;
			for (i = 0; i < m; i++)
			{
				string strArea;

				if (Obstacles.Parts[i].Prima)
					strArea = Resources.str02030;
				else
					strArea = Resources.str02031;

				_stwr.WriteLine("<tr>");
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
			}

			_stwr.WriteLine("</table>");
			_stwr.WriteLine("<br><br>");
		}

		public static void SaveGeometry(string RepFileName, string RepFileTitle, ReportHeader pReport, List<Segment> segments, double allRoutsLen)
		{
			CReportFile RoutsGeomRep = new CReportFile();

			RoutsGeomRep.OpenFile(RepFileName + "_Geometry", Resources.str00107);
			RoutsGeomRep.WriteString(System.Net.WebUtility.HtmlEncode(Resources.str00030 + " - " + Resources.str00107));

			RoutsGeomRep.WriteString("");

			RoutsGeomRep.WriteString(System.Net.WebUtility.HtmlEncode(RepFileTitle));
			RoutsGeomRep.WriteHeader(pReport);

			RoutsGeomRep.WriteString("");
			RoutsGeomRep.WriteString("");

			int n = segments.Count;
			for (int i = 0; i < n; i++)
				RoutsGeomRep.WriteSegment(segments[i]);
			
			RoutsGeomRep.WriteString("");

			RoutsGeomRep.WriteParam(Resources.str00108, GlobalVars.unitConverter.DistanceToDisplayUnits(allRoutsLen, eRoundMode.NEAREST).ToString(), GlobalVars.unitConverter.DistanceUnit);

			RoutsGeomRep.CloseFile();
			RoutsGeomRep = null;
		}
	}
}
