using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Aran.Panda.RadarMA.Utils;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using Aran.Panda.RadarMA.ViewModels;
using Aran.PANDA.Common;

namespace Aran.Panda.RadarMA.Models
{
    public class DbModule
    {
        private IWorkspace _workspace;
        public List<RadarPoint> GetAirportList()
        {
            IFeatureLayer layer = EsriFunctions.GetLayerByName("AirportHeliport") as IFeatureLayer;
            //IFeatureClass featureClass= _featureWorkspace.OpenFeatureClass("Aerodrom");
            if (layer == null)
            {
                MessageBox.Show("Cannot find Aerodrome layer");
                return new List<RadarPoint>();
            }

            IDataset dataset = layer as IDataset;
            _workspace = dataset.Workspace;
            IFeatureClass featureClass = layer.FeatureClass;
            IFeatureCursor featCursor = featureClass.Search(null, false);
            var adhpFeature = featCursor.NextFeature();

            var result = new List<RadarPoint>();
            while (adhpFeature!=null)
            {
                RadarPoint tmpRadarPoint = new RadarPoint();
                tmpRadarPoint.Type = RadarPointChoiceType.AirportHeliport;
                tmpRadarPoint.Id = adhpFeature.OID;
                tmpRadarPoint.Geo =(IPoint) adhpFeature.Shape;
                int designatorIndex = adhpFeature.Fields.FindField("Designator");
                if (designatorIndex>0)
                    tmpRadarPoint.Name = (string)adhpFeature.get_Value(designatorIndex);
                result.Add(tmpRadarPoint);
                adhpFeature = featCursor.NextFeature();
            }
            return result;
        }

        public List<RadarPoint> GetRadarSystemList()
        {
            IFeatureLayer layer = EsriFunctions.GetLayerByName("RadarSystem") as IFeatureLayer;
            //IFeatureClass featureClass= _featureWorkspace.OpenFeatureClass("Aerodrom");
            if (layer == null)
            {
                MessageBox.Show("Cannot find RadarSytem layer");
                return new List<RadarPoint>();
            }

            IDataset dataset = layer as IDataset;
            _workspace = dataset.Workspace;
            IFeatureClass featureClass = layer.FeatureClass;
            IFeatureCursor featCursor = featureClass.Search(null, false);
            var radarFeature = featCursor.NextFeature();

            var result = new List<RadarPoint>();
            while (radarFeature != null)
            {
                RadarPoint tmpRadarPoint = new RadarPoint();
                tmpRadarPoint.Type = RadarPointChoiceType.AirportHeliport;
                tmpRadarPoint.Id = radarFeature.OID;
                tmpRadarPoint.Geo = (IPoint)radarFeature.Shape;
                int designatorIndex = radarFeature.Fields.FindField("Name");
                if (designatorIndex > 0)
                    tmpRadarPoint.Name = (string)radarFeature.get_Value(designatorIndex);

                int rangeIndex = radarFeature.Fields.FindField("Range");
                if (rangeIndex > 0)
                    tmpRadarPoint.Range = (double)radarFeature.get_Value(rangeIndex);

                int rangeUom = radarFeature.Fields.FindField("Range_UOM");
                if (rangeUom > 0)
                    tmpRadarPoint.RangeUOM = (string) radarFeature.get_Value(rangeUom);

                int magVar = radarFeature.Fields.FindField("magneticVariation");
                if (rangeUom > 0)
                    tmpRadarPoint.MagVar = (double)radarFeature.get_Value(magVar);


                result.Add(tmpRadarPoint);
                radarFeature = featCursor.NextFeature();
            }
            return result;
        }

        public List<RadarPoint> GetNavaidList()
        {
            IFeatureLayer layer = EsriFunctions.GetLayerByName("Navaid") as IFeatureLayer;
            //IFeatureClass featureClass= _featureWorkspace.OpenFeatureClass("Aerodrom");
            if (layer == null)
            {
                MessageBox.Show("Cannot find Navaid layer");
                return new List<RadarPoint>();
            }

            IDataset dataset = layer as IDataset;
            _workspace = dataset.Workspace;
            IFeatureClass featureClass = layer.FeatureClass;
            IFeatureCursor featCursor = featureClass.Search(null, false);
            var navaidFeature = featCursor.NextFeature();

            var result = new List<RadarPoint>();
            while (navaidFeature != null)
            {
                RadarPoint tmpRadarPoint = new RadarPoint();
                tmpRadarPoint.Type = RadarPointChoiceType.Navaid;
                tmpRadarPoint.Id = navaidFeature.OID;
                tmpRadarPoint.Geo = (IPoint)navaidFeature.Shape;
                int designatorIndex = navaidFeature.Fields.FindField("Designator");
                if (designatorIndex > 0)
                    tmpRadarPoint.Name = (string)navaidFeature.get_Value(designatorIndex);
                result.Add(tmpRadarPoint);
                navaidFeature = featCursor.NextFeature();
            }
            return result;
        }

