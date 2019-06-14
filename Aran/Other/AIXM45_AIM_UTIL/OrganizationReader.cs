using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Geometries;
using Aran.Aim.DataTypes;

namespace AIXM45_AIM_UTIL
{
	public class OrganisationReader : Aixm45AimUtil.IAIXM45_DATA_READER
	{
		public List<ConvertedObj> ListOfObjects { get; set; }

		public void ReadDataFromTable(string TblName, ITable ARAN_TABLE)
		{
			var indexMid = ARAN_TABLE.Fields.FindField ("mid");
			var indexTxtName = ARAN_TABLE.Fields.FindField ("txtName");
			var indexSnapshotEffectivedate = ARAN_TABLE.FindField ("SnapshotEffectivedate");

			var cursor = ARAN_TABLE.Search (null, true);

			ListOfObjects = new List<ConvertedObj> ();
			IRow row = null;
			while ((row = cursor.NextRow ()) != null)
			{
				try
				{
					var mid = row.get_Value (indexMid).ToString ();
					int midVal;
					if (!int.TryParse (mid, out midVal))
						continue;

					var org = new OrganisationAuthority ();
					org.Identifier = Guid.NewGuid ();
					org.Designator = row.get_Value (indexTxtName).ToString ();
					org.Name = org.Designator;
					

					var convObj = new ConvertedObj (mid, "", org);
					var tmp = row.get_Value (indexSnapshotEffectivedate);
					if (tmp != null && tmp != DBNull.Value)
						convObj.Tag = DateTime.Parse (tmp.ToString ());
					
					ListOfObjects.Add (convObj);
				}
				catch
				{
					continue;
				}
			}
		}
	}
}