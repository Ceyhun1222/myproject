using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfEnvelope.WpfShell.UI.Controls
{
	[TemplatePart(Name = "PART_MainBorder1", Type = typeof(Border))]
	[TemplatePart(Name = "PART_MainBorder2", Type = typeof(Border))]
	[TemplatePart(Name = "PART_MainBackgroundRectangle", Type = typeof(Rectangle))]
	[TemplatePart(Name = "PART_HeaderBorder", Type = typeof(Border))]
	[TemplatePart(Name = "PART_ContentBackgroundRectangle", Type = typeof(Rectangle))]
	public class HeadingContentView : ContentControl
	{
		static HeadingContentView()
		{
			DefaultStyleKeyProperty.OverrideMetadata(
				typeof(HeadingContentView),
				new FrameworkPropertyMetadata(typeof(HeadingContentView)));
		}

		private Border _mainBorder1;
		private Border _mainBorder2;
		private Rectangle _mainBackgroundRectangle;
		private Border _headerBorder;
		private Rectangle _contentBackgroundRectangle;
		private bool _isTemplateAssigned = false;

		private double _previousHeaderBorderHeight;
		private Thickness _previousMainBorderThickness;

		#region Header

		public string Header
		{
			get { return (string)GetValue(HeaderProperty); }
			set { SetValue(HeaderProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty HeaderProperty =
			DependencyProperty.Register("Header", typeof(string), typeof(HeadingContentView), new UIPropertyMetadata(string.Empty));

		#endregion

		#region IsSuperiorContainer

		public bool IsSuperiorContainer
		{
			get { return (bool)GetValue(IsSuperiorContainerProperty); }
			set { SetValue(IsSuperiorContainerProperty, value); }
		}

		// Using a DependencyProperty as the backing store for IsSuperiorContainer.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty IsSuperiorContainerProperty =
			DependencyProperty.Register("IsSuperiorContainer", typeof(bool), typeof(HeadingContentView),
			new UIPropertyMetadata(new PropertyChangedCallback(IsSuperiorContainerPropertyChanged)));

		private static void IsSuperiorContainerPropertyChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			var me = (HeadingContentView)s;
			if (me._isTemplateAssigned)
				me.Refresh();
		}

		private void Refresh()
		{
			if (IsSuperiorContainer)
			{
				_headerBorder.Visibility = Visibility.Hidden;
				_previousHeaderBorderHeight = _headerBorder.Height;
				_headerBorder.Height = 0;
				_previousMainBorderThickness = _mainBorder1.BorderThickness;
				_mainBorder1.BorderThickness = new Thickness(0);
				_mainBorder2.BorderThickness = new Thickness(0);
				_contentBackgroundRectangle.Visibility = Visibility.Hidden;

				_mainBackgroundRectangle.Visibility = Visibility.Visible;
			}
			else
			{
				_headerBorder.Visibility = Visibility.Visible;
				_headerBorder.Height = _previousHeaderBorderHeight;
				_mainBorder1.BorderThickness = _previousMainBorderThickness;
				_mainBorder2.BorderThickness = _previousMainBorderThickness;
				_contentBackgroundRectangle.Visibility = Visibility.Visible;

				_mainBackgroundRectangle.Visibility = Visibility.Hidden;
			}
		}

		#endregion

		#region HeaderForeground

		public Brush HeaderForeground
		{
			get { return (Brush)GetValue(HeaderForegroundProperty); }
			set { SetValue(HeaderForegroundProperty, value); }
		}

		// Using a DependencyProperty as the backing store for HeaderForeground.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty HeaderForegroundProperty =
			DependencyProperty.Register(
			"HeaderForeground", 
			typeof(Brush), 
			typeof(HeadingContentView), 
			new UIPropertyMetadata(Brushes.AliceBlue));

		#endregion

		#region HeaderBackground

		public Brush HeaderBackground
		{
			get { return (Brush)GetValue(HeaderBackgroundProperty); }
			set { SetValue(HeaderBackgroundProperty, value); }
		}

		// TODO: DIese Farben gibts auch in Styles-Resources
		// Using a DependencyProperty as the backing store for HeaderBackground.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty HeaderBackgroundProperty =
			DependencyProperty.Register(
			"HeaderBackground",
			typeof(Brush),
			typeof(HeadingContentView),
			new UIPropertyMetadata(new SolidColorBrush(Color.FromRgb(0x5d, 0x65, 0x89))));

		#endregion

		#region ContentBackground

		public Brush ContentBackground
		{
			get { return (Brush)GetValue(ContentBackgroundProperty); }
			set { SetValue(ContentBackgroundProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ContentBackground.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ContentBackgroundProperty =
			DependencyProperty.Register(
			"ContentBackground", 
			typeof(Brush), 
			typeof(HeadingContentView), 
			new UIPropertyMetadata(Brushes.AliceBlue));

		#endregion

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_mainBorder1 = this.Template.FindName("PART_MainBorder1", this) as Border;
			_mainBorder1 = _mainBorder1 == null ? new Border() : _mainBorder1;
			_mainBorder2 = this.Template.FindName("PART_MainBorder2", this) as Border;
			_mainBorder2 = _mainBorder2 == null ? new Border() : _mainBorder2;

			_mainBackgroundRectangle = this.Template.FindName("PART_MainBackgroundRectangle", this) as Rectangle;
			_mainBackgroundRectangle = _mainBackgroundRectangle == null ? new Rectangle() : _mainBackgroundRectangle;

			_headerBorder = this.Template.FindName("PART_HeaderBorder", this) as Border;
			_headerBorder = _headerBorder == null ? new Border() : _headerBorder;

			_contentBackgroundRectangle = this.Template.FindName("PART_ContentBackgroundRectangle", this) as Rectangle;
			_contentBackgroundRectangle = _contentBackgroundRectangle == null ? new Rectangle() : _contentBackgroundRectangle;

			_previousHeaderBorderHeight = _headerBorder.Height;
			_previousMainBorderThickness = _mainBorder1.BorderThickness;
			_isTemplateAssigned = true;
			Refresh();
		}
	}
}
