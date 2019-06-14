using System;
using System.Collections.Generic;
using System.Linq;
using ADM.Graph;
using Aran.Aim;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.ViewModel;
using MvvmCore;

namespace ADM.ViewModel.Tool
{
    public class RelationFinderToolViewModel : ViewModelBase
    {
        #region Ctor

        public static string ToolContentId = "Relation Finder";

        public Uri IconSource
        {
            get
            {
                return new Uri("pack://application:,,,/Resources/Images/direction.png", UriKind.RelativeOrAbsolute);
            }
        }

        public RelationFinderToolViewModel() 
        {
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
            get { return _blockerModel??(_blockerModel=new BlockerModel()); }
            set { _blockerModel = value; }
        }

        private object _featureType;
        public object FeatureType
        {
            get { return _featureType; }
            set
            {
                _featureType = value;
                BlockerModel.BlockForAction(FindRelations);
            }
        }

        private IList<FeatureType> _featureListFiltered;
        public IList<FeatureType> FeatureListFiltered
        {
            get
            {
                if (_featureListFiltered == null)
                {
                    _featureListFiltered = new List<FeatureType>(FeatureList);
                }
                return _featureListFiltered;
            }
            set
            {
                _featureListFiltered = value;
                OnPropertyChanged("FeatureListFiltered");
            }
        }

        private string _featureTypeFilter;
        public string FeatureTypeFilter
        {
            get { return _featureTypeFilter; }
            set
            {
                _featureTypeFilter = value;
                OnPropertyChanged("FeatureTypeFilter");


                if (string.IsNullOrEmpty(FeatureTypeFilter))
                {
                    FeatureListFiltered = new List<FeatureType>(FeatureList);
                }
                else
                {
                    var result = new List<FeatureType>();
                    var result2 = new List<FeatureType>();
                    foreach (var featureType in FeatureList)
                    {
                        if (featureType.ToString().ToLower().Contains(FeatureTypeFilter.ToLower()))
                        {
                            result.Add(featureType);
                        }
                        else
                        {
                            result2.Add(featureType);
                        }
                    }

                    //result.AddRange(result2);
                    FeatureListFiltered = result;
                }

            }
        }

        private List<FeatureType> _featureList;
        public List<FeatureType> FeatureList
        {
            get
            {
                if (_featureList == null)
                {
                    _featureList = new List<FeatureType>();
                    foreach (FeatureType ft in Enum.GetValues(typeof(FeatureType)))
                    {
                        _featureList.Add(ft);
                    }
                    _featureList = new List<FeatureType>(_featureList.OrderBy(t => t.ToString()));
                }
                return _featureList;
            }
            set
            {
                _featureList = value;
                OnPropertyChanged("FilteredFeatureList");
            }
        }

        #endregion

        #region FeatureList2

        private object _featureType2;
        public object FeatureType2
        {
            get { return _featureType2; }
            set
            {
                _featureType2 = value;
                BlockerModel.BlockForAction(FindRelations);
            }
        }

        private IList<FeatureType> _featureListFiltered2;
        public IList<FeatureType> FeatureListFiltered2
        {
            get
            {
                if (_featureListFiltered2 == null)
                {
                    _featureListFiltered2 = new List<FeatureType>(FeatureList2);
                }
                return _featureListFiltered2;
            }
            set
            {
                _featureListFiltered2 = value;
                OnPropertyChanged("FeatureListFiltered2");
            }
        }

        private string _featureTypeFilter2;
        public string FeatureTypeFilter2
        {
            get { return _featureTypeFilter2; }
            set
            {
                _featureTypeFilter2 = value;
                OnPropertyChanged("FeatureTypeFilter2");


                if (string.IsNullOrEmpty(FeatureTypeFilter2))
                {
                    FeatureListFiltered2 = new List<FeatureType>(FeatureList2);
                }
                else
                {
                    var result = new List<FeatureType>();
                    var result2 = new List<FeatureType>();
                    foreach (var featureType in FeatureList2)
                    {
                        if (featureType.ToString().ToLower().Contains(FeatureTypeFilter2.ToLower()))
                        {
                            result.Add(featureType);
                        }
                        else
                        {
                            result2.Add(featureType);
                        }
                    }

                    //result.AddRange(result2);
                    FeatureListFiltered2 = result;
                }

            }
        }

        private List<FeatureType> _featureList2;
        public List<FeatureType> FeatureList2
        {
            get
            {
                if (_featureList2 == null)
                {
                    _featureList2 = new List<FeatureType>();
                    foreach (FeatureType ft in Enum.GetValues(typeof(FeatureType)))
                    {
                        _featureList2.Add(ft);
                    }
                    _featureList2 = new List<FeatureType>(_featureList2.OrderBy(t => t.ToString()));
                }
                return _featureList2;
            }
            set
            {
                _featureList2 = value;
                OnPropertyChanged("FeatureList2");
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

        public List<String> LayoutAlgorithmTypes
        {
            get { return _layoutAlgorithmTypes; }
        }

        public string LayoutAlgorithmType
        {
            get { return _layoutAlgorithmType; }
            set
            {
                _layoutAlgorithmType = value;
                OnPropertyChanged("LayoutAlgorithmType");
            }
        }

        public PocGraph Graph
        {
            get { return _graph; }
            set
            {
                _graph = value;
                OnPropertyChanged("Graph");
            }
        }

        #endregion
    }
}
