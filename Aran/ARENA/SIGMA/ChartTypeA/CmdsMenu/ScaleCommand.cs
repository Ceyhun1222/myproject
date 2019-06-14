using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChartTypeA.Models;
using ChartTypeA.Utils;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geometry;

namespace ChartTypeA.CmdsMenu
{
    /// <summary>
    /// Summary description for TypeAUpdateCommand.
    /// </summary>
    [Guid("c5b6e2a3-aa82-4ae8-bbe0-8b1ac4d3fa00")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ChartTypeA.CmdsMenu.TypeAScaleCommand")]
    public sealed class TypeAScaleCommand : BaseCommand
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
        private HookHelperClass m_hookHelper;

        public TypeAScaleCommand()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "TypeA"; //localizable text
            base.m_caption = "Generate Scale";  //localizable text
            base.m_message = "";  //localizable text 
            base.m_toolTip = "";  //localizable text 
            base.m_name = "Type A";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                //string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = global::ChartTypeA.Properties.Resources.Sigma;

                //base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
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
                if (GlobalParams.GrCreater == null)
                {
                    MessageBox.Show("It is not Type A Chart!");
                    return;
                }

                if (InitChartTypeA.InitCommand())
                {
                    var savedHeightUnitConverter = InitChartTypeA.HeightConverterList.FirstOrDefault(
                        heightConverTer => heightConverTer.Unit == GlobalParams.TypeAExtension.HeightUnit);
                    if (savedHeightUnitConverter.Unit != null)
                        InitChartTypeA.HeightConverter = savedHeightUnitConverter;

                    var gc = GlobalParams.GrCreater as Models.GridCreater;
                    
                    Clear();

                    ScaleElement scaleElement = new ScaleElement((IGraphicsContainer)GlobalParams.HookHelper.PageLayout,100,new LineElementCreater(100));    
                    var pt = new PointClass();
                    pt.PutCoords(40,30);
                    scaleElement.CreateScale(pt,GlobalParams.HookHelper.FocusMap.MapScale / 10);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }
        }

        public bool InitalizeExtension()
        {
            try
            {
                if (GlobalParams.TypeAExtension == null)
                {
                    var pID = new ESRI.ArcGIS.esriSystem.UID();
                    pID.Value = "ChartTypeA.CmdsMenu.TypeAExtension";
                    GlobalParams.TypeAExtension = GlobalParams.Application.FindExtensionByCLSID(pID) as Extension;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return GlobalParams.TypeAExtension != null;
        }

        public void Clear()
        {
            var graphicsContainer = GlobalParams.HookHelper.PageLayout as IGraphicsContainer;

            var groupElem = new[] {"ScaleElem" };

            if (graphicsContainer != null)
            {
                graphicsContainer.Reset();
                var element = graphicsContainer.Next();

                while (element != null)
                {
                    var elemProperties = element as IElementProperties;

                    if (elemProperties != null && groupElem.Contains(elemProperties.Name))
                        graphicsContainer.DeleteElement(element);

                    element = graphicsContainer.Next();
                }
            }
        }

        #endregion
    }
}
