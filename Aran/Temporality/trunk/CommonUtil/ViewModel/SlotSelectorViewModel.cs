using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.View.Dialog;
using Aran.Temporality.CommonUtil.ViewModel.SlotSelector;
using MvvmCore;
using TOSSM.ViewModel.Control.SlotSelector;

namespace Aran.Temporality.CommonUtil.ViewModel
{
    class SlotSelectorViewModel : ViewModelBase
    {
        private bool _isBusy;


        private string _title="Select User Space";

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        public Action OnClose { get; set; }


        #region Public Slots



        private PublicSlotViewModel _selectedPublicSlot;
        public PublicSlotViewModel SelectedPublicSlot
        {
            get { return _selectedPublicSlot; }
            set
            {
                _selectedPublicSlot = value;
                OnPropertyChanged("SelectedPublicSlot");
                LoadPrivateSlots();
            }
        }

        private MtObservableCollection<PublicSlotViewModel> _publicSlots;
        public MtObservableCollection<PublicSlotViewModel> PublicSlots
        {
            get { return _publicSlots ?? (_publicSlots = new MtObservableCollection<PublicSlotViewModel>()); }
            set { _publicSlots = value; }
        }

        private ICollectionView _publicSlotsFiltered;
        public ICollectionView PublicSlotsFiltered
        {
            get
            {
                if (_publicSlotsFiltered == null)
                {
                    _publicSlotsFiltered = CollectionViewSource.GetDefaultView(PublicSlots);
                    _publicSlotsFiltered.SortDescriptions.Add(new SortDescription("EffectiveDate", ListSortDirection.Descending));
                }
                return _publicSlotsFiltered;
            }
            set { _publicSlotsFiltered = value; }
        }

        #endregion

        #region Private Slots


        private PrivateSlotViewModel _selectedPrivateSlot;
        public PrivateSlotViewModel SelectedPrivateSlot
        {
            get { return _selectedPrivateSlot; }
            set
            {
                _selectedPrivateSlot = value;
                OnPropertyChanged("SelectedPrivateSlot");
            }
        }

        private MtObservableCollection<PrivateSlotViewModel> _privateSlots;
        public MtObservableCollection<PrivateSlotViewModel> PrivateSlots
        {
            get { return _privateSlots ?? (_privateSlots = new MtObservableCollection<PrivateSlotViewModel>()); }
            set { _privateSlots = value; }
        }

        private ICollectionView _privateSlotsFiltered;
        public ICollectionView PrivateSlotsFiltered
        {
            get { return _privateSlotsFiltered ?? (_privateSlotsFiltered = CollectionViewSource.GetDefaultView(PrivateSlots)); }
            set { _privateSlotsFiltered = value; }
        }

        #endregion

        #region Blocker

        private BlockerModel _blockerModel;
        public BlockerModel BlockerModel
        {
            get
            {
                return _blockerModel ?? (_blockerModel = new BlockerModel {});
            }
            set { _blockerModel = value; }
        }

        #endregion

        private void LoadProvateSlotInternal()
        {

            try
            {


                if (SelectedPublicSlot == null)
                {


                    PrivateSlots.Clear();
                    return;
                }


                var list = CommonDataProvider.GetPrivateSlots(SelectedPublicSlot.Id);

                var newIds = new HashSet<int>(list.Select(t => t.Id));

                //delete missing
                var toBeDeleted = PrivateSlots.Where(t => !newIds.Contains(t.Id)).ToList();
                foreach (var model in toBeDeleted)
                {
                    PrivateSlots.Remove(model);
                }

                //add new
                var oldIds = new HashSet<int>(PrivateSlots.Select(t => t.Id));
                var newSlots = list.Where(t => !oldIds.Contains(t.Id)).ToList();
                foreach (var model in newSlots)
                {
                    PrivateSlots.Add(new PrivateSlotViewModel(model)
                    {
                        ActiveId = ActivePrivateSlotId
                    });
                }

                //update current
                var currentSlots = list.Where(t => oldIds.Contains(t.Id)).ToList();
                foreach (var model in currentSlots)
                {
                    var correspondingSlot =
                        PrivateSlots.FirstOrDefault(t => t.Id == model.Id);
                    correspondingSlot?.InitFromSlot(model);
                }

            }
            catch(Exception ex)
            {
                LogManager.GetLogger(typeof(ModifyRegistry)).Error(ex, "Error on loading private slots.");
            }
            finally
            {
                _isBusy = false;
            }
        }

