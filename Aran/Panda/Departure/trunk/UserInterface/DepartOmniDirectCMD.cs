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
	[Guid("1A118700-05FF-4b50-902B-172D5E715251")]
	[ClassInterface(ClassInterfaceType.None)]
#if (ADD_NET)
	[ProgId("Departure_NET.DepartOmniDirectCMD")]
#else
	[ProgId("Departure.DepartOmniDirectCMD")]
#endif

	public sealed class DepartOmniDirectCMD : BaseCommand
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

		public DepartOmniDirectCMD()
		{
			Functions.SetThreadLocaleByConfig();
			Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);

#if (ADD_NET)
			base.m_category = "PANDA_NET.DepartureMenu";	//  localizable text 
			base.m_name = "DepartOmniDirectCMD_NET";			//	unique id, non-localizable
#else
			base.m_category = "PANDA.DepartureMenu";		//  localizable text 
			base.m_name = "DepartOmniDirectCMD";			//	unique id, non-localizable
#endif
			base.m_helpFile = GlobalVars.HelpFile;
			base.m_helpID = 2000;
			base.m_bitmap = null;
			base.m_caption = Resources.str15063;		//"� ����� �����������"
			base.m_message = Resources.str15066;		//"����� �� ���� ������������"
			base.m_toolTip = Resources.str15066;		//"����� �� ���� ������������"
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
				return Resources.str15063 + "_NET";		//	"� ����� �����������"
#else
				return Resources.str15063;				//	"� ����� �����������"
#endif
			}
		}

		public override string Message
		{
			get
			{
				Functions.SetThreadLocaleByConfig();
				Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);

				return Resources.str15066;	//"����� �� ���� ������������"
			}
		}

		public override string Tooltip
		{
			get
			{
				Functions.SetThreadLocaleByConfig();
				Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);

				return Resources.str15066;	//"����� �� ���� ������������"
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

				CDepartOmniDirect DepartOmniDirectFrm = new CDepartOmniDirect();
				DepartOmniDirectFrm.Show(GlobalVars.Win32Window);
				DepartOmniDirectFrm.ComboBox001.Focus();
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
