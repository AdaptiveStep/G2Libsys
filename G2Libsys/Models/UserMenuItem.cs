using G2Libsys.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace G2Libsys.Models
{
    /// <summary>
    /// Item for navigation based on UserType
    /// </summary>
    public class UserMenuItem
    {
        /// <summary>
        /// Header title
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Viewmodel type
        /// </summary>
        public Type VMType { get; private set; }

        /// <summary>
        /// Input the viewmodel and displayname Title, default title is viewmodel name
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="title"></param>
        public UserMenuItem(BaseViewModel vm, string title = null)
        {
            // Set Title to title or viewmodel name
            this.Title = title ?? vm.GetType().Name.Replace("ViewModel", null);

            // Set Type to type of vm
            this.VMType = vm.GetType();
        }
    }
}
