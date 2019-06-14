using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ADF.BaseClasses;
using ETOD.Properties;

namespace ETOD
{
	/// <summary>
	/// Summary description for ArcGISToolbar1.
	/// </summary>

	[Guid("A3FBC413-3105-4C56-8C19-4FC6CC2FA0B3")]
	[ClassInterface(ClassInterfaceType.None)]
	[ProgId("ETOD.ETODToolBar")]


	public sealed class ETODToolBar : BaseToolbar
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
			MxCommandBars.Register(regKey);
		}
		/// <summary>
		/// Required method for ArcGIS Component Category unregistration -
		/// Do not modify the contents of this method with the code editor.
		/// </summary>
		private static void ArcGISCategoryUnregistration(Type registerType)
		{
			string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
			MxCommandBars.Unregister(regKey);
		}

		#endregion
		#endregion

		public ETODToolBar()
		{
			AddItem("ETOD.ETODMenu");
			AddItem("ETOD.Area1VisibleCMD");
			AddItem("ETOD.Area2VisibleCMD");
			AddItem("ETOD.Area3VisibleCMD");
			AddItem("ETOD.Area4VisibleCMD");
		}

		public override string Caption
		{
			get
			{
				Functions.SetThreadLocaleByConfig();
				Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);
				return Resources.Str1;
			}
		}

		public override string Name
		{
			get
			{
				return "ETOD.ETODToolBar";
			}
		}
	}
}
