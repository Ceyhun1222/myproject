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
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using EsriWorkEnvironment;
using ESRI.ArcGIS.DataSourcesGDB;
using PDM;
using ARENA;

namespace SigmaChart.CmdsMenu
{
    public partial class EnroutWizardForm : Form
    {

        public string _FolderName;
        public string _ProjectName;
        public string _TemplatetName;
       // public double _Scale;
        public double mapSize_Height;
        public double mapSize_Width;
        public int Level =0;
        public bool _AllVOR_DMEflag = true;
        private IFeatureLayer2 _Layer;
        public int _AiracCircle;
        public IAOIBookmark _bookmark;
        public double _mapScale;

        public UOM_DIST_VERT vertUom = UOM_DIST_VERT.FT;
        public UOM_DIST_HORZ distUom = UOM_DIST_HORZ.NM;

        private List<RadioCommunicationChanel> cnlList = null;
        public List<string> _selectedChanels = null;
        public int _upperSeparation = 999;
        public int _lowerSeparation = 0;
        public int arspBufWidth = 2;
        public bool arspSign = false;
        public bool magBearing = true;

        public EnroutWizardForm()
        {
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Desktop);

            InitializeComponent();


            _FolderName = "";
            TemplateListComboBox.Items.Clear();
            var tmp = ArenaStaticProc.GetPathToTemplate() + @"\Enroute\";

           

            string[] FN = Directory.GetFiles(tmp, "*.mxd");
            foreach (var fl in FN)
            {

                TemplateListComboBox.Items.Add(System.IO.Path.GetFileName(fl));
            }

            TemplateListComboBox.SelectedIndex = 0;
            _FolderName = ArenaStaticProc.GetPathToMapFolder();

           

            textBox2.Text = _FolderName;
            //AnnotationScaleComboBox.SelectedIndex = 7;
            comboBox1.SelectedIndex = 0;
            //numericUpDown1.Value = AiracCircle;

        }

        public EnroutWizardForm(int AiracCircle)
        {
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Desktop);

            InitializeComponent();


            _FolderName = "";
            TemplateListComboBox.Items.Clear();
            var tmp = ArenaStaticProc.GetPathToTemplate() + @"\Enroute\";



            string[] FN = Directory.GetFiles(tmp, "*.mxd");
            foreach (var fl in FN)
            {

                TemplateListComboBox.Items.Add(System.IO.Path.GetFileName(fl));
            }

            TemplateListComboBox.SelectedIndex = 0;
            _FolderName = ArenaStaticProc.GetPathToMapFolder();



            textBox2.Text = _FolderName;
            //AnnotationScaleComboBox.SelectedIndex = 7;
            comboBox1.SelectedIndex = 0;
            airacControl.AiracCircleValue = AiracCircle>1408 ? AiracCircle : 1408;

