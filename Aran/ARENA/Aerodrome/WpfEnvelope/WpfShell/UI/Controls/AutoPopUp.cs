using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfEnvelope.WpfShell.UI.Controls
{
	public class AutoPopup : Popup
	{
		static AutoPopup()
		{
			DefaultStyleKeyProperty.OverrideMetadata(
				typeof(AutoPopup),
				new FrameworkPropertyMetadata(typeof(AutoPopup)));
		}

		protected override void OnOpened(EventArgs e)
		{
			base.OnOpened(e);

			IInputElement ie = Parent as IInputElement;
			if (ie != null)
			{
				ie.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(Parent_LostKeyboardFocus);
				Mouse.AddMouseDownHandler(ie as DependencyObject, Parent_MouseDown);
				Mouse.Capture(ie, CaptureMode.SubTree);
			}
		}

		void Parent_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			if (!IsDescendant(e.NewFocus as Visual)
					&& e.NewFocus != PlacementTarget)
			{
				IsOpen = false;
			}
		}

		private void Parent_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (Mouse.Captured == Parent && e.OriginalSource == Parent)
				IsOpen = false;
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			IInputElement ie = Parent as IInputElement;
			if (ie != null)
			{
				if (Mouse.Captured == Parent)
					Mouse.Capture(ie, CaptureMode.None);
			}
		}

		private bool IsDescendant(Visual node)
		{
			bool success = false;
			Visual curr = node;

			while (curr != null)
			{
				if (curr == this)
				{
					success = true;
					break;
				}

				// Try to jump up logical links if possible.
				FrameworkElement logicalCurrent = curr as FrameworkElement;
				Visual logicalCurrentVisualParent = null;
				// Check for logical parent and make sure it's a Visual
				if (logicalCurrent != null)
				{
					logicalCurrentVisualParent = logicalCurrent.Parent as Visual;
				}

				if (logicalCurrentVisualParent != null)
				{
					curr = logicalCurrentVisualParent;
				}
				else
				{
					// Logical link isn't there; use child link
					curr = VisualTreeHelper.GetParent(curr) as Visual;
				}
			}

			return success;
		}
	}
}
