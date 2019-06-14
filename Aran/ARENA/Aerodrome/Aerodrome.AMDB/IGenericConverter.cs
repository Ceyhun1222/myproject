using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aerodrome.Converter
{
    public interface IGenericConverter
    {
        TTarget Convert<TSource, TTarget>(TSource source);
    }
}
