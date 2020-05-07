﻿using G2Libsys.Data.Repository;
using G2Libsys.Library;
using G2Libsys.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Linq;

namespace G2Libsys.ViewModels
{
    public class LibrarianViewModel: BaseViewModel
    {
        public ICommand addbutton { get; private set; }
        public ICommand deletebutton { get; private set; }
        private readonly IRepository<User> _repo;

        //binda knapparna från viewen
        //skapa datagrid
        private ObservableCollection<User> _users;
        private User newUser;
        public User NewUser 
        { 
            get => newUser;
            set { newUser = value;
                OnPropertyChanged(nameof(NewUser));
            }
        }
        private User oldUser;
        public User OldUser
        {
            get => oldUser;
            set
            {
                oldUser = value;
                OnPropertyChanged(nameof(oldUser));
            }
        }
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set { _users = value; }
        }
        public LibrarianViewModel()
        {
            _repo = new GeneralRepository<User>();
            Users = new ObservableCollection<User>();
            Users.CollectionChanged += Users_CollectionChanged;
            NewUser = new User();
            GetUsers();
            addbutton = new RelayCommand(x => AddUser());
            deletebutton = new RelayCommand(x=>DeleteUser());
            
          
        }

        private void Users_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            if (e.OldItems != null)
                foreach (INotifyPropertyChanged i in e.OldItems)
                    i.PropertyChanged -= itempropchanged;
            if (e.NewItems != null)
                foreach (INotifyPropertyChanged i in e.NewItems)
                    i.PropertyChanged -= itempropchanged;

        }
        void itempropchanged(object sender, PropertyChangedEventArgs e) { }

        public async void GetUsers()
        {
            Users = new ObservableCollection<User>((await _repo.GetAllAsync()).ToList().Where(x => x.UserType == 3));
            OnPropertyChanged(nameof(Users));
        }

        
        public async void DeleteUser()
        {
            if (OldUser != null)
                await _repo.RemoveAsync(OldUser);
            GetUsers();
        }
        public async void AddUser()
        {
           await _repo.AddAsync(NewUser);
            GetUsers();
        }
        //lista av besökare
        //ta bort besökare funktion
        //lägga till besökare funktion
        //söka efter besökare. antigen en funtion eller i datagrid
        //registrera lånekort

       
    }
}
