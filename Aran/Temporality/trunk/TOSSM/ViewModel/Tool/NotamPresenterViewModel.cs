using System;
using System.IO;
using System.Windows;
using System.Xml.Serialization;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.ViewModel;
using Microsoft.Win32;
using MvvmCore;
using TOSSM.Util.Notams.Xml;
using TOSSM.Utils;
using TOSSM.ViewModel.Pane.Base;

namespace TOSSM.ViewModel.Tool
{
    public class NotamPresenterViewModel : ToolViewModel
    {

        public static string ToolContentId = "Notam";

        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/merge.png", UriKind.RelativeOrAbsolute);

        public NotamPresenterViewModel() : base(ToolContentId)
        {
            ContentId = ToolContentId;

            LoadData();
        }


        public NotamPresenterViewModel(string name) : base(name)
        {
        }


        private void LoadData()
        {
            Notams.Clear();
            var notams = CurrentDataContext.CurrentNoAixmDataService.GetAllNotams();
            if (notams != null)
            {
                foreach (var notam in notams)
                {
                    var notamVm = new NotamViewModel();
                    notamVm.Load(notam);
                    Notams.Add(notamVm);
                }
            }
        }

        private BlockerModel _blockerModel;
        public BlockerModel BlockerModel
        {
            get => _blockerModel ?? (_blockerModel = new BlockerModel());
            set => _blockerModel = value;
        }



        public NotamViewModel Notam { get; private set; } = new NotamViewModel();

        public NotamViewModel SelectedNotam { get; set; }


        private Visibility _notamVisibility = Visibility.Collapsed;
        public Visibility NotamVisibility
        {
            get => _notamVisibility;
            set
            {
                _notamVisibility = value;
                OnPropertyChanged(nameof(NotamVisibility));
            }
        }

        private MtObservableCollection<NotamViewModel> notams = new MtObservableCollection<NotamViewModel>();
        public MtObservableCollection<NotamViewModel> Notams
        {
            get => notams ?? (notams = new MtObservableCollection<NotamViewModel>());
            set
            {

                notams = value;
                OnPropertyChanged(nameof(Notams));
            }
        }



