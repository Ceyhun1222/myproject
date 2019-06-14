using Aerodrome.Enums;
using PDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aerodrome.Converter
{
    public class UomDistHorzConverter : IConverter<UOM_DIST_HORZ, UomDistance>
    {
        public UomDistance Convert(UOM_DIST_HORZ source)
        {
            switch (source)
            {
                case UOM_DIST_HORZ.FT:
                    {
                        return UomDistance.ft;
                    }
                case UOM_DIST_HORZ.M:
                    {
                        return UomDistance.meters;
                    }
                default:
                    return UomDistance.meters;
            }
        }
    }
}
   