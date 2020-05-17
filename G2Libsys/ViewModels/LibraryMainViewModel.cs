#region NameSpaces
using G2Libsys.Commands;
using G2Libsys.Data.Repository;
using G2Libsys.Library;
using G2Libsys.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
#endregion
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
        private ObservableCollection<LibraryObject> fpLibObjects;
        private ObservableCollection<LibraryObject> libObjects;


        #region Public methods
        /// <summary>
        /// En lista med Categories
        /// </summary>
        public ObservableCollection<Category> Categories { get; private set; }
        public Category NavigateCategory { set => GetLibraryObjects(value.ID); }


        /// <summary>
        /// den andra listan med libobjects, används för att binda i blogposterna
        /// </summary>
        public ObservableCollection<LibraryObject> FpLibraryObjects
        {
            get => fpLibObjects;
            set { fpLibObjects = value;}
        }

        /// <summary>
        /// The list of LibraryObjects
        /// </summary>
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
            LibraryObjects = new ObservableCollection<LibraryObject>();
            FpLibraryObjects = new ObservableCollection<LibraryObject>();
            GetLibraryObjects();
            GetFpLibraryObjects();
            SearchObjects = new ObservableCollection<LibraryObject>();

            SearchCommand = new RelayCommand(x=>GetSearchObjects());


        }
        /// <summary>
        /// Vid klick av library object, gå till ny vy av objektet
        /// </summary>
        public LibraryObject SelectedLibraryObject
        {
            set => NavService.HostScreen.SubViewModel = (ISubViewModel)NavService.CreateNewInstance(new LibraryObjectInfoViewModel(value));
         
        }



        #endregion

        #region Private methods
        /// <summary>
        /// hämtar alla library objects ifrån databasen
        /// </summary>
        private async void GetLibraryObjects(int id)
        {
            FrontPage = false;
            LibraryObjects = new ObservableCollection<LibraryObject>((await _repo.GetAllAsync<LibraryObject>()).Where(o => o.Category == id));
        }
        private async void GetLibraryObjects()
        {
            LibraryObjects = new ObservableCollection<LibraryObject>(await _repo.GetAllAsync<LibraryObject>());
        }



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
            FrontPage = false;
            //LibraryObjects = new ObservableCollection<LibraryObject>((await _repo.GetAllAsync<LibraryObject>()).Where(o => o.Category == id));
            
            //SearchObjects.Clear();
            LibraryObjects = new ObservableCollection<LibraryObject>((await _repo.GetRangeAsync<LibraryObject>(SearchText)).Where(o => o.Category == SelectedCategory.ID));
        }


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
            FpLibraryObjects = new ObservableCollection<LibraryObject>(list.GetRange(0,2));
        }
        
        #endregion
    }
}

