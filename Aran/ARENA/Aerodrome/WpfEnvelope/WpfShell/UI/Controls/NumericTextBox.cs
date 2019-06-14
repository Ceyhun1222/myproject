using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfEnvelope.WpfShell.UI.Controls
{
	public class NumericTextBox : TextBox
	{
		static NumericTextBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(
				typeof(NumericTextBox),
				new FrameworkPropertyMetadata(typeof(NumericTextBox)));
		}

		#region AllowDecimal

		public static readonly DependencyProperty AllowDecimalProperty =
			DependencyProperty.Register("AllowDecimal",
			                            typeof (bool),
			                            typeof (NumericTextBox),
			                            new UIPropertyMetadata(false));

		public bool AllowDecimal
		{
			get { return (bool)GetValue(AllowDecimalProperty); }
			set { SetValue(AllowDecimalProperty, value); }
		}

		#endregion

		#region AllowNegative

		public static readonly DependencyProperty AllowNegativeProperty =
			DependencyProperty.Register("AllowNegative",
			                            typeof (bool),
			                            typeof (NumericTextBox),
			                            new UIPropertyMetadata(false));

		public bool AllowNegative
		{
			get { return (bool)GetValue(AllowNegativeProperty); }
			set { SetValue(AllowNegativeProperty, value); }
		}

		#endregion

		protected override void OnPreviewTextInput(TextCompositionEventArgs e)
		{
			int i;
			var numeric = false;

			if (!AllowDecimal)
			{
				numeric = int.TryParse(e.Text, out i);
				if (numeric && !AllowNegative)
					numeric = i >= 0;
			}
			else
				numeric = OnlyNumericCharsUsed(e.Text);

			e.Handled = !numeric;

			base.OnPreviewTextInput(e);
		}

		private bool OnlyNumericCharsUsed(string s)
		{
			var ret = true;
			if (s == NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator |
			    s == NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator |
			    s == NumberFormatInfo.CurrentInfo.CurrencySymbol |
			    s == NumberFormatInfo.CurrentInfo.NegativeSign |
			    s == NumberFormatInfo.CurrentInfo.NegativeInfinitySymbol |
			    s == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator |
			    s == NumberFormatInfo.CurrentInfo.NumberGroupSeparator |
			    s == NumberFormatInfo.CurrentInfo.PercentDecimalSeparator |
			    s == NumberFormatInfo.CurrentInfo.PercentGroupSeparator |
			    s == NumberFormatInfo.CurrentInfo.PercentSymbol |
			    s == NumberFormatInfo.CurrentInfo.PerMilleSymbol |
			    s == NumberFormatInfo.CurrentInfo.PositiveInfinitySymbol |
			    s == NumberFormatInfo.CurrentInfo.PositiveSign)
				return ret;

			var l = s.Length;
			for (var i = 0; i < l; i++)
			{
				var ch = s[i];
				ret &= Char.IsDigit(ch);
			}

			return ret;
		}
	}
}