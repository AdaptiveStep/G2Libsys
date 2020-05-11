using G2Libsys.Dialogs;
using G2Libsys.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace G2Libsys.Services
{
    public class DialogService : IDialogService
    {
        public void Alert(string title = null, string msg = null)
        {
            var viewModel = new AlertDialogViewModel(title, msg);
            ShowDialog(viewModel);
        }

        public bool? Confirm(string title = null, string msg = null)
        {
            var viewModel = new BaseDialogViewModel<bool?>();
            viewModel.DialogResult = ShowDialog(viewModel);
            return viewModel.DialogResult;
        }

        private bool? ShowDialog<VM>(VM viewModel) where VM : IDialogRequestClose
        {
            IDialog dialog = new DialogWindow() { DataContext = viewModel };

            EventHandler<DialogCloseRequestedEventArgs> handler = null;

            handler = (sender, e) =>
            {
                viewModel.CloseRequested -= handler;

                if (e.DialogResult.HasValue)
                {
                    dialog.DialogResult = e.DialogResult;
                }
                else
                {
                    dialog.Close();
                }
            };

            viewModel.CloseRequested += handler;

            dialog.DataContext = viewModel;

            return dialog.ShowDialog();
        }
    }
}
