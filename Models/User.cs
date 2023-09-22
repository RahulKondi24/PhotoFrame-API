using System;
using System.Collections.Generic;

#nullable disable

namespace RKdigitalsAPI.Models
{
    public partial class User
    {
        public User()
        {
            Carts = new HashSet<Cart>();
        }

        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public long? Mobilenumber { get; set; }
        public byte[] Passwordhash { get; set; }
        public byte[] Passwordkey { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }
    }
}
