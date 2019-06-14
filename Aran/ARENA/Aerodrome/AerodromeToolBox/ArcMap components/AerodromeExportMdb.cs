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
    [Guid("98F0C7D1-A114-48FD-BC37-6BB62DF1B0A7")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Aerodrome.AerodromeExportMdb")]
    public sealed class AerodromeExportMdb : BaseCommand
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
        public AerodromeExportMdb()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "Export aerodrome mdb file"; //localizable text
            base.m_caption = "Export mdb";  //localizable text
            base.m_message = "Export mdb";  //localizable text 
            base.m_toolTip = "Export mdb";  //localizable text 
            base.m_name = "AerodromeExportMdb";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")
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

            var saveFileDialog1 = new SaveFileDialog
            {
                Filter = @"Access file format (*.mdb)|*.mdb",
                RestoreDirectory = true
            };

            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;

            string targetDB = AerodromeDataCash.ProjectEnvironment.CurrentProjTempPath;

            string mdbName = targetDB + @"\AMDB.mdb";

            System.IO.File.Copy(mdbName, saveFileDialog1.FileName, true);

            MessageScreen messageScreen = new MessageScreen();
            var parentHandle = new IntPtr(m_application.hWnd);
            var helper = new WindowInteropHelper(messageScreen) { Owner = parentHandle };
            messageScreen.MessageText = "File exported";
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
