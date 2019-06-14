//using System;
//using System.Collections.Generic;
//using System.Text;
//using Aran.Aim;
//using Aran.Aim.Features;
//using Aran.Aim.Metadata.Geo;
//using Aran.Aim.Metadata.UI;
//using Aran.Aim.Utilities;
//using Aran.Converters;
//using ESRI.ArcGIS.Display;
//using ESRI.ArcGIS.esriSystem;
//using ESRI.ArcGIS.Geometry;
//using Aran.Package;
//using Aran.Aim.Data.Filters;

//namespace MapEnv.Layers
//{
//    public class AimTable : IPackable
//    {
//        public AimTable()
//        {
//            Rows = new List<AimRow>();

//            _shapeInfoList = new List<TableShapeInfo>();
//            Visible = true;

//            Globals.Environment.MapSpatialReferenceChanged += new EventHandler(Map_SpatialReferenceChanged);
//        }

//        public bool Open(FeatureType featureType)
//        {
//            _featureType = featureType;
//            _classInfo = UIMetadata.Instance.GetClassInfo((int)_featureType);
//            _geoClassInfo = GeoMetadata.GetGeoInfoByAimInfo(_classInfo);

//            if (string.IsNullOrWhiteSpace(Name))
//                Name = _featureType.ToString();

//            if (_geoClassInfo == null)
//                return false;

//            _mapSpatialReference = Globals.MapSpatialReference;

//            return true;
//        }

//        public void AddFeatures(IEnumerable<Feature> features)
//        {
//            if (features == null)
//                return;

//            foreach (Feature feature in features) {
//                AimRow aimRow = new AimRow();
//                aimRow.AimFeature = feature;
//                Rows.Add(aimRow);
//            }

//            Refresh();
//        }

//        public void Refresh()
//        {
//            if (Rows.Count == 0)
//                return;

//            try {
//                var geoPropInfosList = new List<AimPropInfo[]>();
//                var textPropInfosList = new List<AimPropInfo[]>();
//                var symbolPropInfosList = new List<AimPropInfo[]>();

//                for (int i = 0; i < _shapeInfoList.Count; i++) {
//                    TableShapeInfo shapeInfo = _shapeInfoList[i];

//                    var geoPropInfos = GetInnerProps((int)_featureType, shapeInfo.GeoProperty);
//                    var textPropInfos = GetInnerProps((int)_featureType, shapeInfo.TextProperty);
//                    var symbolPropInfos = GetInnerProps((int)_featureType, shapeInfo.CategorySymbol.PropertyName);

//                    geoPropInfosList.Add(geoPropInfos);
//                    textPropInfosList.Add(textPropInfos);
//                    symbolPropInfosList.Add(symbolPropInfos);
//                }

//                for (int n = 0; n < Rows.Count; n++) {
//                    AimRow aimRow = Rows[n];
//                    Feature feature = aimRow.AimFeature;
//                    IAimObject aimObject = (IAimObject)feature;

//                    aimRow.RowShapeList.Clear();

//                    for (int i = 0; i < _shapeInfoList.Count; i++) {
//                        List<IAimProperty> geoPropValueList = AimMetadataUtility.GetInnerPropertyValue(aimObject, geoPropInfosList[i], true);
//                        List<IAimProperty> textPropValueList = AimMetadataUtility.GetInnerPropertyValue(aimObject, textPropInfosList[i], true);
//                        List<IAimProperty> symbolValuePropValueList = AimMetadataUtility.GetInnerPropertyValue(aimObject, symbolPropInfosList[i], true);

//                        AimRowShape rowShape = new AimRowShape();

//                        #region Add Geometry
//                        foreach (IAimProperty geoPropValue in geoPropValueList) {
//                            if (geoPropValue.PropertyType == AimPropertyType.AranField) {
//                                var aranGeom = (Aran.Geometries.Geometry)((IEditAimField)geoPropValue).FieldValue;

//                                ShapePair geoPair = new ShapePair();
//                                geoPair.Geo = ConvertToEsriGeom.FromGeometry(aranGeom);
//                                geoPair.Geo.SpatialReference = Globals.GeoWGS84_SpatialRef;
//                                geoPair.Prj = geoPair.Geo.Clone();

