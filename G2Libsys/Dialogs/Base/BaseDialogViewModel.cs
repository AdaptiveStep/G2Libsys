using G2Libsys.Commands;
using G2Libsys.Events;
using G2Libsys.ViewModels;
using System;
using System.Windows.Input;

namespace G2Libsys.Dialogs
{
    public class BaseDialogViewModel<T> : BaseNotificationClass, IDialogRequestClose
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public T DialogResult { get; set; }

        public ICommand OKCommand => new RelayCommand(_ => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true)));
        public ICommand CancelCommand => new RelayCommand(_ => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)));

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public BaseDialogViewModel() : this(null, null) { }

        public BaseDialogViewModel(string title = null, string message = null)
        {
            Title = title;
            Message = message;
        }
    }
}
