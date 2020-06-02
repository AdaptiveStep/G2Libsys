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
        private readonly IRepository _repo;
        private readonly ILoansService _loans;
        private Card currentUserCard;
        private ObservableCollection<Loan> loanCart;
        private ObservableCollection<LibraryObject> loanObj;
        private LibraryObject selectedItem;

        public ICommand Confirm { get; set; }
        public ICommand DeleteItem { get; set; }
        public ICommand Clear { get; set; }
        public ICommand CancelCommand => new RelayCommand(_ => _navigationService.HostScreen.CurrentViewModel = _navigationService.CreateNewInstance(new LibraryMainViewModel()));


        public LibraryObject SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        
        public ObservableCollection<LibraryObject> LoanObj
        {
            get => loanObj;
            set
            {
                loanObj = value;
                OnPropertyChanged(nameof(LoanObj));
            }
        }

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

        private void DeleteLoan()
        {
            _loans.LoanCart.Remove(SelectedItem);
            LoanObj.Remove(SelectedItem);
        }
        
        private async void GetUser()
        {

            if (_navigationService.HostScreen.CurrentUser != null)
            {
                
                CurrentUserCard = await _repo.GetByIdAsync<Card>(_navigationService.HostScreen.CurrentUser.ID);
            }
        }
        //public void AddToCart()
        //{
        //    if (_navigationService.HostScreen.CurrentUser != null)
        //    {
        //        //LoanCart.Add();

        //        _dialog.Alert("", "Tillagd i varukorgen");
        //    }
        //    else { _dialog.Alert("", "Vänligen logga in för att låna"); }
        //}
        
        public void ClearLoan()
        {
            LoanObj.Clear();
            
            _loans.LoanCart.Clear();

            CancelCommand.Execute(null);


        }
       
        public async void ConfirmLoan()
        {
            if (LoanObj.Count > 0)
            {
                LoanCart = new ObservableCollection<Loan>();
                foreach (LibraryObject a in LoanObj)
                {
                    LoanCart.Add(new Loan { ObjectID = a.ID, CardID = CurrentUserCard.ID, LoanDate = DateTime.Now });
                }
                await _repo.AddRangeAsync(LoanCart);

                _dialog.Alert("", "Dina lån är nu skapade");
                LoanCart.Clear();
                LoanObj.Clear();
                _loans.LoanCart.Clear();

                CancelCommand.Execute(null);
            }
            else 
            { 
                _dialog.Alert("", "Inga lån tillagda");
                CancelCommand.Execute(null);
            }
        }
    }
}
