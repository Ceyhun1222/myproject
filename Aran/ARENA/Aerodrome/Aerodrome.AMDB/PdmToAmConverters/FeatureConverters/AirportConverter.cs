
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
    public class AirportConverter : IConverter<AirportHeliport, AM_AerodromeReferencePoint>
    {
        public AM_AerodromeReferencePoint Convert(AirportHeliport source)
        {
            AM_AerodromeReferencePoint arp = new AM_AerodromeReferencePoint()
            {
                idarpt = source.Designator,
                iata = source.DesignatorIATA,
                name=source.Name,
                //geopnt=(PointClass)source.Geo,
                idnumber=source.ID
                
            };
            if (source.Elev != null)
            {
                arp.elev = new DataType.DataType<UomDistance>()
                {
                    Value = source.Elev.Value,
                    Uom = Global.EnumGenericConverter.Convert<UOM_DIST_VERT, UomDistance>(source.Elev_UOM)
                };
            }

            return arp;
        }
    }
}
