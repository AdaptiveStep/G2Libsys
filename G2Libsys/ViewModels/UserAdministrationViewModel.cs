namespace G2Libsys.ViewModels
{
    using G2Libsys.Commands;
    using G2Libsys.Library;
    using G2Libsys.Services;
    using System.Windows.Input;

    /// <summary>
    /// User details administration
    /// </summary>
    public class UserAdministrationViewModel : BaseViewModel, ISubViewModel
    {
        #region Fields
        private readonly IDialogService _dialog;
        private User activeUser;

        #endregion

        #region Properties

        /// <summary>
        /// Currently displayed User
        /// </summary>
        public User ActiveUser
        {
            get => activeUser;
            set
            {
                activeUser = value;
                OnPropertyChanged(nameof(ActiveUser));
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Close SubViewModel
        /// </summary>
        public ICommand CancelCommand => new RelayCommand(_ => NavService.HostScreen.SubViewModel = null);

        #endregion

        #region Constructor

        public UserAdministrationViewModel() { }

        public UserAdministrationViewModel(User user)
        {
            this.ActiveUser = user;

            _dialog = new DialogService();

            _dialog.Alert("Test", ActiveUser.Firstname);
        }

        #endregion
    }
}
