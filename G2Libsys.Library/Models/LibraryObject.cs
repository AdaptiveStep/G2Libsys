namespace G2Libsys.Library
{
    using System;

    /// <summary>
    /// Library object model, ex. bok, film etc
    /// </summary>
    public class LibraryObject
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long? ISBN { get; set; }
        public string Publisher { get; set; }
        public double? PurchasePrice { get; set; }
        public int? Pages { get; set; }
        public int? Dewey { get; set; }
        public int Category { get; set; }
        public int? Author { get; set; }
        public string imagesrc { get; set; }
        public int? Library { get; set; }
        public int AddedBy { get; set; }
        public DateTime LastEdited { get; set; }
        public DateTime DateAdded { get; set; }

        //public int ID { get; set; }
        //public string Title { get; set; }
        //public string Description { get; set; }
        //public long ISBN { get; set; }
        //public string Publisher { get; set; }
        //public DateTime? ActivityDate { get; set; }
        //public double? PurchasePrice { get; set; }          
        //public DateTime LastEdited { get; set; }
        //public DateTime DateAdded { get; set; }


        //public DeweyDecimal DeweyDecimal { get; set; }

        ////TODO
        ////WRONG TYPE. Must be Category type
        //public int? Category { get; set; }

        ////TODO
        ////WRONG TYPE, Must be Library type
        //public int? Library { get; set; }

        ////TODO
        ////WRONG TYPE. Must be USER
        //public int AddedBy { get; set; }

        ////TODO 
        ////Possibly Update to a URI type or similar.
        //public string ImageSource { get; set; }
    }
}
