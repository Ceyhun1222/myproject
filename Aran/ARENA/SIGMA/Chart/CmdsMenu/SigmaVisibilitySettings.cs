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

namespace SigmaChart
{
	/// <summary>
	/// Command that works in ArcMap/Map/PageLayout
	/// </summary>
	[Guid ("b5d5b7a6-ff77-47f5-ba14-b0dfbabee741")]
	[ClassInterface (ClassInterfaceType.None)]
	[ProgId ("SigmaChart.VisibilitySettings")]
	public sealed class VisibilitySettings : BaseCommand
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
		public VisibilitySettings ()
		{
			//
			// TODO: Define values for the public properties
			//
			base.m_category = "Visibility Settings"; //localizable text
			base.m_caption = "Changes visibility of data content";  //localizable text 
			base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
			base.m_toolTip = "Visibility Settings";  //localizable text
			base.m_name = "VisibilitySettings";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

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
			var mainWind = new VisibilityTool.MainWindow ();

			var parentHandle = new IntPtr (m_application.hWnd);
			var helper = new WindowInteropHelper (mainWind) { Owner = parentHandle };
			mainWind.ShowInTaskbar = false;
			ObservableCollection<LayerTemplate> dataTemplates = new ObservableCollection<LayerTemplate> ( );
			if ( SigmaDataCash.ChartElementList != null && SigmaDataCash.ChartElementList.Count == 0 )
			{
				if ( !( ( IMxDocument ) m_application.Document ).CurrentContentsView.Name.StartsWith ( "ANCORTOCLayerView" ) )
					ArenaStaticProc.BringToFrontToc ( ( IMxDocument ) m_application.Document, "ANCORTOCLayerView" );
			}
			if ( SigmaDataCash.SigmaChartType == ( int ) SigmaChartTypes.EnrouteChart_Type )
			{
			    EnrouteLayerTemplates().ForEach(t => dataTemplates.Add(t));
			}
			else if ( SigmaDataCash.SigmaChartType == ( int ) SigmaChartTypes.SIDChart_Type)
			{
			    TerminalLayerTemplates().ForEach(t => dataTemplates.Add(t));
			}
			else if ( SigmaDataCash.SigmaChartType == ( int ) SigmaChartTypes.STARChart_Type )
			{
			    TerminalLayerTemplates().ForEach(t => dataTemplates.Add(t));
			}
            else if (SigmaDataCash.SigmaChartType == (int) SigmaChartTypes.IAPChart_Type)
		    {
		        TerminalLayerTemplates().ForEach(t => dataTemplates.Add(t));
		    }

		    mainWind.SetData ( m_hookHelper, dataTemplates );
			// hide from taskbar and alt-tab list
			//MessageBox.Show ("Clicked");
			mainWind.Show ();

			//Application.DoEvents ();

			//for (int i = 0; i < ((IMxDocument) m_application.Document).ContentsViewCount; i++)
			//{
			//	IContentsView cnts = ((IMxDocument) m_application.Document).get_ContentsView (i);

			//	string cntxtName = ((IMxDocument) m_application.Document).ContentsView[i].Name;

			//	if (cntxtName.StartsWith ("ANCORTOCLayerView")) ((IMxDocument) m_application.Document).ContentsView[i].Refresh (cntxtName);
			//	if (cntxtName.StartsWith ("TOCLayerFilter")) ((IMxDocument) m_application.Document).ContentsView[i].Refresh (cntxtName);

			//}
			return;
		}

	    private static List<LayerTemplate> TerminalLayerTemplates()
	    {
	        return new List<LayerTemplate> {AirspaceLayTemp(), DesgPntLayTemp(), NavaidLayTemp(), LegLayTemp()};
	    }

	    private static LayerTemplate LegLayTemp()
	    {
	        LayerTemplate legLayer = new LayerTemplate
	        {
	            Name = "Procedure Leg",
	            IdField = "FeatureGUID",
	            DescriptField = "seqNumberARINC",
	            GroupByField = "ProcName",
	            PrimaryTableName = "ProcedureLegsCartography"
	        };

	        RefLayer heightAnno = new RefLayer
	        {
	            IsAnnotation = true,
	            TableName = "ProcedureLegsAnnoHeightCartography",
	            RefIdField = "PdmUID"
	        };
	        legLayer.RelatedLayers.Add(heightAnno);

	        RefLayer courseAnno = new RefLayer
	        {
	            IsAnnotation = true,
	            TableName = "ProcedureLegsAnnoCourseCartography",
	            RefIdField = "PdmUID"
	        };
	        legLayer.RelatedLayers.Add(courseAnno);

	        RefLayer lengthAnno = new RefLayer
	        {
	            IsAnnotation = true,
	            TableName = "ProcedureLegsAnnoLengthCartography",
	            RefIdField = "PdmUID"
	        };
	        legLayer.RelatedLayers.Add(lengthAnno);

	        RefLayer speedAnno = new RefLayer
	        {
	            IsAnnotation = true,
	            TableName = "ProcedureLegsAnnoSpeedLimitCartography",
	            RefIdField = "PdmUID"
	        };
	        legLayer.RelatedLayers.Add(speedAnno);

	        RefLayer nameAnno = new RefLayer
	        {
	            IsAnnotation = true,
	            TableName = "ProcedureLegsAnnoLegNameCartography",
	            RefIdField = "PdmUID"
	        };
	        legLayer.RelatedLayers.Add(nameAnno);

	        return legLayer;
	    }

