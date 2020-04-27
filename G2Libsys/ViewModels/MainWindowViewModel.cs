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
        private readonly IRepository _repository;
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
            _repository = new GeneralRepository();
            _userRepo = new UserRepository();

            // Exempelkod använder temporär databas
            GetUsers();
            //InsertUser();

            // Initial viewmodel 
            //CurrentViewModel = new FrontPageViewModel();

            //HostScreen = this;
        }

        // Exempelkod använder temporär databas
        private async void GetUsers()
        {
            List<User> userlist = new List<User>(await _userRepo.GetAllAsync());
            List<User> list = new List<User>(await _repository.GetAllAsync<User>());
        }

        private async void InsertUser()
        {
            var user = new User { Email = "bob5@g2systems.com", Firstname = "Bob", Lastname = "Ross", Password = "123" };
            var user2 = new User { Email = "bert3@g2systems.com", Firstname = "Bert", Lastname = "Karlsson", Password = "123" };
            user.ID = await _userRepo.AddAsync(user);
            user2.ID = await _repository.AddAsync(user2);
        }
    }
}
