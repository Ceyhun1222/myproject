using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.AranEnvironment;

namespace Aran.Aim.Data.LocalDbLoader
{
    static class Global
    {
        public static IAranEnvironment AranEnv { get; set; }

        public static LoaderForm LoaderForm { get; set; }
    }
}
