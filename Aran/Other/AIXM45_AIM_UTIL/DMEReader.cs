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
    public class DMEReader : Aixm45AimUtil.IAIXM45_DATA_READER
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

                    DME dme = new DME();
                    dme.Identifier = Guid.NewGuid();
                    //dme.Designator
                    dme.Designator = Row.get_Value(Row.Fields.FindField("R_codeId")).ToString();

                    //dme.Location
                    dme.Location = new ElevatedPoint();
                    Aran.Geometries.Point pnt = new Aran.Geometries.Point();

					var geoLongText = Row.GetString ("R_geoLong");
					var geoLatText = Row.GetString ("R_geoLat");

					pnt.X = Aixm45AimUtil.GetLONGITUDEFromAIXMString (geoLongText); //R_geoLong
                    pnt.Y = Aixm45AimUtil.GetLATITUDEFromAIXMString(geoLatText); //R_geoLat
                    dme.Location.Geo.Assign(pnt);
                    UomDistanceVertical udv;
                    var valElevText = "";

                    if (Enum.TryParse<UomDistanceVertical>(Row.get_Value(Row.Fields.FindField("uomDistVer")).ToString(), true, out udv)) {
                        valElevText = Row.get_Value(Row.Fields.FindField("valElev")).ToString();
                        if (Double.TryParse(valElevText, out dbl))
                            dme.Location.Elevation = new ValDistanceVertical(dbl, udv);
                    }

                    //dme.Channel
                    CodeDMEChannel chanel;
                    if (Enum.TryParse<CodeDMEChannel>("_"+Row.get_Value(Row.Fields.FindField("codeChannel")).ToString(), true, out chanel))
                        dme.Channel = chanel;

                    //dme.Name
                    dme.Name = Row.get_Value(Row.Fields.FindField("txtName")).ToString();

                    //dme.GhostFrequency
                    UomFrequency freqUom;
                    if (Enum.TryParse<UomFrequency>(Row.get_Value(Row.Fields.FindField("uomGhostFreq")).ToString(), true, out freqUom)) {
                        if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valGhostFreq")).ToString(), out dbl))
                            dme.GhostFrequency = new ValFrequency(dbl, freqUom);
                    }

                    //dme.Displace
                    UomDistance uomdstnc;
                    if (Enum.TryParse<UomDistance>(Row.get_Value(Row.Fields.FindField("uomDisplace")).ToString(), true, out uomdstnc)) {
                        if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valDisplace")).ToString(), out dbl))
                            dme.Displace = new ValDistance(dbl, uomdstnc);
                    }

					var convObj = new ConvertedObj (Row.GetString ("R_mid"),
						Row.GetString ("R_VorMid"), dme);
					convObj.CRCInfo = new CRCInfo ();
					convObj.CRCInfo.Latitude = geoLatText;
					convObj.CRCInfo.Longitude = geoLongText;
					convObj.CRCInfo.Height = valElevText;
					convObj.CRCInfo.Geoid = Row.GetString ("valGeoidUndulation");
					convObj.CRCInfo.SourceCRC = Row.GetString ("valCrc");
					convObj.CRCInfo.Name = dme.Designator;

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
