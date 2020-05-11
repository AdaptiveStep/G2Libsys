using System;
using System.Collections.Generic;
using System.Text;

namespace G2Libsys.Dialogs
{
    public interface IDialog
    {
        public bool? DialogResult { get; set; }
        public object DataContext { get; set; }

        bool? ShowDialog();
        void Close();
    }
}
