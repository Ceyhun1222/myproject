using System.Collections.Generic;
using Aran.Temporality.Common.Enum;

namespace Aran.Temporality.Common.Abstract.Query
{
    //possibly not needed
    public class LogicalQuery : AbstractQuery
    {
        public LogicalCondition LogicalCondition { get; set; }
        public IList<AbstractQuery> Queries { get; set; }
    }
}