using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Framework;

namespace EOSID
{
	/// <summary>
	/// Summary description for Command1.
	/// </summary>
	[Guid("4f4e7e0f-2ea3-46c6-be7f-b3f6f534b56c")]
	[ClassInterface(ClassInterfaceType.None)]
	[ProgId("EOSID.EOSIDCmd")]
	public sealed class EOSIDCmd : BaseCommand
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

		public EOSIDCmd()
		{
			//
			// TODO: Define values for the public properties
			//
			base.m_category = "EO"; //localizable text
			base.m_caption = "EO SID";  //localizable text
			base.m_message = "EO SID";  //localizable text 
			base.m_toolTip = "EO SID";  //localizable text 
			base.m_name = "EOSIDCmd";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

			try
			{
				//
				// TODO: change bitmap name if necessary
				//
				string bitmapResourceName = GetType().Name + ".bmp";
				base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
			}
		}

		#region Overridden Class Methods

		/// <summary>
		/// Occurs when this command is created
		/// </summary>
		/// <param name="hook">Instance of the application</param>
		public override void OnCreate(object hook)
		{
			if (hook == null)
				return;

			GlobalVars.Application = hook as IApplication;

			//Disable if it is not ArcMap
			if (hook is IMxApplication)
				base.m_enabled = true;
			else
				base.m_enabled = false;

			// TODO:  Add other initialization code
		}

		/// <summary>
		/// Occurs when this command is clicked
		/// </summary>
		public override void OnClick()
		{
			//ArcMap.Application.CurrentTool = null;

			if (GlobalVars.InitCommand(1))
			{
				MainForm mainForm = new MainForm();
				mainForm.Show(GlobalVars.win32Window);
				GlobalVars.CurrCmd = 1;
			}
		}

		#endregion
	}
}
