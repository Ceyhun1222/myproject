using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;

using ETOD.Properties;

namespace ETOD
{
	/// <summary>
	/// Summary description for Command1.
	/// </summary>
	[Guid("456D0211-BB0C-4DA1-84C5-D366F298C8B8")]
	[ClassInterface(ClassInterfaceType.None)]
	[ProgId("ETOD.Area3VisibleCMD")]

	public sealed class Area3VisibleTool : BaseCommand
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

		public Area3VisibleTool()
		{
			Functions.SetThreadLocaleByConfig();
			Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);

			base.m_category = "ETOD.AreaVisibility";
			base.m_name = "Area3Visible";

			base.m_helpFile = GlobalVars.HelpFile;
			base.m_helpID = 0;
			base.m_bitmap = Resources.Area1VisibleCMD;
			base.m_caption = "";
			base.m_message = Resources.Str102;
			base.m_toolTip = Resources.Str102;
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

		//public override string Caption
		//{
		//    get
		//    {
		//        Functions.SetThreadLocaleByConfig();
		//        Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);

		//        if (GlobalVars.Area1State)
		//            return Resources.Str100;		//"Скрыть FIX"
		//        else
		//            return Resources.Str100;		//"Отобразить FIX"
		//    }
		//}

		//public override string Message
		//{
		//    get
		//    {
		//        Functions.SetThreadLocaleByConfig();
		//        Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);

		//        if (GlobalVars.Area1State)
		//            return Resources.Str100;		//"Скрыть FIX"
		//        else
		//            return Resources.Str100;		//"Отобразить FIX"
		//    }
		//}

		//public override string Tooltip
		//{
		//    get
		//    {
		//        Functions.SetThreadLocaleByConfig();
		//        Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);

		//        if (GlobalVars.Area1State)
		//            return Resources.Str100;		//"Скрыть FIX"
		//        else
		//            return Resources.Str100;		//"Отобразить FIX"
		//    }
		//}

		public override bool Checked
		{
			get
			{
				return GlobalVars.Area3State;
			}
		}

		public override bool Enabled
		{
			get
			{
				return m_enabled && GlobalVars.Area3Elem != null;
			}
		}

		/// <summary>
		/// Occurs when this command is clicked
		/// </summary>
		public override void OnClick()
		{
			GlobalVars.Area3State = !GlobalVars.Area3State;

			if (GlobalVars.Area3State)
			{
				GlobalVars.GetActiveView().GraphicsContainer.AddElement(GlobalVars.Area3Elem, 0);
				GlobalVars.Area3Elem.Locked = true;
			}
			else
				Functions.DeleteGraphicsElement(GlobalVars.Area3Elem);

			GlobalVars.GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, null, null);
		}

		#endregion
	}
}
