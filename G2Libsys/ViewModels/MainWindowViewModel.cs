﻿using G2Libsys.Commands;
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
using System.Windows.Threading;

namespace G2Libsys.ViewModels
{
    public class MainWindowViewModel : BaseViewModel, IHostScreen
    {
        #region Private Fields

        private readonly IRepository _repo;
        private User currentUser;
        private ObservableCollection<UserMenuItem> menuItems;
        private IViewModel currentViewModel;
        private bool developerMode = false;
        private IViewModel subViewModel;
        private Dispatcher dispatcher;

        #endregion

        #region Public Properties

        /// <summary>
        /// Viewmodels for developer menu
        /// </summary>
        public List<UserMenuItem> ViewModelList { get; set; }

        /// <summary>
        /// Quick navigation for devmenu
        /// </summary>
        public UserMenuItem SelectedDevItem { set => value.Action.Execute(null); }

        /// <summary>
        /// Active user
        /// </summary>
        public User CurrentUser
        {
            get => currentUser;
            set
            {
                currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
                OnPropertyChanged(nameof(CanLogIn));
                OnPropertyChanged(nameof(IsLoggedIn));

                // Set user viewmodel access
                if (value != null) dispatcher.Invoke(SetUserAccess);
            }
        }

        /// <summary>
        /// User navigation access
        /// </summary>
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
        public IViewModel CurrentViewModel
        {
            get => currentViewModel;
            set
            {
                currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        /// <summary>
        /// Sets the active subviewmodel
        /// </summary>
        public IViewModel SubViewModel
        {
            get => subViewModel;
            set
            {
                subViewModel = value;
                OnPropertyChanged(nameof(SubViewModel));
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

        public ICommand GoToFrontPage => new RelayCommand(_ =>
        {
            if (!(CurrentViewModel is LibraryMainViewModel))
                NavService.HostScreen.CurrentViewModel = NavService.GetViewModel(new LibraryMainViewModel());
            else
            {
                var viewModel = (LibraryMainViewModel)CurrentViewModel;
                viewModel.FrontPage = true;
            }
        });

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

        #region Setup Methods
        /// <summary>
        /// Initial setup
        /// </summary>
        private void Initialize()
        {
            // Initialize navservice and set hostscreen to this MainWindowViewModel
            NavService.Setup(this);

            // Enable dev menu
            DeveloperMode = true;
            DevelopSetup();

            // Initial viewmodel 
            CurrentViewModel = NavService.GetViewModel(new LibraryMainViewModel());

            // Initiate menuitems list
            MenuItems = new ObservableCollection<UserMenuItem>();

            // Set logout command
            LogOutCommand = new RelayCommand(x => LogOut());

            // Get dispatcher
            dispatcher = Application.Current.Dispatcher;

            // Aplication closing event handler
            Application.Current.MainWindow.Closing
                += new CancelEventHandler((o, e) =>
                {
                    LogOut();
                });
        }

        /// <summary>
        /// Setup user navigation access
        /// </summary>
        private void SetUserAccess()
        {
            MenuItems ??= new ObservableCollection<UserMenuItem>();

            if (MenuItems.Count > 0)
            {
                MenuItems.Clear();
            }

            // Create UserMenuItems
            MenuItems.Add(new UserMenuItem(new UserProfileViewModel(), "Profil"));
            MenuItems.Add(new UserMenuItem(new UserReservationsViewModel(), "Mina lån"));

            (CurrentUser.UserType switch
            {
                1 => new List<UserMenuItem>() // Admin
                {
                    new UserMenuItem(new AdminViewModel(), "Användare"),
                    new UserMenuItem(new LibraryObjectAdministrationViewModel(), "Produkter"),
                },
                2 => new List<UserMenuItem>() // Librarian
                {
                    new UserMenuItem(new LibrarianViewModel(), "Användare"),
                    new UserMenuItem(new LibraryObjectAdministrationViewModel(), "Produkter"),
                },
                3 => new List<UserMenuItem>() // Visitor
                {
                    // Implementera eventuella unika viewmodels för endast användare
                },
                _ => new List<UserMenuItem>() // Other
                {
                    new UserMenuItem(new TestVM(), "Saknas")
                }
            }).ForEach(u => MenuItems.Add(u));

            MenuItems.Add(new UserMenuItem(new FrontPageViewModel(), "Logga ut", LogOutCommand));
        }

        /// <summary>
        /// Set viewmodels for developer menu
        /// </summary>
        private void DevelopSetup()
        {
            // Fill with needed viewmodels
            ViewModelList = new List<UserMenuItem>
            {
                new UserMenuItem(new AdminViewModel()),
                new UserMenuItem(new LibraryObjectInfoViewModel(), "ObjectInfo"),
                new UserMenuItem(new LibraryObjectAdministrationViewModel(), "ObjectsAdmin"),
                new UserMenuItem(new UserProfileViewModel(), "Profile"),
                new UserMenuItem(new UserReservationsViewModel(), "UserLoans")
            };
        }

        #endregion

        #region Functions

        /// <summary>
        /// Execution logic for user logout
        /// </summary>
        private void LogOut()
        {
            if (CurrentUser is null) return;

            CurrentUser.LoggedIn = false;
            _repo.UpdateAsync(CurrentUser).ConfigureAwait(false);
            CurrentUser = null;
            NavigateToVM.Execute(typeof(LibraryMainViewModel));
        }

        #endregion
    }
}
