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
	[Guid("1A118712-05FF-4b50-902B-172D5E715251")]
	[ClassInterface(ClassInterfaceType.None)]
#if (ADD_NET)
	[ProgId("Departure_NET.StraightVTool")]
#else
	[ProgId("Departure.StraightVTool")]
#endif

	public sealed class StraightVTool : BaseCommand
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

		public StraightVTool()
		{
			base.m_bitmap = Resources.bmpSTRAIGHTVTOOL;
			base.m_helpID = 0;
			base.m_helpFile = GlobalVars.HelpFile;
			base.m_caption = "";  //localizable text

#if (ADD_NET)
			base.m_category = "PANDA_NET.Departure";
			base.m_name = "StraightAreaVisibility_NET";
#else
			base.m_category = "PANDA.Departure";
			base.m_name = "StraightAreaVisibility";
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

				if (GlobalVars.ButtonControl2State)
					return Resources.str15494;	//"Скрыть зону прямого вылета"
				else
					return Resources.str15495;	//"Отобразить зону прямого вылета"
			}
		}

		public override string Message
		{
			get
			{
				Functions.SetThreadLocaleByConfig();
				Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);

				if (GlobalVars.ButtonControl2State)
					return Resources.str15494;	//"Скрыть зону прямого вылета"
				else
					return Resources.str15495;	//"Отобразить зону прямого вылета"
			}
		}

		public override string Tooltip
		{
			get
			{
				Functions.SetThreadLocaleByConfig();
				Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);

				if (GlobalVars.ButtonControl2State)
					return Resources.str15494;	//"Скрыть зону прямого вылета"
				else
					return Resources.str15495;	//"Отобразить зону прямого вылета"
			}
		}

		public override bool Checked
		{
			get
			{
				return GlobalVars.ButtonControl2State;
			}
		}

		public override bool Enabled
		{
			get
			{
				return m_enabled && GlobalVars.StraightAreaElem != null;
			}
		}

		/// <summary>
		/// Occurs when this command is clicked
		/// </summary>
		public override void OnClick()
		{
			GlobalVars.ButtonControl2State = !GlobalVars.ButtonControl2State;
			if (GlobalVars.StraightAreaElem == null)
				return;

			if (GlobalVars.StraightAreaElem is ESRI.ArcGIS.Carto.GroupElement)
			{
				ESRI.ArcGIS.Carto.IGroupElement pGroupElement = (ESRI.ArcGIS.Carto.IGroupElement)GlobalVars.StraightAreaElem;

				for (int i = 0; i < pGroupElement.ElementCount; i++)
				{
					ESRI.ArcGIS.Carto.IElement pElement = pGroupElement.Element[i];

					if (pGroupElement.Element[i] != null)
					{
						if (GlobalVars.ButtonControl2State)
						{
							GlobalVars.GetActiveView().GraphicsContainer.AddElement(pElement, 0);
							pElement.Locked = true;
						}
						else
							Functions.DeleteGraphicsElement(pElement);
					}
				}
			}
			else
			{
				if (GlobalVars.ButtonControl2State)
				{
					GlobalVars.GetActiveView().GraphicsContainer.AddElement(GlobalVars.StraightAreaElem, 0);
					GlobalVars.StraightAreaElem.Locked = true;
				}
				else
					Functions.DeleteGraphicsElement(GlobalVars.StraightAreaElem);
			}
			GlobalVars.GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, null, null);
		}

		#endregion
	}
}
