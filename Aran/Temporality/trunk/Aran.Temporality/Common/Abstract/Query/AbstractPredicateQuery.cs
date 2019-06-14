using System;

namespace Aran.Temporality.Common.Abstract.Query
{
    public class AbstractPredicateQuery<T> : AbstractQuery
    {
        public AbstractPredicateQuery()
        {
        }

        public AbstractPredicateQuery(Func<T, bool> func)
        {
            Predicate = func;
        }

        public Func<T, bool> Predicate { get; set; }
    }
}