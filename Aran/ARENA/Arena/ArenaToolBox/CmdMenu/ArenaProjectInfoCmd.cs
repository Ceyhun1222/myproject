using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace ARENA
{
    /// <summary>
    /// Summary description for ArenaProjectInfoCmd.
    /// </summary>
    [Guid("6ddba859-4600-4b21-a812-f10f322b08ad")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Arena.ArenaProjectInfoCmd")]
    public sealed class ArenaProjectInfoCmd : BaseCommand
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
        public ArenaProjectInfoCmd()
        {
            base.m_category = "Arena project info"; //localizable text
            base.m_caption = "Arena project info";  //localizable text
            base.m_message = "Arena project info";  //localizable text 
            base.m_toolTip = "Arena project info";  //localizable text 
            base.m_name = "ArenaProjectInfoCmd";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                //string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = global::ArenaToolBox.Properties.Resources.info;
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
            // TODO: Add ArenaVersionInfoCmd.OnClick implementation
            //DataCash.ProjectEnvironment.Data.ProjectLog.Count.ToString();

            var mdbName = ArenaStatic.ArenaStaticProc.GetTargetDB();
            var logName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(mdbName), "ProjectLog.txt");
            if (File.Exists(logName))
            {
                Process.Start(logName);
            }
            else
            {
                if (DataCash.ProjectEnvironment.Data.ProjectLog != null)
                System.IO.File.WriteAllLines(logName, DataCash.ProjectEnvironment.Data.ProjectLog.ToArray());
                Process.Start(logName);
            }


        }

        #endregion
    }
}
