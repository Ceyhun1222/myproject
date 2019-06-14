using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aerodrome.Enums;

namespace Aerodrome.DataType
{
	public class DataType<TUom>
	{
		public double Value
		{
			get;
			set;
		}

		public TUom Uom
		{
			get;
			set;
		}

		public override string ToString ( )
		{
			double d = Convert.ToDouble ( Value );
			return string.Format ( "{0} {1}", d, Uom );
		}
	}
}
