using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using G2Libsys.Commands;

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
        /// Raise property changed event on calling members name
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public BaseViewModel()
        {

        }
    }
}
