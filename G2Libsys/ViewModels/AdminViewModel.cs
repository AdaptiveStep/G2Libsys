using G2Libsys.Commands;
using G2Libsys.Data.Repository;
using G2Libsys.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace G2Libsys.ViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        private readonly IUserRepository _repo;
        private User newUser;
        private User selectedUser;
        private ObservableCollection<User> users;

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<User> Users
        {
            get => users;
            set
            {
                users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public User NewUser
        {
            get => newUser;
            set
            {
                newUser = value;
                OnPropertyChanged(nameof(NewUser));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public User SelectedUser
        {
            get => selectedUser;
            set
            {
                selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand AddUserCommand { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public ICommand RemoveUserCommand { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public AdminViewModel()
        {
            _repo = new UserRepository();

            GetUsers();

            AddUserCommand = new RelayCommand(x => AddUser());
            RemoveUserCommand = new RelayCommand(x => RemoveUser());
        }

        /// <summary>
        /// 
        /// </summary>
        private async void GetUsers()
        {
            NewUser = new User();
            Users = new ObservableCollection<User>(await _repo.GetAllAsync());
        }

        /// <summary>
        /// 
        /// </summary>
        private async void RemoveUser()
        {
            await _repo.RemoveAsync(SelectedUser);
            Users.Remove(SelectedUser);

            // Reset NewUser
            NewUser = new User();
        }

        /// <summary>
        /// 
        /// </summary>
        private async void AddUser()
        {
            NewUser.ID = await _repo.AddAsync(NewUser);
            Users.Add(NewUser);

            // Reset NewUser
            NewUser = new User();
        }
    }
}
