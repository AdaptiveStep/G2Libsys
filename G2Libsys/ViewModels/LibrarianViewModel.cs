using G2Libsys.Data.Repository;
using G2Libsys.Library;
using G2Libsys.Commands;
using System;
using System.Collections.ObjectModel;

namespace G2Libsys.ViewModels
{
    public class LibrarianViewModel: BaseViewModel
    {
        public RelayCommand addbutton { get; private set; }
        public RelayCommand deletebutton { get; private set; }
        private readonly GeneralRepository<User> _repo;

        //binda knapparna från viewen
        //skapa datagrid
        private ObservableCollection<User> _users = new ObservableCollection<User>();
        
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set { _users = value; }
        }
        public LibrarianViewModel()
        {
            addbutton = new RelayCommand(x => AddUser());
            deletebutton = new RelayCommand(DeleteUser);
            _repo = new GeneralRepository<User>();
            GetUsers();
        }
        
        public async void GetUsers()
        {
            Users = new ObservableCollection<User>(await _repo.GetAllAsync());

        }

        
        public async void DeleteUser(object parameter)
        {
            await _repo.RemoveAsync(parameter);
        }
        public async void AddUser()
        {
            //await _repo.AddAsync();
        }
        //lista av besökare
        //ta bort besökare funktion
        //lägga till besökare funktion
        //söka efter besökare. antigen en funtion eller i datagrid
        //registrera lånekort

        public string name;
        public string lastname;
        public string email;
        public int ssnumber;
        public string remail;
    }
}
