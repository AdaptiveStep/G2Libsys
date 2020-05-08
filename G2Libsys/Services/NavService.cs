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
        /// Viewmodel navigation stack
        /// </summary>
        private static List<BaseViewModel> ViewModels { get; set; }

        /// <summary>
        /// NavService setup where vm is HostScreen
        /// </summary>
        /// <param name="vm">HostScreen</param>
        public static void Setup(IHostScreen vm)
        {
            HostScreen = vm;
            if (ViewModels is null) ViewModels = new List<BaseViewModel>();
        }

        /// <summary>
        /// Look for viewmodel in navigationstack
        /// </summary>
        /// <param name="vm">Viewmodel Type</param>
        public static BaseViewModel Locate(this Type vm) => ViewModels != null ? 
            ViewModels.Where(x => x.GetType().Name == vm.Name).FirstOrDefault() : null;

        /// <summary>
        /// Set Hostscreen active viewmodel and reset stack
        /// </summary>
        /// <param name="vm">Viewmodel to navigate to</param>
        public static void GoToAndReset(BaseViewModel vm)
        {
            if (ViewModels is null) ViewModels = new List<BaseViewModel>();
            ViewModels.Clear();
            ViewModels.Add(vm);
            HostScreen.CurrentViewModel = vm;
        }

        /// <summary>
        /// Remove existing one from stack and add a new one
        /// </summary>
        /// <param name="vm">Viewmodel to navigate to</param>
        public static BaseViewModel GoToNewInstance(BaseViewModel vm)
        {
            var viewModel = vm.GetType().Locate();

            if (viewModel != null)
            {
                ViewModels.Remove(viewModel);
            }

            ViewModels.Add(vm);

            return vm;
        }

        /// <summary>
        /// Go to viewmodel in stack or add new to stack
        /// </summary>
        /// <param name="vm">Viewmodel to navigate to</param>
        public static BaseViewModel GoTo(BaseViewModel vm)
        {
            var viewModel = vm.GetType().Locate();

            if (viewModel != null)
            {
                return viewModel;
            }

            ViewModels.Add(vm);

            return vm;
        }
    }
}
