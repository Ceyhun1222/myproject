using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aran.Aim.FmdEditor
{
	public class NullableNumericUpDown : NumericUpDown
	{
		private bool _isNull;
		private decimal _min;
		private decimal _max;
		private readonly string _nullText;
		private bool _readOnly;


		public NullableNumericUpDown ()
		{
			_nullText = "<NULL>";

			(Controls [1] as TextBox).TextChanged += TextBox_TextChanged;
			_isNull = false;
			_min = Minimum;
			_max = Maximum;
		}


		public new bool ReadOnly
		{
			get { return _readOnly; }
			set
			{
				_readOnly = value;
				Enabled = !_readOnly;
			}
		}

		public new decimal Minimum
		{
			get { return base.Minimum; }
			set
			{
				base.Minimum = value;
				_min = value;
			}
		}

		public new decimal Maximum
		{
			get { return base.Maximum; }
			set
			{
				base.Maximum = value;
				_max = value;
			}
		}

		public bool IsNull
		{
			get { return _isNull; }
		}

		public void SetValue (decimal? value)
		{
			if (value == null)
				_isNull = true;
			else
			{
				base.Maximum = _max;
				_isNull = false;
				Value = value.Value;
			}

			UpdateEditText ();
		}

		public decimal? GetValue ()
		{
			return (_isNull ? null : new Nullable<decimal> (Value));
		}

		
		protected override void UpdateEditText ()
		{
			if (_isNull)
			{
				base.Controls [1].Text = _nullText;
			}
			else
			{
				base.UpdateEditText ();
			}
		}

		private void TextBox_TextChanged (object sender, EventArgs e)
		{
			var s = (sender as Control).Text;
			_isNull = (s == string.Empty || s == _nullText);

			if (_isNull)
			{
				Value = Minimum;
				base.Maximum = Minimum;
			}
			else
			{
				base.Maximum = _max;
			}
		}
	}
}