//                                if (_mapSpatialReference != null &&
//                                    geoPair.Geo.SpatialReference.FactoryCode != _mapSpatialReference.FactoryCode) {
//                                    geoPair.Prj.Project(_mapSpatialReference);
//                                }

//                                if (!geoPair.Prj.IsEmpty) {
//                                    rowShape.Shapes.Add(geoPair);
//                                }
//                            }
//                        }
//                        #endregion

//                        rowShape.Text = GetFirstValueAsString(textPropValueList);
//                        rowShape.SymbolValue = GetFirstValueAsString(symbolValuePropValueList);

//                        aimRow.RowShapeList.Add(rowShape);
//                    }
//                }
//            }
//            catch (Exception ex) {
//                throw ex;
//            }
//        }

//        public List<AimRow> Rows { get; private set; }

//        public void Draw(IDisplay Display)
//        {
//            try {
//                //var gr = System.Drawing.Graphics.FromHdc (new IntPtr (Display.hDC));
//                //var textFont = new System.Drawing.Font ("Arial", 10);

//                IEnvelope envelope = Display.DisplayTransformation.FittedBounds as IEnvelope;
//                double textPosDelta = Display.DisplayTransformation.FromPoints(12);

//                foreach (AimRow aimRow in Rows) {
//                    for (int i = 0; i < aimRow.RowShapeList.Count; i++) {
//                        AimRowShape rowShape = aimRow.RowShapeList[i];
//                        TableShapeInfo shapeInfo = _shapeInfoList[i];

//                        foreach (ShapePair shapePair in rowShape.Shapes) {
//                            if (EnvelopeOper.Intersected(envelope, shapePair.Prj)) {
//                                IGeometry geom = shapePair.Prj;
//                                ISymbol geomSymbol = null;

//                                if (aimRow.IsSelected)
//                                    geomSymbol = GetSelectedSymbol(geom.GeometryType);
//                                else
//                                    geomSymbol = shapeInfo.CategorySymbol.GetSymbol(rowShape.SymbolValue);

//                                Display.SetSymbol(geomSymbol);

//                                switch (geom.GeometryType) {
//                                    case esriGeometryType.esriGeometryPoint: {
//                                            IPoint point = geom as IPoint;
//                                            Display.DrawPoint(point);

//                                            if (rowShape.Text != null && shapeInfo.TextSymbol != null) {
//                                                IPoint txtPoint = new Point();
//                                                txtPoint.PutCoords(point.X, point.Y - textPosDelta);

//                                                //var tds = shapeInfo.TextSymbol as ITextDrawSupport;
//                                                //if (tds != null)
//                                                //{
//                                                //    var res = tds.GetDrawPoints (Display.hDC, Display.DisplayTransformation, txtPoint);
//                                                //}

//                                                Display.SetSymbol(shapeInfo.TextSymbol as ISymbol);
//                                                Display.DrawText(txtPoint, rowShape.Text);
//                                                //int x, y;
//                                                //Display.DisplayTransformation.FromMapPoint (point, out x, out y);

//                                                //gr.DrawString (rowShape.Text, textFont, System.Drawing.Brushes.Black, x, y);
//                                            }
//                                        }
//                                        break;
//                                    case esriGeometryType.esriGeometryPolyline:
//                                        Display.DrawPolyline(geom as IPolyline);
//                                        break;
//                                    case esriGeometryType.esriGeometryPolygon:
//                                        Display.DrawPolygon(geom as IPolygon);
//                                        break;
//                                }
//                            }
//                        }
//                    }
//                }

//                //gr.Dispose ();
//            }
//            catch (Exception ex) {
//                System.Console.WriteLine("error:" + ex.Message);
//            }
//        }

//        public FeatureType FeatureType
//        {
//            get { return _featureType; }
//        }

//        public string Name { get; set; }

//        public string LayerDescription
//        {
//            get
//            {
//                //Must add Documentation property to UIClassInfo and get from that property.
//                return _classInfo.UiInfo().Caption;
//            }
//        }

//        public bool Visible { get; set; }

//        public List<TableShapeInfo> ShapeInfoList
//        {
//            get { return _shapeInfoList; }
//        }

