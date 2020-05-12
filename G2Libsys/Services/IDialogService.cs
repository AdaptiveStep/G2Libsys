using G2Libsys.Dialogs;

namespace G2Libsys.Services
{
    public interface IDialogService
    {
        /// <summary>
        /// Show alert message
        /// </summary>
        public void Alert(string title, string message);

        /// <summary>
        /// Return Confirmation bool? result
        /// </summary>
        public bool Confirm(string title, string message);

        /// <summary>
        /// Return generic result
        /// </summary>
        public T Show<T>(BaseDialogViewModel<T> viewModel);
    }
}
