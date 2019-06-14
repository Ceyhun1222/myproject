using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;

namespace AIXM45_AIM_UTIL
{
	public static class RowExtensions
	{
		public static object GetValue (this IRow row, string fieldName)
		{
			var index = row.Fields.FindField (fieldName);
			return row.get_Value (index);
		}

		public static string GetString (this IRow row, string fieldName)
		{
			var index = row.Fields.FindField (fieldName);
			return row.get_Value (index).ToString ();
		}
	}
}
