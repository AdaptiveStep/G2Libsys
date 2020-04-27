using G2Libsys.Commands;
using G2Libsys.Data.Repository;
using G2Libsys.Library;
using G2Libsys.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace G2Libsys.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Privates

        private object currentViewModel;

        #endregion

        #region Public Properties

        /// <summary>
        /// Property for calling the mainviewmodel for navigation purposes
        /// </summary>
        public static MainWindowViewModel HostScreen { get; set; }

        /// <summary>
        /// Sets the active viewmodel
        /// </summary>
        public object CurrentViewModel
        {
            get => currentViewModel;
            set
            {
                currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindowViewModel()
        {
            HostScreen = this;

            CurrentViewModel = new FrontPageViewModel();
        }
    }
}
