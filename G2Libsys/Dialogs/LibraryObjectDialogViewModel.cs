﻿namespace G2Libsys.Dialogs
{
    #region Namespaces
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;
    using System.Windows.Input;
    using G2Libsys.Commands;
    using G2Libsys.Library.Models;
    #endregion

    /// <summary>
    /// Dialog viewmodel for handling LibraryObject
    /// </summary>
    public class LibraryObjectDialogViewModel : BaseDialogViewModel<LibraryObject>
    {
        #region Fields
        private LibraryObject libraryObject;
        private Category category;
        #endregion

        #region Properties
        /// <summary>
        /// Current libraryobject
        /// </summary>
        public LibraryObject LibraryObject
        {
            get => libraryObject;
            set
            {
                libraryObject = value;
                OnPropertyChanged(nameof(LibraryObject));
            }
        }

        /// <summary>
        /// Selected category for libraryobject
        /// </summary>
        public Category SelectedCategory
        {
            get => category;
            set
            {
                category = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }

        /// <summary>
        /// List of object categories
        /// </summary>
        public ObservableCollection<Category> Categories { get; }
        #endregion

        #region Commands
        /// <summary>
        /// Return result
        /// </summary>
        public ICommand SaveCommand => new RelayCommand(Result);
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="libraryObject">Object to edit</param>
        /// <param name="category">Category list</param>
        /// <param name="title"></param>
        public LibraryObjectDialogViewModel(LibraryObject libraryObject, List<Category> category, string title = null)
            : base(title)
        {
            this.LibraryObject = libraryObject;
            this.Categories = new ObservableCollection<Category>(category);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set DialogResult to LibraryObject
        /// </summary>
        /// <param name="param"></param>
        private void Result(object param = null)
        {
            if (LibraryObject.DateAdded == default)
            {
                LibraryObject.DateAdded = DateTime.Now;
            }

            LibraryObject.LastEdited = DateTime.Now;

            base.DialogResult = LibraryObject;
            base.OKCommand.Execute(param);
        }
        #endregion
    }
}