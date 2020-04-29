using G2Libsys.Data.Repository;
using G2Libsys.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace G2Libsys.ViewModels
{
    public class LibrarianViewModel: BaseViewModel
    {
        private readonly UserRepository _repo;

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
            _repo = new UserRepository();
            GetUsers();
        }
        
        public async void GetUsers()
        {
            Users = new ObservableCollection<User>(await _repo.GetAllAsync());
            
        }
        //lista av besökare
        //ta bort besökare funktion
        //lägga till besökare funktion
        //söka efter besökare. antigen en funtion eller i datagrid
        //registrera lånekort
    }
}
