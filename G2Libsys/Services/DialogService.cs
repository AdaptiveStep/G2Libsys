namespace G2Libsys.Services
{
    using G2Libsys.Dialogs;
    using G2Libsys.Events;
    using System;

    /// <summary>
    /// MVVM Dialog handler
    /// </summary>
    public class DialogService : IDialogService
    {
        /// <summary>
        /// Show alert message
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        public void Alert(string title = null, string msg = null)
        {
            var viewModel = new AlertDialogViewModel(title, msg);
            ShowDialog(viewModel);
        }

        /// <summary>
        /// Ask for confirmation
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        public bool? Confirm(string title = null, string msg = null)
        {
            var viewModel = new BaseDialogViewModel<bool?>();
            return ShowDialog(viewModel);
        }

        /// <summary>
        /// Return generic result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewModel"></param>
        public T Show<T>(BaseDialogViewModel<T> viewModel)
        {
            ShowDialog(viewModel);
            return viewModel.DialogResult;
        }

        /// <summary>
        /// Method for calling the ShowDialog
        /// </summary>
        /// <typeparam name="VM"><see cref="IDialogRequestClose"/></typeparam>
        /// <param name="viewModel">ViewModel</param>
        private bool? ShowDialog<VM>(VM viewModel) where VM : IDialogRequestClose
        {
            IDialog dialog = new DialogWindow() { DataContext = viewModel };

            viewModel.CloseRequested += new EventHandler<DialogCloseRequestedEventArgs>(
                (sender, e) =>
                {
                    if (e.DialogResult.HasValue)
                    {
                        dialog.DialogResult = e.DialogResult;
                    }
                    else
                    {
                        dialog.Close();
                    }
                });

            dialog.DataContext = viewModel;

            return dialog.ShowDialog();
        }
    }
}
