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
        /// Navigation service
        /// </summary>
        protected readonly INavigationService _navigationService;

        /// <summary>
        /// Dialog service
        /// </summary>
        protected readonly IDialogService _dialog;

        /// <summary>
        /// Provides services for managing the queue of work on the current thread
        /// </summary>
        protected Dispatcher dispatcher;

        /// <summary>
        /// Check if applciation is in design mode
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


        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        protected BaseViewModel()
        {
            if (IsInDesignMode) return;

            _navigationService = IoC.ServiceProvider.GetService<INavigationService>();
            _dialog            = IoC.ServiceProvider.GetService<IDialogService>()
                               ?? new DialogService();

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
                    _navigationService.HostScreen.CurrentViewModel = _navigationService.GetViewModel(viewModel);
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
                    _navigationService.HostScreen.SubViewModel = (ISubViewModel)_navigationService.GetViewModel(viewModel);
                }
                catch { Debug.WriteLine("Couldn't find " + vm.ToString()); }
            });
        }
        #endregion
    }
}
