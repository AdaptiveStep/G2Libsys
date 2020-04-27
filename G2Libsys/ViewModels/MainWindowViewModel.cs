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

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindowViewModel()
        {
            // Exempelkod använder temporär databas
            _userRepo = new UserRepository();

            //GetUsers();
            //InsertUser();
            // --------------------------------

            // Set MainWindowViewModel to hostscreen
            HostScreen = this;

            // Initial viewmodel 
            //NavigateToVM.Execute(typeof(FrontPageViewModel));
            CurrentViewModel = new FrontPageViewModel();
        }

        #endregion

        // Exempelkod använder temporär databas
        private async void GetUsers()
        {
            List<User> userlist = new List<User>(await _userRepo.GetAllAsync());
        }

        private async void InsertUser()
        {
            var user = new User { Name = "Olja" };
            user.ID = await _userRepo.AddAsync(user);
        }
    }
}
