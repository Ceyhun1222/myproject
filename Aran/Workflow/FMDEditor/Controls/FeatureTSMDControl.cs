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
	public partial class FeatureTSMDControl : UserControl
	{
		private bool _readOnly;

		
		public FeatureTSMDControl ()
		{
			InitializeComponent ();

			ui_measureClassCB.FillNullableEnumValues (typeof (MeasureClassCode));
		}


		public bool ReadOnly
		{
			get { return _readOnly; }
			set
			{
				_readOnly = value;

				ui_dataIntegrityNNud.ReadOnly = 
					ui_dataQualityCont.ReadOnly = 
					ui_dateStampNDTP.ReadOnly = 
					ui_DQNavCont.ReadOnly = 
					ui_horizontalResolutionNNud.ReadOnly = 
					ui_identificationCont.ReadOnly = 
					ui_measEquipClassTB.ReadOnly = 
					ui_responsiblePartyCont.ReadOnly = 
					ui_verticalResolutionNNud.ReadOnly = value;

				ui_measureClassCB.Enabled = !value;
			}
		}

		public void SetValue (FeatureTimeSliceMetadata value)
		{
			if (value == null)
			{
				ui_dateStampNDTP.SetValue (null);
				ui_measEquipClassTB.Text = null;
				ui_dataIntegrityNNud.SetValue (null);
				ui_horizontalResolutionNNud.SetValue (null);
				ui_verticalResolutionNNud.SetValue (null);
				ui_measureClassCB.SetNullableEnumValue<MeasureClassCode> (null);

				ui_responsiblePartyCont.SetValue (null);
				ui_DQNavCont.SetValue (null);
				ui_identificationCont.SetValue (null);
			}
			else
			{
				ui_dateStampNDTP.SetValue (value.DateStamp);
				ui_measEquipClassTB.Text = value.MeasEquipClass;
				ui_dataIntegrityNNud.SetValue ((decimal?) value.DataIntegrity);
				ui_horizontalResolutionNNud.SetValue ((decimal?) value.HorizontalResolution);
				ui_verticalResolutionNNud.SetValue ((decimal?) value.VerticalResolution);
				ui_measureClassCB.SetNullableEnumValue<MeasureClassCode> (value.MeasureClass);

				ui_responsiblePartyCont.SetValue (value.Contact);
				ui_DQNavCont.SetValue (value.DataQualityInfo);
				ui_identificationCont.SetValue (value.FeatureTimesliceIdentificationInfo);
			}
		}

		public FeatureTimeSliceMetadata GetValue ()
		{
			var value = new FeatureTimeSliceMetadata ();

			value.DateStamp = ui_dateStampNDTP.GetValue ();
			value.MeasEquipClass = ui_measEquipClassTB.Text;
			value.DataIntegrity = (double?) ui_dataIntegrityNNud.GetValue ();
			value.HorizontalResolution = (double?) ui_horizontalResolutionNNud.GetValue ();
			value.VerticalResolution = (double?) ui_verticalResolutionNNud.GetValue ();
			value.MeasureClass = ui_measureClassCB.GetNullableEnumValue<MeasureClassCode> ();

			value.Contact = ui_responsiblePartyCont.GetValue ();

			value.DataQualityInfo.Clear ();
			var list = ui_DQNavCont.GetValue ();
			foreach (DataQuality dq in list)
			{
				if (!string.IsNullOrEmpty (dq.attributes) ||
					dq.report != null)
				{
					value.DataQualityInfo.Add (dq);
				}
			}

			value.FeatureTimesliceIdentificationInfo = ui_identificationCont.GetValue ();

			if (value.DateStamp == null &&
				string.IsNullOrEmpty (value.MeasEquipClass) &&
				value.DataIntegrity == null &&
				value.HorizontalResolution == null &&
				value.VerticalResolution == null &&
				value.MeasureClass == null &&
				value.Contact == null &&
				value.DataQualityInfo.Count == 0 &&
				value.FeatureTimesliceIdentificationInfo == null)
			{
				return null;
			}

			return value;
		}
	}
}
