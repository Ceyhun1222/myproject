using Aran.Aim;
using MapEnv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Exporter.Gdb
{
    public class ExportLayerInfo
    {
        public ExportLayerInfo()
        {
            Properties = new List<ExportPropInfo>();
        }

        public AimFeatureLayer Layer { get; set; }

        public bool IsChecked { get; set; }

        public List<ExportPropInfo> Properties { get; private set; }

        public override string ToString()
        {
            if (Layer == null || Layer.Name == null)
                return base.ToString();
            return Layer.Name;
        }
    }

    public class ExportPropInfo
    {
        public ExportPropInfo()
        {
            ChildList = new List<ExportPropInfo>();
        }

        public AimPropInfo AimPropInfo { get; set; }

        public bool IsChecked { get; set; }

        public List<ExportPropInfo> ChildList { get; private set; }

        public bool IsAnyChildChecked ()
        {
            return IsAnyChildChecked(ChildList);
        }

        private bool IsAnyChildChecked(List<ExportPropInfo> childs)
        {
            foreach (var item in childs) {
                if (item.IsChecked)
                    return true;

                if(IsAnyChildChecked(item.ChildList))
                    return true;
            }
            return false;
        }
    }
}
