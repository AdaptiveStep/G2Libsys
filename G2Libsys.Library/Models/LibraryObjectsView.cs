using System;

namespace G2Libsys.Library.Models
{
    public class LibraryObjectsView
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public long? ISBN { get; set; }
        public string Category { get; set; }
        public string DeweyDecimal { get; set; }
        public string DeweyDescription { get; set; }
        public bool Disabled { get; set; }
        public double? PurchasePrice { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? LastEdited { get; set; }
    }
}
