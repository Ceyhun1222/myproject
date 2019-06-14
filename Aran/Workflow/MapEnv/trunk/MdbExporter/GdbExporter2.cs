using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using MapEnv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AF = Aran.Aim.Features;
using Aran.Aim.Metadata.UI;
using System.Collections;
using ESRI.ArcGIS.Geometry;
using System.Runtime.InteropServices;
using Aran.Aim;

namespace Aran.Exporter.Gdb
{
    public class GdbExporter2
    {
        public GdbExporter2()
        {
            Layers = new List<ExportLayerInfo>();
            LayerSpatialRef = MapEnv.Globals.CreateWGS84SR();
            WorkspaceType = ExportWorkspaceType.PersonalGdb;
        }


        public List<ExportLayerInfo> Layers { get; private set; }

        public string FileName { get; private set; }

        public ISpatialReference LayerSpatialRef { get; set; }

        public ExportWorkspaceType WorkspaceType { get; set; }

        public List<Exception> Export(string fileName, ref int savedTableCount, ref int savedFCCount)
        {
            FileName = fileName;

            var mapDoc = new MapDocument() as IMapDocument;
            mapDoc.New(fileName);
            var map = mapDoc.get_Map(0) as IMap;
            map.SpatialReference = MapEnv.Globals.MainForm.Map.SpatialReference;

            var errors = CreateWS(map, ref savedTableCount, ref savedFCCount);

            mapDoc.Save();

            return errors;
        }


        private List<Exception> CreateWS(IMap map, ref int savedTableCount, ref int savedFCCount)
        {
            var errorList = new List<Exception>();

            var dir = System.IO.Path.GetDirectoryName(FileName);
            var name = System.IO.Path.GetFileNameWithoutExtension(FileName);

            IWorkspaceFactory wsf;

            if (WorkspaceType == ExportWorkspaceType.PersonalGdb)
                wsf = new AccessWorkspaceFactory() as IWorkspaceFactory;
            else
                wsf = new FileGDBWorkspaceFactory() as IWorkspaceFactory;

            var wn = wsf.Create(dir, name, null, 0);
            var featWS = (wn as IName).Open() as IFeatureWorkspace;
            var wsEdit = featWS as IWorkspaceEdit;

            wsEdit.StartEditing(false);
            wsEdit.StartEditOperation();

            foreach (var expLayInfo in Layers) {
                var aimFL = expLayInfo.Layer;

                if (!expLayInfo.IsChecked)
                    continue;

                if (!aimFL.IsComplex) {

                    try {

                        if (aimFL.ShapeInfoList.Count == 0) {           //Non geographical table.
                            var table = CreateTable(expLayInfo.Layer.Name, featWS, expLayInfo, aimFL.AimFeatures);

                            var standaloneTable = new StandaloneTable() as IStandaloneTable;
                            standaloneTable.Table = table;
                            standaloneTable.Name = expLayInfo.Layer.Name;

                            (map as IStandaloneTableCollection).AddStandaloneTable(standaloneTable);
                            savedTableCount++;
                        }
                        else {          //Geographic Table.
                            var n = 0;
                            foreach (var layInfo in aimFL.LayerInfoList) {

                                var layerName = expLayInfo.Layer.Name;

                                if (aimFL.LayerInfoList.Count > 1)
                                    layerName += "_" + (n++);

                                var fc = CreateFeatureClass(layerName, featWS, layInfo.Layer.FeatureClass, expLayInfo, aimFL.AimFeatures);

                                var fl = new FeatureLayer() as IFeatureLayer;
                                fl.FeatureClass = fc;
                                fl.Name = fc.AliasName;
                                map.AddLayer(fl);

                                var geoFL = fl as IGeoFeatureLayer;
                                geoFL.Renderer = (layInfo.Layer as IGeoFeatureLayer).Renderer;

                                savedFCCount++;
                            }
                        }
                    }
                    catch (Exception ex) {
                        errorList.Add(ex);
                    }
                }
            }

            wsEdit.StopEditOperation();
            wsEdit.StopEditing(true);

            return errorList;
        }

