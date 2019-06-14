using Aerodrome.Converter;
using PDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aerodrome.Features;

namespace Aerodrome.Converter
{
    public  class Global
    {
        
        public static List<AM_AbstractFeature> AmObjectList { get; set; }        
        public static IGenericConverter FeatureGenericConverter;
        public static IGenericConverter EnumGenericConverter;
        static Global()
        {
            EnumGenericConverter = new EnumGenericConverter(new EnumConverterFactory());
            FeatureGenericConverter = new FeatureGenericConverter(new FeatureConverterFactory());
            
            AmObjectList = new List<AM_AbstractFeature>();

        }
    }
}
