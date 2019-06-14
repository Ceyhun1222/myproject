
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Display;

namespace SigmaLayoutpageClip
{
    /// <summary>
    /// Summary description for LayoutpageClip.
    /// </summary>
    [Guid("3960d84c-a88b-4f53-8098-1cbcea842363")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("LayoutPageClip.LayoutpageClip")]
    public sealed class SigmaLayoutpageClip : BaseTool
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
        //private IApplication m_application;


        public SigmaLayoutpageClip()
        {
            try
            {
                base.m_category = "SigmaChart"; //localizable text
                base.m_caption = "Layout View Clip";  //localizable text 
                base.m_message = "Clip the layout view using the selected element";  //localizable text
                base.m_toolTip = "Layout View Clip";  //localizable text
                base.m_name = "LayoutViewClip";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")
                try
                {
                    //
                    // TODO: change resource name if necessary 
                    //
                    base.m_bitmap = global::SigmaLayoutpageClip.Properties.Resources.LayoutClipTool;
                    base.m_cursor = System.Windows.Forms.Cursors.Cross;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
                }
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
            }
            catch
            {
                m_hookHelper = null;
            }

            if (m_hookHelper == null )
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
            // TODO: Add LayoutpageClip.OnClick implementation
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add LayoutpageClip.OnMouseDown implementation
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add LayoutpageClip.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            int pointX = X;
            int pointY = Y;
            IPoint pPoint = (m_hookHelper.ActiveView as IActiveView).ScreenDisplay.DisplayTransformation.ToMapPoint(pointX, pointY);

            var pageGraphicsContainer = m_hookHelper.ActiveView as IGraphicsContainer;

            var clipOptions = m_hookHelper.FocusMap as IMapClipOptions;
            if (clipOptions.ClipType == esriMapClipType.esriMapClipNone)
                clipOptions.ClipType = esriMapClipType.esriMapClipShape;
            clipOptions.ClipGridAndGraticules = true;

            //Проверка выбранного элемента
            pageGraphicsContainer.Reset();
            IEnumElement selectElemEnum = pageGraphicsContainer.LocateElements(pPoint, 0);
            IElement selectedElem;
            if (selectElemEnum == null)
            {
                return;
            }
            selectElemEnum.Reset();
            selectedElem = selectElemEnum.Next();

            if (!(selectedElem is IPictureElement))
            {
                return;
            }

            //Layout View envelope в IGeometry
            var mapFrame = pageGraphicsContainer.FindFrame(m_hookHelper.ActiveView.FocusMap) as IMapFrame;
            var extent = mapFrame.MapBounds.Envelope;
            ISegmentCollection sc = new PolygonClass() as ISegmentCollection;
            sc.SetRectangle(extent);
            var layoutPolygon = (IGeometry)sc;

            //Добавление всех clipped элементов на Layout View geometry
            pageGraphicsContainer.Reset();
            var clippedGeom = pageGraphicsContainer.Next();
            while ((clippedGeom != null) && (clippedGeom is IPictureElement))
            {
                IElementProperties3 clippedElemProp = (IElementProperties3)clippedGeom;
                if (clippedElemProp.Name.ToString() == "Clipped")
                {
                    ITopologicalOperator clippedTopoOperator = (ITopologicalOperator)layoutPolygon;

                    var clippedRectPtColl = clippedGeom.Geometry as IPointCollection;
                    IPointCollection clippedResultPtColl = new PolygonClass();

                    int o, p;
                    for (int i = 0; i < clippedRectPtColl.PointCount; i++)
                    {
                        (m_hookHelper.ActiveView as IActiveView).ScreenDisplay.DisplayTransformation.FromMapPoint(clippedRectPtColl.Point[i], out o, out p);

                        var mapPoint = (m_hookHelper.ActiveView.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation.ToMapPoint(o, p);
                        clippedResultPtColl.AddPoint(mapPoint);
                    }

                    layoutPolygon = clippedTopoOperator.Difference((IGeometry)clippedResultPtColl);
                }
                clippedGeom = pageGraphicsContainer.Next();

            }


            //Из layout коорд. в географ. коорд.
            var rectPtColl = selectedElem.Geometry as IPointCollection;
            IPointCollection rectresultPtColl = new PolygonClass();

            int x, y;
            for (int i = 0; i < rectPtColl.PointCount; i++)
            {
                (m_hookHelper.ActiveView as IActiveView).ScreenDisplay.DisplayTransformation.FromMapPoint(rectPtColl.Point[i], out x, out y);

                var mapPoint = (m_hookHelper.ActiveView.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                rectresultPtColl.AddPoint(mapPoint);
            }


            ITopologicalOperator pTopoOperator = (ITopologicalOperator)layoutPolygon;

            extent = ((IGeometry)rectresultPtColl).Envelope;
            sc = new PolygonClass() as ISegmentCollection;
            sc.SetRectangle(extent);
            var clipPolygon = (IGeometry)sc;

            //Проверка статуса элемента(Clipped или нет)
            IElementProperties3 elemProp = (IElementProperties3)selectedElem;

            if (elemProp.Name.ToString() == "")
            {

                var result = pTopoOperator.Difference(clipPolygon);

                m_hookHelper.ActiveView.FocusMap.ClipGeometry = result;

                elemProp.Name = "Clipped";

                m_hookHelper.ActiveView.Refresh();

            }
            else
            {
                var result = pTopoOperator.Union(clipPolygon);

                m_hookHelper.ActiveView.FocusMap.ClipGeometry = result;

                elemProp.Name = "";

                m_hookHelper.ActiveView.Refresh();

            }

            int clippedCount = 0;
            pageGraphicsContainer.Reset();
            var clipped = pageGraphicsContainer.Next();
            while ((clipped != null) && (clipped is IPictureElement))
            {
                IElementProperties3 clippedElemProp = (IElementProperties3)clipped;
                if (clippedElemProp.Name.ToString() == "Clipped")
                {
                    clippedCount++;
                }
                clipped = pageGraphicsContainer.Next();
            }

            if (clippedCount == 0)
            {
                clipOptions.ClipType = esriMapClipType.esriMapClipNone;
            }

           // ArcMap.Application.CurrentTool = previousCommand;
        }
        #endregion
    }
}
