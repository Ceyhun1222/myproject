using Aran.Aim;
using Aran.Aim.Features;
using Aran.Aim.Utilities;
using Aran.PANDA.Common;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Id;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using FluentNHibernate.Conventions;
using Microsoft.Win32;
using MvvmCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Xml;
using Aran.Temporality.Common.Logging;
using TOSSM.Common;
using TOSSM.Util;
using TOSSM.Util.Wrapper;
using TOSSM.ViewModel.Document.Relations.Util;
using TOSSM.ViewModel.Pane;
using TOSSM.ViewModel.Pane.Base;
using System.IO;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Util;
using TOSSM.ViewModel.Enums;
using System.Collections.ObjectModel;
using Aran.Temporality.Common.Entity;
using Aran.Aim.Metadata.UI;
using Aran.Aim.Data.Filters;
using Aran.Aim.Data;

namespace TOSSM.ViewModel.Tool
{
    public class ExportToolViewModel : ToolViewModel, IPresenterParent
    {
        public static string ToolContentId = "Export";



        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/export.png", UriKind.RelativeOrAbsolute);

        public ExportToolViewModel()
            : base(ToolContentId)
        {
            ContentId = ToolContentId;
            DataPresenter = new DataPresenterModel { ViewModel = this };
            EventType = EventsEnum.All;
            StateType = StatesEnum.Snapshot;
        }

        public DataPresenterModel DataPresenter { get; }

        private readonly List<FeatureError> _featureErrors = new List<FeatureError>();
        private readonly List<ExceptionError> _exceptionErrors = new List<ExceptionError>();
        private List<FeatureType> _eadFeatureTypeList = new List<FeatureType>{
            FeatureType.AircraftGroundService,
            FeatureType.AircraftStand,
            FeatureType.AirportClearanceService,
            FeatureType.AirportHeliport,
            FeatureType.AirportSuppliesService,
            FeatureType.Airspace,
            FeatureType.AirTrafficControlService,
            FeatureType.AirTrafficManagementService,
            FeatureType.AngleIndication,
            FeatureType.ApproachLightingSystem,
            FeatureType.Apron,
            FeatureType.ApronElement,
            FeatureType.ApronMarking,
            FeatureType.ArrestingGear,
            FeatureType.ArrivalLeg,
            FeatureType.AuthorityForAirspace,
            FeatureType.DeicingArea,
            FeatureType.DepartureLeg,
            FeatureType.DesignatedPoint,
            FeatureType.DistanceIndication,
            FeatureType.DME,
            FeatureType.FinalLeg,
            FeatureType.FireFightingService,
            FeatureType.GeoBorder,
            FeatureType.Glidepath,
            FeatureType.GroundTrafficControlService,
            FeatureType.GuidanceLine,
            FeatureType.HoldingAssessment,
            FeatureType.HoldingPattern,
            FeatureType.InformationService,
            FeatureType.InitialLeg,
            FeatureType.InstrumentApproachProcedure,
            FeatureType.IntermediateLeg,
            FeatureType.Localizer,
            FeatureType.MissedApproachLeg,
            FeatureType.Navaid,
            FeatureType.ObstacleArea,
            FeatureType.OrganisationAuthority,
            FeatureType.PassengerService,
            FeatureType.RadioCommunicationChannel,
            FeatureType.Route,
            FeatureType.RouteSegment,
            FeatureType.RulesProcedures,
            FeatureType.Runway,
            FeatureType.RunwayCentrelinePoint,
            FeatureType.RunwayDirection,
            FeatureType.RunwayDirectionLightSystem,
            FeatureType.RunwayElement,
            FeatureType.RunwayProtectArea,
            FeatureType.SafeAltitudeArea,
            FeatureType.SignificantPointInAirspace,
            FeatureType.SpecialDate,
            FeatureType.StandardInstrumentArrival,
            FeatureType.StandardInstrumentDeparture,
            FeatureType.StandardLevelColumn,
            FeatureType.StandardLevelSector,
            FeatureType.StandardLevelTable,
            FeatureType.TaxiHoldingPosition,
            FeatureType.TaxiHoldingPositionLightSystem,
            FeatureType.Taxiway,
            FeatureType.TaxiwayElement,
            FeatureType.TaxiwayLightSystem,
            FeatureType.TaxiwayMarking,
            FeatureType.TouchDownLiftOff,
            FeatureType.TouchDownLiftOffSafeArea,
            FeatureType.Unit,
            FeatureType.VerticalStructure,
            FeatureType.VisualGlideSlopeIndicator,
            FeatureType.VOR
        };

        private DateTime _airacDate;

        public DateTime AiracDate
        {
            get => _airacDate;
            set
            {
                if (_airacDate == value) return;
                _airacDate = value;
                OnPropertyChanged("AiracDate");
                DataPresenter.EffectiveDate = AiracDate;

                DataPresenter.BlockerModel.BlockForAction(() =>
                {
                    ReloadData(DataPresenter);
                });
            }
        }

        public Visibility InterpretationVisibility
        {
            get => _interpretationVisibility;
            set
            {
                _interpretationVisibility = value;
                OnPropertyChanged("InterpretationVisibility");
            }
        }

        public Visibility EventTypeVisibility
        {
            get => _eventTypeVisibility;
            set
            {
                _eventTypeVisibility = value;
                OnPropertyChanged("EventTypeVisibility");
            }
        }

        public Visibility AiracControlVisibility
        {
            get => _airacControlVisibility;
            set
            {
                _airacControlVisibility = value;
                OnPropertyChanged("AiracControlVisibility");
            }
        }

        public Visibility DecomissionedCheckBoxVisibility
        {
            get => _decomissionedCheckBoxVisibility;
            set
            {
                _decomissionedCheckBoxVisibility = value;
                OnPropertyChanged(nameof(DecomissionedCheckBoxVisibility));
            }
        }

        public Visibility ObstacleCheckBoxVisibility
        {
            get => _obstacleCheckBoxVisibility;
            set
            {
                _obstacleCheckBoxVisibility = value;
                OnPropertyChanged(nameof(ObstacleCheckBoxVisibility));
            }
        }

