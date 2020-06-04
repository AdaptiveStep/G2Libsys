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
		public int Quantity { get; set; }
		public int? Pages { get; set; }
        public string Author { get; set; }
        public int? Dewey { get; set; }
        public int Category { get; set; }
        public bool Disabled { get; set; }
        public string imagesrc { get; set; }
        public int? Library { get; set; }
        public int AddedBy { get; set; }
        public DateTime LastEdited { get; set; }
        public DateTime DateAdded { get; set; }
    }
}