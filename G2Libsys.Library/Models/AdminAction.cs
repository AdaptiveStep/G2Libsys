using System;
using System.Collections.Generic;
using System.Text;

namespace G2Libsys.Library.Models
{
    public class AdminAction
    {
        public int ID { get; set; }

        public int ActionType { get; set; }

        public string Comment { get; set; }

        public DateTime Actiondate { get; set; }

    }
}
