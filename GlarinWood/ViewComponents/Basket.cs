
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 using Microsoft.AspNetCore.Mvc;
using GlarinWood.Services;
using GlarinWood.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace GlarinWood.ViewComponents
{
     public class Basket : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public Basket(ApplicationDbContext context, IHttpContextAccessor httpcontext)

        {
            _contextAccessor = httpcontext;
             _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var Cart = new ShoppingCart(_contextAccessor, _context);

            //var applicationDbContext = Cart.GetCartItems().ToList();
            var applicationDbContext = _context.CartItems.Include(c => c.Product).Include(p => p.Product.Category).Where(current => current.CartId == Cart.GetCart(_contextAccessor)).ToList();
            var pc = applicationDbContext.Count;
            return View("Default", applicationDbContext);
        }
    }
}
