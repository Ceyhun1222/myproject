#define CUSTOM_GRAPHICS_

using System;
using System.Collections.Generic;
using Aran.AranEnvironment;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using Aran.Geometries;
using Aran.Converters;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries.SpatialReferences;
using Aran.Geometries.Operators;

namespace MapEnv
{
    internal class AranGraphics : AranDrawingBase, IAranGraphics
    {
        public AranGraphics (AxMapControl mapControl):base()
        {
            _axMapControl = mapControl;
            _spatialRefConverter = new SpatRefConverter();
            _mapToolDict = new Dictionary<MapTool, MyTool>();
        }


        public PointSymbol SelectedPointSymbol { get; set; }

        public LineSymbol SelectedLineSymbol { get; set; }

        public FillSymbol SelectedFillSymbol { get; set; }

        public SpatialReference ViewProjection
        {
            get
            {
                _spatialReference = _spatialRefConverter.FromEsriSpatRef (_axMapControl.SpatialReference);
                return _spatialReference;
            }
            set
            {
                _axMapControl.SpatialReference = _spatialRefConverter.ToEsriSpatRef (value);
            }
        }

        public SpatialReference WGS84SR
        {
            get
            {
                if (_wgs84SR == null)
                {
                    var esriWGS84 = Globals.CreateWGS84SR ();
                    SpatRefConverter srConv = new SpatRefConverter ();
                    _wgs84SR = srConv.FromEsriSpatRef (esriWGS84);
                }
                return _wgs84SR;
            }
        }

        public void GetExtent (out double xmin, out double ymin, out double xmax, out double ymax)
        {
            IEnvelope extent = ActiveView.Extent;

            xmin = extent.XMin;
            xmax = extent.XMax;
            ymin = extent.YMin;
            ymax = extent.YMax;
        }

        public Box Extent
        {
            get
            {
                IEnvelope extent = ActiveView.Extent;
                Box box = new Box ();
                box [0] = new Aran.Geometries.Point (extent.XMin, extent.YMin);
                box [1] = new Aran.Geometries.Point (extent.XMax, extent.YMax);
                return box;
            }
            set
            {
                IEnvelope env = new Envelope () as IEnvelope;
                env.XMin = value [0].X;
                env.YMin = value [0].Y;
                env.XMax = value [1].X;
                env.YMax = value [1].Y;

                ActiveView.Extent = env;
            }
        }


        public void SetExtent (double xmin, double ymin, double xmax, double ymax)
        {
            IEnvelope extent = ActiveView.Extent;
            extent.XMin = xmin;
            extent.XMax = xmax;
            extent.YMin = ymin;
            extent.YMax = ymax;
        }

        //private void ConvertSpatRefToEsriSpatRef ( SpatialReference spatialReference )
        //{
        //    IGeographicCoordinateSystem geogCoordSystem;
        //    ISpatialReferenceFactory spatRefFactory = new SpatialReferenceEnvironmentClass ( ) as ISpatialReferenceFactory;
        //    IProjection projection;
        //    int paramCount;
        //    int esriParamCount;
        //    IParameter [ ] parameters = new IParameter [ 21 ];
        //    ILinearUnit linearUnit = null;
        //    IProjectedCoordinateSystem projCS;
        //    IProjectedCoordinateSystemEdit pcsEdit;
        //    IUnit projectedXYUnit;

        //    _mapControl.SpatialReference = null;
        //    if ( spatRefFactory == null )
        //        return;

        //    if ( spatialReference.SpatialReferenceType == SpatialReferenceType.srtGeographic )
        //    {
        //        geogCoordSystem = spatRefFactory.CreateGeographicCoordinateSystem ( ConvertSpatialReferenceGeoType ( spatialReference.Ellipsoid.srGeoType ) );
        //        spatRefFactory = null;
        //        _mapControl.SpatialReference = geogCoordSystem as ISpatialReference;
        //        geogCoordSystem = null;
        //    }
        //    else
        //    {
        //        projection = spatRefFactory.CreateProjection ( ConvertSpatialReferenceType ( spatialReference.SpatialReferenceType ) );

