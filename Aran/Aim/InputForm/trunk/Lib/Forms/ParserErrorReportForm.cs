using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.AixmMessage;

namespace Aran.Aim.InputFormLib
{
    public partial class ParserErrorReportForm : Form
    {
        public ParserErrorReportForm()
        {
            InitializeComponent();

            ErrorInfoList = new List<DeserializedErrorInfo>();

            var type = typeof(DeserializedErrorInfo);
            foreach (DataGridViewColumn col in ui_dgv.Columns)
                col.Tag = type.GetProperty(col.DataPropertyName);

            ui_dgv.CellValueNeeded += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.RowIndex < ErrorInfoList.Count)
                {
                    var errorInfo = ErrorInfoList[e.RowIndex];
                    var propInfo = ui_dgv.Columns[e.ColumnIndex].Tag as PropertyInfo;
                    e.Value = propInfo.GetValue(errorInfo, null);
                }
            };

            Load += (s, e) => RefreshGrid();
        }

        public List<DeserializedErrorInfo> ErrorInfoList { get; private set; }

        public void RefreshGrid()
        {
            ui_dgv.RowCount = ErrorInfoList.Count;
            ui_dgv.Refresh();
        }

        private void DGV_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            e.ContextMenuStrip = ui_contextMS;
        }

        private void CopyTSMI_Clic(object sender, EventArgs e)
        {
            if (ui_dgv.CurrentCell == null || ui_dgv.CurrentCell.Value == null)
                return;

            Clipboard.SetText(ui_dgv.CurrentCell.Value.ToString());
        }

        private void ShowXmlPartTSMI_Click(object sender, EventArgs e)
        {
            if (ui_dgv.CurrentCell == null)
                return;

            var errorInfo = ErrorInfoList[ui_dgv.CurrentCell.RowIndex];
            ShowXmlInForm(errorInfo.XmlMessage);
        }

        private void ShowXmlInForm(string xmlMessage)
        {
            var form = new Form();
            form.StartPosition = FormStartPosition.CenterParent;
            form.Size = new Size(500, 500);
            form.ShowInTaskbar = false;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.FormBorderStyle = FormBorderStyle.SizableToolWindow;

            var textBox = new TextBox();
            textBox.ScrollBars = ScrollBars.Both;
            textBox.WordWrap = false;
            textBox.Multiline = true;
            textBox.Font = new Font(textBox.Font.FontFamily, 12, FontStyle.Regular);
            textBox.Dock = DockStyle.Fill;
            form.Controls.Add(textBox);
            
            if (xmlMessage != null)
                textBox.Text = xmlMessage;

            form.ShowDialog(this);
        }

        private void StopImport_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "HTML (*.html)|*.html|All Files (*.*)|(*.*)";

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            var fileName = sfd.FileName;
            using (var sw = File.CreateText(sfd.FileName))
            {
                sw.WriteLine("<!DOCTYPE html>");
                sw.WriteLine("<html>");
                sw.WriteLine("<head>");
                sw.WriteLine("<style>");
                sw.WriteLine("table, th, td {");
                sw.WriteLine("    border: 1px solid black;");
                sw.WriteLine("    border-collapse: collapse;");
                sw.WriteLine("}");
                sw.WriteLine("th, td {");
                sw.WriteLine("    padding: 5px;");
                sw.WriteLine("}");
                sw.WriteLine("</style>");
                sw.WriteLine("</head>");
                sw.WriteLine("<body>");
                sw.WriteLine("");
                sw.WriteLine("<table style=\"width:100%\">");
                sw.WriteLine("  <tr>");
                sw.WriteLine("    <th>FeatureType</th>");
                sw.WriteLine("    <th>Identifier</th>");
                sw.WriteLine("    <th>Property Name</th>");
                sw.WriteLine("    <th>Message</th>");
                sw.WriteLine("    <th>Action</th>");
                sw.WriteLine("  </tr>");


                foreach (var item in ErrorInfoList)
                {
                    var row = new object[] { item.FeatureType, item.Identifier, item.PropertyName, item.ErrorMessage, item.Action };

                    sw.WriteLine("<tr>");
                    foreach (var cell in row)
                        sw.WriteLine("<td>" + cell + "</td>");
                    sw.WriteLine("</tr>");
                }

                sw.WriteLine("</table>");
                sw.WriteLine("</body>");
                sw.WriteLine("</html>");

                sw.Close();
            }
        }

        private void Continue_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
