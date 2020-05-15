using G2Libsys.Services;
using G2Libsys.ViewModels;
using System.Windows.Controls;

namespace G2Libsys.Views
{
    public class BasePage<VM> : UserControl where VM : IViewModel, new()
    {
        private object viewModel;

        public object ViewModel 
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
            var navService = new NavigationService();
            var b = typeof(VM);
            ViewModel = navService.Locate(b) ?? new VM();
            this.DataContext = ViewModel;
        }
    }
}
