using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Model.AimMetadata;

namespace Aran.Aim
{
    #region DebuggerDisplay
    [System.Diagnostics.DebuggerDisplay ("Name = {Name}, AimObjectType = {AimObjectType}, SubClassType = {SubClassType}")]
    #endregion
    public class AimClassInfo
    {
        public AimClassInfo ()
        {
            Properties = new AimPropInfoList ();
            EnumValues = new List<AimEnumInfo>();
        }

        public string Name { get; set; }
        public string AixmName { get; set; }
        public int Index { get; set; }
        public AimObjectType AimObjectType { get; set; }
        public bool IsAbstract { get; set; }
        public AimSubClassType SubClassType { get; set; }


        public List<AimEnumInfo> EnumValues { get; private set; }

        public AimPropInfoList Properties { get; private set; }
        
        public AimClassInfo Parent { get; set; }
        public object Tag { get; set; }
		public string Documentation { get; set; }

        public AimPropInfoList GetOwnProperties ()
        {
            if (Parent == null)
                return Properties;
            else
            {
                AimPropInfoList aranPropInfoList = new AimPropInfoList ();
                for (int i = Parent.Properties.Count; i<Properties.Count; i++)
                    aranPropInfoList.Add (Properties [i]);
                return aranPropInfoList;
            }
        }
    }
}
