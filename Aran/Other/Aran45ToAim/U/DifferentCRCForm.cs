using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AIXM45_AIM_UTIL;
using System.IO;

namespace Aran45ToAixm.U
{
	public partial class DifferentCRCForm : Form
	{
		public DifferentCRCForm ()
		{
			InitializeComponent ();
		}

		public DialogResult ShowDiffCRC (Dictionary<string, List<ConvertedObj>> dict)
		{
			foreach (var dictItem in dict)
			{
				var rowIndex = ui_dgv.Rows.Add (string.Format ("{0} ({1})",dictItem.Key, dictItem.Value.Count));
				var row = ui_dgv.Rows [rowIndex];
				row.DefaultCellStyle = GetGroupLabelStyle ();
				row.Height = 30;
				row.Tag = true;

				foreach (var convObj in dictItem.Value)
				{
					ui_dgv.Rows.Add (convObj.mid, convObj.CRCInfo.Name, convObj.CRCInfo.SourceCRC, convObj.CRCInfo.NewCRC);
				}
			}

			return ShowDialog ();
		}

		private DataGridViewCellStyle GetGroupLabelStyle ()
		{
			var style = new DataGridViewCellStyle ();
			style.BackColor = Color.LightGray;
			style.SelectionBackColor = style.BackColor;
			style.SelectionForeColor = ui_dgv.DefaultCellStyle.ForeColor;
			style.Font = new Font (ui_dgv.Font, FontStyle.Bold);
			return style;
		}

		private void Continue_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		private void Stop_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void SaveReport_Click (object sender, EventArgs e)
		{
			var sfd = new SaveFileDialog ();
			sfd.Filter = "Text File (*.txt)|*.txt";
			if (sfd.ShowDialog () != DialogResult.OK)
				return;

			var fs = new FileStream (sfd.FileName, FileMode.Create, FileAccess.Write);
			var sw = new StreamWriter (fs);
			var newLine = string.Empty;

			foreach (DataGridViewRow row in ui_dgv.Rows)
			{
				string s = string.Empty;
				// If row is group name.
				if (true.Equals (row.Tag))
				{
					s = string.Format ("{0}*** {1} ***", newLine, row.Cells [0].Value);

					if (newLine == string.Empty)
						newLine = "\r\n";
				}
				else
				{
					s += row.Cells [0].Value;
					for (int i = 1; i < row.Cells.Count; i++)
						s += "\t\t" + row.Cells [i].Value;
				}

				sw.WriteLine (s);
			}

			newLine = "\r\n";
			sw.Close ();
			fs.Close ();
		}
	}
}
