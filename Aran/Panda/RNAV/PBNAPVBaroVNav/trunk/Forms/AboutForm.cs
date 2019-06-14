using System;
using System.Reflection;
using System.Windows.Forms;

namespace Aran.PANDA.RNAV.PBNAPVBaroVNav
{
	public partial class AboutForm : Form
	{
		public AboutForm()
		{
			InitializeComponent();

			Assembly currentAssem = Assembly.GetExecutingAssembly();

			lbllVersion.Text = "Version :  " + currentAssem.GetName().Version.ToString();
			lblVersionDate.Text = "Build date : " + Functions.RetrieveLinkerTimestamp().ToString("MM.dd.yyyy");

			AssemblyTitleAttribute titleAttrib = (AssemblyTitleAttribute)currentAssem.GetCustomAttribute(typeof(AssemblyTitleAttribute));
			this.Text = "About " + titleAttrib.Title;

			AssemblyDescriptionAttribute descAttrib = (AssemblyDescriptionAttribute)currentAssem.GetCustomAttribute(typeof(AssemblyDescriptionAttribute));
			lblDescription.Text = descAttrib.Description;

			AssemblyCopyrightAttribute copyrightAttrib = (AssemblyCopyrightAttribute)currentAssem.GetCustomAttribute(typeof(AssemblyCopyrightAttribute));
			lblCopyRight.Text = copyrightAttrib.Copyright;
		}

		private void Button1_Click(object sender, System.EventArgs e)
		{
			Close();
		}
	}
}