            _selectedChanels = new List<string>();

        }



        private void button2_Click(object sender, EventArgs e)
        {
            _ProjectName = textBox1.Text;
            _FolderName = textBox2.Text;
            _TemplatetName = TemplateListComboBox.Text;
           // Double.TryParse(AnnotationScaleComboBox.Text, out _Scale);
            if (!TerminalChartsUtil.CheckFileExisting(_ProjectName, _FolderName)) return;
            _mapScale = axPageLayoutControl1.ActiveView.FocusMap.MapScale;

            _AiracCircle = airacControl.AiracCircleValue;

            ArenaStaticProc.SetPathToMapFolder(_FolderName);

            _bookmark = ChartsHelperClass.CreateBookmark(axPageLayoutControl1.ActiveView.FocusMap, "Enrout_Bookmark");

            ChartsHelperClass.SetMapGridVisibilityState(axPageLayoutControl1.ActiveView, true);


            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void button2_Validated(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.Length <= 0 ? "NewChart" : textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var folderBrowserDialog1 = new FolderBrowserDialog
            {
                ShowNewFolderButton = true,
                SelectedPath = ArenaStaticProc.GetPathToMapFolder(),
            };
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
            }


        }

        private void axPageLayoutControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IPageLayoutControlEvents_OnMouseDownEvent e)
        {

        }

        private void TemplateListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChartsHelperClass.SelectChartTemplate(ref axPageLayoutControl1, ArenaStaticProc.GetPathToTemplate() + @"\Enroute\" + TemplateListComboBox.Text, ref listBox1, out mapSize_Height, out mapSize_Width);


            try
            {
                string ConString = ArenaStaticProc.GetTargetDB();

                IWorkspaceFactory2 workspaceFactory = (IWorkspaceFactory2)new AccessWorkspaceFactoryClass();
                IWorkspace Wksp = workspaceFactory.OpenFromFile(ConString, 0);
                IFeatureWorkspace fWksp = (IFeatureWorkspace)Wksp;
                ITable table = fWksp.OpenTable("RouteSegment");

                IFeatureClass pFeatureClass = (IFeatureClass)table;

                _Layer = (IFeatureLayer2)EsriUtils.getLayerByName(axPageLayoutControl1.ActiveView.FocusMap, "RouteSegment"); ;
                _Layer.FeatureClass = pFeatureClass;


                ChartsHelperClass.SetMapGridVisibilityState(axPageLayoutControl1.ActiveView, false);


                IFeatureLayer newlayer = (IFeatureLayer)_Layer;
                newlayer.Visible = true;

                axPageLayoutControl1.ZoomToWholePage();

                ((IActiveView)axPageLayoutControl1.ActiveView.FocusMap).Extent = ((ILayer)_Layer).AreaOfInterest;
                ((IActiveView)axPageLayoutControl1.ActiveView.FocusMap).Refresh();

                comboBox1_SelectedIndexChanged(sender, e);

            }
            catch { }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Level = comboBox1.SelectedIndex;

            if (_Layer == null) return;
            IFeatureLayerDefinition FD = (ESRI.ArcGIS.Carto.IFeatureLayerDefinition)_Layer;
            
            switch (Level)
            {
                case(0):
                    FD.DefinitionExpression = "[codeLvl] ='LOWER'";
                    cnlList = DataCash.GetEnroutesChanels(CODE_ROUTE_SEGMENT_CODE_LVL.LOWER);
                    break;
                case (1):
                    FD.DefinitionExpression = "[codeLvl] ='BOTH'";
                    cnlList = DataCash.GetEnroutesChanels(CODE_ROUTE_SEGMENT_CODE_LVL.BOTH);
                    break;
                case (2):
                    FD.DefinitionExpression = "[codeLvl] ='UPPER'";
                    cnlList = DataCash.GetEnroutesChanels(CODE_ROUTE_SEGMENT_CODE_LVL.UPPER);
                    break;
                case (3):
                    FD.DefinitionExpression = "[codeLvl] ='OTHER'";
                    cnlList = DataCash.GetEnroutesChanels(CODE_ROUTE_SEGMENT_CODE_LVL.OTHER);

                    break;
                default:
                    FD.DefinitionExpression = "[codeLvl] ='BOTH'";
                    break;
            }

            List<RadioCommunicationChanel> arspsChanels = DataCash.GetAirspaceChanels();
            if (cnlList == null) cnlList = new List<RadioCommunicationChanel>();
            if (arspsChanels != null) cnlList.AddRange(arspsChanels);

            ((IActiveView)axPageLayoutControl1.ActiveView.FocusMap).Refresh();

            linkLabel1.Visible = cnlList != null;

        }

       
        private void button4_Click(object sender, EventArgs e)
        {

            SidSettingsForm frm = new SidSettingsForm();

            frm.comboBoxVert.SelectedIndex = 0;
            frm.comboBoxDist.SelectedIndex = 0;
            if (this.vertUom != UOM_DIST_VERT.OTHER) frm.comboBoxVert.Text = this.vertUom.ToString();
            if (this.distUom != UOM_DIST_HORZ.OTHER) frm.comboBoxDist.Text = this.distUom.ToString();
            _AllVOR_DMEflag = true;
            frm.checkBox1.Checked = _AllVOR_DMEflag;
            frm.groupBox6.Visible = true;
            frm.groupBox3.Visible = true;
            frm.groupBox7.Visible = true;
            frm.groupBox5.Visible = false;
            frm.comboBox1.SelectedIndex = 0;
            frm.ShowDialog();

            _AllVOR_DMEflag = frm.checkBox1.Checked;

            this.vertUom = frm.comboBoxVert.SelectedIndex == 0 ? UOM_DIST_VERT.FT : UOM_DIST_VERT.M;
            this.distUom = frm.comboBoxDist.SelectedIndex == 0 ? UOM_DIST_HORZ.NM : UOM_DIST_HORZ.KM;
            this.arspBufWidth = (int)frm.numericUpDown1.Value;
            this.arspSign = frm.checkBox2.Checked;
            this.magBearing = frm.comboBox1.SelectedIndex == 0;
            this._upperSeparation = (int)frm.numericUpDown4.Value;
            this._lowerSeparation = (int)frm.numericUpDown5.Value;


        }

        private void EnroutWizardForm_Load(object sender, EventArgs e)
        {
            axToolbarControl1.SetBuddyControl(axPageLayoutControl1);
            axToolbarControl2.SetBuddyControl(axPageLayoutControl1);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            CalendarForm frm = new CalendarForm();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                airacControl.AiracCircleValue = AiracUtil.AiracUtil.GetAiracCycleByDate(frm.selectedDate);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RadioCommunicationsChanelsListForm frm = new RadioCommunicationsChanelsListForm(cnlList, _selectedChanels);
            frm.ShowDialog();
            _selectedChanels.Clear();
            if (frm.SelectedChanelsList != null && frm.SelectedChanelsList.Count > 0) _selectedChanels.AddRange(frm.SelectedChanelsList);
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string InitialFoldername = ArenaStaticProc.GetMainFolder() + @"\UserTemplates";

            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Files|*.mxd",
                InitialDirectory = InitialFoldername
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var tmpFolder = ArenaStaticProc.GetPathToTemplate() + @"\Enroute\";
                var tmpFileName = ArenaStaticProc.GetPathToTemplate() + @"\Enroute\" + ofd.SafeFileName;

                if (File.Exists(tmpFileName))
                {
                    if (MessageBox.Show("There is already a file with the same name in this location! " + Environment.NewLine
                        + "Replace the file in the destination folder with the file you are copying?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
                }

                File.Copy(ofd.FileName, tmpFileName, true);

                TemplateListComboBox.Items.Add(System.IO.Path.GetFileName(ofd.FileName));
            }
        }
    }
}
