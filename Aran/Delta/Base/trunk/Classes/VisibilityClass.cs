using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using Aran.Delta.Settings;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using System.Collections.ObjectModel;
using VisibilityTool;
using VisibilityTool.Model;

namespace Aran.Delta.Classes
{
    /// <summary>
    /// Summary description for VisibilityClass.
    /// </summary>
    [Guid("83123f95-a0c9-47a2-b006-549dda3f873a")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Aran.Delta.Classes.VisibilityClass")]
    public sealed class VisibilityClass : BaseCommand
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
        private IHookHelper m_hookHelper;
        public VisibilityClass()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "Visibility";  //localizable text
            base.m_message = "";  //localizable text 
            base.m_toolTip = "";  //localizable text 
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

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
            GlobalParams.Application = m_application;

            m_hookHelper = new HookHelperClass();
            m_hookHelper.Hook = hook;

            GlobalParams.HookHelper = m_hookHelper;
            if (m_application != null) GlobalParams.HWND = m_application.hWnd;

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
            try
            {
                var visibilityForm = new VisibilityTool.MainWindow();
                Win32Windows win32Window = new Win32Windows(GlobalParams.HWND);

                var helper = new WindowInteropHelper(visibilityForm);
                helper.Owner = new IntPtr(GlobalParams.HWND);
                //visibilityForm.SetHookHelper(m_hookHelper);
                ElementHost.EnableModelessKeyboardInterop(visibilityForm);
                visibilityForm.ShowInTaskbar = false; // hide from taskbar and alt-tab list

                ObservableCollection<LayerTemplate> dataTemplates = new ObservableCollection<LayerTemplate>();
                LayerTemplate layTempAirspace_C = new LayerTemplate();
                layTempAirspace_C.Name = "AirspaceVolume";
                layTempAirspace_C.IdField = "FeatureGUID";
                layTempAirspace_C.DescriptField = "txtLocalType";
                layTempAirspace_C.GroupByField = "codeType";
                layTempAirspace_C.PrimaryTableName = "AirspaceVolume";
                dataTemplates.Add(layTempAirspace_C);

                LayerTemplate layTempDsgPnt_C = new LayerTemplate();
                layTempDsgPnt_C.Name = "RouteSegment";
                layTempDsgPnt_C.IdField = "FeatureGUID";
                layTempDsgPnt_C.DescriptField = "designator";
                //layTempDsgPnt_C.GroupByField = "RouteSegment";
                layTempDsgPnt_C.PrimaryTableName = "RouteSegment";
                dataTemplates.Add(layTempDsgPnt_C);

                LayerTemplate layTempDesignin_Area = new LayerTemplate();
                layTempDesignin_Area.Name = "Designing Area";
                layTempDesignin_Area.IdField = "FeatIdentifier";
                layTempDesignin_Area.DescriptField = "NAME";
                //layTempDsgPnt_C.GroupByField = "RouteSegment";
                layTempDesignin_Area.PrimaryTableName = "Designing_Area";
                dataTemplates.Add(layTempDesignin_Area);

                LayerTemplate layTempDesigning_Point = new LayerTemplate();
                layTempDesigning_Point.Name = "Designing Point";
                layTempDesigning_Point.IdField = "FeatIdentifier";
                layTempDesigning_Point.DescriptField = "NAME";
                //layTempDsgPnt_C.GroupByField = "RouteSegment";
                layTempDesigning_Point.PrimaryTableName = "Designing_Point";
                dataTemplates.Add(layTempDesigning_Point);

                LayerTemplate layTempDesigning_Buffer = new LayerTemplate();
                layTempDesigning_Buffer.Name = "Designing Buffer";
                layTempDesigning_Buffer.IdField = "FeatIdentifier";
                layTempDesigning_Buffer.DescriptField = "NAME";
                //layTempDsgPnt_C.GroupByField = "RouteSegment";
                layTempDesigning_Buffer.PrimaryTableName = "Designing_Buffer";
                dataTemplates.Add(layTempDesigning_Buffer);

                LayerTemplate layTempDesigning_Segment = new LayerTemplate();
                layTempDesigning_Segment.Name = "Designing Segment";
                layTempDesigning_Segment.IdField = "FeatIdentifier";
                layTempDesigning_Segment.DescriptField = "NAME";
                //layTempDsgPnt_C.GroupByField = "RouteSegment";
                layTempDesigning_Segment.PrimaryTableName = "Designing_Segment";
                dataTemplates.Add(layTempDesigning_Segment);

                LayerTemplate layTempWayPoint = new LayerTemplate();
                layTempWayPoint.Name = "WayPoint";
                layTempWayPoint.IdField = "FeatureGUID";
                layTempWayPoint.DescriptField = "designator";
                //layTempDsgPnt_C.GroupByField = "RouteSegment";
                layTempWayPoint.PrimaryTableName = "WayPoint";
                dataTemplates.Add(layTempWayPoint);


                //LayerTemplate layTempNavaid_C = new LayerTemplate();
                //layTempNavaid_C.Name = "Navaid";
                //layTempNavaid_C.IdField = "FeatureGUID";
                //layTempNavaid_C.DescriptField = "name";
                //layTempNavaid_C.GroupByField = "Nav_Type";
                //layTempNavaid_C.PrimaryTableName = "Navaids_C";
                //dataTemplates.Add(layTempAirspace_C);

                visibilityForm.SetData(m_hookHelper, dataTemplates);
                visibilityForm.Show();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        #endregion
    }
}
