using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Data;
using Aran.AranEnvironment;
using Aran.Aim.Features;
using System.Windows.Forms;

namespace Aran45ToAixm
{
    internal static class Global
    {
        static Global ()
        {
            Supported45Features = new List<Type> ();
            Supported45Features.Add (typeof (AirspaceField));
            Supported45Features.Add (typeof (VerticalStructureField));
            Supported45Features.Add (typeof (DesignatedPointField));
            Supported45Features.Add (typeof (RouteSegmentField));

            CurrentFuncTagDict = new Dictionary<Feature, object> ();
        }

        public static Dictionary<Feature, object> CurrentFuncTagDict { get; private set; }

        public static DbProvider DbProvider
        {
            get { return AranEvn.DbProvider as DbProvider; }
        }

        public static IAranEnvironment AranEvn { get; set; }

        public static List<Type> Supported45Features { get; private set; }

        public static string GetFeatureTypeName (Type type)
        {
            string name = type.Name;
            return name.Substring (0, name.Length - "Field".Length);
        }

        #region U

        public static void ShowError (Exception ex)
        {
            MessageBox.Show (ex.Message, "Converter", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion
    }
}
