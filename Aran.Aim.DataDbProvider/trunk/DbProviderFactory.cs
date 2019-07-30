using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace Aran.Aim.Data
{
    public static class DbProviderFactory
    {
        public static DbProvider Create (string assemblyName)
        {
            string dllPath = Assembly.GetAssembly (typeof (DbProviderFactory)).Location;
            string dir = Path.GetDirectoryName (dllPath);

            return Create (dir, assemblyName);
        }

        public static DbProvider Create (string dllDir, string assemblyName)
        {
            string assemblyFile =
                (dllDir.EndsWith ("\\") ? dllDir : dllDir + "\\") +
                (assemblyName.EndsWith (".dll") ? assemblyName : assemblyName + ".dll");

			Assembly asmb = Assembly.LoadFrom(assemblyFile);
            Type [] typeArr = asmb.GetTypes ();
            var dbProType = typeof(DbProvider);

            foreach (Type type in typeArr)
            {
                if (dbProType.IsAssignableFrom(type)) {
                    var dbPro = Activator.CreateInstance(type) as DbProvider;
                    return dbPro;
                }
            }

            return null;
        }
    }
}
