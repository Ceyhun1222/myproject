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
    public class LocalizerReader : Aixm45AimUtil.IAIXM45_DATA_READER
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

                    Localizer localizer = new Localizer();

                    //localizerDesignator
                    localizer.Designator = Row.get_Value(Row.Fields.FindField("codeId")).ToString();
                    localizer.Identifier = Guid.NewGuid();

                    //localizerLocation
                    localizer.Location = new ElevatedPoint();
                    Aran.Geometries.Point pnt = new Aran.Geometries.Point();

					var geoLongText = Row.GetString ("geoLong");
					var geoLatText = Row.GetString ("geoLat");

					pnt.X = Aixm45AimUtil.GetLONGITUDEFromAIXMString (geoLongText); //R_geoLong
					pnt.Y = Aixm45AimUtil.GetLATITUDEFromAIXMString (geoLatText); //R_geoLat

                    localizer.Location.Geo.Assign(pnt);
                    UomDistanceVertical udv;
                    var valElevText = "";

                    if (Enum.TryParse<UomDistanceVertical>(Row.get_Value(Row.Fields.FindField("uomDistVer")).ToString(), true, out udv)) {
                        valElevText = Row.GetString("valElev");
                        if (Double.TryParse(valElevText, out dbl))
                            localizer.Location.Elevation = new ValDistanceVertical(dbl, udv);
                    }

                    //localizer.Frequency
                    UomFrequency freq;
                    if (Enum.TryParse<UomFrequency>(Row.get_Value(Row.Fields.FindField("uomFreq")).ToString(), true, out freq)) {
                        if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valFreq")).ToString(), out dbl))
                            localizer.Frequency = new ValFrequency(dbl, freq);
                    }

                    //localizer.MagneticBearing 
                    if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valMagBrg")).ToString(), out dbl))
                        localizer.MagneticBearing = dbl;

                    //localizer.TrueBearing
                    if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valTrueBrg")).ToString(), out dbl))
                        localizer.TrueBearing = dbl;

                    //localizer.MagneticVariation 
                    if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valMagVar")).ToString(), out dbl))
                        localizer.MagneticVariation = dbl;

                    //localizer.DateMagneticVariation
                    localizer.DateMagneticVariation = Row.get_Value(Row.Fields.FindField("dateMagVar")).ToString();

                    //localizer.WidthCourse
                    if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valWidCourse")).ToString(), out dbl))
                        localizer.WidthCourse = dbl;

                    //localizer.BackCourseUsable = 

					var convObj = new ConvertedObj (Row.GetString ("R_mid"), Row.GetString ("R_ILSMID"), localizer);
					convObj.CRCInfo = new CRCInfo (geoLatText, geoLongText) {
						Height = valElevText,
						Geoid = Row.GetString ("valGeoidUndulation"),
						SourceCRC = Row.GetString ("valCrc"),
						Name = localizer.Designator
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
