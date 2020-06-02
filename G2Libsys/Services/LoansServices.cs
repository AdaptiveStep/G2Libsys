using G2Libsys.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
//Service too get the LoanCart between viewmodels
namespace G2Libsys.Services
{
    class LoansServices: ILoansService
    {
        public ObservableCollection<LibraryObject> LoanCart { get; private set; }
        public LoansServices()
        {
            LoanCart = new ObservableCollection<LibraryObject>();
        }
    }
}
