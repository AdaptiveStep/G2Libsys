using G2Libsys.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace G2Libsys.Services
{
    public static class NavService
    {
        public static IHostScreen HostScreen { get; private set; }

        public static void Setup(IHostScreen vm)
        {
            HostScreen = vm;
        }

        private static List<BaseViewModel> ViewModels { get; set; }

        public static BaseViewModel Locate(this Type vm)
        {
            if (ViewModels is null) ViewModels = new List<BaseViewModel>();

            return ViewModels.Where(x => x.GetType().Name == vm.Name).FirstOrDefault();
        }

        public static void GoToAndReset(BaseViewModel vm)
        {
            HostScreen.CurrentViewModel = vm;
            ViewModels.Clear();
        }

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