        private RelayCommand _refreshCommand;
        public RelayCommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new RelayCommand(t =>
                    {
                        BlockerModel.BlockForAction(LoadData);
                    });

                }
                return _refreshCommand;
            }
        }


        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(t =>
                    {
                        if (Notam == null)
                            return;
                        BlockerModel.BlockForAction(
                            () =>
                            {
                                try
                                {
                                    var notams = CurrentDataContext.CurrentNoAixmDataService.GetAllNotams();
                                    if (notams == null)
                                    {
                                        SaveNotam();
                                    }
                                    else
                                    {
                                        Notam found = null;
                                        foreach (var notam in notams)
                                        {
                                            if (notam.Series.Equals(Notam.Notam.Series) &&
                                                notam.Number.Equals(Notam.Notam.Number) &&
                                                notam.Year.Equals(Notam.Notam.Year) &&
                                                notam.Type.Equals(Notam.Notam.Type))
                                            {
                                                found = notam;
                                                break;
                                            }
                                        }

                                        if (found != null)
                                        {
                                            Notam.Notam.Id = found.Id;
                                            UpdateNotam();
                                        }
                                        else
                                            SaveNotam();
                                    }
                                }
                                catch (Exception e)
                                {
                                    LogManager.GetLogger(typeof(NotamPresenterViewModel)).Warn(e, "Notam saving failed.");
                                    MainManagerModel.Instance.StatusText = "Failed";
                                }

                            });
                    }, t => Notam != null);

                }
                return _saveCommand;
            }
        }

        private void SaveNotam()
        {
            int id = CurrentDataContext.CurrentNoAixmDataService.SaveNotam(Notam.Notam);
            if (id > 0)
                MainManagerModel.Instance.StatusText = "Done";
            else
            {
                MainManagerModel.Instance.StatusText = "Failed";
            }
        }

        private void UpdateNotam()
        {
            if (CurrentDataContext.CurrentNoAixmDataService.UpdateNotam(Notam.Notam))
                MainManagerModel.Instance.StatusText = "Done";
            else
            {
                MainManagerModel.Instance.StatusText = "Failed";
            }
        }

        private RelayCommand _editCommand;
        public RelayCommand EditCommand
        {
            get
            {
                return _editCommand ?? (_editCommand = new RelayCommand(
                           t => MainManagerModel.Instance.Notam(SelectedNotam.Notam),
                           delegate
                           {
                               if (SelectedNotam == null) return false;
                               if (CurrentDataContext.CurrentUser?.ActivePrivateSlot == null) return false;
                               if (!CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.Editable) return false;
                               return true;
                           }));
            }
            set => _editCommand = value;
        }

        private RelayCommand _openSourceCommand;

        public RelayCommand OpenSourceCommand
        {
            get
            {
                return _openSourceCommand ?? (_openSourceCommand = new RelayCommand(
                           t2 =>
                           {

                               var dlg = new Microsoft.Win32.OpenFileDialog
                               {
                                   Title = "Open notam",
                                   DefaultExt = ".txt",
                                   //Filter = "TXT Files (*.txt)|*.txt|All Files|*.*"
                                   Filter = "Text Files (*.txt)|*.txt|XML Files(*.xml) | *.xml|All Files|*.*"
                               };

                               if (dlg.ShowDialog() == true)
                               {
                                   BlockerModel.BlockForAction(
                                       () =>
                                       {

                                           MainManagerModel.Instance.StatusText = "Analyzing file...";


                                           try
                                           {
                                               var notam = NotamParser.ParseFile(dlg.FileName);
                                               notam.UserName = CurrentDataContext.CurrentUserName;
                                               Notam.Clean();
                                               Notam.Load(notam);
                                               NotamVisibility = Visibility.Visible;
                                               MainManagerModel.Instance.StatusText = "Done";
                                           }
                                           catch (FormatException ex)
                                           {
                                               Notam.Clean();
                                               NotamVisibility = Visibility.Collapsed;
                                               MainManagerModel.Instance.StatusText = ex.Message;
                                           }
                                           catch (Exception e)
                                           {
                                               Notam.Clean();
                                               NotamVisibility = Visibility.Collapsed;
                                               MainManagerModel.Instance.StatusText = "Parsing failed.";
                                           }



                                       });
                               }
                           }));
            }
        }




        private RelayCommand _pasteCommand;

        public RelayCommand PasteCommand
        {
            get
            {
                return _pasteCommand ?? (_pasteCommand = new RelayCommand(
                           t2 =>
                           {
                               if (Clipboard.ContainsText(TextDataFormat.Text))
                               {
                                   string text = Clipboard.GetText(TextDataFormat.Text);
                                   BlockerModel.BlockForAction(
                                       () =>
                                       {

                                           MainManagerModel.Instance.StatusText = "Analyzing file...";


                                           try
                                           {


                                               var notam = NotamParser.ParseTxt(text);
                                               notam.UserName = CurrentDataContext.CurrentUserName;
                                               Notam.Clean();
                                               Notam.Load(notam);
                                               NotamVisibility = Visibility.Visible;
                                               MainManagerModel.Instance.StatusText = "Done";

                                               ;
                                           }
                                           catch (FormatException ex)
                                           {
                                               Notam.Clean();
                                               NotamVisibility = Visibility.Collapsed;
                                               MainManagerModel.Instance.StatusText = ex.Message;
                                           }
                                           catch (Exception e)
                                           {
                                               Notam.Clean();
                                               NotamVisibility = Visibility.Collapsed;
                                               MainManagerModel.Instance.StatusText = "Parsing failed.";
                                           }



                                       });
                               }
                           }, t3 => Clipboard.ContainsText(TextDataFormat.Text)));
            }
        }
    }

}