        private void LoadPrivateSlots()
        {
            if (_isBusy) return;
            _isBusy = true;

            if (Application.Current == null)
            {
                LoadProvateSlotInternal();
                return;
            }

            if (Application.Current.Dispatcher == null) return;
            Application.Current.Dispatcher.Invoke(
            DispatcherPriority.Background,
            (Action)(LoadProvateSlotInternal));


        }


        private int ActivePrivateSlotId { get; set; }

        public void LoadPublicSlots()
        {
            var list = CommonDataProvider.GetPublicSlots().Where(t => t.Status != SlotStatus.Published && t.Status != SlotStatus.Publishing).
                OrderByDescending(t => t.EffectiveDate);

            var newIds = new HashSet<int>(list.Select(t => t.Id));

            //delete missing
            var toBeDeleted = PublicSlots.Where(t => !newIds.Contains(t.Id)).ToList();
            foreach (var model in toBeDeleted)
            {
                PublicSlots.Remove(model);
            }

            //add new
            var oldIds = new HashSet<int>(PublicSlots.Select(t => t.Id));
            var newSlots = list.Where(t => !oldIds.Contains(t.Id)).ToList();
            foreach (var model in newSlots)
            {
                PublicSlots.Add(new PublicSlotViewModel(model));
            }

            //update current
            var currentSlots = list.Where(t => oldIds.Contains(t.Id)).ToList();
            foreach (var model in currentSlots)
            {
                var correspondingSlot = PublicSlots.FirstOrDefault(t => t.Id == model.Id);
                correspondingSlot?.InitFromSlot(model);
            }


            LoadPrivateSlots();

            InitActiveSlot();
        }

        private bool _activeSlotInited;
        private void InitActiveSlot()
        {
            if (PublicSlots.Count <= 0 || _activeSlotInited) return;
            if (CurrentDataContext.CurrentUser == null) return;

            if (CurrentDataContext.CurrentUser.ActivePrivateSlot != null)
            {
                ActivePrivateSlotId = CurrentDataContext.CurrentUser.ActivePrivateSlot.Id;
            }

            UpdateActiveStatus();

            if (ShowActivePrivateSlotCommand.CanExecute(null))
            {
                ShowActivePrivateSlotCommand.Execute(null);
            }
            _activeSlotInited = true;
        }

        private void UpdateActiveStatus()
        {

            foreach (var model in PrivateSlots)
            {
                model.ActiveId = ActivePrivateSlotId;
            }

            var activePublicSlotId = 0;

            if (ActivePrivateSlotId != 0)
            {
                var activePrivateSlot = CommonDataProvider.GetPrivateSlotById(ActivePrivateSlotId);
                if (activePrivateSlot != null)
                {
                    activePublicSlotId = activePrivateSlot.PublicSlot.Id;
                    ActiveSlotName = activePrivateSlot.Name;
                }
            }
            else
            {
                ActiveSlotName = "No Active Slot";
            }

            foreach (var model in PublicSlots)
            {
                model.ActiveId = activePublicSlotId;
            }
        }

        private RelayCommand _showActivePrivateSlotCommand;
        public RelayCommand ShowActivePrivateSlotCommand
        {
            get
            {
                return _showActivePrivateSlotCommand ?? (_showActivePrivateSlotCommand =
                    new RelayCommand(
                        t =>
                        {
                            var activePublicSlotModel = PublicSlots.
                                FirstOrDefault(s => s.PictureId == SlotViewModel.ActivatedPicture);
                            if (activePublicSlotModel != null)
                            {
                                SelectedPublicSlot = activePublicSlotModel;
                            }
                        },
                        t =>
                            ActivePrivateSlotId != 0 &&
                            (SelectedPublicSlot == null ||
                            SelectedPublicSlot != null &&
                            SelectedPublicSlot.PictureId != SlotViewModel.ActivatedPicture)));
            }
            set { _showActivePrivateSlotCommand = value; }
        }


