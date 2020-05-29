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
        private LibraryObject newLibraryObject;
        private ObservableCollection<LibraryObject> libraryObjects;
        private Category selectedCategory;
        private LibraryObject selectedItem;
        private string searchString;
        private bool disabledLibraryObjects;

        #endregion

        #region Properties

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
        private Predicate<object> CanExecute => _ => SelectedItem != null;

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
        public ICommand EditItem => editItem ??= new RelayCommand(_ => EditLibraryObject(), CanExecute);

        /// <summary>
        /// Delete LibraryObject
        /// </summary>
        public ICommand DeleteItem => deleteItem ??= new RelayCommand(_ => DeleteLibraryObjectAsync(), CanExecute);

        /// <summary>
        /// Command for downloading a csv file with deleted users
        /// </summary>
        public ICommand DownloadLibLogCommand => new RelayCommand(SaveDialogBoxAsync);

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

        private async Task Initialize()
        {
            DisabledLibraryObjects = true;
            Categories     = new ObservableCollection<Category>();
            LibraryObjects = new ObservableCollection<LibraryObject>();
            Task<IEnumerable<Category>>  categoryList = _repo.GetAllAsync<Category>();
            Task<IEnumerable<LibraryObject>> itemList = _repo.GetAllAsync<LibraryObject>();
            await Task.WhenAll(categoryList, itemList);
            Categories.Add(new Category() { ID = 0, Name = "Visa Alla" });

            categoryList.Result.ToList().ForEach(c => Categories    .Add(c));
            itemList    .Result.ToList().ForEach(i => LibraryObjects.Add(i));

            OnPropertyChanged(nameof(Categories));
        }
        /// <summary>
        /// Hämtar en lista med LibraryObject
        /// </summary>
        /// <returns></returns>
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
        /// <returns></returns>
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
            //En dialogruta som tar emot en tuple med bool och string (anledning för att ta bort objekt)

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
                await _repo.UpdateAsync<LibraryObject>(SelectedItem).ConfigureAwait(false);
                
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
        /// A dialogbox that lets you browse where you want to save an object
        /// </summary>
        /// <param name="param"></param>
        public async void SaveDialogBoxAsync(object param = null) //används till att spara .csv fil 
        {
            LibraryObjectsView libraryObjectsView = new LibraryObjectsView() { Disabled = DisabledLibraryObjects, DateAdded = DateTime.Now, LastEdited = DateTime.Now };
            var libObjects = new List<LibraryObjectsView>(await _repo.GetRangeAsync<LibraryObjectsView>(libraryObjectsView));
            var propList = libraryObjectsView.GetType().GetProperties().Select(p => p.Name).ToList();

            // Inställningar för save file dialog box
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "LibsysUserLog"; // Default file name
            dlg.DefaultExt = ".csv"; // Default file extension
            dlg.Filter = "Excel documents (.csv)|*.csv"; // Filter files by extension

            // Visa save file dialog box true if user input string
            bool? saveresult = dlg.ShowDialog();



            // Process save file dialog box results
            if (saveresult == true)
            {
                // Create a FileStream with mode CreateNew  
                FileStream stream = new FileStream(dlg.FileName, FileMode.OpenOrCreate);
                // Create a StreamWriter from FileStream  
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    writer.WriteLine(string.Join("," , propList));
                    foreach (var item in libObjects)
                    {
                        writer.WriteLine($"{ item.ID },{ item.ISBN },{ item.PurchasePrice },{item.Title},{item.Category},{item.DateAdded},{item.Disabled},{item.LastEdited}");
                    }
                }


           
            }
        }

        #endregion
    }
}
