using G2Libsys.Commands;
using G2Libsys.Controls;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Windows.Input;

namespace G2Libsys.Dialogs
{
    public class RemoveItemDialogViewModel : BaseDialogViewModel<(bool isSuccess, string msg)>
    {
        public ICommand RemoveItemCommand => new RelayCommand(ExecuteRunDialog);

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

        private async void ExecuteRunDialog(object o)
        {
            //var view = new SampleDialog
            //{
            //    DataContext = new SampleDialogViewModel()
            //};

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }


        //public void Result(object param = null)
        //{


        //    //base.DialogResult = (true, ReturnMessage);
        //    //base.OKCommand.Execute(param);
        //}


        public RemoveItemDialogViewModel(string title = null) : base(title)
        {

        }
    }
}
