using System;
using Aerodrome.Metadata;
using AerodromeManager.AmdbService;
using AmdbManager;
using GalaSoft.MvvmLight;

namespace ChartManagerTools.ViewModel
{
    public class InfoViewModel : ViewModelBase
    {
        private string _name;
        private string _canPublish;
        private string _chartType;
        private string _publishedBy;
        private string _publishedAt;
        private string _beginEffectiveDate;
        private string _endEffectiveDate;
        private string _org;
        private string _airport;
        private string _rwyDir;
        private string _locked;
        private string _lockedBy;
        private int _version;
        private string _messageStatus;

        public EventHandler Close;

        public InfoViewModel(AmdbMetadata amdb, string msg)
        {
            Name = amdb.Name;
            CanPublish = ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.IsReadOnly ? "No" : "Yes";            
            PublishedBy = $"{amdb.CreatedBy.FirstName} {amdb.CreatedBy.LastName}";
            PublishedAt = amdb.CreatedAt.ToShortDateString();           
            Organisation = amdb.Organization;
            Airport = amdb.Airport;            
            Locked = amdb.IsLocked ? "Yes" : "No";
            LockedBy = $"{amdb.LockedBy?.FirstName} {amdb.LockedBy?.LastName}";
            Version =Int32.Parse(amdb.Version);
            MessageStatus = msg;
        }

        public string MessageStatus
        {
            get => _messageStatus;
            set => Set(ref _messageStatus, value);
        }


        public int Version
        {
            get => _version;
            set => Set(ref _version, value);
        }

        public string LockedBy
        {
            get => _lockedBy;
            set => Set(ref _lockedBy, value);
        }

        public string Locked
        {
            get => _locked;
            set => Set(ref _locked, value);
        }

        public string RwyDir
        {
            get => _rwyDir;
            set => Set(ref _rwyDir, value);
        }

        public string Airport
        {
            get => _airport;
            set => Set(ref _airport, value);
        }

        public string Organisation
        {
            get => _org;
            set => Set(ref _org, value);
        }

        public string EndEffectiveDate
        {
            get => _endEffectiveDate;
            set => Set(ref _endEffectiveDate, value);
        }

        public string BeginEffectiveDate
        {
            get => _beginEffectiveDate;
            set => Set(ref _beginEffectiveDate, value);
        }

        public string PublishedAt
        {
            get => _publishedAt;
            set => Set(ref _publishedAt, value);
        }

        public string PublishedBy
        {
            get => _publishedBy;
            set => Set(ref _publishedBy, value);
        }

        public string ChartType
        {
            get => _chartType;
            set => Set(ref _chartType, value);
        }

        public string CanPublish
        {
            get => _canPublish;
            set => Set(ref _canPublish, value);
        }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }
    }
}