using Aran.Aim;

namespace TOSSM.Util
{
    public partial class GeoDbUtil
    {
        #region Names

        private static string GetForeinKeyName(string name)
        {
            return name+"_id";
        }

        private static string GetValTypeNameForUom(string name)
        {
            return name + "_uom";
        }

        private static string GetValTypeNameForValue(string name)
        {
            return name + "_val";
        }

        public static string GetEsriName(string name)
        {
            if (name == "Position") return "Position_";
            if (name == "Value") return "Value_";
            return name;
        }


        private static string GetClassName(string parent, string child)
        {
            return  parent + "_" + child;
        }

        private static string GetClassName(string grandparent, string parent, string child)
        {
            return grandparent + "_" + parent + "_" + child;
        }

        
        private static string GetClassName(FeatureType parent)
        {
            return  parent.ToString();
        }

        private static string GetRelationName(string grandparent, string parent, string child)
        {
            if (grandparent == null)
            {
                return parent + "_" + child;
            }

            return grandparent + "_" + parent + "_" + child;
        }

        #endregion

        private static string GetLinkName(string name)
        {
            return name + "_link";
        }
    }

}
