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
using ARENA.Enums_Const;
using ArenaStatic;
using DataModule;
using System.IO;
using ArenaLogManager;

namespace ARENA
{
    /// <summary>
    /// Summary description for ArenaExportToMxdCmd.
    /// </summary>
    [Guid("0502cdea-51c0-40bb-b008-f288461ffd7d")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Arena.ArenaExportToMxdCmd")]
    public sealed class ArenaExportToMxdCmd : BaseCommand
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
        public ArenaExportToMxdCmd()
        {
            base.m_category = "Export Arena project file"; //localizable text
            base.m_caption = "Save as ArcView Project";  //localizable text
            base.m_message = "Export Arena project file";  //localizable text 
            base.m_toolTip = "Export Arena project file";  //localizable text 
            base.m_name = "ArenaExportToMxdCmd";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")
            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                base.m_bitmap = global::ArenaToolBox.Properties.Resources.ArcMap16;
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

           // base.m_enabled = (DataCash.ProjectEnvironment!=null );

        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add ArenaExportToMxdCmd.OnClick implementation

           

            if (DataCash.ProjectEnvironment.Data.CurProjectName == null) { MessageBox.Show("Save Arena project"); return; }

            var saveFileDialog1 = new SaveFileDialog
            {
                Filter =  "ArcMap Files | *.mxd"
            };
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                LogManager.GetLogger(GetType().Name).Info($" ----------Export Arena project file---------- ");
                try
                {
                    var mdbName = ArenaStatic.ArenaStaticProc.GetTargetDB();

                    var mxdName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(mdbName), "arena_PDM.mxd");

                    var destFolder = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(saveFileDialog1.FileName), System.IO.Path.GetFileNameWithoutExtension(saveFileDialog1.FileName));
                    if (Directory.Exists(destFolder)) Directory.Delete(destFolder, true);
                    Directory.CreateDirectory(destFolder);
                    File.Copy(mxdName, System.IO.Path.Combine(destFolder, System.IO.Path.GetFileName(saveFileDialog1.FileName)));
                    File.Copy(mdbName, System.IO.Path.Combine(destFolder, System.IO.Path.GetFileName(mdbName)));

                    ArenaDataModule.ClearRelations_Indexes(System.IO.Path.Combine(destFolder, System.IO.Path.GetFileName(mdbName)));

                    
                    ArenaMessageForm msgFrm = new ArenaMessageForm("ARENA", "Projects saved", global::ArenaToolBox.Properties.Resources.ArenaMessage);
                    msgFrm.ShowDialog();
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger(ex.TargetSite.Name)
                    .Error(ex, "Error when export Arena project file");
                    ArenaMessageForm msgFrm = new ArenaMessageForm("ARENA", "Error when export Arena project file", global::ArenaToolBox.Properties.Resources.ArenaMessage);
                    msgFrm.ShowDialog();
                }
                

            }

        }

        #endregion
    }
}
