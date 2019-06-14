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
    public class RunwayDirectionReader : Aixm45AimUtil.IAIXM45_DATA_READER
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

					RunwayDirection rdn = new RunwayDirection ();
					rdn.Identifier = Guid.NewGuid ();
					//rdn.Designator
					rdn.Designator = Row.get_Value (Row.Fields.FindField ("R_txtDesig")).ToString ();

					//rdn.ElevationTDZ
					UomDistanceVertical udv;
                    if (Enum.TryParse<UomDistanceVertical>(Row.get_Value(Row.Fields.FindField("uomElevTdz")).ToString(), true, out udv)) {
                        if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valElevTdz")).ToString(), out dbl))
                            rdn.ElevationTDZ = new ValDistanceVertical(dbl, udv);
                    }

					//rdn.ElevationTDZAccuracy
					UomDistance udst;
                    if (Enum.TryParse<UomDistance>(Row.get_Value(Row.Fields.FindField("uomElevTdz")).ToString(), true, out udst)) {
                        if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valElevTdzAccuracy")).ToString(), out dbl))
                            rdn.ElevationTDZAccuracy = new ValDistance(dbl, udst);
                    }

					//rdn.MagneticBearing
                    if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valMagBrg")).ToString(), out dbl))
                        rdn.MagneticBearing = dbl;

					//rdn.TrueBearing
                    if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valTrueBrg")).ToString(), out dbl))
                        rdn.TrueBearing = dbl;

					//rdn.SlopeTDZ
					if (Double.TryParse (Row.get_Value (Row.Fields.FindField ("valSlopeAngleGpVasis")).ToString (), out dbl))
					    rdn.SlopeTDZ = dbl;

					var convObj = new ConvertedObj (
						Row.get_Value (Row.Fields.FindField ("R_mid")).ToString (),
						Row.get_Value (Row.Fields.FindField ("R_RWYmid")).ToString (),
						rdn);

					//RunwayDirection  Location for RunwayCentrelinePoint
					var location = new AixmPoint ();
					var pnt = new Aran.Geometries.Point ();
					pnt.X = Aixm45AimUtil.GetLONGITUDEFromAIXMString (
						Row.get_Value (Row.Fields.FindField ("geoLong")).ToString ()); //geoLong
					pnt.Y = Aixm45AimUtil.GetLATITUDEFromAIXMString (
						Row.get_Value (Row.Fields.FindField ("geoLat")).ToString ()); //R_geoLat
					location.Geo.Assign (pnt);

					convObj.Tag = location;

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
