using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Util;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.View;
using MvvmCore;
using TOSSM.Converter;
using TOSSM.Util;
using TOSSM.ViewModel.Control.EventView;
using TOSSM.ViewModel.Document.Single.Editable;

namespace TOSSM.ViewModel.Document.Editor
{
    public class FeatureEditorDocViewModel : FeatureSubEditorDocViewModel
    {
        public override void OnAiracDateChanged()
        {
            base.OnAiracDateChanged();
            if (IsLoaded)
            {
                IsLoaded = false;
                Load();
            }
        }


        #region Load

        public override void Load()
        {
            if (IsLoaded) return;

            BlockerModel.BlockForAction(
                () =>
                {
                    var cfg = DataProvider.PublicationConfiguration;
                    if (cfg?.FeatureConfigurations != null && cfg.FeatureConfigurations.ContainsKey((int)FeatureType))
                    {
                        Configuration = cfg.FeatureConfigurations[(int)FeatureType];
                    }


                    switch (DocType)
                    {
                        case DocumentType.CreateNewFeature:
                            Feature newFeature;
                            if (BufferFeatureIdentifier == Guid.Empty)
                                newFeature = AimObjectFactory.CreateFeature(FeatureType);
                            else
                            {
                                var data = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId
                                {
                                    FeatureTypeId =
                                            (int)
                                            FeatureType,
                                    Guid =
                                            BufferFeatureIdentifier,
                                    WorkPackage = Workpackage
                                },
                                    false,
                                    BufferDateTime);
                                if (data == null || data.Count == 0 || data[0].Data == null ||
                                    data[0].Data.Feature == null)
                                {
                                    MessageBoxHelper.Show(
                                        "Sorry, there was error while requesting data. It is possible that you specified actual date when feature does not exist.",
                                        "Error while requesting data", MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                                    //close?
                                    newFeature = AimObjectFactory.CreateFeature(FeatureType);
                                }
                                else
                                {
                                    newFeature = data[0].Data.Feature;

                                }

                            }
                            //set new id
                            StartDateTime = AiracDate;
                            newFeature.Identifier = FeatureIdentifier;
                            newFeature.TimeSlice = new TimeSlice
                            {
                                ValidTime = new TimePeriod(AiracDate),
                                FeatureLifetime = new TimePeriod(AiracDate),
                                SequenceNumber = 1,
                                CorrectionNumber = 0,
                                Interpretation = TimeSliceInterpretationType.PERMDELTA
                            };


                            IsLoaded = true;
                            EditedFeature = newFeature;
                            break;
                        case DocumentType.ViewFeatureState:
                            {
                                Application.Current.Dispatcher.Invoke(
                                    DispatcherPriority.Background,
                                    (Action)(
                                        () =>
                                        {
                                            ColumnCollection.Clear();
                                            ColumnCollection.Add(PropertyColumn);
                                            ColumnCollection.Add(ValueColumn);
                                        }));

                                var data = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId
                                {
                                    FeatureTypeId =
                                        (int)
                                            FeatureType,
                                    Guid =
                                        FeatureIdentifier,
                                    WorkPackage = Workpackage
                                },
                                    false,
                                    Interpretation == Interpretation.Snapshot ? StartDateTime : AiracDate,
                                    Interpretation,
                                    Interpretation == Interpretation.Snapshot ? EndDateTime : null);
                                if (data == null || data.Count == 0 || data[0].Data == null || data[0].Data.Feature == null)
                                {
                                    MessageBoxHelper.Show(
                                        "Sorry, there was error while requesting data. It is possible that you specified actual date when feature does not exist.",
                                        "Error while requesting data", MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                                    //close?
                                    IsLoaded = true;

                                }
                                else
                                {
                                    IsLoaded = true;
                                    EditedFeature = data[0].Data.Feature;
                                }

                            }
                            break;
                        case DocumentType.EditFeatureState:
                            {
                                int interpretationIndex = -1;

                                EventViewModel.ShowDialog(AiracDate, t =>
                                {
                                    interpretationIndex = t.InterpretationIndex;
                                    StartDateTime = t.StartDate;
                                    EndDateTime = t.IsValidEnd ? (DateTime?)t.EndDate : null;
                                });

                                if (interpretationIndex == -1) return;

                                AiracDate = StartDateTime;
                                UpdateTitle();

                                Interpretation = interpretationIndex == 0
                                    ? Interpretation.BaseLine
                                    : Interpretation.Snapshot;
                                //----------------------------------------------------------------------------------------

                                var data = CommonDataProvider.GetDataForEditor(FeatureType, FeatureIdentifier, AiracDate, Workpackage, Interpretation, Interpretation == Interpretation.Snapshot ? EndDateTime : null);

                                if (data == null)
                                {
                                    MessageBoxHelper.Show(
                                        "Sorry, there was error while requesting data. It is possible that you specified actual date when feature does not exist.",
                                        "Error while requesting data", MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                                    //close?
                                    IsLoaded = true;

                                }
                                else
                                {
                                    IsLoaded = true;

                                    //for test
                                    //data.StateBeforeDelta = data.StateAfterDelta;

                                    Feature feature = null;
                                    if (Interpretation == Interpretation.Snapshot)
                                    {
                                        if (data.StateAfterDelta.Feature.TimeSlice.Interpretation == TimeSliceInterpretationType.TEMPDELTA)
                                            feature = PrepareAsCorrection(data, StartDateTime, EndDateTime);
                                        else
                                        if (data.StateAfterDelta.Feature.TimeSlice.Interpretation == TimeSliceInterpretationType.PERMDELTA)
                                            feature = PrepareAsSequence(data, StartDateTime, EndDateTime);
                                        feature.TimeSlice.CorrectionNumber = 0;
                                    }
                                    else
                                    if (data.StateBeforeDelta == null)
                                    {
                                        feature = PrepareAsCorrection(data, StartDateTime, EndDateTime);
                                    }
                                    else if (data.Delta == null)
                                    {
                                        feature = PrepareAsSequence(data, StartDateTime, EndDateTime);
                                    }
                                    else
                                    {
                                        feature = PrepareAsCorrection(data, StartDateTime, EndDateTime);
                                    }

                                    if (feature != null)
                                    {
                                        feature.TimeSlice.Interpretation = Interpretation == Interpretation.BaseLine
                                            ? TimeSliceInterpretationType.PERMDELTA
                                            : TimeSliceInterpretationType.TEMPDELTA;
                                        EditedFeature = feature;
                                    }
                                }
                            }
                            break;
                    }
                });
        }

        private Feature PrepareAsSequence(StateWithDelta<AimFeature> data, DateTime startDateTime,
            DateTime? endDateTime)
        {
            EditingType = DocumentEditingType.EditAsSequence;

            //it is new sequence
            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Background,
                (Action)(
                    () =>
                    {
                        ColumnCollection.Clear();
                        ColumnCollection.Add(DeltaColumn);
                        ColumnCollection.Add(PropertyColumn);
                        ColumnCollection.Add(PreviousValueColumn);
                        ColumnCollection.Add(ValueColumn);
                        ColumnCollection.Add(RestoreColumn);
                    }));

            DeltaFilterVisibility = Visibility.Visible;
            //
            DeltaMask = new DeltaMask
            {
                Delta = data.Delta,
                StateBeforeDelta = data.StateBeforeDelta
            };

            var feature = data.StateAfterDelta.Feature;



            if (feature.TimeSlice.ValidTime == null)
            {
                feature.TimeSlice.ValidTime = new TimePeriod();
            }
            feature.TimeSlice.ValidTime.BeginPosition = startDateTime;
            feature.TimeSlice.ValidTime.EndPosition = endDateTime;


            return feature;
        }

