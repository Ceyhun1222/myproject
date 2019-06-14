using System;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;

namespace EOSID
{
	/// <summary>
	/// Summary description for EOSIDMenu.
	/// </summary>
	[Guid("1e016fd7-dbe1-4740-bb25-60e99a01d06e")]
	[ClassInterface(ClassInterfaceType.None)]
	[ProgId("EOSID.EOSIDMenu")]
	public sealed class EOSIDMenu : BaseMenu	//, ESRI.ArcGIS.Framework.IRootLevelMenu
	{
		#region COM Registration Function(s)
		[ComRegisterFunction()]
		[ComVisible(false)]
		static void RegisterFunction(Type registerType)
		{
			// Required for ArcGIS Component Category Registrar support
			ArcGISCategoryRegistration(registerType);

			//
			// TODO: Add any COM registration code here
			//
		}

		[ComUnregisterFunction()]
		[ComVisible(false)]
		static void UnregisterFunction(Type registerType)
		{
			// Required for ArcGIS Component Category Registrar support
			ArcGISCategoryUnregistration(registerType);

			//
			// TODO: Add any COM unregistration code here
			//
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

		public EOSIDMenu()
		{
			AddItem("EOSID.EOSIDCmd");
		}

		public override string Caption
		{
			get
			{
				return "EOSID Menu";
			}
		}

		public override string Name
		{
			get
			{
				return "EOSIDMenu";
			}
		}
	}
}