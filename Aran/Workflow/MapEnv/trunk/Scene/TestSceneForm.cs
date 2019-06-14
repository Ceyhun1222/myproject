using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using System.IO;
using ESRI.ArcGIS.Catalog;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using E3D = ESRI.ArcGIS.Analyst3D;
using EG = ESRI.ArcGIS.Geometry;
using AIM = Aran.Aim.Features;

using ESRI.ArcGIS.DataSourcesFile;
using Aran.Aim;
using Aran.Aim.Features;
using Ag = Aran.Geometries;
using Aran.Aim.DataTypes;
using Aran.Converters;
using ESRI.ArcGIS.DataSourcesGDB;
using Eg = ESRI.ArcGIS.Geometry;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Display;

namespace MapEnv.Forms
{
	public partial class TestSceneForm : Form
	{
        private Eg.ISpatialReference _wgs84EsriSpRef;
		private List<ILayerEffects> _transparentLayers;


		public TestSceneForm ()
		{
			InitializeComponent ();

            button1.Visible = false;

            _wgs84EsriSpRef = Globals.CreateWGS84SR();
			_transparentLayers = new List<ILayerEffects> ();

			Check3DLisence ();

			numericUpDown1_ValueChanged (null, null);
		}


		public void ShowScene ()
		{
			_transparentLayers.Clear ();

			var myFeatureLayers = new List<AimFeatureLayer> ();
            foreach (var aimLayer in Globals.MainForm.AimLayers)
            {
                if (aimLayer.Layer is AimFeatureLayer)
                    myFeatureLayers.Add(aimLayer.Layer as AimFeatureLayer);
            }

			
			foreach (var mfl in myFeatureLayers)
			{
				foreach (var item in mfl.LayerInfoList)
					AddLayer (item.Layer, mfl.FeatureType);
			}

            //Test1();
            //button1_Click(null, null);

			Show ();
		}


		private void AddLayer (IFeatureLayer featLayer, FeatureType aimFeatureType)
		{
			var layerEff = featLayer as ILayerEffects;

            if (layerEff.SupportsTransparency) {
                layerEff.Transparency = 70;
                _transparentLayers.Add(layerEff);
            }

			var scene = ui_sceneControl.Scene;
			scene.AddLayer (featLayer);

			var geomType = featLayer.FeatureClass.ShapeType;
            if (geomType != EG.esriGeometryType.esriGeometryPoint &&
                geomType != EG.esriGeometryType.esriGeometryPolygon) {
                return;
            }

			var layer = scene.get_Layer (scene.LayerCount - 1);
			var layerExtensions = (ILayerExtensions) layer;

			for (int i = 0; i < layerExtensions.ExtensionCount; i++)
			{
				if (layerExtensions.Extension [i] is ESRI.ArcGIS.Analyst3D.I3DProperties)
				{
					var prop = layerExtensions.Extension [i] as E3D.I3DProperties;

                    if (aimFeatureType == Aran.Aim.FeatureType.Airspace)
						Set3DAirPro (prop);
					else if (aimFeatureType == FeatureType.VerticalStructure)
						Set3DPro (prop);

					prop.Apply3DProperties (layer);

					break;
				}
			}
		}

		private void Set3DAirPro (E3D.I3DProperties prop)
		{
			prop.BaseExpressionString = "0";
			prop.BaseOption = E3D.esriBaseOption.esriBaseShape;
			prop.BaseSurface = null;
			prop.DepthPriorityValue = 1;
			prop.ExtrusionType = E3D.esriExtrusionType.esriExtrusionBase;
			prop.ExtrusionExpressionString = "[z_max]";
			prop.FaceCulling = E3D.esri3DFaceCulling.esriFaceCullingNone;
			prop.Illuminate = true;
			prop.OffsetExpressionString = "";
			prop.RenderMode = E3D.esriRenderMode.esriRenderCache;
			prop.SmoothShading = false;
			prop.ZFactor = 1;

		}

		private void Set3DPro (E3D.I3DProperties prop)
		{
			prop.BaseOption = ESRI.ArcGIS.Analyst3D.esriBaseOption.esriBaseShape;
			prop.ZFactor = 1;
			prop.BaseExpressionString = "0";
			prop.ExtrusionExpressionString = "0";
			prop.DepthPriorityValue = 1;
			prop.ExtrusionType = ESRI.ArcGIS.Analyst3D.esriExtrusionType.esriExtrusionAbsolute;
			prop.FaceCulling = ESRI.ArcGIS.Analyst3D.esri3DFaceCulling.esriFaceCullingNone;
			prop.Illuminate = true;
			prop.OffsetExpressionString = "0";
			prop.RenderMode = ESRI.ArcGIS.Analyst3D.esriRenderMode.esriRenderCache;
		}

