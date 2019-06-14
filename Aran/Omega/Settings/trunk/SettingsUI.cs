using System.Collections.Generic;
using Aran.AranEnvironment;
using System;
using System.Windows;

namespace Aran.Omega.SettingsUI
{
    public static class SettingsGlobals
    {
        public static IAranEnvironment AranEnvironment { get; set; }
    }

    public class SettingsUI : ISettingsPlugin
    {
        public void Startup(IAranEnvironment aranEnv)
        {
            try
            {
                SettingsGlobals.AranEnvironment = aranEnv;
                SettingsForm form = new SettingsForm();
                aranEnv.AranUI.AddSettingsPage(new Guid[] {new Guid("0eaf1c3f-951e-4246-a6aa-5f55a9489163")}, form);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + " " + e.StackTrace);
            }
        }
    }
}
