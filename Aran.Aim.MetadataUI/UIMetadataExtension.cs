using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.Metadata.UI
{
    public static class UIMetadataExtension
    {
        public static UIClassInfo UiInfo (this AimClassInfo classInfo)
        {
            return classInfo.Tag as UIClassInfo;
        }

        public static UIPropInfo UiPropInfo (this AimPropInfo propInfo)
        {
            return propInfo.Tag as UIPropInfo;
        }
    }
}
