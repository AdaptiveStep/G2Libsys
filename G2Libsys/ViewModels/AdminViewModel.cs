namespace G2Libsys.ViewModels
{
    using G2Libsys.Commands;
    using G2Libsys.Data.Repository;
    using G2Libsys.Dialogs;
    using G2Libsys.Library;
    using G2Libsys.Services;
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows.Input;

    public class AdminViewModel : BaseViewModel, IViewModel
    {
        #region Fields
        private readonly IUserRepository _userRepo;
        private readonly IRepository _repo;
        private User newUser;
        private User selectedUser;
        private ObservableCollection<User> users;
        private ObservableCollection<UserType> _userTypes;
        private string searchstring;
        private ICommand goToUser;
        private UserType selectedUserType;
        #endregion

        #region Properties

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
        /// Collection of users
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
        /// Collection of usertypes
        /// </summary>
        public ObservableCollection<UserType> UserTypes
        {
            get => _userTypes;
            set
            {
                _userTypes = value;
                OnPropertyChanged(nameof(UserTypes));
            }
        }

        /// <summary>
        /// Store new user
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
        /// Selected User
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

        public UserType SelectedUserType
        {
            get => selectedUserType;
            set
            {
                selectedUserType = value;
                OnPropertyChanged(nameof(SelectedUserType));
            }
        }

        #endregion

        #region Commands

        public ICommand searchbutton { get; }
        public ICommand cancelsearch { get; }

        /// <summary>
        /// Add new user
        /// </summary>
        public ICommand AddUserCommand { get; }

        /// <summary>
        /// Remove selected user
        /// </summary>
        public ICommand RemoveUserCommand { get; }

        /// <summary>
        /// Go to details for selected user
        /// </summary>
        public ICommand GoToUser => goToUser ??=
            new RelayCommand(_ =>
            {
                _navigationService.HostScreen.SubViewModel = (ISubViewModel)_navigationService.CreateNewInstance(new UserAdministrationViewModel(SelectedUser));
            }, _ => SelectedUser != null);

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public AdminViewModel()
        {
            if (base.IsInDesignMode) return;

            _userRepo = new UserRepository();
            _repo = new GeneralRepository();

            GetUserTypes();
            GetUsers();

            AddUserCommand = new RelayCommand(_ => AddUser());
            RemoveUserCommand = new RelayCommand(_ => RemoveUser());
            searchbutton = new RelayCommand(_ => Search());
            cancelsearch = new RelayCommand(_ => GetUsers());
        }

        #endregion

        #region Methods

        private async void GetUserTypes()
        {
            UserTypes = new ObservableCollection<UserType>(await _repo.GetAllAsync<UserType>());
        }

        private async void Search()
        {
            Users.Clear();
            Users = new ObservableCollection<User>((await _userRepo.GetRangeAsync(SearchString)));
            OnPropertyChanged(nameof(Users));
        }

        /// <summary>
        /// Get users from database
        /// </summary>
        private async void GetUsers()
        {
            NewUser = new User();
            Users = new ObservableCollection<User>(await _userRepo.GetAllAsync());
        }

        /// <summary>
        /// Remove selected user
        /// </summary>
        private async void RemoveUser()
        {
            bool result = _dialog.Confirm("Godkänn", $"Ta bort användaren:\n{SelectedUser.Firstname} {SelectedUser.Lastname}?");

            if (!result) return;

            try
            {
                await _userRepo.DeleteByIDAsync(SelectedUser.ID);
                Users.Remove(SelectedUser);
            }
            catch (Exception ex)
            {
                _dialog.Alert("Fel", "Borttagning misslyckades, försök igen");
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                // Reset NewUser
                NewUser = new User();
            }
        }

        /// <summary>
        /// Create new user
        /// </summary>
        private async void AddUser()
        {
            if (SelectedUserType == null)
            {
                _dialog.Alert("Fel", "Välj användartyp");
                return;
            }

            if (await _userRepo.VerifyEmailAsync(NewUser.Email))
            {
                _dialog.Alert("Fel", "Emailadressen finns redan.");
                return;
            }

            NewUser.UserType = SelectedUserType.ID;

            try
            {
                NewUser.ID = await _userRepo.AddAsync(NewUser);
                Users.Add(NewUser);
            }
            catch (Exception ex)
            {
                _dialog.Alert("Fel", "Kunde inte lägga till ny användare, försök igen");
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                // Reset NewUser
                NewUser = new User();
            }
        }

        #endregion
    }
}
