using System;
using System.Linq;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using EsriWorkEnvironment;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using System.Collections.Generic;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Display;
using ANCOR.MapElements;
using DataModule;
using ARENA.Enums_Const;
using ArenaStatic;
using ESRI.ArcGIS.ArcMapUI;
using ANCOR.MapCore;

namespace SigmaChart.CmdsMenu
{
    /// <summary>
    /// Summary description for ChartElementSelector.
    /// </summary>
    [Guid("34d963fb-a9d7-4c2d-a779-a4babb2b8afa")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.ChartColorSelector")]
    public sealed class ChartColorSelector : BaseTool
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
        public ChartColorSelector()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "SigmaChart"; //localizable text
            base.m_caption = "select Color";  //localizable text 
            base.m_message = "select Color";  //localizable text
            base.m_toolTip = "Sigma Chart";  //localizable text
            base.m_name = "Sigma Chart";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")
            try
            {
                //
                // TODO: change resource name if necessary 
                //
                base.m_bitmap = global::SigmaChart.Properties.Resources.EyeDropper16;
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), GetType().Name + ".cur");

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                {
                    m_hookHelper = null;
                }

                m_application = hook as IApplication;
            }
            catch
            {
                m_hookHelper = null;
            }

            if (m_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add ChartElementSelector.OnClick implementation

        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            //IHookHelper2 m_hookHelper2 = (IHookHelper2)m_hookHelper;

        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {

            
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            ArenaStaticProc.BringToFrontToc((IMxDocument)m_application.Document, "ANCORTOCLayerView");
            string ActiveFrameName = ChartElementsManipulator.GetActiveFrameName(m_hookHelper.ActiveView.FocusMap, m_hookHelper);
            //SigmaDataCash.PutOutPropertyGrid();

            AncorColorselector ancCoplor = new AncorColorselector();
            ancCoplor.SelectedColor = new AncorColor(255, 255, 255);

            if (SigmaDataCash.AncorPropertyGrid.SelectedObject is AncorColorselector)
            {
                ancCoplor = (AncorColorselector)SigmaDataCash.AncorPropertyGrid.SelectedObject;
            }

            if (Shift == 1)
            {
                SigmaDataCash.MultiSelectFlag = false;
                IMxDocument document = (IMxDocument)m_application.Document;
                IMaps maps = document.Maps;
                for (int i = 0; i <= maps.Count - 1; i++)
                {
                    IMap map = maps.get_Item(i);
                    ActiveFrameName = ChartElementsManipulator.GetActiveFrameName(map, m_hookHelper);
                    if (ChartElementsManipulator.GetClickedChartElement(map, X, Y, ActiveFrameName, false, false) != null)
                    {



                        switch (ancCoplor.ScrolList)
                        {
                            case Scroll.FillColor:
                                if (ArenaStaticProc.HasProperty(SigmaDataCash.SelectedChartElements[0], "FillColor"))
                                {
                                    if (ArenaStaticProc.HasProperty(SigmaDataCash.SelectedChartElements[0], "FillStyle"))
                                    {
                                        ArenaStaticProc.SetObjectValue(SigmaDataCash.SelectedChartElements[0], "FillStyle", fillStyle.fSSolid);
                                    }
                                        ArenaStaticProc.SetObjectValue(SigmaDataCash.SelectedChartElements[0], "FillColor", ancCoplor.SelectedColor);
                                }

                                break;

                            case Scroll.FrameColor: 
                                if (ArenaStaticProc.HasProperty(SigmaDataCash.SelectedChartElements[0], "Border"))
                                {
                                    ArenaStaticProc.SetObjectValue(((ChartElement_BorderedText)SigmaDataCash.SelectedChartElements[0]).Border, "FrameColor", ancCoplor.SelectedColor);
                                }
                                if (ArenaStaticProc.HasProperty(SigmaDataCash.SelectedChartElements[0], "Frame"))
                                {
                                    ArenaStaticProc.SetObjectValue(((ChartElement_SigmaCollout)SigmaDataCash.SelectedChartElements[0]).Frame, "FrameColor", ancCoplor.SelectedColor);
                                }
                                if (ArenaStaticProc.HasProperty(SigmaDataCash.SelectedChartElements[0], "MarkerFrameColor"))
                                {
                                    ArenaStaticProc.SetObjectValue(((ChartElement_MarkerSymbol)SigmaDataCash.SelectedChartElements[0]), "MarkerFrameColor", ancCoplor.SelectedColor);
                                }
                                break;
                            default:
                                break;
                        }

                        SigmaDataCash.AncorPropertyGrid.SelectedObject = SigmaDataCash.SelectedChartElements[0];
                        UpdateElement(SigmaDataCash.SelectedChartElements[0]);


                        ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);
                    }

                }

                
            }
            else
            {

                Color color = ChartElementsManipulator.GetPixelColor((IntPtr)m_application.hWnd);
                ancCoplor = new AncorColorselector();
                ancCoplor.SelectedColor = new AncorColor(color.R, color.G, color.B);
            }

            SigmaDataCash.AncorPropertyGrid.SelectedObject = ancCoplor;

        }

        private void UpdateElement(AbstractChartElement obj)
        {


            SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
            SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

            AbstractChartElement cartoEl = (AbstractChartElement)obj;
            IElement iEl = cartoEl.ConvertToIElement() as IElement;

            if (cartoEl is ChartElement_SimpleText)
                ChartElementsManipulator.UpdateSingleElementToDataSet(cartoEl.Name, cartoEl.Id.ToString(), iEl, ref cartoEl, false);


            SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
            SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);


        }






        #endregion
    }
}
