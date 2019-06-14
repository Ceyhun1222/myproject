using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aran.Panda.VisualManoeuvring.Forms
{
    public partial class SegmentDetailsControl : UserControl
    {
        private String name;
        public String Name
        {
            get { return name; }
            set
            {
                name = value;
                textBox1.Text = value;
            }
        }

        private double length;
        public double Length
        {
            get { return length; }
            set
            {
                length = value;
                textBox2.Text = value.ToString();
            }
        }

        private double initialDirection;
        public Double InitialDirection
        {
            get { return initialDirection; }
            set
            {
                initialDirection = value;
                textBox3.Text = value.ToString();
            }
        }

        private double intermediateDirection;
        public Double IntermediateDirection
        {
            get
            {
                return intermediateDirection;
            }
            set
            {
                intermediateDirection = value;
                textBox4.Text = value.ToString();
            }
        }

        private double finalDirection;
        public Double FinalDirection
        {
            get { return finalDirection; }
            set
            {
                finalDirection = value;
                textBox5.Text = value.ToString();
            }
        }

        private double flightAltitude;

        public double FlightAltitude
        {
            get { return flightAltitude; }
            set
            {
                flightAltitude = value;
                textBox6.Text = value.ToString();
            }
        }

        private String description;
        public String Description
        {
            get { return description; }
            set
            {
                description = value;
                txtBox_description.Text = value;
            }
        }

        public SegmentDetailsControl()
        {
            InitializeComponent();
        }

        private bool _isMoreMode;

        public bool IsMoreMode
        {
            get { return _isMoreMode; }
            set
            {
                _isMoreMode = value;
                Height = value ? 150 : 65;

                txtBox_description.Multiline = value;
                txtBox_description.Height = value ? 80 : 20;
                txtBox_description.Dock = value ? DockStyle.Bottom : DockStyle.None;
            }
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (Parent != null)
            {
                Parent.ControlAdded += Parent_ControlChanged;
                Parent.ControlRemoved += Parent_ControlChanged;
            }
        }

        void Parent_ControlChanged(object sender, ControlEventArgs e)
        {
            if (Parent == null)
                return;
            var isLast = (Parent.Controls.IndexOf(this) == Parent.Controls.Count - 1);
            //btn_edit.Visible = isLast; //uncomment this if necessary
        }

        private void btn_more_Click(object sender, EventArgs e)
        {
            IsMoreMode = !IsMoreMode;
            btn_more.Text = IsMoreMode ? "Less" : "More";
        }
    }
}
