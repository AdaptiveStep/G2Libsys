using G2Libsys.Commands;
using G2Libsys.Data.Repository;
using G2Libsys.Library;
using G2Libsys.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;

namespace G2Libsys.ViewModels
{
    class LoanCheckoutViewModel : BaseViewModel, ISubViewModel
    {
        public ICommand CancelCommand => new RelayCommand(_ => _navigationService.HostScreen.SubViewModel = null);
        private readonly IRepository _repo;
        ILoansService _loans = IoC.ServiceProvider.GetService<ILoansService>();
        private Card currentUserCard;
        private ObservableCollection<Loan> loanCart;

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
            _loans = new LoansServices();
            GetUser();
            

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

        public async void ConfirmLoan()
        {
            foreach(LibraryObject a in _loans.LoanCart)
            {
                LoanCart.Add(new Loan { ObjectID = a.ID, CardID = CurrentUserCard.ID });
            }
            
            await _repo.AddRangeAsync(LoanCart);

           

            _dialog.Alert("", "Dina lån är nu skapade");
            LoanCart.Clear();
        }
    }
}
