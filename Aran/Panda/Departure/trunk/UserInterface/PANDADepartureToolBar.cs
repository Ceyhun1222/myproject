using System;
using System.Runtime.InteropServices;
using Aran.PANDA.Departure.Properties;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;

namespace Aran.PANDA.Departure
{
	/// <summary>
	/// Summary description for ArcGISToolbar1.
	/// </summary>

	[Guid("2F5F6502-E5BB-4C9B-BE31-4EC82DF7A6C1")]
	[ClassInterface(ClassInterfaceType.None)]
#if (ADD_NET)
	[ProgId("PANDA_NET.DepartureToolBar")]
#else
	[ProgId("PANDA.DepartureToolBar")]
#endif

	public sealed class PANDADepartureToolBar : BaseToolbar
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

		public PANDADepartureToolBar()
		{
#if (ADD_NET)
			AddItem("PANDA_NET.DepartureMenu");
			AddItem("Departure_NET.CircleVTool");
			AddItem("Departure_NET.CLVTool");
			AddItem("Departure_NET.StraightVTool");
			AddItem("Departure_NET.TurnAreaVTool");
			AddItem("Departure_NET.SecondaryVTool");
			AddItem("Departure_NET.NomTrackVTool");
			AddItem("Departure_NET.KKVTool");
			AddItem("Departure_NET.FIXVTool");
#else
			AddItem("PANDA.DepartureMenu");
			AddItem("Departure.CircleVTool");
			AddItem("Departure.CLVTool");
			AddItem("Departure.StraightVTool");
			AddItem("Departure.TurnAreaVTool");
			AddItem("Departure.SecondaryVTool");
			AddItem("Departure.NomTrackVTool");
			AddItem("Departure.KKVTool");
			AddItem("Departure.FIXVTool");
#endif
		}

		public override string Caption
		{
			get
			{
				Functions.SetThreadLocaleByConfig();
				Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);
#if (ADD_NET)
				return Resources.str80 + "_NET";
#else
				return Resources.str00080;
#endif
			}
		}


		public override string Name
		{
			get
			{
#if (ADD_NET)
				return "PANDA_NET.DepartureToolBar";
#else
				return "PANDA.DepartureToolBar";
#endif
			}
		}
	}
}