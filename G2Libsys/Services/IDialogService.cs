using System;
using System.Collections.Generic;
using System.Text;

namespace G2Libsys.Services
{
    public interface IDialogService
    {
        public void Alert(string title, string message);

        public bool? Confirm(string title, string message);
    }
}
