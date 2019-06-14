using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aerodrome.Opc
{
	public class OpcDataChangedEventArgs : EventArgs
	{
		public OpcDataChangedEventArgs(string variableName, string value)
		{
			Value = value;
			VariableName = variableName;
		}

		public string Value { get; private set; }
		public string VariableName { get; private set; }
	}
}
