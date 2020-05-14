namespace G2Libsys.ViewModels
{
    #region Namespaces
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using G2Libsys.Library;
    using System.Windows.Input;
    using G2Libsys.Commands;
    using G2Libsys.Data.Repository;
    using System.Threading.Tasks;
    using System.Collections.ObjectModel;
    using G2Libsys.Services;
    using G2Libsys.Dialogs;
    using G2Libsys.Library.Extensions;
    using System.Diagnostics;
    #endregion

    /// <summary>
    /// Viewmodel for handling LibraryObjects
    /// </summary>
    public class LibraryObjectAdministrationViewModel : BaseViewModel, IViewModel
    {
        #region Fields
        private readonly IRepository _repo;
        private readonly IDialogService _dialog;
        private LibraryObjectDialogViewModel dialogViewModel;
        private ICommand search;
        private ICommand createItem;
        private ICommand editItem;
        private ICommand deleteItem;
        private ICommand reset;
        private ObservableCollection<LibraryObject> libraryObjects;
        private Category selectedCategory;
        private LibraryObject selectedItem;
        private string searchString;
        #endregion

        #region Properties

        public ObservableCollection<Category> Categories { get; set; }

        public Category SelectedCategory
        {
            get => selectedCategory ?? Categories.First();
            set
            {
                if (selectedCategory != value)
                {
                    selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));

                    dispatcher.InvokeAsync(GetLibraryObjects);
                }
            }
        }

        public ObservableCollection<LibraryObject> LibraryObjects
        {
            get => libraryObjects;
            set
            {
                libraryObjects = value;
                OnPropertyChanged(nameof(libraryObjects));
            }
        }

        public LibraryObject SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public string SearchString
        {
            get => searchString;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    dispatcher.InvokeAsync(GetLibraryObjects);
                }
                
                searchString = value;
                OnPropertyChanged(nameof(SearchString));
            }
        }

        private List<Category> ItemCategories => Categories.ToList().GetRange(1, Categories.Count - 1);

        #endregion

        #region Commands

        /// <summary>
        /// Enable execute if an Item is selected
        /// </summary>
        Predicate<object> CanExecute => _ => SelectedItem != null;

        /// <summary>
        /// Search for LibraryObject
        /// </summary>
        public ICommand Search => search ??= new RelayCommand(async _ => await SearchLibraryObject());

        /// <summary>
        /// Create new LibraryObject
        /// </summary>
        public ICommand CreateItem => createItem ??= new RelayCommand(_ => CreateLibraryObject());

        /// <summary>
        /// Update LibraryObject
        /// </summary>
        public ICommand EditItem => editItem ??= new RelayCommand(_ => EditLibraryObject(), CanExecute);

        /// <summary>
        /// Delete LibraryObject
        /// </summary>
        public ICommand DeleteItem => deleteItem ??= new RelayCommand(_ => DeleteLibraryObject(), CanExecute);

        /// <summary>
        /// Reset lists
        /// </summary>
        public ICommand Reset => reset ??= new RelayCommand(_ => ResetLists());

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LibraryObjectAdministrationViewModel()
        {
            if (base.IsInDesignMode) return;

            _repo = new GeneralRepository();
            _dialog = new DialogService();

            dispatcher.InvokeAsync(Initialize);
        }

        #endregion

        #region Tasks

        private async Task Initialize()
        {
            Categories     = new ObservableCollection<Category>();
            LibraryObjects = new ObservableCollection<LibraryObject>();

            Task<IEnumerable<Category>>  categoryList = _repo.GetAllAsync<Category>();
            Task<IEnumerable<LibraryObject>> itemList = _repo.GetAllAsync<LibraryObject>();

            await Task.WhenAll(categoryList, itemList);

            Categories.Add(new Category() { ID = 0, Name = "Visa Alla" });

            categoryList.Result.ToList().ForEach(c => Categories    .Add(c));
            itemList    .Result.ToList().ForEach(i => LibraryObjects.Add(i));
        }

        private async Task GetLibraryObjects()
        {
            var objects = new List<LibraryObject>();

            (SelectedCategory.ID switch
            {
                0 => (await _repo.GetAllAsync<LibraryObject>()).ToList(),
                _ => (await _repo.GetAllAsync<LibraryObject>()).Where(o => o.Category == SelectedCategory.ID).ToList()
            }).ForEach(u => objects.Add(u));

            LibraryObjects = new ObservableCollection<LibraryObject>(objects);
        }

        private async Task SearchLibraryObject()
        {
            SelectedCategory = Categories.First();

            List<LibraryObject> result;

            try
            {
                result = (await _repo.GetRangeAsync<LibraryObject>(SearchString)).ToList();
                LibraryObjects.Clear();
                result.ForEach(r => LibraryObjects.Add(r));
            }
            catch (Exception ex)
            {
                _dialog.Alert("Error", "Sökning misslyckades försök igen.");
                Debug.WriteLine(ex.Message);
            }
        }

        private void CreateLibraryObject()
        {
            dialogViewModel = new LibraryObjectDialogViewModel(null, ItemCategories, "Skapa ny");

            var result = _dialog.Show(dialogViewModel);
        }

        private void EditLibraryObject()
        {
            dialogViewModel = new LibraryObjectDialogViewModel(SelectedItem, ItemCategories, "Ändra info");

            var result = _dialog.Show(dialogViewModel);
        }

        private void DeleteLibraryObject()
        {
            bool result = _dialog.Confirm("Ta bort", $"\"{SelectedItem.Title.LimitLength(20)}\"\nGodkänn borttagning.");

            if (result)
            {
                _repo.DeleteByIDAsync<LibraryObject>(SelectedItem.ID).ConfigureAwait(false);
                LibraryObjects.Remove(SelectedItem);
            }
        }

        private void ResetLists()
        {
            SearchString = string.Empty;
            dispatcher.InvokeAsync(GetLibraryObjects);
        }

        #endregion
    }
}