        private IFeatureClass CreateFeatureClass(string layerName, IFeatureWorkspace featWS, IFeatureClass featureClass, ExportLayerInfo expLayInfo, IEnumerable<AF.Feature> aimFeatures)
        {
            var clsId = new UIDClass();
            clsId.Value = "esriGeoDatabase.Feature";

            var geomDict = new Dictionary<Guid, List<IGeometry>>();
            var idenIndex = featureClass.FindField("identifier");
            var featCursor = featureClass.Search(null, false);
            IFeature feat;

            while ((feat = featCursor.NextFeature()) != null) {
                var identifier = new Guid(feat.get_Value(idenIndex).ToString());

                List<IGeometry> geomItemList;
                if (!geomDict.TryGetValue(identifier, out geomItemList)) {
                    geomItemList = new List<IGeometry>();
                    geomDict.Add(identifier, geomItemList);
                }
                geomItemList.Add(feat.ShapeCopy);
            }

            var fields = new Fields() as IFields;
            var fieldsEdit = fields as IFieldsEdit;

            #region OBJECTID

            var field = new FieldClass() as IField;
            var fieldEdit = field as IFieldEdit;
            fieldEdit.Name_2 = "OBJECTID";
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            fieldEdit.Editable_2 = false;
            fieldEdit.IsNullable_2 = false;
            fieldEdit.AliasName_2 = "Object ID";
            fieldsEdit.AddField(field);

            #endregion

            #region Shape

            field = new FieldClass() as IField;
            fieldEdit = field as IFieldEdit;
            fieldEdit.Name_2 = "SHAPE";
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;

            var geomDef = new GeometryDef() as IGeometryDefEdit;
            geomDef.GeometryType_2 = featureClass.ShapeType;
            geomDef.SpatialReference_2 = LayerSpatialRef;
            geomDef.HasZ_2 = true;

            fieldEdit.GeometryDef_2 = geomDef;
            fieldsEdit.AddField(field);

            #endregion

            var piContList = CreateAimFeatureFields("", new List<Aim.AimPropInfo>(), expLayInfo.Properties);
            
            foreach (var item in piContList) {
                item.FieldIndex = fieldsEdit.FieldCount;
                fieldsEdit.AddField(item.EsriField);

                if (item.SecondEsriField != null)
                    fieldsEdit.AddField(item.SecondEsriField);
            }

            var fc = featWS.CreateFeatureClass(layerName, fields, clsId, null, esriFeatureType.esriFTSimple, "SHAPE", null);

            var buffFeatCursor = fc.Insert(true);

            foreach (var aimFeat in aimFeatures) {

                List<IGeometry> geomItemList;
                if (!geomDict.TryGetValue(aimFeat.Identifier, out geomItemList))
                    continue;

                foreach (var geomItem in geomItemList) {

                    var featBuff = fc.CreateFeatureBuffer();

                    if (geomItem.GeometryType != esriGeometryType.esriGeometryPoint) {
                        var pointCol = geomItem as IPointCollection;
                        for (int i = 0; i < pointCol.PointCount; ++i) {
                            var pt = pointCol.get_Point(i);
                            pt.Z = 0D;
                            pointCol.UpdatePoint(i, pt);
                        }
                    }

                    try {
                        featBuff.Shape = geomItem;
                    }
                    catch {
                        continue;
                    }

                    foreach (var piCont in piContList) {
                        var aimPropInfo = piCont.PropInfo;
                        var aimObj = aimFeat as Aim.IAimObject;
                        var isAimObjChoice = false;

                        #region Get aimObj from Property
                        // TODO Optimize. Don't get aimObj from property everytime if not chagned.
                        try {
                            foreach (var objPI in piCont.ObjectPropInfoList)
                            {
	                            var aimObjPropVal = isAimObjChoice ? Aim.Utilities.AimMetadataUtility.GetChoicePropValue ( aimObj, objPI ) : aimObj.GetValue ( objPI.Index );
								//var aimObjPropVal = aimObj.GetValue(objPI.Index);

                                if (aimObjPropVal != null) {
                                    if (aimObjPropVal.PropertyType == Aim.AimPropertyType.List) {
                                        var list = aimObj.GetValue(objPI.Index) as IList;
                                        if (list == null || list.Count == 0) {
                                            aimObj = null;
                                            break;
                                        }
                                        aimObj = list[0] as Aim.IAimObject;
                                    }
                                    else {
                                        aimObj = aimObjPropVal as Aim.IAimObject;
                                    }
                                }
                                else {
                                    aimObj = null;
                                    break;
                                }

                                if (aimObj == null)
                                    break;
                                else 
                                    isAimObjChoice = (objPI.PropType.SubClassType == Aim.AimSubClassType.Choice);
                            }
                        }
                        catch (Exception ex) {
                            throw ex;
                        }
                        #endregion

                        if (aimObj == null)
                            continue;

                        Aim.IAimProperty aimPropVal;

                        if (isAimObjChoice) {
                            aimPropVal = Aim.Utilities.AimMetadataUtility.GetChoicePropValue(aimObj, aimPropInfo);
                        }
                        else {
                            aimPropVal = aimObj.GetValue(aimPropInfo.Index);
                        }

                        if (aimPropVal != null)
                            SetField(aimPropVal, aimPropInfo, featBuff, piCont.FieldIndex);
                    }

                    buffFeatCursor.InsertFeature(featBuff);
                }
            }

            Marshal.FinalReleaseComObject(buffFeatCursor);

            return fc;
        }

