namespace G2Libsys.ViewModels
{
	#region NameSpaces
	using G2Libsys.Commands;
	using G2Libsys.Data.Repository;
	using G2Libsys.Library;
	using G2Libsys.Services;
	using System;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Runtime.Serialization;
    using System.Threading.Tasks;
    using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows.Media.TextFormatting;
	#endregion

	public class LibraryMainViewModel : BaseViewModel, IViewModel
	{
		#region Fields
		private readonly IRepository _repo;
		private LibraryObject searchObject;
		private LibraryObject selectedLibraryObject;
		private ObservableCollection<LibraryObject> libObjects;
		private Category selectedCatagory;
		private string searchtext;
		private bool frontPage;
		private bool basicSearch;
		#endregion

		#region Properties

		/// <summary>
		/// We create this object when doing an advanced search query. Its instanse-variables are used for the search method.
		/// </summary>
		private AdvSearchParams advSearchObjectWithParams;

		public AdvSearchParams AdvSearchObjectWithParams
		{
			get => advSearchObjectWithParams;
			set
			{
				advSearchObjectWithParams = value;
				OnPropertyChanged(nameof(advSearchObjectWithParams));
			}
		}

        public LibraryObject SearchObject
		{
			get => searchObject;
			set
            {
				searchObject = value;
				OnPropertyChanged(nameof(SearchObject));
            }
		}

        /// <summary>
        /// En lista med Categories
        /// </summary>
        public ObservableCollection<Category> Categories { get; private set; }

		/// <summary>
		/// Selected search category
		/// </summary>
		public Category SelectedCategory
		{
			get => selectedCatagory ?? Categories?.FirstOrDefault();
			set
			{
				selectedCatagory = value;
				OnPropertyChanged(nameof(SelectedCategory));
			}
		}

		/// <summary>
		/// Selected navigation category
		/// </summary>
		public Category NavigateCategory
		{
			// Get libraryobject with selected category id
			set => GetLibraryObjects(value.ID);
		}

		/// <summary>
		/// den andra listan med libobjects, används för att binda i blogposterna
		/// </summary>
		public ObservableCollection<LibraryObject> FpLibraryObjects { get; set; }

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

		/// <summary>
		/// Basic search string
		/// </summary>
		public string SearchText
		{
			get => searchtext;
			set
			{
				searchtext = value;
				OnPropertyChanged(nameof(SearchText));
			}
		}

		#endregion

		#region Bools

		/// <summary>
		/// If frontpage enabled
		/// </summary>
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

		/// <summary>
		/// If frontpage disabled
		/// </summary>
		public bool SearchPage => !FrontPage;

		/// <summary>
		/// If Basicsearch enabled
		/// </summary>
        public bool BasicSearch
        {
            get => basicSearch;
            set
			{
				basicSearch = value;
				OnPropertyChanged(nameof(BasicSearch));
				OnPropertyChanged(nameof(AdvSearch));
			}
        }

		/// <summary>
		/// If Advancedsearch enabled
		/// </summary>
		public bool AdvSearch => !BasicSearch;

		#endregion

		#region Commands

		public ICommand AdvancedSearchCommand { get; }
		public ICommand AdvClearCommand { get; }
		public ICommand SearchCommand { get; }
		public ICommand EnableAdvancedSearch { get; }

		#endregion

		#region Constructor

		public LibraryMainViewModel()
		{
			if (base.IsInDesignMode) return;

			_repo = new GeneralRepository();
            LibraryObjects = new ObservableCollection<LibraryObject>();
			Categories = new ObservableCollection<Category>();

			FrontPage = true;
			BasicSearch = true;

			GetCategories();
			GetFpLibraryObjects();

			SearchCommand = new RelayCommand(_ => GetSearchObjects());

			AdvSearchObjectWithParams = new AdvSearchParams();
			AdvancedSearchCommand = new RelayCommand(_ => GetAdvancedSearchObjects());
			AdvClearCommand = new RelayCommand(_ => ClearAdvSearch());
			EnableAdvancedSearch = new RelayCommand(_ => BasicSearch = !BasicSearch);

            // Oanvänd kod?
            //SearchObjects = new ObservableCollection<LibraryObject>();
            //GetUser();
            //LoanCart = new ObservableCollection<Loan>();
            //AddLoanButton = new RelayCommand(_=> AddToCart());
            //BookButton = new RelayCommand(_ => ConfirmLoan());
        }

		#endregion

		#region Private methods

