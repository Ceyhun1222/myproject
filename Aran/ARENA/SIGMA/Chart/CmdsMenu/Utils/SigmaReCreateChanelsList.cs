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
using System.Collections.Generic;
using System.Linq;
using PDM;
using ESRI.ArcGIS.Geometry;
using AranSupport;
using ChartCodingTable;
using SigmaChart.CmdsMenu;

namespace SigmaChart
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("1866c616-6f49-4669-aa1d-18d628dbcfc3")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.SigmareCreateChanelsList")]
    public sealed class SigmareCreateChanelsList : BaseCommand
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

        public SigmareCreateChanelsList()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "SigmaChart"; //localizable text
            base.m_caption = "Create FC List";  //localizable text 
            base.m_message = "Create FC List";  //localizable text
            base.m_toolTip = "Create FC List";  //localizable text
            base.m_name = "Create FC List";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")
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

            string FN = System.IO.Path.GetDirectoryName((m_application.Document as IDocumentInfo2).Path);
            chartInfo ci = EsriUtils.GetChartIno(FN);

            var _ADHP = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList
                            where (element != null) && (element.PDM_Type == PDM_ENUM.AirportHeliport) &&
                            (((PDM.AirportHeliport)element).Designator.CompareTo(ci.ADHP) == 0)
                            select element).FirstOrDefault();

            if (_ADHP != null)
            {
                AirportHeliport selectedADHP = (AirportHeliport)_ADHP;
                List<string> _selectedChanels = new List<string>();
                try
                {
                    RadioCommunicationsChanelsListForm frm = new RadioCommunicationsChanelsListForm(selectedADHP.CommunicationChanels, _selectedChanels);
                    frm.ShowDialog();
                    _selectedChanels.Clear();
                    if (frm.SelectedChanelsList != null)
                    {
                        _selectedChanels.AddRange(frm.SelectedChanelsList);

                        TerminalChartsUtil.UpdateDinamicText(m_hookHelper, "Sigma_chanels", _selectedChanels);
                    }


                    ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);
                }
                catch { }
            }
        }


        #endregion
    }
}
