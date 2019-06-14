using System;
using System.Collections.Generic;
using Aran.Temporality.Common.Id;

namespace Aran.Temporality.Common.Abstract.Dependency
{
    public abstract class AbstractDependencyManager
    {
        //returns serializable dictionary
        public abstract IDictionary<TimeSliceId, Object> GetObjectRelations(Object root);
        public abstract IList<TimeSliceId> GetFeatureRelations(Object root);
    }
}