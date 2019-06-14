using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;
using Aran.Controls.Airac;
using Aran.AranEnvironment;

namespace Aran.Queries.Common
{
    public partial class TimeSliceControl : UserControl
    {
        private TimeSlice _timeSlice;


        public TimeSliceControl()
        {
            InitializeComponent();

            //foreach (var item in Enum.GetValues(typeof(TimeSliceInterpretationType)))
                
            ui_interpretationCB.Items.Add(TimeSliceInterpretationType.BASELINE);
            ui_interpretationCB.SelectedIndex = 0;
        }


        public TimeSlice TimeSlice
        {
            get { return _timeSlice; }
            set
            {
                _timeSlice = value;

                ui_seqNumTB.Text = _timeSlice.SequenceNumber.ToString();
                ui_corNumTB.Text = _timeSlice.CorrectionNumber.ToString();
                ui_beginValidAiracCycle.AiracDateTime = AiracCycle.CreateAiracDateTime(_timeSlice.ValidTime.BeginPosition);

                ui_endOfValidPanel.Visible = SetDateTimeValue(ui_endValidTB, value.ValidTime.EndPosition);
                if (value.FeatureLifetime != null)
                {
                    SetDateTimeValue(ui_beginLifeTB, value.FeatureLifetime.BeginPosition);
                    ui_endOfLife.Visible = SetDateTimeValue(ui_endLifeTB, value.FeatureLifetime.EndPosition);
                }
            }
        }

        public bool ReadOnly
        {
            get { return !ui_beginValidAiracCycle.Enabled; }
            set { ui_beginValidAiracCycle.Enabled = !value; }
        }

        public bool IsNewFeature { get; set; }

        public bool AsCorrection { get; set; }


        private void BeginValidAiracCycle_ValueChanged(object sender, EventArgs e)
        {
            var dt = ui_beginValidAiracCycle.Value;

            if (_timeSlice.ValidTime.BeginPosition == dt)
                return;

            if (IsNewFeature || (AsCorrection && _timeSlice.SequenceNumber == 1)) {

                _timeSlice.ValidTime.BeginPosition = dt;
                _timeSlice.FeatureLifetime.BeginPosition = dt;
                SetDateTimeValue(ui_beginLifeTB, dt);
            }
            else {
                if (dt < (_timeSlice.FeatureLifetime.BeginPosition.AddSeconds(-1))) {
                    MessageBox.Show("Begin of valid time cannot be before begin of Lifetime", "ADM", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
					ui_beginValidAiracCycle.Value = _timeSlice.FeatureLifetime.BeginPosition;
                    return;
                }

                _timeSlice.ValidTime.BeginPosition = dt;
            }
        }

        private bool SetDateTimeValue(TextBox textBox, DateTime? dateTime)
        {
            if (dateTime == null) {
                textBox.Text = "";
                return false;
            }
            else {
                var adt = AiracCycle.CreateAiracDateTime(dateTime.Value);
                if (adt.Mode == AiracSelectionMode.Custom)
                    textBox.Text = string.Format("Custom: {0:yyyy - MM - dd HH:mm}  UTC", adt.Value);
                else
                    textBox.Text = string.Format("AIRAC: {0:yyyy - MM - dd}", adt.Value);

                return true;
            }
        }

    }
}
