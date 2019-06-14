using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Aran.PANDA.Departure.Properties;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ArcMapUI;

namespace Aran.PANDA.Departure
{
	/// <summary>
	/// Summary description for Command1.
	/// </summary>
	[Guid("1A118701-05FF-4b50-902B-172D5E715251")]
	[ClassInterface(ClassInterfaceType.None)]
#if (ADD_NET)
	[ProgId("Departure_NET.DepartRoutsCMD")]
#else
	[ProgId("Departure.DepartRoutsCMD")]
#endif

	public sealed class DepartRoutsCMD : BaseCommand
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

		public DepartRoutsCMD()
		{
			Functions.SetThreadLocaleByConfig();
			Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);

#if (ADD_NET)
			base.m_category = "PANDA_NET.DepartureMenu";	//  localizable text 
			base.m_name = "DepartRoutsCMD_NET";			//	unique id, non-localizable
#else
			base.m_category = "PANDA.DepartureMenu";		//  localizable text 
			base.m_name = "DepartRoutsCMD";			//	unique id, non-localizable
#endif
			base.m_helpFile = GlobalVars.HelpFile;
			base.m_helpID = 4000;
			base.m_bitmap = null;
			base.m_caption = Resources.str15270;		//"��� ���������"
			base.m_message = Resources.str15271;		//"����� ��� ���������"
			base.m_toolTip = Resources.str15271;		//"����� ��� ���������"
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
#if (ADD_NET)
				return Resources.str15270 + "_NET";		//	"��� ���������"
#else
				return Resources.str15270;				//	"��� ���������"
#endif
			}
		}

		public override string Message
		{
			get
			{
				Functions.SetThreadLocaleByConfig();
				Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);

				return Resources.str15271;	//"����� ��� ���������"
			}
		}

		public override string Tooltip
		{
			get
			{
				Functions.SetThreadLocaleByConfig();
				Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);

				return Resources.str15271;	//"����� ��� ���������"
			}
		}

		public override bool Checked
		{
			get
			{
				return GlobalVars.CurrCmd == 2;
			}
		}

		public override bool Enabled
		{
			get
			{
				return m_enabled && ((GlobalVars.CurrCmd == 0) || (GlobalVars.CurrCmd == 2));
			}
		}

		/// <summary>
		/// Occurs when this command is clicked
		/// </summary>
		public override void OnClick()
		{
			if (GlobalVars.CurrCmd == 2)
				return;

			GlobalVars.CurrCmd = 0;

			try
			{
				GlobalVars.InitCommand();

				CDepartRouts DepartRoutsFrm = new CDepartRouts();
				DepartRoutsFrm.Show(GlobalVars.Win32Window);
				DepartRoutsFrm.ComboBox001.Focus();
				GlobalVars.CurrCmd = 2;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, this.Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion
	}
}
