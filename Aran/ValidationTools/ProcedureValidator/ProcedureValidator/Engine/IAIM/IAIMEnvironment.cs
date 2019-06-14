using System;
using Aran.AranEnvironment;
using Aran.PANDA.Common;
using PVT.Properties;
using System.Windows.Forms;


namespace PVT.Engine.IAIM
{
    public class IAIMEnvironment:Environment
    {
        public IAranEnvironment AranEnv { get; }
        public Aran.PANDA.Common.Settings Settings { get; private set; }

        public IAIMEnvironment(IAranEnvironment aranEnv)
        {
            AranEnv = aranEnv;
            Value = Environments.IAIM;
        }

        private bool _isInit;
        private  IntPtr _win32Window;

        public override void Initialize()
        {
            if (_isInit) return;
            Settings = new Aran.PANDA.Common.Settings();
            Settings.Load(AranEnv);
            LangCode = Settings.Language;
            NativeMethods.SetThreadLocale(LangCode);
            Resources.Culture = new System.Globalization.CultureInfo(LangCode);
            CurrentAeroport = Settings.Aeroport;
            EffectiveDate = AranEnv.CurrentAiracDateTime.Value;
            Graphics.Init();
            Graphics.CreateLayout();
            Geometry.Init();
            DbProvider.Open();
            _isInit = true;
        }


        public override void Delete()
        {
            Graphics.SetLayoutText(null);
            Graphics.DeleteLayout();
            _isInit = false;
        }

        public override IntPtr EnvWin32Window
        {
            get { return AranEnv.Win32Window.Handle; }
        }

        public override IntPtr Win32Window
        {
            get { return _win32Window; }
        }

        public void SetWin32Window(IntPtr window)
        {
            _win32Window = window;
        }
    }
}
