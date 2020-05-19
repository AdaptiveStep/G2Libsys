namespace G2Libsys.ViewModels
{
    using G2Libsys.Commands;
    using G2Libsys.Data.Repository;
    using G2Libsys.Library;
    using G2Libsys.Services;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    /// <summary>
    /// User details administration
    /// </summary>
    public class UserAdministrationViewModel : BaseViewModel, ISubViewModel
    {
        #region Fields
        private readonly IDialogService _dialog;
        private User activeUser;
        private Card userCard;
        private ObservableCollection<Loan> loanObjects;
        private readonly IRepository _repo;
        private readonly IUserRepository _userrepo;
        private ObservableCollection<LibraryObject> libObjects;
        #endregion


        #region Properties

        /// <summary>
        /// Currently displayed User
        /// </summary>
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
        public ICommand CancelCommand => new RelayCommand(_ => NavService.HostScreen.SubViewModel = null);
        public ICommand Savebutton { get; private set; }
        public ICommand ChangeCardStatusbutton { get; private set; }
        #endregion

        #region Constructor

        public UserAdministrationViewModel() { }

        public UserAdministrationViewModel(User user)
        {
            this.ActiveUser = user;
            
            _dialog = new DialogService();

            _dialog.Alert("Test", ActiveUser.Firstname);

            Savebutton = new RelayCommand(x => Save());
            ChangeCardStatusbutton = new RelayCommand(x => ChangeCardStatus());
            _userrepo = new UserRepository();
            _repo = new GeneralRepository();
        }


        #endregion
        public async void Save()
        {
            await _repo.UpdateAsync(ActiveUser);
        }

        public async void ChangeCardStatus()
        {
            await _repo.UpdateAsync(UserCard);
        }
        public async void GetCard()
        {
            UserCard = await _repo.GetByIdAsync<Card>(ActiveUser.ID);
        }
        public async void GetLoans()
        {
            LoanObjects = new ObservableCollection<Loan>(await _userrepo.GetLoansAsync(ActiveUser.ID));
            LibraryObjects = new ObservableCollection<LibraryObject>(await _userrepo.GetLoanObjectsAsync(ActiveUser.ID));
        }
    }
}
