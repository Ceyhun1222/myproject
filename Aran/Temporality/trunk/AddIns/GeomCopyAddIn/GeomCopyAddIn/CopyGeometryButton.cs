using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using GeomCopyAddIn.Wkt;

namespace GeomCopyAddIn
{
    public class CopyGeometryButton : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public CopyGeometryButton()
        {
        }


        #region Click Command
        protected override void OnClick()
        {
            ArcMap.Application.CurrentTool = null;

            if (ArcMap.Application != null)
            {
                var pMxDoc = ArcMap.Document;
                if (pMxDoc != null)
                {
                    var map = pMxDoc.FocusMap;
                    if (map != null)
                    {
                        var pEnumFeat = (IEnumFeature)map.FeatureSelection;
                        if (pEnumFeat != null)
                        {
                            pEnumFeat.Reset();
                            var pfeat = pEnumFeat.Next();
                            List<IGeometry> geometries = new List<IGeometry>();
                            while (pfeat != null)
                            {
                                if (pfeat.Shape != null)
                                {
                                    switch (pfeat.Shape.GeometryType)
                                    {
                                        case esriGeometryType.esriGeometryPoint:
                                            geometries.Add(pfeat.Shape);
                                            //CopyToClipBoard(pfeat.Shape);
                                            break;
                                        //return;
                                        case esriGeometryType.esriGeometryPolyline:
                                            geometries.Add(pfeat.Shape);
                                            //CopyToClipBoard(pfeat.Shape);
                                            //return;
                                            break;
                                        case esriGeometryType.esriGeometryPolygon:
                                            geometries.Add(pfeat.Shape);
                                            //CopyToClipBoard(pfeat.Shape);
                                            //return;
                                            break;
                                    }
                                }
                                pfeat = pEnumFeat.Next();
                            }
                            CopyToClipBoard(geometries);
                        }
                    }
                }
            }
        }
        #       endregion

        #region OnUpdate
        protected override void OnUpdate()
        {
            if (ArcMap.Application != null)
            {
                var pMxDoc = ArcMap.Document;
                if (pMxDoc != null)
                {
                    var map = pMxDoc.FocusMap;
                    if (map != null)
                    {
                        var pEnumFeat = (IEnumFeature)map.FeatureSelection;
                        if (pEnumFeat != null)
                        {
                            pEnumFeat.Reset();
                            var pfeat = pEnumFeat.Next();
                            while (pfeat != null)
                            {
                                if (pfeat.Shape != null)
                                {
                                    switch (pfeat.Shape.GeometryType)
                                    {
                                        case esriGeometryType.esriGeometryPoint:
                                            Enabled = true;
                                            return;
                                        case esriGeometryType.esriGeometryPolyline:
                                            Enabled = true;
                                            return;
                                        case esriGeometryType.esriGeometryPolygon:
                                            Enabled = true;
                                            return;
                                    }
                                }
                                pfeat = pEnumFeat.Next();
                            }
                        }
                    }
                }
            }
            Enabled = false;
        }
        #endregion

        #region Clipboard
        private void CopyToClipBoard(IGeometry geometry)
        {
            ((ITopologicalOperator)geometry).Simplify();
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            var wkt = geometry.ToWellKnownText();
            Clipboard.SetData("Esri IGeometry", wkt);
        }

        private void CopyToClipBoard(List<IGeometry> geometries)
        {
            
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            geometries.ForEach( geometry => ((ITopologicalOperator)geometry).Simplify());
            List<string> result = new List<string>();
            foreach (var geometry in geometries)
        	{
		        result.Add(geometry.ToWellKnownText());
	        }
            Clipboard.SetData("Esri IGeometry List", result);
        }
        #endregion
    }

}
