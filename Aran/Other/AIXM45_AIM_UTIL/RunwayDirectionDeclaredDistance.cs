using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace AIXM45_AIM_UTIL
{
	public class RunwayDirectionDeclaredDistance : Aixm45AimUtil.IAIXM45_DATA_READER
    {
		public List<ConvertedObj> ListOfObjects { get; set; }

        public void ReadDataFromTable(string TblName, ITable ARAN_TABLE)
        {
            ListOfObjects = new List<ConvertedObj>();
            var cursor = ARAN_TABLE.Search(null, true);
            IRow row = null;

			var indexMid = ARAN_TABLE.FindField ("R_mid");
			var indexRdnMid = ARAN_TABLE.FindField ("R_RdnMid");
			var indexValDict = ARAN_TABLE.FindField ("valDist");
			var indexUomDist = ARAN_TABLE.FindField ("uomDist");
			var indexTxtRmk = ARAN_TABLE.FindField ("txtRmk");
			var indexCodeType = ARAN_TABLE.FindField ("R_codeType");

            while ((row = cursor.NextRow()) != null)
            {
                try
                {
					object tmp1;
					object tmp2;
					double dbl;

					var rdd = new RunwayDeclaredDistance ();
					var rddv = new RunwayDeclaredDistanceValue ();
					rdd.DeclaredValue.Add (rddv);
					
					
					tmp1 = row.get_Value (indexValDict);
					tmp2 = row.get_Value (indexUomDist);

					if (tmp1 != null && tmp2 != null)
					{
						dbl = (double) tmp1;
						UomDistance uomDist;
						if (Enum.TryParse<UomDistance> (tmp2.ToString (), true, out uomDist))
							rddv.Distance = new ValDistance (dbl, uomDist);
					}

					
					tmp1 = row.get_Value (indexCodeType);
					if (tmp1 != null)
					{
						CodeDeclaredDistance cdd;
						if (Enum.TryParse<CodeDeclaredDistance> (tmp1.ToString (), true, out cdd))
							rdd.Type = cdd;
					}


					tmp1 = row.get_Value (indexTxtRmk);
					if (tmp1 != null && !string.IsNullOrWhiteSpace(tmp1.ToString()))
					{
						var note = new Note ();
						var ln = new LinguisticNote ();
						ln.Note = new Aran.Aim.DataTypes.TextNote ();
						ln.Note.Value = tmp1.ToString ();
						note.TranslatedNote.Add (ln);
						rddv.Annotation.Add (note);
					}

					var mid = row.get_Value (indexMid).ToString ();
					var rdnMid = row.get_Value (indexRdnMid).ToString ();

					var convObj = new ConvertedObj (mid, rdnMid, null);
					convObj.Tag = rdd;
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
