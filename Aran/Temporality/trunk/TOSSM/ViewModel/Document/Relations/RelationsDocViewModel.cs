using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Aran.Aim;
using Aran.Aim.Utilities;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.View;
using MvvmCore;
using TOSSM.Converter;
using TOSSM.Util;
using TOSSM.Util.Wrapper;
using TOSSM.ViewModel.Document.Relations.Util;
using TOSSM.ViewModel.Pane;
using TOSSM.ViewModel.Pane.Base;

namespace TOSSM.ViewModel.Document.Relations
{
    public class RelationsDocViewModel : DocViewModel, IPresenterParent
    {
        public static string ToolContentId = "Feature Relations";

        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/file_link.png", UriKind.RelativeOrAbsolute);

        public DataPresenterModel DataPresenter { get; private set; }

        #region Load


        

        private void LoadRelations()
        {
            //get references
            var featurePropList = new List<RefFeatureProp>();
            AimMetadataUtility.GetReferencesFeatures(EditedFeature.Feature, featurePropList);

            var mayRefereFromTypes = RelationUtil.MayRefereFrom(EditedFeature.FeatureType);

            var relationList = new List<SingleTypeRelationViewModel>();

            var missedRelationList = mayRefereFromTypes.Select(featureType => new SingleTypeRelationViewModel
            {
                FeatureType = featureType, Direction = RelationDirection.MissedDirect, IsExpanded = false
            }).ToList();


            foreach (var featureProp in featurePropList)
            {
                var linkedFeature = CommonDataProvider.GetState(featureProp.FeatureType, featureProp.RefIdentifier, AiracDate);

                if (linkedFeature == null)
                {
                    MessageBoxHelper.Show(
                        "Link to " + featureProp.FeatureType + " with identifier " + featureProp.RefIdentifier + " can not be loaded.",
                        "Can not load link", MessageBoxButton.OK, MessageBoxImage.Error);
                    continue;
                }

                missedRelationList.RemoveAll(t => t.FeatureType == linkedFeature.FeatureType);

                var relation = relationList.FirstOrDefault(t => t.FeatureType == linkedFeature.FeatureType && 
                                                                t.Direction==RelationDirection.Direct);

                var wrapper = new ReadonlyFeatureWrapper(linkedFeature);
                wrapper.SetProperty("PropertyPath", string.Join(".",featureProp.PropInfoList.Select(t=>t.Name)));

                if (relation==null)
                {
                    relationList.Add(new SingleTypeRelationViewModel
                    {
                        FeatureType = linkedFeature.FeatureType,
                                                                        Direction = RelationDirection.Direct,
                        Items = { wrapper }
                    });
                }
                else
                {
                    relation.Items.Add(wrapper);
                }
            }

            relationList.AddRange(missedRelationList);


            var mayRefereToTypes = RelationUtil.MayRefereTo(EditedFeature.FeatureType);

            foreach (var featureType in mayRefereToTypes)
            {
                MainManagerModel.Instance.StatusText = "Searching reverse links from " + featureType + "...";

                var data = DataProvider.GetReverseLinksTo(EditedFeature, featureType, AiracDate);

                var relation = new SingleTypeRelationViewModel
                                   {
                                       FeatureType = featureType,
                                       Direction = RelationDirection.Reverse
                                   };

                if (data != null && data.Count>0)
                {
                    foreach (var feature in data)
                    {
                        var reverseFeaturePropList = new List<RefFeatureProp>();
                        AimMetadataUtility.GetReferencesFeatures(feature.Feature, reverseFeaturePropList);

                        var relatedPaths=reverseFeaturePropList.Where(t => t.RefIdentifier == EditedFeature.Identifier).ToList();

                        foreach (var path in relatedPaths)
                        {
                            var reverseWrapper = new ReadonlyFeatureWrapper(feature);
                            reverseWrapper.SetProperty("PropertyPath", string.Join(".", path.PropInfoList.Select(t=>t.Name)));
                            relation.Items.Add(reverseWrapper);
                        }
                    }
                    relationList.Add(relation);
                }
                else
                {
                    relation = new SingleTypeRelationViewModel
                    {
                        FeatureType = featureType,
                        Direction = RelationDirection.MissedReverse,
                        IsExpanded = false
                    };
                    relationList.Add(relation);
                }
            }


            relationList = relationList.OrderBy(t=>t.Direction).ThenByDescending(t => t.Items.Count).
                ThenBy(t=>t.FeatureType.ToString()).ToList();

            foreach (var model in relationList)
            {
                RelationsData.Add(model);
            }
            

            MainManagerModel.Instance.StatusText = "Done";
        }


        public AimFeature EditedFeature { get; set; }

