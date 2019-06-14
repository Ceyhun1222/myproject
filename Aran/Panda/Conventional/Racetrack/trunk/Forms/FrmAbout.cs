using System;
using System.Windows.Forms;

namespace Aran.PANDA.Conventional.Racetrack.Forms
{
	public partial class FormAbout : Form
	{
		public FormAbout ( )
		{
			InitializeComponent ( );
        }

		private void FormAbout_FormClosing ( object sender, FormClosingEventArgs e )
		{
			Hide ( );
			e.Cancel = true;
		}

        private void FormAbout_Load(object sender, EventArgs e)
        {
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = $"{version.Major}.{version.Minor}.{version.Build}";
            lblBuildDate.Text = DateTime.Today.ToShortDateString();
            lblYear.Text = DateTime.Today.Year.ToString();
        }
	}
}
