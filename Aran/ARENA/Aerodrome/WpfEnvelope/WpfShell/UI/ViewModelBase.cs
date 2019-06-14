using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Threading;

namespace WpfEnvelope.WpfShell.UI
{
	public abstract class ViewModelBase : INotifyPropertyChanged
	{
		protected readonly Dispatcher _dispatcher;

		protected ViewModelBase(Dispatcher dispatcher)
		{
			_dispatcher = dispatcher;
		}

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
