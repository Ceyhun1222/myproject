using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Temporality.Internal.Struct;

namespace Aran.Temporality.Internal.MetaData
{
    internal class WorkPackageMetaData
    {
        public int Index { get; set; }
        public int WorkPackage { get; set; }
        public bool IsSafe { get; set; }
        public string Description;

        public WorkPackageMetaData()
        {
        }

        public WorkPackageMetaData(WorkPackageStructure other)
        {
            WorkPackage = other.WorkPackage;
            IsSafe = other.IsSafe;
            Description = other.Description;
        }

    }
}
