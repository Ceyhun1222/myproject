using Aran.Delta.Model;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;
using System.Windows.Interop;

namespace Aran.Delta
{
    public class DesigningLayerDbModule
    {
        public DesigningLayerDbModule()
        {

        }

        public List<Model.DesigningArea> GetDesigningAreas()
        {
            var result = new List<Model.DesigningArea>();
            var designingAreaLayer = EsriFunctions.GetLayerByName(GlobalParams.Map, "Designing_Area");
            if (designingAreaLayer == null)
            {
                Model.Messages.Warning("Cannot find Designing_Area Layer!");
                return result;
            }

            var featLayer = designingAreaLayer as IFeatureLayer;
            var featClass = featLayer.FeatureClass;
            var featCursor = featClass.Search(null, true);
            IFeature feat = featCursor.NextFeature();
            while (feat != null)
            {
                var designingArea = new Model.DesigningArea();
                var fields = feat.Fields;
                designingArea.Name = (string)feat.get_Value(fields.FindField("NAME"));
                designingArea.CodeType = (string)feat.get_Value(fields.FindField("CODE_TYPE"));
                designingArea.LowerLimit = (double)feat.get_Value(fields.FindField("LOWER_LIMIT"));
                designingArea.UomLowerLimit = (string)feat.get_Value(fields.FindField("UOM_LOWER_LIMIT"));
                designingArea.UpperLimit = (double)feat.get_Value(fields.FindField("UPPER_LIMIT"));
                designingArea.UomUpperLimit = (string)feat.get_Value(fields.FindField("UOM_UPPER_LIMIT"));
                designingArea.Designer = (string)feat.get_Value(fields.FindField("DESIGNER"));

                if (fields.FindField("FeatIdentifier") > -1)
                {
                    var guidVal = feat.Value[fields.FindField("FeatIdentifier")].ToString();
                    Guid outVal;
                    if (Guid.TryParse(guidVal, out outVal))
                        designingArea.FeatIdentifier = outVal;
                } 
                designingArea.Geo = Aran.Converters.ConvertFromEsriGeom.ToGeometry(feat.Shape, true);
                result.Add(designingArea);
                feat = featCursor.NextFeature();
            }
            return result;
        }

        private List<Model.DesigningRoute> _designingRoutes;

        public List<Model.DesigningRoute> DesigningRoutes
        {
            get 
            {
                if (_designingRoutes == null)
                {
                    _designingRoutes = GetDesigningRoutes();
                }
                return _designingRoutes; 
            }
            
        }
     
        public List<Model.DesigningSegment> GetDesigningSegments(string routeName) 
        {
            if (DesigningRoutes != null) 
            {
                var designingRoute = DesigningRoutes.FirstOrDefault(route => route.Name == routeName);
                if (designingRoute != null)
                    return designingRoute.SegmentList;
            
            }
            return null;
        }

        public List<Model.DesigningBuffer> GetDesigningBuffers()
        {
            var result = new List<Model.DesigningBuffer>();
            var designingBufferLayer = EsriFunctions.GetLayerByName(GlobalParams.Map, "Designing_Buffer");
            if (designingBufferLayer == null)
            {
                Model.Messages.Warning("Cannot find Designing_Buffer Layer!");
                return result;
            }

            var featLayer = designingBufferLayer as IFeatureLayer;
            var featClass = featLayer.FeatureClass;
            var featCursor = featClass.Search(null, true);
            IFeature feat = featCursor.NextFeature();
            while (feat != null)
            {
                var designingBuffer = new Model.DesigningBuffer();
                var fields = feat.Fields;
                designingBuffer.Name = (string)feat.get_Value(fields.FindField("NAME"));
                designingBuffer.CodeType = (string)feat.get_Value(fields.FindField("CODE_TYPE"));
                designingBuffer.LowerLimit = (double)feat.get_Value(fields.FindField("LOWER_LIMIT"));
                designingBuffer.UomLowerLimit = (string)feat.get_Value(fields.FindField("UOM_LOWER_LIMIT"));
                designingBuffer.UpperLimit = (double)feat.get_Value(fields.FindField("UPPER_LIMIT"));
                designingBuffer.UomUpperLimit = (string)feat.get_Value(fields.FindField("UOM_UPPER_LIMIT"));
                designingBuffer.Designer = (string)feat.get_Value(fields.FindField("DESIGNER"));
                designingBuffer.BufferWidth = (double)feat.get_Value(fields.FindField("BufferWidth"));
                if (fields.FindField("BufferWidthUom") > 0)
                {
                    var value = feat.get_Value(fields.FindField("BufferWidthUom"));
                    if (value != DBNull.Value)
                        designingBuffer.UOMBufferWidth = (string)value;
                }

                if (fields.FindField("FeatIdentifier") > -1)
                {
                    var guidVal = feat.Value[fields.FindField("FeatIdentifier")].ToString();
                    Guid outVal;
                    if (Guid.TryParse(guidVal, out outVal))
                        designingBuffer.FeatIdentifier = outVal;
                }

                designingBuffer.Geo = Aran.Converters.ConvertFromEsriGeom.ToGeometry(feat.Shape);
                result.Add(designingBuffer);
                feat = featCursor.NextFeature();
            }
            return result;
        }

