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
    public class AdminViewModel : BaseViewModel, IViewModel
    {
        private readonly IUserRepository _repo;
        private readonly IRepository<UserType> _repoUT;
        private User newUser;
        private User selectedUser;
        private ObservableCollection<User> users;
        private ObservableCollection<UserType> _userTypes;
        private string searchstring;

        public string SearchString
        {
            get => searchstring;
            set
            {
                searchstring = value;
                OnPropertyChanged(nameof(SearchString));
            }
        }
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
        public ICommand searchbutton { get; private set; }
        public ICommand cancelsearch { get; private set; }

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
        /// 
        public AdminViewModel()
        {
            _repo = new UserRepository();
            _repoUT = new GeneralRepository<UserType>();
            UserTypes = new ObservableCollection<UserType>();
            GetUserTypes();

            GetUsers();

            AddUserCommand = new RelayCommand(x => AddUser());
            RemoveUserCommand = new RelayCommand(x => RemoveUser());
            searchbutton = new RelayCommand(x => Search());
            cancelsearch = new RelayCommand(x => GetUsers());
        }
        public ObservableCollection<UserType> UserTypes
        {
            get => _userTypes;
            set
            {
                _userTypes = value;
                OnPropertyChanged(nameof(UserType));
            }
        }
        private async void GetUserTypes()
        {
            UserTypes = new ObservableCollection<UserType>(await _repoUT.GetAllAsync());
        }
        public async void Search()
        {
            Users.Clear();
            Users = new ObservableCollection<User>((await _repo.GetRangeAsync(SearchString)));
            OnPropertyChanged(nameof(Users));
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
            await _repo.DeleteByIDAsync(SelectedUser.ID);
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
