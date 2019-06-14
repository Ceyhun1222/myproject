using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aran.Queries.Common
{
    public partial class CoordinateControl : UserControl
    {
        private bool _readOnly;

        public event EventHandler ValueChanged;

        public CoordinateControl ()
        {
            InitializeComponent ();

            _readOnly = false;
        }

        public bool IsDD
        {
            get { return ui_xDDMS.IsDD; }
            set
            {
                ui_xDDMS.IsDD = value;
                ui_yDDMS.IsDD = value;
            }
        }

        public int DDAccuracy
        {
            get { return ui_xDDMS.DDAccuracy; }
            set
            {
                ui_xDDMS.DDAccuracy = value;
                ui_yDDMS.DDAccuracy = value;
            }
        }

        public int DMSAccuracy
        {
            get { return ui_xDDMS.DMSAccuracy; }
            set
            {
                ui_xDDMS.DMSAccuracy = value;
                ui_yDDMS.DMSAccuracy = value;
            }
        }

        public int Accuracy
        {
            get;
            set;
        }

        public bool ReadOnly
        {
            get { return _readOnly; }
            set
            {
                if (_readOnly == value)
                    return;

                _readOnly = value;

                ui_xDDMS.Enabled = !value;
                ui_yDDMS.Enabled = !value;
            }
        }

        public void SetValue (Aran.Geometries.Point point)
        {
            if (!point.IsEmpty)
            {
                ui_xDDMS.Value = point.X;
                ui_yDDMS.Value = point.Y;
            }
        }

        public Aran.Geometries.Point GetValue ()
        {
            return new Aran.Geometries.Point (ui_xDDMS.Value, ui_yDDMS.Value);
        }

        private void DDMS_ValueChanged (object sender, EventArgs e)
        {
            if (_readOnly || ValueChanged == null)
                return;
            
            ValueChanged (this, e);
        }
    }
}
