using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Aim.Utilities;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.View;
using Aran.Temporality.CommonUtil.ViewModel;
using TOSSM.Converter;
using TOSSM.Graph;
using TOSSM.Util;
using TOSSM.ViewModel.Pane.Base;

namespace TOSSM.ViewModel.Document.Graph
{
    public class RelationExplorerDocViewModel : DocViewModel
    {
        #region Ctor

        public RelationExplorerDocViewModel(FeatureType featureType, Guid id, DateTime date):base(featureType,id,date)
        {
        }

        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/chart.png", UriKind.RelativeOrAbsolute);

        #endregion

        private BlockerModel _blockerModel;
        public BlockerModel BlockerModel
        {
            get => _blockerModel ?? (_blockerModel = new BlockerModel { ActivatingObject = this });
            set => _blockerModel = value;
        }

        public AimFeature EditedFeature { get; set; }


        public override void Load()
        {
            if (IsLoaded) return;

            BlockerModel.BlockForAction(
                    () =>
                    {
                        EditedFeature = DataProvider.GetState(FeatureType, FeatureIdentifier, AiracDate);
                        Title = "Relations of " + EditedFeature.Feature.GetType().Name + " " +
                            HumanReadableConverter.ShortAimDescription(EditedFeature.Feature) +
                            AiracSelectorViewModel.AiracMessage(AiracDate);

                        
                         _graph.Clear();
                        

                        LoadRelations(EditedFeature.Feature, true);
                        LoadRelations(EditedFeature.Feature, false);


                        OnPropertyChanged("Graph");
                        IsLoaded = true;
                    });

            
        }



        #region Logic

        private void OnVertexClick(PocVertex vertex)
        {
            MainManagerModel.Instance.View(vertex.Feature,AiracDate);
        }


        private void LoadRelations(Feature feature1, bool direct)
        {
            //add vertex
            var v = Vertices.Where(t => t.Identifier == feature1.Identifier).FirstOrDefault();
            if (v == null)
            {
                Application.Current.Dispatcher.Invoke(
                       DispatcherPriority.Background,
                       (Action)(() =>
                       {
                           v = new PocVertex
                           {
                               OnCommand = OnVertexClick,
                               RelationsChanged = UpdateVertex,
                               Feature = feature1
                           };
                           Graph.AddVertex(v);
                           Vertices.Add(v);
                       }));
            }

            if (direct)
            {
                if (v.DirectLinks!=null)
                {
                    var usedTypes = new HashSet<FeatureType>();
                    foreach (var linkedFeature in v.DirectLinks)
                    {
                        usedTypes.Add(linkedFeature.FeatureType);
                        AimFeature feature = linkedFeature;
                        Application.Current.Dispatcher.Invoke(
                               DispatcherPriority.Background,
                               (Action)(() => AddRelation(feature1, feature.Feature)));
                    }
                }
                else
                {
                    v.DirectLinks=new List<AimFeature>();

                    //get references
                    var featurePropList = new List<RefFeatureProp>();
                    AimMetadataUtility.GetReferencesFeatures(feature1, featurePropList);

                    var usedTypes = new HashSet<FeatureType>();
                    foreach (var featureProp in featurePropList)
                    {
                        var linkedFeature = DataProvider.GetState(featureProp.FeatureType,
                                                                featureProp.RefIdentifier,
                                                                AiracDate);
                        usedTypes.Add(linkedFeature.FeatureType);

                        v.DirectLinks.Add(linkedFeature);

                        Application.Current.Dispatcher.Invoke(
                               DispatcherPriority.Background,
                               (Action)(() => AddRelation(feature1, linkedFeature.Feature)));


                    }
                    var mayRefereFromTypes = RelationUtil.MayRefereFrom(feature1.FeatureType).Except(usedTypes);
                    foreach (var featureTypeId in mayRefereFromTypes)
                    {
                        //FeatureType id = featureTypeId;
                        //Application.Current.Dispatcher.Invoke(
                        //      DispatcherPriority.Background,
                        //      (Action)(() => AddRelation(feature1, id)));
                    }
                }
               

                
                v.SetDirect(true);
            }
            else
            {
                var mayRefereToTypes = RelationUtil.MayRefereTo(feature1.FeatureType);

                if (v.ReverseLinks!=null)
                {
                    foreach (KeyValuePair<FeatureType, List<AimFeature>> pair in v.ReverseLinks)
                    {
                        foreach (var feature in pair.Value)
                        {
                            var feature2 = feature;
                            Application.Current.Dispatcher.Invoke(
                                DispatcherPriority.Background,
                                (Action) (() => AddRelation(feature2.Feature, feature1)));
                        }
                    }
                }
                else
                {
                    v.ReverseLinks = new Dictionary<FeatureType, List<AimFeature>>();
                    foreach (var featureType in mayRefereToTypes)
                    {
                        MainManagerModel.Instance.StatusText = "Searching reverse links from " + featureType.ToString() + "...";

                        List<AimFeature> data = DataProvider.GetReverseLinksTo(feature1, featureType, AiracDate, true);

                        if (data != null && data.Count > 0)
                        {
                            v.ReverseLinks[featureType] = data;

                            foreach (var feature in data)
                            {
                                var feature2 = feature;

                                Application.Current.Dispatcher.Invoke(
                                DispatcherPriority.Background,
                                (Action)(() => AddRelation(feature2.Feature, feature1)));
                            }
                        }
                        else
                        {
                            //FeatureType type = featureType;
                            //Application.Current.Dispatcher.Invoke(
                            //  DispatcherPriority.Background,
                            //  (Action)(() => AddRelation(type, feature1)));
                        }
                    }
                    MainManagerModel.Instance.StatusText = "Done";
                }

                v.SetReverse(true);
            }
        }

