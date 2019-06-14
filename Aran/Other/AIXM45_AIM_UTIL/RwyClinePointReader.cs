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
    public class RwyClinePointReader: Aixm45AimUtil.IAIXM45_DATA_READER
    {
        private List<ConvertedObj> _ListOfObjects;
        public List<ConvertedObj> ListOfObjects
        {
            get { return _ListOfObjects; }
            set { _ListOfObjects = value; }
        }

        public void ReadDataFromTable(string TblName, ITable ARAN_TABLE)
        {
            ListOfObjects = new List<ConvertedObj>();

            var queryFilter = new QueryFilterClass();

            queryFilter.WhereClause = ARAN_TABLE.OIDFieldName + ">= 0";

			var index_R_mid = ARAN_TABLE.FindField ("R_mid");
			var index_R_geoLong = ARAN_TABLE.FindField ("R_geoLong");
			var index_R_geoLat = ARAN_TABLE.FindField ("R_geoLat");
			var index_uomDistVer = ARAN_TABLE.FindField ("uomDistVer");
			var index_valElev = ARAN_TABLE.FindField ("valElev");
			var index_R_RWYMID = ARAN_TABLE.FindField ("R_RWYMID");
			var index_txtRmk = ARAN_TABLE.FindField ("txtRmk");
			var index_valCRC = ARAN_TABLE.FindField ("valCrc");

            ICursor cursor = ARAN_TABLE.Search(queryFilter, true);
            IRow Row = null;
            
			while ((Row = cursor.NextRow()) != null)
            {
                try
                {
                    double dbl = 0;

                    RunwayCentrelinePoint rcp = new RunwayCentrelinePoint();
                    rcp.Identifier = Guid.NewGuid();

                    //rcp.Designator
                    rcp.Designator = "";

                    //rcp.Location
                    rcp.Location = new ElevatedPoint();
                    Aran.Geometries.Point pnt = new Aran.Geometries.Point();
					
					var geoLong = Row.get_Value (index_R_geoLong).ToString ();
					var geoLat = Row.get_Value (index_R_geoLat).ToString ();

					pnt.X = Aixm45AimUtil.GetLONGITUDEFromAIXMString (geoLong); //R_geoLong
                    pnt.Y = Aixm45AimUtil.GetLATITUDEFromAIXMString(geoLat); ; //R_geoLat
                    rcp.Location.Geo.Assign(pnt);
                    UomDistanceVertical udv;
                    var valElev = "";

					var uomDistVerVal = Row.get_Value(index_uomDistVer).ToString();
                    if (Enum.TryParse<UomDistanceVertical>(uomDistVerVal, true, out udv)) {
                        valElev = Row.get_Value(index_valElev).ToString();
                        if (Double.TryParse(valElev, out dbl))
                            rcp.Location.Elevation = new ValDistanceVertical(dbl, udv);
                    }
					
					var txtRmkVal = Row.get_Value (index_txtRmk).ToString ();

					if (txtRmkVal.StartsWith ("THR"))
						rcp.Role = CodeRunwayPointRole.THR;
					else if (txtRmkVal.IndexOf ("Sequence number") > 0)
						rcp.Role = CodeRunwayPointRole.MID;

					var mid = Row.get_Value (index_R_mid).ToString ();

					var convObj = new ConvertedObj (
						mid,
						Row.get_Value (index_R_RWYMID).ToString (),
						rcp);

					convObj.CRCInfo = new CRCInfo ();
					convObj.CRCInfo.Latitude = geoLat;
					convObj.CRCInfo.Longitude = geoLong;
                    convObj.CRCInfo.Height = valElev;
					convObj.CRCInfo.SourceCRC = Row.get_Value (index_valCRC).ToString ();
					convObj.CRCInfo.Name = mid;

                    ListOfObjects.Add(convObj);
                }
                catch
                {
                    continue;
                }

            }

        }
    }
}
