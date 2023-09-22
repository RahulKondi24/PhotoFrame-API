using System;
using System.Collections.Generic;

#nullable disable

namespace RKdigitalsAPI.Models
{
    public  class Cart
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? UserId { get; set; }
        public int? Quntity { get; set; }

        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}
