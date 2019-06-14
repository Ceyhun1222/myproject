using System;
using System.IO;
using EOSID.Properties;
using ESRI.ArcGIS.Geometry;

namespace EOSID
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public class CReportFile
	{
		private const string ReportFileExt = ".htm";
		public System.Windows.Forms.ListView lListView;
		public IPoint DerPtPrj;
		public IPoint ThrPtPrj;
		public double hRef;

		private string pFileName;
		private StreamWriter _stwr = null;

		public void OpenFile(string Name, string ReportTitle)
		{
			if (_stwr != null)
				return;

			pFileName = Name + ReportFileExt;

			_stwr = new StreamWriter(pFileName);// System.IO.File.OpenWrite(pFileName);

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
			//WriteString(Resources.str819 + GlobalVars.GetMapFileName());// Application.Templates.get_Item(GlobalVars.Application.Templates.Count - 1));
			//WriteString(Resources.str90);
			if (pReport.Database != null && pReport.Database != "")
				WriteString("<p><b>Database: " + pReport.Database + "</b>");

			if (pReport.Aerodrome != null && pReport.Aerodrome != "")
				WriteString("Aerodrome: " + pReport.Aerodrome);

			if (pReport.RWY != null && pReport.RWY != "")
				WriteString("RWY: " + pReport.RWY);	// + "<p>"

			if (pReport.Category != null && pReport.Category != "")
				WriteString("Category: " + pReport.Category + "</p>");

			if (pReport.Procedure != null && pReport.Procedure != "")
				WriteString("Procedure: " + pReport.Procedure + "</p>");

			if (GlobalVars.UserName != null && GlobalVars.UserName != "")
			WriteString("<p>User name: " + GlobalVars.UserName + "<br>");

			WriteString("<p><b>Date: "  + "</b>    " + DateTime.Now.Date.ToString());
			WriteString("<b>Time: "  + "</b>    " + DateTime.Now.TimeOfDay.ToString() + "</p>");

			WriteString("");
			WriteString("<p>Расстояния в " + UnitConverter.DistanceUnit + ".");
			WriteString("Превышения и высоты в " + UnitConverter.HeightUnit + ".</p>");
			WriteString("");
		}

		public void WriteString(string Message)
		{
			string temp = null;

			temp = "";
			if (Message != "")
				temp = "<b>" + Message + "</b>";

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

			temp = "<b>" + ParamName + ":</b> " + ParamValue;
			if (ParamUnit != "")
				temp = temp + " " + ParamUnit;

			_stwr.WriteLine(temp + " <br>");
		}

		public void WriteParam(string ParamName, string ParamValue)
		{
			WriteParam(ParamName, ParamValue, "");
		}

		//public void WriteLayerInfo(LayerInfo LI)
		//{
		//    _stwr.WriteLine(LI.LayerName + " source: " + LI.Source + "<br>");
		//    _stwr.WriteLine(LI.LayerName + " date: " + LI.FileDate.ToString() + "<br>");
		//    _stwr.WriteLine("<br>");
		//}

		public void WriteTab(string TabComment)
		{
			System.Windows.Forms.ListViewItem itmX;

			if (lListView == null || lListView.Items.Count == 0)
				return;

			if (TabComment != "")
			{
				WriteString(TabComment);
				WriteString("");
			}

			_stwr.WriteLine("<table border='1' cellspacing='0' cellpadding='1'>");

			int i, j;
			int n = lListView.Columns.Count;
			int m = lListView.Items.Count;

			_stwr.WriteLine("<tr>");
			for (i = 0; i < n; i++)
				_stwr.WriteLine("<td><b>" + lListView.Columns[i].Text + "</b></td>");

			_stwr.WriteLine("</tr>");

			for (i = 0; i < m; i++)
			{
				itmX = lListView.Items[i];

				_stwr.WriteLine("<tr>");
				_stwr.WriteLine("<td>" + itmX.Text + "</td>");

				for (j = 1; j < n; j++)
					_stwr.WriteLine("<td>" + itmX.SubItems[j].Text + "</td>");

				_stwr.WriteLine("</tr>");
			}

			_stwr.WriteLine("</table>");
			_stwr.WriteLine("<br><br>");
		}

		public void WriteTab()
		{
			WriteTab("");
		}

		public void WriteImage(string ImageName, string ImageComment)
		{
			if (ImageName == "")
				return;

			if (ImageComment != "")
			{
				WriteString(ImageComment);
				WriteString("");
			}

			_stwr.WriteLine("<img src='" + ImageName + "' border='0'>");
		}

		public void WriteImage(string ImageName)
		{
			WriteImage(ImageName, "");
		}

		public void WriteImageLink(string ImageName, string ImageComment)
		{
			if (ImageName == "")
				return;

			_stwr.WriteLine("<a href='" + ImageName + "'>" + ImageComment + "</a>");
		}

		public void WriteImageLink(string ImageName)
		{
			WriteImageLink(ImageName, "");
		}

		public void WritePoint(ReportPoint RepPoint)
		{
			if (RepPoint.Description == "")
				return;

			WriteString(RepPoint.Description);

			WriteParam("Широта", Functions.Degree2String(RepPoint.Lat, Degree2StringMode.DMSLat), "");
			WriteParam("Долгота", Functions.Degree2String(RepPoint.Lon, Degree2StringMode.DMSLon), "");

			WriteParam("Широта центра окружности", Functions.Degree2String(RepPoint.CenterLat, Degree2StringMode.DMSLat), "");
			WriteParam("Долгота центра окружности", Functions.Degree2String(RepPoint.CenterLon, Degree2StringMode.DMSLon), "");

			WriteParam("Абс. высота", UnitConverter.HeightToDisplayUnits(RepPoint.Height + ThrPtPrj.Z, eRoundMode.NERAEST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Unit);
			WriteParam("Отн. высота", UnitConverter.HeightToDisplayUnits(RepPoint.Height, eRoundMode.NERAEST).ToString(), GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Unit);
			WriteParam("Истинный курс", Math.Round(RepPoint.Direction, 2).ToString(), "°");

			if (RepPoint.Raidus > 0)
			{
				WriteParam("Радиус разворота", UnitConverter.DistanceToDisplayUnits(RepPoint.Raidus, eRoundMode.NERAEST).ToString(), UnitConverter.DistanceUnit);
				if (RepPoint.Turn > 0)
					WriteParam("Направление разворота", "правый", "");
				else
					WriteParam("Направление разворота", "левый", "");
				WriteParam("Угол разворота", Math.Round(RepPoint.turnAngle, 2).ToString(), "°");
				WriteParam("Длина дуги разворота", UnitConverter.DistanceToDisplayUnits(RepPoint.TurnArcLen, eRoundMode.NERAEST).ToString(), UnitConverter.DistanceUnit);
			}

			WriteParam("Расстояние до следующей точки", UnitConverter.DistanceToDisplayUnits(RepPoint.DistToNext, eRoundMode.NERAEST).ToString(), UnitConverter.DistanceUnit);
			WriteString("");
		}
		
		public void WriteTraceSegment(TrackLeg Segment, bool IsLastSegment)
		{
			WriteString(Segment.Comment);
			IPoint ptTmp = (IPoint)Functions.ToGeo(Segment.ptStart.TraceCase.pPoint);
			string StCoords = Functions.Degree2String(ptTmp.Y, Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(ptTmp.X, Degree2StringMode.DMSLon);
			string St1Coords, St2Coords, Center1Coords, Center2Coords;

			double dirbet;

			switch (Segment.SegmentCode)
			{
				case eLegType.straight:
					WriteString("[Начальная точка прямого сегмента]");

					WriteParam("Координаты", StCoords, "");
					WriteParam("Абс. высота", UnitConverter.HeightToDisplayUnits(Segment.ptStart.TraceCase.NetHeight + ThrPtPrj.Z, eRoundMode.NERAEST).ToString(), UnitConverter.HeightUnit);
					WriteParam("Отн. высота", UnitConverter.HeightToDisplayUnits(Segment.ptStart.TraceCase.NetHeight, eRoundMode.NERAEST).ToString(), UnitConverter.HeightUnit);

					WriteParam("Истинный курс", NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(DerPtPrj, Segment.ptStart.TraceCase.Direction), 2)).ToString(), "°");
					WriteParam("Расстояние до следующей точки", UnitConverter.DistanceToDisplayUnits(Segment.TraceCase.Length, eRoundMode.NERAEST).ToString(), UnitConverter.DistanceUnit);

					WriteString("");
					if (IsLastSegment)
						WriteString("[Конечная точка прямого сегмента] ");
					break;
				case eLegType.toHeading:
					WriteString("[Точка входа в разворот]");

					WriteParam("Координаты", StCoords, "");
					WriteParam("Абс. высота", UnitConverter.HeightToDisplayUnits(Segment.ptStart.TraceCase.NetHeight + ThrPtPrj.Z, eRoundMode.NERAEST).ToString(), UnitConverter.HeightUnit);
					WriteParam("Отн. высота", UnitConverter.HeightToDisplayUnits(Segment.ptStart.TraceCase.NetHeight, eRoundMode.NERAEST).ToString(), UnitConverter.HeightUnit);

					WriteParam("Истинный курс", NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(DerPtPrj, Segment.ptStart.TraceCase.Direction), 2)).ToString(), "°");
					WriteParam("Расстояние до следующей точки", UnitConverter.DistanceToDisplayUnits(Segment.TraceCase.Length, eRoundMode.NERAEST).ToString(), UnitConverter.DistanceUnit);
					WriteString("");

					if (Segment.TraceCase.turns > 0)
					{
						ptTmp = (IPoint)Functions.ToGeo(Segment.TraceCase.Turn[0].ptCenter);

						Center1Coords = Functions.Degree2String(ptTmp.Y, Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(ptTmp.X, Degree2StringMode.DMSLon);
						WriteParam("Центр разворота", Center1Coords, "");

						WriteParam("Радиус разворота", UnitConverter.DistanceToDisplayUnits(Segment.TraceCase.Turn[0].Radius, eRoundMode.NERAEST).ToString(), UnitConverter.DistanceUnit);

						if (Segment.TraceCase.Turn[0].TurnDir > 0)
							WriteParam("Направление разворота", "правый", "");
						else
							WriteParam("Направление разворота", "левый", "");

						WriteParam("Угол разворота", System.Math.Round(Segment.TraceCase.Turn[0].Angle, 2).ToString(), "°");
					}

					WriteString("");
					if (IsLastSegment)
						WriteString("[Точка выхода из разворота]");
					break;
				case eLegType.courseIntercept:
					WriteString("[Начальная точка прямого сегмента]");
					WriteParam("Координаты", StCoords, "");

					WriteParam("Абс. высота", UnitConverter.HeightToDisplayUnits(Segment.ptStart.TraceCase.NetHeight + ThrPtPrj.Z, eRoundMode.NERAEST).ToString(), UnitConverter.HeightUnit);
					WriteParam("Отн. высота", UnitConverter.HeightToDisplayUnits(Segment.ptStart.TraceCase.NetHeight, eRoundMode.NERAEST).ToString(), UnitConverter.HeightUnit);

					WriteParam("Истинный курс", NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(DerPtPrj, Segment.ptStart.TraceCase.Direction), 2)).ToString(), "°");
					WriteParam("Расстояние до следующей точки", UnitConverter.DistanceToDisplayUnits(Segment.TraceCase.Turn[0].StartDist, eRoundMode.NERAEST).ToString(), UnitConverter.DistanceUnit);
					WriteString("");

					if (Segment.TraceCase.turns > 0)
					{
						ptTmp = (IPoint)Functions.ToGeo(Segment.TraceCase.Turn[0].ptStart);
						St1Coords = Functions.Degree2String(ptTmp.Y, Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(ptTmp.X, Degree2StringMode.DMSLon);
						WriteString("[Точка входа в разворот]");
						WriteParam("Координаты", St1Coords, "");

						//WriteParam("Абс. высота", UnitConverter.HeightToDisplayUnits(Segment.H1 + ThrPtPrj.Z, eRoundMode.NERAEST).ToString(), UnitConverter.HeightUnit);
						//WriteParam("Отн. высота", UnitConverter.HeightToDisplayUnits(Segment.H1, eRoundMode.NERAEST).ToString(), UnitConverter.HeightUnit);

						WriteParam("Истинный курс", NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(DerPtPrj, Segment.ptStart.TraceCase.Direction), 2)).ToString(), "°");

						ptTmp = (IPoint)Functions.ToGeo(Segment.TraceCase.Turn[0].ptCenter);
						Center1Coords = Functions.Degree2String(ptTmp.Y, Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(ptTmp.X, Degree2StringMode.DMSLon);
						WriteParam("Центр разворота", Center1Coords, "");

						WriteParam("Радиус разворота", UnitConverter.DistanceToDisplayUnits(Segment.TraceCase.Turn[0].Radius, eRoundMode.NERAEST).ToString(), UnitConverter.DistanceUnit);
						if (Segment.TraceCase.Turn[0].TurnDir > 0)
							WriteParam("Направление разворота", "правый", "");
						else
							WriteParam("Направление разворота", "левый", "");

						WriteParam("Угол разворота", System.Math.Round(Segment.TraceCase.Turn[0].Angle, 2).ToString(), "°");

						WriteParam("Расстояние до следующей точки", UnitConverter.DistanceToDisplayUnits(Segment.TraceCase.Turn[0].Length, eRoundMode.NERAEST).ToString(), UnitConverter.DistanceUnit);
						WriteString("");
					}

					WriteString("");
					if (IsLastSegment)
						WriteString("[Точка выхода из разворота]");
					break;
				case eLegType.directToFIX:

					if (Segment.TraceCase.turns > 0)
					{
						WriteString("[Точка входа в разворот]");
						//ptTmp = (IPoint)Functions.ToGeo(Segment.TraceCase.Turn[0].ptStart);
						//string St1Coords = Functions.Degree2String(ptTmp.Y, Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(ptTmp.X, Degree2StringMode.DMSLon);
						WriteParam("Координаты", StCoords, "");//St1Coords

						WriteParam("Абс. высота", UnitConverter.HeightToDisplayUnits(Segment.ptStart.TraceCase.NetHeight + ThrPtPrj.Z, eRoundMode.NERAEST).ToString(), UnitConverter.HeightUnit);
						WriteParam("Отн. высота", UnitConverter.HeightToDisplayUnits(Segment.ptStart.TraceCase.NetHeight, eRoundMode.NERAEST).ToString(), UnitConverter.HeightUnit);

						WriteParam("Истинный курс", NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(DerPtPrj, Segment.ptStart.TraceCase.Direction), 2)).ToString(), "°");

						ptTmp = (IPoint)Functions.ToGeo(Segment.TraceCase.Turn[0].ptCenter);
						Center1Coords = Functions.Degree2String(ptTmp.Y, Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(ptTmp.X, Degree2StringMode.DMSLon);
						WriteParam("Центр разворота", Center1Coords, "");

						WriteParam("Радиус разворота", UnitConverter.DistanceToDisplayUnits(Segment.TraceCase.Turn[0].Radius, eRoundMode.NERAEST).ToString(), UnitConverter.DistanceUnit);
						if (Segment.TraceCase.Turn[0].TurnDir > 0)
							WriteParam("Направление разворота", "правый", "");
						else
							WriteParam("Направление разворота", "левый", "");

						WriteParam("Угол разворота", System.Math.Round(Segment.TraceCase.Turn[0].Angle, 2).ToString(), "°");

						WriteParam("Расстояние до следующей точки", UnitConverter.DistanceToDisplayUnits(Segment.TraceCase.Turn[0].Length, eRoundMode.NERAEST).ToString(), UnitConverter.DistanceUnit);
						WriteString("");
					}

					WriteString("[Начальная точка прямого сегмента]");
					ptTmp = (IPoint)Functions.ToGeo(Segment.TraceCase.Turn[0].ptEnd);
					St2Coords = Functions.Degree2String(ptTmp.Y, Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(ptTmp.X, Degree2StringMode.DMSLon);
					WriteParam("Координаты", St2Coords, "");

					//WriteParam("Абс. высота", UnitConverter.HeightToDisplayUnits(Segment.ptStart.TraceCase.NetHeight + ThrPtPrj.Z, eRoundMode.NERAEST).ToString(), UnitConverter.HeightUnit);
					//WriteParam("Отн. высота", UnitConverter.HeightToDisplayUnits(Segment.ptStart.TraceCase.NetHeight, eRoundMode.NERAEST).ToString(), UnitConverter.HeightUnit);
					dirbet = Segment.ptStart.TraceCase.Direction + Segment.TraceCase.Turn[0].TurnDir * Segment.TraceCase.Turn[0].Angle;

					WriteParam("Истинный курс", NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(DerPtPrj, dirbet), 2)).ToString(), "°");
					WriteParam("Расстояние до следующей точки", UnitConverter.DistanceToDisplayUnits(Segment.TraceCase.Length- Segment.TraceCase.Turn[0].Length, eRoundMode.NERAEST).ToString(), UnitConverter.DistanceUnit);
					WriteString("");

					WriteString("");
					if (IsLastSegment)
						WriteString("[Конечная точка прямого сегмента]");
					break;
				case eLegType.turnAndIntercept:
					WriteString("[Точка входа в 1-ый разворот]");
					WriteParam("Координаты", StCoords, "");

					WriteParam("Абс. высота", UnitConverter.HeightToDisplayUnits(Segment.ptStart.TraceCase.NetHeight + ThrPtPrj.Z, eRoundMode.NERAEST).ToString(), UnitConverter.HeightUnit);
					WriteParam("Отн. высота", UnitConverter.HeightToDisplayUnits(Segment.ptStart.TraceCase.NetHeight, eRoundMode.NERAEST).ToString(), UnitConverter.HeightUnit);

					WriteParam("Истинный курс", NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(DerPtPrj, Segment.ptStart.TraceCase.Direction), 2)).ToString(), "°");
					WriteParam("Расстояние до следующей точки", UnitConverter.DistanceToDisplayUnits(Segment.TraceCase.Turn[0].Length, eRoundMode.NERAEST).ToString(), UnitConverter.DistanceUnit);

					WriteString("");

					if (Segment.TraceCase.turns > 0)
					{
						ptTmp = (IPoint)Functions.ToGeo(Segment.TraceCase.Turn[0].ptCenter);
						Center1Coords = Functions.Degree2String(ptTmp.Y, Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(ptTmp.X, Degree2StringMode.DMSLon);
						WriteParam("Центр 1-го разворота", Center1Coords, "");
						WriteParam("Радиус 1-го разворота", UnitConverter.DistanceToDisplayUnits(Segment.TraceCase.Turn[0].Radius, eRoundMode.NERAEST).ToString(), UnitConverter.DistanceUnit);

						if (Segment.TraceCase.Turn[0].TurnDir > 0)
							WriteParam("Направление 1-го разворота", "правый", "");
						else
							WriteParam("Направление 1-го разворота", "левый", "");

						WriteParam("Угол 1-го разворота", System.Math.Round(Segment.TraceCase.Turn[0].Angle, 2).ToString(), "°");
						WriteString("");

						ptTmp = (IPoint)Functions.ToGeo(Segment.TraceCase.Turn[0].ptEnd);
						string Fin1Coords = Functions.Degree2String(ptTmp.Y, Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(ptTmp.X, Degree2StringMode.DMSLon);
						WriteString("[Точка выхода из 1-го разворота]");
						WriteParam("Координаты", Fin1Coords, "");

					}
					WriteString("");

					dirbet = Segment.ptStart.TraceCase.Direction + Segment.TraceCase.Turn[0].TurnDir * Segment.TraceCase.Turn[0].Angle;
					//Segment.TraceCase.Turn[0].
					//WriteParam("Абс. высота", UnitConverter.HeightToDisplayUnits(Segment.H1 + ThrPtPrj.Z, eRoundMode.NERAEST).ToString(), UnitConverter.HeightUnit);
					//WriteParam("Отн. высота", UnitConverter.HeightToDisplayUnits(Segment.H1, eRoundMode.NERAEST).ToString(), UnitConverter.HeightUnit);

					WriteParam("Истинный курс", NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(DerPtPrj, dirbet), 2)).ToString(), "°");
					if (Segment.TraceCase.turns > 1)
					{
						WriteParam("Расстояние до следующей точки", UnitConverter.DistanceToDisplayUnits(Segment.TraceCase.Turn[1].StartDist, eRoundMode.NERAEST).ToString(), UnitConverter.DistanceUnit);

						WriteString("");


						WriteString("[Точка входа во 2-ой разворот]");

						ptTmp = (IPoint)Functions.ToGeo(Segment.TraceCase.Turn[1].ptStart);
						St2Coords = Functions.Degree2String(ptTmp.Y, Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(ptTmp.X, Degree2StringMode.DMSLon);
						WriteParam("Координаты", St2Coords, "");

						//WriteParam("Абс. высота", UnitConverter.HeightToDisplayUnits(Segment.H2 + ThrPtPrj.Z, eRoundMode.NERAEST).ToString(), UnitConverter.HeightUnit);
						//WriteParam("Отн. высота", UnitConverter.HeightToDisplayUnits(Segment.H2, eRoundMode.NERAEST).ToString(), UnitConverter.HeightUnit);

						WriteParam("Истинный курс", NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(DerPtPrj, dirbet), 2)).ToString(), "°");
						WriteParam("Расстояние до следующей точки", UnitConverter.DistanceToDisplayUnits(Segment.TraceCase.Turn[1].Length, eRoundMode.NERAEST).ToString(), UnitConverter.DistanceUnit);

						ptTmp = (IPoint)Functions.ToGeo(Segment.TraceCase.Turn[1].ptCenter);
						Center2Coords = Functions.Degree2String(ptTmp.Y, Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(ptTmp.X, Degree2StringMode.DMSLon);
						WriteString("");
						WriteParam("Центр 2-го разворота", Center2Coords, "");
						WriteParam("Радиус 2-го разворота", UnitConverter.DistanceToDisplayUnits(Segment.TraceCase.Turn[1].Radius, eRoundMode.NERAEST).ToString(), UnitConverter.DistanceUnit);

						if (Segment.TraceCase.Turn[1].TurnDir > 0)
							WriteParam("Направление 2-го разворота", "правый", "");
						else
							WriteParam("Направление 2-го разворота", "левый", "");

						WriteParam("Угол 2-го разворота", System.Math.Round(Segment.TraceCase.Turn[1].Angle, 2).ToString(), "°");
					}

					WriteString("");
					if (IsLastSegment)
						WriteString("[Точка выхода из 2-го разворота]");
					break;
				case eLegType.arcIntercept:
					WriteString("[Начальная точка перехвата дуги]");

					WriteParam("Координаты", StCoords, "");
					WriteParam("Абс. высота", UnitConverter.HeightToDisplayUnits(Segment.ptStart.TraceCase.NetHeight + ThrPtPrj.Z, eRoundMode.NERAEST).ToString(), UnitConverter.HeightUnit);
					WriteParam("Отн. высота", UnitConverter.HeightToDisplayUnits(Segment.ptStart.TraceCase.NetHeight, eRoundMode.NERAEST).ToString(), UnitConverter.HeightUnit);

					WriteParam("Истинный курс", NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(DerPtPrj, Segment.ptStart.TraceCase.Direction), 2)).ToString(), "°");
					WriteParam("Расстояние до следующей точки", UnitConverter.DistanceToDisplayUnits(Segment.TraceCase.Length, eRoundMode.NERAEST).ToString(), UnitConverter.DistanceUnit);

					WriteString("");
					if (IsLastSegment)
						WriteString("[Конечная точка перехвата дуги] ");
					break;
				case eLegType.arcPath:
					WriteString("[Начальная точка полета по дуге]");

					WriteParam("Координаты", StCoords, "");
					WriteParam("Абс. высота", UnitConverter.HeightToDisplayUnits(Segment.ptStart.TraceCase.NetHeight + ThrPtPrj.Z, eRoundMode.NERAEST).ToString(), UnitConverter.HeightUnit);
					WriteParam("Отн. высота", UnitConverter.HeightToDisplayUnits(Segment.ptStart.TraceCase.NetHeight, eRoundMode.NERAEST).ToString(), UnitConverter.HeightUnit);

					WriteParam("Истинный курс", NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(DerPtPrj, Segment.ptStart.TraceCase.Direction), 2)).ToString(), "°");
					WriteParam("Расстояние до следующей точки", UnitConverter.DistanceToDisplayUnits(Segment.TraceCase.Length, eRoundMode.NERAEST).ToString(), UnitConverter.DistanceUnit);

					WriteString("");
					if (IsLastSegment)
						WriteString("[Конечная точка полета по дуге] ");
					break;
			}

			if (IsLastSegment)
			{
				ptTmp = (IPoint)Functions.ToGeo(Segment.ptEnd.TraceCase.pPoint);
				string FinCoords = Functions.Degree2String(ptTmp.Y, Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(ptTmp.X, Degree2StringMode.DMSLon);
				WriteParam("Координаты", FinCoords, "");

				WriteParam("Абс. высота", UnitConverter.HeightToDisplayUnits(Segment.ptEnd.TraceCase.NetHeight + ThrPtPrj.Z, eRoundMode.NERAEST).ToString(), UnitConverter.HeightUnit);
				WriteParam("Отн. высота", UnitConverter.HeightToDisplayUnits(Segment.ptEnd.TraceCase.NetHeight, eRoundMode.NERAEST).ToString(), UnitConverter.HeightUnit);

				WriteParam("Истинный курс", NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(DerPtPrj, Segment.ptEnd.TraceCase.Direction ), 2)).ToString(), "°");
				WriteString("");
			}
		}

		public static void SaveGeometry(string RepFileName, string RepFileTitle, ReportHeader pReport, RWYData RWY, TrackLeg[] LegList, int LegCount, bool worstcase)
		{
			CReportFile reportFile = new CReportFile();
			reportFile.DerPtPrj = RWY.pPtPrj[eRWY.PtEnd];
			reportFile.ThrPtPrj = RWY.pPtPrj[eRWY.PtEnd];

			reportFile.OpenFile(RepFileName + "_Geometry", "Геометрия схемы");

			reportFile.WriteString("EOSID" + " - " + "Геометрия схемы");
			reportFile.WriteString("");
			reportFile.WriteString(RepFileTitle);

			reportFile.WriteHeader(pReport);

			reportFile.WriteString("");
			reportFile.WriteString("");

			double TraceBestLen = 0, TraceWorstLen = 0;

			for (int i = 0; i < LegCount; i++)
			{	//░░░░░░░░░░░░░░░░░░░░░░░░░░░░░
				//        ░░░░ ▒▒▒▒ ▓▓▓▓ ██████ ■ █ ■ █ ■ █ ■

				reportFile.WriteString("Сегмент номер № " + (i + 1).ToString());
				if (worstcase)
				{
					LegList[i].TraceCase = LegList[i].WorstCase;
					LegList[i].ptStart.TraceCase = LegList[i].ptStart.WorstCase;
					LegList[i].ptEnd.TraceCase = LegList[i].ptEnd.WorstCase;
				}
				else
				{
					LegList[i].TraceCase = LegList[i].BestCase;
					LegList[i].ptStart.TraceCase = LegList[i].ptStart.BestCase;
					LegList[i].ptEnd.TraceCase = LegList[i].ptEnd.BestCase;
				}

				reportFile.WriteTraceSegment(LegList[i], i == LegCount - 1);
				TraceBestLen += LegList[i].BestCase.Length;
				TraceWorstLen += LegList[i].WorstCase.Length;
				reportFile.WriteString("");
			}

			if (worstcase)
				reportFile.WriteParam("Общая протяженность схемы: ", UnitConverter.DistanceToDisplayUnits(TraceWorstLen, eRoundMode.NERAEST).ToString(), UnitConverter.DistanceUnit);
			else
				reportFile.WriteParam("Общая протяженность схемы: ", UnitConverter.DistanceToDisplayUnits(TraceBestLen, eRoundMode.NERAEST).ToString(), UnitConverter.DistanceUnit);

			reportFile.CloseFile();
			reportFile = null;
		}
	}
}
//▒▒▒▒
//▒▒▒▒