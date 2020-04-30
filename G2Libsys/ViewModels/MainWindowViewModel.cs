using G2Libsys.Commands;
using G2Libsys.Data.Repository;
using G2Libsys.Library;
using G2Libsys.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace G2Libsys.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Privates

        private object currentViewModel;
        private bool isLoggedIn;

        #endregion

        #region Public Properties

        /// <summary>
        /// Property for calling the mainviewmodel for navigation purposes
        /// </summary>
        public static MainWindowViewModel HostScreen { get; set; }

        public IRepository<User> repoUser = new GeneralRepository<User>();
        public IRepository repoGen  = new GeneralRepository();
        public IUserRepository userRepo = new UserRepository();


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
                OnPropertyChanged(nameof(CanLogIn));
            }
        }

        #endregion

        #endregion

        #region Public Commands

        public ICommand LogOutCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindowViewModel()
        {
            Initialize();

            LogOutCommand = new RelayCommand(x => LogOut());
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

        private void LogOut()
        {
            IsLoggedIn = false;
            NavigateToVM.Execute(typeof(FrontPageViewModel));
        }
    }
}
