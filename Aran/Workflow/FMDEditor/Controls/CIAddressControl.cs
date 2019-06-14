using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Metadata;

namespace Aran.Aim.FmdEditor
{
	public partial class CIAddressControl : UserControl
	{
		private bool _readOnly;

		public CIAddressControl ()
		{
			InitializeComponent ();

			_readOnly = false;
		}

		public bool ReadOnly
		{
			get { return _readOnly; }
			set
			{
				_readOnly = value;

				ui_administrativeAreaTB.ReadOnly = 
					ui_cityTB.ReadOnly = 
					ui_countryTB.ReadOnly = 
					ui_deliveryPointTB.ReadOnly = 
					ui_electronicMailAddressTB.ReadOnly =
					ui_postalCodeTB.ReadOnly = _readOnly;
			}
		}

		public void SetValue (CIAddress value)
		{
			if (value == null)
			{
				ui_deliveryPointTB.Text = null;
				ui_cityTB.Text = null;
				ui_administrativeAreaTB.Text = null;
				ui_postalCodeTB.Text = null;
				ui_countryTB.Text = null;
				ui_electronicMailAddressTB.Text = null;
			}
			else
			{
				ui_deliveryPointTB.Text = value.deliveryPoint;
				ui_cityTB.Text = value.city;
				ui_administrativeAreaTB.Text = value.administrativeArea;
				ui_postalCodeTB.Text = value.postalCode;
				ui_countryTB.Text = value.country;
				ui_electronicMailAddressTB.Text = value.electronicMailAddress;
			}
		}

		public CIAddress GetValue ()
		{
			var value = new CIAddress ();

			value.deliveryPoint = ui_deliveryPointTB.Text;
			value.city = ui_cityTB.Text;
			value.administrativeArea = ui_administrativeAreaTB.Text;
			value.postalCode = ui_postalCodeTB.Text;
			value.country = ui_countryTB.Text;
			value.electronicMailAddress = ui_electronicMailAddressTB.Text;

			if (string.IsNullOrEmpty (value.deliveryPoint) &&
				string.IsNullOrEmpty (value.city) &&
				string.IsNullOrEmpty (value.administrativeArea) &&
				string.IsNullOrEmpty (value.postalCode) &&
				string.IsNullOrEmpty (value.country) &&
				string.IsNullOrEmpty (value.electronicMailAddress))
			{
				return null;
			}
			
			return value;
		}
	}
}
