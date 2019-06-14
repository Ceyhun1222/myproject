using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.SystemUI;
using System.Drawing;
using ESRI.ArcGIS.Carto;
using Aran.Aim.Features;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using Aran.Queries.Viewer;
using Aran.Queries.Common;
using Aran.Aim.FeatureInfo;
using System.IO;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.ADF.BaseClasses;
using MapEnv.Layers;

namespace MapEnv
{
    public class AimInfoTool : BaseTool
    {
        public AimInfoTool ()
        {
            //m_cursor = Cursors.Help;
            m_cursor = Globals.GetCursor ("aim_info");
            _featureGeomDict = new Dictionary<Feature, IGeometry> ();

            _geomSelectionColor = Color.Red;
        }

        public override void OnCreate (object hook)
        {
        }

        public override void OnMouseUp (int Button, int Shift, int X, int Y)
        {
            IPoint pt = Globals.MainForm.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint (X, Y);
            var mouseEnv = pt.Envelope;

			if (Globals.DbSpatialRef.Name != Globals.MainForm.Map.SpatialReference.Name)
				mouseEnv.SpatialReference = Globals.MainForm.Map.SpatialReference;

			double d = Globals.MainForm.ActiveView.ScreenDisplay.DisplayTransformation.FromPoints (4);
			mouseEnv.Expand (d, d, false);

            var featDict = new Dictionary<Guid, Feature> ();
            var featGeomDict = new Dictionary<Feature, IGeometry> ();

			foreach (Toc.AimLayer aimLayer in Globals.MainForm.AimLayers)
            {
				var layer = aimLayer.Layer;

                if (layer.Visible)
                {
					if (layer is AimFeatureLayer)
					{
						var mfl = layer as AimFeatureLayer;
						var featList = mfl.GetOverPoints (mouseEnv, featGeomDict);

						foreach (var feat in featList)
						{
							if (!featDict.ContainsKey (feat.Identifier))
								featDict.Add (feat.Identifier, feat);
						}
					}
				}
            }

            try
            {
                if (featDict.Values.Count > 0)
                {
                    if (_featureInfo == null)
                    {
                        _featureInfo = new ROFeatureViewer ();
                        _featureInfo.FormHiden += FeatureInfo_FormHiden;
                        _featureInfo.GettedFeature = Globals.FeatureViewer_GetFeature;
                        _featureInfo.SetOwner (Globals.MainForm);
                        _featureInfo.CurrentFeatureChanged += FeatureInfo_CurrentFeatureChanged;
                        Globals.MainForm.AddMapControlVisibleChanged (_featureInfo.MapControl_VisibleChanged);
                    }

                    _featureGeomDict.Clear ();
                    foreach (var item in featGeomDict.Keys)
                        _featureGeomDict.Add (item, featGeomDict [item]);

					if (Globals.Settings.FeatureInfoMode == FeatureInfoMode.Popup)
					{
						_featureInfo.ShowMapFeatures (featDict.Values,
								Globals.MainForm.MapContToScreen (new System.Drawing.Point (X, Y)));
					}
					else
					{
						_featureInfo.ShowFeaturesForm (featDict.Values, false);
					}
                }
            }
            catch (Exception ex)
            {
                Globals.ShowError (ex.Message);
            }
        }

        public void SetBitmap(Bitmap value)
        {
            m_bitmap = value;
        }

        public void SetCaption(string value)
        {
            m_caption = value;
            if (string.IsNullOrWhiteSpace(m_toolTip))
                m_toolTip = value;
            m_message = "";
        }


        private void FeatureInfo_FormHiden (object sender, EventArgs e)
        {
            ClearSelection ();
            Globals.MainForm.ActiveView.Refresh ();
        }

