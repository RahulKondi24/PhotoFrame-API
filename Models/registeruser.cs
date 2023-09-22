using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RKdigitalsAPI.Models
{
    public class registeruser
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public long? Mobilenumber { get; set; }
        public string Password { get; set; }
    }
}
