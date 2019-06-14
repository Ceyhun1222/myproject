using System;
using ChartManager;
using ChartManager.ChartServices;
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
        private string _version;
        private string _messageStatus;

        public EventHandler Close;

        public InfoViewModel(Chart chart, string msg)
        {
            Name = chart.Name;
            CanPublish = EsriExtensionData.IsReadOnly ? "No" : "Yes";
            ChartType = chart.Type.ToString();
            PublishedBy = $"{chart.CreatedBy.FirstName} {chart.CreatedBy.LastName}";
            PublishedAt = chart.CreatedAt.ToShortDateString();
            BeginEffectiveDate = chart.BeginEffectiveDate.ToShortDateString();
            EndEffectiveDate = chart.EndEffectiveDate?.ToShortDateString();
            Organisation = chart.Organization;
            Airport = chart.Airport;
            RwyDir = chart.RunwayDirection;
            Locked = chart.IsLocked ? "Yes" : "No";
            LockedBy = $"{chart.LockedBy?.FirstName} {chart.LockedBy?.LastName}";
            Version = chart.Version;
            MessageStatus = msg;
        }

        public string MessageStatus
        {
            get => _messageStatus;
            set => Set(ref _messageStatus, value);
        }


        public string Version
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