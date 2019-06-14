using Aerodrome.Enums;
using Aerodrome.Features;
using ESRI.ArcGIS.Geometry;
using PDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aerodrome.Converter
{
    public class RwyCenterLinePntConverter : IConverter<RunwayCenterLinePoint, AM_RunwayCenterlinePoint>
    {
        public AM_RunwayCenterlinePoint Convert(RunwayCenterLinePoint source)
        {
            AM_RunwayCenterlinePoint clp = new AM_RunwayCenterlinePoint()
            {
                idnumber = source.ID,
                geopnt=(IPoint)source.Geo
            };
            if (source.Elev != null)
            {
                clp.elev = new DataType.DataType<UomDistance>()
                {
                    Value = source.Elev.Value,
                    Uom = Global.EnumGenericConverter.Convert<UOM_DIST_VERT, UomDistance>(source.Elev_UOM)
                };
            }
            if(source.GeoProperties?.GeoidUndulation!=null)
            {
                clp.geound = new DataType.DataType<UomDistance>()
                {
                    Value = source.GeoProperties.GeoidUndulation.Value,
                    Uom = Global.EnumGenericConverter.Convert<UOM_DIST_HORZ, UomDistance>(source.GeoProperties.GeoidUndulation_UOM)
                };
            }
            return clp;
        }
    }
}
