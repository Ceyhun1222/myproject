using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ARENA;
using System.Windows;

namespace SigmaChart
{
    public class DbModule
    {
        private IFeatureWorkspace _featureWorkspace;
        private List<Obstacle> _obstacleList;
        private List<PDM.PDMObject> _pdmObjectList;
        private List<PDM.VerticalStructure> _vsList;

        public DbModule()
        {
            if (DataCash.ProjectEnvironment == null)
            {
                MessageBox.Show("Error loading Database!");
                throw new Exception("Error loading Db");
            }

            _pdmObjectList = DataCash.ProjectEnvironment.Data.PdmObjectList;// 

            IWorkspace workspace = null;
            var featLayer = GetFeatureLayer();
            if (featLayer == null) return;

            var dataset = featLayer as IDataset;
            workspace = dataset.Workspace;

            _featureWorkspace = (IFeatureWorkspace)workspace;
        }

        public List<Obstacle> ObstacleList
        {
            get
            {
                if (_obstacleList == null)
                {
                    LoadObstacles();
                }
                return _obstacleList;
            }
        }

        public List<PDM.VerticalStructure> VerticalStructureList
        {
            get
            {
                if (_vsList == null)
                {
                    if (_pdmObjectList != null)
                    {
                        _vsList = _pdmObjectList.Where(pdmObject => pdmObject is PDM.VerticalStructure).Cast<PDM.VerticalStructure>().ToList();

                    }
                }
                return _vsList;
            }
        }

        private void LoadObstacles()
        {
            var obstacleList = GetVsTable();
            var partList = GetPartList();
            var geomList = GetGeomList();

            foreach (var obstacle in obstacleList)
            {
                foreach (var obstaclePart in partList)
                {
                    if (obstaclePart.ObstacleID == obstacle.Identifier)
                    {
                        obstacle.ObstacleParts.Add(obstaclePart);
                        foreach (var obstacleGeometry in geomList)
                        {
                            if (obstacleGeometry.PartID == obstaclePart.Identifier)
                            {
                                obstaclePart.Geometry = obstacleGeometry;
                                break;
                            }
                        }
                    }
                }
            }
            _obstacleList = obstacleList;
        }


        private List<Obstacle> GetVsTable()
        {
            var result = new List<Obstacle>();

            if (_featureWorkspace == null) return result;

            ITable featureClass = _featureWorkspace.OpenTable("VerticalStructure");

            ICursor featCursor = featureClass.Search(null, true);
            var vsFeature = featCursor.NextRow();

            using (ComReleaser comReleaser = new ComReleaser())
            {
                while (vsFeature != null)
                {
                    comReleaser.ManageLifetime(featCursor);

                    var vs = new Obstacle();
                    vs.ID = vsFeature.OID;
                    int guidIndex = vsFeature.Fields.FindField("FeatureGuid");
                    if (guidIndex > 0)
                        vs.Identifier = (string)vsFeature.get_Value(guidIndex);

                    int nameIndex = vsFeature.Fields.FindField("Name");
                    if (nameIndex > 0)
                        vs.Name = (string)vsFeature.get_Value(nameIndex);

                    //Read vspart


                    result.Add(vs);
                    vsFeature = featCursor.NextRow();
                }
            }


            return result;
        }

        private List<ObstaclePart> GetPartList()
        {

            var partList = new List<ObstaclePart>();

            if (_featureWorkspace == null) return partList;

            var vsPartfeatureClass = _featureWorkspace.OpenTable("VerticalStructurePart");
            var vsPartFeatCursor = vsPartfeatureClass.Search(null, false);
            var vsPartFeature = vsPartFeatCursor.NextRow();
            while (vsPartFeature != null)
            {
                var obsPart = new ObstaclePart();

                var identifierId = vsPartFeature.Fields.FindField("FeatureGUID");
                if (identifierId > 0)
                    obsPart.Identifier = (string)vsPartFeature.Value[identifierId];

                var vsId = vsPartFeature.Fields.FindField("VerticalStructure_ID");
                if (vsId > 0 && vsPartFeature.Value[vsId] != null)
                    obsPart.ObstacleID = (string)vsPartFeature.Value[vsId];

                var elevId = vsPartFeature.Fields.FindField("Elev");
                if (elevId > 0 && vsPartFeature.Value[elevId] != null)
                {
                    double value;

                    var str = vsPartFeature.Value[elevId];
                    if (Double.TryParse(str.ToString(), out value))
                        obsPart.Elevation = value;
                }

                var uomId = vsPartFeature.Fields.FindField("Height_UOM");
                if (uomId > 0 && vsPartFeature.Value[uomId] != null)
                    obsPart.Uom = (string)vsPartFeature.Value[uomId];


                vsPartFeature = vsPartFeatCursor.NextRow();
                partList.Add(obsPart);
            }
            return partList;
        }

        private List<Obstacle_Geometry> GetGeomList()
        {
            var obsGeomList = new List<Obstacle_Geometry>();

            if (_featureWorkspace == null) return obsGeomList;

            IFeatureClass vsPointfeatureClass = _featureWorkspace.OpenFeatureClass("VerticalStructure_Point");
            IFeatureCursor vsPointFeatCursor = vsPointfeatureClass.Search(null, false);

            var vsPointFeature = vsPointFeatCursor.NextFeature();

            while (vsPointFeature != null)
            {
                var obsGeom = new Obstacle_Geometry();
                obsGeom.Shape = vsPointFeature.Shape;

                var partId = vsPointFeature.Fields.FindField("partID");
                if (partId > 0)
                    obsGeom.PartID = (string)vsPointFeature.Value[partId];

                obsGeomList.Add(obsGeom);
                vsPointFeature = vsPointFeatCursor.NextFeature();
            }
            return obsGeomList;
        }

        private IFeatureLayer GetFeatureLayer()
        {
            int layerCount = GlobalParams.HookHelper.ActiveView.FocusMap.LayerCount;

            for (int i = 0; i < layerCount; i++)
            {
                var layer = GlobalParams.ActiveView.FocusMap.Layer[i];
                if (layer is IFeatureLayer)
                    return layer as IFeatureLayer;
            }
            return null;
        }
    }
}
