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
    public class ADHPReader : Aixm45AimUtil.IAIXM45_DATA_READER
    {
        private List<ConvertedObj> _ListOfObjects;
        public List<ConvertedObj> ListOfObjects
        {
            get { return _ListOfObjects; }
            set { _ListOfObjects = value; }
        }

        public void ReadDataFromTable(string TblName, ITable ARAN_TABLE)
        {
			var indexR_MID = ARAN_TABLE.Fields.FindField ("R_MID");
			var indexTxtName = ARAN_TABLE.FindField ("txtName");
			var indexR_OrgMid = ARAN_TABLE.Fields.FindField ("R_ORGMid");
			var indexValElev = ARAN_TABLE.Fields.FindField ("valElev");
			var indexValGeoidUndulation = ARAN_TABLE.Fields.FindField ("valGeoidUndulation");
			var indexValCrc = ARAN_TABLE.Fields.FindField ("valCrc");

			ICursor cursor = ARAN_TABLE.Search (null, true);

			ListOfObjects = new List<ConvertedObj> ();
            IRow Row = null;
            while ((Row = cursor.NextRow()) != null)
            {
				try
				{
					double dbl = 0;

					AirportHeliport adhp = new AirportHeliport ();
					adhp.Identifier = Guid.NewGuid ();
					//adhp.Designator
					adhp.Designator = Row.get_Value (Row.Fields.FindField ("R_codeId")).ToString ();
					adhp.Name = Row.get_Value (indexTxtName).ToString ();

					//adhp.ARP
					adhp.ARP = new ElevatedPoint ();
					Aran.Geometries.Point pnt = new Aran.Geometries.Point ();
					var geoLat = Row.get_Value (Row.Fields.FindField ("geoLat")).ToString ();		//R_geoLat
					var geoLong = Row.get_Value (Row.Fields.FindField ("geoLong")).ToString ();		//R_geoLong
					pnt.X = Aixm45AimUtil.GetLONGITUDEFromAIXMString (geoLong);
					pnt.Y = Aixm45AimUtil.GetLATITUDEFromAIXMString (geoLat);
					adhp.ARP.Geo.Assign (pnt);
					UomDistanceVertical udv;
                    var valElevStr = "";

                    if (Enum.TryParse<UomDistanceVertical>(Row.get_Value(Row.Fields.FindField("uomDistVer")).ToString(), true, out udv)) {

                        double valElev;
                        if (Double.TryParse(Row.get_Value(indexValElev).ToString(), out valElev)) {
                            adhp.ARP.Elevation = new ValDistanceVertical(valElev, udv);
                            valElevStr = valElev.ToString();
                        }

                        //adhp.FieldElevation
                        adhp.FieldElevation = new ValDistanceVertical(valElev, udv);


                        //adhp.DateMagneticVariation
                        adhp.DateMagneticVariation = Row.get_Value(Row.Fields.FindField("dateMagVar")).ToString();

                        //adhp.FieldElevationAccuracy
                        if (Enum.TryParse<UomDistanceVertical>(Row.get_Value(Row.Fields.FindField("uomDistVer")).ToString(), true, out udv)) {
                            if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valElevAccuracy")).ToString(), out dbl))
                                adhp.FieldElevationAccuracy = new ValDistanceVertical(dbl, udv);
                        }

                        //adhp.MagneticVariation
                        if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valMagVar")).ToString(), out dbl))
                            adhp.MagneticVariation = dbl;

                        //adhp.TransitionAltitude
                        if (Enum.TryParse<UomDistanceVertical>(Row.get_Value(Row.Fields.FindField("uomTransitionAlt")).ToString(), true, out udv)) {
                            if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valTransitionAlt")).ToString(), out dbl))
                                adhp.TransitionAltitude = new ValDistanceVertical(dbl, udv);
                        }
                    }

					//adhp.Type
					if (Row.get_Value (Row.Fields.FindField ("codeType")).ToString ().StartsWith ("AD"))
						adhp.Type = CodeAirportHeliport.AD;
					else if (Row.get_Value (Row.Fields.FindField ("codeType")).ToString ().StartsWith ("HP"))
						adhp.Type = CodeAirportHeliport.HP;

					var mid = Row.get_Value (indexR_MID).ToString ();
					var orgMid = Row.get_Value (indexR_OrgMid).ToString ();
					var convObj = new ConvertedObj (mid, orgMid, adhp);
					convObj.CRCInfo = new CRCInfo ();
					convObj.CRCInfo.Latitude = geoLat;
					convObj.CRCInfo.Longitude = geoLong;
                    convObj.CRCInfo.Height = valElevStr;
					convObj.CRCInfo.Geoid = Row.get_Value (indexValGeoidUndulation).ToString ();
					convObj.CRCInfo.SourceCRC = Row.get_Value (indexValCrc).ToString ();
					convObj.CRCInfo.Name = adhp.Designator;

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