        private string _activeSlotName = "No Active Slot";
        public string ActiveSlotName
        {
            get { return _activeSlotName; }
            set
            {
                _activeSlotName = value;
                OnPropertyChanged("ActiveSlotName");
            }
        }
        private void StopLoading()
        {
        }
        private void StartLoading()
        {

        }

        

        private RelayCommand _newPublicSlotCommand;
        public RelayCommand NewPublicSlotCommand
        {
            get
            {
                return _newPublicSlotCommand ?? (_newPublicSlotCommand =
                    new RelayCommand(
                        t2 =>
                        {
                            var plannedDate = DateTime.Now.AddMonths(2);
                            plannedDate = new DateTime(plannedDate.Year, plannedDate.Month, plannedDate.Day);

                            var dialog = new EditPublicSlotDialog
                            {
                                Owner = Application.Current==null?null:Application.Current.MainWindow
                            };

                            var viewModel = new PublicSlotViewModel
                            {
                                Id = 0,
                                Name = null,
                                PlannedCommitDate = plannedDate,
                                AiracSelectorEnabled = true,
                                PlannedDateTimeSelectorEnabled = true,

                                CancelAction = () => { dialog.Close(); },
                            };

                            viewModel.OkAction = () =>
                            {
                                dialog.Close();
                                BlockerModel.BlockForAction(
                                    () =>
                                    {
                                        StopLoading();

                                        var publicSlot = viewModel.Slot();

                                        if (publicSlot.Id == 0)
                                        {
                                            //new
                                            var id = CommonDataProvider.CreatePublicSlot(publicSlot);
                                            if (id <= 0) return;
                                            var savedSlot = CommonDataProvider.GetPublicSlotById(id);
                                            PublicSlots.Add(new PublicSlotViewModel(savedSlot));
                                        }
                                        else
                                        {
                                            //update
                                            if (CommonDataProvider.UpdatePublicSlot(publicSlot))
                                            {
                                                var publicSlotModel = PublicSlots.FirstOrDefault(t => t.Id == publicSlot.Id);
                                                publicSlotModel?.InitFromSlot(publicSlot);
                                            }
                                        }
                                        StartLoading();
                                    }
                                    );
                            };

                            dialog.DataContext = viewModel;
                            viewModel.Show();
                            dialog.ShowDialog();
                        }));
            }
            set { _newPublicSlotCommand = value; }
        }

     
        private RelayCommand _newPrivateSlotCommand;
        public RelayCommand NewPrivateSlotCommand
        {
            get
            {
                return _newPrivateSlotCommand ?? (_newPrivateSlotCommand =
                    new RelayCommand(
                        t2 =>
                        {
                            var viewModel = new PrivateSlotViewModel
                            {
                                Id = 0,
                                Name = null,
                                Reason = null,
                            };
                            var dialog = new EditPrivateSlotDialog();

                            viewModel.CancelAction = () => { dialog.Close(); };
                            viewModel.OkAction = () =>
                            {
                                dialog.Close();
                                if (SelectedPublicSlot == null) return;
                                if (SelectedPublicSlot.Id == 0) return;

                                BlockerModel.BlockForAction(
                                   () =>
                                   {
                                       StopLoading();

                                       var privateSlot = viewModel.Slot();
                                       privateSlot.PublicSlot = SelectedPublicSlot.Slot();

                                       if (privateSlot.Id == 0)
                                       {
                                           privateSlot.PublicSlot.Status = SlotStatus.Opened;

                                           //new
                                           var id = CommonDataProvider.CreatePrivateSlot(privateSlot);
                                           if (id <= 0) return;
                                           var savedSlot = CommonDataProvider.GetPrivateSlotById(id);
                                           PrivateSlots.Add(new PrivateSlotViewModel(savedSlot));

                                           CommonDataProvider.UpdatePublicSlot(privateSlot.PublicSlot);
                                           var correspondingSlot = PublicSlots.FirstOrDefault(t => t.Id == privateSlot.PublicSlot.Id);
                                           if (correspondingSlot != null) correspondingSlot.SlotStatus = privateSlot.PublicSlot.Status;
                                       }
                                       else
                                       {
                                           var oldSlot = CommonDataProvider.GetPrivateSlotById(privateSlot.Id);
                                           //update
                                           privateSlot.Status = oldSlot.Status;

                                           if (CommonDataProvider.UpdatePrivateSlot(privateSlot))
                                           {
                                               var privateSlotModel = PrivateSlots.FirstOrDefault(t => t.Id == privateSlot.Id);
                                               privateSlotModel?.InitFromSlot(privateSlot);
                                           }
                                       }

                                       StartLoading();
                                   }
                                   );
                            };

                            dialog.Owner = Application.Current == null ? null : Application.Current.MainWindow;
                            dialog.DataContext = viewModel;

                            viewModel.Show();
                            dialog.ShowDialog();
                        },
                        t => SelectedPublicSlot != null));
            }
            set { _newPrivateSlotCommand = value; }
        }


