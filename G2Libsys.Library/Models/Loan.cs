using System;
using G2Libsys.Library.Models;

namespace G2Libsys.Library
{
    public class Loan
    {
        public int ID { get; set; }
        public LibraryObject LibraryObject { get; set; }
        public DateTime LoanDate { get; set; }
        public bool Returned { get; set; }
        public Card Card { get; set; }

    }
}
