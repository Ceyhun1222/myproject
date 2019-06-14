using Aran.Aim.Data;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.Metadata.ISO;
using Aran.Geometries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Aran.Metadata.Utils;
using Aran.Temporality.Common.Extensions;
using Aran.Temporality.Common.Logging;

namespace Aran.Metadata.Forms.ViewModels
{
    public class MetadataViewModel : ViewModel
    {
        #region :> Fields
        private List<MdMetadata> _metadataList;
        private Dictionary<string, List<string>> _accuracyDictionary;
        #endregion

        #region :> Properties
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        private MdMetadata _metadata;
        public MdMetadata Metadata
        {
            get => _metadata;
            set
            {
                _metadata = value;
                NotifyPropertyChanged("Metadata");
            }
        }

        private MdDataIdentification DataIdentification => _metadata.IdentificationInfo.First().Value as MdDataIdentification;

        private CiResponsibleParty OperatorInfo => DataIdentification.PointOfContact.FirstOrDefault(pc => pc.RoleCode == CiRoleCode.PointOfContact);

        public string Name
        {
            get => OperatorInfo.IndividualName;
            set { }
        }
        
        public string Email
        {
            get { return OperatorInfo.ContactInfo.Address.ElectronicMailAddress.First().Value; }
            set
            {
            }
        }
        
        public string Position
        {
            get { return OperatorInfo.PositionName; }
            set
            {
            }
        }
        
        public string Country
        {
            get { return OperatorInfo.ContactInfo.Address.Country; }
            set
            {
            }
        }
        
        public string City
        {
            get { return OperatorInfo.ContactInfo.Address.City; }
            set
            {
            }
        }
        
        public string Address
        {
            get { return OperatorInfo.ContactInfo.Address.DeliveryPoint.First().Value; }
            set
            {
            }
        }
        
        public string Zip
        {
            get { return OperatorInfo.ContactInfo.Address.PostalCode; }
            set
            {
            }
        }
        
        public string FaxNumber
        {
            get { return OperatorInfo.ContactInfo.Phone.Voice.First().Value; }
            set
            {
            }
        }
        
        public string Organization
        {
            get { return OperatorInfo.OrganizationName; }
            set
            {
            }
        }

        public DateTime InteractionDate
        {
            get { return DateTime.Now; }
            set
            {
            }
        }

        public string Amendments
        {
            get { return DataIdentification.Citation.Date.First().DateType.Value.ToString() + ":"; }
            set
            {
            }
        }

        public DateTime? EffectiveDateTime
        {
            get { return ((DataIdentification.Extent.First().TemporalElement.First() as ExTemporalExtent).Extent as BtTimePeriod).BeginPosition.Value; }
            set
            {
            }
        }

        public string CoordinateSystem
        {
            get { return _metadata.ReferenceSystemInfo.First().Code.Remove(0, 16); }
            set
            {
            }
        }

        private string _verificationValidation;
        public string VerificationValidation
        {
            get { return _verificationValidation; }
            set
            {
                _verificationValidation = value;
                NotifyPropertyChanged("VerificationValidation");
            }
        }
        
        private string _confidenceLevel;
        public string ConfidenceLevel
        {
            get
            {
                if (_confidenceLevel == null)
                    _confidenceLevel = ConfidenceLevelList.FirstOrDefault();
                
                return _confidenceLevel;
            }
            set
            {
                _confidenceLevel = value;
                NotifyPropertyChanged("ConfidenceLevel");
            }
        }

        private string _detailsFunctionsApplied;
        public string DetailsFunctionsApplied
        {
            get { return _detailsFunctionsApplied; }
            set
            {
                _detailsFunctionsApplied = value;
                NotifyPropertyChanged("DetailsFunctionsApplied");
            }
        }

        private string _detailsLimitations;
        public string DetailsLimitations
        {
            get { return _detailsLimitations; }
            set
            {
                _detailsLimitations = value;
                NotifyPropertyChanged("DetailsLimitations");
            }
        }
        
