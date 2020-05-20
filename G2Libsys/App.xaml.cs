using G2Libsys.ViewModels;
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

            // Initiate the inversion of control for serviceprovider
            IoC.SetUp();

            var app = new MainWindow { DataContext = new MainWindowViewModel() };
            app.Show();
        }
    }
}