//        public Filter Filter
//        {
//            get { return _filter; }
//            set { _filter = value; }
//        }

//        public AimPropInfo[] GetInnerProps(int aimTypeIndex, string propertyName)
//        {
//            if (propertyName == null)
//                return new AimPropInfo[0];
//            return AimMetadataUtility.GetInnerProps(aimTypeIndex, propertyName);
//        }

//        public AimRowShape[] GetAimRowShape(Feature feature)
//        {
//            foreach (AimRow row in Rows) {
//                if (row.AimFeature == feature)
//                    return row.RowShapeList.ToArray();
//            }
//            return null;
//        }

//        public void FillBaseInfo(AimTable srcTable)
//        {
//            Name = srcTable.Name;
//            Visible = srcTable.Visible;
//            ShapeInfoList.AddRange(srcTable.ShapeInfoList);
//            Filter = srcTable.Filter;
//        }


//        public static void SetGlobalEnvironment(object value)
//        {
//            Globals.Environment = value as Aran.AranEnvironment.IAranEnvironment;
//            GlobalSymbols.FillSelectedSymbols();
//        }


//        private void Map_SpatialReferenceChanged(object sender, EventArgs e)
//        {
//            ISpatialReference value = Globals.MapSpatialReference;

//            int valueCode = (value != null ? value.FactoryCode : -1);
//            int thisCode = (_mapSpatialReference != null ? _mapSpatialReference.FactoryCode : -1);

//            _mapSpatialReference = value;

//            if (valueCode == thisCode)
//                return;

//            foreach (AimRow aimRow in Rows)
//                aimRow.ToProject(_mapSpatialReference);
//        }

//        private string GetFirstValueAsString(List<IAimProperty> propValueList)
//        {
//            if (propValueList.Count > 0 &&
//                        propValueList[0] != null &&
//                        propValueList[0].PropertyType == AimPropertyType.AranField) {
//                return ((IEditAimField)propValueList[0]).FieldValue.ToString();
//            }

//            return null;
//        }

//        private ISymbol GetSelectedSymbol(esriGeometryType esriGeometryType)
//        {
//            switch (esriGeometryType) {
//                case esriGeometryType.esriGeometryPoint:
//                    return GlobalSymbols.SelectedPointSymbol;
//                case esriGeometryType.esriGeometryPolyline:
//                    return GlobalSymbols.SelectedLineSymbol;
//                case esriGeometryType.esriGeometryPolygon:
//                    return GlobalSymbols.SelectedFillSymbol;
//            }
//            throw new Exception("Selected Symbol Not Defined (type: " + esriGeometryType + ")");
//        }

//        #region IPackable

//        void IPackable.Pack(PackageWriter writer)
//        {
//            writer.PutInt32((int)_featureType);
//            writer.PutString(Name);
//            writer.PutBool(Visible);

//            writer.PutInt32(_shapeInfoList.Count);
//            foreach (TableShapeInfo shapeInfo in _shapeInfoList)
//                (shapeInfo as IPackable).Pack(writer);

//            bool filterNotNull = (Filter != null);
//            writer.PutBool(filterNotNull);
//            if (filterNotNull)
//                Filter.Pack(writer);
//        }

//        void IPackable.Unpack(PackageReader reader)
//        {
//            _featureType = (FeatureType)reader.GetInt32();
//            Name = reader.GetString();
//            Visible = reader.GetBool();

//            int count = reader.GetInt32();
//            for (int i = 0; i < count; i++) {
//                TableShapeInfo shapeInfo = new TableShapeInfo();
//                (shapeInfo as IPackable).Unpack(reader);
//                _shapeInfoList.Add(shapeInfo);
//            }

//            bool filterNotNull = reader.GetBool();
//            if (filterNotNull)
//                Filter = LayerPackage.UnpackFilter(reader);
//        }

//        #endregion

//        private FeatureType _featureType;
//        private AimClassInfo _classInfo;
//        private GeoClassInfo _geoClassInfo;
//        private ISpatialReference _mapSpatialReference;
//        private Filter _filter;
//        private List<TableShapeInfo> _shapeInfoList;
//    }
//}
