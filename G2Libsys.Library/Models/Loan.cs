using System;

namespace G2Libsys.Library
{
    public class Loan
    {
        public int ID { get; set; }
        public int ObjectID { get; set; }
        public DateTime LoanDate { get; set; }
        public bool Returned { get; set; }
        public int CardID { get; set; }

    }
}
