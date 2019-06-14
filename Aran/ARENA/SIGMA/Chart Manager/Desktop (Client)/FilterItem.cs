using System.Collections.Generic;

namespace ChartManager
{
    public enum FilterType
    {
        Yes,
        No,
        All
    }

    public class FilterItem
    {
        public FilterType FilterType { get; set; }

        public bool Value => FilterType == FilterType.Yes;

        public override string ToString()
        {
            return FilterType.ToString();
        }
    }

    class FilterBuilder
    {
        private static List<FilterItem> _filterItems;

        public static List<FilterItem> CreateFilterItems()
        {
            return _filterItems ?? (_filterItems = new List<FilterItem>
            {
                new FilterItem {FilterType = FilterType.All},
                new FilterItem {FilterType = FilterType.Yes},
                new FilterItem {FilterType = FilterType.No},                
            });
        }
    }
}
