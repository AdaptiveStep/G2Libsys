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
    using Microsoft.Extensions.DependencyInjection;
    using System.IO;
    using G2Libsys.Library.Models;
    using Microsoft.Win32;
    using System.Text;
    using System.ComponentModel;
    using Microsoft.VisualBasic.CompilerServices;
    #endregion

    /// <summary>
    /// Viewmodel for handling LibraryObjects
    /// </summary>
    public class LibraryObjectAdministrationViewModel : BaseViewModel, IViewModel
    {
        #region Fields
        private readonly IRepository _repo;
        private LibraryObjectDialogViewModel dialogViewModel;
        private ICommand search;
        private ICommand createItem;
        private ICommand editItem;
        private ICommand deleteItem;
        private ICommand reset;
        private ICommand downloadLibLogCommand;
        private LibraryObject newLibraryObject;
        private ObservableCollection<LibraryObject> libraryObjects;
        private Category selectedCategory;
        private LibraryObject selectedItem;
        private string searchString;
        private bool disabledLibraryObjects;
        private List<Category> ItemCategories => Categories.ToList().GetRange(1, Categories.Count - 1);

        #endregion

        #region Properties
        /// <summary>
        /// tooltip for datagrid
        /// </summary>
        public string ToolTip { get; set; }
        public ObservableCollection<Category> Categories { get; set; }

        /// <summary>
        /// Chosen category in combobox
        /// </summary>
        public Category SelectedCategory
        {
            get => selectedCategory ?? Categories?.FirstOrDefault();
            set
            {
                if (selectedCategory != value)
                {
                    selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));

                    ResetLists();
                }
            }
        }

        /// <summary>
        /// Property for disabled Library objects
        /// </summary>
        public bool DisabledLibraryObjects
        {
            get => disabledLibraryObjects;
            set
            {
                disabledLibraryObjects = value;
                OnPropertyChanged(nameof(DisabledLibraryObjects));
            }
        }

        /// <summary>
        /// Gets the ObservableCollection of LibObjects 
        /// </summary>
        public ObservableCollection<LibraryObject> LibraryObjects
        {
            get => libraryObjects;
            set
            {
                libraryObjects = value;
                OnPropertyChanged(nameof(libraryObjects));
            }
        }


        /// <summary>
        /// Selected Item in the datagrid
        /// </summary>
        public LibraryObject SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        /// <summary>
        /// Store new LibraryObject
        /// </summary>
        public LibraryObject NewLibraryObject
        {
            get => newLibraryObject;
            set
            {
                newLibraryObject = value;
                OnPropertyChanged(nameof(NewLibraryObject));
            }
        }
        /// <summary>
        /// string for searching in libraryobjects
        /// </summary>
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
      
       

        #endregion

        #region Commands
    
        /// <summary>
        /// Enable execute if an Item is selected
        /// </summary>
        private Predicate<object> CanRemove => _ => SelectedItem != null && !SelectedItem.Disabled;

        /// <summary>
        /// Search for LibraryObject
        /// </summary>
        public ICommand Search => search ??= new RelayCommand(async _ => await SearchLibraryObject(), _ => !string.IsNullOrWhiteSpace(SearchString));

        /// <summary>
        /// Create new LibraryObject
        /// </summary>
        public ICommand CreateItem => createItem ??= new RelayCommand(_ => CreateLibraryObject());

        /// <summary>
        /// Update LibraryObject
        /// </summary>
        public ICommand EditItem => editItem ??= new RelayCommand(_ => EditLibraryObject(), _ => SelectedItem != null);

        /// <summary>
        /// Delete LibraryObject
        /// </summary>
        public ICommand DeleteItem => deleteItem ??= new RelayCommand(_ => dispatcher.InvokeAsync(DeleteLibraryObjectAsync), CanRemove);

        /// <summary>
        /// Command for downloading a csv file with deleted users
        /// </summary>
        public ICommand DownloadLibLogCommand => downloadLibLogCommand ??= new RelayCommand(async _ => await SaveDialogBoxAsync());

        /// <summary>
        /// Reset lists
        /// </summary>
        public ICommand Reset => reset ??= new RelayCommand(_ => ResetLists());

        /// <summary>
        /// Create LibraryObject
        /// </summary>
        public ICommand CreateLibraryObjectCommand { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LibraryObjectAdministrationViewModel()
        {
            if (base.IsInDesignMode) return;

            _repo = new GeneralRepository();
            CreateLibraryObjectCommand = new RelayCommand(_ => CreateLibraryObject());

            dispatcher.InvokeAsync(Initialize);
        }

        #endregion

        #region Methods
        /// <summary>
        /// set tooltip
        /// </summary>
        private void GetToolTip()
        {
            ToolTip = "\u2022Dubbelklicka för att redigera.\n\u2022Klicka på DELETE knappen för att ta bort";
        }
        /// <summary>
        /// Initial setup
        /// </summary>
        private async Task Initialize()
        {
            // Initiate
            DisabledLibraryObjects = true;
            Categories     = new ObservableCollection<Category>();
            LibraryObjects = new ObservableCollection<LibraryObject>();
            GetToolTip();
            // Call queries
            Task<IEnumerable<Category>>  categoryList = _repo.GetAllAsync<Category>();
            Task<IEnumerable<LibraryObject>> itemList = _repo.GetAllAsync<LibraryObject>();

            // Wait for both queries to complete
            await Task.WhenAll(categoryList, itemList);

            Categories.Add(new Category() { ID = 0, Name = "Visa Alla" });

            categoryList.Result.ToList().ForEach(c => Categories    .Add(c));
            itemList    .Result.ToList().ForEach(i => LibraryObjects.Add(i));

            // Update categories
            OnPropertyChanged(nameof(Categories));
        }

        /// <summary>
        /// Hämtar en lista med LibraryObject
        /// </summary>
        private async Task GetLibraryObjects()
        {
            if (Categories?.Count < 1)
            {
                return;
            }

            var objects = new List<LibraryObject>();

            (SelectedCategory.ID switch
            {
                0 => (await _repo.GetAllAsync<LibraryObject>()).ToList(),
                _ => (await _repo.GetAllAsync<LibraryObject>(SelectedCategory.ID)).ToList()
            }).ForEach(u => objects.Add(u));

            LibraryObjects = new ObservableCollection<LibraryObject>(objects);
        }

        /// <summary>
        /// Hämtar en lista med libraryobject som matchar söksträng
        /// </summary>
        private async Task SearchLibraryObject()
        {
            SelectedCategory = Categories.First();

            var result = new List<LibraryObject>();

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

        /// <summary>
        /// Skapar en ny LibraryObject
        /// </summary>
        private async void CreateLibraryObject()
        {
            dialogViewModel = new LibraryObjectDialogViewModel(new LibraryObject(), ItemCategories, "Lägg till ny");

            LibraryObject newItem = _dialog.Show(dialogViewModel);

            if (newItem == null) return;

            try
            {
                await _repo.AddAsync(newItem);
                ResetLists();
            }
            catch (Exception ex)
            {
                _dialog.Alert("Error", "Ändringarna sparades ej, försök igen");
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Funktion för att ändra i LibraryObject
        /// </summary>
        private async void EditLibraryObject()
        {
            dialogViewModel = new LibraryObjectDialogViewModel(SelectedItem, ItemCategories, "Ändra detaljer");

            LibraryObject editedItem = _dialog.Show(dialogViewModel);

            if (editedItem == null)
            {
                ResetLists();
                return;
            }

            try
            {
                await _repo.UpdateAsync(editedItem);
            }
            catch (Exception ex)
            {
                _dialog.Alert("Error", "Ändringarna sparades ej, försök igen");
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Function used for deleting a library object
        /// </summary>
        private async Task DeleteLibraryObjectAsync()
        {
            if (selectedItem == null) return;

            bool result = _dialog.Confirm("Ta bort", $"\"{SelectedItem.Title.LimitLength(20)}\"\nGodkänn borttagning.");

            if (!result) return;


            //creates an adminaction for the log in database
            var adminAction = new AdminAction()
            {
                Comment = $"ObjektID: {selectedItem.ID} Titel: {selectedItem.Title}",
                Actiondate = DateTime.Now,
                ActionType = 2,
            };

            try
            {
                //sets the selecteditem to disabled
                SelectedItem.Disabled = true;
                await _repo.UpdateAsync(SelectedItem).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _dialog.Alert("Error", "Borttagning misslyckades, försök igen");
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                //updates the list
                ResetLists();
            }
        }

        /// <summary>
        /// A method used to updating lists
        /// </summary>
        private void ResetLists()
        {
            SearchString = string.Empty;
            dispatcher.InvokeAsync(GetLibraryObjects);
        }

        /// <summary>
        /// Export libraryobjects details to csv file
        /// </summary>
        public async Task SaveDialogBoxAsync()
        {
            LibraryObjectsView libraryObjectsView = new LibraryObjectsView() { Disabled = DisabledLibraryObjects };
            var libObjects = new List<LibraryObjectsView>(await _repo.GetRangeAsync<LibraryObjectsView>(libraryObjectsView));

            var _fileService = IoC.ServiceProvider.GetService<IFileService>();

            bool fileCreated = _fileService.CreateFile("LibraryObjectLog");

            if (fileCreated)
            {
                bool success = _fileService.ExportCSV(libObjects);

                if (success)
                {
                    _dialog.Alert("Filen sparad", "");
                }
                else
                {
                    _dialog.Alert("Exportering misslyckades", "Stäng filen om öppen och försök igen.");
                }
            }
        }

        #endregion
    }
}