        public string Accuracy
        {
            get
            {
                if (AccuracyListIsVisible == Visibility.Visible)
                {
                    var accuracyReport = _metadataList.SelectMany(metadata => metadata?.DataQualityInfo?.SelectMany(
                      dataQuality => dataQuality?.Report?.Where(
                          report =>
                            report?.Value?.NameOfMeasure != null &&
                            report.Value.NameOfMeasure.Any(measure => measure?.Value == "Accuracy") &&
                            report.Value.MeasureIdentification.Code == _currentAccuracyLegType + "&" + _currentAccuracyRole
                            ))).FirstOrDefault();

                    var accuracyResult = accuracyReport?.Value?.Result?.FirstOrDefault()?.Value as DqQuantitativeResult;

                    if (accuracyResult?.Value?.FirstOrDefault()?.Value != null)
                        return accuracyResult.Value.First().Value + " m";
                } 
                

                return "";
            }
            set
            {
            }
        }

        public string AccuracyDescription
        {
            get
            {
                if (AccuracyListIsVisible == Visibility.Visible)
                {
                    var accuracyReport = _metadataList.SelectMany(metadata => metadata?.DataQualityInfo?.SelectMany(
                      dataQuality => dataQuality?.Report?.Where(
                          report =>
                            report?.Value?.NameOfMeasure != null &&
                            report.Value.NameOfMeasure.Any(measure => measure?.Value == "Accuracy") &&
                            report.Value.MeasureIdentification.Code == _currentAccuracyLegType + "&" + _currentAccuracyRole
                            ))).FirstOrDefault();

                    if (accuracyReport?.Value?.MeasureDescription != null)
                        return accuracyReport?.Value?.MeasureDescription;
                }

                return "";
            }
            set
            {
            }
        }

        private string _accuracySingle;
        public string AccuracySingle
        {
            get { return _accuracySingle; }
            set
            {
                _accuracySingle = value;
                NotifyPropertyChanged("AccuracySingle");
            }
        }

        private string _resolutionSingle;
        public string ResolutionSingle
        {
            get { return _resolutionSingle; }
            set
            {
                _resolutionSingle = value;
                NotifyPropertyChanged("ResolutionSingle");
            }
        }

        private ObservableCollection<string> _accuracyLegTypeList = new ObservableCollection<string>();
        public ObservableCollection<string> AccuracyLegTypeList
        {
            get => _accuracyLegTypeList;
            set
            {
                _accuracyLegTypeList = value;
                NotifyPropertyChanged("AccuracyLegTypeList");
            }
        }

        private ObservableCollection<string> _accuracyRoleList = new ObservableCollection<string>();
        public ObservableCollection<string> AccuracyRoleList
        {
            get => _accuracyRoleList;
            set
            {
                _accuracyRoleList = value;
                NotifyPropertyChanged("AccuracyRoleList");
                NotifyPropertyChanged("Accuracy");
                NotifyPropertyChanged("AccuracyDescription");
            }
        }
        
        private ObservableCollection<User> _originatorList = new ObservableCollection<User>();
        public ObservableCollection<User> OriginatorList
        {
            get => _originatorList;
            set
            {
                _originatorList = value;
                NotifyPropertyChanged("OriginatorList");
            }
        }

        private string _currentAccuracyLegType;
        public string CurrentAccuracyLegType
        {
            get => _currentAccuracyLegType;
            set
            {
                _currentAccuracyLegType = value;

                if (_accuracyDictionary.ContainsKey(_currentAccuracyLegType))
                    AccuracyRoleList = new ObservableCollection<string>(_accuracyDictionary[_currentAccuracyLegType]);
                else
                    AccuracyRoleList = new ObservableCollection<string>();

                CurrentAccuracyRole = null;

                NotifyPropertyChanged("CurrentAccuracyLegType");
                NotifyPropertyChanged("AccuracyRoleList");
                NotifyPropertyChanged("CurrentAccuracyRole");
            }
        }

        private string _currentAccuracyRole;
        public string CurrentAccuracyRole
        {
            get => _currentAccuracyRole;
            set
            {
                _currentAccuracyRole = value;
                NotifyPropertyChanged("CurrentAccuracyRole");
                NotifyPropertyChanged("Accuracy");
                NotifyPropertyChanged("AccuracyDescription");
            }
        }

