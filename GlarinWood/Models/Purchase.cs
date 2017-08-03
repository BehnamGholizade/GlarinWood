using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Models
{
    public class Purchase
    {

        public int Id { get; set; }
        public System.DateTime BuyDate { get; set; }
        public string FlloweCode { get; set; }
        public string Description { get; set; }
        public Nullable<int> UserId { get; set; }
        public decimal Price { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
