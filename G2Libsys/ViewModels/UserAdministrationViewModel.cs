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
    using System.Linq;
    using System.Reflection;
    using System.Windows.Input;

    /// <summary>
    /// User details administration
    /// </summary>
    public class UserAdministrationViewModel : BaseViewModel, ISubViewModel
    {
        #region Fields
        
        private User activeUser;
        private Card userCard;
        private Card newCard;
        private ObservableCollection<Loan> loanObjects;
        private readonly IRepository _repo;
        private readonly IUserRepository _userrepo;
        private ObservableCollection<LibraryObject> libObjects;
        private string cardStatus;
        private Loan selectedLoan;
        
        private User confirm;
        private User confirm2;
        #endregion


        #region Properties

        /// <summary>
        /// Currently displayed User
        /// </summary>
        /// 
        public Loan SelectedLoan
        {
            get => selectedLoan;
            set
            {
                selectedLoan = value;
                OnPropertyChanged(nameof(SelectedLoan));
            }
        }
        public User Confirm 
        {
            get=> confirm;
            set {confirm=value;
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

        public string CardStatus
        {
            get => cardStatus;
            set
            {
                cardStatus = value;
                OnPropertyChanged(nameof(CardStatus));
            }
        }
        public User ActiveUser
        {
            get => activeUser;
            set
            {
                activeUser = value;
                OnPropertyChanged(nameof(ActiveUser));
            }
        }
        public Card UserCard
        {
            get => userCard;
            set
            {
                userCard = value;
                OnPropertyChanged(nameof(UserCard));
            }
        }
        public Card NewCard
        {
            get => newCard;
            set
            {
                newCard = value;
                OnPropertyChanged(nameof(NewCard));
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
        public ObservableCollection<Loan> LoanObjects
        {
            get => loanObjects;
            set
            {
                loanObjects = value;
                OnPropertyChanged(nameof(LoanObjects));
            }
        }
        #endregion

        #region Commands

        /// <summary>
        /// Close SubViewModel
        /// </summary>
        public ICommand CancelCommand => new RelayCommand(_ => _navigationService.HostScreen.SubViewModel = null);
        public ICommand Savebutton { get; private set; }
        public ICommand ChangeCardStatusbutton { get; private set; }
        public ICommand CreateNewCardbutton { get; private set; }
        public ICommand ReturnLoan { get; private set; }
        #endregion

        #region Constructor

        public UserAdministrationViewModel() { }

        public UserAdministrationViewModel(User user)
        {
            this.ActiveUser = user;
            Confirm = new User();
            Confirm2 = new User();
            
            NewCard = new Card() { ActivationDate = DateTime.Now, ValidUntil = DateTime.Now.AddYears(1), Activated= true};

            NewCard.Owner = ActiveUser.ID;

            ReturnLoan = new RelayCommand(x => Return());
            Savebutton = new RelayCommand(x => Save());
            ChangeCardStatusbutton = new RelayCommand(x => ChangeCardStatus());
            CreateNewCardbutton = new RelayCommand(x => CreateNewCard());
            _userrepo = new UserRepository();
            _repo = new GeneralRepository();
            GetLoans();
            GetCard();
        }


        #endregion

        #region Methods
        public async void Return()
        {
            if (SelectedLoan != null)
            {
               
                        SelectedLoan.Returned = true;
                        await _repo.UpdateAsync<Loan>(SelectedLoan);
                   
                    
                
            }

        }
        public async void CreateNewCard()
        {
            if (UserCard != null)
            {
                await _repo.RemoveAsync<Card>(UserCard.ID);
            }
            UserCard = NewCard;
            await _repo.AddAsync(NewCard);
            GetCard();
            _dialog.Alert("Klart", "Nytt Kort Skapat");
        }
        public async void Save()
        {
            //PropertyInfo[] props = typeof(User).GetProperties();
            //foreach (var atri in props)
            //{
               
                
            //}


                if (Confirm.Firstname == Confirm2.Firstname && Confirm.Lastname == Confirm2.Lastname && Confirm.Password == Confirm2.Password && Confirm.Email == Confirm2.Email)
                {
                    if (Confirm.Firstname != null && Confirm.Firstname !="")
                    {
                        ActiveUser.Firstname = Confirm.Firstname;
                    }
                    if (Confirm.Lastname != null && Confirm.Lastname != "")
                    {
                        ActiveUser.Lastname = Confirm.Lastname;
                    }
                    if (Confirm.Password != null && Confirm.Password != "")
                    {
                        ActiveUser.Password = Confirm.Password;
                    }
                    if (Confirm.Email != null && Confirm.Email != "")
                    {
                        ActiveUser.Email = Confirm.Email;
                    }
                    await _repo.UpdateAsync(ActiveUser);
                   _dialog.Alert("Klart", "Uppgifterna sparades");
            }
                else { _dialog.Alert("Error", "Kunde inte spara. dubbelkolla alla parametrar"); }
        }
       
        public async void ChangeCardStatus()
        {
            
            if (UserCard.Activated == true)
            {
                UserCard.Activated = false;
                CardStatus = "Aktivera Kort";
            }
            else 
            { 
                CardStatus = "Spärra Kort";
                UserCard.Activated = true;
            }
            await _repo.UpdateAsync(UserCard);
            
        }
        public async void GetCard()
        {
                UserCard = await _repo.GetByIdAsync<Card>(ActiveUser.ID);
            if (UserCard != null)
            {
                if (UserCard.Activated == true)
                {
                    CardStatus = "Spärra Kort";
                }
                else { CardStatus = "Aktivera Kort"; }
            }
        }
        public async void GetLoans()
        {
            LoanObjects = new ObservableCollection<Loan>(await _userrepo.GetLoansAsync(ActiveUser.ID));
            LibraryObjects = new ObservableCollection<LibraryObject>(await _userrepo.GetLoanObjectsAsync(ActiveUser.ID));
        }
        #endregion
    }
}
