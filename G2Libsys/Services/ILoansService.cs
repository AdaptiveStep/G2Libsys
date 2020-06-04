using G2Libsys.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace G2Libsys.Services
{
    interface ILoansService
    {
        ObservableCollection<LibraryObject> LoanCart { get; }
    }
}
