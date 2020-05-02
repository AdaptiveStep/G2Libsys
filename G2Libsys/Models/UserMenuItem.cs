using G2Libsys.ViewModels;
using System;
using System.Collections.Generic;
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

        public UserMenuItem(string title, BaseViewModel vm)
        {
            this.Title = title;
            this.VMType = vm.GetType();
        }
    }
}
