using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aerodrome.Converter
{
    class EnumGenericConverter : IGenericConverter
    {
        private readonly IConverterFactory _factory;

        public EnumGenericConverter(IConverterFactory factory)
        {
            _factory = factory;
        }

        public TTarget Convert<TSource, TTarget>(TSource source)
        {
            var converter = _factory.GetConverter<TSource, TTarget>();
            return converter.Convert(source);
            
        }
     
    }
}
