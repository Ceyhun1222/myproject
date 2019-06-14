using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.Common.Util;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.View;
using MvvmCore;
using TOSSM.Converter;
using TOSSM.Util;
using TOSSM.Util.Notams;
using TOSSM.ViewModel.Document.Single.Editable;

namespace TOSSM.ViewModel.Document.Editor
{
    public class NotamFeatureEditorViewModel : FeatureSubEditorDocViewModel
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

        public Feature Feature { get; private set; }
        public Notam Notam { get; private set; }
        public Notam RefNotam { get; private set; }
        public Notam ActualNotam => Notam.Type == (int)NotamType.C ? RefNotam : Notam;

        #region Load

        public override async void Load()
        {
            if (IsLoaded) return;

            await BlockerModel.BlockForAction(
                () =>
                {
                    try
                    {
                        NotamManager.GetOperation(Notam, RefNotam).Prepare(this);
                    }
                    catch (NotImplementedException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unexpected execption");
                        LogManager.GetLogger(typeof(NotamFeatureEditorViewModel)).Error(ex, ex.Message);
                    }
                });
        }

        public Feature PrepareAsSequence(StateWithDelta<AimFeature> data, DateTime startDateTime,
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



        public Feature PrepareAsCorrection(StateWithDelta<AimFeature> data, DateTime startDateTime, DateTime? endDateTime)
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

        public NotamFeatureEditorViewModel(Notam notam, Notam refNotam, Feature feature, int workpackage = 0)
            : this(notam, feature.FeatureType, feature.Identifier, notam.StartValidity, workpackage)
        {
            Feature = feature;
            if (Feature != null)
                DocType = DocumentType.EditFeatureState;
            RefNotam = refNotam;

        }

        public NotamFeatureEditorViewModel(Notam notam, Feature feature, int workpackage = 0)
            : this(notam, feature.FeatureType, feature.Identifier, notam.StartValidity, workpackage)
        {
            Feature = feature;
            if (Feature != null)
                DocType = DocumentType.EditFeatureState;

        }


        public NotamFeatureEditorViewModel(Notam notam, FeatureType type, Guid id, int workpackage = 0)
            : this(notam, type, id, notam.StartValidity, workpackage)
        {
        }

        private NotamFeatureEditorViewModel(Notam notam, FeatureType type, Guid id, DateTime date, int workpackage = 0)
            : base(type, id, date)
        {
            Notam = notam;
            ContentId = ToolContentId;
            Workpackage = workpackage;
            DocType = DocumentType.CreateNewFeature;
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

        public void PrepareDelta()
        {
            var feature = EditedFeature as Feature;

            if (EditingType == DocumentEditingType.EditAsCorrection)
            {
                CollectNillReasons();

                foreach (var prop in PropertyList.Where(t => !t.IsReadOnly && !t.IsDelta))
                {
                    prop.OnSetNull.Execute(null);
                }

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
                        BeginPosition = StartDateTime,
                        EndPosition = EndDateTime
                    };
                }
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

                               try
                               {
                                   NotamManager.GetOperation(Notam, RefNotam).Save(this);
                               }
                               catch (NotImplementedException ex)
                               {
                                   MessageBox.Show(ex.Message);
                               }
                               catch (Exception ex)
                               {
                                   MessageBox.Show("Unexpected execption");
                                   LogManager.GetLogger(typeof(NotamFeatureEditorViewModel)).Error(ex, ex.Message);
                               }
                           }
                       ));
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
            Title = "Generatin Notam of " +
                    EditedFeature.GetType().Name + " " +
                    HumanReadableConverter.ShortAimDescription(EditedFeature);
        }


        private DocumentType _docType = DocumentType.ViewFeatureState;
        public DocumentType DocType
        {
            get => _docType;
            private set
            {
                _docType = value;


                IconSource = new Uri("pack://application:,,,/Resources/Images/edit.png",
                    UriKind.RelativeOrAbsolute);
                TitleToolTip = "This pane is in Notam generation mode";

                IsAiracSelectionEnabled = false;
                ChangedFilterVisibility = Visibility.Visible;
                DeltaFilterVisibility = Visibility.Visible;
                EditLabelVisibility = Visibility.Visible;
                SpecialViewsToolBarVisibility = Visibility.Visible;

                EditButtonVisibility = Visibility.Collapsed;
                SaveButtonVisibility = Visibility.Visible;

                IsDirty = false;

                IsReadOnly = false;


                UpdateDirtyStatus();

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