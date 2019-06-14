using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KFileParser
{
	public partial class FormFolderSelection : Form
	{
		public FormFolderSelection ( )
		{
			InitializeComponent ( );
		}

		private void btnOk_Click ( object sender, EventArgs e )
		{
			if ( radioButton1.Checked )
				SelectedFolder = radioButton1.Text;
			else if ( radioButton2.Checked )
				SelectedFolder = radioButton2.Text;
			else if ( radioButton3.Checked )
				SelectedFolder = radioButton3.Text;
			else if ( radioButton4.Checked )
				SelectedFolder = radioButton4.Text;
			else if ( radBtnNavaid.Checked )
				SelectedFolder = radBtnNavaid.Text;			
			DialogResult = System.Windows.Forms.DialogResult.OK;			
		}

		private void btnCancel_Click ( object sender, EventArgs e )
		{
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}

		public string SelectedFolder
		{
			get;
			set;
		}
	}
}
