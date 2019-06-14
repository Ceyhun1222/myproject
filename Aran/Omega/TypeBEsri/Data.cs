using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Package;

namespace Aran.Omega.TypeBEsri
{
    public class Data : IPackable
    {
        public string UserName { get; set; }
        public string Aerport { get; set; }
        public double Radius { get; set; }

        public void Pack(PackageWriter writer)
        {
            writer.PutString(UserName);
            writer.PutString(Aerport);
            writer.PutDouble(Radius);
        }

        public void Unpack(PackageReader reader)
        {
            UserName = reader.GetString();
            Aerport = reader.GetString();
            Radius = reader.GetDouble();
        }
    }
}
