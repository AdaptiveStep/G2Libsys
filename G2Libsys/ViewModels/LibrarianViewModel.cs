using G2Libsys.Data.Repository;
using G2Libsys.Library;
using G2Libsys.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;

namespace G2Libsys.ViewModels
{
    public class LibrarianViewModel: BaseViewModel, IViewModel
    {
       
        private readonly IRepository<User> _repo;

        //binda knapparna från viewen
        //skapa datagrid

        private ObservableCollection<User> _users;
        private string searchstring;

        private User selectedUser;
        public User SelectedUser
        {
            get => selectedUser;
            set
            {
                selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        public string SearchString
        {
            get => searchstring;
            set
            {
                searchstring = value;
                OnPropertyChanged(nameof(SearchString));
            }
        }

        private User newUser;
        public User NewUser
        {
            get => newUser;
            set
            {
                newUser = value;
                OnPropertyChanged(nameof(NewUser));
            }
        }

        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        #region Commands
        public ICommand addbutton { get; private set; }
        public ICommand deletebutton { get; private set; }
        public ICommand searchbutton { get; private set; }
        public ICommand cancelsearch { get; private set; }
        private ICommand goToUser;

        public ICommand GoToUser => goToUser ??=
            new RelayCommand(_ =>
            {
                _navigationService.HostScreen.SubViewModel = (ISubViewModel)_navigationService.CreateNewInstance(new UserAdministrationViewModel(SelectedUser));
            }, _ => SelectedUser != null);

        #endregion

        #region Constructor

        public LibrarianViewModel()
        {
            if (base.IsInDesignMode) return;

            _repo = new GeneralRepository<User>();
            Users = new ObservableCollection<User>();
            //Users.CollectionChanged += Users_CollectionChanged;
            NewUser = new User();
            GetUsers();
            addbutton = new RelayCommand(x => AddUser());
            deletebutton = new RelayCommand(x=>DeleteUser());
            searchbutton = new RelayCommand(x => Search());
            cancelsearch = new RelayCommand(x => GetUsers());


        }

        #endregion

        #region Public Methods
        public async void Search()
        {
            Users.Clear();
            Users = new ObservableCollection<User>((await _repo.GetRangeAsync(SearchString)).Where(x => x.UserType == 3));
            
        }        

        public async void GetUsers()
        {
            Users = new ObservableCollection<User>((await _repo.GetAllAsync()).ToList().Where(x => x.UserType == 3));
            
        }
        
        public async void DeleteUser()
        {
            if (SelectedUser != null)
                await _repo.RemoveAsync(SelectedUser.ID);
            GetUsers();
        }
        public async void AddUser()
        {
           await _repo.AddAsync(NewUser);
            GetUsers();
        }

        //lista av besökare
        //ta bort besökare funktion
        //lägga till besökare funktion
        //söka efter besökare. antigen en funtion eller i datagrid
        //registrera lånekort

        #endregion
    }
}
