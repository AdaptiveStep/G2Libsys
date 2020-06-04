namespace G2Libsys.ViewModels
{
    using G2Libsys.Commands;
    using G2Libsys.Data.Repository;
    using G2Libsys.Library;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using System.Linq;
    using G2Libsys.Services;
    using System;

    /// <summary>
    ///  Hantera användarens egna info
    /// </summary>
    public class UserProfileViewModel : BaseViewModel, ISubViewModel
    {
        #region fields
        private ObservableCollection<LibraryObject> libObjects;
        private ObservableCollection<Loan> loanObjects;
        private readonly IRepository _repo;
        private readonly IUserRepository _userrepo;
        private Card userCard;
        private User confirm;
        private User confirm2;
        private User currentUser;
        #endregion

        #region commands
        public ICommand Showbutton { get; private set; }
        public ICommand Savebutton { get; private set; }
        public ICommand CancelCommand => new RelayCommand(_ => _navigationService.HostScreen.SubViewModel = null);
        #endregion

        #region properties
        /// <summary>
        /// users card
        /// </summary>
        public Card UserCard
        {
            get => userCard;
            set
            {
                userCard = value;
                OnPropertyChanged(nameof(UserCard));
            }
        }
        /// <summary>
        /// user object to check agains Confirm2 when changing data of user
        /// </summary>
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
        /// <summary>
        /// logged in user
        /// </summary>
        public User CurrentUser
        {
            get => currentUser;
            set
            {
                currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }
        /// <summary>
        /// users loans
        /// </summary>
        public ObservableCollection<Loan> LoanObjects
        {
            get => loanObjects;
            set
            {
                loanObjects = value;
                OnPropertyChanged(nameof(LoanObjects));
            }
        }
        /// <summary>
        /// users loans as libraryobjects
        /// </summary>
        public ObservableCollection<LibraryObject> LibraryObjects
        {
            get => libObjects;
            set
            {
                libObjects = value;
                OnPropertyChanged(nameof(LibraryObjects));
            }
        }
        #endregion

        #region Constructor

        public UserProfileViewModel()
        {
            if (base.IsInDesignMode) return;

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
        /// <summary>
        /// save user and check that all params are correct before sending to DB
        /// </summary>
        public async void Save()
        {
            if (Confirm.Firstname == Confirm2.Firstname && Confirm.Lastname == Confirm2.Lastname && Confirm.Password == Confirm2.Password && Confirm.Email == Confirm2.Email)
            {
                if (!string.IsNullOrEmpty(Confirm.Firstname))
                {
                    CurrentUser.Firstname = Confirm.Firstname;
                }
                if (!string.IsNullOrEmpty(Confirm.Lastname))
                {
                    CurrentUser.Lastname = Confirm.Lastname;
                }
                if (!string.IsNullOrEmpty(Confirm.Password))
                {
                    CurrentUser.Password = Confirm.Password;
                }
                if (!string.IsNullOrEmpty(Confirm.Email))
                {
                    CurrentUser.Email = Confirm.Email;
                }

                await _repo.UpdateAsync(CurrentUser);

                _dialog.Alert("Klart", "Uppgifterna sparades");
            }
            else 
            { 
                _dialog.Alert("Error", "Kunde inte spara. dubbelkolla alla parametrar"); 
            }
        }
        /// <summary>
        /// get users card
        /// </summary>
        public async void GetCard()
        {
            UserCard = await _repo.GetByIdAsync<Card>(CurrentUser.ID);
        }
        /// <summary>
        /// get users loans and the library object of the loans, and get the card
        /// </summary>
        public async void GetLoans()
        {
            if(CurrentUser is null) { return; }
            LoanObjects = new ObservableCollection<Loan>(await _userrepo.GetLoansAsync(CurrentUser.ID));
            foreach (Loan a in LoanObjects)
            {
                a.LoanDate = a.LoanDate.AddDays(14);
            }
            LibraryObjects = new ObservableCollection<LibraryObject>();
            foreach (Loan a in LoanObjects)
            {
                LibraryObjects.Add(await _repo.GetByIdAsync<LibraryObject>(a.ObjectID));
            }
           
            GetCard();
        }

        #endregion 
    }
}
