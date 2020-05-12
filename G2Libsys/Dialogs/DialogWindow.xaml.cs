using System.Windows;

namespace G2Libsys.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window, IDialog
    {
        public DialogWindow()
        {
            InitializeComponent();

            var window = Application.Current.MainWindow;

            this.Owner = window;
            this.Height = window.ActualHeight;
            this.Width = window.ActualWidth;
        }
    }
}
