using G2Libsys.Commands;
using G2Libsys.Data.Repository;
using G2Libsys.Library;
using G2Libsys.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace G2Libsys.ViewModels
{

    // Visa info om ett libraryobject med lånemöjligheter 
    public class LibraryObjectInfoViewModel : BaseViewModel, ISubViewModel
    {
        private readonly IRepository _repo;
        public ICommand CancelCommand => new RelayCommand(_ => _navigationService.HostScreen.SubViewModel = null);

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
        public LibraryObjectInfoViewModel()
        {

        }

        

        public LibraryObjectInfoViewModel(LibraryObject libraryObject)
        {
            _repo = new GeneralRepository();
            //author = new Author();
            currentBook = libraryObject;
            //GetAuthor();
           
        }


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

    }
}
