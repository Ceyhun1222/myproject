using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ArenaStatic;
using EsriWorkEnvironment;
using ARENA;
using SigmaChart.CmdsMenu.TemplatesManagerMenu;
using System.IO;
using System.Windows.Forms;

namespace SigmaChart
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("10ad1854-74e8-4b31-b492-6451e83d0fe7")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.SigmaSaveTemplate")]
    public sealed class SigmaSaveTemplate : BaseCommand
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Register(regKey);
            ControlsCommands.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);
            ControlsCommands.Unregister(regKey);
        }

        #endregion
        #endregion

        private IHookHelper m_hookHelper = null;
        private IApplication m_application;

        public SigmaSaveTemplate()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "SigmaChart"; //localizable text
            base.m_caption = "Save chart as template";  //localizable text 
            base.m_message = "Sigma save as template";  //localizable text
            base.m_toolTip = "Sigma save as template";  //localizable text
            base.m_name = "Sigma save as template";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")
            try
            {
                //
                // TODO: change resource name if necessary 
                //
                base.m_bitmap = global::SigmaChart.Properties.Resources.Sigma;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (hook == null)
                return;

            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                    m_hookHelper = null;

                m_application = hook as IApplication;
            }
            catch
            {
                m_hookHelper = null;
            }

            if (m_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {

            ILayer _Layer = EsriUtils.getLayerByName(((IMxDocument)m_application.Document).FocusMap, "AirportHeliport");
            if (_Layer == null) _Layer = EsriUtils.getLayerByName(((IMxDocument)m_application.Document).FocusMap, "AirportCartography");
            if (_Layer == null) return;

            var fc = ((IFeatureLayer)_Layer).FeatureClass;
            var workspaceEdit = (IWorkspaceEdit)fc.FeatureDataset.Workspace;

            if (workspaceEdit == null) return;

            string path = System.IO.Path.GetDirectoryName(((IWorkspace)workspaceEdit).PathName);
            string filename = System.IO.Path.GetFileName(((IMapDocument)m_application.Document).DocumentFilename);
            string pathToTemplateFileXML = "";
            ILayer lyr = null;
            bool editFlaf = false;
            if (SigmaDataCash.SigmaChartType > 100)
            {
                SigmaDataCash.SigmaChartType = SigmaDataCash.SigmaChartType - 100;
                editFlaf = true;
            }

            SigmaChartTypes  chartTp = (SigmaChartTypes)SigmaDataCash.SigmaChartType;
            switch (chartTp)
            {
                case SigmaChartTypes.EnrouteChart_Type:
                    pathToTemplateFileXML = ArenaStaticProc.GetPathToTemplate() + @"\Enroute\";
                    lyr = AddSpecialLayer("RouteSegment");
                    break;

                case SigmaChartTypes.SIDChart_Type:
                    pathToTemplateFileXML = ArenaStaticProc.GetPathToTemplate() + @"\SID\";
                    lyr = AddSpecialLayer("ProcedureLegs");
                    break;

                case SigmaChartTypes.STARChart_Type:
                    pathToTemplateFileXML = ArenaStaticProc.GetPathToTemplate() + @"\STAR\";
                    lyr = AddSpecialLayer("ProcedureLegs");
                    break;

                case SigmaChartTypes.IAPChart_Type:
                    pathToTemplateFileXML = ArenaStaticProc.GetPathToTemplate() + @"\IAP\";
                    lyr = AddSpecialLayer("ProcedureLegs");
                    break;

                case SigmaChartTypes.AreaChart:
                    pathToTemplateFileXML = ArenaStaticProc.GetPathToTemplate() + @"\AreaChart\";
                    lyr = AddSpecialLayer("AirspaceVolume");
                    lyr = AddSpecialLayer("AirportHeliport");
                    lyr = AddSpecialLayer("WayPoint");
                    break;

                case SigmaChartTypes.AerodromeElectronicChart:
                    pathToTemplateFileXML = ArenaStaticProc.GetPathToTemplate() + @"\AerodromeElectronicChart\";
                    lyr = AddSpecialLayer("AirspaceVolume");
                    lyr = AddSpecialLayer("AirportHeliport");
                    lyr = AddSpecialLayer("WayPoint");
                    break;

                case SigmaChartTypes.AerodromeParkingDockingChart:
                    pathToTemplateFileXML = ArenaStaticProc.GetPathToTemplate() + @"\AerodromeParkingDockingChart\";
                    lyr = AddSpecialLayer("AirspaceVolume");
                    lyr = AddSpecialLayer("AirportHeliport");
                    lyr = AddSpecialLayer("WayPoint");
                    break;

                case SigmaChartTypes.AerodromeGroundMovementChart:
                    pathToTemplateFileXML = ArenaStaticProc.GetPathToTemplate() + @"\AerodromeGroundMovementChart\";
                    lyr = AddSpecialLayer("AirspaceVolume");
                    lyr = AddSpecialLayer("AirportHeliport");
                    lyr = AddSpecialLayer("WayPoint");
                    break;

                case SigmaChartTypes.AerodromeBirdChart:
                    pathToTemplateFileXML = ArenaStaticProc.GetPathToTemplate() + @"\AerodromeBirdChart\";
                    lyr = AddSpecialLayer("AirspaceVolume");
                    lyr = AddSpecialLayer("AirportHeliport");
                    lyr = AddSpecialLayer("WayPoint");
                    break;

                case SigmaChartTypes.AerodromeChart:
                    pathToTemplateFileXML = ArenaStaticProc.GetPathToTemplate() + @"\AerodromeChart\";
                    lyr = AddSpecialLayer("AirspaceVolume");
                    lyr = AddSpecialLayer("AirportHeliport");
                    lyr = AddSpecialLayer("WayPoint");
                    break;

                case SigmaChartTypes.MinimumAltitudeChart:
                    pathToTemplateFileXML = ArenaStaticProc.GetPathToTemplate() + @"\MinimumAltitudeChart\";
                    lyr = AddSpecialLayer("AirspaceVolume");
                    lyr = AddSpecialLayer("AirportHeliport");
                    lyr = AddSpecialLayer("WayPoint");
                    break;

                case SigmaChartTypes.None:
                case SigmaChartTypes.ChartTypeA:
                case SigmaChartTypes.PATChart_Type:
                default:
                    pathToTemplateFileXML = "";
                    break;
            }

            
            if (pathToTemplateFileXML.Length <= 0) return;

           
            try
            {
                m_application.SaveDocument();
                Application.DoEvents();
                string newFileName = filename;


                string _FolderName = TerminalChartsUtil.GetTemplateForChart(chartTp);

                if (!editFlaf)
                {
                    string[] FN = Directory.GetFiles(System.IO.Path.GetDirectoryName(_FolderName), "*.mxd");


                    fileNameForm frm = new fileNameForm();
                    frm.listBox1.Items.Clear();
                    foreach (var item in FN)
                    {
                        string fn = System.IO.Path.GetFileNameWithoutExtension(item);
                        if (fn.StartsWith("sigma_")) continue;
                        frm.listBox1.Items.Add(fn);
                    }

                    frm.textBox1.Text = chartTp.ToString() + "_SigmaTemplate.mxd";
                    if (frm.ShowDialog() == DialogResult.Cancel) return;
                    newFileName = frm.textBox1.Text;
                }


                if (!newFileName.EndsWith(".mxd"))
                    newFileName += ".mxd";

                File.Copy(filename, System.IO.Path.Combine(_FolderName, newFileName), true);

                _FolderName = ArenaStaticProc.GetMainFolder() + @"\UserTemplates" + @"\" + chartTp.ToString();
                if (!System.IO.Directory.Exists(_FolderName)) System.IO.Directory.CreateDirectory(_FolderName);

                File.Copy(filename, System.IO.Path.Combine(_FolderName, newFileName), true);

                ArenaMessageForm msgFrm = new ArenaMessageForm("SIGMA", "Template is saved successfully", global::SigmaChart.Properties.Resources.SigmaMessage);
                msgFrm.TopMost = true;
                msgFrm.checkBox1.Visible = false;
                msgFrm.ShowDialog();

            }
            catch { }


        }

        private ILayer AddSpecialLayer(string layerName)
        {
            //////////////////////////////////////////////////////////

            ITable _Table = EsriUtils.getTableByname((IFeatureWorkspace)SigmaDataCash.environmentWorkspaceEdit, layerName);
            ILayer res = null;

            if (_Table != null)
            {


                ILayer nlayer = (ILayer)(new FeatureLayer());
                nlayer.Name = layerName;
                IFeatureLayer newlayer = (IFeatureLayer)nlayer;
                newlayer.FeatureClass = (IFeatureClass)_Table;
                newlayer.Name = layerName;
                newlayer.Visible = false;
                m_hookHelper.FocusMap.AddLayer(newlayer);
                m_hookHelper.FocusMap.MoveLayer(newlayer, m_hookHelper.FocusMap.LayerCount - 1);
                res = nlayer;

            }

            return res;

            //////////////////////////////////////////////////////////
        }

        #endregion
    }
}