        private Feature PrepareAsCorrection(StateWithDelta<AimFeature> data, DateTime startDateTime, DateTime? endDateTime)
        {
            EditingType = DocumentEditingType.EditAsCorrection;


            if (data.Delta != null)
            {
                //it is correction to sequence
                Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Background,
                (Action)(
                    () =>
                    {
                        ColumnCollection.Clear();
                        ColumnCollection.Add(DeltaColumn);
                        ColumnCollection.Add(PropertyColumn);
                        ColumnCollection.Add(PreviousValueColumn);
                        ColumnCollection.Add(ValueColumn);
                        ColumnCollection.Add(RestoreColumn);
                    }));

                DeltaFilterVisibility = Visibility.Visible;
                //
                DeltaMask = new DeltaMask
                {
                    Delta = data.Delta,
                    StateBeforeDelta = data.StateBeforeDelta
                };
            }
            else
            {
                //it is correction to commissioning 
                Application.Current.Dispatcher.Invoke(
                    DispatcherPriority.Background,
                    (Action)(
                        () =>
                        {
                            ColumnCollection.Clear();
                            ColumnCollection.Add(DeltaColumn);
                            ColumnCollection.Add(PropertyColumn);
                            ColumnCollection.Add(ValueColumn);
                            ColumnCollection.Add(RestoreColumn);
                        }));
                DeltaFilterVisibility = Visibility.Collapsed;
            }

