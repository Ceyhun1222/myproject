using System;
using System.Runtime.InteropServices;
using Aran.PANDA.Departure.Properties;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;

namespace Aran.PANDA.Departure
{
	/// <summary>
	/// Summary description for ArcGISMenu1.
	/// </summary>
	[Guid("756401D5-D13E-4bf9-B71A-19FC04E80EA8")]
	[ClassInterface(ClassInterfaceType.None)]
#if (ADD_NET)
	[ProgId("PANDA_NET.DepartureMenu")]
#else
	[ProgId("PANDA.DepartureMenu")]
#endif

	public sealed class PANDADepartureMenu : BaseMenu
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

		public PANDADepartureMenu()
		{
#if (ADD_NET)
			AddItem("Departure_NET.DepartOmniDirectCMD");
			AddItem("Departure_NET.DepartRoutsCMD");
			AddItem("Departure_NET.DepartGuidanceCMD");
#else
			AddItem("Departure.DepartOmniDirectCMD");
			AddItem("Departure.DepartRoutsCMD");
			AddItem("Departure.DepartGuidanceCMD");
#endif
		}

		public override string Caption
		{
			get
			{
				Functions.SetThreadLocaleByConfig();
				Resources.Culture = new System.Globalization.CultureInfo(GlobalVars.LangCode);

#if (ADD_NET)
				return Resources.str90 + "_NET";	//"Вылет..."
#else
				return Resources.str00090;			//"Вылет..."
#endif
			}
		}

		public override string Name
		{
			get
			{
#if (ADD_NET)
				return "PANDA_NET.DepartureMenu";
#else
				return "PANDA.DepartureMenu";
#endif
			}
		}
	}
}
