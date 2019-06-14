using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using Aran.Aim;
 
namespace MapEnv
{
    #region EventArgs

    public class LayerAddedEventArgs : EventArgs
    {
        public LayerAddedEventArgs (ILayer layer, int index = -1)
        {
            Layer = layer;
			Index = index;
        }

		public int Index
		{
			get;
			set;
		}

        public ILayer Layer { get; private set; }
    }

    public class ZoomToEventArgs : EventArgs
    {
        public ZoomToEventArgs (IGeometry shape)
        {
            Shape = shape;
        }

        public IGeometry Shape { get; private set; }
    }

    public class PropertySelectedEventArgs : EventArgs
    {
        public PropertySelectedEventArgs (AimPropInfo [] selectedProp)
        {
            SelectedProp = selectedProp;
            Cancel = false;
        }

        public AimPropInfo [] SelectedProp { get; private set; }

        public bool Cancel { get; set; }
    }

    public class SelectorAddedPropInfoEventArgs : EventArgs
    {
        public SelectorAddedPropInfoEventArgs (AimPropInfo propInfo)
        {
            PropInfo = propInfo;
            AddToList = false;
        }

        public AimPropInfo PropInfo { get; private set; }

        public bool AddToList { get; set; }
    }


    #endregion

    #region Delegates

    public delegate void LayerAddedEventHandler (LayerAddedEventArgs e);
    public delegate void ZoomToEventHandler (object sender, ZoomToEventArgs e);
    public delegate void CommandEventHandler ();
    public delegate void PropertySelectedEventHandler (object sender, PropertySelectedEventArgs e);
    public delegate void SelectorAddedPropInfoEventHandler (object sender, SelectorAddedPropInfoEventArgs e);

    #endregion
}
