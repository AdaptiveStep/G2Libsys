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
        public ICommand Changebutton { get; private set; }
        public ICommand SavePasswordbutton { get; private set; }
        public ICommand SaveEmailbutton { get; private set; }
        public ICommand Cancelbutton { get; private set; }

        private readonly IRepository _repo;
        private string newValues;

        public string NewValues
        {
            get => newValues;
            set
            {
                newValues = value;
                OnPropertyChanged(nameof(NewValues));
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
                OnPropertyChanged(nameof(CurrentUser));
            }
        }

        public UserProfileViewModel()
        {
            CurrentUser = NavService.HostScreen.CurrentUser;
            _repo = new GeneralRepository();
            //CurrentUser = 
            //Changebutton = new RelayCommand(x => );
            //Cancelbutton = new RelayCommand(x => );
            SavePasswordbutton = new RelayCommand(x => ChangePassword());
            SaveEmailbutton = new RelayCommand(x => ChangeEmail());
        }

        public async void ChangePassword()
        {
           await _repo.UpdateAsync(CurrentUser);
        }
        public async void ChangeEmail()
        {
            await _repo.UpdateAsync(CurrentUser);
        }
        //public async void GetCurrentUser()
        //{ 
        //    CurrentUser = await _repo.GetAllAsync().Where(o => o.Logg)
        
        //}
        public async void GetLoans()
        {
            LoanObjects = new ObservableCollection<Loan>((await _repo.GetAllAsync<Loan>()).Where(o => o.Card.ID == CurrentUser.ID));
        }
    }
}
