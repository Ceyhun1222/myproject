using Aran.Aim;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using TOSSM.Util.GeoUtil;

namespace TOSSM.Util
{
    public partial class GeoDbUtil
    {
        #region Util

        private static void UtilAddAimPropertyToClass(AimPropInfo aimInfo, ClassCreationArgs classCreationArg)
        {
            //we suppose property is not list, list are processed in other place
            if (aimInfo.PropType.SubClassType == AimSubClassType.ValClass)
            {
                IField valueField = new FieldClass();
                IFieldEdit valueFieldEdit = (IFieldEdit)valueField;
                valueFieldEdit.Name_2 = GetValTypeNameForValue(aimInfo.Name);
                valueFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                classCreationArg.Fields.Add(valueField);

                IField uomField = new FieldClass();
                IFieldEdit uomFieldEdit = (IFieldEdit)uomField;
                uomFieldEdit.Name_2 = GetValTypeNameForUom(aimInfo.Name);
                uomFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                classCreationArg.Fields.Add(uomFieldEdit);
            }
            else
            {
                var esriType = GetEsriType(aimInfo);
                IField field = new FieldClass();
                IFieldEdit fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = GetEsriName(aimInfo.Name);
                fieldEdit.Type_2 = esriType;
                

                if (esriType == esriFieldType.esriFieldTypeOID)
                {
                    //should create linked table
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                    classCreationArg.Fields.Add(fieldEdit);
                }
                else if (esriType == esriFieldType.esriFieldTypeGeometry)
                {
                    switch ((AimFieldType)aimInfo.PropType.Index)
                    {
                        case AimFieldType.GeoPoint:
                            fieldEdit.GeometryDef_2 = CreateGeometryDef(esriGeometryType.esriGeometryPoint);
                            break;
                        case AimFieldType.GeoPolyline:
                            fieldEdit.GeometryDef_2 = CreateGeometryDef(esriGeometryType.esriGeometryPolyline);
                            break;
                        case AimFieldType.GeoPolygon:
                            fieldEdit.GeometryDef_2 = CreateGeometryDef(esriGeometryType.esriGeometryPolygon);
                            break;
                    }
                    classCreationArg.Fields.Add(fieldEdit);
                }
                else if (esriType == esriFieldType.esriFieldTypeString)
                {
                    fieldEdit.Length_2 = 3000;
                    classCreationArg.Fields.Add(fieldEdit);
                }
                else
                {
                    classCreationArg.Fields.Add(fieldEdit);
                }
            }
        }


        #endregion


        static void ConvertLabelsToAnnotationSingleLayerMapAnno(IMap pMap, int layerIndex)
        {
            IConvertLabelsToAnnotation pConvertLabelsToAnnotation = new
                ConvertLabelsToAnnotationClass();
            ITrackCancel pTrackCancel = new CancelTrackerClass();
            //Change global level options for the conversion by sending in different parameters to the next line.
            pConvertLabelsToAnnotation.Initialize(pMap,
                esriAnnotationStorageType.esriMapAnnotation,
                esriLabelWhichFeatures.esriVisibleFeatures, true, pTrackCancel, null);
            ILayer pLayer = pMap.get_Layer(layerIndex);
            IGeoFeatureLayer pGeoFeatureLayer = pLayer as IGeoFeatureLayer;
            if (pGeoFeatureLayer != null)
            {
                IFeatureClass pFeatureClass = pGeoFeatureLayer.FeatureClass;
                //Add the layer information to the converter object. Specify the parameters of the output annotation feature class here as well.
                pConvertLabelsToAnnotation.AddFeatureLayer(pGeoFeatureLayer,
                    pGeoFeatureLayer.Name + "_Anno", null, null, false, false, false, false,
                    false, "");
                //Do the conversion.
                pConvertLabelsToAnnotation.ConvertLabels();
                //Turn off labeling for the layer converted.
                pGeoFeatureLayer.DisplayAnnotation = false;
                //Refresh the map to update the display.
                IActiveView pActiveView = pMap as IActiveView;
                pActiveView.Refresh();
            }
        }

        static void ConvertLabelsToGDBAnnotationEntireMap(IMap pMap, bool featureLinked)
        {
            IConvertLabelsToAnnotation pConvertLabelsToAnnotation = new
                ConvertLabelsToAnnotationClass();
            ITrackCancel pTrackCancel = new CancelTrackerClass();
            //Change global level options for the conversion by sending in different parameters to the next line.
            pConvertLabelsToAnnotation.Initialize(pMap,
                esriAnnotationStorageType.esriDatabaseAnnotation,
                esriLabelWhichFeatures.esriVisibleFeatures, true, pTrackCancel, null);
            IUID pUID = new UIDClass();
            pUID.Value = "{E156D7E5-22AF-11D3-9F99-00C04F6BC78E}";
            //IGeoFeatureLayer interface ID.
            IMapLayers pMapLayers = pMap as IMapLayers;
            IEnumLayer pEnumLayer = pMapLayers.get_Layers(pUID as UIDClass, true);
            pEnumLayer.Reset();
            IGeoFeatureLayer pGeoFeatureLayer = pEnumLayer.Next() as IGeoFeatureLayer;

            while (pGeoFeatureLayer != null)
            {
                if (pGeoFeatureLayer.Valid == true)
                {
                    if (pMapLayers.IsLayerVisible(pGeoFeatureLayer as ILayer))
                    //Takes scale and groups layers into account.
                    {
                        if (pGeoFeatureLayer.DisplayAnnotation == true)
                        {
                            IFeatureClass pFeatureClass = pGeoFeatureLayer.FeatureClass;
                            IDataset pDataset = pFeatureClass as IDataset;
                            IFeatureWorkspace pFeatureWorkspace = pDataset.Workspace as
                                IFeatureWorkspace;

                            //Add the layer information to the converter object. Specify the parameters of the output annotation feature class here as well.
                            pConvertLabelsToAnnotation.AddFeatureLayer(pGeoFeatureLayer,
                                pGeoFeatureLayer.Name + "_Anno", pFeatureWorkspace,
                                pFeatureClass.FeatureDataset, featureLinked, false, false,
                                true, true, "");
                        }
                    }
                }
                pGeoFeatureLayer = pEnumLayer.Next() as IGeoFeatureLayer;
            }
            //Do the conversion.
            pConvertLabelsToAnnotation.ConvertLabels();
            IEnumLayer pAnnoEnumLayer = pConvertLabelsToAnnotation.AnnoLayers;
            //Turn off labeling for the layers converted.
            pEnumLayer.Reset();
            pGeoFeatureLayer = pEnumLayer.Next() as IGeoFeatureLayer;
            while (pGeoFeatureLayer != null)
            {
                if (pGeoFeatureLayer.Valid == true)
                {
                    if (pMapLayers.IsLayerVisible(pGeoFeatureLayer as ILayer))
                    //Takes scale and groups layers into account.
                    {
                        if (pGeoFeatureLayer.DisplayAnnotation == true)
                            pGeoFeatureLayer.DisplayAnnotation = false;
                    }
                }
                pGeoFeatureLayer = pEnumLayer.Next() as IGeoFeatureLayer;
            }
            //Add the result annotation layer to the map.
            pMap.AddLayers(pAnnoEnumLayer, true);
            //Refresh the map to update the display.
            IActiveView pActiveView = pMap as IActiveView;
            pActiveView.Refresh();
        }

    }
}
