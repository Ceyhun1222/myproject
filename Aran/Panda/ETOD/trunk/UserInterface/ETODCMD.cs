using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;

using ETOD.Properties;
using ETOD.Forms;

namespace ETOD
{
	/// <summary>
	/// Summary description for TerrainAndObstacleCMD.
	/// </summary>
	[Guid("EED32423-A683-408D-9333-38B8AA74D539")]
	[ClassInterface(ClassInterfaceType.None)]
	[ProgId("ETOD.TerrainAndObstacleCMD")]

	public sealed class TerrainAndObstacleCMD : BaseCommand
	{
		#region COM Registration Function(s)
		[ComRegisterFunction()]
		[ComVisible(false)]
		static void RegisterFunction(Type registerType)
		{
			// Required for ArcGIS Component Category Registrar support
			ArcGISCategoryRegistration(registerType);
		}

		[ComUnregisterFunction()]
		[ComVisible(false)]
		static void UnregisterFunction(Type registerType)
		{
			// Required for ArcGIS Component Category Registrar support
			ArcGISCategoryUnregistration(registerType);
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

		public TerrainAndObstacleCMD()
		{
			Functions.SetThreadLocaleByConfig();
			Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);

			base.m_category = "ETOD.ETODMenu";	//  localizable text 
			base.m_name = "TerrainAndObstacleCMD";			//	unique id, non-localizable

			base.m_helpFile = GlobalVars.HelpFile;
			base.m_helpID = 0;
			base.m_bitmap = null;
			base.m_caption = Resources.Str3;		//"ETOD"
			base.m_message = Resources.Str4;		//"ETOD"
			base.m_toolTip = Resources.Str4;		//"ETOD"
		}

		#region Overriden Class Methods

		/// <summary>
		/// Occurs when this command is created
		/// </summary>
		/// <param name="hook">Instance of the application</param>
		public override void OnCreate(object hook)
		{
			base.m_enabled = false;
			if (hook == null)
				return;

			if (GlobalVars.gHookHelper == null)
				GlobalVars.gHookHelper = new ESRI.ArcGIS.Controls.HookHelper();

			if (hook is IMxApplication)
			{
				GlobalVars.gHookHelper.Hook = hook;
				base.m_enabled = true;
			}
			else if (hook is Aran.AranEnvironment.IAranEnvironment)
			{
				GlobalVars.gAranEnv = (Aran.AranEnvironment.IAranEnvironment)hook;
				GlobalVars.gHookHelper.Hook = GlobalVars.gAranEnv.HookObject;
				base.m_enabled = true;
			}
		}

		public override string Caption
		{
			get
			{
				Functions.SetThreadLocaleByConfig();
				Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);
				return Resources.Str3;				//	"ETOD"
			}
		}

		public override string Message
		{
			get
			{
				Functions.SetThreadLocaleByConfig();
				Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);

				return Resources.Str4;	//"ETOD"
			}
		}

		public override string Tooltip
		{
			get
			{
				Functions.SetThreadLocaleByConfig();
				Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);

				return Resources.Str4;	//"ETOD"
			}
		}

		public override bool Checked
		{
			get
			{
				return GlobalVars.CurrCmd == 1;
			}
		}

		public override bool Enabled
		{
			get
			{
				return m_enabled && ((GlobalVars.CurrCmd == 0) || (GlobalVars.CurrCmd == 1));
			}
		}

		/// <summary>
		/// Occurs when this command is clicked
		/// </summary>
		public override void OnClick()
		{
			if (GlobalVars.CurrCmd == 1)
			    return;

			GlobalVars.CurrCmd = 0;

			try
			{
				GlobalVars.InitCommand();

				CTerrainAndObstacleFrm TerrainAndObstacleFrm = new CTerrainAndObstacleFrm();
				TerrainAndObstacleFrm.Show(GlobalVars.m_Win32Window);
				//TerrainAndObstacleFrm.ComboBox001.Focus();
				GlobalVars.CurrCmd = 1;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, this.Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion
	}
}
