using ArenaStatic;
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
using System.Windows.Interop;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using SigmaChart.CmdsMenu;

namespace ChartTypeA.View
{
    public partial class ChartTypeAWizardForm : Form
    {

        public string _FolderName;
        public string _ProjectName;
        public string _TemplatetName;
       // public double _Scale;
        public double mapSize_Height;
        public double mapSize_Width;
        public int Level =0;

        public ChartTypeAWizardForm()
        {
            
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Desktop);

            InitializeComponent();

            _FolderName = "";
            TemplateListComboBox.Items.Clear();
            var tmp = ArenaStaticProc.GetPathToTemplate() + @"\ChartTypeA\";

           

            string[] FN = Directory.GetFiles(tmp, "*.mxd");
            foreach (var fl in FN)
            {

                TemplateListComboBox.Items.Add(System.IO.Path.GetFileName(fl));
            }

            TemplateListComboBox.SelectedIndex = 0;
            _FolderName = ArenaStaticProc.GetPathToMapFolder();

            textBox2.Text = _FolderName;

            var acDate =GlobalParams.DbModule.AirportHeliportList.
                                                Select(x => x.ActualDate).
                                                Max();
            int _AiracCircle = AiracUtil.AiracUtil.GetAiracCycleByDate(acDate);
            airacControl1.AiracCircleValue = _AiracCircle;

            AiracDate = acDate;
            //AnnotationScaleComboBox.SelectedIndex = 7;
        }

        public DateTime AiracDate { get; private set; }

        private void button2_Click(object sender, EventArgs e)
        {
            _ProjectName = textBox1.Text;
            _FolderName = textBox2.Text;
            _TemplatetName = TemplateListComboBox.Text;
           // Double.TryParse(AnnotationScaleComboBox.Text, out _Scale);

            ArenaStaticProc.SetPathToMapFolder(_FolderName);
        }

        private void button2_Validated(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.Length <= 0 ? "NewChart" : textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var folderBrowserDialog1 = new FolderBrowserDialog
                                           {
                                               ShowNewFolderButton = true
                                           };
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CalendarForm frm = new CalendarForm();

            var win32Window = new Win32Windows(this.Handle.ToInt32());
            if (frm.ShowDialog(win32Window) == DialogResult.OK)
            {
                airacControl1.AiracCircleValue = AiracUtil.AiracUtil.GetAiracCycleByDate(frm.selectedDate);
                AiracDate = frm.selectedDate;
            }
        }

        private void axPageLayoutControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IPageLayoutControlEvents_OnMouseDownEvent e)
        {

        }

        private void TemplateListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChartTypeA.ChartHelperClass.SelectChartTemplate(ref axPageLayoutControl1, ArenaStaticProc.GetPathToTemplate() + @"\ChartTypeA\" + TemplateListComboBox.Text, ref listBox1, out mapSize_Height, out mapSize_Width);
        }

    }
}
