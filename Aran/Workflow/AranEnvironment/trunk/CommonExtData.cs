using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Package;

namespace Aran.AranEnvironment
{
    public class CommonExtData : Dictionary<string, string>, IPackable
    {
        public void Pack(PackageWriter writer)
        {
            writer.PutInt32(Count);
            foreach (var key in Keys)
            {
                writer.PutString(key);
                writer.PutString(this[key]);
            }
        }

        public void Unpack(PackageReader reader)
        {
            var count = reader.GetInt32();

            for (var i = 0; i < count; i++)
            {
                var key = reader.GetString();
                this[key] = reader.GetString();
            }
        }
    }
}
