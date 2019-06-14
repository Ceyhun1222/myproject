using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfEnvelope.Crud
{
	public sealed class CrudConfigurationException : Exception
	{
		internal CrudConfigurationException(string message) : base(message) { }
	}
}