        public override void Load()
        {
            if (IsLoaded) return;
            IsLoaded = true;

            DataPresenter.BlockerModel.BlockForAction(
                    () =>
                    {

                        EditedFeature = DataProvider.GetState(FeatureType, FeatureIdentifier, AiracDate);
                        Title = "Relations of " + EditedFeature.Feature.GetType().Name + " " +
                            HumanReadableConverter.ShortAimDescription(EditedFeature.Feature) + 
                            AiracSelectorViewModel.AiracMessage(AiracDate);

                        DataPresenter.EffectiveDate = AiracDate;
                        LoadRelations();

                        if (RelationsData.Count > 0)
                        {
                            RelationsDataGridVisibility = Visibility.Visible;
                            SelectedRelation = RelationsData.First();
                        }
                        else
                        {
                            RelationsDataGridVisibility = Visibility.Hidden;
                        }

                       
                    });
        }

        #endregion

        public RelationsDocViewModel(FeatureType type, Guid id, DateTime date)
            : base(type, id, date)
        {
            ContentId = ToolContentId;
            DataPresenter = new DataPresenterModel { ViewModel = this };
            DataPresenter.AddPredefinedColumn("Property Path","PropertyPath");
        }

        #region Relations UI

        private SingleTypeRelationViewModel _selectedRelation;
        public SingleTypeRelationViewModel SelectedRelation
        {
            get => _selectedRelation;
            set
            {
                _selectedRelation = value;
                OnPropertyChanged("SelectedRelation");

                if (SelectedRelation == null) return;

                DataPresenter.FeatureType = SelectedRelation.FeatureType;
            }
        }

        private ICollectionView _relationsDataFiltered;
        public ICollectionView RelationsDataFiltered
        {
            get
            {
                if (_relationsDataFiltered == null)
                {
                    _relationsDataFiltered = CollectionViewSource.GetDefaultView(RelationsData);
                    _relationsDataFiltered.GroupDescriptions.Clear();
                    _relationsDataFiltered.GroupDescriptions.Add(new PropertyGroupDescription("Direction"));
                }
                return _relationsDataFiltered;
            }
            set => _relationsDataFiltered = value;
        }

        private MtObservableCollection<SingleTypeRelationViewModel> _relationsData;
        public MtObservableCollection<SingleTypeRelationViewModel> RelationsData
        {
            get => _relationsData ?? (_relationsData = new MtObservableCollection<SingleTypeRelationViewModel>());
            set
            {

                _relationsData = value;
                OnPropertyChanged("RelationsData");
            }
        }

        private Visibility _relationsDataGridVisibility;
        public Visibility RelationsDataGridVisibility
        {
            get => _relationsDataGridVisibility;
            set
            {
                _relationsDataGridVisibility = value;
                OnPropertyChanged("RelationsDataGridVisibility");
            }
        }

        #endregion

        #region Commands

        private RelayCommand _unlinkCommand;
        public RelayCommand UnlinkCommand
        {
            get
            {
                return _unlinkCommand ?? (_unlinkCommand = new RelayCommand(
                t =>
                {
                    throw new NotImplementedException();
                },
                delegate
                    {
                        if (DataPresenter.SelectedFeature == null) return false;
                        if (SelectedRelation == null) return false;
                        if (SelectedRelation.Direction != RelationDirection.Reverse) return false;
                        if (CurrentDataContext.CurrentUser == null) return false;
                        if (CurrentDataContext.CurrentUser.ActivePrivateSlot == null) return false;

                        return CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.EffectiveDate == AiracDate;
              }));
            }
            set => _unlinkCommand = value;
        }

        #endregion

        #region Implementation of IPresenterParent

        public override void OnClosed()
        {
            base.OnClosed();
            DataPresenter.IsTerminated = true;
        }

        protected override void OnDispose()
        {
            DataPresenter.IsTerminated = true;
        }


        public void ReloadData(DataPresenterModel model)
        {
            if (SelectedRelation==null)
            {
                DataPresenter.FeatureData=new List<object>();
                DataPresenter.UpdateFeatureDataFiltered();
                return;
            }

            DataPresenter.FeatureData=new List<object>(SelectedRelation.Items);
            DataPresenter.UpdateFeatureDataFiltered();
        }

        #endregion

        #region Selection

        private ReadonlyFeatureWrapper SelectedWrapper()
        {
            var selected = DataPresenter.SelectedFeature;

            if (selected == null) return null;

            if (selected is ReadonlyFeatureWrapper)
            {
                return selected as ReadonlyFeatureWrapper;
            }

            return null;
        }

        private string _selectedCellColumnHeader;
        public string SelectedCellColumnHeader
        {
            get => _selectedCellColumnHeader;
            set
            {
                if (_selectedCellColumnHeader == value) return;
                _selectedCellColumnHeader = value;
            }
        }

        #endregion
    }
}
