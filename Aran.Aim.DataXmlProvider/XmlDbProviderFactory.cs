using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Aim.Data.XmlProvider;

namespace Aran.Aim.Data
{
    public static class XmlDbProviderFactory
    {
        public static IDbProvider Create ()
        {
            return new XmlDbProvider ();
        }
    }
}
