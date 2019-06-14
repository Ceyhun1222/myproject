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
    public class GlidepathReader : Aixm45AimUtil.IAIXM45_DATA_READER
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
			while ((Row = cursor.NextRow ()) != null)
			{
				try
				{
					double dbl = 0;


					Glidepath glidepath = new Glidepath ();

					//glidepathDesignator
					glidepath.Designator = "";// Row.get_Value(Row.Fields.FindField("R_codeId")).ToString();
					glidepath.Identifier = Guid.NewGuid ();

					//glidepathLocation
					glidepath.Location = new ElevatedPoint ();
					Aran.Geometries.Point pnt = new Aran.Geometries.Point ();

					var geoLongText = Row.GetString ("geoLong");
					var geoLatText = Row.GetString ("geoLat");

					pnt.X = Aixm45AimUtil.GetLONGITUDEFromAIXMString (geoLongText); //R_geoLong
					pnt.Y = Aixm45AimUtil.GetLATITUDEFromAIXMString (geoLatText); //R_geoLat

					glidepath.Location.Geo.Assign (pnt);
					UomDistanceVertical udv;
                    var valElevText = "";
                    if (Enum.TryParse<UomDistanceVertical>(Row.get_Value(Row.Fields.FindField("uomDistVer")).ToString(), true, out udv)) {
                        valElevText = Row.GetString("valElev");
                        if (Double.TryParse(valElevText, out dbl))
                            glidepath.Location.Elevation = new ValDistanceVertical(dbl, udv);
                    }

					//glidepath.Frequency
					UomFrequency freq;
                    if (Enum.TryParse<UomFrequency>(Row.get_Value(Row.Fields.FindField("uomFreq")).ToString(), true, out freq)) {
                        if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valFreq")).ToString(), out dbl))
                            if (dbl > 0)
                                glidepath.Frequency = new ValFrequency(dbl, freq);
                    }

					//glidepath.Rdh
                    if (Enum.TryParse<UomDistanceVertical>(Row.get_Value(Row.Fields.FindField("uomRdh")).ToString(), true, out udv)) {
                        if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valRdh")).ToString(), out dbl))
                            glidepath.Rdh = new ValDistanceVertical(dbl, udv);
                    }

					//glidepath.Slope
                    if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valSlope")).ToString(), out dbl))
                        glidepath.Slope = dbl;


					var convObj = new ConvertedObj (Row.GetString ("R_mid"), Row.GetString ("R_ILSMID"), glidepath);
					convObj.CRCInfo = new CRCInfo (geoLatText, geoLongText) {
						Height = valElevText,
						Geoid = Row.GetString ("valGeoidUndulation"),
						SourceCRC = Row.GetString ("valCrc"),
						Name = glidepath.Designator
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
