using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;
//using System.IO;
//using System.Collections.Generic;
using ZzArchivator;
using ArenaStatic;
using PDM;
using DataModule;
using ARENA.Enums_Const;
using ARENA.Project;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesGDB;
using EsriWorkEnvironment;
using AranSupport;
using ArenaLogManager;

namespace ARENA
{
    /// <summary>
    /// Summary description for ArenaProjectOpenCmd.
    /// </summary>
    [Guid("dc012737-d949-4436-b94f-789633c6be78")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Arena.ArenaProjectOpenCmd")]
    public sealed class ArenaProjectOpenCmd : BaseCommand
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


        private IApplication m_application;
        public ArenaProjectOpenCmd()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "Open arena project file"; //localizable text
            base.m_caption = "Open Project";  //localizable text
            base.m_message = "Open arena project file";  //localizable text 
            base.m_toolTip = "Open arena project file";  //localizable text 
            base.m_name = "ArenaProjectOpenCmd";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")
            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                //string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = global::ArenaToolBox.Properties.Resources.ArcReader16; 

                
                 
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
            var openFileDialog1 = new OpenFileDialog { Filter = @"Panda type files (*.pdm)|*.pdm" };
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            LogManager.GetLogger(GetType().Name).Info($" ----------Open project; Path: " + openFileDialog1.FileName);
            //OpenSelectedFile(openFileDialog1.FileName);
            ArenaProjectOpener.OpenSelectedFile(openFileDialog1.FileName, m_application);
            
        }

     
          #endregion
    }
}