        private User _selectedOriginator;
        public User SelectedOriginator
        {
            get => _selectedOriginator;
            set
            {
                _selectedOriginator = value;
                NotifyPropertyChanged("SelectedOriginator");
            }
        }

        private Visibility _accuracyListIsVisible = Visibility.Collapsed;
        public Visibility AccuracyListIsVisible {
            get => _accuracyListIsVisible;
            set
            {
                _accuracyListIsVisible = value;
                NotifyPropertyChanged("AccuracyListIsVisible");
            }
        }

        private Visibility _accuracySingleIsVisible = Visibility.Collapsed;
        public Visibility AccuracySingleIsVisible
        {
            get => _accuracySingleIsVisible;
            set
            {
                _accuracySingleIsVisible = value;
                NotifyPropertyChanged("AccuracySingleIsVisible");
            }
        }

        private readonly List<string> _confidenceLevelList = new List<string> {"90%", "95%"};

        public IEnumerable<string> ConfidenceLevelList => _confidenceLevelList;
        #endregion

        #region :> Ctor
        public MetadataViewModel(List<MdMetadata> metadataList, List<User> originators)
        {
            LogManager.GetLogger(typeof(MetadataViewModel)).Debug($"Metadata View Model constructor. Metadata count: " +
                        $"{metadataList?.Count}, Originators count: {originators?.Count}");

            _metadataList = metadataList;
            _metadata = _metadataList?.FirstOrDefault();

            OriginatorList = new ObservableCollection<User>(originators);

            SaveCommand = new RelayCommand(SaveCommand_onClick);
            CancelCommand = new RelayCommand(Cancel_onClick);
        }
        #endregion

        #region :> Methods
        public void Clear()
        {
        }

        internal void Init()
        {
            var measures = _metadataList?.SelectMany(metadata => metadata?.DataQualityInfo?.SelectMany(
               dataQuality => dataQuality?.Report?.Select(
                   report => report?.Value)));

            if (measures?.Any() == true)
            {
                var singleNumericalDataType = (from measure in measures
                                               where measure?.MeasureIdentification?.Code != null
                                               select measure)?.Any() != true;

                if (singleNumericalDataType)
                {
                    LogManager.GetLogger(typeof(MetadataViewModel)).Debug($"Metadata View Model for accuracy/resolution(Delta). Measures count: " +
                                                        $"{measures.Count()}, Measures: {measures.Dump()}");
                    foreach (var measure in measures)
                    {
                        if (measure == null)
                            continue;

                        AccuracySingleIsVisible = Visibility.Visible;
                        if (measure?.NameOfMeasure?.FirstOrDefault()?.Value == "Accuracy")
                        {
                            var accuracyResult = measure?.Result?.FirstOrDefault()?.Value as DqQuantitativeResult;
                            LogManager.GetLogger(typeof(MetadataViewModel)).Debug($"Metadata View Model accuracy value. " +
                                                                                  $"Accuracy: {accuracyResult.Dump()}");

                            if (accuracyResult?.Value?.FirstOrDefault()?.Value != null)
                                AccuracySingle = accuracyResult?.Value?.FirstOrDefault()?.Value;
                        }
                        else if (measure?.NameOfMeasure?.FirstOrDefault()?.Value == "Resolution")
                        {
                            var resolutionResult = measure?.Result?.FirstOrDefault()?.Value as DqQuantitativeResult;
                            LogManager.GetLogger(typeof(MetadataViewModel)).Debug($"Metadata View Model resolution value. " +
                                                                                  $"Resolution: {resolutionResult.Dump()}");

                            if (resolutionResult?.Value?.FirstOrDefault()?.Value != null)
                                ResolutionSingle = resolutionResult?.Value?.FirstOrDefault()?.Value;
                        }
                    }
                }
                else
                {
                    LogManager.GetLogger(typeof(MetadataViewModel)).Debug($"Metadata View Model for accuracy(Panda). Measures: " +
                                                                          $"{measures.Count()}, Originators: {measures.Dump()}");

                    AccuracyListIsVisible = Visibility.Visible;
                    _accuracyDictionary = new Dictionary<string, List<string>>();
                    var accuracyLegTypeList = new List<string>();

                    foreach (var measure in measures)
                    {
                        if (measure == null)
                            continue;


                        var measureInfo = measure?.MeasureIdentification?.Code?.Split('&');
                        if (measureInfo != null && measureInfo.Length >= 2)
                        {
                            var legType = measureInfo[0] ?? "";
                            var role = measureInfo[1] ?? "";
                            LogManager.GetLogger(typeof(MetadataViewModel)).Debug($"Metadata View Model fix value. " +
                                                                                  $"Leg type: {legType}, " +
                                                                                  $"Role: {role}");

                            if (measure?.NameOfMeasure?.FirstOrDefault()?.Value == "Accuracy")
                            {
                                if (_accuracyDictionary.ContainsKey(legType))
                                {
                                    _accuracyDictionary[legType].Add(role);
                                }
                                else
                                {
                                    _accuracyDictionary[legType] = new List<string>
                                    {
                                        role
                                    };
                                }
                            }
                        }
                    }

                    AccuracyLegTypeList = new ObservableCollection<string>();

                    foreach (KeyValuePair<string, List<string>> entry in _accuracyDictionary)
                        AccuracyLegTypeList.Add(entry.Key);

                    CurrentAccuracyLegType = AccuracyLegTypeList.FirstOrDefault();
                }
            }
        }