        public ExportEnum ExportFeaturesIndex
        {
            get => _exportFeaturesIndex;
            set
            {
                _exportFeaturesIndex = value;
                OnPropertyChanged("ExportFeaturesIndex");
                AiracControlVisibility = (int)ExportFeaturesIndex < 4 || ExportFeaturesIndex == ExportEnum.ObstacleDataSet ? Visibility.Visible : Visibility.Collapsed;
                CustomRangeVisibility = (int)ExportFeaturesIndex < 4 || ExportFeaturesIndex == ExportEnum.ObstacleDataSet ? Visibility.Collapsed : Visibility.Visible;
                InterpretationVisibility = (int)ExportFeaturesIndex < 6 ? Visibility.Visible : Visibility.Collapsed;
                EventTypeVisibility = (int)ExportFeaturesIndex < 6 || ExportFeaturesIndex == ExportEnum.ObstacleDataSet ? Visibility.Collapsed : Visibility.Visible;
                DecomissionedCheckBoxVisibility = (int)ExportFeaturesIndex < 4 ? Visibility.Visible : Visibility.Collapsed;
                ObstacleCheckBoxVisibility = (int)ExportFeaturesIndex < 4 ? Visibility.Visible : Visibility.Collapsed;
                DataSetConfigurationsVisibility = ExportFeaturesIndex == ExportEnum.CustomDataSet ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public bool ExportDecommissioned
        {
            get => _exportDecommissioned;
            set
            {
                _exportDecommissioned = value;
                OnPropertyChanged(nameof(ExportDecommissioned));
            }
        }

        public bool ExportObstacles
        {
            get => _exportObstacles;
            set
            {
                _exportObstacles = value;
                OnPropertyChanged(nameof(ExportObstacles));
            }
        }

        public StatesEnum StateType
        {
            get => _stateType;
            set
            {
                _stateType = value;
                OnPropertyChanged(nameof(StateType));
            }
        }

        public MtObservableCollection<EventsEnum> EventTypes = new MtObservableCollection<EventsEnum> { EventsEnum.All, EventsEnum.PermDelta, EventsEnum.TempDelta };
        public EventsEnum EventType
        {
            get => _eventType;
            set
            {
                _eventType = value;
                OnPropertyChanged(nameof(EventType));
            }
        }

        public DateTime RangeStartDate { get; set; } = DateTime.Today;

        public DateTime RangeEndDate { get; set; } = DateTime.Today;

        public Visibility CustomRangeVisibility
        {
            get => _customRangeVisibility;
            set
            {
                _customRangeVisibility = value;
                OnPropertyChanged("CustomRangeVisibility");
            }
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
        private RelayCommand _saveCommand;
        private RelayCommand _clearCommand;
        private RelayCommand _openSourceCommand;
        private RelayCommand _createBaselineCommand;

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

        public void RemoveFeatureToExport(FeatureType featureType, Guid id)
        {
            SortedSet<Guid> list;
            if (!_toBeExportedIds.TryGetValue(featureType, out list)) return;
            list.Remove(id);

            var relation = RelationsData.FirstOrDefault(t => t.FeatureType == featureType);
            if (relation == null) return;
            relation.Count = list.Count;
            if (relation.Count == 0)
            {
                RelationsData.Remove(relation);
            }
        }

        public void AddFeatureToExport(FeatureType featureType, Guid id)
        {
            SortedSet<Guid> list;
            if (!_toBeExportedIds.TryGetValue(featureType, out list))
            {
                list = new SortedSet<Guid>();
                _toBeExportedIds[featureType] = list;
            }
            list.Add(id);

            var relation = RelationsData.FirstOrDefault(t => t.FeatureType == featureType);
            if (relation == null)
            {
                relation = new SingleTypeRelationViewModel { FeatureType = featureType };
                RelationsData.Add(relation);
            }

            relation.Count = list.Count;
        }


        private readonly Dictionary<FeatureType, SortedSet<Guid>> _toBeExportedIds =
            new Dictionary<FeatureType, SortedSet<Guid>>();

        private RelayCommand _removeCommand;
        private Visibility _airacControlVisibility = Visibility.Visible;
        private ExportEnum _exportFeaturesIndex;
        private Visibility _customRangeVisibility = Visibility.Collapsed;
        private Visibility _interpretationVisibility = Visibility.Visible;
        private Visibility _decomissionedCheckBoxVisibility = Visibility.Visible;
        private Visibility _obstacleCheckBoxVisibility = Visibility.Visible;
        private Visibility _eventTypeVisibility = Visibility.Collapsed;
        private EventsEnum _eventType;
        private StatesEnum _stateType;
        private bool _exportDecommissioned;
        private bool _exportObstacles;

        private readonly AixmHelper _aimHelper = new AixmHelper();

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
            if (SelectedRelation == null)
            {
                DataPresenter.FeatureData = new List<object>();
                DataPresenter.UpdateFeatureDataFiltered();
                return;
            }

            SortedSet<Guid> ids;
            if (!_toBeExportedIds.TryGetValue((FeatureType)model.FeatureType, out ids))
            {
                DataPresenter.FeatureData = new List<object>();
                DataPresenter.UpdateFeatureDataFiltered();
                return;
            }

            var list = new List<object>();
            foreach (var id in ids)
            {
                var aimFeatures = CurrentDataContext.CurrentService.GetActualDataByDate(
                    new FeatureId { FeatureTypeId = (int)model.FeatureType, Guid = id }, false, AiracDate);
                if (aimFeatures != null && aimFeatures.Count > 0)
                {
                    list.Add(new ReadonlyFeatureWrapper(aimFeatures.First().Data));
                }
            }
            DataPresenter.FeatureData = list;
            DataPresenter.UpdateFeatureDataFiltered();
        }

        #endregion


        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(
                    t =>
                    {

                        CultureInfo ci = CultureInfo.InvariantCulture;
                        Thread.CurrentThread.CurrentCulture = ci;
                        Thread.CurrentThread.CurrentUICulture = ci;

                        var dialog = new SaveFileDialog
                        {
                            Title = "Save AIXM Message",
                            Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*",
                            FileName = CreateFileName()
                        };

                        if (dialog.ShowDialog() == true)
                        {
                            DataPresenter.BlockerModel.BlockForAction(() => { Export(dialog); });
                        }

                    },
                    t => ExportFeaturesIndex == ExportEnum.AllFeatures || ExportFeaturesIndex == ExportEnum.EAD || 
                    (ExportFeaturesIndex == ExportEnum.ObstacleDataSet && _toBeExportedIds.Any(x => x.Key == FeatureType.AirportHeliport)) ||
                    ExportFeaturesIndex == ExportEnum.AllStatesInRange || ExportFeaturesIndex == ExportEnum.AllEventsInRange || RelationsData.Count > 0));
            }
        }

