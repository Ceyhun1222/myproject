using Aran.Aim;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.PANDA.Common;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Aran.PANDA.RNAV.EnRoute
{
	public static class CommonFunctions
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

		[DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
		public static extern IntPtr GetWindow(IntPtr hWnd, int uCmd);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool InsertMenu(IntPtr hMenu, int uPosition, int uFlags, int uIDNewItem, string lpNewItem);

		public static DateTime RetrieveLinkerTimestamp()
		{
			const int c_PeHeaderOffset = 60;
			const int c_LinkerTimestampOffset = 8;

			byte[] b = new byte[2048];
			System.IO.Stream s = null;

			try
			{
				string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
				s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
				s.Read(b, 0, 2048);
			}
			finally
			{
				if (s != null)
					s.Close();
			}

			int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
			int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);

			DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

			dt = dt.AddSeconds(secondsSince1970);
			dt = dt.ToLocalTime();
			return dt;
		}

		public static string[] NavTypeNames = new string[] { "VOR", "DME", "NDB", "LOC", "TACAN", "Radar FIX" };

		public static string Tostring(this eNavaidType navType)
		{
			if (navType == eNavaidType.NONE)
				return "WPT";
			else
				return NavTypeNames[(int)navType];
		}

		public static void TextBoxFloat(ref char KeyChar, string BoxText)
		{
			if (KeyChar < 32)
				return;

			char DecSep = (1.1).ToString().ToCharArray()[1];

			if (((KeyChar < '0') || KeyChar > '9') && KeyChar != DecSep)
				KeyChar = '\0';
			else if (KeyChar == DecSep)
			{
				if (BoxText.Contains(DecSep.ToString()))
					KeyChar = '\0';
			}
		}

		public static bool TextBoxSignedFloat(ref char KeyChar, string BoxText)
		{
			if (KeyChar < 32)
				return false;

			char DecSep = (1.1).ToString().ToCharArray()[1];

			if (((KeyChar < '0') || KeyChar > '9') && KeyChar != DecSep && KeyChar != '-')
				KeyChar = '\0';
			else if (KeyChar == DecSep)
			{
				if (BoxText.Contains(DecSep.ToString()))
					KeyChar = '\0';
			}
			else if (KeyChar == '-')
			{
				KeyChar = '\0';
				return true;
			}

			return false;
		}

		public static void TextBoxInteger(ref char KeyChar)
		{
			if (KeyChar < ' ')
				return;

			if ((KeyChar < '0') || (KeyChar > '9'))
				KeyChar = '\0';
		}

		//public static double WindSpeed(double altitude )
		//{
		//	//  2 h + 47 kts	(h in 1 000 ft)
		//	return 0.514444444444444444444444 * (0.002 / 0.3048 * altitude + 47.0);

		//	// 12 h + 87 km/h	(h in 1 000 m)
		//	//return 0.277777777777777777777777 * (12.0 * 0.03048 * altitude + 87.0);
		//}

		public static bool ShowSaveDialog(out string FileName, ref string FileTitle)
		{
			string ProjectPath = GlobalVars.gAranEnv.DocumentFileName;
			string ProjectDir = Path.GetDirectoryName(ProjectPath);

			System.Windows.Forms.SaveFileDialog saveDialog = new System.Windows.Forms.SaveFileDialog();

			saveDialog.Filter = "PANDA Report File (*.htm)|*.htm|All Files (*.*)|*.*";
			//saveDialog.DefaultExt = ".htm";

			saveDialog.InitialDirectory = ProjectDir;
			saveDialog.FileName = FileTitle + ".htm";
			//saveDialog.Title = Properties.Resources.str00511;

			//saveDialog.AddExtension = true;
			saveDialog.OverwritePrompt = true;

			FileTitle = "";
			FileName = "";

			if (saveDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
			{
				saveDialog.Dispose();
				return false;
			}

			FileTitle = Path.GetFileNameWithoutExtension(saveDialog.FileName);
			FileName = Path.GetDirectoryName(Path.GetFullPath(saveDialog.FileName)) + "\\" + FileTitle;

			saveDialog.Dispose();

			return true;
		}

		internal static string Degree2String(double X, Degree2StringMode Mode)
		{
			string sSign = "", sResult = "", sTmp;
			double xDeg, xMin, xIMin, xSec;
			bool lSign = false;

			if (Mode == Degree2StringMode.DMSLat)
			{
				lSign = Math.Sign(X) < 0;
				if (lSign)
					X = -X;

				xDeg = System.Math.Floor(X);
				xMin = (X - xDeg) * 60.0;
				xIMin = System.Math.Floor(xMin);
				xSec = (xMin - xIMin) * 60.0;	//		xSec = System.Math.Round((xMin - xIMin) * 60.0, 2);
				if (xSec >= 60.0)
				{
					xSec = 0.0;
					xIMin++;
				}

				if (xIMin >= 60.0)
				{
					xIMin = 0.0;
					xDeg++;
				}

				sTmp = xDeg.ToString("00");
				sResult = sTmp + "°";

				sTmp = xIMin.ToString("00");
				sResult = sResult + sTmp + "'";

				sTmp = xSec.ToString("00.00");
				sResult = sResult + sTmp + @"""";

				return sResult + (lSign ? "S" : "N");
			}

			if (Mode >= Degree2StringMode.DMSLon)
			{
				X = NativeMethods.Modulus(X);
				lSign = X > 180.0;
				if (lSign) X = 360.0 - X;

				xDeg = System.Math.Floor(X);
				xMin = (X - xDeg) * 60.0;
				xIMin = System.Math.Floor(xMin);
				xSec = (xMin - xIMin) * 60.0;
				if (xSec >= 60.0)
				{
					xSec = 0.0;
					xIMin++;
				}

				if (xIMin >= 60.0)
				{
					xIMin = 0.0;
					xDeg++;
				}

				sTmp = xDeg.ToString("000");
				sResult = sTmp + "°";

				sTmp = xIMin.ToString("00");
				sResult = sResult + sTmp + "'";

				sTmp = xSec.ToString("00.00");
				sResult = sResult + sTmp + @"""";

				return sResult + (lSign ? "W" : "E");
			}

			if (System.Math.Sign(X) < 0) sSign = "-";
			X = NativeMethods.Modulus(System.Math.Abs(X));

			switch (Mode)
			{
				case Degree2StringMode.DD:
					return sSign + X.ToString("#0.00##") + "°";
				case Degree2StringMode.DM:
					if (System.Math.Sign(X) < 0) sSign = "-";
					X = NativeMethods.Modulus(System.Math.Abs(X));

					xDeg = System.Math.Floor(X);
					xMin = (X - xDeg) * 60.0;
					if (xMin >= 60)
					{
						X++;
						xMin = 0;
					}

					sResult = sSign + xDeg.ToString() + "°";

					sTmp = xMin.ToString("00.00##");
					return sResult + sTmp + "'";
				case Degree2StringMode.DMS:
					if (System.Math.Sign(X) < 0) sSign = "-";
					X = NativeMethods.Modulus(System.Math.Abs(X));

					xDeg = System.Math.Floor(X);
					xMin = (X - xDeg) * 60.0;
					xIMin = System.Math.Floor(xMin);
					xSec = (xMin - xIMin) * 60.0;
					if (xSec >= 60.0)
					{
						xSec = 0.0;
						xIMin++;
					}

					if (xIMin >= 60.0)
					{
						xIMin = 0.0;
						xDeg++;
					}

					sResult = sSign + xDeg.ToString() + "°";

					sTmp = xIMin.ToString("00");
					sResult = sResult + sTmp + "'";

					sTmp = xSec.ToString("00.00");
					return sResult + sTmp + @"""";
			}
			return sResult;
		}

		//static int ComparePartsByOwner(ObstacleData d0, ObstacleData d1)
		//{
		//	if (d0.Owner < d1.Owner) return -1;
		//	if (d0.Owner > d1.Owner) return 1;
		//	return 0;
		//}

		//static int ownerStarts(ref ObstacleData[] parts, int owner)
		//{
		//	int n = parts.Length;
		//	if (parts[0].Owner > owner)
		//		return -1;

		//	if (parts[n - 1].Owner < owner)
		//		return -1;

		//	int start = 0, end = n, curr = n >> 1;

		//	while (true)
		//	{
		//		//curr = (end + start) >> 1;

		//		if (parts[curr].Owner == owner)
		//		{
		//			while (curr > 0 && parts[curr - 1].Owner == owner)
		//				curr--;

		//			return curr;
		//		}

		//		if (parts[curr].Owner < owner)
		//			end = curr;
		//		else
		//			start = curr;

		//		curr = (end + start) >> 1;
		//	}

		//}

		//static public void mergeOstacleLists(LegBase forwardLeg, LegBase backwardLeg, out ObstacleContainer mergedObstacles)
		//{
		//	int l = forwardLeg.Obstacles.Obstacles.Length;
		//	int k = forwardLeg.Obstacles.Parts.Length;

		//	int n = l + forwardLeg.Obstacles.Obstacles.Length;
		//	int m = k + backwardLeg.Obstacles.Parts.Length;

		//	mergedObstacles.Obstacles = new Obstacle[n];
		//	mergedObstacles.Parts = new ObstacleData[m];

		//	Array.Copy(forwardLeg.Obstacles.Obstacles, mergedObstacles.Obstacles, l);
		//	Array.Copy(forwardLeg.Obstacles.Parts, mergedObstacles.Parts, k);

		//	Dictionary<Guid, int> map = new Dictionary<Guid, int>();

		//	for (int i = 0; i < l; i++)
		//		map[mergedObstacles.Obstacles[i].Identifier] = i;
		//	Array.Sort(mergedObstacles.Parts, ComparePartsByOwner);     //by owner

		//	for (int i = 0; i < backwardLeg.Obstacles.Parts.Length; i++)
		//	{
		//		int newOwner;
		//		int oldOwner = backwardLeg.Obstacles.Parts[i].Owner;
		//		Guid ownerGuid = backwardLeg.Obstacles.Obstacles[oldOwner].Identifier;

		//		if (map.TryGetValue(backwardLeg.Obstacles.Obstacles[oldOwner].Identifier, out newOwner))
		//		{
		//			bool addToArray = true;

		//			int j = ownerStarts(ref mergedObstacles.Parts, newOwner);

		//			while (mergedObstacles.Parts[j].Owner == newOwner)
		//			{
		//				double dx = mergedObstacles.Parts[j].pPtPrj.X - backwardLeg.Obstacles.Parts[i].pPtPrj.X;
		//				double dy = mergedObstacles.Parts[j].pPtPrj.Y - backwardLeg.Obstacles.Parts[i].pPtPrj.Y;

		//				if (dx * dx + dy * dy < ARANMath.Epsilon_2Distance)
		//				{
		//					mergedObstacles.Parts[j].MOC = Math.Max(mergedObstacles.Parts[j].MOC, backwardLeg.Obstacles.Parts[i].MOC);
		//					mergedObstacles.Parts[j].ReqH = Math.Max(mergedObstacles.Parts[j].ReqH, backwardLeg.Obstacles.Parts[i].ReqH);
		//					mergedObstacles.Parts[j].fSecCoeff = Math.Max(mergedObstacles.Parts[j].fSecCoeff, backwardLeg.Obstacles.Parts[i].fSecCoeff);
		//					mergedObstacles.Parts[j].Prima = mergedObstacles.Parts[j].Prima || backwardLeg.Obstacles.Parts[i].Prima;

		//					addToArray = false;
		//					break;
		//				}

		//				j++;
		//			}

		//			if (!addToArray)
		//				continue;
		//		}
		//		else
		//		{
		//			newOwner = l;
		//			l++;
		//			if (l >= mergedObstacles.Obstacles.Length)
		//				Array.Resize(ref mergedObstacles.Obstacles, l + l);

		//			map[backwardLeg.Obstacles.Obstacles[oldOwner].Identifier] = newOwner;
		//			mergedObstacles.Obstacles[newOwner] = backwardLeg.Obstacles.Obstacles[oldOwner];
		//		}

		//		mergedObstacles.Parts[k] = backwardLeg.Obstacles.Parts[i];
		//		mergedObstacles.Parts[k].Owner = newOwner;

		//		k++;
		//		if (k >= mergedObstacles.Parts.Length)
		//			Array.Resize<ObstacleData>(ref mergedObstacles.Parts, k + k);

		//		//Array.Resize(ref mergedObstacles.Obstacles[newOwner].Parts, mergedObstacles.Obstacles[newOwner].PartsNum + 1);
		//		//mergedObstacles.Obstacles[newOwner].Parts[mergedObstacles.Obstacles[newOwner].PartsNum] = k;
		//	}

		//	Array.Resize(ref mergedObstacles.Obstacles, l);
		//	Array.Resize(ref mergedObstacles.Parts, k);
		//}

		//public static void mergeOstacleLists(LegBase forwardLeg, LegBase backwardLeg, out ObstacleContainer mergedObstacles)
		//{
		//	int l = forwardLeg.Obstacles.Obstacles.Length;
		//	int k = forwardLeg.Obstacles.Parts.Length;

		//	int n = l + forwardLeg.Obstacles.Obstacles.Length;
		//	int m = k + backwardLeg.Obstacles.Parts.Length;

		//	mergedObstacles.Obstacles = new Obstacle[n];
		//	mergedObstacles.Parts = new ObstacleData[m];

		//	Array.Copy(forwardLeg.Obstacles.Obstacles, mergedObstacles.Obstacles, l);
		//	Array.Copy(forwardLeg.Obstacles.Parts, mergedObstacles.Parts, k);

		//	Dictionary<Guid, int> map = new Dictionary<Guid, int>();

		//	for (int i = 0; i < l; i++)
		//		map[forwardLeg.Obstacles.Obstacles[i].Identifier] = i;


		//	for (int i = 0; i < backwardLeg.Obstacles.Parts.Length; i++)
		//	{
		//		int owner = backwardLeg.Obstacles.Parts[i].Owner, newOwner;

		//		if (map.TryGetValue(backwardLeg.Obstacles.Obstacles[owner].Identifier, out newOwner))
		//		{
		//			bool addToArray = true;

		//			for (int j = 0; j < mergedObstacles.Obstacles[newOwner].PartsNum; j++)
		//			{
		//				double dx = mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].pPtPrj.X - backwardLeg.Obstacles.Parts[i].pPtPrj.X;
		//				double dy = mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].pPtPrj.Y - backwardLeg.Obstacles.Parts[i].pPtPrj.Y;

		//				if (dx * dx + dy * dy < ARANMath.Epsilon_2Distance)
		//				{
		//					mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].MOC = Math.Max(mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].MOC, backwardLeg.Obstacles.Parts[i].MOC);
		//					mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].ReqH = Math.Max(mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].ReqH, backwardLeg.Obstacles.Parts[i].ReqH);
		//					mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].fSecCoeff = Math.Max(mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].fSecCoeff, backwardLeg.Obstacles.Parts[i].fSecCoeff);
		//					mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].Prima = mergedObstacles.Parts[mergedObstacles.Obstacles[newOwner].Parts[j]].Prima || backwardLeg.Obstacles.Parts[i].Prima;

		//					addToArray = false;
		//					break;
		//				}
		//			}

		//			if (!addToArray)
		//				continue;
		//		}
		//		else
		//		{
		//			newOwner = l;
		//			l++;
		//			if (l >= mergedObstacles.Obstacles.Length)
		//				Array.Resize<Obstacle>(ref mergedObstacles.Obstacles, l + l);

		//			map[backwardLeg.Obstacles.Obstacles[owner].Identifier] = newOwner;
		//			mergedObstacles.Obstacles[newOwner] = backwardLeg.Obstacles.Obstacles[owner];
		//		}

		//		mergedObstacles.Parts[k] = backwardLeg.Obstacles.Parts[i];
		//		mergedObstacles.Parts[k].Owner = newOwner;

		//		Array.Resize<int>(ref mergedObstacles.Obstacles[newOwner].Parts, mergedObstacles.Obstacles[newOwner].PartsNum + 1);
		//		mergedObstacles.Obstacles[newOwner].Parts[mergedObstacles.Obstacles[newOwner].PartsNum] = k;

		//		k++;
		//		if (k >= mergedObstacles.Parts.Length)
		//			Array.Resize<ObstacleData>(ref mergedObstacles.Parts, k + k);
		//	}

		//	Array.Resize<Obstacle>(ref mergedObstacles.Obstacles, l);
		//	Array.Resize<ObstacleData>(ref mergedObstacles.Parts, k);
		//}

		public static bool PriorPostFixTolerance(MultiPolygon pPolygon, Point pPtPrj, double fDir, out double PriorDist, out double PostDist)
		{
			PriorDist = -1.0;
			PostDist = -1.0;
			MultiLineString ptIntersection;

			LineString pCutterPolyline = new LineString();
			pCutterPolyline.Add(ARANFunctions.LocalToPrj(pPtPrj, fDir, 1000000.0, 0));
			pCutterPolyline.Add(ARANFunctions.LocalToPrj(pPtPrj, fDir, -1000000.0, 0));

			try
			{
				GeometryOperators pTopological = new GeometryOperators();
				Geometry pIntersection = pTopological.Intersect(pCutterPolyline, pPolygon);
				if (pIntersection.IsEmpty)
					return false;
				ptIntersection = (MultiLineString)pIntersection;
			}
			catch
			{
				return false;
			}

			Point ptDist = ARANFunctions.PrjToLocal(pPtPrj, fDir, ptIntersection[0][0]);

			double fMinDist = ptDist.X;
			double fMaxDist = ptDist.X;
			int n = ptIntersection.Count;

			for (int j = 0; j < n; j++)
			{
				LineString ls = ptIntersection[j];
				int m = ls.Count;

				for (int i = 0; i < m; i++)
				{
					ptDist = ARANFunctions.PrjToLocal(pPtPrj, fDir, ls[i]);

					if (ptDist.X < fMinDist) fMinDist = ptDist.X;
					if (ptDist.X > fMaxDist) fMaxDist = ptDist.X;
				}
			}
			PriorDist = fMinDist;
			PostDist = fMaxDist;

			return true;
		}

		public static void shall_SortfSortD(ObstacleData[] obsArray)
		{
			int LastRow = obsArray.GetUpperBound(0);
			if (LastRow < 0)
				return;

			int FirstRow = obsArray.GetLowerBound(0);
			int NumRows = LastRow - FirstRow + 1;
			int GapSize = 0;

			do
				GapSize = GapSize * 3 + 1;
			while (GapSize <= NumRows);

			do
			{
				GapSize = GapSize / 3;
				for (int i = GapSize + FirstRow; i <= LastRow; i++)
				{
					int CurrPos = i;
					ObstacleData TempVal = obsArray[i];
					while (obsArray[CurrPos - GapSize].ReqH < TempVal.ReqH)
					{
						obsArray[CurrPos] = obsArray[CurrPos - GapSize];
						CurrPos = CurrPos - GapSize;
						if (CurrPos - GapSize < FirstRow)
							break;
					}
					obsArray[CurrPos] = TempVal;
				}
			}
			while (GapSize > 1);
		}

	    public static void GetVerticalHorizontalAccuracy(VerticalStructurePartGeometry horizontalProj, ref double verAccuracy, ref double horAccuracy,ref double elevation)
	    {
            if (horizontalProj==null) return;   

	        if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
	        {
	            if (horizontalProj.Location != null)
	            {
	                verAccuracy = ConverterToSI.Convert(horizontalProj.Location.VerticalAccuracy, verAccuracy);
	                horAccuracy = ConverterToSI.Convert(horizontalProj.Location.HorizontalAccuracy, horAccuracy);
	                elevation = ConverterToSI.Convert(horizontalProj.Location.Elevation, 0);
	            }
	        }
	        else if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedCurve)
	        {
	            if (horizontalProj.LinearExtent != null)
	            {
	                verAccuracy = ConverterToSI.Convert(horizontalProj.LinearExtent.VerticalAccuracy, verAccuracy);
	                horAccuracy = ConverterToSI.Convert(horizontalProj.LinearExtent.HorizontalAccuracy, horAccuracy);
	                elevation = ConverterToSI.Convert(horizontalProj.LinearExtent.Elevation, 0);
	            }
	        }
	        else
	        {
	            if (horizontalProj.SurfaceExtent != null)
	            {
	                verAccuracy = ConverterToSI.Convert(horizontalProj.SurfaceExtent.VerticalAccuracy, verAccuracy);
	                horAccuracy = ConverterToSI.Convert(horizontalProj.SurfaceExtent.HorizontalAccuracy, horAccuracy);
	                elevation = ConverterToSI.Convert(horizontalProj.SurfaceExtent.Elevation, 0);
	            }
	        }
	    }

	    public static Geometry GetPartGeometry(VerticalStructurePart part)
	    {
	        switch (part?.HorizontalProjection?.Choice)
	        {
	            case VerticalStructurePartGeometryChoice.ElevatedPoint:
	                return part.HorizontalProjection.Location?.Geo;
	            case VerticalStructurePartGeometryChoice.ElevatedCurve:
	                return part.HorizontalProjection.LinearExtent?.Geo;
	            case VerticalStructurePartGeometryChoice.ElevatedSurface:
	                return part.HorizontalProjection.SurfaceExtent?.Geo;
	            default:
	                return null;
	        }

	    }
	}
}
