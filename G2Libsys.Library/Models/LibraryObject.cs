using System;
using System.Collections.Generic;
using System.Text;

namespace G2Libsys.Library.Models
{
    public class LibraryObject
    {
        public int ID { get; set; }
        public int CategoryID { get; set; }

        public LibraryObject(int iD, int categoryID)
        {
            ID = iD;
            CategoryID = categoryID;
        }
    }
}
