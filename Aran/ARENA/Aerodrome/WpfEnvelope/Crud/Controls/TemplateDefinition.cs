using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace WpfEnvelope.Crud.Controls
{
	public class TemplateDefinition
	{
		public TemplateDefinition()
		{
			EntityType = typeof(Control);
			Template = new ControlTemplate(EntityType);
		}

		public Type EntityType { get; set; }
		public ControlTemplate Template { get; set; }
	}
}
