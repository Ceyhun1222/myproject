using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Package;
using Aran.Aim.Metadata;

namespace Aran.Aim.Data
{
    internal static class Global
    {
        public static string DateTimeToString (DateTime dt)
        {
            return dt.ToString ("yyyy-MM-dd HH:mm:ss");
        }

		public static byte [] GetBytes (IPackable value)
		{
			if (value == null)
				return null;

			using (var ms = new System.IO.MemoryStream ())
			{
				var bpw = new Aran.Package.BinaryPackageWriter (ms);
				value.Pack (bpw);
				return ms.ToArray ();
			}
		}

		public static FeatureTimeSliceMetadata GetMetadata (byte [] buffer)
		{
			using (var bpr = new Aran.Package.BinaryPackageReader (buffer))
			{
				var value = new FeatureTimeSliceMetadata ();
				(value as IPackable).Unpack (bpr);
				return value;
			}
		}
    }
}