        List<PocVertex> Vertices = new List<PocVertex>();


        private void AddRelation(FeatureType featureType, Feature feature2)
        {
            //add vertices
            var from =
                Vertices.Where(
                    t => t.Identifier == default(Guid) && t.FeatureType == featureType).
                    FirstOrDefault();
            if (from == null)
            {
                from = new PocVertex
                           {
                               RelationsChanged = UpdateVertex,
                               FeatureType = featureType,
                               Border = 2,
                               ID = featureType.ToString()
                           };
                Graph.AddVertex(from);
                Vertices.Add(from);
            }
            var to =
                Vertices.Where(t => t.Identifier == feature2.Identifier).FirstOrDefault();
            if (to == null)
            {
                to = new PocVertex
                         {
                             RelationsChanged = UpdateVertex,
                             FeatureType = feature2.FeatureType,
                             Identifier = feature2.Identifier,
                             ID = HumanReadableConverter.ShortAimDescription(feature2)
                         };


                Graph.AddVertex(to);
                Vertices.Add(to);
            }


            //add edges
            var path = RelationUtil.GetConnectionProperty(from.FeatureType,
                                                          to.FeatureType);
            var edgeString = string.Format("{0} - {1} connected by the following ",
                                           from.ID, to.ID);
            var newEdge = new PocEdge(edgeString, from, to);
            foreach (var connection in path.Split('\n'))
            {
                newEdge.ConnectionList.Add(connection);
            }

            newEdge.ID += newEdge.ConnectionList.Count > 1 ? "properties:" : "property:";

            Graph.AddEdge(newEdge);

        }

        private void AddRelation(Feature feature1, FeatureType featureType)
        {
            //add vertices
            var from = Vertices.Where(t => t.Identifier == feature1.Identifier).FirstOrDefault();
            if (from == null)
            {
                from = new PocVertex
                           {
                               RelationsChanged = UpdateVertex,
                               FeatureType = feature1.FeatureType,
                               Identifier = feature1.Identifier,
                               ID =
                                   featureType + " " +
                                   HumanReadableConverter.ShortAimDescription(
                                       feature1)
                           };
                Graph.AddVertex(from);
                Vertices.Add(from);
            }
            var to =
                Vertices.Where(
                    t =>
                    t.Identifier == default(Guid) && t.FeatureType == featureType).
                    FirstOrDefault();
            if (to == null)
            {
                to = new PocVertex
                         {
                             RelationsChanged = UpdateVertex,
                             FeatureType = featureType,
                             Border = 2,
                             ID = featureType.ToString()
                         };
                Graph.AddVertex(to);
                Vertices.Add(to);
            }

            //add edges
            var path = RelationUtil.GetConnectionProperty(from.FeatureType,
                                                          to.FeatureType);
            var edgeString = string.Format("{0} - {1} connected by the following ",
                                           from.ID, to.ID);
            var newEdge = new PocEdge(edgeString, from, to);
            foreach (var connection in path.Split('\n'))
            {
                newEdge.ConnectionList.Add(connection);
            }

            newEdge.ID += newEdge.ConnectionList.Count > 1
                              ? "properties:"
                              : "property:";

            Graph.AddEdge(newEdge);
        }