        private RelayCommand _editPrivateSlotCommand;
        public RelayCommand EditPrivateSlotCommand
        {
            get
            {
                return _editPrivateSlotCommand ?? (_editPrivateSlotCommand =
                    new RelayCommand(
                        t2 =>
                        {
                            var viewModel = new PrivateSlotViewModel
                            {
                                Id = SelectedPrivateSlot.Id,
                                Name = SelectedPrivateSlot.Name,
                                Reason = SelectedPrivateSlot.Reason,
                            };
                            var dialog = new EditPrivateSlotDialog();

                            viewModel.CancelAction = () => { dialog.Close(); };
                            viewModel.OkAction = () =>
                            {
                                dialog.Close();
                                if (SelectedPublicSlot == null) return;
                                if (SelectedPublicSlot.Id == 0) return;

                                BlockerModel.BlockForAction(
                                   () =>
                                   {
                                       StopLoading();

                                       var privateSlot = viewModel.Slot();
                                       privateSlot.PublicSlot = SelectedPublicSlot.Slot();

                                       if (privateSlot.Id == 0)
                                       {
                                           privateSlot.PublicSlot.Status = SlotStatus.Opened;

                                           //new
                                           var id = CommonDataProvider.CreatePrivateSlot(privateSlot);
                                           if (id <= 0) return;
                                           var savedSlot = CommonDataProvider.GetPrivateSlotById(id);
                                           PrivateSlots.Add(new PrivateSlotViewModel(savedSlot));

                                           CommonDataProvider.UpdatePublicSlot(privateSlot.PublicSlot);
                                           var correspondingSlot = PublicSlots.FirstOrDefault(t => t.Id == privateSlot.PublicSlot.Id);
                                           if (correspondingSlot != null) correspondingSlot.SlotStatus = privateSlot.PublicSlot.Status;
                                       }
                                       else
                                       {
                                           var oldSlot = CommonDataProvider.GetPrivateSlotById(privateSlot.Id);
                                           //update
                                           privateSlot.Status = oldSlot.Status;

                                           if (CommonDataProvider.UpdatePrivateSlot(privateSlot))
                                           {
                                               var privateSlotModel = PrivateSlots.FirstOrDefault(t => t.Id == privateSlot.Id);
                                               if (privateSlotModel != null)
                                               {

                                                   privateSlotModel.InitFromSlot(privateSlot);
                                               }
                                           }
                                       }

                                       StartLoading();
                                   }
                                   );
                            };

                            dialog.Owner = Application.Current == null ? null : Application.Current.MainWindow;
                            dialog.DataContext = viewModel;

                            viewModel.Show();
                            dialog.ShowDialog();
                        },
                        t => SelectedPublicSlot != null && SelectedPrivateSlot != null));
            }
            set { _editPrivateSlotCommand = value; }
        }

       

