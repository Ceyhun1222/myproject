using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfEnvelope.Crud
{
	public abstract class BaseConfiguration
	{
		internal BaseConfiguration(MetaTypeRegistration mtr)
		{
			_mtr = mtr;
		}

		protected readonly MetaTypeRegistration _mtr;

		public void SetDescription(string description)
		{
			_mtr.Description = description;
		}
	}
}
