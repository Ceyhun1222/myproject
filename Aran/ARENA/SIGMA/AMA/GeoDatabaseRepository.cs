using ANCOR.MapElements;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SigmaChart
{
    class GeoDatabaseRepository
    {
        private const double MeterToFtValue = 3.14;
        private LayerHelper _layerHelper;

        public GeoDatabaseRepository(LayerHelper layerHelper)
        {
            _layerHelper = layerHelper;
        }

        public void SaveGrids(IList<GridMuraClass> gridMuraList,double mocDisplayValue)
        {

            IFeatureLayer layer = _layerHelper.GetLayerByName("AMEA") as IFeatureLayer;
            if (layer == null)
                throw new ArgumentException("Cannot find AMEA layer");

            var featureClass = layer.FeatureClass;

            IMap map = GlobalParams.ActiveView.FocusMap;

            IDataset dataset = layer as IDataset;
            IWorkspace _workspace = dataset.Workspace;
            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)_workspace;

            workspaceEdit.StartEditing(false);
            workspaceEdit.StartEditOperation();

            ITable table = featureClass as ITable;
            table.DeleteSearchedRows(null);

            if (SigmaDataCash.AnnotationFeatureClassList == null)
                ChartElementsManipulator.LoadChartElements(workspaceEdit);

            SigmaDataCash.ChartElementList.RemoveAll(x => ((AbstractChartElement)x).Name.CompareTo("AMA_Text") == 0);

            IFeatureClass AMA_ANNO = (IFeatureClass)SigmaDataCash.AnnotationFeatureClassList["AMAEAnno"];
            table = AMA_ANNO as ITable;
            table.DeleteSearchedRows(null);

            IFields fields = featureClass.Fields;
            int i = 0;

            var moc = GlobalParams.UnitConverter.HeightToInternalUnits(mocDisplayValue);
            foreach (var gridMura in gridMuraList)
            {
                if (!gridMura.Elevation.HasValue)
                    continue;

                var centerPoint = gridMura.CenterPoint;
                i++;
                IFeature feat = featureClass.CreateFeature();
                Guid FeatureGUID = Guid.NewGuid();


                double elevDisplay = GlobalParams.UnitConverter.HeightToDisplayUnits(gridMura.Elevation.Value, eRoundMode.CEIL);
                double elevWithMoc = elevDisplay + mocDisplayValue;

                var elevThous = (int)(elevWithMoc / 1000);
                var elevHun = (int)(elevWithMoc - elevThous * 1000) / 100;

                var elevationInTable = Math.Round(gridMura.Elevation.Value * MeterToFtValue);

                feat.Shape = centerPoint;

                feat.set_Value(2, elevThous);
                feat.set_Value(3, elevHun);
                feat.set_Value(4, elevationInTable);
                feat.set_Value(5, mocDisplayValue);
                feat.set_Value(6, GlobalParams.UnitConverter.HeightUnit);
                feat.set_Value(7, FeatureGUID.ToString());
                feat.set_Value(8, gridMura.ObstacleGuid ?? "");
                feat.set_Value(9, gridMura.ObstacleName ?? "");
                feat.Store();


                ChartElement_SimpleText chrtEl_AMA = (ChartElement_SimpleText)SigmaDataCash.prototype_anno_lst.FirstOrDefault(x => x.Name.StartsWith("AMA_Text")).Clone();
                chrtEl_AMA.TextContents[0][0].TextValue = elevThous.ToString();
                chrtEl_AMA.TextContents[0][1].TextValue = elevHun.ToString();
                IElement el_AMA = (IElement)chrtEl_AMA.ConvertToIElement();
                el_AMA.Geometry = centerPoint;

                ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_AMA.Name, FeatureGUID.ToString(), el_AMA, ref chrtEl_AMA, chrtEl_AMA.Id, (int)map.MapScale);

            }
            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

            GlobalParams.ActiveView.Refresh();
        }
    }
}
