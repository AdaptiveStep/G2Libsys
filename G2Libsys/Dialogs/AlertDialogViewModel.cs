using G2Libsys.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using G2Libsys.Events;

namespace G2Libsys.Dialogs
{
    public class AlertDialogViewModel : BaseDialogViewModel<bool>
    {
        public AlertDialogViewModel(string title = null, string msg = null) 
            : base(title, msg) { }
    }
}
