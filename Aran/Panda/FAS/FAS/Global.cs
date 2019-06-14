using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FAS
{
    public static class Global
    {
        public static bool IsTextAllowed(string text, bool allowDot = false)
        {
            var regex = new Regex(allowDot ? "[^0-9.]" : "[^0-9]");
            return !regex.IsMatch(text);
        }
    }
}
