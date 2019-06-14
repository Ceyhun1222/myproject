using System;
using System.Windows.Forms;

namespace Aran.AimEnvironment.Tools
{
    public partial class MeasureAngleToolForm : Form
    {

        public event EventHandler Clicked;

        public MeasureAngleToolForm()
        {
            InitializeComponent();

        }

        public ToolStrip Toolbar { get { return measureAngleToolstrip; } }

        private void measureAngleToolStripButtonClick(object sender, EventArgs e)
        {
            if (Clicked != null)
                Clicked(sender, e);
        }
    }
}