        private void SaveCommand_onClick(object obj)
        {
            if (SelectedOriginator == null)
            {
                MessageBox.Show("Please select originator to proceed work", "Warning");
                return;
            }


            if (_metadataList != null)
            {
                foreach (var metadata in _metadataList)
                {
                    #region Originator info
                    LogManager.GetLogger(typeof(MetadataViewModel)).Debug($"Set Metadata Identification Info: " +
                                                                          $"Selected originator: {SelectedOriginator.Dump()}");

                    metadata.SetMetadataIdentificationInfo(SelectedOriginator, CiRoleCode.Originator);
                    #endregion

                    #region Data quality info
                    LogManager.GetLogger(typeof(MetadataViewModel)).Debug($"Set data quality info");

                    var dataQualityInfo = metadata.DataQualityInfo.First();
                    dataQualityInfo.Lineage = new LiLineage();

                    var processStep = new LiProcessStep
                    {
                        Description = DetailsFunctionsApplied,
                        Rationale = VerificationValidation,
                        DateTime = DateTime.Now
                    };

                    processStep.Processor.Add(metadata.IdentificationInfo.First().Value.PointOfContact
                        .Where(pc => pc.RoleCode == CiRoleCode.Originator).FirstOrDefault());
                    
                    dataQualityInfo.Lineage.ProcessStep.Add(processStep);

                    #endregion

                    #region Limitations
                    LogManager.GetLogger(typeof(MetadataViewModel)).Debug($"Set limitations");

                    var constraints = new MdConstraintsObject
                    {
                        Value = new MdLegalConstraints()
                    };

                    constraints.Value.UseLimitation.Add(new BtString
                    {
                        Value = DetailsLimitations
                    });

                    metadata.IdentificationInfo.First().Value.ResourceConstraints.Add(constraints);

                    #endregion

                    #region Confidence level
                    LogManager.GetLogger(typeof(MetadataViewModel)).Debug($"Set confidence level");

                    var confidenceObject = new DqAbstractElementObject { Value = new DqQuantitativeAttributeAccuracy() };

                    var confidenceResult = new DqQuantitativeResult();
                    confidenceResult.Value.Add(new BtString { Value = ConfidenceLevel });

                    confidenceObject.Value.NameOfMeasure.Add(new BtString { Value = "Confidence level" });

                    confidenceObject.Value.Result.Add(new DqAbstractResultObject { Value = confidenceResult });

                    dataQualityInfo.Report.Add(confidenceObject);

                    #endregion
                }
            }

            Close();
        }

        private void Cancel_onClick(object obj)
        {
            Close();
        }
        #endregion
    }
}
