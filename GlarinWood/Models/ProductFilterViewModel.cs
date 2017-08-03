using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Models
{
    public class ProductFilterViewModel
    {
        
        public List<Product> products;
        public string ProductGener { get; set; }
        //public string  productFilter { get; set;
        public SelectList geners { get; set; }

    }
}
