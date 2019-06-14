using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Aran.Temporality.Common.Aim.Service;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.CommonUtil.Context;
using MvvmCore;

namespace ClientInstaller.ViewModel
{
    internal class UsersListViewModel : ViewModelBase
    {
        public UsersListViewModel(IList<User> users, int selectedUserId)
        {
            AllUsers = new ObservableCollection<User>(users);
            SelectedUser = AllUsers.FirstOrDefault(x => x.Id == selectedUserId);
        }

        public bool IsSuccess = false;
        public int SelectedUserId;

        private ObservableCollection<User> _allUser = new ObservableCollection<User>();
        public ObservableCollection<User> AllUsers
        {
            get => _allUser;
            set
            {
                _allUser = value;
                OnPropertyChanged("AllUsers");
            }
        }

        private User _selectedUser;

        public UsersListViewModel()
        {

        }

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }

        private RelayCommand _selectCommand;

        public RelayCommand SelectCommand => _selectCommand ?? (_selectCommand = new RelayCommand(SelectAction));

        private void SelectAction(object t)
        {
            IsSuccess = true;
            SelectedUserId = SelectedUser.Id;
            ((Window)t)?.Close();
        }
    }
}
