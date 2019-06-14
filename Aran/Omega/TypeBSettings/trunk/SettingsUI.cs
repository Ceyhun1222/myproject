using System.Collections.Generic;
using Aran.AranEnvironment;
using System;

namespace Aran.Omega.TypeB.Settings
{
    public static class SettingsGlobals
    {
        private static IAranEnvironment _aranEnv = null;
       // private static IPandaAranExtension _aranExtension = null;

        public static IAranEnvironment AranEnvironment
        {
            get { return _aranEnv; }
            set { _aranEnv = value; }
        }

        //public static IPandaAranExtension PandaAranExtension
        //{
        //    get { return _aranExtension; }
        //    set { _aranExtension = value; }
        //}
    }

    public class SettingsUI : ISettingsPlugin
    {
        public void Startup(IAranEnvironment aranEnv)
        {
            SettingsGlobals.AranEnvironment = aranEnv;
            //SettingsGlobals.PandaAranExtension = aranEnv.PandaAranExt;
            SettingsForm form = new SettingsForm();
          //  aranEnv.AranUI.AddSettingsPage(new Guid[] { new Guid("0eaf1c3f-951e-4246-a6aa-5f55a9489163") }, form);
        }
    }
}
