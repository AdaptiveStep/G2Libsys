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
        private readonly IRepository _repo;

        #endregion

        #region Public Properties
        #endregion

        #region Public Properties

        public LoginViewModel()
        {
            _repo = new GeneralRepository();

            Getusers();
        }

        #endregion


        #region Public Properties
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public UserMenuItem GetUserAccess(int id)
        {
            switch (id)
            {
                case 1:
                    return new UserMenuItem("Admin", new AdminViewModel());
                case 2:
                    return new UserMenuItem("Bibliotekarie", new TestVM());
                default:
                    return new UserMenuItem("Mina lån", new TestVM());
            }
        }

        private async void Getusers()
        {
            var hostScreen = MainWindowViewModel.HostScreen;

            var rnd = new Random();

            var users = new List<User>();

            users.AddRange(await _repo.GetAllAsync<User>());

            hostScreen.IsLoggedIn = true;
            hostScreen.CurrentUser = users[rnd.Next(0, 3)];
            hostScreen.UserType = GetUserAccess(hostScreen.CurrentUser.ID);
        }

        #endregion
    }
}
