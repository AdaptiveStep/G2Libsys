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
    public class LibraryObjectInfoViewModel : BaseViewModel, IViewModel
    {
        private ICommand exitButtonCommand;
        public ICommand ExitButtonCommand => exitButtonCommand ??= new RelayCommand(x => ExitButton());

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

        private void ExitButton()
        {
            NavService.HostScreen.SubViewModel = null;
        }
    }
}
