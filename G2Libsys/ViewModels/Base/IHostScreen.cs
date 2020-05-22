namespace G2Libsys.ViewModels
{
    using G2Libsys.Library;

    /// <summary>
    /// Implements functionality for the application foundation viewmodel
    /// </summary>
    public interface IHostScreen
    {
        /// <summary>
        /// Current active viewmodel
        /// </summary>
        public IViewModel CurrentViewModel { get; set; }

        /// <summary>
        /// Current subviewmodel if active
        /// </summary>
        public ISubViewModel SubViewModel { get; set; }

        /// <summary>
        /// The user currently using the application
        /// </summary>
        public User CurrentUser { get; set; }
    }
}
