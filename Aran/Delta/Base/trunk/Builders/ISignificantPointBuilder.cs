using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Delta.Model;

namespace Aran.Delta.Builders
{
    public interface ISignificantPointBuilder
    {
        void BuildDesignatedPoint();
        void BuildNavaids();
        void BuildRunwayCenterlinePoints();
        IList<IPointModel> GetSegmentPoints();
    }
}
