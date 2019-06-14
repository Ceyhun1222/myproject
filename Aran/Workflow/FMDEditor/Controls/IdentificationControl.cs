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
	public partial class IdentificationControl : UserControl
	{
		private bool _readOnly;


		public IdentificationControl ()
		{
			InitializeComponent ();

			ui_dataStatusCB.FillNullableEnumValues (typeof (MDProgressCode));
			ui_languageCB.FillNullableEnumValues (typeof (LanguageCodeType));
		}

		
		public bool ReadOnly
		{
			get { return _readOnly; }
			set
			{
				_readOnly = value;

				ui_abstractTB.ReadOnly = 
					ui_citiationCont.ReadOnly = 
					ui_responsiblePartyCont.ReadOnly = value;

				ui_dataStatusCB.Enabled = 
					ui_languageCB.Enabled = !value;
			}
		}

		public void SetValue (IdentificationTimesliceFeature value)
		{
			if (value == null)
			{
				ui_abstractTB.Text = string.Empty;
				ui_dataStatusCB.SetNullableEnumValue<MDProgressCode> (null);
				ui_languageCB.SetNullableEnumValue<LanguageCodeType> (null);

				ui_citiationCont.SetValue (null);
				ui_responsiblePartyCont.SetValue (null);
			}
			else
			{
				ui_abstractTB.Text = value.Abstract;
				ui_dataStatusCB.SetNullableEnumValue<MDProgressCode> (value.DataStatus);
				ui_languageCB.SetNullableEnumValue<LanguageCodeType> (value.Language);

				ui_citiationCont.SetValue (value.Citiation);
				ui_responsiblePartyCont.SetValue (value.PointOfContact);
			}
		}

		public IdentificationTimesliceFeature GetValue ()
		{
			var value = new IdentificationTimesliceFeature ();

			value.Abstract = ui_abstractTB.Text;
			value.DataStatus = ui_dataStatusCB.GetNullableEnumValue<MDProgressCode> ();
			value.Language = ui_languageCB.GetNullableEnumValue<LanguageCodeType> ();
			value.Citiation = ui_citiationCont.GetValue ();
			value.PointOfContact = ui_responsiblePartyCont.GetValue ();

			if (string.IsNullOrEmpty (value.Abstract) &&
				value.DataStatus == null &&
				value.Language == null &&
				value.Citiation == null &&
				value.PointOfContact == null)
			{
				return null;
			}
	
			return value;
		}
	}
}
