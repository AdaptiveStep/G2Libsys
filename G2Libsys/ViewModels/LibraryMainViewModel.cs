using G2Libsys.Commands;
using G2Libsys.Data.Repository;
using G2Libsys.Library;
using G2Libsys.Library.Models;
using G2Libsys.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace G2Libsys.ViewModels
{
    public class LibraryMainViewModel : BaseViewModel
    {
        public ICommand BookButton { get; private set; }
        private readonly IRepository _repo;

        private bool frontPage;

        private ObservableCollection<LibraryObject> libObjects;

        public ObservableCollection<LibraryObject> LibraryObjects
        {
            get => libObjects;
            set
            {
                libObjects = value;
                OnPropertyChanged(nameof(LibraryObjects));
            }
        }
        public bool FrontPage
        {
            get => frontPage;
            set
            {
                frontPage = value;
                OnPropertyChanged(nameof(FrontPage));
                OnPropertyChanged(nameof(SearchPage));
            }
        }

        public bool SearchPage => !FrontPage;


        public ICommand SearchCommand => new RelayCommand(_ => Search());

        private void Search()
        {
            if (FrontPage)
                FrontPage = false;
            else
                FrontPage = true;
        }

        public LibraryMainViewModel()
        {
            FrontPage = true;
            LibraryObjects = new ObservableCollection<LibraryObject>();
            _repo = new GeneralRepository();
            GetLibraryObjects();
            BookButton = new RelayCommand(x => BookButtonClick());
            
        }

        public LibraryObject SelectedLibraryObject
        {
            set => MessageBox.Show(value.ID.ToString());
        }
        public void BookButtonClick()
        {
            MessageBox.Show("Snyggt klickat");
        }
      /// <summary>
        /// hämtar alla library objects ifrån databasen
        /// </summary>
        private async void GetLibraryObjects()
        {
            LibraryObjects = new ObservableCollection<LibraryObject>(await _repo.GetAllAsync<LibraryObject>());
        }
    }
}
