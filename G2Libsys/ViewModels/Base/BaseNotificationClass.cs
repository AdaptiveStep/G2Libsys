using System.ComponentModel;

namespace G2Libsys.ViewModels
{
    public abstract class BaseNotificationClass : INotifyPropertyChanged
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

    }
}
