using System.Collections.Generic;
using GlarinWood.Models;
using GlarinWood.Models.CheckoutViewModel;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GlarinWood.Services
{
    public interface IShoppingCart
    {
        void Add(int id, int quantity);
        object CheckOut(CheckoutViewModel model);
        string GetCart(IHttpContextAccessor _contextAccessor);
        List<CartItem> GetCartItems();
        int Remove(int productId);
    }
}