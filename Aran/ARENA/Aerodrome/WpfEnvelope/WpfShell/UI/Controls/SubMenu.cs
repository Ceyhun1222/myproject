using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WpfEnvelope.WpfShell.UI.Controls
{
	public class SubMenu : AutoPopup
	{
		private readonly SubMenuContent _subMenuContent;

		static SubMenu()
		{
			DefaultStyleKeyProperty.OverrideMetadata(
				typeof(SubMenu),
				new FrameworkPropertyMetadata(typeof(SubMenu)));
		}

		public SubMenu(UIElement placementTarget)
			: base()
		{
			// OPT: in resource auslagern
			PlacementTarget = placementTarget;

			_subMenuContent = new SubMenuContent();
			Child = _subMenuContent;
		}

		public void AddMenuElement(UIElement element)
		{
			_subMenuContent.AddMenuItem(element);
		}
	}
}
