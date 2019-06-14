using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WpfEnvelope.WpfShell.UI.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;

namespace WpfEnvelope.WpfShell.UI.Controls
{
	[TemplatePart(Name = "PART_TextBlock", Type = typeof(TextBlock))]
	[TemplatePart(Name = "PART_Image", Type = typeof(Image))]
	public class MenuButton : Button
	{
		static MenuButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(
				typeof(MenuButton),
				new FrameworkPropertyMetadata(typeof(MenuButton)));
		}

		public MenuButton() { }

		public MenuButton(Type page)
		{
			_page = page;
		}

		public event EventHandler<NavigationEventArgs> NavigationRequest;

		private TextBlock _textBlock;
		private Image _image;
		private Type _page = null;

		public Action MenuAction { get; set; }

		private SubMenu _subMenu;
		public SubMenu SubMenu
		{
			get { return _subMenu; }
			set
			{
				_subMenu = value;

				Panel panelParent = Parent as Panel;
				if (panelParent == null)
					throw new WrongParentTypeException(); // TODO: den Typ angeben
				panelParent.Children.Add(_subMenu);
			}
		}

		#region Text

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register("Text", typeof(string), typeof(MenuButton), new UIPropertyMetadata(string.Empty));

		#endregion

		#region ImageSource

		public ImageSource ImageSource
		{
			get { return (ImageSource)GetValue(ImageSourceProperty); }
			set { SetValue(ImageSourceProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ImageSourceProperty =
			DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(MenuButton), new UIPropertyMetadata(null));

		#endregion

		protected override void OnClick()
		{
			base.OnClick();

			if (MenuAction != null)
				MenuAction();

			OnNavigationRequest();
		}

		protected void OnNavigationRequest()
		{
			if (SubMenu == null)
			{
				if (NavigationRequest != null && _page != null)
					NavigationRequest(this, new NavigationEventArgs(_page));
			}
			else
			{
				SubMenu.IsOpen = !SubMenu.IsOpen;
			}
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_image = this.Template.FindName("PART_Image", this) as Image;
			_image = _image == null ? new Image() : _image;
			_image.SetBinding(Image.SourceProperty, new Binding()
				{
					Source = this,
					Path = new PropertyPath("ImageSource")
				});

			_textBlock = this.Template.FindName("PART_TextBlock", this) as TextBlock;
			_textBlock = _textBlock == null ? new TextBlock() : _textBlock;
			_textBlock.SetBinding(TextBlock.TextProperty, new Binding()
			{
				Source = this,
				Path = new PropertyPath("Text")
			});
		}
	}
}
