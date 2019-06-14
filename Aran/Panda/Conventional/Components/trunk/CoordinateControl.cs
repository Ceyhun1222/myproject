using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ChoosePointNS
{
    public partial class CoordinateControl : UserControl
    {
        public event EventHandler PointChanged;

        private bool _captionVisible;

        public CoordinateControl ()
        {
            InitializeComponent ();

            _captionVisible = true;
        }

        public void SetPoint (Aran.Geometries.Point value)
        {
            ui_lonDDMS.Value = value.X;
            ui_latDDMS.Value = value.Y;
        }

        public bool CaptionVisible
        {
            get { return _captionVisible; }
            set
            {
                if (_captionVisible == value)
                    return;

                _captionVisible = value;
                ui_textPanel.Visible = value;
            }
        }

        public bool IsDD
        {
            get { return ui_latDDMS.IsDD; }
            set
            {
                if (ui_latDDMS.IsDD == value)
                    return;

                var tmp = Accuracy;

                ui_latDDMS.IsDD = value;
                ui_lonDDMS.IsDD = value;

                Accuracy = tmp;
            }
        }

        public int Accuracy
        {
            get
            {
                if (IsDD)
                    return ui_latDDMS.DDAccuracy;
                return ui_latDDMS.DMSAccuracy;
            }
            set
            {
                if (IsDD)
                {
                    ui_latDDMS.DDAccuracy = value;
                    ui_lonDDMS.DDAccuracy = value;
                }
                else
                {
                    ui_latDDMS.DMSAccuracy = value;
                    ui_lonDDMS.DMSAccuracy = value;
                }
            }
        }

        public bool ReadOnly
        {
            get { return ui_latDDMS.ReadOnly; }
            set
            {
                ui_latDDMS.ReadOnly = value;
                ui_lonDDMS.ReadOnly = value;
            }
        }

        public Aran.Geometries.Point GetPoint ()
        {
            return new Aran.Geometries.Point (ui_lonDDMS.Value, ui_latDDMS.Value);
        }

        public double Latitude
        {
            get { return ui_latDDMS.Value; }
            set
            {
                ui_latDDMS.Value = value;
            }
        }


        public double Longtitude
        {
            get { return ui_lonDDMS.Value; }
            set 
            {
                ui_lonDDMS.Value = value;
            }
        }

        public void LatChanged()
        {
            if (LatitudeChanged != null)
                LatitudeChanged(ui_latDDMS.Value, null);
        }

        public void LongChanged()
        {
            if (LongtitudeChanged != null)
                LongtitudeChanged(ui_lonDDMS.Value, null);
        }

        private void LatLon_ValueChanged (object sender, EventArgs e)
        {
            if (PointChanged != null)
            {
                PointChanged (this, e);
            }
            if (sender == ui_latDDMS)
            {
                if (LatitudeChanged != null)
                    LatitudeChanged(ui_latDDMS.Value, null);
            }
            else
            {
                if (LongtitudeChanged != null)
                    LongtitudeChanged(ui_lonDDMS.Value, null);
            }
        }

        public override void Refresh()
        {
            ui_latDDMS.Reset();
            ui_lonDDMS.Reset();
        }

        public event EventHandler LatitudeChanged;
        public event EventHandler LongtitudeChanged;
    
    }
}
