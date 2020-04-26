﻿using System;
using System.Collections.Generic;
using System.Text;

namespace G2Libsys.Library
{
    /// <summary>
    /// User model
    /// </summary>
    public class User
    {
        public int ID { get; set; }
        public int? Card { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Rights { get; set; } = 3; // Default 'Användare'
        public bool LoggedIn { get; set; } = false;
    }
}
