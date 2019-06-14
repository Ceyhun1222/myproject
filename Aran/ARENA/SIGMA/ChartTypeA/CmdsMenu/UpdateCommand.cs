using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms.Integration;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Controls;
using System.Windows.Interop;
using ARENA;
using System.Windows;
using ChartTypeA.Utils;
using ESRI.ArcGIS.Carto;

namespace ChartTypeA.CmdsMenu
{
    /// <summary>
    /// Summary description for TypeAUpdateCommand.
    /// </summary>
    [Guid("c5b6e2a3-aa82-4ae8-bbe0-8b1ac3d3fa00")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ChartTypeA.CmdsMenu.TypeAUpdateCommand")]
    public sealed class UpdateCommand : BaseCommand
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

        public UpdateCommand()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "TypeA"; //localizable text
            base.m_caption = "Activate regenerating chart";  //localizable text
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

                    var gc = GlobalParams.GrCreater;
                    gc.CenterlinePoints = GdbReader.GetRunwayCenterlineList();
                    var gridCreater = gc as Models.GridCreater;
                    gridCreater?.AssignLocalVariables();


                    GlobalParams.VerticalObstacleCreater = new Models.VerticalObstacleCreater(GdbReader.GetObstacles(), gc.VerScale, gc.HorScale,
                        gc.FrameHeight, gc.BaseElevation, gc.Pnt1 ?? gc.Pnt2);
                    Clear();
                    GlobalParams.VerticalObstacleCreater.FrameHeight = gc.FrameHeight;
                    GlobalParams.VerticalObstacleCreater?.Create();

                    gridCreater.ObstacleElements = GlobalParams.VerticalObstacleCreater?.ObstaGroupElement;

                    gridCreater?.CreateElements();
                    gridCreater?.ReCreate();
                    
                    TypeAChartPreperation.FireEvents();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }
        }

        public void Clear()
        {
            var graphicsContainer = GlobalParams.HookHelper.PageLayout as IGraphicsContainer;

            var groupElem =new[] {"ProfileElem", "ScaleElem"};

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

        #endregion
    }
}
