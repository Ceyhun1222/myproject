using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.PANDA.Common;

namespace Aran.PANDA.RNAV.SGBAS
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class NavaidsDatabase
	{
		public static int FindNavaid(string NavCallSign, eNavaidType NavType, out NavaidType Navaid)
		{
			if (NavType == eNavaidType.DME)
			{
				int n = GlobalVars.DMEList.Length;
				for (int i = 0; i < n; i++)
					if (GlobalVars.DMEList[i].CallSign == NavCallSign)
					{
						Navaid = GlobalVars.DMEList[i];
						return 0;
					}
			}
			else
			{
				int n = GlobalVars.NavaidList.Length;
				for (int i = 0; i < n; i++)
					if (GlobalVars.NavaidList[i].TypeCode == NavType && GlobalVars.NavaidList[i].CallSign == NavCallSign)
					{
						Navaid = GlobalVars.NavaidList[i];
						return 0;
					}
			}

			Navaid = default(NavaidType);
			Navaid.TypeCode = eNavaidType.NONE;
			Navaid.CallSign = "";
			return -1;
		}
	}
}
