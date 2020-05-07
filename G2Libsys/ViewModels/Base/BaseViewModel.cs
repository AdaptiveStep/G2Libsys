using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using System.Windows.Navigation;
using G2Libsys.Commands;
using G2Libsys.Services;

namespace G2Libsys.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region PropertyChangedEvent
        /// <summary>
        /// Property changed event handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise property changed event
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Public Commands
        /// <summary>
        /// Command for navigating to another ViewModel
        /// </summary>
        public virtual ICommand NavigateToVM { get; protected set; }
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
                    //MainWindowViewModel.HostScreen.CurrentViewModel = (BaseViewModel)Activator.CreateInstance(vm);

                    Services.NavService.GoToAndReset((BaseViewModel)Activator.CreateInstance(vm));
                }
                catch { throw new Exception("Couldn't find " + vm.ToString()); }
            });
        }
        #endregion
    }
}
