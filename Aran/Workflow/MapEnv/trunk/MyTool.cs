using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.SystemUI;
using System.Drawing;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.Carto;
using Aran.Aim.Features;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using Aran.Queries.Viewer;
using Aran.Queries.Common;
using Aran.Aim.FeatureInfo;
using System.IO;
using Aran.AranEnvironment;
using MapEnv.Layers;

namespace MapEnv
{
    public class MyTool : BaseTool
    {
        public MyTool (MapTool mapTool)
        {
            _mapTool = mapTool;
            m_cursor = mapTool.Cursor;
        }

        public MapTool MapTool { get { return _mapTool; } }

        public override void OnCreate (object hook)
        {
            _mapTool.OnCreate (hook);
        }

        public override void OnMouseUp (int Button, int Shift, int X, int Y)
        {
            _mapTool.OnMouseUp (Button, Shift, X, Y, MapPointConverter);
        }


        private ISpatialReference EsriSR_WGS84
        {
            get
            {
                if (_esriSR_wgs84 == null)
                    _esriSR_wgs84 = Globals.CreateWGS84SR ();
                return _esriSR_wgs84;
            }
        }

        private void MapPointConverter (object sender, EventArgs e)
        {
            var objArr = sender as object [];
            if (objArr == null || objArr.Length != 4)
                return;

            var x = (int) objArr [0];
            var y = (int) objArr [1];
            var mapPoint = objArr [2] as Aran.Geometries.Point;
            var geoPoint = objArr [3] as Aran.Geometries.Point;

            IPoint esriMapPoint = Globals.MainForm.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint (x, y);
            IPoint esriGeoPoint = esriMapPoint.Clone () as IPoint;
            esriGeoPoint.Project (EsriSR_WGS84);

            mapPoint.X = esriMapPoint.X;
            mapPoint.Y = esriMapPoint.Y;

            geoPoint.X = esriGeoPoint.X;
            geoPoint.Y = esriGeoPoint.Y;
        }

        private MapTool _mapTool;
        private ISpatialReference _esriSR_wgs84;
    }
}
