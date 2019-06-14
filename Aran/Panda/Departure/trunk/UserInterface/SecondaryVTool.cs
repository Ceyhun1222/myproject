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
	[Guid("1A118714-05FF-4b50-902B-172D5E715251")]
	[ClassInterface(ClassInterfaceType.None)]
#if (ADD_NET)
	[ProgId("Departure_NET.SecondaryVTool")]
#else
	[ProgId("Departure.SecondaryVTool")]
#endif

	public sealed class SecondaryVTool : BaseCommand
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

		public SecondaryVTool()
		{
			base.m_bitmap = Resources.bmpSECONDARYVTOOL;
			base.m_helpID = 0;
			base.m_helpFile = GlobalVars.HelpFile;
			base.m_caption = "";  //localizable text

#if (ADD_NET)
			base.m_category = "PANDA_NET.Departure";
			base.m_name = "SecondaryAreaVisibility_NET";
#else
			base.m_category = "PANDA.Departure";
			base.m_name = "SecondaryAreaVisibility";
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

				if (GlobalVars.ButtonControl4State)
					return Resources.str15492;			//"Скрыть зоны навигационного средства"
				else
					return Resources.str15493;			//"Отобразить зоны навигационного средства"
			}
		}

		public override string Message
		{
			get
			{
				Functions.SetThreadLocaleByConfig();
				Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);

				if (GlobalVars.ButtonControl4State)
					return Resources.str15492;			//"Скрыть зоны навигационного средства"
				else
					return Resources.str15493;			//"Отобразить зоны навигационного средства"
			}
		}

		public override string Tooltip
		{
			get
			{
				Functions.SetThreadLocaleByConfig();
				Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);

				if (GlobalVars.ButtonControl4State)
					return Resources.str15492;			//"Скрыть зоны навигационного средства"
				else
					return Resources.str15493;			//"Отобразить зоны навигационного средства"
			}
		}

		public override bool Checked
		{
			get
			{
				return GlobalVars.ButtonControl4State;
			}
		}

		public override bool Enabled
		{
			get
			{
				return m_enabled && (GlobalVars.PrimElem != null || GlobalVars.SecRElem != null || GlobalVars.SecLElem != null);
			}
		}

		/// <summary>
		/// Occurs when this command is clicked
		/// </summary>
		public override void OnClick()
		{
			GlobalVars.ButtonControl4State = !GlobalVars.ButtonControl4State;

			if (GlobalVars.ButtonControl4State)
			{
				if (GlobalVars.PrimElem != null)
				{
					GlobalVars.GetActiveView().GraphicsContainer.AddElement(GlobalVars.PrimElem, 0);
					GlobalVars.PrimElem.Locked = true;
				}

				if (GlobalVars.SecRElem != null)
				{
					GlobalVars.GetActiveView().GraphicsContainer.AddElement(GlobalVars.SecRElem, 0);
					GlobalVars.SecRElem.Locked = true;
				}

				if (GlobalVars.SecLElem != null)
				{
					GlobalVars.GetActiveView().GraphicsContainer.AddElement(GlobalVars.SecLElem, 0);
					GlobalVars.SecLElem.Locked = true;
				}
			}
			else
			{
				Functions.DeleteGraphicsElement(GlobalVars.PrimElem);
				Functions.DeleteGraphicsElement(GlobalVars.SecRElem);
				Functions.DeleteGraphicsElement(GlobalVars.SecLElem);
			}

			GlobalVars.GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, null, null);
		}

		#endregion
	}
}