        //        if ( projection == null )
        //        {
        //            spatRefFactory = null;
        //            return;
        //        }
        //        paramCount = spatialReference.ParamList.Count;
        //        esriParamCount = paramCount;

        //        if ( esriParamCount < 20 )
        //            esriParamCount = 20;

        //        for ( int i = 0; i <= paramCount - 1; i++ )
        //        {
        //            parameters [ i ] = spatRefFactory.CreateParameter ( ConvertSpatialReferenceParamType ( spatialReference.ParamList [ i ].srParamType ) );
        //            parameters [ i ].Value = spatialReference.ParamList [ i ].value;
        //        }

        //        geogCoordSystem = spatRefFactory.CreateGeographicCoordinateSystem ( ConvertSpatialReferenceGeoType ( spatialReference.Ellipsoid.srGeoType ) );

        //        if ( geogCoordSystem == null )
        //        {
        //            SetNull ( parameters, paramCount, projection, linearUnit, spatRefFactory, geogCoordSystem );
        //            return;
        //        }

        //        projectedXYUnit = spatRefFactory.CreateUnit ( ConvertSpatialReferenceUnit ( spatialReference.SpatialReferenceUnit ) );
        //        if ( projectedXYUnit == null )
        //        {
        //            SetNull ( parameters, paramCount, projection, linearUnit, spatRefFactory, geogCoordSystem );
        //            return;
        //        }


        //        linearUnit = projectedXYUnit as ILinearUnit;

        //        if ( linearUnit == null )
        //        {
        //            SetNull ( parameters, paramCount, projection, linearUnit, spatRefFactory, geogCoordSystem );
        //            return;
        //        }

        //        projCS = new ProjectedCoordinateSystemClass ( ) as IProjectedCoordinateSystem;
        //        if ( projCS == null )
        //        {
        //            SetNull ( parameters, paramCount, projection, linearUnit, spatRefFactory, geogCoordSystem );
        //            return;
        //        }

        //        pcsEdit = projCS as IProjectedCoordinateSystemEdit;

        //        if ( pcsEdit == null )
        //        {
        //            projCS = null;
        //            SetNull ( parameters, paramCount, projection, linearUnit, spatRefFactory, geogCoordSystem );
        //            return;
        //        }

        //        pcsEdit.DefineEx ( spatialReference.name, spatialReference.name, "", "", "", geogCoordSystem, linearUnit, projection, parameters [ 0 ] );

        //        _mapControl.SpatialReference = projCS as ISpatialReference;
        //    }
        //}

        //private void SetNull ( IParameter [ ] parameters, int paramCount, IProjection projection, ILinearUnit linearUnit,
        //                            ISpatialReferenceFactory spatRefFactory, IGeographicCoordinateSystem geogCoordSystem )
        //{
        //    for ( int i = 0; i <= paramCount - 1; i++ )
        //    {
        //        parameters [ i ] = null;
        //    }
        //    projection = null;
        //    linearUnit = null;
        //    spatRefFactory = null;
        //    geogCoordSystem = null;
        //}

        //private int ConvertSpatialReferenceGeoType ( SpatialReferenceGeoType pandaSrGeoType )
        //{			
        //    esriSRGeoCSType[] esriSRGeoCsTypes = new esriSRGeoCSType[]{esriSRGeoCSType.esriSRGeoCS_WGS1984,esriSRGeoCSType.esriSRGeoCS_Krasovsky1940,esriSRGeoCSType.esriSRGeoCS_NAD1983};
        //    if ( ( int ) pandaSrGeoType < ( int ) SpatialReferenceGeoType.srgtWGS1984 ||
        //            ( int ) pandaSrGeoType > ( int ) SpatialReferenceGeoType.srgtNAD1983 )
        //        return ( int ) esriSRGeoCSType.esriSRGeoCS_WGS1984;
        //    else
        //        return ( int ) esriSRGeoCsTypes [ ( int ) pandaSrGeoType ];
        //}

