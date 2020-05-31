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
    using Microsoft.Extensions.DependencyInjection;
    using System.Threading.Tasks;
    using G2Libsys.Library.Models;

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
        private string reason;
        private User confirm;
        private User confirm2;
        #endregion

        #region Properties
        
        public string Reason
        {
            get => reason;
            set
            {
                reason = value;
                OnPropertyChanged(nameof(Reason));
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

        /// <summary>
        /// Carstatus message
        /// </summary>
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

        public ICommand ExportHistory { get; private set; }

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
            _userrepo = new UserRepository();
            _repo = new GeneralRepository();

            Confirm = new User();
            Confirm2 = new User();

            NewCard = new Card() { ActivationDate = DateTime.Now, ValidUntil = DateTime.Now.AddYears(1), Activated = true };
            NewCard.Owner = ActiveUser.ID;

            SetupCommands();

            GetLoans();
            GetCard();
        }

        #endregion

        #region Methods

        private void SetupCommands()
        {
            ExportHistory = new RelayCommand(async _ => await SaveDialogBoxAsync());
            ReturnLoan = new RelayCommand(_ => Return());
            Savebutton = new RelayCommand(_ => Save());
            ChangeCardStatusbutton = new RelayCommand(_ => ChangeCardStatus(), _ => UserCard != null);
            CreateNewCardbutton = new RelayCommand(_ => CreateNewCard());
        }

        private async void Return()
        {
            foreach (Loan a in LoanObjects)
            {
                await _repo.UpdateAsync(a);
            }

            GetLoans();
        }

        private async void CreateNewCard()
        {
            if (UserCard != null)
            {
                var dialogViewModel = new RemoveItemDialogViewModel("Ta bort kort", "Anledning:");
                (bool isSuccess, string msg) = _dialog.Show(dialogViewModel);

                if (!isSuccess) return;

                var adminAction = new AdminAction()
                {
                    Comment = msg,
                    ActionType = 4,
                    Actiondate = DateTime.Now
                };

                await _repo.AddAsync(adminAction);

                await SaveDialogBoxAsync();

                await _repo.RemoveAsync<Card>(UserCard.Owner);
            }

            await _repo.AddAsync(NewCard);
            GetCard();
            _dialog.Alert("Klart", "Nytt Kort Skapat");
        }

        private async void Save()
        {
            if (Confirm.Firstname == Confirm2.Firstname && Confirm.Lastname == Confirm2.Lastname && Confirm.Password == Confirm2.Password && Confirm.Email == Confirm2.Email)
            {
                if (!string.IsNullOrWhiteSpace(Confirm.Firstname))
                {
                    ActiveUser.Firstname = Confirm.Firstname;
                }
                if (!string.IsNullOrWhiteSpace(Confirm.Lastname))
                {
                    ActiveUser.Lastname = Confirm.Lastname;
                }
                if (!string.IsNullOrWhiteSpace(Confirm.Password))
                {
                    ActiveUser.Password = Confirm.Password;
                }
                if (!string.IsNullOrWhiteSpace(Confirm.Email))
                {
                    ActiveUser.Email = Confirm.Email;
                }

                await _repo.UpdateAsync(ActiveUser);

                _dialog.Alert("Klart", "Uppgifterna sparades");
            }
            else
            {
                _dialog.Alert("Error", "Kunde inte spara, dubbelkolla alla parametrar");
            }
        }

        private async void ChangeCardStatus()
        {
            if (UserCard.Activated)
            {
                var dialogViewModel = new RemoveItemDialogViewModel("Spärra kort");
                (bool isSuccess, string msg) = _dialog.Show(dialogViewModel);

                if (isSuccess)
                {
                    UserCard.Activated = false;

                    // Change cardstatus message
                    CardStatus = "Aktivera Kort";

                    var adminAction = new AdminAction()
                    {
                        Comment = msg,
                        ActionType = 4,
                        Actiondate = DateTime.Now
                    };

                    await _repo.AddAsync(adminAction).ConfigureAwait(false);
                }
            }
            else
            {
                // Change cardstatus message
                CardStatus = "Spärra Kort";
                UserCard.Activated = true;
            }

            await _repo.UpdateAsync(UserCard);
        }

        private async void GetCard()
        {
            UserCard = await _repo.GetByIdAsync<Card>(ActiveUser.ID);

            CardStatus = UserCard?.Activated ?? false ? "Spärra Kort" : "Aktivera Kort";
        }

        /// <summary>
        /// Export this users loan history to a csv file
        /// </summary>
        private async Task SaveDialogBoxAsync()
        {
            if (ActiveUser is null) return;

            var loanHistory = await _repo.GetRangeAsync<Loan>(new { ActiveUser.ID });

            var fileService = IoC.ServiceProvider.GetService<IFileService>();

            bool fileCreated = fileService.CreateFile($"Lånehistorik_{ActiveUser.ID}");

            if (fileCreated)
            {
                bool success = fileService.ExportCSV(loanHistory.ToList());

                if (success)
                {
                    _dialog.Alert("Filen sparad", "");
                }
                else
                {
                    _dialog.Alert("Exportering misslyckades", "Stäng filen om öppen och försök igen.");
                }
            }
        }

        public async void GetLoans()
        {
            LoanObjects = new ObservableCollection<Loan>(await _userrepo.GetLoansAsync(ActiveUser.ID));
            LibraryObjects = new ObservableCollection<LibraryObject>();

            foreach (Loan a in LoanObjects)
            {
                LibraryObjects.Add(await _repo.GetByIdAsync<LibraryObject>(a.ObjectID));
            }
        }

        #endregion
    }
}
