namespace G2Libsys.Views
{
    using G2Libsys.Services;
    using G2Libsys.ViewModels;
    using System.Windows.Controls;
    using Microsoft.Extensions.DependencyInjection;

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
            var navigationService = IoC.ServiceProvider.GetService<INavigationService>();

            var b = typeof(VM);
            ViewModel = navigationService.Locate(b) ?? new VM();
            this.DataContext = ViewModel;
        }
    }
}
