using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.InputFormLib
{
    public static class GlobalFunctions
    {
        public static string GetAssebmlyFileName ()
        {
            return typeof (GlobalFunctions).Assembly.Location;
        }
    }
}
