using G2Libsys.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace G2Libsys.Services
{
    /// <summary>
    /// Viewmodel navigation handler
    /// </summary>
    public static class NavService
    {
        /// <summary>
        /// Main viewmodel
        /// </summary>
        public static IHostScreen HostScreen { get; private set; }

        /// <summary>
        /// Dialog interraction handler
        /// </summary>
        public static IDialogService Dialog { get; set; }

        /// <summary>
        /// Viewmodel navigation stack
        /// </summary>
        private static List<IViewModel> ViewModels { get; set; }

        /// <summary>
        /// NavService setup where vm is HostScreen
        /// </summary>
        /// <param name="vm">HostScreen</param>
        public static void Setup(IHostScreen vm)
        {
            // Set hostscreen
            HostScreen = vm;

            // Initialize dialogservice
            Dialog = new DialogService();

            // Initialize stack
            ViewModels ??= new List<IViewModel>();
        }

        /// <summary>
        /// Look for viewmodel in navigationstack
        /// </summary>
        /// <param name="vm">Viewmodel Type</param>
        public static IViewModel Locate(this Type vm) => ViewModels?.Where(x => x.GetType().Name == vm.Name).FirstOrDefault();

        /// <summary>
        /// Return viewmodel from stack or add new to stack
        /// </summary>
        /// <param name="vm">Viewmodel to navigate to</param>
        public static IViewModel GetViewModel(IViewModel vm)
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
        public static void GoToAndReset(IViewModel vm)
        {
            ViewModels.Clear();
            ViewModels.Add(vm);
            HostScreen.CurrentViewModel = vm;
        }

        /// <summary>
        /// Return new instance of the VM and remove previous from stack
        /// </summary>
        /// <param name="vm">Viewmodel to navigate to</param>
        public static IViewModel CreateNewInstance(IViewModel vm)
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
