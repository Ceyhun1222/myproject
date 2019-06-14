using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules
{
    static class GuidConverter
    {
        internal static Guid ToGuid(Int64 p1, Int64 p2)
        {
            var bytes = new byte[16];
            Array.Copy(BitConverter.GetBytes(p1), 0, bytes, 0, 8);
            Array.Copy(BitConverter.GetBytes(p2), 0, bytes, 8, 8);
            return new Guid(bytes);
        }

        internal static void ToInt64s(Guid guid, out Int64 p1, out Int64 p2)
        {
            var bytes = guid.ToByteArray();

            p1 = BitConverter.ToInt64(bytes, 0);
            p2 = BitConverter.ToInt64(bytes, 8);
        }
    }
}
