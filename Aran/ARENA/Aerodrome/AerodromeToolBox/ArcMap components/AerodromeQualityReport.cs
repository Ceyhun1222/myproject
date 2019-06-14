using Aerodrome.DataCompleteness;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.CatalogUI;
using ESRI.ArcGIS.Framework;
using Framework.Stasy.Context;
using HelperDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;

namespace AerodromeToolBox
{

    [Guid("422CFDA6-E5B7-4BEA-940E-229B734FDD6E")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Aerodrome.AerodromeQualityReport")]
    public sealed class AerodromeQualityReport : BaseCommand
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
            GxCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            GxCommands.Unregister(regKey);

        }

        #endregion
        #endregion

        private IApplication m_application;
        public AerodromeQualityReport()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "Generate quality report"; //localizable text
            base.m_caption = "Generate quality report";  //localizable text
            base.m_message = "Generate quality report";  //localizable text 
            base.m_toolTip = "Generate quality report";  //localizable text 
            base.m_name = "AerodromeQualityReport";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")
            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                // base.m_bitmap = global::ArenaToolBox.Properties.Resources.avia_icon;//new Bitmap(GetType(), bitmapResourceName);
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

            m_application = hook as IApplication;

            //Disable if it is not ArcCatalog
            if (hook is IGxApplication)
                base.m_enabled = true;
            else
                base.m_enabled = false;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            if (AerodromeDataCash.ProjectEnvironment is null)
            {
                System.Windows.MessageBox.Show("Empty project", "Aerodrome", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                return;
            }

            var sfd = new SaveFileDialog();
            sfd.Filter = "HTML (*.html)|*.html|All Files (*.*)|(*.*)";

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            DataEvaluationHelper evaluationHelper = new DataEvaluationHelper(sfd.FileName);
            evaluationHelper.EvaluateData();
            //evaluationHelper.CreateHtmlCompletenessReport();
            evaluationHelper.CreateHtmlQualityReport();

            MessageScreen messageScreen = new MessageScreen();
            var parentHandle = new IntPtr(m_application.hWnd);
            var helper = new WindowInteropHelper(messageScreen) { Owner = parentHandle };
            messageScreen.MessageText = "Report Generated";
            messageScreen.ShowDialog();
        }

        public override bool Enabled
        {
            get
            {
                //if editing is started, then enable the command.  
                if (AerodromeDataCash.ProjectEnvironment is null)
                    return false;
                else
                    return true;
            }
        }

        #endregion
    }

}
