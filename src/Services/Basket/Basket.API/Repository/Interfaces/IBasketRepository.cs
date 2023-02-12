using Basket.API.Entities;
using System.Threading.Tasks;

namespace Basket.API.Repository.Interfaces
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasket(string username);
        Task<ShoppingCart> UpdateBasket(ShoppingCart cart);
        Task DeleteBasket(string username);
    }
}
