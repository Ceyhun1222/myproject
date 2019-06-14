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
	public partial class ResponsiblePartyControl : UserControl
	{
		private bool _readOnly;

		
		public ResponsiblePartyControl ()
		{
			InitializeComponent ();

			ui_roleCB.FillNullableEnumValues (typeof (CIRoleCode));
		}


		public bool ReadOnly
		{
			get { return _readOnly; }
			set
			{
				_readOnly = value;

				ui_contactCont.ReadOnly = 
					ui_digitalCertificateTB.ReadOnly = 
					ui_individualNameTB.ReadOnly = 
					ui_organisationNameTB.ReadOnly = 
					ui_positionNameTB.ReadOnly = 
					ui_systemNameTB.ReadOnly = value;

				ui_roleCB.Enabled = !value;
			}
		}

		public void SetValue (ResponsibleParty value)
		{
			if (value == null)
			{
				ui_individualNameTB.Text = null;
				ui_organisationNameTB.Text = null;
				ui_positionNameTB.Text = null;
				ui_roleCB.SetNullableEnumValue<CIRoleCode> (null);
				ui_systemNameTB.Text = null;
				ui_digitalCertificateTB.Text = null;
				ui_contactCont.SetValue (null);
			}
			else
			{
				ui_individualNameTB.Text = value.IndividualName;
				ui_organisationNameTB.Text = value.OrganisationName;
				ui_positionNameTB.Text = value.PositionName;
				ui_roleCB.SetNullableEnumValue<CIRoleCode> (value.Role);
				ui_systemNameTB.Text = value.SystemName;
				ui_digitalCertificateTB.Text = value.DigitalCertificate;
				ui_contactCont.SetValue (value.ContactInfo);
			}
		}

		public ResponsibleParty GetValue ()
		{
			var value = new ResponsibleParty ();

			value.IndividualName = ui_individualNameTB.Text;
			value.OrganisationName = ui_organisationNameTB.Text;
			value.PositionName = ui_positionNameTB.Text;
			value.Role = ui_roleCB.GetNullableEnumValue<CIRoleCode> ();
			value.SystemName = ui_systemNameTB.Text;
			value.DigitalCertificate = ui_digitalCertificateTB.Text;
			value.ContactInfo = ui_contactCont.GetValue ();

			if (string.IsNullOrEmpty (value.IndividualName) &&
				string.IsNullOrEmpty (value.OrganisationName) &&
				string.IsNullOrEmpty (value.PositionName) &&
				value.Role == null &&
				string.IsNullOrEmpty (value.SystemName) &&
				string.IsNullOrEmpty (value.DigitalCertificate) &&
				value.ContactInfo == null)
			{
				return null;
			}

			return value;
		}
	}
}
