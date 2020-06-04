namespace G2Libsys.Models
{
    using G2Libsys.Commands;
    using G2Libsys.Services;
    using G2Libsys.ViewModels;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Item for navigation based on UserType
    /// </summary>
    public class UserMenuItem
    {
        /// <summary>
        /// Header title
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// MenuItem command
        /// </summary>
        public ICommand Action { get; }

        /// <summary>
        /// Constructor with input of typeof IViewModel, displayname Title, default title is viewmodel name
        /// </summary>
        /// <param name="viewModel">Typeof the attached IViewModel</param>
        /// <param name="title">Display title</param>
        /// <param name="action">Command</param>
        /// <param name="service">Navigation service</param>
        public UserMenuItem(Type viewModel, string title = null, ICommand action = null, INavigationService service = null)
        {
            var navigationService = service ?? IoC.ServiceProvider.GetService<INavigationService>();

            // Set Title to title or viewmodel name
            this.Title = title ?? viewModel.GetType().Name.Replace("ViewModel", null);

            // Create new command if action is null
            Action = action ?? new RelayCommand(_ =>
            {
                var newViewModel = navigationService.CreateNewInstance((IViewModel)Activator.CreateInstance(viewModel));

                if (newViewModel is ISubViewModel subViewModel)
                {
                    navigationService.HostScreen.SubViewModel = subViewModel;
                }
                else
                {
                    // Set CurrentViewModel to viewModel
                    navigationService.HostScreen.CurrentViewModel = newViewModel;
                }
            });
        }

        /// <summary>
        /// Constructor with custom command action input
        /// </summary>
        /// <param name="title">Display title</param>
        /// <param name="action">Command</param>
        public UserMenuItem(string title, ICommand action)
            : this(null, title, action) { }
    }
}
