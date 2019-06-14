using System;
using System.Collections.Generic;

namespace Aran.Temporality.Common.OperationResult
{
    //TODO: rename
#warning rename
    public class FeatureDependencyListException: Exception
    {
        public FeatureDependencyListException(string desc)
            : base(desc)
        {
        }

        public List<FeatureDependencyException> ExceptionList { get; set; }
    }
}
