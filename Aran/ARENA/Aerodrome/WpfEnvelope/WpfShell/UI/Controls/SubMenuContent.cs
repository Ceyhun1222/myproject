using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace WpfEnvelope.WpfShell.UI.Controls
{
	// Muss ich machen, da das SubMenu (welches von AutoPopup erbt) kein ControlTemplate hat.
	[TemplatePart(Name = "PART_MenuItemPanel", Type = typeof(Panel))]
	public class SubMenuContent : ContentControl
	{
		private Panel _menuItemPanel;
		private readonly List<UIElement> _menuItems = new List<UIElement>();

		static SubMenuContent()
		{
			DefaultStyleKeyProperty.OverrideMetadata(
				typeof(SubMenuContent),
				new FrameworkPropertyMetadata(typeof(SubMenuContent)));
		}

		public void AddMenuItem(UIElement item)
		{
			_menuItems.Add(item);
			UpdateMenuItemsPanel();
		}

		private void UpdateMenuItemsPanel()
		{
			if (_menuItemPanel != null)
			{
				_menuItemPanel.Children.Clear();
				_menuItems.ForEach(it => _menuItemPanel.Children.Add(it));
			}
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			
			_menuItemPanel = this.Template.FindName("PART_MenuItemPanel", this) as Panel;
			_menuItemPanel = _menuItemPanel == null ? new StackPanel() : _menuItemPanel;
			UpdateMenuItemsPanel();
		}
	}
}
