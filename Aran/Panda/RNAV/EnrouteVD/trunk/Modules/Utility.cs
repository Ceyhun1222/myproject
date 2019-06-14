using Aran.Aim.Enums;
using Aran.Geometries;
using Aran.PANDA.Common;
using System;
using System.Runtime.InteropServices;

namespace Aran.PANDA.RNAV.Enroute.VD
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class Utility
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

		public static bool ShowSaveDialog(out string FileName, out string FileTitle)
		{
			System.Windows.Forms.SaveFileDialog saveDialog = new System.Windows.Forms.SaveFileDialog();

			saveDialog.FileName = "";
			saveDialog.Filter = "PANDA Report File (*.htm)|*.htm|All Files (*.*)|*.*";

			FileTitle = "";
			FileName = "";

			if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				FileName = saveDialog.FileName;

				int pos = FileName.LastIndexOf('.');
				if (pos > 0)
					FileName = FileName.Substring(0, pos);

				FileTitle = FileName;
				int pos2 = FileTitle.LastIndexOf('\\');
				if (pos2 > 0)
					FileTitle = FileTitle.Substring(pos2 + 1);

				return true;
			}

			return false;
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

		//public static void CreateLegGeometry(CodeDirection dir, ref Segment seg)
		//{
		//	FIX startFIX = seg.Start;
		//	FIX endFIX = seg.End;

		//	seg.PrimaryProtectionArea = new Polygon();
		//	seg.SecondaryProtectionArea = new Polygon();
		//	double dirAngle = ARANFunctions.ReturnAngleInRadians(startFIX.PrjPt, endFIX.PrjPt);
		//	double distance = ARANFunctions.ReturnDistanceInMeters(startFIX.PrjPt, endFIX.PrjPt);

		//	Ring ring = new Ring();
		//	ring.Add(ARANFunctions.LocalToPrj(startFIX.PrjPt, dirAngle, -startFIX.ATT, startFIX.ASW_2_L));
		//	ring.Add(ARANFunctions.LocalToPrj(startFIX.PrjPt, dirAngle, 0, startFIX.ASW_2_L));

		//	ring.Add(ARANFunctions.LocalToPrj(endFIX.PrjPt, dirAngle, 0, endFIX.ASW_2_L));
		//	ring.Add(ARANFunctions.LocalToPrj(endFIX.PrjPt, dirAngle, endFIX.ATT, endFIX.ASW_2_L));

		//	ring.Add(ARANFunctions.LocalToPrj(endFIX.PrjPt, dirAngle, endFIX.ATT, -endFIX.ASW_2_L));
		//	ring.Add(ARANFunctions.LocalToPrj(endFIX.PrjPt, dirAngle, 0, -endFIX.ASW_2_L));

		//	ring.Add(ARANFunctions.LocalToPrj(startFIX.PrjPt, dirAngle, 0, -startFIX.ASW_2_L));
		//	ring.Add(ARANFunctions.LocalToPrj(startFIX.PrjPt, dirAngle, -startFIX.ATT, -startFIX.ASW_2_L));
		//	seg.PrimaryProtectionArea.ExteriorRing = ring;


		//	ring = new Ring();
		//	ring.Add(ARANFunctions.LocalToPrj(startFIX.PrjPt, dirAngle, -startFIX.ATT, startFIX.ASW_L));
		//	ring.Add(ARANFunctions.LocalToPrj(startFIX.PrjPt, dirAngle, 0, startFIX.ASW_L));

		//	ring.Add(ARANFunctions.LocalToPrj(endFIX.PrjPt, dirAngle, 0, endFIX.ASW_L));
		//	ring.Add(ARANFunctions.LocalToPrj(endFIX.PrjPt, dirAngle, endFIX.ATT, endFIX.ASW_L));

		//	ring.Add(ARANFunctions.LocalToPrj(endFIX.PrjPt, dirAngle, endFIX.ATT, -endFIX.ASW_L));
		//	ring.Add(ARANFunctions.LocalToPrj(endFIX.PrjPt, dirAngle, 0, -endFIX.ASW_L));

		//	ring.Add(ARANFunctions.LocalToPrj(startFIX.PrjPt, dirAngle, 0, -startFIX.ASW_L));
		//	ring.Add(ARANFunctions.LocalToPrj(startFIX.PrjPt, dirAngle, -startFIX.ATT, -startFIX.ASW_L));
		//	seg.SecondaryProtectionArea.ExteriorRing = ring;
		//}
	}
}
