using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
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

        #region Commands
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
                    NavService.GoToAndReset((BaseViewModel)Activator.CreateInstance(vm));
                }
                catch { Debug.WriteLine("Couldn't find " + vm.ToString()); }
            });
        }
        #endregion
    }
}
