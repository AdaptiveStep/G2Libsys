using G2Libsys.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace G2Libsys.Views
{
    public class BasePage<T> : Page where T : BaseViewModel, new()
    {
        private T viewModel;

        public T ViewModel 
        { 
            get => viewModel;
            set 
            { 
                if (viewModel != value)
                {
                    viewModel = value;
                }
            } 
        }

        public BasePage()
        {
            this.ViewModel = new T();
            this.DataContext = ViewModel;
        }
    }
}
