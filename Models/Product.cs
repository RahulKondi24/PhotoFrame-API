using System;
using System.Collections.Generic;

#nullable disable

namespace RKdigitalsAPI.Models
{
    public partial class Product
    {
        public Product()
        {
            Carts = new HashSet<Cart>();
        }

        public int Id { get; set; }
        public string Productname { get; set; }
        public string Productdescription { get; set; }
        public int? Productprice { get; set; }
        public string Productimage { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }
    }
}
