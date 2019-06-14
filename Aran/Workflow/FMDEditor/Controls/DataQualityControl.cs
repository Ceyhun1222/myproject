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
	public partial class DataQualityControl : UserControl, INavigationItemControl
	{
		private bool _readOnly;

		
		public DataQualityControl ()
		{
			InitializeComponent ();

			ui_evaluationMethodTypeCB.FillNullableEnumValues (typeof (DQEvaluationMethodTypeCode));
		}

		
		public bool ReadOnly
		{
			get { return _readOnly; }
			set
			{
				_readOnly = value;

				ui_attributesTB.ReadOnly = 
					ui_evaluationMethodNameTB.ReadOnly = value;

				ui_dataQualityElementGrB.Enabled = 
					ui_evaluationMethodTypeCB.Enabled =
					ui_passChB.Enabled = !value;
			}
		}

		public void SetValue (DataQuality value)
		{
			if (value == null)
			{
				ui_attributesTB.Text = null;
				SetElementValue (null);
			}
			else
			{
				ui_attributesTB.Text = value.attributes;
				SetElementValue (value.report);
			}
		}

		public DataQuality GetValue ()
		{
			var value = new DataQuality ();
			
			value.attributes = ui_attributesTB.Text;
			value.report = GetElementValue ();

			if (string.IsNullOrEmpty (value.attributes) &&
				value.report == null)
			{
				return null;
			}

			return value;
		}

		private void SetElementValue (DataQualityElement value)
		{
			if (value == null)
			{
				ui_evaluationMethodNameTB.Text = string.Empty;
				ui_evaluationMethodTypeCB.SetNullableEnumValue<DQEvaluationMethodTypeCode> (null);
				ui_passChB.CheckState = CheckState.Indeterminate;
			}
			else
			{
				ui_evaluationMethodNameTB.Text = value.evaluationMethodName;
				ui_evaluationMethodTypeCB.SetNullableEnumValue<DQEvaluationMethodTypeCode> (value.evaluationMethodType);
				ui_passChB.SetNullableValue (value.pass);
			}
		}

		public DataQualityElement GetElementValue ()
		{
			var value = new DataQualityElement ();

			value.evaluationMethodName = ui_evaluationMethodNameTB.Text;
			value.evaluationMethodType = ui_evaluationMethodTypeCB.GetNullableEnumValue<DQEvaluationMethodTypeCode> ();
			value.pass = ui_passChB.GetNullableValue ();

			if (string.IsNullOrEmpty (value.evaluationMethodName) &&
				value.evaluationMethodType == null &&
				value.pass == null)
			{
				return null;
			}

			return value;
		}

		object INavigationItemControl.GetValue ()
		{
			var value = GetValue ();

			if (value == null)
				return new DataQuality ();

			return value;
		}

		void INavigationItemControl.SetValue (object value)
		{
			SetValue (value as DataQuality);
		}

		object INavigationItemControl.GetNewValue ()
		{
			return new DataQuality ();
		}
	}
}
