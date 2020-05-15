using G2Libsys.Services;
using G2Libsys.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace G2Libsys
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Application startup
        /// </summary>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceProvider = new ServiceCollection();

            serviceProvider.AddSingleton<INavigationService<IViewModel>, NavigationService>();

            serviceProvider.BuildServiceProvider();




            var app = new MainWindow { DataContext = new MainWindowViewModel() };
            app.Show();
        }
    }
}
