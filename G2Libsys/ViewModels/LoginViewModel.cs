namespace G2Libsys.ViewModels
{
    #region Namespaces
    using System;
    using System.Windows.Input;
    using G2Libsys.Library.Extensions;
    using G2Libsys.Services;
    using G2Libsys.Commands;
    using G2Libsys.Data.Repository;
    using G2Libsys.Library;
    #endregion

    /// <summary>
    /// Viewmodel for logging in user
    /// </summary>
    public class LoginViewModel : BaseViewModel, ISubViewModel
    {
        #region Private Fields
        private readonly IUserRepository _repo;
        private readonly IDialogService _dialog;
        private string username;
        private string password;
        private string emailValidationMessage;
        private User newUser;
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

        /// <summary>
        /// For registring new user
        /// </summary>
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

        /// <summary>
        /// Error message for email
        /// </summary>
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
        /// Verify if canExecute login command
        /// </summary>
        private Predicate<object> CanLogin =>
            _ => !string.IsNullOrWhiteSpace(Username)
              && !string.IsNullOrWhiteSpace(Password);

        /// <summary>
        /// Register new user command
        /// </summary>
        public ICommand Register { get; set; }

        /// <summary>
        /// Verify if canExecute Register command
        /// </summary>
        private Predicate<object> CanRegister =>
            _ => !string.IsNullOrWhiteSpace(NewUser.Firstname)
              && !string.IsNullOrWhiteSpace(NewUser.Lastname)
              && !string.IsNullOrWhiteSpace(NewUser.Email)
              && !string.IsNullOrWhiteSpace(NewUser.Password);

        public ICommand CancelCommand => new RelayCommand(_ => NavService.HostScreen.SubViewModel = null);

        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginViewModel()
        {
            if (base.IsInDesignMode) return;

            _dialog = new DialogService();

            _repo = new UserRepository();

            EmailValidationMessage = string.Empty;
            NewUser = new User();

            // Create commands
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
            // Check for user with correct credentials
            var user = await _repo.VerifyLoginAsync(Username, Password);

            if (user is null)
            {
                _dialog.Alert("Fel lösenord", "Försök igen.");
            }
            else
            {
                // Set userstatus to logged in
                user.LoggedIn = true;

                // Update userstatus in db
                await _repo.UpdateAsync(user).ConfigureAwait(false);

                // Set current active user
                NavService.HostScreen.CurrentUser = user;

                // On successfull login go to frontpage
                NavService.GoToAndReset(new LibraryMainViewModel());

                // Exit LoginViewModel
                CancelCommand.Execute(null);
            }

            // Reset new user
            EmailValidationMessage = string.Empty;
            NewUser = new User();
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
                EmailValidationMessage = string.Empty;
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
                _dialog.Alert("Registrerad", "Logga in med: " + NewUser.Email);
            }
            catch (Exception ex)
            {
                // Insert failed
                _dialog.Alert("Misslyckades", "Kunde inte lägga till användare:\n" + ex.Message);
            }
            finally
            {
                // Reset NewUser
                NewUser = new User();
            }
        }

        #endregion
    }
}
