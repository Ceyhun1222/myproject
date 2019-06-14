using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace ChartCompare
{
    public enum Status
    {
        New,
        Changed,
        Deleted,
        Missing
    }

    public class DetailedItem : ViewModelBase
    {
        private bool _isChecked;

        public DetailedItem()
        {
            FieldLogList = new List<FieldLog>();
            IsChecked = true;
        }

        public DetailedItem(PDM.PDMObject feat, string description, Status status)
            : this()
        {
            Feature = feat;
            Name = description;
            ChangedStatus = status;
        }

        public bool IsChecked
        {
            get => _isChecked;
            set => Set(ref _isChecked, value);
        }

        public PDM.PDMObject Feature { get; set; }

        public string Name { get; set; }
        public List<FieldLog> FieldLogList { get; set; }
        public Status ChangedStatus { get; set; }
        public string Id => Feature?.ID ?? string.Empty;

        public override string ToString()
        {
            return this.Name + " (" + this.ChangedStatus.ToString() + ")";
        }
    }
}
