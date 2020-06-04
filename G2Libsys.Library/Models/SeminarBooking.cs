using System;

namespace G2Libsys.Library
{
    public class SeminarBooking
    {
        public int ID { get; set; }
        public DateTime BookingDate { get; set; }
        public bool Attended { get; set; }
        public Seminar Seminar { get; set; }
        public User User { get; set; }
        
    }
}
