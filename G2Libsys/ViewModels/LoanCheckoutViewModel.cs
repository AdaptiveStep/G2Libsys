using G2Libsys.Commands;
using G2Libsys.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace G2Libsys.ViewModels
{
    
   public class LoanCheckoutViewModel : BaseViewModel, IViewModel
    {

        public ObservableCollection<LibraryObject> Cart { get; set; }
        public ICommand DeleteItem => new RelayCommand(_ => RemoveItem());

     

        private void RemoveItem()
        {
            Cart.Remove(SelectedItem);
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
        private LibraryObject selectedItem;


        public LoanCheckoutViewModel()
        {
            if (IsInDesignMode)
                return;

            Cart = new ObservableCollection<LibraryObject>()
            {
                new LibraryObject() { Title = "123", Description = "Test", imagesrc = "https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1320428955l/2776527.jpg"},
                new LibraryObject() { Title = "123", Description = "Test", imagesrc = "https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1320428955l/2776527.jpg" },
                new LibraryObject() { Title = "123", Description = "Test", imagesrc = "https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1320428955l/2776527.jpg"},
                new LibraryObject() { Title = "123", Description = "Test", imagesrc = "https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1320428955l/2776527.jpg"},
                new LibraryObject() { Title = "123", Description = "Test", imagesrc = "https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1320428955l/2776527.jpg"},
                new LibraryObject() { Title = "123", Description = "Test", imagesrc = "https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1320428955l/2776527.jpg"}
             };


        }
    }
}
