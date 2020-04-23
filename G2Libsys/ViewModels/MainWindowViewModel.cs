using G2Libsys.Commands;
using G2Libsys.Data.Repository;
using G2Libsys.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace G2Libsys.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public static MainWindowViewModel HostScreen { get; set; }

        private object currentViewModel;
        private readonly UserRepository _userRepo;

        public object CurrentViewModel
        {
            get => currentViewModel;
            set
            {
                currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public MainWindowViewModel()
        {
            _userRepo = new UserRepository();

            // Exempelkod
            GetUsers();
            InsertUser();
         
            // Initial viewmodel 
            CurrentViewModel = new FrontPageViewModel();

            HostScreen = this;
        }

        // Exempelkod
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
