#region Namespaces
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
#endregion
namespace G2Libsys.ViewModels
{

    // Visa info om ett libraryobject med lånemöjligheter 
    public class LibraryObjectInfoViewModel : BaseViewModel, ISubViewModel
    {
        #region Fields
        
        private readonly IRepository _repo;
        public ICommand CancelCommand => new RelayCommand(_ => _navigationService.HostScreen.SubViewModel = null);
        public ICommand AddLoan { get; private set; }

        private LibraryObject currentBook;
        private Card currentUserCard;
        private string buttonstatus;
        //private Author author;

        //private async void GetAuthor()
        //{
        //    if (LibraryObject?.Author == null)
        //    {
        //        return;
        //    }
            
        //    AuthorObject = await _repo.GetByIdAsync<Author>((int)LibraryObject.Author);
        //}
        #endregion       
        #region Constructor
            public string Buttonstatus
        {
            get => buttonstatus;
            set
            {
                buttonstatus = value;
                OnPropertyChanged(nameof(buttonstatus));
            }
        }
       
        public LibraryObjectInfoViewModel()
        {

        }

        

        public LibraryObjectInfoViewModel(LibraryObject libraryObject)
        {
            
            if (libraryObject.Quantity > 0 || libraryObject.Category > 1)
            {
                Buttonstatus = "Låna";
            }

            else
            {
                Buttonstatus = "Slut";

            }
            _repo = new GeneralRepository();
            //author = new Author();
            currentBook = libraryObject;
            //GetAuthor();
            AddLoan = new RelayCommand(_=> AddToCart(), _=> libraryObject.Category >1 || libraryObject.Quantity>0);
            GetCard();
        }
        public async void GetCard()
        {
			if (_navigationService.HostScreen.CurrentUser != null)
			{
            CurrentUserCard = await _repo.GetByIdAsync<Card>(_navigationService.HostScreen.CurrentUser.ID);

			}
        }
        public void AddToCart()
        {
            if (_navigationService.HostScreen.CurrentUser != null)
            {

                if (CurrentUserCard != null && CurrentUserCard.Activated != false)
                {

                    ILoansService _loans = IoC.ServiceProvider.GetService<ILoansService>();

                    _loans.LoanCart.Add(currentBook);
                    _dialog.Alert("Varukorg", $"Tillagd i varukorgen");
                    if (currentBook.Category == 1)
                    {
                        currentBook.Quantity--;
                    }
                    
                    CancelCommand.Execute(null);
                }
                else { _dialog.Alert("Lånekort", "Du har inget Lånekort registrerat eller aktiverat \nVänligen kontakta personalen"); }
            }
            else { _dialog.Alert("Login", "Vänligen logga in för att låna"); }
        }
        #endregion
        #region Methods
        //private async void GetAuthor()
        //{
        //    if (LibraryObject?.Author == null)
        //    {
        //        return;
        //    }
        //}


        //public Author AuthorObject
        //{
        //    get => author;
        //    set
        //    {
        //        author = value;
        //        OnPropertyChanged(nameof(AuthorObject));
        //    }
        //}
        
        public LibraryObject LibraryObject
        {
            get => currentBook;
            set
            {
                currentBook = value;
                OnPropertyChanged(nameof(LibraryObject));
            }
        }
        public Card CurrentUserCard
        {
            get => currentUserCard;
            set
            {
                currentUserCard = value;
                OnPropertyChanged(nameof(CurrentUserCard));
            }
        }
        #endregion
    }
}
