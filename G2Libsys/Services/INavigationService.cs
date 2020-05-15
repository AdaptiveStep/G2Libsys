namespace G2Libsys.Services
{
    using G2Libsys.ViewModels;
    using System.Collections.Generic;

    public interface INavigationService<TViewModel>
    {
        /// <summary>
        /// Main viewmodel
        /// </summary>
        IHostScreen HostScreen { get; }

        /// <summary>
        /// Viewmodel navigation stack
        /// </summary>
        IList<TViewModel> ViewModels { get; }

        /// <summary>
        /// NavService setup where vm is HostScreen
        /// </summary>
        /// <param name="vm">HostScreen</param>
        void Setup(IHostScreen vm);

        /// <summary>
        /// Return viewmodel from stack or add new to stack
        /// </summary>
        /// <param name="vm">Viewmodel to navigate to</param>
        TViewModel GetViewModel(TViewModel vm);

        /// <summary>
        /// Set main viewmodel and clear stack
        /// </summary>
        /// <param name="vm">Viewmodel to navigate to</param>
        void GoToAndReset(TViewModel vm);

        /// <summary>
        /// Return new instance of the VM and remove previous from stack
        /// </summary>
        /// <param name="vm">Viewmodel to navigate to</param>
        TViewModel CreateNewInstance(TViewModel vm);
    }
}
