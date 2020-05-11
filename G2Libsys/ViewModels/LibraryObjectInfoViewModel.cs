using G2Libsys.Commands;
using G2Libsys.Library.Models;
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
        public ICommand CancelCommand => new RelayCommand(_ => NavService.HostScreen.SubViewModel = null);

        public ObservableCollection<LibraryObject> currentBook;

        public LibraryObjectInfoViewModel()
        {

        }

        public LibraryObjectInfoViewModel(LibraryObject libraryObject)
        {
            currentBook = new ObservableCollection<LibraryObject>();
            var x = libraryObject;
            currentBook.Add(libraryObject);
        }

        
    }
}
