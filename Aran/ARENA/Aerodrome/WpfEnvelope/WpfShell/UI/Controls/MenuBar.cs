using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace WpfEnvelope.WpfShell.UI.Controls
{
	[TemplatePart(Name = "PART_MenuGrid", Type = typeof(UniformGrid))]
	[TemplatePart(Name = "PART_LogoutButton", Type = typeof(Button))]
	public class MenuBar : Control
	{
		static MenuBar()
		{
			DefaultStyleKeyProperty.OverrideMetadata(
				typeof(MenuBar),
				new FrameworkPropertyMetadata(typeof(MenuBar)));
		}

		public event EventHandler LogoutRequest;
		protected void OnLogoutRequest()
		{
			if (LogoutRequest != null)
				LogoutRequest(this, new EventArgs());
		}

		#region MenuGrid

		public UniformGrid MenuGrid
		{
			get { return (UniformGrid)GetValue(MenuGridProperty); }
			set { SetValue(MenuGridProperty, value); }
		}

		// Using a DependencyProperty as the backing store for MenuGrid.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty MenuGridProperty =
			DependencyProperty.Register("MenuGrid", typeof(UniformGrid), typeof(MenuBar), new UIPropertyMetadata(null));

		#endregion

		#region LogoutButton

		public Button LogoutButton
		{
			get { return (Button)GetValue(LogoutElementProperty); }
			set { SetValue(LogoutElementProperty, value); }
		}

		// Using a DependencyProperty as the backing store for LogoutElement.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty LogoutElementProperty =
			DependencyProperty.Register("LogoutButton", typeof(Button), typeof(MenuBar), new UIPropertyMetadata(null));

		#endregion 

		public void Clear()
		{
			MenuGrid.Children.Clear();
		}

		public override void OnApplyTemplate()
		{
			MenuGrid = GetTemplateChild("PART_MenuGrid") as UniformGrid;
			MenuGrid = MenuGrid == null
				? new UniformGrid()
				: MenuGrid;

			LogoutButton = GetTemplateChild("PART_LogoutButton") as Button;
			LogoutButton = LogoutButton == null
				? new Button()
				: LogoutButton;
			LogoutButton.Click += new RoutedEventHandler(LogoutButton_Click);
		}

		void LogoutButton_Click(object sender, RoutedEventArgs e)
		{
			OnLogoutRequest();
		}
	}
}
