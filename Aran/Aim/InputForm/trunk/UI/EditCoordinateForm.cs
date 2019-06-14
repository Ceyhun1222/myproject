using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aran.Aim.InputForm
{
    public partial class EditCoordinateForm : Form
    {
        public EditCoordinateForm ()
        {
            InitializeComponent ();
        }

        public bool IsDD
        {
            get { return ui_coordinateControl.IsDD; }
            set { ui_coordinateControl.IsDD = value; }
        }

        public int Accuracy
        {
            get { return ui_coordinateControl.Accuracy; }
            set { ui_coordinateControl.Accuracy = value; }
        }

        public void SetPoint (Geometries.Point value)
        {
            ui_coordinateControl.SetPoint (value);
        }

        public Geometries.Point GetPoint ()
        {
            return ui_coordinateControl.GetPoint ();
        }

        private void OK_Click (object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
