using G2Libsys.Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace G2Libsys.ViewModels
{
    public class LibraryObjectViewModel : BaseViewModel
    {
        private ObservableCollection<LibraryObject> libraryObjects;
        private string testing1;
        private Category currentCategory;

        public string testing
        {
            get => testing1;
            set
            {
                testing1 = value;
                OnPropertyChanged(nameof(testing));
            }
        }

        public Category CurrentCategory
        {
            get => currentCategory;
            set
            {
                currentCategory = value;
                OnPropertyChanged(nameof(CurrentCategory));
            }
        }

        public ObservableCollection<LibraryObject> LibraryObjects
        {
            get => libraryObjects;
            set
            {
                libraryObjects = value;
                OnPropertyChanged(nameof(LibraryObjects));
            }
        }

        public List<LibraryObject> TempList { get; set; }

        public LibraryObjectViewModel()
        {
            testing = "Test";

            var parent = (LibraryMainViewModel)MainWindowViewModel.HostScreen.CurrentViewModel;

            CurrentCategory = parent.SelectedSearchCategory;

            TempList = new List<LibraryObject>()
            {
                new LibraryObject(1, 1),
                new LibraryObject(2, 1),
                new LibraryObject(3, 1),
                new LibraryObject(4, 2),
                new LibraryObject(5, 2),
                new LibraryObject(6, 2),
                new LibraryObject(7, 3),
                new LibraryObject(8, 3),
                new LibraryObject(9, 3)
            };

            LibraryObjects = new ObservableCollection<LibraryObject>(TempList.Where(o => o.CategoryID == CurrentCategory.ID));
        }
    }
}
