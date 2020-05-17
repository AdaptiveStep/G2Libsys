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
        private readonly IDialogService _dialog;
        public ICommand Showbutton { get; private set; }
        public ICommand SavePasswordbutton { get; private set; }
        public ICommand SaveEmailbutton { get; private set; }
        public ICommand Cancelbutton { get; private set; }

        private readonly IRepository _repo;
        private readonly IUserRepository _userrepo;

        private string newValuesP;

        public string NewValuesP
        {
            get => newValuesP;
            set
            {
                newValuesP = value;
                OnPropertyChanged(nameof(NewValuesP));
            }
        }
        private string newValuessP;

        public string NewValuessP
        {
            get => newValuessP;
            set
            {
                newValuessP = value;
                OnPropertyChanged(nameof(NewValuessP));
            }
        }
        private string newValuesE;

        public string NewValuesE
        {
            get => newValuesE;
            set
            {
                newValuesE = value;
                OnPropertyChanged(nameof(NewValuesP));
            }
        }
        private string newValuessE;

        public string NewValuessE
        {
            get => newValuessE;
            set
            {
                newValuessE = value;
                OnPropertyChanged(nameof(NewValuessE));
            }
        }

        private User currentUser;
        public User CurrentUser
        {
            get => currentUser;
            set
            {
                currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }
        private ObservableCollection<Loan> loanObjects;
        public ObservableCollection<Loan> LoanObjects
        {
            get => loanObjects;
            set
            {
                loanObjects = value;
                OnPropertyChanged(nameof(LoanObjects));
            }
        }
        private ObservableCollection<LibraryObject> libObjects;
        public ObservableCollection<LibraryObject> LibraryObjects
        {
            get => libObjects;
            set
            {
                libObjects = value;
                OnPropertyChanged(nameof(LibraryObjects));
            }
        }

        public UserProfileViewModel()
        {
            _dialog = new DialogService();
            if (base.IsInDesignMode) return;
            CurrentUser = NavService.HostScreen.CurrentUser;
            _repo = new GeneralRepository();
            _userrepo = new UserRepository();

            SavePasswordbutton = new RelayCommand(x => ChangePassword());
            Showbutton = new RelayCommand(x => GetLoans());
            SaveEmailbutton = new RelayCommand(x => ChangeEmail());
            
        }

        public async void ChangePassword()
        {
            if (NewValuesP == NewValuessP && NewValuesP != null)
            {
                CurrentUser.Password = NewValuesP;
                await _repo.UpdateAsync(CurrentUser);
                NewValuesP = null;
                NewValuessP = null;
                _dialog.Alert("", "Ditt Lösenord har Uppdaterats");
            }
            else _dialog.Alert("", "Båda fälten stämmer inte överens");
        }
        public async void ChangeEmail()
        {
            if (NewValuesE == NewValuessE && NewValuesE != null) 
            {
                CurrentUser.Email = NewValuesE;
                await _repo.UpdateAsync(CurrentUser);
                NewValuesE = null;
                NewValuesE = null;
                _dialog.Alert("", "Din Email har Uppdaterats");
            }
            else _dialog.Alert("", "Båda fälten stämmer inte överens");
        }
        //public async void GetCurrentUser()
        //{ 
        //    CurrentUser = await _repo.GetAllAsync().Where(o => o.Logg)
        
        //}
        public async void GetLoans()
        {
            LoanObjects = new ObservableCollection<Loan>(await _userrepo.GetLoansAsync(CurrentUser.ID));
            LibraryObjects = new ObservableCollection<LibraryObject>(await _userrepo.GetLoanObjectsAsync(CurrentUser.ID));
        }
       
    }
}
