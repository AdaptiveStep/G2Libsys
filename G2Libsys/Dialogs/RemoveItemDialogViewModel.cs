using G2Libsys.Commands;
using G2Libsys.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Windows.Input;

namespace G2Libsys.Dialogs
{
    public class RemoveItemDialogViewModel : BaseDialogViewModel<(bool isSuccess, string msg)>
    {
        public ICommand RemoveItemCommand => new RelayCommand(Result);

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


        public void Result(object param = null)
        {


            base.DialogResult = (true, ReturnMessage);
            base.OKCommand.Execute(param);
        }


        public RemoveItemDialogViewModel(string title = null) : base(title)
        {

        }
    }
}
