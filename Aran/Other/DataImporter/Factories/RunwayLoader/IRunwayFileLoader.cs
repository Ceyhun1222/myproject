using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Factories.RunwayLoader
{
    public interface IRunwayFileLoader
    {
        List<RwyCenterPoint> RwyCenterPoints { get; }
    }
}
