using G2Libsys.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace G2Libsys.Data.Repository
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository() : base("users")
        {

        }
    }
}
