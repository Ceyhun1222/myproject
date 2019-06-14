using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BRules.SbvrParser
{
    public static class MyExtensions
    {
        public static bool StartsWithIC(this string text, string value)
        {
            return text.StartsWith(value, StringComparison.CurrentCultureIgnoreCase);
        }

        public static int IndexOfIC(this string text, string value, int startIndex = 0)
        {
            return text.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
