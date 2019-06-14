using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aerodrome.Opc
{
	public interface IOpcDataService
	{
		event EventHandler<OpcDataChangedEventArgs> DataChanged;

		IEnumerable<KeyValuePair<string, string>> Cache { get; }
	}
}
