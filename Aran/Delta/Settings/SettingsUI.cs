using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.AranEnvironment;

namespace Aran.Delta.Settings
{
    public static class SettingsGlobals
    {
        public static IAranEnvironment AranEnvironment { get; set; }
    }

    public class SettingsUI : ISettingsPlugin
    {
        public void Startup(IAranEnvironment aranEnv)
        {
            SettingsGlobals.AranEnvironment = aranEnv;
            var form = new SettingsPage();
            aranEnv.AranUI.AddSettingsPage(new Guid[] { new Guid("d86ab787-4232-422c-b2ec-8f155b936c7b") }, form);
            
            //SettingsForm form = new SettingsForm ();
            //aranEnv.AranUI.AddSettingsPage (form);
        }

        public string Name
        {
            get { return "Delta"; }
        }

        public void AddChildSubMenu(List<string> hierarcy)
        {
        }
    }

}
