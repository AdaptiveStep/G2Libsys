using System;
using System.Collections.Generic;
using System.Text;

namespace G2Libsys.Dialogs
{
    public class ConfirmDialogViewModel : BaseDialogViewModel<bool?>
    {
        public ConfirmDialogViewModel(string title = null, string msg = null)
            : base(title, msg) { }
    }
}
