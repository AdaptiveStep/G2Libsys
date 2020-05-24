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
        public LibraryObjectInfoViewModel()
        {

        }

        

        public LibraryObjectInfoViewModel(LibraryObject libraryObject)
        {

            _repo = new GeneralRepository();
            //author = new Author();
            currentBook = libraryObject;
            //GetAuthor();
            AddLoan = new RelayCommand(_ => AddToCart());
        }
        public void AddToCart()
        {
            if (_navigationService.HostScreen.CurrentUser != null)
            {
                ILoansService _loans = IoC.ServiceProvider.GetService<ILoansService>();

                _loans.LoanCart.Add(currentBook);
                _dialog.Alert("", $"Tillagd i varukorgen{ _loans.LoanCart.Count }");
            }
            else { _dialog.Alert("", "Vänligen logga in för att låna"); }
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
        #endregion
    }
}
