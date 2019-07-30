using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Data
{
	public class ChangeIdentifier
	{
		public long Id
		{
			get;
			set;
		}

		public SortedDictionary<int, IAimProperty> Properties
		{
			get;
			set;
		}

		public TimeSlice TimeSlice
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public DateTime CreatedOn
		{
			get;
			set;
		}
	}
}
