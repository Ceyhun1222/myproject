using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Geometries;
using AIXM45_AIM_UTIL;

namespace Aran45ToAixm
{
    internal abstract class AbsFieldValueGetter<TEnum>
    {
        public int [] FieldIndexes { get; set; }

        public abstract object CurrentRowItem { get; set; }

        public abstract dynamic this [TEnum fieldEnum] { get; }

        public abstract object [] GetValues (params TEnum [] fieldEnumArr);

        public abstract Geometry GetGeometry ();

        public abstract long GetId ();

		public abstract string GetMid ();

		public virtual CRCInfo GetCRCInfo ()
		{
			return null;
		}
    }

}
