using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfEnvelope.WpfShell
{
	public class NavigationEventArgs : EventArgs
	{
		public Type Page { get; private set; }

		public NavigationEventArgs(Type page)
		{
			Page = page;
		}
	}
}