            var feature = data.StateAfterDelta.Feature;

            //TODO: prevent change of interpretation as a correction

            if (feature.TimeSlice.ValidTime == null)
            {
                feature.TimeSlice.ValidTime = new TimePeriod();
            }
            feature.TimeSlice.ValidTime.BeginPosition = startDateTime;
            feature.TimeSlice.ValidTime.EndPosition = endDateTime;

            return feature;
        }

        #endregion

        public int Workpackage { get; private set; }
        public FeatureEditorDocViewModel(FeatureType type, Guid id, DateTime date, int workpackage = 0)
            : base(type, id, date)
        {
            ContentId = ToolContentId;
            Workpackage = workpackage;
        }

        public Guid BufferFeatureIdentifier { get; set; }
        public DateTime BufferDateTime { get; set; }


        public override void UpdateDirtyStatus()
        {
            switch (DocType)
            {
                case DocumentType.CreateNewFeature:
                    IsDirty = true;
                    break;
                case DocumentType.EditFeatureState:
                    IsDirty = IsChanged;
                    break;
            }
        }

        #region Other Commands

        private RelayCommand _evolutionCommand;
        public RelayCommand EvolutionCommand
        {
            get
            {
                return _evolutionCommand ?? (_evolutionCommand = new RelayCommand(
                                                                     t =>
                                                                     MainManagerModel.Instance.Evolution(EditedFeature,
                                                                                                         AiracDate),
                                                                     t => EditedFeature != null));
            }
            set => _evolutionCommand = value;
        }

        private RelayCommand _relationCommand;
        public RelayCommand RelationCommand
        {
            get
            {
                return _relationCommand ?? (_relationCommand = new RelayCommand(
                                                                   t =>
                                                                   MainManagerModel.Instance.Relation(EditedFeature,
                                                                                                      AiracDate),
                                                                   t => EditedFeature != null));
            }
            set => _relationCommand = value;
        }

        private RelayCommand _onMapCommand;
        public RelayCommand OnMapCommand
        {
            get
            {
                return _onMapCommand ?? (_onMapCommand = new RelayCommand(
                                                             t => { },
                                                             t => EditedFeature != null));
            }
            set => _onMapCommand = value;
        }

        private RelayCommand _copyCommand;
        public RelayCommand CopyCommand
        {
            get
            {
                return _copyCommand ?? (_copyCommand = new RelayCommand(
                                                           t => MainManagerModel.Instance.Copy(EditedFeature, AiracDate),
                                                           t => EditedFeature != null));
            }
            set => _copyCommand = value;
        }

