using G2Libsys.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace G2Libsys.Dialogs
{
    public class AlertDialogViewModel : BaseDialogViewModel<string>
    {
        public ICommand OKCommand => new RelayCommand<IDialogWindow>(OK);

        public AlertDialogViewModel(string title = null, string msg = null) 
            : base(title, msg) { }

        private void OK(IDialogWindow window)
        {
            base.CloseDialogWithResult(window, "ok");
        }
    }
}