		private bool Check3DLisence ()
		{
			UID p3DUid = new UID ();
			p3DUid.Value = "{94305472-592E-11D4-80EE-00C04FA0ADF8}";

			var pExtAdmin = new ExtensionManager () as IExtensionManagerAdmin;
			object DD = null;
			pExtAdmin.AddExtension (p3DUid, ref DD);

			var pExtManager = pExtAdmin as IExtensionManager;
			var p3DExtConfig = pExtManager.FindExtension (p3DUid) as IExtensionConfig;

			if (p3DExtConfig.State != esriExtensionState.esriESUnavailable)
			{
				p3DExtConfig.State = esriExtensionState.esriESEnabled;
				return true;
			}

			return false;
		}

		private void numericUpDown1_ValueChanged (object sender, EventArgs e)
		{
			ui_sceneControl.Scene.ExaggerationFactor = Convert.ToDouble (numericUpDown1.Value);
		}

		private void Form_FormClosed (object sender, FormClosedEventArgs e)
		{
			foreach (var layerEff in _transparentLayers)
			{
				layerEff.Transparency = 0;
			}
		}

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Test1()
        {
            var iapGuid = "ebe56414-7697-4ca4-9735-748580249d5e"; //LAT
            //iapGuid = "62974e71-5663-48f5-9905-d79b1c8de202"; //LAT
            iapGuid = "43558534-e698-465d-b675-50fff857de1f"; //HESH
            
            
            var iap = Globals.FeatureViewer_GetFeature(FeatureType.InstrumentApproachProcedure,
                new Guid(iapGuid)) as InstrumentApproachProcedure;

            var segLegList = new List<SegmentLeg>();

            foreach (var procTrans in iap.FlightTransition) {
                foreach (var procTransLeg in procTrans.TransitionLeg) {
                    var segLeg = Globals.FeatureViewer_GetFeature(
                        (FeatureType)procTransLeg.TheSegmentLeg.Type,
                        procTransLeg.TheSegmentLeg.Identifier) as SegmentLeg;

                    segLegList.Add(segLeg);
                }
            }

            var segLegTrajList = new List<SegLegTrajectory>();

            foreach (var segLeg in segLegList) {
                var segLegTraj = ToSegLegTrajectory(segLeg);
                if (segLegTraj != null)
                    segLegTrajList.Add(segLegTraj);
            }

            var fc = CreateSegmentLegFeatureClass(segLegTrajList);

            if (fc == null)
                return;

            var featLayer = new FeatureLayer() as IFeatureLayer;
            featLayer.Name = "SegmentLeg";
            featLayer.Visible = true;
            featLayer.FeatureClass = fc;
            featLayer.SpatialReference = _wgs84EsriSpRef;

            var rgbColor = new RgbColor() as IRgbColor;
            //rgbColor.RGB = 255000;
            rgbColor.Red = 255;
            rgbColor.Green = 255;
            rgbColor.Blue = 0;

            ISimpleLineSymbol simpleLineSymbol = new SimpleLineSymbolClass();
            simpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            simpleLineSymbol.Color = rgbColor;
            simpleLineSymbol.Width = 1;

            Globals.SetLayerSymbol(featLayer, simpleLineSymbol as ISymbol);

            ui_sceneControl.Scene.AddLayer(featLayer, true);

            var layerExtensions = (ILayerExtensions)featLayer;

            for (int i = 0; i < layerExtensions.ExtensionCount; i++) {
                if (layerExtensions.Extension[i] is ESRI.ArcGIS.Analyst3D.I3DProperties) {
                    var prop = layerExtensions.Extension[i] as E3D.I3DProperties;
                    prop.Apply3DProperties(featLayer);
                    break;
                }
            }
        }


