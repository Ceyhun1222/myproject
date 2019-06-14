using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsolineToVS
{
    interface IFormListener
    {
        IEnumerable<string> GetLayerNames();
        string GenerateMessage(
            string layerName,
            string fileName,
            double horizontalAccuracy,
            bool write3DCoordinateIfExists,
            out bool isOk);
    }
}
