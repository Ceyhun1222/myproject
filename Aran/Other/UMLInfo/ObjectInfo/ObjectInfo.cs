using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UMLInfo
{
    public class ObjectInfo 
        : CommonInfo
    {
        public ObjectInfo ()
        {
            Properties = new List<PropInfo> ();
            RestrictionList = new List<string []> ();
        }

        public ObjectInfoType Type { get; set; }
        public string Namespace { get; set; }
        public ObjectInfo Base { get; set; }
        public bool IsAbstract { get; set; }
        public object Tag { get; set; }

        private bool _isUsed;
        public bool IsUsed
        {
            get { return _isUsed; }
            set { _isUsed = value; }
        }

        public AssocBetween AssocBetween { get; set; }
        public ObjectInfo ReplaceWith { get; set; }
        public bool IsChoice { get; set; }

        public List<PropInfo> Properties { get; private set; }
        public List<string []> RestrictionList { get; private set; }
        public string SubDir { get; set; }
        
        public List<PropInfo> GetAllProperties ()
        {
            List<PropInfo> propInfoList = new List<PropInfo> ();
            if (Base != null)
                propInfoList.AddRange (Base.GetAllProperties ());
            propInfoList.AddRange (Properties);
            return propInfoList;
        }

        public override string ToString ()
        {
            return Name;
        }
    }

    public class AssocBetween
    {
        public ObjectInfo ObjectInfoFrom { get; set; }
        public ObjectInfo ObjectInfoTo { get; set; }

        public override string ToString ()
        {
            return ObjectInfoFrom.Name + " -> " + ObjectInfoTo.Name;
        }
    }
}
