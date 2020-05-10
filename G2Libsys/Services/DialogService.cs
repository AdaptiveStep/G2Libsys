using G2Libsys.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace G2Libsys.Services
{
    public class DialogService : IDialogService
    {
        public string Alert(string title = null, string msg = null)
        {
            var viewModel = new AlertDialogViewModel(title, msg);
            IDialogWindow window = new DialogWindow() { DataContext = viewModel };
            window.ShowDialog();
            return viewModel.DialogResult;
        }

        public T Confirm<T>(string title = null, string msg = null)
        {
            var viewModel = new BaseDialogViewModel<T>();
            IDialogWindow window = new DialogWindow();
            window.DataContext = viewModel;
            window.ShowDialog();
            return viewModel.DialogResult;
        }
    }
}