        private void Export(SaveFileDialog dialog)
        {
            var oldCulture = Thread.CurrentThread.CurrentCulture;
            var oldUICulture = Thread.CurrentThread.CurrentUICulture;

            CultureInfo cultureInfo = new CultureInfo("en-US");
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            _featureErrors.Clear();
            _exceptionErrors.Clear();
            Dictionary<FeatureType, int> report = new Dictionary<FeatureType, int>();
            try
            {
                StringWriterWithEncoding stringBuilder;
                var writer = AixmGenerator.Instance.CreateXmlWriter(out stringBuilder);
                switch (ExportFeaturesIndex)
                {
                    case ExportEnum.AllFeatures:
                        ExportAllFeatures(writer, report);
                        break;
                    case ExportEnum.EAD:
                        ExportEAD(writer, report);
                        break;
                    case ExportEnum.SelectedFeatures:
                        ExportSelectedFeatures(writer, report);
                        break;
                    case ExportEnum.SelectedWithReferences:
                        ExportSelectedFeaturesWithReferences(writer, report);
                        break;
                    case ExportEnum.ObstacleDataSet:
                        ExportObstacleDataSet(writer, report);
                        break;
                    case ExportEnum.AllStatesInRange:
                        ExportAllStatesInRange(writer, report);
                        break;
                    case ExportEnum.StatesOfSelectedInRange:
                        ExportSelectedFeaturesAllStatesInRange(writer, report);
                        break;
                    case ExportEnum.AllEventsInRange:
                        ExportAllEventsInRange(writer, report);
                        break;
                    case ExportEnum.EventsOfSelectedInRange:
                        ExportSelectedFeaturesAllEventsInRange(writer, report);
                        break;
                }

                AixmGenerator.Instance.CloseXmlWriter(writer);
                string xmlString = stringBuilder.ToString();
                string crcText = CRC32.CalcCRC32(xmlString);
                string comment =
                    $"<!--version: {CurrentDataContext.Version}; {CreateDateComment()}; CRC32: {crcText}-->\n";
                int index = xmlString.IndexOf("<aixm-message-5.1:AIXMBasicMessage",
                    StringComparison.Ordinal);
                xmlString = xmlString.Insert(index, comment);

                System.IO.StreamWriter file = new System.IO.StreamWriter(dialog.FileName);
                file.WriteLine(xmlString);
                file.Close();

                using (StreamWriter reportFile = new StreamWriter($"{dialog.FileName}.report"))
                    foreach (var entry in report)
                        reportFile.WriteLine("[{0} {1}]", entry.Key, entry.Value);
                //<!--effective-date: 2015-06-08-->
                //writer.WriteComment(string.Format("effective-date: {0}", effectiveDateTime.ToString("yyyy-MM-dd")));

                //dialog.FileName, 
                //var crcText = CalcCRC(stringBuilder.ToString());
                //stringBuilder.Insert(crcTextPosition, crcText);                                    
                MainManagerModel.Instance.StatusText = "Done";
            }
            catch (Exception ex)
            {
                _exceptionErrors.Add(new ExceptionError { Message = ex.Message, Exception = ex });
            }

            if (_featureErrors.IsNotEmpty() || _exceptionErrors.IsNotEmpty())
            {
                foreach (var featureError in _featureErrors)
                {
                    foreach (var feature in featureError.Data)
                    {
                        LogManager.GetLogger(typeof(ExportToolViewModel))
                            .Warn($"{featureError.Message} Id {feature.Identifier} type {feature.FeatureType}");
                    }
                }

                foreach (var exception in _exceptionErrors)
                {
                    LogManager.GetLogger(typeof(ExportToolViewModel)).Error(exception.Exception, exception.Message);
                }

                MessageBoxHelper.Show("Error while exporting data.", "Please check log files", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }

            Thread.CurrentThread.CurrentCulture = oldCulture;
            Thread.CurrentThread.CurrentUICulture = oldUICulture;
        }


        private Interpretation ConvertEventTypeToInterpretation(EventsEnum eventEnum)
        {
            switch (eventEnum)
            {
                case EventsEnum.All:
                    return Interpretation.BaseLine;
                case EventsEnum.PermDelta:
                    return Interpretation.PermanentDelta;
                case EventsEnum.TempDelta:
                    return Interpretation.TempDelta;
            }
            return Interpretation.BaseLine;
        }

        private Interpretation ConvertStateTypeToInterpretation(StatesEnum statetEnum)
        {
            switch (statetEnum)
            {
                case StatesEnum.BaseLine:
                    return Interpretation.BaseLine;
                case StatesEnum.Snapshot:
                    return Interpretation.Snapshot;
                case StatesEnum.Temporary:
                    return Interpretation.Snapshot;
            }
            return Interpretation.BaseLine;
        }

        private void ExportSelectedFeaturesAllEventsInRange(XmlWriter writer, Dictionary<FeatureType, int> report)
        {
            DateTime dateStart = RangeStartDate;
            DateTime dateEnd = RangeEndDate;

            foreach (var pair in _toBeExportedIds)
            {
                var count = 0;
                foreach (var id in pair.Value)
                {
                    var featureType = pair.Key;
                    var data = CurrentDataContext.CurrentService.GetEventsByDate(
                        new FeatureId { FeatureTypeId = (int)featureType, Guid = id },
                        false, dateStart, dateEnd, ConvertEventTypeToInterpretation(EventType));

                    if (data != null && data.IsNotEmpty())
                    {
                        var grouped = data.GroupBy(f => f.Data.Identifier).ToList();
                        foreach (var item in grouped)
                        {
                            var featureList = item.Select(f => f.Data.Feature).ToList();
                            count++;
                            MainManagerModel.Instance.StatusText =
                                "Exporting " + featureType + ": " + count + " / " +
                                grouped.Count;
                            AixmGenerator.Instance.WriteFeature(writer, featureList);
                        }

                        if (grouped.IsNotEmpty())
                        {
                            if (!report.ContainsKey(featureType))
                                report.Add(featureType, 1);
                            else
                                report[featureType] = report[featureType] + 1;
                        }
                    }
                }
            }
        }

        private void ExportSelectedFeaturesAllStatesInRange(XmlWriter writer, Dictionary<FeatureType, int> report)
        {
            DateTime dateStart = RangeStartDate;
            DateTime dateEnd = RangeEndDate;

            foreach (var pair in _toBeExportedIds)
            {
                var count = 0;
                foreach (var id in pair.Value)
                {
                    var featureType = pair.Key;
                    var data = CurrentDataContext.CurrentService.GetStatesInRangeByInterpretation(
                        new FeatureId { FeatureTypeId = (int)featureType, Guid = id },
                        false, dateStart, dateEnd, ConvertStateTypeToInterpretation(StateType));
                    if (data != null && data.IsNotEmpty())
                    {
                        data = FilterByInterpretation(data);
                        var grouped = data.GroupBy(f => f.Data.Identifier).ToList();
                        foreach (var item in grouped)
                        {
                            var featureList = item.Select(f => f.Data.Feature).ToList();
                            count++;
                            MainManagerModel.Instance.StatusText =
                                "Exporting " + featureType + ": " + count + " / " +
                                grouped.Count;
                            AixmGenerator.Instance.WriteFeature(writer, featureList);
                        }

                        if (grouped.IsNotEmpty())
                        {
                            if (!report.ContainsKey(featureType))
                                report.Add(featureType, 1);
                            else
                                report[featureType] = report[featureType] + 1;
                        }
                    }
                }
            }
        }

        private void ExportAllStatesInRange(XmlWriter writer, Dictionary<FeatureType, int> report)
        {
            DateTime dateStart = RangeStartDate;
            DateTime dateEnd = RangeEndDate;

            //all features
            foreach (var featureType in Enum.GetValues(typeof(FeatureType)))
            {
                var count = 0;
                var data = CurrentDataContext.CurrentService.GetStatesInRangeByInterpretation(
                    new FeatureId { FeatureTypeId = (int)featureType }, false, dateStart,
                    dateEnd, ConvertStateTypeToInterpretation(StateType));

                if (data != null && data.IsNotEmpty())
                {
                    //data = RemoveDecomissioned(data, dateEnd);
                    //data = RemoveNonObstacle(data, dateEnd);
                    data = FilterByInterpretation(data);
                    var grouped = data.GroupBy(f => f.Data.Identifier).ToList();
                    foreach (var item in grouped)
                    {
                        var featureList = item.Select(f => f.Data.Feature).ToList();
                        count++;
                        MainManagerModel.Instance.StatusText =
                            "Exporting " + featureType + ": " + count + " / " +
                            grouped.Count;
                        AixmGenerator.Instance.WriteFeature(writer, featureList);
                    }

                    if (grouped.IsNotEmpty())
                        report.Add((FeatureType)featureType, grouped.Count);
                }
            }
        }

        private void ExportAllEventsInRange(XmlWriter writer, Dictionary<FeatureType, int> report)
        {
            DateTime dateStart = RangeStartDate;
            DateTime dateEnd = RangeEndDate;

            //all features
            foreach (var featureType in Enum.GetValues(typeof(FeatureType)))
            {
                var count = 0;
                var data = CurrentDataContext.CurrentService.GetEventsByDate(
                    new FeatureId { FeatureTypeId = (int)featureType }, false, dateStart,
                    dateEnd, ConvertEventTypeToInterpretation(EventType));

                if (data != null && data.IsNotEmpty())
                {
                    var grouped = data.GroupBy(f => f.Data.Identifier).ToList();
                    foreach (var item in grouped)
                    {
                        var featureList = item.Select(f => f.Data.Feature).ToList();
                        count++;
                        MainManagerModel.Instance.StatusText =
                            "Exporting " + featureType + ": " + count + " / " +
                            grouped.Count;
                        AixmGenerator.Instance.WriteFeature(writer, featureList);
                    }

                    if (grouped.IsNotEmpty())
                        report.Add((FeatureType)featureType, grouped.Count);
                }
            }
        }

        private void ExportSelectedFeaturesWithReferences(XmlWriter writer, Dictionary<FeatureType, int> report)
        {
            var processedIds = new SortedSet<Guid>();
            var obstacleAreas = GetObstacleAreas(AiracDate);

            foreach (var pair in _toBeExportedIds)
            {
                var count = 0;

                foreach (var id in pair.Value)
                {
                    if (!processedIds.Add(id))
                    {
                        count++;
                        MainManagerModel.Instance.StatusText =
                            "Exporting " + pair.Key + ": " + count + " / " +
                            pair.Value.Count;
                        continue;
                    }

                    var aimFeatures = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId { FeatureTypeId = (int)pair.Key, Guid = id },
                            false, AiracDate, ConvertStateTypeToInterpretation(StateType));

                    if (aimFeatures != null && aimFeatures.IsNotEmpty())
                    {
                        aimFeatures = FilterByInterpretation(aimFeatures);
                        aimFeatures = RemoveDecomissioned(aimFeatures, AiracDate);
                        aimFeatures = RemoveNonObstacle(aimFeatures, obstacleAreas, AiracDate);
                        if (aimFeatures.IsNotEmpty())
                        {
                            count++;
                            MainManagerModel.Instance.StatusText =
                                "Exporting " + pair.Key +
                                ": " + count + " / " +
                                pair.Value.Count;
                            WriteFeatureWithReference(writer,
                                aimFeatures.First().Data.Feature, processedIds, report);

                            if (!report.ContainsKey(pair.Key))
                                report.Add(pair.Key, 1);
                            else
                                report[pair.Key] = report[pair.Key] + 1;
                        }
                    }
                }
            }
        }

