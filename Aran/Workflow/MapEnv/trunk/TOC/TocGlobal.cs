using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim;
using MapEnv.Layers;

namespace MapEnv.Toc
{
    public static class TocGlobal
    {
        static TocGlobal ()
        {
            BackColor = Globals.ToRGBColor (System.Drawing.Color.White).RGB;
            _compSymbolFeatTypeList = new List<FeatureType> ();
        }

        public static System.Drawing.Image LoadingImage
        {
            get
            {
                if (_loadingImage == null)
                {
                    _loadingImage = Properties.Resources.loader_16;
                }
                return _loadingImage;
            }
        }

        public static TocItem ToTocItem (AimLayer aimLayer, System.Drawing.Graphics gr)
        {
            TocItem tocItem = null;

            if (aimLayer.IsAimLayer) {
                var mfl = aimLayer.Layer as AimFeatureLayer;

                if (mfl.IsComplex) {
                    var aimTocItem = new AimComplexTocItem();
                    _compSymbolFeatTypeList.Clear();
                    FillComplexSymbol(mfl.BaseQueryInfo, aimTocItem, gr);
                    tocItem = aimTocItem;
                }
                else {
                    var aimTocItem = new AimSimpleTocItem();

                    foreach (var shapeInfo in mfl.ShapeInfoList) {
                        var symbol = shapeInfo.CategorySymbol.DefaultSymbol;
                        if (symbol == null)
                            continue;

                        var bitmap = StyleFunctions.SymbolToBitmap(symbol, new System.Drawing.Size(24, 24), gr, BackColor);

                        TocSymbolItem symbolItem = new TocSymbolItem();
                        symbolItem.SymbolImage = ToWpfImage(bitmap);
                        symbolItem.PropertyName = Globals.GeoPropertyNameCaption(shapeInfo.GeoProperty);

                        aimTocItem.SymbolItems.Add(symbolItem);
                    }

                    tocItem = aimTocItem;
                }
            }
            else if (aimLayer.Layer is ESRI.ArcGIS.Carto.IGeoFeatureLayer) {
                ESRI.ArcGIS.Carto.IUniqueValueRenderer uniqValRen;
                ESRI.ArcGIS.Carto.IClassBreaksRenderer classBreaksRen;
                var symbol = Globals.GetLayerSymbol(aimLayer.Layer as ESRI.ArcGIS.Carto.IGeoFeatureLayer, out uniqValRen, out classBreaksRen);

                if (symbol != null) {
                    var bitmap = StyleFunctions.SymbolToBitmap(symbol, new System.Drawing.Size(24, 24),
                        gr, Globals.ToRGBColor(System.Drawing.Color.White).RGB);

                    var esriTocItem = new EsriFeatureTocItem();
                    esriTocItem.SymbolImage = ToWpfImage(bitmap);
                    tocItem = esriTocItem;
                }
            }
            else if (aimLayer.Layer is ESRI.ArcGIS.Carto.IRasterLayer) {
                var esriRaster = aimLayer.Layer as ESRI.ArcGIS.Carto.IRasterLayer;
                var esriTocItem = new EsriFeatureTocItem(TocItemType.EsriRaster);
                esriTocItem.Name = esriRaster.Name;
                tocItem = esriTocItem;
            }
            else if (aimLayer.Layer is ESRI.ArcGIS.Carto.IWMSGroupLayer) {
                var esriTocItem = new EsriFeatureTocItem(TocItemType.EsriRaster);
                esriTocItem.Name = aimLayer.Layer.Name;
                tocItem = esriTocItem;
            }

            if (tocItem == null)
                return null;

            tocItem.Name = aimLayer.Layer.Name;
            tocItem.IsVisible = aimLayer.Layer.Visible;

            return tocItem;
        }

        public static System.Windows.Media.Imaging.BitmapImage ToWpfImage (System.Drawing.Image img)
        {
            var ms = new System.IO.MemoryStream ();  // no using here! BitmapImage will dispose the stream after loading
            img.Save (ms, System.Drawing.Imaging.ImageFormat.Bmp);

            var ix = new System.Windows.Media.Imaging.BitmapImage ();
            ix.BeginInit ();
            ix.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
            ix.StreamSource = ms;
            ix.EndInit ();
            return ix;
        }

        private static bool FillComplexSymbol (QueryInfo queryInfo, AimComplexTocItem aimTocItem, System.Drawing.Graphics gr)
        {
            if (aimTocItem.SymbolItems.Count > 5)
                return false;

            if (!_compSymbolFeatTypeList.Contains (queryInfo.FeatureType))
            {
                foreach (var shapeInfo in queryInfo.ShapeInfoList)
                {
                    var symbol = shapeInfo.CategorySymbol.DefaultSymbol;

                    if (symbol == null)
                        continue;

                    var bitmap = StyleFunctions.SymbolToBitmap (symbol, new System.Drawing.Size (24, 24), gr, BackColor);

                    TocSymbolItem symbolItem = new TocSymbolItem ();
                    symbolItem.SymbolImage = ToWpfImage (bitmap);
                    aimTocItem.SymbolItems.Add (symbolItem);

                    _compSymbolFeatTypeList.Add (queryInfo.FeatureType);
                }
            }

            foreach (var item in queryInfo.RefQueries)
            {
                if (!FillComplexSymbol (item.QueryInfo, aimTocItem, gr))
                    return false;
            }

            foreach (var item in queryInfo.SubQueries)
            {
                if (!FillComplexSymbol (item.QueryInfo, aimTocItem, gr))
                    return false;
            }

            return true;
        }

        private static int BackColor;
        private static List<FeatureType> _compSymbolFeatTypeList;
        private static System.Drawing.Image _loadingImage;

    }
}
