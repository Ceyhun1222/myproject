using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using System.Drawing.Drawing2D;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Queries.Viewer;

namespace Aran.Queries.Common
{
    public partial class TimeSliceControl : UserControl
    {
        private int _showHeight = 300;
        private int _hideHeight = 50;
        private char _hideDetailsText = '▲';
        private char _showDetialsText = '▼';
        private bool _isExpanded = false;
        private Font _arialFont;
        private TimeSlice _timeSlice;
        private bool _readOnly;

        public TimeSliceControl ()
        {
            InitializeComponent ();

            Height = _hideHeight;

            _arialFont = new Font ("Arial", 12);
            //_readOnly = false;
        }

        public TimeSlice TimeSlice
        {
            get { return _timeSlice; }
            set
            {
                _timeSlice = value;

                AimPropInfo validTimePropInfo = null;
                AimPropInfo lifeTimePropInfo = null;
                AimPropInfo interpreatationPropInfo = null;
                AimPropInfo seqNumPropInfo = null;
                AimPropInfo corNumPropInfo = null;

                foreach (AimPropInfo propInfo in MetadataTimeSlice.PropInfoList)
                {
                    switch (propInfo.Index)
                    {
                        case (int) PropertyTimeSlice.ValidTime:
                            validTimePropInfo = propInfo;
                            break;
                        case (int) PropertyTimeSlice.Interpretation:
                            interpreatationPropInfo = propInfo;
                            break;
                        case (int) PropertyTimeSlice.SequenceNumber:
                            seqNumPropInfo = propInfo;
                            break;
                        case (int) PropertyTimeSlice.CorrectionNumber:
                            corNumPropInfo = propInfo;
                            break;
                        case (int) PropertyTimeSlice.FeatureLifetime:
                            lifeTimePropInfo = propInfo;
                            break;
                    }
                }

                pcInterpretation.PropertyTag = new PropControlTag (_timeSlice, interpreatationPropInfo);
                pcSequenceNumber.PropertyTag = new PropControlTag (_timeSlice, seqNumPropInfo);
                pcCorrectionNumber.PropertyTag = new PropControlTag (_timeSlice, corNumPropInfo);

                AimPropInfo beginPosPropInfo = null;
                AimPropInfo endPosPropInfo = null;

                foreach (AimPropInfo propInfo in MetadataTimePeriod.PropInfoList)
                {
                    switch (propInfo.Index)
                    {
                        case (int) PropertyTimePeriod.BeginPosition:
                            beginPosPropInfo = propInfo;
                            break;
                        case (int) PropertyTimePeriod.EndPosition:
                            endPosPropInfo = propInfo;
                            break;
                    }
                }

                if (_timeSlice.ValidTime != null)
                {
                    pcStartOfValidTime.PropertyTag = new PropControlTag (_timeSlice.ValidTime, beginPosPropInfo);
                    pcEndOfValidTime.PropertyTag = new PropControlTag (_timeSlice.ValidTime, endPosPropInfo);
                }

                if (_timeSlice.FeatureLifetime != null)
                {
                    pcStartOfLifeTime.PropertyTag = new PropControlTag (_timeSlice.FeatureLifetime, beginPosPropInfo);
                    pcEndOfLifeTime.PropertyTag = new PropControlTag (_timeSlice.FeatureLifetime, endPosPropInfo);
                }
            }
        }

        public bool ReadOnly
        {
            get { return _readOnly; }
            set
            {
                _readOnly = value;

                pcInterpretation.ReadOnly = value;
                //pcSequenceNumber.ReadOnly = true;
                //pcCorrectionNumber.ReadOnly = true;
                pcStartOfValidTime.ReadOnly = value;
                pcEndOfValidTime.ReadOnly = value;
                pcStartOfLifeTime.ReadOnly = value;
                pcEndOfLifeTime.ReadOnly = value;
            }
        }

        private void SetDataTimePickerValue (DateTimePicker dtp, DateTime? dateTime)
        {
            bool b = (dateTime != null);
            dtp.Visible = b;

            Control control = (Control) dtp.Tag;
            control.Location = dtp.Location;
            control.Size = dtp.Size;
            control.Visible = !b;

            if (b)
                dtp.Value = dateTime.Value;
        }

        private void titlePanel_Paint (object sender, PaintEventArgs e)
        {
            Control cont = (Control) sender;

            Graphics gr = e.Graphics;

            Color color1 = Color.FromArgb (235, 235, 235);
            Color color2 = SystemColors.Control;

            LinearGradientBrush linearBr = new LinearGradientBrush (
                new Point (0, 0),
                new Point (0, cont.Height),
                color1,
                color2);

            gr.FillRectangle (linearBr, new Rectangle (0, 0, cont.Width, cont.Height));

            gr.DrawString ("Time Slice", cont.Font, new SolidBrush (cont.ForeColor),
                new PointF (cont.Width / 2 - 30, 10));

            gr.DrawString ((_isExpanded ? _hideDetailsText : _showDetialsText).ToString (),
                _arialFont, new SolidBrush (cont.ForeColor),
                new PointF (cont.Width - 20, cont.Height / 2 - 10));

            gr.Dispose ();
        }

        private void mainGB_SizeChanged (object sender, EventArgs e)
        {
            showExpanderChb.Width = mainGB.Width - 7;
        }

        private void titlePanel_Click (object sender, EventArgs e)
        {
            _isExpanded = !_isExpanded;

            ShowHideExpander ();
        }

        private void ShowHideExpander ()
        {
            Height = (_isExpanded ? _showHeight : _hideHeight);
            detailPpanel.Visible = _isExpanded;
        }

        private void showExpanderChb_Paint (object sender, PaintEventArgs e)
        {
            Control cont = (Control) sender;

            Graphics gr = e.Graphics;

            Color color1 = Color.FromArgb (235, 235, 235);
            Color color2 = SystemColors.Control;

            LinearGradientBrush linearBr = new LinearGradientBrush (
                new Point (0, 0),
                new Point (0, cont.Height),
                color1,
                color2);

            gr.FillRectangle (linearBr, new Rectangle (0, 0, cont.Width, cont.Height));

            gr.DrawString ("Time Slice", cont.Font, new SolidBrush (cont.ForeColor),
                new PointF (cont.Width / 2 - 30, 10));

            gr.DrawString ((_isExpanded ? _hideDetailsText : _showDetialsText).ToString (),
                _arialFont, new SolidBrush (SystemColors.ControlDark),
                new PointF (cont.Width - 20, cont.Height / 2 - 10));

            if (cont.Focused)
            {
                gr.DrawRectangle (new Pen (SystemColors.ActiveBorder, 1),
                    new Rectangle (1, 1, cont.Width - 2, cont.Height - 2));
            }
        }

        private void showExpanderChb_CheckedChanged (object sender, EventArgs e)
        {
            _isExpanded = showExpanderChb.Checked;
            ShowHideExpander ();
            showExpanderChb.Refresh ();
        }

        private void TimeSliceControl_Load (object sender, EventArgs e)
        {
            showExpanderChb_CheckedChanged (showExpanderChb, null);
        }
    }
}
