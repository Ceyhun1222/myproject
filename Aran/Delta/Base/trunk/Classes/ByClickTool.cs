using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Aran.AranEnvironment;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;

namespace Aran.Delta.Classes
{
    /// <summary>
    /// Summary description for ByClickTool.
    /// </summary>
    [Guid("256bb194-f608-4fbc-b610-fc352e5ddbf4")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Aran.Delta.Classes.ByClickTool")]
    public sealed class ByClickTool : BaseTool
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
        public ByClickTool()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text 
            base.m_caption = "ByClickTool";  //localizable text 
            base.m_message = "";  //localizable text
            base.m_toolTip = "";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_ArcMapTool")
            try
            {
                //
                // TODO: change resource name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_cursor = Cursors.Cross;
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                //new System.Windows.Forms.Cursor(GetType(), GetType().Name + ".cur");
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
            m_application = hook as IApplication;

            //Disable if it is not ArcMap
            if (hook is IMxApplication)
                base.m_enabled = true;
            else
                base.m_enabled = false;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add ByClickTool.OnClick implementation
        }


        public override void OnMouseDown(int button, int shift, int x, int y)
        {
            if (_aranToolStripMenuItem.MouseDownOnMap != null)
            {
                MouseButtons mouseButton = System.Windows.Forms.MouseButtons.None;
                if (button == 1)
                    mouseButton = System.Windows.Forms.MouseButtons.Left;
                else if (button == 2)
                    mouseButton = System.Windows.Forms.MouseButtons.Right;
                else if (button == 4)
                    mouseButton = System.Windows.Forms.MouseButtons.Middle;

                IPoint pt = GlobalParams.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                MapMouseEventArg arg = new MapMouseEventArg(pt.X, pt.Y, mouseButton);
                _aranToolStripMenuItem.MouseDownOnMap(this, arg);
            }
        }

        public override void OnMouseMove(int button, int shift, int x, int y)
        {
            if (_aranToolStripMenuItem.MouseMoveOnMap != null)
            {
                MouseButtons mouseButton = System.Windows.Forms.MouseButtons.None;
                if (button == 1)
                    mouseButton = System.Windows.Forms.MouseButtons.Left;
                else if (button == 2)
                    mouseButton = System.Windows.Forms.MouseButtons.Right;
                else if (button == 4)
                    mouseButton = System.Windows.Forms.MouseButtons.Middle;

                IPoint pt = GlobalParams.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                MapMouseEventArg arg = new MapMouseEventArg(pt.X, pt.Y, mouseButton);
                _aranToolStripMenuItem.MouseMoveOnMap(this, arg);
            }
        }

        public override void OnMouseUp(int button, int shift, int x, int y)
        {
            if (_aranToolStripMenuItem.MouseClickedOnMap != null)
            {
                MouseButtons mouseButton = System.Windows.Forms.MouseButtons.None;
                if (button == 1)
                    mouseButton = System.Windows.Forms.MouseButtons.Left;
                else if (button == 2)
                    mouseButton = System.Windows.Forms.MouseButtons.Right;
                else if (button == 4)
                    mouseButton = System.Windows.Forms.MouseButtons.Middle;

                IPoint pt = GlobalParams.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                MapMouseEventArg arg = new MapMouseEventArg(pt.X, pt.Y, mouseButton);
                _aranToolStripMenuItem.MouseClickedOnMap(this, arg);
            }
        }

        public override void OnDblClick()
        {
            _aranToolStripMenuItem.MouseOnDblClickOnMap?.Invoke();
        }

        public void OnRightlClick()
        {
            _aranToolStripMenuItem.MouseOnRightClickOnMap?.Invoke();
        }

        public void SetAranTool(AranTool aranTool)
        {
            _aranToolStripMenuItem = aranTool;
        }

        private AranTool _aranToolStripMenuItem;
        #endregion
    }
}
