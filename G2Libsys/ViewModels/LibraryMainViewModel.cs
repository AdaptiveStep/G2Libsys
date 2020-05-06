using G2Libsys.Commands;
using G2Libsys.Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace G2Libsys.ViewModels
{
    public class LibraryMainViewModel : BaseViewModel
    {
        private BaseViewModel contentVM;
        private Category selectedCategory;

        public BaseViewModel ContentVM
        {
            get => contentVM;
            set
            {
                contentVM = value;
                OnPropertyChanged(nameof(contentVM));
            }
        }

        public ObservableCollection<Category> Categories { get; set; }

        /// <summary>
        /// Open list of selected category
        /// </summary>
        public Category SelectedCategory
        {
            set => ContentVM = new LibraryObjectViewModel() { CurrentCategory = value };
        }

        public Category SelectedSearchCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                OnPropertyChanged(nameof(SelectedSearchCategory));
            }
        }

        public ICommand SelectContent { get; set; }

        public ICommand SearchCommand { get; set; }


        public LibraryMainViewModel()
        {
            ContentVM = new FrontPageViewModel();

            Categories = new ObservableCollection<Category>()
            {
                new Category() { ID = 1 },
                new Category() { ID = 2 },
                new Category() { ID = 3 }
            };

            SelectedSearchCategory = Categories[2];

            SearchCommand = new RelayCommand<string>(search =>
            {
                ContentVM = new LibraryObjectViewModel();
            });
        }
    }
}
