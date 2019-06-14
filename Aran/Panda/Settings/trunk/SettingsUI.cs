using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.AranEnvironment;

namespace Aran.PANDA.SettingsUI
{
	public static class SettingsGlobals
	{
        public static IAranEnvironment AranEnvironment { get; set; }
	}

	public class SettingPlugin : ISettingsPlugin
	{
		public void Startup(IAranEnvironment aranEnv)
		{
			SettingsGlobals.AranEnvironment = aranEnv;

			SettingsForm form = new SettingsForm();
			aranEnv.AranUI.AddSettingsPage(new Guid[] {
                    new Guid("d548ff47-74c9-49ab-aa9a-80161ed94a27"),
                    new Guid("c3860b1d-89c8-4fc5-9477-c105d223e775"),
                    new Guid("8a709113-d7ca-443a-8648-f8fc8ad33200"),
					new Guid("4412c852-98b4-4eba-ac3b-7b9c6c8315d8"),
					new Guid("2B0DAED0-FDC5-4225-BF19-3EFEF7653276")},	//MapEnvServiceLoader
				form);
		}
	}
}
