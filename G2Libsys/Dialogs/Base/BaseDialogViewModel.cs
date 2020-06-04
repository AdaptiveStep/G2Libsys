using G2Libsys.Commands;
using G2Libsys.Events;
using G2Libsys.Services;
using G2Libsys.ViewModels;
using System;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;

namespace G2Libsys.Dialogs
{
    public class BaseDialogViewModel<T> : BaseNotificationClass, IDialogRequestClose
    {
        protected readonly INavigationService _navigationService;

        public string Title { get; set; }
        public string Message { get; set; }
        public T DialogResult { get; set; }

        public ICommand OKCommand => new RelayCommand(_ => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true)));
        public ICommand CancelCommand => new RelayCommand(_ => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)));

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public BaseDialogViewModel() : this(null, null) { }

        public BaseDialogViewModel(string title = null, string message = null)
        {
            _navigationService = IoC.ServiceProvider.GetService<INavigationService>();

            Title = title;
            Message = message;
        }
    }
}
