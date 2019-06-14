using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IsolineToVS
{
    internal partial class IsolineToVSForm : Form
    {
        private IFormListener _listener;

        public IsolineToVSForm(IFormListener listener)
        {
            _listener = listener;

            InitializeComponent();
        }


        private void IsolineToVSForm_Load(object sender, EventArgs e)
        {
            FillLayers();
        }

        private void FillLayers()
        {
            ui_layersCB.Items.Clear();

            var layerNames = _listener.GetLayerNames();

            foreach (var item in layerNames)
                ui_layersCB.Items.Add(item);

            if (ui_layersCB.Items.Count > 0)
                ui_layersCB.SelectedIndex = 0;
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Generate_Click(object sender, EventArgs e)
        {
            bool isOk = false;
            string result = string.Empty;

            try
            {
                UseWaitCursor = true;
                Application.UseWaitCursor = true;
                Application.DoEvents();

                var layerName = ui_layersCB.SelectedItem as string;
                if (layerName == null)
                {
                    MessageBox.Show("Layer is not selected", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "XML File (*.xml)|*.xml";

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                result = _listener.GenerateMessage(
                    layerName,
                    saveFileDialog.FileName,
                    Convert.ToDouble(ui_horizontalAccuracyNud.Value),
                    ui_write3dChB.Checked,
                    out isOk);

            }
            catch(Exception ex)
            {
                result = "System Error:\n" + ex.Message;
            }
            finally
            {
                UseWaitCursor = false;
                Application.UseWaitCursor = false;
                Application.DoEvents();
            }

            if (!string.IsNullOrEmpty(result))
            {
                MessageBox.Show(
                    result,
                    Text,
                    MessageBoxButtons.OK,
                    (isOk ? MessageBoxIcon.Information : MessageBoxIcon.Error));
            }
        }

        private void RefreshLayerList_Click(object sender, EventArgs e)
        {
            FillLayers();
        }

        private void CurrentLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ui_generateButton.Enabled = (ui_layersCB.SelectedItem != null);

        }
    }
}
