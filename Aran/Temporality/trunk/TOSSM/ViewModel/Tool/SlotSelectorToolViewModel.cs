using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.Metadata.UI;
using Aran.Aim.Utilities;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.Common.Entity.Util;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.Util;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.View.Dialog;
using Aran.Temporality.CommonUtil.ViewModel;
using Aran.Temporality.CommonUtil.ViewModel.SlotSelector;
using Aran.Temporality.Internal.WorkFlow;
using ClosedXML.Excel;
using FluentNHibernate.Conventions;
using Microsoft.Win32;
using MvvmCore;
using TOSSM.Report.Model;
using TOSSM.Util;
using TOSSM.View.Control;
using TOSSM.ViewModel.Control;
using TOSSM.ViewModel.Control.SlotSelector;

namespace TOSSM.ViewModel.Tool
{
    public class SlotSelectorToolViewModel : UpdatableToolViewModel
    {
        #region Ctor
        public static string ToolContentId = "Projects";

        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/case.png", UriKind.RelativeOrAbsolute);
        
        public SlotSelectorToolViewModel() : base(ToolContentId)
        {
            ContentId = ToolContentId;
            SleepTime = 1000;
            StartLoading();
        }
        #endregion

        protected override void LoadFunction()
        {
            LoadPublicSlots();
        }
        
        private BlockerModel _blockerModel;
        public BlockerModel BlockerModel
        {
            get => _blockerModel ?? (_blockerModel = new BlockerModel { ActivatingObject = this });
            set => _blockerModel = value;
        }
        
        public IList SelectedPrivateSlots { get; set; }

        public RelayCommand TestCommand
        {
            get
            {
                return _testCommand ?? (_testCommand = new RelayCommand(
              t =>
              {
                  SlotSelector.ShowDialog();
              }
              ));
            }
        }

        #region Logic
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

        private void LoadPublicSlots()
        {
            try
            {
                var list = CommonDataProvider.GetPublicSlots().OrderByDescending(t => t.EffectiveDate);

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

                Status = CurrentDataContext.CurrentService.GetCurrentOperationStatus();

            }
            catch
            {
                // ignored
            }
        }

        private OperationStatus _status;
        public OperationStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                if (Status == null) return;

