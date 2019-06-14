using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aran.Aim.InputFormLib
{
    public partial class AiracSelectorForm : Form
    {
        public AiracSelectorForm()
        {
            InitializeComponent();

            ui_beginValidDateTimePicker.SetNextCycle();
            SetFeatureCount(0);
        }

        public DateTime BeginValidTimeDateTime
        {
            get { return ui_beginValidDateTimePicker.Value; }
            set { ui_beginValidDateTimePicker.Value = value; }
        }

        public void SetFeatureCount(int featCount)
        {
            ui_featureCountLabel.Text = string.Format("Feature Count: {0}\nDo your want to continue?", featCount);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
