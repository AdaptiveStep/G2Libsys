using G2Libsys.Commands;
using G2Libsys.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace G2Libsys.Dialogs
{
    public class BaseDialogViewModel<T> : IDialogRequestClose
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public T DialogResult { get; set; }

        public ICommand OKCommand => new RelayCommand(p => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true)));
        public ICommand CancelCommand => new RelayCommand(p => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)));

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public BaseDialogViewModel() : this(null, null) { }

        public BaseDialogViewModel(string title = null, string message = null)
        {
            Title = title;
            Message = message;
        }
    }
}
