using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using EnrouteChartCompare;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ARENA.Enums_Const;
using System.Collections.Generic;
using DataModule;
using ArenaStatic;
using System.Windows.Forms;
using ESRI.ArcGIS.Framework;
using ARENA;
using ESRI.ArcGIS.ArcMapUI;
using VisibilityTool;
using System.Collections.ObjectModel;
using VisibilityTool.Model;
using ESRI.ArcGIS.Carto;
using System.Diagnostics;
using System.IO;

namespace SigmaChart
{
	/// <summary>
	/// Command that works in ArcMap/Map/PageLayout
	/// </summary>
	[Guid ("7c2bdccf-a52d-4b63-ac8d-9a5b35a6ed8d")]
	[ClassInterface (ClassInterfaceType.None)]
	[ProgId ("SigmaChart.SigmaLog")]
	public sealed class SigmaLog : BaseCommand
	{
		#region COM Registration Function(s)
		[ComRegisterFunction ()]
		[ComVisible (false)]
		static void RegisterFunction (Type registerType)
		{
			// Required for ArcGIS Component Category Registrar support
			ArcGISCategoryRegistration (registerType);

			//
			// TODO: Add any COM registration code here
			//
		}

		[ComUnregisterFunction ()]
		[ComVisible (false)]
		static void UnregisterFunction (Type registerType)
		{
			// Required for ArcGIS Component Category Registrar support
			ArcGISCategoryUnregistration (registerType);

			//
			// TODO: Add any COM unregistration code here
			//
		}

		#region ArcGIS Component Category Registrar generated code
		/// <summary>
		/// Required method for ArcGIS Component Category registration -
		/// Do not modify the contents of this method with the code editor.
		/// </summary>
		private static void ArcGISCategoryRegistration (Type registerType)
		{
			string regKey = string.Format ("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
			MxCommands.Register (regKey);
			ControlsCommands.Register (regKey);
		}
		/// <summary>
		/// Required method for ArcGIS Component Category unregistration -
		/// Do not modify the contents of this method with the code editor.
		/// </summary>
		private static void ArcGISCategoryUnregistration (Type registerType)
		{
			string regKey = string.Format ("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
			MxCommands.Unregister (regKey);
			ControlsCommands.Unregister (regKey);
		}

		#endregion
		#endregion

		private IHookHelper m_hookHelper = null;
		private IApplication m_application;
		public SigmaLog ()
		{
			//
			// TODO: Define values for the public properties
			//
			base.m_category = "Sigma Log"; //localizable text
			base.m_caption = "View log";  //localizable text 
			base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
			base.m_toolTip = "Sigma Log";  //localizable text
			base.m_name = "SigmaLog";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

			try
			{
				//
				// TODO: change bitmap name if necessary
				//
				//string bitmapResourceName = GetType().Name + ".bmp";
				//base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
				base.m_bitmap = global::SigmaChart.Properties.Resources.Sigma;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.WriteLine (ex.Message, "Invalid Bitmap");
			}
		}

		#region Overridden Class Methods

		/// <summary>
		/// Occurs when this command is created
		/// </summary>
		/// <param name="hook">Instance of the application</param>
		public override void OnCreate (object hook)
		{
			if (hook == null)
				return;
			m_application = hook as IApplication;
			try
			{
				m_hookHelper = new HookHelperClass ();
				m_hookHelper.Hook = hook;
				if (m_hookHelper.ActiveView == null)
					m_hookHelper = null;
			}
			catch
			{
				m_hookHelper = null;
			}

			if (m_hookHelper == null)
				base.m_enabled = false;
			else
				base.m_enabled = true;

			// TODO:  Add other initialization code
		}

		/// <summary>
		/// Occurs when this command is clicked
		/// </summary>
		public override void OnClick ()
		{
            string LoggingPath = System.IO.Path.GetDirectoryName((m_application.Document as IDocumentInfo2).Path) + @"\SIGMA_LOG.txt";
            if (File.Exists(LoggingPath)) Process.Start(LoggingPath);
        }

		#endregion
	}
}
