using Aran.Temporality.Common.Entity;
using Aran.Temporality.CommonUtil.Util;
using MvvmCore;

namespace TOSSM.ViewModel.Document.Slot
{

    public class OverviewViewModel : ViewModelBase
    {
        public static string IsOk = "check";
        public static string IsIssue = "warning";


        public ReportType ReportType { get; set; }

        private string _pictureId = IsOk;
        public string PictureId
        {
            get => _pictureId;
            set
            {
                _pictureId = value;
                OnPropertyChanged("PictureId");
            }
        }

    

        public string Name { get; set; }


        private string _description;

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        public string Date { get; set; }

        public OverviewViewModel()
        {
        }



        public OverviewViewModel(ProblemReport report)
        {

        }

    }

    public class SlotValidationOverviewViewModel : ViewModelBase
    {

        private string _moreDetailedMessage;
        public string MoreDetailedMessage
        {
            get => _moreDetailedMessage;
            set
            {
                _moreDetailedMessage = value;
                OnPropertyChanged("MoreDetailedMessage");
            }
        }

        private MtObservableCollection<OverviewViewModel> _data;

        public MtObservableCollection<OverviewViewModel> Data => _data??(_data=new MtObservableCollection<OverviewViewModel>());
    }
}