        //private int ConvertSpatialReferenceType ( SpatialReferenceType pandaSpatRefType )
        //{
        //    esriSRProjectionType[] esriSpatRefProjTypes = new esriSRProjectionType[]{0, esriSRProjectionType.esriSRProjection_Mercator,esriSRProjectionType.esriSRProjection_TransverseMercator,esriSRProjectionType.esriSRProjection_GaussKruger};
        //    if ( ( int ) pandaSpatRefType < ( int ) SpatialReferenceType.srtMercator || ( int ) pandaSpatRefType > ( int ) SpatialReferenceType.srtGauss_Krueger )
        //        return 0;
        //    else
        //        return ( int ) esriSpatRefProjTypes [ ( int ) pandaSpatRefType ];
        //}

        //private int ConvertSpatialReferenceUnit ( SpatialReferenceUnit pandaSpatRefUnit )
        //{
        //    esriSRUnitType [ ] esriSpatRefUnitTypes = new esriSRUnitType [ ] { esriSRUnitType.esriSRUnit_Meter, esriSRUnitType.esriSRUnit_Foot, esriSRUnitType.esriSRUnit_NauticalMile, esriSRUnitType.esriSRUnit_Kilometer };
        //    if ( ( int ) pandaSpatRefUnit < ( int ) SpatialReferenceUnit.sruMeter || ( int ) pandaSpatRefUnit > ( int ) SpatialReferenceUnit.sruKilometer )
        //        return 0;
        //    else
        //        return ( int ) esriSpatRefUnitTypes [ ( int ) pandaSpatRefUnit ];
        //}

        //private int ConvertSpatialReferenceParamType ( SpatialReferenceParamType pandaSpatRefParamType )
        //{
        //    esriSRParameterType [ ] esriSpatRefParamTypes = new esriSRParameterType [ ]
        //        {esriSRParameterType.esriSRParameter_FalseEasting, esriSRParameterType.esriSRParameter_FalseNorthing, 
        //        esriSRParameterType.esriSRParameter_ScaleFactor, esriSRParameterType.esriSRParameter_Azimuth, 
        //        esriSRParameterType.esriSRParameter_CentralMeridian, esriSRParameterType.esriSRParameter_LatitudeOfOrigin,
        //        esriSRParameterType.esriSRParameter_LongitudeOfCenter};
        //    if ( ( int ) pandaSpatRefParamType < ( int ) SpatialReferenceParamType.srptFalseEasting || ( int ) pandaSpatRefParamType < ( int ) SpatialReferenceParamType.srptFalseEasting )
        //        return 0;
        //    else
        //        return ( int ) esriSpatRefParamTypes [ ( int ) pandaSpatRefParamType ];
        //}

        //private SpatialReference GetSpatialReference ( )
        //{
        //    if ( _spatialReference != null )
        //        return _spatialReference;
        //    _spatialReference = new SpatialReference ( );
        //    IGeographicCoordinateSystem geogCoordSystem = null;
        //    IProjection projection;
        //    ILinearUnit coordinateUnit;
        //    ISpheroid spheroid = null;
        //    ISpatialReference spatRefShp;
        //    if ( _mapControl.SpatialReference == null )
        //        return null;
        //    IProjectedCoordinateSystem projectedCoordSystem = _mapControl.SpatialReference as IProjectedCoordinateSystem;
        //    if ( projectedCoordSystem != null )
        //    {
        //        geogCoordSystem = projectedCoordSystem.GeographicCoordinateSystem;
        //        projection = projectedCoordSystem.Projection as IProjection;
        //        if ( projection != null )
        //        {
        //            if ( projectedCoordSystem.Projection.Name == "Transverse_Mercator" )
        //                _spatialReference.SpatialReferenceType = SpatialReferenceType.srtTransverse_Mercator;
        //            else if ( projectedCoordSystem.Projection.Name == "Mercator" )
        //                _spatialReference.SpatialReferenceType = SpatialReferenceType.srtMercator;
        //            else if ( projectedCoordSystem.Projection.Name == "Gauss_Krueger" )
        //                _spatialReference.SpatialReferenceType = SpatialReferenceType.srtGauss_Krueger;
        //        }
        //        coordinateUnit = projectedCoordSystem.CoordinateUnit as ILinearUnit;
        //        if ( coordinateUnit != null )
        //        {
        //            if ( projectedCoordSystem.CoordinateUnit.Name == "Meter" )
        //                _spatialReference.SpatialReferenceUnit = SpatialReferenceUnit.sruMeter;
        //        }
        //    }

