using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Aran.Aim.InputForm
{
	public partial class DeltaProLoaderForm : Form
	{
		public DeltaProLoaderForm ()
		{
			InitializeComponent ();
		}


		public void SetData (DataTable dataTable)
		{
			ui_dgv.DataSource = dataTable;
		}

		public DataRow SelectedRow { get; private set; }

		
		private void Cancel_Click (object sender, EventArgs e)
		{
			SelectedRow = null;
			DialogResult = DialogResult.Cancel;
		}

		private void OK_Click (object sender, EventArgs e)
		{
			if (ui_dgv.CurrentRow == null)
				return;

			SelectedRow = (ui_dgv.CurrentRow.DataBoundItem as DataRowView).Row;
			DialogResult = DialogResult.OK;
		}


		internal static DataTable LoadDataTable ()
		{
			if (_deltaProParserObject == null)
				return null;

			var ofd = new OpenFileDialog ();
			ofd.Filter = "Microsoft Access Databases (*.mdb)|*.mdb|All Files|*.*";
			if (ofd.ShowDialog () != DialogResult.OK)
				return null;

			var mi = _deltaProParserObject.GetType ().GetMethod ("Load");
			if (mi != null)
			{
				var result = mi.Invoke (_deltaProParserObject, new object [] { ofd.FileName });
				return result as System.Data.DataTable;
			}

			return null;
		}

		internal static void LoadDeltaParserAssembly ()
		{
			try
			{
				var assm = Assembly.LoadFile (System.Windows.Forms.Application.StartupPath + "\\DeltaProParser.dll");
				var type = assm.GetType ("DeltaProParser.DesigningAreaLoader");
				_deltaProParserObject = Activator.CreateInstance (type);
			}
			catch (Exception ex)
			{
				Console.WriteLine ("Error: " + ex.Message);
			}
		}

		internal static object _deltaProParserObject;
		internal static bool _isDeltaProParserObjectLoaded = false;
	}
}
