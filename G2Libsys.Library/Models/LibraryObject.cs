using System;

namespace G2Libsys.Library
{
    /// <summary>
    /// Library object model, ex. bok, film etc
    /// </summary>
    public class LibraryObject
    {
          public int ID { get; set; }
          public string Title { get; set; }
          public string Description { get; set; }
          public long ISBN { get; set; }
          public string Publisher { get; set; }
          public int DeweyDecimal { get; set; }
          public double? PurchasePrice { get; set; }
          public int? Category { get; set; }
          public DateTime? ActivityDate { get; set; }
          public int? Library { get; set; }
          public int AddedBy { get; set; }
          public DateTime LastEdited { get; set; }
          public DateTime DateAdded { get; set; }
          public string ImageSource { get; set; }
    }
}