        private void AddRelation(Feature feature1, Feature feature2)
        {
            if (Graph.Edges.Where(t => t.Source.Identifier == feature1.Identifier && t.Target.Identifier == feature2.Identifier).Any())
            {
                //we already have this relation
                return;
            }

            //add vertices
            var from = Vertices.Where(t => t.Identifier == feature1.Identifier).FirstOrDefault();
            if (from == null)
            {
                from = new PocVertex
                           {
                               OnCommand = OnVertexClick,
                               RelationsChanged = UpdateVertex,
                               Feature = feature1
                           };
                Graph.AddVertex(from);
                Vertices.Add(from);
            }
            var to = Vertices.Where(t => t.Identifier == feature2.Identifier).FirstOrDefault();
            if (to == null)
            {
                to = new PocVertex
                         {
                             OnCommand = OnVertexClick,
                             RelationsChanged = UpdateVertex,
                             Feature = feature2
                         };
                Graph.AddVertex(to);
                Vertices.Add(to);
            }


            //add edges
            var path = RelationUtil.GetConnectionProperty(from.FeatureType, to.FeatureType);
            var edgeString = string.Format("{0} - {1} connected by the following ", from.ID, to.ID);
            var newEdge = new PocEdge(edgeString, from, to);
            foreach (var connection in path.Split('\n'))
            {
                newEdge.ConnectionList.Add(connection);
            }

            newEdge.ID += newEdge.ConnectionList.Count > 1 ? "properties:" : "property:";

            Graph.AddEdge(newEdge);

        }

        #endregion

        private void DeleteVertexR(PocVertex v, HashSet<PocVertex> toBeDeleted)
        {
            if (!toBeDeleted.Add(v)) return;

            var newLevel = Graph.Edges.Where(t => t.Source.Identifier == v.Identifier).Select(t => t.Target).ToList();
            foreach (var pocVertex in newLevel)
            {
                DeleteVertexR(pocVertex, toBeDeleted);
            }

            newLevel = Graph.Edges.Where(t => t.Target.Identifier == v.Identifier).Select(t => t.Source).ToList();
            foreach (var pocVertex in newLevel)
            {
                DeleteVertexR(pocVertex, toBeDeleted);
            }
        }

        private List<PocVertex> GetFirstStepInPath(PocVertex from, PocVertex to)
        {
            var allReferences = new List<List<PocVertex>>();
            var accountedReferences = new HashSet<PocVertex>();

            var currentList = new List<PocVertex> { from };

            allReferences.Add(currentList);
            accountedReferences.Add(from);

            while (true)
            {
                //form next level references
                var references = new List<PocVertex>();

                //load all links from current level
                foreach (var v in currentList)
                {
                    var nextLevelList=Graph.Edges.Where(t => t.Target.Identifier == v.Identifier).Select(t => t.Source).
                        Union(Graph.Edges.Where(t => t.Source.Identifier == v.Identifier).Select(t => t.Target)).
                        Except(accountedReferences).
                        ToList();
                    //take into account
                    foreach (var v2 in nextLevelList)
                    {
                        references.Add(v2);
                        accountedReferences.Add(v2);
                    }
                }

                //here references contains all next level feature types
                if (references.Count == 0) break;

                allReferences.Add(references);
                if (references.Contains(to)) break;

                currentList = new List<PocVertex>(references);
            }

            if (allReferences.Count>0 && allReferences.Last().Contains(to))
            {
                //found

                allReferences.Last().Clear();
                allReferences.Last().Add(to);

                //find way back
                for (var i = allReferences.Count - 1; i >= 1; i--)
                {
                    var references = new List<PocVertex>();
                    //load all links from current level
                    foreach (var v in allReferences[i])
                    {
                        references.AddRange(Graph.Edges.Where(t => t.Target.Identifier == v.Identifier).Select(t => t.Source).
                                Union(Graph.Edges.Where(t => t.Source.Identifier == v.Identifier).Select(t => t.Target)));
                    }

                    allReferences[i - 1] = allReferences[i - 1].Intersect(references).ToList();


                    if (allReferences[i - 1].Count == 0)
                    {
                        throw new Exception("Logic exception");
                    }
                }

                if (allReferences.Count>1)
                {
                    return allReferences[1].ToList();
                }
            }

            return new List<PocVertex>();
        }

