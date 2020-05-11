namespace G2Libsys.ViewModels
{
    using System.Windows.Input;

    /// <summary>
    /// Default subviewmodel
    /// </summary>
    public interface ISubViewModel : IViewModel
    {
        /// <summary>
        /// Implement and create logic for closing current subviewmodel
        /// </summary>
        public ICommand CancelCommand { get; }
    }
}