        private RelayCommand _geoIntersectionCommand;
        public RelayCommand GeoIntersectionCommand
        {
            get
            {
                return _geoIntersectionCommand ?? (_geoIntersectionCommand = new RelayCommand(
                                                                                 t =>
                                                                                 MainManagerModel.Instance.
                                                                                     GeoIntersection(EditedFeature,
                                                                                                     AiracDate),
                                                                                 t => EditedFeature != null));
            }
            set => _geoIntersectionCommand = value;
        }

        #endregion



        #region Save/Edit/Cancel Command

        private void CollectNillReasons()
        {
            var feature = EditedFeature as Feature;
            if (feature == null) return;

            //set new nil reasons
            foreach (var editableSinglePropertyModel in PropertyList.Where(t => !t.IsReadOnly && t is AimEditablePropertyModel))
            {
                var model = (AimEditablePropertyModel)editableSinglePropertyModel;
                NilReason? reason = null;
                if (model.IsDelta && !model.IsNotNull)
                {
                    reason = model.NilReason;
                }
                feature.SetNilReason(model.PropInfo.Index, reason);
            }
        }

        private void SaveDelta()
        {
            var feature = EditedFeature as Feature;

            if (DocType == DocumentType.CreateNewFeature)
            {
                //do commit
                CommonDataProvider.CommitAsNewSequence(feature, Workpackage);
                return;
            }
            if (EditingType == DocumentEditingType.EditAsCorrection)
            {
                CollectNillReasons();

                foreach (var prop in PropertyList.Where(t => !t.IsReadOnly && !t.IsDelta))
                {
                    prop.OnSetNull.Execute(null);
                }


                CommonDataProvider.CommitAsCorrection(feature, Workpackage);
                //apply mask
                return;
            }
            if (EditingType == DocumentEditingType.EditAsSequence)
            {
                if (feature == null) return;

                CollectNillReasons();


                foreach (var prop in PropertyList.Where(t => !t.IsReadOnly && !t.IsDelta))
                {
                    prop.OnSetNull.Execute(null);
                }

                if (feature.TimeSlice == null) feature.TimeSlice = new TimeSlice();
                else
                    feature.TimeSlice.CorrectionNumber = 0;

                if (feature.TimeSlice.ValidTime == null)
                {
                    feature.TimeSlice.ValidTime = new TimePeriod
                    {
                        BeginPosition = AiracDate,
                        EndPosition = null
                    };
                }

                CommonDataProvider.CommitAsNewSequence(feature, Workpackage);
                return;
            }
        }

