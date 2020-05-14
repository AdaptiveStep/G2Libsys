using G2Libsys.Commands;
using G2Libsys.Data.Repository;
using G2Libsys.Library.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace G2Libsys.ViewModels
{
    public class LibraryMainViewModel : BaseViewModel, IViewModel
    {
        public ICommand BookButton { get; private set; }
        private readonly IRepository _repo;

        private bool frontPage;
        private ObservableCollection<LibraryObject> searchObjects;
        public ObservableCollection<LibraryObject> SearchObjects
        {
            get => searchObjects;
            set
            {
                searchObjects = value;
                OnPropertyChanged(nameof(SearchObjects));
            }
        }
        private ObservableCollection<LibraryObject> libObjects;

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


        public ICommand SearchCommand { get; private set; }

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
            SearchObjects = new ObservableCollection<LibraryObject>();

            SearchCommand = new RelayCommand(x=>GetSearchObjects());

            BookButton = new RelayCommand(x => BookButtonClick());

        }
        private LibraryObject selectedLibraryObject;
        public LibraryObject SelectedLibraryObject
        {
            get => selectedLibraryObject;
            set
            {
                selectedLibraryObject = value;
                
                OnPropertyChanged(nameof(SelectedLibraryObject));
            }
        }
        public void BookButtonClick()
        {
            MessageBox.Show("test");
        }
        /// <summary>
        /// hämtar alla library objects ifrån databasen
        /// </summary>
        //private async void GetLibraryObjects(int id)
        //{
        //    LibraryObjects = new ObservableCollection<LibraryObject>((await _repo.GetAllAsync<LibraryObject>()).Where(o => o.Category == id));
        //}













        private string searchtext;
        public string SearchText
        {
            get => searchtext;
            set
            {
                searchtext = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }


        private async void GetSearchObjects()
        {
            Search();
            
            //SearchObjects.Clear();
            SearchObjects = new ObservableCollection<LibraryObject>((await _repo.GetRangeAsync<LibraryObject>(SearchText)).Where(o => o.Category == SelectedCategory.ID));
        }

        public ObservableCollection<Category> Categories { get; private set; }

        private Category selectedCatagory;
        public Category SelectedCategory
        {
            get => selectedCatagory;
            set
            {
                
                selectedCatagory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }
    
    private async void GetCategories()
        {
            
                Categories = new ObservableCollection<Category>(await _repo.GetAllAsync<Category>());
           
        }
    }
}
