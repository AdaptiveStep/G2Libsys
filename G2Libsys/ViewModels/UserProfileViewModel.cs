using G2Libsys.Commands;
using G2Libsys.Data.Repository;
using G2Libsys.Library;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace G2Libsys.ViewModels
{
    // Hantera användarens egna info
    public class UserProfileViewModel : BaseViewModel
    {
        public ICommand Changebutton { get; private set; }
        public ICommand SavePasswordbutton { get; private set; }
        public ICommand SaveEmailbutton { get; private set; }
        public ICommand Cancelbutton { get; private set; }

        private readonly IRepository<User> _repo;
        private string newValues;

        public string NewValues
        {
            get => newValues;
            set
            {
                newValues = value;
                OnPropertyChanged(nameof(NewValues));
            }
        }
        private User currentUser;
        public User CurrentUser
        {
            get => currentUser;
            set
            {
                currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }

        public UserProfileViewModel()
        {
            _repo = new GeneralRepository<User>();
            //CurrentUser = 
            //Changebutton = new RelayCommand(x => );
            //Cancelbutton = new RelayCommand(x => );
            SavePasswordbutton = new RelayCommand(x => ChangePassword());
            SaveEmailbutton = new RelayCommand(x => ChangeEmail());
        }

        public async void ChangePassword()
        {
           await _repo.UpdateAsync(CurrentUser);
        }
        public async void ChangeEmail()
        {
            await _repo.UpdateAsync(CurrentUser);
        }
    }
}
