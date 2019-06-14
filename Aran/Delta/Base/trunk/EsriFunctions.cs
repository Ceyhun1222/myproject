using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Delta
{
    class EsriFunctions
    {

        public static IWorkspaceEdit GetWorkspace(IMap activeMap)
        {

            ILayer _Layer = GetLayerByName(activeMap, "AirportHeliport");
            var fc = ((IFeatureLayer)_Layer).FeatureClass;
            var workspaceEdit = (IWorkspaceEdit)fc.FeatureDataset.Workspace;
            return workspaceEdit;

        }

        public static ILayer GetLayerByName(IMap FocusMap, string layerName)
        {
            ILayer res = null;
            bool ok = false;
            string _name;
            try
            {
                for (int i = 0; i <= FocusMap.LayerCount - 1; i++)
                {
                    ILayer Layer1 = FocusMap.get_Layer(i);
                    _name = Layer1.Name;
                    if (Layer1 is FeatureLayer && ((IFeatureLayer)Layer1).FeatureClass != null) _name = ((IFeatureLayer)Layer1).FeatureClass.AliasName;
                    if (_name.ToUpper().CompareTo(layerName.ToUpper()) == 0)
                    {
                        res = Layer1;
                        ok = true;
                    }
                    else if (Layer1 is ICompositeLayer)
                    {
                        ICompositeLayer Complayer = (ICompositeLayer)Layer1;
                        for (int j = 0; j <= Complayer.Count - 1; j++)
                        {
                            ILayer Layer2 = Complayer.get_Layer(j);
                            _name = Layer2.Name;
                            if (Layer2 is FeatureLayer && ((IFeatureLayer)Layer2).FeatureClass != null) _name = ((IFeatureLayer)Layer2).FeatureClass.AliasName;
                            if (Layer2 is FDOGraphicsLayer) { FDOGraphicsLayer annoL = (FDOGraphicsLayer)Layer2; _name = ((IFeatureLayer)annoL).Name; }
                            if (_name.ToUpper().CompareTo(layerName.ToUpper()) == 0)
                            {
                                res = Layer2;
                                ok = true;
                                break;
                            }
                        }
                    }

                    if (ok) break;
                }
            }
            catch
            {
                res = null;
            }

            return res;

        }

        public static void AddDeltaLayersToMap(IMap map)
        {
           IWorkspaceEdit workSpaceEdit = GetWorkspace(map);
           if (workSpaceEdit == null) return;

           IWorkspace workSpace =workSpaceEdit as IWorkspace;
           IEnumDataset datasets = workSpace.get_Datasets(esriDatasetType.esriDTFeatureClass);
           IFeatureWorkspace fw = (IFeatureWorkspace)workSpace;

           var designingArea =fw.OpenFeatureClass("Designing_Area");

            var deltaFeatureDataSet = fw.OpenFeatureDataset("Delta");

            if (deltaFeatureDataSet == null)
            {
                Model.Messages.Warning("Delta Designing Layer is not found!");
                return;
            }
            var deltaDataSet = deltaFeatureDataSet.Subsets;

            deltaDataSet.Reset();
            IDataset dataset = null;
            while ((dataset = deltaDataSet.Next()) != null)
            {
                if (dataset.Type == esriDatasetType.esriDTFeatureClass)
                {
                    if (GetLayerByName(map, dataset.Name) != null)
                        continue;

                    IFeatureClass featureClass = fw.OpenFeatureClass(dataset.Name);
                    IFeatureLayer featureLayer = new FeatureLayerClass();
                    featureLayer.FeatureClass = featureClass;
                    ILayer layer = (ILayer)featureLayer;
                    layer.Name = dataset.Name;
                    map.AddLayer(layer);
                }
            }

            //datasets.Reset();
            //IDataset dataset = null;
            //while ((dataset = datasets.Next()) != null)
            //{
            //    if (dataset.Type == esriDatasetType.esriDTFeatureClass)
            //    {
            //        IFeatureClass featureClass = fw.OpenFeatureClass(dataset.Name);
            //        IFeatureLayer featureLayer = new FeatureLayerClass();
            //        featureLayer.FeatureClass = featureClass;
            //        ILayer layer = (ILayer)featureLayer;
            //        layer.Name = dataset.Name;
            //        map.AddLayer(layer);
            //    }
            //    dataset = datasets.Next();
            //}

        }

    }
}
