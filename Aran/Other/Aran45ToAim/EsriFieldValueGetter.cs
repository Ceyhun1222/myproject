using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using AIXM45_AIM_UTIL;

namespace Aran45ToAixm
{
    internal class EsriFieldValueGetter <TEnum> : AbsFieldValueGetter <TEnum>
    {
        public override object CurrentRowItem
        {
            get { return _esriFeature; }
            set { _esriFeature = value as IFeature; }
        }

        public override dynamic this [TEnum fieldEnum]
        {
            get
            {
                object val = _esriFeature.get_Value (FieldIndexes [(int) (object) fieldEnum]);

                if (System.DBNull.Value.Equals (val))
                    return null;

                return val;
            }
        }

        public override object [] GetValues (params TEnum [] fieldEnumArr)
        {
            var values = new object [fieldEnumArr.Length];
            for (int i = 0; i <fieldEnumArr.Length; i++)
            {
                values [i] = this [fieldEnumArr [i]];
            }
            return values;
        }

        public override Aran.Geometries.Geometry GetGeometry ()
        {
            var esriGeom = _esriFeature.Shape as ESRI.ArcGIS.Geometry.IGeometry;
            if (esriGeom == null || esriGeom.IsEmpty)
                return null;

            var aranGeom = Aran.Converters.ConvertFromEsriGeom.ToGeometry (esriGeom, true);
            return aranGeom;
        }

        public override long GetId ()
        {
            if (_esriFeature.HasOID)
                return _esriFeature.OID;
            return -1;
        }

		public override string GetMid ()
		{
			return _esriFeature.GetString ("R_mid");
		}

		public override AIXM45_AIM_UTIL.CRCInfo GetCRCInfo ()
		{
			var crcInfo = new CRCInfo ();
			var fieldIndex = _esriFeature.Fields.FindField ("R_geoLat");
			if (fieldIndex >= 0)
				crcInfo.Latitude = _esriFeature.Value [fieldIndex].ToString ();

			fieldIndex = _esriFeature.Fields.FindField ("R_geoLong");
			if (fieldIndex >= 0)
				crcInfo.Longitude = _esriFeature.Value [fieldIndex].ToString ();

			fieldIndex = _esriFeature.Fields.FindField ("valElev");
			if (fieldIndex >= 0)
				crcInfo.Height = _esriFeature.Value [fieldIndex].ToString ();

			fieldIndex = _esriFeature.Fields.FindField ("valGeoidUndulation");
			if (fieldIndex >= 0)
				crcInfo.Geoid = _esriFeature.Value [fieldIndex].ToString ();

			fieldIndex = _esriFeature.Fields.FindField ("valCrc");
			if (fieldIndex >= 0)
				crcInfo.SourceCRC = _esriFeature.Value [fieldIndex].ToString ();

			if (crcInfo.Longitude == null || crcInfo.Longitude == null || crcInfo.SourceCRC == null)
				return null;

			return crcInfo;
		}

        private IFeature _esriFeature;
    }
}
