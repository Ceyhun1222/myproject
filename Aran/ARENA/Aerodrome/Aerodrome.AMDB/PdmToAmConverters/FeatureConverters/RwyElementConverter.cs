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
    public class RwyElementConverter : IConverter<RunwayElement, AM_RunwayElement>
    {
        public AM_RunwayElement Convert(RunwayElement source)
        {
            AM_RunwayElement rwyElem = new AM_RunwayElement()
            {                          
                geopoly=(IPolygon)source.Geo,
                idnumber=source.ID
                

            };
            if(source.SurfaceProperties!=null)
            {
                rwyElem.surftype = Global.EnumGenericConverter.Convert<CodeSurfaceCompositionType, SurfaceComposition>(source.SurfaceProperties.Composition);
                rwyElem.pcn = source.SurfaceProperties.ClassPCN?.ToString();
            }
            if(source.Width!=null)
            {
                rwyElem.width = new DataType.DataType<Enums.UomDistance>()
                {
                    Value = source.Width.Value,
                    Uom = Global.EnumGenericConverter.Convert<UOM_DIST_HORZ, UomDistance>(source.WidthUom)
                };
            }
            if(source.Length!=null)
            {
                rwyElem.length = new DataType.DataType<UomDistance>()
                {
                    Value = source.Length.Value,
                    Uom = Global.EnumGenericConverter.Convert<UOM_DIST_HORZ, UomDistance>(source.LengthUom)
                };
            }

            return rwyElem;
        }
    }
}

   