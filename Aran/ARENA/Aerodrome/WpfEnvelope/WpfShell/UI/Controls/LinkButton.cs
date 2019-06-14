using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Diagnostics;
using System.Windows.Input;

namespace WpfEnvelope.WpfShell.UI.Controls
{
	public class LinkButton : Button
	{
		static LinkButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(
				typeof(LinkButton),
				new FrameworkPropertyMetadata(typeof(LinkButton)));
		}

		#region NavigationUrl

		public string NavigationUrl
		{
			get { return (string)GetValue(NavigationUrlProperty); }
			set { SetValue(NavigationUrlProperty, value); }
		}

		// Using a DependencyProperty as the backing store for NavigationUrl.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty NavigationUrlProperty =
			DependencyProperty.Register("NavigationUrl", typeof(string), typeof(LinkButton), new UIPropertyMetadata(string.Empty));

		#endregion

		#region Text

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register("Text", typeof(string), typeof(LinkButton), new UIPropertyMetadata(string.Empty));

		#endregion

		protected override void OnClick()
		{
			base.OnClick();
			if (!string.IsNullOrEmpty(NavigationUrl))
				Process.Start("rundll32.exe", "url.dll,FileProtocolHandler \"" + NavigationUrl + "\"");
		}
	}
}
