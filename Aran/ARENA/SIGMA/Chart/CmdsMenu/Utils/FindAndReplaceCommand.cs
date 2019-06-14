using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using ANCOR.MapCore;
using ANCOR.MapElements;
using ArenaStatic;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geometry;

namespace SigmaChart
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("34cfa247-e359-4f14-9d29-5c2ab0f12b66")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.FindAndReplaceCommand")]
    public sealed class FindAndReplaceCommand : BaseCommand
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
        private IGraphicsContainer pGraphicsContainer;
        public FindAndReplaceCommand()
        {
            base.m_category = "SigmaChart"; //localizable text
            base.m_caption = "Find and replace Annotations text";  //localizable text 
            base.m_message = "Find and replace Annotations text";  //localizable text
            base.m_toolTip = "Find and replace Annotations text";  //localizable text
            base.m_name = "FindAndReplaceCommand";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")
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
                pGraphicsContainer = (m_hookHelper.PageLayout as IActiveView).GraphicsContainer;
                //m_hookHelper.ActiveView.GraphicsContainer;
            }
            catch
            {
                m_hookHelper = null;
            }

            if (m_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add Command1.OnClick implementation
            if (SigmaDataCash.ChartElementList == null) return;

            FindReplaceForm fr = new FindReplaceForm();

            fr.comboBox1.Items.Clear();

            foreach (var item in SigmaDataCash.prototype_anno_lst)
            {
                if (item.RelatedFeature.Contains("Graphics")) continue;

                if (SigmaDataCash.ChartElementList.FindAll(el => (el is AbstractChartElement) && ((AbstractChartElement)el).Name.CompareTo(item.Name) == 0).Count <= 0) continue;

                if (!fr.comboBox1.Items.Contains(item.Name))
                    fr.comboBox1.Items.Add(item.Name);
            }
            

            fr.oldTextBox.Text = "";
            fr.newTextBox.Text = "";

            if (fr.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            string _oldTxt = fr.oldTextBox.Text;
            string _newTxt = fr.newTextBox.Text;
            bool MatchCase = fr.CaseCheckBox.Checked;
            bool MatchWholeWord = fr.WordCheckBox.Checked;
            bool ignoreAnno = fr.AnnoCheckBox.Checked;
            bool ignoreGraphics = fr.GraphicsElementsCheckBox.Checked;
            string FelFeature = fr.comboBox1.Text;

            string Message = ChartElementsManipulator.FindAndReplaceUtil(_oldTxt, _newTxt, MatchCase, MatchWholeWord, ignoreAnno, ignoreGraphics, FelFeature, pGraphicsContainer);

            
            if (Message.Length > 0 )
            {
                Message = Message + Environment.NewLine + "Please refresh SIGMA TOC";
                ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);

            }

            MessageBox.Show(Message, "Sigma", MessageBoxButton.OK, MessageBoxImage.Information);



        }

        #endregion
    }
}
