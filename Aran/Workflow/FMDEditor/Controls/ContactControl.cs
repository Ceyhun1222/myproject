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
	public partial class ContactControl : UserControl
	{
		private bool _readOnly;


		public ContactControl ()
		{
			InitializeComponent ();
		}


		public bool ReadOnly
		{
			get { return _readOnly; }
			set
			{
				_readOnly = value;

				ui_addressCont.ReadOnly = 
					ui_telephoneCont.ReadOnly = value;
			}
		}

		public void SetValue (Contact value)
		{
			if (value == null)
			{
				ui_addressCont.SetValue (null);
				ui_telephoneCont.SetValue (null);
			}
			else
			{
				ui_addressCont.SetValue (value.Address);
				ui_telephoneCont.SetValue (value.Phone);
			}
		}

		public Contact GetValue ()
		{
			var value = new Contact ();

			value.Address = ui_addressCont.GetValue ();
			value.Phone = ui_telephoneCont.GetValue ();

			if (value.Address == null &&
				value.Phone == null)
			{
				return null;
			}

			return value;
		}
	}
}
