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
	[Guid("1A118715-05FF-4b50-902B-172D5E715251")]
	[ClassInterface(ClassInterfaceType.None)]
#if (ADD_NET)
	[ProgId("Departure_NET.NomTrackVTool")]
#else
	[ProgId("Departure.NomTrackVTool")]
#endif

	public sealed class NomTrackVTool : BaseCommand
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

		public NomTrackVTool()
		{
			base.m_bitmap = Resources.bmpNOMTRACKVTOOL;
			base.m_helpID = 0;
			base.m_helpFile = GlobalVars.HelpFile;
			base.m_caption = "";  //localizable text

#if (ADD_NET)
			base.m_category = "PANDA_NET.Departure";
			base.m_name = "NomTrackVisibility_NET";
#else
			base.m_category = "PANDA.Departure";
			base.m_name = "NomTrackVisibility";
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

				if (GlobalVars.ButtonControl5State)
					return Resources.str15490;		//"Скрыть номинальную линию пути"
				else
					return Resources.str15491;		//"Отобразить номинальную линию пути"
			}
		}

		public override string Message
		{
			get
			{
				Functions.SetThreadLocaleByConfig();
				Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);

				if (GlobalVars.ButtonControl5State)
					return Resources.str15490;		//"Скрыть номинальную линию пути"
				else
					return Resources.str15491;		//"Отобразить номинальную линию пути"
			}
		}

		public override string Tooltip
		{
			get
			{
				Functions.SetThreadLocaleByConfig();
				Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);

				if (GlobalVars.ButtonControl5State)
					return Resources.str15490;		//"Скрыть номинальную линию пути"
				else
					return Resources.str15491;		//"Отобразить номинальную линию пути"
			}
		}

		public override bool Checked
		{
			get
			{
				return GlobalVars.ButtonControl5State;
			}
		}

		public override bool Enabled
		{
			get
			{
				return m_enabled && (GlobalVars.NomTrackElem != null || GlobalVars.StrTrackElem != null);
			}
		}

		/// <summary>
		/// Occurs when this command is clicked
		/// </summary>
		public override void OnClick()
		{
			GlobalVars.ButtonControl5State = !GlobalVars.ButtonControl5State;

			if (GlobalVars.ButtonControl5State)
			{
				if (GlobalVars.NomTrackElem != null)
				{
					GlobalVars.GetActiveView().GraphicsContainer.AddElement(GlobalVars.NomTrackElem, 0);
					GlobalVars.NomTrackElem.Locked = true;
				}

				if (GlobalVars.StrTrackElem != null)
				{
					GlobalVars.GetActiveView().GraphicsContainer.AddElement(GlobalVars.StrTrackElem, 0);
					GlobalVars.StrTrackElem.Locked = true;
				}
			}
			else
			{
				Functions.DeleteGraphicsElement(GlobalVars.NomTrackElem);
				Functions.DeleteGraphicsElement(GlobalVars.StrTrackElem);
			}

			GlobalVars.GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, null, null);
		}

		#endregion
	}
}
