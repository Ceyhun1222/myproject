using ESRI.ArcGIS.Framework;
using System;
using System.Windows.Forms;

namespace ChartPApproachTerrain 
{
    class ArcMapWrapper : IWin32Window
    {
        private IApplication _arcMapApplication;

        public ArcMapWrapper(IApplication mApplication)
        {
            _arcMapApplication = mApplication;
        }

        public IntPtr Handle => new IntPtr(_arcMapApplication.hWnd);
    }
}