        private RelayCommand _editPublicSlotCommand;
        public RelayCommand EditPublicSlotCommand
        {
            get
            {
                return _editPublicSlotCommand ?? (_editPublicSlotCommand =
                    new RelayCommand(
                        t2 =>
                        {
                            var dialog = new EditPublicSlotDialog
                            {
                                Owner = Application.Current == null ? null : Application.Current.MainWindow
                            };

                            var viewModel = new PublicSlotViewModel
                            {
                                Id = SelectedPublicSlot.Id,
                                Name = SelectedPublicSlot.Name,
                                PlannedCommitDate = SelectedPublicSlot.PlannedCommitDate,
                                AiracSelectorEnabled = false,
                                PlannedDateTimeSelectorEnabled = false,

                                CancelAction = () => { dialog.Close(); },
                            };

                            viewModel.OkAction = () =>
                            {
                                dialog.Close();
                                BlockerModel.BlockForAction(
                                    () =>
                                    {
                                        StopLoading();

                                        var publicSlot = viewModel.Slot();

                                        if (publicSlot.Id == 0)
                                        {
                                            //new
                                            var id = CommonDataProvider.CreatePublicSlot(publicSlot);
                                            if (id <= 0) return;
                                            var savedSlot = CommonDataProvider.GetPublicSlotById(id);
                                            PublicSlots.Add(new PublicSlotViewModel(savedSlot));
                                        }
                                        else
                                        {
                                            //update
                                            if (CommonDataProvider.UpdatePublicSlot(publicSlot))
                                            {
                                                var publicSlotModel = PublicSlots.FirstOrDefault(t => t.Id == publicSlot.Id);
                                                if (publicSlotModel != null)
                                                {
                                                    publicSlotModel.InitFromSlot(publicSlot);
                                                }
                                            }
                                        }
                                        StartLoading();
                                    }
                                    );
                            };

                            dialog.DataContext = viewModel;
                            viewModel.Show();
                            dialog.ShowDialog();

                        },
                        t => SelectedPublicSlot != null));
            }
            set { _editPublicSlotCommand = value; }
        }

        


