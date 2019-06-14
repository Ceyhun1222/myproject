using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ARENA;
using System.Collections.Generic;
using System.IO;
using ANCOR.MapElements;
using ANCOR.MapCore;
using System.Xml.Serialization;
using ANCOR;
using System.Linq;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using EsriWorkEnvironment;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ArenaStatic;
using SigmaChart.CmdsMenu;
using PDM;
using System.Collections;
using AranSupport;
using DataModule;
using ARENA.Enums_Const;
using System.Reflection;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace SigmaChart
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("236ad5a1-0513-4963-9996-9579f42ca452")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.AerodromeElectronicChart")]
    public sealed class AerodromeElectronicChart : BaseCommand
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

        public AerodromeElectronicChart()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "SigmaChart"; //localizable text
            base.m_caption = "Electronic Chart";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "Aerodrome Electonic Chart";  //localizable text
            base.m_name = "NewAerodromeElectronicChart";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

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

        public override void OnCreate(object hook)
        {
            if (hook == null)
                return;

            m_application = hook as IApplication;



            m_hookHelper = new HookHelperClass();
            m_hookHelper.Hook = hook;

            
        }
        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            if (((IMapDocument)m_application.Document).DocumentFilename.Length > 0)
            {
                string _DocumentFilename = System.IO.Path.GetDirectoryName(((IMapDocument)m_application.Document).DocumentFilename);

                if (!TerminalChartsUtil.CheckPdmProjectType(SigmaChartTypes.AerodromeElectronicChart, _DocumentFilename)) return;
            }
            AerodromeCharCreator.CreateAerodromeTypeChart(m_application, m_hookHelper, (int)SigmaChartTypes.AerodromeElectronicChart);


        }

         

        #endregion


       
    }
}
