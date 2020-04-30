using G2Libsys.Library;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace G2Libsys.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            var Users = new List<User>() {
                new User()
                {
                    Email = "bob@gmail.com",
                    Firstname = "Bob",
                    Lastname = "Anka",
                    UserType = new UserType() { ID = 1, Name = "Admin" }
                },
                new User()
                {
                    Email = "pelle@gmail.com",
                    Firstname = "Pelle",
                    Lastname = "Planka",
                    UserType = new UserType() { ID = 2, Name = "Bibliotekarie" }
                },
                new User()
                {
                    Email = "guest@gmail.com",
                    Firstname = "Gäst",
                    Lastname = "Låst",
                    UserType = new UserType() { ID = 3, Name = "Användare" }
                }
            };

            var hostScreen = MainWindowViewModel.HostScreen;
            var rnd = new Random();
            hostScreen.IsLoggedIn = true;
            hostScreen.CurrentUser = Users[rnd.Next(0, 3)];
            hostScreen.UserType = GetUserAccess(hostScreen.CurrentUser.UserType.ID);
        }

        public Type GetUserAccess(int id)
        {
            switch (id)
            {
                case 1:
                    return typeof(AdminViewModel);
                case 2:
                    return typeof(TestVM);
                case 3:
                    return typeof(TestVM);
                default:
                    return typeof(FrontPageViewModel);
            }
        }
    }
}
