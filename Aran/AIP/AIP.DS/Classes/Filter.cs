using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim;

namespace AIP.DataSet.Classes
{
    public class Filter
    {
        public string Property { get; set; }
        public string Value { get; set; }
        public FilterOperation Operation { get; set; }

        public Filter(string property, string value, FilterOperation operation)
        {
            Property = property;
            Value = value;
            Operation = operation;
        }
    }

    public class ClassFilter
    {
        public FeatureType FeatureType { get; set; }

        public List<Filter> Filter { get; set; }

        public ClassFilter(FeatureType featType, List<Filter> filter)
        {
            FeatureType = featType;
            Filter = filter;
        }
    }


    public enum FilterOperation
    {
        None,
        Equals,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Contains,
        StartsWith,
        EndsWith
    }
}
