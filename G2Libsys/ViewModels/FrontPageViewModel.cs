using G2Libsys.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace G2Libsys.ViewModels
{
    public class FrontPageViewModel : BaseViewModel
    {
        public virtual ICommand NavigateToVM { get; protected set; }

        public FrontPageViewModel()
        {
            NavigateToVM = new RelayCommand<Type>(vm =>
            {
                // Create new ViewModel
                MainWindowViewModel.HostScreen.CurrentViewModel = Activator.CreateInstance(vm);
            }/*, a => { return CurrentViewModel == null; }*/);
        }
    }
}