        private void UpdateVertex(PocVertex vertex)
        {
            if (vertex.Feature!=null)
            {
                BlockerModel.BlockForAction(
                    () =>
                        {
                            var origin = Vertices.Where(t => t.Identifier == EditedFeature.Feature.Identifier).FirstOrDefault();
                                        
                            switch (vertex.IsDirect)
                            {
                                case false:
                                    {
                                        var firstLevelVerticesToBeDeleted = Graph.Edges.Where(t => t.Source.Identifier == vertex.Identifier).Select(t => t.Target).ToList();
                                        var verticesToBeDeleted=new HashSet<PocVertex> {vertex};
                                        var verticesToKeep = GetFirstStepInPath(vertex, origin);
                                        foreach (var pocVertex in verticesToKeep)
                                        {
                                            verticesToBeDeleted.Add(pocVertex);
                                        }

                                        //load all connected vertices
                                        foreach (var pocVertex in firstLevelVerticesToBeDeleted)
                                        {
                                            DeleteVertexR(pocVertex, verticesToBeDeleted);
                                        }

                                        verticesToBeDeleted.Remove(vertex);
                                        foreach (var pocVertex in verticesToKeep)
                                        {
                                            verticesToBeDeleted.Remove(pocVertex);
                                        }

                                        


                                        var edgesToBeDeleted=Graph.Edges.Where(
                                            t => verticesToBeDeleted.Contains(t.Source) || verticesToBeDeleted.Contains(t.Target)).
                                            ToList();


                                        Application.Current.Dispatcher.Invoke(
                                               DispatcherPriority.Background,
                                               (Action)(() =>
                                                            {
                                                                Graph.RemoveEdgeIf(edgesToBeDeleted.Contains);
                                                                Graph.RemoveVertexIf(verticesToBeDeleted.Contains);
                                                                Vertices=Vertices.Except(verticesToBeDeleted).ToList();
                                                            }));

                                       
                                    }
                                    break;
                                case true:
                                        LoadRelations(vertex.Feature, true);
                                    break;
                            }

                            switch (vertex.IsReverse)
                            {
                                case false:
                                    {
                                        var firstLevelVerticesToBeDeleted = Graph.Edges.Where(t => t.Target.Identifier == vertex.Identifier).Select(t => t.Source).ToList();
                                        var verticesToBeDeleted = new HashSet<PocVertex> { vertex };
                                        var verticesToKeep = GetFirstStepInPath(vertex, origin);
                                        foreach (var pocVertex in verticesToKeep)
                                        {
                                            verticesToBeDeleted.Add(pocVertex);
                                        }

                                        //load all connected vertices
                                        foreach (var pocVertex in firstLevelVerticesToBeDeleted)
                                        {
                                            DeleteVertexR(pocVertex, verticesToBeDeleted);
                                        }

                                        verticesToBeDeleted.Remove(vertex);
                                        foreach (var pocVertex in verticesToKeep)
                                        {
                                            verticesToBeDeleted.Remove(pocVertex);
                                        }



                                        var edgesToBeDeleted = Graph.Edges.Where(
                                            t => verticesToBeDeleted.Contains(t.Source) || verticesToBeDeleted.Contains(t.Target)).
                                            ToList();


                                        Application.Current.Dispatcher.Invoke(
                                               DispatcherPriority.Background,
                                               (Action)(() =>
                                               {
                                                   Graph.RemoveEdgeIf(edgesToBeDeleted.Contains);
                                                   Graph.RemoveVertexIf(verticesToBeDeleted.Contains);
                                                   Vertices = Vertices.Except(verticesToBeDeleted).ToList();
                                               }));
                                    }
                                    break;
                                case true:
                                    LoadRelations(vertex.Feature, false);
                                    break;
                            }


                            OnPropertyChanged("Graph");
                            OnPropertyChanged("LayoutAlgorithmType");
                        });
            }
        }


        #region Data

        private string _layoutAlgorithmType = _layoutAlgorithmTypes[1];
        private PocGraph _graph = new PocGraph(true);
        private static readonly List<String> _layoutAlgorithmTypes = new List<string>
                                                                         {
                                                                       "BoundedFR",
                                                                       "Circular",
                                                                       "LinLog",
                                                                  };

        //private static readonly List<String> _layoutAlgorithmTypes = new List<string>
        //                                                                 {
        //                                                               "BoundedFR",
        //                                                               "Circular",
        //                                                               "CompoundFDP",
        //                                                               "EfficientSugiyama",
        //                                                               "FR",
        //                                                               "ISOM",
        //                                                               "KK",
        //                                                               "LinLog",
        //                                                               "Tree",
        //                                                          };

        #endregion

        #region Public Properties

        public List<String> LayoutAlgorithmTypes => _layoutAlgorithmTypes;

        public string LayoutAlgorithmType
        {
            get => _layoutAlgorithmType;
            set
            {
                _layoutAlgorithmType = value;
                OnPropertyChanged("LayoutAlgorithmType");
            }
        }

        public PocGraph Graph
        {
            get => _graph;
            set
            {
                _graph = value;
                OnPropertyChanged("Graph");
            }
        }

        #endregion
    }
}
