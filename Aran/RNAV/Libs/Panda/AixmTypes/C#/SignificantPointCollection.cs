using System;
using System.Collections.Generic;
using System.Text;
using ARAN.AIXMTypes;

namespace ARAN.Collection
{
	public class SignificantPointCollection : PandaCollection
	{
		public SignificantPointCollection()
			: base()
		{
		}

		public SignificanPoint this[int i]
		{
			get
			{
				return (SignificanPoint)base.GetItem(i);//GetItem(i);
			}
			set
			{
				base.SetItem(i, value);
			}


		}
	}
}
