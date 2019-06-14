using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfEnvelope.Crud.Controls
{
	public class SelectionChangedEventArgs : EventArgs
	{
		public object SelectedItem { get; private set; }

		public SelectionChangedEventArgs(object selectedItem)
		{
			SelectedItem = selectedItem;
		}
	}
}
