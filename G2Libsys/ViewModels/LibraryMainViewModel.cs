using G2Libsys.Commands;
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
        private bool frontPage;

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
        }
    }
}
