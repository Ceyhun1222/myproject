using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.CatalogUI;
using System.Windows.Forms;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using System.IO;
using Aerodrome.DB;
using ESRI.ArcGIS.Geodatabase;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Aerodrome.Features;
using Aerodrome.Enums;
using ESRI.ArcGIS.Geometry;
using Framework.Stasy.SyncProvider;
using Framework.Stasy.Context;
using WpfUI;
using System.Collections.Generic;
using Framework.Stasy.Core;
using System.Linq;
using Framework.Stasy.Helper;
using HelperDialog;
using System.Windows.Interop;
using Aerodrome.Metadata;

namespace AerodromeToolBox
{
    [Guid("2BA8F247-4FEE-4906-B272-21AF2718CAFE")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Aerodrome.AerodromeNewProject")]
    public sealed class AerodromeNewProject : BaseCommand
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
        public AerodromeNewProject()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "Create Empty Aerodrome project file"; //localizable text
            base.m_caption = "Empty Project";  //localizable text
            base.m_message = "Empty Aerodrome project file";  //localizable text 
            base.m_toolTip = "Empty Aerodrome project file";  //localizable text 
            base.m_name = "AerodromeNewProject";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")
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

            if (AerodromeDataCash.ProjectEnvironment != null)
            {
                m_application.SaveDocument(System.IO.Path.Combine(AerodromeDataCash.ProjectEnvironment.CurrentProjTempPath, "AMDB.mxd"));
                if (AerodromeDataCash.ProjectEnvironment.ProjectNeedSaved == true)
                {
                    System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show("Save changes to the AMDB project?", "Aerodrome", System.Windows.MessageBoxButton.YesNoCancel, System.Windows.MessageBoxImage.Question);
                    if (result == System.Windows.MessageBoxResult.Yes)
                    {                        
                        HelperMethods.SaveAmdbProject(m_application, showSplash: true);                        
                    }                   
                    else if (result == System.Windows.MessageBoxResult.Cancel)
                    {
                        return;
                    }
                    AerodromeDataCash.ProjectEnvironment.ProjectNeedSaved = false;
                }
            }
            //Здесь открыть окно для ввода данных аэропорта.

            ARPInputWindow window = new ARPInputWindow();
            window.ShowDialog();
            if (!window.DialogResult.Value) return;

            

            Splasher.Splash = new SplashScreen();
            var parentHandle = new IntPtr(m_application.hWnd);
            var helper = new WindowInteropHelper(Splasher.Splash) { Owner = parentHandle };
            MessageListener.Instance.ReceiveMessage("");
          
            Splasher.ShowSplash();
           
            IMxDocument pNewDocument = (IMxDocument)m_application.Document;
            IMap pMap = pNewDocument.FocusMap;
            

            AerodromeEnvironment Environment = new AerodromeEnvironment { mxApplication = m_application };
            m_application.Caption = "Untitled - ArcMap";

            AerodromeDataCash.ProjectEnvironment = Environment;

            HelperMethods.CreateAmdbProject(m_application, window.ARP);
            

            IMxDocument pMxDoc = AerodromeDataCash.ProjectEnvironment.mxApplication.Document as IMxDocument;
            ESRI.ArcGIS.Geometry.IEnvelope envelopeCls = ((IPoint)window.ARP.geopnt).Envelope;
            double dim = 1.0;
            double layerWidth = ((IPoint)window.ARP.geopnt).Envelope.Width;
            double layerHeight = ((IPoint)window.ARP.geopnt).Envelope.Height;
            double layerDim = System.Math.Max(layerWidth, layerHeight) * 0.05;

            if (layerDim > 0.0)
                dim = System.Math.Min(1.0, layerDim);

            double xMin = ((IPoint)window.ARP.geopnt).Envelope.XMin;
            double yMin = ((IPoint)window.ARP.geopnt).Envelope.YMin;

            ESRI.ArcGIS.Geometry.IPoint pointCls = new ESRI.ArcGIS.Geometry.PointClass();
            pointCls.X = xMin;
            pointCls.Y = yMin;
            envelopeCls.Width = dim;
            envelopeCls.Height = dim;
            envelopeCls.CenterAt(pointCls);
            pMxDoc.ActiveView.Extent = envelopeCls;

            pMxDoc.ActiveView.FocusMap.MapScale = 15000;
            pMxDoc.ActiveView.Refresh();

            AerodromeDataCash.ProjectEnvironment.ProjectNeedSaved = true;
            Application.DoEvents();

            Splasher.CloseSplash();
            MessageScreen messageScreen = new MessageScreen();
            var messageScreeenHelper = new WindowInteropHelper(messageScreen) { Owner = parentHandle };
            messageScreen.MessageText = "New project created";
            messageScreen.ShowDialog();
        }

        #endregion
    }
   
}
