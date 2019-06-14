using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using ESRI.ArcGIS.Display;
using MapEnv.Toc;
using System.Diagnostics;
using Carto = ESRI.ArcGIS.Carto;

namespace MapEnv.Controls
{
    public partial class TOCControl : UserControl
    {
        private List<AimLayer> _aimFeatureList;
        private List<KeyValuePair<AimLayer, TocItem>> _layerTocItemPairList;

        public event TocItemMenuEventHandler TocMenuClicked;

        public TOCControl ()
        {
            InitializeComponent ();

            _aimFeatureList = new List<AimLayer> ();
            _layerTocItemPairList = new List<KeyValuePair<AimLayer, TocItem>> ();

            var tocControlW = ui_elementHost.Child as TOCControlW;
            tocControlW.ItemMenuClicked += TocControlW_ItemMenuClicked;
            tocControlW.ItemReplaced = TocControlW_ItemReplaced;
        }
		
        public void AddLayerAsFirst (AimLayer layer)
        {
            _aimFeatureList.Insert (0, layer);
            TocItem tocItem = null;

            using (var gr = CreateGraphics ())
            {
                tocItem = TocGlobal.ToTocItem (layer, gr);
            }
            
            if (tocItem == null)
            	return;

            _layerTocItemPairList.Insert (0, new KeyValuePair<AimLayer, TocItem> (layer, tocItem));

            tocItem.PropertyChanged += TocItem_PropertyChanged;
            layer.VisibleChanged += Layer_VisibleChanged;
            layer.LoadingChanged += Layer_LoadingChanged;
                        
            (ui_elementHost.Child as TOCControlW).Insert (0, tocItem);
        }

		public void AddLayer (AimLayer layer)
		{
			_aimFeatureList.Add (layer);
			//_readOnlyColl = null;
			TocItem tocItem = null;

			using (var gr = CreateGraphics ())
			{
				tocItem = TocGlobal.ToTocItem (layer, gr);
			}

			if (tocItem == null)
				return;

			_layerTocItemPairList.Add (new KeyValuePair<AimLayer, TocItem> (layer, tocItem));

			tocItem.PropertyChanged += TocItem_PropertyChanged;
			layer.VisibleChanged += Layer_VisibleChanged;
			layer.LoadingChanged += Layer_LoadingChanged;

			(ui_elementHost.Child as TOCControlW).Add (tocItem);
		}

		public void RemoveLayer (AimLayer layer)
		{
			int index;
			TocItem tocItem = GetTocItem (layer, out index);
			if (tocItem == null)
				return;
			(ui_elementHost.Child as TOCControlW).Remove (tocItem);

			_aimFeatureList.Remove (layer);
			_layerTocItemPairList.RemoveAt (index);
		}

		public void RemoveEsriLayer (ESRI.ArcGIS.Carto.ILayer layer)
		{
			foreach (var item in _aimFeatureList)
			{
				if (item.Layer == layer)
				{
					RemoveLayer (item);
					break;
				}
			}
		}

        public void ClearLayers ()
        {
			_aimFeatureList.Clear ();
			_layerTocItemPairList.Clear ();
            ui_tocControlW.ClearItems ();
        }

        public ReadOnlyCollection<AimLayer> AimLayers
        {
            get { return new ReadOnlyCollection<AimLayer> (_aimFeatureList); }
        }

		public void ReOrderLayers ()
		{
			var map = Globals.MainForm.Map;
			var n = 0;
			for (int i = AimLayers.Count - 1; i >= 0 ; i--)
			{
				var item = AimLayers [i];

				if (item.Layer is AimFeatureLayer)
				{
					var aimFL = item.Layer as AimFeatureLayer;
					foreach (var li in aimFL.LayerInfoList)
						//map.MoveLayer (li.Layer, n++);
						map.MoveLayer (li.Layer, map.LayerCount - 1 - (n++));
				}
				else
					//map.MoveLayer (item.Layer, n++);
					map.MoveLayer (item.Layer, map.LayerCount - 1 - (n++));
			}
		}


        private void TocControlW_ItemMenuClicked (object sender, TocItemMenuEventArg e)
        {
            Debug.Assert (TocMenuClicked != null);

            foreach (var pair in _layerTocItemPairList)
            {
                if (pair.Value == e.TocItem)
                {
                    e.AimLayer = pair.Key;
                    break;
                }
            }

            TocMenuClicked (this, e);
        }

        private void TocControlW_ItemReplaced (object sender, TocItemReplacedEventArg e)
        {
            var srcAimLayer = GetLayer (e.SrcItem);
            var destAimLayer = GetLayer (e.DestItem);

			if (srcAimLayer == destAimLayer)
				return;

			var destAimIndex = AimLayers.IndexOf (destAimLayer);

			_aimFeatureList.Remove (srcAimLayer);
			_aimFeatureList.Insert (destAimIndex, srcAimLayer);
			e.Replaced = true;

			ReOrderLayers ();
        }

        private void TocItem_PropertyChanged (object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsVisible")
            {
                var tocItem = sender as TocItem;

                foreach (var pair in _layerTocItemPairList)
                {
                    if (pair.Value == tocItem)
                    {
                        var aimLayer = pair.Key;
                        aimLayer.VisibleChanged -= Layer_VisibleChanged;
                        aimLayer.Layer.Visible = tocItem.IsVisible;
                        Globals.MapEdited = true;
                        aimLayer.VisibleChanged += Layer_VisibleChanged;

                        Globals.Environment.Graphics.Refresh ();

                        break;
                    }
                }
            }
        }

        private void Layer_VisibleChanged (object sender, EventArgs e)
        {
            var aimLayer = sender as AimLayer;

            foreach (var pair in _layerTocItemPairList)
            {
                if (pair.Key == aimLayer)
                {
					bool isVisible = aimLayer.Layer.Visible;
					if (aimLayer.Layer is AimFeatureLayer)
						isVisible = !isVisible;

					pair.Value.IsVisible = isVisible;
                    Globals.MapEdited = true;
                    break;
                }
            }
        }

        private void Layer_LoadingChanged (object sender, LoadingEventArgs e)
        {
            var aimLayer = sender as AimLayer;

            foreach (var pair in _layerTocItemPairList)
            {
                if (pair.Key == aimLayer)
                {
                    pair.Value.IsLoading = e.IsLoading;
                    break;
                }
            }
        }

        private AimLayer GetLayer (TocItem tocItem)
        {
            foreach (var pair in _layerTocItemPairList)
            {
                if (pair.Value == tocItem)
                    return pair.Key;
            }
            return null;
        }

		private TocItem GetTocItem (AimLayer layer, out int index)
		{
			index = -1;
			for (int i = 0; i < _layerTocItemPairList.Count; i++)
			{
				var pair = _layerTocItemPairList [i];
				if (pair.Key == layer)
				{
					index = i;
					return pair.Value;
				}
			}
			return null;
		}
	}
}
