#region NameSpaces
using G2Libsys.Commands;
using G2Libsys.Data.Repository;
using G2Libsys.Library;
using G2Libsys.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;
#endregion
namespace G2Libsys.ViewModels
{
    public class LibraryMainViewModel : BaseViewModel, IViewModel
    {
        public ICommand SearchCommand { get; private set; }
        public ICommand BookButton { get; private set; }
        public ICommand AddLoanButton { get; private set; }
        private readonly IRepository _repo;
        private ObservableCollection<Loan> loanCart;
        private bool frontPage;
        private ObservableCollection<LibraryObject> searchObjects;
        private LibraryObject selectedLibraryObject;
        private User currentUser;
        private Card currentUserCard;

        public Card CurrentUserCard
        {
            get => currentUserCard;
            set
            {
                currentUserCard = value;
                OnPropertyChanged(nameof(currentUserCard));
            }
        }
        public User CurrentUser
        {
            get => currentUser;
            set
            {
                currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }
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
        public Category NavigateCategory
        {
            set
            {
                GetLibraryObjects(value.ID);
            }
        }
        public ObservableCollection<Loan> LoanCart
        {
            get => loanCart;
            set
            {
                loanCart = value;
                OnPropertyChanged(nameof(LoanCart));
            }
        }

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
            GetUser();
            FrontPage = true;
            LibraryObjects = new ObservableCollection<LibraryObject>();
            FpLibraryObjects = new ObservableCollection<LibraryObject>();
            GetLibraryObjects();
            GetFpLibraryObjects();
            LoanCart = new ObservableCollection<Loan>();
            SearchObjects = new ObservableCollection<LibraryObject>();
            BookButton = new RelayCommand(_ => ConfirmLoan());
            SearchCommand = new RelayCommand(_=>GetSearchObjects());
            AddLoanButton = new RelayCommand(_=> AddToCart());


        }
        /// <summary>
        /// Vid klick av library object, gå till ny vy av objektet
        /// </summary>
        public LibraryObject SelectedLibraryObject
        {
            get => selectedLibraryObject;
            set
            {
                selectedLibraryObject = value;
                OnPropertyChanged(nameof(SelectedLibraryObject));
                if (value != null)
                {
                    _navigationService.HostScreen.SubViewModel = (ISubViewModel)_navigationService.CreateNewInstance(new LibraryObjectInfoViewModel(value));
                }
            }
        }



        #endregion

        #region Private methods

        private async void GetUser()
        {
            
            if (_navigationService.HostScreen.CurrentUser != null)
            {
                CurrentUser = _navigationService.HostScreen.CurrentUser;
                CurrentUserCard = await _repo.GetByIdAsync<Card>(CurrentUser.ID);
            }
        }
        public void AddToCart()
        {
            if (_navigationService.HostScreen.CurrentUser.LoggedIn == true)
            {
                LoanCart.Add(new Loan() { LibraryObject = SelectedLibraryObject, Card = CurrentUserCard, LoanDate = DateTime.Now });
                _dialog.Alert("", "Tillagd i varukorgen");
            }
            else { _dialog.Alert("", "Vänligen logga in för att låna"); }
        }

        public async void ConfirmLoan()
        {
            foreach (Loan a in LoanCart)
            {
                await _repo.AddAsync(a);
            }
            _dialog.Alert("", "Dina lån är nu skapade");
            LoanCart.Clear();
        }
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

