using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AIP.DataSet.Lib;

namespace AIP.DataSet.Classes
{
    public class Attributes
    {
        public string RefDir { get; set; }
        public string IsInclude { get; set; }

        public string ToTag()
        {
            try
            {
                string output = "";
                foreach (PropertyInfo prop in this.GetType().GetProperties())
                {
                    output += $"#{prop.Name}={prop.GetValue(this, null)};";
                }
                return output;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }
    }


}
