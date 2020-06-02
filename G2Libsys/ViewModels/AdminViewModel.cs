namespace G2Libsys.ViewModels
{
    using G2Libsys.Commands;
    using G2Libsys.Data.Repository;
    using G2Libsys.Dialogs;
    using G2Libsys.Library;
    using G2Libsys.Library.Extensions;
    using G2Libsys.Library.Models;
    using G2Libsys.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Documents;
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
        private string filePath;
        private RemoveItemDialogViewModel RemoveItemDialogVM { get; set; } = new RemoveItemDialogViewModel();
        //public ICommand RunDialogCommand => new AnotherCommandImplementation(ExecuteRunDialog);
        #endregion

        #region Properties
            /// <summary>
            /// tooltip for datagrid
            /// </summary>
        public string ToolTip { get; set; }
        /// <summary>
        /// filepath for reports
        /// </summary>
        public string FilePath
        {
            get => filePath;
            set
            {
                filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }
        /// <summary>
        /// string for search results
        /// </summary>
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
        /// <propertys>
        /// type for the selected user
        /// </summary>
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
        /// Command for downloading a csv file with deleted users
        /// </summary>
        public ICommand DownloadUserLogCommand => new RelayCommand(SaveDialogBoxAsync);

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
            GetToolTip();
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
        /// <summary>
        /// get all usertypes
        /// </summary>
        private async void GetUserTypes()
        {
            UserTypes = new ObservableCollection<UserType>(await _repo.GetAllAsync<UserType>());
        }
        /// <summary>
        /// get all users that match search results
        /// </summary>
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
            if (SelectedUser == null) return;
        
            var myVM = new RemoveItemDialogViewModel("Ta bort användare");
            var dialogresult = _dialog.Show(myVM);

            //Skapar en CSV fil med anledning till borttagning av användare
            if (!dialogresult.isSuccess) return;
            AdminAction adminAction = new AdminAction()
            {
                Comment = $"AnvändarID: {SelectedUser.ID} Anledning:  { dialogresult.msg}",
                Actiondate = DateTime.Now,
                ActionType = 1
            };

            bool isSuccess = false;
            try
            {
                await _userRepo.RemoveAsync(SelectedUser.ID);
                Users.Remove(SelectedUser);
                isSuccess = true;
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

            if (isSuccess)
            {
                await _repo.AddAsync(adminAction);
            }
        }

        /// <summary>
        /// Export adminactions on users to csv
        /// </summary>
        public async void SaveDialogBoxAsync(object param = null) //används till att spara .csv fil 
        {
            var adminActions = new List<AdminAction>(await _repo.GetAllAsync<AdminAction>(1));

            var fileService = IoC.ServiceProvider.GetService<IFileService>();

            // Return true if user creates a file
            bool fileCreated = fileService.CreateFile("LibsysUserLog");

            if (fileCreated)
            {
                bool success = fileService.ExportCSV(adminActions);

                if (success)
                {
                    _dialog.Alert("Fil skapad!", "En .csv fil har laddats skapats.");
                }
                else
                {
                    _dialog.Alert("Filen används redan", "Stäng filen och försök igen");
                }
            }
        }
        /// <summary>
        /// create the tooltip for datagrid
        /// </summary>
        private void GetToolTip()
        {
            ToolTip = "\u2022Dubbelklicka för att redigera.";
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
