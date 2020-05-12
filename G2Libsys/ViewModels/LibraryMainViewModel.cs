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
            
            GetLibraryObjects();
            BookButton = new RelayCommand(x => BookButtonClick());

        }

        public LibraryObject SelectedLibraryObject
        {
            set => MessageBox.Show(value.ID.ToString());
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
            //LibraryObjects = new ObservableCollection<LibraryObject>(await _repo.GetAllAsync<LibraryObject>());
        }

















        private async void GetLibraryObjects(int id)
        {
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
    }
}
