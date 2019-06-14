using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.Metadata.UI
{
    internal static class GlobalFunctions
    {
        public static string GetAssebmlyFileName ()
        {
            return typeof (GlobalFunctions).Assembly.Location;
        }
    }
}
