using System;
using System.Collections.Generic;
using System.Text;

namespace G2Libsys.Dialogs
{
    public class BaseDialogViewModel<T>
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public T DialogResult { get; set; }

        public BaseDialogViewModel() : this(null, null) { }

        public BaseDialogViewModel(string title = null, string message = null)
        {
            Title = title;
            Message = message;
        }

        public void CloseDialogWithResult(IDialogWindow dialog, T result)
        {
            DialogResult = result;

            if (dialog != null)
                dialog.DialogResult = true;
        }
    }
}
