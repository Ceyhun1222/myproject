using System;
using System.Runtime.InteropServices;
using Aran.PANDA.Departure.Properties;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ArcMapUI;

namespace Aran.PANDA.Departure
{
	/// <summary>
	/// Summary description for Command1.
	/// </summary>
	[Guid("1A118717-05FF-4b50-902B-172D5E715251")]
	[ClassInterface(ClassInterfaceType.None)]
#if (ADD_NET)
	[ProgId("Departure_NET.FIXVTool")]
#else
	[ProgId("Departure.FIXVTool")]
#endif

	public sealed class FIXVTool : BaseCommand
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

		public FIXVTool()
		{
			base.m_bitmap = Resources.bmpFIXVTOOL;
			base.m_helpID = 0;
			base.m_helpFile = GlobalVars.HelpFile;
			base.m_caption = "";  //localizable text

#if (ADD_NET)
			base.m_category = "PANDA_NET.Departure";
			base.m_name = "FIXVisibility_NET";
#else
			base.m_category = "PANDA.Departure";
			base.m_name = "FIXVisibility";
#endif
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

				if (GlobalVars.ButtonControl8State)
					return Resources.str15486;		//"Скрыть FIX"
				else
					return Resources.str15487;		//"Отобразить FIX"
			}
		}

		public override string Message
		{
			get
			{
				Functions.SetThreadLocaleByConfig();
				Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);

				if (GlobalVars.ButtonControl8State)
					return Resources.str15486;		//"Скрыть FIX"
				else
					return Resources.str15487;		//"Отобразить FIX"
			}
		}

		public override string Tooltip
		{
			get
			{
				Functions.SetThreadLocaleByConfig();
				Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);

				if (GlobalVars.ButtonControl8State)
					return Resources.str15486;		//"Скрыть FIX"
				else
					return Resources.str15487;		//"Отобразить FIX"
			}
		}

		public override bool Checked
		{
			get
			{
				return GlobalVars.ButtonControl8State;
			}
		}

		public override bool Enabled
		{
			get
			{
				return m_enabled && GlobalVars.FIXElem != null;
			}
		}

		/// <summary>
		/// Occurs when this command is clicked
		/// </summary>
		public override void OnClick()
		{
			GlobalVars.ButtonControl8State = !GlobalVars.ButtonControl8State;

			if (GlobalVars.ButtonControl8State)
			{
				GlobalVars.GetActiveView().GraphicsContainer.AddElement(GlobalVars.FIXElem, 0);
				GlobalVars.FIXElem.Locked = true;
			}
			else
				Functions.DeleteGraphicsElement(GlobalVars.FIXElem);

			GlobalVars.GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, null, null);
		}

		#endregion
	}
}
