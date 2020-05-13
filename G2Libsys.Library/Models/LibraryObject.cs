﻿using System;

namespace G2Libsys.Library.Models
{
    public class LibraryObject
    {
     /// <summary>
     /// Library object model, ex. bok, film etc
     /// </summary>
          public int ID { get; set; }
          public string Title { get; set; }
          public string Description { get; set; }
          public long ISBN { get; set; }
          public string Publisher { get; set; }
          public DateTime? ActivityDate { get; set; }
          public double? PurchasePrice { get; set; }          
          public DateTime LastEdited { get; set; }
          public DateTime DateAdded { get; set; }




          //TODO
          //WRONG TYPE. Must be DeweyDecimal Type
          public int DeweyDecimal { get; set; }
         
          //TODO
          //WRONG TYPE. Must be Category type
          public int? Category { get; set; }

          //TODO
          //WRONG TYPE, Must be Library type
          public int? Library { get; set; }

          //TODO
          //WRONG TYPE. Must be USER
          public int AddedBy { get; set; }

          //TODO 
          //Possibly Update to a URI type or similar.
          public string ImageSource { get; set; }



    }
}