        public List<RadarAirspace> GetRadarAirspaces()
        {
            var layer = EsriFunctions.GetLayerByName("AirspaceVolume") as IFeatureLayer;
            //IFeatureClass featureClass= _featureWorkspace.OpenFeatureClass("Aerodrom");
            if (layer == null)
            {
                MessageBox.Show("Cannot find AirspaceVolume layer");
                return new List<RadarAirspace>();
            }

            IDataset dataset = layer as IDataset;
            _workspace = dataset.Workspace;
            IFeatureClass featureClass = layer.FeatureClass;
            IFeatureCursor featCursor = featureClass.Search(null, false);
            var airsapceFeature = featCursor.NextFeature();

            var result = new List<RadarAirspace>();
            while (airsapceFeature != null)
            {
                var tmpRadarPoint = new RadarAirspace {Id = airsapceFeature.OID, Geo = (IPolygon) airsapceFeature.Shape};
                int designatorIndex = airsapceFeature.Fields.FindField("txtName");
                if (designatorIndex > 0)
                    tmpRadarPoint.Name = (string)airsapceFeature.get_Value(designatorIndex);

                int codeType = airsapceFeature.Fields.FindField("R_codeType");
                if (codeType > 0)
                    tmpRadarPoint.Type = (string) airsapceFeature.get_Value(codeType);

                result.Add(tmpRadarPoint);
                airsapceFeature = featCursor.NextFeature();
            }
            return result;
        }

        public List<VerticalStructure> GetVerticalstructureList()
        {
            IFeatureLayer layer = EsriFunctions.GetLayerByName("Verticalstructure_point") as IFeatureLayer;
            //IFeatureClass featureClass= _featureWorkspace.OpenFeatureClass("Aerodrom");
            if (layer == null)
            {
                MessageBox.Show("Cannot find Verticalstructuer_point layer");
                return new List<VerticalStructure>();
            }

            IDataset dataset = layer as IDataset;
            _workspace = dataset.Workspace;
            IFeatureClass featureClass = layer.FeatureClass;
            IFeatureCursor featCursor = featureClass.Search(null, false);
            var vsFeature = featCursor.NextFeature();

            var result = new List<VerticalStructure>();
            while (vsFeature != null)
            {
                var vs = new VerticalStructure() {Geo = (IPoint) vsFeature.Shape};
                vs.GeoPrj = GlobalParams.SpatialRefOperation.ToEsriPrj(vsFeature.Shape);
                int designatorIndex = vsFeature.Fields.FindField("Designator");
                if (designatorIndex > 0)
                    vs.Name = (string)vsFeature.Value[designatorIndex];

                int elevationIndex = vsFeature.Fields.FindField("elevation_val");
                if (elevationIndex > 0)
                    vs.Elevation = (double)vsFeature.Value[elevationIndex];

                result.Add(vs);
                vsFeature = featCursor.NextFeature();
            }
            return result;
        }

        public bool SaveRadarAreaToDb(string projectName,ObservableCollection<Sector> sectorList)
        {
            IFeatureLayer layer = EsriFunctions.GetLayerByName("RadarArea") as IFeatureLayer;
            //IFeatureClass featureClass= _featureWorkspace.OpenFeatureClass("Aerodrom");
            if (layer == null)
            {
                MessageBox.Show("Cannot find RadarArea layer");
                return false;
            }
            var featureClass = layer.FeatureClass;

            IDataset dataset = layer as IDataset;
            IWorkspace _workspace = dataset.Workspace;
            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)_workspace;
            workspaceEdit.StartEditing(false);
            workspaceEdit.StartEditOperation();

            ITable table = featureClass as ITable;
            table.DeleteSearchedRows(null);

            IFields fields = featureClass.Fields;

            foreach (var sector in sectorList)
            {
                var esriGeo = new ESRI.ArcGIS.Geometry.Polygon() as ESRI.ArcGIS.Geometry.IPolygon;

                var geomPrj = sector.Geo;
                if (geomPrj == null || geomPrj.IsEmpty)
                    continue;

                IFeature feat = featureClass.CreateFeature();
                var surfaceGeo = GlobalParams.SpatialRefOperation.ToEsriGeo(geomPrj);

                string longtitide = "", latitude = "";
                if (sector.StateMaxElevPoint != null)
                {
                    var maxElevPoint = (IPoint)GlobalParams.SpatialRefOperation.ToEsriGeo(sector.StateMaxElevPoint);
                    //GeomOperators.SimplifyGeometry(maxElevPoint);
                    longtitide = EsriFunctions.DD2DMSText(maxElevPoint.X);
                    latitude = EsriFunctions.DD2DMSText(maxElevPoint.Y);
                }

                var unitConverter = GlobalParams.UnitConverter;

                feat.Shape = surfaceGeo;
                //feat.set_Value(1, esriGeo);
                feat.set_Value(2, projectName);
                feat.set_Value(3, sector.Number);
                feat.set_Value(4,unitConverter.HeightToDisplayUnits(sector.Height,eRoundMode.CEIL));
                feat.set_Value(5, unitConverter.HeightUnit);
                feat.set_Value(6, latitude);
                feat.set_Value(7, longtitide);
                feat.set_Value(8, unitConverter.HeightToDisplayUnits(sector.MOC));
              //  feat.set_Value(9, "FT");
                feat.set_Value(9, unitConverter.DistanceToDisplayUnits(sector.BufferValue));
                feat.set_Value(10, unitConverter.DistanceUnit);

                var maxObstacle = sector.Reports.OrderByDescending(obs => obs.Elevation).FirstOrDefault();

                if (maxObstacle != null)
                {
                    feat.set_Value(13,maxObstacle.Name);
                    feat.set_Value(14,maxObstacle.Elevation);
                }

                feat.Store();

            }
            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

            return true;
        }
    }
}
