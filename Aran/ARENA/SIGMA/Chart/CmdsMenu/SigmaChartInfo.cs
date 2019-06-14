using System;

using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Carto;
using EsriWorkEnvironment;


namespace SigmaChart
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("dd4900ae-f24b-4d5c-9e51-5772ff0fa11e")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.SigmaChartInfo")]
    public sealed class SigmaChartInfo : BaseCommand
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
        public SigmaChartInfo()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "Chart info"; //localizable text
            base.m_caption = "Chart info";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "Chart info";  //localizable text
            base.m_name = "SigmaChartInfo";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                //string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
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
            m_application = hook as IApplication;
            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                    m_hookHelper = null;
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

            if (ci != null)
            {
               string messageString = "Organization: " + ci.organization + "\r" + "\n" + 
                                        "Airport: "+ci.ADHP + "\r" + "\n" +
                                        "Chart: "+ci.chartName + "\r" + "\n" +
                                        "Airac circle: " + ci.airacCircle  +"\r" + "\n" +
                                        "Efective Date: " + ci.efectiveDate;


                if (ci.RunwayDirectionsList!=null && ci.RunwayDirectionsList.Count > 0)
                {
                    messageString = messageString + "\r" + "\n" + "RWY:" + "\r" + "\n";
                    foreach (var item in ci.RunwayDirectionsList)
                    {
                        messageString = messageString +"        " +item + "   " + "\r" + "\n";
                    }
                }


                if (ci.baseTempName !=null && ci.baseTempName.Length >0)
                {
                    messageString = messageString + "Base template: " + ci.baseTempName;
                }
                System.Windows.Forms.MessageBox.Show(messageString, "ChartInfo", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }

        }

        #endregion
    }
}
