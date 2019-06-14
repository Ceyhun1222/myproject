using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.ComponentModel;

namespace WpfEnvelope.Crud.Controls
{
	public class TemplatedControl : Control, INotifyPropertyChanged
	{
		protected void AssignTemplateControl<T>(ref T control)
			where T : FrameworkElement, new()
		{
			AssignTemplateControl(ref control, "PART_" + typeof(T).Name);
		}

		protected void AssignTemplateControl<T>(ref T control, string templatePart)
			where T : FrameworkElement, new()
		{
			AssignTemplateControl(ref control, new T(), templatePart);
		}

		protected void AssignTemplateControl<T>(ref T control, T alternativeControl, string templatePart)
			where T : FrameworkElement
		{
			control = alternativeControl;
			object templateChild = GetTemplateChild(templatePart);
			if (templateChild != null && templateChild is T)
				control = (T)templateChild;
			else
				control.ApplyTemplate();
		}

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
