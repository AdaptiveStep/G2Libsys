using G2Libsys.Commands;
using System.Windows.Input;

namespace G2Libsys.Dialogs
{
    public class RemoveItemDialogViewModel : BaseDialogViewModel<(bool isSuccess, string msg)>
    {
        public ICommand RemoveItemCommand => new RelayCommand(ExecuteDialog);

        private string message;

        public string ReturnMessage
        {
            get { return message; }
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

        public RemoveItemDialogViewModel(string title = null) : base(title)
        {
        }
    }
}
