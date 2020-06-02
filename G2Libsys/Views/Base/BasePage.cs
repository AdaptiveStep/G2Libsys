namespace G2Libsys.Views
{
    using G2Libsys.Services;
    using G2Libsys.ViewModels;
    using System.Windows.Controls;
    using Microsoft.Extensions.DependencyInjection;
    using System.ComponentModel;
    using System.Windows;

    /// <summary>
    /// Custom UserControl that sets datacontext to the typeof <typeparamref name="VM"/>
    /// </summary>
    /// <typeparam name="VM">The datacontext ViewModel</typeparam>
    public class BasePage<VM> : UserControl where VM : IViewModel, new()
    {
        private object viewModel;

        /// <summary>
        /// ViewModel for setting DataContext
        /// </summary>
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

        /// <summary>
        /// Check if applciation is in design mode
        /// </summary>
        private bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(new DependencyObject());

        /// <summary>
        /// Default constructor
        /// </summary>
        public BasePage()
        {
            if (IsInDesignMode) return;

            // Get the navigation service from the serviceprovider
            var navigationService = IoC.ServiceProvider.GetService<INavigationService>();

            // Get the type of VM
            var b = typeof(VM);

            // Locate the viewmodel of type VM in navigation service or create a new if result is null
            ViewModel = navigationService.Locate(b) ?? new VM();

            // Set this UserControls DataContext to ViewModel
            this.DataContext = ViewModel;
        }
    }
}
