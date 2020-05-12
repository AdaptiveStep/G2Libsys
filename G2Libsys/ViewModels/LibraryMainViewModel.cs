using G2Libsys.Commands;
using G2Libsys.Data.Repository;
using G2Libsys.Library.Models;
using G2Libsys.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace G2Libsys.ViewModels
{
    public class LibraryMainViewModel : BaseViewModel, IViewModel
    {
        public ICommand BookButton { get; private set; }
        private readonly IRepository _repo;

        private bool frontPage;
        private ObservableCollection<LibraryObject> fpLibObjects;
        private ObservableCollection<LibraryObject> libObjects;

        public ObservableCollection<LibraryObject> FpLibraryObjects
        {
            get => fpLibObjects;
            set { fpLibObjects = value;}
        }
        public ObservableCollection<LibraryObject> LibraryObjects
        {
            get => libObjects;
            set
            {
                libObjects = value;
                OnPropertyChanged(nameof(LibraryObjects));
            }
        }
        public bool FrontPage
        {
            get => frontPage;
            set
            {
                frontPage = value;
                OnPropertyChanged(nameof(FrontPage));
                OnPropertyChanged(nameof(SearchPage));
            }
        }

        public bool SearchPage => !FrontPage;


        public ICommand SearchCommand => new RelayCommand(_ => Search());

        private void Search()
        {
            if (FrontPage)
                FrontPage = false;
            else
                FrontPage = true;
        }

        public LibraryMainViewModel()
        {
            if (base.IsInDesignMode) return;
            _repo = new GeneralRepository();
            GetCategories();

            FrontPage = true;
            LibraryObjects = new ObservableCollection<LibraryObject>();
            FpLibraryObjects = new ObservableCollection<LibraryObject>();
            GetLibraryObjects();
            GetFpLibraryObjects();
            BookButton = new RelayCommand(x => BookButtonClick());

        }

        public LibraryObject SelectedLibraryObject
        {
            set => NavService.HostScreen.SubViewModel = (ISubViewModel)NavService.CreateNewInstance(new LibraryObjectInfoViewModel(value));
        }
        public void BookButtonClick()
        {
            MessageBox.Show("test");
        }
        /// <summary>
        /// hämtar alla library objects ifrån databasen
        /// </summary>
        private async void GetLibraryObjects()
        {
            LibraryObjects = new ObservableCollection<LibraryObject>(await _repo.GetAllAsync<LibraryObject>());
        }

















        private async void GetLibraryObjects(int id)
        {
            FrontPage = false;
            LibraryObjects = new ObservableCollection<LibraryObject>((await _repo.GetAllAsync<LibraryObject>()).Where(o => o.Category == id));
        }

        public ObservableCollection<Category> Categories { get; private set; }

        public Category SelectedCategory { set => GetLibraryObjects(value.ID); }

        private async void GetCategories()
        {
            try
            {
                Categories = new ObservableCollection<Category>(await _repo.GetAllAsync<Category>());
                if (Categories?.Count > 0) Categories[0].Name = "Böcker";
            }
            catch
            {
                Categories = new ObservableCollection<Category>() { new Category() { ID = 1, Name = "Saknas" } };
            }
        }
        private async void GetFpLibraryObjects()
        {
            var list = (await _repo.GetAllAsync<LibraryObject>()).Where(x => x.Category == 1).ToList();
            FpLibraryObjects = new ObservableCollection<LibraryObject>(list.GetRange(0,4));
        }
      
    }
}
