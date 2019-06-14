using System;
using System.Windows;
using System.Xml;
using MvvmCore;

namespace TOSSM.ViewModel.Control.XmlViewer
{
    public class XmlViewerViewModel : ViewModelBase
    {
        public XmlDocument XmlDocument
        {
            get => _xmlDocument;
            set
            {
                _xmlDocument = value;
                OnPropertyChanged("XmlDocument");
            }
        }

        private Visibility _visibility = Visibility.Hidden;
        public Visibility Visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                OnPropertyChanged("Visibility");
            }
        }

        public Action OkAction { get; set; }

        private RelayCommand _okCommand;
        private XmlDocument _xmlDocument;

        public RelayCommand OkCommand
        {
            get
            {
                return _okCommand ?? (_okCommand =
                    new RelayCommand(
                        t =>
                        {
                            Visibility = Visibility.Hidden;

                            if (OkAction != null)
                            {
                                OkAction();
                            }
                        }));
            }
            set => _okCommand = value;
        }


    }
}