        private IFeatureClass CreateSegmentLegFeatureClass (List<SegLegTrajectory> segLegTrajList)
        {
            var featWS = GetOrCreateWorkspace();

            if (featWS == null)
                return null;

            var wsEdit = featWS as IWorkspaceEdit;

            wsEdit.StartEditing(false);
            wsEdit.StartEditOperation();

            #region Fiels

            var fields = new Fields() as IFields;
            var fieldsEdit = fields as IFieldsEdit;

            #region OBJECTID

            var field = new FieldClass() as IField;
            var fieldEdit = field as IFieldEdit;
            fieldEdit.Name_2 = "OBJECTID";
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            fieldEdit.Editable_2 = false;
            fieldEdit.IsNullable_2 = false;
            fieldEdit.AliasName_2 = "Object ID";
            fieldsEdit.AddField(field);

            #endregion

            #region Shape

            field = new FieldClass() as IField;
            fieldEdit = field as IFieldEdit;
            fieldEdit.Name_2 = "SHAPE";
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;

            var geomDef = new GeometryDef() as IGeometryDefEdit;
            geomDef.GeometryType_2 = EG.esriGeometryType.esriGeometryPolyline;
            geomDef.SpatialReference_2 = _wgs84EsriSpRef;
            geomDef.HasZ_2 = true;
            geomDef.HasM_2 = false;

            fieldEdit.GeometryDef_2 = geomDef;
            fieldsEdit.AddField(field);

            #endregion

            #region Name

            field = new FieldClass() as IField;
            fieldEdit = field as IFieldEdit;
            fieldEdit.Name_2 = "Name";
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit.Length_2 = 255;
            fieldEdit.Editable_2 = true;
            fieldEdit.IsNullable_2 = true;
            fieldEdit.AliasName_2 = "Name";
            fieldsEdit.AddField(field);

            #endregion

            #endregion

            var clsId = new UIDClass();
            clsId.Value = "esriGeoDatabase.Feature";

            var fc = featWS.CreateFeatureClass("SegmentLeg", fields, clsId, null, esriFeatureType.esriFTSimple, "SHAPE", null);

            var buffFeatCursor = fc.Insert(true);

            var nameFieldIndex = fc.FindField("Name");

            foreach (var segLegTraj in segLegTrajList) {
                var featBuff = fc.CreateFeatureBuffer();

                featBuff.Shape = ToEsriPolyline(segLegTraj.Trajectory);

                var rowBuff = featBuff as IRowBuffer;
                rowBuff.set_Value(nameFieldIndex, segLegTraj.SegmentLeg.Identifier.ToString());

                buffFeatCursor.InsertFeature(featBuff);
            }

            Marshal.FinalReleaseComObject(buffFeatCursor);

            wsEdit.StopEditOperation();
            wsEdit.StopEditing(true);

            return fc;
        }

        private SegLegTrajectory ToSegLegTrajectory(SegmentLeg segLeg)
        {
            if (segLeg.Trajectory == null ||
                segLeg.Trajectory.Geo == null ||
                segLeg.Trajectory.Geo.Count == 0 ||
                segLeg.Trajectory.Geo[0].Count == 0) {
                return null;
            }

            var slTraj = new SegLegTrajectory();
            slTraj.SegmentLeg = segLeg;

            var traj = segLeg.Trajectory.Geo[0].Clone() as Ag.LineString;

            if (traj.Count == 2) {
                ValDistanceVertical vdvStart = null;
                ValDistanceVertical vdvEnd = null;

                if (segLeg.SegmentLegType != SegmentLegType.MissedApproachLeg) {
                    vdvStart = segLeg.UpperLimitAltitude;
                    vdvEnd = segLeg.LowerLimitAltitude;
                }
                else {
                    vdvStart = segLeg.LowerLimitAltitude;
                    vdvEnd = segLeg.UpperLimitAltitude;
                }

                traj[0].Z = ConverterToSI.Convert(vdvStart, 0);
                traj[1].Z = ConverterToSI.Convert(vdvEnd, 0);
            }
            else if (traj.Count > 2) {
                foreach (Ag.Point pt in traj) {
                    pt.Z = 100;
                }
            }

            slTraj.Trajectory = traj;

            return slTraj;
        }

        private IFeatureWorkspace GetOrCreateWorkspace()
        {
            var wsf = new AccessWorkspaceFactory() as IWorkspaceFactory;

            //var dir = Globals.TempDir;
            var dir = @"D:\ON\3D";
            var name = "SegmentLeg";

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var path = dir + "\\" + name + ".mdb";
            if (File.Exists(path)) {
                try {
                    File.Delete(path);
                    return null;
                }
                catch { }
            }

            var wn = wsf.Create(dir, name, null, 0);
            var featWS = (wn as IName).Open() as IFeatureWorkspace;
            return featWS;
        }

        private Eg.IPolyline ToEsriPolyline(Ag.LineString aranLineString)
        {
            var esriPolyline = new Eg.Polyline() as Eg.IPolyline;
            var ptColl = esriPolyline as Eg.IPointCollection;

            foreach (Ag.Point aranPoint in aranLineString) {
                
                var esriPoint = new Eg.Point() as Eg.IPoint;
                esriPoint.PutCoords(aranPoint.X, aranPoint.Y);
                (esriPoint as Eg.IZAware).ZAware = true;
                esriPoint.Z = aranPoint.Z;

                ptColl.AddPoint(esriPoint);
            }

            (esriPolyline as Eg.IZAware).ZAware = true;

            return esriPolyline;
        }
	}

    public class SegLegTrajectory
    {
        public SegmentLeg SegmentLeg { get; set; }

        public Ag.LineString Trajectory { get; set; }
    }
}