        //    if ( geogCoordSystem == null )
        //    {
        //        geogCoordSystem = _mapControl.SpatialReference as IGeographicCoordinateSystem;
        //        if ( geogCoordSystem != null )
        //            _spatialReference.SpatialReferenceType = SpatialReferenceType.srtGeographic;
        //    }

        //    if ( geogCoordSystem != null )
        //    {
        //        spheroid = geogCoordSystem.Datum.Spheroid;
        //        spatRefShp = geogCoordSystem;
        //    }
        //    _spatialReference.Ellipsoid.isValid = ( spheroid != null );

        //    if ( _spatialReference.Ellipsoid.isValid )
        //    {
        //        if ( spheroid.Name == "WGS_1984" )
        //            _spatialReference.Ellipsoid.srGeoType = SpatialReferenceGeoType.srgtWGS1984;
        //        _spatialReference.Ellipsoid.semiMajorAxis = spheroid.SemiMajorAxis;
        //        _spatialReference.Ellipsoid.flattening = spheroid.Flattening;
        //    }

        //    switch ( _spatialReference.SpatialReferenceType )
        //    {
        //        case SpatialReferenceType.srtGeographic:
        //            _spatialReference.SpatialReferenceUnit = SpatialReferenceUnit.sruMeter;
        //            break;
        //        case SpatialReferenceType.srtMercator:
        //            break;
        //        case SpatialReferenceType.srtTransverse_Mercator:
        //            _spatialReference.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptFalseEasting, projectedCoordSystem.FalseEasting ) );
        //            _spatialReference.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptFalseNorthing, projectedCoordSystem.FalseNorthing ) );
        //            _spatialReference.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptCentralMeridian, projectedCoordSystem.CentralMeridian [ true ] ) );
        //            _spatialReference.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptLatitudeOfOrigin, 0.0 ) );
        //            _spatialReference.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptScaleFactor, projectedCoordSystem.ScaleFactor ) );
        //            break;
        //        case SpatialReferenceType.srtGauss_Krueger:
        //            break;
        //        default:
        //            break;
        //    }

        //    projectedCoordSystem = null;
        //    geogCoordSystem = null;
        //    spatRefShp = null;
        //    projection = null;
        //    coordinateUnit = null;
        //    spheroid = null;

        //    return _spatialReference;
        //}

      

        public bool SelectSymbol (BaseSymbol inSymbol, out BaseSymbol outSymbol, int hwnd)
        {
            outSymbol = null;
            return false;
        }

        public void SetMapTool (MapTool mapTool)
        {
            if (mapTool == null)
            {
                Globals.MainForm.SetCurrentTool2 (null);
                return;
            }

            MyTool myTool;
            if (!_mapToolDict.TryGetValue (mapTool, out myTool))
            {
                myTool = new MyTool (mapTool);
                _mapToolDict.Add (mapTool, myTool);
            }

            Globals.MainForm.SetCurrentTool2 (myTool);
        }
        
        public List<Geometry> GetSelectedGraphicGeometries()
        {
            var list = new List<Geometry>();

            var containerSelect = _axMapControl.Map as IGraphicsContainerSelect;
            if (containerSelect != null) {
                for (int i = 0; i < containerSelect.ElementSelectionCount; i++) {
                    var esriGeom = containerSelect.SelectedElement(i).Geometry;

                    try {
                        var aranGeom = ConvertFromEsriGeom.ToGeometry(esriGeom, (_spatialReference.SpatialReferenceType == SpatialReferenceType.srtGeographic));
                        if (aranGeom != null)
                            list.Add(aranGeom);
                    }
                    catch { }
                }
            }

            return list;
        }


        protected override IGraphicsContainer GraphicContainer
        {
            get
            {
                return ActiveView.GraphicsContainer;
            }
        }

        protected override IActiveView ActiveView
        {
            get
            {
                return _axMapControl.ActiveView;
            }
        }

        private AxMapControl _axMapControl;
        private SpatRefConverter _spatialRefConverter;
        private SpatialReference _spatialReference;
        private Dictionary<MapTool, MyTool> _mapToolDict;
        private SpatialReference _wgs84SR;
    }
}

