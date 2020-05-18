//using G2Libsys.Commands;
//using G2Libsys.ViewModels;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Windows.Input;

//namespace G2Libsys.ViewModels
//{
//    public class SearchBarViewModel : BaseViewModel
//    {
//        #region Protected Members
//        /// <summary>
//        /// Senaste sökningen i lista
//        /// </summary>
//        protected string _LastSearchText;

//        /// <summary>
//        /// Sökord för searchcommands
//        /// </summary>
//        protected string _SearchText;
//        protected ObservableCollection<BooksAndSeminarContentViewModel> _Items;

//        #endregion
//        #region public properties

//        public ObservableCollection<BooksAndSeminarContentViewModel> Items
//        {
//            get => _Items;
//            set
//            {
//                //kolla i listan
//                if (_Items == value)
//                    return;
//                //uppdatera value
//                _Items = value;

//                // uppdatera filter i lista och matcha
//                // gör ny lista och populate
//                FilteredItems = new ObservableCollection<BooksAndSeminarContentViewModel>mItems;
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public ObservableCollection<BooksAndSeminarContentViewModel> FilteredItems { get; set; }

//        public string SearchText
//        {
//            get => _SearchText;
//            set
//            {
//                //kolla i listan
//                if (_SearchText == value)
//                    return;
//                //uppdatera value
//                _SearchText = value;

//                //kolla om string är null
//                if (string.IsNullOrEmpty(SearchText))
                
//                    Search();
                
//            }
//        }

        

//        #endregion

//        #region Public commands

//        // search command

//        public ICommand SearchCommand { get; set; }

//        // clear searchbar text
//        public ICommand ClearSearchCommand { get; set; }

//        #endregion

//        #region Constructor
//        public SearchBarViewModel()
//        {
//            //Skapa commands
//            SearchCommand = new RelayCommand(Search);
//            ClearSearchCommand = new RelayCommand(ClearSearch);


//        }

//        #endregion

//        #region Command Methods

//        //söker igenom lista på böcker
//        public void SearchBooks()
//        {
//            // Make sure we don't re-search the same text
//            if ((string.IsNullOrEmpty(_LastSearchText) && string.IsNullOrEmpty(SearchText)) ||
//                string.Equals(_LastSearchText, SearchText))
//                return;

//            // If we have no search text, or no items
//            if (string.IsNullOrEmpty(SearchText) || Items == null || Items.Count <= 0)
//            {
//                // Make filtered list the same
//                FilteredItems = new ObservableCollection<BooksAndSeminarContentViewModel>(Items);

//                // Set last search text
//                _LastSearchText = SearchText;

//                return;
//            }

//            //hitta filtrerade sökträffar 
//            //TODO: Optimera sökning
//            var filteredItems = new ObservableCollection<BooksAndSeminarContentViewModel>(
//                Items.Where(item => item.Books.ToLower().Contains(SearchText)));
//            //set last searchtext 
//            _LastSearchText = SearchText;
//        }
        

//        // söker igenom lista på seminarium
//        public void SearchSeminars()
//        {

//        }

//        // clear söktext
//        public void ClearSearch()
//        {
//            // kolla om det finns söktext
//            if (!string.IsNullOrEmpty(SearchText))
//            //clear text
//            SearchText = string.Empty;
            

//        }

//        #endregion

//    }
//}
