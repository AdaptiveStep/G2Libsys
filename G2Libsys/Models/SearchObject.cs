namespace G2Libsys.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

	/// <summary>
	/// Library object model, ex. bok, film etc
	/// </summary>
	public class SearchObject
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public long? ISBN { get; set; }
		public string Publisher { get; set; }
		public int? Dewey { get; set; }
		public int? Category { get; set; }
		public string Author { get; set; }
		public int? AddedBy { get; set; }

		// initialize by looking 7 days back
		public DateTime? DateAdded { get; set; }

		//possible extras
		//public int? Library { get; set; }
		//public int? PurchasePrice { get; set; }
		//public int? Pages { get; set; }
		//public DateTime? LastEdited { get; set; }

	}
}
