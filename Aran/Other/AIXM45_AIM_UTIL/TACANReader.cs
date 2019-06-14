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
    public class TACANReader : Aixm45AimUtil.IAIXM45_DATA_READER
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

            IQueryFilter queryFilter = new QueryFilterClass();

            queryFilter.WhereClause = ARAN_TABLE.OIDFieldName + ">= 0";

            ICursor cursor = ARAN_TABLE.Search(queryFilter, true);
            IRow Row = null;
            while ((Row = cursor.NextRow()) != null)
            {
                try
                {
                    double dbl = 0;
                    
                    TACAN tacan = new TACAN();

                    //tacanDesignator
                    tacan.Designator = Row.get_Value(Row.Fields.FindField("R_codeId")).ToString();
                    tacan.Identifier = Guid.NewGuid();

                    //tacanLocation
                    tacan.Location = new ElevatedPoint();
                    Aran.Geometries.Point pnt = new Aran.Geometries.Point();

					var geoLongText = Row.GetString ("R_geoLong");
					var geoLatText = Row.GetString ("R_geoLat");

					pnt.X = Aixm45AimUtil.GetLONGITUDEFromAIXMString (geoLongText); //R_geoLong
					pnt.Y = Aixm45AimUtil.GetLATITUDEFromAIXMString (geoLatText); //R_geoLat

                    tacan.Location.Geo.Assign(pnt);
                    UomDistanceVertical udv;
                    var valElevText = "";
                    if (Enum.TryParse<UomDistanceVertical>(Row.get_Value(Row.Fields.FindField("uomDistVer")).ToString(), true, out udv)) {
                        valElevText = Row.GetString("valElev");
                        if (Double.TryParse(valElevText, out dbl))
                            tacan.Location.Elevation = new ValDistanceVertical(dbl, udv);
                    }

                    //tacanChannel
                    CodeTACANChannel chanel;
                    if (Enum.TryParse<CodeTACANChannel>("_" + Row.get_Value(Row.Fields.FindField("codeChannel")).ToString(), true, out chanel))
                        tacan.Channel = chanel;

                    //tacan.Declination 
                    if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valDeclination")).ToString(), out dbl))
                        tacan.Declination = dbl;

                    //tacanName
                    tacan.Name = Row.get_Value(Row.Fields.FindField("txtName")).ToString();

                    //tacan.MagneticVariation
                    if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valMagVar")).ToString(), out dbl))
                        tacan.MagneticVariation = dbl;

                    //tacan.DateMagneticVariation
                    tacan.DateMagneticVariation = Row.get_Value(Row.Fields.FindField("dateMagVar")).ToString();

					var convObj = new ConvertedObj (Row.GetString ("R_mid"), Row.GetString ("R_OrgMID"), tacan);
					convObj.CRCInfo = new CRCInfo (geoLatText, geoLongText) {
						Height = valElevText,
						Geoid = Row.GetString ("valGeoidUndulation"),
						SourceCRC = Row.GetString ("valCrc"),
						Name = tacan.Designator
					};

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
