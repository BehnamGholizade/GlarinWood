using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace GlarinWood.Models
{
    public class OrderDetail
    {
        public OrderDetail()
        {

        }
        [Key]

        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        //public decimal Price { get; set; }


        public virtual Product Product { get; set; }

        public virtual Order Order { get; set; }

    }
}
