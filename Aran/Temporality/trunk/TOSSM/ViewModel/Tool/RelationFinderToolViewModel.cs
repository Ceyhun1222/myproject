using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Aim;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.ViewModel;
using TOSSM.Graph;
using TOSSM.ViewModel.Pane.Base;

namespace TOSSM.ViewModel.Tool
{
    public class RelationFinderToolViewModel : ToolViewModel
    {
        #region Ctor

        public static string ToolContentId = "Relation Finder";

        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/direction.png", UriKind.RelativeOrAbsolute);

        public RelationFinderToolViewModel() : base(ToolContentId)
        {
            ContentId = ToolContentId;
        }

        #endregion

        #region Logic

        private void FindRelations()
        {
            if (!(FeatureType is FeatureType)) return;
            if (!(FeatureType2 is FeatureType)) return;

            var ft1 = (FeatureType) FeatureType;
            var ft2 = (FeatureType)FeatureType2;


            var allReferences = new List<List<FeatureType>>();
            var accountedReferences = new HashSet<FeatureType>();
            var currentTypeList = new List<FeatureType> { ft1 };

            allReferences.Add(currentTypeList);
            accountedReferences.Add(ft1);

            while (true)
            {
                //form next level references
                var references = new List<FeatureType>();

                //load all links from current level
                foreach (var featureType in currentTypeList)
                {
                    var referencesSubList = RelationUtil.MayRefereFrom(featureType).
                        Union(RelationUtil.MayRefereTo(featureType)).
                        Except(accountedReferences).ToList();
                    //take into account
                    foreach (var ft in referencesSubList)
                    {
                        references.Add(ft);
                        accountedReferences.Add(ft);
                    }
                }
                //here references contains all next level feature types
                if (references.Count == 0) break;
                allReferences.Add(references);
                if (references.Contains(ft2)) break;

                currentTypeList = new List<FeatureType>(references);
            }

            _graph = new PocGraph(true);

            if (allReferences.Count>0 && allReferences.Last().Contains(ft2))
            {
                //found

                allReferences.Last().Clear();
                allReferences.Last().Add(ft2);

                //find way back
                for (var i = allReferences.Count - 1; i >= 1; i--)
                {
                    var references = new List<FeatureType>();
                    //load all links from current level
                    foreach (var featureType in allReferences[i])
                    {
                        references.AddRange(RelationUtil.MayRefereFrom(featureType).
                            Union(RelationUtil.MayRefereTo(featureType)));
                    }

                    allReferences[i - 1] = allReferences[i - 1].Intersect(references).ToList();


                    if (allReferences[i - 1].Count == 0)
                    {
                        throw new Exception("Logic exception");
                    }
                }

                //add verteces
                var vertexList = new List<PocVertex>();
                foreach (var references in allReferences)
                {
                    foreach (var featureType in references)
                    {
                        var vertex = new PocVertex
                                         {
                                             FeatureType = featureType,
                                             ID = featureType.ToString()
                                         };
                        Graph.AddVertex(vertex);
                        vertexList.Add(vertex);
                    }
                }
                
                //add edges
                for (int index = 0; index < allReferences.Count-1; index++)
                {
                    var references = allReferences[index];
                    var references2 = allReferences[index+1];

                    var list = new List<PocVertex>(); 
                    foreach (var featureType in references)
                    {
                        foreach (var featureType2 in references2)
                        {
                            var from = vertexList.Where(t => t.FeatureType == featureType).FirstOrDefault();
                            var to = vertexList.Where(t => t.FeatureType == featureType2).FirstOrDefault();

                            if (from==null) continue;
                            if (to == null) continue;

                            var directLink = RelationUtil.MayRefereFrom(featureType).Contains(featureType2);
                            if (directLink)
                            {
                                var path=RelationUtil.GetConnectionProperty(featureType, featureType2);
                                var edgeString = string.Format("{0} - {1} connected by the following ", from.ID, to.ID);
                                var newEdge = new PocEdge(edgeString, from, to);
                                foreach (var connection in path.Split('\n'))
                                {
                                    newEdge.ConnectionList.Add(connection);
                                }

                                newEdge.ID += newEdge.ConnectionList.Count > 1 ? "properties:" : "property:";

                                Graph.AddEdge(newEdge);
                            }
                            else
                            {
                                var path = RelationUtil.GetConnectionProperty(featureType2, featureType);

                                if (path==null) continue;
                               

                                var edgeString = string.Format("{0} - {1} connected by the following ", to.ID, from.ID);
                                var newEdge = new PocEdge(edgeString, to, from);
                                foreach(var connection in path.Split('\n'))
                                {
                                    newEdge.ConnectionList.Add(connection);
                                }

                                newEdge.ID += newEdge.ConnectionList.Count > 1 ? "properties:" : "property:";

                                Graph.AddEdge(newEdge);
                            }
                           
                        }
                    }
                   
                }
            }
            else
            {
                //not found

                //do nothing?   
            }

            OnPropertyChanged("Graph");
             
        }

        #endregion


        #region FeatureList

        private BlockerModel _blockerModel;
        public BlockerModel BlockerModel
        {
            get => _blockerModel ?? (_blockerModel = new BlockerModel { ActivatingObject = this });
            set => _blockerModel = value;
        }

        private object _featureType;
        public object FeatureType
        {
            get => _featureType;
            set
            {
                _featureType = value;
                BlockerModel.BlockForAction(FindRelations);
            }
        }

        #endregion

        #region FeatureList2

        private object _featureType2;
        public object FeatureType2
        {
            get => _featureType2;
            set
            {
                _featureType2 = value;
                BlockerModel.BlockForAction(FindRelations);
            }
        }

      
        #endregion

        //-----------

        #region Data

        private string _layoutAlgorithmType = _layoutAlgorithmTypes[3];
        private PocGraph _graph = new PocGraph(true);
        private static readonly List<String> _layoutAlgorithmTypes = new List<string>
                                                                         {
                                                                       "BoundedFR",
                                                                       "Circular",
                                                                       "CompoundFDP",
                                                                       "EfficientSugiyama",
                                                                       "FR",
                                                                       "ISOM",
                                                                       "KK",
                                                                       "LinLog",
                                                                       "Tree",
                                                                  };

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
