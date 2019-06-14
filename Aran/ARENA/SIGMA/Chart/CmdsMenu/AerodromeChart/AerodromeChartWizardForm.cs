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
using CheckComboBox;

namespace SigmaChart.CmdsMenu
{
    public partial class AerodromeChartWizardForm : Form
    {

        public string _FolderName;
        public string _ProjectName;
        public string _TemplatetName;
       // public double _Scale;
        public double mapSize_Height;
        public double mapSize_Width;
        public int Level =0;
        public bool _RNAVflag;
        //public bool _AllVOR_DMEflag = true;
        public int _AiracCircle;
        public IAOIBookmark _bookmark;
        public double _mapScale;
        public double _mapAngle =0;
        public int aerodromeChartType  = 0;

        private List<AirportHeliport> AdhpList;
        private IFeatureLayer2 _LayerTWY;
        private IFeatureLayer2 _Layer;

        public List<string> _selectedChanels = null;
        public UOM_DIST_VERT vertUom = UOM_DIST_VERT.FT;
        public UOM_DIST_HORZ distUom = UOM_DIST_HORZ.M;
        public AirportHeliport selectedADHP = null;

        private int _ChartType;

        public AerodromeChartWizardForm()
        {
            

        }

        public AerodromeChartWizardForm(int AiracCircle, int ChartType)
        {
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Desktop);

            InitializeComponent();
            this.aerodromeChartType = ChartType;

            _FolderName = "";
            TemplateListComboBox.Items.Clear();
            string tmp = "";

            _ChartType = ChartType;

            switch (ChartType)
            {
                case (8):
                    tmp = ArenaStaticProc.GetPathToTemplate() + @"\AerodromeElectronicChart\";
                    textBox1.Text = "AerodromeElectronicChart";
                    break;

                case (9):
                    tmp = ArenaStaticProc.GetPathToTemplate() + @"\AerodromeParkingDockingChart\";
                    textBox1.Text = "AerodromeParkingDockingChart";
                    break;

                case (10):
                    tmp = ArenaStaticProc.GetPathToTemplate() + @"\AerodromeGroundMovementChart\";
                    textBox1.Text = "AerodromeGroundMovementChart";
                    break;

                case (11):
                    tmp = ArenaStaticProc.GetPathToTemplate() + @"\AerodromeBirdChart\";
                    textBox1.Text = "AerodromeBirdChart";
                    break;

                case (12):
                    tmp = ArenaStaticProc.GetPathToTemplate() + @"\AerodromeChart\";
                    textBox1.Text = "AerodromeChart";
                    break;

                default:
                    tmp = ArenaStaticProc.GetPathToTemplate() + @"\AerodromeElectronicChart\";
                    textBox1.Text = "AerodromeElectronicChart";
                    break;
            }
           



            string[] FN = Directory.GetFiles(tmp, "*.mxd");
            foreach (var fl in FN)
            {

                TemplateListComboBox.Items.Add(System.IO.Path.GetFileName(fl));
            }

            TemplateListComboBox.SelectedIndex = 0;
            _FolderName = ArenaStaticProc.GetPathToMapFolder();

            AdhpList = DataCash.GetAirportlist();
            comboBox1.Items.Clear();

            foreach (var itemADHP in AdhpList)
            {
                comboBox1.Items.Add(itemADHP.Designator);
            }

            if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;

            _AiracCircle = AiracCircle;

            textBox2.Text = _FolderName;
            airacControl.AiracCircleValue = AiracCircle > 1408 ? AiracCircle : 1408;

            _selectedChanels = new List<string>();


        }



