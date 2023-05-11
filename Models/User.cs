using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace C_.Models
{
    public class User
    {
        public int Id { get; set; }
        public bool isAdmin { get; set; } = false;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}