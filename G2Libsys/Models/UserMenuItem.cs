using G2Libsys.Commands;
using G2Libsys.Services;
using G2Libsys.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace G2Libsys.Models
{
    /// <summary>
    /// Item for navigation based on UserType
    /// </summary>
    public class UserMenuItem
    {
        private INavigationService navigationService;

        /// <summary>
        /// Header title
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// MenuItem command
        /// </summary>
        public ICommand Action { get; private set; }

        /// <summary>
        /// Input the viewmodel and displayname Title, default title is viewmodel name
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="title"></param>
        public UserMenuItem(IViewModel vm, string title = null, ICommand action = null)
        {
            navigationService = IoC.ServiceProvider.GetService<INavigationService>();

            // Set Title to title or viewmodel name
            this.Title = title ?? vm.GetType().Name.Replace("ViewModel", null);

            // Create new instance of vm in NavService
            var viewmodel = navigationService.CreateNewInstance(vm);

            Action = action ?? new RelayCommand(_ => 
            {
                navigationService.HostScreen.CurrentViewModel = viewmodel;
            });
        }
    }
}
