using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using Aran.AranEnvironment;
using System.Windows.Interop;
using Aran.Omega.TypeBEsri.View;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Forms;
using Aran.Omega.TypeB.Settings;
using ESRI.ArcGIS.Carto;

namespace Aran.Omega.TypeBEsri.EsriExtensions
{
    /// <summary>
    /// Summary description for CreateChartCommand.
    /// </summary>
    [Guid("77a8d05a-7ea1-42da-b866-35e2b56a5552")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Aran.Omega.TypeBEsri.EsriExtensions.CreateChartCommand")]
    public sealed class CreateChartCommand : BaseCommand
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

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);

        }

        #endregion
        #endregion

        public static AranTool ByClickToolButton;
        private IApplication m_application;
        private OmegaExtension _pandaExt;
        public CreateChartCommand()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "Create Chart";  //localizable text
            base.m_message = "";  //localizable text 
            base.m_toolTip = "";  //localizable text 
            base.m_name = "CreateChart_Command";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        private bool InitalizeExtension()
        {
            try
            {
                if (_pandaExt == null)
                {
                    ESRI.ArcGIS.esriSystem.UID pID = new ESRI.ArcGIS.esriSystem.UID();
                    pID.Value = "TypeBExtension.OmegaExtension";
                    _pandaExt = m_application.FindExtensionByCLSID(pID) as OmegaExtension;
                    GlobalParams.Settings = _pandaExt.Settings;
                    IMxDocument document = m_application.Document as IMxDocument;
                    GlobalParams.Map = document.FocusMap;
                    GlobalParams.PageLayout = document.PageLayout;
                    GlobalParams.ActiveView = document.FocusMap as IActiveView;
                }
            }
            catch
            {
                //Console.WriteLine("{0} Exception caught.", e);
            }

            return _pandaExt != null;
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

            //Disable if it is not ArcMap
            if (hook is IMxApplication)
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
            if (InitalizeExtension())
            {
                if (InitOmega.InitCommand())
                {
                    AranTool aranToolItem = new AranTool();
                    aranToolItem.Visible = false;
                    aranToolItem.Cursor = Cursors.Cross;
                    ByClickToolButton = aranToolItem;
                    IntPtr parentHandle = new IntPtr(m_application.hWnd);

                    try
                    {
                        GlobalParams.TypeBView = new TypeBView();
                        TypeBView window = GlobalParams.TypeBView;

                        if (GlobalParams.Logs.Length > 0)
                            System.Windows.MessageBox.Show(GlobalParams.Logs, "Omega", MessageBoxButton.OK, MessageBoxImage.Warning);

                        var helper = new WindowInteropHelper(window);
                        helper.Owner = parentHandle;
                        ElementHost.EnableModelessKeyboardInterop(window);
                        GlobalParams.HWND = parentHandle.ToInt32();
                        window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
                        window.Show();

                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show(ex.Message, "Omega", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        #endregion
    }
}
