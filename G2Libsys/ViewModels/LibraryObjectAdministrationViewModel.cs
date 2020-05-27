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
        private string filePath;

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
        /// Filepath for reports
        /// </summary>
        public string FilePath
        {
            get => filePath;
            set
            {
                filePath = value;
                OnPropertyChanged(nameof(FilePath));
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
        public ICommand DeleteItem => deleteItem ??= new RelayCommand(_ => DeleteLibraryObject(), CanExecute);

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
        private void DeleteLibraryObject()
        {
            if (selectedItem == null) return;
            //bool result = _dialog.Confirm("Ta bort", $"\"{SelectedItem.Title.LimitLength(20)}\"\nGodkänn borttagning.");
            var myVM = new RemoveItemDialogViewModel("Ta bort biblioteksobjekt");
            //En dialogruta som tar emot en tuple med bool och string (anledning för att ta bort objekt)
            var dialogresult = _dialog.Show(myVM);

            if (!dialogresult.isSuccess) return;

            AdminAction adminAction = new AdminAction()
            {
                Comment = $"ObjektID: {selectedItem.ID} Titel: {selectedItem.Title}  Anledning: { dialogresult.msg} ",
                Actiondate = DateTime.Now,
                ActionType = 2
            };
            //Filens sökväg
            //    FilePath = @"C:\Rapporter\Borttagna böcker.csv";


            //    //Skickar med en anledning, ID, Titel och Author och skriver till .csv fil
            //    string createText = myVM.ReturnMessage;
            //    var objectID = selectedItem.ID;
            //    string objectName = selectedItem.Title;
            //    string objectAuthor = selectedItem.Author;

            //    try
            //    {
            //        if (!File.Exists(filePath))
            //        {
            //            File.WriteAllText(filePath, "ID: ");
            //            File.AppendAllText(filePath, objectID.ToString() + Environment.NewLine);

            //            File.AppendAllText(filePath, "Namn: ");
            //            File.AppendAllText(filePath, objectName + Environment.NewLine);

            //            File.AppendAllText(filePath, "Författare: ");
            //            File.AppendAllText(filePath, objectAuthor + Environment.NewLine);

            //            File.AppendAllText(filePath, "Anledning: ");
            //            File.AppendAllText(filePath, createText + Environment.NewLine + Environment.NewLine);
            //        }

            //        else
            //        {
            //            File.AppendAllText(filePath, "ID: ");
            //            File.AppendAllText(filePath, objectID.ToString() + Environment.NewLine);

            //            File.AppendAllText(filePath, "Namn: ");
            //            File.AppendAllText(filePath, objectName + Environment.NewLine);

            //            File.AppendAllText(filePath, "Författare: ");
            //            File.AppendAllText(filePath, objectAuthor + Environment.NewLine);

            //            File.AppendAllText(filePath, "Anledning: ");
            //            File.AppendAllText(filePath, createText + Environment.NewLine + Environment.NewLine);

            //        }

            //    }
            //    catch (Exception ex)
            //{
            //    _dialog.Alert("Fel", "Stäng Excelfilen");
            //    Debug.WriteLine(ex.Message);
            //    return;
            //}

            try
            {
                _repo.RemoveAsync<LibraryObject>(SelectedItem.ID).ConfigureAwait(false);
                LibraryObjects.Remove(SelectedItem);
            }
            catch (Exception ex)
            {
                _dialog.Alert("Error", "Borttagning misslyckades, försök igen");
                Debug.WriteLine(ex.Message);
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
