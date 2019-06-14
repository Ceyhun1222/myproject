using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim;
using Aran.Aim.Objects;
using System.Collections;

namespace Aran.Aim.FeatureInfo
{
    internal class NavInfo
    {
        public NavInfo(string propName, IAimObject aimObject)
            : this(propName)
        {
            AimObject = aimObject;

            PropType = AimMetadata.GetAimTypeName (AimObject);
        }

        public NavInfo(string propName, IAimProperty aimProperty)
            : this(propName)
        {
            AimProperty = aimProperty;

            if (aimProperty.PropertyType == AimPropertyType.List) {
                IList list = aimProperty as IList;
                AimObject = list[0] as IAimObject;
            }
            else {
                AimObject = aimProperty as IAimObject;
            }

            PropType = AimMetadata.GetAimTypeName(AimObject);
        }

        private NavInfo(string propName)
        {
            PropName = propName;
            IsGeomView = false;
        }

        public string PropName { get; private set; }

        public string PropType { get; private set; }

        public IAimProperty AimProperty { get; private set; }

        public IAimObject AimObject { get; private set; }

        public List<BindingInfo> BindingInfoList { get; set; }

        public NavInfo PrevNavInfo { get; set; }

        public bool IsGeomView { get; set; }
    }
}
