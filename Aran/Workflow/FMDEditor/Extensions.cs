using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aran.Aim.FmdEditor
{
	public static class Extensions
	{
		public static void FillNullableEnumValues (this ComboBox cb, Type enumType)
		{
			cb.Items.Add (string.Empty);
			foreach (var item in Enum.GetValues (enumType))
				cb.Items.Add (item.ToString ());
		}

		public static void SetNullableEnumValue<T> (this ComboBox cb, T? value) where T : struct
		{
			if (value == null)
				cb.SelectedItem = string.Empty;
			else
				cb.SelectedItem = value.Value.ToString ();
		}

		public static Nullable <T> GetNullableEnumValue <T> (this ComboBox cb) where T : struct
		{
			var s = (string) cb.SelectedItem;
			if (s == string.Empty)
				return null;

			T res;
			Enum.TryParse<T> (s, out res);
			return res;
		}

		public static void SetNullableValue (this CheckBox chb, bool? value)
		{
			chb.CheckState = (value == null ? CheckState.Indeterminate : 
				(value.Value ? CheckState.Checked : CheckState.Unchecked));
		}

		public static bool? GetNullableValue (this CheckBox chb)
		{
			if (chb.CheckState == CheckState.Indeterminate)
				return null;
			return chb.Checked;
		}

		//public static void SetNullableValue (this DateTimePicker dtp, DateTime? value)
		//{
		//    if (value != null)
		//        dtp.Value = value.Value;
		//}

		//public static DateTime? GetNullableValue (this DateTimePicker dtp)
		//{
		//    if (dtp.Value == default (DateTime))
		//        return null;
		//    return dtp.Value;
		//}

		//public static void SetNullableValue (this NullableNumericUpDown nud, double? value)
		//{
		//    if (value != null)
		//        nud.SetValue (Convert.ToDecimal (value.Value));
		//}

		//public static double? GetNullableValue (this NullableNumericUpDown nud)
		//{
		//    return Convert.ToDouble (nud.GetValue ());
		//}
	}
}
