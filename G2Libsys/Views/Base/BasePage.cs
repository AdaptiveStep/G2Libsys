using G2Libsys.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace G2Libsys.Views
{
    public class BasePage<VM> : Page where VM : BaseViewModel, new()
    {
        private VM viewModel;

        public VM ViewModel 
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

        public BasePage(VM viewModel = null)
        {
            ViewModel = viewModel ?? new VM();
            this.DataContext = ViewModel;
        }
    }
}
