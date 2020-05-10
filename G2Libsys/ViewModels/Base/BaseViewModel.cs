using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using G2Libsys.Commands;
using G2Libsys.Services;

namespace G2Libsys.ViewModels
{
    public abstract class BaseViewModel : BaseNotificationClass
    {
        #region Commands
        /// <summary>
        /// Command for navigating to another ViewModel
        /// </summary>
        public ICommand NavigateToVM { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public ICommand OpenSubVM { get; protected set; }
        #endregion

        /// <summary>
        /// Check if in design mode
        /// </summary>
        public bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(new DependencyObject());

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseViewModel()
        {
            if (IsInDesignMode) return;

            // Navigate to vm where vm = ViewModel
            NavigateToVM = new RelayCommand<Type>(vm =>
            {
                try
                {
                    // Create new ViewModel
                    NavService.HostScreen.CurrentViewModel = NavService.GetViewModel((IViewModel)Activator.CreateInstance(vm));
                }
                catch { Debug.WriteLine("Couldn't find " + vm.ToString()); }
            });
        }
        #endregion
    }
}