                if (Status.PrivateSlotId == -1)
                {
                    foreach (var publicSlot in PublicSlots)
                    {
                        publicSlot.ProgressVisible = Status.PublicSlotId == publicSlot.Id
                                                              ? Visibility.Visible
                                                              : Visibility.Hidden;

                        publicSlot.ProgressValue = Status.PublicSlotId == publicSlot.Id
                                                           ? Status.Progress
                                                           : 0;
                    }

                    foreach (var privateSlote in PrivateSlots)
                    {
                        privateSlote.ProgressVisible = Visibility.Hidden;
                        privateSlote.ProgressValue = 0;
                    }
                }
                else
                {
                    if (SelectedPublicSlot.Id == Status.PublicSlotId)
                    {
                        foreach (var privateSlote in PrivateSlots)
                        {
                            privateSlote.ProgressVisible = Status.PrivateSlotId == privateSlote.Id
                                                               ? Visibility.Visible
                                                               : Visibility.Hidden;

                            privateSlote.ProgressValue = Status.PrivateSlotId == privateSlote.Id
                                                               ? Status.Progress
                                                               : 0;
                        }
                    }
                    else
                    {
                        foreach (var privateSlote in PrivateSlots)
                        {
                            privateSlote.ProgressVisible = Visibility.Hidden;
                            privateSlote.ProgressValue = 0;
                        }
                    }
                }


            }
        }
        private bool _isBusy;

        private void LoadPrivateSlots()
        {
            if (_isBusy) return;
            _isBusy = true;

            if (Application.Current == null) return;
            if (Application.Current.Dispatcher == null) return;


            //Application.Current.Dispatcher.Invoke(
            //DispatcherPriority.DataBind,
            //(Action)(()
            //=>
            //{

            try
            {


                if (SelectedPublicSlot == null)
                {


                    PrivateSlots.Clear();
                    return;
                }


                var list = CommonDataProvider.GetPrivateSlots(SelectedPublicSlot.Id).OrderByDescending(t => t.CreationDate);

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
            catch
            {
            }
            finally
            {
                _isBusy = false;
            }
            //}));


        }
        #endregion

        #region Public Slots
        private PublicSlotViewModel _selectedPublicSlot;
        public PublicSlotViewModel SelectedPublicSlot
        {
            get => _selectedPublicSlot;
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
            get => _publicSlots ?? (_publicSlots = new MtObservableCollection<PublicSlotViewModel>());
            set => _publicSlots = value;
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
            set => _publicSlotsFiltered = value;
        }
        #endregion

        #region Private Slots
        private PrivateSlotViewModel _selectedPrivateSlot;
        public PrivateSlotViewModel SelectedPrivateSlot
        {
            get => _selectedPrivateSlot;
            set
            {
                _selectedPrivateSlot = value;
                OnPropertyChanged("SelectedPrivateSlot");
            }
        }

        private MtObservableCollection<PrivateSlotViewModel> _privateSlots;
        public MtObservableCollection<PrivateSlotViewModel> PrivateSlots
        {
            get => _privateSlots ?? (_privateSlots = new MtObservableCollection<PrivateSlotViewModel>());
            set => _privateSlots = value;
        }

        private ICollectionView _privateSlotsFiltered;
        public ICollectionView PrivateSlotsFiltered
        {
            get => _privateSlotsFiltered ?? (_privateSlotsFiltered = CollectionViewSource.GetDefaultView(PrivateSlots));
            set => _privateSlotsFiltered = value;
        }

        public int CopiedPrivateSlot { get; set; } = 0;
        public DateTime CopiedPrivateSlotAiracDate { get; set; }
        #endregion

        #region Blocker

        #endregion

        #region Commands
        //public slots
        
        private RelayCommand _showFeatureDependencyReportForPrivateSlotCommand;
        public RelayCommand ShowFeatureDependencyReportForPrivateSlotCommand
        {
            get
            {
                return _showFeatureDependencyReportForPrivateSlotCommand
              ?? (_showFeatureDependencyReportForPrivateSlotCommand = new RelayCommand(
                  t => MainManagerModel.Instance.ShowFeatureDependencyReport(CurrentDataContext.CurrentUser.ActivePrivateSlot),
                  t => CurrentDataContext.CurrentUser != null
                  && CurrentDataContext.CurrentUser.ActivePrivateSlot != null));
            }
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
                                Owner = Application.Current.MainWindow
                            };

                            var cycle = AiracCycle.GetPermittedAiracCycle();
                            var cycleDate = AiracCycle.GetAiracCycleByIndex(cycle);

                            var viewModel = new PublicSlotViewModel
                            {
                                Id = 0,
                                Name = null,
                                ActualDate = cycleDate,
                                PlannedCommitDate = plannedDate,
                                AiracSelectorEnabled = true,
                                PlannedDateTimeSelectorEnabled = true,

                                CancelAction = () => { dialog.Close(); },
                            };

                            SavePublicSlot(viewModel, dialog);

                            dialog.DataContext = viewModel;
                            viewModel.Show();
                            dialog.ShowDialog();
                        }));
            }
            set => _newPublicSlotCommand = value;
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
                                Owner = Application.Current.MainWindow
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

                            SavePublicSlot(viewModel, dialog);

                            dialog.DataContext = viewModel;
                            viewModel.Show();
                            dialog.ShowDialog();

                        },
                        t => SelectedPublicSlot != null));
            }
            set => _editPublicSlotCommand = value;
        }

        private void SavePublicSlot(PublicSlotViewModel viewModel, EditPublicSlotDialog dialog)
        {
            viewModel.OkAction = async () =>
            {
                dialog.Close();
                await BlockerModel.BlockForAction(
                    () => { SavePublicSlot(viewModel); }
                );
            };
        }

        private void SavePublicSlot(PublicSlotViewModel viewModel)
        {
            StopLoading();

            var publicSlot = viewModel.Slot();
            if (PublicSlots.Any(t => t.Name.ToLower().Equals(publicSlot.Name.ToLower())))
            {
                MessageBoxHelper.Show(
                    $"Public slot with name \"{publicSlot.Name}\" is already exist");
                StartLoading();
                return;
            }

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

        private RelayCommand _deletePublicSlotCommand;
        public RelayCommand DeletePublicSlotCommand
        {
            get
            {
                return _deletePublicSlotCommand ?? (_deletePublicSlotCommand =
                    new RelayCommand(
                        async t =>
                        {
                            if (SelectedPublicSlot == null) return;

                            await BlockerModel.BlockForAction(
                                () =>
                                {
                                    if (MessageBoxHelper.Show("Are you sure you want to delete selected project?",
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
                                            MessageBoxHelper.Show(
                                                    "Something wrong happened when trying to delete project");
                                        }
                                    }

                                }
                                );
                        },
                        t => SelectedPublicSlot != null && SelectedPublicSlot.SlotStatus == SlotStatus.Empty));
            }
            set => _deletePublicSlotCommand = value;
        }

        private RelayCommand _copyPrivateSlotCommand;
        public RelayCommand CopyPrivateSlotCommand
        {
            get
            {
                return _copyPrivateSlotCommand ?? (_copyPrivateSlotCommand =
                           new RelayCommand(
                               t =>
                               {
                                   if (SelectedPrivateSlot == null) return;
                                   CopiedPrivateSlot = SelectedPrivateSlot.Id;
                                   CopiedPrivateSlotAiracDate = SelectedPublicSlot.ActualDate;
                               },
                               t => SelectedPrivateSlot != null));
            }
            set => _copyPrivateSlotCommand = value;
        }

        private RelayCommand _pastePrivateSlotCommand;
        public RelayCommand PastePrivateSlotCommand
        {
            get
            {
                return _pastePrivateSlotCommand ?? (_pastePrivateSlotCommand =
                           new RelayCommand(
                               async t =>
                               {
                                   if (SelectedPrivateSlot == null) return;


                                   await BlockerModel.BlockForAction(
                                       () =>
                                       {
                                           foreach (var featureType in Enum.GetValues(typeof(FeatureType)))
                                           {
                                               var count = 0;
                                               var data = CurrentDataContext.CurrentService.GetActualDataByDate(
                                                   new FeatureId
                                                   {
                                                       FeatureTypeId = (int)featureType,
                                                       WorkPackage = CopiedPrivateSlot
                                                   }, true,
                                                   CopiedPrivateSlotAiracDate, Interpretation.BaseLine);

                                               if (data != null && data.IsNotEmpty())
                                               {
                                                   foreach (var item in data)
                                                   {
                                                       count++;
                                                       MainManagerModel.Instance.StatusText =
                                                           "Transfering " + featureType + ": " + count + " / " +
                                                           data.Count;
                                                       try
                                                       {
                                                           Migrator.AddFeatureToSlot(item.Data.Feature, SelectedPrivateSlot.Id, 
                                                               SelectedPublicSlot.ActualDate, false);
                                                       }
                                                       catch (Exception e)
                                                       {
                                                           MessageBoxHelper.Show(e.Message);
                                                       }
                                                   }
                                               }
                                           }

                                           MainManagerModel.Instance.StatusText = "Done";


                                           CopiedPrivateSlot = 0;
                                           CopiedPrivateSlotAiracDate = default(DateTime);
                                       });

                               },
                               t => CopiedPrivateSlot > 0 && SelectedPrivateSlot != null && SelectedPrivateSlot.Id != CopiedPrivateSlot
                               && SelectedPublicSlot != null && SelectedPublicSlot.Slot().Editable));
            }
            set => _pastePrivateSlotCommand = value;
        }

        private RelayCommand _mergePrivateSlotCommand;
        public RelayCommand MergePrivateSlotCommand
        {
            get
            {
                return _mergePrivateSlotCommand ?? (_mergePrivateSlotCommand = new RelayCommand(
                           t =>
                           {
                               MainManagerModel.Instance.SlotMergeViewModel.InitMerge(((TOSSM.ViewModel.Control.SlotSelector.SlotViewModel)(SelectedPrivateSlots[0])).Id, ((TOSSM.ViewModel.Control.SlotSelector.SlotViewModel)(SelectedPrivateSlots[1])).Id);
                           },
                           t => _selectedPublicSlot != null && _selectedPublicSlot.SlotType == (int)PublicSlotType.PermanentDelta &&
                                SelectedPrivateSlots?.Count == 2 && !MainManagerModel.Instance.SlotMergeViewModel.IsBusy));
            }
            set => _mergePrivateSlotCommand = value;
        }

        private RelayCommand _validatePublicSlotCommand;
        public RelayCommand ValidatePublicSlotCommand
        {
            get
            {
                return _validatePublicSlotCommand ?? (_validatePublicSlotCommand =
                    new RelayCommand(
                        t =>
                        {

                        },
                        t => SelectedPublicSlot != null));
            }
            set => _validatePublicSlotCommand = value;
        }

        private RelayCommand _publishPublicSlotCommand;
        public RelayCommand PublishPublicSlotCommand
        {
            get
            {
                return _publishPublicSlotCommand ?? (_publishPublicSlotCommand =
                    new RelayCommand(
                       async t => await BlockerModel.BlockForAction(
                            () =>
                            {
                                if (SelectedPublicSlot == null) return;

                                StopLoading();

                                var publicSlot =
                                    CurrentDataContext.CurrentNoAixmDataService.GetPublicSlotById(
                                        SelectedPublicSlot.Id);

                                if (publicSlot != null)
                                {
                                    var privateSlotList = CommonDataProvider.GetPrivateSlots(SelectedPublicSlot.Id);

                                    if (privateSlotList.Count > 1)
                                    {
                                        MessageBoxHelper.Show(
                                            "In current version you can not publish more than one private slot at once. Please select single private slot and delete others.",
                                            "Publish Project", MessageBoxButton.OK,
                                            MessageBoxImage.Information);
                                        return;
                                    }

                                    if (MessageBoxHelper.Show(
                                        "Are you sure you want to publish selected project?",
                                        "Closing Project", MessageBoxButton.YesNo,
                                        MessageBoxImage.Warning)
                                        == MessageBoxResult.Yes)
                                    {



                                        publicSlot.Status = SlotStatus.ToBePublished;
                                        SelectedPublicSlot.SlotStatus = SlotStatus.ToBePublished;
                                        CurrentDataContext.CurrentNoAixmDataService.UpdatePublicSlot(publicSlot);
                                    }
                                }
                                else
                                {
                                    MessageBoxHelper.Show("Slot can not be published. Check server availability.",
                                                    "Error", MessageBoxButton.OK,
                                                    MessageBoxImage.Error);
                                }



                                StartLoading();
                            }),
                        t => SelectedPublicSlot != null &&
                        !SelectedPublicSlot.Frozen &&
                        AiracCycle.CanPublish(SelectedPublicSlot.ActualDate, SelectedPublicSlot.SlotType == 0)
                        && (CurrentDataContext.CurrentUser.RoleFlag & (int)UserRole.Publisher) != 0));
            }
            set => _publishPublicSlotCommand = value;
        }

        //private slots

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
                            SavePrivateSlot(viewModel, dialog);

                            dialog.Owner = Application.Current.MainWindow;
                            dialog.DataContext = viewModel;

                            viewModel.Show();
                            dialog.ShowDialog();
                        },
                        t => SelectedPublicSlot != null && !SelectedPublicSlot.Frozen));
            }
            set => _newPrivateSlotCommand = value;
        }

        private void SavePrivateSlot(PrivateSlotViewModel viewModel, EditPrivateSlotDialog dialog)
        {
            viewModel.OkAction = async () =>
            {
                dialog.Close();
                if (SelectedPublicSlot == null) return;
                if (SelectedPublicSlot.Id == 0) return;

                await BlockerModel.BlockForAction(
                    () => { SavePrivateSlot(viewModel); }
                );
            };
        }

        public int SavePrivateSlot(PrivateSlotViewModel viewModel)
        {
            StopLoading();

            var privateSlot = viewModel.Slot();
            privateSlot.PublicSlot = SelectedPublicSlot.Slot();

            if (PrivateSlots.Any(t => t.Id != privateSlot.Id && t.Name.ToLower().Equals(privateSlot.Name.ToLower())))
            {
                MessageBoxHelper.Show(
                    $"Private slot with name \"{privateSlot.Name}\" is already exist");
                StartLoading();
                return 0;
            }

            if (privateSlot.Id == 0)
            {
                privateSlot.PublicSlot.Status = SlotStatus.Opened;
                privateSlot.CreationDate = DateTime.Now;

                //new
                var id = CommonDataProvider.CreatePrivateSlot(privateSlot);
                if (id <= 0)
                {
                    StartLoading();
                    return 0;
                }
                var savedSlot = CommonDataProvider.GetPrivateSlotById(id);
                PrivateSlots.Add(new PrivateSlotViewModel(savedSlot));

                CommonDataProvider.UpdatePublicSlot(privateSlot.PublicSlot);
                var correspondingSlot = PublicSlots.FirstOrDefault(t => t.Id == privateSlot.PublicSlot.Id);
                if (correspondingSlot != null) correspondingSlot.SlotStatus = privateSlot.PublicSlot.Status;
                StartLoading();
                return id;
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
                StartLoading();
                return privateSlot.Id;
            }


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
                            SavePrivateSlot(viewModel, dialog);

                            dialog.Owner = Application.Current.MainWindow;
                            dialog.DataContext = viewModel;

                            viewModel.Show();
                            dialog.ShowDialog();
                        },
                        t => SelectedPublicSlot != null && SelectedPrivateSlot != null));
            }
            set => _editPrivateSlotCommand = value;
        }

        private RelayCommand _deletePrivateSlotCommand;
        public RelayCommand DeletePrivateSlotCommand
        {
            get
            {
                return _deletePrivateSlotCommand ?? (_deletePrivateSlotCommand =
                    new RelayCommand(
                        async t => await BlockerModel.BlockForAction(
                            () =>
                            {
                                if (SelectedPublicSlot == null) return;
                                StopLoading();

                                if (MessageBoxHelper.Show("Are you sure you want to delete selected private slot?",
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
                                        if (MessageBoxHelper.Show("There " + ((users.Count == 1) ? "is user (" + users[0] + ") " :
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
                                                MessageBoxHelper.Show(
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
                                            MessageBoxHelper.Show(
                                                "Something wrong happened when trying to delete private slot");
                                        }
                                    }


                                }

                                StartLoading();
                            }
                                 ),
                        t => SelectedPublicSlot != null && SelectedPrivateSlot != null));
            }
            set => _deletePrivateSlotCommand = value;
        }

        private RelayCommand _showValidationReportForPrivateSlotCommand;
        public RelayCommand ShowValidationReportForPrivateSlotCommand
        {
            get
            {
                return _showValidationReportForPrivateSlotCommand ?? (_showValidationReportForPrivateSlotCommand =
                    new RelayCommand(
                        t => MainManagerModel.Instance.ShowValidationReport(CurrentDataContext.CurrentUser.ActivePrivateSlot.Id),
                        t => CurrentDataContext.CurrentUser != null
                    && CurrentDataContext.CurrentUser.ActivePrivateSlot != null
                    ));
            }
            set => _showValidationReportForPrivateSlotCommand = value;
        }

        private RelayCommand _clearMissingLinksForPrivateSlotCommand;
        public RelayCommand ClearMissingLinksForPrivateSlotCommand
        {
            get
            {
                return _clearMissingLinksForPrivateSlotCommand ?? (_clearMissingLinksForPrivateSlotCommand =
                    new RelayCommand(
                        async t =>
                        {
                            await BlockerModel.BlockForAction(() =>
                            {
                                var privateSlotId = CurrentDataContext.CurrentUser.ActivePrivateSlot.Id;

                                var missingLinkProblemReportEntity = CurrentDataContext.CurrentNoAixmDataService.GetProblemReport(0, privateSlotId, 0, ReportType.MissingLinkReport);
                                if (missingLinkProblemReportEntity == null)
                                {
                                    MessageBoxHelper.Show("Report is empty");
                                    return;
                                }

                                var linksCheckDate = missingLinkProblemReportEntity.DateTime;
                                var problems = FormatterUtil.ObjectFromBytes<List<ProblemReportUtil>>(missingLinkProblemReportEntity.ReportData).Cast<LinkProblemReportUtil>().ToList();

                                if (problems.Count == 0)
                                {
                                    MessageBoxHelper.Show("Missing links report is empty.");
                                    return;
                                }

                                var res = MessageBox.Show($"Last links check date: {linksCheckDate.ToLongDateString()}. Continue?", "Continue?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (res != MessageBoxResult.Yes)
                                {
                                    return;
                                }

                                var fixedFeatures = new List<Feature>();

                                var actualDate = CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.EffectiveDate;

                                var featureIds = problems.GroupBy(x => x.FeatureType, x => x.Guid).ToDictionary(x => x.Key, x => x.Distinct().ToList());
                                var typesCount = featureIds.Count;
                                var typeNum = 0;

                                foreach (var featureId in featureIds)
                                {
                                    typeNum++;
                                    var featureType = featureId.Key;

                                    var num = 0;
                                    var count = featureId.Value.Count;

                                    foreach (var guid in featureId.Value)
                                    {
                                        num++;
                                        Application.Current.Dispatcher.Invoke(() =>
                                        {
                                            MainManagerModel.Instance.StatusText = $"Clear {featureType} ({typeNum}/{typesCount}) : {guid} ({num}/{count})";
                                        });

                                        var states = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId
                                        {
                                            WorkPackage = privateSlotId,
                                            FeatureTypeId = (int)featureType,
                                            Guid = guid
                                        }, false, actualDate, Interpretation.BaseLine);

                                        var state = states.FirstOrDefault(x => x.Data?.Feature != null
                                            && (x.Data.Feature.TimeSlice.FeatureLifetime.EndPosition == null
                                            || x.Data.Feature.TimeSlice.FeatureLifetime.EndPosition > actualDate));

                                        var feature = state?.Data?.Feature;

                                        if (feature == null)
                                        {
                                            //TODO: report
                                            continue;
                                        }

                                        var links = problems.Where(x => x.Guid == guid)
                                            .GroupBy(x => x.PropertyPath, x => x.ReferenceFeatureIdentifier)
                                            .ToDictionary(x => x.Key, x => x.ToList());

                                        var needProperties = new List<int>();

                                        Feature lastCorrection = null;

                                        if (feature.TimeSlice.ValidTime.BeginPosition == actualDate)
                                        {
                                            var events = CurrentDataContext.CurrentService.GetEventsByDate(new FeatureId
                                            {
                                                WorkPackage = privateSlotId,
                                                FeatureTypeId = (int)featureType,
                                                Guid = guid
                                            }, false, actualDate, feature.TimeSlice.ValidTime.EndPosition ?? DateTime.MaxValue, Interpretation.PermanentDelta);

                                            lastCorrection = events.Where(x => x.Version.SequenceNumber == feature.TimeSlice.SequenceNumber && x.Interpretation == Interpretation.PermanentDelta)
                                                .OrderBy(x => x.Version.CorrectionNumber).LastOrDefault()?.Data?.Feature;
                                        }

                                        if (!AimMetadataUtility.ClearMissingLinks(feature, links, lastCorrection))
                                            continue;

                                        feature.TimeSlice.ValidTime.EndPosition = null;
                                        if (feature.TimeSlice.ValidTime.BeginPosition == actualDate)
                                        {
                                            CommonDataProvider.CommitAsCorrection(feature, privateSlotId);
                                        }
                                        else
                                        {
                                            feature.TimeSlice.CorrectionNumber = 0;
                                            CommonDataProvider.CommitAsNewSequence(feature, privateSlotId);
                                        }
                                    }
                                }

                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    MainManagerModel.Instance.StatusText = "Clearing missing links was successful. Please Check the Active User Space again";
                                });

                                MessageBoxHelper.Show("Clearing missing links was successful. Please Check the Active User Space again.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            });
                        },
                        t => CurrentDataContext.CurrentUser != null
                    && CurrentDataContext.CurrentUser.ActivePrivateSlot != null
                    ));
            }
            set => _clearMissingLinksForPrivateSlotCommand = value;
        }

        private RelayCommand _commitPrivateSlotCommand;
        public RelayCommand CommitPrivateSlotCommand
        {
            get
            {
                return _commitPrivateSlotCommand ?? (_commitPrivateSlotCommand =
                    new RelayCommand(
                        t =>
                        {

                        },
                        t => SelectedPublicSlot != null && SelectedPrivateSlot != null));
            }
            set => _commitPrivateSlotCommand = value;
        }

        private RelayCommand _checkPrivateSlotCommand;
        public RelayCommand CheckPrivateSlotCommand
        {
            get
            {
                return _checkPrivateSlotCommand ?? (_checkPrivateSlotCommand =
                new RelayCommand(
                    async t => await BlockerModel.BlockForAction(()
                         =>
                    {
                        if (SelectedPrivateSlot == null) return;


                        StopLoading();

                        Application.Current.Dispatcher.Invoke(
                            DispatcherPriority.Normal,
                            (Action)(
                                () =>
                                {

                                    var privateSlot =
                               CurrentDataContext.CurrentNoAixmDataService.GetPrivateSlotById(
                                   SelectedPrivateSlot.Id);
                                    if (privateSlot != null)
                                    {

                                        var dialog = new SlotValidationOptionDialog
                                        {
                                            Owner = Application.Current.MainWindow
                                        };


                                        var dialogModel = new SlotValidationOptionViewModel
                                        {
                                            CancelCommand = new RelayCommand(
                                            s =>
                                            {
                                                dialog.Close();
                                            }),
                                        };


                                        var datasets =
                                            CurrentDataContext.CurrentNoAixmDataService.GetFeatureDependencies();

                                        if (datasets != null)
                                        {
                                            foreach (var datasetViewModel in datasets.Select(dataset => new DatasetViewModel(dataset)))
                                            {
                                                dialogModel.Datasets.Add(datasetViewModel);
                                            }
                                        }


                                        dialogModel.OkCommand = new RelayCommand(
                                            s =>
                                            {
                                                dialog.Close();
                                                privateSlot.Status = SlotStatus.ToBeChecked;
                                                SelectedPrivateSlot.SlotStatus = SlotStatus.ToBeChecked;
                                                CurrentDataContext.CurrentNoAixmDataService.UpdatePrivateSlot(
                                                    privateSlot);

                                                var newOption = dialogModel.GetOption();
                                                newOption.PrivateSlot = privateSlot;
                                                CurrentDataContext.CurrentNoAixmDataService
                                                    .UpdateSlotValidationOption(privateSlot.Id, newOption);
                                            });

                                        var option = CurrentDataContext.CurrentNoAixmDataService.GetSlotValidationOption(privateSlot.Id);

                                        if (option != null)
                                        {
                                            dialogModel.Apply(option);
                                        }

                                        dialog.DataContext = dialogModel;
                                        dialog.ShowDialog();
                                    }
                                    else
                                    {
                                        MessageBoxHelper.Show("Slot can not be checked. Check server availability.",
                                            "Error", MessageBoxButton.OK,
                                            MessageBoxImage.Error);
                                    }
                                }));



                        StartLoading();
                    }),
                    t => SelectedPublicSlot != null && SelectedPrivateSlot != null));
            }
            set => _checkPrivateSlotCommand = value;
        }

        //active slot

        private void UpdateActiveStatus()
        {
            MainManagerModel.Instance.UpdateTitle();

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

        private string _activeSlotName = "No Active Slot";
        public string ActiveSlotName
        {
            get => _activeSlotName;
            set
            {
                _activeSlotName = value;
                OnPropertyChanged("ActiveSlotName");
            }
        }

        private int _activePrivateSlotId;
        private int ActivePrivateSlotId
        {
            get => _activePrivateSlotId;
            set
            {
                _activePrivateSlotId = value;
                UpdateActiveStatus();
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
                            var activePublicSlotModel = PublicSlots.Where(s => s.PictureId == SlotViewModel.ActivatedPicture).
                                FirstOrDefault();
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
            set => _showActivePrivateSlotCommand = value;
        }

        private RelayCommand _activatePrivateSlotCommand;
        public RelayCommand ActivatePrivateSlotCommand
        {
            get
            {
                return _activatePrivateSlotCommand ?? (_activatePrivateSlotCommand =
                    new RelayCommand(
                        async t => await BlockerModel.BlockForAction(
                            () =>
                            {
                                if (SelectedPrivateSlot == null) return;

                                MainManagerModel.Instance.CloseAllDocuments();
                                if (MainManagerModel.Instance.Documents.Count > 0) return;



                                if (CommonDataProvider.ActivatePrivateSlot(SelectedPrivateSlot.Id))
                                {
                                    ActivePrivateSlotId = SelectedPrivateSlot.Id;
                                    UpdateActiveStatus();

                                    var ft = MainManagerModel.Instance.FeaturePresenterToolViewModel.DataPresenter.FeatureType;
                                    if (ft != null)
                                    {
                                        MainManagerModel.Instance.FeaturePresenterToolViewModel.ReloadData();
                                    }

                                    CommandManager.InvalidateRequerySuggested();
                                }
                                else
                                {
                                    MessageBoxHelper.Show("Some error occured while activating private slot");
                                }

                            }),
                        t => SelectedPublicSlot != null && SelectedPrivateSlot != null));
            }
            set => _activatePrivateSlotCommand = value;
        }

        private RelayCommand _deactivatePrivateSlotCommand;
        public RelayCommand DeactivatePrivateSlotCommand
        {
            get
            {
                return _deactivatePrivateSlotCommand ?? (_deactivatePrivateSlotCommand =
                    new RelayCommand(
                        async t => await BlockerModel.BlockForAction(
                            () =>
                            {
                                MainManagerModel.Instance.CloseAllDocuments();
                                if (MainManagerModel.Instance.Documents.Count > 0) return;



                                CommonDataProvider.ActivatePrivateSlot(0);
                                ActivePrivateSlotId = 0;
                                UpdateActiveStatus();
                                CurrentDataContext.CurrentUser.ActivePrivateSlot = null;

                                var ft = MainManagerModel.Instance.FeaturePresenterToolViewModel.DataPresenter.FeatureType;
                                if (ft != null)
                                {
                                    MainManagerModel.Instance.FeaturePresenterToolViewModel.ReloadData();
                                }
                                CommandManager.InvalidateRequerySuggested();
                            }),
                        t => CurrentDataContext.CurrentUser.ActivePrivateSlot != null));
            }
            set => _deactivatePrivateSlotCommand = value;
        }
        
        private RelayCommand _viewContentOfPrivateSlotCommand;
        private RelayCommand _testCommand;

        public RelayCommand ViewContentOfPrivateSlotCommand
        {
            get
            {
                return _viewContentOfPrivateSlotCommand ?? (_viewContentOfPrivateSlotCommand =
                    new RelayCommand(
                    t => MainManagerModel.Instance.ShowSlotContent(CurrentDataContext.CurrentUser.ActivePrivateSlot.Id),
                    t => CurrentDataContext.CurrentUser != null
                    && CurrentDataContext.CurrentUser.ActivePrivateSlot != null));
            }
            set => _viewContentOfPrivateSlotCommand = value;
        }
        
        private RelayCommand _contentReportOfPrivateSlotCommand;
        public RelayCommand ContentReportOfPrivateSlotCommand
        {
            get
            {
                return _contentReportOfPrivateSlotCommand ?? (_contentReportOfPrivateSlotCommand =
                    new RelayCommand(async t =>
                    {

                        CultureInfo ci = CultureInfo.InvariantCulture;
                        Thread.CurrentThread.CurrentCulture = ci;
                        Thread.CurrentThread.CurrentUICulture = ci;

                        var dialog = new SaveFileDialog
                        {
                            Title = "Save report",
                            Filter = "HTML Files (*.html)|*.html|All Files (*.*)|*.*",
                            FileName = $"ContentReport_{CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.Name}_{CurrentDataContext.CurrentUser.ActivePrivateSlot.Name}__{DateTime.Now:dd-MM-yyyy HH-mm}"
                        };

                        if (dialog.ShowDialog() == true)
                        {


                            await BlockerModel.BlockForAction(() =>
                            {
                                var report = GetActiveSlotDelta();
                                var result = Razor.Run<ReportViewModel>("Content", report);
                                File.WriteAllText(dialog.FileName, result);
                                BlockerModel.Unblock();
                            });

                        }
                    },
                    t => CurrentDataContext.CurrentUser != null
                    && CurrentDataContext.CurrentUser.ActivePrivateSlot != null));
            }
            set => _contentReportOfPrivateSlotCommand = value;
        }

        private ReportViewModel GetActiveSlotDelta()
        {
            var publicSlot = CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot;
            var privateSlot = CurrentDataContext.CurrentUser.ActivePrivateSlot;
            var result = new Dictionary<Guid, Dictionary<string, Tuple<string, string>>>();
            var report = new ReportViewModel
            {
                PublicSlot = publicSlot,
                PrivateSlot = privateSlot,
                Features = new List<FeatureReportViewModel>()
            };

            foreach (FeatureType type in Enum.GetValues(typeof(FeatureType)))
            {
                report.Features.AddRange(GetActiveSlotDelta(type));
            }

            return report;
        }

        private List<FeatureReportViewModel> GetActiveSlotDelta(FeatureType type)
        {
            var publicSlot = CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot;
            var privateSlot = CurrentDataContext.CurrentUser.ActivePrivateSlot;
            var result = new List<FeatureReportViewModel>();

            var report = new ReportViewModel { PublicSlot = publicSlot, PrivateSlot = privateSlot, Features = new List<FeatureReportViewModel>() };
            var slotStates = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId
            {
                FeatureTypeId = (int)type,
                WorkPackage = privateSlot.Id
            }, true, publicSlot.EffectiveDate);

            foreach (var slotState in slotStates)
            {
                if (slotState.Guid is Guid guid)
                {
                    var publicStates = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId
                    {
                        FeatureTypeId = (int)type,
                        Guid = guid,
                        WorkPackage = -1
                    }, false, publicSlot.EffectiveDate);

                    StateDeltaUtil.GetFeatureRefDescription = GetFeatureRefDescription;
                    var delta = StateDeltaUtil.GetStateDelta(publicStates?.FirstOrDefault(), slotState);
                    if (delta.Count > 0)
                    {
                        var featureReport = new FeatureReportViewModel
                        {
                            FeatureType = $"{((FeatureType)slotState.FeatureTypeId)}",
                            UUID = slotState.Guid.ToString(),
                            Delta = delta
                        };

                        if (publicStates?.FirstOrDefault()?.Data == null)
                            featureReport.Operation = SlotReportOperation.N;
                        else if (slotState?.Data?.Feature?.TimeSlice?.FeatureLifetime.EndPosition != null)
                            featureReport.Operation = SlotReportOperation.W;
                        else
                            featureReport.Operation = SlotReportOperation.U;

                        if (publicStates?.FirstOrDefault()?.Data?.Feature != null)
                        {
                            featureReport.OldDescription = UIUtilities.GetFeatureDescription(publicStates.FirstOrDefault().Data.Feature, out var hasDesc);
                            if (!hasDesc)
                                featureReport.OldDescription = null;
                        }

                        if (slotState?.Data?.Feature != null)
                        {
                            featureReport.NewDescription = UIUtilities.GetFeatureDescription(slotState.Data.Feature, out var hasDesc);
                            if (!hasDesc)
                                featureReport.NewDescription = null;
                        }

                        result.Add(featureReport);
                    }
                }
            }
            return result;
        }
        
        private RelayCommand _accuracyReportOfPrivateSlotCommand;

        public RelayCommand AccuracyReportOfPrivateSlotCommand
        {
            get
            {
                return _accuracyReportOfPrivateSlotCommand ?? (_accuracyReportOfPrivateSlotCommand = new RelayCommand(t =>
                {
                    var dialog = new SaveFileDialog
                    {
                        Title = "Save Excel Report",
                        Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*",
                        FileName = $"AccuracyReport_{CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.Name}_{CurrentDataContext.CurrentUser.ActivePrivateSlot.Name}__{DateTime.Now:dd-MM-yyyy HH-mm}"
                };

                    if (dialog.ShowDialog() == true)
                    {
                        BlockerModel.BlockForAction(() => { SaveAccuracyReport(dialog.FileName); });
                    }

                }));
            }
        }

        private void SaveAccuracyReport(string fileName)
        {
            var workSheets = new Dictionary<string, DataTable>();

            foreach (FeatureType featureType in Enum.GetValues(typeof(FeatureType)))
            {
                var reports = AccuracyReportUtil.GetAccuracyReportForActiveSlot(featureType);

                var workSheet = AccuracyReportUtil.GetWorksheet(reports);

                if (workSheet == null)
                    continue;

                workSheets.Add(featureType.ToString(), workSheet);
            }

            var wb = new XLWorkbook();
            foreach (var workSheet in workSheets.OrderBy(x => x.Key))
            {
                wb.AddWorksheet(workSheet.Value, workSheet.Key);
            }
            wb.SaveAs(fileName);

            MessageBoxHelper.Show("Accuracy report completed successfully!", "Status", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private string GetFeatureRefDescription(Guid? guid, FeatureType? featureType, int workPackage)
        {
            if (featureType == null)
                return guid.ToString();

            var actualDate = CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.EffectiveDate;
            var privateSlotId = CurrentDataContext.CurrentUser.ActivePrivateSlot.Id;

            var states = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId
            {
                WorkPackage = workPackage,
                FeatureTypeId = (int)featureType,
                Guid = guid
            }, false, actualDate);

            if(states.Count == 0)
                return guid.ToString() + " - broken link";
            

            return UIUtilities.GetFeatureDescription(states.FirstOrDefault().Data.Feature, out var hasDesc);
        }

        #endregion
    }
}
