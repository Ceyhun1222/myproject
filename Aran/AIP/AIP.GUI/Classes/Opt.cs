using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIP.GUI.Classes
{
    public static class Opt
    {
        

        public static void Set(string optName, object optValue)
        {
            try
            {
                Properties.Settings.Default[optName] = optValue;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static object Get(string optName)
        {
            try
            {
                return Properties.Settings.Default[optName];
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }
    }
}
