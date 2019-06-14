using Framework.Stasy.Context;
using Aerodrome.Features;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Framework;
using Framework.Stasy.SyncProvider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using Framework.Stasy.Helper;
using HelperDialog;
using System.Windows.Interop;
using Aerodrome.Metadata;

namespace AerodromeToolBox
{
    [Guid("C6A0BC00-C3AE-4ED9-BAA9-9526FDA0AA8B")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Aerodrome.AerodromeOpenProject")]
    public sealed class AerodromeOpenProject : BaseCommand
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
        public AerodromeOpenProject()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "Open aerodrome project file"; //localizable text
            base.m_caption = "Open Project";  //localizable text
            base.m_message = "Open aerodrome project file";  //localizable text 
            base.m_toolTip = "Open aerodrome project file";  //localizable text 
            base.m_name = "AerodromeOpenProject";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")
            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                //string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = global::ArenaToolBox.Properties.Resources.ArcReader16;



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
            var openFileDialog1 = new OpenFileDialog { Filter = @"Aerodrome type files (*.amdb)|*.amdb" };
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;


            HelperMethods.OpenAmdbProject(openFileDialog1.FileName, m_application);
           
        }

       

        #endregion
    }
}
