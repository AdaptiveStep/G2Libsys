namespace G2Libsys.Services
{
    #region Namespaces

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using G2Libsys.ViewModels;

    #endregion

    /// <summary>
    /// Viewmodel navigation handler
    /// </summary>
    public class NavigationService : INavigationService<IViewModel>
    {
        /// <summary>
        /// Main viewmodel
        /// </summary>
        public IHostScreen HostScreen { get; private set; }

        /// <summary>
        /// Viewmodel navigation stack
        /// </summary>
        public IList<IViewModel> ViewModels { get; private set; }

        /// <summary>
        /// NavService setup where vm is HostScreen
        /// </summary>
        /// <param name="vm">HostScreen</param>
        public void Setup(IHostScreen vm)
        {
            // Set hostscreen
            HostScreen = vm;

            // Initialize stack
            ViewModels ??= new List<IViewModel>();
        }

        /// <summary>
        /// Look for viewmodel in navigationstack
        /// </summary>
        /// <param name="vm">Viewmodel Type</param>
        public IViewModel Locate(Type vm) => ViewModels?.Where(x => x.GetType().Name == vm.Name).FirstOrDefault();

        /// <summary>
        /// Return viewmodel from stack or add new to stack
        /// </summary>
        /// <param name="vm">Viewmodel to navigate to</param>
        public IViewModel GetViewModel(IViewModel vm)
        {
            var viewModel = vm.GetType().Locate();

            if (viewModel != null)
            {
                return viewModel;
            }

            ViewModels.Add(vm);

            return vm;
        }

        /// <summary>
        /// Set main viewmodel and clear stack
        /// </summary>
        /// <param name="vm">Viewmodel to navigate to</param>
        public void GoToAndReset(IViewModel vm)
        {
            ViewModels.Clear();
            ViewModels.Add(vm);
            HostScreen.CurrentViewModel = vm;
        }

        /// <summary>
        /// Return new instance of the VM and remove previous from stack
        /// </summary>
        /// <param name="vm">Viewmodel to navigate to</param>
        public IViewModel CreateNewInstance(IViewModel vm)
        {
            var viewModel = vm.GetType().Locate();

            if (viewModel != null)
            {
                ViewModels.Remove(viewModel);
            }

            ViewModels.Add(vm);

            return vm;
        }
    }
}
