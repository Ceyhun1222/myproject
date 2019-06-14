using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Delta.Model
{
    public enum CordinateType
    {
        InStart,
        InEnd
    }

    public class NotamPointClass
    {
        public CordinateType Format { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
    }
}
