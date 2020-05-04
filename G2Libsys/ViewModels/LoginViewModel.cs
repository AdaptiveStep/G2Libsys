using G2Libsys.Commands;
using G2Libsys.Data.Repository;
using G2Libsys.Library;
using G2Libsys.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using G2Libsys.Library.Extensions;
using System.Windows;

namespace G2Libsys.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Private Fields
        private readonly IUserRepository _repo;
        private string username;
        private string password;
        private User newUser;
        private bool invalidEmail;
        private string emailValidationMessage;
        #endregion

        #region Public Properties
        /// <summary>
        /// User email
        /// </summary>
        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        /// <summary>
        /// User password
        /// TODO: Change to secure password and passwordbox
        /// </summary>
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public User NewUser
        {
            get => newUser;
            set
            {
                newUser = value;
                Username = string.Empty;
                Password = string.Empty;
                OnPropertyChanged(nameof(NewUser));
            }
        }

        public bool InvalidEmail
        {
            get => invalidEmail;
            set
            {
                invalidEmail = value;
                OnPropertyChanged(nameof(InvalidEmail));
            }
        }

        public string EmailValidationMessage
        {
            get => emailValidationMessage;
            set
            {
                emailValidationMessage = value;
                OnPropertyChanged(nameof(EmailValidationMessage));
            }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Login user command
        /// </summary>
        public ICommand LogIn { get; set; }

        /// <summary>
        /// Verify if canExecute command
        /// </summary>
        private Predicate<object> CanLogin =>
            o => !string.IsNullOrWhiteSpace(Username)
              && !string.IsNullOrWhiteSpace(Password);

        /// <summary>
        /// Register new user command
        /// </summary>
        public ICommand Register { get; set; }

        /// <summary>
        /// Verify if canExecute command
        /// </summary>
        private Predicate<object> CanRegister =>
            o => !string.IsNullOrWhiteSpace(NewUser.Firstname)
              && !string.IsNullOrWhiteSpace(NewUser.Lastname)
              && !string.IsNullOrWhiteSpace(NewUser.Email)
              && !string.IsNullOrWhiteSpace(NewUser.Password);

        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginViewModel()
        {
            _repo = new UserRepository();
            NewUser = new User();

            LogIn = new RelayCommand(_ => VerifyLogin(), CanLogin);
            Register = new RelayCommand(_ => VerifyRegister(), CanRegister);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Verify user credentials and login
        /// </summary>
        private async void VerifyLogin()
        {
            //var b = await _repo.VerifyEmailAsync(Username);

            var hostScreen = MainWindowViewModel.HostScreen;

            var user = await _repo.VerifyLoginAsync(Username, Password);

            if (user != null)
            {
                user.LoggedIn = true;

                await _repo.UpdateAsync(user).ConfigureAwait(false);

                hostScreen.CurrentUser = user;
                hostScreen.MenuItem = GetUserAccess(user.ID);

                // On successfull login go to frontpage
                NavigateToVM.Execute(typeof(FrontPageViewModel));
            }

            // Reset Username and Password
            Username = string.Empty;
            Password = string.Empty;
        }

        /// <summary>
        /// Verify user registering credentials
        /// </summary>
        private async void VerifyRegister()
        {
            if (!NewUser.Email.IsValidEmail())
            {
                // Email is not valid
                EmailValidationMessage = "Ogiltig mailadress";
            }
            else if (await _repo.VerifyEmailAsync(NewUser.Email))
            {
                // Email already exists in database
                EmailValidationMessage = "Mailadressen finns redan";
            }
            else
            {
                // Call
                RegisterUser();
            }
        }

        /// <summary>
        /// Try to register new user
        /// </summary>
        private async void RegisterUser()
        {
            try
            {
                // Insert new user
                await _repo.AddAsync(NewUser);
                MessageBox.Show("Registrerad, logga in med: \n" + NewUser.Email);
            }
            catch (Exception ex)
            {
                // Insert failed
                MessageBox.Show("Kunde inte lägga till användare\n" + ex.Message);
            }
            finally
            {
                // Reset NewUser
                NewUser = new User();
            }
        }

        /// <summary>
        /// Switch expression that returns user viewmodel access based on UserType
        /// </summary>
        /// <param name="id">UserTypeID</param>
        private UserMenuItem GetUserAccess(int id) => id switch
        {
            1 => new UserMenuItem(new AdminViewModel(), "Admin"), // Case 1
            2 => new UserMenuItem(new TestVM(), "Bibliotekarie"), // Case 2
            3 => new UserMenuItem(new TestVM(), "Mina lån"), // Case 3
            _ => new UserMenuItem(new TestVM(), "Fel"), // Default
        };

        #endregion
    }
}
