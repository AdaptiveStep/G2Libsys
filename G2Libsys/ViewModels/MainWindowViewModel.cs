using G2Libsys.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace G2Libsys.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public static MainWindowViewModel HostScreen { get; set; }

        private object currentViewModel;

        public object CurrentViewModel
        {
            get => currentViewModel;
            set
            {
                currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public MainWindowViewModel()
        {

            //NavigateToVM = new RelayCommand<FrontPageViewModel>(vm =>
            //{
            //    // Create new ViewModel
            //    CurrentViewModel = Activator.CreateInstance(typeof(FrontPageViewModel), new object[] { vm });
            //});

            //CurrentViewModel = Activator.CreateInstance(typeof(FrontPageViewModel));            
            CurrentViewModel = new FrontPageViewModel();

            HostScreen = this;
        }
    }
}
