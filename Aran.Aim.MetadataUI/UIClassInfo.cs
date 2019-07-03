using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.Metadata.UI
{
    public class UIClassInfo
    {
        public UIClassInfo ()
        {
            RefInfoList = new List<UIReferenceInfo> ();
        }

        public string Caption { get; set; }
        public string AixmNamespace { get; set; }
        public string DependsFeature { get; set; }
        public string DescriptionFormat { get; set; }

        public List<UIReferenceInfo> RefInfoList { get; private set; }
    }

    public class UIReferenceInfo
    {
        public UIReferenceInfo ()
        {
            PropertyPath = new List<PropertyPathInfo> ();
        }

        public List<PropertyPathInfo> PropertyPath { get; private set; }

        public AimClassInfo ClassInfo { get; set; }

        public PropertyDirection Direction { get; set; }
    }

    public class PropertyPathInfo
    {
        public PropertyPathInfo () : 
            this (null, false)
        {
        }

        public PropertyPathInfo (string name, bool isList, 
            PropertyPathStatus status = PropertyPathStatus.Normal)
        {
            Name = name;
            IsList = isList;
            Status = status;
        }

        public string Name { get; set; }

        public bool IsList { get; set; }

        public PropertyPathStatus Status { get; set; }
    }

    public enum PropertyPathStatus
    {
        Default, Normal, Hide, Remove
    }

    public enum PropertyDirection
    {
        Sub, Ref
    }
}
