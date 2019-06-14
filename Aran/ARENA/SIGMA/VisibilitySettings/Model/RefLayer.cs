using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisibilityTool.Model
{
    public class RefLayer
    {
        public RefLayer()
        {
            IsAnnotation = false;
        }

        public string RefIdField
        {
            get;
            set;
        }

        public string TableName
        {
            get;
            set;
        }

        public bool IsAnnotation
        {
            get;
            set;
        }

        public string SplittedLayerName { get; set; }
    }
}
