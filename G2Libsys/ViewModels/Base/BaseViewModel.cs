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

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        protected BaseViewModel()
        {
            if (IsInDesignMode) return;

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
                    NavService.HostScreen.CurrentViewModel = NavService.GetViewModel(viewModel);

                    NavService.HostScreen.SubViewModel = null;
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
                    NavService.HostScreen.SubViewModel = (ISubViewModel)NavService.GetViewModel(viewModel);
                }
                catch { Debug.WriteLine("Couldn't find " + vm.ToString()); }
            });
        }
        #endregion
    }
}
