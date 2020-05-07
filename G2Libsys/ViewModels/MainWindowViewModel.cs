using G2Libsys.Commands;
using G2Libsys.Data.Repository;
using G2Libsys.Library;
using G2Libsys.Models;
using G2Libsys.Services;
using G2Libsys.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace G2Libsys.ViewModels
{
    public class MainWindowViewModel : BaseViewModel, IHostScreen
    {
        #region Private Fields

        private readonly IRepository _repo;
        private User currentUser;
        private UserMenuItem menuItem;
        private ObservableCollection<UserMenuItem> menuItems;
        private BaseViewModel currentViewModel;
        private bool developerMode = false;

        #endregion

        #region Public Properties

        /// <summary>
        /// Viewmodels for developer menu
        /// </summary>
        public ObservableCollection<UserMenuItem> ViewModelList { get; set; }

        /// <summary>
        /// Quick navigation for devmenu
        /// </summary>
        public UserMenuItem SelectedDevItem { set => NavigateToVM.Execute(value.VMType); }

        public User CurrentUser
        {
            get => currentUser;
            set
            {
                currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
                OnPropertyChanged(nameof(CanLogIn));
                OnPropertyChanged(nameof(IsLoggedIn));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UserMenuItem MenuItem
        {
            get => menuItem;
            set
            {
                menuItem = value;
                OnPropertyChanged(nameof(MenuItem));
            }
        }

        public ObservableCollection<UserMenuItem> MenuItems
        {
            get => menuItems;
            set
            {
                menuItems = value;
                OnPropertyChanged(nameof(MenuItems));
            }
        }

        /// <summary>
        /// Sets the active viewmodel
        /// </summary>
        public BaseViewModel CurrentViewModel
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
        public bool CanLogIn => CurrentUser == null ? true : !CurrentUser.LoggedIn;

        /// <summary>
        /// Check if user is logged in
        /// </summary>
        public bool IsLoggedIn => CurrentUser == null ? false : CurrentUser.LoggedIn;

        public bool DeveloperMode
        {
            get => developerMode;
            set
            {
                developerMode = value;
                OnPropertyChanged(nameof(DeveloperMode));
            }
        }

        #endregion

        #endregion

        #region Public Commands

        /// <summary>
        /// Call logout method
        /// </summary>
        public ICommand LogOutCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindowViewModel()
        {
            _repo = new GeneralRepository();

            Initialize();
        }

        #endregion

        public void Initialize()
        {
            // Enable dev menu
            DeveloperMode = false;
            SetDevViewModels();

            // Set MainWindowViewModel to hostscreen
            //HostScreen = this;

            // Initial viewmodel 
            CurrentViewModel = new LibraryMainViewModel();

            // Set logout command
            LogOutCommand = new RelayCommand(x => LogOut());

            // Aplication closing event handler
            Application.Current.MainWindow.Closing
                += new CancelEventHandler((o, e) =>
                {
                    LogOut();
                });
        }

        private void LogOut()
        {
            if (CurrentUser is null) return;

            CurrentUser.LoggedIn = false;
            _repo.UpdateAsync(CurrentUser).ConfigureAwait(false);
            CurrentUser = null;
            NavigateToVM.Execute(typeof(LibraryMainViewModel));
        }

        /// <summary>
        /// Set viewmodels for developer menu
        /// </summary>
        private void SetDevViewModels()
        {
            // Fill with needed viewmodels
            ViewModelList = new ObservableCollection<UserMenuItem>
            {
                new UserMenuItem(new TestVM()),
                new UserMenuItem(new LoginViewModel()),
                new UserMenuItem(new AdminViewModel())
            };
        }
    }
}
