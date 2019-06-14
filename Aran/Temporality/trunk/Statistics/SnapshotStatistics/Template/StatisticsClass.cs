using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnapshotStatistics.Template
{
    public class Issue
    {
        public string FeatureType;
        public int Count;
        //public int AllProperties;
        public int Links;
        public int RootProperties;
    }

    public partial class StatisticsPage
    {
        public List<Issue> Issues=new List<Issue>();
        public string FileName;

        public int Count;
        //public int AllProperties;
        public int Links;
        public int RootProperties;
    }
}
