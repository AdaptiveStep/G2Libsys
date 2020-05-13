using G2Libsys.Library.Models;

namespace G2Libsys.Library
{
    /// <summary>
    /// User model
    /// </summary>
    public class User
    {
        public int ID { get; set; }
        public decimal? LoanID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool LoggedIn { get; set; } = false;

        //TODO . WRONG TYPE. fix this
        public int UserType { get; set; } = 3;
    }
}
