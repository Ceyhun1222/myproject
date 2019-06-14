using System.Collections.Generic;

namespace Aran.Temporality.Common.Abstract.Query
{
    //possibly not needed
    internal class MultiClassQuery : AbstractQuery
    {
        public IList<string> ApplicableFeatureTypeNames { get; set; }
    }
}