        private RelayCommand _addToExportCommand;
        public RelayCommand AddToExportCommand
        {
            get
            {


                return _addToExportCommand ?? (_addToExportCommand = new RelayCommand(
                           t =>
                           {
                               BlockerModel.BlockForAction(
                                   () =>
                                   {
                                       MainManagerModel.Instance.AddToExport(EditedFeature);
                                       MainManagerModel.Instance.ExportToolViewModel.SelectedRelation = null;
                                       MainManagerModel.Instance.ExportToolViewModel.Reload();

                                   }
                               );

                           },
                           t => EditedFeature != null));
            }
        }

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand
                                                           (
                t =>
                    {
                        if (!(EditedFeature is Feature)) return;

                        if (ConfigUtil.UseWebApiForMetadata && SelectedOriginator == null)
                        {
                            MessageBoxHelper.Show("Please select originator");
                            return;
                        }

                        if (MessageBoxHelper.Show(
                            "Are you sure you want to commit changes?",
                            "Committing data", MessageBoxButton.YesNo,
                            MessageBoxImage.Warning)
                            == MessageBoxResult.Yes)
                        {
                            try
                            {
                                SaveDelta();
                                MainManagerModel.Instance.OnFeatureChanged((Feature)EditedFeature);

                                DocType = DocumentType.ViewFeatureState;

                                IsLoaded = false;
                                Load();
                            }
                            catch (Exception exception)
                            {
                                MessageBoxHelper.Show(
                                    "The following error occured while commiting data:\n" +
                                    exception.Message,
                                    "Error while committing data",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                            }
                        }
                    },
                t =>
                PropertyList.Any(
                    singlePropertyModel => singlePropertyModel.IsChanged))
                                       );
            }
            set => _saveCommand = value;
        }

        private RelayCommand _editCommand;
        public RelayCommand EditCommand
        {
            get
            {
                return _editCommand ?? (_editCommand = new RelayCommand(
                    t =>
                        {
                            DocType = DocumentType.EditFeatureState;
                            IsLoaded = false;
                            Load();
                        },
                    t =>
                        {
                            if (CurrentDataContext.CurrentUser == null)
                                return false;
                            if (
                                CurrentDataContext.CurrentUser.ActivePrivateSlot ==
                                null) return false;
                            if (!CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.Editable) return false;
                            return
                                CurrentDataContext.CurrentUser.ActivePrivateSlot.
                                    PublicSlot.EffectiveDate == AiracDate;
                        }));
            }
            set => _editCommand = value;
        }

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new RelayCommand(
                    t =>
                        {
                            if (!(PropertyList.Any(
                                singlePropertyModel =>
                                singlePropertyModel.IsChanged))
                                ||
                                MessageBoxHelper.Show(
                                    "Are you sure you want to discard all changes?",
                                    "Some changes were not saved",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Warning)
                                == MessageBoxResult.Yes)
                            {
                                //cancel all changes
                                foreach (
                                    var singlePropertyModel in
                                        PropertyList.Where(
                                            singlePropertyModel
                                            =>
                                            singlePropertyModel.
                                                IsChanged))
                                {
                                    singlePropertyModel.
                                        OnRestore.Execute(null);
                                }

                                //change document type if needed
                                if (DocType ==
                                    DocumentType.
                                        EditFeatureState)
                                {
                                    DocType =
                                        DocumentType.
                                            ViewFeatureState;
                                    EditedFeature =
                                        EditedFeature;
                                }
                            }

                        },
                    t =>
                    DocType == DocumentType.EditFeatureState ||
                    PropertyList.Any(
                        singlePropertyModel =>
                        singlePropertyModel.IsChanged)));
            }
            set => _cancelCommand = value;
        }

        #endregion

        #region Doc Type and Title

        public override void UpdateTitle()
        {
            if (!IsLoaded) return;

            var airacMessage = AiracSelectorViewModel.AiracMessage(AiracDate);

            switch (DocType)
            {
                case DocumentType.EditFeatureState:
                    Title = "Editing state of " +
                            EditedFeature.GetType().Name + " " +
                            HumanReadableConverter.ShortAimDescription(EditedFeature) +
                            airacMessage;

                    break;
                case DocumentType.ViewFeatureState:
                    Title = "Current state of " +
                            EditedFeature.GetType().Name + " " +
                            HumanReadableConverter.ShortAimDescription(EditedFeature) +
                            airacMessage;

                    break;
                case DocumentType.CreateNewFeature:
                    Title = "Commissioning of new " +
                            EditedFeature.GetType().Name + " " +
                            HumanReadableConverter.ShortAimDescription(EditedFeature) +
                            airacMessage;

                    break;
            }
        }


        private DocumentType _docType = DocumentType.ViewFeatureState;
        public DocumentType DocType
        {
            get => _docType;
            set
            {
                _docType = value;

                switch (DocType)
                {
                    case DocumentType.CreateNewFeature:
                        IconSource = new Uri("pack://application:,,,/Resources/Images/edit.png",
                                             UriKind.RelativeOrAbsolute);
                        TitleToolTip = "This pane is in Edit mode";

                        IsAiracSelectionEnabled = false;
                        ChangedFilterVisibility = Visibility.Collapsed;
                        DeltaFilterVisibility = Visibility.Collapsed;
                        SpecialViewsToolBarVisibility = Visibility.Collapsed;
                        EditLabelVisibility = Visibility.Collapsed;
                        EditButtonVisibility = Visibility.Collapsed;
                        SaveButtonVisibility = Visibility.Visible;
                        OriginatorToolBarVisibility = Visibility.Visible;


                        IsReadOnly = false;

                        break;
                    case DocumentType.ViewFeatureState:
                        IconSource = new Uri("pack://application:,,,/Resources/Images/open.png",
                                             UriKind.RelativeOrAbsolute);
                        TitleToolTip = "This pane is in Read-only mode";

                        IsAiracSelectionEnabled = true;
                        ChangedFilterVisibility = Visibility.Collapsed;
                        DeltaFilterVisibility = Visibility.Collapsed;
                        EditLabelVisibility = Visibility.Collapsed;
                        SpecialViewsToolBarVisibility = Visibility.Visible;

                        EditButtonVisibility = Visibility.Visible;
                        SaveButtonVisibility = Visibility.Collapsed;
                        OriginatorToolBarVisibility = Visibility.Collapsed;
                        IsDirty = false;

                        IsReadOnly = true;
                        break;
                    case DocumentType.EditFeatureState:
                        IconSource = new Uri("pack://application:,,,/Resources/Images/edit.png",
                                             UriKind.RelativeOrAbsolute);
                        TitleToolTip = "This pane is in Edit mode";

                        IsAiracSelectionEnabled = false;
                        ChangedFilterVisibility = Visibility.Visible;
                        DeltaFilterVisibility = Visibility.Visible;
                        EditLabelVisibility = Visibility.Visible;
                        SpecialViewsToolBarVisibility = Visibility.Visible;

                        EditButtonVisibility = Visibility.Collapsed;
                        SaveButtonVisibility = Visibility.Visible;
                        OriginatorToolBarVisibility = Visibility.Visible;

                        IsDirty = false;

                        IsReadOnly = false;
                        break;
                }


                UpdateDirtyStatus();
                //if (CurrentViewer != null) 
                RecreateColumns();

                UpdateTitle();
            }
        }


        private String _editingTypeString;
        public String EditingTypeString
        {
            get => _editingTypeString;
            set
            {
                _editingTypeString = value;
                OnPropertyChanged("EditingTypeString");
            }
        }

        public Interpretation Interpretation { get; set; } = Interpretation.Snapshot;
        public DateTime StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; } = null;

        public DocumentEditingType EditingType
        {
            get => _editingType;
            set
            {
                _editingType = value;
                switch (_editingType)
                {
                    case DocumentEditingType.EditAsSequence:
                        EditingTypeString = "New Sequence";
                        break;
                    case DocumentEditingType.EditAsCorrection:
                        EditingTypeString = "Correction";
                        break;

                }
                OnPropertyChanged("EditingType");
            }
        }

        private DocumentEditingType _editingType;

        #endregion

        #region Columns

        public virtual void RecreateColumns()
        {
            ColumnCollection.Clear();

            switch (DocType)
            {
                case DocumentType.CreateNewFeature:
                    ColumnCollection.Add(PropertyColumn);
                    ColumnCollection.Add(ValueColumn);
                    break;
                case DocumentType.ViewFeatureState:
                    //ColumnCollection.Add(DeltaColumn);
                    ColumnCollection.Add(PropertyColumn);
                    ColumnCollection.Add(ValueColumn);
                    break;
                case DocumentType.EditFeatureState:
                    ColumnCollection.Add(DeltaColumn);
                    ColumnCollection.Add(PropertyColumn);
                    ColumnCollection.Add(PreviousValueColumn);
                    ColumnCollection.Add(ValueColumn);
                    ColumnCollection.Add(RestoreColumn);
                    break;
            }

        }


        public override void OnCurrentViewerSet()
        {
            RecreateColumns();
        }

        #endregion

    }
}
