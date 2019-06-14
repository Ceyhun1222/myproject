using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Delta.Model;

namespace Aran.Delta.Builders
{
    public interface ISignificantPointDirector
    {
        IList<PointModel> Create(ISignificantPointBuilder builder);
        
    }

    class SignificantPointDirector
    {
        public IList<IPointModel> Create(ISignificantPointBuilder builder)
        {
            builder.BuildDesignatedPoint();
            builder.BuildNavaids();
            builder.BuildRunwayCenterlinePoints();
            return builder.GetSegmentPoints();
        }
    }
}
