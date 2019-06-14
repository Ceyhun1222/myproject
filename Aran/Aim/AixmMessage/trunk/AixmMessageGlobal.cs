using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.AixmMessage
{
    static class AixmMessageGlobal
    {
        static AixmMessageGlobal ()
        {
            Errors = new List<string> ();
        }

        public static List<String> Errors { get; private set;}
    }
}
