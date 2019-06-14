using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aran.Aim.FmdEditor
{
	public class NullableDateTimePicker : DateTimePicker
	{
		private string _customFormat;
		private bool _readOnly;


		public NullableDateTimePicker ()
		{
			CustomFormat = "yyyy-MM-dd";
			Format = DateTimePickerFormat.Custom;
			ShowCheckBox = true;
		}

		
		public bool ReadOnly
		{
			get { return _readOnly; }
			set
			{
				_readOnly = value;
				Enabled = !_readOnly;
			}
		}

		public new string CustomFormat
		{
			get { return base.CustomFormat; }
			set
			{
				_customFormat = value;
				base.CustomFormat = value;
			}
		}

		public bool IsNull
		{
			get { return !Checked; }
		}

		public DateTime? GetValue ()
		{
			return (IsNull ? null : new Nullable<DateTime> (Value));
		}

		public void SetValue (DateTime? value)
		{
			if (value == null)
			{
				Checked = false;
			}
			else
			{
				Checked = true;
				Value = value.Value;
			}

			OnValueChanged (new EventArgs ());
		}


		protected override void OnValueChanged (EventArgs eventargs)
		{
			base.CustomFormat = (IsNull ? "<NULL>" : _customFormat);

			base.OnValueChanged (eventargs);
		}
	}
}
