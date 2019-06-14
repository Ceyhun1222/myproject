using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Aran.AranEnvironment;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;

namespace Aran.Panda.RadarMA
{
    /// <summary>
    /// Summary description for Tool1.
    /// </summary>
    [Guid("f5f6681a-1cbe-4129-925e-d9247705cbe8")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Aran.Panda.RadarMA.DrawLineTool")]
    public sealed class DrawLineTool : BaseTool
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

        public DrawLineTool()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text 
            base.m_caption = "";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            try
            {
                //
                // TODO: change resource name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_cursor = Cursors.Cross;
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
               //  new System.Windows.Forms.Cursor(GetType(), GetType().Name + ".cur");
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
            // TODO: Add Tool1.OnClick implementation
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
            if (_aranToolStripMenuItem.MouseOnDblClickOnMap != null)
                _aranToolStripMenuItem.MouseOnDblClickOnMap();
        }

        public void SetAranTool(AranTool aranTool)
        {
            _aranToolStripMenuItem = aranTool;
        }

        private AranTool _aranToolStripMenuItem;
        #endregion
    }
}
