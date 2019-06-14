using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eg = ESRI.ArcGIS.Geometry;

namespace MapEnv
{
    public class GraphicsLayer : BaseDynamicLayer, IIdentify
    {
        private Dictionary<int, GeomSymbol> _geomDict;
        private Eg.IEnvelope _extent;


        public GraphicsLayer()
        {
            _geomDict = new Dictionary<int, GeomSymbol>();

            m_sName = "GraphicsLayer";


            _extent = new Eg.Envelope() as Eg.IEnvelope;
        }

        public int AddGeometry(Eg.IGeometry geom, ISymbol symbol)
        {
            return AddGeometry(geom, symbol, null);
        }

        public int AddGeometry(Eg.IPoint esriPoint, string text, ISymbol symbol)
        {
            return AddGeometry(esriPoint, symbol, text);
        }

        private int AddGeometry(Eg.IGeometry geom, ISymbol symbol, string text)
        {
            var random = new Random();
            var id = 0;

            do {
                id = random.Next(1, 1000);
            } while (_geomDict.ContainsKey(id));

            _geomDict.Add(id, new GeomSymbol() { Geom = geom, Symbol = symbol, Text = text });

            RefreshExtend();

            return id;
        }

        public void RemoveGeometry(int id)
        {
            if (_geomDict.ContainsKey(id))
                _geomDict.Remove(id);

            RefreshExtend();
        }

        public override void Draw(esriDrawPhase drawPhase, IDisplay Display, ITrackCancel trackCancel)
        {
            //base.Draw(drawPhase, Display, trackCancel);

            if (Display == null)
                return;

            var envelope = Display.DisplayTransformation.FittedBounds as Eg.IEnvelope;
            
            //short esriNoScreenCache = -1;
            //Display.StartDrawing(Display.hDC, esriNoScreenCache);

            foreach (var geomSym in _geomDict.Values) {
                Display.SetSymbol(geomSym.Symbol);
                var geom = geomSym.Geom;

                switch (geom.GeometryType) {
                    case Eg.esriGeometryType.esriGeometryPoint:
                        Display.DrawPoint(geom);

                        if (!string.IsNullOrEmpty(geomSym.Text)) {
                            try {
                                Display.DrawText(geom, geomSym.Text);
                            }
                            catch { }
                        }

                        break;
                    case Eg.esriGeometryType.esriGeometryPolyline:
                        Display.DrawPolyline(geom);
                        break;
                    case Eg.esriGeometryType.esriGeometryPolygon:
                        Display.DrawPolygon(geom);
                        break;
                }
            }

            //Display.FinishDrawing();
        }

        public override void DrawDynamicLayer(
            esriDynamicDrawPhase DynamicDrawPhase, 
            IDisplay Display, 
            IDynamicDisplay DynamicDisplay)
        {
            
        }

        public ESRI.ArcGIS.esriSystem.IArray Identify(Eg.IGeometry pGeom)
        {
            return null;
        }

        public override Eg.IEnvelope Extent
        {
            get
            {
                return _extent;
            }
        }

        public void RefreshLayer()
        {
            Globals.MainForm.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Globals.GraphicsLayer, null);
        }
    
    
        private void RefreshExtend()
        {
            if (_geomDict.Count == 0)
                return;

            var geomList = _geomDict.Values.ToList();
            _extent = (geomList[0].Geom.Envelope as IClone).Clone() as Eg.IEnvelope;

            for (int i = 1; i < geomList.Count; i++) {
                _extent.Union(geomList[i].Geom.Envelope);
            }
        }
    }

    public class GeomSymbol
    {
        public Eg.IGeometry Geom { get; set; }
        public ISymbol Symbol { get; set; }
        public string Text { get; set; }
    }
}
