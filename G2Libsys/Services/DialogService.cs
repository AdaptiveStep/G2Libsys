namespace G2Libsys.Services
{
    using G2Libsys.Dialogs;
    using G2Libsys.Events;
    using G2Libsys.Library.Extensions;
    using System;

    /// <summary>
    /// MVVM Dialog handler
    /// </summary>
    public class DialogService : IDialogService
    {
        /// <summary>
        /// Show alert message
        /// </summary>
        /// <param name="title">Display Title with character limit</param>
        /// <param name="msg">Display message with character limit</param>
        public void Alert(string title = null, string msg = null)
        {
            var viewModel = new AlertDialogViewModel(title.LimitLength(20), msg.LimitLength(80));
            ShowDialog(viewModel);
        }

        /// <summary>
        /// Ask for confirmation
        /// </summary>
        /// <param name="title">Display Title with character limit</param>
        /// <param name="msg">Display message with character limit</param>
        public bool? Confirm(string title = null, string msg = null)
        {
            var viewModel = new ConfirmDialogViewModel(title.LimitLength(20), msg.LimitLength(80));
            return ShowDialog(viewModel);
        }

        /// <summary>
        /// Return generic result
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="viewModel">New dialogviewmodel</param>
        /// <returns><typeparam name="T"/> DialogResult</returns>
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
