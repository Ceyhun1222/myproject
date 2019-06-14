using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;

namespace MapEnv.Toc
{
    public class AimLayer
    {
        public event EventHandler VisibleChanged;
        public event LoadingEventHandler LoadingChanged;


        public AimLayer()
        {

        }

        public AimLayer(ILayer layer)
        {
            SetLayer(layer);
        }


        public AimLayerType LayerType { get; private set; }

        public ILayer Layer
        {
            get { return _layer; }
            set
            {
                SetLayer(value);
            }
        }

        public bool IsAimLayer
        {
            get { return AimLayer.IsLayerTypeAime(LayerType); }
        }

        public static bool IsLayerTypeAime(AimLayerType layerType)
        {
            return (layerType < AimLayerType.EsriFeature);
        }

        public List<AimFeatureLayer> MyFeatLayerList { get; private set; }


        private void SetLayer(ILayer layer)
        {
            _layer = layer;

            if (layer is AimFeatureLayer) {
                LayerType = AimLayerType.AimSimpleShapefile;

                var mfl = layer as AimFeatureLayer;
                mfl.UpdateStarted += Layer_UpdateStarted;
                mfl.UpdateEnded += Layer_UpdateEnded;
            }
            else if (layer is ESRI.ArcGIS.Carto.IGeoFeatureLayer) {
                LayerType = AimLayerType.EsriFeature;
            }
            else if (layer is ESRI.ArcGIS.Carto.IRasterLayer) {
                LayerType = AimLayerType.EsriRaster;
            }
            else if (layer is IWMSLayer) {
            }
            else if (layer is IWMSGroupLayer) {
                LayerType = AimLayerType.EsriWMS;
            }

            var le = layer as ILayerEvents_Event;
            if (le != null)
                le.VisibilityChanged += Layer_VisibilityChanged;
        }

        private void Layer_UpdateStarted(object sender, EventArgs e)
        {
            if (LoadingChanged != null)
                LoadingChanged(this, new LoadingEventArgs(true));
        }

        private void Layer_UpdateEnded(object sender, EventArgs e)
        {
            if (LoadingChanged != null)
                LoadingChanged(this, new LoadingEventArgs(false));
        }

        private void Layer_VisibilityChanged(bool currentState)
        {
            if (VisibleChanged != null)
                VisibleChanged(this, new EventArgs());
        }

        private ILayer _layer;
    }

    public enum AimLayerType
    {
        AimSample,
        AimComplex,
        AimSimpleShapefile,
        EsriFeature,
        EsriRaster,
        EsriWMS
    }

    public delegate void LoadingEventHandler(object sender, LoadingEventArgs e);

    public class LoadingEventArgs : EventArgs
    {
        public LoadingEventArgs(bool isLoading)
        {
            IsLoading = isLoading;
        }

        public bool IsLoading { get; private set; }
    }
}