        private ITable CreateTable(string tableName, IFeatureWorkspace featWS, ExportLayerInfo expLayInfo, IEnumerable<AF.Feature> aimFeatures)
        {
            var clsId = new UIDClass();
            clsId.Value = "esriGeoDatabase.Feature";

            var fields = new Fields() as IFields;
            var fieldsEdit = fields as IFieldsEdit;

            #region OBJECTID

            var field = new FieldClass() as IField;
            var fieldEdit = field as IFieldEdit;
            fieldEdit.Name_2 = "OBJECTID";
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            fieldEdit.Editable_2 = false;
            fieldEdit.IsNullable_2 = false;
            fieldEdit.AliasName_2 = "Object ID";
            fieldsEdit.AddField(field);

            #endregion

            var piContList = CreateAimFeatureFields("", new List<Aim.AimPropInfo>(), expLayInfo.Properties);

            foreach (var item in piContList) {
                item.FieldIndex = fieldsEdit.FieldCount;
                fieldsEdit.AddField(item.EsriField);
                
                if (item.SecondEsriField != null)
                    fieldsEdit.AddField(item.SecondEsriField);
            }

            var table = featWS.CreateTable(tableName, fields, clsId, null, null);

            foreach (var aimFeat in aimFeatures) {

                var row = table.CreateRow();

                foreach (var piCont in piContList) {
                    var aimPropInfo = piCont.PropInfo;
                    var aimObj = aimFeat as Aim.IAimObject;

                    #region Get aimObj from Property
                    // TODO Optimize. Don't get aimObj from property everytime if not chagned.

                    foreach (var objPI in piCont.ObjectPropInfoList) {
                        var aimObjPropVal = aimObj.GetValue(objPI.Index);
                        if (aimObjPropVal.PropertyType == Aim.AimPropertyType.List) {
                            var list = aimObj.GetValue(objPI.Index) as IList;
                            if (list == null || list.Count == 0) {
                                aimObj = null;
                                break;
                            }
                            aimObj = list[0] as Aim.IAimObject;
                        }
                        else {
                            aimObj = aimObjPropVal as Aim.IAimObject;
                        }

                        if (aimObj == null)
                            break;
                    }
                    #endregion

                    if (aimObj == null)
                        continue;

                    var aimPropVal = aimObj.GetValue(aimPropInfo.Index);

                    if (aimPropVal != null)
                        SetField(aimPropVal, aimPropInfo, row, piCont.FieldIndex);
                }

                row.Store();
            }

            return table;
        }

        private void SetField(Aim.IAimProperty aimPropVal, Aim.AimPropInfo aimPropInfo, IRowBuffer row, int fieldIndex)
        {
            if (aimPropVal.PropertyType == Aim.AimPropertyType.AranField) {
                var val = (aimPropVal as Aim.IEditAimField).FieldValue;

                if (val is Guid)
                    val = "{" + val + "}";

                if (aimPropInfo.PropType.SubClassType == Aim.AimSubClassType.Enum)
                    val = Aim.AimMetadata.GetEnumValueAsString((int)val, aimPropInfo.TypeIndex);

                row.set_Value(fieldIndex, val);
            }
            else if (aimPropInfo.PropType.SubClassType == Aim.AimSubClassType.ValClass) {
                var editVC = aimPropVal as Aim.DataTypes.IEditValClass;
                var uomPropInfo = aimPropInfo.PropType.Properties["Uom"];
                var uomStr = Aim.AimMetadata.GetEnumValueAsString(editVC.Uom, uomPropInfo.TypeIndex);

                row.set_Value(fieldIndex, editVC.Value);
                row.set_Value(fieldIndex + 1, uomStr);
            }
            else if (aimPropInfo.TypeIndex == (int)Aran.Aim.DataType.FeatureRef ||
                    aimPropInfo.PropType.SubClassType == Aran.Aim.AimSubClassType.AbstractFeatureRef) {
                var featRef = aimPropVal as Aran.Aim.DataTypes.FeatureRef;
                row.set_Value(fieldIndex, "{" + featRef.Identifier + "}");
            }
        }

