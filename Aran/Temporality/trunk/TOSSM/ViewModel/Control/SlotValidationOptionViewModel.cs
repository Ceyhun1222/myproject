using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Util;
using Aran.Temporality.CommonUtil.Util;
using MvvmCore;

namespace TOSSM.ViewModel.Control
{
    public class DatasetViewModel : ViewModelBase
    {
        private bool _isChecked;

        public DatasetViewModel()
        {
        }

        public DatasetViewModel(FeatureDependencyConfiguration configuration)
        {

            Template = configuration.DataSourceTemplate==null?"":configuration.DataSourceTemplate.Name;
            Name = configuration.Name+" ("+((Aran.Aim.FeatureType)configuration.RootFeatureType)+")";
            Id = configuration.Id;
        }

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        public string Template { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class SlotValidationOptionViewModel : ViewModelBase
    {
         private ICollectionView _datasetsFiltered;
        public ICollectionView DatasetsFiltered
        {
            get
            {
                if (_datasetsFiltered == null)
                {
                    _datasetsFiltered = CollectionViewSource.GetDefaultView(Datasets.OrderBy(x => x.Template).ThenBy(x => x.Name));
                    _datasetsFiltered.GroupDescriptions.Clear();
                    _datasetsFiltered.GroupDescriptions.Add(new PropertyGroupDescription("Template"));
                }
                return _datasetsFiltered;
            }
            set => _datasetsFiltered = value;
        }


        public MtObservableCollection<DatasetViewModel> Datasets => _datasets??(_datasets=new MtObservableCollection<DatasetViewModel>());


        public RelayCommand AllCommand
        {
            get { return _allCommand??(_allCommand=new RelayCommand(
                t =>
                {
                    foreach (var dataset in Datasets)
                    {
                        dataset.IsChecked = true;
                    }
                    CheckBusinessRules = true;
                    CheckLinks = true;
                    CheckSyntax = true;
                }
                )); }
        }

        private RelayCommand _okCommand;

        public RelayCommand OkCommand
        {
            get => _okCommand;
            set
            {
                _okCommand = value;
                OnPropertyChanged("OkCommand");
            }
        }

        private RelayCommand _cancelCommand;
     

        public RelayCommand CancelCommand
        {
            get => _cancelCommand;
            set
            {
                _cancelCommand = value;
                OnPropertyChanged("CancelCommand");
            }
        }

        private bool _checkSyntax;
        public bool CheckSyntax
        {
            get => _checkSyntax;
            set
            {
                _checkSyntax = value;
                OnPropertyChanged("CheckSyntax");
            }
        }

        private bool _checkLinks;
        public bool CheckLinks
        {
            get => _checkLinks;
            set
            {
                _checkLinks = value;
                OnPropertyChanged("CheckLinks");
            }
        }

        private bool _checkBusinessRules;
        private MtObservableCollection<DatasetViewModel> _datasets;
        private RelayCommand _allCommand;

        public bool CheckBusinessRules
        {
            get => _checkBusinessRules;
            set
            {
                _checkBusinessRules = value;
                OnPropertyChanged("CheckBusinessRules");
            }
        }

        public void Apply(SlotValidationOption option)
        {
            if (option==null) return;
            CheckSyntax = (option.Flag & (int)ValidationOption.CheckSyntax)!=0;
            CheckLinks = (option.Flag & (int)ValidationOption.CheckLinks) != 0;
            CheckBusinessRules = (option.Flag & (int)ValidationOption.CheckBusinessRules) != 0;

            if (option.MoreOptions != null)
            {
                var onfigIds = FormatterUtil.ObjectFromBytes<int[]>(option.MoreOptions);
                foreach (var dataset in Datasets.Where(t => onfigIds.Contains(t.Id)))
                {
                    dataset.IsChecked = true;
                }
            }

       
        }

        public SlotValidationOption GetOption()
        {
            var result = new SlotValidationOption();
            result.Flag = 0;
            if (CheckSyntax) result.Flag += (int) ValidationOption.CheckSyntax;
            if (CheckLinks) result.Flag += (int)ValidationOption.CheckLinks;
            if (CheckBusinessRules) result.Flag += (int)ValidationOption.CheckBusinessRules;
            //if (CheckSyntax) result.Flag += (int)ValidationOption.CheckMore;

            if (Datasets != null)
            {
                result.MoreOptions=FormatterUtil.ObjectToBytes(Datasets.Where(t => t.IsChecked).Select(t=>t.Id).ToArray());
            }

            return result;
        }
    }
}
