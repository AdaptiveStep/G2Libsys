namespace G2Libsys.ViewModels
{
    #region Namespaces
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using G2Libsys.Commands;
    using G2Libsys.Services;
    using Microsoft.Extensions.DependencyInjection;
    #endregion

    public abstract class BaseViewModel : BaseNotificationClass
    {
        #region Fields
        /// <summary>
        /// Provides services for managing the queue of work on the current thread
        /// </summary>
        protected Dispatcher dispatcher;

        /// <summary>
        /// Check if in design mode
        /// </summary>
        protected bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(new DependencyObject());
        #endregion

        #region Commands
        /// <summary>
        /// Set CurrentViewModel
        /// </summary>
        public ICommand NavigateToVM { get; protected set; }

        /// <summary>
        /// Set SubViewModel
        /// </summary>
        public ICommand OpenSubVM { get; protected set; }
        #endregion

        protected INavigationService navigationService;

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        protected BaseViewModel()
        {
            if (IsInDesignMode) return;

            navigationService = IoC.ServiceProvider.GetService<INavigationService>();

            // Set dispatcher
            dispatcher = Application.Current.Dispatcher;

            // Navigate to vm where vm = ViewModel
            NavigateToVM = new RelayCommand<Type>(vm =>
            {
                try
                {
                    // Create viewmodel
                    var viewModel = (IViewModel)Activator.CreateInstance(vm);

                    // Set CurrentViewModel
                    navigationService.HostScreen.CurrentViewModel = navigationService.GetViewModel(viewModel);

                    navigationService.HostScreen.SubViewModel = null;
                }
                catch { Debug.WriteLine("Couldn't find " + vm.ToString()); }
            });

            // Navigate to vm where vm = ViewModel
            OpenSubVM = new RelayCommand<Type>(vm =>
            {
                try
                {
                    // Create viewmodel
                    var viewModel = (ISubViewModel)Activator.CreateInstance(vm);

                    // Set SubViewModel
                    navigationService.HostScreen.SubViewModel = (ISubViewModel)navigationService.GetViewModel(viewModel);
                }
                catch { Debug.WriteLine("Couldn't find " + vm.ToString()); }
            });
        }
        #endregion
    }
}
