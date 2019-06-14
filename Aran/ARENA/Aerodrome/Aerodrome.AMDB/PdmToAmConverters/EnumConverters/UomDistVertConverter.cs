using Aerodrome.Enums;
using PDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aerodrome.Converter
{
    public class UomDistVertConverter : IConverter<UOM_DIST_VERT, UomDistance>
    {
        public UomDistance Convert(UOM_DIST_VERT source)
        {
            switch(source)
            {
                case UOM_DIST_VERT.FT:
                    {
                        return UomDistance.ft;
                    }
                case UOM_DIST_VERT.M:
                    {
                        return UomDistance.meters;
                    }
                default:
                    return UomDistance.meters;
            }
        }
    }
}
