using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Aran.Aim;
using Aran.Temporality.CommonUtil.Control.TreeViewDragDrop;
using Aran.Temporality.CommonUtil.ViewModel;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using TOSSM.Util.GeoUtil;
using TOSSM.ViewModel.Tool;

namespace TOSSM.Util
{
    public class GeoDbCreationContext : IDisposable
    {
        //provided data
        public Action<string> StatusChangedAction;
        public IMap Map;
        public IFeatureWorkspace Workspace;
 
        //calculated data
        public List<ClassCreationArgs> ToBeCreatedObjects = new List<ClassCreationArgs>();
        public List<RelationCreationArgs> ToBeCreatedRelations = new List<RelationCreationArgs>();
        public ISet<FeatureType> ToBeCreatedFeatureTypes = new SortedSet<FeatureType>();

        //created data
        public Dictionary<FeatureType, IDataset> CreatedDatasets = new Dictionary<FeatureType, IDataset>();
        public Dictionary<string, IObjectClass> CreatedClasses = new Dictionary<string, IObjectClass>();
        public Dictionary<string, ILayer> CreatedLayers = new Dictionary<string, ILayer>();

        public void Release()
        {
            ToBeCreatedObjects.Clear();
            ToBeCreatedRelations.Clear();
            ToBeCreatedFeatureTypes.Clear();
            foreach (var item in CreatedDatasets.Values)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(item);
            }
            foreach (var item in CreatedClasses.Values)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(item);
            }
            foreach (var item in CreatedLayers.Values)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(item);
            }
            if (Map != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(Map);
            }
            if (Workspace != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(Workspace);
            }
        }

        private bool _isDisposed;

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;
            Release();
        }
    }

    public partial class GeoDbUtil
    {
        public static string SystemTableName = "Table_Info";

        #region Do create tables

        private static void CreateSystemClass(GeoDbCreationContext context)
        {  
            var fields = new List<IField>();

            IField pathField = new FieldClass();
            IFieldEdit pathFieldEdit = (IFieldEdit)pathField;
            pathFieldEdit.Name_2 = "Path";
            pathFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            fields.Add(pathFieldEdit);

            IField datasetField = new FieldClass();
            IFieldEdit datasetFieldEdit = (IFieldEdit)datasetField;
            datasetFieldEdit.Name_2 = "DatasetName";
            datasetFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            fields.Add(datasetFieldEdit);

            IField nameField = new FieldClass();
            IFieldEdit nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "TableName";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            fields.Add(nameFieldEdit);

            var objectClass = CreateObjectClass((IWorkspace)context.Workspace, SystemTableName, SystemTableName, fields);
            var pathFieldIndex=((ITable) objectClass).FindField("Path");
            var tableFieldIndex = ((ITable)objectClass).FindField("TableName");
            var datasetFieldIndex = ((ITable)objectClass).FindField("DatasetName");
            

            foreach (var item in context.ToBeCreatedObjects)
            {
                IRow row=((ITable) objectClass).CreateRow();
                row.Value[pathFieldIndex] = GetPath(item.PathName);
                row.Value[tableFieldIndex] = item.ClassName;
                row.Value[datasetFieldIndex] = item.FeatureType.ToString();
                row.Store();
            }
        }

        private static string GetPath(string s)
        {
            var i = s.LastIndexOf("\\", StringComparison.Ordinal);
            return i == -1 ? s : s.Substring(i + 1);
        }

        private static void CheckClasNames(GeoDbCreationContext context)
        {
            //deal with long names
            foreach (ClassCreationArgs classCreation in context.ToBeCreatedObjects)
            {
                //preserve name as alias
                classCreation.ClassAlias = classCreation.ClassName;
                while (classCreation.ClassName.Length > 40)
                {
                    var index = classCreation.ClassName.IndexOf("_");

                    if (index == -1)
                    {
                        classCreation.ClassName = "c_" + classCreation.ClassName.Substring(classCreation.ClassName.Length - 38);
                    }
                    else
                    {
                        classCreation.ClassName = classCreation.ClassName.Substring(index + 1);
                    }
                }
            }

            //deal with same names for tables
            foreach (var group in context.ToBeCreatedObjects.GroupBy(t=>t.ClassName))
            {
                if (group.Count() > 1)
                {
                    var count = 0;
                    foreach (var item in group)
                    {
                        count++;
                        item.ClassName = item.ClassName + "_" +count;
                    }
                }
            }
        }

        private static void CreateClasses(GeoDbCreationContext context)
        {
            foreach (var arg in context.ToBeCreatedObjects)
            {
                if (context.StatusChangedAction != null)
                {
                    context.StatusChangedAction("Saving class " + arg.ClassName + "...");
                }
                var dataset = (IFeatureDataset) context.CreatedDatasets[arg.FeatureType];
                var objectClass = CreateClass(dataset, arg.ClassName, arg.ClassAlias, arg.Fields);
                context.CreatedClasses[arg.ClassName] = objectClass;

                if (!(objectClass is IFeatureClass))
                {
                    (context.Map as ITableCollection).AddTable((ITable)objectClass);
                }
                
            }
        }

        private static IRelationshipClass CreateRelationInContainer(GeoDbCreationContext context, RelationCreationArgs arg, IRelationshipClassContainer relClassContainer)
        {
            IObjectClass target = context.CreatedClasses[arg.Target.ClassName];
            IObjectClass source = context.CreatedClasses[arg.Source.ClassName];
            try
            {
                IRelationshipClass relClass = relClassContainer.CreateRelationshipClass(
                    arg.Name,
                    source,
                    target,
                    //arg.Description, arg.ReversedDescription,
                    arg.Target.ClassName, arg.Source.ClassName,
                    arg.Cardinality,
                    esriRelNotification.esriRelNotificationNone, false, false, null,
                    "OBJECTID", "",
                    arg.KeyInTarget, "");

                return relClass;
            }
            catch
            {
            }
            return null;
        }

        private static IRelationshipClass CreateRelationInFeatureWorkspace(GeoDbCreationContext context, RelationCreationArgs arg, IFeatureWorkspace workspace)
        {
            IObjectClass target = context.CreatedClasses[arg.Target.ClassName];
            IObjectClass source = context.CreatedClasses[arg.Source.ClassName];
            try
            {
                IRelationshipClass relClass = workspace.CreateRelationshipClass(
                    arg.Name,
                    source,
                    target,
                    //arg.Description, arg.ReversedDescription,
                    arg.Target.ClassName, arg.Source.ClassName,
                    arg.Cardinality,
                    esriRelNotification.esriRelNotificationNone, false, false, null,
                    "OBJECTID", "",
                    arg.KeyInTarget, "");

                return relClass;
            }
            catch
            {
            }
            return null;
        }

        private static void CreateRelations(GeoDbCreationContext context)
        {
            var relationsToBeCreated = new List<RelationCreationArgs>(context.ToBeCreatedRelations);
            while (true)
            {
                var sources = relationsToBeCreated.Select(t => t.Source).Distinct().ToList();
                var independentTargets = relationsToBeCreated.Select(t => t.Target).Distinct().Except(sources).ToList();
                if (independentTargets.Count == 0) break;
                foreach (var target in independentTargets)
                {
                    var correspondingRelations = relationsToBeCreated.Where(t => t.Target == target).ToList();
                    foreach (var relation in correspondingRelations)
                    {
                        if (context.StatusChangedAction != null)
                        {
                            context.StatusChangedAction("Saving relation " + relation.Name + "...");
                        }

                        var dataset = context.CreatedDatasets[relation.Target.FeatureType];
                        IRelationshipClassContainer relClassContainer = (IRelationshipClassContainer)dataset;

                        IRelationshipClass relationClass =
                            CreateRelationInContainer(context, relation, relClassContainer) ??
                            CreateRelationInFeatureWorkspace(context, relation, context.Workspace);

                        if (relationClass == null)
                        {
                            MessageBoxHelper.Show("Can not create relation " + relation.Name + " for classes " +
                                                  relation.Source.ClassName + " and " + relation.Target.ClassName);
                        }
                        //mark relation as processed
                        relationsToBeCreated.Remove(relation);
                    }
                }
            }
        }

        private static ITable CreateJoinTable(GeoDbCreationContext context, string tables, string where, string primary)
        {

            //private static ITable Join()
            //        //{
            //          //foreach (var pair in mergedDictionary)
            //            //{
            //            //    context.Map.AddLayer(new FeatureLayerClass
            //            //    {
            //            //        Name = pair.Key.First().FeatureType.ToString(),
            //            //        FeatureClass = pair.Value as IFeatureClass
            //            //    });     
            //            //}

            //            // Open the RelQueryTable as a feature class.
            //            Type rqtFactoryType = Type.GetTypeFromProgID("esriGeodatabase.RelQueryTableFactory");
            //            IRelQueryTableFactory rqtFactory = (IRelQueryTableFactory)Activator.CreateInstance(rqtFactoryType);

            //            //ESRI.ArcGIS.Geodatabase.RelQueryTableFactory

            //            //merge source and target
            //            //ITable joinedTable = (ITable)rqtFactory.Open(relationClass, false, null, null, String.Empty, false, true);



            //            //CreateJoinTable(context, relation.Source.ClassName + ", " + relation.Target.ClassName,
            //            //    relation.Source.ClassName + ".OBJECTID = " + relation.Target.ClassName + "." +
            //            //    relation.KeyInTarget, relation.Target.ClassName + ".OBJECTID");


            //            var objectClass = CreateJoinTable(context, "Airspace_GeometryComponent_TheAirspaceVolume, Airspace_GeometryComponent, Airspace",
            //            //               "AirportHeliport.OBJECTID = AirportHeliport_ServedCity.AirportHeliport_id OR "
            //            "Airspace_GeometryComponent.OBJECTID = Airspace_GeometryComponent_TheAirspaceVolume.GeometryComponent_id "
            //            + "AND Airspace.OBJECTID = Airspace_GeometryComponent_TheAirspaceVolume.GeometryComponent_id"
            //            ,
            //                           "Airspace_GeometryComponent_TheAirspaceVolume.OBJECTID");

            //            if (objectClass is IFeatureClass)
            //            {
            //                context.Map.AddLayer(new FeatureLayerClass
            //                {
            //                    Name = "Test",
            //                    FeatureClass = objectClass as IFeatureClass
            //                });
            //            }
            //            else
            //            {
            //                ITableCollection tableCollection = context.Map as ITableCollection;
            //                tableCollection.AddTable((ITable)objectClass);
            //            }

            //            //ITableCollection tableCollection = context.Map as ITableCollection;
            //            //tableCollection.AddTable(table);



            ////            System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
            //        //    ITable newTable = null;
            //    //Join
            //    try
            //    {
            //        IMemoryRelationshipClassFactory pMemRelFact = new MemoryRelationshipClassFactory();
            //        IFeatureLayer feaLayer = layer as IFeatureLayer;
            //        ITable originTable = feaLayer.FeatureClass as ITable;
            //        IRelationshipClass pRelClass = pMemRelFact.Open("Join", originTable as IObjectClass, "ID",
            //            table as IObjectClass, "NAMECN", "forward", "backward",
            //            esriRelCardinality.esriRelCardinalityOneToOne);
            //        IDisplayRelationshipClass pDispRC = feaLayer as IDisplayRelationshipClass;
            //        pDispRC.DisplayRelationshipClass(pRelClass, esriJoinType.esriLeftOuterJoin);

            //        newTable = pDispRC as ITable;
            //    }
            //    catch (System.Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //    return newTable;
            //}

            ////merge tables
            var queryDef = (IQueryDef2)context.Workspace.CreateQueryDef();

            // Provide a list of tables to join.
            queryDef.Tables = tables;
            queryDef.WhereClause = where;
          
            IQueryName2 queryName2 = new TableQueryNameClass();
            queryName2.QueryDef = queryDef;
            queryName2.PrimaryKey = primary;
            queryName2.CopyLocally = true;


            IDataset dataset2 = (IDataset) context.Workspace; //Workspaces implement IDataset

            ////// Set the workspace and name of the new QueryTable.
            IDatasetName datasetName = (IDatasetName) queryName2;
            datasetName.WorkspaceName = (IWorkspaceName) dataset2.FullName;
            datasetName.Name = "TestJoin";


            ////// Open the virtual table.
            IName name = (IName) queryName2;


            ITable table = (ITable) name.Open();

          

            return table;
        }

        private static void CreateDatasets(GeoDbCreationContext context)
        {
            foreach (var featureType in context.ToBeCreatedFeatureTypes)
            {
                context.CreatedDatasets[featureType] = CreateFeatureDataset((IWorkspace)context.Workspace, featureType.ToString(), Wgs1984);
            }
        }

        private static void CreateLayers(GeoDbCreationContext context)
        {
            foreach (var arg in context.ToBeCreatedObjects.ToList())
            {
                var objectClass=context.CreatedClasses[arg.ClassName];
                if (objectClass is IFeatureClass)
                {
                    var featureType = arg.FeatureType;
                    var layerName = featureType + "_" + arg.SimpleName;
                    var layerBaseName = layerName;
                    var count = 1;
                    while (context.CreatedLayers.ContainsKey(layerName))
                    {
                        count++;
                        layerName = layerBaseName +"_"+ count;
                    }

                    if (context.StatusChangedAction != null)
                    {
                        context.StatusChangedAction("Saving layer " + layerName + "...");
                    }

                    var classSequence = new List<ClassCreationArgs> {arg};
                    while (classSequence.Last().Parent!=null)
                    {
                        classSequence.Add(classSequence.Last().Parent);
                    }

                    var tables = string.Join(", ", classSequence.Select(t=>t.ClassName));
                    classSequence.Remove(arg);
                    var where = string.Join(" AND ", classSequence.Select(t => arg.ClassName + ".FeatureId = " + t.ClassName + ".FeatureId"));

                    //form sequence
                    var queryDef = (IQueryDef2)context.Workspace.CreateQueryDef();
                    queryDef.Tables = tables;
                    queryDef.WhereClause = where;
                    IQueryName2 queryName2 = new TableQueryNameClass();
                    queryName2.QueryDef = queryDef;
                    queryName2.PrimaryKey = arg.ClassName+".OBJECTID";
                    queryName2.CopyLocally = true;
                    IDataset dataset2 = (IDataset)context.Workspace; //Workspaces implement IDataset
                    ////// Set the workspace and name of the new QueryTable.
                    IDatasetName datasetName = (IDatasetName)queryName2;
                    datasetName.WorkspaceName = (IWorkspaceName)dataset2.FullName;
                    datasetName.Name = layerName;

                    try
                    {
                        ////// Open the virtual table.
                        IName name = (IName)queryName2;
                        ITable table = (ITable)name.Open();
                        IFeatureLayer layer = new FeatureLayerClass
                        {
                            Name = layerName,
                            FeatureClass = table as IFeatureClass
                        };

                        ((ILayerGeneralProperties) layer).LayerDescription = arg.PathName;

                        context.CreatedLayers[layerName] = layer;

                        context.Map.AddLayer(layer);
                    }
                    catch (Exception exception)
                    {
                        MessageBoxHelper.Show("Can not create layer " + layerName);
                    }
                    
                }
            }

            //ITableCollection tableCollection = context.Map as ITableCollection;
            //tableCollection.AddTable((ITable)objectClass);
        }

        private static void CreateGeoDbFromContext(GeoDbCreationContext context)
        {
            //check procedures
            CheckClasNames(context);
            //create procedures
            CreateDatasets(context);
            CreateSystemClass(context);
            CreateClasses(context);
            CreateRelations(context);
            CreateLayers(context);
        }

        #endregion

        public static void CreateGeoDb(string filePath, List<ConfigurationViewModel> configurationList, Action<string> statusChangedAction = null)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            var mxFilePath = filePath.Substring(0, filePath.Length - 4) + ".mxd";
            if (File.Exists(mxFilePath))
            {
                File.Delete(mxFilePath);
            }

            IMapDocument mapDocument = new MapDocumentClass();
            mapDocument.New(mxFilePath);

            IMap map = mapDocument.Map[0];

            var folderName = Path.GetDirectoryName(filePath);
            var fileName = Path.GetFileName(filePath);
            var extension=Path.GetExtension(filePath);

            IWorkspace workspace=extension.ToLower()==".mdb"? CreateAccessWorkspace(folderName, fileName):
                CreateFileGdbWorkspace(folderName, fileName);
            

            var context = new GeoDbCreationContext
            {
                Map = map,
                StatusChangedAction = statusChangedAction,
                Workspace = (IFeatureWorkspace) workspace
            };

            //gather config data
            var modelList = new List<FeatureTreeViewItemViewModel>();
            foreach (var configuration in configurationList)
            {
                if (configuration == null)
                {
                    MessageBoxHelper.Show("Dataset is not complete.", "Bad Dataset", MessageBoxButton.OK, MessageBoxImage.Error);
                    continue;
                }
                if (configuration.Entity == null || configuration.Entity.Data == null)
                {
                    MessageBoxHelper.Show("Dataset configuration " + configuration.Name + " is not complete.", "Bad Dataset", MessageBoxButton.OK, MessageBoxImage.Error);
                    continue;
                }
                var configurationModel = new FeatureDependencyConfigurationViewModel();
                configurationModel.Deserialize(configuration.Entity.Data);
                modelList.Add(configurationModel.FirstGeneration.First());
            }

            //merge models
            var list = new List<FeatureTreeViewItemViewModel>();
            foreach (var model in modelList)
            {
                GetAllModel(model, list);
            }

            
            var groups = list.GroupBy(t => t.FeatureType);
            foreach (var group in groups)
            {
                if (group.Count() > 1)
                {
                    MergeModels(group.ToList());
                }
            }

            foreach (var model in modelList)
            {
                ProcessModel(context, model);
            }

            CreateGeoDbFromContext(context);

            mapDocument.Save();
            mapDocument.Close();
           

            if (statusChangedAction != null)
            {
                statusChangedAction("Done");
            }
        }

        public static void MergeModels(IList<FeatureTreeViewItemViewModel> group)
        {
            //united properties and links
            var optionalProperties = new SortedSet<string>();
            var mandatoryProperties = new SortedSet<string>();
            var optionalLinks = new SortedSet<string>();
            var mandatoryLinks = new SortedSet<string>();

            //calculate united
            foreach (var item in group)
            {
                optionalProperties.UnionWith(item.OptionalProperties ?? new HashSet<string>());
                mandatoryProperties.UnionWith(item.MandatoryProperties ?? new HashSet<string>());
                optionalLinks.UnionWith(item.OptionalLinks ?? new HashSet<string>());
                mandatoryLinks.UnionWith(item.MandatoryLinks ?? new HashSet<string>());
            }

            //set united
            foreach (var item in group)
            {
                item.OptionalProperties = new HashSet<string>();
                item.OptionalProperties.UnionWith(optionalProperties);

                item.MandatoryProperties = new HashSet<string>();
                item.MandatoryProperties.UnionWith(mandatoryProperties);

                item.OptionalLinks = new HashSet<string>();
                item.OptionalLinks.UnionWith(optionalLinks);

                item.MandatoryLinks = new HashSet<string>();
                item.MandatoryLinks.UnionWith(mandatoryLinks);
            }
        }

        private static void GetAllModel(FeatureTreeViewItemViewModel treeItem, List<FeatureTreeViewItemViewModel> allList)
        {
            allList.Add(treeItem);

            foreach (FeatureTreeViewItemViewModel child in treeItem.Children)
            {
                GetAllModel(child, allList);


                var links = new HashSet<String>();

                foreach (var link in (child.OptionalLinks ?? new HashSet<string>()).Union(child.MandatoryLinks ?? new HashSet<string>()))
                {
                    int i = link.LastIndexOf("/", StringComparison.Ordinal);
                    if (i > 0)
                    {
                        var parts=(link.Substring(0, i)+"\\").Split('/');
                        string subPath=null;
                        foreach (var part in parts)
                        {
                            if (subPath == null) subPath = part;
                            else subPath += "\\" + part;
                            links.Add(subPath);
                        }
                    }
                }
                    
                if (child.IsDirect)
                {
                    if (treeItem.OptionalProperties == null) treeItem.OptionalProperties = new HashSet<string>();
                    treeItem.OptionalProperties.UnionWith(links);
                }
                else
                {
                    if (child.OptionalProperties == null) child.OptionalProperties = new HashSet<string>();
                    child.OptionalProperties.UnionWith(links);
                }
            }
        }

        public static ClassCreationArgs ProcessModel(GeoDbCreationContext context, FeatureTreeViewItemViewModel model, ClassCreationArgs parentModel = null, string classPath = null)
        {
            if (model == null)
            {
                throw new Exception("Abnormal model");
            }
            if (model.FeatureType == null)
            {
                throw new Exception("Abnormal model, FeatureType is missing");
            }
            var modelFeatureType = (FeatureType)model.FeatureType;


            context.ToBeCreatedFeatureTypes.Add(modelFeatureType);

            //avoid infinite loop
            var classCreationArg = context.ToBeCreatedObjects.FirstOrDefault(t => t.SimpleName == modelFeatureType.ToString());

            if (classCreationArg == null)
            {
                if (context.StatusChangedAction != null)
                {
                    context.StatusChangedAction("Creating " + modelFeatureType + " feature class...");
                }

                //process node
                //create class creation ars

                classPath = classPath == null ? modelFeatureType.ToString() : classPath + "\\" + modelFeatureType;

                classCreationArg = new ClassCreationArgs
                {
                    ClassName = GetClassName(modelFeatureType),
                    SimpleName = modelFeatureType.ToString(),
                    PathName = classPath,
                    FeatureType = modelFeatureType
                };
                //and register it in dictionary
                context.ToBeCreatedObjects.Add(classCreationArg);

                //get aim properties corresponding to root type
                var aimPropInfoArr = AimMetadata.GetAimPropInfos((int)modelFeatureType).ToList();

                //prepare properties
                var properties = new HashSet<string>(model.OptionalProperties ?? (new HashSet<string>()).Union(model.MandatoryProperties ?? new HashSet<string>()));

                //add them
                AddPropertyListToClass(context, properties, classCreationArg, aimPropInfoArr);

            }
            else
            {
                return classCreationArg;
            }
           
            //add links
            //if (parentModel != null)
            //{
            //    //add link path
            //    IField linkPathField = new FieldClass();
            //    IFieldEdit linkPathFieldEdit = (IFieldEdit)linkPathField;
            //    linkPathFieldEdit.Name_2 = GetLinkName(parentModel.SimpleName);
            //    linkPathFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            //    classCreationArg.Fields.Add(linkPathField);

            //    //add forein key from parentModel to classCreationArg
            //    var foreinKeyName = GetForeinKeyName(parentModel.SimpleName);

            //    IField field = new FieldClass();
            //    IFieldEdit fieldEdit = (IFieldEdit)field;
            //    fieldEdit.Name_2 = foreinKeyName;
            //    fieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;

            //    classCreationArg.Fields.Add(fieldEdit);

            //    context.ToBeCreatedRelations.Add(new RelationCreationArgs
            //    {
            //        Source = parentModel,
            //        Target = classCreationArg,
            //        Cardinality = esriRelCardinality.esriRelCardinalityOneToMany,
            //        KeyInTarget = foreinKeyName,
            //        Name = GetRelationName(parentModel.SimpleName, classCreationArg.SimpleName, "Link"),
            //        Description = "Owns",
            //        ReversedDescription = "Is owned by"
            //    });
            //}

            //process node's children
            if (model.Children != null)
            {
                foreach (var treeViewItemViewModel in model.Children)
                {
                    var child = (FeatureTreeViewItemViewModel)treeViewItemViewModel;
                    var childClass=ProcessModel(context, child, classCreationArg, classPath);


                    var links =
                        (child.MandatoryLinks ?? new HashSet<string>()).Union(child.OptionalLinks ??
                                                                              new HashSet<string>()).Distinct().ToList();
                    //add links
                    if (child.IsDirect)
                    {
                        foreach (var link in links)
                        {
                            var source = model.FeatureType + "/" + link;
                            int i = source.LastIndexOf('/');
                            var objectName=source.Substring(0, i).Replace("/",".");
                            var objectModel = context.ToBeCreatedObjects.First(t => t.PathName.Equals(objectName));

                 
                            var linkClass = new ClassCreationArgs();
                            linkClass.PathName = source.Replace("/", ".");
                           
                            linkClass.ClassAlias = linkClass.PathName+" link from "+model.FeatureType+" to "+child.FeatureType;
                            var parts=linkClass.PathName.Split('.').ToList();
                            while (parts.Count>3) parts.RemoveAt(0);

                            linkClass.ClassName = String.Join("_", parts);
                            {
                                IField field = new FieldClass();
                                IFieldEdit pathFieldEdit = (IFieldEdit)field;
                                pathFieldEdit.Name_2 = "SOURCE_ID";
                                pathFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                                linkClass.Fields.Add(field);
                            }
                            {
                                IField field = new FieldClass();
                                IFieldEdit pathFieldEdit = (IFieldEdit)field;
                                pathFieldEdit.Name_2 = "TARGET_ID";
                                pathFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                                linkClass.Fields.Add(field);
                            }

                            linkClass.FeatureType = (FeatureType)model.FeatureType;

                            context.ToBeCreatedObjects.Add(linkClass);


                            var targetRelation = new RelationCreationArgs
                            {
                                Name = linkClass.ClassName + "_target",
                                Source = classCreationArg,
                                Target = linkClass,
                                KeyInSource = "OBJECTID",
                                KeyInTarget = "TARGET_ID",
                                Cardinality = esriRelCardinality.esriRelCardinalityOneToMany
                            };

                            context.ToBeCreatedRelations.Add(targetRelation);
                            

                            if (objectModel != null)
                            {
                                var sourceRelation = new RelationCreationArgs
                                {
                                    Name = linkClass.ClassName + "_source",
                                    Source = objectModel,
                                    Target = linkClass,
                                    KeyInSource = "OBJECTID",
                                    KeyInTarget = "SOURCE_ID",
                                    Cardinality = esriRelCardinality.esriRelCardinalityOneToMany
                                };

                                context.ToBeCreatedRelations.Add(sourceRelation);
                            }
                        }
                    }
                    else
                    {
                        foreach (var link in links)
                        {
                            var source = child.FeatureType + "/" + link;
                            int i = source.LastIndexOf('/');
                            var objectName = source.Substring(0, i).Replace("/", ".");
                            var objectModel = context.ToBeCreatedObjects.First(t => t.PathName.Equals(objectName));


                            var linkClass = new ClassCreationArgs();
                            linkClass.PathName = source.Replace("/", ".");
                            linkClass.ClassAlias = linkClass.PathName + " link from " + child.FeatureType + " to " + model.FeatureType;
                            var parts = linkClass.PathName.Split('.').ToList();
                            while (parts.Count > 3) parts.RemoveAt(0);

                            linkClass.ClassName = String.Join("_", parts);
                            {
                                IField field = new FieldClass();
                                IFieldEdit pathFieldEdit = (IFieldEdit)field;
                                pathFieldEdit.Name_2 = "SourceId";
                                pathFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                                linkClass.Fields.Add(field);
                            }
                            {
                                IField field = new FieldClass();
                                IFieldEdit pathFieldEdit = (IFieldEdit)field;
                                pathFieldEdit.Name_2 = "TargetId";
                                pathFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                                linkClass.Fields.Add(field);
                            }

                            linkClass.FeatureType = (FeatureType)child.FeatureType;

                            context.ToBeCreatedObjects.Add(linkClass);


                            var targetRelation = new RelationCreationArgs
                            {
                                Name = linkClass.ClassName + "_target",
                                Source = childClass,
                                Target = linkClass,
                                KeyInSource = "OBJECTID",
                                KeyInTarget = "TARGET_ID",
                                Cardinality = esriRelCardinality.esriRelCardinalityOneToMany
                            };

                            context.ToBeCreatedRelations.Add(targetRelation);


                            if (objectModel != null)
                            {
                                var sourceRelation = new RelationCreationArgs
                                {
                                    Name = linkClass.ClassName + "_source",
                                    Source = classCreationArg,
                                    Target = linkClass,
                                    KeyInSource = "OBJECTID",
                                    KeyInTarget = "SOURCE_ID",
                                    Cardinality = esriRelCardinality.esriRelCardinalityOneToMany
                                };

                                context.ToBeCreatedRelations.Add(sourceRelation);
                            }
                        }
                    }
                }
            }

            return classCreationArg;
        }

        private static void AddPropertyListToClass(GeoDbCreationContext context, IEnumerable<string> properties, 
            ClassCreationArgs classCreationArg, IList<AimPropInfo> aimPropInfoArr, string grandparent=null)
        {

            var rootProperties = properties.Select(t => t.Split('\\')[0]).Distinct().ToList();
            
            foreach (var property in rootProperties)
            {
                var aimInfo = aimPropInfoArr.FirstOrDefault(t => t.Name == property);
                if (aimInfo == null)
                {
                    throw new Exception("Wrong property");
                    //continue;
                }

                var propertyPath = property + "\\";
                var subProperties = properties.Where(t => t.StartsWith(propertyPath)).ToList();

                if (subProperties.Count > 0)
                {
                    //it is complex object

                    var subClassCreationArgs = new ClassCreationArgs
                    {
                        ClassName = grandparent==null? GetClassName(classCreationArg.SimpleName, aimInfo.Name) :
                        GetClassName(grandparent, classCreationArg.SimpleName, aimInfo.Name),
                        SimpleName = aimInfo.Name,
                        PathName = classCreationArg.PathName + "." + aimInfo.Name,
                        FeatureType = classCreationArg.FeatureType,
                        Parent = classCreationArg
                    };
                    context.ToBeCreatedObjects.Add(subClassCreationArgs);

                    //cut root property
                    subProperties = subProperties.Select(t => t.Substring(aimInfo.Name.Length + 1)).Except(new List<String> { string.Empty}).ToList();

                    if (subProperties.Count > 0)
                    {
                        AddPropertyListToClass(context, subProperties, subClassCreationArgs, aimInfo.PropType.Properties, classCreationArg.SimpleName);
                    }

                    AddForeinKeyToClass(context, aimInfo.IsList ? esriRelCardinality.esriRelCardinalityOneToMany : esriRelCardinality.esriRelCardinalityOneToOne,
                        aimInfo, subClassCreationArgs, classCreationArg, grandparent);
                }
                else
                {
                    if (aimInfo.IsList)
                    {
                        var listClassArg = new ClassCreationArgs
                        {
                            ClassName = GetClassName(classCreationArg.SimpleName, aimInfo.Name),
                            SimpleName =aimInfo.Name,
                            PathName = classCreationArg.PathName + "." + aimInfo.Name,
                            FeatureType = classCreationArg.FeatureType,
                            Parent = classCreationArg
                        };
                        context.ToBeCreatedObjects.Add(listClassArg);

                        UtilAddAimPropertyToClass(aimInfo, listClassArg);

                        AddForeinKeyToClass(context, esriRelCardinality.esriRelCardinalityOneToMany, aimInfo, listClassArg, classCreationArg, grandparent);
                    }
                    else
                    {
                        if (aimInfo.PropType.SubClassType == AimSubClassType.Choice)
                        {
                            //TODO: support!
                            #warning do support
                            throw new NotSupportedException();
                        }
                        UtilAddAimPropertyToClass(aimInfo, classCreationArg);
                    }
                }
            }

        }

        private static void AddForeinKeyToClass(
           GeoDbCreationContext context,
           esriRelCardinality cardinality,
           AimPropInfo aimInfo,
           ClassCreationArgs listClassArg,
           ClassCreationArgs classCreationArg,
           string grandparent
           )
        {
            var foreinKeyName = GetForeinKeyName(classCreationArg.SimpleName);

            IField field = new FieldClass();
            IFieldEdit fieldEdit = (IFieldEdit)field;
            fieldEdit.Name_2 = foreinKeyName;
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;

            listClassArg.Fields.Add(fieldEdit);

            context.ToBeCreatedRelations.Add(new RelationCreationArgs
            {
                Source = classCreationArg,
                Target = listClassArg,
                Cardinality = cardinality,
                KeyInTarget = foreinKeyName,
                Name = GetRelationName(grandparent, classCreationArg.SimpleName, aimInfo.Name),
                Description = "Owns",
                ReversedDescription = "Is owned by"
            });
        }

        #region AimProperty

        public static bool IsSimple(AimPropInfo aimInfo)
        {
            switch (aimInfo.PropType.AimObjectType)
            {
                case AimObjectType.Feature:
                case AimObjectType.Object:
                    return false;
                case AimObjectType.DataType:
                case AimObjectType.Field:
                    switch (aimInfo.PropType.SubClassType)
                    {
                        case AimSubClassType.AbstractFeatureRef:
                        case AimSubClassType.Choice:
                            return false;
                        case AimSubClassType.None:
                        case AimSubClassType.ValClass:
                        case AimSubClassType.Enum:
                            return true;
                    }
                    break;
            }
            //we do not supposed to be here!
            return false;
        }

        public static esriFieldType GetEsriType(AimPropInfo aimInfo)
        {
            switch (aimInfo.PropType.AimObjectType)
            {
                case AimObjectType.DataType:
                case AimObjectType.Feature:
                case AimObjectType.Object:
                    return esriFieldType.esriFieldTypeOID;
                case AimObjectType.Field:

                    switch (aimInfo.PropType.SubClassType)
                    {
                        case AimSubClassType.Choice:
                            return esriFieldType.esriFieldTypeOID;
                        case AimSubClassType.None:

                            switch ((AimFieldType)aimInfo.PropType.Index)
                            {
                                case AimFieldType.SysBool:
                                    return esriFieldType.esriFieldTypeSmallInteger;
                                case AimFieldType.SysDateTime:
                                    return esriFieldType.esriFieldTypeDate;
                                case AimFieldType.SysDouble:
                                    return esriFieldType.esriFieldTypeDouble;
                                case AimFieldType.SysGuid:
                                    return esriFieldType.esriFieldTypeGUID;
                                case AimFieldType.SysEnum:
                                case AimFieldType.SysString:
                                    return esriFieldType.esriFieldTypeString;
                                case AimFieldType.SysInt64:
                                case AimFieldType.SysInt32:
                                case AimFieldType.SysUInt32:
                                    return esriFieldType.esriFieldTypeInteger;

                                case AimFieldType.GeoPoint:
                                case AimFieldType.GeoPolyline:
                                case AimFieldType.GeoPolygon:
                                    return esriFieldType.esriFieldTypeGeometry;
                            }

                            break;
                        case AimSubClassType.ValClass:
                            //we do not supposed to be here!
                            return esriFieldType.esriFieldTypeInteger;
                        case AimSubClassType.Enum:
                            return esriFieldType.esriFieldTypeString;
                        case AimSubClassType.AbstractFeatureRef:
                            return esriFieldType.esriFieldTypeOID;
                    }

                    break;

            }
            //we do not supposed to be here!
            return esriFieldType.esriFieldTypeInteger;
        }

        #endregion



    }
}
