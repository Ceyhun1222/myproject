using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Metadata;
using Aran.Aim.Enums;

namespace Aran.Aim.FmdEditor
{
	public partial class TelephoneControl : UserControl
	{
		private bool _readOnly;


		public TelephoneControl ()
		{
			InitializeComponent ();

			ui_codeCB.FillNullableEnumValues (typeof (PhoneCodeType));
		}
		

		public bool ReadOnly
		{
			get { return _readOnly; }
			set
			{
				_readOnly = value;
				ui_descriptionTB.ReadOnly = 
					ui_numberTB.ReadOnly = value;
				ui_codeCB.Enabled = !value;
			}
		}

		public void SetValue (Telephone value)
		{
			if (value == null)
			{
				ui_codeCB.SetNullableEnumValue<PhoneCodeType> (null);
				ui_numberTB.Text = null;
				ui_descriptionTB.Text = null;
			}
			else
			{
				ui_codeCB.SetNullableEnumValue<PhoneCodeType> (value.CodeType);
				ui_numberTB.Text = value.Number;
				ui_descriptionTB.Text = value.OtherDescription;
			}
		}

		public Telephone GetValue ()
		{
			var value = new Telephone ();
			value.CodeType = ui_codeCB.GetNullableEnumValue<PhoneCodeType> ();
			value.Number = ui_numberTB.Text;
			value.OtherDescription = ui_descriptionTB.Text;

			if (value.CodeType == null && 
				string.IsNullOrEmpty (value.Number) &&
				string.IsNullOrEmpty (value.OtherDescription))
			{
				return null;
			}
			return value;
		}
	}
}
