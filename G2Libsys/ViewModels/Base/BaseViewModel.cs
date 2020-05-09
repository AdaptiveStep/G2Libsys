using System;
using System.ComponentModel;
using System.Diagnostics;
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

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseViewModel()
        {
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