        private List<PropInfoContainer> CreateAimFeatureFields(string fieldPrefix, List<Aim.AimPropInfo> objPropInfos, List<ExportPropInfo> expPropInfoList)
        {
            var resList = new List<PropInfoContainer>();

            foreach (var expPropInfo in expPropInfoList) {

                var propInfo = expPropInfo.AimPropInfo;

                if (!expPropInfo.IsChecked || propInfo.AixmName.Length == 0)
                    continue;

                #region Field

                if (propInfo.PropType.AimObjectType == Aran.Aim.AimObjectType.Field) {

                    var field = CreateField(fieldPrefix + propInfo.AixmName);
                    var fieldEdit = field as IFieldEdit;

                    var aimFieldType = (Aran.Aim.AimFieldType)propInfo.TypeIndex;

                    if (propInfo.PropType.SubClassType == Aran.Aim.AimSubClassType.Enum) {
                        fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                        fieldEdit.Length_2 = 60;
                        fieldEdit.AliasName_2 = fieldEdit.AliasName + " (ENUM)";
                    }
                    else {
                        switch (aimFieldType) {
                            case Aran.Aim.AimFieldType.SysBool:
                                fieldEdit.Type_2 = esriFieldType.esriFieldTypeSmallInteger;
                                break;

                            case Aran.Aim.AimFieldType.SysDateTime:
                                fieldEdit.Type_2 = esriFieldType.esriFieldTypeDate;
                                break;

                            case Aran.Aim.AimFieldType.SysDouble:
                                fieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                                break;

                            case Aran.Aim.AimFieldType.SysGuid:
                                fieldEdit.Type_2 = esriFieldType.esriFieldTypeGUID;
                                break;

                            case Aran.Aim.AimFieldType.SysInt32:
                            case Aran.Aim.AimFieldType.SysInt64:
                                fieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                                break;

                            case Aran.Aim.AimFieldType.SysString:
                                fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                                if (propInfo.Restriction == null || propInfo.Restriction.Max == null)
                                    fieldEdit.Length_2 = 255;
                                else
                                    fieldEdit.Length_2 = (int)propInfo.Restriction.Max.Value;
                                break;
                        }
                    }

                    resList.Add(new PropInfoContainer(objPropInfos, propInfo,field));
                }

                #endregion

                #region ValClass

                else if (propInfo.PropType.SubClassType == Aran.Aim.AimSubClassType.ValClass) {

                    var field = CreateField(fieldPrefix + propInfo.AixmName + "_val");
                    var fieldEdit = field as IFieldEdit;
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                    var piCont = new PropInfoContainer(objPropInfos, propInfo, field);

                    field = CreateField(fieldPrefix + propInfo.AixmName + "_uom");
                    fieldEdit = field as IFieldEdit;
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    piCont.SecondEsriField = field;
                    
                    resList.Add(piCont);
                }

                #endregion

                #region FeatureRef

                else if (propInfo.TypeIndex == (int)Aran.Aim.DataType.FeatureRef ||
                    propInfo.PropType.SubClassType == Aran.Aim.AimSubClassType.AbstractFeatureRef) {

                        var field = CreateField(fieldPrefix + propInfo.AixmName);
                    var fieldEdit = field as IFieldEdit;
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeGUID;

                    resList.Add(new PropInfoContainer(objPropInfos, propInfo, field));
                }

                #endregion

                #region Object

                else if (propInfo.PropType.AimObjectType == Aim.AimObjectType.Object ) {
                    var subObjPropInfos = new List<Aim.AimPropInfo>();
                    subObjPropInfos.AddRange(objPropInfos);
                    subObjPropInfos.Add(propInfo);
                    var subPICont = CreateAimFeatureFields(fieldPrefix + propInfo.AixmName + "_", subObjPropInfos, expPropInfo.ChildList);
                    resList.AddRange(subPICont);
                }

                #endregion
            }

            return resList;
        }

        #region Create Field

        private IField CreateField(string name)
        {
            var field = new FieldClass() as IField;
            var fieldEdit = field as IFieldEdit;
            fieldEdit.Name_2 = name;
            fieldEdit.AliasName_2 = name;
            fieldEdit.IsNullable_2 = true;
            fieldEdit.Editable_2 = true;
            fieldEdit.Length_2 = 100;
            return field;
        }

        private IField CreateStringField(string name, int len)
        {
            var field = CreateField(name);
            var fieldEdit = field as IFieldEdit;
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit.Length_2 = len;
            return field;
        }

        #endregion
    }

    public class PropInfoContainer
    {
        public PropInfoContainer()
        {
            ObjectPropInfoList = new List<Aim.AimPropInfo>();
        }

        public PropInfoContainer(List<Aim.AimPropInfo> objPropInfos, Aim.AimPropInfo propInfo, IField esriField)
            : this()
        {
            ObjectPropInfoList.AddRange(objPropInfos);
            PropInfo = propInfo;
            EsriField = esriField;
        }

        public List<Aim.AimPropInfo> ObjectPropInfoList { get; private set; }

        public Aim.AimPropInfo PropInfo { get; set; }

        public IField EsriField { get; set; }

        public IField SecondEsriField { get; set; }

        public int FieldIndex { get; set; }
    }

    public enum ExportWorkspaceType { FileGdb, PersonalGdb }

}
