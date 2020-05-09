using G2Libsys.Library;
using G2Libsys.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace G2Libsys.ViewModels
{
    public interface IHostScreen
    {
        public IViewModel CurrentViewModel { get; set; }

        public User CurrentUser { get; set; }

        public ObservableCollection<UserMenuItem> MenuItems { get; set; }
    }
}
