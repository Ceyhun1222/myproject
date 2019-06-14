using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ADF.BaseClasses;

namespace SigmaTestMenu
{
	/// <summary>
	/// Summary description for SigmaTestMenu.
	/// </summary>
	[Guid ( "4219ce4b-61f2-4054-bb50-03f5623e9b4d" )]
	[ClassInterface ( ClassInterfaceType.None )]
	[ProgId ( "SigmaTestMenu.SigmaTestMenu" )]
	public sealed class SigmaTestMenu : BaseMenu
	{
		#region COM Registration Function(s)
		[ComRegisterFunction ( )]
		[ComVisible ( false )]
		static void RegisterFunction ( Type registerType )
		{
			// Required for ArcGIS Component Category Registrar support
			ArcGISCategoryRegistration ( registerType );

			//
			// TODO: Add any COM registration code here
			//
		}

		[ComUnregisterFunction ( )]
		[ComVisible ( false )]
		static void UnregisterFunction ( Type registerType )
		{
			// Required for ArcGIS Component Category Registrar support
			ArcGISCategoryUnregistration ( registerType );

			//
			// TODO: Add any COM unregistration code here
			//
		}

		#region ArcGIS Component Category Registrar generated code
		/// <summary>
		/// Required method for ArcGIS Component Category registration -
		/// Do not modify the contents of this method with the code editor.
		/// </summary>
		private static void ArcGISCategoryRegistration ( Type registerType )
		{
			string regKey = string.Format ( "HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID );
			MxCommandBars.Register ( regKey );
		}
		/// <summary>
		/// Required method for ArcGIS Component Category unregistration -
		/// Do not modify the contents of this method with the code editor.
		/// </summary>
		private static void ArcGISCategoryUnregistration ( Type registerType )
		{
			string regKey = string.Format ( "HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID );
			MxCommandBars.Unregister ( regKey );
		}

		#endregion
		#endregion

		public SigmaTestMenu ( )
		{
			//
			// TODO: Define your menu here by adding items
			//
			//AddItem("esriArcMapUI.ZoomInFixedCommand");
			//BeginGroup(); //Separator
			//AddItem("{FBF8C3FB-0480-11D2-8D21-080009EE4E51}", 1); //undo command
			//AddItem(new Guid("FBF8C3FB-0480-11D2-8D21-080009EE4E51"), 2); //redo command
			AddItem ( "SigmaTestMenu.OIS" );
			BeginGroup ( ); //Separator
			AddItem ( "SigmaTestMenu.ChartDataTabulation" );
		}

		public override string Caption
		{
			get
			{
				//TODO: Replace bar caption
				return "SIGMA Tester";
			}
		}
		public override string Name
		{
			get
			{
				//TODO: Replace bar ID
				return "SigmaTestMenu";
			}
		}
	}
}