        private void ExportObstacleDataSetForMongo(XmlWriter writer, Dictionary<FeatureType, int> report)
        {
            if (!_toBeExportedIds.Any(x => x.Key == FeatureType.AirportHeliport))
            {
                MessageBox.Show("Select Airport Heliports to export");
                return;
            }

            var features = new SortedSet<Guid>();
            var processedIds = new SortedSet<Guid>();

            var airportHeliportGuids = _toBeExportedIds.FirstOrDefault(x => x.Key == FeatureType.AirportHeliport).Value.ToList();

            var filter = new Filter(new OperationChoice(new ComparisonOps(ComparisonOpType.In, "Identifier", airportHeliportGuids)));
            var airportHeliports = CurrentDataContext.CurrentService.GetActualDataByDate(
                        new FeatureId { FeatureTypeId = (int)FeatureType.AirportHeliport,},
                        false, AiracDate, Interpretation.Snapshot, filter: filter);
            ExportFeatures(writer, report, airportHeliports.Select(x => x.Data).ToList(), processedIds);

            var runwayDirectionGuids = new List<Guid>();
            if (airportHeliports.IsNotEmpty())
            {
                filter = new Filter(new OperationChoice(new ComparisonOps(ComparisonOpType.In, "AssociatedAirportHeliport", airportHeliportGuids)));
                var runways = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId { FeatureTypeId = (int)FeatureType.Runway },
                        false, AiracDate, Interpretation.Snapshot, filter: filter);
                ExportFeatures(writer, report, runways.Select(x => x.Data).ToList(), processedIds);

                filter = new Filter(new OperationChoice(new ComparisonOps(ComparisonOpType.In, "UsedRunway", runways.Select(x => x.Guid).ToList())));
                var runwayDirections = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId { FeatureTypeId = (int)FeatureType.RunwayDirection },
                        false, AiracDate, Interpretation.Snapshot, filter: filter);
                ExportFeatures(writer, report, runwayDirections.Select(x => x.Data).ToList(), processedIds);

