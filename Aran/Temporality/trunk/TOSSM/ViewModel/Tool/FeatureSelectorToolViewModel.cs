using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Aran.Aim;
using Aran.Temporality.CommonUtil.Control;
using Aran.Temporality.CommonUtil.Control.TreeViewDragDrop;
using MvvmCore;
using TOSSM.ViewModel.Pane.Base;

namespace TOSSM.ViewModel.Tool
{
    public class FeatureSelectorToolViewModel : ToolViewModel, ISelectedItemHolder
    {
        public static string ToolContentId = "Feature Selector";

        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/feature.png", UriKind.RelativeOrAbsolute);

        private void Init()
        {
            ContentId = ToolContentId;

            MainManagerModel.Instance.FeatureSelectorToolViewModel = this;

            FirstGeneration = new List<FeatureTreeViewItemViewModel>();

            foreach (FeatureType ft in Enum.GetValues(typeof(FeatureType)))
            {
                FirstGeneration.Add(new FeatureTreeViewItemViewModel
                                        {
                                            FeatureType = ft,
                                            SelectedItemHolder = this
                                        });
            }

            //IsVisible = false;
        }

        public FeatureSelectorToolViewModel()
            : base(ToolContentId)
        {
            Init();
        }


        #region Feature filter

        private string _featureFilter;
        public string FeatureFilter
        {
            get => _featureFilter;
            set
            {
                _featureFilter = value;
                OnPropertyChanged("FeatureFilter");
                OnPropertyChanged("FeatureFilterEmptyButtonVisibility");
                OnPropertyChanged("FilteredFeatureList");
                SelectedFeature = SelectedFeature;
            }
        }

        public Visibility FeatureFilterEmptyButtonVisibility => String.IsNullOrEmpty(FeatureFilter) ? Visibility.Collapsed : Visibility.Visible;

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
                }
                return _featureList;
            }
            set
            {
                _featureList = value;
                OnPropertyChanged("FilteredFeatureList");
            }
        }

        public List<FeatureType> FilteredFeatureList
        {
            get
            {
                if (String.IsNullOrEmpty(FeatureFilter))
                    return new List<FeatureType>(FeatureList.
                        OrderBy(t => t.ToString()));

                return new List<FeatureType>(
                    FeatureList.Where(t =>
                    t.ToString().ToLower().StartsWith(FeatureFilter.ToLower())).
                    OrderBy(t => t.ToString()));
            }
        }

        private RelayCommand _onClearFeatureFilter;
        public RelayCommand OnClearFeatureFilter
        {
            get
            {
                return _onClearFeatureFilter ??
                       (_onClearFeatureFilter = new RelayCommand(t1 => FeatureFilter = "", t2 => !String.IsNullOrEmpty(FeatureFilter)));
            }
            set
            {
                _onClearFeatureFilter = value;
                OnPropertyChanged("OnClearFeatureFilter");
            }
        }

        private object _selectedFeature;
        public object SelectedFeature
        {
            get => _selectedFeature;
            set
            {
                _selectedFeature = value;
                OnPropertyChanged("SelectedFeature");
            }
        }

        private List<FeatureTreeViewItemViewModel> _firstGeneration;
        public List<FeatureTreeViewItemViewModel> FirstGeneration
        {
            get => _firstGeneration;
            set
            {
                _firstGeneration = value;
                OnPropertyChanged("FirstGeneration");
            }
        }

        #endregion


        //public override Uri IconSource
        //{
        //    get
        //    {
        //        return new Uri("pack://application:,,,/Edi;component/Images/property-blue.png", UriKind.RelativeOrAbsolute);
        //    }
        //}

        #region Implementation of ISelectedItemHolder

        private object _selectedObject;
        public object SelectedObject
        {
            get => _selectedObject;
            set => _selectedObject = value;
        }

        

        private RelayCommand _onAdd;
        public RelayCommand OnAdd
        {
            get
            {
                return _onAdd ?? (_onAdd = new RelayCommand(t =>
                {
                    var name = "test";
                    var newModel = new FeatureTreeViewItemViewModel
                                        {
                                            Name = name,
                                            SelectedItemHolder = this,
                                        };
                    PerformDragDrop(newModel, SelectedObject as FeatureTreeViewItemViewModel);
                }));
            }
            set => _onAdd = value;
        }

        public RelayCommand OnRename { get; set; }

        public RelayCommand OnDelete { get; set; }

        #endregion

        public void PerformDragDrop(FeatureTreeViewItemViewModel sourceModel, FeatureTreeViewItemViewModel targetModel)
        {
            if (sourceModel==null) return;

            //remove source from parent
            if (sourceModel.Parent!=null)
            {
                sourceModel.Parent.Children.Remove(sourceModel);
            }
            else
            {
                FirstGeneration.Remove(sourceModel);
            }

            if (targetModel==null)
            {
                sourceModel.Parent = null;
                FirstGeneration.Add(sourceModel);
            }
            else if (targetModel.FeatureType==null)//target is folder
            {
                //set new parent
                sourceModel.Parent = targetModel;
                //add to parent 
                sourceModel.Parent.Children.Add(sourceModel);
            }
            else//target is file
            {
                if (targetModel.Parent!=null)//find appropriate folder
                {
                    //set new parent
                    sourceModel.Parent = targetModel.Parent;
                    //add to parent 
                    var index=targetModel.Parent.Children.IndexOf(targetModel);
                    targetModel.Parent.Children.Insert(index,sourceModel);
                }
                else //it is root 
                {
                    sourceModel.Parent = null;
                    //add to parent 
                    var index = FirstGeneration.IndexOf(targetModel);
                    FirstGeneration.Insert(index, sourceModel);
                }
            }

            OnPropertyChanged("FirstGeneration");
        }
    }
}
