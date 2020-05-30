using G2Libsys.Commands;
using G2Libsys.Data.Repository;
using G2Libsys.Library;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using G2Libsys.Services;

namespace G2Libsys.ViewModels
{
    // Hantera användarens egna info
    public class UserProfileViewModel : BaseViewModel, IViewModel
    {
        

        private ObservableCollection<LibraryObject> libObjects;
        private ObservableCollection<Loan> loanObjects;
        private readonly IRepository _repo;
        private readonly IUserRepository _userrepo;
        private Card userCard;
        private User confirm;
        private User confirm2;
        private User currentUser;
        public ICommand Showbutton { get; private set; }
        public ICommand Savebutton { get; private set; }
        
        public Card UserCard
        {
            get => userCard;
            set
            {
                userCard = value;
                OnPropertyChanged(nameof(UserCard));
            }
        }
        public User Confirm
        {
            get => confirm;
            set
            {
                confirm = value;
                OnPropertyChanged(nameof(Confirm));
            }
        }
        public User Confirm2
        {
            get => confirm2;
            set
            {
                confirm2 = value;
                OnPropertyChanged(nameof(Confirm2));
            }
        }

        public User CurrentUser
        {
            get => currentUser;
            set
            {
                currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }
        public ObservableCollection<Loan> LoanObjects
        {
            get => loanObjects;
            set
            {
                loanObjects = value;
                OnPropertyChanged(nameof(LoanObjects));
            }
        }
        public ObservableCollection<LibraryObject> LibraryObjects
        {
            get => libObjects;
            set
            {
                libObjects = value;
                OnPropertyChanged(nameof(LibraryObjects));
            }
        }
        #region Construct
        public UserProfileViewModel()
        {
            
           
           

            CurrentUser = _navigationService.HostScreen.CurrentUser;
            _repo = new GeneralRepository();
            _userrepo = new UserRepository();
            Savebutton = new RelayCommand(x => Save());
            Showbutton = new RelayCommand(x => GetLoans());
            Confirm = new User();
            Confirm2 = new User();
            GetLoans();

        }
        #endregion
        #region Methods
        public async void Save()
        {
            //PropertyInfo[] props = typeof(User).GetProperties();
            //foreach (var atri in props)
            //{


            //}


            if (Confirm.Firstname == Confirm2.Firstname && Confirm.Lastname == Confirm2.Lastname && Confirm.Password == Confirm2.Password && Confirm.Email == Confirm2.Email)
            {
                if (Confirm.Firstname != null && Confirm.Firstname != "")
                {
                    CurrentUser.Firstname = Confirm.Firstname;
                }
                if (Confirm.Lastname != null && Confirm.Lastname != "")
                {
                    CurrentUser.Lastname = Confirm.Lastname;
                }
                if (Confirm.Password != null && Confirm.Password != "")
                {
                    CurrentUser.Password = Confirm.Password;
                }
                if (Confirm.Email != null && Confirm.Email != "")
                {
                    CurrentUser.Email = Confirm.Email;
                }
                await _repo.UpdateAsync(CurrentUser);
                _dialog.Alert("Klart", "Uppgifterna sparades");
            }
            else { _dialog.Alert("Error", "Kunde inte spara. dubbelkolla alla parametrar"); }
        }
        //public async void GetCurrentUser()
        //{ 
        //    CurrentUser = await _repo.GetAllAsync().Where(o => o.Logg)

        //}

        public async void GetCard()
        {
            UserCard = await _repo.GetByIdAsync<Card>(CurrentUser.ID);
           
        }
        public async void GetLoans()
        {
            //LoanObjects = new ObservableCollection<Loan>(await _userrepo.GetLoansAsync(CurrentUser.ID));
            LibraryObjects = new ObservableCollection<LibraryObject>(await _userrepo.GetLoanObjectsAsync(CurrentUser.ID));
            GetCard();

        }
        #endregion 
    }
}
