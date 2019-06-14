using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Aran.Aim;
using Aran.Aim.CAWProvider;
using Aran.Aim.Data;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using ESRI.ArcGIS.Carto;
using Aran.Aim.Env2.Layers;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;

namespace MapEnv.Plugin
{
    public class AixmFileLoader
    {
        public AixmFileLoader ()
        {

        }

        public bool Load (string fileName)
        {
            IDbProvider dbProvider = DbProviderFactory.Create ("Aran.Aim.Data.XmlProvider");

            dbProvider.Open (fileName);

            FeatureType [] featTypeArr = new FeatureType [] {
                FeatureType.AirportHeliport,
                FeatureType.RunwayCentrelinePoint,
                FeatureType.VOR,
                FeatureType.DME,
                FeatureType.DepartureLeg,
                FeatureType.MissedApproachLeg,
                FeatureType.IntermediateLeg,
                FeatureType.InitialLeg,
                FeatureType.FinalLeg,
                FeatureType.ArrivalLeg,
                FeatureType.DesignatedPoint };

            foreach (FeatureType featureType in featTypeArr)
            {
                GettingResult result = dbProvider.GetVersionsOf (featureType, TimeSliceInterpretationType.BASELINE);

                List<Feature> featureList = result.List as List<Feature>;

                if (featureList == null)
                    continue;

                AimSimpleLayer asl = new AimSimpleLayer ();
                FillShapeInfo (featureType, asl.AimTable.ShapeInfoList);

                asl.Open (featureType, featureList);

                var layer = new AranEsriLayer (asl) as ILayer;

                Globals.MainForm.Map.AddLayer (layer);
                Globals.OnLayerAdded (layer);
            }

            return true;
        }


        private void FillShapeInfo (FeatureType featureType, List<TableShapeInfo> shapeInfoList)
        {
            TableShapeInfo shapeInfo = new TableShapeInfo ();
            shapeInfo.GeoProperty = "Location/Geo";
            shapeInfo.TextSymbol = GetTextSymbol ();
            shapeInfo.TextProperty = "Designator";
            shapeInfo.CategorySymbol.DefaultSymbol = GetSymbol (esriGeometryType.esriGeometryPoint, featureType, 0);
            shapeInfoList.Add (shapeInfo);

            if (featureType == Aran.Aim.FeatureType.DesignatedPoint)
            {
                shapeInfo.CategorySymbol.DefaultSymbol = GetSymbol (esriGeometryType.esriGeometryPoint, featureType, 0);
                shapeInfo.CategorySymbol.PropertyName = "Type";
                shapeInfo.CategorySymbol.Symbols.Add ("DESIGNED", GetSymbol (esriGeometryType.esriGeometryPoint, featureType, 1));
            }
            else if (featureType == FeatureType.AirportHeliport)
            {
                shapeInfo.GeoProperty = "ARP/Geo";
            }
            else if (IsAbstractType (typeof (SegmentLegType), featureType))
            {
                shapeInfo.GeoProperty = "Trajectory/Geo";
                shapeInfo.CategorySymbol.DefaultSymbol = GetSymbol (esriGeometryType.esriGeometryPolyline, featureType, 0);

                shapeInfo = new TableShapeInfo ();
                shapeInfo.TextSymbol = GetTextSymbol ();
                shapeInfo.GeoProperty = "StartPoint/FacilityMakeup/FixToleranceArea/Geo";

                shapeInfo.CategorySymbol.DefaultSymbol = GetSymbol (esriGeometryType.esriGeometryPolygon, featureType, 0);

                shapeInfoList.Add (shapeInfo);

            }
        }

        private ISymbol GetSymbol (esriGeometryType esriGeomType, Aran.Aim.FeatureType featureType, int typeCode)
        {
            switch (esriGeomType)
            {
                case esriGeometryType.esriGeometryPolyline:
                    return GetPolylineSymbol (featureType, typeCode);
                case esriGeometryType.esriGeometryPolygon:
                    return GetPolygonSymbol (featureType, typeCode);
                default:
                    return GetPointSymbol (featureType, typeCode);
            }
        }

        private ISymbol GetPointSymbol (Aran.Aim.FeatureType featureType, int typeCode)
        {
            int charIndex = GetSymbolChar (featureType);

            if (charIndex != -1)
            {
                ICharacterMarkerSymbol charMrkSym = new CharacterMarkerSymbol ();
                stdole.IFontDisp fontDisp = (stdole.IFontDisp) (new stdole.StdFont ());
                fontDisp.Name = "RISK Aero";
                fontDisp.Size = 12;
                charMrkSym.Font = fontDisp;
                charMrkSym.Size = 12;
                charMrkSym.XOffset = 0;
                charMrkSym.YOffset = 0;
                charMrkSym.CharacterIndex = charIndex + typeCode;

                return charMrkSym as ISymbol;
            }
            else
            {
                ISimpleMarkerSymbol markerSymbol = new SimpleMarkerSymbol ();
                Random random = new Random ();
                markerSymbol.Color = Globals.ColorFromRGB (
                    random.Next (0, 255), random.Next (0, 255), random.Next (0, 255));
                markerSymbol.Size = 8;

                return markerSymbol as ISymbol;
            }
        }

        private ISymbol GetPolygonSymbol (Aran.Aim.FeatureType featureType, int typeCode)
        {
            ISimpleFillSymbol sfSym = new SimpleFillSymbol ();

            ISimpleLineSymbol sls = new SimpleLineSymbol ();
            Random random = new Random ();
            sls.Color = Globals.ColorFromRGB (
                        random.Next (0, 255), random.Next (0, 255), random.Next (0, 255));
            sls.Style = esriSimpleLineStyle.esriSLSSolid;

            sfSym.Outline = sls;
            sfSym.Style = esriSimpleFillStyle.esriSFSNull;

            return sfSym as ISymbol;
        }

        private ISymbol GetPolylineSymbol (Aran.Aim.FeatureType featureType, int typeCode)
        {
            ISimpleLineSymbol sls = new SimpleLineSymbol ();
            Random random = new Random ();
            sls.Color = Globals.ColorFromRGB (
                        random.Next (0, 255), random.Next (0, 255), random.Next (0, 255));
            sls.Style = esriSimpleLineStyle.esriSLSSolid;
            return sls as ISymbol;
        }

        private int GetSymbolChar (Aran.Aim.FeatureType featureType)
        {
            switch (featureType)
            {
                case Aran.Aim.FeatureType.AirportHeliport: return 61518;
                case Aran.Aim.FeatureType.DesignatedPoint: return 61511;
                case Aran.Aim.FeatureType.VOR: return 61505;
                case Aran.Aim.FeatureType.DME: return 61506;
                case Aran.Aim.FeatureType.VerticalStructure: return 61537;
            }
            return -1;
        }

        private ITextSymbol GetTextSymbol ()
        {
            ITextSymbol textSymbol = new TextSymbol ();
            stdole.IFontDisp fontDisp = (stdole.IFontDisp) (new stdole.StdFont ());
            fontDisp.Name = "Arial";
            fontDisp.Size = 8;
            textSymbol.Font = fontDisp;
            textSymbol.Text = "AaBbCc";
            return textSymbol;
        }

        private bool IsAbstractType (Type absEnumType, FeatureType featureType)
        {
            System.Array enumItemArr = absEnumType.GetEnumValues ();

            foreach (object enumItem in enumItemArr)
            {
                if ((FeatureType) enumItem == featureType)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
