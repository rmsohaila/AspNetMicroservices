using System.Collections.Generic;

namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public string Username { get; set; }
        public IList<ShoppingCartItem> Products { get; set; } = new List<ShoppingCartItem>();
        public decimal Total
        {
            get
            {
                decimal total = 0;
                foreach (var item in Products)
                    total += item.Price * item.Quantity;
                return total;
            }
        }

        public ShoppingCart()
        {

        }

        public ShoppingCart(string username)
        {
            Username = username;
        }
    }
}
