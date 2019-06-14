using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aran.AranEnvironment
{
    public class MapTool
    {
        //***********************
        //*** As it will be required should be added other virtual methods derived by BaseTool
        //***********************

        public ToolMouseUpEventHandler MouseUp;

        public Cursor Cursor { get; set; }

        public virtual void OnCreate (object hook)
        {
        }

        public virtual void OnMouseUp (int button, int shift, int x, int y, EventHandler mapPointConverter)
        {
            if (MouseUp == null)
                return;

            var e = new ToolMouseUpEventArgs ();
            e.Button = button;
            e.Shift = shift;
            e.X = x;
            e.Y = y;
            e.GettedMapPoint += mapPointConverter;
            MouseUp (this, e);
        }
    }

    public delegate void ToolMouseUpEventHandler (object sender, ToolMouseUpEventArgs e);

    public class ToolMouseUpEventArgs : EventArgs
    {
        public event EventHandler GettedMapPoint;

        public int Button { get; set; }

        public int Shift { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public Aran.Geometries.Point GeoPoint
        {
            get
            {
                Geometries.Point mapPoint, geoPoint;
                if (GetPoints (out mapPoint, out geoPoint))
                    return geoPoint;
                return null;
            }
        }

        public Aran.Geometries.Point MapPoint
        {
            get
            {
                Geometries.Point mapPoint, geoPoint;
                if (GetPoints (out mapPoint, out geoPoint))
                    return mapPoint;
                return null;
            }
        }


        private bool GetPoints (out Geometries.Point mapPoint, out Geometries.Point geoPoint)
        {
            mapPoint = null;
            geoPoint = null;

            if (GettedMapPoint == null)
                return false;

            mapPoint = new Geometries.Point ();
            geoPoint = new Geometries.Point ();
            GettedMapPoint (new object [] { X, Y, mapPoint, geoPoint }, null);

            return true;
        }
    }
}
