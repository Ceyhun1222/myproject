using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Queries.Common
{
    public class Settings
    {
        public static Settings Instance { get; private set; }

        static Settings ()
        {
            Instance = new Settings ();
        }

        public Settings ()
        {
            CoordinateFormatIsDMS = true;
            CoordinateFormatAccuracy = 4;
        }

        public bool CoordinateFormatIsDMS { get; set; }

        public int CoordinateFormatAccuracy { get; set; }
    }
}