        /// <summary>
        /// Get library objects with the matching category id
        /// </summary>
        private async void GetLibraryObjects(int id)
        {
            FrontPage = false;
            LibraryObjects = new ObservableCollection<LibraryObject>(await _repo.GetAllAsync<LibraryObject>(id));
        }

		/// <summary>
		/// Get search results
		/// </summary>
		private async void GetSearchObjects()
		{
			FrontPage = false;
			LibraryObjects = new ObservableCollection<LibraryObject>((await _repo.GetRangeAsync<LibraryObject>(SearchText)).Where(o => o.Category == SelectedCategory.ID));
		}

		/// <summary>
		/// Uses the filtered search stored procedure to get libraryobjects.
		/// The stored procedure smart_filter_Search takes all the parameters the Libraryobject instance has.
		/// </summary>
		private async void GetAdvancedSearchObjects()
		{
			FrontPage = false;

			LibraryObjects = new ObservableCollection<LibraryObject>(await _repo.AdvancedSearchAsync(AdvSearchObjectWithParams));
		}


		private void ClearAdvSearch()
		{
			//Todo: Do a simple for-loop over all properties instead.
			//advSearchObjectWithParams.Publisher = string.Empty;
			//advSearchObjectWithParams.Dewey = null;
			//advSearchObjectWithParams.Author = string.Empty;
			//advSearchObjectWithParams.AddedBy = null;
			//advSearchObjectWithParams.Description = string.Empty;
			//advSearchObjectWithParams.Category = null;
			//advSearchObjectWithParams.DateAdded = DateTime.Now;
			//advSearchObjectWithParams.ISBN = null;
			//advSearchObjectWithParams.Title = string.Empty;

			AdvSearchObjectWithParams = new AdvSearchParams();
		}

		/// <summary>
		/// Get category navigation
		/// </summary>
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

		/// <summary>
		/// Get frontpage content
		/// </summary>
		private async void GetFpLibraryObjects()
		{
			var list = (await _repo.GetAllAsync<LibraryObject>(1)).ToList();
			FpLibraryObjects = new ObservableCollection<LibraryObject>(list.GetRange(0,2));
		}

        #endregion

        // Oanvänd kod?

   //     private async void GetLibraryObjects()
   //     {
			//FrontPage = false;
   //         LibraryObjects = new ObservableCollection<LibraryObject>(await _repo.GetAllAsync<LibraryObject>());
   //     }

        //private ObservableCollection<LibraryObject> searchObjects;

        //public ObservableCollection<LibraryObject> SearchObjects
        //{
        //	get => searchObjects;
        //	set
        //	{
        //		searchObjects = value;
        //		OnPropertyChanged(nameof(SearchObjects));
        //	}
        //}

        //private ObservableCollection<Loan> loanCart;
        //private User currentUser;
        //private Card currentUserCard;

        //public User CurrentUser
        //{
        //	get => currentUser;
        //	set
        //	{
        //		currentUser = value;
        //		OnPropertyChanged(nameof(CurrentUser));
        //	}
        //}

        //public Card CurrentUserCard
        //{
        //	get => currentUserCard;
        //	set
        //	{
        //		currentUserCard = value;
        //		OnPropertyChanged(nameof(currentUserCard));
        //	}
        //}

        //private void GetUser()
        //{

        //	if (_navigationService.HostScreen.CurrentUser != null)
        //	{
        //		CurrentUser = _navigationService.HostScreen.CurrentUser;
        //		//CurrentUserCard = await _repo.GetByIdAsync<Card>(CurrentUser.ID);
        //	}
        //}

        //private void AddToCart()
        //{
        //	if (_navigationService.HostScreen.CurrentUser.LoggedIn == true)
        //	{
        //		LoanCart.Add(new Loan() { LibraryObject = SelectedLibraryObject, Card = CurrentUserCard, LoanDate = DateTime.Now });
        //		_dialog.Alert("", "Tillagd i varukorgen");
        //	}
        //	else { _dialog.Alert("", "Vänligen logga in för att låna"); }
        //}

        //private async void ConfirmLoan()
        //{
        //	foreach (Loan a in LoanCart)
        //	{
        //		await _repo.AddAsync(a);
        //	}
        //	_dialog.Alert("", "Dina lån är nu skapade");
        //	LoanCart.Clear();
        //}

        //public ObservableCollection<Loan> LoanCart
        //{
        //	get => loanCart;
        //	set
        //	{
        //		loanCart = value;
        //		OnPropertyChanged(nameof(LoanCart));
        //	}
        //}
    }
}

