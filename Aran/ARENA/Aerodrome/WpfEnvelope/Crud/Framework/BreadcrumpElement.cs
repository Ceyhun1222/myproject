using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfEnvelope.Crud.Framework
{
	public class BreadcrumpElement
	{
		public ViewModel ViewModel { get; private set; }

		public string Description
		{
			get
			{
				string description = string.Empty;
				description = ViewModel.TypeRegistration.Description;
				if (ViewModel.SelectedItem != null)
					description += " (" + ViewModel.SelectedItem.ToString() + ")";
				return description;
			}
		}

		public BreadcrumpElement(ViewModel model)
		{
			ViewModel = model;
		}

		public override string ToString()
		{
			return Description;
		}
	}
}
