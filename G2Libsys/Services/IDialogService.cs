using System;
using System.Collections.Generic;
using System.Text;

namespace G2Libsys.Services
{
    public interface IDialogService
    {
        public string Alert(string title, string message);

        public T Confirm<T>(string title, string message);
    }
}
