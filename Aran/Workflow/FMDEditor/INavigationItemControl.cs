using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.FmdEditor
{
	public interface INavigationItemControl
	{
		bool ReadOnly { get; set; }

		object GetValue ();
		void SetValue (object value);
		object GetNewValue ();
	}
}