        public List<Model.DesigningPoint> GetDesigningPoints()
        {
            var result = new List<Model.DesigningPoint>();
            var designingPointLayer = EsriFunctions.GetLayerByName(GlobalParams.Map, "Designing_Point");
            if (designingPointLayer == null)
            {
                Model.Messages.Warning("Cannot find Designing_Point Layer!");
                return result;
            }

            var featLayer = designingPointLayer as IFeatureLayer;
            var featClass = featLayer.FeatureClass;
            var featCursor = featClass.Search(null, true);
            IFeature feat = featCursor.NextFeature();
            while (feat != null)
            {
                var designingPoint = new Model.DesigningPoint();
                var fields = feat.Fields;
                designingPoint.Name = (string)feat.get_Value(fields.FindField("NAME"));
                designingPoint.Designer = (string)feat.get_Value(fields.FindField("DESIGNER"));

                if (fields.FindField("FeatIdentifier") > -1)
                {
                    var guidVal = feat.Value[fields.FindField("FeatIdentifier")].ToString();
                    Guid outVal;
                    if (Guid.TryParse(guidVal, out outVal))
                        designingPoint.FeatIdentifier = outVal;
                }

                designingPoint.Geo = Aran.Converters.ConvertFromEsriGeom.ToGeometry(feat.Shape);
                result.Add(designingPoint);
                feat = featCursor.NextFeature();
            }
            return result;
        }

        public bool SavePoint(Model.DesigningPoint designingPoint)
        {
            try
            {
                if (designingPoint == null)
                    return false;

                var designingPointLayer = EsriFunctions.GetLayerByName(GlobalParams.Map, "Designing_Point");
                if (designingPointLayer == null)
                {
                    Model.Messages.Warning("Cannot find Designing_Point Layer!");
                    return false;
                }

                var workspaceEdit = EsriFunctions.GetWorkspace(GlobalParams.Map);
                if (!workspaceEdit.IsBeingEdited())
                {
                    workspaceEdit.StartEditing(false);
                    workspaceEdit.StartEditOperation();
                }

                var window = new View.Save.SaveDesigninPoint(designingPoint);

                var helper = new WindowInteropHelper(window);
                helper.Owner = new IntPtr(GlobalParams.HWND);
                ElementHost.EnableModelessKeyboardInterop(window);
                window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
                if (window.ShowDialog() == true)
                {
                    var featLayer = designingPointLayer as IFeatureLayer;
                    var featClass = featLayer.FeatureClass;
                    var feat = featClass.CreateFeature();

                    var fields = feat.Fields;

                    //    var aranPrjPt = GlobalParams.SpatialRefOperation.ToPrj((Aran.Geometries.Point)designingPoint.Geo);

                    var esriPrjPt =
                        Aran.Converters.ConvertToEsriGeom.FromPoint((Aran.Geometries.Point) designingPoint.Geo,true);

                    feat.set_Value(1, esriPrjPt);
                    if (designingPoint.Designer != null)
                    {
                        int tmpIndex = fields.FindField("Designer");
                        if (tmpIndex > 0)
                            feat.set_Value(tmpIndex, designingPoint.Designer);
                    }

                    if (designingPoint.Name != null)
                    {
                        int tmpIndex = fields.FindField("Name");
                        if (tmpIndex > 0)
                            feat.set_Value(tmpIndex, designingPoint.Name);
                    }

                    if (designingPoint.Lat != null)
                    {
                        int tmpIndex = fields.FindField("Lat");
                        if (tmpIndex > 0)
                            feat.set_Value(tmpIndex, designingPoint.Lat);
                    }

                    if (designingPoint.Lon != null)
                    {
                        int tmpIndex = fields.FindField("Lon");
                        if (tmpIndex > 0)
                            feat.set_Value(tmpIndex, designingPoint.Lon);
                    }

                    var dateIndex = fields.FindField("InputDate");
                    if (dateIndex > -1)
                        feat.set_Value(dateIndex, DateTime.Now);

                    var identifierIndex = fields.FindField("FeatIdentifier");
                    if (identifierIndex > -1)
                        feat.set_Value(identifierIndex, Guid.NewGuid().ToString());

                    feat.Store();

                    workspaceEdit.StopEditOperation();
                    workspaceEdit.StopEditing(true);

                }
                return true;
            }
            catch (Exception e)
            {
                Messages.Error("Error when trying to Save!" + e.Message);
                return false;
            }

        }
      
