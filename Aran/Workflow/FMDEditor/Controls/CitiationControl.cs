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
	public partial class CitiationControl : UserControl
	{
		private bool _readOnly;

		public CitiationControl ()
		{
			InitializeComponent ();

			ui_dateTypeCB.FillNullableEnumValues (typeof (CIDateTypeCode));
		}

		public bool ReadOnly
		{
			get { return _readOnly; }
			set
			{
				_readOnly = value;

				ui_dateTimePicker.ReadOnly = 
					ui_processCertificationTB.ReadOnly = 
					ui_titleTB.ReadOnly = value;
				ui_dateTypeCB.Enabled = !value;
			}
		}

		public void SetValue (Citiation value)
		{
			if (value == null)
			{
				ui_titleTB.Text = string.Empty;
				ui_processCertificationTB.Text = string.Empty;
				SetCIDateValue (null);
			}
			else
			{
				ui_titleTB.Text = value.Title;
				ui_processCertificationTB.Text = value.processCertification;
				SetCIDateValue (value.Date);
			}
		}

		public Citiation GetValue ()
		{
			var value = new Citiation ();

			value.Title = ui_titleTB.Text;
			value.processCertification = ui_processCertificationTB.Text;
			value.Date = GetCIDateValue ();

			if (string.IsNullOrEmpty (value.Title) && 
				string.IsNullOrEmpty (value.processCertification) &&
				value.Date == null)
			{
				return null;
			}

			return value;
		}

		private void SetCIDateValue (CIDate value)
		{
			if (value == null)
			{
				ui_dateTimePicker.SetValue (null);
				ui_dateTypeCB.SetNullableEnumValue<CIDateTypeCode> (null);
			}
			else
			{
				ui_dateTimePicker.SetValue (value.Date);
				ui_dateTypeCB.SetNullableEnumValue<CIDateTypeCode> (value.DateType);
			}
		}

		public CIDate GetCIDateValue ()
		{
			var value = new CIDate ();
			value.Date = ui_dateTimePicker.GetValue ();
			value.DateType = ui_dateTypeCB.GetNullableEnumValue<CIDateTypeCode> ();

			if (value.Date == null &&
				value.DateType == null)
			{
				return null;
			}

			return value;
		}
	}
}
