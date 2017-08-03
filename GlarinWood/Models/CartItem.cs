using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Models
{
    public class CartItem
    {
        public CartItem()
        {
            
        }
        [Key]

        public int CartItemId { get; set; }

        [Required]

        public string CartId { get; set; }

        public int ProductId { get; set; }

        public int Count { get; set; }

        [DataType(DataType.DateTime)]

        public DateTime DateCreated { get; set; }

        public virtual Product Product { get; set; }
        //public double Total => Count * Product.Price;
    }
}
