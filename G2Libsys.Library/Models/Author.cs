namespace G2Libsys.Library
{
	using System;

	public class Author
	{
		public int ID { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }

		//Todo maybe make it into type URI or something similar
		public string ImageSRC { get; set; }
	}
}