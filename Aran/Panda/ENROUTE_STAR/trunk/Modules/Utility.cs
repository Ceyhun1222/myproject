using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aran.Panda.Common;

namespace Aran.Panda.EnrouteStar
{
	[System.Runtime.InteropServices.ComVisible(false)]

	public static class Utility
	{
		public static List<CBWPT_FixItem> FillDirectionWPTList(FIX prevFix, double axis, double maxTurn, double MinDist, double MaxnDist)
		{
			List<CBWPT_FixItem> result = new List<CBWPT_FixItem>();
			int n = GlobalVars.WPTList.Length;

			for (int i = 0; i < n; i++)
			{
				if (GlobalVars.WPTList[i].CallSign != null)
				{
					double dist = ARANFunctions.ReturnDistanceInMeters(prevFix.PrjPt, GlobalVars.WPTList[i].pPtPrj);
					if (dist > MinDist && dist < MaxnDist)
					{
						if (maxTurn >= ARANMath.C_PI)
							result.Add(new CBWPT_FixItem(GlobalVars.WPTList[i]));
						else
						{
							double dir = ARANFunctions.ReturnDistanceInMeters(prevFix.PrjPt, GlobalVars.WPTList[i].pPtPrj);
							double diff = ARANMath.SubtractAngles(dir, axis);
							if (diff < maxTurn)
								result.Add(new CBWPT_FixItem(GlobalVars.WPTList[i]));
						}
					}
				}
			}
			return result;
		}

		public static List<CBWPT_FixItem> FillPositionWPTList(FIX prevFix, double axis, double MinDist, double MaxnDist)
		{
			const double dirTreshold = ARANMath.DegToRadValue; // *1.5

			List<CBWPT_FixItem> result = new List<CBWPT_FixItem>();
			int n = GlobalVars.WPTList.Length;

			for (int i = 0; i < n; i++)
			{
				if (GlobalVars.WPTList[i].CallSign != null)
				{
					double dist = ARANFunctions.ReturnDistanceInMeters(prevFix.PrjPt, GlobalVars.WPTList[i].pPtPrj);
					if (dist > MinDist && dist < MaxnDist)
					{
						double dir = ARANFunctions.ReturnAngleInRadians(prevFix.PrjPt, GlobalVars.WPTList[i].pPtPrj);
						double diff = ARANMath.SubtractAngles(dir, axis);
						if (diff < dirTreshold)
						{
							result.Add(new CBWPT_FixItem(GlobalVars.WPTList[i]));
						}
					}
				}
			}
			return result;
		}
	}
}
