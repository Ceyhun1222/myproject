using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Metadata.UI;

namespace Aran.Aim.InputFormLib
{
    public partial class NonExistingReferenceForm : Form
    {
        private List<Utilities.RefFeatureProp> _refFeaturePropList;

        public NonExistingReferenceForm ()
        {
            InitializeComponent ();
        }

        public void SetFeatures (List<Utilities.RefFeatureProp> refFeaturePropList)
        {
            _refFeaturePropList = refFeaturePropList;
        }


        private void Form_Load (object sender, EventArgs e)
        {
            foreach (var item in _refFeaturePropList)
            {
                int rowIndex = -1;
                rowIndex = ui_dgv.Rows.Add ();
                var row = ui_dgv.Rows [rowIndex];
                row.Tag = item;
                row.Cells [0].Value = UIUtilities.GetFeatureDescription (item.Feature);
            }

            if (ui_dgv.Rows.Count > 0)
                ui_dgv.CurrentCell = ui_dgv.Rows [0].Cells [0];

            DGV_CurrentCellChanged (null, null);
        }

        private void DGV_CellContentClick (object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex == 1)
            {
                var row = ui_dgv.Rows [e.RowIndex];
                var rpf = row.Tag as Utilities.RefFeatureProp;

                var featList = new List<Aran.Aim.Features.Feature> ();
                featList.Add (rpf.Feature);

                var fv = new Aran.Aim.FeatureInfo.ROFeatureViewer ();
                fv.SetOwner (this);
                fv.ShowFeaturesForm (featList);
            }
        }

        private void DGV_CurrentCellChanged (object sender, EventArgs e)
        {
            ui_propInfoLabel.Text = string.Empty;

            if (ui_dgv.CurrentRow == null)
                return;

            var rpf = ui_dgv.CurrentRow.Tag as Utilities.RefFeatureProp;
            if (rpf == null || rpf.PropInfoList.Count == 0)
                return;

            var s = rpf.PropInfoList [0].Name;
            for (int i = 1; i < rpf.PropInfoList.Count; i++)
                s += " -> " + rpf.PropInfoList [i].Name;

            ui_propInfoLabel.Text = s;
        }

        private void CloseButton_Click (object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void CleanAndContinueButton_Click (object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void SaveLog_Click(object sender, EventArgs e)
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
                sw.WriteLine("    <th>Feature</th>");
                sw.WriteLine("    <th>Property</th>");
                sw.WriteLine("    <th>Reference</th>");
                sw.WriteLine("  </tr>");


                foreach (var item in _refFeaturePropList)
                {
                    var prNames = item.PropInfoList.Select(pi => pi.AixmName);
                    var propName = string.Join<string>("/", prNames);

                    sw.WriteLine("<tr>");
                    sw.WriteLine(string.Format("<td>{0} {1}</td>", item.Feature.FeatureType, item.Feature.Identifier));
                    sw.WriteLine("<td>" + propName + "</td>");
                    sw.WriteLine(string.Format("<td>{0}  {1}</td>", item.PropInfoList.Last().ReferenceFeature, item.RefIdentifier));
                    sw.WriteLine("</tr>");
                }

                sw.WriteLine("</table>");
                sw.WriteLine("</body>");
                sw.WriteLine("</html>");

                sw.Close();
            }
        }
    }
}