        private void FeatureInfo_CurrentFeatureChanged (object sender, EventArgs e)
        {
            var aimFeature = sender as Feature;

			IGeometry esriGeom;
            if (_featureGeomDict.TryGetValue (aimFeature, out esriGeom))
            {
                if (esriGeom.IsEmpty)
                    return;

				if (Globals.DbSpatialRef.Name != Globals.MainForm.Map.SpatialReference.Name)
				{
					esriGeom = esriGeom.Clone () as IGeometry;
					esriGeom.Project (Globals.MainForm.Map.SpatialReference);
				}

                if (esriGeom.GeometryType == esriGeometryType.esriGeometryPoint)
                {
                    DrawPoint (esriGeom);
                }
                else if (esriGeom.GeometryType == esriGeometryType.esriGeometryPolyline)
                {
                    DrawPolyline (esriGeom);
                }
                else if (esriGeom.GeometryType == esriGeometryType.esriGeometryPolygon)
                {
                    DrawPolygon (esriGeom);
                }
            }
        }

        private void DrawPoint (IGeometry esriGeom)
        {
            ClearSelection ();

            var pMarkerShpElement = new MarkerElement () as IMarkerElement;
            var pointElement = pMarkerShpElement as IElement;
            pointElement.Geometry = esriGeom;

            var pMarkerSym = new SimpleMarkerSymbol () as ISimpleMarkerSymbol;
            pMarkerSym.Color = Globals.ColorFromRGB (_geomSelectionColor.R, _geomSelectionColor.G, _geomSelectionColor.B);
            pMarkerSym.Size = 14;
            pMarkerSym.Style = esriSimpleMarkerStyle.esriSMSCircle;

            pMarkerShpElement.Symbol = pMarkerSym;

            DrawSelection (pointElement);
        }

        private void DrawPolygon (IGeometry esriGeom)
        {
            ClearSelection ();

            var pFillSym = new SimpleFillSymbol () as ISimpleFillSymbol;
            var pFillShpElement = new PolygonElement () as IFillShapeElement;

            var pElementofPoly = pFillShpElement as ESRI.ArcGIS.Carto.IElement;
            pElementofPoly.Geometry = esriGeom;

            pFillSym.Color = Globals.ColorFromRGB (_geomSelectionColor.R, _geomSelectionColor.G, _geomSelectionColor.B);
            pFillSym.Style = esriSimpleFillStyle.esriSFSNull; //'esriSFSDiagonalCross

            var pLineSimbol = new SimpleLineSymbol ();
            pLineSimbol.Color = pFillSym.Color;
            pLineSimbol.Width = 2;
            pFillSym.Outline = pLineSimbol;

            pFillShpElement.Symbol = pFillSym;

            DrawSelection (pElementofPoly);
        }

        private void DrawPolyline (IGeometry esriGeom)
        {
            ClearSelection ();

            var pLineElement = new LineElement () as ILineElement;
            var lineElement = pLineElement as IElement;
            lineElement.Geometry = esriGeom;

            var pLineSym = new SimpleLineSymbol ();
            pLineSym.Color = Globals.ColorFromRGB (_geomSelectionColor.R, _geomSelectionColor.G, _geomSelectionColor.B);
            pLineSym.Style = esriSimpleLineStyle.esriSLSSolid;
            pLineSym.Width = 2;

            pLineElement.Symbol = pLineSym;

            DrawSelection (lineElement);
        }

        private void DrawSelection (IElement element)
        {
            IGraphicsContainer pGraphics = Globals.MainForm.ActiveView.GraphicsContainer;
            pGraphics.AddElement (element, 0);
            Globals.MainForm.ActiveView.PartialRefresh (esriViewDrawPhase.esriViewGraphics, null, null);

            _selectionElement = element;
        }

        private void ClearSelection ()
        {
            if (_selectionElement != null)
            {
                try
                {
                    Globals.MainForm.ActiveView.GraphicsContainer.DeleteElement (_selectionElement);
                }
                catch { }

                _selectionElement = null;
            }
        }

        private ROFeatureViewer _featureInfo;
        private Dictionary<Feature, IGeometry> _featureGeomDict;
        private Color _geomSelectionColor;
        private IElement _selectionElement;
    }
}
