using System;
using System.Collections.Generic;
using System.Text;

namespace G2Libsys.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            MainWindowViewModel.HostScreen.IsLoggedIn = true;
        }
    }
}