        public bool SaveArea(Model.DesigningArea designingArea)
        {
            try
            {
                if (designingArea == null)
                    return false;

                var designingAreaLayer = EsriFunctions.GetLayerByName(GlobalParams.Map, "Designing_Area");
                if (designingAreaLayer == null)
                {
                    Model.Messages.Warning("Cannot find Designing_Area Layer!");
                    return false;
                }

                var workspaceEdit = EsriFunctions.GetWorkspace(GlobalParams.Map);
                if (!workspaceEdit.IsBeingEdited())
                {
                    workspaceEdit.StartEditing(false);
                    workspaceEdit.StartEditOperation();
                }

                var window = new View.Save.SaveDesigningArea(designingArea);

                var helper = new WindowInteropHelper(window);
                helper.Owner = new IntPtr(GlobalParams.HWND);
                ElementHost.EnableModelessKeyboardInterop(window);
                window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
                if (window.ShowDialog() == true)
                {
                    var featLayer = designingAreaLayer as IFeatureLayer;
                    var featClass = featLayer.FeatureClass;
                    var feat = featClass.CreateFeature();

                    var fields = feat.Fields;

                    //    var aranPrjPt = GlobalParams.SpatialRefOperation.ToPrj((Aran.Geometries.Point)designingArea.Geo);

                    var esriPrjGeo = Aran.Converters.ConvertToEsriGeom.FromGeometry(designingArea.Geo);

                    feat.set_Value(1, esriPrjGeo);
                    if (designingArea.Designer != null)
                    {
                        int tmpIndex = fields.FindField("Designer");
                        if (tmpIndex > -1)
                            feat.set_Value(tmpIndex, designingArea.Designer);
                    }

                    if (designingArea.Name != null)
                    {
                        int tmpIndex = fields.FindField("Name");
                        if (tmpIndex > -1)
                            feat.set_Value(tmpIndex, designingArea.Name);
                    }

                    if (designingArea.LowerLimit != double.NaN)
                    {
                        int tmpIndex = fields.FindField("LOWER_LIMIT");
                        if (tmpIndex > -1)
                            feat.set_Value(tmpIndex, designingArea.LowerLimit);
                    }

                    if (designingArea.UpperLimit != double.NaN)
                    {
                        int tmpIndex = fields.FindField("UPPER_LIMIT");
                        if (tmpIndex > -1)
                            feat.set_Value(tmpIndex, designingArea.UpperLimit);
                    }

                    if (designingArea.UomLowerLimit != null)
                    {
                        int tmpIndex = fields.FindField("UOM_LOWER_LIMIT");
                        if (tmpIndex > -1)
                            feat.set_Value(tmpIndex, designingArea.UomLowerLimit);
                    }

                    if (designingArea.UomUpperLimit != null)
                    {
                        int tmpIndex = fields.FindField("UOM_UPPER_LIMIT");
                        if (tmpIndex > -1)
                            feat.set_Value(tmpIndex, designingArea.UomUpperLimit);
                    }

                    if (designingArea.CodeType != null)
                    {
                        int tmpIndex = fields.FindField("CODE_TYPE");
                        if (tmpIndex > -1)
                            feat.set_Value(tmpIndex, designingArea.CodeType);
                    }

                    var dateIndex =fields.FindField("InputDate");
                    if (dateIndex >-1)
                        feat.set_Value(dateIndex, DateTime.Now);

                    var identifierIndex = fields.FindField("FeatIdentifier");
                    if (identifierIndex >-1)
                        feat.set_Value(identifierIndex, Guid.NewGuid().ToString());

                    feat.Store();

                    workspaceEdit.StopEditOperation();
                    workspaceEdit.StopEditing(true);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Messages.Error("Error when trying to Save!" + e.Message);
                return false;
            }
        }

        public bool SaveBuffer(Model.DesigningBuffer designingBuffer)
        {
            try
            {

                if (designingBuffer == null)
                    return false;

                var designingBufferLayer = EsriFunctions.GetLayerByName(GlobalParams.Map, "Designing_Buffer");
                if (designingBufferLayer == null)
                {
                    Model.Messages.Warning("Cannot find Designing_Buffer Layer!");
                    return false;
                }

                var workspaceEdit = EsriFunctions.GetWorkspace(GlobalParams.Map);
                if (!workspaceEdit.IsBeingEdited())
                {
                    workspaceEdit.StartEditing(false);
                    workspaceEdit.StartEditOperation();
                }

                var window = new View.Save.SaveDesigningBuffer(designingBuffer);

                var helper = new WindowInteropHelper(window);
                helper.Owner = new IntPtr(GlobalParams.HWND);
                ElementHost.EnableModelessKeyboardInterop(window);
                window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
                if (window.ShowDialog() == true)
                {
                    var featLayer = designingBufferLayer as IFeatureLayer;
                    var featClass = featLayer.FeatureClass;
                    var feat = featClass.CreateFeature();

                    var fields = feat.Fields;

                    //    var aranPrjPt = GlobalParams.SpatialRefOperation.ToPrj((Aran.Geometries.Point)designingBuffer.Geo);

                    var esriPrjGeo = Aran.Converters.ConvertToEsriGeom.FromGeometry(designingBuffer.Geo);

                    feat.set_Value(1, esriPrjGeo);
                    if (designingBuffer.Designer != null)
                    {
                        int tmpIndex = fields.FindField("Designer");
                        if (tmpIndex > -1)
                            feat.set_Value(tmpIndex, designingBuffer.Designer);
                    }

                    if (designingBuffer.Name != null)
                    {
                        int tmpIndex = fields.FindField("Name");
                        if (tmpIndex > -1)
                            feat.set_Value(tmpIndex, designingBuffer.Name);
                    }

                    if (designingBuffer.LowerLimit != double.NaN)
                    {
                        int tmpIndex = fields.FindField("LOWER_LIMIT");
                        if (tmpIndex > -1)
                            feat.set_Value(tmpIndex, designingBuffer.LowerLimit);
                    }

                    if (designingBuffer.UpperLimit != double.NaN)
                    {
                        int tmpIndex = fields.FindField("UPPER_LIMIT");
                        if (tmpIndex > -1)
                            feat.set_Value(tmpIndex, designingBuffer.UpperLimit);
                    }

                    if (designingBuffer.UomLowerLimit != null)
                    {
                        int tmpIndex = fields.FindField("UOM_LOWER_LIMIT");
                        if (tmpIndex > -1)
                            feat.set_Value(tmpIndex, designingBuffer.UomLowerLimit);
                    }

                    if (designingBuffer.UomUpperLimit != null)
                    {
                        int tmpIndex = fields.FindField("UOM_UPPER_LIMIT");
                        if (tmpIndex > -1)
                            feat.set_Value(tmpIndex, designingBuffer.UomUpperLimit);
                    }

                    if (designingBuffer.CodeType != null)
                    {
                        int tmpIndex = fields.FindField("CODE_TYPE");
                        if (tmpIndex > -1)
                            feat.set_Value(tmpIndex, designingBuffer.CodeType);
                    }

                    if (designingBuffer.BufferWidth != null)
                    {
                        int tmpIndex = fields.FindField("BufferWidth");
                        if (tmpIndex > -1)
                            feat.set_Value(tmpIndex, designingBuffer.BufferWidth);
                    }

                    if (designingBuffer.UOMBufferWidth != null)
                    {
                        int tmpIndex = fields.FindField("BufferWidthUom");
                        if (tmpIndex > -1)
                            feat.set_Value(tmpIndex, designingBuffer.UOMBufferWidth);
                    }

                    if (designingBuffer.MarkerLayer != null)
                    {
                        int tmpIndex = fields.FindField("MakerLayer");
                        if (tmpIndex > -1)
                            feat.set_Value(tmpIndex, designingBuffer.MarkerLayer);
                    }
                    if (designingBuffer.MarkerObjectName != null)
                    {
                        int tmpIndex = fields.FindField("MakerObjectName");
                        if (tmpIndex > -1)
                            feat.set_Value(tmpIndex, designingBuffer.MarkerObjectName);
                    }

                    var dateIndex = fields.FindField("InputDate");
                    if (dateIndex >-1)
                        feat.set_Value(dateIndex, DateTime.Now);

                    var identifierIndex = fields.FindField("FeatIdentifier");
                    if (identifierIndex >-1)
                        feat.set_Value(identifierIndex, Guid.NewGuid().ToString());

                    feat.Store();

                    workspaceEdit.StopEditOperation();
                    workspaceEdit.StopEditing(true);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Messages.Error("Error when trying to Save!" + e.Message);
                return false;
            }
        }

        public bool SaveRoute(Model.DesigningRoute designingRoute)
        {
            try
            {

                if (designingRoute == null)
                    return false;

                var designingSagmentLayer = EsriFunctions.GetLayerByName(GlobalParams.Map, "Designing_Segment");
                if (designingSagmentLayer == null)
                {
                    Model.Messages.Warning("Cannot find Designing_Segment Layer!");
                    return false;
                }

                var workspaceEdit = EsriFunctions.GetWorkspace(GlobalParams.Map);
                if (!workspaceEdit.IsBeingEdited())
                {
                    workspaceEdit.StartEditing(false);
                    workspaceEdit.StartEditOperation();
                }

                var window = new View.Save.SaveSegment(designingRoute);

                var helper = new WindowInteropHelper(window);
                helper.Owner = new IntPtr(GlobalParams.HWND);
                ElementHost.EnableModelessKeyboardInterop(window);
                window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
                if (window.ShowDialog() == true)
                {
                    var featLayer = designingSagmentLayer as IFeatureLayer;
                    var featClass = featLayer.FeatureClass;
                    foreach (var segment in designingRoute.SegmentList)
                    {
                        var feat = featClass.CreateFeature();

                        var fields = feat.Fields;

                        //    var aranPrjPt = GlobalParams.SpatialRefOperation.ToPrj((Aran.Geometries.Point)designingBuffer.Geo);

                        var esriPrjGeo = Aran.Converters.ConvertToEsriGeom.FromGeometry(segment.Geo);

                        feat.set_Value(1, esriPrjGeo);

                        if (designingRoute.Name != null)
                        {
                            int tmpIndex = fields.FindField("Route_Name");
                            if (tmpIndex > 0)
                                feat.set_Value(tmpIndex, designingRoute.Name);
                        }

                    

                        if (designingRoute.Designer != null)
                        {
                            int tmpIndex = fields.FindField("Designer");
                            if (tmpIndex > 0)
                                feat.set_Value(tmpIndex, designingRoute.Designer);
                        }

                        if (segment.Name != null)
                        {
                            int tmpIndex = fields.FindField("Name");
                            if (tmpIndex > 0)
                                feat.set_Value(tmpIndex, segment.Name);
                        }

                        if (segment.ValLen != double.NaN)
                        {
                            int tmpIndex = fields.FindField("VAL_LEN");
                            if (tmpIndex > 0)
                                feat.set_Value(tmpIndex, segment.ValLen);
                        }

                        if (segment.ValueReverseTrueTrack != double.NaN)
                        {
                            int tmpIndex = fields.FindField("VALUE_REVERSE_TRUE_TRACK");
                            if (tmpIndex > 0)
                                feat.set_Value(tmpIndex, segment.ValueReverseTrueTrack);
                        }

                        if (segment.ValueTrueTrack != double.NaN)
                        {
                            int tmpIndex = fields.FindField("VALUE_TRUE_TRACK");
                            if (tmpIndex > 0)
                                feat.set_Value(tmpIndex, segment.ValueTrueTrack);
                        }

                        if (segment.UomDist != null)
                        {
                            int tmpIndex = fields.FindField("UOM_DIST");
                            if (tmpIndex > 0)
                                feat.set_Value(tmpIndex, segment.UomDist);
                        }

                        if (segment.CodeTypeSegPath != null)
                        {
                            int tmpIndex = fields.FindField("CODE_TYPE_SEG_PATH");
                            if (tmpIndex > 0)
                                feat.set_Value(tmpIndex, segment.CodeTypeSegPath);
                        }

                        if (segment.WptStart != null)
                        {
                            int tmpIndex = fields.FindField("WPT_START");
                            if (tmpIndex > 0)
                                feat.set_Value(tmpIndex, segment.WptStart);
                        }
                        if (segment.WptEnd != null)
                        {
                            int tmpIndex = fields.FindField("WPT_END");
                            if (tmpIndex > 0)
                                feat.set_Value(tmpIndex, segment.WptEnd);
                        }

                        int nameIndex = fields.FindField("Name");
                        if (nameIndex > 0)
                            feat.set_Value(nameIndex, segment.WptStart + "-" + segment.WptEnd);

                        var dateIndex = fields.FindField("InputDate");
                        if (dateIndex >-1)
                            feat.set_Value(dateIndex, DateTime.Now);

                        var identifierIndex = fields.FindField("FeatIdentifier");
                        if (identifierIndex >-1)
                            feat.set_Value(identifierIndex, Guid.NewGuid().ToString());

                        feat.Store();
                    }

                    workspaceEdit.StopEditOperation();
                    workspaceEdit.StopEditing(true);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Messages.Error("Error when trying to Save!" + e.Message);
                return false;
            }
        }


        private List<Model.DesigningRoute> GetDesigningRoutes()
        {
            try
            {
                var result = new List<Model.DesigningRoute>();
                var designingSegmentLayer = EsriFunctions.GetLayerByName(GlobalParams.Map, "Designing_Segment");
                if (designingSegmentLayer == null)
                {
                    Model.Messages.Warning("Cannot find Designing_Segment Layer!");
                    return result;
                }

                var featLayer = designingSegmentLayer as IFeatureLayer;
                var featClass = featLayer.FeatureClass;
                var featCursor = featClass.Search(null, true);
                IFeature feat = featCursor.NextFeature();
                var segmentList = new List<Model.DesigningSegment>();
                while (feat != null)
                {
                    var designingSegment = new Model.DesigningSegment();
                    var fields = feat.Fields;
                    designingSegment.Name = (string)feat.get_Value(fields.FindField("NAME"));
                    designingSegment.CodeTypeSegPath = (string)feat.get_Value(fields.FindField("CODE_TYPE_SEG_PATH"));
                    designingSegment.ValueTrueTrack = (double)feat.get_Value(fields.FindField("VALUE_TRUE_TRACK"));
                    designingSegment.ValueReverseTrueTrack = (double)feat.get_Value(fields.FindField("VALUE_REVERSE_TRUE_TRACK"));
                    designingSegment.WptStart = (string)feat.get_Value(fields.FindField("WPT_START"));
                    designingSegment.WptEnd = (string)feat.get_Value(fields.FindField("WPT_END"));
                    if (fields.FindField("Route_Name") > 0)
                    {
                        var value = feat.get_Value(fields.FindField("Route_Name"));
                        if (value != DBNull.Value)
                            designingSegment.RouteName = (string)feat.get_Value(fields.FindField("Route_Name"));
                    }

                    if (fields.FindField("FeatIdentifier") > -1)
                        designingSegment.FeatIdentifier = new Guid(feat.Value[fields.FindField("FeatIdentifier")].ToString());
             
                    designingSegment.ValLen = (double)feat.get_Value(fields.FindField("VAL_LEN"));
                    designingSegment.Geo = Aran.Converters.ConvertFromEsriGeom.ToGeometry(feat.Shape);
                    designingSegment .Designer = (string)feat.get_Value(fields.FindField("DESIGNER"));

                    if (fields.FindField("FeatIdentifier") > -1)
                    {
                        var guidVal = feat.Value[fields.FindField("FeatIdentifier")].ToString();
                        Guid outVal;
                        if (Guid.TryParse(guidVal, out outVal))
                            designingSegment.FeatIdentifier = outVal;
                    }

                    segmentList.Add(designingSegment);
                    feat = featCursor.NextFeature();
                }

                result = segmentList.GroupBy(segment => segment.RouteName).
                        Select(a => new DesigningRoute { Name = a.Key, SegmentList = a.ToList() }).ToList();

                return result;
            }
            catch (Exception e)
            {
                Messages.Error("Error when trying to load Designsegments!" + e.Message);
                return null;
            }
        }

    }
}
