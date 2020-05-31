using G2Libsys.Commands;
using System.Windows.Input;

namespace G2Libsys.Dialogs
{
    public class RemoveItemDialogViewModel : BaseDialogViewModel<(bool isSuccess, string msg)>
    {
        public ICommand SaveCommand => new RelayCommand(ExecuteDialog, _ => !string.IsNullOrWhiteSpace(ReturnMessage));

        private string message;

        public string ReturnMessage
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged(nameof(ReturnMessage));
            }
        }

        public void ExecuteDialog(object param = null)
        {
            base.DialogResult = (true, ReturnMessage);
            base.OKCommand.Execute(param);
        }

        public RemoveItemDialogViewModel(string title = null, string message = null) : base(title, message)
        {
        }
    }
}
