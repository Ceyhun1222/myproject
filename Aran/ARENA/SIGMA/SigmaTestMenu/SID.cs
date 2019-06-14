using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ChartDataTabulation;
using System.Linq;
using System.Collections.Generic;

namespace SigmaTestMenu
{
	/// <summary>
	/// Summary description for SID.
	/// </summary>
	[Guid ( "303ae14f-4062-44a6-9bfa-c0d612416f7a" )]
	[ClassInterface ( ClassInterfaceType.None )]
	[ProgId ( "SigmaTestMenu.SID" )]
	public sealed class SID : BaseCommand
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
			MxCommands.Register ( regKey );

		}
		/// <summary>
		/// Required method for ArcGIS Component Category unregistration -
		/// Do not modify the contents of this method with the code editor.
		/// </summary>
		private static void ArcGISCategoryUnregistration ( Type registerType )
		{
			string regKey = string.Format ( "HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID );
			MxCommands.Unregister ( regKey );

		}

		#endregion
		#endregion

		private IApplication m_application;
		public SID ( )
		{
			//
			// TODO: Define values for the public properties
			//
			base.m_category = "SID"; //localizable text
			base.m_caption = "SID";  //localizable text
			base.m_message = "SID";  //localizable text 
			base.m_toolTip = "SID";  //localizable text 
			base.m_name = "SID";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

			try
			{
				//
				// TODO: change bitmap name if necessary
				//
				//string bitmapResourceName = GetType ( ).Name + ".bmp";
				//base.m_bitmap = new Bitmap ( GetType ( ), bitmapResourceName );
			}
			catch ( Exception ex )
			{
				System.Diagnostics.Trace.WriteLine ( ex.Message, "Invalid Bitmap" );
			}
		}

		#region Overridden Class Methods

		/// <summary>
		/// Occurs when this command is created
		/// </summary>
		/// <param name="hook">Instance of the application</param>
		public override void OnCreate ( object hook )
		{
			if ( hook == null )
				return;

			m_application = hook as IApplication;

			//Disable if it is not ArcMap
			if ( hook is IMxApplication )
				base.m_enabled = true;
			else
				base.m_enabled = false;

			// TODO:  Add other initialization code
		}

		/// <summary>
		/// Occurs when this command is clicked
		/// </summary>
		public override void OnClick ( )
		{
            //var list = ARENA.DataCash.ProjectEnvironment.Data.PdmObjectList.FindAll ( pdm => pdm.PDM_Type == PDM.PDM_ENUM.StandardInstrumentDeparture ).ToList ( );
            //List<PDM.StandardInstrumentDeparture> sidList = new List<PDM.StandardInstrumentDeparture> ( );
            //sidList.Add ( list[ 0 ] as PDM.StandardInstrumentDeparture );
            //sidList.Add ( list[ 1 ] as PDM.StandardInstrumentDeparture );
            //sidList.Add ( list[ list.Count - 2 ] as PDM.StandardInstrumentDeparture );
            //sidList.Add ( list[ list.Count - 1 ] as PDM.StandardInstrumentDeparture );
            //SIDTabulation sidTab = new SIDTabulation ( );
            //sidTab.CrateTable ( sidList, "Tabular" );
			return;
		}

		#endregion
	}
}
