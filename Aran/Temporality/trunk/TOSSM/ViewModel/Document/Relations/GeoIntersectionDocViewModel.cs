using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using Aran.Aim;
using Aran.Temporality.Common.Aim.MetaData;
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
    public class GeoIntersectionDocViewModel : DocViewModel, IPresenterParent
    {
         public static string ToolContentId = "Feature Geometry Intersections";

        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/ungroup.png", UriKind.RelativeOrAbsolute);

        public DataPresenterModel DataPresenter { get; set; }

        #region Load


        private void LoadRelations()
        {
            foreach (FeatureType ft in Enum.GetValues(typeof(FeatureType)))
            {
                MainManagerModel.Instance.StatusText = "Searching intersections with "+ft.ToString()+"...";
                var data=DataProvider.GetDataForGeoIntersections(EditedFeature, ft, AiracDate);

                if (data == null || data.Count <= 0) continue;

                var relation=new SingleTypeRelationViewModel{FeatureType = ft};

                foreach (var feature in data)
                {
                    relation.Items.Add(new ReadonlyFeatureWrapper(feature));
                }


                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                                                  (Action)(() => RelationsData.Add(relation)));
                
            }

            MainManagerModel.Instance.StatusText = "Done";
        }


        private AimFeature EditedFeature { get; set; }



        public override void Load()
        {
            if (IsLoaded) return;

            DataPresenter.BlockerModel.BlockForAction(
                    () =>
                    {
                        EditedFeature = DataProvider.GetState(FeatureType, FeatureIdentifier, AiracDate);
                        Title = "Features having geometry intersection with " + EditedFeature.Feature.GetType().Name + " " + 
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

                        IsLoaded = true;
                    });
        }

        #endregion

        public GeoIntersectionDocViewModel(FeatureType type, Guid id, DateTime date)
            : base(type, id, date)
        {
            ContentId = ToolContentId;
            DataPresenter = new DataPresenterModel { ViewModel = this };
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

        private RelayCommand _onMapCommand;
        public RelayCommand OnMapCommand
        {
            get
            {
                return _onMapCommand ?? (_onMapCommand = new RelayCommand(
                t =>
                {
                    var list = SelectedRelation.Items.Cast<ReadonlyFeatureWrapper>().Select(w => w.Feature).ToList();
                    if (list.Count>0)
                    {
                        
                    }
                },
                t =>SelectedRelation != null));
            }
            set => _onMapCommand = value;
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