        private void button2_Click(object sender, EventArgs e)
        {
            _ProjectName = textBox1.Text;
            _FolderName = textBox2.Text;
            _TemplatetName = TemplateListComboBox.Text;
            if (!TerminalChartsUtil.CheckFileExisting(_ProjectName, _FolderName)) return;
            _mapScale = axPageLayoutControl1.ActiveView.FocusMap.MapScale;
            //_RNAVflag = checkBox1.Checked;

            _AiracCircle = airacControl.AiracCircleValue;

            ArenaStaticProc.SetPathToMapFolder(_FolderName);


            _bookmark = ChartsHelperClass.CreateBookmark(axPageLayoutControl1.ActiveView.FocusMap, "SigmaAerodromeChart_Bookmark");

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

        private void TemplateListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (this.aerodromeChartType)
            {
                case (8):
                    ChartsHelperClass.SelectChartTemplate(ref axPageLayoutControl1, ArenaStaticProc.GetPathToTemplate() + @"\AerodromeElectronicChart\" + TemplateListComboBox.Text, ref listBox1, out mapSize_Height, out mapSize_Width);
                    break;

                case (9):
                    ChartsHelperClass.SelectChartTemplate(ref axPageLayoutControl1, ArenaStaticProc.GetPathToTemplate() + @"\AerodromeParkingDockingChart\" + TemplateListComboBox.Text, ref listBox1, out mapSize_Height, out mapSize_Width);
                    break;

                case (10):
                    ChartsHelperClass.SelectChartTemplate(ref axPageLayoutControl1, ArenaStaticProc.GetPathToTemplate() + @"\AerodromeGroundMovementChart\" + TemplateListComboBox.Text, ref listBox1, out mapSize_Height, out mapSize_Width);
                    break;

                case (11):
                    ChartsHelperClass.SelectChartTemplate(ref axPageLayoutControl1, ArenaStaticProc.GetPathToTemplate() + @"\AerodromeBirdChart\" + TemplateListComboBox.Text, ref listBox1, out mapSize_Height, out mapSize_Width);
                    break;

                case (12):
                    ChartsHelperClass.SelectChartTemplate(ref axPageLayoutControl1, ArenaStaticProc.GetPathToTemplate() + @"\AerodromeChart\" + TemplateListComboBox.Text, ref listBox1, out mapSize_Height, out mapSize_Width);
                    break;
                

                default:
                    ChartsHelperClass.SelectChartTemplate(ref axPageLayoutControl1, ArenaStaticProc.GetPathToTemplate() + @"\AerodromeElectronicChart\" + TemplateListComboBox.Text, ref listBox1, out mapSize_Height, out mapSize_Width);
                    break;
            }

            


            try
            {
                string ConString = ArenaStaticProc.GetTargetDB_Aerodrome();

                IWorkspaceFactory2 workspaceFactory = (IWorkspaceFactory2)new AccessWorkspaceFactoryClass();
                IWorkspace Wksp = workspaceFactory.OpenFromFile(ConString, 0);
                IFeatureWorkspace fWksp = (IFeatureWorkspace)Wksp;
                ITable table = fWksp.OpenTable("RunwayElement");

                IFeatureClass pFeatureClass = (IFeatureClass)table;

                _Layer = (IFeatureLayer2)EsriUtils.getLayerByName(axPageLayoutControl1.ActiveView.FocusMap, "RunwayElement");
                _Layer.FeatureClass = pFeatureClass;

                IFeatureLayer newlayer = (IFeatureLayer)_Layer;
                newlayer.Visible = true;


                table = fWksp.OpenTable("TaxiwayElement");

                pFeatureClass = (IFeatureClass)table;

                _LayerTWY = (IFeatureLayer2)EsriUtils.getLayerByName(axPageLayoutControl1.ActiveView.FocusMap, "TaxiwayElement");
                _LayerTWY.FeatureClass = pFeatureClass;

                newlayer = (IFeatureLayer)_LayerTWY;
                newlayer.Visible = true;




                axPageLayoutControl1.ZoomToWholePage();

                ((IActiveView)axPageLayoutControl1.ActiveView.FocusMap).Extent = ((ILayer)_Layer).AreaOfInterest;
                ((IActiveView)axPageLayoutControl1.ActiveView.FocusMap).Refresh();


            }
            catch { }
        }

       
        private void button4_Click(object sender, EventArgs e)
        {

            AerodromeChartSettings frm = new AerodromeChartSettings();

            frm.comboBoxVert.SelectedIndex = 0;
            frm.comboBoxDist.SelectedIndex = 0;
            if (this.vertUom != UOM_DIST_VERT.OTHER) frm.comboBoxVert.Text = this.vertUom.ToString();
            if (this.distUom != UOM_DIST_HORZ.OTHER) frm.comboBoxDist.Text = this.distUom.ToString();
            //_AllVOR_DMEflag = true;
            //frm.checkBox1.Checked = _AllVOR_DMEflag;
            frm.ShowDialog();
            //_AllVOR_DMEflag = frm.checkBox1.Checked;

            this.vertUom = frm.comboBoxVert.SelectedIndex == 0 ? UOM_DIST_VERT.FT : UOM_DIST_VERT.M;
            this.distUom = frm.comboBoxDist.SelectedIndex == 0 ? UOM_DIST_HORZ.M : UOM_DIST_HORZ.FT;

        }

        private void AerodromeChartWizardForm_Load(object sender, EventArgs e)
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
            RadioCommunicationsChanelsListForm frm = new RadioCommunicationsChanelsListForm(selectedADHP.CommunicationChanels, _selectedChanels);
            frm.ShowDialog();
            _selectedChanels.Clear();
            if (frm.SelectedChanelsList != null && frm.SelectedChanelsList.Count > 0) _selectedChanels.AddRange(frm.SelectedChanelsList);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;

            if (AdhpList != null && AdhpList.Count > 0)
            {

                selectedADHP = AdhpList[comboBox1.SelectedIndex];
                linkLabel1.Text = selectedADHP.CommunicationChanels != null && selectedADHP.CommunicationChanels.Count > 0 && this.aerodromeChartType !=11 ? "Communication chanels ....." : "";
               

                comboBox2.Items.Clear();
                if (selectedADHP.RunwayList != null)
                {
                    foreach (var item in selectedADHP.RunwayList)
                    {
                        comboBox2.Items.Add(item.GetObjectLabel());
                    }

                    comboBox2.SelectedIndex = 0;
                }

                if (_Layer == null) return;


                IFeatureLayerDefinition FD = (ESRI.ArcGIS.Carto.IFeatureLayerDefinition)_Layer;
                if (FD == null) return;
                FD.DefinitionExpression = "ID_AirportHeliport IN ('" + selectedADHP.ID + "')";


                FD = (ESRI.ArcGIS.Carto.IFeatureLayerDefinition)_LayerTWY;
                if (FD == null) return;
                FD.DefinitionExpression = "ID_AirportHeliport IN ('" + selectedADHP.ID + "')";


                (axPageLayoutControl1.ActiveView.FocusMap as IActiveView).Extent = ChartsHelperClass.GetLayerExtent(_Layer, FD.DefinitionExpression);


                if (selectedADHP != null)
                {
                    if (selectedADHP.Geo == null) selectedADHP.RebuildGeo();

                    if (selectedADHP.Geo != null)
                    {
                        double newX = ((IPoint)selectedADHP.Geo).X;
                        EsriUtils.ChangeProjectionAndMeredian(newX, axPageLayoutControl1.ActiveView.FocusMap);
                    }
                }

                ((IActiveView)axPageLayoutControl1.ActiveView.FocusMap).Refresh();

            }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox3.Clear();
            double angl = 9999;
            double mv = selectedADHP.MagneticVariation.HasValue ? selectedADHP.MagneticVariation.Value : 0;
            if (selectedADHP.RunwayList[comboBox2.SelectedIndex].RunwayDirectionList != null)
            {
                foreach (var rdn in selectedADHP.RunwayList[comboBox2.SelectedIndex].RunwayDirectionList)
                {
                    if (!rdn.TrueBearing.HasValue) continue;
                    mv = rdn.MagBearing.HasValue ? rdn.MagBearing.Value : mv;
                    if ((rdn.TrueBearing.Value + mv) < angl) angl = rdn.TrueBearing.Value;
                }

                _mapAngle = angl;
               // _mapAngle = selectedADHP.RunwayList[comboBox2.SelectedIndex].RunwayDirectionList[0].TrueBearing.HasValue ? selectedADHP.RunwayList[comboBox2.SelectedIndex].RunwayDirectionList[0].TrueBearing.Value : 0;
                textBox3.Text = (_mapAngle - 90).ToString();

                ((IActiveView)axPageLayoutControl1.ActiveView.FocusMap).ScreenDisplay.DisplayTransformation.Rotation = _mapAngle -90;
            }


         }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _mapAngle = Double.Parse(textBox3.Text) +90;

            ((IActiveView)axPageLayoutControl1.ActiveView.FocusMap).ScreenDisplay.DisplayTransformation.Rotation = _mapAngle - 90;
            ((IActiveView)axPageLayoutControl1.ActiveView.FocusMap).Refresh();
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
                string tmp = "";

                switch (_ChartType)
                {
                    case (8):
                        tmp = ArenaStaticProc.GetPathToTemplate() + @"\AerodromeElectronicChart\";
                        break;

                    case (9):
                        tmp = ArenaStaticProc.GetPathToTemplate() + @"\AerodromeParkingDockingChart\";
                        break;

                    case (10):
                        tmp = ArenaStaticProc.GetPathToTemplate() + @"\AerodromeGroundMovementChart\";
                        break;

                    case (11):
                        tmp = ArenaStaticProc.GetPathToTemplate() + @"\AerodromeBirdChart\";
                        break;

                    case (12):
                        tmp = ArenaStaticProc.GetPathToTemplate() + @"\AerodromeChart\";
                        break;

                    default:
                        tmp = ArenaStaticProc.GetPathToTemplate() + @"\AerodromeElectronicChart\";
                        break;
                }
                var tmpFileName = tmp + ofd.SafeFileName;

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
