using G2Libsys.Commands;
using G2Libsys.Data.Repository;
using G2Libsys.Library;
using G2Libsys.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace G2Libsys.ViewModels
{
    public class LoanCheckoutViewModel : BaseViewModel, IViewModel
    {
        #region fields
        private readonly IRepository _repo;
        private readonly ILoansService _loans;
        private Card currentUserCard;
        private ObservableCollection<Loan> loanCart;
        private ObservableCollection<LibraryObject> loanObj;
        private LibraryObject selectedItem;
        #endregion

        #region commands
        public ICommand Confirm { get; set; }
        public ICommand DeleteItem { get; set; }
        public ICommand Clear { get; set; }
        public ICommand CancelCommand => new RelayCommand(_ => _navigationService.HostScreen.SubViewModel = null);
        #endregion

        #region properties
        public LibraryObject SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        /// <summary>
        /// collection of libraryobject
        /// </summary>
        public ObservableCollection<LibraryObject> LoanObj
        {
            get => loanObj;
            set
            {
                loanObj = value;
                OnPropertyChanged(nameof(LoanObj));
            }
        }
        /// <summary>
        /// collection of loans
        /// </summary>
        public ObservableCollection<Loan> LoanCart
        {
            get => loanCart;
            set
            {
                loanCart = value;
                OnPropertyChanged(nameof(LoanCart));
            }
        }

        public Card CurrentUserCard
        {
            get => currentUserCard;
            set
            {
                currentUserCard = value;
                OnPropertyChanged(nameof(currentUserCard));
            }
        }
        #endregion

        #region Constructor
        public LoanCheckoutViewModel()
        {
            if (base.IsInDesignMode) return;

            _repo = new GeneralRepository();
            _loans = IoC.ServiceProvider.GetService<ILoansService>();
            LoanObj = _loans.LoanCart;
            GetUser();
            Confirm = new RelayCommand(_ => ConfirmLoan());
            Clear = new RelayCommand(_ => ClearLoan());
            DeleteItem = new RelayCommand(_ => DeleteLoan());
        }
        #endregion

        #region methods

        /// <summary>
        /// delete selected loan
        /// </summary>
        private void DeleteLoan()
        {
            _loans.LoanCart.Remove(SelectedItem);
            LoanObj.Remove(SelectedItem);
        }
        /// <summary>
        /// get user card for current user so loans can be created on the card
        /// </summary>
        private async void GetUser()
        {

            if (_navigationService.HostScreen.CurrentUser != null)
            {
                
                CurrentUserCard = await _repo.GetByIdAsync<Card>(_navigationService.HostScreen.CurrentUser.ID);
            }
        }
       
        /// <summary>
        /// clear all loans and close view
        /// </summary>
        public void ClearLoan()
        {
            LoanObj.Clear();
            
            _loans.LoanCart.Clear();

            CancelCommand.Execute(null);


        }
       
        /// <summary>
        /// add loans to database
        /// </summary>
        public async void ConfirmLoan()
        {
            if (LoanObj.Count > 0)
            {
                LoanCart = new ObservableCollection<Loan>();
                //create loanobjects foreach library object that are selected
                foreach (LibraryObject a in LoanObj)
                {
                    LoanCart.Add(new Loan { ObjectID = a.ID, CardID = CurrentUserCard.ID, LoanDate = DateTime.Now });
                }
                await _repo.AddRangeAsync(LoanCart);

                _dialog.Alert("", "Dina lån är nu skapade");
                LoanCart.Clear();
                LoanObj.Clear();
                _loans.LoanCart.Clear();

                // close view when loans are saved
                CancelCommand.Execute(null);
            }
            else 
            { 
                _dialog.Alert("", "Inga lån tillagda");
                
                CancelCommand.Execute(null);
            }

        }
        #endregion
    }
}
