
using Aerodrome.DB;
using Aerodrome.Metadata;
using Aerodrome.Features;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Framework;
using Framework.Stasy.Context;
using Framework.Stasy.SyncProvider;
using HelperDialog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Windows.Interop;

namespace AerodromeToolBox
{
    [Guid("9B2F071C-7DDD-48AC-8F75-119EC2C4773A")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Aerodrome.AerodromeSaveProject")]
    public sealed class AerodromeSaveProject : BaseCommand
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

        public AerodromeSaveProject()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "Save aerodrome project file"; //localizable text
            base.m_caption = "Save Project";  //localizable text
            base.m_message = "Save aerodrome project file";  //localizable text 
            base.m_toolTip = "Save aerodrome project file";  //localizable text 
            base.m_name = "AerodromeSaveProject";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                //base.m_bitmap = global::ArenaToolBox.Properties.Resources.Analyze16;
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

            System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show("Save changes to the AMDB project?", "Aerodrome", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Question);
            if (result == System.Windows.MessageBoxResult.Cancel)
            {
                return;
            }
            HelperMethods.SaveAmdbProject(m_application, showSplash: true);

            MessageScreen messageScreen = new MessageScreen();
            var parentHandle = new IntPtr(m_application.hWnd);
            var messageScreeenHelper = new WindowInteropHelper(messageScreen) { Owner = parentHandle };
            messageScreen.MessageText = "Succesfully saved";
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