        private static LayerTemplate NavaidLayTemp()
	    {
	        LayerTemplate layTempNavaidC = new LayerTemplate
	        {
	            Name = "Navaid",
	            IdField = "FeatureGUID",
	            DescriptField = "name",
	            GroupByField = "Nav_Type",
	            PrimaryTableName = "NavaidsCartography"
	        };

	        RefLayer navaidAnno = new RefLayer
	        {
	            IsAnnotation = true,
	            TableName = "NavaidsAnno",
	            RefIdField = "PdmUID"
	        };
	        layTempNavaidC.RelatedLayers.Add(navaidAnno);
	        return layTempNavaidC;
	    }

	    private static LayerTemplate DesgPntLayTemp()
	    {
	        LayerTemplate layTempDsgPntC = new LayerTemplate
	        {
	            Name = "Designated Point",
	            IdField = "FeatureGUID",
	            DescriptField = "SegmentPointDesignator",
	            GroupByField = "ReportingATC",
	            PrimaryTableName = "DesignatedPointCartography"
	        };

	        RefLayer dsgPntAnno = new RefLayer
	        {
	            IsAnnotation = true,
	            TableName = "DesignatedPointAnno",
	            RefIdField = "PdmUID"
	        };
	        layTempDsgPntC.RelatedLayers.Add(dsgPntAnno);
	        return layTempDsgPntC;
	    }

	    private static LayerTemplate AirspaceLayTemp()
	    {
	        LayerTemplate layTempAirspaceC = new LayerTemplate
	        {
	            Name = "Airspace",
	            IdField = "FeatureGUID",
	            DescriptField = "txtName",
	            GroupByField = "CodeType",
	            CanSplitLayers = true,
	            SplittedLayerName = "Border",
	            PrimaryTableName = "AirspaceC"
	        };

	        RefLayer airspaceBuffer = new RefLayer
	        {
	            RefIdField = "MasterID",
	            TableName = "AirspaceB",
	            SplittedLayerName = "Buffer"
	        };
	        layTempAirspaceC.RelatedLayers.Add(airspaceBuffer);

	        RefLayer airspaceAnno = new RefLayer
	        {
	            IsAnnotation = true,
	            TableName = "AirspaceAnno",
	            RefIdField = "PdmUID"
	        };
	        layTempAirspaceC.RelatedLayers.Add(airspaceAnno);
	        return layTempAirspaceC;
	    }

	    private static List<LayerTemplate> EnrouteLayerTemplates()
	    {
	        List<LayerTemplate> result = new List<LayerTemplate>();
	        LayerTemplate layTempAirspaceC = new LayerTemplate
	        {
	            Name = "Airspace",
	            IdField = "FeatureGUID",
	            DescriptField = "txtName",
	            GroupByField = "CodeType",
	            CanSplitLayers = true,
	            SplittedLayerName = "Border",
	            PrimaryTableName = "AirspaceC"
	        };

	        RefLayer airspaceBuffer = new RefLayer
	        {
	            RefIdField = "MasterID",
	            TableName = "AirspaceB",
	            SplittedLayerName = "Buffer"
	        };
	        layTempAirspaceC.RelatedLayers.Add(airspaceBuffer);

	        RefLayer airspaceAnno = new RefLayer
	        {
	            IsAnnotation = true,
	            TableName = "AirspaceAnno",
	            RefIdField = "PdmUID"
	        };
	        layTempAirspaceC.RelatedLayers.Add(airspaceAnno);
	        result.Add(layTempAirspaceC);


	        LayerTemplate layTempDsgPntC = new LayerTemplate
	        {
	            Name = "Designated Point",
	            IdField = "FeatureGUID",
	            DescriptField = "SegmentPointDesignator",
	            GroupByField = "ReportingATC",
	            PrimaryTableName = "DesignatedPointCartography"
	        };

	        RefLayer dsgPntAnno = new RefLayer
	        {
	            IsAnnotation = true,
	            TableName = "DesignatedPointAnno",
	            RefIdField = "PdmUID"
	        };
	        layTempDsgPntC.RelatedLayers.Add(dsgPntAnno);
	        result.Add(layTempDsgPntC);


	        LayerTemplate layTempNavaidC = new LayerTemplate
	        {
	            Name = "Navaid",
	            IdField = "FeatureGUID",
	            DescriptField = "name",
	            GroupByField = "Nav_Type",
	            PrimaryTableName = "NavaidsCartography"
	        };

	        RefLayer navaidAnno = new RefLayer
	        {
	            IsAnnotation = true,
	            TableName = "NavaidsAnno",
	            RefIdField = "PdmUID"
	        };
	        layTempNavaidC.RelatedLayers.Add(navaidAnno);
	        result.Add(layTempNavaidC);
	        return result;
	    }

	    #endregion
	}
}