                runwayDirectionGuids = runwayDirections.Select(x => x.Guid.Value).ToList();
            }

            if (runwayDirectionGuids.IsNotEmpty())
            {
                var binaryLogicOp = new BinaryLogicOp { Type = BinaryLogicOpType.Or };
                binaryLogicOp.OperationList.Add(new OperationChoice(new ComparisonOps(ComparisonOpType.In, "Reference.OwnerAirport", airportHeliportGuids)));
                binaryLogicOp.OperationList.Add(new OperationChoice(new ComparisonOps(ComparisonOpType.In, "Reference.OwnerRunway", runwayDirectionGuids)));
                filter = new Filter(new OperationChoice(binaryLogicOp));
            }
            else
            {
                filter = new Filter(new OperationChoice(new ComparisonOps(ComparisonOpType.In, "Reference.OwnerAirport", airportHeliportGuids)));
            }

            var obstacleAreas = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId { FeatureTypeId = (int)FeatureType.ObstacleArea },
                    false, AiracDate, Interpretation.Snapshot, filter: filter);
            ExportFeatures(writer, report, obstacleAreas.Select(x => x.Data).ToList(), processedIds);

            if (obstacleAreas.IsNotEmpty())
            {
                var verticalStructureGuids = obstacleAreas.SelectMany(x => 
                    (x.Data?.Feature as ObstacleArea).Obstacle.Select(o => o.Feature?.Identifier))
                    .Where(x => x != null).Distinct().ToList();
                filter = new Filter(new OperationChoice(new ComparisonOps(ComparisonOpType.In, "Identifier", verticalStructureGuids)));
                var verticalStructures = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId { FeatureTypeId = (int)FeatureType.VerticalStructure },
                        false, AiracDate, Interpretation.Snapshot, filter: filter);
                ExportFeatures(writer, report, verticalStructures.Select(x => x.Data).ToList(), processedIds);
            }
        }

        private void ExportObstacleDataSet(XmlWriter writer, Dictionary<FeatureType, int> report)
        {
            if (!_toBeExportedIds.Any(x => x.Key == FeatureType.AirportHeliport))
            {
                MessageBox.Show("Select Airport Heliports to export");
                return;
            }

            var processedIds = new SortedSet<Guid>();

            var toRemove = _toBeExportedIds.Keys.Where(x => x != FeatureType.AirportHeliport);
            foreach (var key in toRemove)
            {
                _toBeExportedIds.Remove(key);
            }

            var verticalStructureTypeList = new HashSet<FeatureType> { FeatureType.VerticalStructure };

            foreach (var airport in _toBeExportedIds[FeatureType.AirportHeliport])
            {
                var airportHeliport = CurrentDataContext.CurrentService.GetActualDataByDate(
                    new FeatureId { FeatureTypeId = (int)FeatureType.AirportHeliport, Guid = airport }, false, AiracDate).FirstOrDefault();

                if (airportHeliport == null)
                    continue;

                ExportFeatures(writer, report, airportHeliport.Data, processedIds);

                var obstacleAreas = DataProvider.GetReverseLinksTo(airportHeliport.Data, FeatureType.ObstacleArea, AiracDate);
                foreach (var obstacleArea in obstacleAreas)
                {
                    WriteFeatureWithReference(writer, obstacleArea.Feature, processedIds, report, verticalStructureTypeList);

                    if (!report.ContainsKey(FeatureType.ObstacleArea))
                        report.Add(FeatureType.ObstacleArea, 1);
                    else
                        report[FeatureType.ObstacleArea] = report[FeatureType.ObstacleArea] + 1;
                }

                var runways = DataProvider.GetReverseLinksTo(airportHeliport.Data, FeatureType.Runway, AiracDate);

                ExportFeatures(writer, report, runways, processedIds);

                foreach (var runway in runways)
                {
                    var runwayDirections = DataProvider.GetReverseLinksTo(runway, FeatureType.RunwayDirection, AiracDate);

                    ExportFeatures(writer, report, runwayDirections, processedIds);

                    foreach (var runwayDirection in runwayDirections)
                    {
                        obstacleAreas = DataProvider.GetReverseLinksTo(runwayDirection, FeatureType.ObstacleArea, AiracDate);
                        
                        foreach (var obstacleArea in obstacleAreas)
                        {
                            WriteFeatureWithReference(writer, obstacleArea.Feature, processedIds, report, verticalStructureTypeList);

                            if (!report.ContainsKey(FeatureType.ObstacleArea))
                                report.Add(FeatureType.ObstacleArea, 1);
                            else
                                report[FeatureType.ObstacleArea] = report[FeatureType.ObstacleArea] + 1;
                        }
                    }
                }
            }
        }

        private void ExportFeatures(XmlWriter writer, Dictionary<FeatureType, int> report, IList<AimFeature> features, SortedSet<Guid> processedIds)
        {
            features = FilterByInterpretation(features);
            features = RemoveDecomissioned(features, AiracDate);

            if (features.Count == 0)
                return;

            foreach (var feature in features.Select(x => x.Feature))
            {
                if (!processedIds.Add(feature.Identifier))
                    continue;

                AixmGenerator.Instance.WriteFeature(writer, feature);

                if (!report.ContainsKey(feature.FeatureType))
                    report.Add(feature.FeatureType, 1);
                else
                    report[feature.FeatureType]++;
            }
        }

        private void ExportFeatures(XmlWriter writer, Dictionary<FeatureType, int> report, AimFeature feature, SortedSet<Guid> processedIds)
        {
            ExportFeatures(writer, report, new List<AimFeature> { feature }, processedIds);
        }

        private void ExportSelectedFeatures(XmlWriter writer, Dictionary<FeatureType, int> report)
        {
            IList<ObstacleArea> obstacleAreas = GetObstacleAreas(AiracDate);

            foreach (var pair in _toBeExportedIds)
            {
                var count = 0;
                foreach (var id in pair.Value)
                {
                    var aimFeatures =
                        CurrentDataContext.CurrentService.GetActualDataByDate(
                            new FeatureId { FeatureTypeId = (int)pair.Key, Guid = id },
                            false, AiracDate, ConvertStateTypeToInterpretation(StateType));

                    if (aimFeatures != null && aimFeatures.IsNotEmpty())
                    {
                        aimFeatures = FilterByInterpretation(aimFeatures);
                        aimFeatures = RemoveDecomissioned(aimFeatures, AiracDate);
                        aimFeatures = RemoveNonObstacle(aimFeatures, obstacleAreas, AiracDate);

                        if (aimFeatures.IsNotEmpty())
                        {
                            count++;
                            MainManagerModel.Instance.StatusText =
                                "Exporting " + pair.Key +
                                ": " + count + " / " +
                                pair.Value.Count;

                            AixmGenerator.Instance.WriteFeature(writer, aimFeatures.First().Data.Feature);

                            if (!report.ContainsKey(pair.Key))
                                report.Add(pair.Key, 1);
                            else
                                report[pair.Key] = report[pair.Key] + 1;
                        }
                    }
                }
            }
        }

        private void ExportAllFeatures(XmlWriter writer, Dictionary<FeatureType, int> report)
        {
            foreach (var featureType in Enum.GetValues(typeof(FeatureType)))
            {
                ExportFeatureType(writer, report, featureType);
            }
        }




        private void ExportEAD(XmlWriter writer, Dictionary<FeatureType, int> report)
        {
            foreach (var featureType in _eadFeatureTypeList)
            {
                ExportFeatureType(writer, report, featureType);
            }
        }

        private void ExportFeatureType(XmlWriter writer, Dictionary<FeatureType, int> report, object featureType)
        {
            var count = 0;
            var data = CurrentDataContext.CurrentService.GetActualDataByDate(
                new FeatureId { FeatureTypeId = (int)featureType }, false,
                AiracDate, ConvertStateTypeToInterpretation(StateType));

            if (data != null && data.IsNotEmpty())
            {
                data = FilterByInterpretation(data);
                data = RemoveDecomissioned(data, AiracDate);
                data = RemoveNonObstacle(data, AiracDate);
                foreach (var item in data)
                {
                    count++;
                    MainManagerModel.Instance.StatusText =
                        "Exporting " + featureType + ": " + count + " / " +
                        data.Count;
                    AixmGenerator.Instance.WriteFeature(writer, item.Data.Feature);
                }

                if (data.IsNotEmpty())
                    report.Add((FeatureType)featureType, data.Count);
            }
        }

        private string CreateFileName()
        {
            string result = "TOSSM";
            switch (ExportFeaturesIndex)
            {
                case ExportEnum.AllFeatures:
                    result = $"{result} {StateType} {AiracDate:dd-MM-yyyy}";
                    break;
                case ExportEnum.EAD:
                    result = $"{result} EAD {StateType} {AiracDate:dd-MM-yyyy}";
                    break;
                case ExportEnum.SelectedFeatures:
                    result = $"{result} {StateType} {AiracDate:dd-MM-yyyy}";
                    result = _toBeExportedIds.Count(t => t.Value.IsNotEmpty()) == 1 ? $"{result} {_toBeExportedIds.First(t => t.Value.IsNotEmpty()).Key}" : $"{result} Selected Features";
                    break;
                case ExportEnum.SelectedWithReferences:
                    result = $"{result} {StateType} {AiracDate:dd-MM-yyyy}";
                    result = _toBeExportedIds.Count(t => t.Value.IsNotEmpty()) == 1 ? $"{result} {_toBeExportedIds.First(t => t.Value.IsNotEmpty()).Key}" : $"{result} Selected Features";
                    result = $"{result} With References";
                    break;
                case ExportEnum.AllStatesInRange:
                    result = $"{result} States {StateType} {RangeStartDate:dd-MM-yyyy} {RangeEndDate:dd-MM-yyyy}";
                    break;
                case ExportEnum.StatesOfSelectedInRange:
                    result = $"{result} States {StateType} {RangeStartDate:dd-MM-yyyy} {RangeEndDate:dd-MM-yyyy}";
                    result = _toBeExportedIds.Count(t => t.Value.IsNotEmpty()) == 1 ? $"{result} {_toBeExportedIds.First(t => t.Value.IsNotEmpty()).Key}" : $"{result} Selected Features";
                    break;
                case ExportEnum.AllEventsInRange:
                    result = $"{result} Events {EventType} {RangeStartDate:dd-MM-yyyy} {RangeEndDate:dd-MM-yyyy}";
                    break;
                case ExportEnum.EventsOfSelectedInRange:
                    result = $"{result} Events {EventType} {RangeStartDate:dd-MM-yyyy} {RangeEndDate:dd-MM-yyyy}";
                    result = _toBeExportedIds.Count(t => t.Value.IsNotEmpty()) == 1 ? $"{result} {_toBeExportedIds.First(t => t.Value.IsNotEmpty()).Key}" : $"{result} Selected Features";
                    break;

            }
            result = $"{result} {DateTime.Now:dd-MM-yyyy HH-mm}";
            return result;
        }

        private string CreateDateComment()
        {
            string result = "";
            switch (ExportFeaturesIndex)
            {
                case ExportEnum.AllFeatures:
                case ExportEnum.EAD:
                case ExportEnum.SelectedFeatures:
                case ExportEnum.SelectedWithReferences:
                    result = $"Effective Date: {AiracDate:dd-MM-yyyy}";
                    break;
                case ExportEnum.AllStatesInRange:
                case ExportEnum.StatesOfSelectedInRange:
                case ExportEnum.AllEventsInRange:
                case ExportEnum.EventsOfSelectedInRange:
                    result = $"Range:{RangeStartDate:dd-MM-yyyy} {RangeEndDate:dd-MM-yyyy}";
                    break;

            }
            result = $"{result}; Snapshot Date: {DateTime.Now:dd-MM-yyyy HH-mm}";
            return result;
        }

        public RelayCommand OpenSourceCommand
        {
            get
            {
                return _openSourceCommand ?? (_openSourceCommand = new RelayCommand(
                           t2 =>
                           {

                               AObjectListConfig.IgnoreNotes = false;
                               MemoryUtil.CompactLoh();

                               var dlg = new Microsoft.Win32.OpenFileDialog
                               {
                                   Title = "Open Snapshot",
                                   DefaultExt = ".xml",
                                   Filter = "Xml Files (*.xml)|*.xml|All Files|*.*"
                               };

                               if (dlg.ShowDialog() == true)
                               {
                                   DataPresenter.BlockerModel.BlockForAction(
                                       () =>
                                       {

                                           MainManagerModel.Instance.StatusText = "Analyzing file...";
                                           RelationsDataGridVisibility = Visibility.Visible;

                                           int count = 0;
                                           int totalCount = 0;


                                           _aimHelper.Open(dlg.FileName,
                                               () =>
                                               {
                                                   totalCount++;
                                                   MainManagerModel.Instance.StatusText = totalCount + " features loaded...";
                                               },
                                               () =>
                                               {
                                                   MainManagerModel.Instance.StatusText = "Cleaning memory...";
                                               },
                                               (aixmFeatureList, collection) =>
                                               {
                                                   foreach (Feature feature in aixmFeatureList)
                                                   {
                                                       var xml = aixmFeatureList.Xml(feature);
                                                       AddFeatureToExport(feature.FeatureType, feature.Identifier);
                                                       count++;
                                                       MainManagerModel.Instance.StatusText = "Processed " + count + " features from " + totalCount + "...";
                                                   }
                                                   collection.Clear();//clear memory
                                               });



                                           MainManagerModel.Instance.StatusText = "Cleaning memory...";
                                           MemoryUtil.CompactLoh();
                                           GC.WaitForPendingFinalizers();

                                           Reload();
                                           MainManagerModel.Instance.StatusText = _aimHelper.IsOpened ? "Done" : "Failed to open";
                                       });
                               }
                           }));
            }
        }


        public RelayCommand CreateBaselineCommand
        {
            get
            {
                return _createBaselineCommand ?? (_createBaselineCommand = new RelayCommand(
                t =>
                {
                    try
                    {
                        DataPresenter.BlockerModel.BlockForAction(
                            CreateBaseLine);
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

                },
                t => _toBeExportedIds.IsNotEmpty()));
            }
        }

        private void CreateBaseLine()
        {
            var processedIds = new SortedSet<Guid>();
            IList<ObstacleArea> obstacleAreas = GetObstacleAreas(AiracDate);

            foreach (var pair in _toBeExportedIds)
            {
                var count = 0;
                foreach (var id in pair.Value)
                {
                    if (!processedIds.Add(id))
                    {
                        count++;
                        MainManagerModel.Instance.StatusText =
                            "Processing " + pair.Key + ": " + count + " / " +
                            pair.Value.Count;
                        continue;
                    }
                    var aimFeatures = CurrentDataContext.CurrentService.GetActualDataByDate(
                        new FeatureId { FeatureTypeId = (int)pair.Key, Guid = id },
                        false, AiracDate, Interpretation.BaseLine);

                    if (aimFeatures != null && aimFeatures.IsNotEmpty())
                    {
                        aimFeatures = RemoveDecomissioned(aimFeatures, AiracDate);
                        aimFeatures = RemoveNonObstacle(aimFeatures, obstacleAreas, AiracDate);
                        if (aimFeatures.IsNotEmpty())
                        {
                            count++;
                            var feature = aimFeatures.First().Data.Feature;
                            var aimPropInfoArr = AimMetadata.GetAimPropInfos(feature);
                            //add Aim properties
                            foreach (var aimPropInfo in aimPropInfoArr)
                            {
                                if (String.IsNullOrEmpty(aimPropInfo.AixmName)) continue;
                                if (aimPropInfo.AixmName.ToLower().Equals("identifier")) continue;
                                if (aimPropInfo.AixmName.ToLower().Equals("timeslice")) continue;

                                if (aimPropInfo.AixmName.ToLower() == "annotation")
                                {
                                    var annotations =
                                        (feature as IAimObject).GetValue(aimPropInfo.Index) as IList<Note>;

                                    var note = new Note
                                    {
                                        Purpose = CodeNotePurpose.REMARK,
                                        TranslatedNote =
                                        {
                                            new LinguisticNote
                                            {
                                                Note = new TextNote
                                                {
                                                    Value =
                                                        "TRACEABILITY: transition to EAD SDD (AIXM 5.1)",
                                                    Lang = language.ENG
                                                }
                                            }
                                        }
                                    };

                                    if (annotations == null)
                                    {
                                        annotations = AimObjectFactory.CreateAimProperty(aimPropInfo) as IList<Note>;
                                        annotations.Add(note);
                                        (feature as IAimObject).SetValue(aimPropInfo.Index, annotations as IAimProperty);
                                    }
                                    else
                                        annotations.Add(note);
                                }
                                else
                                {
                                    (feature as IAimObject).SetValue(aimPropInfo.Index, null);
                                }
                            }


                            if (feature.TimeSlice == null) feature.TimeSlice = new TimeSlice();
                            else
                                feature.TimeSlice.CorrectionNumber = 0;

                            if (feature.TimeSlice.ValidTime == null)
                            {
                                feature.TimeSlice.ValidTime = new TimePeriod();
                            }
                            feature.TimeSlice.Interpretation = TimeSliceInterpretationType.PERMDELTA; ;
                            feature.TimeSlice.ValidTime.BeginPosition = AiracDate;
                            feature.TimeSlice.ValidTime.EndPosition = null;



                            CommonDataProvider.CommitAsNewSequence(feature);
                        }
                    }
                }
            }
            Reload();
            MainManagerModel.Instance.StatusText = "Done";
        }

        private IList<AimFeature> RemoveDecomissioned(IList<AimFeature> data, DateTime endDateTime)
        {
            var errorData = data.Where(f => f.Feature.TimeSlice.FeatureLifetime == null);
            var errorList = errorData as IList<AimFeature> ?? errorData.ToList();
            if (errorList.IsNotEmpty())
            {
                _featureErrors.AddRange(errorList.Select(t => new FeatureError
                {
                    Message = "Feature has a null feature life time.",
                    Type = ErrorType.NullReference,
                    FeatureType = t.FeatureType,
                    Data = { t.Feature }
                }));
            }

            if (ExportDecommissioned) return data;

            data =
               data.Where(
                   f => f.Feature.TimeSlice.FeatureLifetime?.EndPosition == null || f.Feature.TimeSlice.FeatureLifetime.EndPosition.Value > endDateTime).ToList();
            return data;
        }

        private IList<AbstractState<AimFeature>> RemoveDecomissioned(IList<AbstractState<AimFeature>> data, DateTime endDateTime)
        {
            var errorData = data.Where(f => f.Data.Feature.TimeSlice.FeatureLifetime == null);
            var errorList = errorData as IList<AbstractState<AimFeature>> ?? errorData.ToList();
            if (errorList.IsNotEmpty())
            {
                _featureErrors.AddRange(errorList.Select(t => new FeatureError
                {
                    Message = "Feature has a null feature life time.",
                    Type = ErrorType.NullReference,
                    FeatureType = t.Data.FeatureType,
                    Data = { t.Data.Feature }
                }));
            }

            if (ExportDecommissioned) return data;

            data =
               data.Where(
                   f => f.Data.Feature.TimeSlice.FeatureLifetime?.EndPosition == null || f.Data.Feature.TimeSlice.FeatureLifetime.EndPosition.Value > endDateTime).ToList();
            return data;
        }

        private IList<AbstractState<AimFeature>> FilterByInterpretation(IList<AbstractState<AimFeature>> data)
        {
            if (StateType == StatesEnum.Temporary)
                return data.Where(t => t.Data?.Feature?.TimeSlice.Interpretation == TimeSliceInterpretationType.TEMPDELTA).ToList();
            return data;
        }

        private IList<AimFeature> FilterByInterpretation(IList<AimFeature> data)
        {
            if (StateType == StatesEnum.Temporary)
                return data.Where(t => t.Feature?.TimeSlice.Interpretation == TimeSliceInterpretationType.TEMPDELTA).ToList();
            return data;
        }

        private bool IsObstacle(IList<ObstacleArea> areas, Guid vs)
        {
            foreach (var area in areas)
            {
                if (area.Obstacle != null)
                {
                    if (area.Obstacle.Any(t => t.Feature.Identifier.Equals(vs)))
                        return true;
                }
            }
            return false;
        }

        protected virtual IList<ObstacleArea> GetObstacleAreas(DateTime airacDate)
        {
            IList<ObstacleArea> obstacleAreas = new List<ObstacleArea>();
            if (ExportObstacles)
            {
                var featureList = CurrentDataContext.CurrentService.GetActualDataByDate(
                    new FeatureId { FeatureTypeId = (int)FeatureType.ObstacleArea }, false, airacDate);

                if (featureList != null)
                    obstacleAreas = featureList.Select(t => (ObstacleArea)t.Data.Feature).Where(
                        f => f.TimeSlice.FeatureLifetime != null && (f.TimeSlice.FeatureLifetime.EndPosition == null || f.TimeSlice.FeatureLifetime.EndPosition.Value > airacDate)).ToList();
            }
            return obstacleAreas;
        }

        private IList<AbstractState<AimFeature>> RemoveNonObstacle(IList<AbstractState<AimFeature>> data, DateTime airacDate)
        {
            if (!ExportObstacles)
                return data;

            var obstacleAreas = GetObstacleAreas(airacDate);

            return data
                .Where(
                    t => t.Data.FeatureType != FeatureType.VerticalStructure || IsObstacle(obstacleAreas, t.Data.Identifier))
                .ToList();
        }

        private IList<AbstractState<AimFeature>> RemoveNonObstacle(IList<AbstractState<AimFeature>> data, IList<ObstacleArea> obstacleAreas, DateTime airacDate)
        {
            if (!ExportObstacles)
                return data;


            return data
                .Where(
                    t => t.Data.FeatureType != FeatureType.VerticalStructure || IsObstacle(obstacleAreas, t.Data.Identifier))
                .ToList();
        }

        private void WriteFeatureWithReference(XmlWriter writer, Feature feature, SortedSet<Guid> processedIds, Dictionary<FeatureType, int> report, HashSet<FeatureType> featureTypes = null)
        {
            //add feature itself
            AixmGenerator.Instance.WriteFeature(writer, feature);

            //get links
            var links = new List<RefFeatureProp>();
            AimMetadataUtility.GetReferencesFeatures(feature, links);

            //add links
            foreach (var link in links)
            {
                if (featureTypes != null && !featureTypes.Contains(link.FeatureType))
                    continue;

                var id = link.RefIdentifier;
                if (!processedIds.Add(id)) continue;

                var linkedFeatures = CurrentDataContext.CurrentService.GetActualDataByDate(
                                                  new FeatureId { FeatureTypeId = (int)link.FeatureType, Guid = id }, false, AiracDate, ConvertStateTypeToInterpretation(StateType));
                if (linkedFeatures != null && linkedFeatures.Count > 0)
                {
                    WriteFeatureWithReference(writer, linkedFeatures.First().Data.Feature, processedIds, report, featureTypes);
                    var featureType = linkedFeatures.First().Data.Feature.FeatureType;
                    if (!report.ContainsKey(featureType))
                        report.Add(featureType, 1);
                    else
                        report[featureType] = report[featureType] + 1;
                }
            }


        }

        public RelayCommand ClearCommand
        {
            get
            {
                return _clearCommand ?? (_clearCommand = new RelayCommand(
                    t => { Clear(); },
                    t => ExportFeaturesIndex != 0 || RelationsData.Count > 0));
            }
        }

        private void Clear()
        {
            RelationsData.Clear();
            _toBeExportedIds.Clear();
            SelectedRelation = null;
            Reload();
        }

        public void Reload()
        {
            ReloadData(DataPresenter);
        }

        public RelayCommand RemoveCommand
        {
            get
            {
                return _removeCommand ?? (_removeCommand = new RelayCommand(
              t =>
              {
                  if (DataPresenter.SelectedFeatures != null)
                  {
                      foreach (var item in DataPresenter.SelectedFeatures)
                      {
                          var wrapper = item as ReadonlyFeatureWrapper;
                          if (wrapper == null)
                          {
                              return;
                          }

                          RemoveFeatureToExport(wrapper.Feature.FeatureType, wrapper.Feature.Identifier);
                      }
                  }
                  else
                  {
                      var wrapper = DataPresenter.SelectedFeature as ReadonlyFeatureWrapper;
                      if (wrapper == null)
                      {
                          return;
                      }

                      RemoveFeatureToExport(wrapper.Feature.FeatureType, wrapper.Feature.Identifier);
                  }

                  ReloadData(DataPresenter);
              },
              t => DataPresenter.SelectedFeature != null));
            }
        }

        #region Custom DataSet

        private Visibility _dataSetConfigurationsVisibility = Visibility.Hidden;
        public Visibility DataSetConfigurationsVisibility
        {
            get => _dataSetConfigurationsVisibility;
            set
            {
                _dataSetConfigurationsVisibility = value;
                if (value == Visibility.Visible)
                    LoadDataSetConfigurationLists();
                OnPropertyChanged("DataSetConfigurationsVisibility");
            }
        }

        private void LoadDataSetConfigurationLists()
        {
            var list = CommonDataProvider.GetFeatureDependencies();
            DataSetConfigurations = new ObservableCollection<FeatureDependencyConfiguration>(list);
            SelectedDataSetConfiguration = DataSetConfigurations.FirstOrDefault();
            if (SelectedDataSetConfiguration == null)
                MessageBoxHelper.Show("No Data Sets. Please create new configuration", "Warning", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private FeatureDependencyConfiguration _selectedDataSetConfiguration;
        public FeatureDependencyConfiguration SelectedDataSetConfiguration
        {
            get => _selectedDataSetConfiguration;
            set
            {
                _selectedDataSetConfiguration = value;
                OnPropertyChanged("SelectedDataSetConfiguration");
            }
        }

        private ObservableCollection<FeatureDependencyConfiguration> _dataSetConfigurations = new ObservableCollection<FeatureDependencyConfiguration>();
        public ObservableCollection<FeatureDependencyConfiguration> DataSetConfigurations
        {
            get => _dataSetConfigurations;
            set
            {
                _dataSetConfigurations = value;
                OnPropertyChanged("DataSetConfigurations");
            }
        }

        #endregion
    }

    public class StringWriterWithEncoding : StringWriter
    {
        private readonly Encoding _encoding;

        public StringWriterWithEncoding()
        {
        }

        public StringWriterWithEncoding(IFormatProvider formatProvider)
            : base(formatProvider)
        {
        }

        public StringWriterWithEncoding(StringBuilder sb)
            : base(sb)
        {
        }

        public StringWriterWithEncoding(StringBuilder sb, IFormatProvider formatProvider)
            : base(sb, formatProvider)
        {
        }


        public StringWriterWithEncoding(Encoding encoding)
        {
            _encoding = encoding;
        }

        public StringWriterWithEncoding(IFormatProvider formatProvider, Encoding encoding)
            : base(formatProvider)
        {
            _encoding = encoding;
        }

        public StringWriterWithEncoding(StringBuilder sb, Encoding encoding)
            : base(sb)
        {
            _encoding = encoding;
        }

        public StringWriterWithEncoding(StringBuilder sb, IFormatProvider formatProvider, Encoding encoding)
            : base(sb, formatProvider)
        {
            _encoding = encoding;
        }

        public override Encoding Encoding => _encoding ?? base.Encoding;
    }
}


