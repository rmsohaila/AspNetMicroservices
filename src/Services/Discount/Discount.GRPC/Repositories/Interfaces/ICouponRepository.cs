using Discount.GRPC.Entites;
using System.Threading.Tasks;

namespace Discount.API.Repositories.Interfaces
{
    public interface ICouponRepository
    {
        Task<Coupon> GetDiscount(string productName);
        Task<bool> Create(Coupon coupon);
        Task<bool> Update(Coupon coupon);
        Task<bool> Delete(string productName);
    }
}
