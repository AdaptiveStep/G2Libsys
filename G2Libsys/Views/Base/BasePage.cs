namespace G2Libsys.Views
{
    using G2Libsys.Services;
    using G2Libsys.ViewModels;
    using System.Windows.Controls;
    using Microsoft.Extensions.DependencyInjection;
    using System.ComponentModel;
    using System.Windows;

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
        private bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(new DependencyObject());

        public BasePage()
        {
            if (IsInDesignMode) return;

           
        
            var navigationService = IoC.ServiceProvider.GetService<INavigationService>();

            var b = typeof(VM);
            ViewModel = navigationService.Locate(b) ?? new VM();
            this.DataContext = ViewModel;
        }

    }
}
