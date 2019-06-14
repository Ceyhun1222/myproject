using Aran.PANDA.Common;
using System;
using System.Windows.Forms;

namespace Aran.Panda.RNAV.RNPAR.Context
{
    class SystemContext
    {
        private AppEnvironment _environment;
        public String ConstantDirectory { get; }
        public SystemContext(AppEnvironment environment)
        {
            _environment = environment;

            bool isExists;
            ConstantDirectory = RegFuncs.GetConstantsDir(out isExists);

            if (!isExists)
                throw new Exception("Installation Path Not Exists.");
        }

        public IntPtr EnvWin32Window => _environment.AranEnv.Win32Window.Handle;

        public IWin32Window Win32Window { get; private set; }

        public void SetWin32Window(IWin32Window window)
        {
            Win32Window = window;
        }

        public  int WM_SYSCOMMAND => 0x112;
        public int MF_STRING => 0x0;
        public int MF_SEPARATOR => 0x800;
        public int SYSMENU_ABOUT_ID => 0x1;
    }
}
