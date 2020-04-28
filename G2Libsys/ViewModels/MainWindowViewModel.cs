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
        private bool isLoggedIn;
        private readonly UserRepository _userRepo;

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

        #region Bools

        /// <summary>
        /// True if user is not already logged in
        /// </summary>
        public bool CanLogIn
        {
            get => !isLoggedIn;
            set
            {
                isLoggedIn = !value;
                OnPropertyChanged(nameof(CanLogIn));
            }
        }

        /// <summary>
        /// Check if user is logged in
        /// </summary>
        public bool IsLoggedIn
        {
            get => isLoggedIn;
            set
            {
                isLoggedIn = value;
                OnPropertyChanged(nameof(IsLoggedIn));
            }
        }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindowViewModel()
        {
            Initialize();

        }

        #endregion

        public void Initialize()
        {
            // Set MainWindowViewModel to hostscreen
            HostScreen = this;

            IsLoggedIn = false;

            // Initial viewmodel 
            //NavigateToVM.Execute(typeof(FrontPageViewModel));
            CurrentViewModel = new FrontPageViewModel();
        }
    }
}
