using System;

namespace G2Libsys.Library
{
    public class Card
    {
        public int ID { get; set; }
        public DateTime ActivationDate { get; set; }
        public bool Activated { get; set; }
        public User Owner { get; set; }
        
        /// <summary>
        /// Calculated in the database from Activationdate.
        /// </summary>
        public DateTime ValidUntil { get; set; }

        /// <summary>
        /// Calculated column from the database
        /// Has the form 4444-4444-4444-4444
        /// </summary>
        public int CardNumber { get; set; }
    }
}
