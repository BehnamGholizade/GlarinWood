using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlarinWood.Models;
using GlarinWood.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using GlarinWood.Models.CheckoutViewModel;
using Microsoft.AspNetCore.Identity;

namespace GlarinWood.Services
{
    public class ShoppingCart : IShoppingCart
    {
           private readonly ApplicationDbContext _context;
        private readonly string _cartId;
        private readonly IHttpContextAccessor _contextAccessor;
  
        public ShoppingCart()
        {
            new ShoppingCart(_contextAccessor, _context);
        }

        public ShoppingCart(IHttpContextAccessor contextAccessor, ApplicationDbContext context)
        {
            _context = context;

            _contextAccessor = contextAccessor;

            _cartId = GetCartId(_contextAccessor);
        }

        //public ShoppingCart(IHttpContextAccessor contextAccessor)
        //{
        //    _contextAccessor = contextAccessor;
        //}
        //GetCartItems
        //public Task<List<CartItem>> GetCartItems()
        //{
        //    return _context
        //    .CartItems
        //    .Where(cart => cart.CartId == _cartId).ToList();
        //}

        public List<CartItem> GetCartItems()
        {
            //GetCartId(_contextAccessor)
           var ShoppingCartId = _cartId;

            return _context.CartItems.Where(
                c => c.CartId == ShoppingCartId).ToList();
        }
        

        //CheckOut
        public  object  CheckOut(CheckoutViewModel model)
        {

            var items = GetCartItems();
            
            var order = new Order()
            {

                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                Email = model.Email,
                Phone = model.Phone,
                Total = model.Total,
                UserId = model.UserId,
                OrderDate = DateTime.Now

            };

            foreach (var item in items)
            {
                var product = _context.Product.SingleOrDefault(p => p.Id == item.ProductId);

                var detail = new OrderDetail()
                {

                ProductId = item.ProductId,
                    UnitPrice = product.Price,
                    Quantity = item.Count
                };
                order.Total += (product.Price * item.Count);
                order.Qty += item.Count;
                order.OrderDetails.Add(detail);
            }
            model.Total = order.Total;
            // TODO : Authorize payment
            // TODO : Assign TransactionId
            _context.Order.Add(order);
            // if paymeny was successfull , order will saved in DB , otherwise it won't be saved .
 
             _context.SaveChanges();
            return null;
        }
        //getcart
        public string GetCart(IHttpContextAccessor contextAccessor)
        {

             var cartId = string.Empty;

          
                cartId = contextAccessor.HttpContext.Request.Cookies["ShoppingCart"];
            
            return cartId;
        }

        //GetCartId
        private string GetCartId(IHttpContextAccessor contextAccessor)
        {

            var cookie = contextAccessor.HttpContext.Request.Cookies["ShoppingCart"];
            var cartId = string.Empty;

            if (cookie == null || string.IsNullOrWhiteSpace(cookie))
            {
                cartId = Guid.NewGuid().ToString();

                contextAccessor.HttpContext.Response.Cookies.Append
                    (
              "ShoppingCart",
              cartId,
              new Microsoft.AspNetCore.Http.CookieOptions()
              {


                  Expires = DateTimeOffset.UtcNow.AddDays(7),
                  Path = "/",
                  HttpOnly = false,
                  Secure = false
              }
                    );
 
            }
            else
            {
                cartId = contextAccessor.HttpContext.Request.Cookies["ShoppingCart"];
            }
            return cartId;
        }

        //Add
        public void AddOne(int id)
        {
            var product = _context.Product.SingleOrDefault(pn => pn.Id == id);
            if (product == null)
            {
                // TODO : Exception
                return;
            }
            var cartItem = _context.CartItems
                .SingleOrDefault(c => c.CartId == _cartId && c.ProductId == id);
            //if (cartItem != null)
            //{
                cartItem.Count = cartItem.Count+1;
            //}
            //else
            //{
            //    cartItem = new Models.CartItem()
            //    {

            //        ProductId = id,
            //        //Product=product,
            //        CartId = _cartId,
            //        //اگر از قبل مقداری درج شده بود باید آن را با مقدار کنونی جمع کنیم
            //        Count = cartItem.Count,
            //        DateCreated = DateTime.Now,
            //    };
            //    _context.CartItems.Add(cartItem);
            //}
            _context.SaveChanges();
        }

        //[HttpPost]
        public void Add(int id,int quantity)
        {
            var product = _context.Product.SingleOrDefault(pn => pn.Id == id);
            if (product == null)
            {
                // TODO : Exception
                return;
            }
            var cartItem = _context.CartItems
                .SingleOrDefault(c => c.CartId == _cartId && c.ProductId == id);
            if (cartItem != null)
            {
                cartItem.Count = quantity + cartItem.Count;
            }
            else
            {
                cartItem = new Models.CartItem()
                {
                    
                    ProductId = id,
                    //Product=product,
                    CartId = _cartId,
                    //اگر از قبل مقداری درج شده بود باید آن را با مقدار کنونی جمع کنیم
                    Count = quantity,
                    DateCreated = DateTime.Now,
                };
                _context.CartItems.Add(cartItem);
            }
            _context.SaveChanges();
        }

        //remove
        public int Remove(int productId)
        {
            var cartItem = _context.CartItems
                .SingleOrDefault(c => c.CartId == _cartId && c.ProductId == productId);
            var ItemCount = 0;
            if (cartItem == null)
            {
                return ItemCount;
            }
            if (cartItem.Count > 1)
            {
                cartItem.Count--;
                ItemCount = cartItem.Count;
            }
            else
            {
                _context.CartItems.Remove(cartItem);
            }
            _context.SaveChanges();
            return ItemCount;
        }

    
  
    }
}
