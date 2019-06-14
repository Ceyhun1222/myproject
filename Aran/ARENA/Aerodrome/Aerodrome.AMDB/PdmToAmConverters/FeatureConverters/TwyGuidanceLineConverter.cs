using Aerodrome.Enums;
using Aerodrome.Features;
using PDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aerodrome.Converter
{
    public class TwyGuidanceLineConverter : IConverter<GuidanceLine, AM_TaxiwayGuidanceLine>
    {
        public AM_TaxiwayGuidanceLine Convert(GuidanceLine source)
        {
            AM_TaxiwayGuidanceLine twyGuidLine = new AM_TaxiwayGuidanceLine()
            {
                idnumber = source.ID,
            };
            if(source.MaxSpeed!=null && !Double.IsNaN(source.MaxSpeed.Value))
            {
                twyGuidLine.maxspeed = new DataType.DataType<UomSpeed>()
                {
                    Value = source.ConvertSpeedToKNOT(source.MaxSpeed.Value, source.MaxSpeed_UOM.ToString()),
                    Uom=UomSpeed.knots                    
                };
                
            }
            if (source.UsageDirection != null)
            {
                twyGuidLine.direc = Global.EnumGenericConverter.Convert<CodeDirectionType, AM_Direction>(source.UsageDirection);
            }
            
            if(source.GuidanceLineMarkingList?.Count>0 && source.GuidanceLineMarkingList[0].MarkingElementList.Count>0)
            {
                twyGuidLine.color = Global.EnumGenericConverter.Convert<ColourType, AM_Color>(source.GuidanceLineMarkingList[0].MarkingElementList[0].Colour);
            }

            return twyGuidLine;
        }
    }
}
