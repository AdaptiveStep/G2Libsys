namespace G2Libsys.Library
{
    public class DeweyDecimal
    {
        public int ID { get; set; }

		/// <summary>
		/// Numbers shorter than three digits will be assumed to begin with zeroes.
		/// </summary>
		public int DeweyINT { get; set; }

		/// <summary>
		/// Is calculated from the DeweyINT column in the database
		/// </summary>
		//public int DeweyDecimal { get; set; }


		//TODO N�gra �r inte helt korrekt �versatta. Kan vara v�rt att kolla upp?
		/// <summary>
		/// 100 gives "filosofi och psykologi". 
		/// 101 gives "Theory of philosophy" . etc..
		/// </summary>
		public string Description { get; set; }
    }
}
