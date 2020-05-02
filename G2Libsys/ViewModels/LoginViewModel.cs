using G2Libsys.Data.Repository;
using G2Libsys.Library;
using G2Libsys.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace G2Libsys.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Private Fields
        private readonly IUserRepository _repo;
        private string username;
        private string password;
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
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public LoginViewModel()
        {
            _repo = new UserRepository();

            Username = "Johan@johan.com";
            password = "25857";

            VerifyLogin();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 
        /// </summary>
        private async void VerifyLogin()
        {
            var hostScreen = MainWindowViewModel.HostScreen;

            var user = await _repo.VerifyLoginAsync(Username, Password);

            if (user != null)
            {
                user.LoggedIn = true;

                await _repo.UpdateAsync(user).ConfigureAwait(false);

                hostScreen.CurrentUser = user;
                hostScreen.MenuItem = GetUserAccess(user.ID);
            }
        }

        /// <summary>
        /// Switch expression that returns user viewmodel access based on UserType
        /// </summary>
        /// <param name="id">UserTypeID</param>
        private UserMenuItem GetUserAccess(int id) => id switch
        {
            1 => new UserMenuItem("Admin", new AdminViewModel()), // Case 1
            2 => new UserMenuItem("Bibliotekarie", new TestVM()), // Case 2
            3 => new UserMenuItem("Mina lån", new TestVM()), // Case 3
            _ => new UserMenuItem("Fel", new TestVM()), // Default
        };

        #endregion
    }
}
