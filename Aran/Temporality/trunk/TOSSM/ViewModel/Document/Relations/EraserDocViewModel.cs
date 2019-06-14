using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using Aran.Aim;
using Aran.Aim.Utilities;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Id;
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
    public class EraserDocViewModel : DocViewModel, IPresenterParent
    {
        public static string ToolContentId = "Feature Eraser";

        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/delete_red.png", UriKind.RelativeOrAbsolute);

        public DataPresenterModel DataPresenter { get; set; }


        public List<Guid> BatchGuids { get; set; }


        #region Load

        private AimFeature _editedFeature;




        private void UpdateFeature()
        {
            _editedFeature = DataProvider.GetState(FeatureType, FeatureIdentifier, AiracDate);

            Title = "Decommissioning of " + (FeatureType)FeatureType + " "
                + HumanReadableConverter.ShortAimDescription(_editedFeature.Feature) + 
                AiracSelectorViewModel.AiracMessage(AiracDate);

            AddFeatureToBeRemoved(_editedFeature);

            if (RelationsData.Count > 0)
            {
                RelationsDataGridVisibility = Visibility.Visible;
                SelectedRelation = RelationsData.First();
            }
            else
            {
                RelationsDataGridVisibility = Visibility.Hidden;
            }
        }

        public override void Load()
        {
            if (IsLoaded) return;

            DataPresenter.BlockerModel.BlockForAction(
                    () =>
                    {
                        UpdateFeature();
                        IsLoaded = true;
                    });
        }

        #endregion

        public EraserDocViewModel(FeatureType type, Guid id, DateTime date) : base (type, id, date)
        {
            ContentId = ToolContentId;
            DataPresenter = new DataPresenterModel { ViewModel = this };
        }

        #region Implementation of IPresenterParent

        public override void OnClosed()
        {
            base.OnClosed();
            DataPresenter.IsTerminated = true;
        }

        public void ReloadData(DataPresenterModel model)
        {
            if (SelectedRelation == null)
            {
                DataPresenter.FeatureData = new List<object>();
                DataPresenter.UpdateFeatureDataFiltered();
                return;
            }

            DataPresenter.FeatureData = new List<object>(SelectedRelation.Items);
            DataPresenter.UpdateFeatureDataFiltered();
        }

        protected override void OnDispose()
        {
            DataPresenter.IsTerminated = true;
        }


        #endregion

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
                    _relationsDataFiltered.GroupDescriptions.Add(new PropertyGroupDescription("PurposeDescription"));
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

        #region Relations logic

        readonly HashSet<Guid> _guidsToBeRemoved=new HashSet<Guid>();
        readonly HashSet<Guid> _guidsToBeMarked = new HashSet<Guid>();
        readonly HashSet<Guid> _guidsToStayIntact = new HashSet<Guid>();

       private void AddFeatureToBeProcessed(AimFeature feature)
       {
           if (feature == null) return;
           if (_guidsToBeMarked.Contains(feature.Identifier)) return;
           if (_guidsToStayIntact.Contains(feature.Identifier)) return;
           if (_guidsToBeRemoved.Contains(feature.Identifier)) return;


           //_guidsToStayIntact.Remove(feature.Identifier);
           //_guidsToBeRemoved.Remove(feature.Identifier);

           _guidsToBeMarked.Add(feature.Identifier);

           var relationModel = RelationsData.Where(t => t.Purpose == RelationPurpose.NeedToBeMarked && t.FeatureType == feature.FeatureType).
                FirstOrDefault();
           if (relationModel == null)
           {
               RelationsData.Add(new SingleTypeRelationViewModel
               {
                   FeatureType = feature.FeatureType,
                   Purpose = RelationPurpose.NeedToBeMarked,
                   Items = { new ReadonlyFeatureWrapper(feature) }
               });
           }
           else
           {
               relationModel.Items.Add(new ReadonlyFeatureWrapper(feature));
           }
       }

       private void AddFeatureToStayIntact(AimFeature feature)
       {
           if (feature == null) return;
           if (_guidsToStayIntact.Contains(feature.Identifier)) return;

           _guidsToBeMarked.Remove(feature.Identifier);
           _guidsToBeRemoved.Remove(feature.Identifier);


           _guidsToStayIntact.Add(feature.Identifier);

           var relationModel = RelationsData.Where(t => t.Purpose == RelationPurpose.StaysIntact && t.FeatureType == feature.FeatureType).
                FirstOrDefault();
           if (relationModel == null)
           {
               RelationsData.Add(new SingleTypeRelationViewModel
               {
                   FeatureType = feature.FeatureType,
                   Purpose = RelationPurpose.StaysIntact,
                   Items = { new ReadonlyFeatureWrapper(feature) }
               });
           }
           else
           {
               relationModel.Items.Add(new ReadonlyFeatureWrapper(feature));
           }
       }

        private void AddFeatureToBeRemoved(AimFeature feature)
        {
            if (feature==null) return;
            if (_guidsToBeRemoved.Contains(feature.Identifier)) return;


            _guidsToStayIntact.Remove(feature.Identifier);
            _guidsToBeMarked.Remove(feature.Identifier);

            _guidsToBeRemoved.Add(feature.Identifier);

            var relationModel=RelationsData.Where(t => t.Purpose == RelationPurpose.ToBeDeleted && t.FeatureType == feature.FeatureType).
                FirstOrDefault();
            if (relationModel==null)
            {
                RelationsData.Add(new SingleTypeRelationViewModel
                                    { FeatureType = feature.FeatureType, 
                                        Purpose = RelationPurpose.ToBeDeleted, 
                                        Items = {new ReadonlyFeatureWrapper(feature)}});
            }
            else
            {
                relationModel.Items.Add(new ReadonlyFeatureWrapper(feature));
            }

            //get references
            var featurePropList = new List<RefFeatureProp>();
            AimMetadataUtility.GetReferencesFeatures(feature.Feature, featurePropList);

            foreach (var featureProp in featurePropList)
            {
                AimFeature linkedFeature=DataProvider.GetState(featureProp.FeatureType,featureProp.RefIdentifier,AiracDate);
                if (linkedFeature!=null)
                {
                    AddFeatureToBeProcessed(linkedFeature);
                }
            }

        }


        #endregion

        #region Commands

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(
                t =>
                {

                    if (MessageBoxHelper.Show("Are you sure you want to commit changes?",
                        "Committing data", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                        == MessageBoxResult.Yes)
                    {
                        DataPresenter.BlockerModel.BlockForAction(
                            ()=>
                        {
                            foreach (var relationViewModel in RelationsData.Where(tt => tt.Purpose == RelationPurpose.ToBeDeleted))
                            {
                                foreach (ReadonlyFeatureWrapper item in relationViewModel.Items)
                                {
                                    var featureType = item.Feature.Feature.FeatureType;
                                    var guid = item.Feature.Feature.Identifier;
                                    CurrentDataContext.CurrentService.Decommission(
                                        new FeatureId { FeatureTypeId = (int)featureType, Guid = guid }, AiracDate);
                                }
                            }

                          


                            Application.Current.Dispatcher.Invoke(
                                DispatcherPriority.Normal,
                                (Action) (
                                    () =>
                                    {
                                        IsClosed = true;
                                        MainManagerModel.Instance.ActiveDocument = null;
                                        MainManagerModel.Instance.Documents.Remove(this);




                                        MainManagerModel.Instance.View(_editedFeature.Feature, AiracDate);
                                    }));
                        
                        });
                    }
                },
                t => !RelationsData.Any(d=>d.Purpose==RelationPurpose.NeedToBeMarked)));
            }
            set => _saveCommand = value;
        }

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new RelayCommand(
                t => DataPresenter.BlockerModel.BlockForAction(()=>
                {
                    RelationsData.Clear();
                    _guidsToBeRemoved.Clear();
                    _guidsToBeMarked.Clear();
                    _guidsToStayIntact.Clear();
                    UpdateFeature();
                })));
            }
            set => _cancelCommand = value;
        }

        private RelayCommand _markStayIntactCommand;
        public RelayCommand MarkStayIntactCommand
        {
            get
            {
                return _markStayIntactCommand ?? (_markStayIntactCommand = new RelayCommand(
                t =>
                {
                    if (SelectedRelation == null) return;
                    if (DataPresenter.SelectedFeature == null) return;
                    if (DataPresenter.SelectedFeatures == null) return;

                    foreach (var wrapper in SelectedFeatures())
                    {
                        AddFeatureToStayIntact(wrapper.Feature);    
                    }


                    foreach (var obj in DataPresenter.SelectedFeatures)
                    {
                        SelectedRelation.Items.Remove(obj);
                    }
                    
                    if (SelectedRelation.Items.Count==0)
                    {
                        RelationsData.Remove(SelectedRelation);
                        SelectedRelation = null;
                    }

                },
                t => DataPresenter.SelectedFeature != null && SelectedRelation != null && SelectedRelation.Purpose == RelationPurpose.NeedToBeMarked));
            }
            set => _markStayIntactCommand = value;
        }

        private RelayCommand _markToBeDeletedCommand;
        public RelayCommand MarkToBeDeletedCommand
        {
            get
            {
                return _markToBeDeletedCommand ?? (_markToBeDeletedCommand = new RelayCommand(
                t =>
                {
                    if (SelectedRelation==null) return;
                    if (DataPresenter.SelectedFeature == null) return;
                    if (DataPresenter.SelectedFeatures == null) return;

                    foreach (var wrapper in SelectedFeatures())
                    {
                        AddFeatureToBeRemoved(wrapper.Feature);
                    }

                    foreach (var obj in DataPresenter.SelectedFeatures)
                    {
                        SelectedRelation.Items.Remove(obj);
                    }

                    if (SelectedRelation.Items.Count == 0)
                    {
                        RelationsData.Remove(SelectedRelation);
                        SelectedRelation = null;
                    }
                },
                t => DataPresenter.SelectedFeature != null && SelectedRelation != null && SelectedRelation.Purpose!=RelationPurpose.ToBeDeleted));
            }
            set => _markToBeDeletedCommand = value;
        }

        #endregion

        #region Selection


        private List<ReadonlyFeatureWrapper> SelectedFeatures()
        {
            var selected = DataPresenter.SelectedFeatures;
            if (selected == null || selected.Count==0 ) return new List<ReadonlyFeatureWrapper>();
            return selected.OfType<ReadonlyFeatureWrapper>().ToList();
        }


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