        private RelayCommand _deletePrivateSlotCommand;
        public RelayCommand DeletePrivateSlotCommand
        {
            get
            {
                return _deletePrivateSlotCommand ?? (_deletePrivateSlotCommand =
                    new RelayCommand(
                        t => BlockerModel.BlockForAction(
                            () =>
                            {
                                if (SelectedPublicSlot == null) return;
                                StopLoading();

                                if (MessageBox.Show("Are you sure you want to delete selected private slot?",
                                                    "Deleting User Space", MessageBoxButton.YesNo,
                                                    MessageBoxImage.Warning)
                                    == MessageBoxResult.Yes)
                                {
                                    if (CurrentDataContext.CurrentUser != null &&
                                     CurrentDataContext.CurrentUser.ActivePrivateSlot != null &&
                                     CurrentDataContext.CurrentUser.ActivePrivateSlot.Id == SelectedPrivateSlot.Id)
                                    {
                                        DeactivatePrivateSlotCommand.Execute(null);
                                    }

                                    var users = CurrentDataContext.CurrentService.GetUsersOfPrivateSlot(
                                            SelectedPrivateSlot.Id);

                                    var userlist = users.Aggregate(string.Empty, (current, user) => current + (user + "\n"));

                                    if (users.Count > 0)
                                    {
                                        if (MessageBox.Show("There " + ((users.Count == 1) ? "is user (" + users[0] + ") " :
                                            "are " + users.Count + " users including:\n" + userlist) +
                                            "for which selected private slot is active. Are you sure you still want to delete it?",
                                            "Deleting User Space", MessageBoxButton.YesNo,
                                            MessageBoxImage.Warning) == MessageBoxResult.Yes)
                                        {
                                            if (CommonDataProvider.DeletePrivateSlot(SelectedPrivateSlot.Id))
                                            {
                                                PrivateSlots.Remove(SelectedPrivateSlot);
                                                SelectedPrivateSlot = null;
                                                if (PrivateSlots.Count == 0)
                                                {
                                                    var publicSlot = SelectedPublicSlot.Slot();
                                                    publicSlot.Status = SlotStatus.Empty;
                                                    CommonDataProvider.UpdatePublicSlot(publicSlot);
                                                    var correspondingSlot = PublicSlots.Where(s => s.Id == publicSlot.Id).FirstOrDefault();
                                                    if (correspondingSlot != null) correspondingSlot.SlotStatus = publicSlot.Status;

                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show(
                                                    "Something wrong happened when trying to delete private slot");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (CommonDataProvider.DeletePrivateSlot(SelectedPrivateSlot.Id))
                                        {
                                            PrivateSlots.Remove(SelectedPrivateSlot);
                                            SelectedPrivateSlot = null;
                                            if (PrivateSlots.Count == 0)
                                            {
                                                var publicSlot = SelectedPublicSlot.Slot();
                                                publicSlot.Status = SlotStatus.Empty;
                                                CommonDataProvider.UpdatePublicSlot(publicSlot);
                                                var correspondingSlot = PublicSlots.Where(s => s.Id == publicSlot.Id).FirstOrDefault();
                                                if (correspondingSlot != null) correspondingSlot.SlotStatus = publicSlot.Status;

                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show(
                                                "Something wrong happened when trying to delete private slot");
                                        }
                                    }


                                }

                                StartLoading();
                            }
                                 ),
                        t => SelectedPublicSlot != null && SelectedPrivateSlot != null));
            }
            set { _deletePrivateSlotCommand = value; }
        }

        private RelayCommand _deletePublicSlotCommand;
        public RelayCommand DeletePublicSlotCommand
        {
            get
            {
                return _deletePublicSlotCommand ?? (_deletePublicSlotCommand =
                    new RelayCommand(
                        t =>
                        {
                            if (SelectedPublicSlot == null) return;

                            BlockerModel.BlockForAction(
                                () =>
                                {
                                    if (MessageBox.Show("Are you sure you want to delete selected project?",
                                                    "Deleting Project", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                                                    == MessageBoxResult.Yes)
                                    {
                                        if (CommonDataProvider.DeletePublicSlot(SelectedPublicSlot.Id))
                                        {
                                            PublicSlots.Remove(SelectedPublicSlot);
                                            SelectedPublicSlot = null;
                                        }
                                        else
                                        {
                                            MessageBox.Show(
                                                "Something wrong happened when trying to delete project");
                                        }
                                    }

                                }
                                );
                        },
                        t => SelectedPublicSlot != null && SelectedPublicSlot.SlotStatus == SlotStatus.Empty));
            }
            set { _deletePublicSlotCommand = value; }
        }


        private RelayCommand _activatePrivateSlotCommand;
        public RelayCommand ActivatePrivateSlotCommand
        {
            get
            {
                return _activatePrivateSlotCommand ?? (_activatePrivateSlotCommand =
                    new RelayCommand(
                        t => BlockerModel.BlockForAction(
                            () =>
                            {
                                if (SelectedPrivateSlot == null) return;


                                if (CommonDataProvider.ActivatePrivateSlot(SelectedPrivateSlot.Id))
                                {
                                    ActivePrivateSlotId = SelectedPrivateSlot.Id;
                                    UpdateActiveStatus();

                                    CommandManager.InvalidateRequerySuggested();

                                    if (OnClose != null)
                                    {
                                        OnClose();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Some error occured while activating private slot");
                                }

                            }),
                        t => SelectedPublicSlot != null && SelectedPrivateSlot != null));
            }
            set { _activatePrivateSlotCommand = value; }
        }

        private RelayCommand _deactivatePrivateSlotCommand;
        public RelayCommand DeactivatePrivateSlotCommand
        {
            get
            {
                return _deactivatePrivateSlotCommand ?? (_deactivatePrivateSlotCommand =
                    new RelayCommand(
                        t => BlockerModel.BlockForAction(
                            () =>
                            {

                                CommonDataProvider.ActivatePrivateSlot(0);
                                ActivePrivateSlotId = 0;
                                UpdateActiveStatus();
                                CurrentDataContext.CurrentUser.ActivePrivateSlot = null;
                                CommandManager.InvalidateRequerySuggested();
                            }),
                        t => CurrentDataContext.CurrentUser.ActivePrivateSlot != null));
            }
            set { _deactivatePrivateSlotCommand = value; }
        }

   

    }
}
