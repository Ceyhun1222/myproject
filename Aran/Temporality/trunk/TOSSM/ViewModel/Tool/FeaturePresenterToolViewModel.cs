using System;
using System.Diagnostics;
using System.Windows;
using Aran.Aim;
using Aran.Temporality.CommonUtil.Context;
using MvvmCore;
using TOSSM.Util;
using TOSSM.Util.Wrapper;
using TOSSM.ViewModel.Control.AnnotationLanguage;
using TOSSM.ViewModel.Control.DatabaseDate;
using TOSSM.ViewModel.Control.GuidControl;
using TOSSM.ViewModel.Control.Interpretation;
using TOSSM.ViewModel.Control.OriginatorFilter;
using TOSSM.ViewModel.Pane;
using TOSSM.ViewModel.Pane.Base;



namespace TOSSM.ViewModel.Tool
{



    public class FeaturePresenterToolViewModel : ToolViewModel, IPresenterParent
    {
        public DataPresenterModel DataPresenter { get; private set; }

        public static string ToolContentId = "Feature Presenter";

        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/detail.png", UriKind.RelativeOrAbsolute);


        #region Ctor

        public FeaturePresenterToolViewModel(): base(ToolContentId)
        {
            ContentId = ToolContentId;
            MainManagerModel.Instance.FeaturePresenterToolViewModel = this;
            DataPresenter = new DataPresenterModel { ViewModel = this };
        }
       
        #endregion


        #region Selection

        private ReadonlyFeatureWrapper SelectedWrapper()
        {
            var selected = DataPresenter.SelectedFeature;

            if (selected == null) return null;

            return selected as ReadonlyFeatureWrapper;
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

        #region Implementation of IPresenterParent

        public override void OnClosed()
        {
            base.OnClosed();
            DataPresenter.IsTerminated = true;
        }

        private DateTime _airacDate;
        public DateTime AiracDate
        {
            get => _airacDate;
            set
            {
                _airacDate = value;
                OnPropertyChanged("AiracDate");
                DataPresenter.EffectiveDate = AiracDate;
                ReloadData();
            }
        }

        protected override void OnDispose()
        {
            DataPresenter.IsTerminated = true;
        }

        public void ReloadData(DataPresenterModel model = null)
        {
            if (DataPresenter.FeatureType ==null) return;
            
            Stopwatch s=new Stopwatch();
            s.Start();
            DataPresenter.FeatureData = DataProvider.GetDataForPresenter(
                (FeatureType)DataPresenter.FeatureType, 
                AiracDate,
                OriginatorFilterViewModel.SelectedOrigination,
                InterpretationViewModel.SelectedInterpretation,
                DatabaseDateViewModel.SelectedDatabaseState,
                DatabaseDateViewModel.SelectedDate,
                AnnotationLanguageViewModel.SelectedLanguage);
            DataPresenter.UpdateFeatureDataFiltered();
            s.Stop();
            MainManagerModel.Instance.StatusText = "Loaded " + DataPresenter.FeatureData.Count + " items in " +
                                                   s.ElapsedMilliseconds + " ms";
        }

        #endregion


        private void OnChangedOriginator(OriginatorFilterViewModel model)
        {
            if (model==null) return;

            ReloadData();
        }

        private OriginatorFilterViewModel _originatorFilterViewModel;
       
        public OriginatorFilterViewModel OriginatorFilterViewModel
        {
            get => _originatorFilterViewModel ?? (
                       _originatorFilterViewModel = new OriginatorFilterViewModel
                       {
                           OnChanged = OnChangedOriginator
                       });
            set => _originatorFilterViewModel = value;
        }

        private void OnChangedInterpretation(InterpretationViewModel model)
        {
            if (model == null) return;

            ReloadData();
        }
        private InterpretationViewModel _interpretationViewModel;
        private DatabaseDateViewModel _databaseDateViewModel;
        private RelayCommand _getTypeByGuidCommand;

        public InterpretationViewModel InterpretationViewModel
        {
            get => _interpretationViewModel ?? (_interpretationViewModel = new InterpretationViewModel
            {
                OnChanged = OnChangedInterpretation
            });
            set => _interpretationViewModel = value;
        }


        private void OnChangedAnnotationLanguage(AnnotationLanguageViewModel model)
        {
            if (model == null) return;

            ReloadData();
        }
        private AnnotationLanguageViewModel _annotationLanguageViewModel;

        public AnnotationLanguageViewModel AnnotationLanguageViewModel
        {
            get => _annotationLanguageViewModel ?? (_annotationLanguageViewModel = new AnnotationLanguageViewModel
            {
                OnChanged = OnChangedAnnotationLanguage
            });
            set => _annotationLanguageViewModel = value;
        }

        private void OnChangedDatabaseDate(DatabaseDateViewModel model)
        {
            if (model == null) return;

            ReloadData();
        }
        public DatabaseDateViewModel DatabaseDateViewModel
        {
            get => _databaseDateViewModel??(_databaseDateViewModel = new DatabaseDateViewModel
            {
                SelectedDatabaseStateIndex = 0,
                SelectedDate = DateTime.Now,
                OnChanged = OnChangedDatabaseDate
            });
            set => _databaseDateViewModel = value;
        }

        public void OnDoubleClick()
        {
            if (DataPresenter.SelectedFeature!=null)
            {
                MainManagerModel.Instance.View(DataPresenter.SelectedFeature, AiracDate);
            }
        }

        public RelayCommand GetTypeByGuidCommand
        {
            get
            {
                return _getTypeByGuidCommand??(_getTypeByGuidCommand=new RelayCommand(t =>
                {

                    DataPresenter.BlockerModel.BlockForAction(
                        () =>
                        {

                            GuidControlViewModel.ShowDialog(model =>
                            {
                                int featureType = CurrentDataContext.CurrentService.GetFeatureTypeById(model.GuidValue);
                                if (featureType > 0)
                                {
                                    DataPresenter.FeatureType = (FeatureType?)featureType;
                                    DataPresenter.PropertyFilter = model.GuidValue.ToString();
                                }
                                else
                                {
                                    MessageBoxHelper.Show("Feature Identifier not found", "Found Feature By Identifier", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                        );
                        });
                }));
            }
            set => _getTypeByGuidCommand = value;
        }
    }
}
