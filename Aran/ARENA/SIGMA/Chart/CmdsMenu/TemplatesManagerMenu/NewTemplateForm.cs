using ArenaStatic;
using ESRI.ArcGIS.Carto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SigmaChart.CmdsMenu.TemplatesManagerMenu
{
    public partial class NewTemplateForm : Form
    {
        public string _FolderName;
        public SigmaChartTypes chrtTp;
        public string FileName;
        public NewTemplateForm()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            axToolbarControl1.SetBuddyControl(axPageLayoutControl1);
        }

        private void wizardTabControl1_TabIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            wizardTabControl1.SelectedIndex++;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            wizardTabControl1.SelectedIndex--;

        }

        private void wizardTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Prev_button.Enabled = wizardTabControl1.SelectedIndex != 0;
            Next_button.Enabled = wizardTabControl1.SelectedIndex != wizardTabControl1.TabCount - 1;

            button4.Enabled = wizardTabControl1.SelectedIndex == wizardTabControl1.TabCount - 1;
            button5.Enabled = wizardTabControl1.SelectedIndex == wizardTabControl1.TabCount - 1;



            if (wizardTabControl1.SelectedIndex == wizardTabControl1.TabCount - 1)
            {
                _FolderName = Path.Combine(_FolderName, TemplateListComboBox.Text);




                axPageLayoutControl1.LoadMxFile(_FolderName);
                Application.DoEvents();

                ((IActiveView)axPageLayoutControl1.ActiveView).GraphicsContainer.Reset();
                IElement element = ((IActiveView)axPageLayoutControl1.ActiveView).GraphicsContainer.Next();
                while (element != null)
                {
                    IElementProperties3 elprop = (IElementProperties3)element;

                    int i = 0;
                    foreach (var item in checkedListBox1.Items)
                    {
                        if(item.ToString().CompareTo(elprop.Name)==0)
                        {
                            checkedListBox1.SetItemChecked(i, true);
                            break;
                        }
                        i++;
                    }
                    element = ((IActiveView)axPageLayoutControl1.ActiveView).GraphicsContainer.Next();
                }


                //IMap pMap = ((IActiveView)axPageLayoutControl1.ActiveView).FocusMap;
                //int mDesc = Convert.ToInt32( pMap.Description);

                //chartTp = (SigmaChartTypes)mDesc;


            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void NewTemplateForm_Load(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TemplateListComboBox.Items.Clear();
            _FolderName = ArenaStaticProc.GetPathToTemplate() + @"\SID\";
            chrtTp = (SigmaChartTypes)comboBox1.SelectedIndex + 1;

            switch (chrtTp)
            {
                case SigmaChartTypes.EnrouteChart_Type:
                    _FolderName = ArenaStaticProc.GetPathToTemplate() + @"\Enroute\";
                    break;
                case SigmaChartTypes.SIDChart_Type:
                    _FolderName = ArenaStaticProc.GetPathToTemplate() + @"\SID\";
                    break;
                case SigmaChartTypes.ChartTypeA:
                    _FolderName = ArenaStaticProc.GetPathToTemplate() + @"\ChartTypeA\";
                    break;
                case SigmaChartTypes.STARChart_Type:
                    _FolderName = ArenaStaticProc.GetPathToTemplate() + @"\STAR\";
                    break;
                case SigmaChartTypes.IAPChart_Type:
                    _FolderName = ArenaStaticProc.GetPathToTemplate() + @"\IAP\";
                    break;
                case SigmaChartTypes.PATChart_Type:
                    _FolderName = ArenaStaticProc.GetPathToTemplate() + @"\PATC\";
                    break;
                case SigmaChartTypes.AreaChart:
                    _FolderName = ArenaStaticProc.GetPathToTemplate() + @"\AreaChart\";
                    break;
                case SigmaChartTypes.AerodromeElectronicChart:
                    _FolderName = ArenaStaticProc.GetPathToTemplate() + @"\AerodromeElectronicChart\";
                    break;
                case SigmaChartTypes.AerodromeParkingDockingChart:
                    _FolderName = ArenaStaticProc.GetPathToTemplate() + @"\AerodromeParkingDockingChart\";
                    break;
                case SigmaChartTypes.AerodromeGroundMovementChart:
                    _FolderName = ArenaStaticProc.GetPathToTemplate() + @"\AerodromeGroundMovementChart\";
                    break;
                case SigmaChartTypes.AerodromeBirdChart:
                    _FolderName = ArenaStaticProc.GetPathToTemplate() + @"\AerodromeBirdChart\";
                    break;
                case SigmaChartTypes.AerodromeChart:
                    _FolderName = ArenaStaticProc.GetPathToTemplate() + @"\AerodromeChart\";
                    break;
                case SigmaChartTypes.MinimumAltitudeChart:
                    _FolderName = ArenaStaticProc.GetPathToTemplate() + @"\MinimumAltitudeChart\";
                    break;
                case SigmaChartTypes.None:
                default:
                    break;
            }


            string[] FN = Directory.GetFiles(_FolderName, "*.mxd");
            foreach (var fl in FN)
            {
                TemplateListComboBox.Items.Add(System.IO.Path.GetFileName(fl));
            }


            if (TemplateListComboBox.Items.Count > 0)
            {
                TemplateListComboBox.SelectedIndex = 0;
                //_FolderName = ArenaStaticProc.GetPathToMapFolder();
            }
            

        }




        private void button4_Click(object sender, EventArgs e)
        {
            fileNameForm frm = new fileNameForm();
            frm.textBox1.Text = chrtTp.ToString() + "_SigmaTemplate.mxd";
            frm.ShowDialog();
            FileName = frm.textBox1.Text;
        }

       
        private void AddTemplate(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = _FolderName;
            openFileDialog1.Filter = "mxd files (*.mxd)|*.mxd";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.Copy(openFileDialog1.FileName, System.IO.Path.Combine(_FolderName, System.IO.Path.GetFileName(openFileDialog1.FileName)), true);
                    comboBox1_SelectedIndexChanged(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
