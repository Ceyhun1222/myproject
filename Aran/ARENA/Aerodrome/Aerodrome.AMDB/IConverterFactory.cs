using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aerodrome.Converter
{
    public interface IConverterFactory
    {
        Dictionary<Tuple<Type, Type>, Func<object>> _converters { get; set; }
        IConverter<TSource, TTarget> GetConverter<TSource, TTarget>();

        void RegisterConverter<TSource, TTarget>(Func<object> constructor);
    }
}
